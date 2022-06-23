using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhiteRabbit_WebApi.DataAccess;
using WhiteRabbit_WebApi.Models;
using WhiteRabbit_WebApi.DTO;

namespace WhiteRabbit_WebApi.Controllers
{
    [Route("api/UserProfiles")]
    [ApiController]
    public class UserProfilesController : Controller
    {
        private readonly PostgreSqlContext _context;
        public UserProfilesController(PostgreSqlContext context)
        {
            _context = context;
        }

        // GET: api/UserProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetUserProfiles()
        {
            if (_context.UserProfiles == null)
            {
                return NotFound();
            }

            return await _context.UserProfiles
                                 .Select(x => UserProfileToDTO(x))
                                 .ToListAsync();
        }

        // GET: api/UserProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile(long id)
        {
            if (_context.UserProfiles == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return UserProfileToDTO(userProfile);
        }


        // PUT: api/UserProfiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(long id, UserProfileDTO profileDTO)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            userProfile.Firstname = profileDTO.Firstname;
            userProfile.Lastname = profileDTO.Lastname;
            userProfile.Email = profileDTO.Email;
            userProfile.Phonenumber = profileDTO.Phonenumber;
            userProfile.Gender = profileDTO.Gender;
            userProfile.City = profileDTO.City;
            userProfile.Date_of_birth = profileDTO.Date_of_birth;
            userProfile.AvatarUrl = profileDTO.AvatarUrl;  
                
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserProfileExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/UserProfiles
        [HttpPost]
        public async Task<ActionResult<UserProfileDTO>> CreateUserProfile(UserProfileDTO profileDTO) 
        {
            if (_context.UserProfiles == null)
            {
                return Problem("Entity set 'DbContext.UserProfiles' is null.");
            }

            var userProfile = new UserProfile
            {
                Firstname = profileDTO.Firstname,
                Lastname = profileDTO.Lastname,
                Email = profileDTO.Email,
                Phonenumber = profileDTO.Phonenumber,
                Gender = profileDTO.Gender,
                City = profileDTO.City,
                Date_of_birth = profileDTO.Date_of_birth,
                AvatarUrl = profileDTO.AvatarUrl
            };

            _context.UserProfiles.Add(userProfile);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserProfile), new { id = userProfile.Id }, UserProfileToDTO(userProfile));
        }

        // DELETE: api/UserProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(long id)
        {
            if (_context.UserProfiles == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userProfile);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserProfileExists(long id)
        {
            return (_context.UserProfiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static UserProfileDTO? UserProfileToDTO(UserProfile userProfile) 
        {
            var profileDTO = new UserProfileDTO 
            {
                Id = userProfile.Id,
                Firstname = userProfile.Firstname,
                Lastname = userProfile.Lastname,
                Email = userProfile.Email,
                Phonenumber = userProfile.Phonenumber,
                Gender = userProfile.Gender,
                City = userProfile.City,
                Date_of_birth = userProfile.Date_of_birth,
                AvatarUrl = userProfile.AvatarUrl
            };


            return profileDTO;
        } 

    }
}
