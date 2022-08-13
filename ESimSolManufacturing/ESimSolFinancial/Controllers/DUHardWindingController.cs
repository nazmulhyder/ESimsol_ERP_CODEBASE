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
using iTextSharp.text.pdf;
using System.Web.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUHardWindingController : Controller
    {
        #region Declaration
        DUHardWinding _oDUHardWinding = new DUHardWinding();
        List<DUHardWinding> _oDUHardWindings = new List<DUHardWinding>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        public ActionResult ViewDUHardWindings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUHardWinding).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oDUHardWindings = new List<DUHardWinding>();
            _oDUHardWindings = DUHardWinding.Gets("SELECT * FROM View_DUHardWinding WHERE isnull([Status],0) NOT IN (" + (int)EnumWindingStatus.Completed + "," + (int)EnumWindingStatus.Delivered + ") ORDER BY DUHardWindingID", (int)Session[SessionInfo.currentUserID]);
           
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RSShifts = RSShift.GetsByModule(buid, (int)EnumModuleName.DUHardWinding +"",((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.WarpWeftType = EnumObject.jGets(typeof(EnumWarpWeft));
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RSInQCSetup = EnumObject.jGets(typeof(EnumYarnType));// RSInQCSetup.Gets("Select * from View_RSInQCSetup Where Activity=1 order by YarnType ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oDUHardWindings);
        }
        public ActionResult ViewDUHardWinding(int id, int buid)
        {
            _oDUHardWinding = new DUHardWinding();
            if (id > 0)
            {
                _oDUHardWinding = _oDUHardWinding.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            #region RSShift
            List<RSShift> oRSShifts = new List<RSShift>();
            oRSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.DUHardWinding, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            #region Machine
            List<Machine> oMachines = new List<Machine>();
            oMachines = Machine.GetsByModule(buid, "" + (int)EnumModuleName.DUHardWinding, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.Machines = oMachines;
            ViewBag.RSShifts = oRSShifts;
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.DUHardWindingTypes = EnumObject.jGets(typeof(EnumColorType));
            ViewBag.WarpWeftType = EnumObject.jGets(typeof(EnumWarpWeft));
            return View(_oDUHardWinding);
        }
        public ActionResult ViewDUHardWindingYarnOut(int id)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            string sSQL = "";
            if (id > 0)
            {
                _oDUHardWinding = _oDUHardWinding.Get(id, (int)Session[SessionInfo.currentUserID]);
                sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 ";
                sSQL = sSQL + "AND RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where ProductID=" + _oDUHardWinding.ProductID + " and DyeingOrderID=" + _oDUHardWinding.DyeingOrderID + " ))";
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                sSQL = "Select * from View_RouteSheet Where RSState in (3) and LotID in (Select LotID from DUHardWinding)";
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            return View(oRouteSheets);
        }
        public ActionResult AdvSearchDUHardWinding()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Save(DUHardWinding oDUHardWinding)
        {
            _oDUHardWinding = new DUHardWinding();
            try
            {
                _oDUHardWinding = oDUHardWinding;
                _oDUHardWinding = _oDUHardWinding.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWinding = new DUHardWinding();
                _oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Rewind(DUHardWinding oDUHardWinding)
        {
            _oDUHardWinding = new DUHardWinding();
            try
            {
                _oDUHardWinding = _oDUHardWinding.Get(oDUHardWinding.DUHardWindingID, (int)Session[SessionInfo.currentUserID]);

                if (oDUHardWinding.DUHardWindingID <= 0) throw new Exception("Invalid Hardwinding !");
                if (oDUHardWinding.Balance < oDUHardWinding.Qty) throw new Exception("Invalid Rewinding Qty !");

                //_oDUHardWinding.DUHardWindingID = 0;
                _oDUHardWinding.IsRewinded = true;
                _oDUHardWinding.ReceiveDate = DateTime.Now;
                _oDUHardWinding.StartDate = DateTime.Now;
                _oDUHardWinding.EndDate = DateTime.Now;
                _oDUHardWinding.Qty = oDUHardWinding.Qty; //Rewinding Qty
                _oDUHardWinding.NumOfCone = oDUHardWinding.NumOfCone; //Rewinding Qty
                
                _oDUHardWinding = _oDUHardWinding.Rewinding((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWinding = new DUHardWinding();
                _oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Receive(DUHardWinding oDUHardWinding)
        {
            _oDUHardWinding = new DUHardWinding();
            try
            {
                _oDUHardWinding = oDUHardWinding;
                _oDUHardWinding = _oDUHardWinding.Receive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWinding = new DUHardWinding();
                _oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DODAssign(DUHardWinding oDUHardWinding)
        {
            _oDUHardWinding = new DUHardWinding();
            try
            {
                _oDUHardWinding = oDUHardWinding;
                _oDUHardWinding = _oDUHardWinding.DODAssign((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWinding = new DUHardWinding();
                _oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateReceivedate(DUHardWinding oDUHardWinding)
        {
            _oDUHardWinding = new DUHardWinding();
            try
            {
                _oDUHardWinding = oDUHardWinding;
                _oDUHardWinding = _oDUHardWinding.UpdateReceivedate((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWinding = new DUHardWinding();
                _oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                DUHardWinding oDUHardWinding = new DUHardWinding();
                sFeedBackMessage = oDUHardWinding.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult RouteSheetYarnOut(string sTemp)
        {
            DUHardWinding oDUHardWinding = new DUHardWinding();
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                if (string.IsNullOrEmpty(sTemp)) throw new Exception("Please select a valid routesheet.");

                //int.TryParse(oRouteSheet.Params, out nEventEmpID);
                //string ErrorMessage = RSStateValidation(oRouteSheet.RSState, EnumRSState.YarnOut);
                //if (ErrorMessage != "") { throw new Exception(ErrorMessage); }

                oRouteSheets = oDUHardWinding.YarnOut(sTemp, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsByNo(DUHardWinding oDUHardWinding)
        {
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();
            try
            {
                string sSQL = "SELECT * FROM View_DUHardWinding WHERE ISNULL(DyeingOrderNo,'')+ISNULL(LotNo,'') LIKE '%"+oDUHardWinding.DyeingOrderNo+"%' ORDER BY DUHardWindingID DESC";
                oDUHardWindings = DUHardWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUHardWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRouteSheetWithPacking(DUHardWinding oDUHardWinding)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
            try
            {
                if (oDUHardWinding.RouteSheetID <= 0)
                    throw new Exception("Please enter routesheet no to search.");

                string sSQL = "Select * from View_RouteSheet Where RouteSheetID<>0 And RSState In (9,10,11,12,13,14,15,16,18) And RouteSheetID ='" + oDUHardWinding.RouteSheetID + "'";
                oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oRouteSheets.FirstOrDefault() != null && oRouteSheets.FirstOrDefault().RouteSheetID > 0)
                {
                    oRouteSheet = oRouteSheets.FirstOrDefault();

                    _oDUHardWinding = new DUHardWinding();
                    _oDUHardWinding = _oDUHardWinding.Get(oDUHardWinding.DUHardWindingID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.Qty = _oDUHardWinding.Qty; 

                  //  oRouteSheet.PTU = PTU.Get(oRouteSheet.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oRouteSheetHistory = oRouteSheetHistory.GetBy(oRouteSheet.RouteSheetID, (int)EnumRSState.QC_Done, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.Note = oRouteSheetHistory.Note;
                    sSQL = "Select * from View_RoutesheetPacking Where RouteSheetID=" + oDUHardWinding.RouteSheetID + " AND DyeingOrderDetailID=" + oDUHardWinding.DyeingOrderDetailID;
                    oRouteSheet.RouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSQL = "Select * from View_RSInQCDetail Where RouteSheetID=" + oDUHardWinding.RouteSheetID + " AND DyeingOrderDetailID=" + oDUHardWinding.DyeingOrderDetailID;
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
                            oRSInQCDetail.DyeingOrderDetailID = oDUHardWinding.DyeingOrderDetailID;
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
        public JsonResult GetSHWPacking(DUHardWinding oDUHardWinding)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            string sSQL = "";
            try
            {
                if (oDUHardWinding.DUHardWindingID <= 0)
                    throw new Exception("Please enter routesheet no to search.");

                if (oDUHardWinding.DUHardWindingID > 0 && oDUHardWinding.DyeingOrderDetailID>0)
                {

                    _oDUHardWinding = new DUHardWinding();
                    _oDUHardWinding = _oDUHardWinding.Get(oDUHardWinding.DUHardWindingID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderDetail = oDyeingOrderDetail.Get(oDUHardWinding.DyeingOrderDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.Qty = _oDUHardWinding.Qty;
                    //oRouteSheetHistory = oRouteSheetHistory.GetBy(oRouteSheet.RouteSheetID, (int)EnumRSState.QC_Done, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oRouteSheet.Note = oRouteSheetHistory.Note;
                    sSQL = "Select * from View_RoutesheetPacking Where DUHardWindingID=" + oDUHardWinding.DUHardWindingID + " AND DyeingOrderDetailID=" + oDUHardWinding.DyeingOrderDetailID+" Order by BagNo";
                    oRouteSheet.RouteSheetPackings = RouteSheetPacking.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSQL = "Select * from View_RSInQCDetail Where DUHardWindingID=" + oDUHardWinding.DUHardWindingID + " AND DyeingOrderDetailID=" + oDUHardWinding.DyeingOrderDetailID;
                    oRouteSheet.RSInQCDetails = RSInQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RSInQCDetails.AddRange(this.GenerateQCInspection(oRouteSheet.RSInQCDetails.Select(x => x.RSInQCSetupID).ToArray()));
                    //oRouteSheet.RouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //oRouteSheet.RouteSheetDOs = RouteSheetDO.Gets("Select * from View_RouteSheetDO where RouteSheetID in (" + oRouteSheet.RouteSheetID + ") AND DyeingOrderDetailID="+ oDUHardWinding.DyeingOrderDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRouteSheet.RouteSheetDOs = new List<RouteSheetDO>();
                    oRouteSheet.RSInQCDetails = oRouteSheet.RSInQCDetails.OrderBy(x => x.YarnType).ThenBy(x => x.QCSetupName).ToList();
                    double nQty = (oRouteSheet.RouteSheetPackings.Count() > 0 && oRouteSheet.RouteSheetPackings.FirstOrDefault().PackingID > 0) ? oRouteSheet.RouteSheetPackings.Sum(x => x.Weight) : 0;
                    if (_oDUHardWinding.IsQCDone==false)
                    {
                        if (nQty > 0 && oRouteSheet.RSInQCDetails.Where(x => x.RSInQCDetailID <= 0 && x.YarnType == EnumYarnType.FreshDyedYarn).Any())
                        {
                            oRouteSheet.RSInQCDetails.FirstOrDefault().Qty = nQty;

                            var oRSInQCDetail = oRouteSheet.RSInQCDetails.FirstOrDefault();
                            oRSInQCDetail.RouteSheetID = oRouteSheet.RouteSheetID;
                            oRSInQCDetail.DyeingOrderDetailID = oDUHardWinding.DyeingOrderDetailID;
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

        private List<RSInQCDetail> GenerateQCInspection(int[] SetupIDs)
        {
            List<RSInQCDetail> oRSInQCDetails = new List<RSInQCDetail>();
            try
            {
                List<RSInQCSetup> oRSInQCSetups = new List<RSInQCSetup>();
                string sSQL = "Select * from View_RSInQCSetup Where Activity=1 ";
                if (SetupIDs.Count() > 0)
                    sSQL = sSQL + " And RSInQCSetupID Not In (" + string.Join(",", SetupIDs.Select(x => x).ToList()) + ")";

                oRSInQCSetups = RSInQCSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

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
            catch (Exception ex)
            {
                oRSInQCDetails = new List<RSInQCDetail>();
            }

            return oRSInQCDetails;
        }

        
        [HttpPost]
        public JsonResult SendToRequsitionToDeliveryHW(DUHardWinding oDUHardWinding)
        {
            string sReturn = "";
            _oDUHardWinding = new DUHardWinding();
            List<Lot> oLots = new List<Lot>();
            oLots = Lot.Gets("SELECT * FROM View_Lot WHERE ParentType=106 and ParentID=" + oDUHardWinding.RouteSheetID + " and WorkingUnitID=10  ORDER BY LotID ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            try
            {
                if (oLots.Count <= 0)
                {
                    sReturn = "First Hard winding Lot Not found";
                }
                else
                {
                    _oDUHardWinding.LotID = oLots[0].LotID;
                    _oDUHardWinding.Qty = oLots[0].Balance;
                    oDUHardWinding = oDUHardWinding.SendToDelivery( ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oDUHardWinding = new DUHardWinding();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SendToRequsitionToDeliveryHWPartily(DUHardWinding oDUHardWinding)
        {
            ITransaction oITransaction = new ITransaction();
            List<ITransaction> oITransactions = new List<ITransaction>();
            _oDUHardWinding = new DUHardWinding();
             try
            {
                    _oDUHardWinding = oDUHardWinding.SendToDelivery(((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (!string.IsNullOrEmpty(_oDUHardWinding.ErrorMessage)) throw new Exception("Receive not possible. " + _oDUHardWinding.ErrorMessage);
                    oITransactions = ITransaction.Gets("Select * from View_ITransaction where TriggerParentType=" + (int)EnumTriggerParentsType.TransferRequisitionDetail + " and InOutType=" + (int)EnumInOutType.Receive + " and  TriggerParentID in (Select HH.TRSDetailID from TransferRequisitionSlipDetail as HH where LotID in (select LotID from DUHardWinding where LotID>0 and DUHardWindingID=" + oDUHardWinding.DUHardWindingID + "))  order by TransactionTime,ITransactionID ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oITransactions = new List<ITransaction>();

               
                oITransaction.ErrorMessage = ex.Message;
                oITransactions.Add(oITransaction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITransactions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsITransactionBYRS(DUHardWinding oDUHardWinding)
        {
            double nQty = 0;
            List<ITransaction> oITransactions = new List<ITransaction>();
            try
            {
                //if (oDUHardWinding.RouteSheetID >0)
                //{
                    oITransactions = ITransaction.Gets("Select * from View_ITransaction where TriggerParentType=" + (int)EnumTriggerParentsType.TransferRequisitionDetail + " and InOutType=" + (int)EnumInOutType.Receive + " and  TriggerParentID in (Select HH.TRSDetailID from TransferRequisitionSlipDetail as HH where LotID=" + oDUHardWinding.LotID + ")    order by TransactionTime,ITransactionID  ", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                //else
                //{
                //    oITransactions = ITransaction.Gets("Select * from View_ITransaction where  InOutType=" + (int)EnumInOutType.Disburse + " and  LotID=" + oDUHardWinding.LotID + "  order by TransactionTime,ITransactionID  ", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}

                foreach (ITransaction oItem in oITransactions)
                {
                    nQty = nQty + oItem.Qty;
                    oItem.CurrentBalance = nQty;
                }
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
        public JsonResult RSQCDOne(DUHardWinding oDUHardWinding)
        {
            try
            {
                if (oDUHardWinding.DUHardWindingID <= 0) throw new Exception("Please select a valid HW.");
                oDUHardWinding = oDUHardWinding.RSQCDOne(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUHardWinding = new DUHardWinding();
                oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region Gets
        public JsonResult GetsFabricBeamFinish(int nDyeingOrderID)
        {
            List<FabricBeamFinish> _oFabricBeamFinishs = new List<FabricBeamFinish>();
            try
            {
                //string Ssql = "SELECT * FROM View_FabricBeamFinish WHERE DyeingOrderID=" + nDyeingOrderID + " ";
                string Ssql = "SELECT * FROM View_FabricBeamFinish WHERE FSCDID=(SELECT FSCDetailID FROM DyeingOrder WHERE DyeingOrderID = " + nDyeingOrderID + ") ";
                _oFabricBeamFinishs = new List<FabricBeamFinish>();
                _oFabricBeamFinishs = FabricBeamFinish.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBeamFinishs = new List<FabricBeamFinish>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBeamFinishs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsData(DUHardWinding objDUHardWinding)
        {
            List<DispoHW> oDispoHWList = new List<DispoHW>();
            string sSQL = " where FEOSID in ( Select FEOSID from DyeingOrderFabric where DyeingOrderID=" + objDUHardWinding.DyeingOrderID + ")";
            oDispoHWList = DispoHW.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jSonResult = Json(oDispoHWList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
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
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        #endregion

        #region Advance Search
        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();
            DUHardWinding oDUHardWinding = new DUHardWinding();
            try
            {
                string sSQL = GetSQL(sTemp);
                oDUHardWindings = DUHardWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUHardWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUHardWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nCount = 0;
            int nOrderDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtDUHardWindingEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nReceiveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nQCDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtQCDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtQCDateEnd = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            string sOrderNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            string sLotNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            bool nYTStart = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTComplete = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTDelivery = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nIsDefault = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nIsWaitingForColorAssign = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nIsRewinding = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool bIsGreyYarn = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            //int nRequisitionType = Convert.ToInt32(sTemp.Split('~')[9]);
            //string sRequisitionNo = Convert.ToString(sTemp.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_DUHardWinding";
            string sReturn = "";

            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion

            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderNo Like '%" + sOrderNo + "%'";
            }
           
            #endregion
            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo Like '%" + sLotNo + "%'";
            }
            #endregion
            #region nYTStart
            if (nYTStart)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Initialize;
            }
            #endregion
            #region nYTComplete
            if (nYTComplete)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Running;
            }
            #endregion
            
            #region nYTDelivery
            if (nYTDelivery)
            {
                Global.TagSQL(ref sReturn);
                
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Completed;
            }
            #endregion
            #region nIsDefault
            if (nIsDefault)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull([Status],0) NOT IN (" + (int)EnumWindingStatus.Completed + "," + (int)EnumWindingStatus.Delivered + ")";

            }
            #endregion
            #region nIsWaitingForColorAssign
            if (nIsWaitingForColorAssign)
            {
                Global.TagSQL(ref sReturn);

                sReturn = sReturn + "isnull(DyeingOrderDetailID,0)=0";
            }
            #endregion
            #region nIsRewinding
            if (nIsRewinding)
            {
                Global.TagSQL(ref sReturn);

                sReturn = sReturn + " isnull(IsRewinded,0)=1";
            }
            #endregion
            #region bIsGreyYarn
            if (bIsGreyYarn)
            {
                Global.TagSQL(ref sReturn);

                sReturn = sReturn + " isnull(RouteSheetID,0)=0";
            }
            #endregion
            #region Order Date Wise
            if (nOrderDateCompare > 0)
            {
                if (nOrderDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUHardWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUHardWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Receive Date Wise
            if (nReceiveDateCompare > 0)
            {
                if (nReceiveDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUHardWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUHardWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region QC Date Wise 
            //SELECT QCDate,ColorName FROM View_DUHardWinding
            if (nQCDateCompare > 0)
            {
                if (nQCDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nQCDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nQCDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nQCDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nQCDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateEnd.ToString("dd MMM yyyy") + "',106))";
                }
                if (nQCDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtQCDateEnd.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Report
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
        public ActionResult PrintDUHardWinding(string sTemp)
        {
            List<DUHardWinding> oDUHardWindingList = new List<DUHardWinding>();
            DUHardWinding oDUHardWinding = new DUHardWinding();

            int nBUID;
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                string sSQL = GetSQL(sTemp);
                oDUHardWindingList = DUHardWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[15]);
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUHardWinding oReport = new rptDUHardWinding();
            byte[] abytes = oReport.PrepareReport(oDUHardWindingList, oCompany, oBusinessUnit, "Hard Winding Report", "");
            return File(abytes, "application/pdf");
        }

        public void ExportToExcelDUHardWinding(string sTemp)
        {
            DUHardWinding oDUHardWinding = new DUHardWinding();
            string _sErrorMesage;
            int nBUID=0;
            try
            {
                _sErrorMesage = "";
                _oDUHardWindings = new List<DUHardWinding>();
                string sSQL = GetSQL(sTemp);
                _oDUHardWindings = DUHardWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[15]);
                
                if (_oDUHardWindings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUHardWindings = new List<DUHardWinding>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 13, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUHardWinding");
                    sheet.Name = "Hard Winding ";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //Recv Date
                    sheet.Column(++nColumn).Width = 15; //Order No
                    sheet.Column(++nColumn).Width = 35; //Buyer
                    sheet.Column(++nColumn).Width = 15; //Lot
                    sheet.Column(++nColumn).Width = 35; //Product
                    sheet.Column(++nColumn).Width = 20; //Product
                    sheet.Column(++nColumn).Width = 15; //OrderQty
                    sheet.Column(++nColumn).Width = 15; //RcvQty
                    sheet.Column(++nColumn).Width = 15; //DlvQy
                    sheet.Column(++nColumn).Width = 15; //NoOfCone
                    sheet.Column(++nColumn).Width = 15; //Status
                    //   nEndCol = 12;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Hard Winding Report"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-4]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = false;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Delivered Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "No Of Cone"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (DUHardWinding oItem in _oDUHardWindings)
                    {
                        nCount++;
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ReceiveDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DyeingOrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_Order; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_RSOut; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.NumOfCone; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StatusST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oDUHardWindings.Select(c => c.Qty_Order).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 4]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oDUHardWindings.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oDUHardWindings.Select(c => c.Qty_RSOut).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DUHardWinding.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        public ActionResult Print_HWStatement(int nID,int nBUID)
        {
            _oDUHardWindings = new List<DUHardWinding>();
            List<DUHardWinding> oDUHardWindingList = new List<DUHardWinding>();

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            List<RouteSheetPacking> _oRouteSheetPackings = new List<RouteSheetPacking>();
            FabricExecutionOrderSpecification oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
            string sErrorMessage="";
            try
            {
                if (nID <= 0)
                {
                    throw new Exception("Nothing To Print.");
                }
                else
                {
                    string sSQL = "SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID IN (SELECT DD.DyeingOrderID FROM DyeingOrderDetail AS DD WHERE DD.DyeingOrderDetailID = " + nID  + ")";
                    oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    
                    _oDUHardWindings = DUHardWinding.Gets("SELECT * FROM View_DUHardWinding WHERE DyeingOrderDetailID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);

                    if (_oDUHardWindings.Count <= 0)
                    {
                        throw new Exception("No Data Found.");
                    }
                    oDyeingOrders = DyeingOrder.Gets("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets("SELECT * FROM View_DyeingOrderFabricDetail WHERE DyeingOrderID IN (" + string.Join(",", _oDUHardWindings.Select(x => x.DyeingOrderID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    if (oDyeingOrderFabricDetails.Count > 0)
                    {
                        sSQL = "Select * from View_FabricExecutionOrderYarnReceive where FSCDID>0 and FSCDID IN (" + string.Join(",", oDyeingOrderFabricDetails.Select(x => x.FSCDetailID)) + ")";
                        oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    }

                    double nQty_Req = 0;

                    if (oFabricExecutionOrderYarnReceives.Count > 0)
                    {
                        oDyeingOrderFabricDetails.ForEach(x =>
                        {
                            nQty_Req = oDyeingOrderFabricDetails.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Select(o => o.QtyDyed).Sum();

                            x.Qty_Assign = _oDUHardWindings.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Select(o => o.Qty).Sum() * x.QtyDyed;
                            if (x.Qty_Assign > 0)
                            {
                                x.Qty_Assign = x.Qty_Assign / nQty_Req;
                            }
                            x.Qty_RS = oFabricExecutionOrderYarnReceives.Where(p => p.FEOSDID == x.FEOSDID).Select(o => o.ReceiveQty).Sum();

                        });
                        _oDUHardWindings.ForEach(x =>
                        {
                            x.Qty_RSOut = oFabricExecutionOrderYarnReceives.Where(p => p.IssueLotID == x.LotID).Select(o => o.ReceiveQty).Sum();

                        });
                    }
                }

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch(Exception e)
            {
                sErrorMessage = e.Message;
            }
            if (!string.IsNullOrEmpty(sErrorMessage)) 
            {
                rptErrorMessage orptErrorMessage = new rptErrorMessage();
                byte[] abytes_error = orptErrorMessage.PrepareReport(sErrorMessage);
                return File(abytes_error, "application/pdf");
            }

            rptHWStatement oReport = new rptHWStatement();
            byte[] abytes = oReport.PrepareReport(_oDUHardWindings, oDyeingOrders, oDyeingOrderDetails, oDyeingOrderFabricDetails, oBusinessUnit, oCompany, oFabricExecutionOrderYarnReceives, _oRouteSheetPackings, oFabricExecutionOrderSpecification);
            return File(abytes, "application/pdf");
        }

        public void ExportToExcel_HWStatement(string sTemp)
        {
            DUHardWinding oDUHardWinding = new DUHardWinding();
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            string _sErrorMesage;
            int nBUID = 0;
            try
            {
                _sErrorMesage = "";
                _oDUHardWindings = new List<DUHardWinding>();
                
                string sSQL = GetSQL(sTemp);
                _oDUHardWindings = DUHardWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oDyeingOrderDetails = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID IN (" + string.Join(",", _oDUHardWindings.Select(x => x.DyeingOrderDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
                oDyeingOrders = DyeingOrder.Gets("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderID)) + ")", (int)Session[SessionInfo.currentUserID]);
                
                nBUID = Convert.ToInt32(sTemp.Split('~')[15]);

                if (_oDUHardWindings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUHardWindings = new List<DUHardWinding>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                #region Print XL

                int nRowIndex = 2, nStartCol = 2, nEndCol = 13, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUHardWinding");
                    sheet.Name = "Hard Winding ";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //Recv Date
                    sheet.Column(++nColumn).Width = 15; //Order No
                    sheet.Column(++nColumn).Width = 35; //Buyer
                    sheet.Column(++nColumn).Width = 15; //Lot
                    sheet.Column(++nColumn).Width = 35; //Product
                    sheet.Column(++nColumn).Width = 20; //Product
                    sheet.Column(++nColumn).Width = 15; //OrderQty
                    sheet.Column(++nColumn).Width = 15; //RcvQty
                    sheet.Column(++nColumn).Width = 15; //DlvQy
                    sheet.Column(++nColumn).Width = 15; //NoOfCone
                    sheet.Column(++nColumn).Width = 15; //Status
                    //   nEndCol = 12;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Dispo Statement"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = false;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Delivered Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "No Of Cone"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (DUHardWinding oItem in _oDUHardWindings)
                    {
                        nCount++;
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ReceiveDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DyeingOrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_Order; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_RSOut; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BagNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StatusST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oDUHardWindings.Select(c => c.Qty_Order).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 4]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oDUHardWindings.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oDUHardWindings.Select(c => c.Qty_RSOut).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DUHardWinding.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetsDOD(DUHardWinding oDUHardWinding)
        {
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            string sSQL = "";

            sSQL = "SELECT * FROM View_DyeingOrderReport";
            string sReturn = "";
            if (!String.IsNullOrEmpty(oDUHardWinding.DyeingOrderNo))
            {
                oDUHardWinding.DyeingOrderNo = oDUHardWinding.DyeingOrderNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderNo Like'%" + oDUHardWinding.DyeingOrderNo + "%'";
            }

            if (oDUHardWinding.DyeingOrderID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID  in(" + oDUHardWinding.DyeingOrderID + ")";
            }

            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "Status!=" + (int)EnumDyeingOrderState.Cancelled;

            sSQL = sSQL + sReturn + " order by DyeingOrderID, ProductID";

            oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}

