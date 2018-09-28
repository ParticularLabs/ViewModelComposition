namespace Particular.ViewModelComposition.AspNetCore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

    public class CompositionHandler
    {
        static Dictionary<string, IRouteInterceptor[]> cache = new Dictionary<string, IRouteInterceptor[]>();

        public static async Task<(dynamic ViewModel, int StatusCode)> HandleGetRequest(HttpContext context)
        {
            var routeData = context.GetRouteData();
            var request = context.Request;
            var viewModel = new DynamicViewModel(routeData, request.Query);

            if (!cache.TryGetValue(request.Path, out var interceptors))
            {
                interceptors = context.RequestServices.GetServices<IRouteInterceptor>()
                    .Where(a => a.Matches(routeData, request.Method))
                    .ToArray();

                cache.Add(request.Path, interceptors);
            }

            try
            {
                foreach (var subscriber in interceptors.OfType<ISubscribeToCompositionEvents>())
                {
                    subscriber.Subscribe(viewModel);
                }

                var pendingTasks = new List<Task>();

                foreach (var appender in interceptors.OfType<IViewModelAppender>())
                {
                    pendingTasks.Add(appender.Append(viewModel, routeData, context.Request.Query));
                }

                if (!pendingTasks.Any())
                {
                    return (null, StatusCodes.Status404NotFound);
                }
                else
                {
                    await Task.WhenAll(pendingTasks);

                    return (viewModel, StatusCodes.Status200OK);
                }
            }
            finally
            {
                viewModel.ClearSubscriptions();
            }
        }
    }
}
