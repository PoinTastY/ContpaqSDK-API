using Application.DTOs;
using Newtonsoft.Json;

namespace Application.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        /// <summary>
        /// We need the url of the API to make the requests
        /// </summary>
        /// <param name="url"></param>
        public ApiService(string url)
        {
            _httpClient = new HttpClient();
            _url = url;
        }

        /// <summary>
        /// Get a document by its concepto, serie and folio
        /// </summary>
        /// <param name="concepto"></param>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <returns>DocumentDTO, parsed from the data from the apiresponse</returns>
        /// <exception cref="Exception"></exception>
        public async Task<DocumentDTO> GetDocumentoByConceptoSerieAndFolioSDKAsync(string concepto, string serie, string folio)
        {
            try
            {
                var response = await _httpClient.GetAsync(_url + $"/getDocumentByConceptoFolioAndSerieSDK/document/{concepto}/{serie}/{folio}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (apiResponse.Success)
                    {
                        var document = JsonConvert.DeserializeObject<DocumentDTO>(apiResponse.Data.ToString());
                        return document;
                    }
                    else
                    {
                        throw new Exception("Parece que no tuvimos una respuesta Exitosa :c: " + apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception("Parece que no tuvimos una respuesta Exitosa :c: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el documento: " + ex.Message);
            }
        }

        /// <summary>
        /// Get a document by its id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>DocumentDTO parsed form the ApiResponse Data atrbute</returns>
        /// <exception cref="Exception"></exception>
        public async Task<DocumentDTO> GetDocumentByIdSDKAsync(int idDocumento)
        {
            try
            {
                var response = await _httpClient.GetAsync(_url + $"/getDocumentByIdSDK/{idDocumento}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (apiResponse.Success)
                    {
                        var document = JsonConvert.DeserializeObject<DocumentDTO>(apiResponse.Data.ToString());
                        return document;
                    }
                    else
                    {
                        throw new Exception("Parece que no tuvimos una respuesta Exitosa :c: " + apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception("Parece que no tuvimos una respuesta Exitosa :c: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el documento: " + ex.Message);
            }
        }
    }
}
