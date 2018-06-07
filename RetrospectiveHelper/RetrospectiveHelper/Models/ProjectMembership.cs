using RetrospectiveHelper.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetrospectiveHelper.Models
{
    public class ProjectMembership
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Key, Column(Order = 1)]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public ProjectRoles Role { get; set; }

    }
}