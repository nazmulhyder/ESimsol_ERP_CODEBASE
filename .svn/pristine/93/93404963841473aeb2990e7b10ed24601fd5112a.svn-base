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
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class RouteSheetController : Controller
    {
        #region Declaration
        RouteSheet _oRouteSheet = new RouteSheet();
        RouteSheetDetail _oRouteSheetDetail = new RouteSheetDetail();
        List<RouteSheet> _oRouteSheets = new List<RouteSheet>();

        RSDetailAdditonal _oRSDetailAdditonal = new RSDetailAdditonal();
        List<RSDetailAdditonal> _oRSDetailAdditonals = new List<RSDetailAdditonal>();

        string _sErrorMessage = "";
        #endregion

        #region RouteSheet
        public ActionResult ViewRouteSheets(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RouteSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oRouteSheets = new List<RouteSheet>();
            string sSQL = "Select top(100)* from View_RouteSheet Where RSState=" + (int)EnumRSState.Initialized + " order by RouteSheetDate DESC";
            _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.EnumRSStatuss = Enum.GetValues(typeof(EnumRSState)).Cast<EnumRSState>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();
            //foreach (DUOrderSetup oItem in oDUOrderSetups)
            //{
            //    if (oItem.OrderType != (int)EnumOrderType.LabDipOrder)
            //    {
            //        oDUOrderSetupsTemp.Add(oItem);
            //    }
            //}
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();

            sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";

            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));
            return View(_oRouteSheets);
        }
        public ActionResult ViewRouteSheetsApprove(int nID, double ts)
        {
            _oRouteSheets = new List<RouteSheet>();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            string sSQL = "Select * from View_RouteSheet Where RSState<=" + (int)EnumRSState.InFloor + " and LotID=" + nID;
            _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumRSStatuss = Enum.GetValues(typeof(EnumRSState)).Cast<EnumRSState>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();
            ViewBag.OrderTypes = oDUOrderSetups;
          
            return View(_oRouteSheets);
        }
      
        private string GetOrderNos(List<RouteSheetDO> oRouteSheetDOs)
        {
            oRouteSheetDOs = oRouteSheetDOs.OrderBy(x => x.OrderNoFull).ToList();
            string sResult = "";
            string sOrderNo = "";
            foreach (RouteSheetDO oItem in oRouteSheetDOs)
            {
                if (sOrderNo != oItem.OrderNoFull)
                {
                    sResult = oItem.OrderNoFull + "," + sResult;
                }
                sOrderNo = oItem.OrderNoFull;
            }
            if (sResult != "")
            {
                sResult = sResult.Remove((sResult.Length - 1), 1);
            }
            return sResult;
        }
        public ActionResult ViewRouteSheet(int nId,int buid, double ts)
        {
            _oRouteSheet = new RouteSheet();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            var IsOut = false;
            int nOutState = 0;
            string sSQL = "";
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (nId > 0)
            {
                _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oRouteSheet.RouteSheetID > 0)
                {

                    _oRSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE ISNULL(SequenceNo,0) > 0 AND RouteSheetID =" + _oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
                    oRouteSheetDetails = RouteSheetDetail.Gets(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //nOutState = oRouteSheetDetails.Where(x => x.TotalQtyLotID > 0).Count();
                    //if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 0); }
                    nOutState = oRouteSheetDetails.Where(x => x.AddOneLotID > 0).Count() ;
                    if (nOutState>0) { oRouteSheetDetails.ForEach(o => o.OutState = 1); }
                    nOutState = oRouteSheetDetails.Where(x => x.AddTwoLotID > 0).Count();
                    if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 2); }
                    nOutState = oRouteSheetDetails.Where(x => x.AddThreeLotID > 0).Count();
                    if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 3); }

                    oRouteSheetDetails.ForEach(x =>
                    {
                        if (_oRSDetailAdditonals.FirstOrDefault() != null && _oRSDetailAdditonals.FirstOrDefault().RouteSheetDetailID > 0 && _oRSDetailAdditonals.Where(b => b.RouteSheetDetailID == x.RouteSheetDetailID ).Count() > 0)
                        {
                            x.AddOneQty = _oRSDetailAdditonals.Where(p => p.RouteSheetDetailID == x.RouteSheetDetailID && p.InOutType == (int)EnumInOutType.Disburse && p.SequenceNo == 1 && p.RouteSheetDetailID > 0).Sum(k => k.Qty);
                            x.AddTwoQty = _oRSDetailAdditonals.Where(p => p.RouteSheetDetailID == x.RouteSheetDetailID && p.InOutType == (int)EnumInOutType.Disburse && p.SequenceNo == 2 && p.RouteSheetDetailID > 0).Sum(k => k.Qty);
                            x.AddThreeQty = _oRSDetailAdditonals.Where(p => p.RouteSheetDetailID == x.RouteSheetDetailID && p.InOutType==(int)EnumInOutType.Disburse &&  p.SequenceNo>=3 && p.RouteSheetDetailID > 0).Sum(k => k.Qty);
                            x.ReturnQty = _oRSDetailAdditonals.Where(p => p.RouteSheetDetailID == x.RouteSheetDetailID && p.InOutType==(int)EnumInOutType.Receive && p.RouteSheetDetailID > 0).Sum(k => k.Qty);
                        }
                    });
                    _oRouteSheet.RouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                    if (_oRouteSheet.RouteSheetDOs.Count > 0)
                    {
                        _oRouteSheet.OrderNo = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.OrderNoFull).Distinct().ToList());// this.GetOrderNo(s_oRouteSheet.RouteSheetDOs);
                    }
                }
            }

            if (oDyeingTypes.Count > 0 && _oRouteSheet.HanksCone<=0)
            {
                _oRouteSheet.HanksCone = oDyeingTypes[0].DyeingTypeInt;
            }
             if (_oRouteSheet.TtlLiquire<=0)
            {
                _oRouteSheet.TtlLiquire = _oRouteSheet.Qty*6;
            }   
            
            ViewBag.IsOut = IsOut;
            
            List<Location> oLocations = new List<Location>();
            sSQL = "SELECT * FROM View_Location where ParentID !=0 and IsActive =1 and LocationID in (Select LocationID from view_WorkingUnit where IsActive=1 and IsStore=1 and BUID=" + buid + ")";
            oLocations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            //sSQL = "Select * from View_WorkingUnit Where IsActive=1 And UnitType=" + (short)EnumWoringUnitType.Raw + " and IsStore=1 And BUID in (Select BusinessUnit.BusinessUnitID from BusinessUnit where BusinessUnit.BusinessUnitType=" + (short)EnumBusinessUnitType.Dyeing + " )";
            //oWorkingUnits = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.Locations = oLocations;

            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheetDetail, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnitsDC = oWorkingUnits;

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            DUPSchedule oDUPSchedule = new DUPSchedule();
            if (_oRouteSheet.DUPScheduleID > 0)
            {
                oDUPSchedule = oDUPSchedule.Get(_oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.DUPSchedule = oDUPSchedule;
            ViewBag.BUID = buid;
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProductTypes = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Dyes || x.id == (int)EnumProductNature.Chemical).ToList();
            ViewBag.DyeingRecipeType = EnumObject.jGets(typeof(EnumDyeingRecipeType));
            return View(_oRouteSheet);
        }
        [HttpPost]
        public JsonResult GetMachineLabelByMachineID(MachineLiquor oMachineLiquor)
        {
            List<MachineLiquor> oMachineLiquors = new List<MachineLiquor>();
            string sSqL = "SELECT * FROM View_MachineLiquor Where MachineID = " + oMachineLiquor.MachineID + " Order By Label";
            oMachineLiquors = MachineLiquor.Gets(sSqL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachineLiquors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMachineLiquorByLabel(MachineLiquor oMachineLiquor)
        {
            List<MachineLiquor> oMachineLiquors = new List<MachineLiquor>();
            string sSqL = "SELECT * FROM View_MachineLiquor Where MachineLiquorID = " + oMachineLiquor.MachineLiquorID + " Order By MachineLiquorID";
            oMachineLiquors = MachineLiquor.Gets(sSqL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachineLiquors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RouteSheetEditSave(RouteSheet oRouteSheet)
        {
            RouteSheet _oRouteSheet = new RouteSheet();
            try
            {
                _oRouteSheet = oRouteSheet;
                _oRouteSheet = _oRouteSheet.RouteSheetEditSave(_oRouteSheet, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRouteSheet = new RouteSheet();
                _oRouteSheet.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private bool ValidateInput(RouteSheet oRouteSheet)
        {

            string sYear = "";
            string sNo = "";
            bool isNumeric;
            int i;
            string str = "";

            if (!string.IsNullOrEmpty(oRouteSheet.RouteSheetNo) )
            {
                if (oRouteSheet.RouteSheetNo != null)
                {
                    if (oRouteSheet.RouteSheetNo.Length < 4 && oRouteSheet.RouteSheetNo != null)
                    {
                        _sErrorMessage = "Please enter valid Order No";
                        return false;
                    }
                }
                if (oRouteSheet.RouteSheetNo != null)
                {
                    isNumeric = int.TryParse(oRouteSheet.RouteSheetNo, out i);
                    if (isNumeric)
                    {
                        _sErrorMessage = "Year and number both mendatory";
                        return false;
                    }
                    str = oRouteSheet.RouteSheetNo.Split('-')[1];
                    isNumeric = int.TryParse(str, out i);
                    if (!isNumeric)
                    {
                        _sErrorMessage = "Please enter Numeric Order No";
                        return false;
                    }
                    //str = oRouteSheet.RouteSheetNo.Split('-')[0];
                    //if (str != "16" && str != "17")
                    //{
                    //    _sErrorMessage = "Please enter valid Year";
                    //    return false;
                    //}
                }
            }

            return true;
        }

        [HttpPost]
        public JsonResult Save(RouteSheet oRouteSheet)
        {
            try
            {
                if (this.ValidateInput(oRouteSheet))
                {
                if (oRouteSheet.RouteSheetID <= 0)
                {
                    oRouteSheet = oRouteSheet.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRouteSheet = oRouteSheet.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                if (oRouteSheet.RouteSheetID>0)
                {
                    oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                }
                  else
                  {
                      oRouteSheet.ErrorMessage = _sErrorMessage;
                  }
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMachine(RouteSheet oRouteSheet)
        {
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) throw new Exception("Error: Invalid Dye Line!");
                if (oRouteSheet.MachineID <= 0) throw new Exception("Error: Invalid Dye Line Machine!");

                oRouteSheet = oRouteSheet.UpdateMachine((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(RouteSheet oRouteSheet)
        {
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) { throw new Exception("Please select an valid item."); }
                oRouteSheet = oRouteSheet.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IUDTemplate(RouteSheet oRouteSheet)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            try
            {
                int nDSID = 0;
                int.TryParse(oRouteSheet.Params, out nDSID);
                if (nDSID <= 0) throw new Exception("Please select a valid dyeing solution.");

                oRouteSheetDetails = RouteSheetDetail.IUDTemplate(nDSID, oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveDetail(RouteSheetDetail oRouteSheetDetail)
        {
            try
            {
                if (oRouteSheetDetail.RouteSheetDetailID <= 0)
                {
                    oRouteSheetDetail = oRouteSheetDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRouteSheetDetail = oRouteSheetDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult UpdateDetail(RouteSheet oRouteSheet)
        {

            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();

            try
            {
                oRouteSheetDetails = RouteSheetDetail.Gets(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRouteSheetDetails = RouteSheetDetail.Update(oRouteSheetDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
                if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID > 0)
                {
                    _oRouteSheetDetail = this.MakeTree(oRouteSheetDetails);
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
                _oRouteSheetDetail = new RouteSheetDetail();

                _oRouteSheetDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(RouteSheetDetail oRouteSheetDetail)
        {
            try
            {
                if (oRouteSheetDetail.RouteSheetDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oRouteSheetDetail = oRouteSheetDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetDetail = new RouteSheetDetail();
                oRouteSheetDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                DateTime dtRouteSheetFrom_His = DateTime.Now;
                DateTime dtRouteSheetTo_His = DateTime.Now;
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[0])) ? "" : oRouteSheet.Params.Split('~')[0].Trim();
                int ncboRSDateSearch = Convert.ToInt16(oRouteSheet.Params.Split('~')[1]);
                DateTime dtRouteSheetFrom = Convert.ToDateTime(oRouteSheet.Params.Split('~')[2]);
                DateTime dtRouteSheetTo = Convert.ToDateTime(oRouteSheet.Params.Split('~')[3]);
                string sDOID = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[4])) ? "" : oRouteSheet.Params.Split('~')[4].Trim();
                string sContractorIDs = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[5])) ? "" : oRouteSheet.Params.Split('~')[5].Trim();
                string sMachineIDs = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[6])) ? "" : oRouteSheet.Params.Split('~')[6].Trim();
                string sRSState = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[7])) ? "" : oRouteSheet.Params.Split('~')[7].Trim();
                string sOrderNo = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[8])) ? "" : oRouteSheet.Params.Split('~')[8].Trim();
                string sProductIDs = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[9])) ? "" : oRouteSheet.Params.Split('~')[9].Trim();
                int nOrderType = Convert.ToInt16(oRouteSheet.Params.Split('~')[10]);

                int ncboRSDateSearch_His = 0;
                if (oRouteSheet.Params.Split('~').Length > 11)
                    Int32.TryParse(oRouteSheet.Params.Split('~')[11], out ncboRSDateSearch_His);

                if (ncboRSDateSearch_His > 0)
                {
                    dtRouteSheetFrom_His = Convert.ToDateTime(oRouteSheet.Params.Split('~')[12]);
                    dtRouteSheetTo_His = Convert.ToDateTime(oRouteSheet.Params.Split('~')[13]);
                }

                int nRSShiftID = 0;
                int nDUDyeingType = 0;
                bool IsReDyeing = false;
                int nRSStateHis = 0;
                if (oRouteSheet.Params.Split('~').Length > 14) 
                {
                    Int32.TryParse(oRouteSheet.Params.Split('~')[14], out nRSShiftID);
                    Int32.TryParse(oRouteSheet.Params.Split('~')[15], out nDUDyeingType);
                    Boolean.TryParse(oRouteSheet.Params.Split('~')[16], out IsReDyeing);
                }
                if (oRouteSheet.Params.Split('~').Length > 17)
                {
                    Int32.TryParse(oRouteSheet.Params.Split('~')[17], out nRSStateHis);
                }

                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 ";
                string sReturn = " ";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";

                #region OrderDate Date
                if (ncboRSDateSearch != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboRSDateSearch == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) ";
                    }

                    sSQL = sSQL + sReturn;
                }
                #endregion

                #region History Date
                if (ncboRSDateSearch_His != (int)EnumCompareOperator.None)
                {
                    if (nRSStateHis <= 0) { nRSStateHis = ((int)EnumRSState.InFloor); }

                    Global.TagSQL(ref sReturn);
                    if (ncboRSDateSearch_His == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + nRSStateHis.ToString() + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                        //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + nRSStateHis.ToString() + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + nRSStateHis.ToString() + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + nRSStateHis.ToString() + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + nRSStateHis.ToString() + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + nRSStateHis.ToString() + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    sRSState = "";
                    sSQL = sSQL + sReturn;
                }
                #endregion
               // if (IsdtRouteSheetSearch) sSQL = sSQL + " And RouteSheetDate Between '" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "' And '" + dtRouteSheetTo.ToString("dd MMM yyyy") + "'";

                if (sDOID != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (" + sDOID + "))";
                if (sContractorIDs != "") sSQL = sSQL + " And RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ContractorID In (" + sContractorIDs + "))";
                if (sMachineIDs != "") sSQL = sSQL + " And MachineID In (" + sMachineIDs + ")";
                if (sRSState != "") sSQL = sSQL + " And RSState In (" + sRSState + ")";
                if (sProductIDs != "") sSQL = sSQL + " And ProductID_Raw In (" + sProductIDs + ")";
                if (nOrderType > 0) sSQL = sSQL + " And OrderType In (" + nOrderType + ")";
                if (nRSShiftID > 0) sSQL = sSQL + " And RSShiftID In (" + nRSShiftID + ")";
                if (nDUDyeingType > 0) sSQL = sSQL + " And HanksCone In (" + nDUDyeingType + ")";
                if (IsReDyeing) sSQL = sSQL + " And IsReDyeing = 1";
                if (sOrderNo != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like '%"+ sOrderNo +"%')";

                sSQL=sSQL+" order by RouteSheetDate DESC";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheets = new List<RouteSheet>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oRouteSheets);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(_oRouteSheets, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
     
        [HttpPost]
        public JsonResult GetsRouteSheetDO(RouteSheetDO oRouteSheetDO)// For Delivery Order
        {
            string sSQL = "";
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
           
            try
            {
              
                    sSQL = "Select * from View_DyeingOrderDetailWaitingForRS ";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oRouteSheetDO.OrderNo))
                    {
                        oRouteSheetDO.OrderNo = oRouteSheetDO.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderNo Like'%" + oRouteSheetDO.OrderNo + "'";
                    }
                    if (oRouteSheetDO.OrderTypeInt > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderType=" + oRouteSheetDO.OrderTypeInt + "";
                    }
                    if (oRouteSheetDO.LabDipDetailID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "LabDipDetailID=" + oRouteSheetDO.LabDipDetailID + "";
                    }

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(OrderQty+30)>Qty_Pro";

                    sSQL = sSQL + "" + sReturn;
                    oRouteSheetDOs = RouteSheetDO.GetsDOYetTORS(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
               
            }
            catch (Exception ex)
            {
                oRouteSheetDOs = new List<RouteSheetDO>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRouteSheetDO_Grace(RouteSheetDO oRouteSheetDO)// For Grace Entry
        {
            string sSQL = "";
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            List<RouteSheetDO> oRouteSheetDOs_Prv = new List<RouteSheetDO>();
            List<RouteSheetGrace> oRouteSheetGraces = new List<RouteSheetGrace>();
            List<RouteSheetGrace> oRouteSheetGraces_Total = new List<RouteSheetGrace>();
            List<RouteSheetGrace> oRouteSheetGraces_Selected = new List<RouteSheetGrace>();
            string sDODIDs = "";
            try
            {
                sSQL = "SELECT RouteSheetID, DyeingOrderDetailID,  OrderNo, OrderQty, ProductName, SUM(Qty_RS)  AS Qty_RS FROM View_RouteSheetDO WHERE RouteSheetID = " + oRouteSheetDO.RouteSheetID;
                oRouteSheetDOs      = RouteSheetDO.Gets(sSQL + " GROUP BY RouteSheetID, DyeingOrderDetailID, OrderNo, OrderQty, ProductName", ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT RouteSheetID, DyeingOrderDetailID,  OrderNo, OrderQty, ProductName, SUM(Qty_RS)  AS Qty_RS ,SUM(RecycleQty)  AS RecycleQty, SUM(WastageQty)  AS WastageQty  FROM View_RouteSheetDO AS TT WHERE TT.DyeingOrderDetailID IN (SELECT DyeingOrderDetailID FROM View_RouteSheetDO WHERE RouteSheetID=" + oRouteSheetDO.RouteSheetID + ") AND RouteSheetID != " + oRouteSheetDO.RouteSheetID;
                oRouteSheetDOs_Prv = RouteSheetDO.Gets(sSQL  + " GROUP BY RouteSheetID, DyeingOrderDetailID, OrderNo, OrderQty, ProductName", ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_RouteSheetGrace WHERE RouteSheetID = " + oRouteSheetDO.RouteSheetID;
                oRouteSheetGraces = RouteSheetGrace.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sDODIDs = string.Join(",", oRouteSheetDOs.Select(x => x.DyeingOrderDetailID).ToList());

                sSQL = "SELECT * FROM View_RouteSheetGrace WHERE DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderDetailID in (" + sDODIDs +") ) and RouteSheetID <>"+ oRouteSheetDO.RouteSheetID;
                oRouteSheetGraces_Total = RouteSheetGrace.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (var oRSDO in oRouteSheetDOs)
                {
                    var oRSG = oRouteSheetGraces.Where(x=>x.RouteSheetID == oRSDO.RouteSheetID && x.DyeingOrderDetailID == oRSDO.DyeingOrderDetailID).FirstOrDefault();

                    if (oRSG == null) oRSG = new RouteSheetGrace();

                    oRouteSheetGraces_Selected.Add(
                            new RouteSheetGrace() 
                            {
                                RouteSheetGraceID   = oRSG.RouteSheetGraceID,
                                QtyGrace            = oRSG.QtyGrace,
                                Note                = oRSG.Note,
                                ApprovedByID        = oRSG.ApprovedByID,
                                ApprovedByName      = oRSG.ApprovedByName,

                                RouteSheetID        = oRSDO.RouteSheetID,
                                DyeingOrderDetailID = oRSDO.DyeingOrderDetailID,
                                OrderNo             = oRSDO.OrderNo,
                                ProductName         = oRSDO.ProductName,
                                QtyRS               = oRSDO.Qty_RS,
                                OrderQty            = oRSDO.OrderQty,
                                RecycleQty          = oRouteSheetDOs_Prv.Where(x => x.DyeingOrderDetailID == oRSDO.DyeingOrderDetailID).Sum(x => x.RecycleQty),// oRSDO.WastageQty,    = oRSDO.RecycleQty,
                                WastageQty          = oRouteSheetDOs_Prv.Where(x => x.DyeingOrderDetailID == oRSDO.DyeingOrderDetailID).Sum(x => x.WastageQty),// oRSDO.WastageQty,
                                QtyRS_Previous      = oRouteSheetDOs_Prv.Where(x=>x.DyeingOrderDetailID == oRSDO.DyeingOrderDetailID).Sum(x=>x.Qty_RS),
                                QtyGrace_Previous = oRouteSheetGraces_Total.Where(x => x.DyeingOrderDetailID == oRSDO.DyeingOrderDetailID).Sum(x => x.QtyGrace),
                            }
                        );
                }
            }
            catch (Exception ex)
            {
                oRouteSheetDOs = new List<RouteSheetDO>();
                oRouteSheetDOs.Add(new RouteSheetDO() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetGraces_Selected);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRSDO(RouteSheetDO oRouteSheetDO)
        {
            string sErrorMessage = "";
            try
            {
                if (oRouteSheetDO.RouteSheetDOID > 0)
                {
                    sErrorMessage = oRouteSheetDO.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRouteSheet(RouteSheet oRouteSheet)
        {
            try
            {
                oRouteSheet = RouteSheet.Get(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRouteSheet.RouteSheetID > 0)
                {
                    oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRouteSheetDetail(RouteSheetDetail oRSD)
        {
            try
            {
                if (oRSD.RouteSheetDetailID <= 0)
                    throw new Exception("Please select a valid items.");
                oRSD = RouteSheetDetail.Get(oRSD.RouteSheetDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSD = new RouteSheetDetail();
                oRSD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRSDetailWithAddition(RouteSheetDetail oRSD)
        {
            try
            {
                if (oRSD.RouteSheetDetailID <= 0)
                    throw new Exception("Please select a valid items.");
                oRSD = RouteSheetDetail.Get(oRSD.RouteSheetDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRSD.RSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE ISNULL(SequenceNo,0) > 0 AND RouteSheetDetailID =" + oRSD.RouteSheetDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSD = new RouteSheetDetail();
                oRSD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRSDetailWithAdditionV2(RouteSheetDetail oRSD)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            string sIDs = "";
            try
            {
                if (oRSD.RouteSheetDetailID <= 0)
                    throw new Exception("Please select a valid items.");
                oRSD = RouteSheetDetail.Get(oRSD.RouteSheetDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRSD.IsDyesChemical == true)
                {
                    oRouteSheetDetails = RouteSheetDetail.Gets("Select * from RouteSheetDetail where RouteSheetID=" + oRSD.RouteSheetID + " and IsDyesChemical=1 and RouteSheetDetailID=" + oRSD.RouteSheetDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRSD.RSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE  RouteSheetDetailID =" + oRSD.RouteSheetDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRouteSheetDetails = RouteSheetDetail.Gets("Select * from RouteSheetDetail where RouteSheetID="+ oRSD.RouteSheetID +" and IsDyesChemical=1 and ParentID=" + oRSD.RouteSheetDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sIDs = string.Join(",", oRouteSheetDetails.Select(x => x.RouteSheetDetailID).Distinct().ToList());
                    oRSD.RSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE RouteSheetDetailID in (" + sIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                int ncount = 0;
                foreach (RouteSheetDetail oitem in oRouteSheetDetails)
                {
                  ncount= oRSD.RSDetailAdditonals.Where(b => b.RouteSheetDetailID == oitem.RouteSheetDetailID && b.SequenceNo<=0 ).Count();
                     if (ncount<=0)
                    {
                        _oRSDetailAdditonal = new RSDetailAdditonal();
                        _oRSDetailAdditonal.RouteSheetDetailID = oitem.RouteSheetDetailID;
                        _oRSDetailAdditonal.RouteSheetID = oitem.RouteSheetID;
                        _oRSDetailAdditonal.RSDetailAdditonalID = 0;
                        _oRSDetailAdditonal.LotID = oitem.SuggestLotID;
                        _oRSDetailAdditonal.LotNo = oitem.SuggestLotNo;
                        _oRSDetailAdditonal.SequenceNo = 0;
                        _oRSDetailAdditonal.InOutType = (int)EnumInOutType.Disburse;
                        _oRSDetailAdditonal.RouteSheetID = oitem.RouteSheetID;
                        _oRSDetailAdditonal.Qty = oitem.TotalQty;
                        //_oRSDetailAdditonal.IssueDate = oitem.Sequence;
                        _oRSDetailAdditonals.Add(_oRSDetailAdditonal);
                    }
                }
                _oRSDetailAdditonals.ForEach(x => oRSD.RSDetailAdditonals.Add(x));
            }
            catch (Exception ex)
            {
                oRSD = new RouteSheetDetail();
                oRSD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRSDetailTree(RouteSheet oRouteSheet)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail= new RouteSheetDetail();
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");

                string sSQL = "Select * from View_RouteSheetDetail WHere RouteSheetID=" + oRouteSheet.RouteSheetID + " Order By Sequence";
                oRouteSheetDetails = RouteSheetDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRouteSheetDetails.Count() > 0 && oRouteSheetDetails.FirstOrDefault().RouteSheetDetailID > 0)
                {
                    oRouteSheetDetail = this.MakeTree(oRouteSheetDetails);
                }
                else
                {
                    throw new Exception("No dyeing solution found for this routesheet.");
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
        public JsonResult GetsLotForRS(RouteSheet oRouteSheet)
        {
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            int nContractorID = 0;
            int nDyeingOrderDetailID = 0;
            int nDyeingOrderID = 0;
            int nProductType = 0;
            string sWUIDs = "";
            if (!String.IsNullOrEmpty(oRouteSheet.Params))
            {
                nContractorID = Convert.ToInt32(oRouteSheet.Params.Split('~')[0]);
                nDyeingOrderDetailID = Convert.ToInt32(oRouteSheet.Params.Split('~')[1]);
                nProductType = Convert.ToInt32(oRouteSheet.Params.Split('~')[2]);
            }

            List<Lot> oLots = new List<Lot>();
            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            if (oRouteSheet.RouteSheetID>0)
            {
                oDUProGuideLines = DUProGuideLine.Gets("Select * from view_DUProGuideLine where isnull(ApproveByID,0)<>0 and ProductType=" + nProductType + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID in (select DyeingOrderDetailID from RoutesheetDO where RoutesheetDO.RouteSheetID=" + oRouteSheet.RouteSheetID + " ) )", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oDUProGuideLines = DUProGuideLine.Gets(" Select * from view_DUProGuideLine where isnull(ApproveByID,0)<>0 and ProductType=" + nProductType + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID=" + nDyeingOrderDetailID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (oDUProGuideLines.Count > 0)
            {
                nContractorID = oDUProGuideLines[0].ContractorID;
                nDyeingOrderID = oDUProGuideLines[0].DyeingOrderID;
            }
            else
            {
                nDyeingOrderID = 0;
                nContractorID = 0;
                nDyeingOrderDetailID = 0;
            }

            List<LotParent> oLotParents = new List<LotParent>();
            oLotParents = LotParent.Gets("Select * from View_LotParent Where LotID<>0 and isnull(Balance,0)>0 and isnull(BalanceLot,0)>0.1 and DyeingorderID in (Select DyeingOrderID from View_RouteSheetDO where RouteSheetID="+oRouteSheet.RouteSheetID+")", (int)Session[SessionInfo.currentUserID]);


            try
            {
                oWorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                sWUIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());

                string sLotNo = (!string.IsNullOrEmpty(oRouteSheet.LotNo)) ? oRouteSheet.LotNo.Trim() : "";

                string sSQL = "Select * from View_Lot Where isnull(LotStatus,0) in (" + (int)EnumLotStatus.Open + "," + (int)EnumLotStatus.Running + ") and Balance>0.2";

                if (!string.IsNullOrEmpty(sLotNo))
                    sSQL = sSQL + " And LotNo Like '%" + sLotNo + "%'";
                if (oRouteSheet.ProductID_Raw > 0)
                    sSQL = sSQL + " And ProductID=" + oRouteSheet.ProductID_Raw;
                if (oRouteSheet.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oRouteSheet.WorkingUnitID;
                if (nContractorID > 0) { sSQL = sSQL + " And ContractorID=" + nContractorID;}
                if (nDyeingOrderID > 0 && nProductType > 0)
                    sSQL = sSQL + "and LotID in (Select LotID from DUProGuideLineDetail where DUProGuideLineID in (Select DUProGuideLineID from DUProGuideLine where isnull(ApproveByID,0)<>0 and productType=" + nProductType + " and  DyeingOrderID=" + nDyeingOrderID + " )) or ParentLotID in (Select LotID from DUProGuideLineDetail where DUProGuideLineID in (Select DUProGuideLineID from  DUProGuideLine where productType=" + nProductType + " and DyeingOrderID =" + nDyeingOrderID + "))";

                if (oRouteSheet.WorkingUnitID <= 0)
                {
                    throw new Exception("Store Not Found!!");
                }
                if (!string.IsNullOrEmpty(sWUIDs))
                {
                    sSQL = sSQL + " And WorkingunitID in (" + sWUIDs + ")";
                }
                else
                {
                    throw new Exception("Store not Permited!");
                }
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                 oLots.ForEach(x =>
                    {
                        if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID )  ).Count() > 0)
                        {
                            x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                        }
                    });
            

               // oLots.ForEach(o => o.OrderRecapNo = oLotParents.Select(x=>x.LotID==o.LotID).ToList().FirstOrDefault().;
                
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
                oLots = new List<Lot>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oLots);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsLotForRSDC(RouteSheet oRouteSheet)
        {
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            int nContractorID = 0;
            int nDyeingOrderDetailID = 0;
            int nDyeingOrderID = 0;
            int nProductType = 0;
          
            //if (!String.IsNullOrEmpty(oRouteSheet.Params))
            //{
            //    nContractorID = Convert.ToInt32(oRouteSheet.Params.Split('~')[0]);
            //    nDyeingOrderDetailID = Convert.ToInt32(oRouteSheet.Params.Split('~')[1]);
            //    nProductType = Convert.ToInt32(oRouteSheet.Params.Split('~')[2]);
            //}

            List<Lot> oLots = new List<Lot>();
            //List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            //if (oRouteSheet.RouteSheetID > 0)
            //{
            //    oDUProGuideLines = DUProGuideLine.Gets("Select * from view_DUProGuideLine where isnull(ApproveByID,0)<>0 and ProductType=" + nProductType + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID in (select DyeingOrderDetailID from RoutesheetDO where RoutesheetDO.RouteSheetID=" + oRouteSheet.RouteSheetID + " ) )", ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}
            //else
            //{
            //    oDUProGuideLines = DUProGuideLine.Gets(" Select * from view_DUProGuideLine where isnull(ApproveByID,0)<>0 and ProductType=" + nProductType + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID=" + nDyeingOrderDetailID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}

            //if (oDUProGuideLines.Count > 0)
            //{
            //    nContractorID = oDUProGuideLines[0].ContractorID;
            //    nDyeingOrderID = oDUProGuideLines[0].DyeingOrderID;
            //}
            //else
            //{
            //    nDyeingOrderID = 0;
            //    nContractorID = 0;
            //    nDyeingOrderDetailID = 0;
            //}

            //List<LotParent> oLotParents = new List<LotParent>();
            //oLotParents = LotParent.Gets("Select * from View_LotParent Where LotID<>0 and isnull(Balance,0)>0 and isnull(BalanceLot,0)>0.1 and DyeingorderID in (Select DyeingOrderID from View_RouteSheetDO where RouteSheetID=" + oRouteSheet.RouteSheetID + ")", (int)Session[SessionInfo.currentUserID]);

            try
            {
                string sLotNo = (!string.IsNullOrEmpty(oRouteSheet.LotNo)) ? oRouteSheet.LotNo.Trim() : "";
                string sSQL = "Select * from View_Lot Where  isnull(LotStatus,0) in (" + (int)EnumLotStatus.Open + "," + (int)EnumLotStatus.Running + ") and Balance>0.001";
                if (!string.IsNullOrEmpty(sLotNo))
                    sSQL = sSQL + " And LotNo Like '%" + sLotNo + "%'";
                if (oRouteSheet.ProductID_Raw > 0)
                    sSQL = sSQL + " And ProductID=" + oRouteSheet.ProductID_Raw;
                if (oRouteSheet.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oRouteSheet.WorkingUnitID;
                if (nContractorID > 0) { sSQL = sSQL + " And ContractorID=" + nContractorID; }
                if (nDyeingOrderID > 0 && nProductType > 0)
                    sSQL = sSQL + "and LotID in (Select LotID from DUProGuideLineDetail where DUProGuideLineID in (Select DUProGuideLineID from DUProGuideLine where isnull(ApproveByID,0)<>0 and productType=" + nProductType + " and  DyeingOrderID=" + nDyeingOrderID + " )) or ParentLotID in (Select LotID from DUProGuideLineDetail where DUProGuideLineID in (Select DUProGuideLineID from  DUProGuideLine where productType=" + nProductType + " and DyeingOrderID =" + nDyeingOrderID + "))";

                if (oRouteSheet.WorkingUnitID <= 0)
                {
                    throw new Exception("Store Not Found!!");
                }
                //if (!string.IsNullOrEmpty(sWUIDs))
                //{
                //    sSQL = sSQL + " And WorkingunitID in (" + sWUIDs + ")";
                //}
                //else
                //{
                //    throw new Exception("Store not Permited!");
                //}
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //oLots.ForEach(x =>
                //{
                //    if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID)).Count() > 0)
                //    {
                //        x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                //    }
                //});


                // oLots.ForEach(o => o.OrderRecapNo = oLotParents.Select(x=>x.LotID==o.LotID).ToList().FirstOrDefault().;

            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
                oLots = new List<Lot>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oLots);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsLotForRSDC_Addition(RouteSheet oRouteSheet)
        {
            List<Lot> oLots = new List<Lot>();
            
            try
            {
                string sLotNo = (!string.IsNullOrEmpty(oRouteSheet.LotNo)) ? oRouteSheet.LotNo.Trim() : "";
                string sSQL = "SELECT * FROM View_Lot WHERE isnull(LotStatus,0) in (" + (int)EnumLotStatus.Open + "," + (int)EnumLotStatus.Running + ") and Balance>0.001";
                if (!string.IsNullOrEmpty(sLotNo))
                    sSQL = sSQL + " And LotNo Like '%" + sLotNo + "%'";
                if (oRouteSheet.ProductID_Raw > 0)
                    sSQL = sSQL + " And ProductID=" + oRouteSheet.ProductID_Raw;
                if (oRouteSheet.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oRouteSheet.WorkingUnitID;
              
                if (oRouteSheet.WorkingUnitID <= 0)
                {
                    throw new Exception("Store Not Found!!");
                }
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
                oLots = new List<Lot>();
            }
            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsLot(RouteSheet oRouteSheet)
        {
            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            Lot oLot = new Lot();
            string sLotID = "";
            string sParentLotID = "";
            string sSQL = "";
            string sWUIDs = "";
            int nDyeingOrderDetailID = 0;
            string sDyeingOrderIDs = "";
            if (!String.IsNullOrEmpty(oRouteSheet.Params))
            {
                //    nContractorID = Convert.ToInt32(oRouteSheet.Params.Split('~')[0]);
                nDyeingOrderDetailID = Convert.ToInt32(oRouteSheet.Params.Split('~')[1]);
                //  nProductType = Convert.ToInt32(oRouteSheet.Params.Split('~')[2]);
            }
            try
            {

                
                oWorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                sWUIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());

                    if (oRouteSheet.RouteSheetID <= 0)
                    {
                        sSQL = "Select * from View_DyeingOrderDetailWaitingForRS DyeingOrderDetailID=" + nDyeingOrderDetailID + "";
                        oRouteSheetDOs = RouteSheetDO.GetsDOYetTORS(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        oRouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (nDyeingOrderDetailID > 0)
                        {
                            oRouteSheetDOs = oRouteSheetDOs.Where(x => x.DyeingOrderDetailID == nDyeingOrderDetailID).ToList();
                        }
                    }

                    if (oRouteSheetDOs.Count <= 0)
                    {
                        throw new Exception("order not found for this Dyeing card!!");
                    }

                    if (oRouteSheetDOs.Count > 0)
                    {
                        oDUOrderSetup = oDUOrderSetup.GetByType((int)oRouteSheetDOs[0].OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //nDyeingOrderID = oRouteSheetDOs[0].DyeingOrderID;
                        sDyeingOrderIDs = string.Join(",", oRouteSheetDOs.Select(x => x.DyeingOrderID).ToList());

                    }
                    oRouteSheet.LotNo = (!string.IsNullOrEmpty(oRouteSheet.LotNo)) ? oRouteSheet.LotNo.Trim() : "";
                    if (oRouteSheet.IsReDyeing != EnumReDyeingStatus.None) { oDUOrderSetup.IsOpenRawLot = true; }

                    if (oDUOrderSetup.IsOpenRawLot == false)
                    {
                        // sSQL = "Select * from View_FabricLotAssign where  FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where DyeingOrderID=" + nDyeingOrderID + " )";
                        sSQL="";
                        if (oRouteSheet.ProductID_Raw > 0)
                            sSQL = sSQL + " where HH.ProductID=" + oRouteSheet.ProductID_Raw;
                      //  sSQL = "Select  LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,OperationUnitName,ParentLotID,sum(HH.Qty) as Qty,(sum(HH.Qty)-isnull((Select SUM(Qty) from DURequisitionDetail as DUR where ProductID=HH.ProductID and  DyeingOrderID=HH.DyeingOrderID and  DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=102)) ,0) ) as Balance from  (Select DestinationLotID as LotID ,Lt.LotNo, DO.OrderNo as ExeNo, Qty as Qty,  Lt.Balance as BalanceLot, DURD.ProductID,DURD.DyeingOrderID ,Product.ProductCode,Product.ProductName,Lt.WorkingUnitID,WU.OperationUnitName,lt.ParentLotID  from DURequisitionDetail as DURD  left join DURequisition as DUR ON DUR.DURequisitionID =DURD.DURequisitionID  left join DyeingOrder as DO ON DO.DyeingOrderID =DURD.DyeingOrderID  left join Lot as Lt ON Lt.LotID =DURD.DestinationLotID  left join Product as Product ON Product.ProductID =DURD.ProductID  left join View_WorkingUnit as WU ON WU.WorkingUnitID =Lt.WorkingUnitID   where DURD.DyeingOrderID=" + nDyeingOrderID + " and DUR.RequisitionType=101 and DestinationLotID in (Select Lot.LotID from Lot where WorkingUnitID=" + oRouteSheet.WorkingUnitID + ") )as HH " + sSQL + "  group by LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,OperationUnitName,ParentLotID";
                        sSQL = "Select MUName, LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,LocationName,OperationUnitName,ParentLotID,sum(HH.Qty) as Qty,(sum(HH.Qty)-isnull((Select SUM(Qty) from DURequisitionDetail as DUR where DUR.ProductID=HH.ProductID and  LotID=HH.LotID and  DyeingOrderID=HH.DyeingOrderID and  DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=102)) ,0) ) -isnull((Select SUM(Qty) from view_RSRawLot where DyeingOrderID=HH.DyeingOrderID and LotID=HH.LotID and RouteSheetID in (Select RouteSheetID from RouteSheetDO where RouteSheetDO.DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderDetail.ProductID=HH.ProductID and DyeingOrderID=HH.DyeingOrderID ))),0) as Balance from  (Select DestinationLotID as LotID ,Lt.LotNo, DO.OrderNo as ExeNo, Qty as Qty,  Lt.Balance as BalanceLot, DURD.ProductID,DURD.DyeingOrderID ,Product.ProductCode,Product.ProductName,Lt.WorkingUnitID,WU.OperationUnitName,WU.LocationName,lt.ParentLotID,MU.Symbol as MUName  from DURequisitionDetail as DURD  left join DURequisition as DUR ON DUR.DURequisitionID =DURD.DURequisitionID    left join DyeingOrder as DO ON DO.DyeingOrderID =DURD.DyeingOrderID     left join Lot as Lt ON Lt.LotID =DURD.DestinationLotID     left join Product as Product ON Product.ProductID =DURD.ProductID 	    left join View_WorkingUnit as WU ON WU.WorkingUnitID =Lt.WorkingUnitID 		 left join MeasurementUnit as MU ON Product.MeasurementUnitID =MU.MeasurementUnitID    where DURD.DyeingOrderID in (" + sDyeingOrderIDs + ") and DUR.RequisitionType=101 and DestinationLotID 	   in (Select Lot.LotID from Lot where  WorkingUnitID in (" + sWUIDs + ") and  WorkingUnitID=" + oRouteSheet.WorkingUnitID + ") )as HH		" + sSQL + "       group by MUName, LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,LocationName,OperationUnitName,ParentLotID";                     
                        oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                     
                        if (oFabricLotAssigns.Count <= 0)
                        {
                            throw new Exception("Lot yet not assign with this order!!");
                        }

                      
                    }
                    else
                    {
                        sSQL = "Select top(500)* from View_Lot Where isnull(LotStatus,0) in (" + (int)EnumLotStatus.Open + "," + (int)EnumLotStatus.Running + ") and Balance>0.1 "; //and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where UnitType=" + (int)EnumWoringUnitType.Raw + ")
                        if(!string.IsNullOrEmpty(oRouteSheet.LotNo ))
                        {
                            sSQL = sSQL + " And LotNo like '%" + oRouteSheet.LotNo+"%'";
                        }
                        if (oRouteSheet.WorkingUnitID > 0)
                            sSQL = sSQL + " And WorkingUnitID=" + oRouteSheet.WorkingUnitID;
                        if (oDUOrderSetup.IsOpenRawLot == true)
                        {
                            //oRouteSheet.ProductID_Raw = 0;

                            if (oRouteSheet.IsReDyeing  != EnumReDyeingStatus.None && oRouteSheet.CopiedFrom > 0)
                            {
                                sSQL = sSQL + " And ParentType="+(int)EnumTriggerParentsType.RouteSheet+" and ParentID=" + oRouteSheet.CopiedFrom;
                            }
                            else
                            {
                                if (oRouteSheet.ProductID_Raw > 0)
                                    sSQL = sSQL + " And ProductID=" + oRouteSheet.ProductID_Raw;
                            }

                            //ParentType=106 and ParentID=11562
                        }
                        else
                        {
                            //if (oLotParent.ContractorID > 0) { sSQL = sSQL + " And ContractorID=" + oLotParent.ContractorID; }

                            if (!string.IsNullOrEmpty(sLotID))
                                sSQL = sSQL + " And  (LotID in (" + sLotID + ") or ParentLotID in (" + sLotID + "))";
                        }

                        if (!string.IsNullOrEmpty(sWUIDs))
                        {
                            sSQL = sSQL + " And WorkingunitID in (" + sWUIDs + ")";
                        }
                        else
                        {
                            throw new Exception("Store not Permited!");
                        }

                        if (oRouteSheet.WorkingUnitID <= 0)
                        {
                            throw new Exception("Store not found!");
                        }

                        oLots = Lot.Gets(sSQL+" Order by LotID Desc", (int)Session[SessionInfo.currentUserID]);
                    }

             
                if (oDUOrderSetup.IsOpenRawLot == false)
                {
                    foreach (FabricLotAssign oItem in oFabricLotAssigns)
                    {
                        oLot = new Lot();
                        oLot.LotID = oItem.LotID;
                        oLot.LotNo = oItem.LotNo;
                        oLot.OrderRecapNo = oItem.ExeNo;
                        oLot.Balance = Math.Round(oItem.BalanceLot,4);
                        oLot.StockValue = oItem.Balance;
                        if (oLot.Balance <= 0) { oLot.Balance = 0; }
                        if (oLot.StockValue <= 0) { oLot.StockValue = 0; }
                        //  oLot.Balance = oItem.BalanceLot;
                        oLot.LotID = oItem.LotID;
                        oLot.OperationUnitName = oItem.OperationUnitName;
                        oLot.LocationName = oItem.LocationName;
                        oLot.MUName = oItem.MUName;
                        oLot.ProductCode = oItem.ProductCode;
                        oLot.ProductName = oItem.ProductName;
                        oLots.Add(oLot);
                    }
                    //if (oDUOrderSetup.IsInHouse)
                    //{
                      
                    //    //oLots.ForEach(x =>
                    //    //{
                    //    //    if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID || b.ParentLotID == x.LotID || b.LotID == x.ParentLotID)).Count() > 0)
                    //    //    {
                    //    //        x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                    //    //        x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().Balance;
                    //    //        // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    //    //    }
                    //    //});
                    //}
                    //else
                    //{
                    //    oLots.ForEach(x =>
                    //    {
                    //        if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID || b.LotID == x.ParentLotID)).Count() > 0)
                    //        {
                    //            x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    //            x.StockValue = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().Balance;
                    //            // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    //        }
                    //    });
                    //}
                }
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
               // oLotParent = new LotParent();

                oRouteSheet.ErrorMessage = ex.Message; 
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLotAll(RouteSheet oRouteSheet)
        {
            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            Lot oLot = new Lot();
            string sLotID = "";
            string sParentLotID = "";
            string sSQL = "";
            string sWUIDs = "";
            int nDyeingOrderDetailID = 0;
            int nDyeingOrderID = 0;
            if (!String.IsNullOrEmpty(oRouteSheet.Params))
            {
                //    nContractorID = Convert.ToInt32(oRouteSheet.Params.Split('~')[0]);
                nDyeingOrderDetailID = Convert.ToInt32(oRouteSheet.Params.Split('~')[1]);
                //  nProductType = Convert.ToInt32(oRouteSheet.Params.Split('~')[2]);
            }
            try
            {


                oWorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                sWUIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());

                if (oRouteSheet.RouteSheetID <= 0)
                {
                    sSQL = "Select * from View_DyeingOrderDetailWaitingForRS DyeingOrderDetailID=" + nDyeingOrderDetailID + "";
                    oRouteSheetDOs = RouteSheetDO.GetsDOYetTORS(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                if (oRouteSheetDOs.Count <= 0)
                {
                    throw new Exception("order not found for this Dyeing card!!");
                }

                if (oRouteSheetDOs.Count > 0)
                {
                    oDUOrderSetup = oDUOrderSetup.GetByType((int)oRouteSheetDOs[0].OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    nDyeingOrderID = oRouteSheetDOs[0].DyeingOrderID;

                }
                oRouteSheet.LotNo = (!string.IsNullOrEmpty(oRouteSheet.LotNo)) ? oRouteSheet.LotNo.Trim() : "";
                if (oDUOrderSetup.IsOpenRawLot == false)
                {
                    // sSQL = "Select * from View_FabricLotAssign where  FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where DyeingOrderID=" + nDyeingOrderID + " )";
                    sSQL = "";
                    if (oRouteSheet.ProductID_Raw > 0)
                        sSQL = sSQL + " where HH.ProductID=" + oRouteSheet.ProductID_Raw;
                    //  sSQL = "Select  LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,OperationUnitName,ParentLotID,sum(HH.Qty) as Qty,(sum(HH.Qty)-isnull((Select SUM(Qty) from DURequisitionDetail as DUR where ProductID=HH.ProductID and  DyeingOrderID=HH.DyeingOrderID and  DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=102)) ,0) ) as Balance from  (Select DestinationLotID as LotID ,Lt.LotNo, DO.OrderNo as ExeNo, Qty as Qty,  Lt.Balance as BalanceLot, DURD.ProductID,DURD.DyeingOrderID ,Product.ProductCode,Product.ProductName,Lt.WorkingUnitID,WU.OperationUnitName,lt.ParentLotID  from DURequisitionDetail as DURD  left join DURequisition as DUR ON DUR.DURequisitionID =DURD.DURequisitionID  left join DyeingOrder as DO ON DO.DyeingOrderID =DURD.DyeingOrderID  left join Lot as Lt ON Lt.LotID =DURD.DestinationLotID  left join Product as Product ON Product.ProductID =DURD.ProductID  left join View_WorkingUnit as WU ON WU.WorkingUnitID =Lt.WorkingUnitID   where DURD.DyeingOrderID=" + nDyeingOrderID + " and DUR.RequisitionType=101 and DestinationLotID in (Select Lot.LotID from Lot where WorkingUnitID=" + oRouteSheet.WorkingUnitID + ") )as HH " + sSQL + "  group by LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,OperationUnitName,ParentLotID";
                    sSQL = "Select MUName, LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,LocationName,OperationUnitName,ParentLotID,sum(HH.Qty) as Qty,(sum(HH.Qty)-isnull((Select SUM(Qty) from DURequisitionDetail as DUR where ProductID=HH.ProductID and  DyeingOrderID=HH.DyeingOrderID and  DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=102)) ,0) ) -isnull((Select SUM(Qty) from RSRawLot where LotID=HH.LotID and RouteSheetID in (Select RouteSheetID from RouteSheetDO where RouteSheetDO.DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=HH.DyeingOrderID ))),0) as Balance from  (Select DestinationLotID as LotID ,Lt.LotNo, DO.OrderNo as ExeNo, Qty as Qty,  Lt.Balance as BalanceLot, DURD.ProductID,DURD.DyeingOrderID ,Product.ProductCode,Product.ProductName,Lt.WorkingUnitID,WU.OperationUnitName,WU.LocationName,lt.ParentLotID,MU.Symbol as MUName  from DURequisitionDetail as DURD  left join DURequisition as DUR ON DUR.DURequisitionID =DURD.DURequisitionID    left join DyeingOrder as DO ON DO.DyeingOrderID =DURD.DyeingOrderID     left join Lot as Lt ON Lt.LotID =DURD.DestinationLotID     left join Product as Product ON Product.ProductID =DURD.ProductID 	    left join View_WorkingUnit as WU ON WU.WorkingUnitID =Lt.WorkingUnitID 		 left join MeasurementUnit as MU ON Product.MeasurementUnitID =MU.MeasurementUnitID    where DURD.DyeingOrderID=" + nDyeingOrderID + " and DUR.RequisitionType=101 and DestinationLotID 	   in (Select Lot.LotID from Lot where  WorkingUnitID in (" + sWUIDs + ") and  WorkingUnitID=" + oRouteSheet.WorkingUnitID + " and  WorkingUnitID=116) )as HH		" + sSQL + "       group by MUName, LotID,LotNo,ExeNo,BalanceLot,ProductID,DyeingOrderID,ProductCode,ProductName,WorkingUnitID,LocationName,OperationUnitName,ParentLotID";
                    oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    if (oFabricLotAssigns.Count <= 0)
                    {
                        throw new Exception("Lot yet not assign with this order!!");
                    }


                }
                else
                {
                    sSQL = "Select top(500)* from View_Lot Where  isnull(LotStatus,0) in (" + (int)EnumLotStatus.Open + "," + (int)EnumLotStatus.Running + ") and Balance>0 "; //and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where UnitType=" + (int)EnumWoringUnitType.Raw + ")
                    if (!string.IsNullOrEmpty(oRouteSheet.LotNo))
                    {
                        sSQL = sSQL + " And LotNo like '%" + oRouteSheet.LotNo + "%'";
                    }
                    if (oRouteSheet.WorkingUnitID > 0)
                        sSQL = sSQL + " And WorkingUnitID=" + oRouteSheet.WorkingUnitID;
                    if (oDUOrderSetup.IsOpenRawLot == true)
                    {
                        //oRouteSheet.ProductID_Raw = 0;
                        if (oRouteSheet.ProductID_Raw > 0)
                            sSQL = sSQL + " And ProductID=" + oRouteSheet.ProductID_Raw;
                    }
                    else
                    {
                        //if (oLotParent.ContractorID > 0) { sSQL = sSQL + " And ContractorID=" + oLotParent.ContractorID; }

                        if (!string.IsNullOrEmpty(sLotID))
                            sSQL = sSQL + " And  (LotID in (" + sLotID + ") or ParentLotID in (" + sLotID + "))";
                    }

                    if (!string.IsNullOrEmpty(sWUIDs))
                    {
                        sSQL = sSQL + " And WorkingunitID in (" + sWUIDs + ")";
                    }
                    else
                    {
                        throw new Exception("Store not Permited!");
                    }

                    if (oRouteSheet.WorkingUnitID <= 0)
                    {
                        throw new Exception("Store not found!");
                    }

                    oLots = Lot.Gets(sSQL + " Order by LotID Desc", (int)Session[SessionInfo.currentUserID]);
                }


                if (oDUOrderSetup.IsOpenRawLot == false)
                {
                    foreach (FabricLotAssign oItem in oFabricLotAssigns)
                    {
                        oLot = new Lot();
                        oLot.LotID = oItem.LotID;
                        oLot.LotNo = oItem.LotNo;
                        oLot.OrderRecapNo = oItem.ExeNo;
                        oLot.Balance = Math.Round(oItem.BalanceLot, 4);
                        oLot.StockValue = oItem.Balance;
                        if (oLot.Balance <= 0) { oLot.Balance = 0; }
                        if (oLot.StockValue <= 0) { oLot.StockValue = 0; }
                        //  oLot.Balance = oItem.BalanceLot;
                        oLot.LotID = oItem.LotID;
                        oLot.OperationUnitName = oItem.OperationUnitName;
                        oLot.LocationName = oItem.LocationName;
                        oLot.MUName = oItem.MUName;
                        oLot.ProductCode = oItem.ProductCode;
                        oLot.ProductName = oItem.ProductName;
                        oLots.Add(oLot);
                    }
                    //if (oDUOrderSetup.IsInHouse)
                    //{

                    //    //oLots.ForEach(x =>
                    //    //{
                    //    //    if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID || b.ParentLotID == x.LotID || b.LotID == x.ParentLotID)).Count() > 0)
                    //    //    {
                    //    //        x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                    //    //        x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().Balance;
                    //    //        // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    //    //    }
                    //    //});
                    //}
                    //else
                    //{
                    //    oLots.ForEach(x =>
                    //    {
                    //        if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID || b.LotID == x.ParentLotID)).Count() > 0)
                    //        {
                    //            x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    //            x.StockValue = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().Balance;
                    //            // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    //        }
                    //    });
                    //}
                }
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                // oLotParent = new LotParent();

                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeRSStatus(RouteSheetHistory oRSH)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            try
            {
                if (oRSH.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");

                string ErrorMessage = RSStateValidation(oRSH.PreviousState, oRSH.CurrentStatus);
                if (ErrorMessage != "") { throw new Exception(ErrorMessage); }

                oRSH = oRSH.ChangeRSStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRSH.RouteSheetID > 0)
                {
                    oRouteSheet = RouteSheet.Get(oRSH.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    throw new Exception(oRSH.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult CopyRouteSheet(RouteSheet oRouteSheet)
        {
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) throw new Exception("Please select a valid RouteSheet.");

                oRouteSheet.CopiedFrom = oRouteSheet.RouteSheetID;
                oRouteSheet.RouteSheetID = 0;
                oRouteSheet.RSState = EnumRSState.Initialized;
                oRouteSheet.RouteSheetDate = DateTime.Today;
                foreach(RouteSheetDO oItem in  oRouteSheet.RouteSheetDOs)
                {
                    oItem.RouteSheetDOID = 0;
                    oItem.RouteSheetID = 0;
                }

                oRouteSheet = oRouteSheet.CopyRouteSheet(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult CopyTemplateFromRS(RouteSheet oRouteSheet)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) throw new Exception("Please select a valid Operation.");
                if (oRouteSheet.CopiedFrom <= 0) throw new Exception("Please select a valid Operation.");

                oRouteSheetDetails = RouteSheetDetail.IUDTemplateCopyFromRS(oRouteSheet.CopiedFrom, oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        public ActionResult PrintRouteSheetCustom(int nId, bool bIsCommon, double nts)
        {
            _oRouteSheet = new RouteSheet();
            List<RouteSheetDetail> oRSDetails = new List<RouteSheetDetail>();
            RouteSheetCombine oRouteSheetCombine = new RouteSheetCombine();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();
            List<RSInQCSubStatus> oRSInQCSubStatus = new List<RSInQCSubStatus>();
            try
            {
                oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nId > 0)
                {
                    _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oRouteSheet.RouteSheetID > 0)
                    {
                        string sSQL = "Select * from View_RouteSheetDetail Where RouteSheetID=" + _oRouteSheet.RouteSheetID + " Order By Sequence";
                        oRSDetails = RouteSheetDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.RouteSheetDetail = this.MakeTree(oRSDetails);
                        _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (_oRouteSheet.RouteSheetDOs.Count > 0)
                        {
                            _oRouteSheet.OrderNo = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.OrderNoFull).Distinct().ToList());
                        }
                        //_oRouteSheet.OrderNo = this.GetOrderNos(_oRouteSheet.RouteSheetDOs);
                        oRSRawLots = RSRawLot.GetsByRSID(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oRSRawLots.Count > 0)
                        {
                            _oRouteSheet.LotNo = string.Join(",", oRSRawLots.Select(x => x.LotNo).ToList());
                        }
                        oRouteSheetGrace = oRouteSheetGrace.GetByRS(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        oRouteSheetHistorys = RouteSheetHistory.Gets(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRSInQCSubStatus = RSInQCSubStatus.Gets("SELECT * FROM RSInQCSubStatus WHERE RouteSheetID=" + _oRouteSheet.RouteSheetID + " order by DBServerDateTime DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                    }
                    // Gets Total 
                    oRouteSheetCombine = oRouteSheetCombine.GetBy(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            var oBUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            var oBU = new BusinessUnit();
            if (oBUs.Count() > 0)
            {
                oBUs = oBUs.Where(x => x.BusinessUnitType == EnumBusinessUnitType.Dyeing).ToList();
                oBU = oBUs.FirstOrDefault();
            }

            if (oRouteSheetSetup.PrintNo == EnumExcellColumn.A)
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareReport(_oRouteSheet, oRSDetails, oRouteSheetCombine, oBU, bIsCommon, oRouteSheetSetup, oRouteSheetGrace);
                return File(abytes, "application/pdf");
            }
            if (oRouteSheetSetup.PrintNo == EnumExcellColumn.B)
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareWeighingSheet(_oRouteSheet, oRSDetails, oRouteSheetCombine, oBU, bIsCommon, oRouteSheetSetup, oDUPScheduleDetails, oRouteSheetHistorys, oRSInQCSubStatus);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareReport(_oRouteSheet, oRSDetails, oRouteSheetCombine, oBU, bIsCommon, oRouteSheetSetup, oRouteSheetGrace);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintRouteSheet(int nId, bool bIsCommon ,double nts)
        {
            _oRouteSheet = new RouteSheet();
            List<RouteSheetDetail> oRSDetails=new List<RouteSheetDetail>();
            RouteSheetCombine oRouteSheetCombine = new RouteSheetCombine();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            RSShift oRSShift = new RSShift();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();
            List<RSInQCSubStatus> oRSInQCSubStatus = new List<RSInQCSubStatus>();
           
            try
            {
                oRouteSheetSetup=oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nId > 0)
                {
                    _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oRouteSheet.RouteSheetID > 0)
                    {
                        string sSQL = "Select * from View_RouteSheetDetail Where RouteSheetID=" + _oRouteSheet.RouteSheetID + " Order By Sequence";
                        oRSDetails = RouteSheetDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        oRSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE ISNULL(SequenceNo,0) > 0 AND RouteSheetID =" + _oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        foreach (var oItem in oRSDetails)
                        {
                            oRSDetailAdditonals.ForEach(additon =>
                            {
                                if (additon.RouteSheetDetailID == oItem.RouteSheetDetailID)
                                {
                                    oItem.RSDetailAdditonals.Add(additon);
                                }
                            });
                        }

                        _oRouteSheet.RouteSheetDetail = this.MakeTree(oRSDetails);
                        _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRSRawLots = RSRawLot.GetsByRSID(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //_oRouteSheet.OrderNo = this.GetOrderNos(_oRouteSheet.RouteSheetDOs);
                        if (_oRouteSheet.RouteSheetDOs.Count > 0)
                        {
                            _oRouteSheet.OrderNo = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.OrderNoFull).Distinct().ToList());
                        }
                        oRouteSheetGrace = oRouteSheetGrace.GetByRS(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    if (_oRouteSheet.DUPScheduleID > 0)
                    {
                        oDUPScheduleDetails = DUPScheduleDetail.Gets(_oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oRSRawLots.Count <= 0)
                        {
                            oDUPSLots = DUPSLot.Gets("SELECT * FROM View_DUPSLot WHERE DUPScheduleID=" + _oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                    if (oDUPSLots.Count>0)
                    {
                       // oRSRawLots = RSRawLot.GetsByRSID(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.LotNo = string.Join(",", oDUPSLots.Select(x => x.LotNo).ToList());
                    }
                    if (oRSRawLots.Count>0)
                    {
                        _oRouteSheet.LotNo = string.Join(",", oRSRawLots.Select(x => x.LotNo).ToList());
                    }
                    if (_oRouteSheet.RSShiftID > 0)
                    {
                        oRSShift = oRSShift.Get(_oRouteSheet.RSShiftID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.Shift = oRSShift.Name;
                    }
                   // Gets Total 
                    oRouteSheetCombine = oRouteSheetCombine.GetBy(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheetHistorys = RouteSheetHistory.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRSInQCSubStatus = RSInQCSubStatus.Gets("SELECT * FROM View_RSInQCSubStatus WHERE RouteSheetID=" + nId + " order by DBServerDateTime DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            var oBUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            var oBU = new BusinessUnit();
            if (oBUs.Count() > 0)
            {
                oBUs = oBUs.Where(x => x.BusinessUnitType == EnumBusinessUnitType.Dyeing).ToList();
                oBU = oBUs.FirstOrDefault();
            }

            if (oRouteSheetSetup.PrintNo == EnumExcellColumn.A)
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareReport(_oRouteSheet, oRSDetails, oRouteSheetCombine, oBU, bIsCommon, oRouteSheetSetup, oRouteSheetGrace);
                return File(abytes, "application/pdf");
            }
            else if (oRouteSheetSetup.PrintNo == EnumExcellColumn.B)
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareWeighingSheet(_oRouteSheet, oRSDetails, oRouteSheetCombine, oBU, bIsCommon, oRouteSheetSetup, oDUPScheduleDetails, oRouteSheetHistorys, oRSInQCSubStatus);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareReport(_oRouteSheet, oRSDetails, oRouteSheetCombine, oBU, bIsCommon, oRouteSheetSetup, oRouteSheetGrace);
                return File(abytes, "application/pdf");
            }

        

        }
        public ActionResult PrintRouteSheetCombine(int nId, int nRSID, bool bIsCombine, double nts)
        {

            RouteSheetCombine oRouteSheetCombine = new RouteSheetCombine();
            List<RouteSheetDetail> oRSDetails = new List<RouteSheetDetail>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();
            try
            {
                if (nId > 0)
                {

                    oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheetCombine = oRouteSheetCombine.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oRouteSheetCombine.RouteSheetCombineID > 0)
                    {
                        oRouteSheetCombine.RouteSheetCombineDetails = RouteSheetCombineDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    }

                    _oRouteSheet = new RouteSheet();
                    _oRouteSheet.TtlCotton = 0;
                    if (bIsCombine)
                    {
                        if (oRouteSheetCombine.RouteSheetCombineDetails.Count > 0)
                        {
                            nRSID = oRouteSheetCombine.RouteSheetID;
                        }


                        _oRouteSheet = RouteSheet.Get(nRSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.Qty = 0;
                        _oRouteSheet.TtlLiquire = 0;
                        _oRouteSheet.TtlCotton = 0;
                        _oRouteSheet.RouteSheetNo = "";
                        _oRouteSheet.QtyDye = 0;
                        foreach (RouteSheetCombineDetail oItem in oRouteSheetCombine.RouteSheetCombineDetails)
                        {
                            _oRouteSheet.RouteSheetNo = _oRouteSheet.RouteSheetNo + oItem.RouteSheetNo + ",";
                            _oRouteSheet.Qty = _oRouteSheet.Qty + oItem.Qty;
                            _oRouteSheet.QtyDye = _oRouteSheet.QtyDye + oItem.QtyDye;
                            _oRouteSheet.TtlLiquire = _oRouteSheet.TtlLiquire + oItem.TtlLiquire;
                            _oRouteSheet.NoOfHanksCone = _oRouteSheet.NoOfHanksCone + oItem.NoOfHanksCone;
                            _oRouteSheet.TtlCotton = _oRouteSheet.TtlCotton + oItem.TtlCotton;
                        }
                    }
                    else
                    {

                        _oRouteSheet = RouteSheet.Get(nRSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.RouteSheetNo = "";
                        foreach (RouteSheetCombineDetail oItem in oRouteSheetCombine.RouteSheetCombineDetails)
                        {
                            _oRouteSheet.RouteSheetNo = _oRouteSheet.RouteSheetNo + oItem.RouteSheetNo + ",";
                            _oRouteSheet.Qty = _oRouteSheet.Qty + oItem.Qty;
                            _oRouteSheet.QtyDye = _oRouteSheet.QtyDye + oItem.QtyDye;
                            _oRouteSheet.NoOfHanksCone = _oRouteSheet.NoOfHanksCone + oItem.NoOfHanksCone;
                            _oRouteSheet.TtlCotton = _oRouteSheet.TtlCotton + oItem.TtlCotton;
                           
                        }
                    }
                    if (_oRouteSheet.RouteSheetID > 0)
                    {
                        string sSQL = "Select * from View_RouteSheetDetail Where RouteSheetID=" + _oRouteSheet.RouteSheetID + " Order By Sequence";
                        oRSDetails = RouteSheetDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.RouteSheetDetail = this.MakeTree(oRSDetails);
                        _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //_oRouteSheet.OrderNo = this.GetOrderNos(_oRouteSheet.RouteSheetDOs);
                        if (_oRouteSheet.RouteSheetDOs.Count > 0)
                        {
                            _oRouteSheet.OrderNo = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.OrderNoFull).Distinct().ToList());
                        }
                    }
                    //Gets Schedule
                    if (oRouteSheetCombine.RouteSheetCombineDetails.Count > 0)
                    {
                  
                     oRouteSheetCombine.Params=  string.Join(",", oRouteSheetCombine.RouteSheetCombineDetails.Select(x => x.RouteSheetID).ToList());
                     _oRouteSheet.RouteSheetDOs = RouteSheetDO.Gets("Select * from View_RouteSheetDO where RouteSheetID in (" + oRouteSheetCombine.Params + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        oRouteSheetCombine.Params = _oRouteSheet.RouteSheetID.ToString();
                    }
                    oDUPScheduleDetails = DUPScheduleDetail.GetsSqL("SELECT * FROM View_DUPScheduleDetail where RouteSheetID in(" + oRouteSheetCombine.Params + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //oRouteSheetCombine.Params = string.Join(",", oDUPScheduleDetails.Select(x => x.DUPScheduleID).Distinct().ToList());
                    //if (!string.IsNullOrEmpty(oRouteSheetCombine.Params))
                    //{
                    //    oDUPSLots = DUPSLot.Gets("SELECT * FROM View_DUPSLot where DUPScheduleID in (" + oRouteSheetCombine.Params + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //}

                    //oRSRawLots = RSRawLot.Gets("SELECT * FROM View_RSRawLot WHERE RouteSheetID in (" + oRouteSheetCombine.Params + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oDUPScheduleDetails.ForEach(o => o.ApproveLotNo = string.Join(",", oRSRawLots.Where(p => p.RouteSheetID == o.RouteSheetID).Select(x => x.LotNo).ToList()));
                    //_oRouteSheet.LotNo = string.Join(",", oRSRawLots.Select(x => x.LotNo).ToList());


                    if (oDUPScheduleDetails !=null && oDUPScheduleDetails.Count> 0)
                    {
                        oRouteSheetCombine.Params = string.Join(",", oDUPScheduleDetails.Select(x => x.DUPScheduleID).Distinct().ToList());
                        oDUPSLots = DUPSLot.Gets("SELECT * FROM View_DUPSLot WHERE DUPScheduleID in (" + oRouteSheetCombine.Params+")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oRouteSheet.LotNo = string.Join(",", oDUPSLots.Select(x => x.LotNo).ToList());
                        oDUPScheduleDetails.ForEach(o => o.ApproveLotNo =string.Join(",", oDUPSLots.Where(p => p.DUPScheduleID == o.DUPScheduleID).Select(x => x.LotNo).ToList()));
                    }
                    else
                    {
                        oDUPScheduleDetails = new List<DUPScheduleDetail>();
                    }
                    
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            var oBUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            var oBU = new BusinessUnit();
            if (oBUs.Count() > 0)
            {
                oBUs = oBUs.Where(x => x.BusinessUnitType == EnumBusinessUnitType.Dyeing).ToList();
                oBU = oBUs.FirstOrDefault();
            }

            if (oRouteSheetSetup.PrintNo == EnumExcellColumn.A)
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareReport_Combine(_oRouteSheet, oRSDetails, oRouteSheetCombine.RouteSheetCombineDetails, oCompany, oBU, bIsCombine, oRouteSheetSetup);
                return File(abytes, "application/pdf");
            }
            else if (oRouteSheetSetup.PrintNo == EnumExcellColumn.B)
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareReport_Combine_B(_oRouteSheet, oRSDetails, oRouteSheetCombine, oCompany, oBU, bIsCombine, oRouteSheetSetup, oDUPScheduleDetails, oRouteSheetHistorys);
                return File(abytes, "application/pdf");

            }
            else
            {
                rptRouteSheet oReport = new rptRouteSheet();
                byte[] abytes = oReport.PrepareReport_Combine(_oRouteSheet, oRSDetails, oRouteSheetCombine.RouteSheetCombineDetails, oCompany, oBU, bIsCombine, oRouteSheetSetup);
                return File(abytes, "application/pdf");
            }



        }
      
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

        #region RS in SubfinishingStore
        public ActionResult ViewRouteSheetInSubFinishing(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.EnumWarpWefts = Enum.GetValues(typeof(EnumWarpWeft)).Cast<EnumWarpWeft>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
         
            _oRouteSheet = new RouteSheet();
            return View(_oRouteSheet);
        }
        [HttpPost]
        public JsonResult GetsITransactionBYRS(RouteSheet oRouteSheet)
        {
            List<ITransaction> oITransactions = new List<ITransaction>();
            try
            {
                oITransactions = ITransaction.Gets("Select * from View_ITransaction where TriggerParentID=" + oRouteSheet.RouteSheetID + " and TriggerParentType="+  (int)EnumTriggerParentsType.RouteSheet+" and WorkingUnitID in (Select WorkingUnitID from RSInQCSetup where activity=1 and YarnType="+(int)EnumYarnType.FreshDyedYarn+") and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where LocationID=" + oRouteSheet.LocationID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {

                oITransactions = new List<ITransaction>();
              
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITransactions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRouteSheetDO_HW(RouteSheet oRouteSheet)
        {
            List<ITransaction> oITransactions = new List<ITransaction>();
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            try
            {
                //oITransactions = ITransaction.Gets("Select * from View_ITransaction where TriggerParentID=" + oRouteSheet.RouteSheetID + " and TriggerParentType=106 and WorkingUnitID in (Select WorkingUnitID from RSInQCSetup where activity=1 and YarnType=" + (int)EnumYarnType.FreshDyedYarn + ") and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where LocationID=" + oRouteSheet.LocationID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRouteSheetDOs = RouteSheetDO.Gets("Select * from View_RouteSheetDO where RouteSheetID=" + oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oRouteSheetDOs.Count() > 0) 
                {
                    oDUHardWindings = DUHardWinding.Gets("Select * from View_DUHardWinding where RouteSheetID IN (" + oRouteSheet.RouteSheetID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (RouteSheetDO oRS in oRouteSheetDOs) 
                    {
                        oRS.Qty_Finish = oDUHardWindings.Where(x => x.DyeingOrderDetailID == oRS.DyeingOrderDetailID).Sum(x => x.Qty);
                        oRS.Qty_Pro = oRS.Qty_RS - oRS.Qty_Finish;
                    }
                }                
            }
            catch (Exception ex)
            {

                oITransactions = new List<ITransaction>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRouteSheetSubFinishing(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                oRouteSheet.RouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.RouteSheetNo)) ? "" : oRouteSheet.RouteSheetNo.Trim();
                string sSQL = "Select top(250)* from View_RouteSheet Where RouteSheetID<>0 "; //+(int)EnumRSState.InFloor;
                if (oRouteSheet.RouteSheetNo != "")
                { sSQL = sSQL + "And RSState in (10,11,12,13,14,15,16,18) And RouteSheetNo Like '%" + oRouteSheet.RouteSheetNo + "%'";
                }
                else
                {
                    sSQL = sSQL + "And RSState in (10,11,12,13,14,15,16) And RouteSheetID not in (Select RouteSheetID from RouteSheetHistory where CurrentStatus=" + (int)EnumRSState.QC_Done + " )"; 
                }
                sSQL = sSQL + " order by RouteSheetDate desc";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                _oRouteSheets = new List<RouteSheet>();
                oRouteSheet.ErrorMessage = ex.Message;
                _oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RSInRSInSubFinishing(RouteSheet oRouteSheet)
        {
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");
                oRouteSheet.RSState = EnumRSState.InSubFinishingstore_Partially;
                oRouteSheet = oRouteSheet.RSInRSInSubFinishing(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region RSRawLot 
        [HttpPost]
        public JsonResult SaveRSRawLot(RouteSheet oRouteSheet)
        {
            RSRawLot oRSRawLot = new RSRawLot();
            try
            {
                oRouteSheet.RSRawLots = oRSRawLot.SaveMultiple(oRouteSheet.RSRawLots, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet.RSRawLots = new List<RSRawLot>();
                RSRawLot oRSRawLot_temp = new RSRawLot();
                oRSRawLot_temp.ErrorMessage = ex.Message;
                oRouteSheet.ErrorMessage = ex.Message;
                oRouteSheet.RSRawLots.Add(oRSRawLot_temp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteRSRawLot(RSRawLot oRSRawLot)
        {
            try
            {
                if (oRSRawLot.RSRawLotID <= 0) { throw new Exception("Please select an valid item."); }
                oRSRawLot.ErrorMessage = oRSRawLot.Delete(oRSRawLot.RSRawLotID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSRawLot = new RSRawLot();
                oRSRawLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSRawLot.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsRSRawLot(RouteSheet oRouteSheet)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            try
            {
                oRouteSheet.RSRawLots = RSRawLot.GetsByRSID(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oRouteSheet.DUPScheduleID > 0)
                //{
                //   // oDUPScheduleDetails = DUPScheduleDetail.Gets(oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsByRSID(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                if (oRouteSheet.RSRawLots.Count > 0)
                {
                    oRouteSheet.RSRawLots[0].Param = " YD No:";
                }
                //else
                //{
                //    RSRawLot oRSRawLot = new RSRawLot();
                //    oRSRawLot.Param = "jhghgh";
                //    oRouteSheet.RSRawLots.Add(oRSRawLot);
                //}
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetRSRawLotByOrder(int nRSID, int nDODID)
        {
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            try
            {
                oRSRawLots = RSRawLot.Gets("SELECT * FROM View_RSRawLot WHERE RouteSheetID=" + nRSID + " AND DyeingOrderDetailID=" + nDODID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSRawLots = new List<RSRawLot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSRawLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Yarn Out
        public ActionResult ViewRouteSheetYarnOut(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
       
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #region Issue Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(oBusinessUnit.BusinessUnitID, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = oWorkingUnits;
            #endregion

            List<Employee> oEmployees = new List<Employee>();
            oEmployees=Employee.Gets(EnumEmployeeDesignationType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = oEmployees;

            ViewBag.EnumCompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            return View(_oRouteSheets);
        }
        public ActionResult ViewRouteSheetMultiYarnOut(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            if (menuid > 0)
            {
                 sSQL = "Select top(100)* from View_RouteSheet Where RSState in (" + (int)EnumRSState.InFloor + ") order by RoutesheetDate DESC";// and LotID in (Select LotID from DUSoftWinding)
                _oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {

                sSQL = "Select * from View_RouteSheet where RouteSheetID in (Select RouteSheetID from RouteSheetHistory where RouteSheetHistory.CurrentStatus=" + (int)EnumRSState.YarnOut + " and EventTime>'"+DateTime.Today.AddMonths(-1).ToString("dd MMM yy")+"' and EventTime<'"+DateTime.Today.ToString("dd MMM yy")+"')";
                _oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            #region Issue Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            string sRSIDs = string.Join(",", _oRouteSheets.Select(x => x.RouteSheetID).ToList());

            if (!string.IsNullOrEmpty(sRSIDs))
            {
                sSQL = "SELECT * FROM View_RSRawLot WHERE RouteSheetID in (" + sRSIDs + ")";
                oRSRawLots = RSRawLot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            _oRouteSheets.ForEach(x =>
            {
                if (oRSRawLots.FirstOrDefault() != null && oRSRawLots.FirstOrDefault().RouteSheetID > 0 && oRSRawLots.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                {
                    //x.LotNo = oRSRawLots.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().LotNo;
                    x.LotNo = string.Join(",", oRSRawLots.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.LotNo).Distinct().ToList());
                }
            });
            if (_oRouteSheets.Count > 0)
            {
                _oRouteSheets = GetsYarnOuts(_oRouteSheets);
            }


            #endregion

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetSetup = oRouteSheetSetup;

            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.BUID = buid;

            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(EnumEmployeeDesignationType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = oEmployees;

            ViewBag.RSShifts = RSShift.GetsByModule(buid, (int)EnumModuleName.RouteSheet + "", ((User)Session[SessionInfo.CurrentUser]).UserID);


            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();

            sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";

            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            List<Location> oLocations = new List<Location>();
            ViewBag.Locations = oLocations;

            #region Order Type
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();
            #endregion

            ViewBag.OrderType = oDUOrderSetups;
            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState));
            ViewBag.ProductionScheduleStatus = EnumObject.jGets(typeof(EnumProductionScheduleStatus));
            return View(_oRouteSheets);
        }

        public ActionResult ViewRouteSheetMultiYarnOut2(int buid, int nProductID, DateTime dStartDate, DateTime dEndDate)
        {
            string sSQL = "Select * from View_RouteSheet_YarnOut where ProductID_Raw = " + nProductID + " AND RouteSheetID in (Select RouteSheetID from RouteSheetHistory where RouteSheetHistory.CurrentStatus=" + (int)EnumRSState.YarnOut + " and EventTime>'" + dStartDate.ToString("dd MMM yy") + "' and EventTime<'" + dEndDate.ToString("dd MMM yy") + "')";
            _oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            #region Issue Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            string sRSIDs = string.Join(",", _oRouteSheets.Select(x => x.RouteSheetID).ToList());

            if (!string.IsNullOrEmpty(sRSIDs))
            {
                sSQL = "SELECT * FROM View_RSRawLot WHERE RouteSheetID in (" + sRSIDs + ")";
                oRSRawLots = RSRawLot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            _oRouteSheets.ForEach(x =>
            {
                if (oRSRawLots.FirstOrDefault() != null && oRSRawLots.FirstOrDefault().RouteSheetID > 0 && oRSRawLots.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                {
                    x.LotNo = oRSRawLots.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().LotNo;
                }
            });
            if (_oRouteSheets.Count > 0)
            {
                _oRouteSheets = GetsYarnOuts(_oRouteSheets);
            }
            #endregion

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetSetup = oRouteSheetSetup;

            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.BUID = buid;

            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(EnumEmployeeDesignationType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = oEmployees;

            ViewBag.RSShifts = RSShift.GetsByModule(buid, (int)EnumModuleName.RouteSheet + "", ((User)Session[SessionInfo.CurrentUser]).UserID);


            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();

            sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";

            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            List<Location> oLocations = new List<Location>();
            ViewBag.Locations = oLocations;

            #region Order Type
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();
            #endregion

            ViewBag.OrderType = oDUOrderSetups;
            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState));
            ViewBag.ProductionScheduleStatus = EnumObject.jGets(typeof(EnumProductionScheduleStatus));
            string sTempString = 0 + "~" + DateTime.Today.ToString("dd MMM yyyy") + "~" + DateTime.Today.ToString("dd MMM yyyy") + "~" + "" + "~" + "" + "~" + "" + "~" + 0 + "~" + "" + "~"
                + (int)EnumRSState.YarnOut + "~" + 5 + "~" + dStartDate + "~" + dEndDate + "~" + "" + "~" + 0 + "~" + false + "~" + false + "~" + "" + "~" + "" + "~" + nProductID;
            ViewBag.sTempString = sTempString;
            return View(_oRouteSheets);
        }

        [HttpGet]
        public JsonResult RouteSheetYarnOut_Multi(string sTemp)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                if (string.IsNullOrEmpty(sTemp)) throw new Exception("Please select a valid routesheet.");
                oRouteSheets = oDUSoftWinding.YarnOut_Multi(sTemp, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheets = new List<RouteSheet>();
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
                oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRouteSheetForYarnOut(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.RouteSheetNo)) ? "" : oRouteSheet.RouteSheetNo.Trim();
                string sSQL = "Select top(100)* from View_RouteSheet Where RouteSheetID<>0 And RSState= "+(int)EnumRSState.InFloor;
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet=new RouteSheet();
                _oRouteSheets = new List<RouteSheet>();
                oRouteSheet.ErrorMessage=ex.Message;
                _oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<RouteSheet> GetsYarnOuts(List<RouteSheet> oRouteSheets)
        {
            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<RouteSheetYarnOut>();
            string sRSIDs = string.Join(",", _oRouteSheets.Select(x => x.RouteSheetID).ToList());
            if (!string.IsNullOrEmpty(sRSIDs))
            {
                sRSIDs = "Select * from View_RouteSheet_YarnOut Where RouteSheetID<>0 and RouteSheetID in (" + sRSIDs + ")";
                oRouteSheetYarnOuts = ESimSol.BusinessObjects.RouteSheetYarnOut.Gets(sRSIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oRouteSheets.ForEach(x =>
                {
                    x.RecipeByName = "";
                    x.DBServerDateTime = DateTime.MinValue;
                    if (oRouteSheetYarnOuts.FirstOrDefault() != null && oRouteSheetYarnOuts.FirstOrDefault().RouteSheetID > 0 && oRouteSheetYarnOuts.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                    {
                        if (x.RSState >= EnumRSState.YarnOut)
                        {
                            x.RecipeByName = oRouteSheetYarnOuts.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().UserName;
                            x.DBServerDateTime = oRouteSheetYarnOuts.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().EventTime;
                        }
                     
                    }
                  
                });
            }


            return _oRouteSheets;
        }

        [HttpPost]
        public JsonResult GetsRouteSheetByNo_All(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.RouteSheetNo)) ? "" : oRouteSheet.RouteSheetNo.Trim();
                string sOrderNo = (string.IsNullOrEmpty(oRouteSheet.OrderNo)) ? "" : oRouteSheet.OrderNo.Trim();

                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 ";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                //if (sOrderNo != "") sSQL = sSQL + " And OrderNo Like '%" + sOrderNo + "%'";
                if (sOrderNo != "") sSQL = sSQL + " And  RouteSheetID in (Select RouteSheetID from view_RouteSheetDO where OrderNo like '%" + sOrderNo + "%')";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oRouteSheets.Count > 0)
                {
                    _oRouteSheets = GetsYarnOuts(_oRouteSheets);
                }

            
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                _oRouteSheets = new List<RouteSheet>();
                oRouteSheet.ErrorMessage = ex.Message;
                _oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsYarnOut(RouteSheetYarnOut oRouteSheetYarnOut)
        {
            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<RouteSheetYarnOut>();
            try
            {

                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheetYarnOut.Params.Split('~')[0])) ? "" : oRouteSheetYarnOut.Params.Split('~')[0].Trim();
                int nWorkingUnitID = Convert.ToInt32(oRouteSheetYarnOut.Params.Split('~')[1]);
                string sProductIDs = oRouteSheetYarnOut.Params.Split('~')[2];
                string sLotIDs = oRouteSheetYarnOut.Params.Split('~')[3].Trim();
                short nComparator = (short)(EnumCompareOperator)Convert.ToInt16(oRouteSheetYarnOut.Params.Split('~')[4]);
                DateTime dtFrom = Convert.ToDateTime(oRouteSheetYarnOut.Params.Split('~')[5]);
                DateTime dtTo = Convert.ToDateTime(oRouteSheetYarnOut.Params.Split('~')[6]);

                string sSQL = "Select * from View_RouteSheet_YarnOut Where RouteSheetID<>0 ";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                if (nWorkingUnitID > 0) sSQL = sSQL + " And WorkingUnitID = " + nWorkingUnitID + "";
                if (!string.IsNullOrEmpty(sProductIDs)) sSQL = sSQL + " And ProductID_Raw In (" + sProductIDs.Trim() + ")";
                if (!string.IsNullOrEmpty(sLotIDs)) sSQL = sSQL + " And LotID In (" + sLotIDs.Trim() + ")";
                if (nComparator > 0) sSQL = sSQL + "And " + Global.DateSQLGenerator("EventTime", nComparator, dtFrom, dtTo, false);

                oRouteSheetYarnOuts = ESimSol.BusinessObjects.RouteSheetYarnOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetYarnOuts = new List<RouteSheetYarnOut>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetYarnOuts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsYarnOutAdv(RouteSheetYarnOut oRouteSheetYarnOut)
        {
            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<RouteSheetYarnOut>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheetYarnOut.Params.Split('~')[0])) ? "" : oRouteSheetYarnOut.Params.Split('~')[0].Trim();
                int nWorkingUnitID = Convert.ToInt32(oRouteSheetYarnOut.Params.Split('~')[1]);
                string sProductIDs = oRouteSheetYarnOut.Params.Split('~')[2];
                string sLotIDs = oRouteSheetYarnOut.Params.Split('~')[3].Trim();
                short nComparator = (short)(EnumCompareOperator)Convert.ToInt16(oRouteSheetYarnOut.Params.Split('~')[4]);
                DateTime dtFrom = Convert.ToDateTime(oRouteSheetYarnOut.Params.Split('~')[5]);
                DateTime dtTo = Convert.ToDateTime(oRouteSheetYarnOut.Params.Split('~')[6]);

                string sSQL = "Select * from View_RouteSheet_YarnOut Where RouteSheetID<>0 ";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                if (nWorkingUnitID > 0) sSQL = sSQL + " And WorkingUnitID = " + nWorkingUnitID + "";
                if (!string.IsNullOrEmpty(sProductIDs)) sSQL = sSQL + " And ProductID_Raw In (" + sProductIDs.Trim() + ")";
                if (!string.IsNullOrEmpty(sLotIDs)) sSQL = sSQL + " And LotID In (" + sLotIDs.Trim() + ")";
                if (nComparator > 0) //sSQL = sSQL + "And " + Global.DateSQLGenerator("EventTime", nComparator, dtFrom, dtTo, true);
                {
                    if (nComparator == (int)EnumCompareOperator.EqualTo)
                    {
                        dtTo = dtFrom;
                        sSQL += " AND EventTime>='" + dtFrom.ToString("dd MMM yyyy HH:00") + "' AND EventTime<'" + dtTo.ToString("dd MMM yyyy HH:00") + "'";
                    }
                    else if (nComparator == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sSQL += " AND EventTime != CONVERT(DATETIME,convert(varchar, '" + dtFrom + "',105))";
                    }
                    else if (nComparator == (int)EnumCompareOperator.GreaterThan)
                    {
                        sSQL += " AND EventTime > CONVERT(DATETIME,convert(varchar, '" + dtFrom + "',105))";
                    }
                    else if (nComparator == (int)EnumCompareOperator.SmallerThan)
                    {
                        sSQL += " AND EventTime < CONVERT(DATETIME,convert(varchar, '" + dtFrom + "',105))";
                    }
                    else if (nComparator == (int)EnumCompareOperator.Between)
                    {
                        sSQL += " AND EventTime>='" + dtFrom.ToString("dd MMM yyyy HH:00") + "' AND EventTime<'" + dtTo.ToString("dd MMM yyyy HH:00") + "'";
                    }
                    else if (nComparator == (int)EnumCompareOperator.NotBetween)
                    {
                        sSQL += " AND EventTime<='" + dtFrom.ToString("dd MMM yyyy HH:00") + "' AND EventTime>'" + dtTo.ToString("dd MMM yyyy HH:00") + "'";
                        //sSQL += " AND EventTime NOT BETWEEN CONVERT(DATETIME,convert(varchar, '" + dtFrom + "',105)) AND CONVERT(DATETIME,convert(varchar, '" + dtTo + "',105))";
                    }
                }
                oRouteSheetYarnOuts = ESimSol.BusinessObjects.RouteSheetYarnOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetYarnOuts = new List<RouteSheetYarnOut>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetYarnOuts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RouteSheetYarnOut(RouteSheet oRouteSheet)
        {
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");
            
                int nEventEmpID = 0; 
                int.TryParse(oRouteSheet.Params, out nEventEmpID);

                string ErrorMessage = RSStateValidation(oRouteSheet.RSState, EnumRSState.YarnOut);
                if (ErrorMessage != "") { throw new Exception(ErrorMessage); }

                oRouteSheet = oRouteSheet.YarnOut(nEventEmpID,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Yarn Out Muliple (Advance Search)
        public ActionResult AdvRouteSheetYarnOut()
        {
            return PartialView();
        }
        public JsonResult AdvSearch_YarnOut(string sTemp)
        {
            _oRouteSheets = new List<RouteSheet>();
            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<ESimSol.BusinessObjects.RouteSheetYarnOut>();
            List<RSRawLot> oRSRawLots = new List<ESimSol.BusinessObjects.RSRawLot>();
            try
            {
                string sSQL = GetSQL_Adavnce(sTemp);
                _oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (_oRouteSheets.Count > 0)
                {
                    _oRouteSheets = GetsYarnOuts(_oRouteSheets);
                }

                string sRSIDs = string.Join(",", _oRouteSheets.Select(x => x.RouteSheetID).ToList());
                if (!string.IsNullOrEmpty(sRSIDs))
                {
                    oRSRawLots = RSRawLot.Gets("SELECT * FROM View_RSRawLot WHERE RouteSheetID in (" + sRSIDs + ")", (int)Session[SessionInfo.currentUserID]);
                    _oRouteSheets.ForEach(x =>
                    {
                        if (oRSRawLots.FirstOrDefault() != null && oRSRawLots.FirstOrDefault().RouteSheetID > 0 && oRSRawLots.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                        {
                            x.LotNo = string.Join(",", oRSRawLots.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.LotNo).Distinct().ToList());
                        }

                    });
                }
            }
            catch (Exception e)
            {
                _oRouteSheets = new List<RouteSheet>();
                _oRouteSheets.Add(new RouteSheet() { ErrorMessage = e.Message });
            }
            var jsonResult = Json(_oRouteSheets, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = sjson = serializer.Serialize(_oRouteSheets);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL_Adavnce(string sTemp)
        {
            //PI Date
            string sRouteSheetNo = "", sOrderNo = "";
            int cboDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime StartTime = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndTime = Convert.ToDateTime(sTemp.Split('~')[2]);
            string LocationName = Convert.ToString(sTemp.Split('~')[3]);
            string MachineIds = Convert.ToString(sTemp.Split('~')[4]);
            string DODID = Convert.ToString(sTemp.Split('~')[5]);
            int MachineTypeID = Convert.ToInt16(sTemp.Split('~')[6]);
            string ScheduleStatus = sTemp.Split('~')[7];
            string sRSStatus = sTemp.Split('~')[8];
            int cboDate_RS = Convert.ToInt32(sTemp.Split('~')[9]);
            DateTime StartTime_RS = Convert.ToDateTime(sTemp.Split('~')[10]);
            DateTime EndTime_RS = Convert.ToDateTime(sTemp.Split('~')[11]);
            string BuyerIDs = Convert.ToString(sTemp.Split('~')[12]);
            int nRSShiftID = Convert.ToInt32(sTemp.Split('~')[13]);
            bool bIsInHouse = Convert.ToBoolean(sTemp.Split('~')[14]);
            bool bIsOutSide = Convert.ToBoolean(sTemp.Split('~')[15]);

            if (cboDate_RS != 5) { EndTime_RS = StartTime_RS.AddDays(1); }

            if (sTemp.Split('~').Count() > 16) 
            {
                sRouteSheetNo = Convert.ToString(sTemp.Split('~')[16]); sOrderNo = Convert.ToString(sTemp.Split('~')[17]);
            }

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sReturn1 = "Select * from View_RouteSheet_YarnOut ";
            string sReturn = "";

            #region EnumRSState & Date
            if (cboDate > 0)
            {
                if (cboDate == 1)
                {
                    EndTime = StartTime.AddDays(1);

                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.YarnOut + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')";
                    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.YarnOut + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy HH:mm") + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy HH:mm") + "')";
                }
                if (cboDate == 5)
                {
                    EndTime = EndTime.AddDays(1);
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.YarnOut + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' )";
                    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.YarnOut + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy HH:mm") + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy HH:mm") + "' )";
                    //sReturn = sReturn + " ((StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")+"'))" );
                    //  sReturn = sReturn + "(StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM"))+"')";
                }
            }
            //else
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.InFloor + ")) ";
            //}
            #endregion

            if (sRouteSheetNo != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetNo Like '%" + sRouteSheetNo + "%'"; }
            if (sOrderNo != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " OrderNo Like '%" + sOrderNo + "%'"; }

            if (!string.IsNullOrEmpty(ScheduleStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID in (Select RouteSheetID from Routesheet where DUPScheduleID>0 and DUPScheduleID in(SELECT DUPScheduleID FROM DUPSchedule WHERE ScheduleStatus IN (" + ScheduleStatus + ")) )";
                // sReturn = sReturn + " DUPScheduleID IN (SELECT DUPScheduleID FROM DUPSchedule WHERE ScheduleStatus IN (" + ScheduleStatus + "))";
            }

            if (!string.IsNullOrEmpty(sRSStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + sRSStatus + ") and EventTime>='" + StartTime_RS.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'   and EventTime<'" + EndTime_RS.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' )";
            }

            if (!string.IsNullOrEmpty(MachineIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in ( select RoutesheetID from RouteSheet where  MachineID IN ( " + MachineIds + "))";
            }
            else if (MachineTypeID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheet where  MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + MachineTypeID + "))";
            }

            if (!string.IsNullOrEmpty(DODID))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RoutesheetID in (select RoutesheetID FROM RouteSheetDO WHERE DyeingOrderDetailID IN(" + DODID + "))";
            }
            if (!string.IsNullOrEmpty(BuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID in (" + BuyerIDs + ")";
            } 
            if (nRSShiftID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RSShiftID =" + nRSShiftID;
            }
            if (bIsInHouse)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RoutesheetID IN (SELECT RouteSheetID FROM View_RouteSheetDO WHERE OrderType IN (SELECT OrderType FROM DUOrderSetup WHERE IsInHouse = 1))";
            }
            if (bIsOutSide)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RoutesheetID IN (SELECT RouteSheetID FROM View_RouteSheetDO WHERE OrderType IN (SELECT OrderType FROM DUOrderSetup WHERE IsInHouse = 0))";
            }
            

            sReturn = sReturn1 + sReturn + "";
            return sReturn;
        }

        #endregion

        #region Print (Yarn Out)
        public void ExcelYarnOut(string sIDs, double nts)
        {
            List<RouteSheetYarnOut> oRSYarnOuts = new List<RouteSheetYarnOut>();
            string sSQL = "Select * from View_RouteSheet_YarnOut Where RouteSheetID<>0 And RouteSheetID In (" + sIDs + ")";
            oRSYarnOuts = ESimSol.BusinessObjects.RouteSheetYarnOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Yarn Out");
                sheet.Name = "Yarn Out List";
                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 16; //Yarn Out Date
                sheet.Column(4).Width = 15; //RouteSheet No
                sheet.Column(5).Width = 25; //RSState
                sheet.Column(6).Width = 32; //Product Name
                sheet.Column(7).Width = 15; //Lot No
                sheet.Column(8).Width = 22; //Store
                sheet.Column(9).Width = 12; //Qty

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 9].Merge = true;
                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion
                #region Content part


                #region Column Header

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Yarn Out Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "RouteSheet No"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = "Current State"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Store Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                int nSL = 0;
                double nQty = oRSYarnOuts.Sum(x=>x.Qty);
                bool IsSpan = true;

                oRSYarnOuts=oRSYarnOuts.OrderBy(x=>x.EventTime).ToList();
                while(oRSYarnOuts.Count()>0)
                {
                    IsSpan = true;
                    var oTList=oRSYarnOuts.Where(x=>x.EventTimeStr == oRSYarnOuts.FirstOrDefault().EventTimeStr).ToList();
                    oRSYarnOuts.RemoveAll(x=>x.EventTimeStr==oTList.FirstOrDefault().EventTimeStr);

                    foreach(var oItem in oTList)
                    {
                        ++nSL;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (IsSpan)
                        {
                            IsSpan = false;
                            sheet.Cells[rowIndex, 3].Merge = true;
                            cell = sheet.Cells[rowIndex, 3];  cell.Value = oItem.EventTimeStr; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
             
                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.RouteSheetNo; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.RSStateStr; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.WUName; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.Qty; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;

                    }

                }
                #endregion

                #region Bottom Part Total

                sheet.Cells[rowIndex, 2, rowIndex, 8].Merge = true;
                cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = nQty; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
             
                #endregion
        
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=YarnOut.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public ActionResult Print_YarnOut(string sParams)
        {
            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<ESimSol.BusinessObjects.RouteSheetYarnOut>();
            string sRouteSheetNo = "";
            int nWorkingUnitID = 0;
            string sProductIDs = "";
            short nComparator = 0;
            string sLotIDs = "";
            DateTime dtFrom = DateTime.Now;
            DateTime dtTo = DateTime.Now;
            string sDateRange = "";
            if (!String.IsNullOrEmpty(sParams))
            {

                sRouteSheetNo = (string.IsNullOrEmpty(sParams) ? "" : sParams.Split('~')[0].Trim());
                nWorkingUnitID = Convert.ToInt32(sParams.Split('~')[1]);
                sProductIDs = sParams.Split('~')[2];
                sLotIDs = sParams.Split('~')[3].Trim();
                nComparator = (short)(EnumCompareOperator)Convert.ToInt16(sParams.Split('~')[4]);
                dtFrom = Convert.ToDateTime(sParams.Split('~')[5]);
                dtTo = Convert.ToDateTime(sParams.Split('~')[6]);
            }
            else
            {
                nComparator = (int)EnumCompareOperator.EqualTo;
            }
            if (nComparator == (int)EnumCompareOperator.EqualTo)
            {
                sDateRange = "Date: " + dtFrom.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = "Date: " + dtFrom.ToString("dd MMM yyyy") + " To " + dtTo.ToString("dd MMM yyyy");
            }

            string sSQL = "Select * from View_RouteSheet_YarnOut Where RouteSheetID<>0 ";
            if (nWorkingUnitID > 0) sSQL = sSQL + " And WorkingUnitID = " + nWorkingUnitID + "";
            if (!string.IsNullOrEmpty(sProductIDs)) sSQL = sSQL + " And ProductID_Raw In (" + sProductIDs.Trim() + ")";
            if (!string.IsNullOrEmpty(sLotIDs)) sSQL = sSQL + " And LotID In (" + sLotIDs.Trim() + ")";
            if (nComparator > 0) sSQL = sSQL + "And " + Global.DateSQLGenerator("EventTime", nComparator, dtFrom, dtTo, false);
            oRouteSheetYarnOuts = ESimSol.BusinessObjects.RouteSheetYarnOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            rptRouteSheetYarnIssue oReport = new rptRouteSheetYarnIssue();
            byte[] abytes = oReport.PrepareReport(oRouteSheetYarnOuts,oRouteSheetSetup, oCompanys.First(), oBusinessUnit, sDateRange);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Yarn Out Multiple
        private string GetSQL_YarnOut(string sTemp)
        {
            //PI Date
            string sRouteSheetNo = "", sOrderNo = "";
            int nProductID = 0;
            int cboDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime StartTime = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndTime = Convert.ToDateTime(sTemp.Split('~')[2]);
            string LocationName = Convert.ToString(sTemp.Split('~')[3]);
            string MachineIds = Convert.ToString(sTemp.Split('~')[4]);
            string DODID = Convert.ToString(sTemp.Split('~')[5]);
            int MachineTypeID = Convert.ToInt16(sTemp.Split('~')[6]);
            string ScheduleStatus = sTemp.Split('~')[7];
            string sRSStatus = sTemp.Split('~')[8];
            int cboDate_RS = Convert.ToInt32(sTemp.Split('~')[9]);
            DateTime StartTime_RS = Convert.ToDateTime(sTemp.Split('~')[10]);
            DateTime EndTime_RS = Convert.ToDateTime(sTemp.Split('~')[11]);
            string BuyerIDs = Convert.ToString(sTemp.Split('~')[12]);
            int nRSShiftID = Convert.ToInt32(sTemp.Split('~')[13]);
            bool bIsInHouse = Convert.ToBoolean(sTemp.Split('~')[14]);
            bool bIsOutSide = Convert.ToBoolean(sTemp.Split('~')[15]);

            if (cboDate_RS != 5) { EndTime_RS = StartTime_RS.AddDays(1); }

            if (sTemp.Split('~').Count() > 16) // 14,15: IsInHouse, IsOutSide
            {
                sRouteSheetNo = Convert.ToString(sTemp.Split('~')[16]); sOrderNo = Convert.ToString(sTemp.Split('~')[17]);
            }
            if (sTemp.Split('~').Count() > 18)
                nProductID = Convert.ToInt32(sTemp.Split('~')[18]);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sReturn1 = "Select * from View_RouteSheet_YarnOut ";
            string sReturn = "";

            #region EnumRSState & Date
            if (cboDate > 0)
            {
                if (cboDate == 1)
                {
                    EndTime = StartTime.AddDays(1);

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.YarnOut + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy HH:mm") + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy HH:mm") + "')";
                }
                if (cboDate == 5)
                {
                    EndTime = EndTime.AddDays(1);
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.YarnOut + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy HH:mm") + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy HH:mm") + "' )";
                    //sReturn = sReturn + " ((StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")+"'))" );
                    //  sReturn = sReturn + "(StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM"))+"')";
                }
            }
            //else
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.InFloor + ")) ";
            //}

            #endregion

            if (sRouteSheetNo != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetNo Like '%" + sRouteSheetNo + "%'"; }
            //if (sOrderNo != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " OrderNo Like '%" + sOrderNo + "%'"; }
            if (sOrderNo != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from view_RouteSheetDO where OrderNo like '%" + sOrderNo + "%')"; }

            if (!string.IsNullOrEmpty(ScheduleStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID in (Select RouteSheetID from Routesheet where DUPScheduleID>0 and DUPScheduleID in(SELECT DUPScheduleID FROM DUPSchedule WHERE ScheduleStatus IN (" + ScheduleStatus + ")) )";
                // sReturn = sReturn + " DUPScheduleID IN (SELECT DUPScheduleID FROM DUPSchedule WHERE ScheduleStatus IN (" + ScheduleStatus + "))";
            }

            if (!string.IsNullOrEmpty(sRSStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + sRSStatus + ") and EventTime>='" + StartTime_RS.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'   and EventTime<'" + EndTime_RS.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' )";
            }

            if (!string.IsNullOrEmpty(MachineIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in ( select RoutesheetID from RouteSheet where  MachineID IN ( " + MachineIds + "))";
            }
            else if (MachineTypeID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheet where  MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + MachineTypeID + "))";
            }

            if (!string.IsNullOrEmpty(DODID))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RoutesheetID in (select RoutesheetID FROM RouteSheetDO WHERE DyeingOrderDetailID IN(" + DODID + "))";
            }
            if (!string.IsNullOrEmpty(BuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheet where ContractorID in (" + BuyerIDs + "))";
            }
            if (nRSShiftID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheet where  RSShiftID =" + nRSShiftID+")";
            }
            if (bIsInHouse)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RoutesheetID IN (SELECT RouteSheetID FROM View_RouteSheetDO WHERE OrderType IN (SELECT OrderType FROM DUOrderSetup WHERE IsInHouse = 1))";
            }
            if (bIsOutSide)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RoutesheetID IN (SELECT RouteSheetID FROM View_RouteSheetDO WHERE OrderType IN (SELECT OrderType FROM DUOrderSetup WHERE IsInHouse = 0))";
            }
            if (nProductID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID_Raw = " + nProductID;
            }
            sReturn = sReturn1 + sReturn + "";
            return sReturn;
        }
        public ActionResult Print_YarnOut_Multiple(string sParams)
        {

            List<RSRawLot> oRSRawLots = new List<RSRawLot>();

            List<RouteSheetYarnOut> oRouteSheetYarnOuts = new List<ESimSol.BusinessObjects.RouteSheetYarnOut>();
            int nWorkingUnitID = 0;
            string sProductIDs = "";
            short nComparator = 0;
            string sLotIDs = "";
            DateTime dtFrom = DateTime.Now;
            DateTime dtTo = DateTime.Now;
            string sDateRange = "";
            if (!String.IsNullOrEmpty(sParams))
            {
                nComparator = (short)(EnumCompareOperator)Convert.ToInt16(sParams.Split('~')[0]);
                dtFrom = Convert.ToDateTime(sParams.Split('~')[1]);
                dtTo = Convert.ToDateTime(sParams.Split('~')[2]);
            }
            else
            {
                nComparator = (int)EnumCompareOperator.EqualTo;
            }

            if (nComparator == (int)EnumCompareOperator.EqualTo)
            {
                sDateRange = "Date: " + dtFrom.ToString("dd MMM yyyy hh:mm tt");
            }
            else
            {
                sDateRange = "Date: " + dtFrom.ToString("dd MMM yyyy hh:mm tt") + " To " + dtTo.ToString("dd MMM yyyy hh:mm tt");
            }

            string sSQL = GetSQL_YarnOut(sParams);
            //if (nWorkingUnitID > 0) sSQL = sSQL + " And WorkingUnitID = " + nWorkingUnitID + "";
            //if (!string.IsNullOrEmpty(sProductIDs)) sSQL = sSQL + " And ProductID_Raw In (" + sProductIDs.Trim() + ")";
            //if (!string.IsNullOrEmpty(sLotIDs)) sSQL = sSQL + " And LotID In (" + sLotIDs.Trim() + ")";
            //if (nComparator > 0) sSQL = sSQL + "And " + Global.DateSQLGenerator("EventTime", nComparator, dtFrom, dtTo, false);
            oRouteSheetYarnOuts = ESimSol.BusinessObjects.RouteSheetYarnOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sRSIDs = string.Join(",", oRouteSheetYarnOuts.Select(x => x.RouteSheetID).ToList());
            if (!string.IsNullOrEmpty(sRSIDs))
            {
                oRSRawLots = RSRawLot.Gets("SELECT * FROM View_RSRawLot WHERE RouteSheetID in (" + sRSIDs + ")", (int)Session[SessionInfo.currentUserID]);
                oRouteSheetYarnOuts.ForEach(x =>
                {
                    if (oRSRawLots.FirstOrDefault() != null && oRSRawLots.FirstOrDefault().RouteSheetID > 0 && oRSRawLots.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                    {
                        x.LotNo = string.Join(",", oRSRawLots.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.LotNo).Distinct().ToList());
                    }

                });
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            rptRouteSheetYarnIssue oReport = new rptRouteSheetYarnIssue();
            byte[] abytes = oReport.PrepareReport(oRouteSheetYarnOuts, oRouteSheetSetup ,oCompanys.First(), oBusinessUnit, sDateRange);
            return File(abytes, "application/pdf");
        }
        public void ExcelYarnOut_Multiple(string sParams, double nts)
        {
            List<RouteSheetYarnOut> oRSYarnOuts = new List<RouteSheetYarnOut>();
            //string sSQL = "Select * from View_RouteSheet_YarnOut Where RouteSheetID<>0 And RouteSheetID In (" + sIDs + ")";
            string sSQL = GetSQL_YarnOut(sParams);
            oRSYarnOuts = ESimSol.BusinessObjects.RouteSheetYarnOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            int rowIndex = 2, nEndColumn=0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Yarn Out");
                sheet.Name = "Yarn Out List";

                //#SL No    #Yarn Out Date      #Order No       #NAME OF RAW YARN       #Lot No         #Dye Line No    #RSState      #Qty(LBS)         #Bags/Hank         #Dyeing Type         #Store

                int nColumn = 2;
                sheet.Column(nColumn++).Width = 8; //SL
                sheet.Column(nColumn++).Width = 20; //Yarn Out Date
                sheet.Column(nColumn++).Width = 15; //Order No
                sheet.Column(nColumn++).Width = 32; //Product Name
                sheet.Column(nColumn++).Width = 15; //Lot No

                sheet.Column(nColumn++).Width = 15; //RouteSheet No
                sheet.Column(nColumn++).Width = 25; //RSState

                sheet.Column(nColumn++).Width = 12; //Qty
                sheet.Column(nColumn++).Width = 12; //Cone
                sheet.Column(nColumn++).Width = 12; //Dyeing Type
                sheet.Column(nColumn++).Width = 35; //Store

                nEndColumn = nColumn;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nEndColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2, rowIndex, nEndColumn]; cell.Value = "ISSUED SLIP"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Content part


                #region Column Header

                //#SL No    #Yarn Out Date      #Order No       #NAME OF RAW YARN       #Lot No         #Dye Line No    #RSState      #Qty(LBS)         #Bags/Hank         #Dyeing Type         #Store
                nColumn = 2;
                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Yarn Out Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "NAME OF RAW YARN"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Dye Lot No"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Current State"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Qty(" + oRouteSheetSetup.MUnit + ")"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Cone/Hank"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Dyeing Type"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = "Store Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                int nSL = 0;
                double nQty = oRSYarnOuts.Sum(x => x.Qty);
                double nNoOfHanksCone = oRSYarnOuts.Sum(x => x.NoOfHanksCone);
                bool IsSpan = true;

                oRSYarnOuts = oRSYarnOuts.OrderBy(x => x.EventTime).ToList();
                while (oRSYarnOuts.Count() > 0)
                {
                    IsSpan = true;
                    var oTList = oRSYarnOuts.Where(x => x.EventTimeStr == oRSYarnOuts.FirstOrDefault().EventTimeStr).ToList();
                    oRSYarnOuts.RemoveAll(x => x.EventTimeStr == oTList.FirstOrDefault().EventTimeStr);

                    foreach (var oItem in oTList)
                    {
                        ++nSL;

                        //#SL No    #Yarn Out Date      #Order No       #NAME OF RAW YARN       #Lot No         #Dye Line No    #RSState      #Qty(LBS)         #Bags/Hank         #Dyeing Type         #Store
                        nColumn = 2;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = nSL; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //if (IsSpan) // will be removed soon
                        //{
                        //    IsSpan = false;
                            sheet.Cells[rowIndex, nColumn].Merge = true;
                            cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.EventTimeStr; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //}

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.OrderNoFull; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.RouteSheetNo; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.RSStateStr; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.NoOfHanksCone; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.DyeingType; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumn++]; cell.Value = oItem.WUName; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                    }
                }
                #endregion

                #region Bottom Part Total
                sheet.Cells[rowIndex, 2, rowIndex, 8].Merge = true;
                cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = nQty; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = nNoOfHanksCone; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;  cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=YarnOut.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #endregion

        #region Dye Chemical Out
        [HttpPost]
        public JsonResult GetRouteSheetDetails(RouteSheet oRS)
        {
            int nOutState = 0;
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
            List<RouteSheetDetail> oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
            try
            {
                if (oRS.RouteSheetID <= 0)
                    throw new Exception("Please select a valid items.");
                oRouteSheetDetails = RouteSheetDetail.Gets(oRS.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE ISNULL(SequenceNo,0) > 0 AND RouteSheetID =" + oRS.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (var oItem in oRouteSheetDetails) 
                {
                    oRSDetailAdditonals.ForEach(additon =>
                    {
                        if (additon.RouteSheetDetailID == oItem.RouteSheetDetailID)
                        {
                            oItem.RSDetailAdditonals.Add(additon);
                        }
                    });
                }

                //if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 0); }
                nOutState = oRouteSheetDetails.Where(x => x.AddOneLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 1); }
                nOutState = oRouteSheetDetails.Where(x => x.AddTwoLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 2); }
                nOutState = oRouteSheetDetails.Where(x => x.AddThreeLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 3); }
                   

                foreach (RouteSheetDetail oItem in oRouteSheetDetails)
                {
                    if (oItem.IsDyesChemical)
                    {

                        if (oItem.TotalQtyLotID <= 0)
                        {
                            oItem.TotalQtyLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID <= 0 && oItem.AddOneQty>0)
                        {
                            oItem.AddOneLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID > 0 && oItem.AddTwoLotID <= 0 && oItem.AddTwoQty > 0)
                        {
                            oItem.AddTwoLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID > 0 && oItem.AddTwoLotID > 0 && oItem.AddThreeLotID <= 0 && oItem.AddThreeQty>0)
                        {
                            oItem.AddThreeLotNo = oItem.SuggestLotNo;
                        }
                        oRouteSheetDetails_Ret.Add(oItem);
                    }
                }
                oRouteSheetDetails_Ret = oRouteSheetDetails_Ret.OrderBy(x => x.Sequence).ThenBy(x=>x.RouteSheetDetailID) .ToList();
            }
            catch (Exception ex)
            {
                oRouteSheetDetails_Ret = new List<RouteSheetDetail>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetails_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRouteSheetDetails_V2(RouteSheet oRS)
        {
            int nOutState = 0;
            string sWUIDs=""; 
            string sProductIDs="";
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
            List<RouteSheetDetail> oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
            List<Lot> oLots = new List<Lot>();
            try
            {
                if (oRS.RouteSheetID <= 0)
                    throw new Exception("Please select a valid items.");
                oRouteSheetDetails = RouteSheetDetail.Gets(oRS.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oRouteSheetDetails = this.GetProcessWiseRSList(oRouteSheetDetails);
                
                oRSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE ISNULL(SequenceNo,0) > 0 AND InOutType = "+ (int)EnumInOutType.Disburse +" AND RouteSheetID =" + oRS.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sWUIDs = string.Join(",", oRouteSheetDetails.Where(p => p.IsDyesChemical == true).Select(x => x.WUID).Distinct().ToList());
                sProductIDs = string.Join(",", oRouteSheetDetails.Where(p => p.IsDyesChemical == true).Select(x => x.ProcessID).Distinct().ToList());

                if (string.IsNullOrEmpty(sWUIDs))
                    sWUIDs = "0";
                if (string.IsNullOrEmpty(sProductIDs))
                    sProductIDs = "0";

                oLots = Lot.Gets("Select * from view_Lot where  isnull(LotStatus,0) in (" + (int)EnumLotStatus.Open + "," + (int)EnumLotStatus.Running + ")  and WorkingUnitID in (" + sWUIDs + ") and ProductID in (" + sProductIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (var oItem in oRouteSheetDetails)
                {
                    oRSDetailAdditonals.ForEach(additon =>
                    {
                        if (additon.RouteSheetDetailID == oItem.RouteSheetDetailID)
                        {
                            oItem.RSDetailAdditonals.Add(additon);
                        }
                    });
                }

                //if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 0); }
                nOutState = oRouteSheetDetails.Where(x => x.AddOneLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 1); }
                nOutState = oRouteSheetDetails.Where(x => x.AddTwoLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 2); }
                nOutState = oRouteSheetDetails.Where(x => x.AddThreeLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 3); }

                foreach (RouteSheetDetail oItem in oRouteSheetDetails)
                {
                    if (oItem.IsDyesChemical)
                    {
                        if (oItem.TotalQtyLotID <= 0 && oItem.SuggestLotID <= 0) 
                        {
                            var oLot = oLots.Where(x => x.ProductID == oItem.ProcessID && x.WorkingUnitID == oItem.WUID).FirstOrDefault();

                            if (oLot != null) 
                            {
                                oItem.SuggestLotID = oLot.LotID;
                                oItem.SuggestLotNo = oLot.LotNo;
                                oItem.Balance = oLot.Balance;
                            }
                        }
                       
                        if (oItem.TotalQtyLotID <= 0)
                        {
                            oItem.TotalQtyLotNo = oItem.SuggestLotNo;
                           // oItem.TotalQtyLotID = oItem.SuggestLotID;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID <= 0 && oItem.AddOneQty > 0)
                        {
                            oItem.AddOneLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID > 0 && oItem.AddTwoLotID <= 0 && oItem.AddTwoQty > 0)
                        {
                            oItem.AddTwoLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID > 0 && oItem.AddTwoLotID > 0 && oItem.AddThreeLotID <= 0 && oItem.AddThreeQty > 0)
                        {
                            oItem.AddThreeLotNo = oItem.SuggestLotNo;
                        }
                    }
                    oRouteSheetDetails_Ret.Add(oItem);
                }
              //  oRouteSheetDetails_Ret = oRouteSheetDetails_Ret.OrderBy(x => x.Sequence).ThenBy(x => x.RouteSheetDetailID).ToList();
            }
            catch (Exception ex)
            {
                oRouteSheetDetails_Ret = new List<RouteSheetDetail>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetails_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRouteSheetDetails_Return(RouteSheet oRS)
        {
            int nOutState = 0;
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
            List<RouteSheetDetail> oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
            try
            {
                if (oRS.RouteSheetID <= 0)
                    throw new Exception("Please select a valid items.");
                oRouteSheetDetails = RouteSheetDetail.Gets(oRS.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRouteSheetDetails = this.GetProcessWiseRSList(oRouteSheetDetails);
                
                oRSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE ISNULL(SequenceNo,0) > 0 AND InOutType = " + (int)EnumInOutType.Receive + " AND RouteSheetID =" + oRS.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (var oItem in oRouteSheetDetails)
                {
                    oRSDetailAdditonals.ForEach(additon =>
                    {
                        if (additon.RouteSheetDetailID == oItem.RouteSheetDetailID)
                        {
                            oItem.RSDetailAdditonals.Add(additon);
                        }
                    });
                }

                //if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 0); }
                nOutState = oRouteSheetDetails.Where(x => x.AddOneLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 1); }
                nOutState = oRouteSheetDetails.Where(x => x.AddTwoLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 2); }
                nOutState = oRouteSheetDetails.Where(x => x.AddThreeLotID > 0).Count();
                if (nOutState > 0) { oRouteSheetDetails.ForEach(o => o.OutState = 3); }

                foreach (RouteSheetDetail oItem in oRouteSheetDetails)
                {
                    if (oItem.IsDyesChemical)
                    {
                        if (oItem.TotalQtyLotID <= 0)
                        {
                            oItem.TotalQtyLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID <= 0 && oItem.AddOneQty > 0)
                        {
                            oItem.AddOneLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID > 0 && oItem.AddTwoLotID <= 0 && oItem.AddTwoQty > 0)
                        {
                            oItem.AddTwoLotNo = oItem.SuggestLotNo;
                        }
                        else if (oItem.TotalQtyLotID > 0 && oItem.AddOneLotID > 0 && oItem.AddTwoLotID > 0 && oItem.AddThreeLotID <= 0 && oItem.AddThreeQty > 0)
                        {
                            oItem.AddThreeLotNo = oItem.SuggestLotNo;
                        }
                        //oRouteSheetDetails_Ret.Add(oItem);
                    }
                    oRouteSheetDetails_Ret.Add(oItem);
                }
                oRouteSheetDetails_Ret = oRouteSheetDetails_Ret.OrderBy(x => x.Sequence).ThenBy(x => x.RouteSheetDetailID).ToList();
            }
            catch (Exception ex)
            {
                oRouteSheetDetails_Ret = new List<RouteSheetDetail>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetails_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region Get Process List [parent - child]
        private List<RouteSheetDetail> GetProcessWiseRSList(List<RouteSheetDetail> oRouteSheetDetails)
        {
            RouteSheetDetail oRouteSheetDetail = new RouteSheetDetail();
            List<RouteSheetDetail> oResults = new List<RouteSheetDetail>();
            if (oRouteSheetDetails.Count() > 0)
            {
                var root = oRouteSheetDetails.Where(x => x.ParentID == 0).FirstOrDefault();
                oResults.Add(root);

                this.AddChildProcess(ref oResults, root.RouteSheetDetailID, oRouteSheetDetails);
            }
            return oResults;
        }
        public void AddChildProcess(ref List<RouteSheetDetail> oResults, int nParentID, List<RouteSheetDetail> oRouteSheetDetails)
        {
            var children = oRouteSheetDetails.Where(x => x.ParentID == nParentID).ToList();
            if (children.Count() > 0 && oRouteSheetDetails.Count() > 0)
            {
                oRouteSheetDetails.RemoveAll(x => x.ParentID == nParentID);
                foreach (RouteSheetDetail oItem in children)
                {
                    oResults.Add(oItem);
                    this.AddChildProcess(ref oResults, oItem.RouteSheetDetailID, oRouteSheetDetails);
                }
            }
        }
        #endregion


        public ActionResult ViewRouteSheetDyeChemicalOut( int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((short)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            //string sSQL = "Select * from View_WorkingUnit Where  IsActive=1 And IsStore=1 And BUID=" + oBusinessUnit.BusinessUnitID;
            //oWorkingUnits = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oWorkingUnits = WorkingUnit.GetsPermittedStore(oBusinessUnit.BusinessUnitID, EnumModuleName.RouteSheetDetail, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = oWorkingUnits;

            //List<Employee> oEmployees = new List<Employee>();
            //oEmployees = Employee.Gets(EnumEmployeeType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Employees = oEmployees;
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID); ;
            return View(_oRouteSheets);
        }

        public ActionResult ViewRouteSheetDyeChemicalOut_V2(int menuid)
        {
          
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((short)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            //string sSQL = "Select * from View_WorkingUnit Where  IsActive=1 And IsStore=1 And BUID=" + oBusinessUnit.BusinessUnitID;
            //oWorkingUnits = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oWorkingUnits = WorkingUnit.GetsPermittedStore(oBusinessUnit.BusinessUnitID, EnumModuleName.RouteSheetDetail, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = oWorkingUnits;

            //oWorkingUnits = new List<WorkingUnit>();
            //oWorkingUnits = WorkingUnit.GetsPermittedStore(oBusinessUnit.BusinessUnitID, EnumModuleName.RouteSheetDetail, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            //ViewBag.WorkingUnitsDC = oWorkingUnits;

            //List<Employee> oEmployees = new List<Employee>();
            //oEmployees = Employee.Gets(EnumEmployeeType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Employees = oEmployees;
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID); ;
            return View(_oRouteSheets);
        }
        [HttpPost]
        public JsonResult GetsRouteSheetForDyeChemicalOut(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.RouteSheetNo)) ? "" : oRouteSheet.RouteSheetNo.Trim();
                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState= " + (int)EnumRSState.YarnOut;
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                _oRouteSheets = new List<RouteSheet>();
                oRouteSheet.ErrorMessage = ex.Message;
                _oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDyeChemicalOut(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                //string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[0])) ? "" : oRouteSheet.Params.Split('~')[0].Trim();
                //bool IsdtSearch = Convert.ToBoolean(oRouteSheet.Params.Split('~')[1]);
                //DateTime dtFrom = Convert.ToDateTime(oRouteSheet.Params.Split('~')[2]);
                //DateTime dtTo = Convert.ToDateTime(oRouteSheet.Params.Split('~')[3]);

                string sSQL = "Select top(150)* from View_RouteSheet Where RouteSheetID<>0 And RSState not In (" + (int)EnumRSState.LotReturn + "," + (int)EnumRSState.Initialized + "," + (int)EnumRSState.InLab + "," + (int)EnumRSState.InFloor + ")";
                if (oRouteSheet.RouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + oRouteSheet.RouteSheetNo + "%'";
                if (oRouteSheet.RouteSheetID > 0) sSQL = sSQL + " And RouteSheetID = " + oRouteSheet.RouteSheetID;
                //if (IsdtSearch) sSQL = sSQL + " And CONVERT(Date,RouteSheetDate) Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'";

                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheets = new List<RouteSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWaitingForOutRS(string tsv)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                string sSQL = "select top(50)* from View_RouteSheet where RSState>=" + (int)EnumRSState.YarnOut + " and RouteSheetid in (select distinct(RouteSheetid) from RouteSheetHistory where isnull(RSState,0)<>5) order by RouteSheetid desc";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheets = new List<RouteSheet>();
                RouteSheet oRS = new RouteSheet();
                oRS.ErrorMessage = ex.Message;
                _oRouteSheets.Add(oRS);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RouteSheetDyeChemicalOut(RouteSheetDetail oRSDetail)
        {
            try
            {
                if (oRSDetail.RouteSheetDetailID <= 0) throw new Exception("Please select a valid routesheet.");

                oRSDetail = oRSDetail.DyeChemicalOut(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSDetail = new RouteSheetDetail();
                oRSDetail.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RouteSheetDyeChemicalOutAll(List<RouteSheetDetail> oRouteSheetDetails)
        {
            List<RouteSheetDetail>  oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
            try
            {
                oRouteSheetDetails_Ret = RouteSheetDetail.DyeChemicalOut_All(oRouteSheetDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetDetail = new RouteSheetDetail();
                _oRouteSheetDetail.ErrorMessage = ex.Message;
                oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
                oRouteSheetDetails_Ret.Add(_oRouteSheetDetail);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetails_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      
        [HttpPost]
        public JsonResult RouteSheetDyeChemicalOutAll_V2(RSDetailAdditonal oRSDetailAdditonal)
        {
            List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
            List<RouteSheetDetail> oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
            RSDetailAdditonal oRSDetailAdd = new RSDetailAdditonal();
            try
            {
                if (oRSDetailAdditonal.SequenceNo == 0)
                {
                    string sSQL = "Select * from View_RouteSheetDetail Where IsDyesChemical=1 and RouteSheetID=" + oRSDetailAdditonal.RouteSheetID + " Order By Sequence";
                    oRouteSheetDetails_Ret = RouteSheetDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach(RouteSheetDetail oitem in  oRouteSheetDetails_Ret)
                    {
                        if (oitem.TotalQtyLotID <= 0)
                        {
                            oRSDetailAdd = new RSDetailAdditonal();
                            oRSDetailAdd.RouteSheetID = oitem.RouteSheetID;
                            oRSDetailAdd.RouteSheetDetailID = oitem.RouteSheetDetailID;
                            oRSDetailAdd.SequenceNo = 0;
                            oRSDetailAdd.LotID = oitem.SuggestLotID;
                            oRSDetailAdd.ApproveDate = oRSDetailAdditonal.ApproveDate;
                            oRSDetailAdditonals.Add(oRSDetailAdd);
                        }
                    }

                }
                else
                {
                    oRSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE isnull(ApprovedByID,0)=0 and RouteSheetid=" + oRSDetailAdditonal.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                  
                }
                if (oRSDetailAdditonals.Count > 0)
                {
                    oRouteSheetDetails_Ret = RouteSheetDetail.DyeChemicalOut_All_V2(oRSDetailAdditonals, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oRouteSheetDetail = new RouteSheetDetail();
                _oRouteSheetDetail.ErrorMessage = ex.Message;
                oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
                oRouteSheetDetails_Ret.Add(_oRouteSheetDetail);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetails_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    
        [HttpPost]
        public JsonResult RouteSheetDyeChemicalOutByID(RSDetailAdditonal oRSDetailAdditonal)
        {
            List<RSDetailAdditonal> oRSDetailAdditonals = new List<RSDetailAdditonal>();
            List<RouteSheetDetail> oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
            RSDetailAdditonal oRSDetailAdd = new RSDetailAdditonal();
            try
            {
                if (oRSDetailAdditonal.SequenceNo == 0)
                {
                    string sSQL = "Select * from View_RouteSheetDetail Where IsDyesChemical=1 and RouteSheetDetailID IN (" + oRSDetailAdditonal.ErrorMessage + ")";
                  //  string sSQL = "Select * from View_RouteSheetDetail Where IsDyesChemical=1 and RouteSheetID=" + oRSDetailAdditonal.RouteSheetID + " Order By Sequence";
                    oRouteSheetDetails_Ret = RouteSheetDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (RouteSheetDetail oitem in oRouteSheetDetails_Ret)
                    {
                        if (oitem.TotalQtyLotID <= 0)
                        {
                            oRSDetailAdd = new RSDetailAdditonal();
                            oRSDetailAdd.RouteSheetID = oitem.RouteSheetID;
                            oRSDetailAdd.RouteSheetDetailID = oitem.RouteSheetDetailID;
                            oRSDetailAdd.SequenceNo = 0;
                            oRSDetailAdd.LotID = oitem.SuggestLotID;
                            oRSDetailAdd.ApproveDate = oRSDetailAdditonal.ApproveDate;
                            oRSDetailAdditonals.Add(oRSDetailAdd);
                        }
                    }
                }
                else
                {
                    oRSDetailAdditonals = RSDetailAdditonal.Gets("SELECT * FROM View_RSDetailAdditonal WHERE isnull(ApprovedByID,0)=0 and RouteSheetDetailID IN (" + oRSDetailAdditonal.ErrorMessage + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                if (oRSDetailAdditonals.Count > 0)
                {
                    oRouteSheetDetails_Ret = RouteSheetDetail.DyeChemicalOut_All_V2(oRSDetailAdditonals, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oRouteSheetDetail = new RouteSheetDetail();
                _oRouteSheetDetail.ErrorMessage = ex.Message;
                oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
                oRouteSheetDetails_Ret.Add(_oRouteSheetDetail);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetails_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RouteSheetDyeChemicalOutAll_Return(List<RouteSheetDetail> oRouteSheetDetails)
        {
            List<RouteSheetDetail> oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
            try
            {
                oRouteSheetDetails_Ret = RouteSheetDetail.DyeChemicalOut_All_Return(oRouteSheetDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetDetail = new RouteSheetDetail();
                _oRouteSheetDetail.ErrorMessage = ex.Message;
                oRouteSheetDetails_Ret = new List<RouteSheetDetail>();
                oRouteSheetDetails_Ret.Add(_oRouteSheetDetail);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetails_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update_RSDetail(RouteSheetDetail oRouteSheetDetail)
        {
            try
            {
                if (oRouteSheetDetail.RouteSheetDetailID <= 0) throw new Exception("Invalid Dye Card/Sheet Detail!");

                oRouteSheetDetail = oRouteSheetDetail.Update_RSDetail(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetDetail = new RouteSheetDetail();
                _oRouteSheetDetail.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region RouteSheet Progress
        public ActionResult ViewRouteSheetProgress(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Hydro).ToString() + "," + ((int)EnumModuleName.Dryer).ToString() + "," + ((int)EnumModuleName.DyeingLoadUnLoad).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

           // List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
           //oAuthorizationRoleMappings= AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Hydro).ToString() + "," + ((int)EnumModuleName.Dryer).ToString() + "," + ((int)EnumModuleName.DyeingLoadUnLoad).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(EnumEmployeeDesignationType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = oEmployees;
            RouteSheet oRouteSheet = new RouteSheet();
            return View(oRouteSheet);
        }
        public ActionResult ViewRouteSheetProgress2(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Hydro).ToString() + "," + ((int)EnumModuleName.Dryer).ToString() + "," + ((int)EnumModuleName.DyeingLoadUnLoad).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

        
            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(EnumEmployeeDesignationType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = oEmployees;
            User oUser = new ESimSol.BusinessObjects.User();
            ViewBag.User = oUser.Get(((User)Session[SessionInfo.CurrentUser]).UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            RouteSheet oRouteSheet = new RouteSheet();
            ViewBag.BUID = buid;
            return View(oRouteSheet);
        }
        public ActionResult View_RouteSheetProgresss(int menuid, int buid) // NEW COLLECTION PAGE
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RouteSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();

            _oRouteSheets = new List<RouteSheet>();
            string sSQL = "Select top(100)* from View_RouteSheet Where RSState IN (" + (int)EnumRSState.YarnOut +
                                                                                 "," + (int)EnumRSState.LoadedInDyeMachine +
                                                                                 "," + (int)EnumRSState.UnloadedFromDyeMachine +
                                                                                 "," + (int)EnumRSState.LoadedInHydro +
                                                                                 "," + (int)EnumRSState.UnloadedFromHydro +
                                                                                 "," + (int)EnumRSState.LoadedInDrier +
                                                                                 "," + (int)EnumRSState.UnLoadedFromDrier +
                          ") order by RouteSheetDate DESC";
            _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.EnumRSStatuss = Enum.GetValues(typeof(EnumRSState)).Cast<EnumRSState>().ToList().Where(x => (int)x == (int)EnumRSState.YarnOut || (int)x == (int)EnumRSState.LoadedInDyeMachine ||
                                                                                                                (int)x == (int)EnumRSState.UnloadedFromDyeMachine || (int)x == (int)EnumRSState.LoadedInHydro ||
                                                                                                                (int)x == (int)EnumRSState.UnloadedFromHydro || (int)x == (int)EnumRSState.LoadedInDrier ||
                                                                                                                (int)x == (int)EnumRSState.UnLoadedFromDrier || (int)x == (int)EnumRSState.InPackageing 
                ).Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();

            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = buid;
            ViewBag.RouteSheetSetup = oRouteSheetSetup;
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));

            return View(_oRouteSheets);
        }
        public ActionResult View_RouteSheetProgress(int buid,int nId, double ts)
        {
            _oRouteSheet = new RouteSheet();

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            RouteSheetHistory oDyeingProgress = new RouteSheetHistory();
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();

            List<RouteSheetHistory> oRSHistorys = new List<RouteSheetHistory>();

            string YPCode = "";
            if(nId > 0) 
            {
                _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingProgress.RouteSheetID = _oRouteSheet.RouteSheetID;
                oDyeingProgress = oDyeingProgress.GetRSDyeingProgress(((User)Session[SessionInfo.CurrentUser]).UserID);
                
                _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

                oDUPScheduleDetails = DUPScheduleDetail.Gets(_oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUPScheduleDetails.Count > 0)
                {
                    YPCode = string.Join("+", oDUPScheduleDetails.Select(x => x.PSBatchNo).Distinct().ToList());
                }

                string sSQL = "Select * from View_RouteSheetHistory Where RouteSheetID= " + _oRouteSheet.RouteSheetID + " And CurrentStatus in (" + (int)EnumRSState.YarnOut + "," + (int)EnumRSState.LoadedInDyeMachine + "," + (int)EnumRSState.UnloadedFromDyeMachine + "," + (int)EnumRSState.LoadedInHydro + "," + (int)EnumRSState.UnloadedFromHydro + "," + (int)EnumRSState.LoadedInDrier + "," + (int)EnumRSState.UnLoadedFromDrier + ")";

                oRSHistorys = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oRouteSheet.RSHistorys = oRSHistorys.Where(p => p.CurrentStatus != EnumRSState.YarnOut).ToList();
                oRSHistorys = oRSHistorys.Where(p => p.CurrentStatus == EnumRSState.YarnOut).ToList();
            }
            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(EnumEmployeeDesignationType.Helper, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
            ViewBag.RouteSheetSetup = oRouteSheetSetup;
            ViewBag.Employees = oEmployees;
            ViewBag.YPCode = YPCode;
            ViewBag.BUID = buid;
            ViewBag.DyeingProgress = oDyeingProgress;
            ViewBag.RSHistorysYarnOut = oRSHistorys;
            return View(_oRouteSheet);
        }

        [HttpPost]
        public JsonResult GetsMachine_Hydro(Machine oMachine)
        {
            List<Machine> oMachines = new List<Machine>();
            try
            {
                oMachines = Machine.GetsByModule(oMachine.BUID, ((int)EnumModuleName.Hydro).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMachines = new List<Machine>();
                oMachines.Add(new Machine() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsMachine_Dryer(Machine oMachine)
        {
            List<Machine> oMachines = new List<Machine>();
            try
            {
                oMachines = Machine.GetsByModule(oMachine.BUID, ((int)EnumModuleName.Dryer).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMachines = new List<Machine>();
                oMachines.Add(new Machine() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetRouteSheetForProcess(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            try
            {

                //if (string.IsNullOrEmpty(oRouteSheet.RouteSheetNo) || oRouteSheet.RouteSheetNo.Trim()=="")
                //    throw new Exception("Please enter routesheet no to search.");

                string sSQL = "";
                if (!string.IsNullOrEmpty(oRouteSheet.RouteSheetNo))
                {
                    sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState >" + (int)EnumRSState.InFloor + " And RouteSheetNo = '" + oRouteSheet.RouteSheetNo.Trim() + "'";
                    //sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState In (4,5,6,7,8,9,10,11) And RouteSheetNo Like '" + oRouteSheet.RouteSheetNo.Trim() + "'";
                }
                else if (oRouteSheet.RouteSheetID > 0)
                {
                    sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState >" + (int)EnumRSState.InFloor + "  And RouteSheetID = " + oRouteSheet.RouteSheetID;
                }
                
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oRouteSheets.FirstOrDefault() != null && _oRouteSheets.FirstOrDefault().RouteSheetID > 0)
                {
                    oRouteSheet = _oRouteSheets.FirstOrDefault();
                    oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from View_RouteSheetHistory Where RouteSheetID= " + oRouteSheet.RouteSheetID + " And CurrentStatus In (4,6,7,8,9,10,11)";
                    oRouteSheet.RSHistorys = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.DyeingProgress = new RouteSheetHistory(); 
                    RouteSheetHistory oDyeingProgress = new RouteSheetHistory();
                    oDyeingProgress.RouteSheetID = oRouteSheet.RouteSheetID;
                    oDyeingProgress = oDyeingProgress.GetRSDyeingProgress(((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oRouteSheet.RSHistorys.FirstOrDefault() != null && oRouteSheet.RSHistorys.FirstOrDefault().RouteSheetID > 0 && oRouteSheet.RSHistorys.Where(b => (b.CurrentStatus == EnumRSState.YarnOut)).Count() > 0)
                    {
                        oDyeingProgress.UserName = oRouteSheet.RSHistorys.Where(p => p.CurrentStatus == EnumRSState.YarnOut).FirstOrDefault().UserName;
                        oDyeingProgress.EventTime = oRouteSheet.RSHistorys.Where(p => p.CurrentStatus == EnumRSState.YarnOut).FirstOrDefault().EventTime;
                        oRouteSheet.RSHistorys = oRouteSheet.RSHistorys.Where(p => p.CurrentStatus != EnumRSState.YarnOut).ToList();
                    }
                    oRouteSheet.DyeingProgress = oDyeingProgress;

                    oDUPScheduleDetails = DUPScheduleDetail.Gets(oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oDUPScheduleDetails.Count > 0)
                    {
                        oRouteSheet.DyeingProgress.Params = string.Join("+", oDUPScheduleDetails.Select(x => x.PSBatchNo).Distinct().ToList());
                    }

                    

                }
                else
                {
                    throw new Exception("Please Type valid Batch No.");
                }
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RouteSheetProgress(RouteSheetHistory oRSH)
        {
            try
            {
                if (oRSH.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");

                string ErrorMessage = RSStateValidation(oRSH.PreviousState, oRSH.CurrentStatus);
                if (ErrorMessage != "") { throw new Exception(ErrorMessage); }
                oRSH = oRSH.ChangeRSStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSH = new RouteSheetHistory();
                oRSH.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSH);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult RouteSheetProgressHistory(RouteSheetHistory oRSH)
        {
            try
            {
                if (oRSH.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");

                string ErrorMessage = RSStateValidation_Progress(oRSH.PreviousState, oRSH.CurrentStatus);
                if (ErrorMessage != "") { throw new Exception(ErrorMessage); }

                if (oRSH.CurrentStatus == EnumRSState.LoadedInHydro && oRSH.MachineID_Hydro <= 0) { throw new Exception("Please select a hydro machine & try again!!"); }
                else if (oRSH.CurrentStatus != EnumRSState.LoadedInHydro) oRSH.MachineID_Hydro = 0;

                if (oRSH.CurrentStatus == EnumRSState.LoadedInDrier && oRSH.MachineID_Dryer <= 0) { throw new Exception("Please select a drier machine & try again!!"); }
                else if (oRSH.CurrentStatus != EnumRSState.LoadedInDrier) oRSH.MachineID_Dryer = 0;

                oRSH = oRSH.ChangeRSStatus_Process(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSH = new RouteSheetHistory();
                oRSH.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSH);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Print Dyeing Progress Report & Preview_BatchProgress
        public void ExcelProgressHistory(string sParam, double nts)
        {
            if (!string.IsNullOrEmpty(sParam)) 
            {
                DateTime dtRouteSheetFrom_His = DateTime.Now;
                DateTime dtRouteSheetTo_His = DateTime.Now;
                string sRouteSheetNo = (string.IsNullOrEmpty(sParam.Split('~')[0])) ? "" : sParam.Split('~')[0].Trim();
                int ncboRSDateSearch = Convert.ToInt16(sParam.Split('~')[1]);
                DateTime dtRouteSheetFrom = Convert.ToDateTime(sParam.Split('~')[2]);
                DateTime dtRouteSheetTo = Convert.ToDateTime(sParam.Split('~')[3]);
                string sDOID = (string.IsNullOrEmpty(sParam.Split('~')[4])) ? "" : sParam.Split('~')[4].Trim();
                string sContractorIDs = (string.IsNullOrEmpty(sParam.Split('~')[5])) ? "" : sParam.Split('~')[5].Trim();
                string sMachineIDs = (string.IsNullOrEmpty(sParam.Split('~')[6])) ? "" : sParam.Split('~')[6].Trim();
                string sRSState = (string.IsNullOrEmpty(sParam.Split('~')[7])) ? "" : sParam.Split('~')[7].Trim();
                string sOrderNo = (string.IsNullOrEmpty(sParam.Split('~')[8])) ? "" : sParam.Split('~')[8].Trim();
                string sProductIDs = (string.IsNullOrEmpty(sParam.Split('~')[9])) ? "" : sParam.Split('~')[9].Trim();
                int nOrderType = Convert.ToInt16(sParam.Split('~')[10]);

                int ncboRSDateSearch_His = 0;
                if (sParam.Split('~').Length > 11)
                    Int32.TryParse(sParam.Split('~')[11], out ncboRSDateSearch_His);

                if (ncboRSDateSearch_His > 0)
                {
                    dtRouteSheetFrom_His = Convert.ToDateTime(sParam.Split('~')[12]);
                    dtRouteSheetTo_His = Convert.ToDateTime(sParam.Split('~')[13]);
                }

                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 ";
                string sReturn = " ";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                #region OrderDate Date
                if (ncboRSDateSearch != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboRSDateSearch == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) ";
                    }

                    sSQL = sSQL + sReturn;
                }
                #endregion
                #region History Date
                if (ncboRSDateSearch_His != (int)EnumCompareOperator.None)
                {
                    if (string.IsNullOrEmpty(sRSState)) { sRSState = ((int)EnumRSState.InFloor).ToString(); }

                    Global.TagSQL(ref sReturn);
                    if (ncboRSDateSearch_His == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                        //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    sRSState = "";
                    sSQL = sSQL + sReturn;
                }
                #endregion
                // if (IsdtRouteSheetSearch) sSQL = sSQL + " And RouteSheetDate Between '" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "' And '" + dtRouteSheetTo.ToString("dd MMM yyyy") + "'";


                if (sDOID != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (" + sDOID + "))";
                if (sContractorIDs != "") sSQL = sSQL + " And RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ContractorID In (" + sContractorIDs + "))";
                if (sMachineIDs != "") sSQL = sSQL + " And MachineID In (" + sMachineIDs + ")";
                if (sRSState != "") sSQL = sSQL + " And RSState In (" + sRSState + ")";
                if (sProductIDs != "") sSQL = sSQL + " And ProductID_Raw In (" + sProductIDs + ")";
                if (nOrderType > 0) sSQL = sSQL + " And OrderType In (" + nOrderType + ")";
                if (sOrderNo != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like '%" + sOrderNo + "%')";



                sSQL = sSQL + " order by RouteSheetDate DESC";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Dyeing Progress");
                sheet.Name = "Dyeing Progress List";
                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 16; //Yarn Out Date
                sheet.Column(4).Width = 15; //RouteSheet No
                sheet.Column(5).Width = 25; //RSState
                sheet.Column(6).Width = 32; //Product Name
                sheet.Column(7).Width = 15; //Lot No
                sheet.Column(8).Width = 22; //Store
                sheet.Column(9).Width = 12; //Qty

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 9].Merge = true;
                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion
                #region Content part


                #region Column Header

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "RS Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "RouteSheet No"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = "Current State"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Store Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                int nSL = 0;
                double nQty = _oRouteSheets.Sum(x => x.Qty);
                bool IsSpan = true;

                _oRouteSheets = _oRouteSheets.OrderBy(x => x.RouteSheetDate).ToList();
                while (_oRouteSheets.Count() > 0)
                {
                    IsSpan = true;
                    var oTList = _oRouteSheets.Where(x => x.RouteSheetDate == _oRouteSheets.FirstOrDefault().RouteSheetDate).ToList();
                    _oRouteSheets.RemoveAll(x => x.RouteSheetDate == oTList.FirstOrDefault().RouteSheetDate);

                    foreach (var oItem in oTList)
                    {
                        ++nSL;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (IsSpan)
                        {
                            IsSpan = false;
                            sheet.Cells[rowIndex, 3].Merge = true;
                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.RouteSheetDateStr; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.RouteSheetNo; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.RSStateStr; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.StoreName; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.Qty; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;

                    }

                }
                #endregion

                #region Bottom Part Total

                sheet.Cells[rowIndex, 2, rowIndex, 8].Merge = true;
                cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = nQty; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DyeingProgressReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public void ExportToExcel_DyeingProgress(string sParams, int buid, double ts)
        {
            string Header = "", sErrorMesage = ""; int nReportType = 0, nDateCriteriya = 0; DateTime dStartDate = DateTime.Now, dEndDate = DateTime.Now;

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            RouteSheetHistory oRSHistory = new RouteSheetHistory();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            _oRouteSheets = new List<RouteSheet>();
            List<RouteSheetHistory> oRSDyeingProgress = new List<RouteSheetHistory>();
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();

            try
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Get RS History

                if (!string.IsNullOrEmpty(sParams))
                {
                    nReportType = Convert.ToInt16(sParams.Split('~')[0]);
                    nDateCriteriya = Convert.ToInt16(sParams.Split('~')[1]);
                    dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
                    dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);
                }

                string sSQL = "SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID<>0 ";

                //if (nDateCriteriya > 0)
                //{
                //    DateObject.CompareDateQuery(ref sSQL, "EventTime", nDateCriteriya, dStartDate, dEndDate);
                //}
                if (nDateCriteriya > 0)
                {
                    if (nDateCriteriya == (int)EnumCompareOperator.EqualTo) { dEndDate = dStartDate; }
                    sSQL = sSQL + " and EventTime>=  '" + dStartDate.ToString("dd MMM yyy HH:mm") + "' and EventTime<'" + dEndDate.AddDays(1).ToString("dd MMM yyy HH:mm") + "'";
                }

                if (nReportType == 1) sSQL += " AND CurrentStatus IN (" + (int)EnumRSState.UnloadedFromDyeMachine + ", " + (int)EnumRSState.UnloadedFromHydro + ", " + (int)EnumRSState.UnLoadedFromDrier + ")";
                if (nReportType == 2) sSQL += " AND CurrentStatus = " + (int)EnumRSState.UnloadedFromHydro;
                if (nReportType == 3) sSQL += " AND CurrentStatus = " + (int)EnumRSState.UnLoadedFromDrier;
                if (nReportType == 4) sSQL += " AND CurrentStatus = " + (int)EnumRSState.UnloadedFromDyeMachine;

                oRouteSheetHistorys = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion

                if (oRouteSheetHistorys.Count <= 0)
                {
                    sErrorMesage = "There is no data for print!";
                }
                else
                {
                    #region add Loaded data (not between the date range)
                    List<RouteSheetHistory> oRouteSheetHistorys_Load = new List<RouteSheetHistory>();

                    if (nReportType == 1)
                    {
                        #region LoadedInDyeMachine
                        sSQL = " SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromDyeMachine).Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";
                        sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDyeMachine;

                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                        #endregion

                        #region LoadedInHydro
                        sSQL = " SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromHydro).Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";
                        sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInHydro;

                        oRouteSheetHistorys_Load = new List<RouteSheetHistory>();
                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                        #endregion

                        #region LoadedInDrier
                        sSQL = " SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Where(x => x.CurrentStatus == EnumRSState.UnLoadedFromDrier).Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";
                        sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDrier;

                        oRouteSheetHistorys_Load = new List<RouteSheetHistory>();
                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                        #endregion
                    }
                    else
                    {
                        sSQL = "SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";

                        if (nReportType == 2) sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInHydro;
                        if (nReportType == 3) sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDrier;
                        if (nReportType == 4) sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDyeMachine;

                        oRouteSheetHistorys_Load = new List<RouteSheetHistory>();
                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                    }
                    #endregion

                    _oRouteSheets = RouteSheet.Gets("SELECT * FROM View_RouteSheet WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRSDyeingProgress = RouteSheetHistory.Gets("SELECT * FROM View_RouteSheetDyeingProgress WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                sErrorMesage = ex.Message;
            }

            if (sErrorMesage == "")
            {
                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = 0;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {

                    string sHeader = "";

                    if (nReportType == 1) sHeader = "Dye_Hydro_Dryer";
                    else if (nReportType == 2) sHeader = "Hydro";
                    else if (nReportType == 3) sHeader = "Dryer";
                    else if (nReportType == 4) sHeader = "Dyeing Machine";

                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(sHeader+"_Report");
                    sheet.Name = sHeader+"_Report";

                    #region Col Def
                    int nCount_Column = 2;

                    //#SL Batch No Order No Buyer Name Machine Name Load Time Load By UnLoad Time UnLoad By Remarks
                    sheet.Column(nCount_Column++).Width = 10; //SL
                    sheet.Column(nCount_Column++).Width = 20; //Batch
                    sheet.Column(nCount_Column++).Width = 20; //Order

                    sheet.Column(nCount_Column++).Width = 15; //OrderQty
                    sheet.Column(nCount_Column++).Width = 40; //Buyer
                    sheet.Column(nCount_Column++).Width = 30; //Machine


                    sheet.Column(nCount_Column++).Width = 30; //Load
                    sheet.Column(nCount_Column++).Width = 30; //Load
                    sheet.Column(nCount_Column++).Width = 30; //Load
                    sheet.Column(nCount_Column++).Width = 30; //UnLoad
                    sheet.Column(nCount_Column++).Width = 30; //UnLoad
                    sheet.Column(nCount_Column++).Width = 30; //UnLoad
                    sheet.Column(nCount_Column++).Width = 30; //UnLoad
                    sheet.Column(nCount_Column++).Width = 30; //UnLoad
                    nEndCol = nCount_Column - 1;
                    #endregion

                    #region Report Header
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 16; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;
                    //cell = sheet.Cells[nRowIndex, nEndCol / 2 + 1, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = Header; cell.Style.Font.Bold = true;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion

                    nRowIndex++;

                    #region Date Wise Loop
                    if (nReportType == 1)
                    {
                        nStartCol = 2;
                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Dyeing Progress Report (Dye)"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Column Header
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, nStartCol, nEndCol, true, ExcelHorizontalAlignment.Left);

                        nStartCol = 2;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "#SL", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Batch", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Order No", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Buyer Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Yarn Type", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Shade", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Qty(" + oRouteSheetSetup.MUnit + ")", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Machine Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        nRowIndex++;
                        #endregion

                        Excel_AddRSData(ref sheet, ref nRowIndex, 4, oRouteSheetHistorys, oRSDyeingProgress);

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                        ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                        nStartCol = 8;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, _oRouteSheets.Select(x => x.Qty).Sum().ToString(), true, true);
                        ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                        #endregion

                        nRowIndex++;
                        nRowIndex++;

                        nStartCol = 2;
                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Dyeing Progress Report (Hydro)"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Column Header
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, nStartCol, nEndCol, true, ExcelHorizontalAlignment.Left);

                        nStartCol = 2;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "#SL", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Batch", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Order No", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Buyer Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Yarn Type", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Shade", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Qty(" + oRouteSheetSetup.MUnit + ")", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Machine Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        nRowIndex++;
                        #endregion

                        Excel_AddRSData(ref sheet, ref nRowIndex, 2, oRouteSheetHistorys, oRSDyeingProgress);

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                        ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                        nStartCol = 8;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, _oRouteSheets.Select(x => x.Qty).Sum().ToString(), true, true);
                        ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                        #endregion

                        nRowIndex++;
                        nRowIndex++;

                        nStartCol = 2;
                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Dyeing Progress Report (Dryer)"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion
                        #region Column Header
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, nStartCol, nEndCol, true, ExcelHorizontalAlignment.Left);

                        nStartCol = 2;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "#SL", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Batch", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Order No", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Buyer Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Yarn Type", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Shade", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Qty(" + oRouteSheetSetup.MUnit + ")", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Machine Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        nRowIndex++;
                        #endregion

                        Excel_AddRSData(ref sheet, ref nRowIndex, 3, oRouteSheetHistorys, oRSDyeingProgress);

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                        ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                        nStartCol = 8;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, _oRouteSheets.Select(x => x.Qty).Sum().ToString(), true, true);
                        ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                        #endregion

                    }
                    else
                    {
                        nStartCol = 2;
                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Dyeing Progress Report (" + sHeader + ")"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Column Header
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, nStartCol, nEndCol, true, ExcelHorizontalAlignment.Left);

                        nStartCol = 2;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "#SL", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Batch", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Order No", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Buyer Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Yarn Type", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Shade", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Qty(" + oRouteSheetSetup.MUnit + ")", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Machine Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Load By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload Time", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Unload By", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Remarks", false, true);
                        nRowIndex++;
                        #endregion

                        Excel_AddRSData(ref sheet, ref nRowIndex, nReportType, oRouteSheetHistorys, oRSDyeingProgress);

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                        ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                        nStartCol = 8;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, _oRouteSheets.Select(x => x.Qty).Sum().ToString(), true, true);
                        ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                        #endregion
                    }
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Day_Basis_Report_Shift_Wise.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void Excel_AddRSData(ref ExcelWorksheet sheet, ref int nRowIndex, int eReportLayout, List<RouteSheetHistory> oRSHistorys, List<RouteSheetHistory> oRSDyeingProgresss) 
        {
            #region Data List Wise Loop

            int nCount = 0, nStartCol;
            foreach (var oItem in _oRouteSheets)
            {
                var oMachine_LD = new RouteSheetHistory();
                var oMachine_ULD = new RouteSheetHistory();

                var oRSDProgress = oRSDyeingProgresss.Where(x => x.RouteSheetID == oItem.RouteSheetID).FirstOrDefault();
                var oRSHistory = oRSHistorys.Where(x => x.RouteSheetID == oItem.RouteSheetID).ToList();

                if (eReportLayout == 2)
                {
                    oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInHydro).FirstOrDefault();
                    oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromHydro).FirstOrDefault();
                }
                else if (eReportLayout == 3)
                {
                    oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInDrier).FirstOrDefault();
                    oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnLoadedFromDrier).FirstOrDefault();
                }
                else if (eReportLayout == 4)
                {
                    oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInDyeMachine).FirstOrDefault();
                    oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromDyeMachine).FirstOrDefault();
                }

                if (oMachine_LD != null || oMachine_ULD != null)
                {

                    nStartCol = 2; nCount++;

                    #region DATA
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.RouteSheetNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderNoFull, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorName, false);
                    ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);

                    if (eReportLayout == 2 && oRSDProgress != null)
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRSDProgress.MachineName_Hydro, false);
                    else if (eReportLayout == 3 && oRSDProgress != null)
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRSDProgress.MachineName_Dryer, false);
                    else if (eReportLayout == 4 && oRSDProgress != null)
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.MachineName, false);
                    else
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);

                    if (eReportLayout == 2)
                    {
                        oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInHydro).FirstOrDefault();
                        oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromHydro).FirstOrDefault();
                    }
                    else if (eReportLayout == 3)
                    {
                        oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInDrier).FirstOrDefault();
                        oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnLoadedFromDrier).FirstOrDefault();
                    }
                    else if (eReportLayout == 4)
                    {
                        oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInDyeMachine).FirstOrDefault();
                        oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromDyeMachine).FirstOrDefault();
                    }

                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oMachine_LD == null ? "" : oMachine_LD.EventTimeStr), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oMachine_LD == null ? "" : oMachine_LD.UserName), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oMachine_LD == null ? "" : oMachine_LD.Note), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oMachine_ULD == null ? "" : oMachine_ULD.EventTimeStr), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oMachine_ULD == null ? "" : oMachine_ULD.UserName), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oMachine_ULD == null ? "" : oMachine_ULD.Note), false);

                    nRowIndex++;
                    #endregion
                }
            }
            #endregion

            //int nCol_Total = 4;
            //ExcelTool.FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, 2, 3, true, ExcelHorizontalAlignment.Center);
            //foreach (var oDataGrp in dataGrpList)
            //{
            //    ExcelTool.FillCell(sheet, nRowIndex, nCol_Total, oDataGrp.Results.Sum(x => x.Qty_Batch).ToString(), true, true);
            //    ExcelTool.FillCell(sheet, nRowIndex, nCol_Total + 1, oDataGrp.Results.Sum(x => x.Value).ToString(), true, true);
            //    ExcelTool.FillCell(sheet, nRowIndex, nCol_Total + 2, oDataGrp.Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
            //    nCol_Total += 3;
            //}
        }
        public ActionResult Preview_BatchProgress(string sParams, int buid, double ts)
        {
            string Header = "", sErrorMesage = ""; int nReportType = 0, nDateCriteriya=0; DateTime dStartDate= DateTime.Now, dEndDate= DateTime.Now;

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            RouteSheetHistory oRSHistory = new RouteSheetHistory();

            _oRouteSheets = new List<RouteSheet>();
            List<RouteSheetHistory> oRSDyeingProgress = new List<RouteSheetHistory>();
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();

            try
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Get RS History

                if (!string.IsNullOrEmpty(sParams)) 
                {
                    nReportType = Convert.ToInt16(sParams.Split('~')[0]);
                    nDateCriteriya = Convert.ToInt16(sParams.Split('~')[1]);
                    dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
                    dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);
                }

                string sSQL = "SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID<>0 ";

                if(nDateCriteriya>0)
                {
                  //  DateObject.CompareDateQuery(ref sSQL, "EventTime", nDateCriteriya, dStartDate, dEndDate);
                    if (nDateCriteriya == (int)EnumCompareOperator.EqualTo) { dEndDate = dStartDate ; }
                        sSQL = sSQL + " and EventTime>=  '" + dStartDate.ToString("dd MMM yyy HH:mm") + "' and EventTime<'" + dEndDate.AddDays(1).ToString("dd MMM yyy HH:mm")+"'";
                    //sSQL = sSQL + " EB." + sDateType + " BETWEEN '" + dStartDateCritaria.ToString("dd MMM yyy") + "' AND '" + dEndDateCritaria.ToString("dd MMM yyy") + "' ";
                }

                //if (nReportType == 1) sSQL += " AND CurrentStatus >= " + (int)EnumRSState.LoadedInDyeMachine + " AND CurrentStatus <= " + (int)EnumRSState.UnLoadedFromDrier;
                //if (nReportType == 2) sSQL += " AND CurrentStatus >= " + (int)EnumRSState.LoadedInHydro + " AND CurrentStatus <= " + (int)EnumRSState.UnloadedFromHydro;
                //if (nReportType == 3) sSQL += " AND CurrentStatus >= " + (int)EnumRSState.LoadedInDrier + " AND CurrentStatus <= " + (int)EnumRSState.UnLoadedFromDrier;
                //if (nReportType == 4) sSQL += " AND CurrentStatus >= " + (int)EnumRSState.LoadedInDyeMachine + " AND CurrentStatus <= " + (int)EnumRSState.UnloadedFromDyeMachine;

                if (nReportType == 1) sSQL += " AND CurrentStatus IN (" + (int)EnumRSState.UnloadedFromDyeMachine + ", " + (int)EnumRSState.UnloadedFromHydro + ", " + (int)EnumRSState.UnLoadedFromDrier + ")";
                if (nReportType == 2) sSQL += " AND CurrentStatus = " + (int)EnumRSState.UnloadedFromHydro;
                if (nReportType == 3) sSQL += " AND CurrentStatus = " + (int)EnumRSState.UnLoadedFromDrier;
                if (nReportType == 4) sSQL += " AND CurrentStatus = " + (int)EnumRSState.UnloadedFromDyeMachine;

                oRouteSheetHistorys = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion

                if (oRouteSheetHistorys.Count <= 0)
                {
                    sErrorMesage = "There is no data for print!";
                }
                else
                {
                    #region add Loaded data (not between the date range)
                    List<RouteSheetHistory> oRouteSheetHistorys_Load = new List<RouteSheetHistory>();

                    if (nReportType == 1)
                    {
                        #region LoadedInDyeMachine
                        sSQL =  " SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromDyeMachine).Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";
                        sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDyeMachine;

                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                        #endregion

                        #region LoadedInDyeMachine
                        sSQL = " SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromHydro).Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";
                        sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInHydro;

                        oRouteSheetHistorys_Load = new List<RouteSheetHistory>();
                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                        #endregion

                        #region LoadedInDrier
                        sSQL = " SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Where(x => x.CurrentStatus == EnumRSState.UnLoadedFromDrier).Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";
                        sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDrier;

                        oRouteSheetHistorys_Load = new List<RouteSheetHistory>();
                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                        #endregion
                    }
                    else 
                    {
                        sSQL = "SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetID)) + ")";
                        sSQL += " AND RouteSheetHistoryID NOT IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetHistoryID)) + ")";

                        if (nReportType == 2) sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInHydro;
                        if (nReportType == 3) sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDrier;
                        if (nReportType == 4) sSQL += " AND CurrentStatus = " + (int)EnumRSState.LoadedInDyeMachine;

                        oRouteSheetHistorys_Load = new List<RouteSheetHistory>();
                        oRouteSheetHistorys_Load = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheetHistorys.AddRange(oRouteSheetHistorys_Load);
                    }
                    #endregion

                    _oRouteSheets = RouteSheet.Gets("SELECT * FROM View_RouteSheet WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRSDyeingProgress = RouteSheetHistory.Gets("SELECT * FROM View_RouteSheetDyeingProgress WHERE RouteSheetID IN (" + string.Join(",", oRouteSheetHistorys.Select(x => x.RouteSheetID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                sErrorMesage = ex.Message;
            }

            if (!string.IsNullOrEmpty(sErrorMesage))
            {
                string sMessage = "There is no data for print";
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(sMessage);
                return File(abytes, "application/pdf");
            }
            else
            {
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

                rptRouteSheetProgress oReport = new rptRouteSheetProgress();
                byte[] abytes = oReport.PrepareReport(_oRouteSheets, oRouteSheetHistorys, oRSDyeingProgress, oBusinessUnit, oCompany, nReportType, oRouteSheetSetup);
                return File(abytes, "application/pdf");
            }
        }
        #endregion

        #endregion

        #region RouteSheet QC SetUp


        public ActionResult ViewRSInQCSetups(int buid, int menuid)
        {
            List<RSInQCSetup> oRSInQCSetups = new List<RSInQCSetup>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);


            string sSQL = "SELECT * FROM View_RSInQCSetup  Order by LocationName, YarnType,WUName";
            oRSInQCSetups = RSInQCSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumYarnTypes = Enum.GetValues(typeof(EnumYarnType)).Cast<EnumYarnType>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();

            List<WorkingUnit> oWUs=new List<WorkingUnit>();
            oWUs = WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Location> oLocations = new List<Location>();
            oLocations = Location.GetsIncludingStore( ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.WorkingUnits = oWUs;
            ViewBag.Locations = oLocations;
            
            return View(oRSInQCSetups);
        }


        [HttpPost]
        public JsonResult SaveRSInQCSetup(RSInQCSetup oRSInQCSetup)
        {
            try
            {
                if (oRSInQCSetup.RSInQCSetupID <= 0)
                {
                    oRSInQCSetup = oRSInQCSetup.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRSInQCSetup = oRSInQCSetup.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oRSInQCSetup = new RSInQCSetup();
                oRSInQCSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSInQCSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRSInQCSetup(RSInQCSetup oRSInQCSetup)
        {
            try
            {
                if (oRSInQCSetup.RSInQCSetupID <= 0) { throw new Exception("Please select an valid item."); }
                oRSInQCSetup = oRSInQCSetup.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSInQCSetup = new RSInQCSetup();
                oRSInQCSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSInQCSetup.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityRSInQCSetup(RSInQCSetup oRSInQCSetup)
        {
            try
            {
                if (oRSInQCSetup.RSInQCSetupID <= 0) { throw new Exception("Please select an valid item."); }

                oRSInQCSetup.Activity = !oRSInQCSetup.Activity;
                oRSInQCSetup = oRSInQCSetup.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSInQCSetup = new RSInQCSetup();
                oRSInQCSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSInQCSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRSInQCSetup(RSInQCSetup oRSInQCSetup)
        {
            try
            {
                oRSInQCSetup = RSInQCSetup.Get(oRSInQCSetup.RSInQCSetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSInQCSetup = new RSInQCSetup();
                oRSInQCSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSInQCSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region RouteSheet Packing & QC

        public ActionResult ViewRouteSheetPacking(string menuid)
        {
            string token = menuid;
            Menu oMenu= Menu.Get("ViewRouteSheetPacking", "RouteSheet",-9);
            RouteSheetPacking oRouteSheetPacking = new RouteSheetPacking();
            BusinessUnit    oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (token.Contains(Global.DefaultEncodeValue()) && oMenu.MenuID>0)
            {
                string actualToken = Global.GetActualToken(token);
                try
                {
                    int mid = Convert.ToInt32(actualToken);
                    if (oMenu.MenuID != mid)
                        throw new Exception("Opps! Some this went wrong.");

                    this.Session.Remove(SessionInfo.MenuID);
                    this.Session.Add(SessionInfo.MenuID, mid);
                    ViewBag.EnumWarpWefts = Enum.GetValues(typeof(EnumWarpWeft)).Cast<EnumWarpWeft>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

                    RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
                    ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
                    //RSShift oRSShift = new RSShift();
                    ViewBag.RSShifts = RSShift.GetsByModule(oBusinessUnit.BusinessUnitID, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //List<ColorCategory> oColorCategorys = new List<ColorCategory>();
                    //oColorCategorys = ColorCategory.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                    //ViewBag.ColorCategorys = oColorCategorys;
                }
                catch(Exception ex)
                {
                    ViewBag.IsError = true;
                    ModelState.AddModelError("Error", "Opps! Something went wrong.");
                }
                return View(oRouteSheetPacking);
            }
            else
            {
                token = Global.GetEncodeValue(token);
                return RedirectToAction("ViewRouteSheetPacking", "RouteSheet", new { menuid = token});
            }
        }

        [HttpPost]
        public JsonResult SavePacking(RouteSheetPacking oRouteSheetPacking)
        {
            try
            {
                if (oRouteSheetPacking.PackingID <= 0)
                {
                    oRouteSheetPacking = oRouteSheetPacking.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRouteSheetPacking = oRouteSheetPacking.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oRouteSheetPacking = new RouteSheetPacking();
                oRouteSheetPacking.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetPacking);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PackingMultiple(RouteSheetPacking oRouteSheetPacking)
        {
            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            try
            {
                int nBag = 0;
                int.TryParse(oRouteSheetPacking.Params, out nBag);
                if (nBag <= 0)
                    throw new Exception("Please enter bag no.");
                if (oRouteSheetPacking.DUHardWindingID <= 0)
                {
                    if (oRouteSheetPacking.RouteSheetID <= 0)
                        throw new Exception("Routesheet required.");
                }

                oRouteSheetPackings = oRouteSheetPacking.PackingMultiple(nBag, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRouteSheetPackings.FirstOrDefault() == null || oRouteSheetPackings.FirstOrDefault().PackingID <= 0)
                    throw new Exception(oRouteSheetPackings.FirstOrDefault().ErrorMessage);
                else
                {
                    oRouteSheetPacking = new RouteSheetPacking();
                    oRouteSheetPacking.RouteSheetPackings = oRouteSheetPackings;
                }

            }
            catch (Exception ex)
            {
                oRouteSheetPacking = new RouteSheetPacking();
                oRouteSheetPacking.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetPacking);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePacking(RouteSheetPacking oRouteSheetPacking)
        {
            try
            {
                if (oRouteSheetPacking.PackingID <= 0) { throw new Exception("Please select an valid item."); }
                oRouteSheetPacking = oRouteSheetPacking.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetPacking = new RouteSheetPacking();
                oRouteSheetPacking.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetPacking.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRouteSheetPacking(RouteSheetPacking oRouteSheetPacking)
        {
            try
            {
                oRouteSheetPacking = RouteSheetPacking.Get(oRouteSheetPacking.PackingID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetPacking = new RouteSheetPacking();
                oRouteSheetPacking.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetPacking);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRouteSheetWithPacking(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
            try
            {
                if (oRouteSheet.RouteSheetID<=0)
                    throw new Exception("Please enter routesheet no to search.");

                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState In (9,10,11,12,13,14,15,16,18) And RouteSheetID ='" + oRouteSheet.RouteSheetID + "'";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oRouteSheets.FirstOrDefault() != null && _oRouteSheets.FirstOrDefault().RouteSheetID > 0)
                {
                    oRouteSheet = _oRouteSheets.FirstOrDefault();
                    oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oRouteSheetHistory = oRouteSheetHistory.GetBy(oRouteSheet.RouteSheetID, (int)EnumRSState.QC_Done, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.Note = oRouteSheetHistory.Note;
                    sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + oRouteSheet.RouteSheetID + " Order by BagNo";
                    oRouteSheet.RouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                     sSQL = "Select * from View_RSInQCDetail Where RouteSheetID=" + oRouteSheet.RouteSheetID;
                    oRouteSheet.RSInQCDetails = RSInQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RSInQCDetails.AddRange(this.GenerateQCInspection(oRouteSheet.RSInQCDetails.Select(x => x.RSInQCSetupID).ToArray()));
                    oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID,((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RSInQCDetails=oRouteSheet.RSInQCDetails.OrderBy(x=>x.YarnType).ThenBy(x=>x.QCSetupName).ToList();
                    double nQty = (oRouteSheet.RouteSheetPackings.Count() > 0 && oRouteSheet.RouteSheetPackings.FirstOrDefault().PackingID > 0) ? oRouteSheet.RouteSheetPackings.Sum(x => x.Weight) : 0;
                    if (oRouteSheetHistory.RouteSheetHistoryID <= 0)
                    {
                        if (nQty > 0 && oRouteSheet.RSInQCDetails.Where(x => x.RSInQCDetailID <= 0 && x.YarnType == EnumYarnType.FreshDyedYarn).Any())
                        {

                            oRouteSheet.RSInQCDetails.ForEach(x =>
                            {
                                if (oRouteSheet.RSInQCDetails.FirstOrDefault() != null && oRouteSheet.RSInQCDetails.Where(b => (b.YarnType == EnumYarnType.FreshDyedYarn)).Count() > 0)
                                {
                                    x.Qty = nQty;
                                }
                                x.RouteSheetID = oRouteSheet.RouteSheetID;
                            });

                            oRouteSheet.RSInQCDetails.ForEach(x =>
                            {
                                if (oRouteSheet.RSInQCDetails.FirstOrDefault() != null && oRouteSheet.RSInQCDetails.Where(b => (b.YarnType == EnumYarnType.FreshDyedYarn)).Count() > 0)
                                {
                                    if (x.YarnType == EnumYarnType.FreshDyedYarn)
                                    {
                                        x.Qty = nQty;
                                    }
                                    x.RouteSheetID = oRouteSheet.RouteSheetID;
                                }

                            });

                            var oRSInQCDetail = oRouteSheet.RSInQCDetails.Where(x => x.RSInQCDetailID <= 0 && x.YarnType == EnumYarnType.FreshDyedYarn).FirstOrDefault();
                            oRSInQCDetail.RouteSheetID = oRouteSheet.RouteSheetID;

                            oRSInQCDetail = oRSInQCDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            oRouteSheet.RSInQCDetails[0] = oRSInQCDetail;
                        }
                    }
                    else
                    {
                        oRouteSheet.Params = "QCDone";
                    }

                }
                else
                {
                    throw new Exception("Invalid Batch/Dyeing Card.");
                }
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRouteSheetWithPackingDOD(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
            try
            {
                if (oRSFreshDyedYarn.RouteSheetID <= 0)
                    throw new Exception("Please enter routesheet no to search.");

                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState In (9,10,11,12,13,14,15,16,18) And RouteSheetID ='" + oRSFreshDyedYarn.RouteSheetID + "'";
                oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oRouteSheets.FirstOrDefault() != null && oRouteSheets.FirstOrDefault().RouteSheetID > 0)
                {
                    oRouteSheet = oRouteSheets.FirstOrDefault();

                    //_oDUHardWinding = new DUHardWinding();
                    //_oDUHardWinding = _oDUHardWinding.Get(oDUHardWinding.DUHardWindingID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oRouteSheet.Qty = _oDUHardWinding.Qty;

                    //  oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oRouteSheetHistory = oRouteSheetHistory.GetBy(oRouteSheet.RouteSheetID, (int)EnumRSState.QC_Done, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.Note = oRouteSheetHistory.Note;
                    sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + oRSFreshDyedYarn.RouteSheetID + " AND DyeingOrderDetailID=" + oRSFreshDyedYarn.DyeingOrderDetailID + " Order by BagNo";
                    oRouteSheet.RouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSQL = "Select * from View_RSInQCDetail Where RouteSheetID=" + oRSFreshDyedYarn.RouteSheetID + " AND DyeingOrderDetailID=" + oRSFreshDyedYarn.DyeingOrderDetailID;
                    oRouteSheet.RSInQCDetails = RSInQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RSInQCDetails.AddRange(this.GenerateQCInspection(oRouteSheet.RSInQCDetails.Select(x => x.RSInQCSetupID).ToArray()));
                    oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //oRouteSheet.RouteSheetDOs = RouteSheetDO.Gets("Select * from View_RouteSheetDO where RouteSheetID in (" + oRouteSheet.RouteSheetID + ") AND DyeingOrderDetailID="+ oDUHardWinding.DyeingOrderDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oRouteSheet.RSInQCDetails = oRouteSheet.RSInQCDetails.OrderBy(x => x.YarnType).ThenBy(x => x.QCSetupName).ToList();
                    double nQty = (oRouteSheet.RouteSheetPackings.Count() > 0 && oRouteSheet.RouteSheetPackings.FirstOrDefault().PackingID > 0) ? oRouteSheet.RouteSheetPackings.Sum(x => x.Weight) : 0;
                    if (oRouteSheetHistory.RouteSheetHistoryID <= 0)
                    {
                        if (nQty > 0 && oRouteSheet.RSInQCDetails.Where(x => x.RSInQCDetailID <= 0 && x.YarnType == EnumYarnType.FreshDyedYarn).Any())
                        {
                            oRouteSheet.RSInQCDetails.FirstOrDefault().Qty = nQty;

                            var oRSInQCDetail = oRouteSheet.RSInQCDetails.FirstOrDefault();
                            oRSInQCDetail.RouteSheetID = oRouteSheet.RouteSheetID;
                            oRSInQCDetail.DyeingOrderDetailID = oRSFreshDyedYarn.DyeingOrderDetailID;
                            oRSInQCDetail = oRSInQCDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            oRouteSheet.RSInQCDetails[0] = oRSInQCDetail;
                        }
                    }
                    else
                    {
                        oRouteSheet.Params = "QCDone";
                    }

                }
                else
                {
                    throw new Exception("Invalid Batch/Dyeing Card.");
                }
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRouteSheetWithQC(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
            try
            {
                if (oRouteSheet.RouteSheetID <= 0)
                    throw new Exception("Please enter DyeingCard/Batch no to search.");

                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState In (9,10,11,12,13,14,15,16,18) And RouteSheetID ='" + oRouteSheet.RouteSheetID + "'";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oRouteSheets.FirstOrDefault() != null && _oRouteSheets.FirstOrDefault().RouteSheetID > 0)
                {
                    oRouteSheet = _oRouteSheets.FirstOrDefault();
                    oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oRouteSheetHistory = oRouteSheetHistory.GetBy(oRouteSheet.RouteSheetID, (int)EnumRSState.QC_Done, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.Note = oRouteSheetHistory.Note;
                    //sSQL = "Select * from RouteSheetPacking Where RouteSheetID=" + oRouteSheet.RouteSheetID;
                    //oRouteSheet.RouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSQL = "Select * from View_RSInQCDetail Where RouteSheetID=" + oRouteSheet.RouteSheetID;
                    oRouteSheet.RSInQCDetails = RSInQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RSInQCDetails.AddRange(this.GenerateQCInspection(oRouteSheet.RSInQCDetails.Select(x => x.RSInQCSetupID).ToArray()));
                    oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RSInQCDetails = oRouteSheet.RSInQCDetails.OrderBy(x => x.YarnType).ThenBy(x => x.QCSetupName).ToList();
                    //double nQty = (oRouteSheet.RouteSheetPackings.Count() > 0 && oRouteSheet.RouteSheetPackings.FirstOrDefault().PackingID > 0) ? oRouteSheet.RouteSheetPackings.Sum(x => x.Weight) : 0;
                    if (oRouteSheetHistory.RouteSheetHistoryID <= 0)
                    {
                        if (oRouteSheet.RSInQCDetails.Where(x => x.RSInQCDetailID <= 0 && x.YarnType == EnumYarnType.FreshDyedYarn).Any())
                        {
                            oRouteSheet.RSInQCDetails.FirstOrDefault().Qty = oRouteSheet.Qty;

                            var oRSInQCDetail = oRouteSheet.RSInQCDetails.FirstOrDefault();
                            oRSInQCDetail.RouteSheetID = oRouteSheet.RouteSheetID;

                            //oRSInQCDetail = oRSInQCDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            //oRouteSheet.RSInQCDetails[0] = oRSInQCDetail;
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid Dyeing Card.");
                }
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintRSPacking(int nId)//nId=RouteSheetID
        {
            //List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            string sBatchNo = "", sCombineRSNo = ""; ;
            _oRouteSheet = new RouteSheet();
            _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<RouteSheetPacking> oRouteSheetPackings=new List<RouteSheetPacking>();
            string sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + nId;
            oRouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
          
            rptRouteSheetLabel oReport = new rptRouteSheetLabel();
            byte[] abytes = oReport.PrepareReport(_oRouteSheet, oRouteSheetPackings, oBusinessUnit, oRouteSheetSetup);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintRSPackingCard(int nId)//nId=RouteSheetID
        {
            //List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            string sBatchNo = "", sCombineRSNo = "";
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRouteSheet = new RouteSheet();
            _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            string sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + nId;
            oRouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptRouteSheetLabel oReport = new rptRouteSheetLabel();
            byte[] abytes = oReport.PrepareReportCARD(_oRouteSheet, oRouteSheetPackings, oBusinessUnit, oRouteSheetSetup);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintDynamicLabel(int nId, int docId)//nId=RouteSheetID
        {
            string sBatchNo = "";
            int nLotNo=0;
            EnumExcellColumn eExcellColumn ;
            #region DocPrintEngine
            DocPrintEngine oDocPrintEngine = new DocPrintEngine();

            if (docId <= 0)
            {
                oDocPrintEngine = oDocPrintEngine.GetActiveByTypenModule((int)EnumDocumentPrintType.STICKER_PRINT, (int)EnumModuleName.HangerSticker, (int)Session[SessionInfo.currentUserID]);
            }
            else
                oDocPrintEngine = oDocPrintEngine.Get(docId, (int)Session[SessionInfo.currentUserID]);

            oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region RS & RS PACKING
            _oRouteSheet = new RouteSheet();
            _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            oRouteSheetDOs = RouteSheetDO.GetsBy(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            string sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + nId ;
            oRouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

            #region CREATING LIST

            if (_oRouteSheet.RouteSheetNo.Contains("A"))
            {
                nLotNo = (int)EnumExcellColumn.A;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("B"))
            {
                nLotNo = (int)EnumExcellColumn.B;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("C"))
            {
                nLotNo = (int)EnumExcellColumn.C;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("D"))
            {
                nLotNo = (int)EnumExcellColumn.D;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("E"))
            {
                nLotNo = (int)EnumExcellColumn.E;
            }

            //EnumExcellColumn.A; 19-14626

            foreach (RouteSheetPacking oItem in oRouteSheetPackings)
            {
                oItem.QtyGWWithMunitValue = oItem.QtyGW * oRouteSheetSetup.SMUnitValue;
                oItem.WeightWithMunitValue = oItem.Weight * oRouteSheetSetup.SMUnitValue;
                List<RouteSheetDO> oOrder = oRouteSheetDOs.Where(x => x.DyeingOrderDetailID == oItem.DyeingOrderDetailID).ToList();
                if (oOrder != null)
                {
                    oItem.CustomerName = oOrder.Select(x => x.ContractorName).FirstOrDefault() ;
                    oItem.ColorName = oOrder.Select(x => x.ColorName).FirstOrDefault();
                    oItem.OrderNo = oOrder.Select(x => x.OrderNoFull).FirstOrDefault();
                    oItem.ColorName = oOrder.Select(x => x.ColorName).FirstOrDefault();
                    oItem.ProductName = oOrder.Select(x => x.ProductName).FirstOrDefault();
                }
                sBatchNo = "";
                //oItem.CustomerName = _oRouteSheet.PTU.ContractorName;
                //oItem.OrderNo = _oRouteSheet.PTU.OrderNo;
                if (oItem.YarnType == EnumYarnType.DyedYarnOne)
                {
                   
                    sBatchNo = ((EnumExcellColumn)(nLotNo+1)).ToString();
                }
                if (oItem.YarnType == EnumYarnType.DyedYarnTwo)
                {
                   
                    sBatchNo = ((EnumExcellColumn)(nLotNo+2)).ToString();
                }
                if (oItem.YarnType == EnumYarnType.DyedYarnThree)
                {
                   
                    sBatchNo = ((EnumExcellColumn)(nLotNo+3)).ToString();
                }
                if (!string.IsNullOrEmpty(sBatchNo))
                {
                    oItem.RouteSheetNo = _oRouteSheet.RouteSheetNo + "-" + sBatchNo;
                }
                else { oItem.RouteSheetNo = _oRouteSheet.RouteSheetNo; }
                //oItem.ColorName = _oRouteSheet.PTU.ColorName;
                //oItem.ProductName = _oRouteSheet.ProductName;

            }

            #endregion

            rptDynamicStickerPrint oReport = new rptDynamicStickerPrint();
            byte[] abytes = oReport.PrepareReport_Dynamic_Sticker(oRouteSheetPackings.Cast<object>().ToList(), oCompany, oDocPrintEngine);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintDynamicLabelForSelectedItems(int nId, int docId, string sPackingIDs)//nId=RouteSheetID
        {
            string sBatchNo = "";
            int nLotNo = 0;
            EnumExcellColumn eExcellColumn;
            #region DocPrintEngine
            DocPrintEngine oDocPrintEngine = new DocPrintEngine();

            if (docId <= 0)
            {
                oDocPrintEngine = oDocPrintEngine.GetActiveByTypenModule((int)EnumDocumentPrintType.STICKER_PRINT, (int)EnumModuleName.HangerSticker, (int)Session[SessionInfo.currentUserID]);
            }
            else
                oDocPrintEngine = oDocPrintEngine.Get(docId, (int)Session[SessionInfo.currentUserID]);

            oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region RS & RS PACKING
            _oRouteSheet = new RouteSheet();
            _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            oRouteSheetDOs = RouteSheetDO.GetsBy(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            string sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + nId + " AND PackingID IN (" + sPackingIDs + ")";
            oRouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

            #region CREATING LIST

            if (_oRouteSheet.RouteSheetNo.Contains("A"))
            {
                nLotNo = (int)EnumExcellColumn.A;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("B"))
            {
                nLotNo = (int)EnumExcellColumn.B;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("C"))
            {
                nLotNo = (int)EnumExcellColumn.C;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("D"))
            {
                nLotNo = (int)EnumExcellColumn.D;
            }
            else if (_oRouteSheet.RouteSheetNo.Contains("E"))
            {
                nLotNo = (int)EnumExcellColumn.E;
            }

            //EnumExcellColumn.A; 19-14626

            foreach (RouteSheetPacking oItem in oRouteSheetPackings)
            {
                oItem.QtyGWWithMunitValue = oItem.QtyGW * oRouteSheetSetup.SMUnitValue;
                oItem.WeightWithMunitValue = oItem.Weight * oRouteSheetSetup.SMUnitValue;
                List<RouteSheetDO> oOrder = oRouteSheetDOs.Where(x => x.DyeingOrderDetailID == oItem.DyeingOrderDetailID).ToList();
                if (oOrder != null)
                {
                    oItem.CustomerName = oOrder.Select(x => x.ContractorName).FirstOrDefault();
                    oItem.ColorName = oOrder.Select(x => x.ColorName).FirstOrDefault();
                    oItem.OrderNo = oOrder.Select(x => x.OrderNoFull).FirstOrDefault();
                    oItem.ColorName = oOrder.Select(x => x.ColorName).FirstOrDefault();
                    oItem.ProductName = oOrder.Select(x => x.ProductName).FirstOrDefault();
                }
                sBatchNo = "";
                //oItem.CustomerName = _oRouteSheet.PTU.ContractorName;
                //oItem.OrderNo = _oRouteSheet.PTU.OrderNo;
                if (oItem.YarnType == EnumYarnType.DyedYarnOne)
                {

                    sBatchNo = ((EnumExcellColumn)(nLotNo + 1)).ToString();
                }
                if (oItem.YarnType == EnumYarnType.DyedYarnTwo)
                {

                    sBatchNo = ((EnumExcellColumn)(nLotNo + 2)).ToString();
                }
                if (oItem.YarnType == EnumYarnType.DyedYarnThree)
                {

                    sBatchNo = ((EnumExcellColumn)(nLotNo + 3)).ToString();
                }
                if (!string.IsNullOrEmpty(sBatchNo))
                {
                    oItem.RouteSheetNo = _oRouteSheet.RouteSheetNo + "-" + sBatchNo;
                }
                else { oItem.RouteSheetNo = _oRouteSheet.RouteSheetNo; }
                //oItem.ColorName = _oRouteSheet.PTU.ColorName;
                //oItem.ProductName = _oRouteSheet.ProductName;

            }

            #endregion

            rptDynamicStickerPrint oReport = new rptDynamicStickerPrint();
            byte[] abytes = oReport.PrepareReport_Dynamic_Sticker(oRouteSheetPackings.Cast<object>().ToList(), oCompany, oDocPrintEngine);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintDynamicCard(int nId, int docId)//nId=RouteSheetID
        {
            #region DocPrintEngine
            DocPrintEngine oDocPrintEngine = new DocPrintEngine();

            if (docId <= 0)
            {
                oDocPrintEngine = oDocPrintEngine.GetActiveByTypenModule((int)EnumDocumentPrintType.STICKER_PRINT, (int)EnumModuleName.HangerSticker, (int)Session[SessionInfo.currentUserID]);
            }
            else
                oDocPrintEngine = oDocPrintEngine.Get(docId, (int)Session[SessionInfo.currentUserID]);

            oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region RS & RS PACKING
            _oRouteSheet = new RouteSheet();
            _oRouteSheet = RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRouteSheet.PTU = PTU.Get(_oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            string sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + nId;
            oRouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

            #region CREATING Object
            _oRouteSheet.ContractorName = _oRouteSheet.PTU.ContractorName;
            _oRouteSheet.OrderNo = _oRouteSheet.PTU.OrderNo;
            _oRouteSheet.ColorName = _oRouteSheet.PTU.ColorName;
            _oRouteSheet.WidthWithMU = oRouteSheetPackings.Sum(x => x.Weight).ToString("#,##0.00") + " " + oRouteSheetSetup.MUnit;
            _oRouteSheet.ErrorMessage = DateTime.Now.ToString("dd MMM yyyy");
            List<RouteSheet> oRSs = new List<RouteSheet>();
            oRSs.Add(_oRouteSheet);
            #endregion

            rptDynamicStickerPrint oReport = new rptDynamicStickerPrint();
            byte[] abytes = oReport.PrepareReport_Dynamic_Sticker(oRSs.Cast<object>().ToList(), oCompany, oDocPrintEngine);
            return File(abytes, "application/pdf");
        }

        /*--------- QC-------------------*/

        [HttpPost]
        public JsonResult SaveRSInQCDetail(RSInQCDetail oRSInQCDetail)
        {
            try
            {
                if (oRSInQCDetail.RSInQCDetailID <= 0)
                {
                    oRSInQCDetail = oRSInQCDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRSInQCDetail = oRSInQCDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oRSInQCDetail = new RSInQCDetail();
                oRSInQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSInQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveRSInQCDetailAll(List<RSInQCDetail> oRSInQCDetails)
        {
            try
            {
                if (oRSInQCDetails.Count>0)
                {
                    oRSInQCDetails = RSInQCDetail.SaveAll(oRSInQCDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                RSInQCDetail oRSInQCDetail = new RSInQCDetail();
                oRSInQCDetail.ErrorMessage = ex.Message;
                oRSInQCDetails = new List<RSInQCDetail>();
                oRSInQCDetails.Add(oRSInQCDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSInQCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteRSInQCDetail(RSInQCDetail oRSInQCDetail)
        {
            try
            {
                if (oRSInQCDetail.RSInQCDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oRSInQCDetail = oRSInQCDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRSInQCDetail = new RSInQCDetail();
                oRSInQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSInQCDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RSQCDOneByForce(RouteSheet oRouteSheet)
        {
            try
            {
                if (oRouteSheet.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");
                oRouteSheet = oRouteSheet.RSQCDOneByForce(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RSQCDOne(RouteSheetDO RouteSheetDO)
        {
            try
            {
                if (RouteSheetDO.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");
                _oRouteSheet = _oRouteSheet.RSQCDOne(RouteSheetDO,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheet = new RouteSheet();
                _oRouteSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<RSInQCDetail> GenerateQCInspection(int[] SetupIDs)
        {
            
            List<RSInQCDetail> oRSInQCDetails = new List<RSInQCDetail>();
            try
            {
                List<RSInQCSetup> oRSInQCSetups = new List<RSInQCSetup>();
                string sSQL = "Select * from View_RSInQCSetup Where Activity=1 ";
                if (SetupIDs.Count() > 0)
                    sSQL = sSQL + " And RSInQCSetupID Not In (" + string.Join(",", SetupIDs.Select(x=>x).ToList()) + ")";

                oRSInQCSetups = RSInQCSetup.Gets(sSQL + " order by YarnType ", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oRSInQCSetups.Count() > 0 && oRSInQCSetups.FirstOrDefault().RSInQCSetupID > 0)
                {
                    RSInQCDetail oRSInQCDetail = new RSInQCDetail();
                    foreach (RSInQCSetup oItem in oRSInQCSetups)
                    {
                        oRSInQCDetail = new RSInQCDetail();
                        oRSInQCDetail.RSInQCDetailID = 0;
                        oRSInQCDetail.RouteSheetID = 0;
                        oRSInQCDetail.RSInQCSetupID = oItem.RSInQCSetupID;
                        oRSInQCDetail.Qty = 0;
                        oRSInQCDetail.Note = "";
                        oRSInQCDetail.QCSetupName = oItem.Name;
                        oRSInQCDetail.YarnType = oItem.YarnType;
                        oRSInQCDetails.Add(oRSInQCDetail);
                    }
                }
             
            }
            catch(Exception ex)
            {
                oRSInQCDetails = new List<RSInQCDetail>();
            }

            return oRSInQCDetails;
        }

        
        #endregion

        #region RouteSheet Cancel

        private List<RouteSheetCancel> GetsRSCancels(List<RouteSheetCancel> oRouteSheetCancels)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            string sRSIDs = string.Join(",", oRouteSheetCancels.Select(x => x.RouteSheetID).ToList());
            if (!string.IsNullOrEmpty(sRSIDs))
            {
            //    sRSIDs = "Select * from View_RouteSheet_YarnOut Where RouteSheetID<>0 and RouteSheetID in (" + sRSIDs + ")";
                oRouteSheetDOs = RouteSheetDO.Gets("SELECT * FROM View_RouteSheetDO WHERE RouteSheetID in (" + sRSIDs + ")", (int)Session[SessionInfo.currentUserID]);
                oRSRawLots = RSRawLot.Gets("SELECT * FROM View_RSRawLot WHERE RouteSheetID in (" + sRSIDs + ")", (int)Session[SessionInfo.currentUserID]);

                oRouteSheetCancels.ForEach(x =>
                {
                    if (oRouteSheetDOs.FirstOrDefault() != null && oRouteSheetDOs.FirstOrDefault().RouteSheetID > 0 && oRouteSheetDOs.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                    {
                        x.OrderNo = string.Join("+", oRouteSheetDOs.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.OrderNoFull).Distinct().ToList());
                        x.ContractorName = string.Join("+", oRouteSheetDOs.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.ContractorName).Distinct().ToList());
                        x.BuyerName = string.Join("+", oRouteSheetDOs.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.DeliveryToName).Distinct().ToList());
                        x.ProductName = string.Join("+", oRouteSheetDOs.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.ProductName).Distinct().ToList());
                        x.Qty =  oRouteSheetDOs.Where(b => b.RouteSheetID == x.RouteSheetID).Sum(P => P.Qty_RS);
                    }
                });

                oRouteSheetCancels.ForEach(x =>
                {
                    if (oRSRawLots.FirstOrDefault() != null && oRSRawLots.FirstOrDefault().RouteSheetID > 0 && oRSRawLots.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                    {
                        x.LotNo = string.Join(",", oRSRawLots.Where(b => b.RouteSheetID == x.RouteSheetID).Select(P => P.LotNo).Distinct().ToList());
                    }

                });
            }


            return oRouteSheetCancels;
        }


        public ActionResult ViewRouteSheetCancels(string menuid)
        {
            string token = menuid;
            Menu oMenu = Menu.Get("ViewRouteSheetCancels", "RouteSheet", -9);

            List<RouteSheetCancel> oRouteSheetCancels = new List<RouteSheetCancel>();

            if (token.Contains(Global.DefaultEncodeValue()) && oMenu.MenuID > 0)
            {
                string actualToken = Global.GetActualToken(token);
                try
                {
                    int mid = Convert.ToInt32(actualToken);
                    if (oMenu.MenuID != mid)
                        throw new Exception("Opps! Some this went wrong.");

                    this.Session.Remove(SessionInfo.MenuID);
                    this.Session.Add(SessionInfo.MenuID, mid);
                    this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RouteSheetCancel).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
                    string sSQL = "Select * from View_RouteSheetCancel Where ApproveBy=0";
                    oRouteSheetCancels = RouteSheetCancel.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
                   
                    oWorkingUnits = WorkingUnit.GetsPermittedStore(oBusinessUnit.BusinessUnitID, EnumModuleName.RouteSheetCancel, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
                    ViewBag.WorkingUnits = oWorkingUnits;
                    ViewBag.ReDyeingStatusList = EnumObject.jGets(typeof(EnumReDyeingStatus));

                    ViewBag.EnumCompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();

                    if (oRouteSheetCancels.Count > 0)
                    {
                        oRouteSheetCancels = GetsRSCancels(oRouteSheetCancels);
                    }


                }
                catch (Exception ex)
                {
                    ViewBag.IsError = true;
                    ModelState.AddModelError("Error", "Opps! Something went wrong.");
                }
                return View(oRouteSheetCancels);
            }
            else
            {
                token = Global.GetEncodeValue(token);
                return RedirectToAction("ViewRouteSheetCancels", "RouteSheet", new { menuid = token });
            }
        }

        public ActionResult ViewRouteSheetCancel(string token, double ts)
        {
            RouteSheetCancel oRouteSheetCancel = new RouteSheetCancel();
            RouteSheet oRouteSheet=new RouteSheet();
            if (token.Contains(Global.DefaultEncodeValue()))
            {
                string actualToken = Global.GetActualToken(token);
                try
                {
                    int nId = Convert.ToInt32(actualToken);
                    oRouteSheetCancel = RouteSheetCancel.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if(oRouteSheetCancel.RouteSheetID>0){
                        oRouteSheet=RouteSheet.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    ViewBag.RouteSheet = oRouteSheet;
                }
                catch
                {
                    ViewBag.IsError = true;
                    ModelState.AddModelError("Error", "Opps! Something went wrong.");
                }
                return View(oRouteSheetCancel);
            }
            else
            {
                token = Global.GetEncodeValue(token);
                TimeSpan nts = DateTime.Now - DateTime.MinValue;
                ts= nts.TotalSeconds / 1000;
                return RedirectToAction("ViewRouteSheetCancel", "RouteSheet", new { token = token, ts=ts });
            }
        }

      
        [HttpPost]
        public JsonResult SaveRouteSheetCancel(RouteSheetCancel oRouteSheetCancel)
        {
            List<RouteSheetCancel> oRSCs = new List<RouteSheetCancel>();
            try
            {
                if (oRouteSheetCancel.RouteSheetID <= 0)
                {
                    int nRouteSheetID = 0;
                    int.TryParse(oRouteSheetCancel.Params,out nRouteSheetID);
                    oRouteSheetCancel.RouteSheetID = nRouteSheetID;
                    oRouteSheetCancel = oRouteSheetCancel.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRouteSheetCancel = oRouteSheetCancel.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oRSCs.Add(oRouteSheetCancel);
                oRSCs = GetsRSCancels(oRSCs);
                if (oRSCs.Count > 0) oRouteSheetCancel = oRSCs[0];
            }
            catch (Exception ex)
            {
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetCancel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveRouteSheetCancel(RouteSheetCancel oRouteSheetCancel)
        {
            List<RouteSheetCancel> oRSCs = new List<RouteSheetCancel>();
            try
            {
                if (oRouteSheetCancel.RouteSheetID <= 0)
                    throw new Exception("Invalid Batch/Dyeing Card.");
                oRouteSheetCancel = oRouteSheetCancel.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oRSCs.Add(oRouteSheetCancel);
                oRSCs = GetsRSCancels(oRSCs);
                if (oRSCs.Count > 0) oRouteSheetCancel = oRSCs[0];

            }
            catch (Exception ex)
            {
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetCancel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRouteSheetCancel(RouteSheetCancel oRouteSheetCancel)
        {
            try
            {
                if (oRouteSheetCancel.RouteSheetID <= 0) { throw new Exception("Please select an valid item."); }
                oRouteSheetCancel = oRouteSheetCancel.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetCancel.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetRouteSheetCancel(RouteSheetCancel oRouteSheetCancel)
        {
            RouteSheet oRouteSheet = new RouteSheet();   
            try
            {
                oRouteSheet = RouteSheet.Get(oRouteSheetCancel.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRouteSheetCancel = RouteSheetCancel.Get(oRouteSheetCancel.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
                oRouteSheetCancel.RouteSheet = oRouteSheet;

            }
            catch (Exception ex)
            {
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetCancel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRouteSheetCancel(RouteSheetCancel oRouteSheetCancel)
        {
            List<RouteSheetCancel> oRouteSheetCancels = new List<RouteSheetCancel>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheetCancel.RouteSheetNo)) ? "" : oRouteSheetCancel.RouteSheetNo.Trim();
                bool bIsInitialized = Convert.ToBoolean(oRouteSheetCancel.Params.Split('~')[0]);
                short nComparator = (short)(EnumCompareOperator)Convert.ToInt16(oRouteSheetCancel.Params.Split('~')[1]);
                DateTime dtFrom = Convert.ToDateTime(oRouteSheetCancel.Params.Split('~')[2]);
                DateTime dtTo = Convert.ToDateTime(oRouteSheetCancel.Params.Split('~')[3]);
                string sOrderNo = oRouteSheetCancel.Params.Split('~')[4];

                string sSQL = "Select * FROM View_RouteSheetCancel Where RouteSheetID<>0";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                if (bIsInitialized) sSQL = sSQL + " And ApproveBy=0";
                if (nComparator > 0)
                {
                    string sDateStr = "";
                    if (nComparator == (int)EnumCompareOperator.EqualTo)
                        sDateStr = "Select RouteSheetID from RouteSheetHistory Where CurrentStatus=17 And CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtFrom.ToString("dd MMM yyyy") + "', 106))";
                    else if (nComparator == (int)EnumCompareOperator.NotEqualTo)
                        sDateStr = "Select RouteSheetID from RouteSheetHistory Where CurrentStatus=17 And CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtFrom.ToString("dd MMM yyyy") + "', 106))";
                    else if (nComparator == (int)EnumCompareOperator.GreaterThan)
                        sDateStr = "Select RouteSheetID from RouteSheetHistory Where CurrentStatus=17 And CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtFrom.ToString("dd MMM yyyy") + "', 106))";
                    else if (nComparator == (int)EnumCompareOperator.SmallerThan)
                        sDateStr = "Select RouteSheetID from RouteSheetHistory Where CurrentStatus=17 And CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtFrom.ToString("dd MMM yyyy") + "', 106))";
                    else if (nComparator == (int)EnumCompareOperator.Between)
                        sDateStr = "Select RouteSheetID from RouteSheetHistory Where CurrentStatus=17 And CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtFrom.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtTo.ToString("dd MMM yyyy") + "', 106))";
                    else if (nComparator == (int)EnumCompareOperator.NotBetween)
                        sDateStr = "Select RouteSheetID from RouteSheetHistory Where CurrentStatus=17 And CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtFrom.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dtTo.ToString("dd MMM yyyy") + "', 106))";

                    sSQL = sSQL + ((bIsInitialized) ? " Or " : " And ") + " RouteSheetID In (" + sDateStr + ")";
                }
                if (!string.IsNullOrEmpty(sOrderNo))
                    sSQL = sSQL + " AND RouteSheetID IN (SELECT RouteSheetID FROM View_RouteSheetDO WHERE OrderNo LIKE '%" + sOrderNo + "%')";
             
                oRouteSheetCancels = RouteSheetCancel.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRouteSheetCancels.Count > 0)
                {
                    oRouteSheetCancels = GetsRSCancels(oRouteSheetCancels);
                }
            }
            catch (Exception ex)
            {
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancels = new List<RouteSheetCancel>();
                oRouteSheetCancel.ErrorMessage = ex.Message;
                oRouteSheetCancels.Add(oRouteSheetCancel);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetCancels);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsRouteSheetForCancel(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.RouteSheetNo)) ? "" : oRouteSheet.RouteSheetNo.Trim();
                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState In (4,6,7,8,9,10,11,12) And RouteSheetID Not In (Select RouteSheetID from RouteSheetCancel)";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                _oRouteSheets = new List<RouteSheet>();
                oRouteSheet.ErrorMessage = ex.Message;
                _oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetRouteSheetData(RouteSheetCancel oRouteSheetCancel)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oRouteSheetCancel);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintRouteSheetCancels()
        {
            List<RouteSheetCancel> oRouteSheetCancels = new List<RouteSheetCancel>();
            RouteSheetCancel oRouteSheetCancel = new RouteSheetCancel();
            try
            {
                oRouteSheetCancel = (RouteSheetCancel)Session[SessionInfo.ParamObj];
                string sSQL = "Select * FROM View_RouteSheetCancel Where RouteSheetID<>0 AND RouteSheetID IN (" + oRouteSheetCancel.ErrorMessage + ") Order By RouteSheetID";
                oRouteSheetCancels = RouteSheetCancel.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oRouteSheetCancels.Count > 0)
                {
                    oRouteSheetCancels = GetsRSCancels(oRouteSheetCancels);
                }
            }
            catch (Exception ex)
            {
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancels = new List<RouteSheetCancel>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptRouteSheetCancels oReport = new rptRouteSheetCancels();
            byte[] abytes = oReport.PrepareReport(oRouteSheetCancels, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ExcelRouteSheetCancels()
        {
            List<RouteSheetCancel> oRouteSheetCancels = new List<RouteSheetCancel>();
            RouteSheetCancel oRouteSheetCancel = new RouteSheetCancel();
            try
            {
                oRouteSheetCancel = (RouteSheetCancel)Session[SessionInfo.ParamObj];
                string sSQL = "Select * FROM View_RouteSheetCancel Where RouteSheetID<>0 AND RouteSheetID IN (" + oRouteSheetCancel.ErrorMessage + ") Order By RouteSheetID";
              
                oRouteSheetCancels = RouteSheetCancel.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oRouteSheetCancels.Count > 0)
                {
                    oRouteSheetCancels = GetsRSCancels(oRouteSheetCancels);
                }

            }
            catch (Exception ex)
            {
                oRouteSheetCancel = new RouteSheetCancel();
                oRouteSheetCancels = new List<RouteSheetCancel>();
            }

            if (oRouteSheetCancels.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch/Lot No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Lot No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Assign Lot", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Count", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Store", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Requested By", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Approved By", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Requested Remarks", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Approval Remarks", Width = 20f, IsRotate = false });
                
                #endregion


                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Route Sheet Cancel List");
                    
                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion
                    nRowIndex++;
                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = "Route Sheet Cancel List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 2;
                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    #region Data

                    nRowIndex++;


                    int nCount = 0;
                    foreach (var oItem in oRouteSheetCancels)
                    {
                        nStartCol = 2;
                        #region DATA
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DBServerDateTime.ToString("dd MMM yy mm:tt"), false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RouteSheetNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.OrderNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.IsNewLotStr, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.StoreName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RequestedByName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ApprovedByName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Remarks, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ApprovalRemarks, false, ExcelHorizontalAlignment.Left, false, false);

                        #endregion
                        nRowIndex++;

                    }
                    #endregion

                    #region Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right);
                    ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    nStartCol = 7;
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetCancels.Select(x => x.Qty).Sum().ToString(), true, true);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    #endregion
                    



                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=RouteSheetCancelList.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }

        #endregion

        #region Actions RS DETAIL ADDITIONAL

        public ActionResult ViewRSDetailAdditonals(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oRSDetailAdditonals = new List<RSDetailAdditonal>();
            _oRSDetailAdditonals = RSDetailAdditonal.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oRSDetailAdditonals);
        }

        public ActionResult ViewRSDetailAdditonal(int id)
        {
            _oRSDetailAdditonal = new RSDetailAdditonal();
            if (id > 0)
            {
                _oRSDetailAdditonal = _oRSDetailAdditonal.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oRSDetailAdditonal);
        }

        [HttpPost]
        public JsonResult SaveRSDA(RSDetailAdditonal oRSDetailAdditonal)
        {
            _oRSDetailAdditonal = new RSDetailAdditonal();
            try
            {
                _oRSDetailAdditonal = oRSDetailAdditonal;
                _oRSDetailAdditonal = _oRSDetailAdditonal.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRSDetailAdditonal = new RSDetailAdditonal();
                _oRSDetailAdditonal.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRSDetailAdditonal);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRSDA(RSDetailAdditonal oRSDetailAdditonal)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oRSDetailAdditonal.Delete(oRSDetailAdditonal.RSDetailAdditonalID, (int)Session[SessionInfo.currentUserID]);
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

        #region Validation
        private string RSStateValidation(EnumRSState CurrentState, EnumRSState NextState)
        {
            try
            {
                if (CurrentState == EnumRSState.None)
                    throw new Exception("Invalid routesheet status.");

                if (CurrentState == EnumRSState.Initialized && NextState != EnumRSState.InFloor)
                    throw new Exception("Reqired to send in floor.");

                if (CurrentState == EnumRSState.InFloor && NextState != EnumRSState.Initialized && NextState != EnumRSState.YarnOut)
                    throw new Exception("Reqired to yarn In Floor status.");
                //if (CurrentState == EnumRSState.Approved &&  NextState != EnumRSState.InYarnStore)
                //    throw new Exception("Reqired to yarn In Yarn Store.");

                if (CurrentState == EnumRSState.YarnOut && NextState != EnumRSState.LoadedInDyeMachine)
                    throw new Exception("Reqired to loaded in dye machine.");

                if (CurrentState == EnumRSState.LoadedInDyeMachine && NextState != EnumRSState.UnloadedFromDyeMachine)
                    throw new Exception("Reqired to unload from dye machine.");

                if (CurrentState == EnumRSState.UnloadedFromDyeMachine && NextState != EnumRSState.LoadedInHydro)
                    throw new Exception("Reqired to loaded in hydro machine.");

                if (CurrentState == EnumRSState.LoadedInHydro && NextState != EnumRSState.UnloadedFromHydro)
                    throw new Exception("Reqired to unload from hydro machine.");

                if (CurrentState == EnumRSState.UnloadedFromHydro && NextState != EnumRSState.LoadedInDrier)
                    throw new Exception("Reqired to loaded in drier machine.");

                if (CurrentState == EnumRSState.LoadedInDrier && NextState != EnumRSState.UnLoadedFromDrier)
                    throw new Exception("Reqired to unload from drier machine.");
                if (CurrentState == EnumRSState.LoadedInDrier && NextState != EnumRSState.UnLoadedFromDrier)
                    throw new Exception("Reqired to unload from drier machine.");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }
        private string RSStateValidation_Progress(EnumRSState CurrentState, EnumRSState NextState)
        {
            try
            {
                if (CurrentState == EnumRSState.None)
                    throw new Exception("Invalid routesheet status.");

                if (CurrentState == EnumRSState.Initialized && NextState != EnumRSState.InFloor)
                    throw new Exception("Reqired to send in floor.");

                if (CurrentState == EnumRSState.InFloor && NextState != EnumRSState.Initialized && NextState != EnumRSState.YarnOut)
                    throw new Exception("Reqired to yarn In Floor status.");
                //if (CurrentState == EnumRSState.Approved &&  NextState != EnumRSState.InYarnStore)
                //    throw new Exception("Reqired to yarn In Yarn Store.");

                if (CurrentState == EnumRSState.YarnOut && NextState != EnumRSState.LoadedInDyeMachine)
                    throw new Exception("Reqired to loaded in dye machine.");

                if (CurrentState == EnumRSState.LoadedInDyeMachine && !(NextState != EnumRSState.UnloadedFromDyeMachine || NextState != EnumRSState.LoadedInHydro))
                    throw new Exception("Reqired to unload from dye machine.");

                if (CurrentState == EnumRSState.UnloadedFromDyeMachine && NextState != EnumRSState.LoadedInHydro)
                    throw new Exception("Reqired to loaded in hydro machine.");

                if (CurrentState == EnumRSState.LoadedInHydro && !(NextState != EnumRSState.UnloadedFromDyeMachine || NextState != EnumRSState.UnloadedFromHydro || NextState != EnumRSState.LoadedInDrier))
                    throw new Exception("Reqired to unload from hydro machine.");

                if (CurrentState == EnumRSState.UnloadedFromHydro && NextState != EnumRSState.LoadedInDrier)
                    throw new Exception("Reqired to loaded in drier machine.");

                if (CurrentState == EnumRSState.LoadedInDrier && !(NextState != EnumRSState.UnloadedFromHydro || NextState != EnumRSState.UnLoadedFromDrier))
                    throw new Exception("Reqired to unload from drier machine.");
                //if (CurrentState == EnumRSState.LoadedInDrier && NextState != EnumRSState.UnLoadedFromDrier)
                //    throw new Exception("Reqired to unload from drier machine.");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        #endregion

        #region Sequence
        [HttpPost]
        public JsonResult GetRSDChilds(RouteSheetDetail oRouteSheetDetail)
        {
            List<RouteSheetDetail> oRouteSheetDetails = new List<RouteSheetDetail>();
            try
            {
                RouteSheetDetail oTempDST = new RouteSheetDetail();
                oTempDST = RouteSheetDetail.Get(oRouteSheetDetail.ParentID, (int)Session[SessionInfo.currentUserID]);
                oRouteSheetDetail.ProcessName = "Selected parent proces : " + oTempDST.ProcessName;
                string sSQL = "SELECT * FROM VIEW_RouteSheetDetail WHERE ParentID = " + oRouteSheetDetail.ParentID + " Order By Sequence";
                oRouteSheetDetail.children = RouteSheetDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult RefreshMenuSequence(RouteSheetDetail oRouteSheetDetail)
        {
            try
            {
                oRouteSheetDetail = oRouteSheetDetail.RefreshSequence((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRouteSheetDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        [HttpPost]
        public JsonResult GetLabdipRecipe(PTU oPTU)
        {
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();
            try
            {
                oLabdipRecipes = LabdipRecipe.Gets(oPTU.LabDipDetailID, oPTU.Shade, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                LabdipRecipe oLabdipRecipe = new LabdipRecipe();
                oLabdipRecipes = new List<LabdipRecipe>();
                oLabdipRecipe.ErrorMessage = ex.Message;
                oLabdipRecipes.Add(oLabdipRecipe);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabdipRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Product BU, User and Name wise ( write by Mamun)
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            EnumProductUsages eEnumProductUsages = new EnumProductUsages();
            try
            {
                if (oProduct.ProductCategoryID == (int)EnumProductNature.Dyes)
                {
                    eEnumProductUsages = EnumProductUsages.Dyes;
                }
                else if (oProduct.ProductCategoryID == (int)EnumProductNature.Chemical)
                {
                    eEnumProductUsages = EnumProductUsages.Chemical;
                }
                else
                {
                    eEnumProductUsages = EnumProductUsages.Dyes_Chemical;
                }
                //if (oProduct.ProductCategoryID == 25)
                //{
                //    eEnumProductUsages = EnumProductUsages.Dyes;
                //}
                //else if (oProduct.ProductCategoryID == 26)
                //{
                //    eEnumProductUsages = EnumProductUsages.Chemical;
                //}
                //else
                //{
                //    eEnumProductUsages = EnumProductUsages.Dyes_Chemical;
                //}

                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.Labdip, eEnumProductUsages, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.Labdip, eEnumProductUsages, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        public ActionResult PrintCard(int nID, int buid)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            oRouteSheetSetup = oRouteSheetSetup.GetBy( ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRouteSheet = new RouteSheet();
            _oRouteSheet = RouteSheet.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRSRawLots = RSRawLot.GetsByRSID(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();

            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            oRouteSheetDOs = RouteSheetDO.GetsBy(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRouteSheet.RouteSheetDOs = oRouteSheetDOs;

            if (_oRouteSheet.DUPScheduleID > 0)
            {
                oDUPScheduleDetails = DUPScheduleDetail.Gets(_oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                if (oRSRawLots.Count <= 0)
                {
                    oDUPSLots = DUPSLot.Gets("SELECT * FROM View_DUPSLot WHERE DUPScheduleID=" + _oRouteSheet.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }

         
            if (oDUPSLots.Count > 0)
            {
                // oRSRawLots = RSRawLot.GetsByRSID(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oRouteSheet.LotNo = string.Join(",", oDUPSLots.Select(x => x.LotNo).ToList());
            }
            if (oRSRawLots.Count > 0)
            {
                _oRouteSheet.LotNo = string.Join(",", oRSRawLots.Select(x => x.LotNo).ToList());
            }

            rptRouteSheetCard orptBatchCard = new rptRouteSheetCard();
            byte[] abytes = orptBatchCard.PrepareReport(oCompany, _oRouteSheet, oBusinessUnit, oDUPScheduleDetails, oRouteSheetSetup);
            return File(abytes, "application/pdf");
        }
    }
}