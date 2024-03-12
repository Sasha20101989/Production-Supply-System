using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateTransportParameters
    {
        public CreateTransportParameters(Transport entity)
        {
            TransportName = entity.TransportName;
        }

        public string TransportName { get; set; }
    }
}
