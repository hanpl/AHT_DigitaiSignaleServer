using DigitalSignageSevice.Hubs;
using DigitalSignageSevice.Models;
using DigitalSignageSevice.Repositories;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace DigitalSignageSevice.SubscribeTableDependencies
{
    public class SubscribeDigitalSignageTableDependency
    {
        SqlTableDependency<AHT_DigitalSignage> tableDependency;
        DashboardHub dashboardHub;
        DigitalSignarlRepository digitalSignarlRepository;
        public SubscribeDigitalSignageTableDependency(DashboardHub dashboardHub, IConfiguration configuration)
        {
            this.dashboardHub = dashboardHub;
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            digitalSignarlRepository = new DigitalSignarlRepository(connectionString);
        }
        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<AHT_DigitalSignage>(connectionString);
            tableDependency.OnChanged += TableDependency_Onchanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private async void TableDependency_Onchanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<AHT_DigitalSignage> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                if ((e.Entity.Work == "Yes")&& (e.Entity.ConnectionId != ""))
                {
                    await dashboardHub.SendReloadToClient(e.Entity.ConnectionId!, "Reload");
                    digitalSignarlRepository.UpdateWorkToNo(e.Entity.Id.ToString());
                }
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        
    }
}
