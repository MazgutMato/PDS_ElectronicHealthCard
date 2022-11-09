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
        public IActionResult Index()
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

                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }

            return View(person);
        }
        public IActionResult DeletePerson(string id)
        {
            var result = _repository.DeletePerson(id);
            return RedirectToAction("Index");
        }
    }
}
