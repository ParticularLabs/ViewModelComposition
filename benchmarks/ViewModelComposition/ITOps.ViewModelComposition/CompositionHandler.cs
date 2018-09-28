namespace ITOps.ViewModelComposition
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CompositionHandler
    {
        static Dictionary<string, IInterceptRoutes[]> cache = new Dictionary<string, IInterceptRoutes[]>();

        public static async Task<(dynamic ViewModel, int StatusCode)> HandleRequest(HttpContext context)
        {
            var pending = new List<Task>();
            var routeData = context.GetRouteData();
            var request = context.Request;
            var vm = new DynamicViewModel(routeData, request.Query);

            if (!cache.TryGetValue(request.Path, out IInterceptRoutes[] matching))
            {
                matching = context.RequestServices.GetServices<IInterceptRoutes>()
                    .Where(a => a.Matches(routeData, request.Method, request))
                    .ToArray();

                cache.Add(request.Path, matching);
            }

            try
            {
                foreach (var subscriber in matching.OfType<ISubscribeToViewModelCompositionEvent>())
                    subscriber.RegisterCallback(vm);

                foreach (var handler in matching.OfType<IHandleRequests>())
                    pending.Add
                    (
                        handler.Handle(vm, routeData, request)
                    );

                if (pending.Count == 0)
                {
                    return (null, StatusCodes.Status404NotFound);
                }
                else
                {
                    await Task.WhenAll(pending);

                    return (vm, StatusCodes.Status200OK);
                }
            }
            finally
            {
                vm.ClearCallbackRegistrations();
            }
        }
    }
}