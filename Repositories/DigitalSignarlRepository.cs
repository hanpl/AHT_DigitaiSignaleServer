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

        //Http Get data By Gate Name (get connetionID by gate name call from Subscribe)
        #region Get data By Gate Name
        public List<AHT_DigitalSignage> GetConnectionId(string gate)
        {
            List<AHT_DigitalSignage> aHT_DigitalSignages = new List<AHT_DigitalSignage>();
            AHT_DigitalSignage aHT_DigitalSignage;
            var data = getConnectionId(gate);
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
        public DataTable getConnectionId(string gate)
        {
            string query = "SELECT * FROM [MSMQFLIGHT].[dbo].[AHT_DigitalSignage] where Name = '"+gate+"' order by Location ASC";
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

        #region Update Data Gate
        public bool UpdateDisConnectionIdGate(string ip)
        {
            string query = "UPDATE [MSMQFLIGHT].[dbo].[AHT_DigitalSignage] SET Status =@Status, ConnectionId =@ConnectionId WHERE Ip = @Ip";
            DataTable dataTable = new DataTable();
            //string connectionString = "Data Source=172.17.2.38;Initial Catalog=MSMQFLIGHT;Persist Security Info=True;User ID=sa;Password=AHT@2019";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", "No");
                        command.Parameters.AddWithValue("@ConnectionId", "");
                        command.Parameters.AddWithValue("@Ip", ip);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally { connection.Close(); }
            }
        }
        public bool UpdateWorkToNo(string ip)
        {
            string query = "UPDATE [MSMQFLIGHT].[dbo].[AHT_DigitalSignage] SET Work = 'No' WHERE Ip = @Ip";
            DataTable dataTable = new DataTable();
            //string connectionString = "Data Source=172.17.2.38;Initial Catalog=MSMQFLIGHT;Persist Security Info=True;User ID=sa;Password=AHT@2019";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Ip", ip);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally { connection.Close(); }
            }
        }

        public bool UpdateOnConnectionIdGate(string ip, string connect)
        {
            string query = "UPDATE [MSMQFLIGHT].[dbo].[AHT_DigitalSignage] SET Status =@Status, ConnectionId =@ConnectionId WHERE Ip = @Ip";
            DataTable dataTable = new DataTable();
            //string connectionString = "Data Source=172.17.2.38;Initial Catalog=MSMQFLIGHT;Persist Security Info=True;User ID=sa;Password=AHT@2019";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", "Yes");
                        command.Parameters.AddWithValue("@ConnectionId", connect);
                        command.Parameters.AddWithValue("@Ip", ip);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally { connection.Close(); }
            }
        }
        #endregion
    }
}
