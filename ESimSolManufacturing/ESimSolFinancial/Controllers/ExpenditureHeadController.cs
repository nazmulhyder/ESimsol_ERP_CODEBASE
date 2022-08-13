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
    public class ExpenditureHeadController : PdfViewController
    {
        #region Declaration
        ExpenditureHead _oExpenditureHead = new ExpenditureHead();
        List<ExpenditureHead> _oExpenditureHeads = new List<ExpenditureHead>();
        List<ExpenditureHeadMapping> _oExpenditureHeadMappings = new List<ExpenditureHeadMapping>();
        #endregion

        public ActionResult ViewExpenditureHeads(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ExpenditureHead> oExpenditureHeads = new List<ExpenditureHead>();
            oExpenditureHeads = ExpenditureHead.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
            return View(oExpenditureHeads);
        }
        public ActionResult ViewExpenditureHead(int nId, double ts)
        {
            ExpenditureHead oExpenditureHead = new ExpenditureHead();
            List<ExpenditureHeadMapping> oExpenditureHeadMappings = new List<ExpenditureHeadMapping>();
            ExpenditureHeadMapping oExpenditureHeadMapping = new ExpenditureHeadMapping();
            if (nId > 0)
            {
                oExpenditureHead = oExpenditureHead.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExpenditureHeadMappings = ExpenditureHeadMapping.Gets(oExpenditureHead.ExpenditureHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            oExpenditureHeadMappings = _oExpenditureHeadMappings;
            ViewBag.ExpenditureHeadMappings = oExpenditureHeadMappings;
            ViewBag.ExpenditureHeadTypes = EnumObject.jGets(typeof (EnumExpenditureHeadType));
            return View(oExpenditureHead);
        }

        [HttpPost]
        public JsonResult Save(ExpenditureHead oExpenditureHead)
        {
            oExpenditureHead.RemoveNulls();
            _oExpenditureHead = new ExpenditureHead();
            try
            {
                oExpenditureHead.ExpenditureHeadType = (EnumExpenditureHeadType)oExpenditureHead.ExpenditureHeadTypeInt;
                _oExpenditureHead = oExpenditureHead.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExpenditureHead = new ExpenditureHead();
                _oExpenditureHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExpenditureHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
     
       
        [HttpPost]
        public JsonResult Delete(ExpenditureHead oExpenditureHead)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExpenditureHead.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<ExpenditureHead> oExpenditureHeads = new List<ExpenditureHead>();
            oExpenditureHeads = ExpenditureHead.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oExpenditureHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetList(ExpenditureHeadMapping oExpenditureHeadMapping)
        {
            List<ExpenditureHeadMapping> oExpenditureHeadMappings = new List<ExpenditureHeadMapping>();
            try
            {
                List<EnumObject> oExpenditureTypeObjs = new List<EnumObject>();
                oExpenditureTypeObjs = EnumObject.jGets(typeof(EnumExpenditureType));
                foreach (EnumObject oItem in oExpenditureTypeObjs)
                {
                    //if (!IsExists((EnumExpenditureType)oItem.id))
                    //{
                        oExpenditureHeadMapping = new ExpenditureHeadMapping();
                        oExpenditureHeadMapping.OperationTypeInt = oItem.id;
                        oExpenditureHeadMapping.OperationType = (EnumExpenditureType)oItem.id;
                        oExpenditureHeadMappings.Add(oExpenditureHeadMapping);
                    //}
                }

            }
            catch (Exception ex)
            {
                oExpenditureHeadMappings = new List<ExpenditureHeadMapping>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExpenditureHeadMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Search  by Press Enter
        [HttpGet]
        public JsonResult searchByName(string sTempData, double ts)
        {
            _oExpenditureHeads = new List<ExpenditureHead>();
            string sSQL = "";
      
            sSQL = "SELECT * FROM View_ExpenditureHead WHERE Name LIKE'%" + sTempData + "%'";
           
            try
            {
                _oExpenditureHeads = ExpenditureHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExpenditureHead = new ExpenditureHead();
                _oExpenditureHead.ErrorMessage = ex.Message;
                _oExpenditureHeads.Add(_oExpenditureHead);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExpenditureHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByInvoiceNo(string sTempData, double ts)
        {
            _oExpenditureHeads = new List<ExpenditureHead>();
            string sSQL = "";

            sSQL = "SELECT * FROM View_ExpenditureHead where ExpenditureHeadID in (Select ExpenditureHeadID from View_ExpenditureHeadDetail where View_ExpenditureHeadDetail.ImportInvoiceNo  LIKE'%" + sTempData + "%')";

            try
            {
                _oExpenditureHeads = ExpenditureHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExpenditureHead = new ExpenditureHead();
                _oExpenditureHead.ErrorMessage = ex.Message;
                _oExpenditureHeads.Add(_oExpenditureHead);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExpenditureHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    
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
