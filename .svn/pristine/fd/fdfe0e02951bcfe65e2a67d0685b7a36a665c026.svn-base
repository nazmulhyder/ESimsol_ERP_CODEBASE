using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
 
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ESimSolFinancial.Controllers;

namespace ESimSolFinancial.Controllers
{
    public class DevelopmentRecapController : Controller
    {
        #region Declaration
        TechnicalSheet _oTechnicalSheet = new TechnicalSheet();
        DevelopmentRecap _oDevelopmentRecap = new DevelopmentRecap();
        List<DevelopmentRecap> _oDevelopmentRecaps = new List<DevelopmentRecap>();
        List<DevelopmentRecapSummary> _oDevelopmentRecapSummarys = new List<DevelopmentRecapSummary>();
        DevelopmentRecapDetail _oDevelopmentRecapDetail = new DevelopmentRecapDetail();
        List<DevelopmentRecapDetail> _oDevelopmentRecapDetails = new List<DevelopmentRecapDetail>();
        OrderRecap _OOrderRecap = new OrderRecap();
        OrderRecapDetail _OOrderRecapDetail = new OrderRecapDetail();
        List<OrderRecapDetail> _oOrderRecapDetails = new List<OrderRecapDetail>();
        List<ProductionOrder> _OProductionOrder = new List<ProductionOrder>();
        List<DevelopmentRecapHistory> _oDevelopmentRecapHistorys = new List<DevelopmentRecapHistory>();
        List<DevelopmentRecapSizeColorRatio> _oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
        List<DevelopmentYarnOption> _oDevelopmentYarnOptions = new List<DevelopmentYarnOption>();
        DevelopmentRecapSizeColorRatio _oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
        TechnicalSheetImage _oTechnicalSheetImage = new TechnicalSheetImage();
        TechnicalSheetThumbnail _oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        DevelopementRecapMgtReport _oDevelopementRecapMgtReport = new DevelopementRecapMgtReport();
        #endregion

