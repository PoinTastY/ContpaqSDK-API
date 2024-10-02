using Application.DTOs;
using Application.ViewModels;

namespace Y_Report.Views;

public partial class ViewProducts : ContentPage
{
	private VMViewProducts _viewModel;

	public ViewProducts(VMViewProducts vMViewProducts)
	{
		InitializeComponent();
		_viewModel = vMViewProducts;
		BindingContext = _viewModel;
	}

	public ViewProducts(MovimientoDTO movimiento, ProductoDTO producto) : this(MauiProgram.ServiceProvider.GetRequiredService<VMViewProducts>())
    {
		_viewModel.Movimiento = movimiento;
        _viewModel.Producto = producto;
    }

	public ViewProducts() : this(MauiProgram.ServiceProvider.GetRequiredService<VMViewProducts>())
    {
    }

    private async void Button_Clicked(object sender, EventArgs e)
	{
		var cantidadAnterior = _viewModel.Movimiento.CUNIDADES;
		try
		{
			var cantidad = double.Parse(CantidadEntry.Text);
			var choice = await DisplayAlert("Sumar o Reemplazar", "Quieres sumar al valor de unidades actual? o reemplazarlo", "Sumar", "Reemplazar");
			if (choice)
			{
                await _viewModel.UpdateUnits(cantidad + _viewModel.Movimiento.CUNIDADES);
                await DisplayAlert("Exito", $"Unidades sumadas (Anterior: {cantidadAnterior}, Nuevo: {_viewModel.Movimiento.CUNIDADES})", "Ok");

            }
            else
			{
                await _viewModel.UpdateUnits(cantidad);
                await DisplayAlert("Exito", $"Unidades Actualizadas (Anterior: {cantidadAnterior}, Nuevo: {_viewModel.Movimiento.CUNIDADES})", "Ok");
            }
        }
		catch (Exception ex)
        {
			_viewModel.Movimiento.CUNIDADES = cantidadAnterior;
            await DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}