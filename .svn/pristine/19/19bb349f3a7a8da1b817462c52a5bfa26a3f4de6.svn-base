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
using System.Data;
using System.Data.OleDb;


namespace ESimSolFinancial.Controllers
{
    public class PerformanceIncentiveController : Controller
    {
        #region Declaration
        PerformanceIncentive _oPerformanceIncentive;
        List<PerformanceIncentive> _oPerformanceIncentives;

        #endregion

        #region Views
 
        public ActionResult View_PerformanceIncentives(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPerformanceIncentives = new List<PerformanceIncentive>();
            string sSql = "SELECT *  FROM View_PerformanceIncentive";
            _oPerformanceIncentives = PerformanceIncentive.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oPerformanceIncentives);
        }

        public ActionResult View_PerformanceIncentive(string sid, string sMsg)
        {
            _oPerformanceIncentive = new PerformanceIncentive();
            int nPIID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            if (nPIID > 0)
            {
                _oPerformanceIncentive = PerformanceIncentive.Get(nPIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string sSql = "SELECT *  FROM PerformanceIncentiveSlab WHERE PIID=" + nPIID;
                _oPerformanceIncentive.PerformanceIncentiveSlabs = PerformanceIncentiveSlab.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            string sSql_SalaryHead = "SELECT * FROM SalaryHead WHERE SalaryHeadType=2 AND IsActive=1";//Only Addition Type salary head
            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            oSalaryHeads = SalaryHead.Gets(sSql_SalaryHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.SalaryHeads = oSalaryHeads;
            return View(_oPerformanceIncentive);
        }

        #endregion

        #region PerformanceIncentive_IUD
        [HttpPost]
        public JsonResult PerformanceIncentive_IU(PerformanceIncentive oPerformanceIncentive)
        {
            _oPerformanceIncentive = new PerformanceIncentive();
            try
            {
                _oPerformanceIncentive = oPerformanceIncentive;
                if (_oPerformanceIncentive.PIID > 0)
                {
                    _oPerformanceIncentive = _oPerformanceIncentive.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oPerformanceIncentive = _oPerformanceIncentive.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oPerformanceIncentive = new PerformanceIncentive();
                _oPerformanceIncentive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPerformanceIncentive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PI_Delete(PerformanceIncentive oPerformanceIncentive)
        {
            string sErrorMease = "";
            try
            {
                oPerformanceIncentive = oPerformanceIncentive.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oPerformanceIncentive.ErrorMessage;
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
        public JsonResult PerformanceIncentiveSlab_IU(PerformanceIncentiveSlab  oPerformanceIncentiveSlab)
        {
            _oPerformanceIncentive = new PerformanceIncentive();
            try
            {
                _oPerformanceIncentive = oPerformanceIncentiveSlab.PerformanceIncentive;
                if (_oPerformanceIncentive.PIID <= 0)
                {
                    _oPerformanceIncentive = _oPerformanceIncentive.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }

                if (_oPerformanceIncentive.PIID > 0 && string.IsNullOrEmpty(_oPerformanceIncentive.ErrorMessage))
                {
                    oPerformanceIncentiveSlab.PIID = _oPerformanceIncentive.PIID;
                    if (oPerformanceIncentiveSlab.PISlabID > 0)
                    {
                        oPerformanceIncentiveSlab = oPerformanceIncentiveSlab.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    else
                    {
                        oPerformanceIncentiveSlab = oPerformanceIncentiveSlab.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    oPerformanceIncentiveSlab.PerformanceIncentive = _oPerformanceIncentive;
                }
                else {
                    oPerformanceIncentiveSlab.ErrorMessage = _oPerformanceIncentive.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                oPerformanceIncentiveSlab = new PerformanceIncentiveSlab();
                oPerformanceIncentiveSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPerformanceIncentiveSlab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PISlab_Delete(PerformanceIncentiveSlab oPISlab)
        {
            string sErrorMease = "";
            try
            {
                oPISlab = oPISlab.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oPISlab.ErrorMessage;
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion PerformanceIncentive_IUD

        #region Performance Incentive Activity
        [HttpPost]
        public JsonResult CheckMembers(PerformanceIncentive oPerformanceIncentive)
        {
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            PerformanceIncentive oPI = new PerformanceIncentive();
            try
            {
                string sSql = "IF(SELECT COUNT(*) FROM PerformanceIncentiveMember WHERE PIID=" + oPerformanceIncentive.PIID + ")>0"
                              + " BEGIN SELECT * FROM PerformanceIncentiveMember WHERE PIID=" + oPerformanceIncentive.PIID + " END ELSE BEGIN"
                              + " UPDATE PerformanceIncentive SET InactiveBy=" + ((User)(Session[SessionInfo.CurrentUser])).UserID
                              + ", InactiveDate='" + DateTime.Now.ToString("dd MMM yyyy") + "' WHERE PIID=" + oPerformanceIncentive.PIID + " END";
                oPerformanceIncentiveMembers = PerformanceIncentiveMember.Gets(sSql,((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oPerformanceIncentiveMembers.Count <= 0) {
                    oPI = PerformanceIncentive.Get(oPerformanceIncentive.PIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else { oPI.PerformanceIncentiveMembers = oPerformanceIncentiveMembers;}
            }
            catch (Exception ex)
            {
                oPI = new PerformanceIncentive();
                oPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PerformanceIncentive_Transfer(int PreviousPIID, int PresentPIID)
        {
            _oPerformanceIncentive = new PerformanceIncentive();
            try
            {
                _oPerformanceIncentive = PerformanceIncentive.PerformanceIncentive_Transfer(PreviousPIID, PresentPIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oPerformanceIncentive = new PerformanceIncentive();
                _oPerformanceIncentive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPerformanceIncentive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PerformanceIncentive_InActive(PerformanceIncentive oPerformanceIncentive)
        {
            _oPerformanceIncentive = new PerformanceIncentive();
            try
            {

                _oPerformanceIncentive = oPerformanceIncentive;
                _oPerformanceIncentive = PerformanceIncentive.InActive(_oPerformanceIncentive.PIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oPerformanceIncentive = new PerformanceIncentive();
                _oPerformanceIncentive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPerformanceIncentive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PerformanceIncentive Approve
        [HttpPost]
        public JsonResult PerformanceIncentive_Approve(PerformanceIncentive oPerformanceIncentive)
        {
            _oPerformanceIncentive = new PerformanceIncentive();
            try
            {

                _oPerformanceIncentive = oPerformanceIncentive;
                _oPerformanceIncentive = PerformanceIncentive.Approve(_oPerformanceIncentive.PIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oPerformanceIncentive = new PerformanceIncentive();
                _oPerformanceIncentive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPerformanceIncentive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PI Member
        public ActionResult View_PIMembers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            //string sSql = "SELECT *  FROM View_PerformanceIncentiveMember WHERE ApproveBy<=0 OR ApproveBy IS NULL";
            //oPerformanceIncentiveMembers = PerformanceIncentiveMember.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.PIs = PerformanceIncentive.Gets("SELECT * FROM View_PerformanceIncentive WHERE ApproveBy>0 AND InactiveBy=0", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return View(oPerformanceIncentiveMembers);
        }
        public ActionResult View_PIMember(string sid, string sMsg)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            int nPIMID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            if (nPIMID > 0)
            {
                oPerformanceIncentiveMember = PerformanceIncentiveMember.Get(nPIMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.PIs = PerformanceIncentive.Gets("SELECT * FROM View_PerformanceIncentive WHERE ApproveBy>0 AND InactiveBy=0", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return View(oPerformanceIncentiveMember);
        }

        [HttpPost]
        public JsonResult PerformanceIncentiveMember_IU(List<PerformanceIncentiveMember> oPIMembers)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            try
            {
                oPerformanceIncentiveMembers = oPerformanceIncentiveMember.IUD(oPIMembers, (int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            catch (Exception ex)
            {
                oPerformanceIncentiveMember = new PerformanceIncentiveMember();
                oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
                oPerformanceIncentiveMember.ErrorMessage = ex.Message;
                oPerformanceIncentiveMembers.Add(oPerformanceIncentiveMember);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPerformanceIncentiveMembers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult PerformanceIncentiveMember_Delete(List<PerformanceIncentiveMember> oPIMembers)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            string sErrorMease = "";
            try
            {
                oPerformanceIncentiveMembers = oPerformanceIncentiveMember.IUD(oPIMembers, (int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oPerformanceIncentiveMembers[0].ErrorMessage;
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
        public JsonResult PerformanceIncentiveMember_InActive(PerformanceIncentiveMember oPIMember)
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            try
            {

                oPerformanceIncentiveMember = oPIMember;
                oPerformanceIncentiveMember = PerformanceIncentiveMember.InActive(oPerformanceIncentiveMember.PIMemberID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oPerformanceIncentiveMember = new PerformanceIncentiveMember();
                oPerformanceIncentiveMember.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPerformanceIncentiveMember);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<PerformanceIncentiveMember> GetPIMemberFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    for (int i = 0; i < oRows.Count; i++)
                    {
                       
                        oPerformanceIncentiveMember = new PerformanceIncentiveMember();
                        oPerformanceIncentiveMember.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        oPerformanceIncentiveMember.PICode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][1]);
                        oPerformanceIncentiveMember.Rate = Convert.ToDouble(oRows[i][0] == DBNull.Value ? "" : oRows[i][2]);

                        oPerformanceIncentiveMembers.Add(oPerformanceIncentiveMember);
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oPerformanceIncentiveMembers;
        }

        [HttpPost]
        public ActionResult View_PIMembers(HttpPostedFileBase filePIMs)
        {
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            List<PerformanceIncentiveMember> oPIMs = new List<PerformanceIncentiveMember>();
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            try
            {
                if (filePIMs == null) { throw new Exception("File not Found"); }
                oPerformanceIncentiveMembers = this.GetPIMemberFromExcel(filePIMs);
                oPIMs = PerformanceIncentiveMember.UploadPIMXL(oPerformanceIncentiveMembers, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPIMs.Count >0)
                {
                    oPIMs[0].ErrorMessage = "Uploaded Successfully!";
                }

            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View(oPIMs);
            }
            return View(oPIMs);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }

        #region  Approve
        [HttpPost]
        public JsonResult PIM_Approve(string sPIMIDs, double nts)
        {
            List<PerformanceIncentiveMember> oPIMs = new List<PerformanceIncentiveMember>();
            PerformanceIncentiveMember oPIM = new PerformanceIncentiveMember();
            try
            {
                oPIMs = PerformanceIncentiveMember.Approve(sPIMIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oPIMs = new List<PerformanceIncentiveMember>();
                oPIM = new PerformanceIncentiveMember();
                oPIM.ErrorMessage=ex.Message;
                oPIMs.Add(oPIM);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPIMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult GetsPIMember(string sEmployeeIDs, int nPISchemeID,int nLoadRecords ,int nRowLength )
        {
            PerformanceIncentiveMember oPerformanceIncentiveMember = new PerformanceIncentiveMember();
            List<PerformanceIncentiveMember> oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
            
            try
            {
                string sSql = "SELECT * FROM View_PerformanceIncentiveMember WHERE PIMemberID<>0";
                sSql = "SELECT * FROM (SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY PIMemberID) Row,* FROM View_PerformanceIncentiveMember WHERE PIMemberID<>0";

                if(sEmployeeIDs!="")
                {
                    sSql += " AND EmployeeID IN(" + sEmployeeIDs + ")";
                }
                if(nPISchemeID>0)
                {
                      sSql+=" AND PIID ="+nPISchemeID;
                }

                sSql = sSql + ") aa WHERE Row >" + nRowLength + ") aaa ";
                oPerformanceIncentiveMembers = PerformanceIncentiveMember.Gets(sSql,((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oPerformanceIncentiveMember = new PerformanceIncentiveMember();
                oPerformanceIncentiveMembers = new List<PerformanceIncentiveMember>();
                oPerformanceIncentiveMember.ErrorMessage = ex.Message;
                oPerformanceIncentiveMembers.Add(oPerformanceIncentiveMember);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPerformanceIncentiveMembers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion PI Member

        #region PI Evaluation
        public ActionResult View_PIEvaluations(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations = new List<PerformanceIncentiveEvaluation>();
            string sSql = "SELECT *  FROM View_PerformanceIncentiveEvaluation WHERE ApproveBy<=0 OR ApproveBy IS NULL";
            oPerformanceIncentiveEvaluations = PerformanceIncentiveEvaluation.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(oPerformanceIncentiveEvaluations);
        }

        private List<PerformanceIncentiveEvaluation> GetPIEvaluationFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations = new List<PerformanceIncentiveEvaluation>();
            PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation = new PerformanceIncentiveEvaluation();
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    for (int i = 0; i < oRows.Count; i++)
                    {

                        oPerformanceIncentiveEvaluation = new PerformanceIncentiveEvaluation();
                        oPerformanceIncentiveEvaluation.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        oPerformanceIncentiveEvaluation.Year = Convert.ToInt32(oRows[i][0] == DBNull.Value ? "" : oRows[i][1]);
                        oPerformanceIncentiveEvaluation.MonthID = (EnumMonth)Convert.ToInt32((oRows[i][0] == DBNull.Value ? "" : oRows[i][2]));
                        oPerformanceIncentiveEvaluation.Point = Convert.ToDouble(oRows[i][0] == DBNull.Value ? "" : oRows[i][3]);

                        oPerformanceIncentiveEvaluations.Add(oPerformanceIncentiveEvaluation);
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oPerformanceIncentiveEvaluations;
        }

        [HttpPost]
        public ActionResult View_PIEvaluations(HttpPostedFileBase filePIEs)
        {
            List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations = new List<PerformanceIncentiveEvaluation>();
            List<PerformanceIncentiveEvaluation> oPIMs = new List<PerformanceIncentiveEvaluation>();
            PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation = new PerformanceIncentiveEvaluation();
            try
            {
                if (filePIEs == null) { throw new Exception("File not Found"); }
                oPerformanceIncentiveEvaluations = this.GetPIEvaluationFromExcel(filePIEs);
                oPIMs = PerformanceIncentiveEvaluation.UploadPIEXL(oPerformanceIncentiveEvaluations, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPIMs.Count > 0)
                {
                    oPIMs[0].ErrorMessage = "Uploaded Successfully!";
                }

            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View(oPIMs);
            }
            return View(oPIMs);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }

        #region  Approve
        [HttpPost]
        public JsonResult PIE_Approve(string sPIEIDs, double nts)
        {
            List<PerformanceIncentiveEvaluation> oPIMs = new List<PerformanceIncentiveEvaluation>();
            PerformanceIncentiveEvaluation oPIM = new PerformanceIncentiveEvaluation();
            try
            {
                oPIMs = PerformanceIncentiveEvaluation.Approve(sPIEIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oPIMs = new List<PerformanceIncentiveEvaluation>();
                oPIM = new PerformanceIncentiveEvaluation();
                oPIM.ErrorMessage = ex.Message;
                oPIMs.Add(oPIM);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPIMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region search
        [HttpPost]
        public JsonResult PIE_Search(string sEmployeeIDs, string sMonth, string sYear, int nLocationID, string sDepartmentIds, string sDesignationIds)
        {
            List<PerformanceIncentiveEvaluation> oPIEs = new List<PerformanceIncentiveEvaluation>();
            PerformanceIncentiveEvaluation oPIE = new PerformanceIncentiveEvaluation();
            try
            {
                string sSQL = "SELECT * FROM View_PerformanceIncentiveEvaluation WHERE PIEvaluationID<>0";
                if (!string.IsNullOrEmpty(sEmployeeIDs))
                {
                    sSQL += " AND PIMemberID IN(SELECT PIMemberID FROM PerformanceIncentiveMember WHERE EmployeeID IN(" + sEmployeeIDs + "))";
                }
                if (!string.IsNullOrEmpty(sMonth) && sMonth != "0")
                {
                    sSQL += "AND MonthID=" + sMonth;
                }
                if (!string.IsNullOrEmpty(sYear) && sYear != "0")
                {
                    sSQL += "AND [Year]=" + sYear;
                }
                if(nLocationID>0)
                {
                    sSQL += "AND PIMemberID IN(SELECT PIMemberID FROM PerformanceIncentiveMember WHERE EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DRPID IN(SELECT DRPID FROM DepartmentRequirementPolicy WHERE LocationID="+nLocationID+" )))";
                }
                if (!string.IsNullOrEmpty(sDepartmentIds))
                {
                    sSQL += "AND PIMemberID IN(SELECT PIMemberID FROM PerformanceIncentiveMember WHERE EmployeeID IN(SELECT EmployeeID FROM View_Employee WHERE DepartmentID IN( "+sDepartmentIds+")))";
                }
                if (!string.IsNullOrEmpty(sDesignationIds))
                {
                    sSQL += "AND PIMemberID IN(SELECT PIMemberID FROM PerformanceIncentiveMember WHERE EmployeeID IN(SELECT EmployeeID FROM View_Employee WHERE DesignationID IN(" + sDesignationIds + ")))";
                }

                oPIEs = PerformanceIncentiveEvaluation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oPIEs.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oPIEs = new List<PerformanceIncentiveEvaluation>();
                oPIE = new PerformanceIncentiveEvaluation();
                oPIE.ErrorMessage = ex.Message;
                oPIEs.Add(oPIE);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPIEs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion search
        #endregion PI Evaluation
    }
}
