using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Net.Mail {
    public class SMTPClientSending : IClientSending {
        private SMTPServerConnection _connection;
        private CredentialType _credentialType;

        public event EventHandler BeforeConnectingServer;

        public event EventHandler BeforeDisposal;

        public event EmailEventHandler BeforeSendingEmail;

        public event EmailEventHandler AfterSendEmail;

        public event EmailErrorEventHandler ErrorOccuredDuringSeding;

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection">info of connection</param>
        /// <param name="credentialType">choose one type to credential</param>
        public SMTPClientSending(SMTPServerConnection connection, CredentialType credentialType) {
            _connection = connection;
            _credentialType = credentialType;

        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public void DeliverToServerQueue() {

        }

        public void Send(IEnumerable<Email> emails) {
            BeforeConnectingServer?.Invoke(this, EventArgs.Empty);

            using (var smtp = GetSMTPClient()) {
                foreach (var item in emails) {
                    try {
                        BeforeSendingEmail?.Invoke(this, new EmailEventArgs(item));

                        smtp.Send(item);

                        AfterSendEmail?.Invoke(this, new EmailEventArgs(item));
                    }
                    catch (Exception e) {
                        ErrorOccuredDuringSeding?.Invoke(this, new EmailErrorEventArgs(item, e));
                        if (ErrorOccuredDuringSeding == null) {
                            throw e;
                        }
                    }
                    finally {
                        item.Dispose();
                    }
                }
                BeforeDisposal?.Invoke(this, EventArgs.Empty);
            }
        }

        private SmtpClient GetSMTPClient() {
            SmtpClient smtp;
            if (_connection.Port.HasValue) {
                smtp = new SmtpClient(_connection.ServerAddress, _connection.Port.Value);
            }
            else {
                smtp = new SmtpClient(_connection.ServerAddress);
            }

            smtp.EnableSsl = _connection.EnableSSL;

            if (_credentialType == CredentialType.WindowUser) {
                smtp.UseDefaultCredentials = true;
            }
            else {
                smtp.Credentials = new NetworkCredential(_connection.UserName, _connection.Password);
            }

            return smtp;
        }
    }
}
