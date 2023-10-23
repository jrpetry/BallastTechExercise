using MagazineManager.Models;
using MagazineManager.Server.Controllers.BLL;
using MagazineManager.Server.Data.Repositories.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize] //All methods of this controller accept only authorized requests
    public class MagazineController : ControllerBase
    {
        private readonly ILogger<MagazineController> _logger;
        private readonly MagazineBLL _magazineBLL;

        public MagazineController(ILogger<MagazineController> logger, BaseRepository<Magazine> repo, BaseRepository<ApplicationUser> repositoryApplicationUser)
        {
            _magazineBLL = new MagazineBLL(repo, repositoryApplicationUser);
            _logger = logger;
        }

        // GET: api/<MagazineController>
        [HttpGet]
        public IEnumerable<Magazine> Get()
        {
            var magazines = _magazineBLL.GetAll();
            return magazines.ToArray();
        }

        // GET: api/<MagazineController>
        [HttpGet("{name}")]
        public IEnumerable<Magazine> GetByName(string name)
        {
            var magazines = _magazineBLL.GetByName(name);
            return magazines.ToArray();
        }

        // GET: api/<MagazineController>
        [HttpGet("{applicationUserId}")]
        public IEnumerable<Magazine> GetByApplicationUserId(int applicationUserId)
        {
            var magazines = _magazineBLL.GetByApplicationUserId(applicationUserId);
            return magazines.ToArray();
        }

        // GET api/<MagazineController>/5
        [HttpGet("{id}")]
        public Magazine Get(int id)
        {
            var magazine = _magazineBLL.Get(id);
            return magazine;
        }

        // POST api/<MagazineController>
        [HttpPost]
        public void Post([FromBody] Magazine value)
        {
            _magazineBLL.Save(value);
        }

        // PUT api/<MagazineController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Magazine value)
        {
            _magazineBLL.Save(value);
        }

        // DELETE api/<MagazineController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _magazineBLL.Delete(id);
        }
    }
}
