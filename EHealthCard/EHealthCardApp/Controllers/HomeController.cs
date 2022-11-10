using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EHealthCardApp.Models;
using EHealthCardApp.Repository;

namespace EHealthCardApp.Controllers;

public class HomeController : Controller
{
    private readonly IRepository _repository;

    public HomeController(IRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult GenerateData()
    {
        try
        {
            var number = int.Parse(Request.Form["number"]);
        }
        catch(Exception ex)
        {
            TempData["Message"] = ex.Message;
            return View("Index");
        }  
        return View("Index");
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
