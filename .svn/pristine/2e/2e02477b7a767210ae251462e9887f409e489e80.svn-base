using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;


//using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class DepartmentRequirementPolicyController : Controller
    {
        #region Declartion
        RosterPlan _oRosterPlan = new RosterPlan();
        List<BusinessUnit>  _oBusinessUnits = new List<BusinessUnit>();
        List<Location> _oLocations = new List<Location>();
        DepartmentSetUp _oDepartmentSetUp = new DepartmentSetUp();
        DesignationResponsibility _oDesignationResponsibility = new DesignationResponsibility();
        List<DepartmentSetUp> _oDepartmentSetUps = new List<DepartmentSetUp>();
        DepartmentRequirementPolicy _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
        List<DepartmentRequirementPolicy> _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
        List<DepartmentRequirementDesignation> _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
        #endregion

        #region Function
        private List<DepartmentRequirementDesignation> GetDepartmentRequirementDesignations(DepartmentRequirementPolicy oDepartmentRequirementPolicy)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            DepartmentRequirementDesignation oDepartmentRequirementDesignation = new DepartmentRequirementDesignation();
            int i = 0;
            string sPropertyName = "";
            foreach (TempDesignation oTempDesignation in oDepartmentRequirementPolicy.TempDesignations)
            {
                i = 0;
                foreach (HRMShift oShift in oDepartmentRequirementPolicy.Shifts)
                {
                    oDepartmentRequirementDesignation = new DepartmentRequirementDesignation();
                    oDepartmentRequirementDesignation.DepartmentRequirementDesignationID = oTempDesignation.DepartmentRequirementDesignationID;
                    oDepartmentRequirementDesignation.DepartmentRequirementPolicyID = oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
                    oDepartmentRequirementDesignation.DesignationResponsibilitys = oTempDesignation.DesignationResponsibilitys;
                    oDepartmentRequirementDesignation.DesignationID = oTempDesignation.DesignationID;
                    oDepartmentRequirementDesignation.DesignationSequence = oTempDesignation.Sequence;
                    oDepartmentRequirementDesignation.ShiftID = oShift.ShiftID;
                    oDepartmentRequirementDesignation.ShiftSequence = oShift.Sequence;

                    i++;
                    sPropertyName = "Column" + i.ToString();
                    Type obJectType = oTempDesignation.GetType();
                    PropertyInfo[] aPropertys = obJectType.GetProperties();
                    foreach (PropertyInfo oProperty in aPropertys)
                    {
                        if (oProperty.Name == sPropertyName)
                        {
                            oDepartmentRequirementDesignation.RequiredPerson = Convert.ToInt32(oTempDesignation.GetType().GetProperty(oProperty.Name).GetValue(oTempDesignation, null));
                            break;
                        }
                    }
                    oDepartmentRequirementDesignations.Add(oDepartmentRequirementDesignation);
                }

            }
            return oDepartmentRequirementDesignations;
        }
        private List<TempDesignation> MapTempDesignation(DepartmentRequirementPolicy oDepartmentRequirementPolicy)
        {
            List<TempDesignation> oTempDesignations = new List<TempDesignation>();

            int sPOM = 0;
            string sPropertyName = "";
            int i = 0;
            TempDesignation oTempDesignation = new TempDesignation();
            foreach (DepartmentRequirementDesignation oItem in oDepartmentRequirementPolicy.DepartmentRequirementDesignations)
            {
                if (oItem.DesignationID != sPOM)
                {
                    oTempDesignation = new TempDesignation();
                    oTempDesignation.DesignationID = oItem.DesignationID;
                    oTempDesignation.Designation = oItem.DesignationName;
                    oTempDesignation.DepartmentRequirementDesignationID = oItem.DepartmentRequirementDesignationID;
                    i = 0;
                    foreach (HRMShift oSft in oDepartmentRequirementPolicy.SelectedShifts)
                    {
                        i++;
                        sPropertyName = "Column" + i.ToString();
                        PropertyInfo prop = oTempDesignation.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oTempDesignation, GetDesignation(oSft.ShiftID, oItem.DesignationID, oDepartmentRequirementPolicy.DepartmentRequirementDesignations), null);
                        }
                    }
                    oTempDesignations.Add(oTempDesignation);
                }
                sPOM = oItem.DesignationID;
            }
            return oTempDesignations;
        }
        private int GetDesignation(int nShiftID, int nDesignationID, List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations)
        {
            foreach (DepartmentRequirementDesignation oItem in oDepartmentRequirementDesignations)
            {
                if (oItem.DesignationID == nDesignationID && oItem.ShiftID == nShiftID)
                {
                    return oItem.RequiredPerson;
                }

            }
            return 0;
        }
        private DepartmentSetUp GetRoot()
        {
            DepartmentSetUp oDepartmentSetUp = new DepartmentSetUp();
            foreach (DepartmentSetUp oItem in _oDepartmentSetUps)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oDepartmentSetUp;
        }
        private List<DepartmentSetUp> GetChild(int nDepartmentID)
        {
            List<DepartmentSetUp> oDepartmentSetUps = new List<DepartmentSetUp>();
            foreach (DepartmentSetUp oItem in _oDepartmentSetUps)
            {
                if (oItem.parentid == nDepartmentID)
                {
                    oDepartmentSetUps.Add(oItem);
                }
            }
            return oDepartmentSetUps;
        }
        private int GetChildEmployeeCount(int nDepartmentID)
        {
            int nEmployeeCount = 0;
            foreach (DepartmentSetUp oItem in _oDepartmentSetUps)
            {
                if (oItem.parentid == nDepartmentID)
                {
                    nEmployeeCount = nEmployeeCount + oItem.EmployeeCount;
                }
            }
            return nEmployeeCount;
        }
        private void AddTreeNodes(ref DepartmentSetUp oDepartmentSetUp)
        {
            List<DepartmentSetUp> oChildNodes;
            oChildNodes = GetChild(oDepartmentSetUp.id);
            oDepartmentSetUp.children = oChildNodes;
            if(oDepartmentSetUp.DataType==2)
            {
                int nEmployeeCount = this.GetChildEmployeeCount(oDepartmentSetUp.id);
                oDepartmentSetUp.EmployeeCount = nEmployeeCount;
                oDepartmentSetUp.DetailsInfo = " Total Employee  : " + nEmployeeCount.ToString();
            }
            foreach (DepartmentSetUp oItem in oChildNodes)
            {
                DepartmentSetUp oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private List<Location> GetChildLocations(int nLocationID)
        {
            List<Location> oLocations = new List<Location>();
            foreach (Location oItem in _oLocations)
            {
                if (oItem.ParentID == nLocationID)
                {
                    oLocations.Add(oItem);
                }
            }
            return oLocations;
        }
        private List<DepartmentRequirementDesignation> GetDepartmentByPolicyAndDesignation(int nDesignationID, int nPolicyID)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            foreach (DepartmentRequirementDesignation oItem in _oDepartmentRequirementDesignations)
            {
                if (oItem.DepartmentRequirementPolicyID == nPolicyID && oItem.DesignationID==nDesignationID)
                {
                    oDepartmentRequirementDesignations.Add(oItem);
                }
            }
            return oDepartmentRequirementDesignations;
        }
        private List<DepartmentRequirementDesignation> GetDepartmentByPolicy(int nDepartmentRequirementDesignationID)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            foreach (DepartmentRequirementDesignation oItem in _oDepartmentRequirementDesignations)
            {
                if (oItem.DepartmentRequirementPolicyID == nDepartmentRequirementDesignationID)
                {
                    oDepartmentRequirementDesignations.Add(oItem);
                }
            }
            return oDepartmentRequirementDesignations;
        }
        private bool IsExists(List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations, int nDesignationID)
        {
            foreach (DepartmentRequirementDesignation oItem in oDepartmentRequirementDesignations)
            {
                if (oItem.DesignationID == nDesignationID)
                {
                    return true;
                }
            }
            return false;
        }        
        private string MakeDesignationWiseShiftInfo(int nDesignationID, int nPolicyID, bool bIsEmployeeCount)
        {
            string sDetailsInfo = ""; int nEmployeeCount = 0;
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            oDepartmentRequirementDesignations = this.GetDepartmentByPolicyAndDesignation(nDesignationID, nPolicyID);
            foreach (DepartmentRequirementDesignation oItem in oDepartmentRequirementDesignations)
            {
                if (bIsEmployeeCount)
                {
                    nEmployeeCount = nEmployeeCount + oItem.RequiredPerson;
                }
                else
                {
                    sDetailsInfo = sDetailsInfo + "Shift(" + oItem.StartTime + "-to-" + oItem.EndTime + ") @ " + oItem.RequiredPerson.ToString() + " || ";
                }
            }
            if (bIsEmployeeCount)
            {
                sDetailsInfo = nEmployeeCount.ToString();
            }
            else
            {
                if (sDetailsInfo.Length > 0)
                {
                    sDetailsInfo = sDetailsInfo.Remove(sDetailsInfo.Length - 3, 3);
                }
            }
            return sDetailsInfo;        
        }
        private void MapDesignation(DepartmentRequirementPolicy oDepartmentRequirementPolicy, int nParentID)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            List<DepartmentRequirementDesignation> oTempDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            oDepartmentRequirementDesignations = this.GetDepartmentByPolicy(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID);
            foreach (DepartmentRequirementDesignation oItem in oDepartmentRequirementDesignations)
            {
                if (!IsExists(oTempDepartmentRequirementDesignations, oItem.DesignationID))
                {
                    int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                    _oDepartmentSetUp = new DepartmentSetUp();
                    _oDepartmentSetUp.id = nid;
                    _oDepartmentSetUp.parentid = nParentID;
                    _oDepartmentSetUp.text = oItem.DesignationName;
                    _oDepartmentSetUp.state = "";
                    _oDepartmentSetUp.attributes = "";
                    //_oDepartmentSetUp.DetailsInfo = this.MakeDesignationWiseShiftInfo(oItem.DesignationID, oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, false);
                    _oDepartmentSetUp.DetailsInfo = "";
                    _oDepartmentSetUp.EmployeeCount = Convert.ToInt32(this.MakeDesignationWiseShiftInfo(oItem.DesignationID, oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, true));
                    _oDepartmentSetUp.LocationID = 0;
                    _oDepartmentSetUp.DataType = 4;
                    _oDepartmentSetUps.Add(_oDepartmentSetUp);
                    oTempDepartmentRequirementDesignations.Add(oItem);
                }
            }
        }
        private List<DepartmentRequirementPolicy> GetDepartmentByLocation(int nLocationID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            foreach (DepartmentRequirementPolicy oItem in _oDepartmentRequirementPolicys)
            {
                if (oItem.LocationID == nLocationID)
                {
                    oDepartmentRequirementPolicys.Add(oItem);
                }
            }
            return oDepartmentRequirementPolicys;
        }
        private int GetEmployeeCount(int nLocationID, bool bIsLocationWise)
        {
            int nEmployeeCount = 0;
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            foreach (DepartmentRequirementPolicy oItem in _oDepartmentRequirementPolicys)
            {
                if (bIsLocationWise)
                {
                    if (oItem.LocationID == nLocationID)
                    {
                        nEmployeeCount = nEmployeeCount + oItem.EmployeeCount;
                    }
                }
                else
                {
                    nEmployeeCount = nEmployeeCount + oItem.EmployeeCount;
                }
            }
            return nEmployeeCount;
        }
        private void MapDepartment(int nLocationID, int nParentID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            oDepartmentRequirementPolicys =this.GetDepartmentByLocation(nLocationID);
            foreach (DepartmentRequirementPolicy oItem in oDepartmentRequirementPolicys)
            {
                int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp.id = nid;
                _oDepartmentSetUp.parentid = nParentID;
                _oDepartmentSetUp.text = oItem.DepartmentName+" @ Department";
                _oDepartmentSetUp.state = "closed";
                _oDepartmentSetUp.attributes = "";
                _oDepartmentSetUp.DetailsInfo = " Total Employee  : " + oItem.EmployeeCount.ToString();
                _oDepartmentSetUp.EmployeeCount = oItem.EmployeeCount;
                _oDepartmentSetUp.LocationID = 0;
                _oDepartmentSetUp.DeptReqPolicyID = oItem.DepartmentRequirementPolicyID;
                _oDepartmentSetUp.DataType = 3;
                _oDepartmentSetUps.Add(_oDepartmentSetUp);
                this.MapDesignation(oItem, nid);
            }

        }
        private void MapLocation(int nBUID, int nParentID)
        {
            List<DepartmentRequirementPolicy> oLocations = new List<DepartmentRequirementPolicy>();
            oLocations = this.GetLocationByBU(nBUID);
            foreach (DepartmentRequirementPolicy oItem in oLocations)
            {
                int nEmployeeCount = this.GetEmployeeCount(oItem.LocationID, true);
                int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count-1].id + 1;
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp.id = nid;
                _oDepartmentSetUp.parentid = nParentID;
                _oDepartmentSetUp.text = oItem.LocationName + " @ Location";
                _oDepartmentSetUp.state = "";
                _oDepartmentSetUp.attributes = "";
                _oDepartmentSetUp.DetailsInfo = " Total Employee  : " + nEmployeeCount.ToString();
                _oDepartmentSetUp.EmployeeCount = nEmployeeCount;
                _oDepartmentSetUp.LocationID = oItem.LocationID;
                _oDepartmentSetUp.BusinessUnitID = oItem.BusinessUnitID;
                _oDepartmentSetUp.DataType = 2;
                _oDepartmentSetUps.Add(_oDepartmentSetUp);                
                this.MapDepartment(oItem.LocationID, nid);
                //this.MapLocation(oItem.LocationID, nid);
            }
        }
        private bool IsShiftExists(List<HRMShift> oHRMShifts, int nShiftID)
        {
            foreach (HRMShift oItem in oHRMShifts)
            {
                if (oItem.ShiftID == nShiftID)
                {
                    return true;
                }
            }
            return false;
        }        
        #endregion

        #region New Task
        public ActionResult ViewDepartmentSetup(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            
            Company oCompany = new Company();
            _oLocations = new List<Location>();
            _oDepartmentSetUp = new DepartmentSetUp();
            _oDepartmentSetUps = new List<DepartmentSetUp>();
            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();

            try
            {
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                _oBusinessUnits = BusinessUnit.Gets((int)(Session[SessionInfo.currentUserID]));
                _oLocations = Location.Gets((int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets((int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementDesignations = DepartmentRequirementDesignation.Gets((int)(Session[SessionInfo.currentUserID]));

                #region Set Company Name as Root Location 
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp.id = 1;
                _oDepartmentSetUp.parentid = 0;
                _oDepartmentSetUp.text = oCompany.Name;
                _oDepartmentSetUp.state = "";
                _oDepartmentSetUp.attributes = "";
                _oDepartmentSetUp.LocationID = 0;
                _oDepartmentSetUps.Add(_oDepartmentSetUp);
                #endregion

                #region Map Location in Common Tree
                foreach (BusinessUnit oItem in _oBusinessUnits)
                {
                    int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                    _oDepartmentSetUp = new DepartmentSetUp();
                    _oDepartmentSetUp.id = nid;
                    _oDepartmentSetUp.parentid = 1;
                    _oDepartmentSetUp.text = oItem.Name + " @ BU";
                    _oDepartmentSetUp.state = "";
                    _oDepartmentSetUp.attributes = "";
                    _oDepartmentSetUp.DetailsInfo = "";
                    _oDepartmentSetUp.EmployeeCount = 0;
                    _oDepartmentSetUp.LocationID = 0;                    
                    _oDepartmentSetUps.Add(_oDepartmentSetUp);  

                    //int nParentID = 1;//Common tree RootID
                    int nLocationID = 1;// Avoid Location RootID
                    this.MapLocation(oItem.BusinessUnitID, nid);
                }
                #endregion
                
                int nEmployeeCount = this.GetEmployeeCount(0, false);
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp = GetRoot();
                _oDepartmentSetUp.DetailsInfo = " Total Employee  : " + nEmployeeCount.ToString();
                _oDepartmentSetUp.EmployeeCount = nEmployeeCount;
                this.AddTreeNodes(ref _oDepartmentSetUp);                
                return View(_oDepartmentSetUp);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oDepartmentSetUp);
            }
        }

        [HttpPost]
        public JsonResult Save(DepartmentRequirementPolicy oDepartmentRequirementPolicy)
        {
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            try
            {
                _oDepartmentRequirementPolicy = oDepartmentRequirementPolicy;
                _oDepartmentRequirementPolicy.DepartmentRequirementDesignations = GetDepartmentRequirementDesignations(oDepartmentRequirementPolicy);
                _oDepartmentRequirementPolicy = _oDepartmentRequirementPolicy.Save((int)(Session[SessionInfo.currentUserID]));

            }
            catch (Exception ex)
            {
                _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                _oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDepartmentRequirementPolicy);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPolicy(DepartmentRequirementPolicy oDepartmentRequirementPolicy)
        {
            List<HRMShift> oHRMShifts = new List<HRMShift>(); 
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            List<DepartmentRequirementDesignation> oDRDShiftOrder = new List<DepartmentRequirementDesignation>();
            List<TempDesignation> oTempDesignations = new List<TempDesignation>();
            List<DesignationResponsibility> oDesignationResponsibilitys = new List<DesignationResponsibility>();
            List<DesignationResponsibility> oTempDesignationResponsibilitys = new List<DesignationResponsibility>();
            try
            {
                _oDepartmentRequirementPolicy = _oDepartmentRequirementPolicy.Get(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, (int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicy.DepartmentRequirementDesignations = DepartmentRequirementDesignation.Gets(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, false, (int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicy.DepartmentCloseDays = DepartmentCloseDay.Gets(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, (int)(Session[SessionInfo.currentUserID]));
                oDRDShiftOrder = DepartmentRequirementDesignation.Gets(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, true, (int)(Session[SessionInfo.currentUserID]));
                oDesignationResponsibilitys = DesignationResponsibility.GetsByPolicy(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, (int)(Session[SessionInfo.currentUserID]));
                foreach (DepartmentRequirementDesignation oItem in oDRDShiftOrder)
                {
                    if (!IsShiftExists(oHRMShifts, oItem.ShiftID))
                    {
                        HRMShift oHRMShift = new HRMShift();
                        oHRMShift.ShiftID = oItem.ShiftID;
                        oHRMShift.Name = oItem.NameOfShift;
                        oHRMShift.StartTime = Convert.ToDateTime(oItem.StartTime);
                        oHRMShift.EndTime = Convert.ToDateTime(oItem.EndTime);
                        oHRMShift.Sequence = oItem.ShiftSequence;
                        oHRMShifts.Add(oHRMShift);
                    }
                }
                _oDepartmentRequirementPolicy.SelectedShifts = oHRMShifts;
                oTempDesignations = MapTempDesignation(_oDepartmentRequirementPolicy);
                foreach (TempDesignation oTempDesignation in oTempDesignations)
                {
                    oTempDesignationResponsibilitys = new List<DesignationResponsibility>();
                    foreach (DesignationResponsibility oDesignationResponsibility in oDesignationResponsibilitys)
                    {
                        if (oDesignationResponsibility.DesignationID == oTempDesignation.DesignationID)
                        {
                            oTempDesignationResponsibilitys.Add(oDesignationResponsibility);
                        }
                    }
                    oTempDesignation.DesignationResponsibilitys = oTempDesignationResponsibilitys;
                }
                _oDepartmentRequirementPolicy.TempDesignations = oTempDesignations;
            }
            catch (Exception ex)
            {
                _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                _oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDepartmentRequirementPolicy);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(DepartmentSetUp oDepartmentSetUp)
        {
            string sFeedbackmessage = "";
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            try
            {
                sFeedbackmessage = _oDepartmentRequirementPolicy.Delete(oDepartmentSetUp.DeptReqPolicyID, (int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                sFeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsHRResponsibility(HRResponsibility oHRResponsibility)
        {
            List<HRResponsibility> oHRResponsibilitys = new List<HRResponsibility>();            
            try
            {
                oHRResponsibilitys = HRResponsibility.Gets((int)(Session[SessionInfo.currentUserID]));                
            }
            catch (Exception ex)
            {
                oHRResponsibilitys = new List<HRResponsibility>();
                oHRResponsibility = new HRResponsibility();
                oHRResponsibility.ErrorMessage = ex.Message;
                oHRResponsibilitys.Add(oHRResponsibility);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHRResponsibilitys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets(int nLocationID, int nDepartmentID, double ts)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            string sSQL = "select * from View_DepartmentRequirementDesignation WHERE DepartmentRequirementDesignationID<>0";
            if (nLocationID > 0)
            {
                sSQL = sSQL + " AND LocationID =" + nLocationID;
            }
            if (nDepartmentID > 0)
            {
                sSQL = sSQL + " AND DepartmentID=" + nDepartmentID + "";
            }
            try
            {
                oDepartmentRequirementDesignations = DepartmentRequirementDesignation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oDepartmentRequirementDesignations[0] = new DepartmentRequirementDesignation();
                oDepartmentRequirementDesignations[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDepartmentRequirementDesignations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        #endregion

        #region Old Task
        public ActionResult ViewDepartmentRequirementPolicys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);


            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oDepartmentRequirementPolicys);
        }

        public ActionResult ViewDepartmentRequirementPolicy(int id, double ts)
        {
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            string sSql = "";
            if (id > 0)
            {
                _oDepartmentRequirementPolicy = _oDepartmentRequirementPolicy.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "SELECT * FROM View_DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" +
                       id.ToString() + " ORDER By DesignationID";
                _oDepartmentRequirementPolicy.DepartmentRequirementDesignations =
                    DepartmentRequirementDesignation.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "SELECT * FROM HRM_Shift WHERE ShiftID IN (SELECT Distinct ShiftID FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" + id + ")";
                _oDepartmentRequirementPolicy.SelectedShifts =
                    HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "SELECT * FROM DepartmentCloseDay where DepartmentRequirementPolicyID=" +
                       _oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
                _oDepartmentRequirementPolicy.DepartmentCloseDays =
                    DepartmentCloseDay.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                _oDepartmentRequirementPolicy.TempDesignations = MapTempDesignation(_oDepartmentRequirementPolicy);


            }
            _oDepartmentRequirementPolicy.Shifts = HRMShift.Gets("SELECT * FROM HRM_Shift WHERE IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oDepartmentRequirementPolicy);
        }

        [HttpPost]
        public JsonResult DepartmentRequirementPolicy_Copy(DepartmentRequirementPolicy oDepartmentRequirementPolicy)
        {
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            int id = oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
            string sSql = "";

            _oDepartmentRequirementPolicy = _oDepartmentRequirementPolicy.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //_oDepartmentRequirementPolicy.Organograms = Organogram.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oDepartmentRequirementPolicy.Designations = Designation.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (id > 0)
            {
                sSql = "SELECT * FROM View_DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" +
                       id.ToString() + " ORDER By DesignationID";
                _oDepartmentRequirementPolicy.DepartmentRequirementDesignations =
                    DepartmentRequirementDesignation.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "SELECT * FROM HRM_Shift WHERE ShiftID IN (SELECT Distinct ShiftID FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" + id + ")";
                _oDepartmentRequirementPolicy.SelectedShifts =
                    HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                _oDepartmentRequirementPolicy.TempDesignations = MapTempDesignation(_oDepartmentRequirementPolicy);
                sSql = "SELECT * FROM DepartmentCloseDay where DepartmentRequirementPolicyID=" +
                       _oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
                _oDepartmentRequirementPolicy.DepartmentCloseDays =
                    DepartmentCloseDay.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDepartmentRequirementPolicy);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        

        

        public ActionResult DepartmentRequirementPolicySearch()
        {
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            _oDepartmentRequirementPolicy.Shifts = HRMShift.Gets("SELECT * FROM HRM_Shift WHERE IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oDepartmentRequirementPolicy);
        }

        [HttpPost]
        public JsonResult DepartmentRequirementPolicySearch(string sTempString)
        {
            List<DepartmentRequirementPolicy> DepartmentRequirementPolicies = new List<DepartmentRequirementPolicy>();
            string sPolicyName = Convert.ToString(sTempString.Split('~')[0]);
            string slocationID = Convert.ToString(sTempString.Split('~')[1]);
            string sdepartmentID = Convert.ToString(sTempString.Split('~')[2]);
            int nShiftID = Convert.ToInt32(sTempString.Split('~')[3]);
            string sSQL = "select * from View_DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (sPolicyName != "")
            {
                sSQL = sSQL + " AND Name like'" + "%" + sPolicyName + "%" + "'";
            }
            if (slocationID != "")
            {
                sSQL = sSQL + " AND LocationID=" + slocationID + "";
            }
            if (sdepartmentID != "")
            {
                sSQL = sSQL + " AND DepartmentID=" + sdepartmentID + "";
            }
            if (nShiftID != 0)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN ("
                            + " SELECT DepartmentRequirementPolicyID FROM DepartmentRequirementDesignation"
                            + " WHERE ShiftID=" + nShiftID + ")";
            }
            try
            {
                DepartmentRequirementPolicies = DepartmentRequirementPolicy.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                _oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(DepartmentRequirementPolicies);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WeekDayPicker()
        {
            return PartialView(_oDepartmentRequirementPolicy);
        }

        public ActionResult DRPSearchByName(string sDRPName, double nts)
        {
            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            try
            {
                string sSql = "";
                if (sDRPName == "")
                {
                    sSql = "SELECT * FROM VIEW_DepartmentRequirementPolicy";
                }
                else
                {
                    sSql = "SELECT * FROM VIEW_DepartmentRequirementPolicy WHERE Name LIKE '%" + sDRPName + "%' ";
                }
                _oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDepartmentRequirementPolicys.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
                _oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
                _oDepartmentRequirementPolicys.Add(_oDepartmentRequirementPolicy);
            }
            return PartialView(_oDepartmentRequirementPolicys);
        }
        #endregion

        #region Department Pick for Emp Basic
        [HttpPost]
        public JsonResult DRPPick(string sDRPName)
        {

            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            try
            {
                string sSql = "";

                sSql = " SELECT * FROM View_DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0 AND DepartmentID!=1 ";
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
                }
                if (sDRPName != "")
                {
                    sSql = sSql + " AND Department LIKE '%" + sDRPName + "%' ";
                }
                sSql = sSql + " Order By DepartmentID";

                //if (sDRPName == "")
                //{
                //    sSql = "SELECT * FROM VIEW_DepartmentRequirementPolicy";
                //}
                //else
                //{
                //    sSql = "SELECT * FROM VIEW_DepartmentRequirementPolicy WHERE Department LIKE '%" + sDRPName + "%' ";
                //}
                _oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDepartmentRequirementPolicys.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
                _oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
                _oDepartmentRequirementPolicys.Add(_oDepartmentRequirementPolicy);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDepartmentRequirementPolicys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Department Pick for Emp Basic


        [HttpPost]
        public JsonResult GetDRPByLocations(DepartmentRequirementPolicy oDepartmentRequirementPolicy)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            string sSQL = "select * from View_DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (oDepartmentRequirementPolicy.LocationID > 0)
            {
                sSQL = sSQL + " AND LocationID =" + oDepartmentRequirementPolicy.LocationID;
            } 
            //Added By ASAD
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            try
            {
                oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if(oDepartmentRequirementPolicys.Count<=0)
                {
                    throw new Exception("There is no department in this location!");
                }
            }
            catch (Exception ex)
            {
                oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
                oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
                oDepartmentRequirementPolicys.Add(oDepartmentRequirementPolicy);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDepartmentRequirementPolicys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        // Temp Work Start
        public ActionResult ViewDepartmentSetup_V1(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            Company oCompany = new Company();
            _oLocations = new List<Location>();
            _oDepartmentSetUp = new DepartmentSetUp();
            _oDepartmentSetUps = new List<DepartmentSetUp>();
            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();

            try
            {
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                _oBusinessUnits = BusinessUnit.Gets((int)(Session[SessionInfo.currentUserID]));
                _oLocations = Location.Gets((int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets((int)(Session[SessionInfo.currentUserID]));
                //_oDepartmentRequirementDesignations = DepartmentRequirementDesignation.Gets("SELECT DISTINCT Designation, DesignationID, DepartmentRequirementPolicyID FROM View_DepartmentRequirementDesignation", (int)(Session[SessionInfo.currentUserID]));

                #region Set Company Name as Root Location
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp.id = 1;
                _oDepartmentSetUp.parentid = 0;
                _oDepartmentSetUp.text = oCompany.Name;
                _oDepartmentSetUp.state = "";
                _oDepartmentSetUp.attributes = "";
                _oDepartmentSetUp.LocationID = 0;
                _oDepartmentSetUps.Add(_oDepartmentSetUp);
                #endregion

                #region Map Location in Common Tree
                foreach (BusinessUnit oItem in _oBusinessUnits)
                {
                    int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                    _oDepartmentSetUp = new DepartmentSetUp();
                    _oDepartmentSetUp.id = nid;
                    _oDepartmentSetUp.parentid = 1;
                    _oDepartmentSetUp.text = oItem.Name + " @ BU";
                    _oDepartmentSetUp.state = "";
                    _oDepartmentSetUp.attributes = "";
                    _oDepartmentSetUp.DetailsInfo = "";
                    _oDepartmentSetUp.EmployeeCount = 0;
                    _oDepartmentSetUp.BusinessUnitID = oItem.BusinessUnitID;
                    _oDepartmentSetUp.LocationID = 0;
                    _oDepartmentSetUp.DataType = 1;
                    _oDepartmentSetUps.Add(_oDepartmentSetUp);

                    //int nParentID = 1;//Common tree RootID
                    int nLocationID = 1;// Avoid Location RootID
                    this.MapLocation_V1(oItem.BusinessUnitID, nid);
                }
                #endregion

                int nEmployeeCount = this.GetEmployeeCount(0, false);
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp = GetRoot();
                //_oDepartmentSetUp.DetailsInfo = " Total Employee  : " + nEmployeeCount.ToString();
                _oDepartmentSetUp.DetailsInfo = "";
                _oDepartmentSetUp.EmployeeCount = nEmployeeCount;
                this.AddTreeNodes(ref _oDepartmentSetUp);
                ViewBag.oLocations = _oLocations;
                return View(_oDepartmentSetUp);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                ViewBag.oLocations = _oLocations;
                return View(_oDepartmentSetUp);
            }
        }

        //[HttpPost]
        //public JsonResult SaveDRPDesignation(List<DepartmentRequirementDesignation> oDRPDesigs)
        //{
        //    string sFeedbackmessage = "";
        //    DepartmentRequirementDesignation oDepartmentRequirementPolicy = new DepartmentRequirementDesignation();
        //    try
        //    {
        //        sFeedbackmessage = oDepartmentRequirementPolicy.SaveDRPDesignations(oDRPDesigs, (int)(Session[SessionInfo.currentUserID]));
        //    }
        //    catch (Exception ex)
        //    {
        //        sFeedbackmessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sFeedbackmessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public JsonResult RemoveDRPDesignation(int PID, int CID, bool IsDesig)
        //{
        //    string sFeedbackmessage = "";
        //    string sSql = "";
        //    if (IsDesig) { sSql="DELETE FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" + PID + " AND DesignationID=" + CID; }
        //    else { sSql = "DELETE FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" + PID + " AND ShiftID=" + CID; }
        //    List<DepartmentRequirementDesignation> oDepartmentRequirementPolicys = new List<DepartmentRequirementDesignation>();
        //    DepartmentRequirementDesignation oDepartmentRequirementPolicy = new DepartmentRequirementDesignation();
        //    try
        //    {
        //        oDepartmentRequirementPolicys = DepartmentRequirementDesignation.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
        //    }
        //    catch (Exception ex)
        //    {
        //        sFeedbackmessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sFeedbackmessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //private void MapBU(int nParentID)
        //{
        //    foreach (BusinessUnit oItem in _oBusinessUnits)
        //    {
        //        int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
        //        _oDepartmentSetUp = new DepartmentSetUp();
        //        _oDepartmentSetUp.id = nid;
        //        _oDepartmentSetUp.parentid = nParentID;
        //        _oDepartmentSetUp.text = oItem.Name + " @ BU";
        //        _oDepartmentSetUp.state = "";
        //        _oDepartmentSetUp.attributes = "";
        //        _oDepartmentSetUp.DetailsInfo = " Total Employee  : 0";
        //        _oDepartmentSetUp.EmployeeCount = 0;
        //        _oDepartmentSetUp.LocationID = 0;
        //        _oDepartmentSetUp.DataType = 1;
        //        _oDepartmentSetUps.Add(_oDepartmentSetUp);
        //        this.MapLocation_V1(1, oItem.BusinessUnitID, nid);
        //        this.MapBU(nid);
        //    }
        //}
        private void MapLocation_V1(int nBusinessUnitID, int nParentID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            oDepartmentRequirementPolicys = this.GetLocationByBU(nBusinessUnitID);
            foreach (DepartmentRequirementPolicy oItem in oDepartmentRequirementPolicys)
            {
                int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp.id = nid;
                _oDepartmentSetUp.parentid = nParentID;
                _oDepartmentSetUp.text = oItem.LocationName + " @ Location";
                _oDepartmentSetUp.state = "closed";
                _oDepartmentSetUp.attributes = "";
                //_oDepartmentSetUp.DetailsInfo = " Total Employee  : " + oItem.EmployeeCount.ToString();
                _oDepartmentSetUp.DetailsInfo = "";
                _oDepartmentSetUp.EmployeeCount = oItem.EmployeeCount;
                _oDepartmentSetUp.LocationID = 0;
                _oDepartmentSetUp.DeptReqPolicyID = oItem.DepartmentRequirementPolicyID;
                _oDepartmentSetUp.DataType = 2;
                _oDepartmentSetUps.Add(_oDepartmentSetUp);
                this.MapDepartment_V1(nBusinessUnitID,oItem.LocationID, nid);
            }

        }
        private List<DepartmentRequirementPolicy> GetLocationByBU(int nBUID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            List<DepartmentRequirementPolicy> oLocations = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementPolicys.ForEach(x => oLocations.Add(x));
            oLocations = oLocations.Where(x => x.BusinessUnitID == nBUID).ToList();
            oLocations = oLocations.GroupBy(x=>x.LocationID).Select(x=>x.First()).ToList();
            //List<DepartmentRequirementPolicy> oLocations = new List<DepartmentRequirementPolicy>();
            //foreach (DepartmentRequirementPolicy oItem in _oDepartmentRequirementPolicys)
            //{
            //    if (oItem.BusinessUnitID == nBUID)
            //    {
            //        oDepartmentRequirementPolicys.Add(oItem);
            //    }
            //}
            return oLocations;
        }

        private void MapDepartment_V1(int nBusinessUnitID,int nLocationID, int nParentID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            oDepartmentRequirementPolicys = this.GetDepartmentByLocation_V1(nBusinessUnitID,nLocationID);
            foreach (DepartmentRequirementPolicy oItem in oDepartmentRequirementPolicys)
            {
                int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp.id = nid;
                _oDepartmentSetUp.parentid = nParentID;
                _oDepartmentSetUp.text = oItem.DepartmentName + " @ Department";
                _oDepartmentSetUp.state = "";
                _oDepartmentSetUp.attributes = "";
                //_oDepartmentSetUp.DetailsInfo = " Total Employee  : " + oItem.EmployeeCount.ToString();
                _oDepartmentSetUp.DetailsInfo = "";
                _oDepartmentSetUp.EmployeeCount = oItem.EmployeeCount;
                _oDepartmentSetUp.LocationID = 0;
                _oDepartmentSetUp.DeptReqPolicyID = oItem.DepartmentRequirementPolicyID;
                _oDepartmentSetUp.DataType = 3;
                _oDepartmentSetUps.Add(_oDepartmentSetUp);
                //this.MapDepartment_V1(nBusinessUnitID, oItem.LocationID, nid);
                //this.MapDesignation(oItem, nid);
            }
        }
        private List<DepartmentRequirementPolicy> GetDepartmentByLocation_V1(int nBusinessUnitID, int nLocationID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            foreach (DepartmentRequirementPolicy oItem in _oDepartmentRequirementPolicys)
            {
                if (oItem.LocationID == nLocationID && oItem.BusinessUnitID == nBusinessUnitID)
                {
                    oDepartmentRequirementPolicys.Add(oItem);
                }
            }
            return oDepartmentRequirementPolicys;
        }
        // Temp Work End
    }
}
