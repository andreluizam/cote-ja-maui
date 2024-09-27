using CoteAqui.ViewModel;

namespace CoteAqui.Views;

public partial class PaginaPrincipal : ContentPage
{
	public PaginaPrincipal()
	{
		InitializeComponent();
        BindingContext = new CotacaoViewModel();
    }
}