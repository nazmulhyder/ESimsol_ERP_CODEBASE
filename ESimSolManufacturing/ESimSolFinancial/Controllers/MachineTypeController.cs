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
    public class MachineTypeController : PdfViewController
    {
        #region Declaration
        MachineType _oMachineType = new MachineType();
        List<MachineType> _oMachineTypes = new List<MachineType>();
        #endregion

        public ActionResult ViewMachineTypes(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<MachineType> oMachineTypes = new List<MachineType>();

            string sSQL = "SELECT * FROM MachineType";
            if (buid > 0)
                sSQL += " WHERE BUID=" + buid;

            oMachineTypes = MachineType.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
            return View(oMachineTypes);
        }
        public ActionResult ViewMachineType(int nId, int buid, double ts)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            MachineType oMachineType = new MachineType();
            if (nId > 0)
            {
                oMachineType = oMachineType.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
                oMachineType.BUID = buid;

            ViewBag.BUID = buid;
            if (buid > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnits.Add(oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID));
                ViewBag.BusinessUnits = oBusinessUnits;
            }
            else
            ViewBag.BusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.Modules = EnumObject.jGets(typeof(EnumModuleName)).OrderBy(x => x.Value).ToList(); 
            return View(oMachineType);
        }
     

        [HttpPost]
        public JsonResult Save(MachineType oMachineType)
        {
            oMachineType.RemoveNulls();
            _oMachineType = new MachineType();
            try
            {
                _oMachineType = oMachineType.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMachineType = new MachineType();
                _oMachineType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMachineType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult Delete(MachineType oMachineType)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oMachineType.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<MachineType> oMachineTypes = new List<MachineType>();
            oMachineTypes = MachineType.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMachineTypes);
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


        #region Adv Search
  
    
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        #endregion

    }
}
