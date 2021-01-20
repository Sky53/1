using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class DALHelper
    {
        private static List<User> users = new List<User> { 
            new User { Name = "Sky", Pass = "123" },
            new User { Name = "Din", Pass = "123" }
        };
        public static User Authorization(AuthorizationMessage request)
        {
            foreach (var item in users)
            {
                if (item.Name.Equals(request.UserName))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
