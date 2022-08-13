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

namespace ESimSolFinancial.Controllers
{
    public class FabricController : Controller
    {
        #region Declaration
        Fabric _oFabric = new Fabric();
        List<Fabric> _oFabrics = new List<Fabric>();
        string _sErrorMessage = "";
        FabricPattern _oFabricPattern = new FabricPattern();
        List<FabricPattern> _oFabricPatterns = new List<FabricPattern>();
        FabricPatternDetail _oFPDetail = new FabricPatternDetail();
        List<FabricPatternDetail> _oFPDetails = new List<FabricPatternDetail>();
        FabricSticker _oFabricSticker = new FabricSticker();
        List<FabricSticker> _oFabricStickers = new List<FabricSticker>();
        FabricPlanning _oFabricPlanning = new FabricPlanning();
        List<FabricPlanning> _oFabricPlannings = new List<FabricPlanning>();
        #endregion

        #region Fabric
        public ActionResult ViewFabrics(int buid,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL="";
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<Product> oProducts = new List<Product>();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            oMarketingAccounts = MarketingAccount.GetsByUser(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabrics = new List<Fabric>();
            if (oMarketingAccounts.Count > 0)
            {
               // oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT top(120)* FROM View_Fabric WHERE  Isnull(ApprovedBy,0)=0 and View_Fabric.MKTPersonID in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") order by IssueDate DESC,Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
            }
            else
            {
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT top(120)* FROM View_Fabric WHERE  Isnull(ApprovedBy,0)=0  order by IssueDate DESC,Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
            }

            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //string sSQL = "SELECT Top(100)* FROM View_Fabric WHERE SubmissionDate IS NOT NULL ORDER BY FabricNo";
            _oFabrics = Fabric.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SwatchTypes = EnumObject.jGets(typeof(EnumSwatchType)); //SwatchTypesObj.Gets();            
            ViewBag.MktPersons = oMarketingAccounts;

            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.BUID = buid;
            ViewBag.Products = oProducts;
            ViewBag.FabricOrderSetups = oFabricOrderSetups;
            ViewBag.CurrentUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;

            //string FOSetupIDs = string.Join(",", (EnumObject.jGets(typeof(EnumFabricRequestType))).Where(x =>
            //                                 x.id!= (int)EnumFabricRequestType.None
            //                           || x.id != (int)EnumFabricRequestType.Sample
            //                           || x.id != (int)EnumFabricRequestType.Bulk   ).Select(x=>x.id.ToString()).ToList());


            ViewBag.FabricOrderSetup = FabricOrderSetup.GetByOrderTypes(buid,false, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<LightSource> oLSs = new List<LightSource>();
            oLSs = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = oLSs;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumMKTRef));
            return View(_oFabrics);
        }
        public ActionResult ViewFabricsRnD(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<Product> oProducts = new List<Product>();

            _oFabrics = new List<Fabric>();
            sSQL = "SELECT top(120)* FROM View_Fabric WHERE FabricOrderType in ("+(int)EnumFabricRequestType.Analysis+") and ReceiveDate is null and Isnull(ApprovedBy,0)!=0  order by IssueDate DESC,Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SwatchTypes = EnumObject.jGets(typeof(EnumSwatchType)); //SwatchTypesObj.Gets();            
            //ViewBag.MktPersons = oMarketingAccounts;
            ViewBag.Products = oProducts;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.BUID = buid;
            ViewBag.CurrentUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            ViewBag.FabricOrderSetups = oFabricOrderSetups;

            List<LightSource> oLSs = new List<LightSource>();
            oLSs = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = oLSs;

            return View(_oFabrics);
        }
        public ActionResult ViewFabricsN(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<Product> oProducts = new List<Product>();

            _oFabrics = new List<Fabric>();
            sSQL = "SELECT top(120)* FROM View_Fabric WHERE FabricOrderType in (" + (int)EnumFabricRequestType.Analysis + ") and ReceiveDate is null and Isnull(ApprovedBy,0)!=0  order by IssueDate DESC,Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SwatchTypes = EnumObject.jGets(typeof(EnumSwatchType)); //SwatchTypesObj.Gets();            
            //ViewBag.MktPersons = oMarketingAccounts;
            ViewBag.Products = oProducts;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.BUID = buid;
            ViewBag.CurrentUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            ViewBag.FabricOrderSetups = oFabricOrderSetups;

            List<LightSource> oLSs = new List<LightSource>();
            oLSs = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = oLSs;

            return View(_oFabrics);
        }
        public ActionResult ViewFabric(int id, int buid)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Fabric).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabric = new Fabric();
          
