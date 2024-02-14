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
    }
}
