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
    public class HospitalizationsController : Controller
    {
        private readonly EHealthCardContext _context;

        public HospitalizationsController(EHealthCardContext context)
        {
            _context = context;
        }

        // GET: Hospitalizations
        public async Task<IActionResult> Index()
        {
            return View(new List<Hospitalization>());
        }
        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (hospitalization.PersonId.IsNullOrEmpty() && hospitalization.HospitalName.IsNullOrEmpty())
            {
                return View("Index", new List<Hospitalization>());
            }

            if (hospitalization.HospitalName.IsNullOrEmpty())
            {
                return View("Index", await _context.Hospitalizations
                          .Include(p => p.Person)
                          .Include(p => p.HospitalNameNavigation)
                          .Where(i => i.PersonId == hospitalization.PersonId)
                          .ToListAsync());
            }

            if (hospitalization.PersonId.IsNullOrEmpty())
            {
                return View("Index", await _context.Hospitalizations
                          .Include(p => p.Person)
                          .Include(p => p.HospitalNameNavigation)
                          .Where(i => i.HospitalName == hospitalization.HospitalName)
                          .ToListAsync());
            }

            return View("Index", await _context.Hospitalizations
                          .Include(p => p.Person)
                          .Include(p => p.HospitalNameNavigation)
                          .Where(i => i.PersonId == hospitalization.PersonId)
                          .Where(i => i.HospitalName == hospitalization.HospitalName)
                          .ToListAsync());
        }

        // GET: Hospitalizations/Details/5
        public async Task<IActionResult> Details(Hospitalization p_hospitalization)
        {
            if (_context.Hospitalizations == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                .Include(h => h.HospitalNameNavigation)
                .Include(h => h.Person)
                .Where(i => i.PersonId == p_hospitalization.PersonId)
                .Where(i => i.HospitalName == p_hospitalization.HospitalName)
                .Where(i => i.DateStart == p_hospitalization.DateStart)
                .FirstOrDefaultAsync();
            if (hospitalization == null)
            {
                return NotFound();
            }

            return View(hospitalization);
        }

        // GET: Hospitalizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitalizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            try
            {
                _context.Add(hospitalization);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View(hospitalization);
        }

        // GET: Hospitalizations/Edit/5
        public async Task<IActionResult> Edit(Hospitalization p_hospitalization)
        {
            if (_context.Hospitalizations == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                .FindAsync(p_hospitalization.DateStart, p_hospitalization.HospitalName, p_hospitalization.PersonId);
            if (hospitalization == null)
            {
                return NotFound();
            }
            return View(hospitalization);
        }

        // POST: Hospitalizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            try
            {
                _context.Update(hospitalization);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

            }

            TempData["Message"] = "Data Edition Failed";
            return View(hospitalization);
        }

        // GET: Hospitalizations/Delete/5
        public async Task<IActionResult> Delete(Hospitalization p_hospitalization)
        {
            if (_context.Hospitalizations == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                    .Include(h => h.HospitalNameNavigation)
                    .Include(h => h.Person)
                    .Where(i => i.PersonId == p_hospitalization.PersonId)
                    .Where(i => i.HospitalName == p_hospitalization.HospitalName)
                    .Where(i => i.DateStart == p_hospitalization.DateStart)
                    .FirstOrDefaultAsync();
            if (hospitalization == null)
            {
                return NotFound();
            }

            return View(hospitalization);
        }

        // POST: Hospitalizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id, [Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            if (_context.Hospitalizations == null)
            {
                return Problem("Entity set 'EHealthCardContext.Hospitalizations'  is null.");
            }

            if (hospitalization != null)
            {
                TempData["Message"] = "Data Deleted";
                _context.Hospitalizations.Remove(hospitalization);
            }    
            else
            {
                TempData["Message"] = "Data Deletion Failed";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
