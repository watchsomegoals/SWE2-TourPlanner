using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using SWE2TourPlanner.DataAccessLayer;

namespace NUnitTests
{
    public class FileSystemTests
    {
        public string folderPathTest = "C:\\Users\\Lucian\\Desktop\\swe2\\SWE2TourPlanner\\PicturesTest\\";
        public string filePathTest = "C:\\Users\\Lucian\\Desktop\\swe2\\SWE2TourPlanner\\ToDeleteTest.txt";
        public FileSystem fs = new FileSystem();
        

        [Test, Order(0)]
        public void CreateImageTest()
        {
            fs.PicturesFolderPath = folderPathTest;
            fs.ToDeleteFilePath = filePathTest;
            string path = fs.CreateImage("Berlin", "Vienna", fs.PicturesFolderPath);
            fs.SaveImagePath(path, fs.ToDeleteFilePath);
            int fCount = Directory.GetFiles(fs.PicturesFolderPath, "*", SearchOption.TopDirectoryOnly).Length;
            Assert.AreEqual(1, fCount);
        }

        [Test, Order(1)]
        public void DeleteImageTest()
        {
            fs.PicturesFolderPath = folderPathTest;
            fs.ToDeleteFilePath = filePathTest;
            fs.DeleteImage(fs.ToDeleteFilePath);
            int fCount = Directory.GetFiles(fs.PicturesFolderPath, "*", SearchOption.TopDirectoryOnly).Length;
            Assert.AreEqual(0, fCount);
        }
    }
}
