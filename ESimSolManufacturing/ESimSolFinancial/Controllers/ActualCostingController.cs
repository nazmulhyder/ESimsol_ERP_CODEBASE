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
using System.Linq;
using System.Drawing.Imaging;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class ActualCostingController : Controller
    {
        #region Declaration
        GRN _oGRN = new GRN();
        List<GRN> _oGRNs = new List<GRN>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        Company _oCompany = new Company();
        List<ImportLC> _oImportLCs = new List<ImportLC>();
        ImportLC _oImportLC = new ImportLC();
        List<GRNDetail> _oGRNDetails = new List<GRNDetail>();
        GRNDetail _oGRNDetail = new GRNDetail();
        #endregion
        #region Actions

        public ActionResult ViewActualCosting(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<GRNDetail> oGRNDetails = new List<GRNDetail>();

            return View(oGRNDetails);
        }

        #endregion

        [HttpPost]
        public JsonResult GetLC(ImportLC oImportLC)
        {

            string sSQL = "SELECT * FROM View_ImportLC WHERE ISNULL(ImportLCID,0) IN (SELECT ImportLCID FROM View_GRNDetailLandingCost) AND ImportLCNo LIKE '%"+oImportLC.ImportLCNo+"%' ";
            _oImportLCs = ImportLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProduct(GRNDetail oGRNDetail)
        {
            string sSQL = "SELECT * FROM View_GRNDetail WHERE ProductName LIKE '%"+oGRNDetail.ProductName+"%' ";
            _oGRNDetails = GRNDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region Search
        [HttpPost]
        public JsonResult Search(GRNDetail oGRNDetail)//
        {
            _oGRNDetails = new List<GRNDetail>();
            try
            {

                string sSQL = GetSQLAdvSrc(oGRNDetail.ErrorMessage);
                //string SQL = sSQL + query + ordrBy;
                _oGRNDetails = GRNDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRNDetail = new GRNDetail();
                _oGRNDetail.ErrorMessage = ex.Message;
                _oGRNDetails.Add(_oGRNDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQLAdvSrc(string sTemp)
        {
            DateTime GRNStartDate = DateTime.MinValue;
            DateTime GRNEndDate = DateTime.MinValue;
            string sProductIDs = sTemp.Split('~')[0];
            string sLCIDs = sTemp.Split('~')[1];

            if (sTemp.Split('~')[2] != "")
            {
                 GRNStartDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            }
            if (sTemp.Split('~')[3] != "")
            {
                 GRNEndDate = Convert.ToDateTime(sTemp.Split('~')[3]);
            }

            string sReturn1 = "SELECT * FROM View_GRNDetailLandingCost";
            string sReturn = "";
            #region Default Clasuse Ref type
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " RefType =" +((int)EnumGRNType.ImportInvoice).ToString() ;
            #endregion

            if (sProductIDs != "undefined" && sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID IN (" + sProductIDs + ") ";
            }
            if (sLCIDs != "undefined" && sLCIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportLCID IN (" + sLCIDs + ") ";
            }

            if (GRNStartDate != DateTime.MinValue && GRNEndDate != DateTime.MinValue)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNDate Between '" + GRNStartDate + "' AND '" + GRNEndDate + "' ";
            }
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Print
        public ActionResult Costing(int GRNID, int ProductID, int GRNDetailID)
        {
            List<GRNDetailBreakDown> oGRNDetailBreakDowns = new List<GRNDetailBreakDown>();
            _oGRN = new GRN();
            _oGRNDetail = new GRNDetail();
            try
            {
                _oGRN = _oGRN.Get(GRNID, (int)Session[SessionInfo.currentUserID]);
                _oGRNDetail = _oGRNDetail.Get(GRNDetailID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_GRNDetailBreakDown WHERE GRNDetailID = "+GRNDetailID+" ORDER BY CostHeadID";
                _oGRN.GRNDetailBreakDowns = GRNDetailBreakDown.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRNDetail = new GRNDetail();
                _oGRNDetails = new List<GRNDetail>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oGRN.Company = oCompany;
            rptCosting oReport = new rptCosting();
            byte[] abytes = oReport.PrepareReport(_oGRN, _oGRNDetail);
            return File(abytes, "application/pdf");
        }


        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
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
