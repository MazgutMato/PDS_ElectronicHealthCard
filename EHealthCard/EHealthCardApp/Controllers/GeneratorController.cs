using EHealthCardApp.Models;
using EHealthCardApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EHealthCardApp.Controllers
{
    public class GeneratorController : Controller
    {
        private readonly IRepository _repository;
        public GeneratorController(IRepository repository)
        {
            _repository = repository;
        }
        public IActionResult GenerateData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateData(Generator generator)
        {
            if (!ModelState.IsValid)
            {
                return View(generator);
            }

            var result = _repository.GenerateData(generator);
            TempData["Message"] = result.message;


            return View();
        }
    }
}
