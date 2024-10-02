using Application.DTOs;
using Application.ViewModels;

namespace Y_Report.Views;

public partial class ViewDocumentDetails : ContentPage
{
	private VMViewDocumentDetails _viewModel;
    private DocumentDTO _documentDTO;

    public ViewDocumentDetails(VMViewDocumentDetails vMViewDocumentDetails)
	{
		InitializeComponent();
        _viewModel = vMViewDocumentDetails;
        BindingContext = _viewModel;
    }

    public ViewDocumentDetails(DocumentDTO document) : this(MauiProgram.ServiceProvider.GetRequiredService<VMViewDocumentDetails>())
    {
        _viewModel.Initialize(document);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductos();
        
    }

    public void Initialize(DocumentDTO document)
    {
        _viewModel.Initialize(document);
    }

    private void BtnAgregarProducto_Clicked(object sender, EventArgs e)
    {
        var document = _viewModel.Documento;
        var productos = _viewModel.Productos;
        return;
    }
}