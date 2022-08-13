using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class PFEmployerTransactionController : Controller
    {
        #region Declaration
        private PFEmployerTransaction _oPFET = new PFEmployerTransaction();
        private List<PFEmployerTransaction> _oPFETs = new List<PFEmployerTransaction>();
        private List<PFDistribution> _oPFDs = new List<PFDistribution>();
        #endregion

        #region Action Result View

        public ActionResult ViewPFEmployerTransactions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPFETs = new List<PFEmployerTransaction>();

            string sSQL = "Select * from View_PFEmployerTransaction Where PETID<>0";
            _oPFETs = PFEmployerTransaction.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.PETTypes = Enum.GetValues(typeof(EnumPETType)).Cast<EnumPETType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(_oPFETs);
        }
        #endregion

        #region PF Scheme
        [HttpPost]
        public JsonResult SavePFET(PFEmployerTransaction oPFET)
        {
            try
            {
                if (oPFET.PETID <= 0)
                {
                    oPFET = oPFET.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oPFET = oPFET.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oPFET = new PFEmployerTransaction();
                oPFET.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFET);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePFET(PFEmployerTransaction oPFET)
        {
            try
            {
                if (oPFET.PETID <= 0) { throw new Exception("Please select an valid item."); }

                oPFET = oPFET.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFET = new PFEmployerTransaction();
                oPFET.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFET.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovePFET(PFEmployerTransaction oPFET)
        {
            try
            {
                if (oPFET.PETID <= 0) { throw new Exception("Please select an valid item."); }
                oPFET = oPFET.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFET = new PFEmployerTransaction();
                oPFET.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFET);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Search PFEmployerTransaction
        [HttpPost]
        public ActionResult GetPFET(PFEmployerTransaction oPFET)
        {
            try
            {
                if (oPFET.PETID <= 0) { throw new Exception("Please select an valid item."); }
                oPFET = PFEmployerTransaction.Get(oPFET.PETID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPFET = new PFEmployerTransaction();
                oPFET.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFET);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Distribute PF
        [HttpPost]

        public ActionResult Distribute(PFEmployerTransaction oPFEmpT)
        {

            PFEmployerTransaction oPFET = new PFEmployerTransaction();
            try
            {
                if (oPFEmpT.PETID <= 0) { throw new Exception("Please select an valid item."); }
                oPFET = PFEmployerTransaction.Distribute((int)EnumDBOperation.Insert, oPFEmpT.PETID, ((User)Session[SessionInfo.CurrentUser]).UserID);

             }
            catch (Exception ex)
            {
                oPFET.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFET);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Rollback
        [HttpPost]
        public ActionResult Rollback(PFDistribution oPFD)
        {


            PFEmployerTransaction oPFET = new PFEmployerTransaction();
            try
            {
                int nPETID = oPFD.PETID;
                if (oPFD.PETID <= 0) { throw new Exception("Please select an valid item."); }
                oPFD = PFDistribution.Rollback((int)EnumDBOperation.Delete, oPFD.PETID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oPFD.ErrorMessage != Global.DeleteMessage)
                    throw new Exception(oPFD.ErrorMessage);
                else
                    oPFET = PFEmployerTransaction.Get(nPETID, ((User)Session[SessionInfo.CurrentUser]).UserID);

               
            }
            catch (Exception ex)
            {
                oPFET = new PFEmployerTransaction();
                oPFET.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPFET);
            return Json(sjson, JsonRequestBehavior.AllowGet);
            
        }

        #endregion

        #region PrintXL
        public void Print_ReportXL(int id)
        {

            List<PFDistribution> _oPFDistribution = new List<PFDistribution>();
            List<PFDistribution> oPFDistribution = new List<PFDistribution>();


            _oPFDistribution = PFDistribution.Gets(("Select * from View_PFDistribute where PETID = " + id), ((User)Session[SessionInfo.CurrentUser]).UserID);
          
           
            
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;



            

        
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 8;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border; 
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PF DISTRIBUTION");
                sheet.Name = "PF DISTRIBUTION";

                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 25;
                sheet.Column(5).Width = 25;
                sheet.Column(6).Width = 25;
                sheet.Column(7).Width = 25;



                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "PF DISTRIBUTION  "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Self Contribution"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "All member contribution"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Profit amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    
                nRowIndex = nRowIndex + 1;


                #endregion
                string sTemp = "";
                #region Data
                foreach (PFDistribution oItem in _oPFDistribution)
                {

                    nSL++;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.SelfContribution; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AllMemberContribution; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ProfitAmountX; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                    
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PF_Distribution.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            #endregion

        }
    






        }
        #endregion
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