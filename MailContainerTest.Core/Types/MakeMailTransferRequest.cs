using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MailContainerTest.Core.Types
{
    public class MakeMailTransferRequest
    {
        [DisplayName("Source Mail Container Number")]
        [Required]
        public string? SourceMailContainerNumber { get; set; }

        [DisplayName("Destination Mail Container Number")]
        [Required]
        public string DestinationMailContainerNumber { get; set; }

        [DisplayName("Number Of Mail Items")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int NumberOfMailItems { get; set; }

        [DisplayName("Transfer Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime TransferDate { get; set; }

        [DisplayName("Mail Type")]
        [Required]
        public MailType MailType { get; set; }  
    }
}
