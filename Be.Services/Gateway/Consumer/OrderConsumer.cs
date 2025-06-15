using Be.Common.Gateway.Misa;
using Be.Common.Request.Gateway;
using MassTransit;

namespace Be.Services.Gateway.Consumer
{
    public class OrderConsumer : IConsumer<KiotVietUpdateOrderRequest>
    {
        public async Task Consume(ConsumeContext<KiotVietUpdateOrderRequest> context)
        {
            //Console.WriteLine(context.Message);
            await Task.Delay(10000);
            Task.CompletedTask.Wait();
        }
    }
}
