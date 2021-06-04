using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Pdf.Text;
using Newtonsoft.Json;
using SWE2TourPlanner.Models;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Controls;

namespace SWE2TourPlanner.DataAccessLayer
{
    public class FileSystem : IDataAccess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string picturesfolderPath;
        private string toDeleteFilePath;
        private ConfigFile configFile;

        public string PicturesFolderPath { get; set; }
        public string ToDeleteFilePath { get; set; }

        public FileSystem()
        {
            string path = "configFile.json";
            string json = File.ReadAllText(path);
            this.configFile = JsonConvert.DeserializeObject<ConfigFile>(json);
            // get filepath from config file
            this.picturesfolderPath = configFile.FsSettings.PicturesFolderPath;
            this.toDeleteFilePath = configFile.FsSettings.ToDeleteFilePath;
        }

        public void CreatePdf(TourItem tourItem, List<LogItem> logItems)
        {
            try
            {
                // Initialize document object
                Document document = new Document();

                // Add page
                Aspose.Pdf.Page page = document.Pages.Add();

                // Add Header
                var header = new TextFragment("Tour: " + tourItem.Name + " from " + tourItem.From + " to " + tourItem.To + "\nRoute Type: " + tourItem.Route);
                header.TextState.Font = FontRepository.FindFont("Arial");
                header.TextState.FontSize = 20;
                header.HorizontalAlignment = HorizontalAlignment.Center;
                header.Position = new Position(130, 720);
                page.Paragraphs.Add(header);

                // Add table
                var table1 = new Table
                {
                    ColumnWidths = "70",
                    Border = new BorderInfo(BorderSide.Box, 1f, Color.DarkSlateGray),
                    DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Color.Black),
                    DefaultCellPadding = new MarginInfo(4.5, 4.5, 4.5, 4.5),
                    Margin =
                {
                    Bottom = 10
                },
                    DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("Helvetica")
                }
                };

