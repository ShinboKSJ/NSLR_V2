using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLR_ObservationControl.Subsystem
{
    public interface ISubSystem
    {
        string Tag { get; } // SLR , DLT
        bool IsInitialized { get; }
        void Initialize();
        bool IsConnected();

        void doPBIT();

        void Start();
        void End();
        //string Anomaly_Check();
        //void ExecuteCommand(string command);
    }
}
