using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MODEL.Entities
{
    public class OrderDetail:BaseEntity
    {
        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }



        //Relational Properties

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }


    }
}
