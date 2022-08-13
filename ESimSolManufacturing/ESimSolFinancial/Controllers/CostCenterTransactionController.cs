using System;
using System.Collections.Generic;
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
    public class CostCenterTransactionController : Controller
    {
        #region Declaration
        CostCenterTransaction _oCostCenterTransaction = new CostCenterTransaction();
        List<CostCenterTransaction> _oCostCenterTransactions = new List<CostCenterTransaction>();        
        string _sErrorMessage = "";
        #endregion

        public ActionResult CostCenterEntry(int nAccountHeadID)
        {
            //List<COA_ChartOfAccountCostCenter> oCOA_ChartOfAccountCostCenters = new List<COA_ChartOfAccountCostCenter>();
            //string Sql = "SELECT * FROM View_COA_ChartOfAccountCostCenter WHERE AccountHeadID=" + nAccountHeadID + "";
            //oCOA_ChartOfAccountCostCenters = new List<COA_ChartOfAccountCostCenter>();
            //oCOA_ChartOfAccountCostCenters = COA_ChartOfAccountCostCenter.Gets(Sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView();
        }

        public ActionResult ViewCostCenters_VoucherOpening(string sTemp, double ts)
        {
            _oCostCenterTransactions = new List<CostCenterTransaction>();
            _oCostCenterTransactions = CostCenterTransaction.GetsByAcccountHead(Convert.ToInt16(sTemp), ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oCostCenterTransactions);
        }

        public ActionResult ViewAddCostCenter_Account(int nAccountHeadID, int nCCID, double ts)
        {
            ChartsOfAccount oCOA = new ChartsOfAccount();            
            oCOA = oCOA.Get(nAccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oCostCenterTransaction = new CostCenterTransaction();
            string sSQL = "";
            if (nCCID > 0)
            {
                sSQL = "SELECT * FROM View_CostCenter where CCID=" + nCCID + " and CCGID=5 and IsLastLayer=1 or CCID in (SELECT CC.ParentID FROM CostCenter as CC where CCID=" + nCCID + " and CCGID=5 and IsLastLayer=1) order by Name";
            }
            else
            {
                sSQL = "SELECT * FROM View_CostCenter where CCGID=5 and ParentID=1 order by Name";
            }
            //oCostCenters = CostCenter.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
         
            
            _oCostCenterTransaction.AccountHeadID = oCOA.AccountHeadID;
            _oCostCenterTransaction.CCID = nCCID;
            _oCostCenterTransaction.AccountHeadName = oCOA.AccountHeadName;
            //_oCostCenterTransaction.CostCenters = oCostCenters;
            _oCostCenterTransaction.LstCurrency = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oCostCenterTransaction);
        }
    }
}
