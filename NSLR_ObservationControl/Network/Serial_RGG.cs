using log4net;
using log4net.Config;
using NSLR_ObservationControl.Util;
using System;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Network
{
    class Serial_RGG : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        Serial_RGG RGGserial;
        SerialPort serialPort = new SerialPort();

        public ulong seq_num_tx { get; set; }
        private string NAME { get; set; }
        public string PORT { get; set; }

        public delegate void OnPacketReceivedEventHandler(byte[] recvData);
        public event OnPacketReceivedEventHandler OnPacketReceivedEvent;

        public delegate void OnOpenEventHandler(string result);
        public event OnOpenEventHandler OnOpenEvent;

        //private ulong Counter_Received_Packets { get; set; }

        #region 생성자/소멸자
        public Serial_RGG(string name)
        {
            NAME = name;
        }

        public void OpenSerial(string port)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.PortName = port;
                   // serialPort.BaudRate = 115200;
                    serialPort.BaudRate = 230400;
                    serialPort.DataBits = 8;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Parity = Parity.None;
                    serialPort.Handshake = Handshake.None;
                    serialPort.ReadTimeout = 500;
                    serialPort.WriteTimeout = 500;
                    serialPort.RtsEnable = false;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serialPort.Open();
                }
                
                if (serialPort.IsOpen)
                { 
                    log.Info($"{NAME}_{PORT} Open [{serialPort.PortName}]");
                    OnOpenEvent?.Invoke($"{serialPort.PortName} Open OK"); 
                }
                else 
                {
                    log.Info($"{NAME}_{PORT} Open Fail [{serialPort.PortName}]");
                    OnOpenEvent?.Invoke($"{serialPort.PortName} Open NOK");
                    MessageBox.Show($"[{NAME} Serial Open Fail ...");
                }
            }            
            catch (UnauthorizedAccessException ex)
            {

                // 포트가 이미 열려 있거나 권한이 없는 경우
                //MessageBox.Show(new Form() { TopMost = true },  $"{ex.Message}\n다른 Uart 프로그램이 실행중인가요?", $"{ NAME} SerialData Open Fatal");
                //CustomMessageBox msgBox = new CustomMessageBox(lbl.Text);
                //msgBox.ShowDialog();

                // Show with one button
                CustomMessageBox.Show("Operation completed.", "Info");

                // Show with two buttons
                var result = CustomMessageBox.Show("Delete this item?", "Confirm", "Yes", "No");
                if (result == DialogResult.OK)
                {
                    // Yes clicked
                }
                else
                {
                    // No clicked
                }
            }
            catch (ArgumentException ex)
            {
                // 포트 이름 등 인자 오류
                Console.WriteLine("포트 이름이 잘못되었습니다: " + ex.Message);
            }
            catch (IOException ex)
            {
                // 하드웨어 연결 등 IO 오류
                var info =$"{NAME} {ex.Message} ";
                // Show with one button
                CustomMessageBox.Show(info, "Fatal" , "확인");
            }
            catch (InvalidOperationException ex)
            {
                // 이미 포트가 열려 있는 경우 등
                Console.WriteLine("포트가 이미 열려 있습니다: " + ex.Message);
            }
            catch (Exception ex)
            {
                // 기타 모든 예외
                Console.WriteLine("알 수 없는 오류: " + ex.Message);
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool bDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!bDisposed)
                return;

            if (disposing)
            {
                if (disposing)
                {
                 
                }
                if (serialPort != null)
                {
                    serialPort.Close();
                    serialPort.Dispose();
                    log.Debug($"연결 해제");
                }
            }
            bDisposed = true;
        }

        ~Serial_RGG()
        {
            Dispose(false);
        }
        #endregion

        #region Handler
        public string CRC(string sTmpMsg)
        {
            byte[] convertArr = new byte[(sTmpMsg.Length) / 2];
            for (int i = 0; i < convertArr.Length; i++)
            {
                convertArr[i] = Convert.ToByte(sTmpMsg.Substring(i * 2, 2), 16);
            }
            int checksum = 0;
            //Step1: Add byte values.
            foreach (byte value in convertArr)
            {
                checksum += value;
            }
            checksum = 256 - checksum;
            checksum &= 0xFF; // FFFFFF replace
            return checksum.ToString("X2");
        }
        public ulong SendData(string sendData)
        {
            //log.Debug($" 제어명령 [{sendData}]"); ///.Length / 2}) {sendData}");
            byte[] xbytes = new byte[sendData.Length / 2];
            for (int i = 0; i < xbytes.Length; i++)
            {
                xbytes[i] = Convert.ToByte(sendData.Substring(i * 2, 2), 16);
            }
            serialPort.Write(xbytes, 0, xbytes.Length);
            seq_num_tx++;
            log.Debug($"[ {NAME} TX  <----   ] [#{seq_num_tx}] ({xbytes.Length}) {BitConverter.ToString(xbytes).Replace("-", string.Empty)}");
             //log.Debug($"[ {NAME} TX  <----   ] [#{seq_num_tx}] ({xbytes.Length})");
            return (seq_num_tx);

        }


        public ulong SendData(byte[] sendData)
        {
            //log.Debug($" 제어명령 [{sendData}]"); ///.Length / 2}) {sendData}");
            serialPort.Write(sendData, 0, sendData.Length);
            seq_num_tx++;
            //log.Debug($"[ {NAME} TX  <----   ] [#{seq_num_tx}] ({xbytes.Length}) {BitConverter.ToString(xbytes).Replace("-", string.Empty)}");
            //log.Debug($"[ {NAME} TX  <----   ] [#{seq_num_tx}] ({xbytes.Length})");
            return (seq_num_tx);

        }

        /*
        const int BUFFER_SIZE = 60; //Jason 25.02.20 RGG packet data Length 늘어난 것에 맞춤
        byte[] packetBuffer = new byte[BUFFER_SIZE];
        int packetCount = 0;
        const byte STX = 0x7E;
        const byte ETX = 0xFE;

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;

            while (sp.BytesToRead > 0)
            {
                packetBuffer[packetCount++] = (byte)sp.ReadByte();
                //Console.WriteLine($"[Serial_RGG] packetBuffer {packetBuffer[packetCount]}");
                //if (packetBuffer[0].Equals(STX) && packetBuffer[packetCount - 1].Equals(ETX))
                if ( packetBuffer[0].Equals(STX) && packetBuffer[packetCount - 1].Equals(ETX) && (packetCount == packetBuffer[1]+8) )
                {
                    byte[] aa = new byte[packetCount];
                    Array.Copy(packetBuffer, 0, aa, 0, packetCount);
                    OnPacketReceivedEvent?.Invoke(aa);
                    //log.Info($"[ ---> {NAME} RX ] ([0]--[{packetCount-1}] PckCnt{packetCount} pckBuff[1]+8={packetBuffer[1] + 8} {packetCount}) [{aa.Length}] {BitConverter.ToString(packetBuffer).Replace(" -", string.Empty)}");
                    ClearRxState();
                }
                //Test
                
                //{
                //    //log.Error($"[ -xx-> {NAME} ] ({packetCount}) {BitConverter.ToString(packetBuffer).Replace("-", string.Empty)}");
                //}

                if (packetCount == BUFFER_SIZE) { ClearRxState(); }
            }
        }
        private void ClearRxState()
        {
            Array.Clear(packetBuffer, 0, packetBuffer.Length); // 버퍼 초기화
            packetCount = 0;
        }
        */
        const int BUFFER_SIZE = 1024;
        const byte STX = 0x7E;
        const byte ETX = 0xFE;

        byte[] ringBuffer = new byte[BUFFER_SIZE];
        int ringHead = 0, ringTail = 0;

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;
            int bytesToRead = sp.BytesToRead;

            byte[] readBuf = new byte[bytesToRead];
            sp.Read(readBuf, 0, bytesToRead);

            //Into the ringbuffer 
            for (int i = 0; i < bytesToRead; i++)
            {
                ringBuffer[ringTail] = readBuf[i];
                ringTail = (ringTail + 1) % BUFFER_SIZE;
                //Let's avoid buffer overflow
                if (ringTail == ringHead) { ringHead = (ringHead + 1) % BUFFER_SIZE; } //Waste the old
            }
            ParsePackets();//Slide Parsing 
        }
        private int GetUsedLength() { return (ringTail - ringHead + BUFFER_SIZE) % BUFFER_SIZE; }
        private int GetUsedLength(int from) { return (ringTail - from + BUFFER_SIZE) % BUFFER_SIZE; }
        private void ParsePackets()
        {
            while (GetUsedLength() >= 3) //STX + length + ETX = 3  minimum
            {
                int tempHead = ringHead;

                // STX 찾기
                while (GetUsedLength(tempHead) > 0 && ringBuffer[tempHead] != STX)
                {
                    tempHead = (tempHead + 1) % BUFFER_SIZE;
                    ringHead = tempHead; //Let's ignore the uneffective data
                }
                if (GetUsedLength(tempHead) < 3) break; // STX 못 찾으면? 종료해야지 모

                int lenIndex = (tempHead + 1) % BUFFER_SIZE;
                byte dataLength = ringBuffer[lenIndex];
                int fullLength = dataLength + 8;

                if (GetUsedLength(tempHead) < fullLength) break; //전체길이 만큼 자랐을까

                int etxIndex = (tempHead + fullLength - 1) % BUFFER_SIZE;
                if (ringBuffer[etxIndex] == ETX)
                {
                    //Effective data
                    byte[] packet = new byte[fullLength];
                    for (int i = 0; i < fullLength; i++) { packet[i] = ringBuffer[(tempHead + i) % BUFFER_SIZE]; }

                    OnPacketReceivedEvent?.Invoke(packet);
                    //log.Info($"[ ---> {NAME} RX ] ([0]--[{fullLength - 1}] PckCnt{fullLength} {BitConverter.ToString(packet).Replace(" -", string.Empty)}");
                    ringHead = (tempHead + fullLength) % BUFFER_SIZE; // 다음 패킷으로 이동
                }
                else { ringHead = (tempHead + 1) % BUFFER_SIZE; } //What Can I do?  Without STX
            }
        }
        #endregion
    }
}
