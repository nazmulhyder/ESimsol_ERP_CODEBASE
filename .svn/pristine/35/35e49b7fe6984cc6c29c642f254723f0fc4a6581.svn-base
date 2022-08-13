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
using iTextSharp.text.pdf;

namespace ESimSolFinancial.Controllers
{
    public class WorkOrderRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<WorkOrderRegister> _oWorkOrderRegisters = new List<WorkOrderRegister>();        

        #region Actions
        public ActionResult ViewWorkOrderRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            WorkOrderRegister oWorkOrderRegister = new WorkOrderRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApproveBy FROM WorkOrder AS MM WHERE ISNULL(MM.ApproveBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ( (EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails ||  (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails )
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.WorkOrderStates = EnumObject.jGets(typeof(EnumWorkOrderStatus));
            
            return View(oWorkOrderRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(WorkOrderRegister oWorkOrderRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oWorkOrderRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintWorkOrderRegister(double ts)
        {
            WorkOrderRegister oWorkOrderRegister = new WorkOrderRegister();
            try
            {
                _sErrorMesage = "";
                _oWorkOrderRegisters = new List<WorkOrderRegister>();                
                oWorkOrderRegister = (WorkOrderRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oWorkOrderRegister);
                _oWorkOrderRegisters = WorkOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oWorkOrderRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oWorkOrderRegisters = new List<WorkOrderRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oWorkOrderRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptWorkOrderRegisters oReport = new rptWorkOrderRegisters();
                byte[] abytes = oReport.PrepareReport(_oWorkOrderRegisters, oCompany, oWorkOrderRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public void ExportToExcelWorkOrderRegister(double ts)
        {
            WorkOrderRegister oWorkOrderRegister = new WorkOrderRegister();
            try
            {
                _sErrorMesage = "";
                _oWorkOrderRegisters = new List<WorkOrderRegister>();
                oWorkOrderRegister = (WorkOrderRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oWorkOrderRegister);
                _oWorkOrderRegisters = WorkOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oWorkOrderRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oWorkOrderRegisters = new List<WorkOrderRegister>();
                _sErrorMesage = ex.Message;
            }

            byte[] abytes = null;
            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oWorkOrderRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptWorkOrderRegisters oReport = new rptWorkOrderRegisters();
                PdfPTable oPdfPTable = oReport.PrepareExcel(_oWorkOrderRegisters, oCompany, oWorkOrderRegister.ReportLayout, _sDateRange);

                ExportToExcel.WorksheetName = "WorkOrderRegister";
                abytes = ExportToExcel.ConvertToExcel(oPdfPTable);
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                PdfPTable oPdfPTable = oReport.PrepareExcel(_sErrorMesage);
                abytes = ExportToExcel.ConvertToExcel(oPdfPTable);
            }

            Response.ClearContent();
            Response.BinaryWrite(abytes);
            Response.AddHeader("content-disposition", "attachment; filename=Excel001.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }
        #endregion

        #region Support Functions
        private string GetSQL(WorkOrderRegister oWorkOrderRegister)
        {
            _sDateRange = "";
            string sSearchingData = oWorkOrderRegister.SearchingData;
            EnumCompareOperator eWorkOrderDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dWorkOrderStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dWorkOrderEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eExpectedDeliveryDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dExpectedDeliveryDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dExpectedDeliveryDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);


            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oWorkOrderRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oWorkOrderRegister.BUID.ToString();
            }
            #endregion

            #region WorkOrderNo
            if (oWorkOrderRegister.WorkOrderNo != null && oWorkOrderRegister.WorkOrderNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " WorkOrderNo LIKE'%" + oWorkOrderRegister.WorkOrderNo + "%'";
            }
            #endregion

            #region File No
            if (oWorkOrderRegister.FiLeNo != null && oWorkOrderRegister.FiLeNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FiLeNo LIKE'%" + oWorkOrderRegister.FiLeNo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oWorkOrderRegister.ApproveBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApproveBy =" + oWorkOrderRegister.ApproveBy.ToString();
            }
            #endregion

            #region WorkOrderStatus
            if (oWorkOrderRegister.WorkOrderStatus != EnumWorkOrderStatus.Intialize)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " WorkOrderStatus =" + ((int)oWorkOrderRegister.WorkOrderStatus).ToString();
            }
            #endregion

            #region Remarks
            if (oWorkOrderRegister.Remarks != null && oWorkOrderRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Remarks LIKE'%" + oWorkOrderRegister.Remarks + "%'";
            }
            #endregion

            #region Supplier
            if (oWorkOrderRegister.SupplierName != null && oWorkOrderRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " SupplierID IN(" + oWorkOrderRegister.SupplierName + ")";
            }
            #endregion

            #region Product
            if (oWorkOrderRegister.ProductName != null && oWorkOrderRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oWorkOrderRegister.ProductName + ")";
            }
            #endregion

            #region Issue Date
            if (eWorkOrderDate != EnumCompareOperator.None)
            {
                if (eWorkOrderDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),WorkOrderDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date @ " + dWorkOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (eWorkOrderDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),WorkOrderDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Not Equal @ " + dWorkOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (eWorkOrderDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),WorkOrderDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Greater Then @ " + dWorkOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (eWorkOrderDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),WorkOrderDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Smaller Then @ " + dWorkOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (eWorkOrderDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),WorkOrderDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Between " + dWorkOrderStartDate.ToString("dd MMM yyyy") + " To " + dWorkOrderEndDate.ToString("dd MMM yyyy");
                }
                else if (eWorkOrderDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),WorkOrderDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dWorkOrderEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date NOT Between " + dWorkOrderStartDate.ToString("dd MMM yyyy") + " To " + dWorkOrderEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region ExpectedDeliveryDate Date
            if (eExpectedDeliveryDate != EnumCompareOperator.None)
            {
                if (eExpectedDeliveryDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpectedDeliveryDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eExpectedDeliveryDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpectedDeliveryDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eExpectedDeliveryDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpectedDeliveryDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eExpectedDeliveryDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpectedDeliveryDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eExpectedDeliveryDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpectedDeliveryDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eExpectedDeliveryDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpectedDeliveryDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpectedDeliveryDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Approved Date
            if (eApprovedDate != EnumCompareOperator.None)
            {
                if (eApprovedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Amount
            if (ePIAmount != EnumCompareOperator.None)
            {
                if (ePIAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount = " + nPIAmountStsrt.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount != " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount > " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount < " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount NOT BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region Report Layout
           if (oWorkOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_WorkOrderRegister ";
                sOrderBy = " ORDER BY  WorkOrderDate, WorkOrderID, WorkOrderDetailID ASC";
            }
            
            else if (oWorkOrderRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_WorkOrderRegister ";
                sOrderBy = " ORDER BY  SupplierName, WorkOrderID, WorkOrderDetailID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_WorkOrderRegister ";
                sOrderBy = " ORDER BY WorkOrderDate, WorkOrderID, WorkOrderDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
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
        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
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

