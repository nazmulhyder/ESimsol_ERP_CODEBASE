
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;

namespace ESimSolFinancial.Controllers
{
	public class BTMAController : Controller
	{
		#region Declaration
		BTMA _oBTMA = new BTMA();
		List<BTMA> _oBTMAs = new  List<BTMA>();
        BTMADetail _oBTMADetail = new BTMADetail();
        List<BTMADetail> _oBTMADetails = new List<BTMADetail>();
        #endregion

		#region Actions
		public ActionResult ViewBTMAs(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			_oBTMAs = new List<BTMA>();
            _oBTMAs = BTMA.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MaxCTNo = _oBTMA.GetMaxCNo(((User)Session[SessionInfo.CurrentUser]).UserID).CertificateNo+1;
			return View(_oBTMAs);
		}
        public ActionResult ViewBTMACs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oBTMAs = new List<BTMA>();
            string SSQL = "SELECT * FROM View_BTMA AS HH WHERE ISNULL(HH.PrintBy,0)=0 Order By BTMAID";
            _oBTMAs = BTMA.Gets(SSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.buid = buid;
            //ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BusinessUnits = EnumObject.jGets(typeof(EnumBusinessUnitType)).Where(x=>x.id!=2 && x.id!=3 && x.id!=7 && x.id!=8 &&x.id!=9 && x.id!=10).ToList();
            ViewBag.MaxCTNo = _oBTMA.GetMaxCNo(((User)Session[SessionInfo.CurrentUser]).UserID).CertificateNo + 1;
            return View(_oBTMAs);
        }
        public ActionResult ViewBTMAC(int id)
        {
            _oBTMA = new BTMA();
            if (id > 0)
            {
                _oBTMA = _oBTMA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBTMA.BTMADetails = BTMADetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oBTMA);
        }
		public ActionResult ViewBTMA(int id)
		{
			_oBTMA = new BTMA();
			if (id > 0)
			{
                _oBTMA = _oBTMA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBTMA.BTMADetails = BTMADetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
			}
			return View(_oBTMA);
		}
		[HttpPost]
        public JsonResult Save(BTMA oBTMA)
		{
			_oBTMA = new BTMA();
			try
			{
				_oBTMA = oBTMA;
				_oBTMA = _oBTMA.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
			}
			catch (Exception ex)
			{
				_oBTMA = new BTMA();
				_oBTMA.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oBTMA);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult SaveBTMA(BTMA oBTMA)
        {
            _oBTMA = new BTMA();
            try
            {
                _oBTMA = oBTMA;
                _oBTMA = _oBTMA.SaveBTMA((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBTMA = new BTMA();
                _oBTMA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBTMA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(BTMA oBTMA)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oBTMA.Delete(oBTMA.BTMAID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeleteBTMA(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                BTMA oBTMA = new BTMA();
                sFeedBackMessage = oBTMA.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Update_PrintBy(BTMA oBTMA)
        {
            try
            {
                if (oBTMA.BTMAID > 0)
                {
                    oBTMA = oBTMA.Update_PrintBy(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else throw new Exception("Sorry, Invalid BTMA ID !!");
            }
            catch (Exception ex)
            {
                oBTMA = new BTMA();
                oBTMA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBTMA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult WaitForPrint(BTMA oBTMA)
        {
            _oBTMAs = new List<BTMA>();
            try
            {
                string sSQL = "SELECT * FROM View_BTMA AS HH WHERE ISNULL(HH.PrintBy,0)=0";
                _oBTMAs = BTMA.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBTMA = new BTMA();
                _oBTMA.ErrorMessage = ex.Message;
                _oBTMAs.Add(_oBTMA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBTMAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetbyLCNo(ExportLC oExportLC)
        {
            List<MasterLCMapping> oMasterLCMappings = new List<MasterLCMapping>();
            List<ExportLC> oExportLCs = new List<ExportLC>();
            string sReturn1 = "SELECT * FROM View_ExportLC ";
            string sReturn = "";

            #region Export LC NO
            if (!string.IsNullOrEmpty(oExportLC.ExportLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportLCNo LIKE '%" + oExportLC.ExportLCNo + "'";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "CurrentStatus in(" + (int)EnumExportLCStatus.None + "," + (int)EnumExportLCStatus.FreshLC + "," + (int)EnumExportLCStatus.Approved + "," + (int)EnumExportLCStatus.OutstandingLC + "," + (int)EnumExportLCStatus.RequestForAmendment + "," + (int)EnumExportLCStatus.AmendmentReceive + ") ";
            }
            #endregion

            #region BUID
            if (oExportLC.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oExportLC.BUID;
            }
            #endregion
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "  ExportLCID in (SELECT ExportLCID FROM ExportBill where ExportLCID!=" + oExportLC.ExportLCID + " and  ExportBillID not in (SELECT ExportBillID FROM BTMA))";


            string sSQL = sReturn1 + sReturn;
            oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = string.Join(",", oExportLCs.Select(k => k.ExportLCID).ToList());
            
            oMasterLCMappings = MasterLCMapping.Gets("SELECT MAP.* FROM View_MasterLCMapping MAP WHERE MAP.ExportLCID IN (" + sSQL + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportLCs.ForEach(x =>
            {
                if (oMasterLCMappings.FirstOrDefault() != null && oMasterLCMappings.FirstOrDefault().ExportLCID > 0 && oMasterLCMappings.Where(b => (b.ExportLCID == x.ExportLCID)).Count() > 0)
                {
                    //oMasterLCMappings.Where(p => (p.ExportLCID == x.ExportLCID) && p.ExportLCID > 0).FirstOrDefault().MasterLCNo;
                    sSQL = string.Join(",", oMasterLCMappings.Where(L => L.ExportLCID == x.ExportLCID).Select(k => k.MasterLCNo).ToList());
                    x.MasterLCNos = sSQL;
                    sSQL = string.Join(",", oMasterLCMappings.Where(L => L.ExportLCID == x.ExportLCID).Select(k => k.MasterLCDateSt).ToList());
                    x.MasterLCDates = sSQL;
                }
            });

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult SearchLCNoOrBillNo(BTMA oBTMA)
        {
            _oBTMAs = new List<BTMA>();
            try
            {
                string sSQL = "SELECT * FROM View_BTMA ";
                string sSReturn = "";
                if (oBTMA.ExportLCNo != "" && oBTMA.ExportLCNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " ExportLCNo LIKE '%" + oBTMA.ExportLCNo + "%'";
                }
                if (oBTMA.ExportBillNo != "" && oBTMA.ExportBillNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " ExportBillNo LIKE '%" + oBTMA.ExportBillNo + "%'";
                }
                sSQL += sSReturn;
                _oBTMAs = BTMA.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBTMA = new BTMA();
                _oBTMA.ErrorMessage = ex.Message;
                _oBTMAs.Add(_oBTMA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBTMAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region ADV SEARCH
        public ActionResult AdvanceSearch()
        {
            _oBTMA = new BTMA();
            return PartialView(_oBTMA);
        }
        [HttpPost]
        public JsonResult AdvSearch(BTMA oBTMA)
        {
            _oBTMAs = new List<BTMA>();
            try
            {
                string sSQL = this.GetSQL(oBTMA);
                _oBTMAs = BTMA.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBTMAs = new List<BTMA>();
                oBTMA = new BTMA();
                oBTMA.ErrorMessage = ex.Message;
                _oBTMAs.Add(oBTMA);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBTMAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(BTMA oBTMA)
        {
            EnumCompareOperator eEntryDate = (EnumCompareOperator)Convert.ToInt32(oBTMA.sSearchParam1.Split('~')[0]);
            DateTime dEntryStartDate = Convert.ToDateTime(oBTMA.sSearchParam1.Split('~')[1]);
            DateTime dEntryEndDate = Convert.ToDateTime(oBTMA.sSearchParam1.Split('~')[2]);

            EnumCompareOperator ePrintDate = (EnumCompareOperator)Convert.ToInt32(oBTMA.sSearchParam2.Split('~')[0]);
            DateTime dPrintStartDate = Convert.ToDateTime(oBTMA.sSearchParam2.Split('~')[1]);
            DateTime dPrintEndDate = Convert.ToDateTime(oBTMA.sSearchParam2.Split('~')[2]);


            string sReturn1 = "SELECT * FROM View_BTMA AS HH";
            string sReturn = "";

            #region BU
            if (oBTMA.BUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BUType = " + oBTMA.BUID;
            }
            #endregion

            #region Entry Date
            if (eEntryDate != EnumCompareOperator.None)
            {
                if (eEntryDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.LastUpdateDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eEntryDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.LastUpdateDateTime,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eEntryDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.LastUpdateDateTime,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eEntryDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.LastUpdateDateTime,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eEntryDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.LastUpdateDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eEntryDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.LastUpdateDateTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEntryEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Print Date
            if (ePrintDate != EnumCompareOperator.None)
            {
                if (ePrintDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.PrintDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePrintDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.PrintDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePrintDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.PrintDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePrintDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.PrintDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePrintDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.PrintDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePrintDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.PrintDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPrintEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY BTMAID ASC";
            return sReturn;
        }

        [HttpPost]
        public JsonResult SearchWaitForBTMA(BTMA oBTMA)
        {
            _oBTMAs = new List<BTMA>();
            List<ExportBill> _oExportBills = new List<ExportBill>();
            string sSReturn = ""; string sSQL = "";
            try
            {
                sSQL = "SELECT TOP 100* FROM View_ExportBill";
                int nValue = Convert.ToInt32(oBTMA.ErrorMessage);

                #region WAITING
                if (nValue == 1)
                {
                    sSReturn += " WHERE ExportBillID NOT IN (SELECT ExportBillID FROM View_BTMA) Order By ExportBillID DESC";
                    sSQL += sSReturn;
                    _oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (ExportBill oItem in _oExportBills)
                    {
                        _oBTMA = new BTMA();
                        _oBTMA.ExportLCNo = oItem.ExportLCNo;
                        _oBTMA.LCDate = oItem.LCRecivedDate;
                        _oBTMA.Amount = oItem.Amount;
                        _oBTMA.ExportBillNo = oItem.ExportBillNo;
                        _oBTMA.MushakNo = "";
                        _oBTMA.SupplierName = "";
                        _oBTMA.GarmentsQty = "";
                        _oBTMA.PrintByName = "Waiting For BTMA";
                        _oBTMAs.Add(_oBTMA);
                    }
                }
                #endregion

                else
                {
                    sSQL = "SELECT * FROM View_BTMA AS HH WHERE ISNULL(HH.PrintBy,0)=0";
                    _oBTMAs = BTMA.Gets(sSQL + " Order By BTMAID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                         
            }
            catch (Exception ex)
            {
                _oBTMA = new BTMA();
                _oBTMA.ErrorMessage = ex.Message;
                _oBTMAs.Add(_oBTMA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBTMAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult AdvSearch(BTMA oBTMA)
        //{
        //    List<BTMA> oBTMAs = new List<BTMA>();
        //    string sSQL = MakeSQL(oBTMA);
        //    if (sSQL == "Error")
        //    {
        //        _oBTMA = new BTMA();
        //        _oBTMA.ErrorMessage = "Please select a searching critaria.";
        //        oBTMAs = new List<BTMA>();
        //    }
        //    else
        //    {
        //        oBTMAs = new List<BTMA>();
        //        oBTMAs = BTMA.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        if (oBTMAs.Count == 0)
        //        {
        //            oBTMAs = new List<BTMA>();
        //        }
        //    }
        //    var jsonResult = Json(oBTMAs, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}


        private string MakeSQL(BTMA oBTMA)
        {
            string sParams = oBTMA.ErrorMessage;

            int nDateCriteria_LC = 0;
            DateTime dStart_LCDate = DateTime.Today;
            DateTime dEnd_LCDate = DateTime.Today;
            int nDateCriteria_Expirey = 0;
            DateTime dStart_ExpireyDate = DateTime.Today;
            DateTime dEnd_ExpireyDate = DateTime.Today;
            int nDateCriteria_Mushak = 0;
            DateTime dStart_MushakDate = DateTime.Today;
            DateTime dEnd_MushakDate = DateTime.Today;
            int nDateCriteria_GatePass = 0;
            DateTime dStart_GatePassDate = DateTime.Today;
            DateTime dEnd_GatePassDate = DateTime.Today;
            int nDateCriteria_Invoice = 0;
            DateTime dStart_InvoiceDate = DateTime.Today;
            DateTime dEnd_InvoiceDate = DateTime.Today;
            string sSupplierIDs = "";
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                nDateCriteria_LC = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_LCDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_LCDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_Expirey = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_ExpireyDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_ExpireyDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_Invoice = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_InvoiceDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_InvoiceDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_Mushak = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_MushakDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_MushakDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_GatePass = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_GatePassDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_GatePassDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sSupplierIDs = Convert.ToString(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_BTMA AS EB";
            string sReturn = "";

            #region DATE SEARCH
            //BillRelizationDate,PRCDate,ApplicationDate,BTMAIssueDate,RealizedDate,AuditCertDate
            MakeSQLByDate(ref sReturn, "LCDate", nDateCriteria_LC, dStart_LCDate, dEnd_LCDate);
            MakeSQLByDate(ref sReturn, "[LCExpireDate]", nDateCriteria_Expirey, dStart_ExpireyDate, dEnd_ExpireyDate);
            MakeSQLByDate(ref sReturn, "InvoiceDate", nDateCriteria_Invoice, dStart_InvoiceDate, dEnd_InvoiceDate);
            MakeSQLByDate(ref sReturn, "MushakDate", nDateCriteria_Mushak, dStart_MushakDate, dEnd_MushakDate);
            MakeSQLByDate(ref sReturn, "GatePassDate", nDateCriteria_GatePass, dStart_GatePassDate, dEnd_GatePassDate);
            #endregion


            #region Master LC
            if (!string.IsNullOrEmpty(oBTMA.MasterLCNos))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.MasterLCNos LIKE '%" + oBTMA.MasterLCNos+"%'";
            }
            #endregion
            #region Export LC No
            if (!string.IsNullOrEmpty(oBTMA.ExportLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCNo LIKE '%" + oBTMA.ExportLCNo + "%'";
            }
            #endregion
            #region Export Bill No
            if (!string.IsNullOrEmpty(oBTMA.ExportBillNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportBillNo LIKE '%" + oBTMA.ExportBillNo + "%'"; 
            }
            #endregion
            #region Mushak No
            if (!string.IsNullOrEmpty(oBTMA.MushakNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.MushakNo LIKE '%" + oBTMA.MushakNo + "%'"; 
            }
            #endregion
            #region Gate Pass No
            if (!string.IsNullOrEmpty(oBTMA.GatePassNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.GatePassNo  LIKE '%" + oBTMA.GatePassNo + "%'"; 
            }
            #endregion
            #region Supplier
            if (!string.IsNullOrEmpty(sSupplierIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.SupplierID  IN (" + sSupplierIDs + ")";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        private void MakeSQLByDate(ref string sReturn, string sSearchDate, int nDateCriteria, DateTime dStartDate, DateTime dEndDate)
        {
            #region ChalllanDate
            if (nDateCriteria > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nDateCriteria == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " EB." + sSearchDate + "= '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " != '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " > '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " < '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " BETWEEN '" + dStartDate.ToString("dd MMM yyy") + "' AND '" + dEndDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " NOT BETWEEN '" + dStartDate.ToString("dd MMM yyy") + "' AND '" + dEndDate.ToString("dd MMM yyy") + "' ";
                }
            }
            #endregion
        }
        #endregion
        
        #region GETS FUNCTIONS
        [HttpPost]
        public JsonResult GetMasterLC(ExportBill oExportBill)
        {
            List<MasterLC> oMasterLCs = new List<MasterLC>();

            if (oExportBill.ExportLCID > 0)
            {
                oMasterLCs = MasterLC.GetsByLCID(oExportBill.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            var oMLC = new string[]
                        {
                            string.Join(", ", oMasterLCs.Select(x => x.MasterLCNo)), 
                            string.Join(", ", oMasterLCs.Select(x => x.MasterLCDateSt))
                        };

            var jsonResult = Json(oMLC, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
     
        [HttpPost]
        public JsonResult GetsByLC(ExportBill oExportBill)
        {

            List<ExportBill> oExportBills = new List<ExportBill>();
            try
            {
                if (oExportBill.ExportLCID <= 0) { throw new Exception("Please select a valid Export LC."); }
                oExportBills = ExportBill.Gets("SELECT * FROM View_ExportBill where   ExportLCID=" + oExportBill.ExportLCID + " and ExportBillID not in (SELECT ExportBillID FROM BTMA where ExportBillID!=" + oExportBill.ExportBillID + " and ExportLCID=" + oExportBill.ExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsProduct(ExportBill oExportBill)
        {
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            string sSQL = "SELECT * FROM VIEW_ExportBillDetail " //ProductID,ProductName,MUnitID,MUName, AVG(UnitPrice) as UnitPrice, SUM(QTY) as Qty
                        + "WHERE ExportBillID =" + oExportBill.ExportBillID
                        + (string.IsNullOrEmpty(oExportBill.ErrorMessage) ? "" : "AND ProductName LIKE '%" + oExportBill.ErrorMessage + "%'")
                        ;//+" GROUP BY ProductID,ProductName,MUnitID,MUName ";
            try 
            {
                oExportBillDetails = ExportBillDetail.GetsBySQL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oExportBillDetails = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName,x.Construction,x.FabricWidth }, (key, grp) =>
                                         new ExportBillDetail
                                         {
                                             ProductID = key.ProductID,
                                             ProductName = key.ProductName,
                                             Construction = key.Construction,
                                             FabricWidth = key.FabricWidth,
                                             Qty = grp.Sum(x => x.Qty),
                                             UnitPrice = grp.Average(x => x.UnitPrice),
                                             MUnitID = grp.Select(x => x.MUnitID).FirstOrDefault(),
                                             MUName = grp.Select(x => x.MUName).FirstOrDefault(),
                                           
                                         }).ToList();
                foreach(ExportBillDetail oItem in  oExportBillDetails)
                {
                    oItem.ProductName = oItem.ProductName + ((String.IsNullOrEmpty(oItem.Construction)) ? "" : " CONST:" + oItem.Construction) + ((String.IsNullOrEmpty(oItem.FabricWidth)) ? "" : " WIDTH:" + oItem.FabricWidth);
                }

                //oExportBillDetails = oExportBillDetails.GroupBy(x => x.ProductID,x.PINo).Select(grp => new ExportBillDetail
                //{
                //    ProductID = grp.Key,
                //    ProductName = grp.Select(x => x.ProductName).FirstOrDefault(),
                //    PINo = grp.Select(x => x.PINo).FirstOrDefault(),
                //    MUnitID = grp.Select(x => x.MUnitID).FirstOrDefault(),
                //    MUName = grp.Select(x => x.MUName).FirstOrDefault(),
                //    Qty = grp.Sum(x => x.Qty),
                //    UnitPrice = grp.Average(x => x.UnitPrice)
                //}).ToList();
            }
            catch (Exception e) 
            {
                oExportBillDetails = new List<ExportBillDetail>();
                ExportBillDetail oExportBillDetail = new ExportBillDetail();
                oExportBillDetail.ErrorMessage = e.Message;
                oExportBillDetails.Add(oExportBillDetail);
            }
            
            var jsonResult = Json(oExportBillDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsByImportLC(ImportLC oImportLC)
        {
            List<ImportLC> oImportLCs = new List<ImportLC>();
            string sSQL = "SELECT * FROM View_ImportLC";
            string sReturn = "";
            if (!String.IsNullOrEmpty(oImportLC.ImportLCNo))
            {
                oImportLC.ImportLCNo = oImportLC.ImportLCNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ImportLCNo like '%" + oImportLC.ImportLCNo + "%'";
            }
            if (oImportLC.ContractorID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID =" + oImportLC.ContractorID + "";
            }
            if (oImportLC.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID =" + oImportLC.BUID + "";
            }
            //if (!String.IsNullOrEmpty(oImportLC.ErrorMessage))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "ImportLCID in (Select ImportInvoice.ImportLCID from ImportInvoice where Invoicetype in ("+(int)EnumImportPIType.LC_Foreign+","+(int)EnumImportPIType.LC_Local+") and ImportInvoiceid in (Select ImportInvoiceid from ImportInvoiceDetail where ProductID in ("+oImportLC.ErrorMessage+")))";
            //}

            try
            {
                sSQL = sSQL + "" + sReturn;
                //if (oExportBill.ExportLCID <= 0) { throw new Exception("Please select a valid Export LC."); }
                oImportLCs = ImportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportLCs = new List<ImportLC>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Actions Details
        [HttpPost]
        public JsonResult SaveDetail(BTMADetail oBTMADetail)
        {
            _oBTMADetail = new BTMADetail();
            try
            {
                _oBTMADetail = oBTMADetail;
                _oBTMADetail = _oBTMADetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBTMADetail = new BTMADetail();
                _oBTMADetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBTMADetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDetail(BTMADetail oBTMADetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBTMADetail.Delete(oBTMADetail.BTMADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region PRINT DETAIL
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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

        public ActionResult PrintBTMA(int id, int _nBUID)
        {
            BTMA oBTMA = new BTMA();
            DocPrintEngine oDocPrintEngine = new DocPrintEngine();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();

            oBTMA = oBTMA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBTMA.BTMAID > 0)
            {
                oBTMA.BTMADetails = BTMADetail.Gets(oBTMA.BTMAID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //SELECT TOP 1 * FROM [View_DocPrintEngine] WHERE Activity=1 AND LetterType=%n
                //oDocPrintEngine = oDocPrintEngine.GetActiveByType((int)EnumDocumentPrintType.BTMA_CASH_ASSISTANCE, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDocPrintEngine = DocPrintEngine.Gets("SELECT TOP 1 * FROM [View_DocPrintEngine] WHERE Activity=1 AND LetterType=" + (int)EnumDocumentPrintType.BTMA_CASH_ASSISTANCE + " AND BUID = " + oBTMA.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID).FirstOrDefault();
                if (oDocPrintEngine != null)
                    oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID=" + oDocPrintEngine.DocPrintEngineID + " Order By  CONVERT(int,SLNo)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(oBTMA.BUID, (int)Session[SessionInfo.currentUserID]);

            oBTMA.BTMADetails.ForEach(o => o.QtyTwo = (o.Qty * oMeasurementUnitCon.Value));
            oBTMA.BTMADetails.ForEach(o => o.MUNameTwo =  oMeasurementUnitCon.ToMUnit);
           

            if (oDocPrintEngine != null)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptBTMA_Doc oReport = new rptBTMA_Doc();
                byte[] abytes = oReport.PrepareReport(oBTMA, oCompany, oDocPrintEngine);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintBTMA_GSP(int id, int _nBUID)
        {
            BTMA oBTMA = new BTMA();
            DocPrintEngine oDocPrintEngine = new DocPrintEngine();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oBTMA = oBTMA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBTMA.BTMAID > 0)
            {
                oBTMA.BTMADetails = BTMADetail.Gets(oBTMA.BTMAID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oDocPrintEngine = oDocPrintEngine.GetActiveByType((int)EnumDocumentPrintType.BTMA_GSP_Facility, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDocPrintEngine = DocPrintEngine.Gets("SELECT TOP 1 * FROM [View_DocPrintEngine] WHERE Activity=1 AND LetterType=" + (int)EnumDocumentPrintType.BTMA_GSP_Facility + " AND BUID = " + oBTMA.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID).FirstOrDefault();
                if (oDocPrintEngine != null)
                    oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID=" + oDocPrintEngine.DocPrintEngineID + " Order By  CONVERT(int,SLNo)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(oBTMA.BUID, (int)Session[SessionInfo.currentUserID]);

            oBTMA.BTMADetails.ForEach(o => o.QtyTwo = (o.Qty * oMeasurementUnitCon.Value));
            oBTMA.BTMADetails.ForEach(o => o.MUNameTwo = oMeasurementUnitCon.ToMUnit);
           

            if (oDocPrintEngine != null)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptBTMA_Doc oReport = new rptBTMA_Doc();
                byte[] abytes = oReport.PrepareReport(oBTMA, oCompany, oDocPrintEngine);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
            
        }
        #endregion
	}

}
