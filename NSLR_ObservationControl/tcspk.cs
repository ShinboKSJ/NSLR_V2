using Emgu.CV.XImgproc;
using NSLR_ObservationControl.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static NSLR_ObservationControl.Module.Observation_TMS;
namespace NSLR_ObservationControl
{
    public class tcspk
    {
        /* slamac.h -------------------------------------------------------------------------*/

        /* slalib.h -------------------------------------------------------------------------*/

        /* tcssys.h -------------------------------------------------------------------------*/
        public const int MAXTRM = 100;       /* maximum number of terms in pointing model */
        public const int NTROOM = 200;       /* maximum number of terms in repertoire */
        public const int NPOS = 6;           /* number of pointing origins available */
        public const int MAXAUX = 99;        /* maximum number of auxiliary readings */

        /* tcsmac.h -------------------------------------------------------------------------*/
        /* Mathematical constants */
        public const double D90 = 1.57079632679489661923132;     /* Pi/2 */
        public const double PI = 3.14159265358979323846264;     /* Pi */
        public const double PI2 = 6.28318530717958647692529;     /* 2Pi */
        public const double D2R = 0.0174532925199432957692369;     /* degrees to radians */
        public const double S2R = 7.27220521664303990384871e-5;  /* seconds to radians */
        public const double AS2R = 4.84813681109535993589914e-6;  /* arcseconds to radians */
        public const double DAYSEC = 86400000.0;                       /* day length in seconds */

        /* System constants */
        public const double TINY = 1e-10;   /* a small number */
        public const double TINY2 = 1e-20;  /* a much smaller number */
        public const double DEL = 0.005;    /* probe-vector spacing (rad) for SPM generation */

        /* Physical constants */
        public const double C = 173.14463331;            /* speed of light (AU/day) */
        public const double UTST = 1.00273790934;        /* ratio of SI to ST second */
        public const double STRPD = (UTST * S2R * DAYSEC);   /* Earth spin rate (radians/UT day) */

        /* Command codes for the FAST process */
        public const int TRACK = 0;   /* calculate new mount and rotator demands */
        public const int ROTATOR = 1;   /* update achieved rotator angle and speed */
        public const int ST = 2;   /* update sidereal time */
        public const int MODEL = 4;   /* update pointing model */
        public const int TARGET = 8;   /* update target */
        public const int PO = 16;   /* update pointing origin */
        public const int PA = 32;   /* update field orientation */
        public const int TRANSFORM = 64;   /* update transformation matrices */
        public const int ALL = 127;   /* update the whole context */
        /* dsactx.h --------------------------------------------------------------------------*/

        /* tcsctx.h --------------------------------------------------------------------------*/
        public static TIMEO teo;
        public static SITE tsite;
        public static TSCOPE tel;
        public static ASTROM m_ast;
        public static TARG tar;
        public static ASTROM r_ast;
        public static PORIG[] por = new PORIG[NPOS];
        public static FLDOR fld;
        public static TPMOD pmod;
        public static double roll;
        public static double pitch;
        public static int jbp;
        public static double Ga, Gb;
        public static double Rota;
        public static double Rmat;
        public static double Rma;
        public static double Rmav;
        public static double[] aux = new double[MAXAUX];
        public static int ipo;
        /* tcs.h -----------------------------------------------------------------------------*/
        public enum FRAMETYPE
        {
            AZEL_OBS = 0,      /* observed Az/El */
            AZEL_TOPO = 1,     /* topocentric Az/El */
            APPT_TOPO = 2,     /* topocentric apparent RA/Dec */
            APPT = 3,          /* geocentric apparent RA/Dec */
            FK5 = 4,           /* IAU 1976 RA/Dec, any equinox */
            FK4 = 5            /* pre IAU 1976 RA/Dec, any equinox */
        }

        public enum ROTLOC
        {
            OTA = 1,           /* prime/Newtonian/Cass */
            NASMYTH_L = 2,     /* Nasmyth (left) */
            NASMYTH_R = 3,     /* Nasmyth (right) */
            COUDE_L = 4,       /* coude (left) */
            COUDE_R = 5,       /* coude (right) */
            GENRO = 6          /* generalized */
        }

        public enum MTYPE
        {
            EQUAT = 1,         /* equatorial */
            ALTAZ = 2,         /* altazimuth */
            GIMBAL = 3         /* generalized gimbal */
        }

        public enum FOPT
        {
            SLITO = 0,         /* 1D, e.g. slit */
            FIELDO = 1         /* 2D, e.g. imager */
        }

        public enum PKCODE
        {
            PK_GET,     /* retrieve item from context */
            PK_PUT,     /* insert item into context */

            PK_GTO,     /* guide by */
            PK_GBY,     /* guide to */
            PK_GTARBY,  /* guide target by */
            PK_GPOBY,   /* guide pointing-origin by */
            PK_GQ,      /* inquire the current guiding */

            PK_PN,      /* select a P.O. */
            PK_PXY,     /* set a P.O.'s base x,y */
            PK_POB,     /* set a P.O.'s offsets */
            PK_PQN,     /* inquire current P.O. */
            PK_PQXY,    /* inquire a P.O.'s net x,y */

            PK_M,       /* mount */
            PK_R,       /* rotator */
            PK_U,       /* auxiliary */
            PK_A,       /* guide star A */
            PK_B,       /* guide star B */
            PK_TBA,     /* set a target's base position */
            PK_TOB,     /* set a target's offsets */
            PK_TDT,     /* set a target's differential tracking */
            PK_TQP,     /* inquire a target's net position */

            PK_ALL,     /* slow and medium updates */
            PK_MED      /* medium updates alone */
        }

        /* ------------------------------------------------------------------ */

        /* Pointers to (optional) refraction functions */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PRF(
            int i1,
            double d1,
            double d2,
            double d3,
            double d4,
            double d5,
            double d6,
            double d7,
            double d8,
            double d9,
            double d10,
            double d11,
            double d12,

            ref double dp1,
            ref double dp2,
            ref double dp3
        );
        /// <summary>
        /// slalib header 
        /// </summary>
        /*  slalib.h */
        public struct CIpars
        {
            double pmt;        /* time interval for proper motion (Julian years) */
            double[] eb;      /* SSB to Earth (AU) */
            double[] ehn;     /* Sun to Earth unit vector */
            double gr2e;       /* (grav rad Sun)*2/(Sun-Earth distance) */
            double[] abv;     /* barycentric Earth velocity in units of c */
            double ab1;        /* sqrt(1-v**2) where v=modulus(abv) */
            double[,] bpn;  /* bias-precession-nutation matrix */
        }
        /* Star-independent intermediate-to-observed parameters */
        public struct IOpars
        {
            double along;      /* longitude + s' + dERA(DUT) (radians) */
            double phi;        /* geodetic latitude (radians) */
            double hm;         /* height above sea level (metres) */
            double xpl;        /* polar motion xp wrt local meridian (radians) */
            double ypl;        /* polar motion yp wrt local meridian (radians) */
            double sphi;       /* sine of geodetic latitude */
            double cphi;       /* cosine of geodetic latitude */
            double diurab;     /* magnitude of diurnal aberration vector */
            double p;          /* pressure (mb,hPa) */
            double tk;         /* ambient temperature (K) */
            double rh;         /* relative humidity (0-1) */
            double tlr;        /* tropospheric lapse rate (K per metre) */
            double wl;         /* wavelength (micron) or minus frequency (GHz) */
            double refa;       /* refraction constant A (radians) */
            double refb;       /* refraction constant B (radians) */
            double eral;       /* "Local" Earth Rotation Angle (radians) */
        }
        /*slamac.h*/
        public static T GMax<T>(T A, T B) where T : IComparable<T>
        {
            return A.CompareTo(B) > 0 ? A : B;
        }
        public static T GMin<T>(T A, T B) where T : IComparable<T>
        {
            return A.CompareTo(B) < 0 ? A : B;
        }
        public static double DInt(double A)
        {
            return A < 0.0 ? Math.Ceiling(A) : Math.Floor(A);
        }
        public static float AInt(float A)
        {
            return A < 0.0f ? (float)Math.Ceiling(A) : (float)Math.Floor(A);
        }
        public static double DnInt(double A)
        {
            return A < 0.0 ? Math.Ceiling(A - 0.5) : Math.Floor(A + 0.5);
        }
        public static float AnInt(float A)
        {
            return (float)DnInt(A);
        }
        public static double DSign(double A, double B)
        {
            return B < 0.0 ? -A : A;
        }
        public static double DMod(double A, double B)
        {
            if (B != 0.0)
            {
                if (A * B > 0.0)
                    return A - B * Math.Floor(A / B);
                else
                    return A + B * Math.Floor(-A / B);
            }
            else
            {
                return A;
            }
        }

        // pi
        public const double DPI = 3.1415926535897932384626433832795028841971693993751;

        // 2pi
        public const double D2PI = 6.2831853071795864769252867665590057683943387987502;

        // 1/(2pi)
        public const double D1B2PI = 0.15915494309189533576888376337251436203445964574046;

        // 4pi
        public const double D4PI = 12.566370614359172953850573533118011536788677597500;

        // 1/(4pi)
        public const double D1B4PI = 0.079577471545947667884441881686257181017229822870228;

        // pi^2
        public const double DPISQ = 9.8696044010893586188344909998761511353136994072408;

