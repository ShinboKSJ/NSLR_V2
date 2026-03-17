using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLR_ObservationControl
{
    internal class ControlCommand
    {
        //Start/End Of COMMON Message 
        public const string MSG_SOM = "70717883";
        public const string MSG_EOM = "83787170";
        public const string MSG_SEQNUMBER = "00000000";
        public const string MSG_DATA_BYTE_CNT = "00000004";

        //Start/End Of Serial message 
        public const string MSG_SOF = "7E";
        public const string MSG_EOT = "FE";

        // //////////////////////////////////////////////////
        // 레이저부
        // //////////////////////////////////////////////////
        // 운영제어부 --> 레이저부
        public const string MSG_ID_LASER_OP_CMD =   "01020001";
        public const string MSG_ID_LASER_PBIT_CMD = "01020002";
        public const string MSG_ID_LASER_IBIT_CMD = "01020003";        
        // 레이저부 --> 운영제어부
        public const string MSG_ID_LASER_OP_RSP =   "01020101";
        public const string MSG_ID_LASER_PBIT_RSP = "01020102";
        public const string MSG_ID_LASER_IBIT_RSP = "01020103";

        
        // //////////////////////////////////////////////////
        // 광전자부(RGG)
        // //////////////////////////////////////////////////
        // 운영제어부 --> 광전자부(RGG)
        public const string MSG_ID_RGG_OP_CMD =   "01030001";
        public const string MSG_ID_RGG_PBIT_CMD = "01030002";
        public const string MSG_ID_RGG_IBIT_CMD = "01030003";
        public const string MSG_ID_RGG_CBIT_CMD = "01030004";
        // 광전자부(RGG) -->  운영제어부 
        public const string MSG_ID_RGG_STAT_RSP = "01030101";
        public const string MSG_ID_RGG_PBIT_RSP = "01030102";
        public const string MSG_ID_RGG_IBIT_RSP = "01030103";
        public const string MSG_ID_RGG_CBIT_RSP = "01030104";

        // //////////////////////////////////////////////////
        // 항공기탐지(AID)
        // //////////////////////////////////////////////////
        // 운영제어부 --> 항공기탐지(AID)
        public const string MSG_ID_AID_OP_CMD =   "01050001";
        public const string MSG_ID_AID_PBIT_CMD = "01050002";
        public const string MSG_ID_AID_IBIT_CMD = "01050003";
        
        // 항공기탐지(AID) -->  운영제어부 
        public const string MSG_ID_AID_STAT_RSP = "01050101";
        public const string MSG_ID_AID_PBIT_RSP = "01050102";
        public const string MSG_ID_AID_IBIT_RSP = "01050103";
        

        // //////////////////////////////////////////////////
        //LGAO
        // //////////////////////////////////////////////////
        // 운영제어부 --> LGAO
        public const string MSG_ID_LGAO_CMD = "00010001";
        public const string MSG_ID_LGAO_1 =   "00010002";
        public const string MSG_ID_LGAO_2 =   "00010003";
        public const string MSG_ID_LGAO_3 =   "00010004";
        // LGAO --> 운영제어부
        public const string MSG_ID_LGAO_STA_RSP = "00010101";
        public const string MSG_ID_LGAO_1_RSP =   "00010102";
        public const string MSG_ID_LGAO_2_RSP =   "00010103";


        // //////////////////////////////////////////////////
        //LDAP
        // //////////////////////////////////////////////////
        // 운영제어부 --> LDAP
        public const string MSG_ID_LDAP_CMD =  "00020001";
        public const string MSG_ID_LDAP_1 =    "00020002";
        public const string MSG_ID_LDAP_2 =    "00020003";
        public const string MSG_ID_LDAP_3 =    "00020004";
        // LDAP --> 운영제어부 
        public const string MSG_ID_LDAP_STA_RSP = "00020101";
        public const string MSG_ID_LDAP_1_RSP =   "00020102";
        public const string MSG_ID_LDAP_2_RSP =   "00020103";
        

        // //////////////////////////////////////////////////
        //우주정보상황실
        // //////////////////////////////////////////////////
        public const string MSG_ID_COMMD_CMD = "00030101";
        public const string MSG_ID_COMMD_1 =   "00030102";
        public const string MSG_ID_COMMD_2 =   "00030103";


        // //////////////////////////////////////////////////
        // 추적마운트
        // //////////////////////////////////////////////////
        public static readonly byte[] MOUNT_SOM = { 0x70, 0x71, 0x78, 0x83 };
        public static readonly byte[] MOUNT_EOM = { 0x83, 0x78, 0x71, 0x70 };
        public static readonly byte[] MOUNT_SEQNUMBER = { 0x00, 0x00, 0x00, 0x00 };

        public static readonly byte[] MSG_ID_MOUNT_OP_CMD = { 0x01, 0x01, 0x00, 0x01 };
        public static readonly byte[] MSG_ID_MOUNT_PBIT_CMD = { 0x01, 0x01, 0x00, 0x02 };
        public static readonly byte[] MSG_ID_MOUNT_IBIT_CMD = { 0x01, 0x01, 0x00, 0x03 };
        public static readonly byte[] MSG_ID_MOUNT_OP_RSP = { 0x01, 0x01, 0x01, 0x01 };
        public static readonly byte[] MSG_ID_MOUNT_PBIT_RSP = { 0x01, 0x01, 0x01, 0x02 };
        public static readonly byte[] MSG_ID_MOUNT_IBIT_RSP = { 0x01, 0x01, 0x01, 0x03 };

        public static readonly byte[] MOUNT_OP_INIT = { 0x00, 0x01 };
        public static readonly byte[] MOUNT_OP_READY = { 0x00, 0x02 };
        public static readonly byte[] MOUNT_OP_OP = { 0x00, 0x03 };
        public static readonly byte[] MOUNT_OP_CHECK = { 0x00, 0x04 };
        public static readonly byte[] MOUNT_OP_SAFE = { 0x00, 0x05 };


        // //////////////////////////////////////////////////
        // 광학망원경 ICD
        // //////////////////////////////////////////////////
        public static readonly byte[] SOM = { 0x70, 0x71, 0x78, 0x83 };
        public static readonly byte[] EOM = { 0x83, 0x78, 0x71, 0x70 };
        public static readonly byte[] ReverseSOM = { 0x83, 0x78, 0x71, 0x70 };
        public static readonly byte[] ReverseEOM = { 0x70, 0x71, 0x78, 0x83 };
        public uint seqNumber = 0;
        public uint databyteCount;

        //NSLR ===> OpticalSystem
        public static readonly byte[] OPSCONTROL = { 0x01, 0x04, 0x00, 0x01 };
        public static readonly byte[] OPSPBIT = { 0x01, 0x04, 0x00, 0x02 };
        public static readonly byte[] OPSIBIT = { 0x01, 0x04, 0x00, 0x03 };
        //OpticalSystem ===> NSLR
        public static readonly byte[] OPSSTATEINFO = { 0x01, 0x04, 0x01, 0x01 };
        public static readonly byte[] ReverseOPSSTATEINFO = { 0x01, 0x01, 0x04, 0x01 };
        public static readonly byte[] OPSPBITRESULT = { 0x01, 0x04, 0x01, 0x02 };
        public static readonly byte[] ReverseOPSPBITRESULT = { 0x01, 0x04, 0x01, 0x02 };
        public static readonly byte[] OPSIBITRESULT = { 0x01, 0x04, 0x01, 0x03 };        
        public static readonly byte[] ReverseOPSIBITRESULT = { 0x01, 0x04, 0x01, 0x03 };        
    }
}
