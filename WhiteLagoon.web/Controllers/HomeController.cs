using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.web.Models;
using WhiteLagoon.web.ViewModel;

namespace WhiteLagoon.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public HomeController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;

        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),


            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
