using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElectronicHealthCardApp.Models;
using ElectronicHealthCardApp.Repository;

namespace ElectronicHealthCardApp.Controllers;

public class HomeController : Controller
{
    private readonly IRepository _repository;

    public HomeController(IRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var people = _repository.GetPeople();
        return View(people.Data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
