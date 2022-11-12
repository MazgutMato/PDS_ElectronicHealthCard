using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCardApp.Models;

namespace EHealthCardApp.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly EHealthCardContext _context;

        public InsurancesController(EHealthCardContext context)
        {
            _context = context;
        }

        // GET: Insurances
        public async Task<IActionResult> Index()
        {
            var eHealthCardContext = _context.Insurances.Include(i => i.Comp).Include(i => i.Person);
            return View(await eHealthCardContext.ToListAsync());
        }

        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .Include(i => i.Comp)
                .Include(i => i.Person)
                .FirstOrDefaultAsync(m => m.PersonId == id);

            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Insurances/Create
        public IActionResult Create()
        {
            ViewData["CompId"] = new SelectList(_context.InsuranceComps, "CompId", "CompId");
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "PersonId");
            return View();
        }

        // POST: Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,CompId,DateStart,DateEnd")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompId"] = new SelectList(_context.InsuranceComps, "CompId", "CompId", insurance.CompId);
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "PersonId", insurance.PersonId);
            return View(insurance);
        }

        // GET: Insurances/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            ViewData["CompId"] = new SelectList(_context.InsuranceComps, "CompId", "CompId", insurance.CompId);
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "PersonId", insurance.PersonId);
            return View(insurance);
        }

        // POST: Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,CompId,DateStart,DateEnd")] Insurance insurance)
        {
            if (id != insurance.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompId"] = new SelectList(_context.InsuranceComps, "CompId", "CompId", insurance.CompId);
            ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "PersonId", insurance.PersonId);
            return View(insurance);
        }

        // GET: Insurances/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .Include(i => i.Comp)
                .Include(i => i.Person)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Insurances == null)
            {
                return Problem("Entity set 'EHealthCardContext.Insurances'  is null.");
            }
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurances.Remove(insurance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(string id)
        {
          return _context.Insurances.Any(e => e.PersonId == id);
        }
    }
}
