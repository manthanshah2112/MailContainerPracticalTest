using MailContainerTest.Service.Data;
using MailContainerTest.Core.Types;
using MailContainerTest.Core.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace MailContainerTest.Business
{
    public class MailTransferService : IMailTransferService
    {
        #region Global Variables
        private readonly AppSettings _appSettings;
        public readonly IServiceProvider _serviceProvider;
        #endregion

        #region Ctor
        public MailTransferService(IOptions<AppSettings> appSettings,
            IServiceProvider serviceProvider)
        {
            _appSettings = appSettings.Value;
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Method 
        public MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request)
        {
            string dataStoreType = _appSettings.DataStoreType;

            var services = _serviceProvider.GetServices<IMailContainerDataStore>();

            IMailContainerDataStore mailContainerDataStore;            
            var result = new MakeMailTransferResult();

            if (dataStoreType == "Backup")
            {
                mailContainerDataStore = services.First(o => o.GetType() == typeof(BackupMailContainerDataStore));
            }
            else
            {
                mailContainerDataStore = services.First(o => o.GetType() == typeof(MailContainerDataStore));

            }

            MailContainer sourceMailContainer = mailContainerDataStore
                .GetMailContainer(request.SourceMailContainerNumber);

            MailContainer destinationMailContainer = mailContainerDataStore
                .GetMailContainer(request.DestinationMailContainerNumber);

            switch (request.MailType)
            {
                case MailType.StandardLetter:
                    if (sourceMailContainer == null || destinationMailContainer == null
                        || !destinationMailContainer.AllowedMailType.HasFlag(AllowedMailType.StandardLetter))
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;

                case MailType.LargeLetter:
                    if (sourceMailContainer == null || destinationMailContainer == null
                        || (!destinationMailContainer.AllowedMailType.HasFlag(AllowedMailType.LargeLetter))
                        || (destinationMailContainer.Capacity < request.NumberOfMailItems))
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;

                case MailType.SmallParcel:
                    if (sourceMailContainer == null || destinationMailContainer == null
                        || !destinationMailContainer.AllowedMailType.HasFlag(AllowedMailType.SmallParcel)
                        || destinationMailContainer.Status != MailContainerStatus.Operational)                    
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;
            }

            if (result.Success)
            {
                sourceMailContainer.Capacity -= request.NumberOfMailItems;
                destinationMailContainer.Capacity += request.NumberOfMailItems;

                // Update Mail Container
                mailContainerDataStore.UpdateMailContainer(sourceMailContainer);
                mailContainerDataStore.UpdateMailContainer(destinationMailContainer);
            }

            return result;
        }
        #endregion
    }
}
