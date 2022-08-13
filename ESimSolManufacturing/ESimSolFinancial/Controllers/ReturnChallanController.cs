using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class ReturnChallanController : Controller
    {
        #region Declaration
        ReturnChallan _oReturnChallan = new ReturnChallan();
        List<ReturnChallan> _oReturnChallans = new List<ReturnChallan>();
        ReturnChallanDetail _oReturnChallanDetail = new ReturnChallanDetail();
        List<ReturnChallanDetail> _oReturnChallanDetails = new List<ReturnChallanDetail>();
        ExportSC _oExportSC = new ExportSC();
        List<ExportSC> _oExportSCs = new List<ExportSC>();
        #endregion

        #region Actions
        public ActionResult ViewReturnChallans(int ProductNature, int buid, int menuid)
        {
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oReturnChallans = new List<ReturnChallan>();
            string sSQL = "Select * from View_ReturnChallan Where ISNULL(ApprovedBy,0)=0 AND BUID=" + buid + " AND ProductNature = " + ProductNature;
            _oReturnChallans = ReturnChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oReturnChallans);
        }

        public ActionResult ViewReturnChallan(int id, int buid)
        {
            _oReturnChallan = new ReturnChallan();
            if (id > 0)
            {
                _oReturnChallan = _oReturnChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oReturnChallan.ReturnChallanDetails = ReturnChallanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.ReturnChallan, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oReturnChallan);
        }


        [HttpPost]
        public JsonResult Save(ReturnChallan oReturnChallan)
        {
            _oReturnChallan = new ReturnChallan();
            try
            {
                short nDBOperation = (short)((oReturnChallan.ReturnChallanID <= 0) ? EnumDBOperation.Insert : EnumDBOperation.Update);
                _oReturnChallan = oReturnChallan.IUD(nDBOperation, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oReturnChallan = new ReturnChallan();
                _oReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(ReturnChallan oReturnChallan)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oReturnChallan.ReturnChallanID <= 0)
                    throw new Exception("Invalid Return challan.");
                oReturnChallan = oReturnChallan.IUD((short)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oReturnChallan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approve(ReturnChallan oReturnChallan)
        {
            try
            {
                oReturnChallan.ApprovedBy = (int)Session[SessionInfo.currentUserID];
                oReturnChallan = oReturnChallan.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oReturnChallan = new ReturnChallan();
                oReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsExportSCs(ExportSC oExportSC)
        {
            _oExportSCs = new List<ExportSC>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportSC WHERE BUID=" + oExportSC.BUID + " AND ProductNature = " + oExportSC.ProductNatureInInt + " AND ExportSCID IN (SELECT DC.ExportSCID FROM View_DeliveryChallan AS DC WHERE ISNULL(DC.ApproveBy,0)!=0 AND ISNULL(DC.YetToReturnChallanQty,0)>0 AND DC.ChallanDate Between  DATEADD(YEAR,-1, Getdate()) AND GETDATE())";
                if (oExportSC.PINo != null && oExportSC.PINo != "")
                {
                    sSQL += " And PINo Like '%" + oExportSC.PINo.Trim() + "%'";
                }
                if (oExportSC.ContractorID > 0)
                {
                    sSQL += " And ContractorID = " + oExportSC.ContractorID + "";
                }
                _oExportSCs = ExportSC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSCs = new List<ExportSC>();
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
                _oExportSCs.Add(_oExportSC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Receive(ReturnChallan oReturnChallan)
        {
            try
            {
                oReturnChallan = oReturnChallan.Receive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oReturnChallan = new ReturnChallan();
                oReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Searching
        private string GetSQL(string sTemp)
        {

            int nBUID = Convert.ToInt32(sTemp.Split('~')[0]);
            string sChallanNo = sTemp.Split('~')[1];
            string sDONo = sTemp.Split('~')[2];
            short nCreateDateCom = Convert.ToInt16(sTemp.Split('~')[3]);
            DateTime dtDCFrom = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dtDCTo = Convert.ToDateTime(sTemp.Split('~')[5]);
            string sBuyerIDs = sTemp.Split('~')[6];
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[7]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[8]);
            int nProductNature = Convert.ToInt32(sTemp.Split('~')[9]);
            string sReturnNo = sTemp.Split('~')[10];
            string sReturn1 = "SELECT * FROM View_ReturnChallan";
            string sReturn = "";


            #region Return No
            if (!string.IsNullOrEmpty(sReturnNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReturnChallanNo LIKE '%" + sReturnNo+"%'";
            }
            #endregion

            #region BU
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region ProductNature
            if (nProductNature != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductNature = " + nProductNature;
            }
            #endregion

            

            #region sChallanNo
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReturnChallanID IN (SELECT ReturnChallanID FROM View_ReturnChallanDetail WHERE DeliveryChallanNo Like '%" + sChallanNo.Trim() + "%' )";
            }
            #endregion

            #region DO No
            if (!string.IsNullOrEmpty(sDONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReturnChallanID IN (SELECT ReturnChallanID FROM View_ReturnChallanDetail WHERE DONo Like '%" + sDONo.Trim() + "%' )";
            }
            #endregion

            #region REturn Date Wise
            if (nCreateDateCom > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + Global.DateSQLGenerator("ReturnDate", nCreateDateCom, dtDCFrom, dtDCTo, false);
            }

            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApproveBy,0) != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ISNULL(ApproveBy,0) = 0";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY ContractorID, ReturnChallanID";
            return sReturn;
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<ReturnChallan> oReturnChallans = new List<ReturnChallan>();
            try
            {
                string sSQL = GetSQL(Temp);
                oReturnChallans = ReturnChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oReturnChallans = new List<ReturnChallan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oReturnChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets Return Order from different Module
        [HttpPost]
        public JsonResult GetsDeliveryChallanDetail(ReturnChallan oReturnChallan)
        {
            List<DeliveryChallanDetail> oDeliveryChallanDetails = new List<DeliveryChallanDetail>();
            _oReturnChallanDetails = new List<ReturnChallanDetail>();
            try
            {
                string sSQL = " SELECT * FROM View_DeliveryChallanDetail WHERE RefDetailID IN ( SELECT ExportSCDetailID FROM ExportSCDetail  WHERE ExportSCID = " + oReturnChallan.ExportSCID + ") Order By DeliveryChallanDetailID Desc";
                oDeliveryChallanDetails = DeliveryChallanDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach(DeliveryChallanDetail oItem in oDeliveryChallanDetails)
                {
                    _oReturnChallanDetail = new ReturnChallanDetail();
                    _oReturnChallanDetail.ReturnChallanDetailID = 0;
                    _oReturnChallanDetail.ReturnChallanID = 0;
                    _oReturnChallanDetail.DeliveryChallanDetailID = oItem.DeliveryChallanDetailID;
                    _oReturnChallanDetail.DeliveryChallanID = oItem.DeliveryChallanID;
                    _oReturnChallanDetail.DeliveryChallanNo = oItem.ChallanNo;
                    _oReturnChallanDetail.DONo = oItem.DONo;
                    _oReturnChallanDetail.DeliveryChallanDetailID = oItem.DeliveryChallanDetailID;
                    _oReturnChallanDetail.ProductID = oItem.ProductID;
                    _oReturnChallanDetail.ProductName = oItem.ProductName;
                    _oReturnChallanDetail.MUnitID = oItem.MUnitID;
                    _oReturnChallanDetail.MUnit = oItem.MUnit;
                    _oReturnChallanDetail.Qty = oItem.YetToReturnQty;
                    _oReturnChallanDetail.DeliveryChallanQty = oItem.Qty;
                    _oReturnChallanDetail.YetToReturnQty = oItem.YetToReturnQty;
                    _oReturnChallanDetail.Note = "";
                    _oReturnChallanDetail.ColorName = oItem.ColorName;
                    _oReturnChallanDetails.Add(_oReturnChallanDetail);
                }

            }
            catch (Exception ex)
            {
                _oReturnChallanDetails = new List<ReturnChallanDetail>();
                _oReturnChallanDetail = new ReturnChallanDetail();
                _oReturnChallanDetail.ErrorMessage = ex.Message;
                _oReturnChallanDetails.Add(_oReturnChallanDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReturnChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Print
        [HttpPost]
        public ActionResult SetReturnChallanListData(ReturnChallan oReturnChallan)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oReturnChallan);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintReturnChallans()
        {
            _oReturnChallan = new ReturnChallan();
            try
            {
                _oReturnChallan = (ReturnChallan)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_ReturnChallan WHERE ReturnChallanID IN (" + _oReturnChallan.Note + ") Order By ReturnChallanID";
                _oReturnChallan.ReturnChallans = ReturnChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oReturnChallan = new ReturnChallan();
                _oReturnChallans = new List<ReturnChallan>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oReturnChallan.Company = oCompany;

            string Messge = "Return Challan List";
            rptReturnChallans oReport = new rptReturnChallans();
            byte[] abytes = oReport.PrepareReport(_oReturnChallan, Messge);
            return File(abytes, "application/pdf");
        }

        public ActionResult ReturnChallanPrintPreview(int id)
        {
            _oReturnChallan = new ReturnChallan();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oReturnChallan = _oReturnChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oReturnChallan.ReturnChallanDetails = ReturnChallanDetail.Gets(_oReturnChallan.ReturnChallanID, (int)Session[SessionInfo.currentUserID]);
                _oReturnChallan.BusinessUnit = oBusinessUnit.Get(_oReturnChallan.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oReturnChallan.BusinessUnit = new BusinessUnit();
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oReturnChallan.Company = oCompany;
            byte[] abytes;
            rptReturnChallan oReport = new rptReturnChallan();
            abytes = oReport.PrepareReport(_oReturnChallan);     
            return File(abytes, "application/pdf");
        }

        #endregion Print

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
    }

}
