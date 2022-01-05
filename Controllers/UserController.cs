using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using SocialWorkouts.ApplicationDb.Models;
using System.Linq;
using SocialWorkouts.Services;
using System.Collections;

namespace SocialWorkouts.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    // public ActionResult UserDoesNotExist { get => NotFound("A user with that id does not exist"); }
    // public async Task<bool> VerifyExistence(int userId)
    // {
    //     if (await _context.User.FindAsync(userId) is null) return false;
    //     return true;
    // }

    /// <summary>Gets all users matching the provided search parameters</summary>
    /// <param name="id">Their id number</param>
    /// <param name="firstName">Their first name</param>
    /// <param name="lastName">Their last name</param>
    /// <param name="email">Their email address</param>
    /// <param name="type">The type of user ("Trainer" or "StandardUser")</param>
    /// <response code="200">Returns the list of matching users</response>
    [HttpGet("")]
    public IEnumerable<UserEntity> Query(int? id = null, String? firstName = null,
    String? lastName = null, String? email = null, String? type = null)
    {
        //Append "Entity" to the end of the type
        //since our entity types in C# all end with "Entity"
        //ex. "Trainer" -> "TrainerEntity"
        if (type is not null) type += "Entity";
        //Get all users matching the parameters provided.
        //If a parameter was not provided (it is null), then
        //ignore it.
        return _context.User.AsEnumerable().Where(user => (
            (user.Id == id || id == null) &&
            (user.FName == firstName || firstName == null) &&
            (user.LName == lastName || lastName == null) &&
            (user.Email == email || email == null) &&
            (user.GetType().Name == type || type == null)));
    }


    /// <summary> Returns a user by their id number</summary>
    /// <param name="id">The id of the user</param>
    /// <returns>The user</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="404">User with that id could not be found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserEntity>> Get(int id)
    {
        //Return user with that id if it exists
        var user = await _context.User.FindAsync(id);
        //If it was not found, return notFound
        if (user is null) return NotFound();
        //Else, return the user
        return Ok(user);
    }

    /// <summary> Updates a users info</summary>
    /// <param name="id">The id of the user</param>
    /// <param name="user">The updated user</param>
    /// <returns>The updated user</returns>
    /// <response code="200">Returns the updated user</response>
    /// <response code="404">User with that id could not be found</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserEntity user)
    {
        await _context.SaveChangesAsync();
        user.id = id;
        _context.Update(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
        return Ok(user);
    }


    /// <summary> Deletes a user by id</summary>
    /// <param name="id">The id of the user</param>
    /// <returns>No content</returns>
    /// <response code="200">User that was successfully deleted</response>
    /// <response code="404">The user could not be found</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.User.FindAsync(id);
        if (user is null) return NotFound();
        _context.User.Remove(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }


    /// <summary>Logs in a user based on their username and password</summary>
    /// <remarks>The passwords of a user are hashed and salted, so they are safer</remarks>
    /// <param name="username">The username of the user</param>
    /// <param name="password">The password of the user</param>
    /// <response code="204">Correct password, returns the user</response>
    /// <response code="400">Incorrect password</response>
    /// <response code="404">User with that username could not be found</response>
    [HttpGet("Login")]
    public async Task<ActionResult> Login(string username, string password)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return NotFound();
        if (user.Login(password)) return Ok(user);
        else return BadRequest();
    }
}