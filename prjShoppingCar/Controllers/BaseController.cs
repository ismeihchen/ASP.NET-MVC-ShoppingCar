using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using prjShoppingCar.Models;

namespace prjShoppingCar.Controllers
{
    public class BaseController : Controller
    {
        //建立可存取dbShoppingCar.mdf 資料庫的dbShoppingCarEntities 類別物件db
        protected dbShoppingCarEntities db = new dbShoppingCarEntities();

        protected tMember Member
        {
            get
            {
                if (Session["Member"] == null)
                {
                    Session["Member"] = new tMember();
                }
                return (Session["Member"] as tMember);
            }
            set { Session["Member"] = value; }
        }

    }
}