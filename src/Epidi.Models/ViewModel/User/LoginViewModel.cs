using Epidi.Enums;

namespace Epidi.Models.ViewModel.User
{
    public class LoginViewModel
    {
       
        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public string SocialId { get; set; }
        public string SocialType { get; set; }
    }
}
