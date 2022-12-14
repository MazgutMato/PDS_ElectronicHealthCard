using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using EHealthCard.Models;

namespace EHealthCard.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ModelContext _context;

        public CitiesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cities.ToListAsync());
        }
        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("Zip,CityName")] City city)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (string.IsNullOrEmpty(city.Zip) && string.IsNullOrEmpty(city.CityName))
            {
                return View("Index", new List<City>());
            }

            if (string.IsNullOrEmpty(city.Zip))
            {
                return View("Index", await _context.Cities
                          .Where(i => i.CityName == city.CityName)
                          .ToListAsync());
            }

            if (string.IsNullOrEmpty(city.CityName))
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
                catch (Exception ex)
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
            try
            {
                _context.Update(city);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Edition Failed";
                return View(city);
            }

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
            try
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
