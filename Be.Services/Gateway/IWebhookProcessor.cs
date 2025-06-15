namespace Be.Services.Gateway
{
    public interface IWebhookProcessor
    {
        Task ProcessQueuedWebhooksAsync(CancellationToken cancellationToken = default);
    }
}
