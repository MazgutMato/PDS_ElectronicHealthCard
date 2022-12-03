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
    public class DiagnosesTypesController : Controller
    {
        private readonly ModelContext _context;

        public DiagnosesTypesController(ModelContext context)
        {
            _context = context;
        }

        // GET: DiagnosesTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.DiagnosesTypes.ToListAsync());
        }

        // GET: DiagnosesTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DiagnosesTypes == null)
            {
                return NotFound();
            }

            var diagnosesType = await _context.DiagnosesTypes
                .FirstOrDefaultAsync(m => m.DiagnosisId == id);
            if (diagnosesType == null)
            {
                return NotFound();
            }

            return View(diagnosesType);
        }

        // GET: DiagnosesTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DiagnosesTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiagnosisId,Description,DailyCosts")] DiagnosesType diagnosesType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosesType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diagnosesType);
        }

        // GET: DiagnosesTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DiagnosesTypes == null)
            {
                return NotFound();
            }

            var diagnosesType = await _context.DiagnosesTypes.FindAsync(id);
            if (diagnosesType == null)
            {
                return NotFound();
            }
            return View(diagnosesType);
        }

        // POST: DiagnosesTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DiagnosisId,Description,DailyCosts")] DiagnosesType diagnosesType)
        {
            if (id != diagnosesType.DiagnosisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosesType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosesTypeExists(diagnosesType.DiagnosisId))
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
            return View(diagnosesType);
        }

        // GET: DiagnosesTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DiagnosesTypes == null)
            {
                return NotFound();
            }

            var diagnosesType = await _context.DiagnosesTypes
                .FirstOrDefaultAsync(m => m.DiagnosisId == id);
            if (diagnosesType == null)
            {
                return NotFound();
            }

            return View(diagnosesType);
        }

        // POST: DiagnosesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DiagnosesTypes == null)
            {
                return Problem("Entity set 'ModelContext.DiagnosesTypes'  is null.");
            }
            var diagnosesType = await _context.DiagnosesTypes.FindAsync(id);
            if (diagnosesType != null)
            {
                _context.DiagnosesTypes.Remove(diagnosesType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosesTypeExists(string id)
        {
          return _context.DiagnosesTypes.Any(e => e.DiagnosisId == id);
        }
    }
}
