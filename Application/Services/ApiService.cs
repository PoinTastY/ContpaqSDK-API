using Application.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace Application.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// We need the url of the API to make the requests
        /// </summary>
        /// <param name="url"></param>
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get a document by its concepto, serie and folio
        /// </summary>
        /// <param name="concepto"></param>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <returns>DocumentDTO, parsed from the data from the apiresponse</returns>
        /// <exception cref="Exception"></exception>
        public async Task<DocumentDTO> GetDocumentoByConceptoSerieAndFolioSDKAsync(string codConcepto, string serie, string folio)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/getDocumentByConceptoFolioAndSerieSDK/{codConcepto}/{serie}/{folio}");
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
                throw new Exception($"Error al obtener el documento: {ex.Message} (Inner: {ex.InnerException})");
            }
        }

        /// <summary>
        /// Gets the movements of a document by its id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<MovimientoDTO>> GetMovimientosByIdDocumentoSQLAsync(int idDocumento)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/getMovimientosByIdDocumentoSQL/{idDocumento}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (apiResponse.Success)
                    {
                        try
                        {
                            var movimientos = JsonConvert.DeserializeObject<List<MovimientoDTO>>(apiResponse.Data.ToString());
                            return movimientos;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error al parsear los movimientos, string recibido({apiResponse.Data.ToString()}): " + ex.Message);
                        }
                    }
                    else
                    {
                        throw new Exception("Parece que no tuvimos una respuesta Exitosa para obtener movimientos: " + apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception($"Parece que tuvimos un status code: {response.StatusCode}: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los movimientos: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the products by their ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ProductoDTO>> GetProductosByIdsSQLAsync(List<int> ids)
        {
            try
            {
                var response = await _httpClient.PostAsync("/getProductosByIdsSQL", new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (apiResponse.Success)
                    {
                        var productos = JsonConvert.DeserializeObject<List<ProductoDTO>>(apiResponse.Data.ToString());
                        return productos;
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
                throw new Exception("Error al obtener los productos: " + ex.Message);
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
                var response = await _httpClient.GetAsync($"/getDocumentByIdSDK/{idDocumento}");
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
        /// Ask for te products by id filterig CPE requirements
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ProductoDTO>> GetProductosByIdListCPESQLAsync(List<int> ids)
        {
            try
            {
                var response = await _httpClient.PostAsync("/getProductosByIdsCPESQL/", new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode) 
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (apiResponse.Success)
                    {
                        var productos = JsonConvert.DeserializeObject<List<ProductoDTO>>(apiResponse.Data.ToString());
                        return productos;
                    }
                    else
                    {
                        throw new Exception("Parece que no tuvimos una respuesta Exitosa en la obtencion de productos :c: " + apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception("Parece que response dice que no tuvimos success status code al pedir productos:" + response.ReasonPhrase);
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }
        }
    }
}
