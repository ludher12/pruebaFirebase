using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pruebaFirebase.Helpers;
using pruebaFirebase.Models;

namespace pruebaFirebase.Controllers
{
    public class PeliculaController : Controller
    {
        private readonly Models.ApplicationDbContext _context;
        private readonly IFilesHelper _filesHelper;

        public PeliculaController(Models.ApplicationDbContext context, IFilesHelper filesHelper)
        {
            _context = context;
            _filesHelper = filesHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Peliculas.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {

                Stream image = Imagen.OpenReadStream();
                //llamamos a nuestra interfaz para subir el archivo
                string urlimagen = await _filesHelper.SubirArchivo(image, Imagen.FileName);

                try
                {

                    //agregamos la url que nos devolvio el metodo SubirArchivo, a nuestro objeto pelicula.
                    pelicula.UrlImagen = urlimagen;
                    //agregamos el objeto en base de datos	
                    _context.Add(pelicula);
                    //guardamos los cambios	
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ViewBag.Error(exception.Message);
                }
            }
            return View(pelicula);
        }
    }
}
