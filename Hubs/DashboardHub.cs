using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;
using DigitalSignageSevice.Models;
using DigitalSignageSevice.Repositories;

namespace DigitalSignageSevice.Hubs
{
    public class DashboardHub : Hub
    {
        DigitalSignarlRepository digitalSignarlRepository;
        public DashboardHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            digitalSignarlRepository = new DigitalSignarlRepository(connectionString);
        }
        public async Task SendToClientChanged(string connectId, string name)
        {
            //var workOrders = digitalSignarlRepository.GetWorkOrder();
            //await Clients.All.SendAsync("ReceivedClientChanged", connectId, name);
        }


    }
}
