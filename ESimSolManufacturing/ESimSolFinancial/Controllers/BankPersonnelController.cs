using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;


namespace ESimSolFinancial.Controllers
{
    public class BankPersonnelController : Controller
    {
        #region Declaration
        BankPersonnel _oBankPersonnel = new BankPersonnel();
        List<BankPersonnel> _oBankPersonnels = new List<BankPersonnel>();
        BankBranch _oBankBranch = new BankBranch();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private bool ValidateInput(BankPersonnel oBankPersonnel)
        {
            if (oBankPersonnel.BankID == null || oBankPersonnel.BankID<=0)
            {
                _sErrorMessage = "Invalid Bank Please try again";
                return false;
            }
            if (oBankPersonnel.Name == null || oBankPersonnel.Name == "")
            {
                _sErrorMessage = "Please enter Personnel Name";
                return false;
            }
            if (oBankPersonnel.Phone == null || oBankPersonnel.Phone == "")
            {
                _sErrorMessage = "Please enter Personnel Phone ";
                return false;
            }
            return true;
        }
        #endregion

        public ActionResult ViewBankPersonnels(int id)
        {
            _oBankBranch = new BankBranch();
            if (id > 0)
            {
                _oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBankBranch.BankPersonnels = BankPersonnel.GetsByBankBranch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return PartialView(_oBankBranch);
        }

        public ActionResult ViewBankPersonnel(int id, int nid)
        {
            _oBankPersonnel = new BankPersonnel();
            if (id > 0)
            {
                _oBankPersonnel = _oBankPersonnel.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                BankBranch oBankBranch = new BankBranch();
                oBankBranch = BankBranch.Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBankPersonnel.BankID = oBankBranch.BankID;
                _oBankPersonnel.BankBranchID = oBankBranch.BankBranchID;
            }
            return PartialView(_oBankPersonnel);
        }

        [HttpPost]
        public JsonResult Save(BankPersonnel oBankPersonnel)
        {
            _oBankPersonnel = new BankPersonnel();
            try
            {
                _oBankPersonnel = oBankPersonnel;
                _oBankPersonnel = _oBankPersonnel.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBankPersonnel = new BankPersonnel();
                _oBankPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                BankPersonnel oBankPersonnel = new BankPersonnel();
                sErrorMease = oBankPersonnel.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
