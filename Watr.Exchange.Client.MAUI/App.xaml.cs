using ReactiveUI;
using System.Reactive.Linq;
using Watr.Exchange.Security.Core;
using Watr.Exchange.ViewModels;

namespace Watr.Exchange.Client.MAUI
{
    public interface IViewForResolver
    {
        Page? ResolveView(IRoutableViewModel viewModel);
    }

    public class ViewForResolver : IViewForResolver
    {
        readonly IServiceProvider _serviceProvider;

        public ViewForResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Page? ResolveView(IRoutableViewModel viewModel)
        {
            return viewModel switch
            {
                MainViewModel => _serviceProvider.GetService<MainPage>(),
                _ => null
            };
        }
    }

    public partial class App : Application
    {
        protected NavigationPage NavPage { get; }
        public App(IScreen screen, IViewForResolver resolver)
        {
            InitializeComponent();
            NavPage = new NavigationPage();

            screen.Router
                .Navigate
                .Select(resolver.ResolveView)
                .Where(page => page is not null)
                .Subscribe(page => NavPage.PushAsync(page!));
              
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(NavPage);
        }
    }
}