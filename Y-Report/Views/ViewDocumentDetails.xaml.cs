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

    public ViewDocumentDetails() : this(MauiProgram.ServiceProvider.GetRequiredService<VMViewDocumentDetails>())
    {
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductos();
        
    }

    private void BtnAgregarProducto_Clicked(object sender, EventArgs e)
    {
        var document = _viewModel.Documento;
        var productos = _viewModel.Productos;
        return;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var cv = (CollectionView)sender;
        if (cv.SelectedItem == null)
            return;

        try
        {
            var productoSeleccionado = (e.CurrentSelection.FirstOrDefault() as ProductoDTO);
            var movimiento = _viewModel.Movimientos.FirstOrDefault(m => m.CIDPRODUCTO == productoSeleccionado.CIDPRODUCTO);

            var viewProducts = new ViewProducts(movimiento, productoSeleccionado);
            await Shell.Current.Navigation.PushAsync(viewProducts);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }

        cv.SelectedItem = null;
    }
}