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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Image = System.Drawing.Image;

namespace ESimSolFinancial.Controllers
{
    public class ClientOperationSettingController:Controller
    {
        #region Declaration
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        List<ClientOperationSetting> _oClientOperationSettings = new List<ClientOperationSetting>();
        COSImage _oCOSImage = new COSImage();
        List<COSImage> _oCOSImages = new List<COSImage>();
        ThemeUpload _oThemeUpload = new ThemeUpload();
        #endregion

        #region ClientOperationSetting
        public ActionResult ViewClientOperationSettings(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ClientOperationSetting).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oClientOperationSetting = new ClientOperationSetting();
            _oClientOperationSettings = ClientOperationSetting.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oClientOperationSettings);
        }

        public ActionResult ViewClientOperationSetting(int id)
        {            
            _oClientOperationSetting = new ClientOperationSetting();
            _oClientOperationSetting = _oClientOperationSetting.Get( id,(int)Session[SessionInfo.currentUserID]);
            ViewBag.OperationTypes = EnumObject.jGets(typeof(EnumOperationType));
            ViewBag.DataTypes = EnumObject.jGets(typeof(EnumDataType));
            return View(_oClientOperationSetting);
        }

        [HttpPost]
        public JsonResult GetClientOperationSetupEnumList(ClientOperationSetting oClientOperationSetting)
        {
            EnumObject oClientOperationValueFormat = new EnumObject();
            List<EnumObject> oClientOperationValueFormats = new  List<EnumObject>();            
            try
            {
                EnumOperationType eOperationType = (EnumOperationType)oClientOperationSetting.OperationTypeInInt;
                List<EnumObject> oEnumObjects = EnumObject.jGets(typeof(EnumClientOperationValueFormat));
                foreach (EnumObject oItem in oEnumObjects)
                {
                    switch (eOperationType)
                    {
                        case EnumOperationType.OrderSheet_PreviewFormat:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Default || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Format_1 || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Format_2 || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Format_3)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;

                        case EnumOperationType.LCTransfer_PreviewFormat:
                        case EnumOperationType.PurchaseOrderReportFormat:
                        case EnumOperationType.WorkOrder_Normal_Format:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Default || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Format_1 || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Format_2)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;

                        case EnumOperationType.Export_Doc_Setup_Format:
                        
                        case EnumOperationType.PurchaseRequisitionReportFormat:
                        case EnumOperationType.OrderRecap_Report_Format:
                        case EnumOperationType.NOAReportFormat:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Default || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Format_1)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }                            
                            break;
                        case EnumOperationType.PIFormat:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Country_Shortname_BuyerShortName_No_Year || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.CountryShortname_BuyerShortName_DAte_Session || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Manual)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }                            
                            break;

                        case EnumOperationType.Way_Of_LCTransfer:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Percent_Base || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.FOB_Base)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }                            
                            break;

                        case EnumOperationType.PIOperation:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Default || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.With_Appllicant_And_DiscountAmount)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }                             
                            break;

                        case EnumOperationType.LCTransferType:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.PartialValue || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.FullValue)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }                             
                            break;

                        case EnumOperationType.VoucherFormat:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.SingleCurrencyVoucher || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.MultiCurrencyVoucher)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }                            
                            break;
                        case EnumOperationType.CommercialInvoiceFormat:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Buying_Format || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Garments_Format)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;
                        case EnumOperationType.StockReportFormat:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.StockReportWithQty || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.StockReportWithValue)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;
                        case EnumOperationType.BanglaFont:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Bijoy || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Avro)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;
                        case EnumOperationType.Sub_Ledger_Report_Format:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.TextWise || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.ColumnWise)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;
                        case EnumOperationType.Comprehensive_Income_Satatement_Format:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Manufactureing_Format || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Trading_Format)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;
                        case EnumOperationType.BanglaOrEnglish:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.English || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.Bangla)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;
                        case EnumOperationType.Commercial_Module_Using_System:
                            if ((EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.PIWise || (EnumClientOperationValueFormat)oItem.id == EnumClientOperationValueFormat.WithoutPIWise)
                            {
                                oClientOperationValueFormats.Add(oItem);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                oClientOperationValueFormat = new EnumObject(); ;
                oClientOperationValueFormat.Value = ex.Message;
                oClientOperationValueFormats.Add(oClientOperationValueFormat);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oClientOperationValueFormats);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(ClientOperationSetting oClientOperationSetting)
        {
            _oClientOperationSetting = new ClientOperationSetting();
            try
            {
                _oClientOperationSetting = oClientOperationSetting;
                _oClientOperationSetting.OperationType = (EnumOperationType)oClientOperationSetting.OperationTypeInInt;
                _oClientOperationSetting.DataType = (EnumDataType)oClientOperationSetting.DataTypeInInt;
                _oClientOperationSetting = _oClientOperationSetting.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oClientOperationSetting = new ClientOperationSetting();
                _oClientOperationSetting.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oClientOperationSetting);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = _oClientOperationSetting.Delete(id, (int)Session[SessionInfo.currentUserID]);
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

        #region Image load
        public ActionResult ViewClientOperationSettingAttachment(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ClientOperationSetting).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oCOSImages = new List<COSImage>();
            _oCOSImages = COSImage.Gets((int)Session[SessionInfo.currentUserID]);
            //ViewBag.DataTypes = EnumObject.jGets(typeof(EnumClientOperationValueFormat));
            return View(_oCOSImages);
        }

        public ViewResult ImageHelper(int id, string ms, string sSavedMessag)
        {
            _oCOSImage = new COSImage();
            _oCOSImage.COSImageID = id;
            _oCOSImage = _oCOSImage.Get(id,(int)Session[SessionInfo.currentUserID]);
            _oCOSImage.OperationTypes = EnumObject.jGets(typeof(EnumOperationType));
            _oCOSImage.COSVFormats = EnumObject.jGets(typeof(EnumClientOperationValueFormat));
            TempData["message"] = ms;
            _oCOSImage.ErrorMessage= sSavedMessag;
            return View(_oCOSImage);
        }


        [HttpPost]
        public ActionResult ImageHelper(HttpPostedFileBase file, COSImage oCOSImage)
        {

            // Verify that the user selected a file
            string sErrorMessage = "", SavedMessag = "";
            if (file != null && file.ContentLength > 0 )
            {
                Image oImage = Image.FromStream(file.InputStream, true, true);
                //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                //Orginal Image to byte array
                byte[] aImageInByteArray = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    oImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    aImageInByteArray = ms.ToArray();
                }

                double nMaxLength = 1000 * 1024;// File Size upto 1 MB
                if (aImageInByteArray.Length > nMaxLength)
                {
                    sErrorMessage = "You can selecte maximum 1MB image";
                }
                else if (oCOSImage.ImageTitle == null || oCOSImage.ImageTitle == "")
                {
                    sErrorMessage = "Please enter image title!";
                }
                else
                {
                    oCOSImage.LargeImage = aImageInByteArray;
                    oCOSImage = oCOSImage.Save((int)Session[SessionInfo.currentUserID]);
                    if ((oCOSImage.ErrorMessage == null || oCOSImage.ErrorMessage == ""))
                    {
                        SavedMessag = "Succefully Saved";
                    }
                    else
                    {
                        sErrorMessage = oCOSImage.ErrorMessage;
                    }
                    
                }
            }
            else if(oCOSImage.COSImageID>0)//for edit
            {

                oCOSImage.LargeImage = null;
                oCOSImage = oCOSImage.Save((int)Session[SessionInfo.currentUserID]);
                if ((oCOSImage.ErrorMessage == null || oCOSImage.ErrorMessage == ""))
                {
                    SavedMessag = "Succefully Saved";
                }
                else
                {
                    sErrorMessage = oCOSImage.ErrorMessage;
                }
            }
            else
            {
                sErrorMessage = "Please select an image!";
            }
            return RedirectToAction("ImageHelper", new { id = oCOSImage.COSImageID, ms = sErrorMessage, sSavedMessag = SavedMessag });
        }

        public Image GetLargeImage(int id)
        {
            COSImage oCOSImage = new COSImage();
            oCOSImage = oCOSImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oCOSImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oCOSImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult DeleteCOSImage(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = _oCOSImage.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStyleImageInBase64(int nOperationType, int nCOSFormat)
        {
            COSImage oCOSImage = new COSImage();
            oCOSImage = oCOSImage.GetByOperationAndCOSFormat(nOperationType, nCOSFormat, (int)Session[SessionInfo.currentUserID]);
            if (oCOSImage.LargeImage == null)
            {
                oCOSImage.LargeImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oCOSImage.LargeImage) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region theme upload

        public ActionResult ViewThemeUpload(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'ThemeUpload'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oThemeUpload = new ThemeUpload();
            //_oThemeUpload = _oThemeUpload.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(_oThemeUpload);
        }

        public JsonResult SaveThemeUpload(double nts)
        {
            string sMessage = "", ext = "";
            ThemeUpload oThemeUpload = new ThemeUpload();

            try
            {
                string fileName = Convert.ToString(Request.Headers["FileName"]);
                //fileName = fileName.Split('.')[0];
                //oThemeUpload.FileName = Convert.ToString(Request.Headers["FileName"]);  string sTaskNo = sTemp.Split('~')[0];

                byte[] data;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = null;
                    if (Request.Files.Count == 1)
                    {
                        file = Request.Files[0];
                    }
                    else
                    {
                        if (Convert.ToBoolean(Request.Headers["IsFile"]) == true)
                        {
                            file = Request.Files[0];
                        }
                    }

                    if (file != null)
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
                            
                            ext = ".jpg";      //Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(Server.MapPath("~/Content/Images"), "BGTheme" + ext);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            file.SaveAs(filePath);
                            sMessage = "Save Successfully" + "~" + "BGTheme" + ext;
                        }
                        oThemeUpload.File = data;
                        oThemeUpload.FileName = "BGTheme" + ext;
                    }
                }

            }
            catch (Exception ex)
            {
                oThemeUpload = new ThemeUpload();
                oThemeUpload.ErrorMessage = ex.Message;
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}