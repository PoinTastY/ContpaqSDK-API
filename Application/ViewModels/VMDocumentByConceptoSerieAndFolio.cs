using Core.Application.ViewModels.Base;
using Core.Domain.Interfaces.Services.ApiServices.Documentos;
using Core.Domain.Entities.DTOs;
using System.Collections.ObjectModel;

namespace Core.Application.ViewModels
{
    public class VMDocumentByConceptoSerieAndFolio : ViewModelBase
    {
        private IDocumentoService _documentoService;
        public VMDocumentByConceptoSerieAndFolio(IDocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        public VMDocumentByConceptoSerieAndFolio() { }

        private ObservableCollection<DocumentoDto>? _documents;

        public ObservableCollection<DocumentoDto>? Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }

        public async Task GetDocuments(DateTime fechaInicio, DateTime fechaFin, string serie)
        {
            try
            {
                var documents = await _documentoService.GetPedidosByFechaSerieCPESQL<DocumentoDto>(fechaInicio, fechaFin, serie);
                _documents = new ObservableCollection<DocumentoDto>(documents);
                OnPropertyChanged(nameof(Documents));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el documento: " + ex.Message);
            }
        }
    }
}
