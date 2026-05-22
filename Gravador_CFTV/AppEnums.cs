using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gravador_CFTV
{
    public class AppEnums
    {
        public enum DeviceType
        {
            [Description("Câmera IP")]
            CameraIP,
            [Description("DVR")]
            DVR,
            [Description("NVR")]
            NVR,
            [Description("Other")]
            Other
        }

        public enum Manufacturer
        {
            [Description("Hikvision")]
            Hikvision,
            [Description("Intelbras")]
            Intelbras,
            [Description("Dahua")]
            Dahua,
            [Description("Other")]
            Other
        }

        public enum StreamType
        {
            [Description("Principal")]
            Main,
            [Description("Secundário")]
            Sub
        }
    }
}
