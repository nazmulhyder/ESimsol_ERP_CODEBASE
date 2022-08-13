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
    public class SalesCommissionLCController :PdfViewController
    {
        #region Declaration
        SalesCommissionLC _oSalesCommissionLC = new SalesCommissionLC();
        List<SalesCommissionLC> _oSalesCommissionLCs = new List<SalesCommissionLC>();
        #endregion
      
        public ActionResult ViewSalesCommissionLCs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<SalesCommissionLC> oSalesCommissionLCs = new List<SalesCommissionLC>();
            //oSalesCommissionLCs = SalesCommissionLC.Gets("Select * from View_SalesCommissionLC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(oSalesCommissionLCs);
        }

        #region Advance Search
        public ActionResult AdvSearchSCLC()
        {
            return PartialView();
        }
        #endregion
        #region Search PINo
        [HttpPost]
        public JsonResult SearchByPINo(SalesCommissionLC oSalesCommissionLC)
        {
            try
            {
                 _oSalesCommissionLCs = new List<SalesCommissionLC>();
                if (!string.IsNullOrEmpty(oSalesCommissionLC.Params))
                {
                    string sSQL = "Select * from View_SalesCommissionLC Where PINo Like '%"+oSalesCommissionLC.Params.Trim()+"%'";
                    _oSalesCommissionLCs = SalesCommissionLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
              
                oSalesCommissionLC = new SalesCommissionLC();
                oSalesCommissionLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesCommissionLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Sales Commission Distribution and Approval
        public ActionResult ViewSalesComDistribution(int nId, double ts, int buid)
        {
            ExportPI _oExportPI = new ExportPI();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<SalesCommission> oSalesCommissions = new List<SalesCommission>();
            List<Contractor> oContractors = new List<Contractor>();
            Contractor oContractor = new Contractor();
            if (nId > 0)
            {
                _oExportPI = _oExportPI.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportPI.ContractorID>0)
                {
                    oContractor.ContractorID = _oExportPI.ContractorID;
                    oContractor.Name = _oExportPI.ContractorName;
                    oContractors.Add(oContractor);
                }
                if (_oExportPI.BuyerID > 0)
                {
                    oContractor = new Contractor();
                    oContractor.ContractorID = _oExportPI.BuyerID;
                    oContractor.Name = _oExportPI.BuyerName;
                    oContractors.Add(oContractor);
                }
              
                if (_oExportPI.ExportPIID > 0)
                {
                    oExportPIDetails = ExportPIDetail.GetsByPI(_oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ExportPIDetail oItem in  oExportPIDetails)
                    {
                        if(oItem.QtyCom==0)
                        {
                            oItem.QtyCom=(oItem.Qty-oItem.AdjQty);
                            if (_oExportPI.ProductNature == EnumProductNature.Hanger || _oExportPI.ProductNature == EnumProductNature.Poly)
                            {
                                oItem.QtyCom =Math.Round(oItem.QtyCom/12);
                            }
                        }

                    }
                    oSalesCommissions = SalesCommission.Gets(_oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            ViewBag.Contractors = oContractors;
            ViewBag.ExportPIDetails = oExportPIDetails;
            ViewBag.SalesCommissions = oSalesCommissions;

            return View(_oExportPI);
        }
        public ActionResult ViewSalesComReqForApproval(int nId, double ts, int buid)
        {
            string sSql = "";
            ExportPI _oExportPI = new ExportPI();
            ExportBill _oExportBill = new ExportBill();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<SalesCommission> oSalesCommissions = new List<SalesCommission>();
            List<ExportBill> oExportBills = new List<ExportBill>();
            if (nId > 0)
            {
                _oExportPI = _oExportPI.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportPI.ExportPIID > 0)
                {
                    sSql = "Select * from View_ExportPIDetail Where  ExportPIID =  " + _oExportPI.ExportPIID;
                    oExportPIDetails = ExportPIDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oSalesCommissions = SalesCommission.Gets(_oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSql = "select * from view_ExportBill where [State] >=" + (int)EnumLCBillEvent.BankAcceptedBill + " and [State] <>" + (int)EnumLCBillEvent.BillCancel + " and ExportBillID in (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID =" + _oExportPI.ExportPIID + "))";
                    oExportBills = ExportBill.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }

            ViewBag.ExportPIDetails = oExportPIDetails;
            ViewBag.SalesCommissions = oSalesCommissions;
            ViewBag.ExportBills = oExportBills;

            return View(_oExportPI);
        }

        public ActionResult ViewSalesComApproval(int nId, double ts, int buid)
        {
            string sSql = "";
            ExportPI _oExportPI = new ExportPI();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<SalesCommission> oSalesCommissions = new List<SalesCommission>();
            List<ExportBill> oExportBills = new List<ExportBill>();

            if (nId > 0)
            {
                _oExportPI = _oExportPI.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oExportPI.ExportPIID > 0)
                {
                    sSql = "Select * from View_ExportPIDetail Where  ExportPIID =  " + _oExportPI.ExportPIID;
                    oExportPIDetails = ExportPIDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oSalesCommissions = SalesCommission.Gets(_oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSql = "select * from view_ExportBill where [State] >=" + (int)EnumLCBillEvent.BankAcceptedBill + " and [State] <>" + (int)EnumLCBillEvent.BillCancel + " and ExportBillID in (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID =" + _oExportPI.ExportPIID + "))";
                    oExportBills = ExportBill.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                }
            }

            ViewBag.ExportPIDetails = oExportPIDetails;
            ViewBag.SalesCommissions = oSalesCommissions;
            ViewBag.ExportBills = oExportBills;

            return View(_oExportPI);
        }
     
        #endregion
        #region HTTP

        [HttpPost]
        public JsonResult UpdateCRRate(ExportPIDetail oExportPIDetail)
        {
            if (oExportPIDetail.CRate < 0) { throw new Exception("Commission Rate Can not be Negative or Zero !!"); }
            oExportPIDetail = oExportPIDetail.UpdateCRate(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSalesCommissionLCByExportPIID(ExportPI oExportPI)
        {
            _oSalesCommissionLC = new SalesCommissionLC();
            try
            {
                if (oExportPI.ExportPIID <= 0)
                {
                    throw new Exception("Please select an valid item.");
                }
                else
                {
                    _oSalesCommissionLC = _oSalesCommissionLC.GetByExportPIID(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                _oSalesCommissionLC = new SalesCommissionLC();
                _oSalesCommissionLC.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesCommissionLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreatePayable(SalesCommissionLC oSalesCommissionLC)
        {
            _oSalesCommissionLCs = new List<SalesCommissionLC>();
            try
            {
              _oSalesCommissionLCs = oSalesCommissionLC.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oSalesCommissionLC = new SalesCommissionLC();
                oSalesCommissionLC.ErrorMessage = ex.Message;
                _oSalesCommissionLCs = new List<SalesCommissionLC>();
                _oSalesCommissionLCs.Add(oSalesCommissionLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesCommissionLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<SalesCommissionLC> oSalesCommissionLCs = new List<SalesCommissionLC>();
            SalesCommissionLC oSalesCommissionLC = new SalesCommissionLC();
            try
            {
                string sSQL = GetSQL(sTemp);
                oSalesCommissionLCs = SalesCommissionLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {

                oSalesCommissionLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesCommissionLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(string sTemp)
        {
            //PI Date
            int nPIDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtPIDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtPIEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            //LCDate
            int nLCDateCompare = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dtLCStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dtLCEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            //Amendent Date
            int nAmmendentDateCompare = Convert.ToInt32(sTemp.Split('~')[6]);
            DateTime dtAmendentStartDate = Convert.ToDateTime(sTemp.Split('~')[7]);
            DateTime dtAmendentEndDate = Convert.ToDateTime(sTemp.Split('~')[8]);

            string sPINo = sTemp.Split('~')[9];
            string sLCNo = sTemp.Split('~')[10];
            string sBuyerIDs = sTemp.Split('~')[11];
            string sContractorIDs = sTemp.Split('~')[12];

            bool bYettoDistribute = Convert.ToBoolean(sTemp.Split('~')[13]);
            bool byettoPayable = Convert.ToBoolean(sTemp.Split('~')[14]);
            bool byettoPaid = Convert.ToBoolean(sTemp.Split('~')[15]);
            bool bMaturityReceived = Convert.ToBoolean(sTemp.Split('~')[16]);
            bool bPaymentReceived = Convert.ToBoolean(sTemp.Split('~')[17]);
            int BUID = 0;// Convert.ToInt32(sTemp.Split('~')[18]);
          
            if (sTemp.Split('~').Length > 18)
                Int32.TryParse(sTemp.Split('~')[18], out BUID);

            //sPINo = sTemp.Split('~')[18];


            string sReturn1 = "SELECT * FROM View_SalesCommissionLC";
            string sReturn = "";
            #region BUID

            if (BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + BUID;

            }
            #endregion

            #region PI No

            if (sPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PINo LIKE '%" + sPINo + "%'";

            }
            #endregion

            #region LC No
            if (sLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCNo LIKE '%" + sLCNo + "%'";
            }
            #endregion

            #region Buyer wise

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion
            #region Contractor wise

            if (sContractorIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sContractorIDs + ")";
            }
            #endregion

            #region PI Date Wise
            if (nPIDateCompare > 0)
            {
                if (nPIDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PIDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nPIDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PIDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nPIDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PIDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nPIDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PIDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nPIDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PIDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nPIDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PIDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtPIEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region LC Date Wise
            if (nLCDateCompare > 0)
            {
                if (nLCDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Amendent Date Wise
            if (nAmmendentDateCompare > 0)
            {
                if (nAmmendentDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),AmendmentDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nAmmendentDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),AmendmentDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nAmmendentDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),AmendmentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nAmmendentDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),AmendmentDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nAmmendentDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),AmendmentDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nAmmendentDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),AmendmentDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtAmendentEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region YettoDistribute
            if (bYettoDistribute)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Com_PI>0 and Round(Com_Dis,2)<Round(Com_PI,2)";
            }
            #endregion
            #region byettoPayable
            if (byettoPayable)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Com_Dis>0 and Round(Com_Payable,2)<Round(Com_Dis,2)";
            }
            #endregion
            #region byettoPaid
            if (byettoPaid)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Com_Payable>0 and  Round(Com_Paid,2)<Round(Com_Payable,2)";
            }
            #endregion

            #region MaturityReceived
            //   Bill State BillRealized = 10,
            if (bMaturityReceived)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "isnull(Com_Dis,0)>0 and ExportLCID in (Select ExportLCID from ExportBill where [state]>=" + (int)EnumLCBillEvent.BankAcceptedBill + " and ExportBillID in (Select ExportBillID from ExportBillHistory where ExportBillHistory.[State]=" + (int)EnumLCBillEvent.BankAcceptedBill + ") and ExportBillID not in (select ExportBillID from SalesCommissionPayable))";
            }
            #endregion
            #region bPaymentReceived
            //   Bill State BillRealized = 10,
            if (bPaymentReceived)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "isnull(Com_Dis,0)>0 and ExportLCID in (Select ExportLCID from ExportBill where [state]>=" + (int)EnumLCBillEvent.BankAcceptedBill + " and ExportBillID in (Select ExportBillID from ExportBillHistory where ExportBillHistory.[State]=" + (int)EnumLCBillEvent.BillRealized + ") and ExportBillID not in (select ExportBillID from SalesCommissionPayable))";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion
        #region Report
        #region PDF
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
        public ActionResult PrintCommissionMemoReportByPI(int nId, int nType, double nts)
        {
            string sSql = "";
            ExportPI _oExportPI = new ExportPI();
            List<SalesCommission> oSalesCommissions = new List<SalesCommission>();
            List<ExportBill> oExportBills = new List<ExportBill>();
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            List<SalesComPayment> osalesComPayments = new List<SalesComPayment>();
            List<SalesComPaymentDetail> oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
            List<ExportPIReport> oExportPIReports = new List<ExportPIReport>();
            if (nId > 0)
            {
                _oExportPI = _oExportPI.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportPI.ExportPIID > 0)
                {
                    sSql = "Select * from View_ExportPIReport  where ExportPIID=" + _oExportPI.ExportPIID;
                    oExportPIReports = ExportPIReport.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    switch (nType) {

                        case 1: 
                            oSalesCommissions = SalesCommission.Gets(_oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSql = "SELECT * FROM View_SalesCommissionPayable where ExportPIID =" + _oExportPI.ExportPIID;
                            oSalesCommissionPayables = SalesCommissionPayable.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSql = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from SalesCommissionPayable where ExportBillID IN (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID = " + _oExportPI.ExportPIID + "))))";
                            osalesComPayments = SalesComPayment.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSql = "select * from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from SalesCommissionPayable where ExportBillID IN (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID = " + _oExportPI.ExportPIID + ")))";
                            oSalesComPaymentDetails = SalesComPaymentDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            break;
                        case 2:
                            //sSql = "SELECT * FROM View_SalesCommission WHERE ExportPIID=" + _oExportPI.ExportPIID + " AND ContactPersonnelID IN (SELECT ContactPersonnelID FROM ContactPersonnel WHERE ContractorID IN (SELECT ContractorID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + "))";
                            sSql = "SELECT * FROM View_SalesCommission WHERE ExportPIID=" + _oExportPI.ExportPIID + " AND  ContractorID IN (SELECT ContractorID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + ")";
                            oSalesCommissions = SalesCommission.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            //sSql = "SELECT * FROM View_SalesCommissionPayable where ExportPIID =" + _oExportPI.ExportPIID + " AND ContactPersonnelID IN (SELECT ContactPersonnelID FROM ContactPersonnel WHERE ContractorID IN (SELECT ContractorID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + "))";
                            sSql = "SELECT * FROM View_SalesCommissionPayable where ExportPIID =" + _oExportPI.ExportPIID + " AND ContactPersonnelID in (Select ContactPersonnelID from SalesCommission where  ExportPIID=" + _oExportPI.ExportPIID + " and ContractorID in (SELECT ContractorID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + "))";
                            oSalesCommissionPayables = SalesCommissionPayable.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                            sSql = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from View_SalesCommissionPayable where ContractorID IN (SELECT ContractorID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + ") AND ExportBillID IN (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID = " + _oExportPI.ExportPIID + "))))";
                            osalesComPayments = SalesComPayment.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSql = "select * from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from SalesCommissionPayable where ContractorID IN (SELECT ContractorID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + ") AND  ExportBillID IN (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID = " + _oExportPI.ExportPIID + ")))";
                            oSalesComPaymentDetails = SalesComPaymentDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            break;
                        case 3:
                            //sSql = "SELECT * FROM View_SalesCommission WHERE ExportPIID=" + _oExportPI.ExportPIID + " AND ContactPersonnelID IN (SELECT ContactPersonnelID FROM ContactPersonnel WHERE ContractorID IN (SELECT BuyerID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + "))";
                            sSql = "SELECT * FROM View_SalesCommission WHERE ExportPIID=" + _oExportPI.ExportPIID + " AND  ContractorID IN (SELECT BuyerID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + ")";
                            oSalesCommissions = SalesCommission.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            //sSql = "SELECT * FROM View_SalesCommissionPayable where ExportPIID =" + _oExportPI.ExportPIID + " AND ContactPersonnelID IN (SELECT ContactPersonnelID FROM ContactPersonnel WHERE ContractorID IN (SELECT BuyerID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + "))";
                            sSql = "SELECT * FROM View_SalesCommissionPayable where ExportPIID =" + _oExportPI.ExportPIID + " AND ContactPersonnelID in (Select ContactPersonnelID from SalesCommission where  ExportPIID=" + _oExportPI.ExportPIID + " and ContractorID in (SELECT BuyerID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + "))";
                            oSalesCommissionPayables = SalesCommissionPayable.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                            sSql = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from View_SalesCommissionPayable where ContractorID IN (SELECT BuyerID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + ") AND ExportBillID IN (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID = " + _oExportPI.ExportPIID + "))))";
                            osalesComPayments = SalesComPayment.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSql = "select * from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from SalesCommissionPayable where ContractorID IN (SELECT BuyerID FROM ExportPI where ExportPIID=" + _oExportPI.ExportPIID + ") AND  ExportBillID IN (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID = " + _oExportPI.ExportPIID + ")))";
                            oSalesComPaymentDetails = SalesComPaymentDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            break;
                    }

                    sSql = "select * from view_ExportBill where [State] >=" + (int)EnumLCBillEvent.BankAcceptedBill + " and [State] <>" + (int)EnumLCBillEvent.BillCancel + " and ExportBillID in (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select  ExportPIDetailID from ExportPIDetail where ExportPIID =" + _oExportPI.ExportPIID + "))";
                    oExportBills = ExportBill.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);//oExportBills.Select(x=>x.BUID).FirstOrDefault()

            rptCommissionMemoDistribution oReport = new rptCommissionMemoDistribution();
            byte[] abytes = oReport.PrepareReport(_oExportPI, oExportPIReports, oExportBills, oSalesCommissions, oSalesCommissionPayables, osalesComPayments, oSalesComPaymentDetails, oCompany, oBusinessUnit, nType);
            return File(abytes, "application/pdf");
        }
        #endregion
        #region XL
        public void Print_ReportXL(string sTempString, int BUID)
        {

            _oSalesCommissionLCs = new List<SalesCommissionLC>();
            SalesCommissionLC oSalesCommissionLC = new SalesCommissionLC();
            oSalesCommissionLC.ErrorMessage = sTempString;
            string sSQL = GetSQL(sTempString);
            _oSalesCommissionLCs = SalesCommissionLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nLCAmount = 0;
            double nINVValue = 0;
            double nComPI = 0;
            double nComdist = 0;
            double nComPayable = 0;
            double nComPaid = 0;

            double nAmount = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LC Sales Commission ");
                sheet.Name = "Sales Commision";
                sheet.Column(2).Width = 22;
                sheet.Column(3).Width = 12;
                sheet.Column(4).Width = 22;
                sheet.Column(5).Width = 7;
                sheet.Column(6).Width = 12;
                sheet.Column(7).Width = 18;

                //sheet.Column(6).Width = 12;

                sheet.Column(8).Width = 25;
                sheet.Column(9).Width = 25;

                sheet.Column(10).Width = 18;
                sheet.Column(11).Width = 18;
                sheet.Column(12).Width = 18;
                sheet.Column(13).Width = 18;
                sheet.Column(14).Width = 18;



                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Sales Commision LC  "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Version No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 6]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Contractor Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Invoice Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Commision PI"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Commision Distribution"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Commision Payable"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Commision Paid"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion
                string sTemp = "";
                #region Data
                foreach (SalesCommissionLC oItem in _oSalesCommissionLCs)
                {

                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.PIDateStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.VersionNoStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AmendmentDateStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LCOpenDateStr; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#####";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.InvoiceValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Com_PI; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Com_Dis; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.Com_Payable; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.Com_Paid; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nAmount = nAmount + oItem.Amount;
                    nINVValue = nINVValue + oItem.InvoiceValue;
                    nComPI = nComPI + oItem.Com_PI;
                    nComdist = nComdist + oItem.Com_Dis;
                    nComPayable = nComPayable + oItem.Com_Payable;
                    nComPaid = nComPaid + oItem.Com_Paid;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + "Total"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nINVValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nComPI; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nComdist; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nComPayable; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = nComPaid; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalesCommision.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            #endregion

            }



        }
        #endregion
        #endregion
    }
}