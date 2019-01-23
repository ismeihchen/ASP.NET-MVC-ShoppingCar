using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjShoppingCar.Models
{
  
    public class tOrderViewModel
    {
        public tOrder tOrderData { get; set; }
        public IEnumerable<tOrderDetail> tOrderDetailsData { get; set; }
    }
}