using DigitalSignageSevice.Hubs;
using DigitalSignageSevice.Models;
using DigitalSignageSevice.Repositories;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace DigitalSignageSevice.SubscribeTableDependencies
{
    public class SubscribeInforGateTableDependency
    {
        SqlTableDependency<AHT_GateInformation> tableDependency;
        DashboardHub dashboardHub;
        DigitalSignarlRepository digitalSignarlRepository;
        public SubscribeInforGateTableDependency(DashboardHub dashboardHub, IConfiguration configuration)
        {
            this.dashboardHub = dashboardHub;
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            digitalSignarlRepository = new DigitalSignarlRepository(connectionString);
        }
        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<AHT_GateInformation>(connectionString);
            tableDependency.OnChanged += TableDependency_Onchanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }
        private async void TableDependency_Onchanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<AHT_GateInformation> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var time = Convert.ToDateTime(e.Entity.Mcdt);
                DateTime currentTime = DateTime.Now;
                Console.WriteLine(time + "---" + currentTime + "----" + currentTime.AddMinutes(30));
                if ((currentTime.AddMinutes(-20)<time) && (currentTime.AddMinutes(150) > time))
                {
                    Console.WriteLine(e.Entity.Gate);
                    var data = digitalSignarlRepository.GetConnectionId("AHTBG" + e.Entity.Gate);
                    if (data != null)
                    {
                        for (int i = 0; i < data.Count(); i++)
                        {
                            if (data[i].ConnectionId != "")
                            {
                                await dashboardHub.SendReloadToClient(data[i].ConnectionId!, "Reload");
                            }
                        }
                    }
                }
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
