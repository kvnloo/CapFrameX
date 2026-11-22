using CapFrameX.Contracts.Configuration;
using CapFrameX.PresentMonInterface;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CapFrameX.Test.PresentMonInterface
{
    [TestClass]
    public class PresentMonCaptureServiceLayoutTest
    {
        [TestMethod]
        public void PcLatencyEnabled_ExposesPcLatencyColumnAtExpectedIndex()
        {
            var service = CreateService(usePcLatency: true);

            var columns = PresentMonCaptureService.COLUMN_HEADER_WITH_PC_LATENCY.Split(',');
            int expectedIndex = Array.IndexOf(columns, "MsPCLatency");

            Assert.IsTrue(expectedIndex >= 0);
            Assert.AreEqual(expectedIndex, service.MsPcLatency_Index);
            Assert.AreEqual(columns.Length, service.ValidLineLength);
            Assert.AreEqual(PresentMonCaptureService.COLUMN_HEADER_WITH_PC_LATENCY, service.ColumnHeader);
        }

        [TestMethod]
        public void PcLatencyDisabled_HasNoPcLatencyColumnAndUsesShorterLayout()
        {
            var service = CreateService(usePcLatency: false);

            var columns = PresentMonCaptureService.COLUMN_HEADER_WITHOUT_PC_LATENCY.Split(',');

            Assert.AreEqual(-1, service.MsPcLatency_Index);
            Assert.AreEqual(columns.Length, service.ValidLineLength);
            Assert.AreEqual(PresentMonCaptureService.COLUMN_HEADER_WITHOUT_PC_LATENCY, service.ColumnHeader);
            Assert.AreEqual(
                PresentMonCaptureService.COLUMN_HEADER_WITH_PC_LATENCY.Split(',').Length - 1,
                service.ValidLineLength);
        }

        private static PresentMonCaptureService CreateService(bool usePcLatency)
        {
            var configuration = new Mock<IAppConfiguration>();
            configuration.SetupGet(x => x.UsePcLatency).Returns(usePcLatency);
            var logger = new Mock<ILogger<PresentMonCaptureService>>();

            return new PresentMonCaptureService(logger.Object, configuration.Object);
        }
    }
}
