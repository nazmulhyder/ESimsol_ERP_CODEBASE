using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using System.Drawing.Printing;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class ClassController : Controller
    {
        #region Declaration
        Bank _oBank = new Bank();
        List<Bank> _oBanks = new List<Bank>();
        string _sErrorMessage = "";
      
        #endregion

        #region Functions


        #region New CodeD:\Local\Faruk\Project\B007\ESimSolManufacturing\ESimSolFinancial\Controllers\BankController.cs
        public ActionResult ViewBanks(int menuid)
        {
            try
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
                //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'Bank', 'BankBranch'", (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));

                _oBanks = new List<Bank>();
                _oBanks = Bank.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.AccountTypes = Enum.GetValues(typeof(EnumBankAccountType)).Cast<EnumBankAccountType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                return View(_oBanks);
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "User");
            }
        }
        public ActionResult ViewBank(int id, double ts)
        {
            _oBank = new Bank();
            if (id > 0)
            {
                _oBank = _oBank.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oBank);
        }

        public ActionResult ViewBankBranchs(int id, double ts)
        {
            _oBank = new Bank();
            if (id > 0)
            {
                _oBank = _oBank.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBank.BankBranchs = BankBranch.GetsByBank(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oBank);
        }

        public ActionResult ViewBankBranch(int id, int nid, double ts)
        {
            BankBranch oBankBranch = new BankBranch();
            List<BankBranchDept> oBankBranchDepts = new List<BankBranchDept>();
            if (id > 0)
            {
                oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankBranch.BankBranchBUs = BankBranchBU.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oBankBranch.BankID = nid;
            }
            List<EnumObject> oEnumOperationalDepts = EnumObject.jGets(typeof(EnumOperationalDept));
            foreach (EnumObject oItem in oEnumOperationalDepts)
            {
                BankBranchDept oBankBranchDept = new BankBranchDept();
                oBankBranchDept.BankBranchID = id;
                oBankBranchDept.OperationalDept = (EnumOperationalDept)oItem.id;
                oBankBranchDept.OperationalDeptInInt = oItem.id;
                oBankBranchDept.OperationalDeptName = oItem.Value;
                if (oBankBranchDept.OperationalDeptInInt > 0)
                {
                    oBankBranchDepts.Add(oBankBranchDept);
                }
            }
            ViewBag.OperationalDepts = oBankBranchDepts;
            ViewBag.BankBranchDepts = BankBranchDept.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            
            return View(oBankBranch);
        }
        public ActionResult ViewBankAccounts(int id)
        {

            BankBranch oBankBranch = new BankBranch();
            if (id > 0)
            {
                oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankBranch.BankAccounts = BankAccount.GetsByBankBranch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(oBankBranch);
        }

        public ActionResult ViewBankAccount(int id, int nid)
        {
            BankAccount oBankAccount = new BankAccount();
            if (id > 0)
            {
                oBankAccount = oBankAccount.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                BankBranch oBankBranch = new BankBranch();
                oBankBranch = BankBranch.Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankAccount.BankID = oBankBranch.BankID;
                oBankAccount.BankBranchID = oBankBranch.BankBranchID;
            }
            return View(oBankAccount);
        }
        public ActionResult ViewBankPersonnels(int id)
        {
            BankBranch oBankBranch = new BankBranch();
            if (id > 0)
            {
                oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankBranch.BankPersonnels = BankPersonnel.GetsByBankBranch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(oBankBranch);
        }

        public ActionResult ViewBankPersonnel(int id, int nid)
        {
            BankPersonnel oBankPersonnel = new BankPersonnel();
            if (id > 0)
            {
                oBankPersonnel = oBankPersonnel.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                BankBranch oBankBranch = new BankBranch();
                oBankBranch = BankBranch.Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankPersonnel.BankID = oBankBranch.BankID;
                oBankPersonnel.BankBranchID = oBankBranch.BankBranchID;
            }
            return View(oBankPersonnel);
        }

        [HttpPost]
        public JsonResult Save(Bank oBank)
        {
            _oBank = new Bank();
            try
            {
                _oBank = oBank;
                _oBank = _oBank.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBank = new Bank();
                _oBank.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBank);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Bank oBank)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBank.Delete(oBank.BankID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPIAddress(Bank oBank)
        {
            string ip = "";
            try
            {
                ip = Request.UserHostAddress;
            }
            catch (Exception ex)
            {
                ip = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(ip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(Bank oBank)
        {
            _oBank = new Bank();
            try
            {
                if (oBank.BankID > 0)
                {
                    _oBank = _oBank.Get(oBank.BankID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oBank = new Bank();
                _oBank.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBank);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(Bank oBank)
        {
            string sSQL = "";
            oBank.Name = oBank.Name == null ? "" : oBank.Name;
            sSQL = "SELECT * FROM View_Bank WHERE Name LIKE '%" + oBank.Name + "%'";
            _oBanks = new List<Bank>();
            _oBanks = Bank.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBanks);
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

        #endregion



        #region Old Code

        private bool ValidateInput(Bank oBank)
        {
            if (string.IsNullOrEmpty(oBank.Name))
            {
                _sErrorMessage = "Please enter Bank Name";
                return false;
            }
            if (string.IsNullOrEmpty(oBank.ShortName))
            {
                _sErrorMessage = "Please insert Bank Initial";
                return false;
            }
            return true;
        }
        #endregion


        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                Bank oBank = new Bank();
                sFeedBackMessage = oBank.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<Bank> oBanks = new List<Bank>();
            Bank oBank = new Bank();
            oBank.Name = "-- Select Bank --";
            oBanks.Add(oBank);
            oBanks.AddRange(Bank.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBanks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
