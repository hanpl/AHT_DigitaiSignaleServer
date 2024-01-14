using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;
using DigitalSignageSevice.Models;
using DigitalSignageSevice.Repositories;
using System.Threading.Tasks;

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

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var clientIpAddressip = Context.GetHttpContext().Connection.RemoteIpAddress;
            Console.WriteLine(connectionId + "  "+ clientIpAddressip);
            // Xử lý địa chỉ IP của client kết nối đến
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var clientIpAddressip = Context.GetHttpContext().Connection.RemoteIpAddress;
            Console.WriteLine(connectionId + "  " + clientIpAddressip);
            // Thực hiện xử lý khi client ngắt kết nối
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendToClientChanged(string connectId, string name)
        {
            //var workOrders = digitalSignarlRepository.GetWorkOrder();
            //await Clients.All.SendAsync("ReceivedClientChanged", connectId, name);
        }


    }
}
