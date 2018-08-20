namespace Particular.ViewModelComposition
{
    public interface IPublishCompositionEvents
    {
        void Subscribe<TEvent>(EventHandler<TEvent> handler);
    }
}
