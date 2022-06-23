using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhiteRabbit_WebApi.DataAccess;
using WhiteRabbit_WebApi.Models;
using WhiteRabbit_WebApi.Infrastructure;
using WhiteRabbit_WebApi.DTO;

namespace WhiteRabbit_WebApi.Controllers
{
    [Route("api/AnimalProfiles")]
    [ApiController]
    public class AnimalProfilesController : Controller
    {
        private readonly PostgreSqlContext _context;
        public AnimalProfilesController(PostgreSqlContext context)
        {
            _context = context;
        }

        // GET: api/AnimalProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalProfileDTO>>> GetAnimalProfiles()
        {
            if (_context.AnimalProfiles == null)
            {
                return NotFound();
            }

            return await _context.AnimalProfiles
                                 .Select(x => AnimalProfileToDTO(x))
                                 .ToListAsync();
        }

        // GET: api/AnimalProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalProfileDTO>> GetAnimalProfile(long id)
        {
            if (_context.AnimalProfiles == null)
            {
                return NotFound();
            }

            var animal = await _context.AnimalProfiles.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return AnimalProfileToDTO(animal);
        }


        // PUT: api/AnimalProfiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimalProfile(long id, AnimalProfileDTO animalDTO)
        {
            var animal = await _context.AnimalProfiles.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            animal.Name = animalDTO.Name;
            animal.Age = animalDTO.Age;
            animal.AvatarUrl = animalDTO.AvatarUrl;
            animal.Is_liked = animalDTO.Is_liked;
            animal.Is_booked = animalDTO.Is_booked;
            animal.About = animalDTO.About;
            animal.Address = animalDTO.Address;

            //if(animalDTO.Photo != null) 
            //{
            //    animal.Photo = ImageConvertion.ConvertToByteArray(animalDTO.Photo);
            //}    

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AnimalProfileExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/AnimalProfiles
        [HttpPost]
        public async Task<ActionResult<AnimalProfileDTO>> CreateAnimalProfile(AnimalProfileDTO animalDTO) 
        {
            if (_context.AnimalProfiles == null)
            {
                return Problem("Entity set 'DbContext.AnimalProfiles' is null.");
            }

            var animal = new AnimalProfile
            {
                Name = animalDTO.Name,
                Age = animalDTO.Age,
                AvatarUrl = animalDTO.AvatarUrl,
                Is_liked = animalDTO.Is_liked,
                Is_booked = animalDTO.Is_booked,
                About = animalDTO.About,
                Address = animalDTO.Address
            };

            //if (animalDTO.Photo != null)
            //{
            //    animal.Photo = ImageConvertion.ConvertToByteArray(animalDTO.Photo);
            //}

            _context.AnimalProfiles.Add(animal);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimalProfile), new { id = animal.Id }, AnimalProfileToDTO(animal));
        }

        // DELETE: api/AnimalProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimalProfile(long id)
        {
            if (_context.AnimalProfiles == null)
            {
                return NotFound();
            }

            var animal = await _context.AnimalProfiles.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            _context.AnimalProfiles.Remove(animal);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalProfileExists(long id)
        {
            return (_context.AnimalProfiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static AnimalProfileDTO? AnimalProfileToDTO(AnimalProfile animal) 
        {
            var animalDTO = new AnimalProfileDTO 
            {
                Id = animal.Id,
                Name = animal.Name,
                Age = animal.Age,
                AvatarUrl = animal.AvatarUrl,
                Is_liked = animal.Is_liked,
                Is_booked = animal.Is_booked,
                About = animal.About,
                Address = animal.Address
            };

            //if (animal.Photo != null)
            //{
            //    var stream = new MemoryStream(animal.Photo);
            //    IFormFile photo = new FormFile(stream, 0, animal.Photo.Length, animal.Name, animal.Name)
            //    {
            //        Headers = new HeaderDictionary()
            //    };

            //    animalDTO.Photo = photo;

            //    return animalDTO;
            //}

            return animalDTO;
        } 

    }
}
