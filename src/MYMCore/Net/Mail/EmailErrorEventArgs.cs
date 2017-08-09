using System;

namespace MYMCore.Net.Mail {
    public class EmailErrorEventArgs : EmailEventArgs {
        public EmailErrorEventArgs(Email email, Exception e) : base(email) {
            Exception = e;
        }

        public Exception Exception { get; }
    }
}
