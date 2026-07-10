using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace dotnetapp.Controllers
{
 
    public class AccountController : Controller
    {
        private readonly UserManager <IdentityUser> _userManager;
        private readonly SignInManager <IdentityUser> _signInManager;
       
        public AccountController(UserManager<IdentityUser>userManager,SignInManager<IdentityUser>signInManager){
            _userManager=userManager;
            _signInManager=signInManager;
        }
 
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register (RegisterViewModel model){
            if(ModelState.IsValid){
                IdentityUser user=new IdentityUser{
                    UserName=model.Email,
                    Email=model.Email
                };
                var result=await _userManager.CreateAsync(user,model.Password);
                if(result.Succeeded){
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index","Instructor");
                }
                foreach(var error in result.Errors){
                    ModelState.AddModelError("",error.Description);
                }  
 
            }
            return View(model);
        }
 
        [HttpGet]
        // [ValidateAntiForgeryToken]
         public async Task<IActionResult> Login (LoginViewModel model){
            if(ModelState.IsValid){
               
               
                var result=await _signInManager.PasswordSignInAsync(
                    model.Email!,model.Password!,true,false);
 
                if(result.Succeeded){
                   
                    return RedirectToAction("Index","Instructor");
                }
                ModelState.AddModelError("","Invalid login attempt.");  
 
            }
            return View(model);
        }
 
        [HttpGet]
        public async Task<IActionResult>Logout(){
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
 
        }
 
 
    }
}
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
 
namespace dotnetapp.Controllers
{
    [Route("courses")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static List<Course> Courses = new List<Course>();
        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }
 
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(Courses);
        }
 
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }
 
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseId = Courses.Count + 1;
                Courses.Add(course);
                return RedirectToAction("Index");
            }
            return View(course);
        }
 
 
        [Route("")]
        [HttpGet]
        public IActionResult IndexDbContext()
        {
            var courseList = _context.Courses.Include(c => c.Instructor).ToList();
            return View(courseList);
        }
 
 
        [Route("create")]
        [HttpGet]
        public IActionResult CreateDbContext()
        {
            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name");
            return View();
        }
 
 
        [Route("")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDbContext(Course course)
        {
 
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(IndexDbContext));
            }
            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name", course.InstructorId);
            return View(course);
        }
 
 
 
        [HttpGet("edit/{id?}")]
        [HttpGet("/Course/EditDbContext/{id?}")]
        public IActionResult EditDbContext(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name", course.InstructorId);
            return View(course);
        }
 
        [HttpPost("edit/{id?}")]
        [HttpPost("/Course/EditDbContext/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditDbContext(int id,Course course)
        {
            if(id !=course.CourseId){
                return NotFound();
            }
            if(ModelState.IsValid){
                _context.Courses.Update(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(IndexDbContext));
            }
 
            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "InstructorId", "Name", course.InstructorId);
            return View(course);
        }
 
 
 
 
        [HttpGet("delete/{id?}")]
        [HttpGet("/Course/DeleteDbContext/{id?}")]
        public IActionResult DeleteDbContext(int id)
        {
            var course = _context.Courses.Include(c => c.Instructor).FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
 
            return View(course);
        }
 
 
 
 
        [HttpPost("delete/{id?}")]
        [HttpPost("/Course/DeleteDbContext/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmedDbContext(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
 
            return RedirectToAction(nameof(IndexDbContext));
        }
    }
}
 
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
 