        // sqrt(pi)
        public const double DSQRPI = 1.7724538509055160272981674833411451827975494561224;

        // pi/2:  90 degrees in radians
        public const double DPIBY2 = 1.5707963267948966192313216916397514420985846996876;

        // pi/180:  degrees to radians
        public const double DD2R = 0.017453292519943295769236907684886127134428718885417;

        // 180/pi:  radians to degrees
        public const double DR2D = 57.295779513082320876798154814105170332405472466564;

        // pi/(180*3600):  arcseconds to radians
        public const double DAS2R = 4.8481368110953599358991410235794797595635330237270e-6;

        // 180*3600/pi :  radians to arcseconds
        public const double DR2AS = 2.0626480624709635515647335733077861319665970087963e5;

        // pi/12:  hours to radians
        public const double DH2R = 0.26179938779914943653855361527329190701643078328126;

        // 12/pi:  radians to hours
        public const double DR2H = 3.8197186342054880584532103209403446888270314977709;

        // pi/(12*3600):  seconds of time to radians
        public const double DS2R = 7.2722052166430399038487115353692196393452995355905e-5;

        // 12*3600/pi:  radians to seconds of time
        public const double DR2S = 1.3750987083139757010431557155385240879777313391975e4;

        // 15/(2pi):  hours to degrees x radians to turns
        public const double D15B2P = 2.3873241463784300365332564505877154305168946861068;


        /* ------------------------------------------------------------------ */

        /*
        ** ---------------------------------
        ** Time scales and Earth orientation
        ** ---------------------------------
        */

