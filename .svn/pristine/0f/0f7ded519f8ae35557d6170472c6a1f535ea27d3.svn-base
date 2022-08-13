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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using CrystalDecisions.CrystalReports.Engine;


namespace ESimSolFinancial.Controllers
{
    public class EmployeeController : PdfViewController
    {
        #region Declaration
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        EmployeeGroup _oEmployeeGroup = new EmployeeGroup();
        List<EmployeeGroup> _oEmployeeGroups = new List<EmployeeGroup>();
        record rec = new record();
        recordInactive recInactive = new recordInactive();
        List<record> showSummery = new List<record>();
        List<recordInactive> showSummeryInactive = new List<recordInactive>();
        private string _sSQL = "";
        EmployeeSalaryStructure _oEmployeeSalaryStructure;

        #endregion

        #region Employee

        public ActionResult View_Employees(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployees = new List<Employee>();
            //_oEmployees = Employee.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oEmployees);
        }

        public ActionResult View_DiscontinuedEmployees(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));


            _oEmployees = new List<Employee>();

            return View(_oEmployees);
        }

        public ActionResult View_NewWorkerList(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployees = new List<Employee>();

            return View(_oEmployees);
        }

        public ActionResult View_Employee(int nid, string sMsg) // EmployeeID
        {
            _oEmployee = new Employee();
            List<EmployeeNominee> oEmployeeNominees = new List<EmployeeNominee>();
            if (nid > 0)
            {
                _oEmployee = _oEmployee.Get(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeEducations = EmployeeEducation.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeReferences = EmployeeReference.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeAuthentications = EmployeeAuthentication.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                oEmployeeNominees = EmployeeNominee.Gets(nid, (int)Session[SessionInfo.currentUserID]);
            }
            if (sMsg != "N/A")
            {
                _oEmployee.ErrorMessage = sMsg;
            }
            ViewBag.EmployeeNominees = oEmployeeNominees;
            return PartialView(_oEmployee);
        }

        public ActionResult View_EmployeeIDCard(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployees = new List<Employee>();

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalary).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            return View(_oEmployees);
        }
        #region For Not HR this List ( thouse pepole are not use our HR Module)
        public ActionResult View_Employees_NoHR(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployees = new List<Employee>();
            _oEmployees = Employee.Gets("SELECT top(200)* FROM View_Employee Order By EmployeeID DESC", (int)Session[SessionInfo.currentUserID]);
            return View(_oEmployees);
        }
        public ActionResult View_Employee_NoHR(int id)
        {
            if (id > 0)
            {
                _oEmployee = _oEmployee.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.Locations = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EmployeeTypes = EnumObject.jGets(typeof(EnumEmployeeDesignationType));
            return View(_oEmployee);
        }
        #endregion


        [HttpPost]
        public JsonResult View_Employee(Employee oEmp)
        {
            _oEmployee = new Employee();
            try
            {
                _oEmployee = oEmp;
                if (_oEmployee.EmployeeID <= 0)
                {
                    _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EmployeeIU(HttpPostedFileBase file1, Employee oEmp)
        {
            // Verify that the user selected a file
            string sErrorMessage = "";
            try
            {
                #region Photo Image
                if (file1 != null && file1.ContentLength > 0)
                {
                    System.Drawing.Image oPhotoImage = System.Drawing.Image.FromStream(file1.InputStream, true, true);

                    //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                    //Orginal Image to byte array
                    byte[] aPhotoImageInByteArray = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oPhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        aPhotoImageInByteArray = ms.ToArray();
                    }

                    #region Image Size Validation
                    double nMaxLength = 40 * 1024;
                    if (aPhotoImageInByteArray.Length > nMaxLength)
                    {
                        sErrorMessage = "Youe Photo Image " + aPhotoImageInByteArray.Length + "KB! You can selecte maximum 100KB image";
                        //return RedirectToAction("ViewContactPersonnel", new { cid = oContactPersonnel.ContractorID, id = oContactPersonnel.ContactPersonnelID, ms = sErrorMessage });
                        //return RedirectToAction("View_Employee", new { nid = _oEmployee.EmployeeID, sMsg = _oEmployee.ErrorMessage });
                    }
                    //else
                    //{
                    //    oContactPersonnel.Photo = aPhotoImageInByteArray;
                    //}
                    #endregion
                    oEmp.Photo = aPhotoImageInByteArray;
                }
                #endregion


                if (sErrorMessage == "")
                {
                    _oEmployee = oEmp;
                    if (_oEmployee.EmployeeID <= 0)
                    {
                        _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                    }
                    else
                    {
                        _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                    }
                    //if (_oEmployee.EmployeeID > 0)
                    //{
                    //    _oEmployee.ErrorMessage = "Data saved successfuly !";   
                    //}
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oEmployee);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            return RedirectToAction("View_Employee", new { nid = _oEmployee.EmployeeID, sMsg = _oEmployee.ErrorMessage });
        }

        [HttpPost]
        public JsonResult EmployeeDetailUpdate(Employee oEmp)
        {
            _oEmployee = new Employee();
            try
            {
                _oEmployee = oEmp;
                _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public System.Drawing.Image GetPhoto(int nid)//EmployeeID
        {
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nid, (int)Session[SessionInfo.currentUserID]);
            if (oEmployee.Photo != null)
            {
                MemoryStream m = new MemoryStream(oEmployee.Photo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public System.Drawing.Image GetSignature(int nid)//EmployeeID
        {
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nid, (int)Session[SessionInfo.currentUserID]);
            if (oEmployee.Signature != null)
            {
                MemoryStream m = new MemoryStream(oEmployee.Signature);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DeleteImage(Employee oEmp)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oEmp.DeleteImage((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult EmployeeDelete(int nEmpId, double nts)
        {
            _oEmployee = new Employee();
            try
            {
                _oEmployee.EmployeeID = nEmpId;
                _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployee.ErrorMessage == "")
                {
                    _oEmployee.ErrorMessage = "Delete Successfully.";
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Eduction

        public ActionResult View_EmployeeEducation(int nEmployeeID, int nid) // EmployeeEducationID
        {
            EmployeeEducation oEE = new EmployeeEducation();
            oEE.EmployeeID = nEmployeeID;
            if (nid > 0)
            {
                oEE = oEE.Get(nid, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(oEE);
        }

        [HttpPost]
        public JsonResult EmployeeEducationIU(EmployeeEducation oEE)
        {
            EmployeeEducation oEmpEducation = new EmployeeEducation();
            try
            {
                oEmpEducation = oEE;
                if (oEmpEducation.EmployeeEducationID > 0)
                {
                    oEmpEducation = oEmpEducation.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpEducation = oEmpEducation.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpEducation = new EmployeeEducation();
                oEmpEducation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpEducation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeEducationDelete(EmployeeEducation oEE)
        {
            EmployeeEducation oEmpEducation = new EmployeeEducation();
            try
            {
                oEmpEducation = oEE;
                oEmpEducation = oEmpEducation.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmpEducation = new EmployeeEducation();
                oEmpEducation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpEducation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Experience

        public ActionResult View_EmployeeExperience(int nEmployeeID, int nid) // EmployeeExperienceID
        {
            EmployeeExperience oEExp = new EmployeeExperience();
            oEExp.EmployeeID = nEmployeeID;

            if (nid > 0)
            {
                oEExp = oEExp.Get(nid, (int)Session[SessionInfo.currentUserID]);

            }
            string sSql = "SELECT * FROM Department WHERE DepartmentID<>1";
            oEExp.Departments = Department.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oEExp);
        }

        [HttpPost]
        public JsonResult EmployeeExperienceIU(EmployeeExperience oEExp)
        {
            EmployeeExperience oEmpExp = new EmployeeExperience();
            try
            {
                oEmpExp = oEExp;
                if (oEmpExp.EmployeeExperienceID > 0)
                {
                    oEmpExp = oEmpExp.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpExp = oEmpExp.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpExp = new EmployeeExperience();
                oEmpExp.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpExp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeExperienceDelete(EmployeeExperience oEExp)
        {
            EmployeeExperience oEmpExp = new EmployeeExperience();
            try
            {
                oEmpExp = oEExp;
                oEmpExp = oEmpExp.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmpExp = new EmployeeExperience();
                oEmpExp.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpExp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Training

        public ActionResult View_EmployeeTraining(int nEmployeeID, int nid) // EmployeeTrainingID
        {
            EmployeeTraining oET = new EmployeeTraining();
            oET.EmployeeID = nEmployeeID;
            if (nid > 0)
            {
                oET = oET.Get(nid, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(oET);
        }

        [HttpPost]
        public JsonResult EmployeeTrainingIU(EmployeeTraining oET)
        {
            EmployeeTraining oEmpTr = new EmployeeTraining();
            try
            {
                oEmpTr = oET;
                if (oEmpTr.EmployeeTrainingID > 0)
                {
                    oEmpTr = oEmpTr.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpTr = oEmpTr.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpTr = new EmployeeTraining();
                oEmpTr.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpTr);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeTrainingDelete(EmployeeTraining oET)
        {
            EmployeeTraining oEmpTr = new EmployeeTraining();
            try
            {
                oEmpTr = oET;
                oEmpTr = oEmpTr.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmpTr = new EmployeeTraining();
                oEmpTr.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpTr);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Reference

        public ActionResult View_EmployeeReference(int nEmployeeID, int nid) // EmployeeReferenceID
        {
            EmployeeReference oER = new EmployeeReference();
            oER.EmployeeID = nEmployeeID;
            if (nid > 0)
            {
                oER = oER.Get(nid, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(oER);
        }

        [HttpPost]
        public JsonResult EmployeeReferenceIU(EmployeeReference oER)
        {
            EmployeeReference oEmpRef = new EmployeeReference();
            try
            {
                oEmpRef = oER;
                if (oEmpRef.EmployeeReferenceID > 0)
                {
                    oEmpRef = oEmpRef.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpRef = oEmpRef.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpRef = new EmployeeReference();
                oEmpRef.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpRef);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeReferenceDelete(EmployeeReference oER)
        {
            EmployeeReference oEmpRef = new EmployeeReference();
            try
            {
                oEmpRef = oER;
                oEmpRef = oEmpRef.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmpRef = new EmployeeReference();
                oEmpRef.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpRef);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeeBankAccount

        public ActionResult View_EmployeeBankAccount(int nEmployeeID, int nid) // EmployeeBankAccountID
        {
            EmployeeBankAccount oEBA = new EmployeeBankAccount();
            oEBA.EmployeeID = nEmployeeID;
            oEBA.BankBranchs = BankBranch.Gets((int)Session[SessionInfo.currentUserID]);
            if (nid > 0)
            {
                oEBA = oEBA.Get(nid, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(oEBA);
        }

        [HttpPost]
        public JsonResult EmployeeBankAccountIU(EmployeeBankAccount oEBA)
        {
            EmployeeBankAccount oEmpBA = new EmployeeBankAccount();
            try
            {
                oEmpBA = oEBA;
                EnumBankAccountType nAccounttype = (EnumBankAccountType)Enum.Parse(typeof(EnumBankAccountType), oEBA.ErrorMessage);
                oEmpBA.AccountType = nAccounttype;
                //oEmpBA.AccountType = (EnumBankAccountType)oEBA.AccountTypeInt;
                if (oEmpBA.EmployeeBankACID > 0)
                {
                    oEmpBA = oEmpBA.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpBA = oEmpBA.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpBA = new EmployeeBankAccount();
                oEmpBA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpBA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeBankAccountDelete(EmployeeBankAccount oEBA)
        {
            EmployeeBankAccount oEmpBA = new EmployeeBankAccount();
            try
            {
                oEmpBA = oEBA;
                oEmpBA = oEmpBA.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmpBA = new EmployeeBankAccount();
                oEmpBA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpBA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Authentication

        public ActionResult View_EmployeeAuthentication(int nEmployeeID, int nid) // EmployeeAuthenticationID
        {
            EmployeeAuthentication oEA = new EmployeeAuthentication();
            oEA.EmployeeID = nEmployeeID;
            if (nid > 0)
            {
                oEA = oEA.Get(nid, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(oEA);
        }

        [HttpPost]
        public JsonResult EmployeeAuthenticationIU(EmployeeAuthentication oEA)
        {
            EmployeeAuthentication oEmpA = new EmployeeAuthentication();
            try
            {
                oEmpA = oEA;
                if (oEmpA.EmployeeAuthenticationID > 0)
                {
                    oEmpA = oEmpA.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpA = oEmpA.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpA = new EmployeeAuthentication();
                oEmpA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeAuthenticationDelete(EmployeeAuthentication oEA)
        {
            EmployeeAuthentication oEmpA = new EmployeeAuthentication();
            try
            {
                oEmpA = oEA;
                oEmpA = oEmpA.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmpA = new EmployeeAuthentication();
                oEmpA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Nominee

        [HttpPost]
        public JsonResult EmployeeNomineeIU(EmployeeNominee oEN)
        {
            EmployeeNominee oEmpN = new EmployeeNominee();
            try
            {
                oEmpN = oEN;
                if (oEmpN.ENID > 0)
                {
                    oEmpN = oEmpN.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpN = oEmpN.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpN = new EmployeeNominee();
                oEmpN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeNomineeDelete(EmployeeNominee oEN)
        {
            EmployeeNominee oEmpN = new EmployeeNominee();
            try
            {
                oEmpN = oEN;
                oEmpN = oEmpN.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmpN = new EmployeeNominee();
                oEmpN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeNominee(EmployeeNominee oEmployeeNominee)
        {
            try
            {
                oEmployeeNominee = oEmployeeNominee.Get(oEmployeeNominee.ENID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployeeNominee = new EmployeeNominee();
                oEmployeeNominee.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeNominee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee Commission Material

        [HttpPost]
        public JsonResult ECMIU(EmployeeCommissionMaterial oECM)
        {
            EmployeeCommissionMaterial oEmpCM = new EmployeeCommissionMaterial();
            try
            {
                oEmpCM = oECM;
                if (oEmpCM.ECMID > 0)
                {
                    oEmpCM = oEmpCM.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpCM = oEmpCM.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpCM = new EmployeeCommissionMaterial();
                oEmpCM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpCM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ECM_Delete(EmployeeCommissionMaterial oECM)
        {
            try
            {
                oECM = oECM.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oECM = new EmployeeCommissionMaterial();
                oECM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oECM.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ECM_Activity(EmployeeCommissionMaterial oECM)
        {
            EmployeeCommissionMaterial oEmployeeCommissionMaterial = new EmployeeCommissionMaterial();
            try
            {
                oEmployeeCommissionMaterial = oECM;
                oEmployeeCommissionMaterial = EmployeeCommissionMaterial.Activite(oEmployeeCommissionMaterial.ECMID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployeeCommissionMaterial = new EmployeeCommissionMaterial();
                oEmployeeCommissionMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeCommissionMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ECM_Approve(EmployeeCommissionMaterial oECM)
        {
            try
            {
                if (oECM.ECMID > 0)
                {
                    string sSql = "UPDATE EmployeeCommissionMaterial SET ApproveBy=" + (int)Session[SessionInfo.currentUserID] + ",ApproveByDate='" + DateTime.Now + "' WHERE ECMID=" + oECM.ECMID + ";SELECT * FROM View_EmployeeCommissionMaterial WHERE ECMID=" + oECM.ECMID;
                    oECM = EmployeeCommissionMaterial.Approve(sSql, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oECM = new EmployeeCommissionMaterial();
                oECM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oECM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee Piker

        public ActionResult EmployeeHRMPiker()
        {
            _oEmployee = new Employee();
            _oEmployee.EmployeeTypes = EmployeeType.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(_oEmployee);
        }

        public ActionResult EmployeePikerByCode(string sCode, int nDepartmentID, double nts)
        {
            _oEmployees = new List<Employee>();
            _oEmployee = new Employee();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE IsActive=1 AND Code LIKE '%" + sCode + "%' OR Name LIKE '%" + sCode + "%'";
                if (nDepartmentID != 0)
                {
                    sSql += " AND DepartmentID =" + nDepartmentID;
                }
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            return PartialView(_oEmployees);
        }

        public ActionResult CrewPikerByCode(string sCode, int nDepartmentID, double nts)
        {
            _oEmployees = new List<Employee>();
            _oEmployee = new Employee();
            try
            {
                string sSql = "SELECT * FROM VIEW_Crew WHERE IsActive=1 AND Code LIKE '%" + sCode + "%' OR Name LIKE '%" + sCode + "%'";
                if (nDepartmentID != 0)
                {
                    sSql += " AND DepartmentID =" + nDepartmentID;
                }
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            return PartialView(_oEmployees);
        }
        [HttpPost]
        public JsonResult EmployeeConfirmationSearch(string sParam)
        {
            int nCategory = Convert.ToInt32(sParam.Split('~')[0]);
            DateTime dtStartDate = Convert.ToDateTime(sParam.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(sParam.Split('~')[2]);

            List<Employee> oEmployees = new List<Employee>();
            string sSql = "SELECT *  FROM View_Employee WHERE IsActive=1 AND EmployeeCategory=" + nCategory + " AND ConfirmationDate BETWEEN'" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "'";
            try
            {
                oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
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
            string sPresentAddress = "";
            if (nCount >= 32) { sPresentAddress = sParam.Split('~')[31]; }
            string sPermanentAddress = "";
            if (nCount >= 33) { sPermanentAddress = sParam.Split('~')[32]; }
            string sEmployeeBlock = "";
            if (nCount >= 34) { sEmployeeBlock = sParam.Split('~')[33]; }
            string sEmployeeGroup = "";
            if (nCount >= 35) { sEmployeeGroup = sParam.Split('~')[34]; }

            bool bIsWorkDay = false;
            if (nCount >= 36) { bIsWorkDay = Convert.ToBoolean(sParam.Split('~')[35]); }
            //string sEmployeeBlock = "";
            //if (nCount >= 36) { sEmployeeBlock = sParam.Split('~')[35]; }


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
                if (sEmployeeGroup != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sEmployeeGroup + "))";
                }
                if (sEmployeeBlock != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sEmployeeBlock + "))";
                }

                if (nShift > 0)
                {
                    if (bIsWorkDay)
                    {
                        bIsOfficial = true;
                        sSQL1 = sSQL1 + " AND EmployeeID IN ( SELECT RP.EmployeeID FROM RosterPlanEmployee AS RP WHERE RP.ShiftID = " + nShift + " AND  RP.AttendanceDate BETWEEN '" + dtDateFrom.ToString("dd MMM yyyy") + "' AND '" + dtDateTo.ToString("dd MMM yyyy") + "')";
                    }
                    else
                    {
                        bIsOfficial = true;
                        sSQL1 = sSQL1 + " AND CurrentShiftID=" + nShift;
                    }
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
                    sSQL = sSQL + " AND JoiningDate BETWEEN '" + dtDateFrom.ToString("dd MMM yyyy") + "' AND '" + dtDateTo.ToString("dd MMM yyyy") + "'";
                }
                if (bIsWorkDay)
                {
                    sSQL = sSQL + " AND EmployeeID IN ( SELECT RP.EmployeeID FROM RosterPlanEmployee AS RP WHERE RP.AttendanceDate BETWEEN '" + dtDateFrom.ToString("dd MMM yyyy") + "' AND '" + dtDateTo.ToString("dd MMM yyyy") + "')";
                }
                if (nCategory > 0)
                {
                    sSQL = sSQL + " AND EmployeeCategory=" + nCategory;
                }
                if (nBusinessUnitID > 0)
                {
                    sSQL = sSQL + " AND DRPID IN(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID=" + nBusinessUnitID + ")";
                }
                if (sPresentAddress != " " && sPresentAddress != "")
                {
                    sSQL = sSQL + " AND presentAddress like'" + "%" + sPresentAddress + "%" + "'";
                }
                if (sPermanentAddress != " " && sPermanentAddress != "")
                {
                    sSQL = sSQL + " AND ParmanentAddress like'" + "%" + sPermanentAddress + "%" + "'";
                }

            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
            }
            sSQL = sSQL + ") aa WHERE Row >" + nRowLength + " Order By Code";
            //sSQL = sSQL + " Order By Code";
            try
            {
                oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult EmployeeSearchFromPIMS(string sParam)
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
            string sBUIDs = "";
            if (nCount >= 31) { sBUIDs = sParam.Split('~')[30]; }
            string sPresentAddress = "";
            if (nCount >= 32) { sPresentAddress = sParam.Split('~')[31]; }
            string sPermanentAddress = "";
            if (nCount >= 33) { sPermanentAddress = sParam.Split('~')[32]; }
            string sEmployeeBlock = "";
            if (nCount >= 34) { sEmployeeBlock = sParam.Split('~')[33]; }
            string sEmployeeGroup = "";
            if (nCount >= 35) { sEmployeeGroup = sParam.Split('~')[34]; }


            string sMaritalStatus = "";
            if (nCount >= 36) { sMaritalStatus = sParam.Split('~')[35]; }
            double nStartSalaryRange = 0;
            if (nCount >= 37) { nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[36]); }
            double nEndSalaryRange = 0;
            if (nCount >= 38) { nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[37]); }
            string sSalarySchemeIDs = "";
            if (nCount >= 39) { sSalarySchemeIDs = sParam.Split('~')[38]; }


            //string sEmployeeBlock = "";
            //if (nCount >= 36) { sEmployeeBlock = sParam.Split('~')[35]; }


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
                if (nStartSalaryRange > 0 && nEndSalaryRange > 0)
                {
                    sSQL = sSQL + " AND EmployeeID IN(SELECT EmployeeID From EmployeeSalary WITH (NOLOCK) WHERE GrossAmount BETWEEN " + nStartSalaryRange + " AND " + nEndSalaryRange + ")";
                }
                if (sSalarySchemeIDs != "")
                {
                    sSQL = sSQL + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN(" + sSalarySchemeIDs + "))";
                }
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
                    sSQL1 = sSQL1 + " AND LocationID IN(" + sLocationID + ")";
                }
                if (sdepartmentID != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND DepartmentID IN(" + sdepartmentID + ")";
                }
                if (sdesignationID != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND DesignationID IN(" + sdesignationID + ")";
                }
                if (nEmployeeType > 0)
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND EmployeeTypeID=" + nEmployeeType;
                }
                if (sEmployeeGroup != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sEmployeeGroup + "))";
                }
                if (sEmployeeBlock != "")
                {
                    bIsOfficial = true;
                    sSQL1 = sSQL1 + " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sEmployeeBlock + "))";
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
                    sSQL = sSQL + " AND JoiningDate BETWEEN '" + dtDateFrom.ToString("dd MMM yyyy") + "' AND '" + dtDateTo.ToString("dd MMM yyyy") + "'";
                }
                if (nCategory > 0)
                {
                    sSQL = sSQL + " AND EmployeeCategory=" + nCategory;
                }
                if (!string.IsNullOrEmpty(sBUIDs))
                {
                    sSQL = sSQL + " AND DRPID IN(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy WHERE BusinessUnitID IN(" + sBUIDs + "))";
                }
                if (sPresentAddress != " " && sPresentAddress != "")
                {
                    sSQL = sSQL + " AND presentAddress like'" + "%" + sPresentAddress + "%" + "'";
                }
                if (sPermanentAddress != " " && sPermanentAddress != "")
                {
                    sSQL = sSQL + " AND ParmanentAddress like'" + "%" + sPermanentAddress + "%" + "'";
                }

            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
            }
            sSQL = sSQL + ") aa WHERE Row >" + nRowLength + " Order By Code";
            //sSQL = sSQL + " Order By Code";
            try
            {
                oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsEmployee(Employee oEmployee)
        {
            _oEmployees = new List<Employee>();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE IsActive=1 ";
                if (oEmployee.Name.Trim() != "") { sSql += " AND Code LIKE '%" + oEmployee.Name.Trim() + "%' OR Name LIKE '%" + oEmployee.Name.Trim() + "%'"; }
                if (oEmployee.DepartmentID > 0) { sSql += " AND DepartmentID =" + oEmployee.DepartmentID; }
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsEmployeeGroupAdvSrc(EmployeeType oEmployeeType)
        {
            List<EmployeeType> _oEmployeeTypes = new List<EmployeeType>();
            try
            {
                string sSql = "";
                if (!string.IsNullOrEmpty(oEmployeeType.Name))
                {
                    sSql = "select * from EmployeeType where Name LIKE'%" + oEmployeeType.Name + "%' AND EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker;
                }
                else
                    sSql = "select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker;
                _oEmployeeTypes = EmployeeType.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployeeTypes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                EmployeeType _oEmployeeGroup = new EmployeeType();
                _oEmployeeGroup.ErrorMessage = ex.Message;
                _oEmployeeTypes.Add(_oEmployeeGroup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsEmployeeBlockAdvSrc(EmployeeType oEmployeeType)
        {
            List<EmployeeType> _oEmployeeTypes = new List<EmployeeType>();
            try
            {
                string sSql = "";
                if (!string.IsNullOrEmpty(oEmployeeType.Name))
                {
                    sSql = "select * from EmployeeType where Name LIKE'%" + oEmployeeType.Name + "%' AND EmployeeGrouping=" + (int)EnumEmployeeGrouping.Block;
                }
                else
                    sSql = "select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.Block;
                _oEmployeeTypes = EmployeeType.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployeeTypes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                EmployeeType _oEmployeeGroup = new EmployeeType();
                _oEmployeeGroup.ErrorMessage = ex.Message;
                _oEmployeeTypes.Add(_oEmployeeGroup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Official Info
        public ActionResult View_OfficialInfo(int nEmpId) // EmployeeID
        {
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            string sSql = "";
            sSql = "SELECT * FROM View_EmployeeOfficial WHERE EmployeeID=" + nEmpId;
            oEmployeeOfficial = EmployeeOfficial.Get(sSql, (int)Session[SessionInfo.currentUserID]);
            oEmployeeOfficial.EmployeeID = nEmpId;
            if (oEmployeeOfficial.AttendanceSchemeID > 0)
            {
                AttendanceScheme oAS = new AttendanceScheme();
                oEmployeeOfficial.AttendanceScheme = oAS.Get(oEmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                oEmployeeOfficial.AttendanceScheme.AttendanceSchemeHolidays = AttendanceSchemeHoliday.Gets(oEmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                oEmployeeOfficial.AttendanceScheme.AttendanceSchemeLeaves = AttendanceSchemeLeave.Gets(oEmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                oEmployeeOfficial.AttendanceScheme.RosterPlanDetails = RosterPlanDetail.Gets(oEmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
            }
            //AttendanceScheme attendanceScheme = new AttendanceScheme();
            //var attendanceCalenders = AttendanceCalendar.Gets((int)Session[SessionInfo.currentUserID]);
            //var employeeTypes = EmployeeType.Gets((int)Session[SessionInfo.currentUserID]);
            //sSql = "select * from AttendanceSchemeDayOff";
            //var dayOffs = AttendanceSchemeDayOff.Gets(sSql,(Guid) Session[SessionInfo.wcfSessionID]);
            //var departmentRequirementPolicys = DepartmentRequirementPolicy.Gets((Guid) Session[SessionInfo.wcfSessionID]);
            ////attendanceScheme.DepartmentRequirementPolicys = departmentRequirementPolicys;
            //attendanceScheme.AttendanceSchemeDayOffs = dayOffs;
            //attendanceScheme.EmployeeTypes = employeeTypes;
            //attendanceScheme.AttendanceCalendars = attendanceCalenders;

            //sSql = "select * from View_RosterPlanDetail";
            //attendanceScheme.RosterPlanDetails = RosterPlanDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            //attendanceScheme.RosterPlans = RosterPlan.Gets((int)Session[SessionInfo.currentUserID]);

            //sSql = "select * from View_AttendanceSchemeLeave";
            //attendanceScheme.AttendanceSchemeLeaves = AttendanceSchemeLeave.Gets(sSql,
            //    (Guid) Session[SessionInfo.wcfSessionID]);
            //sSql = "select * from View_AttendanceSchemeHoliDay";
            //attendanceScheme.AttendanceSchemeHolidays = AttendanceSchemeHoliday.Gets(sSql,
            //    (Guid) Session[SessionInfo.wcfSessionID]);
            //oEmployee.Organograms = Organogram.Gets((Guid) Session[SessionInfo.wcfSessionID]);
            //oEmployee.Designations = Designation.Gets((int)Session[SessionInfo.currentUserID]);
            //oEmployee.AttendanceScheme = attendanceScheme;
            return PartialView(oEmployeeOfficial);
        }

        [HttpPost]
        public ActionResult OfficialInfoIU(List<EmployeeOfficial> oEmployeeOfficials)
        {
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            EmployeeOfficial resEmployeeOfficial = new EmployeeOfficial();
            oEmployeeOfficial.EmployeeOfficials = oEmployeeOfficials;
            if (oEmployeeOfficials[0].EmployeeOfficialID <= 0)
            {
                resEmployeeOfficial = oEmployeeOfficial.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                resEmployeeOfficial = oEmployeeOfficial.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(resEmployeeOfficial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EMPOfficialInfoIU(EmployeeOfficial oEmployeeOfficial)
        {
            Employee oEmployee = new Employee();
            EmployeeOfficial oEmpOff = new EmployeeOfficial();
            oEmpOff.EmployeeOfficials.Add(oEmployeeOfficial);

            try
            {
                if (oEmpOff.EmployeeOfficials[0].EmployeeOfficialID <= 0)
                {
                    oEmpOff = oEmpOff.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oEmpOff = oEmpOff.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }

                string sSQL = "SELECT * FROM VIEW_Employee WHERE EmployeeID=" + oEmployeeOfficial.EmployeeID;
                oEmployee = Employee.Get(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oEmpOff.ErrorMessage != "")
                {
                    throw new Exception(oEmpOff.ErrorMessage);
                }
            }
            catch (Exception e)
            {
                //oEmployee = new Employee();
                oEmployee.ErrorMessage = e.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employees For Recruiting Agency
        public ActionResult View_EmployeeForRecruitingAgencys()
        {
            _oEmployees = new List<Employee>();
            //_oEmployees = Employee.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(_oEmployees);
        }

        public ActionResult View_EmployeeForRecruitingAgency(int nid, string sMsg) // EmployeeID
        {
            _oEmployee = new Employee();
            if (nid > 0)
            {
                _oEmployee = _oEmployee.Get(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeEducations = EmployeeEducation.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeReferences = EmployeeReference.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(nid, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeAuthentications = EmployeeAuthentication.Gets(nid, (int)Session[SessionInfo.currentUserID]);

                string sEmployeeRecommendedDesignationSQL = "SELECT  * FROM View_EmployeeRecommendedDesignation WHERE EmployeeID =" + nid;
                _oEmployee.EmployeeRecommendedDesignations = EmployeeRecommendedDesignation.Gets(sEmployeeRecommendedDesignationSQL, (int)Session[SessionInfo.currentUserID]);
            }
            if (sMsg != "N/A")
            {
                _oEmployee.ErrorMessage = sMsg;
            }
            return PartialView(_oEmployee);
        }

        [HttpPost]
        public ActionResult EmployeeIUForRecruitingAgency(HttpPostedFileBase file1, Employee oEmp)
        {
            // Verify that the user selected a file
            string sErrorMessage = "";
            try
            {
                #region Photo Image
                if (file1 != null && file1.ContentLength > 0)
                {
                    System.Drawing.Image oPhotoImage = System.Drawing.Image.FromStream(file1.InputStream, true, true);

                    //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                    //Orginal Image to byte array
                    byte[] aPhotoImageInByteArray = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oPhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        aPhotoImageInByteArray = ms.ToArray();
                    }

                    #region Image Size Validation
                    double nMaxLength = 40 * 1024;
                    if (aPhotoImageInByteArray.Length > nMaxLength)
                    {
                        sErrorMessage = "Youe Photo Image " + aPhotoImageInByteArray.Length + "KB! You can selecte maximum 100KB image";
                        //return RedirectToAction("ViewContactPersonnel", new { cid = oContactPersonnel.ContractorID, id = oContactPersonnel.ContactPersonnelID, ms = sErrorMessage });
                        //return RedirectToAction("View_Employee", new { nid = _oEmployee.EmployeeID, sMsg = _oEmployee.ErrorMessage });
                    }
                    //else
                    //{
                    //    oContactPersonnel.Photo = aPhotoImageInByteArray;
                    //}
                    #endregion
                    oEmp.Photo = aPhotoImageInByteArray;
                }
                #endregion

                if (sErrorMessage == "")
                {
                    _oEmployee = oEmp;
                    _oEmployee.IsOwnEmployee = false;
                    if (_oEmployee.EmployeeID <= 0)
                    {
                        _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                    }
                    else
                    {
                        _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                    }
                    if (_oEmployee.EmployeeID > 0)
                    {
                        _oEmployee.ErrorMessage = "Data saved successfuly !";
                    }
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oEmployee);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            return RedirectToAction("View_EmployeeForRecruitingAgency", new { nid = _oEmployee.EmployeeID, sMsg = _oEmployee.ErrorMessage });
        }



        #region EmployeeRecommendedDesignation

        public ActionResult View_EmployeeRecommendedDesignation(int nid) // ARDID
        {
            EmployeeRecommendedDesignation oEmployeeRecommendedDesignation = new EmployeeRecommendedDesignation();

            if (nid > 0)
            {

                oEmployeeRecommendedDesignation = EmployeeRecommendedDesignation.Get(nid, (int)Session[SessionInfo.currentUserID]);

            }
            string sSQL = "";
            sSQL = "SELECT * FROM Department WHERE DepartmentID<>1";
            oEmployeeRecommendedDesignation.Departments = Department.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT * FROM Designation WHERE DesignationID<>1";
            oEmployeeRecommendedDesignation.Designations = Designation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oEmployeeRecommendedDesignation);
        }

        [HttpPost]
        public ActionResult EmployeeRecommendedDesignationIU(EmployeeRecommendedDesignation oEmployeeRecommendedDesignation)
        {
            try
            {
                if (oEmployeeRecommendedDesignation.ARDID <= 0)
                {
                    oEmployeeRecommendedDesignation = oEmployeeRecommendedDesignation.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oEmployeeRecommendedDesignation = oEmployeeRecommendedDesignation.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oEmployeeRecommendedDesignation = new EmployeeRecommendedDesignation();
                oEmployeeRecommendedDesignation.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeRecommendedDesignation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EmployeeRecommendedDesignation_Delete(int nARDID)
        {
            EmployeeRecommendedDesignation oEmployeeRecommendedDesignation = new EmployeeRecommendedDesignation();
            try
            {
                oEmployeeRecommendedDesignation.ARDID = nARDID;
                oEmployeeRecommendedDesignation = oEmployeeRecommendedDesignation.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
                if (oEmployeeRecommendedDesignation.ErrorMessage == "")
                {
                    oEmployeeRecommendedDesignation.ErrorMessage = "Delete Successfully";
                }
            }
            catch (Exception ex)
            {
                oEmployeeRecommendedDesignation = new EmployeeRecommendedDesignation();
                oEmployeeRecommendedDesignation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeRecommendedDesignation.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult View_CrewAdvancedSearch()
        {
            _oEmployee = new Employee();

            return PartialView(_oEmployee);
        }

        [HttpPost]
        public JsonResult CrewSearch(string sParam)
        {
            List<Employee> oEmployees = new List<Employee>();

            int nRecommendDesigID = Convert.ToInt32(sParam.Split('~')[0]);
            int nExperienceDesigID = Convert.ToInt32(sParam.Split('~')[1]);
            int nVesselType = Convert.ToInt32(sParam.Split('~')[2]);
            int nCurrentEmploymentStatus = Convert.ToInt32(sParam.Split('~')[3]);
            bool bAvailableWithin = Convert.ToBoolean(sParam.Split('~')[4]);
            DateTime dDateFrom = Convert.ToDateTime(sParam.Split('~')[5]);
            DateTime dDateTo = Convert.ToDateTime(sParam.Split('~')[6]);
            int nRowLength = Convert.ToInt32(sParam.Split('~')[7]);
            int nLoadRecord = Convert.ToInt32(sParam.Split('~')[8]);
            string sName = sParam.Split('~')[9];
            string sCode = sParam.Split('~')[10];

            string sSQL = "SELECT top(" + nLoadRecord + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Crew WHERE EmployeeID<>0 ";

            if (sName != "")
            {
                sSQL = sSQL + " AND Name LIKE'%" + sName + "%'";
            }
            if (sCode != "")
            {
                sSQL = sSQL + " AND Code LIKE'%" + sCode + "%'";
            }
            if (nRecommendDesigID > 0)
            {
                sSQL = sSQL + " AND EmployeeID IN (SELECT EmployeeID FROM EmployeeRecommendedDesignation WHERE DesignationID=" + nRecommendDesigID + ") ";
            }
            if (nExperienceDesigID > 0)
            {
                sSQL = sSQL + " AND EmployeeID IN (SELECT EmployeeID FROM EmployeeExperience WHERE DesignationID=" + nExperienceDesigID + ")";
            }
            if (nVesselType > 0)
            {
                sSQL = sSQL + " AND EmployeeID IN (SELECT EmployeeID FROM EmployeeExperience WHERE VesselID IN (SELECT VesselID FROM Vessel WHERE VesselType=" + nVesselType + "))";
            }
            if (nCurrentEmploymentStatus > 0)
            {
                sSQL = sSQL + " AND CurrentEmploymentStatus=" + nCurrentEmploymentStatus;
            }
            if (bAvailableWithin == true)
            {
                sSQL = sSQL + " AND EmployeeID IN (SELECT distinct EmployeeID FROM ContractAgreement WHERE IsActive=1 AND EndDate BETWEEN '" + dDateFrom + "' AND '" + dDateTo + "')";
            }

            sSQL = sSQL + ") aa WHERE Row >" + nRowLength;

            try
            {
                oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployees = new List<Employee>();
                _oEmployees.Add(_oEmployee);
                _oEmployees[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Transfer Shift

        public ActionResult View_TransferShift(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //List<EmployeeOfficial> oEmployeeOfficials = new List<EmployeeOfficial>();
            //EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            //string sSql = "";
            //sSql = "Select * from View_EmployeeOfficial";
            //oEmployeeOfficials = EmployeeOfficial.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            //oEmployeeOfficial.EmployeeOfficials = oEmployeeOfficials;
            _oEmployee = new Employee();
            string sSql_R = "SELECT * FROM RosterPlan WHERE IsActive=1 AND RosterCycle>0  AND RosterPlanID IN(SELECT RosterPlanID  FROM RosterPlanDetail WHERE NextShiftID>0)";
            _oEmployee.RosterPlans = RosterPlan.Gets(sSql_R, (int)Session[SessionInfo.currentUserID]);
            string sSql_RD = "SELECT * FROM View_RosterPlanDetail WHERE  NextShiftID>0 AND RosterPlanID IN (SELECT RosterPlanID FROM RosterPlan WHERE IsActive=1 AND RosterCycle>0)";
            _oEmployee.RosterPlanDetails = RosterPlanDetail.Gets(sSql_RD, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oEmployee);
        }

        [HttpPost]
        public ActionResult TransferShift(List<Employee> oEmployeeOfficials, int nCurrentShiftID, DateTime dDate)
        {
            List<Employee> oEMPS = new List<Employee>();
            try
            {
                string sEmployeeIDs = "";
                foreach (var oEmployeeOfficial in oEmployeeOfficials)
                {
                    sEmployeeIDs = sEmployeeIDs + oEmployeeOfficial.EmployeeID.ToString() + ",";
                }
                if (sEmployeeIDs.Length > 0)
                {
                    sEmployeeIDs = sEmployeeIDs.Remove(sEmployeeIDs.Length - 1, 1);
                }
                oEMPS = Employee.TransferShift(sEmployeeIDs, nCurrentShiftID, dDate, (int)Session[SessionInfo.currentUserID]);
                if (oEMPS.Count <= 0)
                {
                    throw new Exception("Transfer is not possible!!");
                }
            }
            catch (Exception ex)
            {
                oEMPS = new List<Employee>();
                Employee oEMP = new Employee();
                oEMP.ErrorMessage = ex.Message;
                oEMPS.Add(oEMP);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEMPS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SwapShift(int nRosterPlanID, DateTime dDate)
        {
            Employee oEMp = new Employee();
            try
            {
                oEMp = Employee.SwapShift(nRosterPlanID, dDate, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEMp = new Employee();
                oEMp.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEMp.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Activity
        //[HttpPost]
        //public JsonResult Employee_Activity(Employee oEmployee)
        //{
        //    _oEmployee = new Employee();
        //    try
        //    {
        //        _oEmployee = oEmployee;
        //        _oEmployee = Employee.Activite(_oEmployee.EmployeeID, _oEmployee.IsActive, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oEmployee = new Employee();
        //        _oEmployee.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oEmployee);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult Employee_Activity(string sEmpIDs, double nts)
        {
            _oEmployees = new List<Employee>();
            try
            {
                _oEmployees = Employee.Activite(sEmpIDs, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get


        [HttpGet]
        public JsonResult GetByEmployeeType(int nEmpDesType, double nts)
        {
            Employee oEmployee = new Employee();
            _oEmployees = new List<Employee>();
            try
            {
                oEmployee.EmployeeDesignationType = (EnumEmployeeDesignationType)nEmpDesType;
                int nLocationID = ((User)Session[SessionInfo.CurrentUser]).LocationID;
                _oEmployees = Employee.Gets(oEmployee.EmployeeDesignationType, nLocationID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetByEmployeeID(int nEmpID)
        {
            Employee oEmployee = new Employee();
            try
            {
                oEmployee = oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetByEmployeeNameCode(string sNameCode, bool bIsCode, double nts)
        {
            Employee oEmployee = new Employee();
            _oEmployees = new List<Employee>();

            try
            {
                string sSQL = "";
                if (bIsCode)
                {
                    sSQL = "SELECT * FROM View_Employee WHERE Code LIKE '%" + sNameCode + "%'";
                }
                else
                {
                    sSQL = "SELECT * FROM View_Employee WHERE Name LIKE '%" + sNameCode + "%'";
                }
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSQL = sSQL + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
                }
                _oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEmployeeSalary(Employee oEmployee)
        {
            List<EmployeeSalary> oEmployeeSalarys = new List<EmployeeSalary>();
            _oEmployees = new List<Employee>();
            try
            {
                string sSQL = "SELECT * FROM EmployeeSalary WHERE EmployeeID =" + oEmployee.EmployeeID;
                oEmployeeSalarys = EmployeeSalary.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSalarys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByEmployeeName(Employee oEmployee)
        {
            _oEmployees = new List<Employee>();
            try
            {
                string sSQL = "SELECT * FROM View_Employee WHERE Name LIKE '%" + oEmployee.Name + "%'";
                _oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeeWorkingStatusChange
        [HttpPost]
        public JsonResult EmployeeWorkingStatusChange(string sEmpIDs, double nts)
        {
            _oEmployees = new List<Employee>();
            //string x = string.Join(",", nEmpIds.Select(o => o.ToString()));
            //int n = x.Split(',').Length;
            //List<int> oEmpIDs = new List<int>();
            //oEmpIDs = nEmpIds.ToList();
            try
            {

                _oEmployees = Employee.EmployeeWorkingStatusChange(sEmpIDs, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ContinuedEmployee(string sEmpIDs, double nts)
        {
            _oEmployees = new List<Employee>();
            try
            {

                _oEmployees = Employee.ContinuedEmployee(sEmpIDs, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee Basic Information

        public ActionResult View_EmployeeBasicInformations(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployees = new List<Employee>();
            //_oEmployees = Employee.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oEmployees);
        }

        public ActionResult View_EmployeeBasicInformation(int nId, double ts)
        {
            _oEmployee = new Employee();
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            AttendanceScheme oAttendanceScheme = new AttendanceScheme();
            DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            string sSql = "SELECT * FROM View_EmployeeOfficial WHERE EmployeeID=" + nId;
            if (nId > 0)
            {
                _oEmployee = _oEmployee.Get(nId, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeOfficial = EmployeeOfficial.Get(sSql, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.AttendanceScheme = oAttendanceScheme.Get(_oEmployee.EmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.DRP = oDepartmentRequirementPolicy.Get(_oEmployee.EmployeeOfficial.DRPID, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeSalaryStructure = ESS(nId);
                _oEmployee.EmployeeAuthentications = EmployeeAuthentication.Gets(nId, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(_oEmployee);
        }

        #region IUD
        [HttpPost]
        public JsonResult EmployeeBasicInformation_IU(Employee oEmployee)
        {
            _oEmployee = new Employee();
            try
            {
                _oEmployee = oEmployee;
                _oEmployee.WorkingStatus = (int)(EnumEmployeeWorkigStatus).1;
                _oEmployee.EmployeeDesignationType = (EnumEmployeeDesignationType)oEmployee.EmployeeDesignationTypeInt;

                if (_oEmployee.EmployeeID > 0)
                {
                    _oEmployee = _oEmployee.EmployeeBasicInformation_IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oEmployee = _oEmployee.EmployeeBasicInformation_IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                if (_oEmployee.ErrorMessage == "")
                {
                    string sSQL = "SELECT * FROM VIEW_Employee WHERE EmployeeID=" + _oEmployee.EmployeeID;
                    _oEmployee = Employee.Get(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else { throw new Exception(_oEmployee.ErrorMessage); }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmployeeBasicInformation_Delete(int nId, double ts)//nId=EmployeeID
        {
            _oEmployee = new Employee();
            try
            {

                _oEmployee.EmployeeID = nId;
                _oEmployee = _oEmployee.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion IUD


        #endregion Employee Basic Information

        #region Employee Card

        public ActionResult View_EmployeeCards(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<EmployeeCard> oEmployeeCards = new List<EmployeeCard>();
            return View(oEmployeeCards);
        }

        public ActionResult View_EmployeeCardHistorys(int nEmployeeID, double ts)
        {

            List<EmployeeCardHistory> oEmployeeCardHistorys = new List<EmployeeCardHistory>();
            string sSql = "SELECT * FROM View_EmployeeCardHistory WHERE EmployeeCardID = (SELECT EmployeeCardID FROM EmployeeCard WHERE EmployeeID=" + nEmployeeID + ") ORDER BY EmployeeCardHistoryID";
            oEmployeeCardHistorys = EmployeeCardHistory.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oEmployeeCardHistorys);
        }

        [HttpPost]
        public JsonResult EmployeeCardIssue(string sEmployeeIDs, int EmployeeCardStatusInt, double ts)
        {

            EmployeeCard oEmployeeCard = new EmployeeCard();
            oEmployeeCard.ErrorMessage = sEmployeeIDs;
            oEmployeeCard.EmployeeCardStatusInt = EmployeeCardStatusInt;
            oEmployeeCard.EmployeeCardStatus = (EnumEmployeeCardStatus)oEmployeeCard.EmployeeCardStatusInt;

            try
            {
                oEmployeeCard = oEmployeeCard.IUD((int)Session[SessionInfo.currentUserID]);
                if (oEmployeeCard.ErrorMessage == "")
                {
                    string sSql = "select * from View_Employee WHERE EmployeeID IN(" + sEmployeeIDs + ")";
                    oEmployeeCard.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                oEmployeeCard = new EmployeeCard();
                oEmployeeCard.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeCard);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public Image GetEmployeePhoto(Employee oEmployee)
        //{
        //    if (oEmployee.Photo != null)
        //    {
        //        MemoryStream m = new MemoryStream(oEmployee.Photo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(Response.OutputStream, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        public Image GetImage(byte[] Image)
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Image.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;

            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult GetEmployeeWithImage(string sIDs, double ts)
        {

            _oEmployees = new List<Employee>();

            try
            {
                string sSql = "select * from View_Employee WHERE EmployeeID IN(" + sIDs + ")";
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                //foreach (Employee oItem in _oEmployees)
                //{
                //    oItem.EmployeePhoto = GetEmployeePhoto(oItem);
                //    if (oItem.EmployeePhoto != null)
                //    {
                //        oItem.EmployeePhotoItext = iTextSharp.text.Image.GetInstance(oItem.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                //        oItem.EmployeePhotoItext.ScaleAbsolute(30f, 25f);
                //    }
                //}

            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion Employee Card

        #region Search

        [HttpPost]
        public JsonResult GetByEmpID(int nEmpID)
        {

            _oEmployee = new Employee();
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            AttendanceScheme oAttendanceScheme = new AttendanceScheme();
            DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            string sSql = "SELECT * FROM View_EmployeeOfficial WHERE EmployeeID=" + nEmpID;

            try
            {
                if (nEmpID > 0)
                {
                    _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeOfficial = EmployeeOfficial.Get(sSql, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.AttendanceScheme = oAttendanceScheme.Get(_oEmployee.EmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.DRP = oDepartmentRequirementPolicy.Get(_oEmployee.EmployeeOfficial.DRPID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeSalaryStructure = ESS(nEmpID);
                    _oEmployee.EmployeeAuthentications = EmployeeAuthentication.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);

                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByEmpCode(Employee oEmployee)
        {

            string sCode = oEmployee.Params.Split('~')[0];
            int nDepartmentID = Convert.ToInt32(oEmployee.Params.Split('~')[1]);
            _oEmployees = new List<Employee>();
            _oEmployee = new Employee();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE (Code LIKE '%" + sCode + "%' OR Name LIKE '%" + sCode + "%')";
                if (nDepartmentID != 0)
                {
                    sSql += " AND DepartmentID =" + nDepartmentID;
                }

                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
                }

                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByEmpCodeForTimeCard(Employee oEmployee)
        {

            string sCode = oEmployee.Params.Split('~')[0];
            int nDepartmentID = Convert.ToInt32(oEmployee.Params.Split('~')[1]);
            _oEmployees = new List<Employee>();
            _oEmployee = new Employee();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE (Code LIKE '%" + sCode + "%' OR Name LIKE '%" + sCode + "%')";
                if (nDepartmentID != 0)
                {
                    sSql += " AND DepartmentID =" + nDepartmentID;
                }

                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
                }

                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeByNameCodeDesgDeptLocBUwithDRP(Employee objEmployee)               //24-09-2019 added by akram
        {

            List<Employee> oEmployees = new List<Employee>();
            Employee oEmployee = new Employee();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE IsActive = 1 ";
                bool isNameCode = false;

                #region Name & Code Search
                if (!string.IsNullOrEmpty(objEmployee.Name) && !string.IsNullOrEmpty(objEmployee.Code))
                {
                    isNameCode = true;
                }

                if (isNameCode)
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " Name LIKE " + "'%" + objEmployee.Name + "%' OR Code LIKE " + "'%" + objEmployee.Code + "%'";
                }
                else
                {
                    if (!string.IsNullOrEmpty(objEmployee.Name))
                    {
                        Global.TagSQL(ref sSql);
                        sSql = sSql + " Name LIKE " + "'%" + objEmployee.Name + "%'";
                    }
                    if (!string.IsNullOrEmpty(objEmployee.Code))
                    {
                        Global.TagSQL(ref sSql);
                        sSql = sSql + " Code LIKE " + "'%" + objEmployee.Code + "%'";
                    }
                }

                #endregion
                #region BU
                if (!string.IsNullOrEmpty(objEmployee.BusinessUnitName))
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " BusinessUnitID IN " + "(" + objEmployee.BusinessUnitName + ")";
                }
                #endregion
                #region Location
                if (!string.IsNullOrEmpty(objEmployee.LocationName))
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " LocationID IN " + "(" + objEmployee.LocationName + ")";
                }
                #endregion
                #region Department
                if (!string.IsNullOrEmpty(objEmployee.DepartmentName))
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " DepartmentID IN " + "(" + objEmployee.DepartmentName + ")";
                }
                #endregion
                #region Designation
                if (!string.IsNullOrEmpty(objEmployee.DesignationName))
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " DesignationID IN " + "(" + objEmployee.DesignationName + ")";
                }
                #endregion
                #region DRP Permission
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
                }
                #endregion
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            var jSonResult = Json(_oEmployees, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpPost]
        public JsonResult GetEmployeeAuthentication(EmployeeAuthentication oEmployeeAuthentication)
        {
            List<EmployeeAuthentication> oEmployeeAuthentications = new List<EmployeeAuthentication>();
            try
            {
                oEmployeeAuthentication = oEmployeeAuthentication.Get(oEmployeeAuthentication.EmployeeAuthenticationID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployeeAuthentication = new EmployeeAuthentication();
                oEmployeeAuthentication.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeAuthentication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchDiscontinuedEmployee(string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime sDFrom, DateTime sDTo, int nLoadRecords, int nRowLength)
        {
            //sDTo = sDTo.AddDays(1);
            string sDateFrom = sDFrom.ToString("dd MMM yyyy");
            string sDateTo = sDTo.ToString("dd MMM yyyy");

            _oEmployees = new List<Employee>();
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeID) Row,* FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + sDateFrom + "' AND '" + sDateTo + "'";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }


            sSql += " OR EmployeeID IN(SELECT EmployeeID from EmployeeWorkingStatusHistory WHERE CurrentStatus = 6 AND CONVERT(DATE,DBServerDateTime) BETWEEN '" + sDateFrom + "' AND '" + sDateTo + "'))";

            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + "))";
            }

            sSql = sSql + ") aa WHERE Row >" + nRowLength;
            //string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeWorkingStatusHistory WHERE CurrentStatus = 6 AND DBServerDateTime > '"+sDFrom+"' AND DbServerDateTime< '"+sDTo+"')";
            try
            {
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchNewEmployee(string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime sDFrom, DateTime sDTo, int nLoadRecords, int nRowLength)
        {
            string sDateFrom = sDFrom.ToString("dd MMM yyyy");
            string sDateTo = sDTo.ToString("dd MMM yyyy");

            _oEmployees = new List<Employee>();
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeID) Row,* FROM View_Employee WHERE WorkingStatus = 1 AND JoiningDate BETWEEN '" + sDateFrom + "' AND '" + sDateTo + "'";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
            }
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }
            sSql = sSql + ") aa WHERE Row >" + nRowLength;

            try
            {
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeForLoan(Employee oEmployee)
        {

            string sCode = oEmployee.Params.Split('~')[0];
            int nDepartmentID = Convert.ToInt32(oEmployee.Params.Split('~')[1]);
            _oEmployees = new List<Employee>();
            _oEmployee = new Employee();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE IsActive=1 AND (Code LIKE '%" + sCode + "%' OR Name LIKE '%" + sCode + "%')";

                sSql += " And EmployeeID In (Select EmployeeID from EmployeeOfficial Where IsActive=1 And EmployeeID In (Select EmployeeID from EmployeeSalaryStructure Where IsActive=1))";
                if (nDepartmentID != 0)
                {
                    sSql += " AND DepartmentID =" + nDepartmentID;
                }
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployees = new List<Employee>();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee Salary Structure Calculation
        public EmployeeSalaryStructure ESS(int nId)
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            if (nId > 0)
            {
                string sESSsql = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID =" + nId;
                _oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(sESSsql, (int)Session[SessionInfo.currentUserID]);
                string Ssql = "SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID IN (SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nId + ")";
                _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(Ssql, (int)Session[SessionInfo.currentUserID]);


                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();

                string sSql = "SELECT * FROM  View_SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + " ORDER BY SalarySchemeDetailID ";
                string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + ") ORDER BY SalarySchemeDetailID ";

                oSalarySchemeDetails = SalarySchemeDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, (int)Session[SessionInfo.currentUserID]);
                oSalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalarySchemeDetails, oSalarySchemeDetailCalculations);

                foreach (SalarySchemeDetail oSSDItem in oSalarySchemeDetails)
                {
                    foreach (EmployeeSalaryStructureDetail oESSDItem in _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails)
                    {

                        if (oSSDItem.SalaryHeadID == oESSDItem.SalaryHeadID)
                        {
                            oESSDItem.Calculation = oSSDItem.Calculation;
                        }
                    }

                }

            }
            return _oEmployeeSalaryStructure;

        }

        #endregion

        #region EmployeeOfficialGetForPTI
        [HttpPost]
        public JsonResult GetEmployeeOfficialInfo(EmployeeOfficial oEmployeeOfficial)
        {
            try
            {

                string sSql = "";
                sSql = "SELECT * FROM View_EmployeeOfficial WHERE EmployeeID=" + oEmployeeOfficial.EmployeeID;
                oEmployeeOfficial = new EmployeeOfficial();
                oEmployeeOfficial = EmployeeOfficial.Get(sSql, (int)Session[SessionInfo.currentUserID]);
                if (oEmployeeOfficial.EmployeeID == 0)
                {
                    throw new Exception("This Employee Has No Official Information !");
                }
            }
            catch (Exception ex)
            {
                oEmployeeOfficial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeOfficial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion EmployeeOfficialGetForPTI

        #region Employee Self Service

        public ActionResult View_AttendanceRecords()
        {
            _oEmployees = new List<Employee>();
            return PartialView(_oEmployees);
        }

        public ActionResult View_SalaryRecords()
        {
            _oEmployees = new List<Employee>();
            return PartialView(_oEmployees);
        }

        public ActionResult View_Leaves()
        {
            _oEmployees = new List<Employee>();
            return PartialView(_oEmployees);
        }

        public ActionResult View_OrganizationalRulesSchemeFacilities()
        {
            _oEmployees = new List<Employee>();
            return PartialView(_oEmployees);
        }

        public ActionResult View_SelfInformations()
        {
            _oEmployee = new Employee();
            _oEmployee = GetSelfInformation();
            return PartialView(_oEmployee);
        }

        public ActionResult PrintSelfInformation()
        {
            _oEmployee = new Employee();
            _oEmployee = GetSelfInformation();

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            rptEmployeeSelfInformation oReport = new rptEmployeeSelfInformation();
            byte[] abytes = oReport.PrepareReport(_oEmployee);
            return File(abytes, "application/pdf");
        }

        public Employee GetSelfInformation()
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(((User)Session[SessionInfo.CurrentUser]).UserID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeEducations = EmployeeEducation.Gets(((User)Session[SessionInfo.CurrentUser]).UserID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(((User)Session[SessionInfo.CurrentUser]).UserID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(((User)Session[SessionInfo.CurrentUser]).UserID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeReferences = EmployeeReference.Gets(((User)Session[SessionInfo.CurrentUser]).UserID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(((User)Session[SessionInfo.CurrentUser]).UserID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeAuthentications = EmployeeAuthentication.Gets(((User)Session[SessionInfo.CurrentUser]).UserID, (int)Session[SessionInfo.currentUserID]);
            return _oEmployee;
        }

        #endregion

        #region Employee Code Generator
        [HttpPost]
        public JsonResult GetGeneratedEmpCode(Employee oEmp)
        {
            Employee oEmployee = new Employee();
            try
            {
                oEmployee.Code = Employee.GetGeneratedEmpCode(oEmp.DRPID, oEmp.DesignationID, oEmp.DateOfJoin, oEmp.CompanyID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployee = new Employee();
                oEmployee.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Employee Image
        [HttpPost]
        public ActionResult EmpImageIU(HttpPostedFileBase file1, Employee oEmp)
        {
            // Verify that the user selected a file

            try
            {
                if (oEmp.EmployeeID <= 0)
                {
                    throw new Exception("Please save employee information first !");
                }
                #region Photo Image
                if (file1 != null && file1.ContentLength > 0)
                {
                    System.Drawing.Image oPhotoImage = System.Drawing.Image.FromStream(file1.InputStream, true, true);

                    //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                    //Orginal Image to byte array
                    byte[] aPhotoImageInByteArray = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oPhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        aPhotoImageInByteArray = ms.ToArray();
                    }

                    #region Image Size Validation
                    //double nMaxLength = 100 * 1024;
                    //if (aPhotoImageInByteArray.Length > nMaxLength)
                    //{
                    //    sErrorMessage = "Youe Photo Image " + aPhotoImageInByteArray.Length + "KB! You can selecte maximum 100KB image";
                    //    return RedirectToAction("ViewContactPersonnel", new { cid = oContactPersonnel.ContractorID, id = oContactPersonnel.ContactPersonnelID, ms = sErrorMessage });
                    //}                
                    //else
                    //{
                    //    oContactPersonnel.Photo = aPhotoImageInByteArray;
                    //}
                    #endregion
                    oEmp.Photo = aPhotoImageInByteArray;
                }
                #endregion


                _oEmployee = oEmp;
                _oEmployee.ErrorMessage = _oEmployee.EmployeeImageIU((int)Session[SessionInfo.currentUserID]);


            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }

            return RedirectToAction("View_EmployeeInformation", new { sid = Global.Encrypt(_oEmployee.EmployeeID.ToString()), sMsg = _oEmployee.ErrorMessage });

        }
        #endregion

        #region Employee Basic Info By Sagor
        public ActionResult BanglaPrint()
        {
            string sSQL = "SELECT HH.EmployeeID, HH.Name, HH.NameInBangla FROM View_Employee AS HH WHERE ISNULL(HH.NameInBangla,'N/A')!='N/A' AND ISNULL(HH.NameInBangla,'N/A')!=''";
            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Employee.rpt"));
            rd.SetDataSource(oEmployees);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch { throw; }


        }
        public ActionResult View_EmployeeInformations(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid); EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oEmployees = new List<Employee>();

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeGroups = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            ViewBag.IDCardFormats = GetPermissionList();
            return View(_oEmployees);
        }
        public List<EnumObject> GetPermissionList()
        {
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            List<EnumObject> objEnumObjects = new List<EnumObject>();

            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.IDCardFormat).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
            {
                if (oItem.OperationType == EnumRoleOperationType.IDCardProtrait)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Protrait;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Protrait);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardLandscape)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Landscape;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Landscape);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBothSideProtrait)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Both_Side_Protrait;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Both_Side_Protrait);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBothSideBangla)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Both_Side_Bangla;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Both_Side_Bangla);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBanglaF1)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.ID_Card_Bangla_F1;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.ID_Card_Bangla_F1);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBanglaF2)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.ID_Card_Bangla_F2;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.ID_Card_Bangla_F2);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBothSideProtraitF2)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Both_Side_Protrait_F2;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Both_Side_Protrait_F2);
                    objEnumObjects.Add(oEnumObject);
                }
            }
            return objEnumObjects;
        }
        public ActionResult View_EmployeeInformation(string sid, string sMsg)
        {
            int nEmpID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oEmployee = new Employee();

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            this.Session.Add(SessionInfo.ParamObj, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Employee).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            AttendanceScheme oAttendanceScheme = new AttendanceScheme();
            DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            string sSql = "SELECT * FROM View_EmployeeOfficial WHERE EmployeeID=" + nEmpID;
            List<EmployeeLeaveLedger> oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            List<EmployeeReportingPerson> oEmployeeReportingPersons = new List<EmployeeReportingPerson>();
            List<EmployeeNominee> oEmployeeNominees = new List<EmployeeNominee>();
            List<EmployeeCommissionMaterial> oEmployeeCommissionMaterials = new List<EmployeeCommissionMaterial>();

            try
            {
                if (nEmpID > 0)
                {
                    _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.SignatureInBase64String = (_oEmployee.Signature != null) ? "data:image/Jpeg;base64," + Convert.ToBase64String(_oEmployee.Signature) : "";

                    _oEmployee.EmployeeOfficial = EmployeeOfficial.Get(sSql, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.AttendanceScheme = oAttendanceScheme.Get(_oEmployee.EmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.DRP = oDepartmentRequirementPolicy.Get(_oEmployee.EmployeeOfficial.DRPID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeSalaryStructure = ESS(nEmpID);
                    _oEmployee.EmployeeAuthentications = EmployeeAuthentication.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);                    
                    oEmployeeLeaveLedgers = EmployeeLeaveLedger.GetsActiveLeaveLedger(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    oEmployeeReportingPersons = EmployeeReportingPerson.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    oEmployeeNominees = EmployeeNominee.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    oEmployeeCommissionMaterials = EmployeeCommissionMaterial.Gets("SELECT * FROM View_EmployeeCommissionMaterial WHERE EmployeeID=" + nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeConfirmations = EmployeeConfirmation.Gets("SELECT TOP(1)* FROM VIEW_EmployeeConfirmation WHERE EmployeeID=" + nEmpID + " ORDER BY ECID DESC", (int)Session[SessionInfo.currentUserID]);
                }
                _oEmployee.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeGroups = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeBlocks = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.Block, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmpGroups = EmployeeGroup.Gets("select * from View_EmployeeGroup where EmployeeID=" + nEmpID + " AND EmployeeTypeID IN(select EmployeeTypeID from EmployeeType where EmployeeGrouping =" + (int)EnumEmployeeGrouping.StaffWorker + ")", (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmpBlocks = EmployeeGroup.Gets("select * from View_EmployeeGroup where EmployeeID=" + nEmpID + " AND EmployeeTypeID IN(select EmployeeTypeID from EmployeeType where EmployeeGrouping =" + (int)EnumEmployeeGrouping.Block + ")", (int)Session[SessionInfo.currentUserID]);
                _oEmployee.SalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType IN(2,3)", (int)Session[SessionInfo.currentUserID]);
                ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql("SELECT TOP(1)* FROM View_User WHERE EmployeeID != 0 AND  EmployeeID=" + nEmpID, (int)Session[SessionInfo.currentUserID]);

                ViewBag.LeaveLedgerAuthorization = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LeaveLedger).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

                ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
                oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.COS = oTempClientOperationSetting;


                ClientOperationSetting oTempClientOperationSettingBE = new ClientOperationSetting();
                oTempClientOperationSettingBE = oTempClientOperationSettingBE.GetByOperationType((int)EnumOperationType.BanglaOrEnglish, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.COSBE = oTempClientOperationSettingBE;
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            if (sMsg != "N/A")
            {
                _oEmployee.ErrorMessage = sMsg;
            }

            sSql = "Select * from LeaveHead Where IsActive=1";
            oLeaveHeads = LeaveHead.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LeaveHeads = oLeaveHeads;

            ViewBag.EmployeeLeaveLedgers = oEmployeeLeaveLedgers;
            ViewBag.EmployeeReportingPersons = oEmployeeReportingPersons;
            ViewBag.RecruitmentEvents = Enum.GetValues(typeof(EnumRecruitmentEvent)).Cast<EnumRecruitmentEvent>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeNominees = oEmployeeNominees;

            ViewBag.CommissionMaterials = CommissionMaterial.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeCommissionMaterials = oEmployeeCommissionMaterials;


            ViewBag.BankBranchs = BankBranch.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(_oEmployee);
        }

        public ActionResult View_EmployeeInformation_Others(string sid, string sMsg)
        {
            int nEmpID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oEmployee = new Employee();
            try
            {
                if (nEmpID > 0)
                {
                    _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.BlockMachineMappingSupervisors = BlockMachineMappingSupervisor.Gets("SELECT * FROM View_BlockMachineMappingSupervisor WHERE EmployeeID =" + nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeEducations = EmployeeEducation.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeReferences = EmployeeReference.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
                }
                ViewBag.CustomDateFormat = EnumObject.jGets(typeof(EnumCustomDateFormat));
                ViewBag.EmployeeID = _oEmployee.EmployeeID;
                ViewBag.ProductionProcesss = Enum.GetValues(typeof(EnumProductionProcess)).Cast<EnumProductionProcess>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                ViewBag.EmployeeTINInformation = EmployeeTINInformation.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
                ViewBag.TaxAreas = Enum.GetValues(typeof(EnumTaxArea)).Cast<EnumTaxArea>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();


                string sSQL_Doc = "SELECT * FROM EmployeeDocument WHERE EDID <>0 AND EmployeeID=" + _oEmployee.EmployeeID;
                List<EmployeeDocument> oEmployeeDocuments = new List<EmployeeDocument>();
                oEmployeeDocuments = EmployeeDocument.Gets(sSQL_Doc, (int)Session[SessionInfo.currentUserID]);

                oEmployeeDocuments.ForEach(
                    x =>
                    {
                        x.DocFile = null;
                    }
                    );

                ViewBag.EmployeeDocuments = oEmployeeDocuments;

                ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
                oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.COS = oTempClientOperationSetting;
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            ViewBag.EmployeeDesignationTypes = EnumObject.jGets(typeof(EnumEmployeeDesignationType));
            return View(_oEmployee);
        }

        [HttpGet]
        public JsonResult GetsBoardNameAutocomplete(string BoardName)
        {
            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            BoardName = BoardName == null ? "" : BoardName;
            string sSQL = "SELECT DISTINCT BoardUniversity FROM EmployeeEducation AS HH WHERE HH.BoardUniversity LIKE '%" + BoardName + "%' ORDER BY HH.BoardUniversity ASC";
            oEmployeeEducations = EmployeeEducation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oEmployeeEducations, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            // Query is not required as of version 1.2.5
            //jsonResult = { "query": "Unit",    "suggestions": }
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetEmployeeOfficialByID(int nEmpID)
        {
            _oEmployee = new Employee();
            try
            {
                string sSql = "SELECT * FROM View_EmployeeOfficial WHERE EmployeeID=" + nEmpID;
                _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeOfficial = EmployeeOfficial.Get(sSql, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult SaveEmployeeGroup(EmployeeGroup oEmployeeGroup)
        {
            try
            {
                oEmployeeGroup = oEmployeeGroup.Save((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oEmployeeGroup = new EmployeeGroup();
                oEmployeeGroup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeGroup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeGroup(EmployeeGroup oEmployeeGroup)
        {
            List<EmployeeGroup> oEmployeeGroups = new List<EmployeeGroup>();
            try
            {
                string sSQL = "Select * from View_EmployeeGroup Where EmployeeID<> " + oEmployeeGroup.EmployeeID;
                oEmployeeGroups = EmployeeGroup.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oEmployeeGroups = new List<EmployeeGroup>();
                oEmployeeGroup = new EmployeeGroup();
                oEmployeeGroup.ErrorMessage = ex.Message;
                oEmployeeGroups.Add(oEmployeeGroup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeGroups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteEmployeeGroup(EmployeeGroup oEmployeeGroup)
        {
            string msg = "";
            try
            {
                msg = oEmployeeGroup.Delete(oEmployeeGroup.EGID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(msg);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Employee Reporting Person By Sagor

        [HttpPost]
        public JsonResult GetsReportingPerson(Employee oEmployee)
        {
            List<Employee> oEmployees = new List<Employee>();
            try
            {
                string sSQL = "Select * from View_Employee Where EmployeeID<> " + oEmployee.EmployeeID + " And (Name Like '%" + oEmployee.Name + "%' OR Code Like '%" + oEmployee.Code + "%')";
                oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

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

        [HttpPost]
        public JsonResult SaveReportingPerson(EmployeeReportingPerson oEmployeeReportingPerson)
        {
            try
            {

                if (oEmployeeReportingPerson.ERPID > 0)
                {
                    oEmployeeReportingPerson = oEmployeeReportingPerson.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oEmployeeReportingPerson = oEmployeeReportingPerson.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                oEmployeeReportingPerson = new EmployeeReportingPerson();
                oEmployeeReportingPerson.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeReportingPerson);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteReportingPerson(EmployeeReportingPerson oEmployeeReportingPerson)
        {
            try
            {

                if (oEmployeeReportingPerson.ERPID <= 0) { throw new Exception("Please select an item from list."); }
                if (!oEmployeeReportingPerson.IsActive) { throw new Exception("Unable to delete, You can't delete inactive one."); }
                oEmployeeReportingPerson = oEmployeeReportingPerson.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oEmployeeReportingPerson = new EmployeeReportingPerson();
                oEmployeeReportingPerson.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeReportingPerson.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee Signature By Sagor
        [HttpPost]
        public JsonResult SaveEmpSignature(double nts)
        {
            string sMessage = "";

            Employee oEmployee = new Employee();

            try
            {
                oEmployee.EmployeeID = Convert.ToInt32(Request.Headers["EmployeeID"]);

                byte[] data;
                #region File
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }
                    oEmployee.Signature = data;
                }
                else
                {
                    throw new Exception("No employee signature found to save.");
                }
                #endregion
                oEmployee = Employee.SaveSignature(oEmployee.EmployeeID, oEmployee.Signature, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oEmployee.EmployeeID > 0 && (oEmployee.ErrorMessage == null || oEmployee.ErrorMessage == ""))
                {
                    oEmployee.SignatureInBase64String = (oEmployee.Signature != null) ? "data:image/Jpeg;base64," + Convert.ToBase64String(oEmployee.Signature) : "";
                    sMessage = "Upload Successfully" + "~" + oEmployee.EmployeeID + '~' + oEmployee.SignatureInBase64String;
                }
                else
                {
                    if ((oEmployee.ErrorMessage == null || oEmployee.ErrorMessage == "")) { throw new Exception("Unable to Upload."); }
                    else { throw new Exception(oEmployee.ErrorMessage); }

                }
            }
            catch (Exception ex)
            {
                oEmployee = new Employee();
                oEmployee.SignatureInBase64String = "";
                sMessage = ex.Message + "~" + oEmployee.EmployeeID + '~' + oEmployee.SignatureInBase64String;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveEmpSignature(Employee oEmployee)
        {
            try
            {

                if (oEmployee.EmployeeID <= 0) { throw new Exception("No valid employee found."); }
                oEmployee = Employee.RemoveSignature(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oEmployee = new Employee();
                oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region File Attachment
        public ActionResult ViewEmployeeAttatchments(int nEmployeeID, int nDocType, int nDocTypeID, string sMessage)
        {
            EmployeeDoc oEmployeeDoc = new EmployeeDoc();
            List<EmployeeDoc> oEmployeeDocs = new List<EmployeeDoc>();
            oEmployeeDoc.EmployeeID = nEmployeeID;
            string sSQL = "Select * from EmployeeDoc Where EmployeeID=" + nEmployeeID + " AND DocType=" + nDocType + " AND DocTypeID=" + nDocTypeID + "";
            oEmployeeDocs = EmployeeDoc.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oEmployeeDoc.EmployeeDocs = oEmployeeDocs;
            TempData["message"] = sMessage;
            return PartialView(oEmployeeDoc);
        }

        [HttpPost]
        public ActionResult ViewEmployeeAttatchment(HttpPostedFileBase file, EmployeeDoc oEmployeeDoc)
        {
            string sErrorMessage = ""; int nEmployeeID = 0, nDocType = 0, nDocTypeID = 0;
            try
            {
                #region Excel
                //var fileName = Path.GetFileName(file.FileName);
                //file.SaveAs(Path.Combine(@"D:\Attach_Ment", fileName));


                byte[] data;
                using (Stream inputStream = file.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                nEmployeeID = oEmployeeDoc.EmployeeID;
                nDocType = oEmployeeDoc.DocTypeInt;
                nDocTypeID = oEmployeeDoc.DocTypeID;
                oEmployeeDoc.AttachmentFile = data;
                //oEmployeeDoc.IssueDate = file.FileName;
                oEmployeeDoc.FileType = file.ContentType;
                oEmployeeDoc.DocType = (EnumEmployeeDoc)oEmployeeDoc.DocTypeInt;
                oEmployeeDoc = oEmployeeDoc.Save((int)Session[SessionInfo.currentUserID]);
                if (oEmployeeDoc.EmployeeDocID > 0 && oEmployeeDoc.ErrorMessage == "")
                {
                    sErrorMessage = "Save Successfully";
                }
                #endregion
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }

            return RedirectToAction("ViewEmployeeAttatchments", new { nEmployeeID = nEmployeeID, nDocType = nDocType, nDocTypeID = nDocTypeID, sMessage = sErrorMessage });
        }

        public ActionResult DownloadEmployeeAttachment(int id)
        {
            EmployeeDoc oEmployeeDoc = new EmployeeDoc();
            try
            {
                oEmployeeDoc.EmployeeDocID = id;
                oEmployeeDoc = oEmployeeDoc.Get(id, (int)Session[SessionInfo.currentUserID]);
                if (oEmployeeDoc.AttachmentFile != null)
                {
                    //var fileName = Path.GetFileName(file.FileName);
                    //file.SaveAs(Path.Combine(@"D:\Attach_Ment", fileName));
                    //http://stackoverflow.com/questions/181214/file-input-accept-attribute-is-it-useful

                    //var file = File(oTechnicalSheetThumbnail.ThumbnailImage, "application/vnd.ms-excel");
                    //file.FileDownloadName = oTechnicalSheetThumbnail.ImageTitle+".xlsx";                    

                    //var file = File(oTechnicalSheetThumbnail.ThumbnailImage, "application/vnd.ms-word");
                    //file.FileDownloadName = oTechnicalSheetThumbnail.ImageTitle;

                    //var file = File(oTechnicalSheetThumbnail.ThumbnailImage, "application/zip");
                    //file.FileDownloadName = oTechnicalSheetThumbnail.ImageTitle;


                    var file = File(oEmployeeDoc.AttachmentFile, oEmployeeDoc.FileType);
                    file.FileDownloadName = oEmployeeDoc.Description;

                    return file;

                    //MemoryStream ms = new MemoryStream();
                    //ms.Write(oTechnicalSheetThumbnail.ThumbnailImage, 0, oTechnicalSheetThumbnail.ThumbnailImage.Length);
                    //ms.Position = 0;
                    //Workbook workbook = new Workbook(ms);
                    //return File(workbook.Path, "application/zip", oTechnicalSheetThumbnail.ImageTitle);


                    //MemoryStream ms = new MemoryStream();
                    //ms.Write(oTechnicalSheetThumbnail.ThumbnailImage, 0, oTechnicalSheetThumbnail.ThumbnailImage.Length);
                    //ms.Position = 0;
                    //Workbook workbook = new Workbook(ms);
                    //string fileName = "test.xlsx";
                    //workbook.SaveAs(Path.Combine(@"D:\Attach_Ment", fileName));
                    ////workbook.Save(filePath + ".out.xlsx");
                    ////workbook.Save(filePath + ".out.pdf", SaveFormat.Pdf);
                    //return workbook;
                }
                else
                {
                    return null;
                }

                //var fs = System.IO.File.OpenRead(Server.MapPath("/some/path/" + fileName));
                //return File(fs, "application/zip", fileName);
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oEmployeeDoc.Description);
            }
        }

        public ActionResult DeleteEmployeeAttachment(int id)
        {
            EmployeeDoc oEmployeeDoc = new EmployeeDoc();
            oEmployeeDoc = oEmployeeDoc.Get(id, (int)Session[SessionInfo.currentUserID]);
            int nEmployeeID = oEmployeeDoc.EmployeeID;
            int nDocType = (int)oEmployeeDoc.DocType;
            int nDocTypeID = oEmployeeDoc.DocTypeID;
            string sErrorMessage = "";
            try
            {
                sErrorMessage = oEmployeeDoc.Delete(oEmployeeDoc.EmployeeDocID.ToString(), (int)Session[SessionInfo.currentUserID]);
                if (sErrorMessage == "")
                {
                    sErrorMessage = "Delete Successfully";
                }
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }
            return RedirectToAction("ViewEmployeeAttatchments", new { nEmployeeID = nEmployeeID, nDocType = nDocType, nDocTypeID = nDocTypeID, sMessage = sErrorMessage });
        }

        //[HttpPost]
        //public JsonResult DeleteEmployeeAttachment(string sIDs, double nts)
        //{
        //    EmployeeDoc oEmployeeDoc = new EmployeeDoc();
        //    string sErrorMease = "";

        //    try
        //    {
        //        sErrorMease = oEmployeeDoc.Delete(sIDs, (int)Session[SessionInfo.currentUserID]);
        //        if (sErrorMease == "")
        //        {
        //            sErrorMease = "Delete Successfully";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sErrorMease = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sErrorMease);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult DownloadEmployeeAsZip(string sIDs, double nts)
        //{
        //    string sErrorMease = "";

        //    try
        //    {
        //        List<EmployeeDoc> oEmployeeDocs = new List<EmployeeDoc>();
        //        string sSQL = "Select * from EmployeeDoc Where EmployeeDocID In (" + sIDs + ")";
        //        oEmployeeDocs = EmployeeDoc.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        var file = (dynamic)null;
        //        foreach(EmployeeDoc  oItem in oEmployeeDocs)
        //        {
        //            if (oItem.AttachmentFile != null)
        //            {
        //                file = File(oItem.AttachmentFile, oItem.FileType);
        //                file.FileDownloadName = oItem.Description;
        //                ZipFile zip = new ZipFile();

        //            }

        //        }
        //        if (sErrorMease == "")
        //        {
        //            sErrorMease = "Delete Successfully";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sErrorMease = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sErrorMease);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Print
        public ActionResult View_PrintEmployeeCV()
        {
            _oEmployee = new Employee();
            return PartialView(_oEmployee);
        }

        public ActionResult PrintEmployeeCV(string sParams)
        {
            int nEmployeeID = Convert.ToInt32(sParams.Split('~')[0]);
            bool IsBasicInfo = Convert.ToBoolean(sParams.Split('~')[1]);
            bool IsOfficialInfo = Convert.ToBoolean(sParams.Split('~')[2]);
            bool IsSalaryInfo = Convert.ToBoolean(sParams.Split('~')[3]);

            _oEmployee = new Employee();

            //string sEmpSql = "SELECT * FROM View_Employee_WithImage WHERE EmployeeID=" + nEmployeeID;
            _oEmployee = _oEmployee.Get(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeePhoto = GetImage(_oEmployee.Photo);
            if (IsBasicInfo == true)
            {
                _oEmployee.EmployeeEducations = EmployeeEducation.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeReferences = EmployeeReference.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
            }
            if (IsOfficialInfo == true)
            {
                EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
                string sSql = "";
                sSql = "SELECT * FROM View_EmployeeOfficialALL WHERE EmployeeID=" + nEmployeeID;
                _oEmployee.EmployeeOfficial = EmployeeOfficial.Get(sSql, (int)Session[SessionInfo.currentUserID]);

                if (_oEmployee.EmployeeOfficial.AttendanceSchemeID > 0)
                {
                    AttendanceScheme oAS = new AttendanceScheme();
                    _oEmployee.AttendanceScheme = oAS.Get(_oEmployee.EmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.AttendanceSchemeHolidays = AttendanceSchemeHoliday.Gets(_oEmployee.EmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.AttendanceSchemeLeaves = AttendanceSchemeLeave.Gets(_oEmployee.EmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                    _oEmployee.RosterPlanDetails = RosterPlanDetail.Gets(_oEmployee.EmployeeOfficial.AttendanceSchemeID, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oEmployee.AttendanceScheme = new AttendanceScheme();
                    _oEmployee.AttendanceSchemeHolidays = new List<AttendanceSchemeHoliday>();
                    _oEmployee.AttendanceSchemeLeaves = new List<AttendanceSchemeLeave>();
                    _oEmployee.RosterPlanDetails = new List<RosterPlanDetail>();
                }
            }
            if (IsSalaryInfo == true)
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
                string Ssql_ESS = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID;
                _oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(Ssql_ESS, (int)Session[SessionInfo.currentUserID]);

                string Ssql = "SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID=" + _oEmployeeSalaryStructure.ESSID;
                oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(Ssql, (int)Session[SessionInfo.currentUserID]);

                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();

                string sSql = "SELECT * FROM  View_SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + " ORDER BY SalarySchemeDetailID ";
                string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + ") ORDER BY SalarySchemeDetailID ";

                oSalarySchemeDetails = SalarySchemeDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, (int)Session[SessionInfo.currentUserID]);
                oSalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalarySchemeDetails, oSalarySchemeDetailCalculations);

                foreach (SalarySchemeDetail oSSDItem in oSalarySchemeDetails)
                {
                    foreach (EmployeeSalaryStructureDetail oESSDItem in oEmployeeSalaryStructureDetails)
                    {

                        if (oSSDItem.SalaryHeadID == oESSDItem.SalaryHeadID)
                        {
                            oESSDItem.Calculation = oSSDItem.Calculation;
                        }
                    }

                }

                _oEmployee.EmployeeSalaryStructure = _oEmployeeSalaryStructure;
                _oEmployee.EmployeeSalaryStructureDetails = oEmployeeSalaryStructureDetails;
            }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.ErrorMessage = sParams;

            rptEmployeeCV oReport = new rptEmployeeCV();
            byte[] abytes = oReport.PrepareReport(_oEmployee);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintJobApplication(int nEmpID, string sLanguage, double ts)
        {

            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();

            byte[] abytes;
            if (sLanguage == "Bangla")
            {
                rptEmployeeJobApplicationInBangla oReport = new rptEmployeeJobApplicationInBangla();
                abytes = oReport.PrepareReport(_oEmployee);
            }
            else
            {
                rptEmployeeJobApplication oReport = new rptEmployeeJobApplication();
                abytes = oReport.PrepareReport(_oEmployee);

            }
            return File(abytes, "application/pdf");

        }

        public ActionResult rptEmployeeInBangla(int nEmpID, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();
            return this.ViewPdf("", "rptEmployeeInBangla", _oEmployee, iTextSharp.text.PageSize.A4_LANDSCAPE, 40, 40, 40, 40);
        }

        public ActionResult PrintAppointmentLetter(int nEmpID, string sLanguage, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            string sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID + ") AND SalaryHeadType=1 ";
            _oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();

            byte[] abytes;
            if (sLanguage == "Bangla")
            {
                rptAppointmentLetterInBangla oReport = new rptAppointmentLetterInBangla();
                abytes = oReport.PrepareReport(_oEmployee);
            }
            else
            {
                rptAppointmentLetter oReport = new rptAppointmentLetter();
                abytes = oReport.PrepareReport(_oEmployee);
            }
            return File(abytes, "application/pdf");
        }

        public ActionResult View_PrintAppointmentLetterInBangla(int nEmpID, string sLanguage, double ts)
        {

            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            string sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID + ") AND SalaryHeadType=1 ";
            _oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();

            return PartialView(_oEmployee);

        }

        public ActionResult View_PrintAppointmentLetterInBangla_ZN(int nEmpID, string sLanguage, double ts)
        {

            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            string sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID + ") AND SalaryHeadType=1 ";
            _oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]).OrderBy(x => x.SalaryHeadID).ToList();
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();

            return PartialView(_oEmployee);

        }

        public ActionResult View_PrintJoiningLetterInBangla(int nEmpID, double ts)
        {

            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();
            return PartialView(_oEmployee);
        }

        public ActionResult PrintEmployeeList(string sEmployeeIDs, double ts)
        {
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM VIEW_Employee WHERE EmployeeID IN (" + sEmployeeIDs + ")";
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            string sSql1 = "SELECT * FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID IN (" + sEmployeeIDs + ")";

            _oEmployee.EmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql1, (int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();

            rptEmployeeList oReport = new rptEmployeeList();
            byte[] abytes = oReport.PrepareReport(_oEmployee);

            return File(abytes, "application/pdf");
        }

        public ActionResult PrintCrerwInformation(int nEmpID, double ts)
        {
            _oEmployee = new Employee();
            string sEmployeeSql = "SELECT * FROM VIEW_Crew WHERE EmployeeID =" + nEmpID;
            _oEmployee = Employee.Get(sEmployeeSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeReferences = EmployeeReference.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            rptCrewInformation oReport = new rptCrewInformation();
            byte[] abytes = oReport.PrepareReport(_oEmployee);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintDiscontinuedEmployee(string sEmployeeIDs, string sDateRange, double ts)
        {
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM VIEW_Employee WHERE EmployeeID IN (" + sEmployeeIDs + ")";
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDateRange;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            rptDiscontinuedEmployee oReport = new rptDiscontinuedEmployee();
            byte[] abytes = oReport.PrepareReport(_oEmployee);

            return File(abytes, "application/pdf");

        }

        //public ActionResult PrintLeftyEmployee(string sEmployeeIDs, string sDateRange, double ts)
        //{
        //    _oEmployee = new Employee();


        //    string sSql = "SELECT * FROM VIEW_Employee WHERE EmployeeID IN (" + sEmployeeIDs + ")";
        //    _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);


        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.ErrorMessage = sDateRange;
        //    _oEmployee.Company = oCompanys.First();
        //    _oEmployee.Company.CompanyLogo = GetCompanyLogo(_oEmployee.Company);


        //    rptLeftyEmployee oReport = new rptLeftyEmployee();
        //    byte[] abytes = oReport.PrepareReport(_oEmployee);

        //    return File(abytes, "application/pdf");
        //}

        public ActionResult PrintNewEmployee(string sEmployeeIDs, string sDateRange, double ts)
        {
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM VIEW_Employee WHERE EmployeeID IN (" + sEmployeeIDs + ")";
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDateRange;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);


            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployee.Employees.Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", (int)Session[SessionInfo.currentUserID]); }

            rptNewEmployeeList oReport = new rptNewEmployeeList();
            byte[] abytes = oReport.PrepareReport(_oEmployee, oBusinessUnits);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintEmployeeCard_Landscape(string sIDs, string sIssueDate, string sExpireDate, double ts)
        {

            _oEmployee = new Employee();
            DateTime dIssueDate = Convert.ToDateTime(sIssueDate);
            DateTime dExpireDate = Convert.ToDateTime(sExpireDate);
            string sSql = "select * from View_Employee_For_IDCard_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            //_oEmployee.Company.CompanyLogo = null; //GetImage(_oEmployee.Company.OrganizationLogo); ;
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            //_oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }

            rptEmployeeCard oReport = new rptEmployeeCard();
            byte[] abytes = oReport.PrepareReport(_oEmployee, dIssueDate, dExpireDate);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintEmployeeCard_Potrait(string sIDs, string itemIDs, double ts)
        {
            _oEmployee = new Employee();
            string sSql = "select * from View_Employee_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }
            _oEmployee.ErrorMessage = itemIDs;
            rptEmployeeCard_Potrait oReport = new rptEmployeeCard_Potrait();
            byte[] abytes = oReport.PrepareReport(_oEmployee);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintEmployeeCard_Potrait_BothSideF2(string sIDs, string itemIDs, string sIssueDate, string sExpireDate, double ts)
        {
            DateTime dIssueDate = Convert.ToDateTime(sIssueDate);
            DateTime dExpireDate = Convert.ToDateTime(sExpireDate);

            _oEmployee = new Employee();
            string sSql = "select * from View_Employee_WithImage WHERE EmployeeID IN(" + sIDs + ") ORDER BY Code";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.OrganizationTitleLogo = GetImage(_oEmployee.Company.OrganizationTitle);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }
            _oEmployee.ErrorMessage = itemIDs;
            rptEmployeeCard_Potrait_BothSideF2 oReport = new rptEmployeeCard_Potrait_BothSideF2();
            byte[] abytes = oReport.PrepareReport(_oEmployee, dIssueDate, dExpireDate);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintEmployeeCard_Potrait_BothSide(string sIDs, string itemIDs, string sIssueDate, string sExpireDate, double ts)
        {
            DateTime dIssueDate = Convert.ToDateTime(sIssueDate);
            DateTime dExpireDate = Convert.ToDateTime(sExpireDate);

            _oEmployee = new Employee();
            string sSql = "select * from View_Employee_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }
            _oEmployee.ErrorMessage = itemIDs;
            rptEmployeeCard_Potrait_BothSide oReport = new rptEmployeeCard_Potrait_BothSide();
            byte[] abytes = oReport.PrepareReport(_oEmployee, dIssueDate, dExpireDate);
            return File(abytes, "application/pdf");
        }
        public Image GetImage(byte[] Image, string sImageName = "Image.jpg")
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Images/" + sImageName);
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;

            }
            else
            {
                return null;
            }
        }
        //public ActionResult PrintEmployeeCard_Potrait_BothSideBangla(string sIDs, string itemIDs, string sIssueDate, string sExpireDate,  double ts)
        //{
        //    DateTime dIssueDate = Convert.ToDateTime(sIssueDate);
        //    DateTime dExpireDate = Convert.ToDateTime(sExpireDate);

        //    _oEmployee = new Employee();
        //    string sSql = "select * from View_Employee_WithImage WHERE EmployeeID IN(" + sIDs + ")";
        //    _oEmployee.EmployeeHrms = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    List<Company> oCompanys = new List<Company>();
        //    oCompany = oCompany.GetCompanyLogo(1, (int)(Session[SessionInfo.currentUserID]));
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    oCompany.AuthSig = GetImage(oCompany.AuthorizedSignature, "AuthSig.jpg");
        //    oCompanys.Add(oCompany);

        //    foreach (Employee oItem in _oEmployee.EmployeeHrms)
        //    {
        //        oItem.EmployeePhoto = GetImage(oItem.Photo);
        //        oItem.EmployeeSiganture = GetImage(oItem.Signature);
        //    }
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "BanglaIDCardBothSidePotrait.rpt"));
        //    rd.Database.Tables["EmployeeForIDCard"].SetDataSource(_oEmployee.EmployeeHrms);
        //    rd.Database.Tables["Company"].SetDataSource(oCompanys);
        //    //rd.SetDataSource(_oEmployeeForIDCards);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "application/pdf");
        //    }
        //    catch { throw; }
        //}
        public ActionResult IDCardBanglaBothSide(string sEmpIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime expDate, int nRowLength, int nLoadRecords, string sGroupIDs, string sBlockIDs)
        {
            List<EmployeeForIDCard> _oEmployeeForIDCards = new List<EmployeeForIDCard>();
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Employee_For_IDCard WHERE EmployeeID <>0";

            //string sSql = "select * from View_Employee_For_IDCard WHERE EmployeeID<>0";
            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }
            if (!string.IsNullOrEmpty(sBusinessUnitIds))
            {
                sSql += " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (!string.IsNullOrEmpty(sLocationID))
            {
                sSql += " AND LocationID IN(" + sLocationID + ")";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql += " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (!string.IsNullOrEmpty(sGroupIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
            }
            if (!string.IsNullOrEmpty(sBlockIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
            }
            sSql = sSql + ") aa WHERE Row >" + nRowLength + " AND IsActive = 1 Order By Code";
            _oEmployeeForIDCards = EmployeeForIDCard.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            List<Company> oCompanys = new List<Company>();
            oCompany = oCompany.GetCompanyLogo(1, (int)(Session[SessionInfo.currentUserID]));
            //oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo, "ComanyLogo.jpg");
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.AuthSig = GetImage(oCompany.AuthorizedSignature, "AuthSig.jpg");
            oCompanys.Add(oCompany);

            foreach (EmployeeForIDCard oEmployeeForIDCard in _oEmployeeForIDCards)
            {
                oEmployeeForIDCard.AuthSigPath = Server.MapPath("~/Content/Images/AuthSig.jpg");
                oEmployeeForIDCard.CompanyLogoPath = Server.MapPath("~/Content/Images/ComanyLogo.jpg");
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "BanglaIDCardBothSide.rpt"));
            rd.Database.Tables["EmployeeForIDCard"].SetDataSource(_oEmployeeForIDCards);
            rd.Database.Tables["Company"].SetDataSource(oCompanys);
            //rd.SetDataSource(_oEmployeeForIDCards);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //string[] months = new string[] { "জানুয়ারী", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };
                //string[] months = new string[] { "Rvbyqvix", "‡deªyqvwi", "gvP©", "GwcÖj", "‡g", "Ryb", "RyjvB", "AMvó", "‡m‡Þ¤^i", "A‡±vei", "b‡f¤^i", "wW‡m¤^i" };
                //string sMonthName = months[nMonthID - 1] + " " + NumberFormatWithBijoy(nYear.ToString());

                //TextObject txtExpDate = (TextObject)rd.ReportDefinition.Sections["Section3"].ReportObjects["txtExpDate"];
                //txtExpDate.Text = expDate.ToString("dd/MM/yyyy");

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch { throw; }
        }
        #endregion


        #region PrintWithFormat
        public ActionResult PrintFormat1(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM VIEW_Employee WHERE CONVERT(DATE, JoiningDate) BETWEEN'" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
            }
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);


            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployee.Employees.Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", (int)Session[SessionInfo.currentUserID]); }


            rptNewEmployeeList oReport = new rptNewEmployeeList();
            byte[] abytes = oReport.PrepareReport(_oEmployee, oBusinessUnits);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFormat2(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM VIEW_Employee WHERE CONVERT(DATE, JoiningDate) BETWEEN'" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
            }
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployee.Employees.Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", (int)Session[SessionInfo.currentUserID]); }

            rptNewEmployeeListWithSummery oReport = new rptNewEmployeeListWithSummery();
            byte[] abytes = oReport.PrepareReport(_oEmployee, oBusinessUnits);
            return File(abytes, "application/pdf");
        }


        public ActionResult PrintLeftyEmployee(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, bool bIsPermissionSalary, int Format, double ts)
        {
            byte[] abytes = null;
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            _oEmployee = new Employee();
            string sSql;

            if (Format == 1 || Format == 2)
            {
                sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
                if (sBusinessUnitIds != "")
                {
                    sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
                }
                if (sLocationID != "")
                {
                    sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
                }
                if (sDepartmentIds != "")
                {
                    sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIds != "")
                {
                    sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
                }
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
                }
                _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

                sSql = "SELECT * FROM EmployeeSettlement WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";

                _oEmployee.EmployeeSettlements = EmployeeSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);


                List<Company> oCompanys = new List<Company>();
                oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
                _oEmployee.ErrorMessage = sDate + "~" + eDate;
                _oEmployee.Company = oCompanys.First();
                _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            }

            if (Format == 3)
            {
                sSql = "SELECT * FROM View_Employee WHERE IsActive=0 AND EmployeeID IN (SELECT EmployeeID FROM EmployeeActiveInactiveHistory WHERE InactiveDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
                if (sBusinessUnitIds != "")
                {
                    sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
                }
                if (sLocationID != "")
                {
                    sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
                }
                if (sDepartmentIds != "")
                {
                    sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIds != "")
                {
                    sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
                }
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
                }

                _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

                sSql = "Select * from EmployeeActiveInactiveHistory Where EAIHID In (Select MAX(EAIHID) from EmployeeActiveInactiveHistory Where InactiveDate IS NOT NULL group by EmployeeID) AND InactiveDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";

                _oEmployee.EmployeeActiveInactiveHistorys = EmployeeActiveInactiveHistory.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

                List<Company> oCompanys = new List<Company>();
                oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
                _oEmployee.ErrorMessage = sDate + "~" + eDate;
                _oEmployee.Company = oCompanys.First();
                _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            }
            if (Format == 4)
            {
                sSql = "SELECT * FROM View_Employee_WithImage WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
                if (sBusinessUnitIds != "")
                {
                    sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
                }
                if (sLocationID != "")
                {
                    sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
                }
                if (sDepartmentIds != "")
                {
                    sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIds != "")
                {
                    sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
                }
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DRPID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
                }
                _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

                sSql = "SELECT * FROM EmployeeSettlement WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";

                _oEmployee.EmployeeSettlements = EmployeeSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                foreach (Employee oItem in _oEmployee.Employees)
                {
                    oItem.EmployeePhoto = GetEmployeePhoto(oItem);
                }

                List<Company> oCompanys = new List<Company>();
                oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
                _oEmployee.ErrorMessage = sDate + "~" + eDate;
                _oEmployee.Company = oCompanys.First();
                _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            }

            if (Format == 1)
            {
                rptDiscontinuedEmployee oReport = new rptDiscontinuedEmployee();
                abytes = oReport.PrepareReport(_oEmployee);
            }
            if (Format == 2)
            {
                rptDiscontinuedEmployeeWithSummery oReport = new rptDiscontinuedEmployeeWithSummery();
                abytes = oReport.PrepareReport(_oEmployee);
            }
            if (Format == 3)
            {
                rptDiscontinuedInactiveEmployeeWithSummery oReport = new rptDiscontinuedInactiveEmployeeWithSummery();
                abytes = oReport.PrepareReport(_oEmployee);
            }
            if (Format == 4)
            {
                rptDiscontinuedEmployeeWithImage oReport = new rptDiscontinuedEmployeeWithImage();
                abytes = oReport.PrepareReport(_oEmployee);
            }

            return File(abytes, "application/pdf");
        }
        public Image GetEmployeePhoto(Employee oEmployee)
        {
            if (oEmployee.Photo != null)
            {
                MemoryStream m = new MemoryStream(oEmployee.Photo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "Employeeimage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        //public ActionResult PrintFormat1Left(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, bool bIsPermissionSalary, double ts)
        //{
        //    DateTime dt = Convert.ToDateTime(sDate);
        //    DateTime dtTo = Convert.ToDateTime(eDate);
        //    _oEmployee = new Employee();
        //    string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
        //    if (sBusinessUnitIds != "")
        //    {
        //        sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
        //    }
        //    if (sLocationID != "")
        //    {
        //        sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
        //    }
        //    if (sDepartmentIds != "")
        //    {
        //        sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
        //    }
        //    if (sDesignationIds != "")
        //    {
        //        sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
        //    }
        //    if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
        //    {
        //        sSql = sSql + " AND DRPID "
        //                    + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
        //    }
        //    _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

        //    sSql = "SELECT * FROM EmployeeSettlement WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";

        //    _oEmployee.EmployeeSettlements = EmployeeSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);


        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.ErrorMessage = sDate + "~" + eDate;
        //    _oEmployee.Company = oCompanys.First();
        //    _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

        //    rptDiscontinuedEmployee oReport = new rptDiscontinuedEmployee();
        //    byte[] abytes = oReport.PrepareReport(_oEmployee);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult PrintFormat2Left(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, bool bIsPermissionSalary, double ts)
        //{
        //    DateTime dt = Convert.ToDateTime(sDate);
        //    DateTime dtTo = Convert.ToDateTime(eDate);
        //    _oEmployee = new Employee();
        //    string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
        //    if (sBusinessUnitIds != "")
        //    {
        //        sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
        //    }
        //    if (sLocationID != "")
        //    {
        //        sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
        //    }
        //    if (sDepartmentIds != "")
        //    {
        //        sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
        //    }
        //    if (sDesignationIds != "")
        //    {
        //        sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
        //    }
        //    if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
        //    {
        //        sSql = sSql + " AND DRPID "
        //                    + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
        //    }
        //    _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

        //    sSql = "SELECT * FROM EmployeeSettlement WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";

        //    _oEmployee.EmployeeSettlements = EmployeeSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.ErrorMessage = sDate + "~" + eDate;
        //    _oEmployee.Company = oCompanys.First();
        //    _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

        //    rptDiscontinuedEmployeeWithSummery oReport = new rptDiscontinuedEmployeeWithSummery();
        //    byte[] abytes = oReport.PrepareReport(_oEmployee);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult PrintFormat3Left(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, bool bIsPermissionSalary, double ts)
        //{
        //    DateTime dt = Convert.ToDateTime(sDate);
        //    DateTime dtTo = Convert.ToDateTime(eDate);
        //    _oEmployee = new Employee();
        //    //string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt + "' AND '" + dtTo + "')";

        //    string sSql = "SELECT * FROM View_Employee WHERE IsActive=0 AND EmployeeID IN (SELECT EmployeeID FROM EmployeeActiveInactiveHistory WHERE InactiveDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
        //    if (sBusinessUnitIds != "")
        //    {
        //        sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
        //    }
        //    if (sLocationID != "")
        //    {
        //        sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
        //    }
        //    if (sDepartmentIds != "")
        //    {
        //        sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
        //    }
        //    if (sDesignationIds != "")
        //    {
        //        sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
        //    }
        //    if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
        //    {
        //        sSql = sSql + " AND DRPID "
        //                    + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
        //    }

        //    _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

        //    sSql = "Select * from EmployeeActiveInactiveHistory Where EAIHID In (Select MAX(EAIHID) from EmployeeActiveInactiveHistory Where InactiveDate IS NOT NULL group by EmployeeID) AND InactiveDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";

        //    _oEmployee.EmployeeActiveInactiveHistorys = EmployeeActiveInactiveHistory.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
        //    _oEmployee.ErrorMessage = sDate + "~" + eDate;
        //    _oEmployee.Company = oCompanys.First();
        //    _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

        //    rptDiscontinuedInactiveEmployeeWithSummery oReport = new rptDiscontinuedInactiveEmployeeWithSummery();
        //    byte[] abytes = oReport.PrepareReport(_oEmployee);
        //    return File(abytes, "application/pdf");
        //}

        #endregion

        #region Excel

        public void ExcelFormat1(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            List<Department> _oDepartments = new List<Department>();
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM VIEW_Employee WHERE CONVERT(DATE, JoiningDate) BETWEEN'" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
            }
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            sSql = "SELECT * FROM View_EmployeeSalaryStructure WHERE JoiningDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            sSql = "SELECT * FROM EmployeeEducation WHERE PassingYear IN(SELECT MAX(PassingYear) FROM EmployeeEducation GROUP BY EmployeeID) AND EmployeeID IN (SELECT EmployeeID FROM VIEW_Employee WHERE JoiningDate BETWEEN '" + dt + "' AND '" + dtTo + "')";
            oEmployeeEducations = EmployeeEducation.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployee.Employees.Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", (int)Session[SessionInfo.currentUserID]); }

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            int nMaxColumn = 6;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("NEW EMPLOYEE List");
                sheet.Name = "NEW EMPLOYEE LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeName
                sheet.Column(4).Width = 20; //father/hausband
                sheet.Column(5).Width = 20; //age
                sheet.Column(6).Width = 20; //BU
                sheet.Column(7).Width = 20; //Loc
                sheet.Column(8).Width = 20; //Department
                sheet.Column(9).Width = 20; //Designation
                sheet.Column(9).Width = 20; //Employee Grade
                sheet.Column(10).Width = 20; //educationh
                sheet.Column(11).Width = 20; //card
                sheet.Column(12).Width = 20; //DOB
                sheet.Column(13).Width = 20; //Joining Date
                if (bView) { sheet.Column(14).Width = 20; } //Last Gross


                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 4]; cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "NEW EMPLOYEE LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "From " + sDate + " To " + eDate; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region Table Body
                int nSL = 0;

                _oDepartments = _oEmployee.Departments;
                _oEmployees = _oEmployee.Employees;

                //foreach (Department oDepartmentItem in _oDepartments)
                //{
                //    List<Employee> oEmployees = (from oEs in _oEmployees
                //                                 where oEs.DepartmentID == oDepartmentItem.DepartmentID
                //                                 select oEs).ToList();

                //    if (oEmployees.Count > 0)
                //    {
                //        #region Table Header 02

                //        cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Value = "SEC : " + oEmployees[0].DepartmentName; cell.Merge = true; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //        rowIndex++;
                //        colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Father/Husband"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Age"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Business Unit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Grade"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Education"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bView)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                rowIndex++;
                #endregion

                foreach (Employee oItem in _oEmployee.Employees)
                {
                    nSL++;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.FatherName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateDifference sAge = new DateDifference(oItem.DateOfBirth, DateTime.Now);

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = sAge.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.BUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.EmployeeTypeName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    string sEducation = "";
                    List<EmployeeEducation> oEdus = new List<EmployeeEducation>();
                    oEdus = oEmployeeEducations.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                    if (oEdus.Count > 0) { sEducation = oEdus[0].Degree; }

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = sEducation; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    if (bView)
                    {
                        cell = sheet.Cells[rowIndex, 15];
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        List<EmployeeSalaryStructure> oESSs = new List<EmployeeSalaryStructure>();
                        oESSs = oEmployeeSalaryStructures.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                        double GrossAmount = 0;
                        if (oESSs.Count > 0) { GrossAmount = oESSs[0].GrossAmount; }
                        cell.Value = GrossAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    rowIndex++;
                }
                rowIndex++;
                nSL = 0;

                //    }
                //}
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=NewEmployeeList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }

        #endregion


        public void ExcelFormat2(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            List<Department> _oDepartments = new List<Department>();
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM VIEW_Employee WHERE CONVERT(DATE, JoiningDate) BETWEEN'" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            sSql = "SELECT * FROM View_EmployeeSalaryStructure WHERE JoiningDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            sSql = "SELECT * FROM EmployeeEducation WHERE PassingYear IN(SELECT MAX(PassingYear) FROM EmployeeEducation GROUP BY EmployeeID) AND EmployeeID IN (SELECT EmployeeID FROM VIEW_Employee WHERE JoiningDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeEducations = EmployeeEducation.Gets(sSql, (int)Session[SessionInfo.currentUserID]);


            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployee.Employees.Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", (int)Session[SessionInfo.currentUserID]); }

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            int nMaxColumn = 6;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("NEW EMPLOYEE List");
                sheet.Name = "NEW EMPLOYEE LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //ID
                sheet.Column(4).Width = 20; //Name
                sheet.Column(5).Width = 20; //father/hausband
                sheet.Column(6).Width = 20; //age
                sheet.Column(7).Width = 20; //BU
                sheet.Column(8).Width = 20; //Loc
                sheet.Column(9).Width = 20; //Department
                sheet.Column(10).Width = 20; //Designation
                sheet.Column(11).Width = 20; //educationh
                sheet.Column(12).Width = 20; //DOB
                sheet.Column(13).Width = 20; //Joining Date
                if (bView) { sheet.Column(14).Width = 20; } //Last Gross


                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 4]; cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "NEW EMPLOYEE LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "From " + sDate + " To " + eDate; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region Table Body
                int nSL = 0;

                _oDepartments = _oEmployee.Departments;
                _oEmployees = _oEmployee.Employees;

                foreach (Department oDepartmentItem in _oDepartments)
                {

                    List<Employee> oEmployees = (from oEs in _oEmployees

                                                 where oEs.DepartmentID == oDepartmentItem.DepartmentID

                                                 select oEs).ToList();
                    if (oEmployees.Count > 0)
                    {
                        rec = new record();
                        rec.NoOfEmployee = oEmployees.Count();
                        rec.DepartmentName = oEmployees[0].DepartmentName;
                        showSummery.Add(rec);
                    }
                }
                //if (oEmployees.Count > 0)
                //{
                //    #region Table Header 02

                //    cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Value = "SEC : " + oEmployees[0].DepartmentName; cell.Merge = true; cell.Style.Font.Bold = false;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //    rowIndex++;
                //    colIndex = 2;
                #region Table Header 02
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Father/Husband"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Age"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Business Unit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Education"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bView)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
                #endregion

                foreach (Employee oItem in _oEmployee.Employees)
                {
                    nSL++;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.FatherName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateDifference sAge = new DateDifference(oItem.DateOfBirth, DateTime.Now);

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = sAge.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.BUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    string sEducation = "";
                    List<EmployeeEducation> oEdus = new List<EmployeeEducation>();
                    oEdus = oEmployeeEducations.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                    if (oEdus.Count > 0) { sEducation = oEdus[0].Degree; }

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = sEducation; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bView)
                    {
                        cell = sheet.Cells[rowIndex, 14];
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        List<EmployeeSalaryStructure> oESSs = new List<EmployeeSalaryStructure>();
                        oESSs = oEmployeeSalaryStructures.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                        double GrossAmount = 0;
                        if (oESSs.Count > 0) { GrossAmount = oESSs[0].GrossAmount; }
                        cell.Value = GrossAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    rowIndex++;
                }
                rowIndex++;
                nSL = 0;

                //}

                //}
                rowIndex++;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "No of Person"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                List<Employee> oEmps = new List<Employee>();
                var oBUIDs = _oEmployees.Select(p => p.BusinessUnitID).Distinct().ToList();
                foreach (var buid in oBUIDs)
                {
                    var oLocIDs = _oEmployees.Where(x => x.BusinessUnitID == buid).Select(x => x.LocationID).Distinct().ToList();
                    foreach (var locid in oLocIDs)
                    {
                        var oDepts = _oEmployees.Where(x => x.LocationID == locid).Select(x => x.DepartmentID).Distinct().ToList();
                        if (oDepts.Count > 0)
                        {

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = "BU Name : " + _oEmployees.Where(x => x.BusinessUnitID == buid).ToList().First().BUName + " , Location Name: " + _oEmployees.Where(x => x.LocationID == locid).ToList().First().LocationName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            rowIndex++;

                            foreach (var oDeptID in oDepts)
                            {
                                var tempSummery = _oEmployees.Where(x => x.DepartmentID == oDeptID && x.BusinessUnitID == buid && x.LocationID == locid).ToList();

                                cell = sheet.Cells[rowIndex, 3]; cell.Value = tempSummery.First().DepartmentName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, 4]; cell.Value = tempSummery.Count(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                rowIndex++;

                            }
                        }
                    }
                }

                //foreach (record oRec in showSummery)
                //{
                //    cell = sheet.Cells[rowIndex, 3]; cell.Value = oRec.DepartmentName; cell.Style.Font.Bold = false;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[rowIndex, 4]; cell.Value = oRec.NoOfEmployee; cell.Style.Font.Bold = false;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    rowIndex++;

                //}

                double nTotalEmp = showSummery.Sum(x => x.NoOfEmployee);
                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = nTotalEmp; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=NewEmployeeList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }
        public void ExcelFormat1Left(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            List<Department> _oDepartments = new List<Department>();
            List<EmployeeSettlement> _oEmployeeSettlements = new List<EmployeeSettlement>();
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }

            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + "))";

            }
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            sSql = "SELECT * FROM EmployeeSettlement WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            _oEmployee.EmployeeSettlements = EmployeeSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            sSql = "SELECT * FROM EmployeeSalaryStructure WHERE EmployeeID IN (SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            sSql = "SELECT * FROM EmployeeEducation WHERE PassingYear IN(SELECT MAX(PassingYear) FROM EmployeeEducation GROUP BY EmployeeID) AND EmployeeID IN (SELECT EmployeeID FROM VIEW_Employee WHERE JoiningDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeEducations = EmployeeEducation.Gets(sSql, (int)Session[SessionInfo.currentUserID]);


            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployee.Employees.Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", (int)Session[SessionInfo.currentUserID]); }


            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            int nMaxColumn = 7;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LEFTY EMPLOYEE List");
                sheet.Name = "LEFTY EMPLOYEE LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeName
                sheet.Column(4).Width = 20; //BU
                sheet.Column(5).Width = 20; //Loc
                sheet.Column(6).Width = 20; //Department
                sheet.Column(7).Width = 20; //Designation
                sheet.Column(8).Width = 20; //Education
                sheet.Column(9).Width = 20; //card
                sheet.Column(10).Width = 20; //DOB
                sheet.Column(11).Width = 20; //Joining Date
                sheet.Column(12).Width = 20; //Last Working Day
                if (bView) { sheet.Column(13).Width = 20; } //Last Gross

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 4]; cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 4]; cell.Value = "LEFTY EMPLOYEE LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "From " + sDate + " To " + eDate; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region Table Body
                int nSL = 0;


                _oDepartments = _oEmployee.Departments;
                _oEmployees = _oEmployee.Employees;
                _oEmployeeSettlements = _oEmployee.EmployeeSettlements;

                //foreach (Department oDepartmentItem in _oDepartments)
                //{

                //    List<Employee> oEmployees = (from oEs in _oEmployees

                //                                 where oEs.DepartmentID == oDepartmentItem.DepartmentID

                //                                 select oEs).ToList();

                //    if (oEmployees.Count > 0)
                //    {
                #region Table Header 02

                //cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Value = "SEC : " + oEmployees[0].DepartmentName; cell.Merge = true; cell.Style.Font.Bold = false;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //rowIndex++;
                //colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Business Unit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Education"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Card No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Working Day"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bView)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
                #endregion

                foreach (Employee oItem in _oEmployee.Employees)
                {
                    nSL++;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.BUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    string sEducation = "";
                    List<EmployeeEducation> oEdus = new List<EmployeeEducation>();
                    oEdus = oEmployeeEducations.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                    if (oEdus.Count > 0) { sEducation = oEdus[0].Degree; }

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = sEducation; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = _oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().EffectDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bView)
                    {
                        cell = sheet.Cells[rowIndex, 13];
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        List<EmployeeSalaryStructure> oESSs = new List<EmployeeSalaryStructure>();
                        oESSs = oEmployeeSalaryStructures.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                        double GrossAmount = 0;
                        if (oESSs.Count > 0) { GrossAmount = oESSs[0].GrossAmount; }

                        cell.Value = GrossAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    rowIndex++;
                }
                rowIndex++;
                nSL = 0;

                //    }
                //}

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LeftyEmployeeList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }
        public void ExcelFormat2Left(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            List<Department> _oDepartments = new List<Department>();
            List<EmployeeSettlement> _oEmployeeSettlements = new List<EmployeeSettlement>();
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }

            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + "))";
            }

            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            sSql = "SELECT * FROM EmployeeSettlement WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            _oEmployee.EmployeeSettlements = EmployeeSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            sSql = "SELECT * FROM EmployeeSalaryStructure WHERE EmployeeID IN (SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            sSql = "SELECT * FROM EmployeeEducation WHERE PassingYear IN(SELECT MAX(PassingYear) FROM EmployeeEducation GROUP BY EmployeeID) AND EmployeeID IN (SELECT EmployeeID FROM VIEW_Employee WHERE JoiningDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeEducations = EmployeeEducation.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployee.Employees.Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", (int)Session[SessionInfo.currentUserID]); }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            int nMaxColumn = 7;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LEFTY EMPLOYEE List");
                sheet.Name = "LEFTY EMPLOYEE LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeCode
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //BU
                sheet.Column(6).Width = 20; //Loc
                sheet.Column(7).Width = 20; //Department
                sheet.Column(8).Width = 20; //Designation
                sheet.Column(9).Width = 20; //Education
                sheet.Column(10).Width = 20; //DOB
                sheet.Column(11).Width = 20; //Joining Date
                sheet.Column(12).Width = 20; //Last Working Day
                if (bView) { sheet.Column(13).Width = 20; } //Last Gross

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header

                cell = sheet.Cells[rowIndex, 4]; cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "LEFTY EMPLOYEE LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "From " + sDate + " To " + eDate; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region Table Body
                int nSL = 0;

                _oDepartments = _oEmployee.Departments;
                _oEmployees = _oEmployee.Employees;
                _oEmployeeSettlements = _oEmployee.EmployeeSettlements;

                foreach (Department oDepartmentItem in _oDepartments)
                {

                    List<Employee> oEmployees = (from oEs in _oEmployees

                                                 where oEs.DepartmentID == oDepartmentItem.DepartmentID

                                                 select oEs).ToList();

                    if (oEmployees.Count > 0)
                    {
                        rec = new record();
                        rec.NoOfEmployee = oEmployees.Count();
                        rec.DepartmentName = oEmployees[0].DepartmentName;
                        showSummery.Add(rec);
                    }
                }
                //    if(oEmployees.Count > 0) {
                //        #region Table Header 02

                //        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Value = "SEC : " + oEmployees[0].DepartmentName; cell.Merge = true; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //        rowIndex++;
                //        colIndex = 2;

                #region Table Header 02
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Business Unit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Education"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Working Day"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bView)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
                #endregion

                foreach (Employee oItem in _oEmployee.Employees)
                {
                    nSL++;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.BUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    string sEducation = "";
                    List<EmployeeEducation> oEdus = new List<EmployeeEducation>();
                    oEdus = oEmployeeEducations.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                    if (oEdus.Count > 0) { sEducation = oEdus[0].Degree; }

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = sEducation; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = _oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().EffectDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bView)
                    {
                        cell = sheet.Cells[rowIndex, 13];
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        List<EmployeeSalaryStructure> oESSs = new List<EmployeeSalaryStructure>();
                        oESSs = oEmployeeSalaryStructures.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                        double GrossAmount = 0;
                        if (oESSs.Count > 0) { GrossAmount = oESSs[0].GrossAmount; }

                        cell.Value = GrossAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    rowIndex++;
                }
                rowIndex++;
                nSL = 0;

                //    }
                //}

                #endregion
                rowIndex++;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "No of Person"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;


                List<Employee> oEmps = new List<Employee>();
                var oBUIDs = _oEmployees.Select(p => p.BusinessUnitID).Distinct().ToList();
                foreach (var buid in oBUIDs)
                {
                    var oLocIDs = _oEmployees.Where(x => x.BusinessUnitID == buid).Select(x => x.LocationID).Distinct().ToList();
                    foreach (var locid in oLocIDs)
                    {
                        var oDepts = _oEmployees.Where(x => x.LocationID == locid).Select(x => x.DepartmentID).Distinct().ToList();
                        if (oDepts.Count > 0)
                        {

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = "BU Name : " + _oEmployees.Where(x => x.BusinessUnitID == buid).ToList().First().BUName + " , Location Name: " + _oEmployees.Where(x => x.LocationID == locid).ToList().First().LocationName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            rowIndex++;

                            foreach (var oDeptID in oDepts)
                            {
                                var tempSummery = _oEmployees.Where(x => x.DepartmentID == oDeptID && x.BusinessUnitID == buid && x.LocationID == locid).ToList();

                                cell = sheet.Cells[rowIndex, 3]; cell.Value = tempSummery.First().DepartmentName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, 4]; cell.Value = tempSummery.Count(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                rowIndex++;

                            }
                        }
                    }
                }

                double nTotalEmp = showSummery.Sum(x => x.NoOfEmployee);
                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = nTotalEmp; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #region Summary

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LeftyEmployeeList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }
        public void ExcelFormat3Left(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            List<Department> _oDepartments = new List<Department>();
            List<EmployeeActiveInactiveHistory> _oEmployeeActiveInactiveHistorys = new List<EmployeeActiveInactiveHistory>();
            _oEmployee = new Employee();
            //string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt + "' AND '" + dtTo + "')";

            string sSql = "SELECT * FROM View_Employee WHERE IsActive=0 AND EmployeeID IN (SELECT EmployeeID FROM EmployeeActiveInactiveHistory WHERE InactiveDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }
            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            sSql = "Select * from EmployeeActiveInactiveHistory Where EAIHID In (Select MAX(EAIHID) from EmployeeActiveInactiveHistory Where InactiveDate IS NOT NULL group by EmployeeID) AND InactiveDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            _oEmployee.EmployeeActiveInactiveHistorys = EmployeeActiveInactiveHistory.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            sSql = "SELECT * FROM EmployeeSalaryStructure WHERE EmployeeID IN (SELECT EmployeeID FROM EmployeeActiveInactiveHistory WHERE InactiveDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            sSql = "SELECT * FROM EmployeeEducation WHERE PassingYear IN(SELECT MAX(PassingYear) FROM EmployeeEducation GROUP BY EmployeeID) AND EmployeeID IN (SELECT EmployeeID FROM VIEW_Employee WHERE JoiningDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeEducations = EmployeeEducation.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            int nMaxColumn = 7;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LEFTY EMPLOYEE List");
                sheet.Name = "LEFTY EMPLOYEE LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeCode
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //Department
                sheet.Column(6).Width = 20; //Designation
                sheet.Column(7).Width = 20; //Education
                sheet.Column(8).Width = 20; //DOB
                sheet.Column(9).Width = 20; //Joining Date
                sheet.Column(10).Width = 20; //Last Working Day
                if (bView) { sheet.Column(11).Width = 20; } //Last Gross

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header

                cell = sheet.Cells[rowIndex, 4]; cell.Value = _oEmployee.Employees.Count > 0 ? _oEmployee.Employees[0].BUName : oCompany.Name; ; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "LEFTY EMPLOYEE LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "From " + sDate + " To " + eDate; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region Table Body
                int nSL = 0;

                _oDepartments = _oEmployee.Departments;
                _oEmployees = _oEmployee.Employees;
                _oEmployeeActiveInactiveHistorys = _oEmployee.EmployeeActiveInactiveHistorys;

                foreach (Department oDepartmentItem in _oDepartments)
                {

                    List<Employee> oEmployees = (from oEs in _oEmployees

                                                 where oEs.DepartmentID == oDepartmentItem.DepartmentID

                                                 select oEs).ToList();

                    if (oEmployees.Count > 0)
                    {
                        recInactive = new recordInactive();
                        recInactive.NoOfEmployee = oEmployees.Count();
                        recInactive.DepartmentName = oEmployees[0].DepartmentName;
                        showSummeryInactive.Add(recInactive);
                    }
                }
                //if (oEmployees.Count > 0)
                //{
                //    #region Table Header 02

                //    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Value = "SEC : " + oEmployees[0].DepartmentName; cell.Merge = true; cell.Style.Font.Bold = false;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //    rowIndex++;
                //    colIndex = 2;
                #region Table Header 02
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Education"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Inactive Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bView)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                rowIndex++;
                #endregion

                foreach (Employee oItem in _oEmployee.Employees)
                {
                    nSL++;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    string sEducation = "";
                    List<EmployeeEducation> oEdus = new List<EmployeeEducation>();
                    oEdus = oEmployeeEducations.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                    if (oEdus.Count > 0) { sEducation = oEdus[0].Degree; }

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = sEducation; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (_oEmployeeActiveInactiveHistorys.Any(x => x.EmployeeID == oItem.EmployeeID))
                    {
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = _oEmployeeActiveInactiveHistorys.Where(x => x.EmployeeID == oItem.EmployeeID).First().InactiveDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }

                    if (bView)
                    {
                        List<EmployeeSalaryStructure> oESSs = new List<EmployeeSalaryStructure>();
                        oESSs = oEmployeeSalaryStructures.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();
                        double nGrossAmount = 0;
                        if (oESSs.Count > 0) { nGrossAmount = oESSs.First().GrossAmount; }

                        cell = sheet.Cells[rowIndex, 11];
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Value = nGrossAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    rowIndex++;
                }
                rowIndex++;
                nSL = 0;

                //}
                //}

                //#endregion
                rowIndex++;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "No of Person"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;


                List<Employee> oEmps = new List<Employee>();
                var oBUIDs = _oEmployees.Select(p => p.BusinessUnitID).Distinct().ToList();
                foreach (var buid in oBUIDs)
                {
                    var oLocIDs = _oEmployees.Where(x => x.BusinessUnitID == buid).Select(x => x.LocationID).Distinct().ToList();
                    foreach (var locid in oLocIDs)
                    {
                        var oDepts = _oEmployees.Where(x => x.LocationID == locid).Select(x => x.DepartmentID).Distinct().ToList();
                        if (oDepts.Count > 0)
                        {

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = "BU Name : " + _oEmployees.Where(x => x.BusinessUnitID == buid).ToList().First().BUName + " , Location Name: " + _oEmployees.Where(x => x.LocationID == locid).ToList().First().LocationName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            rowIndex++;

                            foreach (var oDeptID in oDepts)
                            {
                                var tempSummery = _oEmployees.Where(x => x.DepartmentID == oDeptID && x.BusinessUnitID == buid && x.LocationID == locid).ToList();

                                cell = sheet.Cells[rowIndex, 3]; cell.Value = tempSummery.First().DepartmentName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, 4]; cell.Value = tempSummery.Count(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                rowIndex++;

                            }
                        }
                    }
                }

                double nTotalEmp = showSummeryInactive.Sum(x => x.NoOfEmployee);
                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = nTotalEmp; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #region Summary



                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LeftyEmployeeList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

                #endregion



        public void ExcelFormat4Left(string sDate, string eDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, double ts)
        {
            DateTime dt = Convert.ToDateTime(sDate);
            DateTime dtTo = Convert.ToDateTime(eDate);
            List<Department> _oDepartments = new List<Department>();
            List<EmployeeSettlement> _oEmployeeSettlements = new List<EmployeeSettlement>();
            _oEmployee = new Employee();
            string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            if (sBusinessUnitIds != "")
            {
                sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (sLocationID != "")
            {
                sSql = sSql + " AND LocationID IN(" + sLocationID + ")";
            }
            if (sDepartmentIds != "")
            {
                sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (sDesignationIds != "")
            {
                sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")";
            }

            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + "))";
            }

            _oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Departments = Department.Gets((int)Session[SessionInfo.currentUserID]);

            sSql = "SELECT * FROM EmployeeSettlement WHERE EmployeeID IN(SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            _oEmployee.EmployeeSettlements = EmployeeSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            sSql = "SELECT * FROM EmployeeSalaryStructure WHERE EmployeeID IN (SELECT EmployeeID from EmployeeSettlement WHERE EffectDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            sSql = "SELECT * FROM EmployeeEducation WHERE PassingYear IN(SELECT MAX(PassingYear) FROM EmployeeEducation GROUP BY EmployeeID) AND EmployeeID IN (SELECT EmployeeID FROM VIEW_Employee WHERE JoiningDate BETWEEN '" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "')";
            oEmployeeEducations = EmployeeEducation.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sDate + "~" + eDate;
            _oEmployee.Company = oCompanys.First();
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            int nMaxColumn = 17;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LEFTY EMPLOYEE List");
                sheet.Name = "LEFTY EMPLOYEE LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //Code
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //Department
                sheet.Column(6).Width = 20; //Designation
                sheet.Column(7).Width = 20; //Employee Category
                sheet.Column(8).Width = 20; //Gender
                sheet.Column(9).Width = 20; //DOB
                sheet.Column(10).Width = 20; //Joining Date
                sheet.Column(11).Width = 20; //Confirmation Date
                sheet.Column(12).Width = 20; //Settlement submission date
                sheet.Column(13).Width = 20; //Last working date
                sheet.Column(14).Width = 20; //settlement type
                sheet.Column(15).Width = 20; //Service Year
                sheet.Column(16).Width = 20; //Reason
                sheet.Column(17).Width = 20; //Notice Pay Status
                sheet.Column(18).Width = 20; //Gratuity Status

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header

                cell = sheet.Cells[rowIndex, 4]; cell.Value = _oEmployee.Employees.Count > 0 ? _oEmployee.Employees[0].BUName : oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "LEFTY EMPLOYEE LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "From " + sDate + " To " + eDate; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region Table Body
                int nSL = 0;

                _oDepartments = _oEmployee.Departments;
                _oEmployees = _oEmployee.Employees;
                _oEmployeeSettlements = _oEmployee.EmployeeSettlements;

                //    if(oEmployees.Count > 0) {
                //        #region Table Header 02

                //        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Value = "SEC : " + oEmployees[0].DepartmentName; cell.Merge = true; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //        rowIndex++;
                //        colIndex = 2;

                #region Table Header 02
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Category"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gender"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Confirmation Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sett. Submission Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Working Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Settlement Type"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reason"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Notice Pay Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gratuity Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                foreach (Employee oItem in _oEmployee.Employees)
                {
                    nSL++;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    //EmployeeCategory
                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.EmployeeCategoryInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.Gender; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.ConfirmationDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = _oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().EffectDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = _oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().SubmissionDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = _oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().SettlementTypeInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Service Year

                    DateTime TempLastWorkingDate = _oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().EffectDate;
                    TempLastWorkingDate = TempLastWorkingDate.AddDays(1);

                    DateDifference ServiceYear = new DateDifference(oItem.DateOfJoin, TempLastWorkingDate);

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = ServiceYear.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = _oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().Reason; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = ((_oEmployeeSettlements.Where(x => x.EmployeeID == oItem.EmployeeID).First().IsNoticePayDeduction) ? "Yes" : "No"); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Gratuity
                    cell = sheet.Cells[rowIndex, 18]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex++;
                }
                rowIndex++;
                nSL = 0;

                //    }
                //}

                #endregion
                rowIndex++;
                rowIndex++;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LeftyEmployeeList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }



        [HttpGet]
        public JsonResult GetsforPOP()
        {
            List<Employee> oEmployees = new List<Employee>();
            Employee oEmployee = new Employee();
            oEmployee.Name = "-- Select Employee Name --";
            oEmployees.Add(oEmployee);
            oEmployees.AddRange(Employee.GetsforPOP((int)Session[SessionInfo.currentUserID]));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View_PrintJobApplication(int nEmpID, string sLanguage, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();


            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM View_Employee WHERE EmployeeID=" + nEmpID + ")", (int)Session[SessionInfo.currentUserID]);
            _oEmployee.BusinessUnit = oBusinessUnits.Any() ? oBusinessUnits[0] : oBusinessUnit;


            return PartialView(_oEmployee);
        }

        public ActionResult View_PrintJobApplication_ZN(int nEmpID, string sLanguage, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();


            return PartialView(_oEmployee);
        }

        public Image GetCompanyLogoForHTMLPrint(int nid)//nid is employeeID which is doing nothing.
        {
            Company oCompany = new Company();
            oCompany = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);
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

        public Image GetAuthSignatureHTMLPrint(int nid)//nid is employeeID which is doing nothing.
        {
            Company oCompany = new Company();
            oCompany = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);

            if (oCompany.AuthorizedSignature != null)
            {
                MemoryStream m = new MemoryStream(oCompany.AuthorizedSignature);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public ActionResult View_EmployeePayrollPicker()
        {
            _oEmployee = new Employee();
            _oEmployee.EmployeeTypes = EmployeeType.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.SalarySchemes = SalaryScheme.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(_oEmployee);
        }

        [HttpPost]
        public JsonResult SearchEmployeePayroll(string sParam)
        {
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            string sName = Convert.ToString(sParam.Split('~')[0]).TrimStart(' ');
            string sCode = Convert.ToString(sParam.Split('~')[1]).TrimStart(' ');
            string sEnrollNo = Convert.ToString(sParam.Split('~')[2]).TrimStart(' ');
            string sASID = Convert.ToString(sParam.Split('~')[3]);
            string slocationID = Convert.ToString(sParam.Split('~')[4]);
            string sdepartmentID = Convert.ToString(sParam.Split('~')[5]);
            string sdesignationID = Convert.ToString(sParam.Split('~')[6]);
            string sGender = Convert.ToString(sParam.Split('~')[7]);
            int nEmployeeType = Convert.ToInt32(sParam.Split('~')[8]);
            int nShift = Convert.ToInt32(sParam.Split('~')[9]);
            int nIsActive = Convert.ToInt32(sParam.Split('~')[10]);
            int nIsInActive = Convert.ToInt32(sParam.Split('~')[11]);
            int nbIsUser = Convert.ToInt32(sParam.Split('~')[12]);
            int nSalarySchemeID = Convert.ToInt32(sParam.Split('~')[13]);
            int nRowLength = Convert.ToInt32(sParam.Split('~')[14]);
            int nLoadRecord = Convert.ToInt32(sParam.Split('~')[15]);

            //string sSQL = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID<>0";
            string sSQL = "SELECT top(" + nLoadRecord + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeID) Row,* FROM View_EmployeeSalaryStructure WHERE EmployeeID<>0 ";


            if (sName != "" && sName != " ")
            {
                sSQL = sSQL + " AND EmployeeId IN(SELECT EmployeeID FROM Employee WHERE Name  like '" + "%" + sName + "%" + "')";
            }
            if (sCode != " " && sCode != "")
            {
                sSQL = sSQL + " AND EmployeeId IN(SELECT EmployeeID FROM Employee WHERE Code  like '" + "%" + sCode + "%" + "')";
            }
            if (sEnrollNo != " " && sCode != "")
            {
                sSQL = sSQL + " AND EmployeeID IN (SELECT EmployeeID FROM EmployeeAuthentication WHERE IsActive=1 AND CardNo like'" + "%" + sEnrollNo + "%" + "')";
            }

            if (sGender != "None")
            {
                sSQL = sSQL + " AND EmployeeId IN(SELECT EmployeeID FROM Employee WHERE Gender='" + sGender + "')";
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
                sSQL = sSQL + "AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE AttendanceSchemeID=" + sASID + ")";
            }
            if (slocationID != "")
            {
                sSQL = sSQL + "AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE LocationID=" + slocationID + ")";

            }
            if (sdepartmentID != "")
            {
                sSQL = sSQL + "AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE DepartmentID=" + sdepartmentID + ")";

            }
            if (sdesignationID != "")
            {
                sSQL = sSQL + "AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE DesignationID=" + sdesignationID + ")";

            }
            if (nEmployeeType > 0)
            {
                sSQL = sSQL + "AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE EmployeeTypeID=" + nEmployeeType + ")";

            }

            if (nShift > 0)
            {
                sSQL = sSQL + "AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE CurrentShiftID=" + nShift + ")";

            }

            if (nSalarySchemeID > 0)
            {
                sSQL = sSQL + "AND SalarySchemeID=" + nSalarySchemeID;

            }
            sSQL = sSQL + ") aa WHERE Row >" + nRowLength;

            try
            {
                oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oEmployeeSalaryStructures[0] = new EmployeeSalaryStructure();
                oEmployeeSalaryStructures[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSalaryStructures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View_EmployeePhoto(int nId, string sMsg, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee.EmployeeID = nId;
            string sSql = "select * from View_Employee_WithImage WHERE EmployeeID =" + nId;
            _oEmployee = Employee.Get(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.ErrorMessage = sMsg;
            return PartialView(_oEmployee);
        }

        [HttpPost]
        public ActionResult EmployeeImageIU(HttpPostedFileBase file1, Employee oEmp)
        {
            // Verify that the user selected a file
            string sErrorMessage = "";
            try
            {
                #region Photo Image
                if (file1 != null && file1.ContentLength > 0)
                {
                    System.Drawing.Image oPhotoImage = System.Drawing.Image.FromStream(file1.InputStream, true, true);

                    //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                    //Orginal Image to byte array
                    byte[] aPhotoImageInByteArray = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oPhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        aPhotoImageInByteArray = ms.ToArray();
                    }

                    #region Image Size Validation
                    //double nMaxLength = 100 * 1024;
                    //if (aPhotoImageInByteArray.Length > nMaxLength)
                    //{
                    //    sErrorMessage = "Youe Photo Image " + aPhotoImageInByteArray.Length + "KB! You can selecte maximum 100KB image";
                    //    return RedirectToAction("ViewContactPersonnel", new { cid = oContactPersonnel.ContractorID, id = oContactPersonnel.ContactPersonnelID, ms = sErrorMessage });
                    //}                
                    //else
                    //{
                    //    oContactPersonnel.Photo = aPhotoImageInByteArray;
                    //}
                    #endregion
                    oEmp.Photo = aPhotoImageInByteArray;
                }
                #endregion


                _oEmployee = oEmp;
                _oEmployee.ErrorMessage = _oEmployee.EmployeeImageIU((int)Session[SessionInfo.currentUserID]);


            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            return RedirectToAction("View_EmployeePhoto", new { nId = _oEmployee.EmployeeID, sMsg = _oEmployee.ErrorMessage, ts = 0 });

        }

        public ActionResult PrintEmpAuthenticationXL(string sEmployeeIDs, double ts)
        {
            List<Employee> oEmployees = new List<Employee>();
            List<EmployeeAuthentication> oEmployeeAuthentications = new List<EmployeeAuthentication>();
            string sSql = "SELECT * FROM VIEW_Employee WHERE EmployeeID IN(" + sEmployeeIDs + ") ORDER BY Code";
            string sSql1 = "SELECT * FROM EmployeeAuthentication WHERE EmployeeID IN(" + sEmployeeIDs + ")";

            oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            oEmployeeAuthentications = EmployeeAuthentication.Gets(sSql1, (int)Session[SessionInfo.currentUserID]);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<EmpAuthenticationXL>));

            EmpAuthenticationXL oEAXL = new EmpAuthenticationXL();
            List<EmpAuthenticationXL> oEAXLs = new List<EmpAuthenticationXL>();

            int nCount = 0;
            foreach (Employee oItem in oEmployees)
            {
                nCount++;
                oEAXL = new EmpAuthenticationXL();
                oEAXL.SL = nCount.ToString();
                oEAXL.Code = oItem.Code;
                oEAXL.Name = oItem.Name;
                oEAXL.ContactNo = oItem.ContactNo;
                oEAXL.DesignationName = oItem.DesignationName;
                oEAXL.DepartmentName = oItem.DepartmentName;
                oEAXL.CurrentShift = oItem.CurrentShift;

                List<EmployeeAuthentication> oEAuths = (from oEAuth in oEmployeeAuthentications
                                                        where oItem.EmployeeID == oEAuth.EmployeeID
                                                        select oEAuth).ToList();
                if (oEAuths.Count > 0)
                { oEAXL.CardNo = oEAuths[0].CardNo; }
                else { oEAXL.CardNo = ""; }
                oEAXLs.Add(oEAXL);
            }

            //serializer.Serialize(stream, oDTESs);
            serializer.Serialize(stream, oEAXLs);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "EmplyeeAuthentication.xls");
        }
        public ActionResult View_PrintCompanyRules(int nEmpID, string sCRNIDs, double ts)
        {
            CompanyRuleName oCompanyRuleName = new CompanyRuleName();
            List<CompanyRuleDescription> oCompanyRuleDescriptions = new List<CompanyRuleDescription>();
            oCompanyRuleName.CompanyRuleNames = CompanyRuleName.Gets("SELECT * FROM CompanyRuleName WHERE IsActive=1 AND CRNID IN(" + sCRNIDs + ")", (int)Session[SessionInfo.currentUserID]);
            oCompanyRuleName.CompanyRuleDescriptions = CompanyRuleDescription.Gets("SELECT * FROM View_CompanyRuleDescription WHERE IsActive=1 AND CRNID IN(" + sCRNIDs + ")", (int)Session[SessionInfo.currentUserID]);
            oCompanyRuleName.Employee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            oCompanyRuleName.Company = oCompanys.First();
            return PartialView(oCompanyRuleName);
        }

        public ActionResult View_PrintDeclaration(int nEmpID, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeNominees = EmployeeNominee.Gets(nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM View_Employee WHERE EmployeeID=" + nEmpID + ")", (int)Session[SessionInfo.currentUserID]);
            _oEmployee.BusinessUnit = oBusinessUnits.Any() ? oBusinessUnits[0] : oBusinessUnit;

            return PartialView(_oEmployee);
        }

        public Image GetEmpPhotoForHTMLPrint(int nid)//EmpID.
        {
            _oEmployee = new Employee();
            string sSql = "select * from View_Employee_WithImage WHERE EmployeeID =" + nid;
            _oEmployee = Employee.Get(sSql, (int)Session[SessionInfo.currentUserID]);

            if (_oEmployee.Photo != null)
            {
                MemoryStream m = new MemoryStream(_oEmployee.Photo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }

            //return GetImage(_oEmployee.Photo);
        }

        #region  MAMIYA
        public ActionResult PrintAppointmentLetter_MAMIYA(int nEmpID, string itemIDs, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            string sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID + ") AND SalaryHeadType=1 ";
            _oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.EmployeeSalaryStructure = EmployeeSalaryStructure.Get("SELECT * FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID, (int)Session[SessionInfo.currentUserID]);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();
            _oEmployee.ErrorMessage = itemIDs;

            rptAppointmentLetter_MAMIYA oReport = new rptAppointmentLetter_MAMIYA();
            byte[] abytes = oReport.PrepareReport(_oEmployee);
            return File(abytes, "application/pdf");
        }

        public ActionResult View_PrintAppointmentLetterInBangla_MAMIYA(int nEmpID, string itemIDs, double ts)
        {
            _oEmployee = new Employee();
            _oEmployee = _oEmployee.Get(nEmpID, (int)Session[SessionInfo.currentUserID]);
            string sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID + ") AND SalaryHeadType=1 ";
            _oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]).OrderBy(x => x.SalaryHeadID).ToList();
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company = oCompanys.First();
            if (itemIDs != "")
            {
                _oEmployee.ErrorMessage = itemIDs;
            }
            return PartialView(_oEmployee);
        }

        public ActionResult PrintEmpDetailList(string sEmpIDs, double ts)
        {
            Employee oEmployee = new Employee();
            string sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN(" + sEmpIDs + ")";
            oEmployee.Employees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]).OrderBy(x => x.Code).ToList();

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            oEmployee.Company = oCompanys.First();

            rptEmpDetailList oReport = new rptEmpDetailList();
            byte[] abytes = oReport.PrepareReport(oEmployee);
            return File(abytes, "application/pdf");
        }

        #endregion  MAMIYA

        #region Import & Export
        private List<Employee_UploadXL> GetEmployeeFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<Employee_UploadXL> oEXLs = new List<Employee_UploadXL>();
            Employee_UploadXL oEXL = new Employee_UploadXL();
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
                    DateTime dtDate = DateTime.Now;
                    Int16 nUser = 0;
                    for (int i = 0; i < oRows.Count; i++)
                    {
                        oEXL = new Employee_UploadXL();
                        oEXL.Code = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        if (oEXL.Code != "")
                        {
                            oEXL.Name = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            oEXL.FathersName = Convert.ToString(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                            oEXL.MothersName = Convert.ToString(oRows[i][3] == DBNull.Value ? "" : oRows[i][3]);
                            oEXL.Address = Convert.ToString(oRows[i][4] == DBNull.Value ? "" : oRows[i][4]);
                            //oEXL.DateOfBirth = Convert.ToDateTime(oRows[i][5] == DBNull.Value ? DateTime.Today : oRows[i][5]);
                            oRows[i][5] = oRows[i][5] ?? "";
                            oEXL.DateOfBirth = (DateTime.TryParse(oRows[i][5].ToString(), out dtDate) ? dtDate : DateTime.Now);
                            oEXL.Gender = Convert.ToString(oRows[i][6] == DBNull.Value ? "" : oRows[i][6]);
                            oEXL.LocationCode = Convert.ToString(oRows[i][7] == DBNull.Value ? "" : oRows[i][7]);
                            oEXL.AttScheme = Convert.ToString(oRows[i][8] == DBNull.Value ? "" : oRows[i][8]);
                            oEXL.DepartmentCode = Convert.ToString(oRows[i][9] == DBNull.Value ? "" : oRows[i][9]);
                            oEXL.DesignationCode = Convert.ToString(oRows[i][10] == DBNull.Value ? "" : oRows[i][10]);
                            oRows[i][11] = oRows[i][11] ?? "";
                            oEXL.DateOfJoin = (DateTime.TryParse(oRows[i][11].ToString(), out dtDate) ? dtDate : DateTime.Now);
                            oRows[i][12] = oRows[i][12] ?? "";
                            oEXL.DateOfConfirmation = (DateTime.TryParse(oRows[i][12].ToString(), out dtDate) ? dtDate : DateTime.Now);
                            //oEXL.DateOfConfirmation = Convert.ToDateTime(oRows[i][12] == DBNull.Value ? DateTime.Today : oRows[i][12]);
                            oEXL.ShiftCode = Convert.ToString(oRows[i][13] == DBNull.Value ? "" : oRows[i][13]);
                            oEXL.SalaryScheme = Convert.ToString(oRows[i][14] == DBNull.Value ? "" : oRows[i][14]);
                            oEXL.GrossSalary = Convert.ToDouble(oRows[i][15] == DBNull.Value ? 0 : oRows[i][15]);
                            oEXL.ProximityCardNo = Convert.ToString(oRows[i][16] == DBNull.Value ? "" : oRows[i][16]);
                            oEXL.BankCode = Convert.ToString(oRows[i][17] == DBNull.Value ? "" : oRows[i][17]);
                            oEXL.ACNo = Convert.ToString(oRows[i][18] == DBNull.Value ? "" : oRows[i][18]);

                            //oRows[i][19] = oRows[i][19] ?? 0;
                            //oEXL.IsUser = (Int16.TryParse( oRows[i][19], out nUser) ? nUser : 0);

                            oEXL.IsUser = Convert.ToInt16(oRows[i][19] == DBNull.Value ? 0 : oRows[i][19]);
                            oEXL.Category = Convert.ToString(oRows[i][20] == DBNull.Value ? "" : oRows[i][20]);
                            oEXL.BUCode = Convert.ToString(oRows[i][21] == DBNull.Value ? "" : oRows[i][21]);

                            double BankAmount = Convert.ToDouble(oRows[i][22] == DBNull.Value ? 0 : oRows[i][22]);
                            double CashAmount = Convert.ToDouble(oRows[i][23] == DBNull.Value ? 0 : oRows[i][23]);
                            if (BankAmount > 0)
                            {
                                oEXL.IsCashFixed = false;
                                oEXL.CashAmount = BankAmount;
                            }
                            else if (CashAmount > 0)
                            {
                                oEXL.IsCashFixed = true;
                                oEXL.CashAmount = CashAmount;
                            }

                            oEXLs.Add(oEXL);
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
            return oEXLs;
        }

        [HttpPost]
        public ActionResult View_EmployeeInformations(HttpPostedFileBase fileEmployees)
        {
            List<Employee_UploadXL> oEXLs = new List<Employee_UploadXL>();
            Employee_UploadXL oEXL = new Employee_UploadXL();
            List<Employee> oEmps = new List<Employee>();

            try
            {
                if (fileEmployees == null) { throw new Exception("File not Found"); }
                oEXLs = this.GetEmployeeFromExcel(fileEmployees);
                oEmps = Employee.UploadXL(oEXLs, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oEmps.Count > 0)
                {
                    ExcelRange cell;
                    OfficeOpenXml.Style.Border border;
                    ExcelFill fill;
                    int colIndex = 1;
                    int rowIndex = 2;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Error List");
                        sheet.Name = "Error List";

                        int n = 1;
                        sheet.Column(n++).Width = 13;
                        sheet.Column(n++).Width = 50;

                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowIndex += 1;

                        foreach (Employee oItem in oEmps)
                        {
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                        }

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                }

                //if (oEmps.Count > 0)
                //{
                //    oEmps[0].ErrorMessage = "Uploaded Successfully!";
                //}
                //else
                //{
                //    oEmps = new List<Employee>();
                //    Employee oEmp = new Employee();
                //    oEmp.ErrorMessage = "nothing to upload or these employees alraedy uploaded!";
                //    oEmps.Add(oEmp);
                //}

                ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);
                ViewBag.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
                ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View(oEmps);
            }
            return View(oEmps);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }


        #endregion Import & Export

        #region TIN
        [HttpPost]
        public JsonResult ETINIU(EmployeeTINInformation oTIN)
        {
            EmployeeTINInformation oEmpTIN = new EmployeeTINInformation();
            try
            {
                oEmpTIN = oTIN;
                //oEmpTIN.TaxArea = (EnumTaxArea)oEmpTIN.TaxAreaInint;
                if (oEmpTIN.ETINID > 0)
                {
                    oEmpTIN = oEmpTIN.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else { oEmpTIN = oEmpTIN.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]); }
            }
            catch (Exception ex)
            {
                oEmpTIN = new EmployeeTINInformation();
                oEmpTIN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpTIN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion TIN

        #region Employee_Picker_With_Official_And_Salary_Staructure
        [HttpPost]
        public JsonResult GetsEmployee_With_Official_And_Salary_Staructure(Employee oEmployee)
        {
            _oEmployees = new List<Employee>();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE IsActive=1  AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE IsActive=1) AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE IsActive=1)";
                if (oEmployee.Name.Trim() != "") { sSql += " AND Code LIKE '%" + oEmployee.Name.Trim() + "%' OR Name LIKE '%" + oEmployee.Name.Trim() + "%'"; }
                if (oEmployee.DepartmentID > 0) { sSql += " AND DepartmentID =" + oEmployee.DepartmentID; }
                _oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("There is no employee found with official & salary structure!");
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
                _oEmployees.Add(_oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Employee_Picker_With_Official_And_Salary_Staructure

        #region Edit date of join
        [HttpPost]
        public JsonResult EditDateOfJoin(Employee oEMP)
        {
            _oEmployee = new Employee();
            _oEmployee = oEMP;
            try
            {
                _oEmployee = _oEmployee.EditDateOfJoin((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Edit date of join

        [HttpPost]
        public JsonResult EditBankCash(EmployeeSalaryStructure oESS)
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            _oEmployeeSalaryStructure = oESS;
            try
            {
                _oEmployeeSalaryStructure = _oEmployeeSalaryStructure.EditBankCash((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Employee Database XL
        public void EmployeeDatabaseXL(string sParam)
        {
            EmployeeDatabase oEmployeeDatabase = new EmployeeDatabase();
            List<EmployeeDatabase> oEmployeeDatabases = new List<EmployeeDatabase>();
            List<EmployeeNominee> oEmployeeNominees = new List<EmployeeNominee>();
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();


            bool bIsComp = false;
            bIsComp = Convert.ToBoolean(sParam.Split('~')[37]); // it is in this index in view;

            oEmployeeDatabases = EmployeeDatabase.Gets(sParam, (int)Session[SessionInfo.currentUserID]);
            string EmpIDs = string.Join(",", oEmployeeDatabases.Select(x => x.EmployeeID));
            if (oEmployeeDatabases.Count > 0)
            {
                string sSql = "SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID IN(" + EmpIDs + "))";
                oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }

            //List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            //oAUOEDOs = AuthorizationUserOEDO.GetsByUser((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            //bool bView = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._View, "EmployeeDatabase", oAUOEDOs);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

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
                var sheet = excelPackage.Workbook.Worksheets.Add("EMPLOYEE");
                sheet.Name = "EMPLOYEE";

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 15; //CODE
                sheet.Column(4).Width = 18; //NAME
                sheet.Column(5).Width = 18; //Card No
                sheet.Column(6).Width = 18; //FatherName
                sheet.Column(7).Width = 18; //MaotheName
                sheet.Column(8).Width = 18; //Present Address
                sheet.Column(9).Width = 18; //Permanent Address


                sheet.Column(10).Width = 18; //Thana
                sheet.Column(11).Width = 18; //District
                sheet.Column(12).Width = 18; //Village
                sheet.Column(13).Width = 18; //PostOffice
                sheet.Column(14).Width = 18; //PostCode

                sheet.Column(15).Width = 18; //Contact No
                sheet.Column(16).Width = 18; //DateOfBirth


                sheet.Column(17).Width = 18; //Email
                sheet.Column(18).Width = 18; //NationalID
                sheet.Column(19).Width = 18; //BirthRegID

                sheet.Column(20).Width = 18; //Employee Type
                sheet.Column(21).Width = 18; //Employee Category
                sheet.Column(22).Width = 18; //Gender
                sheet.Column(23).Width = 18; //B Group
                sheet.Column(24).Width = 18; //religion
                sheet.Column(25).Width = 18; //Location
                sheet.Column(26).Width = 18; //BU Name
                sheet.Column(27).Width = 18; //Department
                sheet.Column(28).Width = 18; //Designation

                sheet.Column(29).Width = 18; //Group
                sheet.Column(30).Width = 18; //Block

                sheet.Column(31).Width = 18; //DateOfJoin
                sheet.Column(32).Width = 18; //DateOfconf
                sheet.Column(33).Width = 18; //Reporting Person
                sheet.Column(34).Width = 18; //S. Year
                sheet.Column(35).Width = 18; //Age
                sheet.Column(36).Width = 18; //Last Edu

                sheet.Column(37).Width = 18; //AttendanceScheme
                sheet.Column(38).Width = 18; //Shift
                sheet.Column(39).Width = 18; //salaryScheme


                int n = 41;

                List<EmployeeSalaryStructureDetail> oESSDs_SH = new List<EmployeeSalaryStructureDetail>();
                if (bView)
                {
                    sheet.Column(n).Width = 18; //Gross
                    oESSDs_SH = oEmployeeSalaryStructureDetails.GroupBy(x => x.SalaryHeadID).Select(grp => grp.First()).OrderBy(x => x.SalaryHeadID).ToList();

                    foreach (EmployeeSalaryStructureDetail oitem in oESSDs_SH)
                    {
                        sheet.Column(n).Width = 18; //SH
                        n++;
                    }
                }

                sheet.Column(n++).Width = 18; //BankName
                sheet.Column(n++).Width = 18; //AccountNo
                sheet.Column(n++).Width = 18; //E-TIN Number
                sheet.Column(n++).Width = 18; //PFScheme
                sheet.Column(n++).Width = 18; //PF Member Date
                sheet.Column(n++).Width = 18; //PIScheme
                sheet.Column(n++).Width = 18; //TAXPerMonth
                sheet.Column(n++).Width = 18; //Loan

                sheet.Column(n++).Width = 18; //Nominee Name
                sheet.Column(n++).Width = 18; //Nominee Contact
                sheet.Column(n++).Width = 18; //Relation

                sheet.Column(n++).Width = 18; //BankAmount
                sheet.Column(n++).Width = 18; //CashAmount

                nMaxColumn = n;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "EMPLOYEE DATABASE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Card No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Father Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mother Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Present Address"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Permanent Address"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Village"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Post Office"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Thana"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "District"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Post Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Contact No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Email"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "National ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Birth Reg. ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Type"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Category"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gender"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "B. Group"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Religion"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BU Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Group Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Block Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Join"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Conf"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reporting Person"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Age"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Degree"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att. Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "salary Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bView)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    foreach (EmployeeSalaryStructureDetail oItem in oESSDs_SH)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                }
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Account No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "E-TIN Number"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF Member Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PI Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TAX Per Month"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Loan Balance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Nominee Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Nominee Contact"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Nominee Relation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Cash Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                #endregion

                #region Table Body

                int nSL = 0;
                foreach (EmployeeDatabase oItem in oEmployeeDatabases)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    int Code = 0;
                    //if (int.TryParse(oItem.EmployeeCode, out number))
                    //{ cell.Style.Numberformat.Format = "0"; }
                    //if (int.TryParse(oItem.EmployeeCode, out Code))
                    //{
                    //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Code; cell.Style.Font.Bold = false;
                    //    cell.Style.Numberformat.Format = "0";
                    //}
                    //else
                    //{
                    //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;

                    //}
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CardNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FatherName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MotherName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PresentAddress; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PermanentAddress; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Village; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PostOffice; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Thana; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.District; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PostCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContactNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Email; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NationalID; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BirthID; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeType; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCategoryInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Gender; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BloodGroup; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Religion; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BUName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GroupName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BlockName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfConfInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReportingPerson; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateTime TempLastWorkingDate = oItem.LastWorkingDate;
                    TempLastWorkingDate = TempLastWorkingDate.AddDays(1);

                    DateDifference ServiceYear = new DateDifference(oItem.DateOfJoin, TempLastWorkingDate);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ServiceYear.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateDifference Age = new DateDifference(oItem.DateOfBirth, DateTime.Now.AddDays(1));
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Age.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LastEducationDegree; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AttendanceScheme; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShiftName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.salaryScheme; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bView)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (bIsComp) ? Global.MillionFormat(oItem.CompGross) : Global.MillionFormat(oItem.Gross); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        foreach (EmployeeSalaryStructureDetail oDetailItem in oESSDs_SH)
                        {
                            List<EmployeeSalaryStructureDetail> oESSDs = new List<EmployeeSalaryStructureDetail>();
                            oESSDs = oEmployeeSalaryStructureDetails.Where(x => x.ESSID == oItem.EmployeeStructureID && x.SalaryHeadID == oDetailItem.SalaryHeadID).ToList();

                            if (oESSDs.Count > 0)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (bIsComp) ? Global.MillionFormat(oESSDs[0].CompAmount) : Global.MillionFormat(oESSDs[0].Amount); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(0.0); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                        }
                    }
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AccountNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ETINNumber; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PFScheme; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PFMemberDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PIScheme; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.TAXPerMonth); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.LoanBalance); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NomineeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NomineeContact; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NomineeRelation; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankAmount; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CashAmount; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EMPLOYEE.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion Employee Database XL

        #region Employee Service Book XL
        public void PrintServiceBookInXL(int nEmployeeID, double ts)
        {
            List<Employee> oEmployees = new List<Employee>();
            Employee oEmployee = new Employee();
            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            List<EmployeeTraining> oEmployeeTrainings = new List<EmployeeTraining>();
            List<EmployeeExperience> oEmployeeExperiences = new List<EmployeeExperience>();
            EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();

            List<EmployeeSalaryStructure> oEmployeeSalaryStructureHistorys = new List<EmployeeSalaryStructure>();
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetailHistorys = new List<EmployeeSalaryStructureDetail>();

            List<ServiceBookLeave> oServiceBookLeaves = new List<ServiceBookLeave>();

            bool bViewESS = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bViewESS = true; }

            string sSql = "";
            int RowDown = 0;
            if (nEmployeeID > 0)
            {
                sSql = "";
                sSql = "SELECT * FROM View_Employee_WithImage  WHERE  EmployeeID=" + nEmployeeID;
                oEmployees = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                if (oEmployees.Count > 0)
                {
                    oEmployee = oEmployees[0];
                    oEmployee.EmployeePhoto = GetImage(oEmployee.Photo);
                };
                oEmployeeEducations = EmployeeEducation.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
                oEmployeeTrainings = EmployeeTraining.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
                oEmployeeExperiences = EmployeeExperience.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]).OrderBy(x => x.EmployeeExperienceID).ToList();
                sSql = "";
                sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeID=" + nEmployeeID;
                oEmployeeSettlement = EmployeeSettlement.Get(sSql, (int)Session[SessionInfo.currentUserID]);
                sSql = "";
                sSql = "SELECT * FROM View_EmployeeSalaryStructure  WHERE IsActive=1 AND EmployeeID=" + nEmployeeID;
                oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(sSql, (int)Session[SessionInfo.currentUserID]);
                sSql = "";
                sSql = "SELECT * FROM VIEW_EmployeeSalaryStructureDetail  WHERE ESSID  IN(SELECT ESSID FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID + ")";
                oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

                sSql = "";
                sSql = "SELECT * FROM View_EmployeeSalaryStructureHistory  WHERE IsActive=1 AND EmployeeID=" + nEmployeeID;
                oEmployeeSalaryStructureHistorys = EmployeeSalaryStructure.HistoryGets(sSql, (int)Session[SessionInfo.currentUserID]);
                sSql = "";
                sSql = "SELECT * FROM VIEW_EmployeeSalaryStructureDetailHistory  WHERE ESSHistoryID  IN(SELECT ESSHistoryID FROM EmployeeSalaryStructureHistory WHERE EmployeeID=" + nEmployeeID + ")";
                oEmployeeSalaryStructureDetailHistorys = EmployeeSalaryStructureDetail.GetHistorys(sSql, (int)Session[SessionInfo.currentUserID]);

                oServiceBookLeaves = ServiceBookLeave.Gets(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
            }
            RowDown = 25;
            if (oEmployeeEducations.Count > 0) RowDown = RowDown - oEmployeeEducations.Count - 1;
            if (oEmployeeTrainings.Count > 0) RowDown = RowDown - oEmployeeTrainings.Count - 1;

            int nMaxColumn = 0;
            int colIndex = 1;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SERVICE BOOk");
                sheet.Name = "SERVICE BOOk";

                sheet.Column(1).Width = 4;
                sheet.Column(2).Width = 18;
                sheet.Column(3).Width = 16;
                sheet.Column(4).Width = 16;
                sheet.Column(5).Width = 16;
                sheet.Column(6).Width = 18;
                sheet.Column(7).Width = 8;
                sheet.Column(8).Width = 8;
                sheet.Column(9).Width = 8;
                sheet.Column(10).Width = 12;
                sheet.Column(11).Width = 13;
                sheet.Column(12).Width = 14;
                sheet.Column(13).Width = 14;

                nMaxColumn = 13;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "সার্ভিস বুক"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region প্রথম ভাগ
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "ফর্ম নং-৭(ক)"; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 4];
                cell.Merge = true;
                cell.Value = "প্রথম ভাগ "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = colIndex + 4;
                if (oEmployee.EmployeePhoto != null)
                {
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 5, colIndex];
                    cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    ExcelPicture excelImage = null;
                    excelImage = sheet.Drawings.AddPicture(oEmployee.Name + oEmployee.EmployeeID.ToString(), oEmployee.EmployeePhoto);
                    excelImage.From.Column = colIndex - 1;
                    excelImage.From.Row = rowIndex - 1;
                    excelImage.SetSize(125, 120);
                    excelImage.From.ColumnOff = this.Pixel2MTU(2);
                    excelImage.From.RowOff = this.Pixel2MTU(2);
                }
                else
                {
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 5, colIndex];
                    cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                }
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 4];
                cell.Merge = true;
                cell.Value = "শ্রমিকের সনাক্তকরনের তথ্যঃ"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 4];
                cell.Merge = true;
                cell.Value = "১। শ্রমিকের নামঃ" + (oEmployee.NameInBangla != "" ? oEmployee.NameInBangla : oEmployee.Name); cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 4];
                cell.Merge = true;
                cell.Value = "২। পিতার নামঃ" + oEmployee.FatherName; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 4];
                cell.Merge = true;
                cell.Value = "৩। মাতার নামঃ" + oEmployee.MotherName; ; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 4];
                cell.Merge = true;
                cell.Value = "৪। স্বামী/ স্ত্রীর নাম(প্রযোজ্য ক্ষেত্রে):"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5];
                cell.Merge = true;
                cell.Value = "৫। স্থায়ি ঠিকানাঃ" + oEmployee.ParmanentAddress; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                //colIndex = 1;
                //cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                //cell.Value = "৬। বর্তমান ঠিকানাঃ" + oEmployee.PresentAddress; cell.Style.Font.Bold = false;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5];
                cell.Merge = true;
                cell.Value = "৬। বর্তমান ঠিকানাঃ" + (oEmployee.PresentAddress); cell.Style.Font.Bold = false;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5];
                cell.Merge = true;
                cell.Value = "৭। জন্মতারিখ/বয়সঃ" + this.DateFormat(oEmployee.DateOfBirthInString); cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5];
                cell.Merge = true;
                cell.Value = "৮। জাতীয় পরিচয় পত্র নং(যদি থাকে):" + (oEmployee.NationalID.ToString()); cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5];
                cell.Merge = true;
                cell.Value = "৯। শিক্ষাগত যোগ্যতাঃ"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                int nSL = 0;
                if (oEmployeeEducations.Count > 0)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "নং"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "ডিগ্রি"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "বোর্ড"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "পাসের সন"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "ফলাফল"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "প্রতিষ্ঠান"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex = rowIndex + 1;

                    foreach (EmployeeEducation oitem in oEmployeeEducations)
                    {
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(nSL.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oitem.Degree; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oitem.BoardUniversity; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(oitem.PassingYear.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(oitem.Result); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oitem.Institution; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        rowIndex = rowIndex + 1;
                    }
                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "১০। প্রশিক্ষণ/ বিশেষ দক্ষতা (যদি থাকে):"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                if (oEmployeeTrainings.Count > 0)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "নং"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "কোর্সের নাম"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "হইতে"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "পর্যন্ত"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "পাসের তারিখ"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "ফলাফল"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    nSL = 0;
                    foreach (EmployeeTraining oitem in oEmployeeTrainings)
                    {
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(nSL.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oitem.CourseName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oitem.StartDateInString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oitem.EndDateInString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oitem.PassingDateInString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oitem.Result; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        rowIndex = rowIndex + 1;
                    }
                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "১১। উচ্চতাঃ" + oEmployee.Height + "সেঃমিঃ"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "১২। রক্তের গ্রুপ(যদি থাকে):" + oEmployee.BloodGroup; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "১৩। সনাক্ত করার জন্য বিশেষ কোন চিহ্ন(যদি থাকে):" + oEmployee.IdentificationMart; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "১৪। সার্ভিস বুক খোলার তারিখঃ" + this.DateFormat(oEmployee.DateOfJoinInString); cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "১৫। বাম হাতের বৃদ্ধাঙ্গুলির ছাপঃ"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                rowIndex = rowIndex + RowDown;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 3, colIndex = colIndex + 2]; cell.Merge = true;
                cell.Value = "শ্রমিকের স্বাক্ষর"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, colIndex + 1, rowIndex + 3, colIndex + 3]; cell.Merge = true;
                cell.Value = "মালিক/ব্যবস্থাপনা কর্তৃপক্ষের স্বাক্ষর"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rowIndex = rowIndex + 4;


                #endregion

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                rowIndex = rowIndex + 2;

                #region দ্বিতীয় ভাগ
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "ফর্ম নং-৭(খ)"; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = "দ্বিতীয় ভাগ "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = " মালিকের ও চাকুরির তথ্য সমূহ "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                if (oEmployeeExperiences.Count > 0)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "নং"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "প্রতিষ্ঠান"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "মালিক/ ব্যবস্থাপনা কর্তৃপক্ষের নাম"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "চাকরি ত্যাগ/অবসানের ধরন/কারন"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "যোগদান"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "চাকরি ত্যাগ"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "কর্তৃপক্ষের স্বাক্ষর"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "শ্রমিকের স্বাক্ষর"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    nSL = 0;
                    foreach (EmployeeExperience oitem in oEmployeeExperiences)
                    {
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = this.NumberFormat(nSL.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oitem.Organization; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oitem.StartDateString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oitem.EndDateString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        rowIndex = rowIndex + 1;
                    }

                    if (oEmployeeSettlement.EmployeeSettlementID > 0)
                    {
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = this.NumberFormat(nSL.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        rowIndex = rowIndex + 1;
                    }
                }
                #endregion দ্বিতীয় ভাগ

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex++, nMaxColumn]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                rowIndex = rowIndex + 2;

                if (bViewESS)
                {
                    #region তৃতীয় ভাগ
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = "ফর্ম নং-৭(গ)"; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn];
                    cell.Merge = true;
                    cell.Value = "তৃতীয় ভাগ "; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn];
                    cell.Merge = true;
                    cell.Value = " সার্ভিস রেকর্ড ও মজুরি এবং ভাতা সংক্রান্ত তথ্য সমূহ "; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    if (oEmployeeSalaryStructure.ESSID > 0)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true;
                        cell.Value = "নং"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true;
                        cell.Value = "যোগদান"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true;
                        cell.Value = "পদবি ও কার্ড"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                        cell.Value = "মাসিক মজুরি"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        colIndex = colIndex + 5;
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true;
                        cell.Value = "প্রভিডেন্ট ফান্ড"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true;
                        cell.Value = "কর্তৃপক্ষের স্বাক্ষর"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, nMaxColumn]; cell.Merge = true;
                        cell.Value = "শ্রমিকের স্বাক্ষর"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        colIndex = 4;
                        rowIndex++;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "মুল মজুরি"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "বাড়ি ভাড়া"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "চিকিৎসা ভাতা"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "বোনাস"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "অন্যান্য"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "মোট"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "শ্রমিকের দেয়"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                        cell.Value = "কর্তৃপক্ষের দেয়"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        rowIndex = rowIndex + 1;

                        nSL = 1;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = this.NumberFormat(nSL.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oEmployeeSalaryStructure.DateOfJoinInString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oEmployeeSalaryStructure.DesignationName + "[" + oEmployeeSalaryStructure.EmployeeCode + "]"; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        List<EmployeeSalaryStructureDetail> oESSDetails = new List<EmployeeSalaryStructureDetail>();
                        oESSDetails = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).OrderBy(x => x.SalaryHeadID).ToList();

                        //if(oESSDetails.Any())
                        //{
                        while (oESSDetails.Count() < 3)
                            oESSDetails.Add(new EmployeeSalaryStructureDetail());

                        foreach (EmployeeSalaryStructureDetail oItem in oESSDetails.GetRange(0, 3))
                        {
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = this.NumberFormat(oItem.Amount.ToString()); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        //}

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat("0"); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        double nOthersAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic && x.SalaryHeadID == 18).Sum(x => x.Amount);//conveyance
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(nOthersAmount.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        //double nTotalAmount = oEmployeeSalaryStructureDetails.Sum(x => x.Amount);
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(oEmployeeSalaryStructure.GrossAmount.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(0.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(0.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        rowIndex = rowIndex + 1;

                        foreach (EmployeeSalaryStructure oItem in oEmployeeSalaryStructureHistorys)
                        {
                            nSL++;
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = this.NumberFormat(nSL.ToString()); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = this.DateFormat(oItem.DateOfJoinInString); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = oItem.DesignationName + "[" + oItem.EmployeeCode + "]"; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            oESSDetails = new List<EmployeeSalaryStructureDetail>();
                            oESSDetails = oEmployeeSalaryStructureDetailHistorys.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic && x.ESSID == oItem.ESSHistoryID).OrderBy(x => x.SalaryHeadID).ToList();

                            //if (oESSDetails.Any())
                            //{
                            while (oESSDetails.Count() < 3)
                                oESSDetails.Add(new EmployeeSalaryStructureDetail());

                            foreach (EmployeeSalaryStructureDetail oItemDetail in oESSDetails.GetRange(0, 3))
                            {
                                cell = sheet.Cells[rowIndex, colIndex++];
                                cell.Value = this.NumberFormat(oItemDetail.Amount.ToString()); cell.Style.Font.Bold = false;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            //}

                            //foreach (EmployeeSalaryStructureDetail oItemDetail in oESSDetails)
                            //{
                            //    cell = sheet.Cells[rowIndex, colIndex++];
                            //    cell.Value = this.NumberFormat(oItemDetail.Amount.ToString()); cell.Style.Font.Bold = false;
                            //    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //}

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = this.NumberFormat("0"); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            nOthersAmount = oEmployeeSalaryStructureDetailHistorys.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic && x.ESSID == oItem.ESSHistoryID && x.SalaryHeadID == 18).Sum(x => x.Amount);//conveyance
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = this.NumberFormat(nOthersAmount.ToString()); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            //nTotalAmount = oEmployeeSalaryStructureDetailHistorys.Where(x => x.ESSID == oItem.ESSHistoryID).Sum(x => x.Amount);
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = this.NumberFormat(oItem.GrossAmount.ToString()); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = this.NumberFormat(0.ToString()); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = this.NumberFormat(0.ToString()); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            rowIndex = rowIndex + 1;
                        }
                    }
                    #endregion তৃতীয় ভাগ
                }

                #region চতুর্থ  ভাগ
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex++, colIndex + 5]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                rowIndex = rowIndex + 2;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = bViewESS ? "ফর্ম নং-৭(ঘ)" : "ফর্ম নং-৭(গ)"; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = bViewESS ? "চতুর্থ ভাগ " : "তৃতীয় ভাগ "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true;
                cell.Value = " ছুটির রেকর্ড "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                if (oServiceBookLeaves.Count > 0)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "নং"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "হইতে "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "পর্যন্ত"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "ছুটি"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "মোট"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "কর্তৃপক্ষের স্বাক্ষর"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "শ্রমিকের স্বাক্ষর"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex = rowIndex + 1;

                    nSL = 0;
                    foreach (ServiceBookLeave oitem in oServiceBookLeaves)
                    {
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(nSL.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oitem.StartDateInString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.DateFormat(oitem.EndDateInString); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = oitem.Leave; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = this.NumberFormat(oitem.LeaveTaken.ToString()); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        rowIndex = rowIndex + 1;
                    }
                }

                #endregion চতুর্থ ভাগ

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SERVICEBOOK.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }

        public string NumberFormat(string sNum)
        {
            char[] NumbersInBangla = { '০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯' };
            char[] NumbersInEnglish = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            char[] arr = sNum.ToCharArray();

            foreach (char ch in arr)
            {
                int i = 0;
                while (i != 10)
                {
                    if (ch == NumbersInEnglish[i])
                    {
                        sNum = sNum.Replace(ch, NumbersInBangla[i]);
                        break;
                    }
                    i++;
                }
            }
            return sNum;
        }

        public string DateFormat(string sDate)
        {
            string banglaDate = "";
            string[] DateInEnglish = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string[] DateInBangla = { "জানু.", "ফেব্রু.", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভে.", "ডিসে." };

            string[] arr = sDate.Split(' ');

            banglaDate = this.NumberFormat(arr[0]);

            foreach (string st in arr)
            {
                int i = 0;
                while (i != 12)
                {
                    if (st == DateInEnglish[i])
                    {
                        banglaDate = banglaDate + " " + DateInBangla[i];
                        break;
                    }
                    i++;
                }
            }
            return banglaDate + " " + this.NumberFormat(arr[2]);
        }

        #endregion Employee Service Book XL
        //public Image GetCompanyLogo(Company oCompany)
        //{
        //    if (oCompany.OrganizationLogo != null)
        //    {
        //        MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(Response.OutputStream, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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

        #region Employee gets write by Mahabub for Merchandiser picker in A007
        [HttpPost]
        public JsonResult EmployeeSearchByName(Employee oEmployee)
        {
            List<Employee> oEmployees = new List<Employee>();
            _oEmployees = new List<Employee>();
            string sSQL = "";
            if ((int)Session[SessionInfo.FinancialUserType] != (int)EnumFinancialUserType.Normal_User)
            {
                if (oEmployee.Name != "" && oEmployee.Name != null)
                {
                    sSQL = "SELECT * FROM View_Employee Where Name LIKE ('%" + oEmployee.Name + "%') AND  EmployeeDesignationType =" + oEmployee.EmployeeTypeID;
                    _oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oEmployees = Employee.Gets((EnumEmployeeDesignationType)oEmployee.EmployeeTypeID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                if (oEmployee.Name != "" && oEmployee.Name != null)
                {
                    sSQL = "SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ") AND  EmployeeDesignationType = " + oEmployee.EmployeeTypeID.ToString() + " AND Name LIKE ('%" + oEmployee.Name + "%')";
                }
                else
                {
                    sSQL = "SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ") AND  EmployeeDesignationType = " + oEmployee.EmployeeTypeID.ToString();
                }
                _oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region EmployeeDocument
        [HttpPost]
        public string UploadAttchment(HttpPostedFileBase file, EmployeeDocument oEmployeeDocument, int nEmployeeID)
        {
            string sFeedBackMessage = "File Upload successfully";
            byte[] data;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    //double nMaxLength = 1024 * 1024;
                    if (data == null || data.Length <= 0)
                    {
                        sFeedBackMessage = "Please select an file!";
                    }
                    //else if (data.Length > nMaxLength)
                    //{
                    //    sFeedBackMessage = "You can select maximum 1MB file size!";
                    //}
                    //else if (oCompanyDocument.CDID <= 0)
                    //{
                    //    sFeedBackMessage = "Your Selected CompanyDocument Is Invalid!";
                    //}
                    else
                    {
                        oEmployeeDocument.DocFile = data;
                        oEmployeeDocument.FileName = file.FileName;
                        oEmployeeDocument.DocFileType = file.ContentType;
                        oEmployeeDocument.EmployeeID = nEmployeeID;
                        oEmployeeDocument = oEmployeeDocument.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    sFeedBackMessage = "Please select a file!";
                }
            }
            catch (Exception ex)
            {
                sFeedBackMessage = "";
                sFeedBackMessage = ex.Message;
            }

            return sFeedBackMessage;

        }

        [HttpPost]
        public ActionResult DownloadAttachment(FormCollection oFormCollection)
        {
            EmployeeDocument oEmployeeDocument = new EmployeeDocument();
            try
            {
                int nEmployeeDocumentAttchmentID = Convert.ToInt32(oFormCollection["EDID"]);
                oEmployeeDocument = EmployeeDocument.GetWithAttachFile(nEmployeeDocumentAttchmentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oEmployeeDocument.DocFile != null)
                {
                    var file = File(oEmployeeDocument.DocFile, oEmployeeDocument.DocFileType);
                    file.FileDownloadName = oEmployeeDocument.FileName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oEmployeeDocument.FileName);
            }
        }

        [HttpPost]
        public JsonResult DeleteEmployeeDocument(EmployeeDocument oEmployeeDocument)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oEmployeeDocument.Delete(oEmployeeDocument.EDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeDocument(int nEmpID)
        {
            List<EmployeeDocument> _oEmployeeDocument = new List<EmployeeDocument>();
            try
            {
                string sSql = "SELECT * FROM EmployeeDocument WHERE EmployeeID=" + nEmpID;
                _oEmployeeDocument = EmployeeDocument.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeDocument[0].ErrorMessage = ex.Message;
            }
            _oEmployeeDocument.ForEach(
            x =>
            {
                x.DocFile = null;
            }
            );
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeDocument);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion




        #region BanglaIDCard

        public void PrintEmployeeCardBothSideBangla(string sIDs, string itemIDs, DateTime expDate, double ts)
        {
            int nPrintCount = 1;

            _oEmployee = new Employee();
            //string sSql = "select * from View_Employee_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            string sSql = "select * from View_Employee_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            foreach (Employee item in _oEmployee.EmployeeHrms)
            {
                string sQuery = "select TOP(1)* from HRResponsibility where HRRID IN(SELECT HRResponsibilityID FROM DesignationResponsibility WHERE DRPID=" + item.DRPID + " AND DesignationID =" + item.DesignationID + ") ";
                item.HRResponsibility = HRResponsibility.Gets(sQuery, (int)Session[SessionInfo.currentUserID]);
            }
            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }
            _oEmployee.ErrorMessage = itemIDs;


            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nRowIndex = 0, nEndCol = 16, nColumnIndex = 2;
            double nRowHeight = 11;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ID Card");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                //sheet.PrinterSettings.TopMargin = 0;
                //sheet.PrinterSettings.LeftMargin = 0;
                //sheet.PrinterSettings.BottomMargin = 0;
                //sheet.PrinterSettings.RightMargin = 0;
                sheet.PrinterSettings.Orientation = eOrientation.Portrait;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;
                sheet.Name = "ID Card";

                #region Column Declare
                sheet.Column(1).Width = 0;//Extra
                sheet.Column(2).Width = 3; //Blank
                sheet.Column(3).Width = 8;//Image
                sheet.Column(4).Width = 3;//Blank
                sheet.Column(5).Width = 10;//caption
                sheet.Column(6).Width = 2; //:
                sheet.Column(7).Width = 16;//value
                sheet.Column(8).Width = 1;//Blank

                sheet.Column(9).Width = 2; //Middle Blank

                sheet.Column(10).Width = 7;//Caption
                sheet.Column(11).Width = 1; //:
                sheet.Column(12).Width = 12;//Value
                sheet.Column(13).Width = 11;//Caption
                sheet.Column(14).Width = 1;//:
                sheet.Column(15).Width = 10;//Value
                sheet.Column(16).Width = 1;//Blank
                #endregion

                int nCount = 0;
                int comCounter = 0;
                int empCounter = 0;
                int comSigCount = 0;
                if (_oEmployee.EmployeeHrms != null)
                {
                    int nStartEmpIndex = 0;
                    for (int i = 0; i < _oEmployee.EmployeeHrms.Count; i++)
                    {
                        comCounter++;
                        empCounter++;
                        comSigCount++;

                        nStartEmpIndex = nRowIndex;
                        nColumnIndex = 2;
                        nRowIndex = nRowIndex + 1;
                        #region Blank with Top/Left/Right Border
                        //if (i > 0)
                        //{
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        //}
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region front side
                        //1st row
                        #region Company image

                        sheet.Row(nRowIndex).Height = 30;
                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 4)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = 30;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)];
                        cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture((++comCounter).ToString(), _oEmployee.Company.CompanyLogo);
                        excelImage.From.Column = nColumnIndex;
                        excelImage.From.Row = nRowIndex - 2;
                        excelImage.SetSize(250, 25);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);

                        sheet.Row(nRowIndex).Height = 30;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        //2nd row
                        #region শ্রমিকের পরিচয়পত্র
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "\"শ্রমিকের পরিচয়পত্র\"";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion


                        //3rd row
                        #region image


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, (nColumnIndex), (nRowIndex + 7), (nColumnIndex)].Merge = true;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex), (nRowIndex + 7), (nColumnIndex)]; cell.Value = "ইস্যুর তারিখঃ " + this.StringFormat(DateTime.Now.ToString("dd/MM/yyyy"));
                        cell.Style.Font.Size = 5; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;



                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "প্রতিষ্ঠানের নাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].BusinessUnitNameInBangla)) ? _oEmployee.EmployeeHrms[i].BusinessUnitNameInBangla : _oEmployee.EmployeeHrms[i].BusinessUnitName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //4th row
                        if (_oEmployee.EmployeeHrms[i].EmployeePhoto != null)
                        {
                            sheet.Row(nRowIndex).Height = nRowHeight;
                            sheet.Cells[nRowIndex, (nColumnIndex + 1), (nRowIndex + 3), (nColumnIndex + 1)].Merge = true;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex + 3, (nColumnIndex + 1)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(_oEmployee.EmployeeHrms[i].EmployeeID.ToString(), _oEmployee.EmployeeHrms[i].EmployeePhoto);
                            excelImage.From.Column = nColumnIndex;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(60, 60);
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = nRowHeight;
                            sheet.Cells[nRowIndex, (nColumnIndex + 1), (nRowIndex + 3), (nColumnIndex + 1)].Merge = true;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex + 3, (nColumnIndex + 1)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        }

                        sheet.Cells[nRowIndex, (nColumnIndex + 2), (nRowIndex + 6), (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 2), (nRowIndex + 6), (nColumnIndex + 2)]; cell.Value = "";
                        cell.Style.Font.Size = 5; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;



                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "শ্রমিকের নাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].NameInBangla)) ? _oEmployee.EmployeeHrms[i].NameInBangla : _oEmployee.EmployeeHrms[i].Name;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //5th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "আই ডি কার্ড নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = _oEmployee.EmployeeHrms[i].Code;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //6th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "পদবী";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].DesignationNameInBangla)) ? _oEmployee.EmployeeHrms[i].DesignationNameInBangla : _oEmployee.EmployeeHrms[i].DesignationName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //7th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "বিভাগ/শাখা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].DepartmentNameInBangla)) ? _oEmployee.EmployeeHrms[i].DepartmentNameInBangla : _oEmployee.EmployeeHrms[i].DepartmentName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //8th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "যোগদানের তারিখ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = this.DateFormat(_oEmployee.EmployeeHrms[i].DateOfJoin.ToString("dd MMM yyyy"));
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //9th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "কাজের ধরন";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        string sStringWorkType = "";

                        if (_oEmployee.EmployeeHrms[i].HRResponsibility.Count > 0)
                        {
                            sStringWorkType = _oEmployee.EmployeeHrms[i].HRResponsibility[0].DescriptionInBangla;
                        }

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = sStringWorkType;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //10th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "ফ্লোর";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].LocationNameInBangla)) ? _oEmployee.EmployeeHrms[i].LocationNameInBangla : _oEmployee.EmployeeHrms[i].LocationName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //11th row
                        sheet.Row(nRowIndex).Height = 20;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                        cell.Style.Font.Size = 9; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        if (_oEmployee.EmployeeHrms[i].EmployeeSiganture != null)
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)].Merge = true;
                            cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture((empCounter += 2).ToString(), _oEmployee.EmployeeHrms[i].EmployeeSiganture);
                            excelImage.From.Column = nColumnIndex;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(125, 20);
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)].Merge = true;
                            cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        }

                        //sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 2)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 2)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        if (_oEmployee.Company.AuthSig != null)
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(_oEmployee.EmployeeHrms[i].NameCode, _oEmployee.Company.AuthSig);
                            excelImage.From.Column = nColumnIndex + 3;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(125, 20);
                            excelImage.From.ColumnOff = this.Pixel2MTU(1);
                            excelImage.From.RowOff = this.Pixel2MTU(1);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        }

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 20;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;



                        //12th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex), nRowIndex, (nColumnIndex)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex, nColumnIndex + 4]; cell.Value = "শ্রমিকের সাক্ষর"; cell.Merge = true;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "মালিক/ব্যবস্থাপক";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //nRowIndex = nRowIndex + 1;

                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        //nRowIndex = nRowIndex + 1;

                        #endregion
                        #endregion

                        nRowIndex = nStartEmpIndex;
                        nColumnIndex = 10;
                        nRowIndex = nRowIndex + 1;

                        #region back side
                        //if(i > 0)
                        //{
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        // }


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = ExcelBorderStyle.None;

                        nRowIndex = nRowIndex + 1;

                        #region প্রতিষ্ঠানের ঠিকানা
                        sheet.Row(nRowIndex).Height = 13;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 2)]; cell.Value = "প্রতিষ্ঠানের ঠিকানা : ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 13;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex, (nColumnIndex + 6)]; cell.Value = "শ্রমিকের স্থায়ী ঠিকানা : ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Address
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 2, (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 2, (nColumnIndex + 2)]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].BUAddressInBangla)) ? _oEmployee.EmployeeHrms[i].BUAddressInBangla : _oEmployee.EmployeeHrms[i].BUAddress;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "গ্রাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].Village)) ? _oEmployee.EmployeeHrms[i].Village : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;



                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "পোস্ট";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].PostOffice)) ? _oEmployee.EmployeeHrms[i].PostOffice : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "থানা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].Thana)) ? _oEmployee.EmployeeHrms[i].Thana : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;


                        //

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "টেলিফোন নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2]; cell.Value = this.StringFormat(_oEmployee.EmployeeHrms[i].BUPhone);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "জেলা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].District)) ? _oEmployee.EmployeeHrms[i].District : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "ফ্যাক্স নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = this.StringFormat(_oEmployee.EmployeeHrms[i].BUFaxNo);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "জরুরী যোগাযোগের ফোন নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployee.EmployeeHrms[i].OtherPhoneNo)) ? this.StringFormat(_oEmployee.EmployeeHrms[i].OtherPhoneNo) : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;


                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "রক্তের গ্রুপ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = _oEmployee.EmployeeHrms[i].BloodGroupST;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "জাতীয় পরিচয় পত্র নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = this.StringFormat(_oEmployee.EmployeeHrms[i].NationalID);
                        cell.Style.Font.Size = 6; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "কার্ডের মেয়াদ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = 9;
                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = this.DateFormat(expDate.ToString("dd MMM yyyy"));
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "প্রক্সিমিটি নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 4, nRowIndex + 1, nColumnIndex + 4].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 5, nRowIndex + 1, nColumnIndex + 5].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = this.StringFormat(_oEmployee.EmployeeHrms[i].CardNo);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 9;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;

                        //

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "উক্ত পরিচয়পত্র হারিয়ে গেলে তাৎক্ষনিক ব্যবস্থাপনা কর্তৃপক্ষকে জানাতে হইবে";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;
                        //nRowIndex = nRowIndex + 1;

                        #endregion
                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.None;


                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        //nRowIndex = nRowIndex + 1;

                        //nRowIndex = nRowIndex + 1;

                        //if (nRowIndex % 75 == 0)
                        //{
                        //    PageBreak(ref sheet, nRowIndex, nEndCol);
                        //}

                        if (nRowIndex % 56 == 0)
                        {
                            PageBreak(ref sheet, nRowIndex, nEndCol);
                        }
                        #endregion
                    }
                }
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=IDCard.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion
        public void PageBreak(ref ExcelWorksheet sheet, int nRowIndex, int nEndCol)
        {
            sheet.Row(nRowIndex).PageBreak = true;
            sheet.Row(nEndCol).PageBreak = true;
        }
        public string StringFormat(string sNum)
        {
            char[] NumbersInBangla = { '০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯', '-', '/', '+', '(', ')' };
            char[] NumbersInEnglish = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '/', '+', '(', ')' };

            char[] arr = sNum.ToCharArray();

            foreach (char ch in arr)
            {
                int i = 0;
                while (i != 10)
                {
                    if (ch == NumbersInEnglish[i])
                    {
                        sNum = sNum.Replace(ch, NumbersInBangla[i]);
                        break;
                    }
                    i++;
                }
            }
            return sNum;
        }





        public void PrintIDCardWithSearchingCriteria(string sEmpIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime expDate, int nRowLength, int nLoadRecords, string sGroupIDs, string sBlockIDs)
        {
            List<EmployeeForIDCard> _oEmployeeForIDCards = new List<EmployeeForIDCard>();
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Employee_For_IDCard WHERE EmployeeID <>0";

            //string sSql = "select * from View_Employee_For_IDCard WHERE EmployeeID<>0";
            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }
            if (!string.IsNullOrEmpty(sBusinessUnitIds))
            {
                sSql += " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (!string.IsNullOrEmpty(sLocationID))
            {
                sSql += " AND LocationID IN(" + sLocationID + ")";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql += " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (!string.IsNullOrEmpty(sGroupIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
            }
            if (!string.IsNullOrEmpty(sBlockIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
            }
            sSql = sSql + ") aa WHERE Row >" + nRowLength + " Order By Code";

            _oEmployeeForIDCards = EmployeeForIDCard.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            foreach (EmployeeForIDCard item in _oEmployeeForIDCards)
            {
                string sQuery = "select TOP(1)* from HRResponsibility where HRRID IN(SELECT HRResponsibilityID FROM DesignationResponsibility WHERE DRPID=" + item.DRPID + " AND DesignationID =" + item.DesignationID + ") ";
                item.HRResponsibility = HRResponsibility.Gets(sQuery, (int)Session[SessionInfo.currentUserID]);
            }
            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, (int)Session[SessionInfo.currentUserID]);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);

            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nRowIndex = 0, nEndCol = 16, nColumnIndex = 2;
            double nRowHeight = 11;
            if (_oEmployeeForIDCards.Count <= 0)
            {
                nRowIndex = 1;
            }

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ID Card");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                //sheet.PrinterSettings.TopMargin = 0;
                //sheet.PrinterSettings.LeftMargin = 0;
                //sheet.PrinterSettings.BottomMargin = 0;
                //sheet.PrinterSettings.RightMargin = 0;
                sheet.PrinterSettings.Orientation = eOrientation.Portrait;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;
                sheet.Name = "ID Card";

                #region Column Declare
                sheet.Column(1).Width = 0;//Extra
                sheet.Column(2).Width = 3; //Blank
                sheet.Column(3).Width = 8;//Image
                sheet.Column(4).Width = 3;//Blank
                sheet.Column(5).Width = 10;//caption
                sheet.Column(6).Width = 2; //:
                sheet.Column(7).Width = 16;//value
                sheet.Column(8).Width = 1;//Blank

                sheet.Column(9).Width = 2; //Middle Blank

                sheet.Column(10).Width = 7;//Caption
                sheet.Column(11).Width = 1; //:
                sheet.Column(12).Width = 12;//Value
                sheet.Column(13).Width = 11;//Caption
                sheet.Column(14).Width = 1;//:
                sheet.Column(15).Width = 10;//Value
                sheet.Column(16).Width = 1;//Blank
                #endregion

                int nCount = 0;
                int comCounter = 0;
                int empCounter = 0;
                int comSigCount = 0;
                if (_oEmployeeForIDCards.Count > 0)
                {
                    int nStartEmpIndex = 0;
                    for (int i = 0; i < _oEmployeeForIDCards.Count; i++)
                    {
                        comCounter++;
                        empCounter++;
                        comSigCount++;

                        nStartEmpIndex = nRowIndex;
                        nColumnIndex = 2;
                        nRowIndex = nRowIndex + 1;
                        #region Blank with Top/Left/Right Border
                        //if (i > 0)
                        //{
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        //}
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region front side
                        //1st row
                        #region Company image

                        sheet.Row(nRowIndex).Height = 30;
                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 4)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = 30;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)];
                        cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture((++comCounter).ToString(), _oEmployee.Company.CompanyLogo);
                        excelImage.From.Column = nColumnIndex;
                        excelImage.From.Row = nRowIndex - 2;
                        excelImage.SetSize(250, 25);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);

                        sheet.Row(nRowIndex).Height = 30;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        //2nd row
                        #region শ্রমিকের পরিচয়পত্র
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "\"শ্রমিকের পরিচয়পত্র\"";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion


                        //3rd row
                        #region image


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, (nColumnIndex), (nRowIndex + 7), (nColumnIndex)].Merge = true;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex), (nRowIndex + 7), (nColumnIndex)]; cell.Value = "ইস্যুর তারিখঃ " + this.StringFormat(DateTime.Now.ToString("dd/MM/yyyy"));
                        cell.Style.Font.Size = 5; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;



                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "প্রতিষ্ঠানের নাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].BusinessUnitNameInBangla)) ? _oEmployeeForIDCards[i].BusinessUnitNameInBangla : _oEmployeeForIDCards[i].BusinessUnitName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //4th row
                        if (_oEmployeeForIDCards[i].EmployeePhoto != null)
                        {
                            sheet.Row(nRowIndex).Height = nRowHeight;
                            sheet.Cells[nRowIndex, (nColumnIndex + 1), (nRowIndex + 3), (nColumnIndex + 1)].Merge = true;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex + 3, (nColumnIndex + 1)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(_oEmployeeForIDCards[i].EmployeeID.ToString(), _oEmployeeForIDCards[i].EmployeePhoto);
                            excelImage.From.Column = nColumnIndex;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(60, 60);
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = nRowHeight;
                            sheet.Cells[nRowIndex, (nColumnIndex + 1), (nRowIndex + 3), (nColumnIndex + 1)].Merge = true;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex + 3, (nColumnIndex + 1)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        }

                        sheet.Cells[nRowIndex, (nColumnIndex + 2), (nRowIndex + 6), (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 2), (nRowIndex + 6), (nColumnIndex + 2)]; cell.Value = "";
                        cell.Style.Font.Size = 5; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;



                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "শ্রমিকের নাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].NameInBangla)) ? _oEmployeeForIDCards[i].NameInBangla : _oEmployeeForIDCards[i].Name;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //5th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "আই ডি কার্ড নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = _oEmployeeForIDCards[i].Code;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //6th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "পদবী";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].DesignationNameInBangla)) ? _oEmployeeForIDCards[i].DesignationNameInBangla : _oEmployeeForIDCards[i].DesignationName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //7th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "বিভাগ/শাখা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].DepartmentNameInBangla)) ? _oEmployeeForIDCards[i].DepartmentNameInBangla : _oEmployeeForIDCards[i].DepartmentName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //8th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "যোগদানের তারিখ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = this.DateFormat(_oEmployeeForIDCards[i].JoiningDate.ToString("dd MMM yyyy"));
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //9th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "কাজের ধরন";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        string sStringWorkType = "";

                        if (_oEmployeeForIDCards[i].HRResponsibility.Count > 0)
                        {
                            sStringWorkType = _oEmployeeForIDCards[i].HRResponsibility[0].DescriptionInBangla;
                        }

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = sStringWorkType;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //10th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "ফ্লোর";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].LocationNameInBangla)) ? _oEmployeeForIDCards[i].LocationNameInBangla : _oEmployeeForIDCards[i].LocationName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //11th row
                        sheet.Row(nRowIndex).Height = 20;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                        cell.Style.Font.Size = 9; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        if (_oEmployeeForIDCards[i].EmployeeSiganture != null)
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)].Merge = true;
                            cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture((empCounter += 2).ToString(), _oEmployeeForIDCards[i].EmployeeSiganture);
                            excelImage.From.Column = nColumnIndex;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(125, 20);
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)].Merge = true;
                            cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        }

                        //sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 2)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 2)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        if (_oEmployee.Company.AuthSig != null)
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(_oEmployeeForIDCards[i].NameCode, _oEmployee.Company.AuthSig);
                            excelImage.From.Column = nColumnIndex + 3;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(125, 20);
                            excelImage.From.ColumnOff = this.Pixel2MTU(1);
                            excelImage.From.RowOff = this.Pixel2MTU(1);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        }

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 20;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;



                        //12th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex), nRowIndex, (nColumnIndex)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex, nColumnIndex + 4]; cell.Value = "শ্রমিকের সাক্ষর"; cell.Merge = true;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "মালিক/ব্যবস্থাপক";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //nRowIndex = nRowIndex + 1;

                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        //nRowIndex = nRowIndex + 1;

                        #endregion
                        #endregion

                        nRowIndex = nStartEmpIndex;
                        nColumnIndex = 10;
                        nRowIndex = nRowIndex + 1;

                        #region back side
                        //if(i > 0)
                        //{
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        // }


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = ExcelBorderStyle.None;

                        nRowIndex = nRowIndex + 1;

                        #region প্রতিষ্ঠানের ঠিকানা
                        sheet.Row(nRowIndex).Height = 13;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 2)]; cell.Value = "প্রতিষ্ঠানের ঠিকানা : ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 13;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex, (nColumnIndex + 6)]; cell.Value = "শ্রমিকের স্থায়ী ঠিকানা : ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Address
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 2, (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 2, (nColumnIndex + 2)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].BUAddressInBangla)) ? _oEmployeeForIDCards[i].BUAddressInBangla : _oEmployeeForIDCards[i].BUAddress;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "গ্রাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].Village)) ? _oEmployeeForIDCards[i].Village : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;



                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "পোস্ট";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].PostOffice)) ? _oEmployeeForIDCards[i].PostOffice : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "থানা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].Thana)) ? _oEmployeeForIDCards[i].Thana : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;


                        //

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "টেলিফোন নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].BUPhone);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "জেলা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].District)) ? _oEmployeeForIDCards[i].District : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "ফ্যাক্স নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].BUFaxNo);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "জরুরী যোগাযোগের ফোন নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].OtherPhoneNo)) ? this.StringFormat(_oEmployeeForIDCards[i].OtherPhoneNo) : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;


                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "রক্তের গ্রুপ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = _oEmployeeForIDCards[i].BloodGroupST;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "জাতীয় পরিচয় পত্র নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].NationalID);
                        cell.Style.Font.Size = 6; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "কার্ডের মেয়াদ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = 9;
                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = this.DateFormat(expDate.ToString("dd MMM yyyy"));
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "প্রক্সিমিটি নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 4, nRowIndex + 1, nColumnIndex + 4].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 5, nRowIndex + 1, nColumnIndex + 5].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].CardNo);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 9;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;

                        //

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "উক্ত পরিচয়পত্র হারিয়ে গেলে তাৎক্ষনিক ব্যবস্থাপনা কর্তৃপক্ষকে জানাতে হইবে";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;
                        //nRowIndex = nRowIndex + 1;

                        #endregion
                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.None;


                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        //nRowIndex = nRowIndex + 1;

                        //nRowIndex = nRowIndex + 1;

                        //if (nRowIndex % 75 == 0)
                        //{
                        //    PageBreak(ref sheet, nRowIndex, nEndCol);
                        //}

                        if (nRowIndex % 56 == 0)
                        {
                            PageBreak(ref sheet, nRowIndex, nEndCol);
                        }
                        #endregion
                    }
                }
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=IDCard.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }



        #region download format

        public void DownloadFormat()
        {
            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Format Downlaod");
                sheet.Name = "Format Downlaod";

                int n = 1;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 13;


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "FathersName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "MothersName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Address"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date of Birth"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gender"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att. Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date of join"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date of Confirmation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Proximity Card No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AccNo"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "User"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Category"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Cash Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mr. X"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mr. A"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mrs. B"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Address"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "22/12/1995"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Male"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att Scheme1"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "2002"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "3003"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "22/12/2017"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "23/12/2017"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "4001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Scheme 1"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "20000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "100008"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101.101.10101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Permanent"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "2001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "15000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                rowIndex++;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 16]; cell.Value = "Green Color columns are mandatory fields."; cell.Style.Font.Bold = false; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        #endregion

        #region Print EMployee Information
        public ActionResult PrintEmployeeProfile(int id, int buid)
        {
            Employee oEmployee = new Employee();
            if (id > 0)
            {
                oEmployee = oEmployee.Get(id, (int)Session[SessionInfo.currentUserID]);
                oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM View_EmployeeBankAccount WHERE EmployeeID =" + oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeTINInformations = EmployeeTINInformation.Get(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeEducations = EmployeeEducation.Gets(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeTrainings = EmployeeTraining.Gets(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeSettlement = EmployeeSettlement.Get(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeePhoto = this.GetImage(oEmployee.Photo);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPrintEmployeeProfile oReport = new rptPrintEmployeeProfile();
            byte[] abytes = oReport.PrepareReport(oEmployee, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintEmployeeProfileList(string ids)
        {
            Employee oEmployee = new Employee();
            List<Employee> oEmployees = new List<Employee>();
            List<Employee> oTempEmployees = new List<Employee>();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            string sSQL = "SELECT * FROM View_Employee WHERE EmployeeID IN (" + ids + ")";
            oTempEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (Employee oItem in oTempEmployees)
            {
                oEmployee = new Employee();
                oBusinessUnit = new BusinessUnit();
                oEmployee = oEmployee.Get(oItem.EmployeeID, (int)Session[SessionInfo.currentUserID]);
                oBusinessUnit = oBusinessUnit.GetWithImage(oEmployee.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oEmployee.BusinessUnit = oBusinessUnit;
                oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM View_EmployeeBankAccount WHERE EmployeeID =" + oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeTINInformations = EmployeeTINInformation.Get(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeEducations = EmployeeEducation.Gets(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeTrainings = EmployeeTraining.Gets(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeeSettlement = EmployeeSettlement.Get(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oEmployee.EmployeePhoto = this.GetImage(oEmployee.Photo);
                oEmployees.Add(oEmployee);
            }

            List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);



            rptPrintEmployeeProfileList oReport = new rptPrintEmployeeProfileList();
            byte[] abytes = oReport.PrepareReport(oEmployees, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion
        #region Regresh Degree Sequence
        [HttpPost]
        public JsonResult RefreshDegreeSequence(List<EmployeeEducation> oEmployeeEducations)
        {
            EmployeeEducation oEmployeeEducation = new EmployeeEducation();
            List<EmployeeEducation> _oEmployeeEducations = new List<EmployeeEducation>();
            try
            {
                foreach (EmployeeEducation oItem in oEmployeeEducations)
                {
                    oEmployeeEducation = oItem.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                _oEmployeeEducations = EmployeeEducation.Gets("SELECT * FROM EmployeeEducation WHERE EmployeeID=" + oEmployeeEducations[0].EmployeeID+" ORDER BY Sequence ASC", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeEducations = new List<EmployeeEducation>();
                oEmployeeEducation = new EmployeeEducation();
                oEmployeeEducation.ErrorMessage = ex.Message;
                _oEmployeeEducations.Add(oEmployeeEducation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeEducations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Regresh Training Sequence
        [HttpPost]
        public JsonResult RefreshTrainingSequence(List<EmployeeTraining> oEmployeeTrainings)
        {
            EmployeeTraining oEmployeeTraining = new EmployeeTraining();
            List<EmployeeTraining> _oEmployeeTrainings = new List<EmployeeTraining>();
            try
            {
                foreach (EmployeeTraining oItem in oEmployeeTrainings)
                {
                    oEmployeeTraining = oItem.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                _oEmployeeTrainings = EmployeeTraining.Gets(oEmployeeTrainings[0].EmployeeID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeTrainings = new List<EmployeeTraining>();
                oEmployeeTraining = new EmployeeTraining();
                oEmployeeTraining.ErrorMessage = ex.Message;
                _oEmployeeTrainings.Add(oEmployeeTraining);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeTrainings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }


}
