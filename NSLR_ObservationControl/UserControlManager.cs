using NSLR_ObservationControl.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace NSLR_ObservationControl
{
    public class UserControlManager
    {
        private readonly Form _mainForm;

        public UserControl _newControl;
        
        private Panel _panel;
        private UserControl _currentControl;
        private Observation_TMS tms;
        private CSU_Observation csu_observation;
        private CSU_StarCalibration csu_starcalibration;
        public UserControlManager(Form mainForm)
        {
            _mainForm = mainForm;
            _panel = _mainForm.Controls.Find("panel_Main", true).FirstOrDefault() as Panel;
            _mainForm.KeyDown += new KeyEventHandler(UserControl_KeyDown);
        }
        public void SwitchUserControl(UserControl newControl)
        {
            if (_currentControl != null)
            {
                _panel.Controls.Clear();
            }
            _newControl = newControl;

            _currentControl = newControl;
            _currentControl.Refresh();
            _panel.Controls.Add(_currentControl);
            _panel.Refresh();
            _currentControl.Dock = DockStyle.Fill;

        }
        public interface IKeyControl
        {
            void HandleKeyPress(KeyEventArgs e);
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (_currentControl is IKeyControl keyControl)
            {
                keyControl.HandleKeyPress(e);
                e.Handled = true; // 방향키 입력이 다른 컨트롤로 전달되지 않도록 설정
            }
        }
    }
}
