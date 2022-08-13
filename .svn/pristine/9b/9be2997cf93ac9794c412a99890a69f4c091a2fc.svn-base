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
    public class PolyMeasurementController : Controller
    {
        #region Declaration
        PolyMeasurement _oPolyMeasurement = new PolyMeasurement();
        List<PolyMeasurement> _oPolyMeasurements = new List<PolyMeasurement>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewPolyMeasurements(int menuid)
        {
            
	        this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PolyMeasurement).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oPolyMeasurements = new List<PolyMeasurement>();
            _oPolyMeasurements = PolyMeasurement.Gets((int)Session[SessionInfo.currentUserID]);

            List<AutoComplete> oAutoCompletes = new List<AutoComplete>();
            AutoComplete oAutoComplete = new AutoComplete();
            foreach (PolyMeasurement oItem in _oPolyMeasurements)
            {
                oAutoComplete = new AutoComplete();
                oAutoComplete.data = oItem.PolyMeasurementID.ToString();
                oAutoComplete.value = oItem.Measurement;
                oAutoCompletes.Add(oAutoComplete);
            }

            ViewBag.PolyMeasurements = oAutoCompletes;
            ViewBag.PolyMeasurementTypes = EnumObject.jGets(typeof(EnumPolyMeasurementType));
            return View(_oPolyMeasurements);
        }

        public ActionResult ViewPolyMeasurement(int id)
        {
            _oPolyMeasurement = new PolyMeasurement();
            if (id > 0)
            {
                _oPolyMeasurement = _oPolyMeasurement.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.PolyMeasurementTypes = EnumObject.jGets(typeof(EnumPolyMeasurementType));//combo
            return View(_oPolyMeasurement);
        }

        [HttpPost]
        public JsonResult Save(PolyMeasurement oPolyMeasurement)
        {
            _oPolyMeasurement = new PolyMeasurement();
            try
            {
                _oPolyMeasurement = oPolyMeasurement;                
                _oPolyMeasurement = _oPolyMeasurement.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPolyMeasurement = new PolyMeasurement();
                _oPolyMeasurement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPolyMeasurement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                PolyMeasurement oPolyMeasurement = new PolyMeasurement();
                sFeedBackMessage = oPolyMeasurement.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchColor(string sTemp, double ts)
        {
            List<PolyMeasurement> oPolyMeasurements = new List<PolyMeasurement>();
            oPolyMeasurements = PolyMeasurement.GetsbyMeasurement(sTemp, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oPolyMeasurements);
        }

        public ActionResult PolyMeasurementSearch()
        {
            List<PolyMeasurement> oPolyMeasurements = new List<PolyMeasurement>();            
            return PartialView(oPolyMeasurements);
        }
        #region Searching

        [WebMethod]
        public List<AutoComplete> GetsForAutoComplete()
        {
            List<PolyMeasurement> oPolyMeasurements = new List<PolyMeasurement>();
            oPolyMeasurements = PolyMeasurement.Gets((int)Session[SessionInfo.currentUserID]);

            List<AutoComplete> oAutoCompletes = new List<AutoComplete>();
            AutoComplete oAutoComplete = new AutoComplete();
            foreach (PolyMeasurement oItem in oPolyMeasurements)
            {
                oAutoComplete = new AutoComplete();
                oAutoComplete.value = oItem.PolyMeasurementID.ToString();
                oAutoComplete.data = oItem.Measurement;
                oAutoCompletes.Add(oAutoComplete);
            }
            return oAutoCompletes;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize((object)oAutoCompletes);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByMeasurement(PolyMeasurement oPolyMeasurement)
        {
            List<PolyMeasurement> oPolyMeasurements = new List<PolyMeasurement>();
            string sSQL = "";
            if ((EnumPolyMeasurementType)oPolyMeasurement.PolyMeasurementTypeInt != EnumPolyMeasurementType.None)
            {
                sSQL = "SELECT * FROM PolyMeasurement AS HH WHERE HH.PolyMeasurementType = " + oPolyMeasurement.PolyMeasurementTypeInt.ToString() + " AND HH.Measurement LIKE '%" + oPolyMeasurement.Measurement + "%' ORDER BY HH.Measurement ASC";
            }
            else
            {
                sSQL = "SELECT * FROM PolyMeasurement AS HH WHERE HH.Measurement LIKE '%" + oPolyMeasurement.Measurement + "%' ORDER BY HH.Measurement ASC";
            }
            oPolyMeasurements = PolyMeasurement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPolyMeasurements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Gets()
        {
            List<PolyMeasurement> oPolyMeasurements = new List<PolyMeasurement>();
            oPolyMeasurements = PolyMeasurement.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPolyMeasurements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SearchMeasurement(PolyMeasurement oPolyMeasurement)
        {
            List<PolyMeasurement> oPolyMeasurements = new List<PolyMeasurement>();
            if (oPolyMeasurement.Measurement == null || oPolyMeasurement.Measurement == "")
            {
                oPolyMeasurements = PolyMeasurement.Gets((int)Session[SessionInfo.currentUserID]);
            }
            {
                oPolyMeasurements = PolyMeasurement.GetsbyMeasurement(oPolyMeasurement.Measurement, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPolyMeasurements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getColorByMeasurement(PolyMeasurement oPolyMeasurement)
        {
            List<PolyMeasurement> oPolyMeasurements = new List<PolyMeasurement>();
            
            {
                oPolyMeasurements = PolyMeasurement.GetsbyMeasurement(oPolyMeasurement.Measurement, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPolyMeasurements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print List
        //public ActionResult PrintList(string sIDs)
        //{
        public ActionResult PrintList()
        {
            //_oPolyMeasurement = new PolyMeasurement();
            //string sSQL = "SELECT * FROM PolyMeasurement WHERE PolyMeasurementID IN (" + sIDs + ")";
            //_oPolyMeasurement.ColorCategories = PolyMeasurement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            //Company oCompany = new Company();
            //oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //rptPolyMeasurementList oReport = new rptPolyMeasurementList();
            //byte[] abytes = oReport.PrepareReport(_oPolyMeasurement, oCompany);
            //return File(abytes, "application/pdf");

           

            _oPolyMeasurements = new List<PolyMeasurement>();
            _oPolyMeasurements = PolyMeasurement.Gets((int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptPolyMeasurementList oReport = new rptPolyMeasurementList();
            byte[] abytes = oReport.PrepareReport(_oPolyMeasurements, oCompany, "PolyMeasurement List");
            return File(abytes, "application/pdf");
        }

        //public void PolyMeasurementExportToExcel(string sIDs)
        //{

        //    _oPolyMeasurement = new PolyMeasurement();
        //    string sSQL = "SELECT * FROM PolyMeasurement WHERE PolyMeasurementID IN (" + sIDs + ")";
        //    _oPolyMeasurement.ColorCategories = PolyMeasurement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = null;
        //    rptPolyMeasurementList oReport = new rptPolyMeasurementList();
        //    PdfPTable oPdfPTable = oReport.PrepareExcel(_oPolyMeasurement, oCompany);

        //    ExportToExcel.WorksheetName = "Color Category";
        //    byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

        //    Response.ClearContent();
        //    Response.BinaryWrite(abytes);
        //    Response.AddHeader("content-disposition", "attachment; filename=PolyMeasurement.xlsx");
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

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion 

    }
}
