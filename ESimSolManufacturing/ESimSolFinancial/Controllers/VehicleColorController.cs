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
    public class VehicleColorController : Controller
    {
        #region Declaration
        VehicleColor _oVehicleColor = new VehicleColor();
        List<VehicleColor> _oVehicleColors = new List<VehicleColor>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewVehicleColors(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleColor).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            _oVehicleColors = new List<VehicleColor>();
            _oVehicleColors = VehicleColor.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oVehicleColors);
        }

        public ActionResult ViewVehicleColor(int id)
        {
            _oVehicleColor = new VehicleColor();
            if (id > 0)
            {
                _oVehicleColor = _oVehicleColor.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.VehicleColorTypes = EnumObject.jGets(typeof(EnumColorType));
            return View(_oVehicleColor);
        }

        [HttpPost]
        public JsonResult Save(VehicleColor oVehicleColor)
        {
            _oVehicleColor = new VehicleColor();
            try
            {
                _oVehicleColor = oVehicleColor;
                _oVehicleColor = _oVehicleColor.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleColor = new VehicleColor();
                _oVehicleColor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleColor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                VehicleColor oVehicleColor = new VehicleColor();
                sFeedBackMessage = oVehicleColor.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VehicleColorSearch()
        {
            List<VehicleColor> oVehicleColors = new List<VehicleColor>();
            return PartialView(oVehicleColors);
        }
        #region Searching

        [HttpPost]
        public JsonResult Gets()
        {
            List<VehicleColor> oVehicleColors = new List<VehicleColor>();
            oVehicleColors = VehicleColor.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVehicleColors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchColor(VehicleColor oVehicleColor)
        {
            List<VehicleColor> oVehicleColors = new List<VehicleColor>();
            if (oVehicleColor.Param == null || oVehicleColor.Param == "")
            {
                oVehicleColors = VehicleColor.GetsByColorNameWithType(oVehicleColor.ColorName,oVehicleColor.ColorType, (int)Session[SessionInfo.currentUserID]);
            }
            {
                //string sFTypes = (int)EnumColorType.StandardVehicleColor + "," + (int)EnumVehicleColorType.SafetyVehicleColor + "," + (int)EnumVehicleColorType.InteriorVehicleColor + "," + (int)EnumVehicleColorType.ExteriorVehicleColor + "," + (int)EnumVehicleColorType.CountrySettingVehicleColor + "," + (int)EnumVehicleColorType.OptionalVehicleColor;
                //oVehicleColors = VehicleColor.GetsbyVehicleColorNameWithType(oVehicleColor.Param, sFTypes, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleColors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region Print List
        //public ActionResult PrintList(string sIDs)
        //{

        //    _oVehicleColor = new VehicleColor();
        //    string sSQL = "SELECT * FROM View_VehicleColor WHERE VehicleColorID IN (" + sIDs + ")";
        ////_oVehicleColor.ColorCategories = VehicleColor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //rptVehicleColorList oReport = new rptVehicleColorList();
        //    byte[] abytes = oReport.PrepareReport(_oVehicleColor, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public void VehicleColorExportToExcel(string sIDs)
        //{

        //    _oVehicleColor = new VehicleColor();
        //    string sSQL = "SELECT * FROM VehicleColor WHERE VehicleColorID IN (" + sIDs + ")";
        //    _oVehicleColor.ColorCategories = VehicleColor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = null;
        //    rptVehicleColorList oReport = new rptVehicleColorList();
        //    PdfPTable oPdfPTable = oReport.PrepareExcel(_oVehicleColor, oCompany);

        //    ExportToExcel.WorksheetName = "Model Category";
        //    byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

        //    Response.ClearContent();
        //    Response.BinaryWrite(abytes);
        //    Response.AddHeader("content-disposition", "attachment; filename=VehicleColor.xlsx");
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

