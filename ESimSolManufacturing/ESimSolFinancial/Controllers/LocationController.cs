using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using System.Data;
using System.Data.OleDb;

namespace ESimSolFinancial.Controllers
{
    public class LocationController : Controller
    {
        #region Declaration
        Location _oLocation = new Location();
        List<Location> _oLocations = new List<Location>();
        TLocation _oTLocation = new TLocation();
        List<TLocation> _oTLocations = new List<TLocation>();
        string _sErrorMessage = "";
        string _sSQL = "";
        #endregion

        #region Functions
        private TLocation GetRoot()
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

        private IEnumerable<TLocation> GetChild(int nLocationID)
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

        private void AddTreeNodes(ref TLocation oTLocation)
        {
            IEnumerable<TLocation> oChildNodes;
            oChildNodes = GetChild(oTLocation.id);
            oTLocation.children = oChildNodes;

            foreach (TLocation oItem in oChildNodes)
            {
                TLocation oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private bool ValidateInput(Location oLocation)
        {
            if (oLocation.Name == null || oLocation.Name == "")
            {
                _sErrorMessage = "Please enter Location Name";
                return false;
            }
            if (oLocation.ParentID <= 0)
            {
                _sErrorMessage = "Invalid Parent location try again";
                return false;
            }
            return true;
        }

        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_Location";
            string sLocCode = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sName = (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            //string sLocationIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";
            string sSQL = "";


            #region LocCode
            if (sLocCode != null)
            {
                if (sLocCode != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " LocCode LIKE '%" + sLocCode + "%' ";
                }
            }
            #endregion
            #region Name
            if (sName != null)
            {
                if (sName != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " Name LIKE '%" + sName + "%' ";
                }
            }
            #endregion
            


            if (sSQL != "")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " LocationID != 1 AND LocationType = " + (int)_oLocation.LocationType + " AND ParentID= " + _oLocation.ParentID;
                _sSQL = _sSQL + sSQL; 
            }
        }
        #endregion

        #region New Task
        public ActionResult ViewLocations(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUID = buid;
            _oLocations = new List<Location>();
            _oLocation = new Location();
            _oTLocation = new TLocation();
            _oTLocations = new List<TLocation>();
            try
            {
                _oLocations = Location.GetsAll(((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (Location oItem in _oLocations)
                {

                    _oTLocation = new TLocation();
                    _oTLocation.id = oItem.LocationID;
                    _oTLocation.parentid = oItem.ParentID;
                    _oTLocation.text = oItem.Name;
                    _oTLocation.attributes = "";
                    _oTLocation.code = oItem.LocCode;
                    _oTLocation.Description = oItem.Description;
                    _oTLocation.Activity = oItem.Activity;
                    _oTLocation.LocationType = oItem.LocationType;
                    _oTLocation.LocationTypeName = oItem.LocationTypeName;
                    _oTLocations.Add(_oTLocation);
                }
                _oTLocation = new TLocation();
                _oTLocation = GetRoot();
                this.AddTreeNodes(ref _oTLocation);
                List<TLocation> oTLocations = new List<TLocation>();
                oTLocations.Add(_oTLocation);
                return View(oTLocations);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                List<TLocation> oTLocations = new List<TLocation>();
                oTLocations.Add(_oTLocation);
                return View(oTLocations);
            }
        }
        public ActionResult ViewManageLocations(string gfdb, int lttl, int menuid)
        {

            if (gfdb == null)
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
                this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser("'Location'", ((User)Session[SessionInfo.CurrentUser]).UserID, ((User)Session[SessionInfo.CurrentUser]).UserID));
                _oLocations = new List<Location>();
                _oLocations = Location.GetsByType((EnumLocationType)lttl, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.LT = (EnumLiabilityType)lttl;
            return View(_oLocations);
        }
        public ActionResult ViewManageLocation(int id)
        {
            _oLocation = new Location();
            if (id > 0)
            {
                _oLocation = _oLocation.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oLocation);
        }
        public ActionResult ViewLocation(int id)
        {
            Location oTempLocation = new Location();
            oTempLocation = oTempLocation.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Location oLocation = new Location();
            oLocation.ParentID = oTempLocation.LocationID;
            oLocation.LocationType = (EnumLocationType)(((int)oTempLocation.LocationType) + 1);
            oLocation.ParentNodeName = "Selected parent location are : " + oTempLocation.Name + "[" + oTempLocation.LocCode + "]";
            string sSQL = "SELECT * FROM View_Location WHERE ParentID=" + oTempLocation.LocationID;
            oLocation.ChildNodes = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COS = oTempClientOperationSetting;

            return View(oLocation);
        }
        public ActionResult EditLocation(int id)
        {
            _oLocation = new Location();
            _oLocation = _oLocation.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oLocation);
        }


        [HttpPost]
        public JsonResult Save(Location oLocation)
        {
            _oLocation = new Location();
            try
            {
                _oLocation = oLocation;
                _oLocation = _oLocation.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLocation = new Location();
                _oLocation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLocation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(Location oLocation)
        {
            _oLocation = new Location();
            try
            {
                _oLocation = oLocation;
                _oLocation = _oLocation.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLocation = new Location();
                _oLocation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLocation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getchildren(int parentid)
        {
            _oLocations = new List<Location>();
            try
            {
                string sSQL = "SELECT * FROM View_Location WHERE ParentID=" + parentid.ToString();
                _oLocations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oLocations = new List<Location>();
                _oLocation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Location oLocation)
        {
            string sfeedbackmessage = "";
            _oLocation = new Location();
            try
            {
                sfeedbackmessage = _oLocation.Delete(oLocation.LocationID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sfeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        

        #endregion

        #region Picker
        public ActionResult LocationPiker()
        {

            Location oLocation = new Location();
            oLocation.Locations = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(oLocation);
            //oLocation.Locations = Location.GetsForAUI(((User)Session[SessionInfo.CurrentUser]).UserID); 
            //oProduct.Products = Product.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);            
        }

        // Modified on 26 Feb 2013 by Fauzul as required Combo on Product Unique Identification
        [HttpGet]
        public JsonResult LoadComboLocationList()
        {
            List<Location> oLocations = new List<Location>();
            Location oLocation = new Location();
            oLocation.Name = "-- Select Location --";
            oLocations.Add(oLocation);
            oLocations.AddRange(Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadComboLocationListbyID(int id)
        {
            List<Location> oLocations = new List<Location>();
            Location oLocation = new Location();
            oLocation.Name = "-- Select Location --";
            oLocations.Add(oLocation);
            string sSQL = "SELECT * FROM View_Location WHERE LocationID != " + id + " AND IsActive = 1";
            oLocation.Locations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oLocations.AddRange(Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActiveInactive(TLocation oTLocation)
        {
            _oLocation = new Location();
            string sErrorMease = "";
            try
            {
                _oLocation.LocationID = oTLocation.id;
                _oLocation.LocCode = oTLocation.code;
                _oLocation.Name = oTLocation.text;
                _oLocation.Description = oTLocation.Description;
                _oLocation.ParentID = oTLocation.parentid;
                _oLocation.IsActive = (oTLocation.IsActive == true ? false : true);
                _oLocation = _oLocation.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLocation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public JsonResult GetsByCodeOrName(Location oLocation)
        {
            List<Location> oLocations = new List<Location>();
            oLocations = Location.GetsByCodeOrName(oLocation, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsLocationByBU(Location oLocation)
        {
            List<Location> oLocations = new List<Location>();
            string sSql = "select * from Location where LocationID in (select LocationID  from View_DepartmentRequirementPolicy where BusinessUnitID = " + oLocation.BusinessUnitID +")";
            oLocations = Location.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByCodeOrNamePick(Location oLocation)
        {
            List<Location> oLocations = new List<Location>();
            oLocations = Location.GetsByCodeOrNamePick(oLocation, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByCode(Location oLocation)
        {
            List<Location> oLocations = new List<Location>();
            oLocations = Location.GetsByCode(oLocation, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsByParentID(Location oLocation) //use in BOQsssssssssssssss
        {
            List<Location> oLocations = new List<Location>();
            string sSQL = "";
            if(oLocation.ParentID>0)
            {
                if (oLocation.BusinessUnitID > 0)
                {
                    sSQL = "SELECT * FROM View_Location as L WHERE L.ParentID=" + oLocation.ParentID
                    + " AND L.LocationID IN (SELECT LocationID FROM BusinessLocation WHERE BusinessUnitID = " + oLocation.BusinessUnitID + ")"
                    + " AND L.LocationNameCode LIKE '%" + (oLocation.LocCode == null ? "" : oLocation.LocCode) + "%'";
                }
                else
                {
                    sSQL = "SELECT * FROM View_Location as L WHERE L.ParentID=" + oLocation.ParentID
                    + " AND L.LocationNameCode LIKE '%" + (oLocation.LocCode == null ? "" : oLocation.LocCode) + "%'";
                }
                oLocations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Gets(Location oLocation)
        {
            _oLocation = new Location();
            _oLocation = oLocation;
            List<Location> oLocations = new List<Location>();
            this.MakeSQL(oLocation.ErrorMessage);
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region Import From Excel
        private List<Location> GetLocationsFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory= "";
            List<Location> oLocations = new List<Location>();
            Location oLocation = new Location();
            List<Location> oLocs = new List<Location>();
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


                    string query = string.Format("Select * from [{0}]", "Locations$");
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    for (int i = 0; i < oRows.Count; i++)
                    {

                        oLocation = new Location();
                        oLocation.LocationType = _oLocation.LocationType;
                        oLocation.Name = Convert.ToString(oRows[i]["Name"] == DBNull.Value ? "" : oRows[i]["Name"]);
                        oLocation.ShortName = Convert.ToString(oRows[i]["ShortName"] == DBNull.Value ? "" : oRows[i]["ShortName"]);
                        if (oLocation.Name == "" || oLocation.ShortName == "")
                        {
                            continue;
                        }
                        oLocation.Description = Convert.ToString(oRows[i]["Description"] == DBNull.Value ? "" : oRows[i]["Description"]);
                        if (_oLocation.LocationType == EnumLocationType.Area)
                        {
                            oLocation.ParentID = 1;
                        }
                        else if (_oLocation.LocationType == EnumLocationType.Zone)
                        {
                            if (oRows[i]["AreaCode"] == DBNull.Value)
                            {
                                throw new Exception("Area not found.");
                            }
                            else
                            {
                                oLocs = new List<Location>();
                                oLocs = Location.Gets("SELECT * FROM View_Location WHERE LocationType=" + (((int)_oLocation.LocationType) - 1) + " AND LocCode LIKE '%" + Convert.ToString(oRows[i]["AreaCode"]) + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                                if (oLocs.Count <= 0) { throw new Exception("Area not found."); }
                                oLocation.ParentID = oLocs[0].LocationID;
                            }
                        }
                        else if (_oLocation.LocationType == EnumLocationType.Site)
                        {
                            if (oRows[i]["ZoneCode"] == DBNull.Value)
                            {
                                throw new Exception("Zone not found.");
                            }
                            else
                            {
                                oLocs = new List<Location>();
                                oLocs = Location.Gets("SELECT * FROM View_Location WHERE LocationType=" + (((int)_oLocation.LocationType) - 1) + " AND LocCode LIKE '%" + Convert.ToString(oRows[i]["ZoneCode"]) + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                                if (oLocs.Count <= 0) { throw new Exception("Zone not found."); }
                                oLocation.ParentID = oLocs[0].LocationID;
                            }
                        }
                        oLocation.IsActive = Convert.ToBoolean(oRows[i]["ActiveInActive"] == DBNull.Value ? false : oRows[i]["ActiveInActive"]);

                        oLocations.Add(oLocation);
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
            return oLocations;
        }
        public ActionResult ImportFromExcel()
        {
            return View(new Location());
        }
        [HttpPost]
        public ActionResult ImportFromExcel(HttpPostedFileBase fileLocations, Location oLocation)
        {
            List<Location> oLocations = new List<Location>();
            _oLocation = new Location();
            _oLocation = oLocation;
            Location oLoc = new Location();

            try
            {
                if (fileLocations == null) { throw new Exception("File not Found"); }
                oLocations = this.GetLocationsFromExcel(fileLocations);
                foreach (Location oItem in oLocations)
                {
                    oLoc= oItem.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oLoc.ErrorMessage != "")
                    {
                        ViewBag.FeedBack = "Error for Location Name: " + oItem.Name + ". " + oLoc.ErrorMessage;
                        return View(oLoc);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View(new Location());
            }
            return RedirectToAction("ViewManageLocations", new {lttl=(int)_oLocation.LocationType, menuid = (int)Session[SessionInfo.MenuID] });
        }
        //public ActionResult DownloadFormat(int ift)
        //{
        //    ImportFormat oImportFormat = new ImportFormat();
        //    try
        //    {

        //        oImportFormat = ImportFormat.GetByType((EnumImportFormatType)ift, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        if (oImportFormat.AttatchFile != null)
        //        {
        //            var file = File(oImportFormat.AttatchFile, oImportFormat.FileType);
        //            file.FileDownloadName = oImportFormat.AttatchmentName;
        //            return file;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        throw new HttpException(404, "Couldn't find " + oImportFormat.AttatchmentName);
        //    }
        //}
        #endregion

        #region Location Picker

        public ActionResult LocationPickerWithCheckBox(int id)
        {
            try
            {
                Location oLocation = new Location();
                oLocation = oLocation.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                return PartialView(oLocation);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return PartialView(_oLocation);
            }
        }

        [HttpPost]
        public JsonResult GetsLocationMenuTree(Location oLocation)
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
                string sSql="SELECT * FROM View_Location WHERE LocationID IN(SELECT LocationID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0 ";
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
                }
                if (BusinessUnitIDs != "" && BusinessUnitIDs != "0" && BusinessUnitIDs != null) { sSql = sSql + " AND BusinessUnitID IN(" + BusinessUnitIDs + ")"; }
                sSql=sSql+ ") OR ParentID=0  Order By LocationID";
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



        private IEnumerable<TLocation> GetLocationChild(int nid)
        {
            List<TLocation> oTLocations = new List<TLocation>();
            foreach (TLocation oItem in _oTLocations)
            {
                if (oItem.parentid == nid)
                {
                    if ((((User)Session[SessionInfo.CurrentUser]).IsSuperUser) || ((User)Session[SessionInfo.CurrentUser]).IsPermitted(oItem.id))
                    {
                        oTLocations.Add(oItem);
                    }
                }
            }
            return oTLocations;
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
        #endregion

    }



}
