namespace Sales01.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Newtonsoft.Json.Linq;
    using Sales01.API.Helpers;
    using Sales01.API.Models;
    using Sales01.Common.Models;
    using Sales01.Domain.Models;

    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private DataContext db = new DataContext();

        [HttpPost]
        [Authorize]
        [Route("GetUserByEmail")]
        public async Task<IHttpActionResult> GetUserByEmail(JObject form)
        {
            var email = string.Empty;
            dynamic jsonObject = form;
            try
            {
                email = jsonObject.Email.Value;
            }
            catch
            {
                return BadRequest("Missing parameter.");
            }

            var user = await db.Users.
                Where(u => u.UserName.ToLower() == email.ToLower())
                .Include(u => u.UserType)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.UserType.UserTypeId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        //[Route("Login/{user}/{pasword}")][Authorize]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(JObject form)
        {
            // db.Configuration.ProxyCreationEnabled = false;

            var email = string.Empty;
            var password = string.Empty;

            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
                password = jsonObject.Password.Value;
            }
            catch
            {
                return BadRequest("Incorrect call.");
            }

            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.Find(email, password);

            if (userASP == null)
            {
                return this.BadRequest("Wrong User or password");
            }

            var user = await db.Users
                .Where(u => u.UserName == email)
                .Include(u => u.UserType)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.UserType.UserTypeId)
                .FirstAsync();

            if (user == null)
            {
                return this.BadRequest("User not found");
            }

            var userResponse = new UserRequest
            {
                Address = user.Address,
                FirstName = user.FirstName,
                IsAdmin = userManager.IsInRole(userASP.Id, RolesHelper.ADMIN),
                IsUser = userManager.IsInRole(userASP.Id, RolesHelper.PowerUser),
                LastName = user.LastName,
                ImagePath = user.ImagePath,
                ImageMimeType = user.ImageMimeType,
                ImagenUser = user.ImagenUser,
                Telephone = user.Telephone,
                Cellphone = user.Cellphone,
                UserRequestId = user.UserId,
                UserName = user.UserName,
            };

            return Ok(userResponse);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(JObject form)
        {
            var email = string.Empty;
            var currentPassword = string.Empty;
            var newPassword = string.Empty;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
                currentPassword = jsonObject.CurrentPassword.Value;
                newPassword = jsonObject.NewPassword.Value;
            }
            catch
            {
                return BadRequest("Incorrect call");
            }

            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            if (userASP == null)
            {
                return BadRequest("Incorrect call");
            }

            var response = await userManager.ChangePasswordAsync(userASP.Id, currentPassword, newPassword);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors.FirstOrDefault());
            }

            return Ok("ok");
        }


        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(UserRequest userRequest)
        {
            if (userRequest.ImagenUser != null && userRequest.ImagenUser.Length > 0)
            {
                var stream = new MemoryStream(userRequest.ImagenUser);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Users";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    userRequest.ImagePath = fullPath;
                }
            }

            var answer = UsersHelper.CreateUserASP(userRequest);
            return Ok(answer);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}