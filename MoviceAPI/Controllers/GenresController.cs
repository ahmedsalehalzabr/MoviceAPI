using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviceAPI.Dtos;
using MoviceAPI.Models;
using MoviceAPI.Models.Data;

namespace MoviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext _db;
        public GenresController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var gener = await _db.Genres.ToListAsync();
            return Ok(gener);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };

            await _db.AddAsync(genre);

            _db.SaveChanges();
            return Ok(genre);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , [FromBody] CreateGenreDto dto)
        {
            var genre = await _db.Genres.SingleOrDefaultAsync(x => x.Id == id);

            if (genre == null) 
                return NotFound($"No genre was found with ID : {id}");

            genre.Name = dto.Name;
            _db.SaveChanges();

            return Ok(genre);
        }

        [HttpDelete("{id}")]
           public async Task<IActionResult> Delete(int id)
        {
            var genre = await _db.Genres.SingleOrDefaultAsync(x => x.Id == id);

            if (genre == null)
                return NotFound($"No genre was found with ID : {id}");

            _db.Remove(genre);
            _db.SaveChanges();

            return Ok(genre);
        }
    }
}
