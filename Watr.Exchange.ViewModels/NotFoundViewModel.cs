using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watr.Exchange.ViewModels
{
    public class NotFoundViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => null;

        public IScreen HostScreen { get; }
        public NotFoundViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
