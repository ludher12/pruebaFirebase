namespace pruebaFirebase.Helpers
{
    public interface IFilesHelper
    {
        Task<string> SubirArchivo(Stream archivo, string nombre, string ruta);
        Task<bool> EliminarArchivo(string nombre, string ruta);
        Task<Stream> DescargarArchivo(string nombre, string rutaCompleta);
        Task<string> DescargarCarpetaComoZip(string rutaCompleta);
    }
}
