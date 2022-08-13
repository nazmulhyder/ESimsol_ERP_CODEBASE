using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ESimSol.Reports;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class TrainingDevelopmentController : Controller
    {
        #region Declaration
        TrainingDevelopment _oTrainingDevelopment;
        private List<TrainingDevelopment> _oTrainingDevelopments;
        #endregion

        #region Views
        public ActionResult View_TrainingDevelopments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            string sSql = "SELECT * FROM View_TrainingDevelopment WHERE IsCompleted = 0";
            _oTrainingDevelopments = TrainingDevelopment.Gets(sSql,((User)(Session[SessionInfo.CurrentUser])).UserID);

            //List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            //oAUOEDOs = AuthorizationUserOEDO.GetsByUser(((User)(Session[SessionInfo.CurrentUser])).UserID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //bool bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Approve, "TrainingDevelopment", oAUOEDOs);
            //TempData["Approve"] = bApprove;

            return View(_oTrainingDevelopments);
        }

        public ActionResult View_TrainingDevelopment(int nId, double ts)//nId=TDID 
        {
            _oTrainingDevelopment = new TrainingDevelopment();
            if (nId > 0)
            {
                _oTrainingDevelopment = TrainingDevelopment.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            //string sSql = "SELECT * FROM View_TrainingDevelopment";
            //_oTrainingDevelopment.oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return PartialView(_oTrainingDevelopment);
        }


        public ActionResult TrainingDevelopmentPikerByCourseName(string sCourseName, double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(CourseName), 0 AS TDID, 0 AS EmployeeID, '' AS Specification,"
                               +"'' AS Institute, '' AS Vendor, '' AS Country, '' AS [State], '' AS [Address]"
                               + ", '' AS Duration, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus, '' AS ApproveByName "
                               + " FROM View_TrainingDevelopment ";
                if (sCourseName != "")
                {
                    sSql = sSql + "WHERE CourseName ='" + sCourseName+"'";
                }
                sSql = sSql + " ORDER BY CourseName";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTrainingDevelopments.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }

        public ActionResult TrainingDevelopmentPikerForSpecification(string sCourseName ,double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(Specification), 0 AS TDID, 0 AS EmployeeID,'' AS CourseName,"
                               + "'' AS Institute, '' AS Vendor, '' AS Country, '' AS [State], '' AS [Address]"
                               + ", '' AS Duration, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus, '' AS ApproveByName "
                               + " FROM View_TrainingDevelopment WHERE CourseName ='" + sCourseName + "' ORDER BY Specification";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
               
            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }

        public ActionResult TrainingDevelopmentPikerForInstitute(string sCourseName, double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(Institute), 0 AS TDID, 0 AS EmployeeID,'' AS CourseName,"
                               + "'' AS Specification, '' AS Vendor, '' AS Country, '' AS [State], '' AS [Address]"
                               + ", '' AS Duration, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus, '' AS ApproveByName"
                               + " FROM View_TrainingDevelopment WHERE CourseName ='" + sCourseName + "' ORDER BY Institute";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }

        public ActionResult TrainingDevelopmentPikerForVendor(string sCourseName, double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(Vendor), 0 AS TDID, 0 AS EmployeeID,'' AS CourseName,"
                               + "'' AS Specification, '' AS Institute, '' AS Country, '' AS [State], '' AS [Address]"
                               + ", '' AS Duration, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus ,'' AS ApproveByName"
                               + " FROM View_TrainingDevelopment WHERE CourseName ='" + sCourseName + "' ORDER BY Vendor";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }

        public ActionResult TrainingDevelopmentPikerForCountry(double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(Country), 0 AS TDID, 0 AS EmployeeID, '' AS Specification,"
                               + "'' AS Institute, '' AS Vendor, '' AS CourseName, '' AS [State], '' AS [Address]"
                               + ", '' AS Duration, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus ,'' AS ApproveByName"
                               + " FROM View_TrainingDevelopment ORDER BY Country";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTrainingDevelopments.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }

        public ActionResult TrainingDevelopmentPikerForState(string sCountryName, double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(State), 0 AS TDID, 0 AS EmployeeID,'' AS CourseName,"
                               + "'' AS Specification, '' AS Institute, '' AS Country, '' AS Vendor, '' AS [Address]"
                               + ", '' AS Duration, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus ,'' AS ApproveByName"
                               + " FROM View_TrainingDevelopment WHERE Country ='" + sCountryName + "' ORDER BY State";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }

        public ActionResult TrainingDevelopmentPikerForAddress(string sInstituteName, double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(Address), 0 AS TDID, 0 AS EmployeeID,'' AS CourseName,"
                               + "'' AS Specification, '' AS Institute, '' AS Country, '' AS Vendor, '' AS State"
                               + ", '' AS Duration, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus ,'' AS ApproveByName"
                               + " FROM View_TrainingDevelopment WHERE Institute ='" + sInstituteName + "' ORDER BY Address";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }

        public ActionResult TrainingDevelopmentPikerForDuration(string sCourseName, double nts)
        {
            _oTrainingDevelopments = new List<TrainingDevelopment>();
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "SELECT DISTINCT(Duration), 0 AS TDID, 0 AS EmployeeID,'' AS CourseName,"
                               + "'' AS Specification, '' AS Institute, '' AS Country, '' AS [State], '' AS [Address]"
                               + ", '' AS Vendor, GETDATE() AS StartDate, GETDATE() AS EndDate, GETDATE() AS EffectFromDate,"
                               + "GETDATE() AS EffectToDate, '' AS Note, 0 AS ApproveBy, '' AS ApprovalNote, '' AS Result,"
                               + "GETDATE() AS PassingDate, '' AS ResultNote, 1 AS IsCompleted, 1 AS IsActive"
                               + ",'' AS InactiveNote, GETDATE() AS InactiveDate, '' AS EmployeeName, '' AS EmployeeCode,"
                               + "'' AS DesignationName, '' AS DepartmentName, '' AS LocationName, '' AS EmployeeTypeName, 0 AS WorkingStatus, '' AS ApproveByName "
                               + " FROM View_TrainingDevelopment WHERE CourseName ='" + sCourseName + "' ORDER BY Duration";

                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }
            return PartialView(_oTrainingDevelopments);
        }


        #endregion
        [HttpPost]
        public JsonResult GetLastTrainingInfo(int nEmployeeID)
        {

            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                if (nEmployeeID > 0)
                {
                    string sSql = "SELECT * FROM View_TrainingDevelopment WHERE TDID= (SELECT MAX(TDID) FROM View_TrainingDevelopment WHERE EmployeeID=" + nEmployeeID + ")";
                    _oTrainingDevelopment = TrainingDevelopment.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                }
                else
                {
                    throw new Exception("Please Select an Employee !!");
                }
                
            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region IUD
        [HttpPost]
        public JsonResult TrainingDevelopment_IU(TrainingDevelopment oTrainingDevelopment)
        {

            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                _oTrainingDevelopment = oTrainingDevelopment;

                if (_oTrainingDevelopment.TDID > 0)
                {
                    _oTrainingDevelopment = _oTrainingDevelopment.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oTrainingDevelopment = _oTrainingDevelopment.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TrainingDevelopment_Delete(int nTDID, double ts)//nnTDID=nTDID
        {
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {

                _oTrainingDevelopment.TDID = nTDID;
                _oTrainingDevelopment = _oTrainingDevelopment.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Activity
        [HttpPost]
        public JsonResult TrainingDevelopment_Activity(TrainingDevelopment oTrainingDevelopment)
        {
            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {

                _oTrainingDevelopment = TrainingDevelopment.Activite(oTrainingDevelopment.TDID, oTrainingDevelopment.TDID, oTrainingDevelopment.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Approve
        [HttpPost]
        public JsonResult TrainingDevelopment_Approve(string sApprovalNote , int nTDID)
        {

            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "UPDATE TrainingDevelopment SET ApprovalNote ='" + sApprovalNote + "',ApproveBy=" + ((User)Session[SessionInfo.CurrentUser]).UserID + "WHERE TDID=" + nTDID
                             + "SELECT * FROM View_TrainingDevelopment WHERE TDID =" + nTDID;
                _oTrainingDevelopment = TrainingDevelopment.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
              
            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Approve

        #region Finalize
        [HttpPost]
        public JsonResult TrainingDevelopment_Finalize(string sResult, string sResultNote, string dPassingDate, int IsComplete, int nTDID)
        {

            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "UPDATE TrainingDevelopment SET Result ='" + sResult + "',ResultNote='" + sResultNote + "', PassingDate ='" + dPassingDate + "', IsCompleted =" + IsComplete + " WHERE TDID=" + nTDID
                             + "SELECT * FROM View_TrainingDevelopment WHERE TDID =" + nTDID;
                _oTrainingDevelopment = TrainingDevelopment.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Finalize

        #region InActive
        [HttpPost]
        public JsonResult TrainingDevelopment_InActive(string sInActiveNote, int nTDID)
        {

            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                string sSql = "UPDATE TrainingDevelopment SET InactiveNote ='" + sInActiveNote + "', InactiveDate= '" + DateTime.Now + "', IsActive=0" + " WHERE TDID=" + nTDID
                             + "SELECT * FROM View_TrainingDevelopment WHERE TDID =" + nTDID;
                _oTrainingDevelopment = TrainingDevelopment.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion InActive

        #region Search


        [HttpPost]
        public JsonResult Get(TrainingDevelopment oTrainingDevelopment)
        {

            _oTrainingDevelopment = new TrainingDevelopment();
            try
            {
                _oTrainingDevelopment = TrainingDevelopment.Get(oTrainingDevelopment.TDID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TrainingDevelopment_Search(string sParam)
        {

            _oTrainingDevelopments = new List<TrainingDevelopment>();

            int nEmployeeID =Convert.ToInt32( sParam.Split('~')[0]);
            string sCourseName = sParam.Split('~')[1];
            int nStatus = Convert.ToInt16(sParam.Split('~')[2]);
            int nAll = Convert.ToInt16(sParam.Split('~')[3]);
            

            _oTrainingDevelopments = new List<TrainingDevelopment>();
            string sSql = "";

            sSql = "SELECT * FROM View_TrainingDevelopment WHERE TDID <>0";

            if (nEmployeeID != 0)
            {
                sSql = sSql + "AND EmployeeID = " + nEmployeeID;
            }

            if (sCourseName != "")
            {
                sSql = sSql + "AND CourseName = '" + sCourseName + "'"; 
            }
            if (nStatus != 0)
            {
                if (nStatus == 1)
                {
                    sSql = sSql + "AND  ApproveBy <= 0";
                }
                else if (nStatus == 2)
                {
                    sSql = sSql + "AND ApproveBy > 0 AND EffectFromDate > '" +DateTime.Now+"'";
                }
                else if (nStatus == 3)
                {
                    sSql = sSql + "AND  ApproveBy > 0 AND EffectFromDate <='"+ DateTime.Now+"' AND '"+  DateTime.Now +"'<= EffectToDate"+ " AND  IsCompleted == 0";
                    
                }
                else if (nStatus == 4)
                {

                    sSql = sSql + "AND  ApproveBy > 0 AND EffectToDate <='"+ DateTime.Now +"'AND IsCompleted == 0";
                }
                else if (nStatus == 5)
                {
                    sSql = sSql + "AND  IsCompleted =1";
                }
                
                else if (nStatus == 6)
                {
                    sSql = sSql + "AND  IsActive ==0";
                }

            }
            try
            {
                _oTrainingDevelopments = TrainingDevelopment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (_oTrainingDevelopments.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

            }
            catch (Exception ex)
            {
                _oTrainingDevelopment = new TrainingDevelopment();
                _oTrainingDevelopments = new List<TrainingDevelopment>();
                _oTrainingDevelopment.ErrorMessage = ex.Message;
                _oTrainingDevelopments.Add(_oTrainingDevelopment);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTrainingDevelopments);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        
        #endregion Search

    }
}
