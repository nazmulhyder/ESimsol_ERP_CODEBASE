using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ReportManagement;
using ICS.Core.Utility;
using ESimSol.Reports;
//using iTextSharp.text;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class ProductionCostController : Controller
    {

        #region Production Cost Report  By Sagor on 08 Aug 2014

        #region Declaration
        ProductionCost _oPC = new ProductionCost();
        List<ProductionCost> _oPCs = new List<ProductionCost>();
        ProductionCostReDyeing _oPCRD = new ProductionCostReDyeing();
        List<ProductionCostReDyeing> _oPCRDs = new List<ProductionCostReDyeing>();
        RptProductionCostAnalysis _oRptProductionCostAnalysis = new RptProductionCostAnalysis();
        List<RptProductionCostAnalysis> _RptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();
        int BUID = 0;
        #endregion

        #region Views

        public ActionResult ViewProductionCosts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oPC = new ProductionCost();
            ViewBag.Title = "ProductionCost";
            return View(_oPCs);
        }

        public ActionResult ViewProductionCostReDyeings(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oPC = new ProductionCost();
            ViewBag.Title = "ProductionCostReDyeing";
            return View(_oPCs);
        }

        #endregion

        #region Search Production Cost
        [HttpPost]
        public JsonResult Search(string sValue, double nts)
        {
            int nDateType= Convert.ToInt32( sValue.Split('~')[0]);
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            string sBuyerIDs = (sValue.Split('~')[3]).Trim();
            string sRouteSheetNos = (sValue.Split('~')[4]).Trim();
            string sPTUIDs = (sValue.Split('~')[5]).Trim();

            _oPCs = new List<ProductionCost>();

            try
            {

                if (nDateType == 0)
                {
                    StartDate = DateTime.MinValue;
                    EndDate = DateTime.MinValue;
                    StartDate = new DateTime(1900, StartDate.Month, StartDate.Day);
                    EndDate = new DateTime(1900, EndDate.Month, EndDate.Day);
                }
                else if (nDateType == 1)
                {
                    EndDate = StartDate.AddHours(23);
                    EndDate = EndDate.AddMinutes(59);
                    EndDate = EndDate.AddSeconds(59);
                }
                _oPCs = ProductionCost.Gets(StartDate,EndDate,sBuyerIDs,sRouteSheetNos, sPTUIDs,((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oPCs.Count <= 0)
                {
                    throw new Exception("No data found.");
                }

            }
            catch (Exception ex)
            {
                _oPC = new ProductionCost();
                _oPC.ErrorMessage = ex.Message;
                _oPCs.Add(_oPC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByViewType(string sValue, int nViewType, double nts)
        {
            _oPCs = new List<ProductionCost>();
            _oPCs = GetsProductionCost(sValue, nViewType);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public List<ProductionCost> GetsProductionCost(string sValue, int nViewType)
        {
            int nDateType = Convert.ToInt32(sValue.Split('~')[0]);
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            string sBuyerIDs = (sValue.Split('~')[3]).Trim();
            string sRouteSheetNos = (sValue.Split('~')[4]).Trim();
            string sPTUIDs = (sValue.Split('~')[5]).Trim();

            _oPCs = new List<ProductionCost>();
            // var oPCs = (dynamic)null;
            try
            {

                if (nDateType == 0)
                {
                    StartDate = DateTime.MinValue;
                    EndDate = DateTime.MinValue;
                    StartDate = new DateTime(1900, StartDate.Month, StartDate.Day);
                    EndDate = new DateTime(1900, EndDate.Month, EndDate.Day);
                }
                else if (nDateType == 1)
                {
                    EndDate = StartDate.AddHours(23);
                    EndDate = EndDate.AddMinutes(59);
                    EndDate = EndDate.AddSeconds(59);
                }
                _oPCs = ProductionCost.Gets(StartDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oPCs.Count <= 0)
                {
                    throw new Exception("No data found.");
                }

                else
                {
                    if (nViewType == 2) // Buyer Wise
                    {
                        var oPCs = from oPC in _oPCs
                                group oPC by oPC.BuyerName into oTempPC
                                select new
                                {
                                    BuyerName = oTempPC.Key,
                                    TotalYarnValue =oTempPC.Sum(oPC => oPC.TotalYarnValue),
                                    TotalDyesValue = oTempPC.Sum(oPC => oPC.TotalDyesValue),
                                    TotalChemicalValue = oTempPC.Sum(oPC => oPC.TotalChemicalValue),
                                    TotalDyesChemicalValue=oTempPC.Sum(oPC => oPC.TotalDyesChemicalValue),
                                    TotalValue = oTempPC.Sum(oPC => oPC.TotalValue),
                                };
                        _oPCs = new List<ProductionCost>();
                        foreach (var oItem in oPCs)
                        {
                            _oPC = new ProductionCost();
                            _oPC.BuyerName = oItem.BuyerName;
                            _oPC.TYarnValue = oItem.TotalYarnValue;
                            _oPC.TDyesValue = oItem.TotalDyesValue;
                            _oPC.TChemicalValue = oItem.TotalChemicalValue;
                            _oPC.TDCValue = oItem.TotalDyesChemicalValue;
                            _oPC.TValue = oItem.TotalValue;
                            _oPCs.Add(_oPC);
                        }
                    }
                    else if (nViewType == 3) // Execution Wise
                    {
                        var oPCs = from oPC in _oPCs
                                group oPC by oPC.OrderNo into oTempPC
                                select new
                                {
                                    OrderNo = oTempPC.Key,
                                    TotalYarnValue = oTempPC.Sum(oPC => oPC.TotalYarnValue),
                                    TotalDyesValue = oTempPC.Sum(oPC => oPC.TotalDyesValue),
                                    TotalChemicalValue = oTempPC.Sum(oPC => oPC.TotalChemicalValue),
                                    TotalDyesChemicalValue = oTempPC.Sum(oPC => oPC.TotalDyesChemicalValue),
                                    TotalValue = oTempPC.Sum(oPC => oPC.TotalValue),
                                };
                        _oPCs = new List<ProductionCost>();
                        foreach (var oItem in oPCs)
                        {
                            _oPC = new ProductionCost();
                            _oPC.OrderNo = oItem.OrderNo;
                            _oPC.TYarnValue = oItem.TotalYarnValue;
                            _oPC.TDyesValue = oItem.TotalDyesValue;
                            _oPC.TChemicalValue = oItem.TotalChemicalValue;
                            _oPC.TDCValue = oItem.TotalDyesChemicalValue;
                            _oPC.TValue = oItem.TotalValue;
                            _oPCs.Add(_oPC);
                        }

                    }
                    else if (nViewType == 4) // Day Wise
                    {
                        var oPCs = from oPC in _oPCs
                                group oPC by oPC.ProductionDateInString into oTempPC
                                select new
                                {
                                    ProductionDateInString = oTempPC.Key,
                                    TotalYarnValue = oTempPC.Sum(oPC => oPC.TotalYarnValue),
                                    TotalDyesValue = oTempPC.Sum(oPC => oPC.TotalDyesValue),
                                    TotalChemicalValue = oTempPC.Sum(oPC => oPC.TotalChemicalValue),
                                    TotalDyesChemicalValue = oTempPC.Sum(oPC => oPC.TotalDyesChemicalValue),
                                    TotalValue = oTempPC.Sum(oPC => oPC.TotalValue),
                                };
                        _oPCs = new List<ProductionCost>();
                        foreach (var oItem in oPCs)
                        {
                            _oPC = new ProductionCost();
                            _oPC.PDateInString = oItem.ProductionDateInString;
                            _oPC.TYarnValue = oItem.TotalYarnValue;
                            _oPC.TDyesValue = oItem.TotalDyesValue;
                            _oPC.TChemicalValue = oItem.TotalChemicalValue;
                            _oPC.TDCValue = oItem.TotalDyesChemicalValue;
                            _oPC.TValue = oItem.TotalValue;
                            _oPCs.Add(_oPC);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _oPCs = new List<ProductionCost>();
                _oPC = new ProductionCost();
                _oPC.ErrorMessage = ex.Message;
                _oPCs.Add(_oPC);
            }
            return _oPCs;
        }

        #endregion

        #region Print Production Cost
        public ActionResult PrintProductionCost(string sValue, int nViewType)
        {
            int nDateType = Convert.ToInt32(sValue.Split('~')[0]);
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            string sDateRange = "";
            if (nDateType == 0)
            {
                sDateRange = "";
            }
            else if (nDateType == 1)
            {
                sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy") + " - " + EndDate.ToString("dd MMM yyyy");
            }

            _oPCs = new List<ProductionCost>();
            _oPCs = GetsProductionCost(sValue, nViewType);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptProductionCost oReport = new rptProductionCost();
            byte[] abytes = oReport.PrepareReport(_oPCs, oCompany, sDateRange, nViewType);
            return File(abytes, "application/pdf");
        }
        
        #endregion

        #region Search Production Cost Redying
        [HttpPost]
        public JsonResult SearchReDyeing(string sValue, double nts)
        {
            int nDateType = Convert.ToInt32(sValue.Split('~')[0]);
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            string sBuyerIDs = (sValue.Split('~')[3]).Trim();
            string sRouteSheetNos = (sValue.Split('~')[4]).Trim();
            string sPTUIDs = (sValue.Split('~')[5]).Trim();

            _oPCRDs = new List<ProductionCostReDyeing>();

            try
            {

                if (nDateType == 0)
                {
                    StartDate = DateTime.MinValue;
                    EndDate = DateTime.MinValue;
                    StartDate = new DateTime(1900, StartDate.Month, StartDate.Day);
                    EndDate = new DateTime(1900, EndDate.Month, EndDate.Day);
                }
                else if (nDateType == 1)
                {
                    EndDate = StartDate.AddHours(23);
                    EndDate = EndDate.AddMinutes(59);
                    EndDate = EndDate.AddSeconds(59);
                }
                _oPCRDs = ProductionCostReDyeing.Gets(StartDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oPCRDs.Count <= 0)
                {
                    throw new Exception("No data found.");
                }

            }
            catch (Exception ex)
            {
                _oPCRD = new ProductionCostReDyeing();
                _oPCRD.ErrorMessage = ex.Message;
                _oPCRDs.Add(_oPCRD);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPCRDs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByViewTypeReDyeing(string sValue, int nViewType, double nts)
        {
            _oPCRDs = new List<ProductionCostReDyeing>();
            _oPCRDs = GetsProductionCostReDyeing(sValue, nViewType);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPCRDs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public List<ProductionCostReDyeing> GetsProductionCostReDyeing(string sValue, int nViewType)
        {
            int nDateType = Convert.ToInt32(sValue.Split('~')[0]);
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            string sBuyerIDs = (sValue.Split('~')[3]).Trim();
            string sRouteSheetNos = (sValue.Split('~')[4]).Trim();
            string sPTUIDs = (sValue.Split('~')[5]).Trim();

            _oPCRDs = new List<ProductionCostReDyeing>();
            // var oPCs = (dynamic)null;
            try
            {

                if (nDateType == 0)
                {
                    StartDate = DateTime.MinValue;
                    EndDate = DateTime.MinValue;
                    StartDate = new DateTime(1900, StartDate.Month, StartDate.Day);
                    EndDate = new DateTime(1900, EndDate.Month, EndDate.Day);
                }
                else if (nDateType == 1)
                {
                    EndDate = StartDate.AddHours(23);
                    EndDate = EndDate.AddMinutes(59);
                    EndDate = EndDate.AddSeconds(59);
                }
                _oPCRDs = ProductionCostReDyeing.Gets(StartDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oPCRDs.Count <= 0)
                {
                    throw new Exception("No data found.");
                }

                else
                {
                    if (nViewType == 2) // Buyer Wise
                    {
                        var oPCRDs = from oPCRD in _oPCRDs
                                   group oPCRD by oPCRD.BuyerName into oTempPC
                                   select new
                                   {
                                       BuyerName = oTempPC.Key,
                                       TotalYarnValue = oTempPC.Sum(oPCRD => oPCRD.TotalYarnValue),
                                       TotalDyesValue = oTempPC.Sum(oPCRD => oPCRD.TotalDyesValue),
                                       TotalChemicalValue = oTempPC.Sum(oPCRD => oPCRD.TotalChemicalValue),
                                       TotalDyesChemicalValue = oTempPC.Sum(oPCRD => oPCRD.TotalDyesChemicalValue),
                                       TotalValue = oTempPC.Sum(oPCRD => oPCRD.TotalValue),

                                       ReDyeBuyerName = oTempPC.Key,
                                       ReDyeTotalYarnValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalYarnValue),
                                       ReDyeTotalDyesValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalDyesValue),
                                       ReDyeTotalChemicalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalChemicalValue),
                                       ReDyeTotalDyesChemicalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalDyesChemicalValue),
                                       ReDyeTotalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalValue),
                                   };
                        _oPCRDs = new List<ProductionCostReDyeing>();
                        foreach (var oItem in oPCRDs)
                        {
                            _oPCRD = new ProductionCostReDyeing();
                            _oPCRD.BuyerName = oItem.BuyerName;
                            _oPCRD.TYarnValue = oItem.TotalYarnValue;
                            _oPCRD.TDyesValue = oItem.TotalDyesValue;
                            _oPCRD.TChemicalValue = oItem.TotalChemicalValue;
                            _oPCRD.TDCValue = oItem.TotalDyesChemicalValue;
                            _oPCRD.TValue = oItem.TotalValue;

                            _oPCRD.ReDyeBuyerName = oItem.ReDyeBuyerName;
                            _oPCRD.ReDyeTYarnValue = oItem.ReDyeTotalYarnValue;
                            _oPCRD.ReDyeTDyesValue = oItem.ReDyeTotalDyesValue;
                            _oPCRD.ReDyeTChemicalValue = oItem.ReDyeTotalChemicalValue;
                            _oPCRD.ReDyeTDCValue = oItem.ReDyeTotalDyesChemicalValue;
                            _oPCRD.ReDyeTValue = oItem.ReDyeTotalValue;
                            _oPCRDs.Add(_oPCRD);
                        }
                    }
                    else if (nViewType == 3) // Execution Wise
                    {
                        var oPCRDs = from oPCRD in _oPCRDs
                                   group oPCRD by oPCRD.OrderNo into oTempPC
                                   select new
                                   {
                                       OrderNo = oTempPC.Key,
                                       TotalYarnValue = oTempPC.Sum(oPCRD => oPCRD.TotalYarnValue),
                                       TotalDyesValue = oTempPC.Sum(oPCRD => oPCRD.TotalDyesValue),
                                       TotalChemicalValue = oTempPC.Sum(oPCRD => oPCRD.TotalChemicalValue),
                                       TotalDyesChemicalValue = oTempPC.Sum(oPCRD => oPCRD.TotalDyesChemicalValue),
                                       TotalValue = oTempPC.Sum(oPCRD => oPCRD.TotalValue),

                                       ReDyeBuyerName = oTempPC.Key,
                                       ReDyeTotalYarnValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalYarnValue),
                                       ReDyeTotalDyesValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalDyesValue),
                                       ReDyeTotalChemicalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalChemicalValue),
                                       ReDyeTotalDyesChemicalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalDyesChemicalValue),
                                       ReDyeTotalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalValue),
                                   };
                        _oPCRDs = new List<ProductionCostReDyeing>();
                        foreach (var oItem in oPCRDs)
                        {
                            _oPCRD = new ProductionCostReDyeing();
                            _oPCRD.OrderNo = oItem.OrderNo;
                            _oPCRD.TYarnValue = oItem.TotalYarnValue;
                            _oPCRD.TDyesValue = oItem.TotalDyesValue;
                            _oPCRD.TChemicalValue = oItem.TotalChemicalValue;
                            _oPCRD.TDCValue = oItem.TotalDyesChemicalValue;
                            _oPCRD.TValue = oItem.TotalValue;
                            _oPCRD.ReDyeBuyerName = oItem.ReDyeBuyerName;
                            _oPCRD.ReDyeTYarnValue = oItem.ReDyeTotalYarnValue;
                            _oPCRD.ReDyeTDyesValue = oItem.ReDyeTotalDyesValue;
                            _oPCRD.ReDyeTChemicalValue = oItem.ReDyeTotalChemicalValue;
                            _oPCRD.ReDyeTDCValue = oItem.ReDyeTotalDyesChemicalValue;
                            _oPCRD.ReDyeTValue = oItem.ReDyeTotalValue;
                            _oPCRDs.Add(_oPCRD);
                        }

                    }
                    else if (nViewType == 4) // Day Wise
                    {
                        var oPCRDs = from oPCRD in _oPCRDs
                                     group oPCRD by oPCRD.ProductionDateInString into oTempPC
                                   select new
                                   {
                                       ProductionDateInString = oTempPC.Key,
                                       TotalYarnValue = oTempPC.Sum(oPCRD => oPCRD.TotalYarnValue),
                                       TotalDyesValue = oTempPC.Sum(oPCRD => oPCRD.TotalDyesValue),
                                       TotalChemicalValue = oTempPC.Sum(oPCRD => oPCRD.TotalChemicalValue),
                                       TotalDyesChemicalValue = oTempPC.Sum(oPCRD => oPCRD.TotalDyesChemicalValue),
                                       TotalValue = oTempPC.Sum(oPCRD => oPCRD.TotalValue),

                                       ReDyeBuyerName = oTempPC.Key,
                                       ReDyeTotalYarnValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalYarnValue),
                                       ReDyeTotalDyesValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalDyesValue),
                                       ReDyeTotalChemicalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalChemicalValue),
                                       ReDyeTotalDyesChemicalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalDyesChemicalValue),
                                       ReDyeTotalValue = oTempPC.Sum(oPCRD => oPCRD.ReDyeTotalValue),
                                   };
                        _oPCRDs = new List<ProductionCostReDyeing>();
                        foreach (var oItem in oPCRDs)
                        {
                            _oPCRD = new ProductionCostReDyeing();
                            _oPCRD.PDateInString = oItem.ProductionDateInString;
                            _oPCRD.TYarnValue = oItem.TotalYarnValue;
                            _oPCRD.TDyesValue = oItem.TotalDyesValue;
                            _oPCRD.TChemicalValue = oItem.TotalChemicalValue;
                            _oPCRD.TDCValue = oItem.TotalDyesChemicalValue;
                            _oPCRD.TValue = oItem.TotalValue;
                            _oPCRD.ReDyeBuyerName = oItem.ReDyeBuyerName;
                            _oPCRD.ReDyeTYarnValue = oItem.ReDyeTotalYarnValue;
                            _oPCRD.ReDyeTDyesValue = oItem.ReDyeTotalDyesValue;
                            _oPCRD.ReDyeTChemicalValue = oItem.ReDyeTotalChemicalValue;
                            _oPCRD.ReDyeTDCValue = oItem.ReDyeTotalDyesChemicalValue;
                            _oPCRD.ReDyeTValue = oItem.ReDyeTotalValue;
                            _oPCRDs.Add(_oPCRD);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _oPCRDs = new List<ProductionCostReDyeing>();
                _oPCRD = new ProductionCostReDyeing();
                _oPCRD.ErrorMessage = ex.Message;
                _oPCRDs.Add(_oPCRD);
            }
            return _oPCRDs;
        }

        #endregion

        #region Print Production Cost Redyeing
        public ActionResult PrintProductionCostReDyeing(string sValue, int nViewType)
        {
            int nDateType = Convert.ToInt32(sValue.Split('~')[0]);
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            string sDateRange = "";
            if (nDateType == 0)
            {
                sDateRange = "";
            }
            else if (nDateType == 1)
            {
                sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy") + " - " + EndDate.ToString("dd MMM yyyy");
            }

            _oPCRDs = new List<ProductionCostReDyeing>();
            _oPCRDs = GetsProductionCostReDyeing(sValue, nViewType);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptProductionCostReDyeing oReport = new rptProductionCostReDyeing();
            byte[] abytes = oReport.PrepareReport(_oPCRDs, oCompany, sDateRange, nViewType);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Get Company Logo

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
        #endregion

        #region Search By Job
        //[HttpPost]
        //public JsonResult SearchByJob(string sValue, double nts)
        //{
        //    List<PTU> oPTUS = new List<PTU>();
        //    PTU oPTU = new PTU();
        //    try
        //    {
        //        string sJobCode = sValue.Split('~')[0].Trim();
        //        string sJobNo = sValue.Split('~')[1].Trim();
        //        string sJobYear = sValue.Split('~')[2].Trim();
        //        string sDeoCode = sJobCode + "-" + sJobNo + "/" + sJobYear;
        //        string sSQL = "";
        //        sSQL = "where OrderID in ( select DEOID from DyeingExecutionOrder where Code = '" + sDeoCode + "') ";
        //        oPTUS = PTU.GetsforWeb(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        if (oPTUS.Count <= 0)
        //        {
        //            throw new Exception("No data found.");
        //        }
        //        else
        //        {
        //            oPTU = oPTUS[0];
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oPTU = new PTU();
        //        oPTU.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oPTU);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #endregion

        #region Production Cost Analysis Added By 22 Jan Sagor 2015

        public ActionResult ViewProductionCostAnalysis(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            ViewBag.BUID = buid;
            return View(oRPCAs);
        }
        public ActionResult ViewDUProductionRFT(int menuid,int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            ViewBag.BUID = buid;
            BUID = buid;
            return View(oRPCAs);
        }

        public ActionResult ViewProductionPerformanceAnalysis(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RptProductionCostAnalysis> oRpcas = new List<RptProductionCostAnalysis>();
            return View(oRpcas);
        }

        [HttpPost]
        public JsonResult SearchProductionCostAnalysis(string sValue, double nts)
        {
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);

            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRPCAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchDUProductionRFT(string sValue, double nts)
        {
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);

            //StartTime = StartTime.AddHours(10);
            //EndTime = EndTime.AddHours(10);

            oRPCAs = GetsDUProductionRFT(nPSSID, IsDate, StartTime, EndTime);

            var jsonResult = Json(oRPCAs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

          
        }

     
        public ActionResult PrintDUProductionRFT(string sValue, double nts)
        {           
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);

            //StartTime = StartTime.AddHours(09);
            //EndTime = EndTime.AddDays(1).AddHours(09);
            oRPCAs = GetsDUProductionRFT(nPSSID, IsDate, StartTime, EndTime);
            _RptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();
            _RptProductionCostAnalysiss = oRPCAs;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptDUProductionRFT oReport = new rptDUProductionRFT();
            byte[] abytes = oReport.PrepareReport(_RptProductionCostAnalysiss, oCompany, "");
            return File(abytes, "application/pdf");           
        }
     
    
        public ActionResult PrintProductionRFTSummary(string sValue, double nts)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            Company oCompany = new Company();
            string dateRange = "";
            try
            {
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
                int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
                bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
                DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
                DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);

                //StartTime = StartTime.AddHours(09);
                //EndTime = EndTime.AddDays(1).AddHours(09);
               // oRPCAs = GetsDUProductionRFT(nPSSID, IsDate, StartTime, EndTime);
                oRPCAs = RptProductionCostAnalysis.MailContentDUProductionRFT(nPSSID, StartTime, EndTime, 2, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 dateRange = "Date : " + StartTime + " To " + EndTime;
                _RptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();
                _RptProductionCostAnalysiss = oRPCAs;
               
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            rptProductionRFTSummary oReport = new rptProductionRFTSummary();
            byte[] abytes = oReport.PrepareReport(_RptProductionCostAnalysiss, dateRange, oCompany, oBusinessUnit, "");
            return File(abytes, "application/pdf");

        }
        #region ExcelDUProductionRFT
     
        public void PrintExcelDUProductionRFT(string sValue, double nts)
        {
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            //StartTime = StartTime.AddHours(09);
            //EndTime = EndTime.AddDays(1).AddHours(09);
            //oRPCAs = GetsDUProductionRFT(nPSSID, IsDate, StartTime, EndTime);
            oRPCAs = RptProductionCostAnalysis.MailContentDUProductionRFT(nPSSID, StartTime, EndTime, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            try
            {
                 Company oCompany = new Company();
                _RptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();
                _RptProductionCostAnalysiss = oRPCAs;
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                if (_RptProductionCostAnalysiss == null)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }     
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int nTotalCol = 0;
                    int nCount = 0;
                    double AvgDz = 0;
                    int colIndex = 2;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Production RFT");
                        sheet.Name = "Production List";
                        sheet.Column(colIndex++).Width = 10; //SL
                        sheet.Column(colIndex++).Width = 20; //Machine No
                        sheet.Column(colIndex++).Width = 20; //Load Data
                        sheet.Column(colIndex++).Width = 20; //Unload Data
                        sheet.Column(colIndex++).Width = 25; //Buyer Name
                        sheet.Column(colIndex++).Width = 20; //Order No
                        sheet.Column(colIndex++).Width = 20; //Batch No
                        sheet.Column(colIndex++).Width = 35; //Yarn Type
                        sheet.Column(colIndex++).Width = 20; //Color
                        sheet.Column(colIndex++).Width = 20; //Shade
                        sheet.Column(colIndex++).Width = 20; //Shade(%)
                        sheet.Column(colIndex++).Width = 20; //Liqior
                        sheet.Column(colIndex++).Width = 20; //Batch QTY(kg)
                        sheet.Column(colIndex++).Width = 20; //Lapdip No
                        sheet.Column(colIndex++).Width = 20; //Addition  
                        sheet.Column(colIndex++).Width = 20; //In house
                        sheet.Column(colIndex++).Width = 20; //Order Type
                        sheet.Column(colIndex++).Width = 20; //Is Dyeing
                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 17].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 17].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Production RPM"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Machine No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Load Date"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Upload Date"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Color"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Shade(%)"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Shade Name"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Liquior"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch Qty(kg)"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Lap Dip No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Addition"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "In House"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Is Redyeing"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Combine No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;   

                        rowIndex++;
                        #endregion

                        #region Report Body
                        int nSL = 1;
                        colIndex = 2;
                        foreach (RptProductionCostAnalysis oItem in _RptProductionCostAnalysiss)
                        {
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex,colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MachineName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StartDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EndDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;  cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RouteSheetNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ColorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShadePercentage; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShadeName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Liquor; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LabdipNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AddCount; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.IsInHouseST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderTypeSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.IsReDyeingST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CombineRSNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            nSL++;
                            rowIndex++;
                        }

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_DUProductionRFT.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();

                    }
               
            }
            catch (Exception ex)
            {
                _RptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();
                _oRptProductionCostAnalysis = new RptProductionCostAnalysis();
                _oRptProductionCostAnalysis.ErrorMessage = ex.Message;
                _RptProductionCostAnalysiss.Add(_oRptProductionCostAnalysis);
            }
            #endregion

        }
        #endregion

        public List<RptProductionCostAnalysis> GetsDUProductionRFT(int PSSID, bool IsDate, DateTime StartTime, DateTime EndTime)
        {
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            RptProductionCostAnalysis oRPCA = new RptProductionCostAnalysis();
            try
            {
                if (PSSID <= 0 && !IsDate) { throw new Exception("Please select searching criteria."); }
                if (!IsDate)
                {
                    StartTime = DateTime.MinValue;
                    EndTime = DateTime.MinValue;
                    StartTime = new DateTime(1900, StartTime.Month, StartTime.Day);
                    EndTime = new DateTime(1900, EndTime.Month, EndTime.Day);
                }
                oRPCAs = RptProductionCostAnalysis.MailContentDUProductionRFT(PSSID, StartTime, EndTime, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRPCAs.Count() <= 0) { throw new Exception("No Information Found."); }
            }
            catch (Exception ex)
            {
                oRPCA.ErrorMessage = ex.Message;
                oRPCAs = new List<RptProductionCostAnalysis>();
                oRPCAs.Add(oRPCA);
            }

            return oRPCAs;
        }

        public ActionResult PrintProductionAnalysis(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != ""){ oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }
            rptProductionAnalysis oReport = new rptProductionAnalysis();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionAnalysisCost(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != ""){ oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }
            rptProductionAnalysisWithCost oReport = new rptProductionAnalysisWithCost();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionAnalysisSummary(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            List<ReportComments> oRCs = new List<ReportComments>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count() > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }
            if (oRPCAs.Count() > 0)
            {

                //string sSQL = "Select * from ReportComments Where CommentDate Between '" + oRPCAs.Min(x => x.EndTime).ToString("dd MMM yyyy") + "' And '" + oRPCAs.Max(x => x.EndTime).ToString("dd MMM yyyy") + "'";
                //oRCs = ReportComments.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            
            rptProductionCostSummary oReport = new rptProductionCostSummary();
            byte[] abytes = oReport.PrepareReport(oRPCAs,oRCs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionAnalysisShadeGroupSummary(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }

            rptProductionAnalysisWithShadeSummary oReport = new rptProductionAnalysisWithShadeSummary();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionAnalysisByMachine(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }

            rptProductionAnalysisByMachine oReport = new rptProductionAnalysisByMachine();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionAnalysisShade(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }
            rptProductionAnalysisShade oReport = new rptProductionAnalysisShade();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionSummaryWithCostAnalysis(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }
            rptProductionSummaryWithCostAnalysis oReport = new rptProductionSummaryWithCostAnalysis();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionWithCostAnalysis(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            List<ReportComments> oRCs = new List<ReportComments>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }

            if (oRPCAs.Count >0)
            {
                //string sSQL="Select * from ReportComments Where CommentDate Between '"+oRPCAs.Min(x => x.EndTime).ToString("dd MMM yyyy")+"' And '"+oRPCAs.Max(x => x.EndTime).ToString("dd MMM yyyy")+"'";
                //oRCs = ReportComments.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            rptProductionWithCostAnalysis oReport = new rptProductionWithCostAnalysis();
            byte[] abytes = oReport.PrepareReport(oRPCAs,oRCs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionAnalysisWithSummary(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            List<ReportComments> oRCs = new List<ReportComments>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(10);
            EndTime = EndTime.AddDays(1).AddHours(10);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }

            if (oRPCAs.Count > 0)
            {

                //string sSQL = "Select * from ReportComments Where CommentDate Between '" + oRPCAs.Min(x => x.EndTime).ToString("dd MMM yyyy") + "' And '" + oRPCAs.Max(x => x.EndTime).ToString("dd MMM yyyy") + "'";
                //oRCs = ReportComments.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            rptProductionAnalysisSummary oReport = new rptProductionAnalysisSummary();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oRCs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintProductionSummaryByMachine(string sValue, double nts)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            int nPSSID = Convert.ToInt32(sValue.Split('~')[0]);
            bool IsDate = Convert.ToBoolean(sValue.Split('~')[1]);
            DateTime StartTime = Convert.ToDateTime(sValue.Split('~')[2]);
            DateTime EndTime = Convert.ToDateTime(sValue.Split('~')[3]);
            string sDateRange = "";

            StartTime = StartTime.AddHours(09);
            EndTime = EndTime.AddDays(1).AddHours(09);
            if (IsDate) { sDateRange = "Date: " + StartTime.ToString("dd.MM.yy HH:mm tt") + " - " + EndTime.ToString("dd.MM.yy HH:mm tt"); }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRPCAs = GetsProductionCostAnalysis(nPSSID, IsDate, StartTime, EndTime);
            if (oRPCAs[0].ErrorMessage != "") { oRPCAs = new List<RptProductionCostAnalysis>(); }
            if (oRPCAs.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oRPCAs.Min(x => x.EndTime).ToString("dd.MM.yy HH:mm tt") + " - " + oRPCAs.Max(x => x.EndTime).ToString("dd.MM.yy HH:mm tt");
            }

            rptProductionSummaryByMachine oReport = new rptProductionSummaryByMachine();
            byte[] abytes = oReport.PrepareReport(oRPCAs, oCompany, sDateRange);
            return File(abytes, "application/pdf");
        }


        

        public List<RptProductionCostAnalysis> GetsProductionCostAnalysis(int PSSID, bool IsDate, DateTime StartTime, DateTime EndTime)
        {

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            RptProductionCostAnalysis oRPCA = new RptProductionCostAnalysis();
            try
            {
                if (PSSID <= 0 && !IsDate) { throw new Exception("Please select searching criteria."); }
                if (!IsDate)
                {
                    StartTime = DateTime.MinValue;
                    EndTime = DateTime.MinValue;
                    StartTime = new DateTime(1900, StartTime.Month, StartTime.Day);
                    EndTime = new DateTime(1900, EndTime.Month, EndTime.Day);
                }
                oRPCAs = RptProductionCostAnalysis.MailContent(PSSID, StartTime, EndTime, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if(oRPCAs.Count()<=0){ throw new Exception("No Information Found."); }
            }
            catch (Exception ex)
            {
                oRPCA.ErrorMessage = ex.Message;
                oRPCAs = new List<RptProductionCostAnalysis>();
                oRPCAs.Add(oRPCA);
            }

            return oRPCAs;
        }


        #region Mail Production Report With Shade Group


        public List<RptProductionCostAnalysis> GetsProductionCost(int PSSID, bool IsDate, DateTime StartTime, DateTime EndTime)
        {
            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            RptProductionCostAnalysis oRPCA = new RptProductionCostAnalysis();
            try
            {
                if (PSSID <= 0 && !IsDate) { throw new Exception("Please select searching criteria."); }
                if (!IsDate)
                {
                    StartTime = DateTime.MinValue;
                    EndTime = DateTime.MinValue;
                    StartTime = new DateTime(1900, StartTime.Month, StartTime.Day);
                    EndTime = new DateTime(1900, EndTime.Month, EndTime.Day);
                }
                oRPCAs = RptProductionCostAnalysis.MailContent(PSSID, StartTime, EndTime, 0);
                if (oRPCAs.Count() <= 0) { throw new Exception("No Information Found."); }
            }
            catch (Exception ex)
            {
                oRPCA.ErrorMessage = ex.Message;
                oRPCAs = new List<RptProductionCostAnalysis>();
                oRPCAs.Add(oRPCA);
            }

            return oRPCAs;
        }


        //[HttpPost]
        //public JsonResult SummaryReportForMail(double nts)
        //{
        //    string sMessage = "", sDateRange = "";

        //    DateTime sysDate = DateTime.Now, sysDateShift = DateTime.Now;
        //    DateTime tempDate = sysDate.AddDays(-1);

        //    sysDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day, 09, 00, 00);
        //    DateTime startTime = new DateTime(tempDate.Year, tempDate.Month, 1, 09, 00, 00);


        //    sysDateShift = new DateTime(sysDateShift.Year, sysDateShift.Month, sysDateShift.Day, 06, 00, 00);
        //    DateTime startTimeForShift = new DateTime(sysDateShift.Year, sysDateShift.Month, 1, 06, 00, 00);

        //    //sysDate = new DateTime(2014,8, 1, 09, 00, 00);
        //    //DateTime startTime = new DateTime(2014, 5, 1, 09, 00, 00);
        //    List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
        //    List<ReportComments> oRCs = new List<ReportComments>();
        //    try
        //    {
        //        sDateRange = "Date: " + startTime.ToString("dd.MM.yy HH:mm tt") + " - " + sysDate.ToString("dd.MM.yy HH:mm tt");
        //        oRPCAs = GetsProductionCostAnalysis(0, true, startTime, sysDate);
        //        if (oRPCAs.Count > 0)
        //        {
        //            string sSQL = "Select * from ReportComments Where CommentDate Between '" + oRPCAs.Min(x => x.DyeUnloadTime).ToString("dd MMM yyyy") + "' And '" + oRPCAs.Max(x => x.DyeUnloadTime).ToString("dd MMM yyyy") + "'";
        //            oRCs = ReportComments.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //        if (MailReportProductionSummaryWithShadeGroup(sDateRange, oRPCAs, oRCs)) { sMessage = "Mail send successfully."; }

        //        sDateRange = "Date: " + startTimeForShift.ToString("dd.MM.yy HH:mm tt") + " - " + sysDateShift.ToString("dd.MM.yy HH:mm tt");
        //        oRPCAs = GetsProductionCost(0, true, startTimeForShift, sysDateShift);
        //        if (MailReportProductionSummaryWithShadeGroupByShift(sDateRange, oRPCAs)) { sMessage = "Mail send successfully."; }
        //        else { throw new Exception("An error occured.."); }
        //    }
        //    catch (Exception ex)
        //    {
        //        sMessage = ex.Message;
        //    }

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        public object[] MailProductionSummaryWithShadeGroup()
        {
            object[] objArr = new object[1];
            string sDateRange = "";
            DateTime sysDate = DateTime.Now , sysDateShift=DateTime.Now;
            DateTime tempDate = sysDate.AddDays(-1);
            sysDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day, 09, 00, 00);
            DateTime startTime = new DateTime(tempDate.Year, tempDate.Month, 1, 09, 00, 00);

            sysDateShift = new DateTime(sysDateShift.Year, sysDateShift.Month, sysDateShift.Day, 06, 00, 00);
            DateTime startTimeForShift = new DateTime(sysDateShift.Year, sysDateShift.Month, 1, 06, 00, 00);

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
            List<ReportComments> oRCs = new List<ReportComments>();

            sDateRange = "Date: " + startTime.ToString("dd.MM.yy HH:mm tt") + " - " + sysDate.ToString("dd.MM.yy HH:mm tt");
            oRPCAs = GetsProductionCost(0, true, startTime, sysDate);
            if (oRPCAs.Count > 0)
            {
                string sSQL = "Select * from ReportComments Where CommentDate Between '" + oRPCAs.Min(x => x.EndTime).ToString("dd MMM yyyy") + "' And '" + oRPCAs.Max(x => x.EndTime).ToString("dd MMM yyyy") + "'";
               // oRCs = ReportComments.Gets(sSQL, 0);
            }
            objArr[0] = GetReportProductionSummaryWithShadeGroup(sDateRange, oRPCAs, oRCs); ;
            return objArr;
        }

        public object[] MailProductionSummaryWithShadeGroupByShift()
        {
            object[] objArr = new object[1];
            string sDateRange = "";
            DateTime sysDate = DateTime.Now, sysDateShift = DateTime.Now;
            DateTime tempDate = sysDate.AddDays(-1);
            sysDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day, 09, 00, 00);
            DateTime startTime = new DateTime(tempDate.Year, tempDate.Month, 1, 09, 00, 00);

            sysDateShift = new DateTime(sysDateShift.Year, sysDateShift.Month, sysDateShift.Day, 06, 00, 00);
            DateTime startTimeForShift = new DateTime(sysDateShift.Year, sysDateShift.Month, 1, 06, 00, 00);

            List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();

            sDateRange = "Date: " + startTimeForShift.ToString("dd.MM.yy HH:mm tt") + " - " + sysDateShift.ToString("dd.MM.yy HH:mm tt");
            oRPCAs = GetsProductionCost(0, true, startTimeForShift, sysDateShift);
            objArr[0] = GetReportProductionSummaryWithShadeGroupByShift(sDateRange, oRPCAs); ;
            return objArr;
        }

        public string GetReportProductionSummaryWithShadeGroup(string sDateRange, List<RptProductionCostAnalysis> oRptProductionCostAnalysiss, List<ReportComments> oRCs)
        {
           
            int nCount = 0;
            double nTotalProductionQty = 0;
            double nTotalProductionQtyWhite = 0;
            double nTotalProductionQtyLight = 0;
            double nTotalProductionQtyMedium = 0;
            double nTotalProductionQtyDeep = 0;
            double nTotalProductionQtyExtra = 0;

            int nTotalDay = 0;
            int nTotalHour = 0;
            int nTotalMin = 0;
            int nTotalInterval = 0;

            int nTotalWhiteDay = 0;
            int nTotalWhiteHour = 0;
            int nTotalWhiteMin = 0;
            int nTotalWhiteInterval = 0;

            int nTotalLightDay = 0;
            int nTotalLightHour = 0;
            int nTotalLightMin = 0;
            int nTotalLightInterval = 0;

            int nTotalMediumDay = 0;
            int nTotalMediumHour = 0;
            int nTotalMediumMin = 0;
            int nTotalMediumInterval = 0;

            int nTotalDeepDay = 0;
            int nTotalDeepHour = 0;
            int nTotalDeepMin = 0;
            int nTotalDeepInterval = 0;

            int nTotalExtraDay = 0;
            int nTotalExtraHour = 0;
            int nTotalExtraMin = 0;
            int nTotalExtraInterval = 0;

            double nTotalCost = 0;
            double nTotalCostDyes = 0;
            double nTotalCostChemical = 0;

            double nTotalDyedShadePercentage = 0;
            double nTotalAddShadePercentage = 0;
            double nTotalShadePercentage = 0;
            double nTotalBatchPerMachine = 0;
            double nTotalBatchLoading = 0;
            double nTotalNetRFT = 0;
            double nTotalGrossRFT = 0;

            double nTotalCostPerKg = 0;
            double nTotalDyesCostPerkg = 0;
            double nTotalChemicalCostPerKg = 0;
            double nTotalWhiteChemicalCostPerKg = 0;

            double nAvgDyedTotalCostPerKg = 0;
            double nAvgDyedChemicalCostPerKg = 0;
            double nAvgDyedDyesCostPerKg = 0; 
            double nAvgWhiteChemicalCostPerKg = 0;


            string sSubject = "ATML Dyeing Production Summary Report() @" + DateTime.Now.ToString("dd MMM yyyy HH:mm") + " [Auto Generated From ESimSol]";
            string sBodyInformation = "";

            sBodyInformation = "<div>" + sDateRange + "</div>";
            sBodyInformation = sBodyInformation + "<table cellspacing='0' border='1' style='font-size:11px; border:1px solid gray'>";

            #region Table Header
            sBodyInformation = sBodyInformation +
                               "<thead>" +
                                    "<tr>" +
                                        "<td rowspan='2' align='center' style=' min-width:30px;'>SL No</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:65px;'>Date</td>" +
                                        "<td colspan='6' align='center' style=' min-width:192px;'>Production(kg)</td>" +
                                        "<td colspan='6' align='center' style=' min-width:162px;'>Duration</td>" +

                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Dyed Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Add. Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch Load (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Net RFT (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Gross RFT (%)</td>" +

                                        "<td colspan='3' align='center' style=' min-width:108px;'>Dyed Cost/Kg(BDT)</td>" +

                                        "<td align='center' style=' min-width:38px;'>White Cost/Kg  (BDT)</td>" +

                                        "<td colspan='3' align='center' style=' min-width:150px;'>Total Cost(BDT)</td>" +

                                         "<td rowspan='2' align='center' style=' min-width:160px;'>Remark</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td align='center' style=' min-width:40px;'>Total</td>" +
                                        "<td align='center' style=' min-width:38px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Deep (2.01-3%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Ext. Dark (>3%)</td>" +

                                        "<td align='center' style=' min-width:42px;'>Average</td>" +
                                        "<td align='center' style=' min-width:40px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Deep (2.01-3%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Ext. Dark (>3%)</td>" +

                                        "<td align='center' style=' min-width:38px;'>Total</td>" +
                                        "<td align='center' style=' min-width:32px;'>Dyes</td>" +
                                        "<td align='center' style=' min-width:38px;'>Chemical</td>" +

                                        "<td align='center' style=' min-width:38px;'>Chemical</td>" +

                                        "<td align='center' style=' min-width:50px;'>Total</td>" +
                                        "<td align='center' style=' min-width:50px;'>Dyes</td>" +
                                        "<td align='center' style=' min-width:50px;'>Chemical</td>" +
                                    "</tr>" +
                                "</thead>";

            #endregion

            if (oRptProductionCostAnalysiss.Count() > 0 && oRptProductionCostAnalysiss[0].ErrorMessage == "")
            {
                #region Table Content

                oRptProductionCostAnalysiss = oRptProductionCostAnalysiss.OrderBy(x => x.EndTime).ToList();

                nTotalBatchLoading = Convert.ToDouble(oRptProductionCostAnalysiss.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRptProductionCostAnalysiss.Sum(x => x.UsesWeight));
                nTotalShadePercentage = Math.Round(Convert.ToDouble((oRptProductionCostAnalysiss.Count(x => x.DyesQty > 0) > 0) ? oRptProductionCostAnalysiss.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRptProductionCostAnalysiss.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);
                nTotalAddShadePercentage = (oRptProductionCostAnalysiss.Count(x => x.Remark != "Ok") > 0) ? oRptProductionCostAnalysiss.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRptProductionCostAnalysiss.Count(x => Math.Round(x.DyesCost) > 0)) : 0;

                nAvgDyedTotalCostPerKg = Math.Round((oRptProductionCostAnalysiss.Count(x => x.TotalShadePercentage > 0) > 0) ? oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.TotalCost)) / oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.ProductionQty)) : 0);
                nAvgDyedChemicalCostPerKg = Math.Round((oRptProductionCostAnalysiss.Count(x => x.TotalShadePercentage > 0) > 0) ? oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.ChemicalCost)) / oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.ProductionQty)) : 0);
                nAvgDyedDyesCostPerKg = Math.Round((oRptProductionCostAnalysiss.Count(x => x.TotalShadePercentage > 0) > 0) ? oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.DyesCost)) / oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.ProductionQty)) : 0);
                nAvgWhiteChemicalCostPerKg = Math.Round((oRptProductionCostAnalysiss.Count(x => x.TotalShadePercentage == 0) > 0) ? oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ChemicalCost)) / oRptProductionCostAnalysiss.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty)) : 0);

                nTotalNetRFT = oRptProductionCostAnalysiss.Count(x => x.Remark == "Ok") > 0 ? (oRptProductionCostAnalysiss.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRptProductionCostAnalysiss.Count()) : 0;
                int nAddOneCount = oRptProductionCostAnalysiss.Count(x => x.Remark == "Ok, ADD-01"); int nAddTwoCount = oRptProductionCostAnalysiss.Count(x => x.Remark == "Ok, ADD-02"); int nAddThreeCount = oRptProductionCostAnalysiss.Count(x => x.Remark == "Ok, ADD-03");
                if ((oRptProductionCostAnalysiss.Count(x => x.Remark == "Ok") + nAddOneCount + nAddTwoCount + nAddThreeCount) > 0)
                {
                    nTotalGrossRFT = nTotalNetRFT + ((nAddOneCount * 65) + (nAddTwoCount * 35) + (nAddThreeCount * 0)) / Convert.ToDouble(oRptProductionCostAnalysiss.Count());
                }


                List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
                while (oRptProductionCostAnalysiss.Count() > 0)
                {

                    oRPCAs = new List<RptProductionCostAnalysis>();
                    DateTime StartTime = oRptProductionCostAnalysiss[0].EndTime;
                    if (oRptProductionCostAnalysiss[0].EndTime.Hour < 9) { StartTime = StartTime.AddDays(-1); }
                    StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 9, 0, 0);
                    DateTime EndTime = StartTime.AddDays(1);
                    oRPCAs = oRptProductionCostAnalysiss.Where(x => x.EndTime >= StartTime).Where(x => x.EndTime < EndTime).ToList();

                    if (oRPCAs.Count()>0)
                    {
                        foreach (RptProductionCostAnalysis oItem in oRPCAs)
                        {
                            oRptProductionCostAnalysiss.RemoveAll(x => x.RouteSheetID == oItem.RouteSheetID);
                        }

                        nCount++;

                        int nDay = 0;
                        int nHour = 0;
                        int nMin = 0;
                        nDay = oRPCAs.Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                        nHour = oRPCAs.Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                        nMin = oRPCAs.Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));


                        //nTotalDay = nTotalDay + nDay; nTotalHour = nTotalHour + nHour; nTotalMin = nTotalMin + nMin;


                        if (oRPCAs[0].EndTime.Hour < 9) { oRPCAs[0].EndTime = oRPCAs[0].EndTime.AddDays(-1); }

                        sBodyInformation = sBodyInformation +
                        "<tr>" +
                            "<td align='center' style=' width:30px;'>" + nCount + "</td>" +
                            "<td align='center' style=' width:45px;'>" + oRPCAs[0].EndTime.ToString("dd.MM.yy") + "</td>";

                        #region Production

                        nTotalProductionQty = nTotalProductionQty + oRPCAs.Sum(x => Math.Round(x.ProductionQty));
                        nTotalProductionQtyWhite = nTotalProductionQtyWhite + oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty));
                        nTotalProductionQtyLight = nTotalProductionQtyLight + oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty));
                        nTotalProductionQtyMedium = nTotalProductionQtyMedium + oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty));
                        nTotalProductionQtyDeep = nTotalProductionQtyDeep + oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => Math.Round(x.ProductionQty));
                        nTotalProductionQtyExtra = nTotalProductionQtyExtra + oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => Math.Round(x.ProductionQty));

                        sBodyInformation = sBodyInformation +
                        "<td align='right' style=' width:40px;'>" + Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>";

                        #endregion

                        #region Duration

                        nTotalDay = nTotalDay + nDay; nTotalHour = nTotalHour + nHour; nTotalMin = nTotalMin + nMin;
                        nTotalInterval = nTotalInterval + oRPCAs.Count();

                        sBodyInformation = sBodyInformation +
                        "<td align='center' style=' width:40px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Count()) + "</td>";

                        if (oRPCAs.Where(x => x.TotalShadePercentage == 0).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        nTotalWhiteDay = nTotalWhiteDay + nDay; nTotalWhiteHour = nTotalWhiteHour + nHour; nTotalWhiteMin = nTotalWhiteMin + nMin;
                        nTotalWhiteInterval = nTotalWhiteInterval + oRPCAs.Where(x => x.TotalShadePercentage == 0).Count();
                        sBodyInformation = sBodyInformation +
                        "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage == 0).Count()) + "</td>";


                        if (oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        nTotalLightDay = nTotalLightDay + nDay; nTotalLightHour = nTotalLightHour + nHour; nTotalLightMin = nTotalLightMin + nMin;
                        nTotalLightInterval = nTotalLightInterval + oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count();
                        sBodyInformation = sBodyInformation +
                        "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count()) + "</td>";


                        if (oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        nTotalMediumDay = nTotalMediumDay + nDay; nTotalMediumHour = nTotalMediumHour + nHour; nTotalMediumMin = nTotalMediumMin + nMin;
                        nTotalMediumInterval = nTotalMediumInterval + oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count();
                        sBodyInformation = sBodyInformation +
                        "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count()) + "</td>";


                        if (oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage<=3).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        nTotalDeepDay = nTotalDeepDay + nDay; nTotalDeepHour = nTotalDeepHour + nHour; nTotalDeepMin = nTotalDeepMin + nMin;
                        nTotalDeepInterval = nTotalDeepInterval + oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Count();
                        sBodyInformation = sBodyInformation +
                        "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Count()) + "</td>";


                        if (oRPCAs.Where(x => x.TotalShadePercentage > 3).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        nTotalExtraDay = nTotalExtraDay + nDay; nTotalExtraHour = nTotalExtraHour + nHour; nTotalExtraMin = nTotalExtraMin + nMin;
                        nTotalDeepInterval = nTotalDeepInterval + oRPCAs.Where(x => x.TotalShadePercentage > 3).Count();
                        sBodyInformation = sBodyInformation +
                        "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > 3).Count()) + "</td>";
                       
                        #endregion

                        #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT


                        nTotalDyedShadePercentage = nTotalDyedShadePercentage + Math.Round(Convert.ToDouble((oRPCAs.Count(x => x.DyesQty > 0) > 0) ? oRPCAs.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAs.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2);


                        sBodyInformation = sBodyInformation +
                        "<td align='right'  style=' width:25px;'>" + Math.Round(Convert.ToDouble((oRPCAs.Count(x => x.DyesQty > 0) > 0) ? oRPCAs.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAs.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>";

                        double nAvgAddition = 0;
                        int nAddition = oRPCAs.Count(x => x.Remark != "Ok");
                        if (nAddition > 0) { nAvgAddition = oRPCAs.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRPCAs.Count(x => Math.Round(x.DyesCost) > 0)); }

                        //nTotalAddShadePercentage = nTotalAddShadePercentage + nAvgAddition;

                        //nTotalShadePercentage = nTotalShadePercentage + Math.Round(Convert.ToDouble((oRPCAs.Count(x => x.DyesQty > 0) > 0) ? oRPCAs.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAs.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);

                        int nMachine = oRPCAs.Select(x => x.MachineName).Distinct().Count();
                        double nResult = Convert.ToDouble(oRPCAs.Count()) / Convert.ToDouble(nMachine);
                        nTotalBatchPerMachine = nTotalBatchPerMachine + nResult;

                        // nTotalBatchLoading = nTotalBatchLoading + Convert.ToDouble(oRPCAs.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAs.Sum(x => x.UsesWeight));

                        sBodyInformation = sBodyInformation +
                        "<td align='right'  style=' width:25px;'>" + nAvgAddition.ToString("0.00") + "</td>" +
                        "<td align='right'  style=' width:25px;'>" + Math.Round(Convert.ToDouble((oRPCAs.Count(x => x.DyesQty > 0) > 0) ? oRPCAs.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAs.Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>" +
                        "<td align='right'  style=' width:25px;'>" + nResult.ToString("0.00") + "</td>" +
                        "<td align='right'  style=' width:25px;'>" + Convert.ToDouble(oRPCAs.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAs.Sum(x => x.UsesWeight)).ToString("0.00") + "</td>";

                        double nNetRFT = 0;
                        nNetRFT = oRPCAs.Count(x => x.Remark == "Ok") > 0 ? (oRPCAs.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAs.Count()) : 0;
                        //nTotalNetRFT = nTotalNetRFT + nNetRFT;
                        sBodyInformation = sBodyInformation +
                        "<td align='right'  style=' width:25px;'>" + Math.Round(nNetRFT, 2).ToString() + "</td>";

                        int nAddOneOkCount = oRPCAs.Count(x => x.Remark == "Ok, ADD-01"); int nAddTwoOkCount = oRPCAs.Count(x => x.Remark == "Ok, ADD-02"); int nAddThreeOkCount = oRPCAs.Count(x => x.Remark == "Ok, ADD-03");
                        double nGrossRFT = 0;
                        if ((oRPCAs.Count(x => x.Remark == "Ok") + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                        {
                            nGrossRFT = nNetRFT + ((nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0)) / Convert.ToDouble(oRPCAs.Count());
                        }
                        //nTotalGrossRFT = nTotalGrossRFT + nGrossRFT;
                        sBodyInformation = sBodyInformation +
                        "<td align='right'  style=' width:25px;'>" + Math.Round(nGrossRFT, 2).ToString() + "</td>";
                        #endregion

                        #region Cost Calculation Per Kg

                        double nCostPerKg = 0;
                        nCostPerKg = Math.Round((oRPCAs.Where(x => x.TotalShadePercentage > 0).Count() > 0) ? oRPCAs.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.TotalCost)) / oRPCAs.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.ProductionQty)) : 0);
                        nTotalCostPerKg = nTotalCostPerKg + nCostPerKg;

                        double nDyesCostPerKg = 0;
                        //nDyesCostPerKg = oRPCAs.Sum(x => Math.Round(x.DyesCost)) / oRPCAs.Sum(x => Math.Round(x.ProductionQty));
                        nDyesCostPerKg = Math.Round((oRPCAs.Where(x => x.TotalShadePercentage > 0).Count() > 0) ? (oRPCAs.Sum(x => Math.Round(x.DyesCost)) / oRPCAs.Where(x => x.TotalShadePercentage > 0).Sum(x => Math.Round(x.ProductionQty))) : 0);
                        nTotalDyesCostPerkg = nTotalDyesCostPerkg + nDyesCostPerKg;

                        double nChemicalCostPerKg = 0;
                        nChemicalCostPerKg = Math.Round((oRPCAs.Count(x => x.TotalShadePercentage != 0) > 0) ? oRPCAs.Where(x => x.TotalShadePercentage != 0).Sum(x => Math.Round(x.ChemicalCost)) / oRPCAs.Where(x => x.TotalShadePercentage != 0).Sum(x => Math.Round(x.ProductionQty)) : 0);
                        nTotalChemicalCostPerKg = nTotalChemicalCostPerKg + nChemicalCostPerKg;

                        sBodyInformation = sBodyInformation +
                        "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nCostPerKg).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:32px;'>" + Global.MillionFormat(nDyesCostPerKg).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nChemicalCostPerKg).Split('.')[0] + "</td>";

                        double nWhiteChemicalCostPerKg = 0;
                        nWhiteChemicalCostPerKg = Math.Round((oRPCAs.Count(x => x.TotalShadePercentage == 0) > 0) ? oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ChemicalCost)) / oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty)) : 0);
                        nTotalWhiteChemicalCostPerKg = nTotalWhiteChemicalCostPerKg + nWhiteChemicalCostPerKg;
                        sBodyInformation = sBodyInformation +
                       "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nWhiteChemicalCostPerKg).Split('.')[0] + "</td>";
                        #endregion

                        #region Total Cost Calculation

                        nTotalCost = nTotalCost + oRPCAs.Sum(x => Math.Round(x.TotalCost));
                        nTotalCostDyes = nTotalCostDyes + oRPCAs.Sum(x => Math.Round(x.DyesCost));
                        nTotalCostChemical = nTotalCostChemical + oRPCAs.Sum(x => Math.Round(x.ChemicalCost));

                        sBodyInformation = sBodyInformation +
                        "<td align='right' style=' width:50px;'>" + Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.TotalCost))).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:50px;'>" + Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.DyesCost))).Split('.')[0] + "</td>" +
                        "<td align='right' style=' width:50px;'>" + Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.ChemicalCost))).Split('.')[0] + "</td>";
                        #endregion

                        #region Remark
                        string sRemark = "";
                        if (oRCs.Count() > 0)
                        {
                            if (oRCs.Where(x => x.CommentDateInStr == oRPCAs[0].EndTime.ToString("dd MMM yyyy")).Count() > 0)
                            {
                                sRemark = oRCs.Where(x => x.CommentDateInStr == oRPCAs[0].EndTime.ToString("dd MMM yyyy")).ElementAtOrDefault(0).Note;
                            }
                        }
                        sBodyInformation = sBodyInformation +
                        "<td align='left' style=' width:160px;'>" + sRemark + "</td>";
                        #endregion

                        sBodyInformation = sBodyInformation + "</tr>";
                    }

                }

                #endregion
            }

            #region Table Footer


            #region Total

                sBodyInformation = sBodyInformation +
                   "<tr>" +
                       "<td colspan='2' align='center' style=' width:75px;'>Total</td>";
                #region Production

                sBodyInformation = sBodyInformation +
                "<td align='right' style=' width:40px;'>" + Global.MillionFormat(nTotalProductionQty).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyWhite).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyLight).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyMedium).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyDeep).Split('.')[0] + "</td>"+
                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyExtra).Split('.')[0] + "</td>";

                #endregion

                #region Duration

                sBodyInformation = sBodyInformation +
                       "<td align='center' style=' width:40px;'></td>" +
                       "<td align='center' style=' width:38px;'></td>" +
                       "<td align='center' style=' width:38px;'></td>" +
                       "<td align='center' style=' width:38px;'></td>" +
                       "<td align='center' style=' width:38px;'></td>" +
                       "<td align='center' style=' width:38px;'></td>";
                #endregion

                #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
                sBodyInformation = sBodyInformation +
                       "<td align='right' style=' width:25px;'></td>" +
                       "<td align='right' style=' width:25px;'></td>" +
                       "<td align='right' style=' width:25px;'></td>" +
                       "<td align='right' style=' width:25px;'></td>" +
                       "<td align='right' style=' width:25px;'></td>" +
                       "<td align='right' style=' width:25px;'></td>" +
                       "<td align='right' style=' width:25px;'></td>";
                #endregion

                #region Cost Calculation Per Kg
                sBodyInformation = sBodyInformation +
                       "<td align='right' style=' width:38px;'></td>" +
                       "<td align='right' style=' width:32px;'></td>" +
                       "<td align='right' style=' width:38px;'></td>" +

                       "<td align='right' style=' width:38px;'></td>";
                #endregion

                #region Total Cost Calculation
                sBodyInformation = sBodyInformation +
                       "<td align='right' style=' width:50px;'>" + Global.MillionFormat(nTotalCost).Split('.')[0] + "</td>" +
                       "<td align='right' style=' width:50px;'>" + Global.MillionFormat(nTotalCostDyes).Split('.')[0] + "</td>" +
                       "<td align='right' style=' width:50px;'>" + Global.MillionFormat(nTotalCostChemical).Split('.')[0] + "</td>";
                 #endregion

                #region Remark
                sBodyInformation = sBodyInformation +
                       "<td align='left' style=' width:160px;'></td>";
                #endregion

                sBodyInformation = sBodyInformation +
                   "</tr>";

               

            #endregion

            #region Average

                sBodyInformation = sBodyInformation +
                   "<tr>" +
                       "<td colspan='2' align='center' style=' width:75px;'>Average</td>";
                #region Production

                sBodyInformation = sBodyInformation +
                "<td align='right' style=' width:40px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQty / nCount)).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyWhite / nCount)).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyLight / nCount)).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyMedium / nCount)).Split('.')[0] + "</td>" +
                "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyDeep / nCount)).Split('.')[0] + "</td>"+
                "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyExtra / nCount)).Split('.')[0] + "</td>";

                #endregion

                #region Duration

                sBodyInformation = sBodyInformation +
                       "<td align='center' style=' width:40px;'>" + TimeStampCoversion(nTotalDay, nTotalHour, nTotalMin, nTotalInterval) + "</td>" +
                       "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalWhiteDay, nTotalWhiteHour, nTotalWhiteMin, nTotalWhiteInterval) + "</td>" +
                       "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalLightDay, nTotalLightHour, nTotalLightMin, nTotalLightInterval) + "</td>" +
                       "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalMediumDay, nTotalMediumHour, nTotalMediumMin, nTotalMediumInterval) + "</td>" +
                       "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalDeepDay, nTotalDeepHour, nTotalDeepMin, nTotalDeepInterval) + "</td>"+
                       "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalDeepDay, nTotalExtraHour, nTotalExtraMin, nTotalExtraInterval) + "</td>";
                #endregion

                #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
                sBodyInformation = sBodyInformation +
                       "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalDyedShadePercentage / nCount)) + "</td>" +
                       "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalAddShadePercentage)) + "</td>" +
                       "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalShadePercentage)) + "</td>" +
                       "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchPerMachine / nCount)) + "</td>" +
                       "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchLoading)) + "</td>" +
                       "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalNetRFT)) + "</td>" +
                       "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalGrossRFT)) + "</td>";
                #endregion

                #region Cost Calculation Per Kg
                sBodyInformation = sBodyInformation +
                       "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nAvgDyedTotalCostPerKg).Split('.')[0] + "</td>" +
                       "<td align='right' style=' width:32px;'>" + Global.MillionFormat(nAvgDyedDyesCostPerKg).Split('.')[0] + "</td>" +
                       "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nAvgDyedChemicalCostPerKg).Split('.')[0] + "</td>" +

                       "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nAvgWhiteChemicalCostPerKg).Split('.')[0] + "</td>";
                #endregion

                #region Total Cost Calculation
                sBodyInformation = sBodyInformation +
                       "<td align='right' style=' width:50px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalCost / nCount)).Split('.')[0] + "</td>" +
                       "<td align='right' style=' width:50px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalCostDyes / nCount)).Split('.')[0] + "</td>" +
                       "<td align='right' style=' width:50px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalCostChemical / nCount)).Split('.')[0] + "</td>";
                #endregion

                #region Remark
                sBodyInformation = sBodyInformation +
                       "<td align='left' style=' width:160px;'></td>";
                #endregion

                sBodyInformation = sBodyInformation +
                   "</tr>";

            #endregion

            sBodyInformation = sBodyInformation + "</table>";
            #endregion

            return sBodyInformation;
        }

        public string GetReportProductionSummaryWithShadeGroupByShift(string sDateRange, List<RptProductionCostAnalysis> oRptProductionCostAnalysiss)
        {

            int nCount = 0;
            double nTotalProductionQtyShiftA = 0, nTotalProductionQtyWhiteShiftA = 0, nTotalProductionQtyLightShiftA = 0, nTotalProductionQtyMediumShiftA = 0, nTotalProductionQtyDeepShiftA = 0;
            double nTotalProductionQtyShiftB = 0, nTotalProductionQtyWhiteShiftB = 0, nTotalProductionQtyLightShiftB = 0, nTotalProductionQtyMediumShiftB = 0, nTotalProductionQtyDeepShiftB = 0;
            double nTotalProductionQtyShiftC = 0, nTotalProductionQtyWhiteShiftC = 0, nTotalProductionQtyLightShiftC = 0, nTotalProductionQtyMediumShiftC = 0, nTotalProductionQtyDeepShiftC = 0;

            int nTotalDayShiftA = 0, nTotalHourShiftA = 0, nTotalMinShiftA = 0, nTotalIntervalShiftA = 0;
            int nTotalDayShiftB = 0, nTotalHourShiftB = 0, nTotalMinShiftB = 0, nTotalIntervalShiftB = 0;
            int nTotalDayShiftC = 0, nTotalHourShiftC = 0, nTotalMinShiftC = 0, nTotalIntervalShiftC = 0;

            int nTotalWhiteDayShiftA = 0, nTotalWhiteHourShiftA = 0, nTotalWhiteMinShiftA = 0, nTotalWhiteIntervalShiftA = 0;
            int nTotalWhiteDayShiftB = 0, nTotalWhiteHourShiftB = 0, nTotalWhiteMinShiftB = 0, nTotalWhiteIntervalShiftB = 0;
            int nTotalWhiteDayShiftC = 0, nTotalWhiteHourShiftC = 0, nTotalWhiteMinShiftC = 0, nTotalWhiteIntervalShiftC = 0;

            int nTotalLightDayShiftA = 0, nTotalLightHourShiftA = 0, nTotalLightMinShiftA = 0, nTotalLightIntervalShiftA = 0;
            int nTotalLightDayShiftB = 0, nTotalLightHourShiftB = 0, nTotalLightMinShiftB = 0, nTotalLightIntervalShiftB = 0;
            int nTotalLightDayShiftC = 0, nTotalLightHourShiftC = 0, nTotalLightMinShiftC = 0, nTotalLightIntervalShiftC = 0;

            int nTotalMediumDayShiftA = 0, nTotalMediumHourShiftA = 0, nTotalMediumMinShiftA = 0, nTotalMediumIntervalShiftA = 0;
            int nTotalMediumDayShiftB = 0, nTotalMediumHourShiftB = 0, nTotalMediumMinShiftB = 0, nTotalMediumIntervalShiftB = 0;
            int nTotalMediumDayShiftC = 0, nTotalMediumHourShiftC = 0, nTotalMediumMinShiftC = 0, nTotalMediumIntervalShiftC = 0;

            int nTotalDeepDayShiftA = 0, nTotalDeepHourShiftA = 0, nTotalDeepMinShiftA = 0, nTotalDeepIntervalShiftA = 0;
            int nTotalDeepDayShiftB = 0, nTotalDeepHourShiftB = 0, nTotalDeepMinShiftB = 0, nTotalDeepIntervalShiftB = 0;
            int nTotalDeepDayShiftC = 0, nTotalDeepHourShiftC = 0, nTotalDeepMinShiftC = 0, nTotalDeepIntervalShiftC = 0;


            double nTotalDyedShadePercentageShiftA = 0, nTotalAddShadePercentageShiftA = 0, nTotalShadePercentageShiftA = 0, nTotalBatchPerMachineShiftA = 0, nTotalBatchLoadingShiftA = 0, nTotalNetRFTShiftA = 0, nTotalGrossRFTShiftA = 0;
            double nTotalDyedShadePercentageShiftB = 0, nTotalAddShadePercentageShiftB = 0, nTotalShadePercentageShiftB = 0, nTotalBatchPerMachineShiftB = 0, nTotalBatchLoadingShiftB = 0, nTotalNetRFTShiftB = 0, nTotalGrossRFTShiftB = 0;
            double nTotalDyedShadePercentageShiftC = 0, nTotalAddShadePercentageShiftC = 0, nTotalShadePercentageShiftC = 0, nTotalBatchPerMachineShiftC = 0, nTotalBatchLoadingShiftC = 0, nTotalNetRFTShiftC = 0, nTotalGrossRFTShiftC = 0;



            string sSubject = "ATML Dyeing Production Summary Report Shift Wise @ " + DateTime.Now.ToString("dd MMM yyyy HH:mm") + " [Auto Generated From ESimSol]";
            string sBodyInformation = "";

            sBodyInformation = "<div>" + sDateRange + "</div>";
            sBodyInformation = sBodyInformation + "<table cellspacing='0' border='1' style='font-size:11px; border:1px solid gray'>";

            #region Table Header
            sBodyInformation = sBodyInformation +
                               "<thead>" +
                                    "<tr>" +
                                        "<td rowspan='3' align='center' style=' min-width:30px;'>SL No</td>" +
                                        "<td rowspan='3' align='center' style=' min-width:65px;'>Date</td>" +
                                        "<td colspan='17' align='center' style=' min-width:25px;'>Shift A (6:00 AM - 2:00 PM)</td>" +
                                        "<td colspan='17' align='center' style=' min-width:25px; background-color:#EFEEEE'>Shift B (2:00 PM - 10:00 PM)</td>" +
                                        "<td colspan='17' align='center' style=' min-width:25px;'>Shift C (10:00 PM - 6:00 AM)</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td colspan='5' align='center' style=' min-width:192px;'>Production(kg)</td>" +
                                        "<td colspan='5' align='center' style=' min-width:162px;'>Duration</td>" +

                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Dyed Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Add. Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch Load (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Net RFT (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Gross RFT (%)</td>" +



                                        "<td colspan='5' align='center' style=' min-width:192px; background-color:#EFEEEE'>Production(kg)</td>" +
                                        "<td colspan='5' align='center' style=' min-width:162px; background-color:#EFEEEE'>Duration</td>" +

                                        "<td rowspan='2' align='center' style=' min-width:25px; background-color:#EFEEEE'>Dyed Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px; background-color:#EFEEEE'>Add. Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px; background-color:#EFEEEE'>Avg Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px; background-color:#EFEEEE'>Avg Batch</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px; background-color:#EFEEEE'>Avg Batch Load (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px; background-color:#EFEEEE'>Net RFT (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px; background-color:#EFEEEE'>Gross RFT (%)</td>" +



                                        "<td colspan='5' align='center' style=' min-width:192px;'>Production(kg)</td>" +
                                        "<td colspan='5' align='center' style=' min-width:162px;'>Duration</td>" +

                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Dyed Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Add. Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch Load (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Net RFT (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Gross RFT (%)</td>" +

                                    "</tr>" +
                                    "<tr>" +
                                        "<td align='center' style=' min-width:40px;'>Total</td>" +
                                        "<td align='center' style=' min-width:38px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Deep (>2%)</td>" +

                                        "<td align='center' style=' min-width:42px;'>Average</td>" +
                                        "<td align='center' style=' min-width:40px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Deep (>2%)</td>" +


                                        "<td align='center' style=' min-width:40px; background-color:#EFEEEE'>Total</td>" +
                                        "<td align='center' style=' min-width:38px; background-color:#EFEEEE'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:38px; background-color:#EFEEEE'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:38px; background-color:#EFEEEE'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:38px; background-color:#EFEEEE'>Deep (>2%)</td>" +

                                        "<td align='center' style=' min-width:42px; background-color:#EFEEEE'>Average</td>" +
                                        "<td align='center' style=' min-width:40px; background-color:#EFEEEE'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:40px; background-color:#EFEEEE'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:40px; background-color:#EFEEEE'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:40px; background-color:#EFEEEE'>Deep (>2%)</td>" +



                                        "<td align='center' style=' min-width:40px;'>Total</td>" +
                                        "<td align='center' style=' min-width:38px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Deep (>2%)</td>" +

                                        "<td align='center' style=' min-width:42px;'>Average</td>" +
                                        "<td align='center' style=' min-width:40px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Deep (>2%)</td>" +
                                    "</tr>" +

                                "</thead>";

            #endregion

            if (oRptProductionCostAnalysiss.Count() > 0 && oRptProductionCostAnalysiss[0].ErrorMessage == "")
            {
                #region Table Content

                oRptProductionCostAnalysiss = oRptProductionCostAnalysiss.OrderBy(x => x.EndTime).ToList();

                List<RptProductionCostAnalysis> oRPCAShiftA = new List<RptProductionCostAnalysis>();
                List<RptProductionCostAnalysis> oRPCAShiftB = new List<RptProductionCostAnalysis>();
                List<RptProductionCostAnalysis> oRPCAShiftC = new List<RptProductionCostAnalysis>();

                int nAddOneCount = 0, nAddTwoCount = 0, nAddThreeCount=0;


                oRPCAShiftA = oRptProductionCostAnalysiss.Where(x => x.EndTime.Hour >= 6 && x.EndTime.Hour <= 13).ToList();
                oRPCAShiftB = oRptProductionCostAnalysiss.Where(x => x.EndTime.Hour >= 14 && x.EndTime.Hour <= 21).ToList();
                oRPCAShiftC = oRptProductionCostAnalysiss.Where(x => x.EndTime.Hour >= 22 || x.EndTime.Hour <= 5).ToList();

                #region Shift A
                if (oRPCAShiftA.Count() > 0)
                {
                    nTotalBatchLoadingShiftA = Convert.ToDouble(oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftA.Sum(x => x.UsesWeight));
                    nTotalShadePercentageShiftA = Math.Round(Convert.ToDouble((oRPCAShiftA.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftA.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);
                    nTotalAddShadePercentageShiftA = (oRPCAShiftA.Count(x => x.Remark != "Ok") > 0) ? oRPCAShiftA.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRPCAShiftA.Count(x => Math.Round(x.DyesCost) > 0)) : 0;
                    nTotalNetRFTShiftA = oRPCAShiftA.Count(x => x.Remark == "Ok") > 0 ? (oRPCAShiftA.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAShiftA.Count()) : 0;
                    nAddOneCount = oRPCAShiftA.Count(x => x.Remark == "Ok, ADD-01"); nAddTwoCount = oRPCAShiftA.Count(x => x.Remark == "Ok, ADD-02"); nAddThreeCount = oRPCAShiftA.Count(x => x.Remark == "Ok, ADD-03");
                    if ((oRPCAShiftA.Count(x => x.Remark == "Ok") + nAddOneCount + nAddTwoCount + nAddThreeCount) > 0)
                    {
                        nTotalGrossRFTShiftA = nTotalNetRFTShiftA + ((nAddOneCount * 65) + (nAddTwoCount * 35) + (nAddThreeCount * 0)) / Convert.ToDouble(oRPCAShiftA.Count());
                    }
                }
                
                #endregion
          
                #region Shift B
                nTotalBatchLoadingShiftB = Convert.ToDouble(oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftB.Sum(x => x.UsesWeight));
                nTotalShadePercentageShiftB = Math.Round(Convert.ToDouble((oRPCAShiftB.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftB.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);
                nTotalAddShadePercentageShiftB = (oRPCAShiftB.Count(x => x.Remark != "Ok") > 0) ? oRPCAShiftB.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRPCAShiftB.Count(x => Math.Round(x.DyesCost) > 0)) : 0;
                nTotalNetRFTShiftB = oRPCAShiftB.Count(x => x.Remark == "Ok") > 0 ? (oRPCAShiftB.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAShiftB.Count()) : 0;
                nAddOneCount = oRPCAShiftB.Count(x => x.Remark == "Ok, ADD-01"); nAddTwoCount = oRPCAShiftB.Count(x => x.Remark == "Ok, ADD-02"); nAddThreeCount = oRPCAShiftB.Count(x => x.Remark == "Ok, ADD-03");
                if ((oRPCAShiftB.Count(x => x.Remark == "Ok") + nAddOneCount + nAddTwoCount + nAddThreeCount) > 0)
                {
                    nTotalGrossRFTShiftB = nTotalNetRFTShiftB + ((nAddOneCount * 65) + (nAddTwoCount * 35) + (nAddThreeCount * 0)) / Convert.ToDouble(oRPCAShiftB.Count());
                }
                #endregion

                #region Shift C
                nTotalBatchLoadingShiftC = Convert.ToDouble(oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftC.Sum(x => x.UsesWeight));
                nTotalShadePercentageShiftC = Math.Round(Convert.ToDouble((oRPCAShiftC.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftC.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);
                nTotalAddShadePercentageShiftC = (oRPCAShiftC.Count(x => x.Remark != "Ok") > 0) ? oRPCAShiftC.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRPCAShiftC.Count(x => Math.Round(x.DyesCost) > 0)) : 0;
                nTotalNetRFTShiftC = oRPCAShiftC.Count(x => x.Remark == "Ok") > 0 ? (oRPCAShiftC.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAShiftC.Count()) : 0;
                nAddOneCount = oRPCAShiftC.Count(x => x.Remark == "Ok, ADD-01"); nAddTwoCount = oRPCAShiftC.Count(x => x.Remark == "Ok, ADD-02"); nAddThreeCount = oRPCAShiftC.Count(x => x.Remark == "Ok, ADD-03");
                if ((oRptProductionCostAnalysiss.Count(x => x.Remark == "Ok") + nAddOneCount + nAddTwoCount + nAddThreeCount) > 0)
                {
                    nTotalGrossRFTShiftC = nTotalNetRFTShiftC + ((nAddOneCount * 65) + (nAddTwoCount * 35) + (nAddThreeCount * 0)) / Convert.ToDouble(oRptProductionCostAnalysiss.Count());
                }
                #endregion


                List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
                while (oRptProductionCostAnalysiss.Count() > 0)
                {

                    oRPCAs = new List<RptProductionCostAnalysis>();
                    DateTime StartTime = oRptProductionCostAnalysiss[0].EndTime;
                    if (oRptProductionCostAnalysiss[0].EndTime.Hour < 6) { StartTime = StartTime.AddDays(-1); }
                    StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 6, 0, 0);
                    DateTime EndTime = StartTime.AddDays(1);
                    oRPCAs = oRptProductionCostAnalysiss.Where(x => x.EndTime >= StartTime).Where(x => x.EndTime < EndTime).ToList();

                    if (oRPCAs.Count() > 0)
                    {
                        foreach (RptProductionCostAnalysis oItem in oRPCAs)
                        {
                            int xa = oItem.EndTime.Hour;
                            oRptProductionCostAnalysiss.RemoveAll(x => x.RouteSheetID == oItem.RouteSheetID);
                        }

                        nCount++;


                        oRPCAShiftA = oRPCAs.Where(x => x.EndTime.Hour >= 6 && x.EndTime.Hour <= 13).ToList();
                        oRPCAShiftB = oRPCAs.Where(x => x.EndTime.Hour >= 14 && x.EndTime.Hour <= 21).ToList();
                        oRPCAShiftC = oRPCAs.Where(x => x.EndTime.Hour >= 22 || x.EndTime.Hour <= 5).ToList();

                        //if (oRPCAs[0].DyeUnloadTime.Hour < 6) { oRPCAs[0].DyeUnloadTime = oRPCAs[0].DyeUnloadTime.AddDays(-1); }

                        sBodyInformation = sBodyInformation +
                        "<tr>" +
                            "<td align='center' style=' width:30px;'>" + nCount + "</td>" +
                            "<td align='center' style=' width:45px;'>" + oRPCAs[0].EndTime.ToString("dd.MM.yy") + "</td>";

                            #region Shift A
                        
                            if(oRPCAShiftA.Count()>0){
                                #region Production

                                nTotalProductionQtyShiftA = nTotalProductionQtyShiftA + oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyWhiteShiftA = nTotalProductionQtyWhiteShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyLightShiftA = nTotalProductionQtyLightShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyMediumShiftA = nTotalProductionQtyMediumShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyDeepShiftA = nTotalProductionQtyDeepShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Sum(x => Math.Round(x.ProductionQty));

                                sBodyInformation = sBodyInformation +
                                "<td align='right' style=' width:40px;'>" + Global.MillionFormat(oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>";

                                #endregion

                                #region Duration

                                int nDay = 0, nHour = 0, nMin = 0;

                                nDay = oRPCAShiftA.Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                nHour = oRPCAShiftA.Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                nMin = oRPCAShiftA.Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));

                                nTotalDayShiftA = nTotalDayShiftA + nDay; nTotalHourShiftA = nTotalHourShiftA + nHour; nTotalMinShiftA = nTotalMinShiftA + nMin;
                                nTotalIntervalShiftA = nTotalIntervalShiftA + oRPCAShiftA.Count();

                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:40px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftA.Count()) + "</td>";

                                if (oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Count() > 0)
                                {
                                    nDay = oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalWhiteDayShiftA = nTotalWhiteDayShiftA + nDay; nTotalWhiteHourShiftA = nTotalWhiteHourShiftA + nHour; nTotalWhiteMinShiftA = nTotalWhiteMinShiftA + nMin;
                                nTotalWhiteIntervalShiftA = nTotalWhiteIntervalShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftA.Where(x => x.TotalShadePercentage == 0).Count()) + "</td>";


                                if (oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count() > 0)
                                {
                                    nDay = oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalLightDayShiftA = nTotalLightDayShiftA + nDay; nTotalLightHourShiftA = nTotalLightHourShiftA + nHour; nTotalLightMinShiftA = nTotalLightMinShiftA + nMin;
                                nTotalLightIntervalShiftA = nTotalLightIntervalShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftA.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count()) + "</td>";


                                if (oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count() > 0)
                                {
                                    nDay = oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalMediumDayShiftA = nTotalMediumDayShiftA + nDay; nTotalMediumHourShiftA = nTotalMediumHourShiftA + nHour; nTotalMediumMinShiftA = nTotalMediumMinShiftA + nMin;
                                nTotalMediumIntervalShiftA = nTotalMediumIntervalShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftA.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count()) + "</td>";


                                if (oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Count() > 0)
                                {
                                    nDay = oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalDeepDayShiftA = nTotalDeepDayShiftA + nDay; nTotalDeepHourShiftA = nTotalDeepHourShiftA + nHour; nTotalDeepMinShiftA = nTotalDeepMinShiftA + nMin;
                                nTotalDeepIntervalShiftA = nTotalDeepIntervalShiftA + oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftA.Where(x => x.TotalShadePercentage > 2).Count()) + "</td>";
                                #endregion

                                #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT


                                nTotalDyedShadePercentageShiftA = nTotalDyedShadePercentageShiftA + Math.Round(Convert.ToDouble((oRPCAShiftA.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftA.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftA.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2);


                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(Convert.ToDouble((oRPCAShiftA.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftA.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftA.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>";

                                double nAvgAddition = 0;
                                int nAddition = oRPCAShiftA.Count(x => x.Remark != "Ok");
                                if (nAddition > 0) { nAvgAddition = oRPCAShiftA.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRPCAShiftA.Count(x => Math.Round(x.DyesCost) > 0)); }

                                //nTotalAddShadePercentage = nTotalAddShadePercentage + nAvgAddition;

                                //nTotalShadePercentage = nTotalShadePercentage + Math.Round(Convert.ToDouble((oRPCAShiftA.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftA.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);

                                int nMachine = oRPCAShiftA.Select(x => x.MachineName).Distinct().Count();
                                double nResult = Convert.ToDouble(oRPCAShiftA.Count()) / Convert.ToDouble(nMachine);
                                nTotalBatchPerMachineShiftA = nTotalBatchPerMachineShiftA + nResult;

                                // nTotalBatchLoading = nTotalBatchLoading + Convert.ToDouble(oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftA.Sum(x => x.UsesWeight));

                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + nAvgAddition.ToString("0.00") + "</td>" +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(Convert.ToDouble((oRPCAShiftA.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftA.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>" +
                                "<td align='right'  style=' width:25px;'>" + nResult.ToString("0.00") + "</td>" +
                                "<td align='right'  style=' width:25px;'>" + Convert.ToDouble(oRPCAShiftA.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftA.Sum(x => x.UsesWeight)).ToString("0.00") + "</td>";

                                double nNetRFT = 0;
                                nNetRFT = oRPCAShiftA.Count(x => x.Remark == "Ok") > 0 ? (oRPCAShiftA.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAShiftA.Count()) : 0;
                                //nTotalNetRFT = nTotalNetRFT + nNetRFT;
                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(nNetRFT, 2).ToString() + "</td>";

                                int nAddOneOkCount = oRPCAShiftA.Count(x => x.Remark == "Ok, ADD-01"); int nAddTwoOkCount = oRPCAShiftA.Count(x => x.Remark == "Ok, ADD-02"); int nAddThreeOkCount = oRPCAShiftA.Count(x => x.Remark == "Ok, ADD-03");
                                double nGrossRFT = 0;
                                if ((oRPCAShiftA.Count(x => x.Remark == "Ok") + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                                {
                                    nGrossRFT = nNetRFT + ((nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0)) / Convert.ToDouble(oRPCAShiftA.Count());
                                }
                                //nTotalGrossRFT = nTotalGrossRFT + nGrossRFT;
                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(nGrossRFT, 2).ToString() + "</td>";
                                #endregion
                            }
                            else
                            {
                                sBodyInformation = sBodyInformation + BlankGridGeneration(false);
                            }

                            #endregion

                            #region Shift B
                            if (oRPCAShiftB.Count() > 0)
                            {
                                #region Production

                                nTotalProductionQtyShiftB = nTotalProductionQtyShiftB + oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyWhiteShiftB = nTotalProductionQtyWhiteShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyLightShiftB = nTotalProductionQtyLightShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyMediumShiftB = nTotalProductionQtyMediumShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyDeepShiftB = nTotalProductionQtyDeepShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Sum(x => Math.Round(x.ProductionQty));

                                sBodyInformation = sBodyInformation +
                                "<td align='right' style=' width:40px; background-color:#EFEEEE'>" + Global.MillionFormat(oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>";

                                #endregion

                                #region Duration

                                int nDay = 0, nHour = 0, nMin = 0;

                                nDay = oRPCAShiftB.Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                nHour = oRPCAShiftB.Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                nMin = oRPCAShiftB.Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));

                                nTotalDayShiftB = nTotalDayShiftB + nDay; nTotalHourShiftB = nTotalHourShiftB + nHour; nTotalMinShiftB = nTotalMinShiftB + nMin;
                                nTotalIntervalShiftB = nTotalIntervalShiftB + oRPCAShiftB.Count();

                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:40px; background-color:#EFEEEE'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftB.Count()) + "</td>";

                                if (oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Count() > 0)
                                {
                                    nDay = oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalWhiteDayShiftB = nTotalWhiteDayShiftB + nDay; nTotalWhiteHourShiftB = nTotalWhiteHourShiftB + nHour; nTotalWhiteMinShiftB = nTotalWhiteMinShiftB + nMin;
                                nTotalWhiteIntervalShiftB = nTotalWhiteIntervalShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftB.Where(x => x.TotalShadePercentage == 0).Count()) + "</td>";


                                if (oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count() > 0)
                                {
                                    nDay = oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalLightDayShiftB = nTotalLightDayShiftB + nDay; nTotalLightHourShiftB = nTotalLightHourShiftB + nHour; nTotalLightMinShiftB = nTotalLightMinShiftB + nMin;
                                nTotalLightIntervalShiftB = nTotalLightIntervalShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftB.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count()) + "</td>";


                                if (oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count() > 0)
                                {
                                    nDay = oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalMediumDayShiftB = nTotalMediumDayShiftB + nDay; nTotalMediumHourShiftB = nTotalMediumHourShiftB + nHour; nTotalMediumMinShiftB = nTotalMediumMinShiftB + nMin;
                                nTotalMediumIntervalShiftB = nTotalMediumIntervalShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftB.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count()) + "</td>";


                                if (oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Count() > 0)
                                {
                                    nDay = oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalDeepDayShiftB = nTotalDeepDayShiftB + nDay; nTotalDeepHourShiftB = nTotalDeepHourShiftB + nHour; nTotalDeepMinShiftB = nTotalDeepMinShiftB + nMin;
                                nTotalDeepIntervalShiftB = nTotalDeepIntervalShiftB + oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftB.Where(x => x.TotalShadePercentage > 2).Count()) + "</td>";
                                #endregion

                                #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT


                                nTotalDyedShadePercentageShiftB = nTotalDyedShadePercentageShiftB + Math.Round(Convert.ToDouble((oRPCAShiftB.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftB.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftB.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2);


                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px; background-color:#EFEEEE'>" + Math.Round(Convert.ToDouble((oRPCAShiftB.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftB.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftB.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>";

                                double nAvgAddition = 0;
                                int nAddition = oRPCAShiftB.Count(x => x.Remark != "Ok");
                                if (nAddition > 0) { nAvgAddition = oRPCAShiftB.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRPCAShiftB.Count(x => Math.Round(x.DyesCost) > 0)); }

                                //nTotalAddShadePercentage = nTotalAddShadePercentage + nAvgAddition;

                                //nTotalShadePercentage = nTotalShadePercentage + Math.Round(Convert.ToDouble((oRPCAShiftB.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftB.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);

                                int nMachine = oRPCAShiftB.Select(x => x.MachineName).Distinct().Count();
                                double nResult = Convert.ToDouble(oRPCAShiftB.Count()) / Convert.ToDouble(nMachine);
                                nTotalBatchPerMachineShiftB = nTotalBatchPerMachineShiftB + nResult;

                                // nTotalBatchLoading = nTotalBatchLoading + Convert.ToDouble(oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftB.Sum(x => x.UsesWeight));

                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px; background-color:#EFEEEE'>" + nAvgAddition.ToString("0.00") + "</td>" +
                                "<td align='right'  style=' width:25px; background-color:#EFEEEE'>" + Math.Round(Convert.ToDouble((oRPCAShiftB.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftB.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>" +
                                "<td align='right'  style=' width:25px; background-color:#EFEEEE'>" + nResult.ToString("0.00") + "</td>" +
                                "<td align='right'  style=' width:25px; background-color:#EFEEEE'>" + Convert.ToDouble(oRPCAShiftB.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftB.Sum(x => x.UsesWeight)).ToString("0.00") + "</td>";

                                double nNetRFT = 0;
                                nNetRFT = oRPCAShiftB.Count(x => x.Remark == "Ok") > 0 ? (oRPCAShiftB.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAShiftB.Count()) : 0;
                                //nTotalNetRFT = nTotalNetRFT + nNetRFT;
                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px; background-color:#EFEEEE'>" + Math.Round(nNetRFT, 2).ToString() + "</td>";

                                int nAddOneOkCount = oRPCAShiftB.Count(x => x.Remark == "Ok, ADD-01"); int nAddTwoOkCount = oRPCAShiftB.Count(x => x.Remark == "Ok, ADD-02"); int nAddThreeOkCount = oRPCAShiftB.Count(x => x.Remark == "Ok, ADD-03");
                                double nGrossRFT = 0;
                                if ((oRPCAShiftB.Count(x => x.Remark == "Ok") + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                                {
                                    nGrossRFT = nNetRFT + ((nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0)) / Convert.ToDouble(oRPCAShiftB.Count());
                                }
                                //nTotalGrossRFT = nTotalGrossRFT + nGrossRFT;
                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px; background-color:#EFEEEE'>" + Math.Round(nGrossRFT, 2).ToString() + "</td>";
                                #endregion
                            }
                            else
                            {
                                sBodyInformation = sBodyInformation + BlankGridGeneration(true);
                            }
                            #endregion

                            #region Shift C
                            if (oRPCAShiftC.Count() > 0)
                            {
                                #region Production

                                nTotalProductionQtyShiftC = nTotalProductionQtyShiftC + oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyWhiteShiftC = nTotalProductionQtyWhiteShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyLightShiftC = nTotalProductionQtyLightShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyMediumShiftC = nTotalProductionQtyMediumShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty));
                                nTotalProductionQtyDeepShiftC = nTotalProductionQtyDeepShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Sum(x => Math.Round(x.ProductionQty));

                                sBodyInformation = sBodyInformation +
                                "<td align='right' style=' width:40px;'>" + Global.MillionFormat(oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>" +
                                "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0] + "</td>";

                                #endregion

                                #region Duration

                                int nDay = 0, nHour = 0, nMin = 0;

                                nDay = oRPCAShiftC.Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                nHour = oRPCAShiftC.Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                nMin = oRPCAShiftC.Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));

                                nTotalDayShiftC = nTotalDayShiftC + nDay; nTotalHourShiftC = nTotalHourShiftC + nHour; nTotalMinShiftC = nTotalMinShiftC + nMin;
                                nTotalIntervalShiftC = nTotalIntervalShiftC + oRPCAShiftC.Count();

                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:40px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftC.Count()) + "</td>";

                                if (oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Count() > 0)
                                {
                                    nDay = oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalWhiteDayShiftC = nTotalWhiteDayShiftC + nDay; nTotalWhiteHourShiftC = nTotalWhiteHourShiftC + nHour; nTotalWhiteMinShiftC = nTotalWhiteMinShiftC + nMin;
                                nTotalWhiteIntervalShiftC = nTotalWhiteIntervalShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftC.Where(x => x.TotalShadePercentage == 0).Count()) + "</td>";


                                if (oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count() > 0)
                                {
                                    nDay = oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalLightDayShiftC = nTotalLightDayShiftC + nDay; nTotalLightHourShiftC = nTotalLightHourShiftC + nHour; nTotalLightMinShiftC = nTotalLightMinShiftC + nMin;
                                nTotalLightIntervalShiftC = nTotalLightIntervalShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftC.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count()) + "</td>";


                                if (oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count() > 0)
                                {
                                    nDay = oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalMediumDayShiftC = nTotalMediumDayShiftC + nDay; nTotalMediumHourShiftC = nTotalMediumHourShiftC + nHour; nTotalMediumMinShiftC = nTotalMediumMinShiftC + nMin;
                                nTotalMediumIntervalShiftC = nTotalMediumIntervalShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftC.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count()) + "</td>";


                                if (oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Count() > 0)
                                {
                                    nDay = oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                                    nHour = oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                                    nMin = oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                                }
                                else { nDay = 0; nHour = 0; nMin = 0; }

                                nTotalDeepDayShiftC = nTotalDeepDayShiftC + nDay; nTotalDeepHourShiftC = nTotalDeepHourShiftC + nHour; nTotalDeepMinShiftC = nTotalDeepMinShiftC + nMin;
                                nTotalDeepIntervalShiftC = nTotalDeepIntervalShiftC + oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Count();
                                sBodyInformation = sBodyInformation +
                                "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oRPCAShiftC.Where(x => x.TotalShadePercentage > 2).Count()) + "</td>";
                                #endregion

                                #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT


                                nTotalDyedShadePercentageShiftC = nTotalDyedShadePercentageShiftC + Math.Round(Convert.ToDouble((oRPCAShiftC.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftC.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftC.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2);


                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(Convert.ToDouble((oRPCAShiftC.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftC.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftC.Where(x => x.DyesQty > 0).Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>";

                                double nAvgAddition = 0;
                                int nAddition = oRPCAShiftC.Count(x => x.Remark != "Ok");
                                if (nAddition > 0) { nAvgAddition = oRPCAShiftC.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(oRPCAShiftC.Count(x => Math.Round(x.DyesCost) > 0)); }

                                //nTotalAddShadePercentage = nTotalAddShadePercentage + nAvgAddition;

                                //nTotalShadePercentage = nTotalShadePercentage + Math.Round(Convert.ToDouble((oRPCAShiftC.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftC.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty)) : 0), 2);

                                int nMachine = oRPCAShiftC.Select(x => x.MachineName).Distinct().Count();
                                double nResult = Convert.ToDouble(oRPCAShiftC.Count()) / Convert.ToDouble(nMachine);
                                nTotalBatchPerMachineShiftC = nTotalBatchPerMachineShiftC + nResult;

                                // nTotalBatchLoading = nTotalBatchLoading + Convert.ToDouble(oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftC.Sum(x => x.UsesWeight));

                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + nAvgAddition.ToString("0.00") + "</td>" +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(Convert.ToDouble((oRPCAShiftC.Count(x => x.DyesQty > 0) > 0) ? oRPCAShiftC.Where(x => x.DyesQty > 0).Sum(x => x.TotalDyesQty) * 100 / oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty)) : 0), 2).ToString() + "</td>" +
                                "<td align='right'  style=' width:25px;'>" + nResult.ToString("0.00") + "</td>" +
                                "<td align='right'  style=' width:25px;'>" + Convert.ToDouble(oRPCAShiftC.Sum(x => Math.Round(x.ProductionQty)) * 100 / oRPCAShiftC.Sum(x => x.UsesWeight)).ToString("0.00") + "</td>";

                                double nNetRFT = 0;
                                nNetRFT = oRPCAShiftC.Count(x => x.Remark == "Ok") > 0 ? (oRPCAShiftC.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAShiftC.Count()) : 0;
                                //nTotalNetRFT = nTotalNetRFT + nNetRFT;
                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(nNetRFT, 2).ToString() + "</td>";

                                int nAddOneOkCount = oRPCAShiftC.Count(x => x.Remark == "Ok, ADD-01"); int nAddTwoOkCount = oRPCAShiftC.Count(x => x.Remark == "Ok, ADD-02"); int nAddThreeOkCount = oRPCAShiftC.Count(x => x.Remark == "Ok, ADD-03");
                                double nGrossRFT = 0;
                                if ((oRPCAShiftC.Count(x => x.Remark == "Ok") + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                                {
                                    nGrossRFT = nNetRFT + ((nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0)) / Convert.ToDouble(oRPCAShiftC.Count());
                                }
                                //nTotalGrossRFT = nTotalGrossRFT + nGrossRFT;
                                sBodyInformation = sBodyInformation +
                                "<td align='right'  style=' width:25px;'>" + Math.Round(nGrossRFT, 2).ToString() + "</td>";
                                #endregion
                            }
                            else
                            {
                               sBodyInformation = sBodyInformation + BlankGridGeneration(false);
                            }
                            #endregion



                            sBodyInformation = sBodyInformation + "</tr>";
                    }

                }

                #endregion
            }

            #region Table Footer


            #region Total

            sBodyInformation = sBodyInformation +
               "<tr>" +

                   "<td colspan='2' align='center' style=' width:75px;'>Total</td>";

            #region Shift A
            #region Production

            sBodyInformation = sBodyInformation +
            "<td align='right' style=' width:40px;'>" + Global.MillionFormat(nTotalProductionQtyShiftA).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyWhiteShiftA).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyLightShiftA).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyMediumShiftA).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyDeepShiftA).Split('.')[0] + "</td>";

            #endregion

            #region Duration

            sBodyInformation = sBodyInformation +
                   "<td align='center' style=' width:40px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>";
            #endregion

            #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
            sBodyInformation = sBodyInformation +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>";
            #endregion

            #endregion

            #region Shift B
            #region Production

            sBodyInformation = sBodyInformation +
            "<td align='right' style=' width:40px; background-color:#EFEEEE'>" + Global.MillionFormat(nTotalProductionQtyShiftB).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(nTotalProductionQtyWhiteShiftB).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(nTotalProductionQtyLightShiftB).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(nTotalProductionQtyMediumShiftB).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + Global.MillionFormat(nTotalProductionQtyDeepShiftB).Split('.')[0] + "</td>";

            #endregion

            #region Duration

            sBodyInformation = sBodyInformation +
                   "<td align='center' style=' width:40px; background-color:#EFEEEE'></td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'></td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'></td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'></td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'></td>";
            #endregion

            #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
            sBodyInformation = sBodyInformation +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'></td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'></td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'></td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'></td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'></td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'></td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'></td>";
            #endregion

            #endregion

            #region Shift C
            #region Production

            sBodyInformation = sBodyInformation +
            "<td align='right' style=' width:40px;'>" + Global.MillionFormat(nTotalProductionQtyShiftC).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyWhiteShiftC).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyLightShiftC).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyMediumShiftC).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + Global.MillionFormat(nTotalProductionQtyDeepShiftC).Split('.')[0] + "</td>";

            #endregion

            #region Duration

            sBodyInformation = sBodyInformation +
                   "<td align='center' style=' width:40px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>" +
                   "<td align='center' style=' width:38px;'></td>";
            #endregion

            #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
            sBodyInformation = sBodyInformation +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>" +
                   "<td align='right' style=' width:25px;'></td>";
            #endregion

            #endregion

            sBodyInformation = sBodyInformation +
               "</tr>";



            #endregion

            #region Average

            sBodyInformation = sBodyInformation +
               "<tr>" +
                   "<td colspan='2' align='center' style=' width:75px;'>Average</td>";

            #region Shift A
            #region Production

            sBodyInformation = sBodyInformation +
            "<td align='right' style=' width:40px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyShiftA / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyWhiteShiftA / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyLightShiftA / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyMediumShiftA / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyDeepShiftA / nCount)).Split('.')[0] + "</td>";

            #endregion

            #region Duration

            sBodyInformation = sBodyInformation +
                   "<td align='center' style=' width:40px;'>" + TimeStampCoversion(nTotalDayShiftA, nTotalHourShiftA, nTotalMinShiftA, nTotalIntervalShiftA) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalWhiteDayShiftA, nTotalWhiteHourShiftA, nTotalWhiteMinShiftA, nTotalWhiteIntervalShiftA) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalLightDayShiftA, nTotalLightHourShiftA, nTotalLightMinShiftA, nTotalLightIntervalShiftA) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalMediumDayShiftA, nTotalMediumHourShiftA, nTotalMediumMinShiftA, nTotalMediumIntervalShiftA) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalDeepDayShiftA, nTotalDeepHourShiftA, nTotalDeepMinShiftA, nTotalDeepIntervalShiftA) + "</td>";
            #endregion

            #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
            sBodyInformation = sBodyInformation +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalDyedShadePercentageShiftA / nCount)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalAddShadePercentageShiftA)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalShadePercentageShiftA)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchPerMachineShiftA / nCount)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchLoadingShiftA)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalNetRFTShiftA)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalGrossRFTShiftA)) + "</td>";
            #endregion

            #endregion

            #region Shift B
            #region Production

            sBodyInformation = sBodyInformation +
            "<td align='right' style=' width:40px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyShiftB / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyWhiteShiftB / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyLightShiftB / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyMediumShiftB / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyDeepShiftB / nCount)).Split('.')[0] + "</td>";

            #endregion

            #region Duration

            sBodyInformation = sBodyInformation +
                   "<td align='center' style=' width:40px; background-color:#EFEEEE'>" + TimeStampCoversion(nTotalDayShiftB, nTotalHourShiftB, nTotalMinShiftB, nTotalIntervalShiftB) + "</td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nTotalWhiteDayShiftB, nTotalWhiteHourShiftB, nTotalWhiteMinShiftB, nTotalWhiteIntervalShiftB) + "</td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nTotalLightDayShiftB, nTotalLightHourShiftB, nTotalLightMinShiftB, nTotalLightIntervalShiftB) + "</td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nTotalMediumDayShiftB, nTotalMediumHourShiftB, nTotalMediumMinShiftB, nTotalMediumIntervalShiftB) + "</td>" +
                   "<td align='center' style=' width:38px; background-color:#EFEEEE'>" + TimeStampCoversion(nTotalDeepDayShiftB, nTotalDeepHourShiftB, nTotalDeepMinShiftB, nTotalDeepIntervalShiftB) + "</td>";
            #endregion

            #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
            sBodyInformation = sBodyInformation +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalDyedShadePercentageShiftB / nCount)) + "</td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalAddShadePercentageShiftB)) + "</td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalShadePercentageShiftB)) + "</td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchPerMachineShiftB / nCount)) + "</td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchLoadingShiftB)) + "</td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalNetRFTShiftB)) + "</td>" +
                   "<td align='right' style=' width:25px; background-color:#EFEEEE'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalGrossRFTShiftB)) + "</td>";
            #endregion

            #endregion

            #region Shift C
            #region Production

            sBodyInformation = sBodyInformation +
            "<td align='right' style=' width:40px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyShiftC / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyWhiteShiftC / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyLightShiftC / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyMediumShiftC / nCount)).Split('.')[0] + "</td>" +
            "<td align='right' style=' width:38px;'>" + ((nCount == 0) ? "0" : Global.MillionFormat(nTotalProductionQtyDeepShiftC / nCount)).Split('.')[0] + "</td>";

            #endregion

            #region Duration

            sBodyInformation = sBodyInformation +
                   "<td align='center' style=' width:40px;'>" + TimeStampCoversion(nTotalDayShiftC, nTotalHourShiftC, nTotalMinShiftC, nTotalIntervalShiftC) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalWhiteDayShiftC, nTotalWhiteHourShiftC, nTotalWhiteMinShiftC, nTotalWhiteIntervalShiftC) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalLightDayShiftC, nTotalLightHourShiftC, nTotalLightMinShiftC, nTotalLightIntervalShiftC) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalMediumDayShiftC, nTotalMediumHourShiftC, nTotalMediumMinShiftC, nTotalMediumIntervalShiftC) + "</td>" +
                   "<td align='center' style=' width:38px;'>" + TimeStampCoversion(nTotalDeepDayShiftC, nTotalDeepHourShiftC, nTotalDeepMinShiftC, nTotalDeepIntervalShiftC) + "</td>";
            #endregion

            #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
            sBodyInformation = sBodyInformation +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalDyedShadePercentageShiftC / nCount)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalAddShadePercentageShiftC)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalShadePercentageShiftC)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchPerMachineShiftC / nCount)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalBatchLoadingShiftC)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalNetRFTShiftC)) + "</td>" +
                   "<td align='right' style=' width:25px;'>" + ((nCount == 0) ? "0.00" : Global.MillionFormat(nTotalGrossRFTShiftC)) + "</td>";
            #endregion
            #endregion

           
            sBodyInformation = sBodyInformation +
               "</tr>";

            #endregion

            sBodyInformation = sBodyInformation + "</table>";

            #region Production Summary

             sBodyInformation = sBodyInformation + "<div style='padding-top:10px'>"+
                 "<table cellspacing='0' border='1' style='font-size:11px; border:1px solid gray'>"+
                    "<tr>"+
                        "<td align='center' style=' min-width:192px;'>Shift</td>"+
                        "<td align='right' style=' min-width:120px;'>Production Qty</td>"+
                    "</tr>"+
                    "<tr>"+
                        "<td align='center' style=' min-width:192px;'>Shift A (6:00 AM - 2:00 PM)</td>"+
                        "<td align='right' style=' min-width:120px;'>" + Global.MillionFormat(nTotalProductionQtyShiftA).Split('.')[0] + "</td>" +
                    "</tr>"+
                    "<tr>"+
                        "<td align='center' style=' min-width:192px;'>Shift B (2:00 PM - 10:00 PM)</td>"+
                        "<td align='right' style=' min-width:120px;'>" + Global.MillionFormat(nTotalProductionQtyShiftB).Split('.')[0] + "</td>" +
                    "</tr>"+
                    "<tr>"+
                        "<td align='center' style=' min-width:192px;'>Shift C (10:00 PM - 6:00 AM)</td>"+
                        "<td align='right' style=' min-width:120px;'>" + Global.MillionFormat(nTotalProductionQtyShiftC).Split('.')[0] + "</td>" +
                    "</tr>"+
                    "<tr>" +
                        "<td align='center' style=' min-width:192px;'>Total</td>" +
                        "<td align='right' style=' min-width:120px;'>" + Global.MillionFormat(nTotalProductionQtyShiftA + nTotalProductionQtyShiftB + nTotalProductionQtyShiftC).Split('.')[0] + "</td>" +
                    "</tr>" +
                "</table>"+
            "</div>";

            #endregion

            #endregion




             return sBodyInformation;
        }



        #region Day Hour Min Conversion

        public String TimeStampCoversion(int nDay, int nHour, int nMin, int nCount)
        {
            string sAvgTime = "";
            //if (nCount > 0)// This condiiton is added by sshojib to resolve error..Dated on :03 Dec 2017
            //{
                if (nMin > 0)
                {
                    nHour = nHour + (nMin / 60);
                    nMin = nMin % 60;
                }
                if (nHour >= 24)
                {
                    nDay = nDay + (nHour / 24);
                    nHour = nHour % 24;
                }

                if (nMin > 0 || nHour > 0 || nDay > 0)
                {
                    int nRemainDay = 0;
                    if (nCount > 0) { nRemainDay= nDay % nCount;}
                     
                    if (nRemainDay != 0) { nHour += nRemainDay * 24; }
                    int nReminHour = 0;
                    if (nCount > 0) { nReminHour = nHour % nCount;}                    
                    if (nReminHour != 0) { nMin += nReminHour * 60; }

                    int nAvgDay = 0;
                    int nAvgHour = 0;
                    double nAvgMin =0;

                    if (nCount > 0) {
                        nAvgDay = nDay / nCount;
                        nAvgHour = nHour / nCount;
                        nAvgMin = nMin / nCount;
                    }

                    if (nAvgDay > 0)
                    {
                        nAvgHour = nAvgHour + (nAvgDay * 24);
                    }

                    sAvgTime = ((nAvgHour > 9) ? nAvgHour.ToString() : "0" + nAvgHour.ToString()) + ":" + ((nAvgMin > 9) ? nAvgMin.ToString() : "0" + nAvgMin.ToString());


                    //string sAvgDay = "", sAvgHour = "", sAvgMin = "";
                    //if (nAvgDay < 10) { sAvgDay = "0" + nAvgDay.ToString(); } else { sAvgDay = nAvgDay.ToString(); }
                    //if (nAvgHour < 10) { sAvgHour = "0" + nAvgHour.ToString(); } else { sAvgHour = nAvgHour.ToString(); }
                    //if (nAvgMin < 10)
                    //{
                    //    nAvgMin = Math.Round(nAvgMin, 2);
                    //    if (nAvgMin.ToString().Contains('.'))
                    //    {
                    //        sAvgMin = "0" + nAvgMin.ToString().Split('.')[0] + "." + nAvgMin.ToString().Split('.')[1];
                    //    }
                    //    else
                    //    {
                    //        sAvgMin = "0" + nAvgMin.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    sAvgMin = nAvgMin.ToString();
                    //}
                    //if (Convert.ToInt32(sAvgDay) <= 0) { sAvgTime = sAvgHour + "h " + sAvgMin + "m"; }
                    //else { sAvgTime = sAvgDay + "d " + sAvgHour + "h " + sAvgMin + "m"; }

                }
            //}
            return sAvgTime;
        }

        #endregion

        #region Blank Grid Generation
        private string BlankGridGeneration(bool IsBackGroundColored)
        {
            string sColor = (IsBackGroundColored) ? "#EFEEEE" : "none";
            string sBodyInformation = "";
            #region Production

            sBodyInformation = sBodyInformation +
            "<td align='right' style=' width:40px; background-color: " + sColor + "'></td>" +
            "<td align='right' style=' width:38px; background-color: " + sColor + "'></td>" +
            "<td align='right' style=' width:38px; background-color: " + sColor + "'></td>" +
            "<td align='right' style=' width:38px; background-color: " + sColor + "'></td>" +
            "<td align='right' style=' width:38px; background-color: " + sColor + "'></td>";

            #endregion

            #region Duration

            sBodyInformation = sBodyInformation +
                   "<td align='center' style=' width:40px; background-color: " + sColor + "'></td>" +
                   "<td align='center' style=' width:38px; background-color: " + sColor + "'></td>" +
                   "<td align='center' style=' width:38px; background-color: " + sColor + "'></td>" +
                   "<td align='center' style=' width:38px; background-color: " + sColor + "'></td>" +
                   "<td align='center' style=' width:38px; background-color: " + sColor + "'></td>";
            #endregion

            #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT
            sBodyInformation = sBodyInformation +
                   "<td align='right' style=' width:25px; background-color: " + sColor + "'></td>" +
                   "<td align='right' style=' width:25px; background-color: " + sColor + "'></td>" +
                   "<td align='right' style=' width:25px; background-color: " + sColor + "'></td>" +
                   "<td align='right' style=' width:25px; background-color: " + sColor + "'></td>" +
                   "<td align='right' style=' width:25px; background-color: " + sColor + "'></td>" +
                   "<td align='right' style=' width:25px; background-color: " + sColor + "'></td>" +
                   "<td align='right' style=' width:25px; background-color: " + sColor + "'></td>";
            #endregion

            return sBodyInformation;
        }
        #endregion
     
        #endregion

        #endregion

        #region Add Report Remak 13 May 2015

        public ActionResult ViewReportRemark()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SearchRepotRemark(DateTime dSearchDate, double nts)
        {
            ReportComments oRC = new ReportComments();
            List<ReportComments> oRCs = new List<ReportComments>();
            try
            {
                string sSQL = "Select * from ReportComments Where CommentDate='" + dSearchDate.ToString("dd MMM yyyy") + "'";
                oRCs = ReportComments.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRCs.Count() <= 0)
                {
                    throw new Exception("No information found.");
                }
            }
            catch (Exception ex)
            {
                oRCs = new List<ReportComments>();
                oRC.ErrorMessage = ex.Message;
                oRCs.Add(oRC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRepotRemark(ReportComments oRC, double nts)
        {
            try
            {
                if(oRC.RCID<=0)
                {
                    oRC = oRC.IUD((int)EnumDBOperation.Insert ,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oRC = oRC.IUD((int)EnumDBOperation.Update ,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }
            catch (Exception ex)
            {
                oRC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteRepotRemark(int nRCID, double nts)
        {
            ReportComments oRC = new ReportComments();
            try
            {
                if (nRCID <= 0) { throw new Exception("Unable to delete. Please valid item."); }
                oRC.RCID = nRCID;
                oRC = oRC.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRC.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

