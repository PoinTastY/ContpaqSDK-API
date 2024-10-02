using Application.DTOs;
using Domain.Interfaces.Services.ApiServices.Movimientos;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services.API.Movimientos
{
    public class MovimientoService : IMovimientoService
    {
        private readonly HttpClient _client;
        public MovimientoService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        
        public async Task<List<MovimientoDTO>> GetMovimientosByIdDocumentoSQLAsync<MovimientoDTO>(int idDocumento)
        {
            try
            {
                var response = await _client.GetAsync($"/getMovimientosByIdDocumentoSQL/{idDocumento}");
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

        public async Task<string> UpdateUnidadesMovimiento(int idMovimiento, double unidades)
        {
            try
            {
                var response = await _client.PatchAsync($"patchUnidadesMovimientoByIdSQL/{idMovimiento}/{unidades}", new StringContent(JsonConvert.SerializeObject(unidades), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (!apiResponse.Success)
                    {
                        throw new Exception("Parece que no tuvimos una respuesta Exitosa para actualizar las unidades del movimiento: " + apiResponse.Message);
                    }
                    return apiResponse.Message;
                }
                else
                {
                    throw new Exception($"Parece que tuvimos un status code: {response.StatusCode}: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar las unidades del movimiento: " + ex.Message);
            }
        }
    }
}
