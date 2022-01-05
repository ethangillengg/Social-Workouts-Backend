using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialWorkouts.ApplicationDb.Models;
using SocialWorkouts.Controllers;

namespace SocialWorkouts.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ILogger<MessageController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary> Sends a message to another user</summary>
    /// <param name="message">The message</param>
    /// <response code="201">The message that was created successfully</response>
    /// <response code="404">Either one of the sending or receiving users do not exist</response>
    [HttpPost("send")]
    public async Task<ActionResult<MessageEntity>> SendTo(MessageEntity message)
    {
        var sender = await _context.User.FindAsync(message.SenderId);
        var receiver = await _context.User.FindAsync(message.ReceiverId);
        if (sender is null || receiver is null) return NotFound(ErrorCode.DoesNotExist("user"));
        await _context.AddAsync(message);
        await _context.SaveChangesAsync();
        return CreatedAtAction("SendTo", message);
    }

    /// <summary> Returns the received messages of a user</summary>
    /// <param name="id">The id of the user</param>
    /// <returns>Received messages</returns>
    /// <response code="200">Returns the received messages</response>
    /// <response code="404">User with that id could not be found</response>
    [HttpGet("{id}/inbox")]
    public async Task<ActionResult<IEnumerable<MessageEntity>>> GetReceived(int id)
    {
        var user = await _context.User.FindAsync(id);
        if (user is null) return NotFound(ErrorCode.DoesNotExist("user"));

        return await _context.Message.Where(msg => msg.ReceiverId == id).ToListAsync();
    }

    /// <summary> Returns the message history of a user</summary>
    /// <param name="id">The id of the user</param>
    /// <returns>The message history</returns>
    /// <response code="200">Returns the message history</response>
    /// <response code="404">User with that id could not be found</response>
    [HttpGet("{id}/history")]
    public async Task<ActionResult<IEnumerable<MessageEntity>>> GetSent(int id)
    {
        var user = await _context.User.FindAsync(id);
        if (user is null) return NotFound(ErrorCode.DoesNotExist("user"));

        return await _context.Message.Where(msg => msg.SenderId == id).ToListAsync();
    }


    /// <summary> Returns a thread of messages between two users, sorted chronologically</summary>
    /// <param name="user1Id">The id of the first user</param>
    /// <param name="user2Id">The id of the second user</param>
    /// <returns>Matching messages</returns>
    /// <response code="200">Returns the messages</response>
    /// <response code="404">User with that id could not be found</response>
    [HttpGet("thread")]
    public async Task<ActionResult<IEnumerable<MessageEntity>>> GetMessageThread(int user1Id, int user2Id)
    {
        var user1 = await _context.User.FindAsync(user1Id);
        if (user1 is null) return NotFound(ErrorCode.DoesNotExist("user"));
        var user2 = await _context.User.FindAsync(user2Id);
        if (user2 is null) return NotFound(ErrorCode.DoesNotExist("user"));
        var thread = await _context.Message.Where(msg =>
        (msg.SenderId == user1Id || msg.ReceiverId == user1Id) &&
        (msg.SenderId == user2Id || msg.ReceiverId == user2Id))
        .OrderByDescending(msg => msg.DateSent).ToListAsync();
        return Ok(thread);
    }

}