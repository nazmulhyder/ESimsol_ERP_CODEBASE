using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ReportManagement;
using iTextSharp.text;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers.ReportController
{
    public class ReportController : PdfViewController
    {
        #region Declaration
        Rpt_DailyStockSummery _oRDS = new Rpt_DailyStockSummery();
        List<Rpt_DailyStockSummery> _oRDSs = new List<Rpt_DailyStockSummery>();

        Lot _oLot = new Lot();
        
        DataSet _oDataSet = new DataSet();
        DataTable _oDataTable = new DataTable();
        double _nValue = 0;
        double _nBalance = 0;
        string _sCCName = "";
        #endregion
       
        public ActionResult ViewDailySummeryReport()
        {
            _oRDSs = new List<Rpt_DailyStockSummery>();
            return PartialView(_oRDSs);  
        }

        public void BalanceAndValue(int nProductID)
        {
            double nValue = 0;
            double nUnitPrice = 0;
            double nBalance = 0;
            _nBalance = 0;
            _nValue = 0;
            DataRow[] oDataRows = _oDataTable.Select(" ProductID = " +nProductID);
            foreach (DataRow oRow in oDataRows)
            {
                nBalance = (oRow["PreferenceUnitQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["PreferenceUnitQty"]);
                _nBalance = _nBalance + nBalance;
                nUnitPrice = (oRow["PreferenceUnitPrice"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["PreferenceUnitPrice"]);
                nValue = nUnitPrice * nBalance;
                _nValue = _nValue + nValue;
            }
        }

        [HttpPost]
        public JsonResult SearchRDSR(string sParam)// 
        {
            _oRDSs = new List<Rpt_DailyStockSummery>();
            int nPTMID = Convert.ToInt32(sParam.Split('~')[0]);
            string sDate = sParam.Split('~')[1];
            string sMonth = sParam.Split('~')[2];
            

            string sSQL = "SELECT ProductID, ProductName,MUName"
                            +" ,SUM(OpeningBalance) AS OpeningBalance"
                            +" ,SUM(ReceiveQty) AS ReceiveQty"
                            +" ,SUM(ReturnQty) AS ReturnQty"
                            +" ,SUM(TransferIn) AS TransferIn"
                            +" ,SUM(TransferOut) AS TransferOut"
                            +" ,SUM(ConsumedQty) AS ConsumedQty"
                            +" ,SUM(ClosingBalance) AS ClosingBalance"
                            +" ,AVG(UnitPrice) AS UnitPrice "
                            + " FROM VIEW_Rpt_DailyStockSummery WHERE PTMID=" + nPTMID + "";



            if (sDate != "")
            {
                sSQL = sSQL + " AND TransactionDate='" + sDate+"'";
            }
            if (sMonth != "")
            {
                sSQL = sSQL + " AND datename(YEAR, TransactionDate)='2013' AND datename(month, TransactionDate)='" + sMonth+"'";
            }

            sSQL = sSQL + " GROUP BY ProductID,ProductName,MUName";

            try
            {
                _oRDSs = Rpt_DailyStockSummery.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oRDS = new Rpt_DailyStockSummery();
                _oRDS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((List<Rpt_DailyStockSummery>)_oRDSs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #region Report Study
        public ActionResult PrintStockSummary(string rows)
        {

            _oRDS = new Rpt_DailyStockSummery();
            string sSSQL = "";
            if (rows.Length > 0)
            {
                sSSQL = "SELECT * FROM VIEW_Rpt_DailyStockSummery WHERE RSSID IN (" + rows + ")";
            }
            else
            {
                sSSQL = "SELECT * FROM VIEW_Rpt_DailyStockSummery";
            }
            _oRDS.StockSummaryforPrint = Rpt_DailyStockSummery.Gets(sSSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oRDS.Companys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return this.ViewPdf("", "rptStockSummary", _oRDS, PageSize.A4, 40, 40, 40, 40,true);
        }

        public ActionResult ViewStoreWiseReport()
        {
            StoreWiseReport oStoreWiseReport = new StoreWiseReport();
            string sSql = "SELECT * FROM View_WorkingUnit WHERE IsActive=1 AND IsStore=1";
            oStoreWiseReport.WorkingUnitList = WorkingUnit.Gets(sSql,((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(oStoreWiseReport);
        }

        public ActionResult PrintStoreWise(string sParam) 
        {
            StoreWiseReport oStoreWiseReport = new StoreWiseReport();
            Company oCompany=new Company();
            string eCompareOperator = sParam.Split('~')[0];
            DateTime dStartDate = Convert.ToDateTime(sParam.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParam.Split('~')[2]);
            string sProductIDs = sParam.Split('~')[3];
            string sStoreIDs = sParam.Split('~')[4];
            string sStorename = sParam.Split('~')[5];
            oStoreWiseReport.StoreName = sStorename;
            string sSQL = "";

            #region DateSearchStart

            if (eCompareOperator != EnumCompareOperator.None.ToString())
            {

                if (eCompareOperator == EnumCompareOperator.Between.ToString())
                {
                    oStoreWiseReport.ErrorMessage = "From " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
                    sSQL = sSQL + " Between '" + dStartDate.ToString("dd MMM yyyy") + "' AND '" + dEndDate.ToString("dd MMM yyyy") + "'";

                }
                else if (eCompareOperator == EnumCompareOperator.NotBetween.ToString())
                {
                    oStoreWiseReport.ErrorMessage = "Except " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
                    sSQL = sSQL + " Not Between '" + dStartDate.ToString("dd MMM yyyy") + "' AND '" + dEndDate.ToString("dd MMM yyyy") + "'";

                }
                else if (eCompareOperator == EnumCompareOperator.EqualTo.ToString())
                {
                    oStoreWiseReport.ErrorMessage = dStartDate.ToString("dd MMM yyyy");
                    sSQL = sSQL  + " = '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }

                else if (eCompareOperator == EnumCompareOperator.NotEqualTo.ToString())
                {
                    oStoreWiseReport.ErrorMessage = "Except " + dStartDate.ToString("dd MMM yyyy");
                    sSQL = sSQL  + " <> '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                else if (eCompareOperator == EnumCompareOperator.GreaterThen.ToString())
                {
                    oStoreWiseReport.ErrorMessage = "Greater Than " + dStartDate.ToString("dd MMM yyyy");
                    sSQL = sSQL  + "> '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                else if (eCompareOperator == EnumCompareOperator.SmallerThen.ToString())
                {
                    oStoreWiseReport.ErrorMessage = "Smaller Than " + dStartDate.ToString("dd MMM yyyy");
                    sSQL = sSQL + "< '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }

            }
            #endregion
            oStoreWiseReport.StoreWiseReports = StoreWiseReport.Gets(sStoreIDs, sProductIDs, sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oStoreWiseReport.Company = oCompany.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return this.ViewPdf("", "rptStoreWise", oStoreWiseReport, PageSize.A4, 40, 40, 40, 40, true);
        }
        
        #endregion
    }
}
