using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.Collections;


namespace ESimSolFinancial.Controllers
{
    public class NoticeController : PdfViewController
    {
        #region Declaration
        Notice _oNotice = new Notice();
        List<Notice> _oNotices = new List<Notice>();
        #endregion

        #region   Notice

        public ActionResult ViewNotices(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oNotices = new List<Notice>();
            string sSQL = "Select * from View_Notice Where IsActive=1 And [ExpireDate]>='" + DateTime.Now.AddDays(-7).ToString("dd MMM yyyy") + "'  Order by IssueDate DESC";
            _oNotices = Notice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oNotices.GroupBy(x => x.NoticeID).Select(x => x.OrderBy(y => y.NoticeID).FirstOrDefault()).ToList();
            return View(_oNotices);
        }

        [HttpPost]
        public JsonResult Save(Notice oNotice)
        {
            try
            {
                if (oNotice.NoticeID <= 0)
                {
                    oNotice = oNotice.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oNotice = oNotice.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oNotice = new Notice();
                oNotice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNotice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Notice oNotice)
        {
            try
            {
                if (oNotice.NoticeID <= 0) { throw new Exception("Please select an valid item."); }
                oNotice = oNotice.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oNotice = new Notice();
                oNotice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNotice.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityChange(Notice oNotice)
        {
            try
            {
                if (oNotice.NoticeID <= 0) { throw new Exception("Please select an valid item."); }
                oNotice.IsActive = !oNotice.IsActive;
                oNotice = oNotice.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oNotice = new Notice();
                oNotice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNotice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Get(Notice oNotice)
        {
            try
            {
                if (oNotice.NoticeID <= 0) { throw new Exception("Please select an valid item."); }
                oNotice = Notice.Get(oNotice.NoticeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oNotice = new Notice();
                oNotice.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNotice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Gets(Notice oNotice)
        {
            _oNotices = new List<Notice>();
            try
            {
                string sNoticeNo = oNotice.Params.Split('~')[0].Trim();
                bool bIsActive = Convert.ToBoolean(oNotice.Params.Split('~')[1]);
                bool bIsInActive = Convert.ToBoolean(oNotice.Params.Split('~')[2]);
                DateTime ExpireDateFrom = Convert.ToDateTime(oNotice.Params.Split('~')[3]);
                DateTime ExpireDateTo = Convert.ToDateTime(oNotice.Params.Split('~')[4]);
                bool IsDateSearch = Convert.ToBoolean(oNotice.Params.Split('~')[5]);

                string sSQL = "Select * from View_Notice Where NoticeID <> 0  ";
                if (sNoticeNo != "") { sSQL = sSQL + " and NoticeNo Like '%" + sNoticeNo + "%'"; }
                if (bIsActive) { sSQL = sSQL + " and IsActive = 1"; }
                if (bIsInActive) { sSQL = sSQL + " and IsActive = 0"; }
                if (IsDateSearch) { sSQL = sSQL + " and [ExpireDate] Between '" + ExpireDateFrom.ToString("dd-MMM-yyyy") + "' And '" + ExpireDateFrom.ToString("dd-MMM-yyyy") + "'"; }

                _oNotices = Notice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNotices = new List<Notice>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNotices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PrintNotice(int id)
        {
            Notice oNotice = new Notice();
            oNotice = Notice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            rptNotice oReport = new rptNotice();
            byte[] abytes = oReport.PrepareReport(oNotice, oCompany);
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