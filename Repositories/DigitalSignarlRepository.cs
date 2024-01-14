using DigitalSignageSevice.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DigitalSignageSevice.Repositories
{
    public class DigitalSignarlRepository
    {
        string connectionString;
        public DigitalSignarlRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        //Http Get All
        #region Get All # Done 
        public List<AHT_DigitalSignage> GetDataDigital()
        {
            List<AHT_DigitalSignage> aHT_DigitalSignages = new List<AHT_DigitalSignage>();
            AHT_DigitalSignage aHT_DigitalSignage;
            var data = GetWorkOrderDetailsFromDb();
            foreach (DataRow row in data.Rows)
            {
                aHT_DigitalSignage = new AHT_DigitalSignage
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Ip = row["Ip"].ToString(),
                    Location = row["Location"].ToString(),
                    Live = row["Live"].ToString(),
                    Remark = row["Remark"].ToString(),
                    Status = row["Status"].ToString(),
                    LeftRight = row["LeftRight"].ToString(),
                    GateChange = row["GateChange"].ToString(),
                    Mode = row["Mode"].ToString(),
                    Auto = row["Auto"].ToString(),
                    Iata = row["Iata"].ToString(),
                    NameLineCode = row["NameLineCode"].ToString(),
                    TimeMcdt = row["TimeMcdt"].ToString(),
                    ConnectionId = row["ConnectionId"].ToString(),
                    LiveAuto = row["LiveAuto"].ToString(),
                };
                aHT_DigitalSignages.Add(aHT_DigitalSignage);
            }
            return aHT_DigitalSignages;
        }
        public DataTable GetWorkOrderDetailsFromDb()
        {
            string query = "SELECT * FROM [MSMQFLIGHT].[dbo].[AHT_DigitalSignage] order by Location ASC";
            DataTable dataTable = new DataTable();
            //string connectionString = "Data Source=172.17.2.38;Initial Catalog=MSMQFLIGHT;Persist Security Info=True;User ID=sa;Password=AHT@2019";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally { connection.Close(); }
            }
        }
        #endregion
    }
}
