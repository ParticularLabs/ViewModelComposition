namespace Particular.ViewModelComposition.AspNetCore.Mvc
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.DependencyInjection;

	public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddViewModelCompositionMvcSupport(this IMvcBuilder builder)
        {
            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(CompositionFilter));
            });

            return builder;
        }
    }
}
