using Core.Domain.Entities.DTOs;

namespace Core.Domain.Interfaces.Repositories.DTOs
{
    public interface IDocumentoDtoRepo
    {
        /// <summary>
        /// Agrega un documento
        /// </summary>
        /// <param name="pedido"></param>
        /// <param name="movimientos"></param>
        /// <returns>El objeto resultante (con el id generado a la hora de insercion)</returns>
        Task<DocumentoDto> AddAsync(DocumentoDto documento);

        /// <summary>
        /// Obtiene los documentos no impresos, y sin IdContpaqiSQL
        /// </summary>
        Task<IEnumerable<DocumentoDto>> GetPendientesAsync();

        /// <summary>
        /// Obtiene un documento por su id
        /// </summary>
        /// <param name="id"></param>
        Task<DocumentoDto> GetByIdAsync(int id);

        /// <summary>
        /// Actualiza el documento en la base de datos, se usa para marcarlo como surtido y dar id de contpaqi
        /// </summary>
        /// <param name="documento"></param>
        Task UpdateAsync(DocumentoDto documento);

        /// <summary>
        /// Elimina un documento por su id
        /// </summary>
        Task DeleteByIdAsync(int id);
    }
}
