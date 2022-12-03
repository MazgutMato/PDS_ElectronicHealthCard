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
            return View(new List<Person>());
        }

        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("PersonId,Zip")] Person person)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (String.IsNullOrEmpty(person.PersonId) && String.IsNullOrEmpty(person.Zip))
            {
                return View("Index", new List<InsuranceComp>());
            }

            if (String.IsNullOrEmpty(person.PersonId))
            {
                return View("Index", await _context.People
                          .Include(p => p.ZipNavigation)
                          .Where(i => i.Zip == person.Zip)
                          .ToListAsync());
            }

            if (String.IsNullOrEmpty(person.Zip))
            {
                return View("Index", await _context.People
                          .Include(p => p.ZipNavigation)
                          .Where(i => i.PersonId == person.PersonId)
                          .ToListAsync());
            }

            return View("Index", await _context.People
                          .Include(p => p.ZipNavigation)
                          .Where(i => i.Zip == person.Zip)
                          .Where(i => i.PersonId == person.PersonId)
                          .ToListAsync());
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
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Zip," + "FirstName,LastName,Phone,Email")] Person person)
        {
            OracleConnection connection = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
            connection.Open();
            OracleTransaction transaction;
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            OracleCommand command = new OracleCommand();
            try
            {
                command.Transaction = transaction;
                command.Connection = connection;
                command.CommandText = "INSERT INTO person values(:ID, :ZIP,person_inf(:First_Name,:Last_Name,:Phone,:Email))";
                OracleParameter[] parameters = new OracleParameter[]
                {
                    new OracleParameter("ID", person.PersonId),
                    new OracleParameter("ZIP", person.Zip),
                    new OracleParameter("First_Name", person.FirstName),
                    new OracleParameter("Last_Name", person.LastName),
                    new OracleParameter("Phone", person.Phone),
                    new OracleParameter("Email", person.Email)
                };
                command.Parameters.AddRange(parameters);

                command.ExecuteNonQuery();
                //Comit
                transaction.Commit();
                connection.Close();

                TempData["Message"] = "Data Created";
                var ret_list = new List<Person>();
                ret_list.Add(person);
                return View("Index", ret_list);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connection.Close();
                TempData["Message"] = "Data Creation Failed";
                return View(person);
            }
        }
        // GET: People/Edit/5
        public async Task<IActionResult> Edit(Person p_person)
        {
            if (p_person == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(p_person.PersonId);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,Zip," + "FirstName,LastName,Phone,Email")] Person person)
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
                    TempData["Message"] = "Data Edited";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Message"] = "Data Edition Failed";
                    return View(person);
                }
            }
            TempData["Message"] = "Data are not valid";
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(Person p_person)
        {
            if (p_person == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.ZipNavigation)
                .FirstOrDefaultAsync(m => m.PersonId == p_person.PersonId);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("PersonId,Zip," + "FirstName,LastName,Phone,Email")] Person person)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'ModelContext.People'  is null.");
            }
            if (person != null)
            {
                TempData["Message"] = "Data Deleted";
                _context.People.Remove(person);
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