        #region Functions
        private List<ColorSizeRatio> MapColorSizeRationFromDevelopmentRecapSizeColorRatios(List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios, List<TechnicalSheetSize> oSizes)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (DevelopmentRecapSizeColorRatio oItem in oDevelopmentRecapSizeColorRatios)
            {
                if (oItem.ColorID != nColorID)
                {
                    oColorSizeRatio = new ColorSizeRatio();
                    oColorSizeRatio.ColorID = oItem.ColorID;
                    oColorSizeRatio.ColorName = oItem.ColorName;
                    nCount = 0;
                    foreach (TechnicalSheetSize oSize in oSizes)
                    {
                        nCount++;
                        #region Set OrderQty
                        sPropertyName = "OrderQty" + nCount.ToString();
                        PropertyInfo prop = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oColorSizeRatio, GetQty(oSize.SizeCategoryID, oItem.ColorID, oDevelopmentRecapSizeColorRatios), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oDevelopmentRecapSizeColorRatios), null);
                        }
                        #endregion
                    }
                    #region ColorWiseTotal
                    sPropertyName = "ColorWiseTotal";
                    PropertyInfo propobjtotal = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjtotal && propobjtotal.CanWrite)
                    {
                        propobjtotal.SetValue(oColorSizeRatio, GetColorWiseTotalQty(oItem.ColorID, oDevelopmentRecapSizeColorRatios), null);
                    }
                    #endregion
                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }

        private double GetQty(int nSizeID, int nColorID, List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios)
        {
            foreach (DevelopmentRecapSizeColorRatio oItem in oDevelopmentRecapSizeColorRatios)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.Qty;
                }
            }
            return 0;
        }

        private double GetColorWiseTotalQty(int nColorID, List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios)
        {
            double nTotalQty = 0;
            foreach (DevelopmentRecapSizeColorRatio oItem in oDevelopmentRecapSizeColorRatios)
            {
                if (oItem.ColorID == nColorID)
                {
                    nTotalQty = nTotalQty + oItem.Qty;
                }
            }
            return nTotalQty;
        }

        private int GetObjectID(int nSizeID, int nColorID, List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios)
        {
            foreach (DevelopmentRecapSizeColorRatio oItem in oDevelopmentRecapSizeColorRatios)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.DevelopmentRecapSizeColorRatioID;
                }
            }
            return 0;
        }
        #region For Save Development Recap With Detail
        private List<DevelopmentRecapDetail> MapDevelopmentRecapDetailFromColorSizeRatio(List<TechnicalSheetSize> oSizes, List<DevelopmentRecapDetail> oDevelopmentRecapDetails)
        {
            _oDevelopmentRecapDetails = new List<DevelopmentRecapDetail>();
            _oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            DevelopmentRecapSizeColorRatio oTempDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();


            foreach (DevelopmentRecapDetail oDRD in oDevelopmentRecapDetails)
            {

                _oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
                _oDevelopmentRecapDetail = new DevelopmentRecapDetail();
                _oDevelopmentRecapDetail = oDRD;
                int nCount = 0;
                foreach (ColorSizeRatio oCSR in oDRD.ColorSizeRatios)
                {
                    nCount = 0;
                    foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                    {
                        nCount++;
                        oTempDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
                        oTempDevelopmentRecapSizeColorRatio = GetObjIDAndQty(nCount, oCSR);
                        if (oTempDevelopmentRecapSizeColorRatio.Qty > 0)
                        {
                            _oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
                            _oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oTempDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID;
                            _oDevelopmentRecapSizeColorRatio.DevelopmentRecapDetailID = oDRD.DevelopmentRecapDetailID;
                            _oDevelopmentRecapSizeColorRatio.ColorID = oCSR.ColorID;
                            _oDevelopmentRecapSizeColorRatio.SizeID = oTempTechnicalSheetSize.SizeCategoryID; ;
                            _oDevelopmentRecapSizeColorRatio.Qty = oTempDevelopmentRecapSizeColorRatio.Qty;
                            _oDevelopmentRecapSizeColorRatios.Add(_oDevelopmentRecapSizeColorRatio);
                        }
                    }
                }


                _oDevelopmentRecapDetail.DevelopmentRecapSizeColorRatios = _oDevelopmentRecapSizeColorRatios;
                _oDevelopmentRecapDetails.Add(_oDevelopmentRecapDetail);
            }

            return _oDevelopmentRecapDetails;
        }

        private DevelopmentRecapSizeColorRatio GetObjIDAndQty(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
            switch (nCount)
            {
                case 1:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID1;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty1;
                    break;
                case 2:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID2;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty2;
                    break;
                case 3:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID3;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty3;
                    break;
                case 4:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID4;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty4;
                    break;
                case 5:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID5;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty5;
                    break;
                case 6:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID6;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty6;
                    break;
                case 7:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID7;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty7;
                    break;
                case 8:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID8;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty8;
                    break;
                case 9:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID9;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty9;
                    break;
                case 10:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID10;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty10;
                    break;
                case 11:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID11;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty11;
                    break;
                case 12:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID12;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty12;
                    break;
                case 13:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID13;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty13;
                    break;
                case 14:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID14;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty14;
                    break;
                case 15:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID15;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty15;
                    break;
                case 16:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID16;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty16;
                    break;
                case 17:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID17;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty17;
                    break;
                case 18:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID18;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty18;
                    break;
                case 19:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID19;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty19;
                    break;
                case 20:
                    oDevelopmentRecapSizeColorRatio.DevelopmentRecapSizeColorRatioID = oColorSizeRatio.OrderObjectID20;
                    oDevelopmentRecapSizeColorRatio.Qty = oColorSizeRatio.OrderQty20;
                    break;
            }
            return oDevelopmentRecapSizeColorRatio;
        }
        #endregion


        #region Development Recap
        private List<SizeCategory> GetDistinctDevelopmentRecapSizes(int nDevelopmentRecapDetailID, List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios)
        {
            List<SizeCategory> oSizeCategoryies = new List<SizeCategory>();
            SizeCategory oSizeCategory = new SizeCategory();

            foreach (DevelopmentRecapSizeColorRatio oItem in oDevelopmentRecapSizeColorRatios)
            {
                if (oItem.DevelopmentRecapDetailID == nDevelopmentRecapDetailID)
                {
                    if (!IsDRSCRExist(oSizeCategoryies, oItem))
                    {
                        oSizeCategory = new SizeCategory();
                        oSizeCategory.SizeCategoryID = oItem.SizeID;
                        oSizeCategory.SizeCategoryName = oItem.SizeName;
                        oSizeCategoryies.Add(oSizeCategory);
                    }
                }
            }

            return oSizeCategoryies;
        }

        private bool IsDRSCRExist(List<SizeCategory> oSizeCategories, DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio)
        {
            foreach (SizeCategory oITem in oSizeCategories)
            {
                if (oITem.SizeCategoryID == oDevelopmentRecapSizeColorRatio.SizeID)
                {
                    return true;
                }
            }
            return false;
        }

        private List<ColorCategory> GetDistinctDevelopmentRecapColors(int nDevelopmentRecapDetailID, List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios)
        {
            List<ColorCategory> oColorCategories = new List<ColorCategory>();
            ColorCategory oColorCategory = new ColorCategory();
            foreach (DevelopmentRecapSizeColorRatio oItem in oDevelopmentRecapSizeColorRatios)
            {
                if (oItem.DevelopmentRecapDetailID == nDevelopmentRecapDetailID)
                {
                    if (!IsDRDSCRExist(oColorCategories, oItem))
                    {
                        oColorCategory = new ColorCategory();
                        oColorCategory.ColorCategoryID = oItem.ColorID;
                        oColorCategory.ColorName = oItem.ColorName;
                        oColorCategories.Add(oColorCategory);
                    }
                }
            }
            return oColorCategories;
        }

        private bool IsDRDSCRExist(List<ColorCategory> oColorCategories, DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio)
        {
            foreach (ColorCategory oITem in oColorCategories)
            {
                if (oITem.ColorCategoryID == oDevelopmentRecapSizeColorRatio.ColorID)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #endregion


        #region Action Result
        public ActionResult ViewDevelopmentRecaps(int nTSID, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DevelopmentRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDevelopmentRecaps = new List<DevelopmentRecap>();            
            if (menuid>0)
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);                   
            }
            else
            {
                _oDevelopmentRecaps = DevelopmentRecap.GetsByTechnicalSheet(nTSID, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            ViewBag.DevelopmentTypes = DevelopmentType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.DevelopmentRecapStatus = EnumObject.jGets(typeof(EnumDevelopmentStatus));
            ViewBag.BUID = buid;
            return View(_oDevelopmentRecaps);
        }

        public ActionResult ViewDevelopmentRecap(int id, int tsid, double ts)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            _oDevelopmentRecapDetails = new List<DevelopmentRecapDetail>();
            _oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            List<ContactPersonnel> oContactPersonnels = new List<ContactPersonnel>();
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            bool bIstsID = false;
            if (id > 0)
            {
                _oDevelopmentRecap = _oDevelopmentRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDevelopmentRecap.DevelopmentYarnOptions = DevelopmentYarnOption.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oDevelopmentRecapDetails = DevelopmentRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                List<TechnicalSheetSize> oTechnicalSheetSizes = TechnicalSheetSize.Gets(_oDevelopmentRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                foreach (DevelopmentRecapDetail oItem in _oDevelopmentRecapDetails)
                {
                    oDevelopmentRecapSizeColorRatios = DevelopmentRecapSizeColorRatio.Gets(oItem.DevelopmentRecapDetailID, (int)Session[SessionInfo.currentUserID]);
                    oItem.ColorSizeRatios = MapColorSizeRationFromDevelopmentRecapSizeColorRatios(oDevelopmentRecapSizeColorRatios, oTechnicalSheetSizes);
                }
            }
            else
            {
                oTechnicalSheet = oTechnicalSheet.Get(tsid, (int)Session[SessionInfo.currentUserID]);
                bIstsID = true;
                #region Map Development Recap
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.DevelopmentRecapID = 0;
                _oDevelopmentRecap.BusinessSessionID = oTechnicalSheet.BusinessSessionID;
                _oDevelopmentRecap.SessionName = "";
                _oDevelopmentRecap.DevelopmentRecapNo = "";
                _oDevelopmentRecap.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
                _oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.Initialize;
                _oDevelopmentRecap.InquiryReceivedDate = DateTime.MinValue;
                _oDevelopmentRecap.GG = oTechnicalSheet.GG;
                _oDevelopmentRecap.SampleQty = 3.00;
                _oDevelopmentRecap.SampleSizeID = 0;
                _oDevelopmentRecap.SampleReceivedDate = DateTime.MinValue;
                _oDevelopmentRecap.SampleSendingDate = DateTime.MinValue;
                _oDevelopmentRecap.SendingDeadLine = DateTime.MinValue;
                _oDevelopmentRecap.AwbNo = "";
                _oDevelopmentRecap.Remarks = "";
                _oDevelopmentRecap.SpecialFinish = oTechnicalSheet.SpecialFinish;
                _oDevelopmentRecap.MerchandiserID = 0;
                _oDevelopmentRecap.TransportType =  EnumTransportType.None;
                _oDevelopmentRecap.YarnCategoryID = oTechnicalSheet.YarnCategoryID;
                _oDevelopmentRecap.UnitID = oTechnicalSheet.MeasurementUnitID;
                _oDevelopmentRecap.Weight = oTechnicalSheet.Weight;
                _oDevelopmentRecap.UnitName = "";
                _oDevelopmentRecap.MerchandiserName = "";
                _oDevelopmentRecap.YarnName = oTechnicalSheet.FabricDescription;
                _oDevelopmentRecap.CollectionName = "";
                _oDevelopmentRecap.StyleNo = oTechnicalSheet.StyleNo;
                _oDevelopmentRecap.BuyerID = oTechnicalSheet.BuyerID;
                _oDevelopmentRecap.BuyerContactPersonID = 0;
                _oDevelopmentRecap.SampleSize = "";
                _oDevelopmentRecap.BuyerName = oTechnicalSheet.BuyerName;
                _oDevelopmentRecap.BuerContactPersonName = "";
                _oDevelopmentRecap.ErrorMessage = "";
                _oDevelopmentRecap.PrepareBy = "";
                _oDevelopmentRecap.StyleCoverImage = null;
                _oDevelopmentRecap.ProductName = oTechnicalSheet.ProductName;
                _oDevelopmentRecap.FabricOptionA = "";
                _oDevelopmentRecap.ProductID = oTechnicalSheet.ProductID;
                _oDevelopmentRecap.FabricOptionB = "";
                _oDevelopmentRecap.FabricOptionC = "";
                _oDevelopmentRecap.Count = oTechnicalSheet.Count;
                _oDevelopmentRecap.ProductCategoryID = 0;
                _oDevelopmentRecap.OperationDate = DateTime.Today;
                _oDevelopmentRecap.Note = "";
                _oDevelopmentRecap.OperationBy = 0;
                _oDevelopmentRecap.CurrentStatusInt = (int)EnumDevelopmentStatus.Initialize; ;
                _oDevelopmentRecap.DevelopmentType = 0;
                _oDevelopmentRecap.OrderRecapQty = 0;
                _oDevelopmentRecap.DevelopmentYarnOptions = new List<DevelopmentYarnOption>();
                _oDevelopmentRecap.ColorSizeRatios = new List<ColorSizeRatio>();
                _oDevelopmentRecap.DevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
                #endregion
            }
            _oDevelopmentRecap.ContactPersonnels = ContactPersonnel.GetsByContractor(_oDevelopmentRecap.BuyerID, (int)Session[SessionInfo.currentUserID]);
            if(bIstsID==true)
            {
                foreach(ContactPersonnel oItem in _oDevelopmentRecap.ContactPersonnels)
                {
                    if(oTechnicalSheet.ConcernName==oItem.Name)
                    {
                        _oDevelopmentRecap.BuyerContactPersonID = oItem.ContactPersonnelID;
                        break;
                    }
                }
            }
            Currency oCurrency = new Currency();
            List<Currency> oCurrencys = new List<Currency>();
            oCurrency.CurrencyName = "--Currency--";
            oCurrencys.Add(oCurrency);
            oCurrencys.AddRange(Currency.Gets((int)Session[SessionInfo.currentUserID]));

            _oDevelopmentRecap.Currencys = oCurrencys;
            _oDevelopmentRecap.DevelopmentRecapDetails = _oDevelopmentRecapDetails;
            _oDevelopmentRecap.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);           
            ViewBag.DevelopmentTypes = DevelopmentType.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oDevelopmentRecap);
        }             

        [HttpPost]
        public JsonResult GetSizeAndUnits(DevelopmentRecapDetail oDevelopmentRecapDetail)
        {
            _oDevelopmentRecapDetail = new DevelopmentRecapDetail();
            _oDevelopmentRecapDetail.Units = MeasurementUnit.GetsbyProductID(oDevelopmentRecapDetail.ProductID, (int)Session[SessionInfo.currentUserID]);
            _oDevelopmentRecapDetail.TechnicalSheetSizes = TechnicalSheetSize.Gets(oDevelopmentRecapDetail.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecapDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Report Of Development Recap  Mgt.
        public ActionResult DevelopmentRecapReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDevelopmentRecaps = new List<DevelopmentRecap>();
            DevelopmentType oDevelopmentType = new DevelopmentType();
            List<DevelopmentType> oDevelopmentTypes = new List<DevelopmentType>();
            oDevelopmentType.Name = "None";
            oDevelopmentTypes.Add(oDevelopmentType);
            oDevelopmentTypes.AddRange(DevelopmentType.Gets((int)Session[SessionInfo.currentUserID]));
            ViewBag.DevelopmentTypes = oDevelopmentTypes;
            ViewBag.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.DevelopmentRecapStatus = EnumObject.jGets(typeof(EnumDevelopmentStatus));
            ViewBag.ProductCategories = ProductCategory.GetsBUWiseLastLayer(buid, (int)Session[SessionInfo.currentUserID]);
            return View(_oDevelopmentRecaps);
        }

        public ActionResult DevelopmentRecapManagementReport(string sParam , int ReportFormat)
        {
            _oDevelopementRecapMgtReport = new DevelopementRecapMgtReport();
            // _oDevelopmentRecap.
            Company oCompany = new Company();
            _oDevelopmentRecaps = new List<DevelopmentRecap>();
            string sSQL = MakeQuery(sParam, ReportFormat);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDevelopmentRecapMgt oReport = new rptDevelopmentRecapMgt();
            if (ReportFormat == (int)EnumReportLayout.YarnFabricWise)
            {
                #region User Set
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {
                    sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                }
                #endregion
                sSQL += "AND YarnCategoryID > 0 Order By YarnCategoryID";
                _oDevelopmentRecaps = DevelopmentRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oDevelopmentRecaps.Count > 0)
                {
                    byte[] abytes = oReport.YarnWisePrepareReport(_oDevelopmentRecaps, oCompany);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    string sMessage = "There is no data for print";
                    return RedirectToAction("MessageHelper", "User", new { message = sMessage });
                }
            }
            else
            {
                _oDevelopementRecapMgtReport.DevelopementRecapMgtReports = DevelopementRecapMgtReport.Gets(sSQL, ReportFormat, (int)Session[SessionInfo.currentUserID]);
                if (_oDevelopementRecapMgtReport.DevelopementRecapMgtReports.Count > 0)
                {
                    if (ReportFormat == (int)EnumReportLayout.Factorywise)
                    {
                        byte[] abytes = oReport.FactoryWisePrepareReport(_oDevelopementRecapMgtReport, oCompany);
                        return File(abytes, "application/pdf");
                    }
                    else if (ReportFormat == (int)EnumReportLayout.MerchandiserWise)
                    {
                        byte[] abytes = oReport.MerchandiserWisePrepareReport(_oDevelopementRecapMgtReport, oCompany);
                        return File(abytes, "application/pdf");
                    }
                    else if (ReportFormat == (int)EnumReportLayout.ProductCatagoryWise)
                    {
                        byte[] abytes = oReport.ProductCategoryWisePrepareReport(_oDevelopementRecapMgtReport, oCompany);
                        return File(abytes, "application/pdf");
                    }
                    else if (ReportFormat == (int)EnumReportLayout.PartyWise)
                    {
                        byte[] abytes = oReport.BuyerWisePrepareReport(_oDevelopementRecapMgtReport, oCompany);
                        return File(abytes, "application/pdf");
                    }
                    else
                    {

                        string sMessage = "There is no Report for print";
                        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
                    }

                }
                else
                {

                    string sMessage = "There is no data for print";
                    return RedirectToAction("MessageHelper", "User", new { message = sMessage });
                }
            }
        }

        public string MakeQuery(string sTemp, int nReportFormat)
        {
            string sReturnMain = "";
            if (nReportFormat == (int)EnumReportLayout.YarnFabricWise)
            {
                sReturnMain = "SELECT * FROM View_DevelopmentRecap ";
            }
            else
            {
                sReturnMain = "SELECT DevelopmentRecapID, DevelopmentRecapDetailID, FactoryID FROM View_DevelopmentRecapDetail WHERE DevelopmentRecapID IN ( SELECT DevelopmentRecapID FROM View_DevelopmentRecap";
            }            
            string sReturn = "";
         
            /*Inquyery  Receive Date Set*/
            int nInqueryRcvDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dInqueryRcvStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dInquerRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            /*Sample Receive Date Set*/
            int nSampleRcvDate = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dSampleRcvStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dSampleRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            /*Sending Date Set*/
            int nSendingDate = Convert.ToInt32(sTemp.Split('~')[6]);
            DateTime dSendingStartDate = Convert.ToDateTime(sTemp.Split('~')[7]);
            DateTime dSendingEndDate = Convert.ToDateTime(sTemp.Split('~')[8]);


            string sRecapNo = sTemp.Split('~')[9];
            string sStyleNo = sTemp.Split('~')[10];
            string sBuyerIDs = sTemp.Split('~')[11];
            int nProductCatagoryID = Convert.ToInt32(sTemp.Split('~')[12]);
            int nDevelopmentTypeID = Convert.ToInt32(sTemp.Split('~')[13]);
            string sStatus = sTemp.Split('~')[14];
            string sMerchandiserIDs = sTemp.Split('~')[15];
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[16]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[17]);
            
            #region Recap No

            if (sRecapNo != "")
            {
                  Global.TagSQL(ref sReturn);
                  sReturn = sReturn + " DevelopmentRecapNo ='" + sRecapNo + "'";

            }
            #endregion
            
            #region Development Type
            if (nDevelopmentTypeID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DevelopmentType =" + nDevelopmentTypeID;

            }
            #endregion
            
            #region Status

            if (sStatus != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DevelopmentStatus IN (" + sStatus + ")";

            }

            #endregion

            #region Merchandiser 

            if (sMerchandiserIDs!= "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID IN (" + sMerchandiserIDs + ")";

            }

            #endregion

            #region Business Session

            if (nSessionID >0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nSessionID;

            }

            #endregion
                        
            #region Inquery Receive Date
            if (nInqueryRcvDate > 0)
            {
                if (nInqueryRcvDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate = '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate != '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate > '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate < '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate>= '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "' AND InquiryReceivedDate < '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate< '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "' OR InquiryReceivedDate > '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }

            }
            #endregion

            #region Sample Receive Date
            if (nSampleRcvDate > 0)
            {
                if (nSampleRcvDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate = '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate != '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate > '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate < '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate>= '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "' AND SampleReceivedDate < '" + dSampleRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate< '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "' OR SampleReceivedDate > '" + dSampleRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
         
            }
            #endregion
            
            #region Sending Date
            if (nSendingDate > 0)
            {
                if (nSendingDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate = '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate != '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate > '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate < '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate>= '" + dSendingStartDate.ToString("dd MMM yyyy") + "' AND SampleSendingDate < '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate< '" + dSendingStartDate.ToString("dd MMM yyyy") + "' OR SampleSendingDate > '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
      
            }
            #endregion

            #region Style Part
            string sTSPart = " TechnicalSheetID  IN (SELECT TS.TechnicalSheetID FROM TechnicalSheet AS TS"; // start Technical Sheet Part 
            string sTSPart2 = "";

            #region Style No
            if (sStyleNo != "")
            {
                Global.TagSQL(ref sTSPart2);
                sTSPart2 = sTSPart2 + " TS.StyleNo LIKE'%" + sStyleNo + "%'";                
            }
            #endregion

            #region Buyer Name
            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sTSPart2);
                sTSPart2 = sTSPart2 + " TS.BuyerID IN  (" + sBuyerIDs + ")";
            }

            #endregion

            #region Prouduct Catagory
            if (nProductCatagoryID > 0)
            {
                Global.TagSQL(ref sTSPart2);
                sTSPart2 = sTSPart2 + "TS.ProductID IN (SELECT VP.ProductID FROM View_Product AS VP WHERE VP.ProductCategoryID =" + nProductCatagoryID.ToString() + ")";
            }
            #endregion

            #region User Set
            if (nReportFormat != (int)EnumReportLayout.YarnFabricWise)
            {
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {
                    Global.TagSQL(ref sTSPart2);
                    sTSPart2 = sTSPart2 + "  TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                }
            }
            #endregion

            if (sTSPart2 != "")
            {
                sTSPart2 = sTSPart + sTSPart2 + ")";                
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + sTSPart2;
            }
            #endregion

            #region Business Unit
            if (nBUID> 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = "+nBUID;
            }
            #endregion

            if (nReportFormat != (int)EnumReportLayout.YarnFabricWise)
            {
                sReturn = sReturn + ")";
            }

            sReturnMain = sReturnMain + sReturn;
             return sReturnMain;
        }
        #endregion


        # endregion Action Result

        [HttpPost]
        public JsonResult RefreshControl(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                _oDevelopmentRecap = _oDevelopmentRecap.Get(oDevelopmentRecap.DevelopmentRecapID, (int)Session[SessionInfo.currentUserID]);
                _oDevelopmentRecap.DevelopmentStatusInInt = (int)_oDevelopmentRecap.DevelopmentStatus;
                _oDevelopmentRecap.DevelopmentRecapDetails = DevelopmentRecapDetail.Gets(oDevelopmentRecap.DevelopmentRecapID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Print
        private string FillImageUrl(string sImageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            return url + "Content/" + sImageName;
        }

        public void DevelopmentRecapPrintList()
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            _oDevelopmentRecap.DevelopmentRecapList = DevelopmentRecap.Gets((int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            _oDevelopmentRecap.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //return this.ViewPdf("", "rptSampleOrderList", _oDevelopmentRecap, 40, 40, 40, 40, false);
        }

        public void DevelopmentRecapPreviewPrint(int id)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            Company oCompany = new Company();
            if (id > 0)
            {
                _oDevelopmentRecap = _oDevelopmentRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDevelopmentRecapDetails = DevelopmentRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oDevelopmentRecap.DevelopmentYarnOptions = GetDevelopmentYarnOptions(id);
            }
            _oDevelopmentRecap.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oDevelopmentRecap.ImageUrl = FillImageUrl("Sample.jpg");
            //_oDevelopmentRecap.ImageUrl = FillImageUrl("EWYDLB.GIF");
            //return this.ViewPdf("", "rptSampleOrderPreview", _oDevelopmentRecap, 40, 40, 40, 40, false);
        }

        #endregion Print


        public int CheckOrderRecapDetails(int nColorID, int nSizeID)
        {
            int i = -1;
            foreach (OrderRecapDetail oItem in _oOrderRecapDetails)
            {
                i++;
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return i;
                }
            }
            return -1;
        }


        [HttpPost]
        public JsonResult Save(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                #region  Development Recap
                _oDevelopmentRecap = oDevelopmentRecap;
                _oDevelopmentRecap.DevelopmentStatus = (EnumDevelopmentStatus)oDevelopmentRecap.DevelopmentStatusInInt;
                _oDevelopmentRecap.DevelopmentType = oDevelopmentRecap.DevelopmentType;
                _oDevelopmentRecap.DevelopmentYarnOptions = oDevelopmentRecap.DevelopmentYarnOptions;
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(oDevelopmentRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oDevelopmentRecap.DevelopmentRecapDetails = MapDevelopmentRecapDetailFromColorSizeRatio(oTechnicalSheetSizes, oDevelopmentRecap.DevelopmentRecapDetails);

                #endregion
                if (_oDevelopmentRecap.SampleReceivedDate != DateTime.MinValue)
                {
                    if (_oDevelopmentRecap.InquiryReceivedDate > _oDevelopmentRecap.SampleReceivedDate)
                    {
                        _oDevelopmentRecap.ErrorMessage = "Your enterd Inq. Recv.Date is gatter than Sample Recive Date!!!";
                    }
                    else if (_oDevelopmentRecap.SampleReceivedDate > _oDevelopmentRecap.SampleSendingDate)
                    {
                        _oDevelopmentRecap.ErrorMessage = "Your enterd Sample  Recv.Date is gatter than Sample Sending Date!!!";
                    }
                    else
                    {
                        _oDevelopmentRecap = _oDevelopmentRecap.Save((int)Session[SessionInfo.currentUserID]);
                    }
                }
                else
                {
                    _oDevelopmentRecap = _oDevelopmentRecap.Save((int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                smessage = _oDevelopmentRecap.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region HTTP GET Details of Development Recap
        [HttpGet]
        public JsonResult GetDevelopmentRecapDetails(int id)//id is Development Recap Id
        {
            _oDevelopmentRecapDetails = new List<DevelopmentRecapDetail>();
            try
            {
                _oDevelopmentRecapDetails = DevelopmentRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oDevelopmentRecapDetail = new DevelopmentRecapDetail();
                _oDevelopmentRecapDetail.ErrorMessage = ex.Message;
                _oDevelopmentRecapDetails.Add(_oDevelopmentRecapDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecapDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        # region Inproduction

        [HttpPost]
        public JsonResult SaveInProduction(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                _oDevelopmentRecap = oDevelopmentRecap;
                _OOrderRecap = _OOrderRecap.Gets_byTechnicalSheet(_oDevelopmentRecap.TechnicalSheetID, 2, (int)Session[SessionInfo.currentUserID]);
              //  _OProductionOrder = ProductionOrder.Gets_bySalorderID(_OOrderRecap.OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                _oDevelopmentRecap.ProductionOrders = _OProductionOrder;
                _oDevelopmentRecap = _oDevelopmentRecap.SaveInProduction((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion InProduction

        private string GetDevelopmentRecapInString(List<DevelopmentRecapDetail> oDevelopmentRecapDetails)
        {
            string sDevelopmentRecapInString = "";
            foreach (DevelopmentRecapDetail oItem in oDevelopmentRecapDetails)
            {
                sDevelopmentRecapInString = "";//sDevelopmentRecapInString + oItem.DevelopmentRecapDetailID.ToString() + "~" + oItem.YarnCategoryID.ToString() + "~" + oItem.Note + "^";
            }
            if (sDevelopmentRecapInString.Length > 0)
            {
                sDevelopmentRecapInString = sDevelopmentRecapInString.Remove(sDevelopmentRecapInString.Length - 1, 1);
            }
            return sDevelopmentRecapInString;
        }



        #region Development Recap Summary
        public ActionResult ViewDevelopmentRecapSummary(int ispre, int count, int smr, int sminr, int mr, string ReportHeader, int BuyerID, int BrandID,  int Category, int BusinessSessionID, int DevelopmentType, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DevelopmentRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));            

            #region set Start & End Point
            int nStartRow = 0;
            int nEndRow = 0;
            //int nBuyerID = 0;
            //nBuyerID = BuyerID;
            if (ispre != 1)
            {
                nStartRow = smr + 1;
                nEndRow = smr + count;

                if (nStartRow > mr)
                {
                    nStartRow = sminr;
                    nEndRow = smr;
                }
            }
            else
            {
                nStartRow = sminr - count;
                //nBuyerID = BuyerID;

                if (nStartRow < 0)
                {
                    nStartRow = 1;
                }

                if (sminr > count)
                {
                    nEndRow = sminr - 1;
                }
                else
                {
                    nEndRow = smr;
                }
            }
            #endregion

            _oDevelopmentRecapSummarys = new List<DevelopmentRecapSummary>();
            DevelopmentRecapSummary oDevelopmentRecapSummary = new DevelopmentRecapSummary();
            string sDevelopmentRecapIDs = "0";
            string sSQL = "";
            if (BuyerID == 0 && Category == 0 && BusinessSessionID == 0)
            {
                _oDevelopmentRecapSummarys = new List<DevelopmentRecapSummary>();
            }
            else
            {
                sSQL = GetRecapSummarySQL(BuyerID, Category, BusinessSessionID, DevelopmentType, BrandID, buid);
                _oDevelopmentRecapSummarys = DevelopmentRecapSummary.GetsRecapWithDevelopmentRecapSummarys(nStartRow, nEndRow, sSQL, sDevelopmentRecapIDs, false, (int)Session[SessionInfo.currentUserID]);
            }
            if (_oDevelopmentRecapSummarys.Count > 0)
            {
                sDevelopmentRecapIDs = DevelopmentRecapSummary.IDInString(_oDevelopmentRecapSummarys);
                _oDevelopmentYarnOptions = new List<DevelopmentYarnOption>();
                sSQL = "SELECT * FROM View_DevelopmentYarnOption WHERE DevelopmentRecapID IN(" + sDevelopmentRecapIDs + ")";
                _oDevelopmentYarnOptions = DevelopmentYarnOption.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (DevelopmentRecapSummary oItem in _oDevelopmentRecapSummarys)
                {
                    oItem.DevelopmentYarnOptions = GetDevelopmentYarnOptions(oItem.DevelopmentRecapID);
                }
            }

            #region Searching Data
            oDevelopmentRecapSummary.DevelopmentRecapSummarys = _oDevelopmentRecapSummarys;
            if ((int)Session[SessionInfo.FinancialUserType] != (int)EnumFinancialUserType.Normal_User)
            {
                oDevelopmentRecapSummary.ContractorList = Contractor.GetsByNamenType("",((int)EnumContractorType.Buyer).ToString(),buid, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
              string  sContractorSQL = "SELECT * FROM Contractor WHERE ContractorType= " + (int)EnumContractorType.Buyer + " AND ContractorID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ") Order by [Name]";
              oDevelopmentRecapSummary.ContractorList = (Contractor.Gets(sContractorSQL, (int)Session[SessionInfo.currentUserID]));
            }
            
            string sTempSQL = "SELECT * FROM VIEW_ProductCategory WHERE ParentCategoryID=3";
            oDevelopmentRecapSummary.ProductCategorys = ProductCategory.Gets(sTempSQL, (int)Session[SessionInfo.currentUserID]);//here 3=Finish Goods 
            oDevelopmentRecapSummary.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);//here business year get distinct shipment year
            #endregion

            #region Temp Data
            TempData["message"] = count;          
            TempData["DevelopmentRecapIDs"] = sDevelopmentRecapIDs;
            TempData["BuyerID"] = BuyerID;
            TempData["BrandID"] = BrandID;
            TempData["Category"] = Category;
            TempData["BusinessSessionID"] = BusinessSessionID;
            TempData["DevelopmentType"] = DevelopmentType;
            TempData["ReportHeader"] = ReportHeader;
            TempData["BUID"] = buid;
            TempData["menuid"] = menuid;
            if (_oDevelopmentRecapSummarys.Count > 0)
            {
                TempData["numberlist"] = "(" + _oDevelopmentRecapSummarys[0].RowNumber.ToString() + " - " + _oDevelopmentRecapSummarys[_oDevelopmentRecapSummarys.Count - 1].RowNumber.ToString() + ") of " + _oDevelopmentRecapSummarys[0].MaxRowNumber.ToString();
            }
            oDevelopmentRecapSummary.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.DevelopmentTypes = ESimSol.BusinessObjects.DevelopmentType.Gets((int)Session[SessionInfo.currentUserID]);
            #endregion

            return View(oDevelopmentRecapSummary);
        }

        public string GetRecapSummarySQL(int BuyerID, int ProductCategoryID, int BusinessSessionID, int DevelopmentType, int nBrandID, int buid)
        {
            string sSQL = "SELECT DevelopmentRecapID FROM View_DevelopmentRecap";
            string sReturn = "";

            #region Bu
            if (buid > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + buid;
            }
            #endregion
            #region Buyer Name
            if (BuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID = " + BuyerID.ToString();
            }
            #endregion

            #region Product Category
            if (ProductCategoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE ProductID IN (SELECT ProductID FROM View_Product WHERE ProductCategoryID =" + ProductCategoryID.ToString() + "))";
            }
            #endregion

            #region Business Year
            if (BusinessSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + BusinessSessionID.ToString();
            }
            #endregion

            #region Buyer Name
            if (DevelopmentType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DevelopmentType = " + DevelopmentType;
            }
            #endregion

            #region Brand
            if (nBrandID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE BrandID=" + nBrandID + " )";
            }
            #endregion

            #region Avoid Hidden Style
            Global.TagSQL(ref sReturn);
            sReturn += " TechnicalSheetID NOT IN (SELECT TT.TechnicalSheetID FROM UserWiseStyleConfigure AS TT  WHERE TT.UserID=" + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";
            #endregion

            sSQL = sSQL + sReturn;
            return sSQL;
        }

        public ActionResult PrintDevelopmentRecapSummary(string RecapIDs, string ReportHeader)
        {
            List<DevelopmentRecapSummary> oDevelopmentRecapSummarys = new List<DevelopmentRecapSummary>();
            _oDevelopmentRecapSummarys = new List<DevelopmentRecapSummary>();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oDevelopmentRecapSummarys = DevelopmentRecapSummary.GetsRecapWithDevelopmentRecapSummarys(0, 0, "",RecapIDs, true, (int)Session[SessionInfo.currentUserID]);
            _oDevelopmentYarnOptions = new List<DevelopmentYarnOption>();
            string sSQL = "SELECT * FROM View_DevelopmentYarnOption WHERE DevelopmentRecapID IN(" + DevelopmentRecapSummary.IDInString(oDevelopmentRecapSummarys) + ") ORDER by DevelopmentYarnOptionID";
            _oDevelopmentYarnOptions = DevelopmentYarnOption.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (DevelopmentRecapSummary oItem in oDevelopmentRecapSummarys)
            {
                oItem.StyleCoverImage = GetThumImageForPrint(oItem.TechnicalSheetID);
                oItem.FabricOptionA = GetOptionWiseFabric(0, oItem.DevelopmentRecapID);
                oItem.FabricOptionB = GetOptionWiseFabric(1, oItem.DevelopmentRecapID);
                oItem.FabricOptionC = GetOptionWiseFabric(2, oItem.DevelopmentRecapID);
                _oDevelopmentRecapSummarys.Add(oItem);
            }
            rptDevelopmentRecapSummary oReport = new rptDevelopmentRecapSummary();
            byte[] abytes = oReport.PrepareReport(_oDevelopmentRecapSummarys, oCompany, ReportHeader);
            return File(abytes, "application/pdf");
        }

        public ActionResult ViewRecapDetail(int id)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            _oDevelopmentRecap = _oDevelopmentRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oDevelopmentRecapDetails = new List<DevelopmentRecapDetail>();
            _oDevelopmentRecapDetails = DevelopmentRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oDevelopmentRecap.DevelopmentYarnOptions = GetDevelopmentYarnOptions(id);
            return View(_oDevelopmentRecap);
        }

        public Image GetLargeImage(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        private List<DevelopmentYarnOption> GetDevelopmentYarnOptions(int nDRID)
        {
            List<DevelopmentYarnOption> oDevelopmentYarnOptions = new List<DevelopmentYarnOption>();
            int nCount = 0;
            foreach (DevelopmentYarnOption oItem in _oDevelopmentYarnOptions)
            {
                if (oItem.DevelopmentRecapID == nDRID)
                {
                    nCount++;
                    oItem.FabricsOptions = GetFabricOption(nCount);
                    oDevelopmentYarnOptions.Add(oItem);
                }
            }
            DevelopmentYarnOption oDevelopmentYarnOption = new DevelopmentYarnOption();
            if (nCount < 4)
            {
                int n = 4 - nCount;
                for (int i = 1; i <= n; i++)
                {
                    nCount++;
                    oDevelopmentYarnOption = new DevelopmentYarnOption();
                    oDevelopmentYarnOption.FabricsOptions = GetFabricOption(nCount);
                    oDevelopmentYarnOptions.Add(oDevelopmentYarnOption);
                }
            }
            return oDevelopmentYarnOptions;
        }

        public Image GetThumImage(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                //img.Save(Server.MapPath("~/Content/") + "styleimage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetThumImageForPrint(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "styleimage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        private string GetOptionWiseFabric(int nIdex, int nDRID)
        {
            List<DevelopmentYarnOption> oDevelopmentYarnOptions = new List<DevelopmentYarnOption>();
            foreach (DevelopmentYarnOption oItem in _oDevelopmentYarnOptions)
            {
                if (oItem.DevelopmentRecapID == nDRID)
                {
                    oDevelopmentYarnOptions.Add(oItem);
                }
            }

            if (nIdex < oDevelopmentYarnOptions.Count)
            {
                if (oDevelopmentYarnOptions[nIdex].YarnPly != "")
                {
                    return oDevelopmentYarnOptions[nIdex].ProductName + ", " + oDevelopmentYarnOptions[nIdex].YarnPly;
                }
                else
                {
                    return oDevelopmentYarnOptions[nIdex].ProductName;
                }
            }
            else
            {
                return "";
            }
        }
        private string GetFabricOption(int n)
        {
            string sOption = "";
            switch (n)
            {
                case 1:
                    sOption = "A";
                    break;
                case 2:
                    sOption = "B";
                    break;
                case 3:
                    sOption = "C";
                    break;
                case 4:
                    sOption = "D";
                    break;
                case 5:
                    sOption = "E";
                    break;
                case 6:
                    sOption = "F";
                    break;
                case 7:
                    sOption = "G";
                    break;
                case 8:
                    sOption = "H";
                    break;
                case 9:
                    sOption = "I";
                    break;
                case 10:
                    sOption = "J";
                    break;
                default:
                    sOption = "N/A";
                    break;
            }
            //sOption = "Yarn-" + sOption;
            sOption = "Yarn/Count/Ply";
            return sOption;
        }


        #endregion

        #region Advance Search
        
        #region HttpGet For Search
        [HttpGet]
        public JsonResult Gets(string sTemp)
        {
            List<DevelopmentRecap> oDevelopmentRecaps = new List<DevelopmentRecap>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oDevelopmentRecaps = DevelopmentRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDevelopmentRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            /*Inquyery  Receive Date Set*/
            int nInqueryRcvDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dInqueryRcvStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dInquerRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            /*Sample Receive Date Set*/
            int nSampleRcvDate = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dSampleRcvStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dSampleRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            /*Sending Date Set*/
            int nSendingDate = Convert.ToInt32(sTemp.Split('~')[6]);
            DateTime dSendingStartDate = Convert.ToDateTime(sTemp.Split('~')[7]);
            DateTime dSendingEndDate = Convert.ToDateTime(sTemp.Split('~')[8]);


            string sRecapNo = sTemp.Split('~')[9];
            string sStyleNo = sTemp.Split('~')[10];
            string sBuyerIDs = sTemp.Split('~')[11];
            int nProductCatagoryID = Convert.ToInt32(sTemp.Split('~')[12]);
            int nDevelopmentTypeID = Convert.ToInt32(sTemp.Split('~')[13]);
            string sStatus = sTemp.Split('~')[14];
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[15]);
            int nBrandID = Convert.ToInt32(sTemp.Split('~')[16]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[17]);

            string sReturn1 = "SELECT * FROM View_DevelopmentRecap";
            string sReturn = "";

            #region BUIID
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID =" + nBUID;
            }
            #endregion

            #region Recap No

            if (sRecapNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DevelopmentRecapNo ='" + sRecapNo + "'";
            }
            #endregion

            #region Style No

            if (sStyleNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleNo ='" + sStyleNo + "'";
            }
            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }

            #endregion

            #region Prouduct Catagory

            if (nProductCatagoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE ProductID IN (SELECT ProductID FROM View_Product WHERE ProductCategoryID =" + nProductCatagoryID + "))";
            }

            #endregion

            #region Development Type
            if (nDevelopmentTypeID >0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DevelopmentType =" +nDevelopmentTypeID;
            }
            #endregion


            #region Status

            if (sStatus != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DevelopmentStatus IN (" + sStatus + ")";
            }

            #endregion

            #region Session
            if (nSessionID > 0)
            {
                
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID ="+nSessionID;
                
            }
            #endregion

            #region Brand
            if (nBrandID > 0)
            {

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BrandID =" + nBrandID;

            }
            #endregion

            #region Date Wise
            #region Inquery Receive Date
            if (nInqueryRcvDate > 0)
            {
                if (nInqueryRcvDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate = '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate != '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate > '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate < '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate>= '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "' AND InquiryReceivedDate < '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nInqueryRcvDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InquiryReceivedDate< '" + dInqueryRcvStartDate.ToString("dd MMM yyyy") + "' OR InquiryReceivedDate > '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region Sample Receive Date
            if (nSampleRcvDate > 0)
            {
                if (nSampleRcvDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate = '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate != '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate > '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate < '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate>= '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "' AND SampleReceivedDate < '" + dSampleRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nSampleRcvDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleReceivedDate< '" + dSampleRcvStartDate.ToString("dd MMM yyyy") + "' OR SampleReceivedDate > '" + dSampleRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion


            #region Sending Date
            if (nSendingDate > 0)
            {
                if (nSendingDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate = '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate != '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate > '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate < '" + dSendingStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate>= '" + dSendingStartDate.ToString("dd MMM yyyy") + "' AND SampleSendingDate < '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nSendingDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SampleSendingDate< '" + dSendingStartDate.ToString("dd MMM yyyy") + "' OR SampleSendingDate > '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            
            #region Avoid Hidden Style
            Global.TagSQL(ref sReturn);
            sReturn += " TechnicalSheetID NOT IN (SELECT TT.TechnicalSheetID FROM UserWiseStyleConfigure AS TT  WHERE TT.UserID=" + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";            
            #endregion

            

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #endregion

        #region Update Development Recap Date
        [HttpPost]
        public JsonResult UpdateInquiryReceivedDate(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                _oDevelopmentRecap = _oDevelopmentRecap.UpdateInqRcvDate(oDevelopmentRecap.DevelopmentRecapID, oDevelopmentRecap.InquiryReceivedDate, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSampleReceivedDate(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                _oDevelopmentRecap = _oDevelopmentRecap.UpdateSmplRcvDate(oDevelopmentRecap.DevelopmentRecapID, oDevelopmentRecap.SampleReceivedDate, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSampleSendingDate(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                _oDevelopmentRecap = _oDevelopmentRecap.UpdateSmplSendingDate(oDevelopmentRecap.DevelopmentRecapID, oDevelopmentRecap.SampleSendingDate, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult UpdateSendingDeadLine(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                _oDevelopmentRecap = _oDevelopmentRecap.UpdateSendingDeadLine(oDevelopmentRecap.DevelopmentRecapID, oDevelopmentRecap.SendingDeadLine, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

       

        #region Active in Active
        [HttpPost]
        public JsonResult ActiveInActive(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                _oDevelopmentRecap = oDevelopmentRecap.ActiveInActive(oDevelopmentRecap.DevelopmentRecapID, oDevelopmentRecap.IsActive, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region HTTP ChangeStatus

        [HttpPost]
        public JsonResult ChangeStatus(DevelopmentRecap oDevelopmentRecap)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                if (oDevelopmentRecap.ActionTypeExtra == "RequestForApproved")
                {
                    //  oDevelopmentRecap.OperationBy=
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.RequestForApproved;

                }
                else if (oDevelopmentRecap.ActionTypeExtra == "UndoRequest")
                {

                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.UndoRequest;

                }
                else if (oDevelopmentRecap.ActionTypeExtra == "ApproveDone")
                {

                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.ApprovedDone;
                    
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "UndoApprove")
                {

                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.UndoApprove;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "InProduction")
                {

                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.Inproduction;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "RawMaterialCollectionDone")
                {
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.RawMaterialCollectionDone;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "QCDone")
                {
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.QCDone;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "ReceiveFromFactory")
                {
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.ReceivefromFactory;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "SendtoBuyer")
                {
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.SendtoBuyer;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "Feedbackfrombuyer")
                {
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.Feedbackfrombuyer;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "OrderConfirmation")
                {
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.OrderConfirmation;
                }
                else if (oDevelopmentRecap.ActionTypeExtra == "Cancel")
                {
                    oDevelopmentRecap.ActionType = EnumDevelopmentRecapActionType.Cancel;
                }

                oDevelopmentRecap.Note = oDevelopmentRecap.Note;
                oDevelopmentRecap.OperationBy = (int)Session[SessionInfo.currentUserID];
                oDevelopmentRecap = SetDevelopmnentRecap(oDevelopmentRecap);

                if (oDevelopmentRecap.ActionTypeExtra == "RequestForApproved") // for SEt Approval Request Value
                {
                    oDevelopmentRecap.ApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    oDevelopmentRecap.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.DevelopmentRecap;

                }
                else
                {
                    oDevelopmentRecap.ApprovalRequest = new ApprovalRequest();
                }

             
                
                _oDevelopmentRecap = oDevelopmentRecap.ChangeContactStatus(oDevelopmentRecap.ApprovalRequest,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        # endregion

        #region Set Status
        private DevelopmentRecap SetDevelopmnentRecap(DevelopmentRecap oDevelopmentRecap)//Set EnumOrderStatus Value
        {
            switch (oDevelopmentRecap.StatusExtra)
            {

                case 1:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.Initialize;
                        break;
                    }
                case 2:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.RequestForApproved;
                        break;
                    }
                case 3:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.ApprovedDone;
                        break;
                    }
                case 4:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.Inproduction;
                        break;
                    }
                case 5:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.RawMaterialCollectionDone;
                        break;

                    }
                case 6:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.QCDone;
                        break;
                    }

                case 7:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.ReceivefromFactory;
                        break;
                    }
                case 8:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.SendtoBuyer;
                        break;
                    }
                case 9:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.Feedbackfrombuyer;
                        break;


                    }
                case 10:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.OrderConfirmation;
                        break;


                    }
                case 11:
                    {
                        oDevelopmentRecap.DevelopmentStatus = EnumDevelopmentStatus.Cancel;
                        break;
                    }

            }
            oDevelopmentRecap.Note = oDevelopmentRecap.Note;
            oDevelopmentRecap.OperationBy = oDevelopmentRecap.OperationBy;
            return oDevelopmentRecap;
        }
        #endregion

        #region Waiting Search
        [HttpGet]
        public JsonResult WaitingSearch(int nStatusExtra)
        {
            _oDevelopmentRecaps = new List<DevelopmentRecap>();
            string sSQL = "SELECT * FROM View_DevelopmentRecap WHERE DevelopmentStatus = " + (int)EnumDevelopmentStatus.RequestForApproved + " AND DevelopmentRecapID IN (SELECT OperationObjectID FROM ApprovalRequest WHERE OperationType = " + (int)EnumApprovalRequestOperationType.DevelopmentRecap + " AND RequestTo = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            try
            {
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {
                    sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                }
                _oDevelopmentRecaps = DevelopmentRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Development RecapHistory Search
        [HttpGet]
        public JsonResult DevelopmentRecapHistory(int nDevelopmentRecapID, int nCurrentStatus)
        {

            try
            {
                _oDevelopmentRecap.DevelopmentRecapHistorys = ESimSol.BusinessObjects.DevelopmentRecapHistory.GetsDevelopmentRecapHistotry(nDevelopmentRecapID, nCurrentStatus, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Printing
        public ActionResult PrintDevelopmentRecaps(string sParam)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            // _oDevelopmentRecap.

            Company oCompany = new Company();
            string sSql = "SELECT * FROM View_DevelopmentRecap WHERE DevelopmentRecapID IN (" + sParam + ")";

            _oDevelopmentRecap.DevelopmentRecapList = DevelopmentRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            // _oDevelopmentRecap.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oDevelopmentRecap.DevelopmentRecapList.Count > 0)
            {

                rptDevelopmentRecaps oReport = new rptDevelopmentRecaps();
                byte[] abytes = oReport.PrepareReport(_oDevelopmentRecap, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            
            //_oDevelopmentRecaps = new List<DevelopmentRecap>();
            //_oDevelopmentRecaps = DevelopmentRecap.Gets((int)Session[SessionInfo.currentUserID]);
            //Company oCompany = new Company();
            //  oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            // ompany.CompanyLogo = GetCompanyLogo(oCompany);

            //rptDevelopmentRecaps oReport = new rptDevelopmentRecaps();
            //byte[] abytes = oReport.PrepareReport(_oDevelopmentRecaps, oCompany);
            //return File(abytes, "application/pdf");
        }

        public ActionResult PrintDevelopmentRecap(int id)
        {
            _oDevelopmentRecap = new DevelopmentRecap();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oDevelopmentRecap = _oDevelopmentRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oDevelopmentRecap.StyleCoverImage = GetThumImageForPrint(_oDevelopmentRecap.TechnicalSheetID);
            _oDevelopmentRecap.DevelopmentYarnOptions = DevelopmentYarnOption.Gets_Report(id, (int)Session[SessionInfo.currentUserID]);
            _oDevelopmentRecap.DevelopmentRecapDetails = DevelopmentRecapDetail.Gets(_oDevelopmentRecap.DevelopmentRecapID, (int)Session[SessionInfo.currentUserID]);
            string sSql = "SELECT * FROM View_DevelopmentRecapSizeColorRatio Where DevelopmentRecapDetailID IN (Select DevelopmentRecapDetailID from DevelopmentRecapDetail where DevelopmentRecapID=" + _oDevelopmentRecap.DevelopmentRecapID + ") Order by DevelopmentRecapDetailID";
            _oDevelopmentRecap.DevelopmentRecapSizeColorRatios = DevelopmentRecapSizeColorRatio.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            foreach (DevelopmentRecapDetail oItem in _oDevelopmentRecap.DevelopmentRecapDetails)
            {
                oItem.SizeCategorys = GetDistinctDevelopmentRecapSizes(oItem.DevelopmentRecapDetailID, _oDevelopmentRecap.DevelopmentRecapSizeColorRatios);
                oItem.ColorCategorys = GetDistinctDevelopmentRecapColors(oItem.DevelopmentRecapDetailID, _oDevelopmentRecap.DevelopmentRecapSizeColorRatios);
            }
            _oDevelopmentRecap.BusinessUnit = oBusinessUnit.Get(_oDevelopmentRecap.BUID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            rptDevelopmentRecap oReport = new rptDevelopmentRecap();
            byte[] abytes = oReport.PrepareReport(_oDevelopmentRecap, oCompany);
            return File(abytes, "application/pdf");
        }


        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region Search Style OR Buyer by Press Enter
        [HttpGet]
        public JsonResult SearchStyleAndBuyer(string sTempData, bool bIsStyle,int buid,  double ts)
        {
            _oDevelopmentRecaps = new List<DevelopmentRecap>();
            string sSQL = "";
            if (bIsStyle == true)
            {
                sSQL = "SELECT * FROM View_DevelopmentRecap WHERE StyleNo LIKE ('%" + sTempData + "%')";
            }
            else
            {
                sSQL = "SELECT * FROM View_DevelopmentRecap WHERE BuyerName LIKE ('%" + sTempData + "%')";
            }
            if(buid>0)
            {
                sSQL += " AND BUID = " + buid;
            }
            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {

                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            try
            {
                DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecaps = DevelopmentRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = ex.Message;
                _oDevelopmentRecaps.Add(_oDevelopmentRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Product BU, User and Name wise
        [HttpPost]
        public JsonResult GetFinishGoods(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.DevelopmentRecap, EnumProductUsages.FinishGoods, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.DevelopmentRecap, EnumProductUsages.FinishGoods, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetYarnCategory(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.DevelopmentRecap, EnumProductUsages.RawMaterial, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.DevelopmentRecap, EnumProductUsages.RawMaterial, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetPocketLinkFabric(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.DevelopmentRecap, EnumProductUsages.PocketLinkFabric, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.DevelopmentRecap, EnumProductUsages.PocketLinkFabric, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
    }
}
