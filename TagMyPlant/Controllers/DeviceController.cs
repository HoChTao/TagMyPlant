using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TagMyPlant;
using TagMyPlant.Data;
using TagMyPlant.Models;
using System.IO;

namespace TagMyPlant.Controllers
{
    public class DeviceController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DeviceController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Device> objDeviceList = _db.Devices;
            return View(objDeviceList);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Device obj, IFormFile? imageFile, IFormFile? pdfFileDE, IFormFile? pdfFileGB)
        {
            if (ModelState.IsValid)
            {
                if (IsDeviceExists(obj.Name, obj.Code))
                {
                    ModelState.AddModelError(string.Empty, "Device with the same name or barcode has been registered.");
                    TempData["error"] = "Device with the same name or barcode has been registered";
                    return View(obj);
                }

                // Get device name
                string deviceName = obj.Name;

                //Build the path to the device folder
                string deviceFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", deviceName);

                // Check if the device folder exists, if not create it
                if (!Directory.Exists(deviceFolderPath))
                {
                    Directory.CreateDirectory(deviceFolderPath);
                }

                //Save image file
                if (imageFile != null && imageFile.Length > 0)
                {
                    string imageFileName = Path.GetFileName(imageFile.FileName); // Get the original file name of the uploaded file
                    string imageFilePath = Path.Combine(deviceFolderPath, imageFileName); // Use the uploaded image name as the save file name
                    using (var stream = new FileStream(imageFilePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                    // Update the ImageURL field in the database to the URL of the image
                    obj.ImageURL = "/KVA-datasheets/" + deviceName + "/" + imageFileName; // Set to relative URL
                }

                // Save PDF file - DE
                if (pdfFileDE != null && pdfFileDE.Length > 0)
                {
                    string pdfFileNameDE = Path.GetFileName(pdfFileDE.FileName); // Get the original file name of the uploaded file
                    string pdfFilePathDE = Path.Combine(deviceFolderPath, pdfFileNameDE); // Use the uploaded file name as the save file name
                    using (var stream = new FileStream(pdfFilePathDE, FileMode.Create))
                    {
                        pdfFileDE.CopyTo(stream);
                    }
                    // Update the PdfUrlDE field in the database to the URL of the PDF file
                    obj.PdfUrlDE = "/KVA-datasheets/" + deviceName + "/" + pdfFileNameDE; // Set to relative URL
                }

                // Save PDF file - GB
                if (pdfFileGB != null && pdfFileGB.Length > 0)
                {
                    string pdfFileNameGB = Path.GetFileName(pdfFileGB.FileName); // Get the original file name of the uploaded file
                    string pdfFilePathGB = Path.Combine(deviceFolderPath, pdfFileNameGB); // Use the uploaded file name as the save file name
                    using (var stream = new FileStream(pdfFilePathGB, FileMode.Create))
                    {
                        pdfFileGB.CopyTo(stream);
                    }
                    // Update the PdfUrlGB field in the database to the URL of the PDF file
                    obj.PdfUrlGB = "/KVA-datasheets/" + deviceName + "/" + pdfFileNameGB; // Set to relative URL
                }

                _db.Devices.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Device created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var deviceFromDb = _db.Devices
        .FirstOrDefault(d => d.Id == id);
            if (deviceFromDb == null)
            {
                return NotFound();
            }
            return View(deviceFromDb);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Device obj, IFormFile? imageFile, IFormFile? pdfFileDE, IFormFile? pdfFileGB)
        {
            var deviceFromDb = _db.Devices.FirstOrDefault(d => d.Id == obj.Id);
            if (ModelState.IsValid)
            {
                if (IsDeviceExists(obj.Name, obj.Code))
                {
                    ModelState.AddModelError(string.Empty, "Device with the same name or barcode has been registered.");
                    TempData["error"] = "Device with the same name or barcode has been registet";
                    return View(obj);
                }
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Delete old image files
                    if (!string.IsNullOrEmpty(deviceFromDb.ImageURL))
                    {
                        string oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, deviceFromDb.ImageURL.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    // Save new image file
                    string newImageFileName = Path.GetFileName(imageFile.FileName);
                    string newImageFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name, newImageFileName);
                    using (var stream = new FileStream(newImageFilePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                    obj.ImageURL = "/KVA-datasheets/" + obj.Name + "/" + newImageFileName; // Update image URL
                }
                
                if (pdfFileDE != null && pdfFileDE.Length > 0)
                {
                    // Delete old PDF files - DE
                    if (!string.IsNullOrEmpty(deviceFromDb.PdfUrlDE))
                    {
                        string oldPdfPathDE = Path.Combine(_hostingEnvironment.WebRootPath, deviceFromDb.PdfUrlDE.TrimStart('/'));
                        if (System.IO.File.Exists(oldPdfPathDE))
                        {
                            System.IO.File.Delete(oldPdfPathDE);
                        }
                    }
                    // Save new PDF file - DE
                    string newPdfFileNameDE = Path.GetFileName(pdfFileDE.FileName);
                    string newPdfFilePathDE = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name, newPdfFileNameDE);
                    using (var stream = new FileStream(newPdfFilePathDE, FileMode.Create))
                    {
                        pdfFileDE.CopyTo(stream);
                    }
                    obj.PdfUrlDE = "/KVA-datasheets/" + obj.Name + "/" + newPdfFileNameDE; // Update PDF URL (DE)
                }
               
                if (pdfFileGB != null && pdfFileGB.Length > 0)
                {
                    // Delete old PDF files - GB
                    if (!string.IsNullOrEmpty(deviceFromDb.PdfUrlGB))
                    {
                        string oldPdfPathGB = Path.Combine(_hostingEnvironment.WebRootPath, deviceFromDb.PdfUrlGB.TrimStart('/'));
                        if (System.IO.File.Exists(oldPdfPathGB))
                        {
                            System.IO.File.Delete(oldPdfPathGB);
                        }
                    }
                    // Save new PDF file - GB
                    string newPdfFileNameGB = Path.GetFileName(pdfFileGB.FileName);
                    string newPdfFilePathGB = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name, newPdfFileNameGB);
                    using (var stream = new FileStream(newPdfFilePathGB, FileMode.Create))
                    {
                        pdfFileGB.CopyTo(stream);
                    }
                    obj.PdfUrlGB = "/KVA-datasheets/" + obj.Name + "/" + newPdfFileNameGB; // Update PDF URL (GB)
                }

                //Update device information in the database
                _db.Devices.Update(obj);
                _db.SaveChanges();

                TempData["success"] = "Device updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var deviceFromDb = _db.Devices
        .FirstOrDefault(d => d.Id == id);
            if (deviceFromDb == null)
            {
                return NotFound();
            }
            return View(deviceFromDb);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Devices.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            // Get the path of the device folder
            string deviceFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name);

            // Delete the device folder and all its files
            if (Directory.Exists(deviceFolderPath))
            {
                Directory.Delete(deviceFolderPath, true); //The second parameter indicates recursive deletion of the folder and its contents
            }

            _db.Devices.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Device deleted successfully";
            return RedirectToAction("Index");
        }

        public bool IsDeviceExists(string name, string code)
        {
            // Query the database to check if there is a device with the same Name or the same Code
            return _db.Devices.Any(d => d.Name == name || d.Code == code);
        }

    }
}
