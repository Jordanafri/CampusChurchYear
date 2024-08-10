using Microsoft.AspNetCore.Mvc;
using CampusChurch.DataAccess.Repository.IRepository;
using CampusChurch.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CampusChurchWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [AllowAnonymous]
    public class SermonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<SermonController> _logger;

        public SermonController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ILogger<SermonController> logger)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Download(int id)
        {
            var sermon = _unitOfWork.Sermon.GetFirstOrDefault(s => s.Id == id);
            if (sermon == null)
            {
                return NotFound();
            }

            var relativePath = sermon.FilePath.TrimStart('\\');
            var path = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

            _logger.LogInformation($"Constructed file path: {path}");

            if (!System.IO.File.Exists(path))
            {
                _logger.LogWarning($"File not found: {path}");
                return NotFound($"The file {path} does not exist.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetData([FromQuery] DataTableRequest request)
        {
            if (request == null)
            {
                _logger.LogError("DataTableRequest is null.");
                return BadRequest("Request cannot be null.");
            }

            var query = _unitOfWork.Sermon.GetAll(includeProperties: "Series").AsQueryable();

            // Apply filtering
            if (request.Search != null && !string.IsNullOrEmpty(request.Search.Value))
            {
                query = query.Where(s => s.Title.Contains(request.Search.Value) || s.Description.Contains(request.Search.Value));
            }

            // Apply sorting
            if (request.Order != null && request.Order.Any() && request.Columns != null && request.Columns.Any())
            {
                var sortColumn = request.Columns[request.Order[0].Column].Data;
                var sortDirection = request.Order[0].Dir;
                query = sortDirection == "asc" ? query.OrderBy(s => EF.Property<object>(s, sortColumn)) : query.OrderByDescending(s => EF.Property<object>(s, sortColumn));
            }
            else
            {
                _logger.LogWarning("Sorting parameters are missing or incomplete.");
            }

            // Apply pagination
            var data = query.Skip(request.Start).Take(request.Length).ToList();
            var totalRecords = query.Count();

            return Json(new { data = data, recordsTotal = totalRecords, recordsFiltered = totalRecords });
        }

    }
}


//using System;
//using System.Linq;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions;
//using CampusChurch.DataAccess.Repository.IRepository;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Authorization;
//using System.IO;

//namespace CampusChurchWeb.Areas.Customer.Controllers
//{
//    [Area("Customer")]
//    [AllowAnonymous]
//    public class SermonController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        private readonly ILogger<SermonController> _logger;

//        public SermonController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ILogger<SermonController> logger)
//        {
//            _unitOfWork = unitOfWork;
//            _webHostEnvironment = webHostEnvironment;
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [AllowAnonymous]
//        public IActionResult Download(int id)
//        {
//            var sermon = _unitOfWork.Sermon.GetFirstOrDefault(s => s.Id == id);
//            if (sermon == null)
//            {
//                return NotFound();
//            }

//            var relativePath = sermon.FilePath.TrimStart('\\');
//            var path = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

//            _logger.LogInformation($"Constructed file path: {path}");

//            if (!System.IO.File.Exists(path))
//            {
//                _logger.LogWarning($"File not found: {path}");
//                return NotFound($"The file {path} does not exist.");
//            }

//            var fileBytes = System.IO.File.ReadAllBytes(path);
//            return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult GetAll()
//        {
//            var sermonList = _unitOfWork.Sermon.GetAll(includeProperties: "Series");
//            var result = sermonList.Select(s => new
//            {
//                s.Id,
//                s.Title,
//                s.Description,
//                s.Date,
//                SeriesName = s.Series != null ? s.Series.Name : "N/A",
//                s.ImagePath // Include the image path
//            });
//            return Json(new { data = result });
//        }



//    }
//}
