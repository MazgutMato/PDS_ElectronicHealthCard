using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;

namespace EHealthCardApp.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly ModelContext _context;

        public InsurancesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Insurances
        public async Task<IActionResult> Index()
        {
            return View(new List<Insurance>());
        }

        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("PersonId,CompId,DateStart,DateEnd")] Insurance insurance)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (String.IsNullOrEmpty(insurance.PersonId) && String.IsNullOrEmpty(insurance.CompId))
            {
                return View("Index", new List<Insurance>());
            }

            if (String.IsNullOrEmpty(insurance.CompId))
            {
                return View("Index", await _context.Insurances
                          .Include(p => p.Comp)
                          .Include(p => p.Person)
                          .Where(i => i.PersonId == insurance.PersonId)
                          .ToListAsync());
            }

            if (String.IsNullOrEmpty(insurance.PersonId))
            {
                return View("Index", await _context.Insurances
                          .Include(p => p.Comp)
                          .Include(p => p.Person)
                          .Where(i => i.CompId == insurance.CompId)
                          .ToListAsync());
            }

            return View("Index", await _context.Insurances
                          .Include(p => p.Comp)
                          .Include(p => p.Person)
                          .Where(i => i.CompId == insurance.CompId)
                          .Where(i => i.PersonId == insurance.PersonId)
                          .ToListAsync());
        }

        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(Insurance p_insurance)
        {
            if (_context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .Include(i => i.Comp)
                .Include(i => i.Person)
                .Where(i => i.PersonId == p_insurance.PersonId)
                .Where(i => i.CompId == p_insurance.CompId)
                .Where(i => i.DateStart == p_insurance.DateStart)
                .FirstOrDefaultAsync();
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Insurances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,CompId,DateStart,DateEnd")] Insurance insurance)
        {
            try
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

            }
            TempData["Message"] = "Data Creation Failed";
            return View(insurance);
        }

        // GET: Insurances/Edit/5
        public async Task<IActionResult> Edit(Insurance p_insurance)
        {
            if (_context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.
                FindAsync(p_insurance.PersonId, p_insurance.CompId, p_insurance.DateStart);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,CompId,DateStart,DateEnd")] Insurance insurance)
        {
            try
            {
                _context.Update(insurance);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

            }
            TempData["Message"] = "Data Edition Failed";
            return View(insurance);
        }

        // GET: Insurances/Delete/5
        public async Task<IActionResult> Delete(Insurance p_insurance)
        {
            if (_context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .Include(i => i.Comp)
                .Include(i => i.Person)
                .Where(i => i.PersonId == p_insurance.PersonId)
                .Where(i => i.CompId == p_insurance.CompId)
                .Where(i => i.DateStart == p_insurance.DateStart)
                .FirstOrDefaultAsync();
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, [Bind("PersonId,CompId,DateStart,DateEnd")] Insurance insurance)
        {
            if (_context.Insurances == null)
            {
                return Problem("Entity set 'EHealthCardContext.Insurances'  is null.");
            }
            if (insurance != null)
            {
                TempData["Message"] = "Data Deleted";
                _context.Insurances.Remove(insurance);
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
