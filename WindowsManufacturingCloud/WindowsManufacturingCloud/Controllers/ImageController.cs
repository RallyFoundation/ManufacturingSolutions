using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WindowsManufacturingCloud.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Install()
        {
            return View();
        }

        public IActionResult Boot()
        {
            return View();
        }

        public IActionResult Group()
        {
            return View();
        }

        public IActionResult File()
        {
            return View();
        }

        public IActionResult Lookup()
        {
            return View();
        }

        public IActionResult LogHistory()
        {
            return View();
        }

        public IActionResult ClientPulse()
        {
            return View();
        }
    }
}