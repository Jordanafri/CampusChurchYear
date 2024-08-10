using Microsoft.AspNetCore.Mvc;
using CampusChurch.DataAccess.Data;
using CampusChurch.Models;
using CampusChurch.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using CampusChurch.Utility;
using Microsoft.AspNetCore.Authorization;
using CampusChurch.Models.ViewModels;
using System.Collections.Generic;

namespace CampusChurchWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class SermonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SermonController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Create/Update
        public IActionResult Upsert(int? id)
        {
            SermonVM sermonVM = new()
            {
                SeriesList = _unitOfWork.Series.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Sermon = new Sermon()
            };

            var iconPath = Path.Combine(_webHostEnvironment.WebRootPath, "icons");
            if (Directory.Exists(iconPath))
            {
                sermonVM.ImageList = Directory.GetFiles(iconPath).Select(f => Path.GetFileName(f));
            }
            else
            {
                sermonVM.ImageList = new List<string>(); // Initialize as empty if directory does not exist
            }

            if (id == null || id == 0)
            {
                return View(sermonVM);
            }
            else
            {
                sermonVM.Sermon = _unitOfWork.Sermon.Get(u => u.Id == id);
                if (sermonVM.Sermon == null)
                {
                    return NotFound();
                }
                return View(sermonVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SermonVM sermonVM, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                sermonVM.SeriesList = _unitOfWork.Series.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                sermonVM.ImageList = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "icons"))
                    ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "icons")).Select(f => Path.GetFileName(f))
                    : new List<string>();
                return View(sermonVM);
            }

            var sermon = sermonVM.Sermon;

            // Handle file upload
            if (file != null && file.Length > 0)
            {
                // Ensure the target directory exists
                var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "audio");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                // Delete the old file if it exists and a new file was uploaded
                if (!string.IsNullOrEmpty(sermon.FilePath))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, sermon.FilePath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                sermon.FilePath = "audio/" + file.FileName;
            }
            else if (sermon.Id == 0)
            {
                ModelState.AddModelError("FilePath", "Please upload an audio file.");
                sermonVM.SeriesList = _unitOfWork.Series.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                sermonVM.ImageList = Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "icons"))
                    ? Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "icons")).Select(f => Path.GetFileName(f))
                    : new List<string>();
                return View(sermonVM);
            }

            // Handle image path
            if (string.IsNullOrEmpty(sermon.ImagePath))
            {
                sermon.ImagePath = "default.jpg"; // Default image if none is selected
            }

            // Add or Update the sermon
            if (sermon.Id == 0)
            {
                _unitOfWork.Sermon.Add(sermon);
            }
            else
            {
                var existingSermon = _unitOfWork.Sermon.Get(s => s.Id == sermon.Id);
                if (existingSermon != null)
                {
                    existingSermon.Title = sermon.Title;
                    existingSermon.Description = sermon.Description;
                    existingSermon.Date = sermon.Date;
                    existingSermon.SeriesId = sermon.SeriesId;
                    existingSermon.ImagePath = sermon.ImagePath;

                    // Only update the FilePath if a new file was uploaded
                    if (!string.IsNullOrEmpty(sermon.FilePath))
                    {
                        existingSermon.FilePath = sermon.FilePath;
                    }

                    _unitOfWork.Sermon.Update(existingSermon);
                }
            }
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var sermonList = _unitOfWork.Sermon.GetAll(includeProperties: "Series");
            var result = sermonList.Select(s => new
            {
                s.Id,
                s.Title,
                s.Description,
                s.Date,
                SeriesName = s.Series != null ? s.Series.Name : "N/A",
                s.ImagePath // Include the image path
            });
            return Json(new { data = result });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Error while deleting: ID is null" });
            }

            var sermonToBeDeleted = _unitOfWork.Sermon.Get(u => u.Id == id);
            if (sermonToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting: Sermon not found" });
            }

            if (!string.IsNullOrEmpty(sermonToBeDeleted.FilePath))
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, sermonToBeDeleted.FilePath.TrimStart('\\'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            _unitOfWork.Sermon.Remove(sermonToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Sermon deleted successfully" });
        }

        #endregion
    }
}
