using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;


namespace ESimSolFinancial.Controllers
{
    public class ExportPIController : Controller
    {
        #region Declaration
        ExportPI _oExportPI = new ExportPI();
        ExportPIDetail _oExportPIDetail = new ExportPIDetail();
        List<ExportPI> _oExportPIs = new List<ExportPI>();
        List<ExportPIDetail> _oExportPIDetails = new List<ExportPIDetail>();
        DataSet _oDataSet = new DataSet();
        DataTable _oDataTable = new DataTable();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<PISizerBreakDown> _oPISizerBreakDowns = new List<PISizerBreakDown>();
        PISizerBreakDown _oPISizerBreakDown = new PISizerBreakDown();
        ExportPISignatory _oExportPISignatory = new ExportPISignatory();
        List<ExportPISignatory> _oExportPISignatorys = new List<ExportPISignatory>();
        //List<NOASignatoryComment> _oNOASignatoryComments = new List<ExportPISignatoryComment>();
        #endregion

        #region Function
        private List<SizeCategory> GetDistictSizes(List<PISizerBreakDown> oPISizerBreakDowns)
        {
            List<SizeCategory> oSizeCategoryies = new List<SizeCategory>();
            SizeCategory oSizeCategory = new SizeCategory();
            List<PISizerBreakDown> oNewPISizerBreakDowns = oPISizerBreakDowns.OrderBy(CB => CB.StyleNo).ToList();
            foreach (PISizerBreakDown oItem in oNewPISizerBreakDowns)
            {
                if (!IsExist(oSizeCategoryies, oItem))
                {
                    oSizeCategory = new SizeCategory();
                    oSizeCategory.SizeCategoryID = oItem.SizeID;
                    oSizeCategory.SizeCategoryName = oItem.SizeName;
                    oSizeCategoryies.Add(oSizeCategory);
                }
            }

            return oSizeCategoryies;
        }
        private bool IsExist(List<SizeCategory> oSizeCategories, PISizerBreakDown oPISizerBreakDown)
        {
            foreach (SizeCategory oITem in oSizeCategories)
            {
                if (oITem.SizeCategoryID == oPISizerBreakDown.SizeID)
                {
                    return true;
                }
            }
            return false;
        }
        private List<ColorCategory> GetDistictColorWithProducts(List<PISizerBreakDown> oPISizerBreakDowns)
        {
            List<ColorCategory> oColorCategories = new List<ColorCategory>();
            ColorCategory oColorCategory = new ColorCategory();
            foreach (PISizerBreakDown oItem in oPISizerBreakDowns)
            {
                if (!IsExist(oColorCategories, oItem))
                {
                    oColorCategory = new ColorCategory();
                    oColorCategory.ColorCategoryID = oItem.ColorID;
                    oColorCategory.ColorName = oItem.ColorName;
                    oColorCategory.ObjectID = oItem.ProductID;
                    oColorCategory.ObjectName = oItem.ProductName;
                    oColorCategories.Add(oColorCategory);
                }
            }
            return oColorCategories;
        }
        private bool IsExist(List<ColorCategory> oColorCategories, PISizerBreakDown oPISizerBreakDown)
        {
            foreach (ColorCategory oITem in oColorCategories)
            {
                if (oITem.ColorCategoryID == oPISizerBreakDown.ColorID && oITem.ObjectID == oPISizerBreakDown.ProductID)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Export PI
        public ActionResult View_ExportPIs(int buid, int ProductNature, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ExportPI).ToString()+","+((int)EnumModuleName.SalesCommission).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPIs = new List<ExportPI>();
            string sSQL = "SELECT * FROM View_ExportPI WHERE PIStatus = " + (int)EnumPIStatus.Initialized + " AND ISNULL(BUID,0) = " + buid + " AND ISNULL(ProductNature,0) = "+ProductNature;
            _oExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(),buid,"", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = _oBusinessUnit.Get(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.BaseCurrencyID = oCompany.BaseCurrencyID;
            ViewBag.ProductNature = ProductNature;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            return View(_oExportPIs);
        }
        public ActionResult ViewExportPI(int id, int buid, int nature)
        {
            _oExportPI = new ExportPI();
            List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
            if(id>0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.PISizerBreakDowns = PISizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankFDDs = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.ShipmentTerms = Enum.GetValues(typeof(EnumShipmentTerms)).Cast<EnumShipmentTerms>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.Recipes = Recipe.GetsByTypeWithBUAndNature((int)EnumRecipeType.Consumtion, buid, nature, (int)Session[SessionInfo.currentUserID]);
            ViewBag.PolyMeasurementTypes = EnumObject.jGets(typeof(EnumPolyMeasurementType));
            return View(_oExportPI);
        }
        public ActionResult ViewMasterPI(int id, int buid, int nature)
        {
            _oExportPI = new ExportPI();
            List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
            if (id > 0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.PISizerBreakDowns = PISizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.MasterPIMappings = MasterPIMapping.GetsByMasterPI(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankFDDs = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.ShipmentTerms = Enum.GetValues(typeof(EnumShipmentTerms)).Cast<EnumShipmentTerms>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.Recipes = Recipe.GetsByTypeWithBUAndNature((int)EnumRecipeType.Consumtion, buid, nature, (int)Session[SessionInfo.currentUserID]);
            ViewBag.PolyMeasurementTypes = EnumObject.jGets(typeof(EnumPolyMeasurementType));
            return View(_oExportPI);
        }
        public ActionResult ViewDyeingExportPI(int id, int buid)
        {
            _oExportPI = new ExportPI();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (id > 0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.GetsByPI(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oExportPI.ExportPIDetails.Count>0)
            {
                _oExportPI.ExportPIDetails.ForEach(o => o.QtyTwo = (o.Qty * oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.UnitPriceTwo = (o.UnitPrice / oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.MUNameTwo = oMeasurementUnitCon.ToMUnit);
            }

            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankFDDs = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.ShipmentTerms = Enum.GetValues(typeof(EnumShipmentTerms)).Cast<EnumShipmentTerms>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.ExportQualitys = ExportQuality.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.SaleType = EnumObject.jGets(typeof(EnumProductionType));
            return View(_oExportPI);
        }
        public ActionResult ViewDyeingExportPIBill(int id, int buid)
        {
            _oExportPI = new ExportPI();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (id > 0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.GetsByPI(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oExportPI.ExportPIDetails.Count > 0)
            {
                _oExportPI.ExportPIDetails.ForEach(o => o.QtyTwo = (o.Qty * oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.UnitPriceTwo = (o.UnitPrice / oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.MUNameTwo = oMeasurementUnitCon.ToMUnit);
            }

            //ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankFDDs = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.ShipmentTerms = Enum.GetValues(typeof(EnumShipmentTerms)).Cast<EnumShipmentTerms>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.ExportQualitys = ExportQuality.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            return View(_oExportPI);
        }
        public ActionResult ViewExportPIRevise(int id, int buid, int nature)
        {
            _oExportPI = new ExportPI();
            List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
            if (id > 0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.PISizerBreakDowns = PISizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankFDDs = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));

            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.ShipmentTerms = Enum.GetValues(typeof(EnumShipmentTerms)).Cast<EnumShipmentTerms>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.ExportQualitys = ExportQuality.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Recipes = Recipe.GetsByTypeWithBUAndNature((int)EnumRecipeType.Consumtion, buid, nature, (int)Session[SessionInfo.currentUserID]);
            return View(_oExportPI);
        }
        public ActionResult View_ExportPIs_Tex(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ExportPI).ToString()+","+((int)EnumModuleName.SalesCommission).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oExportPIs = new List<ExportPI>();
            // return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract as FSC where Isnull(ApprovedBy,0)=0 and FSC.MktAccountID in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =%n) or UserID =%n) order by SCDate DESC", nDBUserID, nDBUserID);

            string sSQL = "SELECT Top(100)* FROM View_ExportPI WHERE PIStatus in (" + (int)EnumPIStatus.Initialized + "," + (int)EnumPIStatus.RequestForApproved + ")";
            if (buid > 0)
            {
                sSQL = sSQL + "AND ISNULL(BUID,0) =" + buid;
            }
            List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count >0)
            {
                sSQL = sSQL + " and MKTEmpID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            }
            sSQL = sSQL + "Order by IssueDate DESC";
            if (oMarketingAccounts.Count <= 0)
            {
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            _oExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = oMarketingAccounts;
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
          
          
            ViewBag.BUs = oBusinessUnits;
            ViewBag.BUID = buid;
            //ViewBag.ProductNature = ProductNature;
            return View(_oExportPIs);
        }
        [HttpPost]
        public JsonResult Approve(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            _oExportPI = oExportPI.Approve(oExportPI, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return Json(_oExportPI);
        }
        public ActionResult View_ExportPIs_Tex_Signetory(int buid, int menuid) 
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ExportPI).ToString() + "," + ((int)EnumModuleName.SalesCommission).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ExportPI).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);

            EnumRoleOperationType eRole_Signatory_Approve = EnumRoleOperationType.None;
            EnumRoleOperationType eRole_Add = EnumRoleOperationType.None;
            if (oARMs.Count > 0)
            {
                foreach (AuthorizationRoleMapping oARM in oARMs)
                {
                    if (oARM.OperationType == EnumRoleOperationType.Signatory_Approve) { eRole_Signatory_Approve = oARM.OperationType; }
                    if (oARM.OperationType == EnumRoleOperationType.Add) { eRole_Add = oARM.OperationType; }
                }
            }

            _oExportPIs = new List<ExportPI>();
            // return tc.ExecuteReader("SELECT * FROM View_FabricSalesContract as FSC where Isnull(ApprovedBy,0)=0 and FSC.MktAccountID in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =%n) or UserID =%n) order by SCDate DESC", nDBUserID, nDBUserID);

            string sSQL = "";
            if (eRole_Signatory_Approve == EnumRoleOperationType.Signatory_Approve)
            {
                sSQL = "SELECT * FROM View_ExportPI WHERE isnull(ApprovedBy,0) = 0 and ExportPIID in (Select ExportPIID from ExportPISignatory where  isnull(ApproveBy,0)=0 and RequestTo=" + ((User)Session[SessionInfo.CurrentUser]).UserID + " and ExportPISignatoryID in (Select ExportPISignatoryID from ( Select *,ROW_NUMBER() OVER (Partition by ExportPIID Order by ExportPISignatory.SLNO ASC) AS RowNumber from ExportPISignatory where isnull(ApproveBy,0)=0 ) as dd where dd.RowNumber in (1,2) and dd.ApprovalHeadID in ( Select ApprovalHeadID from ( Select *,ROW_NUMBER() OVER (Partition by ExportPIID Order by ExportPISignatory.SLNO ASC) AS RowNumber from ExportPISignatory where isnull(IsApprove,0)=0  ) as ff where ff.RowNumber=1)))";
            }
            else
            {
                sSQL = "SELECT * FROM View_ExportPI WHERE ApprovedBy = 0";
            }
            if (buid > 0)
            {
                sSQL = sSQL + "AND ISNULL(BUID,0) =" + buid;
            }
            List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count > 0)
            {
                sSQL = sSQL + " and MKTEmpID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            }
            sSQL = sSQL + "Order by IssueDate DESC";
            if (oMarketingAccounts.Count <= 0)
            {
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            _oExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = oMarketingAccounts;
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);


            ViewBag.BUs = oBusinessUnits;
            ViewBag.BUID = buid;
            //ViewBag.ProductNature = ProductNature;
            return View(_oExportPIs);
        }
        public ActionResult ViewExportPI_Tex(int id, int buid)
        {
            _oExportPI = new ExportPI();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (id > 0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.GetsByPI(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = _oExportPI.ExportPIDetails.OrderByDescending(o => o.Construction).ToList();
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count<=0)
            {
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType)); 
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankFDDs = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.MktPersons = oMarketingAccounts;
           
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.ShipmentTerms = Enum.GetValues(typeof(EnumShipmentTerms)).Cast<EnumShipmentTerms>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.ExportQualitys = ExportQuality.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            //UnitConversion oUnitConversion = new UnitConversion();
            //if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving || oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing)
            //{
            //    oUnitConversion.MeasurementUnitID = 17;
            //    oUnitConversion.ConvertedUnitID =13;
            //    oUnitConversion.MeasurementUnitSymbol = "Y";
            //    oUnitConversion.ConvertedUnitSymbol = "M";
            //    oUnitConversion.ConvertedUnitValue = 0.9144;
            //}
            //if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            //{
            //    oUnitConversion.MeasurementUnitID = 5;
            //    oUnitConversion.ConvertedUnitID = 7;
            //    oUnitConversion.MeasurementUnitSymbol = "Kg";
            //    oUnitConversion.ConvertedUnitSymbol= "Lbs";
            //    oUnitConversion.ConvertedUnitValue = 0.45359237;
            //}


            if (_oExportPI.ExportPIDetails.Count > 0)
            {
                _oExportPI.ExportPIDetails.ForEach(o => o.QtyTwo = (o.Qty * oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.UnitPriceTwo = (o.UnitPrice / oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.MUNameTwo = oMeasurementUnitCon.ToMUnit);
            }

            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();

            ViewBag.BUs = oBusinessUnits;
            ViewBag.UnitConversion = oMeasurementUnitCon;
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            return View(_oExportPI);
        }

        public ActionResult ViewExportPI_TexBill(int id, int buid)
        {
            _oExportPI = new ExportPI();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (id > 0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.GetsByPI(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count <= 0)
            {
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankFDDs = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.LIBORRates = EnumObject.jGets(typeof(EnumAllowNotAllow));
            ViewBag.MktPersons = oMarketingAccounts;

            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.ShipmentTerms = Enum.GetValues(typeof(EnumShipmentTerms)).Cast<EnumShipmentTerms>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.ExportQualitys = ExportQuality.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            UnitConversion oUnitConversion = new UnitConversion();
            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving || oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing)
            {
                oUnitConversion.MeasurementUnitID = 17;
                oUnitConversion.ConvertedUnitID = 13;
                oUnitConversion.MeasurementUnitSymbol = "Y";
                oUnitConversion.ConvertedUnitSymbol = "M";
                oUnitConversion.ConvertedUnitValue = 0.9144;
            }
            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                oUnitConversion.MeasurementUnitID = 5;
                oUnitConversion.ConvertedUnitID = 7;
                oUnitConversion.MeasurementUnitSymbol = "Kg";
                oUnitConversion.ConvertedUnitSymbol = "Lbs";
                oUnitConversion.ConvertedUnitValue = 0.45359237;
            }


            if (_oExportPI.ExportPIDetails.Count > 0)
            {
                _oExportPI.ExportPIDetails.ForEach(o => o.QtyTwo = (o.Qty * oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.UnitPriceTwo = (o.UnitPrice / oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.MUNameTwo = oMeasurementUnitCon.ToMUnit);
            }

            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();

            ViewBag.BUs = oBusinessUnits;
            ViewBag.UnitConversion = oUnitConversion;
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            return View(_oExportPI);
        }
        public ActionResult ViewExportPISignatorys(int id, double ts)
        {
            ExportPI oExportPI = new ExportPI();
            _oExportPISignatorys = new List<ExportPISignatory>();
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            List<ApprovalHeadPerson> oApprovalHeadPersons = new List<ApprovalHeadPerson>();
            oExportPI = oExportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oExportPISignatorys = ExportPISignatory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            if (_oExportPISignatorys.Count == 0)
            {
                //_oExportPISignatorys = ExportPISignatory.Gets("SELECT * FROM view_ExportPISignatory WHERE ExportPIID in (Select max(ExportPIID) from ExportPIRequisition where  ExportPIID in (SELECT ExportPIID FROM ExportPISignatory) and ExportPIID!=" + oExportPI.ExportPIID + " and PRID in (Select PurchaseRequisition.PRID from PurchaseRequisition where DepartmentID in (Select PurchaseRequisition.DepartmentID from PurchaseRequisition where PRID in (Select PRID from ExportPIRequisition where ExportPIID=" + oExportPI.ExportPIID + "))))", (int)Session[SessionInfo.currentUserID]);
                //_oExportPISignatorys = ExportPISignatory.Gets("SELECT * FROM view_ExportPISignatory WHERE ExportPIID in (Select max(ExportPIID) from ExportPI where ExportPIID = " + oExportPI.ExportPIID + ")", (int)Session[SessionInfo.currentUserID]);
                ////////////////////////////////////////////// SELECT * FROM view_ExportPISignatory WHERE ExportPIID in (Select max(ExportPIID) from ExportPI where ExportPIID = 2381)

                if (_oExportPISignatorys.Count == 0)
                {
                    oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.ExportPI + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);
                    //oApprovalHeadPersons = ApprovalHeadPerson.Gets("SELECT * FROM View_ApprovalHeadPerson WHERE ApprovalHeadID in (SELECT ApprovalHeadID FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.ExportPI + " ) and UserID not in (SELECT RequestTo FROM ExportPISignatory WHERE ExportPIID=" + id + ")", (int)Session[SessionInfo.currentUserID]);
                    foreach (ApprovalHead oitem in oApprovalHeads)
                    {
                        _oExportPISignatory = new ExportPISignatory();
                        _oExportPISignatory.ExportPIID = id;
                        _oExportPISignatory.ApprovalHeadID = oitem.ApprovalHeadID;
                        if (oitem.Sequence == 1)
                        {
                            _oExportPISignatory.RequestTo = (int)Session[SessionInfo.currentUserID];
                            _oExportPISignatory.Name_Request = (string)Session[SessionInfo.currentUserName];
                        }
                        _oExportPISignatory.ApproveDate = DateTime.MinValue;
                        //_oExportPISignatory.SLNo = oitem.ApprovalHeadID;
                        _oExportPISignatory.SLNo = oitem.Sequence;
                        _oExportPISignatory.HeadName = oitem.Name;
                        _oExportPISignatorys.Add(_oExportPISignatory);
                    }
                }
                else
                {
                    foreach (ExportPISignatory oitem in _oExportPISignatorys)
                    {
                        oitem.ExportPISignatoryID = 0;
                        oitem.ExportPIID = oExportPI.ExportPIID;
                        oitem.ApproveDate = DateTime.MinValue;
                        oitem.ApproveBy = 0;
                        oitem.IsApprove = false;

                    }


                }
            }
            _oExportPISignatorys = _oExportPISignatorys.OrderBy(x => x.SLNo).ToList();
            ViewBag.ExportPI = oExportPI;
            return View(_oExportPISignatorys);
        }

        #region PI Mapping
        public ActionResult ViewMasterPIMapping(int id)
        {
            _oExportPI = new ExportPI();
            List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
            if (id > 0)
            {
                _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);    
                _oExportPI.MasterPIMappings= MasterPIMapping.GetsByMasterPI(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oExportPI);
        }
        public ActionResult ViewExportPI_Approve(int id, double ts)// Approved
        {
            //// this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ExportPI).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]););
            //List<ExportPISignatory> oExportPISignatorys = new List<ExportPISignatory>();
            //ExportPISignatory oExportPISignatory = new ExportPISignatory();
            //ClientOperationSetting oClientOperationSettingExportPI = new ClientOperationSetting();
            //oClientOperationSettingExportPI = oClientOperationSettingExportPI.GetByOperationType((int)EnumOperationType.ExportPIOperationType, (int)Session[SessionInfo.currentUserID]);
            //oExportPISignatorys = ExportPISignatory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            //if (!string.IsNullOrEmpty(oClientOperationSettingExportPI.Value))
            //{
            //    oClientOperationSettingExportPI.Value = oClientOperationSettingExportPI.Value;
            //}
            //else
            //{
            //    oClientOperationSettingExportPI.Value = "ExportPI";
            //}
            //ViewBag.ExportPIOperationType = oClientOperationSettingExportPI;
            //ViewBag.ExportPISignatorys = oExportPISignatorys;
            //ViewBag.ExportPISignatory = oExportPISignatorys.Where(x => x.RequestTo == (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
            //_oExportPI = new ExportPI();

            ////int maxValue = oExportPISignatorys.Max();
            //oExportPISignatorys = oExportPISignatorys.Where(p => p.ApproveBy != 0).ToList();
            //int nExportPISignatoryID = 0;
            //if (oExportPISignatorys.Count > 0)
            //{
            //    nExportPISignatoryID = oExportPISignatorys.Where(p => p.ApproveBy != 0).Last().ExportPISignatoryID;
            //}
            //if (id > 0)
            //{
            //    _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    if (_oExportPI.ApprovedBy != 0)
            //    {
            //        _oExportPI.SupplierRateProcess = SupplierRateProcess.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    }
            //    else
            //    {
            //        _oExportPI.SupplierRateProcess = SupplierRateProcess.GetsBy(id, nExportPISignatoryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    }
            //}

            return View(_oExportPI);
        }
        [HttpPost]
        public JsonResult GetMasterPIMappingList(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            List<MasterPIMapping> oMasterPIMappings = new List<MasterPIMapping>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPI WHERE ContractorID = " + oExportPI.ContractorID + " AND RateUnit = " + oExportPI.RateUnit + "   AND ExportPIID NOT IN (SELECT ExportPIID FROM ExportPILCMapping) AND CurrencyID = " + oExportPI.CurrencyID + " AND ExportPIID NOT IN (SELECT ExportPIID FROM MasterPIMapping) AND ISNULL(PIType,0)!=" + (int)EnumPIType.MasterPI;
                if(oExportPI.BUID!=0)
                {
                    sSQL += " AND BUID ="+oExportPI.BUID;
                }
                _oExportPIs = ExportPI.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(ExportPI oItem in _oExportPIs)
                {
                    MasterPIMapping oMasterPIMapping = new MasterPIMapping();
                    oMasterPIMapping.MasterPIMappingID = 0;
                    oMasterPIMapping.ExportPIID = oItem.ExportPIID;
                    oMasterPIMapping.CurrencyID = oItem.CurrencyID;
                    oMasterPIMapping.Amount = oItem.Amount;
                    oMasterPIMapping.PINo = oItem.PINo;
                    oMasterPIMapping.Currency = oItem.Currency;
                    oMasterPIMapping.IssueDate = oItem.IssueDate;
                    oMasterPIMapping.ValidityDate = oItem.ValidityDate;
                    oMasterPIMapping.BranchAddress = oItem.BranchAddress;
                    oMasterPIMappings.Add(oMasterPIMapping);
                }
            }
            catch (Exception ex)
            {
                MasterPIMapping oMasterPIMapping = new MasterPIMapping();
                oMasterPIMapping.ErrorMessage = ex.Message;
                oMasterPIMappings.Add(oMasterPIMapping);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMasterPIMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPIDetailProducts(ExportPI oExportPI)
        {
            _oExportPIDetails = new List<ExportPIDetail>();
            List<MasterPIMapping> oMasterPIMappings = new List<MasterPIMapping>();
            try
            {
                string sSQL = "SELECT  ProductID, ColorID,  ProductCode, ProductName, ColorName, SizeName,MUnitID, MUName,PolyMUnitID, Measurement , SUM(ISNULL(Qty,0)) AS Qty, Avg(ISNULL(UnitPrice,0)) AS UnitPrice, SUM(ISNULL(Amount,0)) as Amount,  RecipeID,   RecipeName, ProductDescription, StyleNo, BuyerReference, ModelReferenceID, ModelReferenceName    FROM View_ExportPIDetail WHERE ExportPIID IN (" + oExportPI.Params + ")  GROUP BY ProductID, ColorID, PolyMUnitID,  SizeName, ProductCode, ProductName, ColorName,Measurement, MUnitID, MUName, RecipeID,   RecipeName, ProductDescription, StyleNo, BuyerReference, ModelReferenceID, ModelReferenceName ";
                _oExportPIDetails = ExportPIDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                MasterPIMapping oMasterPIMapping = new MasterPIMapping();
                oMasterPIMapping.ErrorMessage = ex.Message;
                oMasterPIMappings.Add(oMasterPIMapping);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExpotPIMapping(ExportPI oExportPI)
        {
            try
            {
                oExportPI = oExportPI.SavePIMapping(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportPI = new ExportPI();
                oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult Save(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = oExportPI.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateExportPI(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = oExportPI.UpdatePIInfo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdatePaymentType(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = oExportPI.UpdatePaymentType(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExportPI(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = _oExportPI.Get(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPIDetails = ExportPIDetail.GetsByPI(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPriviousSCRemarks(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "SELECT top 1 * FROM  View_ExportPI WHERE ISNULL(SCRemarks,'')!='' Order BY ExportPIID DESC";
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(_oExportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs[0]);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSizerBreakDowns(ExportPI oExportPI)
        {
            List<POSizerBreakDown> oPOSizerBreakDowns = new List<POSizerBreakDown>();
            _oPISizerBreakDowns = new List<PISizerBreakDown>();
            try
            {
               _oPISizerBreakDowns = PISizerBreakDown.Gets(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(PISizerBreakDown oItem in _oPISizerBreakDowns)
                {
                    POSizerBreakDown oPOSizerBreakDown = new POSizerBreakDown();
                    oPOSizerBreakDown.ProductName = oItem.ProductName;
                    oPOSizerBreakDown.ProductID = oItem.ProductID;
                    oPOSizerBreakDown.ColorID = oItem.ColorID;
                    oPOSizerBreakDown.ColorName = oItem.ColorName;
                    oPOSizerBreakDown.SizeID = oItem.SizeID;
                    oPOSizerBreakDown.SizeName = oItem.SizeName;
                    oPOSizerBreakDown.Model = oItem.Model;
                    oPOSizerBreakDown.PantonNo= oItem.PantonNo;
                    oPOSizerBreakDown.StyleNo = oItem.StyleNo;
                    oPOSizerBreakDown.Quantity = oItem.Quantity;
                    oPOSizerBreakDown.Remarks = oItem.Remarks;
                    oPOSizerBreakDowns.Add(oPOSizerBreakDown);
                }
            }
            catch (Exception ex)
            {
                oExportPI = new ExportPI();
                oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPOSizerBreakDowns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteExportPI(ExportPI oExportPI)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportPI.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult UpdatePIStatus(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = oExportPI.UpdatePIStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdatePINo(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = oExportPI.UpdatePINo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateQuality(ExportPIDetail oExportPIDetail)
        {
            oExportPIDetail = oExportPIDetail.UpdateQuality(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oExportDocSetup.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateAutoPO(ExportPI oExportPI)
        {

            try
            {
                oExportPI = oExportPI.Get(oExportPI.ExportPIID,((User)Session[SessionInfo.CurrentUser]).UserID);
                oExportPI.ExportPIDetails = ExportPIDetail.Gets(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oExportPI.ExportPIDetails = oExportPI.ExportPIDetails.OrderBy(CB => CB.FabricID).ToList();

                List<FabricSalesContractDetail> oFabricSCDetails = new List<FabricSalesContractDetail>();
                FabricSalesContractDetail oFabricSCDetail = new FabricSalesContractDetail();

                FabricSalesContract oFabricSalesContract = new FabricSalesContract();
                oFabricSalesContract.FabricSalesContractID = 0;
                oFabricSalesContract.ContractorID = oExportPI.ContractorID;
                oFabricSalesContract.BuyerID = oExportPI.BuyerID;
                oFabricSalesContract.ContactPersonnelID = oExportPI.ContractorContactPerson;
                oFabricSalesContract.ContactPersonnelID_Buyer = oExportPI.BuyerContactPerson;
                oFabricSalesContract.CurrencyID = oExportPI.CurrencyID;
                oFabricSalesContract.PaymentType = oExportPI.PaymentType;
                oFabricSalesContract.MktAccountID = oExportPI.MKTEmpID;
                oFabricSalesContract.OrderType = (int)EnumFabricRequestType.Bulk;
                oFabricSalesContract.SCDate = DateTime.Now;
                oFabricSalesContract.LCTermID = oExportPI.LCTermID;
                oFabricSalesContract.BUID = oExportPI.BUID; //Pls change it by BU

                foreach (ExportPIDetail oitem in oExportPI.ExportPIDetails)
                {
                    if (oitem.OrderSheetDetailID <= 0 && oitem.FabricID>0)
                    {
                        oFabricSCDetail = new FabricSalesContractDetail();
                        oFabricSCDetail.ProductID = oitem.ProductID;
                        oFabricSCDetail.FabricID = oitem.FabricID;
                        oFabricSCDetail.Qty = oitem.Qty;
                        oFabricSCDetail.ExportPIDetailID= oitem.ExportPIDetailID;
                        oFabricSCDetail.MUnitID = oitem.MUnitID;
                        oFabricSCDetail.UnitPrice = oitem.UnitPrice;
                        oFabricSCDetail.BuyerReference = oitem.BuyerReference;
                        oFabricSCDetail.ColorInfo = oitem.ColorInfo;
                        oFabricSCDetail.StyleNo = oitem.StyleNo;
                        oFabricSCDetail.Note = oitem.ProductDescription;
                        oFabricSCDetail.FabricWeave = oitem.FabricWeave;
                        oFabricSCDetail.FinishType = oitem.FinishType;
                        //oFabricSCDetail.DesignPattern = oitem.DesignPattern;
                        oFabricSCDetail.FabricWidth = oitem.FabricWidth;
                        oFabricSCDetail.FabricDesignID = oitem.FabricDesignID;
                        oFabricSCDetail.Construction = oitem.Construction;
                        oFabricSCDetail.Weight = oitem.Weight;
                        oFabricSCDetail.Shrinkage = oitem.Shrinkage;
                        oFabricSCDetails.Add(oFabricSCDetail);
                    }
                }
                if (oFabricSCDetails.Count > 0)
                {
                    oFabricSalesContract.FabricSalesContractDetails = oFabricSCDetails;
                    oFabricSalesContract = oFabricSalesContract.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oExportPI.ErrorMessage ="PO No:"+ oFabricSalesContract.SCNo;
                }
                else
                {
                    oFabricSalesContract.ErrorMessage = "PO Already issue";
                }
                oExportPI.ErrorMessage = oFabricSalesContract.ErrorMessage;
                }
            catch (Exception ex)
            {

                oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptExportPIRevise(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = oExportPI.AcceptExportPIRevise(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvSearch(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = MakeSQL(oExportPI);
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                _oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(_oExportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBySearchKey(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                if (!string.IsNullOrEmpty(oExportPI.PINo)) 
                {
                    oExportPI.PINo = oExportPI.PINo.Trim();
                }
                else
                {
                    oExportPI.PINo = "";
                }
                string sSQL = "";
                if (oExportPI.BUID == 0)
                {
                    sSQL = "SELECT * FROM View_ExportPI WHERE PINO Like '%" + oExportPI.PINo + "%'";
                }
                else
                {
                    sSQL = "SELECT * FROM View_ExportPI WHERE BUID = " + oExportPI.BUID + " AND PINO Like '%" + oExportPI.PINo + "%'";
                }
                if (oExportPI.ProductNatureInt>0)
                {
                    sSQL += " AND ISNULL(ProductNature,0) = " + oExportPI.ProductNatureInt;
                }
                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(oExportPI.BUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    sSQL += " AND MKTEmpID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }

                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(_oExportPI);
            }

            var jsonResult = Json(_oExportPIs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsByContractorForSU(ExportPI oExportPI)
        {
            //Ratin
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "SELECT EPI.* FROM View_ExportPI AS EPI WHERE "
                            + " EPI.PIStatus > " + (int)EnumPIStatus.RequestForApproved + ""
                            + " AND EPI.PIStatus < " + (int)EnumPIStatus.Cancel + " "
                            + " AND EPI.BUID = " + oExportPI.BUID + " ";

                if (!string.IsNullOrEmpty(oExportPI.PINo))
                {
                    sSQL = sSQL + " AND EPI.PINo LIKE '%" + oExportPI.PINo + "%'";
                }

                if (oExportPI.ContractorID > 0)
                {
                    sSQL = sSQL + " AND EPI.ContractorID IN (" + oExportPI.ContractorID + ") ";
                }

                if (oExportPI.LCID > 0)
                {
                    sSQL = sSQL + " AND EPI.LCID IN (" + oExportPI.LCID + ") ";
                }

                if (oExportPI.PaymentTypeInInt == (int)EnumPIPaymentType.NonLC)
                {
                    sSQL = sSQL + " AND EPI.PaymentType = " + (int)EnumPIPaymentType.NonLC + " ";
                }
                else
                {
                    sSQL = sSQL + " AND EPI.PaymentType = " + (int)EnumPIPaymentType.LC + " ";
                }


                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(_oExportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByPINoAndContractor(ExportPI oExportPI)
        {
            if (oExportPI.ContractorName == "") oExportPI.ContractorName = null;
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPI WHERE ContractorName Like '%" + oExportPI.ContractorName + "%' AND ContractorID = " + oExportPI.ContractorID;
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(_oExportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByExportPIBUWise(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPI WHERE BUID=" + oExportPI.BUID + " AND ProductNature = " + oExportPI.ProductNatureInt;
                if (oExportPI.PINo != null && oExportPI.PINo != "")
                {
                    sSQL += " And PINo Like '%" + oExportPI.PINo.Trim() + "%'";
                }

                if (oExportPI.ContractorID > 0)
                {
                    sSQL += " And ContractorID = " + oExportPI.ContractorID + "";
                }
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(_oExportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        
        private string MakeSQL(ExportPI oExportPI)
        {
            string sParams = oExportPI.Note;
         
            int nCboIssueDate = 0;
            DateTime dFromIssueDate = DateTime.Today;
            DateTime dToIssueDate = DateTime.Today;
            int nCboValidityDate = 0;
            DateTime dFromValidityDate = DateTime.Today;
            DateTime dToValidityDate = DateTime.Today;
            int nCboLCDate = 0;
            DateTime dFromLCDate = DateTime.Today;
            DateTime dToLCDate = DateTime.Today;
            int nCboPIBank = 0;
            int nCboMkPerson = 0;
            string sCurrentStatus = "";
            int nPaymentType = 0;
            bool bExportLCIsntCreateYet = false;
            int nPIType = 0;
            bool bYetNotMakeFEO = false;
            string sTemp = "";
            string sConstruction = "";
            int nProcessType = 0;
            string sStyleNo = "";
            int nBUID = 0;
            int nProductNature = 0;
            string sPONo = "";
            string sMAccountIds = "";
            string _sMAGroupIds = "";
            int nCboStatusDate = 0;
            DateTime dFromStatusDate = DateTime.Today;
            DateTime dToStatusDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oExportPI.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                _oExportPI.BuyerName = Convert.ToString(sParams.Split('~')[1]);
                nCboIssueDate = Convert.ToInt32(sParams.Split('~')[2]);
                dFromIssueDate = Convert.ToDateTime(sParams.Split('~')[3]);
                dToIssueDate = Convert.ToDateTime(sParams.Split('~')[4]);
                nCboValidityDate = Convert.ToInt32(sParams.Split('~')[5]);
                dFromValidityDate = Convert.ToDateTime(sParams.Split('~')[6]);
                dToValidityDate = Convert.ToDateTime(sParams.Split('~')[7]);
                nCboLCDate = Convert.ToInt32(sParams.Split('~')[8]);
                dFromLCDate = Convert.ToDateTime(sParams.Split('~')[9]);
                dToLCDate = Convert.ToDateTime(sParams.Split('~')[10]);
                nCboPIBank = Convert.ToInt32(sParams.Split('~')[11]);
                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[12]);
                sCurrentStatus = Convert.ToString(sParams.Split('~')[13]);
                nPaymentType = Convert.ToInt32(sParams.Split('~')[14]);
               
                nPIType = Convert.ToInt32(sParams.Split('~')[15]);
                sStyleNo = Convert.ToString(sParams.Split('~')[16]);

                nCboStatusDate = Convert.ToInt32(sParams.Split('~')[17]);
                sTemp = sParams.Split('~')[18];
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dFromStatusDate = Convert.ToDateTime(sParams.Split('~')[18]);
                }
                sTemp = sParams.Split('~')[19];
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dToStatusDate = Convert.ToDateTime(sParams.Split('~')[19]);
                }
               
                nBUID = Convert.ToInt32(sParams.Split('~')[20]);
                nProductNature = Convert.ToInt32(sParams.Split('~')[21]);
                sPONo = sParams.Split('~')[22];

                 sMAccountIds = string.Empty;
                 if (sParams.Split('~').Length > 23)
                    sMAccountIds = sParams.Split('~')[23];

                 _sMAGroupIds = string.Empty;
                 if (sParams.Split('~').Length > 24)
                    _sMAGroupIds = sParams.Split('~')[24];
            }


            string sReturn1 = "SELECT * FROM View_ExportPI AS EPI ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(_oExportPI.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.ContractorID in(" + _oExportPI.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oExportPI.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.BuyerID in(" + _oExportPI.BuyerName+")";
            }
            #endregion

            #region Issue Date
            if (nCboIssueDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboIssueDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Validity Date
            if (nCboValidityDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboValidityDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.ValidityDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.ValidityDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.ValidityDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.ValidityDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.ValidityDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.ValidityDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region LC Date
            if (nCboLCDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
              
                if (nCboLCDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "EPI.ExportPIID in (Select ExportPIID from ExportPILCMapping  where Activity=1 and  CONVERT(DATE,CONVERT(VARCHAR(12),ExportPILCMapping.[Date],106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboLCDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.LCOpenDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboLCDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.LCOpenDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboLCDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.LCOpenDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }
              
                else if (nCboLCDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "EPI.ExportPIID in (Select ExportPIID from ExportPILCMapping  where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),ExportPILCMapping.[Date],106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboLCDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.LCOpenDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Status With Date
            if (nCboStatusDate != (int)EnumCompareOperator.None)
            {
                if (!String.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboStatusDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " EPI.ExportPIID in (Select ExportPIID from ExportPIHistory where CurrentStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    if (nCboStatusDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EPI.ExportPIID in (Select ExportPIID from ExportPIHistory where CurrentStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToStatusDate.ToString("dd MMM yyyy") + "',106)) ) ";
                    }
                    sCurrentStatus = "";
                }
              
            }
            #endregion
            #region PI Bank (BankBranchID)
            if (nCboPIBank > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.BankBranchID = " + nCboPIBank;
            }
            #endregion

            #region nPayment Type
            if (nPaymentType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.PaymentType = " + nPaymentType;
            }
            #endregion

            #region Mkt. Person
            if (nCboMkPerson > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.MKTEmpID = " + nCboMkPerson;
            }
            #endregion

            #region sMAccountIDs
            if (!String.IsNullOrEmpty(sMAccountIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.MKTEmpID  in(" + sMAccountIds + ") ";
            }
            #endregion

            #region sMKTGroupIDs
            if (!String.IsNullOrEmpty(_sMAGroupIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "EPI.MKTEmpID  in(Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + _sMAGroupIds + ")) ";
            }
            #endregion
            #region Current Status
            if (!string.IsNullOrEmpty(sCurrentStatus))
            {
                if (nCboStatusDate == (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " EPI.PIStatus IN (" + sCurrentStatus + ")";
                }
            }
            #endregion

            #region LC Pending
            if (bExportLCIsntCreateYet)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCID=0 AND PIStatus>" + (int)EnumPIStatus.RequestForApproved + " ";
            }
            #endregion

            #region PI Type
            if (nPIType >= 0)/// none =-1
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.PIType = " + nPIType;
            }
            #endregion

            #region Yet Not Make FEO
            if (bYetNotMakeFEO)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.ExportPIID NOT IN (SELECT PIID FROM FabricExecutionOrder) ";
            }
            #endregion

            #region FEO No
            //if (!string.IsNullOrEmpty(sFEONo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " EPI.ExportPIID IN (SELECT FEO.PIID FROM FabricExecutionOrder AS FEO WHERE FEO.FEONo LIKE '%" + sFEONo + "%') ";
            //}
            #endregion

            #region Construction
            if (!string.IsNullOrEmpty(sConstruction))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.ExportPIID IN (SELECT EPD.ExportPIID FROM ExportPIDetail AS EPD WHERE EPD.FabricID IN (SELECT F.FabricID FROM Fabric AS F WHERE F.Construction LIKE '%" + sConstruction + "%')) ";
            }
            #endregion

            #region ProcessType
            if (nProcessType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.ExportPIID IN (SELECT EPD.ExportPIID FROM ExportPIDetail AS EPD WHERE EPD.FabricID IN (SELECT F.FabricID FROM Fabric AS F WHERE F.ProcessType = " + nProcessType + ")) ";
            }
            #endregion

            #region Style No
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.ExportPIID IN (SELECT EPD.ExportPIID FROM ExportPIDetail AS EPD WHERE EPD.StyleNo LIKE '%" + sStyleNo + "%') ";
            }
            #endregion

            #region Style No
            if (!string.IsNullOrEmpty(sPONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "exportPIID in (Select ExportPIID from ExportPIDetail where OrderSheetDetailID in (Select FSD.FabricSalesContractDetailID from FabricSalesContractDetail as FSD where FSD.FabricSalesContractID in ( Select FabricSalesContractID from FabricSalesContract where SCNo like '%" + sPONo + "%')))";
            }
            #endregion

            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.BUID = " + nBUID;
            }
            #endregion
            #region ProductNature
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " ISNULL(EPI.ProductNature,0) = " + nProductNature;
            #endregion

            List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MKTEmpID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            }


            string sSQL = sReturn1 + " " + sReturn;
            return sSQL;
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
        public Image GetSignature(UserImage oUserImage)
        {
            if (oUserImage.ImageFile != null)
            {

                string fileDirectory = Server.MapPath("~/Content/SignatureImage.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public Image GetBU_Footer(AttachDocument oAttachDocument)
        {
            if (oAttachDocument.AttachFile != null)
            {

                string fileDirectory = Server.MapPath("~/Content/SignatureImage.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oAttachDocument.AttachFile);
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

        #region Export PI Detail
      
     
        [HttpPost]
        public JsonResult GetsPIDetails(ExportPI oExportPI)
        {
            _oExportPIDetails = new List<ExportPIDetail>();
            try
            {
                if (oExportPI.ExportPIID > 0)
                {
                    _oExportPIDetails = ExportPIDetail.GetsByPI(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oExportPIDetail = new ExportPIDetail();
                _oExportPIDetail.ErrorMessage = ex.Message;
                _oExportPIDetails.Add(_oExportPIDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
     
        #endregion

        #region Export PI History
        [HttpPost]
        public JsonResult GetExportPIHistory(ExportPIHistory oExportPIHistory)
        {
            List<ExportPIHistory> oExportPIHistorys = new List<ExportPIHistory>();
            try
            {
                oExportPIHistorys = ExportPIHistory.GetsByExportId(oExportPIHistory.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportPIHistorys = new List<ExportPIHistory>();
                ExportPIHistory oEPIH = new ExportPIHistory();
                oEPIH.ErrorMessage = ex.Message;
                oExportPIHistorys.Add(oEPIH);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Revise History
        [HttpPost]
        public JsonResult GetReviseHistory(ExportPI oExportPI)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            try
            {
                oExportPIs = ExportPI.GetsLog(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportPIs = new List<ExportPI>();
                ExportPI oEPI = new ExportPI();
                oEPI.ErrorMessage = ex.Message;
                oExportPIs.Add(oEPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PISWAP(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                _oExportPI = oExportPI.PISWAP( ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExpotPILogs(ExportPI oExportPILog)
        {
            List<ExportPIDetail> oExportPIDetailLogs = new List<ExportPIDetail>();
            List<ExportPITandCClause> oExportPITandCClauseLogs = new List<ExportPITandCClause>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPIDetailLog WHERE ExportPILogID = " + oExportPILog.ExportPILogID;
                oExportPILog.ExportPIDetails = ExportPIDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_ExportPITandCClauseLog WHERE ExportPILogID = " + oExportPILog.ExportPILogID;
                oExportPILog.ExportPITandCClauses = ExportPITandCClause.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //oExportPILog.ExportPITandCClauses = ExportPITandCClause.Gets(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportPILog = new ExportPI();
                oExportPILog.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPILog);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult GetsByBuyer(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                int nBuyerID = Convert.ToInt32(oExportPI.Params.Split('~')[0]);
                int nMktPersonID = Convert.ToInt32(oExportPI.Params.Split('~')[1]);
                string sPINo = oExportPI.Params.Split('~')[2].Trim();
                bool bIsAll = Convert.ToBoolean(oExportPI.Params.Split('~')[3]);

                string sSQL = "SELECT * from View_ExportPI Where ExportPIID<>0 and BuyerID=" + nBuyerID + "";

                if (nMktPersonID > 0) { sSQL = sSQL + " and MKTEmpID=" + nMktPersonID + ""; }
                if (sPINo != "") { sSQL = sSQL + " and PINo Like '%" + sPINo + "%'"; }

                if (!bIsAll)
                {
                    sSQL = sSQL + " and ExportPIID Not In( Select distinct(PIID) from FabricExecutionOrder  Where PIID<>0)";
                }
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByBuyerAndWeaving(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                int nBuyerID = Convert.ToInt32(oExportPI.Params.Split('~')[0]);
                string sPINo = oExportPI.Params.Split('~')[1].Trim();
                bool bIsAll = Convert.ToBoolean(oExportPI.Params.Split('~')[2]);

                string sSQL = "SELECT * from View_ExportPI WHERE BUID=" + oExportPI.BUID + " AND ExportPIID<>0 and BuyerID=" + nBuyerID + "";

                if (sPINo != "") { sSQL = sSQL + " and PINo Like '%" + sPINo + "%'"; }

                //if (!bIsAll)
                //{
                //    sSQL = sSQL + " and ExportPIID Not In( Select distinct(PIID) from FabricExecutionOrder  Where PIID<>0)";
                //}
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region
        [HttpPost]
        public JsonResult GetsPI(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sPINo = oExportPI.Params.Split('~')[0].Trim();
                int nPaymentType = Convert.ToInt32(oExportPI.Params.Split('~')[1]);

                string sSQL = "SELECT * from View_ExportPI Where ExportPIID<>0 and ApprovedBy>0 ";
                if (nPaymentType > 0)
                {
                    sSQL = " And PaymentType=" + nPaymentType + "";
                }
                if (sPINo != "")
                {
                    sSQL = sSQL + " And PINo Like '%" + sPINo + "%'";
                }
                if (oExportPI.BUID > 0)
                {
                    sSQL = sSQL + " And BUID=" + oExportPI.BUID + "";
                }
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ExportPI Summary Report

        public ActionResult ViewExportPISummary(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportPI = new ExportPI();
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType)).Where(x => (x.id == 1 || x.id == 2));

            ViewBag.MktPersons = MarketingAccount.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oExportPI);
        }


        public ActionResult PrintExportPISummaryReport(string sParams, double nts)
        {
            List<ExportPISummary> oExportPISummarys = new List<ExportPISummary>();

            oExportPISummarys = this.GetExportPISummaryList(sParams);

            int nTexUnit = 0;
            if (!string.IsNullOrEmpty(sParams))
            {
                nTexUnit = Convert.ToInt32(sParams.Split('~')[0]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptExportPISummary oReport = new rptExportPISummary();
            byte[] abytes = oReport.PrepareReport(oExportPISummarys, oCompany, nTexUnit);
            return File(abytes, "application/pdf");

        }


        public void PrintExportPISummaryReportXL1(string sParams)
        {
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            ExportPISummary oExportPISummary = new ExportPISummary();
            List<ExportPISummary> oExportPISummarys = new List<ExportPISummary>();

            oExportPISummarys = this.GetExportPISummaryList(sParams);

            int nBUUnit = 0;
            if (!string.IsNullOrEmpty(sParams))
            {
                nBUUnit = Convert.ToInt32(sParams.Split('~')[0]);
            }

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export PI Summary List");
                sheet.Name = "Export PI Summary";

                if (nBUUnit == 1)//Yarn dyheing
                {
                    sheet.Column(2).Width = 10; //SL
                    sheet.Column(3).Width = 22; //PINo
                    sheet.Column(4).Width = 13; //PIDate
                    sheet.Column(5).Width = 40; //BuyerName

                    sheet.Column(6).Width = 20; //GarmentsName
                    sheet.Column(7).Width = 20; //Construction
                    sheet.Column(8).Width = 20; //ProcessType
                    sheet.Column(9).Width = 20; //FabricWeave
                    sheet.Column(10).Width = 20; //FinishType
                    sheet.Column(11).Width = 20; //StyleNo
                    sheet.Column(12).Width = 20; //ColorInfo
                    sheet.Column(13).Width = 25; //BuyerReference

                    sheet.Column(14).Width = 14; //DaysToExpire
                    sheet.Column(15).Width = 22; //LCNo
                    sheet.Column(16).Width = 13; //LCDate
                    sheet.Column(17).Width = 22; //FileNo
                    sheet.Column(18).Width = 12; //LCAmdNo
                    sheet.Column(19).Width = 13; //LCAmdDate
                    sheet.Column(20).Width = 13; //LCRecivedDate
                    sheet.Column(21).Width = 13; //ExpiryDate
                    sheet.Column(22).Width = 22; //InvoiceNo
                    sheet.Column(23).Width = 13; //InvoiceDate
                    sheet.Column(24).Width = 14; //ShipmentDate
                    sheet.Column(25).Width = 35; //NameDays
                    sheet.Column(26).Width = 40; //BankName
                    sheet.Column(27).Width = 40; //BranchName
                    sheet.Column(28).Width = 40; //MktPersonName
                    sheet.Column(29).Width = 40; //ProductName
                    sheet.Column(30).Width = 10; //Qty
                    sheet.Column(31).Width = 10; //Rate
                    sheet.Column(32).Width = 10; //Value
                    sheet.Column(33).Width = 22; //DONo
                    sheet.Column(34).Width = 13; //DODate
                    sheet.Column(35).Width = 10; //DoQty

                    sheet.Column(36).Width = 40; //PI Remark
                }
                else
                {
                    sheet.Column(2).Width = 10; //SL
                    sheet.Column(3).Width = 22; //PINo
                    sheet.Column(4).Width = 13; //PIDate
                    sheet.Column(5).Width = 40; //BuyerName

                    sheet.Column(6).Width = 14; //DaysToExpire
                    sheet.Column(7).Width = 22; //LCNo
                    sheet.Column(8).Width = 13; //LCDate
                    sheet.Column(9).Width = 22; //FileNo
                    sheet.Column(10).Width = 12; //LCAmdNo
                    sheet.Column(11).Width = 13; //LCAmdDate
                    sheet.Column(12).Width = 13; //LCRecivedDate
                    sheet.Column(13).Width = 13; //ExpiryDate
                    sheet.Column(14).Width = 22; //InvoiceNo
                    sheet.Column(15).Width = 13; //InvoiceDate
                    sheet.Column(16).Width = 14; //ShipmentDate
                    sheet.Column(17).Width = 35; //NameDays
                    sheet.Column(18).Width = 40; //BankName
                    sheet.Column(19).Width = 40; //BranchName
                    sheet.Column(20).Width = 40; //MktPersonName
                    sheet.Column(21).Width = 40; //ProductName
                    sheet.Column(22).Width = 10; //Qty
                    sheet.Column(23).Width = 10; //Rate
                    sheet.Column(24).Width = 10; //Value
                    sheet.Column(25).Width = 22; //DONo
                    sheet.Column(26).Width = 13; //DODate
                    sheet.Column(27).Width = 10; //DoQty

                    sheet.Column(28).Width = 40; //PI Remark
                }


                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 27].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 27].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Export PI Summary List"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Column Header

                if (nBUUnit == 1)
                {
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Garments Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Process Type"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Weave Type"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Finish Type"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "Color Info"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Buyer Reference"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Days To Expire"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "File No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "LC Amd No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = "LC Amd Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 20]; cell.Value = "LC Recived Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 21]; cell.Value = "Expiry Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 22]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 23]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 24]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 25]; cell.Value = "Name Days"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 26]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 27]; cell.Value = "Branch Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 28]; cell.Value = "Mkt Person Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 29]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 30]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 31]; cell.Value = "Rate"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 32]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 33]; cell.Value = "DO No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 34]; cell.Value = "DO Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 35]; cell.Value = "DO Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 36]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                else
                {
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Days To Expire"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "File No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "LC Amd No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "LC Amd Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "LC Recived Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Expiry Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "Name Days"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = "Branch Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 20]; cell.Value = "Mkt Person Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 21]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 22]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 23]; cell.Value = "Rate"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 24]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 25]; cell.Value = "DO No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 26]; cell.Value = "DO Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 27]; cell.Value = "DO Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 28]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex = rowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                double nTotalQty = 0, nTotalValue = 0, nTotalDoQty = 0;
                if (nBUUnit == 1)//Yarn dyeing
                {
                    foreach (ExportPISummary oItem in oExportPISummarys)
                    {
                        nCount++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.PIDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.GarmentsName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.ProcessType; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.FabricWeave; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.FinishType; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.ColorInfo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.BuyerReference; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = oItem.DaysToExpire.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = oItem.LCDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 17]; cell.Value = oItem.FileNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = oItem.LCAmdNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 19]; cell.Value = oItem.LCAmdDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 20]; cell.Value = oItem.LCRecivedDateST; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 21]; cell.Value = oItem.ExpiryDateSt; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 22]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 23]; cell.Value = oItem.InvoiceDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 24]; cell.Value = oItem.ShipmentDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 25]; cell.Value = oItem.NameDaysInString; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 26]; cell.Value = oItem.BankName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 27]; cell.Value = oItem.BranchName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 28]; cell.Value = oItem.MktPersonName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 29]; cell.Value = oItem.ProductName + " " + oItem.ProductDescription; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalQty += oItem.Qty;
                        cell = sheet.Cells[rowIndex, 30]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 31]; cell.Value = oItem.Rate; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalValue += oItem.Value;
                        cell = sheet.Cells[rowIndex, 32]; cell.Value = oItem.Value; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 33]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 34]; cell.Value = oItem.DODateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalDoQty += oItem.DoQty;
                        cell = sheet.Cells[rowIndex, 35]; cell.Value = oItem.DoQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 36]; cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                }
                else
                {
                    foreach (ExportPISummary oItem in oExportPISummarys)
                    {
                        nCount++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.PIDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.DaysToExpire.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.LCDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.FileNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.LCAmdNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.LCAmdDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.LCRecivedDateST; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.ExpiryDateSt; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = oItem.InvoiceDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = oItem.ShipmentDateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 17]; cell.Value = oItem.NameDaysInString; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = oItem.BankName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 19]; cell.Value = oItem.BranchName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 20]; cell.Value = oItem.MktPersonName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 21]; cell.Value = oItem.ProductName + " " + oItem.ProductDescription; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalQty += oItem.Qty;
                        cell = sheet.Cells[rowIndex, 22]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 23]; cell.Value = oItem.Rate; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalValue += oItem.Value;
                        cell = sheet.Cells[rowIndex, 24]; cell.Value = oItem.Value; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 25]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 26]; cell.Value = oItem.DODateInStr; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalDoQty += oItem.DoQty;
                        cell = sheet.Cells[rowIndex, 27]; cell.Value = oItem.DoQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 28]; cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                }
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportPISummary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        private List<ExportPISummary> GetExportPISummaryList(string sParams)
        {
            List<ExportPISummary> oExportPISummarys = new List<ExportPISummary>();
            int nTexUnit = 0,
            nBuyerID = 0,
            nMktPersonID = 0;

            string sPINo = "",
            sPIStartDate = "",
            sPIEndDate = "",
            sLCRecDateFrom = "",
            sLCRecDateTo = "",
            sDOIssueDateFrom = "",
            sDOIssueDateTo = "";

            bool IsDate = true,
            IsRecDate = true,
            IsDOIssueDate = true;

            if (!string.IsNullOrEmpty(sParams))
            {
                nTexUnit = Convert.ToInt32(sParams.Split('~')[0]);
                sPINo = Convert.ToString(sParams.Split('~')[1]);
                nBuyerID = Convert.ToInt32(sParams.Split('~')[2]);
                nMktPersonID = Convert.ToInt32(sParams.Split('~')[3]);

                IsDate = Convert.ToBoolean(sParams.Split('~')[4]);
                sPIStartDate = Convert.ToString(sParams.Split('~')[5]);
                sPIEndDate = Convert.ToString(sParams.Split('~')[6]);

                IsRecDate = Convert.ToBoolean(sParams.Split('~')[7]);
                sLCRecDateFrom = Convert.ToString(sParams.Split('~')[8]);
                sLCRecDateTo = Convert.ToString(sParams.Split('~')[9]);

                IsDOIssueDate = Convert.ToBoolean(sParams.Split('~')[10]);
                sDOIssueDateFrom = Convert.ToString(sParams.Split('~')[11]);
                sDOIssueDateTo = Convert.ToString(sParams.Split('~')[12]);
            }
            oExportPISummarys = ExportPISummary.Gets(nTexUnit, sPINo, nBuyerID, nMktPersonID, sPIStartDate, sPIEndDate, sLCRecDateFrom, sLCRecDateTo, sDOIssueDateFrom, sDOIssueDateTo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return oExportPISummarys;
        }
        #endregion

        #region 



        [HttpPost]
        public JsonResult GetsBuyerExportPIForPayment(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPI WHERE Amount > 0 AND ExportPIID<>0 AND BuyerID=" + oExportPI.BuyerID + "";
                if (oExportPI.PaymentTypeInInt > 0) { sSQL = sSQL + " AND PaymentType = " + oExportPI.PaymentTypeInInt + " "; }
                if (oExportPI.IsApproved) { sSQL = sSQL + " AND ApprovedBy != 0 "; }
                if (!string.IsNullOrEmpty(oExportPI.PINo)) { sSQL = sSQL + " AND PINo LIKE '%" + oExportPI.PINo + "%'"; }
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<PaymentPI> oPaymentPIs = new List<PaymentPI>();
                sSQL = "SELECT * FROM View_PaymentPI WHERE PIID IN (" + GetExportPIIDs(_oExportPIs) + ") ";
                oPaymentPIs = PaymentPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<PaymentPI> oPPIs = new List<PaymentPI>();
                List<ExportPI> oEPIs = new List<ExportPI>();
                foreach (ExportPI oItem in _oExportPIs)
                {
                    oPPIs = oPaymentPIs.Where(o => o.PIID == oItem.ExportPIID).ToList();
                    if (oPPIs.Count > 0)
                    {
                        var TotalAmount = oPPIs.Select(o => o.Amount).Sum();
                        if (oItem.Amount > TotalAmount)
                        {
                            oEPIs.Add(oItem);
                        }
                    }
                    else
                    {
                        oEPIs.Add(oItem);
                    }
                }
                _oExportPIs = new List<ExportPI>();
                _oExportPIs = oEPIs;
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetExportPIIDs(List<ExportPI> oExportPIs)
        {
            string result = "";
            foreach (ExportPI oItem in oExportPIs)
            {
                result = oItem.ExportPIID + "," + result;
            }
            result = result.Remove((result.Length - 1), 1);
            return result;
        }
        [HttpPost]
        public JsonResult GetsBuyersExportPIs(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                //Here BuyerIDs in oExportPI.Note field;
                string sSQL = "SELECT * FROM View_ExportPI WHERE ApprovedBy > 0 AND PaymentType = " + (int)EnumPIPaymentType.NonLC + "";
                if (!string.IsNullOrEmpty(oExportPI.Note)) { sSQL = sSQL + " AND ExportPIID<>0 AND BuyerID IN (" + oExportPI.Note + ")"; }
                if (!string.IsNullOrEmpty(oExportPI.PINo)) { sSQL = sSQL + " AND PINo LIKE '%" + oExportPI.PINo + "%'"; }
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       

        public ActionResult ViewLocalBillStatement(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportPI = new ExportPI();
            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType)).Where(x => (x.id == 1 || x.id == 2));
            ViewBag.MktPersons = MarketingAccount.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oExportPI);
        }
     
        public ActionResult PrintLocalBillStatementReportXL(string sParams)
        {
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ExportPILocalBillStatementXL>));

            ExportPI oExportPI = new ExportPI();
            List<ExportPI> oExportPIs = new List<ExportPI>();

            oExportPIs = this.GetLBSRs(sParams);

            int nCount = 0;
            double nTotalQty = 0, nTotalAmount = 0;
            ExportPILocalBillStatementXL oExportPILocalBillStatementXL = new ExportPILocalBillStatementXL();
            List<ExportPILocalBillStatementXL> oExportPILocalBillStatementXLs = new List<ExportPILocalBillStatementXL>();
            if (oExportPIs.Count > 0)
            {
                foreach (ExportPI oItem in oExportPIs)
                {
                    nCount++;
                    oExportPILocalBillStatementXL = new ExportPILocalBillStatementXL();
                    oExportPILocalBillStatementXL.SL = nCount.ToString();
                    oExportPILocalBillStatementXL.BillNo = oItem.PINo;
                    oExportPILocalBillStatementXL.IssueDate = oItem.IssueDateInString;
                    oExportPILocalBillStatementXL.BuyerName = oItem.BuyerName;
                    oExportPILocalBillStatementXL.BankName = oItem.BankName;
                    oExportPILocalBillStatementXL.AccountName = oItem.AccountName;
                    nTotalQty += oItem.Qty;
                    oExportPILocalBillStatementXL.Qty = Global.MillionFormat(oItem.Qty);
                    nTotalAmount += oItem.Amount;
                    oExportPILocalBillStatementXL.Amount = Global.MillionFormat(oItem.Amount);
                    oExportPILocalBillStatementXLs.Add(oExportPILocalBillStatementXL);
                }

                #region Total
                oExportPILocalBillStatementXL = new ExportPILocalBillStatementXL();
                oExportPILocalBillStatementXL.SL = "";
                oExportPILocalBillStatementXL.BillNo = "";
                oExportPILocalBillStatementXL.IssueDate = "";
                oExportPILocalBillStatementXL.BuyerName = "";
                oExportPILocalBillStatementXL.BankName = "";
                oExportPILocalBillStatementXL.AccountName = "Total : ";
                oExportPILocalBillStatementXL.Qty = Global.MillionFormat(nTotalQty);
                oExportPILocalBillStatementXL.Amount = Global.MillionFormat(nTotalAmount);
                oExportPILocalBillStatementXLs.Add(oExportPILocalBillStatementXL);
                #endregion
            }
            else
            {
                oExportPILocalBillStatementXL = new ExportPILocalBillStatementXL();
                oExportPILocalBillStatementXL.IssueDate = "No List Found";
                oExportPILocalBillStatementXLs.Add(oExportPILocalBillStatementXL);
            }
            serializer.Serialize(stream, oExportPILocalBillStatementXLs);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "Export PI Summary.xls");
        }
        private List<ExportPI> GetLBSRs(string sParams)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            int nBUID = 0,
            nMktPersonID = 0,
            nBuyerID = 0;

            bool IsIssueDate = true;

            string sIssueDateFromLBSR = "",
            sIssueDateToLBSR = "";

            if (!string.IsNullOrEmpty(sParams))
            {
                nBUID = Convert.ToInt32(sParams.Split('~')[0]);
                nMktPersonID = Convert.ToInt32(sParams.Split('~')[1]);
                nBuyerID = Convert.ToInt32(sParams.Split('~')[2]);

                IsIssueDate = Convert.ToBoolean(sParams.Split('~')[3]);
                sIssueDateFromLBSR = Convert.ToString(sParams.Split('~')[4]);
                sIssueDateToLBSR = Convert.ToString(sParams.Split('~')[5]);
            }

            string sReturn1 = "SELECT * FROM View_ExportPI ";
            string sReturn = "";

            #region Textile Unit
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " BUID = " + nBUID;
            #endregion

            #region PaymentType
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " PaymentType = " + (int)EnumPIPaymentType.NonLC;
            #endregion

            #region Mkt Person
            if (nMktPersonID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MKTEmpID = " + nMktPersonID;
            }
            #endregion

            #region Buyer
            if (nBuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID = " + nBuyerID;
            }
            #endregion

            #region Issue Date
            if (IsIssueDate)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + sIssueDateFromLBSR + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + sIssueDateToLBSR + "',106)) ";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn;

            oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return oExportPIs;
        }
     
      
        [HttpPost]
        public JsonResult GetsPIBySearchingCriterias(ExportPI oExportPI)
        {
            _oExportPI = new ExportPI();
            try
            {
                string sSQL = this.MakeSqlForSearchingCriterias(oExportPI);
                _oExportPIs = new List<ExportPI>();
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                _oExportPI = new ExportPI();
                _oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(_oExportPI);
            }
            var jsonResult = Json(_oExportPIs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public string MakeSqlForSearchingCriterias(ExportPI oExportPI)
        {
            //Ratin
            string sReturn1 = "SELECT * FROM View_ExportPI ";
            string sReturn = "";


            #region PI No
            if (!string.IsNullOrEmpty(oExportPI.PINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PINo LIKE '%" + oExportPI.PINo + "%' ";
            }
            #endregion

            #region Payment Type
            if (oExportPI.PaymentTypeInInt > (int)EnumPIPaymentType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PaymentType = " + oExportPI.PaymentTypeInInt;
            }
            #endregion

            #region Textile Unit
            if (oExportPI.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oExportPI.BUID;
            }
            #endregion

            #region Buyer ID
            if (oExportPI.BuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID = " + oExportPI.BuyerID;
            }
            #endregion

            #region Contractor ID
            if (oExportPI.ContractorID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID = " + oExportPI.ContractorID;
            }
            #endregion

            #region PI Status
            if (oExportPI.PIStatus > (int)EnumPIStatus.Initialized)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PIStatus IN (" + (int)oExportPI.PIStatus + ")";
            }
            #endregion

            #region LC ID
            if (oExportPI.LCID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCID = " + oExportPI.LCID;
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " AND PIStatus < " + (int)EnumPIStatus.Cancel + " ORDER BY PINo";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetsPIForFEO(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                //string sSQL = "SELECT * FROM View_ExportPI WHERE TextileUnit=" + (int)EnumTextileUnit.Weaving + " AND ExportPIID NOT IN (SELECT PIID FROM FabricExecutionOrder) AND BuyerID = " + oExportPI.BuyerID;
                string sSQL = "SELECT * FROM View_ExportPI WHERE BUID=" + oExportPI.BUID+ " AND BuyerID = " + oExportPI.BuyerID;

                if (!string.IsNullOrEmpty(oExportPI.PINo))
                {
                    sSQL = sSQL + " AND PINo LIKE '%" + oExportPI.PINo + "%'";
                }
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                oExportPI = new ExportPI();
                oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(oExportPI);
            }
            var jsonResult = Json(_oExportPIs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        [HttpPost]
        public JsonResult GetsWUPI(ExportPI oExportPI)
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPI WHERE ISNULL(BUID,0) =" + oExportPI.BUID + " AND BuyerID = " + oExportPI.BuyerID;
                _oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIs = new List<ExportPI>();
                oExportPI = new ExportPI();
                oExportPI.ErrorMessage = ex.Message;
                _oExportPIs.Add(oExportPI);
            }
            var jsonResult = Json(_oExportPIs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.ExportPI, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.ExportPI, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                //if (oProducts.Count <= 0)
                //{
                //    oProducts = Product.GetsByBU(oProduct.BUID, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}

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
       
        [HttpPost]
        public JsonResult GetSampleInvoices(ExportPI oExportPI)
        {
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            string sSQL = "Select top(200)* from View_SampleInvoice where  InvoiceType in (" + (int)EnumSampleInvoiceType.None +","+(int)EnumSampleInvoiceType.SampleInvoice + ")  and PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + ")";

            //if (!String.IsNullOrEmpty(oSampleInvoiceDetail.OrderNo)) sSQL = sSQL + " And RouteSheetNo Like '%" + oSampleInvoiceDetail.OrderNo + "%'";
            //if (!string.IsNullOrEmpty(oExportPI.ContractorName)) sSQL = sSQL + " And ContractorID in (" + oExportPI.ContractorName+ ")";
            if (oExportPI.ContractorID > 0) sSQL = sSQL + " And ContractorID in (" + oExportPI.ContractorID + "," + oExportPI.BuyerID + ")";
            if (oExportPI.ExportPIID > 0 && oExportPI.IsApproved == false) sSQL = sSQL + " And isnull(ExportPIID,0) In (" + oExportPI.ExportPIID + ")";
            if (oExportPI.ExportPIID == 0) sSQL = sSQL + " And  isnull(ExportPIID,0)=0";
            if (oExportPI.ExportPIID > 0 && oExportPI.IsApproved == true) sSQL = sSQL + " And isnull(ExportPIID,0) In (0," + oExportPI.ExportPIID + ")";
            sSQL = sSQL + " order by SampleInvoiceDate ASC";
            oSampleInvoices = SampleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oSampleInvoices.Count() > 0)
            {
                sSQL = "select * from View_DyeingOrderReport Where SampleInvoiceID>0 and SampleInvoiceID In (" + string.Join(",", oSampleInvoices.Select(x => x.SampleInvoiceID).ToList()) + ")";
                oDyeingOrderReports=DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oSampleInvoiceDetails = oDyeingOrderReports.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) =>
                                                 new SampleInvoiceDetail
                                                 {
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Qty = grp.Sum(p => p.Qty),
                                                     Amount = grp.Sum(p => p.Amount),
                                                     UnitPrice = grp.Sum(p => p.Amount) / grp.Sum(p => p.Qty),
                                                 }).ToList();
                oSampleInvoices[0].SampleInvoiceDetails = oSampleInvoiceDetails;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetDODetails(List<SampleInvoice> oSampleInvoices)
        {
            //List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            string sSQL = "";
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            if (oSampleInvoices != null)
            {
                if (oSampleInvoices.Count() > 0)
                {
                    sSQL = "select * from View_DyeingOrderReport Where SampleInvoiceID>0 and SampleInvoiceID In (" + string.Join(",", oSampleInvoices.Select(x => x.SampleInvoiceID).ToList()) + ")";
                    oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oSampleInvoiceDetails = oDyeingOrderReports.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) =>
                                                     new SampleInvoiceDetail
                                                     {
                                                         ProductID = key.ProductID,
                                                         ProductName = key.ProductName,
                                                         Qty = grp.Sum(p => p.Qty),
                                                         Amount = grp.Sum(p => p.Amount),
                                                         UnitPrice = grp.Sum(p => p.Amount) / grp.Sum(p => p.Qty),
                                                     }).ToList();
                    //oSampleInvoices[0].SampleInvoiceDetails = oSampleInvoiceDetails;
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        //NAZMUL
        [HttpPost]
        public JsonResult GetAllExportPIShipment(ExportPI oExportPI)
        {
            List<ExportPIShipment> oExportPIShipments = new List<ExportPIShipment>();
            List<ExportPIShipment> _tempExportPIShipments = new List<ExportPIShipment>();

            ExportPIShipment _ExportPIShipment = new ExportPIShipment();
            try
            {
                string sSQL = "";
                string sParamName = "";

                sParamName = oExportPI.ContractorID.ToString();
              
                if (oExportPI.ExportPIID > 0)
                {
                    sSQL = "SELECT * FROM View_ExportPIShipment WHERE ExportPIID IN(0," + oExportPI.ExportPIID + ")";
                }
                else
                {
                     sSQL = "SELECT * FROM View_ExportPIShipment WHERE ExportPIID = 0";
                }

                if (!string.IsNullOrEmpty(oExportPI.ErrorMessage))
                {
                    sSQL += " AND ShipmentBy LIKE'%" + oExportPI.ErrorMessage + "%'";
                }

                if (oExportPI.ContractorID>0)
                {
                    sSQL += " and ExportPIID =0 or ExportPIID in (Select max(ExportPIID) from ExportPIShipment where ExportPIID in (Select ExportPIID from ExportPI where ContractorID=" + oExportPI.ContractorID + "))";
                }


                oExportPIShipments = ExportPIShipment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportPIShipment oItem in oExportPIShipments)
                {
                    _ExportPIShipment = new ExportPIShipment();
                    _ExportPIShipment.ExportPIShipmentID = oItem.ExportPIShipmentID;
                    _ExportPIShipment.ExportBillID = oItem.ExportBillID;
                    _ExportPIShipment.ExportPIID = oItem.ExportPIID;
                    _ExportPIShipment.ShipmentBy = oItem.ShipmentBy;
                    _ExportPIShipment.DestinationPort = oItem.DestinationPort;
                    if (oItem.ExportPIID == oExportPI.ExportPIID)
                    {
                        oItem.Remarks = "Already Exist";
                    }
                    _ExportPIShipment.Remarks = oItem.Remarks;
                    _tempExportPIShipments.Add(_ExportPIShipment);
                }
            }
            catch (Exception ex)
            {
                oExportPIShipments = new List<ExportPIShipment>();
                oExportPIShipments.Add(new ExportPIShipment() { ErrorMessage = ex.Message });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_tempExportPIShipments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      

        #region ExportPIDescription
     

        #endregion

        #region Print
        [HttpPost]
        public ActionResult SetExportPIListData(ExportPI oExportPI)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportPI);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetByExportPISignatory(ExportPISignatory oExportPISignatory)
        {
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            _oExportPISignatorys = new List<ExportPISignatory>();
            try
            {
                _oExportPISignatorys = ExportPISignatory.Gets(oExportPISignatory.ExportPIID, (int)Session[SessionInfo.currentUserID]);
                string sApprovalHeadIDs = string.Join(",", _oExportPISignatorys.Where(p => p.ApprovalHeadID > 0).ToList().Select(x => x.ApprovalHeadID).ToList());
                if (string.IsNullOrEmpty(sApprovalHeadIDs)) { sApprovalHeadIDs = "0"; }
                oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ApprovalHeadID not in (" + sApprovalHeadIDs + ") and ModuleID = " + (int)EnumModuleName.ExportPI + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);
                foreach (ApprovalHead oitem in oApprovalHeads)
                {
                    _oExportPISignatory = new ExportPISignatory();
                    _oExportPISignatory.ExportPIID = oExportPISignatory.ExportPIID;
                    _oExportPISignatory.RequestTo = 0;
                    _oExportPISignatory.Name_Request = "";
                    _oExportPISignatory.ApproveDate = DateTime.MinValue;
                    _oExportPISignatory.ApprovalHeadID = oitem.ApprovalHeadID;
                    _oExportPISignatory.HeadName = oitem.Name;
                    _oExportPISignatory.SLNo = oitem.Sequence;  // oApprovalHeads.Where(p => p.ApprovalHeadID == oitem.ApprovalHeadID).FirstOrDefault().Sequence;
                    _oExportPISignatorys.Add(_oExportPISignatory);
                }
                _oExportPISignatorys = _oExportPISignatorys.OrderBy(x => x.SLNo).ToList();
            }
            catch (Exception ex)
            {
                //oExportPISignatoryComment = new ExportPISignatoryComment();
                _oExportPISignatorys = new List<ExportPISignatory>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPISignatorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintExportPIs()
        {
            _oExportPIs = new List<ExportPI>();
            try
            {
                _oExportPI = (ExportPI)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_ExportPI WHERE ExportPIID IN (" + _oExportPI.Note + ") Order By ExportPIID";
               _oExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportPI = new ExportPI();
                _oExportPIs = new List<ExportPI>();
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oExportPI.Company = oCompany;

            string Messge = "Export PI List";
            rptExportPIs oReport = new rptExportPIs();
            byte[] abytes = oReport.PrepareReport(_oExportPIs, Messge, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintoExportPIPreview(int id, bool bPrintFormat, int nTitleTypeInImg)
        {
            _oExportPI = new ExportPI();
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.ExportPIDetails = ExportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ExportPIShipment _oExportPIShipment = new ExportPIShipment();
            _oExportPIShipment = _oExportPIShipment.GetByExportPIID(_oExportPI.ExportPIID, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_ExportPITandCClause WHERE ExportPIID = " + id + " AND DocFor IN (" + ((int)EnumDocFor.PI).ToString() + ")";
            _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oExportPI.PISizerBreakDowns = PISizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.SizeCategories = GetDistictSizes(_oExportPI.PISizerBreakDowns);
            _oExportPI.ColorCategories = GetDistictColorWithProducts(_oExportPI.PISizerBreakDowns);
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            _oExportPI.ExportPIPrintSetup = oExportPIPrintSetup.Get(_oExportPI.ExportPIPrintSetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oExportPIShipment.ExportPIShipmentID>0)
            {
                _oExportPI.ExportPIPrintSetup.ShipmentBy = _oExportPIShipment.ShipmentBy;
                _oExportPI.ExportPIPrintSetup.PlaceOfDelivery = _oExportPIShipment.DestinationPort;
            }

            _oExportPI.CurrentUserId = ((User)Session[SessionInfo.CurrentUser]).UserID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);           

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oExportPI.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (!String.IsNullOrEmpty(oContractor.Address3))
            {
                _oExportPI.ContractorAddress = oContractor.Address;
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(_oExportPI.BUID, (int)Session[SessionInfo.currentUserID]);

            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                if (_oExportPI.ExportPIID > 0)
                {
                    sSQL = "Select * from View_SampleInvoice where ExportPIID=" + _oExportPI.ExportPIID + " and CurrentStatus in (" + (int)EnumSampleInvoiceStatus.Approved + ") and InvoiceType in (" + (int)EnumSampleInvoiceType.None + "," + (int)EnumSampleInvoiceType.SampleInvoice + ")  and PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + ")";
                    sSQL = sSQL + " order by SampleInvoiceDate ASC";
                    oSampleInvoices = SampleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oSampleInvoices.Count > 0)
                    {
                        _oExportPI.OrderSheetNo = string.Join(",", oSampleInvoices.Select(x => x.SampleInvoiceNo).ToList());
                    }
                }

                //rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                //byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                //return File(abytes, "application/pdf");
                if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.A)
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.B)
                {
                    if (!bPrintFormat)
                    {
                        _oExportPI.ExportPIDetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));
                        _oExportPI.ExportPIDetails.ForEach(o => o.UnitPrice = (o.UnitPrice / oMeasurementUnitCon.Value));
                    }
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_WUTwo(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.D)
                {
                    AttachDocument oAttachDocument = new AttachDocument();
                    oAttachDocument = oAttachDocument.GetUserSignature(_oExportPI.ApprovedBy, (int)Session[SessionInfo.currentUserID]);
                    _oExportPI.Signature = this.GetSignature(oAttachDocument);
                    if (!bPrintFormat)
                    {
                        _oExportPI.ExportPIDetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));
                        _oExportPI.ExportPIDetails.ForEach(o => o.UnitPrice = (o.UnitPrice / oMeasurementUnitCon.Value));
                    }
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_D(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else
                {

                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }

            }
            else if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving)
            {
                rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                byte[] abytes = oReport.PrepareReport_WU(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
            else
            {
                AttachDocument oAttachDocument = new AttachDocument();                
                oAttachDocument = oAttachDocument.GetUserSignature(_oExportPI.ApprovedBy, (int)Session[SessionInfo.currentUserID]);
                _oExportPI.Signature = this.GetSignature(oAttachDocument);
                if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Plastic || oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Integrated)
                {
                    _oExportPI.IsPrintAC = false;
                }

                rptExportPI oReport = new rptExportPI();//for plastic and integrated
                byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
        }
        [HttpPost]
        public JsonResult Delete(ExportPISignatory oExportPISignatory)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportPISignatory.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetUesrsByName(User oUser)
        {
            List<User> oUsers = new List<User>();
            try
            {
                string sSQL = "Select * from View_User Where UserID<>0 ";
                if (!String.IsNullOrEmpty(oUser.UserName))
                {
                    oUser.UserName = oUser.UserName.Trim();
                    sSQL = sSQL + "And UserName+LogInID like '%" + oUser.UserName + "%'";
                }
                else
                {
                    sSQL = sSQL + "and UserID in (SELECT UserID FROM ApprovalHeadPerson WHERE ApprovalHeadID in (SELECT ApprovalHeadID FROM ApprovalHead WHERE ModuleID =" + (int)EnumModuleName.NOA + "))";
                }
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oUsers.Count <= 0)
                {
                    throw new Exception("No information found");
                }
            }
            catch (Exception ex)
            {
                oUsers = new List<User>();
                oUser.ErrorMessage = ex.Message;
                oUsers.Add(oUser);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintoExportPIPreviewTwo(int id, bool bPrintFormat, int nTitleTypeInImg)
        {
            _oExportPI = new ExportPI();
            _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.ExportPIDetails = ExportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT * FROM View_ExportPITandCClause WHERE ExportPIID = " + id + " AND DocFor IN (" + ((int)EnumDocFor.PI).ToString() + ")";
            _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.PISizerBreakDowns = PISizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.SizeCategories = GetDistictSizes(_oExportPI.PISizerBreakDowns);
            _oExportPI.ColorCategories = GetDistictColorWithProducts(_oExportPI.PISizerBreakDowns);
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            _oExportPI.ExportPIPrintSetup = oExportPIPrintSetup.Get(_oExportPI.ExportPIPrintSetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.CurrentUserId = ((User)Session[SessionInfo.CurrentUser]).UserID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            //UserImage oUserImage = new UserImage();
            //oUserImage = oUserImage.GetbyUser(_oExportPI.ApprovedBy, (int)EnumUserImageType.Signature, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oExportPI.Signature = GetSignature(oUserImage);

            AttachDocument oAttachDocument = new AttachDocument();
            if (_oExportPI.ApprovedBy != 0)
            {
                oAttachDocument = oAttachDocument.GetUserSignature(_oExportPI.ApprovedBy, (int)Session[SessionInfo.currentUserID]);
                _oExportPI.Signature = this.GetSignature(oAttachDocument);
            }


            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oExportPI.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (!String.IsNullOrEmpty(oContractor.Address3))
            {
                _oExportPI.ContractorAddress = oContractor.Address;
            }
            
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(_oExportPI.BUID, (int)Session[SessionInfo.currentUserID]);

            List<FabricSalesContract> oFabricSalesContracts = new List<FabricSalesContract>();
            oFabricSalesContracts = FabricSalesContract.GetsByPI(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach ( FabricSalesContract oFabricSalesContract in   oFabricSalesContracts)
            {
                _oExportPI.OrderSheetNo = oFabricSalesContract.SCNoFull + "," + _oExportPI.OrderSheetNo;
            }
            if(_oExportPI.OrderSheetNo.Length>0)
            {
                _oExportPI.OrderSheetNo = _oExportPI.OrderSheetNo.Remove(_oExportPI.OrderSheetNo.Length - 1, 1);
            }


            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
            oAttachDocuments = AttachDocument.Gets_WithAttachFile(_oExportPI.BUID, (int)EnumAttachRefType.BusinessUnit_Footer, (int)Session[SessionInfo.currentUserID]);
            if (oAttachDocuments.Count > 0)
            {
                _oExportPI.BU_Footer = GetBU_Footer(oAttachDocuments[0]);
            }

            if (!bPrintFormat)
            {
                _oExportPI.ExportPIDetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));
                _oExportPI.ExportPIDetails.ForEach(o => o.UnitPrice = (o.UnitPrice / oMeasurementUnitCon.Value));
            }

            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
               
            

                if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.A)
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.B)
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_WUTwo(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_WUTwo(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }

            }
            else if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving || oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing)
            {

                if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.A)
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_WUOne(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.B)
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_WUTwo(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else if (_oExportPI.ExportPIPrintSetup.PrintNo == (int)EnumExcellColumn.C)
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_WU_Type_C(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                    byte[] abytes = oReport.PrepareReport_WUOne(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                    return File(abytes, "application/pdf");
                }

              
            }
            else
            {
                //AttachDocument oAttachDocument = new AttachDocument();
                //oAttachDocument = oAttachDocument.GetUserSignature(_oExportPI.ApprovedBy, (int)Session[SessionInfo.currentUserID]);
                //_oExportPI.Signature = this.GetSignature(oAttachDocument);

                rptExportPI oReport = new rptExportPI();//for plastic and integrated
                byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintBillPreview(int id, bool bPrintFormat, int nTitleTypeInImg)
        {
            _oExportPI = new ExportPI();
            _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.ExportPIDetails = ExportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT * FROM View_ExportPITandCClause WHERE ExportPIID = " + _oExportPI.ExportPIID + " AND DocFor IN (" + ((int)EnumDocFor.Bill).ToString() + ")";
            _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            _oExportPI.ExportPIPrintSetup = oExportPIPrintSetup.Get(true, _oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.CurrentUserId = ((User)Session[SessionInfo.CurrentUser]).UserID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);

            AttachDocument oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oExportPI.ApprovedBy, (int)Session[SessionInfo.currentUserID]);
            _oExportPI.Signature = this.GetSignature(oAttachDocument);

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oExportPI.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (!String.IsNullOrEmpty(oContractor.Address3))
            {
                _oExportPI.ContractorAddress = oContractor.Address;
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Plastic || oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Integrated)
            {
                _oExportPI.IsPrintAC = false;
            }

            rptExportBill oReport = new rptExportBill();//for plastic and integrated
            byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
            return File(abytes, "application/pdf");
           
        }
        public ActionResult PrintSalesContractPreview(int id, bool bPrintFormat, int nTitleTypeInImg)
        {
            _oExportPI = new ExportPI();
            List<ExportLC> oExportLCs = new List<ExportLC>();
            Contractor oNewContractor = new Contractor();
            _oExportPI = _oExportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.ExportPIDetails = ExportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT * FROM View_ExportPITandCClause WHERE ExportPIID = " + _oExportPI.ExportPIID + " AND DocFor IN (" + ((int)EnumDocFor.SalesContract).ToString() + ")";
            _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.Buyer = oNewContractor.Get(_oExportPI.BuyerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.DeliveryTo = oNewContractor.Get(_oExportPI.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.OrderSheets = new List<OrderSheet>();
            string sPONos = "";
            foreach(OrderSheet oItem in _oExportPI.OrderSheets)
            {
                sPONos = oItem.FullPONo + ", DT. " + oItem.OrderDate.ToString("dd.MM.yyyy");
            }
            string SQL = "SELECT  * FROM View_ExportLC WHERE ExportLCID  = ( SELECT top 1 ExportLCID FROM ExportPILCMapping WHERE ExportPIID = " + _oExportPI.ExportPIID + " )";
            oExportLCs = ExportLC.GetsSQL(SQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oExportLCs.Count>0)
            {
                _oExportPI.ExportLC = oExportLCs[0];
            }
            else
            {
                _oExportPI.ExportLC = new ExportLC();

            }
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            _oExportPI.ExportPIPrintSetup = oExportPIPrintSetup.Get(true, _oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.CurrentUserId = ((User)Session[SessionInfo.CurrentUser]).UserID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);

            AttachDocument oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oExportPI.ApprovedBy, (int)Session[SessionInfo.currentUserID]);
            _oExportPI.Signature = this.GetSignature(oAttachDocument);


            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oExportPI.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (!String.IsNullOrEmpty(oContractor.Address3))
            {
                _oExportPI.ContractorAddress = oContractor.Address;
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptSalesContactPreview oReport = new rptSalesContactPreview();//for plastic and integrated
            byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit, sPONos);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintExportPIPreviewLog(int id, bool bPrintFormat, int nTitleTypeInImg)//id is log id
        {
            _oExportPI = new ExportPI();
            _oExportPI = _oExportPI.GetByLog(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.ExportPIDetails = ExportPIDetail.GetsLogDetail(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT * FROM View_ExportPITandCClauseLog WHERE ExportPILogID = " + id + " AND DocFor IN (" + (int)EnumDocFor.Common + "," + (int)EnumDocFor.PI + ")";
            _oExportPI.ExportPITandCClauses = ExportPITandCClause.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oExportPI.PISizerBreakDowns = PISizerBreakDown.GetsByLog(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oExportPI.SizeCategories = GetDistictSizes(_oExportPI.PISizerBreakDowns);
            //_oExportPI.ColorCategories = GetDistictColorWithProducts(_oExportPI.PISizerBreakDowns);
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            _oExportPI.ExportPIPrintSetup = oExportPIPrintSetup.Get(true, _oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.CurrentUserId = ((User)Session[SessionInfo.CurrentUser]).UserID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);


            UserImage oUserImage = new UserImage();
            oUserImage = oUserImage.GetbyUser(_oExportPI.ApprovedBy, (int)EnumUserImageType.Signature, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportPI.Signature = GetSignature(oUserImage);

            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oExportPI.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (!String.IsNullOrEmpty(oContractor.Address3))
            {
                _oExportPI.ContractorAddress = oContractor.Address;
            }
            //else
            //{
            //    _oExportPI.ContractorAddress = oContractor.Address;
            //}
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
            else if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving)
            {
                rptExportPIForDyeing oReport = new rptExportPIForDyeing();//only for Dyeing
                byte[] abytes = oReport.PrepareReport_WUTwo(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptExportPI oReport = new rptExportPI();//for plastic and integrated
                byte[] abytes = oReport.PrepareReport(_oExportPI, oCompany, bPrintFormat, nTitleTypeInImg, oBusinessUnit);
                return File(abytes, "application/pdf");
            }

        }
        [HttpPost]
        public JsonResult GetsApprovalHead(ApprovalHead oApprovalHead)
        {
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            try
            {
                oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.ExportPI + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oApprovalHeads = new List<ApprovalHead>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveAll(List<ExportPISignatory> oExportPISignatorys)
		{
            _oExportPISignatorys = new List<ExportPISignatory>();
			try
			{
                string sExportPISignatoryIDs = string.Join(",", oExportPISignatorys.Where(p=>p.ExportPISignatoryID>0).ToList().Select(x => x.ExportPISignatoryID).ToList());
                if (string.IsNullOrEmpty(sExportPISignatoryIDs)) { sExportPISignatoryIDs = "0"; }

                _oExportPISignatorys = ExportPISignatory.SaveAll(oExportPISignatorys, sExportPISignatoryIDs, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oExportPISignatory = new ExportPISignatory();
				_oExportPISignatory.ErrorMessage = ex.Message;
                _oExportPISignatorys = new List<ExportPISignatory>();
                _oExportPISignatorys.Add(_oExportPISignatory);
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPISignatorys);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        public Image GetSignature(AttachDocument oAttachDocument)
        {
            if (oAttachDocument.AttachFile != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oAttachDocument.AttachFile);
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
