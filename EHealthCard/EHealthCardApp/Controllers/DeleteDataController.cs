using EHealthCardApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EHealthCardApp.Controllers
{
    public class DeleteDataController : Controller
    {
        private readonly EHealthCardContext _context;
        public DeleteDataController(EHealthCardContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            _context.Cities.RemoveRange(_context.Cities);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
