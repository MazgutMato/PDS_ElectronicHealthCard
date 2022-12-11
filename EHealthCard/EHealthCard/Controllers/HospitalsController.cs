using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using NuGet.Packaging.Signing;

namespace EHealthCard.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly ModelContext _context;

        public HospitalsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Hospitals
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Hospitals.Include(h => h.ZipNavigation);
            return View(await modelContext.ToListAsync());
        }
        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("HospitalName,Zip,Capacity")] Hospital hospital)
        {
            TempData["Message"] = "Corresponding Data Listed";
            if (string.IsNullOrEmpty(hospital.HospitalName) && string.IsNullOrEmpty(hospital.Zip))
            {
                return View("Index", new List<InsuranceComp>());
            }

            if (string.IsNullOrEmpty(hospital.Zip))
            {
                return View("Index", await _context.Hospitals
                          .Include(p => p.ZipNavigation)
                          .Where(i => i.HospitalName == hospital.HospitalName)
                          .ToListAsync());
            }

            if (string.IsNullOrEmpty(hospital.HospitalName))
            {
                return View("Index", await _context.Hospitals
                          .Include(p => p.ZipNavigation)
                          .Where(i => i.Zip == hospital.Zip)
                          .ToListAsync());
            }

            return View("Index", await _context.Hospitals
                          .Include(p => p.ZipNavigation)
                          .Where(i => i.Zip == hospital.Zip)
                          .Where(i => i.HospitalName == hospital.HospitalName)
                          .ToListAsync());
        }

        // GET: Hospitals/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Hospitals == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .Include(h => h.ZipNavigation)
                .FirstOrDefaultAsync(m => m.HospitalName == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalName,Zip,Capacity")] Hospital hospital)
        {
            try
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Creation Failed";
                return View(hospital);
            }
        }

        // GET: Hospitals/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Hospitals == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return View(hospital);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HospitalName,Zip,Capacity")] Hospital hospital)
        {
            try
            {
                _context.Update(hospital);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Edition Failed";
                return View(hospital);
            }


        }

        // GET: Hospitals/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Hospitals == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .Include(h => h.ZipNavigation)
                .FirstOrDefaultAsync(m => m.HospitalName == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                if (_context.Hospitals == null)
                {
                    return Problem("Entity set 'EHealthCardContext.Hospitals'  is null.");
                }
                var hospital = await _context.Hospitals.FindAsync(id);
                if (hospital != null)
                {
                    TempData["Message"] = "Data Deleted";
                    _context.Hospitals.Remove(hospital);
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
        public IActionResult Graph()
        {
            return View();
        }
		[HttpPost]
        public IActionResult Graph(int p_year, string p_hospitalName)
        {
            if (p_year <= 0)
            {
                return View();
            }

            try
            {
                var dataPoints = new List<DataPoint>();

                for (var i = 1; i < 13; i++)
                {
                    OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "select get_hosp_count(:YEAR, :MONTH, :HOSP_NAME) from dual";
                    cmd.Parameters.Add(new OracleParameter("YEAR", p_year));
                    cmd.Parameters.Add(new OracleParameter("MONTH", i));
                    cmd.Parameters.Add(new OracleParameter("HOSP_NAME", p_hospitalName));

                    conn.Open();
                    OracleDataReader oraReader = cmd.ExecuteReader();

                    var date = new DateTime(p_year, i, 1).Date;

                    while (oraReader.Read())
                    {
                        dataPoints.Add(new DataPoint(date.Month.ToString(), oraReader.GetInt32(0)));
                    }
                    oraReader.Close();
                    conn.Close();
                }

                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.Year = p_year;
                ViewBag.Hospital = p_hospitalName;

            } catch
            {

            }

            return View();
        }
		public IActionResult  ActualHosp()
		{
			return View(new List<HospitalCapacity>());
		}
		[HttpPost]
		public IActionResult ActualHosp(string zip)
		{
            if (zip.Count() != 5)
            {
                return View(new List<HospitalCapacity>());
            }

            try
            {
                OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;

                cmd.CommandText = "select hospital_name, count(person_id) hosp, capacity from hospital " +
                                    "left join hospitalization using(hospital_name) " +
                                    "where ZIP = :p_zip and date_end is null " +
                                    "group by hospital_name, capacity";
                cmd.Parameters.Add(new OracleParameter("p_zip", zip));

                conn.Open();
                OracleDataReader oraReader = cmd.ExecuteReader();

                var hospRecords = new List<HospitalCapacity>();
                while (oraReader.Read())
                {
                    var hospRecord = new HospitalCapacity();
                    hospRecord.HospitalName = oraReader.GetString(0);
                    hospRecord.CurrentHosp = oraReader.GetInt32(1);
                    hospRecord.Capacity = oraReader.GetInt32(2);

                    hospRecords.Add(hospRecord);
                }
                oraReader.Close();
                conn.Close();


                return View(hospRecords);
            } catch
            {

            }
            return View(new List<HospitalCapacity>());
        }
	}
}
