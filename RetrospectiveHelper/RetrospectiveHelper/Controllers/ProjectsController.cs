using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RetrospectiveHelper.Models;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using RetrospectiveHelper.Enums;

namespace RetrospectiveHelper.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/Projects
        public IHttpActionResult GetProjects()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return Unauthorized();
            }
            var projects = user.Projects.Select(p => new ProjectViewModel(p.Project)).ToList();
            return Ok(projects);
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(new ProjectViewModel(project));
        }

        // PUT: api/Projects/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [HttpPost]
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> CreateProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return Unauthorized();
            }

            var creatorMembership = new ProjectMembership
            {
                UserId = user.Id,
                ProjectId = project.Id,
                Role = ProjectRoles.Admin
            };
            project.Members = new List<ProjectMembership> { creatorMembership };

            db.Projects.Add(project);
            await db.SaveChangesAsync();
            // eager loading
            project = db.Projects.Where(p => p.Id == project.Id).Include(p => p.Members.Select(m => m.User)).First();
            return Ok(new ProjectViewModel(project));
        }

        // TODO: Refactor these methods - code duplication

        // POST api/Projects/AddMember
        [HttpPost]
        [Route("AddMember")]
        public async Task<IHttpActionResult> AddMember(AddUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var project = db.Projects.Find(model.ProjectId);
            if (project == null)
            {
                return BadRequest();
            }

            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var projectMembership = currentUser.Projects.FirstOrDefault(p => p.Role == ProjectRoles.Admin && p.ProjectId == project.Id);
            if (projectMembership == null)
            {
                return Unauthorized();
            }

            var newMember = UserManager.FindByEmail(model.Email);
            if (newMember == null)
            {
                return BadRequest();
            }

            var newMemberProjectMembership = newMember.Projects.FirstOrDefault(p => p.ProjectId == project.Id);
            if (newMemberProjectMembership == null)
            {
                var newMembership = new ProjectMembership
                {
                    UserId = newMember.Id,
                    ProjectId = project.Id,
                    Role = ProjectRoles.Member
                };
                project.Members.Add(newMembership);
            
            }
            else if (newMemberProjectMembership.Role == ProjectRoles.Admin)
            {
                project.Members.First(p => p.UserId == newMember.Id).Role = ProjectRoles.Member;
            }

            await db.SaveChangesAsync();
            // eager loading
            project = db.Projects.Where(p => p.Id == project.Id).Include(p => p.Members.Select(m => m.User)).First();
            return Ok(new ProjectViewModel(project));
        }

        // POST api/Projects/AddMember
        [HttpPost]
        [Route("AddAdmin")]
        public async Task<IHttpActionResult> AddAdmin(AddUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var project = db.Projects.Find(model.ProjectId);
            if (project == null)
            {
                return BadRequest();
            }

            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var projectMembership = currentUser.Projects.FirstOrDefault(p => p.Role == ProjectRoles.Admin && p.ProjectId == project.Id);
            if (projectMembership == null)
            {
                return Unauthorized();
            }

            var newMember = UserManager.FindByEmail(model.Email);
            if (newMember == null)
            {
                return BadRequest();
            }

            var newMemberProjectMembership = newMember.Projects.FirstOrDefault(p => p.ProjectId == project.Id);
            if (newMemberProjectMembership == null)
            {
                var newMembership = new ProjectMembership
                {
                    UserId = newMember.Id,
                    ProjectId = project.Id,
                    Role = ProjectRoles.Admin
                };
                project.Members.Add(newMembership);

            }
            else if (newMemberProjectMembership.Role == ProjectRoles.Member)
            {
                project.Members.First(p => p.UserId == newMember.Id).Role = ProjectRoles.Admin;
            }

            await db.SaveChangesAsync();
            project = db.Projects.Where(p => p.Id == project.Id).Include(p => p.Members.Select(m => m.User)).First();
            return Ok(new ProjectViewModel(project));
        }

        // DELETE api/Projects/RemoveMember
        [HttpDelete]
        [Route("RemoveMember")]
        public async Task<IHttpActionResult> RemoveMember(AddUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var project = db.Projects.Find(model.ProjectId);
            if (project == null)
            {
                return BadRequest();
            }

            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var projectMembership = currentUser.Projects.FirstOrDefault(p => p.Role == ProjectRoles.Admin && p.ProjectId == project.Id);
            if (projectMembership == null)
            {
                return Unauthorized();
            }

            var selectedUser = UserManager.FindByEmail(model.Email);
            if (selectedUser == null)
            {
                return BadRequest();
            }

            var selectedUserProjectMembership = selectedUser.Projects.FirstOrDefault(p => p.ProjectId == project.Id);
            if (selectedUserProjectMembership == null)
            {
                return BadRequest();

            }
            else 
            {
                var membership = project.Members.First(p => p.UserId == selectedUser.Id);
                project.Members.Remove(membership);
            }

            await db.SaveChangesAsync();
            return Ok(new ProjectViewModel(project));
        }

        // DELETE api/Projects/Leave
        [HttpDelete]
        [Route("Leave")]
        public async Task<IHttpActionResult> Leave(LeaveBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var project = db.Projects.Find(model.ProjectId);
            if (project == null)
            {
                return BadRequest();
            }

            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var projectMembership = project.Members.FirstOrDefault(p => p.UserId == currentUser.Id);
            if (projectMembership == null)
            {
                return Unauthorized();
            }

            project.Members.Remove(projectMembership);

            await db.SaveChangesAsync();
            return Ok(new ProjectViewModel(project));
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.Id == id) > 0;
        }
    }
}