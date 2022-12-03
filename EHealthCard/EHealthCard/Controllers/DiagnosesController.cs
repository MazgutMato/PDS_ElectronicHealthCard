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
    public class DiagnosesController : Controller
    {
        private readonly ModelContext _context;

        public DiagnosesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Diagnoses
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Diagnoses.Include(d => d.DiagnosisNavigation).Include(d => d.Hospitalization);
            return View(await modelContext.ToListAsync());
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.DiagnosisNavigation)
                .Include(d => d.Hospitalization)
                .FirstOrDefaultAsync(m => m.DateStart == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public IActionResult Create()
        {
            ViewData["DiagnosisId"] = new SelectList(_context.DiagnosesTypes, "DiagnosisId", "DiagnosisId");
            ViewData["DateStart"] = new SelectList(_context.Hospitalizations, "DateStart", "HospitalName");
            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateStart,HospitalName,PersonId,DiagnosisId,Document")] Diagnosis diagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiagnosisId"] = new SelectList(_context.DiagnosesTypes, "DiagnosisId", "DiagnosisId", diagnosis.DiagnosisId);
            ViewData["DateStart"] = new SelectList(_context.Hospitalizations, "DateStart", "HospitalName", diagnosis.DateStart);
            return View(diagnosis);
        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
            {
                return NotFound();
            }
            ViewData["DiagnosisId"] = new SelectList(_context.DiagnosesTypes, "DiagnosisId", "DiagnosisId", diagnosis.DiagnosisId);
            ViewData["DateStart"] = new SelectList(_context.Hospitalizations, "DateStart", "HospitalName", diagnosis.DateStart);
            return View(diagnosis);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("DateStart,HospitalName,PersonId,DiagnosisId,Document")] Diagnosis diagnosis)
        {
            if (id != diagnosis.DateStart)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisExists(diagnosis.DateStart))
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
            ViewData["DiagnosisId"] = new SelectList(_context.DiagnosesTypes, "DiagnosisId", "DiagnosisId", diagnosis.DiagnosisId);
            ViewData["DateStart"] = new SelectList(_context.Hospitalizations, "DateStart", "HospitalName", diagnosis.DateStart);
            return View(diagnosis);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.DiagnosisNavigation)
                .Include(d => d.Hospitalization)
                .FirstOrDefaultAsync(m => m.DateStart == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            if (_context.Diagnoses == null)
            {
                return Problem("Entity set 'ModelContext.Diagnoses'  is null.");
            }
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis != null)
            {
                _context.Diagnoses.Remove(diagnosis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisExists(DateTime id)
        {
          return _context.Diagnoses.Any(e => e.DateStart == id);
        }
    }
}
