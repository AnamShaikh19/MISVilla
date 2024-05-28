
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastracture.Data;

namespace WhiteLagoon.web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitofWork.Villa.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images\VillaImage");
                    using var fileStream = new FileStream(Path.Combine(imagePath,fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl =@"\images\VillaImage\"+ fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600*400";
                }
               _unitofWork.Villa.Add(obj);
                _unitofWork.save();
                TempData["success"] = "The villa has been Created successfully";

                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Update(int VillaId)

        {
            //Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == VillaId);
            //var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);


            Villa? obj = _unitofWork.Villa.Get(u=>u.Id == VillaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id>0)
            {
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images\VillaImage");
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.Trim('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
              
                _unitofWork.Villa.Update(obj);
                _unitofWork.save();
                TempData["success"] = "The villa has been Updated successfully";

                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int VillaId)

        {
            
            Villa? obj = _unitofWork.Villa.Get(u => u.Id == VillaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitofWork.Villa.Get(u => u.Id == obj.Id);

            if (objFromDb is not null)
            {
                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.Trim('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitofWork.Villa.Remove(objFromDb);
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