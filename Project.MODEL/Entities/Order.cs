﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MODEL.Entities
{
    public class Order:BaseEntity
    {

        public string ShippedAddress { get; set; }

        public int? AppUserID { get; set; }

        public string UserName { get; set; }

        public decimal TotalPrice { get; set; }

        public string Email { get; set; }

        public int? ShipperID { get; set; }





        //Relational Properties
        public virtual AppUser AppUser { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

        public virtual Shipper Shipper { get; set; }




    }
}
