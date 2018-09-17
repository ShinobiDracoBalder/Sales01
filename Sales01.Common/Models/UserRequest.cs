namespace Sales01.Common.Models
{
    public class UserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string ImagePath { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsUser { get; set; }

        public string ImageMimeType { get; set; }

        public string Cellphone { get; set; }

        public byte[] ImagenUser { get; set; }

        public int UserId { get; set; }
    }
}
