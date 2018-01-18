using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WindowsManufacturingCloud.Models;

namespace WindowsManufacturingCloud.Controllers
{
    public class ImageController : Controller
    {
        public WdsApiConfig WdsApiConfig { get; }

        public ImageController(IOptions<WdsApiConfig> wdsApiConfig)
        {
            WdsApiConfig = wdsApiConfig.Value;
        }
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
            return View(new ClientPulseViewModel() { SocketIOClientUrl = WdsApiConfig.SocketIOClientUrl });
        }
    }
}