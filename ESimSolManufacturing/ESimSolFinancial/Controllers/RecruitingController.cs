using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class RecruitingController : Controller
    {
        #region Declaration
        Circular _oCircular;
        private List<Circular> _oCirculars;
        #endregion

        #region Views

        public ActionResult View_Circulars(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oCirculars = new List<Circular>();
            _oCirculars = Circular.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oCirculars);
        }

        public ActionResult View_Circular(int nId, double ts)
        {
            _oCircular = new Circular();

            if (nId > 0)
            {
                _oCircular = Circular.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            return PartialView(_oCircular);
        }
        public ActionResult View_vacancys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<Vacancy> oVacancys = new List<Vacancy>();
            string sSql = "SELECT * FROM View_Vacancy  WHERE  (RequiredPerson-ExistPerson)>0";
            oVacancys = Vacancy.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oVacancys);
        }
        public ActionResult View_Applications(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<CandidateApplication> oCandidateApplications = new List<CandidateApplication>();
            string sSql = "SELECT * FROM View_CandidateApplication WHERE IsActive = 1";
            oCandidateApplications = CandidateApplication.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oCandidateApplications);
        }

        #endregion

        #region IUD
        [HttpPost]
        public JsonResult Circular_IU(Circular oCircular)
        {

            _oCircular = new Circular();
            try
            {
                _oCircular = oCircular;
                if (_oCircular.CircularID > 0)
                {
                    _oCircular = _oCircular.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oCircular = _oCircular.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oCircular = new Circular();
                _oCircular.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCircular);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Circular_Delete(int nId, double ts)//nId=CircularID
        {
            _oCircular = new Circular();
            try
            {

                _oCircular.CircularID = nId;
                _oCircular = _oCircular.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oCircular = new Circular();
                _oCircular.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCircular.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Search
        [HttpPost]
        public JsonResult Circular_Search(Circular oCircular, bool IsDate)
        {

            string sSQL = "";
            sSQL = "SELECT * FROM View_Circular WHERE CircularID <>0";
           
            if (oCircular.DepartmentID != 0)
            {
                sSQL = sSQL + " AND DepartmentID = " + oCircular.DepartmentID;
            }
            if (oCircular.DesignationID != 0)
            {
                sSQL = sSQL + " AND DesignationID = " + oCircular.DesignationID;
            }
            if(IsDate == true)
            {
                sSQL = sSQL + " AND (StartDate BETWEEN '" + oCircular.StartDate + "' AND '" + oCircular.EndDate+"' OR EndDate BETWEEN '"+ oCircular.StartDate + "' AND '" + oCircular.EndDate+"')";
            }
          
            _oCirculars = Search(sSQL);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCirculars);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public List<Circular> Search(string sSQL)
        {
            _oCirculars = new List<Circular>();
            try
            {
                _oCirculars = Circular.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oCirculars.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

            }
            catch (Exception ex)
            {
                _oCircular = new Circular();
                _oCirculars = new List<Circular>();
                _oCircular.ErrorMessage = ex.Message;
                _oCirculars.Add(_oCircular);
            }
            return _oCirculars;
        }

        #endregion

        #region Activity
        [HttpPost]
        public JsonResult Circular_Activity(Circular oCircular)
        {
            _oCircular = new Circular();
            try
            {

                _oCircular = oCircular;
                _oCircular = Circular.Activity(_oCircular.CircularID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oCircular = new Circular();
                _oCircular.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCircular);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Approve
        [HttpPost]
        public JsonResult Circular_Approve(int nId)// nID=CircularID
        {

            _oCircular = new Circular();
            try
            {
                //_oCircular = new Circular();
                //string sSql = "SELECT * FROM View_Circular WHERE CircularID=" + nId;
                //_oCircular = Circular.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //if (_oCircular.CircularID <= 0)
                //{
                //    throw new Exception("Invalid Employee Production Sheet!");
                //}
                //else if (_oCircular.ApproveBy > 0)
                //{

                //    throw new Exception("Already Approved!");

                //}
                //else if (((User)(Session[SessionInfo.CurrentUser])).UserID <= 0)
                //{

                //    throw new Exception("SuperUser has no permission to approve !");

                //}
                //else
                //{
                //    _oCircular = new Circular();
                    string sSql = "UPDATE Circular SET ApproveBy=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ",ApproveByDate='" + DateTime.Now + "' WHERE CircularID=" + nId + ";SELECT * FROM View_Circular WHERE CircularID=" + nId;
                    _oCircular = Circular.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //}
            }
            catch (Exception ex)
            {
                _oCircular = new Circular();
                _oCircular.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCircular);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Approve


    }
}
