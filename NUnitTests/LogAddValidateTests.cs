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

    }
}
