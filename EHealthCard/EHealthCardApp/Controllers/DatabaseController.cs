using EHealthCardApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Xml;

namespace EHealthCardApp.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly EHealthCardContext _context;
        const int yearsBack = 50;
        public DatabaseController(EHealthCardContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        private async Task DeleteDataAsync()
		{
            //Diagnoses
            _context.Diagnoses.RemoveRange(_context.Diagnoses);
            //Diagnoses types
            _context.DiagnosesTypes.RemoveRange(_context.DiagnosesTypes);
            //Hospitalizations
            _context.Hospitalizations.RemoveRange(_context.Hospitalizations);
            //Payments
            _context.Payments.RemoveRange(_context.Payments);
            //Insurances
            _context.Insurances.RemoveRange(_context.Insurances);
            //Companies
            _context.InsuranceComps.RemoveRange(_context.InsuranceComps);
            //Hospitals
            _context.Hospitals.RemoveRange(_context.Hospitals);
            //People
            _context.People.RemoveRange(_context.People);
            //Cities
            _context.Cities.RemoveRange(_context.Cities);

            await _context.SaveChangesAsync();
        }
        public async Task<IActionResult> Delete()
        {
			try
			{
                await this.DeleteDataAsync();
                TempData["Message"] = "Data successfully deleted!";
            }
            catch
			{
                TempData["Message"] = "Data deleted faild!";
            }            
            return RedirectToAction("Index");
        }

        private string RandomString(string chars, int minSize, int maxSize, bool firstUpper, bool allUpper)
        {
            var random = new Random();
            var word = new char[random.Next(minSize, maxSize)];

            if (firstUpper)
            {
                word[0] = Char.ToUpper(chars[random.Next(chars.Length)]);
            }
            else
            {
                word[0] = chars[random.Next(chars.Length)];
            }

            for (int i = 1; i < word.Length; i++)
            {
                if (allUpper)
                {
                    word[i] = Char.ToUpper(chars[random.Next(chars.Length)]);
                }
                else
                {
                    word[i] = chars[random.Next(chars.Length)];
                }
            }

            return new string(word);
        }

        public async Task<IActionResult> Generate()
        {
            //Delete all data
            await this.DeleteDataAsync();

            Random random = new Random();

            //Cities
            var count = 0;
            var cities = new List<City>();
            while (count < 100)
            {
                var city = new City();
                city.Zip = this.RandomString("0123456789", 5, 5, false, false);
                city.CityName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                var res = false;
                try
                {
                    await _context.AddAsync(city);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(city);
                    res = false;
                }
                if (res)
                {
                    count++;
                    cities.Add(city);
                }
            }
            await _context.SaveChangesAsync();

            //People
            count = 0;
            var people = new List<Person>();
            while (count != 1000)
            {
                var person = new Person();
                person.PersonId = this.RandomString("0123456789", 10, 10, false, false);
                person.FirstName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                person.LastName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                person.Phone = "+4219" + this.RandomString("0123456789", 8, 8, false, false);
                person.Email = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 10, false, false) + "@"
                    + this.RandomString("abcdefghijklmnopqrstuvwxyz", 3, 8, false, false) + ".com";
                person.Zip = cities[random.Next(cities.Count)].Zip;
                var res = false;
                try
                {
                    await _context.AddAsync(person);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(person);
                    res = false;
                }
                if (res)
                {
                    count++;
                    people.Add(person);
                }
            }
            await _context.SaveChangesAsync();

            //Hospitals
            var hospitals = new List<Hospital>();
            count = 0;
            while (count != 100)
            {
                var hospital = new Hospital();
                hospital.Capacity = random.Next(50, 500);
                hospital.HospitalName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                hospital.Zip = cities[random.Next(cities.Count)].Zip;
                var res = false;
                try
                {
                    await _context.AddAsync(hospital);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(hospital);
                    res = false;
                }
                if (res)
                {
                    count++;
                    hospitals.Add(hospital);
                }
            }
            await _context.SaveChangesAsync();

            //Insurance companies
            var companies = new List<InsuranceComp>();
            count = 0;
            while (count != 10)
            {
                var comp = new InsuranceComp();
                comp.CompId = this.RandomString("abcdefghijklmnopqrstuvwxyz", 3, 3, true, true);
                comp.CompName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 15, true, false);
                var res = false;
                try
                {
                    await _context.AddAsync(comp);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(comp);
                    res = false;
                }
                if (res)
                {
                    count++;
                    companies.Add(comp);
                }
            }
            await _context.SaveChangesAsync();

            //Incurencies ended
            count = 0;
            while (count != 600)
            {
                var person = people[random.Next(people.Count)];
                var comp = companies[random.Next(companies.Count)];

                var insurance = new Insurance();
                insurance.PersonId = person.PersonId;
                insurance.CompId = comp.CompId;
                int days = random.Next(365 * yearsBack);
                var starDate = DateTime.Now.AddDays(-days);
                var endDate = DateTime.Now.AddDays(-random.Next(days));
                insurance.DateStart = starDate;
                insurance.DateEnd = endDate;
                var res = false;
                try
                {
                    await _context.AddAsync(insurance);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(insurance);
                    res = false;
                }
                if (res)
                {
                    count++;
                }
            }
            await _context.SaveChangesAsync();

            //Incurencies actual
            var NotInsured = new List<Person>(people);
            var Insured = new List<Person>();
            count = 0;
            while (count != 800)
            {
                var person = NotInsured[random.Next(NotInsured.Count)];
                var comp = companies[random.Next(companies.Count)];

                var insurance = new Insurance();
                insurance.PersonId = person.PersonId;
                insurance.CompId = comp.CompId;

                int days = random.Next(365 * yearsBack);
                var starDate = DateTime.Now.AddDays(-days);
                insurance.DateStart = starDate;

                var res = false;
                try
                {
                    await _context.AddAsync(insurance);                    
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(insurance);
                    res = false;
                }
                if (res)
                {
                    NotInsured.Remove(person);
                    Insured.Add(person);
                    count++;
                }
            }
            await _context.SaveChangesAsync();

            //Payments
            count = 0;
            while (count != 1000)
            {
                var hospital = hospitals[random.Next(hospitals.Count)];
                var comp = companies[random.Next(companies.Count)];

                var payment = new Payment();
                payment.HospitalName = hospital.HospitalName;
                payment.CompId = comp.CompId;

                int days = random.Next(365 * yearsBack);
                var date = DateTime.Now.AddDays(-days);
                var period = date.AddDays(-random.Next(365));
                payment.PaymentDate = date;
                payment.PaymentPeriod = new DateTime(period.Year, period.Month,1);
                var XmlDetails = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(XmlDetails))
                {
                    writer.WriteStartElement("Payment");

                    writer.WriteStartElement("Sender");
                    writer.WriteAttributeString("CompId", comp.CompId);
                    writer.WriteElementString("BankName",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 10, true, false));
                    writer.WriteElementString("IBAN",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 2, 2, true, true) +
                        this.RandomString("0123456789", 22, 22, false, false));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Reciever");
                    writer.WriteAttributeString("HospName", hospital.HospitalName);
                    writer.WriteElementString("BankName",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 10, true, false));
                    writer.WriteElementString("IBAN",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 2, 2, true, true) +
                        this.RandomString("0123456789", 22, 22, false, false));
                    writer.WriteEndElement();

                    writer.WriteElementString("Amount",
                       random.Next(500, 10000).ToString());

                    writer.WriteEndElement();
                }
                payment.Details = XmlDetails.ToString();
                
                var res = false;
                try
                {
                    await _context.AddAsync(payment);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(payment);
                    res = false;
                }
                if (res)
                {
                    count++;
                }
            }
            await _context.SaveChangesAsync();

            //DiagnosesTypes
            var diagnosesTypes = new List<DiagnosesType>();
            count = 0;
            while (count != 100)
            {
                var type = new DiagnosesType();
                type.DiagnosisId = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 5, true, true);
                type.Description = this.RandomString("abcdefghijklmnopqrstuvwxyz", 10, 40, true, false);
                type.DailyCosts = random.Next(10, 100);
                var res = false;
                try
                {
                    await _context.AddAsync(type);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(type);
                    res = false;
                }
                if (res)
                {
                    count++;
                    diagnosesTypes.Add(type);
                }
            }
            await _context.SaveChangesAsync();

            //Hospitalizations ended
            count = 0;
            while (count != 800)
            {
                var person = Insured[random.Next(Insured.Count)];
                var hospital = hospitals[random.Next(hospitals.Count)];

                var hospitalization = new Hospitalization();
                hospitalization.HospitalName = hospital.HospitalName;
                hospitalization.PersonId = person.PersonId;
                int days = random.Next(365 * yearsBack);
                var starDate = DateTime.Now.AddDays(-days);
                var endDate = DateTime.Now.AddDays(-random.Next(days));
                hospitalization.DateStart = starDate;
                hospitalization.DateEnd = endDate;
                
                var res = false;
                try
                {
                    await _context.AddAsync(hospitalization);
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(hospitalization);
                    res = false;
                }
                if (res)
                {
                    count++;

                    //Diagnoses
                    var diagnosesCount = random.Next(1, 3);
                    while(diagnosesCount > 0)
                    {
                        var hospDiagnoze = new Diagnosis();
                        hospDiagnoze.HospitalName = hospitalization.HospitalName;
                        hospDiagnoze.PersonId = hospitalization.PersonId;
                        hospDiagnoze.DateStart = hospitalization.DateStart;
                        hospDiagnoze.DiagnosisId = diagnosesTypes[random.Next(diagnosesTypes.Count)].DiagnosisId;
                        var resDiagnose = false;
                        try
                        {
                            await _context.AddAsync(hospDiagnoze);
                            resDiagnose = true;
                        }
                        catch (Exception ex)
                        {
                            _context.Remove(hospDiagnoze);
                            resDiagnose = false;
                        }
                        if (resDiagnose)
                        {
                            diagnosesCount--;                            
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();

            //Hospitalizations actual
            count = 0;
            while (count != 200)
            {
                var person = Insured[random.Next(Insured.Count)];
                var hospital = hospitals[random.Next(hospitals.Count)];

                var hospitalization = new Hospitalization();
                hospitalization.HospitalName = hospital.HospitalName;
                hospitalization.PersonId = person.PersonId;
                int days = random.Next(365 * yearsBack);
                var starDate = DateTime.Now.AddDays(-days);
                hospitalization.DateStart = starDate;

                var res = false;
                try
                {
                    await _context.AddAsync(hospitalization);    
                    if(hospital.Capacity < 0)
                    {
                        throw new Exception();
                    }
                    res = true;
                }
                catch (Exception ex)
                {
                    _context.Remove(hospitalization);
                    res = false;
                }
                if (res)
                {
                    Insured.Remove(person);
                    hospital.Capacity --;
                    count++;

                    //Diagnoses
                    var diagnosesCount = random.Next(1, 3);
                    while (diagnosesCount > 0)
                    {
                        var hospDiagnoze = new Diagnosis();
                        hospDiagnoze.HospitalName = hospitalization.HospitalName;
                        hospDiagnoze.PersonId = hospitalization.PersonId;
                        hospDiagnoze.DateStart = hospitalization.DateStart;
                        hospDiagnoze.DiagnosisId = diagnosesTypes[random.Next(diagnosesTypes.Count)].DiagnosisId;
                        var resDiagnose = false;
                        try
                        {
                            await _context.AddAsync(hospDiagnoze);
                            resDiagnose = true;
                        }
                        catch (Exception ex)
                        {
                            _context.Remove(hospDiagnoze);
                            resDiagnose = false;
                        }
                        if (resDiagnose)
                        {
                            diagnosesCount--;
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();

            TempData["Message"] = "Data successfully generated!";
            return RedirectToAction(nameof(Index));
        }
    }
}
