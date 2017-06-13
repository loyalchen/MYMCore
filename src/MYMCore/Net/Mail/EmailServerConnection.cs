using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Net.Mail {
    [Serializable]
    public class SMTPServerConnection {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ServerAddress { get; set; }

        public int? Port { get; set; }

        public bool EnableSSL { get; set; }
    }
}
