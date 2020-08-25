using Domain.Entities;

namespace MessagePublish.Sender
{
    public interface IProductUpdateSender
    {
        void Publish(Product product);
    }
}