namespace dotnetapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
 
        public HomeController(ILogger<HomeController> logger)
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
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace dotnetapp.Controllers
{
   
    [Route("instructors")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;
 
        public InstructorController(ApplicationDbContext context){
            _context=context;
        }
 
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            var instructors=_context.Instructors.ToList();
 
            return View(instructors);
        }
 
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
 
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Instructor instructor)
        {
            if(ModelState.IsValid){
                _context.Instructors.Add(instructor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }
 
       
    }
}
 
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
 
 
namespace dotnetapp.Models
{
     public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext>options):base (options){
 
        }
        public DbSet<Course >Courses{get;set;}
        public DbSet<Instructor>Instructors{get;set;}
    }
}
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class Course
    {
        [Key]
        public int CourseId{get;set;}
        [Required(ErrorMessage="Title is required.")]
        public string? Title{get;set;}
        [Required(ErrorMessage="Description is required.")]
        public string? Description{get;set;}
        [Required(ErrorMessage="Duration is required.")]
        [Range(9,int.MaxValue,ErrorMessage="Duration must be greater than 8 hours.")]
        public int Duration{get;set;}
 
        [Display(Name="Instructor Name")]
        public int InstructorId{get;set;}
 
        [ForeignKey("InstructorId")]
        public Instructor? Instructor{get;set;}
 
    }
}
 
namespace dotnetapp.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
 
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorId{get;set;}
        [Required(ErrorMessage="Name is required.")]
        public String? Name { get; set; }
        [Required(ErrorMessage="Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
 
        public ICollection<Course>?Courses{get;set;}
    }
}
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class LoginViewModel
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
 
    }
}
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ? Password{ get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage="Password and ConfrimPassword must match.")]
        public string? ConfrimPassword { get; set; }
       
    }
}
 
@model dotnetapp.Models.LoginViewModel;
 
<h2>Login</h2>
 
<form method="post">
    <input asp-for="Email"/>
    <input asp-for="Password"/>
    <button type="submit">Login</button>
</form>
 
@model dotnetapp.Models.RegisterViewModel
 
<h2>Register</h2>
 
<form asp-action="Register" method="post">
 
    @*<label>Email</Email>*@
    <input type="text" id="Email" name="Email"/>
 
     @*<label>Password</Email>*@
    <input type="password" id="Password" name="Password"/>
 
     @*<label>Confirm Password</Email>*@
    <input type="password" id="ConfirmPassword" name="ConfirmPassword"/>
 
    <button type="submit">Register</button>
 
 
</form>
 
@model dotnetapp.Models.Course
 
<form asp-action="Edit" method="post">
    <input asp-for="CourseId" type="hidden"/>
 
    <label asp-for="Title"></label>
    <input asp-for="Title">
 
    <label asp-for="Description"></label>
    <input asp-for="Description">
 
    <label asp-for="Duration"></label>
    <input asp-for="Duration">
 
    <label asp-for="InstructorId"></label>
    <input asp-for="InstructorId">
 
 
 
    <button type="submit">Save</button>
</form>
 
@model dotnetapp.Models.Course
 
<h2>Edit Course</h2>
 
<form asp-action="Edit" method="post">
    <input asp-for="CourseId" type="hidden"/>
 
    <label asp-for="Title"></label>
    <input asp-for="Title">
 
    <label asp-for="Description"></label>
    <input asp-for="Description">
 
    <label asp-for="Duration"></label>
    <input asp-for="Duration">
 
    <label asp-for="InstructorId"></label>
    <input asp-for="InstructorId">
 
 
 
    <button type="submit">Save</button>
</form>
 
@{
    ViewData["Title"] = "Home Page";
}
 
<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <p>Learn about <a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>
</div>
 
ASP.NET documentation | Microsoft Learn
Learn to use ASP.NET Core to create web apps and services that are fast, secure, cross-platform, and cloud-based. Browse tutorials, sample code, fundamentals, API reference and more.
 
@{
    ViewData["Title"] = "Privacy Policy";
}
<h1>@ViewData["Title"]</h1>
 
<p>Use this page to detail your site's privacy policy.</p>
 
 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
builder.Services.AddControllersWithViews();
 
// Add your DbContext and Identity services
// ...
var cs = "User ID=sa; password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False";
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));
 
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}
 
app.UseHttpsRedirection();
app.UseStaticFiles();
 
app.UseRouting();
 
app.UseAuthorization();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");
 
app.Run();
 
 
 
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:20714",
      "sslPort": 44331
    }
  },
  "profiles": {
    "dotnetapp": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://0.0.0.0:8081",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
 
 
