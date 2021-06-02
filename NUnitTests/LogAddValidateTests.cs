using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SWE2TourPlanner.ViewModels;

namespace NUnitTests
{
    public class LogAddValidateTests
    {
        LogAddViewModel logAddViewModel = new LogAddViewModel();
        bool res;

        [Test, Order(0)]
        public void ValidateDateTimeTestNull()
        {
            logAddViewModel.DateTime = null;
            logAddViewModel.ValidateDateTime();
            Assert.AreEqual("Date Time cannot be empty.", logAddViewModel._errorsByPropertyName["DateTime"][0]);
        }

        [Test, Order(1)]
        public void ValidateDateTimeTestWrongFormat()
        {
            logAddViewModel.DateTime = "27.08.1992";
            logAddViewModel.ValidateDateTime();
            Assert.AreEqual("Date Time Format must be DD/MM/YYYY.", logAddViewModel._errorsByPropertyName["DateTime"][0]);
        }

        [Test, Order(2)]
        public void ValidateDateTimeTestCorrect()
        {
            logAddViewModel.DateTime = "27/08/1992";
            res = logAddViewModel.ValidateDateTime();
            Assert.IsTrue(res);
        }

        [Test, Order(3)]
        public void ValidateReportTestNull()
        {
            logAddViewModel.Report = null;
            logAddViewModel.ValidateReport();
            Assert.AreEqual("Report cannot be empty.", logAddViewModel._errorsByPropertyName["Report"][0]);
        }

        [Test, Order(4)]
        public void ValidateReportTestTooLong()
        {
            logAddViewModel.Report = "1123423454354542435454234454243534234543443453453535" +
                "sdsafdsffasdafdsfasfdsfdsafsfdfasfasfadsfdasdfasdfasdfadsljhhjhljkdsfasfd" +
                "asddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
            logAddViewModel.ValidateReport();
            Assert.AreEqual("Report has to be lower than 50 characters.", logAddViewModel._errorsByPropertyName["Report"][0]);
        }

        [Test, Order(5)]
        public void ValidateReportTestCorrect()
        {
            logAddViewModel.Report = "A correct report";
            res = logAddViewModel.ValidateReport();
            Assert.IsTrue(res);
        }

        [Test, Order(6)]
        public void ValidateDistanceTestNull()
        {
            logAddViewModel.Distance = null;
            logAddViewModel.ValidateDistance();
            Assert.AreEqual("Distance cannot be empty.", logAddViewModel._errorsByPropertyName["Distance"][0]);
        }

        [Test, Order(7)]
        public void ValidateDistanceTestDataType()
        {
            logAddViewModel.Distance = "asd";
            logAddViewModel.ValidateDistance();
            Assert.AreEqual("Distance has to be a float.", logAddViewModel._errorsByPropertyName["Distance"][0]);
        }

        [Test, Order(8)]
        public void ValidateDistanceTestTooBig()
        {
            logAddViewModel.Distance = "10001";
            logAddViewModel.ValidateDistance();
            Assert.AreEqual("Distance has to be between 1 and 10000 km.", logAddViewModel._errorsByPropertyName["Distance"][0]);
        }

        [Test, Order(9)]
        public void ValidateDistanceTestCorrect()
        {
            logAddViewModel.Distance = "27";
            res = logAddViewModel.ValidateDistance();
            Assert.IsTrue(res);
        }

        [Test, Order(10)]
        public void ValidateTotalTimeTestNull()
        {
            logAddViewModel.TotalTime = null;
            logAddViewModel.ValidateTotalTime();
            Assert.AreEqual("Total Time cannot be empty.", logAddViewModel._errorsByPropertyName["TotalTime"][0]);
        }

        [Test, Order(11)]
        public void ValidateTotalTimeTestWrongFormat()
        {
            logAddViewModel.TotalTime = "26:44";
            logAddViewModel.ValidateTotalTime();
            Assert.AreEqual("Total Time Format must be HH:MM:SS.", logAddViewModel._errorsByPropertyName["TotalTime"][0]);
        }

