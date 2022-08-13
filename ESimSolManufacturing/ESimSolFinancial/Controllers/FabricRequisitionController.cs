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
    public class FabricRequisitionController : Controller
    {
        #region Declaration

        FabricRequisition _oFabricRequisition = new FabricRequisition();
        List<FabricRequisition> _oFabricRequisitions = new List<FabricRequisition>();
        FabricRequisitionDetail _oFabricRequisitionDetail = new FabricRequisitionDetail();
        List<FabricRequisitionDetail> _oFabricRequisitionDetails = new List<FabricRequisitionDetail>();
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewFabricRequisitions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricRequisition).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricRequisitions = new List<FabricRequisition>();
            _oFabricRequisitions = FabricRequisition.Gets("SELECT * FROM View_FabricRequisition WHERE ISNULL(DisburseBy,0) = 0", (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            #region Gets Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            ViewBag.IssueStores = oWorkingUnits;
            oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricRequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            ViewBag.ReceiveStores = oWorkingUnits;
            #endregion
            return View(_oFabricRequisitions);
        }

        public ActionResult ViewFabricRequisition(int id, int buid)
        {
            _oFabricRequisition = new FabricRequisition();
            if (id > 0)
            {
                _oFabricRequisition = _oFabricRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFabricRequisition.FabricRequisitionDetails = FabricRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.BUID = buid;
            #region Gets Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            ViewBag.IssueStores = oWorkingUnits;
            oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricRequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            ViewBag.ReceiveStores = oWorkingUnits;
            #endregion


            return View(_oFabricRequisition);
        }

        [HttpPost]
        public JsonResult Save(FabricRequisition oFabricRequisition)
        {
            _oFabricRequisition = new FabricRequisition();
            try
            {
                _oFabricRequisition = oFabricRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricRequisition = new FabricRequisition();
                _oFabricRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(FabricRequisition oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricRequisition oFabricRequisition = new FabricRequisition();
                sFeedBackMessage = oFabricRequisition.Delete(oJB.FabricRequisitionID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult DeleteFabricRequisitionRoll(FabricRequisitionRoll oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
                sFeedBackMessage = oFabricRequisitionRoll.Delete(oJB.FabricRequisitionRollID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult SaveFabricRequisitionRoll(List<FabricRequisitionRoll> oFabricRequisitionRolls)
        {
            List<FabricRequisitionRoll> oFRRs = new List<FabricRequisitionRoll>();
            FabricRequisitionRoll oFRR = new FabricRequisitionRoll();
            try
            {
                oFRRs = oFRR.SaveFabricRequisitionRoll(oFabricRequisitionRolls, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFRRs = new List<FabricRequisitionRoll>();
                oFRR = new FabricRequisitionRoll();
                oFRR.ErrorMessage = ex.Message;
                oFRRs.Add(oFRR);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFRRs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRoll(FabricRequisitionRoll oFabricRequisitionRoll)
        {
            FabricRequisitionRoll _oFabricRequisitionRoll = new FabricRequisitionRoll();
            try
            {
                _oFabricRequisitionRoll = oFabricRequisitionRoll.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricRequisitionRoll = new FabricRequisitionRoll();
                _oFabricRequisitionRoll.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricRequisitionRoll);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeStatus(FabricRequisition oFabricRequisition)
        {
            _oFabricRequisition = new FabricRequisition();
            try
            {
                _oFabricRequisition = oFabricRequisition.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricRequisition = new FabricRequisition();
                _oFabricRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Adv Search
        [HttpPost]
        public JsonResult AdvSearch(FabricRequisition oFabricRequisition)
        {
            List<FabricRequisition> oFabricRequisitions = new List<FabricRequisition>();
            string sSQL = MakeSQL(oFabricRequisition);
            oFabricRequisitions = FabricRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(oFabricRequisitions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQL(FabricRequisition oFabricRequisition)
        {
            int nDateCriteria_QuotationDate = 0, nRcvDate = 0, nDisburseDate = 0;
            string sDispoNo = "", sLotNo = "", sReqNo = "";
            DateTime dStart_QuotationDate = DateTime.Today, dEnd_QuotationDate = DateTime.Today, dStart_RcvDate = DateTime.Today, dEnd_RcvDate = DateTime.Today,
                dStart_DisburseDate = DateTime.Today, dEnd_DisburseDate = DateTime.Today;

            if (!String.IsNullOrEmpty(oFabricRequisition.ErrorMessage))
            {
                nDateCriteria_QuotationDate = Convert.ToInt32(oFabricRequisition.ErrorMessage.Split('~')[0]);
                dStart_QuotationDate = Convert.ToDateTime(oFabricRequisition.ErrorMessage.Split('~')[1]);
                dEnd_QuotationDate = Convert.ToDateTime(oFabricRequisition.ErrorMessage.Split('~')[2]);

                nRcvDate = Convert.ToInt32(oFabricRequisition.ErrorMessage.Split('~')[3]);
                dStart_RcvDate = Convert.ToDateTime(oFabricRequisition.ErrorMessage.Split('~')[4]);
                dEnd_RcvDate = Convert.ToDateTime(oFabricRequisition.ErrorMessage.Split('~')[5]);

                nDisburseDate = Convert.ToInt32(oFabricRequisition.ErrorMessage.Split('~')[6]);
                dStart_DisburseDate = Convert.ToDateTime(oFabricRequisition.ErrorMessage.Split('~')[7]);
                dEnd_DisburseDate = Convert.ToDateTime(oFabricRequisition.ErrorMessage.Split('~')[8]);

                sDispoNo = oFabricRequisition.ErrorMessage.Split('~')[9];
                sReqNo = oFabricRequisition.ErrorMessage.Split('~')[10];
                sLotNo = oFabricRequisition.ErrorMessage.Split('~')[11];
            }

            string sReturn1 = "SELECT * FROM View_FabricRequisition";
            string sReturn = "";

            #region sReqNo
            if (!string.IsNullOrEmpty(sReqNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReqNo LIKE '%" + sReqNo + "%'";
            }
            #endregion

            #region Req. DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " ReqDate", nDateCriteria_QuotationDate, dStart_QuotationDate, dEnd_QuotationDate);
            #endregion

            #region RCV DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " ReceiveDate", nRcvDate, dStart_RcvDate, dEnd_RcvDate);
            #endregion

            #region Disburse DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " DisburseDate", nDisburseDate, dStart_DisburseDate, dEnd_DisburseDate);
            #endregion

            #region DispoNo
            if (!string.IsNullOrEmpty(sDispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricRequisitionID IN (SELECT FabricRequisitionID FROM View_FabricRequisitionDetail WHERE ExeNo LIKE '%" + sDispoNo + "%')";
            }
            #endregion

            #region Lot No
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricRequisitionID IN (SELECT FabricRequisitionID FROM FabricRequisitionDetail WHERE FabricRequisitionDetailID IN (SELECT FabricRequisitionDetailID FROM View_FabricRequisitionRoll WHERE LotNo LIKE '%" + sLotNo + "%'))";
            }
            #endregion

            #region IssueStore
            if (oFabricRequisition.IssueStoreID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " IssueStoreID = " + oFabricRequisition.IssueStoreID;
            }
            #endregion

            #region ReceiveStore
            if (oFabricRequisition.ReceiveStoreID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReceiveStoreID = " + oFabricRequisition.ReceiveStoreID;
            }
            #endregion

            #region RequisitionType
            if (oFabricRequisition.RequisitionType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionType = " + oFabricRequisition.RequisitionType;
            }
            #endregion
            

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult GetDispos(FabricExecutionOrderSpecification oFabricExecutionOrderSpecification)
        {
            List<FabricExecutionOrderSpecification> oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
            try
            {
                string Ssql = "SELECT TOP 500 * FROM View_FabricExecutionOrderSpecification WHERE ProdtionType IN (1,2) ";
                if (!string.IsNullOrEmpty(oFabricExecutionOrderSpecification.ExeNo))
                {
                    Global.TagSQL(ref Ssql);
                    Ssql = Ssql + " ExeNo LIKE '%" + oFabricExecutionOrderSpecification.ExeNo + "%'";
                }
                Ssql = Ssql + " order by FEOSID";
                oFabricExecutionOrderSpecifications = FabricExecutionOrderSpecification.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricExecutionOrderSpecification oFSCD = new FabricExecutionOrderSpecification();
                oFSCD.ErrorMessage = ex.Message;
                oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
                oFabricExecutionOrderSpecifications.Add(oFSCD);
            }
            var jSonResult = Json(oFabricExecutionOrderSpecifications, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetFabricRequisitionRolls(FabricRequisitionDetail oFabricRequisitionDetail)
        {
            List<FabricRequisitionRoll> oFRRs = new List<FabricRequisitionRoll>();
            FabricRequisitionRoll oFRR = new FabricRequisitionRoll();
            try
            {
                if (oFabricRequisitionDetail.RollType == 1)
                {
                    oFRRs = FabricRequisitionRoll.Gets("SELECT * FROM View_FabricRequisitionRoll WHERE FabricRequisitionDetailID = " + oFabricRequisitionDetail.FabricRequisitionDetailID + " AND FabricBatchQCLotID<=0", (int)Session[SessionInfo.currentUserID]);
                }
                if (oFabricRequisitionDetail.RollType == 2)
                {
                    oFRRs = FabricRequisitionRoll.Gets("SELECT * FROM View_FabricRequisitionRoll WHERE FabricRequisitionDetailID = " + oFabricRequisitionDetail.FabricRequisitionDetailID + " AND FabricBatchQCLotID>0", (int)Session[SessionInfo.currentUserID]);
                }
                
            }
            catch (Exception ex)
            {
                oFRRs = new List<FabricRequisitionRoll>();
                oFRR = new FabricRequisitionRoll();
                oFRR.ErrorMessage = ex.Message;
                oFRRs.Add(oFRR);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFRRs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRollNos(FabricRequisitionRoll oFRR)
        {
            List<FabricBatchQCDetail> oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            List<FabricRequisitionRoll> oFabricRequisitionRolls = new List<FabricRequisitionRoll>();
            List<FabricRequisitionRoll> otempRequisitionRolls = new List<FabricRequisitionRoll>();
            FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
            
            string sSQL = "";

            try
            {
                if (oFRR.RollType == 1)
                {
                    sSQL = "SELECT * ,isnull((Select SUM(Qty) from FabricRequisitionRoll where FBQCDetailID=TT.FBQCDetailID ),0) as QtyTR   FROM view_FabricBatchQCDetail as TT where LotID in (Select LotID from Lot where Balance>0.1 and  WorkingUnitID=" + oFRR.WUID + ") and isnull(ReceiveBy,0)<>0 and FEOID >0";
                    if (oFRR.FEOID > 0)
                        sSQL += " AND FEOID= " + oFRR.FEOID + "";
                    if (!string.IsNullOrEmpty(oFRR.DispoNo))
                        sSQL += " AND DispoNo LIKE '%" + oFRR.DispoNo + "%'";
                    if (!string.IsNullOrEmpty(oFRR.LotNo))
                        sSQL += " AND LotNo LIKE '%" + oFRR.LotNo + "%' ";
                    sSQL = sSQL + " ORDER BY LotNo";
                    oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (FabricBatchQCDetail oItem in oFabricBatchQCDetails)
                    {
                        oFabricRequisitionRoll = new FabricRequisitionRoll();
                        oFabricRequisitionRoll.FabricRequisitionDetailID = oFRR.FabricRequisitionDetailID;
                        oFabricRequisitionRoll.FabricRequisitionRollID = oFRR.FabricRequisitionRollID;
                        oFabricRequisitionRoll.FEOID = 0;
                        oFabricRequisitionRoll.LotID = oItem.LotID;
                        oFabricRequisitionRoll.Qty = oItem.Qty - oItem.QtyTR;
                        //oFabricRequisitionRoll.Bal = oItem.LotBalance;
                        oFabricRequisitionRoll.DispoNo = oItem.DispoNo;
                        oFabricRequisitionRoll.MUName = "Y";
                        oFabricRequisitionRoll.LotNo = oItem.LotNo;


                        oFabricRequisitionRoll.FBQCDetailID = oItem.FBQCDetailID;
                        if (oFabricRequisitionRoll.Qty > 0)
                        {                         
                            oFabricRequisitionRolls.Add(oFabricRequisitionRoll);
                        }
                    }
                }

                if (oFRR.RollType == 2)
                {
                    sSQL = "SELECT *,isnull((Select SUM(Qty) from FabricRequisitionRoll where FabricBatchQCLotID=TT.FabricBatchQCLotID ),0) as QtyTR FROM View_FabricBatchQCLot AS TT WHERE FSCDID= " + oFRR.FEOID + " AND LotID in (Select LotID from Lot where Balance>0.1 and  WorkingUnitID=" + oFRR.WUID + ")";
                    oFabricRequisitionRolls = FabricRequisitionRoll.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);               
                    foreach(FabricRequisitionRoll oItem in oFabricRequisitionRolls){
                        oFabricRequisitionRoll = new FabricRequisitionRoll();
                        if (oItem.YetQty > 0)
                        {
                            oItem.Qty = oItem.YetQty;
                            otempRequisitionRolls.Add(oItem);
                        }
                    }

                    oFabricRequisitionRolls =  otempRequisitionRolls;

                }
                
    
               

                //string sSQL = "SELECT * FROM View_Lot WHERE Balance > 0 AND ParentType=" + (int)EnumTriggerParentsType.FabricQCDetail + " AND WorkingUnitID=" + oFRR.WUID;
                //if (oFRR.FEOID > 0)
                //    sSQL += " AND ParentID IN (SELECT FBQCDetailID FROM FabricBatchQCDetail WHERE FBQCID IN ( SELECT FBQCID FROM FabricBatchQC WHERE FBID IN (SELECT FBID FROM FabricBatch WHERE FEOID = " + oFRR.FEOID + ")))";
                //if (!string.IsNullOrEmpty(oFRR.FEONo))
                //    sSQL += " AND ParentID IN (SELECT FBQCDetailID FROM FabricBatchQCDetail WHERE FBQCID IN ( SELECT FBQCID FROM FabricBatchQC WHERE FBID IN (SELECT FBID FROM View_FabricBatch WHERE FEONo LIKE '%" + oFRR.FEONo + "%')))";
                //if (!string.IsNullOrEmpty(oFRR.LotNo))
                //    sSQL += " AND LotNo LIKE '%" + oFRR.LotNo + "%' ";
                //sSQL = sSQL + " ORDER BY LotNo";
                //oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                FabricRequisitionRoll ooFabricRequisitionRoll = new FabricRequisitionRoll();
                oFabricRequisitionRoll.ErrorMessage = ex.Message;
                oFabricRequisitionRolls.Add(oFabricRequisitionRoll);
            }
            var jsonResult = Json(oFabricRequisitionRolls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region receive
        [HttpPost]
        public JsonResult ReceiveFabricRequisition(FabricRequisition oFabricRequisition)
        {
            try
            {
                oFabricRequisition = oFabricRequisition.Receive(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (string.IsNullOrEmpty(oFabricRequisition.ErrorMessage) && oFabricRequisition.IsFabricRequisitionRoll)
                {
                    if (oFabricRequisition.FabricRequisitionRoll.FabricRequisitionDetailID > 0)
                    {
                        oFabricRequisition.FabricRequisitionRoll = FabricRequisitionRoll.GetByDetailID(oFabricRequisition.FabricRequisitionRoll.FabricRequisitionDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                oFabricRequisition = new FabricRequisition();
                oFabricRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region print
        public ActionResult FabricRequisitionPrintPreview(int id, int buid, bool bIsInMtr)
        {
            _oFabricRequisition = new FabricRequisition();
            List<FabricRequisitionDetail> oFabricRequisitionDetails = new List<FabricRequisitionDetail>();
            List<FabricRequisitionRoll> oFabricRequisitionRolls = new List<FabricRequisitionRoll>();
            if (id > 0)
            {
                _oFabricRequisition = _oFabricRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                oFabricRequisitionDetails = FabricRequisitionDetail.Gets(_oFabricRequisition.FabricRequisitionID, (int)Session[SessionInfo.currentUserID]);
                oFabricRequisitionRolls = FabricRequisitionRoll.Gets("SELECT * FROM View_FabricRequisitionRoll WHERE FabricRequisitionDetailID IN (" + string.Join(",", oFabricRequisitionDetails.Select(x => x.FabricRequisitionDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (buid > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }

            byte[] abytes;
            rptFabricRequisition oReport = new rptFabricRequisition();
            abytes = oReport.PrepareReport(_oFabricRequisition, oFabricRequisitionDetails, oFabricRequisitionRolls, oCompany, bIsInMtr);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(Company oCompany)
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

        [HttpPost]
        public ActionResult SetSessionSearchCriterias(FabricRequisition oFabricRequisition)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricRequisition);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintLists(double ts)
        {
            _oFabricRequisitions = new List<FabricRequisition>();
            _oFabricRequisitionDetails = new List<FabricRequisitionDetail>();
            string sSQL = ""; string sReportHeader = ""; string sIDs = ""; string sHeaderParam = "";
            try
            {
                _oFabricRequisition = (FabricRequisition)Session[SessionInfo.ParamObj];
                sHeaderParam = _oFabricRequisition.ErrorMessage;
                 sSQL = MakeSQL(_oFabricRequisition);
                _oFabricRequisitions = FabricRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sIDs = string.Join(",", _oFabricRequisitions.Select(s => s.FabricRequisitionID).Distinct());
                if(!String.IsNullOrEmpty(sIDs))
                _oFabricRequisitionDetails = FabricRequisitionDetail.Gets("SELECT * FROM View_FabricRequisitionDetail Where FabricRequisitionID IN ("+sIDs+") Order By FabricRequisitionID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricRequisition = new FabricRequisition();
                _oFabricRequisition.ErrorMessage = ex.Message;
                _oFabricRequisitions.Add(_oFabricRequisition);
            }

            if (_oFabricRequisitions.Count>0)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptFabricRequisitions oReport = new rptFabricRequisitions();
                if (!String.IsNullOrEmpty(sHeaderParam))
                {
                   
                    int nReqDate = Convert.ToInt32(sHeaderParam.Split('~')[0]);
                    int nRcvDate = Convert.ToInt32(sHeaderParam.Split('~')[3]);
                    int nDisburseDate = Convert.ToInt32(sHeaderParam.Split('~')[6]);
                    #region Req Date
                    if (nReqDate > 0)
                    {
                        DateTime nReqStartDate = Convert.ToDateTime(sHeaderParam.Split('~')[1]);
                        DateTime nReqEndDate = Convert.ToDateTime(sHeaderParam.Split('~')[2]);
                        if (nReqDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader += "Request Date " + nReqStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader += ", Request Date " + nReqStartDate.ToString("dd MMM yyyy");
                            }
                        }
                        if (nReqDate == 5)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader += "Request Date " + nReqStartDate.ToString("dd MMM yyyy") + " To " + nReqEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader += ",Request Date " + nReqStartDate.ToString("dd MMM yyyy") + " To " + nReqEndDate.ToString("dd MMM yyyy");
                            }
                        }
                    }
                    #endregion
                    #region Rcv Date
                    if (nRcvDate > 0)
                    {
                        DateTime nRcvStartDate = Convert.ToDateTime(sHeaderParam.Split('~')[4]);
                        DateTime nRcvEndDate = Convert.ToDateTime(sHeaderParam.Split('~')[5]);
                        if (nRcvDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader += "Receive Date " + nRcvStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader += ", Receive Date " + nRcvStartDate.ToString("dd MMM yyyy");
                            }
                        }
                        if (nRcvDate == 5)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader += "Receive Date " + nRcvStartDate.ToString("dd MMM yyyy") + " To " + nRcvEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader += ",Receive Date " + nRcvStartDate.ToString("dd MMM yyyy") + " To " + nRcvEndDate.ToString("dd MMM yyyy");
                            }
                        }
                    }
                    #endregion
                    #region disburse Date
                    if (nDisburseDate > 0)
                    {
                        DateTime nDisburseStartDate = Convert.ToDateTime(sHeaderParam.Split('~')[7]);
                        DateTime nDisburseEndDate = Convert.ToDateTime(sHeaderParam.Split('~')[8]);
                        if (nDisburseDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader += "Disburse Date " + nDisburseStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader += ", Disburse Date " + nDisburseStartDate.ToString("dd MMM yyyy");
                            }
                        }
                        if (nDisburseDate == 5)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader += "Disburse Date " + nDisburseStartDate.ToString("dd MMM yyyy") + " To " + nDisburseEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader += ",Disburse Date " + nDisburseStartDate.ToString("dd MMM yyyy") + " To " + nDisburseEndDate.ToString("dd MMM yyyy");
                            }
                        }
                    }
                    #endregion
                }

                byte[] abytes = oReport.PrepareReport(_oFabricRequisitions,_oFabricRequisitionDetails, oCompany, sReportHeader);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }         
         }
        
        #endregion

    }
   
}
