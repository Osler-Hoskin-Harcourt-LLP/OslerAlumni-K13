using ECA.Core.Models;

namespace OslerAlumni.Mvc.Core.Models
{
    public class PasswordPolicyConfig 
        : IConfig
    {
        public int RequiredLength { get; set; }

        public bool RequireUppercase { get; set; }

        public bool RequireLowercase { get; set; }

        public bool RequireNonLetterOrDigit { get; set; }

        public bool RequireDigit { get; set; }
    }
}
