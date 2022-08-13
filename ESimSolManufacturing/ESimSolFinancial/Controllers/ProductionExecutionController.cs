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
    public class ProductionExecutionController : Controller
    {

        #region Declaration

        ProductionExecution _oProductionExecution = new ProductionExecution();
        List<ProductionExecution> _oProductionExecutions = new List<ProductionExecution>();
        PETransaction _oPETransaction = new PETransaction();
        List<PETransaction> _oPETransactions = new List<PETransaction>();
        List<ProductionProcedure> _oProductionProcedures = new List<ProductionProcedure>();
        ProductionProcedure _oProductionProcedure = new ProductionProcedure();
        string _sErrorMesage = "", _sDateRange = "";
        #endregion

        #region Functions

        #endregion

        #region Actions

        public ActionResult ViewProductionExecution(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionExecution).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            ViewBag.Operators = Employee.BUGets(EnumEmployeeDesignationType.Operational,buid, (int)Session[SessionInfo.currentUserID]);
            //ViewBag.Operators = Employee.Gets((int)Session[SessionInfo.currentUserID]);
            //ViewBag.HRMShifts = HRMShift.Gets( (int)Session[SessionInfo.currentUserID]);
            ViewBag.HRMShifts = HRMShift.BUWiseGets(buid, (int)Session[SessionInfo.currentUserID]);
            
            return View(_oProductionExecution);
        }
        public ActionResult ViewProductionExecutionPoly(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionExecution).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            ViewBag.Operators = Employee.Gets(EnumEmployeeDesignationType.Operational, (int)Session[SessionInfo.currentUserID]);
            ViewBag.HRMShifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProductionExecution);
        }

        [HttpPost]
        public JsonResult SavePETransaction(PETransaction oPETransaction)
        {
            _oPETransaction = new PETransaction();
            try
            {
                _oPETransaction = oPETransaction.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPETransaction = new PETransaction();
                _oPETransaction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPETransaction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetYetToProductionHour(PETransaction oPETransaction)
        {
            double nYetToProductionHour = 0.0;
            try
            {
                string sSQL = "SELECT (12 - ISNULL((SELECT SUM(ISNULL(MM.ProductionHour,0)) FROM View_PETransaction AS MM WHERE CONVERT(DATE,CONVERT(VARCHAR(12),MM.TransactionDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oPETransaction.TransactionDate.ToString("dd MMM yyyy") + "',106)) AND MM.MachineID = " + oPETransaction.MachineID.ToString() + " AND MM.ProductionStepType = 1 AND MM.ShiftID = " + oPETransaction.ShiftID.ToString() + "),0)) AS YetToProductionHour FROM View_HRM_Shift AS HH WHERE HH.ShiftID = " + oPETransaction.ShiftID.ToString();
                nYetToProductionHour = oPETransaction.GetYetToProductionHour(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                nYetToProductionHour = 0;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nYetToProductionHour);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }        

        [HttpPost]
        public JsonResult GetProductionExecutions(ProductionExecution oProductionExecution)
        {
            _oProductionExecutions = new List<ProductionExecution>();
            try
            {
                string sSQL = "SELECT * FROM View_ProductionExecution WHERE ProductionSheetID = "+oProductionExecution.ProductionSheetID+" ORDER BY Sequence";
                _oProductionExecutions = ProductionExecution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionExecution = new ProductionExecution();
                _oProductionExecution.ErrorMessage = ex.Message;
                _oProductionExecutions.Add(_oProductionExecution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionExecutions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get PE Transactions
        [HttpPost]
        public JsonResult GetPETransactions(ProductionExecution oProductionExecution)
        {
            _oPETransactions = new List<PETransaction>();
            try
            {
                string sSQL = "SELECT * FROM View_PETransaction WHERE ProductionExecutionID = " + oProductionExecution.ProductionExecutionID + " Order by TransactionDate";
                _oPETransactions = PETransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionExecution = new ProductionExecution();
                _oProductionExecution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPETransactions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Production Execution Report
        public ActionResult ViewProductionExecutionReports(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionExecution).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oPETransactions = new List<PETransaction>();
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ProductionSteps = ProductionStep.Gets((int)Session[SessionInfo.currentUserID]);


            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise || (EnumReportLayout)oItem.id == EnumReportLayout.Machine_Wise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.ReportLayouts = oReportLayouts;
            return View(_oPETransactions);
        }

        //[HttpPost]
        //public JsonResult GetsPE(PETransaction oPETransaction)
        //{
        //    _oPETransactions = new List<PETransaction>();
        //    try
        //    {
        //        string sSQL = GetSQL(oPETransaction);
        //        _oPETransactions = PETransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oProductionExecution = new ProductionExecution();
        //        _oProductionExecution.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oPETransactions);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ProductionRegister oProductionRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oProductionRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintPETransaction(double ts)
        {
            ProductionRegister oProductionRegister = new ProductionRegister();
            try
            {
                _sErrorMesage = "";
                _oPETransactions = new List<PETransaction>();
                oProductionRegister = (ProductionRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oProductionRegister);
                _oPETransactions = PETransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPETransactions.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPETransactions = new List<PETransaction>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oProductionRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptPETransactions oReport = new rptPETransactions();
                byte[] abytes = oReport.PrepareReport(_oPETransactions, oCompany, (int)oProductionRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        #endregion

        #region HttpGet For Search
        private string GetSQL(ProductionRegister oProductionRegister)
        {
            #region Splited Data
            //int nTransactionCreateDateCom = o
            //DateTime dTransactionStartDate = Convert.ToDateTime(oProductionRegister.Params.Split('~')[1]);
            //DateTime oProductionRegister.TransactionEndDate = Convert.ToDateTime(oProductionRegister.Params.Split('~')[2]);
            //int nReportLayout = Convert.ToInt32(oPETransaction.oProductionRegister.Split('~')[3]);
            #endregion
            string sReturnMainPart = "";
            string sReturn = "", sGroupBy = "", sOrderBy = "" ; 

            #region Transaction Date Wise
            if (oProductionRegister.DateCriteria > 0)
            {
                if (oProductionRegister.DateCriteria == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,TransactionDate,106)  = Convert(Date,'" + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + "',106)";
                    _sDateRange = "Transaction Date Equal to "+oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy");
                }
                if (oProductionRegister.DateCriteria == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,TransactionDate,106)  != Convert(Date,'" + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + "',106)";
                    _sDateRange = "Transaction Date Not Equal to " + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy");
                }
                if (oProductionRegister.DateCriteria == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,TransactionDate,106)  > Convert(Date,'" + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + "',106)";
                    _sDateRange = "Transaction Date Greater Than " + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy");
                }
                if (oProductionRegister.DateCriteria == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,TransactionDate,106)  < Convert(Date,'" + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + "',106)";
                    _sDateRange = "Transaction Date Less Than " + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy");
                }
                if (oProductionRegister.DateCriteria == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,TransactionDate,106) >= Convert(Date,'" + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,TransactionDate,106)  < Convert(Date,'" + oProductionRegister.TransactionEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                    _sDateRange = "Transaction Date Between " + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + " To " + oProductionRegister.TransactionEndDate.AddDays(1).ToString("dd MMM yyyy");
                }
                if (oProductionRegister.DateCriteria == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,TransactionDate,106) < Convert(Date,'" + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,TransactionDate,106)  > Convert(Date,'" + oProductionRegister.TransactionEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                    _sDateRange = "Transaction Date Not Between " + oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy") + " To " + oProductionRegister.TransactionEndDate.AddDays(1).ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region ProductionStep
            if (oProductionRegister.ProductionStepID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " ProductionStepID = " + oProductionRegister.ProductionStepID;
            }
            #endregion

            #region BU
            if (oProductionRegister.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BUID = " + oProductionRegister.BUID;
            }
            #endregion

            //#region Product Nature
            //if (oPETransaction.ProductNatureInInt > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn += " ProductNature = " + oPETransaction.ProductNatureInInt;
            //}
            //#endregion

            if (oProductionRegister.ReportLayout== EnumReportLayout.MerchandiserWise)//Employee wise
            {
                sReturnMainPart = "SELECT OperationEmpByName, OperationEmpID, UnitSymbol, SuM(Quantity) AS Quantity FROM View_PETransaction";
                sGroupBy = " GROUP By  OperationEmpID, UnitSymbol, OperationEmpByName ";
                sOrderBy = "";
            }
            else if (oProductionRegister.ReportLayout == EnumReportLayout.DateWise)//Date wise
            {
                sReturnMainPart = "SELECT OperationEmpByName, OperationEmpID, Convert(Date,TransactionDate,106)AS TransactionDate, UnitSymbol,  SuM(Quantity) AS Quantity FROM View_PETransaction";
                sGroupBy = " GROUP By  OperationEmpID,Convert(Date,TransactionDate,106), UnitSymbol, OperationEmpByName";
                sOrderBy = " Order BY OperationEmpID,TransactionDate , UnitSymbol";
            }
            sReturn = sReturnMainPart + sReturn+ sGroupBy+sOrderBy;
            return sReturn;
        }
        #endregion

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

    }
    
    
}