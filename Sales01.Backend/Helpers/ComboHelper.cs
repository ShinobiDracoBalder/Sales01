namespace Sales01.Backend.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sales01.Backend.Models;
    using Sales01.Domain.Models;

    public class ComboHelper : IDisposable
    {
        private static LocalDataContext db = new LocalDataContext();

        public static List<UserType> GetUserTypeId()
        {
            var userTypes = db.UserTypes.ToList();
            userTypes.Add(new UserType
            {
                UserTypeId = 0,
                Description = "[Select a User Type ......]",
            });

            return userTypes.OrderBy(d => d.UserTypeId).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}