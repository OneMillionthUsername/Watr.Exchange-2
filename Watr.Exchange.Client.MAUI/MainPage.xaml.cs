using ReactiveUI.Maui;
using Watr.Exchange.ViewModels;

namespace Watr.Exchange.Client.MAUI
{
    public partial class MainPage : ReactiveContentPage<MainViewModel>
    {
        public MainPage(MainViewModel vm)
        {
            ViewModel = vm;
            InitializeComponent();
        }
    }
}
