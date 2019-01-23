using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using prjShoppingCar.Models;


namespace prjShoppingCar.Controllers
{
    public class HomeController : BaseController
    {

        // Get:Home/Index
        public ActionResult Index()
        {
            //取得所有產品放入products
            var products = db.tProduct.ToList();
            //若Session["Member"]為空，表示會員未登入
            if (Session["Member"] == null)
            {
                //指定Index.cshtml套用_Layout.cshtml，View使用products模型
                return View("Index", "_Layout", products);
            }
            //會員登入狀態
            //指定Index.cshtml套用_LayoutMember.cshtml，View使用products模型
            return View("Index", "_LayoutMember", products);
        }



 



   





    }
}