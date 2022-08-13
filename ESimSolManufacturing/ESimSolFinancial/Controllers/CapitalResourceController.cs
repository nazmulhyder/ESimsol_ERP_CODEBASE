using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using ReportManagement;
using System.Net.Mail;
using ESimSol.Reports;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace ESimSolFinancial.Controllers
{
    public class CapitalResourceController : PdfViewController
    {
        #region Declaration
        CapitalResource _oCapitalResource = new CapitalResource();
        List<CapitalResource> _oCapitalResources = new List<CapitalResource>();
       
        TCapitalResource _oTCapitalResource = new TCapitalResource();
        List<TCapitalResource> _oTCapitalResources = new List<TCapitalResource>();

        string _sErrorMessage = "";
        #endregion

        #region New Task

        public ActionResult ViewCapitalResources(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oCapitalResources = new List<CapitalResource>();
            _oCapitalResources = CapitalResource.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ResourcesType = CapitalResource.BUWiseResourceTypeGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oCapitalResources);
        }

        public ActionResult ViewCapitalResource(int id, int nBUID, bool bIsCopy, double ts)
        {            
            _oCapitalResource = new CapitalResource();
            Company oCompany = new Company();
            if(id>0)
            {
                _oCapitalResource = CapitalResource.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.ResourceTypes = EnumObject.jGets(typeof(EnumResourcesType));
            string sSQL = "SELECT * FROM View_WorkingUnit WHERE IsStore=1 AND IsActive=1 ";
            ViewBag.WUs = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Suppliers = Contractor.GetsByNamenType("", ((int)EnumContractorType.Supplier).ToString(), nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Racks = Rack.BUWiseGets(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Company = oCompany.Get(((User)Session[SessionInfo.CurrentUser]).CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FGUnits = MeasurementUnit.Gets(EnumUniteType.Weight, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = nBUID;
            if (bIsCopy) _oCapitalResource.CRID = 0;
            ViewBag.bIsCopy = bIsCopy;
            return View(_oCapitalResource);
        }


        #region Insert, Update, Delete, Activity Change
        [HttpPost]
        public JsonResult Save(CapitalResource oCapitalResource)
        {
            _oCapitalResource = new CapitalResource();
            try
            {
                _oCapitalResource = oCapitalResource;
                _oCapitalResource.ResourcesType = (EnumResourcesType)oCapitalResource.ResourcesTypeInInt;
                if (_oCapitalResource.CRID<=0)
                {
                    _oCapitalResource = _oCapitalResource.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oCapitalResource = _oCapitalResource.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                
                
            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResource);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Copy(CapitalResource oCapitalResource)
        {
            int nPreviousCRID = oCapitalResource.CRID;
            _oCapitalResource = new CapitalResource();
            try
            {
                _oCapitalResource = CapitalResource.Get(oCapitalResource.CRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCapitalResource = _oCapitalResource.CopyCapitalResource(_oCapitalResource, ((User)Session[SessionInfo.CurrentUser]).UserID);

                CRWiseSpareParts oCRWiseSpareParts = new CRWiseSpareParts();
                List<CRWiseSpareParts> oCRWiseSparePartss = new List<CRWiseSpareParts>();
                oCRWiseSparePartss = CRWiseSpareParts.GetsByNameCRAndBUID("", nPreviousCRID, oCapitalResource.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(CRWiseSpareParts oItem in oCRWiseSparePartss)
                {
                    oItem.CRID = _oCapitalResource.CRID;
                    oItem.CRWiseSparePartsID = 0;
                    oCRWiseSpareParts = oItem.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResource);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int nCRID, double nts)
        {
            string sMessage = "";
            _oCapitalResource = new CapitalResource();
            try
            {
                if (nCRID <= 0) { throw new Exception ( "Select a valid item from list." ); }
                _oCapitalResource.CRID = nCRID;
                _oCapitalResource = _oCapitalResource.IUD( (int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sMessage=_oCapitalResource.ErrorMessage;
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ActivityChange(int nCRID, bool bIsActive, double nts)
        {
            _oCapitalResource = new CapitalResource();
            try
            {
                if (nCRID <= 0) { throw new Exception("Please select a valid item from list."); }
                _oCapitalResource = CapitalResource.Get(nCRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oCapitalResource.CRID <= 0) { throw new Exception("Invalid request.");  }
                _oCapitalResource.IsActive = bIsActive;
                _oCapitalResource = _oCapitalResource.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResource);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region

        
        [HttpPost]
        public JsonResult CopyCapitalResource(int nCRID, double nts)
        {
            _oCapitalResource = new CapitalResource();
            try
            {
                if (nCRID <= 0) { throw new Exception ( "Select a valid item from list." ); }
                _oCapitalResource = _oCapitalResource.CopyCR(nCRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResource);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Retrive Information

        [HttpPost]
        public JsonResult Get(int nCRID, double nts)
        {
            _oCapitalResource = new CapitalResource();
            try
            {
                if (nCRID <= 0) { throw new Exception("Select a valid item from list."); }
                _oCapitalResource = CapitalResource.Get(nCRID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResource);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCRByNameBUID(CapitalResource oCapitalResource)
        {
            _oCapitalResources = new List<CapitalResource>();
            try
            {
                if(string.IsNullOrEmpty(oCapitalResource.Params))
                {
                    _oCapitalResources = CapitalResource.Gets("SELECT * FROM View_CapitalResource WHERE Name LIKE '%" + oCapitalResource.Name + "%' AND BUID = " + oCapitalResource.BUID + " AND ParentID>1", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oCapitalResources = CapitalResource.Gets("SELECT * FROM View_CapitalResource WHERE Name LIKE '%" + oCapitalResource.Name + "%' AND BUID = " + oCapitalResource.BUID + " AND ParentID>1 AND " + oCapitalResource.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
                _oCapitalResources.Add(_oCapitalResource);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResources);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCRByNameCodeTypeBUID(CapitalResource oCapitalResource)
        {
            _oCapitalResources = new List<CapitalResource>();
            try
            {
                if(oCapitalResource.ParentID>0)
                {
                    _oCapitalResources = CapitalResource.Gets("SELECT * FROM View_CapitalResource WHERE Name LIKE '%" + oCapitalResource.Name + "%' AND Code LIKE '%" + oCapitalResource.Code + "%' AND ParentID = '" + oCapitalResource.ParentID + "' AND BUID = " + oCapitalResource.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oCapitalResources = CapitalResource.Gets("SELECT * FROM View_CapitalResource WHERE Name LIKE '%" + oCapitalResource.Name + "%' AND Code LIKE '%" + oCapitalResource.Code + "%' AND BUID = " + oCapitalResource.BUID + " AND ParentID>1", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
                _oCapitalResources.Add(_oCapitalResource);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResources);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult Gets(double nts)
        //{
        //    _oCapitalResource = new CapitalResource();
        //    try
        //    {
        //        int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
        //        string sSQL = "SELECT * FROM View_CapitalResource Where CompanyID=" + nCompanyID + "";
        //        _oCapitalResources = CapitalResource.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        if (_oCapitalResources.Count() > 0)
        //        {
        //            foreach (CapitalResource oItem in _oCapitalResources)
        //            {
        //                _oTCapitalResource = new TCapitalResource();
        //                _oTCapitalResource.id = oItem.CRID;
        //                _oTCapitalResource.parentid = oItem.ParentID;
        //                _oTCapitalResource.text = oItem.Name;
        //                _oTCapitalResource.activity = oItem.ActivityInStr;
        //                _oTCapitalResource.IsLastLayer = oItem.IsLastLayer;
        //                _oTCapitalResource.code = oItem.Code;
        //                _oTCapitalResource.Note = oItem.Note;
        //                _oTCapitalResource.CRGID = oItem.CRGID;
        //                _oTCapitalResources.Add(_oTCapitalResource);
        //            }
        //            _oTCapitalResource = new TCapitalResource();
        //            _oTCapitalResource = GetRoot();
        //            this.AddTreeNodes(ref _oTCapitalResource);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _oTCapitalResource = new TCapitalResource();
        //    }

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oTCapitalResource);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetsByName(string sName,double nts)
        {
            _oCapitalResource = new CapitalResource();
            _oCapitalResources = new List<CapitalResource>();
            try
            {
                int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
                string sSQL = "SELECT * FROM View_CapitalResource Where  IsLastLayer=1 And CompanyID=" + nCompanyID + "";
                if (sName.Trim()!="")
                {
                    sSQL = sSQL + " And Name like '%" + sName.Trim() + "%'";
                }
                _oCapitalResources = CapitalResource.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oCapitalResources.Count() <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                _oCapitalResources = new List<CapitalResource>();
                _oCapitalResource.ErrorMessage=ex.Message;
                _oCapitalResources.Add(_oCapitalResource);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResources);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByBUandResourceTypeWithName(CapitalResource oCapitalResource)
        {
            _oCapitalResources = new List<CapitalResource>();
            try
            {
                
                _oCapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(oCapitalResource.BUID, oCapitalResource.ResourcesTypeInInt, oCapitalResource.Name,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
                _oCapitalResources.Add(_oCapitalResource);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResources);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCapitalResourceLastLayer(CapitalResource oCapitalResource)
        {
            _oCapitalResources = new List<CapitalResource>();
            try
            {                
                string sSQL = "Select  * from View_CapitalResource  Where BUID=" + oCapitalResource.BUID + " and ResourcesType=" + oCapitalResource.ResourcesTypeInInt + " And Name Like '%" + oCapitalResource.Name + "%' And IsLastLayer=1";
                _oCapitalResources = CapitalResource.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCapitalResource = new CapitalResource();
                _oCapitalResource.ErrorMessage = ex.Message;
                _oCapitalResources.Add(_oCapitalResource);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCapitalResources);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Print
        public ActionResult PrintMachine(int nCRID, double nts) 
        {
            int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;

            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            _oCapitalResource = new CapitalResource();
            _oCapitalResource = CapitalResource.Get(nCRID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPrintMachine oReport = new rptPrintMachine();
            byte[] abytes = oReport.PrepareReport(_oCapitalResource, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintMachineList(int nBuid, double nts) 
        {
            int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBuid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oCapitalResource = new CapitalResource();
            _oCapitalResources = CapitalResource.Gets("SELECT * FROM View_CapitalResource Where BUID = " + nBuid + " AND ParentID>1 Order By ParentName, Name", ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPrintMachineList oReport = new rptPrintMachineList();
            byte[] abytes = oReport.PrepareReport(_oCapitalResources, oCompany, oBusinessUnit);
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

        #endregion

    }
}

