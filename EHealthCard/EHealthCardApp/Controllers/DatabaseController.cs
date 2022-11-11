using EHealthCardApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                //Delete all data
                await this.DeleteDataAsync();

                Random random = new Random();

                //Generate cities
                Dictionary<string, City> cities = new Dictionary<string, City>();
                for (var i = 0; i < 10000; i++)
                {
                    var city = new City();
                    city.Zip = this.RandomString("0123456789", 5, 5, false, false);
                    city.CityName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                    if (!cities.ContainsKey(city.Zip))
                    {
                        cities.Add(city.Zip, city);
                    }
                }
                await _context.Cities.AddRangeAsync(cities.Values);

                //Generate people
                var citiesKeys = new List<string>(cities.Keys);
                Dictionary<string, Person> people = new Dictionary<string, Person>();
                for (var i = 0; i < 100000; i++)
                {
                    var person = new Person();
                    person.PersonId = this.RandomString("0123456789", 10, 10, false, false);
                    person.FirstName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                    person.LastName = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 20, true, false);
                    person.Phone = "+4219" + this.RandomString("0123456789", 8, 8, false, false);
                    person.Email = this.RandomString("abcdefghijklmnopqrstuvwxyz", 5, 10, false, false) + "@" 
                        + this.RandomString("abcdefghijklmnopqrstuvwxyz", 3, 8, false, false) + ".com";
                    person.Zip = citiesKeys[random.Next(citiesKeys.Count)];
                    if (!people.ContainsKey(person.PersonId))
                    {
                        people.Add(person.PersonId, person);
                    }
                }
                await _context.People.AddRangeAsync(people.Values);


                await _context.SaveChangesAsync();
                TempData["Message"] = "Data successfully generated!";
            }
            catch(Exception ex)
            {
                var message = ex.Message;
                TempData["Message"] = "Data generated faild!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
