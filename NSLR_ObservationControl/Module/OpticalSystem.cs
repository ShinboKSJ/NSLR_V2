using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NSLR_ObservationControl.ControlCommand;
using static NSLR_ObservationControl.Module.Observation_TMS;
//using static System.Net.Mime.MediaTypeNames;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace NSLR_ObservationControl.Module
{
    public partial class OpticalSystem : UserControl
    {
        Form1 form1;

        public byte[] controlID = new byte[4];
       // public UInt32 datalength = 124;  //117
        public UInt32 datalength = 140;  //117
        public UInt32 Bitdatalength = 4;
        UInt32 seqcount = 0;
        byte[] recvSeqcount = new byte[4];
        byte[] recvDataLength = new byte[4];

        //TX
        public byte OPSMode = 0x00;
        public byte coverControl = 0x00;
        public double TxState = 0.0f;
        public double TyState = 0.0f;
        public double TzState = 0.0f;
        public double RxState = 0.0f;
        public double RyState = 0.0f;
        public double RzState = 0.0f;
        public double TxpivotState = 0.00;
        public double TypivotState = 0.00;
        public double TzpivotState = 0.00;
        public double RxpivotState = 0.00;
        public double RypivotState = 0.00;
        public double RzpivotState = 0.00;
        public double MountAzimuth = 0.00;
        public double MountElevation = 0.00;
        public byte fanControl = 0x00;
        public byte fanMode = 0x00;
        public byte Cbit = 0x00;

        //RX
        public byte recvOPSMode;
        public byte recvcoverControl;
        public double recvTxState;
        public double recvTyState;
        public double recvTzState;
        public double recvRxState;
        public double recvRyState;
        public double recvRzState;
        public double recvTxpivotState ;
        public double recvTypivotState ;
        public double recvTzpivotState ;
        public double recvRxpivotState ;
        public double recvRypivotState ;
        public double recvRzpivotState;
        public double recvMountAz;
        public double recvMountEl;

        public byte recvFanControlState;
        public byte recvFanModeState;
        public short Temp1 ;
        public short Temp8 ;
        public short TempM1 ;
        public short TempM2;
        public short TempA;
        public short TempB;
        public short TempC;
        public byte[] RecvPBit =new byte[1];
        public byte[] RecvIBit  =new byte[1];
        public byte[] RecvCBit = new byte[1];
        public short M7rotationStage;
        public short recvM7rotationStage;
        private short t1 ;
        private short t2 ;
        private short t3 ;
        private short t4 ;
        private short t5 ;
        private short t6 ;
        private short t7;
        byte t8 = 0x00;
        byte t9 = 0x00;
        public OpticalSystem()
        {
            InitializeComponent();
            Init_value();
        }
        public void Init_value()
        {
            recvOPSMode = 0x00;
            recvcoverControl = 0x00;
            recvTxState = 0f;
            recvTyState = 0f;
            recvTzState = 0f;
            recvRxState = 0f;
            recvRyState = 0f;
            recvRzState = 0f;
            recvTxpivotState = 0;
            recvTypivotState = 0;
            recvTzpivotState = 0;
            recvRxpivotState = 0;
            recvRypivotState = 0;
            recvRzpivotState = 0;
            recvFanControlState = 0x00;
            recvFanModeState = 0x00;
            RecvCBit[0] = 0x00;
            RecvIBit[0] = 0x00;
            RecvPBit[0] = 0x00;
            Temp1 = 0;
            Temp8 = 0;
            TempM1 = 0;
            TempM2 = 0;
            TempA = 0;
            TempB = 0;
            TempC = 0;
            recvM7rotationStage = 0;
            t1 = 0;
            t2 = 0;
            t3 = 0;
            t4 = 0;
            t5 = 0;
            t6 = 0;
            t7 = 0;
            CollectData();
        }
        public OPSUserData CollectData()
        {
            OPSUserData userData = new OPSUserData
            {
                recvOPSMode = recvOPSMode,
                recvcoverControl = recvcoverControl,
                recvTxState = recvTxState,
                recvTyState = recvTyState,
                recvTzState = recvTzState,
                recvRxState = recvRxState,
                recvRyState = recvRyState,
                recvRzState = recvRzState,
                recvTxpivotState = recvTxpivotState,
                recvTypivotState = recvTypivotState,
                recvTzpivotState = recvTzpivotState,
                recvRxpivotState = recvRxpivotState,
                recvRypivotState = recvRypivotState,
                recvRzpivotState = recvRzpivotState,
                recvFanControlState = recvFanControlState,
                recvFanModeState = recvFanModeState,
                Temp1 = Temp1,
                Temp8 = Temp8,
                TempM1 = TempM1,
                TempM2 = TempM2,
                TempA = TempA,
                TempB = TempB,
                TempC = TempC,
                RecvPBit = RecvPBit,
                RecvIBit = RecvIBit,
                RecvCBit = RecvCBit,
                recvM7rotationStage = recvM7rotationStage
            };
            return userData;
        }
        private void OPS_Controlcommand(object sender, EventArgs e)
        {
            //NetworkStream clientStream = form1.currentClient.GetStream();
            //controlID = ControlCommand.OPSCONTROL;

            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1:
                    OPSMode = 0x01; //광학망원경 운용모드 초기/종료 상태
                    break;
                case 2:
                    OPSMode = 0x02; //광학망원경 운용모드 준비상태
                    break;
                case 3:
                    OPSMode = 0x03; //광학망원경 운용모드 운용상태
                    break;
                case 4:
                    OPSMode = 0x04; //광학망원경 운용모드 점검상태
                    break;
                case 5:
                    OPSMode = 0x05; //광학망원경 운용모드 안전상태
                    break;
                case 6:
                    coverControl = 0x01; //보호덮개 제어 Close
                    break;
                case 7:
                    coverControl = 0x02; //보호덮개 제어 Open
                    break;
                case 8:
                    fanControl = 0x01; //Fan 제어 Off
                    break;
                case 9:
                    fanControl = 0x02; //Fan 제어 On
                    break;
                case 10:
                    fanMode = 0x01; //Fan 모드 Mode1(2500rpm,상)
                    break;
                case 11:
                    fanMode = 0x02; //Fan 모드 Mode2(1250rpm,중)
                    break;
                case 12:
                    fanMode = 0x03; //Fan 모드 Mode3(620rpm,하)
                    break;
                case 13:
                    M7rotationStage = 0x01; //M7rotationStage
                    break;
                case 14:
                    M7rotationStage = 0x02; // M7rotationStage
                    break;
                case 15:
                    M7rotationStage = 0x03; //M7rotationStage
                    break;
            }
        }
        public void SendPBITRequest()
        {
            NetworkStream clientStream = MainForm.OpticalsystemTCP.GetStream();
            // SOM.Length = 4, sizeof(UInt32) = 4, controlID.Length = 4, EOM.Length = 4

            byte[] data = new byte[SOM.Length + 4 + 4 + 4 + EOM.Length];
            int index = 0;

            byte[] reversedSOM = { 0x83, 0x78, 0x71, 0x70 };
            Array.Copy(reversedSOM, 0, data, index, reversedSOM.Length);
            index += reversedSOM.Length;

            byte[] controlIDReversed = ControlCommand.OPSPBIT;
            Array.Copy(controlIDReversed.Reverse().ToArray(), 0, data, index, controlIDReversed.Length);
            index += controlIDReversed.Length;

            UInt32 seqNumberReversed = DataEndianConverter.ReverseEndian(seqcount);
            byte[] seqNumberBytes = BitConverter.GetBytes(seqNumberReversed);
            Array.Copy(seqNumberBytes, 0, data, index, seqNumberBytes.Length);
            index += seqNumberBytes.Length;

            byte[] dataByteCountReversed = DataEndianConverter.ReverseBytes(BitConverter.GetBytes(Bitdatalength));
            Array.Copy(dataByteCountReversed, 0, data, index, dataByteCountReversed.Length);
            index += dataByteCountReversed.Length;

            // Add Payload Data Fields (value)

            byte[] reversedEOM = { 0x70, 0x71, 0x78, 0x83 };
            Array.Copy(reversedEOM, 0, data, index, reversedEOM.Length);
            index += reversedEOM.Length;
            clientStream.Write(data, 0, data.Length);
            clientStream.Flush();
        }
        public void SendIBITRequest()
        {
            NetworkStream clientStream = MainForm.OpticalsystemTCP.GetStream();
            // SOM.Length = 4, sizeof(UInt32) = 4, controlID.Length = 4, EOM.Length = 4

            byte[] data = new byte[SOM.Length + 4 + 4 + 4 + EOM.Length];
            int index = 0;

            byte[] reversedSOM = { 0x83, 0x78, 0x71, 0x70 };
            Array.Copy(reversedSOM, 0, data, index, reversedSOM.Length);
            index += reversedSOM.Length;

            byte[] controlIDReversed = ControlCommand.OPSIBIT;
            Array.Copy(controlIDReversed.Reverse().ToArray(), 0, data, index, controlIDReversed.Length);
            index += controlIDReversed.Length;

            UInt32 seqNumberReversed = DataEndianConverter.ReverseEndian(seqcount);
            byte[] seqNumberBytes = BitConverter.GetBytes(seqNumberReversed);
            Array.Copy(seqNumberBytes, 0, data, index, seqNumberBytes.Length);
            index += seqNumberBytes.Length;

            byte[] dataByteCountReversed = DataEndianConverter.ReverseBytes(BitConverter.GetBytes(Bitdatalength));
            Array.Copy(dataByteCountReversed, 0, data, index, dataByteCountReversed.Length);
            index += dataByteCountReversed.Length;

            // Add Payload Data Fields (value)

            byte[] reversedEOM = { 0x70, 0x71, 0x78, 0x83 };
            Array.Copy(reversedEOM, 0, data, index, reversedEOM.Length);
            index += reversedEOM.Length;
            clientStream.Write(data, 0, data.Length);
            clientStream.Flush();
        }
        public void OPS_Period20ms(byte[] controlID)
        {
            NetworkStream clientStream = MainForm.OpticalsystemTCP.GetStream();
            // SOM.Length = 4, sizeof(UInt32) = 4, controlID.Length = 4, EOM.Length = 4

            byte[] data = new byte[SOM.Length + 4 + 4 + 4 + datalength + EOM.Length];
            byte[] value = new byte[datalength];
            int index = 0;

            byte[] reversedSOM = ControlCommand.SOM.Reverse().ToArray();  // { 0x83, 0x78, 0x71, 0x70 };
            Array.Copy(reversedSOM, 0, data, index, reversedSOM.Length);
            index += reversedSOM.Length;

            byte[] ControlID = controlID.Reverse().ToArray();
            Array.Copy(ControlID, 0, data, index, ControlID.Length);
            index += ControlID.Length;

            UInt32 seqNumberReversed = DataEndianConverter.ReverseEndian(seqcount);
            byte[] seqNumberBytes = BitConverter.GetBytes(seqNumberReversed);
            Array.Copy(seqNumberBytes, 0, data, index, seqNumberBytes.Length);
            index += seqNumberBytes.Length;

           // byte[] dataByteCountReversed = DataEndianConverter.ReverseBytes(BitConverter.GetBytes(datalength));
            byte[] dataByteCountReversed = BitConverter.GetBytes(datalength).Reverse().ToArray();
            Array.Copy(dataByteCountReversed, 0, data, index, dataByteCountReversed.Length);
            index += dataByteCountReversed.Length;

            value = PayloadDataFields(datalength);
            Array.Copy(value, 0, data, index, value.Length);
            index += value.Length;

            byte[] reversedEOM = ControlCommand.EOM.Reverse().ToArray();  //{ 0x70, 0x71, 0x78, 0x83 };
            Array.Copy(reversedEOM, 0, data, index, reversedEOM.Length);
            index += reversedSOM.Length;

            clientStream.Write(data, 0, data.Length);
            clientStream.Flush();
        }
        private byte[] PayloadDataFields(UInt32 datalength)
        {
            byte[] data = new byte[datalength];
            int currentIndex = 0;

            data[currentIndex++] = OPSMode; // OPSMode (1바이트)
            data[currentIndex++] = coverControl; // coverControl (1바이트)
            byte[] dummy1 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Array.Copy(dummy1, 0, data, currentIndex, dummy1.Length);
            currentIndex += dummy1.Length;

            // TxState (double, 8 바이트)
            byte[] txStateBytes = BitConverter.GetBytes(TxState);
            Array.Copy(txStateBytes, 0, data, currentIndex, txStateBytes.Length);
            currentIndex += txStateBytes.Length;

            // TyState (double, 8 바이트)
            byte[] tyStateBytes = BitConverter.GetBytes(TyState);
            Array.Copy(tyStateBytes, 0, data, currentIndex, tyStateBytes.Length);
            currentIndex += tyStateBytes.Length;

            // TzState (double, 8 바이트)
            byte[] tzStateBytes = BitConverter.GetBytes(TzState);
            Array.Copy(tzStateBytes, 0, data, currentIndex, tzStateBytes.Length);
            currentIndex += tzStateBytes.Length;

            // RxState (double, 8 바이트)
            byte[] rxStateBytes = BitConverter.GetBytes(RxState);
            Array.Copy(rxStateBytes, 0, data, currentIndex, rxStateBytes.Length);
            currentIndex += rxStateBytes.Length;

            // RyState (double, 8 바이트)
            byte[] ryStateBytes = BitConverter.GetBytes(RyState);
            Array.Copy(ryStateBytes, 0, data, currentIndex, ryStateBytes.Length);
            currentIndex += ryStateBytes.Length;

            // RzState (double, 8 바이트)
            byte[] rzStateBytes = BitConverter.GetBytes(RzState);
            Array.Copy(rzStateBytes, 0, data, currentIndex, rzStateBytes.Length);
            currentIndex += rzStateBytes.Length;

            // TxpivotState (double, 8 바이트)
            byte[] txpivotStateBytes = BitConverter.GetBytes(TxpivotState);
            Array.Copy(txpivotStateBytes, 0, data, currentIndex, txpivotStateBytes.Length);
            currentIndex += txpivotStateBytes.Length;

            // TypivotState (double, 8 바이트)
            byte[] typivotStateBytes = BitConverter.GetBytes(TypivotState);
            Array.Copy(typivotStateBytes, 0, data, currentIndex, typivotStateBytes.Length);
            currentIndex += typivotStateBytes.Length;

            // TzpivotState (double, 8 바이트)
            byte[] tzpivotStateBytes = BitConverter.GetBytes(TzpivotState);
            Array.Copy(tzpivotStateBytes, 0, data, currentIndex, tzpivotStateBytes.Length);
            currentIndex += tzpivotStateBytes.Length;

            // RxpivotState (double, 8 바이트)
            byte[] rxpivotStateBytes = BitConverter.GetBytes(RxpivotState);
            Array.Copy(rxpivotStateBytes, 0, data, currentIndex, rxpivotStateBytes.Length);
            currentIndex += rxpivotStateBytes.Length;

            // RypivotState (double, 8 바이트)
            byte[] rypivotStateBytes = BitConverter.GetBytes(RypivotState);
            Array.Copy(rypivotStateBytes, 0, data, currentIndex, rypivotStateBytes.Length);
            currentIndex += rypivotStateBytes.Length;

            // RzpivotState (double, 8 바이트)
            byte[] rzpivotStateBytes = BitConverter.GetBytes(RzpivotState);
            Array.Copy(rzpivotStateBytes, 0, data, currentIndex, rzpivotStateBytes.Length);
            currentIndex += rzpivotStateBytes.Length;

            //250401
            // MountAzimuth (double, 8 바이트)
           // Console.WriteLine($"TMSAZ :{TMSAZPositon}");
            byte[] mountAzimuthBytes = BitConverter.GetBytes(TMSAZPositon);
            Array.Copy(mountAzimuthBytes, 0, data, currentIndex, mountAzimuthBytes.Length);
            currentIndex += mountAzimuthBytes.Length;

            // MountElevation (double, 8 바이트)
          //  Console.WriteLine($"TMSEL :{TMSELPosition}");
            byte[] mountElevationBytes = BitConverter.GetBytes(TMSELPosition);
            Array.Copy(mountElevationBytes, 0, data, currentIndex, mountElevationBytes.Length);
            currentIndex += mountElevationBytes.Length;

            byte[] M7rotationBytes = BitConverter.GetBytes(M7rotationStage);
            Array.Copy(M7rotationBytes, 0, data, currentIndex, M7rotationBytes.Length);
            currentIndex += M7rotationBytes.Length;




            data[currentIndex++] = fanControl; // fanControl(1바이트)
            data[currentIndex++] = fanMode; // fanMode (1바이트)
            byte[] t1Bytes = BitConverter.GetBytes(t1);
            Array.Copy(t1Bytes, 0, data, currentIndex, t1Bytes.Length);
            currentIndex += t1Bytes.Length;
            byte[] t2Bytes = BitConverter.GetBytes(t2);
            Array.Copy(t2Bytes, 0, data, currentIndex, t2Bytes.Length);
            currentIndex += t1Bytes.Length;
            byte[] t3Bytes = BitConverter.GetBytes(t3);
            Array.Copy(t3Bytes, 0, data, currentIndex, t3Bytes.Length);
            currentIndex += t3Bytes.Length;
            byte[] t4Bytes = BitConverter.GetBytes(t4);
            Array.Copy(t4Bytes, 0, data, currentIndex, t4Bytes.Length);
            currentIndex += t4Bytes.Length;
            byte[] t5Bytes = BitConverter.GetBytes(t5);
            Array.Copy(t5Bytes, 0, data, currentIndex, t5Bytes.Length);
            currentIndex += t5Bytes.Length;
            byte[] t6Bytes = BitConverter.GetBytes(t6);
            Array.Copy(t6Bytes, 0, data, currentIndex, t6Bytes.Length);
            currentIndex += t6Bytes.Length;
            byte[] t7Bytes = BitConverter.GetBytes(t7);
            Array.Copy(t7Bytes, 0, data, currentIndex, t7Bytes.Length);
            currentIndex += t7Bytes.Length;

            data[currentIndex++] = t8;
            data[currentIndex++] = t9;

            return data;
        }
        private void ProcessReceivedData(byte[] receivedData)
        {
            int SOMLength = 4;
            int EOMLength = 4;
            int IDLength = 4;
            if (receivedData.Length < SOMLength + EOMLength)
            {
                return;
            }

            byte[] receivedSOM = new byte[SOMLength];
            byte[] receivedEOM = new byte[EOMLength];
            byte[] receivedID = new byte[IDLength];

            Array.Copy(receivedData, 0, receivedSOM, 0, SOMLength);
            Array.Copy(receivedData, SOMLength, receivedID, 0, IDLength);
            Array.Copy(receivedData, receivedData.Length - EOMLength, receivedEOM, 0, EOMLength);
            Array.Reverse(receivedSOM);
            Array.Reverse(receivedEOM);
            Array.Reverse(receivedID);

            if (ByteArrayEquals(receivedSOM, SOM) &&
                ByteArrayEquals(receivedEOM, EOM) &&
                ByteArrayEquals(receivedID, OPSSTATEINFO))
            {
                using (MemoryStream stream = new MemoryStream(receivedData, SOMLength + IDLength, receivedData.Length - SOMLength - EOMLength - IDLength))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    recvSeqcount = DataEndianConverter.ReverseBytes(reader.ReadBytes(4));
                    recvDataLength = DataEndianConverter.ReverseBytes(reader.ReadBytes(4));
                    recvOPSMode = reader.ReadByte();
                    recvcoverControl = reader.ReadByte();
                    uint dummy1 = reader.ReadUInt32();
                    byte dummy2 = reader.ReadByte();
                    byte dummy3 = reader.ReadByte();
                    recvTxState = reader.ReadDouble();
                    recvTyState = reader.ReadDouble();
                    recvTzState = reader.ReadDouble();
                    recvRxState = reader.ReadDouble();
                    recvRyState = reader.ReadDouble();
                    recvRzState = reader.ReadDouble();
                    recvTxpivotState = reader.ReadDouble();
                    recvTypivotState = reader.ReadDouble();
                    recvTzpivotState = reader.ReadDouble();
                    recvRxpivotState = reader.ReadDouble();
                    recvRypivotState = reader.ReadDouble();
                    recvRzpivotState = reader.ReadDouble();
                    recvMountAz = reader.ReadDouble(); // 250401
                    recvMountEl = reader.ReadDouble();
                    recvM7rotationStage = reader.ReadInt16();
                    recvFanControlState = reader.ReadByte();
                    recvFanModeState = reader.ReadByte();
                    TempM1 = reader.ReadInt16();
                    TempM2 = reader.ReadInt16();
                    Temp1 = reader.ReadInt16();
                    Temp8 = reader.ReadInt16();
                    TempA = reader.ReadInt16();
                    TempB = reader.ReadInt16();
                    TempC = reader.ReadInt16();


                    string M7rotationStatus = (recvM7rotationStage == 0x01) ? "LGAO" : (recvM7rotationStage == 0x02) ? "NSLR532nm" : (recvM7rotationStage == 0x03) ? "NSLR1064nm" : "N/A";
                    string recvOPSModeStatus = (recvOPSMode == 0x01) ? "초기/종료 상태" : (recvOPSMode == 0x02) ? "준비상태" : (recvOPSMode == 0x03) ? "운용상태" : (recvOPSMode == 0x04) ? "점검상태" : (recvOPSMode == 0x05) ? "안전상태" : "N/A";
                    string recvcoverControlStatus = (recvcoverControl == 0x01) ? "Close" : (recvcoverControl == 0x02) ? "Open" : "N/A";
                    string recvFanControlStatus = (recvFanControlState == 0x01) ? "Off" : (recvFanControlState == 0x2) ? "On" : "N/A";
                    string recvFanModeStatus = (recvFanModeState == 0x01) ? "Mode1" : (recvFanModeState == 0x02) ? "Mode2" : (recvFanModeState == 0x03) ? "Mode3" : "N/A";
                    Console.WriteLine($"M7 상태 확인 : {M7rotationStatus}");
                    RecvCBit = reader.ReadBytes(1);
                    byte dummy4 = reader.ReadByte();
                }
            }
            else if (ByteArrayEquals(receivedSOM, SOM) &&
                     ByteArrayEquals(receivedEOM, EOM) &&
                     ByteArrayEquals(receivedID, OPSPBITRESULT))
            {
                using (MemoryStream stream = new MemoryStream(receivedData, SOMLength + IDLength, receivedData.Length - SOMLength - EOMLength - IDLength))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    recvSeqcount = DataEndianConverter.ReverseBytes(reader.ReadBytes(4));
                    recvDataLength = DataEndianConverter.ReverseBytes(reader.ReadBytes(4));
                    RecvPBit = reader.ReadBytes(1);
                    byte temp1 = reader.ReadByte();
                    byte temp2 = reader.ReadByte();
                    byte temp3 = reader.ReadByte();
                    Console.WriteLine($"RecvPBIt : {RecvPBit}");

                }
            }
            else if (ByteArrayEquals(receivedSOM, SOM) &&
                     ByteArrayEquals(receivedEOM, EOM) &&
                     ByteArrayEquals(receivedID, OPSIBITRESULT))
            {
                using (MemoryStream stream = new MemoryStream(receivedData, SOMLength + IDLength, receivedData.Length - SOMLength - EOMLength - IDLength))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    recvSeqcount = DataEndianConverter.ReverseBytes(reader.ReadBytes(4));
                    recvDataLength = DataEndianConverter.ReverseBytes(reader.ReadBytes(4));
                    RecvIBit = reader.ReadBytes(1);
                    byte temp1 = reader.ReadByte();
                    byte temp2 = reader.ReadByte();
                    byte temp3 = reader.ReadByte();
                }
            }
            else
            {
                return;
            }
        }
        private bool ByteArrayEquals(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }

            return true;
        }
        public async Task ReceiveDataFromClient()
        {
            NetworkStream clientStream = MainForm.OpticalsystemTCP.GetStream();
            byte[] buffer = new byte[256];
            /*            while (true)
                        {
                            try
                            {
                                int bytesRead = clientStream.Read(buffer, 0, buffer.Length);
                                if (bytesRead > 0)
                                {
                                    byte[] receivedData = new byte[bytesRead];
                                    Array.Copy(buffer, receivedData, bytesRead);
                                    ProcessReceivedData(receivedData);
                                }
                                Thread.Sleep(200);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                break;
                            }
                        }*/
            while (true)
            {
                try
                {
                    int bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        byte[] receivedData = new byte[bytesRead];
                        Array.Copy(buffer, receivedData, bytesRead);
                        ProcessReceivedData(receivedData);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }
        private void UpdateLabel(System.Windows.Forms.Label label, string newText)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = newText));
            }
            else
            {
                label.Text = newText;
            }
        }
        private void Up_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: //추적마운트 방위각
                    MountAzimuth += 0.008;
                    break;
                case 2: //추적마운트 고각
                    MountElevation += 0.008;
                    break;
                case 3: //Tx 
                    TxState += 0.1;
                    UpdateLabel(label68, TxState.ToString());
                    break;
                case 4: //Ty
                    TyState += 0.1;
                    UpdateLabel(label55, TyState.ToString());
                    break;
                case 5: //Tz 
                    TzState += 0.1;
                    UpdateLabel(label51, TzState.ToString());
                    break;
                case 6: //Rx
                    RxState += 2;
                    UpdateLabel(label47, RxState.ToString());
                    break;
                case 7: //Ry
                    RyState += 2;
                    UpdateLabel(label43, RyState.ToString());
                    break;
                case 8: //Rz
                    RzState += 2;
                    UpdateLabel(label38, RzState.ToString());
                    break;
                case 9: //Tx Pivot
                    TxpivotState += 0.1;
                    UpdateLabel(label26, TxpivotState.ToString());
                    break;
                case 10: //Ty Pivot
                    TypivotState += 0.1;
                    UpdateLabel(label22, TypivotState.ToString());
                    break;
                case 11: //Tz Pivot
                    TzpivotState += 0.1;
                    UpdateLabel(label18, TzpivotState.ToString());
                    break;
                case 12: //Rx Pivot
                    RxpivotState += 2;
                    UpdateLabel(label4, RxpivotState.ToString());
                    break;
                case 13: //Ry Pivot
                    RypivotState += 2;
                    UpdateLabel(label10, RypivotState.ToString());
                    break;
                case 14: //Rz Pivot
                    RzpivotState += 2;
                    UpdateLabel(label14, RzpivotState.ToString());
                    break;
            }
        }

        private void Down_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: //추적마운트 방위각
                    MountAzimuth -= 0.008;
                    break;
                case 2: //추적마운트 고각
                    MountElevation -= 0.008;
                    break;
                case 3: //Tx 
                    TxState -= 0.1;
                    UpdateLabel(label68, TxState.ToString());
                    break;
                case 4: //Ty
                    TyState -= 0.1;
                    UpdateLabel(label55, TyState.ToString());
                    break;
                case 5: //Tz 
                    TzState -= 0.1;
                    UpdateLabel(label51, TzState.ToString());
                    break;
                case 6: //Rx
                    RxState -= 2;
                    UpdateLabel(label47, RxState.ToString());
                    break;
                case 7: //Ry
                    RyState -= 2;
                    UpdateLabel(label43, RyState.ToString());
                    break;
                case 8: //Rz
                    RzState -= 2;
                    UpdateLabel(label38, RzState.ToString());
                    break;
                case 9: //Tx Pivot
                    TxpivotState -= 0.1;
                    UpdateLabel(label26, TxpivotState.ToString());
                    break;
                case 10: //Ty Pivot
                    TypivotState -= 0.1;
                    UpdateLabel(label22, TypivotState.ToString());
                    break;
                case 11: //Tz Pivot
                    TzpivotState -= 0.1;
                    UpdateLabel(label18, TzpivotState.ToString());
                    break;
                case 12: //Rx Pivot
                    RxpivotState -= 2;
                    UpdateLabel(label4, RxpivotState.ToString());
                    break;
                case 13: //Ry Pivot
                    RypivotState -= 2;
                    UpdateLabel(label10, RypivotState.ToString());
                    break;
                case 14: //Rz Pivot
                    RzpivotState -= 2;
                    UpdateLabel(label14, RzpivotState.ToString());
                    break;
            }
        }

        private void SendPBITRequest_Click(object sender, EventArgs e)
        {
            SendPBITRequest();
        }

        private void IBITRequest_Click(object sender, EventArgs e)
        {
            SendIBITRequest();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            OPS_Period20ms(ControlCommand.OPSCONTROL);
        }
    }
    public class DataEndianConverter
    {
        public static byte[] ReverseBytes(byte[] data)
        {
            Array.Reverse(data);
            return data;
        }

        public static ushort ReverseEndian(ushort value)
        {
            return (ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
        }

        public static short ReverseEndian(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }
        public static uint ReverseEndian(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static ulong ReverseEndian(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static int ReverseEndian(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static long ReverseEndian(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        public static float ReverseEndian(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }


        public static double ReverseEndian(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }


        public static void ReverseStreamEndian(BinaryReader reader, BinaryWriter writer)
        {
            byte[] buffer = new byte[4];
            int bytesRead;
            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                Array.Reverse(buffer, 0, bytesRead);
                writer.Write(buffer, 0, bytesRead);
            }
        }
    }
}
