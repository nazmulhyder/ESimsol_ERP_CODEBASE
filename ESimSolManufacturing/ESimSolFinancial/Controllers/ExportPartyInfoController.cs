using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using ReportManagement;
using iTextSharp.text;
using System.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class ExportPartyInfoController : Controller
    {
        #region Declaration
        ExportPartyInfo _oExportPartyInfo = new ExportPartyInfo();
        List<ExportPartyInfo> _oExportPartyInfos = new List<ExportPartyInfo>();
        ExportPartyInfoDetail _oExportPartyInfoDetail = new ExportPartyInfoDetail();
        #endregion

        #region Actions
        public ActionResult ViewExportPartyInfos(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);

            _oExportPartyInfos = new List<ExportPartyInfo>();
            _oExportPartyInfos = ExportPartyInfo.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
          
            return View(_oExportPartyInfos);
        }

         [HttpPost]
        public JsonResult Save(ExportPartyInfo oExportPartyInfo)
        {
            _oExportPartyInfo = new ExportPartyInfo();
            try
            {
                _oExportPartyInfo = oExportPartyInfo;
                
                _oExportPartyInfo = _oExportPartyInfo.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPartyInfo = new ExportPartyInfo();
                _oExportPartyInfo.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPartyInfo);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
         public JsonResult Get(ExportPartyInfo oExportPartyInfo)
         {
             _oExportPartyInfo = new ExportPartyInfo();
             try
             {
                 if (oExportPartyInfo.ExportPartyInfoID <= 0) { throw new Exception("Please select a valid contractor."); }
                 _oExportPartyInfo = _oExportPartyInfo.Get(oExportPartyInfo.ExportPartyInfoID, ((User)Session[SessionInfo.CurrentUser]).UserID);
             }
             catch (Exception ex)
             {
                 _oExportPartyInfo.ErrorMessage = ex.Message;
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oExportPartyInfo);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        [HttpPost]
         public JsonResult Delete(ExportPartyInfo oExportPartyInfo)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportPartyInfo.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActiveInactive(ExportPartyInfo oExportPartyInfo)
        {
            _oExportPartyInfo = new ExportPartyInfo();
            try
            {
                bool bActivity = true;
                _oExportPartyInfo = _oExportPartyInfo.Get(oExportPartyInfo.ExportPartyInfoID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
                _oExportPartyInfo = _oExportPartyInfo.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPartyInfo = new ExportPartyInfo();
                _oExportPartyInfo.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPartyInfo);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get
        [HttpPost]
        public JsonResult GetsAll()
        {
            _oExportPartyInfos = new List<ExportPartyInfo>();
            _oExportPartyInfos = ExportPartyInfo.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oExportPartyInfos);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Party wise Export Info
        public ActionResult ViewPartyWiseExportInfo(int pid)
        {
            Contractor oContractor =new Contractor();
            ExportPartyInfo oExportPartyInfo = new ExportPartyInfo();
            List<ExportPartyInfo> oExportPartyInfos = new List<ExportPartyInfo>();            
            ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
            oExportPartyInfo = new ExportPartyInfo();
            oExportPartyInfo.Name = "--Select Party Info--";
            oExportPartyInfos.Add(oExportPartyInfo);

            oExportPartyInfos.AddRange(ExportPartyInfo.Gets((int)Session[SessionInfo.currentUserID]));
            oContractor = oContractor.Get(pid, (int)Session[SessionInfo.currentUserID]);
            oExportPartyInfoDetail.ExportPartyInfoDetails = ExportPartyInfoDetail.GetsByParty(pid, (int)Session[SessionInfo.currentUserID]);
            oExportPartyInfoDetail.ExportPartyInfos = oExportPartyInfos;
            oExportPartyInfoDetail.ContractorID = oContractor.ContractorID;
            oExportPartyInfoDetail.PartyName = oContractor.Name;
            return View(oExportPartyInfoDetail);
        }
        public ActionResult ViewPartyWiseExportInfoLC(int pid)
        {
            ExportLC oExportLC = new ExportLC();
            ExportPartyInfo oExportPartyInfo = new ExportPartyInfo();
            List<ExportPartyInfo> oExportPartyInfos = new List<ExportPartyInfo>();
            ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
            oExportPartyInfo = new ExportPartyInfo();
            oExportPartyInfo.Name = "--Select Party Info--";
            oExportPartyInfos.Add(oExportPartyInfo);

            oExportPartyInfos.AddRange(ExportPartyInfo.Gets((int)Session[SessionInfo.currentUserID]));
            oExportLC = oExportLC.Get(pid, (int)Session[SessionInfo.currentUserID]);
            oExportPartyInfoDetail.ExportPartyInfoDetails = ExportPartyInfoDetail.GetsBy(oExportLC.ApplicantID, oExportLC.BBranchID_Issue, (int)Session[SessionInfo.currentUserID]);
            oExportPartyInfoDetail.ExportPartyInfos = oExportPartyInfos;
            oExportPartyInfoDetail.ContractorID = oExportLC.ApplicantID;
            oExportPartyInfoDetail.BankBranchID = oExportLC.BBranchID_Issue;
            oExportPartyInfoDetail.PartyName = oExportLC.ApplicantName + ", Bank:" + oExportLC.BankName_Issue + "[" + oExportLC.BBranchName_Issue+"]";
            return View(oExportPartyInfoDetail);
        }

        [HttpPost]
        public JsonResult SavePartyWiseExportInfo(ExportPartyInfoDetail oExportPartyInfoDetail)
        {
            _oExportPartyInfoDetail = new ExportPartyInfoDetail();
            try
            {
                _oExportPartyInfoDetail = oExportPartyInfoDetail;
                if (_oExportPartyInfoDetail.RefNo == null) { _oExportPartyInfoDetail.RefNo = ""; }
                if (_oExportPartyInfoDetail.RefDate == null) { _oExportPartyInfoDetail.RefDate = ""; }
                _oExportPartyInfoDetail = _oExportPartyInfoDetail.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportPartyInfoDetail = new ExportPartyInfoDetail();
                _oExportPartyInfoDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPartyInfoDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteExportPartyInfoDetail(ExportPartyInfoDetail oExportPartyInfoDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportPartyInfoDetail.Delete(oExportPartyInfoDetail.ExportPartyInfoDetailID, (int)Session[SessionInfo.currentUserID]);
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

        #region ExportPartyInfoBill
        [HttpPost]
        public JsonResult GetsExportPartyInfoBill(ExportPartyInfoBill oExportPartyInfoBill)
        {
            ExportPartyInfoBill oTempEPIB = new ExportPartyInfoBill();
            ExportPartyInfoBill oTempExportPartyInfoBill = new ExportPartyInfoBill();
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            List<ExportPartyInfo> oExportPartyInfos = new List<ExportPartyInfo>();
            List<ExportPartyInfoBill> oTempExportPartyInfoBills = new List<ExportPartyInfoBill>();
            try
            {
                oExportPartyInfos = ExportPartyInfo.Gets((int)Session[SessionInfo.currentUserID]);
                oTempExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(oExportPartyInfoBill.ReferenceID, (int)EnumMasterLCType.ExportLC, (int)Session[SessionInfo.currentUserID]);
                foreach (ExportPartyInfo oItem in oExportPartyInfos)
                {
                    oTempEPIB = this.GetExistingBillInfo(oTempExportPartyInfoBills, oItem.ExportPartyInfoID);
                    if (oTempEPIB.ExportPartyInfoBillID > 0)
                    {
                        oTempExportPartyInfoBill = new ExportPartyInfoBill();
                        oTempExportPartyInfoBill.ExportPartyInfoBillID = oTempEPIB.ExportPartyInfoBillID;
                        oTempExportPartyInfoBill.ReferenceID = oTempEPIB.ReferenceID;
                        oTempExportPartyInfoBill.ExportPartyInfoID = oTempEPIB.ExportPartyInfoID;
                        oTempExportPartyInfoBill.PartyInfo = oTempEPIB.PartyInfo;
                        oTempExportPartyInfoBill.Selected = true;
                        oExportPartyInfoBills.Add(oTempExportPartyInfoBill);
                    }
                    else
                    {
                        oTempExportPartyInfoBill = new ExportPartyInfoBill();
                        oTempExportPartyInfoBill.ExportPartyInfoBillID = 0;
                        oTempExportPartyInfoBill.ReferenceID = oExportPartyInfoBill.ReferenceID;
                        oTempExportPartyInfoBill.ExportPartyInfoID = oItem.ExportPartyInfoID;
                        oTempExportPartyInfoBill.PartyInfo = oItem.Name;
                        oTempExportPartyInfoBill.Selected = false;
                        oExportPartyInfoBills.Add(oTempExportPartyInfoBill);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.ErrorMessage = ex.Message;
                oExportPartyInfoBills.Add(oExportPartyInfoBill);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPartyInfoBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ExportPartyInfoBill GetExistingBillInfo(List<ExportPartyInfoBill> oExportPartyInfoBills, int nExportPartyInfoID)
        {
            ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
            foreach (ExportPartyInfoBill oItem in oExportPartyInfoBills)
            {
                if (oItem.ExportPartyInfoID == nExportPartyInfoID)
                {
                    return oItem;
                }                    
            }
            return oExportPartyInfoBill;
        }

        [HttpPost]
        public JsonResult SaveExportPartyInfoBills(ExportPartyInfoBill oExportPartyInfoBill)
        {            
            try
            {
                oExportPartyInfoBill = oExportPartyInfoBill.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPartyInfoBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}