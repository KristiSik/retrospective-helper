using RetrospectiveHelper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetrospectiveHelper.Models
{
    public class ProjectViewModel
    {
        public ProjectViewModel(Project project)
        {
            Name = project.Name;
            Created = project.Created;
            Admins = project.Members.Where(m => m.Role == ProjectRoles.Admin).Select(u => u.User.FullName).ToList();
            Members = project.Members.Where(m => m.Role == ProjectRoles.Member).Select(u => u.User.FullName).ToList();
        }

        public string Name { get; set; }
        public DateTime Created { get; set; }

        public ICollection<string> Admins { get; set; }

        public ICollection<string> Members { get; set; }
    }
}