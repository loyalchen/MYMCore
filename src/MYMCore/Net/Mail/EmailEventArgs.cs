using System;

namespace MYMCore.Net.Mail {
    public class EmailEventArgs : EventArgs {
        public Email CurrentEmail { get; set; }

        public EmailEventArgs(Email email) {
            CurrentEmail = email;
        }
    }
}
