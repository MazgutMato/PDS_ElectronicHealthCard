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

        public async Task<IActionResult> Search()
        {
            return View();
        }
        public async Task<IActionResult> SearchItems([Bind("DiagnosisId,Description,DailyCosts")] DiagnosesType diagnosesType)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (string.IsNullOrEmpty(diagnosesType.DiagnosisId))
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Index", await _context.DiagnosesTypes
                          .Where(i => i.DiagnosisId == diagnosesType.DiagnosisId)
                          .ToListAsync());
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
                try
                {
                    _context.Add(diagnosesType);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Data Created";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Data Creation Failed";
                    return View(diagnosesType);
                }

            }
            TempData["Message"] = "Data Creation Failed";
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
            try
            {
                _context.Update(diagnosesType);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Edition Failed";
                return View(diagnosesType);
            }
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
            try
            {
                if (_context.DiagnosesTypes == null)
                {
                    return Problem("Entity set 'ModelContext.DiagnosesTypes'  is null.");
                }
                var diagnosesType = await _context.DiagnosesTypes.FindAsync(id);
                if (diagnosesType != null)
                {
                    TempData["Message"] = "Data Deleted";
                    _context.DiagnosesTypes.Remove(diagnosesType);
                }
                else
                {
                    TempData["Message"] = "Data Deletion Failed";
                }


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } catch
            {
                TempData["Message"] = "Data Deletion Failed";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