                var table2 = new Table
                {
                    ColumnWidths = "70",
                    Border = new BorderInfo(BorderSide.Box, 1f, Color.DarkSlateGray),
                    DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Color.Black),
                    DefaultCellPadding = new MarginInfo(4.5, 4.5, 4.5, 4.5),
                    Margin =
                {
                    Bottom = 10
                },
                    DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("Helvetica")
                }
                };

                var headerRow1 = table1.Rows.Add();
                headerRow1.Cells.Add("Log id");
                headerRow1.Cells.Add("Date");
                headerRow1.Cells.Add("Report");
                headerRow1.Cells.Add("Distance (km)");
                headerRow1.Cells.Add("Time");
                headerRow1.Cells.Add("Rating");

                var headerRow2 = table2.Rows.Add();
                headerRow2.Cells.Add("Log id");
                headerRow2.Cells.Add("Avg Speed (km/h)");
                headerRow2.Cells.Add("Inclination (degrees)");
                headerRow2.Cells.Add("Top Speed (km/h)");
                headerRow2.Cells.Add("Max Height (m)");
                headerRow2.Cells.Add("Min Height (m)");

                foreach (Cell headerRowCell in headerRow1.Cells)
                {
                    headerRowCell.BackgroundColor = Color.Gray;
                    headerRowCell.DefaultCellTextState.ForegroundColor = Color.WhiteSmoke;
                }
                foreach (Cell headerRowCell in headerRow2.Cells)
                {
                    headerRowCell.BackgroundColor = Color.Gray;
                    headerRowCell.DefaultCellTextState.ForegroundColor = Color.WhiteSmoke;
                }

                foreach (LogItem logItem in logItems)
                {
                    var dataRow1 = table1.Rows.Add();
                    dataRow1.Cells.Add(logItem.LogId.ToString());
                    dataRow1.Cells.Add(logItem.DateTime);
                    dataRow1.Cells.Add(logItem.Report);
                    dataRow1.Cells.Add(logItem.Distance);
                    dataRow1.Cells.Add(logItem.TotalTime);
                    dataRow1.Cells.Add(logItem.Rating);

                    var dataRow2 = table2.Rows.Add();
                    dataRow2.Cells.Add(logItem.LogId.ToString());
                    dataRow2.Cells.Add(logItem.AvgSpeed);
                    dataRow2.Cells.Add(logItem.Inclination);
                    dataRow2.Cells.Add(logItem.TopSpeed);
                    dataRow2.Cells.Add(logItem.MaxHeight);
                    dataRow2.Cells.Add(logItem.MinHeight);
                }

                page.Paragraphs.Add(table1);
                page.Paragraphs.Add(table2);

                document.Save(configFile.FsSettings.DocumentsFolderPath + tourItem.TourId.ToString() + ".pdf");
                string fileName = configFile.FsSettings.DocumentsFolderPath + tourItem.TourId.ToString() + ".pdf";

                PdfViewer viewer = new PdfViewer(document);
                viewer.PrintDocument();
            }
            catch(Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
        }

        public void CreatePdfSummary(int tourNr, int logsNr, float distanceSum, float distanceAvg, float ratingAvg, float avgSpeedAvg, int topSpeed)
        {
            try 
            {
                // Initialize document object
                Document document = new Document();
                // Add page
                Aspose.Pdf.Page page = document.Pages.Add();
                // Add Header
                var header = new TextFragment("Tours and Logs Summary");
                header.TextState.Font = FontRepository.FindFont("Arial");
                header.TextState.FontSize = 20;
                header.HorizontalAlignment = HorizontalAlignment.Center;
                header.Position = new Position(130, 720);
                page.Paragraphs.Add(header);
                // Add description
                var descriptionText = "Number of tours: " + tourNr + "\nNumber of logs: " + logsNr + "\nTotal Distance: " + distanceSum + " m\nAverage Distance: " + distanceAvg +
                                      " m\nAverage Rating: " + ratingAvg + "/10" + "\nAverage Speed: " + avgSpeedAvg + " km/h" +"\nTop Speed: " + topSpeed + " km/h";
                var description = new TextFragment(descriptionText);
                description.TextState.Font = FontRepository.FindFont("Times New Roman");
                description.TextState.FontSize = 14;
                description.HorizontalAlignment = HorizontalAlignment.Center;
                page.Paragraphs.Add(description);

                Guid obj = Guid.NewGuid();
                document.Save(configFile.FsSettings.DocumentsFolderPath + "summary" + obj.ToString() + "test.pdf");
                //PdfViewer viewer = new PdfViewer(document);
                //viewer.PrintDocument();
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
        }

        public string CreateImage(string from, string to, string path = "No path")
        {
            try
            {
                string key = configFile.FsSettings.Key;
                string imageNumber;
                string imageFilePath;
                string url = @"https://www.mapquestapi.com/staticmap/v5/map?start=" + from + "&end=" + to + "&size=600,400@2x&key=" + key;

                if (path == "No path")
                {
                    path = picturesfolderPath;
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                    {
                        Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                        Random rnd = new Random();
                        imageNumber = Convert.ToString(rnd.Next(100000));
                        imageFilePath = path + imageNumber + ".jpg";
                        using (FileStream lxFS = new FileStream(imageFilePath, FileMode.Create))
                        {
                            lxFS.Write(lnByte, 0, lnByte.Length);
                        }
                    }
                }
                return imageFilePath;
            }
            catch(Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
                return strResponseValue;
            }
        }

        public void DeleteImage(string path = "No path")
        {
            try
            {
                if (path == "No path")
                {
                    path = toDeleteFilePath;
                }

                string json = File.ReadAllText(path);
                List<ImagesToBeDeleted> images = JsonConvert.DeserializeObject<List<ImagesToBeDeleted>>(json);
                if (images != null)
                {
                    foreach (ImagesToBeDeleted image in images)
                    {
                        File.Delete(image.PathToDelete);
                    }
                    File.WriteAllText(path, String.Empty);
                }
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
        }

        public void SaveImagePath(string path, string deletePath = "No path")
        {
            try
            {
                if (deletePath == "No path")
                {
                    deletePath = toDeleteFilePath;
                }
                string initialJson = File.ReadAllText(deletePath);
                var list = JsonConvert.DeserializeObject<List<ImagesToBeDeleted>>(initialJson);
                ImagesToBeDeleted image = new ImagesToBeDeleted() { PathToDelete = path };
                if (list == null)
                {
                    list = new List<ImagesToBeDeleted>();
                }
                list.Add(image);

                string output = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(deletePath, output);
            }
            catch(Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
        }

        public void DeleteItem(int tourid)
        {
            throw new NotImplementedException();
        }

        public List<TourItem> GetItems()
        {
            throw new NotImplementedException();
        }

        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime, string rating, string avgSpeed, string inclination, string topSpeed, string maxHeight, string minHeight)
        {
            throw new NotImplementedException();
        }

        public List<LogItem> GetLogs(int tourid)
        {
            throw new NotImplementedException();
        }

        public void AddItem(string name, string from, string to, string imagePath, string description, string route)
        {
            throw new NotImplementedException();
        }

        public void DeleteLog(int logid)
        {
            throw new NotImplementedException();
        }

        public void ModifyTour(TourItem currentTour, string description, string route)
        {
            throw new NotImplementedException();
        }

        public void ModifyLog(LogItem currentLog, string typeLogData, string newEntry)
        {
            throw new NotImplementedException();
        }

        public bool DoesTourExist(int tourid)
        {
            throw new NotImplementedException();
        }

        public bool DoesLogExist(int logid)
        {
            throw new NotImplementedException();
        }

        public void Export(List<ExportObject> exportObjects)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.NullValueHandling = NullValueHandling.Ignore;
                string fileName = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
                using (StreamWriter sw = new StreamWriter(configFile.FsSettings.JsonExports + fileName + ".json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, exportObjects);
                }
            }
            catch(Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
        }

        public int GetToursCount()
        {
            throw new NotImplementedException();
        }

        public float GetDistanceSum()
        {
            throw new NotImplementedException();
        }

        public int GetLogsCount()
        {
            throw new NotImplementedException();
        }

        public int GetRatingSum()
        {
            throw new NotImplementedException();
        }

        public int GetAvgSpeedSum()
        {
            throw new NotImplementedException();
        }

        public int GetTopSpeed()
        {
            throw new NotImplementedException();
        }
    }
}
    

