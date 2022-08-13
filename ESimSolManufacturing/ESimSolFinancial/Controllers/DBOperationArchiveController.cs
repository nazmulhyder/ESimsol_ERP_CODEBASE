using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class DBOperationArchiveController : Controller
    {
        #region Declaration

        DBOperationArchive _oDBOperationArchive = new DBOperationArchive();
        List<DBOperationArchive> _oDBOperationArchives = new List<DBOperationArchive>();

        #endregion

        #region Actions

        public ActionResult ViewDBOperationArchives(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oDBOperationArchives = new List<DBOperationArchive>();
            string sSQL = "SELECT * FROM View_DBOperationArchive AS HH WHERE CONVERT(DATE,CONVERT(VARCHAR(12),HH.DBServerDateTime, 106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + DateTime.Today.ToString("dd MMM yyyy") + "', 106)) ORDER BY HH.DBOperationArchiveID ASC";
            _oDBOperationArchives = DBOperationArchive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT * FROM BusinessUnit AS BU ORDER BY BU.BusinessUnitID ASC";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            EnumObject oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumDBOperation.Update;
            oEnumObject.Value = EnumObject.jGet(EnumDBOperation.Update);
            oEnumObjects.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumDBOperation.Delete;
            oEnumObject.Value = EnumObject.jGet(EnumDBOperation.Delete);
            oEnumObjects.Add(oEnumObject);

            ViewBag.DBOperationTypes = oEnumObjects;
            return View(_oDBOperationArchives);
        }

        #endregion

        #region Search

        [HttpPost]
        public JsonResult Search(DBOperationArchive oDBOperationArchive)
        {
            List<DBOperationArchive> oDBOperationArchives = new List<DBOperationArchive>();
            try
            {
                string sSQL = this.GetSQL(oDBOperationArchive);
                oDBOperationArchives = DBOperationArchive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDBOperationArchive = new DBOperationArchive();
                _oDBOperationArchive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDBOperationArchives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(DBOperationArchive oDBOperationArchive)
        {          
            string sReturn1 = "SELECT * FROM View_DBOperationArchive AS HH";
            string sReturn = "";

            #region BU
            if (oDBOperationArchive.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BUID = " + oDBOperationArchive.BUID.ToString();
            }
            #endregion

            #region DBOperationType
            if (oDBOperationArchive.DBOperationType != EnumDBOperation.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.DBOperationType = " + ((int)oDBOperationArchive.DBOperationType).ToString();
            }
            #endregion
            
            #region ModuleName
            if (oDBOperationArchive.ModuleName != EnumModuleName.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ModuleName = " + ((int)oDBOperationArchive.ModuleName).ToString();
            }
            #endregion

            #region RefText
            if (!string.IsNullOrEmpty(oDBOperationArchive.RefText))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.RefText LIKE '%" + oDBOperationArchive.RefText + "%'";
            }
            #endregion

            #region DB ServerDateTime
            if (oDBOperationArchive.IsDateSearch)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.DBServerDateTime, 106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oDBOperationArchive.StartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oDBOperationArchive.EndDate.ToString("dd MMM yyyy") + "', 106))";
            }
            #endregion
           
            sReturn = sReturn1 + sReturn + " ORDER BY HH.DBOperationArchiveID ASC";
            return sReturn;
        }

        #endregion

        #region PDF

        [HttpPost]
        public ActionResult SetSessionData(DBOperationArchive oDBOperationArchive)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDBOperationArchive);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintList(double ts)
        {
            DBOperationArchive oDBOperationArchive = new DBOperationArchive();
            List<DBOperationArchive> oDBOperationArchives = new List<DBOperationArchive>();
            try
            {
                oDBOperationArchive = (DBOperationArchive)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_DBOperationArchive AS HH WHERE HH.DBOperationArchiveID IN (" + oDBOperationArchive.ErrorMessage + ") ORDER BY HH.DBOperationArchiveID ASC";
                oDBOperationArchives = DBOperationArchive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDBOperationArchive = new DBOperationArchive();
                throw new Exception(oDBOperationArchive.ErrorMessage);
            }

            this.Session.Remove(SessionInfo.ParamObj);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDBOperationArchives rptDBOperationArchive = new rptDBOperationArchives();
            byte[] abytes = rptDBOperationArchive.PrepareReport(oDBOperationArchives, oCompany);
            return File(abytes, "application/pdf");

        }

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
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetsModuleAutocomplete(string ModuleName)
        {
            if (ModuleName == null) { ModuleName = ""; }
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            List<EnumObject> oTempEnumObjects = new List<EnumObject>();
            oTempEnumObjects = EnumObject.jGets(typeof(EnumModuleName));
            foreach (EnumObject oItem in oTempEnumObjects)
            {
                if (oItem.Value.ToUpper().IndexOf(ModuleName.ToUpper()) >= 0)
                {
                    oEnumObjects.Add(oItem);
                }
            }

            var jsonResult = Json(oEnumObjects, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
    }
}