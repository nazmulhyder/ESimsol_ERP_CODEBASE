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

namespace ESimSolFinancial.Controllers
{
    public class ImportPIController : Controller
    {
        #region
        ImportPI _oImportPI = new ImportPI();
        List<ImportPI> _oImportPIs = new List<ImportPI>();
        List<ImportPIDetail> _oImportPIDetails = new List<ImportPIDetail>();
        List<Product> _oProducts = new List<Product>();
        Product _oProduct = new Product();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        private string sReturn = "";
        string _sErrorMessage = "";
        #region View ImportPI Collection For LC
        public ActionResult ViewImportPIs(int nPIEntryType, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportPI).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oImportPIs = new List<ImportPI>();
            //_oImportPIs = ImportPI.Gets("SELECT * FROM View_ImportPI WHERE BUID=" + buid.ToString() + " AND ImportPIStatus IN (" + ((int)EnumImportPIState.Initialized).ToString() + ") ", (int)Session[SessionInfo.currentUserID]);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumImportPIState));
            ViewBag.BUID = buid;
            ViewBag.PIEntryType = nPIEntryType;//0: open OR 1:Ref type
            string sSQL = "SELECT * FROM View_User WHERE UserID IN (SELECT distinct ISNULL(ApprovedBy,0) FROM ImportPI WHERE ImportPIType in (" + (int)EnumImportPIType.Foreign + "," + (int)EnumImportPIType.Local + "," + (int)EnumImportPIType.NonLC + "," + (int)EnumImportPIType.TTForeign + ") and  BUID = " + buid + " AND ISNULL(ApprovedBy,0)!=0) ";
            ViewBag.ApprovedByUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]);
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.ImportPITypeObj = EnumObject.jGets(typeof(EnumImportPIType)).Where(x => x.id == (int)EnumImportPIType.Foreign || x.id == (int)EnumImportPIType.Local || x.id == (int)EnumImportPIType.NonLC || x.id == (int)EnumImportPIType.TTForeign || x.id == (int)EnumImportPIType.TTLocal);
            return View(_oImportPIs);
        }
        public ActionResult ViewImportPI(int id, int buid)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportPI).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            ImportSetup oImportSetup = new ImportSetup();
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            _oClientOperationSetting = new ClientOperationSetting();
            oImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI = new ImportPI();
            if (id > 0)
            {
                _oImportPI = _oImportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIDetails = ImportPIDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);                
            }
            oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            oLCTerms = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTerms = oLCTerms;
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.PCReferenceTypes = EnumObject.jGets(typeof(EnumPCReferenceType));
         
            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            ViewBag.Currencys = oCurrencys;
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.ImportSetup = oImportSetup;
            ViewBag.ConcernPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI.ImportPITypeInt = (int)_oImportPI.ImportPIType;
            ViewBag.ShipmentTerms = EnumObject.jGets(typeof(EnumShipmentTerms));

            List<EnumObject> oPITypeObjs = new List<EnumObject>();
            List<EnumObject> oPITypes = new List<EnumObject>();
            oPITypeObjs = EnumObject.jGets(typeof(EnumImportPIType));
            foreach (EnumObject oItem in oPITypeObjs)
            {
                if (oItem.id == (int)EnumImportPIType.Foreign || oItem.id == (int)EnumImportPIType.Local || oItem.id == (int)EnumImportPIType.NonLC || oItem.id == (int)EnumImportPIType.TTForeign || oItem.id == (int)EnumImportPIType.TTLocal)
                {
                    oPITypes.Add(oItem);
                }

            }
            ViewBag.ImportPITypeObj = oPITypes;// EnumObject.jGets(typeof(EnumImportPIType));
            return View(_oImportPI);
        }

        public ActionResult ViewImportPIReference(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            ImportSetup oImportSetup = new ImportSetup();
            Company oCompany = new Company();
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            _oClientOperationSetting = new ClientOperationSetting();
            oImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI = new ImportPI();
            if (id > 0)
            {
                _oImportPI = _oImportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIDetails = ImportPIDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIReferenceList = ImportPIReference.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            oLCTerms = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTerms = oLCTerms;
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.PCReferenceTypes = EnumObject.jGets(typeof(EnumPCReferenceType));

            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            ViewBag.Currencys = oCurrencys;
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.ImportSetup = oImportSetup;
            ViewBag.ConcernPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI.ImportPITypeInt = (int)_oImportPI.ImportPIType;
            ViewBag.ShipmentTerms = EnumObject.jGets(typeof(EnumShipmentTerms));

            List<EnumObject> oPITypeObjs = new List<EnumObject>();
            List<EnumObject> oPITypes = new List<EnumObject>();
            oPITypeObjs = EnumObject.jGets(typeof(EnumImportPIType));
            foreach (EnumObject oItem in oPITypeObjs)
            {
                if (oItem.id == (int)EnumImportPIType.Foreign || oItem.id == (int)EnumImportPIType.Local || oItem.id == (int)EnumImportPIType.NonLC || oItem.id == (int)EnumImportPIType.TTForeign || oItem.id == (int)EnumImportPIType.TTLocal)
                {
                    oPITypes.Add(oItem);
                }

            }
            ViewBag.ImportPITypeObj = oPITypes;// EnumObject.jGets(typeof(EnumImportPIType));
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumImportPIRefType));
            ViewBag.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oImportPI);
        }
        public ActionResult ViewImportPIReferenceRevise(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            ImportSetup oImportSetup = new ImportSetup();
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            _oClientOperationSetting = new ClientOperationSetting();
            oImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            _oImportPI = new ImportPI();
            if (id > 0)
            {
                _oImportPI = _oImportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIDetails = ImportPIDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIReferenceList = ImportPIReference.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            oLCTerms = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTerms = oLCTerms;
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.PCReferenceTypes = EnumObject.jGets(typeof(EnumPCReferenceType));

            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            ViewBag.Currencys = oCurrencys;
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.ImportSetup = oImportSetup;
            ViewBag.ConcernPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI.ImportPITypeInt = (int)_oImportPI.ImportPIType;
            ViewBag.ShipmentTerms = EnumObject.jGets(typeof(EnumShipmentTerms));

            List<EnumObject> oPITypeObjs = new List<EnumObject>();
            List<EnumObject> oPITypes = new List<EnumObject>();
            oPITypeObjs = EnumObject.jGets(typeof(EnumImportPIType));
            foreach (EnumObject oItem in oPITypeObjs)
            {
                if (oItem.id == (int)EnumImportPIType.Foreign || oItem.id == (int)EnumImportPIType.Local || oItem.id == (int)EnumImportPIType.NonLC || oItem.id == (int)EnumImportPIType.TTForeign || oItem.id == (int)EnumImportPIType.TTLocal)
                {
                    oPITypes.Add(oItem);
                }

            }
            ViewBag.ImportPITypeObj = oPITypes;// EnumObject.jGets(typeof(EnumImportPIType));
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumImportPIRefType));
            ViewBag.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oImportPI);
        }

        public ActionResult ViewImportPIRevise(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            ImportSetup oImportSetup = new ImportSetup();

            _oImportPI = new ImportPI();
            if (id > 0)
            {
                _oImportPI = _oImportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIDetails = ImportPIDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oLCTerms = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTerms = oLCTerms;
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.PCReferenceTypes = EnumObject.jGets(typeof(EnumPCReferenceType));
            ViewBag.ImportPITypeObj = EnumObject.jGets(typeof(EnumImportPIType));
            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            ViewBag.ConcernPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.ImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.Currencys = oCurrencys;
            _oImportPI.ImportPITypeInt = (int)_oImportPI.ImportPIType;
            ViewBag.ShipmentTerms = EnumObject.jGets(typeof(EnumShipmentTerms));
            return View(_oImportPI);
        }

        public ActionResult ViewImportPIGRNProduct(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            _oImportPI = new ImportPI();
            if (id > 0)
            {
                _oImportPI = _oImportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIGRNDetails = ImportPIGRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oLCTerms = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTerms = oLCTerms;
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.PCReferenceTypes = EnumObject.jGets(typeof(EnumPCReferenceType));
            ViewBag.ImportPITypeObj = EnumObject.jGets(typeof(EnumImportPIType));
            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            ViewBag.Currencys = oCurrencys;
            ViewBag.ConcernPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI.ImportPITypeInt = (int)_oImportPI.ImportPIType;
            return View(_oImportPI);
        }
        public ActionResult ViewImportPI_History(int nId, double ts)
        {
            List<ImportPIHistory> oImportPIHistorys = new List<ImportPIHistory>();
            _oImportPI = _oImportPI.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportPIHistorys = ImportPIHistory.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            bool bFlag = false;
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            foreach (ImportPIHistory oItem in oImportPIHistorys)
            {
                dEndDate = oItem.DateTime;
                if (bFlag == true)
                {
                    TimeSpan n = dEndDate - dStartDate;
                    if (n.Days >= 1)
                    {
                        oItem.ErrorMsg = n.Days + " days";
                    }
                    else if (n.Hours > 0)
                    {
                        oItem.ErrorMsg = oItem.ErrorMsg + " " + n.Hours.ToString() + " hrs.";
                    }
                    else
                    {
                        oItem.ErrorMsg = "";
                    }

                }
                else
                {
                    oItem.ErrorMsg = "";
                }
                dStartDate = oItem.DateTime;
                bFlag = true;
            }
            ViewBag.ImportPIHistorys = oImportPIHistorys;
            return View(_oImportPI);
        }
    
    
     

      

        public ActionResult ViewImportPIFancys(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportPI).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oImportPIs = new List<ImportPI>();
            _oImportPIs = ImportPI.Gets("SELECT * FROM View_ImportPI WHERE ImportPIType=" + (int)EnumImportPIType.FancyYarn + " and BUID=" + buid.ToString() + " AND ImportPIStatus IN (" + ((int)EnumImportPIState.Initialized).ToString() + ") ", (int)Session[SessionInfo.currentUserID]);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumImportPIState));
            ViewBag.BUID = buid;
            string sSQL = "SELECT * FROM View_User WHERE UserID IN (SELECT distinct ISNULL(ApprovedBy,0) FROM ImportPI WHERE ImportPIType=" + (int)EnumImportPIType.FancyYarn + " and BUID = " + buid + " AND ISNULL(ApprovedBy,0)!=0) ";
            ViewBag.ApprovedByUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oImportPIs);
        }
        public ActionResult ViewImportPIFancy(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            ImportSetup oImportSetup = new ImportSetup();
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            oImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI = new ImportPI();
            if (id > 0)
            {
                _oImportPI = _oImportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIDetails = ImportPIDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            oLCTerms = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTerms = oLCTerms;
            //ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.PCReferenceTypes = EnumObject.jGets(typeof(EnumPCReferenceType));

            List<EnumObject> oImportPITypes = new List<EnumObject>();
            List<EnumObject> ImportPITypeObj = new List<EnumObject>();
            oImportPITypes = EnumObject.jGets(typeof(EnumImportPIType));

            foreach (EnumObject oItem in oImportPITypes)
            {
                if (oItem.id == (int)EnumImportPIType.FancyYarn)
                {
                    ImportPITypeObj.Add(oItem);
                }
            }
            ViewBag.ImportPITypeObj = ImportPITypeObj;
            ViewBag.Currencys = oCurrencys;
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.ImportSetup = oImportSetup;
            ViewBag.ConcernPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI.ImportPITypeInt = (int)_oImportPI.ImportPIType;
            return View(_oImportPI);
        }
        #endregion
        #region HTTP Save

        [HttpPost]
        public JsonResult Save(ImportPI oImportPI)
        {
            _oImportPI = new ImportPI();
            try
            {
                _oImportPI = oImportPI.Save((int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIDetails = ImportPIDetail.Gets(_oImportPI.ImportPIID, (int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

     

        #endregion

        //SaveImportPIGRNDetail
        #region HTTP Save
        [HttpPost]
        public JsonResult SaveImportPIGRNDetail(ImportPIGRNDetail oImportPIGRNDetail)
        {
            List<ImportPIGRNDetail> oImportPIGRNDetails = new List<ImportPIGRNDetail>();
            try
            {
                oImportPIGRNDetails = oImportPIGRNDetail.SaveImportPIGRNDetail((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                ImportPIGRNDetail oTempImportPIGRNDetail = new ImportPIGRNDetail();
                oTempImportPIGRNDetail.ErrorMessage = ex.Message;
                oImportPIGRNDetails.Add(oTempImportPIGRNDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIGRNDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAmount(ImportPI oImportPI)
        {
            _oImportPI = new ImportPI();
            try
            {
                _oImportPI = oImportPI.UpdateAmount((int)Session[SessionInfo.currentUserID]);
                _oImportPI.ImportPIDetails = ImportPIDetail.Gets("SELECT * FROM View_ImportPIDetail WHERE ImportPIID = " + oImportPI.ImportPIID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP Delete

        [HttpPost]
        public JsonResult Delete(ImportPI oImportPI)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oImportPI.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePCDetail(ImportPIDetail oImportPIDetail)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oImportPIDetail.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }        
        #endregion

        #region Get ImportPI for Individual Contractor


        [HttpPost]
        public JsonResult GetImportPI(ImportPI oImportPI)
        {
            string sErrorMease = "";
            try
            {

                string sImportPIType = "";
                _oImportPIs = new List<ImportPI>();

                oImportPI.ImportPIStatus = EnumImportPIState.Accepted;
                sImportPIType = ((int)EnumImportPIType.Foreign).ToString() + "," + ((int)EnumImportPIType.Local).ToString()+"," + ((int)EnumImportPIType.TTForeign).ToString();
                _oImportPIs = ImportPI.GetsImportPI(oImportPI.SupplierID, ((int)oImportPI.ImportPIStatus).ToString(), sImportPIType, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetsOA(OrderAcknowledgement oOrderAcknowledgement)
        //{
        //    ImportPIDetail_GRN oImportPIDetail_GRN = new ImportPIDetail_GRN();

        //    _oImportPIDetail_GRNs = new List<ImportPIDetail_GRN>();
        //    List<OrderAcknowledgement> oOrderAcknowledgements = new List<OrderAcknowledgement>();
        //    string sSQL = "";
        //    sSQL = "SELECT * FROM View_OrderAcknowledgement AS OA where OA.OAStatus=3";
        //    oOrderAcknowledgements = OrderAcknowledgement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]); 
        //    foreach(OrderAcknowledgement oItem in oOrderAcknowledgements )
        //    {
        //        oImportPIDetail_GRN = new ImportPIDetail_GRN();
        //        oImportPIDetail_GRN.OAID = oItem.OrderAcknowledgementID;
        //        oImportPIDetail_GRN.ProductID = oItem.ProductID;
        //        oImportPIDetail_GRN.ProductName = oItem.ProductName;
        //        oImportPIDetail_GRN.ProductCode = oItem.ProductCode;
        //        oImportPIDetail_GRN.Quantity = oItem.Qty;
        //        oImportPIDetail_GRN.UnitPrice = oItem.UnitPrice;
        //        oImportPIDetail_GRN.MUnitID = oItem.MUnitID;
        //        oImportPIDetail_GRN.MUName = oItem.MUnitName;
        //        oImportPIDetail_GRN.MUName = oItem.MUnitName;
        //        oImportPIDetail_GRN.OrderNo = oItem.OrderNo;
        //        oImportPIDetail_GRN.AcknowledgementNo = oItem.AcknowledgementNo;
        //        _oImportPIDetail_GRNs.Add(oImportPIDetail_GRN);
        //    }


        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize((object)_oImportPIDetail_GRNs);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Approve ImportPI
        private bool ValidateInputApproved(ImportPI oPurchaseContact)
        {

            if (oPurchaseContact.SupplierID <= 0)
            {
                _sErrorMessage = "Please entry Supplier Name.";
                return false;
            }

            if (oPurchaseContact.ImportPITypeInt == (int)EnumImportPIType.Foreign || oPurchaseContact.ImportPITypeInt == (int)EnumImportPIType.Local)
            {
             
                if (oPurchaseContact.LCTermID <= 0)
                {
                    _sErrorMessage = "Please entry LC Term.";
                    return false;
                }
            }
            return true;
        }

        [HttpPost]
        public JsonResult ApproveImportPI(ImportPI oImportPI)
        {
            _oImportPI = new ImportPI();
            try
            {
                if (this.ValidateInputApproved(oImportPI))
                {
                    oImportPI.ImportPIStatus = EnumImportPIState.Accepted;
                    oImportPI.ImportPIStatusInt = (int)EnumImportPIState.Accepted;
                    oImportPI.DateOfApproved = DateTime.Now;
                    oImportPI.ApprovedBy = (int)Session[SessionInfo.currentUserID];
                    _oImportPI = oImportPI.ApproveImportPI((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oImportPI.ErrorMessage = _sErrorMessage;
                }


            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult NotApproveImportPI(ImportPI oPurchaseContact)
        {
            _oImportPI = new ImportPI();
            try
            {
                if (this.ValidateInputApproved(oPurchaseContact))
                {
                    oPurchaseContact.ImportPIStatus = EnumImportPIState.Cancel;
                    oPurchaseContact.DateOfApproved = DateTime.Now;
                    _oImportPI = oPurchaseContact.ApproveImportPI((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oImportPI.ErrorMessage = _sErrorMessage;
                }


            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Request For Revise
        [HttpPost]
        public JsonResult RequestForReviseImportPI(ImportPI oImportPI)
        {
            _oImportPI = new ImportPI();
            try
            {
                oImportPI.ImportPIStatusInt =(int)EnumImportPIState.Request_For_Revise;
                _oImportPI = oImportPI.RequestForReviseImportPI((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReviseImportPI(ImportPI oImportPI)
        {
            _oImportPI = new ImportPI();
            try
            {
                oImportPI.ImportPIStatus = (EnumImportPIState)1;
                _oImportPI = oImportPI.ReviseImportPI((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View Advance Search
        [HttpPost]
        public JsonResult GetsSearchedData(ImportPI oImportPI)
        {
            _oImportPI.ImportPIDetails = new List<ImportPIDetail>();
            List<ImportPI> oImportPIs = new List<ImportPI>();
            try
            {
                string sSQL = GetSQL(oImportPI);
                oImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportPIs = new List<ImportPI>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(ImportPI oImportPI)
        {
            int nIssueDate = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[2]);
            int nValidityDate = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[3]);
            DateTime dValidityStartDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[4]);
            DateTime dValidityEndDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[5]);
            int nDateOfApproved = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[6]);
            DateTime dApproveStartDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[7]);
            DateTime dApproveEndDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[8]);

            int nComboAmount = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[9]);
            double nAmountStart= Convert.ToDouble(oImportPI.ErrorMessage.Split('~')[10]);
            double nAmountEnd = Convert.ToDouble(oImportPI.ErrorMessage.Split('~')[11]);

            string sImportPINo = oImportPI.ErrorMessage.Split('~')[12];
            string sSupplierIDs = oImportPI.ErrorMessage.Split('~')[13];
            string sAgentIDs = oImportPI.ErrorMessage.Split('~')[14];
            string sProductIDs = oImportPI.ErrorMessage.Split('~')[15];
            string sBankBranchIDs = oImportPI.ErrorMessage.Split('~')[16];
            string sPIStatuss = oImportPI.ErrorMessage.Split('~')[17];
            int nBUID = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[18]);
            int nApprovedBy = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[19]);
            string sRemarks = oImportPI.ErrorMessage.Split('~')[20];
            int nProductType = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[21]);

            int nPostingDate = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[22]);
            DateTime dPostingDateStartDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[23]);
            DateTime dPostingDateEndDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[24]);

            string sReturn1 = "SELECT * FROM View_ImportPI ";

            int nImportPIType = (int)oImportPI.ImportPIType;

            #region ImportPINo
            if (sImportPINo != null && sImportPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPINo LIKE " + "'%" + sImportPINo + "%'";
            }
            #endregion

            #region POSTING DATE (DBServerDateTime)
            DateObject.CompareDateQuery(ref sReturn, "DBServerDateTime", nPostingDate, dPostingDateStartDate, dPostingDateEndDate);
            #endregion
          
            #region Supplier id
            if (sSupplierIDs != null && sSupplierIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SupplierID IN(" + sSupplierIDs + ")";
            }
            #endregion

            #region Agent id
            if (sAgentIDs != null && sAgentIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AgentID IN(" + sAgentIDs + ")";
            }
            #endregion

            #region Product id
            if (sProductIDs != null && sProductIDs!= "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPIID IN(SELECT ImportPIID FROM ImportPIDetail WHERE ProductID IN (" + sProductIDs + "))";
            }
            #endregion
            #region nProductType
            if (nProductType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductType =" + nProductType;
            }
            #endregion

            #region bank branch
            if (sBankBranchIDs != null && sBankBranchIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankBranchID_Advise IN(" + sBankBranchIDs + ")";
            }
            #endregion

            #region status
            if (sPIStatuss != null && sPIStatuss != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPIStatus IN(" + sPIStatuss + ")";
            }
            #endregion       

            #region status
            if (nImportPIType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPIType IN(" + nImportPIType + ")";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPIType IN("+ (int)EnumImportPIType.Foreign + "," + (int)EnumImportPIType.Local + "," + (int)EnumImportPIType.NonLC + "," + (int)EnumImportPIType.TTForeign + ")";
            }
            #endregion        

           #region Issue Date Wise
            if (nIssueDate > 0)
            {
                if (nIssueDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate< '" + dStartDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

           #region Validity Date Wise
            if (nValidityDate > 0)
            {
                if (nValidityDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ValidityDate>= '" + dValidityStartDate.ToString("dd MMM yyyy") + "' AND ValidityDate < '" + dValidityStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nValidityDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ValidityDate != '" + dValidityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nValidityDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ValidityDate > '" + dValidityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nValidityDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ValidityDate < '" + dValidityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nValidityDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ValidityDate>= '" + dValidityStartDate.ToString("dd MMM yyyy") + "' AND ValidityDate < '" + dValidityEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nValidityDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ValidityDate< '" + dValidityStartDate.ToString("dd MMM yyyy") + "' OR ValidityDate > '" + dValidityEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

          #region Approve Date Wise
            if (nDateOfApproved > 0)
            {
                if (nDateOfApproved == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateOfApproved>= '" + dApproveStartDate.ToString("dd MMM yyyy") + "' AND DateOfApproved < '" + dApproveStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateOfApproved == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateOfApproved != '" + dApproveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateOfApproved == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateOfApproved > '" + dApproveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateOfApproved == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateOfApproved < '" + dApproveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateOfApproved == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateOfApproved>= '" + dApproveStartDate.ToString("dd MMM yyyy") + "' AND DateOfApproved < '" + dApproveEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateOfApproved == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateOfApproved< '" + dApproveStartDate.ToString("dd MMM yyyy") + "' OR DateOfApproved > '" + dApproveEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

          #region Amount  Wise
            if (nComboAmount > 0)
            {
                if (nComboAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue = "+nAmountStart;
                }
                if (nComboAmount == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue != " + nAmountStart;
                }
                if (nComboAmount == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue > " + nAmountStart;
                }
                if (nComboAmount == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue < " + nAmountStart;
                }
                if (nComboAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (TotalValue>= " + nAmountStart + " AND TotalValue < " + nAmountEnd +")";
                }
                if (nComboAmount == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (TotalValue <" + nAmountStart + " OR TotalValue > " + nAmountEnd+")";
                }
            }
            #endregion

        #region approved By
        if (nApprovedBy > 0)
        {
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " ISNULL(ApprovedBy,0) = " + nApprovedBy;
        }
        #endregion
        #region Remarks
        if (sRemarks != null && sRemarks != "")
        {
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " Note LIKE " + "'%" + sRemarks + "%'";
        }
        #endregion

       #region Bu
            if (nBUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = "+ nBUID ;
            }
            #endregion
            

       sReturn = sReturn1 + sReturn;

        return sReturn;
        }
        #endregion

        #region Text Search
        [HttpPost]
        public JsonResult GetsBySearchKey(ImportPI oImportPI)
        {
            string sSQL = "";
            _oImportPIs = new List<ImportPI>();
            try
            {
                if (oImportPI.ImportPINo == "") oImportPI.ImportPINo = null;
                if (oImportPI.ImportPITypeInt >0)
                {
                    sSQL = "SELECT * FROM View_ImportPI WHERE ImportPIType=" + oImportPI.ImportPITypeInt + " and BUID = " + oImportPI.BUID + " AND ImportPINo Like '%" + oImportPI.ImportPINo + "%'";
                }
                else
                {
                    sSQL = "SELECT * FROM View_ImportPI WHERE ImportPIType in (" + (int)EnumImportPIType.Foreign + "," + (int)EnumImportPIType.Local + "," + (int)EnumImportPIType.NonLC + "," + (int)EnumImportPIType.TTForeign + ") and BUID = " + oImportPI.BUID + " AND ImportPINo Like '%" + oImportPI.ImportPINo + "%'";
                }
                
                _oImportPIs = ImportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportPIs = new List<ImportPI>();
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
                _oImportPIs.Add(_oImportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GEts Impoat PI BUwise
        [HttpPost]
        public JsonResult GetsByBUSearchKey(ImportPI oImportPI)
        {
            string sSQL = "";
            _oImportPIs = new List<ImportPI>();
            try
            {
                 sSQL = "SELECT * FROM View_ImportPI WHERE BUID = " + oImportPI.BUID;
                 if (!string.IsNullOrEmpty(oImportPI.ImportPINo))
                 {
                     sSQL += " AND ImportPINo Like '%" + oImportPI.ImportPINo + "%'";
                 }
                _oImportPIs = ImportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportPIs = new List<ImportPI>();
                _oImportPI = new ImportPI();
                _oImportPI.ErrorMessage = ex.Message;
                _oImportPIs.Add(_oImportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPIs);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion

        #region ImportPI ImportPIAttachment
        public PartialViewResult Attachment(int id, string ms, double ts)
        {
            ImportPIAttachment oImportPIAttachment = new ImportPIAttachment();
            List<ImportPIAttachment> oImportPIAttachments = new List<ImportPIAttachment>();
            oImportPIAttachments = ImportPIAttachment.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oImportPIAttachment.ImportPIID = id;
            oImportPIAttachment.ImportPIAttachments = oImportPIAttachments;
            TempData["message"] = ms;
            return PartialView(oImportPIAttachment);
        }

        [HttpPost]
        public ActionResult UploadAttchment(HttpPostedFileBase file, ImportPIAttachment oImportPIAttachment)
        {
            string sErrorMessage = "";
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
                        sErrorMessage = "Please select an file!";
                    }
                    else if (data.Length > nMaxLength)
                    {
                        sErrorMessage = "You can select maximum 1MB file size!";
                    }
                    else if (oImportPIAttachment.ImportPIID <= 0)
                    {
                        sErrorMessage = "Your Selected Import PI Is Invalid!";
                    }
                    else
                    {
                        oImportPIAttachment.AttatchFile = data;
                        oImportPIAttachment.AttatchmentName = file.FileName;
                        oImportPIAttachment.FileType = file.ContentType;
                        oImportPIAttachment = oImportPIAttachment.Save((int)Session[SessionInfo.currentUserID]);
                    }
                }
                else
                {
                    sErrorMessage = "Please select an file!";
                }
            }
            catch (Exception ex)
            {
                sErrorMessage = "";
                sErrorMessage = ex.Message;
            }
            return RedirectToAction("Attachment", new { id = oImportPIAttachment.ImportPIID, ms = sErrorMessage, ts = Convert.ToDouble(DateTime.Now.Millisecond) });
        }
        public ActionResult DownloadAttachment(int id, double ts)
        {
            ImportPIAttachment oImportPIAttachment = new ImportPIAttachment();
            try
            {
                oImportPIAttachment.ImportPIAttachmentID = id;
                oImportPIAttachment = ImportPIAttachment.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oImportPIAttachment.AttatchFile != null)
                {
                    var file = File(oImportPIAttachment.AttatchFile, oImportPIAttachment.FileType);
                    file.FileDownloadName = oImportPIAttachment.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oImportPIAttachment.AttatchmentName);
            }
        }
        [HttpPost]
        public JsonResult DeleteAttachment(ImportPIAttachment oImportPIAttachment)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oImportPIAttachment.Delete(oImportPIAttachment.ImportPIAttachmentID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region BillSearch
        [HttpPost]
        public JsonResult BillSearch(ImportPI oImportPI)
        {
            string sSQL = "";
            List<ImportPIReference> oImportPIReferences = new List<ImportPIReference>();
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            ImportPIReference oImportPIReference = new ImportPIReference();
            try
            {
                if(oImportPI.RefTypeInInt ==(int)EnumImportPIRefType.Bill)
                {
                    sSQL = "SELECT * FROM View_PurchaseInvoice WHERE BUID = " + oImportPI.BUID + " AND ContractorID = " + oImportPI.SupplierID + " AND ISNULL(ApprovedBy,0)!=0 AND InvoicePaymentMode = " + (int)EnumInvoicePaymentMode.LC + " AND PurchaseInvoiceID NOT IN (SELECT IMPR.ReferenceID FROM View_ImportPIReference AS IMPR  WHERE IMPR.RefType="+(int)EnumImportPIRefType.Bill+")";
                }
                oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(PurchaseInvoice oItem in oPurchaseInvoices)
                {
                    oImportPIReference = new ImportPIReference();
                    oImportPIReference.ReferenceID = oItem.PurchaseInvoiceID;
                    oImportPIReference.ReferenceNo = oItem.PurchaseInvoiceNo;
                    oImportPIReference.BillNo = oItem.BillNo;
                    oImportPIReference.ReferenceDate = oItem.DateofInvoice;
                    oImportPIReference.ReferenceAmount = oItem.Amount;
                    oImportPIReference.YetToReferenceAmount = oItem.YeToBillAmount;
                    oImportPIReference.Amount = oItem.YeToBillAmount;
                    oImportPIReference.CurrencyID = oItem.CurrencyID;
                    oImportPIReference.CurrencySymbol = oItem.CurrencySymbol; 
                    oImportPIReferences.Add(oImportPIReference);
                }
            }
            catch (Exception ex)
            {
                oImportPIReferences = new List<ImportPIReference>();
                oImportPIReference = new ImportPIReference();
                oImportPIReference.ErrorMessage = ex.Message;
                oImportPIReferences.Add(oImportPIReference);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIReferences);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion
        #region GetImportPIDetails
        [HttpPost]
        public JsonResult GetImportPIDetails(ImportPI oImportPI)
        {
            string sSQL = "";
            List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
            ImportPIDetail oImportPIDetail = new ImportPIDetail();
            Company oCompany = new Company();
            try
            {
                if(oImportPI.RefTypeInInt ==(int)EnumImportPIRefType.Bill)
                {
                    sSQL = "SELECT ProductID, SUM(Qty) AS Qty, AVG( UnitPrice) AS UnitPrice, SUM(Amount) AS Amount, ProductCode, ProductName, MUName FROM View_PurchaseInvoiceDetail WHERE PurchaseInvoiceID IN(" + oImportPI.Note + ") GROUP BY ProductID, ProductCode, ProductName, MUName";
                }
                oPurchaseInvoiceDetails = PurchaseInvoiceDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(PurchaseInvoiceDetail oItem in oPurchaseInvoiceDetails)
                {
                    oImportPIDetail = new ImportPIDetail();
                    oImportPIDetail.ProductID = oItem.ProductID;
                    oImportPIDetail.ProductCode = oItem.ProductCode;
                    oImportPIDetail.ProductName = oItem.ProductName;
                    oImportPIDetail.MUName = oItem.MUName;
                    oImportPIDetail.Qty = oItem.Qty;
//                    oImportPIDetail.MUName = oItem.Qty;
                    oImportPIDetail.UnitPrice = oItem.UnitPrice;
                    oImportPIDetail.Amount = oItem.Amount;
                    oImportPIDetails.Add(oImportPIDetail);
                }
            }
            catch (Exception ex)
            {
                oImportPIDetails = new List<ImportPIDetail>();
                oImportPIDetail = new ImportPIDetail();
                oImportPIDetail.ErrorMessage = ex.Message;
                oImportPIDetails.Add(oImportPIDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIDetails);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion


        #region POSearch
        [HttpPost]
        public JsonResult POSearch(ImportPI oImportPI)
        {
            string sSQL = "";
            List<ImportPIReference> oImportPIReferences = new List<ImportPIReference>();
            List<PurchaseOrder> oPurchaseOrders = new List<PurchaseOrder>();
            ImportPIReference oImportPIReference = new ImportPIReference();
            try
            {
                if (oImportPI.RefTypeInInt == (int)EnumImportPIRefType.PO)
                {
                    sSQL = "SELECT * FROM View_PurchaseOrder WHERE BUID = " + oImportPI.BUID + " AND ContractorID = " + oImportPI.SupplierID + " AND ISNULL(ApproveBy,0)!=0 AND PaymentMode = " + (int)EnumInvoicePaymentMode.LC + " AND POID NOT IN (SELECT IMPR.ReferenceID FROM View_ImportPIReference AS IMPR  WHERE IMPR.RefType=" + (int)EnumImportPIRefType.PO + ")";
                }
                oPurchaseOrders = PurchaseOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (PurchaseOrder oItem in oPurchaseOrders)
                {
                    oImportPIReference = new ImportPIReference();
                    oImportPIReference.ReferenceID = oItem.POID;
                    oImportPIReference.ReferenceNo = oItem.PONo;
                    oImportPIReference.BillNo = oItem.PONo;
                    oImportPIReference.ReferenceDate = oItem.PODate;
                    oImportPIReference.ReferenceAmount = oItem.Amount;
                    oImportPIReference.YetToReferenceAmount = oItem.YetToPI_Amount;
                    oImportPIReference.Amount = oItem.YetToPI_Amount;// oItem.YeToPOAmount;
                    oImportPIReference.CurrencyID = oItem.CurrencyID;
                    oImportPIReference.CurrencySymbol = oItem.CurrencySymbol;
                    oImportPIReferences.Add(oImportPIReference);
                }
            }
            catch (Exception ex)
            {
                oImportPIReferences = new List<ImportPIReference>();
                oImportPIReference = new ImportPIReference();
                oImportPIReference.ErrorMessage = ex.Message;
                oImportPIReferences.Add(oImportPIReference);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIReferences);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion
        #region GetPODetails
        [HttpPost]
        public JsonResult GetPODetails(ImportPI oImportPI)
        {
            string sSQL = "";
            List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();
            List<PurchaseOrderDetail> oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
            ImportPIDetail oImportPIDetail = new ImportPIDetail();
            Company oCompany = new Company();
            try
            {
                if (oImportPI.RefTypeInInt == (int)EnumImportPIRefType.PO)
                {
                    sSQL = "SELECT ProductID, SUM(Qty) AS Qty, AVG( UnitPrice) AS UnitPrice, SUM(Qty*UnitPrice) AS Amount, ProductCode, ProductName, UnitName FROM View_PurchaseOrderDetail WHERE POID IN(" + oImportPI.Note + ") GROUP BY ProductID, ProductCode, ProductName, UnitName";
                }
                oPurchaseOrderDetails = PurchaseOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (PurchaseOrderDetail oItem in oPurchaseOrderDetails)
                {
                    oImportPIDetail = new ImportPIDetail();
                    oImportPIDetail.ProductID = oItem.ProductID;
                    oImportPIDetail.ProductCode = oItem.ProductCode;
                    oImportPIDetail.ProductName = oItem.ProductName;
                    oImportPIDetail.MUName = oItem.UnitName;
                    oImportPIDetail.Qty = oItem.Qty;
                    //                    oImportPIDetail.MUName = oItem.Qty;
                    oImportPIDetail.UnitPrice = oItem.UnitPrice;
                    oImportPIDetail.Amount = oItem.Qty*oItem.UnitPrice;
                    oImportPIDetails.Add(oImportPIDetail);
                }
            }
            catch (Exception ex)
            {
                oImportPIDetails = new List<ImportPIDetail>();
                oImportPIDetail = new ImportPIDetail();
                oImportPIDetail.ErrorMessage = ex.Message;
                oImportPIDetails.Add(oImportPIDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIDetails);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
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

        #region Print
        [HttpPost]
        public ActionResult SetImportPIListData(ImportPI oImportPI)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportPI);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintImportPIs()
        {
            _oImportPI = new ImportPI();
            try
            {
                _oImportPI = (ImportPI)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_ImportPI WHERE ImportPIID IN (" + _oImportPI.Note + ") Order By ImportPIID";
                _oImportPI.ImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPIs = new List<ImportPI>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oImportPI.Company = oCompany;

            string Messge = "Import PI List";
            rptImportPIs oReport = new rptImportPIs();
            byte[] abytes = oReport.PrepareReport(_oImportPI, Messge);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintoImportPIPreview(int id)
        {
            _oImportPI = new ImportPI();
            _oImportPI = _oImportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportPI.ImportPIDetails = ImportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ImportInvoice oImportInvoice = new ImportInvoice();
            if (_oImportPI.ImportPIType == EnumImportPIType.Servise || _oImportPI.ImportPIType == EnumImportPIType.FancyYarn)
            {
                oImportInvoice = oImportInvoice.Get(_oImportPI.ImportPITypeInt, _oImportPI.ImportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oImportInvoice.ImportInvoiceID > 0)
                {
                    oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oImportPI.SupplierID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            BankBranch oBankBranch = new BankBranch();
            oBankBranch = BankBranch.Get(_oImportPI.BankBranchID_Advise, ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oImportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptImportPI oReport = new rptImportPI();//for plastic and integrated
            byte[] abytes = oReport.PrepareReport(_oImportPI, oImportInvoice, oContractor, oCompany, oBusinessUnit, oBankBranch);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintInvoice_GRN(int id)
        {
            _oImportPI = new ImportPI();
            _oImportPI = _oImportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ImportInvoice oImportInvoice = new ImportInvoice();
            List<GRN> oGRNs = new List<GRN>();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            oImportInvoice = oImportInvoice.Get(_oImportPI.ImportPITypeInt, _oImportPI.ImportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oImportInvoice.ImportInvoiceID > 0)
            {
                oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGRNs = GRN.Gets("Select * from View_GRN where GRNType in (" + (int)EnumGRNType.ImportInvoice + "," + (int)EnumGRNType.FancyYarn + ") and RefObjectID=" + oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGRNDetails = GRNDetail.Gets("Select * from View_GRNDetail where  GRNID in (Select GRN.GRNID from GRN where  GRNType in (" + (int)EnumGRNType.ImportInvoice + ","  + (int)EnumGRNType.FancyYarn + ") and RefObjectID=" + oImportInvoice.ImportInvoiceID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oImportPI.SupplierID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oImportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptImportGRN oReport = new rptImportGRN();//for plastic and integrated
            byte[] abytes = oReport.PrepareReport(oGRNs, oGRNDetails, oImportInvoice, oContractor, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
       
        #endregion

        #region Get Product BU, User and Name wise ( write by Mahabub)
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.ImportPI, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.ImportPI, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #endregion
        #region Get Product BU, User and Name wise ( write by Mamun)
        [HttpPost]
        public JsonResult GetProductsPI(ImportPI oImportPI)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oImportPI.ProductType > 0)
                {
                    _oProducts = Product.Gets_Import(oImportPI.ErrorMessage, oImportPI.BUID, (int)oImportPI.ProductType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else {

                    if (oImportPI.ErrorMessage != null && oImportPI.ErrorMessage != "")
                    {
                        _oProducts = Product.GetsPermittedProductByNameCode(oImportPI.BUID, EnumModuleName.ImportPI, EnumProductUsages.Regular, oImportPI.ErrorMessage, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        _oProducts = Product.GetsPermittedProduct(oImportPI.BUID, EnumModuleName.ImportPI, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
               
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            var jsonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
        #endregion
    }
}