        [Test, Order(12)]
        public void ValidateTotalTimeTestCorrect()
        {
            logAddViewModel.TotalTime = "13:05:37";
            res = logAddViewModel.ValidateTotalTime();
            Assert.IsTrue(res);
        }

        [Test, Order(13)]
        public void ValidateRatingTestNull()
        {
            logAddViewModel.Rating = null;
            logAddViewModel.ValidateRating();
            Assert.AreEqual("Rating cannot be empty.", logAddViewModel._errorsByPropertyName["Rating"][0]);
        }

        [Test, Order(14)]
        public void ValidateRatingTestDataType()
        {
            logAddViewModel.Rating = "34.7";
            logAddViewModel.ValidateRating();
            Assert.AreEqual("Rating has to be an integer.", logAddViewModel._errorsByPropertyName["Rating"][0]);
        }

        [Test, Order(15)]
        public void ValidateRatingTestTooBig()
        {
            logAddViewModel.Rating = "11";
            logAddViewModel.ValidateRating();
            Assert.AreEqual("Rating has to be between 1 and 10.", logAddViewModel._errorsByPropertyName["Rating"][0]);
        }

        [Test, Order(16)]
        public void ValidateRatingTestCorrect()
        {
            logAddViewModel.Rating = "7";
            res = logAddViewModel.ValidateRating();
            Assert.IsTrue(res);
        }

        [Test, Order(17)]
        public void ValidateAvgSpeedTestNull()
        {
            logAddViewModel.AvgSpeed = null;
            logAddViewModel.ValidateAvgSpeed();
            Assert.AreEqual("Average Speed cannot be empty.", logAddViewModel._errorsByPropertyName["AvgSpeed"][0]);
        }

        [Test, Order(18)]
        public void ValidateAvgSpeedTestDataType()
        {
            logAddViewModel.AvgSpeed = "27.7";
            logAddViewModel.ValidateAvgSpeed();
            Assert.AreEqual("Average Speed has to be an integer.", logAddViewModel._errorsByPropertyName["AvgSpeed"][0]);
        }

        [Test, Order(19)]
        public void ValidateAvgSpeedTestTooBig()
        {
            logAddViewModel.AvgSpeed = "201";
            logAddViewModel.ValidateAvgSpeed();
            Assert.AreEqual("Average Speed has to be between 1 and 200 km/h.", logAddViewModel._errorsByPropertyName["AvgSpeed"][0]);
        }

        [Test, Order(20)]
        public void ValidateAvgSpeedTestCorrect()
        {
            logAddViewModel.AvgSpeed = "7";
            res = logAddViewModel.ValidateAvgSpeed();
            Assert.IsTrue(res);
        }

        [Test, Order(21)]
        public void ValidateInclinationTestNull()
        {
            logAddViewModel.Inclination = null;
            logAddViewModel.ValidateInclination();
            Assert.AreEqual("Inclination cannot be empty.", logAddViewModel._errorsByPropertyName["Inclination"][0]);
        }

        [Test, Order(22)]
        public void ValidateInclinationTestDataType()
        {
            logAddViewModel.Inclination = "27.7";
            logAddViewModel.ValidateInclination();
            Assert.AreEqual("Inclination has to be an integer.", logAddViewModel._errorsByPropertyName["Inclination"][0]);
        }

        [Test, Order(23)]
        public void ValidateInclinationTestTooBig()
        {
            logAddViewModel.Inclination = "71";
            logAddViewModel.ValidateInclination();
            Assert.AreEqual("Inclination has to be between 1 and 70 degrees.", logAddViewModel._errorsByPropertyName["Inclination"][0]);
        }

        [Test, Order(24)]
        public void ValidateInclinationTestCorrect()
        {
            logAddViewModel.Inclination = "69";
            res = logAddViewModel.ValidateInclination();
            Assert.IsTrue(res);
        }

