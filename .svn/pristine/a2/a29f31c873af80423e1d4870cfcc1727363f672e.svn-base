using System;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class DisciplinaryActionController : Controller
    {
        #region Declaration
        DisciplinaryAction _oDisciplinaryAction;
        private List<DisciplinaryAction> _oDisciplinaryActions;
        #endregion

        public ActionResult ViewDisciplinaryActions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FinancialAdjustment).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            string sSQL = "";
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            sSQL = "select * from View_DisciplinaryAction WHERE ApproveBy<=0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }
            _oDisciplinaryActions = DisciplinaryAction.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            sSQL = "";
            sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSQL = sSQL + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
            return View(_oDisciplinaryActions);
        }

        public ActionResult ViewDisciplinaryAction(string sid, string sMsg)
        {
            int nDAID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oDisciplinaryAction = new DisciplinaryAction();

            if (nDAID > 0)
            {
                _oDisciplinaryAction = _oDisciplinaryAction.Get(nDAID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            if (sMsg != "N/A")
            {
                _oDisciplinaryAction.ErrorMessage = sMsg;
            }

            ViewBag.SalaryHeads_Addition = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType IN(2)", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.SalaryHeads_Deduction = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType IN(3)", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SalaryHeadTypes = Enum.GetValues(typeof(EnumSalaryHeadType)).Cast<EnumSalaryHeadType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x=>x.Value !=0.ToString() && x.Value !=1.ToString()).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSQL = "";
            sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSQL = sSQL + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
            return View(_oDisciplinaryAction);
        }

        [HttpPost]
        public JsonResult DisciplinaryAction_IU(DisciplinaryAction oDisciplinaryAction)
        {
            _oDisciplinaryAction = new DisciplinaryAction();
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            try
            {
                _oDisciplinaryAction = oDisciplinaryAction;
                _oDisciplinaryAction.PaymentCycle = (EnumPaymentCycle)oDisciplinaryAction.PaymentCycleInt;
                if (_oDisciplinaryAction.DisciplinaryActionID>0)
                {
                    _oDisciplinaryActions = _oDisciplinaryAction.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oDisciplinaryActions = _oDisciplinaryAction.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
                _oDisciplinaryActions.Add(_oDisciplinaryAction);
            }
            _oDisciplinaryAction.DisciplinaryActions = new List<DisciplinaryAction>();
            _oDisciplinaryAction.DisciplinaryActions.AddRange(_oDisciplinaryActions);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryAction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult DisciplinaryAction_Update(DisciplinaryAction oDisciplinaryAction)
        //{
        //    _oDisciplinaryAction = new DisciplinaryAction();
        //    try
        //    {
        //        _oDisciplinaryAction = oDisciplinaryAction;
        //        _oDisciplinaryAction = _oDisciplinaryAction.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oDisciplinaryAction = new DisciplinaryAction();
        //        _oDisciplinaryAction.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oDisciplinaryAction);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult DisciplinaryAction_Delete( string sDisciplinaryActionIDs)
        {
            _oDisciplinaryAction = new DisciplinaryAction();
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            try
            {
                _oDisciplinaryAction.sIDs = sDisciplinaryActionIDs;
                _oDisciplinaryActions = _oDisciplinaryAction.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if(_oDisciplinaryActions.Count>0 && _oDisciplinaryActions[0].ErrorMessage !="")
                {
                    _oDisciplinaryAction.ErrorMessage = _oDisciplinaryActions[0].ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryAction.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region DA Approve Unapprove
        [HttpPost]
        public JsonResult DisciplinaryAction_Approve(DisciplinaryAction oDisciplinaryAction)
        {
            _oDisciplinaryAction = new DisciplinaryAction();
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            try
            {
                _oDisciplinaryActions = oDisciplinaryAction.IUD((int)EnumDBOperation.Approval, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if(_oDisciplinaryActions.Count>0 && _oDisciplinaryActions[0].ErrorMessage!="")
                {
                    _oDisciplinaryAction.ErrorMessage = _oDisciplinaryActions[0].ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
                _oDisciplinaryActions.Add(_oDisciplinaryAction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryActions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DisciplinaryAction_UnApprove(DisciplinaryAction oDisciplinaryAction)
        {
            _oDisciplinaryAction = new DisciplinaryAction();
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            try
            {
                _oDisciplinaryActions = oDisciplinaryAction.IUD((int)EnumDBOperation.Revise, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDisciplinaryActions.Count > 0 && _oDisciplinaryActions[0].ErrorMessage != "")
                {
                    _oDisciplinaryAction.ErrorMessage = _oDisciplinaryActions[0].ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
                _oDisciplinaryActions.Add(_oDisciplinaryAction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryActions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion DA Approve Unapprove

        [HttpPost]
        public JsonResult Gets(DisciplinaryAction oDisciplinaryAction)
       {
            List<DisciplinaryAction> oDisciplinaryActions = new List<DisciplinaryAction>();
            try
            {
                string sSql = "SELECT * from View_DisciplinaryAction where EmployeeID IN (" + oDisciplinaryAction.sIDs + ")";
                oDisciplinaryActions = DisciplinaryAction.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDisciplinaryActions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
       }

        #region Advance Payment
        public ActionResult View_AdvancePayments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            return View(_oDisciplinaryActions);
        }
        public ActionResult View_AdvancePayment()
        {
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            return PartialView(_oDisciplinaryActions);
        }
        [HttpPost]
        public JsonResult DisciplinaryAction_IUD_List(List<DisciplinaryAction> oDisciplinaryActions)
        {
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            _oDisciplinaryAction = new DisciplinaryAction();
            try
            {
                _oDisciplinaryActions = oDisciplinaryActions;
                _oDisciplinaryActions = _oDisciplinaryAction.IUD_List(oDisciplinaryActions, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
                _oDisciplinaryActions.Add(_oDisciplinaryAction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryActions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DisciplinaryAction_Delete_List(string sIds)
        {
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            _oDisciplinaryAction = new DisciplinaryAction();
            try
            {
                string sSql = "DELETE FROM DisciplinaryAction WHERE DisciplinaryActionID IN(" + sIds + ")";
                _oDisciplinaryActions = DisciplinaryAction.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oDisciplinaryAction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryAction.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DisciplinaryAction_Approve_List(string sIds)
        {
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            try
            {
                string sSql = "UPDATE DisciplinaryAction SET ApproveBy = 1 , ApproveByDate = '" + DateTime.Now + "' WHERE DisciplinaryActionID IN(" + sIds + ")"
                    + " SELECT * FROM View_DisciplinaryAction WHERE DisciplinaryActionID IN(" + sIds + ")";
                _oDisciplinaryActions = DisciplinaryAction.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDisciplinaryActions.Count <= 0)
                {
                    throw new Exception("Data not found!");
                }
            }
            catch (Exception ex)
            {
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
                _oDisciplinaryActions.Add(_oDisciplinaryAction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryActions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Search Disciplinary Action Process
        [HttpPost]
        public JsonResult SearchWithPagination(string sParams, double ts)
        {
            try
            {


                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryActions = DisciplinaryAction.GetsForDAProcess(sParams, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDisciplinaryActions.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
                _oDisciplinaryActions.Add(_oDisciplinaryAction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryActions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion Advance Payment

        #region Print
        public ActionResult PrintDisciplinaryAction(string sParams, double ts)
        {

            _oDisciplinaryAction = new DisciplinaryAction();


            string sEmployeeIDs = sParams.Split(',')[0];
            string sDepartmentIDs = sParams.Split(',')[1];
            string sDesignationIDs = sParams.Split(',')[2];
            string sStartDate_Exe = sParams.Split(',')[3];
            string sEndDate_Exe = sParams.Split(',')[4];


            string sSql = "SELECT * FROM View_DisciplinaryAction WHERE EmployeeID<>0"
             + " AND ExecutedOn BETWEEN '" + sStartDate_Exe + "' AND '" + sEndDate_Exe + "'";
            if (sEmployeeIDs != "")
            {
                sSql += " AND EmployeeID IN(" + sEmployeeIDs + ")";
            }
            if (sDepartmentIDs != "")
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIDs + ")";
            }
            if (sDesignationIDs != "")
            {
                sSql += " AND DesignationID IN(" + sDesignationIDs + ")";
            }

            sSql += " ORDER BY EmployeeCode";
            _oDisciplinaryAction.DisciplinaryActions = DisciplinaryAction.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //_oDisciplinaryAction.ErrorMessage = sDates;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oDisciplinaryAction.Company = oCompanys.First();
            _oDisciplinaryAction.Company.CompanyLogo = GetCompanyLogo(_oDisciplinaryAction.Company);

            rptDisciplinaryAction oReport = new rptDisciplinaryAction();
            byte[] abytes = oReport.PrepareReport(_oDisciplinaryAction);
            return File(abytes, "application/pdf");
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
        #endregion Print

        #region Other Payment
        public ActionResult View_OtherPayments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDisciplinaryActions = new List<DisciplinaryAction>();
            return View(_oDisciplinaryActions);
        }
        #endregion

        #region Search
        [HttpPost]
        public JsonResult SearchDAWithPasignation(string sEmployeeIDs, string dtDateFrom, string dtDateTo, int nApproved, int nLoadRecords, int nRowLength, double ts)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM (SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeCode) Row,* FROM View_DisciplinaryAction WHERE ExecutedOn BETWEEN '" + dtDateFrom + "' AND '" + dtDateTo + "'";

                if (sEmployeeIDs != "")
                {
                    sSql = sSql + " AND EmployeeID IN(" + sEmployeeIDs + ")";
                }
                if (nApproved==1)
                {
                    sSql = sSql + " AND ApproveBy>0";
                }
                else  if (nApproved==0)
                {
                    sSql = sSql + " AND ApproveBy<=0";
                }
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
                }
                sSql = sSql + ") aa WHERE Row >" + nRowLength + ") aaa ORDER BY EmployeeCode";
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryActions = DisciplinaryAction.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oDisciplinaryActions.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oDisciplinaryActions = new List<DisciplinaryAction>();
                _oDisciplinaryAction = new DisciplinaryAction();
                _oDisciplinaryAction.ErrorMessage = ex.Message;
                _oDisciplinaryActions.Add(_oDisciplinaryAction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDisciplinaryActions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Search

        #region Import & Export
        private List<DisciplinaryAction> GetDisciplinaryActionFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<DisciplinaryAction> oDAXLs = new List<DisciplinaryAction>();
            DisciplinaryAction oDAXL = new DisciplinaryAction();
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
                    int nCol = 0;
                    nCol = Convert.ToInt16(oRows[0][0] == DBNull.Value ? 0 : oRows[0][0]);
                    if (nCol <= 0) { Convert.ToInt16(oRows[0][1] == DBNull.Value ? 0 : oRows[0][1]); }
                    if (nCol <= 0) { throw new Exception("Please enter total no of salary head!");}
                    nCol = nCol + 2;
                    string[] SHead = new string[nCol];
                    string SH = "";
                    string EmployeeCode = "";
                    DateTime ExecutedOn = DateTime.Now;
                    int nCompCount = nCol;

                    int nCompSH = Convert.ToInt16(((oRows[0][1] == DBNull.Value) || (oRows[0][1] == "")) ? 0 : oRows[0][1]);

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        for (int j = 0; j <= nCol; j++)
                        {
                            if (i == 0 && j <= nCol-3)
                            {
                                SH += Convert.ToString(oRows[0][j+2] == DBNull.Value ? "" : oRows[0][j+2])+",";
                            }
                            else if(i!=0)
                            {
                                if (i == 1 && j==0)
                                {
                                    SH = SH.Remove(SH.Length - 1);
                                    SHead = SH.Split(',');
                                }

                                oDAXL = new DisciplinaryAction();

                                if(j==0)
                                {
                                    EmployeeCode = Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]);
                                    ExecutedOn = Convert.ToDateTime(oRows[i][j+1] == DBNull.Value ? "" : oRows[i][j+1]);
                                }
                                if (j <= nCol - 3)
                                {
                                    oDAXL.EmployeeCode = EmployeeCode;
                                    oDAXL.ExecutedOn = ExecutedOn;
                                    if (oDAXL.EmployeeCode != "")
                                    {
                                        oDAXL.SalaryHeadName = SHead[j];
                                        oDAXL.Amount = Convert.ToDouble(oRows[i][j + 2] == DBNull.Value ? 0 : oRows[i][j + 2]);
                                        oDAXL.CompAmount = Convert.ToDouble(oRows[i][j + 2] == DBNull.Value ? 0 : oRows[i][j + 2]);
                                        if (nCompSH == 1)
                                        {
                                            oDAXL.CompAmount = Convert.ToDouble(oRows[i][nCompCount] == DBNull.Value ? 0 : oRows[i][nCompCount]);
                                            nCompCount++;
                                        }

                                        oDAXLs.Add(oDAXL);
                                    }
                                }
                               
                            }
                        }
                        nCompCount = nCol;
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
            return oDAXLs;
        }

        [HttpPost]
        public ActionResult ViewDisciplinaryActions(HttpPostedFileBase fileDAs)
        {
            List<DisciplinaryAction> oDAXLs = new List<DisciplinaryAction>();
            DisciplinaryAction oDAXL = new DisciplinaryAction();
            List<DisciplinaryAction> oDISAs = new List<DisciplinaryAction>();

            try
            {
                if (fileDAs == null) { throw new Exception("File not Found"); }
                oDAXLs = new List<DisciplinaryAction>();
      
                oDISAs = new List<DisciplinaryAction>();
                oDAXLs = this.GetDisciplinaryActionFromExcel(fileDAs);

                if (oDAXLs.Count() <= 0)
                    throw new Exception("Nothing found to Upload");

                oDISAs = DisciplinaryAction.UploadXL(oDAXLs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDISAs.Count > 0)
                {
                    GenerateErrorExcel(oDISAs);
                    //oDISAs[0].ErrorMessage = "Uploaded Successfully!";
                }
                else
                {
                    int nCount = 0;
                    int nFromExcel = oDAXLs.Count();
                    int nUnsuccessful = oDISAs.Count();
                    nCount = nFromExcel - nUnsuccessful;

                    DisciplinaryAction oDISA = new DisciplinaryAction();
                    oDISA.ErrorMessage = "Uploaded Successfully! " + nCount + " record uploaded.";
                    oDISA.bUpload = true;
                    oDISAs.Add(oDISA);
                }
                ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View(oDISAs);
            }
            return View(oDISAs);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }
        public void GenerateErrorExcel(List<DisciplinaryAction> oDISAs)
        { 
            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 2;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Error List");
                sheet.Name = "Error List";

                int n = 1;
                sheet.Column(n++).Width = 18;
                sheet.Column(n++).Width = 100;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;

                foreach (DisciplinaryAction oItem in oDISAs)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }


        public void PrintDAListInXL(string sParams, double ts)
        {
            List<DisciplinaryAction> DAs = new List<DisciplinaryAction>();
            List<DisciplinaryAction> DActions = new List<DisciplinaryAction>();

            string sDateFrom=sParams.Split('~')[0];
            string sDateTo = sParams.Split('~')[1];
            string sEmployeeIDs = sParams.Split('~')[2];
            int nApproved = Convert.ToInt32(sParams.Split('~')[3]);

            string sSql = "";
            sSql = "SELECT * FROM View_DisciplinaryAction WHERE ExecutedOn BETWEEN '" + sDateFrom + "' AND '" + sDateTo + "'";

            if (sEmployeeIDs != "")
            {
                sSql = sSql + " AND EmployeeID IN(" + sEmployeeIDs + ")";
            }
            if (nApproved == 1)
            {
                sSql = sSql + " AND ApproveBy>0";
            }
            else if (nApproved == 0)
            {
                sSql = sSql + " AND ApproveBy<=0";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }
            sSql = sSql + " ORDER BY EmployeeCode";


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            DAs = DisciplinaryAction.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            DActions = DAs.GroupBy(x => x.SalaryHeadID).Select(grp => grp.First()).ToList();
            DActions=DActions.OrderBy(x=>x.SalaryHeadID).ToList();

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Financial Adjustment List");
                sheet.Name = "Financial Adjustment List";

                 nMaxColumn = 4+(DActions.Count);
                int xTotalCol = 4+(DActions.Count * 2);

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeName
                sheet.Column(4).Width = 15; //Code
                sheet.Column(5).Width = 15; //ExecutedOnInString

                for(int i=6; i<nMaxColumn; i++)
                {
                    sheet.Column(i).Width = 10; //SH
                }
                for (int i = nMaxColumn; i <= xTotalCol; i++)
                {
                    sheet.Column(i).Width = 10; //SHComp
                }

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Financial Adjustment List"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Effect Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                foreach (DisciplinaryAction oItem in DActions)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                foreach (DisciplinaryAction oItem in DActions)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SalaryHeadName + "(Comp)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
                #endregion

                #region Table Body
                int nSL = 0;
                List<DisciplinaryAction> DActionLists = new List<DisciplinaryAction>();
                DAs.ForEach(x => DActionLists.Add(x));
          
                while (DAs.Count>0)
                {
                    List<DisciplinaryAction> TempDActions = new List<DisciplinaryAction>();
                    TempDActions = DAs.Where(x => x.EmployeeID == DAs[0].EmployeeID).OrderBy(x => x.SalaryHeadID).ToList();
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = TempDActions.Count > 0 ? TempDActions[0].EmployeeName : ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = TempDActions.Count > 0 ? TempDActions[0].EmployeeCode : ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = TempDActions.Count > 0 ? TempDActions[0].ExecutedOnInString : ""; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    foreach (DisciplinaryAction oItem in DActions)
                    {
                        double nAmount = TempDActions.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Sum(x => x.Amount);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nAmount, 2); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    foreach (DisciplinaryAction oItem in DActions)
                    {
                        double nAmount = TempDActions.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Sum(x => x.CompAmount);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nAmount, 2); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    rowIndex++;
                    DAs.RemoveAll(x => x.EmployeeID == TempDActions[0].EmployeeID);
                }
                #endregion

                #region Total

                sheet.Cells[rowIndex, 2, rowIndex, 5].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                colIndex = 6;
                foreach (DisciplinaryAction oItem in DActions)
                {
                    double nTotalAmount = DActionLists.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Sum(x => x.Amount);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nTotalAmount, 2); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                foreach (DisciplinaryAction oItem in DActions)
                {
                    double nTotalAmount = DActionLists.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Sum(x => x.CompAmount);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nTotalAmount, 2); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                //signature

                rowIndex = rowIndex + 8;
                colIndex = 1;

                int Colspan = 0;

                Colspan = (nMaxColumn - nMaxColumn % 4) / 4;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex=colIndex + Colspan]; cell.Merge = true; cell.Value = "_________________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_______________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "__________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_____________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                rowIndex = rowIndex + 1;
                colIndex = 0;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Checked By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Reviewed By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DAList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        private string GetAmountInStr(double amount)
        {
            amount = Math.Round(amount);
            return Global.MillionFormat(amount);
        }
        #endregion Import & Export



        public void DownloadFormat()
        {
            int nRowIndex = 1, nStartCol = 1, nEndCol = 2;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Format Downlaod");
                sheet.Name = "Format Downlaod";

                int n = 1;
                sheet.Column(n++).Width = 18;//code
                sheet.Column(n++).Width = 18;//EffectDate
                sheet.Column(n++).Width = 18;//Head
                sheet.Column(n++).Width = 18;//Head
                sheet.Column(n++).Width = 18;//Head
                sheet.Column(n++).Width = 18;//Head
                sheet.Column(n++).Width = 170;//Head
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EffectDate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "HeadName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                colIndex = 1;
                rowIndex++;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "2"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "*(0)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mobile"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Canteen"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "if put 0 or blank then compliance amount will be same as actual amount"; cell.Style.Font.Bold = true; cell.Merge = true; cell.Style.WrapText = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                colIndex = 1;
                rowIndex++;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "02997"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "30-Nov-2017"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1500"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                rowIndex++;
                rowIndex++;



                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EffectDate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "HeadName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                colIndex = 1;
                rowIndex++;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "2"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "*(1)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mobile"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Canteen"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mobile"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Canteen"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "if put 1 then you need to put compliance amount of the salary heads along with the actual amount"; cell.Style.Font.Bold = true; cell.Merge = true; cell.Style.WrapText = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                colIndex = 1;
                rowIndex++;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "02998"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "30-Nov-2017"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1500"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1500"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "800"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

    }
    
}
