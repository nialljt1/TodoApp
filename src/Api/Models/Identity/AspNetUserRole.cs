using System.ComponentModel.DataAnnotations;

namespace Api.Models.Identity
{
    public partial class AspNetUserRole
    {
        [StringLength(450)]
        public string RoleId { get; set; }
        public AspNetUserRole Role { get; set; }

        [StringLength(450)]
        public string AspNetUserId { get; set; }
        public AspNetUser User { get; set; }
    }
}
