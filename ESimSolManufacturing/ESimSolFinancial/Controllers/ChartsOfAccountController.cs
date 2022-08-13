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
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web;
using System.Data;
using System.Data.OleDb;
namespace ESimSolFinancial.Controllers
{
    public class ChartsOfAccountController : Controller
    {
        #region Declaration
        ChartsOfAccount _oChartsOfAccount = new ChartsOfAccount();
        List<ChartsOfAccount> _oChartsOfAccountList = new List<ChartsOfAccount>();
        List<ChartsOfAccount> _oChartsOfAccounts = new List<ChartsOfAccount>();
        COA_ChartOfAccountCostCenter _oChartsOfAccountCostCenter = new COA_ChartOfAccountCostCenter();
        List<COA_ChartOfAccountCostCenter> _oChartsOfAccountCostCenters = new List<COA_ChartOfAccountCostCenter>();
        BusinessUnitWiseAccountHead _oCompanyWiseAccountHead = new BusinessUnitWiseAccountHead();
        List<BusinessUnitWiseAccountHead> _oCompanyWiseAccountHeads = new List<BusinessUnitWiseAccountHead>();
        TChartsOfAccount _oTChartsOfAccount = new TChartsOfAccount();
        List<TChartsOfAccount> _oTChartsOfAccounts = new List<TChartsOfAccount>();
        AccountHeadConfigure _oAccountHeadConfigure = new AccountHeadConfigure();
        List<ACCostCenter> _oACCostCenters = new List<ACCostCenter>();
        List<ChartsOfAccountXL> _oChartsOfAccountXLs = new List<ChartsOfAccountXL>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private TChartsOfAccount GetRoot(int nParentID)
        {
            TChartsOfAccount oTChartsOfAccount = new TChartsOfAccount();
            foreach (TChartsOfAccount oItem in _oTChartsOfAccounts)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTChartsOfAccount;
        }

        private TChartsOfAccount GetRootForMove(int nID)
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

        private bool ValidateInput(ChartsOfAccount oCOA, ChartsOfAccount oParent)
        {
            if (oCOA.AccountCode == null || oCOA.AccountCode == "")
            {
                _sErrorMessage = "Please enter Account code";
                return false;
            }
            if (oCOA.AccountHeadName == null || oCOA.AccountHeadName == "")
            {
                _sErrorMessage = "Please enter Account Head Name";
                return false;
            }
            if (oCOA.AccountType == EnumAccountType.None)
            {
                _sErrorMessage = "please Select Account Type";
                return false;
            }

            if (oCOA.AccountType == EnumAccountType.Component) 
            {
                _sErrorMessage = "please Select Account Type  Group or Ledger";
                return false;
            }

            if (oCOA.ParentHeadID == 1)
            {
                _sErrorMessage = "Invalid Operation! Component item is fixed please change parent selection";
                return false;
            }

            int nParentAccountType = (int)oParent.AccountType;
            int nSelectedAccountType = (int)oCOA.AccountType;
            if (nSelectedAccountType < nParentAccountType)
            {
                _sErrorMessage = "Invalid Operation! Account Type hierarchy  is indaild!";
                return false;
            }



            return true;
        }

        #endregion
        
