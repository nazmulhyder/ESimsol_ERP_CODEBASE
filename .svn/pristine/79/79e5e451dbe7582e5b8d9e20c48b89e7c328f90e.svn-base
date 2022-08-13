using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{

    public class PartsRequisitionController : Controller
    {

        #region Declartion
        PartsRequisition _oPartsRequisition = new PartsRequisition();
        List<PartsRequisition> _oPartsRequisitions = new List<PartsRequisition>();
        PartsRequisitionDetail _oPartsRequisitionDetail = new PartsRequisitionDetail();
        List<PartsRequisitionDetail> _oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
        List<PartsRequisitionRegister> _oPartsRequisitionRegisters = new List<PartsRequisitionRegister>();
        PartsRequisitionRegister _oPartsRequisitionRegister = new PartsRequisitionRegister();
        string _sDateRange = "";
        #endregion

        #region Collection Page
        public ActionResult ViewPartsRequisitions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PartsRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            _oPartsRequisitions = new List<PartsRequisition>();
            string sSQL = "SELECT * FROM View_PartsRequisition AS HH WHERE  ISNULL(HH.ApprovedBy,0)=0 ";
            if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND HH.BUID = " + buid ;
            }
            sSQL += " ORDER BY PartsRequisitionID ASC";
            _oPartsRequisitions = PartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            #region Requisition User
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequisitionBy FROM PartsRequisition AS MM WHERE MM.BUID =" + buid.ToString() + " AND ISNULL(MM.RequisitionBy,0)!=0) ORDER BY HH.UserName";
            }
            else
            {
                sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequisitionBy FROM PartsRequisition AS MM WHERE ISNULL(MM.RequisitionBy,0)!=0) ORDER BY HH.UserName";
            }
            List<User> oRequisitionUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oRequisitionUser = new ESimSol.BusinessObjects.User();
            oRequisitionUser.UserID = 0; oRequisitionUser.UserName = "--Select Requisition User--";
            oRequisitionUsers.Add(oRequisitionUser);
            oRequisitionUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.PartsRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.PRTypes = EnumObject.jGets(typeof(EnumPRequisutionType));
            ViewBag.Stores = oWorkingUnits;
            ViewBag.RequisitionUsers = oRequisitionUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));

            return View(_oPartsRequisitions);
        }

        #endregion

        #region Add, Edit, Delete, Search
        public ActionResult ViewPartsRequisition(int id, int buid)
        {
            _oPartsRequisition = new PartsRequisition();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            if (id > 0)
            {
                _oPartsRequisition = _oPartsRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPartsRequisition.PartsRequisitionDetails = PartsRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oPartsRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oPartsRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.PartsRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            ViewBag.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, (int)Session[SessionInfo.currentUserID]);
            
            ViewBag.PRTypes = EnumObject.jGets(typeof(EnumPRequisutionType));
            ViewBag.ChargeTypes = EnumObject.jGets(typeof(EnumServiceILaborChargeType));
            return View(_oPartsRequisition);
        }

        public ActionResult ViewPartsRequisitionRevise(int id, int buid)
        {
            _oPartsRequisition = new PartsRequisition();
            if (id > 0)
            {
                _oPartsRequisition = _oPartsRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPartsRequisition.PartsRequisitionDetails = PartsRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oPartsRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oPartsRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.PartsRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            ViewBag.CRTypes = EnumObject.jGets(typeof(EnumPartsType));
            ViewBag.Shifts = EnumObject.jGets(typeof(EnumShift));
            return View(_oPartsRequisition);
        }
        public ActionResult ViewPartsRequisitionDisburse(int id, int buid)
        {
            _oPartsRequisition = new PartsRequisition();
            if (id > 0)
            {
                _oPartsRequisition = _oPartsRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPartsRequisition.PartsRequisitionDetails = PartsRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oPartsRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oPartsRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.PartsRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            ViewBag.CRTypes = EnumObject.jGets(typeof(EnumPartsType));
            return View(_oPartsRequisition);
        }
        public ActionResult AdvSearchPartsRequisition()
        {
            return PartialView();
        }
        #region HTTP Save
        [HttpPost]
        public JsonResult Save(PartsRequisition oPartsRequisition)
        {

            _oPartsRequisition = new PartsRequisition();
            _oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
            try
            {
                _oPartsRequisition = oPartsRequisition;
                _oPartsRequisition = _oPartsRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPartsRequisition = new PartsRequisition();
                _oPartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPartsRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Delivery
        [HttpPost]
        public JsonResult Delivery(PartsRequisition oPartsRequisition)
        {

            _oPartsRequisition = new PartsRequisition();
            _oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
            try
            {
                _oPartsRequisition = oPartsRequisition;
                _oPartsRequisition = _oPartsRequisition.Delivery((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPartsRequisition = new PartsRequisition();
                _oPartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPartsRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET Delete
        [HttpPost]
        public JsonResult Delete(PartsRequisition oPartsRequisition)
        {
            string smessage = "";
            try
            {
                smessage = oPartsRequisition.Delete(oPartsRequisition.PartsRequisitionID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region HTTP AcceptPRRevise
        [HttpPost]
        public JsonResult AcceptPRRevise(PartsRequisition oPartsRequisition)
        {
            _oPartsRequisition = new PartsRequisition();
            List<PartsRequisitionDetail> oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
            try
            {
                _oPartsRequisition = oPartsRequisition;
                _oPartsRequisition = _oPartsRequisition.AcceptPartsRequisitionRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPartsRequisition = new PartsRequisition();
                _oPartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPartsRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Print and Preview
        public ActionResult PartsRequisitionPrintList(string sIDs, double ts)
        {
            _oPartsRequisition = new PartsRequisition();
            string sSQL = "";
            sSQL = "SELECT * FROM  View_PartsRequisition WHERE PartsRequisitionID IN ( " + sIDs + " )";
            _oPartsRequisition.PartsRequisitions = PartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPartsRequisition.PartsRequisitions[0].BUID, (int)Session[SessionInfo.currentUserID]);
            _oPartsRequisition.Company = oCompany;
            _oPartsRequisition.BusinessUnit = oBusinessUnit;

            if (_oPartsRequisition.PartsRequisitions.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();
                SelectedField oSelectedField = new SelectedField("RequisitionNo", "Req. No", 30, SelectedField.FieldType.Data,SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("IssueDateSt", "Date", 45, SelectedField.FieldType.Data, SelectedField.Alginment.CENTER); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ServiceOrderNo", "Order No", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("VehicleRegNo", "Reg. No", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ChassisNo", "Chassis No/ VIN", 60, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("PRStatusSt", "Status", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("RequisitionByName", "Requisition By", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);

                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 0;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReport(_oPartsRequisition.PartsRequisitions.Cast<object>().ToList(), oBusinessUnit, oCompany, "Parts Requisition", oSelectedFields);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PartsRequisitionPreview(int id)
        {
            _oPartsRequisition = new PartsRequisition();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            Contractor oContractor = new Contractor();
            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            Company oCompany = new Company();
            _oPartsRequisition = _oPartsRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oPartsRequisition.PartsRequisitionDetails = PartsRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oPartsRequisition.Company = oCompany;
            
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPartsRequisition.BUID, (int)Session[SessionInfo.currentUserID]);
            _oPartsRequisition.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oPartsRequisition.BusinessUnit = oBusinessUnit;

            rptPartsRequisition oReport = new rptPartsRequisition();
            byte[] abytes = oReport.PrepareReport(_oPartsRequisition);
            return File(abytes, "application/pdf");
        }

        public ActionResult PartsRequisitionLogPreview(int id)
        {
            _oPartsRequisition = new PartsRequisition();
            Contractor oContractor = new Contractor();
            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            Company oCompany = new Company();
            _oPartsRequisition = _oPartsRequisition.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oPartsRequisition.PartsRequisitionDetails = PartsRequisitionDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oPartsRequisition.Company = oCompany;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPartsRequisition.BUID, (int)Session[SessionInfo.currentUserID]);
            _oPartsRequisition.BusinessUnit = oBusinessUnit;

            //rptPartsRequisition oReport = new rptPartsRequisition();
            //byte[] abytes = oReport.PrepareReport(_oPartsRequisition);
            //return File(abytes, "application/pdf");
            return null;
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

        #endregion Print        
    
        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(PartsRequisition oPartsRequisition)
        {
            _oPartsRequisition = new PartsRequisition();
            _oPartsRequisition = oPartsRequisition;
            try
            {
                if (oPartsRequisition.ActionTypeExtra == "RequestForApproval")
                {
                    _oPartsRequisition.PRActionType = EnumCRActionType.RequestForApproval;
                    _oPartsRequisition.PRActionTypeInt = (int)EnumCRActionType.RequestForApproval;
                    _oPartsRequisition.PRStatus = EnumCRStatus.RequestForApproval;
                    _oPartsRequisition.PRStatusInt = (int)EnumCRStatus.RequestForApproval;

                }
                else if (oPartsRequisition.ActionTypeExtra == "UndoRequest")
                {
                    _oPartsRequisition.PRActionType = EnumCRActionType.UndoRequest;
                    _oPartsRequisition.PRActionTypeInt = (int)EnumCRActionType.UndoRequest;
                }
                else if (oPartsRequisition.ActionTypeExtra == "Approve")
                {
                    _oPartsRequisition.PRActionType = EnumCRActionType.Approve;
                    _oPartsRequisition.PRActionTypeInt = (int)EnumCRActionType.Approve;
                    _oPartsRequisition.PRStatus = EnumCRStatus.Initiallize;
                    _oPartsRequisition.PRStatusInt = (int)EnumCRStatus.Initiallize;
                }
                else if (oPartsRequisition.ActionTypeExtra == "UndoApprove")
                {
                    _oPartsRequisition.PRActionType = EnumCRActionType.UndoApprove;
                    _oPartsRequisition.PRActionTypeInt = (int)EnumCRActionType.UndoApprove;
                    _oPartsRequisition.PRStatus = EnumCRStatus.Approve;
                    _oPartsRequisition.PRStatusInt = (int)EnumCRStatus.Approve;
                }
                else if (oPartsRequisition.ActionTypeExtra == "RequestForRevise")
                {
                    _oPartsRequisition.PRActionType = EnumCRActionType.Request_revise;
                    _oPartsRequisition.PRActionTypeInt = (int)EnumCRActionType.Request_revise;
                    _oPartsRequisition.PRStatus = EnumCRStatus.Approve;
                    _oPartsRequisition.PRStatusInt = (int)EnumCRStatus.Approve;
                }
                else if (oPartsRequisition.ActionTypeExtra == "Disburse")
                {
                    _oPartsRequisition.PRActionType = EnumCRActionType.StockOut;
                    _oPartsRequisition.PRActionTypeInt = (int)EnumCRActionType.StockOut;
                    _oPartsRequisition.PRStatus = EnumCRStatus.StockOut;
                    _oPartsRequisition.PRStatusInt = (int)EnumCRStatus.StockOut;
                }
                _oPartsRequisition.Remarks = oPartsRequisition.Remarks;
                _oPartsRequisition = oPartsRequisition.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPartsRequisition = new PartsRequisition();
                _oPartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPartsRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
        
        #region Advance Search

        #region Http Get For Search
        [HttpPost]
        public JsonResult AdvanceSearch(PartsRequisition oPartsRequisition)
        {
            List<PartsRequisition> oPartsRequisitions = new List<PartsRequisition>();
            try
            {
                string sSQL = GetSQL(oPartsRequisition.Remarks, oPartsRequisition.BUID);
                oPartsRequisitions = PartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPartsRequisition = new PartsRequisition();
                _oPartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPartsRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            int nCount = 0;
            string sRefNo = Convert.ToString(sSearchingData.Split('~')[nCount++]);
            EnumCompareOperator eIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[nCount++]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[nCount++]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[nCount++]);
            EnumCompareOperator eCRAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[nCount++]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[nCount++]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[nCount++]);
            int nRequsitionBy = Convert.ToInt32(sSearchingData.Split('~')[nCount++]);
            string sStores = Convert.ToString(sSearchingData.Split('~')[nCount++]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[nCount++]);
            string sPRTypes = Convert.ToString(sSearchingData.Split('~')[nCount++]);
            string sRegNo = Convert.ToString(sSearchingData.Split('~')[nCount++]);
            string sBinNo = Convert.ToString(sSearchingData.Split('~')[nCount++]);
            string sCustomerName = Convert.ToString(sSearchingData.Split('~')[nCount++]);


            string sReturn1 = "SELECT * FROM View_PartsRequisition";
            string sReturn = "";

            #region BUID
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region RefNo
            if (sRefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionNo LIKE '%" + sRefNo + "%'";
            }
            #endregion
            
            #region IssueDate
            if (eIssueDate != EnumCompareOperator.None)
            {
                if (eIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Amount
            if (eCRAmount != EnumCompareOperator.None)
            {
                if (eCRAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
            }
            #endregion

            #region RequisitionBy
            if (nRequsitionBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(RequisitionBy,0) = " + nRequsitionBy.ToString();
            }
            #endregion

            #region PartsUnits
            if (sStores != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StoreID IN (" + sStores + ")";
            }
            #endregion

            #region Style
            if (sPRTypes != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PRType IN (" + sPRTypes + ")";
            }
            #endregion

            #region Products
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PartsRequisitionID IN (SELECT TT.PartsRequisitionID FROM PartsRequisitionDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            #region RegustrationNo
            if (sRegNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VehicleRegNo LIKE '%" + sRegNo + "%'";
            }
            #endregion

            #region Bin No
            if (sBinNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChassisNo LIKE '%" + sBinNo + "%'";
            }
            #endregion

            #region Customer Name
            if (sCustomerName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CustomerName LIKE '%" + sCustomerName + "%'";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        [HttpPost]
        public JsonResult GetsByRefNo(PartsRequisition oPartsRequisition)
        {
            List<PartsRequisition> oPartsRequisitions = new List<PartsRequisition>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PartsRequisition AS HH WHERE  (ISNULL(HH.ServiceOrderNo,'')+ISNULL(HH.VechileRegNo,'')+ISNULL(HH.RequisitionNo,'')) LIKE '%" + oPartsRequisition.RequisitionNo + "%' ";
                #region BUID
                if (oPartsRequisition.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL = sSQL + " BUID = " + oPartsRequisition.BUID.ToString();
                }
                sSQL = sSQL + " ORDER BY PartsRequisitionID ASC";
                #endregion
                oPartsRequisitions = PartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPartsRequisitions = new List<PartsRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPartsRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitingSearch(PartsRequisition oPartsRequisition)
        {
            List<PartsRequisition> oPartsRequisitions = new List<PartsRequisition>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PartsRequisition AS HH WHERE ISNULL(HH.CRStatus,0) = 1 ";
                #region BUID
                if (oPartsRequisition.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL = sSQL + " BUID = " + oPartsRequisition.BUID.ToString();
                }
                sSQL = sSQL + " ORDER BY PartsRequisitionID ASC";
                #endregion
                oPartsRequisitions = PartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPartsRequisitions = new List<PartsRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPartsRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReviseHistory(PartsRequisition oPartsRequisition)
        {
            List<PartsRequisition> oPartsRequisitions = new List<PartsRequisition>();
            try
            {
                string sSQL = "SELECT * FROM View_PartsRequisitionLog AS HH WHERE HH.PartsRequisitionID=" + oPartsRequisition.PartsRequisitionID.ToString() + " ORDER BY PartsRequisitionLogID";
                oPartsRequisitions = PartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPartsRequisitions = new List<PartsRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPartsRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Get Lots
        [HttpPost]
        public JsonResult GetLots(Lot oInvLot)
        {
            List<Lot> oInvLots = new List<Lot>();
            if (oInvLot.ProductID > 0 && oInvLot.WorkingUnitID > 0 && (int)oInvLot.ParentType > 0)
            {
                //oInvLots = InvLot.Gets((int)oInvLot.ParentType, oInvLot.WorkingUnitID,oInvLot.ProductID, (int)Session[SessionInfo.currentUserID]);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oInvLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Gets Product & Lots
        [HttpPost]
        public JsonResult SearchProducts(Product oProduct)
        {
            List<Product>  oProducts = new List<Product>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_Product";
                string sSQL1 = "";

                #region BUID
                if (oProduct.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID =" + oProduct.BUID.ToString() + ")";
                }
                #endregion

                #region ProductName
                if (oProduct.ProductName == null) { oProduct.ProductName = ""; }
                if (oProduct.ProductName != "")
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ISNULL(ProductName,'')+ISNULL(ProductCode,'') LIKE '%" + oProduct.ProductName + "%'";
                }
                #endregion

                #region Deafult
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " Activity=1";
                #endregion

                #region Invoice Wise Product
                if (oProduct.ProductID > 0) //Hare ProductID  Use as a StyleID
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID IN (SELECT ServiceInvoiceDetail.VehiclePartsID FROM ServiceInvoiceDetail WHERE ServiceInvoiceDetail.ServiceInvoiceID=" + oProduct.ProductID.ToString() + ")";
                }
                #endregion

                sSQL = sSQL + sSQL1 + " Order By ProductName ASC";

                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oProducts.Count() <= 0) throw new Exception("No product found.");
            }
            catch (Exception ex)
            {
                oProducts = new List<Product>();
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchLots(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_Lot";
                string sSQL1 = "";

                #region BUID
                if (oLot.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " BUID =" + oLot.BUID.ToString();
                }
                #endregion

                #region ProductID
                if (oLot.ProductID > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID =" + oLot.ProductID.ToString();
                }
                #endregion

                #region WorkingUnitID
                if (oLot.WorkingUnitID > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " WorkingUnitID =" + oLot.WorkingUnitID.ToString();
                }
                #endregion

                #region LotNo
                if (oLot.LotNo == null) { oLot.LotNo = ""; }
                if (oLot.LotNo != "")
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " LotNo LIKE '%" + oLot.LotNo + "%'";
                }
                #endregion

                #region Deafult
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " Balance>0";
                #endregion

                #region Style Wise Suggested Lot
                if (oLot.StyleID > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " LotID IN (SELECT HH.LotID FROM Lot AS HH WHERE HH.StyleID=" + oLot.StyleID.ToString() + ")";
                }
                #endregion

                sSQL = sSQL + sSQL1 + " ORDER BY LotID ASC";

                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oLots.Count() <= 0) throw new Exception("No Lot found.");
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLotByNo(Lot oLot)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                oLot = Lot.GetByLotNo(oLot.LotNo,oLot.BUID, oLot.WorkingUnitID, (int)Session[SessionInfo.currentUserID]);
                if (oLot== null && oLot.LotID<=0) throw new Exception("Lot Not Found.");
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLot);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region register
        public ActionResult ViewPartsRequisitionRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oPartsRequisition = new PartsRequisition();
            _oPartsRequisition.BUID = buid;

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.ConsuptionTypes = EnumObject.jGets(typeof(EnumPRequisutionType));
            return View(_oPartsRequisition);
        }

        public ActionResult SetSessionSearchCriteria(PartsRequisitionRegister oPartsRequisitionRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oPartsRequisitionRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPartsRequisitionRegister(double ts)
        {
            PartsRequisitionRegister oPartsRequisitionRegister = new PartsRequisitionRegister();
            string _sErrorMesage = "";
            try
            {
                _oPartsRequisitionRegisters = new List<PartsRequisitionRegister>();
                oPartsRequisitionRegister = (PartsRequisitionRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPartsRequisitionRegister);
                _oPartsRequisitionRegisters = PartsRequisitionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPartsRequisitionRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPartsRequisitionRegisters = new List<PartsRequisitionRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oPartsRequisitionRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oPartsRequisitionRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptPartsRequisitionRegisters oReport = new rptPartsRequisitionRegisters();
                byte[] abytes = oReport.PrepareReport(_oPartsRequisitionRegisters, oCompany, oPartsRequisitionRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
                //return null;
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        private string GetSQL(PartsRequisitionRegister oPartsRequisitionRegister)
        {
            string sSearchingData = oPartsRequisitionRegister.ErrorMessage;
            EnumCompareOperator ePartsRequisitionDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPartsRequisitionStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPartsRequisitionEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            
            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oPartsRequisitionRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oPartsRequisitionRegister.BUID.ToString();
            }
            #endregion

            #region PartsRequisitionNo
            if (oPartsRequisitionRegister.RequisitionNo != null && oPartsRequisitionRegister.RequisitionNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RequisitionNo LIKE'%" + oPartsRequisitionRegister.RequisitionNo + "%'";
            }
            #endregion

            #region Vehicle Registration
            if (oPartsRequisitionRegister.VehicleRegID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " VehicleRegID =" + oPartsRequisitionRegister.VehicleRegID;
            }
            #endregion

            #region Product
            if (oPartsRequisitionRegister.ProductID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID =" + oPartsRequisitionRegister.ProductID;
            }
            #endregion

            #region VIN No
            if (oPartsRequisitionRegister.ChassisNo != null && oPartsRequisitionRegister.ChassisNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ChassisNo LIKE'%" + oPartsRequisitionRegister.ChassisNo + "%'";
            }
            #endregion

            #region Customer
            if (oPartsRequisitionRegister.CustomerID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " CustomerID =" + oPartsRequisitionRegister.CustomerID;
            }
            #endregion

            #region PR Type
            if (oPartsRequisitionRegister.PRType > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PRType =" + (int)oPartsRequisitionRegister.PRType;
            }
            #endregion

            #region RequisitionDate Date
            if (ePartsRequisitionDate != EnumCompareOperator.None)
            {
                if (ePartsRequisitionDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requisition Date: " + dPartsRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePartsRequisitionDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requisition Date: Not Equal to " + dPartsRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePartsRequisitionDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requisition Date: Greater Than " + dPartsRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePartsRequisitionDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requisition Date: Smaller Than " + dPartsRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePartsRequisitionDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requisition Date: " + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + " - To - " + dPartsRequisitionEndDate.ToString("dd MMM yyyy");
                }
                else if (ePartsRequisitionDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPartsRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requisition Date: Not Between " + dPartsRequisitionStartDate.ToString("dd MMM yyyy") + " - To - " + dPartsRequisitionEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Report Layout
            if (oPartsRequisitionRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PartsRequisitionRegister ";
                sOrderBy = " ORDER BY  IssueDate, PartsRequisitionID, PartsRequisitionDetailID ASC";
            }

            else if (oPartsRequisitionRegister.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PartsRequisitionRegister ";
                sOrderBy = " ORDER BY  CustomerID, PartsRequisitionID, PartsRequisitionDetailID ASC";
            }
            else if (oPartsRequisitionRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PartsRequisitionRegister ";
                sOrderBy = " ORDER BY  ProductID, PartsRequisitionID, PartsRequisitionDetailID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PartsRequisitionRegister ";
                sOrderBy = " ORDER BY IssueDate, PartsRequisitionID, PartsRequisitionDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        public void ExportToExcelPartsRequisition()
        {
            PartsRequisitionRegister oPartsRequisitionRegister = new PartsRequisitionRegister();
            string _sErrorMesage = "";
            try
            {
                _oPartsRequisitionRegisters = new List<PartsRequisitionRegister>();
                oPartsRequisitionRegister = (PartsRequisitionRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPartsRequisitionRegister);
                _oPartsRequisitionRegisters = PartsRequisitionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPartsRequisitionRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPartsRequisitionRegisters = new List<PartsRequisitionRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
                if (oPartsRequisitionRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oPartsRequisitionRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                double GrandTotaQty = 0, GrandTotalAmount = 0;
                int count = 0, num = 0;
                double SubTotalQty = 0, SubTotalAmount = 0;
                int nPartsRequisitionID = 0;

                if (oPartsRequisitionRegister.ReportLayout == EnumReportLayout.DateWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    int colIndex = 1;
                    int totalColIndex = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Date Wise Requisition(Details)");
                        sheet.Name = "Date Wise Requisition(Details)";
                        sheet.Column(++colIndex).Width = 5; //SL
                        sheet.Column(++colIndex).Width = 10; //req. no
                        sheet.Column(++colIndex).Width = 15; //part no
                        sheet.Column(++colIndex).Width = 15; //part name
                        sheet.Column(++colIndex).Width = 20; //store
                        sheet.Column(++colIndex).Width = 10; //shelf
                        sheet.Column(++colIndex).Width = 10; //rack
                        sheet.Column(++colIndex).Width = 10; //consumption type
                        sheet.Column(++colIndex).Width = 18; //service order no
                        sheet.Column(++colIndex).Width = 18; //party
                        sheet.Column(++colIndex).Width = 15; //model
                        sheet.Column(++colIndex).Width = 12; //reg no
                        sheet.Column(++colIndex).Width = 12; //vin no
                        sheet.Column(++colIndex).Width = 10; //munit
                        sheet.Column(++colIndex).Width = 12; //qty
                        sheet.Column(++colIndex).Width = 12; //unit price
                        sheet.Column(++colIndex).Width = 15; //amount

                        totalColIndex = colIndex;

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = "Date Wise Requisition(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Req. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Part No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Part Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Shelf"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Rack"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Consumption Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Service Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Party"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Model No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Reg. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "VIN No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oPartsRequisitionRegisters.Count > 0)
                        {
                            var data = _oPartsRequisitionRegisters.GroupBy(x => new { x.IssueDateSt }, (key, grp) => new  
                            {
                                IssueDateSt = key.IssueDateSt,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotaQty = 0; GrandTotalAmount = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = "Issue Date : @ " + oData.IssueDateSt; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalQty = 0; SubTotalAmount = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    if (nPartsRequisitionID != 0)
                                    {
                                        if (nPartsRequisitionID != oItem.PartsRequisitionID && count > 1)
                                        {
                                            colIndex = 1;
                                            cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex += 14]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            rowIndex = rowIndex + 1;
                                            colIndex = 1;
                                            SubTotalQty = 0; SubTotalAmount = 0;
                                        }
                                    }
                                    #endregion

                                    //if (sQCNo != oItem.QCNo)
                                    //{
                                    //    num++;
                                    //    int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
                                    //    cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //}
                                    colIndex = 1;
                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = count; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.RequisitionNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ShelfNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.RackNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.PRTypeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ServiceOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ModelNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.VehicleRegNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ChassisNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.Quantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQty += oItem.Quantity;
                                    GrandTotaQty += oItem.Quantity;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.LotUnitPrice.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = (oItem.Quantity * oItem.LotUnitPrice).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalAmount += (oItem.Quantity * oItem.LotUnitPrice);
                                    GrandTotalAmount += (oItem.Quantity * oItem.LotUnitPrice);

                                    rowIndex++;
                                    colIndex = 1;
                                    nPartsRequisitionID = oItem.PartsRequisitionID;
                                }
                                #region subtotal
                                if (nPartsRequisitionID != 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex+=14]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex = rowIndex + 1;
                                    colIndex = 1;
                                    SubTotalQty = 0; SubTotalAmount = 0;
                                }
                                #endregion


                                cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex+=14]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = GrandTotaQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = GrandTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             
                            colIndex = 1;
                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Date_Wise_Requisition_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oPartsRequisitionRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    int colIndex = 1;
                    int totalColIndex = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Product Wise Requisition(Details)");
                        sheet.Name = "Product Wise Requisition(Details)";
                        sheet.Column(++colIndex).Width = 5; //SL
                        sheet.Column(++colIndex).Width = 10; //req. no
                        sheet.Column(++colIndex).Width = 10; //issue date
                        sheet.Column(++colIndex).Width = 15; //part no
                        sheet.Column(++colIndex).Width = 15; //part name
                        sheet.Column(++colIndex).Width = 20; //store
                        sheet.Column(++colIndex).Width = 10; //shelf
                        sheet.Column(++colIndex).Width = 10; //rack
                        sheet.Column(++colIndex).Width = 10; //consumption type
                        sheet.Column(++colIndex).Width = 18; //service order no
                        sheet.Column(++colIndex).Width = 15; //model
                        sheet.Column(++colIndex).Width = 12; //reg no
                        sheet.Column(++colIndex).Width = 12; //vin no
                        sheet.Column(++colIndex).Width = 10; //munit
                        sheet.Column(++colIndex).Width = 12; //qty
                        sheet.Column(++colIndex).Width = 12; //unit price
                        sheet.Column(++colIndex).Width = 15; //amount

                        totalColIndex = colIndex;

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = "Product Wise Requisition(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Req. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Issue Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Part No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Part Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Shelf"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Rack"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Consumption Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Service Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Model No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Reg. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "VIN No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oPartsRequisitionRegisters.Count > 0)
                        {
                            var data = _oPartsRequisitionRegisters.GroupBy(x => new { x.CustomerID, x.CustomerName }, (key, grp) => new  
                            {
                                CustomerID = key.CustomerID,
                                CustomerName = key.CustomerName,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotaQty = 0; GrandTotalAmount = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = "Party : @ " + oData.CustomerName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalQty = 0; SubTotalAmount = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    if (nPartsRequisitionID != 0)
                                    {
                                        if (nPartsRequisitionID != oItem.PartsRequisitionID && count > 1)
                                        {
                                            colIndex = 1;
                                            cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex += 14]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            rowIndex = rowIndex + 1;
                                            colIndex = 1;
                                            SubTotalQty = 0; SubTotalAmount = 0;
                                        }
                                    }
                                    #endregion

                                    //if (sQCNo != oItem.QCNo)
                                    //{
                                    //    num++;
                                    //    int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
                                    //    cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //}
                                    colIndex = 1;
                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = count; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.RequisitionNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.IssueDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ShelfNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.RackNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.PRTypeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ServiceOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ModelNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.VehicleRegNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ChassisNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.Quantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQty += oItem.Quantity;
                                    GrandTotaQty += oItem.Quantity;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.LotUnitPrice.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = (oItem.Quantity * oItem.LotUnitPrice).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalAmount += (oItem.Quantity * oItem.LotUnitPrice);
                                    GrandTotalAmount += (oItem.Quantity * oItem.LotUnitPrice);

                                    rowIndex++;
                                    colIndex = 1;
                                    nPartsRequisitionID = oItem.PartsRequisitionID;
                                }
                                #region subtotal
                                if (nPartsRequisitionID != 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex += 14]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex = rowIndex + 1;
                                    colIndex = 1;
                                    SubTotalQty = 0; SubTotalAmount = 0;
                                }
                                #endregion


                                cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex += 14]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = GrandTotaQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = GrandTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            colIndex = 1;
                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Party_Wise_Requisition_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oPartsRequisitionRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    int colIndex = 1;
                    int totalColIndex = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Product Wise Requisition(Details)");
                        sheet.Name = "Product Wise Requisition(Details)";
                        sheet.Column(++colIndex).Width = 5; //SL
                        sheet.Column(++colIndex).Width = 10; //req. no
                        sheet.Column(++colIndex).Width = 10; //issue date
                        sheet.Column(++colIndex).Width = 15; //part no
                        sheet.Column(++colIndex).Width = 15; //party name
                        sheet.Column(++colIndex).Width = 20; //store
                        sheet.Column(++colIndex).Width = 10; //shelf
                        sheet.Column(++colIndex).Width = 10; //rack
                        sheet.Column(++colIndex).Width = 10; //consumption type
                        sheet.Column(++colIndex).Width = 18; //service order no
                        sheet.Column(++colIndex).Width = 15; //model
                        sheet.Column(++colIndex).Width = 12; //reg no
                        sheet.Column(++colIndex).Width = 12; //vin no
                        sheet.Column(++colIndex).Width = 10; //munit
                        sheet.Column(++colIndex).Width = 12; //qty
                        sheet.Column(++colIndex).Width = 12; //unit price
                        sheet.Column(++colIndex).Width = 15; //amount

                        totalColIndex = colIndex;

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = "Product Wise Requisition(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Req. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Issue Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Part No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Party Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Shelf"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Rack"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Consumption Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Service Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Model No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Reg. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "VIN No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oPartsRequisitionRegisters.Count > 0)
                        {
                            var data = _oPartsRequisitionRegisters.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new
                            {
                                ProductID = key.ProductID,
                                ProductName = key.ProductName,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotaQty = 0; GrandTotalAmount = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = "Product : @ " + oData.ProductName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalQty = 0; SubTotalAmount = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    if (nPartsRequisitionID != 0)
                                    {
                                        if (nPartsRequisitionID != oItem.PartsRequisitionID && count > 1)
                                        {
                                            colIndex = 1;
                                            cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex += 14]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            rowIndex = rowIndex + 1;
                                            colIndex = 1;
                                            SubTotalQty = 0; SubTotalAmount = 0;
                                        }
                                    }
                                    #endregion

                                    //if (sQCNo != oItem.QCNo)
                                    //{
                                    //    num++;
                                    //    int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
                                    //    cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //}
                                    colIndex = 1;
                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = count; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.RequisitionNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.IssueDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ShelfNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.RackNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.PRTypeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ServiceOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ModelNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.VehicleRegNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.ChassisNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.Quantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQty += oItem.Quantity;
                                    GrandTotaQty += oItem.Quantity;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = oItem.LotUnitPrice.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Merge = true; cell.Value = (oItem.Quantity * oItem.LotUnitPrice).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalAmount += (oItem.Quantity * oItem.LotUnitPrice);
                                    GrandTotalAmount += (oItem.Quantity * oItem.LotUnitPrice);

                                    rowIndex++;
                                    colIndex = 1;
                                    nPartsRequisitionID = oItem.PartsRequisitionID;
                                }
                                #region subtotal
                                if (nPartsRequisitionID != 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex += 14]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex = rowIndex + 1;
                                    colIndex = 1;
                                    SubTotalQty = 0; SubTotalAmount = 0;
                                }
                                #endregion


                                cell = sheet.Cells[rowIndex, 2, rowIndex, totalColIndex]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex += 14]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = GrandTotaQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = GrandTotalAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            colIndex = 1;
                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Product_Wise_Requisition_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                
            }
        }

        #endregion

    }
}