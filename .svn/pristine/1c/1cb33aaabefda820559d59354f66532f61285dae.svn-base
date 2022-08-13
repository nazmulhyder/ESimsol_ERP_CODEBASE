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
using System.Drawing.Printing;
using System.Reflection;

namespace ESimSolFinancial.Controllers
{
    public class SourcingConfigHeadController : Controller
    {
        #region Declaration
        SourcingConfigHead _oSourcingConfigHead = new SourcingConfigHead();
        List<SourcingConfigHead> _oSourcingConfigHeads = new List<SourcingConfigHead>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewSourcingConfigHeads(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser("SourcingConfigHead", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oSourcingConfigHeads = new List<SourcingConfigHead>();
            _oSourcingConfigHeads = SourcingConfigHead.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oSourcingConfigHeads);
        }

        public ActionResult ViewSourcingConfigHead(int id)
        {
            _oSourcingConfigHead = new SourcingConfigHead();
            if (id > 0)
            {
                _oSourcingConfigHead = _oSourcingConfigHead.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.SourcingConfigHeadTypes = EnumObject.jGets(typeof(EnumSourcingConfigHeadType));
            return View(_oSourcingConfigHead);
        }

        [HttpPost]
        public JsonResult Save(SourcingConfigHead oSourcingConfigHead)
        {
            _oSourcingConfigHead = new SourcingConfigHead();
            try
            {
                _oSourcingConfigHead = oSourcingConfigHead.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSourcingConfigHead = new SourcingConfigHead();
                _oSourcingConfigHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSourcingConfigHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                SourcingConfigHead oSourcingConfigHead = new SourcingConfigHead();
                sFeedBackMessage = oSourcingConfigHead.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region gets
        public JsonResult GetSourcingHead(SourcingConfigHead oSourcingConfigHead)
        {
            List<SourcingConfigHead> oSourcingConfigHeads = new List<SourcingConfigHead>();
            try
            {
                string Ssql = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadType IN ( " + oSourcingConfigHead.ErrorMessage + ")";
                if (oSourcingConfigHead.SourcingConfigHeadName == null)
                {
                    oSourcingConfigHead.SourcingConfigHeadName = "";
                }
                if (oSourcingConfigHead.SourcingConfigHeadName != "")
                {
                    Ssql = Ssql + " AND SourcingConfigHeadName LIKE '%" + oSourcingConfigHead.SourcingConfigHeadName + "%'";
                }
                oSourcingConfigHeads = SourcingConfigHead.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSourcingConfigHeads = new List<SourcingConfigHead>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSourcingConfigHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWash(string SourcingConfigHeadName)
        {
            List<SourcingConfigHead> oSourcingConfigHeads = new List<SourcingConfigHead>();
            try
            {
                //string Ssql = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadName LIKE '%" + SourcingConfigHeadName + "%' AND SourcingConfigHeadType IN ( " + (int)EnumSourcingConfigHeadType.Wash + "," + (int)EnumSourcingConfigHeadType.Yard + ")";
                string Ssql = "SELECT * FROM SourcingConfigHead";
                oSourcingConfigHeads = SourcingConfigHead.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSourcingConfigHeads = new List<SourcingConfigHead>();
            }
            var jsonResult = Json(oSourcingConfigHeads, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetFinishDia(string SourcingConfigHeadName)
        {
            List<SourcingConfigHead> oSourcingConfigHeads = new List<SourcingConfigHead>();
            try
            {
                string Ssql = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadName LIKE '%" + SourcingConfigHeadName + "%' AND SourcingConfigHeadType IN ( " + (int)EnumSourcingConfigHeadType.FinishDIA + ")";
                oSourcingConfigHeads = SourcingConfigHead.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSourcingConfigHeads = new List<SourcingConfigHead>();
            }
            var jsonResult = Json(oSourcingConfigHeads, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetGSM(string SourcingConfigHeadName)
        {
            List<SourcingConfigHead> oSourcingConfigHeads = new List<SourcingConfigHead>();
            try
            {
                string Ssql = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadName LIKE '%" + SourcingConfigHeadName + "%' AND SourcingConfigHeadType IN ( " + (int)EnumSourcingConfigHeadType.GSM + ")";
                oSourcingConfigHeads = SourcingConfigHead.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSourcingConfigHeads = new List<SourcingConfigHead>();
            }
            var jsonResult = Json(oSourcingConfigHeads, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Searching
        public ActionResult SourcingConfigHeadSearch()
        {
            _oSourcingConfigHeads = new List<SourcingConfigHead>();
            return PartialView(_oSourcingConfigHeads);
        }

        [HttpPost]
        public JsonResult GetsByNameAndTpe(SourcingConfigHead oSourcingConfigHead)
        {
            List<SourcingConfigHead> oSourcingConfigHeads = new List<SourcingConfigHead>();
            string sSQL = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadType = " + (int)oSourcingConfigHead.SourcingConfigHeadType + " AND SourcingConfigHeadName LIKE '%" + oSourcingConfigHead.SourcingConfigHeadName + "%'";
            oSourcingConfigHeads = SourcingConfigHead.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oSourcingConfigHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets()
        {
            List<SourcingConfigHead> oSourcingConfigHeads = new List<SourcingConfigHead>();
            oSourcingConfigHeads = SourcingConfigHead.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oSourcingConfigHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult SearchSize(SourcingConfigHead oSourcingConfigHead)
        //{
        //    List<SourcingConfigHead> oSourcingConfigHeads = new List<SourcingConfigHead>();
        //    if (oSourcingConfigHead.Param == null || oSourcingConfigHead.Param == "")
        //    {
        //        oSourcingConfigHeads = SourcingConfigHead.Gets((int)Session[SessionInfo.currentUserID]);
        //    }
        //    else
        //    {
        //        oSourcingConfigHeads = SourcingConfigHead.GetsBySourcingConfigHead(oSourcingConfigHead.Param, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oSourcingConfigHeads);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);

        //}
        #endregion

        #region Print List
        //public ActionResult PrintList(string sIDs)
        //{

        //    _oSourcingConfigHead = new SourcingConfigHead();
        //    string sSQL = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadID IN (" + sIDs + ") ORDER BY Sequence ASC";
        //    _oSourcingConfigHead.SizeCategories = SourcingConfigHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    rptSourcingConfigHeadList oReport = new rptSourcingConfigHeadList();
        //    byte[] abytes = oReport.PrepareReport(_oSourcingConfigHead, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
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
        #endregion

    }
}
