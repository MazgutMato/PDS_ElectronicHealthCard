using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections;

namespace EHealthCard.Controllers
{
    public class HospitalizationsController : Controller
    {
        private readonly ModelContext _context;

        public HospitalizationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Hospitalizations
        public async Task<IActionResult> Index()
        {
            return View(new List<Hospitalization>());
        }
        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            //ONLY DATE
            hospitalization.DateStart = hospitalization.DateStart.Date;
            hospitalization.DateEnd = hospitalization.DateEnd?.Date;

            TempData["Message"] = "Corresponding Data Listed";
            if (string.IsNullOrEmpty(hospitalization.PersonId) && string.IsNullOrEmpty(hospitalization.HospitalName))
            {
                return View("Index", new List<Hospitalization>());
            }

            if (string.IsNullOrEmpty(hospitalization.HospitalName))
            {
                return View("Index", await _context.Hospitalizations
                          .Include(p => p.Person)
                          .Include(p => p.HospitalNameNavigation)
                          .Where(i => i.PersonId == hospitalization.PersonId)
                          .ToListAsync());
            }

            if (string.IsNullOrEmpty(hospitalization.PersonId))
            {
                return View("Index", await _context.Hospitalizations
                          .Include(p => p.Person)
                          .Include(p => p.HospitalNameNavigation)
                          .Where(i => i.HospitalName == hospitalization.HospitalName)
                          .ToListAsync());
            }

            return View("Index", await _context.Hospitalizations
                          .Include(p => p.Person)
                          .Include(p => p.HospitalNameNavigation)
                          .Where(i => i.PersonId == hospitalization.PersonId)
                          .Where(i => i.HospitalName == hospitalization.HospitalName)
                          .ToListAsync());
        }

        // GET: Hospitalizations/Details/5
        public async Task<IActionResult> Details(Hospitalization p_hospitalization)
        {
            //ONLY DATE
            p_hospitalization.DateStart = p_hospitalization.DateStart.Date;
            p_hospitalization.DateEnd = p_hospitalization.DateEnd?.Date;

            if (_context.Hospitalizations == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                .Include(h => h.HospitalNameNavigation)
                .Include(h => h.Person)
                .Where(i => i.PersonId == p_hospitalization.PersonId)
                .Where(i => i.HospitalName == p_hospitalization.HospitalName)
                .Where(i => i.DateStart == p_hospitalization.DateStart)
                .FirstOrDefaultAsync();
            if (hospitalization == null)
            {
                return NotFound();
            }

            return View(hospitalization);
        }

        // GET: Hospitalizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitalizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            //ONLY DATE
            hospitalization.DateStart = hospitalization.DateStart.Date;
            hospitalization.DateEnd = hospitalization.DateEnd?.Date;

            try
            {
                _context.Add(hospitalization);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Creation Failed"; ;
            }
            TempData["Message"] = "Data Creation Failed";
            return View(hospitalization);
        }

        // GET: Hospitalizations/Edit/5
        public async Task<IActionResult> Edit(Hospitalization p_hospitalization)
        {
            //ONLY DATE
            p_hospitalization.DateStart = p_hospitalization.DateStart.Date;
            p_hospitalization.DateEnd = p_hospitalization.DateEnd?.Date;

            if (_context.Hospitalizations == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                .FindAsync(p_hospitalization.DateStart, p_hospitalization.HospitalName, p_hospitalization.PersonId);
            if (hospitalization == null)
            {
                return NotFound();
            }
            return View(hospitalization);
        }

        // POST: Hospitalizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            //ONLY DATE
            hospitalization.DateStart = hospitalization.DateStart.Date;
            hospitalization.DateEnd = hospitalization.DateEnd?.Date;

            try
            {
                _context.Update(hospitalization);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Edition Failed";
            }

            TempData["Message"] = "Data Edition Failed";
            return View(hospitalization);
        }

        // GET: Hospitalizations/Delete/5
        public async Task<IActionResult> Delete(Hospitalization p_hospitalization)
        {
            //ONLY DATE
            p_hospitalization.DateStart = p_hospitalization.DateStart.Date;
            p_hospitalization.DateEnd = p_hospitalization.DateEnd?.Date;

            if (_context.Hospitalizations == null)
            {
                return NotFound();
            }

            var hospitalization = await _context.Hospitalizations
                    .Include(h => h.HospitalNameNavigation)
                    .Include(h => h.Person)
                    .Where(i => i.PersonId == p_hospitalization.PersonId)
                    .Where(i => i.HospitalName == p_hospitalization.HospitalName)
                    .Where(i => i.DateStart == p_hospitalization.DateStart)
                    .FirstOrDefaultAsync();
            if (hospitalization == null)
            {
                return NotFound();
            }

            return View(hospitalization);
        }

