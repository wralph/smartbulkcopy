using NUnit.Framework;
using NLog;
using SmartBulkCopy;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using DotNetEnv;
using NUnit.Framework.Legacy;


namespace SmartBulkCopy.Tests
{
    public class Configuration
    {
        private SmartBulkCopyConfiguration _config;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            Env.Load();

            _logger = LogManager.GetCurrentClassLogger();

            _config = SmartBulkCopyConfiguration.LoadFromConfigFile("smartbulkcopy.config.test.json", _logger);
        }

        [Test]
        public void CommandTimeOut()
        {
            ClassicAssert.AreEqual(_config.CommandTimeOut, 90 * 60);
        }

        [Test]
        public void StopIfSecondaryIndex()
        {
            ClassicAssert.IsTrue(_config.StopIf.HasFlag(StopIf.SecondaryIndex));
        }

        [Test]
        public void DontStopIfTemporalTable()
        {
           ClassicAssert.IsFalse(_config.StopIf.HasFlag(StopIf.TemporalTable));
        }

        [Test]
        public void CompatibilityMode()
        {
            ClassicAssert.IsTrue(_config.UseCompatibilityMode);
        }
    }
}
