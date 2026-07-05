namespace dotnetapp.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }

    public int? InstructorId { get; set; }
    public Instructor? Instructor { get; set; }
}


namespace dotnetapp.Models;

public class Instructor
{
    public int InstructorId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime HireDate { get; set; }

    public ICollection<Course>? Courses { get; set; }
}


namespace dotnetapp.Models;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}


namespace dotnetapp.Models;

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}



using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<User> Users { get; set; }
}





using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InstructorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetInstructors")]
    public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
    {
        return Ok(await _context.Instructors
            .Include(i => i.Courses)
            .ToListAsync());
    }

    [HttpPost("PostInstructor")]
    public async Task<ActionResult<Instructor>> PostInstructor(Instructor instructor)
    {
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInstructors),
            new { id = instructor.InstructorId }, instructor);
    }

    [HttpPut("PutInstructor/{id}")]
    public async Task<IActionResult> PutInstructor(int id, Instructor instructor)
    {
        if (id != instructor.InstructorId)
            return BadRequest();

        _context.Entry(instructor).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("DeleteInstructor/{id}")]
    public async Task<IActionResult> DeleteInstructor(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);

        if (instructor == null)
            return NotFound();

        var hasCourses = await _context.Courses
            .AnyAsync(c => c.InstructorId == id);

        if (hasCourses)
            return Conflict("Instructor has associated courses");

        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CourseController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetCourses")]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        return Ok(await _context.Courses
            .Include(c => c.Instructor)
            .ToListAsync());
    }

    [HttpPost("PostCourse")]
    public async Task<ActionResult<Course>> PostCourse(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCourses),
            new { id = course.CourseId }, course);
    }

    [HttpPut("PutCourse/{id}")]
    public async Task<IActionResult> PutCourse(int id, Course course)
    {
        if (id != course.CourseId)
            return BadRequest();

        _context.Entry(course).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("DeleteCourse/{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return NotFound();

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}





using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly string[] validRoles =
    {
        "Admin",
        "Organizer"
    };

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        if (!validRoles.Contains(user.Role))
            return BadRequest("Invalid Role");

        var exists = await _context.Users
            .AnyAsync(x => x.Username == user.Username);

        if (exists)
            return Conflict("Username already exists");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created("", user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login(LoginModel user)
    {
        var data = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Username == user.Username &&
                x.Password == user.Password);

        if (data == null)
            return BadRequest("Login failed");

        return Ok(new
        {
            Message = "Login Successful",
            User = data
        });
    }
}




 
