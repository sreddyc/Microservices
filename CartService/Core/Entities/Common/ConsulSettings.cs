namespace Core.Entities.Common
{
   public class ConsulSettings : IConsulSettings
    {
        public string ServiceName { get; set; }
        public string ServiceHost { get; set; }
        public int ServicePort { get; set; }
        public string ConsulAddresss { get; set; }
    }

    public interface IConsulSettings
    {
        public string ServiceName { get; set; }
        public string ServiceHost { get; set; }
        public int ServicePort { get; set; }
        public string ConsulAddresss { get; set; }
    }
}