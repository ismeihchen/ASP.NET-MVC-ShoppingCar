using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjShoppingCar.Models;

namespace prjShoppingCar.Controllers
{
    public class MemberController : BaseController
    {
        //Get: Member/Login
        public ActionResult Login()
        {
            return View();
        }

        //Post: Member/Login
        [HttpPost]
        public ActionResult Login(string fUserId, string fPwd)
        {
            // 依帳密取得會員並指定給member
            var member = db.tMember
                .Where(m => m.fUserId == fUserId && m.fPwd == fPwd)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            }
            //使用Session變數記錄歡迎詞
            Session["WelCome"] = member.fName + "歡迎光臨";
            //使用Session變數記錄登入的會員物件
            Session["Member"] = member;
            //執行Home控制器的Index動作方法
            return RedirectToAction("Index","Home");
        }


        //Get:Member/Register
        public ActionResult Register()
        {
            return View();
        }

        //Post:Member/Register
        [HttpPost]
        public ActionResult Register(tMember pMember)
        {
            //若模型沒有通過驗證則顯示目前的View
            if (ModelState.IsValid == false)
            {
                return View();
            }
            // 依帳號取得會員並指定給member
            var member = db.tMember
                .Where(m => m.fUserId == pMember.fUserId)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                //將會員記錄新增到tMember資料表
                db.tMember.Add(pMember);
                db.SaveChanges();
                //執行Home控制器的Login動作方法
                return RedirectToAction("Login");
            }
            ViewBag.Message = "此帳號己有人使用，註冊失敗";
            return View();
        }

        // 執行會員登出
        public ActionResult Logout()
        {
            Session.Clear();  //清除Session變數資料
            return RedirectToAction("Index", "Home");
        }

    }
}