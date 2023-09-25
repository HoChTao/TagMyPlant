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

                // 获取设备名称
                string deviceName = obj.Name;

                // 构建设备文件夹的路径
                string deviceFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", deviceName);

                // 检查设备文件夹是否存在，如果不存在则创建它
                if (!Directory.Exists(deviceFolderPath))
                {
                    Directory.CreateDirectory(deviceFolderPath);
                }

                // 保存图片文件
                if (imageFile != null && imageFile.Length > 0)
                {
                    string imageFileName = Path.GetFileName(imageFile.FileName); // 获取上传文件的原始文件名
                    string imageFilePath = Path.Combine(deviceFolderPath, imageFileName); // 假设图片扩展名为jpg
                    using (var stream = new FileStream(imageFilePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                    // 更新数据库中的ImageURL字段为图片的URL
                    obj.ImageURL = "/KVA-datasheets/" + deviceName + "/" + imageFileName; // 设置为相对URL
                }

                // 保存PDF文件 - DE
                if (pdfFileDE != null && pdfFileDE.Length > 0)
                {
                    string pdfFileNameDE = Path.GetFileName(pdfFileDE.FileName); // 获取上传文件的原始文件名
                    string pdfFilePathDE = Path.Combine(deviceFolderPath, pdfFileNameDE); // 使用上传的文件名作为保存文件名
                    using (var stream = new FileStream(pdfFilePathDE, FileMode.Create))
                    {
                        pdfFileDE.CopyTo(stream);
                    }
                    // 更新数据库中的PdfUrlDE字段为PDF文件的URL
                    obj.PdfUrlDE = "/KVA-datasheets/" + deviceName + "/" + pdfFileNameDE; // 设置为相对URL
                }

                // 保存PDF文件 - GB
                if (pdfFileGB != null && pdfFileGB.Length > 0)
                {
                    string pdfFileNameGB = Path.GetFileName(pdfFileGB.FileName); // 获取上传文件的原始文件名
                    string pdfFilePathGB = Path.Combine(deviceFolderPath, pdfFileNameGB); // 使用上传的文件名作为保存文件名
                    using (var stream = new FileStream(pdfFilePathGB, FileMode.Create))
                    {
                        pdfFileGB.CopyTo(stream);
                    }
                    // 更新数据库中的PdfUrlGB字段为PDF文件的URL
                    obj.PdfUrlGB = "/KVA-datasheets/" + deviceName + "/" + pdfFileNameGB; // 设置为相对URL
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
                    TempData["error"] = "Device with the same name or barcode has been registet";
                    return View(obj);
                }
                if (imageFile != null && imageFile.Length > 0)
                {
                    // 删除旧图片文件
                    if (!string.IsNullOrEmpty(deviceFromDb.ImageURL))
                    {
                        string oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, deviceFromDb.ImageURL.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    // 保存新图片文件
                    string newImageFileName = Path.GetFileName(imageFile.FileName);
                    string newImageFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name, newImageFileName);
                    using (var stream = new FileStream(newImageFilePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                    obj.ImageURL = "/KVA-datasheets/" + obj.Name + "/" + newImageFileName; // 更新图片URL
                }
                
                // 保存新PDF文件 - DE
                if (pdfFileDE != null && pdfFileDE.Length > 0)
                {
                    // 删除旧PDF文件 - DE
                    if (!string.IsNullOrEmpty(deviceFromDb.PdfUrlDE))
                    {
                        string oldPdfPathDE = Path.Combine(_hostingEnvironment.WebRootPath, deviceFromDb.PdfUrlDE.TrimStart('/'));
                        if (System.IO.File.Exists(oldPdfPathDE))
                        {
                            System.IO.File.Delete(oldPdfPathDE);
                        }
                    }

                    string  newPdfFileNameDE = Path.GetFileName(pdfFileDE.FileName);
                    string newPdfFilePathDE = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name, newPdfFileNameDE);
                    using (var stream = new FileStream(newPdfFilePathDE, FileMode.Create))
                    {
                        pdfFileDE.CopyTo(stream);
                    }
                    obj.PdfUrlDE = "/KVA-datasheets/" + obj.Name + "/" + newPdfFileNameDE; // 更新PDF URL (DE)
                }

                // 保存新PDF文件 - GB
                if (pdfFileGB != null && pdfFileGB.Length > 0)
                {
                    // 删除旧PDF文件 - GB
                    if (!string.IsNullOrEmpty(deviceFromDb.PdfUrlGB))
                    {
                        string oldPdfPathGB = Path.Combine(_hostingEnvironment.WebRootPath, deviceFromDb.PdfUrlGB.TrimStart('/'));
                        if (System.IO.File.Exists(oldPdfPathGB))
                        {
                            System.IO.File.Delete(oldPdfPathGB);
                        }
                    }

                    string newPdfFileNameGB = Path.GetFileName(pdfFileGB.FileName);
                    string newPdfFilePathGB = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name, newPdfFileNameGB);
                    using (var stream = new FileStream(newPdfFilePathGB, FileMode.Create))
                    {
                        pdfFileGB.CopyTo(stream);
                    }
                    obj.PdfUrlGB = "/KVA-datasheets/" + obj.Name + "/" + newPdfFileNameGB; // 更新PDF URL (GB)
                }

                // 更新数据库中的设备信息
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

            // 获取设备文件夹的路径
            string deviceFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "KVA-datasheets", obj.Name);

            // 删除设备文件夹及其所有文件
            if (Directory.Exists(deviceFolderPath))
            {
                Directory.Delete(deviceFolderPath, true); // 第二个参数表示递归删除文件夹及其内容
            }

            _db.Devices.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Device deleted successfully";
            return RedirectToAction("Index");
        }

        public bool IsDeviceExists(string name, string code)
        {
            // 查询数据库以检查是否存在具有相同Name或相同Code的设备
            return _db.Devices.Any(d => d.Name == name || d.Code == code);
        }

    }
}
