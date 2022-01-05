using Microsoft.EntityFrameworkCore;
using SocialWorkouts.ApplicationDb.Models;
// using User.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    // public DbSet<PizzaEntity> Pizzas { get; set; }
    public DbSet<UserEntity> User { get; set; }
    public DbSet<MessageEntity> Message { get; set; }
    public DbSet<StandardUserEntity> StandardUser { get; set; }
    public DbSet<TrainerEntity> Trainer { get; set; }
    public DbSet<WorkoutPlanEntity> WorkoutPlan { get; set; }
    public DbSet<WorkoutEntity> Workout { get; set; }
    // public DbSet<ExerciseEntity> Exercises { get; set; }
    // public DbSet<SearchPreferencesEntity> SearchPreferences { get; set; }
    public DbSet<PastWorkoutRelation> PastWorkoutRelation { get; set; }
    public DbSet<FriendRelation> FriendRelation { get; set; }
    public DbSet<InvoiceEntity> Invoice { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }


}