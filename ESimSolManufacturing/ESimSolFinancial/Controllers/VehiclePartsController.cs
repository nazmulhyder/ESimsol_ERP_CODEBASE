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
    public class VehiclePartsController : Controller
    {
        #region Declaration
        VehicleParts _oVehicleParts = new VehicleParts();
        List<VehicleParts> _oVehiclePartss = new List<VehicleParts>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewVehiclePartsList(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleParts).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oVehiclePartss = new List<VehicleParts>();
            _oVehiclePartss = VehicleParts.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oVehiclePartss);
        }

        public ActionResult ViewVehicleParts(int id)
        {
            _oVehicleParts = new VehicleParts();
            if (id > 0)
            {
                _oVehicleParts = _oVehicleParts.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.VehiclePartsTypes = EnumObject.jGets(typeof(EnumPartsType));
            return View(_oVehicleParts);
        }

        [HttpPost]
        public JsonResult Save(VehicleParts oVehicleParts)
        {
            _oVehicleParts = new VehicleParts();
            try
            {
                _oVehicleParts = oVehicleParts;
                _oVehicleParts = _oVehicleParts.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleParts = new VehicleParts();
                _oVehicleParts.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleParts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                VehicleParts oVehicleParts = new VehicleParts();
                sFeedBackMessage = oVehicleParts.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VehiclePartsSearch()
        {
            List<VehicleParts> oVehiclePartss = new List<VehicleParts>();
            return PartialView(oVehiclePartss);
        }
        #region Searching


        [HttpPost]
        public JsonResult Gets()
        {
            List<VehicleParts> oVehiclePartss = new List<VehicleParts>();
            oVehiclePartss = VehicleParts.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVehiclePartss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchParts(VehicleParts oVehicleParts)
        {
            List<VehicleParts> oVehiclePartss = new List<VehicleParts>();
            if (oVehicleParts.Param == null || oVehicleParts.Param == "")
            {
                oVehiclePartss = VehicleParts.GetsByPartsNameWithType(oVehicleParts.PartsName, oVehicleParts.PartsType, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehiclePartss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region Print List
        //public ActionResult PrintList(string sIDs)
        //{

        //    _oVehicleParts = new VehicleParts();
        //    string sSQL = "SELECT * FROM View_VehicleParts WHERE VehiclePartsID IN (" + sIDs + ")";
        ////_oVehicleParts.PartsCategories = VehicleParts.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //rptVehiclePartsList oReport = new rptVehiclePartsList();
        //    byte[] abytes = oReport.PrepareReport(_oVehicleParts, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public void VehiclePartsExportToExcel(string sIDs)
        //{

        //    _oVehicleParts = new VehicleParts();
        //    string sSQL = "SELECT * FROM VehicleParts WHERE VehiclePartsID IN (" + sIDs + ")";
        //    _oVehicleParts.PartsCategories = VehicleParts.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = null;
        //    rptVehiclePartsList oReport = new rptVehiclePartsList();
        //    PdfPTable oPdfPTable = oReport.PrepareExcel(_oVehicleParts, oCompany);

        //    ExportToExcel.WorksheetName = "Model Category";
        //    byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

        //    Response.ClearContent();
        //    Response.BinaryWrite(abytes);
        //    Response.AddHeader("content-disposition", "attachment; filename=VehicleParts.xlsx");
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

