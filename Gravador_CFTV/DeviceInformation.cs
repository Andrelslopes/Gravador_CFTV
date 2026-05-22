using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gravador_CFTV.AppEnums;

namespace Gravador_CFTV
{
    public class DeviceInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public AppEnums.DeviceType Type { get; set; }
        public AppEnums.Manufacturer Manufacturer { get; set; }
        public int Channel { get; set; }
        public string Model { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public StreamType Stream { get; set; }  
        public string CustomUrl { get; set; }
    }
}
