using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;



namespace ESimSolFinancial.Controllers
{
    public class LabDipSetupController : PdfViewController
    {
        #region Declaration
        LabDipSetup _oLabDipSetup = new LabDipSetup();
        List<LabDipSetup> _oLabDipSetups = new List<LabDipSetup>();
        #endregion

        public ActionResult ViewLabDipSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LabDipSetup> oLabDipSetups = new List<LabDipSetup>();
            oLabDipSetups = LabDipSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
           
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
          
            return View(oLabDipSetups);
        }
        public ActionResult ViewLabDipSetup(int nId, int buid, double ts)
        {
            LabDipSetup oLabDipSetup = new LabDipSetup();
            if (nId > 0)
            {
                oLabDipSetup = oLabDipSetup.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
            }
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.PrintNos = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Weight,((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oLabDipSetup);
        }
     

        [HttpPost]
        public JsonResult Save(LabDipSetup oLabDipSetup)
        {
            oLabDipSetup.RemoveNulls();
            _oLabDipSetup = new LabDipSetup();
            try
            {
                _oLabDipSetup = oLabDipSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLabDipSetup = new LabDipSetup();
                _oLabDipSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLabDipSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
    
       
        [HttpPost]
        public JsonResult Delete(LabDipSetup oLabDipSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLabDipSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<LabDipSetup> oLabDipSetups = new List<LabDipSetup>();
            oLabDipSetups = LabDipSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLabDipSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #region Search  by Press Enter
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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
