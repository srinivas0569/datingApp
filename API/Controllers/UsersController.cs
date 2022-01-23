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
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
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
            var user = _context.UserData.FirstOrDefault(u => u.Id == request.Id);
            if (user != null)
            {
                _context.UserData.Update(request);
                _context.SaveChanges();
            }
            return await _context.UserData.ToListAsync();
        }
         [HttpPost]
        public async Task<ActionResult<IEnumerable<AppUser>>> AddUser([FromBody] AppUser request)
        {
            await _context.Users.AddAsync(request);
            UserData user = new UserData();
            user.Id = request.Id;
            user.UserName = request.UserName;
            await _context.UserData.AddAsync(user);
            _context.SaveChanges();
            return await _context.Users.ToListAsync();
        }
         [HttpDelete]
        public async Task<ActionResult<IEnumerable<AppUser>>> DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                UserData userdata = new UserData();
                userdata.Id = id;
                _context.UserData.Remove(userdata);
            }
            _context.SaveChanges();

            return await _context.Users.ToListAsync();
        }
    }
}