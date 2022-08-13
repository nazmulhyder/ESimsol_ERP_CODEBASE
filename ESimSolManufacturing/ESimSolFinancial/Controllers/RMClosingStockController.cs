using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class RMClosingStockController : Controller
    {
        #region Declaration
        RMClosingStock _oRMClosingStock = new RMClosingStock();
        List<RMClosingStock> _oRMClosingStocks = new List<RMClosingStock>();
        #endregion

        public ActionResult ViewRMClosingStocks(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RMClosingStock).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oRMClosingStocks = new List<RMClosingStock>();
            _oRMClosingStocks = RMClosingStock.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oRMClosingStocks);
        }

        public ActionResult ViewRMClosingStock(int id)
        {
            _oRMClosingStock = new RMClosingStock();
            if (id > 0)
            {
                _oRMClosingStock = _oRMClosingStock.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oRMClosingStock.RMClosingStockDetails = RMClosingStockDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }

            string sSQL = "SELECT * FROM AccountingSession WHERE ParentSessionID=1 AND YearStatus IN (1,2)";
            ViewBag.AccountingSessions = AccountingSession.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountType=5 AND HH.ParentHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType=" + ((int)EnumCISSetup.Inventory_Head).ToString() + ") ORDER BY HH.AccountHeadName";
            //ViewBag.AccountHeads = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oRMClosingStock);
        }

        [HttpPost]
        public JsonResult Save(RMClosingStock oRMClosingStock)
        {
            _oRMClosingStock = new RMClosingStock();
            try
            {
                _oRMClosingStock = oRMClosingStock;
                _oRMClosingStock = _oRMClosingStock.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMClosingStock.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMClosingStock);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Approve(RMClosingStock oRMClosingStock)
        {
            _oRMClosingStock = new RMClosingStock();
            try
            {
                _oRMClosingStock = oRMClosingStock;
                _oRMClosingStock = _oRMClosingStock.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMClosingStock.ErrorMessage = ex.Message;
            }
            _oRMClosingStock=_oRMClosingStock.Get(oRMClosingStock.RMClosingStockID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMClosingStock);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApprove(RMClosingStock oRMClosingStock)
        {
            _oRMClosingStock = new RMClosingStock();
            try
            {
                _oRMClosingStock = oRMClosingStock;
                _oRMClosingStock = _oRMClosingStock.UndoApprove((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMClosingStock.ErrorMessage = ex.Message;
            }
            _oRMClosingStock = _oRMClosingStock.Get(oRMClosingStock.RMClosingStockID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMClosingStock);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            RMClosingStock oRMClosingStock = new RMClosingStock();
            try
            {
                sErrorMease = oRMClosingStock.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Searching
      
        [HttpPost]
        public JsonResult Get(RMClosingStock oRMClosingStock)
        {
            _oRMClosingStock = new RMClosingStock();
            try
            {
                if (oRMClosingStock.RMClosingStockID <= 0) { throw new Exception("Please select a valid RMClosingStock."); }
                _oRMClosingStock = _oRMClosingStock.Get(oRMClosingStock.RMClosingStockID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRMClosingStock.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMClosingStock);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public string RMClosingStockSearchByName(string Name)
        {
            List<RMClosingStock> oRMClosingStocks = new List<RMClosingStock>();
            oRMClosingStocks = RMClosingStock.GetsByName(Name, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMClosingStocks);
            return sjson;
        }

        #endregion






    }
}
