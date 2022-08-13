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
    public class BankBranchController : Controller
    {
        #region Declaration
        BankBranch _oBankBranch = new BankBranch();
        List<BankBranch> _oBankBranchs = new List<BankBranch>();
        string _sErrorMessage = "";
        DateTime dt = DateTime.Today;
        Bank _oBank = new Bank();
        #endregion

        #region New Code
        [HttpPost]
        public JsonResult Save(BankBranch oBankBranch)
        {
            _oBankBranch = new BankBranch();
            try
            {
                _oBankBranch = oBankBranch;
                _oBankBranch = _oBankBranch.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBankBranch = new BankBranch();
                _oBankBranch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankBranch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(BankBranch oBankBranch)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oBankBranch.Delete(oBankBranch.BankBranchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsBankBranchSearchByBankName(BankBranch oBankBranch) 
        {
            _oBankBranchs = new List<BankBranch>();
            try
            {
                //string sSQL = "";

                //if (string.IsNullOrEmpty(oBankBranch.BankName)) 
                //{
                //    sSQL = sSQL + "Select * from View_BankBranch  order by BankName"; 
                //}
                //else
                //{
                //    oBankBranch.BankName = oBankBranch.BankName.Trim();
                //    sSQL = sSQL + "Select * from View_BankBranch where BranchName Like '%" + oBankBranch.BankName + "%' OR BankName Like '%" + oBankBranch.BankName + "%' order by BankName"; 
                //}
                _oBankBranchs = BankBranch.GetsByDeptAndBU(oBankBranch.DeptIDs,oBankBranch.BUID, oBankBranch.BankName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oBankBranchs.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oBankBranch = new BankBranch();
                oBankBranch.ErrorMessage = ex.Message;
                _oBankBranchs.Add(oBankBranch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankBranchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBankAndBranchkName(BankBranch oBankBranch) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            _oBankBranchs = new List<BankBranch>();
            try
            {
                if (oBankBranch.BranchName == null)
                {
                    oBankBranch.BranchName = "";
                }
                string sSQL = "";
                if(oBankBranch.BranchName.Trim() == "")
                {
                    sSQL = sSQL + "Select * from View_BankBranch where BankID = " + oBankBranch.BankID.ToString() + " order by BranchName";
                }
                else
                {
                    sSQL = sSQL + "Select * from View_BankBranch where BankID = " + oBankBranch.BankID.ToString() + " AND BranchName Like '%" + oBankBranch.BranchName + "%' order by BranchName";
                }
                _oBankBranchs = BankBranch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oBankBranchs.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oBankBranch = new BankBranch();
                oBankBranch.ErrorMessage = ex.Message;
                _oBankBranchs.Add(oBankBranch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankBranchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsBankBranch(BankBranch oBankBranch) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            _oBankBranchs = new List<BankBranch>();
            try
            {
             
                string sSQL = "";
                if (oBankBranch.BankBranchID>0)
                {
                    sSQL = sSQL + "Select * from View_BankBranch where BankID in (Select BankBranch.BankID from BankBranch where BankBranchID=" + oBankBranch.BankBranchID.ToString() + ")";
                }
                else if (oBankBranch.BankID > 0)
                {
                    sSQL = sSQL + "Select * from View_BankBranch where BankID = " + oBankBranch.BankID.ToString() + " order by BranchName";
                }
                _oBankBranchs = BankBranch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oBankBranchs.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oBankBranch = new BankBranch();
                oBankBranch.ErrorMessage = ex.Message;
                _oBankBranchs.Add(oBankBranch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankBranchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBankByBranchkName(BankBranch oBankBranch)
        {
            _oBankBranchs = new List<BankBranch>();
            try
            {
                if (oBankBranch.BranchName == null)
                {
                    oBankBranch.BranchName = "";
                }
                string sSQL = "";
                if (oBankBranch.BranchName.Trim() == "")
                {
                    sSQL = sSQL + "SELECT * FROM View_BankBranch";
                }
                else
                {
                    sSQL = sSQL + "SELECT * FROM View_BankBranch WHERE BranchName LIKE '%" + oBankBranch.BranchName.ToString() + "%' ORDER BY BranchName";
                }
                _oBankBranchs = BankBranch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oBankBranchs.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oBankBranch = new BankBranch();
                oBankBranch.ErrorMessage = ex.Message;
                _oBankBranchs.Add(oBankBranch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankBranchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ATMLNewGetsByBank(BankBranch oBankBranch)
        {
            _oBankBranchs = new List<BankBranch>();
            if (oBankBranch.BankID > 0)
            {
                _oBankBranchs = BankBranch.GetsByBank(oBankBranch.BankID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankBranchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Old Code
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

        public ActionResult ViewBankBranch(int id, int nid)
        {
            _oBankBranch = new BankBranch();
            if (id > 0)
            {
                _oBankBranch = BankBranch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oBankBranch.BankID = nid;
            }
            return View(_oBankBranch);
        }

       

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                BankBranch oBankBranch = new BankBranch();
                sErrorMease = oBankBranch.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

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
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            BankBranch oBankBranch = new BankBranch();
            oBankBranch.BranchName = "-- Select Bank Branch --";
            oBankBranchs.Add(oBankBranch);
            oBankBranchs.AddRange(BankBranch.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBankBranchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
