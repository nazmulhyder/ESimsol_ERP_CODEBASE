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
using ESimSolFinancial.Controllers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class ProcurementReportController : Controller
    {
        string _sDateRange = "";
        List<rptMPRToGRN> _orptMPRToGRNs = new List<rptMPRToGRN>();

        #region Actions
        public ActionResult ViewMPRToGRNRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            rptMPRToGRN orptMPRToGRN = new rptMPRToGRN();
            
            #region Requisition Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApproveBy FROM PurchaseRequisition AS MM WHERE ISNULL(MM.ApproveBy,0)!=0) ORDER BY HH.UserName";
            List<User> oReqApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oReqApprovalUsers.Add(oApprovalUser);
            oReqApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Requisition User
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequisitionBy FROM PurchaseRequisition AS MM WHERE ISNULL(MM.RequisitionBy,0)!=0) ORDER BY HH.UserName";
            List<User> oReqByUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oByUser = new ESimSol.BusinessObjects.User();
            oByUser.UserID = 0; oByUser.UserName = "--Select Approval User--";
            oReqByUsers.Add(oByUser);
            oReqByUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region PurchaseOrder Approval User
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApproveBy FROM PurchaseOrder AS MM WHERE ISNULL(MM.ApproveBy,0)!=0) ORDER BY HH.UserName";
            List<User> oPOApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oPOUser = new ESimSol.BusinessObjects.User();
            oPOUser.UserID = 0; oPOUser.UserName = "--Select Approval User--";
            oPOApprovalUsers.Add(oPOUser);
            oPOApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion
            #region PurchaseOrder Approval User
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM PurchaseInvoice AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oPIApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oPIUser = new ESimSol.BusinessObjects.User();
            oPIUser.UserID = 0; oPIUser.UserName = "--Select Approval User--";
            oPIApprovalUsers.Add(oPIUser);
            oPIApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit", ((User)(Session[SessionInfo.CurrentUser])).UserID); 
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oReqApprovalUsers;
            ViewBag.ReqByUsers = oReqByUsers;
            ViewBag.POApprovalUsers = oPOApprovalUsers;
            ViewBag.PIApprovalUsers=oPIApprovalUsers;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(orptMPRToGRN);
        }

        private string GetMPRSQL(string sSearchingData ,string sCommonSearching)
        {
            _sDateRange = "";
            string txtPRNo=sSearchingData.Split('~')[0];
            EnumCompareOperator ePRDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[1]);
            DateTime dPurchaseRequisitionStartDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            DateTime dPurchaseRequisitionEndDate = Convert.ToDateTime(sSearchingData.Split('~')[3]);
            int nReqBy = Convert.ToInt32(sSearchingData.Split('~')[4]);
            int nApproveBy = Convert.ToInt32(sSearchingData.Split('~')[5]);
            int nBUID = Convert.ToInt32(sSearchingData.Split('~')[6]);
            string sDepartmentName= sCommonSearching.Split('~')[0];
            string sProductName= sCommonSearching.Split('~')[1];

            string sSQL = "SELECT  PRDetailID, PRID, ProductID, MUnitID, Qty  FROM PurchaseRequisitionDetail WHERE PRDetailID>0 ";
            string sSQLQuery = "", sWhereCluse = "";

            #region BusinessUnit
            if (nBUID> 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" +nBUID.ToString();
            }
            #endregion

            #region PRNo
            if (!string.IsNullOrEmpty(txtPRNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PRNo LIKE'%" + txtPRNo + "%'";
            }
            #endregion

            #region RequisitionBy
            if (nReqBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RequisitionBy =" + nReqBy;
            }
            #endregion

            #region ApproveBy
            if (nApproveBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApproveBy =" + nApproveBy;
            }
            #endregion


            #region DepartmentName
            if (sDepartmentName != null && sDepartmentName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " DepartmentID IN(" + sDepartmentName + ")";
            }
            #endregion

            #region Product
            if (sProductName != null && sProductName != "")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ProductID IN(" + sProductName + ")";
            }
            #endregion

            #region PRDate
            if (ePRDate != EnumCompareOperator.None)
            {
                if (ePRDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Not Equal @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Greater Then @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Smaller Then @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Between " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date NOT Between " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Report Layout
            sSQLQuery = "";
            if (!string.IsNullOrEmpty(sWhereCluse))
            {
                sSQLQuery = " AND PRID IN ( SELECT PRID FROM  PurchaseRequisition " + sWhereCluse + ")";
            }
            #endregion

            sSQL = sSQL + sSQLQuery;
            return sSQL;

        }

        private string GetPOSQL(string sSearchingData, string sCommonSearching)
        {
            _sDateRange = "";
          
            string txtPONo = sSearchingData.Split('~')[0];
            EnumCompareOperator ePODate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[1]);
            DateTime dPurchaseOrderStartDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            DateTime dPurchaseOrderEndDate = Convert.ToDateTime(sSearchingData.Split('~')[3]);
            int nPOApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[4]);
            bool isApprove = Convert.ToBoolean(sSearchingData.Split('~')[5]);
            int nBUID = Convert.ToInt32(sSearchingData.Split('~')[6]);
            string sDepartmentName = sCommonSearching.Split('~')[0];
            string sProductName = sCommonSearching.Split('~')[1];

            string sSQl = "SELECT POID, ProductID, MUnitID, Qty, UnitPrice, RefDetailID  FROM PurchaseOrderDetail WHERE PODetailID>0";
            string sSQLQuery = "", sWhereCluse = "";

            #region BusinessUnit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + nBUID.ToString();
            }
            #endregion

            #region PONo
            if (txtPONo != null && txtPONo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PONo LIKE'%" + txtPONo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (nPOApprovedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApproveBy =" + nPOApprovedBy.ToString();
            }
            #endregion

            
            #region Product
            if (sProductName != null && sProductName != "")
            {
                Global.TagSQL(ref sSQl);
                sSQl = sSQl + " ProductID IN(" + sProductName + ")";
            }
            #endregion

            #region PODate
            if (ePODate != EnumCompareOperator.None)
            {
                if (ePODate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Not Equal @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Greater Then @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Smaller Then @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Between " + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseOrderEndDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date NOT Between " + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseOrderEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Report Layout
            sSQLQuery = "";
            if (!string.IsNullOrEmpty(sWhereCluse))
            {
                sSQLQuery = " AND POID IN ( SELECT POID FROM PurchaseOrder " + sWhereCluse + ")";
            }
            #endregion

            sSQl = sSQl + sSQLQuery;
            return sSQl;

        }

        private string GetPISQL(string sSearchingData, string sCommonSearching)
        {
            _sDateRange = "";
            _sDateRange = "";

            string txtPINo = sSearchingData.Split('~')[0];
            EnumCompareOperator eDateofInvoice = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[1]);
            DateTime dPurchaseInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            DateTime dPurchaseInvoiceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[3]);
            int nPIApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[4]);
            bool isApprove = Convert.ToBoolean(sSearchingData.Split('~')[5]);
            int nBUID = Convert.ToInt32(sSearchingData.Split('~')[6]);
            string sDepartmentName = sCommonSearching.Split('~')[0];
            string sProductName = sCommonSearching.Split('~')[1];

            string sSQL = "SELECT PurchaseInvoiceID, ProductID, MUnitID, Qty, UnitPrice  FROM PurchaseInvoiceDetail WHERE  PurchaseInvoiceDetailID>0 ";
            string sSQLQuery = "", sWhereCluse = "";

            #region BusinessUnit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + nBUID.ToString();
            }
            #endregion

            #region PurchaseInvoiceNo
            if (txtPINo != null && txtPINo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PurchaseInvoiceNo LIKE'%" + txtPINo + "%'";
            }
            #endregion

    

            #region ApprovedBy
            if (nPIApprovedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApprovedBy =" + nPIApprovedBy.ToString();
            }
            #endregion

            #region Product
            if (sProductName != null && sProductName != "")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ProductID IN(" + sProductName + ")";
            }
            #endregion

            #region Issue Date
            if (eDateofInvoice != EnumCompareOperator.None)
            {
                if (eDateofInvoice == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Not Equal @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Greater Then @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Smaller Then @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Between " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date NOT Between " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Report Layout
            sSQLQuery = "";
            if (!string.IsNullOrEmpty(sWhereCluse))
            {
                sSQLQuery = " AND PurchaseInvoiceID IN ( SELECT PurchaseInvoiceID FROM PurchaseInvoice " + sWhereCluse + ")";
            }
            #endregion

            sSQL = sSQL + sSQLQuery;
            return sSQL;
        }

        private string GetGRNSQL(string sSearchingData, string sCommonSearching)
        {
            _sDateRange = "";
           
            string txtGRNNo = sSearchingData.Split('~')[0];
            bool IsGRNRecv = Convert.ToBoolean(sSearchingData.Split('~')[1]);
            int nBUID = Convert.ToInt32(sSearchingData.Split('~')[2]);
            string sDepartmentName = sCommonSearching.Split('~')[0];
            string sProductName = sCommonSearching.Split('~')[1];

            string sSQL = "SELECT GRNID, ProductID, MUnitID, RefType, RefObjectID, RefQty, ReceivedQty,RejectQty,  UnitPrice  FROM GRNDetail  WHERE  GRNDetailID>0 ";
            string sSQLQuery = "", sWhereCluse = "";

            #region BusinessUnit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + nBUID.ToString();
            }
            #endregion

            #region PurchaseInvoiceNo
            if (txtGRNNo != null && txtGRNNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " GRNNo LIKE'%" + txtGRNNo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (IsGRNRecv)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ISNULL(ReceivedBy,0) !=0";
            }
            #endregion

            #region Product
            if (sProductName != null && sProductName != "")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ProductID IN(" + sProductName + ")";
            }
            #endregion

            

            #region Report Layout
            sSQLQuery = "";
            if (!string.IsNullOrEmpty(sWhereCluse))
            {
                sSQLQuery = " AND GRNID IN ( SELECT GRNID FROM GRN " + sWhereCluse + ")";
            }
            #endregion

            sSQL = sSQL + sSQLQuery;
            return sSQL;
        }


        public void ExcelMPRToGRN(FormCollection data)
        {

            //string sMPRSearchingData = data["sMPRSearchingData"];
            //string sPOSearchingData = data["sPOSearchingData"];
            //string sPISearchingData = data["sPISearchingData"];
            //string sGRNSearchingData = data["sGRNSearchingData"];
            //string sCommonSearching = data["sCommonSearching"];
            string sSearchingData = data["sSearchingData"];
            List<rptMPRToGRN> orptMPRToGRNs = new List<rptMPRToGRN>();
            //string SMPRSql= GetMPRSQL(sMPRSearchingData, sCommonSearching);
            //string SPOSql = GetPOSQL(sPOSearchingData, sCommonSearching);
            //string SPISql = GetPISQL(sPISearchingData, sCommonSearching);
            //string SGRNSql = GetGRNSQL(sGRNSearchingData, sCommonSearching);
            int nBUID = Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sPRNo = sSearchingData.Split('~')[3];
            //string sPONo = sSearchingData.Split('~')[4];
            //string sPINo = sSearchingData.Split('~')[5];
            //string sGRNNo = sSearchingData.Split('~')[6];
            //string sNOANo = sSearchingData.Split('~')[7];
            orptMPRToGRNs = rptMPRToGRN.Gets(nBUID, dStartDate, dEndDate, sPRNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            int rowIndex = 2;
            int nMaxColumn = 41;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("MPR To GRN Report");
                sheet.Name = "MPR To GRN Report";

                #region Coloums
                string[] columnHead = new string[] {                                                            
                                                        "SL No",
                                                        "MPR No",
                                                        "MPR Date",
                                                        "Department Name",
                                                        "Requisiton By",
                                                        "MPR ApprovedBy ",
                                                        "Requisiton Qty",

                                                        "Product Code",
                                                        "Product Name",
                                                        "Product Category",
                                                        "Product Group",
                                                        "Specification",

                                                        "Quotation No",

                                                        "NOA No",
                                                        "NOA Date",                                                        
                                                        "NOA Qty",
                                                        "NOA ApprovedBy",
							
                                                        "PO No",
                                                        "PO Date",
                                                        "PO IssueBy ",
                                                        "PO Approved By",
                                                        "PO Qty",

                                                        "Invoice No",
                                                        "Invoice Date",
                                                        "Invoice CreateBy",
                                                        "Invoice ApprovedBy",
                                                        "Invoice Qty",

                                                        "GRN No",
                                                        "GRN By",
                                                        "GRN ReceiveBy",
                                                        "GRN Date",
                                                        "Ref Qty",
                                                        "Unit Name",
                                                        "Reject Qty",
                                                        "Received Qty",
                                                        "YetToReceive Qty",
                                                        "Unit Price",
                                                        "Discount",
                                                        "Expense",
                                                        "TotalAmount",
                                                        "PresentStock"
                                                    };

                colIndex = 2;
                for (int i = 0; i < columnHead.Length; i++)
                {                    
                    sheet.Column(colIndex).Width = 13;
                    colIndex++;
                }

                #endregion

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "MPR To GRN Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex +1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Reporting Date :"+DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Column Header
                colIndex = 2;

                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;
                }

                rowIndex++;
                #endregion


                #region Body
                int count = 0;

                if (orptMPRToGRNs.Any())
                {
                    foreach (rptMPRToGRN oitem in orptMPRToGRNs)
                    {
                        colIndex = 2;
                        ++count;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = count; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.MPRNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.MPRDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //required Date
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.DepartmentName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.RequisitonByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //required for
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ApprovedByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //operationStatus
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ReqQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ProductCategoryName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ProductGroupName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.Specification; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.QuotationNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                        
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.NOANo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.NOADateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.NOAQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.CSApprovedByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.PONo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.PODateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.POCrateByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.POApprovedByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.POQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.InvoiceDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.InvoiceCreateByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.InvoiceApprovedByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.InvQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.GRNNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.GRNCreateByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.GRNReceiveByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.GRNReceiveDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.RefQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //GRNQty
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =oitem.UnitName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.RejectQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ReceivedQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.YetToReceiveQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.Discount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.Expense; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.TotalAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.PresentStock; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                    


                    #region Summary
                    //colIndex = 29;
                    //cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
    
                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =orptMPRToGRNs.Sum(x=>x.ReqQty); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =orptMPRToGRNs.Sum(x=>x.POQty); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = orptMPRToGRNs.Sum(x=>x.InvQty); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    ////GRNQty
                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =""; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = orptMPRToGRNs.Sum(x=>x.RejectQty); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = orptMPRToGRNs.Sum(x=>x.ReceivedQty); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =orptMPRToGRNs.Sum(x=>x.YetToReceiveQty); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =orptMPRToGRNs.Sum(x=>x.UnitPrice); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = orptMPRToGRNs.Sum(x=>x.Discount); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =orptMPRToGRNs.Sum(x=>x.Expense); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = orptMPRToGRNs.Sum(x=>x.TotalAmount); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =orptMPRToGRNs.Sum(x=>x.PresentStock); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //rowIndex++;
                    #endregion
                }
                else
                {
                    for (int i = 0; i < columnHead.Length; i++)
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colIndex++;
                    }
                    rowIndex++;
                }

                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=MPRToGRNStatement.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        //[HttpPost]
        //public ActionResult SetSessionSearchCriteria(rptMPRToGRN orptMPRToGRN)
        //{
        //    this.Session.Remove(SessionInfo.ParamObj);
        //    this.Session.Add(SessionInfo.ParamObj, orptMPRToGRN);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(Global.SessionParamSetMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult PrintPurchaseRequisitionRegister(double ts)
        //{
        //    rptMPRToGRN orptMPRToGRN = new rptMPRToGRN();
        //    try
        //    {
        //        _sErrorMesage = "";
        //        _orptMPRToGRNs = new List<rptMPRToGRN>();
        //        orptMPRToGRN = (rptMPRToGRN)Session[SessionInfo.ParamObj];
        //        string sSQL = this.GetSQL(orptMPRToGRN);
        //        _orptMPRToGRNs = rptMPRToGRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        if (_orptMPRToGRNs.Count <= 0)
        //        {
        //            _sErrorMesage = "There is no data for print!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _orptMPRToGRNs = new List<rptMPRToGRN>();
        //        _sErrorMesage = ex.Message;
        //    }

        //    if (_sErrorMesage == "")
        //    {
        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //        BusinessUnit oBU = new BusinessUnit();
        //        oBU = oBU.Get(_orptMPRToGRNs.Max(x => x.BUID), (int)Session[SessionInfo.currentUserID]);
        //        oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

        //        rptPurchaseRequisitionRegisters oReport = new rptPurchaseRequisitionRegisters();
        //        byte[] abytes = oReport.PrepareReport(_orptMPRToGRNs, oCompany, orptMPRToGRN.ReportLayout, _sDateRange);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {
        //        rptErrorMessage oReport = new rptErrorMessage();
        //        byte[] abytes = oReport.PrepareReport(_sErrorMesage);
        //        return File(abytes, "application/pdf");
        //    }
        //}
        #endregion

        //#region Excel
        //private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        //{
        //    return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        //}
        //private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        //{
        //    ExcelRange cell;
        //    OfficeOpenXml.Style.Border border;

        //    cell = sheet.Cells[nRowIndex, nStartCol++];
        //    if (IsNumber)
        //        cell.Value = Convert.ToDouble(sVal);
        //    else
        //        cell.Value = sVal;
        //    cell.Style.Font.Bold = IsBold;
        //    cell.Style.WrapText = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //    if (IsNumber)
        //    {
        //        cell.Style.Numberformat.Format = _sFormatter;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    }
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    return cell;
        //}
        //public void ExportToExcelPurchaseRequisitionRegister(double ts)
        //{
        //    int PRID = -999;
        //    string Header = "", HeaderColumn = "";

        //    Company oCompany = new Company();
        //    rptMPRToGRN orptMPRToGRN = new rptMPRToGRN();
        //    try
        //    {
        //        _sErrorMesage = "";

        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _orptMPRToGRNs = new List<rptMPRToGRN>();
        //        orptMPRToGRN = (rptMPRToGRN)Session[SessionInfo.ParamObj];
        //        string sSQL = this.GetSQL(orptMPRToGRN);
        //        _orptMPRToGRNs = rptMPRToGRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        if (_orptMPRToGRNs.Count <= 0)
        //        {
        //            _sErrorMesage = "There is no data for print!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _orptMPRToGRNs = new List<rptMPRToGRN>();
        //        _sErrorMesage = ex.Message;
        //    }

        //    if (_sErrorMesage == "")
        //    {
        //        #region Header
        //        List<TableHeader> table_header = new List<TableHeader>();
        //        table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "PR No", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "PR Status", Width = 15f, IsRotate = false });

        //        if (orptMPRToGRN.ReportLayout == EnumReportLayout.DateWiseDetails)
        //            table_header.Add(new TableHeader { Header = "Department Name", Width = 45f, IsRotate = false });
        //        else
        //            table_header.Add(new TableHeader { Header = "PR Date", Width = 25f, IsRotate = false });

        //        table_header.Add(new TableHeader { Header = "Requirement Date", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Approved By Name", Width = 25f, IsRotate = false });

        //        if (orptMPRToGRN.ReportLayout == EnumReportLayout.ProductWise)
        //        {
        //            table_header.Add(new TableHeader { Header = "Required Date", Width = 20f, IsRotate = false });
        //            table_header.Add(new TableHeader { Header = "Department Name", Width = 45f, IsRotate = false });
        //        }
        //        else
        //        {
        //            table_header.Add(new TableHeader { Header = "Product Code", Width = 20f, IsRotate = false });
        //            table_header.Add(new TableHeader { Header = "Product Name", Width = 45f, IsRotate = false });
        //        }

        //        table_header.Add(new TableHeader { Header = "M Unit", Width = 10f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Qty", Width = 10f, IsRotate = false });
        //        #endregion

        //        #region Layout Wise Header
        //        if (orptMPRToGRN.ReportLayout == EnumReportLayout.ProductWise)
        //        {
        //            Header = "Product Wise"; HeaderColumn = "Product Name : ";
        //        }
        //        else if (orptMPRToGRN.ReportLayout == EnumReportLayout.DepartmentWise)
        //        {
        //            Header = "Department Wise"; HeaderColumn = "Department Name : ";
        //        }
        //        else if (orptMPRToGRN.ReportLayout == EnumReportLayout.DateWiseDetails)
        //        {
        //            Header = "Date Wise"; HeaderColumn = "PO Date : ";
        //        }
        //        #endregion

        //        #region Export Excel
        //        int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
        //        ExcelRange cell; ExcelFill fill;
        //        OfficeOpenXml.Style.Border border;

        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            excelPackage.Workbook.Properties.Author = "ESimSol";
        //            excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //            var sheet = excelPackage.Workbook.Worksheets.Add("Purchase Reuisition Register");
        //            sheet.Name = "Purchase Reuisition Register";

        //            foreach (TableHeader listItem in table_header)
        //            {
        //                sheet.Column(nStartCol++).Width = listItem.Width;
        //            }

        //            #region Report Header
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Merge = true;
        //            cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 7, nRowIndex, nEndCol]; cell.Merge = true;
        //            cell.Value = "Purchase Reuisition Register (" + Header + ") "; cell.Style.Font.Bold = true;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Address & Date
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Merge = true;
        //            cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
        //            cell.Value = ""; cell.Style.Font.Bold = false;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Group By Layout Wise
        //            var data = _orptMPRToGRNs.GroupBy(x => new { x.DepartmentName, x.DepartmentID }, (key, grp) => new
        //            {
        //                HeaderName = key.DepartmentName,
        //                TotalQty = grp.Sum(x => x.Qty),
        //                Results = grp.ToList()
        //            });

        //            if (orptMPRToGRN.ReportLayout == EnumReportLayout.ProductWise)
        //            {
        //                data = _orptMPRToGRNs.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
        //                {
        //                    HeaderName = key.ProductName,
        //                    TotalQty = grp.Sum(x => x.Qty),
        //                    Results = grp.ToList()
        //                });
        //            }
        //            else if (orptMPRToGRN.ReportLayout == EnumReportLayout.DateWiseDetails)
        //            {
        //                data = _orptMPRToGRNs.GroupBy(x => new { x.PRDateSt }, (key, grp) => new
        //                {
        //                    HeaderName = key.PRDateSt,
        //                    TotalQty = grp.Sum(x => x.Qty),
        //                    Results = grp.ToList()
        //                });
        //            }
        //            #endregion

        //            #region Data
        //            foreach (var oItem in data)
        //            {
        //                nRowIndex++;

        //                nStartCol = 2;
        //                FillCellMerge(ref sheet, HeaderColumn + oItem.HeaderName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

        //                nRowIndex++;
        //                foreach (TableHeader listItem in table_header)
        //                {
        //                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                }

        //                PRID = 0;
        //                nRowIndex++; int nCount = 0, nRowSpan = 0;
        //                foreach (var obj in oItem.Results)
        //                {
        //                    #region Order Wise Merge
        //                    if (PRID != obj.PRID)
        //                    {
        //                        if (nCount > 0)
        //                        {
        //                            nStartCol = 8;
        //                            FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

        //                            _sFormatter = " #,##0;(#,##0)";
        //                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PRID == PRID).Sum(x => x.Qty).ToString(), true, true);
        //                            nRowIndex++;
        //                        }

        //                        nStartCol = 2;
        //                        nRowSpan = oItem.Results.Where(OrderR => OrderR.PRID == obj.PRID).ToList().Count;

        //                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.PRNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.StatusSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

        //                        if (orptMPRToGRN.ReportLayout == EnumReportLayout.DateWiseDetails)
        //                            FillCellMerge(ref sheet, obj.DepartmentName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        else
        //                            FillCellMerge(ref sheet, obj.PRDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

        //                        FillCellMerge(ref sheet, obj.RequirementDateInString, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.ApprovedByName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                    }
        //                    #endregion

        //                    nStartCol = 8;

        //                    if (orptMPRToGRN.ReportLayout == EnumReportLayout.ProductWise)
        //                    {
        //                        FillCell(sheet, nRowIndex, nStartCol++, obj.ApproveDateSt, false);
        //                        FillCell(sheet, nRowIndex, nStartCol++, obj.DepartmentName, false);
        //                    }
        //                    else
        //                    {
        //                        FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCode, false);
        //                        FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
        //                    }

        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.UnitName, false);
        //                    _sFormatter = " #,##0;(#,##0)";
        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);
        //                    nRowIndex++;

        //                    PRID = obj.PRID;
        //                }

        //                nStartCol = 8;
        //                FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
        //                _sFormatter = " #,##0;(#,##0)";
        //                FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PRID == PRID).Sum(x => x.Qty).ToString(), true, true);
        //                nRowIndex++;

        //                nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
        //                FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
        //                FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalQty.ToString(), true, true);
        //                nRowIndex++;
        //            }

        //            #region Grand Total
        //            nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
        //            FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
        //            FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalQty).ToString(), true, true);
        //            nRowIndex++;
        //            #endregion

        //            cell = sheet.Cells[1, 1, nRowIndex, 11];
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.White);
        //            #endregion

        //            Response.ClearContent();
        //            Response.BinaryWrite(excelPackage.GetAsByteArray());
        //            Response.AddHeader("content-disposition", "attachment; filename=rptMPRToGRN(" + Header + ").xlsx");
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.Flush();
        //            Response.End();
        //        }
        //        #endregion
        //    }
        //}
        //private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        //{
        //    ExcelRange cell;
        //    OfficeOpenXml.Style.Border border;

        //    cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
        //    cell.Merge = true;
        //    cell.Value = sVal;
        //    cell.Style.Font.Bold = false;
        //    cell.Style.WrapText = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //}
        //private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        //{
        //    FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        //}
        //private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        //{
        //    ExcelRange cell;
        //    OfficeOpenXml.Style.Border border;

        //    cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
        //    cell.Merge = true;
        //    cell.Value = sVal;
        //    cell.Style.Font.Bold = isBold;
        //    cell.Style.WrapText = true;
        //    cell.Style.HorizontalAlignment = HoriAlign;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //}
        //#endregion

        //#region Support Functions
        //private string GetSQL(rptMPRToGRN orptMPRToGRN)
        //{
        //    _sDateRange = "";
        //    string sSearchingData = orptMPRToGRN.SearchingData;
        //    EnumCompareOperator ePRDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
        //    DateTime dPurchaseRequisitionStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
        //    DateTime dPurchaseRequisitionEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

        //    EnumCompareOperator eRequirementDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
        //    DateTime dRequirementDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
        //    DateTime dRequirementDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

        //    EnumCompareOperator eApproveDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
        //    DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
        //    DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

        //    string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

        //    #region BusinessUnit
        //    if (orptMPRToGRN.BUID > 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " BUID =" + orptMPRToGRN.BUID.ToString();
        //    }
        //    #endregion

        //    #region PRNo
        //    if (orptMPRToGRN.PRNo != null && orptMPRToGRN.PRNo != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " PRNo LIKE'%" + orptMPRToGRN.PRNo + "%'";
        //    }
        //    #endregion

        //    #region RequisitionBy
        //    if (orptMPRToGRN.RequisitionBy != 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " RequisitionBy =" + orptMPRToGRN.RequisitionBy;
        //    }
        //    #endregion

        //    #region ApproveBy
        //    if (orptMPRToGRN.ApproveBy != 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " ApproveBy =" + orptMPRToGRN.ApproveBy;
        //    }
        //    #endregion

        //    #region PurchaseRequisitionStatus
        //    if (orptMPRToGRN.Status >= 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " Status =" + orptMPRToGRN.Status;
        //    }
        //    #endregion

        //    #region Remarks
        //    if (orptMPRToGRN.Remarks != null && orptMPRToGRN.Remarks != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " Note LIKE'%" + orptMPRToGRN.Remarks + "%'";
        //    }
        //    #endregion

        //    #region DepartmentName
        //    if (orptMPRToGRN.DepartmentName != null && orptMPRToGRN.DepartmentName != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " DepartmentID IN(" + orptMPRToGRN.DepartmentName + ")";
        //    }
        //    #endregion

        //    #region Product
        //    if (orptMPRToGRN.ProductName != null && orptMPRToGRN.ProductName != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " ProductID IN(" + orptMPRToGRN.ProductName + ")";
        //    }
        //    #endregion

        //    #region PRDate
        //    if (ePRDate != EnumCompareOperator.None)
        //    {
        //        if (ePRDate == EnumCompareOperator.EqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "PO Date @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (ePRDate == EnumCompareOperator.NotEqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "PO Date Not Equal @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (ePRDate == EnumCompareOperator.GreaterThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "PO Date Greater Then @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (ePRDate == EnumCompareOperator.SmallerThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "PO Date Smaller Then @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (ePRDate == EnumCompareOperator.Between)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "PO Date Between " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy");
        //        }
        //        else if (ePRDate == EnumCompareOperator.NotBetween)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "PO Date NOT Between " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy");
        //        }
        //    }
        //    #endregion

        //    #region Requisition Date
        //    if (eRequirementDate != EnumCompareOperator.None)
        //    {
        //        if (eRequirementDate == EnumCompareOperator.EqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eRequirementDate == EnumCompareOperator.NotEqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eRequirementDate == EnumCompareOperator.GreaterThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eRequirementDate == EnumCompareOperator.SmallerThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eRequirementDate == EnumCompareOperator.Between)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eRequirementDate == EnumCompareOperator.NotBetween)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //    }
        //    #endregion

        //    #region Approved Date
        //    if (eApproveDate != EnumCompareOperator.None)
        //    {
        //        if (eApproveDate == EnumCompareOperator.EqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApproveDate == EnumCompareOperator.NotEqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApproveDate == EnumCompareOperator.GreaterThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApproveDate == EnumCompareOperator.SmallerThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApproveDate == EnumCompareOperator.Between)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApproveDate == EnumCompareOperator.NotBetween)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //    }
        //    #endregion

        //    #region Report Layout
        //    if (orptMPRToGRN.ReportLayout == EnumReportLayout.DateWiseDetails)
        //    {
        //        sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
        //        sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
        //        sOrderBy = " ORDER BY  PRDate, PRID ASC";
        //    }
        //    else if (orptMPRToGRN.ReportLayout == EnumReportLayout.DepartmentWise)
        //    {
        //        sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
        //        sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
        //        sOrderBy = " ORDER BY  DepartmentName,DepartmentID, PRID ASC";
        //    }
        //    else if (orptMPRToGRN.ReportLayout == EnumReportLayout.ProductWise)
        //    {
        //        sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
        //        sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
        //        sOrderBy = " ORDER BY  ProductName,ProductID, PRID ASC";
        //    }
        //    else
        //    {
        //        sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
        //        sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
        //        sOrderBy = " ORDER BY PRDate, PRID ASC";
        //    }
        //    #endregion

        //    sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
        //    return sSQLQuery;
        //}
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
        //public Image GetCompanyTitle(Company oCompany)
        //{
        //    if (oCompany.OrganizationTitle != null)
        //    {
        //        string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
        //        if (System.IO.File.Exists(fileDirectory))
        //        {
        //            System.IO.File.Delete(fileDirectory);
        //        }

        //        MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(fileDirectory, ImageFormat.Jpeg);
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