using MailContainerTest.Core.Types;

namespace MailContainerTest.Business
{
    public interface IMailTransferService
    {
        MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request);
    }
}