        [Test, Order(25)]
        public void ValidateTopSpeedTestNull()
        {
            logAddViewModel.TopSpeed = null;
            logAddViewModel.ValidateTopSpeed();
            Assert.AreEqual("Top Speed cannot be empty.", logAddViewModel._errorsByPropertyName["TopSpeed"][0]);
        }

        [Test, Order(26)]
        public void ValidateTopSpeedTestDataType()
        {
            logAddViewModel.TopSpeed = "27.7";
            logAddViewModel.ValidateTopSpeed();
            Assert.AreEqual("Top Speed has to be an integer.", logAddViewModel._errorsByPropertyName["TopSpeed"][0]);
        }

        [Test, Order(27)]
        public void ValidateTopSpeedTestTooBig()
        {
            logAddViewModel.TopSpeed = "301";
            logAddViewModel.ValidateTopSpeed();
            Assert.AreEqual("Top Speed has to be between 1 and 300 km/h.", logAddViewModel._errorsByPropertyName["TopSpeed"][0]);
        }

        [Test, Order(28)]
        public void ValidateTopSpeedTestCorrect()
        {
            logAddViewModel.TopSpeed = "69";
            res = logAddViewModel.ValidateTopSpeed();
            Assert.IsTrue(res);
        }

        [Test, Order(29)]
        public void ValidateMaxHeightTestNull()
        {
            logAddViewModel.MaxHeight = null;
            logAddViewModel.ValidateMaxHeight();
            Assert.AreEqual("Max Height cannot be empty.", logAddViewModel._errorsByPropertyName["MaxHeight"][0]);
        }

        [Test, Order(30)]
        public void ValidateMaxHeightTestDataType()
        {
            logAddViewModel.MaxHeight = "27.7";
            logAddViewModel.ValidateMaxHeight();
            Assert.AreEqual("Max Height has to be an integer.", logAddViewModel._errorsByPropertyName["MaxHeight"][0]);
        }

        [Test, Order(31)]
        public void ValidateMaxHeightTestTooBig()
        {
            logAddViewModel.MaxHeight = "10001";
            logAddViewModel.ValidateMaxHeight();
            Assert.AreEqual("Max Height has to be between 1 and 10000 metres.", logAddViewModel._errorsByPropertyName["MaxHeight"][0]);
        }
        
        [Test, Order(32)]
        public void ValidateMaxHeightTestCorrect()
        {
            logAddViewModel.MaxHeight = "1000";
            res = logAddViewModel.ValidateMaxHeight();
            Assert.IsTrue(res);
        }

        [Test, Order(33)]
        public void ValidateMinHeightTestNull()
        {
            logAddViewModel.MinHeight = null;
            logAddViewModel.ValidateMinHeight();
            Assert.AreEqual("Min Height cannot be empty.", logAddViewModel._errorsByPropertyName["MinHeight"][0]);
        }

        [Test, Order(34)]
        public void ValidateMinHeightTestDataType()
        {
            logAddViewModel.MinHeight = "27.7";
            logAddViewModel.ValidateMinHeight();
            Assert.AreEqual("Min Height has to be an integer.", logAddViewModel._errorsByPropertyName["MinHeight"][0]);
        }

        [Test, Order(35)]
        public void ValidateMinHeightTestTooBig()
        {
            logAddViewModel.MinHeight = "10001";
            logAddViewModel.ValidateMinHeight();
            Assert.AreEqual("Min Height has to be between 1 and 10000 metres.", logAddViewModel._errorsByPropertyName["MinHeight"][0]);
        }

        [Test, Order(36)]
        public void ValidateMinHeightTestCorrect()
        {
            logAddViewModel.MinHeight = "1000";
            res = logAddViewModel.ValidateMinHeight();
            Assert.IsTrue(res);
        }
    }
}
