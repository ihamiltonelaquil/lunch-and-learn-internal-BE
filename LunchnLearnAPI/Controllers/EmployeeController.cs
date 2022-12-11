using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LunchnLearnAPI.Controllers
{
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly TestDbContext _context;

        public EmployeeController(TestDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var employee = new Employee()
            {
                Id = 1,
                Name = "test",
                TestString = "Another string"
            };
            return View();
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            var employee = new Employee()
            {
                Id = 1,
                Name = "test",
                TestString = "Another string"
            };
            
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
