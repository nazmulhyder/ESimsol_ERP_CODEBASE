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
	public class CostHeadController : Controller
	{
		#region Declaration

		CostHead _oCostHead = new CostHead();
		List<CostHead> _oCostHeads = new  List<CostHead>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewCostHeads(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			//this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'CostHead'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oCostHeads = new List<CostHead>(); 
			_oCostHeads = CostHead.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EnumCostHeadTypes = EnumObject.jGets(typeof(EnumCostHeadType));
            //ViewBag.EnumCostHeadCategoreys = EnumObject.jGets(typeof(EnumCostHeadCategorey));
			return View(_oCostHeads);
		}

		

		[HttpPost]
		public JsonResult Save(CostHead oCostHead)
		{
			_oCostHead = new CostHead();
			try
			{
				_oCostHead = oCostHead;
				_oCostHead = _oCostHead.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oCostHead = new CostHead();
				_oCostHead.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oCostHead);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Delete(CostHead oCostHead)
		{
			string sFeedBackMessage = "";
			try
			{
                //CostHead oCostHead = new CostHead();
                sFeedBackMessage = oCostHead.Delete(oCostHead.CostHeadID, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult GetsCostHead(CostHead oCostHead)
        {
            List<CostHead> oCostHeads = new List<CostHead>();
            List<PurchaseCost> oPurchaseCosts = new List<PurchaseCost>();
            PurchaseCost oPurchaseCost = new PurchaseCost();
            string sSQL = "";
            if (!string.IsNullOrEmpty(oCostHead.Params))
            {
                sSQL = "SELECT * FROM View_CostHead WHERE CostHeadID Not IN(" + oCostHead.Params + ")";
            }
            else
            {
                sSQL = "SELECT * FROM View_CostHead";
            }

            oCostHeads = CostHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (CostHead oItem in oCostHeads)
            {
                oPurchaseCost = new PurchaseCost();
                oPurchaseCost.PurchaseCostID = 0;
                oPurchaseCost.RefID = 0;
                oPurchaseCost.RefType =(int) EnumModuleName.PurchaseOrder;
                oPurchaseCost.CostHeadID = oItem.CostHeadID;
                oPurchaseCost.Name = oItem.Name;
                oPurchaseCost.ValueInPercent = 0;
                oPurchaseCost.ValueInAmount = 0;
                oPurchaseCost.CostHeadType = oItem.CostHeadType;
                //oPurchaseCost.CostHeadCategorey = oItem.CostHeadCategorey;
                oPurchaseCosts.Add(oPurchaseCost);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPurchaseCosts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


		#endregion

	}

}
