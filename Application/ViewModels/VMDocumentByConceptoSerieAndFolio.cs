using Application.DTOs;
using Application.ViewModels.Base;
using Core.Domain.Interfaces.Services.ApiServices.Documentos;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMDocumentByConceptoSerieAndFolio : ViewModelBase
    {
        private IDocumentoService _documentoService;
        public VMDocumentByConceptoSerieAndFolio(IDocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        public VMDocumentByConceptoSerieAndFolio() { }

        private ObservableCollection<DocumentDTO>? _documents;

        public ObservableCollection<DocumentDTO>? Documents
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
                var documents = await _documentoService.GetPedidosByFechaSerieCPESQL<DocumentDTO>(fechaInicio, fechaFin, serie);
                _documents = new ObservableCollection<DocumentDTO>(documents);
                OnPropertyChanged(nameof(Documents));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el documento: " + ex.Message);
            }
        }
    }
}
