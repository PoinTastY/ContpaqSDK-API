using Application.DTOs;
using Application.ViewModels.Base;
using Core.Domain.Interfaces.Services.ApiServices.Movimientos;
using Core.Domain.Interfaces.Services.ApiServices.Productos;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMViewDocumentDetails : ViewModelBase
    {
        private IProductoService _productoService;
        private IMovimientoService _movimientoService;
        private DocumentDTO? _documentDTO;
        private ObservableCollection<ProductoDTO>? _productos;
        private List<MovimientoDTO> _movimientos;

        public VMViewDocumentDetails(){}
        public VMViewDocumentDetails(IProductoService productoService, IMovimientoService movimientoService)
        {
            _productoService = productoService;
            _movimientoService = movimientoService;
        }

        public DocumentDTO? Documento
        {
            get => _documentDTO;
            set
            {
                _documentDTO = value;
                OnPropertyChanged(nameof(Documento));
            }
        }

        public ObservableCollection<ProductoDTO>? Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged(nameof(Productos));
            }
        }

        public List<MovimientoDTO> Movimientos
        {
            get => _movimientos;
            set
            {
                _movimientos = value;
                OnPropertyChanged(nameof(Movimientos));
            }
        }

        public void Initialize(DocumentDTO document)
        {
            _documentDTO = document;
            OnPropertyChanged(nameof(Documento));
        }

        public async Task LoadProductos()
        {
            if (Documento != null)
            {
                _movimientos = await _movimientoService.GetMovimientosByIdDocumentoSQLAsync<MovimientoDTO>(_documentDTO.CIDDOCUMENTO);
                var resultados = await _productoService.GetProductosByIdListCPESQLAsync<ProductoDTO>(_movimientos.Select(m => m.CIDPRODUCTO).ToList());
                _productos = new ObservableCollection<ProductoDTO>(resultados);
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
