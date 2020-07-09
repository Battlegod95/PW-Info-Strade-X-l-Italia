using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace PWInfoStradeFunctions
{
//    public static class CosmosTriggerToSQL
//    {
//        public static bool BulkInsertDataTable(string tableName, DataTable dataTable)
//        {
//            bool isSuccuss;
//            try
//            {

//                /*SqlConnection sqlserverconnection = new SqlConnection(
//                    new SqlConnectionStringBuilder()
//                    {
//                        DataSource = "pwdatabaseinfostrade",
//                        InitialCatalog = "PW_InfoStradeXItalia_Database",
//                        UserID = "emiliano",
//                        Password = "Donbosco95"
//                    }.ConnectionString
//                );*/

//                SqlConnection sqlserverconnection = new SqlConnection();
//                sqlserverconnection.ConnectionString = "Data Source=pwdatabaseinfostrade.database.windows.net,1433;"
//                                                        + "Initial Catalog=PW_InfoStradeXItalia_Database;"
//                                                        + "User ID=emiliano;"
//                                                        + "Password=Donbosco95;";
//                sqlserverconnection.Open();
//                SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlserverconnection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
//                bulkCopy.DestinationTableName = tableName;
//                bulkCopy.WriteToServer(dataTable);
//                sqlserverconnection.Close();
//                isSuccuss = true;
//                Console.WriteLine(isSuccuss.ToString());
//            }
//            catch (Exception ex)
//            {
//                isSuccuss = false;
//            }
//            return isSuccuss;
//        }

//        [FunctionName("CosmosTriggerToSQL")]
//        public static void Run([CosmosDBTrigger(
//            databaseName: "PWInfoStrade-Telemetric",
//            collectionName: "SensorData",
//            ConnectionStringSetting = "ConnectionStringCosmos",
//            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
//        {
//            if (input != null && input.Count > 0)
//            {
//                DataTable dt = new DataTable();

//                dt.Columns.Add("Description");
//                dt.Columns.Add("Id_gateway");
//                dt.Columns.Add("Id_incrocio");
//                dt.Columns.Add("Crossroad_id");
//                dt.Columns.Add("Intersection");
//                dt.Columns.Add("Location");
//                dt.Columns.Add("Healthy");
//                dt.Columns.Add("Date");
//                dt.Columns.Add("Time");
//                dt.Columns.Add("Temperature");
//                dt.Columns.Add("Humidity");
//                dt.Columns.Add("Pressure");
//                dt.Columns.Add("Coordinate_x");
//                dt.Columns.Add("Coordinate_y");

//                DataRow row = dt.NewRow();

//                foreach (Document item in input)
//                {
//                    row = dt.NewRow();

//                    row["Description"] = item.GetPropertyValue<string>("Description");
//                    row["Id_gateway"] = item.GetPropertyValue<string>("idGateway");
//                    row["Id_incrocio"] = item.GetPropertyValue<int>("idIncrocio");
//                    row["Crossroad_id"] = item.GetPropertyValue<string>("CrossRoad");
//                    row["Intersection"] = item.GetPropertyValue<string>("Interserction");
//                    row["Location"] = item.GetPropertyValue<string>("Location");
//                    row["Healthy"] = item.GetPropertyValue<int>("Healthy");
//                    row["Date"] = item.GetPropertyValue<SqlDateTime>("Date");
//                    row["Time"] = item.GetPropertyValue<DateTime>("Time");
//                    row["Temperature"] = item.GetPropertyValue<float>("Temperature");
//                    row["Humidity"] = item.GetPropertyValue<float>("Humidity");
//                    row["Pressure"] = item.GetPropertyValue<float>("Pressure");
//                    row["Coordinate_x"] = item.GetPropertyValue<int>("Coordinate_x");
//                    row["Coordinate_y"] = item.GetPropertyValue<int>("Coordinate_y");

//                    dt.Rows.Add(row);
//                    log.LogInformation(row["Description"].ToString());
//                    log.LogInformation(row["Id_gateway"].ToString());
//                    log.LogInformation(row["Id_incrocio"].ToString());
//                    log.LogInformation(row["Crossroad_id"].ToString());
//                    log.LogInformation(row["Intersection"].ToString());
//                    log.LogInformation(row["Location"].ToString());
//                    log.LogInformation(row["Healthy"].ToString());
//                    log.LogInformation(row["Date"].ToString());
//                    log.LogInformation(row["Time"].ToString());
//                    log.LogInformation(row["Temperature"].ToString());
//                    log.LogInformation(row["Humidity"].ToString());
//                    log.LogInformation(row["Pressure"].ToString());
//                    log.LogInformation(row["Coordinate_x"].ToString());
//                    log.LogInformation(row["Coordinate_y"].ToString());

//                    BulkInsertDataTable("dbo.SensorTable", dt);
//                }
//            }
//        }
//    }
}
