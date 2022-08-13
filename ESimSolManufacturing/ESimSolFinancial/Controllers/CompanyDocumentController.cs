using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;

namespace ESimSolFinancial.Controllers
{
    public class CompanyDocumentController : Controller
    {
        #region Declaration
        CompanyDocument _oCompanyDocument = new CompanyDocument();
        List<CompanyDocument> _oCompanyDocuments = new List<CompanyDocument>();

        #endregion

        #region view
        public ActionResult View_CompanyDocuments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oCompanyDocuments = new List<CompanyDocument>();
            return View(_oCompanyDocuments);
        }
        #endregion view

        #region CompanyDocument Attachment
        [HttpPost]
        public string UploadAttchment(HttpPostedFileBase file, CompanyDocument oCompanyDocument)
        {
            string sFeedBackMessage = "File Upload successfully";
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

                    //double nMaxLength = 1024 * 1024;
                    if (data == null || data.Length <= 0)
                    {
                        sFeedBackMessage = "Please select an file!";
                    }
                    //else if (data.Length > nMaxLength)
                    //{
                    //    sFeedBackMessage = "You can select maximum 1MB file size!";
                    //}
                    //else if (oCompanyDocument.CDID <= 0)
                    //{
                    //    sFeedBackMessage = "Your Selected CompanyDocument Is Invalid!";
                    //}
                    else
                    {
                        oCompanyDocument.DocFile = data;
                        oCompanyDocument.FileName = file.FileName;
                        oCompanyDocument.FileType = file.ContentType;
                        oCompanyDocument.CompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
                        oCompanyDocument = oCompanyDocument.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    sFeedBackMessage = "Please select a file!";
                }
            }
            catch (Exception ex)
            {
                sFeedBackMessage = "";
                sFeedBackMessage = ex.Message;
            }

            return sFeedBackMessage;

        }

        [HttpPost]
        public ActionResult DownloadAttachment(FormCollection oFormCollection)
        {
            CompanyDocument oCompanyDocument = new CompanyDocument();
            try
            {
                int nCompanyDocumentAttchmentID = Convert.ToInt32(oFormCollection["CDID"]);
                oCompanyDocument = CompanyDocument.GetWithAttachFile(nCompanyDocumentAttchmentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oCompanyDocument.DocFile != null)
                {
                    var file = File(oCompanyDocument.DocFile, oCompanyDocument.FileType);
                    file.FileDownloadName = oCompanyDocument.FileName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oCompanyDocument.FileName);
            }
        }

        [HttpPost]
        public JsonResult DeleteCompanyDocument(CompanyDocument oCompanyDocument)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oCompanyDocument.Delete(oCompanyDocument.CDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        [HttpPost]
        public JsonResult CDSearch(string sParam)
        {
            List<CompanyDocument> oCompanyDocuments = new List<CompanyDocument>();
            string sFileName = Convert.ToString(sParam.Split('~')[0]).TrimStart(' ');
            string sDescription = Convert.ToString(sParam.Split('~')[1]).TrimStart(' ');
            int nRowLength = Convert.ToInt32(sParam.Split('~')[2]);
            int nLoadRecord = Convert.ToInt32(sParam.Split('~')[3]);

            string sSQL = "SELECT top(" + nLoadRecord + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY CDID) Row,* FROM CompanyDocument WHERE CDID <>0";
            if (sFileName!="")
            {
                sSQL = sSQL + " AND FileName LIKE'%" + sFileName + "%'";
            }
            if (sDescription != "")
            {
                sSQL = sSQL + " AND Description LIKE'%" + sDescription + "%'";
            }
            sSQL = sSQL + ") aa WHERE Row >" + nRowLength;
            
            try
            {
                oCompanyDocuments = CompanyDocument.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                oCompanyDocuments.ForEach(
                x =>
                {
                    x.FileName = "";
                    x.FileName = "";
                    x.DocFile = null;
                }
                );

            }
            catch (Exception ex)
            {
                oCompanyDocuments = new List<CompanyDocument>();
                CompanyDocument oCompanyDocument = new CompanyDocument();
                oCompanyDocument.ErrorMessage = ex.Message;
                oCompanyDocuments.Add(oCompanyDocument);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompanyDocuments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
