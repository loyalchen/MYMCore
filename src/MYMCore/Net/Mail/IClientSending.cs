using System.Collections.Generic;

namespace MYMCore.Net.Mail {
    public interface IClientSending {
        void DeliverToServerQueue();

        void Send(IEnumerable<Email> emails);
    }
}
