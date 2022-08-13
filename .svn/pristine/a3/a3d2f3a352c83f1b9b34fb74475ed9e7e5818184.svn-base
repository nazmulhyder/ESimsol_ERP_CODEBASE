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
    public class DepartmentRequirementPolicyPermissionController : Controller
    {
        #region Declartion
        RosterPlan _oRosterPlan = new RosterPlan();
        List<DepartmentRequirementPolicy> _oDRPBusinessUnits = new List<DepartmentRequirementPolicy>();
        List<Location> _oLocations = new List<Location>();
        Location _oLocation = new Location();
        TLocation _oTLocation = new TLocation();
        List<TLocation> _oTLocations = new List<TLocation>();
        DepartmentSetUp _oDepartmentSetUp = new DepartmentSetUp();
        List<DepartmentSetUp> _oDepartmentSetUps = new List<DepartmentSetUp>();
        DepartmentRequirementPolicy _oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
        List<DepartmentRequirementPolicy> _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
        List<DepartmentRequirementDesignation> _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
        List<DepartmentRequirementPolicyPermission> _oDepartmentRequirementPolicyPermissions = new List<DepartmentRequirementPolicyPermission>();
        #endregion

        public ActionResult View_DRPPermission(int id)
        {
            User oUser = new User();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            return View(oUser);
        }

        [HttpPost]
        public JsonResult GetDRPSetup(User oUser)
        {
            Company oCompany = new Company();
            _oLocations = new List<Location>();
            _oDepartmentSetUp = new DepartmentSetUp();
            _oDepartmentSetUps = new List<DepartmentSetUp>();
            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();

            try
            {
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                string sSql = "SELECT  * FROM View_DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT  MIN(DepartmentRequirementPolicyID) FROM DepartmentRequirementPolicy GROUP BY BusinessUnitID)";
                _oDRPBusinessUnits = DepartmentRequirementPolicy.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
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
                foreach (DepartmentRequirementPolicy oItem in _oDRPBusinessUnits)
                {
                    int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                    _oDepartmentSetUp = new DepartmentSetUp();
                    _oDepartmentSetUp.id = nid;
                    _oDepartmentSetUp.parentid = 1;
                    _oDepartmentSetUp.text = oItem.BUName + " @ BU";
                    _oDepartmentSetUp.state = "";
                    _oDepartmentSetUp.attributes = "";
                    _oDepartmentSetUp.DetailsInfo = "";
                    _oDepartmentSetUp.EmployeeCount = 0;
                    _oDepartmentSetUp.BusinessUnitID = oItem.BusinessUnitID;
                    _oDepartmentSetUp.LocationID = 0;
                    _oDepartmentSetUp.DataType = 1;
                    _oDepartmentSetUps.Add(_oDepartmentSetUp);
                    this.MapLocation(oItem.BusinessUnitID, nid);
                }
                #endregion

                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp = GetRoot();
                _oDepartmentSetUp.DetailsInfo = "";
                _oDepartmentSetUp.EmployeeCount = 0;
                this.AddTreeNodes(ref _oDepartmentSetUp);

                _oDepartmentSetUp.DRPPs = DepartmentRequirementPolicyPermission.Gets("SELECT * FROM  DepartmentRequirementPolicyPermission WHERE USERID=" + oUser.UserID, (int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                _oDepartmentSetUp = new DepartmentSetUp();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oDepartmentSetUp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDRPSetupForAttProcess(User oUser)
        {
            Company oCompany = new Company();
            _oLocations = new List<Location>();
            _oDepartmentSetUp = new DepartmentSetUp();
            _oDepartmentSetUps = new List<DepartmentSetUp>();
            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();

            try
            {
                
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                string sSql = "SELECT  * FROM View_DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT  MIN(DepartmentRequirementPolicyID) FROM DepartmentRequirementPolicy GROUP BY BusinessUnitID)";
                _oDRPBusinessUnits = DepartmentRequirementPolicy.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets((int)(Session[SessionInfo.currentUserID]));
                sSql = "SELECT * FROM View_DepartmentRequirementPolicyPermission WHERE UserID = " + (int)(Session[SessionInfo.currentUserID]);
                _oDepartmentRequirementPolicyPermissions = DepartmentRequirementPolicyPermission.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

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
                if (_oDepartmentRequirementPolicyPermissions.Count > 0)
                {
                    foreach (DepartmentRequirementPolicy oItem in _oDRPBusinessUnits)
                    {
                        int nBUID = 0;
                        try
                        {
                            nBUID = (_oDepartmentRequirementPolicyPermissions.Where(x => x.BusinessUnitID == oItem.BusinessUnitID).ToList())[0].BusinessUnitID;
                            if (nBUID == 0) throw new Exception();
                            int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                            _oDepartmentSetUp = new DepartmentSetUp();
                            _oDepartmentSetUp.id = nid;
                            _oDepartmentSetUp.parentid = 1;
                            _oDepartmentSetUp.text = oItem.BUName + " @ BU";
                            _oDepartmentSetUp.state = "";
                            _oDepartmentSetUp.attributes = "";
                            _oDepartmentSetUp.DetailsInfo = "";
                            _oDepartmentSetUp.EmployeeCount = 0;
                            _oDepartmentSetUp.BusinessUnitID = oItem.BusinessUnitID;
                            _oDepartmentSetUp.LocationID = 0;
                            _oDepartmentSetUp.DataType = 1;
                            _oDepartmentSetUps.Add(_oDepartmentSetUp);
                            this.MapLocation(oItem.BusinessUnitID, nid, true);
                        }
                        catch
                        {
                            nBUID = 0;
                        }
                    }
                }
                #endregion

                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp = GetRoot();
                _oDepartmentSetUp.DetailsInfo = "";
                _oDepartmentSetUp.EmployeeCount = 0;
                this.AddTreeNodes(ref _oDepartmentSetUp);
                //_oDepartmentSetUp.DRPPs = DepartmentRequirementPolicyPermission.Gets("SELECT * FROM  DepartmentRequirementPolicyPermission WHERE USERID=" + ((int)(Session[SessionInfo.currentUserID])).ToString(), (int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                _oDepartmentSetUp = new DepartmentSetUp();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oDepartmentSetUp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveDRPDesignation(List<DepartmentRequirementDesignation> oDRPDesigs)
        {
            string sFeedbackmessage = "";
            DepartmentRequirementDesignation oDepartmentRequirementPolicy = new DepartmentRequirementDesignation();
            try
            {
                sFeedbackmessage = oDepartmentRequirementPolicy.SaveDRPDesignations(oDRPDesigs, (int)(Session[SessionInfo.currentUserID]));
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
        public JsonResult RemoveDRPDesignation(int PID, int CID, bool IsDesig)
        {
            string sFeedbackmessage = "";
            string sSql = "";
            if (IsDesig) { sSql = "DELETE FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" + PID + " AND DesignationID=" + CID; }
            else { sSql = "DELETE FROM DepartmentRequirementDesignation WHERE DepartmentRequirementPolicyID=" + PID + " AND ShiftID=" + CID; }
            List<DepartmentRequirementDesignation> oDepartmentRequirementPolicys = new List<DepartmentRequirementDesignation>();
            DepartmentRequirementDesignation oDepartmentRequirementPolicy = new DepartmentRequirementDesignation();
            try
            {
                oDepartmentRequirementPolicys = DepartmentRequirementDesignation.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                sFeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private void MapLocation(int nBusinessUnitID, int nParentID, bool bIsPermitted = false)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            oDepartmentRequirementPolicys = this.GetLocationByBU(nBusinessUnitID);
            foreach (DepartmentRequirementPolicy oItem in oDepartmentRequirementPolicys)
            {
                if (bIsPermitted && _oDepartmentRequirementPolicyPermissions.Count() > 0)
                {
                    int nLocationID = 0;
                    try
                    {
                        nLocationID = (_oDepartmentRequirementPolicyPermissions.Where(x => x.LocationID == oItem.LocationID).ToList())[0].LocationID;
                        if (nLocationID == 0) throw new Exception();
                        int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                        _oDepartmentSetUp = new DepartmentSetUp();
                        _oDepartmentSetUp.id = nid;
                        _oDepartmentSetUp.parentid = nParentID;
                        _oDepartmentSetUp.text = oItem.LocationName + " @ Location";
                        _oDepartmentSetUp.state = "closed";
                        _oDepartmentSetUp.attributes = "";
                        _oDepartmentSetUp.DetailsInfo = " Total Employee  : " + oItem.EmployeeCount.ToString();
                        _oDepartmentSetUp.EmployeeCount = oItem.EmployeeCount;
                        _oDepartmentSetUp.LocationID = 0;
                        _oDepartmentSetUp.DeptReqPolicyID = oItem.DepartmentRequirementPolicyID;
                        _oDepartmentSetUp.DataType = 2;
                        _oDepartmentSetUps.Add(_oDepartmentSetUp);
                        this.MapDepartment(nBusinessUnitID, oItem.LocationID, nid, true);
                    }
                    catch
                    {
                        nLocationID = 0;
                    }
                }
                else
                {
                    int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                    _oDepartmentSetUp = new DepartmentSetUp();
                    _oDepartmentSetUp.id = nid;
                    _oDepartmentSetUp.parentid = nParentID;
                    _oDepartmentSetUp.text = oItem.LocationName + " @ Location";
                    _oDepartmentSetUp.state = "closed";
                    _oDepartmentSetUp.attributes = "";
                    _oDepartmentSetUp.DetailsInfo = " Total Employee  : " + oItem.EmployeeCount.ToString();
                    _oDepartmentSetUp.EmployeeCount = oItem.EmployeeCount;
                    _oDepartmentSetUp.LocationID = 0;
                    _oDepartmentSetUp.DeptReqPolicyID = oItem.DepartmentRequirementPolicyID;
                    _oDepartmentSetUp.DataType = 2;
                    _oDepartmentSetUps.Add(_oDepartmentSetUp);
                    this.MapDepartment(nBusinessUnitID, oItem.LocationID, nid);
                }

            }
        }
        private List<DepartmentRequirementPolicy> GetLocationByBU(int nBUID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            List<DepartmentRequirementPolicy> oLocations = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementPolicys.ForEach(x => oLocations.Add(x));
            oLocations = oLocations.Where(x => x.BusinessUnitID == nBUID).ToList();
            oLocations = oLocations.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            return oLocations;
        }
        private void MapDepartment(int nBusinessUnitID, int nLocationID, int nParentID, bool bIsPermitted = false)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            oDepartmentRequirementPolicys = this.GetDepartmentByLocation(nBusinessUnitID, nLocationID);
            foreach (DepartmentRequirementPolicy oItem in oDepartmentRequirementPolicys)
            {
                if (bIsPermitted && _oDepartmentRequirementPolicyPermissions.Count() > 0)
                {
                    int nDepartmentID = 0;
                    try
                    {
                        nDepartmentID = (_oDepartmentRequirementPolicyPermissions.Where(x => x.DepartmentID == oItem.DepartmentID).ToList())[0].DepartmentID;
                        if (nDepartmentID == 0) throw new Exception();
                        int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                        _oDepartmentSetUp = new DepartmentSetUp();
                        _oDepartmentSetUp.id = nid;
                        _oDepartmentSetUp.parentid = nParentID;
                        _oDepartmentSetUp.text = oItem.DepartmentName + " @ Department";
                        _oDepartmentSetUp.state = "closed";
                        _oDepartmentSetUp.attributes = "";
                        _oDepartmentSetUp.DetailsInfo = " Total Employee  : " + oItem.EmployeeCount.ToString();
                        _oDepartmentSetUp.EmployeeCount = oItem.EmployeeCount;
                        _oDepartmentSetUp.LocationID = 0;
                        _oDepartmentSetUp.DeptReqPolicyID = oItem.DepartmentRequirementPolicyID;
                        _oDepartmentSetUp.DepartmentID = oItem.DepartmentID;
                        _oDepartmentSetUp.DataType = 3;
                        _oDepartmentSetUps.Add(_oDepartmentSetUp);
                    }
                    catch
                    {
                        nDepartmentID = 0;
                    }
                }
                else
                {
                    int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                    _oDepartmentSetUp = new DepartmentSetUp();
                    _oDepartmentSetUp.id = nid;
                    _oDepartmentSetUp.parentid = nParentID;
                    _oDepartmentSetUp.text = oItem.DepartmentName + " @ Department";
                    _oDepartmentSetUp.state = "closed";
                    _oDepartmentSetUp.attributes = "";
                    _oDepartmentSetUp.DetailsInfo = " Total Employee  : " + oItem.EmployeeCount.ToString();
                    _oDepartmentSetUp.EmployeeCount = oItem.EmployeeCount;
                    _oDepartmentSetUp.LocationID = 0;
                    _oDepartmentSetUp.DeptReqPolicyID = oItem.DepartmentRequirementPolicyID;
                    _oDepartmentSetUp.DataType = 3;
                    _oDepartmentSetUps.Add(_oDepartmentSetUp);
                }
            }
        }
        private List<DepartmentRequirementPolicy> GetDepartmentByLocation(int nBusinessUnitID, int nLocationID)
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

        #region Save
        [HttpPost]
        public JsonResult ConfirmDRPPermission(DepartmentRequirementPolicyPermission oDRPP)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sSuccessfullmessage = "";
            string sjson = "";
            try
            {
                if (oDRPP.Keys != null && oDRPP.Keys != "")
                {
                    oDRPP.Keys = oDRPP.Keys.Remove(oDRPP.Keys.Length - 1, 1);
                }
                if (oDRPP.ConfirmDRPPermission(oDRPP.UserID, oDRPP.Keys, (int)Session[SessionInfo.currentUserID]))
                {
                    sSuccessfullmessage = "Data save successfully";
                    sjson = serializer.Serialize((object)sSuccessfullmessage);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
                sjson = serializer.Serialize((object)sSuccessfullmessage);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                sjson = serializer.Serialize((object)ex.Message);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Save

        #region Searching(BU, Loc, Dept, Emp) With DRP Permission by AKRAM 24-09-2019
        #region Search BusinessUnit
        [HttpPost]
        public JsonResult GetsBUWithDRP(BusinessUnit oBusinessUnit)
        {
            BusinessUnit _oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
            string sSQL = "";
            try
            {
                sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
                }
                sSQL = sSQL + ")";
                _oBusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                _oBusinessUnit = new BusinessUnit();
                _oBusinessUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBusinessUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Search Location
        [HttpPost]
        public JsonResult GetsLocationMenuTreeWithDRP(Location oLocation)
        {
            Location oTempLocation = new Location();
            oTempLocation = oTempLocation.Get(oLocation.LocationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            TLocation _oTLocation = new TLocation();
            oTempLocation.TLocation = this.GetTLocation(oLocation.BusinessUnitIDs);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oTempLocation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public TLocation GetTLocation(string BusinessUnitIDs)
        {
            _oLocations = new List<Location>();
            _oLocation = new Location();
            _oTLocation = new TLocation();
            _oTLocations = new List<TLocation>();
            try
            {
                string sSql = "SELECT * FROM View_Location WHERE LocationID IN(SELECT LocationID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0 ";
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
                }
                if (BusinessUnitIDs != "" && BusinessUnitIDs != "0" && BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + BusinessUnitIDs + ")"; }
                sSql = sSql + ") OR ParentID=0  Order By LocationID";
                _oLocations = Location.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //_oLocations = Location.GetsAll(((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (Location oItem in _oLocations)
                {
                    _oTLocation = new TLocation();
                    _oTLocation.id = oItem.LocationID;
                    _oTLocation.parentid = oItem.ParentID;
                    _oTLocation.text = oItem.Name;
                    _oTLocation.attributes = "";
                    _oTLocation.Description = oItem.Description;
                    _oTLocations.Add(_oTLocation);
                }
                _oTLocation = new TLocation();
                _oTLocation = GetLocationRoot();
                this.AddTreeNodes(ref _oTLocation);
                return _oTLocation;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return _oTLocation;
            }
        }
        private TLocation GetLocationRoot()
        {
            TLocation oTLocation = new TLocation();
            foreach (TLocation oItem in _oTLocations)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oTLocation;
        }
        private void AddTreeNodes(ref TLocation oTLocation)
        {
            IEnumerable<TLocation> oChildNodes;
            oChildNodes = GetChildLocation(oTLocation.id);
            oTLocation.children = oChildNodes;

            foreach (TLocation oItem in oChildNodes)
            {
                TLocation oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private IEnumerable<TLocation> GetChildLocation(int nLocationID)
        {
            List<TLocation> oTLocations = new List<TLocation>();
            foreach (TLocation oItem in _oTLocations)
            {
                if (oItem.parentid == nLocationID)
                {
                    oTLocations.Add(oItem);
                }
            }
            return oTLocations;
        }
        #endregion
        #region Department
        [HttpPost]
        public JsonResult GetsDepartmentWithDRP(User oUser)
        {
            Company oCompany = new Company();
            _oLocations = new List<Location>();
            _oDepartmentSetUp = new DepartmentSetUp();
            _oDepartmentSetUps = new List<DepartmentSetUp>();
            _oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            _oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();
            string _sBUIDs = oUser.ErrorMessage.Split('~')[0];
            string _sLocationIDs = oUser.ErrorMessage.Split('~')[1];
            string _sDepartmentIDs = oUser.ErrorMessage.Split('~')[2];

            try
            {
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                string sSql = "SELECT  * FROM View_DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT  MIN(DepartmentRequirementPolicyID) FROM DepartmentRequirementPolicy GROUP BY BusinessUnitID)";
                _oDRPBusinessUnits = DepartmentRequirementPolicy.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
                _oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets((int)(Session[SessionInfo.currentUserID]));
                sSql = "SELECT * FROM View_DepartmentRequirementPolicyPermission WHERE UserID = " + (int)(Session[SessionInfo.currentUserID]);
                if (!string.IsNullOrEmpty(_sBUIDs)) sSql = sSql + " AND BusinessUnitID IN (" + _sBUIDs + ")";
                if (!string.IsNullOrEmpty(_sLocationIDs)) sSql = sSql + " AND LocationID IN (" + _sLocationIDs + ")";
                if (!string.IsNullOrEmpty(_sDepartmentIDs)) sSql = sSql + " AND DepartmentID IN (" + _sDepartmentIDs + ")";

                _oDepartmentRequirementPolicyPermissions = DepartmentRequirementPolicyPermission.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

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
                if (_oDepartmentRequirementPolicyPermissions.Count > 0)
                {
                    foreach (DepartmentRequirementPolicy oItem in _oDRPBusinessUnits)
                    {
                        int nBUID = 0;
                        try
                        {
                            nBUID = (_oDepartmentRequirementPolicyPermissions.Where(x => x.BusinessUnitID == oItem.BusinessUnitID).ToList())[0].BusinessUnitID;
                            if (nBUID == 0) throw new Exception();
                            int nid = _oDepartmentSetUps[_oDepartmentSetUps.Count - 1].id + 1;
                            _oDepartmentSetUp = new DepartmentSetUp();
                            _oDepartmentSetUp.id = nid;
                            _oDepartmentSetUp.parentid = 1;
                            _oDepartmentSetUp.text = oItem.BUName + " @ BU";
                            _oDepartmentSetUp.state = "";
                            _oDepartmentSetUp.attributes = "";
                            _oDepartmentSetUp.DetailsInfo = "";
                            _oDepartmentSetUp.EmployeeCount = 0;
                            _oDepartmentSetUp.BusinessUnitID = oItem.BusinessUnitID;
                            _oDepartmentSetUp.LocationID = 0;
                            _oDepartmentSetUp.DataType = 1;
                            _oDepartmentSetUps.Add(_oDepartmentSetUp);
                            this.MapLocation(oItem.BusinessUnitID, nid, true);
                        }
                        catch
                        {
                            nBUID = 0;
                        }
                    }
                }
                #endregion

                _oDepartmentSetUp = new DepartmentSetUp();
                _oDepartmentSetUp = GetRoot();
                _oDepartmentSetUp.DetailsInfo = "";
                _oDepartmentSetUp.EmployeeCount = 0;
                this.AddTreeNodes(ref _oDepartmentSetUp);

                _oDepartmentSetUp.DRPPs = DepartmentRequirementPolicyPermission.Gets("SELECT * FROM  DepartmentRequirementPolicyPermission WHERE USERID=" + oUser.UserID, (int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                _oDepartmentSetUp = new DepartmentSetUp();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oDepartmentSetUp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}
