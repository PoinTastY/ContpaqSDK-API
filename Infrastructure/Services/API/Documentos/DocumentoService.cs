using Core.Application.DTOs;
using Core.Domain.Interfaces.Services.ApiServices.Documentos;
using Newtonsoft.Json;

namespace Infrastructure.Services.API.Documentos
{
    public class DocumentoService : IDocumentoService
    {
        private readonly HttpClient _client;

        public DocumentoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<DocumentDto> GetByConceptoSerieAndFolioSDKAsync<DocumentDto>(string codConcepto, string serie, string folio)
        {
            try
            {
                var response = await _client.GetAsync($"/getDocumentByConceptoFolioAndSerieSDK/{codConcepto}/{serie}/{folio}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (apiResponse.Success)
                    {
                        var document = JsonConvert.DeserializeObject<DocumentDto>(apiResponse.Data.ToString());
                        return document;
                    }
                    else
                    {
                        throw new Exception(apiResponse.Message + apiResponse.ErrorDetails);
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

        public async Task<DocumentDTO> GetDocumentByIdSDKAsync<DocumentDTO>(int idDocumento)
        {
            try
            {
                var response = await _client.GetAsync($"/getDocumentByIdSDK/{idDocumento}");
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

        public async Task<List<DocumentDTO>> GetPedidosByFechaSerieCPESQL<DocumentDTO>(DateTime fechaInicio, DateTime fechaFin, string serie)
        {
            try
            {
                // Formatear las fechas en formato ISO 8601 para que el API las pueda interpretar
                string fechaInicioFormatted = fechaInicio.ToString("yyyy-MM-ddTHH:mm:ss");
                string fechaFinFormatted = fechaFin.ToString("yyyy-MM-ddTHH:mm:ss");

                string url = $"/getPedidosByFechaSerieCPESQL/{Uri.EscapeDataString(fechaInicioFormatted)}/{Uri.EscapeDataString(fechaFinFormatted)}/{Uri.EscapeDataString(serie)}";
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
                    if (apiResponse.Success)
                    {
                        var documents = JsonConvert.DeserializeObject<List<DocumentDTO>>(apiResponse.Data.ToString());
                        return documents;
                    }
                    else
                    {
                        throw new Exception("Parece que no tuvimos una respuesta Exitosa para la lista de documentos :c: " + apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception($"Status Code: {response.StatusCode}, " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de documentos: " + ex.Message);
            }
        }
    }
}
