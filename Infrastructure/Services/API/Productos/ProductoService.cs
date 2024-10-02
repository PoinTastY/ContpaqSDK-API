using Application.DTOs;
using Domain.Interfaces.Services.ApiServices.Productos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Infrastructure.Services.API.Productos
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _client;

        public ProductoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ProductoDTO>> GetProductosByIdsSQLAsync<ProductoDTO>(List<int> ids)
        {
            try
            {
                var response = await _client.PostAsync("/getProductosByIdsSQL", new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"));
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

        public async Task<List<ProductoDTO>> GetProductosByIdListCPESQLAsync<ProductoDTO>(List<int> ids)
        {
            try
            {
                var response = await _client.PostAsync("/getProductosByIdsCPESQL/", new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"));
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
