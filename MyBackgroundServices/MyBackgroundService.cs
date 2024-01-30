using DigitalSignageSevice.Models;
using Microsoft.Data.SqlClient;
using NuGet.Packaging.Signing;
using System;
using System.Data;

namespace DigitalSignageSevice.MyBackgroundServices
{
    public class MyBackgroundService : BackgroundService
    {
        string connectionString;
        public MyBackgroundService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection"); 
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Thực hiện công việc cần chạy định kỳ tại đây
                Console.WriteLine("Running scheduled task..." + connectionString);
                //Convert(Datetime, "Jan 14 2024  4:30PM") between DATEADD(Mi, -20,getdate()) and DATEADD(Mi, 150,getdate()) 
                
                //for (int i = 1; i < 11; i++)
                //{
                //    var Mode = "";
                //    var Live = "";
                //    var data = GetWorkOrderDetailsFromDb(Convert.ToString(i));
                //    //Console.WriteLine($"{i} {data}");
                //    //Console.WriteLine(data.Rows.Count);
                //    if (data.Rows.Count != 0)
                //    {
                //        foreach (DataRow row in data.Rows)
                //        {

                //            Mode = (CheckModeEgate(row["LineCode"].ToString(), Convert.ToString(i)) == true ? "Yes" : "No");
                //            Live = row["LineCode"].ToString() + "_" +(Mode =="Yes"?"EGATE":"NOEGATE")+"_"+ row["Remark"].ToString()+"_"+ row["LeftRight"].ToString();
                //            Console.WriteLine("Gate Doing..." + row["Name"].ToString() + " - " + row["LineCode"].ToString() +" - "+ row["Remark"].ToString() +" - "+ Mode+"  "+Live);
                //            if ((row["Live"].ToString()==Live)&& (row["Mode"].ToString() == Mode)&& (row["Mcdt"].ToString() == row["TimeMcdt"].ToString()))
                //            {
                //                Console.WriteLine("Gate not yet change...");
                //                //Bổ sung code gửi signalr đến  client tương ứng
                //            }
                //            else
                //            {
                //                if (UpdateGateToDoing(Convert.ToInt32(row["Id"]), Live, row["Remark"].ToString(), Mode, row["LineCode"].ToString(), row["FlightNo"].ToString(), row["Mcdt"].ToString()))
                //                {
                //                    Console.WriteLine("UpdateGateToDoing");
                //                    //Bổ sung code gửi signalr đến  client tương ứng
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        Console.WriteLine("Gate AHTBG" + Convert.ToString(i)+" Free...");
                //        if(UpdateGateToFree("AHTBG" + Convert.ToString(i)))
                //        {
                //            Console.WriteLine("UpdateGateToFree");
                //            //Bổ sung code gửi signalr đến  client tương ứng
                //        }
                //    }

                //}

