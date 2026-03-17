using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Util
{
    internal class DisplayResolutionSetting 
    {

        #region Screen Resolution refer
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, ChangeDisplaySettingsFlags flags);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public enum ChangeDisplaySettingsFlags : uint
        {
            CDS_UPDATEREGISTRY = 0x01,
            CDS_TEST = 0x02,
            CDS_FULLSCREEN = 0x04,
            CDS_GLOBAL = 0x08,
            CDS_SET_PRIMARY = 0x10,
            CDS_VIDEOPARAMETERS = 0x20,
            CDS_RESET = 0x40000000,
            CDS_NORESET = 0x1000000
        }

        public enum ScreenOrientation
        {
            DMDO_DEFAULT = 0,
            DMDO_90 = 1,
            DMDO_180 = 2,
            DMDO_270 = 3
        }
        public enum SCREEN
        {
            NORMAL = 0,
            HIGH_4K = 1,
        }

        private DEVMODE originalDevMode;
        private DEVMODE devMode;
        #endregion Screen Reolsution refer

        #region Screen Scale refer
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("SHCore.dll", SetLastError = true)]
        public static extern bool SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetDpiForSystem();

        public enum PROCESS_DPI_AWARENESS
        {
            ProcessDPIUnaware = 0,
            ProcessSystemDPIAware = 1,
            ProcessPerMonitorDPIAware = 2
        }

        private int originalDpi;
        private float originalDpiX;
        private float originalDpiY;

        #endregion Screen scale refer

        public void DisplaySettings()
        {
            // 현재 디스플레이 설정 저장
            originalDevMode = GetCurrentDevMode();
            Console.WriteLine($"0. 시작 해상도: {originalDevMode.dmPelsWidth}x{originalDevMode.dmPelsHeight}");

            // 프로그램 시작 시 4K 해상도로 변경
            SetResolution(SCREEN.HIGH_4K);

#if FUNCTION_SCALE
                originalDpiX = this.DeviceDpi;
                originalDpiY = this.DeviceDpi;

                // 폼 크기 및 컨트롤 크기 조절
                AdjustFormAndControls(originalDpiX, originalDpiY);


                // 프로그램 시작 시 DPI 저장
                originalDpi = GetDpiForSystem();
                // DPI 설정을 변경하려면 먼저 DPI Awareness를 설정해야 합니다.
                SetDpiAwareness(PROCESS_DPI_AWARENESS.ProcessPerMonitorDPIAware);
#endif
        }

#if FUNCTION_SCALE
            #region Screen Scale part
            private void SetDpiAwareness(PROCESS_DPI_AWARENESS awareness)
            {
                // SetProcessDpiAwareness 함수를 사용하여 DPI Awareness를 설정
                SetProcessDpiAwareness(awareness);
            }


            private void AdjustFormAndControls(float dpiX, float dpiY)
            {
                // DPI에 따라 크기를 조절
                float scaleX = dpiX / 96.0f;
                float scaleY = dpiY / 96.0f;

                this.Scale(new SizeF(scaleX, scaleY));

                // 모든 컨트롤 크기를 조절
                AdjustControlSizes(this.Controls, scaleX, scaleY);

                Console.WriteLine($"DPI 변경: {dpiX}x{dpiY}");
            }

            private void AdjustControlSizes(Control.ControlCollection controls, float scaleX, float scaleY)
            {
                foreach (Control control in controls)
                {
                    // 컨트롤 크기 조절
                    control.Width = (int)(control.Width * scaleX);
                    control.Height = (int)(control.Height * scaleY);

                    // 컨트롤 내부의 컨트롤들도 재귀적으로 크기 조절
                    if (control.Controls.Count > 0)
                    {
                        AdjustControlSizes(control.Controls, scaleX, scaleY);
                    }
                }
            }

            
            private void SetDpi(int dpi)
            {
                // DPI에 따라 크기를 조절
                float scaleX = dpi / 100.0f;
                float scaleY = dpi / 100.0f;

                this.Scale(new SizeF(scaleX, scaleY));

                // 모든 컨트롤 크기를 조절
                AdjustControlSizes(this.Controls, scaleX, scaleY);

                Console.WriteLine($"DPI 변경: {dpi}%");
            }           
            #endregion Screen Scale part
#endif

        private void SetResolution(SCREEN resolution)
        {
            DEVMODE devMode = GetCurrentDevMode();
            Console.WriteLine($"1. 현재 해상도: {devMode.dmPelsWidth}x{devMode.dmPelsHeight}");

            if (resolution == SCREEN.HIGH_4K)
            {
                devMode.dmPelsWidth = 3840;
                devMode.dmPelsHeight = 2160;

            }
            else if (resolution == SCREEN.NORMAL)
            {
                devMode.dmPelsWidth = 1920;
                devMode.dmPelsHeight = 1080;
            }
            ChangeDisplaySettingsEx(null, ref devMode, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);
            Console.WriteLine($"2. 해상도 4K로 변경: {devMode.dmPelsWidth}x{devMode.dmPelsHeight}");
        }

        public void RestoreDisplaySetting()
        {
            devMode.dmPelsWidth = 1920;
            devMode.dmPelsHeight = 1080;
            ChangeDisplaySettings(ref originalDevMode, ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY);
            //ChangeDisplaySettings(ref devMode, ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY);
            Console.WriteLine($"4. 해상도 복원: {devMode.dmPelsWidth}x{devMode.dmPelsHeight}");
        }

        private DEVMODE GetCurrentDevMode()
        {
            DEVMODE devMode = new DEVMODE();
            devMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(null, -1, ref devMode);
            return devMode;
        }

        private void DisplaySetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            RestoreDisplaySetting();
#if FUCNTION_SCALE
                // 폼이 닫힐 때 원래 DPI로 복원
                SetDpi(125);
#endif
        }
    }

}
