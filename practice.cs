dotnetapp:-

models:

course model:-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dotnetapp.Models
{
    public class Course
    {
        public int CourseId {get;set;}

        public string Title {get;set;}

        public string Description {get;set;}

        public int Duration {get;set;}
    
        public int? InstructorId {get;set;}

        [JsonIgnore]
        public Instructor? Instructor {get;set;}

    }
}

---------------

instructor:-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dotnetapp.Models
{
    public class Instructor
    {
        public int InstructorId{get;set;}

        public string Name{get;set;}

        public string Email {get;set;}

        public DateTime HireDate {get;set;}

        [JsonIgnore]
        public ICollection<Course> Courses{get;set;} = new List<Course>();
    }
}

------------------

ApplicationDbContext:-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){

        }

        public DbSet<Course> Courses{get;set;}

        public DbSet<Instructor> Instructors {get;set;}

        public DbSet<User> Users {get;set;}

        protected override void OnModelCreating(ModelBuilder m)
        {
            
            m.Entity<Instructor>()
            .HasMany(s=>s.Courses)
            .WithOne(c=>c.Instructor)
            .HasForeignKey(fk=>fk.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        }

        
    }
}

---------------------

User:-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetapp.Models
{
    public class User
    {
        public long Id {get;set;}

        public string Username {get;set;}

        public string Password {get;set;}

        public string Role {get;set;}
    }
}

--------------------
LoginModel:-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetapp.Models
{
    public class LoginModel
    {
        public string Username {get;set;}

        public string Password{get;set;}
    }
}

------------------------------

Controller:-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public CourseController(ApplicationDbContext db1)
        {
            db = db1;
        }

        [HttpGet("GetCourses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {

            try
            {
                var res = await db.Courses.Include(i => i.Instructor).ToListAsync();

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("PostCourse")]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            try
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCourses), new {id = course.CourseId}, course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("PutCourse/{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest(new { message = "Course ID mismatch." });
            }

            var existing = await db.Courses.FindAsync(id);

            if (existing == null)
            {
                return NotFound(new { message = "Course not found." });
            }

            try
            {
                existing.Title = course.Title;
                existing.Description = course.Description;
                existing.Duration = course.Duration;
                existing.InstructorId = course.InstructorId;

                await db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {

            try
            {
                var course = await db.Courses.FindAsync(id);

                if( course == null){
                    return NotFound(new {message = "Course not found."});
                }

                db.Courses.Remove(course);
                await db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


---------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        public ApplicationDbContext db;

        public InstructorController(ApplicationDbContext db1)
        {
            db = db1;
        }

        [HttpGet("GetInstructors")]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
        {

            try
            {
                var res = await db.Instructors.Include(i => i.Courses).ToListAsync();

                return res;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("PostInstructor")]
        public async Task<ActionResult<Instructor>> PostInstructor(Instructor instructor)
        {
            try
            {
                db.Instructors.Add(instructor);
                await db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetInstructors), new {id = instructor.InstructorId} ,instructor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("PutInstructor/{id}")]
        public async Task<IActionResult> PutInstructor(int id, Instructor instructor)
        {
            if (id != instructor.InstructorId)
            {
                return BadRequest(new { message = "Instructor ID mismatch." });
            }

            var existing = await db.Instructors.FindAsync(id);

            if (existing == null)
            {
                return NotFound(new { message = "Instructor not found." });
            }

            try
            {
                existing.Name = instructor.Name;
                existing.Email = instructor.Email;
                existing.HireDate = instructor.HireDate;

                await db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("DeleteInstructor/{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {

            try
            {
                var instructor = await db.Instructors.Include(i => i.Courses).FirstOrDefaultAsync(i=> i.InstructorId==id);

                if( instructor == null){
                    return NotFound(new {message = "Instructor not found."});
                }

                if(instructor.Courses != null && instructor.Courses.Any()){
                    return Conflict( new {message = "Cannot delete instructor with associated courses."});
                }

                db.Instructors.Remove(instructor);
                await db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        private readonly string[] validRoles = {
            "Admin",
            "Organizer"
        };

        public UserController(ApplicationDbContext db1){
            db = db1;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user){

            if( !IsValidRole(user.Role)){
                return BadRequest("Ivalid role provided.");
            }

            bool usernameExists = db.Users.Any(u=> u.Username == user.Username);

            if(usernameExists){
                return Conflict("Username already exists");
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new {id = user.Id}, user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginModel user){
            var existingUser = db.Users.FirstOrDefault(u => 
                u.Username == user.Username &&
                u.Password == user.Password );

            if(existingUser == null){
                return BadRequest(new {
                    message = "Login failed."
                });
            }

            return Ok(new{
                message = "Login successfull",
                user = existingUser
            });
        }

        private bool IsValidRole(string role){
            return validRoles.Contains(role);
        }
    }
}


------------------------------

Program.cs

using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => {options.JsonSerializerOptions.PropertyNamingPolicy = null;});

//builder.Services.AddAuthentication("Basic Authentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic Authentication" , null);
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


-------------------------------

appsetting.json

{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=sa;password=examlyMssql@123;Server=localhost;Database=appdb;trusted_connection=false;persist security info=false;encrypt=false"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}