        #region Function For Print Chart Of Account
        private ChartsOfAccount GetRootFromCOA()
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                if (oItem.ParentHeadID == 0)
                {
                    return oItem;
                }
            }
            return _oChartsOfAccount;
        }

        private IEnumerable<ChartsOfAccount> GetChildFromCOA(int nAccountHeadID)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                if (oItem.ParentHeadID == nAccountHeadID)
                {
                    oChartsOfAccounts.Add(oItem);
                }
            }
            return oChartsOfAccounts;
        }

        private void AddTreeNodesFromCOA(ref ChartsOfAccount oChartsOfAccount)
        {
            IEnumerable<ChartsOfAccount> oChildNodes;
            oChildNodes = GetChildFromCOA(oChartsOfAccount.AccountHeadID);
            oChartsOfAccount.ChildNodes = oChildNodes;
            if (oChartsOfAccount.ParentHeadID != 0)
            {
                _oChartsOfAccountList.Add(oChartsOfAccount);
            }
            foreach (ChartsOfAccount oItem in oChildNodes)
            {
                ChartsOfAccount oTemp = oItem;
                AddTreeNodesFromCOA(ref oTemp);
            }
        }
        #endregion
        
        #region New Task   
        public ActionResult ViewMoveChartOfAccounts(int id, double ts)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            
            _oChartsOfAccount = _oChartsOfAccount.Get(id, (int)Session[SessionInfo.currentUserID]);
            string sTempSQL = "SELECT * FROM View_ChartsOfAccount WHERE ComponentID=(SELECT ComponentID FROM View_ChartsOfAccount WHERE AccountHeadID=" + id.ToString() + ") AND AccountType<(SELECT AccountType FROM View_ChartsOfAccount WHERE AccountHeadID=" + id.ToString() + ")";            
            _oChartsOfAccounts = ChartsOfAccount.Gets(sTempSQL, (int)Session[SessionInfo.currentUserID]);

            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                if (oItem.AccountHeadID != _oChartsOfAccount.ParentHeadID)//Same parent head move not possible
                {
                    _oTChartsOfAccount = new TChartsOfAccount();
                    _oTChartsOfAccount.id = oItem.AccountHeadID;
                    _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                    _oTChartsOfAccount.text = oItem.AccountHeadName;
                    _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                    _oTChartsOfAccount.code = oItem.AccountCode;
                    _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                    _oTChartsOfAccount.CName = oItem.CName;
                    _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                    _oTChartsOfAccount.AccountTypeInInt = (int)oItem.AccountType;
                    _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                    _oTChartsOfAccount.AOTypeInInt = (int)oItem.AccountOperationType;
                    _oTChartsOfAccount.AOTypeSt = oItem.AccountOperationTypeSt;
                    _oTChartsOfAccount.ParentAOTypeInInt = (int)oItem.ParentAccountOperationType;
                    _oTChartsOfAccount.PathName = oItem.PathName;
                    _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                    _oTChartsOfAccount.Description = oItem.Description;
                    _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                    _oTChartsOfAccounts.Add(_oTChartsOfAccount);
                }
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootForMove(_oChartsOfAccount.ComponentID);
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oChartsOfAccount.TChartsOfAccount = _oTChartsOfAccount;
            return PartialView(_oChartsOfAccount);
        }


        [HttpPost]
        public JsonResult GetsMovesAccount(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();

            _oChartsOfAccount = _oChartsOfAccount.Get(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            string sTempSQL = "SELECT * FROM View_ChartsOfAccount WHERE ComponentID=(SELECT ComponentID FROM View_ChartsOfAccount WHERE AccountHeadID=" + oChartsOfAccount.AccountHeadID.ToString() + ") AND AccountType<(SELECT AccountType FROM View_ChartsOfAccount WHERE AccountHeadID=" + oChartsOfAccount.AccountHeadID.ToString() + ")";
            _oChartsOfAccounts = ChartsOfAccount.Gets(sTempSQL, (int)Session[SessionInfo.currentUserID]);

            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                if (oItem.AccountHeadID != _oChartsOfAccount.ParentHeadID)//Same parent head move not possible
                {
                    _oTChartsOfAccount = new TChartsOfAccount();
                    _oTChartsOfAccount.id = oItem.AccountHeadID;
                    _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                    _oTChartsOfAccount.text = oItem.AccountHeadName;
                    _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                    _oTChartsOfAccount.code = oItem.AccountCode;
                    _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                    _oTChartsOfAccount.CName = oItem.CName;
                    _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                    _oTChartsOfAccount.AccountTypeInInt = (int)oItem.AccountType;
                    _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                    _oTChartsOfAccount.AOTypeInInt = (int)oItem.AccountOperationType;
                    _oTChartsOfAccount.AOTypeSt = oItem.AccountOperationTypeSt;
                    _oTChartsOfAccount.ParentAOTypeInInt = (int)oItem.ParentAccountOperationType;
                    _oTChartsOfAccount.PathName = oItem.PathName;
                    _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                    _oTChartsOfAccount.Description = oItem.Description;
                    _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                    _oTChartsOfAccounts.Add(_oTChartsOfAccount);
                }
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootForMove(_oChartsOfAccount.ComponentID);
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oChartsOfAccount.TChartsOfAccount = _oTChartsOfAccount;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       

        [HttpPost]
        public JsonResult MoveChartOfAccount(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccount = new ChartsOfAccount();
            try
            {
                _oChartsOfAccount = oChartsOfAccount;                
                _oChartsOfAccount = _oChartsOfAccount.MoveChartOfAccount((int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChartsOfAccountsByCISSetup(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccountList = new List<ChartsOfAccount>();
            string sSQL = "SELECT * From View_ChartsOfAccount WHERE  AccountType = 4 AND ComponentID = "; ;
            try
            {
                /* Gross_Turnover = 1,//Income ;Other_Income = 5,Profit_From_Associate_Undertaking = 8,Comprehensive_Income = 9//incoem                   
                   Value_Added_Tax = 2, Overhead_Cost = 3,Operating_Expenses = 4,WPPF_Allocation = 6,Income_Tax = 7,//expenditure;
                 */
                if ("1,8".Contains(oChartsOfAccount.PathName))//income
                {
                    sSQL = sSQL + ((int)EnumComponentType.Income).ToString();
                }
                else if ("4,7,9,10".Contains(oChartsOfAccount.PathName))//expense
                {
                    sSQL = sSQL + ((int)EnumComponentType.Expenditure).ToString();
                }
                else if ("2,5,6".Contains(oChartsOfAccount.PathName))//Asset Inventory
                {
                    sSQL = sSQL + ((int)EnumComponentType.Asset).ToString();
                }
                else if ("3".Contains(oChartsOfAccount.PathName))//Material Purchase
                {
                    sSQL = "SELECT * From View_ChartsOfAccount WHERE  AccountType = 4 AND ComponentID IN(" + ((int)EnumComponentType.Asset).ToString() + "," + ((int)EnumComponentType.Expenditure).ToString() + ")";
                }
                _oChartsOfAccountList = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
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
        //GetsByString  //Common for all
        [HttpPost]
        public JsonResult GetsByString(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccountList = new List<ChartsOfAccount>();
            
            try
            {
                _oChartsOfAccountList = ChartsOfAccount.Gets(oChartsOfAccount.PathName, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsByAccountType(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccountList = new List<ChartsOfAccount>();
            try
            {
                string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountType = "+oChartsOfAccount.AccountTypeInInt;
                if(!string.IsNullOrEmpty(oChartsOfAccount.AccountHeadName))
                {
                    sSQL += " AND AccountHeadName LIKE '%" + oChartsOfAccount.AccountHeadName + "%'";
                }
                _oChartsOfAccountList = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
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

        [HttpGet]
        public JsonResult GetChartOfAccounts(double ts)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();           
            _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.state = "";
                _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                _oTChartsOfAccount.code = oItem.AccountCode;
                _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                _oTChartsOfAccount.CName = oItem.CName;
                _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                _oTChartsOfAccount.Description = oItem.Description;
                _oTChartsOfAccount.PathName = oItem.PathName;
                _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(0);
            this.AddTreeNodes(ref _oTChartsOfAccount);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBOAHs(ChartsOfAccount oChartsOfAccount)
        {
            List<BusinessUnitWiseAccountHead> oBOAHs = new List<BusinessUnitWiseAccountHead>();

            if (oChartsOfAccount.AccountHeadID > 0) {
                oBOAHs = BusinessUnitWiseAccountHead.GetsByCOA(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAHs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFromCOA(BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead)
        {

            try
            {


                //  oBusinessLocation.LocationName.Split(','[0])

                oBusinessUnitWiseAccountHead = oBusinessUnitWiseAccountHead.SaveFromCOA((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
                oBusinessUnitWiseAccountHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBusinessUnitWiseAccountHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewChartOfAccounts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChartsOfAccount).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            try
            {
                _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
                foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
                {
                    _oTChartsOfAccount = new TChartsOfAccount();
                    _oTChartsOfAccount.id = oItem.AccountHeadID;
                    _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                    _oTChartsOfAccount.text = oItem.AccountHeadName;
                    _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                    _oTChartsOfAccount.code = oItem.AccountCode;
                    _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                    _oTChartsOfAccount.CName = oItem.CName;
                    _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                    _oTChartsOfAccount.AccountTypeInInt = (int)oItem.AccountType;
                    _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                    _oTChartsOfAccount.AOTypeInInt = (int)oItem.AccountOperationType;
                    _oTChartsOfAccount.AOTypeSt = oItem.AccountOperationTypeSt;
                    _oTChartsOfAccount.ParentAOTypeInInt = (int)oItem.ParentAccountOperationType;
                    _oTChartsOfAccount.PathName = oItem.PathName;
                    _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                    _oTChartsOfAccount.InventoryHeadID = oItem.InventoryHeadID;
                    _oTChartsOfAccount.Description = oItem.Description;
                    _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                    _oTChartsOfAccounts.Add(_oTChartsOfAccount);
                }
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount = GetRoot(0);
                this.AddTreeNodes(ref _oTChartsOfAccount);

                ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
                oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
                ViewBag.BUs = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
                ViewBag.AOTs = EnumObject.jGets(typeof(EnumAccountOperationType));
                 string sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountType=5 AND HH.ParentHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType=" + ((int)EnumCISSetup.Inventory_Head).ToString() + ") ORDER BY HH.AccountHeadName";
                 ViewBag.InventoryEffects = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                 ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
                return View(_oTChartsOfAccount);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTChartsOfAccount);
            }
        }

        [HttpPost]
        public JsonResult RefreshChartOfAccounts(BusinessUnit oBusinessUnit)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                string sSQL = "SELECT * FROM View_ChartsOfAccount AS TT WHERE TT.AccountHeadID IN (SELECT HH.AccountHeadID FROM BusinessUnitWiseAccountHead AS HH WHERE HH.BusinessUnitID=" + oBusinessUnit.BusinessUnitID.ToString() + ") ORDER BY AccountHeadID";
                _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
            }
            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                _oTChartsOfAccount.code = oItem.AccountCode;
                _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                _oTChartsOfAccount.CName = oItem.CName;
                _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                _oTChartsOfAccount.AccountTypeInInt = (int)oItem.AccountType;
                _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                _oTChartsOfAccount.AOTypeInInt = (int)oItem.AccountOperationType;
                _oTChartsOfAccount.AOTypeSt = oItem.AccountOperationTypeSt;
                _oTChartsOfAccount.ParentAOTypeInInt = (int)oItem.ParentAccountOperationType;
                _oTChartsOfAccount.PathName = oItem.PathName;
                _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                _oTChartsOfAccount.Description = oItem.Description;
                _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(0);
            this.AddTreeNodes(ref _oTChartsOfAccount);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewChatsOfAccount(int id)
        {
            Company oCompany = new Company();
            ChartsOfAccount oCOA = new ChartsOfAccount();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            List<EnumObject> oAOTs = new List<EnumObject>();

            oCOA = oCOA.Get(id, (int)Session[SessionInfo.currentUserID]);

            
            oChartsOfAccount.ParentHeadID = oCOA.AccountHeadID;
            if (oCOA.AccountType == EnumAccountType.None)
            {
                oChartsOfAccount.AccountType = EnumAccountType.Component;
            }
            else if (oCOA.AccountType == EnumAccountType.Component)
            {
                oChartsOfAccount.AccountType = EnumAccountType.Segment;
            }
            else if (oCOA.AccountType == EnumAccountType.Segment)
            {
                oChartsOfAccount.AccountType = EnumAccountType.Group;
            }
            else if (oCOA.AccountType == EnumAccountType.Group)
            {
                oChartsOfAccount.AccountType = EnumAccountType.SubGroup;
            }
            else if (oCOA.AccountType == EnumAccountType.SubGroup)
            {
                oChartsOfAccount.AccountType = EnumAccountType.Ledger;
                oChartsOfAccount.AccountOperationType = oCOA.AccountOperationType;
            }

            oAOTs = EnumObject.jGets(typeof(EnumAccountOperationType));
            List<EnumObject> oTempAOTs = new List<EnumObject>();
            if (oChartsOfAccount.AccountType == EnumAccountType.Ledger)
            {
                foreach (EnumObject oItem in oAOTs)
                {
                    if (oItem.id == 0 || oItem.id == (int)oCOA.AccountOperationType)
                    {
                        oTempAOTs.Add(oItem);
                    }
                }
                ViewBag.AOTs = oTempAOTs;
            }
            else
            {
                ViewBag.AOTs = oAOTs;
            }
            oChartsOfAccount.AccountTypeInInt = (int)oChartsOfAccount.AccountType;
            oChartsOfAccount.DisplayMessage = "Selected parent account head : " + oCOA.AccountHeadNameCode;
            oChartsOfAccount.AccountCode = ChartsOfAccount.GetAccountCode(oCOA.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccount.ChildNodes = ChartsOfAccount.GetsByParent(oCOA.AccountHeadID, (int)Session[SessionInfo.currentUserID]);            
            oChartsOfAccount.Company = oCompany.Get((int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            return View(oChartsOfAccount);
        }

        public ActionResult ViewAccountHeadConfigure(int id)
        {
            List<AccountHeadConfigure> oAccountHeadConfigures = new List<AccountHeadConfigure>();

            if (id > 0)
            {
                oAccountHeadConfigures = AccountHeadConfigure.Gets("SELECT * FROM View_AccountHeadConfigure where AccountHeadID=" + id, (int)Session[SessionInfo.currentUserID]);
            }

            return View(oAccountHeadConfigures);
        }

        [HttpPost]
        public JsonResult SaveAHC(AccountHeadConfigure oAccountHeadConfigure)
        {
            _oAccountHeadConfigure = new AccountHeadConfigure();
            
            try
            {
                _oAccountHeadConfigure = oAccountHeadConfigure;
                
                _oAccountHeadConfigure = _oAccountHeadConfigure.Save((int)Session[SessionInfo.currentUserID]);
                
            }
            catch (Exception ex)
            {
                _oAccountHeadConfigure = new AccountHeadConfigure();
                _oAccountHeadConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAccountHeadConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAHC(AccountHeadConfigure oAccountHeadConfigure)
        {
            string sfeedbackmessage = "";
            _oAccountHeadConfigure = new AccountHeadConfigure();
            try
            {
                sfeedbackmessage = _oAccountHeadConfigure.Delete(oAccountHeadConfigure.AccountHeadConfigureID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sfeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCostCenter(ACCostCenter oACCostCenter)
        {
            _oACCostCenters = new List<ACCostCenter>();

            oACCostCenter.Name = oACCostCenter.Name == null ? "" : oACCostCenter.Name;
            try
            {
                string sSQL = "SELECT * FROM View_ACCostCenter WHERE ParentID=1 AND Name LIKE '%" + oACCostCenter.Name + "%'";
                _oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oACCostCenter = new ACCostCenter();
                oACCostCenter.ErrorMessage = ex.Message;
                _oACCostCenters.Add(oACCostCenter);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Save(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccount = new ChartsOfAccount();
          
            try
            {
                _oChartsOfAccount = oChartsOfAccount;                
                _oChartsOfAccount.AccountType = (EnumAccountType)oChartsOfAccount.AccountTypeInInt;
                _oChartsOfAccount = _oChartsOfAccount.Save((int)Session[SessionInfo.currentUserID]);
                _oChartsOfAccount.NewAccountCode = ChartsOfAccount.GetAccountCode(_oChartsOfAccount.ParentHeadID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccount = new ChartsOfAccount();
            try
            {
                _oChartsOfAccount = _oChartsOfAccount.Get(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
                _oChartsOfAccount.AccountHeadName = oChartsOfAccount.AccountHeadName;
                _oChartsOfAccount.Description = oChartsOfAccount.Description;
                _oChartsOfAccount.AccountOperationType = oChartsOfAccount.AccountOperationType;
                _oChartsOfAccount.CurrencyID = oChartsOfAccount.CurrencyID;
                _oChartsOfAccount = _oChartsOfAccount.Save((int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        public JsonResult UpdateInventoryEffect(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccount = new ChartsOfAccount();
            try
            {
                _oChartsOfAccount = oChartsOfAccount.Update_InventoryHead((int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public JsonResult Delete(ChartsOfAccount oChartsOfAccount)
        {
            string sfeedbackmessage = "";
            _oChartsOfAccount = new ChartsOfAccount();
            try
            {
                sfeedbackmessage = _oChartsOfAccount.Delete(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sfeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getchildren(int parentid)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            try
            {
                int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
                string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID=" + parentid.ToString();
                _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       

        public ActionResult EditChatsOfAccount(int id)
        {
            _oChartsOfAccount = new ChartsOfAccount();            
            _oChartsOfAccount = _oChartsOfAccount.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oChartsOfAccount.AccountTypeInInt = (int)_oChartsOfAccount.AccountType;
            return PartialView(_oChartsOfAccount);
        }

        [HttpGet]
        public JsonResult GetRefresh(int ParentID)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            if (ParentID > 0)
            {
                _oChartsOfAccounts = ChartsOfAccount.GetRefresh(ParentID, (int)Session[SessionInfo.currentUserID]);
                foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
                {
                    _oTChartsOfAccount = new TChartsOfAccount();
                    _oTChartsOfAccount.id = oItem.AccountHeadID;
                    _oTChartsOfAccount.text = oItem.AccountHeadName;
                    _oTChartsOfAccount.state = "";
                    _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                    _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                    _oTChartsOfAccount.code = oItem.AccountCode;
                    _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                    _oTChartsOfAccount.CName = oItem.CName;
                    _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                    _oTChartsOfAccount.Description = oItem.Description;
                    _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                    _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                    _oTChartsOfAccounts.Add(_oTChartsOfAccount);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult Gets()
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();          
            _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCharofaccoutTree()
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();      
            _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {

                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName + "[" + oItem.AccountCode + "]";
                _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                _oTChartsOfAccount.code = oItem.AccountCode;
                _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                _oTChartsOfAccount.CName = oItem.CName;
                _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                _oTChartsOfAccount.AccountTypeInString = oItem.AccountType.ToString();
                _oTChartsOfAccount.Description = oItem.Description;
                _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(0);
            this.AddTreeNodes(ref _oTChartsOfAccount);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oTChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsCostCenterEnable(int nCOAID)
        {
            bool bFlag = false;
            string Sql = "SELECT * FROM View_COA_ChartOfAccountCostCenter WHERE AccountHeadID=" + nCOAID + "";
            List<COA_ChartOfAccountCostCenter> oCOA_ChartOfAccountCostCenters = new List<COA_ChartOfAccountCostCenter>();
            oCOA_ChartOfAccountCostCenters = COA_ChartOfAccountCostCenter.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
            if (oCOA_ChartOfAccountCostCenters.Count > 0)
            {
                bFlag = true;
            }
            return Json(bFlag, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOpeningBalance(AccountOpenning oAccountOpenning)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            oAccountOpenning = oAccountOpenning.Get(oAccountOpenning.AccountHeadID, oAccountOpenning.AccountingSessionID, oAccountOpenning.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oAccountOpenning);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAccountsHead(ChartsOfAccount oChartsOfAccount)
        {
            if (oChartsOfAccount.AccountHeadName == null) oChartsOfAccount.AccountHeadName = "";
        //    int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            _oChartsOfAccountList = ChartsOfAccount.GetsbyAccountsName(oChartsOfAccount.AccountHeadName,  (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oChartsOfAccountList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAccountsHeadByComponentAndAccountType(ChartsOfAccount oChartsOfAccount)//written by Mahabub (03 Dec 2015)
        {
            string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ComponentID = "+oChartsOfAccount.ComponentID+" AND AccountType =" +oChartsOfAccount.AccountTypeInInt;
            if (oChartsOfAccount.AccountHeadName != null || oChartsOfAccount.AccountHeadName != "")
            {
                sSQL += "AND AccountHeadName LIke '%"+oChartsOfAccount.AccountHeadName+"%'";
            }
            _oChartsOfAccountList = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oChartsOfAccountList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAccountsHead(string sTemp, double ts)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            oChartsOfAccounts = ChartsOfAccount.GetsbyAccountsName(sTemp,  (int)Session[SessionInfo.currentUserID]);
            return PartialView(oChartsOfAccounts);
        }

        [HttpPost]
        public JsonResult GetAccountsHeads(ChartsOfAccount oChartsOfAccount)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            oChartsOfAccounts = ChartsOfAccount.GetsbyAccountsName(oChartsOfAccount.AccountHeadName, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Import From Excel
        private List<ChartsOfAccount> GetChartsOfAccountsFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            List<ChartsOfAccount> oCOAs = new List<ChartsOfAccount>();
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", "AccountHeads$");
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    for (int i = 0; i < oRows.Count; i++)
                    {

                        oChartsOfAccount = new ChartsOfAccount();
                        oChartsOfAccount.ParentHeadID = _oChartsOfAccount.AccountHeadID;
                        oChartsOfAccount.AccountType = (EnumAccountType)(((int)_oChartsOfAccount.AccountType) + 1);
                        oChartsOfAccount.AccountHeadName = Convert.ToString(oRows[i]["AccountHead"] == DBNull.Value ? "" : oRows[i]["AccountHead"]);
                        oChartsOfAccount.Description = Convert.ToString(oRows[i]["Description"] == DBNull.Value ? "" : oRows[i]["Description"]);
                        if (oChartsOfAccount.AccountHeadName == "")
                        {
                            continue;
                        }
                        

                        oChartsOfAccounts.Add(oChartsOfAccount);
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oChartsOfAccounts;
        }
        public ActionResult ImportFromExcel(int id)
        {
            ChartsOfAccount oCOA = new ChartsOfAccount();
            oCOA = oCOA.Get(id, (int)Session[SessionInfo.currentUserID]);
            return View(oCOA);
        }
        [HttpPost]
        public ActionResult ImportFromExcel(HttpPostedFileBase fileChartsOfAccounts, ChartsOfAccount oChartsOfAccount)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oChartsOfAccount = oChartsOfAccount;
            ChartsOfAccount oCOA = new ChartsOfAccount();

            try
            {
                if (fileChartsOfAccounts == null) { throw new Exception("File not Found"); }
                oChartsOfAccounts = this.GetChartsOfAccountsFromExcel(fileChartsOfAccounts);
                foreach (ChartsOfAccount oItem in oChartsOfAccounts)
                {
                    oCOA = oItem.Save((int)Session[SessionInfo.currentUserID]);
                    if (oCOA.ErrorMessage != "")
                    {
                        ViewBag.FeedBack = "Error for Account Head Name: " + oItem.AccountHeadName + ". " + oCOA.ErrorMessage;
                        return View(oCOA);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View(_oChartsOfAccount);
            }
            return RedirectToAction("ViewChartOfAccounts", new { menuid = Session[SessionInfo.MenuID] });
        }
        public ActionResult DownloadFormat(int ift)
        {
            ImportFormat oImportFormat = new ImportFormat();
            try
            {

                oImportFormat = ImportFormat.GetByType((EnumImportFormatType)ift, (int)Session[SessionInfo.currentUserID]);
                if (oImportFormat.AttatchFile != null)
                {
                    var file = File(oImportFormat.AttatchFile, oImportFormat.FileType);
                    file.FileDownloadName = oImportFormat.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oImportFormat.AttatchmentName);
            }
        }
        #endregion
        #endregion

        #region Configuration
        public ActionResult ViewCAO_Configuration(int id, double ts)
        {
            COA_AccountHeadConfig oCOA_AccountHeadConfig = new COA_AccountHeadConfig();
            try
            {
                if (id > 0)
                {
                    ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
                    oChartsOfAccount = oChartsOfAccount.Get(id, (int)Session[SessionInfo.currentUserID]);
                    oCOA_AccountHeadConfig = oCOA_AccountHeadConfig.Get(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
                    oCOA_AccountHeadConfig.AccountType = oChartsOfAccount.AccountType;
                    oCOA_AccountHeadConfig.ComponentID = oChartsOfAccount.ComponentID;
                    oCOA_AccountHeadConfig.ACCostCenters = ACCostCenter.Gets(1, (int)Session[SessionInfo.currentUserID]);
                    string sSQL = "SELECT * FROM View_ProductCategory WHERE ParentCategoryID=1";
                    oCOA_AccountHeadConfig.ProductCategorys = ProductCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "SELECT * FROM View_AccountHeadConfigure WHERE ReferenceObjectType = " + (int)EnumVoucherExplanationType.CostCenter + " AND AccountHeadID = " + id;
                    oCOA_AccountHeadConfig.AccHeadCostCenters = AccountHeadConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "SELECT * FROM View_AccountHeadConfigure WHERE ReferenceObjectType = " + (int)EnumVoucherExplanationType.InventoryReference + " AND AccountHeadID = " + id;
                    oCOA_AccountHeadConfig.AccHeadProductCategorys = AccountHeadConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oCOA_AccountHeadConfig = new COA_AccountHeadConfig();
                oCOA_AccountHeadConfig.ErrorMessage = ex.Message;
            }
            return PartialView(oCOA_AccountHeadConfig);
        }

        [HttpPost]
        public JsonResult GetCAO_Configurations(ChartsOfAccount oChartsOfAccount)
        {
            COA_AccountHeadConfig oCOA_AccountHeadConfig = new COA_AccountHeadConfig();
            try
            {
                if (oChartsOfAccount.AccountHeadID > 0)
                {
                    oChartsOfAccount = oChartsOfAccount.Get(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
                    oCOA_AccountHeadConfig = oCOA_AccountHeadConfig.Get(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
                    oCOA_AccountHeadConfig.AccountType = oChartsOfAccount.AccountType;
                    oCOA_AccountHeadConfig.ComponentID = oChartsOfAccount.ComponentID;
                    oCOA_AccountHeadConfig.ACCostCenters = ACCostCenter.Gets(1, (int)Session[SessionInfo.currentUserID]);
                    string sSQL = "SELECT * FROM View_ProductCategory WHERE ParentCategoryID=1";
                    oCOA_AccountHeadConfig.ProductCategorys = ProductCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "SELECT * FROM View_AccountHeadConfigure WHERE ReferenceObjectType = " + (int)EnumVoucherExplanationType.CostCenter + " AND AccountHeadID = " + oChartsOfAccount.AccountHeadID;
                    oCOA_AccountHeadConfig.AccHeadCostCenters = AccountHeadConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "SELECT * FROM View_AccountHeadConfigure WHERE ReferenceObjectType = " + (int)EnumVoucherExplanationType.InventoryReference + " AND AccountHeadID = " + oChartsOfAccount.AccountHeadID;
                    oCOA_AccountHeadConfig.AccHeadProductCategorys = AccountHeadConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    oCOA_AccountHeadConfig.BOAHs = BusinessUnitWiseAccountHead.GetsByCOA(oChartsOfAccount.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oCOA_AccountHeadConfig = new COA_AccountHeadConfig();
                oCOA_AccountHeadConfig.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCOA_AccountHeadConfig);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View_DynamicAccountHead(int id, double ts)
        {
            _oChartsOfAccount = new ChartsOfAccount();
            ChartsOfAccount oCOA = new ChartsOfAccount();
            oCOA = oCOA.Get(id, (int)Session[SessionInfo.currentUserID]);
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount.ParentHeadID = id;
            if (oCOA.AccountType == EnumAccountType.None)
            {
                oChartsOfAccount.AccountType = EnumAccountType.Component;
            }
            if (oCOA.AccountType == EnumAccountType.Component)
            {
                oChartsOfAccount.AccountType = EnumAccountType.Group;
            }
            if (oCOA.AccountType == EnumAccountType.Group)
            {
                oChartsOfAccount.AccountType = EnumAccountType.SubGroup;
            }
            if (oCOA.AccountType == EnumAccountType.SubGroup)
            {
                oChartsOfAccount.AccountType = EnumAccountType.Ledger;
            }
            oChartsOfAccount.AccountTypeInInt = (int)oChartsOfAccount.AccountType;
            oChartsOfAccount.DisplayMessage = "Selected parent account head : " + oCOA.AccountHeadNameCode;
            string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID=" + id.ToString();
            string sTempSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID=" + id.ToString() + " AND AccountHeadID=(SELECT ISNULL(MAX(AccountHeadID),0) FROM View_ChartsOfAccount WHERE ParentHeadID=" + id.ToString() + ")";
            List<ChartsOfAccount> oTempChartsOfAccounts = new List<ChartsOfAccount>();
            ChartsOfAccount oMaxChartsOfAccount = new ChartsOfAccount();
            oTempChartsOfAccounts = ChartsOfAccount.Gets(sTempSQL, (int)Session[SessionInfo.currentUserID]);
            if (oTempChartsOfAccounts.Count > 0)
            {
                oMaxChartsOfAccount = oTempChartsOfAccounts[0];
            }
            oChartsOfAccount.AccountCode = oCOA.AccountCode;
            oChartsOfAccount.AccountHeadName = oCOA.AccountHeadName;
            oChartsOfAccount.AccountHeadID = oCOA.AccountHeadID;
            oChartsOfAccount.AccountType = oCOA.AccountType;
            oChartsOfAccount.ReferenceObjectID = oCOA.ReferenceObjectID;
            oChartsOfAccount.ReferenceTypeInt = oCOA.ReferenceTypeInt;
            oChartsOfAccount.ChildNodes = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccount.VoucherReferenceTypeObjs = EnumObject.jGets(typeof(EnumReferenceType));
            return PartialView(oChartsOfAccount);
        }

        [HttpPost]
        public JsonResult GetsReferenceObj(ChartsOfAccount oChartsOfAccount)
        {
            List<Contractor> oContractors = new List<Contractor>();
            List<Bank> oBanks = new List<Bank>();
            List<BankAccount> oBankAccounts = new List<BankAccount>();

            _oChartsOfAccounts = new List<ChartsOfAccount>();
            try
            {
                oChartsOfAccount.AccountType = (EnumAccountType)oChartsOfAccount.AccountTypeInInt;
                oChartsOfAccount.ReferenceType = (EnumReferenceType)oChartsOfAccount.ReferenceTypeInt;
                if (oChartsOfAccount.ReferenceType == EnumReferenceType.Customer)
                {
                    oContractors = Contractor.GetsForAccount((int)EnumContractorType.Buyer, (int)oChartsOfAccount.ReferenceType, (int)Session[SessionInfo.currentUserID]);
                }
                if (oChartsOfAccount.ReferenceType == EnumReferenceType.Vendor)
                {
                    oContractors = Contractor.GetsForAccount((int)EnumContractorType.Supplier, (int)oChartsOfAccount.ReferenceType, (int)Session[SessionInfo.currentUserID]);
                }
                

                //if (oChartsOfAccount.ReferenceType == EnumReferenceType.IBP_EDF || oChartsOfAccount.ReferenceType == EnumReferenceType.IBP_Regular || oChartsOfAccount.ReferenceType == EnumReferenceType.IBP_Offshore || oChartsOfAccount.ReferenceType == EnumReferenceType.LTR || oChartsOfAccount.ReferenceType == EnumReferenceType.PAD || oChartsOfAccount.ReferenceType == EnumReferenceType.LDBP || oChartsOfAccount.ReferenceType == EnumReferenceType.Export_Bill_Receivable || oChartsOfAccount.ReferenceType == EnumReferenceType.Account_Receivable || oChartsOfAccount.ReferenceType == EnumReferenceType.GR_EDF || oChartsOfAccount.ReferenceType == EnumReferenceType.GR_Regular || oChartsOfAccount.ReferenceType == EnumReferenceType.GR_Offshore || oChartsOfAccount.ReferenceType == EnumReferenceType.GIT_EDF || oChartsOfAccount.ReferenceType == EnumReferenceType.GIT_Regular || oChartsOfAccount.ReferenceType == EnumReferenceType.GIT_Offshore)
                //{
                //    oBanks = Bank.GetsForAccount((int)oChartsOfAccount.ReferenceType, (int)Session[SessionInfo.currentUserID]);
                //}
                //if (oChartsOfAccount.ReferenceType == EnumReferenceType.Bank_AccountNo)
                //{
                //    oBankAccounts = BankAccount.GetsForAccount((int)oChartsOfAccount.ReferenceType, (int)Session[SessionInfo.currentUserID]);
                //}
                if (oContractors.Count > 0)
                {
                    foreach (Contractor oItem in oContractors)
                    {
                        _oChartsOfAccount = new ChartsOfAccount();
                        _oChartsOfAccount.ReferenceObjectID = oItem.ContractorID;
                        _oChartsOfAccount.ReferenceType = oChartsOfAccount.ReferenceType;
                        _oChartsOfAccount.AccountHeadName = oItem.Name;
                        _oChartsOfAccount.ReferenceType = (EnumReferenceType)oChartsOfAccount.ReferenceTypeInt;
                        _oChartsOfAccount.AccountTypeInInt = oChartsOfAccount.AccountTypeInInt;
                        _oChartsOfAccount.ReferenceTypeInt = oChartsOfAccount.ReferenceTypeInt;
                        _oChartsOfAccount.ParentHeadID = oChartsOfAccount.AccountHeadID;

                        _oChartsOfAccounts.Add(_oChartsOfAccount);
                    }
                }
                if (oBanks.Count > 0)
                {
                    foreach (Bank oItem in oBanks)
                    {
                        _oChartsOfAccount = new ChartsOfAccount();
                        _oChartsOfAccount.ReferenceObjectID = oItem.BankID;
                        _oChartsOfAccount.ReferenceType = oChartsOfAccount.ReferenceType;
                        _oChartsOfAccount.AccountHeadName = oItem.Name;
                        _oChartsOfAccount.ReferenceType = (EnumReferenceType)oChartsOfAccount.ReferenceTypeInt;
                        _oChartsOfAccount.AccountTypeInInt = oChartsOfAccount.AccountTypeInInt;
                        _oChartsOfAccount.ReferenceTypeInt = oChartsOfAccount.ReferenceTypeInt;
                        _oChartsOfAccount.ParentHeadID = oChartsOfAccount.AccountHeadID;
                        _oChartsOfAccounts.Add(_oChartsOfAccount);
                    }
                }
                if (oBankAccounts.Count > 0)
                {
                    foreach (BankAccount oItem in oBankAccounts)
                    {
                        _oChartsOfAccount = new ChartsOfAccount();
                        _oChartsOfAccount.ReferenceObjectID = oItem.BankAccountID;
                        _oChartsOfAccount.ReferenceType = oChartsOfAccount.ReferenceType;
                        _oChartsOfAccount.AccountHeadName = oItem.AccountNameandNo + "[" + oItem.BankNameBranch + "]";

                        _oChartsOfAccount.AccountType = (EnumAccountType)oChartsOfAccount.AccountTypeInInt;
                        _oChartsOfAccount.ReferenceType = (EnumReferenceType)oChartsOfAccount.ReferenceTypeInt;
                        _oChartsOfAccount.AccountTypeInInt = oChartsOfAccount.AccountTypeInInt;
                        _oChartsOfAccount.ReferenceTypeInt = oChartsOfAccount.ReferenceTypeInt;
                        _oChartsOfAccount.ParentHeadID = oChartsOfAccount.AccountHeadID;

                        _oChartsOfAccounts.Add(_oChartsOfAccount);
                    }
                }
            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
                _oChartsOfAccounts.Add(_oChartsOfAccount);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_DynamicCOA(List<ChartsOfAccount> oChartsOfAccounts)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            try
            {
                foreach (ChartsOfAccount oItem in oChartsOfAccounts)
                {
                    _oChartsOfAccount = new ChartsOfAccount();
                    oItem.AccountType = EnumAccountType.Ledger;
                    _oChartsOfAccount = oItem.Save((int)Session[SessionInfo.currentUserID]);
                    if (_oChartsOfAccount.ErrorMessage != "")
                    {
                        _oChartsOfAccounts = new List<ChartsOfAccount>();
                        _oChartsOfAccounts.Add(_oChartsOfAccount);
                        break;
                    }
                    _oChartsOfAccounts.Add(_oChartsOfAccount);
                }

            }
            catch (Exception ex)
            {
                _oChartsOfAccounts = new List<ChartsOfAccount>();
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
                _oChartsOfAccounts.Add(_oChartsOfAccount);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Update_COA(COA_AccountHeadConfig oCOA_AccountHeadConfig)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            COA_AccountHeadConfig oAHC = new COA_AccountHeadConfig();
            try
            {
                oAHC = oCOA_AccountHeadConfig;
                oAHC = oAHC.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAHC = new COA_AccountHeadConfig();
                oAHC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAHC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update_DynamicHead(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccount = new ChartsOfAccount();
            try
            {
                _oChartsOfAccount = oChartsOfAccount;
                _oChartsOfAccount = _oChartsOfAccount.Update_DynamicHead((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oChartsOfAccount = new ChartsOfAccount();
                _oChartsOfAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewChartsOfAccountsPiker()
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();

            return PartialView(oChartsOfAccount);
        }

        [HttpPost]
        public JsonResult GetChartsOfAccounts(ChartsOfAccount oChartsOfAccount)
        {
            if (oChartsOfAccount.AccountHeadName == null) oChartsOfAccount.AccountHeadName = "";
            _oChartsOfAccounts = new List<ChartsOfAccount>();
          //  int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Ledger + " AND AccountHeadName LIKE ('%" + oChartsOfAccount.AccountHeadName + "%') ";
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChartsOfAccountsForClosingStock(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();   
            string sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountType=5 AND HH.ParentHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType=" + ((int)EnumCISSetup.Inventory_Head).ToString() + ") ";
            if(!string.IsNullOrEmpty(oChartsOfAccount.AccountHeadName))
            {
                sSQL += " AND AccountHeadName LIKE '%"+oChartsOfAccount.AccountHeadName+"%'";
            }
            sSQL += " ORDER BY HH.AccountHeadName";
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Old Task
        [HttpGet]
        public JsonResult GetData(int id)
        {
            return Json("success" + id.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AccountHeadPiker(int nParentId)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID=" + nParentId + "";
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
               
        public ActionResult COARule(int nAccountHeadID)
        {
            if (nAccountHeadID <= 0)
            {
                return RedirectToAction("RefreshList");
            }
            ChartsOfAccount oCOA = new ChartsOfAccount();
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            oCOA = oCOA.Get(nAccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.AccountHead = "Selected account head : " + oCOA.AccountHeadNameCode;
            string Sql = "SELECT * FROM View_COA_ChartOfAccountCostCenter WHERE AccountHeadID=" + nAccountHeadID + "";
            _oChartsOfAccountCostCenter = new COA_ChartOfAccountCostCenter();
            _oChartsOfAccountCostCenter.AccountHeadID = oCOA.AccountHeadID;
            _oChartsOfAccountCostCenter.LstChartOfAccountcostCenter = COA_ChartOfAccountCostCenter.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
            return View(_oChartsOfAccountCostCenter);
        }
        
        [HttpPost]
        public ActionResult COARule(COA_ChartOfAccountCostCenter oChartsOfAccountCostCenter)
        {
            try
            {
                if (oChartsOfAccountCostCenter.SelectedCostCenterIDs.Length > 0)
                {
                    oChartsOfAccountCostCenter.SelectedCostCenterIDs = oChartsOfAccountCostCenter.SelectedCostCenterIDs.Remove(oChartsOfAccountCostCenter.SelectedCostCenterIDs.Length - 1, 1);
                }
                oChartsOfAccountCostCenter = oChartsOfAccountCostCenter.Save((int)Session[SessionInfo.currentUserID]);
                return RedirectToAction("RefreshList");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(oChartsOfAccountCostCenter);
            }
        }

        public ActionResult AccountHeadPikerWithCheckBox()
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            try
            {               
                _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);                
                return PartialView(_oChartsOfAccount);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return PartialView(_oChartsOfAccount);
            }
        }
        #endregion

        #region ChartsOfAccount New Actions
        public ActionResult ChartsOfAccounts()
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount.AccountTypeObjs = EnumObject.jGets(typeof(EnumAccountType));
            return PartialView(oChartsOfAccount);
        }
        [HttpPost]
        public JsonResult Refresh(ChartsOfAccount oChartsOfAccounts)
        {
            if (oChartsOfAccounts.AccountHeadName == null) oChartsOfAccounts.AccountHeadName = "";
            oChartsOfAccounts.AccountType = (EnumAccountType)oChartsOfAccounts.AccountTypeInInt;
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            string sSQL = "";
            if (oChartsOfAccounts.AccountType == EnumAccountType.Ledger)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Ledger + " AND AccountHeadName Like '%" + oChartsOfAccounts.AccountHeadName + "%' ";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.SubGroup)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.SubGroup + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%' )";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.Group)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Group + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%'))";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.Component)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Component + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%' )))";
            }
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Copy COA
        public ActionResult ViewCopyCOATree(double ts)
        {
            try
            {
                ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
                string sSQL = "SELECT * FROM View_ChartsOfAccount ORDER BY AccountHeadID";
                oChartsOfAccount.ChartsOfAccounts = ChartsOfAccount.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
                foreach (ChartsOfAccount oItem in oChartsOfAccount.ChartsOfAccounts)
                {
                    _oTChartsOfAccount = new TChartsOfAccount();
                    _oTChartsOfAccount.id = oItem.AccountHeadID;
                    _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                    _oTChartsOfAccount.text = oItem.AccountHeadName + "[" + oItem.AccountCode + "]";
                    _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                    _oTChartsOfAccount.code = oItem.AccountCode;
                    _oTChartsOfAccount.AccountTypeInString = oItem.AccountType.ToString();
                    _oTChartsOfAccount.Description = oItem.Description;
                    _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                    _oTChartsOfAccounts.Add(_oTChartsOfAccount);
                }
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount = GetRoot(0);
                this.AddTreeNodes(ref _oTChartsOfAccount);
                oChartsOfAccount.TChartsOfAccount = _oTChartsOfAccount;
                return PartialView(oChartsOfAccount);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return PartialView(_oChartsOfAccount);
            }
        }

        [HttpPost]
        public JsonResult RefreshForCopyCOA(ChartsOfAccount oChartsOfAccounts)
        {
            if (oChartsOfAccounts.AccountHeadName == null) oChartsOfAccounts.AccountHeadName = "";
            oChartsOfAccounts.AccountType = (EnumAccountType)oChartsOfAccounts.AccountTypeInInt;
            string sSQL = "";
            if (oChartsOfAccounts.AccountType == EnumAccountType.Ledger)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID NOT IN(SELECT AccountHeadID FROM CompanyWiseAccountHead WHERE CompanyID = "+(int)Session[SessionInfo.CurrentCompanyID] +") AND   AccountType = " + (int)EnumAccountType.Ledger + " AND AccountHeadName Like '%" + oChartsOfAccounts.AccountHeadName + "%'";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.SubGroup)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID NOT IN(SELECT AccountHeadID FROM CompanyWiseAccountHead WHERE CompanyID = " + (int)Session[SessionInfo.CurrentCompanyID] + ") AND     ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.SubGroup + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%')";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.Group)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountHeadID NOT IN(SELECT AccountHeadID FROM CompanyWiseAccountHead WHERE CompanyID = " + (int)Session[SessionInfo.CurrentCompanyID] + ") AND   ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Group + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%'))";
            }
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        [HttpPost]
        public JsonResult AccountHeadSetCompany(ChartsOfAccount oChartsOfAccount)
        {
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            oChartsOfAccount.IDs = oChartsOfAccount.IDs;         
            _oChartsOfAccounts = ChartsOfAccount.SaveCopyAccountHeads(oChartsOfAccount.IDs,(int)Session[SessionInfo.currentUserID]);
            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                _oTChartsOfAccount.code = oItem.AccountCode;
                _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                _oTChartsOfAccount.CName = oItem.CName;
                _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                _oTChartsOfAccount.AccountTypeInString = oItem.AccountType.ToString();
                _oTChartsOfAccount.PathName = oItem.PathName;
                _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                _oTChartsOfAccount.Description = oItem.Description;
                _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(0);
            this.AddTreeNodes(ref _oTChartsOfAccount);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MultipleCOA_Piker(int nComponentType, double ts)
        {
            string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountType="+(int)EnumAccountType.Ledger+" AND ComponentID=" + nComponentType + "";
            _oChartsOfAccount.ChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oChartsOfAccount);
        }


        [HttpPost]
        public JsonResult GetsByCodeOrName(ChartsOfAccount oChartsOfAccount)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            oChartsOfAccounts = ChartsOfAccount.GetsByCodeOrName(oChartsOfAccount, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult GetsByComponentAndCodeName(ChartsOfAccount oChartsOfAccount)
        {
            oChartsOfAccount.AccountHeadCodeName = oChartsOfAccount.AccountHeadCodeName == null ? "" : oChartsOfAccount.AccountHeadCodeName;
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            oChartsOfAccounts = ChartsOfAccount.GetsByComponentAndCodeName(oChartsOfAccount, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetsbyAccountTypeOrName(ChartsOfAccount oChartsOfAccount)
        {
            if(String.IsNullOrEmpty(oChartsOfAccount.AccountHeadName))
            {
                oChartsOfAccount.AccountHeadName = "";
            }
           _oChartsOfAccounts = new List<ChartsOfAccount>();
           _oChartsOfAccounts = ChartsOfAccount.GetsbyAccountTypeOrName(oChartsOfAccount.AccountTypeInInt, oChartsOfAccount.AccountHeadName, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsbyAccountTypeWithName(ChartsOfAccount oChartsOfAccount)
        {
            if (String.IsNullOrEmpty(oChartsOfAccount.AccountHeadName))
            {
                oChartsOfAccount.AccountHeadName = "";
            }
            string sSQL = "";
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            if ((EnumAccountType)oChartsOfAccount.AccountTypeInInt == EnumAccountType.SubGroup)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.AccountType = 5 AND COA.ParentHeadID IN (SELECT HH.AccountHeadID FROM ChartsOfAccount AS HH WHERE HH.AccountType = 4 AND HH.AccountHeadName LIKE '%" + oChartsOfAccount.AccountHeadName + "%')  ORDER BY COA.AccountHeadName ASC";
            }
            else
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.AccountType=5 AND COA.AccountHeadName LIKE '%" + oChartsOfAccount.AccountHeadName + "%' ORDER BY COA.AccountHeadName ASC";
            }
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsbyParentAndCodeName(ChartsOfAccount oChartsOfAccount)
        {
            oChartsOfAccount.AccountHeadCodeName = oChartsOfAccount.AccountHeadCodeName == null ? "" : oChartsOfAccount.AccountHeadCodeName;
            oChartsOfAccount.ParentHeadID = oChartsOfAccount.ParentHeadID <= 0 ? 1 : oChartsOfAccount.ParentHeadID;
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            string sSQL = "SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.ParentHeadID=" + oChartsOfAccount.ParentHeadID + " AND COA.AccountHeadCodeName LIKE '%" + oChartsOfAccount.AccountHeadCodeName + "%'";
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintInXL()
        {
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ChartsOfAccountXL>));
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            _oChartsOfAccountXLs = new List<ChartsOfAccountXL>();

            _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);

            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                _oTChartsOfAccount.code = oItem.AccountCode;
                _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                _oTChartsOfAccount.CName = oItem.CName;
                _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                _oTChartsOfAccount.AccountTypeInInt = (int)oItem.AccountType;
                _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                _oTChartsOfAccount.AOTypeInInt = (int)oItem.AccountOperationType;
                _oTChartsOfAccount.AOTypeSt = oItem.AccountOperationTypeSt;
                _oTChartsOfAccount.ParentAOTypeInInt = (int)oItem.ParentAccountOperationType;
                _oTChartsOfAccount.PathName = oItem.PathName;
                _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                _oTChartsOfAccount.Description = oItem.Description;
                _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(0);
            this.AddTreeNodes(ref _oTChartsOfAccount);

            //ChartsOfAccountXL oChartsOfAccountXL = new ChartsOfAccountXL();
            //oChartsOfAccountXL = new ChartsOfAccountXL();
            //oChartsOfAccountXL.AccountCode = _oTChartsOfAccount.code;
            //oChartsOfAccountXL.AccountType = _oTChartsOfAccount.AccountTypeInString;
            //oChartsOfAccountXL.AccountName = _oTChartsOfAccount.text;
            //_oChartsOfAccountXLs.Add(oChartsOfAccountXL);
            this.MapChartOfAccountInXL(_oTChartsOfAccount.children);

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, _oChartsOfAccountXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "ChartsOfAccount.xls");
        }

        public ActionResult PrintCOA()
        {
           
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            _oChartsOfAccountXLs = new List<ChartsOfAccountXL>();
            _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);

            foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                _oTChartsOfAccount.code = oItem.AccountCode;
                _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                _oTChartsOfAccount.CName = oItem.CName;
                _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                _oTChartsOfAccount.AccountTypeInInt = (int)oItem.AccountType;
                _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                _oTChartsOfAccount.AOTypeInInt = (int)oItem.AccountOperationType;
                _oTChartsOfAccount.AOTypeSt = oItem.AccountOperationTypeSt;
                _oTChartsOfAccount.ParentAOTypeInInt = (int)oItem.ParentAccountOperationType;
                _oTChartsOfAccount.PathName = oItem.PathName;
                _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                _oTChartsOfAccount.Description = oItem.Description;
                _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(0);
            this.AddTreeNodes(ref _oTChartsOfAccount);
                        
            this.MapChartOfAccountInXL(_oTChartsOfAccount.children);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);

            rptChartsOfAccounts oReport = new rptChartsOfAccounts();
            byte[] abytes = oReport.PrepareReport(_oChartsOfAccountXLs, oCompany);
            return File(abytes, "application/pdf");
           
        }

        private void MapChartOfAccountInXL(List<TChartsOfAccount> children)
        {
            ChartsOfAccountXL oChartsOfAccountXL = new ChartsOfAccountXL();
            foreach (TChartsOfAccount oItem in children)
            {
                oChartsOfAccountXL = new ChartsOfAccountXL();
                oChartsOfAccountXL.AccountCode = oItem.code;
                oChartsOfAccountXL.AccountType = oItem.AccountTypeInString;
                oChartsOfAccountXL.Currency = oItem.CSymbol;
                oChartsOfAccountXL.AccountName = oItem.text;
                _oChartsOfAccountXLs.Add(oChartsOfAccountXL);
               this.MapChartOfAccountInXL(oItem.children);
            }
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
        public ActionResult ViewChartOfAccountsTemp(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccount = new ChartsOfAccount();
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChartsOfAccount).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            try
            {
                _oChartsOfAccounts = ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]);
                foreach (ChartsOfAccount oItem in _oChartsOfAccounts)
                {
                    _oTChartsOfAccount = new TChartsOfAccount();
                    _oTChartsOfAccount.id = oItem.AccountHeadID;
                    _oTChartsOfAccount.parentid = oItem.ParentHeadID;
                    _oTChartsOfAccount.text = oItem.AccountHeadName;
                    _oTChartsOfAccount.attributes = oItem.IsJVNode.ToString();
                    _oTChartsOfAccount.code = oItem.AccountCode;
                    _oTChartsOfAccount.CurrencyID = oItem.CurrencyID;
                    _oTChartsOfAccount.CName = oItem.CName;
                    _oTChartsOfAccount.CSymbol = oItem.CSymbol;
                    _oTChartsOfAccount.AccountTypeInInt = (int)oItem.AccountType;
                    _oTChartsOfAccount.AccountTypeInString = oItem.AccountTypeInString;
                    _oTChartsOfAccount.AOTypeInInt = (int)oItem.AccountOperationType;
                    _oTChartsOfAccount.AOTypeSt = oItem.AccountOperationTypeSt;
                    _oTChartsOfAccount.ParentAOTypeInInt = (int)oItem.ParentAccountOperationType;
                    _oTChartsOfAccount.PathName = oItem.PathName;
                    _oTChartsOfAccount.ComponentID = oItem.ComponentID;
                    _oTChartsOfAccount.InventoryHeadID = oItem.InventoryHeadID;
                    _oTChartsOfAccount.Description = oItem.Description;
                    _oTChartsOfAccount.IsjvNode = oItem.IsJVNode;
                    _oTChartsOfAccount.Amount = oItem.Amount;
                    _oTChartsOfAccounts.Add(_oTChartsOfAccount);
                }
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount = GetRoot(0);
                this.AddTreeNodes(ref _oTChartsOfAccount);

                ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
                oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
                ViewBag.BUs = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
                ViewBag.AOTs = EnumObject.jGets(typeof(EnumAccountOperationType));
                string sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountType=5 AND HH.ParentHeadID IN (SELECT CIS.AccountHeadID FROM CIStatementSetup AS CIS WHERE CIS.CIHeadType=" + ((int)EnumCISSetup.Inventory_Head).ToString() + ") ORDER BY HH.AccountHeadName";
                ViewBag.InventoryEffects = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
                return View(_oTChartsOfAccount);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTChartsOfAccount);
            }
        }
        public JsonResult SaveEditList(ChartsOfAccount oChartsOfAccounts)
        {
            List<TChartsOfAccount> oTChartsOfAccounts = new List<TChartsOfAccount>();
            oTChartsOfAccounts = oChartsOfAccounts.TChartsOfAccounts;
            string AlertMessage = "";
            foreach (TChartsOfAccount oItem in oTChartsOfAccounts)
            {
                if (oItem.Amount > 0)
                {
                    AlertMessage = AlertMessage + "Code: " + oItem.code + " || Head: " + oItem.text + " || Ammount: " + oItem.Amount + Environment.NewLine;
                }
            }
            _oTChartsOfAccount.ErrorMessage = AlertMessage;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTChartsOfAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
    }
}
