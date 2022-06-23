namespace Background.Settings
{
    public abstract class BaseConsumerSettings
    {
        public int MessagesPerCycle { get; set; }
        public int MaxTimeWithoutProcessing { get; set; }
        public string Cron { get; set; }
    }
}