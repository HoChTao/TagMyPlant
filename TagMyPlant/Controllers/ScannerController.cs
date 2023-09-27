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
            if (scandata != null && System.Text.RegularExpressions.Regex.IsMatch(scandata, @"^\d+$"))
            {
                var deviceFromDb = _db.Devices
                    .FirstOrDefault(d => d.Code == scandata);

                if (deviceFromDb == null)
                {
                    return BadRequest("Device with barcode"+ " " + scandata + " " + "is not found!");
                }

                return View(deviceFromDb);
            }
            else
            {
                return BadRequest("Invalid input. Please enter a valid integer.");
            }
        }
    }
}
