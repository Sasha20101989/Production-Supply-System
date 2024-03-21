using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateTransportParameters(Transport entity)
    {
        public string TransportName { get; set; } = entity.TransportName;
    }
}
