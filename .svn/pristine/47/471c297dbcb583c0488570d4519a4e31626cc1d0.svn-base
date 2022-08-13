using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class CommercialInvoiceController : Controller
    {

        #region Declaration
        List<CommercialInvoice> _oCommercialInvoices = new List<CommercialInvoice>();
        CommercialInvoice _oCommercialInvoice = new CommercialInvoice();
        List<CommercialInvoiceDetail> _oCommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
        CommercialInvoiceDetail _oCommercialInvoiceDetail = new CommercialInvoiceDetail();
        LCTransfer _oLCTransfer = new LCTransfer();
        LCTransferDetail _oLCTransferDetail = new LCTransferDetail();
        ExportDocSetup _oExportDocSetup = new ExportDocSetup();
        List<ExportDocSetup> _oExportDocSetups = new List<ExportDocSetup>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        List<ClientOperationSetting> _oClientOperationSettings = new List<ClientOperationSetting>();
        CommercialInvoiceRegister _oCommercialInvoiceRegister = new CommercialInvoiceRegister();
        MasterLC _oMasterLC = new MasterLC();
        #endregion

        #region Actions
        public ActionResult ViewLCWiseCommercialInvoices(int id)
        {
            _oCommercialInvoices = new List<CommercialInvoice>();
            _oLCTransfer = new LCTransfer();
            _oMasterLC = new MasterLC();
            _oClientOperationSetting = new ClientOperationSetting();
            _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.CommercialInvoiceFormat, (int)Session[SessionInfo.currentUserID]);
            if (id > 0)
            {
                if (Convert.ToInt16(_oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Buying_Format)
                {
                    _oLCTransfer = _oLCTransfer.Get(id, (int)Session[SessionInfo.currentUserID]);
                    _oCommercialInvoices = CommercialInvoice.GetsByTransfer(id, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oMasterLC = _oMasterLC.Get(id, (int)Session[SessionInfo.currentUserID]);
                    //_oCommercialInvoices = CommercialInvoice.GetsByLC(id, (int)Session[SessionInfo.currentUserID]);
                }
            }
            ViewBag.LCTransfer = _oLCTransfer;
            ViewBag.MasterLC = _oMasterLC;
            ViewBag.ClientOperationSetting = _oClientOperationSetting;
            
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName. CommercialInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(_oCommercialInvoices);
        }

        public ActionResult ViewCommercialInvoices(int buid, int menuid )
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CommercialInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oClientOperationSetting = new ClientOperationSetting();
            _oCommercialInvoices = new List<CommercialInvoice>();
            //_oCommercialInvoices = CommercialInvoice.Gets("SELECT * FROM View_CommercialInvoice WHERE BUID = "+buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.DocumentTypes = EnumObject.jGets(typeof(EnumDocumentType));
            ViewBag.BUID = buid;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.CommercialInvoiceFormat, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)) ;
            ViewBag.InvoiceStatusList = EnumObject.jGets(typeof(EnumCommercialInvoiceStatus));
            return View(_oCommercialInvoices);
        }

        public ActionResult ViewCommercialInvoice(int id, int nCommercialInvoiceID) //id may MasterLC or LCTransfer
        {
            
            _oLCTransfer = new LCTransfer();
            _oMasterLC = new MasterLC();
            _oCommercialInvoice.CommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
            _oClientOperationSetting = new ClientOperationSetting();
            
            _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.CommercialInvoiceFormat, (int)Session[SessionInfo.currentUserID]);
            
            if (nCommercialInvoiceID > 0)
            {
                _oCommercialInvoice = new CommercialInvoice();
                _oCommercialInvoice = _oCommercialInvoice.Get(nCommercialInvoiceID, (int)Session[SessionInfo.currentUserID]);
                _oCommercialInvoice.CommercialInvoiceDetails = CommercialInvoiceDetail.Gets(nCommercialInvoiceID, (int)Session[SessionInfo.currentUserID]);
                
            }
            else
            {
                
                if(Convert.ToInt16(_oClientOperationSetting.Value)==(int)EnumClientOperationValueFormat.Buying_Format)
                {
                    _oLCTransfer = _oLCTransfer.Get(id, (int)Session[SessionInfo.currentUserID]);
                    _oCommercialInvoice = new CommercialInvoice();
                    _oCommercialInvoice.LCTransferID = _oLCTransfer.LCTransferID;
                    _oCommercialInvoice.MasterLCID = _oLCTransfer.MasterLCID;
                    _oCommercialInvoice.MasterLCNo = _oLCTransfer.MasterLCNo;
                    _oCommercialInvoice.TransferNo = _oLCTransfer.TransferNo;
                    _oCommercialInvoice.TransferAmount = _oLCTransfer.TransferAmount;
                    _oCommercialInvoice.BuyerID = _oLCTransfer.BuyerID;
                    _oCommercialInvoice.BuyerName = _oLCTransfer.BuyerName;
                    _oCommercialInvoice.ProductionFactoryID = _oLCTransfer.ProductionFactoryID;
                    _oCommercialInvoice.YetToInvoiceAmount = _oLCTransfer.YetToInvoiceAmount;
                }
                else
                {
                    _oCommercialInvoice = new CommercialInvoice();
                    _oMasterLC = _oMasterLC.Get(id, (int)Session[SessionInfo.currentUserID]);
                    _oCommercialInvoice.MasterLCID = _oMasterLC.MasterLCID;
                    _oCommercialInvoice.MasterLCNo = _oMasterLC.MasterLCNo;
                    _oCommercialInvoice.BuyerID = _oMasterLC.Applicant;
                    _oCommercialInvoice.BuyerName = _oMasterLC.ApplicantName;
                    _oCommercialInvoice.YetToInvoiceAmount = _oMasterLC.YetToInvoiceAmount;
                }
                
            }
            ViewBag.TransportTypes = EnumObject.jGets(typeof(EnumTransportType));
            ViewBag.COSForCommerecialInvoiceFormat = _oClientOperationSetting;
            ViewBag.COSForLCTransferType = _oClientOperationSetting.Get((int)EnumOperationType.LCTransferType, (int)Session[SessionInfo.currentUserID]);
            
            return View(_oCommercialInvoice);
        }

        #region Get Invoices
        [HttpPost]
        public JsonResult GetInvoicesByLC(CommercialInvoice oCommercialInvoice)
        {
            _oCommercialInvoices = new List<CommercialInvoice>();
            try
            {
                if (oCommercialInvoice.MasterLCID > 0)
                {
                   _oCommercialInvoices = CommercialInvoice.GetsByLC(oCommercialInvoice.MasterLCID, (int)Session[SessionInfo.currentUserID]);
                  // _oCommercialInvoices = _oCommercialInvoices.Where(x => x.CommercialInvoiceID == 761).ToList();
                }
            }
            catch (Exception ex)
            {
                oCommercialInvoice = new CommercialInvoice();
                oCommercialInvoice.ErrorMessage = ex.Message;
                _oCommercialInvoices.Add(oCommercialInvoice);
            }
            var jsonResult = Json(_oCommercialInvoices, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(CommercialInvoice oCommercialInvoice)
        {
            _oCommercialInvoice = new CommercialInvoice();
            try
            {
                oCommercialInvoice.ShipmentMode = (EnumTransportType)oCommercialInvoice.ShipmentMode;
                oCommercialInvoice.CIFormat = (EnumClientOperationValueFormat)oCommercialInvoice.CIFormatInInt;
                _oCommercialInvoice = oCommercialInvoice.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialInvoice = new CommercialInvoice();
                _oCommercialInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommercialInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        [HttpPost]
        public JsonResult Delete(CommercialInvoice oCommercialInvoice)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oCommercialInvoice.Delete(oCommercialInvoice.CommercialInvoiceID, (int)Session[SessionInfo.currentUserID]);
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

        #region Approve
        public JsonResult Approval(CommercialInvoice oCommercialInvoice)
        {
            _oCommercialInvoice = new CommercialInvoice();
            try
            {
                _oCommercialInvoice = oCommercialInvoice.Approval((int)Session[SessionInfo.currentUserID]);
                if (oCommercialInvoice.CommercialInvoiceDetailID != 0)//use for register menu
                {
                    _oCommercialInvoiceRegister = new CommercialInvoiceRegister();
                    _oCommercialInvoiceRegister = CommercialInvoiceRegister.Get(oCommercialInvoice.CommercialInvoiceDetailID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oCommercialInvoice = new CommercialInvoice();
                _oCommercialInvoice.ErrorMessage = ex.Message;
            }
            if (oCommercialInvoice.CommercialInvoiceDetailID != 0)//use for register menu
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string sjson = serializer.Serialize(_oCommercialInvoiceRegister);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string sjson = serializer.Serialize(_oCommercialInvoice);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ChangeField(CommercialInvoice oCommercialInvoice)
        {
            _oCommercialInvoice = new CommercialInvoice();
            try
            {
                _oCommercialInvoice = oCommercialInvoice.ChangeField( (int)Session[SessionInfo.currentUserID]);
                _oCommercialInvoiceRegister = new CommercialInvoiceRegister();
                _oCommercialInvoiceRegister = CommercialInvoiceRegister.Get(oCommercialInvoice.CommercialInvoiceDetailID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialInvoice = new CommercialInvoice();
                _oCommercialInvoice.ErrorMessage = ex.Message;
            }
           JavaScriptSerializer serializer = new JavaScriptSerializer();
           string sjson = serializer.Serialize(_oCommercialInvoiceRegister);
           return Json(sjson, JsonRequestBehavior.AllowGet);
           
        }
        #endregion

        #region Document Attachment
        public PartialViewResult AttachDocument(int id, string ms)
        {
            CommercialInvoiceDoc oCommercialInvoiceDoc = new CommercialInvoiceDoc();
            List<CommercialInvoiceDoc> oCommercialInvoiceDocs = new List<CommercialInvoiceDoc>();
            oCommercialInvoiceDoc.CommercialInvoiceID = id;
            oCommercialInvoiceDocs = CommercialInvoiceDoc.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oCommercialInvoiceDoc.CommercialInvoiceDocList = oCommercialInvoiceDocs;
            //oCommercialInvoiceDoc.DocumentTypes = DocumentType.Gets();
            TempData["message"] = ms;
            return PartialView(oCommercialInvoiceDoc);
        }

        [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase file, CommercialInvoiceDoc oCommercialInvoiceDoc)
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

                    double nMaxLength = 500 * 1024;
                    if (data == null || data.Length <= 0)
                    {
                        sErrorMessage = "Please select an file!";
                    }
                    else if (data.Length > nMaxLength)
                    {
                        sErrorMessage = "You can select maximum 500KB file size!";
                    }
                    else if (oCommercialInvoiceDoc.CommercialInvoiceID <= 0)
                    {
                        sErrorMessage = "Your Selected Commercial Invoice Is Invalid!";
                    }
                    else if (oCommercialInvoiceDoc.DocTypeInInt <= 0)
                    {
                        sErrorMessage = "Please select an Document Type!";
                    }
                    else
                    {
                        oCommercialInvoiceDoc.DocType = (EnumDocumentType)oCommercialInvoiceDoc.DocTypeInInt;
                        oCommercialInvoiceDoc.DocFile = data;
                        oCommercialInvoiceDoc.DocName = file.FileName;
                        oCommercialInvoiceDoc.FileType = file.ContentType;
                        oCommercialInvoiceDoc = oCommercialInvoiceDoc.Save((int)Session[SessionInfo.currentUserID]);
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
            return RedirectToAction("AttachDocument", new { id = oCommercialInvoiceDoc.CommercialInvoiceID, ms = sErrorMessage });
        }

        public ActionResult DownloadPackingPolicy(int id)
        {
            CommercialInvoiceDoc oCommercialInvoiceDoc = new CommercialInvoiceDoc();
            try
            {
                oCommercialInvoiceDoc.CommercialInvoiceDocID = id;
                oCommercialInvoiceDoc = CommercialInvoiceDoc.GetWithDocFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oCommercialInvoiceDoc.DocFile != null)
                {
                    var file = File(oCommercialInvoiceDoc.DocFile, oCommercialInvoiceDoc.FileType);
                    file.FileDownloadName = oCommercialInvoiceDoc.DocName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oCommercialInvoiceDoc.DocName);
            }
        }

        [HttpGet]
        public ActionResult DeleteDocument(int id)
        {
            CommercialInvoiceDoc oCommercialInvoiceDoc = new CommercialInvoiceDoc();
            oCommercialInvoiceDoc = CommercialInvoiceDoc.Get(id, (int)Session[SessionInfo.currentUserID]);
            string sErrorMease = "";
            int nId = 0;
            nId = oCommercialInvoiceDoc.CommercialInvoiceID;
            try
            {
                sErrorMease = oCommercialInvoiceDoc.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            return RedirectToAction("AttachDocument", new { id = nId, ms = sErrorMease });
        }
        #endregion

        #region GEt Hitory Picker
        [HttpPost]
        public JsonResult GetInvoiceHistory(CommercialInvoice oCommercialInvoice)
        {
            List<CommercialInvoiceHistory> oCommercialInvoiceHistories = new List<CommercialInvoiceHistory>();
            try
            {
                oCommercialInvoiceHistories = ESimSol.BusinessObjects.CommercialInvoiceHistory.Gets(oCommercialInvoice.CommercialInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                CommercialInvoiceHistory oCommercialInvoiceHistory = new CommercialInvoiceHistory();
                oCommercialInvoiceHistory.ErrorMessage = ex.Message;
                oCommercialInvoiceHistories.Add(oCommercialInvoiceHistory);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommercialInvoiceHistories);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewCommercialInvoiceHistory(int id) // id is  CommercialInvoice ID
        {
            _oCommercialInvoice = new CommercialInvoice();
            _oCommercialInvoice.CommercialInvoiceHistories = ESimSol.BusinessObjects.CommercialInvoiceHistory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            return View(_oCommercialInvoice);
        }
        #endregion

        [HttpPost]
        public JsonResult GetCommercialInvoiceDetails(CommercialInvoiceDetail oCommercialInvoiceDetail)
        {
            List<CommercialInvoiceDetail> oCommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_CommercialInvoiceDetail WHERE CommercialInvoiceID = "+oCommercialInvoiceDetail.CommercialInvoiceID+" AND  CommercialInvoiceDetailID NOT IN (SELECT CIDID FROM PackingList)";
                oCommercialInvoiceDetails = CommercialInvoiceDetail.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oCommercialInvoiceDetail = new CommercialInvoiceDetail();
                oCommercialInvoiceDetail.ErrorMessage = ex.Message;
                oCommercialInvoiceDetails.Add(oCommercialInvoiceDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommercialInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCIDetails(CommercialInvoice oCommercialInvoice)
        {
            _oCommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
            
            _oClientOperationSetting = new ClientOperationSetting();
            _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.CommercialInvoiceFormat, (int)Session[SessionInfo.currentUserID]);
            ClientOperationSetting oCommercial_Module_Using_System = new ClientOperationSetting();
            oCommercial_Module_Using_System = oCommercial_Module_Using_System.GetByOperationType((int)EnumOperationType.Commercial_Module_Using_System, (int)Session[SessionInfo.currentUserID]);
            try
            {
                if(Convert.ToInt16(_oClientOperationSetting.Value)==(int)EnumClientOperationValueFormat.Buying_Format)
                {
                    List<LCTransferDetail> oLCTransferDetails = new List<LCTransferDetail>();
                    string sSQL = "SELECT * FROM View_LCTransferDetail WHERE LCTransferID = " + oCommercialInvoice.LCTransferID+ " AND YetToInvoiceQty >0";
                    oLCTransferDetails = LCTransferDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (LCTransferDetail oItem in oLCTransferDetails)
                    {
                        _oCommercialInvoiceDetail = new CommercialInvoiceDetail();
                        _oCommercialInvoiceDetail.ReferenceDetailID = oItem.LCTransferDetailID;
                        _oCommercialInvoiceDetail.TechnicalSheetID = oItem.TechnicalSheetID;
                        _oCommercialInvoiceDetail.OrderRecapID = oItem.OrderRecapID;
                        _oCommercialInvoiceDetail.ShipmentDate = oItem.ShipmentDate;
                        _oCommercialInvoiceDetail.TransferQty = oItem.TransferQty;
                        _oCommercialInvoiceDetail.InvoiceQty = oItem.YetToInvoiceQty;
                        _oCommercialInvoiceDetail.YetToInvoiceQty = oItem.YetToInvoiceQty;
                        _oCommercialInvoiceDetail.DiscountInPercent = oItem.DiscountInPercent;
                        _oCommercialInvoiceDetail.AdditionInPercent = oItem.AdditionInPercent;
                        _oCommercialInvoiceDetail.UnitPrice = oItem.FOB;
                        _oCommercialInvoiceDetail.Discount=0;
                        _oCommercialInvoiceDetail.FOB = oItem.FOB;
                        _oCommercialInvoiceDetail.Amount =oItem.YetToInvoiceQty*oItem.FOB;
                        _oCommercialInvoiceDetail.StyleNo = oItem.StyleNo;
                        _oCommercialInvoiceDetail.OrderRecapNo = oItem.OrderRecapNo;
                        _oCommercialInvoiceDetail.ProductName = oItem.ProductName;
                        _oCommercialInvoiceDetail.Fabrication = oItem.Fabrication;
                        _oCommercialInvoiceDetail.HSCode="";
                        _oCommercialInvoiceDetail.CAT="";
                        _oCommercialInvoiceDetails.Add(_oCommercialInvoiceDetail);
                    }
                }
                else
                {
                    if (Convert.ToInt16(oCommercial_Module_Using_System.Value) == (int)EnumClientOperationValueFormat.PIWise)
                    {
                        List<ProformaInvoiceDetail> oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();
                        string sSQL = "SELECT * FROM View_ProformaInvoiceDetail WHERE ProformaInvoiceID IN (SELECT ProformaInvoiceID FROM MasterLCDetail WHERE MasterLCID = " + oCommercialInvoice.MasterLCID + ") AND ISNULL(YetToInvoiceQty,0)>0";
                        oProformaInvoiceDetails = ProformaInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        foreach (ProformaInvoiceDetail oItem in oProformaInvoiceDetails)
                        {
                            _oCommercialInvoiceDetail = new CommercialInvoiceDetail();
                            _oCommercialInvoiceDetail.ReferenceDetailID = oItem.ProformaInvoiceDetailID;
                            _oCommercialInvoiceDetail.TechnicalSheetID = oItem.TechnicalSheetID;
                            _oCommercialInvoiceDetail.OrderRecapID = oItem.OrderRecapID;
                            _oCommercialInvoiceDetail.ShipmentDate = oItem.ShipmentDate;
                            //_oCommercialInvoiceDetail.InvoiceQty = oItem.YetToInvoiceQty;
                            //_oCommercialInvoiceDetail.YetToInvoiceQty = oItem.YetToInvoiceQty;
                            _oCommercialInvoiceDetail.InvoiceQty = oItem.InvoiceQty;
                            _oCommercialInvoiceDetail.YetToInvoiceQty = oItem.YetToInvoiceQty;
                            _oCommercialInvoiceDetail.ShipmentQty = oItem.ShipmentQty;
                            _oCommercialInvoiceDetail.PIDetailQty = oItem.Quantity;

                            _oCommercialInvoiceDetail.DiscountInPercent = oItem.DiscountInPercent;
                            _oCommercialInvoiceDetail.AdditionInPercent = oItem.AdditionInPercent;
                            _oCommercialInvoiceDetail.UnitPrice = oItem.FOB;
                            _oCommercialInvoiceDetail.Discount = 0;
                            _oCommercialInvoiceDetail.FOB = oItem.FOB;
                            _oCommercialInvoiceDetail.Amount =(oItem.InvoiceQty * oItem.FOB);
                            _oCommercialInvoiceDetail.StyleNo = oItem.StyleNo;
                            _oCommercialInvoiceDetail.OrderRecapNo = oItem.OrderRecapNo;
                            _oCommercialInvoiceDetail.ProductName = oItem.ProductName;
                            _oCommercialInvoiceDetail.Fabrication = oItem.FabricName;
                            _oCommercialInvoiceDetail.HSCode = "";
                            _oCommercialInvoiceDetail.CAT = "";
                            _oCommercialInvoiceDetails.Add(_oCommercialInvoiceDetail);
                        }

                    }
                    else
                    {
                        List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
                        string sSQL = "SELECT  * FROM View_OrderRecap WHERE BuyerID = " + oCommercialInvoice.BuyerID + " AND  OrderRecapID IN (SELECT HH.OrderRecapID  FROM ShipmentDetail AS HH WHERE HH.ShipmentID IN (SELECT SS.ShipmentID FROM Shipment AS SS WHERE ISNULL(SS.ApprovedBy,0)!=0 )) AND  ISNULL(YetToInvoicety,0)>0";
                        if (string.IsNullOrWhiteSpace(oCommercialInvoice.Param))
                        {
                            sSQL += "AND OrderRecapID IN (SELECT OrderRecapID FROM ShipmentDetail WHERE ShipmentID IN (SELECT  ShipmentID FROM Shipment WHERE ShipmentDate between  '" + DateTime.Today.AddDays(-7).ToString("dd MMM  yyyy") + "' and '" + DateTime.Today.ToString("dd MMM  yyyy") + "' ))";//get last one week
                        }
                        else
                        {
                            bool bIsDateSearch = Convert.ToBoolean(oCommercialInvoice.Param.Split('~')[0]);
                            DateTime dStartDate = Convert.ToDateTime(oCommercialInvoice.Param.Split('~')[1]);
                            DateTime dEndDate = Convert.ToDateTime(oCommercialInvoice.Param.Split('~')[2]);
                            string sStyleNO = oCommercialInvoice.Param.Split('~')[3];
                            string sOrderNo = oCommercialInvoice.Param.Split('~')[4];
                            #region Date
                            if (bIsDateSearch)
                            {
                                sSQL += "AND OrderRecapID IN (SELECT OrderRecapID FROM ShipmentDetail WHERE ShipmentID IN (SELECT  ShipmentID FROM Shipment WHERE ShipmentDate between  '" + dStartDate.ToString("dd MMM  yyyy") + "' and '" + dEndDate.ToString("dd MMM  yyyy") + "' ))";//get last one week
                            }
                            #endregion
                            #region OrderRecapNo
                            if (sOrderNo != null && sOrderNo != "")
                            {
                                sSQL = sSQL + " AND OrderRecapNo LIKE'%" + sOrderNo + "%'";
                            }
                            #endregion

                            #region StyleNo
                            if (sStyleNO != null && sStyleNO != "")
                            {
                                
                                sSQL = sSQL + " AND StyleNo LIKE'%" + sStyleNO + "%'";
                            }
                            #endregion

                        }
                        oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        foreach (OrderRecap oItem in oOrderRecaps)
                        {
                            _oCommercialInvoiceDetail = new CommercialInvoiceDetail();
                            _oCommercialInvoiceDetail.ReferenceDetailID = 0;
                            _oCommercialInvoiceDetail.TechnicalSheetID = oItem.TechnicalSheetID;
                            _oCommercialInvoiceDetail.OrderRecapID = oItem.OrderRecapID;
                            _oCommercialInvoiceDetail.ShipmentDate = oItem.ShipmentDate;
                            _oCommercialInvoiceDetail.CartonQty = oItem.ShipmentCTNQty;
                            //_oCommercialInvoiceDetail.InvoiceQty = oItem.YetToInvoiceQty;
                            //_oCommercialInvoiceDetail.YetToInvoiceQty = oItem.YetToInvoiceQty;
                            _oCommercialInvoiceDetail.InvoiceQty = oItem.YetToInvoicety;
                            _oCommercialInvoiceDetail.YetToInvoiceQty = oItem.YetToInvoicety;
                            _oCommercialInvoiceDetail.ShipmentQty = oItem.AlreadyShipmentQty;
                            

                            _oCommercialInvoiceDetail.DiscountInPercent = 0;
                            _oCommercialInvoiceDetail.AdditionInPercent = 0;
                            _oCommercialInvoiceDetail.UnitPrice = oItem.UnitPrice;
                            _oCommercialInvoiceDetail.Discount = 0;
                            _oCommercialInvoiceDetail.FOB = oItem.UnitPrice;
                            _oCommercialInvoiceDetail.Amount =(oItem.YetToInvoicety*oItem.UnitPrice);
                            _oCommercialInvoiceDetail.StyleNo = oItem.StyleNo;
                            _oCommercialInvoiceDetail.OrderRecapNo = oItem.OrderRecapNo;
                            _oCommercialInvoiceDetail.ProductName = oItem.ProductName;
                            _oCommercialInvoiceDetail.Fabrication = oItem.FabricName;
                            _oCommercialInvoiceDetail.HSCode = "";
                            _oCommercialInvoiceDetail.CAT = "";
                            _oCommercialInvoiceDetails.Add(_oCommercialInvoiceDetail);
                        }
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommercialInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetExportDocs(CommercialInvoice oCommercialInvoice)
        {
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            string sSQL = "SELECT * FROM View_ExportDocSetup WHERE ExportDocSetupID IN (SELECT ExportDocID FROM ExportDocForwarding WHERE ReferenceID = " + oCommercialInvoice.MasterLCID + " AND RefType =" + (int)EnumMasterLCType.MasterLC + " )  ORDER BY Sequence ASC";

            if (oCommercialInvoice.MasterLCID > 0)
            {
                oExportDocSetups = ExportDocSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oExportDocSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Advance Search
        public ActionResult AdvanceSearch()
        {
            CommercialInvoice oCommercialInvoice = new CommercialInvoice();
            
            return PartialView(oCommercialInvoice);
        }

        #region HttpGet For Search
        [HttpGet]
        public JsonResult Search(string sTemp)
        {
            List<CommercialInvoice> oCommercialInvoices = new List<CommercialInvoice>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oCommercialInvoices = CommercialInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialInvoice = new CommercialInvoice();
                _oCommercialInvoice.ErrorMessage = ex.Message;
                oCommercialInvoices.Add(_oCommercialInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommercialInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            #region Splited Data
            //Issue Date
            int nInvoiceCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dInvoiceStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dInvoiceEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            //Send To Buyer Date
            int nSendToBuyerCreateDateCom = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dSendToBuyerStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dSendToBuyerEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            //Buery Accept Date
            int nBuyerAceptCreateDateCom = Convert.ToInt32(sTemp.Split('~')[6]);
            DateTime dBuyerAcceptStartDate = Convert.ToDateTime(sTemp.Split('~')[7]);
            DateTime dBuyerAcceptEndDate = Convert.ToDateTime(sTemp.Split('~')[8]);
            
            //LCReceiveDate
            int nReceiveCreateDateCom = Convert.ToInt32(sTemp.Split('~')[9]);
            DateTime dReceiveStartDate = Convert.ToDateTime(sTemp.Split('~')[10]);
            DateTime dReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[11]);
            
            //Encashment Date
            int nEnCashmentCreateDateCom = Convert.ToInt32(sTemp.Split('~')[12]);
            DateTime dEnCashmentStartDate = Convert.ToDateTime(sTemp.Split('~')[13]);
            DateTime dEnCashmentEndDate = Convert.ToDateTime(sTemp.Split('~')[14]);


            //Bill of Amount
            int nBillOfAmountCom = Convert.ToInt32(sTemp.Split('~')[15]);
            double nBillOfAmountStart = Convert.ToDouble(sTemp.Split('~')[16]);
            double nBillOfAmountEnd = Convert.ToDouble(sTemp.Split('~')[17]);
            //Descrepency
            int nDescrepencyCom = Convert.ToInt32(sTemp.Split('~')[18]);
            double nDescrepencyChargeStart = Convert.ToDouble(sTemp.Split('~')[19]);
            double nDescrepencyChargeEnd = Convert.ToDouble(sTemp.Split('~')[20]);

            string sMasterLCNo = sTemp.Split('~')[21];
            string sInvoiceNo = sTemp.Split('~')[22];
            string sBuyerIDs = sTemp.Split('~')[23];
            string sAdviceBankAccountIDs = sTemp.Split('~')[24];
            string sIssueBankIDs = sTemp.Split('~')[25];
            int nBusinessSessionID = Convert.ToInt32(sTemp.Split('~')[26]);
            string sInvoiceStatuss = sTemp.Split('~')[27];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[28]);
            #endregion

            string sReturn1 = "SELECT * FROM View_CommercialInvoice";
            string sReturn = "";

            #region LC No

            if (sMasterLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MasterLCNo ='" + sMasterLCNo + "'";

            }
            #endregion

            #region Invoice  No
            if (sInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InvoiceNo = '" + sInvoiceNo + "'";
            }
            #endregion

            #region Buyer wise

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Issue Bank wise

            if (sIssueBankIDs!= "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " IssueBankID IN (" + sIssueBankIDs + ")";
            }
            #endregion

            #region Advice Bank wise

            if (sAdviceBankAccountIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AdviceBankID IN (" + sAdviceBankAccountIDs + ")";
            }
            #endregion

            #region Invoice Status wise
            if (sInvoiceStatuss != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InvoiceStatus IN (" + sInvoiceStatuss + ")";
            }
            #endregion

            #region Issue Date Wise
            if (nInvoiceCreateDateCom > 0)
            {
                if (nInvoiceCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,InvoiceDate,106)  = Convert(Date,'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nInvoiceCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,InvoiceDate,106)  != Convert(Date,'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nInvoiceCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,InvoiceDate,106)  > Convert(Date,'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nInvoiceCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,InvoiceDate,106)  < Convert(Date,'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nInvoiceCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,InvoiceDate,106) >= Convert(Date,'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,InvoiceDate,106)  < Convert(Date,'" + dInvoiceEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
                if (nInvoiceCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,InvoiceDate,106) < Convert(Date,'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,InvoiceDate,106)  > Convert(Date,'" + dInvoiceEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
            }
            #endregion

            #region LC Recive Date Wise
            if (nReceiveCreateDateCom > 0)
            {
                if (nReceiveCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,LCReceiveDate,106)  = Convert(Date,'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nReceiveCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,LCReceiveDate,106)  != Convert(Date,'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nReceiveCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,LCReceiveDate,106)  > Convert(Date,'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nReceiveCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,LCReceiveDate,106)  < Convert(Date,'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nReceiveCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,LCReceiveDate,106) >= Convert(Date,'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,LCReceiveDate,106)  < Convert(Date,'" + dReceiveEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
                if (nReceiveCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,LCReceiveDate,106) < Convert(Date,'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,LCReceiveDate,106)  > Convert(Date,'" + dReceiveEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
            }
            #endregion

            #region Send To Buyer Date Wise
            if (nSendToBuyerCreateDateCom > 0)
            {
                if (nSendToBuyerCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,SendToBuyerDate,106)  = Convert(Date,'" + dSendToBuyerStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nSendToBuyerCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,SendToBuyerDate,106)  != Convert(Date,'" + dSendToBuyerStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nSendToBuyerCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,SendToBuyerDate,106)  > Convert(Date,'" + dSendToBuyerStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nSendToBuyerCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,SendToBuyerDate,106)  < Convert(Date,'" + dSendToBuyerStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nSendToBuyerCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,SendToBuyerDate,106) >= Convert(Date,'" + dSendToBuyerStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,SendToBuyerDate,106)  < Convert(Date,'" + dSendToBuyerEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
                if (nSendToBuyerCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,SendToBuyerDate,106) < Convert(Date,'" + dSendToBuyerStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,SendToBuyerDate,106)  > Convert(Date,'" + dSendToBuyerEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
            }
            #endregion

            #region  Buyer Accept Date Wise
            if (nBuyerAceptCreateDateCom > 0)
            {
                if (nBuyerAceptCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,BuyerAcceptDate,106)  = Convert(Date,'" + dBuyerAcceptStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nBuyerAceptCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,BuyerAcceptDate,106)  != Convert(Date,'" + dBuyerAcceptStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nBuyerAceptCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,BuyerAcceptDate,106)  > Convert(Date,'" + dBuyerAcceptStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nBuyerAceptCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,BuyerAcceptDate,106)  < Convert(Date,'" + dBuyerAcceptStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nBuyerAceptCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,BuyerAcceptDate,106) >= Convert(Date,'" + dBuyerAcceptStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,BuyerAcceptDate,106)  < Convert(Date,'" + dBuyerAcceptEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
                if (nBuyerAceptCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,BuyerAcceptDate,106) < Convert(Date,'" + dBuyerAcceptStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,BuyerAcceptDate,106)  > Convert(Date,'" + dBuyerAcceptEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
            }
            #endregion

            #region  EnCashment Date Wise
            if (nEnCashmentCreateDateCom > 0)
            {
                if (nEnCashmentCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,EncashmentDate,106)  = Convert(Date,'" + dEnCashmentStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nEnCashmentCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,EncashmentDate,106)  != Convert(Date,'" + dEnCashmentStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nEnCashmentCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,EncashmentDate,106)  > Convert(Date,'" + dEnCashmentStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nEnCashmentCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,EncashmentDate,106)  < Convert(Date,'" + dEnCashmentStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nEnCashmentCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,EncashmentDate,106) >= Convert(Date,'" + dEnCashmentStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,EncashmentDate,106)  < Convert(Date,'" + dEnCashmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
                if (nEnCashmentCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,EncashmentDate,106) < Convert(Date,'" + dEnCashmentStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,EncashmentDate,106)  > Convert(Date,'" + dEnCashmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
            }
            #endregion

            #region Bill Of Amount Wise
            if (nBillOfAmountCom > 0)
            {
                if (nBillOfAmountCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetInvoiceAmount  = " + nBillOfAmountStart;
                }
                if (nBillOfAmountCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetInvoiceAmount  != " + nBillOfAmountStart;
                }
                if (nBillOfAmountCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetInvoiceAmount  > " + nBillOfAmountStart;
                }
                if (nBillOfAmountCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetInvoiceAmount  < " + nBillOfAmountStart;
                }
                if (nBillOfAmountCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetInvoiceAmount >= " + nBillOfAmountStart + " AND NetInvoiceAmount  < " + nBillOfAmountEnd;
                }
                if (nBillOfAmountCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetInvoiceAmount <" + nBillOfAmountStart + " OR NetInvoiceAmount  > " + nBillOfAmountEnd;
                }
            }
            #endregion

            #region Descripency Charge Wise
            if (nDescrepencyCom > 0)
            {
                if (nDescrepencyCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DiscrepancyCharge = " + nDescrepencyChargeStart;
                }
                if (nDescrepencyCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DiscrepancyCharge != " + nDescrepencyChargeStart;
                }
                if (nDescrepencyCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DiscrepancyCharge >" + nDescrepencyChargeStart;
                }
                if (nDescrepencyCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DiscrepancyCharge < " + nDescrepencyChargeStart;
                }
                if (nDescrepencyCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DiscrepancyCharge>= " + nDescrepencyChargeStart + " AND DiscrepancyCharge < " + nDescrepencyChargeEnd;
                }
                if (nDescrepencyCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DiscrepancyCharge < " + nDescrepencyChargeStart + "  OR DiscrepancyCharge > " + nDescrepencyChargeEnd;
                }
            }
            #endregion

            #region BusinessSession
            if (nBusinessSessionID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " CommercialInvoiceID IN (SELECT MM.CommercialInvoiceID FROM CommercialInvoiceDetail AS MM WHERE MM.OrderRecapID IN (SELECT SO.OrderRecapID FROM OrderRecap AS SO WHERE SO.BusinessSessionID="+nBusinessSessionID.ToString()+"))";
            }
            #endregion

            #region BU
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BUID = "+nBUID;
            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion


            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

    
        #endregion

        #region Print Document
        public ActionResult PrintDocument()
        {
            return PartialView();
        }
        #endregion

        #region Print

        public ActionResult CommercialInvoiceHistoryPreview(int id)
        {
            List<CommercialInvoiceHistory> _oCommercialInvoiceHistories = new List<ESimSol.BusinessObjects.CommercialInvoiceHistory>();
            _oCommercialInvoiceHistories = ESimSol.BusinessObjects.CommercialInvoiceHistory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptCommercialInvoiceHistory oReport = new rptCommercialInvoiceHistory();
            byte[] abytes = oReport.PrepareReport(_oCommercialInvoiceHistories, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintInvoiceList(string sIDs)
        {
            _oCommercialInvoice = new CommercialInvoice();
            string sSQL = "SELECT * FROM View_CommercialInvoice WHERE CommercialInvoiceID IN (" + sIDs + ")";
            _oCommercialInvoice.CommercialInvoices = CommercialInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptCommercialInvoiceList oReport = new rptCommercialInvoiceList();
            byte[] abytes = oReport.PrepareReport(_oCommercialInvoice, oCompany);
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

        //public ActionResult PrintExportDoc(int id, int nDocType)
        //{
        //    _oCommercialInvoice = new CommercialInvoice();
        //    _oCommercialInvoice = _oCommercialInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oCommercialInvoice.CommercialInvoiceDetails = CommercialInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
        //    _oCommercialInvoice.ExportDocSetup = _oExportDocSetup.GetByDocType(nDocType, (int)Session[SessionInfo.currentUserID]);
        //    _oClientOperationSetting = new ClientOperationSetting();
        //    _oClientOperationSetting = _oClientOperationSetting.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oCommercialInvoice.Company = oCompany;

        //    byte[] abytes;
        //    if ((int)_oCommercialInvoice.ExportDocSetup.DocumentType == (int)EnumDocumentType.Commercial_Invoice)
        //    {
        //        if (_oClientOperationSetting.ExportDocSetupFormat == EnumExportDocSetupFormat.Formate_1)
        //        {
        //            _oMasterLC = new MasterLC();
        //            _oCommercialInvoice.MasterLC = _oMasterLC.Get(_oCommercialInvoice.MasterLCID, (int)Session[SessionInfo.currentUserID]);
        //            _oCommercialInvoice.ErrorMessage = "(" + _oClientOperationSetting.CommercialManager + ")";//value set
        //            rptExportCommercialInvoiceDocFormat1 oReport = new rptExportCommercialInvoiceDocFormat1();
        //            abytes = oReport.PrepareReport(_oCommercialInvoice);
        //        }
        //        else
        //        {
        //            rptExportCommercialInvoiceDoc oReport = new rptExportCommercialInvoiceDoc();
        //            abytes = oReport.PrepareReport(_oCommercialInvoice);
        //        }
        //    }
        //    else if ((int)_oCommercialInvoice.ExportDocSetup.DocumentType == (int)EnumDocumentType.Beneficiary_Certificate)
        //    {
        //        rptExportBenificiarycertificateDoc oReport = new rptExportBenificiarycertificateDoc();
        //        abytes = oReport.PrepareReport(_oCommercialInvoice);
        //    }
        //    else if ((int)_oCommercialInvoice.ExportDocSetup.DocumentType == (int)EnumDocumentType.Certificate_of_Origin)
        //    {
        //        rptExportDocCertificateOfOrigin oReport = new rptExportDocCertificateOfOrigin();
        //        abytes = oReport.PrepareReport(_oCommercialInvoice);
        //    }
        //    else if ((int)_oCommercialInvoice.ExportDocSetup.DocumentType == (int)EnumDocumentType.Applicant_Certificate)
        //    {
        //        rptExportDocApplicantCertificate oReport = new rptExportDocApplicantCertificate();
        //        abytes = oReport.PrepareReport(_oCommercialInvoice);
        //    }
        //    else if ((int)_oCommercialInvoice.ExportDocSetup.DocumentType == (int)EnumDocumentType.Bill_Of_Exchange)
        //    {
        //        rptCommercialInvoiceOfExchangeDoc oReport = new rptCommercialInvoiceOfExchangeDoc();
        //        abytes = oReport.PrepareReport(_oCommercialInvoice);
        //    }
        //    else if ((int)_oCommercialInvoice.ExportDocSetup.DocumentType == (int)EnumDocumentType.Packing_List)
        //    {
        //        _oCommercialInvoice.MasterLC = _oMasterLC.Get(_oCommercialInvoice.MasterLCID, (int)Session[SessionInfo.currentUserID]);
        //        _oCommercialInvoice.ErrorMessage = "(" + _oClientOperationSetting.CommercialManager + ")";//value set
        //        rptExportPackingListDoc oReport = new rptExportPackingListDoc();
        //        abytes = oReport.PrepareReport(_oCommercialInvoice);
        //    }
        //    else
        //    {
        //        return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Report" });
        //    }
        //    return File(abytes, "application/pdf");
        //}

        public System.Drawing.Image GetCompanyTitle(Company oCompany)
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
        public ActionResult PrintExportDoc(int id, int nDocType, int nPrintType, int nUnitType, int nPageSize)
        {
            /*
             * When nUnitType == 1 then "In KG"
             * When nUnitType == 2 then "In LBS"
             * When nPageSize == 1 then "A4"
             * When nPageSize == 2 then "Legal"
             */

            _oCommercialInvoice = new CommercialInvoice();
            _oCommercialInvoice = _oCommercialInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            Company oSubCompany = new Company();
            oCompany = oCompany.Get(_oCommercialInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = this.GetCompanyTitle(oCompany);

            ExportDocSetup oExportDocSetup = new ExportDocSetup();
            oExportDocSetup = oExportDocSetup.Get(nDocType, ((User)Session[SessionInfo.CurrentUser]).UserID);


            //ExportLC oExportLC = new ExportLC();
            //oExportLC = oExportLC.Get(_oCommercialInvoice.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ExportCommercialDoc oExportCommercialDoc = new ExportCommercialDoc();//new ExportCommercialDoc(oExportDocSetup, _oCommercialInvoice);
            oExportCommercialDoc = oExportCommercialDoc.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);



            #region Mapping from Setup
            oExportCommercialDoc.DocumentType = oExportDocSetup.DocumentType;
            oExportCommercialDoc.IsPrintHeader = oExportDocSetup.IsPrintHeader;
            oExportCommercialDoc.DocName = oExportDocSetup.DocName;
            oExportCommercialDoc.BillNo = oExportDocSetup.BillNo;
            oExportCommercialDoc.DocHeader = oExportDocSetup.DocHeader;
            oExportCommercialDoc.Beneficiary = oExportDocSetup.Beneficiary;
            oExportCommercialDoc.NoAndDateOfDoc = oExportDocSetup.NoAndDateOfDoc;
            oExportCommercialDoc.ProformaInvoiceNoAndDate = oExportDocSetup.ProformaInvoiceNoAndDate;
            oExportCommercialDoc.AccountOf = oExportDocSetup.AccountOf;
            oExportCommercialDoc.DocumentaryCreditNoDate = oExportDocSetup.DocumentaryCreditNoDate;
            oExportCommercialDoc.AgainstExportLC = oExportDocSetup.AgainstExportLC;
            oExportCommercialDoc.PortofLoading = oExportDocSetup.PortofLoading;
            oExportCommercialDoc.FinalDestination = oExportDocSetup.FinalDestination;
            oExportCommercialDoc.IssuingBank = oExportDocSetup.IssuingBank;
            oExportCommercialDoc.NegotiatingBank = oExportDocSetup.NegotiatingBank;
            oExportCommercialDoc.ToTheOrderOf = oExportDocSetup.ToTheOrderOf;
            oExportCommercialDoc.CountryofOrigin = oExportDocSetup.CountryofOrigin;
            oExportCommercialDoc.TermsofPayment = oExportDocSetup.TermsofPayment;
            oExportCommercialDoc.AmountInWord = oExportDocSetup.AmountInWord;
            oExportCommercialDoc.Wecertifythat = oExportDocSetup.Wecertifythat;
            oExportCommercialDoc.Certification = oExportDocSetup.Certification;
            oExportCommercialDoc.ClauseOne = oExportDocSetup.ClauseOne;
            oExportCommercialDoc.ClauseTwo = oExportDocSetup.ClauseTwo;
            oExportCommercialDoc.ClauseThree = oExportDocSetup.ClauseThree;
            oExportCommercialDoc.ClauseFour = oExportDocSetup.ClauseFour;
            oExportCommercialDoc.Carrier = oExportDocSetup.Carrier;
            oExportCommercialDoc.Account = oExportDocSetup.Account;
            oExportCommercialDoc.NotifyParty = oExportDocSetup.NotifyParty;
            //oExportCommercialDoc.CompanyName =oExportDocSetup.CompanyName;
            oExportCommercialDoc.DeliveryTo = oExportDocSetup.DeliveryTo;
            oExportCommercialDoc.SellingOnAbout = oExportDocSetup.SellingOnAbout;
            oExportCommercialDoc.IsPrintInvoiceDate = oExportDocSetup.IsPrintInvoiceDate;

            oExportCommercialDoc.AuthorisedSignature = oExportDocSetup.AuthorisedSignature;
            oExportCommercialDoc.ReceiverSignature = oExportDocSetup.ReceiverSignature;
            oExportCommercialDoc.For = oExportDocSetup.For;

            oExportCommercialDoc.FinalDestinationName = oExportDocSetup.FinalDestinationName;
            oExportCommercialDoc.CountryofOriginName = oExportDocSetup.CountryofOriginName;

            oExportCommercialDoc.NetWeight = oExportDocSetup.NetWeightName;
            oExportCommercialDoc.GrossWeight = oExportDocSetup.GrossWeightName;
            oExportCommercialDoc.Bag_Name = oExportDocSetup.Bag_Name;
            oExportCommercialDoc.IsPrintUnitPrice = oExportDocSetup.IsPrintUnitPrice;
            oExportCommercialDoc.IsPrintValue = oExportDocSetup.IsPrintValue;

            oExportCommercialDoc.GarmentsQty_Head = oExportDocSetup.GarmentsQty;
            if (String.IsNullOrEmpty(oExportDocSetup.GarmentsQty)) { oExportCommercialDoc.GarmentsQty = ""; }

            oExportCommercialDoc.Remark_Head = oExportDocSetup.Remark;
            if (String.IsNullOrEmpty(oExportDocSetup.Remark)) { oExportCommercialDoc.Remark = ""; }

            oExportCommercialDoc.SpecialNote_Head = oExportDocSetup.SpecialNote;
            if (String.IsNullOrEmpty(oExportDocSetup.SpecialNote)) { oExportCommercialDoc.SpecialNote = ""; }

            oExportCommercialDoc.AreaCode_Head = oExportDocSetup.AreaCode;
            if (String.IsNullOrEmpty(oExportDocSetup.AreaCode)) { oExportCommercialDoc.AreaCode = ""; }

            oExportCommercialDoc.HSCode_Head = oExportDocSetup.HSCode;
            if (String.IsNullOrEmpty(oExportDocSetup.HSCode)) { oExportCommercialDoc.HSCode = ""; }

            oExportCommercialDoc.IRC_Head = oExportDocSetup.IRC;
            if (String.IsNullOrEmpty(oExportDocSetup.IRC)) { oExportCommercialDoc.IRC = ""; }

            oExportCommercialDoc.Carrier = oExportDocSetup.Carrier;
            if (String.IsNullOrEmpty(oExportDocSetup.Carrier)) { oExportCommercialDoc.CarrierName = ""; }
            if (String.IsNullOrEmpty(oExportCommercialDoc.CarrierName))
            {
                if (_oCommercialInvoice.ProductNature == EnumProductNature.Dyeing)
                {
                    oExportCommercialDoc.CarrierName = "By Truck";
                }
                else
                {
                    oExportCommercialDoc.CarrierName = "Our Own Vehicle";
                }
            }

            oExportCommercialDoc.TruckNo_Print = oExportDocSetup.TruckNo_Print;
            if (String.IsNullOrEmpty(oExportCommercialDoc.TruckNo_Print)) { oExportCommercialDoc.TruckNo_Print = ""; }

            oExportCommercialDoc.To = oExportDocSetup.TO;
            if (String.IsNullOrEmpty(oExportCommercialDoc.To)) { oExportCommercialDoc.To = ""; }

            oExportCommercialDoc.ShippingMark = oExportDocSetup.ShippingMark;
            if (!String.IsNullOrEmpty(oExportCommercialDoc.ShippingMark))
            {
                Contractor oContractor = new Contractor();
                oContractor = oContractor.Get(_oCommercialInvoice.BuyerID, (int)Session[SessionInfo.currentUserID]);
                oExportCommercialDoc.ShippingMarkName = (oContractor.ShortName).ToUpper();
            }

            oExportCommercialDoc.ReceiverCluse = oExportDocSetup.ReceiverCluse;
            if (String.IsNullOrEmpty(oExportCommercialDoc.ReceiverCluse)) { oExportCommercialDoc.ReceiverCluse = ""; }
            oExportCommercialDoc.ForCaptionInDubleLine = oExportDocSetup.ForCaptionInDubleLine;

            //oExportCommercialDoc.NoOfPackages = _oCommercialInvoice.NoOfPackages;
            //oExportCommercialDoc.PlasticNetWeight = _oCommercialInvoice.NetWeight;
            //oExportCommercialDoc.PlasticGrossWeight = _oCommercialInvoice.GrossWeight;

            oExportCommercialDoc.Var_ReqNo_Head = "VAT Reg. No.";
            if (!oExportDocSetup.IsVat) { oExportCommercialDoc.Vat_ReqNo = ""; }
            oExportCommercialDoc.TIN_Head = "TIN.";
            if (!oExportDocSetup.IsRegistration) { oExportCommercialDoc.TIN = ""; }
            if (!oExportDocSetup.IsPrintFrieghtPrepaid) { oExportCommercialDoc.FrightPrepaid = ""; }

            if (oExportDocSetup.IsPrintOriginal == true && oExportCommercialDoc.IsPrintOriginal == true)
            {
                oExportCommercialDoc.Orginal = "Orginal";
            }
            else
            {
                oExportCommercialDoc.Orginal = "";
            }

            oExportCommercialDoc.Term_Head = "Trade Term ";
            if (oExportDocSetup.IsPrintTerm == true && oExportCommercialDoc.IsTerm == true)
            {
                oExportCommercialDoc.Term = oExportCommercialDoc.Term;
            }
            else
            {
                oExportCommercialDoc.Term = "";
            }
            if (oExportDocSetup.IsPrintDeliveryBy == true && oExportCommercialDoc.IsDeliveryBy == true)
            {
                oExportCommercialDoc.DeliveryBy = oExportCommercialDoc.DeliveryBy;
            }
            else
            {
                oExportCommercialDoc.DeliveryBy = "";
            }

            if (oExportDocSetup.IsPrintGrossNetWeight == true && oExportCommercialDoc.IsPrintGrossNetWeight == true)
            {
                oExportCommercialDoc.IsPrintGrossNetWeight = true;
            }
            else
            {
                oExportCommercialDoc.IsPrintGrossNetWeight = false;
            }



            #endregion

            List<MasterLCDetail> oMasterLCDetails = new List<MasterLCDetail>();
            oMasterLCDetails = MasterLCDetail.Gets(_oCommercialInvoice.MasterLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportCommercialDoc.PINos = "";

            foreach (MasterLCDetail oItem in oMasterLCDetails)
            {
                oExportCommercialDoc.PINos = oExportCommercialDoc.PINos + "" + oItem.PINo + " DT : " + oItem.PIIssueDateInString + "\n";
                //oExportCommercialDoc.VersionNo = oItem.VersionNo;
                //oExportCommercialDoc.AmendmentDate = oItem.a;
            }
            if (oExportCommercialDoc.PINos.Length > 0)
            {
                oExportCommercialDoc.PINos = oExportCommercialDoc.PINos.Remove(oExportCommercialDoc.PINos.Length - 1, 1);
            }

            if (oExportCommercialDoc.VersionNo > 0)
            {
                oExportCommercialDoc.AmendmentNonDate = "";
                if (oExportDocSetup.IsShowAmendmentNo == true)
                {
                    oExportCommercialDoc.AmendmentNonDate = ", A. No: " + oExportCommercialDoc.VersionNo + "(" + oExportCommercialDoc.AmendmentDate + ")";
                }
            }
            else
            {
                oExportCommercialDoc.AmendmentNonDate = "";
            }


            oExportCommercialDoc.PortofLoadingName = "Beneficiary Factoy"; ///Add In setup Applicant Factory
            //oExportCommercialDoc.PortofLoadingPoint = "Beneficiary Factoy";
            oExportCommercialDoc.PortofLoadingAddress = "";


            oExportCommercialDoc.CommercialInvoiceDetails = CommercialInvoiceDetail.Gets(_oCommercialInvoice.CommercialInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Set Unit Type
            string sMUName = "LBS";

            /* By Default nUnitType = 2 means LBS*/

            #endregion

            int nMUnitID = 0;
            string sCurrency = "";
            foreach (CommercialInvoiceDetail oItem in oExportCommercialDoc.CommercialInvoiceDetails)
            {
                sMUName = oItem.UnitName;
                nMUnitID = oItem.MeasurementUnitID;
                sCurrency = oItem.CurrencySymbol;
                break;
            }

            if (nMUnitID != 4)
            {
                if (nUnitType == 1)
                {
                    oExportCommercialDoc.CommercialInvoiceDetails.ForEach(o => o.InvoiceQty = Global.GetKG(o.InvoiceQty, 8));
                    oExportCommercialDoc.CommercialInvoiceDetails.ForEach(o => o.UnitPrice = Global.GetLBS(o.UnitPrice, 8));
                    sMUName = "KG";
                }
            }
            //if (nMUnitID == 4)
            //{
            //    oExportCommercialDoc.CommercialInvoiceDetails.ForEach(o => o.QtyTwo = Global.GetMeter(o.InvoiceQty, 8));
            //    sMUName = "Meter";
            //}


            oExportCommercialDoc.SL = "SL";
            oExportCommercialDoc.DescriptionOfGoods = "Description Of Goods";
            oExportCommercialDoc.NoOfBag = "No of Bag";
            oExportCommercialDoc.WtperBag = "Wt/Bag";
            oExportCommercialDoc.MarkSAndNos = "MARKS & NOS";
            oExportCommercialDoc.NetWeight = "NET WEIGHT";
            oExportCommercialDoc.GrossWeight = "GROSS WEIGHT";
            oExportCommercialDoc.Header_Construction = "CONSTRUCTION";

            oExportCommercialDoc.Header_FabricsType = "Fabrics Type";
            oExportCommercialDoc.Header_Color = "COLOR/STYLE";



            if (oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic)
            {
                if (oExportCommercialDoc.WeightPerBag <= 0)
                {
                    oExportCommercialDoc.WeightPerBag = 120;
                }
                if (oExportCommercialDoc.BagWeight <= 0)
                {
                    oExportCommercialDoc.BagWeight = 0.25;
                }
            }



            oExportCommercialDoc.Amount = "Amount";
            if (sMUName == "")
            {
                oExportCommercialDoc.QtyInKg = "Qty";
            }
            else
            {
                oExportCommercialDoc.QtyInKg = "Qty.(" + sMUName + ")";
            }
            oExportCommercialDoc.QtyInLBS = "Qty.(" + sMUName + ")";
            oExportCommercialDoc.ValueDes = "Amount(" + sCurrency + ")";
            oExportCommercialDoc.UnitPriceDes = "Unit Price\n(" + sCurrency + "/" + sMUName + ")";


            oExportCommercialDoc.DocumentsDes = "Documents for which are attached herewith."; // Bill of Exchange
            oExportCommercialDoc.BagCount = 12;
            oExportCommercialDoc.ProductNature = _oCommercialInvoice.ProductNature;
            oExportCommercialDoc.Currency = sCurrency;





            UserImage oUserImage = new UserImage();
            oUserImage = oUserImage.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportCommercialDoc.BOEImage = GetBoEImage(oUserImage);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oExportCommercialDoc.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oExportCommercialDoc.BUAddress = oBusinessUnit.Address;
            oExportCommercialDoc.BUName = oBusinessUnit.Name;
            if (oExportDocSetup.DocumentType == EnumDocumentType.Bill_Of_Exchange)
            {
                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_BillOfExchange(oExportCommercialDoc, oCompany, nPrintType, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Delivery_Challan || oExportDocSetup.DocumentType == EnumDocumentType.Truck_Receipt || oExportDocSetup.DocumentType == EnumDocumentType.Weight_MeaList)
            {
                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_DeliveryChallan(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Commercial_Invoice)
            {

                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_CommercialInvoice(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Beneficiary_Certificate)
            {
                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_BeneficiaryCertificate(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Bank_Submission)
            {
                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_BankForwarding(oExportCommercialDoc, oCompany, nPrintType, _oCommercialInvoice, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Certificate_of_Origin)
            {
                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_CertificateOfORGIN(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Bank_Forwarding)
            {

                List<ExportDocForwarding> oExportDocForwardings = new List<ExportDocForwarding>();
                oExportDocForwardings = ExportDocForwarding.Gets(_oCommercialInvoice.MasterLCID,(int)EnumMasterLCType.MasterLC,  ((User)Session[SessionInfo.CurrentUser]).UserID);

                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_Bank_Forwarding(oExportCommercialDoc, oCompany, nPrintType, oExportDocForwardings, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptExportDocForBuying oReport = new rptExportDocForBuying();
                byte[] abytes = oReport.PrepareReport_CommercialInvoice(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
        }
        public System.Drawing.Image GetBoEImage(UserImage oUserImage)
        {
            if (oUserImage.ImageFile != null)
            {
                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "BoEImage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region MIS Reports
        //MasterLCReport
        public ActionResult CommercialInvoiceReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CommercialInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oCommercialInvoice = new CommercialInvoice();
            _oCommercialInvoice.ReportLayouts = ReportLayout.Gets(EnumModuleName.CommercialInvoice, (int)Session[SessionInfo.currentUserID]);            
            return View(_oCommercialInvoice);
        }
        public ActionResult MISReports(string Param)
        {
            string sCommercialInvoiceIDs = Param.Split('~')[0];
            int nReportType = Convert.ToInt32(Param.Split('~')[1]);
            string sReportNo = Param.Split('~')[2];
            _oCommercialInvoices = new List<CommercialInvoice>();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string sSQL = "SELECT * FROM View_CommercialInvoice WHERE CommercialInvoiceID IN ( " + sCommercialInvoiceIDs + " )";
            switch (nReportType)
            {
                case (int)EnumReportLayout.PartyWise:
                    {
                        sSQL += "Order By BuyerID , MasterLCID, ProductionFactoryID";
                        _oCommercialInvoices = CommercialInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        rptBuyerWiseCInvoice oReport = new rptBuyerWiseCInvoice();
                        byte[] abytes = oReport.PrepareReport(_oCommercialInvoices, oCompany, sReportNo);
                        return File(abytes, "application/pdf");
                    };

                case (int)EnumReportLayout.Factorywise:
                    {
                        sSQL += "Order By ProductionFactoryID, BuyerID, MasterLCID";
                        _oCommercialInvoices = CommercialInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        rptFactoryWiseCI oReport = new rptFactoryWiseCI();
                        byte[] abytes = oReport.PrepareReport(_oCommercialInvoices, oCompany, sReportNo);
                        return File(abytes, "application/pdf");
                    };
                default:
                    return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Report" });
            }

        }

        #endregion
        #endregion
    }

}
