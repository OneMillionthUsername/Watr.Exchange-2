using Microsoft.AspNetCore.Components;
using ReactiveUI;
using System.Windows.Input;
using System.Reactive.Linq;

namespace Watr.Exchange.Web.Helpers
{
    public static class Helpers
    {

        public static Func<string, CancellationToken, Task<IEnumerable<T>>> BindCommand<T>(this ReactiveCommand<string?, IEnumerable<T>> command)
        {
            return async (str, token) => await command.Execute(str).GetAwaiter();
        }
        public static EventCallback<T> BindCommand<T>(this ICommand command, object? parameter = null)
        {
            MulticastDelegate m1 = () => command.Execute(parameter);
            return new EventCallback<T>(null, m1);
        }
    }
}
