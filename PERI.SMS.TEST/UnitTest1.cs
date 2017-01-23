using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using System.Collections.Generic;
using EntityFramework.BulkInsert.Extensions;

namespace PERI.SMS.TEST
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CoreSMSSender()
        {
            var port = Core.COMPort.OpenPort("COM22", 9600, 8, 300, 300);
            var ok = Core.SMS.Send(port, "09255791113", "hello!");
        }

        [TestMethod]
        public void CoreSMSReadAll()
        {
            var port = Core.COMPort.OpenPort("COM34", 9600, 8, 300, 300);
            var msgs = Core.SMS.Read.All(port);
        }

        [TestMethod]
        public void CoreSMSReadAllUnread()
        {
            var port = Core.COMPort.OpenPort("COM25", 9600, 8, 300, 300);
            var msgs = Core.SMS.Read.AllUnread(port);
        }

        [TestMethod]
        public void CoreSMSDeleteAll()
        {
            var port = Core.COMPort.OpenPort("COM25", 9600, 8, 300, 300);
            var isOk = Core.SMS.Delete.All(port);
        }

        [TestMethod]
        public void CoreSMSDeleteAllRead()
        {
            var port = Core.COMPort.OpenPort("COM25", 9600, 8, 300, 300);
            var isOk = Core.SMS.Delete.AllRead(port);
        }
    }
}
