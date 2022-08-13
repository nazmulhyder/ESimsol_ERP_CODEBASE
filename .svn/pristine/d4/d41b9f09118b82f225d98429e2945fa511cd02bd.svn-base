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
using OfficeOpenXml;
using OfficeOpenXml.Style;

using word = Microsoft.Office.Interop.Word;


namespace ESimSolFinancial.Controllers
{
    public class SalesQuotationController : Controller
    {
        #region Declaration
        SalesQuotation _oSalesQuotation = new SalesQuotation();
        SalesQuotationDetail _oSalesQuotationDetail = new SalesQuotationDetail();
        List<SalesQuotation> _oSalesQuotations = new List<SalesQuotation>();
        List<SalesQuotationDetail> _oSalesQuotationDetails = new List<SalesQuotationDetail>();
        SalesQuotationImage _oSalesQuotationImage = new SalesQuotationImage();
       // SalesQuotationImage _oSalesQuotationImage = new SalesQuotationImage();
        #endregion

        #region Functions

        #endregion

        #region Actions
        public ActionResult ViewSalesQuotationList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SalesQuotation).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oSalesQuotations = new List<SalesQuotation>();
            string sSQL = "SELECT * FROM View_SalesQuotation AS HH WHERE HH.BUID = " + buid.ToString() + " AND ISNULL(HH.ApproveBy,0) = 0 AND CONVERT(DATE,CONVERT(VARCHAR(12), HH.QuotationDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12), DATEADD(DAY, -60,  GETDATE()),106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), GETDATE(),106)) ORDER BY HH.SalesQuotationID ASC";
            _oSalesQuotations = SalesQuotation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.SalesStatusList = EnumObject.jGets(typeof(EnumSalesStatus));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oSalesQuotations);
        }

        public ActionResult ViewSalesQuotation(int id, int buid)
        {
            _oSalesQuotation = new SalesQuotation();
            List<PartyWiseBank> oPartyWiseBanks=new List<PartyWiseBank>();
            if (id > 0)
            {
                _oSalesQuotation = _oSalesQuotation.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSalesQuotation.SalesQuotationDetails = SalesQuotationDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                string sSQL = "SELECT top 1  Acceptance,OfferValidity,OrderSpecifications,VehicleInspection,CancelOrChangeOrder,PaymentMode,DeliveryDescription,PriceFluctuationClause,CustomsClearance,VehicleSpecification,Insurance,ForceMajeure,FuelQuality,SpecialInstruction,WarrantyTerms,SalesStatusRemarks   FROM SalesQuotation Order By SalesQuotationID DESC";
                _oSalesQuotations = new List<SalesQuotation>();
                _oSalesQuotations = SalesQuotation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oSalesQuotations.Count > 0)
                {
                    _oSalesQuotation = _oSalesQuotations[0];
                }
            }
            if (_oSalesQuotation.BuyerID>0)
                oPartyWiseBanks = PartyWiseBank.GetsByContractor(_oSalesQuotation.BuyerID, (int)Session[SessionInfo.currentUserID]);

            ViewBag.PartyWiseBanks = oPartyWiseBanks;

            ViewBag.BaseCurrencyID = (int)Session[SessionInfo.BaseCurrencyID];
            ViewBag.QuotationTypes = EnumObject.jGets(typeof(EnumQuotationType));
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.MarketingAccounts = MarketingAccount.GetsByBU(buid,(int)Session[SessionInfo.currentUserID]);
            return View(_oSalesQuotation);
        }

        [HttpPost]
        public JsonResult Save(SalesQuotation oSalesQuotation)
        {
            _oSalesQuotation = new SalesQuotation();
            try
            {
                _oSalesQuotation = oSalesQuotation;
                _oSalesQuotation = _oSalesQuotation.Save((int)Session[SessionInfo.currentUserID]);

                if (oSalesQuotation.SalesQuotationID == 0)
                {
                    _oSalesQuotation.SalesStatus = (int)EnumSalesStatus.Open;
                    _oSalesQuotation = _oSalesQuotation.UpdateStatus((int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStatus(SalesQuotation oSalesQuotation)
        {
            _oSalesQuotation = new SalesQuotation();
            try
            {
                _oSalesQuotation = oSalesQuotation;
                _oSalesQuotation = _oSalesQuotation.UpdateStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = ex.Message;
            }

            if (_oSalesQuotation.SalesQuotationID <= 0 && string.IsNullOrEmpty(_oSalesQuotation.ErrorMessage))
                _oSalesQuotation.ErrorMessage = "Invalid Operation";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(SalesQuotation oSalesQuotation)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSalesQuotation.Delete(oSalesQuotation.SalesQuotationID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approve(SalesQuotation oSalesQuotation)
        {
            _oSalesQuotation = new SalesQuotation();
            try
            {
                _oSalesQuotation = oSalesQuotation.Approve((int)Session[SessionInfo.currentUserID]);

                //if (oSalesQuotation.SalesQuotationID == 0)
                //{
                //    _oSalesQuotation.SalesStatus = (int)EnumSalesStatus.Open;
                //    _oSalesQuotation = _oSalesQuotation.UpdateStatus((int)Session[SessionInfo.currentUserID]);
                //}
            }
            catch (Exception ex)
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UndoApprove(SalesQuotation oSalesQuotation)
        {
            _oSalesQuotation = new SalesQuotation();
            try
            {
                _oSalesQuotation = oSalesQuotation.UndoApprove((int)Session[SessionInfo.currentUserID]);

                //if (oSalesQuotation.SalesQuotationID == 0)
                //{
                //    _oSalesQuotation.SalesStatus = (int)EnumSalesStatus.Open;
                //    _oSalesQuotation = _oSalesQuotation.UpdateStatus((int)Session[SessionInfo.currentUserID]);
                //}
            }
            catch (Exception ex)
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Revise(SalesQuotation oSalesQuotation)
        {
            _oSalesQuotation = new SalesQuotation();
            try
            {
                _oSalesQuotation = oSalesQuotation;
                _oSalesQuotation = _oSalesQuotation.Revise((int)Session[SessionInfo.currentUserID]);

                //if (oSalesQuotation.SalesQuotationID == 0)
                //{
                //    _oSalesQuotation.SalesStatus = (int)EnumSalesStatus.Open;
                //    _oSalesQuotation = _oSalesQuotation.UpdateStatus((int)Session[SessionInfo.currentUserID]);
                //}
            }
            catch (Exception ex)
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFeaturesByName(Feature oFeature)
        {
            List<Feature> oFeatures = new List<Feature>();
            try
            {
                string sSQL = "SELECT * FROM View_Feature WHERE FeatureName LIKE ('%" + oFeature.FeatureName + "%')";
                oFeatures = Feature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFeature = new Feature();
                oFeature.ErrorMessage = ex.Message;
                oFeatures.Add(oFeature);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFeatures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalesQuotationDetailsByKommFileID(KommFile oKommFile)
        {
            List<KommFileDetail> oKommFileDetails = new List<KommFileDetail>();
            _oSalesQuotationDetails = new List<SalesQuotationDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_KommFileDetail WHERE KommFileID=" + oKommFile.KommFileID;
                oKommFileDetails = KommFileDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(KommFileDetail oItem in oKommFileDetails)
                {
                    _oSalesQuotationDetail = new SalesQuotationDetail();
                    _oSalesQuotationDetail.FeatureID = oItem.FeatureID;
                    _oSalesQuotationDetail.Price = oItem.Price;
                    _oSalesQuotationDetail.CurrencyID = oItem.CurrencyID;
                    _oSalesQuotationDetail.CurrencyName = oItem.CurrencyName;
                    _oSalesQuotationDetail.CurrencySymbol = oItem.CurrencySymbol;
                    _oSalesQuotationDetail.FeatureCode = oItem.FeatureCode;
                    _oSalesQuotationDetail.FeatureName = oItem.FeatureName;
                    _oSalesQuotationDetail.Remarks = oItem.Remarks;
                    _oSalesQuotationDetail.FeatureType = oItem.FeatureType;
                    _oSalesQuotationDetails.Add(_oSalesQuotationDetail);
                }
            }
            catch (Exception ex)
            {
                _oSalesQuotationDetail = new SalesQuotationDetail();
                _oSalesQuotationDetail.ErrorMessage = ex.Message;
                _oSalesQuotationDetails.Add(_oSalesQuotationDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesQuotationDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsSalesQuotationByCustomer(SalesQuotation oSalesQuotation)
        {
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();
            try
            {
                string sSQL = "SELECT * FROM View_SalesQuotation WHERE BuyerID=" + oSalesQuotation.BuyerID + " AND ISNULL(ApproveBy,0)!=0 ";
                oSalesQuotations = SalesQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = ex.Message;
                oSalesQuotations.Add(_oSalesQuotation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ADV SEARCH

        [HttpPost]
        public JsonResult AdvSearch(SalesQuotation oSalesQuotation)
        {
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();
            SalesQuotation _oSalesQuotation = new SalesQuotation();
            string sSQL = MakeSQL(oSalesQuotation);
            if (sSQL == "Error")
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = "Please select a searching critaria.";
                oSalesQuotations = new List<SalesQuotation>();
            }
            else
            {
                oSalesQuotations = new List<SalesQuotation>();
                oSalesQuotations = SalesQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oSalesQuotations.Count == 0)
                {
                    oSalesQuotations = new List<SalesQuotation>();
                }
            }
            var jsonResult = Json(oSalesQuotations, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(SalesQuotation oSalesQuotation)
        {
            string sParams = oSalesQuotation.Params;

            int nDateCriteria_QuotationDate = 0,
                nStatus = 0; ;

            string sKommNo = "",
                   sModelIDs = "",
                   sBuyerIDs = "";

            DateTime dStart_QuotationDate = DateTime.Today,
                     dEnd_QuotationDate = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sKommNo = sParams.Split('~')[nCount++];
                nStatus = Convert.ToInt32(sParams.Split('~')[nCount++]);
                nDateCriteria_QuotationDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_QuotationDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_QuotationDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sModelIDs = sParams.Split('~')[nCount++];
                sBuyerIDs = sParams.Split('~')[nCount++];
            }

            string sReturn1 = "SELECT * FROM View_SalesQuotation AS EB";
            string sReturn = "";

            #region nStatus
            if (nStatus>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.SalesStatus =" + nStatus ;
            }
            #endregion

            #region KommNo
            if (!string.IsNullOrEmpty(sKommNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.KommNo LIKE '%" + sKommNo + "%'";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.QuotationDate", nDateCriteria_QuotationDate, dStart_QuotationDate, dEnd_QuotationDate);
            #endregion

            #region Model IDs
            if (!string.IsNullOrEmpty(sModelIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.VehicleModelID IN (" + sModelIDs + ") ";
            }
            #endregion

            #region Buyer IDs
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BuyerID IN (" + sBuyerIDs + ") ";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region SQ Log
        [HttpPost]
        public JsonResult GetSalesQuotationLogs(SalesQuotation oSalesQuotation)
        {
            _oSalesQuotations = new List<SalesQuotation>();
            try
            {
                string sSQL = "SELECT * FROM View_SalesQuotationLog WHERE SalesQuotationID=" + oSalesQuotation.SalesQuotationID;
                _oSalesQuotations = SalesQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSalesQuotation = new SalesQuotation();
                _oSalesQuotation.ErrorMessage = ex.Message;
                _oSalesQuotations.Add(_oSalesQuotation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintSQ_Log(int logid)
        {
            List<VehicleOrderImage> oVehicleOrderImages = new List<VehicleOrderImage>();
            List<SalesQuotationImage> oSalesQuotationImages = new List<SalesQuotationImage>();
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            List<KommFileImage> oKommFileImages = new List<KommFileImage>();
            KommFileImage oKommFileImage = new KommFileImage();

            rptSalesQuotation_F2 oReport = new rptSalesQuotation_F2();

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

            SalesQuotation oSalesQuotation = new SalesQuotation();
            Contractor oContractor = new Contractor();

            oSalesQuotation = oSalesQuotation.GetLog(logid, (int)Session[SessionInfo.currentUserID]);
            oSalesQuotation.SalesQuotationDetails = SalesQuotationDetail.Gets("SELECT * FROM View_SalesQuotationDetailLog WHERE SalesQuotationID=" + oSalesQuotation.SalesQuotationLogID, (int)Session[SessionInfo.currentUserID]);
            
            oSalesQuotation.Buyer = oContractor.Get(oSalesQuotation.BuyerID, (int)Session[SessionInfo.currentUserID]);

            //Get Signature
            AttachDocument oAttachDocument = new AttachDocument();
            MarketingAccount oMarketingAccount = new MarketingAccount();
            oMarketingAccount = oMarketingAccount.Get(oSalesQuotation.MarketingPerson, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oAttachDocument = oAttachDocument.GetUserSignature(oMarketingAccount.UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oSalesQuotation.Signature = GetImage(oAttachDocument.AttachFile);

            #region GET Images
            //if (oSalesQuotation.QuotationType == EnumQuotationType.Stock_Item)
            //{
            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.ExteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);

            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.ExteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);

            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.InteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);

            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.InteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);
            //}
            //else if (oSalesQuotation.QuotationType == EnumQuotationType.New_Item)
            //{
            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetLogImageByType(oSalesQuotation.SalesQuotationLogID, (int)EnumImageType.ExteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);

            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetLogImageByType(oSalesQuotation.SalesQuotationLogID, (int)EnumImageType.ExteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);

            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetLogImageByType(oSalesQuotation.SalesQuotationLogID, (int)EnumImageType.InteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);

            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetLogImageByType(oSalesQuotation.SalesQuotationLogID, (int)EnumImageType.InteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);
            //}

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.ExteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.ExteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.InteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.InteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);
            #endregion

            List<ModelFeature> oModelFeatures = new List<ModelFeature>();
            oModelFeatures = ModelFeature.Gets(oSalesQuotation.VehicleModelID, (int)Session[SessionInfo.currentUserID]);
            List<Object> oObjects = new List<object>();
            //oObjects = oReport.PrepareReport(oCompany, oSalesQuotation, oVehicleOrderImages, oSalesQuotationImages, oModelFeatures, 11);
            oObjects = oReport.PrepareReport(oCompany, oSalesQuotation, oKommFileImages, oModelFeatures, 11);
            int nnn = (int)oObjects[1];
            //oObjects = oReport.PrepareReport(oCompany, oSalesQuotation, oVehicleOrderImages, oSalesQuotationImages, oModelFeatures, nnn);

            byte[] abytes = (byte[])oObjects[0];
            return File(abytes, "application/pdf");
        }

        #endregion

        #region PrintList
        //public ActionResult PrintList(string sIDs)
        //{
        //    _oSalesQuotation = new SalesQuotation();
        //    string sSQL = "SELECT * FROM View_SalesQuotation WHERE SalesQuotationID IN (" + sIDs + ") ORDER BY SalesQuotationID  ASC";
        //    _oSalesQuotation.SalesQuotationList = SalesQuotation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    rptSalesQuotationList oReport = new rptSalesQuotationList();
        //    byte[] abytes = oReport.PrepareReport(_oSalesQuotation, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        #region PRINT SQ TEST
        public ActionResult PrintSQ(int id)
        {
            List<VehicleOrderImage> oVehicleOrderImages = new List<VehicleOrderImage>();
            List<SalesQuotationImage> oSalesQuotationImages = new List<SalesQuotationImage>();
            List<KommFileImage> oKommFileImages = new List<KommFileImage>();
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            KommFileImage oKommFileImage = new KommFileImage();

            rptSalesQuotation_F2 oReport = new rptSalesQuotation_F2();

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

            SalesQuotation oSalesQuotation = new SalesQuotation();
            Contractor oContractor = new Contractor();

            oSalesQuotation = oSalesQuotation.Get(id, (int)Session[SessionInfo.currentUserID]);
            oSalesQuotation.SalesQuotationDetails = SalesQuotationDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oSalesQuotation.Buyer = oContractor.Get(oSalesQuotation.BuyerID, (int)Session[SessionInfo.currentUserID]);
            
            //Get Signature
            AttachDocument oAttachDocument = new AttachDocument();
            MarketingAccount oMarketingAccount = new MarketingAccount();
            oMarketingAccount = oMarketingAccount.Get(oSalesQuotation.MarketingPerson, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oAttachDocument = oAttachDocument.GetUserSignature(oMarketingAccount.UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oSalesQuotation.Signature = GetImage(oAttachDocument.AttachFile);

            //if (oSalesQuotation.QuotationType == EnumQuotationType.Stock_Item)
            //{
            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.ExteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);

            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.ExteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);

            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.InteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);

            //    oVehicleOrderImage = new VehicleOrderImage();
            //    oVehicleOrderImage = oVehicleOrderImage.GetImageByType(oSalesQuotation.VehicleOrderID, (int)EnumImageType.InteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oVehicleOrderImage.TSImage = GetImage(oVehicleOrderImage.LargeImage);
            //    oVehicleOrderImages.Add(oVehicleOrderImage);
            //}
            //else if (oSalesQuotation.QuotationType == EnumQuotationType.New_Item)
            //{
            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetImageByType(oSalesQuotation.SalesQuotationID, (int)EnumImageType.ExteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);

            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetImageByType(oSalesQuotation.SalesQuotationID, (int)EnumImageType.ExteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);

            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetImageByType(oSalesQuotation.SalesQuotationID, (int)EnumImageType.InteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);

            //    oSalesQuotationImage = new SalesQuotationImage();
            //    oSalesQuotationImage = oSalesQuotationImage.GetImageByType(oSalesQuotation.SalesQuotationID, (int)EnumImageType.InteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            //    oSalesQuotationImage.TSImage = GetImage(oSalesQuotationImage.LargeImage);
            //    oSalesQuotationImages.Add(oSalesQuotationImage);
            //}

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.ExteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.ExteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.InteriorFrontImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);

            oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(oSalesQuotation.KommFileID, (int)EnumImageType.InteriorBackImage, (int)Session[SessionInfo.currentUserID]);
            oKommFileImage.TSImage = GetImage(oKommFileImage.LargeImage);
            oKommFileImages.Add(oKommFileImage);

            List<ModelFeature> oModelFeatures = new List<ModelFeature>();
            oModelFeatures = ModelFeature.Gets(oSalesQuotation.VehicleModelID, (int)Session[SessionInfo.currentUserID]);
            List<Object>  oObjects =  new List<object>();
            oObjects = oReport.PrepareReport(oCompany, oSalesQuotation, oKommFileImages, oModelFeatures, 11);
            int nnn = (int)oObjects[1];
            //oObjects = oReport.PrepareReport(oCompany, oSalesQuotation, oVehicleOrderImages, oSalesQuotationImages, oModelFeatures, nnn);

            byte[] abytes = (byte[])oObjects[0];
            return File(abytes, "application/pdf");
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
        #endregion

        #region PRINT BQ
        public ActionResult PrintBQ(int id, int PWBID, double PrintOTRAmount, string PrintDate)
        {
            rptSalesBankQuotation oReport = new rptSalesBankQuotation();
            //SaveFileDialog sfd = new SaveFileDialog();
            Company oCompany = new Company();
            PartyWiseBank oPartyWiseBank = new PartyWiseBank();
            SalesQuotation oSalesQuotation = new SalesQuotation();

            try 
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);
                oPartyWiseBank = oPartyWiseBank.Get(PWBID, (int)Session[SessionInfo.currentUserID]);
                oSalesQuotation = oSalesQuotation.Get(id, (int)Session[SessionInfo.currentUserID]);

                if (oSalesQuotation.PartyWiseBankID != PWBID || oSalesQuotation.PrintOTRAmount != PrintOTRAmount) 
                {
                    oSalesQuotation.PartyWiseBankID = PWBID;
                    oSalesQuotation.PrintOTRAmount = PrintOTRAmount;
                    string UpdatedText = oSalesQuotation.UpdateBQ((int)Session[SessionInfo.currentUserID]);
                    if (!UpdatedText.Equals("Updated"))
                        throw new Exception(UpdatedText);
                }
            }
            catch(Exception e)
            {
                rptErrorMessage oReport_E = new rptErrorMessage();
                byte[] abytes_E = oReport_E.PrepareReport(e.Message);
                return File(abytes_E, "application/pdf"); //"BQ-"+oSalesQuotation.RefNo+".pdf"
            }

            byte[] abytes = oReport.PrepareReport(oSalesQuotation,oPartyWiseBank, PrintDate,  oCompany);
            return File(abytes, "application/pdf"); //"BQ-"+oSalesQuotation.RefNo+".pdf"
        }
        #endregion

        #endregion

        #region Image Part
        public ViewResult ImageHelper(int id, string ms)
        {
            _oSalesQuotationImage = new SalesQuotationImage();
            _oSalesQuotationImage.SalesQuotationID = id;
            List<SalesQuotationThumbnail> oSalesQuotationThumbnails = new List<SalesQuotationThumbnail>();
            oSalesQuotationThumbnails = SalesQuotationThumbnail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            _oSalesQuotationImage.SalesQuotationThumbnails = oSalesQuotationThumbnails;
            _oSalesQuotationImage.ImageTypeObjs = EnumObject.jGets(typeof(EnumImageType));
            TempData["message"] = ms;
            return View(_oSalesQuotationImage);
        }

        [HttpPost]
        public ActionResult ImageHelper(HttpPostedFileBase file, SalesQuotationImage oSalesQuotationImage)
        {

            // Verify that the user selected a file
            string sErrorMessage = "";
            if (file != null && file.ContentLength > 0)
            {
                Image oImage = Image.FromStream(file.InputStream, true, true);
                //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                #region Get Thumbnail image
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image myThumbnail = oImage.GetThumbnailImage(100, 100, myCallback, IntPtr.Zero);
                //myThumbnail.Save(@"F:\images\thum" + file.FileName + ".jpg");
                #endregion

                //Orginal Image to byte array
                byte[] aImageInByteArray = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    oImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    aImageInByteArray = ms.ToArray();
                }

                //Thumbnail Image to byte array
                byte[] aThumbnailImageInByteArray = null;
                using (MemoryStream Thumbnailms = new MemoryStream())
                {
                    myThumbnail.Save(Thumbnailms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    aThumbnailImageInByteArray = Thumbnailms.ToArray();
                }
                double nMaxLength = 1000 * 1024;// File Size upto 1 MB
                if (aImageInByteArray.Length > nMaxLength)
                {
                    sErrorMessage = "You can selecte maximum 1MB image";
                }
                else if (oSalesQuotationImage.ImageTitle == null || oSalesQuotationImage.ImageTitle == "")
                {
                    sErrorMessage = "Please enter image title!";
                }
                else
                {
                    oSalesQuotationImage.LargeImage = aImageInByteArray;
                    oSalesQuotationImage.SalesQuotationThumbnailID = 0;
                    oSalesQuotationImage.ThumbnailImage = aThumbnailImageInByteArray;
                    oSalesQuotationImage = oSalesQuotationImage.Save((int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                sErrorMessage = "Please select an image!";
            }
            return RedirectToAction("ImageHelper", new { id = oSalesQuotationImage.SalesQuotationID, ms = sErrorMessage });
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        [HttpGet]
        public JsonResult DeleteSalesQuotationImage(int id)
        {
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
            oSalesQuotationImage = oSalesQuotationImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            string sErrorMease = "";
            int nId = 0;
            nId = oSalesQuotationImage.SalesQuotationID;
            try
            {
                oSalesQuotationImage.Delete(id, (int)Session[SessionInfo.currentUserID]);
                oSalesQuotationThumbnail.Delete(id, (int)Session[SessionInfo.currentUserID]);
                sErrorMease = "Delete Successfully";
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public Image GetThumImage(int id)
        {
            SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
            oSalesQuotationThumbnail = oSalesQuotationThumbnail.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oSalesQuotationThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oSalesQuotationThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public Image GetImage(byte[] oImage)
        {
            if (oImage != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetLargeImage(int id, int SQImageid)//id is VehicleOrderImageID
        {
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            if (id> 0)
            {
                oVehicleOrderImage = oVehicleOrderImage.GetImageByType(id, (int)EnumImageType.ModelImage, (int)Session[SessionInfo.currentUserID]);
                if (oVehicleOrderImage.LargeImage != null)
                {
                    MemoryStream m = new MemoryStream(oVehicleOrderImage.LargeImage);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                    img.Save(Response.OutputStream, ImageFormat.Jpeg);
                    return img;
                }
                else
                {
                    return null;
                }
            }
            else if (SQImageid>0)
            {
                oSalesQuotationImage = oSalesQuotationImage.GetFrontImage(SQImageid, (int)Session[SessionInfo.currentUserID]);
                if (oSalesQuotationImage.LargeImage != null)
                {
                    MemoryStream m = new MemoryStream(oSalesQuotationImage.LargeImage);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                    img.Save(Response.OutputStream, ImageFormat.Jpeg);
                    return img;
                }
                else
                {
                    return null;
                }
            }else
            {
                return null;
            }
        }
        #endregion
    }

}
