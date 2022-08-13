using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;


namespace ESimSolFinancial.Controllers
{
    public class FNExecutionOrderController : Controller
    {
        #region Declaration
        List<FNExecutionOrderStatus> _oFNExecutionOrderStatusList = new List<FNExecutionOrderStatus>();

        string _sErrorMessage = "";
        #endregion

        #region FN Execution Order

        #region Search




        #endregion Search

        #region Print

        public Image GetCompanyImgHeader(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "CompanyImageTitle.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region FNEO Status
        public ActionResult ViewFNExecutionOrderStatus(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            _oFNExecutionOrderStatusList = new List<FNExecutionOrderStatus>();
            //ViewBag.FabricSalesContracts = FNExecutionOrderStatus.GetsReport(" ", ((User)(Session[SessionInfo.CurrentUser])).UserID);


            //List<FabricSalesContractDetail> oFabricSalesContractDetails = FabricSalesContractDetail.GetsReport(" ", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //ViewBag.FabricSalesContracts = FabricSalesContractDetail.GetsReport(" ", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //_oFNExecutionOrderStatusList = FNExecutionOrderStatus.GetsReport(" ", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.FabricSalesContracts = _oFNExecutionOrderStatusList;
            return View(_oFNExecutionOrderStatusList);
        }
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        private void PrintReport(List<FNExecutionOrderStatus> oFNExecutionOrderStatuss)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "M K T Ref", Width = 17f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO No", Width = 13f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Process Type", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Constraction", Width = 35f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Order Qty", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Percent", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Raw Fabric Receive Qty", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Plan Qty", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Batch Qty", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Delivered Qty", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Balance", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Order Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Order Report");
                sheet.Name = "Order Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "FN Execution Order"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                foreach (var obj in oFNExecutionOrderStatuss)
                {
                    nStartCol = 2;
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SCNo, false);

                    ExcelTool.Formatter = "";// #,##0;(#,##0)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DispoNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProcessTypeName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Construction.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OrderQtyInStr.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DispoQtyInStr, false);


                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DispoPercent, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RawFabricRcvQtyInStr.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PlannedQtyInStr.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BatchQtyInStr.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DeliveredQtyInStr.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BalanceInStr.ToString(), false);

