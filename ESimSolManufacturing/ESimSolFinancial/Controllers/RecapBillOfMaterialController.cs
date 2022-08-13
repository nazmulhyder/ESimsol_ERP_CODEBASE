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

namespace ESimSolFinancial.Controllers
{
    public class RecapBillOfMaterialController : Controller
    {
        #region Declaration
        private byte[] aImageInByteArray;
        RecapBillOfMaterial _oRecapBillOfMaterial = new RecapBillOfMaterial();
        OrderRecap _oOrderRecap = new OrderRecap();
        #endregion

        public ViewResult ViewRecapBillOfMaterial(int orid, string ms, double ts)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
            List<RecapBillOfMaterial> oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
            oOrderRecap = oOrderRecap.Get(orid, (int)Session[SessionInfo.currentUserID]);
            oRecapBillOfMaterials = RecapBillOfMaterial.Gets(orid, (int)Session[SessionInfo.currentUserID]);
            oRecapBillOfMaterial.OrderRecapID = orid;
            oRecapBillOfMaterial.RecapBillOfMaterials = oRecapBillOfMaterials;
            ViewBag.ErrorMessage = ms;
            ViewBag.HeaderTitle = "Bill of Material for Order Recap No: "+oOrderRecap.OrderRecapNo+", Style No : " + oOrderRecap.StyleNo + " , Buyer Name : " + oOrderRecap.BuyerName + ", Session : " + oOrderRecap.SessionName;
            return View(oRecapBillOfMaterial);
        }

