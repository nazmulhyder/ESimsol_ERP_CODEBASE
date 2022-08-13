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
    public class ServiceWorkController : Controller
    {
        #region Declaration
        ServiceWork _oServiceWork = new ServiceWork();
        List<ServiceWork> _oServiceWorks = new List<ServiceWork>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewServiceWorks(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ServiceWork).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            _oServiceWorks = new List<ServiceWork>();
            _oServiceWorks = ServiceWork.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oServiceWorks);
        }

        public ActionResult ViewServiceWork(int id)
        {
            _oServiceWork = new ServiceWork();
            if (id > 0)
            {
                _oServiceWork = _oServiceWork.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ServiceWorkTypes = EnumObject.jGets(typeof(EnumServiceType));
            return View(_oServiceWork);
        }

        [HttpPost]
        public JsonResult Save(ServiceWork oServiceWork)
        {
            _oServiceWork = new ServiceWork();
            try
            {
                _oServiceWork = oServiceWork;
                _oServiceWork = _oServiceWork.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oServiceWork = new ServiceWork();
                _oServiceWork.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceWork);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ServiceWork oServiceWork = new ServiceWork();
                sFeedBackMessage = oServiceWork.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceWorkSearch()
        {
            List<ServiceWork> oServiceWorks = new List<ServiceWork>();
            return PartialView(oServiceWorks);
        }
        #region Searching

        [HttpPost]
        public JsonResult Gets()
        {
            List<ServiceWork> oServiceWorks = new List<ServiceWork>();
            oServiceWorks = ServiceWork.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oServiceWorks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchService(ServiceWork oServiceWork)
        {
            List<ServiceWork> oServiceWorks = new List<ServiceWork>();
            if (oServiceWork.ServiceTypeInt>0)
            {
                oServiceWorks = ServiceWork.GetsByServiceNameWithType(oServiceWork.ServiceName,oServiceWork.ServiceTypeInt, (int)Session[SessionInfo.currentUserID]);
            }
            {
                //string sFTypes = (int)EnumServiceType.StandardServiceWork + "," + (int)EnumServiceWorkType.SafetyServiceWork + "," + (int)EnumServiceWorkType.InteriorServiceWork + "," + (int)EnumServiceWorkType.ExteriorServiceWork + "," + (int)EnumServiceWorkType.CountrySettingServiceWork + "," + (int)EnumServiceWorkType.OptionalServiceWork;
                //oServiceWorks = ServiceWork.GetsbyServiceWorkNameWithType(oServiceWork.Param, sFTypes, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceWorks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region Print List
        //public ActionResult PrintList(string sIDs)
        //{

        //    _oServiceWork = new ServiceWork();
        //    string sSQL = "SELECT * FROM View_ServiceWork WHERE ServiceWorkID IN (" + sIDs + ")";
        ////_oServiceWork.ServiceCategories = ServiceWork.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //rptServiceWorkList oReport = new rptServiceWorkList();
        //    byte[] abytes = oReport.PrepareReport(_oServiceWork, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public void ServiceWorkExportToExcel(string sIDs)
        //{

        //    _oServiceWork = new ServiceWork();
        //    string sSQL = "SELECT * FROM ServiceWork WHERE ServiceWorkID IN (" + sIDs + ")";
        //    _oServiceWork.ServiceCategories = ServiceWork.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = null;
        //    rptServiceWorkList oReport = new rptServiceWorkList();
        //    PdfPTable oPdfPTable = oReport.PrepareExcel(_oServiceWork, oCompany);

        //    ExportToExcel.WorksheetName = "Model Category";
        //    byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

        //    Response.ClearContent();
        //    Response.BinaryWrite(abytes);
        //    Response.AddHeader("content-disposition", "attachment; filename=ServiceWork.xlsx");
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

