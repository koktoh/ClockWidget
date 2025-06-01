namespace ClockWidget.Models.Initialization
{
    public class InitializableEntry
    {
        public IAsyncInitializable Instance { get; }
        public int Priority { get; }
        public bool ShouldInitialize { get; } = true;

        public InitializableEntry(IAsyncInitializable instance, int priority, bool shouldInitialize)
        {
            this.Instance = instance;
            this.Priority = priority;
            this.ShouldInitialize = shouldInitialize;
        }
    }
}
