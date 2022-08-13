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
    public class ExportDocSetupController : Controller
    {
        #region Declaration
        ExportDocSetup _oExportDocSetup = new ExportDocSetup();
        List<ExportDocSetup> _oExportDocSetups = new List<ExportDocSetup>();
        #endregion

        public ActionResult View_ExportDocSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            oExportDocSetups = ExportDocSetup.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumDocumentTypes = Enum.GetValues(typeof(EnumDocumentType)).Cast<EnumDocumentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            ViewBag.PrintNo = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.BankTypes = EnumObject.jGets(typeof(EnumBankType));
            ViewBag.GoodsDesViewType = EnumObject.jGets(typeof(EnumExportGoodsDesViewType));
            ViewBag.ExportLCTypes = EnumObject.jGets(typeof(EnumExportLCType));
            return View(oExportDocSetups);
        }
        public ActionResult View_ExportDocSetupsBill(int id, int buid)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            ExportBill oExportBill = new ExportBill();
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            List<ExportPartyInfo> oExportPartyInfos = new List<ExportPartyInfo>();
            List<ExportDocSetup> oExportDocSetups_Bill = new List<ExportDocSetup>();
            ExportPartyInfoBill oTempEPIB = new ExportPartyInfoBill();
            ExportPartyInfoBill oTempExportPartyInfoBill = new ExportPartyInfoBill();
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            List<ExportPartyInfoBill> oTempExportPartyInfoBills = new List<ExportPartyInfoBill>();
            
            oExportBill = oExportBill.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            oExportDocSetups = ExportDocSetup.Gets("SELECT * FROM View_ExportDocSetup WHERE ExportDocSetupID IN (SELECT ExportDocID FROM ExportDocForwarding WHERE ReferenceID in (select ExportBill.ExportLCID from ExportBill where exportBillID=" + id + ")  AND RefType =" + (int)EnumMasterLCType.ExportLC + " ) and ExportDocSetupID not in (Select ExportDocSetupID from ExportBillDoc where ExportBillID=" + id + ") ORDER BY Sequence ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportDocSetups_Bill = ExportDocSetup.GetsBy(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
          
            foreach (ExportDocSetup oItemDOD in oExportDocSetups)
            {
                    oItemDOD.ExportBillID = id;
                    oItemDOD.ContractorID = oExportBill.ApplicantID;
                    oExportDocSetups_Bill.Add(oItemDOD);
            }
            //oExportDocSetups_Bill.u
            oExportDocSetups_Bill=oExportDocSetups_Bill.OrderBy(x => x.Sequence).ToList();
            if (oExportBill.ApplicantID > 0)
            {
                oExportPartyInfos = ExportPartyInfo.Gets(oExportBill.ApplicantID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oExportPartyInfos = ExportPartyInfo.Gets((int)Session[SessionInfo.currentUserID]);
            }
            oTempExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(oExportBill.ExportLCID, (int)EnumMasterLCType.ExportLC, (int)Session[SessionInfo.currentUserID]);
            foreach (ExportPartyInfo oItem in oExportPartyInfos)
            {
                oTempEPIB = this.GetExistingBillInfo(oTempExportPartyInfoBills, oItem.ExportPartyInfoID);
                if (oTempEPIB.ExportPartyInfoBillID > 0)
                {
                    oTempExportPartyInfoBill = new ExportPartyInfoBill();
                    oTempExportPartyInfoBill.ExportPartyInfoBillID = oTempEPIB.ExportPartyInfoBillID;
                    oTempExportPartyInfoBill.ReferenceID = oTempEPIB.ReferenceID;
                    oTempExportPartyInfoBill.RefType = oTempEPIB.RefType;
                    oTempExportPartyInfoBill.RefTypeInInt = oTempEPIB.RefTypeInInt;
                    oTempExportPartyInfoBill.ExportPartyInfoID = oTempEPIB.ExportPartyInfoID;
                    oTempExportPartyInfoBill.PartyInfo = oTempEPIB.PartyInfo;
                    oTempExportPartyInfoBill.Selected = true;
                    oExportPartyInfoBills.Add(oTempExportPartyInfoBill);
                }
                else
                {
                    oTempExportPartyInfoBill = new ExportPartyInfoBill();
                    oTempExportPartyInfoBill.ExportPartyInfoBillID = 0;
                    oTempExportPartyInfoBill.ReferenceID = id;
                    oTempExportPartyInfoBill.RefType = (EnumMasterLCType)EnumMasterLCType.CommercialDoc;
                    oTempExportPartyInfoBill.RefTypeInInt = (int)EnumMasterLCType.CommercialDoc;
                    oTempExportPartyInfoBill.ExportPartyInfoID = oItem.ExportPartyInfoID;
                    oTempExportPartyInfoBill.PartyInfo = oItem.Name;
                    oTempExportPartyInfoBill.Selected = false;
                    oExportPartyInfoBills.Add(oTempExportPartyInfoBill);
                }
            }
            if (oExportBill.ExportBillID > 0)
            {
                oExportDocSetups_Bill.ForEach(o => o.ContractorID = oExportBill.ApplicantID);/// Update Con/Aplicant ID for Gets Partyinfo Doc Wise 
                oExportDocSetups_Bill.ForEach(o => o.ExportBillID = oExportBill.ExportBillID);
            }
            ViewBag.BUID = buid;
            ViewBag.ExportPartyInfoBills = oExportPartyInfoBills;
            ViewBag.NotifyBy = EnumObject.jGets(typeof(EnumNotifyBy));
            ViewBag.GoodsDesViewType = EnumObject.jGets(typeof(EnumExportGoodsDesViewType));
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.ExportBill = oExportBill;
            ViewBag.OrderOfBankType = EnumObject.jGets(typeof(EnumBankType));
            return View(oExportDocSetups_Bill);
        }
        //public ActionResult View_ExportDocSetupsBillLDBP(int id, int buid)
        //{
            
        //    return View();
        //}
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
        public JsonResult GetsPartyInfo(ExportDocSetup oExportDocSetup)
        {
            string sIDs = "";
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            List<ExportDocSetup> oExportDocSetups_Bill = new List<ExportDocSetup>();
            ExportBill oExportBill = new ExportBill();
            //ExportPartyInfoBill oTempExportPartyInfoBill = new ExportPartyInfoBill();
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            List<ExportPartyInfoDetail> oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
            ExportPartyInfoBill oTempExportPartyInfoBill = new ExportPartyInfoBill();

            oExportBill = oExportBill.Get(oExportDocSetup.ExportBillID, (int)Session[SessionInfo.currentUserID]);
            if (oExportDocSetup.ExportBillDocID > 0)
            {
                oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(oExportDocSetup.ExportBillDocID, (int)EnumMasterLCType.CommercialDoc, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oExportPartyInfoBills = ExportPartyInfoBill.Gets("SELECT * FROM View_ExportPartyInfoBill WHERE ReferenceID in (Select ExportBill.ExportLCID from ExportBill where  ExportBill.ExportBillID=" + oExportDocSetup.ExportBillID + " ) AND RefType=" + (int)EnumMasterLCType.ExportLC, (int)Session[SessionInfo.currentUserID]);
            }

            sIDs = string.Join(",", oExportPartyInfoBills.Where(o => o.ExportPartyInfoDetailID>0).ToList().Select(x => x.ExportPartyInfoDetailID).ToList());

            if (string.IsNullOrEmpty(sIDs))
                sIDs = "0";
            oExportPartyInfoDetails = ExportPartyInfoDetail.Gets(oExportDocSetup.ContractorID, oExportBill.BankBranchID_Issue, sIDs, (int)Session[SessionInfo.currentUserID]);

            oExportPartyInfoBills.ForEach(o => o.Selected = true);

            foreach (ExportPartyInfoDetail oItem in oExportPartyInfoDetails)
            {
               
                    oTempExportPartyInfoBill = new ExportPartyInfoBill();
                    oTempExportPartyInfoBill.ExportPartyInfoBillID = 0;
                    oTempExportPartyInfoBill.ReferenceID = oExportDocSetup.ExportBillDocID;
                    oTempExportPartyInfoBill.RefType = (EnumMasterLCType)EnumMasterLCType.CommercialDoc;
                    oTempExportPartyInfoBill.RefTypeInInt = (int)EnumMasterLCType.CommercialDoc;
                    oTempExportPartyInfoBill.ExportPartyInfoID = oItem.ExportPartyInfoID;
                    oTempExportPartyInfoBill.ExportPartyInfoDetailID = oItem.ExportPartyInfoDetailID;
                    oTempExportPartyInfoBill.PartyInfo = oItem.InfoCaption;
                    oTempExportPartyInfoBill.RefNo = oItem.RefNo;
                    oTempExportPartyInfoBill.RefDate = oItem.RefDate;
                    oTempExportPartyInfoBill.Selected = false;
                    oExportPartyInfoBills.Add(oTempExportPartyInfoBill);
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPartyInfoBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult ViewSetupSequence(int buid, double ts)
        {
            int nSequence = 0;
            _oExportDocSetups = new List<ExportDocSetup>();
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();            
            oExportDocSetups = ExportDocSetup.BUWiseGets(buid, (int)Session[SessionInfo.currentUserID]);

            foreach (ExportDocSetup oItem in oExportDocSetups)
            {
                nSequence++;
                _oExportDocSetup = new ExportDocSetup();
                _oExportDocSetup.Sequence = nSequence;
                _oExportDocSetup.ExportDocSetupID = oItem.ExportDocSetupID;
                _oExportDocSetup.DocName = oItem.DocName;
                _oExportDocSetups.Add(_oExportDocSetup);
            }
            return PartialView(_oExportDocSetups);
        }

        [HttpPost]
        public JsonResult UpdateSequence(ExportDocSetup oExportDocSetup)
        {
            _oExportDocSetups = new List<ExportDocSetup>();
            try
            {
                _oExportDocSetups = ExportDocSetup.UpdateSequence(oExportDocSetup, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportDocSetup = new ExportDocSetup();
                _oExportDocSetups = new List<ExportDocSetup>();
                _oExportDocSetup.ErrorMessage = ex.Message;
                _oExportDocSetups.Add(_oExportDocSetup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    

        [HttpPost]
        public JsonResult Save(ExportDocSetup oExportDocSetup)
        {
            oExportDocSetup.RemoveNulls();
            _oExportDocSetup = new ExportDocSetup();
            try
            {
                _oExportDocSetup = oExportDocSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportDocSetup = new ExportDocSetup();
                _oExportDocSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Bill(ExportDocSetup oExportDocSetup)
        {
            oExportDocSetup.RemoveNulls();
            _oExportDocSetup = new ExportDocSetup();
            try
            {
                _oExportDocSetup = oExportDocSetup.Save_Bill(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportDocSetup = new ExportDocSetup();
                _oExportDocSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveCopy(ExportDocSetup oExportDocSetup)
        {
            oExportDocSetup.RemoveNulls();
            _oExportDocSetup = new ExportDocSetup();
            try
            {

                oExportDocSetup.ExportDocSetupID = 0;
                _oExportDocSetup = oExportDocSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportDocSetup = new ExportDocSetup();
                _oExportDocSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ActivateExportDocSetup(ExportDocSetup oExportDocSetup)
        {
            _oExportDocSetup = new ExportDocSetup();
            string sMsg = "";
            _oExportDocSetup = oExportDocSetup.Activate(oExportDocSetup, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oExportDocSetup.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ExportDocSetup oExportDocSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportDocSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            oExportDocSetups = ExportDocSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oExportDocSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ExportDocSetupPicker(double ts)// EnumApplyModule{None = 0,ONS_ODS = 1,Work_Order = 2}
        //{
        //    List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
        //    oExportDocSetups = ExportDocSetup.Gets(true, (Guid)Session[SessionInfo.wcfSessionID]);
        //    return PartialView(oExportDocSetups);
        //}

    }
}
