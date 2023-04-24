using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDengine.WebClient
{
    public class ConnectionConfiguration
    {
        public string? Host { get; set; }
        public string? Username { get; set; } = "root";
        public string? Password { get; set; } = "taosdata";
        public string? Database { get; set; }
    }
}
