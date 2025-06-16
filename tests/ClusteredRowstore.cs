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
    public class ClusteredRowstoreTests : BaseTests
    {
        [Test]
        public async Task ClusteredRowstore_Small()
        {
            var tar = await AnalyzeTable("schema1.clustered_rowstore");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(NoPartitionsCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(1, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.ClusteredIndex, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("[col17],[col19]", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }

        [Test]
        public async Task ClusteredRowstore_Big()
        {
            var tar = await AnalyzeTable("dbo.LINEITEM_CLUSTERED_ROWSTORE");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(NoPartitionsCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(1, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.ClusteredIndex, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("[L_ORDERKEY],[L_LINENUMBER]", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }

        [Test]
        public async Task ClusteredRowstore_Big_Partitioned()
        {
            var tar = await AnalyzeTable("dbo.LINEITEM_CLUSTERED_ROWSTORE_PARTITIONED");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(PhysicalPartitionCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(85, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.ClusteredIndex, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("[L_ORDERKEY],[L_LINENUMBER],[L_COMMITDATE]", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("[L_COMMITDATE]", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }

        [Test]
        public async Task ClusteredRowstore_Calculated_Big()
        {
            var tar = await AnalyzeTable("dbo.LINEITEM_CLUSTERED_ROWSTORE_CALCULATED");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(NoPartitionsCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(1, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.ClusteredIndex, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }

        [Test]
        public async Task ClusteredRowstore_Calculated_Big_Partitioned()
        {
            var tar = await AnalyzeTable("dbo.LINEITEM_CLUSTERED_ROWSTORE_CALCULATED_PARTITIONED");

            ClassicAssert.AreEqual(AnalysisOutcome.Success, tar.Outcome);
            ClassicAssert.IsInstanceOf(typeof(PhysicalPartitionCopyInfo), tar.CopyInfo[0]);
            ClassicAssert.AreEqual(85, tar.CopyInfo.Count);
            ClassicAssert.AreEqual(OrderHintType.ClusteredIndex, tar.CopyInfo[0].OrderHintType);
            ClassicAssert.AreEqual("[L_COMMITDATE]", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetOrderByString());
            ClassicAssert.AreEqual("[L_COMMITDATE]", tar.CopyInfo[0].SourceTableInfo.PrimaryIndex.GetPartitionByString());
        }
    }
}