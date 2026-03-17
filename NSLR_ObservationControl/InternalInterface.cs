using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLR_ObservationControl
{
    #region 추적마운트
    public class TMSUserData
    {

        public ushort TMSoperatingMode { get; set; }
        public ushort TMScontrolCommand { get; set; }
        public double TMSPacketTime { get; set; }
        public double TMSAZPosition { get; set; }
        public double TMSELPosition { get; set; }
        public double TMSUsePacketTime { get; set; }
        public byte[] TMSCBIT { get; set; }
        public byte[] TMSPBIT { get; set; }
        public byte[] TMSIBIT { get; set; }
    }
    #endregion
    #region 레이저부
    public class LASUserData
    {
        public string ID;
        public string LASopMode;
        public string LaserMode;
        public string LaserFireState;
        public string LASopEnd;
        public byte BITResult1;
        public byte BITResult2;
    }
    #endregion

    #region 광전자부(RGG)
    public class RGGUserData
    {
        public string ID;
        public string RggControl;
        public string GatePulseWidth;
        public string GatePulseSO;
        public string RcvdLUTSize;
        public string UtcNow;
        public string TofNow;
        public string BIT;
        public string BITResult;
    }
    public class RGGUserData2
    {
        public string ID;
        public string UtcNow;
        public string TofNow;
    }
    #endregion

    #region 가이드 카메라(EMCCD)
    #endregion
    #region 가이드 카메라(MWIR)
    #endregion
    #region 스타카메라
    #endregion

    #region 항공기 감지
    public class AIDUserData
    {
        public string ID;
        public string AIDopMode;
        public string RadarSyncState;
        public string ADSbSyncState;
        public string BITResult;
    }
    #endregion


    #region 개폐돔
    public class DOMUserData
    {
        public string ID;
        public string Shutter;
        public string Rain;
        public string Home;
        public string Driving;
        public string Position;
        public string Bit;
        public string BitResult;
    }
    #endregion


    #region 광학망원경
    public class CombineData
    {
        public OPSUserData OpticalSystem { get; set; }
        public ConnUserData Connection { get; set; }
    }
    public class OPSUserData
    {
        public byte recvOPSMode { get; set; }
        public byte recvcoverControl { get; set; }
        public double recvTxState { get; set; }
        public double recvTyState { get; set; }
        public double recvTzState { get; set; }
        public double recvRxState { get; set; }
        public double recvRyState { get; set; }
        public double recvRzState { get; set; }
        public double recvTxpivotState { get; set; }
        public double recvTypivotState { get; set; }
        public double recvTzpivotState { get; set; }
        public double recvRxpivotState { get; set; }
        public double recvRypivotState { get; set; }
        public double recvRzpivotState { get; set; }
        public short recvM7rotationStage { get; set; }
        public byte recvFanControlState { get; set; }
        public byte recvFanModeState { get; set; }
        public short Temp1 { get; set; }
        public short Temp8 { get; set; }
        public short TempM1 { get; set; }
        public short TempM2 { get; set; }
        public short TempA { get; set; }
        public short TempB { get; set; }
        public short TempC { get; set; }
        public byte[] RecvPBit { get; set; }
        public byte[] RecvIBit { get; set; }
        public byte[] RecvCBit { get; set; }
    }
    #endregion
    #region 구성품 연결상태
    public class ConnUserData
    {
        public bool OpticalsystemConn { get; set; }
        public bool Operatingsystemconn { get; set; }
        public bool TMSConn { get; set; }
        public bool LAS1conn { get; set; }
        public bool LAS2conn { get; set; }
        public bool OEU1conn { get; set; }
        public bool OEU2conn { get; set; }
        public bool CDSconn { get; set; }
        public bool AIDconn {  get; set; }

    }
    #endregion

}
