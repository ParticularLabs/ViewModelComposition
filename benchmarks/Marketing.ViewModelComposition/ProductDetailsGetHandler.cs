namespace Marketing.ViewModelComposition
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Particular.ViewModelComposition.AspNetCore;

    public class ProductDetailsGetHandler : IViewModelAppender
    {
        public bool Matches(RouteData routeData, string httpVerb)
        {
            var controller = (string) routeData.Values["controller"];

            return HttpMethods.IsGet(httpVerb)
                   && controller.ToLowerInvariant() == "products"
                   && routeData.Values.ContainsKey("id");
        }

        public Task Append(dynamic vm, RouteData routeData, IQueryCollection query)
        {
            vm.ProductName = "iPhoneX";
            vm.ProductDescription = "Coolest phone ever";
            vm.ProductId = routeData.Values["id"].ToString();
            vm.ImageUrl = "https://www.t-mobile.com/content/dam/t-mobile/en-p/cell-phones/apple/apple-iphone-x/silver/Apple-iPhoneX-Silver-1-3x.jpg";
            vm.Price = 1095;
            vm.InStock = false;

            return Task.CompletedTask;
        }
    }
}