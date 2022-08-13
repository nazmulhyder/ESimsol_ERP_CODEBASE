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
    public class VehicleEngineController : Controller
    {
        #region Declaration
        VehicleEngine _oVehicleEngine = new VehicleEngine();
        List<VehicleEngine> _oVehicleEngines = new List<VehicleEngine>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewVehicleEngines(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleEngine).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oVehicleEngines = new List<VehicleEngine>();
            _oVehicleEngines = VehicleEngine.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Sessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FuelTypes = EnumObject.jGets(typeof(EnumFuelType));
            return View(_oVehicleEngines);
        }

        public ActionResult ViewVehicleEngine(int id)
        {
            _oVehicleEngine = new VehicleEngine();
            if (id > 0)
            {
                _oVehicleEngine = VehicleEngine.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            
            return PartialView(_oVehicleEngine);
        }

        [HttpPost]
        public JsonResult Save(VehicleEngine oVehicleEngine)
        {
            _oVehicleEngine = new VehicleEngine();
            try
            {
                _oVehicleEngine = oVehicleEngine;
                _oVehicleEngine = _oVehicleEngine.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleEngine = new VehicleEngine();
                _oVehicleEngine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleEngine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                VehicleEngine oVehicleEngine = new VehicleEngine();
                sFeedBackMessage = oVehicleEngine.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
            List<VehicleEngine> oVehicleEngines = new List<VehicleEngine>();
            //oVehicleEngines = VehicleEngine.GetsByColorName(sColorName, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oVehicleEngines);
        }

        public ActionResult VehicleEngineSearch()//NotUsed 
        {
            List<VehicleEngine> oVehicleEngines = new List<VehicleEngine>();
            return PartialView(oVehicleEngines);
        }
        #region Searching



        [HttpPost]
        public JsonResult Gets()
        {
            List<VehicleEngine> oVehicleEngines = new List<VehicleEngine>();
            oVehicleEngines = VehicleEngine.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVehicleEngines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByID(VehicleEngine oVehicleEngine)
        {
            VehicleEngine _oVehicleEngine = new VehicleEngine();
            if (oVehicleEngine.VehicleEngineID > 0)
            {
                _oVehicleEngine = VehicleEngine.Get(oVehicleEngine.VehicleEngineID, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVehicleEngine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByEngineNo(VehicleEngine oVehicleEngine) //NotUsed 
        {
            List<VehicleEngine> oVehicleEngines = new List<VehicleEngine>();
            if (oVehicleEngine.Param == null || oVehicleEngine.Param == "")
            {
                oVehicleEngines = VehicleEngine.GetsByEngineNo(oVehicleEngine.EngineNo,(int)Session[SessionInfo.currentUserID]);
            }
            {
                //string sFTypes = (int)EnumColorType.StandardVehicleEngine + "," + (int)EnumVehicleEngineType.SafetyVehicleEngine + "," + (int)EnumVehicleEngineType.InteriorVehicleEngine + "," + (int)EnumVehicleEngineType.ExteriorVehicleEngine + "," + (int)EnumVehicleEngineType.CountrySettingVehicleEngine + "," + (int)EnumVehicleEngineType.OptionalVehicleEngine;
                //oVehicleEngines = VehicleEngine.GetsbyVehicleEngineNameWithType(oVehicleEngine.Param, sFTypes, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleEngines);
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
            List<VehicleEngine> oVehicleEngines = new List<VehicleEngine>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oVehicleEngines = VehicleEngine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleEngine = new VehicleEngine();
                _oVehicleEngine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleEngines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            string sFileNo = sTemp.Split('~')[0];
            string sEngineNo = sTemp.Split('~')[1];
            string sManufacturerIDs = sTemp.Split('~')[2];
            string sCylinders = sTemp.Split('~')[3];
            string sCompressionRatio = sTemp.Split('~')[4];
            string sBMEP = sTemp.Split('~')[5];
            string sEngineCoolant = sTemp.Split('~')[6];
            string sCatalyticConverter = sTemp.Split('~')[7];

            string sReturn1 = "SELECT * FROM View_VehicleEngine";
            string sReturn = "";


            #region File No
            if (sFileNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FileNo LIKE '%" + sFileNo + "%'";
            }
            #endregion
            #region Engine No
            if (sEngineNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EngineNo LIKE '%" + sEngineNo + "%'";
            }
            #endregion

            #region Manufacturer wise

            if (sManufacturerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ManufacturerID IN (" + sManufacturerIDs + ")";
            }
            #endregion

            #region Cylinders
            if (sCylinders != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Cylinders LIKE '%" + sCylinders + "%'";
            }
            #endregion

            #region Compression Ratio
            if (sCompressionRatio != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CompressionRatio LIKE '%" + sCompressionRatio + "%'";
            }
            #endregion

            #region sBMEP
            if (sBMEP != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BMEP LIKE '%" + sBMEP + "%'";
            }
            #endregion

            #region sEngineCoolant
            if (sEngineCoolant != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EngineCoolant LIKE '%" + sEngineCoolant + "%'";
            }
            #endregion

            #region sCatalyticConverter
            if (sCatalyticConverter != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CatalyticConverter LIKE '%" + sCatalyticConverter + "%'";
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

        //    _oVehicleEngine = new VehicleEngine();
        //    string sSQL = "SELECT * FROM View_VehicleEngine WHERE VehicleEngineID IN (" + sIDs + ")";
        ////_oVehicleEngine.ColorCategories = VehicleEngine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //rptVehicleEngineList oReport = new rptVehicleEngineList();
        //    byte[] abytes = oReport.PrepareReport(_oVehicleEngine, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public void VehicleEngineExportToExcel(string sIDs)
        //{

        //    _oVehicleEngine = new VehicleEngine();
        //    string sSQL = "SELECT * FROM VehicleEngine WHERE VehicleEngineID IN (" + sIDs + ")";
        //    _oVehicleEngine.ColorCategories = VehicleEngine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = null;
        //    rptVehicleEngineList oReport = new rptVehicleEngineList();
        //    PdfPTable oPdfPTable = oReport.PrepareExcel(_oVehicleEngine, oCompany);

        //    ExportToExcel.WorksheetName = "Model Category";
        //    byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

        //    Response.ClearContent();
        //    Response.BinaryWrite(abytes);
        //    Response.AddHeader("content-disposition", "attachment; filename=VehicleEngine.xlsx");
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


