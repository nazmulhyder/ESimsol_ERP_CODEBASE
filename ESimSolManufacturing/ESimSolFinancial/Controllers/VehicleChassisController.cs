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
    public class VehicleChassisController : Controller
    {
        #region Declaration
        VehicleChassis _oVehicleChassis = new VehicleChassis();
        List<VehicleChassis> _oVehicleChassiss = new List<VehicleChassis>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewVehicleChassiss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleChassis).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));            

            _oVehicleChassiss = new List<VehicleChassis>();
            _oVehicleChassiss = VehicleChassis.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oVehicleChassiss);
        }

        public ActionResult ViewVehicleChassis(int id)
        {
            _oVehicleChassis = new VehicleChassis();
            //if (id > 0)
            //{
            //    _oVehicleChassis = _oVehicleChassis.Get(id, (int)Session[SessionInfo.currentUserID]);
            //}
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.VehicleChassisTypes = EnumObject.jGets(typeof(EnumColorType));
            return PartialView(_oVehicleChassis);
        }

        [HttpPost]
        public JsonResult Save(VehicleChassis oVehicleChassis)
        {
            _oVehicleChassis = new VehicleChassis();
            try
            {
                _oVehicleChassis = oVehicleChassis;
                _oVehicleChassis = _oVehicleChassis.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleChassis = new VehicleChassis();
                _oVehicleChassis.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleChassis);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                VehicleChassis oVehicleChassis = new VehicleChassis();
                sFeedBackMessage = oVehicleChassis.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchColor(string sColorName, double ts)//NotUsed 
        {
            List<VehicleChassis> oVehicleChassiss = new List<VehicleChassis>();
            //oVehicleChassiss = VehicleChassis.GetsByColorName(sColorName, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oVehicleChassiss);
        }

        public ActionResult VehicleChassisSearch()//NotUsed 
        {
            List<VehicleChassis> oVehicleChassiss = new List<VehicleChassis>();
            return PartialView(oVehicleChassiss);
        }
        #region Searching



        [HttpPost]
        public JsonResult Gets()
        {
            List<VehicleChassis> oVehicleChassiss = new List<VehicleChassis>();
            oVehicleChassiss = VehicleChassis.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVehicleChassiss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByID(VehicleChassis oVehicleChassis)
        {
            VehicleChassis _oVehicleChassis = new VehicleChassis();
            if (oVehicleChassis.VehicleChassisID > 0)
            {
                _oVehicleChassis = VehicleChassis.Get(oVehicleChassis.VehicleChassisID, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVehicleChassis);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SearchByChassisNo(VehicleChassis oVehicleChassis) //NotUsed 
        {
            List<VehicleChassis> oVehicleChassiss = new List<VehicleChassis>();
            if (oVehicleChassis.Param == null || oVehicleChassis.Param == "")
            {
                oVehicleChassiss = VehicleChassis.GetsByChassisNo(oVehicleChassis.ChassisNo,(int)Session[SessionInfo.currentUserID]);
            }
            {
                //string sFTypes = (int)EnumColorType.StandardVehicleChassis + "," + (int)EnumVehicleChassisType.SafetyVehicleChassis + "," + (int)EnumVehicleChassisType.InteriorVehicleChassis + "," + (int)EnumVehicleChassisType.ExteriorVehicleChassis + "," + (int)EnumVehicleChassisType.CountrySettingVehicleChassis + "," + (int)EnumVehicleChassisType.OptionalVehicleChassis;
                //oVehicleChassiss = VehicleChassis.GetsbyVehicleChassisNameWithType(oVehicleChassis.Param, sFTypes, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleChassiss);
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
            List<VehicleChassis> oVehicleChassiss = new List<VehicleChassis>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oVehicleChassiss = VehicleChassis.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleChassis = new VehicleChassis();
                _oVehicleChassis.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleChassiss);
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
            
            string sReturn1 = "SELECT * FROM View_VehicleChassis";
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

        //#region Print List
        //public ActionResult PrintList(string sIDs)
        //{

        //    _oVehicleChassis = new VehicleChassis();
        //    string sSQL = "SELECT * FROM View_VehicleChassis WHERE VehicleChassisID IN (" + sIDs + ")";
        ////_oVehicleChassis.ColorCategories = VehicleChassis.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //rptVehicleChassisList oReport = new rptVehicleChassisList();
        //    byte[] abytes = oReport.PrepareReport(_oVehicleChassis, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public void VehicleChassisExportToExcel(string sIDs)
        //{

        //    _oVehicleChassis = new VehicleChassis();
        //    string sSQL = "SELECT * FROM VehicleChassis WHERE VehicleChassisID IN (" + sIDs + ")";
        //    _oVehicleChassis.ColorCategories = VehicleChassis.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = null;
        //    rptVehicleChassisList oReport = new rptVehicleChassisList();
        //    PdfPTable oPdfPTable = oReport.PrepareExcel(_oVehicleChassis, oCompany);

        //    ExportToExcel.WorksheetName = "Model Category";
        //    byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

        //    Response.ClearContent();
        //    Response.BinaryWrite(abytes);
        //    Response.AddHeader("content-disposition", "attachment; filename=VehicleChassis.xlsx");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Flush();
        //    Response.End();
        //}

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
        //#endregion

    }

}


