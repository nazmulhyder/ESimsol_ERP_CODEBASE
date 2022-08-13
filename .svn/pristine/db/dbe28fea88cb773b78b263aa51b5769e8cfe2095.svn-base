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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class ItemMovementController : Controller
    {
        #region Declaration
        InventoryTraking _oInventoryTraking = new InventoryTraking();
        List<InventoryTraking> _oInventoryTrakings = new List<InventoryTraking>();
        BusinessUnit oBusinessUnit = new BusinessUnit();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion

        #region Action
        public ActionResult ViewItemMovements(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oInventoryTrakings = new List<InventoryTraking>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oClientOperationSetting = new ClientOperationSetting();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ViewBag.TriggerParentTypes = EnumObject.jGets(typeof(EnumTriggerParentsType));
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            return View(_oInventoryTrakings);
        }
        public ActionResult ViewItemMovementDetails(int ProductID, int WorkingUnitID, int ColorID, int LotID, int BUID, string BUIDs, string sParam)
        {
            string Measurement = "";
            _oInventoryTrakings = new List<InventoryTraking>();
            int nReportLayout = Convert.ToInt32(sParam.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sParam.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParam.Split('~')[2]);
            if (Session[SessionInfo.SearchData] != null && Session[SessionInfo.SearchData] != "")
            {
                Measurement = ((InventoryTraking)Session[SessionInfo.SearchData]).Measurement;
            }
            string sSQL = "INSERT INTo #TempTable (ITransactionID) SELECT HH.ITransactionID  FROM ITransaction AS HH WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "' ,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106)) ";
            string sSQL_FindLot = "INSERT INTO #TempOpening (LotID) SELECT DISTINCT HH.LotID FROM ITransaction AS HH WHERE CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) <= CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
         
                #region Lot
                if (LotID != 0)
                {
                    sSQL += " AND LotID = " + LotID;
                    sSQL_FindLot += " AND LotID = " + LotID;
                }
                #endregion
   
            if (nReportLayout != 4)// Not Lot wise
            {
                #region Multiple BU / for Group
                if (!string.IsNullOrWhiteSpace(BUIDs))
                {
                    sSQL += " AND HH.LotID IN (SELECT Lot.LotID FROM Lot WHERE BUID IN(" + BUIDs + "))";
                    sSQL_FindLot += " AND HH.LotID IN (SELECT Lot.LotID FROM Lot WHERE BUID IN(" + BUIDs + "))";
                }
                #endregion

                #region BU
                if (BUID != 0)
                {
                    sSQL += " AND HH.LotID IN (SELECT Lot.LotID FROM Lot WHERE BUID =" + BUID + ")";
                    sSQL_FindLot += " AND HH.LotID IN (SELECT Lot.LotID FROM Lot WHERE BUID =" + BUID + ")";
                }
                #endregion

                #region Product
                if (ProductID != 0)
                {
                    sSQL += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE ProductID = " + ProductID + ")";
                    sSQL_FindLot += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE ProductID = " + ProductID + ")";
                }
                #endregion
                
                #region WorkingUnit
                if (WorkingUnitID != 0)
                {
                    sSQL += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE WorkingUnitID = " + WorkingUnitID + ")";
                    sSQL_FindLot += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE WorkingUnitID = " + WorkingUnitID + ")";
                }
                #endregion

               #region Color
                if (ColorID != 0)
                {
                    sSQL += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE ColorID = " + ColorID + ")";
                    sSQL_FindLot += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE ColorID = " + ColorID + ")";
                }
                #endregion

                #region Measurement
                if (!string.IsNullOrEmpty(Measurement))
                {
                    sSQL += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE ParentType = " + (int)EnumTriggerParentsType.FinishGoodsQC + " AND ParentID IN (SELECT QCID FROM QC WHERE ProductionSheetID IN (SELECT  ProductionSheetID FROM ProductionSheet WHERE PTUUnit2ID IN(SELECT PTUUnit2ID  FROM PTUUnit2 WHERE Measurement = '"+Measurement+"' ))))";
                    sSQL_FindLot += " AND LotID IN (SELECT Lot.LotID FROM Lot WHERE ParentType = " + (int)EnumTriggerParentsType.FinishGoodsQC + " AND ParentID IN (SELECT QCID FROM QC WHERE ProductionSheetID IN (SELECT  ProductionSheetID FROM ProductionSheet WHERE PTUUnit2ID IN(SELECT PTUUnit2ID  FROM PTUUnit2 WHERE Measurement = '" + Measurement + "' ))))";
                }
                #endregion

            }

            sSQL += " Order BY HH.ITransactionID ASC";
            //sSQL_FindLot += " Order BY HH.ITransactionID ASC";

            #region Store Searcing Criteria
            InventoryTraking oInventoryTraking = new InventoryTraking();
            oInventoryTraking.StartDate = dStartDate;
            oInventoryTraking.EndDate = dEndDate;
            oInventoryTraking.Param = sSQL + '~' + sSQL_FindLot;
            oInventoryTraking.BUID = BUID;
            oInventoryTraking.Measurement= Measurement;
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oInventoryTraking);
            #endregion

        
            _oInventoryTrakings = new List<InventoryTraking>();
            return View(_oInventoryTrakings);
        }

        [HttpPost]
        public JsonResult GetMovementDetails()
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            InventoryTraking oInventoryTraking = new InventoryTraking();
            string sSQL = "";
            string sSQL_FindLot = "";

            try
            {
                oInventoryTraking = (InventoryTraking)Session[SessionInfo.ParamObj];
                if (oInventoryTraking.Param != null)
                {
                    sSQL = oInventoryTraking.Param.Split('~')[0];
                    sSQL_FindLot = oInventoryTraking.Param.Split('~')[1];
                    _oInventoryTrakings = InventoryTraking.Gets_ForItemMovementDetails(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, sSQL_FindLot, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception e)
            {
                _oInventoryTrakings = new List<InventoryTraking>();
                _oInventoryTraking = new InventoryTraking();
                _oInventoryTraking.ErrorMessage = "Failed to load data";
                _oInventoryTrakings.Add(_oInventoryTraking);
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQL(InventoryTraking oInventoryTraking)
        {
            int nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
            string BUIDs = oInventoryTraking.Param.Split('~')[1];
            string WUIDs = oInventoryTraking.Param.Split('~')[2];
            string SupplierIDs = oInventoryTraking.Param.Split('~')[3];
            string TSIDs = oInventoryTraking.Param.Split('~')[4];
            string PrductIDs = oInventoryTraking.Param.Split('~')[5];
            int nPC = Convert.ToInt32(oInventoryTraking.Param.Split('~')[6]);
            string LotNo = oInventoryTraking.Param.Split('~')[7];
            double UnitPrice =Convert.ToDouble(oInventoryTraking.Param.Split('~')[8]);
            string sITDescripton = oInventoryTraking.Param.Split('~')[9];
            string sDiaOrGSM = oInventoryTraking.Param.Split('~')[10];

            string sSQL = "INSERT INTO #TempTable(LotID) SELECT Lot.LotID FROM Lot WHERE Lot.LotID IN (SELECT DISTINCT IT.LotID FROM ITransaction AS IT WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IT.[DateTime],106)) <= CONVERT(DATE,CONVERT(VARCHAR(12),'" + oInventoryTraking.EndDate.ToString("dd MMM yyyy") + "',106)))";

            #region Multiple BU / for Group
            if (!string.IsNullOrWhiteSpace(BUIDs))
            {
                sSQL += " AND Lot.BUID IN (" + BUIDs + ")";
            }
            #endregion

            #region BU
            if (oInventoryTraking.BUID != 0)
            {
                sSQL += " AND Lot.BUID =" + oInventoryTraking.BUID;
            }
            #endregion

            #region WU
            if (!string.IsNullOrEmpty(WUIDs))
            {
                sSQL += " AND Lot.WorkingUnitID IN (" + WUIDs + ")";
            }
            #endregion

            #region Supplier
            if (!string.IsNullOrEmpty(SupplierIDs))
            {
                sSQL += " AND Lot.ContractorID IN (" + SupplierIDs + ")";
            }
            #endregion

            #region StyleID
            if (!string.IsNullOrEmpty(TSIDs))
            {
                sSQL += " AND Lot.StyleID IN (" + TSIDs + ")";
            }
            #endregion

            #region Product
            if (!string.IsNullOrEmpty(PrductIDs))
            {
                sSQL += " AND Lot.ProductID IN (" + PrductIDs + ")";
            }
            #endregion

            #region Product Category
            if (nPC != 0)
            {
                sSQL += " AND Lot.ProductID IN (SELECT ProductID FROM Product WHERE ProductCategoryID  IN  (SELECT MM.ProductCategoryID FROM dbo.fn_GetProductCategory(" + nPC + ") AS MM))";
            }
            #endregion

            #region Lot
            if (!string.IsNullOrEmpty(LotNo))
            {
                sSQL += " AND Lot.LotNo LIKE '%" + LotNo+ "%'";
            }
            #endregion
            #region sDiaOrGSM
            if (!string.IsNullOrEmpty(sDiaOrGSM))
            {
                sSQL += " AND Lot.MCDia+Lot.FinishDia+Lot.GSM LIKE '%" + LotNo + "%'";
            }
            #endregion
            
            #region IT Descripton
            if (!string.IsNullOrEmpty(sITDescripton))
            {
                sSQL += " AND Lot.LotID IN (SELECT IT.LotID FROM ITransaction AS IT WHERE IT.[Description] LIKE '%" + sITDescripton + "%')";
            }
            #endregion

            #region Unit Price
            if (UnitPrice>0)
            {
                sSQL += " AND Lot.UnitPrice =" + UnitPrice;
            }
            #endregion

            return sSQL;
        }

        [HttpPost]
        public JsonResult SearchByDate(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            try
            {
                int nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
                string sSQL = MakeSQL(oInventoryTraking);

                _oInventoryTrakings = InventoryTraking.Gets_ForItemMovement(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, nReportLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count>0)
                {
                    _oInventoryTrakings[0].TriggerParentType = oInventoryTraking.TriggerParentType;
                }
                if(oInventoryTraking.IsTransectionOnly == true)
                {
                    _oInventoryTrakings = _oInventoryTrakings.Where(x => x.InQty - x.OutQty != 0.00).ToList();
                }
            }
            catch (Exception ex)
            {
                _oInventoryTraking = new InventoryTraking();
                //oRPT_DeliverySummary.ErrorMessage = ex.Message;
                _oInventoryTrakings = new List<InventoryTraking>();
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

     
        #endregion
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



       

        #region report

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(InventoryTraking oInventoryTraking)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oInventoryTraking);
            

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintItemMovements(double ts)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            string _sErrorMesage = "";
            int nReportLayout = 0;
            try
            {
                oInventoryTraking = (InventoryTraking)Session[SessionInfo.ParamObj];
                nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
                _oInventoryTrakings = new List<InventoryTraking>();
                string sSQL = this.MakeSQL(oInventoryTraking);

                _oInventoryTrakings = InventoryTraking.Gets_ForItemMovement(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, nReportLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }

                oBusinessUnit = oBusinessUnit.Get(oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTrakings = new List<InventoryTraking>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                if(oBusinessUnit==null || oBusinessUnit.BusinessUnitID<=0)
                {
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                }
                else
                {
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
                rptInventoryTrakings oReport = new rptInventoryTrakings();
                byte[] abytes = oReport.PrepareReport(_oInventoryTrakings, oCompany, nReportLayout);
                //byte[] abytes = null;
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintItemMovementDetails(double ts)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            
            string _sErrorMesage = "";
            try
            {
                oInventoryTraking = (InventoryTraking)Session[SessionInfo.ParamObj];
                //nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
                _oInventoryTrakings = new List<InventoryTraking>();
                string sSQL = oInventoryTraking.Param.Split('~')[0] ;
                string sSQL_FindLot = oInventoryTraking.Param.Split('~')[1];
                _oInventoryTrakings = InventoryTraking.Gets_ForItemMovementDetails(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, sSQL_FindLot, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }

                oBusinessUnit = oBusinessUnit.Get(oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTrakings = new List<InventoryTraking>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                if (oBusinessUnit == null || oBusinessUnit.BusinessUnitID <= 0)
                {
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                }
                else
                {
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
                rptItemTrakingDetails oReport = new rptItemTrakingDetails();
                byte[] abytes = oReport.PrepareReport(_oInventoryTrakings, oCompany);
                //byte[] abytes = null;
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public void PrintItemMovementsExcel(double ts)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();            
            string _sErrorMesage = "";
            int nReportLayout = 0;
            try
            {
                oInventoryTraking = (InventoryTraking)Session[SessionInfo.ParamObj];
                nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
                _oInventoryTrakings = new List<InventoryTraking>();
                string sSQL = this.MakeSQL(oInventoryTraking);
                _oInventoryTrakings = InventoryTraking.Gets_ForItemMovement(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, nReportLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }

                oBusinessUnit = oBusinessUnit.Get(oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTrakings = new List<InventoryTraking>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                if (oBusinessUnit == null || oBusinessUnit.BusinessUnitID <= 0)
                {
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                }
                else
                {
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
                
                //write code
                int rowIndex = 1;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                if (nReportLayout == 4)
                {
                    #region Lot StyleBase Wise
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Item movements To Excel");
                        sheet.Name = "Item movements To Excel";
                        sheet.Column(2).Width = 8; //SL
                        sheet.Column(3).Width = 25; //Product Name
                        sheet.Column(4).Width = 25; //Style no
                        sheet.Column(5).Width = 25; //lot no
                        sheet.Column(6).Width = 50; //store
                        sheet.Column(7).Width = 6; //Currency Symbol
                        sheet.Column(8).Width = 10; //Price
                        sheet.Column(9).Width = 6; //MU
                        sheet.Column(10).Width = 15; //opening
                        sheet.Column(11).Width = 15; //IN
                        sheet.Column(12).Width = 15; //out
                        sheet.Column(13).Width = 15; //closing


                        #region Report Header

                        if (oCompany.CompanyLogo != null)
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex = rowIndex + 1;

                            ExcelPicture excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(oCompany.Phone, oCompany.CompanyLogo);
                            excelImage.From.Column = 2;
                            excelImage.From.Row = rowIndex - 1;
                            excelImage.SetSize(100, 80);
                            // 2x2 px space for better alignment
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex = rowIndex + 1;
                        }

                        
                        cell = sheet.Cells[rowIndex, 6, rowIndex, 13];cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 6, rowIndex, 13];cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 6, rowIndex, 13]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ", " + oCompany.Email; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        rowIndex++;

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Store"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Currency"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Price"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "MU"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Opening QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "In QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Out QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "Closing QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region detail
                        int nSL = 0;

                        double _nTOpeningQty = 0, _nTInQty = 0, _nTOutQty = 0, _nTClosingQty = 0;
                        foreach (InventoryTraking oItem in _oInventoryTrakings)
                        {

                            nSL++;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.StyleNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.WorkingUnitName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.Currency; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.UnitPrice.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.MUnit; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.OpeningQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.InQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.OutQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.ClosingQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            rowIndex++;

                            _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                            _nTInQty = _nTInQty + oItem.InQty;
                            _nTOutQty = _nTOutQty + oItem.OutQty;
                            _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true;
                        cell.Value = "Total:"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        

                        cell = sheet.Cells[rowIndex, 10]; 
                        cell.Value = _nTOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 

                        cell = sheet.Cells[rowIndex, 11]; cell.Merge = true;
                        cell.Value = _nTInQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      

                        cell = sheet.Cells[rowIndex, 12]; 
                        cell.Value = _nTOutQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                     

                        cell = sheet.Cells[rowIndex, 13];
                        cell.Value = _nTClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Excel_Of_ItemMovements.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else if (nReportLayout == 3)
                {
                    #region Product and Measurement Wise
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Item movements To Excel");
                        sheet.Name = "Item movements To Excel";
                        sheet.Column(2).Width = 8; //SL
                        sheet.Column(3).Width = 30; //product
                        sheet.Column(4).Width = 60; //Measuremnt
                        sheet.Column(5).Width = 6; //MU
                        sheet.Column(6).Width = 15; //opening
                        sheet.Column(7).Width = 15; //IN
                        sheet.Column(8).Width = 15; //out
                        sheet.Column(9).Width = 15; //closing


                        #region Report Header

                        if (oCompany.CompanyLogo != null)
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex = rowIndex + 1;

                            ExcelPicture excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(oCompany.Phone, oCompany.CompanyLogo);
                            excelImage.From.Column = 2;
                            excelImage.From.Row = rowIndex - 1;
                            excelImage.SetSize(100, 80);
                            // 2x2 px space for better alignment
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }


                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ", " + oCompany.Email; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        rowIndex++;

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Product"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Measuremnt"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "MU"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Opening QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "In QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Out QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Closing QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region detail
                        int nSL = 0;

                        double _nTOpeningQty = 0, _nTInQty = 0, _nTOutQty = 0, _nTClosingQty = 0;
                        foreach (InventoryTraking oItem in _oInventoryTrakings)
                        {

                            nSL++;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Measurement; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.MUnit; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.OpeningQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.InQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.OutQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.ClosingQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            rowIndex++;

                            _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                            _nTInQty = _nTInQty + oItem.InQty;
                            _nTOutQty = _nTOutQty + oItem.OutQty;
                            _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total:"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = _nTOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 7]; cell.Merge = true;
                        cell.Value = _nTInQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = _nTOutQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 9];
                        cell.Value = _nTClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Excel_Of_ItemMovements.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else if (nReportLayout == 2)
                {
                    #region Product and Color Wise
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Item movements To Excel");
                        sheet.Name = "Item movements To Excel";
                        sheet.Column(2).Width = 8; //SL
                        sheet.Column(3).Width = 30; //product
                        sheet.Column(4).Width = 60; //Color
                        sheet.Column(5).Width = 6; //MU
                        sheet.Column(6).Width = 15; //opening
                        sheet.Column(7).Width = 15; //IN
                        sheet.Column(8).Width = 15; //out
                        sheet.Column(9).Width = 15; //closing


                        #region Report Header

                        if (oCompany.CompanyLogo != null)
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex = rowIndex + 1;

                            ExcelPicture excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(oCompany.Phone, oCompany.CompanyLogo);
                            excelImage.From.Column = 2;
                            excelImage.From.Row = rowIndex - 1;
                            excelImage.SetSize(100, 80);
                            // 2x2 px space for better alignment
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                      

                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ", " + oCompany.Email; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        rowIndex++;

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Product"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "MU"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Opening QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "In QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Out QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Closing QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region detail
                        int nSL = 0;

                        double _nTOpeningQty = 0, _nTInQty = 0, _nTOutQty = 0, _nTClosingQty = 0;
                        foreach (InventoryTraking oItem in _oInventoryTrakings)
                        {

                            nSL++;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.ColorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.MUnit; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.OpeningQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.InQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.OutQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.ClosingQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            rowIndex++;

                            _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                            _nTInQty = _nTInQty + oItem.InQty;
                            _nTOutQty = _nTOutQty + oItem.OutQty;
                            _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total:"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = _nTOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 7]; cell.Merge = true;
                        cell.Value = _nTInQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = _nTOutQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 9];
                        cell.Value = _nTClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Excel_Of_ItemMovements.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else if (nReportLayout == 1)
                {
                    #region Product and store Wise
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Item movements To Excel");
                        sheet.Name = "Item movements To Excel";
                        sheet.Column(2).Width = 8; //SL
                        sheet.Column(3).Width = 30; //product
                        sheet.Column(4).Width = 60; //store
                        sheet.Column(5).Width = 6; //MU
                        sheet.Column(6).Width = 15; //opening
                        sheet.Column(7).Width = 15; //IN
                        sheet.Column(8).Width = 15; //out
                        sheet.Column(9).Width = 15; //closing


                        #region Report Header

                        if (oCompany.CompanyLogo != null)
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex = rowIndex + 1;

                            ExcelPicture excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(oCompany.Phone, oCompany.CompanyLogo);
                            excelImage.From.Column = 2;
                            excelImage.From.Row = rowIndex - 1;
                            excelImage.SetSize(100, 80);
                            // 2x2 px space for better alignment
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex = rowIndex + 1;
                        }


                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 4, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ", " + oCompany.Email; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        rowIndex++;

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Product"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Store"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "MU"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Opening QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "In QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Out QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Closing QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region detail
                        int nSL = 0;

                        double _nTOpeningQty = 0, _nTInQty = 0, _nTOutQty = 0, _nTClosingQty = 0;
                        foreach (InventoryTraking oItem in _oInventoryTrakings)
                        {

                            nSL++;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.WorkingUnitName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.MUnit; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.OpeningQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.InQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.OutQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.ClosingQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            rowIndex++;

                            _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                            _nTInQty = _nTInQty + oItem.InQty;
                            _nTOutQty = _nTOutQty + oItem.OutQty;
                            _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total:"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = _nTOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 7]; cell.Merge = true;
                        cell.Value = _nTInQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = _nTOutQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 9];
                        cell.Value = _nTClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Excel_Of_ItemMovements.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else if (nReportLayout == 0)
                {
                    #region Product Wise
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Item movements To Excel");
                        sheet.Name = "Item movements To Excel";
                        sheet.Column(2).Width = 8; //SL
                        sheet.Column(3).Width = 40; //product
                        sheet.Column(4).Width = 6; //MU
                        sheet.Column(5).Width = 15; //opening
                        sheet.Column(6).Width = 15; //IN
                        sheet.Column(7).Width = 15; //out
                        sheet.Column(8).Width = 15; //closing


                        #region Report Header

                        //if (oCompany.CompanyLogo != null)
                        //{
                        //    //sheet.Cells[rowIndex, 1, rowIndex, 2].Merge = true;
                        //    cell = sheet.Cells[rowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                        //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //    rowIndex = rowIndex + 1;

                        //    ExcelPicture excelImage = null;
                        //    excelImage = sheet.Drawings.AddPicture(oCompany.Phone, oCompany.CompanyLogo);
                        //    excelImage.From.Column = 2;
                        //    excelImage.From.Row = rowIndex - 1;
                        //    excelImage.SetSize(20, 20);
                        //    // 2x2 px space for better alignment
                        //    excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        //    excelImage.From.RowOff = this.Pixel2MTU(2);
                        //}
                        //else
                        //{
                        //    //sheet.Cells[rowIndex, 1, rowIndex, 2].Merge = true;
                        //    cell = sheet.Cells[rowIndex, 1]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    rowIndex = rowIndex + 1;
                        //}


                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ", " + oCompany.Email; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        rowIndex++;

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Product"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "MU"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Opening QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "In QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Out QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Closing QTY"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region detail
                        int nSL = 0;

                        double _nTOpeningQty = 0, _nTInQty = 0, _nTOutQty = 0, _nTClosingQty = 0;
                        foreach (InventoryTraking oItem in _oInventoryTrakings)
                        {

                            nSL++;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.MUnit; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.OpeningQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.InQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.OutQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.ClosingQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            rowIndex++;

                            _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                            _nTInQty = _nTInQty + oItem.InQty;
                            _nTOutQty = _nTOutQty + oItem.OutQty;
                            _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true;
                        cell.Value = "Total:"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 5];
                        cell.Value = _nTOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 6]; cell.Merge = true;
                        cell.Value = _nTInQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 7];
                        cell.Value = _nTOutQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = _nTClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Excel_Of_ItemMovements.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                


            }
            else
            {
                throw new Exception("No Data Available");
            }
        }

        public void PrintItemMovementDetailsExcel(double ts)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();

            string _sErrorMesage = "";
            try
            {
                oInventoryTraking = (InventoryTraking)Session[SessionInfo.ParamObj];
                _oInventoryTrakings = new List<InventoryTraking>();
                string sSQL = oInventoryTraking.Param.Split('~')[0];
                string sSQL_FindLot = oInventoryTraking.Param.Split('~')[1];
                _oInventoryTrakings = InventoryTraking.Gets_ForItemMovementDetails(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, sSQL_FindLot, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }

                oBusinessUnit = oBusinessUnit.Get(oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTrakings = new List<InventoryTraking>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                if (oBusinessUnit == null || oBusinessUnit.BusinessUnitID <= 0)
                {
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                }
                else
                {
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }


                //write code
                int rowIndex = 1;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                #region Item Movements
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Item movement Details To Excel");
                    sheet.Name = "Item movement Details To Excel";
                    sheet.Column(2).Width = 8; //SL
                    sheet.Column(3).Width = 25; //REf Date
                    sheet.Column(4).Width = 25; // Etnry Date
                    sheet.Column(5).Width = 22; //TR Type
                    sheet.Column(6).Width = 22; //TR Ref
                    sheet.Column(7).Width = 15; //Remarks
                    sheet.Column(8).Width = 8; //MU
                    sheet.Column(9).Width = 13; //IN
                    sheet.Column(10).Width = 13; //out
                    sheet.Column(11).Width = 15; //Balance


                    #region Report Header

                    //if (oCompany.CompanyLogo != null)
                    //{
                    //    sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                    //    cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                    //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //    rowIndex = rowIndex + 1;

                    //    ExcelPicture excelImage = null;
                    //    excelImage = sheet.Drawings.AddPicture(oCompany.Phone, oCompany.CompanyLogo);
                    //    excelImage.From.Column = 2;
                    //    excelImage.From.Row = rowIndex - 1;
                    //    excelImage.SetSize(100, 80);
                    //    // 2x2 px space for better alignment
                    //    excelImage.From.ColumnOff = this.Pixel2MTU(2);
                    //    excelImage.From.RowOff = this.Pixel2MTU(2);
                    //}
                    //else
                    //{
                    //    sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                    //    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //    rowIndex = rowIndex + 1;
                    //}


                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ", " + oCompany.Email; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;
                    #endregion

                    rowIndex++;

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Ref Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Entry Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Transfer Type"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Transfer Reference"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "MU"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "In QTY"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Out QTY"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region detail
                    int nSL = 0;

                    //double _nTOpeningQty = 0, _nTInQty = 0, _nTOutQty = 0, _nTClosingQty = 0;
                    foreach (InventoryTraking oItem in _oInventoryTrakings)
                    {

                        nSL++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.TransactionDateSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.EntryDateTimeSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.ParentTypeSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.RefNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.Remarks; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.MUnit; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.InQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.OutQty.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.Balance.ToString("#,###.##;(#,###.##)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;

                    }

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Excel_Of_ItemMovement_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
            else
            {
                throw new Exception("No Data Available");
            }
        }
        
        #endregion



    }
}
