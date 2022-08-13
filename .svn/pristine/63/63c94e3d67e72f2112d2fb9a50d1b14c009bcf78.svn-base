using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;

namespace ESimSolFinancial.Controllers
{
    public class AccountsBookController : Controller
    {
        #region Declaration
        List<AccountsBook> _oAccountsBooks = new List<AccountsBook>();
        AccountsBook _oAccountsBook = new AccountsBook();
        #endregion

        public ActionResult ViewAccountsBooks(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAccountsBook = new AccountsBook();
            _oAccountsBook.AccountsBookSetups = AccountsBookSetup.Gets((int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oAccountsBook);
        }

        [HttpPost]
        public JsonResult Refresh(AccountsBook oAccountsBook)
        {
            _oAccountsBooks = AccountsBook.Gets(oAccountsBook.AccountsBookSetupID, oAccountsBook.StartDate, oAccountsBook.EndDate, oAccountsBook.BUIDs, oAccountsBook.IsApproved, (int)Session[SessionInfo.currentUserID]);
            double nTotalOpenningBalance = 0;
            double nTotalDebit = 0;
            double nTotalCredit = 0;
            double nTotalClosingBalance = 0;
            foreach (AccountsBook oItem in _oAccountsBooks)
            {
                nTotalOpenningBalance = nTotalOpenningBalance + oItem.OpenningBalance;
                nTotalDebit = nTotalDebit + oItem.DebitAmount;
                nTotalCredit = nTotalCredit + oItem.CreditAmount;
                nTotalClosingBalance = nTotalClosingBalance + oItem.ClosingBalance;
            }
            AccountsBook oAB = new AccountsBook();
            oAB.AccountHeadName = "Total :";
            oAB.OpenningBalance = nTotalOpenningBalance;
            oAB.DebitAmount = nTotalDebit;
            oAB.CreditAmount = nTotalCredit;
            oAB.ClosingBalance = nTotalClosingBalance;

            _oAccountsBooks.Add(oAB);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oAccountsBooks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintAccountsBook(string sParams)
        {            
            int nAccountsBookSetupID = Convert.ToInt32(sParams.Split('~')[0]);
            string sAccountsBookSetupName = Convert.ToString(sParams.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);
            string BUIDs =sParams.Split('~')[4];
            bool bIsApproved = Convert.ToBoolean(sParams.Split('~')[5]);
            _oAccountsBooks = AccountsBook.Gets(nAccountsBookSetupID, dStartDate, dEndDate, BUIDs, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            _oAccountsBook.AccountsBookSetupName = sAccountsBookSetupName;
            _oAccountsBook.AccountsBooks = _oAccountsBooks;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (!string.IsNullOrEmpty(BUIDs) && BUIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BUIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }
            
            _oAccountsBook.Company = oCompany;

            rptAccountsBook orptAccountsBooks = new rptAccountsBook();
            byte[] abytes = orptAccountsBooks.PrepareReport(_oAccountsBook);
            return File(abytes, "application/pdf");
        }

        public void PrintAccountsBookInXL(string sParams)
        {
            int nAccountsBookSetupID = Convert.ToInt32(sParams.Split('~')[0]);
            string sAccountsBookSetupName = Convert.ToString(sParams.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);
            string BUIDs = sParams.Split('~')[4];
            bool bIsApproved = Convert.ToBoolean(sParams.Split('~')[5]);
            _oAccountsBooks = AccountsBook.Gets(nAccountsBookSetupID, dStartDate, dEndDate, BUIDs, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (!string.IsNullOrEmpty(BUIDs) && BUIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BUIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            
            #region Print XL
            int nRowIndex = 2, nStartCol = 2, nEndCol = 7, nColumn = 1, nCount = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add(sAccountsBookSetupName.Replace(" ", ""));
                sheet.Name = sAccountsBookSetupName;
                sheet.Column(++nColumn).Width = 30; //Sub Group
                sheet.Column(++nColumn).Width = 50; //Account Head Name
                sheet.Column(++nColumn).Width = 20; //Opening Balance
                sheet.Column(++nColumn).Width = 20; //Debit Amount
                sheet.Column(++nColumn).Width = 20; //Credit Amount
                sheet.Column(++nColumn).Width = 20; //Closing Balance
                
                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Name
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sAccountsBookSetupName + " @ " + dStartDate.ToString("dd MMM yyyy") + "--to--" + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true;  cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion


                #region Report Data
                #region Column Header
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Sub Group Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Account Head Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Opening Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Debit Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Credit Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Data                
                double nTotalOpenningBalance = 0, nTotalDebitAmount = 0, nTotalCreditAmount = 0, nTotalClosingBalance = 0, nSubTotalOpeningBalance = 0,nSubTotalDebit = 0,nSubTotalCredit = 0,nSubTotalClosingBalance = 0, nParentHeadID = 0;
                foreach (AccountsBook oItem in _oAccountsBooks)
                {
                    nColumn = 1;
                    nCount++;
                    if (nParentHeadID != oItem.ParentHeadID)
                    {

                        #region Sub Total Print
                        if (nSubTotalOpeningBalance > 0 || nSubTotalDebit > 0 || nSubTotalCredit > 0 || nSubTotalClosingBalance > 0)
                        {
                            //nRowIndex++;
                            nColumn = 1;
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Sub Total:";  cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalOpeningBalance;  cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalDebit;  cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalCredit;  cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalClosingBalance;  cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                            nRowIndex++;

                            nSubTotalOpeningBalance = 0;
                            nSubTotalDebit = 0;
                            nSubTotalCredit = 0;
                            nSubTotalClosingBalance = 0;
                            
                        }
                        #endregion

                        #region Blank space
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion

                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ParentHeadName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;
                        nParentHeadID = oItem.ParentHeadID;
                        nCount = 1;
                    }
                    else
                    {

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    }

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =nCount.ToString()+".  "+oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;                    
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OpenningBalance; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ClosingBalance; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;border.Bottom.Style =  ExcelBorderStyle.Thin;                                       
                    nRowIndex++;

                    nTotalOpenningBalance = nTotalOpenningBalance + oItem.OpenningBalance; nSubTotalOpeningBalance = nSubTotalOpeningBalance + oItem.OpenningBalance;
                    nTotalDebitAmount = nTotalDebitAmount + oItem.DebitAmount; nSubTotalDebit = nSubTotalDebit + oItem.DebitAmount;
                    nTotalCreditAmount = nTotalCreditAmount + oItem.CreditAmount; nSubTotalCredit = nSubTotalCredit + oItem.CreditAmount;
                    nTotalClosingBalance = nTotalClosingBalance + oItem.ClosingBalance; nSubTotalClosingBalance = nSubTotalClosingBalance + oItem.ClosingBalance;


                }
                #endregion

                #region Sub Total Print
                if (nSubTotalOpeningBalance > 0 || nSubTotalDebit > 0 || nSubTotalCredit > 0 || nSubTotalClosingBalance > 0)
                {
                    //nRowIndex++;
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "";  cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Sub Total:";  cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalOpeningBalance;  cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalDebit;  cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalCredit;  cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nSubTotalClosingBalance;  cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nRowIndex++;

                    nSubTotalOpeningBalance = 0;
                    nSubTotalDebit = 0;
                    nSubTotalCredit = 0;
                    nSubTotalClosingBalance = 0;

                }
                #endregion

                #region Grand Total
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "";  cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Grand Total:";  cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalOpenningBalance;  cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalDebitAmount;  cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalCreditAmount;  cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalClosingBalance;  cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border;  border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=" + sAccountsBookSetupName.Replace(" ", "") + ".xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


           
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
    }
}
