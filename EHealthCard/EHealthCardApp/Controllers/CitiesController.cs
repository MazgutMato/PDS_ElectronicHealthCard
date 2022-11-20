using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCardApp.Models;
using System.Security.Cryptography;

namespace EHealthCardApp.Controllers
{
    public class CitiesController : Controller
    {
        private readonly EHealthCardContext _context;

        public CitiesController(EHealthCardContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            return View( new List<City>());
        }
        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("Zip,CityName")] City city)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (city.Zip.IsNullOrEmpty() && city.CityName.IsNullOrEmpty())
            {
                return View("Index", new List<City>());
            }

            if (city.Zip.IsNullOrEmpty())
            {
                return View("Index", await _context.Cities
                          .Where(i => i.CityName == city.CityName)
                          .ToListAsync());
            }

            if (city.CityName.IsNullOrEmpty())
            {
                return View("Index", await _context.Cities
                          .Where(i => i.Zip == city.Zip)
                          .ToListAsync());
            }

            return View("Index", await _context.Cities
                          .Where(i => i.Zip == city.Zip)
                          .Where(i => i.CityName == city.CityName)
                          .ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Zip == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Zip,CityName")] City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Data Created";
                    return RedirectToAction(nameof(Index));

                } 
                catch(Exception ex)
                {
                    TempData["Message"] = "Data Creation Failed";
                    return View(city);
                }
                
            }            
            TempData["Message"] = "Data Creation Failed";
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Zip,CityName")] City city)
        {
            if (id != city.Zip)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Zip))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            TempData["Message"] = "Data Edition Failed";
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Zip == id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Cities == null)
            {
                return Problem("Entity set 'EHealthCardContext.Cities'  is null.");
            }
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                TempData["Message"] = "Data Deleted";
                _context.Cities.Remove(city);
            } else
            {
                TempData["Message"] = "Data Deletion Failed";
            }

            
            await _context.SaveChangesAsync();            
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(string id)
        {
            return _context.Cities.Any(e => e.Zip == id);
        }
    }
}
