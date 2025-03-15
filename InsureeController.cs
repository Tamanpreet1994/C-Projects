using System.Diagnostics;
using ASP.Net_MVC_Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_MVC_Assignment.Controllers
{
    public class InsureeController : Controller
    {
        private readonly ILogger<InsureeController> _logger;

        public InsureeController(ILogger<InsureeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
}
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create(Insuree insuree)
{
    if (ModelState.IsValid)
    {
        // Calculate the quote
        insuree.Quote = CalculateQuote(insuree);

        db.Insurees.Add(insuree);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    return View(insuree);
}
private decimal CalculateQuote(Insuree insuree)
{
    decimal total = 50m; // Base monthly price

    // Calculate Age
    int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
    if (DateTime.Now < insuree.DateOfBirth.AddYears(age)) age--; // Adjust for birthdays not yet reached in the current year

    // Age-based pricing
    if (age <= 18) total += 100m;
    else if (age >= 19 && age <= 25) total += 50m;
    else if (age >= 26) total += 25m;

    // Car Year Adjustments
    if (insuree.CarYear < 2000) total += 25m;
    if (insuree.CarYear > 2015) total += 25m;

    // Make and Model Adjustments
    if (insuree.CarMake.ToLower() == "porsche")
    {
        total += 25m;
        if (insuree.CarModel.ToLower() == "911 carrera") total += 25m;
    }

    // Speeding Tickets
    total += insuree.SpeedingTickets * 10m;

    // DUI - Add 25%
    if (insuree.DUI) total *= 1.25m;

    // Full Coverage - Add 50%
    if (insuree.CoverageType.ToLower() == "full") total *= 1.50m;

    return total;
}
@model IEnumerable<CarInsurance.Models.Insuree>

@{
    ViewBag.Title = "Admin";
}

< h2 > Admin Panel - Issued Quotes </ h2 >

< table class= "table" >
    < tr >
        < th > First Name </ th >
        < th > Last Name </ th >
        < th > Email </ th >
        < th > Quote </ th >
    </ tr >

    @foreach(var insuree in Model)
    {
        < tr >
            < td > @insuree.FirstName </ td >
            < td > @insuree.LastName </ td >
            < td > @insuree.EmailAddress </ td >
            < td > @insuree.Quote.ToString("C") </ td >
        </ tr >
    }
</ table >
public ActionResult Admin()
{
    var insurees = db.Insurees.ToList();
    return View(insurees);
}
