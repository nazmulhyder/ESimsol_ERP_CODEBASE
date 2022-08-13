using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class DeliverySetupController : Controller
    {
        #region Declaration
        DeliverySetup _oDeliverySetup = new DeliverySetup();
        List<DeliverySetup> _oDeliverySetups = new List<DeliverySetup>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult ViewDeliverySetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDeliverySetups = DeliverySetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PrintNoList = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.PrintFormatType = EnumObject.jGets(typeof(EnumPrintFormatType));
            ViewBag.BUID = buid;
            return View(_oDeliverySetups);
        }
        [HttpPost]
        public JsonResult Save(DeliverySetup oDeliverySetup)
        {
            try
            {
                _oDeliverySetup = _oDeliverySetup.Save(oDeliverySetup, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDeliverySetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliverySetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Save Information by XML
        [HttpPost]
        public JsonResult SaveDeliverySetupInformation(double nts)
        {
            string sMessage = "";
            DeliverySetup oDeliverySetup = new DeliverySetup();
            try
            {
                oDeliverySetup.DeliverySetupID = Convert.ToInt32(Request.Headers["DeliverySetupID"]);
                oDeliverySetup.PrintHeader = Request.Headers["PrintHeader"].Trim();
                oDeliverySetup.OrderPrintNo = (EnumExcellColumn)Convert.ToInt32(Request.Headers["OrderPrintNo"]);
                oDeliverySetup.ChallanPrintNo = (EnumExcellColumn)Convert.ToInt32(Request.Headers["ChallanPrintNo"]);
                oDeliverySetup.BUID = Convert.ToInt32(Request.Headers["BUID"]);
                oDeliverySetup.DCPrefix = Request.Headers["DCPrefix"].Trim();
                oDeliverySetup.GPPrefix = Request.Headers["GPPrefix"].Trim();
                oDeliverySetup.PrintFormatType = (EnumPrintFormatType)Convert.ToInt32(Request.Headers["PrintFormatType"]);
                oDeliverySetup.IsImg = Convert.ToBoolean(Request.Headers["IsImg"]);
                oDeliverySetup.OverDCQty = Convert.ToDouble(Request.Headers["OverDCQty"].Trim());
                oDeliverySetup.OverDeliverPercentage = Convert.ToDouble(Request.Headers["OverDeliverPercentage"].Trim());

                byte[] data;
                #region File
                //if (Request.Files.Count > 0)
                //{
                    HttpPostedFileBase file = null;
                    if (oDeliverySetup.IsImg == true)
                    {
                        file = Request.Files[0];
                    }
                    //if (Request.Files.Count == 3)
                    //{
                    //    file = Request.Files[0];
                    //}
                    //else
                    //{
                    //    if (Convert.ToBoolean(Request.Headers["IsImg"]) == true)
                    //    {
                    //        file = Request.Files[0];
                    //    }
                        
                    //}
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
                            double nMaxLength = 40 * 1024;
                            if (data.Length > nMaxLength)
                            {
                                throw new Exception("Youe Photo Image " + data.Length / 1024 + "KB! You can selecte maximum 40KB image");
                            }
                        }
                        oDeliverySetup.ImagePad = data;
                    }
                    
                //}
                #endregion
                oDeliverySetup = oDeliverySetup.Save(oDeliverySetup, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDeliverySetup.DeliverySetupID > 0 && (oDeliverySetup.ErrorMessage == null || oDeliverySetup.ErrorMessage == ""))
                {
                    sMessage = "Save Successfully" + "~" + oDeliverySetup.DeliverySetupID + "~" + oDeliverySetup.PrintHeader + "~" + oDeliverySetup.OrderPrintNo + "~" + oDeliverySetup.ChallanPrintNo + "~" + oDeliverySetup.BUID + "~" + oDeliverySetup.DCPrefix + "~"
                        + oDeliverySetup.GPPrefix + "~" + oDeliverySetup.PrintFormatType + "~" + oDeliverySetup.OrderPrintNoSt + "~" + oDeliverySetup.ChallanPrintNoSt + "~" + oDeliverySetup.PrintFormatTypeSt + "~" + oDeliverySetup.OverDCQty + "~" + oDeliverySetup.OverDeliverPercentage;
                }
                else
                {
                    if ((oDeliverySetup.ErrorMessage == null || oDeliverySetup.ErrorMessage == "")) { throw new Exception("Unable to Save/Upload."); }
                    else { throw new Exception(oDeliverySetup.ErrorMessage); }

                }
            }
            catch (Exception ex)
            {
                oDeliverySetup = new DeliverySetup();
                oDeliverySetup.ErrorMessage = ex.Message;
                sMessage = oDeliverySetup.ErrorMessage + "~" + oDeliverySetup.DeliverySetupID + "~" + oDeliverySetup.PrintHeader + "~" + oDeliverySetup.OrderPrintNo + "~" + oDeliverySetup.ChallanPrintNo + "~" +
                               oDeliverySetup.BUID + "~" + oDeliverySetup.DCPrefix + "~" + oDeliverySetup.GPPrefix + "~" + oDeliverySetup.PrintFormatType;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult Get(DeliverySetup oDeliverySetup)
        {
            try
            {
                if (oDeliverySetup.DeliverySetupID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oDeliverySetup = _oDeliverySetup.Get(oDeliverySetup.DeliverySetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDeliverySetup.ImagePad != null)
                {
                    _oDeliverySetup.ByteInString = "data:image/Jpeg;base64," + Convert.ToBase64String(_oDeliverySetup.ImagePad);
                }
            }
            catch (Exception ex)
            {
                _oDeliverySetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliverySetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DeliverySetup oDeliverySetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDeliverySetup.Delete(oDeliverySetup.DeliverySetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

    }
}