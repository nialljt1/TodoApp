namespace Api.Models.Identity
{
    public partial class AspNetUserRole
    {
        public int RoleId { get; set; }
        public AspNetUserRole Role { get; set; }

        public int AspNetUserId { get; set; }
        public AspNetUser User { get; set; }

    }
}