        [HttpPost]
        public ActionResult SaveRecapBillOfMaterial(HttpPostedFileBase file, RecapBillOfMaterial oRecapBillOfMaterial)
        {
            string sErrorMessage = "", sRefNameOperationInfo = oRecapBillOfMaterial.ErrorMessage;
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
                    else if (oRecapBillOfMaterial.OrderRecapID <= 0)
                    {
                        sErrorMessage = "Your Selected Technical Sheet Is Invalid!";
                    }
                    else
                    {
                        oRecapBillOfMaterial.AttachFile = data;
                    }
                }
                oRecapBillOfMaterial = oRecapBillOfMaterial.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMessage = "";
                sErrorMessage = ex.Message;
            }
            double tsv = DateTime.Now.Ticks;
            return RedirectToAction("ViewRecapBillOfMaterial", new { tsid = oRecapBillOfMaterial.OrderRecapID, ms = sErrorMessage, ts = tsv });
        }

        [HttpPost]
        public JsonResult SaveBOM(RecapBillOfMaterial oRecapBillOfMaterial)
        {
            try
            {
                oRecapBillOfMaterial = oRecapBillOfMaterial.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecapBillOfMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteRecapBillOfMaterial(RecapBillOfMaterial oRecapBillOfMaterial)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oRecapBillOfMaterial.Delete(oRecapBillOfMaterial.RecapBillOfMaterialID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult ResetSequence(RecapBillOfMaterial oRecapBillOfMaterial)
        {
            List<RecapBillOfMaterial> oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
            try
            {

                oRecapBillOfMaterials = RecapBillOfMaterial.ResetSequence(oRecapBillOfMaterial.RecapBillOfMaterials, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
                oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.ErrorMessage = ex.Message;
                oRecapBillOfMaterials.Add(oRecapBillOfMaterial);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecapBillOfMaterials);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBOMImage(double ts)
        {
            _oRecapBillOfMaterial = new RecapBillOfMaterial();
            string sResult = "";
            try
            {
                _oRecapBillOfMaterial.RecapBillOfMaterialID = Convert.ToInt32(Request.Headers["RecapBillOfMaterialID"]);
                #region File
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    int fileSize = file.ContentLength;
                    //fileName = file.FileName;
                    string mimeType = file.ContentType;
                    if (file != null && file.ContentLength > 0)
                    {
                        Image oImage = Image.FromStream(file.InputStream, true, true);
                        aImageInByteArray = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            oImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            aImageInByteArray = ms.ToArray();
                        }
                        _oRecapBillOfMaterial.AttachFile = aImageInByteArray;
                    }
                    else
                    {
                        _oRecapBillOfMaterial.AttachFile = null;
                    }
                }
                else
                {
                    _oRecapBillOfMaterial.AttachFile = null;
                }

                #endregion

                _oRecapBillOfMaterial = _oRecapBillOfMaterial.UpdateImage((int)Session[SessionInfo.currentUserID]);
                sResult = "~" + _oRecapBillOfMaterial.RecapBillOfMaterialID + "~" + _oRecapBillOfMaterial.OrderRecapID + "~" + _oRecapBillOfMaterial.ProductID + "~" + _oRecapBillOfMaterial.ColorID + "~" + _oRecapBillOfMaterial.SizeID + "~" + _oRecapBillOfMaterial.ItemDescription + "~" + _oRecapBillOfMaterial.Reference + "~" + _oRecapBillOfMaterial.Construction + "~" + _oRecapBillOfMaterial.Sequence + "~" + _oRecapBillOfMaterial.MUnitID + "~" + _oRecapBillOfMaterial.ReqQty + "~" + _oRecapBillOfMaterial.CuttingQty + "~" + _oRecapBillOfMaterial.ConsumptionQty + "~" + _oRecapBillOfMaterial.ProductCode + "~" + _oRecapBillOfMaterial.ProductName + "~" + _oRecapBillOfMaterial.ColorName + "~" + _oRecapBillOfMaterial.SizeName + "~" + _oRecapBillOfMaterial.Symbol + "~" + _oRecapBillOfMaterial.UnitName + "~" + _oRecapBillOfMaterial.IsAttachmentExist;
            }
            catch (Exception ex)
            {
                _oRecapBillOfMaterial = new RecapBillOfMaterial();
                _oRecapBillOfMaterial.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sResult);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadBOMAttachment(int id, double ts)
        {
            RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
            try
            {
                oRecapBillOfMaterial.RecapBillOfMaterialID = id;
                oRecapBillOfMaterial = RecapBillOfMaterial.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oRecapBillOfMaterial.AttachFile != null)
                {
                    var file = File(oRecapBillOfMaterial.AttachFile, "image/jpeg");
                    file.FileDownloadName = "Image #" + oRecapBillOfMaterial.RecapBillOfMaterialID;
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

        //DeleteBOMAttachment
        [HttpGet]
        public JsonResult DeleteBOMAttachment(int id, double ts)
        {
            RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
            try
            {

                oRecapBillOfMaterial = oRecapBillOfMaterial.DeleteImage(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecapBillOfMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintRecapBillOfMaterial(int id)
        {
            Company oCompany = new Company();
            _oOrderRecap = new OrderRecap();
            List<RecapBillOfMaterial> oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
            List<MeasurementSpecAttachment> oMeasurementSpecAttachments = new List<MeasurementSpecAttachment>();
            _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
            oRecapBillOfMaterials = RecapBillOfMaterial.GetsWithImage(id, (int)Session[SessionInfo.currentUserID]);
            foreach (RecapBillOfMaterial oItem in oRecapBillOfMaterials)
            {
                oItem.AttachImage = GetImage(oItem);
            }
            _oOrderRecap.RecapBillOfMaterials = oRecapBillOfMaterials;
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderRecap.Company = oCompany;

            rptRecapBiillOfMaterial oReport = new rptRecapBiillOfMaterial();
            byte[] abytes = oReport.PrepareReport(_oOrderRecap);
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
        public Image GetImage(RecapBillOfMaterial oRecapBillOfMaterial)
        {
            if (oRecapBillOfMaterial.AttachFile != null)
            {
                MemoryStream m = new MemoryStream(oRecapBillOfMaterial.AttachFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }  
    }
}
