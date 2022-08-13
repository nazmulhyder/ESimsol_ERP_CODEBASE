using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;
using ReportManagement;

namespace ESimSolFinancial.Controllers
{

    public class DBTableReferenceController : Controller
    {
        #region Declaration
        DBTableReference _oDBTableReference = new DBTableReference();
        List<DBTableReference> _oDBTableReferences = new List<DBTableReference>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
     
        #endregion

        #region Actions
        public ActionResult ViewDBTableReferences(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'DBTableReference'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oDBTableReferences = new List<DBTableReference>();
            _oDBTableReferences = DBTableReference.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oDBTableReferences);
        }

        public ActionResult ViewDBTableReference(int id, double ts)
        {
            _oDBTableReference = new DBTableReference();
            if (id > 0)
            {
                _oDBTableReference = _oDBTableReference.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oDBTableReference.MainTables = DBTableReference.GetsDBObject(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDBTableReference.ReferenceTables = DBTableReference.GetsDBObject(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oDBTableReference);
        }

        [HttpPost]
        public JsonResult Save(DBTableReference oDBTableReference)
        {
            _oDBTableReference = new DBTableReference();
            try
            {
                _oDBTableReference = oDBTableReference;
                _oDBTableReference = _oDBTableReference.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDBTableReference = new DBTableReference();
                _oDBTableReference.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDBTableReference);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id, double ts)
        {
            string sFeedBackMessage = "";
            try
            {
                DBTableReference oDBTableReference = new DBTableReference();
                sFeedBackMessage = oDBTableReference.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetTableColumns(string sReferenceTable, double ts)
        {
            List<DBObjectColumn> oReferenceColumns = new List<DBObjectColumn>();
            try
            {

                oReferenceColumns = DBTableReference.GetsDBObjectColumn(sReferenceTable, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                DBObjectColumn oDBObjectColumn = new DBObjectColumn();
                oDBObjectColumn.ColumnName= ex.Message;
                oReferenceColumns.Add(oDBObjectColumn);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oReferenceColumns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion


        //#region Printing
        //public ActionResult PrintDBTableReferences()
        //{
        //    _oDBTableReference = new DBTableReference();
        //    _oDBTableReference.DBTableReferences = DBTableReference.Gets((int)Session[SessionInfo.currentUserID]);
        //    GroupInfo oGroupInfo = new GroupInfo();
        //    oGroupInfo = oGroupInfo.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oGroupInfo.GroupInfoLogo = GetGroupInfoLogo(oGroupInfo);
        //    _oDBTableReference.GroupInfo = oGroupInfo;

        //    string Messge = "DBTableReference List";
        //    rptDBTableReferences oReport = new rptDBTableReferences();
        //    byte[] abytes = oReport.PrepareReport(_oDBTableReference, Messge);
        //    return File(abytes, "application/pdf");
        //}

        //public Image GetGroupInfoLogo(GroupInfo oGroupInfo)
        //{
        //    if (oGroupInfo.OrganizationLogo != null)
        //    {
        //        MemoryStream m = new MemoryStream(oGroupInfo.OrganizationLogo);
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
