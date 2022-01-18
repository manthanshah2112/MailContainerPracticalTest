using MailContainerTest.App.Models;
using MailContainerTest.Business;
using MailContainerTest.Core.Types;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MailContainerTest.App.Controllers
{
    public class HomeController : Controller
    {
        #region Global Variables
        private readonly ILogger<HomeController> _logger;
        private readonly IMailTransferService _mailTransferService;
        #endregion

        #region Ctor
        public HomeController(ILogger<HomeController> logger,
            IMailTransferService mailTransferService)
        {
            _logger = logger;
            _mailTransferService = mailTransferService;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MailTransfer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MailTransfer(MakeMailTransferRequest request)
        {
            if (ModelState.IsValid)
            {
                MakeMailTransferResult makeMailTransferResult =
                    _mailTransferService.MakeMailTransfer(request);

                if (makeMailTransferResult != null)
                {
                    RedirectToAction("MakeTransferSuccess", makeMailTransferResult);
                }
            }
            else
            {
                _logger.LogError("Something error occurred.");
            }

            return View(request);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}