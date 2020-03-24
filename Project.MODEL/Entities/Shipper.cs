using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MODEL.Entities
{
    public class Shipper:BaseEntity
    {
        public string Address { get; set; }

        //Relational Properties


        public virtual List<Order> Orders { get; set; }


    }
}
