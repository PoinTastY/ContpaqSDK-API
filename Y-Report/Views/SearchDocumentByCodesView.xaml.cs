using Application.ViewModels;

namespace Y_Report.Views;

public partial class SearchDocumentByCodesView : ContentPage
{
	private VMDocumentByConceptoSerieAndFolio _viewModel;
	public SearchDocumentByCodesView(VMDocumentByConceptoSerieAndFolio viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    private async void BtnBuscarDocumento_Clicked(object sender, EventArgs e)
    {
		var concepto = ConceptEntry.Text;
		var serie = SerieEntry.Text;
		var folio = FolioEntry.Text;
		if (string.IsNullOrEmpty(concepto) || string.IsNullOrEmpty(serie) || string.IsNullOrEmpty(folio))
			await DisplayAlert("Error", "Por favor llena todos los campos", "Ok");

		else
		{
			await _viewModel.GetDocument(concepto, serie, folio);
			SearchSection.IsVisible = false;
            ResultSection.IsVisible = true;
        }
    }
}