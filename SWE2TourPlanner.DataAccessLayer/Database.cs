using Newtonsoft.Json;
using Npgsql;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SWE2TourPlanner.DataAccessLayer
{
    class Database : IDataAccess
    {
        private string connectionString;
        private List<TourItem> tourItems = new List<TourItem>();
        private List<LogItem> logItems = new List<LogItem>();
        private ConfigFile configFile;
        private string key;

        public Database()
        {
            string path = "configFile.json";
            string json = File.ReadAllText(path);
            this.configFile = JsonConvert.DeserializeObject<ConfigFile>(json);
            this.key = this.configFile.FsSettings.Key;
            // get connection data e.g. from config file
            connectionString = "Host=" + this.configFile.DbSettings.Host + ";Username=" + this.configFile.DbSettings.Username + ";Password=" + 
                                this.configFile.DbSettings.Password + ";Database=" + this.configFile.DbSettings.Database + ";Port=" + this.configFile.DbSettings.Port;
        }

        public void AddItem(string name, string from, string to, string imagePath, string description, string route)
        {
            int tourId = 0;
            string url = @"http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + from + "&to=" + to + "&routeType=" + route;

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(url);
                RequestData reqObj = JsonConvert.DeserializeObject<RequestData>(json);

                TimeSpan time = TimeSpan.FromSeconds(reqObj.route.Time);
                string str = time.ToString(@"hh\:mm\:ss");
                reqObj.route.Distance = Math.Round(reqObj.route.Distance * 1.60934, 2);

                description += "\n\nDistance: " + reqObj.route.Distance + " km" + "\nTime: " + str + "\nHas Highway: " + reqObj.route.HasHighway +
                              "\nHas TollRoad: " + reqObj.route.HasTollRoad + "\nRoute Type: "+ route +"\nFrom: " + from + "\nTo: " + to;
             
                tourId = GetTourItemIdForInsert();

                InsertTour(tourId, name, description, from, to, imagePath, route);
            }
        }

        public void InsertTour(int tourId, string name, string description, string from, string to, string imagePath, string route)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strcomm = "INSERT INTO tours (tourid, name, description, startpoint, endpoint, path, route) VALUES (@tourid, @name, @description, @startpoint, @endpoint, @path, @route)";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strcomm, conn);

            sqlcomm.Parameters.AddWithValue("tourid", tourId);
            sqlcomm.Parameters.AddWithValue("name", name);
            sqlcomm.Parameters.AddWithValue("description", description);
            sqlcomm.Parameters.AddWithValue("startpoint", from);
            sqlcomm.Parameters.AddWithValue("endpoint", to);
            sqlcomm.Parameters.AddWithValue("path", imagePath);
            sqlcomm.Parameters.AddWithValue("route", route);
            
            sqlcomm.Prepare();
            sqlcomm.ExecuteNonQuery();

            conn.Close();
        }

        public int GetTourItemIdForInsert()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strmax = "Select max(tourId) from tours";
            string strcount = "Select count(*) from tours";

            NpgsqlCommand sqlmaxcmd = new NpgsqlCommand(strmax, conn);
            NpgsqlCommand sqlountcmd = new NpgsqlCommand(strcount, conn);

            Int32 count = Convert.ToInt32(sqlountcmd.ExecuteScalar());

            if(count == 0)
            {
                conn.Close();
                return 1;
            }
            else
            {
                Int32 tourId = Convert.ToInt32(sqlmaxcmd.ExecuteScalar());
                conn.Close();
                tourId++;
                return tourId;
            }
        }

        public bool DoesTourExist(int tourid)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strcount = "Select count(*) from tours where tourid = @tourid";

            NpgsqlCommand sqlcount = new NpgsqlCommand(strcount, conn);
            sqlcount.Parameters.AddWithValue("tourid", tourid);

            Int32 count = Convert.ToInt32(sqlcount.ExecuteScalar());

            if(count == 0)
            {
                conn.Close();
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool DoesLogExist(int logid)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strcount = "Select count(*) from logs where logid = @logid";

            NpgsqlCommand sqlcount = new NpgsqlCommand(strcount, conn);
            sqlcount.Parameters.AddWithValue("logid", logid);

            Int32 count = Convert.ToInt32(sqlcount.ExecuteScalar());

            if (count == 0)
            {
                conn.Close();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime, string rating, string avgSpeed, string inclination, string topSpeed, string maxHeight, string minHeight)
        {
            int logId = GetLogItemIdForInsert();
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strcomm = "INSERT INTO logs (logid, fk_tourid, datetime, report, distance, totaltime, rating, avgspeed, inclination, topspeed, maxheight, minheight) VALUES (@logid, @fk_tourid, @datetime, @report, @distance, @totaltime, @rating, @avgspeed, @inclination, @topspeed, @maxheight, @minheight)";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strcomm, conn);

            sqlcomm.Parameters.AddWithValue("logid", logId);
            sqlcomm.Parameters.AddWithValue("fk_tourid", tourid);
            sqlcomm.Parameters.AddWithValue("datetime", dateTime);
            sqlcomm.Parameters.AddWithValue("report", report);
            sqlcomm.Parameters.AddWithValue("distance", distance);
            sqlcomm.Parameters.AddWithValue("totaltime", totalTime);
            sqlcomm.Parameters.AddWithValue("rating", rating);
            sqlcomm.Parameters.AddWithValue("avgspeed", avgSpeed);
            sqlcomm.Parameters.AddWithValue("inclination", inclination);
            sqlcomm.Parameters.AddWithValue("topspeed", topSpeed);
            sqlcomm.Parameters.AddWithValue("maxheight", maxHeight);
            sqlcomm.Parameters.AddWithValue("minheight", minHeight);

            sqlcomm.Prepare();
            sqlcomm.ExecuteNonQuery();

            conn.Close();
        }

        public int GetLogItemIdForInsert()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strmax = "Select max(logId) from logs";
            string strcount = "Select count(*) from logs";

            NpgsqlCommand sqlmaxcmd = new NpgsqlCommand(strmax, conn);
            NpgsqlCommand sqlountcmd = new NpgsqlCommand(strcount, conn);

            Int32 count = Convert.ToInt32(sqlountcmd.ExecuteScalar());

            if (count == 0)
            {
                conn.Close();
                return 1;
            }
            else
            {
                Int32 logId = Convert.ToInt32(sqlmaxcmd.ExecuteScalar());
                conn.Close();
                logId++;
                return logId;
            }
        }

        public void DeleteItem(int tourid)
        {
            tourItems.RemoveAt(tourItems.FindIndex(item => item.TourId == tourid));

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string logdelete = "Delete from logs where fk_tourid = @tourid";
            NpgsqlCommand sqllogdelete = new NpgsqlCommand(logdelete, conn);
            sqllogdelete.Parameters.AddWithValue("tourid", tourid);
            sqllogdelete.Prepare();
            sqllogdelete.ExecuteNonQuery();

            string strdelete = "Delete from tours where tourid = @tourid";
            NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);
            sqldelete.Parameters.AddWithValue("tourid", tourid);
            sqldelete.Prepare();
            sqldelete.ExecuteNonQuery();

            conn.Close();
        }

        public void DeleteLog(int logid)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strdelete = "Delete from logs where logid = @logid";
            NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);

            sqldelete.Parameters.AddWithValue("logid", logid);
            sqldelete.Prepare();
            sqldelete.ExecuteNonQuery();

            conn.Close();
        }

        public List<LogItem> GetLogs(int tourid)
        {
            logItems.Clear();

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string strlogs = "Select * from logs where fk_tourid = @tourid";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strlogs, conn);
            sqlcomm.Parameters.AddWithValue("tourid", tourid);
            sqlcomm.Prepare();

            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            while (reader.Read())
            {
                logItems.Add(new LogItem()
                {
                    LogId = reader.GetInt32(0),
                    TourId = reader.GetInt32(1),
                    DateTime = reader.GetString(2),
                    Report = reader.GetString(3),
                    Distance = reader.GetString(4),
                    TotalTime = reader.GetString(5),
                    Rating = reader.GetString(6),
                    AvgSpeed = reader.GetString(7),
                    Inclination = reader.GetString(8),
                    TopSpeed = reader.GetString(9),
                    MaxHeight = reader.GetString(10),
                    MinHeight = reader.GetString(11)
                });
            }
            conn.Close();

            return logItems;
        }

        public List<TourItem> GetItems()
        {
            tourItems.Clear();

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string strtours = "Select * from tours";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strtours, conn);
            sqlcomm.Prepare();

            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            while (reader.Read())
            {
                tourItems.Add(new TourItem()
                {
                    TourId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    From = reader.GetString(3),
                    To = reader.GetString(4),
                    ImagePath = reader.GetString(5),
                    Route = reader.GetString(6)
                });
            }
            conn.Close();
            
            return tourItems;
        }

        public void ModifyTour(TourItem currentTour, string description, string route)
        {
            string url = @"http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + currentTour.From + "&to=" + currentTour.To + "&routeType=" + route;

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(url);
                RequestData reqObj = JsonConvert.DeserializeObject<RequestData>(json);

                TimeSpan time = TimeSpan.FromSeconds(reqObj.route.Time);
                string str = time.ToString(@"hh\:mm\:ss");
                reqObj.route.Distance = Math.Round(reqObj.route.Distance * 1.60934, 2);

                description += "\n\nDistance: " + reqObj.route.Distance + " km" + "\nTime: " + str + "\nHas Highway: " + reqObj.route.HasHighway +
                              "\nHas TollRoad: " + reqObj.route.HasTollRoad + "\nRoute Type: " + route + "\nFrom: " + currentTour.From + "\nTo: " + currentTour.To;
            }

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string updatetours = "Update tours set description = @description, route = @route where tourid = @tourid";
            NpgsqlCommand sqlupdate = new NpgsqlCommand(updatetours, conn);
            sqlupdate.Parameters.AddWithValue("description", description);
            sqlupdate.Parameters.AddWithValue("route", route);
            sqlupdate.Parameters.AddWithValue("tourid", currentTour.TourId);
            sqlupdate.Prepare();
            sqlupdate.ExecuteNonQuery();

            conn.Close();
        }

        public void ModifyLog(LogItem currentLog, string typeLogData, string newEntry)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string updatelogs = null;

            switch (typeLogData)
            {
                case "datetime":
                    updatelogs = "Update logs set datetime = @newEntry where logid = @logid";
                    NpgsqlCommand sqlupdate = new NpgsqlCommand(updatelogs, conn);
                    sqlupdate.Parameters.AddWithValue("newEntry", newEntry);
                    sqlupdate.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqlupdate.Prepare();
                    sqlupdate.ExecuteNonQuery();
                    break;
                case "report":
                    updatelogs = "Update logs set report = @newEntry where logid = @logid";
                    NpgsqlCommand sqluprepo = new NpgsqlCommand(updatelogs, conn);
                    sqluprepo.Parameters.AddWithValue("newEntry", newEntry);
                    sqluprepo.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqluprepo.Prepare();
                    sqluprepo.ExecuteNonQuery();
                    break;
                case "distance":
                    updatelogs = "Update logs set distance = @newEntry where logid = @logid";
                    NpgsqlCommand sqlupdist = new NpgsqlCommand(updatelogs, conn);
                    sqlupdist.Parameters.AddWithValue("newEntry", newEntry);
                    sqlupdist.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqlupdist.Prepare();
                    sqlupdist.ExecuteNonQuery();
                    break;
                case "totaltime":
                    updatelogs = "Update logs set totaltime = @newEntry where logid = @logid";
                    NpgsqlCommand sqluptota = new NpgsqlCommand(updatelogs, conn);
                    sqluptota.Parameters.AddWithValue("newEntry", newEntry);
                    sqluptota.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqluptota.Prepare();
                    sqluptota.ExecuteNonQuery();
                    break;
                case "rating":
                    updatelogs = "Update logs set rating = @newEntry where logid = @logid";
                    NpgsqlCommand sqluprati = new NpgsqlCommand(updatelogs, conn);
                    sqluprati.Parameters.AddWithValue("newEntry", newEntry);
                    sqluprati.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqluprati.Prepare();
                    sqluprati.ExecuteNonQuery();
                    break;
                case "avgspeed":
                    updatelogs = "Update logs set avgspeed = @newEntry where logid = @logid";
                    NpgsqlCommand sqlupavgs = new NpgsqlCommand(updatelogs, conn);
                    sqlupavgs.Parameters.AddWithValue("newEntry", newEntry);
                    sqlupavgs.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqlupavgs.Prepare();
                    sqlupavgs.ExecuteNonQuery();
                    break;
                case "inclination":
                    updatelogs = "Update logs set inclination = @newEntry where logid = @logid";
                    NpgsqlCommand sqlupincl = new NpgsqlCommand(updatelogs, conn);
                    sqlupincl.Parameters.AddWithValue("newEntry", newEntry);
                    sqlupincl.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqlupincl.Prepare();
                    sqlupincl.ExecuteNonQuery();
                    break;
                case "topspeed":
                    updatelogs = "Update logs set topspeed = @newEntry where logid = @logid";
                    NpgsqlCommand sqluptops = new NpgsqlCommand(updatelogs, conn);
                    sqluptops.Parameters.AddWithValue("newEntry", newEntry);
                    sqluptops.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqluptops.Prepare();
                    sqluptops.ExecuteNonQuery();
                    break;
                case "maxheight":
                    updatelogs = "Update logs set maxheight = @newEntry where logid = @logid";
                    NpgsqlCommand sqlupmaxh = new NpgsqlCommand(updatelogs, conn);
                    sqlupmaxh.Parameters.AddWithValue("newEntry", newEntry);
                    sqlupmaxh.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqlupmaxh.Prepare();
                    sqlupmaxh.ExecuteNonQuery();
                    break;
                case "minheight":
                    updatelogs = "Update logs set minheight = @newEntry where logid = @logid";
                    NpgsqlCommand sqlupminh = new NpgsqlCommand(updatelogs, conn);
                    sqlupminh.Parameters.AddWithValue("newEntry", newEntry);
                    sqlupminh.Parameters.AddWithValue("logid", currentLog.LogId);
                    sqlupminh.Prepare();
                    sqlupminh.ExecuteNonQuery();
                    break;
                default:
                    break;
            }

            conn.Close();
        }

        public string CreateImage(string from, string to, string path = "No path")
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(string path = "No path")
        {
            throw new NotImplementedException();
        }

        public void SaveImagePath(string path, string deletePath = "No path")
        {
            throw new NotImplementedException();
        }

        public void CreatePdf(TourItem tourItem, List<LogItem> logItems)
        {
            throw new NotImplementedException();
        }
    }
}
