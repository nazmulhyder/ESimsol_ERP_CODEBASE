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
    public class SalesCommissionPayableController : PdfViewController
    {
        #region Declaration
        SalesCommissionPayable _oSalesCommissionPayable = new SalesCommissionPayable();
        List<SalesCommissionPayable> _oSalesCommissionPayables = new List<SalesCommissionPayable>();
        #endregion

        #region View
        public ActionResult ViewSalesCommissionPayables(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            oSalesCommissionPayables = SalesCommissionPayable.Gets("Select * from View_SalesCommissionPayable Where BUID=" +buid +" and Status_Payable not in (" + (int)EnumLSalesCommissionStatus.Payable + "," + (int)EnumLSalesCommissionStatus.Cancel + "," + (int)EnumLSalesCommissionStatus.PaidPartially + "," + (int)EnumLSalesCommissionStatus.Paid + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<SalesCommissionPayable> oBillWIse = new List<SalesCommissionPayable>();

            oBillWIse = oSalesCommissionPayables.GroupBy(x => new { x.ContactPersonnelID, x.CPName, x.ContractorName, x.ExportLCID, x.LDBCNo, x.ExportBillID, x.ExportLCNo, x.LCOpeningDate, x.MaturityReceivedDate, x.RelizationDate, x.MaturityDate, x.CRate }, (key, grp) =>
                                           new SalesCommissionPayable
                                           {
                                               CPName = key.CPName,
                                               ContactPersonnelID = key.ContactPersonnelID,
                                               ContractorName = key.ContractorName,
                                               ExportLCNo = key.ExportLCNo,
                                               LCOpeningDate = key.LCOpeningDate,
                                               MaturityDate = key.MaturityDate,
                                               MaturityReceivedDate = key.MaturityReceivedDate,
                                               RelizationDate = key.RelizationDate,
                                               LDBCNo = key.LDBCNo,
                                               CommissionAmount = grp.Sum(p => p.CommissionAmount),
                                               MaturityAmount = grp.Sum(p => p.MaturityAmount),
                                               RealizeAmount = grp.Sum(p => p.RealizeAmount),
                                               AdjAdd = grp.Sum(p => p.AdjAdd),
                                               AdjDeduct = grp.Sum(p => p.AdjDeduct),
                                               AdjPayable = grp.Sum(p => p.AdjPayable),
                                               AdjOverdueAmount = grp.Sum(p => p.AdjOverdueAmount),
                                               ExportBillID = key.ExportBillID,
                                               Amount_Paid = grp.Sum(x => x.Amount_Paid),
                                               CRate = key.CRate,
                                               IsWillVoucherEffect = grp.Select(x => x.IsWillVoucherEffect).FirstOrDefault(),
                                               Status = grp.Select(x => x.Status).FirstOrDefault(),
                                               Status_Payable = grp.Select(x => x.Status_Payable).FirstOrDefault(),
                                               ExportPIID = grp.Select(x => x.ExportPIID).FirstOrDefault()
                                               //Status_Payable = key.Status_Payable
                                           }).ToList();

            List<SalesCommissionPayable> oSCPGroupByContactPersonnelID = new List<SalesCommissionPayable>();

            oSCPGroupByContactPersonnelID = oSalesCommissionPayables.GroupBy(x => new { x.ContactPersonnelID, x.CPName, x.ContractorName, x.CRate }, (key, grp) =>
                                            new SalesCommissionPayable
                                            {
                                                CPName = key.CPName,
                                                ContractorName = key.ContractorName,
                                                CommissionAmount = grp.Sum(p => p.CommissionAmount),
                                                MaturityAmount = grp.Sum(p => p.MaturityAmount),
                                                RealizeAmount = grp.Sum(p => p.RealizeAmount),
                                                AdjAdd = grp.Sum(p => p.AdjAdd),
                                                AdjDeduct = grp.Sum(p => p.AdjDeduct),
                                                AdjPayable = grp.Sum(p => p.AdjPayable),
                                                AdjOverdueAmount = grp.Sum(p => p.AdjOverdueAmount),
                                                Amount_Paid = grp.Sum(x => x.Amount_Paid),
                                                CRate = key.CRate,
                                                Status = grp.Select(x => x.Status).FirstOrDefault(),
                                                Status_Payable = grp.Select(x => x.Status_Payable).FirstOrDefault()

                                            }).ToList();
            ViewBag.PersonWise = oSCPGroupByContactPersonnelID;
            ViewBag.BillWise = oBillWIse;
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.SalesCommissionStatus = EnumObject.jGets(typeof(EnumLSalesCommissionStatus));
            return View(oSalesCommissionPayables);
        }
        #region View For Approval Process
        public ActionResult ViewSalesComPayableReqForApproval(int nId,int nid2, int nCPId, double ts, int buid)
        {
            string sSql = "";
            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            List<SalesCommissionPayable> oBillSalesCommissionPayables = new List<SalesCommissionPayable>();
            if (nId > 0)
            {
                sSql = "select * from View_SalesCommissionPayable where SalesCommissionID IN (select SalesCommissionID from SalesCommission where [Status] =3 ) AND ExportBillID = " + nId + " AND ContactPersonnelID = " + nCPId;
                oSalesCommissionPayables = SalesCommissionPayable.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else if(nid2>0)
            {
                sSql = "select * from View_SalesCommissionPayable where SalesCommissionID IN (select SalesCommissionID from SalesCommission where [Status] =3 ) AND isnull(ExportBillID,0)=0 and ExportPIID = " + nid2 + " AND ContactPersonnelID =" + nCPId;
                oSalesCommissionPayables = SalesCommissionPayable.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Company = oCompany;
            return View(oSalesCommissionPayables);
        }
        public ActionResult ViewSalesComPayableApproval(int nId,int nid2, int nCPId, double ts, int buid)
        {
            string sSql = "";
            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            List<SalesCommissionPayable> oBillSalesCommissionPayables = new List<SalesCommissionPayable>();
            if (nId > 0)
            {
                sSql = "select * from View_SalesCommissionPayable where SalesCommissionID IN (select SalesCommissionID from SalesCommission where [Status] =3 ) AND ExportBillID = " + nId + " AND ContactPersonnelID = " + nCPId;
                oSalesCommissionPayables = SalesCommissionPayable.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else if (nid2 > 0)
            {
                sSql = "select * from View_SalesCommissionPayable where SalesCommissionID IN (select SalesCommissionID from SalesCommission where [Status] =3 ) AND isnull(ExportBillID,0)=0 and ExportPIID=" + nid2 + " AND ContactPersonnelID = " + nCPId;
                oSalesCommissionPayables = SalesCommissionPayable.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Company = oCompany;
            return View(oSalesCommissionPayables);
        }
        #endregion
        #endregion

        #region HTTP
        [HttpPost]
        public JsonResult Save(SalesCommissionPayable oSalesCommissionPayable)
        {
            oSalesCommissionPayable.RemoveNulls();
            _oSalesCommissionPayable = new SalesCommissionPayable();
            try
            {
                _oSalesCommissionPayable = oSalesCommissionPayable.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesCommissionPayable);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult VoucherEffect(SalesCommissionPayable oSalesCommissionPayable)
        {
            oSalesCommissionPayable.RemoveNulls();
            _oSalesCommissionPayable = new SalesCommissionPayable();
            try
            {
                _oSalesCommissionPayable = oSalesCommissionPayable.VoucherEffect(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesCommissionPayable);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Gets()
        {
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            oSalesCommissionPayables = SalesCommissionPayable.Gets("",((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oSalesCommissionPayables);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #region Search  by Press Enter
        [HttpGet]
        public JsonResult searchByPINo(string sTempData, double ts)
        {
            _oSalesCommissionPayables = new List<SalesCommissionPayable>();
            string sSQL = "";
            sSQL = "SELECT * FROM View_SalesCommissionPayable WHERE PINo LIKE'%" + sTempData + "%'";
           
            try
            {
                _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = ex.Message;
                _oSalesCommissionPayables.Add(_oSalesCommissionPayable);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesCommissionPayables);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByInvoiceNo(string sTempData, double ts)
        {
            _oSalesCommissionPayables = new List<SalesCommissionPayable>();
            string sSQL = "";

            sSQL = "SELECT * FROM View_SalesCommissionPayable where SalesCommissionPayableID in (Select SalesCommissionPayableID from View_SalesCommissionPayableDetail where View_SalesCommissionPayableDetail.ImportInvoiceNo  LIKE'%" + sTempData + "%')";

            try
            {
                _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = ex.Message;
                _oSalesCommissionPayables.Add(_oSalesCommissionPayable);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesCommissionPayables);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult RequestedSalesCommisionPayableApproval(SalesCommissionPayable oSC)
        {
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            try
            {
                oSalesCommissionPayables = SalesCommissionPayable.ApprovedRequestedPayable(oSC, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = ex.Message;
                oSalesCommissionPayables = new List<SalesCommissionPayable>();
                oSalesCommissionPayables.Add(oSalesCommissionPayable);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesCommissionPayables);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RequestedSalesCommisionPayableApprovalForPI(SalesCommissionPayable oSC)
        {
            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            try
            {
                oSalesCommissionPayable = SalesCommissionPayable.ApprovedRequestedPayableForPI(oSC, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesCommissionPayable);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult RequestedSalesCommisionPayableUpdate(SalesCommissionPayable oSC)
        //{
        //    List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
        //    try
        //    {
        //        oSalesCommissionPayables = SalesCommissionPayable.UpdateRequestedPayable(oSC, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    }
        //    catch (Exception ex)
        //    {
        //        SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
        //        oSalesCommissionPayable.ErrorMessage = ex.Message;
        //        oSalesCommissionPayables = new List<SalesCommissionPayable>();
        //        oSalesCommissionPayables.Add(oSalesCommissionPayable);

        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oSalesCommissionPayables);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult SalesCommisionPayableApproval(SalesCommissionPayable oSC)
        {
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();

            try
            {
                oSalesCommissionPayables = SalesCommissionPayable.ApprovedPayable(oSC, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = ex.Message;
                oSalesCommissionPayables = new List<SalesCommissionPayable>();
                oSalesCommissionPayables.Add(oSalesCommissionPayable);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesCommissionPayables);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalesCommisionPayableApprovalForPI(SalesCommissionPayable oSC)
        {
            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            try
            {
                oSalesCommissionPayable = SalesCommissionPayable.ApprovedPayableForPI(oSC, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesCommissionPayable);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Advance Search
        public ActionResult AdvSalesComissionPayable()
        {
            return PartialView();
        }

        #region HttpGet For Search

        [HttpGet]

        public JsonResult AdvSearchSCP(string sTemp)
        {
            var tuple = new Tuple<List<SalesCommissionPayable>, List<SalesCommissionPayable>, List<SalesCommissionPayable>>(new List<SalesCommissionPayable>(), new List<SalesCommissionPayable>(), new List<SalesCommissionPayable>());
            try
            {
                string sSQL = GetSQL(sTemp);
                var oSCPS = SalesCommissionPayable.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                
                List<SalesCommissionPayable> oSCPGroupByContactPersonnelID = new List<SalesCommissionPayable>();

                oSCPGroupByContactPersonnelID = oSCPS.GroupBy(x => new { x.ContactPersonnelID, x.CPName ,x.ContractorName, x.CRate}, (key, grp)=>
                                                new SalesCommissionPayable
                                                {
                                                    CPName = key.CPName,
                                                    ContractorName = key.ContractorName,
                                                    CommissionAmount = grp.Sum(p => p.CommissionAmount),
                                                    MaturityAmount = grp.Sum(p => p.MaturityAmount),
                                                    RealizeAmount = grp.Sum(p => p.RealizeAmount),
                                                    AdjAdd = grp.Sum(p => p.AdjAdd),
                                                    AdjDeduct = grp.Sum(p => p.AdjDeduct),
                                                    AdjPayable = grp.Sum(p => p.AdjPayable),
                                                    AdjOverdueAmount = grp.Sum(p => p.AdjOverdueAmount),
                                                    Amount_Paid = grp.Sum(x => x.Amount_Paid),
                                                    CRate = key.CRate,
                                                    IsWillVoucherEffect = grp.Select(x => x.IsWillVoucherEffect).FirstOrDefault(),
                                                    Status = grp.Select(x => x.Status).FirstOrDefault(),
                                                    Status_Payable = grp.Select(x => x.Status_Payable).FirstOrDefault()
                                               }).ToList();
                List<SalesCommissionPayable> oBillWIse = new List<SalesCommissionPayable>();

                oBillWIse = oSCPS.GroupBy(x => new { x.ContactPersonnelID, x.CPName, x.ContractorName, x.ExportLCID, x.LDBCNo, x.ExportBillID, x.ExportLCNo, x.LCOpeningDate, x.MaturityReceivedDate, x.RelizationDate, x.MaturityDate, x.CRate }, (key, grp) =>
                                           new SalesCommissionPayable
                                           {
                                               CPName = key.CPName,
                                               ContractorName = key.ContractorName,
                                               ContactPersonnelID = key.ContactPersonnelID,
                                               ExportLCNo = key.ExportLCNo,
                                               LCOpeningDate = key.LCOpeningDate,
                                               MaturityDate = key.MaturityDate,
                                               MaturityReceivedDate = key.MaturityReceivedDate,
                                               RelizationDate = key.RelizationDate,
                                               LDBCNo = key.LDBCNo,
                                               CommissionAmount = grp.Sum(p => p.CommissionAmount),
                                               MaturityAmount = grp.Sum(p => p.MaturityAmount),
                                               RealizeAmount = grp.Sum(p => p.RealizeAmount),
                                               AdjAdd = grp.Sum(p => p.AdjAdd),
                                               AdjDeduct = grp.Sum(p => p.AdjDeduct),
                                               AdjPayable = grp.Sum(p => p.AdjPayable),
                                               AdjOverdueAmount = grp.Sum(p => p.AdjOverdueAmount),
                                               ExportBillID = key.ExportBillID,
                                               Amount_Paid = grp.Sum(x => x.Amount_Paid),
                                               CRate = key.CRate,
                                               Status = grp.Select(x => x.Status).FirstOrDefault(),
                                               IsWillVoucherEffect = grp.Select(x => x.IsWillVoucherEffect).FirstOrDefault(),
                                               Status_Payable = grp.Select(x => x.Status_Payable).FirstOrDefault(),
                                               ExportPIID = grp.Select(x => x.ExportPIID ).FirstOrDefault()
                                               //Status_Payable = key.Status_Payable
                                           }).ToList();

                tuple = new Tuple<List<SalesCommissionPayable>, List<SalesCommissionPayable>, List<SalesCommissionPayable>>(oSCPS, oSCPGroupByContactPersonnelID, oBillWIse);

            }
            catch (Exception e)
            {
                //tuple.Item1; //total list List
                //tuple.Item2;//GroupByContactPersonnel List
                //tuple.Item3;
                tuple = new Tuple<List<SalesCommissionPayable>, List<SalesCommissionPayable>, List<SalesCommissionPayable>>(new List<SalesCommissionPayable>(), new List<SalesCommissionPayable>(), new List<SalesCommissionPayable>());
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(tuple);
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

            //MaturityReceive  Date
            int nMaturityReceiveDateCompare = Convert.ToInt32(sTemp.Split('~')[9]);
            DateTime dtMaturityReceiveStartDate = Convert.ToDateTime(sTemp.Split('~')[10]);
            DateTime dtMaturityReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[11]);

            //Maturity  Date
            int nMaturityDateCompare = Convert.ToInt32(sTemp.Split('~')[12]);
            DateTime dtMaturityStartDate = Convert.ToDateTime(sTemp.Split('~')[13]);
            DateTime dtMaturityEndDate = Convert.ToDateTime(sTemp.Split('~')[14]);

            //Relization Date  
            int nRelizationDateCompare = Convert.ToInt32(sTemp.Split('~')[15]);
            DateTime dtRelizationStartDate = Convert.ToDateTime(sTemp.Split('~')[16]);
            DateTime dtRelizationEndDate = Convert.ToDateTime(sTemp.Split('~')[17]);

            string sPINo = sTemp.Split('~')[18];
            string sLCNo = sTemp.Split('~')[19];
            string sBuyerIDs = sTemp.Split('~')[20];
            string sContractorIDs = sTemp.Split('~')[21];
            string sLDBCNo = sTemp.Split('~')[22];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[23]);
            //int BUID = 0;// Convert.ToInt32(sTemp.Split('~')[18]);

            if (sTemp.Split('~').Length > 23)
                Int32.TryParse(sTemp.Split('~')[23], out nBUID);
            int nStatus = Convert.ToInt32(sTemp.Split('~')[24]);





            string sReturn1 = "SELECT * FROM View_SalesCommissionPayable";
            string sReturn = "";
            #region BUID

            if (nBUID >0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;

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
                sReturn = sReturn + " ExportLCNo LIKE '%" + sLCNo + "%'";
            }
            #endregion

            #region LDBC No
            if (sLDBCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LDBCNo LIKE '%" + sLDBCNo + "%'";
            }
            #endregion

            #region Buyer wise

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContactPersonnelID IN (" + sBuyerIDs + ")";
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
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpeningDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpeningDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpeningDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpeningDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpeningDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nLCDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpeningDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtLCEndDate.ToString("dd MMM yyyy") + "',106))";
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

            #region Maturity Receive Date Wise
            if (nMaturityReceiveDateCompare > 0)
            {
                if (nMaturityReceiveDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityReceivedDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityReceiveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityReceivedDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityReceiveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityReceivedDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityReceiveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityReceivedDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityReceiveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityReceivedDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityReceiveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityReceivedDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Maturity  Date Wise
            if (nMaturityDateCompare > 0)
            {
                if (nMaturityDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMaturityDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MaturityDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMaturityEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Maturity Receive Date Wise
            if (nRelizationDateCompare > 0)
            {
                if (nRelizationDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RelizationDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nRelizationDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RelizationDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nRelizationDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RelizationDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nRelizationDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RelizationDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nRelizationDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RelizationDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nRelizationDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RelizationDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRelizationEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Status
            if (nStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Status_Payable IN ( " + nStatus +" )";

            }
           
            #endregion

            sReturn = sReturn1 + sReturn + " Order By ExportLCID, CPName";
            return sReturn;
        }
        #endregion

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
        #region PDF
        public ActionResult PrintCommissionBillReportBillWise(string sParams, double nts)
        {
            List<SalesCommissionPayable> _oSalesCommissionPayables = new List<SalesCommissionPayable>();
            if (string.IsNullOrEmpty(sParams))
            {
                _oSalesCommissionPayables = SalesCommissionPayable.Gets("Select * from View_SalesCommissionPayable", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                string sSQL = GetSQL(sParams);
                _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            List<SalesCommissionPayable> oBillWIse = new List<SalesCommissionPayable>();

            oBillWIse = _oSalesCommissionPayables.GroupBy(x => new { x.ContactPersonnelID, x.CPName, x.ContractorName, x.ExportLCID, x.LDBCNo, x.ExportBillID, x.ExportLCNo, x.LCOpeningDate,x.Currency }, (key, grp) =>
                                            new SalesCommissionPayable
                                            {
                                                CPName = key.CPName,
                                                ContractorName = key.ContractorName,
                                                ContactPersonnelID = key.ContactPersonnelID,
                                                ExportLCNo = key.ExportLCNo,
                                                LCOpeningDate = key.LCOpeningDate,
                                                LDBCNo = key.LDBCNo,
                                                CommissionAmount = grp.Sum(p => p.CommissionAmount),
                                                MaturityAmount = grp.Sum(p => p.MaturityAmount),
                                                RealizeAmount = grp.Sum(p => p.RealizeAmount),
                                                AdjAdd = grp.Sum(p => p.AdjAdd),
                                                AdjDeduct = grp.Sum(p => p.AdjDeduct),
                                                AdjPayable = grp.Sum(p => p.AdjPayable),
                                                AdjOverdueAmount = grp.Sum(p => p.AdjOverdueAmount),
                                                Currency =key.Currency
                                            }).ToList();


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptCommissionBalaceBillWise oReport = new rptCommissionBalaceBillWise();
            byte[] abytes = oReport.PrepareReport(oBillWIse, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintCommissionMemoReportByBill(int nExportBillID,int nPIID, int nPID, int nType, double nts)
        {
            string sSQL = "";
            List<SalesCommissionPayable> _oSalesCommissionPayables = new List<SalesCommissionPayable>();
            List<ExportPIReport> oExportPIReports = new List<ExportPIReport>();
            List<SalesComPayment> oSalesComPayments = new List<SalesComPayment>();
            List<ExportBill> oExportBills = new List<ExportBill>();

            switch (nType)
            {
                case 1:
                    if (nExportBillID > 0)
                    {
                        sSQL = "Select * from View_SalesCommissionPayable where ExportBillID=" + nExportBillID;
                        _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_ExportPIReport where ExportPIDetailID in (Select ExportPIDetailID from ExportBillDetail where ExportBillID=" + nExportBillID + ")";
                        oExportPIReports = ExportPIReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from SalesCommissionPayable where ExportBillID=" + nExportBillID + "))";
                        oSalesComPayments = SalesComPayment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        sSQL = "Select * from View_SalesCommissionPayable where ExportPIID=" + nPIID;
                        _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_ExportPIReport where ExportPIID=" + nPIID;
                        oExportPIReports = ExportPIReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from SalesCommissionPayable where ExportPIID=" + nPIID + "))";
                        oSalesComPayments = SalesComPayment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    break;

                case 2:
                    if (nExportBillID > 0)
                    {
                        sSQL = "Select * from View_SalesCommissionPayable where ExportBillID=" + nExportBillID + " AND ContactPersonnelID=" + nPID + " AND ContractorID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ")";
                        _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_ExportPIReport where ContractorID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") AND ExportPIDetailID in (Select ExportPIDetailID from ExportBillDetail where ExportBillID=" + nExportBillID + ")";
                        oExportPIReports = ExportPIReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from View_SalesCommissionPayable where ExportBillID=" + nExportBillID + " AND ContactPersonnelID=" + nPID + " AND ContractorID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") ))";
                        oSalesComPayments = SalesComPayment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        sSQL = "Select * from View_SalesCommissionPayable where ExportPIID=" + nPIID + " AND ContactPersonnelID=" + nPID + " AND ContractorID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ")";
                        _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_ExportPIReport where ContractorID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") AND ExportPIID= " + nPIID;
                        oExportPIReports = ExportPIReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from View_SalesCommissionPayable where ExportPIID=" + nPIID + " AND ContactPersonnelID=" + nPID + " AND ContractorID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") ))";
                        oSalesComPayments = SalesComPayment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    break;

                case 3:
                    if (nExportBillID > 0)
                    {
                        sSQL = "Select * from View_SalesCommissionPayable where ExportBillID=" + nExportBillID + " AND ContactPersonnelID=" + nPID + " AND BuyerID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ")";
                        _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_ExportPIReport where BuyerID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") AND ExportPIDetailID in (Select ExportPIDetailID from ExportBillDetail where ExportBillID=" + nExportBillID + ")";
                        oExportPIReports = ExportPIReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from View_SalesCommissionPayable where ExportBillID=" + nExportBillID + " AND ContactPersonnelID=" + nPID + " AND BuyerID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") ))";
                        oSalesComPayments = SalesComPayment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        sSQL = "Select * from View_SalesCommissionPayable where ExportPIID=" + nPIID + " AND ContactPersonnelID=" + nPID + " AND BuyerID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ")";
                        _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_ExportPIReport where BuyerID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") AND ExportPIID= " + nPIID;
                        oExportPIReports = ExportPIReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "select * from view_SalesComPayment where SalesComPaymentID in (select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from View_SalesCommissionPayable where ExportPIID=" + nPIID + " AND ContactPersonnelID=" + nPID + " AND BuyerID IN (SELECT ContractorID FROM ContactPersonnel WHERE ContactPersonnelID =" + nPID + ") ))";
                        oSalesComPayments = SalesComPayment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    break;
            }
            if (nExportBillID > 0)
            {
                sSQL = "select * from view_ExportBill where ExportBillID =" + nExportBillID;
                oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (oExportPIReports.Count > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(oExportPIReports[0].BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (_oSalesCommissionPayables.Count <= 0) {
                rptErrorMessage oReport_Error = new rptErrorMessage();
                byte[] abytes_Erroe = oReport_Error.PrepareReport("No Data Found!!");
                return File(abytes_Erroe, "application/pdf");
            }

            rptCommissionMemo oReport = new rptCommissionMemo();
            byte[] abytes = oReport.PrepareReport(oExportBills,oExportPIReports, _oSalesCommissionPayables, oSalesComPayments, oCompany, oBusinessUnit, nType);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region PrintXL
        public void Print_ReportXL(string sTempString, int BUID, int trackid)
        {

            List<SalesCommissionPayable> _oSalesCommissionPayables = new List<SalesCommissionPayable>();
            List<SalesCommissionPayable> oSalesCommissionPayable = new List<SalesCommissionPayable>();
            if (string.IsNullOrEmpty(sTempString))
            {
                _oSalesCommissionPayables = SalesCommissionPayable.Gets("Select * from View_SalesCommissionPayable Where BUID=" + BUID.ToString() + " and Status_Payable not in (" + (int)EnumLSalesCommissionStatus.Payable + "," + (int)EnumLSalesCommissionStatus.Cancel + "," + (int)EnumLSalesCommissionStatus.PaidPartially + "," + (int)EnumLSalesCommissionStatus.Paid + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                string sSQL = GetSQL(sTempString);
                _oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            List<SalesCommissionPayable> oSCPGroupByContactPersonnelID = new List<SalesCommissionPayable>();
            oSCPGroupByContactPersonnelID = _oSalesCommissionPayables.GroupBy(x => new { x.ContactPersonnelID, x.CPName, x.ContractorName, x.CRate  }, (key, grp) =>
                                            new SalesCommissionPayable
                                            {
                                                CPName = key.CPName,
                                                ContractorName = key.ContractorName,
                                                CommissionAmount = grp.Sum(p => p.CommissionAmount),
                                                MaturityAmount = grp.Sum(p => p.MaturityAmount),
                                                RealizeAmount = grp.Sum(p => p.RealizeAmount),
                                                AdjAdd = grp.Sum(p => p.AdjAdd),
                                                AdjDeduct = grp.Sum(p => p.AdjDeduct),
                                                AdjPayable = grp.Sum(p => p.AdjPayable),
                                                AdjOverdueAmount = grp.Sum(p => p.AdjOverdueAmount),
                                                Amount_Paid = grp.Sum(x => x.Amount_Paid),
                                                CRate = key.CRate,
                                                Status = grp.Select(x => x.Status).FirstOrDefault(),
                                                Status_Payable = grp.Select(x => x.Status_Payable).FirstOrDefault()
                                            }).ToList();
            List<SalesCommissionPayable> oBillWIse = new List<SalesCommissionPayable>();

            oBillWIse = _oSalesCommissionPayables.GroupBy(x => new { x.ContactPersonnelID, x.CPName, x.ContractorName, x.ExportLCID, x.LDBCNo, x.ExportBillID, x.ExportLCNo, x.LCOpeningDate, x.MaturityReceivedDate, x.RelizationDate, x.MaturityDate, x.CRate }, (key, grp) =>
                                            new SalesCommissionPayable
                                            {
                                                CPName = key.CPName,
                                                ContractorName = key.ContractorName,
                                                ContactPersonnelID = key.ContactPersonnelID,
                                                ExportLCNo = key.ExportLCNo,
                                                LCOpeningDate = key.LCOpeningDate,
                                                MaturityDate = key.MaturityDate,
                                                MaturityReceivedDate = key.MaturityReceivedDate,
                                                RelizationDate = key.RelizationDate,
                                                LDBCNo = key.LDBCNo,
                                                CommissionAmount = grp.Sum(p => p.CommissionAmount),
                                                MaturityAmount = grp.Sum(p => p.MaturityAmount),
                                                RealizeAmount = grp.Sum(p => p.RealizeAmount),
                                                AdjAdd = grp.Sum(p => p.AdjAdd),
                                                AdjDeduct = grp.Sum(p => p.AdjDeduct),
                                                AdjPayable = grp.Sum(p => p.AdjPayable),
                                                AdjOverdueAmount = grp.Sum(p => p.AdjOverdueAmount),
                                                ExportBillID = key.ExportBillID,
                                                Amount_Paid = grp.Sum(x => x.Amount_Paid),
                                                CRate = key.CRate,
                                                Status = grp.Select(x => x.Status).FirstOrDefault(),
                                                Status_Payable = grp.Select(x => x.Status_Payable).FirstOrDefault()
                                            }).ToList();


            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nCommissionAmount = 0;
            double nMaturityAmount = 0;
            double nRealizeAmount = 0;
            double nAdjOverdueAmount = 0;
            double nAdjAdd = 0;
            double nAdjDeduct = 0;

            if (trackid == 1)
            {
                #region Export Excel
                int nRowIndex = 2, nStartRow = 1, nEndRow = 0, nStartCol = 1, nEndCol = 16;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add(" Sales Commission Payable(Bill View)");
                    sheet.Name = "Sales Commision Payable(Bill Wise)";

                    sheet.Column(2).Width = 25;
                    sheet.Column(3).Width = 30;
                    sheet.Column(4).Width = 22;
                    sheet.Column(5).Width = 15;
                    sheet.Column(6).Width = 22;
                    sheet.Column(7).Width = 20;
                    sheet.Column(8).Width = 20;
                    sheet.Column(9).Width = 20;
                    sheet.Column(10).Width = 20;
                    sheet.Column(11).Width = 20;
                    sheet.Column(12).Width = 20;
                    sheet.Column(13).Width = 20;
                    sheet.Column(14).Width = 20;
                    sheet.Column(15).Width = 20;
                    sheet.Column(16).Width = 20;
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
                    cell.Value = "Sales Commision Payable(Bill Wise)  "; cell.Style.Font.Bold = true;
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

                    #region HEADER

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Person Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "L B D C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Commission Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Maturity Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Realize Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Adj Deduct"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Adj Over Due Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Actual Payable Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Paid Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Yet To Paid"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Yet To Paid (BDT)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    
                    #endregion
                    string sTemp = "";
                    #region Data
                    foreach (SalesCommissionPayable oItem in oBillWIse)
                    {
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.CPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.LCOpeningDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LDBCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.CommissionAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.MaturityAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.RealizeAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.AdjDeduct + oItem.AdjPayable; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.AdjOverdueAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.PayableAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.Amount_Paid; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.YetToPaid; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.YetToPaidBDT; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.Status_PayableSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCommissionAmount = nCommissionAmount + oItem.CommissionAmount;
                        nMaturityAmount = nMaturityAmount + oItem.MaturityAmount;
                        nRealizeAmount = nRealizeAmount + oItem.RealizeAmount;
                        //nAdjOverdueAmount = nAdjOverdueAmount + oItem.AdjOverdueAmount;
                        nAdjAdd = nAdjAdd + oItem.AdjAdd;
                        nAdjDeduct = nAdjDeduct + oItem.AdjDeduct + oItem.AdjPayable;

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
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = nCommissionAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nMaturityAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = nRealizeAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oBillWIse.Sum(x =>(x.AdjDeduct + x.AdjPayable)); ; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oBillWIse.Sum(x => x.AdjOverdueAmount); ; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oBillWIse.Sum(x => x.PayableAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oBillWIse.Sum(x => x.Amount_Paid); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oBillWIse.Sum(x => x.YetToPaid); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oBillWIse.Sum(x=> x.YetToPaidBDT); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
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
                    Response.AddHeader("content-disposition", "attachment; filename=SalesCommisionPayable(Bill-Wise).xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();


                }
                #endregion
            }

            if (trackid == 2)
            {
                #region Export Excel
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 22;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add(" Sales Commission Payable (PI Wise)");
                    sheet.Name = "Sales Commision Payable (PI Wise)";
                    sheet.Column(2).Width = 25;
                    sheet.Column(3).Width = 30;
                    sheet.Column(4).Width = 22;
                    sheet.Column(5).Width = 15;
                    sheet.Column(6).Width = 22;
                    sheet.Column(7).Width = 15;
                    sheet.Column(8).Width = 15;
                    sheet.Column(9).Width = 22;
                    sheet.Column(10).Width = 15;
                    sheet.Column(11).Width = 15;
                    sheet.Column(12).Width = 15;
                    sheet.Column(13).Width = 20;
                    sheet.Column(14).Width = 20;
                    sheet.Column(15).Width = 20;
                    sheet.Column(16).Width = 20;
                    sheet.Column(17).Width = 20;
                    sheet.Column(18).Width = 20;
                    sheet.Column(19).Width = 20;
                    sheet.Column(20).Width = 20;
                    sheet.Column(21).Width = 20;
                    sheet.Column(22).Width = 20;

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
                    cell.Value = "Sales Commision Payable(Bill Wise)  "; cell.Style.Font.Bold = true;
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

                    #region HEADER

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Person Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Ammendment Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "L D B C No "; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Maturity Received Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Relization Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Commission Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Maturity Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Realize Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Adj Deduct"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Adj Over due Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Actual Payable Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Paid Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Yet To Paid"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 21]; cell.Value = "Yet To Paid (BDT)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 22]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;


                    #endregion
                    string sTemp = "";
                    #region Data
                    foreach (SalesCommissionPayable oItem in _oSalesCommissionPayables)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.CPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.PIDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.LCOpeningDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.AmendmentDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.LDBCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.MaturityReceivedDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.MaturityDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.RelizationDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.CommissionAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.MaturityAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.RealizeAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.AdjDeduct + oItem.AdjPayable; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.AdjOverdueAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.PayableAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 19]; cell.Value = oItem.Amount_Paid; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 20]; cell.Value = oItem.YetToPaid; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 21]; cell.Value = oItem.YetToPaidBDT; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 22]; cell.Value = oItem.Status_PayableSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCommissionAmount = nCommissionAmount + oItem.CommissionAmount;
                        nMaturityAmount = nMaturityAmount + oItem.MaturityAmount;
                        nRealizeAmount = nRealizeAmount + oItem.RealizeAmount;
                        // nAdjOverdueAmount = nAdjOverdueAmount + oItem.AdjOverdueAmount;
                        nAdjAdd = nAdjAdd + oItem.AdjAdd;
                        nAdjDeduct = nAdjDeduct + oItem.AdjDeduct + oItem.AdjPayable;

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

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
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

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   
                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = nCommissionAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = nMaturityAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = nRealizeAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oBillWIse.Sum(x => (x.AdjDeduct + x.AdjPayable));  cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = oBillWIse.Sum(x => x.AdjOverdueAmount); ; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = oBillWIse.Sum(x => x.PayableAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = oBillWIse.Sum(x => x.Amount_Paid); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = oBillWIse.Sum(x => x.YetToPaid); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 21]; cell.Value = oBillWIse.Sum(x => x.YetToPaidBDT); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 22]; cell.Value = ""; cell.Style.Font.Bold = true;
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
                    Response.AddHeader("content-disposition", "attachment; filename=SalesCommisionPayable(PI).xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
             }

            if (trackid == 3)
            {
                #region Export Excel
                int nRowIndex = 2, nStartRow = 1, nEndRow = 0, nStartCol = 1, nEndCol = 13;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add(" Sales Commission Payable(Person Wise)");
                    sheet.Name = "Sales Commision Payable(Person Wise)";

                    sheet.Column(2).Width = 25;
                    sheet.Column(3).Width = 30;
                    sheet.Column(4).Width = 20;
                    sheet.Column(5).Width = 20;
                    sheet.Column(6).Width = 20;
                    sheet.Column(7).Width = 20;
                    sheet.Column(8).Width = 20;
                    sheet.Column(9).Width = 20;
                    sheet.Column(10).Width = 20;
                    sheet.Column(11).Width = 20;
                    sheet.Column(12).Width = 20;
                    sheet.Column(13).Width = 20;
                   
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
                    cell.Value = "Sales Commision Payable(Person Wise)  "; cell.Style.Font.Bold = true;
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

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Person Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Commission Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Maturity Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Realize Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Adj Deduct"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Adj Over due Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Payable Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Actual Payable Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Paid Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Yet To Paid"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Yet To Paid (BDT)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;


                    #endregion
                    string sTemp = "";
                    #region Data
                    foreach (SalesCommissionPayable oItem in oSCPGroupByContactPersonnelID)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.CPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.CommissionAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.MaturityAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.RealizeAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.AdjDeduct + oItem.AdjPayable; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.AdjOverdueAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.PayableAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ActualPayableAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Amount_Paid; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.YetToPaid; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.YetToPaidBDT; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCommissionAmount = nCommissionAmount + oItem.CommissionAmount;
                        nMaturityAmount = nMaturityAmount + oItem.MaturityAmount;
                        nRealizeAmount = nRealizeAmount + oItem.RealizeAmount;
                        // nAdjOverdueAmount = nAdjOverdueAmount + oItem.AdjOverdueAmount;
                        nAdjAdd = nAdjAdd + oItem.AdjAdd;
                        nAdjDeduct = nAdjDeduct + oItem.AdjDeduct+ oItem.AdjPayable;

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


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = nCommissionAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = nMaturityAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = nRealizeAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oSCPGroupByContactPersonnelID.Sum(x=>x.AdjDeduct+x.AdjPayable); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oSCPGroupByContactPersonnelID.Sum(x=>x.AdjOverdueAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oSCPGroupByContactPersonnelID.Sum(x=>x.PayableAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oSCPGroupByContactPersonnelID.Sum(x=>x.ActualPayableAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oSCPGroupByContactPersonnelID.Sum(x=>x.Amount_Paid); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oSCPGroupByContactPersonnelID.Sum(x=>x.YetToPaid); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oSCPGroupByContactPersonnelID.Sum(x=>x.YetToPaidBDT); cell.Style.Font.Bold = true;
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
                    Response.AddHeader("content-disposition", "attachment; filename=SalesCommisionPayable(Person-Wise).xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                #endregion

                }
            }





        }
        #endregion

        #endregion
    }
}
