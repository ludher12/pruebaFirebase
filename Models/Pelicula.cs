using System.ComponentModel.DataAnnotations;

namespace pruebaFirebase.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Genero { get; set; }
        public string? FechaEstreno { get; set; }
        [Display(Name = "Imagen")]
        public string? UrlImagen { get; set; }
    }
}
