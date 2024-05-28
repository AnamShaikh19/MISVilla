
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;
using WhiteLagoon.web.ViewModel;

namespace WhiteLagoon.web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public VillaNumberController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;

        }
        public IActionResult Index()
        {
            var villaNumbers = _unitofWork.VillaNumber.GetAll(includeProperties:"villa");
            return View(villaNumbers);
        }
        public IActionResult Create()

        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList =_unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _unitofWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitofWork.VillaNumber.Add(obj.VillaNumber);
                _unitofWork.save();
                TempData["success"] = "The villa has been Created successfully";

                return RedirectToAction("Index");
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa number already exists";

            }
            obj.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }
        public IActionResult Update(int VillaNumberId)

        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitofWork.VillaNumber.Get(u => u.Villa_Number == VillaNumberId)
            };
            if(villaNumberVM.VillaNumber==null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }



        [HttpPost]
        public IActionResult Update( VillaNumberVM villaNumberVM)
        {

            if (ModelState.IsValid )
            {
                _unitofWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitofWork.save();
                TempData["success"] = "The villa has been Updated successfully";

                return RedirectToAction("Index");
            }

            villaNumberVM.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }
        public IActionResult Delete(int VillaNumberId)

        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitofWork.VillaNumber.Get(u => u.Villa_Number == VillaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _unitofWork.VillaNumber
                .Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);


            if (objFromDb is not null)
            {
                _unitofWork.VillaNumber.Remove(objFromDb);
                _unitofWork.save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be deleted ";

            return View();
        }
    }
}
;