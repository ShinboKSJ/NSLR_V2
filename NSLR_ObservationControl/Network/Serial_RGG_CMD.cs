using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLR_ObservationControl.Network
{
    internal class Serial_RGG_CMD
    {
        public string p_D1_RGGctrl { get; set; }
        public string p_D2_GatePulseWidth { get; set; }
        public string p_D3_GatePulseStartOffset { get; set; }
        public string p_D4_AvoidSetPositionStartOffset { get; set; }
        public string p_D5_AvoidWidth { get; set; }
        public string p_D6_LookupTableUTC { get; set; }
        public string p_D7_LookupTableDelay { get; set; }
    }

}
