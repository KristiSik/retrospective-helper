using System;
using System.Collections.Generic;

namespace RetrospectiveHelper.Models
{
    public class Project
    {
        public Project()
        {
            Created = DateTime.Now;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public virtual ICollection<ProjectMembership> Members { get; set; }
    }
}