using ElectronicHealthCardApp.Models;
using ElectronicHealthCardApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EHealthCardApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly IRepository _repository;

        public PersonController(IRepository repository)
        {
            _repository = repository;
        }
        public IActionResult People()
        {
            var people = _repository.GetPeople();
            return View(people.Data);
        }
        public IActionResult AddPerson()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPerson(Person person)
        {
            if (ModelState.IsValid)
            {
                var result = _repository.AddPerson(person);
                TempData["Message"] = result.message;
                if (result.message == "Successfully added!")
                {
                    return RedirectToAction("People");
                } else
                {
                    return View();
                }       
            }

            return View();
        }
        public IActionResult UpdatePerson(string id)
        {
            var result = _repository.GetPersonById(id);

            return View(result.Data);
        }
        [HttpPost]
        public IActionResult UpdatePerson(Person person)
        {
            if (ModelState.IsValid)
            {
                var result = _repository.UpdatePerson(person);
                TempData["Message"] = result.message;
                if (result.message == "Successfully updated!")
                {
                    return RedirectToAction("People");
                }
                else
                {
                    return View();
                }
            }

            return View(person);
        }
        public IActionResult DeletePerson(string id)
        {
            var result = _repository.DeletePerson(id);
            TempData["Message"] = result.message;
            return RedirectToAction("People");
        }
    }
}
