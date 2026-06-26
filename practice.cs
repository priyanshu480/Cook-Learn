
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Range(8, int.MaxValue, ErrorMessage = "Duration must be greater than 8.")]
        public int Duration { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Instructor
    {
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}




using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>()
                .HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId);
        }
    }
}





using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}








using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}






using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotnetapp.Controllers
{
    public class CourseController : Controller
    {
        static List<Course> courses = new List<Course>()
        {
            new Course { CourseId=1,Title="Course 1",Description="Demo Description 1",Duration=12 },
            new Course { CourseId=2,Title="Course 2",Description="Demo Description 2",Duration=20 },
            new Course { CourseId=3,Title="Course 3",Description="Demo Description 3",Duration=9 }
        };

        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(courses);

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Course c)
        {
            if (ModelState.IsValid)
            {
                c.CourseId = courses.Count + 1;
                courses.Add(c);
                return RedirectToAction("Index");
            }
            return View(c);
        }

        // DB
        public IActionResult IndexDbContext()
        {
            var data = _context.Courses.ToList();
            return View(data);
        }

        public IActionResult CreateDbContext()
        {
            ViewBag.Instructors = new SelectList(_context.Instructors, "InstructorId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateDbContext(Course c)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(c);
                _context.SaveChanges();
                return RedirectToAction("IndexDbContext");
            }
            return View(c);
        }
    }
}




using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Instructors.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Instructor i)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(i);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(i);
        }
    }
}






using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel m)
        {
            if (ModelState.IsValid)
                return RedirectToAction("IndexDbContext","Course");

            return View(m);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel m)
        {
            if (ModelState.IsValid)
                return RedirectToAction("Login");

            return View(m);
        }
    }
}






using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login","Account");
        }
    }
}
