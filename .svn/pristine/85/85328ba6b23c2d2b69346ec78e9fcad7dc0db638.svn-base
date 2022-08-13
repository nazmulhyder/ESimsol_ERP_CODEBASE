using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.Collections;


namespace ESimSolFinancial.Controllers
{
    public class FabricDeliveryChallanController : PdfViewController
    {
        #region Declaration
        FabricDeliveryChallan _oFDChallan = new FabricDeliveryChallan();
        FabricDeliveryChallanDetail _oFDCDetail = new FabricDeliveryChallanDetail();

        List<FabricDeliveryChallan> _oFDChallans = new List<FabricDeliveryChallan>();
        List<FabricDeliveryChallanDetail> _oFDCDetails = new List<FabricDeliveryChallanDetail>();
        #endregion

        #region   Fabric Delivery Challan

        public ActionResult ViewFabricDeliveryChallans(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFDChallans = new List<FabricDeliveryChallan>();
            string sSQL = "Select TOP 50  * from View_FabricDeliveryChallan Where Isnull(DisburseBy,0)=0 and Isnull(IsSample,0)=0 ORDER BY FDCID DESC";
            _oFDChallans = FabricDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FDOrderSetup> oFDOSetups = new List<FDOrderSetup>();
            oFDOSetups = FDOrderSetup.Gets("SELECT FDOType, FDOName FROM FDOrderSetup WHERE Activity = 1", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FDOSetups = oFDOSetups;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(_oFDChallans);
        }
        public ActionResult ViewFabricDeliveryChallan(int id,int buid, double ts)
        {
            _oFDChallan = new FabricDeliveryChallan();
            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            if(id>0)
            {
                _oFDChallan = FabricDeliveryChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDChallan.FDCDetails = FabricDeliveryChallanDetail.Gets(id, _oFDChallan.IsSample, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDChallan.FDCDetails_Adj = FabricDeliveryChallanDetail.Gets("Select * from View_FabricDeliveryChallanDetailPPS where isnull(ParentFDCID,0)="+id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFDChallan.FDCDetails.Any() && _oFDChallan.FDCDetails.FirstOrDefault().FDCDID > 0)
                {
                    var oTFDCDs = new List<FabricDeliveryChallanDetail>();
                    _oFDChallan.FDCDetails.ForEach(x => oTFDCDs.Add(x));
                    oCellRowSpans = RowSpanFDCD.RowMerge(oTFDCDs);
                }
            }
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);

            _oFDChallan.FDCDetails.ForEach(o => o.Qty_Meter = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));

            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;

            ViewBag.CellRowSpans = oCellRowSpans;
            ViewBag.VehicleTypes = VehicleType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FDCDetails = _oFDChallan.FDCDetails;
            ViewBag.BUID = buid;
            return View(_oFDChallan);
        }
        public ActionResult ViewFabricDeliveryChallansPPS(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFDChallans = new List<FabricDeliveryChallan>();
            string sSQL = "Select top(100)* from View_FabricDeliveryChallan Where Isnull(DisburseBy,0)=0 and Isnull(IsSample,0)=1 ORDER BY FDCID DESC";
            _oFDChallans = FabricDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FDOrderSetup> oFDOSetups = new List<FDOrderSetup>();
            oFDOSetups = FDOrderSetup.Gets("SELECT FDOType, FDOName FROM FDOrderSetup WHERE Activity = 1", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FDOSetups = oFDOSetups;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(_oFDChallans);
        }
        public ActionResult ViewFabricDeliveryChallanPPS(int id, int buid, double ts)
        {
            _oFDChallan = new FabricDeliveryChallan();
            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            if (id > 0)
            {
                _oFDChallan = FabricDeliveryChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDChallan.FDCDetails = FabricDeliveryChallanDetail.Gets(id, _oFDChallan.IsSample,((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDChallan.FabricDONo = string.Join(",", _oFDChallan.FDCDetails.Select(x => x.ExeNo).Distinct().ToList());
                if (_oFDChallan.FDCDetails.Any() && _oFDChallan.FDCDetails.FirstOrDefault().FDCDID > 0)
                {
                    _oFDChallan.BuyerName = string.Join(",", _oFDChallan.FDCDetails.Select(x => x.BuyerName).Distinct().ToList());
                    oFabricSCReport = oFabricSCReport.Get(_oFDChallan.FDCDetails[0].FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFDChallan.DeliveryToName = oFabricSCReport.ContractorName;
                }
            }
            ViewBag.CellRowSpans = oCellRowSpans;
            ViewBag.VehicleTypes = VehicleType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FDCDetails = _oFDChallan.FDCDetails;
            ViewBag.BUID = buid;
            return View(_oFDChallan);
        }

        [HttpPost]
        public JsonResult Save(FabricDeliveryChallan oFDChallan)
        {
            try
            {

                oFDChallan.IssueDate = oFDChallan.IssueDate.AddHours(DateTime.Now.Hour);
                oFDChallan.IssueDate = oFDChallan.IssueDate.AddMinutes(DateTime.Now.Minute);
                oFDChallan.IssueDate = oFDChallan.IssueDate.AddSeconds(DateTime.Now.Second);
                if (oFDChallan.FDCID <= 0)
                {
                    oFDChallan = oFDChallan.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFDChallan = oFDChallan.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oFDChallan = new FabricDeliveryChallan();
                oFDChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update_Adj(FabricDeliveryChallanDetail oFDChallanDetail)
        {
            try
            {
                if (oFDChallanDetail.FDCDID > 0)
                {
                    oFDChallanDetail = oFDChallanDetail.Update_Adj(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else throw new Exception("Invalid Fabric Delivery Challan!");
            }
            catch (Exception ex)
            {
                oFDChallanDetail = new FabricDeliveryChallanDetail();
                oFDChallanDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDChallanDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricDeliveryChallan oFDChallan)
        {

            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFDChallan.Delete( ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ApproveFDC(FabricDeliveryChallan oFDChallan)
        {
            try
            {
                if (oFDChallan.FDCID <= 0) { throw new Exception("Please select an valid item."); }
                oFDChallan.ApproveDate = DateTime.Now;
                oFDChallan = oFDChallan.Approve( ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFDChallan = new FabricDeliveryChallan();
                oFDChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApproveFDC(FabricDeliveryChallan oFDChallan)
        {
            try
            {
                if (oFDChallan.FDCID <= 0) { throw new Exception("Please select an valid item."); }
                oFDChallan.ApproveDate = DateTime.Now;
                oFDChallan = oFDChallan.UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFDChallan = new FabricDeliveryChallan();
                oFDChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FDCDisburse(FabricDeliveryChallan oFDChallan)
        {
            try
            {
                if (oFDChallan.FDCID <= 0) { throw new Exception("Please select an valid item."); }
                oFDChallan = FabricDeliveryChallan.FDCDisburse(oFDChallan, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFDChallan = new FabricDeliveryChallan();
                oFDChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FDCDisburseFromFinishing(FabricDeliveryChallan oFDChallan)
        {
            try
            {
                if (oFDChallan.FDCID <= 0) { throw new Exception("Please select an valid item."); }
                oFDChallan = FabricDeliveryChallan.FDCDisburse(oFDChallan, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFDChallan = new FabricDeliveryChallan();
                oFDChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult GetFDC(FabricDeliveryChallan oFDChallan)
        {
            try
            {
                if (oFDChallan.FDCID <= 0) { throw new Exception("Please select an valid item."); }
                oFDChallan = FabricDeliveryChallan.Get(oFDChallan.FDCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDChallan.FDCID > 0)
                {
                    oFDChallan.FDCDetails = FabricDeliveryChallanDetail.Gets(oFDChallan.FDCID, oFDChallan.IsSample,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFDChallan = new FabricDeliveryChallan();
                oFDChallan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetsFDC(FabricDeliveryChallan oFDChallan)
        {
            _oFDChallans = new List<FabricDeliveryChallan>();
            try
            {
                int nIsInHouse = Convert.ToInt32(oFDChallan.Params.Split('~')[0]);
                string sChallanNo = oFDChallan.Params.Split('~')[1].Trim();
                string sDONo = oFDChallan.Params.Split('~')[2].Trim();
                string sExeNo = oFDChallan.Params.Split('~')[3].Trim();
                int nFDCStatus = Convert.ToInt32(oFDChallan.Params.Split('~')[4]);
                DateTime IssueDateFrom = Convert.ToDateTime(oFDChallan.Params.Split('~')[5]);
                DateTime IssueDateTo = Convert.ToDateTime(oFDChallan.Params.Split('~')[6]);
                bool IsDateSearch = Convert.ToBoolean(oFDChallan.Params.Split('~')[7]);
                string sSQL = "Select * from View_FabricDeliveryChallan Where  FDCID <> 0  ";
                if (sChallanNo != "") { sSQL = sSQL + " and ChallanNo Like '%" + sChallanNo + "%'"; }
                if (sDONo != "") { sSQL = sSQL + " and DONo Like '%" + sDONo + "%'"; }
                if (sExeNo != "") { sSQL = sSQL + " and FDOID In (Select FDOID from View_FabricDeliveryOrderDetail Where  IsInHouse = " + nIsInHouse + "  AND FEONo Like '%" + sExeNo + "%')"; }
                if (IsDateSearch) { sSQL = sSQL + " and IssueDate Between '" + IssueDateFrom.ToString("dd-MMM-yyyy") + "' And '" + IssueDateTo.ToString("dd-MMM-yyyy") + "'"; }

                _oFDChallans = FabricDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFDChallans = new List<FabricDeliveryChallan>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetsFDO(FabricDeliveryOrder oFabricDeliveryOrder)
        
        
        
        {
            string _sSQL = "";
            List<FabricDeliveryOrder> oFabricDeliveryOrders = new List<FabricDeliveryOrder>();
            try
            {
                string sReturn = "";
                _sSQL = "";
                _sSQL = "SELECT top(200)* FROM View_FabricDeliveryOrder";

                //Global.TagSQL(ref sReturn);
                //sReturn = sReturn + "Isnull(ApproveBy,0)<>0 and isnull(Qty,0)>isnull(Qty_DO,0)";

                #region PO
                if (oFabricDeliveryOrder.ContractorID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in (" + oFabricDeliveryOrder.IssueToID + "," + oFabricDeliveryOrder.BuyerID + ")";
                }
                #endregion
                #region PO
                if (oFabricDeliveryOrder.BuyerID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in (" + oFabricDeliveryOrder.IssueToID + "," + oFabricDeliveryOrder.BuyerID + ")";
                }
                #endregion
                #region PO
                if (!String.IsNullOrEmpty(oFabricDeliveryOrder.DONo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DONO LIKE '%" + oFabricDeliveryOrder.DONo + "%'";
                }
                #endregion

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(approveBy,0)<>0 and Qty+0.5> isnull(Qty_DC,0) order by DODate DESC";

                if (sReturn != "")
                { _sSQL = _sSQL + sReturn; }

                oFabricDeliveryOrders = FabricDeliveryOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFabricDeliveryOrders = new List<FabricDeliveryOrder>();
        
                //oFabricDeliveryOrderDetails.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetsFDOD(FabricDeliveryChallan oFabricDeliveryChallan)
        {
            string _sSQL = "";
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            try
            {

                oFabricDeliveryOrderDetails = FabricDeliveryOrderDetail.Gets(oFabricDeliveryChallan.FDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                string sFSCDIDs = string.Join(",", oFabricDeliveryOrderDetails.Select(x => x.FEOID).ToList());

                if (!string.IsNullOrEmpty(sFSCDIDs))
                {
                    oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets("SELECT * FROM FabricDeliveryChallanDetail where isnull(FDODID,0)<=0 and isnull(ParentFDCID,0)>0 and FSCDID in (" + sFSCDIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oFabricDeliveryChallanDetails.Count > 0)
                {
                    oFabricDeliveryOrderDetails.ForEach(x =>
                    {
                        if (oFabricDeliveryChallanDetails.FirstOrDefault() != null && oFabricDeliveryChallanDetails.FirstOrDefault().FSCDID > 0 && oFabricDeliveryChallanDetails.Where(b => (b.FSCDID == x.FEOID)).Count() > 0)
                        {
                            x.Qty_DC = x.Qty_DC +oFabricDeliveryChallanDetails.Where(p => (p.FSCDID == x.FEOID) && p.FSCDID > 0).Sum(C => C.Qty);
                        }
                    });
                }
                

                //FabricDeliveryChallanDetail oFabricDCDetail = new FabricDeliveryChallanDetail();
                //foreach (FabricDeliveryOrderDetail oItem in oFabricDeliveryOrderDetails)
                //{
                //    oFabricDCDetail = new FabricDeliveryChallanDetail();
                //    oFabricDCDetail.FabricID = oItem.FabricID;
                //    oFabricDCDetail.FabricNo = oItem.FabricNo;

                //    oFabricDCDetail.PINo = oItem.PINo;
                //    oFabricDCDetail.LCNo = oItem.LCNo;
                //    oFabricDCDetail.FEONo = oItem.FEONo;
                //    //oFabricDCDetail.FEOID = oItem.FEOID;
                //    oFabricDCDetail.Qty = oItem.Qty - oItem.Qty_DC;
                //    //oFabricDCDetail.FEOID = oItem.FEOID;
                //    //oFabricDCDetail.ContractorID = oItem.ContractorID;
                //    //oFabricDCDetail.BuyerID = oItem.BuyerID;
                //    //oFabricDCDetail.ContractorName = oItem.ContractorName;
                //    //oFabricDCDetail.BuyerName = oItem.BuyerName;
                //    oFabricDCDetail.Construction = oItem.Construction;
                //    //oFabricDCDetail.OrderType = (EnumFabricRequestType)oItem.OrderType;
                //    oFabricDCDetail.MUID = oItem.MUID;
                //    oFabricDCDetail.MUName = oItem.MUName;
                //    oFabricDCDetail.Construction = oItem.Construction;
                //    oFabricDCDetail.ColorInfo = oItem.ColorInfo;
                //    oFabricDCDetail.StyleNo = oItem.StyleNo;
                //    //oFabricDCDetail.BuyerRef = oItem.BuyerRef;
                //    oFabricDCDetail.HLReference = oItem.HLReference;
                //    if (oFabricDCDetail.Qty > 0)
                //    {
                //        oFabricDeliveryChallanDetails.Add(oFabricDCDetail);
                //    }
                //}


            }
            catch (Exception ex)
            {
                oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
                //oFabricDeliveryOrderDetails.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDispo(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            string _sSQL = "";
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            try
            {
                string sReturn = "";
                _sSQL = "";
                _sSQL = "SELECT top(100)* FROM View_FabricSalesContractReport";

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "isnull(ExeNo,'')!=''";
                #region PO
                if (oFabricDeliveryOrder.ContractorID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in (" + oFabricDeliveryOrder.IssueToID + "," + oFabricDeliveryOrder.BuyerID + ")";
                }
                #endregion
                #region PO
                if (oFabricDeliveryOrder.BuyerID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in (" + oFabricDeliveryOrder.IssueToID + "," + oFabricDeliveryOrder.BuyerID + ")";
                }
                #endregion
                #region PO
                if (!String.IsNullOrEmpty(oFabricDeliveryOrder.DONo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExeNo LIKE '%" + oFabricDeliveryOrder.DONo + "%'";
                }
                #endregion

                if (sReturn != "")
                { _sSQL = _sSQL + sReturn + " and FabricID>0 order by SCDate DESC"; }

                oFabricSCReports = FabricSCReport.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFabricSCReports = new List<FabricSCReport>();

                //oFabricDeliveryOrderDetails.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsForAdj(FabricDeliveryChallanDetail oFabricDeliveryChallanDetail)
        {
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            try
            {
                oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.GetsForAdj(oFabricDeliveryChallanDetail.ContractorID, oFabricDeliveryChallanDetail.ParentFDCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
                oFabricDeliveryChallanDetails.Add(new FabricDeliveryChallanDetail() { ErrorMessage = ex.Message});
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsAdjSampleForChallan(FabricDeliveryChallanDetail oFabricDeliveryChallanDetail)
        {
            string sSQl = "Select * from View_FabricDeliveryChallanDetailPPS where isnull(FSCDID,0)>0 and isnull(ParentFDCID,0)=0 and isnull(IsSample,0)=1";

            if (!string.IsNullOrEmpty(oFabricDeliveryChallanDetail.ExeNo)) { sSQl = sSQl + " And ExeNo LIKE '%" + oFabricDeliveryChallanDetail.ExeNo + "%'"; }

            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            
            try
            {
                oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets(sSQl, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
                oFabricDeliveryChallanDetails.Add(new FabricDeliveryChallanDetail() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #region 
        [HttpPost]
        public JsonResult GetsLots(FabricDeliveryChallanDetail oFDCD)
        {
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();

            FabricDeliveryOrderDetail oFDODTwo = new FabricDeliveryOrderDetail();
            
            List<Lot> oLots = new List<Lot>();
            int nWorkingUnitID = Convert.ToInt32(oFDCD.ErrorMessage.Split('~')[0]);
            bool bIsQCLot = Convert.ToBoolean(oFDCD.ErrorMessage.Split('~')[1]);
            //int nFabricID = Convert.ToInt32(oFDOD.ErrorMessage.Split('~')[2]);
            string sLotID = Convert.ToString(oFDCD.ErrorMessage.Split('~')[2]);
            oFDODTwo = FabricDeliveryOrderDetail.Get(oFDCD.FDODID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT top(2000)* FROM View_Lot WHERE WorkingUnitID = " + nWorkingUnitID + " And Balance>0 AND ParentType=" + (int)EnumTriggerParentsType._FNProduction;//FBQCDetail
            if (!string.IsNullOrEmpty(oFDCD.LotNo))
            {
                //sSQL += " And ParentID IN (SELECT FBQCDetailID FROM  FabricBatchQCDetail WHERE FBQCID IN (SELECT VFBQC.FBQCID FROM View_FabricBatchQC AS VFBQC WHERE VFBQC.FEOID = " + oFDODTwo.FEOID + " ))";
                sSQL += "And  LotNo Like '%" + oFDCD.LotNo+"%'";
            }
            if (!bIsQCLot)
            {
                //sSQL += " And ParentID IN (SELECT FBQCDetailID FROM  FabricBatchQCDetail WHERE FBQCID IN (SELECT VFBQC.FBQCID FROM View_FabricBatchQC AS VFBQC WHERE VFBQC.FEOID = " + oFDODTwo.FEOID + " ))";
                sSQL += "And  ParentID IN (SELECT FNBatchQCDetail.FNBatchQCDetailID FROM  FNBatchQCDetail WHERE FNBatchQCDetail.FNBatchQCID IN (SELECT VFBQC.FNBatchQCID FROM View_FNBatchQC AS VFBQC WHERE VFBQC.FNExOID=" + oFDODTwo.FEOID + "))";
            }

            if (!string.IsNullOrEmpty(sLotID))
                sSQL += " And LotID Not In (" + sLotID + ")";

            try
            {
                oLots = Lot.Gets(sSQL + " order by ParentID, LotNo ASC", ((User)Session[SessionInfo.CurrentUser]).UserID); //CONVERT(int,dbo.udf_GetNumeric(LotNo))
                if (oLots.Any() && oLots.FirstOrDefault().LotID > 0)
                {
                    sSQL = "Select * from View_FabricDeliveryChallanDetail Where Qty>0 And LotID In (" + string.Join(",", oLots.Select(x => x.LotID).ToArray()) + ")";
                    List<FabricDeliveryChallanDetail> oFDCDs = new List<FabricDeliveryChallanDetail>();
                    oFDCDs = FabricDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oFDCDs.Any() && oFDCDs.FirstOrDefault().FDCDID > 0)
                    {
                       // oLots.ForEach(x => { x.Balance = x.Balance - oFDCDs.Where(p => p.LotID == x.LotID).Sum(o => o.Qty); oFDCDs.RemoveAll(p => p.LotID == x.LotID); });
                        oLots.RemoveAll(x => x.Balance <= 0);
                    }
                    FabricDeliveryChallanDetail oFabricDCDetail = new FabricDeliveryChallanDetail();
                      foreach (Lot oItem in oLots)
                      {
                          oFabricDCDetail = new FabricDeliveryChallanDetail();
                          oFabricDCDetail.FabricNo = oFDODTwo.FabricNo;
                          oFabricDCDetail.FDODID = oFDODTwo.FDODID;
                          oFabricDCDetail.FEONo = oFDODTwo.FEONo;
                          oFabricDCDetail.ExeNo = oFDODTwo.ExeNo;
                          oFabricDCDetail.Construction = oFDODTwo.Construction;
                          oFabricDCDetail.ProductID = oFDODTwo.ProductID;
                          oFabricDCDetail.ProductName = oFDODTwo.ProductName;
                          oFabricDCDetail.BuyerName = oFDODTwo.BuyerName;
                          oFabricDCDetail.LotNo = oItem.LotNo;
                          oFabricDCDetail.LotID = oItem.LotID;
                          //oFabricDCDetail.FDODID = oItem.FDODID;
                          oFabricDCDetail.MUName = oFDODTwo.MUName;
                          oFabricDCDetail.ColorInfo = oFDODTwo.ColorInfo;
                          oFabricDCDetail.BuyerRef = oFDODTwo.BuyerRef;
                          oFabricDCDetail.StyleNo = oFDODTwo.StyleNo;
                          oFabricDCDetail.Qty = oItem.Balance;
                          oFabricDCDetail.Qty_Meter = Global.GetMeter(oFabricDCDetail.Qty, 5);
                          oFabricDCDetail.WorkingUnitID = oItem.WorkingUnitID;
                          oFabricDeliveryChallanDetails.Add(oFabricDCDetail);
                      }

                }
                else
                {
                    throw new Exception("No Lot found.");
                }
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricDeliveryChallanDetails);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(oFabricDeliveryChallanDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsLotsQCDetail(FabricDeliveryChallanDetail oFDCD)
        {
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();

            FabricDeliveryOrderDetail oFDODTwo = new FabricDeliveryOrderDetail();

            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            int nWorkingUnitID = Convert.ToInt32(oFDCD.ErrorMessage.Split('~')[0]);
            bool bIsQCLot = Convert.ToBoolean(oFDCD.ErrorMessage.Split('~')[1]);
            //int nFabricID = Convert.ToInt32(oFDOD.ErrorMessage.Split('~')[2]);
            string sFNBatchQCDetailID = Convert.ToString(oFDCD.ErrorMessage.Split('~')[2]);
            oFDODTwo = FabricDeliveryOrderDetail.Get(oFDCD.FDODID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "Select FQCD.FNBatchQCDetailID, FQCD.LotID,LotNo,FQCD.Qty,ESCDetail.ExeNo,Fabric.FabricNo,ESCDetail.Construction,(Select SUm(Qty) from FabricDeliveryChallanDetail as FDCD where FDCD.FNBatchQCDetailID=FQCD.FNBatchQCDetailID) as QtyDC,(Select SUm(Qty) from FabricReturnChallanDetail as FDCD where FDCD.FNBatchQCDetailID=FQCD.FNBatchQCDetailID) as QtyRC from FNBatchQCDetail   as FQCD  LEFT JOIN	FabricSalesContractDetail as ESCDetail ON FQCD.FSCDID = ESCDetail.FabricSalesContractDetailID LEFT  JOIN	Fabric ON ESCDetail.FabricID = Fabric.FabricID where FQCD.FNBatchQCDetailID>0";

            if (!string.IsNullOrEmpty(sFNBatchQCDetailID))
                sSQL += " And FQCD.FNBatchQCDetailID Not In (" + sFNBatchQCDetailID + ")";

            //if (!string.IsNullOrEmpty(oFDCD.LotNo))
            //{
            //    //sSQL += " And ParentID IN (SELECT FBQCDetailID FROM  FabricBatchQCDetail WHERE FBQCID IN (SELECT VFBQC.FBQCID FROM View_FabricBatchQC AS VFBQC WHERE VFBQC.FEOID = " + oFDODTwo.FEOID + " ))";
            //    sSQL += "And  FQCD.LotNo Like '%" + oFDCD.LotNo + "%'";
            //}
            if (!bIsQCLot)
            {
                if (!string.IsNullOrEmpty(oFDCD.LotNo))
                {
                    //sSQL += " And ParentID IN (SELECT FBQCDetailID FROM  FabricBatchQCDetail WHERE FBQCID IN (SELECT VFBQC.FBQCID FROM View_FabricBatchQC AS VFBQC WHERE VFBQC.FEOID = " + oFDODTwo.FEOID + " ))";
                    sSQL += "And  FQCD.LotNo Like '%" + oFDCD.LotNo + "%'";
                }

                sSQL += "and FQCD.FSCDID=" + oFDODTwo.FEOID + "";
            }
            else
            {
                if (!string.IsNullOrEmpty(oFDCD.LotNo))
                {
                    sSQL += "and FQCD.FSCDID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where Exeno like '%" + oFDCD.LotNo + "%' and isnull(Exeno,0)!='' )";
                }
            }
            if (nWorkingUnitID>0)
            {
                //sSQL += " And ParentID IN (SELECT FBQCDetailID FROM  FabricBatchQCDetail WHERE FBQCID IN (SELECT VFBQC.FBQCID FROM View_FabricBatchQC AS VFBQC WHERE VFBQC.FEOID = " + oFDODTwo.FEOID + " ))";
                sSQL += "and LotID in (SELECT LotID FROM Lot WHERE Balance>0 and WorkingUnitID = " + nWorkingUnitID + ")";
            }

            

            try
            {
                //oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL + " order by ParentID, LotNo ASC", ((User)Session[SessionInfo.CurrentUser]).UserID); //CONVERT(int,dbo.udf_GetNumeric(LotNo))
                oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL + " order by FQCD.FNBatchQCDetailID, LotNo ASC", ((User)Session[SessionInfo.CurrentUser]).UserID); //CONVERT(int,dbo.udf_GetNumeric(LotNo))
                if (oFNBatchQCDetails.Any() && oFNBatchQCDetails.FirstOrDefault().LotID > 0)
                {
                    //sSQL = "Select * from View_FabricDeliveryChallanDetail Where Qty>0 And LotID In (" + string.Join(",", oLots.Select(x => x.LotID).ToArray()) + ")";
                    //List<FabricDeliveryChallanDetail> oFDCDs = new List<FabricDeliveryChallanDetail>();
                    //oFDCDs = FabricDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //if (oFDCDs.Any() && oFDCDs.FirstOrDefault().FDCDID > 0)
                    //{
                    //    // oLots.ForEach(x => { x.Balance = x.Balance - oFDCDs.Where(p => p.LotID == x.LotID).Sum(o => o.Qty); oFDCDs.RemoveAll(p => p.LotID == x.LotID); });
                    //    oLots.RemoveAll(x => x.Balance <= 0);
                    //}
                    FabricDeliveryChallanDetail oFabricDCDetail = new FabricDeliveryChallanDetail();
                    foreach (FNBatchQCDetail oFNBatchQCDetail in oFNBatchQCDetails)
                    {
                        oFabricDCDetail = new FabricDeliveryChallanDetail();
                        oFabricDCDetail.FabricNo = oFDODTwo.FabricNo;
                        oFabricDCDetail.FDODID = oFDODTwo.FDODID;
                        oFabricDCDetail.FEONo = oFDODTwo.FEONo;
                        oFabricDCDetail.ExeNo = oFDODTwo.ExeNo;
                        oFabricDCDetail.Construction = oFDODTwo.Construction;
                        oFabricDCDetail.ProductID = oFDODTwo.ProductID;
                        oFabricDCDetail.ProductName = oFDODTwo.ProductName;
                        oFabricDCDetail.BuyerName = oFDODTwo.BuyerName;
                        oFabricDCDetail.LotNo = oFNBatchQCDetail.LotNo;
                        oFabricDCDetail.LotID = oFNBatchQCDetail.LotID;
                        oFabricDCDetail.FNBatchQCDetailID = oFNBatchQCDetail.FNBatchQCDetailID;
                        //oFabricDCDetail.FDODID = oItem.FDODID;
                        oFabricDCDetail.MUName = oFDODTwo.MUName;
                        oFabricDCDetail.ColorInfo = oFDODTwo.ColorInfo;
                        oFabricDCDetail.BuyerRef = oFDODTwo.BuyerRef;
                        oFabricDCDetail.StyleNo = oFDODTwo.StyleNo;
                        oFabricDCDetail.Qty = oFNBatchQCDetail.Qty + oFNBatchQCDetail.QtyRC- oFNBatchQCDetail.QtyDC;
                        oFabricDCDetail.Qty_Meter = Global.GetMeter(oFabricDCDetail.Qty, 5);
                        oFabricDCDetail.WorkingUnitID = oFNBatchQCDetail.WorkingUnitID;
                        if (oFabricDCDetail.Qty > 0)
                        {
                            oFabricDeliveryChallanDetails.Add(oFabricDCDetail);
                        }
                    }

                }
                else
                {
                    throw new Exception("No Lot found.");
                }
            }
            catch (Exception ex)
            {
                //oLots = new List<Lot>();
                oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricDeliveryChallanDetails);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(oFabricDeliveryChallanDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsLotsByDispo(FabricDeliveryChallanDetail oFDCD)
        {
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            FabricSCReport oFabricSCReport = new FabricSCReport();

            List<Lot> oLots = new List<Lot>();
            int nWorkingUnitID = Convert.ToInt32(oFDCD.ErrorMessage.Split('~')[0]);
            bool bIsQCLot = Convert.ToBoolean(oFDCD.ErrorMessage.Split('~')[1]);
            //int nFabricID = Convert.ToInt32(oFDOD.ErrorMessage.Split('~')[2]);
            string sLotID = Convert.ToString(oFDCD.ErrorMessage.Split('~')[2]);
            oFabricSCReport = oFabricSCReport.Get(oFDCD.FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT top(200)* FROM View_Lot WHERE WorkingUnitID = " + nWorkingUnitID + " And Balance>0 AND ParentType=" + (int)EnumTriggerParentsType._FNProduction;//FBQCDetail
            if (!string.IsNullOrEmpty(oFDCD.LotNo))
            {
                //sSQL += " And ParentID IN (SELECT FBQCDetailID FROM  FabricBatchQCDetail WHERE FBQCID IN (SELECT VFBQC.FBQCID FROM View_FabricBatchQC AS VFBQC WHERE VFBQC.FEOID = " + oFDODTwo.FEOID + " ))";
                sSQL += "And  LotNo Like '%" + oFDCD.LotNo + "%'";
            }
            sSQL += "And  ParentID IN (SELECT FNBatchQCDetail.FNBatchQCDetailID FROM  FNBatchQCDetail WHERE FNBatchQCDetail.FNBatchQCID IN (SELECT VFBQC.FNBatchQCID FROM View_FNBatchQC AS VFBQC WHERE VFBQC.FNExOID=" + oFDCD.FSCDID + "))";
         
            //if (oFDODTwo.FabricID>0)
            //{
            //    sSQL += " And ProductID = (SELECT ProductID FROM  Fabric WHERE FabricID = " + oFDODTwo.FabricID + ")";
            //}

            if (!string.IsNullOrEmpty(sLotID))
                sSQL += " And LotID Not In (" + sLotID + ")";

            try
            {
               // oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oLots.Any() && oLots.FirstOrDefault().LotID > 0)
                //{
                    sSQL = "Select * from View_FabricDeliveryChallanDetail Where Qty>0 And LotID In (" + string.Join(",", oLots.Select(x => x.LotID).ToArray()) + ")";
                    List<FabricDeliveryChallanDetail> oFDCDs = new List<FabricDeliveryChallanDetail>();
                    //oFDCDs = FabricDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oFDCDs.Any() && oFDCDs.FirstOrDefault().FDCDID > 0)
                    {
                        oLots.ForEach(x => { x.Balance = x.Balance - oFDCDs.Where(p => p.LotID == x.LotID).Sum(o => o.Qty); oFDCDs.RemoveAll(p => p.LotID == x.LotID); });
                        oLots.RemoveAll(x => x.Balance <= 0);
                    }
                    FabricDeliveryChallanDetail oFabricDCDetail = new FabricDeliveryChallanDetail();
                    foreach (Lot oItem in oLots)
                    {
                        oFabricDCDetail = new FabricDeliveryChallanDetail();
                        oFabricDCDetail.FabricNo = oFabricSCReport.FabricNo;
                        oFabricDCDetail.FSCDID = oFabricSCReport.FabricSalesContractDetailID;
                        oFabricDCDetail.FEONo = oFabricSCReport.SCNoFull;
                        //oFabricDCDetail.FEONo = oFabricSCReport.SCNoFull;
                        oFabricDCDetail.ExeNo = oFabricSCReport.ExeNo;
                        oFabricDCDetail.Construction = oFabricSCReport.Construction;
                        oFabricDCDetail.ProductID = oFabricSCReport.ProductID;
                        oFabricDCDetail.ProductName = oFabricSCReport.ProductName;
                        oFabricDCDetail.BuyerName = oFabricSCReport.BuyerName;
                        oFabricDCDetail.LotNo = oItem.LotNo;
                        oFabricDCDetail.LotID = oItem.LotID;
                        //oFabricDCDetail.FDODID = oItem.FDODID;
                        oFabricDCDetail.MUName = oFabricSCReport.MUName;
                        oFabricDCDetail.ColorInfo = oFabricSCReport.ColorInfo;
                        oFabricDCDetail.BuyerRef = oFabricSCReport.BuyerReference;
                        oFabricDCDetail.StyleNo = oFabricSCReport.StyleNo;
                        oFabricDCDetail.Qty = oItem.Balance;
                        oFabricDCDetail.WorkingUnitID = oItem.WorkingUnitID;
                        oFabricDeliveryChallanDetails.Add(oFabricDCDetail);
                    }
                   
                        oFabricDCDetail = new FabricDeliveryChallanDetail();
                        oFabricDCDetail.FabricNo = oFabricSCReport.FabricNo;
                        oFabricDCDetail.FSCDID = oFabricSCReport.FabricSalesContractDetailID;
                        oFabricDCDetail.FEONo = oFabricSCReport.SCNoFull;
                        //oFabricDCDetail.FEONo = oFabricSCReport.SCNoFull;
                        oFabricDCDetail.ExeNo = oFabricSCReport.ExeNo;
                        oFabricDCDetail.Construction = oFabricSCReport.Construction;
                        oFabricDCDetail.ProductID = oFabricSCReport.ProductID;
                        oFabricDCDetail.ProductName = oFabricSCReport.ProductName;
                        oFabricDCDetail.BuyerName = oFabricSCReport.BuyerName;
                        oFabricDCDetail.LotNo = oFabricSCReport.ExeNo;
                        oFabricDCDetail.LotID = 0;
                        //oFabricDCDetail.FDODID = oItem.FDODID;
                        oFabricDCDetail.MUName = oFabricSCReport.MUName;
                        oFabricDCDetail.ColorInfo = oFabricSCReport.ColorInfo;
                        oFabricDCDetail.BuyerRef = oFabricSCReport.BuyerReference;
                        oFabricDCDetail.StyleNo = oFabricSCReport.StyleNo;
                        oFabricDCDetail.Qty = 20;
                        oFabricDCDetail.WorkingUnitID = oFDCD.WorkingUnitID;
                        oFabricDeliveryChallanDetails.Add(oFabricDCDetail);
                   

                //}
                //else
                //{
                //    throw new Exception("No Lot found.");
                //}
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fabric Delivery Challan Detail

        [HttpPost]
        public JsonResult SearchByNo(FabricDeliveryChallan oFabricDeliveryChallan)
        {
            _oFDChallans = new List<FabricDeliveryChallan>();

            string sReturn = "";
            string _sSQL = "";
            _sSQL = "SELECT * FROM [View_FabricDeliveryChallan]";

            #region EXE No
            if (!String.IsNullOrEmpty(oFabricDeliveryChallan.Params))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDCID IN (SELECT FDD.FDCID FROM FabricDeliveryChallanDetail AS FDD WHERE FDD.FSCDID IN(SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE ExeNo LIKE '%" + oFabricDeliveryChallan.Params + "%' ))";
            }
            #endregion

            #region Challan No
            if (!String.IsNullOrEmpty(oFabricDeliveryChallan.ChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ChallanNo LIKE '%" + oFabricDeliveryChallan.ChallanNo + "%'";
            }
            #endregion

            #region Is Sample
            if (oFabricDeliveryChallan.IsSample)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(IsSample,0) = 1 ";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(IsSample,0) = 0 ";
            }
            #endregion

            _sSQL = _sSQL + "" + sReturn;
            try
            {
                _oFDChallans = FabricDeliveryChallan.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFDChallans = new List<FabricDeliveryChallan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFDCDetail(FabricDeliveryChallanDetail oFDCDetail)
        {
            try
            {
                if (oFDCDetail.FDCDID <= 0)
                {
                    oFDCDetail = oFDCDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFDCDetail = oFDCDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFDCDetail = new FabricDeliveryChallanDetail();
                oFDCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(FabricDeliveryChallanDetail oFDCDetail)
        {
            try
            {
                if (oFDCDetail.FDCDID <= 0) { throw new Exception("Please select an valid item."); }

                oFDCDetail = oFDCDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFDCDetail = new FabricDeliveryChallanDetail();
                oFDCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDCDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

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
        public ActionResult PrintFDC(int id, int nBUID, bool bIsMeter, int nFormat)
        {
            bool bIsGatePass = false;
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            oFDC = FabricDeliveryChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj = new List<FabricDeliveryChallanDetail>();
            FabricSCReport oFabricSCReport = new FabricSCReport();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(nBUID, (int)Session[SessionInfo.currentUserID]);

            if (oFDC.FDCID > 0)
            {
                oFDC.FDCDetails = FabricDeliveryChallanDetail.Gets(oFDC.FDCID, oFDC.IsSample,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oFDC.FDCDetails.Count > 0)
            {
                oFDC.BuyerName = oFDC.FDCDetails.Where(p => p.BuyerName != null && p.BuyerName != "").Select(x => x.BuyerName).FirstOrDefault();
                oFDC.BuyerCPName = oFDC.FDCDetails.Where(p => p.BuyerCPName != null && p.BuyerCPName != "").Select(x => x.BuyerCPName).FirstOrDefault();
                oFDC.MKTPerson = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();
              //  oFDC.Fab = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();
             
                if (oFDC.IsSample)
                {
                    oFDC.FabricDONo = string.Join(",", oFDC.FDCDetails.Select(x => x.FEONo).Distinct().ToList());
                    oFabricSCReport = oFabricSCReport.Get(oFDC.FDCDetails[0].FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFDC.DeliveryToName = oFabricSCReport.ContractorName;
                    oFDC.ContractorID = oFabricSCReport.ContractorID;
                    oFDC.BuyerCPName = oFabricSCReport.BuyerCPName;
                    oFDC.MKTPerson = oFabricSCReport.MKTPName;
                }
                oFabricDeliveryChallanDetailsAdj = FabricDeliveryChallanDetail.GetsForAdj(0, oFDC.FDCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (bIsMeter)
            {
                // _oFabricDeliveryOrder.FDODetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));

                oFDC.FDCDetails.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                oFDC.FDCDetails.ForEach(o => o.MUName = "Meter");
                oFabricDeliveryChallanDetailsAdj.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
            }
            else
            {
                oFDC.FDCDetails.ForEach(o => o.MUName = "Yard");
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oFDC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFDC.DeliveryToAddress = oContractor.Address;

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricDeliveryChallan + " AND BUID = " + nBUID + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup = oDeliverySetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            oFDOrderSetup = oFDOrderSetup.GetByType((int)oFDC.FDOType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            bool isPadFormat = (oFDOrderSetup.PrintFormat == 1);
            if (nFormat > -1)
                isPadFormat = (nFormat == 1);

            if(oDeliverySetup.DeliverySetupID > 0 && oDeliverySetup.ChallanPrintNo == EnumExcellColumn.B)
            {
                rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
                byte[] abytes = oReport.PrepareReport_B(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads);
                return File(abytes, "application/pdf");
            }
            else 
            {
                oDeliverySetup.DCPrefix = "DCF";
                oDeliverySetup.GPPrefix = "GPF";
                if (string.IsNullOrEmpty(oFDC.GatePassNo)) { oFDC.GatePassNo = oFDC.ChallanNo; }
                oFDC.ChallanNo = oDeliverySetup.DCPrefix+"-" + oFDC.ChallanNo;
                oFDC.GatePassNo = oDeliverySetup.GPPrefix + "-" + oFDC.GatePassNo;
                rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
                byte[] abytes = oReport.PrepareReport(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintPreviewBill(int id, int nBUID, bool bIsMeter, int nFormat)
        {
            bool bIsGatePass = false;
            double nQty = 0;
            double nAmount = 0;
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            oFDC = FabricDeliveryChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj = new List<FabricDeliveryChallanDetail>();
            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            FabricSCReport oFabricSCReport = new FabricSCReport();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(nBUID, (int)Session[SessionInfo.currentUserID]);

            if (oFDC.FDCID > 0)
            {
                oFDC.FDCDetails = FabricDeliveryChallanDetail.Gets(oFDC.FDCID, oFDC.IsSample, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oFDC.FDCDetails.Count > 0)
            {
                oFDC.BuyerName = oFDC.FDCDetails.Where(p => p.BuyerName != null && p.BuyerName != "").Select(x => x.BuyerName).FirstOrDefault();
                oFDC.BuyerCPName = oFDC.FDCDetails.Where(p => p.BuyerCPName != null && p.BuyerCPName != "").Select(x => x.BuyerCPName).FirstOrDefault();
                oFDC.MKTPerson = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();
                //  oFDC.Fab = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();

                if (oFDC.IsSample)
                {
                    oFDC.FabricDONo = string.Join(",", oFDC.FDCDetails.Select(x => x.FEONo).Distinct().ToList());
                    oFabricSCReport = oFabricSCReport.Get(oFDC.FDCDetails[0].FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFDC.DeliveryToName = oFabricSCReport.ContractorName;
                    oFDC.ContractorID = oFabricSCReport.ContractorID;
                    oFDC.BuyerCPName = oFabricSCReport.BuyerCPName;
                    oFDC.MKTPerson = oFabricSCReport.MKTPName;
                }
                oFabricDeliveryChallanDetailsAdj = FabricDeliveryChallanDetail.GetsForAdj(0, oFDC.FDCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                string sSQL = string.Join(",", oFDC.FDCDetails.Select(x => x.FSCDID).Distinct().ToList());
                if (!string.IsNullOrEmpty(sSQL))
                {
                    double nQtyPro = 0;
                    double nAmountAddDis = 0;
                   // sSQL = "SELECT * FROM View_FabricSalesContractDetail FSCD WHERE  FabricSalesContractID in (" + sSQL + ")";
                    sSQL = "SELECT * FROM View_FabricSalesContractDetail FSCD WHERE  FabricSalesContractID in (SELECT FabricSalesContractID FROM FabricSalesContractDetail where  FabricSalesContractDetailID in (" + sSQL + "))";
                    oFabricSalesContractDetails = FabricSalesContractDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    var oFSCDPro = oFabricSalesContractDetails.Where(x => x.SCDetailType == EnumSCDetailType.None || x.SCDetailType == EnumSCDetailType.ExtraOrder).ToList();
                    var oFSCDDisCount = oFabricSalesContractDetails.Where(x => x.SCDetailType == EnumSCDetailType.DeductCharge || x.SCDetailType == EnumSCDetailType.AddCharge).ToList();
                    //  var oFSCDAddtionCount = oFabricSalesContractDetails.Where(x => x.SCDetailType == EnumSCDetailType.AddCharge).ToList();

                    var oFDCDetails = oFSCDDisCount.GroupBy(x => new { x.ProductID, x.ProductName, x.SCDetailType, x.UnitPrice }, (key, grp) =>
                                   new
                                   {
                                       ProductID = key.ProductID,
                                       ProductName = key.ProductName,
                                       SCDetailType = key.SCDetailType,
                                       UnitPrice = key.UnitPrice,
                                       Qty = grp.Sum(p => p.Qty)
                                   }).ToList();



                    nQtyPro = oFSCDPro.Sum(x => x.Qty);
                    nQty = oFDC.FDCDetails.Sum(x => x.Qty);
                    //nAmountAddDis = oFSCDDisCount.Sum(x => x.Qty*x.UnitPrice);
                    foreach (var oItem1 in oFDCDetails)
                    {
                        _oFDCDetail = new FabricDeliveryChallanDetail();
                        _oFDCDetail.Qty = oItem1.Qty * nQty / nQtyPro;
                        _oFDCDetail.UnitPrice = oItem1.UnitPrice;
                        _oFDCDetail.SCDetailType = oItem1.SCDetailType;
                        _oFDCDetail.ProductName = oItem1.ProductName;
                        oFDC.FDCDetails.Add(_oFDCDetail);
                    }
                }
                
            }

            if (bIsMeter)
            {
                // _oFabricDeliveryOrder.FDODetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));

                oFDC.FDCDetails.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                oFDC.FDCDetails.ForEach(o => o.MUName = "Meter");
                oFabricDeliveryChallanDetailsAdj.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
            }
            else
            {
                oFDC.FDCDetails.ForEach(o => o.MUName = "Yard");
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oFDC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFDC.DeliveryToAddress = oContractor.Address;

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricDeliveryChallan + " AND BUID = " + nBUID + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup = oDeliverySetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            oFDOrderSetup = oFDOrderSetup.GetByType((int)oFDC.FDOType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            bool isPadFormat = (oFDOrderSetup.PrintFormat == 1);
            if (nFormat > -1)
                isPadFormat = (nFormat == 1);
            oFDC.IsBill = true;
            if (oDeliverySetup.DeliverySetupID > 0 && oDeliverySetup.ChallanPrintNo == EnumExcellColumn.B)
            {
                rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
                byte[] abytes = oReport.PrepareReport(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
                return File(abytes, "application/pdf");
            }
            else
            {
                oDeliverySetup.DCPrefix = "DCF";
                oDeliverySetup.GPPrefix = "GPF";
                if (string.IsNullOrEmpty(oFDC.GatePassNo)) { oFDC.GatePassNo = oFDC.ChallanNo; }
                oFDC.ChallanNo = oDeliverySetup.DCPrefix + "-" + oFDC.ChallanNo;
                oFDC.GatePassNo = oDeliverySetup.GPPrefix + "-" + oFDC.GatePassNo;
                oFDC.IsBill = true;
                rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
                byte[] abytes = oReport.PrepareReport(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintPackingList(int id, int nBUID, bool bIsMeter)
        {
            bool bIsGatePass = false;
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            oFDC = FabricDeliveryChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj = new List<FabricDeliveryChallanDetail>();
            FabricSCReport oFabricSCReport = new FabricSCReport();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(nBUID, (int)Session[SessionInfo.currentUserID]);

            if (oFDC.FDCID > 0)
            {
                oFDC.FDCDetails = FabricDeliveryChallanDetail.Gets(oFDC.FDCID, oFDC.IsSample, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oFDC.FDCDetails.Count > 0)
            {
                oFDC.BuyerName = oFDC.FDCDetails.Where(p => p.BuyerName != null && p.BuyerName != "").Select(x => x.BuyerName).FirstOrDefault();
                oFDC.BuyerCPName = oFDC.FDCDetails.Where(p => p.BuyerCPName != null && p.BuyerCPName != "").Select(x => x.BuyerCPName).FirstOrDefault();
                oFDC.MKTPerson = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();
                //  oFDC.Fab = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();

                if (oFDC.IsSample)
                {
                    oFDC.FabricDONo = string.Join(",", oFDC.FDCDetails.Select(x => x.FEONo).Distinct().ToList());
                    oFabricSCReport = oFabricSCReport.Get(oFDC.FDCDetails[0].FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFDC.DeliveryToName = oFabricSCReport.ContractorName;
                    oFDC.ContractorID = oFabricSCReport.ContractorID;
                    oFDC.BuyerCPName = oFabricSCReport.BuyerCPName;
                    oFDC.MKTPerson = oFabricSCReport.MKTPName;
                }
                oFabricDeliveryChallanDetailsAdj = FabricDeliveryChallanDetail.GetsForAdj(0, oFDC.FDCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (bIsMeter)
            {
                // _oFabricDeliveryOrder.FDODetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));

                oFDC.FDCDetails.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                oFDC.FDCDetails.ForEach(o => o.MUName = "Meter");
                oFabricDeliveryChallanDetailsAdj.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
            }
            else
            {
                oFDC.FDCDetails.ForEach(o => o.MUName = "Yard");
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oFDC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFDC.DeliveryToAddress = oContractor.Address;

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricDeliveryChallan + " AND BUID = " + nBUID + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup = oDeliverySetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

          
            oFDC.ChallanNo = "DCF-" + oFDC.ChallanNo;
            rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
            byte[] abytes = oReport.PrepareReport_PackingList(oFDC, oCompany, bIsGatePass, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
            return File(abytes, "application/pdf");
           
        }
        public ActionResult PrintFDCNew(int id, int nBUID, bool bIsMeter, int nFormat)
        {
            bool bIsGatePass = false;
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            oFDC = FabricDeliveryChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj = new List<FabricDeliveryChallanDetail>();
            FabricSCReport oFabricSCReport = new FabricSCReport();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(nBUID, (int)Session[SessionInfo.currentUserID]);

            if (oFDC.FDCID > 0)
            {
                oFDC.FDCDetails = FabricDeliveryChallanDetail.Gets(oFDC.FDCID, oFDC.IsSample, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oFDC.FDCDetails.Count > 0)
            {
                oFDC.BuyerName = oFDC.FDCDetails.Where(p => p.BuyerName != null && p.BuyerName != "").Select(x => x.BuyerName).FirstOrDefault();
                oFDC.BuyerCPName = oFDC.FDCDetails.Where(p => p.BuyerCPName != null && p.BuyerCPName != "").Select(x => x.BuyerCPName).FirstOrDefault();
                oFDC.MKTPerson = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();
                //  oFDC.Fab = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();

                if (oFDC.IsSample)
                {
                    oFDC.FabricDONo = string.Join(",", oFDC.FDCDetails.Select(x => x.FEONo).Distinct().ToList());
                    oFabricSCReport = oFabricSCReport.Get(oFDC.FDCDetails[0].FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFDC.DeliveryToName = oFabricSCReport.ContractorName;
                    oFDC.ContractorID = oFabricSCReport.ContractorID;
                    oFDC.BuyerCPName = oFabricSCReport.BuyerCPName;
                    oFDC.MKTPerson = oFabricSCReport.MKTPName;
                }
                oFabricDeliveryChallanDetailsAdj = FabricDeliveryChallanDetail.GetsForAdj(0, oFDC.FDCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (bIsMeter)
            {
                // _oFabricDeliveryOrder.FDODetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));

                oFDC.FDCDetails.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                oFDC.FDCDetails.ForEach(o => o.MUName = "Meter");
                oFabricDeliveryChallanDetailsAdj.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
            }
            else
            {
                oFDC.FDCDetails.ForEach(o => o.MUName = "Yard");
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oFDC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFDC.DeliveryToAddress = oContractor.Address;

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricDeliveryChallan + " AND BUID = " + nBUID + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup = oDeliverySetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            oFDOrderSetup = oFDOrderSetup.GetByType((int)oFDC.FDOType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            bool isPadFormat = (oFDOrderSetup.PrintFormat == 1);
            if (nFormat > -1)
                isPadFormat = (nFormat == 1);

            if (oDeliverySetup.DeliverySetupID > 0 && oDeliverySetup.ChallanPrintNo == EnumExcellColumn.B)
            {
                rptFabricDeliveryChallanNew oReport = new rptFabricDeliveryChallanNew();
                byte[] abytes = oReport.PrepareReport_B(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads);
                return File(abytes, "application/pdf");
            }
            else
            {
                oFDC.ChallanNo = "DCF-" + oFDC.ChallanNo;
                rptFabricDeliveryChallanNew oReport = new rptFabricDeliveryChallanNew();
                byte[] abytes = oReport.PrepareReport(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintFDC_GatePass(int id, int nBUID, bool bIsMeter, int nFormat)
        {
            bool bIsGatePass = true;
            FabricSCReport oFabricSCReport = new FabricSCReport();
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            oFDC = FabricDeliveryChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj = new List<FabricDeliveryChallanDetail>();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(nBUID, (int)Session[SessionInfo.currentUserID]);

            if (oFDC.FDCID > 0)
            {
                oFDC.FDCDetails = FabricDeliveryChallanDetail.Gets(oFDC.FDCID, oFDC.IsSample,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oFDC.FDCDetails.Count > 0)
            {
                oFDC.BuyerName = oFDC.FDCDetails.Where(p => p.BuyerName != null && p.BuyerName != "").Select(x => x.BuyerName).FirstOrDefault();
                oFDC.BuyerCPName = oFDC.FDCDetails.Where(p => p.BuyerCPName != null && p.BuyerCPName != "").Select(x => x.BuyerCPName).FirstOrDefault();
                oFDC.MKTPerson = oFDC.FDCDetails.Where(p => p.MKTPerson != null && p.MKTPerson != "").Select(x => x.MKTPerson).FirstOrDefault();
                if (oFDC.IsSample)
                {
                    oFDC.FabricDONo = string.Join(",", oFDC.FDCDetails.Select(x => x.FEONo).Distinct().ToList());
                    oFabricSCReport = oFabricSCReport.Get(oFDC.FDCDetails[0].FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFDC.DeliveryToName = oFabricSCReport.ContractorName;
                    oFDC.ContractorID = oFabricSCReport.ContractorID;
                    oFDC.BuyerCPName = oFabricSCReport.BuyerCPName;
                    oFDC.MKTPerson = oFabricSCReport.MKTPName;
                }
                oFabricDeliveryChallanDetailsAdj = FabricDeliveryChallanDetail.GetsForAdj(0, oFDC.FDCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (bIsMeter)
            {
                oFDC.FDCDetails.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                oFDC.FDCDetails.ForEach(o => o.MUName = "Meter");
                oFabricDeliveryChallanDetailsAdj.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
            }
            else
            {
                oFDC.FDCDetails.ForEach(o => o.MUName = "Yard");
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricDeliveryChallan + " AND BUID = " + nBUID + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oFDC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFDC.DeliveryToAddress = oContractor.Address;

            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup = oDeliverySetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            oFDOrderSetup = oFDOrderSetup.GetByType((int)oFDC.FDOType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            bool isPadFormat = (oFDOrderSetup.PrintFormat == 1);
            if (nFormat > -1)
                isPadFormat = (nFormat == 1);

            if (oDeliverySetup.DeliverySetupID > 0 && oDeliverySetup.ChallanPrintNo == EnumExcellColumn.B)
            {
                rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
                byte[] abytes = oReport.PrepareReportGatePass(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
                return File(abytes, "application/pdf");
            }
            else
            {
                //oFDC.ChallanNo = "GPF-" + oFDC.ChallanNo;
                oDeliverySetup.DCPrefix = "DCF";
                oDeliverySetup.GPPrefix = "GPF";
                if (string.IsNullOrEmpty(oFDC.GatePassNo)) { oFDC.GatePassNo = oFDC.ChallanNo; }
                oFDC.ChallanNo = oDeliverySetup.DCPrefix + "-" + oFDC.ChallanNo;
                oFDC.GatePassNo = oDeliverySetup.GPPrefix + "-" + oFDC.GatePassNo;
                rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
                byte[] abytes = oReport.PrepareReport(oFDC, oCompany, bIsGatePass, isPadFormat, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
                return File(abytes, "application/pdf");
            }
            //rptFabricDeliveryChallan oReport = new rptFabricDeliveryChallan();
            //byte[] abytes = oReport.PrepareReport(oFDC, oCompany, bIsGatePass, oBusinessUnit, oApprovalHeads, oFabricDeliveryChallanDetailsAdj);
            //return File(abytes, "application/pdf");

        }
        #endregion

        #region Adv Search
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        public JsonResult AdvSearch(string sTemp)
        {
            List<FabricDeliveryChallan> oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oFabricDeliveryChallans = FabricDeliveryChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
                oFabricDeliveryChallans.Add(new FabricDeliveryChallan() {ErrorMessage=ex.Message });
            }
            var jsonResult = Json(oFabricDeliveryChallans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(string sTemp)
        {
            int index = 0;
            int ncboDCDate = Convert.ToInt32(sTemp.Split('~')[index++]);
            DateTime txtDCDateFrom = Convert.ToDateTime(sTemp.Split('~')[index++]);
            DateTime txtDCDateTo = Convert.ToDateTime(sTemp.Split('~')[index++]);
            //
            string sChallanNo = Convert.ToString(sTemp.Split('~')[index++]);
            string sFDONo = Convert.ToString(sTemp.Split('~')[index++]);
            string sApplicantIDs = Convert.ToString(sTemp.Split('~')[index++]);
            string sFabricNo = Convert.ToString(sTemp.Split('~')[index++]);
            string sLotNo = Convert.ToString(sTemp.Split('~')[index++]);
            string sOrderNo = Convert.ToString(sTemp.Split('~')[index++]);
            bool sIsSample = Convert.ToBoolean(sTemp.Split('~')[index++]);
            int nFDOType = Convert.ToInt32(sTemp.Split('~')[index++]);
            string sPINo = Convert.ToString(sTemp.Split('~')[index++]);
            string MKTPersonIDs = Convert.ToString(sTemp.Split('~')[index++]);
            string MKTGroupIDs = Convert.ToString(sTemp.Split('~')[index++]);

            string sReturn1 = "SELECT * FROM View_FabricDeliveryChallan ";
            string sReturn = "";

            #region DO No
            if (!string.IsNullOrEmpty(sFDONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricDONo LIKE '%" + sFDONo + "%' ";
            }
            #endregion

            #region PI No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDCID IN (SELECT FDCID FROM View_FabricDeliveryChallanDetail WHERE PINo LIKE '%" + sPINo + "%' )";
            }
            #endregion

            #region DO Type
            if (nFDOType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOType =" + nFDOType ;
            }
            #endregion

            #region Challan No
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + sChallanNo + "%' ";
            }
            #endregion

            #region Fabric No
            if (!string.IsNullOrEmpty(sFabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDCID IN (SELECT FDCID FROM View_FabricDeliveryChallanDetail WHERE FabricNo LIKE '%" + sFabricNo + "%' )";
            }
            #endregion

            #region Lot No
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDCID IN (SELECT FDCID FROM View_FabricDeliveryChallanDetail WHERE LotNo LIKE '%" + sLotNo + "%' )";
            }
            #endregion

            #region FEONo No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDCID IN (SELECT FDCID FROM View_FabricDeliveryChallanDetail WHERE FEONo LIKE '%" + sOrderNo + "%' )";
            }
            #endregion

            #region Delivery To
            if (!string.IsNullOrEmpty(sApplicantIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID in( " + sApplicantIDs + ")";
            }
            #endregion

            #region Issue Date Wise
            if (ncboDCDate > 0)
            {
                if (ncboDCDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDCDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDCDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDCDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDCDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateTo.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDCDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDCDateTo.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Is Sample
            if (sIsSample)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(IsSample,0) = 1 ";
            }
            else 
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(IsSample,0) = 0 ";
            }
            #endregion

            #region MKTPerson ID
            if (!string.IsNullOrEmpty(MKTPersonIDs))
            {
                Global.TagSQL(ref sReturn);
                //sReturn = sReturn + " FDCID IN (SELECT FDCID FROM View_FabricDeliveryChallanDetail WHERE MKTPersonID IN (" + MKTPersonIDs + ")) ";
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryOrderDetail WHERE ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in  (" + MKTPersonIDs + ")) OR (isnull(ExportPIID,0)<=0 and FEOID in (Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in  (" + MKTPersonIDs + ")))))";
            }
            #endregion

            #region MKTGroup ID
            if (!string.IsNullOrEmpty(MKTGroupIDs))
            {
                Global.TagSQL(ref sReturn);
                //sReturn = sReturn + " FDCID IN (SELECT FDCID FROM View_FabricDeliveryChallanDetail WHERE MKTPersonID IN (Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + MKTGroupIDs + "))) ";
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryOrderDetail WHERE ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in  (Select MarketingAccountID  from MarketingAccount where GroupID >0 and GroupID in (" + MKTGroupIDs + "))) OR (isnull(ExportPIID,0)<=0 and FEOID in (Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in  (Select MarketingAccountID  from MarketingAccount where GroupID >0 and GroupID in (" + MKTGroupIDs + "))))))";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY FDOID";
            return sSQL;
        }
        #endregion
    }
}