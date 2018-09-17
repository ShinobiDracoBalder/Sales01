namespace Sales01.Backend.Helpers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Sales01.Backend.Models;
    using Sales01.Common.Models;
    using Sales01.Domain.Models;

    public class DBHelper
    {
        public async static Task<Response> SaveChanges(LocalDataContext db)
        {
            try
            {
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, };
            }
            catch (Exception ex)
            {
                var response = new Response { IsSuccess = false, };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "There is a record with the same value";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "The record can't be delete because it has related records";
                }
                else
                {
                    response.Message = ex.Message;
                }

                return response;
            }
        }
        public static int GetUserTypes(string description, LocalDataContext db)
        {
            var state = db.UserTypes
                .Where(s => s.Description == description)
                .FirstOrDefault();

            if (state == null)
            {
                state = new UserType
                {
                    Description = description,
                };

                db.UserTypes.Add(state);

                db.SaveChanges();
            }
            return state.UserTypeId;
        }
    }
}