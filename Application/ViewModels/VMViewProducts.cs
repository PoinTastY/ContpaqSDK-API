using Application.DTOs;
using Application.ViewModels.Base;
using Core.Domain.Interfaces.Services.ApiServices.Movimientos;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Services.ApiServices.Productos;

namespace Application.ViewModels
{
    public class VMViewProducts : ViewModelBase
    {

        private ProductoDTO _producto;
        private MovimientoDTO _movimiento;

        private readonly IMovimientoService _movimientoService;

        public VMViewProducts() { }
        public VMViewProducts(IMovimientoService movimientoService) 
        { 
            _movimientoService = movimientoService;
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

        public MovimientoDTO Movimiento
        {
            get => _movimiento;
            set
            {
                _movimiento = value;
                OnPropertyChanged(nameof(Movimiento));
            }
        }

        public async Task<string> UpdateUnits(double cantidad)
        {
            var response = await _movimientoService.UpdateUnidadesMovimiento(_movimiento.CIDMOVIMIENTO, cantidad);
            Movimiento.CUNIDADES = cantidad;
            OnPropertyChanged(nameof(Movimiento));
            return response;
        }
    }
}
