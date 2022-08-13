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
    public class DepartmentRequirementPolicyV2Controller : Controller
    {
        #region Declartion
        RosterPlan _oRosterPlan = new RosterPlan();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        List<Location> _oLocations = new List<Location>();
        DepartmentSetUp _oDepartmentSetUp = new DepartmentSetUp();
        DesignationResponsibility _oDesignationResponsibility = new DesignationResponsibility();
        List<DepartmentSetUp> _oDepartmentSetUps = new List<DepartmentSetUp>();
        DepartmentRequirementPolicy _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
        List<DepartmentRequirementPolicy> _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
        List<DepartmentRequirementDesignation> _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
        #endregion

        public ActionResult ViewDepartmentSetup_V2(int menuid)
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
                    this.MapLocation_V2(oItem.BusinessUnitID, nid);
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

        private void MapLocation_V2(int nBusinessUnitID, int nParentID)
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
                this.MapDepartment_V2(nBusinessUnitID, oItem.LocationID, nid);
            }

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

        private void AddTreeNodes(ref DepartmentSetUp oDepartmentSetUp)
        {
            List<DepartmentSetUp> oChildNodes;
            oChildNodes = GetChild(oDepartmentSetUp.id);
            oDepartmentSetUp.children = oChildNodes;
            if (oDepartmentSetUp.DataType == 2)
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

        private List<DepartmentRequirementPolicy> GetLocationByBU(int nBUID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            List<DepartmentRequirementPolicy> oLocations = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementPolicys.ForEach(x => oLocations.Add(x));
            oLocations = oLocations.Where(x => x.BusinessUnitID == nBUID).ToList();
            oLocations = oLocations.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
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

        private void MapDepartment_V2(int nBusinessUnitID, int nLocationID, int nParentID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            oDepartmentRequirementPolicys = this.GetDepartmentByLocation_V2(nBusinessUnitID, nLocationID);
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

        private List<DepartmentRequirementPolicy> GetDepartmentByLocation_V2(int nBusinessUnitID, int nLocationID)
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
        public JsonResult Save(DepartmentRequirementPolicy oDepartmentRequirementPolicy)
        {
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            try
            {
                _oDepartmentRequirementPolicy = oDepartmentRequirementPolicy;                
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
            _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();

            try
            {
                _oDepartmentRequirementPolicy = _oDepartmentRequirementPolicy.Get(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, (int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicy.DepartmentRequirementDesignations = DepartmentRequirementDesignation.GetsPolicy(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, (int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicy.DepartmentCloseDays = DepartmentCloseDay.Gets(oDepartmentRequirementPolicy.DepartmentRequirementPolicyID, (int)(Session[SessionInfo.currentUserID]));

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
    }
}