                // Đợi một khoảng thời gian trước khi chạy công việc tiếp theo
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        public bool UpdateGateToDoing (int id, string live, string remark, string mode, string iata, string namelinecode, string timemcdt)
        {
            string query = "UPDATE [MSMQFLIGHT].[dbo].[AHT_DigitalSignage] SET Live =@Live, Remark =@Remark, " +
                "Mode = @Mode, Iata =@Iata, NameLineCode = @NameLineCode, TimeMcdt =@TimeMcdt WHERE Id = @Id";
            DataTable dataTable = new DataTable();
            //string connectionString = "Data Source=172.17.2.38;Initial Catalog=MSMQFLIGHT;Persist Security Info=True;User ID=sa;Password=AHT@2019";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Live", live);
                        command.Parameters.AddWithValue("@Remark", remark);
                        command.Parameters.AddWithValue("@Mode", mode);
                        command.Parameters.AddWithValue("@Iata", iata);
                        command.Parameters.AddWithValue("@NameLineCode", namelinecode);
                        command.Parameters.AddWithValue("@TimeMcdt", timemcdt);
                        command.Parameters.AddWithValue("@Id", id);
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
        public bool CheckModeEgate(string linecode, string gate)
        {
            var data = GetModeEgate(linecode, gate);
            if (data.Rows.Count != 0)
            {
                    if (data.Rows[0]["IsEgate"].ToString() == "Yes")
                    {
                        return true;
                    }
                    else 
                    { 
                        return false; 
                    }
            }
            else
            {
                return false;
            }
        }
        public DataTable GetModeEgate(string line, string gate)
        {
            string query = "SELECT IsEgate FROM [MSMQFLIGHT].[dbo].[AHT_EGateForFlight] WHERE Name = '" + line + "' AND GateNumber = '" + gate + "'";
            DataTable dataTable = new DataTable();
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
        public bool UpdateGateToFree(string gate)
        {
            string query = "UPDATE [MSMQFLIGHT].[dbo].[AHT_DigitalSignage] SET Live =@Live, Remark =@Remark, " +
                "Mode = @Mode, Iata =@Iata, NameLineCode = @NameLineCode, TimeMcdt =@TimeMcdt WHERE Name = @Name";
            DataTable dataTable = new DataTable();
            //string connectionString = "Data Source=172.17.2.38;Initial Catalog=MSMQFLIGHT;Persist Security Info=True;User ID=sa;Password=AHT@2019";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Live", "AHT");
                        command.Parameters.AddWithValue("@Remark", "");
                        command.Parameters.AddWithValue("@Mode", "No");
                        command.Parameters.AddWithValue("@Iata", "");
                        command.Parameters.AddWithValue("@NameLineCode", "");
                        command.Parameters.AddWithValue("@TimeMcdt", "");
                        command.Parameters.AddWithValue("@Name", gate);
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
        //public List<DigitalSignage> CheckDataGate(string gate)
        //{
        //    List<DigitalSignage> digitalSignages = new List<DigitalSignage>();
        //    DigitalSignage digitalSignage;
        //    var data = GetWorkOrderDetailsFromDb(gate);
        //    foreach (DataRow row in data.Rows)
        //    {
        //        digitalSignage = new DigitalSignage
        //        {
        //            Id = Convert.ToInt32(row["Id"]),
        //            Name = row["Name"].ToString(),
        //            Ip = row["Ip"].ToString(),
        //            Location = row["Location"].ToString(),
        //            Live = row["Live"].ToString(),
        //            Remark = row["Remark"].ToString(),
        //            Status = row["Status"].ToString(),
        //            LeftRight = row["LeftRight"].ToString(),
        //            GateChange = row["GateChange"].ToString(),
        //            Mode = row["Mode"].ToString(),
        //            Auto = row["Auto"].ToString(),
        //            Iata = row["Iata"].ToString(),
        //            NameLineCode = row["NameLineCode"].ToString(),
        //            TimeMcdt = row["TimeMcdt"].ToString(),
        //            ConnectionId = row["ConnectionId"].ToString(),
        //            LiveAuto = row["LiveAuto"].ToString(),
        //        };
        //        digitalSignages.Add(digitalSignage);
        //    }
        //    return digitalSignages;
        //}
        public DataTable GetWorkOrderDetailsFromDb(string gate)
        {
            string query = "select TOP (2) A.LineCode, CONCAT (A.LineCode,A.Number) as FlightNo,A.Remark, A.Mcdt, B.* "+
                "from AHT_FlightInformation AS A JOIN AHT_DigitalSignage AS B ON CONCAT ('AHTBG',A.Gate) = B.Name "+
                "where CONVERT(Datetime ,A.Mcdt) between DATEADD(Mi, -20,getdate()) and DATEADD(Mi, 150,getdate()) "+
                "AND A.Status<>'' and A.Status<> 'Cancelled'  and A.Adi = 'D' AND A.Gate = '"+gate+"' AND A.Remark != 'Gate closed' "+
                "AND A.Remark != 'Departed' Order by CONVERT(Datetime ,A.Mcdt) ASC";
            DataTable dataTable = new DataTable();
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
    }
}
