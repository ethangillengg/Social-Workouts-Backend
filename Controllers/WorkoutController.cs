using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialWorkouts.ApplicationDb.Models;
using SocialWorkouts.Services;

namespace SocialWorkouts.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WorkoutController> _logger;

    public WorkoutController(ILogger<WorkoutController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>Returns the next workout of a user from their id</summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>The workout</returns>
    /// <response code="200">Returns the workout</response>
    /// <response code="404">Standard user with that id could not be found</response>
    [HttpGet("{userId}/NextWorkout")]
    public async Task<ActionResult<WorkoutEntity>> GetNextWorkout(int userId)
    {
        var user = await _context.StandardUser.Include(u => u.NextWorkout)
        .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return NotFound("Could not find that standard user");

        var workout = user.NextWorkout;
        if (workout is null) return NotFound("That user has no next workout set");

        return workout;
    }

    /// <summary>Logs a workout that a user finished</summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="workoutId">The id of the workout they completed</param>
    /// <returns>No content</returns>
    /// <response code="201">Returns the workout log</response>
    /// <response code="404">A user or workout with that id could not be found</response>
    [HttpPost("{userId}/Log")]
    public async Task<ActionResult<PastWorkoutRelation>> LogWorkout(int userId, int workoutId)
    {
        var user = await _context.StandardUser.FindAsync(userId);
        if (user is null) return NotFound();
        var workout = await _context.Workout.FindAsync(workoutId);
        if (workout is null) return NotFound();
        PastWorkoutRelation log = new PastWorkoutRelation();
        log.Workout = workout;
        log.User = user;
        await _context.AddAsync(log);
        await _context.SaveChangesAsync();
        return CreatedAtAction("LogWorkout", log);
    }

    /// <summary>Returns the workout history of a user from their id</summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>The past workouts</returns>
    /// <response code="200">Returns the workouts</response>
    /// <response code="404">User with that id could not be found</response>
    [HttpGet("{userId}/History")]
    public async Task<ActionResult<IEnumerable<WorkoutEntity>>> GetPastWorkouts(int userId)
    {
        var user = await _context.StandardUser.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return NotFound("Could not find that standard user");
        var allPastWorkouts = await _context.PastWorkoutRelation.Include(pw => pw.Workout)
        .OrderByDescending(w => w.DateRecorded).ToListAsync();
        var userPastWorkouts = allPastWorkouts.Where(pw => pw.UserId == userId);
        return Ok(userPastWorkouts);
    }

    /// <summary>Updates a user's next workout</summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="workoutId">The new next workout's id number</param>
    /// <returns>No content</returns>
    /// <response code="200">Returns the updated next workout</response>
    /// <response code="404">A user or workout with that id could not be found</response>
    [HttpPut("{userId}/NextWorkout")]
    public async Task<ActionResult<WorkoutEntity>> UpdateNextWorkout(int userId, int workoutId)
    {
        var user = await _context.StandardUser.FindAsync(userId);
        if (user is null) return NotFound("Could not find that standard user");
        var workout = await _context.Workout.FindAsync(workoutId);
        if (workout is null) return NotFound("Could not find that workout");
        user.NextWorkout = workout;
        await _context.SaveChangesAsync();
        return Ok(workout);
    }

    /// <summary>Adds a new workout to the db</summary>
    /// <param name="workout">The new workout</param>
    /// <returns>No content</returns>
    /// <response code="201">Returns the workout that was created</response>
    /// <response code="404"></response>
    [HttpPost("")]
    public async Task<ActionResult<WorkoutEntity>> PostWorkout(WorkoutEntity workout)
    {
        foreach (Exercise e in workout.Exercises)
        {
            await _context.AddAsync(e);
        }
        await _context.AddAsync(workout);
        await _context.SaveChangesAsync();
        return CreatedAtAction("PostWorkout", workout);
    }


}