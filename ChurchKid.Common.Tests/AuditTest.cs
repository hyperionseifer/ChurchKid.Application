using ChurchKid.Common.Audit;
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ChurchKid.Common.Tests
{
    [TestClass]
    public class AuditTest
    {

        [TestMethod]
        public void LogFileShouldBeCreated()
        {
            var logFile = Logger.LogFile;
            Assert.IsTrue(File.Exists(logFile));
        }

        [TestMethod]
        public void LogEntryShouldBeSaved()
        {
            var logEntry = Logger.Write("This is a sample entry.");
            Assert.IsNotNull(logEntry);
        }

        [TestMethod]
        public void LogEntryShouldBeSearchable()
        {
            Logger.Clear();
            var logEntry = Logger.Write("This is a searchable entry.");
            Assert.IsNotNull(logEntry);
            var searchedEntry = Logger.GetLog(logEntry.Id);
            Assert.IsNotNull(searchedEntry);
        }

        [TestMethod]
        public void LogEntriesShouldBeSearchable()
        {
            Logger.Clear();

            var infoEntries = 5;
            for (var i = 1; i <= infoEntries; i++)
                Logger.Write(string.Format("Sample information {0}.", i));

            var errorEntries = 3;
            for (var e = 1; e <= errorEntries; e++)
                Logger.Write(new Exception("Sample error " + e));

            var logEntries = (List<LogEntry>)Logger.GetLogs();
            Assert.AreEqual(infoEntries + errorEntries, logEntries.Count);

            var infoSearchResults = (List<LogEntry>)Logger.GetInfoEntries();
            Assert.AreEqual(infoEntries, infoSearchResults.Count);

            var errorSearchResults = (List<LogEntry>)Logger.GetErrorEntries();
            Assert.AreEqual(errorEntries, errorSearchResults.Count);
        }
    }
}
