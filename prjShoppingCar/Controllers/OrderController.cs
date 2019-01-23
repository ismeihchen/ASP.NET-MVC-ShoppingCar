using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjShoppingCar.Models;

namespace prjShoppingCar.Controllers
{
    public class OrderController : BaseController
    {
        //確認訂購
        public ActionResult Complete()
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //找出會員帳號並指定給fUserId
            string fUserId = Member.fUserId;

            var orderDetail = db.tOrderDetail
                .Where(m => m.fIsApproved == "否" && m.fUserId == fUserId).ToList();

            if (orderDetail == null || orderDetail.Count ==0)
            {               
                return RedirectToAction("Index");
            }

            //找出目前會員的所有訂單主檔記錄並依照fDate進行遞增排序
            //將查詢結果指定給orders
            var orders = db.tOrder
                .Where(m => m.fUserId == fUserId)
                .OrderByDescending(m => m.fDate).FirstOrDefault();
            //目前會員的訂單主檔

            var tOrderViewModel = new tOrderViewModel();
            tOrderViewModel.tOrderData = orders;
            tOrderViewModel.tOrderDetailsData = orderDetail;

            //指定Complete.cshtml套用_LayoutMember.cshtml，View使用orders模型
            return View("Complete", "_LayoutMember", tOrderViewModel);
        }

        //Post:Order/Complete
        [HttpPost]
        public ActionResult Complete(tOrderViewModel tOrderViewModel)
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //找出會員帳號並指定給fUserId
            string fUserId = Member.fUserId;
            //建立唯一的識別值並指定給guid變數，用來當做訂單編號
            //tOrder的fOrderGuid欄位會關聯到tOrderDetail的fOrderGuid欄位
            //形成一對多的關係，即一筆訂單資料會對應到多筆訂單明細
            string guid = Guid.NewGuid().ToString();
            //建立訂單主檔資料
            tOrder order = new tOrder();
            order.fOrderGuid = guid;
            order.fUserId = fUserId;
            order.fReceiver = tOrderViewModel.tOrderData.fReceiver;
            order.fEmail = tOrderViewModel.tOrderData.fEmail;
            order.fPhone = tOrderViewModel.tOrderData.fPhone;
            order.fAddress = tOrderViewModel.tOrderData.fAddress;
            order.fDate = DateTime.Now;

            //找出目前會員在訂單明細中是購物車狀態的產品
            var carList = db.tOrderDetail
                .Where(m => m.fIsApproved == "否" && m.fUserId == fUserId)
                .ToList();

            int totalPrice = 0;
            //將購物車狀態產品的fIsApproved設為"是"，表示確認訂購產品
            foreach (var item in carList)
            {
                item.fOrderGuid = guid;
                item.fIsApproved = "是";
                totalPrice += item.fAmount;
            }

            //總價
            order.fTotalPrice = totalPrice; 
            db.tOrder.Add(order);

            //更新資料庫，異動tOrder和tOrderDetail
            //完成訂單主檔和訂單明細的更新
            db.SaveChanges();
            //執行Home控制器的OrderList動作方法
            return RedirectToAction("OrderList");
        }

        //Get:Order/OrderList
        public ActionResult OrderList()
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //找出會員帳號並指定給fUserId
            string fUserId = Member.fUserId;
            //找出目前會員的所有訂單主檔記錄並依照fDate進行遞增排序
            //將查詢結果指定給orders
            var orders = db.tOrder.Where(m => m.fUserId == fUserId)
                .OrderByDescending(m => m.fDate).ToList();
            //目前會員的訂單主檔
            //指定OrderList.cshtml套用_LayoutMember.cshtml，View使用orders模型
            return View("OrderList", "_LayoutMember", orders);
        }

        //Get:Order/OrderDetail
        public ActionResult OrderDetail(string fOrderGuid)
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //找出會員帳號並指定給fUserId
            string fUserId = Member.fUserId;

            var orderDetail = db.tOrderDetail
                .Where(m => m.fOrderGuid == fOrderGuid && m.fUserId == fUserId).ToList();

            //找出目前會員的所有訂單主檔記錄並依照fDate進行遞增排序
            //將查詢結果指定給orders
            var orders = db.tOrder
                .Where(m => m.fOrderGuid == fOrderGuid && m.fUserId == fUserId)
                .OrderByDescending(m => m.fDate).FirstOrDefault();
            
            var tOrderViewModel = new tOrderViewModel();
            tOrderViewModel.tOrderData = orders;
            tOrderViewModel.tOrderDetailsData = orderDetail;

            //指定OrderDetail.cshtml套用_LayoutMember.cshtml，View使用orders模型
            return View("OrderDetail", "_LayoutMember", tOrderViewModel);
        }
    }
}