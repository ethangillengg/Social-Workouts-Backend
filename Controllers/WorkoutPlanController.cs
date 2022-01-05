using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialWorkouts.ApplicationDb.Models;

namespace SocialWorkouts.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkoutPlanController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WorkoutPlanController> _logger;

    public WorkoutPlanController(ILogger<WorkoutPlanController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>Finds a list of all workout plans with matching fields to the provided one</summary>
    /// <param name="id">The id of the plan</param>
    /// <param name="planName">The name of the plan</param>
    /// <param name="creatorId">The id of the trainer who created the plan</param>
    /// <param name="minCost">The minimum cost of the plan</param>
    /// <param name="maxCost">The maximum cost of the plan</param>
    /// <returns>All workout plans matching the provided one</returns>
    /// <response code="200">Returns the list of workout plans</response>
    /// <response code="404">There are no workout plans matching those parameters</response>
    [HttpGet("")]
    public IEnumerable<WorkoutPlanEntity> Query(int? id = null, string? planName = null,
    int? creatorId = null, double? minCost = null, double? maxCost = null)
    {
        return _context.WorkoutPlan.AsEnumerable().Where(plan => (
            (plan.Id == id || id == null) &&
            (plan.Name == planName || planName == null) &&
            (plan.TrainerId == creatorId || creatorId == null) &&
            (plan.Cost >= minCost || minCost == null) &&
            (plan.Cost <= maxCost || maxCost == null) &&
            (plan.Listed)));
    }

    /// <summary>Creates a new workout plan</summary>
    /// <param name="plan">The workout plan to create</param>
    /// <returns>The newly created workout plan</returns>
    /// <response code="201">Returns the newly created workout plan</response>
    /// <response code="400">The workout plan is null or it matches the id of another workout plan</response>
    [HttpPost]
    public async Task<IActionResult> Create(WorkoutPlanEntity plan)
    {
        var trainer = await _context.Trainer.FindAsync(plan.TrainerId);
        if (trainer is null) return BadRequest();
        var dupePlan = await _context.WorkoutPlan.FirstOrDefaultAsync(p =>
        p.Name == plan.Name);
        if (dupePlan is not null) return BadRequest();
        await _context.AddAsync(plan);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Create", plan);
    }

    /// <summary>Removes a workout plan from being listed on the website</summary>
    /// <param name="id">The id number of the workout plan</param>
    /// <returns>No content</returns>
    /// <response code="200">Returns the plan that was successfully unlisted</response>
    /// <response code="400">That plan is already unlisted</response>
    /// <response code="404">A workout plan with that name could not be found</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var planToDelete = await _context.WorkoutPlan.FindAsync(id);
        if (planToDelete is null) return NotFound();
        if (!planToDelete.Listed) return BadRequest();
        planToDelete.Listed = false;
        await _context.SaveChangesAsync();
        return Ok(planToDelete);
    }

    /// <summary>Returns a workout plan by id number</summary>
    /// <param name="id">The id number of the workout plan</param>
    /// <returns>The plan</returns>
    /// <response code="200">Returns the plan</response>
    /// <response code="404">workout plan with that name could not be found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<WorkoutPlanEntity>>> GetWorkouts(int id)
    {
        var plan = await _context.WorkoutPlan
        .Include(p => p.Workouts).FirstOrDefaultAsync(p => p.Id == id);
        if (plan is null) return NotFound();
        return Ok(plan);
    }

    /// <summary>Returns the sales of a workout plan by its name</summary>
    /// <returns>All invoices of the plan</returns>
    /// <param name="id">The id number of the workout plan</param>
    /// <response code="200">Returns the invoices</response>
    /// <response code="404">There is no workout plan with that id number</response>
    [HttpGet("{id}/Sales")]
    public async Task<ActionResult<int>> GetSales(int id)
    {
        var plan = await _context.WorkoutPlan.FindAsync(id);
        if (plan is null) return NotFound();
        return await _context.Invoice.CountAsync(i => i.WorkoutPlan.Id == id);
    }

}