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
    public class ClusteredColumnstoreTests: BaseTests
    {        
        [Test]
        public async Task ClusteredColumnstore_Small()
        {
            var tar = await AnalyzeTable("schema1.clustered_columnstore");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(NoPartitionsCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(1, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.None, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }

        [Test]
        public async Task ClusteredColumnstore_Big()
        {
            var tar = await AnalyzeTable("dbo.LINEITEM_CLUSTERED_COLUMNSTORE");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(LogicalPartitionCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(3, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.None, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }

        [Test]
        public async Task ClusteredColumnstore_Big_Partitioned()
        {
            var tar = await AnalyzeTable("dbo.LINEITEM_CLUSTERED_COLUMNSTORE_PARTITIONED");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(PhysicalPartitionCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(85, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.PartionKeyOnly, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("[L_COMMITDATE]", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }      
    }
}