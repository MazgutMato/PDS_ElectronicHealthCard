using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCardApp.Models;
using Microsoft.IdentityModel.Tokens;

namespace EHealthCardApp.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly EHealthCardContext _context;

        public HospitalsController(EHealthCardContext context)
        {
            _context = context;
        }

        // GET: Hospitals
        public async Task<IActionResult> Index()
        {
            return View(new List<Hospital>());
        }
        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("HospitalName,Zip,Capacity")] Hospital hospital)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (hospital.HospitalName.IsNullOrEmpty() && hospital.Zip.IsNullOrEmpty())
            {
                return View("Index", new List<InsuranceComp>());
            }

            if (hospital.Zip.IsNullOrEmpty())
            {
                return View("Index", await _context.Hospitals
                          .Where(i => i.HospitalName == hospital.HospitalName)
                          .ToListAsync());
            }

            if (hospital.HospitalName.IsNullOrEmpty())
            {
                return View("Index", await _context.Hospitals
                          .Where(i => i.Zip == hospital.Zip)
                          .ToListAsync());
            }

            return View("Index", await _context.Hospitals
                          .Where(i => i.Zip == hospital.Zip)
                          .Where(i => i.HospitalName == hospital.HospitalName)
                          .ToListAsync());
        }

        // GET: Hospitals/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Hospitals == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .Include(h => h.ZipNavigation)
                .FirstOrDefaultAsync(m => m.HospitalName == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalName,Zip,Capacity")] Hospital hospital)
        {
            try
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Creation Failed";
                return View(hospital);
            }
        }

        // GET: Hospitals/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Hospitals == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return View(hospital);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HospitalName,Zip,Capacity")] Hospital hospital)
        {
            if (id != hospital.HospitalName)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(hospital);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Message"] = "Data Edition Failed";
                    return View(hospital);
            }
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index)); 
            
        }

        // GET: Hospitals/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Hospitals == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .Include(h => h.ZipNavigation)
                .FirstOrDefaultAsync(m => m.HospitalName == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Hospitals == null)
            {
                return Problem("Entity set 'EHealthCardContext.Hospitals'  is null.");
            }
            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital != null)
            {
                TempData["Message"] = "Data Deleted";
                _context.Hospitals.Remove(hospital);
            }
            else
            {
                TempData["Message"] = "Data Deletion Failed";
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalExists(string id)
        {
          return _context.Hospitals.Any(e => e.HospitalName == id);
        }
    }
}
