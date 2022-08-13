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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class CashFlowDmSetupController : Controller
    {
        #region Declaration

        CashFlowDmSetup _oCashFlowDmSetup = new CashFlowDmSetup();
        List<CashFlowDmSetup> _oCashFlowDmSetups = new List<CashFlowDmSetup>();
        CashFlow _oCashFlow = new CashFlow();
        List<CashFlow> _oCashFlows = new List<CashFlow>();
        List<ChartsOfAccount> _oChartsOfAccountList = new List<ChartsOfAccount>();
        ChartsOfAccount _oChartsOfAccount = new ChartsOfAccount();
        #endregion

        #region Consumption Unit
        public ActionResult ViewCashFlowHeads(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<CashFlowHead> oCashFlowHeads = new List<CashFlowHead>();
            oCashFlowHeads = CashFlowHead.Gets((int)Session[SessionInfo.currentUserID]);
            return View(oCashFlowHeads);
        }

        public ActionResult ViewCashFlowHead(int id)
        {
            CashFlowHead oCashFlowHead = new CashFlowHead();
            if (id > 0)
            {
                oCashFlowHead = oCashFlowHead.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.DebitCredits = EnumObject.jGets(typeof(EnumDebitCredit));
            ViewBag.CashFlowHeadTypes = EnumObject.jGets(typeof(EnumCashFlowHeadType));
            return View(oCashFlowHead);
        }

        [HttpPost]
        public JsonResult SaveCashFlowHead(CashFlowHead oCashFlowHead)
        {
            try
            {
                oCashFlowHead = oCashFlowHead.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oCashFlowHead = new CashFlowHead();
                oCashFlowHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCashFlowHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteCashFlowHead(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                CashFlowHead oCashFlowHead = new CashFlowHead();
                sFeedBackMessage = oCashFlowHead.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult UpdateScequence(CashFlowHead oCashFlowHead)
        {
            string sFeedBackMessage = "";
            List<CashFlowHead> oCashFlowHeads = new List<CashFlowHead>();
            try
            {
                sFeedBackMessage = oCashFlowHead.UpdateScequence((int)Session[SessionInfo.currentUserID]);
                if (sFeedBackMessage == "Update Successfully")
                {
                    oCashFlowHeads = CashFlowHead.Gets((int)Session[SessionInfo.currentUserID]);
                }
                oCashFlowHead = new CashFlowHead();
                oCashFlowHead.CashFlowHeads = oCashFlowHeads;
                oCashFlowHead.ErrorMessage = sFeedBackMessage;
            }
            catch (Exception ex)
            {
                oCashFlowHead = new CashFlowHead();
                oCashFlowHead.ErrorMessage = ex.Message;
                oCashFlowHead.CashFlowHeads = new List<CashFlowHead>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCashFlowHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region GET Consumption Units
        public JsonResult CashFlowHeadPicker(CashFlowHead oCashFlowHead)
        {
            List<CashFlowHead> oCashFlowHeads = new List<CashFlowHead>();
            string sSQL = "SELECT * FROM View_BUWiseCashFlowHead ";
            //string sNextSql = "";
            //if (oCashFlowHead.BUID > 0)//if apply style configuration business unit
            //{
            //    Global.TagSQL(ref sNextSql);
            //    sNextSql += " BUID = " + oCashFlowHead.BUID;
            //}
            //if (!string.IsNullOrEmpty(oCashFlowHead.Name))
            //{
            //    Global.TagSQL(ref sNextSql);
            //    sNextSql += " CashFlowHeadName Like '%" + oCashFlowHead.Name + "%'";
            //}
            //sSQL += sNextSql;
            oCashFlowHeads = CashFlowHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCashFlowHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region CashFlowDmSetup
        public ActionResult ViewCashFlowDmSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CashFlowDmSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oCashFlowDmSetups = new List<CashFlowDmSetup>();
            _oCashFlowDmSetups = CashFlowDmSetup.Gets((int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM CashFlowHead WHERE CashFlowHeadType IN (" + (int)EnumCashFlowHeadType.Effected_Accounts + "," + (int)EnumCashFlowHeadType.Operating_Activities + "," + (int)EnumCashFlowHeadType.Investing_Activities + "," + (int)EnumCashFlowHeadType.Financing_Activities + ") Order BY Sequence";
            ViewBag.CashFlowHeads = CashFlowHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oCashFlowDmSetups);
        }

        [HttpPost]
        public JsonResult Save(CashFlowDmSetup oCashFlowDmSetup)
        {
            _oCashFlowDmSetup = new CashFlowDmSetup();
            try
            {
                _oCashFlowDmSetup = oCashFlowDmSetup;
                _oCashFlowDmSetup = _oCashFlowDmSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCashFlowDmSetup = new CashFlowDmSetup();
                _oCashFlowDmSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCashFlowDmSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
                sFeedBackMessage = oCashFlowDmSetup.Delete(id, (int)Session[SessionInfo.currentUserID]);
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

        #region Cash Flow Manages
        public ActionResult ViewCashFlows(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CashFlow).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oCashFlows = new List<CashFlow>();
            string sSQL = "SELECT * FROM CashFlowHead WHERE CashFlowHeadType IN (" + (int)EnumCashFlowHeadType.Operating_Activities + "," + (int)EnumCashFlowHeadType.Investing_Activities + "," + (int)EnumCashFlowHeadType.Financing_Activities + ") Order BY Sequence";
            ViewBag.CashFlowHeads = CashFlowHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountType = 4 ORDER BY AccountHeadName ASC";
            ViewBag.SubLedgers = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oCashFlows);
        }

        [HttpPost]
        public JsonResult GetCashFlowList(CashFlow oCashFlow)
        {
            _oCashFlow = new CashFlow();
            try
            {
                _oCashFlows = new List<CashFlow>();
                string sReturn1 = "SELECT * FROM View_CashFlow AS HH";
                string sReturn = "";

                #region BUID
                if (oCashFlow.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.BUID =" + oCashFlow.BUID.ToString();
                }
                #endregion

                #region CashFlowHeadID
                if (oCashFlow.CashFlowHeadID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.CashFlowHeadID =" + oCashFlow.CashFlowHeadID.ToString();
                }
                #endregion

                #region VoucherDate
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCashFlow.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCashFlow.EndDate.ToString("dd MMM yyyy") + "',106)) ";
                #endregion

                #region SubGroupID
                if (oCashFlow.SubGroupID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.SubGroupID =" + oCashFlow.SubGroupID.ToString();
                }
                #endregion

                #region Voucher Narration
                if (!string.IsNullOrEmpty(oCashFlow.Narration))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.Narration LIKE '%" + oCashFlow.Narration + "%'";
                }
                #endregion

                sReturn = sReturn1 + sReturn + " ORDER BY HH.VoucherDate, HH.VoucherDetailID ASC";
                oCashFlow.ErrorMessage = sReturn; //Here ErrorMessage Use As a SQL Crraiar
                _oCashFlows = CashFlow.GetsForCashManage(oCashFlow, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCashFlow = new CashFlow();
                _oCashFlow.ErrorMessage = ex.Message;
                _oCashFlows.Add(_oCashFlow);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCashFlows);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCashFlows(CashFlow oCashFlow)
        {

            try
            {
                _oCashFlows = CashFlow.UpdateCashFlows(oCashFlow.Narration, oCashFlow.CashFlowHeadID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCashFlow = new CashFlow();
                _oCashFlow.ErrorMessage = ex.Message;
                _oCashFlows.Add(_oCashFlow);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCashFlows);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCashFlowUnEffectedHead(ChartsOfAccount oChartsOfAccount)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            try
            {
                oChartsOfAccounts = new List<ChartsOfAccount>();
                string sSQL = "SELECT * FROM View_ChartsOfAccount AS COA WHERE COA.AccountHeadID IN (SELECT COA2.ParentHeadID FROM ChartsOfAccount AS COA2 WHERE COA2.AccountHeadID IN(SELECT VD3.AccountHeadID FROM VoucherDetail AS VD3 WHERE VD3.VoucherID IN (SELECT VD.VoucherID FROM VoucherDetail AS VD WHERE VD.VoucherDetailID IN (SELECT CFUnEH.VoucherDetailID FROM CashFlowUnEffectedHead AS CFUnEH)) AND VD3.AccountHeadID NOT IN (SELECT VD2.AccountHeadID FROM VoucherDetail AS VD2 WHERE VD2.VoucherDetailID IN (SELECT CFUnEH2.VoucherDetailID FROM CashFlowUnEffectedHead AS CFUnEH2))))";
                oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.ErrorMessage = ex.Message;
                oChartsOfAccounts = new List<ChartsOfAccount>();
                oChartsOfAccounts.Add(oChartsOfAccount);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region CashFlow Statement
        #region Set IncomeStatement SessionData
        [HttpPost]
        public ActionResult SetISSessionData(CashFlowDmSetup oCashFlowDmSetup)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oCashFlowDmSetup);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult ViewCashFlowDmStatement(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                _oCashFlowDmSetup = (CashFlowDmSetup)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oCashFlowDmSetup = null;
            }
            if (_oCashFlowDmSetup != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oCashFlowDmSetup.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oAccountingSession = oAccountingSession.GetSessionByDate(_oCashFlowDmSetup.EndDate, nUserID);
                if (oAccountingSession.AccountingSessionID > 0)
                {
                    if (oAccountingSession.StartDate > _oCashFlowDmSetup.StartDate)
                    {
                        _oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                    }
                }
                else
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                    _oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                    _oCashFlowDmSetup.EndDate = oAccountingSession.EndDate;
                }
                _oCashFlowDmSetups = CashFlowDmSetup.Gets(_oCashFlowDmSetup.BUID, _oCashFlowDmSetup.StartDate, _oCashFlowDmSetup.EndDate, _oCashFlowDmSetup.IsDetailsView, nUserID);
                _oCashFlowDmSetup.CashFlowDmSetups = new List<CashFlowDmSetup>();
                _oCashFlowDmSetup.CashFlowDmSetups = _oCashFlowDmSetups;
            }
            else
            {
                _oCashFlowDmSetup = new CashFlowDmSetup();
                _oCashFlowDmSetup.CashFlowDmSetups = new List<CashFlowDmSetup>();
                oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                _oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                _oCashFlowDmSetup.EndDate = oAccountingSession.EndDate;

                _oCashFlowDmSetups = CashFlowDmSetup.Gets(_oCashFlowDmSetup.BUID, _oCashFlowDmSetup.StartDate, _oCashFlowDmSetup.EndDate, _oCashFlowDmSetup.IsDetailsView, nUserID);
                _oCashFlowDmSetup.CashFlowDmSetups = new List<CashFlowDmSetup>();
                _oCashFlowDmSetup.CashFlowDmSetups = _oCashFlowDmSetups;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowDmSetup.BUID, nUserID);
            _oCashFlowDmSetup.Company = oCompany;
            if (oBusinessUnit.BusinessUnitID > 0) { _oCashFlowDmSetup.Company.Name = oBusinessUnit.Name; }
            _oCashFlowDmSetup.SessionDate = _oCashFlowDmSetup.StartDateSt + " To " + _oCashFlowDmSetup.EndDateSt;

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(_oCashFlowDmSetup);
        }

        public ActionResult ViewCFPaymentDetails()
        {

            int nUserID = (int)Session[SessionInfo.currentUserID];
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                _oCashFlowDmSetup = (CashFlowDmSetup)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oCashFlowDmSetup = null;
            }
            if (_oCashFlowDmSetup != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oCashFlowDmSetup.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oAccountingSession = oAccountingSession.GetSessionByDate(_oCashFlowDmSetup.EndDate, nUserID);
                if (oAccountingSession.AccountingSessionID > 0)
                {
                    if (oAccountingSession.StartDate > _oCashFlowDmSetup.StartDate)
                    {
                        _oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                    }
                }
                else
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                    _oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                    _oCashFlowDmSetup.EndDate = oAccountingSession.EndDate;
                }
                _oCashFlowDmSetups = CashFlowDmSetup.Gets(_oCashFlowDmSetup.BUID, _oCashFlowDmSetup.StartDate, _oCashFlowDmSetup.EndDate, _oCashFlowDmSetup.IsDetailsView, nUserID);
                _oCashFlowDmSetup.CashFlowDmSetups = new List<CashFlowDmSetup>();
                _oCashFlowDmSetup.CashFlowDmSetups = _oCashFlowDmSetups;
            }
            else
            {
                _oCashFlowDmSetup = new CashFlowDmSetup();
                _oCashFlowDmSetup.CashFlowDmSetups = new List<CashFlowDmSetup>();
                oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                _oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                _oCashFlowDmSetup.EndDate = oAccountingSession.EndDate;

                _oCashFlowDmSetups = CashFlowDmSetup.Gets(_oCashFlowDmSetup.BUID, _oCashFlowDmSetup.StartDate, _oCashFlowDmSetup.EndDate, _oCashFlowDmSetup.IsDetailsView, nUserID);
                _oCashFlowDmSetup.CashFlowDmSetups = new List<CashFlowDmSetup>();
                _oCashFlowDmSetup.CashFlowDmSetups = _oCashFlowDmSetups;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowDmSetup.BUID, nUserID);
            _oCashFlowDmSetup.Company = oCompany;
            if (oBusinessUnit.BusinessUnitID > 0) { _oCashFlowDmSetup.Company.Name = oBusinessUnit.Name; }
            _oCashFlowDmSetup.SessionDate = _oCashFlowDmSetup.StartDateSt + " To " + _oCashFlowDmSetup.EndDateSt;

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oCashFlowDmSetup);
        }

        public ActionResult PrintCashFlowStatement(string Params)
        {
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            bool IsPaymentDetails = false;
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            _oCashFlowDmSetup.BUID = nBUID;
            _oCashFlowDmSetup.StartDate = dStartDate;
            _oCashFlowDmSetup.EndDate = dEndDate;
            _oCashFlowDmSetup.IsDetailsView = IsPaymentDetails;
            _oCashFlowDmSetups = CashFlowDmSetup.Gets(_oCashFlowDmSetup.BUID, _oCashFlowDmSetup.StartDate, _oCashFlowDmSetup.EndDate, _oCashFlowDmSetup.IsDetailsView, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowDmSetup.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oCashFlowDmSetup.SessionDate = _oCashFlowDmSetup.StartDateSt + " To " + _oCashFlowDmSetup.EndDateSt;
            _oCashFlowDmSetup.CashFlowDmSetups = _oCashFlowDmSetups;

            rptCashFlowDmStatement orptCashFlowStatementStatement = new rptCashFlowDmStatement();
            abytes = orptCashFlowStatementStatement.PrepareReport(_oCashFlowDmSetup, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportCashFlowStatementToExcel(string Params)
        {

            #region Dataget
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            bool IsPaymentDetails = false;
            _oCashFlowDmSetup.BUID = nBUID;
            _oCashFlowDmSetup.StartDate = dStartDate;
            _oCashFlowDmSetup.EndDate = dEndDate;
            _oCashFlowDmSetup.IsDetailsView = IsPaymentDetails;
            _oCashFlowDmSetups = CashFlowDmSetup.Gets(_oCashFlowDmSetup.BUID, _oCashFlowDmSetup.StartDate, _oCashFlowDmSetup.EndDate, _oCashFlowDmSetup.IsDetailsView, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowDmSetup.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            _oCashFlowDmSetup.SessionDate = _oCashFlowDmSetup.StartDateSt + " To " + _oCashFlowDmSetup.EndDateSt;
            _oCashFlowDmSetup.CashFlowDmSetups = _oCashFlowDmSetups;

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Cash Flow Statement");
                sheet.Name = "Cash Flow Statement";
                sheet.Column(2).Width = 9;
                sheet.Column(3).Width = 9;
                sheet.Column(4).Width = 50;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 6;
                nEndCol = 7;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Statement of Cash Flows"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "For the Year Ended " + _oCashFlowDmSetup.EndDateSt; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _oCashFlowDmSetup.SessionDate + " " + oCompany.CurrencyName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Column Header
                nStartRow = nRowIndex; nEndRow = nRowIndex;
                #endregion

                #region Report Data

                CashFlowDmSetup oNetIncreaseFromCashflow = new CashFlowDmSetup();
                CashFlowDmSetup oTempCashFlowDmSetup = new CashFlowDmSetup();
                List<CashFlowDmSetup> oTempCashFlowDmSetups = new List<CashFlowDmSetup>();

                #region  Blank Space
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Operating Activities
                #region Operating_Opening_Caption
                oTempCashFlowDmSetup = GetSpecificList(2)[0]; //Operating_Opening_Caption = 2

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = " ";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3, nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;

                #endregion

                #region Operating_Activities
                oTempCashFlowDmSetups = GetSpecificList(3);//Operating_Activities = 3,
                int nCount = 0;
                foreach (CashFlowDmSetup oItem in oTempCashFlowDmSetups)
                {

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                    border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oItem.DisplayCaption; cell.Style.Font.Bold = false; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AmountSt; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                    if (nCount == 0) { border.Top.Style = ExcelBorderStyle.Thin; } if ((nCount + 1) == oTempCashFlowDmSetups.Count) { border.Bottom.Style = ExcelBorderStyle.Thin; }
                    border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    nCount++;
                }
                #endregion

                #region Operating_Closing_Caption
                oTempCashFlowDmSetup = GetSpecificList(4)[0]; //Operating_Closing_Caption = 4
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.AmountSt; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #endregion

                #region  Blank Space
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Investing Activities
                #region Investing_Opening_Caption
                oTempCashFlowDmSetup = GetSpecificList(5)[0]; //Investing_Opening_Caption = 5

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = " ";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3, nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;

                #endregion

                #region Investing_Activities
                oTempCashFlowDmSetups = GetSpecificList(6);//Investing_Activities = 3,
                nCount = 0;
                foreach (CashFlowDmSetup oItem in oTempCashFlowDmSetups)
                {

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                    border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oItem.DisplayCaption; cell.Style.Font.Bold = false; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AmountSt; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                    if (nCount == 0) { border.Top.Style = ExcelBorderStyle.Thin; } if ((nCount + 1) == oTempCashFlowDmSetups.Count) { border.Bottom.Style = ExcelBorderStyle.Thin; }
                    border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    nCount++;
                }
                #endregion

                #region Investing_Closing_Caption
                oTempCashFlowDmSetup = GetSpecificList(7)[0]; //Investing_Closing_Caption = 7
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.AmountSt; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #endregion

                #region  Blank Space
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Financing Activities
                #region Financing_Opening_Caption
                oTempCashFlowDmSetup = GetSpecificList(8)[0]; //Financing_Opening_Caption = 8

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = " ";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3, nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;

                #endregion

                #region Financing_Activities
                oTempCashFlowDmSetups = GetSpecificList(9);//Financing_Activities = 9,
                nCount = 0;
                foreach (CashFlowDmSetup oItem in oTempCashFlowDmSetups)
                {

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                    border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oItem.DisplayCaption; cell.Style.Font.Bold = false; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AmountSt; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                    if (nCount == 0) { border.Top.Style = ExcelBorderStyle.Thin; } if ((nCount + 1) == oTempCashFlowDmSetups.Count) { border.Bottom.Style = ExcelBorderStyle.Thin; }
                    border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    nCount++;
                }
                #endregion

                #region Financing_Closing_Caption
                oTempCashFlowDmSetup = GetSpecificList(10)[0]; //Financing_Closing_Caption = 10
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.AmountSt; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #endregion

                #region Net_Cash_Flow_Caption
                oTempCashFlowDmSetup = GetSpecificList(11)[0]; //Net_Cash_Flow_Caption = 11
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.AmountSt; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region  Blank Space
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Begaining_Cash_Caption
                oTempCashFlowDmSetup = GetSpecificList(12)[0]; //Begaining_Cash_Caption = 12
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.AmountSt; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Closing_Cash_Caption
                oTempCashFlowDmSetup = GetSpecificList(13)[0]; //Closing_Cash_Caption = 13
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oTempCashFlowDmSetup.DisplayCaption; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTempCashFlowDmSetup.AmountSt; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion


                #region Line

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = " "; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = " "; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                cell = sheet.Cells[nRowIndex, 6]; cell.Value = " "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = " "; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.None;
                border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 7];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=CashFlow_Statement.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        private List<CashFlowDmSetup> GetSpecificList(int nHeadType)
        {
            List<CashFlowDmSetup> oCashFlowDmSetups = new List<CashFlowDmSetup>();
            foreach (CashFlowDmSetup oItem in _oCashFlowDmSetups)
            {
                if (oItem.CashFlowHeadTypeInt == nHeadType)
                {
                    oCashFlowDmSetups.Add(oItem);
                }
            }
            return oCashFlowDmSetups;
        }

        public ActionResult PrintCashFlowBreakdown(string Params)
        {
            int nBUID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nCashFlowHeadID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            int nSubGroupID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            int nAccountHeadID = Params.Split('~')[5] == null ? 0 : Params.Split('~')[5] == "" ? 0 : Convert.ToInt32(Params.Split('~')[5]);

            
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            CashFlow oCashFlow = new CashFlow();
            oCashFlow.BUID = nBUID;
            oCashFlow.StartDate = dStartDate;
            oCashFlow.EndDate = dEndDate;
            oCashFlow.CashFlowHeadID = nCashFlowHeadID;



            List<CashFlow> oCashFlows = new List<CashFlow>();
            string sSQL = "SELECT * FROM View_CashFlow AS CF WHERE CF.BUID =" + nBUID.ToString() + " AND CF.CashFlowHeadID = " + nCashFlowHeadID.ToString() + " AND ISNULL(CF.AuthorizedBy,0)!=0  AND CONVERT(DATE,CONVERT(VARCHAR(12), CF.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12), '" + dEndDate.ToString("dd MMM yyyy") + "',106))";
            if (nSubGroupID > 0)
            {
                sSQL = sSQL + " AND CF.SubGroupID =" + nSubGroupID.ToString();
            }
            if (nAccountHeadID > 0)
            {
                sSQL = sSQL + " AND CF.AccountHeadID =" + nAccountHeadID.ToString();
            }
            sSQL = sSQL + " ORDER BY CF.VoucherDate, CF.VoucherDetailID ASC";
            oCashFlows = CashFlow.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCashFlowDmSetup.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            string sSessionDate = oCashFlow.StartDate.ToString("dd MMM yyyy") + " To " + oCashFlow.EndDate.ToString("dd MMM yyyy");
            CashFlowHead oCashFlowHead = new CashFlowHead();
            oCashFlowHead = oCashFlowHead.Get(nCashFlowHeadID, (int)Session[SessionInfo.currentUserID]);

            rptCashFlowDmBreakdown oCashFlowDmBreakdown = new rptCashFlowDmBreakdown();
            abytes = oCashFlowDmBreakdown.PrepareReport(oCashFlows, oCashFlowHead.DisplayCaption, oCashFlow.StartDate, oCashFlow.EndDate, oCompany);

            return File(abytes, "application/pdf");
        }

        public ActionResult ViewCashFlowBreakdowns(double tv)
        {
            int nUserID = (int)Session[SessionInfo.currentUserID];
            AccountingSession oAccountingSession = new AccountingSession();
            CashFlowDmSetup oCashFlowDmSetup = new CashFlowDmSetup();
            List<CashFlow> oCashFlows = new List<CashFlow>();
            try
            {
                oCashFlowDmSetup = (CashFlowDmSetup)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oCashFlowDmSetup = null;
            }
            if (oCashFlowDmSetup != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oCashFlowDmSetup.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oAccountingSession = oAccountingSession.GetSessionByDate(oCashFlowDmSetup.EndDate, nUserID);
                if (oAccountingSession.AccountingSessionID > 0)
                {
                    if (oAccountingSession.StartDate > oCashFlowDmSetup.StartDate)
                    {
                        oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                    }
                }
                else
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                    oCashFlowDmSetup.StartDate = oAccountingSession.StartDate;
                    oCashFlowDmSetup.EndDate = oAccountingSession.EndDate;
                }
                oCashFlows = CashFlow.GetCashFlowBreakDowns(oCashFlowDmSetup, nUserID);
                oCashFlowDmSetup.CashFlows = oCashFlows;
            }
            else
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Invalid Session!");
                return File(aErrorMessagebytes, "application/pdf");
            }
                        
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oCashFlowDmSetup.BUID, nUserID);
            oCashFlowDmSetup.Company = oCompany;
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCashFlowDmSetup.Company.Name = oBusinessUnit.Name;
            }
            oCashFlowDmSetup.SessionDate = oCashFlowDmSetup.StartDateSt + " To " + oCashFlowDmSetup.EndDateSt;
            
            CashFlowHead oCashFlowHead = new CashFlowHead();
            oCashFlowHead = oCashFlowHead.Get(oCashFlowDmSetup.CashFlowHeadID, (int)Session[SessionInfo.currentUserID]);
            oCashFlowDmSetup.CashFlowHeadName = oCashFlowHead.DisplayCaption;

            string makeTitle = "BU:" + oCashFlowDmSetup.Company.Name + " ||" + "Date: " + oCashFlowDmSetup.SessionDate + " ||" + "CF Head: " + oCashFlowDmSetup.CashFlowHeadName;
            if (oCashFlowDmSetup.SubGroupID > 0)
            {
                ChartsOfAccount oSubGroup = new ChartsOfAccount();
                oSubGroup = oSubGroup.Get(oCashFlowDmSetup.SubGroupID, (int)Session[SessionInfo.currentUserID]);
                makeTitle = makeTitle + " ||" + "Sub Group Name: " + oSubGroup.AccountHeadName;
            }            
            oCashFlowDmSetup.ErrorMessage = makeTitle;
            return View(oCashFlowDmSetup);
        }
        #endregion
    }
}
