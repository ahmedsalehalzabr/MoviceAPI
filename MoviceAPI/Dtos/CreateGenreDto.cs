using System.ComponentModel.DataAnnotations;

namespace MoviceAPI.Dtos
{
    public class CreateGenreDto
    {

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
