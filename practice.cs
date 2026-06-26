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

        [Required(ErrorMessage = "Duration is required.")]
        [Range(9, int.MaxValue, ErrorMessage = "Duration must be greater than 8 hours.")]
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



using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Instructor>()
                .HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Instructor>().HasData(
                new Instructor
                {
                    InstructorId = 1,
                    Name = "Demo",
                    Email = "demo@gmail.com",
                    HireDate = new DateTime(2025, 2, 1)
                },
                new Instructor
                {
                    InstructorId = 2,
                    Name = "Instructor 1",
                    Email = "instructor1@gmail.com",
                    HireDate = new DateTime(2023, 2, 1)
                }
            );
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
        [DataType(DataType.Password)]
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
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}


using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Course");
        }
    }
}

using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [Route("courses")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        private static List<Course> courses = new List<Course>()
        {
            new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Demo Description 1",
                Duration = 12
            },
            new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Demo Description 2",
                Duration = 20
            },
            new Course
            {
                CourseId = 3,
                Title = "Course 3",
                Description = "Demo Description 3",
                Duration = 9
            }
        };

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Week3 Day5 - Static List
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View(courses);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseId = courses.Count + 1;
                courses.Add(course);
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // Week3 Day6 - Database
        [HttpGet]
        [Route("db")]
        public IActionResult IndexDbContext()
        {
            var data = _context.Courses
                .Include(c => c.Instructor)
                .ToList();

            return View(data);
        }

        [HttpGet]
        [Route("db/create")]
        public IActionResult CreateDbContext()
        {
            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name");
            return View();
        }

        [HttpPost]
        [Route("db/create")]
        public IActionResult CreateDbContext(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction("IndexDbContext");
            }

            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name", course.InstructorId);
            return View(course);
        }

        // Week4 Day4 - Edit
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult EditDbContext(int id)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
            {
                return NotFound();
            }

            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name", course.InstructorId);
            return View(course);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult EditDbContext(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
                return RedirectToAction("IndexDbContext");
            }

            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name", course.InstructorId);
            return View(course);
        }

        // Week4 Day4 - Delete
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult DeleteDbContext(int id)
        {
            var course = _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public IActionResult DeleteConfirmedDbContext(int id)
        {
            var course = _context.Courses.Find(id);

            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

            return RedirectToAction("IndexDbContext");
        }
    }
}




using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    [Route("instructors")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View(_context.Instructors.ToList());
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructor);
        }
    }
}


using dotnetapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = _userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    _signInManager.SignInAsync(user, false).Wait();
                    return RedirectToAction("Index", "Course");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    false,
                    false).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Course");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login");
        }
    }
}




@model List<Course>

<h2>Course List</h2>

<a href="/courses/create">Create New Course</a>

<table class="table mt-3">
    <thead>
        <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Duration</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var course in Model)
        {
            <tr>
                <td>@course.Title</td>
                <td>@course.Description</td>
                <td>@course.Duration</td>
            </tr>
        }
    </tbody>
</table>



@model Course

<h2>Create Course</h2>

<form method="post" action="/courses/create">
    <div class="form-group mb-2">
        <label>Title</label>
        <input asp-for="Title" class="form-control" />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Description</label>
        <input asp-for="Description" class="form-control" />
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Duration</label>
        <input asp-for="Duration" class="form-control" />
        <span asp-validation-for="Duration" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Create</button>
</form>

<a href="/courses">Back to List</a>




@model List<Course>

<h2>Course List</h2>

<a href="/courses/db/create">Create Course</a>

