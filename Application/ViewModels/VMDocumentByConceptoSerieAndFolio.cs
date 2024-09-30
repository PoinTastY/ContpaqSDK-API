using Application.DTOs;
using Application.Services;
using Application.ViewModels.Base;

namespace Application.ViewModels
{
    public class VMDocumentByConceptoSerieAndFolio : ViewModelBase
    {
        private ApiService _apiService;
        public VMDocumentByConceptoSerieAndFolio(ApiService apiService)
        {
            _apiService = apiService;
        }

        public VMDocumentByConceptoSerieAndFolio() { }

        private DocumentDTO? _document;

        public DocumentDTO? Document
        {
            get => _document;
            set
            {
                _document = value;
                OnPropertyChanged(nameof(Document));
            }
        }

        public async Task<bool> GetDocument(string concepto, string serie, string folio)
        {
            try
            {
                var document = await _apiService.GetDocumentoByConceptoSerieAndFolioSDKAsync(concepto, serie, folio);
                _document = document;
                OnPropertyChanged(nameof(Document));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el documento: " + ex.Message);
            }
        }
    }
}
