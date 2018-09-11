namespace Particular.ViewModelComposition.AspNetCore
{
    public interface IPublishCompositionEvents
    {
        void Subscribe<TEvent>(EventHandler<TEvent> handler);
    }
}
