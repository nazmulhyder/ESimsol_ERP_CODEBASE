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
	public class VehicleModelController : Controller
	{
		#region Declaration
		VehicleModel _oVehicleModel = new VehicleModel();
		List<VehicleModel> _oVehicleModels = new  List<VehicleModel>();
        VehicleModelImage _oVehicleModelImage = new VehicleModelImage();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewVehicleModelList( int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleModel).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oVehicleModels = new List<VehicleModel>();

            _oVehicleModels = VehicleModel.Gets("SELECT * FROM View_VehicleModel", (int)Session[SessionInfo.currentUserID]);
			return View(_oVehicleModels);
		}
		public ActionResult ViewVehicleModel(int id)
		{
			_oVehicleModel = new VehicleModel();
			if (id > 0)
			{
				_oVehicleModel = _oVehicleModel.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oVehicleModel.ModelFeatures = ModelFeature.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.ModelCategoryList = ModelCategory.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DriveTypes = EnumObject.jGets(typeof(EnumDriveType));
            ViewBag.CurrencyList = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BusinessSession = BusinessSession.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
			return View(_oVehicleModel);
		}

		[HttpPost]
		public JsonResult Save(VehicleModel oVehicleModel)
		{
			_oVehicleModel = new VehicleModel();
			try
			{
				_oVehicleModel = oVehicleModel;
				_oVehicleModel = _oVehicleModel.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oVehicleModel = new VehicleModel();
				_oVehicleModel.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oVehicleModel);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Approve(VehicleModel oVehicleModel)
        {
            _oVehicleModel = new VehicleModel();
            try
            {
                _oVehicleModel = oVehicleModel;
                _oVehicleModel = _oVehicleModel.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleModel = new VehicleModel();
                _oVehicleModel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleModel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
        public JsonResult Delete(VehicleModel oVehicleModel)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oVehicleModel.Delete(oVehicleModel.VehicleModelID, (int)Session[SessionInfo.currentUserID]);
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
                //--Remarks send ad Types
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
        public JsonResult GetVehicleModels(VehicleModel oVehicleModel)
        {
            List<VehicleModel> oVehicleModels = new List<VehicleModel>();
            try
            {
                //--Remarks send ad Types
                oVehicleModels = VehicleModel.GetsByModelNo(oVehicleModel.ModelNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oVehicleModel = new VehicleModel();
                oVehicleModel.ErrorMessage = ex.Message;
                oVehicleModels.Add(oVehicleModel);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleModels);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetVehicleModelFeatures(ModelFeature oModelFeature)
        {
            List<ModelFeature> oModelFeatures = new List<ModelFeature>();
            try
            {
                string sSql = "SELECT * FROM View_ModelFeature WHERE VehicleModelID=" + oModelFeature.VehicleModelID + " AND FeatureType="+(int)EnumFeatureType.OptionalFeature;
                oModelFeatures = ModelFeature.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oModelFeature = new ModelFeature();
                oModelFeature.ErrorMessage = ex.Message;
                oModelFeatures.Add(oModelFeature);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oModelFeatures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region Image Parts
        public ViewResult ImageHelper(int id, string ms)
        {
            _oVehicleModelImage = new VehicleModelImage();
            _oVehicleModelImage.VehicleModelID = id;
            List<VehicleModelThumbnail> oVehicleModelThumbnails = new List<VehicleModelThumbnail>();
            oVehicleModelThumbnails = VehicleModelThumbnail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            //foreach (VehicleModelThumbnail oItem in oVehicleModelThumbnails)
            //{
            //    oItem.ThumImage = GetImage(oItem.ThumbnailImage);
            //}
            _oVehicleModelImage.VehicleModelThumbnails = oVehicleModelThumbnails;
            _oVehicleModelImage.ImageTypeObjs = EnumObject.jGets(typeof(EnumImageType)).Where(x => x.id == (int)EnumImageType.Select_Image_Type || x.id == (int)EnumImageType.FrontImage || x.id == (int)EnumImageType.BackImage || x.id == (int)EnumImageType.TopImage || x.id == (int)EnumImageType.SideImage).ToList();
            
            TempData["message"] = ms;
            return View(_oVehicleModelImage);
        }


        [HttpPost]
        public ActionResult ImageHelper(HttpPostedFileBase file, VehicleModelImage oVehicleModelImage)
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
                else if (oVehicleModelImage.ImageTitle == null || oVehicleModelImage.ImageTitle == "")
                {
                    sErrorMessage = "Please enter image title!";
                }
                else
                {
                    oVehicleModelImage.LargeImage = aImageInByteArray;
                    oVehicleModelImage.VehicleModelThumbnailID = 0;
                    oVehicleModelImage.ThumbnailImage = aThumbnailImageInByteArray;
                    oVehicleModelImage = oVehicleModelImage.Save((int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                sErrorMessage = "Please select an image!";
            }
            return RedirectToAction("ImageHelper", new { id = oVehicleModelImage.VehicleModelID, ms = sErrorMessage });
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        [HttpGet]
        public JsonResult DeleteVehicleModelImage(int id)
        {
            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
            oVehicleModelImage = oVehicleModelImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            string sErrorMease = "";
            int nId = 0;
            nId = oVehicleModelImage.VehicleModelID;
            try
            {
                oVehicleModelImage.Delete(id, (int)Session[SessionInfo.currentUserID]);
                oVehicleModelThumbnail.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            oVehicleModelImage = oVehicleModelImage.GetImageByType(id, (int)EnumImageType.ModelImage,(int)Session[SessionInfo.currentUserID]);
            if (oVehicleModelImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oVehicleModelImage.LargeImage);
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
            VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
            oVehicleModelThumbnail = oVehicleModelThumbnail.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oVehicleModelThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oVehicleModelThumbnail.ThumbnailImage);
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
            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            try
            {
                oVehicleModelImage.VehicleModelImageID = id;
                oVehicleModelImage = oVehicleModelImage.Get(id, (int)Session[SessionInfo.currentUserID]);
                if (oVehicleModelImage.LargeImage != null)
                {
                    var file = File(oVehicleModelImage.LargeImage, "image/jpeg");
                    file.FileDownloadName = "Image #" + oVehicleModelImage.VehicleModelImageID;
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
            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            oVehicleModelImage = oVehicleModelImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oVehicleModelImage.LargeImage == null)
            {
                oVehicleModelImage.LargeImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oVehicleModelImage.LargeImage) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PrintList
        public ActionResult PrintList(string sIDs)
        {
            _oVehicleModel = new VehicleModel();
            string sSQL = "SELECT * FROM View_VehicleModel WHERE VehicleModelID IN (" + sIDs + ") ORDER BY VehicleModelID  ASC";
            _oVehicleModel.VehicleModelList = VehicleModel.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptVehicleModelList oReport = new rptVehicleModelList();
            byte[] abytes = oReport.PrepareReport(_oVehicleModel, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintVehicleModelPreview(int id)
        {
            _oVehicleModel = new VehicleModel();
            _oVehicleModel = _oVehicleModel.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oVehicleModel.VehicleModelImage = _oVehicleModelImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            _oVehicleModel.ModelFeatures = ModelFeature.Gets(id, (int)Session[SessionInfo.currentUserID]);
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptVehicleModel oReport = new rptVehicleModel();
            byte[] abytes = oReport.PrepareReport(_oVehicleModel, oCompany);
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
    }

}
