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
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class BankReconciliationController : Controller
    {
        #region Declaration
        BankReconciliation _oBankReconciliation = new BankReconciliation();
        List<BankReconciliation> _oBankReconciliations = new List<BankReconciliation>();
        List<BankReconcilationStatement> _oBankReconcilationStatements = new List<BankReconcilationStatement>();
        BankReconcilationStatement _oBankReconcilationStatement = new BankReconcilationStatement();
        #endregion

        #region Actions
        public ActionResult ViewBankReconciliation(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BankReconciliation).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oBankReconciliation = new BankReconciliation();
            _oBankReconciliation.BUID = buid;
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            //ViewBag.ReconcileDataTypes = EnumObject.jGets(typeof(EnumReconcileDataType));
            return View(_oBankReconciliation);
        }

        public ActionResult ViewBRBalanceTransfer(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BankReconciliation).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oBankReconciliation = new BankReconciliation();
            _oBankReconciliation.BUID = buid;
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.AccountingSessions = AccountingSession.GetRunningFreezeAccountingYear((int)Session[SessionInfo.currentUserID]);
            return View(_oBankReconciliation);
        }
        #endregion


        #region Bank Reconcilation Statement
        //public ActionResult ViewBankReconciliationStatement(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);

        //    _oBankReconciliation = new BankReconciliation();
        //    //_oBankReconciliation.BUID = buid;
        //    ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
        //    oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
        //    ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
        //    ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
        //    //ViewBag.ReconcileDataTypes = EnumObject.jGets(typeof(EnumReconcileDataType));
            
        //    return View(_oBankReconciliation);
        //}
        #endregion



        #region Post Method
        [HttpPost]
        public JsonResult SaveBankDate(BRObj oBRObj)
        {
            _oBankReconciliation = new BankReconciliation();
            try
            {
                _oBankReconciliation = this.MapTempObjToBankReconciliation(oBRObj);                
                _oBankReconciliation = _oBankReconciliation.Save((int)Session[SessionInfo.currentUserID]);
                oBRObj = this.MapBRToTempObj(_oBankReconciliation);
            }
            catch (Exception ex)
            {
                oBRObj = new BRObj();
                oBRObj.EMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBRObj);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRemarks(BRObj oBRObj)
        {
            _oBankReconciliation = new BankReconciliation();
            try
            {
                _oBankReconciliation = this.MapTempObjToBankReconciliation(oBRObj);
                _oBankReconciliation = _oBankReconciliation.UpdateRemarks((int)Session[SessionInfo.currentUserID]);
                oBRObj = this.MapBRToTempObj(_oBankReconciliation);
            }
            catch (Exception ex)
            {
                oBRObj = new BRObj();
                oBRObj.EMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBRObj);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MultiSave(BankReconciliation oBankReconciliation)
        {            
            try
            {                
                List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
                foreach (BRObj oItem in oBankReconciliation.BRObjs)
                {
                    oBankReconciliations.Add(this.MapTempObjToBankReconciliation(oItem));
                }
                oBankReconciliation.BankReconciliations = oBankReconciliations;
                oBankReconciliations = BankReconciliation.MultiSave(oBankReconciliation, (int)Session[SessionInfo.currentUserID]);
                if (oBankReconciliations.Count > 0)
                {
                    if (oBankReconciliations[0].ErrorMessage == "")
                    {
                        oBankReconciliation = this.MapBankReconciliationToTempObj(oBankReconciliations);
                    }
                    else
                    {
                        oBankReconciliation = new BankReconciliation();
                        oBankReconciliation.ErrorMessage = oBankReconciliations[0].ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                oBankReconciliation = new BankReconciliation();
                oBankReconciliation.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(oBankReconciliation, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AuthorizeBankReconciliation(BankReconciliation oBankReconciliation)
        {
            string sFeedBackMessage = "";
            try
            {
                List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
                List<BankReconciliation> oTempBankReconciliations = new List<BankReconciliation>();
                foreach (BRObj oItem in oBankReconciliation.BRObjs)
                {
                    oBankReconciliations.Add(this.MapTempObjToBankReconciliation(oItem));
                }
                sFeedBackMessage = BankReconciliation.AuthorizeBankReconciliation(oBankReconciliations, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Delete(BankReconciliation oBankReconciliation)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBankReconciliation.Delete(oBankReconciliation.BankReconciliationID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsCOAByCodeOrName(ChartsOfAccount oChartsOfAccount)
        {
            VoucherType oVoucherType = new VoucherType();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            oChartsOfAccounts = ChartsOfAccount.GetsByCodeOrName(oChartsOfAccount, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PrepareBankReconciliation(BankReconciliation oBankReconciliation)
        {
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            oBankReconciliations = BankReconciliation.PrepareBankReconciliation(oBankReconciliation, (int)Session[SessionInfo.currentUserID]);
            oBankReconciliation = this.MapBankReconciliationToTempObj(oBankReconciliations);

            var jsonResult = Json(oBankReconciliation, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetLastDate(ACCostCenter oACCostCenter)
        {
            string sStartDate = DateTime.Today.ToString("dd MMM yyyy");
            try
            {
                BankReconciliation oBankReconciliation = new BankReconciliation();
                oBankReconciliation = oBankReconciliation.GetLastTransaction(oACCostCenter.ACCostCenterID, (int)Session[SessionInfo.currentUserID]);
                if (oBankReconciliation.BankReconciliationID > 0)
                {
                    //sStartDate = (oBankReconciliation.ReconcilDate.AddDays(1)).ToString("dd MMM yyyy");
                    sStartDate = oBankReconciliation.ReconcilDate.ToString("dd MMM yyyy");
                }
            }
            catch (Exception ex)
            {
                sStartDate = DateTime.Today.ToString("dd MMM yyyy");
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sStartDate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubLedgers(BankReconciliationOpenning oBankReconciliationOpenning)
        {
            List<BankReconciliationOpenning> oBankReconciliationOpennings = new List<BankReconciliationOpenning>();
            BankReconciliationOpenning oNewBankReconciliationOpenning = new BankReconciliationOpenning();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            Currency oCurrency = new Currency();
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            string sSQL = "SELECT * FROM View_ACCostCenter AS HH" ;
                    sSQL += " WHERE HH.ACCostCenterID IN (SELECT CCT.CCID FROM CostCenterTransaction AS CCT WHERE CCT.VoucherDetailID IN(SELECT VD.VoucherDetailID FROM VoucherDetail AS VD WHERE VD.AccountHeadID="+oBankReconciliationOpenning.AccountHeadID+"))";
                    sSQL += " OR HH.ACCostCenterID IN (SELECT BRO.SubledgerID FROM BankReconciliationOpenning AS BRO WHERE BRO.AccountHeadID=" + oBankReconciliationOpenning.AccountHeadID + ")";
            oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccount = oChartsOfAccount.Get(oBankReconciliationOpenning.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            oCurrency = oCurrency.Get(oBankReconciliationOpenning.CurrencyID, (int)Session[SessionInfo.currentUserID]);
            foreach(ACCostCenter oItem in oACCostCenters)
            {
                oNewBankReconciliationOpenning = new BankReconciliationOpenning();
                oNewBankReconciliationOpenning.BusinessUnitID = oBankReconciliationOpenning.BusinessUnitID;
                oNewBankReconciliationOpenning.CurrencyID = oBankReconciliationOpenning.CurrencyID;
                oNewBankReconciliationOpenning.AccountHeadID = oBankReconciliationOpenning.AccountHeadID;
                oNewBankReconciliationOpenning.SubledgerID = oItem.ACCostCenterID;
                oNewBankReconciliationOpenning.SubledgerCode = oItem.Code;
                oNewBankReconciliationOpenning.SubledgerName = oItem.Name;
                oNewBankReconciliationOpenning.BUName = oItem.BUName;
                oNewBankReconciliationOpenning.AccountHeadName = oChartsOfAccount.AccountHeadName;
                oNewBankReconciliationOpenning.CurrencyName = oCurrency.CurrencyName;
                oBankReconciliationOpennings.Add(oNewBankReconciliationOpenning);
            }
         
            var jsonResult = Json(oBankReconciliationOpennings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult CommitBR(BankReconciliationOpenning oBRO)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage= oBRO.BRBalanceTranfer(oBRO, (int)Session[SessionInfo.currentUserID]);  
            }
            catch (Exception ex)
            {
                oBRO = new BankReconciliationOpenning();
                oBRO.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
         
        }
        #endregion

        #region Functions
        private BankReconciliation MapTempObjToBankReconciliation(BRObj oBRObj)
        {
            BankReconciliation oBankReconciliation = new BankReconciliation();
            oBankReconciliation.BankReconciliationID = oBRObj.BRID;
            oBankReconciliation.SubledgerID = oBRObj.SLID;
            oBankReconciliation.VoucherDetailID = oBRObj.VDID;
            oBankReconciliation.CCTID = oBRObj.CCTID;
            oBankReconciliation.AccountHeadID = oBRObj.AHID;
            oBankReconciliation.ReconcilHeadID = oBRObj.RHID;
            oBankReconciliation.ReconcilDate = oBRObj.RDate;
            oBankReconciliation.ReconcilRemarks = oBRObj.RRmrk;
            oBankReconciliation.IsDebit = oBRObj.IsDr;
            oBankReconciliation.Amount = oBRObj.Amount;
            oBankReconciliation.ReconcilStatus = oBRObj.RStatus;
            oBankReconciliation.ReconcilStatusInt = oBRObj.RStatusInt;
            oBankReconciliation.ReconcilBy = oBRObj.ReconcilBy;
            oBankReconciliation.AccountCode = oBRObj.AHCode;
            oBankReconciliation.AccountHeadName = oBRObj.AHName;
            oBankReconciliation.RCAHCode = oBRObj.RCAHCode;
            oBankReconciliation.RCAHName = oBRObj.RCAHName;
            oBankReconciliation.ReverseHead = oBRObj.RVAH;
            oBankReconciliation.VoucherID = oBRObj.VID;
            oBankReconciliation.VoucherDate = oBRObj.VDate;
            oBankReconciliation.VoucherNo = oBRObj.VNo;
            oBankReconciliation.DebitAmount = oBRObj.DrAmount;
            oBankReconciliation.CreditAmount = oBRObj.CrAmount;
            oBankReconciliation.CurrentBalance = oBRObj.CBalance;
            return oBankReconciliation;
        }
        private BankReconciliation MapBankReconciliationToTempObj(List<BankReconciliation> oBankReconciliations)
        {
            BankReconciliation oBankReconciliation = new BankReconciliation();
            int nSLNo = 0; string sClosingBlance = "";
            BRObj oBRObj = new BRObj();
            List<BRObj> oBRObjs = new List<BRObj>();            
            foreach (BankReconciliation oItem in oBankReconciliations)
            {
                if (oItem.VoucherID > 0)
                {
                    nSLNo++;
                    oBRObj = new BRObj();
                    oBRObj.SL = nSLNo;
                    oBRObj.BRID = oItem.BankReconciliationID;
                    oBRObj.SLID = oItem.SubledgerID;
                    oBRObj.VDID = oItem.VoucherDetailID;
                    oBRObj.CCTID = oItem.CCTID;
                    oBRObj.AHID = oItem.AccountHeadID;
                    oBRObj.RHID = oItem.ReconcilHeadID;
                    oBRObj.RDate = oItem.ReconcilDate;
                    oBRObj.RRmrk = oItem.ReconcilRemarks;
                    oBRObj.IsDr = oItem.IsDebit;
                    oBRObj.Amount = oItem.Amount;
                    oBRObj.RStatus = oItem.ReconcilStatus;
                    oBRObj.RStatusInt = oItem.ReconcilStatusInt;
                    oBRObj.ReconcilBy = oItem.ReconcilBy;
                    oBRObj.AHCode = oItem.AccountCode;
                    oBRObj.AHName = oItem.AccountHeadName;
                    oBRObj.RCAHCode = oItem.RCAHCode;
                    oBRObj.RCAHName = oItem.RCAHName;
                    oBRObj.RVAH = oItem.ReverseHead;
                    oBRObj.VID = oItem.VoucherID;
                    oBRObj.VDate = oItem.VoucherDate;
                    oBRObj.VNo = oItem.VoucherNo;
                    oBRObj.DrAmount = oItem.DebitAmount;
                    oBRObj.CrAmount = oItem.CreditAmount;
                    oBRObj.CBalance = oItem.CurrentBalance;
                    oBRObjs.Add(oBRObj);
                    sClosingBlance = oBRObj.CBalanceSt;
                }
                else
                {
                    oBankReconciliation.OpeningBalance = oItem.OpeningBalance;
                }
            }
            oBankReconciliation.ClosingBalanceSt = sClosingBlance;
            oBankReconciliation.BRObjs = oBRObjs;
            return oBankReconciliation;
        }
        private BRObj MapBRToTempObj(BankReconciliation oBankReconciliation)
        { 
            BRObj oBRObj = new BRObj();
            oBRObj = new BRObj();            
            oBRObj.BRID = oBankReconciliation.BankReconciliationID;
            oBRObj.SLID = oBankReconciliation.SubledgerID;
            oBRObj.VDID = oBankReconciliation.VoucherDetailID;
            oBRObj.CCTID = oBankReconciliation.CCTID;
            oBRObj.AHID = oBankReconciliation.AccountHeadID;
            oBRObj.RHID = oBankReconciliation.ReconcilHeadID;
            oBRObj.RDate = oBankReconciliation.ReconcilDate;
            oBRObj.RRmrk = oBankReconciliation.ReconcilRemarks;
            oBRObj.IsDr = oBankReconciliation.IsDebit;
            oBRObj.Amount = oBankReconciliation.Amount;
            oBRObj.RStatus = oBankReconciliation.ReconcilStatus;
            oBRObj.RStatusInt = oBankReconciliation.ReconcilStatusInt;
            oBRObj.ReconcilBy = oBankReconciliation.ReconcilBy;
            oBRObj.AHCode = oBankReconciliation.AccountCode;
            oBRObj.AHName = oBankReconciliation.AccountHeadName;
            oBRObj.RCAHCode = oBankReconciliation.RCAHCode;
            oBRObj.RCAHName = oBankReconciliation.RCAHName;
            oBRObj.RVAH = oBankReconciliation.ReverseHead;
            oBRObj.VID = oBankReconciliation.VoucherID;
            oBRObj.VDate = oBankReconciliation.VoucherDate;
            oBRObj.VNo = oBankReconciliation.VoucherNo;
            if (oBankReconciliation.IsDebit)
            {
                oBRObj.DrAmount = oBankReconciliation.Amount;
                oBRObj.CrAmount = 0;
            }
            else
            {
                oBRObj.DrAmount = 0;
                oBRObj.CrAmount = oBankReconciliation.Amount;
            }            
            return oBRObj;
        }

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
        #endregion

        #region Print 
        [HttpPost]
        public ActionResult SetPrintSessionData(BankReconciliation oBankReconciliation)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oBankReconciliation);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintBankReconciliation()
        {
            int nBUID = 0;
            int nSubledgerID = 0;
            string sErrorMessage = "Common";
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            BankReconciliation oBankReconciliation = new BankReconciliation();
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            List<BankReconciliation> oTempBankReconciliations = new List<BankReconciliation>();
            try
            {
                oBankReconciliation = (BankReconciliation)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oBankReconciliation = null;
            }

            if (oBankReconciliation != null)
            {
                nBUID = oBankReconciliation.BUID;
                nSubledgerID = oBankReconciliation.SubledgerID;
                dStartDate = oBankReconciliation.StartDate;
                dEndDate = oBankReconciliation.EndDate;
                sErrorMessage = oBankReconciliation.ErrorMessage;
                oBankReconciliations = BankReconciliation.PrepareBankReconciliation(oBankReconciliation, (int)Session[SessionInfo.currentUserID]);
                if (oBankReconciliation.ErrorMessage == "Closing")
                {
                    oTempBankReconciliations = new List<BankReconciliation>();
                    oTempBankReconciliations = oBankReconciliations.Where(x => x.ReconcilDate!= DateTime.MinValue).ToList();
                    oBankReconciliations = oTempBankReconciliations;
                }
                else if (oBankReconciliation.ErrorMessage == "Unaffected")
                {
                    oTempBankReconciliations = new List<BankReconciliation>();
                    oTempBankReconciliations = oBankReconciliations.Where(x => x.ReconcilDate== DateTime.MinValue).ToList();
                    oBankReconciliations = oTempBankReconciliations;
                }
                oBankReconciliation = this.MapBankReconciliationToTempObj(oBankReconciliations);
            }
            else
            {
                oBankReconciliation = new BankReconciliation();
            }

            Company oCompany = new Company();
            Currency oCurrency = new Currency();
            ACCostCenter oACCostCenter = new ACCostCenter();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCurrency = oCurrency.Get(oCompany.BaseCurrencyID, (int)Session[SessionInfo.currentUserID]);
            oACCostCenter = oACCostCenter.Get(nSubledgerID, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            oBankReconciliation.Company = oCompany;
            oBankReconciliation.Currency = oCurrency;
            oBankReconciliation.ErrorMessage = sErrorMessage;
            this.Session.Remove(SessionInfo.ParamObj);
            rptBankReconciliation orptBankReconciliation = new rptBankReconciliation();
            byte[] abytes = orptBankReconciliation.PrepareReport(oBankReconciliation, dStartDate, dEndDate, oACCostCenter.Name);
            return File(abytes, "application/pdf");            
        }

        public void ExportBankReconciliationToExcel()
        {
            int nBUID = 0;
            int nSubledgerID = 0;

            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            BankReconciliation oBankReconciliation = new BankReconciliation();
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            try
            {
                oBankReconciliation = (BankReconciliation)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oBankReconciliation = null;
            }

            if (oBankReconciliation != null)
            {
                nBUID = oBankReconciliation.BUID;
                nSubledgerID = oBankReconciliation.SubledgerID;
                dStartDate = oBankReconciliation.StartDate;
                dEndDate = oBankReconciliation.EndDate;
                oBankReconciliations = BankReconciliation.PrepareBankReconciliation(oBankReconciliation, (int)Session[SessionInfo.currentUserID]);
                oBankReconciliation = this.MapBankReconciliationToTempObj(oBankReconciliations);
            }
            else
            {
                oBankReconciliation = new BankReconciliation();
            }

            Company oCompany = new Company();
            Currency oCurrency = new Currency();
            ACCostCenter oACCostCenter = new ACCostCenter();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCurrency = oCurrency.Get(oCompany.BaseCurrencyID, (int)Session[SessionInfo.currentUserID]);
            oACCostCenter = oACCostCenter.Get(nSubledgerID, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            oBankReconciliation.Company = oCompany;
            oBankReconciliation.Currency = oCurrency;
            this.Session.Remove(SessionInfo.ParamObj);
            
            #region Export To Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Bank Reconciliation");
                sheet.Name = "Bank Reconciliation";
                sheet.Column(2).Width = 5;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 110;
                sheet.Column(5).Width = 25;
                sheet.Column(6).Width = 17;
                sheet.Column(7).Width = 17;
                sheet.Column(8).Width = 17;
                sheet.Column(9).Width = 17;
                nEndCol = 9;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBankReconciliation.Company.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Bank Reconciliation"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol+2]; cell.Merge = true;
                cell.Value = "Bank Account :"+oACCostCenter.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 3, nRowIndex, nStartCol +5]; cell.Merge = true;
                cell.Value = dStartDate.ToString("dd MMM yyyy") + " to " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol+6, nRowIndex, nStartCol +7]; cell.Merge = true;
                cell.Value = "Openning Balance :" + oBankReconciliation.OpeningBalanceSt; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Particulars"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Reconcile Remarks"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Voucher Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Bank Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 8];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nDebit = 0, nCredit = 0, nUnaffectedDebit = 0, nUnaffectedCredit = 0;
                foreach (BRObj oItem in oBankReconciliation.BRObjs)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.VNo; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.RVAH; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.RRmrk; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.VDateSt; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.RDateSt; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.DrAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.CrAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (oItem.RDateStRpt != "")
                    {
                        nDebit = nDebit + oItem.DrAmount;
                        nCredit = nCredit + oItem.CrAmount;
                    }
                    else
                    {
                        nUnaffectedDebit = nUnaffectedDebit + oItem.DrAmount;
                        nUnaffectedCredit = nUnaffectedCredit + oItem.CrAmount;
                    }
                    nEndRow = nRowIndex;
                    nRowIndex++;
                    nCount++;
                }

                #region Grand Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol + 4]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Total:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;                
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nDebit; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nCredit; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Closing Balance
                double nClosingBalance = (oBankReconciliation.OpeningBalance + nDebit - nCredit);
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol + 4]; cell.Merge = true;
                cell.Value = "Closing Balance: " + (nClosingBalance < 0 ? "(" + Global.MillionFormat(nClosingBalance * (-1)) + ")" : Global.MillionFormat(nClosingBalance)); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Unaffected:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nUnaffectedDebit; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nUnaffectedCredit; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BankReconciliation.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        
        public ActionResult PrintBankReconciliationFlow()
        {
            int nBUID = 0;
            int nSubledgerID = 0;
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            BankReconciliation oBankReconciliation = new BankReconciliation();
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            try
            {
                oBankReconciliation = (BankReconciliation)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oBankReconciliation = null;
            }

            if (oBankReconciliation != null)
            {
                nBUID = oBankReconciliation.BUID;
                nSubledgerID = oBankReconciliation.SubledgerID;
                dStartDate = oBankReconciliation.StartDate;
                dEndDate = oBankReconciliation.EndDate;
                oBankReconciliations = BankReconciliation.PrepareBankReconciliation(oBankReconciliation, (int)Session[SessionInfo.currentUserID]);
                oBankReconciliation = this.MapBankReconciliationToTempObj(oBankReconciliations);
            }
            else
            {
                oBankReconciliation = new BankReconciliation();
            }

            Company oCompany = new Company();
            Currency oCurrency = new Currency();
            ACCostCenter oACCostCenter = new ACCostCenter();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCurrency = oCurrency.Get(oCompany.BaseCurrencyID, (int)Session[SessionInfo.currentUserID]);
            oACCostCenter = oACCostCenter.Get(nSubledgerID, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }

            this.Session.Remove(SessionInfo.ParamObj);

            List<BankReconciliation> oBRObjsHeadWise = new List<BankReconciliation>();

            double nOpenningBalance = oBankReconciliation.OpeningBalance;
            oBRObjsHeadWise = oBankReconciliations.Where(x => (x.BankReconciliationID > 0) && (x.ReconcilDateSt != "")).ToList().GroupBy(x => new { x.ReverseHeadID, x.ReverseHeadIsLedger, x.AccountHeadName }, (key, grp) =>
            new BankReconciliation
            {
                ReverseHeadName = grp.First().ReverseHeadName,
                DebitAmount = grp.Sum(p => p.DebitAmount),
                CreditAmount = grp.Sum(p => p.CreditAmount),
                DrCAmount = grp.Sum(p => p.DrCAmount),
                CrCAmount = grp.Sum(p => p.CrCAmount),
                CUSymbol = grp.First().CUSymbol,
                IsDebit = grp.First().IsDebit,
            }).ToList();
            oBRObjsHeadWise = oBRObjsHeadWise.OrderByDescending(x => x.IsDebit).ThenBy(x => x.ReverseHeadName).ToList();
            //oBRObjsHeadWise = oBRObjsHeadWise.OrderByDescending;
            rptBankReconciliationFlow orptBankReconciliation = new rptBankReconciliationFlow();
            byte[] abytes = orptBankReconciliation.PrepareReport(oBRObjsHeadWise, dStartDate, dEndDate, oACCostCenter.Name, oCompany, oCurrency, nOpenningBalance);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region BankReconcilationStatement

        public ActionResult ViewBankReconciliationStatement(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BankReconcilationStatement).ToString() + "," + ((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oBankReconcilationStatement = new BankReconcilationStatement();
            _oBankReconcilationStatements = new List<BankReconcilationStatement>();
            //_oBankReconcilationStatements = BankReconcilationStatement.Gets((int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);
            _oBankReconcilationStatement.Company = oCompany;
            return View(_oBankReconcilationStatement);
        }

        [HttpPost]
        public JsonResult GetBankReconciliationStatements(BankReconcilationStatement oBankReconcilationStatement)
        {
            try
            {
                _oBankReconcilationStatement = new BankReconcilationStatement();
                _oBankReconcilationStatement.BankReconciliationStatements = oBankReconcilationStatement.GetBankReconcilationStatements(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeZero = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 0).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeOne = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 1).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeTwo = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 2).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeThree = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 3).ToList();

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);
                _oBankReconcilationStatement.Company = oCompany;
            }
            catch (Exception ex)
            {
                _oBankReconcilationStatement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankReconcilationStatement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SetBankReconcilationStatementListData(BankReconcilationStatement oBankReconcilationStatement)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oBankReconcilationStatement);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PrintBankReconcilationStatementList()
        {
            BankReconcilationStatement oBankReconcilationStatement = new BankReconcilationStatement();
            try
            {
                oBankReconcilationStatement = (BankReconcilationStatement)Session[SessionInfo.ParamObj];

                _oBankReconcilationStatement = new BankReconcilationStatement();
                _oBankReconcilationStatement.BankReconciliationStatements = oBankReconcilationStatement.GetBankReconcilationStatements(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeZero = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 0).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeOne = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 1).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeTwo = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 2).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeThree = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 3).ToList();

            }
            catch (Exception ex)
            {
                _oBankReconcilationStatement = new BankReconcilationStatement();
                _oBankReconcilationStatements = new List<BankReconcilationStatement>();
            }
            _oBankReconcilationStatement.Note = oBankReconcilationStatement.Note;
            _oBankReconcilationStatement.BalanceDate = oBankReconcilationStatement.BalanceDate;
            _oBankReconcilationStatement.ErrorMessage = oBankReconcilationStatement.ErrorMessage;

            rptBankReconcilationStatements oReport = new rptBankReconcilationStatements();
            byte[] abytes = oReport.PrepareReport(_oBankReconcilationStatement);
            return File(abytes, "application/pdf");
        }

        public void PrintBankReconcilationStatementListForExcel()
        {
            BankReconcilationStatement oBankReconcilationStatement = new BankReconcilationStatement();
            try
            {
                oBankReconcilationStatement = (BankReconcilationStatement)Session[SessionInfo.ParamObj];

                _oBankReconcilationStatement = new BankReconcilationStatement();
                _oBankReconcilationStatement.BankReconciliationStatements = oBankReconcilationStatement.GetBankReconcilationStatements(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeZero = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 0).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeOne = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 1).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeTwo = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 2).ToList();
                _oBankReconcilationStatement.BankReconciliationStatementsDataTypeThree = _oBankReconcilationStatement.BankReconciliationStatements.Where(x => x.DataType == 3).ToList();

                _oBankReconcilationStatement.Note = oBankReconcilationStatement.Note;
                _oBankReconcilationStatement.BalanceDate = oBankReconcilationStatement.BalanceDate;
                _oBankReconcilationStatement.ErrorMessage = oBankReconcilationStatement.ErrorMessage;

                //write code
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                #region Bank Reconcilation Statement
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Bank Reconcilation Statement in excel.");
                    sheet.Name = "Bank Reconcilation Statement in excel.";
                    sheet.Column(2).Width = 8; //SL
                    sheet.Column(3).Width = 15; //Payment date
                    sheet.Column(4).Width = 27; //Voucher No
                    sheet.Column(5).Width = 25; //party
                    sheet.Column(6).Width = 15; //amount
                    sheet.Column(7).Width = 20; //cheque no
                    sheet.Column(8).Width = 18; //Total


                    #region Report Header

                    cell=sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = _oBankReconcilationStatement.Note; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8];cell.Merge = true; cell.Value = "Bank Reconciliation Statement"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8];cell.Merge = true; cell.Value = "As on " + _oBankReconcilationStatement.BalanceDateInString; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8];cell.Merge = true; cell.Value = "Account No. " + _oBankReconcilationStatement.ErrorMessage; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;
                    
                    #endregion

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    #region Bank Reconciliation Statements Data Type Zero
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 7];cell.Merge = true; cell.Value = _oBankReconcilationStatement.BankReconciliationStatementsDataTypeZero[0].OperationHeadName; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = _oBankReconcilationStatement.BankReconciliationStatementsDataTypeZero[0].RefAmount.ToString("#,###.00;(#,###.00)"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;
                    #endregion

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    #region Bank Reconciliation Statements Data Type One

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8];cell.Merge = true; cell.Value = "Add:      List of payment in transit but not appearing on statement."; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; 
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Payment Date"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Party"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Cheque No"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    

                    #region detail
                    int nCount = 0;
                    double nTotalAmount = 0;
                    foreach (BankReconcilationStatement oItem in _oBankReconcilationStatement.BankReconciliationStatementsDataTypeOne)
                    {
                        nCount++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.PaymentDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.VoucherNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.PartyName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.RefAmount.ToString("#,###.00;(#,###.00)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.ChequeNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;

                        nTotalAmount += oItem.RefAmount;
                    }

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 7];cell.Merge = true; cell.Value = "Total Payment not appeared on bank statement: "; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = nTotalAmount.ToString("#,###.00;(#,###.00)"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    #endregion
                    #endregion

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    #region Bank Reconciliation Statements Data Type Two

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Less:    List of cheques outstanding/not appearing in bank statement."; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Payment Date"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Party"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Cheque No"; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;


                    #region detail
                    int nnCount = 0;
                    double nnTotalAmount = 0;
                    foreach (BankReconcilationStatement oItem in _oBankReconcilationStatement.BankReconciliationStatementsDataTypeTwo)
                    {
                        nnCount++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.PaymentDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.VoucherNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.PartyName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.RefAmount.ToString("#,###.00;(#,###.00)"); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.ChequeNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;    //= border.Top.Style = border.Left.Style = border.Right.Style

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;

                        nnTotalAmount += oItem.RefAmount;
                    }

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Total outstanding cheques not listed on bank statement:"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[rowIndex, 8]; cell.Value = nnTotalAmount.ToString("#,###.00;(#,###.00)"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    #endregion
                    #endregion

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    #region Bank Reconciliation Statements Data Type Three
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 7];cell.Merge = true; cell.Value = _oBankReconcilationStatement.BankReconciliationStatementsDataTypeThree[0].OperationHeadName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[rowIndex, 8]; cell.Value = _oBankReconcilationStatement.BankReconciliationStatementsDataTypeThree[0].RefAmount.ToString("#,###.00;(#,###.00)"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Excel_Of_BankReconcilationStatement.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion


            }
            catch (Exception ex)
            {
                _oBankReconcilationStatement = new BankReconcilationStatement();
                _oBankReconcilationStatements = new List<BankReconcilationStatement>();
            }

        }

        #endregion
    }
}
