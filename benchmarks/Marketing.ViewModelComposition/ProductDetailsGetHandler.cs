namespace Marketing.ViewModelComposition
{
    using System.Threading.Tasks;
    using ITOps.ViewModelComposition;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ProductDetailsGetHandler : IHandleRequests
    {
        public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
        {
            var controller = (string) routeData.Values["controller"];

            return HttpMethods.IsGet(httpVerb)
                   && controller.ToLowerInvariant() == "products"
                   && routeData.Values.ContainsKey("id");
        }

        public Task Handle(dynamic vm, RouteData routeData, HttpRequest request)
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