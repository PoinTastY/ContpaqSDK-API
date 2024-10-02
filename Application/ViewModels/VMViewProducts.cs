using Application.DTOs;
using Application.ViewModels.Base;
using Domain.Interfaces.Services.ApiServices.Productos;

namespace Application.ViewModels
{
    public class VMViewProducts : ViewModelBase
    {
        private readonly IProductoService _productoService;

        private ProductoDTO _producto;

        public VMViewProducts() { }
        public VMViewProducts(IProductoService productoService) 
        { 
            _productoService = productoService;
        }

        public ProductoDTO Producto
        {
            get => _producto;
            set
            {
                _producto = value;
                OnPropertyChanged(nameof(Producto));
            }
        }

        public async Task UpdateProductStatus()
        {

        }
    }
}
