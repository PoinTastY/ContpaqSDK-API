namespace Domain.Interfaces.Services.ApiServices.Movimientos
{
    public interface IMovimientoService
    {
        /// <summary>
        /// Gets the movements of a document by its id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<List<T>> GetMovimientosByIdDocumentoSQLAsync<T>(int idDocumento);
    }
}
