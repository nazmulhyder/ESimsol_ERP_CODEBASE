using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class VoucherReportController : Controller
    {
        #region Declaration
        VoucherReport _oVoucherReport = new VoucherReport();

        List<VoucherReport> _oVoucherReports = new List<VoucherReport>();
        DataSet _oDataSet = new DataSet();
        DataTable _oDataTable = new DataTable();
        #endregion
        #region
        public ActionResult ViewVoucherReports(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oVoucherReport = new VoucherReport();
            return View();
        }

        [HttpPost]
        public JsonResult Gets_ReportData(VoucherReport oVoucherReport)
        {

            _oVoucherReport = new VoucherReport();
            _oVoucherReports = new List<VoucherReport>();
            if (oVoucherReport.DateTypeInt < 2)
            {
                oVoucherReport.EndDate = oVoucherReport.StartDate;
            }

            string sSQL = "";
            int nSLno = 0;
            try
            {

                _oDataSet = VoucherReport.Gets_Report(oVoucherReport.StartDate, oVoucherReport.EndDate.AddDays(1), oVoucherReport.VoucherTypeID, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDataSet = new DataSet();
            }
            if (_oDataSet.Tables.Count > 0)
            {
                _oDataTable = _oDataSet.Tables[0];
                DateTime dDate = DateTime.Today;

                //DataRow[] oDataRows = _oDataTable.Select(" LCBillID > 0 ", "LCBillID");
                foreach (DataRow oRow in _oDataTable.Rows)
                {
                    _oVoucherReport = new VoucherReport();
                    nSLno++;
                    _oVoucherReport.SLNo = nSLno;
                    _oVoucherReport.DateTypeInt = oVoucherReport.DateTypeInt;
                    _oVoucherReport.StartDate = (oRow["StartDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["StartDate"]);
                    _oVoucherReport.EndDate = (oRow["EndDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["EndDate"]);
                    _oVoucherReport.VoucherTypeID = (oRow["VoucherTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherTypeID"]);
                    _oVoucherReport.VoucherTypeName = (oRow["VoucherTypeName"] == DBNull.Value) ? "" : Convert.ToString(oRow["VoucherTypeName"]);
                    _oVoucherReport.VoucherTypeCount = (oRow["VoucherTypeCount"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherTypeCount"]);
                    _oVoucherReport.AccountTypeName = (oRow["AccountTypeName"] == DBNull.Value) ? "" : Convert.ToString(oRow["AccountTypeName"]);
                    _oVoucherReport.AccountTypeCount = (oRow["AccountTypeCount"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["AccountTypeCount"]);

                    _oVoucherReports.Add(_oVoucherReport);

                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintVoucherType(string sTemp)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            int nDateon = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            int nVoucherTypeID = Convert.ToInt32(sTemp.Split('~')[3]);

            string sDateRange = "";
            if (nDateon > 1)
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " to " + dEndDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = "For " + dStartDate.ToString("dd MMM yyyy");
                dEndDate = dStartDate;
            }

            _oVoucherReport = new VoucherReport();
            _oVoucherReports = new List<VoucherReport>();

            try
            {
                _oDataSet = VoucherReport.Gets_Report(dStartDate, dEndDate.AddDays(1), nVoucherTypeID, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDataSet = new DataSet();
            }
            if (_oDataSet.Tables.Count > 0)
            {
                _oDataTable = _oDataSet.Tables[0];
                DateTime dDate = DateTime.Today;

                //DataRow[] oDataRows = _oDataTable.Select(" LCBillID > 0 ", "LCBillID");
                foreach (DataRow oRow in _oDataTable.Rows)
                {
                    _oVoucherReport = new VoucherReport();

                    _oVoucherReport.StartDate = (oRow["StartDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["StartDate"]);
                    _oVoucherReport.EndDate = (oRow["EndDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["EndDate"]);
                    _oVoucherReport.VoucherTypeID = (oRow["VoucherTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherTypeID"]);
                    _oVoucherReport.VoucherTypeName = (oRow["VoucherTypeName"] == DBNull.Value) ? "" : Convert.ToString(oRow["VoucherTypeName"]);
                    _oVoucherReport.VoucherTypeCount = (oRow["VoucherTypeCount"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherTypeCount"]);
                    _oVoucherReport.AccountTypeName = (oRow["AccountTypeName"] == DBNull.Value) ? "" : Convert.ToString(oRow["AccountTypeName"]);
                    _oVoucherReport.AccountTypeCount = (oRow["AccountTypeCount"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["AccountTypeCount"]);

                    _oVoucherReports.Add(_oVoucherReport);

                }
            }


            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptVoucherReportReport_Statistics oReport = new rptVoucherReportReport_Statistics();
            byte[] abytes = oReport.PrepareReport(_oVoucherReports, oCompany, "Statistics", sDateRange);
            return File(abytes, "application/pdf");
        }
        #endregion
        ///////

        #region Register
        public ActionResult ViewVoucherRegister(int nID)
        {
            _oVoucherReport = new VoucherReport();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Gets_VoucherRegister(VoucherReport oVoucherReport)
        {
            if (oVoucherReport.DateTypeInt < 2)
            {
                oVoucherReport.EndDate = oVoucherReport.StartDate;
            }
            _oVoucherReport = new VoucherReport();
            _oVoucherReports = new List<VoucherReport>();
            string sSQL = "";
            int nSLno = 0;
            try
            {

                _oDataSet = VoucherReport.Gets_Report(oVoucherReport.StartDate, oVoucherReport.EndDate.AddDays(1), oVoucherReport.VoucherTypeID, 2, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDataSet = new DataSet();
            }
            if (_oDataSet.Tables.Count > 0)
            {
                _oDataTable = _oDataSet.Tables[0];
                DateTime dDate = DateTime.Today;

                foreach (DataRow oRow in _oDataTable.Rows)
                {
                    _oVoucherReport = new VoucherReport();

                    //   _oVoucherReport.StartDate
                    nSLno++;
                    _oVoucherReport.SLNo = nSLno;
                    _oVoucherReport.StartDate = (oRow["StartDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["StartDate"]);
                    _oVoucherReport.EndDate = (oRow["EndDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["EndDate"]);
                    _oVoucherReport.VoucherCount = (oRow["VoucherCount"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherCount"]);
                    _oVoucherReport.VoucherTypeID = (oRow["VoucherTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherTypeID"]);
                    _oVoucherReport.VoucherTypeName = "";
                    _oVoucherReports.Add(_oVoucherReport);

                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ViewVoucherFor a voucher Type
        public ActionResult ViewVouchersForAVType(int nID)
        {
            _oVoucherReport = new VoucherReport();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Gets_VouchersForAVType(VoucherReport oVoucherReport)
        {

            _oVoucherReport = new VoucherReport();
            _oVoucherReports = new List<VoucherReport>();
            if (oVoucherReport.DateTypeInt < 2)
            {
                oVoucherReport.EndDate = oVoucherReport.StartDate;
            }
            int nSLno = 0;
            try
            {

                _oDataSet = VoucherReport.Gets_Report(oVoucherReport.StartDate, oVoucherReport.EndDate, oVoucherReport.VoucherTypeID, 3, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDataSet = new DataSet();
            }
            if (_oDataSet.Tables.Count > 0)
            {
                _oDataTable = _oDataSet.Tables[0];
                DateTime dDate = DateTime.Today;

                foreach (DataRow oRow in _oDataTable.Rows)
                {
                    _oVoucherReport = new VoucherReport();

                    //   _oVoucherReport.StartDate
                    nSLno++;
                    _oVoucherReport.SLNo = nSLno;
                    _oVoucherReport.VoucherDate = (oRow["VoucherDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["VoucherDate"]);
                    _oVoucherReport.VoucherID = (oRow["VoucherTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherTypeID"]);
                    _oVoucherReport.VoucherNo = (oRow["VoucherNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["VoucherNo"]);
                    _oVoucherReport.Particulars = (oRow["Particulars"] == DBNull.Value) ? "" : Convert.ToString(oRow["Particulars"]);
                    _oVoucherReport.VoucherTypeID = (oRow["VoucherTypeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["VoucherTypeID"]);
                    _oVoucherReport.AccountHeadID = (oRow["AccountHeadID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["AccountHeadID"]);
                    _oVoucherReport.VoucherTypeName = (oRow["VoucherTypeName"] == DBNull.Value) ? "" : Convert.ToString(oRow["VoucherTypeName"]);
                    _oVoucherReport.DebitAmount = (oRow["DebitAmount"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["DebitAmount"]);
                    _oVoucherReport.CreditAmount = (oRow["CreditAmount"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["CreditAmount"]);

                    _oVoucherReports.Add(_oVoucherReport);

                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
