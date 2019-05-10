using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TaskGroupWeb.Models;

namespace TaskGroupWeb.Helpers
{
    public class UserUtilities
    {
        public static bool UserIsTaskOwn(IEnumerable<Claim> claims, TaskModel task)
        {
            return int.Parse(claims.FirstOrDefault(c => c.Type == "userId").Value) == task.userOwnId;
        }
    }
}
