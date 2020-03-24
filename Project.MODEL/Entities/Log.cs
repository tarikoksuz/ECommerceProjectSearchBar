using Project.MODEL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MODEL.Entities
{
    public class Log:BaseEntity
    {
        public string Name { get; set; }
        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string Information { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogOutTime { get; set; }

        public KeyWord Description { get; set; }
    }
}
