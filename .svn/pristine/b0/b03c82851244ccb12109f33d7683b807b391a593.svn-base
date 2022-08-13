using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class BankAccountController : Controller
    {
        #region Declaration
        BankAccount _oBankAccount = new BankAccount();
        List<BankAccount> _oBankAccounts = new List<BankAccount>();
        string _sErrorMessage = "";
        BankBranch _oBankBranch = new BankBranch();
        #endregion

      

        #region Functions
      
        [HttpPost]
        public JsonResult Save(BankAccount oBankAccount)
        {
            _oBankAccount = new BankAccount();
            try
            {
                _oBankAccount = oBankAccount;
                _oBankAccount.AccountType = (EnumBankAccountType)oBankAccount.AccountTypeInInt;
                _oBankAccount = _oBankAccount.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBankAccount = new BankAccount();
                _oBankAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                BankAccount oBankAccount = new BankAccount();
                sErrorMease = oBankAccount.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<BankAccount> oBankAccounts = new List<BankAccount>();
            BankAccount oBankAccount = new BankAccount();
            oBankAccount.AccountNo = "-- Select Account --";
            oBankAccounts.Add(oBankAccount);
            oBankAccounts.AddRange(BankAccount.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBankAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets(BankAccount oBankAccount)
        {
            List<BankAccount> oBankAccounts = new List<BankAccount>();
            string sSQL,sTempSQL = "";
            string sAccountNo = oBankAccount.AccountNo == null ? "" : oBankAccount.AccountNo;
            string sBusinessUnitNameCode = oBankAccount.BusinessUnitNameCode == null ? "" : oBankAccount.BusinessUnitNameCode;
            sSQL = "SELECT * FROM View_BankAccount ";
            sTempSQL="";
            #region AccountNo
            if (sAccountNo != null)
            {
                if (sAccountNo != "")
                {
                        Global.TagSQL(ref sTempSQL);
                        sTempSQL = sTempSQL + " AccountNo LIKE '%" + sAccountNo + "%' ";
                }
            }
            #endregion
            #region BusinessUnitNameCode
            if (sBusinessUnitNameCode != null)
            {
                if (sBusinessUnitNameCode != "")
                {
                         Global.TagSQL(ref sTempSQL);
                         sTempSQL = sTempSQL + " BusinessUnitNameCode LIKE '%" + sBusinessUnitNameCode + "%' ";
                }
            }
            #endregion
            if (sTempSQL != "")
            { sSQL = sSQL + sTempSQL; }
            oBankAccounts = BankAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBankAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByBranchAndAccount(BankAccount oBankAccount)
        {
            List<BankAccount> oBankAccounts = new List<BankAccount>();
            string sSQL = "";
            if (oBankAccount.AccountNo == null)
            {
                oBankAccount.AccountNo = "";
            }
            if (oBankAccount.AccountNo == "")
            {
                sSQL = "SELECT * FROM View_BankAccount WHERE BankBranchID = " + oBankAccount.BankBranchID.ToString() + " ORDER BY AccountNo";
            }
            else
            {
                sSQL = "SELECT * FROM View_BankAccount WHERE BankBranchID = " + oBankAccount.BankBranchID.ToString() + " AND AccountNo LIKE '%" + oBankAccount.AccountNo + "%' ORDER BY AccountNo";
            }
            oBankAccounts = BankAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBankAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBUWithDeptWise(BankAccount oBankAccount)
        {
            List<BankAccount> oBankAccounts = new List<BankAccount>();
            string sSQL, sTempSQL = "";
            string sAccountNo = oBankAccount.AccountNo == null ? "" : oBankAccount.AccountNo;
            sSQL = "SELECT * FROM View_BankAccount ";
            sTempSQL = "";
            #region AccountNo
            if (sAccountNo != null)
            {
                if (sAccountNo != "")
                {
                    Global.TagSQL(ref sTempSQL);
                    sTempSQL = sTempSQL + " AccountNo LIKE '%" + sAccountNo + "%' ";
                }
            }
            #endregion
            #region BusinessUnitID
            if (oBankAccount.BusinessUnitID>0)
            {
                    Global.TagSQL(ref sTempSQL);
                    sTempSQL = sTempSQL + " BusinessUnitID = " + oBankAccount.BusinessUnitID;
            }
            #endregion

            #region Depts
            if (oBankAccount.OperationalDeptInInt> 0)
            {
                Global.TagSQL(ref sTempSQL);
                sTempSQL = sTempSQL + " BankBranchID IN (SELECT BankBranchID FROM BankBranchDept WHERE OperationalDept = "+(int)(EnumOperationalDept)oBankAccount.OperationalDeptInInt+")";
            }
            #endregion

            if (sTempSQL != "")
            { sSQL = sSQL + sTempSQL; }
            oBankAccounts = BankAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBankAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
