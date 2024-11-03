namespace pruebaFirebase.Helpers
{
    public interface IFilesHelper
    {
        Task<string> SubirArchivo(Stream archivo, string nombre);
        Task<bool> EliminarArchivo(string nombre);
    }
}
