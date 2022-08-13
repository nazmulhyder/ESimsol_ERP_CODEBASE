using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using System.Web;

namespace ESimSolFinancial.Controllers
{
    public class ImportFormatController : Controller
    {
        #region Declartion
        
        #endregion

        #region ImportFormat
        public ActionResult Attachment(int menuid, string msg)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser( ((int)EnumModuleName.ImportFormat).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            ImportFormat oImportFormat = new ImportFormat();
            oImportFormat.ImportFormatTypes = EnumObject.jGets(typeof(EnumImportFormatType));
            oImportFormat.ImportFormats =ImportFormat.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.MSG = msg == null ? "" : msg;
            return View(oImportFormat);
        }

        [HttpPost]
        public ActionResult UploadAttachment(HttpPostedFileBase file, ImportFormat oImportFormat)
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

                        double nMaxLength = 1024 * 1024;
                        if (data == null || data.Length <= 0)
                        {
                            sErrorMessage = "Please select an file!";
                        }
                        else if (data.Length > nMaxLength)
                        {
                            sErrorMessage = "You can select maximum 1MB file size!";
                        }
                        
                        else
                        {
                            oImportFormat.AttatchFile = data;
                            oImportFormat.AttatchmentName = file.FileName;
                            oImportFormat.FileType = file.ContentType;
                            oImportFormat.Remarks = oImportFormat.Remarks == null ? "" : oImportFormat.Remarks;
                            oImportFormat = oImportFormat.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                            if (oImportFormat.ErrorMessage != "")
                            {
                                sErrorMessage = oImportFormat.ErrorMessage;
                            }
                        }
                    }
                    

               
            }
            catch (Exception ex)
            {
                sErrorMessage = "";
                sErrorMessage = ex.Message;
            }
            return RedirectToAction("Attachment", new { menuid = (int)Session[SessionInfo.MenuID], msg = sErrorMessage });
        }


        public ActionResult DownloadAttachment(int id, double ts)
        {
            ImportFormat oImportFormat = new ImportFormat();
            try
            {
                oImportFormat.ImportFormatID = id;
                oImportFormat = ImportFormat.GetWithAttachFile(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oImportFormat.AttatchFile != null)
                {
                    var file = File(oImportFormat.AttatchFile, oImportFormat.FileType);
                    file.FileDownloadName = oImportFormat.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oImportFormat.AttatchmentName);
            }
        }

        [HttpPost]
        public JsonResult DeleteAttachment(ImportFormat oImportFormat)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oImportFormat.Delete(oImportFormat.ImportFormatID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
