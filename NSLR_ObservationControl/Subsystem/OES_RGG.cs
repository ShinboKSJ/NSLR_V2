using log4net;
using Newtonsoft.Json;
using NSLR_ObservationControl.Network;
using NSLR_ObservationControl.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Subsystem
{
    public class OES_RGG
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //public event EventHandler<string> connnectionInform;

        const string THIS = "[OES_RGG]";
        public ulong seq_num_rx { get; set; }
        public ulong seq_num_tx { get; set; }
        public bool connected { get; set; }

        string pLEN = "1A"; //26
        string pRGG_Control = "0001";
        static string pGatePulseWidth = "0000";
        static string pGatePulseStartOffset = "0000";
        static string pAvoidSetPositionStartOffset = "0000";
        static string pAvoidWidth = "0000";
        static string pLUT_UTC = "0000000000000000";
        static string pLUT_Delay = "0000000000000000";
        static string pMSG_ID = "01030001";
        static string pCHECKSUM = "";
        static string pPacket = "";

        string[] rggControlName = {
            "0x00 : N/A", "0x01 : 대기", "0x02 : LUT 송신",  "0x03 : Ground  Calibration",
            "0x04 : Range", "0x05 : Overlap Avoidance", "0x06 : 안전상태 진입" };

        
        Serial_RGG rggSerial;
        IniUtil ini;   // 만들

        public OES_RGG()
        {
            if (rggSerial != null)
            {
                rggSerial.Dispose();
            }
            rggSerial = new Serial_RGG();
            rggSerial.OnPacketReceivedEvent += OnPacketReceivedEvent;
            rggSerial.OnConnectedEvent += ChangeConnected;
            rggSerial.OpenSerial("COM7");
            log.Info($"{THIS} 생성자..Open? {connected}");
        }


        private void ChangeConnected(bool bCon)
        {
            if (bCon)
            {
                //connection_indicator.Image = Properties.Resources.icons8_connected100;
                connected = true;
                //connnectionInform?.Invoke(this, "on");   
                log.Info($"{THIS} Connected");
            }
            else
            {
                //connection_indicator.Image = Properties.Resources.icons8_disconnected100;
                connected = false;
                //connnectionInform?.Invoke(this, "off");
                log.Info($"{THIS} Disconnected");
            }
        }

        private void OnPacketReceivedEvent(byte[] recvData)
        {
            const string CTRL = "01030101";
            const string PBIT = "01030102";
            const string IBIT = "01030103";
            const string CBIT = "01030104";
            string strData;
            string whatBIT;

            log.Info($"{THIS} [RX] {BitConverter.ToString(recvData).Replace("-", string.Empty)}");
            //MessageBox.Show(BitConverter.ToString(recvData).Replace("-", string.Empty));

            //Data 전달 to 운영제어PC 
            //((BitConverter.ToString(recvData)).Replace("-", string.Empty));

            var strPacket = (BitConverter.ToString(recvData)).Replace("-", string.Empty);
            var strDataLen = strPacket.Substring(2, 2);  //DataLength  1Byt
            var DataLen = Convert.ToInt32(strDataLen, 16);
            var strMSGID = strPacket.Substring(4, 8);  //ID 4Byte            

            StringBuilder stringBuilder = new StringBuilder();

            string[] searchStrings = { PBIT, IBIT, CBIT };
            foreach (string searchString in searchStrings)
            {
                if (strMSGID.Contains(searchString))
                {
                    strData = strPacket.Substring(12, 2); //Data NB 
                    var setting = int.Parse(strData);
                    //2023.11 By Little Endian 
                    //cb_DDR.Checked = (setting & 1) != 0;
                    //cb_FPGAclk.Checked = (setting & 2) != 0;
                    //cb_GPScom.Checked = (setting & 4) != 0;
                    /*
                    if (strMSGID.Equals(PBIT)) whatBIT = "OES_PBIT";
                    else if (strMSGID.Equals(IBIT)) whatBIT = "OES_IBIT";
                    else if (strMSGID.Equals(CBIT)) whatBIT = "OES_CBIT";
                    else whatBIT = "";

                    var whichBIT = new MONITOR_RGG_BIT
                    {
                        ID = whatBIT,
                        BIT_Data = 111,
                    };
                    string jsonData = JsonConvert.SerializeObject(whichBIT);
                    log.Info(jsonData);
                    */

                }
            }

            if (strMSGID.Contains(CTRL))
            {
                strData = strPacket.Substring(12, DataLen * 2); //Data NB     

                var whatCTRL = new MONITOR_RGG_DATA
                {
                    ID = "OES_CONTROL",
                    RGGctrl = strData.Substring(0, 4),
                    GatePulseWidth = strData.Substring(4, 4),
                    GatePulseStartOffset = strData.Substring(8, 4),
                    AvoidSetPositionStartOffset = strData.Substring(12, 4),
                    AvoidWidth = strData.Substring(16, 4),
                    LookupTableUTC = strData.Substring(20, 8),
                    LookupTableDelay = strData.Substring(28, 8),
                };
                string jsonData = JsonConvert.SerializeObject(whatCTRL);
                log.Info(jsonData);
            }

            //seq_num_rx++;
            //label_rcvdPacket_counter.Text = seq_num_rx.ToString();                            
        }

        ///
        public string Checksum(string sTmpMsg)
        {
            //sTmpMsg = pLEN + pMSG_ID + pDATA;            
            byte[] convertArr = new byte[(sTmpMsg.Length) / 2];
            for (int i = 0; i < convertArr.Length; i++)
            {
                convertArr[i] = Convert.ToByte(sTmpMsg.Substring(i * 2, 2), 16);
            }
            int checksum = 0;
            //Step1: Exclusive OR values.            
            foreach (byte value in convertArr)
            {
                Debug.WriteLine($"EXOR : {checksum} ^ {value.ToString("X")} = {checksum ^ value}");
                checksum ^= value;
            }
            //-checksum = 256 - checksum;
            checksum &= 0xFF; // FFFFFF replace
            return checksum.ToString("X2");
        }


        public void rgg_set_ctrl_command(string pData)
        {
            if (connected)
            {
                pLEN = "1A";
                pMSG_ID = ControlCommand.MSG_ID_RGG_OP_CMD;
                pCHECKSUM = Checksum(pLEN + pMSG_ID + pData );
                pPacket = ControlCommand.MSG_SOF + pLEN + pMSG_ID + pData + pCHECKSUM + ControlCommand.MSG_EOT;
                seq_num_tx = rggSerial.SendData(pPacket);
                Debug.WriteLine($"[OES_RGG] SetCommand  {pPacket}");
                //else { periodicSend_timer.Enabled = false; }
            }
            else
            {
                MessageBox.Show("Serial Port를 먼저 Open해 주세요");
            }
        }

        public void rgg_set_bit_command(int cmd)
        {
            if (connected)
            {
                pLEN = "00";
                if (cmd == 2) { pMSG_ID = ControlCommand.MSG_ID_RGG_PBIT_CMD;  }
                else if (cmd == 3) { pMSG_ID = ControlCommand.MSG_ID_RGG_IBIT_CMD; }
                else if (cmd == 4) { pMSG_ID = ControlCommand.MSG_ID_RGG_CBIT_CMD; }
                pCHECKSUM = Checksum(pLEN + pMSG_ID );
                pPacket = ControlCommand.MSG_SOF + pLEN + pMSG_ID + pCHECKSUM + ControlCommand.MSG_EOT;
                //label_PacketData.Text = pPacket;
                //Log(LOG.D, THIS, $"Packet: [{label_PacketData.Text}]");
                seq_num_tx = rggSerial.SendData(pPacket);
                //label_sendPacket_counter.Text = seq_num_tx.ToString();
                //if (tranmissionReat_check.Checked) { periodicSend_timer.Enabled = true; }
                //else { periodicSend_timer.Enabled = false; }
            }
            else
            {
                MessageBox.Show("Serial Port를 먼저 Open해 주세요");
            }
        }

    }

    class MONITOR_RGG_DATA
    {
        public string ID { get; set; }
        public string RGGctrl { get; set; }
        public string GatePulseWidth { get; set; }
        public string GatePulseStartOffset { get; set; }
        public string AvoidSetPositionStartOffset { get; set; }
        public string AvoidWidth { get; set; }
        public string LookupTableUTC { get; set; }
        public string LookupTableDelay { get; set; }
    }

    class MONITOR_RGG_BIT
    {
        public string ID { get; set; }
        public bool BIT_DDR { get; set; }
        public bool BIT_FPG { get; set; }
        public bool BIT_GPS { get; set; }
    }
}
