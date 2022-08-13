using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using iTextSharp.text;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class ApprovalHeadController : PdfViewController
    {
        #region Declaration
        ApprovalHead _oApprovalHead;
        List<ApprovalHead> _oApprovalHeads;
        ApprovalHeadPerson _oApprovalHeadPerson;
        List<ApprovalHeadPerson> _oApprovalHeadPersons;
        int AHID;
        
        #endregion

        #region Views
        public ActionResult ViewApprovalHeads(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oApprovalHeads = new List<ApprovalHead>();
            ViewBag.BUs = BusinessUnit.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Module = EnumObject.jGets(typeof(EnumModuleName)).OrderBy(x=>x.Value);
            return View(_oApprovalHeads);
        }
        public ActionResult ViewApprovalHeadPerson(int id)
        {
            AHID = id;
            _oApprovalHeadPersons = new List<ApprovalHeadPerson>();
            string sSQL = "select * from View_ApprovalHeadPerson WHERE ApprovalHeadID = " + id;
            _oApprovalHeadPersons = ApprovalHeadPerson.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.Head = ApprovalHead.Get("select * from ApprovalHead where ApprovalHeadID =" + id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.AHID = id;
            return View(_oApprovalHeadPersons);
        }

        #endregion

        #region IUD ApprovalHead
        [HttpPost]
        public JsonResult ApprovalHead_IU(ApprovalHead oApprovalHead)
        {
            _oApprovalHead = new ApprovalHead();
            try
            {
                _oApprovalHead = oApprovalHead;
                if (_oApprovalHead.ApprovalHeadID > 0)
                {
                    _oApprovalHead = _oApprovalHead.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oApprovalHead = _oApprovalHead.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oApprovalHead = new ApprovalHead();
                _oApprovalHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oApprovalHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovalHead_Delete(ApprovalHead oApprovalHead)
        {
            _oApprovalHead = new ApprovalHead();
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            try
            {
                _oApprovalHead = oApprovalHead.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oApprovalHead.ErrorMessage == "Deleted")
                {
                    try
                    {
                        string sSql = "SELECT * from ApprovalHead where ModuleID IN (" + (int)oApprovalHead.ModuleID + ")  AND BUID = "+oApprovalHead.BUID+"  Order By Sequence";
                        oApprovalHeads = ApprovalHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    catch (Exception ex)
                    {
                        _oApprovalHead = new ApprovalHead();
                        _oApprovalHead.ErrorMessage = ex.Message;
                        oApprovalHeads.Add(_oApprovalHead);
                    }
                }
                else
                {
    
                    oApprovalHeads.Add(_oApprovalHead);
                }
               
               
            }
            catch (Exception ex)
            {
                _oApprovalHead = new ApprovalHead();
                _oApprovalHead.ErrorMessage = ex.Message;
                oApprovalHeads.Add(_oApprovalHead);
            }

            

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region IUD ApprovalHeadPerson


        [HttpPost]
        public JsonResult ApprovalHeadPerson_IU(ApprovalHead oApprovalHead)
        {
            _oApprovalHeadPersons = new List<ApprovalHeadPerson>();
            _oApprovalHeadPerson = new ApprovalHeadPerson();
            _oApprovalHead = new ApprovalHead();
            try
            {
                _oApprovalHead = _oApprovalHeadPerson.IUD(oApprovalHead, (int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
             
            }
            catch (Exception ex)
            {
                _oApprovalHead = new ApprovalHead();
                _oApprovalHead.ErrorMessage = ex.Message;
            }
          
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oApprovalHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovalHeadPerson_Delete(ApprovalHeadPerson oApprovalHeadPerson)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oApprovalHeadPerson.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ActivateApprovalHeadPerson(ApprovalHeadPerson oApprovalHeadPerson)
        {
            _oApprovalHeadPerson = new ApprovalHeadPerson();
            _oApprovalHeadPerson = oApprovalHeadPerson.Activate(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oApprovalHeadPerson);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Get
        [HttpGet]
        public JsonResult Gets(string sTemp, double ts)
        {
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            try
            {
                string sSql = "SELECT * from ApprovalHead where ApprovalHeadID IN (" + sTemp + ")";
                oApprovalHeads = ApprovalHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oApprovalHead = new ApprovalHead();
                _oApprovalHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetModule(ApprovalHead oApprovalHead)
        {
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            try
            {
                string sSql = "SELECT * from ApprovalHead where ModuleID IN (" + (int)oApprovalHead.ModuleID + ") AND BUID = "+oApprovalHead.BUID+"  Order By Sequence";
                oApprovalHeads = ApprovalHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oApprovalHead = new ApprovalHead();
                _oApprovalHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpDown(ApprovalHead oApprovalHead)
        {
            ApprovalHead oAH = new ApprovalHead();
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            try
            {
                oAH = oAH.UpDown(oApprovalHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oApprovalHead = new ApprovalHead();
                _oApprovalHead.ErrorMessage = ex.Message;
            }
            if (oAH.ErrorMessage == "")
            {
                try
                {
                    string sSql = "SELECT * from ApprovalHead where ModuleID IN (" + (int)oApprovalHead.ModuleID + ") AND BUID = "+oApprovalHead.BUID+" Order By Sequence";
                    oApprovalHeads = ApprovalHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                catch (Exception ex)
                {
                    _oApprovalHead = new ApprovalHead();
                    _oApprovalHead.ErrorMessage = ex.Message;
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get(string sSql, double ts)
        {
            ApprovalHead oApprovalHead = new ApprovalHead();
            try
            {
                oApprovalHead = ApprovalHead.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oApprovalHead = new ApprovalHead();
                oApprovalHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region For Send

        [HttpPost]
        public JsonResult GetHeadAndPerson(ApprovalHead oApprovalHead)
        {
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            List<ApprovalHeadPerson> oApprovalHeadPersons = new List<ApprovalHeadPerson>();
            string sSql;
            try
            {
                sSql = "SELECT TOP(1)* from ApprovalHead where ModuleID=" + (int)oApprovalHead.ModuleID +"  AND Sequence=" + oApprovalHead.Sequence;
                if(oApprovalHead.BUID>0)
                {
                    sSql += " AND BUID =" + oApprovalHead.BUID;
                }
                oApprovalHeads = ApprovalHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sSql = "SELECT * FROM View_ApprovalHeadPerson where ApprovalHeadID=" + oApprovalHeads[0].ApprovalHeadID;
                oApprovalHeadPersons = ApprovalHeadPerson.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oApprovalHeads[0].ApprovalHeadPersons = oApprovalHeadPersons;
            }
            catch (Exception ex)
            {
                _oApprovalHead = new ApprovalHead();
                _oApprovalHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion


    }
}
