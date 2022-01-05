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
public class FriendController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FriendController> _logger;

    public FriendController(ILogger<FriendController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary> Adds another user as a friend</summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="friendId">The id of the user they want to friend</param>
    /// <response code="201">Returns the new friend relation</response>
    /// <response code="400">That user is already a friend</response>
    /// <response code="404">One of the users could not be found</response>
    [HttpPost("")]
    public async Task<IActionResult> AddFriend(int userId, int friendId)
    {
        var user = await _context.User.FindAsync(userId);
        var friendUser = await _context.User.FindAsync(friendId);
        if (user == null || friendUser == null) return NotFound(ErrorCode.DoesNotExist("user"));

        var sameFriend = await _context.FriendRelation
        .FirstOrDefaultAsync(fr => fr.UserId == userId && fr.FriendId == friendId);
        if (sameFriend != null) return BadRequest("That user is already a friend");

        FriendRelation fr = new FriendRelation();
        fr.UserId = userId;
        fr.FriendId = friendId;

        await _context.AddAsync(fr);
        await _context.SaveChangesAsync();
        return CreatedAtAction("AddFriend", fr);
    }

    /// <summary>  Finds a list of all the friends of a user</summary>
    /// <param name="id">The id of the user</param>
    /// <response code="200">Returns a list of their friends</response>
    /// <response code="404">No user found with that id, or no friends found for that user</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<UserEntity>>> Friends(int id)
    {
        var user = await _context.User.FindAsync(id);
        if (user is null) return NotFound(ErrorCode.DoesNotExist("user"));
        List<FriendRelation> userFriendRelations = _context.FriendRelation.Where(fr => fr.UserId == id).ToList();
        return Ok(_context.User.AsEnumerable()
        .Join(userFriendRelations,
            user => user.Id,
            friendRelation => friendRelation.FriendId,
            (user, friendRelation) => user));
    }
}