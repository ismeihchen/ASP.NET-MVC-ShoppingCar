using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjShoppingCar.Models;

namespace prjShoppingCar.Controllers
{
    public class CartController : BaseController
    {
        //Get:Index/ShoppingCar
        public ActionResult Index()
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //取得登入會員的帳號並指定給fUserId
            string fUserId = Member.fUserId;
            //找出未成為訂單明細的資料，即購物車內容
            var orderDetails = db.tOrderDetail.Where
                (m => m.fUserId == fUserId && m.fIsApproved == "否")
                .ToList();

            //指定ShoopingCar.cshtml套用_LayoutMember.cshtml，View使用orderDetails模型
            return View("Index", "_LayoutMember", orderDetails);
        }

        //Get:Car/AddCar
        [HttpGet]
        public ActionResult AddCar(string fPId)
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //取得會員帳號並指定給fUserId
            string fUserId = Member.fUserId;
            //找出會員放入訂單明細的產品，該產品的fIsApproved為"否"
            //表示該產品是購物車狀態
            var currentCar = db.tOrderDetail
                .Where(m => m.fPId == fPId && m.fIsApproved == "否" && m.fUserId == fUserId)
                .FirstOrDefault();
            //若currentCar等於null，表示會員選購的產品不是購物車狀態
            if (currentCar == null)
            {
                //找出目前選購的產品並指定給product
                var product = db.tProduct.Where(m => m.fPId == fPId).FirstOrDefault();
                //將產品放入訂單明細，因為產品的fIsApproved為"否"，表示為購物車狀態
                tOrderDetail orderDetail = new tOrderDetail();
                orderDetail.fUserId = fUserId;
                orderDetail.fPId = product.fPId;
                orderDetail.fName = product.fName;
                orderDetail.fPrice = product.fPrice;
                orderDetail.fQty = 1;
                orderDetail.fAmount = product.fPrice * orderDetail.fQty;
                orderDetail.fIsApproved = "否";
                db.tOrderDetail.Add(orderDetail);
            }
            else
            {
                //若產品為購物車狀態，即將該產品數量加1                
                currentCar.fQty = (currentCar.fQty > 8 ? 9 : currentCar.fQty++);
                currentCar.fAmount = currentCar.fPrice * currentCar.fQty;
            }
            db.SaveChanges();
            //執行Cart控制器的Index動作方法
            return RedirectToAction("Index");
        }

        //Get:Car/DeleteCar
        public ActionResult DeleteCar(int fId)
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //取得會員帳號並指定給fUserId
            string fUserId = Member.fUserId;
            // 依fId找出要刪除購物車狀態的產品
            var orderDetail = db.tOrderDetail.Where
                (m => m.fId == fId && m.fUserId == fUserId).FirstOrDefault();
            //刪除購物車狀態的產品
            db.tOrderDetail.Remove(orderDetail);
            db.SaveChanges();
            //執行Cart控制器的Index動作方法
            return RedirectToAction("Index");
        }

        //Post:Index/UpdateCar
        [HttpPost]
        public ActionResult UpdateCar(int fId, int fQty)
        {
            var Member = this.Member;
            if (Member == null)
            {
                Session.Clear();  //清除Session變數資料
                return RedirectToAction("Index");
            }

            //取得會員帳號並指定給fUserId
            string fUserId = Member.fUserId;
            // 依fId找出要變更購物車數量的產品
            var orderDetail = db.tOrderDetail.Where
                (m => m.fId == fId && m.fUserId == fUserId).FirstOrDefault();
            //若currentCar等於null，表示會員選購的產品不是購物車狀態
            if (orderDetail != null)
            {
                //若產品為購物車狀態，即將該產品數量加異動
                orderDetail.fQty = fQty; //更新數量
                orderDetail.fAmount = orderDetail.fPrice * orderDetail.fQty;
            }
            db.SaveChanges();
            //執行Cart控制器的Index動作方法
            return RedirectToAction("Index");
        }
    }
}