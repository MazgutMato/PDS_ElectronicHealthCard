﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EHealthCard.Models;
using System.Text;
using System.Xml;

namespace EHealthCard.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ModelContext _context;

        public PaymentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            return View(new List<Payment>());
        }

        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchItems([Bind("HospitalName,CompId,PaymentDate,PaymentPeriod")] Payment payment)
        {
            //ONLY DATE
            payment.PaymentPeriod = payment.PaymentPeriod.Date;
            payment.PaymentDate = payment.PaymentDate.Date;

            TempData["Message"] = "Corresponding Data Listed";
            if (string.IsNullOrEmpty(payment.CompId) && string.IsNullOrEmpty(payment.HospitalName))
            {
                return View("Index", new List<InsuranceComp>());
            }

            if (string.IsNullOrEmpty(payment.CompId))
            {
                return View("Index", await _context.Payments
                          .Include(p => p.HospitalNameNavigation)
                          .Include(p => p.Comp)
                          .Where(i => i.HospitalName == payment.HospitalName)
                          .ToListAsync());
            }

            if (string.IsNullOrEmpty(payment.HospitalName))
            {
                return View("Index", await _context.Payments
                          .Include(p => p.HospitalNameNavigation)
                          .Include(p => p.Comp)
                          .Where(i => i.CompId == payment.CompId)
                          .ToListAsync());
            }

            return View("Index", await _context.Payments
                          .Include(p => p.Comp)
                          .Include(p => p.HospitalNameNavigation)
                          .Where(i => i.CompId == payment.CompId)
                          .Where(i => i.HospitalName == payment.HospitalName)
                          .ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(Payment p_payment)
        {
            //ONLY DATE
            p_payment.PaymentPeriod = p_payment.PaymentPeriod.Date;
            p_payment.PaymentDate = p_payment.PaymentDate.Date;

            if (p_payment == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Comp)
                .Include(p => p.HospitalNameNavigation)
                .Where(i => i.CompId == p_payment.CompId)
                .Where(i => i.HospitalName == p_payment.HospitalName)
                .Where(i => i.PaymentId == p_payment.PaymentId)
                .FirstOrDefaultAsync();
            if (payment == null)
            {
                return NotFound();
            }

            var stringReader = new StringReader(payment.Details);
            XmlReader reader = XmlReader.Create(stringReader);

            while (reader.Name != "Bank")
            {
                reader.Read();
            }
            payment.CompBank = reader.ReadElementContentAsString();
            while (reader.Name != "IBAN")
            {
                reader.Read();
            }
            payment.CompIban = reader.ReadElementContentAsString();
            while (reader.Name != "Bank")
            {
                reader.Read();
            }
            payment.HospitalBank = reader.ReadElementContentAsString();
            while (reader.Name != "IBAN")
            {
                reader.Read();
            }
            payment.HospitalIban = reader.ReadElementContentAsString();
            while (reader.Name != "Amount")
            {
                reader.Read();
            }
            payment.Amount = reader.ReadElementContentAsDouble();
            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,HospitalName,CompId," +
            "PaymentDate,PaymentPeriod,CompBank,HospitalBank,CompIban,HospitalIban,Amount")] Payment payment)
        {
            //ONLY DATE
            payment.PaymentPeriod = payment.PaymentPeriod.Date;
            payment.PaymentDate = payment.PaymentDate.Date;

            try
            {
                var XmlDetails = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(XmlDetails))
                {
                    writer.WriteStartElement("Payment");

                    writer.WriteStartElement("Sender");
                    writer.WriteAttributeString("CompId", payment.CompId);
                    writer.WriteElementString("Bank", payment.CompBank);
                    writer.WriteElementString("IBAN", payment.CompIban);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Reciever");
                    writer.WriteAttributeString("HospName", payment.HospitalName);
                    writer.WriteElementString("Bank", payment.HospitalBank);
                    writer.WriteElementString("IBAN", payment.HospitalIban);
                    writer.WriteEndElement();

                    writer.WriteElementString("Amount", payment.Amount.ToString());

                    writer.WriteEndElement();
                }
                payment.Details = XmlDetails.ToString();
                _context.Add(payment);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Created";
                var ret_list = new List<Payment>();
                ret_list.Add(payment);
                return View("Index", ret_list);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Creation Failed";
                return View(payment);
            }

        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(Payment p_payment)
        {
            //ONLY DATE
            p_payment.PaymentPeriod = p_payment.PaymentPeriod.Date;
            p_payment.PaymentDate = p_payment.PaymentDate.Date;

            if (p_payment == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(p_payment.PaymentId);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PaymentId,HospitalName,CompId,PaymentDate,PaymentPeriod,Details")] Payment payment)
        {
            //ONLY DATE
            payment.PaymentPeriod = payment.PaymentPeriod.Date;
            payment.PaymentDate = payment.PaymentDate.Date;

            try
            {
                _context.Update(payment);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Data Edited";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Data Edition Failed";
                return View(payment);
            }
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(Payment p_payment)
        {
            //ONLY DATE
            p_payment.PaymentPeriod = p_payment.PaymentPeriod.Date;
            p_payment.PaymentDate = p_payment.PaymentDate.Date;

            if (p_payment == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Comp)
                .Include(p => p.HospitalNameNavigation)
                .Where(i => i.CompId == p_payment.CompId)
                .Where(i => i.HospitalName == p_payment.HospitalName)
                .Where(i => i.PaymentId == p_payment.PaymentId)
                .FirstOrDefaultAsync();
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, [Bind("PaymentId,HospitalName,CompId,PaymentDate,PaymentPeriod,Details")] Payment payment)
        {
            //ONLY DATE
            payment.PaymentPeriod = payment.PaymentPeriod.Date;
            payment.PaymentDate = payment.PaymentDate.Date;

            try
            {
                if (_context.Payments == null)
                {
                    return Problem("Entity set 'EHealthCardContext.Payments'  is null.");
                }
                if (payment != null)
                {
                    TempData["Message"] = "Data Deleted";
                    _context.Payments.Remove(payment);
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
