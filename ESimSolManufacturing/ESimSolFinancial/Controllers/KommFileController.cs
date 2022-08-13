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

namespace ESimSolFinancial.Controllers
{
    public class KommFileController : Controller
    {
        #region Declaration
        KommFile _oKommFile = new KommFile();
        List<KommFile> _oKommFiles = new List<KommFile>();
        KommFileImage _oKommFileImage = new KommFileImage();
        #endregion

        #region Functions

        #endregion

        #region Actions
        public ActionResult ViewKommFileList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.KommFile).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oKommFiles = new List<KommFile>();
            string sSQL = "SELECT * FROM View_KommFile AS HH WHERE HH.BUID = " + buid.ToString() + " AND HH.KommFileStatus != 4  Order By VehicleModelID";
            _oKommFiles = KommFile.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.KommFileStatusList = EnumObject.jGets(typeof(EnumKommFileStatus));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oKommFiles);
        }

        public ActionResult ViewKommFile(int id)
        {
            _oKommFile = new KommFile();
            if (id > 0)
            {
                _oKommFile = _oKommFile.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oKommFile.KommFileDetails = KommFileDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.DisplayParts = EnumObject.jGets(typeof(EnumDisplayPart));
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FuelTypes = EnumObject.jGets(typeof(EnumFuelType));
            ViewBag.Sessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BaseCurrencyID = (int)Session[SessionInfo.BaseCurrencyID];
            return View(_oKommFile);
        }

        [HttpPost]
        public JsonResult GetSalesStatus(KommFile oKommFile)
        {
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();
            try
            {
                oSalesQuotations = SalesQuotation.Gets("SELECT * FROM View_SalesQuotation Where SalesStatus ="+(int)EnumSalesStatus.Hold+" AND KommFileID=" + oKommFile.KommFileID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                SalesQuotation oSalesQuotation = new SalesQuotation();
                oSalesQuotation.ErrorMessage = ex.Message;
                oSalesQuotations.Add(oSalesQuotation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesQuotations[0]);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetMKTStatus(KommFile oKommFile)
        {
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();
            try
            {
                oSalesQuotations = SalesQuotation.Gets("SELECT * FROM View_SalesQuotation WHERE KommFileID=" + oKommFile.KommFileID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                SalesQuotation oSalesQuotation = new SalesQuotation();
                oSalesQuotation.ErrorMessage = ex.Message;
                oSalesQuotations.Add(oSalesQuotation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(KommFile oKommFile)
        {
            _oKommFile = new KommFile();
            try
            {
                _oKommFile = oKommFile;
                _oKommFile = _oKommFile.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKommFile = new KommFile();
                _oKommFile.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKommFile);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(KommFile oKommFile)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oKommFile.Delete(oKommFile.KommFileID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateStatus(KommFile oKommFile)
        {
            _oKommFile = new KommFile();
            try
            {
                _oKommFile = oKommFile.UpdateStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKommFile = new KommFile();
                _oKommFile.ErrorMessage = ex.Message;
            }

            if (_oKommFile.KommFileID <= 0 && string.IsNullOrEmpty(_oKommFile.ErrorMessage))
                _oKommFile.ErrorMessage = "Invalid Operation";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKommFile);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult KommFileSearchByNo(KommFile oKommFile)
        {
            _oKommFiles = new List<KommFile>();
            try
            {
                string sSQL = "SELECT * FROM View_KommFile WHERE KommNo LIKE '%"+oKommFile.KommNo+"%' AND BUID = "+oKommFile.BUID;
                _oKommFiles = KommFile.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKommFile = new KommFile();
                _oKommFile.ErrorMessage = ex.Message;
                _oKommFiles.Add(_oKommFile);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKommFiles);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByRef(KommFile oKommFile)
        {
            _oKommFiles = new List<KommFile>();
            try
            {
                string sSQL = "SELECT * FROM View_KommFile WHERE EngineID <> 0";

                if (oKommFile.EngineID>0)
                    sSQL += " AND EngineID =" + oKommFile.EngineID;
                if (oKommFile.ChassisID>0)
                    sSQL += " AND ChassisID =" + oKommFile.ChassisID;
                
                _oKommFiles = KommFile.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKommFile = new KommFile();
                _oKommFile.ErrorMessage = ex.Message;
                _oKommFiles.Add(_oKommFile);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKommFiles);
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
        public JsonResult GetKommFileDetailsByOrderID(KommFileDetail oKommFileDetail)
        {
            List<KommFileDetail> oKommFileDetails = new List<KommFileDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_VehicleOrderDetail WHERE VehicleOrderID=" + oKommFileDetail.KommFileID;
                oKommFileDetails = KommFileDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oKommFileDetail = new KommFileDetail();
                oKommFileDetail.ErrorMessage = ex.Message;
                oKommFileDetails.Add(oKommFileDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKommFileDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ADV SEARCH

        [HttpPost]
        public JsonResult AdvSearch(KommFile oKommFile)
        {
            List<KommFile> oKommFiles = new List<KommFile>();
            KommFile _oKommFile = new KommFile();
            string sSQL = MakeSQL(oKommFile);
            if (sSQL == "Error")
            {
                _oKommFile = new KommFile();
                _oKommFile.ErrorMessage = "Please select a searching critaria.";
                oKommFiles = new List<KommFile>();
            }
            else
            {
                oKommFiles = new List<KommFile>();
                oKommFiles = KommFile.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oKommFiles.Count == 0)
                {
                    oKommFiles = new List<KommFile>();
                }
            }
            var jsonResult = Json(oKommFiles, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(KommFile oKommFile)
        {
            string sParams = oKommFile.Params;

            int nDateCriteria_Issue = 0,
                nStatus=0;
            
            string sSLNo = "",  
                   sKommNo = "", 
                   sModelIDs = "";
            
            DateTime dStart_Issue = DateTime.Today,
                     dEnd_Issue = DateTime.Today;
        
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sSLNo = sParams.Split('~')[nCount++];
                sKommNo = sParams.Split('~')[nCount++];
                nStatus = Convert.ToInt32(sParams.Split('~')[nCount++]);
                nDateCriteria_Issue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sModelIDs = sParams.Split('~')[nCount++];
            }

            string sReturn1 = "SELECT * FROM View_KommFile AS EB";
            string sReturn = "";

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.IssueDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            #endregion

            #region FileNo
            if (!string.IsNullOrEmpty(sSLNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.FileNo LIKE '%" + sSLNo + "%'";
            }
            #endregion

            #region KommNo
            if (!string.IsNullOrEmpty(sKommNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.KommNo LIKE '%" + sKommNo + "%'";
            }
            #endregion

            #region Status
            if (nStatus>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.KommFileStatus =" + nStatus;
            }
            #endregion

            #region Model IDs
            if (!string.IsNullOrEmpty(sModelIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.VehicleModelID IN (" + sModelIDs + ") ";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region PrintList
        public ActionResult PrintList(string sIDs)
        {
            _oKommFile = new KommFile();
            string sSQL = "SELECT * FROM View_KommFile WHERE KommFileID IN (" + sIDs + ") ORDER BY KommFileID  ASC";
            _oKommFile.KommFileList = KommFile.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptKommFileList oReport = new rptKommFileList();
            byte[] abytes = oReport.PrepareReport(_oKommFile, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintKommFilePreview(int id)
        {
            VehicleOrderImage oVehicleOrderImage=new VehicleOrderImage();
            _oKommFile = new KommFile();
            _oKommFile = _oKommFile.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oKommFile.VehicleOrderImage = oVehicleOrderImage.GetFrontImage(_oKommFile.VehicleOrderID, (int)Session[SessionInfo.currentUserID]);
            _oKommFile.KommFileDetails = KommFileDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptKommFile oReport = new rptKommFile();
            byte[] abytes = oReport.PrepareReport(_oKommFile, oCompany);
            return File(abytes, "application/pdf");
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
        #endregion

        #region Image Parts
        public ViewResult ImageHelper(int id, string ms)
        {
            _oKommFileImage = new KommFileImage();
            _oKommFileImage.KommFileID = id;
            List<KommFileThumbnail> oKommFileThumbnails = new List<KommFileThumbnail>();
            oKommFileThumbnails = KommFileThumbnail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            //foreach (KommFileThumbnail oItem in oKommFileThumbnails)
            //{
            //    oItem.ThumImage = GetImage(oItem.ThumbnailImage);
            //}
            _oKommFileImage.KommFileThumbnails = oKommFileThumbnails;
            _oKommFileImage.ImageTypeObjs = EnumObject.jGets(typeof(EnumImageType));
            TempData["message"] = ms;
            return View(_oKommFileImage);
        }


        [HttpPost]
        public ActionResult ImageHelper(HttpPostedFileBase file, KommFileImage oKommFileImage)
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
                else if (oKommFileImage.ImageTitle == null || oKommFileImage.ImageTitle == "")
                {
                    sErrorMessage = "Please enter image title!";
                }
                else
                {
                    oKommFileImage.LargeImage = aImageInByteArray;
                    oKommFileImage.KommFileThumbnailID = 0;
                    oKommFileImage.ThumbnailImage = aThumbnailImageInByteArray;
                    oKommFileImage = oKommFileImage.Save((int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                sErrorMessage = "Please select an image!";
            }
            return RedirectToAction("ImageHelper", new { id = oKommFileImage.KommFileID, ms = sErrorMessage });
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        [HttpGet]
        public JsonResult DeleteKommFileImage(int id)
        {
            KommFileImage oKommFileImage = new KommFileImage();
            KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
            oKommFileImage = oKommFileImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            string sErrorMease = "";
            int nId = 0;
            nId = oKommFileImage.KommFileID;
            try
            {
                oKommFileImage.Delete(id, (int)Session[SessionInfo.currentUserID]);
                oKommFileThumbnail.Delete(id, (int)Session[SessionInfo.currentUserID]);
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

        public Image GetLargeImage(int id)
        {
            KommFileImage oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.GetImageByType(id, (int)EnumImageType.ModelImage, (int)Session[SessionInfo.currentUserID]);
            if (oKommFileImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oKommFileImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetThumImage(int id)
        {
            KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
            oKommFileThumbnail = oKommFileThumbnail.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oKommFileThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oKommFileThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public ActionResult DownloadModelImage(int id, double ts)
        {
            KommFileImage oKommFileImage = new KommFileImage();
            try
            {
                oKommFileImage.KommFileImageID = id;
                oKommFileImage = oKommFileImage.Get(id, (int)Session[SessionInfo.currentUserID]);
                if (oKommFileImage.LargeImage != null)
                {
                    var file = File(oKommFileImage.LargeImage, "image/jpeg");
                    file.FileDownloadName = "Image #" + oKommFileImage.KommFileImageID;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + "Your Image");
            }
        }


        [HttpGet]
        public ActionResult GetImageInBase64(int id)
        {
            KommFileImage oKommFileImage = new KommFileImage();
            oKommFileImage = oKommFileImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oKommFileImage.LargeImage == null)
            {
                oKommFileImage.LargeImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oKommFileImage.LargeImage) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print With Excel
        public void PrintKommFileXL(int id)
        {
            KommFile oKommFile = new KommFile();
            List<KommFileDetail> oKommFileDetails = new List<KommFileDetail>();

            oKommFile = oKommFile.Get(id, (int)Session[SessionInfo.currentUserID]);
            oKommFileDetails = KommFileDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);


            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Komm File");
                sheet.Name = "Komm File";

                sheet.Column(1).Width = 15;//Caption
                sheet.Column(2).Width = 40;//Value
                sheet.Column(3).Width = 15;//Option
                nMaxColumn = 3;

                int rowIndex = 0;
                int colIndex = 1;
                int nfontSize = 12;
                double nRH = 12;

                #region Model
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Model"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oKommFile.ModelNo; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = " "; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                #endregion

                #region Code
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                string sCode = "XXXX";
                if (oKommFile.RefNo != "")
                {
                    sCode = oKommFile.RefNo;
                }
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sCode; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = " "; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                #endregion

                #region ChassisNo
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Chassis No."; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                string sChassisNo = "-";
                if (oKommFile.ChassisNo !="")
                {
                    sChassisNo = oKommFile.ChassisNo;
                }
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sChassisNo; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                #endregion

                #region EngineNo
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Engine No."; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                string sEngineNo = "-";
                if (oKommFile.EngineNo !="")
                {
                    sEngineNo = oKommFile.EngineNo;
                }
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sEngineNo; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                #endregion

                #region Exterior Color
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Exterior Color"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oKommFile.ExteriorColorName + "   (" + oKommFile.ExteriorColorCode + ")"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = " "; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                #endregion

                #region Interior Color
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Interior Color"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oKommFile.InteriorColorName + "   (" + oKommFile.InteriorColorCode + ")"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = " "; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                #endregion

                #region E.T.A
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "E.T.A"; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                string sETA = "XXXX Weeks";
                if (oKommFile.ETAValue > 0)
                {
                    sETA = oKommFile.ETAValue.ToString() + " " + oKommFile.ETATypeInString;
                }
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sEngineNo; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 3;
                #endregion

                #region FeatureSetupName
                colIndex = 1;
                //sheet.Row(rowIndex).Height = nRH;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oKommFile.FeatureSetupName; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = " "; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                #endregion

                #region Optional Feature
                foreach (KommFileDetail oItem in oKommFileDetails)
                {
                    colIndex = 1;
                    //sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FeatureCode; cell.Style.Font.Bold = false;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FeatureName; cell.Style.Font.Bold = false;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "option"; cell.Style.Font.Bold = false;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                    rowIndex = rowIndex + 1;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=OrderFile.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void PrintKommFileListXL() 
        {
            #region Get Data
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            
            string sSQL = "SELECT * FROM View_KommFile ORDER BY FileNo";
            _oKommFiles = KommFile.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            
            //ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            //oClientOperationSetting = oClientOperationSetting.Get(1, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region XL LIST
           
            int nRowIndex = 0, nStartRow = 0, nEndRow = 0, nStartCol = 0, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Komm File List");
                sheet.Name = "Komm File List";

                int nCloumn = 0;

                sheet.Column(++nCloumn).Width = 6;   // SL NO
                sheet.Column(++nCloumn).Width = 20;   // Komm NO
                sheet.Column(++nCloumn).Width = 15;  // Model
                sheet.Column(++nCloumn).Width = 15;  // Client
                sheet.Column(++nCloumn).Width = 20;  // Exterrior color Name
                sheet.Column(++nCloumn).Width = 20;  // Interiror color Name
                sheet.Column(++nCloumn).Width = 25;   // Option included
                sheet.Column(++nCloumn).Width = 20;  // Option Price
                sheet.Column(++nCloumn).Width = 20; // Option Total 
                sheet.Column(++nCloumn).Width = 20; // Unit Price
                sheet.Column(++nCloumn).Width = 20; // Total 
                sheet.Column(++nCloumn).Width = 20; // Vat
                sheet.Column(++nCloumn).Width = 20; // REG
                sheet.Column(++nCloumn).Width = 25; //OTR
                nEndCol = nCloumn;

                #region Report Header
                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                //cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //nRowIndex = nRowIndex + 1;

                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                //cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //nRowIndex = nRowIndex + 1;

                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                //cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //nRowIndex = nRowIndex + 2;

                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                //cell.Value = "KOMM FILE LIST"; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                //nRowIndex = nRowIndex + 1;
                #endregion

                #region Column Header
                nRowIndex = nRowIndex + 1;
                nStartRow = nRowIndex;
                int nHeaderIndex = 0;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Komm No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;
                
                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Model"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Client"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Exterior Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Interior Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Option Included"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Option Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Option Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Vat"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Ref Fee"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "OTR"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 21];
                nRowIndex = nRowIndex + 1;
                #endregion

                int nCount = 0, nStratRow = 0;
                double nTotalOptionPrice = 0;
               // double nTotalOrderQty = 0, nTotalCuttingQty = 0, nTotalSweeingQty = 0, nTotalShipmentQty = 0;
                foreach (KommFile oItem in _oKommFiles)
                {
                    nCount++;
                    int nStartColIndex = 0;
                    int nRowSpan = 0;
                    bool isMerge=false;
                    KommFileDetail oKommFileDetail=new KommFileDetail();
                    List<KommFileDetail> oKommFileDetails = new List<KommFileDetail>();
                    oKommFileDetails = KommFileDetail.Gets(oItem.KommFileID, (int)Session[SessionInfo.currentUserID]);
                    if (oKommFileDetails == null)
                        oKommFileDetails[0]=oKommFileDetail;
                    if (oKommFileDetails.Count > 0)
                    {
                        isMerge = true;
                        nRowSpan=oKommFileDetails.Count - 1;
                    }

                    #region Data
                    int nDataIndex = 0;
                    nStratRow = nRowIndex;
                    cell = sheet.Cells[nRowIndex, ++nDataIndex, nRowIndex +nRowSpan, nDataIndex]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Numberformat.Format = "###0;(###0)"; cell.Merge = isMerge;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex, nRowIndex +nRowSpan, nDataIndex]; cell.Value = oItem.FileNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = isMerge; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex, nRowIndex +nRowSpan, nDataIndex]; cell.Value = oItem.ModelNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = isMerge; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex, nRowIndex +nRowSpan, nDataIndex]; cell.Value = "Stock"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = isMerge; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex, nRowIndex +nRowSpan, nDataIndex]; cell.Value = oItem.ExteriorColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = isMerge; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex, nRowIndex +nRowSpan, nDataIndex]; cell.Value = oItem.InteriorColorName; cell.Merge = isMerge; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style =  ExcelBorderStyle.Thick;

                    nStartColIndex = ++nDataIndex; 
                    int nDetailCount=0;
                    foreach (KommFileDetail oDetail in oKommFileDetails)
                    {
                        nDetailCount++;
                        cell = sheet.Cells[nRowIndex, nStartColIndex]; cell.Value = oDetail.FeatureName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        if (nDetailCount==oKommFileDetails.Count)
                        border = cell.Style.Border; border.Bottom.Style =  ExcelBorderStyle.Thick;

                        cell = sheet.Cells[nRowIndex, nStartColIndex + 1]; cell.Value = oDetail.Price; cell.Style.Font.Bold = false; cell.Style.WrapText = true; 
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        
                        if (nDetailCount == oKommFileDetails.Count)
                        border = cell.Style.Border; border.Bottom.Style =  ExcelBorderStyle.Thick;
                        nTotalOptionPrice += oDetail.Price;
                        nRowIndex++; 
                    }
                    nRowIndex--;

                    cell = sheet.Cells[nRowIndex, nDataIndex+=2]; cell.Value = nTotalOptionPrice; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";                   
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; cell.Merge = isMerge; border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; cell.Merge = isMerge; border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = (oItem.UnitPrice + nTotalOptionPrice);
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; cell.Merge = isMerge; border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.VatInPercent; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; cell.Merge = isMerge; border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.RegistrationFeePercent; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = isMerge; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style =  ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = (oItem.UnitPrice + nTotalOptionPrice + oItem.VatInPercent + oItem.RegistrationFeePercent);
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.Font.Bold = false; cell.Style.WrapText = true; cell.Merge = isMerge;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style =  ExcelBorderStyle.Thick;
                    nTotalOptionPrice = 0;
                    #endregion
                    nEndRow = nRowIndex;
                    nRowIndex++;
                }

                #region Grand Total
                //int nGTotalIndex = 15;
                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nGTotalIndex];
                //if (oCompany.CompanyID <= 0)
                //    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, ++nGTotalIndex];

                //cell.Merge = true;
                //cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, ++nGTotalIndex]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                //cell.Style.Font.UnderLine = true;
                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, ++nGTotalIndex]; cell.Value = nTotalCuttingQty; cell.Style.Font.Bold = true;
                //cell.Style.Font.UnderLine = true;
                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, ++nGTotalIndex]; cell.Value = nTotalSweeingQty; cell.Style.Font.Bold = true;
                //cell.Style.Font.UnderLine = true;
                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //cell = sheet.Cells[nRowIndex, ++nGTotalIndex]; cell.Value = nTotalShipmentQty; cell.Style.Font.Bold = true;
                //cell.Style.Font.UnderLine = true;
                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, ++nGTotalIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //nRowIndex++;
                #endregion

                //cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                ////fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=KommFileList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
    }

}
