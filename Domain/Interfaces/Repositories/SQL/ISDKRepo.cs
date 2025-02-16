using Core.Domain.Entities.DTOs;
using Core.Domain.Entities.SDK.Estructuras;
using Core.Domain.Entities.SQL;
using Core.Domain.Exceptions;
using Domain.SDK_Comercial;

namespace Core.Domain.Interfaces.Repositories.SQL
{
    public interface ISDKRepo
    {
        /// <summary>
        /// Establece el estado de impreso de un documento por su id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="impressed"
        /// <exception cref="SDKException"></exception>
        Task SetImpreso(int idDocumento, bool impressed);

        /// <summary>
        /// Prepara el SDK para ser utilizado, correr en el mismo hilo que el que lo llama
        /// </summary>
        /// <exception cref="SDKException"></exception>
        /// <exception cref="AccessViolationException"></exception>
        void InicializarSDK();

        /// <summary>
        /// Llama a fTerminaSDK(); para liberar recursos del SDK, ni sirve la chingadera
        /// </summary>
        Task TerminaSDK();

        /// <summary>
        /// Llama el fCierraEmpresa();
        /// </summary>
        void CierraEmpresaSDK();

        /// <summary>
        /// Agrega un documento con el SDK de contpaqi
        /// </summary>
        /// <param name="documento"></param>
        /// <remarks>el retorno debe ser diccionario, porque se generan dos valores al insertar</remarks>
        /// <returns>DICCIONARIO clave = id SQL, valor = folio (double)</returns>
        /// <exception cref="SDKException"></exception>
        Task<Dictionary<int, double>> AddDocumento(DocumentoDto documentoDto);

        /// <summary>
        /// Establece valores en las columnas de un documento, por su id
        /// </summary>
        /// <remarks>La clave del diccionario es la columna, y el valor, pues el valor JeJe</remarks>
        /// <param name="camposValores"></param>
        /// <param name="idDocumento"></param>
        /// <exception cref="SDKException"></exception>
        Task SetDatoDocumento(Dictionary<string, string> camposValores, int idDocumento);

        /// <summary>
        /// Agrega un movimiento con el SDK de contpaqi, a un documento existente por su id
        /// </summary>
        /// <remarks>
        /// Primero se debe crear el documento y obtener su id de SQl,
        /// para que el puntero se mantenga en el nuevo registro y permita agregar movimientos
        /// </remarks>
        /// <param name="movimiento"></param>
        /// <param name="idDocumento"></param>
        /// <returns>Id of the created movement (diferent from folio)</returns>
        Task<int> AddMovimiento(MovimientoDto movimientoDto, int idDocumento);

        /// <summary>
        /// Is required to do anything, to open the empresa in contpaqi
        /// </summary>
        /// <returns></returns>
        Task<bool> StartTransaction(string empresa);

        /// <summary>
        /// Ends the transaction to release resources.
        /// </summary>
        void StopTransaction();
    }
}
