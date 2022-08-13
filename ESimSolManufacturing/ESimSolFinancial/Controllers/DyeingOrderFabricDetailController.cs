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
	public class DyeingOrderFabricDetailController : Controller
	{
		#region Declaration

		DyeingOrderFabricDetail _oDyeingOrderFabricDetail = new DyeingOrderFabricDetail();
		List<DyeingOrderFabricDetail> _oDyeingOrderFabricDetails = new  List<DyeingOrderFabricDetail>();

        FabricLotAssign _oFabricLotAssign = new FabricLotAssign();
        List<FabricLotAssign> _oFabricLotAssigns = new List<FabricLotAssign>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewDyeingOrderFabricDetails(int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			_oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            _oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets("SELECT TOP (100)* FROM View_DyeingOrderFabricDetail ORDER BY FSCDetailID, SLNo", (int)Session[SessionInfo.currentUserID]);

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
			return View(_oDyeingOrderFabricDetails);
		}
		public ActionResult ViewDyeingOrderFabricDetail(int id)
		{
            FabricLotAssign oFabricLotAssign= new FabricLotAssign();
            FabricExecutionOrderSpecificationDetail oFEOSD = new FabricExecutionOrderSpecificationDetail();

            oFEOSD = FabricExecutionOrderSpecificationDetail.Get(id, (int)Session[SessionInfo.currentUserID]);
            oFabricLotAssign.FabricLotAssigns = FabricLotAssign.Gets("SELECT * FROM View_FabricLotAssign WHERE FEOSDID=" + id, (int)Session[SessionInfo.currentUserID]);

            oFabricLotAssign.ProductName = oFEOSD.ProductName;
            oFabricLotAssign.ProductCode = oFEOSD.ProductCode;
            oFabricLotAssign.Qty = oFEOSD.Qty;
            oFabricLotAssign.Qty_Order = oFabricLotAssign.FabricLotAssigns.Sum(x=>x.Qty);
         
            return View(oFabricLotAssign);
        }


        [HttpPost]
        public JsonResult GetDOFDetail(FabricLotAssign oFabricLotAssign)
        {
            _oFabricLotAssign = new FabricLotAssign();
            try
            {
                FabricExecutionOrderSpecificationDetail oFEOSD = new FabricExecutionOrderSpecificationDetail();

                _oFabricLotAssign = _oFabricLotAssign.Get(oFabricLotAssign.FEOSDID, (int)Session[SessionInfo.currentUserID]);
                _oFabricLotAssign.FabricLotAssigns = FabricLotAssign.Gets("SELECT * FROM View_FabricLotAssign WHERE FEOSDID=" + oFabricLotAssign.FEOSDID, (int)Session[SessionInfo.currentUserID]);

                oFEOSD = FabricExecutionOrderSpecificationDetail.Get(oFabricLotAssign.FEOSDID, (int)Session[SessionInfo.currentUserID]);
                _oFabricLotAssign.FEOSDID = oFEOSD.FEOSDID;
                _oFabricLotAssign.ProductName = oFEOSD.ProductName;
                _oFabricLotAssign.ProductCode = oFEOSD.ProductCode;
                _oFabricLotAssign.Qty = oFEOSD.Qty;
                _oFabricLotAssign.Qty_Order = oFabricLotAssign.FabricLotAssigns.Sum(x => x.Qty);
            }
            catch (Exception ex)
            {
                _oFabricLotAssign = new FabricLotAssign();
                _oFabricLotAssign.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLotAssign);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetDOFDetail_For_Multiple(FabricLotAssign oFabricLotAssign)
        {
            _oFabricLotAssign = new FabricLotAssign();
            try
            {
                FabricExecutionOrderSpecificationDetail oFEOSD = new FabricExecutionOrderSpecificationDetail();

                _oFabricLotAssign = _oFabricLotAssign.Get(oFabricLotAssign.FEOSDID, (int)Session[SessionInfo.currentUserID]);
                _oFabricLotAssign.FabricLotAssigns = FabricLotAssign.Gets("SELECT * FROM View_FabricLotAssign WHERE FEOSDID IN (" + oFabricLotAssign.Params +")", (int)Session[SessionInfo.currentUserID]);

                oFEOSD = FabricExecutionOrderSpecificationDetail.Get(oFabricLotAssign.FEOSDID, (int)Session[SessionInfo.currentUserID]);
                _oFabricLotAssign.FEOSDID = oFEOSD.FEOSDID;
                _oFabricLotAssign.ProductName = oFEOSD.ProductName;
                _oFabricLotAssign.ProductCode = oFEOSD.ProductCode;
                _oFabricLotAssign.Qty = oFEOSD.Qty;
                _oFabricLotAssign.Qty_Order = oFabricLotAssign.FabricLotAssigns.Sum(x => x.Qty);
            }
            catch (Exception ex)
            {
                _oFabricLotAssign = new FabricLotAssign();
                _oFabricLotAssign.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLotAssign);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Gets_Lot(LotParent oLotParent)
        {
            List<Lot> oLots = new List<Lot>();
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<LotParent> oLotParents = new List<LotParent>();
            try
            {
                    #region Lot For Inhouse
                oIssueStores = WorkingUnit.GetsPermittedStore(oLotParent.BUID, EnumModuleName.DyeingOrder, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                    if (oIssueStores.Any()) 
                    {
                        oLots = Lot.Gets("SELECT top(100)* FROM View_Lot AS Lot WHERE ParentType !=" + (int)EnumTriggerParentsType.DUProGuideLineDetail + " and Lot.WorkingUnitID IN (" + string.Join(",", oIssueStores.Select(x => x.WorkingUnitID)) + ") AND Lot.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE BU.BusinessUnitType= " + (int)EnumBusinessUnitType.Dyeing + ") AND ISNULL(Balance,0)>0 " + (string.IsNullOrEmpty(oLotParent.LotNo) ? "" : " AND LotNo Like '%" + oLotParent.LotNo + "%' "), ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oLots.ForEach(x => oLotParents.Add(new LotParent
                        {
                            LotID = x.LotID,
                            LotNo = x.LotNo,
                            ProductID = oLotParent.ProductID,
                            ProductName = x.ProductName,
                            StoreName = x.OperationUnitName,
                            DyeingOrderID = oLotParent.DyeingOrderID,
                            Qty = x.Balance,
                            Balance = x.Balance,
                            UnitPrice = x.UnitPrice
                        }));
                    }
                    #endregion
            }
            catch (Exception e)
            {
                oLotParents.Add(new LotParent() { ErrorMessage = e.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLotParents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region Fabric Lot Assign

        [HttpPost]
        public JsonResult Save_FLA(FabricLotAssign oFabricLotAssign)
        {
            _oFabricLotAssign = new FabricLotAssign();
            try
            {
                _oFabricLotAssign = oFabricLotAssign;
                _oFabricLotAssign = _oFabricLotAssign.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricLotAssign = new FabricLotAssign();
                _oFabricLotAssign.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLotAssign);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save_Multiple_FLA(List<FabricLotAssign> oFabricLotAssigns)
        {
            _oFabricLotAssign = new FabricLotAssign();
            try
            {
                _oFabricLotAssign = _oFabricLotAssign.Save(oFabricLotAssigns, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricLotAssign = new FabricLotAssign();
                _oFabricLotAssign.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLotAssign);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_FLA(FabricLotAssign oFabricLotAssign)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricLotAssign.Delete(oFabricLotAssign.FabricLotAssignID, (int)Session[SessionInfo.currentUserID]);
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

        #region Yarn Out Muliple (Advance Search)

        public JsonResult AdvSearch(DyeingOrderFabricDetail oDyeingOrderFabricDetail)
        {
            _oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            try
            {
                string sSQL = GetSQL_Adavnce(oDyeingOrderFabricDetail);
                _oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets(sSQL + " ORDER BY FSCDetailID, SLNo", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception e)
            {
                _oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
                _oDyeingOrderFabricDetails.Add(new DyeingOrderFabricDetail() { ErrorMessage = e.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(_oDyeingOrderFabricDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL_Adavnce(DyeingOrderFabricDetail oDyeingOrderFabricDetail)
        {
            //PI Date
            int cboDate = 0;
            DateTime StartTime = DateTime.MinValue;
            DateTime EndTime = DateTime.MinValue;

            if (!string.IsNullOrEmpty(oDyeingOrderFabricDetail.SearchStringDate))
            {
                cboDate = Convert.ToInt32(oDyeingOrderFabricDetail.SearchStringDate.Split('~')[0]);
                StartTime = Convert.ToDateTime(oDyeingOrderFabricDetail.SearchStringDate.Split('~')[1]);
                EndTime = Convert.ToDateTime(oDyeingOrderFabricDetail.SearchStringDate.Split('~')[2]);
            }

            string sReturn1 = "Select top(500)* from View_DyeingOrderFabricDetail ";
            string sReturn = "";

            #region Date
            if (cboDate > 0)
            {
                Global.TagSQL(ref sReturn);

                //Select ForwardDODate,FEOSID from FabricExecutionOrderSpecification

                string sDateQuery = "";
                DateObject.CompareDateQuery(ref sDateQuery, "ForwardDODate", cboDate, StartTime, EndTime);
                sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecification " + sDateQuery + ")";
            }
            #endregion

            #region Yet To Lot Assign
            if (oDyeingOrderFabricDetail.YetToLotAssign)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(LotNo,'') = '' ";
            }
            #endregion

            #region Exe No
            if (!string.IsNullOrEmpty(oDyeingOrderFabricDetail.ExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ExeNo LIKE '%" + oDyeingOrderFabricDetail.ExeNo + "%'";
            }
            #endregion

            #region Lot No
            if (!string.IsNullOrEmpty(oDyeingOrderFabricDetail.LotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FEOSDID IN (SELECT FEOSDID FROM FabricLotAssign WHERE LotID IN (" + oDyeingOrderFabricDetail.LotNo + "))";
            }
            #endregion

            #region Buyer
            if (!string.IsNullOrEmpty(oDyeingOrderFabricDetail.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  BuyerID IN (" + oDyeingOrderFabricDetail.BuyerName + ")";
            }
            #endregion

            #region Customer
            if (!string.IsNullOrEmpty(oDyeingOrderFabricDetail.CustomerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ContractorID IN (" + oDyeingOrderFabricDetail.CustomerName + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn + "";
            return sReturn;
        }

        #endregion

	}

}
