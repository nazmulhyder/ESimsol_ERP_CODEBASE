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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
	public class CashFlowSetupController : Controller
	{
		#region Declaration

		CashFlowSetup _oCashFlowSetup = new CashFlowSetup();
		List<CashFlowSetup> _oCashFlowSetups = new  List<CashFlowSetup>();
        
        List<ChartsOfAccount> _oChartsOfAccountList = new List<ChartsOfAccount>();
        ChartsOfAccount _oChartsOfAccount = new ChartsOfAccount();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewCashFlowSetups(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CashFlowSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oCashFlowSetups = new List<CashFlowSetup>(); 
			_oCashFlowSetups = CashFlowSetup.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CFTransactionGroups = EnumObject.jGets(typeof(EnumCFTransactionGroup));
            ViewBag.CFDataTypes = EnumObject.jGets(typeof(EnumCFDataType));
			return View(_oCashFlowSetups);
		}

        
        [HttpPost]
        public JsonResult GetDataTypes(CashFlowSetup oCashFlowSetup)
        {
            List<EnumObject> oCFDataTypeObjs = new List<EnumObject>();
            EnumObject oCFDataTypeObj = new EnumObject();
            
            try
            {
                switch ((EnumCFTransactionGroup)oCashFlowSetup.CFTransactionGroupInInt)
               {
                   case EnumCFTransactionGroup.Cash_Receipts:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Net_Trunover_of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Net_Trunover_of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Cash_Paid:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.COGS_of_SCI_CA_And_CL_Chnages;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.COGS_of_SCI_CA_And_CL_Chnages);
                           break;
                       }
                   case EnumCFTransactionGroup.Interest_Paid:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Financila_Cost_of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Financila_Cost_of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Income_Tax_paid:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Income_Tax_of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Income_Tax_of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Acquisition_of_Fixed_Asset:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Fixed_Asset_SFP_Changes_Less_Depreciation;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Fixed_Asset_SFP_Changes_Less_Depreciation);
                           break;
                       }
                   case EnumCFTransactionGroup.Fixed_Doposit:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Fixed_Deposit_SFP_Changes;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Fixed_Deposit_SFP_Changes);
                           break;
                       }
                   case EnumCFTransactionGroup.Acquisition_of_Intengible_Asstes:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Intengible_Asstes_SFP_Changes_Less_Depreciation;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Intengible_Asstes_SFP_Changes_Less_Depreciation);
                           break;
                       }
                   case EnumCFTransactionGroup.Capital_WIP:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Investment_SFP_Changes;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Investment_SFP_Changes);
                           break;
                       }
                   case EnumCFTransactionGroup.Payment_of_Lease_Loan:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.None_Current_Loan_SFP_Changes;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.None_Current_Loan_SFP_Changes);
                           break;
                       }
                   case EnumCFTransactionGroup.Payment_of_Term_Loan:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Current_Loan_SFP_Changes;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Current_Loan_SFP_Changes);
                           break;
                       }
                   case EnumCFTransactionGroup.Payment_of_Dividend:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Dividend_SFP_Changes;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Dividend_SFP_Changes);
                           break;
                       }
                   case EnumCFTransactionGroup.Cost_of_Sales:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.COGS_of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.COGS_of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Administrative_Cost:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Administrative_Cost_Of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Administrative_Cost_Of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Selling_Cost:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Selling_Cost_of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Selling_Cost_of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Current_Asset_Chnages:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Current_Asset_Chnages_Of_SFP;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Current_Asset_Chnages_Of_SFP);
                           break;
                       }
                   case EnumCFTransactionGroup.Current_Libility_Chnages:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Current_Libility_Chnages_Of_SFP;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Current_Libility_Chnages_Of_SFP);
                           break;
                       }
                   case EnumCFTransactionGroup.Fixed_Asset_Depreciation_Cost:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Fixed_Asset_Depreciation_Cost_Of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Fixed_Asset_Depreciation_Cost_Of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Intengible_Assets_Depreciation_Cost:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Intengible_Assets_Depreciation_Cost_Of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Intengible_Assets_Depreciation_Cost_Of_SCI);
                           break;
                       }
                   case EnumCFTransactionGroup.Opening_Balance:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Opening_Balance_of_SFP;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Opening_Balance_of_SFP);
                           break;
                       }
                   case EnumCFTransactionGroup.Other_Income:
                       {
                           oCFDataTypeObj.id = (int)EnumCFDataType.Other_Income_of_SCI;
                           oCFDataTypeObj.Value = EnumObject.jGet(EnumCFDataType.Other_Income_of_SCI);
                           break;
                       } 
                        
               }
                oCFDataTypeObjs.Add(oCFDataTypeObj);
            }
            catch (Exception ex)
            {
                oCFDataTypeObj = new EnumObject();
                oCFDataTypeObj.Value= ex.Message;
                oCFDataTypeObjs.Add(oCFDataTypeObj);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCFDataTypeObjs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByDataType(CashFlowSetup oCashFlowSetup)
        {
            _oChartsOfAccountList = new List<ChartsOfAccount>();
            try
            {
                string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE  AccountType = " + (int)EnumAccountType.SubGroup;
                string sComponent = "";
                switch ((EnumCFDataType)oCashFlowSetup.CFDataTypeInInt)
                {                    
                    case EnumCFDataType.Fixed_Asset_SFP_Changes_Less_Depreciation:
                    case EnumCFDataType.Fixed_Deposit_SFP_Changes:
                    case EnumCFDataType.Intengible_Asstes_SFP_Changes_Less_Depreciation:
                    case EnumCFDataType.Investment_SFP_Changes:
                    case EnumCFDataType.Current_Asset_Chnages_Of_SFP:
                    case EnumCFDataType.Opening_Balance_of_SFP:
                        {
                            sComponent = " AND ComponentID = 2";//asset
                            break;
                        }
                    case EnumCFDataType.None_Current_Loan_SFP_Changes:
                    case EnumCFDataType.Current_Loan_SFP_Changes:
                    case EnumCFDataType.Dividend_SFP_Changes:
                    case EnumCFDataType.Current_Libility_Chnages_Of_SFP:
                        {
                            sComponent = " AND ComponentID = 3";//liability
                            break;
                        }
                    case EnumCFDataType.Administrative_Cost_Of_SCI:
                    case EnumCFDataType.Selling_Cost_of_SCI:
                    case EnumCFDataType.Fixed_Asset_Depreciation_Cost_Of_SCI:
                    case EnumCFDataType.Intengible_Assets_Depreciation_Cost_Of_SCI:
                        {
                            sComponent = " AND ComponentID = 6";//Expenditure
                            break;
                        }
                }
                if (sComponent != "")
                {
                    sSQL += sComponent;
                    _oChartsOfAccountList = ChartsOfAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccountList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Save(CashFlowSetup oCashFlowSetup)
        {
            _oCashFlowSetup = new CashFlowSetup();
            try
            {
                switch ((EnumCFTransactionGroup)oCashFlowSetup.CFTransactionGroupInInt)
                {
                    case EnumCFTransactionGroup.Cash_Receipts:
                    case EnumCFTransactionGroup.Cash_Paid:
                    case EnumCFTransactionGroup.Interest_Paid:
                    case EnumCFTransactionGroup.Income_Tax_paid:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Operating_Activities;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Operating_Activities;
                            break;
                        }
                    case EnumCFTransactionGroup.Acquisition_of_Fixed_Asset:
                    case EnumCFTransactionGroup.Fixed_Doposit:
                    case EnumCFTransactionGroup.Acquisition_of_Intengible_Asstes:
                    case EnumCFTransactionGroup.Capital_WIP:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Investing_Activities;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Investing_Activities;
                            break;
                        }
                    case EnumCFTransactionGroup.Payment_of_Lease_Loan:
                    case EnumCFTransactionGroup.Payment_of_Term_Loan:
                    case EnumCFTransactionGroup.Payment_of_Dividend:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Financing_Activities;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Financing_Activities;
                            break;
                        }
                    case EnumCFTransactionGroup.Cost_of_Sales:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Expenses;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Expenses;
                            break;
                        }
                    case EnumCFTransactionGroup.Administrative_Cost:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Expenses;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Expenses;
                            break;
                        }
                    case EnumCFTransactionGroup.Selling_Cost:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Expenses;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Expenses;
                            break;
                        }
                    case EnumCFTransactionGroup.Other_Income:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Operating_Activities;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Operating_Activities;
                            break;
                        }
                    case EnumCFTransactionGroup.Current_Asset_Chnages:
                    case EnumCFTransactionGroup.Current_Libility_Chnages:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Changes_In_CA_AND_CL;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Changes_In_CA_AND_CL;
                            break;
                        }
                    case EnumCFTransactionGroup.Fixed_Asset_Depreciation_Cost:
                    case EnumCFTransactionGroup.Intengible_Assets_Depreciation_Cost:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Depreciation;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Depreciation;
                            break;
                        }
                    case EnumCFTransactionGroup.Opening_Balance:
                        {
                            oCashFlowSetup.CFTransactionCategory = EnumCFTransactionCategory.Opening_Balance;
                            oCashFlowSetup.CFTransactionCategoryInInt = (int)EnumCFTransactionCategory.Opening_Balance;
                            break;
                        }
                }
                _oCashFlowSetup = oCashFlowSetup;
                _oCashFlowSetup = _oCashFlowSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCashFlowSetup = new CashFlowSetup();
                _oCashFlowSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCashFlowSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				CashFlowSetup oCashFlowSetup = new CashFlowSetup();
				sFeedBackMessage = oCashFlowSetup.Delete(id, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 
		#endregion

        #region CashFlow Statement
        #region Set IncomeStatement SessionData
        [HttpPost]
        public ActionResult SetISSessionData(CashFlowSetup oCashFlowSetup)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oCashFlowSetup);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult ViewCashFlowStatement(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                _oCashFlowSetup = (CashFlowSetup)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oCashFlowSetup = null;
            }
            if (_oCashFlowSetup != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oCashFlowSetup.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oAccountingSession = oAccountingSession.GetSessionByDate(_oCashFlowSetup.EndDate, nUserID);
                if (oAccountingSession.AccountingSessionID > 0)
                {
                    if (oAccountingSession.StartDate > _oCashFlowSetup.StartDate)
                    {
                        _oCashFlowSetup.StartDate = oAccountingSession.StartDate;
                    }
                }
                else
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                    _oCashFlowSetup.StartDate = oAccountingSession.StartDate;
                    _oCashFlowSetup.EndDate = oAccountingSession.EndDate;
                }
                _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails, nUserID); 
                _oCashFlowSetup.CashFlowSetups = new List<CashFlowSetup>();
                _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;
            }
            else
            {
                _oCashFlowSetup = new CashFlowSetup();
                _oCashFlowSetup.CashFlowSetups = new List<CashFlowSetup>();
                oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                _oCashFlowSetup.StartDate = oAccountingSession.StartDate;
                _oCashFlowSetup.EndDate = oAccountingSession.EndDate;

                _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails, nUserID); 
                _oCashFlowSetup.CashFlowSetups = new List<CashFlowSetup>();
                _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowSetup.BUID, nUserID);
            _oCashFlowSetup.Company = oCompany;
            if (oBusinessUnit.BusinessUnitID > 0) { _oCashFlowSetup.Company.Name = oBusinessUnit.Name; }
            _oCashFlowSetup.SessionDate = _oCashFlowSetup.StartDateSt + " To " + _oCashFlowSetup.EndDateSt;

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(_oCashFlowSetup);
        }

        public ActionResult ViewCFPaymentDetails()
        {
            
            int nUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                _oCashFlowSetup = (CashFlowSetup)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oCashFlowSetup = null;
            }
            if (_oCashFlowSetup != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oCashFlowSetup.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oAccountingSession = oAccountingSession.GetSessionByDate(_oCashFlowSetup.EndDate, nUserID);
                if (oAccountingSession.AccountingSessionID > 0)
                {
                    if (oAccountingSession.StartDate > _oCashFlowSetup.StartDate)
                    {
                        _oCashFlowSetup.StartDate = oAccountingSession.StartDate;
                    }
                }
                else
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                    _oCashFlowSetup.StartDate = oAccountingSession.StartDate;
                    _oCashFlowSetup.EndDate = oAccountingSession.EndDate;
                }
                _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails, nUserID);
                _oCashFlowSetup.CashFlowSetups = new List<CashFlowSetup>();
                _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;
            }
            else
            {
                _oCashFlowSetup = new CashFlowSetup();
                _oCashFlowSetup.CashFlowSetups = new List<CashFlowSetup>();
                oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                _oCashFlowSetup.StartDate = oAccountingSession.StartDate;
                _oCashFlowSetup.EndDate = oAccountingSession.EndDate;

                _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails, nUserID);
                _oCashFlowSetup.CashFlowSetups = new List<CashFlowSetup>();
                _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowSetup.BUID, nUserID);
            _oCashFlowSetup.Company = oCompany;
            if (oBusinessUnit.BusinessUnitID > 0) { _oCashFlowSetup.Company.Name = oBusinessUnit.Name; }
            _oCashFlowSetup.SessionDate = _oCashFlowSetup.StartDateSt + " To " + _oCashFlowSetup.EndDateSt;

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oCashFlowSetup);
        }

        public ActionResult PrintCashFlowStatement(string Params)
        {
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            bool IsPaymentDetails  = false;
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            _oCashFlowSetup.BUID = nBUID;
            _oCashFlowSetup.StartDate = dStartDate;
            _oCashFlowSetup.EndDate = dEndDate;
            _oCashFlowSetup.IsPaymentDetails = IsPaymentDetails;
            _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails,((User)Session[SessionInfo.CurrentUser]).UserID); 
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oCashFlowSetup.SessionDate = _oCashFlowSetup.StartDateSt + " To " + _oCashFlowSetup.EndDateSt;
            _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;

            rptCashFlowStatement orptCashFlowStatementStatement = new rptCashFlowStatement();
            abytes = orptCashFlowStatementStatement.PrepareReport(_oCashFlowSetup, oCompany);

            return File(abytes, "application/pdf");
        }


        public ActionResult PrintCFPaymentDetails()
        {
            _oCashFlowSetup = (CashFlowSetup)Session[SessionInfo.ParamObj];

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(_oCashFlowSetup.BUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            bool IsPaymentDetails = true;
            byte[] abytes = null;
            _oCashFlowSetup.IsPaymentDetails = IsPaymentDetails;
            _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oCashFlowSetup.SessionDate = _oCashFlowSetup.StartDateSt + " To " + _oCashFlowSetup.EndDateSt;
            _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;

            rptCFPaymentDetails orptCFPaymentDetails = new rptCFPaymentDetails();
            abytes = orptCFPaymentDetails.PrepareReport(_oCashFlowSetup, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportCashFlowStatementToExcel(string Params)
        {

            #region Dataget
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            bool IsPaymentDetails = false;
           _oCashFlowSetup.BUID = nBUID;
            _oCashFlowSetup.StartDate = dStartDate;
            _oCashFlowSetup.EndDate = dEndDate;
            _oCashFlowSetup.IsPaymentDetails = IsPaymentDetails;
            _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oCashFlowSetup.SessionDate = _oCashFlowSetup.StartDateSt + " To " + _oCashFlowSetup.EndDateSt;
            _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Cash Flow Statement");
                sheet.Name = "Cash Flow Statement";
                sheet.Column(2).Width = 9;
                sheet.Column(3).Width = 60;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 6;
                nEndCol = 5;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Statement of Cash Flows"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "For the Year Ended " + _oCashFlowSetup.EndDateSt; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _oCashFlowSetup.SessionDate + " " + oCompany.CurrencyName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Column Header
                nStartRow = nRowIndex; nEndRow = nRowIndex;
                #endregion

                #region Report Data
               
               CashFlowSetup oNetIncreaseFromCashflow = new CashFlowSetup();
               CashFlowSetup oOpeningBalance = new CashFlowSetup();
               CashFlowSetup oClosingBalance = new CashFlowSetup();
               int nCFTransactionCategoryInInt=0;
               for (int i=0; i< _oCashFlowSetups.Count;i++)
               {
              
                    if(_oCashFlowSetups[i].CashFlowSetupID>1003)
                    {
                        if(_oCashFlowSetups[i].CashFlowSetupID==1004)
                        {
                            oNetIncreaseFromCashflow = _oCashFlowSetups[i];
                        }
                        else if(_oCashFlowSetups[i].CashFlowSetupID==1005)
                        {
                            oOpeningBalance = _oCashFlowSetups[i];
                        }
                        else if(_oCashFlowSetups[i].CashFlowSetupID==1006)
                        {
                            oClosingBalance = _oCashFlowSetups[i];
                        }
                    }
                else
                {
                    if(nCFTransactionCategoryInInt!=_oCashFlowSetups[i].CFTransactionCategoryInInt)
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        
                        
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = _oCashFlowSetups[i].SubGroupName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style =  border.Right.Style = ExcelBorderStyle.None;
                        border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = _oCashFlowSetups[i].DisplayCaption; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                       

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = " "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style =  ExcelBorderStyle.None;
                        border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                     i=i+1;
                    }

 
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = " ";  cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                    border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _oCashFlowSetups[i].DisplayCaption;
                    if (_oCashFlowSetups[i].CashFlowSetupID == 1000 || _oCashFlowSetups[i].CashFlowSetupID == 1001 || _oCashFlowSetups[i].CashFlowSetupID == 1002 || _oCashFlowSetups[i].CashFlowSetupID == 1003)
                    {
                         cell.Style.Font.Bold = true;
                    }
                    else
                    {   
                        
                        cell.Style.Font.Bold = false;
                    }
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    if(_oCashFlowSetups[i].CashFlowSetupID==1000 || _oCashFlowSetups[i].CashFlowSetupID==1001 || _oCashFlowSetups[i].CashFlowSetupID==1002 || _oCashFlowSetups[i].CashFlowSetupID==1003)
                    {


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oCashFlowSetups[i].Amount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.SingleAccounting;
                        border = cell.Style.Border;border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oCashFlowSetups[i].Amount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";  cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                    }
                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    nCFTransactionCategoryInInt =_oCashFlowSetups[i].CFTransactionCategoryInInt;
                }
               }

               #region net incease from cash flow
               cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style  = ExcelBorderStyle.None;
               border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               nRowIndex++;


               cell = sheet.Cells[nRowIndex, 2]; cell.Value = " "; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
               border.Left.Style = ExcelBorderStyle.Thin;

               cell = sheet.Cells[nRowIndex, 3]; cell.Value = oNetIncreaseFromCashflow.DisplayCaption; cell.Style.Font.Bold = true;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


               cell = sheet.Cells[nRowIndex, 4]; cell.Value = oNetIncreaseFromCashflow.Amount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";  cell.Style.Font.Bold = true;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

               cell = sheet.Cells[nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
               border.Right.Style = ExcelBorderStyle.Thin;
               nRowIndex++;

               #endregion

               #region Opening Balance
               cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
               border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               nRowIndex++;


               cell = sheet.Cells[nRowIndex, 2]; cell.Value = " "; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
               border.Left.Style = ExcelBorderStyle.Thin;

               cell = sheet.Cells[nRowIndex, 3]; cell.Value = oOpeningBalance.DisplayCaption; cell.Style.Font.Bold = true;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


               cell = sheet.Cells[nRowIndex, 4]; cell.Value = oOpeningBalance.Amount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

               cell = sheet.Cells[nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
               border.Right.Style = ExcelBorderStyle.Thin;
               nRowIndex++;

               #endregion

               #region Closing Balance
               cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
               border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               nRowIndex++;


               cell = sheet.Cells[nRowIndex, 2]; cell.Value = " "; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
               border.Left.Style = ExcelBorderStyle.Thin;

               cell = sheet.Cells[nRowIndex, 3]; cell.Value = oClosingBalance.DisplayCaption; cell.Style.Font.Bold = true;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


               cell = sheet.Cells[nRowIndex, 4]; cell.Value = oClosingBalance.Amount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

               cell = sheet.Cells[nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
               border.Right.Style = ExcelBorderStyle.Thin;
               nRowIndex++;
               #endregion

               cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
               cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
               border = cell.Style.Border;  border.Top.Style = ExcelBorderStyle.None;
               border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               nRowIndex++;
                #endregion

               cell = sheet.Cells[1, 1, nRowIndex, 7];
               fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
               fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=CashFlow_Statement.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public void ExportCFPaymentsToExcel()
        {

            #region Dataget
            _oCashFlowSetup = (CashFlowSetup)Session[SessionInfo.ParamObj];
            bool IsPaymentDetails = true;
            _oCashFlowSetup.IsPaymentDetails = IsPaymentDetails;
            _oCashFlowSetups = CashFlowSetup.Gets(_oCashFlowSetup.BUID, _oCashFlowSetup.StartDate, _oCashFlowSetup.EndDate, _oCashFlowSetup.IsPaymentDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oCashFlowSetup.SessionDate = _oCashFlowSetup.StartDateSt + " To " + _oCashFlowSetup.EndDateSt;
            _oCashFlowSetup.CashFlowSetups = _oCashFlowSetups;
            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Cash Flow Payment Details");
                sheet.Name = "Cash Flow Payment Details";
                sheet.Column(2).Width = 9;
                sheet.Column(3).Width = 60;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 6;
                nEndCol = 5;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Payment for Cost And Expenses"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "For the Year Ended " + _oCashFlowSetup.EndDateSt; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _oCashFlowSetup.SessionDate + " " + oCompany.CurrencyName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Column Header
                nStartRow = nRowIndex; nEndRow = nRowIndex;
                #endregion

                #region Report Data
                int nCFTransactionCategoryInInt = 0;
                for (int i = 0; i < _oCashFlowSetups.Count; i++)
                {

                    if (_oCashFlowSetups[i].CashFlowSetupID == 1003)
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;


                        
                    }
                    if (nCFTransactionCategoryInInt != _oCashFlowSetups[i].CFTransactionCategoryInInt)
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;


                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = " "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                        border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = _oCashFlowSetups[i].DisplayCaption; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = " "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                        border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        i = i + 1;
                    }


                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = " "; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                    border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _oCashFlowSetups[i].DisplayCaption;
                    if (_oCashFlowSetups[i].CashFlowSetupID == 1001 || _oCashFlowSetups[i].CashFlowSetupID == 1002 || _oCashFlowSetups[i].CashFlowSetupID == 1003 || _oCashFlowSetups[i].CashFlowSetupID == 1004)
                    {
                        cell.Style.Font.Bold = true;
                    }
                    else
                    {

                        cell.Style.Font.Bold = false;
                    }
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    if (_oCashFlowSetups[i].CashFlowSetupID == 1001 || _oCashFlowSetups[i].CashFlowSetupID == 1002 || _oCashFlowSetups[i].CashFlowSetupID == 1003 || _oCashFlowSetups[i].CashFlowSetupID == 1004)
                    {

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oCashFlowSetups[i].Amount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.SingleAccounting;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        border.Top.Style = ExcelBorderStyle.Thin;

                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oCashFlowSetups[i].Amount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                    }
                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    nCFTransactionCategoryInInt = _oCashFlowSetups[i].CFTransactionCategoryInInt;
                }
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.None;
                border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 7];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=CashFlow_Statement.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

	}

}
