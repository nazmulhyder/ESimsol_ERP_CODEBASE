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
	public class VehicleOrderController : Controller
	{
		#region Declaration
		VehicleOrder _oVehicleOrder = new VehicleOrder();
		List<VehicleOrder> _oVehicleOrders = new  List<VehicleOrder>();
        VehicleOrderImage _oVehicleOrderImage = new VehicleOrderImage();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewVehicleOrderList(int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oVehicleOrders = new List<VehicleOrder>();
            _oVehicleOrders = VehicleOrder.BUWiseGets( buid, (int)Session[SessionInfo.currentUserID]);
			return View(_oVehicleOrders);
		}
		public ActionResult ViewVehicleOrder(int id)
		{
			_oVehicleOrder = new VehicleOrder();
			if (id > 0)
			{
				_oVehicleOrder = _oVehicleOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oVehicleOrder.VehicleOrderDetails = VehicleOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.DisplayParts= EnumObject.jGets(typeof(EnumDisplayPart));
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FuelTypes = EnumObject.jGets(typeof(EnumFuelType));
            ViewBag.Sessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oVehicleOrder);
		}

		[HttpPost]
		public JsonResult Save(VehicleOrder oVehicleOrder)
		{
			_oVehicleOrder = new VehicleOrder();
			try
			{
				_oVehicleOrder = oVehicleOrder;
				_oVehicleOrder = _oVehicleOrder.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oVehicleOrder = new VehicleOrder();
				_oVehicleOrder.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oVehicleOrder);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
        public JsonResult Delete(VehicleOrder oVehicleOrder)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oVehicleOrder.Delete(oVehicleOrder.VehicleOrderID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetFeatures(Feature oFeature)
        {
            List<Feature> oFeatures = new List<Feature>();
            try
            {
                oFeatures = Feature.GetsbyFeatureNameWithType(oFeature.FeatureName, oFeature.Remarks, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsOrderByRefNo(VehicleOrder oVehicleOrder)
        {
            List<VehicleOrder> oVehicleOrders = new List<VehicleOrder>();
            try
            {
                //--Remarks send ad Types
                string sSQL = "SELECT * FROM View_VehicleOrder WHERE (ISNULL(RefNo,'')+ISNULL(ModelNo,'')+ISNULL(EngineNo,'')+ISNULL(ChassisNo,'')) LIKE '%" + oVehicleOrder.RefNo + "%'";
                oVehicleOrders = VehicleOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oVehicleOrder = new VehicleOrder();
                oVehicleOrder.ErrorMessage = ex.Message;
                oVehicleOrders.Add(oVehicleOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region Image Parts
        public ViewResult ImageHelper(int id, string ms)
        {
            _oVehicleOrderImage = new VehicleOrderImage();
            _oVehicleOrderImage.VehicleOrderID = id;
            List<VehicleOrderThumbnail> oVehicleOrderThumbnails = new List<VehicleOrderThumbnail>();
            oVehicleOrderThumbnails = VehicleOrderThumbnail.Gets(id, (int)Session[SessionInfo.currentUserID]);
       
            _oVehicleOrderImage.VehicleOrderThumbnails = oVehicleOrderThumbnails;
            _oVehicleOrderImage.ImageTypeObjs = EnumObject.jGets(typeof(EnumImageType));
            TempData["message"] = ms;
            return View(_oVehicleOrderImage);
        }

        [HttpPost]
        public ActionResult ImageHelper(HttpPostedFileBase file, VehicleOrderImage oVehicleOrderImage)
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
                else if (oVehicleOrderImage.ImageTitle == null || oVehicleOrderImage.ImageTitle == "")
                {
                    sErrorMessage = "Please enter image title!";
                }
                else
                {
                    oVehicleOrderImage.LargeImage = aImageInByteArray;
                    oVehicleOrderImage.VehicleOrderThumbnailID = 0;
                    oVehicleOrderImage.ThumbnailImage = aThumbnailImageInByteArray;
                    oVehicleOrderImage = oVehicleOrderImage.Save((int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                sErrorMessage = "Please select an image!";
            }
            return RedirectToAction("ImageHelper", new { id = oVehicleOrderImage.VehicleOrderID, ms = sErrorMessage });
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        [HttpGet]
        public JsonResult DeleteVehicleOrderImage(int id)
        {
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
            oVehicleOrderImage = oVehicleOrderImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            string sErrorMease = "";
            int nId = 0;
            nId = oVehicleOrderImage.VehicleOrderID;
            try
            {
                oVehicleOrderImage.Delete(id, (int)Session[SessionInfo.currentUserID]);
                oVehicleOrderThumbnail.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            if (id > 0)
            {
                oVehicleOrderImage = oVehicleOrderImage.GetImageByType(id, (int)EnumImageType.ModelImage, (int)Session[SessionInfo.currentUserID]);
            }
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

        public Image GetThumImage(int id)
        {
            VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
            oVehicleOrderThumbnail = oVehicleOrderThumbnail.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oVehicleOrderThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oVehicleOrderThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public ActionResult DownloadTSImage(int id, double ts)
        {
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            try
            {
                oVehicleOrderImage.VehicleOrderImageID = id;
                oVehicleOrderImage = oVehicleOrderImage.Get(id, (int)Session[SessionInfo.currentUserID]);
                if (oVehicleOrderImage.LargeImage != null)
                {
                    var file = File(oVehicleOrderImage.LargeImage, "image/jpeg");
                    file.FileDownloadName = "Image #" + oVehicleOrderImage.VehicleOrderImageID;
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
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            oVehicleOrderImage = oVehicleOrderImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oVehicleOrderImage.LargeImage == null)
            {
                oVehicleOrderImage.LargeImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oVehicleOrderImage.LargeImage) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PrintList
        public ActionResult PrintList(string sIDs)
        {
            _oVehicleOrder = new VehicleOrder();
            string sSQL = "SELECT * FROM View_VehicleOrder WHERE VehicleOrderID IN (" + sIDs + ") ORDER BY VehicleOrderID  ASC";
            _oVehicleOrder.VehicleOrderList = VehicleOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptVehicleOrderList oReport = new rptVehicleOrderList();
            byte[] abytes = oReport.PrepareReport(_oVehicleOrder, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintVehicleOrderPreview(int id)
        {
            _oVehicleOrder = new VehicleOrder();
            _oVehicleOrder = _oVehicleOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oVehicleOrder.VehicleOrderImage = _oVehicleOrderImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            _oVehicleOrder.VehicleOrderDetails = VehicleOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptVehicleOrder oReport = new rptVehicleOrder();
            byte[] abytes = oReport.PrepareReport(_oVehicleOrder, oCompany);
            return File(abytes, "application/pdf");
        }
        
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region Print With Excel
        public void PrintVehicleOrderXL(int id)
        {
            VehicleOrder oVehicleOrder = new VehicleOrder();
            List<VehicleOrderDetail> oVehicleOrderDetails = new List<VehicleOrderDetail>();

            oVehicleOrder = oVehicleOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            oVehicleOrderDetails = VehicleOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);


            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Order File");
                sheet.Name = "Order File";

                sheet.Column(1).Width = 15;//Caption
                sheet.Column(2).Width = 40;//Value
                sheet.Column(3).Width = 15;//Option
                nMaxColumn = 3;

                int rowIndex = 3;            
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oVehicleOrder.ModelNo; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = " ";cell.Style.Font.Bold = false;
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
                if (oVehicleOrder.RefNo != "")
                {
                    sCode = oVehicleOrder.RefNo;
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
                if (oVehicleOrder.ChassisID>0)
                {
                    sChassisNo = oVehicleOrder.ChassisNo;
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
                if (oVehicleOrder.EngineID  > 0)
                {
                    sEngineNo = oVehicleOrder.EngineNo;
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oVehicleOrder.ExteriorColorName+ "   ("+oVehicleOrder.ExteriorColorCode+")"; cell.Style.Font.Bold = false;
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oVehicleOrder.InteriorColorName + "   (" + oVehicleOrder.InteriorColorCode + ")"; cell.Style.Font.Bold = false;
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
                if (oVehicleOrder.ETAValue > 0)
                {
                    sETA = oVehicleOrder.ETAValue.ToString() + " " + oVehicleOrder.ETATypeInString;
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oVehicleOrder.FeatureSetupName; cell.Style.Font.Bold = true;
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
                foreach (VehicleOrderDetail oItem in oVehicleOrderDetails)
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
        #endregion
    }

}
