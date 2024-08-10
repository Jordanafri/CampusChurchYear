using Microsoft.AspNetCore.Mvc;
using CampusChurch.DataAccess.Data;
using CampusChurch.Models;
using CampusChurch.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using CampusChurch.Utility;

namespace CampusChurchWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class SeriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Series> objSeriesList = _unitOfWork.Series.GetAll().ToList();
            return View(objSeriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Series obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Series.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Series created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Series? seriesFromDb = _unitOfWork.Series.Get(u => u.Id == id);
            if (seriesFromDb == null)
            {
                return NotFound();
            }
            return View(seriesFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Series obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Series.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Series edited successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Series? seriesFromDb = _unitOfWork.Series.Get(u => u.Id == id);
            if (seriesFromDb == null)
            {
                return NotFound();
            }
            return View(seriesFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Series? obj = _unitOfWork.Series.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Series.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Series deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
