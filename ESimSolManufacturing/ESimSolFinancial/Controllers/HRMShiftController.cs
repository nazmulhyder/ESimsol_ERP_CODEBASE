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

namespace ESimSolFinancial.Controllers
{
    public class HRMShiftController : Controller
    {
        #region Declartion
        HRMShift _oHRMShift = new HRMShift();
        List<HRMShift> _oHRMShifts = new List<HRMShift>();
        BUPermission _oBUPermission = new BUPermission();
        List<BUPermission> _oBUPermissions = new List<BUPermission>();
        #endregion

        public ActionResult ViewHRMShifts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oHRMShifts = new List<HRMShift>();

            _oHRMShifts = HRMShift.Gets("SELECT * FROM HRM_Shift ORDER BY Name, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return View(_oHRMShifts);
        }

        public ActionResult ViewHRMShift(int id)
        {

            _oHRMShift = new HRMShift();
            if (id > 0)
            {
                _oHRMShift = _oHRMShift.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(_oHRMShift);
        }
        public ActionResult View_HRMShift_V1(int id)
        {

            _oHRMShift = new HRMShift();
            if (id > 0)
            {
                _oHRMShift = _oHRMShift.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return View(_oHRMShift);
        }
        [HttpPost]
        public JsonResult Save(HRMShift oHRMShift)
        {
            _oHRMShift = new HRMShift();
            oHRMShift.IsLeaveOnOFFHoliday = true;
            try
            {
                _oHRMShift = oHRMShift.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oHRMShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Copy
        [HttpPost]
        public JsonResult Copy(HRMShift oHRMShift)
        {
            List<ShiftBreakSchedule> oShiftBreakSchedules = new List<ShiftBreakSchedule>();
            List<ShiftOTSlab> oShiftOTSlabs = new List<ShiftOTSlab>();
            try
            {
                _oHRMShift = _oHRMShift.Get(oHRMShift.ShiftID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oHRMShift.ShiftBreakSchedules = ShiftBreakSchedule.Gets("SELECT * FROM View_ShiftBreakSchedule WHERE ShiftID=" + oHRMShift.ShiftID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                oShiftBreakSchedules = _oHRMShift.ShiftBreakSchedules;
                foreach (ShiftBreakSchedule oItem in oShiftBreakSchedules)
                {
                    oItem.ShiftBScID = 0;
                }

                _oHRMShift.ShiftOTSlabs = ShiftOTSlab.Gets("SELECT * FROM ShiftOTSlab WHERE ShiftID=" + oHRMShift.ShiftID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                oShiftOTSlabs = _oHRMShift.ShiftOTSlabs;
                foreach (ShiftOTSlab oItem in oShiftOTSlabs)
                {
                    oItem.ShiftOTSlabID = 0;
                }

                _oHRMShift.Name = "";
                _oHRMShift.Code= "";
                _oHRMShift.ShiftID = 0;
                _oHRMShift.MaxOTComplianceInMin = 0;
                _oHRMShift.CompMaxEndTime = DateTime.Now;
                _oHRMShift = _oHRMShift.Copy(((User)(Session[SessionInfo.CurrentUser])).UserID);
                

            }
            catch (Exception ex)
            {
                _oHRMShift = new HRMShift();
                _oHRMShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        [HttpPost]
        public JsonResult ActiveInActive(HRMShift oHRMShift)
        {
            _oHRMShift = new HRMShift();
            try
            {
                _oHRMShift = _oHRMShift.Get(oHRMShift.ShiftID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oHRMShift.IsActive = oHRMShift.IsActive;
                _oHRMShift = _oHRMShift.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oHRMShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult HRMShiftInActive(int nShiftID, int ntRShiftID)
        {
            _oHRMShift = new HRMShift();
            try
            {
                _oHRMShift = _oHRMShift.ShiftInActive( nShiftID,  ntRShiftID,((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oHRMShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HRMShiftActive(int nShiftID)
        {
            _oHRMShifts = new List<HRMShift>();
            _oHRMShift = new HRMShift();
            try
            {
                _oHRMShifts = HRMShift.Gets("UPDATE HRM_Shift  SET Isactive=1 WHERE ShiftID=" + nShiftID + " SELECT * FROM HRM_Shift WHERE ShiftID=" + nShiftID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oHRMShifts.Count > 0) { _oHRMShift = _oHRMShifts[0]; }
            }
            catch (Exception ex)
            {
                _oHRMShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(HRMShift oHRMShift)
        {
            string sErrorMease = "";
            try
            {
                _oHRMShift = new HRMShift();
                sErrorMease = _oHRMShift.Delete(oHRMShift.ShiftID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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
        public JsonResult GetShiftByDepartment(string sDepartmentIDs)
        {


            _oHRMShifts = new List<HRMShift>();

            string sSql = "";

            if (sDepartmentIDs !="")
            {
                sSql = "SELECT * FROM HRM_Shift WHERE ShiftID IN (SELECT ShiftID FROM DepartmentRequirementPolicy WHERE DepartmentID IN ("+sDepartmentIDs+"))";

            }
            
            try
            {
                _oHRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (_oHRMShifts.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

            }
            catch (Exception ex)
            {
                _oHRMShift = new HRMShift();
                _oHRMShifts = new List<HRMShift>();
                _oHRMShift.ErrorMessage = ex.Message;
                _oHRMShifts.Add(_oHRMShift);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShifts);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetsShifts(HRMShift oHRMShift)
        {
            List<HRMShift> oHRMShifts = new List<HRMShift>();
            oHRMShifts = HRMShift.Gets("SELECT * FROM HRM_Shift WHERE IsActive=1", (int)(Session[SessionInfo.currentUserID]));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHRMShifts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getHRMShift(HRMShift oHRMShift)
        {
            try
            {
                _oHRMShift = new HRMShift();
                _oHRMShift = _oHRMShift.Get(oHRMShift.ShiftID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oHRMShifts = new List<HRMShift>();
                _oHRMShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Shift Break Name
        public ActionResult View_ShiftBreakNames(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ShiftBreakName>  oShiftBreakNames = new List<ShiftBreakName>();
            oShiftBreakNames = ShiftBreakName.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oShiftBreakNames);
        }

        [HttpPost]
        public JsonResult ShiftBreakName_IU(ShiftBreakName oShiftBreakName)
        {
            try
            {
                if (oShiftBreakName.ShiftBNID > 0)
                {
                    oShiftBreakName = oShiftBreakName.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oShiftBreakName = oShiftBreakName.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oShiftBreakName = new ShiftBreakName();
                oShiftBreakName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakName);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShiftBreakName_Delete(ShiftBreakName oShiftBreakName)
        {
            try
            {
                oShiftBreakName = oShiftBreakName.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftBreakName = new ShiftBreakName();
                oShiftBreakName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakName.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShiftBreakName_Activity(ShiftBreakName oShiftBreakName)
        {
            try
            {
                oShiftBreakName = ShiftBreakName.Activite(oShiftBreakName.ShiftBNID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftBreakName = new ShiftBreakName();
                oShiftBreakName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakName);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getsShiftBreakName(ShiftBreakName oShiftBreakName)
        {
            List<ShiftBreakName> oShiftBreakNames = new List<ShiftBreakName>();
            try
            {
                oShiftBreakNames = ShiftBreakName.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftBreakNames = new List<ShiftBreakName>();
                oShiftBreakName = new ShiftBreakName();
                oShiftBreakName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakNames);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion Shift Break Name

        #region Shift Break Schedule

        [HttpPost]
        public JsonResult ShiftBreakSchedule_IU(ShiftBreakSchedule oShiftBreakSchedule)
        {
            try
            {
                if (oShiftBreakSchedule.ShiftBScID > 0)
                {
                    oShiftBreakSchedule = oShiftBreakSchedule.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oShiftBreakSchedule = oShiftBreakSchedule.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oShiftBreakSchedule = new ShiftBreakSchedule();
                oShiftBreakSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShiftBreakSchedule_Delete(ShiftBreakSchedule oShiftBreakSchedule)
        {
            try
            {
                oShiftBreakSchedule = oShiftBreakSchedule.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftBreakSchedule = new ShiftBreakSchedule();
                oShiftBreakSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakSchedule.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShiftBreakSchedule_Activity(ShiftBreakSchedule oShiftBreakSchedule)
        {
            try
            {
                oShiftBreakSchedule = ShiftBreakSchedule.Activite(oShiftBreakSchedule.ShiftBScID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftBreakSchedule = new ShiftBreakSchedule();
                oShiftBreakSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getsShiftBreakSchedule(ShiftBreakSchedule oShiftBreakSchedule)
        {
            List<ShiftBreakSchedule> oShiftBreakSchedules = new List<ShiftBreakSchedule>();
            try
            {
                string sSql = "SELECT * FROM View_ShiftBreakSchedule WHERE ShiftID=" + oShiftBreakSchedule.ShiftID;
                oShiftBreakSchedules = ShiftBreakSchedule.Gets(sSql,((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftBreakSchedules = new List<ShiftBreakSchedule>();
                oShiftBreakSchedule = new ShiftBreakSchedule();
                oShiftBreakSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBreakSchedules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Shift Break Schedule

        #region Print
        public ActionResult PrintShift(string sShiftIDs, double ts)
        {
            _oHRMShift = new HRMShift();
            string sSql = "SELECT * FROM HRM_Shift WHERE ShiftID IN(" + sShiftIDs + ")";
            _oHRMShift.HRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            string sSql_SBN = "SELECT * FROM View_ShiftBreakSchedule WHERE ShiftID IN(" + sShiftIDs + ")";
            _oHRMShift.ShiftBreakSchedules = ShiftBreakSchedule.Gets(sSql_SBN, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oHRMShift.Company = oCompanys.First();
            _oHRMShift.Company.CompanyLogo = GetImage(_oHRMShift.Company.OrganizationLogo);

            rptHRMShift oReport = new rptHRMShift();
            byte[] abytes = oReport.PrepareReport(_oHRMShift);
            return File(abytes, "application/pdf");
        }
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
        #endregion Print

        #region XL
        public ActionResult PrintHRMShiftXL(double ts)
        {
            List<HRMShift> oHRMShifts = new List<HRMShift>();
            string sSql = "SELECT * FROM HRM_Shift  ORDER BY Code";

            oHRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<HRMShiftXL>));

            HRMShiftXL oHRMShiftXL = new HRMShiftXL();
            List<HRMShiftXL> oHRMShiftXLs = new List<HRMShiftXL>();

            int nCount = 0;
            foreach (HRMShift oItem in oHRMShifts)
            {
                nCount++;
                oHRMShiftXL = new HRMShiftXL();
                oHRMShiftXL.SL = nCount.ToString();
                oHRMShiftXL.Code = oItem.Code;
                oHRMShiftXL.Name = oItem.Name;
                oHRMShiftXL.StartTime = oItem.StartTimeInString;
                oHRMShiftXL.EndTime = oItem.EndTimeInString;

                oHRMShiftXLs.Add(oHRMShiftXL);
            }

            //serializer.Serialize(stream, oDTESs);
            serializer.Serialize(stream, oHRMShiftXLs);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "HRMShiftXL.xls");
        }
        #endregion XL

        #region Shift OT Slab

        [HttpPost]
        public JsonResult ShiftOTSlab_IU(ShiftOTSlab oShiftOTSlab)
        {
            try
            {
                if (oShiftOTSlab.ShiftOTSlabID > 0)
                {
                    oShiftOTSlab = oShiftOTSlab.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oShiftOTSlab = oShiftOTSlab.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oShiftOTSlab = new ShiftOTSlab();
                oShiftOTSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftOTSlab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShiftOTSlab_Delete(ShiftOTSlab oShiftOTSlab)
        {
            try
            {
                oShiftOTSlab = oShiftOTSlab.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftOTSlab = new ShiftOTSlab();
                oShiftOTSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftOTSlab.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShiftOTSlab_Activity(ShiftOTSlab oShiftOTSlab)
        {
            try
            {
                oShiftOTSlab = ShiftOTSlab.Activite(oShiftOTSlab.ShiftOTSlabID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftOTSlab = new ShiftOTSlab();
                oShiftOTSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftOTSlab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShiftOTSlab_ActivityComp(ShiftOTSlab oShiftOTSlab)
        {
            try
            {
                oShiftOTSlab = ShiftOTSlab.ActiviteComp(oShiftOTSlab.ShiftOTSlabID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftOTSlab = new ShiftOTSlab();
                oShiftOTSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftOTSlab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getsShiftOTSlab(ShiftOTSlab oShiftOTSlab)
        {
            List<ShiftOTSlab> oShiftOTSlabs = new List<ShiftOTSlab>();
            try
            {
                string sSql = "SELECT * FROM ShiftOTSlab WHERE ShiftID=" + oShiftOTSlab.ShiftID+" ORDER BY MinOTInMin ASC";
                oShiftOTSlabs = ShiftOTSlab.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oShiftOTSlabs = new List<ShiftOTSlab>();
                oShiftOTSlab = new ShiftOTSlab();
                oShiftOTSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftOTSlabs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Shift OT Slab

        #region ShiftBULocConfigure
        public ActionResult View_ShiftBULocConfigure(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sSql = "SELECT * FROM HRM_Shift WHERE IsActive=1";
            List<HRMShift> HRSs = new List<HRMShift>();
            HRSs = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            ViewBag.Locations = Location.Gets("SELECT * FROM Location WHERE LocationID NOT IN (SELECT ParentID FROM Location)", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return View(HRSs);
        }

        [HttpPost]
        public JsonResult ShiftBULocConfigure_IU(ShiftBULocConfigure oShiftBULocConfigure)
        {
            ShiftBULocConfigure _oShiftBULocConfigure = new ShiftBULocConfigure();
            try
            {
                _oShiftBULocConfigure = oShiftBULocConfigure;
                _oShiftBULocConfigure = _oShiftBULocConfigure.IUD(((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            catch (Exception ex)
            {
                _oShiftBULocConfigure = new ShiftBULocConfigure();
                _oShiftBULocConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShiftBULocConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsShiftsBULoc(ShiftBULocConfigure oShiftBULocConfigure)
        {
            List<ShiftBULocConfigure> oShiftBULocConfigures = new List<ShiftBULocConfigure>();
            oShiftBULocConfigures = ShiftBULocConfigure.Gets("SELECT * FROM View_ShiftBULocConfigure WHERE BUID="+oShiftBULocConfigure.BUID+" AND LocationID="+oShiftBULocConfigure.LocationID, (int)(Session[SessionInfo.currentUserID]));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShiftBULocConfigures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region BUPermission Actions
        public ActionResult ViewBUPermission(int id, double ts)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BUPermission).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            HRMShift oHRMShift = new ESimSol.BusinessObjects.HRMShift();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit.Name = "--Select Business Unit--";

            _oBUPermission = new BUPermission();
            oHRMShift = oHRMShift.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oBUPermission.BUPermissions = BUPermission.Gets("SELECT * FROM View_BUWiseShift WHERE ShiftID= " + id + " ", (int)Session[SessionInfo.currentUserID]);
            _oBUPermission.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            _oBUPermission.BusinessUnits.Add(oBusinessUnit);
            _oBUPermission.ShiftName = oHRMShift.Name;
            _oBUPermission.ShiftID = id;
            return View(_oBUPermission);
        }

        [HttpPost]
        public JsonResult SaveBUPermission(BUPermission oBUPermission)
        {
            _oBUPermission = new BUPermission();
            try 
            {
                _oBUPermission = oBUPermission;
                if (_oBUPermission.Remarks == null) { _oBUPermission.Remarks = ""; }
                _oBUPermission = _oBUPermission.SaveBUWiseShift((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBUPermission = new BUPermission();
                _oBUPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBUPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBUPermission(BUPermission oBUPermission)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBUPermission.DeleteBUWiseShift(oBUPermission.BUWiseShiftID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
