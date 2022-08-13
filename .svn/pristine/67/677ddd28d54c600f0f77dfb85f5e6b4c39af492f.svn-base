using System;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data;
using System.Data.OleDb;
using System.Net.Mail;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class PFSchemeController : Controller
    {
        #region Declaration
        private PFScheme _oPF = new PFScheme();
        private List<PFScheme> _oPFs = new List<PFScheme>();

        #endregion

        #region PF Scheme
        public ActionResult ViewPFSchemes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPFs = new List<PFScheme>();

            string sSQL = "Select * from View_PFScheme Where PFSchemeID<>0";
            _oPFs = PFScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oPFs);
        }

        public ActionResult ViewPFScheme(int nId)
        {
            PFScheme oPF = new PFScheme();
            string sSQL = "";
            if (nId != null && nId > 0)
            {
                oPF = PFScheme.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPF.PFSchemeID > 0)
                {
                    sSQL = "SELECT * FROM PFMemberContribution Where PFMCID<>0 And PFSchemeID=" + oPF.PFSchemeID + "";
                    oPF.PFMCs = PFMemberContribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSQL = "Select * from PFSchemeBenefit Where PFSchemeID=" + oPF.PFSchemeID + "";
                    oPF.PFBs = PFSchemeBenefit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            sSQL = "Select * from SalaryHead Where SalaryHeadType=3";
            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            oSalaryHeads = SalaryHead.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSQL = "Select * from Currency Where CurrencyID =(Select BaseCurrencyID from Company Where CompanyID=" + ((User)(Session[SessionInfo.CurrentUser])).CompanyID + ")";

            Company oCompany = new Company();
            ViewBag.CurrencySymbol = oCompany.Get(((User)(Session[SessionInfo.CurrentUser])).CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID).CurrencySymbol;
            ViewBag.SalaryHeads = oSalaryHeads;
            ViewBag.CalculationOns = Enum.GetValues(typeof(EnumPayrollApplyOn)).Cast<EnumPayrollApplyOn>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.RecruitmentEvents = Enum.GetValues(typeof(EnumRecruitmentEvent)).Cast<EnumRecruitmentEvent>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();


            return View(oPF);
        }
      
        
        [HttpPost]
        public JsonResult SavePF(PFScheme oPF)
        {
            try
            {
                if (oPF.PFSchemeID <= 0)
                {
                    oPF = oPF.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oPF = oPF.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oPF = new PFScheme();
                oPF.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPF);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePF(PFScheme oPF)
        {
            try
            {
                if (oPF.PFSchemeID <= 0) { throw new Exception("Please select an valid item."); }

                oPF = oPF.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPF = new PFScheme();
                oPF.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPF.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovePF(PFScheme oPF)
        {
            try
            {
                if (oPF.PFSchemeID <= 0) { throw new Exception("Please select an valid item."); }
                oPF = oPF.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPF = new PFScheme();
                oPF.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPF);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityChangePF(PFScheme oPF)
        {
            try
            {
                if (oPF.PFSchemeID <= 0) { throw new Exception("Please select an valid item."); }
                oPF.IsActive = !oPF.IsActive;
                oPF = oPF.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPF = new PFScheme();
                oPF.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPF);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Search PFScheme
        [HttpPost]
        public ActionResult GetPF(PFScheme oPF)
        {
            try
            {
                if (oPF.PFSchemeID <= 0) { throw new Exception("Please select an valid item."); }
                oPF = PFScheme.Get(oPF.PFSchemeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPF.PFSchemeID > 0)
                {
                    string sSQL = "Select * from PFSchemeBenefit Where PFSchemeID=" + oPF.PFSchemeID + "";
                    oPF.PFBs = PFSchemeBenefit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oPF = new PFScheme();
                oPF.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPF);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Gets(PFScheme oPF)
        {
            List<PFScheme> oPFSchemes = new List<PFScheme>();
            try
            {
                if (oPF.Name.Trim() == "") { throw new Exception("Please PF Scheme Name to Search."); }

                string sSQL = "Select * from View_PFScheme Where Name Like '%" + oPF.Name.Trim() + "%'";
                oPFSchemes = PFScheme.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oPF = new PFScheme();
                oPFSchemes = new List<PFScheme>();
                oPF.ErrorMessage = ex.Message;
                oPFSchemes.Add(oPF);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFSchemes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion

        #region PF Scheme Employee Contributions

        [HttpPost]
        public JsonResult SavePFMC(PFMemberContribution oPFMC)
        {
            try
            {
                if (oPFMC.PFMCID <= 0)
                {
                    oPFMC = oPFMC.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oPFMC = oPFMC.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oPFMC = new PFMemberContribution();
                oPFMC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFMC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePFMC(PFMemberContribution oPFMC)
        {
            try
            {
                if (oPFMC.PFMCID <= 0) { throw new Exception("Please select an valid item."); }

                oPFMC = oPFMC.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFMC = new PFMemberContribution();
                oPFMC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFMC.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityChangePFMC(PFMemberContribution oPFMC)
        {
            try
            {
                if (oPFMC.PFMCID <= 0) { throw new Exception("Please select an valid item."); }
                oPFMC.IsActive = !oPFMC.IsActive;
                oPFMC = oPFMC.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFMC = new PFMemberContribution();
                oPFMC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFMC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PF Scheme Benefits

        [HttpPost]
        public JsonResult SavePFB(PFSchemeBenefit oPFB)
        {
            try
            {
                if (oPFB.PFSBID <= 0)
                {
                    oPFB = oPFB.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oPFB = oPFB.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oPFB = new PFSchemeBenefit();
                oPFB.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFB);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePFB(PFSchemeBenefit oPFB)
        {
            try
            {
                if (oPFB.PFSBID <= 0) { throw new Exception("Please select an valid item."); }

                oPFB = oPFB.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFB = new PFSchemeBenefit();
                oPFB.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFB.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityChangePFB(PFSchemeBenefit oPFB)
        {
            try
            {
                if (oPFB.PFSBID <= 0) { throw new Exception("Please select an valid item."); }
                oPFB.IsActive = !oPFB.IsActive;
                oPFB = oPFB.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFB = new PFSchemeBenefit();
                oPFB.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFB);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PF Member
        public ActionResult ViewPFMembers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<PFmember> oPFMs = new List<PFmember>();

            string sSQL = "";
            //string sSQL = "Select * from View_PFmember Where PFMID<>0 ";
            //oPFMs = PFmember.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSQL = "Select * from View_PFScheme Where PFSchemeID<>0 And ApproveBy<>0 And IsActive=1";
            List<PFScheme> oPFs = new List<PFScheme>();
            oPFs = PFScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            PFScheme oPF = new PFScheme();
            if (oPFs.Count() > 0)
            {
                oPF = oPFs.ElementAtOrDefault(0);
                sSQL = "Select * from PFSchemeBenefit Where PFSchemeID=" + oPF.PFSchemeID + "";
                oPF.PFBs = PFSchemeBenefit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.PFScheme = oPF;

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            return View(oPFMs);
        }

        public ActionResult ViewPFMember(int nId)
        {
            PFmember oPFM = new PFmember();
            PFScheme oPF = new PFScheme();
            string sSQL = "";
            if (nId > 0)
            {
                oPFM = PFmember.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oPFM.PFSchemeID > 0)
                {
                    sSQL = "SELECT * FROM PFMemberContribution Where PFMCID<>0 And PFSchemeID=" + oPFM.PFSchemeID + " And IsActive=1";
                    oPF.PFMCs = PFMemberContribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sSQL = "Select * from PFSchemeBenefit Where PFSchemeID=" + oPFM.PFSchemeID + " And IsActive=1";
                    oPFM.PFBs = PFSchemeBenefit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }

            sSQL = "Select top(1)* from View_PFScheme Where PFSchemeID<>0 And ApproveBy<>0 And IsActive=1";
            List<PFScheme> oPFs = new List<PFScheme>();
            oPFs = PFScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oPFs.Count() > 0)
            {
                oPF = oPFs.ElementAtOrDefault(0);
                sSQL = "SELECT * FROM PFMemberContribution Where PFMCID<>0 And PFSchemeID=" + oPF.PFSchemeID + " And IsActive=1";
                oPF.PFMCs = PFMemberContribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from PFSchemeBenefit Where PFSchemeID=" + oPF.PFSchemeID + " And IsActive=1";
                oPF.PFBs = PFSchemeBenefit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.PFScheme = oPF;
            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            return View(oPFM);
        }

        [HttpPost]
        public JsonResult SavePFM(PFmember oPFM)
        {
            List<PFmember> oPFMs = new List<PFmember>();
            try
            {
                foreach (PFmember oItem in oPFM.PFmembers)
                {
                    PFmember oPFMemeber = new PFmember();
                    oPFMemeber = oPFM;
                    oPFMemeber.EmployeeID = oItem.EmployeeID;
                    oPFMemeber.PFMembershipDate = oItem.PFMembershipDate;
                    if (oPFMemeber.PFMID <= 0)
                    {
                        oPFMemeber = oPFMemeber.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        oPFMemeber = oPFMemeber.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    oPFMs.Add(oPFMemeber);
                }
            }
            catch (Exception ex)
            {
                oPFMs = new List<PFmember>();
                oPFM = new PFmember();
                oPFM.ErrorMessage = ex.Message;
                oPFMs.Add(oPFM);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePFM(PFmember oPFM)
        {
            try
            {
                if (oPFM.PFMID <= 0) { throw new Exception("Please select an valid item."); }

                oPFM = oPFM.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFM = new PFmember();
                oPFM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFM.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PFmember_Delete(string sPFmemberIDs)
        {
            PFmember oPFmember = new PFmember();
            try
            {
                PFmember.Gets("DELETE FROM PFmember WHERE PFMID IN("+sPFmemberIDs+")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oPFmember = new PFmember();
                oPFmember.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFmember.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovePFM(PFmember oPFM)
        {
            try
            {
                if (oPFM.PFMID <= 0) { throw new Exception("Please select an valid item."); }
                oPFM = oPFM.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFM = new PFmember();
                oPFM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityChangePFM(PFmember oPFM)
        {
            try
            {
                if (oPFM.PFMID <= 0) { throw new Exception("Please select an valid item."); }
                oPFM.IsActive = !oPFM.IsActive;
                oPFM = oPFM.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFM = new PFmember();
                oPFM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPFM(PFmember oPFM)
        {
            try
            {
                if (oPFM.PFMID <= 0) { throw new Exception("Please select an valid item."); }
                oPFM = PFmember.Get(oPFM.PFMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPFM.PFMID > 0)
                {
                    string sSQL = "Select * from PFSchemeBenefit Where PFSchemeID=" + oPFM.PFSchemeID + "";
                    oPFM.PFBs= PFSchemeBenefit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oPFM = new PFmember();
                oPFM.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetsPFMember(PFmember oPFM)
        {
            List<PFmember> oPFMs = new List<PFmember>();
            try
            {
                string sPFSchemeIDs = oPFM.Params.Split('~')[0].Trim();
                string sEmployeeIDs = oPFM.Params.Split('~')[1].Trim();
                bool bActive = Convert.ToBoolean(oPFM.Params.Split('~')[2]);
                bool bInactive = Convert.ToBoolean(oPFM.Params.Split('~')[3]);
                bool bApprove = Convert.ToBoolean(oPFM.Params.Split('~')[4]);
                bool bUnapprove = Convert.ToBoolean(oPFM.Params.Split('~')[5]);

                string sSQL = "Select * from View_PFmember Where PFMID <>0 ";

                if (sPFSchemeIDs != "")
                {
                    sSQL = sSQL + " And PFSchemeID In (" + sPFSchemeIDs + ")";
                }
                if (sEmployeeIDs != "")
                {
                    sSQL = sSQL + " And EmployeeID In (" + sEmployeeIDs + ")";
                }

                if (bActive && !bInactive)
                {
                    sSQL = sSQL + " And IsActive = 1";
                }
                else if (!bActive && bInactive)
                {
                    sSQL = sSQL + " And IsActive = 0";
                } 
                else if (bActive && bInactive)
                {
                    sSQL = sSQL + " And (IsActive = 1 OR IsActive = 0)";
                }

                if (bApprove && !bUnapprove)
                {
                    sSQL = sSQL + " And ApproveBy >0";
                }
                else if (!bActive && bInactive)
                {
                    sSQL = sSQL + " And ApproveBy <= 0";
                }
                else if (bActive && bInactive)
                {
                    sSQL = sSQL + " And (ApproveBy > 0 OR ApproveBy <= 0)";
                }
                oPFMs = PFmember.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPFMs.Count() <= 0) { throw new Exception("No employee found."); }
            }
            catch (Exception ex)
            {
                oPFMs = new List<PFmember>();
                oPFM = new PFmember();
                oPFM.ErrorMessage = ex.Message;
                oPFMs.Add(oPFM);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetsPFMByNameCode(PFmember oPFM)
        {
            List<PFmember> oPFMs = new List<PFmember>();
            try
            {
                if (oPFM.EmployeeName.Trim() == "") { throw new Exception("Please enter someting to search."); }
                string sSQL = "Select * from View_PFmember Where EmployeeName Like '%" + oPFM.EmployeeName + "%' OR EmployeeCode Like '%" + oPFM.EmployeeName + "%'";
                oPFMs = PFmember.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPFMs.Count() <= 0) { throw new Exception("No employee found."); }
            }
            catch (Exception ex)
            {
                oPFMs = new List<PFmember>();
                oPFM = new PFmember();
                oPFM.ErrorMessage = ex.Message;
                oPFMs.Add(oPFM);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Import & Export
        private List<PFmember> GetPFmemberFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<PFmember> oPFmembers = new List<PFmember>();
            PFmember oPFmember = new PFmember();
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

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        oPFmember = new PFmember();
                        oPFmember.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        if (oPFmember.EmployeeCode != "")
                        {
                            oPFmember.PFSchemeName = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            oPFmember.PFBalance = Convert.ToDouble(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                            oPFmembers.Add(oPFmember);
                        }
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
            return oPFmembers;
        }

        [HttpPost]
        public ActionResult ViewPFMembers(HttpPostedFileBase files)
        {
            List<PFmember> oPFmembers = new List<PFmember>();
            PFmember oPFmember = new PFmember();
            List<PFmember> oPFMs = new List<PFmember>();

            try
            {
                if (files == null) { throw new Exception("File not Found"); }
                oPFmembers = this.GetPFmemberFromExcel(files);
                oPFMs = PFmember.UploadXL(oPFmembers, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPFMs.Count > 0)
                {
                    oPFMs[0].ErrorMessage = "Uploaded Successfully!";
                }
                else
                {
                    oPFMs = new List<PFmember>();
                    PFmember oPFM = new PFmember();
                    oPFM.ErrorMessage = "nothing to upload or these employees alraedy uploaded!";
                    oPFMs.Add(oPFM);
                }
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                //return View(oPFMs);
            }

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            return View(oPFMs);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }


        #endregion Import & Export

        #region PF Ledger XL
        public void PrintPFLedger_All_XL(string sPFSchemaIDs,string  sEmployeeIDs,bool ActivePFM, bool InactivePFM , bool ApprovePFM, bool UnapprovePFM, double ts)
        {
            List<PFmember> oPFMs = new List<PFmember>();
            string sSQL = "SELECT PFMID,PFBalance,EmployeeCode,EmployeeName,DepartmentName,DesignationName"
                        +",(SELECT DateOfJoin FROM EmployeeOfficial WHERE EmployeeID=View_PFmember.EmployeeID) DateOfJoin"
                        +",(SELECT DateOfConfirmation FROM EmployeeOfficial WHERE EmployeeID=View_PFmember.EmployeeID) DateOfConfirmation"
                        + ", ApproveByDate,IsActive"
                        +", (SELECT SUM(Amount) FROM EmployeeSalaryDetail WHERE EmployeeSalaryID IN ("
                        +" SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID=View_PFmember.EmployeeID) AND SalaryHeadID IN ("
                        +" SELECT RecommandedSalaryHeadID FROM PFScheme WHERE PFSchemeID=View_PFmember.PFSchemeID)) EmployeeContribution"
                        +" FROM View_PFmember WHERE EmployeeCode IS NOT NULL";

            if (sPFSchemaIDs != "")
            {
                sSQL = sSQL + " And PFSchemeID In (" + sPFSchemaIDs + ")";
            }
            if (sEmployeeIDs != "")
            {
                sSQL = sSQL + " And EmployeeID In (" + sEmployeeIDs + ")";
            }
            if (ActivePFM && !ActivePFM)
            {
                sSQL = sSQL + " And IsActive = 1";
            }
            else if (!InactivePFM && InactivePFM)
            {
                sSQL = sSQL + " And IsActive = 0";
            }
            else if (ActivePFM && InactivePFM)
            {
                sSQL = sSQL + " And (IsActive = 1 OR IsActive = 0)";
            }
            if (ApprovePFM && !UnapprovePFM)
            {
                sSQL = sSQL + " And ApproveBy >0";
            }
            else if (!ActivePFM && InactivePFM)
            {
                sSQL = sSQL + " And ApproveBy <= 0";
            }
            else if (ActivePFM && InactivePFM)
            {
                sSQL = sSQL + " And (ApproveBy > 0 OR ApproveBy <= 0)";
            }
            sSQL = sSQL + " ORDER BY EmployeeCode";
            oPFMs = PFmember.GetsPFLedgerReport(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PFLEDGER");
                sheet.Name = "PFLEDGER";

                sheet.Column(2).Width = 8;//SL
                sheet.Column(3).Width = 15;//CODE
                sheet.Column(4).Width = 25;//NAME
                sheet.Column(5).Width = 25;//DEPARTMENT
                sheet.Column(6).Width = 25;//DESIGNATION
                sheet.Column(7).Width = 15;//JOINING
                sheet.Column(8).Width = 15;//CONFIRMATION
                sheet.Column(9).Width = 15;//MEMEBER DATE
                sheet.Column(10).Width = 15;//STATUS
                sheet.Column(11).Width = 15;//EmployeeCont
                sheet.Column(12).Width = 15;//EmployerCont
                sheet.Column(13).Width = 15;//Total Profit
                sheet.Column(14).Width = 15;//Total PF

                nMaxColumn = 14;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "PF LEDGER"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "JOINING"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "CONFIRMATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "PF MEMBER DATE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "STATUS"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "EMPLOYEE SUBSCRIP."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "EMPLOYER CONT."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "TOTAL PROFIT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "TOTAL PF BALANCE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                double nEmployeeContribution = 0;
                double nGTotalPF = 0;
                int nSL = 0;
                foreach (PFmember oPFmember in oPFMs)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.EmployeeName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DateOfConfirmationInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.ApproveByDateInStr; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.ActivityStatusInST; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEmployeeContribution += Math.Round(oPFmember.EmployeeContribution);
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(Math.Round(oPFmember.EmployeeContribution)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(Math.Round(oPFmember.EmployeeContribution)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(0.0); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nGTotalPF += oPFmember.TotalPF;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(oPFmember.TotalPF); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 8]; cell.Merge = true;
                cell.Value = "TOTAL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 9];
                cell.Value = Global.MillionFormat(nEmployeeContribution); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 10];
                cell.Value = Global.MillionFormat(nEmployeeContribution); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 11];
                cell.Value = Global.MillionFormat(0.0); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 12];
                cell.Value = Global.MillionFormat(nGTotalPF); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                //signature
                rowIndex = rowIndex + 4;
                int nColSpan1_From = 0;
                int nColSpan2_From = 0;
                int nColSpan3_From = 0;
                int nColSpan1_To = 0;
                int nColSpan2_To = 0;
                int nColSpan3_To = 0;

                if (nMaxColumn % 3 == 0)
                {
                    nColSpan1_From = 1;
                    nColSpan1_To = nColSpan1_From + nMaxColumn / 3;

                    nColSpan2_From = nColSpan1_To + 1;
                    nColSpan2_To = nColSpan2_From + nMaxColumn / 3;

                    nColSpan3_From = nColSpan2_To + 1;
                    nColSpan3_To = nMaxColumn;
                }
                else
                {
                    nColSpan1_From = 1;
                    nColSpan1_To = nColSpan1_From + (nMaxColumn - nMaxColumn % 3) / 3;

                    nColSpan2_From = nColSpan1_To + 1;
                    nColSpan2_To = nColSpan2_From + (nMaxColumn - nMaxColumn % 3) / 3;

                    nColSpan3_From = nColSpan2_To + 1;
                    nColSpan3_To = nMaxColumn;
                }

                cell = sheet.Cells[rowIndex, nColSpan1_From, rowIndex + 3, nColSpan1_To]; cell.Merge = true; cell.Value = "____________________\nPrepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, nColSpan2_From, rowIndex + 3, nColSpan2_To]; cell.Merge = true; cell.Value = "______________________\nChecked By APM/PM"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, nColSpan3_From, rowIndex + 3, nMaxColumn]; cell.Merge = true; cell.Value = "______________________\nApproved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PF_LEDGER.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        public void PrintPFLedgerAdvSearch_All_XL(string sPFSchemaIDs, string sEmployeeIDs, bool ActivePFM, bool InactivePFM, bool ApprovePFM, bool UnapprovePFM, double ts)
        {
            List<PFmember> oPFMs = new List<PFmember>();
            string sSQL = "SELECT PFMID,PFBalance,EmployeeCode,EmployeeName,DepartmentName,DesignationName"
                        + ",(SELECT DateOfJoin FROM EmployeeOfficial WHERE EmployeeID=View_PFmember.EmployeeID) DateOfJoin"
                        + ",(SELECT DateOfConfirmation FROM EmployeeOfficial WHERE EmployeeID=View_PFmember.EmployeeID) DateOfConfirmation"
                        + ", ApproveByDate,IsActive"
                        + ", (SELECT SUM(Amount) FROM EmployeeSalaryDetail WHERE EmployeeSalaryID IN ("
                        + " SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID=View_PFmember.EmployeeID) AND SalaryHeadID IN ("
                        + " SELECT RecommandedSalaryHeadID FROM PFScheme WHERE PFSchemeID=View_PFmember.PFSchemeID)) EmployeeContribution"
                        + " FROM View_PFmember WHERE EmployeeCode IS NOT NULL";

            if (sPFSchemaIDs != "")
            {
                sSQL = sSQL + " And PFSchemeID In (" + sPFSchemaIDs + ")";
            }
            if (sEmployeeIDs != "")
            {
                sSQL = sSQL + " And EmployeeID In (" + sEmployeeIDs + ")";
            }
            if (ActivePFM && !ActivePFM)
            {
                sSQL = sSQL + " And IsActive = 1";
            }
            else if (!InactivePFM && InactivePFM)
            {
                sSQL = sSQL + " And IsActive = 0";
            }
            else if (ActivePFM && InactivePFM)
            {
                sSQL = sSQL + " And (IsActive = 1 OR IsActive = 0)";
            }
            if (ApprovePFM && !UnapprovePFM)
            {
                sSQL = sSQL + " And ApproveBy >0";
            }
            else if (!ActivePFM && InactivePFM)
            {
                sSQL = sSQL + " And ApproveBy <= 0";
            }
            else if (ActivePFM && InactivePFM)
            {
                sSQL = sSQL + " And (ApproveBy > 0 OR ApproveBy <= 0)";
            }
            sSQL = sSQL + " ORDER BY EmployeeCode";
            oPFMs = PFmember.GetsPFLedgerReport(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PFLEDGER");
                sheet.Name = "PFLEDGER";

                sheet.Column(2).Width = 8;//SL
                sheet.Column(3).Width = 15;//CODE
                sheet.Column(4).Width = 25;//NAME
                sheet.Column(5).Width = 25;//DEPARTMENT
                sheet.Column(6).Width = 25;//DESIGNATION
                sheet.Column(7).Width = 15;//JOINING
                sheet.Column(8).Width = 15;//CONFIRMATION
                sheet.Column(9).Width = 15;//MEMEBER DATE
                sheet.Column(10).Width = 15;//STATUS
                sheet.Column(11).Width = 15;//EmployeeCont
                sheet.Column(12).Width = 15;//EmployerCont
                sheet.Column(13).Width = 15;//Total Profit
                sheet.Column(14).Width = 15;//Total PF

                nMaxColumn = 14;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "PF LEDGER"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "JOINING"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "CONFIRMATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "PF MEMBER DATE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "STATUS"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "EMPLOYEE SUBSCRIP."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "EMPLOYER CONT."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "TOTAL PROFIT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "TOTAL PF BALANCE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                double nEmployeeContribution = 0;
                double nGTotalPF = 0;
                int nSL = 0;
                foreach (PFmember oPFmember in oPFMs)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.EmployeeName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.DateOfConfirmationInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.ApproveByDateInStr; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = oPFmember.ActivityStatusInST; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEmployeeContribution += Math.Round(oPFmember.EmployeeContribution);
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(Math.Round(oPFmember.EmployeeContribution)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(Math.Round(oPFmember.EmployeeContribution)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(0.0); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nGTotalPF += oPFmember.TotalPF;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = Global.MillionFormat(oPFmember.TotalPF); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 8]; cell.Merge = true;
                cell.Value = "TOTAL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 9];
                cell.Value = Global.MillionFormat(nEmployeeContribution); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 10];
                cell.Value = Global.MillionFormat(nEmployeeContribution); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 11];
                cell.Value = Global.MillionFormat(0.0); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex + 12];
                cell.Value = Global.MillionFormat(nGTotalPF); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                //signature
                rowIndex = rowIndex + 4;
                int nColSpan1_From = 0;
                int nColSpan2_From = 0;
                int nColSpan3_From = 0;
                int nColSpan1_To = 0;
                int nColSpan2_To = 0;
                int nColSpan3_To = 0;

                if (nMaxColumn % 3 == 0)
                {
                    nColSpan1_From = 1;
                    nColSpan1_To = nColSpan1_From + nMaxColumn / 3;

                    nColSpan2_From = nColSpan1_To + 1;
                    nColSpan2_To = nColSpan2_From + nMaxColumn / 3;

                    nColSpan3_From = nColSpan2_To + 1;
                    nColSpan3_To = nMaxColumn;
                }
                else
                {
                    nColSpan1_From = 1;
                    nColSpan1_To = nColSpan1_From + (nMaxColumn - nMaxColumn % 3) / 3;

                    nColSpan2_From = nColSpan1_To + 1;
                    nColSpan2_To = nColSpan2_From + (nMaxColumn - nMaxColumn % 3) / 3;

                    nColSpan3_From = nColSpan2_To + 1;
                    nColSpan3_To = nMaxColumn;
                }

                cell = sheet.Cells[rowIndex, nColSpan1_From, rowIndex + 3, nColSpan1_To]; cell.Merge = true; cell.Value = "____________________\nPrepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, nColSpan2_From, rowIndex + 3, nColSpan2_To]; cell.Merge = true; cell.Value = "______________________\nChecked By APM/PM"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, nColSpan3_From, rowIndex + 3, nMaxColumn]; cell.Merge = true; cell.Value = "______________________\nApproved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PF_LEDGER.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        #endregion PF Ledger XL

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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

        #region Gets Employee for PF Member
        [HttpPost]
        public JsonResult EmployeeSearch(string sParam)
        {
            int nCount = sParam.Split('~').Length;
            List<Employee> oEmployees = new List<Employee>();
            string sName = Convert.ToString(sParam.Split('~')[0]).TrimStart(' ');
            string sCode = Convert.ToString(sParam.Split('~')[1]).TrimStart(' ');
            string sASID = Convert.ToString(sParam.Split('~')[2]);
            string sLocationID = Convert.ToString(sParam.Split('~')[3]);
            string sdepartmentID = Convert.ToString(sParam.Split('~')[4]);
            string sdesignationID = Convert.ToString(sParam.Split('~')[5]);
            string sGender = Convert.ToString(sParam.Split('~')[6]);
            int nEmployeeType = Convert.ToInt32(sParam.Split('~')[7]);
            int nShift = Convert.ToInt32(sParam.Split('~')[8]);
            int nIsActive = Convert.ToInt32(sParam.Split('~')[9]);
            int nIsUnOfficial = Convert.ToInt32(sParam.Split('~')[10]);
            int nIsInActive = Convert.ToInt32(sParam.Split('~')[11]);
            string sLastEmployeeIDs = sParam.Split('~')[12];
            int nbIsUser = Convert.ToInt32(sParam.Split('~')[13]);
            int nbIsOfficial = Convert.ToInt32(sParam.Split('~')[14]);
            int nCardStatus = Convert.ToInt32(sParam.Split('~')[15]);
            int nCardNotAsigned = Convert.ToInt32(sParam.Split('~')[16]);
            int nWorkingStatus = Convert.ToInt32(sParam.Split('~')[17]);
            int nSSNotAsigned = Convert.ToInt32(sParam.Split('~')[18]);// SS=Salary Structure
            string sStartDate = sParam.Split('~')[19];
            string sEndDate = sParam.Split('~')[20];
            int nDateType = Convert.ToInt32(sParam.Split('~')[21]);
            int nRowLength = Convert.ToInt32(sParam.Split('~')[22]);
            int nLoadRecord = Convert.ToInt32(sParam.Split('~')[23]);
            bool bIsJoiningDate = Convert.ToBoolean(sParam.Split('~')[24]);
            DateTime dtDateFrom = Convert.ToDateTime(sParam.Split('~')[25]);
            DateTime dtDateTo = Convert.ToDateTime(sParam.Split('~')[26]);
            string sEnrollNo = "";
            if (nCount >= 28) { sEnrollNo = Convert.ToString(sParam.Split('~')[27]).TrimStart(' '); }
            bool bIsnotEnroll = false;
            if (nCount >= 29) { bIsnotEnroll = Convert.ToBoolean(sParam.Split('~')[28]); }
            short nCategory = 0;
            if (nCount >= 30) { nCategory = Convert.ToInt16(sParam.Split('~')[29]); }
            int nBusinessUnitID = 0;
            if (nCount >= 31) { nBusinessUnitID = Convert.ToInt16(sParam.Split('~')[30]); }

            string sSQL = "SELECT top(" + nLoadRecord + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Employee WHERE EmployeeID <>0";

            //if (sLastEmployeeIDs == "")
            //{
            //    sLastEmployeeIDs = "0";
            //}
            //string sSQL = "SELECT top(50)* FROM View_Employee WHERE EmployeeID NOT IN (" + sLastEmployeeIDs+")";

            bool bIsOfficial = false;
            string sSQL1 = " AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE EmployeeID<>0";
            if (nIsUnOfficial > 0)
            {
                sSQL = sSQL + " AND EmployeeID NOT IN (SELECT EmployeeID FROM EmployeeOfficial) ";
            }
            if (nbIsOfficial > 0)
            {
                sSQL = sSQL + " AND EmployeeID IN (SELECT EmployeeID FROM EmployeeOfficial) ";
            }
            else
            {
                if (sName != "" && sName != " ")
                {
                    sSQL = sSQL + " AND Name like'" + "%" + sName + "%" + "'";
                }
                if (sCode != " " && sCode != "")
                {
                    sSQL = sSQL + " AND Code like'" + "%" + sCode + "%" + "'";
                }

                if (sEnrollNo != "")
                {
                    sSQL = sSQL + " AND EmployeeID IN (SELECT EmployeeID FROM EmployeeAuthentication WHERE IsActive=1 AND CardNo like'" + "%" + sEnrollNo + "%" + "')";
                }
                if (bIsnotEnroll)
                {
                    sSQL = sSQL + " AND EmployeeID NOT IN(SELECT EmployeeID FROM EmployeeAuthentication WHERE IsActive=1)";
                }
                if (sGender != "None")
                {
                    sSQL = sSQL + " AND Gender='" + sGender + "'";
                }
                if (nIsActive > 0)
                {
                    sSQL = sSQL + " AND IsActive=1";
                }
                if (nIsInActive > 0)
                {
                    sSQL = sSQL + " AND IsActive=0";
                }
                if (nbIsUser > 0)
                {
                    sSQL = sSQL + "AND EmployeeID IN (SELECT EmployeeID FROM Users)";
                }
                if (sASID != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND AttendanceSchemeID=" + sASID;
                }
                if (sLocationID != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND LocationID=" + sLocationID;
                }
                if (sdepartmentID != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND DepartmentID=" + sdepartmentID;
                }
                if (sdesignationID != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND DesignationID=" + sdesignationID;
                }
                if (nEmployeeType > 0)
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND EmployeeTypeID=" + nEmployeeType;
                }

                if (nShift > 0)
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND CurrentShiftID=" + nShift;
                }
                if (bIsOfficial == true)
                {
                    sSQL1 = sSQL1 + ")";
                    sSQL = sSQL + sSQL1;
                }
                if (nCardStatus > 0)
                {
                    sSQL = sSQL + " AND EmployeeCardStatus=" + nCardStatus;

                }

                if (nCardNotAsigned > 0)
                {
                    sSQL = sSQL + " AND (EmployeeCardStatus IS NULL OR EmployeeCardStatus=0) AND WorkingStatus IN(1,2) AND IsActive=1";

                }

                if (nWorkingStatus > 0)
                {
                    sSQL = sSQL + " AND WorkingStatus = " + nWorkingStatus;

                }
                if (nSSNotAsigned > 0)
                {
                    sSQL = sSQL + " AND EmployeeID NOT IN(SELECT EmployeeID FROM View_EmployeeSalaryStructure)";

                }
                //if (sFloor != "None" && sFloor != "")
                //{
                //    sSQL = sSQL + " AND Note LIKE '%" + sFloor.Split(' ')[0].Trim() + "%' AND Note LIKE'%" + sFloor.Split(' ')[1].Trim() + "%'";
                //}
                if (nDateType != 0)
                {
                    if (nDateType == 1)
                    {
                        sSQL = sSQL + " AND MONTH(DateOfBirth) = " + sStartDate.Split('-')[0] + " AND DAY(DateOfBirth)=" + sStartDate.Split('-')[1];
                    }
                    else if (nDateType == 2)
                    {
                        sSQL = sSQL + " AND DateOfBirth BETWEEN  '1900-" + sStartDate + "' AND '" + DateTime.Now.ToString("yyyy") + "-" + sEndDate + "'";
                    }
                }
                if (bIsJoiningDate)
                {
                    sSQL = sSQL + " AND JoiningDate BETWEEN '" + dtDateFrom + "' AND '" + dtDateTo + "'";
                }
                if (nCategory > 0)
                {
                    sSQL = sSQL + " AND EmployeeCategory=" + nCategory;
                }
                if (nBusinessUnitID > 0)
                {
                    sSQL = sSQL + " AND DRPID IN(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID=" + nBusinessUnitID + ")";
                }
                
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }
            sSQL = sSQL + " AND EmployeeID NOT IN (SELECT EmployeeID FROM PFMEMber WHERE IsActive=1 OR ApproveBy>0)";
            sSQL = sSQL + ") aa WHERE Row >" + nRowLength + " Order By Code";
            //sSQL = sSQL + " Order By Code";
            try
            {
                oEmployees = Employee.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                Employee oEmp = new Employee();
                oEmp.ErrorMessage = ex.Message;
                oEmployees.Add(oEmp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByEmpCode(Employee oEmployee)
        {
            string sCode = oEmployee.Params.Split('~')[0];
            int nDepartmentID = Convert.ToInt32(oEmployee.Params.Split('~')[1]);
            List<Employee> oEmployees = new List<Employee>();
            
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE IsActive=1 AND Code LIKE '%" + sCode + "%' OR Name LIKE '%" + sCode + "%'";
                if (nDepartmentID != 0)
                {
                    sSql += " AND DepartmentID =" + nDepartmentID;
                }

                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
                }
                sSql = sSql + " AND EmployeeID NOT IN (SELECT EmployeeID FROM PFMEMber WHERE IsActive=1 OR ApproveBy>0)";
                oEmployees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oEmployees = new List<Employee>();
                oEmployee = new Employee();
                oEmployee.ErrorMessage = ex.Message;
                oEmployees.Add(oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Gets Employee for PF Member

    }
}