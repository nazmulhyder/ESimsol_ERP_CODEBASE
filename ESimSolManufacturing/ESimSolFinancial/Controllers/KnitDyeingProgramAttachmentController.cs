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
    public class KnitDyeingProgramAttachmentController:Controller
    {
        #region  KnitDyeingProgramAttachment
        public ViewResult KnitDyeingProgramAttachments(int id, string ms, string OperationInfo)
        {
            KnitDyeingProgramAttachment oKnitDyeingProgramAttachment = new KnitDyeingProgramAttachment();
            List<KnitDyeingProgramAttachment> oKnitDyeingProgramAttachments = new List<KnitDyeingProgramAttachment>();
            oKnitDyeingProgramAttachments = KnitDyeingProgramAttachment.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oKnitDyeingProgramAttachment.KnitDyeingProgramID = id;
            oKnitDyeingProgramAttachment.KnitDyeingProgramAttachments = oKnitDyeingProgramAttachments;
            TempData["message"] = ms;
            oKnitDyeingProgramAttachment.ErrorMessage = OperationInfo;
            return View(oKnitDyeingProgramAttachment);
        }

        [HttpPost]
        public ActionResult UploadAttchment(HttpPostedFileBase file, KnitDyeingProgramAttachment oKnitDyeingProgramAttachment)
        {
            string sErrorMessage = "", sRefNameOperationInfo = oKnitDyeingProgramAttachment.ErrorMessage;
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
                    else if (oKnitDyeingProgramAttachment.KnitDyeingProgramID <= 0)
                    {
                        sErrorMessage = "Your Selected Knit Dyeing Program Is Invalid!";
                    }
                    else
                    {
                        oKnitDyeingProgramAttachment.AttachFile = data;
                        oKnitDyeingProgramAttachment.FileName = file.FileName;
                        oKnitDyeingProgramAttachment.FileType = file.ContentType;
                        if(oKnitDyeingProgramAttachment.FileType.Substring(0,5)=="image")
                        {
                            oKnitDyeingProgramAttachment = oKnitDyeingProgramAttachment.Save((int)Session[SessionInfo.currentUserID]);
                        }
                        else
                        {
                            sErrorMessage = "Please Select Only Image";
                        }
                        
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
            return RedirectToAction("KnitDyeingProgramAttachments", new { id = oKnitDyeingProgramAttachment.KnitDyeingProgramID, ms = sErrorMessage, OperationInfo = sRefNameOperationInfo });
        }


        public ActionResult DownloadAttachment(int id, double ts)
        {
            KnitDyeingProgramAttachment oKnitDyeingProgramAttachment = new KnitDyeingProgramAttachment();
            try
            {
                oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID = id;
                oKnitDyeingProgramAttachment = KnitDyeingProgramAttachment.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oKnitDyeingProgramAttachment.AttachFile != null)
                {
                    var file = File(oKnitDyeingProgramAttachment.AttachFile, oKnitDyeingProgramAttachment.FileType);
                    file.FileDownloadName = oKnitDyeingProgramAttachment.FileName;
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
                throw new HttpException(404, "Couldn't find " + oKnitDyeingProgramAttachment.FileName);
            }
        }

        [HttpPost]
        public JsonResult DeleteAttachment(KnitDyeingProgramAttachment oKnitDyeingProgramAttachment)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oKnitDyeingProgramAttachment.Delete(oKnitDyeingProgramAttachment.KnitDyeingProgramAttachmentID, (int)Session[SessionInfo.currentUserID]);
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