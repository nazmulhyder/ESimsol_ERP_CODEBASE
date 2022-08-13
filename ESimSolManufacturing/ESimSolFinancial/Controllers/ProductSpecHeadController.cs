using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
	public class ProductSpecHeadController : Controller
	{
		#region Declaration

		ProductSpecHead _oProductSpecHead = new ProductSpecHead();
		List<ProductSpecHead> _oProductSpecHeads = new  List<ProductSpecHead>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewProductSpecHeads(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oProductSpecHeads = new List<ProductSpecHead>(); 

			return View(_oProductSpecHeads);
		}
        [HttpPost]
		public JsonResult Save(ProductSpecHead oProductSpecHead)
		{
			_oProductSpecHead = new ProductSpecHead();
            _oProductSpecHeads = new List<ProductSpecHead>();
			try
			{
				_oProductSpecHead = oProductSpecHead;
                if (oProductSpecHead.ProductSpecHeads.Any())
                {
                    foreach (var oitem in oProductSpecHead.ProductSpecHeads)
                    {
                        _oProductSpecHead = new ProductSpecHead();
                        _oProductSpecHead = oitem.Save((int)Session[SessionInfo.currentUserID]);
                        _oProductSpecHeads.Add(_oProductSpecHead);
                    }
                   

                }
				
			}
			catch (Exception ex)
			{
				_oProductSpecHead = new ProductSpecHead();
				_oProductSpecHead.ErrorMessage = ex.Message;
                _oProductSpecHeads.Add(_oProductSpecHead);
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductSpecHeads);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

		[HttpPost]
        public JsonResult Delete(int id, int ProductID)
		{
			string sFeedBackMessage = "";
            ProductSpecHead _oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> _oSProductSpecHeads = new List<ProductSpecHead>();
		
				ProductSpecHead oProductSpecHead = new ProductSpecHead();
				sFeedBackMessage = oProductSpecHead.Delete(id,ProductID, (int)Session[SessionInfo.currentUserID]);
                if (sFeedBackMessage == "Data delete successfully")
                {
                    string sSQL = "Select * from View_ProductSpecHead  Where ProductID =" + ProductID + " Order By Sequence";
                    _oSProductSpecHeads = ProductSpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProductSpecHead = new ProductSpecHead();
                    _oProductSpecHead.ErrorMessage = sFeedBackMessage;
                    _oSProductSpecHeads.Add(_oProductSpecHead);
                }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSProductSpecHeads);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult ProductSearchByName(Product oProduct) 
        {
            Product _oProduct = new Product();
            List<Product> _oProducts = new List<Product>();
            string sSQL = string.Empty;
            try
            {

                sSQL = "Select * from Product Where ProductName like '%" + oProduct.ProductName + "'";
                _oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }

            var jsonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SpecHeadSearchByName(SpecHead oSpecHead)
        {
            SpecHead _oSpecHead = new SpecHead();
            List<SpecHead> _oSpecHeads = new List<SpecHead>();
            string sSQL = string.Empty;
            try
            {
                if(!string.IsNullOrEmpty(oSpecHead.Params)){
                    string SpecHeadIDs = oSpecHead.Params.Substring(0, oSpecHead.Params.LastIndexOf(','));
                    sSQL = "Select * from SpecHead Where IsActive <>0 AND SpecName like '%" + oSpecHead.SpecName + "%' AND SpecHeadID NOT in(" + SpecHeadIDs + ")";
                }
                else
                {
                    sSQL = "Select * from SpecHead Where IsActive <>0 AND SpecName like '%" + oSpecHead.SpecName + "%'";
                }
                 
               
               
                _oSpecHeads = SpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSpecHead = new SpecHead();
                _oSpecHead.ErrorMessage = ex.Message;
                _oSpecHeads.Add(_oSpecHead);
            }

            var jsonResult = Json(_oSpecHeads, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ProductSpecHeadByProduct(ProductSpecHead oProductSpecHead)
        {
            ProductSpecHead _oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> _oSProductSpecHeads = new List<ProductSpecHead>();
            string sSQL = string.Empty;
            try
            {

                sSQL = "Select * from View_ProductSpecHead Where ProductID =" + oProductSpecHead.ProductID + "Order By Sequence";
                _oSProductSpecHeads = ProductSpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oProductSpecHead = new ProductSpecHead();
                _oProductSpecHead.ErrorMessage = ex.Message;
                _oSProductSpecHeads.Add(_oProductSpecHead);
            }

            var jsonResult = Json(_oSProductSpecHeads, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        public JsonResult UpDown(ProductSpecHead oPSHead)
        {
            ProductSpecHead oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> oProductSpecHeads = new List<ProductSpecHead>();
            try
            {
                oProductSpecHead = oProductSpecHead.UpDown(oPSHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oProductSpecHead = new ProductSpecHead();
                oProductSpecHead.ErrorMessage = ex.Message;
            }
            if (oProductSpecHead.ErrorMessage == "")
            {
                try
                {
                    string sSql = "SELECT * from View_ProductSpecHead  Where ProductID =" + oPSHead.ProductID + " Order By Sequence";
                    oProductSpecHeads = ProductSpecHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                catch (Exception ex)
                {
                    oProductSpecHead = new ProductSpecHead();
                    oProductSpecHead.ErrorMessage = ex.Message;
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductSpecHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SpecHeadSearchByProduct(SpecHead oSpecHead)
        {
            SpecHead _oSpecHead = new SpecHead();
            List<SpecHead> _oSpecHeads = new List<SpecHead>();
            string sSQL = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(oSpecHead.Params))
                {
                    string SpecHeadIDs = oSpecHead.Params.Substring(0, oSpecHead.Params.LastIndexOf(','));
                    sSQL = "Select * from SpecHead Where IsActive <>0 AND SpecHeadID NOT in(" + SpecHeadIDs + ")";
                }
                else
                {
                    sSQL = "Select * from SpecHead Where IsActive <>0 ";
                }



                _oSpecHeads = SpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSpecHead = new SpecHead();
                _oSpecHead.ErrorMessage = ex.Message;
                _oSpecHeads.Add(_oSpecHead);
            }

            var jsonResult = Json(_oSpecHeads, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


		#endregion

	}

}
