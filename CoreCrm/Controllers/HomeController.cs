using System.Diagnostics;
using CoreCrm.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrm.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            var viewModel = new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier};

            return View(viewModel);
        }
    }
}