            if (id > 0)
            {
                _oFabric = _oFabric.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            FabricSCReport oFabricSCReport = new FabricSCReport();
            string sSQL = "";
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            FabricRnD oFabricRnD = new FabricRnD();
            List<Product> oProducts = new List<Product>();
            List<FabricSCReport> _oFabricSCReports = new List<FabricSCReport>();
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabricSCReports = FabricSCReport.Gets("Select top(1)* from View_FabricSalesContractReport where FabricID in (" + _oFabric.FabricID + ") and ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " )  ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            if(_oFabricSCReports.Count>0)
            {
                oFabricSCReport = _oFabricSCReports[0];
            }

            _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricRnD = oFabricRnD.GetBy(oFabricSCReport.FabricSalesContractDetailID, id, ((User)Session[SessionInfo.CurrentUser]).UserID);
          
            ViewBag.FabricRnD = oFabricRnD;
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SwatchTypes = EnumObject.jGets(typeof(EnumSwatchType)); //SwatchTypesObj.Gets();            
            //ViewBag.MktPersons = oMarketingAccounts;
            ViewBag.Products = oProducts;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.BUID = buid;
            ViewBag.CurrentUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            ViewBag.FabricOrderSetups = oFabricOrderSetups;
            ViewBag.FabricPOSetup = oFabricSCReport;
            List<LightSource> oLSs = new List<LightSource>();
            oLSs = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFabric);
        }
      
        private bool ValidateInput(Fabric oFabric)
        {

            if (oFabric.ProductID <= 0)
            {
                _sErrorMessage = "Please Pick, Composition .";
                return false;
            }

            //if (!String.IsNullOrEmpty(oFabric.FabricNo))
            //{

            //    if (oFabric.FabricNo.Length != 9)
            //    {
            //        _sErrorMessage = "Please  Number Must be 9 digit. Like ->YYMMXXXXX Example:(160100001)";
            //        return false;
            //    }
            //    if (!Global.IsNumeric(oFabric.FabricNo))
            //    {
            //        _sErrorMessage = "Please Number Must be a 9-digit neumeric number. Like ->YYMMXXXXX Example:(160100001)";
            //        return false;
            //    }
            //}w

            return true;
        }

        [HttpPost]
        public JsonResult Save(Fabric oFabric)
        {
            try
            {
                oFabric.PriorityLevel = (EnumPriorityLevel)oFabric.PriorityLevelInInt;
                if(!String.IsNullOrEmpty(oFabric.Construction))
                     oFabric.Construction = oFabric.Construction.Trim();

                if (this.ValidateInput(oFabric))
                    oFabric = oFabric.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                else
                    throw new Exception(_sErrorMessage);
                    //oFabric.ErrorMessage = _sErrorMessage;
            }
            catch (Exception ex)
            {
                oFabric = new Fabric();
                oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFRnD(FabricRnD oFabricRnD)
        {
            try
            {
                oFabricRnD = oFabricRnD.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricRnD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveAll(List<Fabric> oFabrics)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                _oFabrics = Fabric.SaveAll(oFabrics, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics = new List<Fabric>();
                _oFabrics.Add(_oFabric);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Fabric oFabric)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabric.Delete(oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveReceiveDate(Fabric oFabric)
        {
            try
            {
                oFabric = oFabric.SaveReceiveDate(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabric = new Fabric();
                oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSubmissionDate(Fabric oFabric)
        {
            try
            {
                oFabric = oFabric.SaveSubmissionDate(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabric = new Fabric();
                oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(Fabric oFabric)
        {
            _oFabric = new Fabric();
            try
            {
                if (oFabric.FabricID > 0)
                {
                    _oFabric = _oFabric.Get(oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabric.FabricSeekingDates = FabricSeekingDate.Gets(_oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetForReceiveDate(Fabric oFabric)
        {
            _oFabric = new Fabric();
            try
            {
                if (oFabric.FabricID > 0)
                {
                    _oFabric = _oFabric.Get(oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabric.FabricSeekingDates = FabricSeekingDate.Gets(_oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oFabric.ReceiveDate == DateTime.MinValue)
                    {
                        _oFabric.ReceiveDate = DateTime.Today;
                    }
                }
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetForSubmitDate(Fabric oFabric)
        {
            _oFabric = new Fabric();
            try
            {
                if (oFabric.FabricID > 0)
                {
                    _oFabric = _oFabric.Get(oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabric.FabricSeekingDates = FabricSeekingDate.Gets(_oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oFabric.SubmissionDate == DateTime.MinValue)
                    {
                        _oFabric.SubmissionDate = DateTime.Today;
                    }
                }
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PrintFabrics(FormCollection DataCollection)
        {
            _oFabric= new Fabric();
            string sFabricIDs = DataCollection["txtFabricCollectionList"];
            //_oFabric.Fabrics = new JavaScriptSerializer().Deserialize<List<Fabric>>(DataCollection["txtFabricCollectionList"]);

            string sSQL = "SELECT * FROM View_Fabric WHERE FabricID IN (" + sFabricIDs + ")";
            _oFabric.Fabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oFabric.Company = oCompany; 

            string Messge = "Fabric List";
            rptFabrics oReport = new rptFabrics();
            byte[] abytes = oReport.PrepareReport(_oFabric, Messge, oCompany);
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult ReFabricSubmission(Fabric oFabric)
        {
            try
            {
                oFabric.PriorityLevel = (EnumPriorityLevel)oFabric.PriorityLevelInInt;
                if (!String.IsNullOrEmpty(oFabric.Construction))
                    oFabric.Construction = oFabric.Construction.Trim();

                if (this.ValidateInput(oFabric))
                    oFabric = oFabric.ReFabricSubmission(((User)Session[SessionInfo.CurrentUser]).UserID);
                else
                    oFabric.ErrorMessage = _sErrorMessage;
            }
            catch (Exception ex)
            {
                oFabric = new Fabric();
                oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabric);
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


        [HttpPost]
        public JsonResult AdvSearch(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                /*
                 * ViewFabrics
                 * ViewFabricForPattern
                 */

                string sSQL = this.MakeSQL(oFabric);
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabrics = new List<Fabric>();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            var jsonResult = Json(_oFabrics, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchByFabricNo(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                if (oFabric.FabricNo == "") oFabric.FabricNo = null;
                string sSQL = "SELECT top(1000)* FROM View_Fabric WHERE FabricNo LIKE '%" + oFabric.FabricNo + "%'";
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oFabrics);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(_oFabrics, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchByBuyerName(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                if (!String.IsNullOrEmpty(oFabric.BuyerName));
                {
                    string sSQL = "SELECT top(1000)* FROM View_Fabric WHERE BuyerName LIKE '%" + oFabric.BuyerName + "%' order by IssueDate DESC";
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oFabrics);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(_oFabrics, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchByBuyerFabricNo(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                string sSQL = "SELECT * FROM View_Fabric WHERE FabricID<>0";
                if (oFabric.BuyerID > 0) { sSQL = sSQL + " And BuyerID=" + oFabric.BuyerID + ""; }
                if(oFabric.FabricNo!=null && oFabric.FabricNo.Trim()!=""){
                    sSQL = sSQL + " And FabricNo LIKE '%" + oFabric.FabricNo.Trim() + "%' ORDER BY FabricNo";
                }
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFabricByFabricNo(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                string sSQL = "SELECT top(100)* FROM View_Fabric WHERE FabricID<>0";
                if (oFabric.BuyerID > 0) { sSQL = sSQL + " And BuyerID=" + oFabric.BuyerID + ""; }
                if (oFabric.FabricNo != null && oFabric.FabricNo.Trim() != "")
                {
                    sSQL = sSQL + " And FabricNo LIKE '%" + oFabric.FabricNo.Trim() + "%' ORDER BY FabricNo";
                }
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SearchByBuyerNameOnlyApprovedFabric(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                if (oFabric.BuyerName == "") oFabric.BuyerName = null;
                string sSQL = "SELECT * FROM View_Fabric WHERE ApprovedBy = 0 AND BuyerName LIKE '%" + oFabric.BuyerName + "%' ORDER BY FabricNo";
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
           
            try
            {
                bool hasFabricPattern = false;
                bool.TryParse(oProduct.Params, out hasFabricPattern);

                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.Fabric, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (!hasFabricPattern)
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (hasFabricPattern)
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.FabricPattern, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsFabricsForPI(Fabric oFabric)
        {
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<Fabric> oFabrics = new List<Fabric>();
            string sSQL = "SELECT top(500)* FROM View_Fabric";

            string sReturn = "";
            if (!String.IsNullOrEmpty(oFabric.FabricNo))
            {
                oFabric.FabricNo = oFabric.FabricNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricNo like '%" + oFabric.FabricNo + "%'";
            }
            if (!String.IsNullOrEmpty(oFabric.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BuyerID in (" + oFabric.BuyerName + ")";
            }

            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "Isnull(ApprovedBy,0)<>0";

            sSQL = sSQL + " " + sReturn + " order by Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) DESC";

            oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult StatusChange(Fabric oFabric)
        {
            try
            {
                if (oFabric.FabricID > 0)
                {
                    oFabric = oFabric.StatusChange(oFabric.FabricID,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else {
                    oFabric = new Fabric();
                    oFabric.ErrorMessage = "Invalid Fabric.";
                }
            }
            catch (Exception ex)
            {
                oFabric = new Fabric();
                oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approve(List<Fabric> oFabrics)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                _oFabrics = Fabric.Approve(oFabrics, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics = new List<Fabric>();
                _oFabrics.Add(_oFabric);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Received(List<Fabric> oFabrics)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                _oFabrics = Fabric.Received(oFabrics, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics = new List<Fabric>();
                _oFabrics.Add(_oFabric);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Submission(List<Fabric> oFabrics)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                _oFabrics = Fabric.Submission(oFabrics, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics = new List<Fabric>();
                _oFabrics.Add(_oFabric);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabrics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveSubmission(Fabric oFabric)
        {
            _oFabric = new Fabric();
            try
            {
                _oFabric = oFabric.SaveSubmission( ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveReceived(Fabric oFabric)
        {
            try
            {
                oFabric = oFabric.SaveReceived(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabric = new Fabric();
                oFabric.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabric);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintSticker(string sStickerHeader, string sParams, double nts)
        {
            _oFabric = new Fabric();
            string sSQL = "SELECT * FROM View_Fabric WHERE FabricID IN (" + sParams + ")";
            //_oFabric.Fabrics = new JavaScriptSerializer().Deserialize<List<Fabric>>(DataCollection["txtPrintStickerFabric"]);
            _oFabric.Fabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabric.StickerHeader = sStickerHeader.Replace("aaaa","&");
            rptFabricsPrintSticker oReport = new rptFabricsPrintSticker();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            byte[] abytes = oReport.PrepareReport(_oFabric, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFabricOrder(string sParams,int FOType, double nts)
        {
            _oFabric = new Fabric();
            List<FNLabDipDetail> oFNLabDipDetails = new List<FNLabDipDetail>();
            FabricOrderSetup oFebricOrderSetup = new FabricOrderSetup();
            string sSQL = "SELECT * FROM View_Fabric WHERE FabricID IN (" + sParams + ")";
            _oFabric.Fabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFebricOrderSetup = oFebricOrderSetup.Get(FOType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFNLabDipDetails = FNLabDipDetail.Gets("SELECT * FROM View_FNLabDipDetail WHERE FabricID IN (" + sParams + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFebricOrderSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFabricOrder oReport = new rptFabricOrder();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            byte[] abytes = oReport.PrepareReport(_oFabric, oFebricOrderSetup, oFNLabDipDetails, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        //[HttpPost]
        //public JsonResult GetsFabricByFEO(FabricExecutionOrder oFabricExecutionOrder)
        //{
        //    _oFabrics = new List<Fabric>();
        //    try
        //    {
        //        string sSQL = "SELECT * FROM View_Fabric WHERE FabricID IN (SELECT FabricID FROM FabricExecutionOrder WHERE FEOID IN (" + oFabricExecutionOrder.Params + "))";
        //        _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabrics = new List<Fabric>();
        //        _oFabric.ErrorMessage = ex.Message;
        //        _oFabrics.Add(_oFabric);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oFabrics);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Fabric Attachment
        public ViewResult ViewFabricAttachment(int id,  string ms, string OperationInfo)
        {
                _oFabric = new Fabric();
                _oFabric = _oFabric.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            FabricAttachment oFabricAttachment = new FabricAttachment();
            List<FabricAttachment> oFabricAttachments = new List<FabricAttachment>();
            oFabricAttachments = FabricAttachment.GetsAttachmentByFabric(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricAttachment.FabricID = id;
            ViewBag.SwatchTypes = EnumObject.jGets(typeof(EnumSwatchType)); //SwatchTypesObj.Gets();
            ViewBag.Fabric = _oFabric;   
            oFabricAttachment.FabricAttachments = oFabricAttachments;
            TempData["message"] = ms;
            oFabricAttachment.ErrorMessage = "";
            return View(oFabricAttachment);
        }
        [HttpPost]        
        public string UploadAttchment(HttpPostedFileBase file, FabricAttachment oFabricAttachment)
        {
            string sFeedBackMessage = "File Upload successfully";
            byte[] data;
            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    double nMaxLength = 1024 * 1024;
                    if (data == null || data.Length <= 0)
                    {
                        sFeedBackMessage = "Please select an file!";
                    }
                    else if (data.Length > nMaxLength)
                    {
                        sFeedBackMessage = "You can select maximum 1MB file size!";
                    }
                    else if (oFabricAttachment.FabricID <= 0)
                    {
                        sFeedBackMessage = "Your Selected Fabric Is Invalid!";
                    }
                    else
                    {
                        oFabricAttachment.SwatchType = (EnumSwatchType) oFabricAttachment.SwatchTypeInInt;
                        oFabricAttachment.AttatchFile = data;
                        oFabricAttachment.AttatchmentName = file.FileName;
                        oFabricAttachment.FileType = file.ContentType;
                        oFabricAttachment = oFabricAttachment.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    sFeedBackMessage = "Please select a file!";
                }
            }
            catch (Exception ex)
            {
                sFeedBackMessage = "";
                sFeedBackMessage = ex.Message;
            }

            return sFeedBackMessage;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(sFeedBackMessage);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
                        
            //return RedirectToAction("ViewFabrics", new { menuid = (int)Session[SessionInfo.MenuID], fabricId = oFabricAttachment.FabricID });
        }

        [HttpPost]
        public ActionResult DownloadAttachment(FormCollection oFormCollection)
        {
            FabricAttachment oFabricAttachment = new FabricAttachment();
            try
            {
                int nFabricAttchmentID=Convert.ToInt32(oFormCollection["FabricAttchmentID"]);
                oFabricAttachment = FabricAttachment.GetWithAttachFile(nFabricAttchmentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricAttachment.AttatchFile != null)
                {
                    var file = File(oFabricAttachment.AttatchFile, oFabricAttachment.FileType);
                    file.FileDownloadName = oFabricAttachment.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oFabricAttachment.AttatchmentName);
            }
        }

        [HttpPost]
        public JsonResult GetsAttachmentByFabric(FabricAttachment oFabricAttachment)
        {
            List<FabricAttachment> oFabricAttachments = new List<FabricAttachment>();
            try
            {
                oFabricAttachments = FabricAttachment.GetsAttachmentByFabric(oFabricAttachment.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricAttachments = new List<FabricAttachment>();
                oFabricAttachment = new FabricAttachment();
                oFabricAttachment.ErrorMessage = ex.Message;
                oFabricAttachments.Add(oFabricAttachment);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricAttachments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFabricAttachment(FabricAttachment oFabricAttachment)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricAttachment.Delete(oFabricAttachment.FabricAttachmentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<FabricAttachment> GetsFabricAttachment(FabricAttachment oFabricAttachment)
        {
            List<FabricAttachment> oFabricAttachments = new List<FabricAttachment>();
            oFabricAttachments = FabricAttachment.GetsAttachmentByFabric(oFabricAttachment.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return oFabricAttachments;
        }

        #endregion

        #region Make SQL
        private string MakeSQL(Fabric oFabric)
        {
            string sReturn1 = "SELECT * FROM View_Fabric AS F";
            string sReturn = "";

            string sParams = oFabric.Remarks;

            if (!string.IsNullOrEmpty(sParams))
            {
                string sFabricNo = Convert.ToString(sParams.Split('~')[0]);
                string nBuyerID = Convert.ToString(sParams.Split('~')[1]);
                int nProductID = Convert.ToInt32(sParams.Split('~')[2]);
                string sBuyerReference = Convert.ToString(sParams.Split('~')[3]);
                string sProductName = Convert.ToString(sParams.Split('~')[4]);
                string nMKTPersonID = Convert.ToString(sParams.Split('~')[5]);
                int nFinishTypeInInt = Convert.ToInt32(sParams.Split('~')[6]);

                bool bIsIssueDate = Convert.ToBoolean(sParams.Split('~')[7]);
                DateTime dFromIssueDate = Convert.ToDateTime(sParams.Split('~')[8]);
                DateTime dToIssueDate = Convert.ToDateTime(sParams.Split('~')[9]);

                bool bIsLabDipReqDate = Convert.ToBoolean(sParams.Split('~')[10]);
                DateTime dFromLabDipReqDate = Convert.ToDateTime(sParams.Split('~')[11]);
                DateTime dToLabDipReqDate = Convert.ToDateTime(sParams.Split('~')[12]);

                bool bIsFabricPatternDate = Convert.ToBoolean(sParams.Split('~')[13]);
                DateTime dFromFabricPatternDate = Convert.ToDateTime(sParams.Split('~')[14]);
                DateTime dToFabricPatternDate = Convert.ToDateTime(sParams.Split('~')[15]);

                bool bIsApproveDate = Convert.ToBoolean(sParams.Split('~')[16]);
                DateTime dFromApproveDate = Convert.ToDateTime(sParams.Split('~')[17]);
                DateTime dToApproveDate = Convert.ToDateTime(sParams.Split('~')[18]);

                bool bIsYetToLabDip = Convert.ToBoolean(sParams.Split('~')[19]);
                bool bYetToPattern = Convert.ToBoolean(sParams.Split('~')[20]);

                int nProcessTypeInInt = Convert.ToInt32(sParams.Split('~')[21]);
                string sConstruction = Convert.ToString(sParams.Split('~')[22]);

                bool bApprove = Convert.ToBoolean(sParams.Split('~')[23]);
                bool bUnapprove = Convert.ToBoolean(sParams.Split('~')[24]);

                bool bIsSubmissionDate = Convert.ToBoolean(sParams.Split('~')[25]);
                DateTime dFromSubmissionDate = Convert.ToDateTime(sParams.Split('~')[26]);
                DateTime dToSubmissionDate = Convert.ToDateTime(sParams.Split('~')[27]);

                bool bIsReceiveDate= Convert.ToBoolean(sParams.Split('~')[28]);
                DateTime dFromReceiveDate= Convert.ToDateTime(sParams.Split('~')[29]);
                DateTime dToReceiveDate= Convert.ToDateTime(sParams.Split('~')[30]);

                int nWeaveTypeInInt = Convert.ToInt32(sParams.Split('~')[31]);
                int nFabricDesignInInt = Convert.ToInt32(sParams.Split('~')[32]);

                string sColor = Convert.ToString(sParams.Split('~')[33]);
                string sStyle = Convert.ToString(sParams.Split('~')[34]);

                string sMKTGroupIDs = "";
                if (sParams.Split('~').Count() > 35)
                    sMKTGroupIDs = Convert.ToString(sParams.Split('~')[35]);

                #region Fabric No
                if (!string.IsNullOrEmpty(sFabricNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.FabricNo LIKE '%" + sFabricNo + "%' ";
                }
                #endregion

                #region Buyer Id
                if (!String.IsNullOrEmpty(nBuyerID))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.BuyerID in( " + nBuyerID+")";
                }
                #endregion

                #region Product Name (Composition)
                if (!string.IsNullOrEmpty(sProductName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.ProductName LIKE '%" + sProductName + "%' ";
                }
                #endregion

                #region Buyer Reference
                if (!string.IsNullOrEmpty(sBuyerReference))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.BuyerReference LIKE '%" + sBuyerReference + "%' ";
                }
                #endregion

                #region MKTPerson ID
                if (!string.IsNullOrEmpty(nMKTPersonID))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.MKTPersonID in(" + nMKTPersonID+")";
                }
                #endregion

                #region MKTGroup ID
                if (!string.IsNullOrEmpty(sMKTGroupIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.MKTPersonID IN (Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + sMKTGroupIDs + "))";
                }
                #endregion

                #region Finish Type
                if (nFinishTypeInInt > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.FinishType = " + (int)nFinishTypeInInt;
                }
                #endregion

                #region Issue Date
                if (bIsIssueDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),F.IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                #region Lab Dip Request Date
                if (bIsLabDipReqDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.FabricID IN (SELECT LDO.FabricID FROM View_LabDipOrders AS LDO WHERE CONVERT(DATE,CONVERT(VARCHAR(12),LDO.CreateDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDipReqDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLabDipReqDate.ToString("dd MMM yyyy") + "',106))) ";
                }
                #endregion

                #region Fabric Pattern Date
                if (bIsFabricPatternDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.FabricID IN (SELECT FP.FabricID FROM FabricPattern AS FP WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FP.DBServerDateTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromFabricPatternDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToFabricPatternDate.ToString("dd MMM yyyy") + "',106))) ";
                }
                #endregion

                #region Fabric Approve Date
                if (bIsApproveDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),F.ApprovedByDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                //#region Yet To Lab Dip
                //if (bIsYetToLabDip)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " F.FabricID NOT IN (SELECT LDOr.FabricID FROM View_LabDipOrders AS LDOr) ";
                //}
                //#endregion

                #region Yet To Pattern
                if (bYetToPattern)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.FabricID NOT IN (SELECT FP.FabricID FROM FabricPattern AS FP) ";
                }
                #endregion

                #region Process Type
                if (nProcessTypeInInt > (int)EnumProcessType.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.ProcessType = " + (int)nProcessTypeInInt;
                }
                #endregion

                #region Construction
                if (!string.IsNullOrEmpty(sConstruction))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.Construction LIKE '%" + sConstruction + "%' ";
                }
                #endregion

                #region Approved Data
                if (bApprove && !bUnapprove)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.ApprovedBy > 0 ";
                }
                #endregion

                #region Unapproved Data
                if (bUnapprove && !bApprove)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.ApprovedBy = 0 ";
                }
                #endregion

                #region Fabric Submission Date
                if (bIsSubmissionDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),F.SubmissionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmissionDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSubmissionDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                #region Fabric Receive Date
                if (bIsReceiveDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),F.ReceiveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                #region Weave Type
                if (nWeaveTypeInInt > (int)EnumProcessType.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.FabricWeave = " + (int)nWeaveTypeInInt;
                }
                #endregion

                #region Fabric Design
                if (nFabricDesignInInt > (int)EnumProcessType.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.FabricDesignID = " + (int)nFabricDesignInInt;
                }
                #endregion

                #region Color
                if (!string.IsNullOrEmpty(sColor))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.ColorInfo LIKE '%" + sColor + "%' ";
                }
                #endregion

                #region Style No
                if (!string.IsNullOrEmpty(sStyle))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " F.StyleNo LIKE '%" + sStyle + "%' ";
                }
                #endregion
            }
            string sSQL = sReturn1 + " " + sReturn + " order by IssueDate DESC,Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
            return sSQL;
        }
        #endregion

        #region Fabric Pattern
        public ActionResult ViewFabricForPattern(int buid, int menuid, int nFabricId = 0)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);


            _oFabrics = new List<Fabric>();
            string sSQL = "";
            if (oMarketingAccounts.Count > 0)
            {
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT * FROM View_Fabric WHERE ISNULL(ApprovedBy,0)<> 0 AND SubmissionDate IS NULL AND View_Fabric.MKTPersonID in ( Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") ORDER BY IssueDate DESC";
            }
            else
            {
                oMarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT top(120)* FROM View_Fabric WHERE  ISNULL(ApprovedBy,0)<> 0  AND SubmissionDate IS NULL ORDER BY IssueDate DESC";
            }
            _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FabricId = nFabricId;
            ViewBag.menuid = menuid;
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();


            ViewBag.MktPersons = oMarketingAccounts;
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SwatchTypes = EnumObject.jGets(typeof(EnumSwatchType)); //SwatchTypesObj.Gets();

            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            sSQL = "SELECT * FROM FabricProcess";
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FinishTypes = oFabricProcesss.Where(o=>o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();

            List<LightSource> oLSs = new List<LightSource>();
            oLSs = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = oLSs;
            ViewBag.BUID = buid;

            return View(_oFabrics);
        }
        public ActionResult ViewFabricPatterns(int menuid, int nFSCDetailID, int nFabricID, int nBUID)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            _oFabricPatterns = new List<FabricPattern>();
            _oFabric = new Fabric();
            if (nFabricID != 0)
            {
                _oFabric = _oFabric.Get(nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "Select * from View_FabricPattern Where FabricID =" + nFabricID + "";
                _oFabricPatterns = FabricPattern.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.Fabric = _oFabric;
            ViewBag.menuid = menuid;
            ViewBag.BUID = nBUID;
            ViewBag.FSCDetailID = nFSCDetailID;
            sSQL = "SELECT * FROM FabricProcess WHERE ProcessType=" + (int)EnumFabricProcess.Weave;
            ViewBag.FabricWeaves = FabricProcess.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricPatterns);
        }
        [HttpPost]
        public JsonResult SaveFP(FabricPattern oFabricPattern)
        {
            try
            {
                if (oFabricPattern.FPID <= 0)
                {
                    oFabricPattern = oFabricPattern.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFabricPattern = oFabricPattern.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApproveFP(FabricPattern oFabricPattern)
        {
            try
            {
                if (oFabricPattern.FPID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricPattern = oFabricPattern.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CopyFP(FabricPattern oFabricPattern)
        {
            try
            {
                oFabricPattern = FabricPattern.Get(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricPattern.FPID > 0)
                {
                    oFabricPattern.FabricPatternDetails = FabricPatternDetail.Gets(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oFabricPattern.FPID = 0;
                oFabricPattern.ApproveBy = 0;
                oFabricPattern.IsActive = false;
                oFabricPattern.FabricPatternDetails.ForEach(o => o.FPDID = 0);
                if (oFabricPattern.FPID <= 0)
                {
                    oFabricPattern = oFabricPattern.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeActivityFP(FabricPattern oFabricPattern)
        {
            try
            {
                if (oFabricPattern.FPID <= 0) { throw new Exception("Please select an valid item."); }
                if (oFabricPattern.IsActive) { oFabricPattern.IsActive = false; }
                else { oFabricPattern.IsActive = true; }
                oFabricPattern = oFabricPattern.IUD((int)EnumDBOperation.Revise, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFP(FabricPattern oFabricPattern)
        {
            try
            {
                if (oFabricPattern.FPID <= 0) { throw new Exception("Please select an valid item."); }

                oFabricPattern = oFabricPattern.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetFP(FabricPattern oFabricPattern)
        {
            try
            {
                if (oFabricPattern.FPID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricPattern = FabricPattern.Get(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricPattern.FPID > 0)
                {
                    oFabricPattern.FPDetails = FabricPatternDetail.Gets(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricPattern.FabricPatternDetails = oFabricPattern.FPDetails.Where(x => x.IsWarp == false).ToList();
                    oFabricPattern.FPDetails = oFabricPattern.FPDetails.Where(x => x.IsWarp == true).ToList();
                    if (oFabricPattern.FPDetails.Count > 0)
                    {
                       // oFabricPatternDetails = oFabricPatternDetails.Where(x => x.IsWarp == oFabricPatternDetail.IsWarp).ToList();
                        if (oFabricPattern.FPDetails.FirstOrDefault() != null && oFabricPattern.FPDetails.FirstOrDefault().FPDID > 0 && oFabricPattern.FPDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                        {
                            List<FabricPatternDetail> oFEOSDetails = new List<FabricPatternDetail>();
                            oFabricPattern.FPDetails.ForEach((item) => { oFEOSDetails.Add(item); });
                            oFabricPattern.FPDetails = this.TwistedDetails(oFabricPattern.FPDetails);
                            oFabricPattern.FPDetails[0].CellRowSpans = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == true).ToList());
                           // oFabricPattern.FPDetails[0].CellRowSpansWeft = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == false).ToList());
                        }
                    }
                 ///
                    if (oFabricPattern.FabricPatternDetails.Count > 0)
                    {
                        // oFabricPatternDetails = oFabricPatternDetails.Where(x => x.IsWarp == oFabricPatternDetail.IsWarp).ToList();
                        if (oFabricPattern.FabricPatternDetails.FirstOrDefault() != null && oFabricPattern.FabricPatternDetails.FirstOrDefault().FPDID > 0 && oFabricPattern.FabricPatternDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                        {
                            List<FabricPatternDetail> oFEOSDetails = new List<FabricPatternDetail>();
                            oFabricPattern.FabricPatternDetails.ForEach((item) => { oFEOSDetails.Add(item); });
                            oFabricPattern.FabricPatternDetails = this.TwistedDetails(oFabricPattern.FabricPatternDetails);
                            //oFabricPattern.FabricPatternDetails[0].CellRowSpans = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == true).ToList());
                            oFabricPattern.FabricPatternDetails[0].CellRowSpansWeft = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == false).ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFPDetail(FabricPatternDetail oFPDetail)
        {
            try
            {
                int nFPID = oFPDetail.FPID;
                oFPDetail.ColorName = oFPDetail.ColorName.ToUpper();
                if (oFPDetail.FPDID <= 0)
                {
                    oFPDetail = oFPDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (nFPID <= 0 && oFPDetail.FPID > 0)
                    {
                        oFPDetail.FP = new FabricPattern();
                        oFPDetail.FP = FabricPattern.Get(oFPDetail.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    oFPDetail = oFPDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oFPDetail = new FabricPatternDetail();
                oFPDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveByLabdip(FabricPattern oFabricPattern_Labdip)
        {
            int nSL = 0;
            FabricPattern oFabricPattern = new FabricPattern();
            List<LabDipDetailFabric> oLabDipDetailFabrics = new List<LabDipDetailFabric>();
            List<LabDipDetailFabric> oLabDipDetailFabrics_Temp = new List<LabDipDetailFabric>();

            List<LabDip> oLabDips = new List<LabDip>();
            LabDip oLabDip = new LabDip();
            List<LabDipDetail> LabDipDetails_Temp = new List<LabDipDetail>();
            try
            {

                string sSQL = "select * from view_LabDipDetailFabric where FabricID=" + oFabricPattern_Labdip.FabricID;

                oLabDipDetailFabrics = LabDipDetailFabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricPattern.FabricID = oFabricPattern_Labdip.FabricID;
                FabricPatternDetail oFabricPatternDetail = new FabricPatternDetail();
                oLabDipDetailFabrics_Temp = oLabDipDetailFabrics.Where(o => o.WarpWeftType == EnumWarpWeft.Warp || o.WarpWeftType == EnumWarpWeft.WarpnWeft).ToList();
                foreach (LabDipDetailFabric oItem in oLabDipDetailFabrics_Temp)
                {
                    nSL = nSL++;
                    oFabricPatternDetail = new FabricPatternDetail();
                    oFabricPatternDetail.SLNo = nSL;
                    oFabricPatternDetail.IsWarp = true;
                    oFabricPatternDetail.ColorName = oItem.ColorName;
                    oFabricPatternDetail.ColorNo = oItem.ColorNo;
                    oFabricPatternDetail.LabDipDetailID = oItem.LabDipDetailID;
                    oFabricPatternDetail.ProductID = oItem.ProductID;
                    oFabricPatternDetail.TwistedGroup = oItem.TwistedGroup;
                    oFabricPatternDetail.PantonNo = oItem.PantonNo;
                    oFabricPattern.FabricPatternDetails.Add(oFabricPatternDetail);
                }
                oLabDipDetailFabrics_Temp = oLabDipDetailFabrics.Where(o => o.WarpWeftType == EnumWarpWeft.Weft || o.WarpWeftType == EnumWarpWeft.WarpnWeft).ToList();
                foreach (LabDipDetailFabric oItem in oLabDipDetailFabrics_Temp)
                {
                    nSL = nSL++;
                    oFabricPatternDetail = new FabricPatternDetail();
                    oFabricPatternDetail.SLNo = nSL;
                    oFabricPatternDetail.IsWarp = false;
                    oFabricPatternDetail.ColorName = oItem.ColorName;
                    oFabricPatternDetail.ColorNo = oItem.ColorNo;
                    oFabricPatternDetail.LabDipDetailID = oItem.LabDipDetailID;
                    oFabricPatternDetail.TwistedGroup = oItem.TwistedGroup;
                    oFabricPatternDetail.ProductID = oItem.ProductID;
                    oFabricPatternDetail.PantonNo = oItem.PantonNo;
                    oFabricPattern.FabricPatternDetails.Add(oFabricPatternDetail);
                }
                if (oFabricPattern.FabricPatternDetails.Count > 0)
                {
                    oFabricPattern = oFabricPattern.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }


              
                // sSQL = "SELECT * FROM View_LabDip Where LabDipID in (select LabDipID from LabDipDetailFabric where FabricID=" + oFabricPattern_Labdip.FabricID + ")";




                //oLabDips = LabDip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oLabDips.Count > 0 )
                //{
                //    oLabDip = oLabDips.FirstOrDefault();
                //}

                //if (oLabDip.LabDipID > 0 && oFabricPattern_Labdip.FabricID > 0)
                //{
                //    //sSQL = "Select * from View_LabdipDetail Where LabDipID=" + oLabDip.LabDipID + " order by WarpWeftType";
                //    sSQL = "Select * from View_LabdipDetail Where LabDipID=" + oLabDip.LabDipID + " and  LabDipDetailID in (select LabDipDetailID from LabDipDetailFabric where FabricID=" + oFabricPattern_Labdip.FabricID + ") order by WarpWeftType";
                //    oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                //else
                //{ oFabricPattern.ErrorMessage = "Labdip Not found"; }

                //oFabricPattern.FSCDetailID = oFabricPattern_Labdip.FSCDetailID;
                //oFabricPattern.FabricID = oFabricPattern_Labdip.FabricID;
                //oFabricPattern.FPID = 0;
                ////oFabricPattern.StyleNo = oLabDip.BuyerRefNo;
                //FabricPatternDetail oFabricPatternDetail = new FabricPatternDetail();
                //LabDipDetails_Temp = oLabDip.LabDipDetails.Where(o => o.WarpWeftType == EnumWarpWeft.Warp || o.WarpWeftType == EnumWarpWeft.WarpnWeft).ToList();
                //foreach (LabDipDetail oItem in LabDipDetails_Temp)
                //{
                //    oFabricPatternDetail = new FabricPatternDetail();
                //    oFabricPatternDetail.IsWarp =true;
                //    oFabricPatternDetail.ColorName = oItem.ColorName;
                //    oFabricPatternDetail.ColorNo = oItem.ColorNo;
                //    oFabricPatternDetail.LabDipDetailID = oItem.LabDipDetailID;
                //    oFabricPatternDetail.ProductID = oItem.ProductID;
                //    oFabricPatternDetail.PantonNo = oItem.PantonNo;
                //    oFabricPattern.FabricPatternDetails.Add(oFabricPatternDetail);
                //}
                //LabDipDetails_Temp = oLabDip.LabDipDetails.Where(o => o.WarpWeftType == EnumWarpWeft.Weft || o.WarpWeftType == EnumWarpWeft.WarpnWeft).ToList();
                //foreach (LabDipDetail oItem in LabDipDetails_Temp)
                //{
                //    oFabricPatternDetail = new FabricPatternDetail();
                //    oFabricPatternDetail.IsWarp = false;
                //    oFabricPatternDetail.ColorName = oItem.ColorName;
                //    oFabricPatternDetail.ColorNo = oItem.ColorNo;
                //    oFabricPatternDetail.LabDipDetailID = oItem.LabDipDetailID;
                //    oFabricPatternDetail.ProductID = oItem.ProductID;
                //    oFabricPatternDetail.PantonNo = oItem.PantonNo;
                //    oFabricPattern.FabricPatternDetails.Add(oFabricPatternDetail);
                //}
                //if (oFabricPattern.FabricPatternDetails.Count > 0)
                //{
                //    oFabricPattern = oFabricPattern.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                //}

            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSequence(FabricPattern oFabricPattern)
        {
            try
            {
                if (oFabricPattern.FPID>0)
                {
                    oFabricPattern = oFabricPattern.SaveSequence( ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFabricPattern = new FabricPattern();
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFPDetail(FabricPatternDetail oFPDetail)
        {
            try
            {
                oFPDetail = oFPDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFPDetail = new FabricPatternDetail();
                oFPDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFPDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintFabricPattern(int nFPID, bool bIsDeisplyPattern,  int nBUID, double nts)
        {
            FabricPattern oFabricPattern = new FabricPattern();
            oFabricPattern = FabricPattern.Get(nFPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oFabricPattern.FPID > 0)
            {
                oFabricPattern.FPDetails = FabricPatternDetail.Gets(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFabricPattern oReport = new rptFabricPattern();
            byte[] abytes = oReport.PrepareReport(oFabricPattern, oCompany, oBU, bIsDeisplyPattern);
            return File(abytes, "application/pdf");
        }
        public ActionResult DeskLoomSample(int nFPID, bool bIsDeisplyPattern, int nBUID, double nts)
        {
            FabricPattern oFabricPattern = new FabricPattern();
            oFabricPattern = FabricPattern.Get(nFPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oFabricPattern.FPID > 0)
            {
                oFabricPattern.FPDetails = FabricPatternDetail.Gets(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFabricPattern oReport = new rptFabricPattern();
            byte[] abytes = oReport.PrepareReport(oFabricPattern, oCompany, oBU, bIsDeisplyPattern);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintDeskLoom(int nFabricId, double nts)
        {
            Fabric oFabric = new Fabric();
            FabricPattern oFabricPattern = new FabricPattern();

            oFabric = oFabric.Get(nFabricId, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFabric.FabricID > 0)
            {
                oFabricPattern = FabricPattern.GetActiveFP(oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptDeskLoom oReport=new rptDeskLoom();
            byte[] abytes = oReport.PrepareReport(oFabric, oFabricPattern, oCompany);
            return File(abytes, "application/pdf");
        }
        [HttpPost]
        public ActionResult GetFPColors(FabricPattern oFabricPattern)
        {
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sReturn1 = "SELECT top(100)* FROM View_LabdipDetail ";
                string sReturn = "";
                if (!string.IsNullOrEmpty(oFabricPattern.Note))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ColorName LIKE '%" + oFabricPattern.Note + "%'";
                }
                if (oFabricPattern.ProductID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "  ProductID = " + oFabricPattern.ProductID;
                }
                //if (oFabricPattern.FabricID > 0)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "  FabricID = " + oFabricPattern.FabricID;
                //}
               
                string sSQL = sReturn1 + " " + sReturn + " ORDER BY ColorName";
                oLabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetails = new List<LabDipDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetsProductByFabric(FabricPattern oFabricPattern)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = "SELECT * FROM View_Product WHERE ShortName LIKE '%" + oFabricPattern.Note + "%' AND ProductID IN (SELECT ProductID FROM FabricPatternDetail WHERE FPID IN (SELECT FPID FROM FabricPattern WHERE IsActive=1 AND FabricID=" + oFabricPattern.FabricID + "))";
                oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                Product oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CopyFromWarp(FabricPatternDetail oFPD)
        {
            try
            {
                oFPD = oFPD.CopyFromWarp(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFPD = new FabricPatternDetail();
                oFPD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFPD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsPatternFabrics(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                string sSQL = "SELECT * FROM View_Fabric F WHERE F.FabricID IN (SELECT FP.FabricID FROM FabricPattern FP WHERE FP.IsActive=1 AND FP.FPID IN (SELECT FPD.FPID FROM FabricPatternDetail FPD))";
                if (!string.IsNullOrEmpty(oFabric.FabricNo))
                {
                    sSQL = sSQL + " AND FabricNo LIKE '%" + oFabric.FabricNo + "%'";
                }
                sSQL = sSQL + " ORDER By FabricNo";
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabric = new Fabric();
                oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(oFabric);
            }
            var jsonResult = Json(_oFabrics, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult CopyFabricPattern(FabricPattern oFabricPattern)
        {
            _oFabricPattern = new FabricPattern();
            try
            {
                if (oFabricPattern.CopyFabricID > 0)
                {
                    _oFabricPatterns = new List<FabricPattern>();
                    string sSQL = "SELECT * FROM View_FabricPattern WHERE FabricID=" + oFabricPattern.CopyFabricID + " AND IsActive=1";
                    _oFabricPatterns = FabricPattern.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oFabricPatterns.Count > 0)
                    {
                        _oFabricPattern = _oFabricPatterns[0];
                        _oFabricPattern.FabricPatternDetails = new List<FabricPatternDetail>();
                        _oFabricPattern.FabricPatternDetails = FabricPatternDetail.Gets(_oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oFabricPattern.FabricID = oFabricPattern.FabricID;
                        _oFabricPattern = _oFabricPattern.Copy(((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                _oFabricPattern = new FabricPattern();
                _oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void FabricListInExcel(FormCollection fc)
        {

            List<Fabric> oFabrics = new List<Fabric>();
            string FabricIDs = fc["FabricIDs"];

            string sSQL = "SELECT * FROM View_Fabric WHERE  ISNULL(ApprovedBy,0)<> 0 And FabricID In (" + FabricIDs + ") ORDER BY IssueDate DESC";
            oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);


            int rowIndex = 1;
            int nMaxColumn = 0;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Fabric List");
                sheet.Name = "Fabric List";

                #region Coloums

                string[] columnHead = new string[] { "SL", "Hand Loom No", "P-Number", "Buyer Name", "Construction", "Weave Type", "Mkt Person", "Issue Date", "Receive Date"};
                int[] colWidth = new int[] { 6, 18, 15, 30, 18, 15, 25, 14, 14 };

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                #endregion

                #region Column Header

                cell = sheet.Cells[rowIndex, 2, rowIndex, columnHead.Length + 1]; cell.Merge = true; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, columnHead.Length + 1]; cell.Merge = true; cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                colIndex = 2;
                rowIndex++;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }
                rowIndex++;
                #endregion

                #region Boby
                if (oFabrics.Any() && oFabrics.First().FabricID > 0)
                {
                    int nCount = 0;
                    foreach (Fabric oItem in oFabrics)
                    {
                        colIndex = 2;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.HandLoomNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MKTPersonName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.IssueDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceiveDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rowIndex++;
                    }
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FabricList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private List<CellRowSpan> RowMerge(List<FabricPatternDetail> oFabricPatternDetails)
        {

            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[] rowIndex = new int[1];
            int[] rowSpan = new int[1];

            List<FabricPatternDetail> oTWGLDDetails = new List<FabricPatternDetail>();
            List<FabricPatternDetail> oLDDetails = new List<FabricPatternDetail>();
            List<FabricPatternDetail> oTempLDDetails = new List<FabricPatternDetail>();

            oTWGLDDetails = oFabricPatternDetails.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oFabricPatternDetails.Where(x => x.TwistedGroup == 0).ToList();

            while (oFabricPatternDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricPatternDetails.FirstOrDefault().FPDID == oTWGLDDetails.FirstOrDefault().FPDID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();

                    oFabricPatternDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricPatternDetails.FirstOrDefault().FPDID == oLDDetails.FirstOrDefault().FPDID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FPDID == oLDDetails.FirstOrDefault().FPDID).ToList();

                    oFabricPatternDetails.RemoveAll(x => x.FPDID == oTempLDDetails.FirstOrDefault().FPDID);
                    oLDDetails.RemoveAll(x => x.FPDID == oTempLDDetails.FirstOrDefault().FPDID);
                }

                rowIndex[0] = rowIndex[0] + rowSpan[0];
                rowSpan[0] = oTempLDDetails.Count();
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("TwistedGroup", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        private List<FabricPatternDetail> TwistedDetails(List<FabricPatternDetail> oFabricPatternDetails)
        {
            List<FabricPatternDetail> oTwistedLDDetails = new List<FabricPatternDetail>();
            List<FabricPatternDetail> oTWGLDDetails = new List<FabricPatternDetail>();
            List<FabricPatternDetail> oLDDetails = new List<FabricPatternDetail>();
            List<FabricPatternDetail> oTempLDDetails = new List<FabricPatternDetail>();

            oTWGLDDetails = oFabricPatternDetails.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oFabricPatternDetails.Where(x => x.TwistedGroup == 0).ToList();

            while (oFabricPatternDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricPatternDetails.FirstOrDefault().FPDID == oTWGLDDetails.FirstOrDefault().FPDID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();
                    oFabricPatternDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricPatternDetails.FirstOrDefault().FPDID == oLDDetails.FirstOrDefault().FPDID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FPDID == oLDDetails.FirstOrDefault().FPDID).ToList();

                    oFabricPatternDetails.RemoveAll(x => x.FPDID == oTempLDDetails.FirstOrDefault().FPDID);
                    oLDDetails.RemoveAll(x => x.FPDID == oTempLDDetails.FirstOrDefault().FPDID);
                }
                oTwistedLDDetails.AddRange(oTempLDDetails);
            }
            return oTwistedLDDetails;
        }
        [HttpPost]
        public JsonResult MakeTwistedGroup(FabricPatternDetail oFabricPatternDetail)
        {
            FabricPattern oFabricPattern = new FabricPattern();
            List<FabricPatternDetail> oFabricPatternDetails = new List<FabricPatternDetail>();
            try
            {
                string sLabDipDetailID = string.IsNullOrEmpty(oFabricPatternDetail.ErrorMessage) ? "" : oFabricPatternDetail.ErrorMessage;
                if (sLabDipDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (oFabricPatternDetail.FPID <= 0)
                    throw new Exception("No valid labdip found.");

                oFabricPatternDetails = FabricPatternDetail.MakeTwistedGroup(sLabDipDetailID, oFabricPatternDetail.FPID, oFabricPatternDetail.TwistedGroup, (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricPatternDetails = oFabricPatternDetails.Where(x => x.IsWarp == oFabricPatternDetail.IsWarp).ToList();
                if (oFabricPatternDetails.FirstOrDefault() != null && oFabricPatternDetails.FirstOrDefault().FPDID > 0 && oFabricPatternDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<FabricPatternDetail> oTempFabricPatternDetails = new List<FabricPatternDetail>();
                    oFabricPatternDetails.ForEach((item) => { oTempFabricPatternDetails.Add(item); });
                    oFabricPatternDetails = this.TwistedDetails(oFabricPatternDetails);
                    oFabricPatternDetails[0].CellRowSpans = this.RowMerge(oTempFabricPatternDetails);
                }
                oFabricPattern.FabricPatternDetails = oFabricPatternDetails;

            }
            catch (Exception ex)
            {
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveTwistedGroup(FabricPatternDetail oFabricPatternDetail)
        {
            FabricPattern oFabricPattern = new FabricPattern();
            List<FabricPatternDetail> oFabricPatternDetails = new List<FabricPatternDetail>();
            try
            {
                string sLabDipDetailID = string.IsNullOrEmpty(oFabricPatternDetail.ErrorMessage) ? "" : oFabricPatternDetail.ErrorMessage;
                if (sLabDipDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (oFabricPatternDetail.FPID <= 0)
                    throw new Exception("No valid labdip found.");

                oFabricPatternDetails = FabricPatternDetail.MakeTwistedGroup(sLabDipDetailID, oFabricPatternDetail.FPID, oFabricPatternDetail.TwistedGroup, (int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricPatternDetails = oFabricPatternDetails.Where(x => x.IsWarp == oFabricPatternDetail.IsWarp).ToList();
                if (oFabricPatternDetails.FirstOrDefault() != null && oFabricPatternDetails.FirstOrDefault().FPDID > 0 && oFabricPatternDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<FabricPatternDetail> oTempFabricPatternDetails = new List<FabricPatternDetail>();
                    oFabricPatternDetails.ForEach((item) => { oTempFabricPatternDetails.Add(item); });
                    oFabricPatternDetails = this.TwistedDetails(oFabricPatternDetails);
                    oFabricPatternDetails[0].CellRowSpans = this.RowMerge(oTempFabricPatternDetails);
                }
                oFabricPattern.FabricPatternDetails = oFabricPatternDetails;

            }
            catch (Exception ex)
            {
                oFabricPattern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPattern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fabric Pattern Repeat
        [HttpPost]
        public ActionResult PatternRepeat(FabricPatternDetail oFabricPatternDetail)
        {
            FabricPatternDetail oFPD = new FabricPatternDetail();
            try
            {
                oFPD = oFabricPatternDetail.SavePatternRepeat(((User)Session[SessionInfo.CurrentUser]).UserID);
                oFPD.FabricPatternDetails = oFPD.FabricPatternDetails.OrderBy(c => c.FPDID).ToList();
            }
            catch (Exception ex)
            {
                oFPD = new FabricPatternDetail();
                oFPD.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFPD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fabric Sticker
        public ActionResult View_FabricStickers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);         
            _oFabricStickers = FabricSticker.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
   
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            return View(_oFabricStickers);
        }

        [HttpPost]
        public JsonResult SaveFabricSticker(FabricSticker oFabricSticker)
        {
            try
            {
                oFabricSticker = oFabricSticker.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSticker = new FabricSticker();
                oFabricSticker.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSticker);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFabricSticker(FabricSticker oFabricSticker)
        {
            _oFabricSticker = new FabricSticker();
            try
            {
                if (oFabricSticker.FabricStickerID > 0)
                {
                    _oFabricSticker = _oFabricSticker.Get(oFabricSticker.FabricStickerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabricSticker = new FabricSticker();
                _oFabricSticker.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSticker);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFabricSticker(FabricSticker oFabricSticker)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricSticker.Delete(oFabricSticker.FabricStickerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public ActionResult PrintFabricStickers(FormCollection DataCollection)
        {
            _oFabric = new Fabric();
            _oFabricStickers = new JavaScriptSerializer().Deserialize<List<FabricSticker>>(DataCollection["txtFabricStickerCollectionList"]);
            _oFabricSticker = (_oFabricStickers.Count > 0 ? _oFabricStickers[0] : new FabricSticker());
            rptFabricStickers oReport = new rptFabricStickers();
            byte[] abytes = oReport.PrepareReport(_oFabricSticker);
            return File(abytes, "application/pdf");
        }
        [HttpPost]
        public ActionResult PrintFabricStickersNew(FormCollection DataCollection)
        {
            _oFabric = new Fabric();
            _oFabricStickers = new JavaScriptSerializer().Deserialize<List<FabricSticker>>(DataCollection["txtFabricStickerCollectionListNew"]);
            _oFabricSticker = (_oFabricStickers.Count > 0 ? _oFabricStickers[0] : new FabricSticker());
            rptFabricStickersNew oReport = new rptFabricStickersNew();
            byte[] abytes = oReport.PrepareReport(_oFabricSticker);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region New Actions
        [HttpPost]
        public JsonResult SearchByFabricNoMax(Fabric oFabric)
        {
            _oFabrics = new List<Fabric>();
            try
            {
                if (oFabric.FabricNo == "") oFabric.FabricNo = null;
                string sSQL = "SELECT * FROM View_Fabric WHERE ApprovedBy > 0 AND FabricNo LIKE '%" + oFabric.FabricNo + "%' order by IssueDate DESC";
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            var jsonResult = Json(_oFabrics, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsFabricBySearchingCriterias(Fabric oFabric)
        {
            _oFabric = new Fabric();
            try
            {
                string sSQL = this.MakeSqlForSearchingCriterias(oFabric);
                _oFabrics = new List<Fabric>();
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabrics = new List<Fabric>();
                _oFabric = new Fabric();
                _oFabric.ErrorMessage = ex.Message;
                _oFabrics.Add(_oFabric);
            }
            var jsonResult = Json(_oFabrics, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public string MakeSqlForSearchingCriterias(Fabric oFabric)
        {
            string sReturn1 = "SELECT * FROM View_Fabric ";
            string sReturn = "";

            #region Fabric ID
            if (oFabric.FabricID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricID = " + oFabric.FabricID;
            }
            #endregion

            #region Fabric No
            if (!string.IsNullOrEmpty(oFabric.FabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricNo LIKE '%" + oFabric.FabricNo + "%' ";
            }
            #endregion

            #region Buyer ID
            if (oFabric.BuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID = " + oFabric.BuyerID;
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + "  order by IssueDate DESC";
            return sSQL;
        }


        #endregion

        #region FabricProcess
        public ActionResult View_FabricProcesss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricProcesses = ViewBag.EnumContractorTypes = Enum.GetValues(typeof(EnumFabricProcess)).Cast<EnumFabricProcess>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(oFabricProcesss);
        }

        [HttpPost]
        public JsonResult SaveFabricProcess(FabricProcess oFabricProcess)
        {
            try
            {
                oFabricProcess = oFabricProcess.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricProcess = new FabricProcess();
                oFabricProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFabricProcess(FabricProcess oFabricProcess)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricProcess.Delete(oFabricProcess.FabricProcessID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region
        public void Print_ReportXL(string sTempString)
        {

            _oFabrics = new List<Fabric>();
            Fabric oFabric = new Fabric();
            oFabric.Remarks = sTempString;
            string sSQL = MakeSQL(oFabric);
            _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty = 0;
            double nAmount = 0;
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
             oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add(oFabricPOSetup.FabricCode+" Report");
                sheet.Name = " Report";
                sheet.Column(3).Width = 22;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 35;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 30;


                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oFabricPOSetup.NoCode+" Register "; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Issue Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = oFabricPOSetup.NoCode+" Number"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Construction(P/I)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Process Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Fabric Design"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Width"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Style No "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Finish Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Weave"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Wash"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Finish"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Dyeing"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Print"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Approved By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion
                string sTemp = "";
                #region Data
                foreach (Fabric oItem in _oFabrics)
                {

                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.IssueDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.FabricNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.MKTPersonName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ConstructionPI; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.ProcessTypeName); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FabricDesignName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.FabricWidth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    sTemp = "";
                    if(oItem.IsWash)
                    {
                        sTemp = "Yes";
                    }

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = sTemp; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sTemp = "";
                    if (oItem.IsFinish)
                    {
                        sTemp = "Yes";
                    }

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = sTemp; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sTemp = "";
                    if (oItem.IsDyeing)
                    {
                        sTemp = "Yes";
                    }

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = sTemp; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sTemp = "";
                    if (oItem.IsPrint)
                    {
                        sTemp = "Yes";
                    }

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = sTemp; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.ApprovedByName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                //#region Total
                //cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 7]; cell.Value = nQty; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 9]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                //nRowIndex = nRowIndex + 1;
                //#endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        #endregion

        #region Fabric Update History

        [HttpPost]
        public JsonResult GetsFabricHistory(Fabric oFabric)
        {
            List<FabricSuggestionHistory> oFSHs = new List<FabricSuggestionHistory>();

            try
            {
                if (oFabric.FabricID > 0)
                {
                    string sSQL = "Select * from View_FabricSuggestionHistory Where FabricID=" + oFabric.FabricID;
                    oFSHs = FabricSuggestionHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFSHs = new List<FabricSuggestionHistory>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFSHs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Analysis
        public ActionResult PrintFabricAnalysis(int nFabricID, int buid)
        {
            _oFabric= new Fabric();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            _oFabric = _oFabric.Get(nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //MarketingAccount oFabricPOSetup = new FabricPOSetup();

            rptFabricAnalysis oReport = new rptFabricAnalysis();
            byte[] abytes = oReport.PrepareReport(_oFabric,oFabricPOSetup, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult MeetingSummaryPrint(int nFabricID, int nRefType, int buid)
        {
            _oFabric = new Fabric();
            _oFabric = _oFabric.Get(nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<MeetingSummary> oMeetingSummarys = new List<MeetingSummary>();
            oMeetingSummarys = MeetingSummary.GetsByRef(nFabricID, nRefType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptMeetingSummary oReport = new rptMeetingSummary();
            byte[] abytes = oReport.PrepareReport(_oFabric, oMeetingSummarys, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        #endregion

        public ActionResult OrderStatementPrint(int nFabricID, int buid)
        {
            bool isAuthorized = false;
            List<AuthorizationRoleMapping>  oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Fabric).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            foreach (AuthorizationRoleMapping obj in oAuthorizationRoleMappings)
            {
                if (obj.ModuleName == EnumModuleName.Fabric && obj.OperationType == EnumRoleOperationType.ViewCMValue)
                {
                    isAuthorized = true;
                }
            }
            Fabric oFabric = new Fabric();
            oFabric = oFabric.Get(nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            oFabricSCReports = FabricSCReport.Gets("SELECT * FROM View_FabricSalesContractReport WHERE FabricID =" + nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ExportPIRWU oExportPIRWU = new ExportPIRWU();
            List<ExportPIRWU> oExportPIRWUs = new List<ExportPIRWU>();
            oExportPIRWUs = ExportPIRWU.Gets("SELECT * FROM View_ExportPIReport_WU WHERE FabricID =" + nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            string sString = oFabricPOSetup.FabricCode;

            rptOrderStatement oReport = new rptOrderStatement();
            byte[] abytes = oReport.PrepareReport(oFabric, oFabricSCReports, oCompany, oBusinessUnit, sString, oExportPIRWUs, isAuthorized);
            return File(abytes, "application/pdf");
        }


        
    }
}
