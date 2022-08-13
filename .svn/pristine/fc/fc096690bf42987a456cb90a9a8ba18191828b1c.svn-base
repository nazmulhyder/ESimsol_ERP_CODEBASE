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
    public class ExportDocTnCController : Controller
    {
        #region Declaration
        ExportDocTnC _oExportDocTnC = new ExportDocTnC();
        List<ExportDocTnC> _oExportDocTnCs = new List<ExportDocTnC>();
       List<ExportDocForwarding> _oExportDocForwardings = new List<ExportDocForwarding>();
        string _sErrorMessage = "";
        #endregion


        #region Export Doc T and C Setup
        public ActionResult ViewExportDocTAndC()
        {
            return PartialView();
        }
        #endregion


        [HttpPost]
        public JsonResult Get_ExportDocTnCSetup(ExportLC oExportLC)
        {
            _oExportDocTnC = new ExportDocTnC();
            ExportPartyInfoBill oTempEPIB = new ExportPartyInfoBill();
            ExportPartyInfoBill oTempExportPartyInfoBill = new ExportPartyInfoBill();
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            List<ExportPartyInfo> oExportPartyInfos = new List<ExportPartyInfo>();
            List<ExportPartyInfoBill> oTempExportPartyInfoBills = new List<ExportPartyInfoBill>();
            _oExportDocForwardings = new List<ExportDocForwarding>();
            ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
            List<ExportDocForwarding> oNewExportDocForwardings = new List<ExportDocForwarding>();
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            ExportDocForwarding oTempExportDocForwarding = new ExportDocForwarding();
            int nReferenceID= oExportLC.RefTypeInInt==2?oExportLC.ExportLCID:oExportLC.MasterLCID;
            int nRefTypeInInt = oExportLC.RefTypeInInt;
            try
            {
                if (nReferenceID<= 0) { throw new Exception("Please select a valid LC."); }
                if (nReferenceID > 0)
                {
                    _oExportDocTnC = _oExportDocTnC.GetByLCID(nReferenceID,nRefTypeInInt,  ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oExportDocTnC.ExportLC = oExportLC;
                    _oExportDocTnC.ReferenceID = nReferenceID;//oExportLC.ExportLCID;
                    _oExportDocTnC.RefTypeInInt = nRefTypeInInt;
                    if (oExportLC.ApplicantID > 0)
                    {
                        oExportPartyInfos = ExportPartyInfo.Gets(oExportLC.ApplicantID, (int)Session[SessionInfo.currentUserID]);
                    }
                    else
                    {
                        oExportPartyInfos = ExportPartyInfo.Gets((int)Session[SessionInfo.currentUserID]);
                    }
                    oTempExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(nReferenceID, nRefTypeInInt, (int)Session[SessionInfo.currentUserID]);
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
                            oTempExportPartyInfoBill.ReferenceID = nReferenceID;
                            oTempExportPartyInfoBill.RefType = (EnumMasterLCType)nRefTypeInInt;
                            oTempExportPartyInfoBill.RefTypeInInt = nRefTypeInInt;
                            oTempExportPartyInfoBill.ExportPartyInfoID = oItem.ExportPartyInfoID;
                            oTempExportPartyInfoBill.PartyInfo = oItem.Name;
                            oTempExportPartyInfoBill.Selected = false;
                            oExportPartyInfoBills.Add(oTempExportPartyInfoBill);
                        }
                    }
                    _oExportDocTnC.ExportPartyInfoBills = oExportPartyInfoBills;
                }
                //Forwarding
              
                    oExportDocSetups = ExportDocSetup.Gets(true, oExportLC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ExportDocSetup oItem in oExportDocSetups)
                    {
                        oExportDocForwarding = new ExportDocForwarding();
                        oExportDocForwarding.ReferenceID = nReferenceID;
                        oExportDocForwarding.RefType = (EnumMasterLCType)nRefTypeInInt;
                        oExportDocForwarding.RefTypeInInt = nRefTypeInInt;
                        oExportDocForwarding.ExportDocID = oItem.ExportDocSetupID;
                        oExportDocForwarding.Name_Print = oItem.DocName;
                        oExportDocForwarding.Name_Doc = oItem.DocName;
                        if (oItem.DocumentType == EnumDocumentType.Commercial_Invoice)
                        {
                            oExportDocForwarding.Copies = 7;
                        }
                        if (oItem.DocumentType == EnumDocumentType.Packing_List)
                        {
                            oExportDocForwarding.Copies = 5;
                        }
                        if (oItem.DocumentType == EnumDocumentType.Delivery_Challan)
                        {
                            oExportDocForwarding.Copies = 2;
                        }
                        _oExportDocForwardings.Add(oExportDocForwarding);
                    }
                   
                    if (nReferenceID > 0)
                    {
                        oNewExportDocForwardings = ExportDocForwarding.Gets(nReferenceID,nRefTypeInInt,  ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    foreach (ExportDocForwarding oItem in _oExportDocForwardings)
                    {
                        oTempExportDocForwarding = this.GetExistingForwardingInfo(oNewExportDocForwardings, oItem.ExportDocID);
                        if(oTempExportDocForwarding.ExportDocForwardingID>0)
                        {
                            oItem.ExportDocForwardingID = oTempExportDocForwarding.ExportDocForwardingID;
                            oItem.Name_Print = oTempExportDocForwarding.Name_Print;
                            oItem.Copies = oTempExportDocForwarding.Copies;
                            oItem.Selected = true;
                        }
                        else
                        {
                            oItem.ExportDocForwardingID = 0;
                            oItem.Selected = false;
                        }
                        
                    }
                    _oExportDocTnC.ExportDocForwardings = _oExportDocForwardings;
            }
            catch (Exception ex)
            {
                _oExportDocTnC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocTnC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ExportDocTnCSetupExportLC(ExportLC oExportLC)
        {
            string sIDs = "";
            _oExportDocTnC = new ExportDocTnC();
           
            ExportPartyInfoBill oTempExportPartyInfoBill = new ExportPartyInfoBill();
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            List<ExportPartyInfoDetail> oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
           
            _oExportDocForwardings = new List<ExportDocForwarding>();
            ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
            List<ExportDocForwarding> oNewExportDocForwardings = new List<ExportDocForwarding>();
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            ExportDocForwarding oTempExportDocForwarding = new ExportDocForwarding();
            int nReferenceID = oExportLC.RefTypeInInt == 2 ? oExportLC.ExportLCID : oExportLC.MasterLCID;
            int nRefTypeInInt = oExportLC.RefTypeInInt;
            try
            {
                if (nReferenceID <= 0) { throw new Exception("Please select a valid LC."); }
                if (nReferenceID > 0)
                {
                    _oExportDocTnC = _oExportDocTnC.GetByLCID(nReferenceID, nRefTypeInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oExportDocTnC.ExportLC = oExportLC;
                    _oExportDocTnC.ReferenceID = nReferenceID;//oExportLC.ExportLCID;
                    _oExportDocTnC.RefTypeInInt = nRefTypeInInt;

                    oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(nReferenceID, nRefTypeInInt, (int)Session[SessionInfo.currentUserID]);
                    sIDs = string.Join(",", oExportPartyInfoBills.Select(x => x.ExportPartyInfoDetailID).ToList());

                    if (string.IsNullOrEmpty(sIDs))
                        sIDs = "0";
                    oExportPartyInfoDetails = ExportPartyInfoDetail.Gets(oExportLC.ApplicantID,oExportLC.BBranchID_Issue,sIDs, (int)Session[SessionInfo.currentUserID]);

                    oExportPartyInfoBills.ForEach(o => o.Selected = true);
                    foreach (ExportPartyInfoDetail oItem in oExportPartyInfoDetails)
                    {
                            oTempExportPartyInfoBill = new ExportPartyInfoBill();
                            oTempExportPartyInfoBill.ExportPartyInfoBillID = 0;
                            oTempExportPartyInfoBill.ReferenceID = nReferenceID;
                            oTempExportPartyInfoBill.RefType = (EnumMasterLCType)nRefTypeInInt;
                            oTempExportPartyInfoBill.RefTypeInInt = nRefTypeInInt;
                            oTempExportPartyInfoBill.ExportPartyInfoID = oItem.ExportPartyInfoID;
                            oTempExportPartyInfoBill.ExportPartyInfoDetailID = oItem.ExportPartyInfoDetailID;
                            oTempExportPartyInfoBill.PartyInfo = oItem.InfoCaption;
                            oTempExportPartyInfoBill.RefNo = oItem.RefNo;
                            oTempExportPartyInfoBill.RefDate = oItem.RefDate;
                            oTempExportPartyInfoBill.Selected = false;
                            oExportPartyInfoBills.Add(oTempExportPartyInfoBill);
                    }
                    _oExportDocTnC.ExportPartyInfoBills = oExportPartyInfoBills;
                }
                //Forwarding

              //  oExportDocSetups = ExportDocSetup.Gets(true, oExportLC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oExportDocSetups = ExportDocSetup.GetsByType((int)oExportLC.ExportLCTypeInt, oExportLC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportDocSetup oItem in oExportDocSetups)
                {
                    oExportDocForwarding = new ExportDocForwarding();
                    oExportDocForwarding.ReferenceID = nReferenceID;
                    oExportDocForwarding.RefType = (EnumMasterLCType)nRefTypeInInt;
                    oExportDocForwarding.RefTypeInInt = nRefTypeInInt;
                    oExportDocForwarding.ExportDocID = oItem.ExportDocSetupID;
                    oExportDocForwarding.Name_Print = oItem.DocName;
                    oExportDocForwarding.Name_Doc = oItem.DocName;
                    if (oItem.DocumentType == EnumDocumentType.Commercial_Invoice)
                    {
                        oExportDocForwarding.Copies = 7;
                    }
                    if (oItem.DocumentType == EnumDocumentType.Packing_List)
                    {
                        oExportDocForwarding.Copies = 5;
                    }
                    if (oItem.DocumentType == EnumDocumentType.Delivery_Challan)
                    {
                        oExportDocForwarding.Copies = 2;
                    }
                    _oExportDocForwardings.Add(oExportDocForwarding);
                }

                if (nReferenceID > 0)
                {
                    oNewExportDocForwardings = ExportDocForwarding.Gets(nReferenceID, nRefTypeInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                foreach (ExportDocForwarding oItem in _oExportDocForwardings)
                {
                    oTempExportDocForwarding = this.GetExistingForwardingInfo(oNewExportDocForwardings, oItem.ExportDocID);
                    if (oTempExportDocForwarding.ExportDocForwardingID > 0)
                    {
                        oItem.ExportDocForwardingID = oTempExportDocForwarding.ExportDocForwardingID;
                        oItem.Name_Print = oTempExportDocForwarding.Name_Print;
                        oItem.Copies = oTempExportDocForwarding.Copies;
                        oItem.Selected = true;
                    }
                    else
                    {
                        oItem.ExportDocForwardingID = 0;
                        oItem.Selected = false;
                    }

                }
                _oExportDocTnC.ExportDocForwardings = _oExportDocForwardings;
            }
            catch (Exception ex)
            {
                _oExportDocTnC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocTnC);
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

        public ExportDocForwarding GetExistingForwardingInfo(List<ExportDocForwarding> oExportDocForwardings, int nExportDocID)
        {
            ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
            foreach (ExportDocForwarding oItem in oExportDocForwardings)
            {
                if (oItem.ExportDocID == nExportDocID)
                {
                    return oItem;
                }
            }
            return oExportDocForwarding;
        }
        public ActionResult PickTruckReceipt()
        {
            ExportDocTnC oExportDocTnC = new ExportDocTnC();
            string sSQL = "SELECT * FROM ExportTR WHERE Activity=1";
            oExportDocTnC.ExportTRs = ExportTR.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(oExportDocTnC);
        }

        [HttpPost]
        public JsonResult Save(ExportDocTnC oExportDocTnC)
        {
            _oExportDocTnC = new ExportDocTnC();
            try
            {
                _oExportDocTnC = oExportDocTnC;
                _oExportDocTnC.RefType = (EnumMasterLCType)oExportDocTnC.RefTypeInInt;
                _oExportDocTnC = _oExportDocTnC.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportDocTnC = new ExportDocTnC();
                _oExportDocTnC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocTnC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}