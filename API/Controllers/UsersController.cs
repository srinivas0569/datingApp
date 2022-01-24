using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserData>>> GetUsers()
        {
            return await _context.UserData.ToListAsync();
        }
    }
    [ApiController]
    [Route("api/User/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly DataContext _context;
        public UserDataController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserData>> GetUserDatabyId(int id)
        {
            return await _context.UserData.FindAsync(id);
        }
        [HttpPut]
        public async Task<ActionResult<IEnumerable<UserData>>> EditUserData([FromBody] UserData request)
        {
            var user = _context.UserData.Find(request.Id);
            if (user != null)
            {
               _context.Entry(user).CurrentValues.SetValues(request);
            _context.SaveChanges();
            }
            return await _context.UserData.ToListAsync();
        }
         [HttpPost]
        public async Task<ActionResult<IEnumerable<UserData>>> AddUser([FromBody] UserData request)
        {
            await _context.UserData.AddAsync(request);
            _context.SaveChanges();
            return await _context.UserData.ToListAsync();
        }
         [HttpDelete]
        public async Task<ActionResult<IEnumerable<UserData>>> DeleteUser(int id)
        {
            var user = _context.UserData.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.UserData.Remove(user);
            }
            _context.SaveChanges();

            return await _context.UserData.ToListAsync();
        }
    }
}