                    nRowIndex++;
                }
                #endregion

                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=OrderReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        //[HttpPost]
        public void PrintExcel(string sParams)
        {
            List<FNExecutionOrderStatus> oFNExecutionOrderStatuss = new List<FNExecutionOrderStatus>();
            FNExecutionOrderStatus oFNExecutionOrderStatus = new FNExecutionOrderStatus();
            oFNExecutionOrderStatus.Params = sParams;
            try
            {
                string sString = MakeSQL(oFNExecutionOrderStatus);
                oFNExecutionOrderStatuss = FNExecutionOrderStatus.GetsReport(sString, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                PrintReport(oFNExecutionOrderStatuss);

            }
            catch (Exception ex)
            {

            }
        }
        private string MakeSQL(FNExecutionOrderStatus oFNExecutionOrderStatus)
        {
            List<FNExecutionOrderStatus> oFNExecutionOrderStatuss = new List<FNExecutionOrderStatus>();
            try
            {
                string sSQL = " ";
                string sSQLSub = "Select FabricSalesContractID from FabricSalesContract Where OrderType in (3,2,9,10,11,12)";
                if (!string.IsNullOrEmpty(oFNExecutionOrderStatus.Params))
                {
                    string sFabricNo = oFNExecutionOrderStatus.Params.Split('~')[0];
                    int nIssueDateCom = Convert.ToInt32(oFNExecutionOrderStatus.Params.Split('~')[1]);
                    DateTime dStartDate = Convert.ToDateTime(oFNExecutionOrderStatus.Params.Split('~')[2]);
                    DateTime dEndDate = Convert.ToDateTime(oFNExecutionOrderStatus.Params.Split('~')[3]);
                    int nProcessType = Convert.ToInt32(oFNExecutionOrderStatus.Params.Split('~')[4]);
                    string sBuyerIDs = oFNExecutionOrderStatus.Params.Split('~')[5];
                    string sFactoryIDs = oFNExecutionOrderStatus.Params.Split('~')[6];
                    string sPONo = oFNExecutionOrderStatus.Params.Split('~')[9];
                    string sDispoNo = oFNExecutionOrderStatus.Params.Split('~')[10];


                    if (!string.IsNullOrEmpty(sFabricNo))
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " FabricNo like ''%" + sFabricNo + "%''";
                    }
                    if (nIssueDateCom > 0)
                    {
                        if (nIssueDateCom == 1)
                        {
                            Global.TagSQL(ref sSQLSub);
                            sSQLSub = sSQLSub + "CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dStartDate.ToString("dd MMM yyyy") + "'',106))";
                        }
                        if (nIssueDateCom == 2)
                        {
                            Global.TagSQL(ref sSQLSub);
                            sSQLSub = sSQLSub + "CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dStartDate.ToString("dd MMM yyyy") + "'',106))";
                        }
                        if (nIssueDateCom == 3)
                        {
                            Global.TagSQL(ref sSQLSub);
                            sSQLSub = sSQLSub + "CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dStartDate.ToString("dd MMM yyyy") + "'',106))";
                        }
                        if (nIssueDateCom == 4)
                        {
                            Global.TagSQL(ref sSQLSub);
                            sSQLSub = sSQLSub + "CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dStartDate.ToString("dd MMM yyyy") + "'',106))";
                        }
                        if (nIssueDateCom == 5)
                        {
                            Global.TagSQL(ref sSQLSub);
                            sSQLSub = sSQLSub + "CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dStartDate.ToString("dd MMM yyyy") + "'',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dEndDate.ToString("dd MMM yyyy") + "'',106))";
                        }
                        if (nIssueDateCom == 6)
                        {
                            Global.TagSQL(ref sSQLSub);
                            sSQLSub = sSQLSub + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dStartDate.ToString("dd MMM yyyy") + "'',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dEndDate.ToString("dd MMM yyyy") + "'',106))";
                        }
                    }
                    if (!string.IsNullOrEmpty(sPONo))
                    {
                        sPONo = sPONo.Trim();
                        //Global.TagSQL(ref sSQL);
                        sSQLSub = sSQLSub + " and SCNo like ''%" + sPONo + "%''";
                    }

                    if (!string.IsNullOrEmpty(sDispoNo))
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " ExeNo like ''%" + sDispoNo + "%''";
                    }

                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + "FabricSalesContractID in (" + sSQLSub + ")";

                    if (nProcessType > 0)
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + "ProcessType=" + nProcessType + ")";
                    }
                    //if (sBuyerIDs != "" || sFactoryIDs != "")
                    //{
                    //    Global.TagSQL(ref sSQL);
                    //    sSQL = sSQL + "FabricSalesContractDetailID In (Select FabricSalesContractID from FabricSalesContract Where FabricSalesContractID<>0 ";

                    //    if (sBuyerIDs != "")
                    //    {
                    //        Global.TagSQL(ref sSQL);
                    //        sSQL = sSQL + "BuyerID In (" + sBuyerIDs + ")";
                    //    }
                    //    if (sFactoryIDs != "")
                    //    {
                    //        Global.TagSQL(ref sSQL);
                    //        sSQL = sSQL + "ContractorID In (" + sFactoryIDs + ")";
                    //    }
                    //    sSQL = sSQL + ")";
                    //}

                    return sSQL;         //final string
                }

                else if (!string.IsNullOrEmpty(oFNExecutionOrderStatus.FabricNo))
                {
                    if (oFNExecutionOrderStatus.FabricNo != "")
                    {

                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + "FabricSalesContractID in (" + sSQLSub + ")";
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " FabricNo like ''%" + oFNExecutionOrderStatus.FabricNo + "%''";
                    }
                    //string sSQLSub = "Select FabricSalesContractID from FabricSalesContract Where OrderType in (3,2,9,10,11,12)";
                    //Global.TagSQL(ref sSQLSub);
                    //sSQLSub = sSQLSub + "FabricNo like '%" + oFNExecutionOrderStatus.FabricNo +"% ' ))";
                    //string sString = "and FabricSalesContractID IN(Select FabricSalesContractID from FabricSalesContractDetail WHERE FabricID IN(Select FabricID from Fabric Where FabricNo LIKE '%" + oFNExecutionOrderStatus.FabricNo + "%'))";
                    oFNExecutionOrderStatuss = FNExecutionOrderStatus.GetsReport(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                }

                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                oFNExecutionOrderStatus = new FNExecutionOrderStatus();
                oFNExecutionOrderStatus.ErrorMessage = ex.Message;
                oFNExecutionOrderStatuss.Add(oFNExecutionOrderStatus);
            }
            return "";
        }
        [HttpPost]
        public JsonResult GetsStatusForFNExONo(FNExecutionOrderStatus oFNExecutionOrderStatus)
        {
            List<FNExecutionOrderStatus> oFNExecutionOrderStatuss = new List<FNExecutionOrderStatus>();
            try
            {
                string sString = MakeSQL(oFNExecutionOrderStatus);
                oFNExecutionOrderStatuss = FNExecutionOrderStatus.GetsReport(sString, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oFNExecutionOrderStatus = new FNExecutionOrderStatus();
                oFNExecutionOrderStatus.ErrorMessage = ex.Message;
                oFNExecutionOrderStatuss.Add(oFNExecutionOrderStatus);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNExecutionOrderStatuss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult ViewFNExecutionOrderProcess(int id)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            if (id > 0)
            {
                oFabricSalesContractDetail = oFabricSalesContractDetail.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                ViewBag.FNExecutionOrderProcessList = FNExecutionOrderProcess.Gets(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return View(oFabricSalesContractDetail);
        }
        [HttpPost]
        public JsonResult DeleteFNEOProcess(FNExecutionOrderProcess oFNExecutionOrderProcess)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFNExecutionOrderProcess.Delete(oFNExecutionOrderProcess.FNExOProcessID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpDown(FNExecutionOrderProcess oFNExecutionOrderProcess)
        {
            FNExecutionOrderProcess oAH = new FNExecutionOrderProcess();
            List<FNExecutionOrderProcess> oFNExecutionOrderProcesss = new List<FNExecutionOrderProcess>();
            try
            {
                oAH = oAH.UpDown(oFNExecutionOrderProcess, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNExecutionOrderProcess = new FNExecutionOrderProcess();
                oFNExecutionOrderProcess.ErrorMessage = ex.Message;
            }
            if (oAH.ErrorMessage == "")
            {
                try
                {
                    oFNExecutionOrderProcesss = FNExecutionOrderProcess.Gets(oFNExecutionOrderProcess.FNExOID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                catch (Exception ex)
                {
                    oFNExecutionOrderProcess = new FNExecutionOrderProcess();
                    oFNExecutionOrderProcess.ErrorMessage = ex.Message;
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNExecutionOrderProcesss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RefreshMenuSequence(List<FNExecutionOrderProcess> oFNExecutionOrderProcesss)
        {
            try
            {
                oFNExecutionOrderProcesss = FNExecutionOrderProcess.RefreshSequence(oFNExecutionOrderProcesss, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                FNExecutionOrderProcess oFNExecutionOrderP = new FNExecutionOrderProcess();
                oFNExecutionOrderP.ErrorMessage = ex.Message;
                oFNExecutionOrderProcesss.Add(oFNExecutionOrderP);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNExecutionOrderProcesss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFNExODetailsForRequisition(FabricSCReport oFabricSCReport)
        {
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            string sSQL = "", sTempSql = "";
            try
            {
                sSQL = "SELECT top(50)* FROM View_FabricSalesContractReport";
                sTempSql = " where ScNoFull LIKE '%" + oFabricSCReport.SCNoFull + "%'";
                sSQL = sSQL + sTempSql + " AND ExeNo !='' ORDER BY FabricSalesContractDetailID DESC";
                oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFabricSCReport = new FabricSCReport();
                oFabricSCReport.ErrorMessage = ex.Message;
                oFabricSCReports.Add(oFabricSCReport);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetsFNExODetailsForRequisitionFromDispo(FabricSCReport oFabricSCReport)
        {
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            string sSQL = "", sTempSql = "";
            try
            {
                sSQL = "SELECT top(50)* FROM View_FabricSalesContractReport";
                sTempSql = " where ExeNo LIKE '%" + oFabricSCReport.ExeNo + "%'";
                sSQL = sSQL + sTempSql + " AND ExeNo !='' ORDER BY FabricSalesContractDetailID DESC";
                oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFabricSCReport = new FabricSCReport();
                oFabricSCReport.ErrorMessage = ex.Message;
                oFabricSCReports.Add(oFabricSCReport);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadFromDispo(FabricSCReport oFabricSCReport)
        {

            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            FNRecipe oFNRecipe = new FNRecipe();
            try
            {
                if(!string.IsNullOrEmpty(oFabricSCReport.ErrorMessage))
                {
                    int FabricSalesContractDetailID = Convert.ToInt32(oFabricSCReport.ErrorMessage.Split('~')[0]);
                    int TreatmentID = Convert.ToInt32(oFabricSCReport.ErrorMessage.Split('~')[1]);
                    oFNRecipes = FNRecipe.Gets("SELECT * FROM View_FNRecipe WHERE FNTreatment = '" + TreatmentID + "' AND FSCDID = " + FabricSalesContractDetailID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNRecipe = new FNRecipe();
                oFNRecipe.ErrorMessage = ex.Message;
                oFNRecipes.Add(oFNRecipe);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult SaveFNEOProcess(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
            FabricSalesContractDetail _oFabricSalesContractDetail = new FabricSalesContractDetail();
            try
            {
                _oFabricSalesContractDetail = _oFabricSalesContractDetail.SaveProcess(oFabricSalesContractDetail, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricSalesContractDetail = new FabricSalesContractDetail();
                _oFabricSalesContractDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsTreatmentProcess(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            List<FNTreatmentProcess> oFNTreatmentProcessList = new List<FNTreatmentProcess>();
            try
            {
                string sSQL = "SELECT  * FROM FNTreatmentProcess WHERE FNTPID NOT IN (SELECT FNTPID FROM FNExecutionOrderProcess WHERE FNExOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + ") ORDER BY FNTreatment";
                oFNTreatmentProcessList = FNTreatmentProcess.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
                oFNTreatmentProcess.ErrorMessage = ex.Message;
                oFNTreatmentProcessList.Add(oFNTreatmentProcess);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNTreatmentProcessList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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
        //public ActionResult PrintProcessPDF(int id)
        //{
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    string sSQL="";
        //    List<FNExecutionOrderProcess> oFNExecutionOrderProcessList = new List<FNExecutionOrderProcess>();
        //    FabricSCReport oFabricSCReport = new FabricSCReport();

        //    FabricExecutionOrderSpecification oFEOES = new FabricExecutionOrderSpecification();
        //    List<FabricExecutionOrderSpecification> oFEOESs = new List<FabricExecutionOrderSpecification>();
        //    FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
        //    List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
        //    List<FNBatchCard> oFNBatchCardsFinal = new List<FNBatchCard>();
        //    List<FNOrderFabricReceive> oFNExeOFRs = new List<FNOrderFabricReceive>();
        //    FNBatch oFNBatch = new FNBatch();

        //    try
        //    {
        //        if (id > 0)
        //        {
        //            oFabricSCReport = oFabricSCReport.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //            oFEOESs = FabricExecutionOrderSpecification.Gets("Select * from View_FabricExecutionOrderSpecification Where FSCDID= " + id + " Order By FEOSID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if (oFEOESs.Count > 0)
        //            {
        //                oFEOES = oFEOESs[0];
        //            }
        //            if (oFNExeOFRs.Count > 0)
        //            {
        //                oFEOES.FabricSource = string.Join(",", oFNExeOFRs.Select(x => x.FabricSource).Distinct());
        //            }
                    
        //        }
        //    }
        //    catch
        //    {
        //        oFabricSCReport = new FabricSCReport();
        //        oFNExecutionOrderProcessList = new List<FNExecutionOrderProcess>();
        //    }
        //    List<FNRecipe> oFNRecipes = new List<FNRecipe>();
        //    oFNRecipes = FNRecipe.Gets("select * from View_FNRecipe where FSCDID = '" + id + "' AND IsProcess = 1 Order by FNTreatment, FNProcess", (int)Session[SessionInfo.currentUserID]);

        //    rptPrintProcess orptPrintProcess = new rptPrintProcess();
        //    byte[] abytes = orptPrintProcess.PrepareReport(oCompany, oFabricSCReport, oFNExecutionOrderProcessList, oFNBatch, oFEOES, oFNBatchCardsFinal, oFNRecipes);
        //    return File(abytes, "application/pdf");
        //}
        public ActionResult PrintProcessPDF(int id, int FNBatchID)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string sSQL = "";
            string sName = "";
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
            FnOrderExecute oFEOES = new FnOrderExecute();
            List<FnOrderExecute> oFEOESs = new List<FnOrderExecute>();
            FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            List<FNBatchCard> oFNBatchCardsFinal = new List<FNBatchCard>();
            List<FNOrderFabricReceive> oFNExeOFRs = new List<FNOrderFabricReceive>();
            FNBatch oFNBatch = new FNBatch();
         
            try
            {
                if (id > 0)
                {

                    if (FNBatchID > 0)
                    {
                        oFNBatch = FNBatch.Get(FNBatchID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        id = oFNBatch.FNExOID;
                         oFabricSCReport = oFabricSCReport.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                         oFNBatchCardsFinal = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE  FNBatchID = " + FNBatchID + " order by Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);

                         sSQL = "Select * from View_FNOrderFabricReceive Where FSCDID=" + id + " and LotID in (Select LotID from FNBatchRawMaterial where FNBatchID=" + FNBatchID + ")";
                        oFNExeOFRs = FNOrderFabricReceive.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    }
                    else
                    {
                        oFabricSCReport = oFabricSCReport.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        oFNRecipes = FNRecipe.Gets("select * from View_FNRecipe where FSCDID = '" + id + "' AND IsProcess = 1 Order by  Code", (int)Session[SessionInfo.currentUserID]);
                        oFNRecipes = oFNRecipes.OrderBy(x => x.Code).ToList();
                        FNBatchCard oFNBatchCard = new FNBatchCard();

                        sSQL = "Select * from View_FNOrderFabricReceive Where FSCDID=" + id + "";
                        oFNExeOFRs = FNOrderFabricReceive.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                        foreach (FNRecipe oItem in oFNRecipes)
                        {
                            oFNBatchCard = new FNBatchCard();
                            oFNBatchCard.FNBatchCardID = 0;
                            oFNBatchCard.FNBatchID = FNBatchID;
                            oFNBatchCard.FNProcess = oItem.FNProcess;
                            oFNBatchCard.PlannedDate = DateTime.MinValue;
                            oFNBatchCard.Code = oItem.Code;
                            oFNBatchCard.PrepareByName = oItem.PrepareByName;
                            oFNBatchCardsFinal.Add(oFNBatchCard);
                        }
                    }
                    if (oFabricSCReport.FNLabdipDetailID > 0)
                    {
                        oFNLabDipDetail = FNLabDipDetail.Get(oFabricSCReport.FNLabdipDetailID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        oFabricSCReport.LDNo = oFNLabDipDetail.LDNo;
                    }

                    oFEOESs = FnOrderExecute.Gets("Select * from view_FnOrderExecute Where FSCDID= " + id + " Order By FSCDID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFEOESs.Count > 0)
                    {
                        oFEOES = oFEOESs[0];
                        //if (FNBatchID <= 0)
                        //{
                        //    oFEOES.Qty = oFEOES.GreigeDemand;       //to show req qty from FNRecipe Lab
                        //}
                        sName = oFEOES.PrepareByName;
                    }
                    if (oFNExeOFRs.Count > 0)
                    {
                        oFEOES.FabricSource = string.Join(",", oFNExeOFRs.Select(x => x.FabricSource).Distinct());
                    }

                }
            }
            catch
            {
                oFabricSCReport = new FabricSCReport();
            }
            
            if(FNBatchID>0)
            {
                sName = oFNBatch.PrepareByName;
            }
            rptPrintProcess orptPrintProcess = new rptPrintProcess();
            byte[] abytes = orptPrintProcess.PrepareReport(oCompany, oFabricSCReport, oFNBatch, oFEOES, oFNBatchCardsFinal, sName);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintProcessPDFForShadeWise(int id, int nShadeID, int nFSCDID)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string sSQL = "";
            string sName = "";
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
            FnOrderExecute oFEOES = new FnOrderExecute();
            List<FnOrderExecute> oFEOESs = new List<FnOrderExecute>();
            FNExecutionOrderProcess oFNExecutionOrderProcess = new FNExecutionOrderProcess();
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            List<FNBatchCard> oFNBatchCardsFinal = new List<FNBatchCard>();
            List<FNOrderFabricReceive> oFNExeOFRs = new List<FNOrderFabricReceive>();
            FNBatch oFNBatch = new FNBatch();
            try
            {
                if (id > 0)
                {
                    oFabricSCReport = oFabricSCReport.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    sSQL = "select * from View_FNRecipe where FSCDID = '" + id + "' AND IsProcess = 1 AND ShadeID = "+nShadeID+" Order by  Code";
                    oFNRecipes = FNRecipe.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    //oFNRecipes = oFNRecipes.OrderBy(x => x.Code).ToList();
                    FNBatchCard oFNBatchCard = new FNBatchCard();
                    sSQL = "Select * from View_FNOrderFabricReceive Where FSCDID=" + id + "";
                    oFNExeOFRs = FNOrderFabricReceive.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    foreach (FNRecipe oItem in oFNRecipes)
                    {
                        oFNBatchCard = new FNBatchCard();
                        oFNBatchCard.FNBatchCardID = 0;
                        oFNBatchCard.FNBatchID = nFSCDID;
                        oFNBatchCard.FNProcess = oItem.FNProcess;
                        oFNBatchCard.PlannedDate = DateTime.MinValue;
                        oFNBatchCard.Code = oItem.Code;
                        oFNBatchCard.PrepareByName = oItem.PrepareByName;
                        oFNBatchCardsFinal.Add(oFNBatchCard);
                    }
                    if (oFabricSCReport.FNLabdipDetailID > 0)
                    {
                        oFNLabDipDetail = FNLabDipDetail.Get(oFabricSCReport.FNLabdipDetailID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        oFabricSCReport.LDNo = oFNLabDipDetail.LDNo;
                    }
                    oFEOESs = FnOrderExecute.Gets("Select * from view_FnOrderExecute Where FSCDID= " + id + " Order By FSCDID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFEOESs.Count > 0)
                    {
                        oFEOES = oFEOESs[0];
                        sName = oFEOES.PrepareByName;
                    }
                    if (oFNExeOFRs.Count > 0)
                    {
                        oFEOES.FabricSource = string.Join(",", oFNExeOFRs.Select(x => x.FabricSource).Distinct());
                    }
                }
            }
            catch
            {
                oFabricSCReport = new FabricSCReport();
            }
            rptPrintProcess orptPrintProcess = new rptPrintProcess();
            byte[] abytes = orptPrintProcess.PrepareReport(oCompany, oFabricSCReport, oFNBatch, oFEOES, oFNBatchCardsFinal, sName);
            return File(abytes, "application/pdf");
        }
        public JsonResult GetBatchProcess(FNBatchCard objFNBatchCard)
        {
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            FabricExecutionOrderSpecification oFEOES = new FabricExecutionOrderSpecification();
            List<FabricExecutionOrderSpecification> oFEOESs = new List<FabricExecutionOrderSpecification>();
            FNBatch oFNBatch = new FNBatch();
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            //int id = (objFNBatchCard.FNBatchCardID);
            FNBatchCard oFNBatchCard = new FNBatchCard();
            string sFNTPIDs = "";
            try
            {

                if (objFNBatchCard.FNBatchID > 0)
                {
                    oFNBatch = FNBatch.Get(objFNBatchCard.FNBatchID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    oFNBatchCards = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE FNBatchID = " + objFNBatchCard.FNBatchID + " order by Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                if (oFNBatch.FNExOID > 0)
                {
                    oFabricSCReport = oFabricSCReport.Get(oFNBatch.FNExOID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    sFNTPIDs = string.Join(",", oFNBatchCards.Select(x => x.FNTreatmentProcessID).Distinct());
                    if (string.IsNullOrEmpty(sFNTPIDs)) { sFNTPIDs = "0"; }
                    oFNRecipes = FNRecipe.Gets("select * from View_FNRecipe where FNTPID not in (" + sFNTPIDs + ") and FSCDID = " + oFNBatch.FNExOID + " AND IsProcess = 'true' ORDER BY  Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    int nCount = 1;
                    foreach (FNRecipe oItem in oFNRecipes)
                    {
                        oFNBatchCard = new FNBatchCard();
                        oFNBatchCard.FNBatchCardID = 0;
                        oFNBatchCard.FNBatchID = objFNBatchCard.FNBatchID;
                        oFNBatchCard.FNTreatmentProcessID = oItem.FNTPID;
                        oFNBatchCard.FNProcess = oItem.FNProcess;
                        oFNBatchCard.Code = oItem.Code;
                        oFNBatchCard.FNTreatment = (EnumFNTreatment)oItem.FNTreatment;
                        oFNBatchCard.FNTreatmentSt = oItem.FNTreatmentSt;
                        //oFNBatchCard.SequenceNo = nCount++;
                        oFNBatchCards.Add(oFNBatchCard);
                    }
                }
                int nSequenceNo= 1;
                foreach (FNBatchCard oItem in oFNBatchCards)
                {
                    oItem.SequenceNo = nSequenceNo++;
                }
                //oFNBatchCards = oFNBatchCards.OrderBy(x => x.Code).ToList();
            }
            catch
            {
                oFabricSCReport = new FabricSCReport();
                oFNBatchCards = new List<FNBatchCard>();
            }
           
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchCards);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFNBatchCardByNo(FNBatch objFNBatch)
        {
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            FNBatchCard oFNBatchCard = new FNBatchCard();
            try
            {
                if (objFNBatch.FNBatchID > 0)
                {
                    oFNBatchCards = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE FNBatchID = " + objFNBatch.FNBatchID + " AND ISNULL(FNTreatment,0) = 1 AND FNBatchID IN (SELECT FNBatchID FROM FNProductionBatch)", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }            
            }
            catch
            {
                oFNBatchCard = new FNBatchCard();
                oFNBatchCards = new List<FNBatchCard>();
                oFNBatchCards.Add(oFNBatchCard);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchCards);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFNBatches(FNBatch objFNBatch)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            FNBatch oFNBatch = new FNBatch();
            try
            {
                if (!string.IsNullOrEmpty(objFNBatch.BatchNo))
                {
                    oFNBatchs = FNBatch.Gets("SELECT * FROM View_FNBatch WHERE BatchNo LIKE '%" + objFNBatch.BatchNo + "%' AND FNBatchID IN (SELECT FNBatchID FROM FNProductionBatch WHERE FNProductionID IN (SELECT FNProductionID FROM View_FNProduction WHERE FNTreatment = 1)) ", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch
            {
                oFNBatch = new FNBatch();
                oFNBatchs = new List<FNBatch>();
                oFNBatchs.Add(oFNBatch);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePlannedDate(List<FNBatchCard> oFNBatchCards)
        {
            FNBatchCard objFNBatchCard = new FNBatchCard();
            string sIDs = "";
            if (oFNBatchCards.Count > 0)
            {
                foreach (FNBatchCard oItem in oFNBatchCards)
                {
                    objFNBatchCard = oItem;
                    objFNBatchCard = objFNBatchCard.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            else
            {
                objFNBatchCard.ErrorMessage = "Save Failed";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objFNBatchCard);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFNBatchCards(FNBatchCard oFNBatchCard)
        {
            List<FNBatchCard> _oFNBatchCards = new List<FNBatchCard>();
            FNBatchCard objFNBatchCard = new FNBatchCard();
            if (oFNBatchCard.oFNBatchCards.Count > 0)
            {
                _oFNBatchCards = objFNBatchCard.SaveFNBatchCards(oFNBatchCard, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                objFNBatchCard.ErrorMessage = "Save Failed";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objFNBatchCard);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransferFNBatchCard(FNBatch oFNBatch)
        {
            FNBatch _oFNBatch = new FNBatch();
            try
            {
                _oFNBatch = oFNBatch.TransferFNBatchCard(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oFNBatch = new FNBatch();
                _oFNBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFNBatchTransferHistory(FNBatch oFNBatch)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            FNBatch _oFNBatch = new FNBatch();
            try
            {
                oFNBatchs = FNBatch.Gets("SELECT TOP 1 * FROM [View_FNBatchTransferHistory] WHERE DestinationBatchID = " + oFNBatch.FNBatchID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oFNBatchs.Count > 0)
                    _oFNBatch = oFNBatchs[0];
            }
            catch (Exception ex)
            {
                _oFNBatch = new FNBatch();
                _oFNBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
