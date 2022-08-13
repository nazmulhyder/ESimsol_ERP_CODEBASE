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
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class FabricDeliveryChallanBillController : Controller
    {
        #region Declaration
        FabricDeliveryChallanBill _oFabricDeliveryChallanBill = new FabricDeliveryChallanBill();
        List<FabricDeliveryChallanBill> _oFabricDeliveryChallanBills = new List<FabricDeliveryChallanBill>();
        string sFeedBackMessage = "";
        #endregion

        public ActionResult ViewFabricDeliveryChallanBills(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FabricDeliveryChallanBill> oFabricDeliveryChallanBills = new List<FabricDeliveryChallanBill>();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            //ViewBag.MktPersons = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeType.MarketPerson, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oFabricDeliveryChallanBills);
        }
        [HttpPost]
        public JsonResult AdvSearch(FabricDeliveryChallanBill oFabricDeliveryChallanBill)
        {
            List<FabricDeliveryChallanBill> oFabricDeliveryChallanBills = new List<FabricDeliveryChallanBill>();
            try
            {
                string sSQL = MakeSQL(oFabricDeliveryChallanBill.Params);
                oFabricDeliveryChallanBills = FabricDeliveryChallanBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricDeliveryChallanBill.ErrorMessage = ex.Message;
                oFabricDeliveryChallanBills.Add(oFabricDeliveryChallanBill);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryChallanBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(string sString)
        {
            int nCount = 0;
            int DateRange = 0;
            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;
            string BuyerName = "";
            bool YetToReceived = false;
            bool YetToDelivery = false;
            string DispoNo = "";
            string PONo = "";
            string sChallaNo = "";
            string sFDONo = "";

            if (!String.IsNullOrEmpty(sString))
            {
                BuyerName = Convert.ToString(sString.Split('~')[nCount++]);
                DateRange = Convert.ToInt32(sString.Split('~')[nCount++]);
                StartDate = Convert.ToDateTime(sString.Split('~')[nCount++]);
                EndDate = Convert.ToDateTime(sString.Split('~')[nCount++]);
                YetToReceived = Convert.ToBoolean(sString.Split('~')[nCount++]);
                YetToDelivery = Convert.ToBoolean(sString.Split('~')[nCount++]);
                try
                {
                    DispoNo = string.Empty;
                    if (sString.Split('~').Length > nCount)
                        DispoNo = sString.Split('~')[nCount++];
                    PONo = string.Empty;
                    if (sString.Split('~').Length > nCount)
                        PONo = sString.Split('~')[nCount++];
                    sChallaNo = string.Empty;
                    if (sString.Split('~').Length > nCount)
                        sChallaNo = sString.Split('~')[nCount++];
                    sFDONo = string.Empty;
                    if (sString.Split('~').Length > nCount)
                        sFDONo = sString.Split('~')[nCount++];

                }
                catch
                {

                }
            }
            string sReturn1 = "";
            string sReturn2 = "";
            string sReturn = " ";

            #region Order Date Search
            if (DateRange > 0)
            {
                if (DateRange == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn2);
                    sReturn2 = sReturn2 + " SCDate = '" + StartDate.ToString("dd MMM yyyy") + "'";
                }
                if (DateRange == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn2);
                    sReturn2 = sReturn2 + " (SCDate>= '" + StartDate.ToString("dd MMM yyyy") + "' AND SCDate < '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (DateRange == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn2);
                    sReturn2 = sReturn2 + " SCDate > '" + StartDate.ToString("dd MMM yyyy") + "'";
                }
                if (DateRange == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn2);
                    sReturn2 = sReturn2 + " SCDate < '" + StartDate.ToString("dd MMM yyyy") + "'";
                }
                if (DateRange == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn2);
                    sReturn2 = sReturn2 + " (SCDate>= '" + StartDate.ToString("dd MMM yyyy") + "' AND SCDate < '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (DateRange == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn2);
                    sReturn2 = sReturn2 + " (SCDate< '" + StartDate.ToString("dd MMM yyyy") + "' OR SCDate > '" + EndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Buyer Name
            if (!string.IsNullOrEmpty(BuyerName))
            {
                Global.TagSQL(ref sReturn2);
                sReturn2 = sReturn2 + " BuyerID IN (" + BuyerName + ")";
            }
            #endregion

            #region PONo
            if (!string.IsNullOrEmpty(PONo))
            {
                Global.TagSQL(ref sReturn2);
                sReturn2 = sReturn2 + "SCNo Like '%" + PONo + "%'";
            }
            #endregion

            #region Order Type
            Global.TagSQL(ref sReturn2);
            sReturn2 = sReturn2 + "OrderType in (" + (int)EnumFabricRequestType.Local_Bulk + "," + (int)EnumFabricRequestType.Local_Sample + ")";
            #endregion

            if (!string.IsNullOrEmpty(sReturn2))
            {
                sReturn1 = " FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract " + sReturn2 +")";
            }


            #region YetToReceived
            if (YetToReceived)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  FabricSalesContractDetailID IN (SELECT FSCDID FROM View_FabricDeliveryChallanDetail AS QQ WHERE (QQ.Qty_FSCD * QQ.UnitPrice - QQ.Qty * QQ.UnitPrice) > 0)";
            }
            #endregion

            #region YetToDelivery
            if (YetToDelivery)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricSalesContractDetailID IN (SELECT FSCDID FROM View_FabricDeliveryChallanDetail AS QQ WHERE (QQ.Qty_FSCD - QQ.Qty) > 0)";
            }
            #endregion

            #region DispoNo
            if (!string.IsNullOrEmpty(DispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExeNo Like '%" + DispoNo + "%'";
            }
            #endregion

          

            #region Challan No
            if (!string.IsNullOrEmpty(sChallaNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricSalesContractDetailID in ( Select FDCD.FSCDID from FabricDeliveryChallanDetail as FDCD where FDCID in ( Select FDC.FDCID from FabricDeliveryChallan as FDC where ChallanNo like '%" + sChallaNo + "%'))";
            }
            #endregion

            #region Fabric DO No
            if (!string.IsNullOrEmpty(sFDONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricSalesContractDetailID in ( Select FDOD.FEOID from FabricDeliveryOrderDetail as FDOD where FDOD.FDOID in ( Select FDO.FDOID from FabricDeliveryOrder as FDO where DONo like '%" + sFDONo + "%'))";
            }
            #endregion




            return sReturn1 + sReturn;
        }
        public void ExportToExcel(string sParams, int nReportType)
        {
            List<FabricDeliveryChallanBill> oFabricDeliveryChallanBills = new List<FabricDeliveryChallanBill>();
            string sString = MakeSQL(sParams);
            oFabricDeliveryChallanBills = FabricDeliveryChallanBill.Gets(sString, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "PO Date", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Order Type", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 20f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Buying House", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Construction", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Issue To", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Marketing Person Name", Width = 35f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "QTY", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "U/P" + "(" + oCompany.CurrencySymbol + ")", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Amount" + "(" + oCompany.CurrencySymbol + ")", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Discount", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Qty(DC)", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Total(DC)" + "(" + oCompany.CurrencySymbol + ")", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Yet To Delivery" + "(" + oCompany.CurrencySymbol + ")", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Recevied Amount" + "(" + oCompany.CurrencySymbol + ")", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Yet To Received" + "(" + oCompany.CurrencySymbol + ")", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Bill Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Bill Report");
                sheet.Name = "Bill Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 18; //23
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Bill Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                int nSL = 1;
                nEndCol = table_header.Count() + nStartCol;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                FabricDeliveryChallanBill oFabricDeliveryChallanBill = new FabricDeliveryChallanBill();
                foreach (var obj in oFabricDeliveryChallanBills)
                {
                    nStartCol = 2;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.OrderName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.MKTPersonName, false, ExcelHorizontalAlignment.Left, false, false);

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QTY, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.UnitPrice, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Amount, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DiscountAmount, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_DC, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Total_DC, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YetToDelivery, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.PaymentAmount, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YetToReceived, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    
                    nSL++;
                    nRowIndex++;
                }
                #region total
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                nStartCol = 12;

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x=>x.QTY), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x => x.UnitPrice), 2).ToString() +"(" + oCompany.CurrencySymbol + ")", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x => x.Amount), 2).ToString() + "(" + oCompany.CurrencySymbol + ")", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x => x.DiscountAmount), 2).ToString() + "(" + oCompany.CurrencySymbol + ")", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x=>x.Qty_DC), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x => x.Total_DC), 2).ToString() + "(" + oCompany.CurrencySymbol + ")", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x => x.YetToDelivery), 2).ToString() + "(" + oCompany.CurrencySymbol + ")", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x => x.PaymentAmount), 2).ToString() + "(" + oCompany.CurrencySymbol + ")", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oFabricDeliveryChallanBills.Sum(x => x.YetToReceived), 2).ToString() + "(" + oCompany.CurrencySymbol + ")", false, ExcelHorizontalAlignment.Right, false, false);

                #endregion
                #endregion
                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Bill Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public ActionResult PrintOrderStatement(int FSCID, int FSCDID, int BUID)
        {
          
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            oFabricSalesContract = oFabricSalesContract.Get(FSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            oFabricSalesContractDetails = FabricSalesContractDetail.Gets("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractID =" + FSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sFSCDID = string.Join(",", oFabricSalesContractDetails.Select(x => x.FabricSalesContractDetailID).ToList());

            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            oFDCRegisters = FDCRegister.Gets_FDC(" WHERE  FDCID IN (SELECT FDCID FROM FabricDeliveryChallanDetail WHERE FSCDID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail where FabricSalesContractID  IN ("+ FSCID +")))", ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Payment> oPayments = new List<Payment>();
            oPayments = Payment.Gets("Select * from Payment where PaymentID in (Select PaymentID from PaymentDetail where ReferenceType=" + (int)EnumSampleInvoiceType.Fabric_PO + " and ReferenceID in (Select SampleInvoice.SampleInvoiceID from SampleInvoice where ReferenceType=" + (int)EnumSampleInvoiceType.Fabric_PO + " and ReferenceID in (" + FSCID + ")))", ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sPaymentIDs = string.Join(",", oPayments.Select(x => x.PaymentID).ToList());
            List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
            if (!string.IsNullOrEmpty(sPaymentIDs))
            {
                oPaymentDetails = PaymentDetail.Gets("SELECT * FROM View_PaymentDetail where PaymentID IN (" + sPaymentIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricDeliveryChallan + " AND BUID = " + BUID + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptBillReport oReport = new rptBillReport();
            byte[] abytes = oReport.PrepareReport(oFabricSalesContract, oFabricSalesContractDetails, oFDCRegisters, oPayments, oPaymentDetails, oCompany, oBusinessUnit, oApprovalHeads);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(Company oCompany)
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
        
        
    }
}