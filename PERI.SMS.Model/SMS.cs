using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.SMS.Model
{
    public class SMS
    {
        public Guid ID { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
        public DateTime SMSDate { get; set; }
        public int Status { get; set; }
    }
}
