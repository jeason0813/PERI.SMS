using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using System.Diagnostics;

namespace PERI.SMS.XU
{
    public class Class1
    {
        [Fact]
        public void RandomNumber()
        {
            var num = GenerateRandomNumber();
            Debug.WriteLine(num);
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public string GenerateRandomNumber()
        {
            lock (syncLock)
            {
                var prefix = string.Format("09{0}", random.Next(0, 99).ToString().PadLeft(2, '0'));
                var num = random.Next(0, 9999999).ToString().PadLeft(7, '0');
                return string.Concat(prefix, num);
            }
        }
    }
}
