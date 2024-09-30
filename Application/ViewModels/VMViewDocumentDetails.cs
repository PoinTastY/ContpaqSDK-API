using Application.DTOs;
using Application.Services;
using Application.ViewModels.Base;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMViewDocumentDetails : ViewModelBase
    {
        private ApiService _apiService;
        private DocumentDTO? _documentDTO;
        private ObservableCollection<ProductoDTO>? _productos;
        private List<MovimientoDTO> _movimientos;

        public VMViewDocumentDetails(){}
        public VMViewDocumentDetails(ApiService apiService)
        {
            _apiService = apiService;
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

        public void Initialize(DocumentDTO document)
        {
            _documentDTO = document;
            OnPropertyChanged(nameof(Documento));
        }

        public async Task LoadProductos()
        {
            if (Documento != null)
            {
                _movimientos = await _apiService.GetMovimientosByIdDocumentoSQLAsync(_documentDTO.CIDDOCUMENTO);
                var resultados = await _apiService.GetProductosByIdListCPESQLAsync(_movimientos.Select(m => m.CIDPRODUCTO).ToList());
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
