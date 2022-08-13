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
	public class S2SLotTransferController : Controller
	{
		#region Declaration

		S2SLotTransfer _oS2SLotTransfer = new S2SLotTransfer();
		List<S2SLotTransfer> _oS2SLotTransfers = new  List<S2SLotTransfer>();
        TechnicalSheet _oTechnicalSheet = new TechnicalSheet();
        List<TechnicalSheet>  _oTechnicalSheets = new List<TechnicalSheet>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewS2SLotTransfers(int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.S2SLotTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oS2SLotTransfers = new List<S2SLotTransfer>();
            string sSQL = "SELECT * FROM View_S2SLotTransfer WHERE BUID = "+buid+" AND ISNULL(AuthorizedByID,0)=0";
			_oS2SLotTransfers = S2SLotTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
			return View(_oS2SLotTransfers);
		}

		public ActionResult ViewS2SLotTransfer(int id)
		{
			_oS2SLotTransfer = new S2SLotTransfer();
			if (id > 0)
			{
				_oS2SLotTransfer = _oS2SLotTransfer.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oS2SLotTransfer);
		}

		[HttpPost]
		public JsonResult Save(S2SLotTransfer oS2SLotTransfer)
		{
			_oS2SLotTransfer = new S2SLotTransfer();
			try
			{
				_oS2SLotTransfer = oS2SLotTransfer;
				_oS2SLotTransfer = _oS2SLotTransfer.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oS2SLotTransfer = new S2SLotTransfer();
				_oS2SLotTransfer.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oS2SLotTransfer);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        
        [HttpPost]
        public JsonResult Commit(S2SLotTransfer oS2SLotTransfer)
		{
			_oS2SLotTransfer = new S2SLotTransfer();
			try
			{
				_oS2SLotTransfer = oS2SLotTransfer;
				_oS2SLotTransfer = _oS2SLotTransfer.Commit((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oS2SLotTransfer = new S2SLotTransfer();
				_oS2SLotTransfer.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oS2SLotTransfer);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult StyleSearch(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheet = new TechnicalSheet();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                _oTechnicalSheets = new List<TechnicalSheet>();
                string sSQL = "SELECT * FROM View_TechnicalSheet WHERE TechnicalSheetID IN (SELECT StyleID FROM Lot WHERE WorkingUnitID = "+oTechnicalSheet.WorkingUnitID+" AND ISNULL(Balance,0)>0 ) ";
                if (!string.IsNullOrEmpty(oTechnicalSheet.StyleNo))
                {
                    sSQL += " AND StyleNo LIKE  '%" + oTechnicalSheet.StyleNo + "%'";
                }
                if (oTechnicalSheet.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oTechnicalSheet.BUID;
                }
                _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
                _oTechnicalSheets.Add(_oTechnicalSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StyleSearchProductWise(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheet = new TechnicalSheet();
            try
            {
                _oTechnicalSheets = new List<TechnicalSheet>();
                string sSQL = "SELECT * FROM View_TechnicalSheet WHERE BUID ="+oTechnicalSheet.BUID;
                if (!string.IsNullOrEmpty(oTechnicalSheet.StyleNo))
                {
                    sSQL += " AND StyleNo LIKE  '%" + oTechnicalSheet.StyleNo + "%'";
                }
                if (oTechnicalSheet.ProductID > 0)
                {
                    sSQL += " AND TechnicalSheetID IN ( SELECT TechnicalSheetID FROM BillOfMaterial WHERE ProductID = " + oTechnicalSheet.ProductID + ")";
                }
                if (oTechnicalSheet.TechnicalSheetID > 0)
                {
                    sSQL += " AND TechnicalSheetID !=" + oTechnicalSheet.TechnicalSheetID;
                }
               _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
                _oTechnicalSheets.Add(_oTechnicalSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //
        [HttpPost]
        public JsonResult Gets(S2SLotTransfer oS2SLotTransfer)
        {
            try
            {
                _oS2SLotTransfers = new List<S2SLotTransfer>();
                string sSQL = "SELECT * FROM View_S2SLotTransfer WHERE BUID =" + oS2SLotTransfer.BUID;
                if (!string.IsNullOrEmpty(oS2SLotTransfer.IssueStyleNo))
                {
                    sSQL += " AND IssueStyleNo LIKE  '%" + oS2SLotTransfer.IssueStyleNo + "%'";
                }
                else if (!string.IsNullOrEmpty(oS2SLotTransfer.ReceiveStyleNo))
                {
                    sSQL += " AND ReceiveStyleNo LIKE  '%" + oS2SLotTransfer.ReceiveStyleNo + "%'";
                }
                _oS2SLotTransfers = S2SLotTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oS2SLotTransfer = new  S2SLotTransfer();
                _oS2SLotTransfer.ErrorMessage = ex.Message;
                _oS2SLotTransfers.Add(_oS2SLotTransfer);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oS2SLotTransfers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				S2SLotTransfer oS2SLotTransfer = new S2SLotTransfer();
				sFeedBackMessage = oS2SLotTransfer.Delete(id, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 


		#endregion

	}

}
