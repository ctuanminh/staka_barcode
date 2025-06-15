using Be.Core.Gateway;
using Be.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Be.Services.Gateway
{
    public class WebhookProcessorImp
    {
        private readonly IRepository _repository;
        public WebhookProcessorImp(IRepository repository)
        {
            _repository = repository;
        }

        public async Task ProcessQueuedAsync(CancellationToken cancellationToken)
        {
            var items = await _repository.GetQueryable<WorkQueue>()
                .Where(x => x.Status == "Queued" && x.RetryCount <= 5).ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                try
                {
                    item.Status = "Processed";
                    item.UpdatedAt = DateTime.UtcNow;
                    // xử lý payload ở đây
                    // hangfire.
                    await _repository.UpdateAsync(item);
                }
                catch (Exception ex)
                {
                    item.Status = "Failed";
                    item.RetryCount += 1;
                    item.LastError = ex.Message;
                    item.UpdatedAt = DateTime.UtcNow;
                }
            }
            await _repository.SaveChangeAsync();
        }
    }
}
