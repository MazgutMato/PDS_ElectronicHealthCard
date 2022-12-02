using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace EHealthCard.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ModelContext _context;

        public PeopleController(ModelContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {                        
            var modelContext = _context.People.Include(p => p.ZipNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.ZipNavigation)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            
            if (person == null)
            {
                return NotFound();
            }

            OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select get_person_inf(:P_ID) as ret from dual";

            cmd.Parameters.Add(new OracleParameter("P_ID", person.PersonId));

            conn.Open();
            OracleDataReader reader = cmd.ExecuteReader();
            var ret_string = "";
            while (reader.Read())
            {
                ret_string = reader.GetString(0);
            }
            reader.Close();
            string[] values = ret_string.Split(';');
            person.FirstName = values[0];
            person.LastName = values[1];
            person.Phone = values[2];
            person.Email = values[3];

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            ViewData["Zip"] = new SelectList(_context.Cities, "Zip", "Zip");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Zip")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Zip"] = new SelectList(_context.Cities, "Zip", "Zip", person.Zip);
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["Zip"] = new SelectList(_context.Cities, "Zip", "Zip", person.Zip);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,Zip")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
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
            ViewData["Zip"] = new SelectList(_context.Cities, "Zip", "Zip", person.Zip);
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.ZipNavigation)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'ModelContext.People'  is null.");
            }
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
          return _context.People.Any(e => e.PersonId == id);
        }
    }
}
