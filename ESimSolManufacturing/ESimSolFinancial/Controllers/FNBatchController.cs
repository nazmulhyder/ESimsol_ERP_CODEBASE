using System;
using System.Collections.Generic;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class FNBatchController : PdfViewController
    {
        #region Declaration
        FNBatch _oFNBatch = new FNBatch();
        List<FNBatch> _oFNBatchs = new List<FNBatch>();
        FNBatchQC _oFNBatchQC = new FNBatchQC();
        FNBatchQCDetail _oFNBatchQCDetail = new FNBatchQCDetail();
        List<FNBatchQCDetail> _oFNBatchQCDetails = new List<FNBatchQCDetail>();
        #endregion

        #region   FNBatch
        public ActionResult ViewFNBatchs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oFNBatchs = new List<FNBatch>();
            string sSQL = "Select * from View_FNBatch as DD where DD.FNBatchStatus<=" + (int)EnumFNBatchStatus.Peaching;
            _oFNBatchs = FNBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<WorkingUnit> oWUs = new List<WorkingUnit>();
            oWUs = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricSalesContract, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
           // oWUs = WorkingUnit.Gets((int)EnumTriggerParentsType._FNProduction, (int)EnumInOutType._Disburse, false, "FNBatch", (int)EnumOperationFunctionality._Disburse, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); ViewBag.WUs = oWUs;
            return View(_oFNBatchs);
        }
        public ActionResult ViewFNBatch(int nId, double ts)
        {
            _oFNBatch = new FNBatch();
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            //List<FNExecutionOrderFabricReceive> oFNEOFRs = new List<FNExecutionOrderFabricReceive>();
            if (nId>0)
            {
                _oFNBatch = FNBatch.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "Select * from View_FNBatch Where FNExOID=" + _oFNBatch.FNExOID + " And FNBatchID!=" + _oFNBatch.FNBatchID + "";
                oFNBatchs = FNBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //sSQL = "Select * from View_FNExecutionOrderFabricReceive Where FNExOID=" + _oFNBatch.FNExOID + " And ReceiveBy<>0";
                //oFNEOFRs = FNExecutionOrderFabricReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            ViewBag.FNBatchs = oFNBatchs;
            //ViewBag.FNEOFRs = oFNEOFRs;
            return View(_oFNBatch);
        }
        public ActionResult ViewFNBatchs_FSC(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            string sSQL = "Select Top(20)* from View_FabricSalesContractReport Where ExeNo!='' order by FabricSalesContractDetailID DESC";
            oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           // List<WorkingUnit> oWUs = new List<WorkingUnit>();
           //// oWUs = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricSalesContract, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
           // oWUs = WorkingUnit.Gets( (int)Session[SessionInfo.currentUserID]);
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            ViewBag.FabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(oFabricSCReports);
        }
        public ActionResult ViewFNBatch_FSC(int nId, int buid)
        {
            _oFNBatch = new FNBatch();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            if (nId>0)
            {
                string sSQL = "Select * from View_FabricSalesContractReport Where FabricSalesContractDetailID=" + nId;
                oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = oFabricSCReports[0];
                _oFNBatchs = FNBatch.Gets("Select * from View_FNBatch Where FNExOID = " + nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            List<WorkingUnit> oWUs = new List<WorkingUnit>();
             oWUs = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricSalesContract, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
          //  oWUs = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FabricSCReport = oFabricSCReport;
            ViewBag.FNBatchs = _oFNBatchs;
            ViewBag.WUs = oWUs;
            return View(_oFNBatch);
        }

        [HttpPost]
        public JsonResult Search(FabricSCReport oFabricSCReport)
        {
            string sSQL = "Select Top(100)* from View_FabricSalesContractReport Where ExeNo!=''";
            //if (oFabricSCReport.SCNoFull == null) oFabricSCReport.SCNoFull = "";
            //if (oFabricSCReport.ExeNo == null) oFabricSCReport.ExeNo = "";
            if (!string.IsNullOrEmpty( oFabricSCReport.SCNoFull)) 
            {
                  sSQL =sSQL+ "And SCNoFull Like '%" + oFabricSCReport.SCNoFull.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(oFabricSCReport.ExeNo))
            {
                sSQL = sSQL+"And ExeNo Like '%" + oFabricSCReport.ExeNo.Trim() + "%'";
            }
             sSQL = sSQL + " Order By FabricSalesContractDetailID Desc";
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            //string sSQL = "Select Top(30)* from View_FabricSalesContractReport Where ExeNo!='' And SCNoFull Like '%" + oFabricSCReport.SCNoFull.Trim() + "%' And ExeNo Like '%" + oFabricSCReport.ExeNo + "%'  Order By FabricSalesContractDetailID Desc";
            oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
         
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(FNBatch oFNBatch)
        {
            try
            {
                if (oFNBatch.FNBatchID <= 0)
                {
                    oFNBatch = oFNBatch.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNBatch = oFNBatch.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNote(FNBatch oFNBatch)
        {
            FNBatch objFNBatch = new FNBatch();
            try
            {
                if (oFNBatch.FNBatchID > 0)
                {
                    objFNBatch = oFNBatch.SaveNote("Update FNBatch SET Note = '" + oFNBatch.Note + "' WHERE FNBatchID =" + oFNBatch.FNBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                objFNBatch = new FNBatch();
                objFNBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objFNBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }




        

        [HttpPost]
        public JsonResult Delete(FNBatch oFNBatch)
        {
            try
            {
                if (oFNBatch.FNBatchID <= 0) { throw new Exception("Please select an valid item."); }
                oFNBatch = oFNBatch.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatch.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult DeleteOrder(FabricSCReport oFabricSCReport)
        //{
        //    try
        //    {
        //        if (oFabricSCReport.fab <= 0) { throw new Exception("Please select an valid item."); }
        //        oFabricSCReport = oFabricSCReport.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        oFabricSCReport = new FabricSCReport();
        //        oFabricSCReport.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFabricSCReport.ErrorMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult Get(FNBatch oFNBatch)
        {
            try
            {
                if (oFNBatch.FNBatchID <= 0) { throw new Exception("Please select an valid item."); }
                oFNBatch = FNBatch.Get(oFNBatch.FNBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFabricSaleReport(int nId)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            try
            {
                if (nId <= 0) { throw new Exception("Please select an valid item."); }
                oFabricSCReport = oFabricSCReport.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSCReport = new FabricSCReport();
                oFabricSCReport.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(FNBatch oFNBatch)
        {
            _oFNBatchs = new List<FNBatch>();
            try
            {
                string sBatchNo = oFNBatch.Params.Split('~')[0];
                bool bIsDateSearch = Convert.ToBoolean(oFNBatch.Params.Split('~')[1]);
                DateTime dtIssueFrom =  Convert.ToDateTime(oFNBatch.Params.Split('~')[2]);
                DateTime dtIssueTo = Convert.ToDateTime(oFNBatch.Params.Split('~')[3]);
                string sFNExeNo = oFNBatch.Params.Split('~')[4];

                string sSQL = "Select * from View_FNBatch Where FNBatchID <> 0 ";
                if (!string.IsNullOrEmpty(sBatchNo))
                    sSQL += " And BatchNo Like '%" + sBatchNo.Trim() + "%'";
                if (bIsDateSearch)
                    sSQL += " And IssueDate Between '" + dtIssueFrom.ToString("dd MMM yyyy") + "' And '" + dtIssueTo.ToString("dd MMM yyyy") + "'";
                if (!string.IsNullOrEmpty(sFNExeNo))
                    sSQL += " And FNExONo Like '%" + sFNExeNo.Trim() + "%'";

                _oFNBatchs = FNBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFNBatchs = new List<FNBatch>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByFNExONo(FabricSCReport oFNEO)
        {
            List<FabricSCReport> oFNEOs = new List<FabricSCReport>();
            try
            {
                if (string.IsNullOrEmpty(oFNEO.ExeNo)) { throw new Exception("Please give execution no to search."); }
                string sSQL = "SELECT TOP 250 * FROM View_FabricSalesContractReport WHERE  ExeNo LIKE '%" + oFNEO.ExeNo.Trim() + "%'";
                oFNEOs = FabricSCReport.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNEO = new FabricSCReport();
                oFNEO.ErrorMessage = ex.Message;
                oFNEOs.Add(oFNEO);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNEOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetsFNBatchOut(FNBatch oFNBatch)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            try
            {
                //// temporay 
                //string sSQL = "Select * from View_FNBatch Where OutQty>0 And FNBatchID Not In (Select FNBatchID from FNBatchQC)";
                string sSQL = "Select * from View_FNBatch Where FNBatchID Not In (Select FNBatchID from FNBatchQC)";
                if (!string.IsNullOrEmpty(oFNBatch.BatchNo))
                    sSQL += " And BatchNo Like '%" + oFNBatch.BatchNo.Trim() + "%'";
                if (!string.IsNullOrEmpty(oFNBatch.FNExONo))
                    sSQL += " And FNExONo Like '%" + oFNBatch.FNExONo.Trim() + "%'";

                oFNBatchs = FNBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchs = new List<FNBatch>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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

        #endregion

        #region ADV Search
        [HttpPost]
        public JsonResult AdvanchSearch_FSC(FabricSCReport oFabricSCReport)
        {
            List<FabricSCReport>  oFabricSCReports = new List<FabricSCReport>();
            try
            {
                string sSQL = MakeSQL(oFabricSCReport);
                oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSCReports = new List<FabricSCReport>();
                oFabricSCReports.Add(new FabricSCReport() { ErrorMessage = ex.Message });
            }
            var jsonResult = Json(oFabricSCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricSCReport oFabricSCReport)
        {
            string sTemp = oFabricSCReport.ErrorMessage;
            string sReturn1 = "";
            string sReturn = "";
            if (!string.IsNullOrEmpty(sTemp) && sTemp != null)
            {
                sReturn1 = "SELECT * FROM View_FabricSalesContractReport";

                #region Set Values

                int nCount = 0;
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[nCount++]);
                string sBuyerIDs = Convert.ToString(sTemp.Split('~')[nCount++]);

                int nCboSCDate = Convert.ToInt32(sTemp.Split('~')[nCount++]);
                DateTime dFromSCDate = DateTime.Now;
                DateTime dToSCDate = DateTime.Now;
                if (nCboSCDate > 0)
                {
                    dFromSCDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                    dToSCDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                }
                else { nCount += 2; }
                int ncboApproveDate = Convert.ToInt32(sTemp.Split('~')[nCount++]);
                DateTime dFromApproveDate = DateTime.Now;
                DateTime dToApproveDate = DateTime.Now;
                if (ncboApproveDate > 0)
                {
                    dFromApproveDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                    dToApproveDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                }
                else { nCount += 2; }

                int nBUID = 0;
                Int32.TryParse(sTemp.Split('~')[nCount++], out nBUID);
                oFabricSCReport.SCNoFull = sTemp.Split('~')[nCount++];

                bool IsYetToBatch = false;
                bool.TryParse(sTemp.Split('~')[nCount++], out IsYetToBatch);

                int ncboReceivedDate = 0;
                Int32.TryParse(sTemp.Split('~')[nCount++], out ncboReceivedDate);

                DateTime dFromReceivedDate = DateTime.Now;
                DateTime dToReceivedDate = DateTime.Now;
                if (ncboReceivedDate > 0)
                {
                    dFromReceivedDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                    dToReceivedDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                }
                else { nCount += 2; }

                int ncboExcDate = 0;
                Int32.TryParse(sTemp.Split('~')[nCount++], out ncboExcDate);

                DateTime dFromExcDate = DateTime.Now;
                DateTime dToExcDate = DateTime.Now;
                if (ncboExcDate > 0)
                {
                    dFromExcDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                    dToExcDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
                }
                else { nCount += 2; }
                #endregion

                #region Make Query
                #region F SC NO
                if (!string.IsNullOrEmpty(oFabricSCReport.SCNoFull))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SCNoFull LIKE '%" + oFabricSCReport.SCNoFull + "%' ";
                }
                #endregion
                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in( " + sContractorIDs + ") ";
                }
                #endregion
                #region BBuyerID
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in( " + sBuyerIDs + ") ";
                }
                #endregion
                #region F SC Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region Approval Date
                if (ncboApproveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboApproveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region ReceivedDate
                if (ncboReceivedDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboReceivedDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region ncboExcDate
                if (ncboExcDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboExcDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                }
                #endregion

                /*
                  SELECT FNExOID, AVG(ExeQty) AS ExeQty, SUM(Qty) AS FN_Qty FROM View_FNBatch WRERE GROUP BY FNExOID 
                  SELECT * FROM View_FNBatch WHERE FNExOID IN (SELECT A.FNExOID FROM (SELECT FNExOID, AVG(ExeQty) AS ExeQty, SUM(Qty) AS FN_Qty FROM View_FNBatch GROUP BY FNExOID) A WHERE (A.ExeQty-A.FN_Qty)>0)
                */

                #region Yet Submit To HO
                if (IsYetToBatch)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FabricSalesContractID IN ((SELECT A.FNExOID FROM (SELECT FNExOID, AVG(ExeQty) AS ExeQty, SUM(Qty) AS FN_Qty FROM View_FNBatch GROUP BY FNExOID) A WHERE (A.ExeQty-A.FN_Qty)>0))";
                }
                #endregion
                #endregion

                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }
            }
            else
            {
                sReturn1 = "SELECT top(150)* FROM View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ";
                //_oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            sReturn = sReturn1 + sReturn + " order by SCDate DESC, Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
            return sReturn;

        }

        #endregion

        #region Print

        public ActionResult PrintFNBatchCard(int nId, double nts)
        {
            FNBatch oFNBatch = new FNBatch();
            oFNBatch = FNBatch.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFNBatchCard oReport = new rptFNBatchCard();
            byte[] abytes = oReport.PrepareReport(oFNBatch, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintFNBatchCardDetail(double nts)
        {
            rptFNBatchCardDetail oReport = new rptFNBatchCardDetail();
            byte[] abytes = oReport.PrepareReport();
            return File(abytes, "application/pdf");
        }

        public ActionResult QCFaultPreview(int id, int nBUID)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            List<FNBatchQCFault> oFNBatchQCFaults = new List<FNBatchQCFault>();
            List<FabricProductionFault> oFabricProductionFaults = new List<FabricProductionFault>();
            Company oCompany = new Company();            
            if (id > 0)
            {
                oFNBatchQC = FNBatchQC.Get(id, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "Select * from FNBatchQCDetail Where FNBatchQCID =" + id + " AND FNBatchQCDetailID IN (SELECT FNBatchQCDetailID FROM FNBatchQCFault)";
                oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = string.Join(",", oFNBatchQCDetails.Select(s => s.FNBatchQCDetailID));
                oFNBatchQCFaults = FNBatchQCFault.Gets("SELECT * FROM View_FNBatchQCFault WHERE FNBatchQCDetailID IN (" + sSQL + ") order by FNBatchQCDetailID,Sequence", ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT * FROM FabricProductionFault WHERE IsActive=1 AND ISNULL(BUType,0) IN (" + (int)EnumBusinessUnitType.None + "," + (int)EnumBusinessUnitType.Finishing + ") Order By Sequence";
                oFabricProductionFaults = FabricProductionFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }

            byte[] abytes;
            rptQCFaultPreview oReport = new rptQCFaultPreview();
            abytes = oReport.PrepareReport(oFNBatchQC, oFNBatchQCDetails, oFNBatchQCFaults, oFabricProductionFaults, oCompany);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region Get Fabric Yet Out Qty
        //This block of omited temporary
        //[HttpPost]
        //public JsonResult GetFabricYetOutQty(FNExecutionOrderFabricReceive oFNEOFR)
        //{
        //    Dictionary<string, Double> FNBatchOut = new Dictionary<string, double>();
        //    FNBatchOut["YetToOutQty"] = 0;
        //    try
        //    {
        //        List<FNExecutionOrderFabricReceive> oFNEOFRs = new List<FNExecutionOrderFabricReceive>();
        //        string sSQL = "Select * from View_FNExecutionOrderFabricReceive Where FNExOID=" + oFNEOFR.FNExOID + " And ReceiveBy<>0";
        //        oFNEOFRs = FNExecutionOrderFabricReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        sSQL = "Select * from View_FNBatchRawMaterial Where FNExOID In (Select FNBatchID from FNBatch Where FNExOID=" + oFNEOFR.FNExOID + ")";

        //        List<FNBatchRawMaterial> oFNBatchRawMaterials = new List<FNBatchRawMaterial>();
        //        oFNBatchRawMaterials = FNBatchRawMaterial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        double nReceiveQty = oFNEOFRs.Sum(x => x.Qty);
        //        double nOutQty = oFNBatchRawMaterials.Sum(x => x.Qty);

        //        FNBatchOut["YetToOutQty"] = nReceiveQty - nOutQty;

        //    }
        //    catch (Exception ex)
        //    {
        //        FNBatchOut["YetToOutQty"] = 0;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(FNBatchOut);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
       
        
        #endregion

        #region Yarn Out

        [HttpPost]
        public JsonResult GetsLotByFNBatch(FNBatch oFNBatch)
        {
            List<FNBatchRawMaterial> oFNBatchRawMaterials = new List<FNBatchRawMaterial>();
            List<FNOrderFabricReceive> _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            try
            {
                int nWUID = 0;
                int.TryParse(oFNBatch.Params, out nWUID);

                List<Lot> oLots = new List<Lot>();
                //string sSQL = "Select * from View_Lot AS HH Where Balance>0 And LotID In (Select LotID from FNOrderFabricReceive Where LotID>0 And  WUID=" + nWUID + "" +
                //             " And FSCDID = (Select FNExOID from FNBatch Where FNBatchID=" + oFNBatch.FNBatchID + "))";
               // string sSQL = "SELECT *,(SELECT Balance FROM LOT WHERE LotID=FNOrderFabricReceive.LotID AND FSCDID = FNOrderFabricReceive.FSCDID) AS LotBalance FROM FNOrderFabricReceive WHERE WUID= " + nWUID + " AND FSCDID = (Select FNExOID from FNBatch Where FNBatchID=" + oFNBatch.FNBatchID + ")";

                string sSQL = "SELECT *,(HH.Qty+HH.QtyTrIn-HH.QtyTrOut-HH.QtyReturn-HH.QtyCon) AS LotBalance  FROM FNOrderFabricReceive AS HH WHERE WUID= " + nWUID + " AND FSCDID = (Select FNExOID from FNBatch Where FNBatchID=" + oFNBatch.FNBatchID + ")";

                _oFNOrderFabricReceives = FNOrderFabricReceive.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
                //oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oFNOrderFabricReceives.Any() && _oFNOrderFabricReceives.FirstOrDefault().FNOrderFabricReceiveID > 0)
                {
                    FNBatchRawMaterial oFNBRM = new FNBatchRawMaterial();
                    _oFNOrderFabricReceives.ForEach(x =>
                    {
                        oFNBRM = new FNBatchRawMaterial();
                        oFNBRM.FNBatchID = oFNBatch.FNBatchID;
                        oFNBRM.FNOrderFabricReceiveID = x.FNOrderFabricReceiveID;
                        oFNBRM.LotID = x.LotID;
                        oFNBRM.LotNo = x.LotNo;
                        oFNBRM.Balance = x.LotBalance;
                        oFNBRM.TotalRcvQty = x.Qty;
                        oFNBRM.QtyTrIn = x.QtyTrIn;
                        oFNBRM.QtyTrOut = x.QtyTrOut;
                        oFNBRM.QtyCon = x.QtyCon;
                        oFNBRM.QtyReturn = x.QtyReturn;
                        //oFNBRM.Balance = x.YetQty;
                        oFNBRM.Qty = 0;
                        oFNBRM.MeasurementUnitName = "Yrd";
                        oFNBatchRawMaterials.Add(oFNBRM);
                    });
                }

            }
            catch (Exception ex)
            {
                oFNBatchRawMaterials = new List<FNBatchRawMaterial>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchRawMaterials);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FabricOut(FNBatchRawMaterial oFNBatchRawMaterial)
        {
            try
            {
                if (!oFNBatchRawMaterial.FNBatchRawMaterials.Any())
                    throw new Exception("No lot found for yarn out.");

                oFNBatchRawMaterial = oFNBatchRawMaterial.FabricOut(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchRawMaterial = new FNBatchRawMaterial();
                oFNBatchRawMaterial.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchRawMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FN Batch QC
        public ActionResult ViewFNBatchQCs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FNBatchQC> oFNBatchQCs = new List<FNBatchQC>();
            string sSQL = "Select * from View_FNBatchQC where FNBatchID in (Select DD.FNBatchID from View_FNBatch as DD where DD.FNBatchStatus in (" + (int)EnumFNBatchStatus.InQC +"" +(int)EnumFNBatchStatus.PartiallyInDeliveryStore + "))";

            oFNBatchQCs = FNBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.buid = buid;
            return View(oFNBatchQCs);
        }
        public ActionResult ViewFNBatchQC(int nId, double ts)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            List<FNBatchQCFault> oFNBatchQCFaults = new List<FNBatchQCFault>();
            string sSQL = "";
            if (nId > 0)
            {
                oFNBatchQC = FNBatchQC.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from FNBatchQCDetail Where FNBatchQCID=" + oFNBatchQC.FNBatchQCID + "";
                oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFNBatchQCDetails.Any() && oFNBatchQCDetails.FirstOrDefault().FNBatchQCDetailID > 0)
                {
                    sSQL = "Select * from View_FNBatchQCFault Where FNBatchQCDetailID In (" + string.Join(",", oFNBatchQCDetails.Select(x => x.FNBatchQCDetailID).ToList()) + ") order by Sequence";
                    oFNBatchQCFaults = FNBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            ViewBag.FNBatchQCDetails = oFNBatchQCDetails;
            ViewBag.FNBatchQCFaults = oFNBatchQCFaults;


            sSQL = "SELECT * FROM FabricProductionFault WHERE IsActive=1  AND ISNULL(BUType,0) IN ("+  (int)EnumBusinessUnitType.None + "," + (int)EnumBusinessUnitType.Finishing +") Order By Sequence";
            ViewBag.Faults = FabricProductionFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = Employee.Gets(EnumEmployeeDesignationType.Incharge, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Grades = EnumObject.jGets(typeof(EnumFBQCGrade));
            //ViewBag.Grades = FBQCGradeObj.Gets();
            ViewBag.EnumFNShades = Enum.GetValues(typeof(EnumFNShade)).Cast<EnumFNShade>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            
            return View(oFNBatchQC);
        }
        public ActionResult ViewFNBatchQCs_New(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FNBatchQC> oFNBatchQCs = new List<FNBatchQC>();
            string sSQL = "Select * from View_FNBatchQC where FNBatchID in (Select DD.FNBatchID from View_FNBatch as DD where DD.FNBatchStatus in (" + (int)EnumFNBatchStatus.InQC +"" +(int)EnumFNBatchStatus.PartiallyInDeliveryStore + "))";
            oFNBatchQCs = FNBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.buid = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(oFNBatchQCs);
        }
        public ActionResult ViewFNBatchQC_New(int nId, double ts)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            List<FNBatchQCFault> oFNBatchQCFaults = new List<FNBatchQCFault>();
            string sSQL = "";
            if (nId > 0)
            {
                oFNBatchQC = FNBatchQC.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from FNBatchQCDetail Where FNBatchQCID=" + oFNBatchQC.FNBatchQCID + "";
                oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFNBatchQCDetails.Any() && oFNBatchQCDetails.FirstOrDefault().FNBatchQCDetailID > 0)
                {
                    sSQL = "Select * from View_FNBatchQCFault Where FNBatchQCDetailID In (" + string.Join(",", oFNBatchQCDetails.Select(x => x.FNBatchQCDetailID).ToList()) + ") ORDER BY FNBatchQCDetailID,Sequence";
                    oFNBatchQCFaults = FNBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            //oFNBatchQC.FNBatchQCDetails = oFNBatchQCDetails;
            //ViewBag.FNBatchQCDetails = oFNBatchQCDetails;
            ViewBag.FNBatchQCFaults = oFNBatchQCFaults;

            sSQL = "SELECT * FROM FabricProductionFault WHERE IsActive=1 AND ISNULL(BUType,0) IN (" + (int)EnumBusinessUnitType.None + "," + (int)EnumBusinessUnitType.Finishing + ") Order By Sequence";
            ViewBag.Faults = FabricProductionFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = Employee.Gets(EnumEmployeeDesignationType.Incharge, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Grades = EnumObject.jGets(typeof(EnumFBQCGrade));
            //ViewBag.Grades = FBQCGradeObj.Gets();
            ViewBag.EnumFNShades = Enum.GetValues(typeof(EnumFNShade)).Cast<EnumFNShade>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(oFNBatchQC);
        }
        [HttpPost]
        public JsonResult GetsNBatchQCDetails(FNBatchQC oFNBatchQC)
        {
            _oFNBatchQCDetails = new List<FNBatchQCDetail>();
            try
            {
                if (oFNBatchQC.FNBatchQCID <= 0) { throw new Exception("Please select a valid contractor."); }

                _oFNBatchQCDetails = FNBatchQCDetail.Gets("Select * from FNBatchQCDetail Where FNBatchQCID=" + oFNBatchQC.FNBatchQCID + "", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFNBatchQC.ErrorMessage = ex.Message;
            }
            var jsonResult = Json(_oFNBatchQCDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ViewFaultEntry(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sSQL = "SELECT * FROM FabricProductionFault WHERE IsActive=1 AND ISNULL(BUType,0) IN (" + (int)EnumBusinessUnitType.None + "," + (int)EnumBusinessUnitType.Finishing + ") Order By Sequence";
            ViewBag.Faults = FabricProductionFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            FNBatchQC oFNBatchQC = new FNBatchQC();
            ViewBag.buid = buid;
            return View(oFNBatchQC);
        }

        [HttpPost]
        public JsonResult FNBatchQCSave(FNBatchQC oFNBatchQC)
        {
            try
            {
                if (oFNBatchQC.FNBatchID <= 0)
                {
                    oFNBatchQC = oFNBatchQC.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNBatchQC = oFNBatchQC.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNBatchQC = new FNBatchQC();
                oFNBatchQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void PrintExcelList(string sParam, int nBUID)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            oFNBatchQC.Params = sParam;
            List<FNBatchQC> oFNBatchQCs = new List<FNBatchQC>();
            DateTime dtIssueFrom = DateTime.Now;
            DateTime dtIssueTo = DateTime.Now;
            try
            {
                string sFNBatchNo = oFNBatchQC.Params.Split('~')[0];
                string sFNDispoNo = oFNBatchQC.Params.Split('~')[1];
                int nDateSearch = Convert.ToInt32(oFNBatchQC.Params.Split('~')[2]);
                dtIssueFrom = Convert.ToDateTime(oFNBatchQC.Params.Split('~')[3]);
                dtIssueTo = Convert.ToDateTime(oFNBatchQC.Params.Split('~')[4]);
                var sContractorIDs = oFNBatchQC.Params.Split('~')[5];
                var sMktPersonIDs = oFNBatchQC.Params.Split('~')[6];

                string sSQL = "Select TOP 150 * from View_FNBatchQC Where FNBatchQCID <> 0 ";
                if (!string.IsNullOrEmpty(sFNBatchNo))
                    sSQL += " And FNBatchNo Like '%" + sFNBatchNo.Trim() + "%'";
                if (!string.IsNullOrEmpty(sFNDispoNo))
                    sSQL += " And FNExONo Like '%" + sFNDispoNo.Trim() + "%'";
                if (nDateSearch > 0)
                    DateObject.CompareDateQuery(ref sSQL, "StartTime", nDateSearch, dtIssueFrom, dtIssueTo);
                if (!string.IsNullOrEmpty(sContractorIDs))
                    sSQL += " And BuyerID IN (" + sContractorIDs + ")";
                if (!string.IsNullOrEmpty(sMktPersonIDs))
                    sSQL += " And FNExOID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE FabricSalesContractID IN (" + sMktPersonIDs + ")))";
                oFNBatchQCs = FNBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {

            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "BatchNo", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Status", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Customer", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Batch Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Out Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "QC Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Count", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "EPI*PPI", Width = 40f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Finish Type", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol -2;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "QC Inspection Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("QC Inspection Report");
                sheet.Name = "QC Inspection Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
   
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "QC Inspection Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date: (" + dtIssueFrom.ToString("dd MMM yyyy") +" to "+ dtIssueTo.ToString("dd MMM yyyy")+ ")"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;

                double dBatchQty = 0;
                double dOut = 0;
                double dQCQty = 0;

                foreach (FNBatchQC obj in oFNBatchQCs)
                {
                    nStartCol = 2;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false, ExcelHorizontalAlignment.Right, false,false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FNBatchNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FNBatchStatusStr, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BatchQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.OutQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.CountName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Construction.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FinishTypeName.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                    nRowIndex++;

                    dBatchQty = dBatchQty + obj.BatchQty;
                    dOut = dOut + obj.OutQty;
                    dQCQty = dQCQty + obj.Qty;
                }
                #endregion
                #region Total
                nStartCol = 2;

                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "Total:", false, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, dBatchQty.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, dOut.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, dQCQty.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);

                #endregion
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FDCRegister Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        [HttpPost]
        public JsonResult FNBatchQCDelete(FNBatchQC oFNBatchQC)
        {
            try
            {
                if (oFNBatchQC.FNBatchQCID <= 0) { throw new Exception("Please select an valid item."); }
                oFNBatchQC = oFNBatchQC.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchQC = new FNBatchQC();
                oFNBatchQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQC.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult FNBatchQCDetailSave(FNBatchQCDetail oFNBatchQCDetail)
        {
            try
            {
                if (oFNBatchQCDetail.FNBatchQCDetailID <= 0)
                {
                    oFNBatchQCDetail = oFNBatchQCDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNBatchQCDetail = oFNBatchQCDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FNBatchQCDetailSave_Multiple(FNBatchQCDetail oFNBatchQCDetail)
        {
            _oFNBatchQC = new FNBatchQC();
            _oFNBatchQCDetails = new List<FNBatchQCDetail>();
            try
            {
                if (oFNBatchQCDetail.FNBatchQCDetails.Any())
                {
                    _oFNBatchQC = oFNBatchQCDetail.FNBatchQC;
                    foreach (FNBatchQCDetail oFNBQCD in oFNBatchQCDetail.FNBatchQCDetails) 
                    {
                        oFNBQCD.FNBatchQC = _oFNBatchQC;
                        oFNBQCD.FNBatchQCID = _oFNBatchQC.FNBatchQCID;
                        _oFNBatchQCDetail = new FNBatchQCDetail();
                        if (oFNBQCD.FNBatchQCDetailID <= 0)
                        {
                            _oFNBatchQCDetail = oFNBQCD.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        else
                        {
                            _oFNBatchQCDetail = oFNBQCD.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }

                        if(_oFNBatchQCDetail.FNBatchQC.FNBatchID>0)
                            _oFNBatchQC = _oFNBatchQCDetail.FNBatchQC;

                        _oFNBatchQCDetails.Add(_oFNBatchQCDetail);
                    }
                }
                oFNBatchQCDetail.FNBatchQCDetails = _oFNBatchQCDetails;
            }
            catch (Exception ex)
            {
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FNBatchQCDetailDelete(FNBatchQCDetail oFNBatchQCDetail)
        {
            try
            {
                if (oFNBatchQCDetail.FNBatchQCDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oFNBatchQCDetail = oFNBatchQCDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveFNBatchQCFault(FNBatchQCFault oFNBatchQCFault)
        {
            try
            {
                if (oFNBatchQCFault.FNBQCFaultID <= 0)
                {
                    oFNBatchQCFault = oFNBatchQCFault.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNBatchQCFault = oFNBatchQCFault.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oFNBatchQCFault = new FNBatchQCFault();
                oFNBatchQCFault.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCFault);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveMultipleFNBatchQCFault(List<FNBatchQCFault> oFNBatchQCFaults)
        {
            List<FNBatchQCFault> _oFNBatchQCFaults = new List<FNBatchQCFault>();
            try
            {
                if (oFNBatchQCFaults.Count > 0)
                {
                    _oFNBatchQCFaults = FNBatchQCFault.SaveMultipleFNBatchQCFault(oFNBatchQCFaults, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                FNBatchQCFault oFNBatchQCFault = new FNBatchQCFault();
                oFNBatchQCFault.ErrorMessage = ex.Message;
                _oFNBatchQCFaults.Add(oFNBatchQCFault);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNBatchQCFaults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFNBatchQCFault(FNBatchQCFault oFNBatchQCFault)
        {
            try
            {
                if (oFNBatchQCFault.FNBQCFaultID <= 0) { throw new Exception("Please select an valid item."); }
                oFNBatchQCFault = oFNBatchQCFault.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchQCFault = new FNBatchQCFault();
                oFNBatchQCFault.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCFault.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LockFNBatchQCDetail(FNBatchQCDetail oFNBatchQCDetail)
        {
            try
            {
                if (string.IsNullOrEmpty(oFNBatchQCDetail.FNBatchQCDetailIDs))
                    throw new Exception("Please select at least one item.");

               oFNBatchQCDetail = oFNBatchQCDetail.LockFNBatchQCDetail(((User)Session[SessionInfo.CurrentUser]).UserID);


            }
            catch (Exception ex)
            {
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FNBatchQCDone(FNBatchQC oFNBatchQC)
        {
            try
            {
                if (oFNBatchQC.FNBatchQCID<=0)
                    throw new Exception("Invalid Batch QC");

                oFNBatchQC = oFNBatchQC.FNBatchQCDone(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchQC = new FNBatchQC();
                oFNBatchQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       [HttpPost]
       public JsonResult GetsFNBatchQC(FNBatchQC oFNBatchQC)
       {
           List<FNBatchQC> oFNBatchQCs = new List<FNBatchQC>();
           try
           {
               string sFNBatchNo = oFNBatchQC.Params.Split('~')[0];
               string sFNDispoNo = oFNBatchQC.Params.Split('~')[1];
               int nDateSearch = Convert.ToInt32(oFNBatchQC.Params.Split('~')[2]);
               DateTime dtIssueFrom = Convert.ToDateTime(oFNBatchQC.Params.Split('~')[3]);
               DateTime dtIssueTo = Convert.ToDateTime(oFNBatchQC.Params.Split('~')[4]);
               var sContractorIDs = oFNBatchQC.Params.Split('~')[5];
               var sMktPersonIDs = oFNBatchQC.Params.Split('~')[6];

               string sSQL = "Select TOP 150 * from View_FNBatchQC Where FNBatchQCID <> 0 ";
               if (!string.IsNullOrEmpty(sFNBatchNo))
                   sSQL += " And FNBatchNo Like '%" + sFNBatchNo.Trim() + "%'";
               if (!string.IsNullOrEmpty(sFNDispoNo))
                   sSQL += " And FNExONo Like '%" + sFNDispoNo.Trim() + "%'";
               if (nDateSearch > 0)
                   DateObject.CompareDateQuery(ref sSQL, "StartTime", nDateSearch, dtIssueFrom, dtIssueTo);
               if (!string.IsNullOrEmpty(sContractorIDs))
                   sSQL += " And BuyerID IN (" + sContractorIDs + ")";
               if (!string.IsNullOrEmpty(sMktPersonIDs))
                   sSQL += " And FNExOID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE FabricSalesContractID IN (" + sMktPersonIDs + ")))";

               oFNBatchQCs = FNBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           }
           catch (Exception ex)
           {
               oFNBatchQCs = new List<FNBatchQC>();
           }

           JavaScriptSerializer serializer = new JavaScriptSerializer();
           string sjson = serializer.Serialize(oFNBatchQCs);
           return Json(sjson, JsonRequestBehavior.AllowGet);
       }


       [HttpPost]
       public JsonResult GetLastFNBatchQCDetail(FNBatchQC oFNBatchQC)
       {
           FNBatchQCDetail oFNBQCD = new FNBatchQCDetail();
           try
           {
               string sSQL = "Select top(1)* from FNBatchQCDetail Where FNBatchQCID In (Select FNBatchQCID from FNBatchQC Where FNBatchID="+oFNBatchQC.FNBatchID+") Order BY FNBatchQCDetailID DESC";
               var oFNBQCDs = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               if (oFNBQCDs != null && oFNBQCDs.Any())
                   oFNBQCD = oFNBQCDs.FirstOrDefault();
           }
           catch (Exception ex)
           {
               oFNBQCD = new FNBatchQCDetail();
           }

           JavaScriptSerializer serializer = new JavaScriptSerializer();
           string sjson = serializer.Serialize(oFNBQCD);
           return Json(sjson, JsonRequestBehavior.AllowGet);
       }

       [HttpPost]
       public JsonResult GetsFNBatchQCDetail(FNBatchQC oFNBatchQC)
       {
           List<FNBatchQCDetail> oFNBQCDs = new List<FNBatchQCDetail>();
           try
           {
               string sSQL = "Select * from FNBatchQCDetail Where FNBatchQCID =" + oFNBatchQC.FNBatchQCID;
               oFNBQCDs = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           }
           catch (Exception ex)
           {
               oFNBQCDs = new List<FNBatchQCDetail>();
           }

           JavaScriptSerializer serializer = new JavaScriptSerializer();
           string sjson = serializer.Serialize(oFNBQCDs);
           return Json(sjson, JsonRequestBehavior.AllowGet);
       }
        
        #endregion

        #region Fn Batch QC Recceive
        #region Fabric Receive
       public ActionResult ViewFNBatchQCReceive(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FNBatchQC> oFNBatchQCs= new List<FNBatchQC>();
            string sSQL = "Select * from View_FNBatchQC Where FNBatchStatus In (" + (int)EnumFNBatchStatus.InQC + "," + (int)EnumFNBatchStatus.QcDone + "," + (int)EnumFNBatchStatus.PartiallyInDeliveryStore + ") And CountYetNotRecv>0";
            oFNBatchQCs = FNBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Stores = WorkingUnit.Gets("FNBatchQC", (int)EnumTriggerParentsType._FNProduction, (int)EnumOperationFunctionality._Receive, (int)EnumInOutType._Receive, false, 0, 0,  ((User)Session[SessionInfo.CurrentUser]).UserID);
            //List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            //oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DUDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricDeliveryChallan, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            return View(oFNBatchQCs);
        }

        [HttpPost]
        public JsonResult GetFNBatchQCDetails(FNBatchQCDetail oFNBatchQCDetail)//FBQCID
        {
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            try
            {
                if (oFNBatchQCDetail.FNBatchQCID > 0)
                {
                    string sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID  = " + oFNBatchQCDetail.FNBatchQCID + " AND IsLock = 1";
                    oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                }
                else
                {
                    throw new Exception("Please select a valid FN Batch QC");
                }
            }
            catch (Exception ex)
            {
                oFNBatchQCDetails = new List<FNBatchQCDetail>();
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
                oFNBatchQCDetails.Add(oFNBatchQCDetail);
            }

            var jsonResult = Json(oFNBatchQCDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFNBatchQCDetails);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintStockLedger(int nID)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            List<FNBatchQC> oFNBatchQCs = new List<FNBatchQC>();
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            FabricDeliveryChallanDetail oFabricDeliveryChallanDetail = new FabricDeliveryChallanDetail();
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            List<FabricReturnChallanDetail> oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();

            if (nID > 0)
            {
                oFNBatchQC = FNBatchQC.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = oFabricSCReport.Get(oFNBatchQC.FNExOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNBatchQCDetails = FNBatchQCDetail.Gets("SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID IN(SELECT FNBatchQCID FROM View_FNBatchQC WHERE FNExOID = " + oFabricSCReport.FabricSalesContractDetailID + ") order by StoreRcvDate Asc", ((User)Session[SessionInfo.CurrentUser]).UserID);
               // oFDCRegisters = FDCRegister.Gets("SELECT * from View_FabricDeliveryChallanDetail WHERE FSCDID = " + oFabricSCReport.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFDCRegisters = FDCRegister.Gets_FDC("where  FSCDID = " + oFabricSCReport.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNBatchDetail er = new rptFNBatchDetail();
            rptStockLedger oReport = new rptStockLedger();
            byte[] abytes = oReport.PrepareReport( oFNBatchQCDetails, oFabricSCReport, oFDCRegisters,oFabricReturnChallanDetails, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        
        [HttpPost]
        public JsonResult GetFNBatchQCFalults(FNBatchQCDetail oFNBatchQCDetail)//FBQCID
        {
            FNBatchQCFault oFNBatchQCFault = new FNBatchQCFault();
            List<FNBatchQCFault> oFNBatchQCFaults = new List<FNBatchQCFault>();
            try
            {
                if (oFNBatchQCDetail.FNBatchQCID > 0)
                {
                    string sSQL = "SELECT * FROM View_FNBatchQCFault AS HH WHERE HH.FNBatchQCDetailID = " + oFNBatchQCDetail.FNBatchQCDetailID + " order by Sequence";
                    oFNBatchQCFaults = FNBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                }
                else
                {
                    throw new Exception("Please select a valid FN Batch QC");
                }
            }
            catch (Exception ex)
            {
                oFNBatchQCFaults = new List<FNBatchQCFault>();
                oFNBatchQCFault = new FNBatchQCFault();
                oFNBatchQCFault.ErrorMessage = ex.Message;
                oFNBatchQCFaults.Add(oFNBatchQCFault);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCFaults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReceiveFNBatchQC(FNBatchQCDetail oFNBatchQCDetail)
        {
            try
            {
                if (!string.IsNullOrEmpty(oFNBatchQCDetail.FNBatchQCDetailIDs))
                {
                    string sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCDetailID IN (" + oFNBatchQCDetail.FNBatchQCDetailIDs + ")";
                    oFNBatchQCDetail.FNBatchQCDetails = new List<FNBatchQCDetail>();
                    oFNBatchQCDetail.FNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oFNBatchQCDetail = oFNBatchQCDetail.ReceiveInDelivery(((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFNBatchQCDetail.FNBatchQCDetails.Any() && oFNBatchQCDetail.FNBatchQCDetails.FirstOrDefault().FNBatchQCID > 0)
                {
                    oFNBatchQCDetail.FNBatchQC = FNBatchQC.Get(oFNBatchQCDetail.FNBatchQCDetails.FirstOrDefault().FNBatchQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReceiveFNBatchQCNew(FNBatchQCDetail oFNBatchQCDetail)
        {
            try
            {
                oFNBatchQCDetail = oFNBatchQCDetail.ReceiveInDeliveryNew(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNBatchQCDetail.FNBatchQCDetails.Any() && oFNBatchQCDetail.FNBatchQCDetails.FirstOrDefault().FNBatchQCID > 0)
                {
                    oFNBatchQCDetail.FNBatchQC = FNBatchQC.Get(oFNBatchQCDetail.FNBatchQCDetails.FirstOrDefault().FNBatchQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcessQtyUpdate(List<FNBatchQCDetail> oFNBatchQCDetails)
        {
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            _oFNBatchQCDetails = new List<FNBatchQCDetail>();
            try
            {
                _oFNBatchQCDetails = FNBatchQCDetail.ExcessQtyUpdate(oFNBatchQCDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = ex.Message;
                _oFNBatchQCDetails = new List<FNBatchQCDetail>();
                _oFNBatchQCDetails.Add(oFNBatchQCDetail);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNBatchQCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
        #endregion

        #region FN Inspection Sticker
        public ActionResult PrintFNInspectionSticker(int nId, double nts)
        {
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            var oFNBatchQCDetail = FNBatchQCDetail.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oFNBatchQCDetail != null && oFNBatchQCDetail.FNBatchQCDetailID > 0)
                oFNBatchQCDetails.Add(oFNBatchQCDetail);


            FNBatchQC oFNBatchQC = new FNBatchQC();
            oFNBatchQC = FNBatchQC.Get(oFNBatchQCDetail.FNBatchQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFNInspectionSticker oReport = new rptFNInspectionSticker();
            byte[] abytes = oReport.PrepareReport(oFNBatchQC, oFNBatchQCDetails, oCompany, false);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFNInspectionStickerA4(string sId, double nts)
        {
            string sSQL = "Select * from FNBatchQCDetail Where FNBatchQCDetailID In (" + sId + ")";
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            if (!string.IsNullOrEmpty(sId))
                oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
 
            FNBatchQC oFNBatchQC = new FNBatchQC();
            if (oFNBatchQCDetails.Any() && oFNBatchQCDetails.FirstOrDefault().FNBatchQCDetailID>0)
                oFNBatchQC = FNBatchQC.Get(oFNBatchQCDetails.FirstOrDefault().FNBatchQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFNInspectionSticker oReport = new rptFNInspectionSticker();
            byte[] abytes = oReport.PrepareReport(oFNBatchQC, oFNBatchQCDetails, oCompany, true);
            return File(abytes, "application/pdf");
        }
        #endregion
        
        #region FN Packing Transfer
        public ActionResult PrintFNPackingTransfer(string sFNQCDID, int nFNBatchID, int buid, bool IsMeter, double nts)
        {
            string sSQL = "";
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<FNBatchQCFault> oFNBatchQCFaults = new List<FNBatchQCFault>();
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            if (!string.IsNullOrEmpty(sFNQCDID))
            {
                sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCDetailID In (" + sFNQCDID + ")";
            }
            else
            {
                sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID In (" + nFNBatchID + ")";
            }
            oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FNBatchQC oFNBatchQC = new FNBatchQC();
            if (oFNBatchQCDetails.Any() && oFNBatchQCDetails.First().FNBatchQCDetailID > 0)
            {
                oFNBatchQC = FNBatchQC.Get(oFNBatchQCDetails.First().FNBatchQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = " SELECT * FROM View_FNBatchQCFault Where FNBatchQCDetailID In (" + string.Join(",", oFNBatchQCDetails.Select(x => x.FNBatchQCDetailID).ToList()) + ")  order by Sequence";
                oFNBatchQCFaults = FNBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FNBatch + " AND BUID = " + buid + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oBusinessUnit = oBusinessUnit.Get(buid,((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (IsMeter)
            {
                oFNBatchQCDetails.ForEach(o => o.Qty = Global.GetMeter(o.Qty, 4));
            }

            rptFNPackingTransfer oReport = new rptFNPackingTransfer();

            byte[] abytes = oReport.PrepareReport(oFNBatchQC, oFNBatchQCDetails, oFNBatchQCFaults, oCompany, oBusinessUnit, oApprovalHeads, IsMeter);

            return File(abytes, "application/pdf");
        }
        public ActionResult PrintFNPackingTransferNew(string sFNQCDID, int nFNBatchID, int buid, bool IsMeter, double nts)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<FNBatchQCFault> oFNBatchQCFaults = new List<FNBatchQCFault>();
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            string sSQL = "";
            if (!string.IsNullOrEmpty(sFNQCDID))
            {
                sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCDetailID In (" + sFNQCDID + ")";
            }
            else
            {
                sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID In (" + nFNBatchID + ")";
            }

          //  string sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCDetailID In (" + sFNQCDID + ") ORDER BY CONVERT(INT,LotNo) ASC";
            oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FNBatchQC oFNBatchQC = new FNBatchQC();
            if (oFNBatchQCDetails.Any() && oFNBatchQCDetails.First().FNBatchQCDetailID > 0)
            {
                oFNBatchQC = FNBatchQC.Get(oFNBatchQCDetails.First().FNBatchQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = " SELECT * FROM View_FNBatchQCFault Where FNBatchQCDetailID In (" + string.Join(",", oFNBatchQCDetails.Select(x => x.FNBatchQCDetailID).ToList()) + ")  order by Sequence";
                oFNBatchQCFaults = FNBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FNBatch + " AND BUID = " + buid + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (IsMeter)
            {
                oFNBatchQCDetails.ForEach(o => o.Qty = Global.GetMeter(o.Qty, 4));
            }
            rptFNPackingTransferNew oReport = new rptFNPackingTransferNew();

            byte[] abytes = oReport.PrepareReport(oFNBatchQC, oFNBatchQCDetails, oFNBatchQCFaults, oCompany, oBusinessUnit, oApprovalHeads, IsMeter);

            return File(abytes, "application/pdf");
        }
        #endregion

        #region Production Report
        [HttpPost]
        public ActionResult SetFNBatchData(FNBatchQCDetail oFNBatchQCDetail)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFNBatchQCDetail);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintProductionReport()
        {
            List<FNBatchQC> _oFNBatchQCs = new List<FNBatchQC>();
            List<FNBatchQCDetail> _oFNBatchQCDetails = new List<FNBatchQCDetail>();
            //_oFNBatchQC = new FNBatchQC();
            _oFNBatchQCDetail = new FNBatchQCDetail();
            try
            {
                _oFNBatchQCDetail = (FNBatchQCDetail)Session[SessionInfo.ParamObj];

                string sFNBatchNo = _oFNBatchQCDetail.ErrorMessage.Split('~')[0];
                string sFNDispoNo = _oFNBatchQCDetail.ErrorMessage.Split('~')[1];
                int nDateSearch = Convert.ToInt32(_oFNBatchQCDetail.ErrorMessage.Split('~')[2]);
                DateTime dtIssueFrom = Convert.ToDateTime(_oFNBatchQCDetail.ErrorMessage.Split('~')[3]);
                DateTime dtIssueTo = Convert.ToDateTime(_oFNBatchQCDetail.ErrorMessage.Split('~')[4]);
                var sContractorIDs = _oFNBatchQCDetail.ErrorMessage.Split('~')[5];
                var sMktPersonIDs = _oFNBatchQCDetail.ErrorMessage.Split('~')[6];

                string sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID <> 0 ";
                if (!string.IsNullOrEmpty(sFNBatchNo))
                    sSQL += " And FNBatchQCID IN (SELECT FNBatchQCID FROM View_FNBatchQC WHERE FNBatchNo LIKE '%" + sFNBatchNo + "%')";
                if (!string.IsNullOrEmpty(sFNDispoNo))
                    sSQL += " And FNBatchQCID IN (SELECT FNBatchQCID FROM View_FNBatchQC WHERE FNExONo LIKE '%" + sFNBatchNo + "%')";
                if (nDateSearch > 0)
                    DateObject.CompareDateQuery(ref sSQL, "DBServerDate", nDateSearch, dtIssueFrom, dtIssueTo);
                if (!string.IsNullOrEmpty(sContractorIDs))
                    sSQL += " And FNBatchQCID IN (SELECT FNBatchQCID FROM View_FNBatchQC WHERE BuyerID IN (" + sContractorIDs + "))";
                if (!string.IsNullOrEmpty(sMktPersonIDs))
                    sSQL += " And FNBatchQCID IN (SELECT FNBatchQCID FROM View_FNBatchQC WHERE FNExOID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE FabricSalesContractID IN (" + sMktPersonIDs + "))))";

                //string sSQL = "SELECT * FROM FNBatchQCDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + _oFNBatchQCDetail.StoreRcvDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + _oFNBatchQCDetail.DBServerDate.ToString("dd MMM yyyy") + "', 106))";
                //_oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //sSQL = "SELECT * FROM View_FNBatchQC WHERE FNBatchQCID IN (SELECT FNBatchQCID FROM FNBatchQCDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + _oFNBatchQCDetail.StoreRcvDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + _oFNBatchQCDetail.DBServerDate.ToString("dd MMM yyyy") + "', 106))) Order By FNBatchQCID";
                //_oFNBatchQCs = FNBatchQC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                _oFNBatchQCDetails = FNBatchQCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_FNBatchQC WHERE FNBatchQCID IN (" + string.Join(",", _oFNBatchQCDetails.Select(x=>x.FNBatchQCID)) + ") Order By FNBatchQCID";
                _oFNBatchQCs = FNBatchQC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNBatchQC = new FNBatchQC();
                _oFNBatchQCs = new List<FNBatchQC>();
            }

            if (_oFNBatchQCs.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFNBatchQCDetail.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_oFNBatchQCDetail.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
                string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FNBatch + " AND BUID = " + _oFNBatchQCDetail.BUID + "  Order By Sequence";
                oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                rptFNBatchQC_ProductionReport oReport = new rptFNBatchQC_ProductionReport();
                byte[] abytes = oReport.PrepareReport(_oFNBatchQCs, _oFNBatchQCDetail, _oFNBatchQCDetails, oCompany, oApprovalHeads);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
            
        }

        #endregion

        #region Create FN Batch QC With Detail From FNBatch Batch

        public ActionResult PrintFNBatchQCWithProcessSticker(string sParams, double nts)
        {
            int nFNBatchID = 0;
            string sRollNo = "";
            int nRollCountStart = 0;
            int nQCLotItem = 0;

            int.TryParse(sParams.Split('~')[0], out nFNBatchID);
            sRollNo=(string.IsNullOrEmpty(sParams.Split('~')[1]))?"":sParams.Split('~')[1];
            int.TryParse(sParams.Split('~')[2], out nRollCountStart);
            int.TryParse(sParams.Split('~')[3], out nQCLotItem);

            FNBatchQC oFNBatchQC = new FNBatchQC();
            oFNBatchQC = oFNBatchQC.SaveProcess(nFNBatchID, sRollNo, nRollCountStart, nQCLotItem, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FNBatchQCDetail> oFNBQCDs = new List<FNBatchQCDetail>();
            if (oFNBatchQC.FNBatchQCID > 0)
            {
                string sSQL = "Select top(" + nQCLotItem + ")* from FNBatchQCDetail Where FNBatchQCID=" + oFNBatchQC.FNBatchQCID + " Order By FNBatchQCDetailID DESC";

                oFNBQCDs = FNBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNBQCDs == null && !oFNBQCDs.Any())
                    oFNBQCDs = new List<FNBatchQCDetail>();
                else
                    oFNBQCDs = oFNBQCDs.OrderBy(x => x.FNBatchQCDetailID).ToList();
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFNInspectionSticker oReport = new rptFNInspectionSticker();
            byte[] abytes = oReport.PrepareReport(oFNBatchQC, oFNBQCDs, oCompany, false);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region FNBATCH : Production Status
        //Production Status Update
        public ActionResult ViewFNBatch_PS(int buid, int menuid, int treatment)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oFNBatch = new FNBatch();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();

            //if (nId > 0)
            //{
            //    string sSQL = "Select * from View_FabricSalesContractReport Where FabricSalesContractDetailID=" + nId;
            //    oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    oFabricSCReport = oFabricSCReports[0];
            //    _oFNBatchs = FNBatch.Gets("Select * from View_FNBatch Where FNExOID = " + nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}

            //List<WorkingUnit> oWUs = new List<WorkingUnit>();
            //oWUs = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricSalesContract, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            ViewBag.FNMachineProcessList = FNMachine.Gets("Select * from View_FNMachine where FNMachineType=1", (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FabricSCReport = oFabricSCReport;
            ViewBag.FNBatchs = _oFNBatchs;
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment)); ;
            ViewBag.Treatment = treatment;
            ViewBag.BUID = buid;
            return View(_oFNBatch);
        }

    


        [HttpPost]
        public JsonResult GetsFNBatch(FNBatch oFNBatch)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            try
            {
                string sSQL = "Select * from View_FNBatch Where FNExOID = " + oFNBatch.FNExOID;
                if (!string.IsNullOrEmpty(oFNBatch.BatchNo))
                    sSQL += " And FNBatchNo Like '%" + oFNBatch.BatchNo.Trim() + "%'";

                oFNBatchs = FNBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchs = new List<FNBatch>();
                oFNBatchs.Add(new FNBatch() {ErrorMessage=ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFNBatchCard(FNBatchCard oFNBatchCard)
        {
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            try
            {
                string sSQL = "SELECT * FROM View_FNBatchCard WHERE FNBatchID=" + oFNBatchCard.FNBatchID;

                if(oFNBatchCard.FNTreatmentProcessID > 0)
                    sSQL += " AND FNTreatmentProcessID IN (SELECT FNTPID FROM FNTreatmentProcess WHERE FNTreatment=" + oFNBatchCard.FNTreatmentProcessID + ")";

                oFNBatchCards = FNBatchCard.Gets(sSQL + "  ORDER BY SequenceNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchCards = new List<FNBatchCard>();
                oFNBatchCards.Add(new FNBatchCard() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchCards);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsFNBatchCardByNo(FNBatchCard oFNBatchCard)
        {
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            try
            {
                string sSQL = "SELECT TOP 250 * FROM View_FNBatchCard WHERE FNBatchID > 0";

                if (!string.IsNullOrEmpty(oFNBatchCard.FNBatchNo))
                    sSQL += " AND FNBatchNo LIKE '%" + oFNBatchCard.FNBatchNo + "%'";
                if (oFNBatchCard.FNTreatmentProcessID > 0)
                    sSQL += " AND FNTreatmentProcessID IN (" + oFNBatchCard.FNTreatmentProcessID + ")";
                if (oFNBatchCard.FNTreatment > 0)
                    sSQL += " AND FNTreatment =" + (int)oFNBatchCard.FNTreatment;

                oFNBatchCards = FNBatchCard.Gets(sSQL + " AND (ISNULL(Qty_FNPBatch,0) - ISNULL(Qty_Prod,0))>0 ORDER BY PlannedDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchCards = new List<FNBatchCard>();
                oFNBatchCards.Add(new FNBatchCard() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchCards);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFNProductionBatch(FNProductionBatch oFNProductionBatch)
        {
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
            try
            {
                string sSQL = "SELECT * FROM View_FNProductionBatch WHERE FNPBatchID =" + oFNProductionBatch.FNPBatchID + " AND ISNULL(QCStatus,0) NOT IN (" + (int)EnumQCStatus.Reproduction + ")";
                oFNProductionBatchs = FNProductionBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_FNRecipe WHERE FNTPID =" + oFNProductionBatch.FNTPID + " AND FSCDID IN (SELECT FNExOID FROM FNBatch WHERE FNBatchID = " + oFNProductionBatch.FNBatchID + ") ";
                oFNRecipes = FNRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFNProductionBatchs.Count() > 0)
                {
                    oFNProductionBatchs[0].FNProductionConsumptions = Gets_FNPConsumption(oFNRecipes.Where(x => !x.IsProcess).ToList(), new FNRecipe()
                    {
                        FNTPID = oFNProductionBatch.FNTPID,
                        FSCDID = oFNProductionBatch.FNBatchID,
                        FNPBatchID = oFNProductionBatch.FNPBatchID
                    });
                }
                else { oFNProductionBatchs.Add(new FNProductionBatch() { FNPBatchID=0 }); }

                oFNProductionBatchs[0].FNMachines = FNMachine.Gets("SELECT * FROM View_FNMachine WHERE FNMachineID IN (SELECT FNMachineID FROM View_FNMachineProcess WHERE FNTPID = " + oFNProductionBatch.FNTPID + ")", (int)Session[SessionInfo.currentUserID]);
                
                var oProcess = oFNRecipes.Where(x => x.IsProcess).FirstOrDefault();

                if (oProcess != null)
                {
                    double nValue = 0;
                    Double.TryParse(oProcess.PadderPressure, out nValue); oFNProductionBatchs[0].Pressure_Bar_Actual = nValue; nValue = 0;
                    Double.TryParse(oProcess.PH, out nValue); oFNProductionBatchs[0].PH_Actual = nValue; nValue = 0;
                    Double.TryParse(oProcess.Temp, out nValue); oFNProductionBatchs[0].Temp_C_Actual = nValue; nValue = 0;
                    Double.TryParse(oProcess.Speed, out nValue); oFNProductionBatchs[0].MachineSpeed_Actual = nValue; nValue = 0;
                    Double.TryParse(oProcess.CausticStrength, out nValue); oFNProductionBatchs[0].CausticStrength_Actual = nValue; nValue = 0;
                    Double.TryParse(oProcess.Flem, out nValue); oFNProductionBatchs[0].FlameIntensity_Actual = nValue; nValue = 0;
                }
            }
            catch (Exception ex)
            {
                oFNProductionBatchs = new List<FNProductionBatch>();
                oFNProductionBatchs.Add(new FNProductionBatch() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<FNProductionConsumption> Gets_FNPConsumption(List<FNRecipe> oFNRecipes, FNRecipe oFNRecipe)
        {
            List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
            List<FNProductionConsumption> oFNProductionConsumptions = new List<FNProductionConsumption>();
            try
            {
                if (oFNRecipe.FNTPID <= 0) return oFNProductionConsumptions;
                if (oFNRecipe.FSCDID <= 0) return oFNProductionConsumptions;

                oFNProductionConsumptions = FNProductionConsumption.Gets("SELECT * FROM View_FNProductionConsumption WHERE FNPBatchID<>0 AND FNPBatchID=" + oFNRecipe.FNPBatchID , ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFNProductionConsumptions.Count() == 0)
                {
                    string sSQL = "SELECT * FROM View_FNRequisitionDetail WHERE FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(ReceiveBy,0)<>0 AND TreatmentID=" + oFNRecipe.FNTreatment + " AND FNExODetailID IN (SELECT FNExOID FROM FNBatch WHERE FNBatchID = " + oFNRecipe.FSCDID + "))"; //FSCDID as FNBatchID
                    oFNRequisitionDetails = FNRequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oFNRecipes.ForEach(oFNR =>
                       oFNProductionConsumptions.Add(new FNProductionConsumption
                       {
                           ProductID = oFNR.ProductID,
                           ProductName = oFNR.ProductName,
                           ProductCode = oFNR.ProductCode,
                           FNPBatchID = oFNRecipe.FNPBatchID,
                           Qty = oFNR.Qty,
                           LotID = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.LotID).FirstOrDefault(),
                           LotNo = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.LotNo).FirstOrDefault(),
                           LotBalance = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.LotBalance).FirstOrDefault(),
                           MUID = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.MeasurementUnitID).FirstOrDefault(),
                           MUName = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.MUName).FirstOrDefault()
                       }));
                }
            }
            catch (Exception ex)
            {
                oFNProductionConsumptions = new List<FNProductionConsumption>();
                oFNProductionConsumptions.Add(new FNProductionConsumption() { ErrorMessage = ex.Message });
            }
            return oFNProductionConsumptions;
        }
        [HttpPost]
        public JsonResult GetsFNProductionConsumption(FNRecipe oFNRecipe)
        {
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
            List<FNProductionConsumption> oFNProductionConsumptions = new List<FNProductionConsumption>();
            try
            {
                if (oFNRecipe.FNTPID <= 0) throw new Exception("Invalid Process!");
                if (oFNRecipe.FSCDID <= 0) throw new Exception("Invalid Order/Dispo!");

                oFNProductionConsumptions = FNProductionConsumption.Gets("SELECT * FROM View_FNProductionConsumption WHERE FNPBatchID<>0 AND FNPBatchID=" + oFNRecipe.FNPBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFNProductionConsumptions.Count() == 0)
                {
                    string sSQL = "SELECT * FROM View_FNRecipe WHERE FNTPID =" + oFNRecipe.FNTPID + " AND IsProcess=0 AND FSCDID= " + oFNRecipe.FSCDID;
                    oFNRecipes = FNRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "SELECT * FROM View_FNRequisitionDetail WHERE FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(ReceiveBy,0)<>0 AND TreatmentID=" + oFNRecipe.FNTreatment + " AND FNExODetailID=" + oFNRecipe.FSCDID + ")";
                    oFNRequisitionDetails = FNRequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oFNRecipes.ForEach(oFNR =>
                       oFNProductionConsumptions.Add(new FNProductionConsumption
                       {
                           ProductID = oFNR.ProductID,
                           ProductName = oFNR.ProductName,
                           ProductCode = oFNR.ProductCode,
                           FNPBatchID = oFNRecipe.FNPBatchID,
                           Qty = oFNR.Qty,
                           LotID = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.DestinationLotID).FirstOrDefault(),
                           FNRDetailID = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.FNRDetailID).FirstOrDefault(),
                           LotNo = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.DestinationLotNo).FirstOrDefault(),
                           LotBalance = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.DestinationLotBalance).FirstOrDefault(),
                           MUID = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.MeasurementUnitID).FirstOrDefault(),
                           MUName = oFNRequisitionDetails.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.MUName).FirstOrDefault()
                       }));
                }
            }
            catch (Exception ex)
            {
                oFNRecipes = new List<FNRecipe>();
                oFNRecipes.Add(new FNRecipe() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionConsumptions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProducts(Lot oLot)
        {
            List<Product> oProducts = new List<Product>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
         
            try
            {
                string sSql = string.Empty;
                oWorkingUnits = WorkingUnit.GetsPermittedStore(oLot.BUID, EnumModuleName.FNRequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
                sSql = "SELECT * FROM View_Product AS HH WHERE HH.Activity = 1 AND  ProductID IN (SELECT ProductID FROM Lot WHERE Balance >0"
                    + ((oWorkingUnits.Count > 0) ? " AND WorkingUnitID IN (" + string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID)) + ")" : "") + ")";
                
                if (!string.IsNullOrEmpty(oLot.ProductName))
                {
                    sSql = sSql + "and HH.ProductName LIKE '%" + oLot.ProductName + "%'";
                }
                
                sSql = sSql + " ORDER BY HH.ProductName";
                oProducts = Product.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                Product oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }

            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetLots(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();

            try
            {
                string sSql = string.Empty;
                oWorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FNProduction, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //sSql = "SELECT * FROM "
                //+ "(SELECT FNRD.FNRDetailID, FNRD.FNRNo, FNRD.ProductID, FNRD.ProductCode, FNRD.ProductName, FNRD.DestinationLotID, FNRD.DestinationLotID as LotID,FNRD.LotNo, FNRD.MeasurementUnitID AS MUID, FNRD.MUName, "
                //+ "FNRD.DisburseQty, FNRD.DisburseQty - ISNULL((SELECT SUM(ISNULL(FNPC.Qty,0)) FROM FNProductionConsumption AS FNPC WHERE FNPC.FNRDetailID = FNRD.FNRDetailID),0) AS LotBalance "
                //+ "FROM View_FNRequisitionDetail AS FNRD ) HH WHERE ISNULL(HH.LotBalance,0) > 0";

                //sSql = "Select sum(DisburseQty) as DisburseQty "
                    //+",(sum(DisburseQty)-isnull((SELECT SUM(ISNULL(Qty,0)) FROM FNProductionConsumption where LotID=FNRD.DestinationLotID and FNPBatchID in   (Select FNPBatchID from FNProductionBatch where FNPBatchID in (  Select FNBatchID from FNBatch where FNExOID=FNR.FNExODetailID))),0)) as Qty"
                    //+",FNRD.DestinationLotID as LotID,FNRD.ProductID,WorkingUnitReceiveID as WorkingUnitID"
                    //+",Product.ProductCode,Product.ProductName,Lot.LotNo,MU.Symbol as MUName, Lot.Balance   from FNRequisitionDetail as FNRD "
                    //+"left join FNRequisition as FNR on FNRD.FNRID=FNR.FNRID  left join Product on Product.ProductID=FNRD.ProductID"
                    //+" left join Lot on Lot.LotID=FNRD.DestinationLotID   left join MeasurementUnit as MU on MU.MeasurementUnitID=Lot.MUnitID   where FNR.FNExODetailID=1712 group by FNR.FNExODetailID, FNRD.LotID,DestinationLotID,FNRD.ProductID,FNR.WorkingUnitID ,WorkingUnitReceiveID,ProductCode,ProductName,LotNo,MU.Symbol,Lot.Balance";
                    //+ ((oWorkingUnits.Count > 0) ? " AND HH.WorkingUnitID IN (" + string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID)) + ")" : "");

                sSql = "SELECT * FROM View_Lot AS HH WHERE HH.Balance >0";
                   
                if (!string.IsNullOrEmpty(oLot.LotNo))
                {
                    sSql = sSql + " AND HH.LotNo LIKE '%" + oLot.LotNo + "%'";
                }
                if (!string.IsNullOrEmpty(oLot.Params))
                {
                    sSql = sSql + " AND HH.LotID IN (SELECT FNRD.DestinationLotID FROM View_FNRequisitionDetail AS FNRD WHERE FNRD.FNExODetailID IN (SELECT FSCD.FabricSalesContractDetailID FROM FabricSalesContractDetail FSCD WHERE FSCD.FabricSalesContractDetailID = " + oLot.Params + "))";
                }
                if (oLot.ProductID > 0)
                {
                    sSql = sSql + " AND HH.ProductID =" + oLot.ProductID;
                }
                // ((oWorkingUnits.Count > 0) ? " AND HH.WorkingUnitID IN (" + string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID)) + ")" : "");
                sSql += " AND HH.WorkingUnitID in ("+ string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID)) +") AND ISNULL(HH.Balance,0) > 0 ";

                sSql = sSql + " ORDER BY HH.LotNo";
                oLots = Lot.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLots.Add(new Lot() { ErrorMessage = ex.Message });
            }

            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Print Function
        public ActionResult Print_FNPProcess(int nFabricID, int nBUID, double nts)
        {
            Fabric oFabric = new Fabric();
            List<FNBatchCard> oFNBCs = new List<FNBatchCard>();
            List<FNProductionBatch> oFNPBs = new List<FNProductionBatch>();
            List<FNProductionConsumption> oFNPCs = new List<FNProductionConsumption>();
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();

            oFabric = oFabric.Get(nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFabric.FabricID > 0)
            {
                string sSQL = "Select * from View_FabricSalesContractDetail Where FabricID=" + oFabric.FabricID + " Order By FabricNo";
                oFSCDs = FabricSalesContractDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNBatchCard ==== */
                if (oFSCDs != null && oFSCDs.Any())
                    oFNBCs = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE ISNULL(QCStatus,0)IN (" + (int)EnumQCStatus.Yet_To_QC + "," + (int)EnumQCStatus.In_QC+") AND  FNBatchID IN (SELECT FNBatch.FNBatchID FROM FNBatch WHERE FNBatch.FNExOID IN (" + string.Join(",", oFSCDs.Select(x => x.FabricSalesContractDetailID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionBatch ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFNPBs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE FNBatchCardID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchCardID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionConsumption ==== */
                if (oFNPBs != null && oFNPBs.Any())
                    oFNPCs = FNProductionConsumption.Gets("SELECT * FROM View_FNProductionConsumption WHERE FNPBatchID IN (" + string.Join(",", oFNPBs.Select(x => x.FNPBatchID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNProductionBatch oReport = new rptFNProductionBatch();
            byte[] abytes = oReport.PrepareReport(oFabric, oFSCDs, oFNBCs, oFNPBs, oFNPCs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_FNP_TreatmentWise(int buid, double nts, int treatment, string sDateCriteria)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            List<FNBatchCard> oFNBCs = new List<FNBatchCard>();
            List<FNProductionBatch> oFNPBs = new List<FNProductionBatch>();
            List<FNProductionConsumption> oFNPCs = new List<FNProductionConsumption>();
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();

            //string sSQL = "SELECT * FROM View_FNBatchCard WHERE ISNULL(QCStatus,0) IN (" + (int)EnumQCStatus.Yet_To_QC + "," + (int)EnumQCStatus.In_QC+")"; 
            //"FNBatchCardID IN (SELECT FNBatchCardID  FROM FNProductionBatch WHERE FNPBatchID NOT IN (SELECT FNPBatchID FROM FNProductionBatchQuality WHERE ISNULL(IsOk,0) = 1)) ";

            string sSQL = "SELECT FNBC.* FROM View_FNBatchCard AS FNBC WHERE "
                        + "FNBatchID	 NOT IN (SELECT FQC.FNBatchID FROM FNBatchQCStatus FQC WHERE FQC.FNBatchCardID= FNBC.FNBatchCardID) AND  "
                        + "FNTreatment  NOT IN (SELECT FQC.FNTreatment FROM FNBatchQCStatus FQC WHERE FQC.FNBatchCardID=  FNBC.FNBatchCardID) ";

            if (treatment > 0)
                sSQL += " AND FNTreatmentProcessID IN (SELECT FNTPID FROM FNTreatmentProcess WHERE FNTreatment=" + treatment + ")";

            if (!string.IsNullOrEmpty(sDateCriteria)) 
            {
                DateObject.CompareDateQuery(ref sSQL, "PlannedDate", sDateCriteria);
               
                //int nCboDate = Convert.ToInt16(sDateCriteria.Value.Split('~')[0]);
                //DateTime Startdate = Convert.ToDateTime(sDateCriteria.Value.Split('~')[1]);
                //DateTime Enddate = Convert.ToDateTime(sDateCriteria.Value.Split('~')[2]);

                ////SELECT PlannedDate, FNBatchCardID FROM FNBatchCard
                //if(nCboDate>0)
                //{
                //    string sDateQuery = "";
                //    DateObject.CompareDateQuery(ref sSQL, "PlannedDate", nCboDate, Startdate, Enddate);
                //    sSQL = sSQL + " FNBatchCardID IN (SELECT FNBatchCardID FROM FNBatchCard " + sDateQuery + ")";
                //}
            }

            /* ==== View_FNBatchCard ==== */
            oFNBCs = FNBatchCard.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //oFNPBs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE ISNULL(QCStatus,0)=" + (int)EnumQCStatus.Yet_To_QC, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFNBCs != null && oFNBCs.Any())
            {
                /* ==== View_FNProductionBatch ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFSCDs = FabricSalesContractDetail.Gets("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractDetailID IN (SELECT FNExOID FROM  FNBatch WHERE FNBatchID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionBatch ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFNPBs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE FNBatchCardID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchCardID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionConsumption ==== */
                if (oFNPBs != null && oFNPBs.Any())
                    oFNPCs = FNProductionConsumption.Gets("SELECT * FROM View_FNProductionConsumption WHERE FNPBatchID IN (" + string.Join(",", oFNPBs.Select(x => x.FNPBatchID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNBatch ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFNBatchs = FNBatch.Gets("SELECT * FROM View_FNBatch WHERE FNBatchID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else 
            {
                rptErrorMessage oReport_Error = new rptErrorMessage();
                byte[] abytes_Error = oReport_Error.PrepareReport("No Data Found!");
                return File(abytes_Error, "application/pdf");
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNProductionBatch_Process oReport = new rptFNProductionBatch_Process();
            byte[] abytes = oReport.PrepareReport(oFSCDs, oFNBCs, oFNPBs, oFNBatchs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintBatch(int buid, int treatment, int nFNBatchID)
        {
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            FNBatch oFNBatch = new FNBatch();
            try
            {
                string sSQL = "SELECT * FROM View_FNBatchCard WHERE FNBatchID=" + nFNBatchID;
                oFNBatch = FNBatch.Get(nFNBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if(treatment > 0)
                {
                    sSQL += " AND FNTreatmentProcessID IN (SELECT FNTPID FROM FNTreatmentProcess WHERE FNTreatment=" + treatment + ")";
                }
                oFNBatchCards = FNBatchCard.Gets(sSQL + "  ORDER BY  Code", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchCards = new List<FNBatchCard>();
                oFNBatchCards.Add(new FNBatchCard() { ErrorMessage = ex.Message });
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid,  ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNBatchDetail oReport = new rptFNBatchDetail();
            byte[] abytes = oReport.PrepareReport(oFNBatch, oFNBatchCards, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public void PrintXL(int buid, int nTreatment,string sDateCriteria)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            List<FNBatchCard> oFNBCs = new List<FNBatchCard>();
            List<FNProductionBatch> oFNPBs = new List<FNProductionBatch>();
            List<FNProductionConsumption> oFNPCs = new List<FNProductionConsumption>();
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();

            string sSQL = "SELECT FNBC.* FROM View_FNBatchCard AS FNBC WHERE "
                        + "FNBatchID	 NOT IN (SELECT FQC.FNBatchID FROM FNBatchQCStatus FQC WHERE FQC.FNBatchCardID= FNBC.FNBatchCardID) AND  "
                        + "FNTreatment  NOT IN (SELECT FQC.FNTreatment FROM FNBatchQCStatus FQC WHERE FQC.FNBatchCardID=  FNBC.FNBatchCardID) ";

            if (nTreatment > 0)
                sSQL += " AND FNTreatmentProcessID IN (SELECT FNTPID FROM FNTreatmentProcess WHERE FNTreatment=" + nTreatment + ")";

             if (!string.IsNullOrEmpty(sDateCriteria)) 
                DateObject.CompareDateQuery(ref sSQL, "PlannedDate", sDateCriteria);
               
            /* ==== View_FNBatchCard ==== */
            oFNBCs = FNBatchCard.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //oFNPBs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE ISNULL(QCStatus,0)=" + (int)EnumQCStatus.Yet_To_QC, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFNBCs != null && oFNBCs.Any())
            {
                /* ==== View_FNProductionBatch ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFSCDs = FabricSalesContractDetail.Gets("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractDetailID IN (SELECT FNExOID FROM  FNBatch WHERE FNBatchID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionBatch ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFNPBs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE FNBatchCardID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchCardID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionConsumption ==== */
                if (oFNPBs != null && oFNPBs.Any())
                    oFNPCs = FNProductionConsumption.Gets("SELECT * FROM View_FNProductionConsumption WHERE FNPBatchID IN (" + string.Join(",", oFNPBs.Select(x => x.FNPBatchID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNBatch OFF==== */
                //if (oFNBCs != null && oFNBCs.Any())
                //    oFNBatchs = FNBatch.Gets("SELECT * FROM View_FNBatch WHERE FNBatchID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //rptFNProductionBatch_Process oReport = new rptFNProductionBatch_Process();
            //byte[] abytes = oReport.PrepareReport(oFSCDs, oFNBCs, oFNPBs, oFNBatchs, oCompany, oBusinessUnit);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            
            table_header.Add(new TableHeader { Header = "", Width = 10f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "", Width = 20f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "", Width = 50f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "", Width = 50f, IsRotate = false, Align = TextAlign.Left });
            for (int i = 0; i < 15; i++ )
            {
                table_header.Add(new TableHeader { Header = "", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            }
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Production Report");
                sheet.Name = "Production Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Production Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Treatment Wise Print
                List<FNBatchCard> oFNBCards_Temp = new List<FNBatchCard>();
                oFNBCards_Temp = oFNBCs.GroupBy(x => new { x.FNTreatment }, (key, grp) => new FNBatchCard
                {
                    FNTreatment = key.FNTreatment,
                    FNBatchCards = grp.ToList()
                }).ToList();
                //ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);

                foreach (var oFNBatchCard in oFNBCards_Temp)
                {
                    var oProcessList = oFNBatchCard.FNBatchCards.Select(x => new
                    {
                        FNTreatmentProcessID = x.FNTreatmentProcessID,
                        FNProcess = x.FNProcess
                    }).Distinct().ToList();
                   

                    #region FN Treatment Header
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oFNBatchCard.FNTreatmentSt; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex++;
                    #endregion

                    #region Header
                    nStartCol = 2;
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "#SL", false, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Dispo No", false, true, ExcelHorizontalAlignment.Left, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Color", false, true, ExcelHorizontalAlignment.Left, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Construction", false, true, ExcelHorizontalAlignment.Left, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Order Qty", false, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Gray Issue Qty", false, true, ExcelHorizontalAlignment.Right, false);

                    foreach (var oItemProcess in oProcessList)
                    {
                        sheet.Column(nStartCol).Width = 20;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItemProcess.FNProcess, false, true, ExcelHorizontalAlignment.Right, false);
                    }
                    sheet.Column(nStartCol).Width = 20;
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "RED", false, true, ExcelHorizontalAlignment.Left, false);
                    sheet.Column(nStartCol).Width = 20;
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Delivered", false, true, ExcelHorizontalAlignment.Left, false);
                    #endregion

                    nRowIndex++;

                    #region Body

                    /* ==== View_FNBatch ==== */
                    if (oFNBCs != null && oFNBCs.Any())
                        oFNBatchs = FNBatch.Gets("SELECT * FROM View_FNBatch WHERE FNBatchID IN (" + string.Join(",", oFNBatchCard.FNBatchCards.Select(x => x.FNBatchID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
           
                    int nCount = 1, nFNExOID = 0;
                    foreach (var oItem_Batch in oFNBatchs)
                    {
                        var oFSCD = oFSCDs.Where(x => x.FabricSalesContractDetailID == oItem_Batch.FNExOID).FirstOrDefault();
                        if (oItem_Batch.FNExOID != nFNExOID)
                        {
                            var oBatch_list = oFNBCs.Where(x => x.FNBatchID == oItem_Batch.FNBatchID).ToList();
                            nStartCol = 2;
                            ExcelTool.Formatter = "";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false, false, ExcelHorizontalAlignment.Right, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oFSCD.ExeNo, false, false, ExcelHorizontalAlignment.Left, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oFSCD.ColorInfo, false, false, ExcelHorizontalAlignment.Left, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oFSCD.Construction, false, false, ExcelHorizontalAlignment.Left, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oFSCD.Qty, 2).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oFSCD.OrderQty, 2).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                            foreach (var oItemProcess in oProcessList)
                            {
                                double nQTY = 0;
                                oBatch_list.ForEach(p =>
                                        nQTY += oBatch_list.Where(x => x.FNBatchCardID == p.FNBatchCardID).Sum(x => x.StartQty)
                                    );
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(nQTY, 2).ToString(), true, false, ExcelHorizontalAlignment.Right, false); sheet.Column(nStartCol).Width = 20;
                            }
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "-", false, true, ExcelHorizontalAlignment.Left, false); sheet.Column(nStartCol).Width = 20;
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "-", false, true, ExcelHorizontalAlignment.Left, false); sheet.Column(nStartCol).Width = 20;
                            nRowIndex++;
                            nFNExOID = oItem_Batch.FNExOID;
                        }
                    }
                    nRowIndex++;

                    #endregion
                }
                #endregion

                #region Last Part
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=WIP Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
                #endregion
            }
            #endregion
        }
        public ActionResult Print_FNPProcess_Consumtion(int nFNBatchID, int nBUID, int treatment, double nts)
        {
            FNBatch oFNBatch = new FNBatch();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FNBatchCard> oFNBCs = new List<FNBatchCard>();
            List<FNProductionConsumption> oFNPCs = new List<FNProductionConsumption>();

            oFNBatch = FNBatch.Get(nFNBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFNBatch.FNBatchID > 0)
            {
                oFabricSCReport = oFabricSCReport.Get(oFNBatch.FNExOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNBatch.Params = oFabricSCReport.PINo;

                /* ==== View_FNBatchCard ==== */
                oFNBCs = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE ISNULL(QCStatus,0) IN (" + (int)EnumQCStatus.QC_Done + "," + (int)EnumQCStatus.QC_Failed + "," + (int)EnumQCStatus.Reproduction + ") AND  FNBatchID IN (" + oFNBatch.FNBatchID + ") ORDER BY FNTreatment, FNTreatmentProcessID", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionConsumption ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFNPCs = FNProductionConsumption.Gets("SELECT * FROM View_FNProductionConsumption WHERE FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE FNBatchCardID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchCardID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNProductionBatchConsumtion oReport = new rptFNProductionBatchConsumtion();
            byte[] abytes = oReport.PrepareReport(oFNBatch, oFNBCs, oFNPCs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult Print_FNProduction_Consumtion(int nID, int nBUID, int treatment, double nts)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();

            List<FNBatch> oFNBatchs = new List<FNBatch>();
            List<FNBatchCard> oFNBCs = new List<FNBatchCard>();
            List<FNProductionConsumption> oFNPCs = new List<FNProductionConsumption>();

            oFabricSCReport = oFabricSCReport.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFabricSCReport.FabricSalesContractDetailID > 0)
            {
                /* ==== FNBatch ==== */
                oFNBatchs = FNBatch.Gets("SELECT * FROM View_FNBatch WHERE FNExOID = " + nID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFNBatchs.Count() > 0) oFNBatchs[0].Params = oFabricSCReport.PINo;

                /* ==== View_FNBatchCard ==== */
                if (oFNBatchs.Any())
                    oFNBCs = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE ISNULL(QCStatus,0) IN (" + (int)EnumQCStatus.QC_Done + "," + (int)EnumQCStatus.QC_Failed + "," + (int)EnumQCStatus.Reproduction + ") AND  FNBatchID IN (" + string.Join(",", oFNBatchs.Select(x => x.FNBatchID)) + ") ORDER BY FNTreatment, FNTreatmentProcessID", ((User)Session[SessionInfo.CurrentUser]).UserID);

                /* ==== View_FNProductionConsumption ==== */
                if (oFNBCs != null && oFNBCs.Any())
                    oFNPCs = FNProductionConsumption.Gets("SELECT FNBatchCardID, LotID, LotNo, ProductID, ProductName, ProductCode, SUM(Qty) AS Qty, AVG(UnitPrice) AS UnitPrice FROM View_FNProductionConsumption WHERE FNBatchCardID IN (" + string.Join(",", oFNBCs.Select(x => x.FNBatchCardID)) + ") GROUP BY FNBatchCardID, LotID, LotNo, ProductID, ProductName, ProductCode", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNProductionBatchConsumtion oReport = new rptFNProductionBatchConsumtion();
            byte[] abytes = oReport.PrepareReport(oFNBatchs, oFNBCs, oFNPCs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult ViewFNBatch_WIPReport(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FNBatch> oFNBatchs = new List<FNBatch>();
            
            ViewBag.buid = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); 
            return View(oFNBatchs);
        }

        public ActionResult PrintRequisitionStatement(int nID, int nBUID, int treatment, double nts)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            FNRequisition oFNRequisition = new FNRequisition();
            List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();

            oFabricSCReport = oFabricSCReport.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFabricSCReport.FabricSalesContractDetailID > 0)
            {
                string sSql = "SELECT * FROM View_FNRequisitionDetail WHERE FNExODetailID = " + oFabricSCReport.FabricSalesContractDetailID;
                if (treatment > 0)
                    sSql += " AND FNRID IN (SELECT FNRID FROM FNRequisition WHERE TreatmentID = 1)";
                oFNRequisitionDetails = FNRequisitionDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNRequisitionDetails.Count > 0)
                {
                    oFNRequisition = oFNRequisition.Get(oFNRequisitionDetails[0].FNRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (nBUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            rptPrintRequisitionStatement oReport = new rptPrintRequisitionStatement();
            byte[] abytes = oReport.PrepareReport(oFabricSCReport, oFNRequisition, oFNRequisitionDetails, oCompany);
            return File(abytes, "application/pdf");
        }

        #endregion

        #endregion
    }
}