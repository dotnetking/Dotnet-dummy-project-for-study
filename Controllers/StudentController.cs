using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class StudentController : Controller
    {

        private readonly TestContext _testContext;

        public StudentController(TestContext testContext)
        {
            _testContext = testContext;
        }
  
        // GET: Student/Create
        public IActionResult Create()
        {
            var viewModel = new StudentQualificationViewModel();
            return View(viewModel);
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentQualificationViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                // Save the student
                _testContext.Students.Add(viewModel.Student);
                await _testContext.SaveChangesAsync();

                // Save qualifications
                foreach (var qualification in viewModel.Qualifications)
                {
                    qualification.StudentId = viewModel.Student.Id; // Associate qualifications with the student
                    _testContext.QualificationDetails.Add(qualification);
                }

                await _testContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Student/Index
        public async Task<IActionResult> Index(string usernameFilter, string emailFilter, string search)
        {
            // Start by querying the Student and including their QualificationDetails
            var query = _testContext.Students
                        .Include(s => s.QualificationDetails)
                        .AsQueryable();

            // Apply filters based on the search query string parameters
            if (!string.IsNullOrEmpty(usernameFilter))
            {
                query = query.Where(s => s.Username.Contains(usernameFilter));
            }

            if (!string.IsNullOrEmpty(emailFilter))
            {
                query = query.Where(s => s.Email.Contains(emailFilter));
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search));
            }

            // Execute the query asynchronously and get the filtered students
            var students = await query
                .Select(s => new StudentQualificationViewModel
                {
                    Student = s,
                    Qualifications = s.QualificationDetails.ToList(),   
                })
                .ToListAsync();

            // Return the filtered list to the view
            return View(students);
        }


        public IActionResult IndexTwo(string? Search,string? GenderId) {

            var std = _testContext.Students.ToList();
            var GenreQry = _testContext.Students.Select(m=>m.Gender).ToList();

            var Gender = new List<string>() { "All" };
            Gender.AddRange(GenreQry.Distinct());

            ViewBag.Gender = new SelectList(Gender);

            if (!String.IsNullOrEmpty(Search))
            {
                std = std.Where(s => s.Username.Contains(Search)).ToList();
            }

            if (!string.IsNullOrEmpty(GenderId) && GenderId != "All")
            {
                std = std.Where(x => x.Gender == GenderId).ToList();
            }
            return View(std);
        }
    }
}
