using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;

namespace EHealthCard.Controllers
{
    public class InsuranceCompsController : Controller
    {
        private readonly ModelContext _context;

        public InsuranceCompsController(ModelContext context)
        {
            _context = context;
        }

        // GET: InsuranceComps
        public async Task<IActionResult> Index()
        {
            return View(await _context.InsuranceComps.ToListAsync());
        }

        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("CompId,CompName")] InsuranceComp insuranceComp)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (string.IsNullOrEmpty(insuranceComp.CompId) && string.IsNullOrEmpty(insuranceComp.CompName))
            {
                return View("Index", new List<InsuranceComp>());
            }

            if (string.IsNullOrEmpty(insuranceComp.CompId))
            {
                return View("Index", await _context.InsuranceComps
                          .Where(i => i.CompName == insuranceComp.CompName)
                          .ToListAsync());
            }

            if (string.IsNullOrEmpty(insuranceComp.CompName))
            {
                return View("Index", await _context.InsuranceComps
                          .Where(i => i.CompId == insuranceComp.CompId)
                          .ToListAsync());
            }

            return View("Index", await _context.InsuranceComps
                          .Where(i => i.CompId == insuranceComp.CompId)
                          .Where(i => i.CompName == insuranceComp.CompName)
                          .ToListAsync());
        }

        // GET: InsuranceComps/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.InsuranceComps == null)
            {
                return NotFound();
            }

            var insuranceComp = await _context.InsuranceComps
                .FirstOrDefaultAsync(m => m.CompId == id);
            if (insuranceComp == null)
            {
                return NotFound();
            }

            return View(insuranceComp);
        }

        // GET: InsuranceComps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceComps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompId,CompName")] InsuranceComp insuranceComp)
        {

            try
            {
                _context.Add(insuranceComp);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Creation Failed";
                return View(insuranceComp);
            }

        }

        // GET: InsuranceComps/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.InsuranceComps == null)
            {
                return NotFound();
            }

            var insuranceComp = await _context.InsuranceComps.FindAsync(id);
            if (insuranceComp == null)
            {
                return NotFound();
            }
            return View(insuranceComp);
        }

        // POST: InsuranceComps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CompId,CompName")] InsuranceComp insuranceComp)
        {

            try
            {
                _context.Update(insuranceComp);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Edition Failed";
                return View(insuranceComp);
            }
        }

        // GET: InsuranceComps/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.InsuranceComps == null)
            {
                return NotFound();
            }

            var insuranceComp = await _context.InsuranceComps
                .FirstOrDefaultAsync(m => m.CompId == id);
            if (insuranceComp == null)
            {
                return NotFound();
            }

            return View(insuranceComp);
        }

        // POST: InsuranceComps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                if (_context.InsuranceComps == null)
                {
                    return Problem("Entity set 'EHealthCardContext.InsuranceComps'  is null.");
                }
                var insuranceComp = await _context.InsuranceComps.FindAsync(id);
                if (insuranceComp != null)
                {
                    TempData["Message"] = "Data Deleted";
                    _context.InsuranceComps.Remove(insuranceComp);
                }
                else
                {
                    TempData["Message"] = "Data Deletion Failed";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Data Deletion Failed";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
