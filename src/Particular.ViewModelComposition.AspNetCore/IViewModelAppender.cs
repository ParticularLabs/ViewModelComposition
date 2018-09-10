namespace Particular.ViewModelComposition.AspNetCore
{
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Routing;

	public interface IViewModelAppender : IRouteInterceptor
    {
        Task Append(dynamic viewModel, RouteData routeData, IQueryCollection query);
    }
}
