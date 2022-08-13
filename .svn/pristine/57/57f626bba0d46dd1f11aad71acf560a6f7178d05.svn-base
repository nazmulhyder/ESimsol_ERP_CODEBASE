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
using System.Reflection;

namespace ESimSolFinancial.Controllers
{
    public class AttachDocumentController:Controller
    {
        #region  AttachDocument
        public ViewResult Attachment(int id,int RefType, string ms, string OperationInfo)
        {
            AttachDocument oAttachDocument = new AttachDocument();
            List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
            oAttachDocuments = AttachDocument.Gets(id,RefType, (int)Session[SessionInfo.currentUserID]);
            oAttachDocument.RefID = id;
            oAttachDocument.RefType = (EnumAttachRefType)RefType;
            oAttachDocument.RefTypeInInt = RefType;
            oAttachDocument.AttachDocuments = oAttachDocuments;
            TempData["message"] = ms;
            oAttachDocument.ErrorMessage = OperationInfo;
            return View(oAttachDocument);
        }

        [HttpPost]
        public ActionResult UploadAttchment(HttpPostedFileBase file, AttachDocument oAttachDocument)
        {
            string sErrorMessage = "", sRefNameOperationInfo = oAttachDocument.ErrorMessage;
            int nRefType = oAttachDocument.RefTypeInInt;
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
                    else if (oAttachDocument.RefID <= 0)
                    {
                        sErrorMessage = "Your Selected Purchase Order Is Invalid!";
                    }
                    else
                    {
                        oAttachDocument.AttachFile = data;
                        oAttachDocument.FileName = file.FileName;
                        oAttachDocument.FileType = file.ContentType;
                        oAttachDocument.RefType = (EnumAttachRefType)nRefType;
                        oAttachDocument = oAttachDocument.Save((int)Session[SessionInfo.currentUserID]);
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
            return RedirectToAction("Attachment", new { id = oAttachDocument.RefID, ms = sErrorMessage, RefType = nRefType, OperationInfo = sRefNameOperationInfo });
        }


        public ActionResult DownloadAttachment(int id, double ts)
        {
            AttachDocument oAttachDocument = new AttachDocument();
            try
            {
                oAttachDocument.AttachDocumentID = id;
                oAttachDocument = AttachDocument.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oAttachDocument.AttachFile != null)
                {
                    var file = File(oAttachDocument.AttachFile, oAttachDocument.FileType);
                    file.FileDownloadName = oAttachDocument.FileName;
                    TempData["message"] = "";
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oAttachDocument.FileName);
            }
        }

        [HttpPost]
        public JsonResult DeleteAttachment(AttachDocument oAttachDocument)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oAttachDocument.Delete(oAttachDocument.AttachDocumentID, (int)Session[SessionInfo.currentUserID]);
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
    }
}