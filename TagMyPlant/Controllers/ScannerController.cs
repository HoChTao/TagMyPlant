using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TagMyPlant;
using TagMyPlant.Data;

namespace TagMyPlantWeb.Controllers
{
    public class ScannerController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        public ScannerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Scan()
        {            
            return View();
        }

        public IActionResult ScanResult(string scandata)
        {
            if (scandata != null)
            {
                // 现在 'id' 包含了从字符串 'scandata' 转换而来的整数值
                var deviceFromDb = _db.Devices
                    .FirstOrDefault(d => d.Code == scandata);

                if (deviceFromDb == null)
                {
                    return NotFound();
                }

                return View(deviceFromDb);
            }
            else
            {
                // 无法将 'scandata' 转换为整数，显示错误或执行其他操作
                return BadRequest("Invalid input. Please enter a valid integer.");
            }
        }
    }
}
