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
            [Description("Outro")]
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
            [Description("Outro")]
            Other
        }

        public enum StreamType
        {
            [Description("Main Stream")]
            Main,
            [Description("Sub Stream")]
            Sub
        }
    }
}
