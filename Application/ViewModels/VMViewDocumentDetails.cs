using Core.Application.DTOs;
using Core.Application.ViewModels.Base;
using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Services.ApiServices.Movimientos;
using Core.Domain.Interfaces.Services.ApiServices.Productos;
using System.Collections.ObjectModel;

namespace Core.Application.ViewModels
{
    public class VMViewDocumentDetails : ViewModelBase
    {
        private IProductoService _productoService;
        private IMovimientoService _movimientoService;
        private ProductoDto? _documentDto;
        private ObservableCollection<ProductoDto>? _productos;
        private List<MovimientoDto> _movimientos;

        public VMViewDocumentDetails() { }
        public VMViewDocumentDetails(IProductoService productoService, IMovimientoService movimientoService)
        {
            _productoService = productoService;
            _movimientoService = movimientoService;
        }

        public ProductoDto? Documento
        {
            get => _documentDto;
            set
            {
                _documentDto = value;
                OnPropertyChanged(nameof(Documento));
            }
        }


        public ObservableCollection<ProductoDto>? Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged(nameof(Productos));
            }
        }

        public List<MovimientoDto> Movimientos
        {
            get => _movimientos;
            set
            {
                _movimientos = value;
                OnPropertyChanged(nameof(Movimientos));
            }
        }

        public void Initialize(ProductoDto document)
        {
            _documentDto = document;
            OnPropertyChanged(nameof(Documento));
        }

        public async Task LoadProductos()
        {
            if (Documento != null)
            {
                _movimientos = await _movimientoService.GetMovimientosByIdDocumentoSQLAsync<MovimientoDto>(_documentDto.CIDPRODUCTO);
                var resultados = await _productoService.GetProductosByIdListCPESQLAsync<ProductoDto>(_movimientos.Select(m => m.Id).ToList());
                _productos = new ObservableCollection<ProductoDto>(resultados);
                OnCollectionChanged(nameof(Productos));
                OnPropertyChanged(nameof(Documento));
            }
            else
            {
                throw new Exception("Documento is null");
            }
        }
    }
}
