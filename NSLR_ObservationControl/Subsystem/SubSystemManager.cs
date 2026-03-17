using Emgu.CV.Structure;
using log4net;
using mv.impact.acquire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSLR_ObservationControl.Subsystem
{
    public class SubSystemManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<SubSystemManager> _instance = new Lazy<SubSystemManager>(() => new SubSystemManager());
        private static SubSystemManager Instance => _instance.Value;

        private List<ISubSystem> _subSystems;

        private SubSystemManager()
        {
            _subSystems = new List<ISubSystem>
            {
                //리스트(통합 제어용)
                AID_Controller.instance,
                DOM_Controller_v40.instance,
                RGG_SAT_Controller.instance,
                RGG_DEB_Controller.instance,
                LAS_SAT_Controller.instance,
                LAS_DEB_Controller.instance,
            };
        }

        //명시적 제어(개별 제어용)
        public AID_Controller AID => _subSystems.OfType<AID_Controller>().First();
        public DOM_Controller_v40 DOM => _subSystems.OfType<DOM_Controller_v40>().First();
        public RGG_SAT_Controller RGG_SAT => _subSystems.OfType<RGG_SAT_Controller>().First();
        public RGG_DEB_Controller RGG_DEB => _subSystems.OfType<RGG_DEB_Controller>().First();
        public LAS_SAT_Controller LAS_SAT => _subSystems.OfType<LAS_SAT_Controller>().First();
        public LAS_DEB_Controller LAS_DEB => _subSystems.OfType<LAS_DEB_Controller>().First();



        //Static Method for calling from outta this
        public static void SetOperationOne() => Instance.setOperationOne();
        private void setOperationOne()
        {
            log.Info("SetOperationOne 실행");

            var targets = _subSystems.Where(s => (s.Tag == "SLR") || (s.Tag == "ALL"));

            foreach (var s in targets)
            {
                s.doPBIT();
                s.Start();
            }
        }

        public static void SetOperationTwo() => Instance.setOperationTwo();
        private void setOperationTwo()
        {
            log.Info("SetOperationTwo 실행");

            //var targets = _subSystems.Where(s => s.Tag == "LAS" && IsConnected(s));

            //foreach (var s in targets)
            //    s.Start();
        }

        public static void Initialize() => Instance.initialize();
        public void initialize()
        {
            log.Info("Initialize()======================>>>");

            foreach (var subsystem in _subSystems)
            {
                subsystem.Initialize();
            }
        }

        public void DoPBIT()
        {
            foreach (var subsystem in _subSystems)
            {
                subsystem.doPBIT();
            }
        }

        public void Start()
        {
            foreach (var subsystem in _subSystems)
            {
                subsystem.Start();
            }
        }

        public void End()
        {
            foreach (var subsystem in _subSystems)
            {
                subsystem.End();
            }
        }

        public static Dictionary<string, bool> IsConnected()
          => Instance.GetConnectionStatus();

        private Dictionary<string, bool> GetConnectionStatus()
        {
            var statusMap = new Dictionary<string, bool>();

            foreach (var subsystem in _subSystems)
            {
                var name = subsystem.GetType().Name;
                bool isConnected = false;

                var method = subsystem.GetType().GetMethod("IsConnected");
                if (method != null && method.ReturnType == typeof(bool))
                {
                    isConnected = (bool)method.Invoke(subsystem, null);
                }
                else
                {
                    log.Warn($"[{name}] IsConnected() 메서드가 없거나 반환 타입이 bool이 아님");
                }

                statusMap[name] = isConnected;
            }

            return statusMap;
        }



        public List<ISubSystem> GetAllSubSystems()
        {
            return _subSystems;
        }

        public T GetSubsystem<T>() where T : class, ISubSystem
        {
            return _subSystems.OfType<T>().FirstOrDefault();
        }

        public bool getConnectionInfo(string subsystemName)
        {
            // 서브시스템 이름으로 검색
            var subsystem = _subSystems.FirstOrDefault(s =>
                s.GetType().Name.Equals(subsystemName, StringComparison.OrdinalIgnoreCase));

            if (subsystem == null)
            {
                log.Warn($"Subsystem [{subsystemName}] not found");
                return false;
            }

            // 리플렉션을 통해 IsConnected 프로퍼티 확인
            var prop = subsystem.GetType().GetProperty("IsConnected");
            if (prop == null || prop.PropertyType != typeof(bool))
            {
                log.Warn($"Subsystem [{subsystemName}] has no connection status property");
                return false;
            }

            // 연결 상태 반환
            return (bool)prop.GetValue(subsystem);
        }

        public static void domAction() => Instance.DomAction();
        private void DomAction()
        {
            log.Info("SetOperationOne 실행");

            var targets = _subSystems.Where(s => (s.Tag == "SLR") || (s.Tag == "ALL"));

            foreach (var s in targets)
            {
                s.Start();
            }
        }


    }
}