        [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
        public struct TIMEO
        {
            public double ttmtai;
            public double delat;
            public double delut;
            public double xpmr, ypmr;

        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SITE
        {
            public double tlongm, tlatm;
            public double tlong, tlat;
            public double slat, clat;
            public double hm;
            public double diurab;
            public double daz;
            public double temp;
            public double press;
            public double humid;
            public double tlr;
            public double wavelr;
            public double refar, refbr;
            public PRF rfun;
            public double t0;
            public double st0;
            public double tt0;
            public double ttj;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
            public double[] amprms;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RLOC
        {
            public ROTLOC locn;
            public double fa, fb;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TSCOPE
        {
            public double fl;
            public MTYPE mount;
            public RLOC rotl;
            public double rnogo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 3)]
            public double[] ae2nm;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 3)]
            public double[] ae2mt;

            public double ia, ib;
            public double np;
            public double ca;
            public double xt, yt, zt;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ASTROM
        {
            public FRAMETYPE cosys;
            public double eqx;
            public double wavel, refa, refb;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 3)]
            public double[] spm1;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 3)]
            public double[] spm1_i;

            public double sth, cth;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 3)]
            public double[] spm2;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 3)]
            public double[] spm2_i;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TARG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public double[] p0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public double[] dt;

            public double t0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 2)]
            public double[] ob;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public double[] op0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public double[] p;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PORIG
        {
            /*            public double p0_0;
                        public double p0_1;
                        public double ob_0_0;
                        public double ob_0_1;
                        public double ob_1_0;
                        public double ob_1_1;
                        public double ob_2_0;
                        public double ob_2_1;
                        public double p_0;
                        public double p_1;*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public double[] p0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 2)]
            public double[] ob;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public double[] p;

        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FLDOR
        {
            public double sia, cia;
            public double pai;
            public FOPT jf;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TPMOD
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXTRM)]
            public int[] model;
            public int nterml;
            public int ntermx;
            public int nterms;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = NTROOM * 9)]
            public char[] coeffn;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = NTROOM * 9)]
            public char[] coform;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXTRM)]
            public double[] coeffv;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXTRM)]
            public int[] cchain;
        }

        public struct TELV
        {
            ASTROM ast;
            double targa, targb;
            double xpo, ypo;
            double enca, encb;
            double rma;
            TSCOPE tel;
            double ga, gb;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CTX
        {
            public double tlast;
            public double adp, adv, bdp, bdv, rdp, rdv;
            public int islast;
            public double uslo, tslo, umed, tmed;
            public TIMEO teo;
            public SITE tsite;
            [MarshalAs(UnmanagedType.ByValArray)]
            public TSCOPE tel;
            public ASTROM m_ast;
            public TARG star;
            public double m_tara, m_tarb;
            public ASTROM r_ast;
            public ASTROM u_ast;
            public double u_tara, u_tarb;
            public ASTROM a_ast;
            public TARG gsa;
            public double a_tara, a_tarb;
            public ASTROM b_ast;
            public TARG gsb;
            public double b_tara, b_tarb;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = NPOS)]
            public PORIG[] por;
            public FLDOR fld;
            public TPMOD pmod;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXAUX)]
            public double[] aux;

            public double xr, yr;
            public int ipo;
            public int jbp;
            public double ga, gb;
            public double rmat;
            public double rma;
            public double rmav;
        }

        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsItimeo(double d1, double d2, double d3, double d4, ref TIMEO p1);

        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsIsite(ref TIMEO p1, double d1, double d2, double d3, double d4, PRF pr, ref SITE p2);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsIsitew(double d1, double d2, double d3, double d4, ref SITE p1);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsItel(ref SITE p1, double d1, MTYPE mtype, double d2, double d3, double d4,
              ROTLOC rotloc, double d5, double d6, double d7, ref TSCOPE p2);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsIfld(double aia, double pai, FOPT jf, ref FLDOR fld);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsIpor(double x, double y, ref PORIG por);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsIast(FRAMETYPE cosys, double eqx, double wavel, ref ASTROM ast);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsItar(double a, double b, ref TARG tar);


        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int slaDr2tf(int ndp, double angle, char[] sign, int[] i4);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int slaDr2af(int ndp, double angle, char[] sign, int[] i4);

        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsCinit(ref CTX pk, double tai,
                                   double uslo, double umed,
                                   double delat, double delut, double xpmr, double ypmr,
                                   double elon, double phi, double hm,
                                   double wavelr, PRF rfun,
                                   double tc, double pmb, double rh, double tlr,
                                   double fl, MTYPE mount,
                                   double gim1z, double gim2y, double gim3x,
                                   ROTLOC locr, double fa, double fb, double rnogo,
                                   double[] ae2nm, double[] ae2mt,
                                   IntPtr tpfile);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsMinit(ref TPMOD pmod);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        extern public static int tcsAddtrm([MarshalAs(UnmanagedType.LPStr)] string cname, double cvalue, ref TPMOD pmod);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int tcsSterm([MarshalAs(UnmanagedType.LPStr)] string cname, double cvalue, ref TPMOD pmod);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int tcsQterm(int iterm, ref TPMOD pmod, [MarshalAs(UnmanagedType.LPStr)] StringBuilder cname, ref double cvalue);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int tcsQnatrm(StringBuilder cname, ref TPMOD pmod, ref double cvalue, ref int iterm);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsMedium(double tai, double roll, double pitch, int jbp, ref double[] aux, ref SITE tsite, ref TPMOD pmod, ref TARG tar, ref TSCOPE tel, ref ASTROM m_ast, ref ASTROM r_ast);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsFast(int mode, out int isol, double tai, double Rmat, double Rma, double Rmav, ref SITE tsite, ref TSCOPE tel, double Ga, double Gb, ref TARG tar, ref PORIG por, ref FLDOR fld, ref ASTROM m_ast, ref ASTROM r_ast, ref double roll, ref double pitch, ref double rota);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsIporob(int iob, double obx, double oby, ref PORIG por);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsPorup(ref PORIG por);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsItarob(int iob, double oba, double obb, ref TARG tar);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsTargup(double tai, ref TARG tar);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern double slaDranrm(double angle);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsTime(out double tai);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsAer(double t, ref SITE tsite, ref ASTROM m_ast);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsQpor(ref PORIG por, ref TSCOPE tel, ref double xr, ref double yr);

        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsAstup(double tai, ref TARG tar, ref SITE tsite, ref TSCOPE tel, ref ASTROM m_ast, ref ASTROM r_ast);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsTartran(ref SITE tsite, ref TSCOPE tel, double sst, double cst, out double[] target, ref ASTROM r_ast);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsSlow(double tai, ref TIMEO teo, ref SITE tsite);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double slaDrange(double angle);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsVTenc(double tara, double tarb, ref ASTROM m_ast, double rma, double ap, double bp, double xpo, double ypo, ref TSCOPE tel, double ga, double gb, out double a1, out double b1, out double a2, out double b2);
        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tcsVTsky(double enca, double encb, ref ASTROM m_ast, double rma, double xpo, double ypo, ref TSCOPE tel, double ga, double gb, out double tara, out double tarb);

        [DllImport("tcspkDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tptPtappl(char atype, double asize, double sinphi, double cosphi, int mount, double[] ae2nm, int jbp, int method, double[] vc, double[] pmodel);
        /* -----------------------------------------------------------------------------------*/
        public static double[] target = new double[2];

        public const MTYPE MOUNT = MTYPE.ALTAZ;
        public const ROTLOC RLOCN = ROTLOC.OTA;
        public const double GIM1Z = 0.0;
        public const double GIM2Y = 0.0;
        public const double GIM3X = 0.0;
        public void demo()
        {
            int[] i4 = new int[4];
            char[] sterm = new char[9];
            char s;
            int i, n, chorpa, j;
            double vterm, tai, ra, dec, t = 0.0, xim, yim, a1, b1, a2, b2, a, b, waz, wel;

            // 시간 척도 및 지구 방향 초기화
            // tcsItimeo(0, 0.746, 0.25 * AS2R, 0.4 * AS2R, ref teo);
            tcsItimeo(37, 0, 0 * AS2R, 0 * AS2R, ref teo);
            // 사이트별 항목을 초기화
            tcsIsite(ref teo, ObservationSiteInfo.LONGITUDE * D2R, ObservationSiteInfo.LATITUDE * D2R, ObservationSiteInfo.ALTITUDE, 0.5, null, ref tsite);
            // 사이트의 날씨 수치를 업데이트
            // tcsIsitew(1.85, 605.0, 0.8, 0.0065, ref tsite);
            tcsIsitew(0, 0, 0, 0.0065, ref tsite);
            // 망원경 구성을 초기화
            //tcsItel(ref tsite, 128000.0, MOUNT, GIM1Z, GIM2Y, GIM3X, RLOCN, 0.0, 0.0, 0.25 * D2R, ref tel);

            Console.WriteLine($"tlongm: {tsite.tlongm}");
            Console.WriteLine($"tlatm: {tsite.tlatm}");
            Console.WriteLine($"tlong: {tsite.tlong}");
            Console.WriteLine($"tlat: {tsite.tlat}");
            Console.WriteLine($"slat: {tsite.slat}");
            Console.WriteLine($"clat: {tsite.clat}");
            Console.WriteLine($"hm: {tsite.hm}");
            Console.WriteLine($"diurab: {tsite.diurab}");
            Console.WriteLine($"daz: {tsite.daz}");
            Console.WriteLine($"temp: {tsite.temp}");
            Console.WriteLine($"press: {tsite.press}");
            Console.WriteLine($"humid: {tsite.humid}");
            Console.WriteLine($"tlr: {tsite.tlr}");
            Console.WriteLine($"wavelr: {tsite.wavelr}");
            Console.WriteLine($"refar: {tsite.refar}");
            Console.WriteLine($"refbr: {tsite.refbr}");
            Console.WriteLine($"rfun: {tsite.rfun}");
            Console.WriteLine($"t0: {tsite.t0}");
            Console.WriteLine($"st0: {tsite.st0}");
            Console.WriteLine($"tt0: {tsite.tt0}");
            Console.WriteLine($"ttj: {tsite.ttj}");

            Console.Write("amprms: ");
            if (tsite.amprms != null)
            {
                foreach (var value in tsite.amprms)
                {
                    Console.Write($"{value} ");
                }
            }
            Console.WriteLine();
            slalibtest();
        }
        public void slalibtest()
        {
            int ndp = 4;
            double angle = -3.01234;
            StringBuilder sign = new StringBuilder(2);
            sign.Append(' ');
            int[] idmsf = new int[4];

            //  slaDr2tf(ndp, angle, sign, idmsf);

            Console.WriteLine($"Sign: {sign}");
            Console.WriteLine($"Degrees: {idmsf[0]}, Arcminutes: {idmsf[1]}, Arcseconds: {idmsf[2]}, Fraction: {idmsf[3]}");
        }
        public CTX pk;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        char[] sterm;
        public void StarCalibrationInit()
        {
            CTX pk = new CTX();
            TELV vt; //Virtual Telescope
            TPMOD pm;
            MTYPE mount;
            ROTLOC locr;
            ASTROM ast;
            int i, n, chorpa, j, npo, nob, ipo, jbp;
            int[] i4 = new int[4];
            char s;
            // char[] sterm = new char[9];

            string tpfile;
            double tai, uslo, umed, delat, delut, xpmr, ypmr, elon, phi, hm,
                   wavelr, tc, pmb, rh, tlr, fl, gim1z, gim2y, gim3x, fa, fb,
                   rnogo, vterm, da, db, ga, gb, x, y, dx, dy, ra, dec, w1, w2,
                   dra, ddec, dema, demb, demr, a1, b1, a2, b2, a, b,
                   az, el, ypam, ypar, ypau;
            /* TAI (from an internal clock simulator). */
            tai = TimeService.GetCurrentTAI();

            /* SLOW and MEDIUM update intervals (s). */
            uslo = 60.0;
            umed = 5.0;

            /* TAI-UTC and UT1-UTC (s). */
            delat = 29.0;
            delut = 0.746;

            /* Polar motion (radians). */
            xpmr = 0.25 * AS2R;
            ypmr = 0.4 * AS2R;

            /* East longitude and geodetic latitude (radians). */
            elon = -155.471667 * D2R;
            phi = 19.826667 * D2R;

            /* Height above sea level (m). */
            hm = 4145.0;

            /* Reference wavelength (microns). */
            wavelr = 0.5;

            /* Special refraction function. */
            //   rfun = null;

            /* Temperature (C), pressure (hPa) and humidity (1.0 = 100%). */
            tc = 2.25;
            pmb = 606.0;
            rh = 0.79;

            /* Tropospheric lapse rate (K/m). */
            tlr = 0.0065;

            /* Telescope focal length (user units). */
            fl = 128000.0;

            /* Mount type. */
            mount = MTYPE.ALTAZ;

            /* Euler angles (radians, generalized gimbal case only). */
            gim1z = 0.0;
            gim2y = 0.0;
            gim3x = 0.0;

            /* Rotator location code. */
            locr = ROTLOC.OTA;

            /* Effect of roll and pitch (GENRO case only). */
            fa = 10.0;
            fb = 10.0;

            /* Pole avoidance distance (radians). */
            rnogo = 0.25 * D2R;

            /* Name of TPOINT model file. */
            tpfile = "altaz.mod";
            IntPtr tpfilePtr = Marshal.StringToHGlobalAnsi(tpfile);

            double[,] ae2nmArray = new double[,]
{
    { 1.0, 2.0, 3.0 },
    { 4.0, 5.0, 6.0 },
    { 7.0, 8.0, 9.0 }
};

            double[,] ae2mtArray = new double[,]
            {
    { 9.0, 8.0, 7.0 },
    { 6.0, 5.0, 4.0 },
    { 3.0, 2.0, 1.0 }
            };

            double[] flattenedAe2nm = Flatten2DArray(ae2nmArray);
            double[] flattenedAe2mt = Flatten2DArray(ae2mtArray);

            if (tcsCinit(ref pk, tai, uslo, umed, delat, delut, xpmr, ypmr, elon, phi, hm,
                          wavelr, null, tc, pmb, rh, tlr, fl, mount, gim1z, gim2y, gim3x,
                          locr, fa, fb, rnogo, flattenedAe2nm, flattenedAe2mt, tpfilePtr) == 0)
            {
                Console.WriteLine("Context initialization has succeeded!!");
            }
            else
            {
                Console.WriteLine("Context initialization has failed: aborting!");
            }

            Marshal.FreeHGlobal(tpfilePtr);
        }
        private static double[] Flatten2DArray(double[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            double[] flattened = new double[rows * cols];
            int index = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flattened[index++] = array[i, j];
                }
            }

            return flattened;
        }
        private static double[] FlattenArray(double[][] array)
        {
            if (array == null)
            {
                return Array.Empty<double>();
            }

            return array.SelectMany(innerArray => innerArray).ToArray();
        }
        public class TimeService
        {
            private const double Tick = 0.05 / (24 * 60 * 60);
            private static int itick = 0;
            private static double t0;

            public static double GetCurrentTAI()
            {
                double tai;

                if (itick == 0)
                {
                    tai = TcsTime();

                    tai -= 21 * Tick;

                    t0 = tai;
                }
                else
                {
                    tai = t0 + itick * Tick;
                }

                itick++;

                return tai;
            }

            private static double TcsTime()
            {
                return DateTime.UtcNow.ToOADate();
            }
        }

        /*     public void calculatAzEl(double ra, double dec, double lat, double lon, double alt, double *az, double *el)
             {

             }
             public void sendToDevice()
             {

             }*/

        public void StarExcute()
        {
            pk = new CTX();
            int errPM;
            StarInit();
            errPM = PointingModelCheck();
            if (errPM == 0)
            {
                PointingModelUpdating(pmod);
                ObtainFieldOrientation();
                ObtainPointingOrigin();
                // ObtainTarget();
            }
            else
            {
                Console.WriteLine("PointingModel error!");
            }
        }
        public void StarExcute(int ProjectMode)
        {
            pk = new CTX();
            int errPM;
            StarInit();
            errPM = PointingModelCheck(ProjectMode);
            if (errPM == 0)
            {
                PointingModelUpdating(pmod);
                ObtainFieldOrientation();
                ObtainPointingOrigin();
                // ObtainTarget();
            }
            else
            {
                Console.WriteLine("PointingModel error!");
            }
        }
        public void StarInit()
        {
            // 시간 척도 및 지구 방향 초기화
            //   tcsItimeo(27, 0.746, 0.25 * AS2R, 0.4 * AS2R, ref teo);
            tcsItimeo(37, 0, 0 * AS2R, 0 * AS2R, ref teo);
            // 사이트별 항목을 초기화
            //tcsIsite(ref teo, -155.471667 * D2R, 19.826667 * D2R, 4145.0, 0.5, null, ref tsite); 
            tcsIsite(ref teo, ObservationSiteInfo.LONGITUDE * D2R, ObservationSiteInfo.LATITUDE * D2R, ObservationSiteInfo.ALTITUDE, 0.5, null, ref tsite);
            // 사이트의 날씨 수치를 업데이트
          //  tcsIsitew(1.85, 605.0, 0.8, 0.0065, ref tsite); //temperature , pressure, relative humidity (1.0 = 100%) , tropospheric lapse rate(K/m) /////////////// TODO : 환경계측 모듈로부터 수신한 데이터 할당
            // tcsIsitew(25, 605.0, 0.8, 0.0065, ref tsite); //temperature , pressure, relative humidity (1.0 = 100%) , tropospheric lapse rate(K/m) /////////////// TODO : 환경계측 모듈로부터 수신한 데이터 할당
            tcsIsitew(0, 0, 0, 0.0065, ref tsite); //temperature , pressure, relative humidity (1.0 = 100%) , tropospheric lapse rate(K/m) /////////////// TODO : 환경계측 모듈로부터 수신한 데이터 할당
            // 망원경 구성을 초기화
            tcsItel(ref tsite, 128000.0, MOUNT, GIM1Z, GIM2Y, GIM3X, RLOCN, 0.0, 0.0, 0.25 * D2R, ref tel);
            // Field orientation
            tcsIfld(0.0, 0.0, FOPT.FIELDO, ref fld); //IAA
            //Pointing origin (rotator axis)
            ipo = 0;
            tcsIpor(0.0, 0.0, ref por[ipo]);
            //Tracking systems, mount and rotator
            //     tcsIast(FRAMETYPE.FK5, 1975.0, 0.5, ref m_ast);
            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            r_ast = m_ast;
            //Target
            tcsItar(0.0, 0.0, ref tar);
        }
        double cvalue;
        StringBuilder cname = new StringBuilder(9);
        public int PointingModelCheck()
        {
            if (tcsMinit(ref pmod) != 0) return -1;/*
            if (tcsAddtrm("IA", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", 0 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("TF", 0 * AS2R, ref pmod) <= 0) return -1;*/
            //  if (tcsSterm("AW", 0 * AS2R, ref pmod) != 0) return -1;
            //1과제

            /*            if (tcsAddtrm("IA", -9014.481 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", -691.042 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", 113.123 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 1042.742 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", -73.395 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", -30.982 * AS2R, ref pmod) <= 0) return -1;*/
            /*            if (tcsAddtrm("IA", -9243.278 * AS2R, ref pmod) <= 0) return -1;   //0526 마운트모델링 2차
                        if (tcsAddtrm("IE", -654.137 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", -168.000 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 1177.092 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", -90.665 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", -14.881 * AS2R, ref pmod) <= 0) return -1;*/
            /*      if (tcsAddtrm("IA", -5979.406 * AS2R, ref pmod) <= 0) return -1;   //0527 마운트모델링 terms 추가  망
                  if (tcsAddtrm("IE", -1027.047 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("NPAE", -124.793 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("CA", 463.142 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("AN", -31.353 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("AW", -41.666 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("NRX", -3141.012 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("NRY", 124.162 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("CRX", 11.230 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("CRY", -29.571 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("ACES", -25.298 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("ACEC", -63.852 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("ECES", -2838.394 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("TF", -248.956 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("TX", 67.222 * AS2R, ref pmod) <= 0) return -1;
                  if (tcsAddtrm("POX", -510.821 * AS2R, ref pmod) <= 0) return -1;*/
            /*            if (tcsAddtrm("IA", -9237.664 * AS2R, ref pmod) <= 0) return -1;   //0526 마운트모델링 2차 (항목 추가)
                        if (tcsAddtrm("IE", -708.441 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", -162.207 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 1170.251 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", -86.067 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", -22.928 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("TF", -285.063 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("FO", -3.814 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("TX", -109.961 * AS2R, ref pmod) <= 0) return -1;*/
/*            if (tcsAddtrm("IA", -9144.466 * AS2R, ref pmod) <= 0) return -1;   //0527 마운트모델링  (100개)
            if (tcsAddtrm("IE", -1311.403 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("NPAE", -25.825 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("CA", 1009.081 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("AN", -57.661 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("AW", -22.962 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ACES", -41.680 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ACEC", -61.818 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ECES", 563.324 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ECEC", 413.352 * AS2R, ref pmod) <= 0) return -1;*/
            if (tcsAddtrm("IA", -8971.747 * AS2R, ref pmod) <= 0) return -1;   //250918 (ModelingFormat 20250918_test_Convert.dat)
            if (tcsAddtrm("IE", -1376.828 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("NPAE", 20.303 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("CA", 854.038 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("AN", -59.132 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("AW", -44.417 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ACES", -30.129 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ACEC", -64.131 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ECES", 506.642 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("ECEC", 487.119 * AS2R, ref pmod) <= 0) return -1;
            //2과제 
            /*
            if (tcsAddtrm("IA", -9088.143 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("IE", -733.872 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("NPAE", -68.629 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("CA", 1000.502 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("AN", 62.409 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", -94.689 * AS2R, ref pmod) <= 0) return -1;*/

            /*            if (tcsAddtrm("IA", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("TF", 0 * AS2R, ref pmod) <= 0) return -1;*/

            /*            if (tcsAddtrm("IA", -1576.671 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", -15.964 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", 961.893 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", -1363.589 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", 28.344 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", 3627.864 * AS2R, ref pmod) <= 0) return -1;*/
            /*            if (tcsAddtrm("IA", -1565.67 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", -15.96
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", -2.27785032461514E-05
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 1784.11026508702
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", 20.3848843789642 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", -6.73521907741983
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("TF", 0.369756401939849 * AS2R, ref pmod) <= 0) return -1;*/
            //if (tcsSterm("AW", 0.0 * AS2R, ref pmod) != 0) return -1;
            return 0;
        }
        public int PointingModelCheck(int mode)
        {
            if (tcsMinit(ref pmod) != 0) return -1;
/*            if (tcsAddtrm("IA", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", 0 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", 0 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("TF", 0 * AS2R, ref pmod) <= 0) return -1;*/
           // if (tcsSterm("AW", 0 * AS2R, ref pmod) != 0) return -1;
            //1과제
            if (mode == 1)

            {

                /*
                                if (tcsAddtrm("IA", -109.934 * AS2R, ref pmod) <= 0) return -1;   //0526 마운트모델링
                                if (tcsAddtrm("IE", 39.579 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("NPAE", 54.251 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("CA", 53.365 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AN", -169.229 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AW", 59.496 * AS2R, ref pmod) <= 0) return -1;*/
                //if (tcsAddtrm("TF", 0 * AS2R, ref pmod) <= 0) return -1;/*
                /*                if (tcsAddtrm("IA", -9243.278 * AS2R, ref pmod) <= 0) return -1;   //0526 마운트모델링 2차
                                if (tcsAddtrm("IE", -654.137 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("NPAE", -168.000 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("CA", 1177.092 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AN", -90.665 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AW", -14.881 * AS2R, ref pmod) <= 0) return -1;      */
                /*
                                if (tcsAddtrm("IA", -5979.406 * AS2R, ref pmod) <= 0) return -1;   //0527 마운트모델링 terms 추가
                                if (tcsAddtrm("IE", -1027.047 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("NPAE", -124.793 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("CA", 463.142 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AN", -31.353 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AW", -41.666 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("NRX", -3141.012 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("NRY", 124.162 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("CRX", 11.230 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("CRY", -29.571 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("ACES", -25.298 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("ACEC", -63.852 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("ECES", -2838.394 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("TF", -248.956 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("TX", 67.222 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("POX", -510.821 * AS2R, ref pmod) <= 0) return -1;*/
                /*           if (tcsAddtrm("IA", -9164.767 * AS2R, ref pmod) <= 0) return -1;   //0527 마운트모델링  (100개)
                           if (tcsAddtrm("IE", -630.276 * AS2R, ref pmod) <= 0) return -1;
                           if (tcsAddtrm("NPAE", -76.657 * AS2R, ref pmod) <= 0) return -1;
                           if (tcsAddtrm("CA", 1062.078 * AS2R, ref pmod) <= 0) return -1;
                           if (tcsAddtrm("AN", -68.911 * AS2R, ref pmod) <= 0) return -1;
                           if (tcsAddtrm("AW", 0.320 * AS2R, ref pmod) <= 0) return -1;
                           if (tcsAddtrm("TF", 19.084 * AS2R, ref pmod) <= 0) return -1;*/

                /*                if (tcsAddtrm("IA", -9144.466 * AS2R, ref pmod) <= 0) return -1;   //0527 마운트모델링  (100개)
                                if (tcsAddtrm("IE", -1311.403 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("NPAE", -25.825 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("CA", 1009.081 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AN", -57.661 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("AW", -22.962 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("ACES", -41.680 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("ACEC", -61.818 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("ECES", 563.324 * AS2R, ref pmod) <= 0) return -1;
                                if (tcsAddtrm("ECEC", 413.352 * AS2R, ref pmod) <= 0) return -1;*/
                if (tcsAddtrm("IA", -8971.747 * AS2R, ref pmod) <= 0) return -1;   //250918 (ModelingFormat 20250918_test_Convert.dat)
                if (tcsAddtrm("IE", -1376.828 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("NPAE", 20.303 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("CA", 854.038 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("AN", -59.132 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("AW", -44.417 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("ACES", -30.129 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("ACEC", -64.131 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("ECES", 506.642 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("ECEC", 487.119 * AS2R, ref pmod) <= 0) return -1;
                //if (tcsAddtrm("TF", 0 * AS2R, ref pmod) <= 0) return -1;
                /*
                if (tcsAddtrm("IA", -9014.481 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("IE", -691.042 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("NPAE", 113.123 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("CA", 1042.742 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("AN", -73.395 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("AW", -30.982 * AS2R, ref pmod) <= 0) return -1;*/
            }
            if (mode == 2)
            {
                if (tcsAddtrm("IA", -9088.143 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("IE", -733.872 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("NPAE", -68.629 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("CA", 1000.502 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("AN", 62.409 * AS2R, ref pmod) <= 0) return -1;
                if (tcsAddtrm("AW", -94.689 * AS2R, ref pmod) <= 0) return -1; //
            }
            /*            if (tcsAddtrm("IA", -9014.481 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", -691.042 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", 113.123 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 1042.742 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", -73.395 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", -30.982 * AS2R, ref pmod) <= 0) return -1;*/
            //2과제 

/*            if (tcsAddtrm("IA", -9088.143 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("IE", -733.872 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("NPAE", -68.629 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("CA", 1000.502 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("AN", 62.409 * AS2R, ref pmod) <= 0) return -1;
            if (tcsAddtrm("AW", -94.689 * AS2R, ref pmod) <= 0) return -1;*/

            /*            if (tcsAddtrm("IA", -1576.671 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", -15.964 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", 961.893 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", -1363.589 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", 28.344 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", 3627.864 * AS2R, ref pmod) <= 0) return -1;*/
            /*            if (tcsAddtrm("IA", -1565.67 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("IE", -15.96
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("NPAE", -2.27785032461514E-05
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("CA", 1784.11026508702
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AW", 20.3848843789642 * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("AN", -6.73521907741983
             * AS2R, ref pmod) <= 0) return -1;
                        if (tcsAddtrm("TF", 0.369756401939849 * AS2R, ref pmod) <= 0) return -1;*/
            //if (tcsSterm("AW", 0.0 * AS2R, ref pmod) != 0) return -1;
            return 0;
        }
        public void PointingModelUpdating(TPMOD pmod)
        {
            int i = 1;
            int chorpa;
            double vterm;
            int iterm;
            //extern public static int tcsQterm(int iterm, ref TPMOD pmod, string cname, ref double[] cvalue);
            // Get each term in sequence.
            // Call tcsQterm in a loop.
            for (iterm = 1; ; iterm++)
            {
                int result = tcsQterm(iterm, ref pmod, cname, ref cvalue);

                if (result == 0)
                {
                    // Report the term.
                    chorpa = char.IsLower(cname[0]) ? '&' : ' ';
                    string formattedCname = chorpa == '&' ? cname.ToString().ToUpper() : cname.ToString();

                    Console.WriteLine($"{iterm,3}  {chorpa} {formattedCname,-8}{cvalue / AS2R}");
                }
                else if (result == -1)
                {
                    break;
                }
            }
            // Demonstrate looking up a named term "CH".
            StringBuilder sterm1 = new StringBuilder("CH");
            int j1 = tcsQnatrm(sterm1, ref pmod, ref cvalue, ref iterm);
            if (j1 == 0)
                Console.WriteLine($"Term {sterm1} is number {iterm} and has amplitude {cvalue / AS2R} arcsec.");

            // Demonstrate looking up a named term "AW".
            StringBuilder sterm2 = new StringBuilder("TF");
            int j2 = tcsQnatrm(sterm2, ref pmod, ref cvalue, ref iterm);
            if (j2 == 0)
                Console.WriteLine($"\nTerm {sterm2} is number {iterm} and has amplitude {cvalue / AS2R} arcsec.");

        }
        double tai, endtai;

        public void ObtainFieldOrientation()
        {
            roll = 0.0;
            pitch = 0.0;
            jbp = 0;
            double[] aux = new double[MAXAUX];
            for (int index = 0; index < MAXAUX; index++)
            {
                aux[index] = 0.0;
            }
            Ga = 0.0 * AS2R;
            Gb = 0 * AS2R;
            Rma = 0 * D2R;
            Rmav = 0.0;
            Rmat = 0.0;

            tai = GetCurrentTaiMjd();
            tcsSlow(tai, ref teo, ref tsite);

            double temp = tai - 51544.5;
            // double temp = 9060.6094279747631;
            double dGmst = (18.697374558 + (24.06570982441908 * temp)) % 24;
            double st0 = (dGmst + (ObservationSiteInfo.LONGITUDE / 15.0)) % 24;

            tsite.st0 = (st0 * 15) * D2R;
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(ALL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            tcsIfld(0.0, 0.0, FOPT.FIELDO, ref fld);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(PA, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
        }
        public void tcsSlowUpdate()
        {
            roll = 0.0;
            pitch = 0.0;
            jbp = 0;
            double[] aux = new double[MAXAUX];
            for (int index = 0; index < MAXAUX; index++)
            {
                aux[index] = 0.0;
            }
            Ga = 0.0 * AS2R;
            Gb = 0 * AS2R;
            Rma = 0 * D2R;
            Rmav = 0.0;
            Rmat = 0.0;

            var tai = GetCurrentTaiMjd();
            // tai = 60605.1094278; // UTC 2024-10-22 02:37:35
            tcsSlow(tai, ref teo, ref tsite);

            double temp = tai - 51544.5;
            double dGmst = (18.697374558 + (24.06570982441908 * temp)) % 24;
            double st0 = (dGmst + (ObservationSiteInfo.LONGITUDE / 15.0)) % 24;

            tsite.st0 = (st0 * 15) * D2R;
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(ALL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            tcsIfld(0.0, 0.0, FOPT.FIELDO, ref fld);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(PA, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
        }
        double rota;
        public void ObtainPointingOrigin()
        {
            ipo = 0;
             tcsIpor(0.0, 0.0, ref por[ipo]);
            // tcsIpor(180.0, -150.0, ref por[ipo]);
            tcsIporob(0, 0, 0, ref por[ipo]);
            tcsPorup(ref por[ipo]);
            tcsFast(PO, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota); // 16: update pointing origin
            Console.WriteLine($"Azimuth : {roll}, Elevation : {pitch}, Rotator angle : {rota}");
            /*public double adp, adv, bdp, bdv, rdp, rdv;
      public int islast;*/
        }
        /* StringBuilder sign = new StringBuilder(2);*/
        char[] sign = new char[1];
        public int[] i4 = new int[4];
        public int[] j4 = new int[4];
        double t;
        public static double xim;
        public static double yim;
        static double a1, b1, a2, b2, a, b, c, d;
        public static double[] targetposition;
        public static double[] targetendposition;
        public static double[] Satelliteposition;
        public static int starvector;

        // Topo
        double dRa = 3.100645;
        double dDec = 89.365651;
        // 2000.0
        /*        double dRa = 2.530301;
                double dDec = 89.264109;*/

        public double[] ObtainTargetPointed(DateTime currentUTCDate, double currentUTCMillis)
        {
            double[] targettimestamps = GetCurrentTaiMjd(currentUTCDate, currentUTCMillis);
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            DateTime offsetTime = currentUTCDate.AddMilliseconds(currentUTCMillis);
            DateTime endTime = currentUTCDate.AddMilliseconds(currentUTCMillis + 1200000);
            TimeSpan timeSinceMjdEpoch = offsetTime - mjdEpoch;
            TimeSpan additionalSeconds = TimeSpan.FromSeconds(37);
            timeSinceMjdEpoch = timeSinceMjdEpoch.Add(additionalSeconds);
            TimeSpan endtimeSinceMjdEpoch = endTime - mjdEpoch;

            // tai = 60605.1094278;
            tai = GetCurrentTaiMjd();

            endtai = endtimeSinceMjdEpoch.TotalDays;

            targetposition = new double[2];
            targetendposition = new double[2];
            a1 = 0;
            a2 = 0;
            b1 = 0; b2 = 0;
            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);

            r_ast = m_ast;

            tcsItar(StarCalibration_Setting.Ra * D2R, StarCalibration_Setting.Deg * D2R, ref tar);
            //  tcsItar(45.075583* D2R, 89.364417 * D2R, ref tar);
            // tcsItar(172.7663456 * D2R, 37.2731078 * D2R, ref tar);
            // tcsItar(dRa * 15 * D2R, dDec * D2R, ref tar);


            tcsItarob(0, 0.0, 0.0, ref tar);
            tcsTargup(tai, ref tar);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            
            slaDr2tf(4, slaDranrm(tar.p0[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.p0[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- test star");

            slaDr2tf(4, slaDrange(tar.ob[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.ob[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- offset");
            
            tcsFast(TRANSFORM + MODEL + TARGET, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsFast(TRACK, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsTargup(tai, ref tar);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(TRANSFORM + MODEL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
            a = a1;
            b = b1;
            targetposition[0] = slaDranrm(PI - a) / D2R;
            targetposition[1] = b / D2R;

            tcsTargup(endtai, ref tar);
            tcsMedium(endtai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);

            slaDr2tf(4, slaDranrm(tar.p0[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.p0[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- test star");

            slaDr2tf(4, slaDrange(tar.ob[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.ob[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- offset");

            tcsFast(TRANSFORM + MODEL + TARGET, out jbp, endtai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsFast(TRACK, out jbp, endtai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsTargup(endtai, ref tar);
            tcsMedium(endtai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(TRANSFORM + MODEL, out jbp, endtai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
            a = a1;
            b = b1;
            targetendposition[0] = slaDranrm(PI - a) / D2R;
            targetendposition[1] = b / D2R;
            Console.WriteLine("sky-to-encoders");
            Console.WriteLine($"timestamp : {tai}");
            starvector = StarPositionSignDecide(targetposition[0], targetendposition[0]);
            targetposition[0] = AzConvert(targetposition[0], starvector);
            Console.WriteLine($"Az : {targetposition[0]}");
            Console.WriteLine($"El : {targetposition[1]}");
            tcsAer(tai, ref tsite, ref m_ast);
            tcsQpor(ref por[ipo], ref tel, ref xim, ref yim);

            a1 = 0;
            a2 = 0;
            b1 = 0;
            b2 = 0;
            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            return targetposition;
        }


        public (double, double) getSingleSatellitePosition(DateTime currentUTCDate, double currentUTCMillis, double[] satelliteInfo, double addmilsecond, DateTime startTime, DateTime endTime)
        {
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            DateTime offsetTime = startTime.AddMilliseconds(addmilsecond);
            TimeSpan timeSinceMjdEpoch = offsetTime - mjdEpoch;
            TimeSpan endtimeSinceMjdEpoch = endTime - mjdEpoch;

            tai = timeSinceMjdEpoch.TotalDays;
            endtai = endtimeSinceMjdEpoch.TotalDays;

            targetposition = new double[2];
            targetendposition = new double[2];
            a1 = 0; a2 = 0; b1 = 0; b2 = 0;

            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            r_ast = m_ast;
            double satelliteRa = satelliteInfo[0];
            double satelliteDec = satelliteInfo[1];
            tcsItar(satelliteRa * D2R, satelliteDec * D2R, ref tar);

            sateltcsSlowUpdate();
            tcsItarob(0, 0.0, 0.0, ref tar);
            tcsTargup(tai, ref tar);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);

            slaDr2tf(4, slaDranrm(tar.p0[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.p0[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- test star");

            slaDr2tf(4, slaDrange(tar.ob[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.ob[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- offset");

            tcsFast(TRANSFORM + MODEL + TARGET, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsFast(TRACK, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsTargup(tai, ref tar);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(TRANSFORM + MODEL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
            a = a1;
            b = b1;
            targetposition[0] = slaDranrm(PI - a) / D2R;
            targetposition[1] = b / D2R;

            tcsTargup(endtai, ref tar);
            tcsMedium(endtai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);

            slaDr2tf(4, slaDranrm(tar.p0[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.p0[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- test star");

            slaDr2tf(4, slaDrange(tar.ob[0]), sign, i4);
            Console.WriteLine($"sign - {sign}Ra - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}");
            slaDr2af(3, tar.ob[1], sign, i4);
            Console.WriteLine($"Dec - degrees : {i4[0]}, arcminutes : {i4[1]}, arcseconds : {i4[2]}, fraction : {i4[3]}  <<<--- offset");

            tcsFast(TRANSFORM + MODEL + TARGET, out jbp, endtai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsFast(TRACK, out jbp, endtai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsTargup(endtai, ref tar);
            tcsMedium(endtai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(TRANSFORM + MODEL, out jbp, endtai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
            a = a1;
            b = b1;
            targetendposition[0] = slaDranrm(PI - a) / D2R;
            targetendposition[1] = b / D2R;
            Console.WriteLine("sky-to-encoders");
            Console.WriteLine($"timestamp : {tai}");
            starvector = StarPositionSignDecide(targetposition[0], targetendposition[0]);
            targetposition[0] = AzConvert(targetposition[0], starvector);
            Console.WriteLine($"Az : {targetposition[0]}");
            Console.WriteLine($"El : {targetposition[1]}");
            tcsAer(tai, ref tsite, ref m_ast);
            tcsQpor(ref por[ipo], ref tel, ref xim, ref yim);

            a1 = 0;
            a2 = 0;
            b1 = 0;
            b2 = 0;

            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            return (targetposition[0],targetposition[1]);
        }
        public double[] getSSatelliteInfo(DateTime UtcNow)
        {
            var PointingPosition = new double[4];

            TimeSpan timeDifference = ObservingSatellite_Information.observingSatellite_endTime - ObservingSatellite_Information.observingSatellite_startTime;
            double totalDays = timeDifference.TotalMilliseconds;
            int total = (int)(totalDays / (ObservingSatellite_Information.observingSatellite_interval * 1000));
            for (var j = 0; j < total; j++)
            {
                var currentTime = ObservingSatellite_Information.observingSatellite_startTime.Add(TimeSpan.FromMilliseconds(ObservingSatellite_Information.observingSatellite_interval * j * 1000));

                if (currentTime > UtcNow)
                {
                    PointingPosition[0] = ObservingSatellite_Information.observingSatellite_RaDec[0][j];
                    PointingPosition[1] = ObservingSatellite_Information.observingSatellite_RaDec[1][j];
                    PointingPosition[2] = ObservingSatellite_Information.observingSatellite_interval * j * 1000;
                    PointingPosition[3] = j;
                    return PointingPosition;
                }
            }

            return PointingPosition;
        }
        public object[][] getMSatelliteInfo(DateTime UtcNow)
        {
            var satelliteInfo = new object[3][]
            {
        new object[8],
        new object[8],
        new object[8]
            };

            int trigger = 0;

            TimeSpan timeDifference = ObservingSatellite_Information.observingSatellite_endTime - ObservingSatellite_Information.observingSatellite_startTime;
            double totalMilliseconds = timeDifference.TotalMilliseconds;

            int total = (int)(totalMilliseconds / (ObservingSatellite_Information.observingSatellite_interval * 1000));

            long intervalMilliseconds = (long)(ObservingSatellite_Information.observingSatellite_interval * 1000);

            double comparisonTime = (UtcNow - UtcNow.Date).TotalMilliseconds;

            double currentMilli = (ObservingSatellite_Information.observingSatellite_startTime - UtcNow.Date).TotalMilliseconds;

            for (var j = 0; j < total; j++)
            {
                currentMilli = (ObservingSatellite_Information.observingSatellite_startTime - UtcNow.Date).TotalMilliseconds + intervalMilliseconds * j;

                if (currentMilli > comparisonTime)
                {
                    trigger = j;
                    break;
                }
            }
            int baseIndex = trigger - 3;
            if (baseIndex + 7 >= total)
            {
                return null; 
            }
            for (var i = 0; i < 8; i++)
            {
                satelliteInfo[0][i] = (double)ObservingSatellite_Information.observingSatellite_RaDec[0][baseIndex + i];
                satelliteInfo[1][i] = (double)ObservingSatellite_Information.observingSatellite_RaDec[1][baseIndex + i];
                satelliteInfo[2][i] = ObservingSatellite_Information.observingSatellite_startTime.AddMilliseconds(intervalMilliseconds * (baseIndex + i));
            }

            return satelliteInfo;
        }
        public double[][] getSatelliteTrackingPosition(object[][] satelliteInfo)
        {

            double[] t = GetCurrentUTC_Satel(satelliteInfo[2]);
            double[] targetposition = new double[2];
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);

            r_ast = m_ast;


            double a1_deg;
            double b1_deg;
            double[][] positionData = new double[2][];
            for (int i = 0; i < positionData.Length; i++)
            {
                positionData[i] = new double[8];
            }
            for (var i = 0; i < 8; i++)
            {
                tcsItar((double)satelliteInfo[0][i] * D2R, (double)satelliteInfo[1][i] * D2R, ref tar);
                count++;
                if (count > 300)
                {
                    sateltcsSlowUpdate(t[i]);
                    count = 0;
                }
                tcsItarob(0, 0.0, 0.0, ref tar);
                tcsTargup(t[i], ref tar);
                tcsMedium(t[i], roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);

                tcsFast(TRANSFORM + MODEL + TARGET, out jbp, t[i], Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsFast(TRACK, out jbp, t[i], Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsTargup(t[i], ref tar);
                tcsMedium(t[i], roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
                tcsFast(TRANSFORM + MODEL, out jbp, t[i], Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);


                tcsFast(TRACK, out jbp, t[i], Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsTargup(t[i], ref tar);
                tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
                a = a1;
                b = b1;
                positionData[0][i] = slaDranrm(PI - a) / D2R;
                positionData[1][i] = b / D2R;

                tcsAer(t[i], ref tsite, ref m_ast);
                tcsQpor(ref por[ipo], ref tel, ref xim, ref yim);

            }
            positionData = AzConvert(positionData, positionmode);
            return positionData;
        }
        public static double[] GetCurrentUTC_Satel(object[] startTime)
        {
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            double[] mjdValues = new double[8];
            for (int i = 0; i < 8; i++)
            {
                TimeSpan timeSinceMjdEpoch = (DateTime)startTime[i] - mjdEpoch;

                double utcMJD = timeSinceMjdEpoch.TotalDays;
                mjdValues[i] = utcMJD;
            }

            return mjdValues;
        }
        public static double[] GetCurrentUTC_Satel(DateTime currentUTCDate, double currentUTCMillis)
        {
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            // DateTime mjdEpoch = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            DateTime baseTime = currentUTCDate.AddMilliseconds(currentUTCMillis);

            double[] offsets = { -70, -50, -30, -10, 10, 30, 50, 70 };
            double[] mjdValues = new double[offsets.Length];

            for (int i = 0; i < offsets.Length; i++)
            {
                DateTime offsetTime = baseTime.AddMilliseconds(offsets[i]);
                TimeSpan timeSinceMjdEpoch = offsetTime - mjdEpoch;

                double utcMJD = timeSinceMjdEpoch.TotalDays;
                mjdValues[i] = utcMJD;
                //   mjdValues[i] = 60562.116127928239;
            }

            return mjdValues;
        }
        public void sateltcsSlowUpdate()
        {
            roll = 0.0;
            pitch = 0.0;
            jbp = 0;
            double[] aux = new double[MAXAUX];
            for (int index = 0; index < MAXAUX; index++)
            {
                aux[index] = 0.0;
            }
            Ga = 0.0 * AS2R;
            Gb = 0 * AS2R;
            Rma = 0 * D2R;
            Rmav = 0.0;
            Rmat = 0.0;

            // tai = 60605.1094278; // UTC 2024-10-22 02:37:35
            tcsSlow(tai, ref teo, ref tsite);

            double temp = tai - 51544.5;
            double dGmst = (18.697374558 + (24.06570982441908 * temp)) % 24;
            double st0 = (dGmst + (ObservationSiteInfo.LONGITUDE / 15.0)) % 24;

            tsite.st0 = (st0 * 15) * D2R;
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(ALL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            tcsIfld(0.0, 0.0, FOPT.FIELDO, ref fld);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(PA, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
        }
        public void sateltcsSlowUpdate(double tai)
        {
            roll = 0.0;
            pitch = 0.0;
            jbp = 0;
            double[] aux = new double[MAXAUX];
            for (int index = 0; index < MAXAUX; index++)
            {
                aux[index] = 0.0;
            }
            Ga = 0.0 * AS2R;
            Gb = 0 * AS2R;
            Rma = 0 * D2R;
            Rmav = 0.0;
            Rmat = 0.0;

            // tai = 60605.1094278; // UTC 2024-10-22 02:37:35
            tcsSlow(tai, ref teo, ref tsite);

            double temp = tai - 51544.5;
            double dGmst = (18.697374558 + (24.06570982441908 * temp)) % 24;
            double st0 = (dGmst + (ObservationSiteInfo.LONGITUDE / 15.0)) % 24;

            tsite.st0 = (st0 * 15) * D2R;
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(ALL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);
            tcsIfld(0.0, 0.0, FOPT.FIELDO, ref fld);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(PA, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
        }
        int positionmode;
        public int StarPositionSignDecide(double firstposition, double endposition)
        {
            positionmode = 999; // default : 999 error
            if ((int)(firstposition / 270) == 0 && (int)(endposition / 270) ==0)
            {
                positionmode = StarPositionSignMode.Positive;
            }
            else if ((int)(firstposition / 270) ==1 && (int)(endposition / 270) ==0 && 90 > endposition && 270 < firstposition)
            {
                positionmode = StarPositionSignMode.Changed;
            }  
            else if ((int)(firstposition / 270) ==0 && (int)(endposition / 270) ==1 && firstposition < 90 && endposition >270)
            {
                positionmode = StarPositionSignMode.Changed;
            }
            else if ((int)(firstposition / 270) ==1 && (int)(endposition / 270) ==0 && firstposition > 270 && endposition>90)
            {
                positionmode = StarPositionSignMode.Negative;
            }
            else if ((int)(firstposition / 270) ==0 && (int)(endposition / 270) ==1 && firstposition >90 && endposition>270)
            {
                positionmode = StarPositionSignMode.Negative;
            }
            else if ((int)(firstposition / 270) == 1 && (int)(endposition / 270)== 1 )
            {
                positionmode = StarPositionSignMode.Negative;
            }
            return positionmode;
        }
        public int StarPositionSignDecide(double firstposition,double secondposition, double endposition)
        {
            bool sign = true; // + (default) 
            if (firstposition - secondposition < 0) { sign = true; }
            else { sign = false; }
            positionmode = 999; // default : 999 error
            if (sign && (int)(firstposition / 330) == 0 && (endposition > 330 || endposition < 180))
            {
                positionmode = StarPositionSignMode.Changed;
            }
            else if(!sign && (int)(endposition/330)==0 && (firstposition >330 || firstposition < 180))
            {
                positionmode = StarPositionSignMode.Changed;
            }
            else
            {
                positionmode = StarPositionSignMode.Positive;
            }

            return positionmode;
        }
        public double AzConvert_satel(double positionData, int positionmode)
        {
            if (positionmode == StarPositionSignMode.Positive && positionData > 330)
            {
                positionData -= 360;
            }
            else if (positionmode == StarPositionSignMode.Changed && positionData < 30)
            {
            }
            else if (positionmode == StarPositionSignMode.Changed)
            {
                positionData = (positionData > 180 ? positionData - 360 : positionData);

            }
            else
            {
            }
            return positionData;
        }
        public double[][] AzConvert(double[][] positionData ,int positionmode)
        {
            int length = positionData[0].Length;
            if(positionmode == StarPositionSignMode.Positive)
            {
               
            }
            else if(positionmode ==StarPositionSignMode.Negative)
            {
                for (var i = 0; i < length; i++)
                {
                    positionData[0][i] -= 360;
                }
            }
            else if(positionmode == StarPositionSignMode.Changed)
            {
                for (var i = 0; i < length; i++)
                {
                    positionData[0][i] = (positionData[0][i]>270? positionData[0][i]-360: positionData[0][i]);
                }
            }
            else 
            {
                Console.WriteLine("Az error. Please Confirm Star Catalog Data");
            }
            return positionData;
        }
        public double AzConvert(double positionData, int positionmode)
        {
            if (positionmode == StarPositionSignMode.Positive)
            {

            }
            else if (positionmode == StarPositionSignMode.Negative)
            {
                    positionData -= 360;
            }
            else if (positionmode == StarPositionSignMode.Changed)
            {
                    positionData = (positionData > 270 ? positionData - 360 : positionData);
            }
            else
            {
                Console.WriteLine("Az error. Please Confirm Star Catalog Data");
                return -1;
            }
            return positionData;
        }
        public double[][] positionData;
        public static int count = 0;
        public double[][] ObtainTargetTracked(DateTime currentUTCDate, double currentUTCMillis)
        {
            double[] targettimestamps = GetCurrentTaiMjd(currentUTCDate, currentUTCMillis);
            double[] targetposition = new double[2];
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            DateTime offsetTime = currentUTCDate.AddMilliseconds(currentUTCMillis);
            TimeSpan timeSinceMjdEpoch = offsetTime - mjdEpoch;
            TimeSpan additionalSeconds = TimeSpan.FromSeconds(37);
            timeSinceMjdEpoch = timeSinceMjdEpoch.Add(additionalSeconds);
            //  tai = 60605.1094278;
            r_ast = m_ast;
            count++;
            if (count > 100)
            {

                if (StarCalibration_Setting.planetFlag != -9999 && StarCalibration_Setting.planetModeFlag) 
                {
                    tcsItar(StarCalibration_Setting.Ra * D2R, StarCalibration_Setting.Deg * D2R, ref tar);
                } 
                tcsSlowUpdate();
                count = 0;
            }
            tcsTargup(targettimestamps[0], ref tar);

            tcsMedium(targettimestamps[0], roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(TRANSFORM + MODEL + TARGET, out jbp, targettimestamps[0], Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsFast(TRACK, out jbp, targettimestamps[0], Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsTargup(targettimestamps[0], ref tar);
            tcsMedium(targettimestamps[0], roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(TRANSFORM + MODEL, out jbp, targettimestamps[0], Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);


            double a1_deg;
            double b1_deg;
            double[][] positionData = new double[2][];
            for (int i = 0; i < positionData.Length; i++)
            {
                positionData[i] = new double[8];
            }
            for (var i = 0; i < 8; i++)
            {
                t = targettimestamps[i];
                tcsFast(TRACK, out jbp, t, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsTargup(t, ref tar);
                tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
                a = a1;
                b = b1;
                positionData[0][i] = slaDranrm(PI - a) / D2R;
                positionData[1][i] = b / D2R;
                //Console.WriteLine("sky-to-encoders");
                //Console.WriteLine($"timestamp : {t}");
                
                //  Console.WriteLine($"roll : {slaDranrm(PI - roll) / D2R}");
                //  Console.WriteLine($"pitch : {pitch / D2R}");
                //  Console.WriteLine($"Az : {positionData[0][i]}");
                //  Console.WriteLine($"El : {positionData[1][i]}");
                //   ConvertToDMS(positionData[0][i]);
                //  ConvertToDMS(positionData[1][i]);
                 tcsAer(t, ref tsite, ref m_ast);
                 tcsQpor(ref por[ipo], ref tel, ref xim, ref yim);
                
            }
            positionData = AzConvert(positionData, positionmode);
            return positionData;
        }
        public double GetCurrentTaiMjd()
        {
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcTime = DateTime.UtcNow;
            //DateTime utcTime = new DateTime(2024, 11, 4, 2, 24, 0);
            TimeSpan timeSinceMjdEpoch = utcTime - mjdEpoch;

            double utcMJD = timeSinceMjdEpoch.TotalDays;

            return utcMJD;
        }
        public static double[] GetCurrentTaiMjd(DateTime currentUTCDate, double currentUTCMillis)
        {
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
           // DateTime mjdEpoch = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            DateTime baseTime = currentUTCDate.AddMilliseconds(currentUTCMillis);

            double[] offsets = { -70, -50, -30, -10, 10, 30, 50, 70 };
            double[] mjdValues = new double[offsets.Length];

            for (int i = 0; i < offsets.Length; i++)
            {
                DateTime offsetTime = baseTime.AddMilliseconds(offsets[i]);
                TimeSpan timeSinceMjdEpoch = offsetTime - mjdEpoch;

                double utcMJD = timeSinceMjdEpoch.TotalDays;
                mjdValues[i] = utcMJD;
             //   mjdValues[i] = 60562.116127928239;
            }

            return mjdValues;
        }
        static double ConvertTimeToDegrees(int hours, int minutes, int seconds, int microseconds)
        {
            // 1시간 = 15도
            // 1분 = 0.25도
            // 1초 = 0.0041667도
            // 1마이크로초 = 0.0041667 / 1000000 도

            double degreesFromHours = hours * 15.0;
            double degreesFromMinutes = minutes * 0.25;
            double degreesFromSeconds = seconds * 0.0041667;
            double degreesFromMicroseconds = microseconds * (0.0041667 / 1000000.0);

            double totalDegrees = degreesFromHours + degreesFromMinutes + degreesFromSeconds + degreesFromMicroseconds;

            return totalDegrees;
        }
        static double ConvertDMSDegrees(int degrees, int minutes, int seconds, int microseconds)
        {
            // 1도 = 1도
            // 1분 = 1/60도
            // 1초 = 1/3600도
            // 1마이크로초 = 1/3600/1000000도

            double degreesFromMinutes = minutes / 60.0;
            double degreesFromSeconds = seconds / 3600.0;
            double degreesFromMicroseconds = microseconds / 3600000000.0;

            double totalDegrees = degrees + degreesFromMinutes + degreesFromSeconds + degreesFromMicroseconds;

            return totalDegrees;
        }
        public static void ConvertToDMS(double degrees)
        {
            int d = (int)degrees;

            double fractionalPart = Math.Abs(degrees - d);
            double minutesFull = fractionalPart * 60;
            int m = (int)minutesFull;

            double secondsFull = (minutesFull - m) * 60;
            double s = secondsFull;
            Console.WriteLine($"{d}° {m}' {s:0.##}");
        }
        public void SavePointingModel()
        {
            int chorpa;
            int iterm;
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(exePath);
            string fileName = $"pointing_model_{currentTime}.txt"; 
            string filePath = Path.Combine(directory, @"..\..\별보정 결과", fileName);
            filePath = Path.GetFullPath(filePath);
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (iterm = 1; ; iterm++)
                {
                    int result = tcsQterm(iterm, ref pmod, cname, ref cvalue);

                    if (result == 0)
                    {
                        chorpa = char.IsLower(cname[0]) ? '&' : ' ';
                        string formattedCname = chorpa == '&' ? cname.ToString().ToUpper() : cname.ToString();

                        writer.WriteLine($"{formattedCname} {cvalue / AS2R}");
                    }
                    else if (result == -1)
                    {
                        break;
                    }
                }
            }


        }
        public bool ChkElRange(double DecDeg, double elevationMin)
        {
            StarExcute();
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcTime = DateTime.UtcNow;

            TimeSpan timeSinceMjdEpoch = utcTime - mjdEpoch;
            tai = timeSinceMjdEpoch.TotalDays;

            targetposition = new double[2];
            a1 = 0;
            a2 = 0;
            b1 = 0; b2 = 0;
            tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);

            r_ast = m_ast;

            tcsItar(0, DecDeg * D2R, ref tar);

            tcsItarob(0, 0.0, 0.0, ref tar);
            tcsTargup(tai, ref tar);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);

            tcsFast(TRANSFORM + MODEL + TARGET, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsFast(TRACK, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
            tcsTargup(tai, ref tar);
            tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
            tcsFast(TRANSFORM + MODEL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

            tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
            a = a1;
            b = b1;
           
            targetposition[1] = b / D2R;

            Console.WriteLine($"El : {targetposition[1]}");

            if (targetposition[1] > elevationMin) { return true; }
            else { return false; }
        }
        public double[][] FetchAndProcessTrackingData(ObservingSatelliteData satelliteData)
        {
            if (satelliteData == null)
            {
                MessageBox.Show("satelliteData dont exist");
                return null;
            }

            double[][] AzEl = new double[2][];
            for (var j = 0; j < AzEl.Length; j++)
            {
                AzEl[j] = new double[satelliteData.RaDec[j].Length];
            }

            DateTime currentTime = satelliteData.StartTime;
            TimeSpan intervalSpan = TimeSpan.FromMilliseconds(20);

            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);

            int i = 0;
            while (currentTime <= satelliteData.EndTime && i < satelliteData.RaDec[0].Length)
            {
                DateTime utcTime = satelliteData.StartTime.AddMilliseconds(satelliteData.Interval * i*1000);

                TimeSpan timeSinceMjdEpoch = utcTime - mjdEpoch;
                double tai = timeSinceMjdEpoch.TotalDays;

                a1 = 0;
                a2 = 0;
                b1 = 0; b2 = 0;
                tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);

                r_ast = m_ast;

                tcsItar(satelliteData.RaDec[0][i] * D2R, satelliteData.RaDec[1][i] * D2R, ref tar);


                tcsItarob(0, 0.0, 0.0, ref tar);
                tcsTargup(tai, ref tar);
                tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);

                slaDr2tf(4, slaDranrm(tar.p0[0]), sign, i4);
                slaDr2af(3, tar.p0[1], sign, i4);

                slaDr2tf(4, slaDrange(tar.ob[0]), sign, i4);
                slaDr2af(3, tar.ob[1], sign, i4);

                tcsFast(TRANSFORM + MODEL + TARGET, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsFast(TRACK, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsTargup(tai, ref tar);
                tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
                tcsFast(TRANSFORM + MODEL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

                tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
                a = a1;
                b = b1;
                AzEl[0][i] = slaDranrm(PI - a) / D2R;
                AzEl[1][i] = b / D2R;

                tcsAer(tai, ref tsite, ref m_ast);
                tcsQpor(ref por[ipo], ref tel, ref xim, ref yim);

                currentTime += intervalSpan;
                i++;

            }
            return AzEl;
        }
        public double[][] FetchAndProcessTrackingDomeData(ObservingSatelliteData satelliteData)
        {
            if (satelliteData == null)
            {
                MessageBox.Show("satelliteData dont exist");
                return null;
            }

            double[][] AzEl = new double[2][];
            for (var j = 0; j < AzEl.Length; j++)
            {
                AzEl[j] = new double[satelliteData.RaDec[j].Length/5];
            }

            DateTime currentTime = satelliteData.StartTime;
            TimeSpan intervalSpan = TimeSpan.FromMilliseconds(100);

            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);

            int i = 0;
            while (currentTime <= satelliteData.EndTime && i*5 < satelliteData.RaDec[0].Length)
            {
                DateTime utcTime = satelliteData.StartTime.AddMilliseconds(satelliteData.Interval * 5 *i* 1000);

                TimeSpan timeSinceMjdEpoch = utcTime - mjdEpoch;
                double tai = timeSinceMjdEpoch.TotalDays;

                a1 = 0;
                a2 = 0;
                b1 = 0; b2 = 0;
                tcsIast(FRAMETYPE.FK5, 2000.0, 1.0, ref m_ast);

                r_ast = m_ast;

                tcsItar(satelliteData.RaDec[0][i*5] * D2R, satelliteData.RaDec[1][i*5] * D2R, ref tar);


                tcsItarob(0, 0.0, 0.0, ref tar);
                tcsTargup(tai, ref tar);
                tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);

                slaDr2tf(4, slaDranrm(tar.p0[0]), sign, i4);
                slaDr2af(3, tar.p0[1], sign, i4);

                slaDr2tf(4, slaDrange(tar.ob[0]), sign, i4);
                slaDr2af(3, tar.ob[1], sign, i4);

                tcsFast(TRANSFORM + MODEL + TARGET, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsFast(TRACK, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);
                tcsTargup(tai, ref tar);
                tcsMedium(tai, roll, pitch, jbp, ref aux, ref tsite, ref pmod, ref tar, ref tel, ref m_ast, ref r_ast);
                tcsFast(TRANSFORM + MODEL, out jbp, tai, Rmat, Rma, Rmav, ref tsite, ref tel, Ga, Gb, ref tar, ref por[ipo], ref fld, ref m_ast, ref r_ast, ref roll, ref pitch, ref rota);

                tcsVTenc(tar.p[0], tar.p[1], ref m_ast, Rma, roll, pitch, xim, yim, ref tel, Ga, Gb, out a1, out b1, out a2, out b2);
                a = a1;
                b = b1;
                AzEl[0][i] = slaDranrm(PI - a) / D2R;
                AzEl[1][i] = b / D2R;

                tcsAer(tai, ref tsite, ref m_ast);
                tcsQpor(ref por[ipo], ref tel, ref xim, ref yim);

                currentTime += intervalSpan;
                i++;

            }
            return AzEl;
        }
    }
}
