namespace Particular.ViewModelComposition
{
    using Microsoft.AspNetCore.Routing;

    public interface IRouteInterceptor
    {
        bool Matches(RouteData routeData, string httpMethod);
    }
}
