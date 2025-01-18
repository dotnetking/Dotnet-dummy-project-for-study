using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class RateCalculationController : Controller
    {
        // Action to return the view with initial data
        public ActionResult Index()
        {
            return View();
        }

        // Action to handle AJAX request and calculate Total
        [HttpPost]
        public JsonResult CalculateTotal(RateCalculationModel model)
        {
            // Calculate Total (RatePerHour * NumberOfHours)
            model.Total = model.RatePerHour * model.NumberOfHours;

            // Return the calculated Total as JSON
            return Json(model);
        }
    }
    public class RateCalculationModel
    {
        public decimal RatePerHour { get; set; }
        public decimal NumberOfHours { get; set; }
        public decimal Total { get; set; }
    }
}
