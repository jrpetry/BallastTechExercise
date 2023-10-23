using MagazineManager.Models;
using MagazineManager.Server.Controllers.BLL;
using MagazineManager.Server.Data.Repositories.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManager.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize] //All methods of this controller accept only authorized requests
    public class ApplicationUserController : ControllerBase
    {
        //Injected
        private readonly ILogger<ApplicationUserController> _logger;
        private readonly ApplicationUserBLL _applicationUserBLL;

        public ApplicationUserController(ILogger<ApplicationUserController> logger, BaseRepository<ApplicationUser> repo)
        {
            _logger = logger;
            _applicationUserBLL = new ApplicationUserBLL(repo);
        }

        // GET: api/<MagazineController>
        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            var applicationUsers = _applicationUserBLL.GetAll();
            return applicationUsers.ToArray();
        }

        // GET: api/<MagazineController>
        [HttpGet("{name}")]
        public IEnumerable<ApplicationUser> GetByName(string name)
        {
            var applicationUsers = _applicationUserBLL.GetByName(name);
            return applicationUsers.ToArray();
        }

        // GET api/<MagazineController>/5
        [HttpGet("{id}")]
        public ApplicationUser Get(int id)
        {
            var applicationUser = _applicationUserBLL.Get(id);
            return applicationUser;
        }

        // POST api/<MagazineController>
        [HttpPost]
        public void Post([FromBody] ApplicationUser value)
        {
            _applicationUserBLL.Save(value);
        }

        // PUT api/<MagazineController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ApplicationUser value)
        {
            _applicationUserBLL.Save(value);
        }

        // DELETE api/<MagazineController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _applicationUserBLL.Delete(id);
        }
    }
}
