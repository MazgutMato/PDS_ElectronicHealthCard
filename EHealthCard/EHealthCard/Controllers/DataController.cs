using EHealthCard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Text;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EHealthCard.Controllers
{
    public class DataController : Controller
    {
        private readonly ModelContext _context;
        const int yearsBack = 50;
        public DataController(ModelContext context)
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

        private string GeneratePersonId()
        {
            var random = new Random();
            var birth = DateTime.Now.Date.AddDays(-random.Next(365, 365*80));
            var date = birth.Year.ToString().Substring(2,2);
            var month = birth.Month;
            var day = birth.Day;         

            if(random.Next(0,2) == 1)
            {
                month += 50;
            }

            if(month < 10)
            {
                date += "0";               
            }
            date += month;

            if (day < 10)
            {
                date += "0";
            }
            date += day;

            date += this.RandomString("0123456789", 4, 4, false, false);
            return date;
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
            while (count < 5000)
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
            var command = new OracleCommand();
            command.Connection = new OracleConnection("User Id=c##local;Password=oracle;Data Source=25.48.253.17:1521/xe;");
            command.Connection.Open();
            count = 0;
            var people = new List<Person>();
            while (count != 100000)
            {
                //Generate values
                var id = this.GeneratePersonId();
                var zip = cities[random.Next(cities.Count)].Zip;
                var firstName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                var lastName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                var phone = "+4219" + this.RandomString("0123456789", 8, 8, false, false);
                var email = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 10, false, false) + "@"
                    + this.RandomString("abcdefghijklmnopqrstuvwxyz", 3, 8, false, false) + ".com";
                //Create command                
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "insert into person values(:ID, :ZIP,person_inf(:First_Name,:Last_Name,:Phone,:Email))";
                OracleParameter[] parameters = new OracleParameter[]
                {
                    new OracleParameter("ID", id),
                    new OracleParameter("ZIP", zip),
                    new OracleParameter("First_Name", firstName),
                    new OracleParameter("Last_Name", lastName),
                    new OracleParameter("Phone", phone),
                    new OracleParameter("Email", email)
                };
                command.Parameters.AddRange(parameters);
                //Generate values               
                try
                {
                    command.ExecuteNonQuery();
                    var newPerson = new Person();
                    newPerson.PersonId = id;
                    newPerson.Zip = zip;
                    people.Add(newPerson);
                    count++;
                }
                catch (Exception ex)
                {
                    var exeption = ex;
                }                
            }
            command.Connection.Close();

            //Hospitals
            var hospitals = new List<Hospital>();
            count = 0;
            while (count != 2500)
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
            while (count != 100)
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

            //Incurencies
            var NotInsured = new List<Person>(people);
            var Insured = new List<Person>();
            count = 0;
            while (count != 95000)
            {
                var person = NotInsured[random.Next(NotInsured.Count)];

                var year = Convert.ToInt32(person.PersonId.Substring(0, 2));
                if (year < 23)
                {
                    year += 2000;
                }
                else
                {
                    year += 1900;
                }
                var month = Convert.ToInt32(person.PersonId.Substring(2, 2)) % 50;
                var day = Convert.ToInt32(person.PersonId.Substring(4, 2));
                var birthDate = new DateTime(year, month, day).Date;

                var insCount = 0;
                if (random.Next(0,100) < 1)
                {
                    insCount = random.Next(5, 11);
                } else
                {
                    insCount = random.Next(1, 5);
                }                

                var firstDate = birthDate.AddDays(random.Next(0, (int)(DateTime.Now - birthDate).TotalDays)).Date;
                while (count < 95000 && insCount > 0 && firstDate < DateTime.Now.Date.AddDays(-1))
                {
                    var insurance = new Insurance();
                    insurance.PersonId = person.PersonId;
                    var comp = companies[random.Next(companies.Count)];
                    insurance.CompId = comp.CompId;
                    //Generate
                    var starDate = firstDate.AddDays(random.Next(0, (int)(DateTime.Now.Date - firstDate.Date).TotalDays)).Date;
                    insurance.DateStart = starDate;
                    insurance.DateEnd = starDate.AddDays(random.Next(0, (int)(DateTime.Now.Date - starDate.Date).TotalDays)).Date;

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
                        firstDate = (DateTime)insurance.DateEnd;
                        insCount--;
                    }
                }

                //Actual
                if(firstDate < DateTime.Now.Date.AddDays(-1))
                {
                    var insurance = new Insurance();
                    insurance.PersonId = person.PersonId;
                    var comRand = random.Next(0, 100);
                    InsuranceComp comp = null;
                    if(comRand < 75){
                        comp = companies[random.Next(10)];
                    }else
                    {
                        comp = companies[random.Next(companies.Count)];
                    }
                    
                    insurance.CompId = comp.CompId;

                    var starDate = firstDate.AddDays(random.Next(0, (int)(DateTime.Now.Date - firstDate.Date).TotalDays)).Date;
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
                        Insured.Add(person);
                    }
                }
                count++;
                NotInsured.Remove(person);
            }
            await _context.SaveChangesAsync();            

            //Payments
            count = 0;
            while (count != 100000)
            {
                var hospital = hospitals[random.Next(hospitals.Count)];
                var comp = companies[random.Next(companies.Count)];

                var payment = new Payment();
                payment.HospitalName = hospital.HospitalName;
                payment.CompId = comp.CompId;

                int days = random.Next(365 * 10);
                var date = DateTime.Now.AddDays(-days).Date;
                var period = date.AddDays(-random.Next(365));
                payment.PaymentDate = date;
                payment.PaymentPeriod = new DateTime(period.Year, period.Month, 1).Date;
                var XmlDetails = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(XmlDetails))
                {
                    writer.WriteStartElement("Payment");

                    writer.WriteStartElement("Sender");
                    writer.WriteAttributeString("CompId", comp.CompId);
                    writer.WriteElementString("Bank",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 10, true, false));
                    writer.WriteElementString("IBAN",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 2, 2, true, true) +
                        this.RandomString("0123456789", 22, 22, false, false));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Reciever");
                    writer.WriteAttributeString("HospName", hospital.HospitalName);
                    writer.WriteElementString("Bank",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 10, true, false));
                    writer.WriteElementString("IBAN",
                        this.RandomString("abcdefghijklmnopqrstuvwxyz", 2, 2, true, true) +
                        this.RandomString("0123456789", 22, 22, false, false));
                    writer.WriteEndElement();

                    writer.WriteElementString("Amount",
                       random.Next(500, 4000).ToString());

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
            while (count != 500)
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

            //Hospitalizations
            count = 0;
            while (count < 300000 && Insured.Count > 0)
            {
                var person = Insured[random.Next(Insured.Count)];
                var hospital = hospitals[random.Next(hospitals.Count)];

                var hospCount = random.Next(1, 9);
                var year = Convert.ToInt32(person.PersonId.Substring(0, 2));
                if (year < 23)
                {
                    year += 2000;
                }
                else
                {
                    year += 1900;
                }
                var month = Convert.ToInt32(person.PersonId.Substring(2, 2)) % 50;
                var day = Convert.ToInt32(person.PersonId.Substring(4, 2));
                var birthDate = new DateTime(year, month, day).Date;

                if (birthDate < DateTime.Now.AddYears(-10)) {
                    birthDate = DateTime.Now.AddYears(-10);
                }

                //Ended
                var firstDate = birthDate.AddDays(random.Next(0, (int)(DateTime.Now - birthDate).TotalDays)).Date;
                while (count < 300000 && hospCount > 0 && firstDate < DateTime.Now.Date.AddDays(-1) && Insured.Count > 0)
                {
                    var hospitalization = new Hospitalization();
                    hospitalization.HospitalName = hospital.HospitalName;
                    hospitalization.PersonId = person.PersonId;

                    //Generate
                    var starDate = firstDate.AddDays(random.Next(0, (int)(DateTime.Now - firstDate).TotalDays)).Date;
                    hospitalization.DateStart = starDate;
                    hospitalization.DateEnd = starDate.AddDays(random.Next(0, (int)(DateTime.Now - starDate).TotalDays)).Date;

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
                        hospCount--;
                        if(hospitalization.DateEnd != null)
                        {
                            firstDate = (DateTime)hospitalization.DateEnd;
                        }                        

                        //Diagnoses
                        var diagnosesCount = random.Next(1, 3);
                        while (diagnosesCount > 0)
                        {
                            var hospDiagnoze = new Diagnosis();
                            hospDiagnoze.HospitalName = hospitalization.HospitalName;
                            hospDiagnoze.PersonId = hospitalization.PersonId;
                            hospDiagnoze.DateStart = hospitalization.DateStart;
                            hospDiagnoze.DiagnosisId = diagnosesTypes[random.Next(diagnosesTypes.Count)].DiagnosisId;

                            if (random.Next(0, 5) == 0)
                            {
                                string filePath = "..\\..\\Pictures\\" + random.Next(1, 11) + ".jpg";
                                FileStream fls = null;
                                try
                                {
                                    fls = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                }
                                catch (Exception ex)
                                {
                                    var exept = ex;
                                }

                                //a byte array to read the image 
                                byte[] blob = new byte[fls.Length];
                                fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                                fls.Close();
                                hospDiagnoze.Document = blob;
                            }

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
                        Insured.Remove(person);
                    }
                }

                //Actual
                if(random.Next(0,2) == 1 && Insured.Count > 0)
                {
                    var hospitalization = new Hospitalization();
                    hospitalization.HospitalName = hospital.HospitalName;
                    hospitalization.PersonId = person.PersonId;

                    //Generate
                    var starDate = firstDate.AddDays(random.Next(0, (int)(DateTime.Now - firstDate).TotalDays)).Date;
                    hospitalization.DateStart = starDate;                    

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
                        while (diagnosesCount > 0)
                        {
                            var hospDiagnoze = new Diagnosis();
                            hospDiagnoze.HospitalName = hospitalization.HospitalName;
                            hospDiagnoze.PersonId = hospitalization.PersonId;
                            hospDiagnoze.DateStart = hospitalization.DateStart;
                            hospDiagnoze.DiagnosisId = diagnosesTypes[random.Next(diagnosesTypes.Count)].DiagnosisId;

                            if (random.Next(0, 5) == 0)
                            {
                                string filePath = "..\\..\\Pictures\\" + random.Next(1, 11) + ".jpg";
                                FileStream fls = null;
                                try
                                {
                                    fls = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                }
                                catch (Exception ex)
                                {
                                    var exept = ex;
                                }

                                //a byte array to read the image 
                                byte[] blob = new byte[fls.Length];
                                fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                                fls.Close();
                                hospDiagnoze.Document = blob;
                            }

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
                        Insured.Remove(person);
                    }
                }
            }
            await _context.SaveChangesAsync();


            TempData["Message"] = "Data successfully generated!";
            return RedirectToAction(nameof(Index));
        }
    }
}
