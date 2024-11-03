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
        public IActionResult Delete(int? id) {
            if (id == null) {
                return NotFound();
            }
            var pelicula = _context.Peliculas.FirstOrDefault(m => m.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            var pelicula = await _context.Peliculas.FindAsync(id);
            
            if (pelicula != null) {
                var urlImagen = pelicula.UrlImagen;
                // Extrae solo el nombre del archivo (por ejemplo, "lucario.png") usando una expresión regular
                // Extrae solo el nombre del archivo usando expresión regular y quita prefijos como "Fotos_Perfil%2F"
                var regex = new System.Text.RegularExpressions.Regex(@"([^/]+)$");
                var match = regex.Match(urlImagen);
                var nombreArchivo = match.Success ? match.Value.Split('?')[0] : null;

                // Decodifica el nombre del archivo y elimina cualquier prefijo
                if (nombreArchivo != null && nombreArchivo.Contains("%2F"))
                {
                    nombreArchivo = Uri.UnescapeDataString(nombreArchivo.Split(new[] { "%2F" }, StringSplitOptions.None).Last());
                }
                if (!string.IsNullOrEmpty(nombreArchivo))
                {
                    // Llama a EliminarArchivo con solo el nombre del archivo limpio
                    await _filesHelper.EliminarArchivo(nombreArchivo);
                }
                _context.Peliculas.Remove(pelicula);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
