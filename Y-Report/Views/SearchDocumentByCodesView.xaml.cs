using Application.DTOs;
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

    public SearchDocumentByCodesView() : this(MauiProgram.ServiceProvider.GetRequiredService<VMDocumentByConceptoSerieAndFolio>())
    {
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var concepto = ConceptEntry.Text;
        var serie = SerieEntry.Text;
        var folio = FolioEntry.Text;
        if (string.IsNullOrEmpty(concepto) || string.IsNullOrEmpty(serie) || string.IsNullOrEmpty(folio))
            await DisplayAlert("Error", "Por favor llena todos los campos", "Ok");

        else
        {
            try
            {
                await _viewModel.GetDocuments(DateTime.Now.AddDays(-10), DateTime.Now, serie);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
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
			try
			{
                await _viewModel.GetDocuments(DateTime.Now.AddDays(-10), DateTime.Now, serie);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var cv = (CollectionView)sender;
        if (cv.SelectedItem == null)
            return;

        int? idDocument = (e.CurrentSelection.FirstOrDefault() as DocumentDTO)?.CIDDOCUMENTO;
        if (idDocument != null)
		{
			var document = _viewModel.Documents.FirstOrDefault(d => d.CIDDOCUMENTO == idDocument);
			if (document != null)
			{
                await Shell.Current.Navigation.PushAsync(new ViewDocumentDetails(document));
            }
        }

        cv.SelectedItem = null;
    }
}