//using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Identity;

//namespace ChronicleKeeper.Core.Entities.Security
//{
//    public class User : IdentityUser
//    {
//        public User()
//        {
//            if (!Roles.Any())
//            {
//                Roles.Add(new Role { Name = "Reader" });
//            }
//        }
//        [Key]
//        public int Id { get; set; }
//        public string Username { get; set; } = string.Empty;
//        public string Email { get; set; } = string.Empty;
//        public string PasswordHash { get; set; } = string.Empty;

//        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
//    }
//}
