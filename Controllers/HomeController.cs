using System.Diagnostics;
using CharityApp.Data;
using CharityApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CharityApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        // Constructor to inject ApplicationDbContext
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Index action that fetches donations data and displays the home page
        public async Task<IActionResult> Index()
        {
            // Fetch the donation data (optional: limit it or fetch a sample)
            var donations = await _context.Donations.ToListAsync();

            // Pass the data to the View (this can be used in the home page if needed)
            return View(donations);
        }

        // Privacy page (optional)
        public IActionResult Privacy()
        {
            return View();
        }

        // Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
