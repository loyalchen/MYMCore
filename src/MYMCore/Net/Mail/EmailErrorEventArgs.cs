using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Net.Mail {
    public class EmailErrorEventArgs : EmailEventArgs {
        private Exception _exception;

        public EmailErrorEventArgs(Email email, Exception e) : base(email) {
            _exception = e;
        }

        public Exception Exception => _exception;
    }
}
