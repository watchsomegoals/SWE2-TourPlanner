using Newtonsoft.Json;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SWE2TourPlanner.DataAccessLayer
{
    public class FileSystem : IDataAccess
    {
        private string picturesfolderPath;
        private string toDeleteFilePath;

        public string PicturesFolderPath { get; set; }
        public string ToDeleteFilePath { get; set; }

        public FileSystem()
        {
            // get filepath from config file
            this.picturesfolderPath = "C:\\Users\\Lucian\\Desktop\\swe2\\SWE2TourPlanner\\Pictures\\";
            this.toDeleteFilePath = "C:\\Users\\Lucian\\Desktop\\swe2\\SWE2TourPlanner\\ToDelete.txt";
        }

        public void AddItem(string name, string description, string from, string to, string imagePath)
        {
            throw new NotImplementedException();
        }

        public string CreateImage(string from, string to, string path = "No path")
        {
            string key = "k7iKP1Ze0UVSxBLxw4wDlsgFJqfMZZRJ";
            string imageNumber;
            string imageFilePath;
            string url = @"https://www.mapquestapi.com/staticmap/v5/map?start=" + from + "&end=" + to + "&size=600,400@2x&key=" + key;

            if(path == "No path")
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

        public void DeleteImage(string path = "No path")
        {
            if (path == "No path")
            {
                path = toDeleteFilePath;
            }

            string json = File.ReadAllText(path);
            List<ImagesToBeDeleted> images = JsonConvert.DeserializeObject<List<ImagesToBeDeleted>>(json);
            if(images != null)
            {
                foreach (ImagesToBeDeleted image in images)
                {
                    File.Delete(image.PathToDelete);
                }
                File.WriteAllText(path, String.Empty);
            }

        }

        public void SaveImagePath(string path, string deletePath = "No path")
        {
            if (deletePath == "No path")
            {
                deletePath = toDeleteFilePath;
            }
            string initialJson = File.ReadAllText(deletePath);
            var list = JsonConvert.DeserializeObject<List<ImagesToBeDeleted>>(initialJson);
            ImagesToBeDeleted image = new ImagesToBeDeleted() { PathToDelete = path };
            if(list == null)
            {
                list = new List<ImagesToBeDeleted>();
            }
            list.Add(image);

            string output = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(deletePath, output);
        }

        public void DeleteItem(string name)
        {
            throw new NotImplementedException();
        }

        public List<TourItem> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
