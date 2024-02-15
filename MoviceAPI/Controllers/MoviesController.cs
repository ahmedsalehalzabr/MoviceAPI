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
    public class MoviesController : ControllerBase
    {

        private readonly AppDbContext _db;

        //التحكم في حجم الصورة
        private new List<string> _allowedExtenstions = new List<string> { ".ipg", ".png"};
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _db.Movies.Include(m => m.Genre).Select(m => new MoviesDetelesDto
            {
                Id = m.Id,
                GenreId = m.GenreId,
                GenreName = m.Genre.Name,
                Poster = m.Poster,
                Rate = m.Rate,
                Storeline = m.Storeline,
                Title = m.Title,
                Year = m.Year,

            }).ToListAsync();

            return Ok(movies);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = _db.Movies.FindAsync(id); 

            if(movie == null)
                return NotFound();

      
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAny([FromForm] MovieDto dto)   
        {
            //التحكم في حجم الصورة
            if (dto.Poster == null)
                return BadRequest("Poster is required!");

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = await _db.Genres.AnyAsync(x => x.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!"); 

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Poster = dataStream.ToArray(),
                Title = dto.Title,
                Rate = dto.Rate,
                Year = dto.Year,
                Storeline = dto.Storeline,
            };
            await _db.AddAsync(movie);
            _db.SaveChangesAsync();
            return Ok(movie);
        }
    }
}
