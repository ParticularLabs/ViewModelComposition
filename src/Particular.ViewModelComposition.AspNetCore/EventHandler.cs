namespace Particular.ViewModelComposition.AspNetCore
{
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Routing;

	public delegate Task EventHandler<TEvent>(dynamic pageViewModel, TEvent @event, RouteData routeData, IQueryCollection query);
}
