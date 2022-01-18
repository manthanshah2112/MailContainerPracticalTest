using MailContainerTest.Core.Types;

namespace MailContainerTest.Service.Data
{
    public interface IMailContainerDataStore
    {
        MailContainer GetMailContainer(string mailContainerNumber);

        void UpdateMailContainer(MailContainer mailContainer);
    }
}
