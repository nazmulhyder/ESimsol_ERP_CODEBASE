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
using System.Linq;




namespace ESimSolFinancial.Controllers
{
    public class PurchaseInvoiceIBPController : Controller
    {
        #region Declaration
        List<PurchaseInvoiceIBP> _oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
        PurchaseInvoiceIBP _oPurchaseInvoiceIBP = new PurchaseInvoiceIBP();
        string _sErrorMessage = "";
        #endregion

        #region View
        public ActionResult View_PurchaseInvoiceIBP(int menuid)
        {
            
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
           
            _oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
            _oPurchaseInvoiceIBPs = PurchaseInvoiceIBP.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oPurchaseInvoiceIBPs.Count > 0)
            {
                _oPurchaseInvoiceIBPs[0].BankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oPurchaseInvoiceIBPs);
        }

        #endregion

        #region HTTPGet

        [HttpPost]
        public JsonResult GetsSearchedData(PurchaseInvoiceIBP oPurchaseInvoiceIBP)
        {
            List<PurchaseInvoiceIBP> oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
            try
            {
                string sSQL = GetSQL(oPurchaseInvoiceIBP);
                oPurchaseInvoiceIBPs = PurchaseInvoiceIBP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoiceIBPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(PurchaseInvoiceIBP oPurchaseInvoiceIBP)
        {
            string sReturn1 = "SELECT * FROM View_PurchaseInvoiceLC_ABP ";
            string sReturn = " where Bankstatus=3 ";
            #region String BankName
            if (oPurchaseInvoiceIBP.BankName != null)
            {
                if (oPurchaseInvoiceIBP.BankName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NegotiateBankBranchID IN (" + oPurchaseInvoiceIBP.BankName + ")";
                }
            }

          
            if (oPurchaseInvoiceIBP.SelectedOption != null)
            {
                if (oPurchaseInvoiceIBP.SelectedOption != "")
                {
                    if (oPurchaseInvoiceIBP.SelectedOption != EnumCompareOperator.None.ToString())
                    {

                        if (oPurchaseInvoiceIBP.SelectedOption == EnumCompareOperator.Between.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " DateofMaturity >= '" + oPurchaseInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "' AND DateofMaturity < '" + oPurchaseInvoiceIBP.DateofMaturityEnd.AddDays(1).ToString("dd MMM yyyy") + "'";

                        }
                        else if (oPurchaseInvoiceIBP.SelectedOption == EnumCompareOperator.NotBetween.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " DateofMaturity < '" + oPurchaseInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "' OR DateofMaturity > '" + oPurchaseInvoiceIBP.DateofMaturityEnd.AddDays(1).ToString("dd MMM yyyy") + "'";

                        }
                        else if (oPurchaseInvoiceIBP.SelectedOption == EnumCompareOperator.EqualTo.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " DateofMaturity = '" + oPurchaseInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }

                        else if (oPurchaseInvoiceIBP.SelectedOption == EnumCompareOperator.NotEqualTo.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "DateofMaturity != '" + oPurchaseInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }
                        else if (oPurchaseInvoiceIBP.SelectedOption == EnumCompareOperator.GreaterThen.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "DateofMaturity > '" + oPurchaseInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }
                        else if (oPurchaseInvoiceIBP.SelectedOption == EnumCompareOperator.SmallerThen.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "DateofMaturity < '" + oPurchaseInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }

                    }
                }
            }
            #endregion
            sReturn = sReturn1 + sReturn + "  order by NegotiateBankBranchID,DateofMaturity,CurrencyID";
            return sReturn;
        }


        public ActionResult Print_Report(string sTempString)
        {


            _oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
            PurchaseInvoiceIBP oPurchaseInvoiceIBP = new PurchaseInvoiceIBP();
            oPurchaseInvoiceIBP.SelectedOption = sTempString.Split('~')[0];
            oPurchaseInvoiceIBP.DateofMaturity = Convert.ToDateTime(sTempString.Split('~')[1]);
            oPurchaseInvoiceIBP.DateofMaturityEnd = Convert.ToDateTime(sTempString.Split('~')[2]);
            oPurchaseInvoiceIBP.BankName = sTempString.Split('~')[3];

            try
            {
                string sSQL = GetSQL(oPurchaseInvoiceIBP);
                _oPurchaseInvoiceIBPs = PurchaseInvoiceIBP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
            }

            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            oBankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptPurchaseInvoiceIBP oReport = new rptPurchaseInvoiceIBP();
            byte[] abytes = oReport.PrepareReport_ABP(_oPurchaseInvoiceIBPs, oCompany,oCurrencys, oBankBranchs);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region Chart
        [HttpPost]
        public JsonResult GetsForGraph(string sYear, int nBankBranchID)
        {
            _oPurchaseInvoiceIBP = new PurchaseInvoiceIBP();
            _oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
            PurchaseInvoiceIBP oPurchaseInvoiceIBP_Chart = new PurchaseInvoiceIBP();
            List<PurchaseInvoiceIBP>  oPurchaseInvoiceIBPs_Chart = new List<PurchaseInvoiceIBP>();

            try
            {
                _oPurchaseInvoiceIBPs = PurchaseInvoiceIBP.GetsForGraph(sYear, nBankBranchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oPurchaseInvoiceIBPs.Count <= 0 || _oPurchaseInvoiceIBPs[0].ErrorMessage !="")
                {
                    throw new Exception("Data not found!");
                }
                else
                {
                    //_oPurchaseInvoiceIBP.PurchaseInvoiceIBPs = _oPurchaseInvoiceIBPs;

                    while (_oPurchaseInvoiceIBPs.Count > 0)
                    {
                        double nAmount = 0;
                        List<PurchaseInvoiceIBP> oPIIBPs = new List<PurchaseInvoiceIBP>();
                        oPIIBPs = _oPurchaseInvoiceIBPs.Where(x => x.DateofMaturity.Month == _oPurchaseInvoiceIBPs[0].DateofMaturity.Month).ToList();
                        foreach (PurchaseInvoiceIBP oPIIBP in oPIIBPs)
                        {
                            nAmount = nAmount + oPIIBP.Amount;
                        }
                        oPurchaseInvoiceIBP_Chart = new PurchaseInvoiceIBP();
                        oPurchaseInvoiceIBP_Chart.Amount = nAmount;
                        oPurchaseInvoiceIBP_Chart.DateofMaturity = oPIIBPs[0].DateofMaturity;
                        oPurchaseInvoiceIBP_Chart.PurchaseInvoiceIBPs = oPIIBPs;
                        oPurchaseInvoiceIBPs_Chart.Add(oPurchaseInvoiceIBP_Chart);
                        _oPurchaseInvoiceIBPs.RemoveAll(x => x.DateofMaturity.Month == oPIIBPs[0].DateofMaturity.Month);
                    }
                    _oPurchaseInvoiceIBP.PIIBPs_ChartList = oPurchaseInvoiceIBPs_Chart;

                }
             
            }
            catch (Exception ex)
            {
                
                _oPurchaseInvoiceIBP = new PurchaseInvoiceIBP();
                _oPurchaseInvoiceIBP.ErrorMessage = ex.Message;
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseInvoiceIBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Chart
    }
}
