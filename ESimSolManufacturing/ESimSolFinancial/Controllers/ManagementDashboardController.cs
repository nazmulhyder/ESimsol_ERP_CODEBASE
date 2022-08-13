using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class ManagementDashboardController : Controller
    {
        #region Declaration
        MgtDashBoardAccount _oMgtDashBoardAccount = new MgtDashBoardAccount();
        List<MgtDashBoardAccount> _oMgtDashBoardAccounts = new List<MgtDashBoardAccount>();
        #endregion

        #region Actions
        //this Dashboar for Dyeing/Plastic
        public ActionResult ViewMgtDashBoard(int nbuid, string sreportdate, int menuid) 
        {
            DateTime dReportDate = DateTime.Today;
            if (sreportdate != null && sreportdate != "")
            {
                dReportDate = Convert.ToDateTime(sreportdate);
            }
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
                        
            _oMgtDashBoardAccount = new MgtDashBoardAccount();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;

            #region Set BU
            if (nbuid <= 0)
            {
                if (oBusinessUnits.Count > 0)
                {
                    nbuid = oBusinessUnits[0].BusinessUnitID;
                    if (nbuid <= 0)
                    {
                        if (oUser.UserID == -9)
                        {
                            nbuid = 1;
                        }
                    }
                }
                else
                {
                    if (oUser.UserID == -9)
                    {
                        nbuid = 1;
                    }
                }
            }
            #endregion

            MgtDashBoardAccount oMgtDashBoardAccount = new MgtDashBoardAccount();
            oMgtDashBoardAccount.BUID = nbuid;
            oMgtDashBoardAccount.ReportDate = dReportDate;
            ViewBag.MgtDashBoardAccounts = MgtDashBoardAccount.Gets(oMgtDashBoardAccount, (int)Session[SessionInfo.currentUserID]);

            MgtDBObj oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Order_Summery;
            ViewBag.OrderSummerys = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Top_Five_Marketing_Performance;
            ViewBag.Top5MKTPerformances = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Highest_Produced_Product;
            ViewBag.HighestProducedProducts = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Top_Ten_Customer;
            ViewBag.TopTenCustomers = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Top_Five_Over_Due_Customer;
            ViewBag.TopFiveOverDueCustomers = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Highest_Selling_Product;
            ViewBag.HighestSellingProducts = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Export_PI_Issued_Vs_LCReceived;
            ViewBag.ExpPIIssueVsLCRcvs = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Export_Recevable_Vs_Import_Payable;
            ViewBag.ExportRecevableVsImportPayables = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;            
            oMgtDBObj.RefType = EnumMgtDBRefType.Stock_Summery;
            ViewBag.StockSummerys = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);

            ViewBag.DyeingForeCasts = DyeingForeCast.Gets(nbuid, EnumForecastLayout.Dyeing_Order_Issue, DateTime.Today.AddMonths(-3), DateTime.Today, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ForecastLayout = EnumObject.jGets(typeof(EnumForecastLayout));  

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nbuid, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = dReportDate;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefCaption = oBusinessUnit.Name;

            ViewBag.ForecastStartDate = (DateTime.Today.AddMonths(-3)).ToString("dd MMM yyyy");
            ViewBag.ForecastEndDate = (DateTime.Today).ToString("dd MMM yyyy");
            return View(oMgtDBObj);
        }
        #endregion

        #region Gets
        [HttpPost]
        public JsonResult GetsAccountsSummery(MgtDashBoardAccount oMgtDashBoardAccount)
        {
            try
            {
                _oMgtDashBoardAccounts = new List<MgtDashBoardAccount>();
                _oMgtDashBoardAccounts = MgtDashBoardAccount.Gets(oMgtDashBoardAccount, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMgtDashBoardAccount = new MgtDashBoardAccount();
                _oMgtDashBoardAccount.ErrorMessage = ex.Message;
                _oMgtDashBoardAccounts = new List<MgtDashBoardAccount>();
                _oMgtDashBoardAccounts.Add(_oMgtDashBoardAccount);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMgtDashBoardAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsMgtDBData(MgtDBObj oMgtDBObj)
        {
            List<MgtDBObj> oMgtDBObjs = new List<MgtDBObj>();
            try
            {
                oMgtDBObj.RefType = (EnumMgtDBRefType)oMgtDBObj.RefTypeInt;
                oMgtDBObjs = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                MgtDBObj oTempMgtDBObj = new MgtDBObj();
                oTempMgtDBObj.ErrorMessage = ex.Message;
                oMgtDBObjs.Add(oTempMgtDBObj);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMgtDBObjs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PI Issue vs LC Received
        public ActionResult ViewMgtPIvsLC(int nbuid, string sreportdate) 
        {
            DateTime dReportDate = DateTime.Today;
            if (sreportdate != null && sreportdate != "")
            {
                dReportDate = Convert.ToDateTime(sreportdate);
            }
            this.Session.Remove(SessionInfo.MenuID);
           // this.Session.Add(SessionInfo.MenuID, menuid);
                        
            _oMgtDashBoardAccount = new MgtDashBoardAccount();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;

            #region Set BU
            if (nbuid <= 0)
            {
                if (oBusinessUnits.Count > 0)
                {
                    nbuid = oBusinessUnits[0].BusinessUnitID;
                    if (nbuid <= 0)
                    {
                        if (oUser.UserID == -9)
                        {
                            nbuid = 1;
                        }
                    }
                }
                else
                {
                    if (oUser.UserID == -9)
                    {
                        nbuid = 1;
                    }
                }
            }
            #endregion
            
            MgtDBObj oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Export_PI_Issued_Vs_LCReceived;
            ViewBag.ExpPIIssueVsLCRcvsFullView = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);


            oMgtDBObj = new MgtDBObj();
         


            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nbuid, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = dReportDate;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefCaption = oBusinessUnit.Name;
            return View(oMgtDBObj);
        }
        #endregion

        #region Export vs Import
        public ActionResult ViewMgtExportvsImport(int nbuid, string sreportdate)
        {
            DateTime dReportDate = DateTime.Today;
            if (sreportdate != null && sreportdate != "")
            {
                dReportDate = Convert.ToDateTime(sreportdate);
            }
            this.Session.Remove(SessionInfo.MenuID);
            // this.Session.Add(SessionInfo.MenuID, menuid);

            _oMgtDashBoardAccount = new MgtDashBoardAccount();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;

            #region Set BU
            if (nbuid <= 0)
            {
                if (oBusinessUnits.Count > 0)
                {
                    nbuid = oBusinessUnits[0].BusinessUnitID;
                    if (nbuid <= 0)
                    {
                        if (oUser.UserID == -9)
                        {
                            nbuid = 1;
                        }
                    }
                }
                else
                {
                    if (oUser.UserID == -9)
                    {
                        nbuid = 1;
                    }
                }
            }
            #endregion

            MgtDBObj oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = DateTime.Today;
            oMgtDBObj.RefType = EnumMgtDBRefType.Export_Recevable_Vs_Import_Payable;
            ViewBag.ExportRecevableVsImportPayablesFullView = MgtDBObj.Gets(oMgtDBObj, (int)Session[SessionInfo.currentUserID]);


            oMgtDBObj = new MgtDBObj();



            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nbuid, (int)Session[SessionInfo.currentUserID]);

            oMgtDBObj = new MgtDBObj();
            oMgtDBObj.BUID = nbuid;
            oMgtDBObj.ReportDate = dReportDate;
            oMgtDBObj.StartDate = DateTime.Today.AddMonths(-1);
            oMgtDBObj.EndDate = DateTime.Today;
            oMgtDBObj.RefCaption = oBusinessUnit.Name;
            return View(oMgtDBObj);
        }
        #endregion

        #region Full View & Print Forecast
        public ActionResult ViewProductionForeCastFull(int buid, int menuid)
        {
            if (menuid > 0)
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
            }

            List<DyeingForeCast> oDyeingForeCasts = new List<DyeingForeCast>();
            oDyeingForeCasts = DyeingForeCast.Gets(buid, EnumForecastLayout.Dyeing_Order_Issue, DateTime.Today.AddMonths(-3), DateTime.Today, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.ForecastLayout = EnumObject.jGets(typeof(EnumForecastLayout));

            ViewBag.ForecastStartDate = (DateTime.Today.AddMonths(-3)).ToString("dd MMM yyyy");
            ViewBag.ForecastEndDate = (DateTime.Today).ToString("dd MMM yyyy");
            return View(oDyeingForeCasts);
        }
        [HttpPost]
        public JsonResult GetsDyeingForeCast(DyeingForeCast oDyeingForeCast)
        {
            List<DyeingForeCast> oDyeingForeCasts = new List<DyeingForeCast>();
            try
            {
                oDyeingForeCasts = DyeingForeCast.Gets(oDyeingForeCast.BUID, (EnumForecastLayout)oDyeingForeCast.ForecastLayoutInt, oDyeingForeCast.StartDate, oDyeingForeCast.EndDate, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                DyeingForeCast oTempDyeingForeCast = new DyeingForeCast();
                oTempDyeingForeCast.ErrorMessage = ex.Message;
                oDyeingForeCasts.Add(oTempDyeingForeCast);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingForeCasts);
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
        public ActionResult SetSessionSearchCriteria(DyeingForeCast oDyeingForeCast)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDyeingForeCast);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintDyeingForeCastDetails(double ts)
        {
            string sErrorMessage = "";
            DyeingForeCast oDyeingForeCast = new DyeingForeCast();
            List<DyeingForeCast> _oDyeingForeCasts = new List<DyeingForeCast>();            
            try
            {
                oDyeingForeCast = (DyeingForeCast)Session[SessionInfo.ParamObj];
                oDyeingForeCast.DyeingType = (EumDyeingType)oDyeingForeCast.DyeingTypeInt;
                oDyeingForeCast.ForecastLayout = (EnumForecastLayout)oDyeingForeCast.ForecastLayoutInt;                
                _oDyeingForeCasts = DyeingForeCast.GetsDetails(oDyeingForeCast.BUID, oDyeingForeCast.DyeingType, oDyeingForeCast.ForecastLayout, oDyeingForeCast.StartDate, oDyeingForeCast.EndDate, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }

            if (sErrorMessage == "")
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();                
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oBusinessUnit = oBusinessUnit.Get(oDyeingForeCast.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptDyeingForecastDetails oReport = new rptDyeingForecastDetails();
                byte[] abytes = oReport.PrepareReport(_oDyeingForeCasts, oDyeingForeCast, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] abytes = oErrorReport.PrepareReport(sErrorMessage);
                return File(abytes, "application/pdf");
            }
        }
        #endregion
    }
}