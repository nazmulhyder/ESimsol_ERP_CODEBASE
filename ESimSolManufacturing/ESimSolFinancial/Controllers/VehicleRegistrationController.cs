using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;

namespace ESimSolFinancial.Controllers
{
    public class VehicleRegistrationController : Controller
    {
        #region Declaration
        VehicleRegistration _oVehicleRegistration = new VehicleRegistration();
        List<VehicleRegistration> _oVehicleRegistrations = new List<VehicleRegistration>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewVehicleRegistrations(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleRegistration).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            _oVehicleRegistrations = new List<VehicleRegistration>();
            _oVehicleRegistrations = VehicleRegistration.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.VehicleTypes = VehicleType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Sessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FuelTypes = EnumObject.jGets(typeof(EnumFuelType));
            ViewBag.VehicleRegistrationTypes = EnumObject.jGets(typeof(EnumVehicleRegistrationType)).Where(x => x.id == (int)EnumVehicleRegistrationType.Inhouse_Client || x.id == (int)EnumVehicleRegistrationType.Out_Client).ToList();
            return View(_oVehicleRegistrations);
        }

        public ActionResult ViewVehicleRegistration(int id)
        {
            _oVehicleRegistration = new VehicleRegistration();
            //if (id > 0)
            //{
            //    _oVehicleRegistration = _oVehicleRegistration.Get(id, (int)Session[SessionInfo.currentUserID]);
            //}
            return PartialView(_oVehicleRegistration);
        }

        [HttpPost]
        public JsonResult Save(VehicleRegistration oVehicleRegistration)
        {
            _oVehicleRegistration = new VehicleRegistration();
            try
            {
                _oVehicleRegistration = oVehicleRegistration;
                _oVehicleRegistration = _oVehicleRegistration.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleRegistration = new VehicleRegistration();
                _oVehicleRegistration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleRegistration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                VehicleRegistration oVehicleRegistration = new VehicleRegistration();
                sFeedBackMessage = oVehicleRegistration.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Searching

        [HttpPost]
        public JsonResult Gets()
        {
            List<VehicleRegistration> oVehicleRegistrations = new List<VehicleRegistration>();
            oVehicleRegistrations = VehicleRegistration.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVehicleRegistrations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByID(VehicleRegistration oVehicleRegistration)
        {
            VehicleRegistration _oVehicleRegistration = new VehicleRegistration();
            if (oVehicleRegistration.VehicleRegistrationID > 0)
            {
                _oVehicleRegistration = _oVehicleRegistration.Get(oVehicleRegistration.VehicleRegistrationID, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVehicleRegistration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByNo(VehicleRegistration oVehicleRegistration)
        {
            string sSQL = "SELECT * FROM View_VehicleRegistration WHERE (ISNULL(VehicleRegNo,'')+ISNULL(VehicleModelNo,'')+ISNULL(ChassisNo,'')) LIKE '%" + oVehicleRegistration.VehicleRegNo + "%'";
            _oVehicleRegistrations = VehicleRegistration.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVehicleRegistrations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }

        #region HttpGet For Search
        [HttpGet]
        public JsonResult Search(string sTemp)
        {
            List<VehicleRegistration> oVehicleRegistrations = new List<VehicleRegistration>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oVehicleRegistrations = VehicleRegistration.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleRegistration = new VehicleRegistration();
                _oVehicleRegistration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleRegistrations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            string sFileNo = sTemp.Split('~')[0];
            string sChassisNo = sTemp.Split('~')[1];
            string sManufacturerIDs = sTemp.Split('~')[2];
            string sEngineLayout = sTemp.Split('~')[3];
            string sDriveWheels = sTemp.Split('~')[4];
            string sSteering = sTemp.Split('~')[5];
            string sGearBox = sTemp.Split('~')[6];
            
            string sReturn1 = "SELECT * FROM View_VehicleRegistration";
            string sReturn = "";


            #region File No
            if (sFileNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FileNo LIKE '%" + sFileNo + "%'";
            }
            #endregion
            #region Chassis No
            if (sChassisNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChassisNo LIKE '%" + sChassisNo + "%'";
            }
            #endregion

            #region Manufacturer wise

            if (sManufacturerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ManufacturerID IN (" + sManufacturerIDs + ")";
            }
            #endregion

            #region Engine Layout
            if (sEngineLayout != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EngineLayout LIKE '%" + sEngineLayout + "%'";
            }
            #endregion

            #region Drive Wheels
            if (sDriveWheels != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DriveWheels LIKE '%" + sDriveWheels + "%'";
            }
            #endregion

            #region Steering
            if (sSteering != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Steering LIKE '%" + sSteering + "%'";
            }
            #endregion

            #region GearBox
            if (sGearBox != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GearBox LIKE '%" + sGearBox + "%'";
            }
            #endregion
          
            //#region User Set
            //if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn += " Applicant IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            //}
            //#endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
        #endregion

    }

}


