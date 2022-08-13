using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class FabricQCGradeController : Controller
    {
        #region Declaration
        FabricQCGrade _oFabricQCGrade = new FabricQCGrade();
        List<FabricQCGrade> _oFabricQCGrades = new List<FabricQCGrade>();
        #endregion
        [HttpGet]
        public ActionResult ViewFabricQCGrades(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string ssSQL = "SELECT * FROM View_FabricQCGrade  AS HH ORDER BY HH.FabricQCGradeID ASC";
            _oFabricQCGrades = FabricQCGrade.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.QCGradeType = EnumObject.jGets(typeof(EnumFBQCGrade));
            ViewBag.GradeSLs = EnumObject.jGets(typeof(EnumExcellColumn));
            return View(_oFabricQCGrades);
        }

        [HttpPost]
        public JsonResult Save(FabricQCGrade oFabricQCGrade)
        {
            _oFabricQCGrade = new FabricQCGrade();
            try
            {
                _oFabricQCGrade = oFabricQCGrade;
                _oFabricQCGrade = _oFabricQCGrade.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricQCGrade = new FabricQCGrade();
                _oFabricQCGrade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricQCGrade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricQCGrade oFabricQCGrade = new FabricQCGrade();
                sFeedBackMessage = oFabricQCGrade.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Print List

        public ActionResult SetSessionSearchCriterias(FabricQCGrade oFabricQCGrade)
        {
            //this.Session.Remove(SessionInfo.ParamObj);
            //this.Session.Add(SessionInfo.ParamObj, oFabricQCGrade);

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            throw new NotImplementedException();
        }

        public ActionResult PrintLists()
        {
            //_oFabricQCGrade = new FabricQCGrade();
            //_oFabricQCGrades = new List<FabricQCGrade>();
            //_oFabricQCGrade = (FabricQCGrade)Session[SessionInfo.ParamObj];

            //string sSQL = "SELECT * FROM View_FabricQCGrade AS HH WHERE HH.FabricQCGradeID IN (" + _oFabricQCGrade.ErrorMessage + ") ORDER BY HH.FabricQCGradeID ASC";
            //_oFabricQCGrades = FabricQCGrade.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            //Company oCompany = new Company();
            //oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //rptFabricQCGrades oReport = new rptFabricQCGrades();
            //byte[] abytes = oReport.PrepareReport(_oFabricQCGrades, oCompany, "");
            //return File(abytes, "application/pdf");
            throw new NotImplementedException();
        }

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

    }
        #endregion
}