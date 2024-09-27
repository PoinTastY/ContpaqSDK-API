using Domain.Entities.Estructuras;
using Domain.SDK_Comercial;

namespace Domain.Interfaces
{
    public interface ISDKRepo
    {
        /// <summary>
        /// Sets the recieved Document register to printed status
        /// </summary>
        /// <param name="document"></param>
        /// <returns>Bool true confirming all ok</returns>
        /// <exception cref="SDKException">if something is wrong with sdk</exception>
        Task SetImpreso(int idDocumento, bool impressed);

        /// <summary>
        /// TRies to prepare the SDK for it to be ready to use
        /// </summary>
        void Initialize();

        /// <summary>
        /// Calls fTerminaSDK(); to release that thing
        /// </summary>
        /// <returns>confirmation</returns>
        Task DisposeSDK();

        /// <summary>
        /// Force the fCierraEmpresa(); SDK method
        /// </summary>
        void ReleaseSDK();

        /// <summary>
        /// Ask for the Binaries dir, from the specified sdk settings
        /// </summary>
        /// <returns>String with the binaries path</returns>
        string GetBinariesDir();

        /// <summary>
        /// Ask for the SDKSettings instance
        /// </summary>
        /// <returns>SDKSettings instance</returns>
        SDKSettings GetSDKSettings();

        /// <summary>
        /// Adds a document and a movement thru the Contpaqi SDK, it uses the AddDocument and AddMovimiento methods, but in one
        /// </summary>
        /// <param name="document"></param>
        /// <param name="movimiento"></param>
        /// <returns>id of the created document</returns>
        Task<Dictionary<int, Double>> AddDocumentWithMovement(tDocumento document, tMovimiento movimiento);

        /// <summary>
        /// Needs a dictionary with the field and the value, and the id of the target document to update those columns
        /// </summary>
        /// <param name="camposValores"></param>
        /// <param name="idDocumento"></param>
        /// <returns>Task</returns>
        Task SetDatoDocumento(Dictionary<string, string> camposValores, int idDocumento);

        /// <summary>
        /// Adds a document with the Contpaqi SDK.
        /// </summary>
        /// <param name="documento"></param>
        /// <returns>Returns the id of the created document(key) and the value is the folium</returns>
        Task<Dictionary<int, Double>> AddDocument(tDocumento documento);

        /// <summary>
        /// Adds a movement with the Contpaqi SDK. targeting the document from the provided id
        /// </summary>
        /// <param name="movimiento"></param>
        /// <param name="idDocumento"></param>
        /// <returns>Id of the created movement (diferent from folio)</returns>
        Task<int> AddMovimiento(tMovimiento movimiento, int idDocumento);

        /// <summary>
        /// Searches the document by the provided id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        Task<tDocumento> GetDocumentoById(int idDocumento);

        /// <summary>
        /// Searches the document by the provided data
        /// </summary>
        /// <param name="codConcepto"></param>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <returns></returns>
        Task<Dictionary<int, tDocumento>> GetDocumentoByConceptoFolioAndSerie(string codConcepto, string serie, string folio);

        /// <summary>
        /// Is required to do anything, to open the empresa in contpaqi
        /// </summary>
        /// <returns></returns>
        Task<bool> StartTransaction();

        /// <summary>
        /// Ends the transaction to release resources.
        /// </summary>
        void StopTransaction();
    }
}
