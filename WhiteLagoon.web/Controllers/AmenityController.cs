
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;
using WhiteLagoon.web.ViewModel;

namespace WhiteLagoon.web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public AmenityController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;

        }
        public IActionResult Index()
        {
            var amenities = _unitofWork.Amenity.GetAll(includeProperties:"villa");
            return View(amenities);
        }
        public IActionResult Create()

        {
            AmenityVM amenityVM = new()
            {
                VillaList =_unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            if (ModelState.IsValid )
            {
                _unitofWork.Amenity.Add(obj.Amenity);
                _unitofWork.save();
                TempData["success"] = "The amenities has been Created successfully";

                return RedirectToAction("Index");
            }
            
            obj.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }
        public IActionResult Update(int amenityId)

        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitofWork.Amenity.Get(u => u.Id == amenityId)
            };
            if(amenityVM.Amenity==null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }



        [HttpPost]
        public IActionResult Update( AmenityVM amenityVM)
        {

            if (ModelState.IsValid )
            {
                _unitofWork.Amenity.Update(amenityVM.Amenity);
                _unitofWork.save();
                TempData["success"] = "The amenities has been Updated successfully";

                return RedirectToAction("Index");
            }

            amenityVM.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(amenityVM);
        }
        public IActionResult Delete(int amenityId)

        {

            AmenityVM amenityVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitofWork.Amenity.Get(u => u.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _unitofWork.Amenity
                .Get(u => u.Id == amenityVM.Amenity.Id);


            if (objFromDb is not null)
            {
                _unitofWork.Amenity.Remove(objFromDb);
                _unitofWork.save();
                TempData["success"] = "The amenities has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be deleted ";

            return View();
        }
    }
}
;