<table class="table mt-3">
    <thead>
        <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Duration</th>
            <th>Instructor Name</th>
            <th>Actions</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var course in Model)
        {
            <tr>
                <td>@course.Title</td>
                <td>@course.Description</td>
                <td>@course.Duration</td>
                <td>@course.Instructor?.Name</td>
                <td>
                    <a href="/courses/edit/@course.CourseId">Edit</a> |
                    <a href="/courses/delete/@course.CourseId">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>





@model Course

<h2>Create Course</h2>

<form method="post" action="/courses/db/create">
    <div class="form-group mb-2">
        <label>Title</label>
        <input asp-for="Title" class="form-control" />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Description</label>
        <input asp-for="Description" class="form-control" />
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Duration</label>
        <input asp-for="Duration" class="form-control" />
        <span asp-validation-for="Duration" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Instructor Name</label>
        <select asp-for="InstructorId" asp-items="ViewBag.Instructors" class="form-control">
            <option value="">Select a Instructor</option>
        </select>
    </div>

    <button type="submit" class="btn btn-primary">Create</button>
</form>

<a href="/courses/db">Back to List</a>



@model Course

<h2>Edit Course</h2>

<form method="post" action="/courses/edit/@Model.CourseId">
    <input type="hidden" asp-for="CourseId" />

    <div class="form-group mb-2">
        <label>Title</label>
        <input asp-for="Title" class="form-control" />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Description</label>
        <input asp-for="Description" class="form-control" />
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Duration</label>
        <input asp-for="Duration" class="form-control" />
        <span asp-validation-for="Duration" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Instructor Name</label>
        <select asp-for="InstructorId" asp-items="ViewBag.Instructors" class="form-control">
        </select>
    </div>

    <button type="submit" class="btn btn-primary">Save</button>
</form>

<a href="/courses/db">Back to List</a>




@model Course

<h2>Delete Course</h2>

<h4>Are you sure you want to delete this course?</h4>

<hr />

<dl class="row">
    <dt class="col-sm-2">CourseId</dt>
    <dd class="col-sm-10">@Model.CourseId</dd>

    <dt class="col-sm-2">Name</dt>
    <dd class="col-sm-10">@Model.Title</dd>

    <dt class="col-sm-2">Age</dt>
    <dd class="col-sm-10">@Model.Description</dd>

    <dt class="col-sm-2">Condition</dt>
    <dd class="col-sm-10">@Model.Duration</dd>
</dl>

<form method="post" action="/courses/delete/@Model.CourseId">
    <button type="submit" class="btn btn-danger">Delete</button>
</form>

<a href="/courses/db">Back to List</a>



@model List<Instructor>

<h2>Instructor List</h2>

<a href="/instructors/create">Add Instructor</a>

<table class="table mt-3">
    <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>HireDate</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var instructor in Model)
        {
            <tr>
                <td>@instructor.Name</td>
                <td>@instructor.Email</td>
                <td>@instructor.HireDate</td>
            </tr>
        }
    </tbody>
</table>



@model Instructor

<h2>Create Instructor</h2>

<form method="post" actioniv class="form-group mb-2">
        <label>Name</label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Email</label>
        <input asp-for="Email" class="form-control" />
        <span asp-validation-for="Email" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>HireDate</label>
        <input asp-for="HireDate" type="date" class="form-control" />
    </div>

    <button type="submit" class="btn btn-primary">Create</button>
</form>

/instructorsBack to List</a>



@model RegisterViewModel

<h2>Register</h2>

<form method="post    <div class="form-group mb-2">
        <label>Email</label>
        <input asp-for="Email" name="Email" class="form-control" />
        <span asp-validation-for="Email" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Password</label>
        <input asp-for="Password" name="Password" type="password" class="form-control" />
        <span asp-validation-for="Password" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Confirm password</label>
        <input asp-for="ConfirmPassword" name="ConfirmPassword" type="password" class="form-control" />
        <span asp-validation-for="ConfirmPassword" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Register</button>
</form>

<p>
    I already have an account?
    /Account/LoginLogin</a>
</p>



@model LoginViewModel

<h2>Login</h2>

<form method="post" action="/Account/Login">
    <div classabel>Email</label>
        <input asp-for="Email" name="Email" class="form-control" />
        <span asp-validation-for="Email" class="text-danger"></span>
    </div>

    <div class="form-group mb-2">
        <label>Password</label>
        <input asp-for="Password" name="Password" type="password" class="form-control" />
        <span asp-validation-for="Password" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Login</button>
</form>

<p>
    Dont have an account?
    /Account/RegisterRegister</a>
</p>


