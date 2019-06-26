using Objetos;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TaskGroupWeb.Models;

namespace TaskGroupWeb.Helpers
{
    public class UserUtilities
    {
        public static bool UserIsTaskOwner(IEnumerable<Claim> claims, TaskModel task)
        {
            return int.Parse(claims.FirstOrDefault(c => c.Type == "userId").Value) == task.userOwnId;
        }

        public static bool UserIsTaskOwner(IEnumerable<Claim> claims, Task task)
        {
            return int.Parse(claims.FirstOrDefault(c => c.Type == "userId").Value) == task.userOwnId;
        }

        public static int GetUserId(IEnumerable<Claim> claims)
        {
            return int.Parse(claims.FirstOrDefault(c => c.Type == "userId").Value);
        }
    }
}
