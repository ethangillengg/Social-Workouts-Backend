using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using SocialWorkouts.ApplicationDb.Models;
using System.Linq;
using SocialWorkouts.Services;
using System.Collections;

namespace SocialWorkouts.Controllers
{
    [ApiController]
    public class ErrorCode : ControllerBase
    {
        private readonly ApplicationDbContext? _context;
        private readonly ILogger<FriendController>? _logger;

        // [HttpDelete]
        public static string DoesNotExist(string entityName)
        {
            return $"A {entityName} with that id does not exist";
        }
        // public async void VerifyExistence(Type type, int id)
        // {
        //     var entity = await 
        // }
    }
}