using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;

namespace EHealthCard.Controllers
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
            //ONLY DATE
            insurance.DateStart = insurance.DateStart.Date;
            insurance.DateEnd = insurance.DateEnd?.Date;

            TempData["Message"] = "Corresponding Data Listed";
            if (string.IsNullOrEmpty(insurance.PersonId) && string.IsNullOrEmpty(insurance.CompId))
            {
                return View("Index", new List<Insurance>());
            }

            if (string.IsNullOrEmpty(insurance.CompId))
            {
                return View("Index", await _context.Insurances
                          .Include(p => p.Comp)
                          .Include(p => p.Person)
                          .Where(i => i.PersonId == insurance.PersonId)
                          .ToListAsync());
            }

            if (string.IsNullOrEmpty(insurance.PersonId))
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
            //ONLY DATE
            p_insurance.DateStart = p_insurance.DateStart.Date;
            p_insurance.DateEnd = p_insurance.DateEnd?.Date;

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
            //ONLY DATE
            insurance.DateStart = insurance.DateStart.Date;
            insurance.DateEnd = insurance.DateEnd?.Date;

            try
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Creation Failed";
            }
            TempData["Message"] = "Data Creation Failed";
            return View(insurance);
        }

        // GET: Insurances/Edit/5
        public async Task<IActionResult> Edit(Insurance p_insurance)
        {   
            //ONLY DATE
            p_insurance.DateStart = p_insurance.DateStart.Date;
            p_insurance.DateEnd = p_insurance.DateEnd?.Date;

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
            //ONLY DATE
            insurance.DateStart = insurance.DateStart.Date;
            insurance.DateEnd = insurance.DateEnd?.Date;

            try
            {
                _context.Update(insurance);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Edition Failed";
                return View(insurance);
            }

        }

        // GET: Insurances/Delete/5
        public async Task<IActionResult> Delete(Insurance p_insurance)
        {
            //ONLY DATE
            p_insurance.DateStart = p_insurance.DateStart.Date;
            p_insurance.DateEnd = p_insurance.DateEnd?.Date;

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
            //ONLY DATE
            insurance.DateStart = insurance.DateStart.Date;
            insurance.DateEnd = insurance.DateEnd?.Date;

            try
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
            catch
            {
                TempData["Message"] = "Data Deletion Failed";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult MostInsured()
        {
            try
            {
                OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;

                cmd.CommandText = "select rn, person_id, get_person_inf(person_id), ct " +
                                  "from (select row_number() over(order by count(person_id) desc) rn, " +
                                  "person_id, count(person_id) ct " +
                                  "from insurance " + 
                                  "group by person_id)" +
                                  "where rn <= 10";

                conn.Open();
                OracleDataReader oraReader = cmd.ExecuteReader();

                var tableMostInsured = new List<MostInsured>();
                while (oraReader.Read())
                {
                    string[] values = oraReader.GetString(2).Split(';');
                    tableMostInsured.Add(new MostInsured(oraReader.GetString(1), values[0], values[1], oraReader.GetInt32(3)));
                }
                oraReader.Close();
                conn.Close();
                return View(tableMostInsured);
            } catch
            {

            }

            return View(new List<MostInsured>());
        }
        public IActionResult Graph()
        {
            try
            {
                var dataPoints = new List<DataPoint>();

                OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;

                //Count
                cmd.CommandText = "select count(person_id) from insurance " +
                                    "where date_end is null";

                conn.Open();
                var oraReader = cmd.ExecuteReader();

                var count = 0;
                while (oraReader.Read())
                {
                    count = oraReader.GetInt32(0);
                }
                oraReader.Close();                

                //Top 10
                cmd.CommandText = "select * from" +
                                    "(" +
                                        "select row_number() over(order by count(person_id) desc) poradie,comp_id, count(person_id) from insurance " +
                                        "where date_end is null " +
                                        "group by comp_id " +
                                    ")WHERE poradie <= 10";

                oraReader = cmd.ExecuteReader();

                double sum = 0;
                while (oraReader.Read())
                {
                    double comCount = oraReader.GetInt32(2);
                    double percentage = comCount / count * 100;
                    dataPoints.Add(new DataPoint(oraReader.GetString(1), percentage));
                    sum += percentage;
                }
                oraReader.Close();
                conn.Close();

                dataPoints.Add(new DataPoint("Others", 100 - sum));

                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }
            catch
            {

            }

            return View();
        }
    }
}
