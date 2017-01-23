using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.SMS.Model
{
    public class Contact
    {
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Mobile { get; set; }
    }
}