        // POST: Hospitalizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id, [Bind("PersonId,HospitalName,DateStart,DateEnd")] Hospitalization hospitalization)
        {
            //ONLY DATE
            hospitalization.DateStart = hospitalization.DateStart.Date;
            hospitalization.DateEnd = hospitalization.DateEnd?.Date;
            try
            {
                if (_context.Hospitalizations == null)
                {
                    return Problem("Entity set 'EHealthCardContext.Hospitalizations'  is null.");
                }

                if (hospitalization != null)
                {
                    TempData["Message"] = "Data Deleted";
                    _context.Hospitalizations.Remove(hospitalization);
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
        public IActionResult Graph(int year)
        {
            if (year <= 0)
            {
                return View();
            }
            try
            {
                OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;

                cmd.CommandText = "select row_number() over(order by pocet desc) poradie , diagnosis_id, pocet " +
                                    "from " +
                                    "(" +
                                        "select diagnosis_id, count(diagnosis_id) pocet from diagnoses " +
                                            "where to_char(date_start, 'YYYY') like :YEAR " +
                                                "group by diagnosis_id" +
                                    ")fetch first 10 rows only";
                cmd.Parameters.Add(new OracleParameter("YEAR", year));

                conn.Open();
                OracleDataReader oraReader = cmd.ExecuteReader();

                List<DataPoint> dataPoints = new List<DataPoint>();
                while (oraReader.Read())
                {
                    var rank = oraReader.GetInt32(0);
                    dataPoints.Add(new DataPoint(oraReader.GetString(1), oraReader.GetInt32(2)));
                }
                oraReader.Close();
                conn.Close();

                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.Year = year;

                return View();

            } catch
            {

            }
            return View();
        }

        public IActionResult Table()
        {
            return View(new List<HospitalizationTableRecord>());
        }

        [HttpPost]
        public IActionResult Table(int p_year, string p_hospitalName)
        {
            try
            {
                OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select diagnosis_id, " + 
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 1 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 1 and extract(month from NVL(date_end,sysdate)) >= 1 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 1 ) " +
                    "then 1 else 0 end) first, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 2 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 2 and extract(month from NVL(date_end,sysdate)) >= 2 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 2 ) " +
                    "then 1 else 0 end) second, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 3 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 3 and extract(month from NVL(date_end,sysdate)) >= 3 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 3 ) " +
                    "then 1 else 0 end) third, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 4 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 4 and extract(month from NVL(date_end,sysdate)) >= 4 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 4 )" +
                    "then 1 else 0 end) fourth, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 5 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 5 and extract(month from NVL(date_end,sysdate)) >= 5 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 5 ) " +
                    "then 1 else 0 end) fifth, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 6 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 6 and extract(month from NVL(date_end,sysdate)) >= 6 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 6 ) " +
                    "then 1 else 0 end) sixth, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 7 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 7 and extract(month from NVL(date_end,sysdate)) >= 7 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 7 ) " +
                    "then 1 else 0 end) seventh, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 8 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 8 and extract(month from NVL(date_end,sysdate)) >= 8 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 8 ) " +
                    "then 1 else 0 end) eighth, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 9 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 9 and extract(month from NVL(date_end,sysdate)) >= 9 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 9 ) " +
                    "then 1 else 0 end) ninth, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 10 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 10 and extract(month from NVL(date_end,sysdate)) >= 10 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 10 ) " +
                    "then 1 else 0 end) tenth, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 11 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 11 and extract(month from NVL(date_end,sysdate)) >= 11 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 11 ) " +
                    "then 1 else 0 end) eleventh, " +
                    "sum(case when " +
                    "(  extract(year from date_start) < :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) ) or " +
                    "(  extract(year from date_start) = :P_YEAR and (extract(year from NVL(date_end,sysdate)) > :P_YEAR) and extract(month from date_start) <= 12 ) or " +
                    "(  extract(year from date_start) = :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from date_start) <= 12 and extract(month from NVL(date_end,sysdate)) >= 12 ) or " +
                    "(  extract(year from date_start) < :P_YEAR and extract(year from NVL(date_end,sysdate)) = :P_YEAR and extract(month from NVL(date_end,sysdate)) >= 12 ) " +
                    "then 1 else 0 end) twelfth " +
                    "from hospital " +
                    "join hospitalization using(hospital_name) " +
                    "join diagnoses using(person_id,hospital_name,date_start) " +
                    "where extract(year from date_start) <= :P_YEAR and (extract(year from NVL(date_end,sysdate)) >= :P_YEAR) and hospital_name = :HOSPITAL_NAME " +
                    "group by diagnosis_id";
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("P_YEAR", p_year));
                cmd.Parameters.Add(new OracleParameter("HOSPITAL_NAME", p_hospitalName));

                conn.Open();
                OracleDataReader oraReader = cmd.ExecuteReader();
                Diagnosis diagnoses = new Diagnosis();


                List<HospitalizationTableRecord> tableRecords = new List<HospitalizationTableRecord>();
                while (oraReader.Read())
                {
                    tableRecords.Add(new HospitalizationTableRecord(oraReader.GetString(0),
                        oraReader.GetInt32(1),
                        oraReader.GetInt32(2),
                        oraReader.GetInt32(3),
                        oraReader.GetInt32(4),
                        oraReader.GetInt32(5),
                        oraReader.GetInt32(6),
                        oraReader.GetInt32(7),
                        oraReader.GetInt32(8),
                        oraReader.GetInt32(9),
                        oraReader.GetInt32(10),
                        oraReader.GetInt32(11),
                        oraReader.GetInt32(12)));  
                }
                oraReader.Close();
                conn.Close();
                return View(tableRecords);
            } catch
            {}

            return View();
        }

        public IActionResult DailyCosts()
        {
            return View(new List<DailyCosts>());
        }

        [HttpPost]
        public IActionResult DailyCosts(int p_year, int p_month, string p_hospitalName)
        {
            try
            {
                string p_date = "10." + p_month.ToString() + "." + p_year.ToString();



                OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = 
                    "select diagnosis_id, days, daily_costs, daily_costs*days from " +
                    "(select diagnosis_id, sum(dayz) days from " +
                    "(select diagnosis_id,(case when (date_start <= TRUNC(to_date(:P_DATE,'DD.MM.YYYY'), 'MM')) " +
                    "and ( NVL(date_end,sysdate) <= last_day(to_date(:P_DATE,'DD.MM.YYYY'))) then  NVL(date_end,sysdate) - TRUNC(to_date(:P_DATE,'DD.MM.YYYY'), 'MM') + 1 " +
                    "when (date_start <= TRUNC(to_date(:P_DATE,'DD.MM.YYYY'), 'MM')) " +
                    "and ( NVL(date_end,sysdate) >= last_day(to_date(:P_DATE,'DD.MM.YYYY'))) then last_day(to_date(:P_DATE,'DD.MM.YYYY')) - TRUNC(to_date(:P_DATE,'DD.MM.YYYY'), 'MM') + 1 " +
                    "when (date_start >= TRUNC(to_date(:P_DATE,'DD.MM.YYYY'), 'MM')) " +
                    "and ( NVL(date_end,sysdate) <= last_day(to_date(:P_DATE,'DD.MM.YYYY'))) then NVL(date_end,sysdate) - date_start + 1 " +
                    "when (date_start >= TRUNC(to_date(:P_DATE,'DD.MM.YYYY'), 'MM')) " +
                    "and ( NVL(date_end,sysdate) >= last_day(to_date(:P_DATE,'DD.MM.YYYY'))) then last_day(to_date(:P_DATE,'DD.MM.YYYY')) - date_start + 1 " +
                    "end) dayz " +
                    "from hospitalization" +
                    "join diagnoses using(person_id, hospital_name, date_start) " +
                    "where hospital_name = :HOSPITAL_NAME and extract(year from date_start) <= :P_YEAR and extract(month from date_start) <= :P_MONTH " +
                    "and extract(year from NVL(date_end,sysdate)) >= :P_YEAR and extract(month from NVL(date_end,sysdate)) >= :P_MONTH) " +
                    "group by diagnosis_id) " +
                    "join diagnoses_type using(diagnosis_id)";
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("HOSPITAL_NAME", p_hospitalName));
                cmd.Parameters.Add(new OracleParameter("P_YEAR", p_year));
                cmd.Parameters.Add(new OracleParameter("P_MONTH", p_month));
                cmd.Parameters.Add(new OracleParameter("P_DATE", p_date));
                

                conn.Open();
                OracleDataReader oraReader = cmd.ExecuteReader();
                Diagnosis diagnoses = new Diagnosis();


                List<DailyCosts> tableRecords = new List<DailyCosts>();
                while (oraReader.Read())
                {
                    tableRecords.Add(new DailyCosts(oraReader.GetString(0),
                        oraReader.GetInt32(1),
                        oraReader.GetInt32(2),
                        oraReader.GetInt32(3)));
                }
                oraReader.Close();
                conn.Close();
                return View(tableRecords);
            }
            catch
            {

            }
            return View(new List<DailyCosts>());
        }
    }
}
