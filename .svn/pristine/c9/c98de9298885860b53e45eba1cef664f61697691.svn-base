using System;
using System.Linq;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class IncomeStatementController : Controller
    {
        #region Declaration
        List<IncomeStatement> _oIncomeStatements = new List<IncomeStatement>();
        List<IncomeStatement> _oIncomeStatementsFor_PrvSession = new List<IncomeStatement>();
        IncomeStatement _oIncomeStatement = new IncomeStatement();
        List<IncomeStatement> _oRevenues = new List<IncomeStatement>();
        List<IncomeStatement> _oExpenses = new List<IncomeStatement>();
        List<IncomeStatement> _oRevenuesFor_Session = new List<IncomeStatement>();
        List<IncomeStatement> _oExpensesFor_Session = new List<IncomeStatement>();
        IncomeStatement _oIS = new IncomeStatement();
        TChartsOfAccount _oTChartsOfAccount = new TChartsOfAccount();
        List<TChartsOfAccount> _oTChartsOfAccounts = new List<TChartsOfAccount>();
        CIStatementSetup _oCIStatementSetup = new CIStatementSetup();
        List<CIStatementSetup> _oCIStatementSetups = new List<CIStatementSetup>();
        List<CIStatementSetup> _oTempCIStatementSetups = new List<CIStatementSetup>();
        #endregion
        #region Function
        private TChartsOfAccount GetRootByID(int nID)
        {
            TChartsOfAccount oTChartsOfAccount = new TChartsOfAccount();
            foreach (TChartsOfAccount oItem in _oTChartsOfAccounts)
            {
                if (oItem.id == nID)
                {
                    return oItem;
                }
            }
            return _oTChartsOfAccount;
        }
        private void AddTreeNodes(ref TChartsOfAccount oTChartsOfAccount)
        {
            List<TChartsOfAccount> oChildNodes;
            oChildNodes = GetChild(oTChartsOfAccount.id);
            oTChartsOfAccount.children = oChildNodes;

            foreach (TChartsOfAccount oItem in oChildNodes)
            {
                TChartsOfAccount oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private List<TChartsOfAccount> GetChild(int nAccountHeadID)
        {
            List<TChartsOfAccount> oTChartsOfAccounts = new List<TChartsOfAccount>();
            foreach (TChartsOfAccount oItem in _oTChartsOfAccounts)
            {
                if (oItem.parentid == nAccountHeadID)
                {
                    oTChartsOfAccounts.Add(oItem);
                }
            }
            return oTChartsOfAccounts;
        }
        #endregion

        #region New Version
        #region Set IncomeStatement SessionData
        [HttpPost]
        public ActionResult SetISSessionData(IncomeStatement oIncomeStatement)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oIncomeStatement);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Prepare Comprehensive Income
        private List<IncomeStatement> GetChildren(int nAccountHeadID)
        {
            List<IncomeStatement> oIncomeStatements = new List<IncomeStatement>();
            foreach (IncomeStatement oItem in _oIncomeStatements)
            {
                if (oItem.ParentAccountHeadID == nAccountHeadID)
                {
                    oIncomeStatements.Add(oItem);
                }
            }
            return oIncomeStatements;
        }
        private void AddNodes(IncomeStatement oIS)
        {
            List<IncomeStatement> oChildNodes = new List<IncomeStatement>();
            oChildNodes = GetChildren(oIS.AccountHeadID);
            foreach (IncomeStatement oItem in oChildNodes)
            {
                IncomeStatement oTemp = oItem;
                if (oItem.ComponentType == EnumComponentType.Income)
                {
                    _oRevenues.Add(oTemp);
                    _oRevenuesFor_Session.Add(oTemp);
                }
                else if (oItem.ComponentType == EnumComponentType.Expenditure)
                {
                    _oExpenses.Add(oTemp);
                    _oExpensesFor_Session.Add(oTemp);
                }
                AddNodes(oTemp);
            }
        }
        private List<CIStatementSetup> PrepareComprehensiveIncome(IncomeStatement oIncomeStatement, BusinessUnit oBusinessUnit)
        {
            int nUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;//default, because when it call from accountin ration Setup can't find setup
            Company oCompany = new Company();
            _oIncomeStatement = oIncomeStatement;
            oCompany = oIncomeStatement.Company;
            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();
            _oRevenuesFor_Session = new List<IncomeStatement>();
            _oExpensesFor_Session = new List<IncomeStatement>();
            _oCIStatementSetups = new List<CIStatementSetup>();
            List<CIStatementSetup> oTempCIStatementSetups = new List<CIStatementSetup>();
            CIStatementSetup oTempCIStatementSetup = new CIStatementSetup();
            IncomeStatement oIS = new IncomeStatement();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.Comprehensive_Income_Satatement_Format, nUserID);



            oIS.AccountHeadID = 1;
            this.AddNodes(oIS);

            _oIncomeStatement.Revenues = _oRevenues;
            _oIncomeStatement.Expenses = _oExpenses;
            _oCIStatementSetups = CIStatementSetup.Gets(nUserID);

            //EnumCISSetup { None = 0, Gross_Turnover = 1, Inventory_Head = 2, Purchase Material= 3, Overhead_Cost =  4, 
            //Operating_Expenses = 5,  Other_Income = 6,  Income_Tax = 7, Depreciation = 8}

            //None = 0,
            //Gross_Turnover = 1,//Income
            //Inventory_Head = 2, //Assest
            //Purchase_Material = 3, //Expenditure
            //Overhead_Cost = 4,//Expenditure
            //Working_Process = 5,//Asset
            //Finish_Goods = 6,//Asset
            //Operating_Expenses = 7,//Expenditure
            //Other_Income = 8,//Income
            //Income_Tax = 9,//Expenditure
            //Depreciation = 10, //Expenditur

            List<IncomeStatement> oTempStatements = new List<IncomeStatement>();
            foreach (CIStatementSetup oItem in _oCIStatementSetups)
            {
                if ("1,8".Contains(Convert.ToString((int)oItem.CIHeadType)))//income
                {
                    oTempStatements = new List<IncomeStatement>();
                    oTempStatements = _oRevenues.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList();
                    if (oTempStatements != null && oTempStatements.Count > 0)
                    {
                        oItem.Note = _oRevenues.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].AccountCode;
                        oItem.AccountHeadValue = _oRevenues.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].CGSGBalance;
                        oItem.AccountHeadValue_ForSession = _oRevenuesFor_Session.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].CGSGBalance_ForSession;
                    }
                }
                else if ("4,7,9,10".Contains(Convert.ToString((int)oItem.CIHeadType)))//expense
                {
                    oTempStatements = new List<IncomeStatement>();
                    oTempStatements = _oExpenses.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList();
                    if (oTempStatements != null && oTempStatements.Count > 0)
                    {
                        oItem.Note = _oExpenses.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].AccountCode;
                        oItem.AccountHeadValue = _oExpenses.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].CGSGBalance;
                        oItem.AccountHeadValue_ForSession = _oExpensesFor_Session.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].CGSGBalance_ForSession;
                    }
                }
            }
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oRevenues);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oExpenses);

            _oIncomeStatement.TotalRevenuesFor_PSession = IncomeStatement.ComponentBalanceForPSession(EnumComponentType.Income, _oRevenuesFor_Session);
            _oIncomeStatement.TotalExpensesFor_PSession = IncomeStatement.ComponentBalanceForPSession(EnumComponentType.Expenditure, _oExpensesFor_Session);

            double nGlobalTotal = 0, nGlobalTotalFor_PrivSession = 0;

            #region Trun Over
            int nCount = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).Count();//turnover
            if (nCount > 0)
            {
                if (nCount > 1)
                {
                    _oTempCIStatementSetups = new List<CIStatementSetup>();
                    _oTempCIStatementSetups = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).ToList();
                    foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                    {
                        oTempCIStatementSetup = new CIStatementSetup();
                        oTempCIStatementSetup.CIHeadType = EnumCISSetup.Gross_Turnover;
                        oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                        oTempCIStatementSetup.AccountHeadName = oItem.AccountHeadName;
                        oTempCIStatementSetup.Note = oItem.Note;
                        oTempCIStatementSetup.Label = 1;
                        oTempCIStatementSetup.AccountHeadValue = oItem.AccountHeadValue;
                        oTempCIStatementSetup.AccountHeadValue_ForSession = oItem.AccountHeadValue_ForSession;
                        oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    }
                    nGlobalTotal = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).Sum(x => x.AccountHeadValue);
                    nGlobalTotalFor_PrivSession = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).Sum(x => x.AccountHeadValue_ForSession);


                    #region TurnOver
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.AccountHeadName = "TURNOVER";//use in Accounting Ratio
                    oTempCIStatementSetup.Note = "";
                    oTempCIStatementSetup.Label = 3;
                    oTempCIStatementSetup.RatioComponent = EnumRatioComponent.Revenue;
                    oTempCIStatementSetup.AccountHeadValue = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).Sum(x => x.AccountHeadValue);
                    oTempCIStatementSetup.AccountHeadValue_ForSession = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).Sum(x => x.AccountHeadValue_ForSession);
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    #endregion
                }
                else
                {
                    #region TurnOver
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.CIHeadType = EnumCISSetup.Gross_Turnover;
                    oTempCIStatementSetup.AccountHeadName = "TURNOVER";
                    oTempCIStatementSetup.Note = "";
                    oTempCIStatementSetup.Label = 3;
                    oTempCIStatementSetup.RatioComponent = EnumRatioComponent.Revenue;//use in Accounting Ratio
                    oTempCIStatementSetup.AccountHeadValue = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).Sum(x => x.AccountHeadValue);
                    oTempCIStatementSetup.AccountHeadValue_ForSession = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Gross_Turnover).Sum(x => x.AccountHeadValue_ForSession);
                    nGlobalTotal = oTempCIStatementSetup.AccountHeadValue;
                    nGlobalTotalFor_PrivSession = oTempCIStatementSetup.AccountHeadValue_ForSession;
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    #endregion
                }
            }
            else
            {
                #region TurnOver
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Gross_Turnover;
                oTempCIStatementSetup.AccountHeadName = "TURNOVER";
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 3;
                oTempCIStatementSetup.RatioComponent = EnumRatioComponent.Revenue;//use in Accounting Ratio
                oTempCIStatementSetup.AccountHeadValue = nGlobalTotal;
                oTempCIStatementSetup.AccountHeadValue_ForSession = nGlobalTotalFor_PrivSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                #endregion
            }
            #endregion


            #region Cost of Goods Sold
            double nMaterialCost = 0, nCOGSAmount = 0, nMaterialCostFor_Session = 0, nCOGSAmountFor_Session = 0, nWIPChanges = 0, nWIPChangesFor_PSession = 0, nTotalOverHead = 0, nTotalOverHead_ForSession = 0;
            IncomeStatement oFG = new IncomeStatement();
            if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Manufactureing_Format)
            {
                #region Inventory_Head Level-01
                IncomeStatement oTempIncomeStatement = _oIncomeStatement.IncomeStatements.FirstOrDefault(x => x.AccountHeadID == 0 && x.CISSetup == EnumCISSetup.Inventory_Head);
                if (oTempIncomeStatement == null)
                {
                    oTempIncomeStatement = new IncomeStatement();
                }
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Opening Raw Material";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 1;
                oTempCIStatementSetup.AccountHeadValue = oTempIncomeStatement.OpenningBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oTempIncomeStatement.OpenningBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                nMaterialCost = oTempIncomeStatement.OpenningBalance;
                nMaterialCostFor_Session = oTempIncomeStatement.OpenningBalanceFor_PSession;
                #endregion

                #region Materials Purchase Level-01
                List<CIStatementSetup> oMaterialsCISs = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Purchase_Material).ToList();
                List<IncomeStatement> oTempIncomeStatements = new List<IncomeStatement>();
                foreach (CIStatementSetup oItem in oMaterialsCISs)
                {
                    List<IncomeStatement> oStatements = _oIncomeStatement.IncomeStatements.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList();
                    oTempIncomeStatements.AddRange(oStatements);
                }
                foreach (IncomeStatement oItem in oTempIncomeStatements)
                {
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                    oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                    oTempCIStatementSetup.CIHeadType = EnumCISSetup.Purchase_Material;
                    oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Purchase_Material;
                    oTempCIStatementSetup.Note = oItem.AccountCode;
                    oTempCIStatementSetup.AccountHeadValue = (oItem.DebitTransaction - oItem.PurchaseCreditTransaction);
                    oTempCIStatementSetup.AccountHeadValue_ForSession = (oItem.DebitTransactionFor_PSession - oItem.PurchaseCreditTransactionFor_PSession);
                    oTempCIStatementSetup.Label = 1;
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    nMaterialCost = nMaterialCost + (oItem.DebitTransaction - oItem.PurchaseCreditTransaction);
                    nMaterialCostFor_Session = nMaterialCostFor_Session + (oItem.DebitTransactionFor_PSession - oItem.PurchaseCreditTransactionFor_PSession);
                }
                #endregion

                #region Closing Stock of Materials Level-01
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Closing Raw Material";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 1;
                oTempCIStatementSetup.AccountHeadValue = oTempIncomeStatement.ClosingBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oTempIncomeStatement.ClosingBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                nMaterialCost = nMaterialCost - oTempIncomeStatement.ClosingBalance;
                nMaterialCostFor_Session = nMaterialCostFor_Session - oTempIncomeStatement.ClosingBalanceFor_PSession;
                #endregion

                #region Material Consumed
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Available for use";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 2;
                oTempCIStatementSetup.AccountHeadValue = nMaterialCost;
                oTempCIStatementSetup.AccountHeadValue_ForSession = nMaterialCostFor_Session;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                nGlobalTotal = nGlobalTotal - nMaterialCost;
                nCOGSAmount = nMaterialCost;
                nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - nMaterialCostFor_Session;
                nCOGSAmountFor_Session = nMaterialCostFor_Session;
                #endregion

                #region Working Process/WIP
                IncomeStatement oWIP = _oIncomeStatement.IncomeStatements.FirstOrDefault(x => x.AccountHeadID == 0 && x.CISSetup == EnumCISSetup.Working_Process);
                if (oWIP == null)
                {
                    oWIP = new IncomeStatement();
                }
                #region Opening
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Opening WIP";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Working_Process;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Working_Process;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 1;
                oTempCIStatementSetup.AccountHeadValue = oWIP.OpenningBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oWIP.OpenningBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                #endregion

                #region Closing
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Closing WIP";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Working_Process;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Working_Process;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 1;
                oTempCIStatementSetup.AccountHeadValue = oWIP.ClosingBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oWIP.ClosingBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                #endregion

                #region WIP Changes
                nWIPChanges = (oWIP.OpenningBalance - oWIP.ClosingBalance); nWIPChangesFor_PSession = (oWIP.OpenningBalanceFor_PSession - oWIP.ClosingBalanceFor_PSession);
                nGlobalTotal = nGlobalTotal - nWIPChanges; nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - nWIPChangesFor_PSession;
                nCOGSAmount = nCOGSAmount + nWIPChanges; nCOGSAmountFor_Session = nCOGSAmountFor_Session + nWIPChangesFor_PSession;

                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Material Used for Production";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Working_Process;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Working_Process;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 2;
                //oTempCIStatementSetup.AccountHeadValue = nWIPChanges;
                oTempCIStatementSetup.AccountHeadValue = (nWIPChanges + nMaterialCost);
                oTempCIStatementSetup.AccountHeadValue_ForSession = (nWIPChangesFor_PSession + nMaterialCostFor_Session);
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                #endregion
                #endregion

                #region Overhead
                nTotalOverHead = 0; nTotalOverHead_ForSession = 0;
                nCount = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Overhead_Cost).Count();//Overhead
                if (nCount > 0)
                {
                    _oTempCIStatementSetups = new List<CIStatementSetup>();
                    _oTempCIStatementSetups = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Overhead_Cost).ToList();//Overhead
                    foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                    {
                        oTempCIStatementSetup = new CIStatementSetup();
                        oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                        oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                        oTempCIStatementSetup.CIHeadType = EnumCISSetup.Overhead_Cost;
                        oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Overhead_Cost;
                        oTempCIStatementSetup.Note = oItem.Note;
                        oTempCIStatementSetup.Label = 1;
                        oTempCIStatementSetup.AccountHeadValue = oItem.AccountHeadValue;
                        oTempCIStatementSetup.AccountHeadValue_ForSession = oItem.AccountHeadValue_ForSession;
                        oTempCIStatementSetups.Add(oTempCIStatementSetup);
                        nGlobalTotal = nGlobalTotal - oItem.AccountHeadValue;
                        nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - oItem.AccountHeadValue_ForSession;
                        nTotalOverHead = nTotalOverHead + oItem.AccountHeadValue;
                        nTotalOverHead_ForSession = nTotalOverHead_ForSession + oItem.AccountHeadValue_ForSession;
                        nCOGSAmount = nCOGSAmount + oItem.AccountHeadValue;
                        nCOGSAmountFor_Session = nCOGSAmountFor_Session + oItem.AccountHeadValue_ForSession;
                    }
                }

                #region Total OverHead
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Total Production Cost";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Overhead_Cost;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Overhead_Cost;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 2;
                //oTempCIStatementSetup.AccountHeadValue = nTotalOverHead;
                oTempCIStatementSetup.AccountHeadValue = (nMaterialCost + nWIPChanges + nTotalOverHead);
                oTempCIStatementSetup.AccountHeadValue_ForSession = (nMaterialCostFor_Session + nWIPChangesFor_PSession + nTotalOverHead_ForSession);
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                #endregion

                #endregion

                #region Finish Goods/FG
                oFG = _oIncomeStatement.IncomeStatements.FirstOrDefault(x => x.AccountHeadID == 0 && x.CISSetup == EnumCISSetup.Finish_Goods);
                if (oFG == null)
                {
                    oFG = new IncomeStatement();
                }
                #region Opening
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Opening Finish Goods";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Finish_Goods;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Finish_Goods;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 1;
                oTempCIStatementSetup.AccountHeadValue = oFG.OpenningBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oFG.OpenningBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                #endregion

                #region Closing
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Closing Finish Goods";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Finish_Goods;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Finish_Goods;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 1;
                oTempCIStatementSetup.AccountHeadValue = oFG.ClosingBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oFG.ClosingBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                #endregion
                #endregion
            }
            else
            {
                //for Trading CIS  EnumCISSetup.Purchase_Material use as a Direct Cost/COGS
                #region Opening Inventory
                IncomeStatement oTempIncomeStatement = _oIncomeStatement.IncomeStatements.FirstOrDefault(x => x.AccountHeadID == 0);
                if (oTempIncomeStatement == null)
                {
                    oTempIncomeStatement = new IncomeStatement();
                }
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Opening Inventory";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 2;
                oTempCIStatementSetup.AccountHeadValue = oTempIncomeStatement.OpenningBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oTempIncomeStatement.OpenningBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                nMaterialCost = oTempIncomeStatement.OpenningBalance;
                nMaterialCostFor_Session = oTempIncomeStatement.OpenningBalanceFor_PSession;
                #endregion

                #region Inventory Purchase
                List<CIStatementSetup> oMaterialsCISs = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Purchase_Material).ToList();
                List<IncomeStatement> oTempIncomeStatements = new List<IncomeStatement>();
                foreach (CIStatementSetup oItem in oMaterialsCISs)
                {
                    List<IncomeStatement> oStatements = _oIncomeStatement.IncomeStatements.Where(x => x.AccountHeadID == oItem.AccountHeadID && x.ParentAccountHeadID == 0).ToList();
                    oTempIncomeStatements.AddRange(oStatements);
                }
                foreach (IncomeStatement oItem in oTempIncomeStatements)
                {
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                    oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                    oTempCIStatementSetup.CIHeadType = EnumCISSetup.Purchase_Material;
                    oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Purchase_Material;
                    oTempCIStatementSetup.Note = oItem.AccountCode;
                    oTempCIStatementSetup.AccountHeadValue = (oItem.DebitTransaction - oItem.PurchaseCreditTransaction);
                    oTempCIStatementSetup.AccountHeadValue_ForSession = (oItem.DebitTransactionFor_PSession - oItem.PurchaseCreditTransactionFor_PSession);
                    oTempCIStatementSetup.Label = 1;
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    nMaterialCost = nMaterialCost + (oItem.DebitTransaction - oItem.PurchaseCreditTransaction);
                    nMaterialCostFor_Session = nMaterialCostFor_Session + (oItem.DebitTransactionFor_PSession - oItem.PurchaseCreditTransactionFor_PSession);
                }
                #endregion

                #region Closing Inventory
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Closing Inventory";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 1;
                oTempCIStatementSetup.AccountHeadValue = oTempIncomeStatement.ClosingBalance;
                oTempCIStatementSetup.AccountHeadValue_ForSession = oTempIncomeStatement.ClosingBalanceFor_PSession;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                nMaterialCost = nMaterialCost - oTempIncomeStatement.ClosingBalance;
                nMaterialCostFor_Session = nMaterialCostFor_Session - oTempIncomeStatement.ClosingBalanceFor_PSession;
                #endregion

                #region Usages Inventory
                oTempCIStatementSetup = new CIStatementSetup();
                oTempCIStatementSetup.AccountHeadName = "   Usages Inventory";
                oTempCIStatementSetup.CIHeadType = EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Inventory_Head;
                oTempCIStatementSetup.Note = "";
                oTempCIStatementSetup.Label = 2;
                oTempCIStatementSetup.AccountHeadValue = nMaterialCost;
                oTempCIStatementSetup.AccountHeadValue_ForSession = nMaterialCostFor_Session;
                oTempCIStatementSetups.Add(oTempCIStatementSetup);
                //nGlobalTotal = nGlobalTotal - nMaterialCost;
                nCOGSAmount = nMaterialCost;
                //nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - nMaterialCostFor_Session;
                nCOGSAmountFor_Session = nMaterialCostFor_Session;
                #endregion


                #region Overhead
                nCount = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Overhead_Cost).Count();//Overhead
                if (nCount > 0)
                {
                    _oTempCIStatementSetups = new List<CIStatementSetup>();
                    _oTempCIStatementSetups = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Overhead_Cost).ToList();//Overhead
                    foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                    {
                        oTempCIStatementSetup = new CIStatementSetup();
                        oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                        oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                        oTempCIStatementSetup.CIHeadType = EnumCISSetup.Overhead_Cost;
                        oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Overhead_Cost;
                        oTempCIStatementSetup.Note = oItem.Note;
                        oTempCIStatementSetup.Label = 1;
                        oTempCIStatementSetup.AccountHeadValue = oItem.AccountHeadValue;
                        oTempCIStatementSetup.AccountHeadValue_ForSession = oItem.AccountHeadValue_ForSession;
                        oTempCIStatementSetups.Add(oTempCIStatementSetup);

                        nCOGSAmount = nCOGSAmount + oItem.AccountHeadValue;
                        nCOGSAmountFor_Session = nCOGSAmountFor_Session + oItem.AccountHeadValue_ForSession;

                        nTotalOverHead = nTotalOverHead + oItem.AccountHeadValue;
                        nTotalOverHead_ForSession = nTotalOverHead_ForSession + oItem.AccountHeadValue_ForSession;
                    }
                }
                #endregion

                nGlobalTotal = nGlobalTotal - nCOGSAmount;
                nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - nCOGSAmountFor_Session;
            }
            #endregion

            #region Cost of Goods Sold
            oFG = _oIncomeStatement.IncomeStatements.FirstOrDefault(x => x.AccountHeadID == 0 && x.CISSetup == EnumCISSetup.Finish_Goods);
            if (oFG == null)
            {
                oFG = new IncomeStatement();
            }

            double nFGChanges = (oFG.OpenningBalance - oFG.ClosingBalance), nFGChangesFor_PSession = (oFG.OpenningBalanceFor_PSession - oFG.ClosingBalanceFor_PSession);
            nGlobalTotal = nGlobalTotal - nFGChanges; nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - nFGChangesFor_PSession;
            nCOGSAmount = nCOGSAmount + nFGChanges; nCOGSAmountFor_Session = nCOGSAmountFor_Session + nFGChangesFor_PSession;

            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "   Cost of Goods Sold";
            oTempCIStatementSetup.CIHeadType = EnumCISSetup.Finish_Goods;
            oTempCIStatementSetup.CIHeadTypeInt = (int)EnumCISSetup.Finish_Goods;
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 2;
            //oTempCIStatementSetup.AccountHeadValue = nFGChanges;
            oTempCIStatementSetup.AccountHeadValue = (nMaterialCost + nWIPChanges + nTotalOverHead + nFGChanges);
            oTempCIStatementSetup.AccountHeadValue_ForSession = (nMaterialCostFor_Session + nWIPChangesFor_PSession + nTotalOverHead_ForSession + nFGChangesFor_PSession);
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion


            #region Set COGS Value
            foreach (CIStatementSetup oItem in oTempCIStatementSetups)
            {
                if (oItem.CIHeadType == EnumCISSetup.COGSCaption)
                {
                    oItem.AccountHeadValue = nCOGSAmount;
                    oItem.AccountHeadValue_ForSession = nCOGSAmountFor_Session;
                }
            }
            #endregion

            #region GROSS PROFIT/LOSS
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "GROSS PROFIT/LOSS";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.RatioComponent = EnumRatioComponent.GrossProfit;//use in Accounting Ratio
            oTempCIStatementSetup.AccountHeadValue = nGlobalTotal;
            oTempCIStatementSetup.AccountHeadValue_ForSession = nGlobalTotalFor_PrivSession;
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            #region Operating Expense
            #region Operation Expense Caption
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "OPERATING EXPENSES";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.AccountHeadValue = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Operating_Expenses).Sum(x => x.AccountHeadValue);
            oTempCIStatementSetup.AccountHeadValue_ForSession = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Operating_Expenses).Sum(x => x.AccountHeadValue_ForSession);
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            nCount = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Operating_Expenses).Count();//Operating exepense
            if (nCount > 0)
            {
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Operating_Expenses).ToList();
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.CIHeadType = EnumCISSetup.Operating_Expenses;
                    oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                    oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                    oTempCIStatementSetup.Note = oItem.Note;
                    oTempCIStatementSetup.Label = 1;
                    oTempCIStatementSetup.AccountHeadValue = oItem.AccountHeadValue;
                    oTempCIStatementSetup.AccountHeadValue_ForSession = oItem.AccountHeadValue_ForSession;
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    nGlobalTotal = nGlobalTotal - oItem.AccountHeadValue;
                    nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - oItem.AccountHeadValue_ForSession;
                }
            }
            #endregion

            #region  OPERATING PROFIT/(LOSS)
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "OPERATING PROFIT/(LOSS)";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.RatioComponent = EnumRatioComponent.OperatingProfit; //use in Accounting Ratio
            oTempCIStatementSetup.AccountHeadValue = nGlobalTotal;
            oTempCIStatementSetup.AccountHeadValue_ForSession = nGlobalTotalFor_PrivSession;
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            #region Other Income
            #region Other Income Caption
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "OTHER INCOME";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.AccountHeadValue = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Other_Income).Sum(x => x.AccountHeadValue);
            oTempCIStatementSetup.AccountHeadValue_ForSession = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Other_Income).Sum(x => x.AccountHeadValue_ForSession);
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            nCount = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Other_Income).Count();
            if (nCount > 0)
            {
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Other_Income).ToList();
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.CIHeadType = EnumCISSetup.Other_Income;
                    oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                    oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                    oTempCIStatementSetup.Note = oItem.Note;
                    oTempCIStatementSetup.Label = 1;
                    oTempCIStatementSetup.AccountHeadValue = oItem.AccountHeadValue;
                    oTempCIStatementSetup.AccountHeadValue_ForSession = oItem.AccountHeadValue_ForSession;
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    nGlobalTotal = nGlobalTotal + oItem.AccountHeadValue;
                    nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession + oItem.AccountHeadValue_ForSession;
                }
            }
            #endregion

            #region  PROFIT/(LOSS) BEFORE TAX & DEPRECIATION
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "PROFIT/(LOSS) BEFORE TAX & DEPRECIATION";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.AccountHeadValue = nGlobalTotal;
            oTempCIStatementSetup.AccountHeadValue_ForSession = nGlobalTotalFor_PrivSession;
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            #region Income Tax
            #region Income Tax Caption
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "INCOME TAX:";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.AccountHeadValue = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Income_Tax).Sum(x => x.AccountHeadValue);
            oTempCIStatementSetup.AccountHeadValue_ForSession = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Income_Tax).Sum(x => x.AccountHeadValue_ForSession);
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            nCount = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Income_Tax).Count();
            if (nCount > 0)
            {
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Income_Tax).ToList();
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.CIHeadType = EnumCISSetup.Income_Tax;
                    oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                    oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                    oTempCIStatementSetup.Note = oItem.Note;
                    oTempCIStatementSetup.Label = 1;
                    oTempCIStatementSetup.AccountHeadValue = oItem.AccountHeadValue;
                    oTempCIStatementSetup.AccountHeadValue_ForSession = oItem.AccountHeadValue_ForSession;
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    nGlobalTotal = nGlobalTotal - oItem.AccountHeadValue;
                    nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - oItem.AccountHeadValue_ForSession;
                }
            }
            #endregion

            #region  NET PROFIT/(LOSS) BEFORE DEPRECIATION
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "NET PROFIT/(LOSS) BEFORE DEPRECIATION";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.AccountHeadValue = nGlobalTotal;
            oTempCIStatementSetup.AccountHeadValue_ForSession = nGlobalTotalFor_PrivSession;
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            #region Depreciation
            #region Depreciation Caption
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "DEPRECIATION";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.AccountHeadValue = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Depreciation).Sum(x => x.AccountHeadValue);
            oTempCIStatementSetup.AccountHeadValue_ForSession = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Depreciation).Sum(x => x.AccountHeadValue_ForSession);
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            nCount = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Depreciation).Count();
            if (nCount > 0)
            {
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => x.CIHeadType == EnumCISSetup.Depreciation).ToList();
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    oTempCIStatementSetup = new CIStatementSetup();
                    oTempCIStatementSetup.CIHeadType = EnumCISSetup.Depreciation;
                    oTempCIStatementSetup.AccountHeadID = oItem.AccountHeadID;
                    oTempCIStatementSetup.AccountHeadName = "   " + oItem.AccountHeadName;
                    oTempCIStatementSetup.Note = oItem.Note;
                    oTempCIStatementSetup.Label = 1;
                    oTempCIStatementSetup.AccountHeadValue = oItem.AccountHeadValue;
                    oTempCIStatementSetup.AccountHeadValue_ForSession = oItem.AccountHeadValue_ForSession;
                    oTempCIStatementSetups.Add(oTempCIStatementSetup);
                    nGlobalTotal = nGlobalTotal - oItem.AccountHeadValue;
                    nGlobalTotalFor_PrivSession = nGlobalTotalFor_PrivSession - oItem.AccountHeadValue_ForSession;
                }
            }
            #endregion

            #region  NET PROFIT/(LOSS)
            oTempCIStatementSetup = new CIStatementSetup();
            oTempCIStatementSetup.AccountHeadName = "NET PROFIT/(LOSS)";
            oTempCIStatementSetup.Note = "";
            oTempCIStatementSetup.Label = 3;
            oTempCIStatementSetup.RatioComponent = EnumRatioComponent.NetProfit;//use in Accounting Ratio
            oTempCIStatementSetup.AccountHeadValue = nGlobalTotal;
            oTempCIStatementSetup.AccountHeadValue_ForSession = nGlobalTotalFor_PrivSession;
            oTempCIStatementSetups.Add(oTempCIStatementSetup);
            #endregion

            return oTempCIStatementSetups;
        }
        #endregion

        #region Comprehensive Income
        public ActionResult ViewComprehensiveIncome(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];

            //ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            //oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.Comprehensive_Income_Satatement_Format, (int)Session[SessionInfo.currentUserID]);
            //if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Trading_Format)
            //{
            //    return RedirectToAction("ViewComprehensiveIncomeTradingFormat", "IncomeStatement", new { menuid = menuid });
            //}
            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                _oIncomeStatement = (IncomeStatement)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oIncomeStatement = null;
            }

            if (_oIncomeStatement != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oIncomeStatement.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion
                _oIncomeStatement.AccountType = EnumAccountType.SubGroup != _oIncomeStatement.AccountType ? EnumAccountType.SubGroup : _oIncomeStatement.AccountType;
            }
            else
            {
                _oIncomeStatement = new IncomeStatement();
                _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
                oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                _oIncomeStatement.StartDate = oAccountingSession.StartDate;
                _oIncomeStatement.EndDate = oAccountingSession.EndDate;
                _oIncomeStatement.AccountType = EnumAccountType.SubGroup;

                //_oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, 0, (int)_oIncomeStatement.AccountType, nUserID); //5 = Ledger
                //_oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
                //_oIncomeStatement.IncomeStatements = _oIncomeStatements;
            }

            #region find Current & Privious Session
            List<AccountingSession> oTempAccountingSessions = new List<AccountingSession>();
            oAccountingSession = oAccountingSession.GetSessionByDate(_oIncomeStatement.EndDate, nUserID);

            string sSQL = "SELECT top 1 * FROM View_AccountingSession WHERE AccountingSessionID = ( SELECT Max(AccountingSessionID) FROM AccountingSession AS HH WHERE HH.SessionType = 1 AND HH.AccountingSessionID<" + oAccountingSession.AccountingSessionID + ")";
            oTempAccountingSessions = AccountingSession.Gets(sSQL, nUserID);

            DateTime dPSessionStartDate = DateTime.MinValue;
            DateTime dPSessionEndDate = DateTime.MinValue;
            if (oTempAccountingSessions.Count > 0)
            {
                _oIncomeStatement.PriviosSessoinName = oTempAccountingSessions[0].SessionName;
                dPSessionStartDate = oTempAccountingSessions[0].StartDate;
                dPSessionEndDate = oTempAccountingSessions[0].EndDate;
            }
            #endregion

            _oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, 0, (int)_oIncomeStatement.AccountType, nUserID); //5 = Ledger
            _oIncomeStatementsFor_PrvSession = IncomeStatement.Gets(_oIncomeStatement.BUID, dPSessionStartDate, dPSessionEndDate, 0, (int)_oIncomeStatement.AccountType, nUserID); //5 = Ledger
            _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
            _oIncomeStatement.IncomeStatementsFor_PrivSession = new List<IncomeStatement>();

            foreach (IncomeStatement oItem in _oIncomeStatementsFor_PrvSession)
            {
                if (_oIncomeStatements.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList().Count() <= 0)
                {
                    IncomeStatement oNewIncomeStatement = new IncomeStatement();
                    oNewIncomeStatement = oItem;
                    oNewIncomeStatement.OpenningBalanceFor_PSession = oItem.OpenningBalance;
                    oNewIncomeStatement.OpenningBalance = 0;
                    oNewIncomeStatement.ClosingBalanceFor_PSession = oItem.ClosingBalance;
                    oNewIncomeStatement.ClosingBalance = 0;
                    oNewIncomeStatement.DebitTransactionFor_PSession = oItem.DebitTransaction;
                    oNewIncomeStatement.DebitTransaction = 0;
                    oNewIncomeStatement.CreditTransactionFor_PSession = oItem.CreditTransaction;
                    oNewIncomeStatement.CreditTransaction = 0;
                    oNewIncomeStatement.PurchaseCreditTransactionFor_PSession = oItem.PurchaseCreditTransaction;
                    oNewIncomeStatement.PurchaseCreditTransaction = 0;
                    _oIncomeStatements.Add(oNewIncomeStatement);
                }
            }
            if (oTempAccountingSessions.Count > 0)
            {
                foreach (IncomeStatement oItem in _oIncomeStatements)
                {
                    IncomeStatement oIncomeStatement = new IncomeStatement();
                    oIncomeStatement = _oIncomeStatementsFor_PrvSession.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                    oItem.OpenningBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.OpenningBalance;
                    oItem.ClosingBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.ClosingBalance;
                    oItem.DebitTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.DebitTransaction;
                    oItem.CreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.CreditTransaction;
                    oItem.PurchaseCreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.PurchaseCreditTransaction;
                }
            }
            _oIncomeStatement.IncomeStatements = _oIncomeStatements;
            _oIncomeStatement.IncomeStatementsFor_PrivSession = _oIncomeStatementsFor_PrvSession;


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oIncomeStatement.BUID, nUserID);
            _oIncomeStatement.Company = oCompany;
            if (oBusinessUnit.BusinessUnitID > 0) { _oIncomeStatement.Company.Name = oBusinessUnit.Name; }
            _oIncomeStatement.SessionDate = _oIncomeStatement.StartDateSt + " To " + _oIncomeStatement.EndDateSt;
            _oIncomeStatement.CIStatementSetups = this.PrepareComprehensiveIncome(_oIncomeStatement, oBusinessUnit);


            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            //_oIncomeStatement.EndDate = oAccountingSession.EndDate;
            //oCompany.CompanyLogo = null;
            this.Session.Remove(SessionInfo.ParamObj);
            return View(_oIncomeStatement);
        }
        public ActionResult PrintComprehensiveIncome(string Params)
        {
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            #region find Current & Privious Session
            List<AccountingSession> oTempAccountingSessions = new List<AccountingSession>();
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dEndDate, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT top 1 * FROM View_AccountingSession WHERE AccountingSessionID = ( SELECT Max(AccountingSessionID) FROM AccountingSession AS HH WHERE HH.SessionType = 1 AND HH.AccountingSessionID<" + oAccountingSession.AccountingSessionID + ")";
            oTempAccountingSessions = AccountingSession.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            DateTime dPSessionStartDate = DateTime.MinValue;
            DateTime dPSessionEndDate = DateTime.MinValue;
            if (oTempAccountingSessions.Count > 0)
            {
                _oIncomeStatement.PriviosSessoinName = oTempAccountingSessions[0].SessionName;
                dPSessionStartDate = oTempAccountingSessions[0].StartDate;
                dPSessionEndDate = oTempAccountingSessions[0].EndDate;
            }
            #endregion

            _oIncomeStatement.BUID = nBUID;
            _oIncomeStatement.StartDate = dStartDate;
            _oIncomeStatement.EndDate = dEndDate;
            _oIncomeStatement.AccountType = EnumAccountType.SubGroup != _oIncomeStatement.AccountType ? EnumAccountType.SubGroup : _oIncomeStatement.AccountType;
            _oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatementsFor_PrvSession = IncomeStatement.Gets(_oIncomeStatement.BUID, dPSessionStartDate, dPSessionEndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
            _oIncomeStatement.IncomeStatementsFor_PrivSession = new List<IncomeStatement>();
            foreach (IncomeStatement oItem in _oIncomeStatementsFor_PrvSession)
            {
                if (_oIncomeStatements.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList().Count() <= 0)
                {
                    IncomeStatement oNewIncomeStatement = new IncomeStatement();
                    oNewIncomeStatement = oItem;
                    oNewIncomeStatement.OpenningBalanceFor_PSession = oItem.OpenningBalance;
                    oNewIncomeStatement.OpenningBalance = 0;
                    oNewIncomeStatement.ClosingBalanceFor_PSession = oItem.ClosingBalance;
                    oNewIncomeStatement.ClosingBalance = 0;
                    oNewIncomeStatement.DebitTransactionFor_PSession = oItem.DebitTransaction;
                    oNewIncomeStatement.DebitTransaction = 0;
                    oNewIncomeStatement.CreditTransactionFor_PSession = oItem.CreditTransaction;
                    oNewIncomeStatement.CreditTransaction = 0;
                    oNewIncomeStatement.PurchaseCreditTransactionFor_PSession = oItem.PurchaseCreditTransaction;
                    oNewIncomeStatement.PurchaseCreditTransaction = 0;
                    _oIncomeStatements.Add(oNewIncomeStatement);
                }
            }
            if (oTempAccountingSessions.Count > 0)
            {
                foreach (IncomeStatement oItem in _oIncomeStatements)
                {
                    IncomeStatement oIncomeStatement = new IncomeStatement();
                    oIncomeStatement = _oIncomeStatementsFor_PrvSession.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                    oItem.OpenningBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.OpenningBalance;
                    oItem.ClosingBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.ClosingBalance;
                    oItem.DebitTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.DebitTransaction;
                    oItem.CreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.CreditTransaction;
                    oItem.PurchaseCreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.PurchaseCreditTransaction;
                }
            }
            _oIncomeStatement.IncomeStatements = _oIncomeStatements;
            _oIncomeStatement.IncomeStatementsFor_PrivSession = _oIncomeStatementsFor_PrvSession;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oIncomeStatement.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oIncomeStatement.SessionDate = _oIncomeStatement.StartDateSt + " To " + _oIncomeStatement.EndDateSt;
            _oIncomeStatement.CIStatementSetups = this.PrepareComprehensiveIncome(_oIncomeStatement, oBusinessUnit);

            rptComprehensiveIncomeStatement orptComprehensiveIncomeStatement = new rptComprehensiveIncomeStatement();
            abytes = orptComprehensiveIncomeStatement.PrepareReport(_oIncomeStatement, oCompany);

            return File(abytes, "application/pdf");
        }
        public void ExportComprehensiveIncomeToExcel(string Params)
        {

            #region Dataget
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);

            #region find Current & Privious Session
            List<AccountingSession> oTempAccountingSessions = new List<AccountingSession>();
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dEndDate, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT top 1 * FROM View_AccountingSession WHERE AccountingSessionID = ( SELECT Max(AccountingSessionID) FROM AccountingSession AS HH WHERE HH.SessionType = 1 AND HH.AccountingSessionID<" + oAccountingSession.AccountingSessionID + ")";
            oTempAccountingSessions = AccountingSession.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            DateTime dPSessionStartDate = DateTime.MinValue;
            DateTime dPSessionEndDate = DateTime.MinValue;
            if (oTempAccountingSessions.Count > 0)
            {
                _oIncomeStatement.PriviosSessoinName = oTempAccountingSessions[0].SessionName;
                dPSessionStartDate = oTempAccountingSessions[0].StartDate;
                dPSessionEndDate = oTempAccountingSessions[0].EndDate;
            }
            #endregion

            _oIncomeStatement.BUID = nBUID;
            _oIncomeStatement.StartDate = dStartDate;
            _oIncomeStatement.EndDate = dEndDate;
            _oIncomeStatement.AccountType = EnumAccountType.SubGroup != _oIncomeStatement.AccountType ? EnumAccountType.SubGroup : _oIncomeStatement.AccountType;
            _oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatementsFor_PrvSession = IncomeStatement.Gets(_oIncomeStatement.BUID, dPSessionStartDate, dPSessionEndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
            _oIncomeStatement.IncomeStatementsFor_PrivSession = new List<IncomeStatement>();
            foreach (IncomeStatement oItem in _oIncomeStatementsFor_PrvSession)
            {
                if (_oIncomeStatements.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList().Count() <= 0)
                {
                    IncomeStatement oNewIncomeStatement = new IncomeStatement();
                    oNewIncomeStatement = oItem;
                    oNewIncomeStatement.OpenningBalanceFor_PSession = oItem.OpenningBalance;
                    oNewIncomeStatement.OpenningBalance = 0;
                    oNewIncomeStatement.ClosingBalanceFor_PSession = oItem.ClosingBalance;
                    oNewIncomeStatement.ClosingBalance = 0;
                    oNewIncomeStatement.DebitTransactionFor_PSession = oItem.DebitTransaction;
                    oNewIncomeStatement.DebitTransaction = 0;
                    oNewIncomeStatement.CreditTransactionFor_PSession = oItem.CreditTransaction;
                    oNewIncomeStatement.CreditTransaction = 0;
                    oNewIncomeStatement.PurchaseCreditTransactionFor_PSession = oItem.PurchaseCreditTransaction;
                    oNewIncomeStatement.PurchaseCreditTransaction = 0;
                    _oIncomeStatements.Add(oNewIncomeStatement);
                }
            }
            if (oTempAccountingSessions.Count > 0)
            {
                foreach (IncomeStatement oItem in _oIncomeStatements)
                {
                    IncomeStatement oIncomeStatement = new IncomeStatement();
                    oIncomeStatement = _oIncomeStatementsFor_PrvSession.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                    oItem.OpenningBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.OpenningBalance;
                    oItem.ClosingBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.ClosingBalance;
                    oItem.DebitTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.DebitTransaction;
                    oItem.CreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.CreditTransaction;
                    oItem.PurchaseCreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.PurchaseCreditTransaction;
                }
            }
            _oIncomeStatement.IncomeStatements = _oIncomeStatements;
            _oIncomeStatement.IncomeStatementsFor_PrivSession = _oIncomeStatementsFor_PrvSession;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oIncomeStatement.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oIncomeStatement.SessionDate = _oIncomeStatement.StartDateSt + " To " + _oIncomeStatement.EndDateSt;
            _oIncomeStatement.CIStatementSetups = this.PrepareComprehensiveIncome(_oIncomeStatement, oBusinessUnit);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange PreviousCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Comprehensive Income");
                sheet.Name = "Comprehensive Income";
                sheet.Column(2).Width = 60;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 35;
                sheet.Column(7).Width = 8;
                sheet.Column(8).Width = 35;
                nEndCol = 8;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Statement of Comprehensive Income"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "For the Year Ended " + _oIncomeStatement.EndDateFullSt; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                cell.Value = " "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[nRowIndex, 6];
                cell.Value = _oIncomeStatement.SessionDate + " " + oCompany.CurrencyName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[nRowIndex, 7];
                cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[nRowIndex, 8];
                cell.Value = _oIncomeStatement.PriviosSessoinName != "" ? _oIncomeStatement.PriviosSessoinName + " " + oCompany.CurrencyName : ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex; nEndRow = nRowIndex;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Notes"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0; bool IsFirst = true;
                Double nClosing = 0;
                foreach (CIStatementSetup oItem in _oIncomeStatement.CIStatementSetups)
                {
                    string sACCName = oItem.AccountHeadName.ToUpper();
                    bool bIsTotal = (sACCName == "NET TURNOVER" || sACCName == "GROSS PROFIT" || sACCName == "PROFIT FROM OPERATIONS" || sACCName == "PROFIT AFTER FINANCIAL COST" || sACCName == "PROFIT BEFORE WPPF" || sACCName == "PROFIT BEFORE TAX" || sACCName == "PROFIT AFTER TAX" || sACCName == "PROFIT FOR THE YEAR" || sACCName == "TOTAL COMPHREHENSIVE INCOME FOR THE YEAR");

                    if (oItem.Label == 3)
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Value = ""; cell.Merge = true;
                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.AccountHeadName; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    if (oItem.Label == 3)
                    {
                        cell.Style.Font.Bold = true;
                    }
                    else if (oItem.Label == 2)
                    {
                        cell.Style.Font.Bold = true;
                    }
                    else
                    {
                        cell.Style.Font.Bold = false;
                    }

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.Note; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    if (oItem.Label == 1)
                    {
                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AccountHeadValue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        if (IsFirst)
                        {
                            border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            border.Bottom.Style = ExcelBorderStyle.None;
                            //IsFirst = false;
                        }
                        else
                        {
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        }
                        if (_oIncomeStatement.CIStatementSetups[nCount + 1].Label == 3 || _oIncomeStatement.CIStatementSetups[nCount + 1].Label == 2)
                        {
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (border.Top.Style != ExcelBorderStyle.Thin)
                                border.Top.Style = ExcelBorderStyle.None;
                            //IsFirst = true;
                        }


                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = " "; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.AccountHeadValue_ForSession; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        if (IsFirst)
                        {
                            border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            border.Bottom.Style = ExcelBorderStyle.None;
                            IsFirst = false;
                        }
                        else
                        {
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        }
                        if (_oIncomeStatement.CIStatementSetups[nCount + 1].Label == 3 || _oIncomeStatement.CIStatementSetups[nCount + 1].Label == 2)
                        {
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (border.Top.Style != ExcelBorderStyle.Thin)
                                border.Top.Style = ExcelBorderStyle.None;
                            IsFirst = true;
                        }


                    }
                    else if (oItem.Label == 2)
                    {
                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "Text";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AccountHeadValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.AccountHeadValue_ForSession; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[++nRowIndex, 2, nRowIndex, 8]; cell.Value = ""; cell.Merge = true;

                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AccountHeadValue; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.AccountHeadValue_ForSession; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    nEndRow = nRowIndex;
                    nRowIndex++;
                    if (bIsTotal)
                    {
                        if (sACCName != "PROFIT FOR THE YEAR")
                        {
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }
                    }
                    nCount++;
                }
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Comprehensive_Income.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Notes of Comprehensive Income
        public ActionResult ViewNotesOfComprehensiveIncome(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];

            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();
            _oCIStatementSetups = new List<CIStatementSetup>();
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                _oCIStatementSetups = CIStatementSetup.Gets(nUserID);
                _oIncomeStatement = (IncomeStatement)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oIncomeStatement = null;
            }
            if (_oIncomeStatement != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oIncomeStatement.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                //oAccountingSession = oAccountingSession.GetSessionByDate(_oIncomeStatement.EndDate, nUserID);
                //if (oAccountingSession.AccountingSessionID > 0)
                //{
                //    if (oAccountingSession.StartDate > _oIncomeStatement.StartDate)
                //    {
                //        _oIncomeStatement.StartDate = oAccountingSession.StartDate;
                //    }
                //}
                //else
                //{
                //    oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                //    _oIncomeStatement.StartDate = oAccountingSession.StartDate;
                //    _oIncomeStatement.EndDate = oAccountingSession.EndDate;
                //}

                _oIncomeStatement.AccountType = EnumAccountType.Ledger != _oIncomeStatement.AccountType ? EnumAccountType.Ledger : _oIncomeStatement.AccountType;
                _oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, _oIncomeStatement.ParentAccountHeadID, (int)_oIncomeStatement.AccountType, nUserID); //5 = Ledger
                if (IsMaterialPurchase(_oIncomeStatement.ParentAccountHeadID, _oCIStatementSetups))
                {
                    foreach (IncomeStatement oItem in _oIncomeStatements)
                    {
                        oItem.ClosingBalance = oItem.DebitTransaction;
                        oItem.ClosingBalanceFor_PSession = oItem.DebitTransactionFor_PSession;
                    }
                }
                _oIncomeStatements = _oIncomeStatements.Where(x => x.ClosingBalance != 0).ToList();
                _oIncomeStatements = _oIncomeStatements.OrderBy(x => x.AccountType).ThenBy(x => x.AccountHeadID).ToList();
                _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
                _oIncomeStatement.IncomeStatements = _oIncomeStatements;
            }
            else
            {
                _oIncomeStatement = new IncomeStatement();
                _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
                oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                _oIncomeStatement.StartDate = oAccountingSession.StartDate;
                _oIncomeStatement.EndDate = oAccountingSession.EndDate;

                _oIncomeStatement.AccountType = EnumAccountType.Ledger;
                _oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, _oIncomeStatement.ParentAccountHeadID, (int)_oIncomeStatement.AccountType, nUserID); //5 = Ledger                
                if (IsMaterialPurchase(_oIncomeStatement.ParentAccountHeadID, _oCIStatementSetups))
                {
                    foreach (IncomeStatement oItem in _oIncomeStatements)
                    {
                        oItem.ClosingBalance = oItem.DebitTransaction;
                        oItem.ClosingBalanceFor_PSession = oItem.DebitTransactionFor_PSession;
                    }
                }
                _oIncomeStatements = _oIncomeStatements.Where(x => x.ClosingBalance != 0).ToList();
                _oIncomeStatements = _oIncomeStatements.OrderBy(x => x.AccountType).ThenBy(x => x.AccountHeadID).ToList();
                _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
                _oIncomeStatement.IncomeStatements = _oIncomeStatements;
            }




            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oIncomeStatement.BUID, nUserID);
            _oIncomeStatement.Company = oCompany;
            if (oBusinessUnit.BusinessUnitID > 0) { _oIncomeStatement.Company.Name = oBusinessUnit.Name; }
            _oIncomeStatement.SessionDate = _oIncomeStatement.StartDateSt + " To " + _oIncomeStatement.EndDateSt;
            //_oIncomeStatement.CIStatementSetups = this.PrepareComprehensiveIncome(_oIncomeStatement, oBusinessUnit);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            //_oIncomeStatement.EndDate = oAccountingSession.EndDate;
            //oCompany.CompanyLogo = null;


            this.Session.Remove(SessionInfo.ParamObj);
            return View(_oIncomeStatement);
        }

        public bool IsMaterialPurchase(int nAccountHeadID, List<CIStatementSetup> oCIStatementSetups)
        {
            foreach (CIStatementSetup oItem in oCIStatementSetups)
            {
                if (oItem.AccountHeadID == nAccountHeadID && oItem.CIHeadType == EnumCISSetup.Purchase_Material)
                {
                    return true;
                }
            }
            return false;
        }

        public ActionResult PrintNotesOfComprehensiveIncome(string Params)
        {
            _oCIStatementSetups = new List<CIStatementSetup>();
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nParentAccountHeadID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            #region find Current & Privious Session
            List<AccountingSession> oTempAccountingSessions = new List<AccountingSession>();
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dEndDate, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT top 1 * FROM View_AccountingSession WHERE AccountingSessionID = ( SELECT Max(AccountingSessionID) FROM AccountingSession AS HH WHERE HH.SessionType = 1 AND HH.AccountingSessionID<" + oAccountingSession.AccountingSessionID + ")";
            oTempAccountingSessions = AccountingSession.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            DateTime dPSessionStartDate = DateTime.MinValue;
            DateTime dPSessionEndDate = DateTime.MinValue;
            if (oTempAccountingSessions.Count > 0)
            {
                _oIncomeStatement.PriviosSessoinName = oTempAccountingSessions[0].SessionName;
                dPSessionStartDate = oTempAccountingSessions[0].StartDate;
                dPSessionEndDate = oTempAccountingSessions[0].EndDate;
            }
            #endregion

            _oIncomeStatement.BUID = nBUID;
            _oIncomeStatement.StartDate = dStartDate;
            _oIncomeStatement.EndDate = dEndDate;
            _oIncomeStatement.AccountType = EnumAccountType.Ledger;
            _oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatementsFor_PrvSession = IncomeStatement.Gets(_oIncomeStatement.BUID, dPSessionStartDate, dPSessionEndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
            _oIncomeStatement.IncomeStatementsFor_PrivSession = new List<IncomeStatement>();
            foreach (IncomeStatement oItem in _oIncomeStatementsFor_PrvSession)
            {
                if (_oIncomeStatements.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList().Count() <= 0)
                {
                    IncomeStatement oNewIncomeStatement = new IncomeStatement();
                    oNewIncomeStatement = oItem;
                    oNewIncomeStatement.OpenningBalanceFor_PSession = oItem.OpenningBalance;
                    oNewIncomeStatement.OpenningBalance = 0;
                    oNewIncomeStatement.ClosingBalanceFor_PSession = oItem.ClosingBalance;
                    oNewIncomeStatement.ClosingBalance = 0;
                    oNewIncomeStatement.DebitTransactionFor_PSession = oItem.DebitTransaction;
                    oNewIncomeStatement.DebitTransaction = 0;
                    oNewIncomeStatement.CreditTransactionFor_PSession = oItem.CreditTransaction;
                    oNewIncomeStatement.CreditTransaction = 0;
                    oNewIncomeStatement.PurchaseCreditTransactionFor_PSession = oItem.PurchaseCreditTransaction;
                    oNewIncomeStatement.PurchaseCreditTransaction = 0;
                    _oIncomeStatements.Add(oNewIncomeStatement);
                }
            }
            if (oTempAccountingSessions.Count > 0)
            {
                foreach (IncomeStatement oItem in _oIncomeStatements)
                {
                    IncomeStatement oIncomeStatement = new IncomeStatement();
                    oIncomeStatement = _oIncomeStatementsFor_PrvSession.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                    oItem.OpenningBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.OpenningBalance;
                    oItem.ClosingBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.ClosingBalance;
                    oItem.DebitTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.DebitTransaction;
                    oItem.CreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.CreditTransaction;
                    oItem.PurchaseCreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.PurchaseCreditTransaction;
                }
            }
            _oIncomeStatement.IncomeStatements = _oIncomeStatements;
            _oIncomeStatement.IncomeStatementsFor_PrivSession = _oIncomeStatementsFor_PrvSession;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oIncomeStatement.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oIncomeStatement.SessionDate = _oIncomeStatement.StartDateSt + " To " + _oIncomeStatement.EndDateSt;
            _oIncomeStatement.CIStatementSetups = this.PrepareComprehensiveIncome(_oIncomeStatement, oBusinessUnit);


            #region Old Code
            //#region Check Authorize Business Unit
            //if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            //{
            //    rptErrorMessage oErrorReport = new rptErrorMessage();
            //    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
            //    return File(aErrorMessagebytes, "application/pdf");
            //}
            //#endregion

            //_oIncomeStatement.BUID = nBUID;
            //_oIncomeStatement.StartDate = dStartDate;
            //_oIncomeStatement.EndDate = dEndDate;
            //_oIncomeStatement.ParentAccountHeadID = nParentAccountHeadID;
            //_oIncomeStatement.AccountType = EnumAccountType.Ledger;
            //_oCIStatementSetups = CIStatementSetup.Gets((int)Session[SessionInfo.currentUserID]);
            //_oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, _oIncomeStatement.ParentAccountHeadID, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            //if (IsMaterialPurchase(_oIncomeStatement.ParentAccountHeadID, _oCIStatementSetups))
            //{
            //    foreach (IncomeStatement oItem in _oIncomeStatements)
            //    {
            //        oItem.ClosingBalance = oItem.DebitTransaction;
            //        oItem.ClosingBalanceFor_PSession = oItem.DebitTransactionFor_PSession;
            //    }
            //}
            //_oIncomeStatements.Where(x => x.ClosingBalance != 0);
            //_oIncomeStatements = _oIncomeStatements.Where(x => x.ClosingBalance != 0).ToList();
            //_oIncomeStatements = _oIncomeStatements.OrderBy(x => x.AccountType).ThenBy(x => x.AccountHeadID).ToList();
            //_oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
            //_oIncomeStatement.IncomeStatements = _oIncomeStatements;

            //Company oCompany = new Company();
            //oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //BusinessUnit oBusinessUnit = new BusinessUnit();
            //oBusinessUnit = oBusinessUnit.Get(_oIncomeStatement.BUID, (int)Session[SessionInfo.currentUserID]);
            //if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            //_oIncomeStatement.SessionDate = _oIncomeStatement.StartDateSt + " To " + _oIncomeStatement.EndDateSt;
            #endregion

            rptNotesComprehensiveIncomeStatement orptNotesComprehensiveIncomeStatement = new rptNotesComprehensiveIncomeStatement();
            abytes = orptNotesComprehensiveIncomeStatement.PrepareReport(_oIncomeStatement, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportNotesOfComprehensiveIncomeToExcel(string Params)
        {

            _oCIStatementSetups = new List<CIStatementSetup>();
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nParentAccountHeadID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            byte[] abytes = null;

            #region find Current & Privious Session
            List<AccountingSession> oTempAccountingSessions = new List<AccountingSession>();
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dEndDate, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT top 1 * FROM View_AccountingSession WHERE AccountingSessionID = ( SELECT Max(AccountingSessionID) FROM AccountingSession AS HH WHERE HH.SessionType = 1 AND HH.AccountingSessionID<" + oAccountingSession.AccountingSessionID + ")";
            oTempAccountingSessions = AccountingSession.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            DateTime dPSessionStartDate = DateTime.MinValue;
            DateTime dPSessionEndDate = DateTime.MinValue;
            if (oTempAccountingSessions.Count > 0)
            {
                _oIncomeStatement.PriviosSessoinName = oTempAccountingSessions[0].SessionName;
                dPSessionStartDate = oTempAccountingSessions[0].StartDate;
                dPSessionEndDate = oTempAccountingSessions[0].EndDate;
            }
            #endregion

            _oIncomeStatement.BUID = nBUID;
            _oIncomeStatement.StartDate = dStartDate;
            _oIncomeStatement.EndDate = dEndDate;
            _oIncomeStatement.AccountType = EnumAccountType.Ledger;
            _oIncomeStatements = IncomeStatement.Gets(_oIncomeStatement.BUID, _oIncomeStatement.StartDate, _oIncomeStatement.EndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatementsFor_PrvSession = IncomeStatement.Gets(_oIncomeStatement.BUID, dPSessionStartDate, dPSessionEndDate, 0, (int)_oIncomeStatement.AccountType, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatement.IncomeStatements = new List<IncomeStatement>();
            _oIncomeStatement.IncomeStatementsFor_PrivSession = new List<IncomeStatement>();
            foreach (IncomeStatement oItem in _oIncomeStatementsFor_PrvSession)
            {
                if (_oIncomeStatements.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList().Count() <= 0)
                {
                    IncomeStatement oNewIncomeStatement = new IncomeStatement();
                    oNewIncomeStatement = oItem;
                    oNewIncomeStatement.OpenningBalanceFor_PSession = oItem.OpenningBalance;
                    oNewIncomeStatement.OpenningBalance = 0;
                    oNewIncomeStatement.ClosingBalanceFor_PSession = oItem.ClosingBalance;
                    oNewIncomeStatement.ClosingBalance = 0;
                    oNewIncomeStatement.DebitTransactionFor_PSession = oItem.DebitTransaction;
                    oNewIncomeStatement.DebitTransaction = 0;
                    oNewIncomeStatement.CreditTransactionFor_PSession = oItem.CreditTransaction;
                    oNewIncomeStatement.CreditTransaction = 0;
                    oNewIncomeStatement.PurchaseCreditTransactionFor_PSession = oItem.PurchaseCreditTransaction;
                    oNewIncomeStatement.PurchaseCreditTransaction = 0;
                    _oIncomeStatements.Add(oNewIncomeStatement);
                }
            }
            if (oTempAccountingSessions.Count > 0)
            {
                foreach (IncomeStatement oItem in _oIncomeStatements)
                {
                    IncomeStatement oIncomeStatement = new IncomeStatement();
                    oIncomeStatement = _oIncomeStatementsFor_PrvSession.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                    oItem.OpenningBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.OpenningBalance;
                    oItem.ClosingBalanceFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.ClosingBalance;
                    oItem.DebitTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.DebitTransaction;
                    oItem.CreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.CreditTransaction;
                    oItem.PurchaseCreditTransactionFor_PSession = oIncomeStatement == null ? 0 : oIncomeStatement.PurchaseCreditTransaction;
                }
            }
            _oIncomeStatement.IncomeStatements = _oIncomeStatements;
            _oIncomeStatement.IncomeStatementsFor_PrivSession = _oIncomeStatementsFor_PrvSession;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oIncomeStatement.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oIncomeStatement.SessionDate = _oIncomeStatement.StartDateSt + " To " + _oIncomeStatement.EndDateSt;
            _oIncomeStatement.CIStatementSetups = this.PrepareComprehensiveIncome(_oIncomeStatement, oBusinessUnit);

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Comprehensive Income");
                sheet.Name = "Comprehensive Income";
                sheet.Column(2).Width = 15;
                sheet.Column(3).Width = 60;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                nEndCol = 5;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Note of Comprehensive Income Statement"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "For the Year Ended " + _oIncomeStatement.EndDateFullSt; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;              
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex; nEndRow = nRowIndex;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Notes"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Particulars"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oIncomeStatement.SessionDate; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = _oIncomeStatement.PriviosSessoinName; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Revenue

                #region Title Print
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "0000001"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Revenue"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion

                #region Revenue Data
                double nAccountHeadValue = 0, nAccountHeadValueSession = 0;
                foreach (CIStatementSetup oItem in _oCIStatementSetups)
                {
                    if (oItem.CIHeadType == EnumCISSetup.Gross_Turnover)
                    {

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.AccountHeadName.Trim(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.AccountHeadValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nAccountHeadValue += oItem.AccountHeadValue;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadValue_ForSession; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nAccountHeadValueSession += oItem.AccountHeadValue_ForSession;
                        nRowIndex++;
                    }
                }
                #endregion

                #region Total print of Subgroup
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nAccountHeadValue; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nAccountHeadValueSession; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion
                #endregion

                #region Blank Row
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion

                #region Cost Of Goods Sold

                #region Title Print
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "0000002"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Cost Of Goods Sold"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion

                #region Cost Of Goods Sold Data
                nAccountHeadValue = 0; nAccountHeadValueSession = 0;
                foreach (CIStatementSetup oItem in _oCIStatementSetups)
                {
                    if (oItem.CIHeadType == EnumCISSetup.Inventory_Head || oItem.CIHeadType == EnumCISSetup.Purchase_Material || oItem.CIHeadType == EnumCISSetup.Overhead_Cost || oItem.CIHeadType == EnumCISSetup.Finish_Goods)
                    {
                        if (oItem.AccountHeadName.Trim() == "Cost of Goods Sold")
                        {
                            nAccountHeadValue = oItem.AccountHeadValue;
                            nAccountHeadValueSession = oItem.AccountHeadValue_ForSession;

                        }
                        else
                        {
                            bool bIsBoldFont = false;
                            if (oItem.AccountHeadName.Trim() == "Opening Inventory" || oItem.AccountHeadName.Trim() == "Usages Inventory")
                            {
                                bIsBoldFont = true;
                            }

                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = bIsBoldFont;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.AccountHeadName.Trim(); cell.Style.Font.Bold = bIsBoldFont;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.AccountHeadValue; cell.Style.Font.Bold = bIsBoldFont; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadValue_ForSession; cell.Style.Font.Bold = bIsBoldFont; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;                            
                            nRowIndex++;
                        }

                    }
                }
                #endregion

                #region Total print of Subgroup
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nAccountHeadValue; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nAccountHeadValueSession; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion

                #endregion

                #region Blank Row
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion
                
                #region Rest of the all Data 
                foreach (CIStatementSetup oItem in _oIncomeStatement.CIStatementSetups)
                {
                    if (oItem.Label == 1 && (oItem.CIHeadType == EnumCISSetup.Operating_Expenses || oItem.CIHeadType == EnumCISSetup.Other_Income || oItem.CIHeadType == EnumCISSetup.Income_Tax || oItem.CIHeadType == EnumCISSetup.Depreciation))
                    {
                        List<IncomeStatement> oIncomeStatements = new List<IncomeStatement>();
                        oIncomeStatements = _oIncomeStatements.Where(x => x.ParentAccountHeadID == oItem.AccountHeadID).ToList();

                        #region Blank Row
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion

                        #region Title Print
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.Note; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.AccountHeadName.Trim(); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion

                        foreach (IncomeStatement oChildItem in oIncomeStatements)
                        {
                            if (Math.Round(oChildItem.ClosingBalance) != 0 || Math.Round(oChildItem.ClosingBalanceFor_PSession) != 0)
                            {
                                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                                cell = sheet.Cells[nRowIndex, 3]; cell.Value = oChildItem.AccountHeadName.Trim(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oChildItem.ClosingBalance; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                                
                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = oChildItem.ClosingBalanceFor_PSession; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                                nRowIndex++;
                            }
                        }


                        #region Total print of Subgroup
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.AccountHeadValue; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadValue_ForSession; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,###;(#,##,###)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion
                    }
                }
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Comprehensive_Income.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
        #endregion

        public ActionResult RptIncomeStatement(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oIncomeStatement = new IncomeStatement();
            _oIncomeStatements = new List<IncomeStatement>();
            _oIncomeStatement.Revenues = IncomeStatement.GetStatements(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.Expenses = IncomeStatement.GetStatements(EnumComponentType.Expenditure, _oIncomeStatements);
            _oIncomeStatement.AccountingSessions = AccountingSession.GetRunningFreezeAccountingYear((int)Session[SessionInfo.currentUserID]);
            #region Income Tree
            foreach (IncomeStatement oItem in _oIncomeStatement.Revenues)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //{
                //    _oTChartsOfAccount.state = "closed";
                //}
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(5);//Revenue
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oIncomeStatement.TRevenue = _oTChartsOfAccount;
            #endregion
            #region Expense Tree
            _oTChartsOfAccounts = new List<TChartsOfAccount>();

            foreach (IncomeStatement oItem in _oIncomeStatement.Expenses)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //{
                //    _oTChartsOfAccount.state = "closed";
                //}
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(6);//Expenditure
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oIncomeStatement.TExpenditure = _oTChartsOfAccount;
            #endregion


            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oIncomeStatements);
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(2, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatement.SessionDate = oAccountingSession.StartDate.ToString("dd MMM yyyy") + " to " + oAccountingSession.EndDate.ToString("dd MMM yyyy");
            _oIncomeStatement.AccountTypeObjs = EnumObject.jGets(typeof(EnumAccountType));
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oIncomeStatement);
        }

        public ActionResult PrepareIncomeStatement(int nBUID, int nAccountType, int nAccountingSessionID, double ts)
        {
            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();
            //int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(nAccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatement = new IncomeStatement();
            _oIncomeStatements = IncomeStatement.Gets(nBUID, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, nAccountType, (int)Session[SessionInfo.currentUserID]);


            IncomeStatement oIS = new IncomeStatement();
            oIS.AccountHeadID = 1;
            this.AddNodes(oIS);

            _oIncomeStatement.Revenues = _oRevenues;
            _oIncomeStatement.Expenses = _oExpenses;
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oRevenues);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oExpenses);

            if (_oIncomeStatement.TotalExpenses < _oIncomeStatement.TotalRevenues)
            {
                _oIncomeStatement.ProfiteLossAmount = " Net Income = " + Global.MillionFormat(_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses) + " BDT";
            }
            else
            {
                _oIncomeStatement.ProfiteLossAmount = " Net Loss = " + Global.MillionFormat(_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues) + " BDT";
            }
            _oIncomeStatement.SessionDate = oAccountingSession.StartDate.ToString("dd MMM yyyy") + " -to- " + oAccountingSession.EndDate.ToString("dd MMM yyyy");
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oIncomeStatement.Company = oCompany;

            rptIncomeStatement oReport = new rptIncomeStatement();
            byte[] abytes = oReport.PrepareReportShort(_oIncomeStatement, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrepareIncomeStatementInXL(int nBUID, int nAccountType, int nAccountingSessionID, double ts)
        {
            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(nAccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatement = new IncomeStatement();
            _oIncomeStatements = IncomeStatement.Gets(nBUID, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, nAccountType, (int)Session[SessionInfo.currentUserID]);

            IncomeStatement oIS = new IncomeStatement();
            oIS.AccountHeadID = 1;
            this.AddNodes(oIS);

            _oIncomeStatement.Revenues = _oRevenues;
            _oIncomeStatement.Expenses = _oExpenses;
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oRevenues);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oExpenses);

            var stream = new MemoryStream();
            #region Income Statement Short

            var serializer = new XmlSerializer(typeof(List<IncomeStatementShortXL>));

            //We load the data           
            IncomeStatementShortXL oIncomeStatementShortXL = new IncomeStatementShortXL();
            List<IncomeStatementShortXL> oIncomeStatementShortXLs = new List<IncomeStatementShortXL>();

            #region Revenues
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "Revenues";
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            foreach (IncomeStatement oItem in _oIncomeStatement.Revenues)
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                if (oItem.AccountType == EnumAccountType.Group)
                {
                    oIncomeStatementShortXL.Group = oItem.AccountHeadName;
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = 0.00;
                    oIncomeStatementShortXL.GroupBalance = oItem.CGSGBalance;
                }
                else if (oItem.AccountType == EnumAccountType.SubGroup)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = oItem.AccountHeadName;
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                else if (oItem.AccountType == EnumAccountType.Ledger)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = oItem.AccountHeadName;
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            }

            #region Total Revenues
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "Total Revenues=";
            oIncomeStatementShortXL.LedgerBalance = 0.00;
            oIncomeStatementShortXL.GroupBalance = _oIncomeStatement.TotalRevenues;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #endregion

            #region Blank
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = 0;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);

            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = 0;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #region Expenses
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "Expenses ";
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            foreach (IncomeStatement oItem in _oIncomeStatement.Expenses)
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                if (oItem.AccountType == EnumAccountType.Group)
                {
                    oIncomeStatementShortXL.Group = oItem.AccountHeadName;
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = 0.00;
                    oIncomeStatementShortXL.GroupBalance = oItem.CGSGBalance;
                }
                else if (oItem.AccountType == EnumAccountType.SubGroup)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = oItem.AccountHeadName;
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                else if (oItem.AccountType == EnumAccountType.Ledger)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = oItem.AccountHeadName;
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            }

            #region Total Expenses
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "Total Expenses =";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = _oIncomeStatement.TotalExpenses;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #endregion

            #region Blank
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = 0;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #region Net Income or Loss
            if (_oIncomeStatement.TotalExpenses < _oIncomeStatement.TotalRevenues)
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = " Net Income =";
                oIncomeStatementShortXL.SubGroup = (_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses).ToString();
                oIncomeStatementShortXL.LedgerBalance = 0.00;
                oIncomeStatementShortXL.GroupBalance = 0;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                //_oIncomeStatement.ProfiteLossAmount = " Net Income = " + Global.MillionFormat(_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses) + " BDT";
            }
            else
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "  Net Loss =";
                oIncomeStatementShortXL.SubGroup = (_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues).ToString();
                oIncomeStatementShortXL.LedgerBalance = 0.00;
                oIncomeStatementShortXL.GroupBalance = 0;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                // _oIncomeStatement.ProfiteLossAmount = " Net Loss = " + Global.MillionFormat(_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues) + " BDT";
            }
            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oIncomeStatementShortXLs);
            #endregion

            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Income Statement.xls");

        }

        public ActionResult ComprehensiveIncome(int nBUID, int nAccountType, int nAccountingSessionID, double ts)
        {
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(nAccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatement = new IncomeStatement();
            _oIncomeStatements = IncomeStatement.Gets(nBUID, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, nAccountType, (int)Session[SessionInfo.currentUserID]);

            IncomeStatement oIS = new IncomeStatement();
            oIS.AccountHeadID = 1;
            this.AddNodes(oIS);

            _oIncomeStatement.Revenues = _oRevenues;
            _oIncomeStatement.Expenses = _oExpenses;
            _oIncomeStatement.CIStatementSetups = CIStatementSetup.Gets((int)Session[SessionInfo.currentUserID]);
            /* Gross_Turnover = 1,//Income ;Other_Income = 5,Profit_From_Associate_Undertaking = 8,Comprehensive_Income = 9//incoem                    
                Value_Added_Tax = 2, Overhead_Cost = 3,Operating_Expenses = 4,WPPF_Allocation = 6,Income_Tax = 7,//expenditure;
            */
            foreach (CIStatementSetup oItem in _oIncomeStatement.CIStatementSetups)
            {
                if ("1,6".Contains(Convert.ToString((int)oItem.CIHeadType)))//income
                {
                    oItem.Note = _oRevenues.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].AccountCode;
                    oItem.AccountHeadValue = _oRevenues.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].CGSGBalance;
                }
                else if ("3,4,6,7,8".Contains(Convert.ToString((int)oItem.CIHeadType)))//expense
                {
                    oItem.Note = _oExpenses.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].AccountCode;
                    oItem.AccountHeadValue = _oExpenses.Where(x => x.AccountHeadID == oItem.AccountHeadID).ToList()[0].CGSGBalance;
                }
            }
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oRevenues);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oExpenses);


            _oIncomeStatement.SessionDate = oAccountingSession.SessionName; //dStartDate.ToString("dd MMM yyyy") + " -to- " + dEndDate.ToString("dd MMM yyyy");
            _oIncomeStatement.EndDate = oAccountingSession.EndDate;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;
            _oIncomeStatement.Company = oCompany;

            rptComprehensiveIncome oReport = new rptComprehensiveIncome();
            byte[] abytes = oReport.PrepareReport(_oIncomeStatement, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult NotesOfTheComprehensiveIncome(int nBUID, int nAccountType, int nAccountingSessionID, double ts)
        {
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(nAccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatement = new IncomeStatement();
            _oIncomeStatements = IncomeStatement.Gets(nBUID, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, nAccountType, (int)Session[SessionInfo.currentUserID]);

            IncomeStatement oIS = new IncomeStatement();
            oIS.AccountHeadID = 1;
            this.AddNodes(oIS);

            _oIncomeStatement.Revenues = _oRevenues;
            _oIncomeStatement.Expenses = _oExpenses;
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oRevenues);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oExpenses);
            _oIncomeStatement.SessionDate = oAccountingSession.SessionName; //dStartDate.ToString("dd MMM yyyy") + " -to- " + dEndDate.ToString("dd MMM yyyy");
            _oIncomeStatement.EndDate = oAccountingSession.EndDate;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;
            _oIncomeStatement.Company = oCompany;

            rptNotesOfTheComprehensiveIncome oReport = new rptNotesOfTheComprehensiveIncome();
            byte[] abytes = oReport.PrepareReport(_oIncomeStatement, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult GetIncomeStatement(IncomeStatement oIncomeStatement)
        {
            _oRevenues = new List<IncomeStatement>();
            _oExpenses = new List<IncomeStatement>();

            if (oIncomeStatement.AccountTypeInInt == 0)
            {
                oIncomeStatement.AccountTypeInInt = 4;
            }
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(oIncomeStatement.AccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatement = new IncomeStatement();
            _oIncomeStatements = IncomeStatement.Gets(oIncomeStatement.BUID, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, oIncomeStatement.AccountTypeInInt, (int)Session[SessionInfo.currentUserID]);

            IncomeStatement oIS = new IncomeStatement();
            oIS.AccountHeadID = 1;
            this.AddNodes(oIS);

            _oIncomeStatement.Revenues = _oRevenues;
            _oIncomeStatement.Expenses = _oExpenses;
            #region Income Tree
            foreach (IncomeStatement oItem in _oRevenues)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if ((EnumAccountType)oIncomeStatement.AccountTypeInInt == EnumAccountType.Ledger)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}
                //else if ((EnumAccountType)oIncomeStatement.AccountTypeInInt == EnumAccountType.SubGroup)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(5);//Revenue
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oIncomeStatement.TRevenue = _oTChartsOfAccount;
            #endregion
            #region Expense Tree
            _oTChartsOfAccounts = new List<TChartsOfAccount>();

            foreach (IncomeStatement oItem in _oExpenses)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if ((EnumAccountType)oIncomeStatement.AccountTypeInInt == EnumAccountType.Ledger)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}
                //else if ((EnumAccountType)oIncomeStatement.AccountTypeInInt == EnumAccountType.SubGroup)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(6);//Expenditure
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oIncomeStatement.TExpenditure = _oTChartsOfAccount;
            #endregion


            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oRevenues);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oExpenses);

            _oIncomeStatement.SessionDate = oIncomeStatement.StartDate.ToString("dd MMM yyyy") + " to " + oIncomeStatement.EndDate.ToString("dd MMM yyyy");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIncomeStatement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #region Comprehensive Income Statement Setup
        public ActionResult ViewCIStatementSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oCIStatementSetup = new CIStatementSetup();
            _oCIStatementSetup.CIStatementSetups = CIStatementSetup.Gets((int)Session[SessionInfo.currentUserID]);
            _oCIStatementSetup.CISSetupObjs = EnumObject.jGets(typeof(EnumCISSetup));
            return View(_oCIStatementSetup);
        }

        [HttpPost]
        public JsonResult Save(CIStatementSetup oCIStatementSetup)
        {
            _oCIStatementSetups = new List<CIStatementSetup>();
            try
            {

                _oCIStatementSetups = oCIStatementSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCIStatementSetup = new CIStatementSetup();
                _oCIStatementSetup.ErrorMessage = ex.Message;
                _oCIStatementSetups.Add(_oCIStatementSetup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCIStatementSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            _oCIStatementSetup = new CIStatementSetup();
            try
            {
                sFeedBackMessage = _oCIStatementSetup.Delete(id, (int)Session[SessionInfo.currentUserID]);
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

        #region Accounting Ratio Parts
        [HttpPost]
        public ActionResult SetAccountingRatioSetupData(SP_RatioSetup oSP_RatioSetup)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSP_RatioSetup);
            IncomeStatement oIncomeStatement = new IncomeStatement();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oIncomeStatements = IncomeStatement.Gets(oSP_RatioSetup.BusinessUnitID, oSP_RatioSetup.StartDate, oSP_RatioSetup.EndDate, 0, (int)EnumAccountType.SubGroup, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            oBusinessUnit = oBusinessUnit.Get(oSP_RatioSetup.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            oIncomeStatement.IncomeStatements = _oIncomeStatements;
            oIncomeStatement.CIStatementSetups = this.PrepareComprehensiveIncome(oIncomeStatement, oBusinessUnit);
            this.Session.Add(SessionInfo.SearchData, oIncomeStatement.CIStatementSetups);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}