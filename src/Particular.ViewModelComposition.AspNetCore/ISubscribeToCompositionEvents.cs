namespace Particular.ViewModelComposition.AspNetCore
{
    public interface ISubscribeToCompositionEvents : IRouteInterceptor
    {
        void Subscribe(IPublishCompositionEvents publisher);
    }
}
