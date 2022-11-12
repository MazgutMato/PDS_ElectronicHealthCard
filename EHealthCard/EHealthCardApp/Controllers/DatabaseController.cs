using EHealthCardApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EHealthCardApp.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly EHealthCardContext _context;
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
            //Delete hospitals
            _context.InsuranceComps.RemoveRange(_context.InsuranceComps);
            //Delete hospitals
            _context.Hospitals.RemoveRange(_context.Hospitals);
            //Delete people
            _context.People.RemoveRange(_context.People);
            //Delete cities
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

            //Generate cities
            var number = 0;
            var cities = new List<City>();
            while (number < 10000)
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
                    number++;
                    cities.Add(city);
                }
            }

            ////Generate people
            number = 0;
            var people = new List<Person>();
            while (number != 100000)
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
                    number++;
                    people.Add(person);
                }
            }

            //Generate hospitals
            var hospitals = new List<Hospital>();
            number = 0;
            while (number != 10000)
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
                    number++;
                    hospitals.Add(hospital);
                }
            }

            //Generate insurance companies
            var companies = new List<InsuranceComp>();
            number = 0;
            while (number != 100)
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
                    number++;
                    companies.Add(comp);
                }
            }

            //Generate incurencies ended
            number = 0;
            while (number != 60000)
            {
                var person = people[random.Next(people.Count)];
                var comp = companies[random.Next(companies.Count)];

                var insurance = new Insurance();
                insurance.PersonId = person.PersonId;
                insurance.CompId = comp.CompId;
                int days = random.Next(365 * 100);
                var starDate = DateTime.Now.AddDays(-days);
                var endDate = DateTime.Now.AddDays(-random.Next(days));
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
                    number++;
                }
            }

            //Generate incurencies
            number = 0;
            while (number != 80000)
            {
                var person = people[random.Next(people.Count)];
                var comp = companies[random.Next(companies.Count)];

                var insurance = new Insurance();
                insurance.PersonId = person.PersonId;
                insurance.CompId = comp.CompId;
                if (random.Next(2) == 0)
                {
                    int days = random.Next(365 * 100);
                    var starDate = DateTime.Now.AddDays(-days);
                    insurance.DateStart = starDate;
                }
                else
                {
                    int days = random.Next(365 * 100);
                    var starDate = DateTime.Now.AddDays(-days);
                    var endDate = DateTime.Now.AddDays(-random.Next(days));
                    insurance.DateStart = starDate;
                }
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
                    number++;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Data successfully generated!";
            return RedirectToAction(nameof(Index));
        }
    }
}
