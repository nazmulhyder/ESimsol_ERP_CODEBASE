using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class TwistingController : Controller
    {
        #region Declaration
        Twisting _oTwisting = new Twisting();
        List<Twisting> _oTwistings = new List<Twisting>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        public ActionResult ViewTwistings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Twisting).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.TwistingOrderTypes = EnumObject.jGets(typeof(EnumTwistingOrderType)).Where(x => x.id != (int)EnumTwistingOrderType.None);
            _oTwistings = new List<Twisting>();
            _oTwistings = Twisting.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oTwistings);
        }
        public ActionResult ViewTwisting(int id, int buid)
        {
            _oTwisting = new Twisting();
            if (id > 0)
            {
                _oTwisting = _oTwisting.Get(id, (int)Session[SessionInfo.currentUserID]);
                //_oTwisting.TwistingDetails = TwistingDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oTwisting.TwistingDetails = TwistingDetail.Gets("SELECT *,(ISNULL((SELECT Qty FROM DyeingOrderDetail WHERE DyeingOrderDetailID=View_TwistingDetail.DyeingOrderDetailID),0) - ISNULL((SELECT SUM(Qty) FROM TwistingDetail WHERE DyeingOrderDetailID = View_TwistingDetail.DyeingOrderDetailID AND TwistingDetailID!=View_TwistingDetail.TwistingDetailID),0)) AS YetQty FROM View_TwistingDetail WHERE TwistingID="+id, (int)Session[SessionInfo.currentUserID]);
                _oTwisting.TwistingDetails_Product = _oTwisting.TwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Receive).ToList();
                _oTwisting.TwistingDetails_Twisting = _oTwisting.TwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Disburse).ToList();
                _oTwisting.TwistingDetails = _oTwisting.TwistingDetails_Twisting;
            }

            #region Issue Stores
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            oIssueStores = WorkingUnit.GetsPermittedStore(0, EnumModuleName.Twisting, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region Received Stores
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(0, EnumModuleName.Twisting, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.IssueStores = oIssueStores;
            ViewBag.ReceivedStores = oReceivedStores;
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.TwistingTypes = EnumObject.jGets(typeof(EnumColorType));
            ViewBag.TwistingOrderTypes = EnumObject.jGets(typeof(EnumTwistingOrderType)).Where(x => x.id != (int)EnumTwistingOrderType.None);
            return View(_oTwisting);
        }
        public ActionResult AdvSearchTwisting()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Save(Twisting oTwisting)
        {
            _oTwisting = new Twisting();
            try
            {
                _oTwisting = oTwisting;
                _oTwisting = _oTwisting.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTwisting = new Twisting();
                _oTwisting.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTwisting);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Twisting oTwisting)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oTwisting.Delete(oTwisting.TwistingID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region searching
        [HttpPost]
        public JsonResult GetsSearchByThreeParam(Twisting oTwisting)
        {
            _oTwistings = new List<Twisting>();
            try
            {
                string sSQL = "SELECT * FROM View_Twisting ";
                string sSReturn = "";
                if (oTwisting.TWNo != "" && oTwisting.TWNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " TWNo LIKE '%" + oTwisting.TWNo + "%'";
                }
                if (oTwisting.DyeingOrderNo != "" && oTwisting.DyeingOrderNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " DyeingOrderNo LIKE '%" + oTwisting.DyeingOrderNo + "%'";
                }
                if (oTwisting.TwistingOrderTypeInt > 0)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " TwistingOrderType =" + oTwisting.TwistingOrderTypeInt;
                }
                sSQL += sSReturn;
                _oTwistings = Twisting.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oTwisting = new Twisting();
                _oTwisting.ErrorMessage = ex.Message;
                _oTwistings.Add(_oTwisting);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTwistings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Approve, UnApprove, Complete
        [HttpPost]
        public JsonResult Approve(Twisting oTwisting)
        {
            _oTwisting = new Twisting();
            oTwisting.Status = EnumTwistingStatus.Approve;
            _oTwisting = oTwisting.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTwisting);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApprove(Twisting oTwisting)
        {
            _oTwisting = new Twisting();
            oTwisting.Status = EnumTwistingStatus.Initialize;
            _oTwisting = oTwisting.UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTwisting);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Complete(Twisting oTwisting)
        {
            _oTwisting = new Twisting();
            _oTwisting = oTwisting.Complete(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTwisting);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search
        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<Twisting> oTwistings = new List<Twisting>();
            Twisting oTwisting = new Twisting();
            try
            {
                string sSQL = GetSQL(sTemp);
                oTwistings = Twisting.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTwisting.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTwistings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nCount = 0;
            int nOrderDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtTwistingEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nReceiveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nApproveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtApproveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtApproveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nCompletedDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtCompletedDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtCompletedEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            string sOrderNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            string sLotNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            bool nYTStart = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTComplete = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTDelivery = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            int nOrderType = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            //int nRequisitionType = Convert.ToInt32(sTemp.Split('~')[9]);
            //string sRequisitionNo = Convert.ToString(sTemp.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_Twisting";
            string sReturn = "";

            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion

            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderNo Like '%" + sOrderNo + "%'";
            }
           
            #endregion
            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo Like '%" + sLotNo + "%'";
            }
            #endregion
            #region nYTStart
            if (nYTStart)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Initialize;
            }
            #endregion
            #region nYTComplete
            if (nYTComplete)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Running;
            }
            #endregion
            #region nYTDelivery
            if (nYTDelivery)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Completed;
            }
            #endregion
            #region Order Date Wise
            if (nOrderDateCompare > 0)
            {
                if (nOrderDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtTwistingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtTwistingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Receive Date Wise
            if (nReceiveDateCompare > 0)
            {
                if (nReceiveDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtTwistingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtTwistingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Approve Date Wise
            if (nApproveDateCompare > 0)
            {
                if (nApproveDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nApproveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nApproveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nApproveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nApproveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nApproveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtApproveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Completed Date Wise
            if (nCompletedDateCompare > 0)
            {
                if (nCompletedDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CompletedDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompletedDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CompletedDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompletedDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CompletedDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompletedDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CompletedDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompletedDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CompletedDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompletedDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),CompletedDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtCompletedEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Order Type
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TwistingOrderType =" +nOrderType;
            }
            #endregion


            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region PrintTwisting
        [HttpGet]
        public ActionResult PrintTwistings(string sValue)
        {
            List<TwistingDetail> oTwistingDetails = new List<TwistingDetail>();

            List<Twisting> oTwistings = new List<Twisting>();
            Twisting oTwisting = new Twisting();
            try
            {
                string sSQL = GetSQL(sValue);
                oTwistings = Twisting.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = string.Join(",", oTwistings.Select(x => x.TwistingID).ToList());
                if (!string.IsNullOrEmpty(sSQL))
                {
                    oTwistingDetails = TwistingDetail.Gets("SELECT * FROM view_TwistingDetail AS HH  where  HH.TwistingID in (" + sSQL + ")", (int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                oTwisting.ErrorMessage = ex.Message;
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptPrintTwistings oReport = new rptPrintTwistings();
            int nUserID = (int)Session[SessionInfo.currentUserID];
            byte[] abytes = oReport.PrepareReport(oTwistings,oTwistingDetails, oCompany, "", nUserID);
            return File(abytes, "application/pdf");

        }
        public void PrintTwistingsXL(string sValue)
        {
            List<Twisting> oTwistings = new List<Twisting>();
            Twisting oTwisting = new Twisting();
            try
            {
                string sSQL = GetSQL(sValue);
                oTwistings = Twisting.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                List<TwistingDetail> oTwistingDetails = new List<TwistingDetail>();
                sSQL = string.Join(",", oTwistings.Select(x => x.TwistingID).ToList());
                if (!string.IsNullOrEmpty(sSQL))
                {
                    oTwistingDetails = TwistingDetail.Gets("SELECT * FROM view_TwistingDetail AS HH  where  HH.TwistingID in (" + sSQL + ")", (int)Session[SessionInfo.currentUserID]);
                }

                #region EXCEL
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                if (oTwistings == null)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }   
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int nTotalCol = 0;
                    int nCount = 0;
                    int colIndex = 2;
                  
                    List<TwistingDetail> TwistingDetails_Product = new List<TwistingDetail>();
                    List<TwistingDetail> TwistingDetails_Twisting = new List<TwistingDetail>();
                    string oPrevCounts = "";
                    string oPresentCount = "";
                    string sPresentLots = "";
                    string sColorName = "";
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Twisting Report");
                        sheet.Name = "Twisting report";
                        sheet.Column(colIndex++).Width = 10; //1
                        sheet.Column(colIndex++).Width = 25; //2
                        sheet.Column(colIndex++).Width = 25; //3
                        sheet.Column(colIndex++).Width = 25; //4
                        sheet.Column(colIndex++).Width = 40; //5   
                        sheet.Column(colIndex++).Width = 40; //6
                        sheet.Column(colIndex++).Width = 25; //7
                        sheet.Column(colIndex++).Width = 20; //8
                        sheet.Column(colIndex++).Width = 20; //8
                        sheet.Column(colIndex++).Width = 20; //9
                        sheet.Column(colIndex++).Width = 20; //10  
               
                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Doubling & Twisting Production Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Previous Count"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Present Count"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Shade"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Req. No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch/Lot"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                        rowIndex++;
                        #endregion

                        #region DATA
                        nCount = 1; colIndex = 2;
                        foreach (Twisting oItem in oTwistings)
                        {
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nCount); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompletedDateSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DyeingOrderNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                            TwistingDetails_Product = oTwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Receive && x.TwistingID == oItem.TwistingID).ToList();
                            TwistingDetails_Twisting = oTwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Disburse && x.TwistingID == oItem.TwistingID).ToList();

                            oPrevCounts = "";
                            foreach (TwistingDetail _oPrevCount in TwistingDetails_Twisting)
                            {
                                oPrevCounts += _oPrevCount.ProductName + ", ";

                            }

                            oPresentCount = ""; sPresentLots = "";
                            foreach (TwistingDetail _oPresentCount in TwistingDetails_Product)
                            {

                                oPresentCount += _oPresentCount.ProductName + ", ";
                                sPresentLots += _oPresentCount.LotNo + ", ";
                            }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oPrevCounts != "" ? oPrevCounts : ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oPresentCount != "" ? oPresentCount : ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            sColorName = string.Join("+", TwistingDetails_Twisting.Select(x => x.ColorName).ToList());

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (string.IsNullOrEmpty(sColorName) ? "Grey" : sColorName); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TWNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sPresentLots != "" ? sPresentLots : ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompletedByName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex++;
                            nCount++;
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Total:";cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = _oTwistings.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex++;

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_Twisting.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();

                        #endregion
                    }

                       
                #endregion

            }
            catch (Exception ex)
            {
                oTwisting.ErrorMessage = ex.Message;
            }
        
        }

        #endregion

        #region Product
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.Twisting, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.Twisting, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetsDyeingOrder
        [HttpPost]
        public JsonResult GetsDyeingOrder(DUProductionYetTo oDUProductionYetTo)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            if (oDUProductionYetTo.DyeingOrderType > 0)
            {
                string sSQL = "";
                if (oDUProductionYetTo.DyeingOrderType == 10)
                    sSQL = "SELECT top(100)* FROM View_DyeingOrder WHERE DyeingOrderType=" + oDUProductionYetTo.DyeingOrderType;
                else
                    sSQL = "SELECT top(100)* FROM View_DyeingOrder WHERE DyeingOrderType in (Select OrderType  from DUOrderSetup where IsInHouse=1)";

                if(oDUProductionYetTo.ContractorID>0)
                    sSQL += "AND ContractorID=" + oDUProductionYetTo.ContractorID;

                sSQL += " AND OrderNoFull LIKE '%" + oDUProductionYetTo.OrderNo + "%' order by DyeingOrderID DESC";
                oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetsDyeingOrderDetails
        [HttpPost]
        public JsonResult GetsDyeingOrderDetails(Twisting oTwisting)
        {
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            if (oTwisting.DyeingOrderID > 0)
            {
                 string sSQL="";
                //if(oTwisting.TwistingID==0)
                 sSQL = " SELECT *,(ISNULL(View_DyeingOrderDetail.Qty,0) - ISNULL((SELECT SUM(Qty) FROM TwistingDetail WHERE DyeingOrderDetailID = View_DyeingOrderDetail.DyeingOrderDetailID),0)) AS YetQty FROM View_DyeingOrderDetail WHERE DyeingOrderID=" + oTwisting.DyeingOrderID + " AND DyeingOrderDetailID NOT IN "
                            + "(SELECT  ISNULL(DyeingOrderDetailID,0) FROM TwistingDetail WHERE InOutType=" + (int)EnumInOutType.Receive + " AND TwistingID IN (SELECT TwistingID FROM Twisting WHERE DyeingOrderID=" + oTwisting.DyeingOrderID + "))";
                
                oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDyeingOrderDetails.Count > 0)
                {
                    int nBuyerCombo = oDyeingOrderDetails.Min(x => x.BuyerCombo);
                    oDyeingOrderDetails = oDyeingOrderDetails.Where(x => x.BuyerCombo == nBuyerCombo).ToList();
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDyeingOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets Lot
        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            try
            {
                string sSQL = "SELECT * FROM View_Lot WHERE LotID<>0 ";

                if (oLot.ProductID > 0)
                    sSQL = sSQL + " And ProductID = " + oLot.ProductID;
                if (oLot.BUID > 0)
                    sSQL = sSQL + " And BUID = " + oLot.BUID;
                if (oLot.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;
                if (!string.IsNullOrEmpty(oLot.LotNo))
                    sSQL = sSQL + " And LotNo LIKE '%" + oLot.LotNo + "%'";

                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLotByHouse(LotParent oLotParent)
        {
            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            string sLotID = "";
            string sSQL = "";
            try
            {
                oLotParent.LotNo = (!string.IsNullOrEmpty(oLotParent.LotNo)) ? oLotParent.LotNo.Trim() : "";

                if (oLotParent.OrderType != 1) //1=Open Twisting
                {
                    if (oLotParent.Params == "IsInHouse")
                    {

                        sSQL = "Select * from View_FabricLotAssign where Balance>0.0 ";
                        //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                        //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                        if (oLotParent.ProductID > 0)
                            sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                        if (oLotParent.DyeingOrderID > 0)
                            sSQL = sSQL + "  and FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where DyeingOrderID=" + oLotParent.DyeingOrderID + " )";
                        oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        sLotID = string.Join(",", oFabricLotAssigns.Select(x => x.LotID).ToList());
                        if (oFabricLotAssigns.Count <= 0)
                        {
                            throw new Exception("Lot yet not assign with this order!!");
                        }
                    }
                    else
                    {
                        sSQL = "Select * from View_LotParent Where LotID<>0 ";
                        //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                        //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                        if (oLotParent.ProductID > 0)
                            sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                        if (oLotParent.DyeingOrderID > 0)
                            sSQL = sSQL + " And DyeingOrderID=" + oLotParent.DyeingOrderID;
                        oLotParents = LotParent.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        sLotID = string.Join(",", oLotParents.Select(x => x.LotID).ToList());
                        if (oLotParent.DyeingOrderID <= 0)
                        {
                            throw new Exception("Lot yet not assign with this order!!");
                        }
                    }
                }
                
                sSQL = "Select * from View_Lot Where Balance>0 and WorkingUnitID>0";

                if (oLotParent.WorkingUnitID<=0)
                {
                    throw new Exception("Store not found!!");
                }

                if (oLotParent.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLotParent.WorkingUnitID;
                //if (oLotParent.ContractorID > 0) { sSQL = sSQL + " And ContractorID=" + oLotParent.ContractorID; }

                if (oLotParent.OrderType != 1) //1=Open Twisting
                {
                    if (!string.IsNullOrEmpty(sLotID))
                        sSQL = sSQL + " And  (LotID in (" + sLotID + ") or ParentLotID in (" + sLotID + "))";

                    if (string.IsNullOrEmpty(sLotID))
                    {
                        throw new Exception("Lot yet not assign with this order!!");
                    }
                }
                else
                {
                    if (oLotParent.ProductID > 0)
                        sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                }
                
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oLotParent.OrderType != 1) //1=Open Twisting
                {
                    if (oLotParent.Params == "IsInHouse")
                    {
                        oLots.ForEach(x =>
                        {
                            if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID)).Count() > 0)
                            {
                                x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                                x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.ProductID > 0).FirstOrDefault().Balance;
                                // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                            }
                        });
                    }
                    else
                    {
                        oLots.ForEach(x =>
                        {
                            if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID)).Count() > 0)
                            {
                                x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                                x.StockValue = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().Balance;
                                // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                            }
                        });
                    }
                }
                
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                Lot oLot = new Lot();

                oLot.ErrorMessage = ex.Message; oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region Report
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
        public ActionResult PrintTwisting(string sTemp)
        {
            List<Twisting> oTwistingList = new List<Twisting>();
            Twisting oTwisting = new Twisting();

            int nBUID;
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                string sSQL = GetSQL(sTemp);
                oTwistingList = Twisting.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[11]);
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //rptTwisting oReport = new rptTwisting();
            //byte[] abytes = oReport.PrepareReport(oTwistingList, oCompany, oBusinessUnit, "Soft Winding Report", "");
            //return File(abytes, "application/pdf");
            return null;
        }

        public void ExportToExcelTwisting(string sTemp)
        {
            Twisting oTwisting = new Twisting();
            string _sErrorMesage;
            int nBUID=0;
            try
            {
                _sErrorMesage = "";
                _oTwistings = new List<Twisting>();
                string sSQL = GetSQL(sTemp);
                _oTwistings = Twisting.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[11]);
                
                if (_oTwistings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oTwistings = new List<Twisting>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                //#region Print XL

                //int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0;
                //ExcelRange cell;
                //ExcelFill fill;
                //OfficeOpenXml.Style.Border border;

                //using (var excelPackage = new ExcelPackage())
                //{
                //    excelPackage.Workbook.Properties.Author = "ESimSol";
                //    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                //    var sheet = excelPackage.Workbook.Worksheets.Add("Twisting");
                //    sheet.Name = "Soft Winding ";
                //    sheet.Column(++nColumn).Width = 10; //SL No
                //    sheet.Column(++nColumn).Width = 15; //Recv Date
                //    sheet.Column(++nColumn).Width = 15; //Order No
                //    sheet.Column(++nColumn).Width = 35; //Buyer
                //    sheet.Column(++nColumn).Width = 15; //Lot
                //    sheet.Column(++nColumn).Width = 35; //Product
                //    sheet.Column(++nColumn).Width = 15; //OrderQty
                //    sheet.Column(++nColumn).Width = 15; //RcvQty
                //    sheet.Column(++nColumn).Width = 15; //DlvQy
                //    sheet.Column(++nColumn).Width = 15; //NoOfCone
                //    sheet.Column(++nColumn).Width = 15; //Status
                //    //   nEndCol = 12;

                //    #region Report Header
                //    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                //    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                //    cell.Value = "Soft Winding Report"; cell.Style.Font.Bold = true;
                //    cell.Style.WrapText = true;
                //    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    nRowIndex = nRowIndex + 1;
                //    #endregion

                //    #region Address & Date
                //    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-4]; cell.Merge = true;
                //    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                //    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //    //cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                //    //cell.Value = ""; cell.Style.Font.Bold = false;
                //    //cell.Style.WrapText = true;
                //    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    nRowIndex = nRowIndex + 1;
                //    #endregion


                //    #region Report Data

                //    #region Blank
                //    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                //    cell.Value = ""; cell.Style.Font.Bold = true;
                //    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                //    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    nRowIndex = nRowIndex + 1;
                //    #endregion

                //    #region
                //    nColumn = 1;
                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Date"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Qty"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Delivered Qty"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "No Of Cone"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    nRowIndex++;
                //    #endregion

                //    #region Data
                //    foreach (Twisting oItem in _oTwistings)
                //    {
                //        nCount++;
                //        nColumn = 1;
                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ReceiveDateST; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DyeingOrderNo; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_Order; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //        cell.Style.Numberformat.Format = "# #,##0.00";
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =oItem.Qty; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //        cell.Style.Numberformat.Format = "# #,##0.00";
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_RSOut; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //        cell.Style.Numberformat.Format = "# #,##0.00";
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BagNo; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //        cell.Style.Numberformat.Format = "# #,##0.00";
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StatusST; cell.Style.Font.Bold = false;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //        nRowIndex++;
                //    }
                //    #endregion

                //    #region Total
                //    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    double nValue = _oTwistings.Select(c => c.Qty_Order).Sum();
                //    cell = sheet.Cells[nRowIndex, nEndCol - 4]; cell.Value = nValue; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //    cell.Style.Numberformat.Format = "# #,##0.00";
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    nValue = _oTwistings.Select(c => c.Qty).Sum();
                //    cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //    cell.Style.Numberformat.Format = "# #,##0.00";
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    nValue = _oTwistings.Select(c => c.Qty_RSOut).Sum();
                //    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //    cell.Style.Numberformat.Format = "# #,##0.00";
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    nRowIndex = nRowIndex + 1;
                //    #endregion
                //    #endregion

                //    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                //    fill.BackgroundColor.SetColor(Color.White);

                //    Response.ClearContent();
                //    Response.BinaryWrite(excelPackage.GetAsByteArray());
                //    Response.AddHeader("content-disposition", "attachment; filename=Twisting.xlsx");
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    Response.Flush();
                //    Response.End();
                //}
                //#endregion
            }
        }

        [HttpGet]
        public ActionResult PrintTwistingPreview(int nId, int tId)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            List<TwistingDetail> TwistingDetails = new List<TwistingDetail>();
            List<TwistingDetail> TwistingDetails_Product = new List<TwistingDetail>();
            List<TwistingDetail> TwistingDetails_Twisting = new List<TwistingDetail>();
            try
            {
                oDyeingOrder = DyeingOrder.Get(nId, (int)Session[SessionInfo.currentUserID]);
                _oTwisting.TwistingDetails = TwistingDetail.Gets(tId, (int)Session[SessionInfo.currentUserID]);
                TwistingDetails_Product = _oTwisting.TwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Receive).ToList();
                TwistingDetails_Twisting = _oTwisting.TwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Disburse).ToList();

            }
            catch (Exception ex)
            {
                _oTwisting = new Twisting();
                _oTwisting.ErrorMessage = ex.Message;
            }
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptTwistingPreview oReport = new rptTwistingPreview();
                byte[] abytes = oReport.PrepareReport(oDyeingOrder, TwistingDetails_Product,TwistingDetails_Twisting,oCompany,"");           
                return File(abytes, "application/pdf");
        }

        #endregion
    }

}

