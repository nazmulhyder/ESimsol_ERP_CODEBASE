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
    public class BillOfMaterialController : Controller
    {
        #region Declaration
        private byte[] aImageInByteArray;
        BillOfMaterial _oBillOfMaterial = new BillOfMaterial();
        #endregion

        public ViewResult ViewBillOfMaterial(int tsid, string ms, double ts)
        {
            TechnicalSheet oTechnicalSheet =new TechnicalSheet();
            BillOfMaterial oBillOfMaterial = new BillOfMaterial();            
            List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
            oTechnicalSheet = oTechnicalSheet.Get(tsid, (int)Session[SessionInfo.currentUserID]);
            oBillOfMaterials = BillOfMaterial.Gets(tsid, (int)Session[SessionInfo.currentUserID]);
            oBillOfMaterial.TechnicalSheetID = tsid;            
            oBillOfMaterial.BillOfMaterials = oBillOfMaterials;
            ViewBag.ErrorMessage = ms;
            ViewBag.HeaderTitle = "Bill of Material for Style No : " + oTechnicalSheet.StyleNo + ", Buyer Name : " + oTechnicalSheet.BuyerName + ", Session : " + oTechnicalSheet.SessionName;
            return View(oBillOfMaterial);
        }

        [HttpPost]
        public ActionResult SaveBillOfMaterial(HttpPostedFileBase file, BillOfMaterial oBillOfMaterial)
        {
            string sErrorMessage = "", sRefNameOperationInfo = oBillOfMaterial.ErrorMessage;            
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
                    else if (oBillOfMaterial.TechnicalSheetID <= 0)
                    {
                        sErrorMessage = "Your Selected Technical Sheet Is Invalid!";
                    }
                    else
                    {
                        oBillOfMaterial.AttachFile = data;                                                
                    }
                }
                oBillOfMaterial = oBillOfMaterial.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMessage = "";
                sErrorMessage = ex.Message;
            }
            double tsv = DateTime.Now.Ticks;
            return RedirectToAction("ViewBillOfMaterial", new { tsid = oBillOfMaterial.TechnicalSheetID, ms = sErrorMessage, ts = tsv });
        }

        [HttpPost]
        public JsonResult SaveBOM(BillOfMaterial oBillOfMaterial)
        {
            try
            {
                oBillOfMaterial = oBillOfMaterial.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBillOfMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteBillOfMaterial(BillOfMaterial oBillOfMaterial)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oBillOfMaterial.Delete(oBillOfMaterial.BillOfMaterialID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult ResetSequence(BillOfMaterial oBillOfMaterial)
        {
            List<BillOfMaterial>  oBillOfMaterials = new List<BillOfMaterial>();
            try
            {

                oBillOfMaterials = BillOfMaterial.ResetSequence(oBillOfMaterial.BillOfMaterials, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBillOfMaterials = new List<BillOfMaterial>();
                oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.ErrorMessage = ex.Message;
                oBillOfMaterials.Add(oBillOfMaterial);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBillOfMaterials);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBOMImage(double ts)
        {
            _oBillOfMaterial = new BillOfMaterial();
            string sResult = "";
            try
            {
                _oBillOfMaterial.BillOfMaterialID = Convert.ToInt32(Request.Headers["BillOfMaterialID"]);
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
                        _oBillOfMaterial.AttachFile = aImageInByteArray;
                    }
                    else
                    {
                        _oBillOfMaterial.AttachFile = null;
                    }
                }
                else
                {
                    _oBillOfMaterial.AttachFile = null;
                }

                #endregion

                _oBillOfMaterial = _oBillOfMaterial.UpdateImage((int)Session[SessionInfo.currentUserID]);
                sResult = "~" + _oBillOfMaterial.BillOfMaterialID + "~" + _oBillOfMaterial.TechnicalSheetID + "~" + _oBillOfMaterial.ProductID + "~" + _oBillOfMaterial.ColorID + "~" + _oBillOfMaterial.SizeID + "~" + _oBillOfMaterial.ItemDescription + "~" + _oBillOfMaterial.Reference + "~" + _oBillOfMaterial.Construction + "~" + _oBillOfMaterial.Sequence + "~" + _oBillOfMaterial.MUnitID + "~" + _oBillOfMaterial.ReqQty + "~" + _oBillOfMaterial.CuttingQty + "~" + _oBillOfMaterial.ConsumptionQty + "~" + _oBillOfMaterial.ProductCode + "~" + _oBillOfMaterial.ProductName + "~" + _oBillOfMaterial.ColorName + "~" + _oBillOfMaterial.SizeName + "~" + _oBillOfMaterial.Symbol + "~" + _oBillOfMaterial.UnitName + "~" + _oBillOfMaterial.IsAttachmentExist;
            }
            catch (Exception ex)
            {
                _oBillOfMaterial = new BillOfMaterial();
                _oBillOfMaterial.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sResult);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadBOMAttachment(int id, double ts)
        {
            BillOfMaterial oBillOfMaterial = new BillOfMaterial();
            try
            {
                oBillOfMaterial.BillOfMaterialID = id;
                oBillOfMaterial = BillOfMaterial.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oBillOfMaterial.AttachFile != null)
                {
                    var file = File(oBillOfMaterial.AttachFile, "image/jpeg");
                    file.FileDownloadName = "Image #" + oBillOfMaterial.BillOfMaterialID;
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
            BillOfMaterial oBillOfMaterial = new BillOfMaterial();
            try
            {

                oBillOfMaterial = oBillOfMaterial.DeleteImage(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBillOfMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
