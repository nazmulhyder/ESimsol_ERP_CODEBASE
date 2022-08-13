using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSolFinancial.Controllers
{
    public class DepartmentController : Controller
    {
        #region Declaration
        Department _oDepartment = new Department();
        List<Department> _oDepartments = new List<Department>();
        TDepartment _oTDepartment = new TDepartment();
        List<TDepartment> _oTDepartments = new List<TDepartment>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private TDepartment GetRoot()
        {
            TDepartment oTDepartment = new TDepartment();
            foreach (TDepartment oItem in _oTDepartments)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oTDepartment;
        }

        private IEnumerable<TDepartment> GetChild(int nDepartmentID)
        {
            List<TDepartment> oTDepartments = new List<TDepartment>();
            foreach (TDepartment oItem in _oTDepartments)
            {
                if (oItem.parentid == nDepartmentID)
                {
                    oTDepartments.Add(oItem);
                }
            }
            return oTDepartments;
        }

        private void AddTreeNodes(ref TDepartment oTDepartment)
        {
            IEnumerable<TDepartment> oChildNodes;
            oChildNodes = GetChild(oTDepartment.id);
            oTDepartment.children = oChildNodes;

            foreach (TDepartment oItem in oChildNodes)
            {
                TDepartment oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private bool ValidateInput(Department oDepartment)
        {
            if (oDepartment.Name == null || oDepartment.Name == "")
            {
                _sErrorMessage = "Please enter Department Name";
                return false;
            }
            if (oDepartment.ParentID <= 0)
            {
                _sErrorMessage = "Invalid Parent Department try again";
                return false;
            }
            return true;
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }


        #region New Task
        public ActionResult ViewDepartments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDepartments = new List<Department>();
            _oDepartment = new Department();
            _oTDepartment = new TDepartment();
            _oTDepartments = new List<TDepartment>();
            try
            {
                _oDepartments = Department.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDepartments.Count <= 0)
                {
                    _oDepartment.Code = "1001";
                    _oDepartment.Name = "Root";
                    _oDepartment.Description = "N/A";
                    _oDepartment = _oDepartment.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    _oDepartments.Add(_oDepartment);
                }
                foreach (Department oItem in _oDepartments)
                {

                    _oTDepartment = new TDepartment();
                    _oTDepartment.id = oItem.DepartmentID;
                    _oTDepartment.parentid = oItem.ParentID;
                    _oTDepartment.text = oItem.Name;
                    _oTDepartment.attributes = "";
                    _oTDepartment.code = oItem.Code;
                    _oTDepartment.sequence = oItem.Sequence;
                    _oTDepartment.requiredPerson = oItem.RequiredPerson;
                    _oTDepartment.Description = oItem.Description;
                    _oTDepartment.IsActiveInString = oItem.IsActive == true ? "Yes" : "No";
                    _oTDepartments.Add(_oTDepartment);
                }
                _oTDepartment = new TDepartment();
                _oTDepartment = GetRoot();
                this.AddTreeNodes(ref _oTDepartment);

                ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
                oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.COS = oTempClientOperationSetting;

                return View(_oTDepartment);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return PartialView(_oTDepartment);
            }
        }

        [HttpPost]
        public JsonResult Save(Department oDepartment)
        {
            _oDepartment = new Department();
            try
            {
                _oDepartment = oDepartment;
                _oDepartment = _oDepartment.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oDepartment = new Department();
                _oDepartment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDepartment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getchildren(TDepartment oTDepartment)
        {
            _oDepartments = new List<Department>();
            try
            {
                string sSQL = "SELECT * FROM Department WHERE ParentID=" + oTDepartment.id.ToString();
                if(!string.IsNullOrEmpty(oTDepartment.text))
                {
                    sSQL += " AND Name LIKE '%"+oTDepartment.text+"%'";
                }
                _oDepartments = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oDepartments = new List<Department>();
                _oDepartment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDepartments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult getchildrenTree(TDepartment oTDepartment)
        {
            List<TDepartment> oTDepartments = new List<TDepartment>();
            _oDepartments = new List<Department>();
            _oDepartment = new Department();
            _oTDepartment = new TDepartment();
            _oTDepartments = new List<TDepartment>();
            try
            {
                string sSQL = "SELECT * FROM Department";

                if (!string.IsNullOrEmpty(oTDepartment.text))
                {
                    _oDepartments = Department.GetsDeptWithParent(oTDepartment.text, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oDepartments = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                //_oDepartments = Department.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDepartments.Count <= 0)
                {
                    _oDepartment.Code = "1001";
                    _oDepartment.Name = "Root";
                    _oDepartment.Description = "N/A";
                    _oDepartment = _oDepartment.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    _oDepartments.Add(_oDepartment);
                }
                foreach (Department oItem in _oDepartments)
                {
                    _oTDepartment = new TDepartment();
                    _oTDepartment.id = oItem.DepartmentID;
                    _oTDepartment.parentid = oItem.ParentID;
                    _oTDepartment.text = oItem.Name;
                    _oTDepartment.attributes = "";
                    _oTDepartment.code = oItem.Code;
                    _oTDepartment.sequence = oItem.Sequence;
                    _oTDepartment.requiredPerson = oItem.RequiredPerson;
                    _oTDepartment.Description = oItem.Description;
                    _oTDepartment.IsActiveInString = oItem.IsActive == true ? "Yes" : "No";
                    _oTDepartments.Add(_oTDepartment);
                }
                _oTDepartment = new TDepartment();
                _oTDepartment = GetRoot();
                this.AddTreeNodes(ref _oTDepartment);
                oTDepartments = new List<TDepartment>();
                oTDepartments.Add(_oTDepartment);
            }
            catch (Exception ex)
            {
                _oDepartments = new List<Department>();
                _oDepartment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTDepartment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

                


        [HttpPost]
        public JsonResult Delete(Department oDepartment)
        {
            string sfeedbackmessage = "";
            _oDepartment = new Department();
            try
            {
                sfeedbackmessage = _oDepartment.Delete(oDepartment.DepartmentID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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
        public JsonResult getDepartment(Department oDepartment)
        {
            try
            {
                _oDepartment = new Department();
                _oDepartment = _oDepartment.Get(oDepartment.DepartmentID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oDepartments = new List<Department>();
                _oDepartment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDepartment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Picker
        public ActionResult DepartmentPiker()
        {

            Department oDepartment = new Department();
            oDepartment.Departments = Department.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(oDepartment);
            //oDepartment.Departments = Department.GetsForAUI(((User)(Session[SessionInfo.CurrentUser])).UserID); 
            //oProduct.Products = Product.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);            
        }

       
        [HttpGet]
        public JsonResult LoadComboDepartmentList()
        {
            List<Department> oDepartments = new List<Department>();
            Department oDepartment = new Department();
            oDepartment.Name = "-- Select Department --";
            oDepartments.Add(oDepartment);
            oDepartments.AddRange(Department.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDepartments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult LoadComboDepartmentListbyID(int id)
        {
            List<Department> oDepartments = new List<Department>();
            Department oDepartment = new Department();
            oDepartment.Name = "-- Select Department --";
            oDepartments.Add(oDepartment);
            string sSQL = "SELECT * FROM Department WHERE DepartmentID != " + id + "";
            oDepartment.Departments = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oDepartments.AddRange(Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDepartments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Department Picker

        public ActionResult DepartmentPickerWithCheckBox(int id)
        {
            string sSql = "";
            try
            {
                Department oDepartment = new Department();                
                if (id > 0)
                {
                    //("SELECT * FROM Department WHERE DepartmentID=%n", nID);
                    sSql = "SELECT * FROM Department WHERE DepartmentID IN (SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE LocationID=" + id + ")";
                    
                    _oDepartments = Department.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    string sDepartmentIDs = "";

                    if (_oDepartments.Count > 0)
                    {
                        foreach (var oDpt in _oDepartments)
                        {
                            sDepartmentIDs = sDepartmentIDs + oDpt.DepartmentID.ToString() + ",";
                        }
                        if (sDepartmentIDs.Length > 0)
                        {
                            sDepartmentIDs = sDepartmentIDs.Remove(sDepartmentIDs.Length - 1, 1);
                        }
                        _oDepartments = Department.GetDepartmentHierarchy(sDepartmentIDs,
                            ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    else
                    {
                        oDepartment.ErrorMessage = "No departments found";
                        return PartialView(oDepartment);
                    }

                }
                else
                {
                    sSql="SELECT * FROM Department";
                    _oDepartments = Department.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }



                TDepartment _oTDepartment = new TDepartment();
                oDepartment.TDepartment = this.GetTDepartment(_oDepartments);
                return PartialView(oDepartment);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return PartialView(_oDepartment);
            }
        }

        [HttpPost]
        public JsonResult GetsDepartmentMenuTree(Department oDepartment)
        {

            Department oTempDepartment = new Department();
            oTempDepartment = oTempDepartment.Get(oDepartment.DepartmentID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            TDepartment _oTDepartment = new TDepartment();
            //oTempDepartment.TDepartment = this.GetTDepartment();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oTempDepartment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public TDepartment GetTDepartment(List<Department> oDepartments)
        {
            _oDepartments = new List<Department>();
            _oDepartment = new Department();
            _oTDepartment = new TDepartment();
            _oTDepartments = new List<TDepartment>();
            _oDepartments = oDepartments;
            try
            {
                foreach (Department oItem in _oDepartments)
                {

                    _oTDepartment = new TDepartment();
                    _oTDepartment.id = oItem.DepartmentID;
                    _oTDepartment.parentid = oItem.ParentID;
                    _oTDepartment.text = oItem.Name;
                    _oTDepartment.attributes = "";
                    _oTDepartment.code = oItem.Code;
                    _oTDepartment.sequence = oItem.Sequence;
                    _oTDepartment.requiredPerson = oItem.RequiredPerson;
                    _oTDepartment.Description = oItem.Description;
                    _oTDepartments.Add(_oTDepartment);
                }
                _oTDepartment = new TDepartment();
                _oTDepartment = GetRoot();
                this.AddTreeNodes(ref _oTDepartment);
                return _oTDepartment;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return _oTDepartment;
            }
        }


        [HttpPost]
        public JsonResult GetsDepartments(Department oDepartment)
        {
            List<Department> oDepartments = new List<Department>();

            string sSql = "";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + "WITH ProcessHL (DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson)"
                + " AS(SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM Department AS Dept "
                + " Where Dept.ParentID =0 OR DepartmentID In (Select ParentID from Department Where DepartmentID In (SELECT DepartmentID"
                + " FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0 AND  DepartmentRequirementPolicyID IN(SELECT DRPID"
                + " FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID;
                if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                sSql = sSql + "))) UNION ALL"
                + " SELECT Dept.DepartmentID, Dept.Code, Dept.Name,Dept.[Description], Dept.ParentID,Dept.IsActive,Dept.Sequence,Dept.RequiredPerson FROM Department"
                + " AS Dept  INNER JOIN ProcessHL AS DR ON Dept.ParentID = DR.DepartmentID AND Dept.DepartmentID In (SELECT DepartmentID FROM DepartmentRequirementPolicy"
                + " WHERE DepartmentRequirementPolicyID<>0 AND  DepartmentRequirementPolicyID IN(SELECT DRPID FROM"
                + " DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID;
                if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                sSql = sSql + ")))"
                + "SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM ProcessHL Group By DepartmentID,"
                + "Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson Order By DepartmentID";

            }
            else {
                

                sSql = sSql + "WITH ProcessHL (DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson)"
                + " AS(SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM Department AS Dept "
                + " Where Dept.ParentID =0 OR DepartmentID In (Select ParentID from Department Where DepartmentID In (SELECT DepartmentID"
                + " FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
                if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                sSql = sSql + ")) UNION ALL"
                + " SELECT Dept.DepartmentID, Dept.Code, Dept.Name,Dept.[Description], Dept.ParentID,Dept.IsActive,Dept.Sequence,Dept.RequiredPerson FROM Department"
                + " AS Dept  INNER JOIN ProcessHL AS DR ON Dept.ParentID = DR.DepartmentID AND Dept.DepartmentID In (SELECT DepartmentID FROM DepartmentRequirementPolicy"
                + " WHERE DepartmentRequirementPolicyID<>0 ";
                if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                sSql = sSql + "))"
                + "SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM ProcessHL Group By DepartmentID,"
                + "Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson Order By DepartmentID";


                //if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE  BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + "))"; }
                //if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE LocationID IN(" + oDepartment.LocationIDs + "))"; }
            }

            oDepartments = Department.Gets(sSql,(int)(Session[SessionInfo.currentUserID]));
            
            TDepartment oTDepartment = new TDepartment();
            oTDepartment = this.GetTDepartment(oDepartments);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTDepartment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDepartment(Department oDepartment)
        {
            List<Department> oDepartments = new List<Department>();
            try
            {
                //string sSQL = "Select * from Department Where IsActive=1 And DepartmentID Not In(Select distinct(ParentID) from Department)";

                //if(!string.IsNullOrEmpty(oDepartment.LocationIDs))
                //    sSQL += " And DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE LocationID IN (" + oDepartment.LocationIDs + "))";
                //if (!string.IsNullOrEmpty(oDepartment.BusinessUnitIDs))
                //    sSQL += " And DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE BusinessUnitID IN (" + oDepartment.BusinessUnitIDs + "))";
                //oDepartments = Department.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));


                string sSql = "";
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + "WITH ProcessHL (DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson)"
                    + " AS(SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM Department AS Dept "
                    + " Where Dept.ParentID =0 OR DepartmentID In (Select ParentID from Department Where DepartmentID In (SELECT DepartmentID"
                    + " FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0 AND  DepartmentRequirementPolicyID IN(SELECT DRPID"
                    + " FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID;
                    if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                    if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                    sSql = sSql + "))) UNION ALL"
                    + " SELECT Dept.DepartmentID, Dept.Code, Dept.Name,Dept.[Description], Dept.ParentID,Dept.IsActive,Dept.Sequence,Dept.RequiredPerson FROM Department"
                    + " AS Dept  INNER JOIN ProcessHL AS DR ON Dept.ParentID = DR.DepartmentID AND Dept.DepartmentID In (SELECT DepartmentID FROM DepartmentRequirementPolicy"
                    + " WHERE DepartmentRequirementPolicyID<>0 AND  DepartmentRequirementPolicyID IN(SELECT DRPID FROM"
                    + " DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID;
                    if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                    if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                    sSql = sSql + ")))"
                    + "SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM ProcessHL Group By DepartmentID,"
                    + "Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson Order By DepartmentID";

                }
                else
                {


                    sSql = sSql + "WITH ProcessHL (DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson)"
                    + " AS(SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM Department AS Dept "
                    + " Where Dept.ParentID =0 OR DepartmentID In (Select ParentID from Department Where DepartmentID In (SELECT DepartmentID"
                    + " FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
                    if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                    if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                    sSql = sSql + ")) UNION ALL"
                    + " SELECT Dept.DepartmentID, Dept.Code, Dept.Name,Dept.[Description], Dept.ParentID,Dept.IsActive,Dept.Sequence,Dept.RequiredPerson FROM Department"
                    + " AS Dept  INNER JOIN ProcessHL AS DR ON Dept.ParentID = DR.DepartmentID AND Dept.DepartmentID In (SELECT DepartmentID FROM DepartmentRequirementPolicy"
                    + " WHERE DepartmentRequirementPolicyID<>0 ";
                    if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + ")"; }
                    if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND LocationID IN(" + oDepartment.LocationIDs + ")"; }
                    sSql = sSql + "))"
                    + "SELECT DepartmentID, Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson FROM ProcessHL Group By DepartmentID,"
                    + "Code, Name,[Description], ParentID,IsActive,Sequence,RequiredPerson Order By DepartmentID";


                    //if (oDepartment.BusinessUnitIDs != "" && oDepartment.BusinessUnitIDs != null) { sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE  BusinessUnitID IN(" + oDepartment.BusinessUnitIDs + "))"; }
                    //if (oDepartment.LocationIDs != "" && oDepartment.LocationIDs != null) { sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE LocationID IN(" + oDepartment.LocationIDs + "))"; }
                }

                oDepartments = Department.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            }
            catch (Exception e)
            {
                oDepartment = new Department();
                oDepartment.ErrorMessage = e.Message;
                oDepartments = new List<Department>();
                oDepartments.Add(oDepartment);
            }
           
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDepartments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAllDepartments(Department oDepartment)
        {
            List<Department> oDepartments = new List<Department>();
            string sSql = "";
            sSql = " SELECT * FROM Department Order By Name";
            oDepartments = Department.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            TDepartment oTDepartment = new TDepartment();
            oTDepartment = this.GetTDepartment(oDepartments);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTDepartment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region XL
        public ActionResult PrintDepartmentXL(double ts)
        {
            List<Department> oDepartments = new List<Department>();
            string sSql = "SELECT Code,Name,(SELECT Code FROM Department PD WHERE D.ParentID=PD.DepartmentID ) PCode FROM Department  D WHERE DepartmentID >0 ORDER BY Code";


            oDepartments = Department.GetsXL(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<DepartmentXL>));

            DepartmentXL oDepartmentXL = new DepartmentXL();
            List<DepartmentXL> oDepartmentXLs = new List<DepartmentXL>();

            int nCount = 0;
            foreach (Department oItem in oDepartments)
            {
                nCount++;
                oDepartmentXL = new DepartmentXL();
                oDepartmentXL.SL = nCount.ToString();
                oDepartmentXL.Code = oItem.Code;
                oDepartmentXL.Name = oItem.Name;
                oDepartmentXL.NameInBangla = oItem.NameInBangla;
                oDepartmentXL.ParentCode = oItem.PCode;

                oDepartmentXLs.Add(oDepartmentXL);
            }

            //serializer.Serialize(stream, oDTESs);
            serializer.Serialize(stream, oDepartmentXLs);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "DepartmentList.xls");
        }
        #endregion XL

    }


}
