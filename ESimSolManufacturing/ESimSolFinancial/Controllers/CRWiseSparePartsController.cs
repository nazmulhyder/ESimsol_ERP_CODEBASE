using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class CRWiseSparePartsController : Controller
    {
        #region Declaration
        CRWiseSpareParts _oCRWiseSpareParts = new CRWiseSpareParts();
        CapitalResource _oCapitalResource = new CapitalResource();
        List<CRWiseSpareParts> _oCRWiseSparePartss = new List<CRWiseSpareParts>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewCRWiseSparePartss(int nCRID, int nBUID)
        {
            _oCapitalResource = CapitalResource.Get(nCRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oCRWiseSparePartss = CRWiseSpareParts.Gets(nCRID, nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = nBUID;
            ViewBag.CapitalResource = _oCapitalResource;
            return View(_oCRWiseSparePartss);
        }
        public JsonResult Save(CRWiseSpareParts oCRWiseSpareParts)
        {
            _oCRWiseSpareParts = new CRWiseSpareParts();
            _oCRWiseSpareParts = oCRWiseSpareParts;
            try
            {
                _oCRWiseSpareParts = _oCRWiseSpareParts.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCRWiseSpareParts.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCRWiseSpareParts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                _oCRWiseSpareParts = new CRWiseSpareParts();
                sErrorMease = _oCRWiseSpareParts.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveFromCopy(List<CRWiseSpareParts> oCRWiseSparePartss)
        {
            _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            try
            {
                _oCRWiseSparePartss = CRWiseSpareParts.SaveFromCopy(oCRWiseSparePartss, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCRWiseSpareParts = new CRWiseSpareParts();
                _oCRWiseSpareParts.ErrorMessage = ex.Message;
                _oCRWiseSparePartss.Add(_oCRWiseSpareParts);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCRWiseSparePartss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Searching

        [HttpGet]
        public JsonResult Get(int id)
        {
            CRWiseSpareParts oCRWiseSpareParts = new CRWiseSpareParts();
            try
            {
                oCRWiseSpareParts = oCRWiseSpareParts.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oCRWiseSpareParts = new CRWiseSpareParts();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCRWiseSpareParts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            try
            {
                _oCRWiseSparePartss = CRWiseSpareParts.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCRWiseSparePartss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByCRIDAndBUID(CRWiseSpareParts oCRWiseSpareParts)
        {
            _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            try
            {
                _oCRWiseSparePartss = CRWiseSpareParts.Gets(oCRWiseSpareParts.CRID, oCRWiseSpareParts.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCRWiseSparePartss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByNameCRAndBUID(CRWiseSpareParts oCRWiseSpareParts)
        {
            _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            try
            {
                _oCRWiseSparePartss = CRWiseSpareParts.GetsByNameCRAndBUID(oCRWiseSpareParts.ProductName, oCRWiseSpareParts.CRID, oCRWiseSpareParts.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCRWiseSparePartss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult PrintSparePartsList(int nCRID, int nBUID, double nts) 
        {
            int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oCapitalResource = CapitalResource.Get(nCRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oCRWiseSparePartss = CRWiseSpareParts.Gets(nCRID, nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPrintCRWiseSpareParts oReport = new rptPrintCRWiseSpareParts();
            byte[] abytes = oReport.PrepareReport(_oCapitalResource, _oCRWiseSparePartss, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        #endregion
        #region Get Company Logo
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
