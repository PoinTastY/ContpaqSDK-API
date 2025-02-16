using Core.Application.DTOs;
using Core.Application.ViewModels.Base;
using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Services.ApiServices.Movimientos;

namespace Core.Application.ViewModels
{
    public class VMViewProducts : ViewModelBase
    {

        private ProductoDto _producto;
        private MovimientoDTO _movimiento;

        private readonly IMovimientoService _movimientoService;

        public VMViewProducts() { }
        public VMViewProducts(IMovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }

        public ProductoDto Producto
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
