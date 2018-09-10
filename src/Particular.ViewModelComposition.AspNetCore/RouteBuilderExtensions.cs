namespace Particular.ViewModelComposition.AspNetCore
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.AspNetCore.Routing.Constraints;
	using Microsoft.Extensions.DependencyInjection;

	public static class RouteBuilderExtensions
    {
        public static IRouteBuilder MapComposableGet(
            this IRouteBuilder routeBuilder,
            string template,
            RouteValueDictionary defaults = null,
            RouteValueDictionary dataTokens = null)
        {
            var route = new Route(
                target: new RouteHandler(context => ComposableRouteHandler.HandleGetRequest(context)),
                routeTemplate: template,
                defaults: defaults,
                constraints: new RouteValueDictionary(new
                {
                    httpMethod = new HttpMethodRouteConstraint(HttpMethods.Get)
                }),
                dataTokens: dataTokens,
                inlineConstraintResolver: routeBuilder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>());

            routeBuilder.Routes.Add(route);

            return routeBuilder;
        }
    }
}
