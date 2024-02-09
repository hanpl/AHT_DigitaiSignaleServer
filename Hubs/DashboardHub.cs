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
            var clientIpAddressip = Context.GetHttpContext()!.Connection.RemoteIpAddress;
            Console.WriteLine(connectionId + " On "+ clientIpAddressip);
            if(digitalSignarlRepository.UpdateOnConnectionIdGate(clientIpAddressip!.ToString().Trim(), connectionId))
            {
                await ReloadServerDashboard();
                Console.WriteLine("Updated to database !...");
            }    
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var clientIpAddressip = Context.GetHttpContext()!.Connection.RemoteIpAddress;
            Console.WriteLine(connectionId + " Dis " + clientIpAddressip);
            if (digitalSignarlRepository.UpdateDisConnectionIdGate(clientIpAddressip!.ToString().Trim()))
            {
                await ReloadServerDashboard();
                Console.WriteLine("Updated to database !...");
            }
            // Thực hiện xử lý khi client ngắt kết nối
            await base.OnDisconnectedAsync(exception);
        }
        public async Task ReloadServerDashboard()
        {
            var data = digitalSignarlRepository.GetConnectionId("SERVER");
            if (data != null)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    if (data[i].ConnectionId != "")
                    {
                        await Clients.Client(data[i].ConnectionId).SendAsync("ReceivedServerReload", data[i].ConnectionId, "Reload");
                    }
                }
            }
        }
        public async Task SendReloadToClient(string connectId, string name)
        {
            //var workOrders = digitalSignarlRepository.GetWorkOrder();
            await Clients.Client(connectId).SendAsync("ReceivedClientChanged",connectId, name);
        }
        public async Task UpdateAndSync(string name, string lere, string column, string value, string connectId)
        {
            Console.WriteLine("update Ok" +name+lere+ column + " "+value);
            if (digitalSignarlRepository.UpdateToDB(name, lere, column, value))
            {
                await Clients.Client(connectId).SendAsync("ReceivedClientChanged", connectId, "Reload");
                await ReloadServerDashboard();
            }
        }
        public async Task UpdateModeAutoSync(string name, string lere, string mode, string value, string connectId)
        {
            if (digitalSignarlRepository.UpdateModeToDB(name, lere, mode, value))
            {
                await Clients.Client(connectId).SendAsync("ReceivedLoadModeAuto", mode, value);
                await ReloadServerDashboard();
            }
        }

        public async Task ReloadListLineCode(string name)
        {
            var data = digitalSignarlRepository.GetConnectionId("SERVER");
            if (data != null)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    if (data[i].ConnectionId != "")
                    {
                        await Clients.Client(data[i].ConnectionId!).SendAsync("ReceivedServerReloadListLineCode", data[i].ConnectionId, name);
                    }
                }
            }
        }


    }
}
