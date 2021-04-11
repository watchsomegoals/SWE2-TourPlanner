using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    class FileSystem : IDataAccess
    {
        private string filePath;

        public FileSystem()
        {
            // get filepath from config file
            this.filePath = "C:\\Users\\Lucian\\Desktop\\swe2\\SWE2TourPlanner\\Pictures\\";
        }

        public void AddItem(string name, string description, string from, string to, string imagePath)
        {
            throw new NotImplementedException();
        }

        public string CreateImage(string from, string to)
        {
            string key = "k7iKP1Ze0UVSxBLxw4wDlsgFJqfMZZRJ";
            string imageNumber;
            string imageFilePath;
            string url = @"https://www.mapquestapi.com/staticmap/v5/map?start=" + from + "&end=" + to + "&size=600,400@2x&key=" + key;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    Random rand2 = new Random();
                    imageNumber = Convert.ToString(rand2.Next(100000));
                    imageFilePath = filePath + imageNumber + ".jpg";
                    using (FileStream lxFS = new FileStream(imageFilePath, FileMode.Create))
                    {
                        lxFS.Write(lnByte, 0, lnByte.Length);
                    }
                }
            }
            return imageFilePath;
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
