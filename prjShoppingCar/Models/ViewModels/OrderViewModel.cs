using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjShoppingCar.Models.ViewModels
{
    public class OrderViewModel
    {
        public prjShoppingCar.Models.tOrder tOrder { get; set; }
        public IEnumerable<prjShoppingCar.Models.tOrderDetail> tOrderDetail { get; set; }
        
    }
}
