using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class RouteSheetCombineController : Controller
    {
        #region Declaration
        RouteSheetCombine _oRouteSheetCombine = new RouteSheetCombine();
        RouteSheetCombineDetail _oRouteSheetCombineDetail = new RouteSheetCombineDetail();
        List<RouteSheetCombine> _oRouteSheetCombines = new List<RouteSheetCombine>();
        List<RouteSheetCombineDetail> _oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
        string _sErrorMessage = "";
        #endregion

        #region Action
        public ActionResult ViewRouteSheetCombines(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oRouteSheetCombines = new List<RouteSheetCombine>();
            _oRouteSheetCombines = RouteSheetCombine.Gets("SELECT * FROM View_RouteSheetCombine as CB  where Isnull(CB.ApproveBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);
            //where  isnull(AprovedByID,0)=0 and buid="+buid
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
        
            ViewBag.BUID = buid;
            return View(_oRouteSheetCombines);
        }
        public ActionResult ViewRouteSheetCombine(int nId, double ts)
        {
            _oRouteSheetCombine = new RouteSheetCombine();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            string sSQL = "";
            string sOrderNo = "";
            var IsOut = false;
            if (nId > 0)
            {
                _oRouteSheetCombine = _oRouteSheetCombine.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oRouteSheetCombine.RouteSheetCombineID > 0)
                {
                    _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (RouteSheetCombineDetail oItem in _oRouteSheetCombine.RouteSheetCombineDetails)
                    {
                        if (sOrderNo != oItem.RouteSheetNo)
                        {
                            _oRouteSheetCombine.RouteSheetNo = _oRouteSheetCombine.RouteSheetNo = oItem.RouteSheetNo;
                        }
                        sOrderNo = oItem.RouteSheetNo;
                    }
                }
            }
            ///Gets RS Detail
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            if (_oRouteSheetCombine.RouteSheetCombineDetails!=null)
            {
                if (_oRouteSheetCombine.RouteSheetCombineDetails.Count>0)
                {
                    oRouteSheetDetails = RouteSheetDetail.Gets(_oRouteSheetCombine.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    IsOut = (oRouteSheetDetails.Where(x => x.TotalQtyLotID > 0).Count() > 0) ? true : false;
                    _oRouteSheetCombine.RouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                }
            }

            List<Location> oLocations = new List<Location>();
            sSQL = "SELECT * FROM View_Location where ParentID !=0 and IsActive =1 and LocationID in (Select LocationID from view_WorkingUnit where IsActive=1 and IsStore=1 and BUID=1 and UnitType=" + (short)EnumWoringUnitType.Raw + ")";
            oLocations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            sSQL = "Select * from View_WorkingUnit Where IsActive=1 And IsStore=1 And UnitType=" + (short)EnumWoringUnitType.Raw + " and BUID=" + (short)EnumBusinessUnitType.Dyeing;
            oWorkingUnits = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.Locations = oLocations;
            if (_oRouteSheetCombine.RouteSheetID > 0)
            {
                oRouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheetCombine.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetDOs = oRouteSheetDOs;
            ViewBag.RouteSheetDO = oRouteSheetDO;
            
            return View(_oRouteSheetCombine);
        }
        public ActionResult ViewRouteSheetCombinesPS(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oRouteSheetCombines = new List<RouteSheetCombine>();
            _oRouteSheetCombines = RouteSheetCombine.Gets("SELECT * FROM View_RouteSheetCombine as CB  where Isnull(CB.ApproveBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);
            //where  isnull(AprovedByID,0)=0 and buid="+buid
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.BUID = buid;
            return View(_oRouteSheetCombines);
        }
        public ActionResult ViewRouteSheetCombinePS(int nId, double ts)
        {
            _oRouteSheetCombine = new RouteSheetCombine();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            string sSQL = "";
            string sOrderNo = "";
            var IsOut = false;
            if (nId > 0)
            {
                _oRouteSheetCombine = _oRouteSheetCombine.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oRouteSheetCombine.RouteSheetCombineID > 0)
                {
                    _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (RouteSheetCombineDetail oItem in _oRouteSheetCombine.RouteSheetCombineDetails)
                    {
                        if (sOrderNo != oItem.RouteSheetNo)
                        {
                            _oRouteSheetCombine.RouteSheetNo = _oRouteSheetCombine.RouteSheetNo = oItem.RouteSheetNo;
                        }
                        sOrderNo = oItem.RouteSheetNo;
                    }
                }
            }
            ///Gets RS Detail
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            if (_oRouteSheetCombine.RouteSheetCombineDetails != null)
            {
                if (_oRouteSheetCombine.RouteSheetCombineDetails.Count > 0)
                {
                    oRouteSheetDetails = RouteSheetDetail.Gets(_oRouteSheetCombine.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    IsOut = (oRouteSheetDetails.Where(x => x.TotalQtyLotID > 0).Count() > 0) ? true : false;
                    _oRouteSheetCombine.RouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                }
            }

            List<Location> oLocations = new List<Location>();
            sSQL = "SELECT * FROM View_Location where ParentID !=0 and IsActive =1 and LocationID in (Select LocationID from view_WorkingUnit where IsActive=1 and IsStore=1 and BUID=1 and UnitType=" + (short)EnumWoringUnitType.Raw + ")";
            oLocations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            sSQL = "Select * from View_WorkingUnit Where IsActive=1 And IsStore=1 And UnitType=" + (short)EnumWoringUnitType.Raw + " and BUID=" + (short)EnumBusinessUnitType.Dyeing;
            oWorkingUnits = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.Locations = oLocations;
            if (_oRouteSheetCombine.RouteSheetID > 0)
            {
                oRouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheetCombine.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetDOs = oRouteSheetDOs;
            ViewBag.RouteSheetDO = oRouteSheetDO;
            ViewBag.ProductTypes = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Dyes || x.id == (int)EnumProductNature.Chemical).ToList();
            return View(_oRouteSheetCombine);
        }

        private bool ValidateInput(RouteSheetCombine oRouteSheetCombine)
        {
            if (oRouteSheetCombine.RouteSheetCombineDetails.Count <= 0)
            {
                _sErrorMessage = "Please pick order";
                return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Save(RouteSheetCombine oRouteSheetCombine)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            try
            {
                if (this.ValidateInput(oRouteSheetCombine))
                {
                    _oRouteSheetCombine = oRouteSheetCombine.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oRouteSheetCombine.RouteSheetCombineID > 0)
                    {
                        _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(_oRouteSheetCombine.RouteSheetCombineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetDetails = RouteSheetDetail.Gets(_oRouteSheetCombine.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //IsOut = (oRouteSheetDetails.Where(x => x.TotalQtyLotID > 0).Count() > 0) ? true : false;
                        _oRouteSheetCombine.RouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                    }
                }
                else
                {
                    _oRouteSheetCombine.ErrorMessage = _sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oRouteSheetCombine = new RouteSheetCombine();
                _oRouteSheetCombine.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetCombine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approve(RouteSheetCombine oRouteSheetCombine)
        {
            try
            {
                if (this.ValidateInput(oRouteSheetCombine))
                {
                    _oRouteSheetCombine = oRouteSheetCombine.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oRouteSheetCombine.RouteSheetCombineID > 0)
                    {
                        _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(_oRouteSheetCombine.RouteSheetCombineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _oRouteSheetCombine.ErrorMessage = _sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oRouteSheetCombine = new RouteSheetCombine();
                _oRouteSheetCombine.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetCombine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UndoApprove(RouteSheetCombine oRouteSheetCombine)
        {
            try
            {
                if (this.ValidateInput(oRouteSheetCombine))
                {
                    _oRouteSheetCombine = oRouteSheetCombine.UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oRouteSheetCombine.RouteSheetCombineID > 0)
                    {
                        _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(_oRouteSheetCombine.RouteSheetCombineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _oRouteSheetCombine.ErrorMessage = _sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oRouteSheetCombine = new RouteSheetCombine();
                _oRouteSheetCombine.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetCombine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



    

        [HttpPost]
        public JsonResult Delete(RouteSheetCombine oRouteSheetCombine)
        {
            try
            {
                if (oRouteSheetCombine.RouteSheetCombineID <= 0) { throw new Exception("Please select an valid item."); }
                oRouteSheetCombine.ErrorMessage = oRouteSheetCombine.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetCombine = new RouteSheetCombine();
                oRouteSheetCombine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetCombine.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(RouteSheetCombineDetail oRouteSheetCombineDetail)
        {
            try
            {
                if (oRouteSheetCombineDetail.RouteSheetCombineDetailID > 0)
                {
                    oRouteSheetCombineDetail.ErrorMessage = oRouteSheetCombineDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oRouteSheetCombineDetail = new RouteSheetCombineDetail();
                oRouteSheetCombineDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetCombineDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Gets
        [HttpPost]
        public JsonResult GetRoutesheets(RouteSheet oRouteSheet)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            _oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
            try
            {
                string sSQL = "Select top(100)* from View_RouteSheet ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oRouteSheet.RouteSheetNo))
                {
                    oRouteSheet.RouteSheetNo = oRouteSheet.RouteSheetNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RouteSheetNo Like'%" + oRouteSheet.RouteSheetNo + "%'";
                }
                if (!String.IsNullOrEmpty(oRouteSheet.ErrorMessage))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RouteSheetID not in ( "+ oRouteSheet.ErrorMessage+")";
                }
                if (oRouteSheet.LabDipDetailID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RouteSheetID in (Select view_RouteSheetDO.RouteSheetID from view_RouteSheetDO where view_RouteSheetDO.LabDipDetailID=" + oRouteSheet.LabDipDetailID + ")";
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RSState<"+(int)EnumRSState.YarnOut;
                sSQL = sSQL + "" + sReturn + " Order by RouteSheetID DESC ";
                oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (RouteSheet oItem in oRouteSheets)
                {
                    _oRouteSheetCombineDetail = new RouteSheetCombineDetail();
                    _oRouteSheetCombineDetail.OrderNo = oItem.OrderNo;
                    _oRouteSheetCombineDetail.RouteSheetNo = oItem.RouteSheetNo;
                    _oRouteSheetCombineDetail.RouteSheetID = oItem.RouteSheetID;
                    _oRouteSheetCombineDetail.RouteSheetDate = oItem.RouteSheetDate;
                    _oRouteSheetCombineDetail.Qty = oItem.Qty;
                    _oRouteSheetCombineDetail.TtlLiquire = oItem.TtlLiquire;
                    _oRouteSheetCombineDetail.TtlCotton = oItem.TtlCotton;
                    _oRouteSheetCombineDetail.WorkingUnitID = oItem.WorkingUnitID;
                    _oRouteSheetCombineDetail.MachineName = oItem.MachineName;
                    _oRouteSheetCombineDetail.ProductName = oItem.ProductName;
                    _oRouteSheetCombineDetail.ProductName_Raw = oItem.ProductName_Raw;
                    _oRouteSheetCombineDetail.RSState = oItem.RSState;
                    _oRouteSheetCombineDetail.LotNo = oItem.LotNo;
                    //_oRouteSheetCombineDetail.LotNo = oItem.LotNo;
                    _oRouteSheetCombineDetails.Add(_oRouteSheetCombineDetail);
                }
            }
            catch (Exception ex)
            {
                _oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetCombineDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLabDipDetail(LabDipDetail oLabDipDetail)
        {
            List<LabDipReport> oLabDipReports = new List<LabDipReport>();
            try
            {
                string sSQL = "";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oLabDipDetail.LabdipNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.LabdipNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ColorNo!='' and LabdipNo Like'%" + oLabDipDetail.LabdipNo + "%'";
                }
                if (!String.IsNullOrEmpty(oLabDipDetail.ColorNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.ColorNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ColorNo!='' and ColorNo Like'%" + oLabDipDetail.ColorNo + "%'";
                }

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LabDipDetailID in (Select view_RouteSheetDO.LabDipDetailID from view_RouteSheetDO where RouteSheetID in (Select RouteSheet.RouteSheetID from RouteSheet where RSState<"+(int)EnumRSState.YarnOut+"))";

                sSQL = sSQL + "" + sReturn;
                oLabDipReports = LabDipReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipReports = new List<LabDipReport>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult GetRouteSheetDOs(RouteSheet oRouteSheet)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            try
            {
                string sSQL = "SELECT  * FROM View_RouteSheetDO ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oRouteSheet.Params))
                {
                    oRouteSheet.RouteSheetNo = oRouteSheet.RouteSheetNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RouteSheetID IN (" + oRouteSheet.Params + ")";
                }
               
                //Global.TagSQL(ref sReturn);
                //sReturn = sReturn + " RSState<" + (int)EnumRSState.YarnOut;
                sSQL = sSQL + "" + sReturn + " Order by RouteSheetID DESC ";
                oRouteSheetDOs = RouteSheetDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                RouteSheetDO oRouteSheetDO  =new  RouteSheetDO();
                oRouteSheetDO.ErrorMessage = ex.Message;
                oRouteSheetDOs.Add(oRouteSheetDO);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult SaveRouteSheetDO(RouteSheet oRouteSheet)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            oRouteSheetDOs = oRouteSheet.RouteSheetDOs;
            try
            {
                oRouteSheet = oRouteSheet.SaveRouteSheetDO(((User)Session[SessionInfo.CurrentUser]).UserID);
                if(string.IsNullOrEmpty(oRouteSheet.ErrorMessage))
                {
                string sSQL = "Select * from View_RouteSheet WHERE  RouteSheetID IN (" + string.Join(",", oRouteSheetDOs.Select(x => x.RouteSheetID)).ToString() + ") Order by RouteSheetID DESC ";
                oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRouteSheets.Add(oRouteSheet);
                }

                foreach (RouteSheet oItem in oRouteSheets)
                {
                    _oRouteSheetCombineDetail = new RouteSheetCombineDetail();
                    _oRouteSheetCombineDetail.OrderNo = oItem.OrderNo;
                    _oRouteSheetCombineDetail.RouteSheetNo = oItem.RouteSheetNo;
                    _oRouteSheetCombineDetail.RouteSheetID = oItem.RouteSheetID;
                    _oRouteSheetCombineDetail.RouteSheetDate = oItem.RouteSheetDate;
                    _oRouteSheetCombineDetail.Qty = oItem.Qty;
                    _oRouteSheetCombineDetail.TtlLiquire = oItem.TtlLiquire;
                    _oRouteSheetCombineDetail.TtlCotton = oItem.TtlCotton;
                    _oRouteSheetCombineDetail.WorkingUnitID = oItem.WorkingUnitID;
                    _oRouteSheetCombineDetail.MachineName = oItem.MachineName;
                    _oRouteSheetCombineDetail.ProductName = oItem.ProductName;
                    _oRouteSheetCombineDetail.ProductName_Raw = oItem.ProductName_Raw;
                    _oRouteSheetCombineDetail.RSState = oItem.RSState;
                    _oRouteSheetCombineDetail.LotNo = oItem.LotNo;
                    _oRouteSheetCombineDetail.ErrorMessage = oItem.ErrorMessage;
                    _oRouteSheetCombineDetails.Add(_oRouteSheetCombineDetail);
                }

            }
            catch (Exception ex)
            {
                _oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetCombineDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search
        [HttpPost]
        public JsonResult AdvSearch(RouteSheetCombine oRouteSheetCombine)
        {
            _oRouteSheetCombines = new List<RouteSheetCombine>();
            try
            {
                string sSQL = MakeSQL(oRouteSheetCombine);
                _oRouteSheetCombines = RouteSheetCombine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetCombines = new List<RouteSheetCombine>();
                _oRouteSheetCombine.ErrorMessage = ex.Message;
                _oRouteSheetCombines.Add(_oRouteSheetCombine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetCombines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(RouteSheetCombine oRouteSheetCombine)
        {
            string sParams = oRouteSheetCombine.Note;
            string _sContractorIds = "";
            int nCboDate = 0;
            DateTime dFromDate = DateTime.Today;
            DateTime dToDate = DateTime.Today;
            string sRSNo = "";
            string sOrderNo = "";
            string sComboRSNo = "";
            int nBUID = 0;



            if (!string.IsNullOrEmpty(sParams))
            {
                nCboDate = Convert.ToInt32(sParams.Split('~')[1]);
                if (nCboDate > 0)
                {
                    dFromDate = Convert.ToDateTime(sParams.Split('~')[2]);
                    dToDate = Convert.ToDateTime(sParams.Split('~')[3]);
                }
                sRSNo = Convert.ToString(sParams.Split('~')[4]);
                sOrderNo = Convert.ToString(sParams.Split('~')[5]);
                sComboRSNo = Convert.ToString(sParams.Split('~')[6]);
              
            }

            string sReturn1 = "SELECT * FROM View_RouteSheetCombine  ";
            string sReturn = "";

            #region Date
            if (nCboDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CombineRSDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CombineRSDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CombineRSDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CombineRSDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CombineRSDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CombineRSDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region RouteSheetNo
            if (!string.IsNullOrEmpty(sRSNo))
            {
                sRSNo = sRSNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetCombineID in (Select RouteSheetCombineID from View_RouteSheetCombineDetail where RouteSheetNo like '%" + sRSNo + "%')";
            }
            #endregion
            #region OrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                sOrderNo = sOrderNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetCombineID in (Select RouteSheetCombineID from View_RouteSheetCombineDetail where OrderNo like '%" + sOrderNo + "%')";
            }
            #endregion
            #region sComboRSNo
            if (!string.IsNullOrEmpty(sComboRSNo))
            {
                sComboRSNo = sComboRSNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RSNo_Combine like '%" + sComboRSNo + "%'";
            }
            #endregion
         
            string sSQL = sReturn1 + " " + sReturn ;
            return sSQL;
        }
    
        #endregion
        #region GetCompanyLogo
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Add Templet for Combine RS
        #region Tree Generator


        private RouteSheetDetail MakeTree(List<RouteSheetDetail> oRouteSheetDetails)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            if (oRouteSheetDetails.Count() > 0)
            {
                oRouteSheetDetail = oRouteSheetDetails.Where(x => x.ParentID == 0).ElementAtOrDefault(0);
                this.AddChildNode(ref oRouteSheetDetail, oRouteSheetDetails.Where(x => x.RouteSheetDetailID != oRouteSheetDetail.RouteSheetDetailID).ToList());
            }
            return oRouteSheetDetail;
        }

        public void AddChildNode(ref RouteSheetDetail oRouteSheetDetail, List<RouteSheetDetail> oRouteSheetDetails)
        {
            int nParentID = oRouteSheetDetail.RouteSheetDetailID;
            oRouteSheetDetail.children = oRouteSheetDetails.Where(x => x.ParentID == nParentID).ToList();
            if (oRouteSheetDetail.children.Count() > 0 && oRouteSheetDetails.Count() > 0)
            {
                oRouteSheetDetails.RemoveAll(x => x.ParentID == nParentID);
                foreach (RouteSheetDetail OItem in oRouteSheetDetail.children)
                {
                    RouteSheetDetail oTempNode = OItem;
                    this.AddChildNode(ref oTempNode, oRouteSheetDetails);
                }
            }

        }


        #endregion
        [HttpPost]
        public JsonResult IUDTemplate(RouteSheetCombine oRouteSheetCombine)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            try
            {
                int nDSID = 0;
                int.TryParse(oRouteSheetCombine.Params, out nDSID);
                if (nDSID <= 0) throw new Exception("Please select a valid dyeing solution.");

                _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(oRouteSheetCombine.RouteSheetCombineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (RouteSheetCombineDetail oRSC in _oRouteSheetCombine.RouteSheetCombineDetails)
                {
                    oRouteSheetDetails = RouteSheetDetail.IUDTemplate(nDSID, oRSC.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID > 0)
                {
                    oRouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                }
                else
                {
                    if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID <= 0)
                    {
                        throw new Exception(oRouteSheetDetails.FirstOrDefault().ErrorMessage);
                    }
                    else
                    {
                        throw new Exception("Unable to add templete.");
                    }

                }

            }
            catch (Exception ex)
            {
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult IUDTemplateeCopyFromRS(RouteSheetCombine oRouteSheetCombine)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            try
            {
                int nRSID_CopyFrom = 0;
                int.TryParse(oRouteSheetCombine.Params, out nRSID_CopyFrom);
                if (nRSID_CopyFrom <= 0) throw new Exception("Please select a valid dyeing solution.");

                _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(oRouteSheetCombine.RouteSheetCombineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (RouteSheetCombineDetail oRSC in _oRouteSheetCombine.RouteSheetCombineDetails)
                {
                    oRouteSheetDetails = RouteSheetDetail.IUDTemplateCopyFromRS(nRSID_CopyFrom, oRSC.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID > 0)
                {
                    oRouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                }
                else
                {
                    if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID <= 0)
                    {
                        throw new Exception(oRouteSheetDetails.FirstOrDefault().ErrorMessage);
                    }
                    else
                    {
                        throw new Exception("Unable to add templete.");
                    }

                }

            }
            catch (Exception ex)
            {
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateDetail(RouteSheetCombine oRouteSheetCombine)
        {
            oRouteSheetCombine = oRouteSheetCombine.Get(oRouteSheetCombine.RouteSheetCombineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();

            try
            {
                    if (oRouteSheetCombine.RouteSheetID<=0)
                    {
                        throw new Exception("Unable to add templete.");
                    }

                oRouteSheetDetails = RouteSheetDetail.Gets(oRouteSheetCombine.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRouteSheetDetails = RouteSheetDetail.Update(oRouteSheetDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(oRouteSheetCombine.RouteSheetCombineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oRouteSheetCombine.RouteSheetCombineDetails != null)
                {
                    foreach (RouteSheetCombineDetail oRSC in _oRouteSheetCombine.RouteSheetCombineDetails)
                    {
                        if (oRouteSheetCombine.RouteSheetID != oRSC.RouteSheetID)
                        {
                            RouteSheetDetail.IUDTemplateCopyFromRS(oRouteSheetCombine.RouteSheetID, oRSC.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
                if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID > 0)
                {
                    oRouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                }
                else
                {
                    if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID <= 0)
                    {
                        throw new Exception(oRouteSheetDetails.FirstOrDefault().ErrorMessage);
                    }
                    else
                    {
                        throw new Exception("Unable to add templete.");
                    }

                }
            }
            catch (Exception ex)
            {
                oRouteSheetDetail = new RouteSheetDetail();

                oRouteSheetDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}