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
    public class DesignationController : Controller
    {
        #region Declaration
        Designation _oDesignation = new Designation();
        List<Designation> _oDesignations = new List<Designation>();
        TDesignation _oTDesignation = new TDesignation();
        List<TDesignation> _oTDesignations = new List<TDesignation>();
        
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private TDesignation GetRoot()
        {
            TDesignation oTDesignation = new TDesignation();
            foreach (TDesignation oItem in _oTDesignations)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oTDesignation;
        }

        private IEnumerable<TDesignation> GetChild(int nDesignationID)
        {
            List<TDesignation> oTDesignations = new List<TDesignation>();
            foreach (TDesignation oItem in _oTDesignations)
            {
                if (oItem.parentid == nDesignationID)
                {
                    oTDesignations.Add(oItem);
                }
            }
            return oTDesignations;
        }

        private void AddTreeNodes(ref TDesignation oTDesignation)
        {
            IEnumerable<TDesignation> oChildNodes;
            oChildNodes = GetChild(oTDesignation.id);
            oTDesignation.children = oChildNodes;

            foreach (TDesignation oItem in oChildNodes)
            {
                TDesignation oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private bool ValidateInput(Designation oDesignation)
        {
            if (oDesignation.Name == null || oDesignation.Name == "")
            {
                _sErrorMessage = "Please enter Designation Name";
                return false;
            }
            if (oDesignation.ParentID <= 0)
            {
                _sErrorMessage = "Invalid Parent Designation try again";
                return false;
            }
            return true;
        }

        #endregion

        #region New Task
        public ActionResult ViewDesignations(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDesignations = new List<Designation>();
            _oDesignation = new Designation();
            _oTDesignation = new TDesignation();
            _oTDesignations = new List<TDesignation>();
            try
            {
                _oDesignations = Designation.Gets("SELECT * FROM View_Designation ORDER BY Name, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDesignations.Count <= 0)
                {
                    _oDesignation.Code = "1001";
                    _oDesignation.Name = "Root";
                    _oDesignation.Description = "N/A";
                    _oDesignation = _oDesignation.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    _oDesignations.Add(_oDesignation);
                }
                foreach (Designation oItem in _oDesignations)
                {

                    _oTDesignation = new TDesignation();
                    _oTDesignation.id = oItem.DesignationID;
                    _oTDesignation.parentid = oItem.ParentID;
                    _oTDesignation.text = oItem.Name;
                    _oTDesignation.attributes = "";
                    _oTDesignation.code = oItem.Code;
                    _oTDesignation.Responsibility = oItem.Responsibility;
                    _oTDesignation.sequence = oItem.Sequence;
                    _oTDesignation.requiredPerson = oItem.RequiredPerson;
                    _oTDesignation.Description = oItem.Description;
                    _oTDesignations.Add(_oTDesignation);
                }
                _oTDesignation = new TDesignation();
                _oTDesignation = GetRoot();
                this.AddTreeNodes(ref _oTDesignation);
                ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
                oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.COS = oTempClientOperationSetting;

                return View(_oTDesignation);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTDesignation);
            }
        }

        public ActionResult ViewDesignation(int id)
        {
            Designation oTempDesignation = new Designation();
            oTempDesignation = oTempDesignation.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            Designation oDesignation = new Designation();
            oDesignation.ParentID = id;
            oDesignation.ParentNodeName = "Selected parent Designation: " + oTempDesignation.Name + "[" + oTempDesignation.Code + "]";
            string sSQL = "SELECT * FROM Designation WHERE ParentID=" + id.ToString();
            oDesignation.ChildNodes = Designation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(oDesignation);
        }

        [HttpPost]
        public JsonResult Save(Designation oDesignation)
        {
            _oDesignation = new Designation();
            try
            {
                _oDesignation = oDesignation;
                _oDesignation = _oDesignation.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oDesignation = new Designation();
                _oDesignation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDesignation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getchildren(TDesignation oTDesignation)
        {
            _oDesignations = new List<Designation>();
            try
            {
                string sSQL = "SELECT * FROM View_Designation WHERE ParentID=" + oTDesignation.id.ToString();
                _oDesignations = Designation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oDesignations = new List<Designation>();
                _oDesignation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDesignations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Designation oDesignation)
        {
            string sfeedbackmessage = "";
            _oDesignation = new Designation();
            try
            {
                sfeedbackmessage = _oDesignation.Delete(oDesignation.DesignationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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
        public ActionResult EditDesignation(int id)
        {
            _oDesignation = new Designation();
            _oDesignation = _oDesignation.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oDesignation);
        }

        [HttpPost]
        public JsonResult getDesignation(Designation oDesignation)
        {
            try
            {
                _oDesignation = new Designation();
                _oDesignation = _oDesignation.Get(oDesignation.DesignationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oDesignations = new List<Designation>();
                _oDesignation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDesignation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Picker
        public ActionResult DesignationPiker()
        {

            Designation oDesignation = new Designation();
            oDesignation.Designations = Designation.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(oDesignation);
            //oDesignation.Designations = Designation.GetsForAUI(((User)(Session[SessionInfo.CurrentUser])).UserID); 
            //oProduct.Products = Product.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);            
        }

        // Modified on 26 Feb 2013 by Fauzul as required Combo on Product Unique Identification
        [HttpGet]
        public JsonResult LoadComboDesignationList()
        {
            List<Designation> oDesignations = new List<Designation>();
            Designation oDesignation = new Designation();
            oDesignation.Name = "-- Select Designation --";
            oDesignations.Add(oDesignation);
            oDesignations.AddRange(Designation.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDesignations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult LoadComboDesignationListbyID(int id)
        {
            List<Designation> oDesignations = new List<Designation>();
            Designation oDesignation = new Designation();
            oDesignation.Name = "-- Select Designation --";
            oDesignations.Add(oDesignation);
            string sSQL = "SELECT * FROM Designation WHERE DesignationID != " + id + "";
            oDesignation.Designations = Designation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oDesignations.AddRange(Designation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDesignations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Designation Picker
        public ActionResult DesignationPickerWithCheckBox(int id)
        {
            try
            {
                Designation oDesignation = new Designation();
                oDesignation = oDesignation.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                return PartialView(oDesignation);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return PartialView(_oDesignation);
            }
        }

        [HttpPost]
        public JsonResult GetsDesignationMenuTree(Designation oDesignation)
        {

            Designation oTempDesignation = new Designation();
            oTempDesignation = oTempDesignation.Get(oDesignation.DesignationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            TDesignation _oTDesignation = new TDesignation();
            oTempDesignation.TDesignation = this.GetTDesignation();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oTempDesignation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public TDesignation GetTDesignation()
        {
            _oDesignations = new List<Designation>();
            _oDesignation = new Designation();
            _oTDesignation = new TDesignation();
            _oTDesignations = new List<TDesignation>();
            
            try
            {
                _oDesignations = Designation.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

                foreach (Designation oItem in _oDesignations)
                {
                    _oTDesignation = new TDesignation();
                    _oTDesignation.id = oItem.DesignationID;
                    _oTDesignation.parentid = oItem.ParentID;
                    _oTDesignation.text = oItem.Name;
                    _oTDesignation.attributes = "";
                    _oTDesignation.code = oItem.Code;
                    _oTDesignation.sequence = oItem.Sequence;
                    _oTDesignation.requiredPerson = oItem.RequiredPerson;
                    _oTDesignation.Description = oItem.Description;
                    _oTDesignation.HRResponsibilityID = oItem.HRResponsibilityID;
                    _oTDesignation.Responsibility = oItem.Responsibility;
                    _oTDesignation.ResponsibilityInBangla = oItem.ResponsibilityInBangla;
                    _oTDesignations.Add(_oTDesignation);
                }

                _oTDesignation = new TDesignation();
                _oTDesignation = GetDesignationRoot();
                this.AddTreeNodes(ref _oTDesignation);
                return _oTDesignation;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return _oTDesignation;
            }
        }

        

        private IEnumerable<TDesignation> GetDesignationChild(int nid)
        {
            List<TDesignation> oTDesignations = new List<TDesignation>();
            foreach (TDesignation oItem in _oTDesignations)
            {
                if (oItem.parentid == nid)
                {
                    if ((((User)Session[SessionInfo.CurrentUser]).IsSuperUser) || ((User)Session[SessionInfo.CurrentUser]).IsPermitted(oItem.id))
                    {
                        oTDesignations.Add(oItem);
                    }
                }
            }
            return oTDesignations;
        }

        private TDesignation GetDesignationRoot()
        {
            TDesignation oTDesignation = new TDesignation();
            foreach (TDesignation oItem in _oTDesignations)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oTDesignation;
        }


        #region Designation By Department Added By Sagor on 14 Jan 2014

        [HttpPost]
        public JsonResult GetsDesignation(int nDeptID, double nts)
        {

            Designation oDesignation = new Designation();
            List<Designation> oDesignations = new List<Designation>();
            int nLocationID = ((User)Session[SessionInfo.CurrentUser]).LocationID;
            string sSQL = "WITH  tbl "+
		                     "AS ( "+
                                   " SELECT  * FROM    Designation WHERE   DesignationID IN (SELECT DesignationID FROM Organogram WHERE DepartmentID=" + nDeptID + ")" +
                                   " UNION ALL "+
                                   "SELECT  rt.* FROM    Designation AS rt JOIN tbl AS ac ON rt.DesignationID = ac.ParentID "+
                                 ")"+
		                  " SELECT distinct * FROM tbl Order By DesignationID";
            
            oDesignations = Designation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if(oDesignations.Count()>0)
            {
                oDesignation.TDesignation = MakeTree(oDesignations);
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDesignation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public TDesignation MakeTree(List<Designation> oTempDesignations)
        {
            _oDesignations = new List<Designation>();
            _oDesignation = new Designation();
            _oTDesignation = new TDesignation();
            _oTDesignations = new List<TDesignation>();
            try
            {
                _oDesignations = oTempDesignations;
                foreach (Designation oItem in _oDesignations)
                {

                    _oTDesignation = new TDesignation();
                    _oTDesignation.id = oItem.DesignationID;
                    _oTDesignation.parentid = oItem.ParentID;
                    _oTDesignation.text = oItem.Name;
                    _oTDesignation.attributes = "";
                    _oTDesignation.code = oItem.Code;
                    _oTDesignation.sequence = oItem.Sequence;
                    _oTDesignation.requiredPerson = oItem.RequiredPerson;
                    _oTDesignation.Description = oItem.Description;
                    _oTDesignations.Add(_oTDesignation);
                }
                _oTDesignation = new TDesignation();
                _oTDesignation = GetDesignationRoot();
                this.AddTreeNodes(ref _oTDesignation);
                return _oTDesignation;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return _oTDesignation;
            }
        }

        #endregion
        #endregion

        #region Gets

        [HttpGet]
        public JsonResult GetsByDRP(int nDRPID, double ts)
        {
            List<Designation> oDesignations = new List<Designation>();
            Designation oDesig = new Designation();
            string sSQL = "SELECT * FROM Designation WHERE DesignationID IN ("
                         + "SELECT DesignationID FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" + nDRPID + ")";
            try
            {
                oDesignations = Designation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oDesig = new Designation();
                oDesig.ErrorMessage = ex.Message;
                oDesignations.Add(oDesig);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDesignations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDesignations(Designation oDesignation)
        {
            TDesignation oTDesignation = new TDesignation();
            oTDesignation = this.GetTDesignation();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTDesignation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(Designation oDesignation)
        {
            List<Designation> oDesignations = new List<Designation>();
            string sBUID = oDesignation.Params.Split('~')[0];
            string sLocationID = oDesignation.Params.Split('~')[1];
            string sDeptID = oDesignation.Params.Split('~')[2];

            string sSQL = "SELECT * FROM Designation Where ParentID<>0 And IsActive=1";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DesignationID IN(SELECT DesignationID FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID  "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }
            //else
            //{
                sSQL = sSQL + " AND DesignationID IN (SELECT distinct(DesignationID) FROM DepartmentRequirementDesignation " +
                               "Where DepartmentRequirementPolicyID In(SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementPolicy Where DepartmentRequirementPolicyID <> 0 ";

            //}
            if (sBUID != "" && sBUID != "0" && sBUID != null) { sSQL = sSQL + " And BusinessUnitID IN(" + sBUID + ")"; }
            if (sLocationID != "" && sLocationID != "0" && sLocationID != null) { sSQL = sSQL + " And LocationID IN(" + sLocationID + ")"; }
            if (sDeptID != "") { sSQL = sSQL + " And DepartmentID IN(" + sDeptID + ")"; }
            sSQL = sSQL + "))";
            try
            {
                oDesignations = Designation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if(oDesignations.Count<=0)
                {
                    throw new Exception("There is no designation in this loacation & department!");
                }
            }
            catch (Exception ex)
            {
                oDesignation = new Designation();
                oDesignation.ErrorMessage = ex.Message;
                oDesignations.Add(oDesignation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDesignations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region XL
        public ActionResult PrintDesigantionXL(double ts)
        {
            List<Designation> oDesignations = new List<Designation>();
            string sSql = "SELECT Code,Name,(SELECT Code FROM Designation PD WHERE D.ParentID=PD.DesignationID ) PCode FROM Designation  D WHERE DesignationID >0 ORDER BY Code";

            oDesignations = Designation.GetsXL(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<DesignationXL>));

            DesignationXL oDesignationXL = new DesignationXL();
            List<DesignationXL> oDesignationXLs = new List<DesignationXL>();

            int nCount = 0;
            foreach (Designation oItem in oDesignations)
            {
                nCount++;
                oDesignationXL = new DesignationXL();
                oDesignationXL.SL = nCount.ToString();
                oDesignationXL.Code = oItem.Code;
                oDesignationXL.Name = oItem.Name;
                oDesignationXL.ParentCode = oItem.PCode;

                oDesignationXLs.Add(oDesignationXL);
            }

            //serializer.Serialize(stream, oDTESs);
            serializer.Serialize(stream, oDesignationXLs);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "DesignationList.xls");
        }
        #endregion XL
    }
    
}
