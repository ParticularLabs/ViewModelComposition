namespace EShop.UI.Baseline.Controllers
{
    using System.Dynamic;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/products")]
    public class ProductsController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            dynamic vm = new ExpandoObject();
            vm.ProductName = "iPhoneX";
            vm.ProductDescription = "Coolest phone ever";
            vm.ProductId = id;
            vm.ImageUrl = "https://www.t-mobile.com/content/dam/t-mobile/en-p/cell-phones/apple/apple-iphone-x/silver/Apple-iPhoneX-Silver-1-3x.jpg";
            vm.Price = 1095;
            vm.InStock = false;

            return Ok(vm);
        }     
    }
}