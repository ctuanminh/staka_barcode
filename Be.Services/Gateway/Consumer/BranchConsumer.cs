using Be.Common.Request.Gateway;
using MassTransit;

namespace Be.Services.Gateway.Consumer
{
    public class BranchConsumer : IConsumer<KiotVietUpdateBranch>
    {
        public async Task Consume(ConsumeContext<KiotVietUpdateBranch> context)
        {
            //Console.WriteLine(context.Message);
            await Task.Delay(10000);
            Task.CompletedTask.Wait();
        }
    }
}
