using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Reflection.PortableExecutable;
using Microsoft.CodeAnalysis;
using System.Reflection.Metadata;
using System.Globalization;
using OracleInternal.SqlAndPlsqlParser;
using System.IO;
using System.Collections;

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
            return View(new List<Diagnosis>());
        }

        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("DateStart,HospitalName,PersonId,DiagnosisId,Document")] Diagnosis diagnosis)
        {
            //ONLY DATE
            diagnosis.DateStart = diagnosis.DateStart.Date;

            TempData["Message"] = "Corresponding Data Listed";
            if (String.IsNullOrEmpty(diagnosis.PersonId) && String.IsNullOrEmpty(diagnosis.HospitalName))
            {
                return View("Index", new List<Diagnosis>());
            }

            OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;

            if (String.IsNullOrEmpty(diagnosis.PersonId))
            {
                cmd.CommandText = "select * from diagnoses where hospital_name = :HOSPITAL_NAME";
                cmd.Parameters.Add(new OracleParameter("HOSPITAL_NAME", diagnosis.HospitalName));
            }

            if (String.IsNullOrEmpty(diagnosis.HospitalName))
            {
                cmd.CommandText = "select * from diagnoses where person_id = :PERSON_ID";
                cmd.Parameters.Add(new OracleParameter("PERSON_ID", diagnosis.PersonId));
            }

            if (!String.IsNullOrEmpty(diagnosis.PersonId) && !String.IsNullOrEmpty(diagnosis.HospitalName))
            {
                cmd.CommandText = "select * from diagnoses where person_id = :PERSON_ID and hospital_name = :HOSPITAL_NAME";
                cmd.Parameters.Add(new OracleParameter("PERSON_ID", diagnosis.PersonId));
                cmd.Parameters.Add(new OracleParameter("HOSPITAL_NAME", diagnosis.HospitalName));
            }


            conn.Open();
            OracleDataReader oraReader = cmd.ExecuteReader();

            var list = new List<Diagnosis>();
            while (oraReader.Read())
            {
                Diagnosis data = new Diagnosis();
                byte[] document = new byte[0];
                data.DateStart = oraReader.GetDateTime(0);
                data.HospitalName = oraReader.GetString(1);
                data.PersonId = oraReader.GetString(2);
                data.DiagnosisId = oraReader.GetString(3);
                if (oraReader.GetValue(4).ToString() != "")
                {
                    OracleBlob blob = oraReader.GetOracleBlob(4);
                    document = new byte[blob.Length];
                    blob.Read(document, 0, document.Length);
                }
                data.Document = document;
                list.Add(data);
            }
            oraReader.Close();
            conn.Close();

            return View("Index", list);
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(DateTime start, string p_id, string hos_name, string dia_id)
        {
            //ONLY DATE
            start = start.Date;

            if (start == null || String.IsNullOrEmpty(p_id)
                || String.IsNullOrEmpty(hos_name)
                || String.IsNullOrEmpty(dia_id))
            {
                return NotFound();
            }

            OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "select * from diagnoses where person_id = :PERSON_ID" +
                " and hospital_name = :HOSPITAL_NAME" +
                " and date_start = :DATE_START" +
                " and diagnosis_id = :DIAGNOSIS_ID";
            cmd.Parameters.Add(new OracleParameter("PERSON_ID", p_id));
            cmd.Parameters.Add(new OracleParameter("HOSPITAL_NAME", hos_name));
            cmd.Parameters.Add(new OracleParameter("DATE_START", start));
            cmd.Parameters.Add(new OracleParameter("DIAGNOSIS_ID", dia_id));

            conn.Open();
            OracleDataReader oraReader = cmd.ExecuteReader();
            Diagnosis diagnoses = new Diagnosis();
            while (oraReader.Read())
            {
                byte[] document = new byte[0];
                diagnoses.DateStart = oraReader.GetDateTime(0);
                diagnoses.HospitalName = oraReader.GetString(1);
                diagnoses.PersonId = oraReader.GetString(2);
                diagnoses.DiagnosisId = oraReader.GetString(3);
                if (oraReader.GetValue(4).ToString() != "")
                {
                    OracleBlob blob = oraReader.GetOracleBlob(4);
                    document = new byte[blob.Length];
                    blob.Read(document, 0, document.Length);
                }
                diagnoses.Document = document;
                oraReader.Close();
                conn.Close();
                return View(diagnoses);
            }

            TempData["Message"] = "Error";
            return View("Index");
        }

        // GET: Diagnoses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateStart,HospitalName,PersonId,DiagnosisId,Document,ImageFile")] Diagnosis diagnosis)
        {
            //ONLY DATE
            diagnosis.DateStart = diagnosis.DateStart.Date;

            if (diagnosis.ImageFile != null)
            {
                byte[] blob = new byte[diagnosis.ImageFile.Length];
                await diagnosis.ImageFile.OpenReadStream().ReadAsync(blob);
                diagnosis.Document = blob;
            }
            
            try
            {
                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                return RedirectToAction(nameof(Index));
            } catch
            {
                TempData["Message"] = "Data Creation Failed";
                return View(diagnosis);
            }                         
        }

        //// GET: Diagnoses/Edit/5
        //public async Task<IActionResult> Edit(DateTime? id)
        //{
        //    if (id == null || _context.Diagnoses == null)
        //    {
        //        return NotFound();
        //    }

        //    var diagnosis = await _context.Diagnoses.FindAsync(id);
        //    if (diagnosis == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["DiagnosisId"] = new SelectList(_context.DiagnosesTypes, "DiagnosisId", "DiagnosisId", diagnosis.DiagnosisId);
        //    ViewData["DateStart"] = new SelectList(_context.Hospitalizations, "DateStart", "HospitalName", diagnosis.DateStart);
        //    return View(diagnosis);
        //}

        //// POST: Diagnoses/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(DateTime id, [Bind("DateStart,HospitalName,PersonId,DiagnosisId,Document")] Diagnosis diagnosis)
        //{
        //    if (id != diagnosis.DateStart)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(diagnosis);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DiagnosisExists(diagnosis.DateStart))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DiagnosisId"] = new SelectList(_context.DiagnosesTypes, "DiagnosisId", "DiagnosisId", diagnosis.DiagnosisId);
        //    ViewData["DateStart"] = new SelectList(_context.Hospitalizations, "DateStart", "HospitalName", diagnosis.DateStart);
        //    return View(diagnosis);
        //}

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(DateTime start, string p_id, string hos_name, string dia_id)
        {
            //ONLY DATE
            start = start.Date;

            if (start == null || String.IsNullOrEmpty(p_id)
                || String.IsNullOrEmpty(hos_name)
                || String.IsNullOrEmpty(dia_id))
            {
                return NotFound();
            }

            OracleConnection conn = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = "select * from diagnoses" +
                " where person_id = :PERSON_ID" +
                " and hospital_name = :HOSPITAL_NAME" +
                " and date_start = :DATE_START" +
                " and diagnosis_id = :DIAGNOSIS_ID";
            cmd.Parameters.Add(new OracleParameter("PERSON_ID", p_id));
            cmd.Parameters.Add(new OracleParameter("HOSPITAL_NAME", hos_name));
            cmd.Parameters.Add(new OracleParameter("DATE_START", start));
            cmd.Parameters.Add(new OracleParameter("DIAGNOSIS_ID", dia_id));

            conn.Open();
            OracleDataReader oraReader = cmd.ExecuteReader();
            Diagnosis diagnoses = new Diagnosis();
            while (oraReader.Read())
            {
                byte[] document = new byte[0];
                diagnoses.DateStart = oraReader.GetDateTime(0);
                diagnoses.HospitalName = oraReader.GetString(1);
                diagnoses.PersonId = oraReader.GetString(2);
                diagnoses.DiagnosisId = oraReader.GetString(3);
                if (oraReader.GetValue(4).ToString() != "")
                {
                    OracleBlob blob = oraReader.GetOracleBlob(4);
                    document = new byte[blob.Length];
                    blob.Read(document, 0, document.Length);
                }
                diagnoses.Document = document;
                oraReader.Close();
                conn.Close();
                return View(diagnoses);
            }

            TempData["Message"] = "Error";
            return View("Index");
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, [Bind("DateStart,HospitalName,PersonId,DiagnosisId,Document")] Diagnosis diagnosis)
        {
            //ONLY DATE
            diagnosis.DateStart = diagnosis.DateStart.Date;

            try
            {
                if (_context.Diagnoses == null)
                {
                    return Problem("Entity set 'ModelContext.Diagnoses'  is null.");
                }

                if (diagnosis != null)
                {
                    TempData["Message"] = "Data Deleted";
                    _context.Diagnoses.Remove(diagnosis);
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
