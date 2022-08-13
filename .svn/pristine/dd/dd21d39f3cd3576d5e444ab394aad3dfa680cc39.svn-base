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
    public class ImportInvoiceIBPController : Controller
    {
        #region Declaration
        List<ImportInvoiceIBP> _oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
        ImportInvoiceIBP _oImportInvoiceIBP = new ImportInvoiceIBP();
        string _sErrorMessage = "";
        #endregion

        #region View
        public ActionResult View_ImportInvoiceIBP(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            _oImportInvoiceIBPs = ImportInvoiceIBP.Gets(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oImportInvoiceIBPs.Count > 0)
            {
                _oImportInvoiceIBPs[0].BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            return View(_oImportInvoiceIBPs);
        }

        #endregion

        #region HTTPGet

        [HttpPost]
        public JsonResult GetsSearchedData(ImportInvoiceIBP oImportInvoiceIBP)
        {
            List<ImportInvoiceIBP> oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            try
            {
                string sSQL = GetSQL(oImportInvoiceIBP);
                oImportInvoiceIBPs = ImportInvoiceIBP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvoiceIBPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(ImportInvoiceIBP oImportInvoiceIBP)
        {
            string sReturn1 = "SELECT * FROM View_ImportInvoice ";
            string sReturn = " where Bankstatus=3 ";
            #region String BankName
            if (oImportInvoiceIBP.BankName != null)
            {
                if (oImportInvoiceIBP.BankName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BankBranchID_Nego IN (" + oImportInvoiceIBP.BankName + ")";
                }
            }
           
                if (oImportInvoiceIBP.BUID >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BUID IN (" + oImportInvoiceIBP.BUID + ")";
                }
           

          
            if (oImportInvoiceIBP.SelectedOption != null)
            {
                if (oImportInvoiceIBP.SelectedOption != "")
                {
                    if (oImportInvoiceIBP.SelectedOption != EnumCompareOperator.None.ToString())
                    {

                        if (oImportInvoiceIBP.SelectedOption == EnumCompareOperator.Between.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " DateofMaturity >= '" + oImportInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "' AND DateofMaturity < '" + oImportInvoiceIBP.DateofMaturityEnd.AddDays(1).ToString("dd MMM yyyy") + "'";

                        }
                        else if (oImportInvoiceIBP.SelectedOption == EnumCompareOperator.NotBetween.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " DateofMaturity < '" + oImportInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "' OR DateofMaturity > '" + oImportInvoiceIBP.DateofMaturityEnd.AddDays(1).ToString("dd MMM yyyy") + "'";

                        }
                        else if (oImportInvoiceIBP.SelectedOption == EnumCompareOperator.EqualTo.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " DateofMaturity = '" + oImportInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }

                        else if (oImportInvoiceIBP.SelectedOption == EnumCompareOperator.NotEqualTo.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "DateofMaturity != '" + oImportInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }
                        else if (oImportInvoiceIBP.SelectedOption == EnumCompareOperator.GreaterThan.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "DateofMaturity > '" + oImportInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }
                        else if (oImportInvoiceIBP.SelectedOption == EnumCompareOperator.SmallerThan.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "DateofMaturity < '" + oImportInvoiceIBP.DateofMaturity.ToString("dd MMM yyyy") + "'";
                        }

                    }
                }
            }
            #endregion
            sReturn = sReturn1 + sReturn + "  order by BankBranchID_Nego,DateofMaturity,CurrencyID";
            return sReturn;
        }


        public ActionResult Print_Report(string sTempString, int BUID)
        {

            _oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            ImportInvoiceIBP oImportInvoiceIBP = new ImportInvoiceIBP();
            oImportInvoiceIBP.SelectedOption = sTempString.Split('~')[0];
            oImportInvoiceIBP.DateofMaturity = Convert.ToDateTime(sTempString.Split('~')[1]);
            oImportInvoiceIBP.DateofMaturityEnd = Convert.ToDateTime(sTempString.Split('~')[2]);
            oImportInvoiceIBP.BankName = sTempString.Split('~')[3];
            oImportInvoiceIBP.BUID = BUID;
            try
            {
                string sSQL = GetSQL(oImportInvoiceIBP);
                _oImportInvoiceIBPs = ImportInvoiceIBP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oImportInvoiceIBP.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            oBankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptImportInvoiceIBP oReport = new rptImportInvoiceIBP();
            byte[] abytes = oReport.PrepareReport(_oImportInvoiceIBPs, oCompany, oCurrencys, oBankBranchs, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult Print_Report_Month(string sTempString, int BUID)
        {

            _oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            ImportInvoiceIBP oImportInvoiceIBP = new ImportInvoiceIBP();
            oImportInvoiceIBP.SelectedOption = sTempString.Split('~')[0];
            oImportInvoiceIBP.DateofMaturity = Convert.ToDateTime(sTempString.Split('~')[1]);
            oImportInvoiceIBP.DateofMaturity = Convert.ToDateTime(sTempString.Split('~')[2]);
            oImportInvoiceIBP.BankName = sTempString.Split('~')[3];
            oImportInvoiceIBP.BUID = BUID;


            oImportInvoiceIBP.DateofMaturity = new DateTime(oImportInvoiceIBP.DateofMaturity.Year, oImportInvoiceIBP.DateofMaturity.Month, 1);
            oImportInvoiceIBP.DateofMaturityEnd = new DateTime(oImportInvoiceIBP.DateofMaturity.Year, oImportInvoiceIBP.DateofMaturity.Month, DateTime.DaysInMonth(oImportInvoiceIBP.DateofMaturity.Year, oImportInvoiceIBP.DateofMaturity.Month));

            try
            {
                string sSQL = GetSQL(oImportInvoiceIBP);
                _oImportInvoiceIBPs = ImportInvoiceIBP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oImportInvoiceIBP.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            oBankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptImportInvoiceIBP oReport = new rptImportInvoiceIBP();
            byte[] abytes = oReport.PrepareReport(_oImportInvoiceIBPs, oCompany, oCurrencys, oBankBranchs, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region Chart
        [HttpPost]
        public JsonResult GetsForGraph(string sYear, int nBankBranchID, int BUID)
        {
            _oImportInvoiceIBP = new ImportInvoiceIBP();
            _oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            ImportInvoiceIBP oImportInvoiceIBP_Chart = new ImportInvoiceIBP();
            List<ImportInvoiceIBP>  oImportInvoiceIBPs_Chart = new List<ImportInvoiceIBP>();

            try
            {
                _oImportInvoiceIBPs = ImportInvoiceIBP.GetsForGraph(sYear, nBankBranchID,BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oImportInvoiceIBPs.Count <= 0 || _oImportInvoiceIBPs[0].ErrorMessage !="")
                {
                    throw new Exception("Data not found!");
                }
                else
                {
                    //_oImportInvoiceIBP.ImportInvoiceIBPs = _oImportInvoiceIBPs;

                    while (_oImportInvoiceIBPs.Count > 0)
                    {
                        double nAmount = 0;
                        List<ImportInvoiceIBP> oPIIBPs = new List<ImportInvoiceIBP>();
                        //oPIIBPs = _oImportInvoiceIBPs.Where(x => x.DateofMaturity.Month == _oImportInvoiceIBPs[0].DateofMaturity.Month).ToList(); //Previous
                        oPIIBPs = _oImportInvoiceIBPs.Where(x => x.DateofMaturity.Month == _oImportInvoiceIBPs[0].DateofMaturity.Month && x.DateofMaturity.Year == _oImportInvoiceIBPs[0].DateofMaturity.Year).ToList(); //Saurovs Code
                        foreach (ImportInvoiceIBP oPIIBP in oPIIBPs)
                        {
                            nAmount = nAmount + oPIIBP.Amount * oPIIBP.CCRate;
                        }
                        oImportInvoiceIBP_Chart = new ImportInvoiceIBP();
                        oImportInvoiceIBP_Chart.Amount = nAmount;
                        oImportInvoiceIBP_Chart.DateofMaturity = oPIIBPs[0].DateofMaturity;
                        oImportInvoiceIBP_Chart.ImportInvoiceIBPs = oPIIBPs;
                        oImportInvoiceIBPs_Chart.Add(oImportInvoiceIBP_Chart);
                        //_oImportInvoiceIBPs.RemoveAll(x => x.DateofMaturity.Month == oPIIBPs[0].DateofMaturity.Month); //Previous
                        _oImportInvoiceIBPs.RemoveAll(x => x.DateofMaturity.Month == oPIIBPs[0].DateofMaturity.Month && x.DateofMaturity.Year == oPIIBPs[0].DateofMaturity.Year); //Saurovs Code
                    }
                    _oImportInvoiceIBP.PIIBPs_ChartList = oImportInvoiceIBPs_Chart;

                }
             
            }
            catch (Exception ex)
            {

                _oImportInvoiceIBP = new ImportInvoiceIBP();
                _oImportInvoiceIBP.ErrorMessage = ex.Message;
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoiceIBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Chart
    }
}
