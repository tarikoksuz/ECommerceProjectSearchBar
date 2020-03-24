using Project.MODEL.Entities;
using Project.VIEWMODEL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Models.ShoppingTools
{
    public class OrderVM
    {
        public PaymentVM PaymentVM { get; set; }

        public Order Order { get; set; }
    }
}