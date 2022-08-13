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
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSolFinancial.Controllers
{
    public class VoucherController : Controller
    {
        #region Declaration
        Voucher _oVoucher = new Voucher();
        VoucherDetail _oVoucherDetail = new VoucherDetail();
        List<VoucherDetail> _oVoucherDetaillst = new List<VoucherDetail>();
        List<Voucher> _oVouchers = new List<Voucher>();
        CostCenterTransaction _oCostCenterTransaction = new CostCenterTransaction();
        List<CostCenterTransaction> _oCostCenterTransactions = new List<CostCenterTransaction>();
        List<VoucherReference> _oVoucherReferences = new List<VoucherReference>();
        List<VoucherBillTransaction> _oVoucherBillTransactions = new List<VoucherBillTransaction>();
        IntegrationSetup _oIntegrationSetup = new IntegrationSetup();
        List<IntegrationSetupDetail> _oIntegrationSetupDetails = new List<IntegrationSetupDetail>();
        List<DebitCreditSetup> _oDebitCreditSetups = new List<DebitCreditSetup>();
        List<DataCollectionSetup> _oDataCollectionSetups = new List<DataCollectionSetup>();      
        int _nBUID = 0;
        #endregion

        #region Validation Functions
        private string FeedBackForDebitCreditEqual(List<VoucherDetail> oVoucherDetails)
        {
            string sFeedBackMessage = "";
            double nSumDebitAmount = 0;
            double nSumCreditAmount = 0;
            if (oVoucherDetails != null)
            {
                if (oVoucherDetails.Count > 0)
                {
                    foreach (VoucherDetail oItem in oVoucherDetails)
                    {
                        if (oItem.IsDebit == true)
                        {
                            nSumDebitAmount = nSumDebitAmount + oItem.Amount;
                        }
                        else
                        {
                            nSumCreditAmount = nSumCreditAmount + oItem.Amount;
                        }
                    }
                    double nDifferAmount = nSumDebitAmount - nSumCreditAmount;
                    if (nDifferAmount < 0)
                    {
                        nDifferAmount = nDifferAmount * (-1);
                    }
                    if (nDifferAmount > 0.01)
                    {
                        sFeedBackMessage = "Please Confirm Debit & Credit Amount must be equal!\nYour Debit Amount:" + Global.MillionFormat(nSumDebitAmount) + " but Credit Amount:" + Global.MillionFormat(nSumCreditAmount);
                    }
                }
                else
                {
                    sFeedBackMessage = "Please Enter At Least One Debit Item";
                }
            }
            else
            {
                sFeedBackMessage = "Please Enter At Least One Debit Item";
            }
            return sFeedBackMessage;
        }

        private string FeedBackForDataValidation(List<VoucherDetail> oVoucherDetails)
        {
            string sFeedBackMessage = "";            
            if (oVoucherDetails != null)
            {
                if (oVoucherDetails.Count > 0)
                {
                    ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
                    oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.VoucherFormat, (int)Session[SessionInfo.currentUserID]);
                    if (oClientOperationSetting.ClientOperationSettingID <= 0)
                    {
                        return "Please setup Voucher Format in Client Operation Setting!";
                    }

                    foreach (VoucherDetail oItem in oVoucherDetails)
                    {
                        #region Voucher Detail
                        if (oItem.AccountHeadID <= 0)
                        {
                            return "Invalid Account Head!";
                        }
                        //if (oItem.BUID <= 0)
                        //{
                        //    return "Invalid Business Unit!";
                        //}
                        if (oItem.CurrencyID <= 0)
                        {
                            return "Invalid Currency!";
                        }
                        if (oItem.AmountInCurrency <= 0)
                        {
                            return "Invalid Amount In Currency!";
                        }
                        if(Convert.ToInt32(oClientOperationSetting.Value) != (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                        {
                            if (oItem.ConversionRate <= 0)
                            {
                                return "Invalid Conversion Rate!";
                            }
                        }
                        if (oItem.Amount<= 0)
                        {
                            return "Invalid Amount!";
                        }
                        #endregion                      
                    }                    
                }
                else
                {
                    sFeedBackMessage = "Please Enter At Least One Debit Item";
                }
            }
            else
            {
                sFeedBackMessage = "Please Enter At Least One Debit Item";
            }
            return sFeedBackMessage;
        }
        #endregion

        #region Actions
        public ActionResult ViewVouchers(string a, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Voucher).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));            
            _oVouchers = new List<Voucher>();
            if (a == "X")
            {
                //string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE (TT.RequestTo =" + ((int)Session[SessionInfo.currentUserID]).ToString() + ") ORDER BY VoucherBatchID ASC";
                string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE TT.BatchStatus IN (3,4) AND TT.UnApprovedVoucherCount>0 ORDER BY VoucherBatchID ASC";
                List<VoucherBatch> oVoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                ViewBag.VoucherBatchs = oVoucherBatchs;
                TempData["operation"] = "Approved";
            }
            else
            {
                string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE (TT.CreateBy =" + ((int)Session[SessionInfo.currentUserID]).ToString() + ") AND TT.BatchStatus<5 ORDER BY VoucherBatchID ASC";
                List<VoucherBatch> oVoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                ViewBag.VoucherBatchs = oVoucherBatchs;
                TempData["operation"] = "Entry";
            }

            #region Get User
            string sSQLQuery = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.AuthorizedBy FROM Voucher AS MM WHERE ISNULL(MM.AuthorizedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovedUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovedUser = new ESimSol.BusinessObjects.User();
            oApprovedUser.UserID = 0; oApprovedUser.UserName = "--Select Received User--";
            oApprovedUsers.Add(oApprovedUser);
            oApprovedUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQLQuery, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.ApprovedUsers = oApprovedUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.VoucherTypes = VoucherType.Gets((int)Session[SessionInfo.currentUserID]);
                        
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);            
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            ViewBag.ClientSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.VoucherFormat, (int)Session[SessionInfo.currentUserID]); //EnumClientOperationValueFormat : SingleCurrencyVoucher = 13, MultiCurrencyVoucher = 14             
            return View(_oVouchers);
        }
        public ActionResult ViewUnApproveVouchers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Voucher).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oVouchers = new List<Voucher>();            
            ViewBag.VoucherTypes = VoucherType.Gets((int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oVouchers);
        }
        public ActionResult ViewSingleCurrencyVoucher(int buid, int id, int nvtid, int copyid)
        {
            _oVoucher = new Voucher();
            _oVouchers = new List<Voucher>();
            Company oCompany = new Company();
            VoucherType oVoucherType = new VoucherType();
            Voucher oVoucher = new Voucher();
            Voucher oTempVoucher = new Voucher();

            if (id > 0)
            {
                _oVoucher = new Voucher();
                _oVoucher = _oVoucher.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oVoucher.VDObjs = MapVoucherExplanationObject(_oVoucher, false);
                string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE VoucherBatchID =" + _oVoucher.BatchID.ToString();
                ViewBag.VoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oVoucher = oVoucher.GetMaxDate(nvtid, buid, (int)Session[SessionInfo.currentUserID]);
                if (oVoucher.VoucherDate == null || oVoucher.VoucherDate == DateTime.MinValue)
                {
                    oVoucher.VoucherDate = DateTime.Today;
                }
                oTempVoucher = oTempVoucher.CommitVoucherNo(buid, nvtid, oVoucher.VoucherDate, (int)Session[SessionInfo.currentUserID]); // Here 2 refere VoucherTypeID that is PaymentVoucher & 1 refere jam company ID                
                _oVoucher.BUID = oTempVoucher.BUID;
                _oVoucher.VoucherNo = oTempVoucher.VoucherNo;
                _oVoucher.VoucherDate = oVoucher.VoucherDate;
                _oVoucher.CurrencyID = oTempVoucher.CurrencyID;
                _oVoucher.CUSymbol = oTempVoucher.CUSymbol;
                _oVoucher.CRate = 1;
                _oVoucher.VoucherTypeID = nvtid;
                _oVoucher.VoucherName = oTempVoucher.VoucherName;
                _oVoucher.NumberMethodInInt = oTempVoucher.NumberMethodInInt;
                _oVoucher.Narration = "";
                _oVoucher.PreparedByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
                _oVoucher.DBServerDate = DateTime.Now;
                _oVoucher.VDObjs = new List<VDObj>();
                string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE (TT.CreateBy =" + ((int)Session[SessionInfo.currentUserID]).ToString() + " AND  TT.BatchStatus=1) ORDER BY VoucherBatchID ASC";
                List<VoucherBatch> oVoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oVoucherBatchs.Count <= 0)
                {
                    VoucherBatch oVB = new VoucherBatch();
                    oVB.BatchStatus = EnumVoucherBatchStatus.BatchOpen;
                    oVB = oVB.Save((int)Session[SessionInfo.currentUserID]);
                    oVoucherBatchs.Add(oVB);
                }
                ViewBag.VoucherBatchs = oVoucherBatchs;
                if (oVoucherBatchs != null)
                {
                    if (oVoucherBatchs.Count > 0)
                    {
                        _oVoucher.BatchID = oVoucherBatchs[oVoucherBatchs.Count - 1].VoucherBatchID;
                    }
                }

                if (copyid > 0)
                {
                    oVoucher = new Voucher();
                    List<VDObj> oVDObjs = new List<VDObj>();
                    List<VDObj> oTempVDObjs = new List<VDObj>();
                    _oVoucher.VoucherID = copyid;
                    oVoucher = oVoucher.Get(copyid, (int)Session[SessionInfo.currentUserID]);
                    oTempVDObjs = MapVoucherExplanationObject(_oVoucher, false);
                    foreach (VDObj oItem in oTempVDObjs)
                    {
                        if (oItem.ObjType == EnumBreakdownType.VoucherDetail || oItem.ObjType == EnumBreakdownType.CostCenter || oItem.ObjType == EnumBreakdownType.InventoryReference)
                        {
                            oItem.VDObjID = 0;
                            oVDObjs.Add(oItem);
                        }
                    }
                    _oVoucher.VDObjs = oVDObjs;
                    _oVoucher.Narration = oVoucher.Narration;
                    _oVoucher.VoucherID = 0;
                }
            }

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            ViewBag.TaxEffects = EnumObject.jGets(typeof(EnumTaxEffect));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.VoucherTypes = VoucherType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.RunningAccountingYear = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
            ViewBag.ChequeTypes = Enum.GetValues(typeof(EnumChequeType)).Cast<EnumChequeType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oVoucher);
        }
        public ActionResult ViewMultiCurrencyVoucher(int buid, int id, int nvtid, int copyid)
        {
            _oVoucher = new Voucher();
            _oVouchers = new List<Voucher>();
            Company oCompany = new Company();
            VoucherType oVoucherType = new VoucherType();
            Voucher oVoucher = new Voucher();
            Voucher oTempVoucher = new Voucher();

            if (id > 0)
            {
                _oVoucher = new Voucher();
                _oVoucher = _oVoucher.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oVoucher.VDObjs = MapVoucherExplanationObject(_oVoucher, false);
                string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE VoucherBatchID =" + _oVoucher.BatchID.ToString();
                ViewBag.VoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                _oVoucher.VoucherAmount = _oVoucher.TotalAmount;
            }
            else
            {
                oVoucher = oVoucher.GetMaxDate(nvtid, buid, (int)Session[SessionInfo.currentUserID]);
                if (oVoucher.VoucherDate == null || oVoucher.VoucherDate == DateTime.MinValue)
                {
                    oVoucher.VoucherDate = DateTime.Today;
                }
                oTempVoucher = oTempVoucher.CommitVoucherNo(buid, nvtid, oVoucher.VoucherDate, (int)Session[SessionInfo.currentUserID]); // Here 2 refere VoucherTypeID that is PaymentVoucher & 1 refere jam company ID                
                _oVoucher.BUID = oTempVoucher.BUID;
                _oVoucher.VoucherNo = oTempVoucher.VoucherNo;
                _oVoucher.VoucherDate = oVoucher.VoucherDate;
                _oVoucher.CurrencyID = oTempVoucher.CurrencyID;
                _oVoucher.CUSymbol = oTempVoucher.CUSymbol;
                _oVoucher.BaseCurrencyID = oTempVoucher.CurrencyID;
                _oVoucher.BaseCUSymbol = oTempVoucher.CUSymbol;
                _oVoucher.VoucherTypeID = nvtid;
                _oVoucher.VoucherName = oTempVoucher.VoucherName;
                _oVoucher.NumberMethodInInt = oTempVoucher.NumberMethodInInt;
                _oVoucher.Narration = "";
                _oVoucher.PreparedByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
                _oVoucher.DBServerDate = DateTime.Now;
                _oVoucher.VDObjs = new List<VDObj>();

                string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE (TT.CreateBy =" + ((int)Session[SessionInfo.currentUserID]).ToString() + " AND  TT.BatchStatus=1) ORDER BY VoucherBatchID ASC";
                List<VoucherBatch> oVoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oVoucherBatchs.Count <= 0)
                {
                    VoucherBatch oVB = new VoucherBatch();
                    oVB.BatchStatus = EnumVoucherBatchStatus.BatchOpen;
                    oVB = oVB.Save((int)Session[SessionInfo.currentUserID]);
                    oVoucherBatchs.Add(oVB);
                }
                ViewBag.VoucherBatchs = oVoucherBatchs;
                if (oVoucherBatchs != null)
                {
                    if (oVoucherBatchs.Count > 0)
                    {
                        _oVoucher.BatchID = oVoucherBatchs[oVoucherBatchs.Count - 1].VoucherBatchID;
                    }
                }

                if (copyid > 0)
                {
                    oVoucher = new Voucher();
                    List<VDObj> oVDObjs = new List<VDObj>();
                    List<VDObj> oTempVDObjs = new List<VDObj>();
                    _oVoucher.VoucherID = copyid;
                    oVoucher = oVoucher.Get(copyid, (int)Session[SessionInfo.currentUserID]);
                    oTempVDObjs = MapVoucherExplanationObject(_oVoucher, false);
                    foreach (VDObj oItem in oTempVDObjs)
                    {
                        if (oItem.ObjType == EnumBreakdownType.VoucherDetail || oItem.ObjType == EnumBreakdownType.CostCenter || oItem.ObjType == EnumBreakdownType.InventoryReference)
                        {
                            oItem.VDObjID = 0;
                            oVDObjs.Add(oItem);
                        }
                    }
                    _oVoucher.VDObjs = oVDObjs;
                    _oVoucher.Narration = oVoucher.Narration;
                    _oVoucher.VoucherID = 0;
                }
            }
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            ViewBag.TaxEffects = EnumObject.jGets(typeof(EnumTaxEffect));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.VoucherTypes = VoucherType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.RunningAccountingYear = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
            ViewBag.ChequeTypes = EnumObject.jGets(typeof(EnumChequeType));
            return View(_oVoucher);
        }
        public ActionResult VoucherAdvanceSearch()
        {
            Voucher oVoucher = new Voucher();
            string sSql = "SELECT * FROM View_User WHERE UserID IN (SELECT ISNULL(AuthorizedBy,0) FROM Voucher)";
            oVoucher.UserList = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oVoucher);
        }
        public ActionResult ViewSingleVoucherDetails(int id, double ts)
        {
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            _oVoucher = new Voucher();
            Company oCompany = new Company();
            _oVoucher.LstCurrency = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oVoucher.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oVoucher = _oVoucher.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oVoucher.VDObjs = MapVoucherExplanationObject(_oVoucher, false);
            return PartialView(_oVoucher);
        }
        #endregion
                
        #region Post Method
        [HttpPost]
        public JsonResult GetVoucherType()
        {

            List<VoucherType> oVoucherTypes = new List<VoucherType>();
            oVoucherTypes = VoucherType.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucherTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWaitForApproval(double ts)
        {
            List<Voucher> oVouchers = new List<Voucher>();

            oVouchers = Voucher.GetsWaitForApproval((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVouchers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWaitForInventoryEffect(Voucher oVoucher)
        {
            List<Voucher> oVouchers = new List<Voucher>();
            string sSQL = "SELECT * FROM View_Voucher AS HH WHERE ISNULL(HH.CounterVoucherID,0)=0 AND HH.VoucherID IN(SELECT MM.VoucherID FROM dbo.GetInventoryEffectedVoucher() AS MM) ORDER BY VoucherID";
            oVouchers = Voucher.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVouchers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            Voucher oVoucher = new Voucher();
            string sFeedBackMessage = oVoucher.Delete(id, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveTest(TVoucher oTVoucher)
        {            
            string sFeedBackMessage = "";
            Voucher oVoucher = new Voucher();
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            oVoucher = VoucherMap(oTVoucher);
            oVoucherDetails = MapVoucherDetailObject(oVoucher, oTVoucher.VDObjs);
            oVoucher.VoucherDetailLst = oVoucherDetails;

            try
            {
                #region Validate Data
                sFeedBackMessage = "";
                sFeedBackMessage = FeedBackForDataValidation(oVoucher.VoucherDetailLst);
                if (sFeedBackMessage != "")
                {
                    _oVoucher = new Voucher();
                    _oVoucher.ErrorMessage = sFeedBackMessage;

                    JavaScriptSerializer bserializer = new JavaScriptSerializer();
                    string bjson = bserializer.Serialize(_oVoucher);
                    return Json(bjson, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region Debit Credit Equal
                sFeedBackMessage = "";
                sFeedBackMessage = FeedBackForDebitCreditEqual(oVoucher.VoucherDetailLst);
                if (sFeedBackMessage != "")
                {
                    _oVoucher = new Voucher();
                    _oVoucher.ErrorMessage = sFeedBackMessage;

                    JavaScriptSerializer bserializer = new JavaScriptSerializer();
                    string bjson = bserializer.Serialize(_oVoucher);
                    return Json(bjson, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region BU Permission
                if (!BusinessUnit.IsPermittedBU(oVoucher.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    _oVoucher = new Voucher();
                    _oVoucher.ErrorMessage = "Your are not permitted for selected Business Unit!";

                    JavaScriptSerializer bserializer = new JavaScriptSerializer();
                    string bjson = bserializer.Serialize(_oVoucher);
                    return Json(bjson, JsonRequestBehavior.AllowGet);
                }
                #endregion

                oVoucher = oVoucher.Save((int)Session[SessionInfo.currentUserID]);
                VoucherType oVoucherType = new VoucherType();
                oVoucherType = oVoucherType.Get(oVoucher.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);
                oVoucher.IsPrint = oVoucherType.PrintAfterSave; //IsProductRequired use as a printing page size determine A4 or Half of A4
            }
            catch(Exception ex)
            {
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSingleVoucherDetails(Voucher oVoucher)
        {
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            _oVoucher = new Voucher();
            Company oCompany = new Company();
            _oVoucher = _oVoucher.Get(oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            _oVoucher.LstCurrency = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oVoucher.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            
            _oVoucher.VDObjs = MapVoucherExplanationObject(_oVoucher, false);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approved(Voucher oVoucher)
        {
            string IDs = "";
            List<Voucher> oVouchers = new List<Voucher>();
            oVouchers = oVoucher.VoucherList;
            if (oVouchers != null)
            {
                foreach (Voucher oItem in oVouchers)
                {
                    IDs = oItem.VoucherID + "," + IDs;
                }
                IDs = IDs.Remove((IDs.Length - 1), 1);
                oVoucher.Narration = IDs;
            }
            _oVouchers = oVoucher.ApprovedVouchers((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVouchers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitInventoryEffect(Voucher oVoucher)
        {        
            List<Voucher> oVouchers = new List<Voucher>();
            oVouchers = oVoucher.VoucherList;
            if (oVouchers != null)
            {
                _oVouchers = oVoucher.CommitInventoryEffect(oVouchers, (int)Session[SessionInfo.currentUserID]);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVouchers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVoucherInfo(Voucher oVoucher)
        {
            Voucher oTempVoucher = new Voucher();
            oTempVoucher = oTempVoucher.GetMaxDate(oVoucher.VoucherTypeID, oVoucher.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oTempVoucher.VoucherDate == null || oTempVoucher.VoucherDate == DateTime.MinValue)
            {
                oTempVoucher.VoucherDate = DateTime.Today;
            }
            _oVoucher = new Voucher();
            _oVoucher = oVoucher.CommitVoucherNo(oVoucher.BUID, oVoucher.VoucherTypeID, oTempVoucher.VoucherDate, (int)Session[SessionInfo.currentUserID]); // Here 2 refere VoucherTypeID that is PaymentVoucher & 1 refere jam company ID                
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVoucherDetail(Voucher oVoucher)
        {
            List<VDObj> oVDObjs = new List<VDObj>();
            oVDObjs = MapVoucherExplanationObject(oVoucher, false);
            foreach (VDObj oItem in oVDObjs)
            {
                oItem.VDObjID = 0;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVDObjs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UnApprovedVoucher(Voucher oVoucher)
        {
            try
            {
                oVoucher = oVoucher.UnApprovedVoucher(oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }     
        #endregion

        #region Supporting Functions
        public Voucher VoucherMap(TVoucher oTVoucher)
        {
            Voucher oVoucher = new Voucher();
            oVoucher.VoucherID = oTVoucher.VoucherID;
            oVoucher.BUID = oTVoucher.BUID;
            oVoucher.VoucherTypeID = oTVoucher.VoucherTypeID;
            oVoucher.VoucherNo = oTVoucher.VoucherNo;
            oVoucher.Narration = oTVoucher.Narration == null ? "" : oTVoucher.Narration;
            oVoucher.ReferenceNote = oTVoucher.ReferenceNote;
            oVoucher.VoucherDate = oTVoucher.VoucherDate;
            oVoucher.AuthorizedBy = oTVoucher.AuthorizedBy;
            oVoucher.BatchID = oTVoucher.BatchID;
            oVoucher.CurrencyID = oTVoucher.CurrencyID;
            oVoucher.CRate = oTVoucher.CRate;
            oVoucher.TaxEffect = (EnumTaxEffect)oTVoucher.TaxEffectInt;
            oVoucher.TaxEffectInt = oTVoucher.TaxEffectInt;
            return oVoucher;
        }
        public List<VoucherDetail> MapVoucherDetailObject(Voucher oVoucher, List<VDObj> oVDObjs)
        {
            int nIndex = -1;
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            if (oVDObjs != null)
            {
                ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
                oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.VoucherFormat, (int)Session[SessionInfo.currentUserID]);

                for (int i = 0; i < oVDObjs.Count; i++)
                {
                    if (oVDObjs[i].ObjTypeInt == (int)EnumBreakdownType.VoucherDetail)
                    {
                        #region Voucher Detail
                        nIndex++;
                        VoucherDetail oVoucherDetail = new VoucherDetail();
                        oVoucherDetail.VoucherDetailID = oVDObjs[i].VDObjID;
                        oVoucherDetail.VoucherID = 0;
                        oVoucherDetail.BUID = oVDObjs[i].BUID;
                        oVoucherDetail.AreaID = oVDObjs[i].AID;
                        oVoucherDetail.ZoneID = oVDObjs[i].ZID;
                        oVoucherDetail.SiteID = oVDObjs[i].SID;
                        oVoucherDetail.ProductID = oVDObjs[i].PID;
                        oVoucherDetail.DeptID = oVDObjs[i].DptID;
                        oVoucherDetail.AccountHeadID = oVDObjs[i].AHID;
                        oVoucherDetail.CostCenterID = oVDObjs[i].CCID;
                        oVoucherDetail.CurrencyID = oVDObjs[i].CID;
                        oVoucherDetail.AmountInCurrency = oVDObjs[i].CAmount;                        
                        if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                        {
                            oVoucherDetail.ConversionRate = oVoucher.CRate;
                            oVoucherDetail.Amount = (oVDObjs[i].CAmount * oVoucher.CRate);
                        }
                        else
                        {
                            oVoucherDetail.ConversionRate = oVDObjs[i].CRate;
                            oVoucherDetail.Amount = oVDObjs[i].Amount; //(oVDObjs[i].CAmount * oVDObjs[i].CRate);
                        }                        
                        oVoucherDetail.IsDebit = oVDObjs[i].IsDr;
                        oVoucherDetail.Narration = (oVDObjs[i].Detail == null) ? "" : oVDObjs[i].Detail;
                        oVoucherDetail.AccountHeadCode = oVDObjs[i].AHCode;
                        oVoucherDetail.AccountHeadName = oVDObjs[i].AHName;
                        oVoucherDetail.AreaCode = oVDObjs[i].ACode;
                        oVoucherDetail.AreaName = oVDObjs[i].AName;
                        oVoucherDetail.AreaShortName = oVDObjs[i].ASName;
                        oVoucherDetail.ZoneCode = oVDObjs[i].ZCode;
                        oVoucherDetail.ZoneName = oVDObjs[i].ZName;
                        oVoucherDetail.ZoneShortName = oVDObjs[i].ZSName;
                        oVoucherDetail.SiteCode = oVDObjs[i].SCode;
                        oVoucherDetail.SiteName = oVDObjs[i].SName;
                        oVoucherDetail.SiteShortName = oVDObjs[i].SSName;
                        oVoucherDetail.PCode = oVDObjs[i].PCode;
                        oVoucherDetail.PName = oVDObjs[i].PName;
                        oVoucherDetail.PShortName = oVDObjs[i].PSName;
                        oVoucherDetail.DeptCode = oVDObjs[i].DCode;
                        oVoucherDetail.DeptName = oVDObjs[i].DName;
                        oVoucherDetail.DeptShortName = oVDObjs[i].DSName;
                        oVoucherDetail.CCCode = oVDObjs[i].CCCode;
                        oVoucherDetail.CCName = oVDObjs[i].CCName;
                        oVoucherDetail.DebitAmount = oVDObjs[i].DrAmount;
                        oVoucherDetail.CreditAmount = oVDObjs[i].CrAmount;
                        oVoucherDetail.BCDebitAmount = oVDObjs[i].BCDrAmount;
                        oVoucherDetail.BCCreditAmount = oVDObjs[i].BCCrAmount;
                        oVoucherDetails.Add(oVoucherDetail);
                        #endregion
                    }
                    else if (oVDObjs[i].ObjTypeInt == (int)EnumBreakdownType.CostCenter)
                    {
                        #region Subledger Reference
                        CostCenterTransaction oCostCenterTransaction = new CostCenterTransaction();
                        oCostCenterTransaction.AccountHeadID = oVoucherDetails[nIndex].AccountHeadID;
                        oCostCenterTransaction.VoucherDetailID = (long)oVoucherDetails[nIndex].VoucherDetailID;
                        oCostCenterTransaction.CCTID = (int)oVDObjs[i].VDObjID;
                        oCostCenterTransaction.CCID = oVDObjs[i].CCID;
                        if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                        {
                            oCostCenterTransaction.Amount = oVDObjs[i].CAmount;
                        }
                        else
                        {
                            oCostCenterTransaction.Amount = oVDObjs[i].Amount;
                        }
                        oCostCenterTransaction.Description = oVDObjs[i].Detail;
                        oCostCenterTransaction.IsDr = oVoucherDetails[nIndex].IsDebit;
                        oCostCenterTransaction.CurrencyID = oVoucherDetails[nIndex].CurrencyID;
                        oCostCenterTransaction.CurrencyConversionRate = oVoucherDetails[nIndex].ConversionRate;
                        
                        bool bIsBTAply = false; bool bIsOrderaAply = false; bool bIsChkAply = false;
                        bIsBTAply = oVDObjs[i].IsBTAply;
                        bIsOrderaAply = oVDObjs[i].IsOrderaAply;
                        bIsChkAply = oVDObjs[i].IsChkAply;

                        if (bIsBTAply)
                        {
                            oCostCenterTransaction.VBTransactions = this.GetSubLedgerBills(ref i, oVDObjs, oCostCenterTransaction.CCID, oVoucherDetails[nIndex], oClientOperationSetting);
                        }
                        else
                        {
                            oCostCenterTransaction.VBTransactions = new List<VoucherBillTransaction>();
                        }
                        if (bIsOrderaAply)
                        {
                            oCostCenterTransaction.VOReferences = this.GetSubLedgerOrders(ref i, oVDObjs, oCostCenterTransaction.CCID, oVoucherDetails[nIndex], oClientOperationSetting);
                        }
                        else
                        {
                            oCostCenterTransaction.VOReferences = new List<VOReference>();
                        }
                        if (bIsChkAply)
                        {
                            oCostCenterTransaction.VoucherCheques = this.GetSubLedgerCheques(ref i, oVDObjs, oCostCenterTransaction.CCID, oVoucherDetails[nIndex], oClientOperationSetting);
                        }
                        else
                        {
                            oCostCenterTransaction.VoucherCheques = new List<VoucherCheque>();
                        }
                        oVoucherDetails[nIndex].CCTs.Add(oCostCenterTransaction);
                        #endregion
                    }
                    else if (oVDObjs[i].ObjTypeInt == (int)EnumBreakdownType.BillReference)
                    {
                        #region Voucher Bill Reference
                        VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
                        oVoucherBillTransaction.VoucherBillTransactionID = (int)oVDObjs[i].VDObjID;
                        oVoucherBillTransaction.VoucherBillID = oVDObjs[i].BillID;
                        oVoucherBillTransaction.VoucherDetailID = (long)oVoucherDetails[nIndex].VoucherDetailID;
                        oVoucherBillTransaction.CCTID = 0;
                        oVoucherBillTransaction.AccountHeadID = oVoucherDetails[nIndex].AccountHeadID;
                        if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                        {
                            oVoucherBillTransaction.Amount = oVDObjs[i].CAmount;
                        }
                        else
                        {
                            oVoucherBillTransaction.Amount = oVDObjs[i].Amount;
                        }                        
                        oVoucherBillTransaction.TransactionDate = oVDObjs[i].BillDate;
                        oVoucherBillTransaction.TrType = (EnumVoucherBillTrType)oVDObjs[i].TrTypeInt;
                        oVoucherBillTransaction.IsDr = oVoucherDetails[nIndex].IsDebit;
                        oVoucherBillTransaction.CurrencyID = oVoucherDetails[nIndex].CurrencyID;
                        oVoucherBillTransaction.ConversionRate = oVoucherDetails[nIndex].ConversionRate;
                        oVoucherBillTransaction.CurrencySymbol = oVDObjs[i].CSymbol;
                        oVoucherDetails[nIndex].VoucherBillTrs.Add(oVoucherBillTransaction);
                        #endregion
                    }
                    else if (oVDObjs[i].ObjTypeInt == (int)EnumBreakdownType.ChequeReference)
                    {
                        #region Voucher Cheque Reference
                        VoucherCheque oVoucherCheque = new VoucherCheque();
                        oVoucherCheque.VoucherChequeID = (int)oVDObjs[i].VDObjID;
                        oVoucherCheque.VoucherDetailID = (int)oVoucherDetails[nIndex].VoucherDetailID;
                        oVoucherCheque.ChequeID = oVDObjs[i].ChequeID;
                        oVoucherCheque.ChequeType = oVDObjs[i].ChequeType;                        
                        if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                        {
                            oVoucherCheque.Amount = oVDObjs[i].CAmount;
                        }
                        else
                        {
                            oVoucherCheque.Amount = oVDObjs[i].Amount;
                        }       
                        //oVoucherCheque.ChequeType = oVDObjs[i].IsPaidChk ? EnumChequeType.Payment : EnumChequeType.Received;
                        oVoucherCheque.TransactionDate = DateTime.Now;
                        oVoucherDetails[nIndex].VoucherCheques.Add(oVoucherCheque);
                        #endregion
                    }
                    else if (oVDObjs[i].ObjTypeInt == (int)EnumBreakdownType.InventoryReference)
                    {
                        #region Inventory Reference
                        VPTransaction oVPTransaction = new VPTransaction();
                        oVPTransaction.VPTransactionID = (int)oVDObjs[i].VDObjID;
                        oVPTransaction.VoucherDetailID = (long)oVoucherDetails[nIndex].VoucherDetailID;
                        oVPTransaction.Qty = oVDObjs[i].Qty;
                        oVPTransaction.UnitPrice = oVDObjs[i].UPrice;
                        oVPTransaction.IsDr = oVoucherDetails[nIndex].IsDebit;
                        oVPTransaction.MUnitID = oVDObjs[i].MUID;
                        oVPTransaction.WorkingUnitID = oVDObjs[i].WUID;
                        oVPTransaction.ProductID = oVDObjs[i].PID;
                        oVPTransaction.CurrencyID = oVoucherDetails[nIndex].CurrencyID;
                        oVPTransaction.ConversionRate = oVoucherDetails[nIndex].ConversionRate;
                        if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                        {
                            oVPTransaction.Amount = oVDObjs[i].CAmount;
                        }
                        else
                        {
                            oVPTransaction.Amount = oVDObjs[i].Amount;
                        }
                        oVoucherDetails[nIndex].VPTransactions.Add(oVPTransaction);
                        #endregion
                    }
                    else if (oVDObjs[i].ObjTypeInt == (int)EnumBreakdownType.OrderReference)
                    {
                        #region Order Reference
                        VOReference oVOReference = new VOReference();
                        oVOReference.VOReferenceID = (int)oVDObjs[i].VDObjID;
                        oVOReference.VoucherDetailID = (int)oVoucherDetails[nIndex].VoucherDetailID;
                        oVOReference.OrderID = (int)oVDObjs[i].OrderID;
                        oVOReference.Remarks = oVDObjs[i].ORemarks;
                        oVOReference.IsDebit = oVoucherDetails[nIndex].IsDebit;
                        oVOReference.CurrencyID = oVoucherDetails[nIndex].CurrencyID;
                        oVOReference.ConversionRate = oVoucherDetails[nIndex].ConversionRate;
                        oVOReference.AmountInCurrency = oVDObjs[i].CAmount;
                        oVOReference.Amount = oVDObjs[i].Amount;
                        oVoucherDetails[nIndex].VOReferences.Add(oVOReference);
                        #endregion
                    }
                }
            }
            return oVoucherDetails;
        }
        private List<VoucherBillTransaction> GetSubLedgerBills(ref int i, List<VDObj> oVDObjs, int nSubLedgerID, VoucherDetail oVoucherDetail, ClientOperationSetting oClientOperationSetting)
        {
            List<VoucherBillTransaction> oVBTransactions = new List<VoucherBillTransaction>();
            for (int n = i + 1; n < oVDObjs.Count; n++)
            {
                if (oVDObjs[n].CCID == nSubLedgerID && oVDObjs[n].ObjTypeInt == (int)EnumBreakdownType.SubledgerBill)
                {
                    VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
                    oVoucherBillTransaction.VoucherBillTransactionID = (int)oVDObjs[n].VDObjID;
                    oVoucherBillTransaction.VoucherBillID = oVDObjs[n].BillID;
                    oVoucherBillTransaction.VoucherDetailID = (long)oVoucherDetail.VoucherDetailID;
                    oVoucherBillTransaction.CCTID = 0;
                    oVoucherBillTransaction.AccountHeadID = oVoucherDetail.AccountHeadID;
                    if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                    {
                        oVoucherBillTransaction.Amount = oVDObjs[n].CAmount;                        
                    }
                    else
                    {
                        oVoucherBillTransaction.Amount = oVDObjs[n].Amount;                        
                    }
                    oVoucherBillTransaction.TransactionDate = oVDObjs[n].BillDate;
                    oVoucherBillTransaction.TrType = (EnumVoucherBillTrType)oVDObjs[n].TrTypeInt;
                    oVoucherBillTransaction.IsDr = oVoucherDetail.IsDebit;
                    oVoucherBillTransaction.CurrencyID = oVoucherDetail.CurrencyID;
                    oVoucherBillTransaction.ConversionRate = oVoucherDetail.ConversionRate;
                    oVoucherBillTransaction.CurrencySymbol = oVDObjs[n].CSymbol;
                    oVBTransactions.Add(oVoucherBillTransaction);
                    i++;
                }
                else
                {
                    break;
                }
            }
            return oVBTransactions;
        }
        private List<VOReference> GetSubLedgerOrders(ref int i, List<VDObj> oVDObjs, int nSubLedgerID, VoucherDetail oVoucherDetail, ClientOperationSetting oClientOperationSetting)
        {
            List<VOReference> oVOReferences = new List<VOReference>();
            for (int n = i + 1; n < oVDObjs.Count; n++)
            {
                if (oVDObjs[n].CCID == nSubLedgerID && oVDObjs[n].ObjTypeInt == (int)EnumBreakdownType.SLOrderReference)
                {                    
                    VOReference oVOReference = new VOReference();
                    oVOReference.VOReferenceID = (int)oVDObjs[n].VDObjID;
                    oVOReference.VoucherDetailID = (int)oVoucherDetail.VoucherDetailID;
                    oVOReference.OrderID = (int)oVDObjs[n].OrderID;
                    oVOReference.Remarks = oVDObjs[n].ORemarks;
                    oVOReference.IsDebit = oVoucherDetail.IsDebit;
                    oVOReference.CurrencyID = oVoucherDetail.CurrencyID;
                    oVOReference.ConversionRate = oVoucherDetail.ConversionRate;
                    oVOReference.AmountInCurrency = oVDObjs[n].CAmount;
                    oVOReference.Amount = oVDObjs[n].Amount;
                    oVOReferences.Add(oVOReference);
                    i++;
                }
                else
                {
                    break;
                }
            }
            return oVOReferences;
        }
        private List<VoucherCheque> GetSubLedgerCheques(ref int CurrentIndex, List<VDObj> oVDObjs, int nSubLedgerID, VoucherDetail oVoucherDetail, ClientOperationSetting oClientOperationSetting)
        {
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
            for (int n = CurrentIndex + 1; n < oVDObjs.Count; n++)
            {
                if (oVDObjs[n].CCID == nSubLedgerID && oVDObjs[n].ObjTypeInt == (int)EnumBreakdownType.SubledgerCheque)
                {
                    VoucherCheque oVoucherCheque = new VoucherCheque();
                    oVoucherCheque.VoucherChequeID = (int)oVDObjs[n].VDObjID;
                    oVoucherCheque.VoucherDetailID = (int)oVoucherDetail.VoucherDetailID;
                    oVoucherCheque.CCTID = 0;
                    oVoucherCheque.ChequeID = oVDObjs[n].ChequeID;
                    oVoucherCheque.ChequeType = oVDObjs[n].ChequeType;                    
                    if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                    {
                        oVoucherCheque.Amount = oVDObjs[n].CAmount;
                    }
                    else
                    {
                        oVoucherCheque.Amount = oVDObjs[n].Amount;
                    }                    
                    oVoucherCheque.TransactionDate = DateTime.Now;
                    oVoucherCheques.Add(oVoucherCheque);
                    CurrentIndex++;
                }
                else
                {
                    break;
                }
            }
            return oVoucherCheques;
        }
        public List<VDObj> MapVoucherExplanationObject(Voucher oVoucher, bool bIsPrint)
        {
            VDObj oVDObj = new VDObj();
            List<VDObj> oVDObjs = new List<VDObj>();
            List<VoucherDetail> oTempVoucherDetails = new List<VoucherDetail>();
            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<CostCenterTransaction> oTempCostCenterTransactions = new List<CostCenterTransaction>();
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherBillTransaction> oTempVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
            List<VoucherCheque> oTempVoucherCheques = new List<VoucherCheque>();
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            List<VPTransaction> oTempVPTransactions = new List<VPTransaction>();
            List<VOReference> oVOReferences = new List<VOReference>();
            List<VOReference> oTempVOReferences = new List<VOReference>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();

            oTempVoucherDetails = VoucherDetail.Gets(oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            oCostCenterTransactions = CostCenterTransaction.GetsBy((int)oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            oVoucherBillTransactions = VoucherBillTransaction.GetsBy((int)oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            oVoucherCheques = VoucherCheque.GetsBy((int)oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            oVPTransactions = VPTransaction.GetsBy((int)oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            oVOReferences = VOReference.GetsBy((int)oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.VoucherFormat, (int)Session[SessionInfo.currentUserID]);


            foreach (VoucherDetail oItem in oTempVoucherDetails)
            {
                oTempCostCenterTransactions = new List<CostCenterTransaction>();
                oTempVoucherBillTransactions = new List<VoucherBillTransaction>();
                oTempVoucherCheques = new List<VoucherCheque>();
                oTempVPTransactions = new List<VPTransaction>();
                oTempCostCenterTransactions = MapCostCenterTransactions(oCostCenterTransactions, (int)oItem.VoucherDetailID);
                oTempVoucherBillTransactions = MapVoucherBillTransaction(oVoucherBillTransactions, (int)oItem.VoucherDetailID, 0);
                oTempVoucherCheques = MapVoucherCheques(oVoucherCheques, (int)oItem.VoucherDetailID, 0);
                oTempVPTransactions = MapVPTransactions(oVPTransactions, (int)oItem.VoucherDetailID);
                oTempVOReferences = MapVOReferences(oVOReferences, (int)oItem.VoucherDetailID, 0);

                #region Voucher Details
                oVDObj = new VDObj();
                oVDObj.VDObjID = oItem.VoucherDetailID;
                oVDObj.BUID = oItem.BUID;
                oVDObj.AID = oItem.AreaID;
                oVDObj.ZID = oItem.ZoneID;
                oVDObj.SID = oItem.SiteID;
                oVDObj.PID = oItem.ProductID;
                oVDObj.DptID = oItem.DeptID;
                oVDObj.AHID = oItem.AccountHeadID;
                oVDObj.CCID = oItem.CostCenterID;
                oVDObj.CID = oItem.CurrencyID;
                oVDObj.CSymbol = oItem.CUSymbol;
                oVDObj.CAmount = oItem.AmountInCurrency;
                oVDObj.CRate = oItem.ConversionRate;                
                oVDObj.IsDr = oItem.IsDebit;
                if (oItem.IsDebit) 
                { 
                    oVDObj.DR_CR = "Debit"; 
                } 
                else 
                { 
                    oVDObj.DR_CR = "Credit"; 
                }
                oVDObj.Detail = oItem.Narration;
                if (bIsPrint) 
                { 
                    oVDObj.AHCode = oItem.AccountHeadCode; 
                } 
                else 
                {
                    if (oItem.LedgerBalance == "--")
                    {
                        oVDObj.AHCode = oItem.AccountHeadCode;
                    }
                    else
                    {
                        oVDObj.AHCode = oItem.LedgerBalance + "[" + oItem.AccountHeadCode + "]";
                    }
                }
                oVDObj.AHName = oItem.AccountHeadName;
                oVDObj.ACode = oItem.AreaCode;
                oVDObj.AName = oItem.AreaName;
                oVDObj.ASName = oItem.AreaShortName;
                oVDObj.ZCode = oItem.ZoneCode;
                oVDObj.ZName = oItem.ZoneName;
                oVDObj.ZSName = oItem.ZoneShortName;
                oVDObj.SCode = oItem.SiteCode;
                oVDObj.SName = oItem.SiteName;
                oVDObj.SSName = oItem.SiteShortName;
                oVDObj.PCode = oItem.PCode;
                oVDObj.PName = oItem.PName;
                oVDObj.PSName = oItem.PShortName;
                oVDObj.DCode = oItem.DeptCode;
                oVDObj.DName = oItem.DeptName;
                oVDObj.DSName = oItem.DeptShortName;
                oVDObj.CCCode = oItem.CCCode;
                oVDObj.CCName = oItem.CCName;
                if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
                {
                    oVDObj.Amount = oItem.Amount;
                    oVDObj.DrAmount = oItem.DebitAmount;
                    oVDObj.CrAmount = oItem.CreditAmount;
                    oVDObj.BCDrAmount = oItem.BCDebitAmount;
                    oVDObj.BCCrAmount = oItem.BCCreditAmount;
                }
                else
                {
                    oVDObj.Amount = oItem.Amount;// (oItem.AmountInCurrency * oItem.ConversionRate);
                    oVDObj.DrAmount = oItem.BCDebitAmount;
                    oVDObj.CrAmount = oItem.BCCreditAmount;
                    oVDObj.BCDrAmount = oItem.BCDebitAmount;
                    oVDObj.BCCrAmount = oItem.BCCreditAmount;
                    if (oVDObj.CID != oVoucher.BaseCurrencyID)
                    {
                        oVDObj.CFormat = oItem.CUSymbol + " " + Global.MillionFormat(oItem.AmountInCurrency) + " @ " + Global.MillionFormatActualDigit(oItem.ConversionRate);
                    }
                    else
                    {
                        oVDObj.CFormat = oVoucher.BaseCUSymbol + " " + oItem.Amount;
                    }
                }
                oVDObj.IsAEfct = oItem.IsAreaEffect;
                oVDObj.IsZEfct = oItem.IsZoneEffect;
                oVDObj.IsSEfct = oItem.IsSiteEffect;
                oVDObj.IsCCAply = oItem.IsCostCenterApply;
                oVDObj.IsBTAply = oItem.IsBillRefApply;
                oVDObj.IsChkAply = oItem.IsChequeApply;
                oVDObj.IsIRAply = oItem.IsInventoryApply;
                oVDObj.IsOrderaAply = oItem.IsOrderReferenceApply;
                oVDObj.IsPaidChk = oItem.IsPaymentCheque;
                oVDObjs.Add(oVDObj);
                #endregion

                #region Account Head Conversion Rate Display
                if (bIsPrint)
                {
                    if (oVoucher.BaseCurrencyID != oItem.CurrencyID)
                    {
                        oVDObj = new VDObj();
                        oVDObj.VDObjID = 0;
                        oVDObj.ObjType = EnumBreakdownType.VoucherDetail;
                        oVDObj.ObjTypeInt = (int)EnumBreakdownType.VoucherDetail;
                        oVDObj.CFormat = oItem.CUSymbol + Global.MillionFormat(oItem.AmountInCurrency) + " @ " + Global.MillionFormatActualDigit(oItem.ConversionRate) + " " + oVoucher.BaseCUSymbol + " " + Global.MillionFormat(oItem.Amount);
                        oVDObj.CName = "";
                        oVDObj.CSymbol = "";
                        oVDObj.Detail = "";
                        oVDObj.IsDr = false;
                        oVDObj.DR_CR = "Debit";
                        oVDObj.DrAmount = 0.00;
                        oVDObj.CrAmount = 0.00;
                        oVDObj.WUID = 0;
                        oVDObj.WUName = "";
                        oVDObj.MUID = 0;
                        oVDObj.MUName = "";
                        oVDObj.Qty = 0;
                        oVDObj.UPrice = 0;
                        oVDObjs.Add(oVDObj);
                    }
                }
                #endregion

                #region CostCenterTransaction
                int nCCCategoryID = 0;
                foreach (CostCenterTransaction oCostCenterTransaction in oTempCostCenterTransactions)
                {
                    oVDObj = new VDObj();
                    oVDObj.AHID = oItem.AccountHeadID;
                    oVDObj.AHCode = oItem.AccountHeadCode;
                    oVDObj.AHName = oItem.AccountHeadName;
                    oVDObj.VDObjID = oCostCenterTransaction.CCTID;
                    oVDObj.ObjType = EnumBreakdownType.CostCenter;
                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.CostCenter;
                    oVDObj.CCID = oCostCenterTransaction.CCID;
                    oVDObj.CCName = oCostCenterTransaction.CostCenterName;
                    oVDObj.CCCode = oCostCenterTransaction.CostCenterCode;
                    oVDObj.CID = oCostCenterTransaction.CurrencyID;
                    oVDObj.IsBTAply = oCostCenterTransaction.IsBillRefApply;
                    oVDObj.IsOrderaAply = oCostCenterTransaction.IsOrderRefApply;
                    oVDObj.IsChkAply = oCostCenterTransaction.IsChequeApply;
                    oVDObj.CAmount = oCostCenterTransaction.Amount;
                    oVDObj.CRate = oCostCenterTransaction.CurrencyConversionRate;
                    oVDObj.Amount = oCostCenterTransaction.Amount;
                    oVDObj.AmountBC = oCostCenterTransaction.AmountBC;
                    oVDObj.CFormat = "Subledger : " + oCostCenterTransaction.CostCenterName;
                    oVDObj.CName = oItem.CUName;
                    oVDObj.CSymbol = oItem.CUSymbol;
                    oVDObj.Detail = oCostCenterTransaction.Description;
                    oVDObj.IsDr = oCostCenterTransaction.IsDr;
                    oVDObj.DR_CR = "Subledger";
                    oVDObj.DrAmount = 0.00;
                    oVDObj.CrAmount = 0.00;
                    oVDObjs.Add(oVDObj);
                    nCCCategoryID = oCostCenterTransaction.CCCategoryID;

                    #region Map Subledger Bill
                    if (oCostCenterTransaction.IsBillRefApply)
                    {
                        List<VoucherBillTransaction> oVBTransactions = new List<VoucherBillTransaction>();
                        oVBTransactions = this.MapVoucherBillTransaction(oVoucherBillTransactions, (int)oItem.VoucherDetailID, oCostCenterTransaction.CCTID);
                        if (oVBTransactions.Count > 0)
                        {
                            foreach (VoucherBillTransaction oVoucherBillTransaction in oVBTransactions)
                            {
                                oVDObj = new VDObj();
                                oVDObj.AHID = oItem.AccountHeadID;
                                oVDObj.AHCode = oItem.AccountHeadCode;
                                oVDObj.AHName = oItem.AccountHeadName;
                                oVDObj.VDObjID = oVoucherBillTransaction.VoucherBillTransactionID;
                                oVDObj.ObjType = EnumBreakdownType.SubledgerBill;
                                oVDObj.ObjTypeInt = (int)EnumBreakdownType.SubledgerBill;
                                oVDObj.BillID = oVoucherBillTransaction.VoucherBillID;
                                oVDObj.TrType = oVoucherBillTransaction.TrType;
                                oVDObj.TrType = oVoucherBillTransaction.TrType;
                                oVDObj.TrTypeInt = (int)oVoucherBillTransaction.TrType;
                                oVDObj.TrTypeStr = oVoucherBillTransaction.TrType.ToString();
                                oVDObj.BillNo = oVoucherBillTransaction.BillNo;
                                oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                                oVDObj.BillAmount = oVoucherBillTransaction.BillAmount;
                                oVDObj.CID = oVoucherBillTransaction.CurrencyID;
                                oVDObj.CAmount = oVoucherBillTransaction.Amount;
                                oVDObj.CRate = oVoucherBillTransaction.ConversionRate;
                                oVDObj.Amount = oVoucherBillTransaction.Amount;
                                oVDObj.CFormat = "SL Bill : " + oVoucherBillTransaction.ExplanationTransactionTypeInString + "  " + oVoucherBillTransaction.BillNo + " @ " + oVoucherBillTransaction.BillDate.ToString("dd MMM yyyy");
                                oVDObj.CName = oItem.CUName;
                                oVDObj.CSymbol = oItem.CUSymbol;
                                oVDObj.Detail = "";
                                oVDObj.IsDr = oVoucherBillTransaction.IsDr;
                                oVDObj.DR_CR = "SL Bill";
                                oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                                oVDObj.CCID = oVoucherBillTransaction.CCID;
                                oVDObj.CCName = oVoucherBillTransaction.CostCenterName;
                                oVDObj.CCCode = oVoucherBillTransaction.CostCenterCode;
                                oVDObjs.Add(oVDObj);
                            }
                        }
                    }
                    #endregion

                    #region Map Subledger Order Ref
                    if (oCostCenterTransaction.IsOrderRefApply)
                    {
                        List<VOReference> oSLVOReferences = new List<VOReference>();
                        oSLVOReferences = this.MapVOReferences(oVOReferences, (int)oItem.VoucherDetailID, oCostCenterTransaction.CCTID);
                        if (oSLVOReferences.Count > 0)
                        {
                            foreach (VOReference oVOReference in oSLVOReferences)
                            {
                                oVDObj = new VDObj();
                                oVDObj.AHID = oItem.AccountHeadID;
                                oVDObj.AHCode = oItem.AccountHeadCode;
                                oVDObj.AHName = oItem.AccountHeadName;
                                oVDObj.VDObjID = oVOReference.VOReferenceID;
                                oVDObj.ObjType = EnumBreakdownType.SLOrderReference;
                                oVDObj.ObjTypeInt = (int)EnumBreakdownType.SLOrderReference;
                                oVDObj.OrderID = oVOReference.OrderID;
                                oVDObj.RefNo = oVOReference.RefNo;
                                oVDObj.OrderNo = oVOReference.OrderNo;
                                oVDObj.ORemarks = oVOReference.Remarks;
                                oVDObj.CID = oVOReference.CurrencyID;
                                oVDObj.CAmount = oVOReference.AmountInCurrency;
                                oVDObj.CRate = oVOReference.ConversionRate;
                                oVDObj.Amount = oVOReference.Amount;
                                oVDObj.CFormat = "SLOrder Ref : " + (oVOReference.RefNo != "" ? (oVOReference.RefNo + ", " + oVOReference.OrderNo) : "N/A") + "," + oVOReference.Remarks + ", " + " @ " + Global.MillionFormat(oItem.AmountInCurrency) + " " + oItem.CUSymbol;
                                oVDObj.CName = oItem.CUName;
                                oVDObj.CSymbol = oItem.CUSymbol;
                                oVDObj.IsDr = oVOReference.IsDebit;
                                oVDObj.DR_CR = "SLOrder Ref";
                                oVDObj.CCID = oVOReference.SubledgerID;
                                oVDObj.CCName = oVOReference.SubledgerName;
                                oVDObj.CCCode = "";
                                oVDObjs.Add(oVDObj);
                            }
                        }
                    }
                    #endregion

                    #region Map Subledger Cheque
                    if (oCostCenterTransaction.IsChequeApply)
                    {
                        List<VoucherCheque> oVCheques = new List<VoucherCheque>();
                        oVCheques = this.MapVoucherCheques(oVoucherCheques, (int)oItem.VoucherDetailID, oCostCenterTransaction.CCTID);
                        if (oVCheques.Count > 0)
                        {
                            foreach (VoucherCheque oVCheque in oVCheques)
                            {
                                oVDObj = new VDObj();
                                oVDObj.AHID = oItem.AccountHeadID;
                                oVDObj.AHCode = oItem.AccountHeadCode;
                                oVDObj.AHName = oItem.AccountHeadName;
                                oVDObj.VDObjID = oVCheque.VoucherChequeID;
                                oVDObj.ObjType = EnumBreakdownType.SubledgerCheque;
                                oVDObj.ObjTypeInt = (int)EnumBreakdownType.SubledgerCheque;
                                oVDObj.CAmount = oVCheque.Amount;
                                oVDObj.Amount = oVCheque.Amount;
                                oVDObj.CFormat = "SL Cheque : " + oVCheque.ChequeNo + " @ " + oVCheque.ChequeDate.ToString("dd MMM yyyy") + " On " + oVCheque.BankName;
                                oVDObj.Detail = "";
                                oVDObj.IsDr = oItem.IsDebit;
                                if (bIsPrint)
                                {
                                    if (oVCheque.ChequeType == EnumChequeType.Cash)
                                    {
                                        oVDObj.DR_CR = "SL Cash";
                                    }
                                    else if (oVCheque.ChequeType == EnumChequeType.Transfer)
                                    {
                                        oVDObj.DR_CR = "SL Transfer";
                                    }
                                    else
                                    {
                                        oVDObj.DR_CR = "SL Cheque-" + oVCheque.ChequeNo;
                                    }
                                }
                                else
                                {
                                    oVDObj.DR_CR = "SL Cheque";
                                }
                                oVDObj.ChequeDate = oVCheque.TransactionDate;
                                oVDObj.ChequeID = oVCheque.ChequeID;
                                oVDObj.ChequeType = oVCheque.ChequeType;
                                oVDObj.ChequeNo = oVCheque.ChequeNo;
                                oVDObj.BankName = oVCheque.BankName;
                                oVDObj.BranchName = oVCheque.BranchName;
                                oVDObj.AccountNo = oVCheque.AccountNo;
                                oVDObj.CCID = oVCheque.CCID;
                                oVDObj.CCName = oVCheque.CostCenterName;
                                oVDObj.CCCode = oVCheque.CostCenterCode;
                                oVDObjs.Add(oVDObj);
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region VoucherBillTransaction
                foreach (VoucherBillTransaction oVoucherBillTransaction in oTempVoucherBillTransactions)
                {
                    oVDObj = new VDObj();
                    oVDObj.AHID = oItem.AccountHeadID;
                    oVDObj.AHCode = oItem.AccountHeadCode;
                    oVDObj.AHName = oItem.AccountHeadName;
                    oVDObj.VDObjID = oVoucherBillTransaction.VoucherBillTransactionID;
                    oVDObj.ObjType = EnumBreakdownType.BillReference;
                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.BillReference;
                    oVDObj.BillID = oVoucherBillTransaction.VoucherBillID;
                    oVDObj.TrType = oVoucherBillTransaction.TrType;
                    oVDObj.TrType = oVoucherBillTransaction.TrType;
                    oVDObj.TrTypeInt = (int)oVoucherBillTransaction.TrType;
                    oVDObj.TrTypeStr = oVoucherBillTransaction.TrType.ToString();
                    oVDObj.BillNo = oVoucherBillTransaction.BillNo;
                    oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                    oVDObj.BillAmount = oVoucherBillTransaction.BillAmount;
                    oVDObj.CID = oVoucherBillTransaction.CurrencyID;
                    oVDObj.CAmount = oVoucherBillTransaction.Amount;
                    oVDObj.CRate = oVoucherBillTransaction.ConversionRate;
                    oVDObj.Amount = oVoucherBillTransaction.Amount;
                    oVDObj.CFormat = "Bill : " + oVoucherBillTransaction.ExplanationTransactionTypeInString + "  " + oVoucherBillTransaction.BillNo + " @ " + oVoucherBillTransaction.BillDate.ToString("dd MMM yyyy");
                    oVDObj.CName = oItem.CUName;
                    oVDObj.CSymbol = oItem.CUSymbol;
                    oVDObj.Detail = "";
                    oVDObj.IsDr = oVoucherBillTransaction.IsDr;
                    oVDObj.DR_CR = "Bill";
                    oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                    oVDObj.CCID = oVoucherBillTransaction.CCID;
                    oVDObj.CCName = oVoucherBillTransaction.CostCenterName;
                    oVDObj.CCCode = oVoucherBillTransaction.CostCenterCode;
                    oVDObjs.Add(oVDObj);
                }
                #endregion

                #region VoucherCheque
                foreach (VoucherCheque oVoucherCheque in oTempVoucherCheques)
                {
                    oVDObj = new VDObj();
                    oVDObj.AHID = oItem.AccountHeadID;
                    oVDObj.AHCode = oItem.AccountHeadCode;
                    oVDObj.AHName = oItem.AccountHeadName;
                    oVDObj.VDObjID = oVoucherCheque.VoucherChequeID;
                    oVDObj.ObjType = EnumBreakdownType.ChequeReference;
                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.ChequeReference;
                    oVDObj.CAmount = oVoucherCheque.Amount;
                    oVDObj.Amount = oVoucherCheque.Amount;
                    oVDObj.CFormat = "Cheque : " + oVoucherCheque.ChequeNo + " @ " + oVoucherCheque.ChequeDate.ToString("dd MMM yyyy") + " On " + oVoucherCheque.BankName;
                    oVDObj.Detail = "";
                    oVDObj.IsDr = oItem.IsDebit;
                    if (bIsPrint)
                    {
                        if (oVoucherCheque.ChequeType == EnumChequeType.Cash)
                        {
                            oVDObj.DR_CR = "Cash";
                        }
                        else if (oVoucherCheque.ChequeType == EnumChequeType.Transfer)
                        {
                            oVDObj.DR_CR = "Transfer";
                        }
                        else
                        {
                            oVDObj.DR_CR = "Cheque-" + oVoucherCheque.ChequeNo;
                        }
                    }
                    else
                    {
                        oVDObj.DR_CR = "Cheque";
                    }
                    oVDObj.ChequeDate = oVoucherCheque.TransactionDate;
                    oVDObj.ChequeID = oVoucherCheque.ChequeID;
                    oVDObj.ChequeType = oVoucherCheque.ChequeType;
                    oVDObj.ChequeNo = oVoucherCheque.ChequeNo;
                    oVDObj.BankName = oVoucherCheque.BankName;
                    oVDObj.BranchName = oVoucherCheque.BranchName;
                    oVDObj.AccountNo = oVoucherCheque.AccountNo;
                    oVDObjs.Add(oVDObj);
                }
                #endregion

                #region VPTransaction
                foreach (VPTransaction oVPTransaction in oTempVPTransactions)
                {
                    oVDObj = new VDObj();
                    oVDObj.AHID = oItem.AccountHeadID;
                    oVDObj.AHCode = oItem.AccountHeadCode;
                    oVDObj.AHName = oItem.AccountHeadName;
                    oVDObj.VDObjID = oVPTransaction.VPTransactionID;
                    oVDObj.ObjType = EnumBreakdownType.InventoryReference;
                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.InventoryReference;
                    oVDObj.PID = oVPTransaction.ProductID;
                    oVDObj.PName = oVPTransaction.ProductName;
                    oVDObj.PCode = oVPTransaction.ProductCode;
                    oVDObj.CID = oVPTransaction.CurrencyID;
                    oVDObj.CAmount = oVPTransaction.Amount;
                    oVDObj.CRate = oVPTransaction.ConversionRate;
                    oVDObj.Amount = oVPTransaction.Amount;
                    oVDObj.CFormat = "Inventory : " + oVPTransaction.ProductName + ", " + oVPTransaction.Description + "," + oVPTransaction.WorkingUnitName + ", " + oVPTransaction.QtyInString + ", " + oVPTransaction.MUnitName + " @ " + oItem.CUSymbol + " " + oVPTransaction.UnitPriceInString;
                    oVDObj.CName = oItem.CUName;
                    oVDObj.CSymbol = oItem.CUSymbol;
                    oVDObj.IsDr = oVPTransaction.IsDr;
                    oVDObj.DR_CR = "Inventory";
                    oVDObj.WUID = oVPTransaction.WorkingUnitID;
                    oVDObj.WUName = oVPTransaction.WorkingUnitName;
                    oVDObj.MUID = oVPTransaction.MUnitID;
                    oVDObj.MUName = oVPTransaction.MUnitName;
                    oVDObj.Qty = oVPTransaction.Qty;
                    oVDObj.UPrice = oVPTransaction.UnitPrice;
                    oVDObjs.Add(oVDObj);
                }
                #endregion

                #region VOReference
                foreach (VOReference oVOReference in oTempVOReferences)
                {
                    oVDObj = new VDObj();
                    oVDObj.AHID = oItem.AccountHeadID;
                    oVDObj.AHCode = oItem.AccountHeadCode;
                    oVDObj.AHName = oItem.AccountHeadName;
                    oVDObj.VDObjID = oVOReference.VOReferenceID;
                    oVDObj.ObjType = EnumBreakdownType.OrderReference;
                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.OrderReference;
                    oVDObj.OrderID = oVOReference.OrderID;
                    oVDObj.RefNo = oVOReference.RefNo;
                    oVDObj.OrderNo = oVOReference.OrderNo;
                    oVDObj.ORemarks = oVOReference.Remarks;
                    oVDObj.CID = oVOReference.CurrencyID;
                    oVDObj.CAmount = oVOReference.AmountInCurrency;
                    oVDObj.CRate = oVOReference.ConversionRate;
                    oVDObj.Amount = oVOReference.Amount;
                    oVDObj.CFormat = "Order Ref : " + (oVOReference.RefNo != "" ? (oVOReference.RefNo + ", " + oVOReference.OrderNo) : "N/A") + "," + oVOReference.Remarks + ", " + " @ " + Global.MillionFormat(oItem.AmountInCurrency) + " " + oItem.CUSymbol;
                    oVDObj.CName = oItem.CUName;
                    oVDObj.CSymbol = oItem.CUSymbol;
                    oVDObj.IsDr = oVOReference.IsDebit;
                    oVDObj.DR_CR = "Order Ref";
                    oVDObjs.Add(oVDObj);
                }
                #endregion

                #region Account Head Wise Naration
                if (bIsPrint)
                {
                    if (oItem.Narration != "" && oItem.Narration.ToLower() != "n/a")
                    {
                        oVDObj = new VDObj();
                        oVDObj.VDObjID = 0;
                        oVDObj.ObjType = EnumBreakdownType.VoucherDetail;
                        oVDObj.ObjTypeInt = (int)EnumBreakdownType.VoucherDetail;
                        oVDObj.Detail = oItem.Narration;
                        oVDObj.IsDr = false;
                        oVDObj.DR_CR = "Debit";
                        oVDObjs.Add(oVDObj);
                    }
                }
                #endregion
            }
            return oVDObjs;
        }
        public List<VDRptObj> MapVoucherExplanationObject(Voucher oVoucher)
        {
            int nBUID = 0;
            string sSQL = "";
            VDRptObj oVDRptObj = new VDRptObj();
            List<VDRptObj> oVDRptObjs = new List<VDRptObj>();
            _oVoucherDetaillst = new List<VoucherDetail>();
            if (_nBUID != 0)
            {
                sSQL = "SELECT * FROM View_VoucherDetail AS VD WHERE VD.VoucherID=" + oVoucher.VoucherID + " AND VD.BUID=" + _nBUID + " ORDER BY VD.BUID ASC";
            }
            else
            {
                sSQL = "SELECT * FROM View_VoucherDetail WHERE VoucherID=" + oVoucher.VoucherID + " ORDER BY BUID ASC";
            }

            _oVoucherDetaillst = VoucherDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (VoucherDetail oItem in _oVoucherDetaillst)
            {
                if (oItem.BUID != nBUID)
                {
                    oVDRptObj = new VDRptObj();
                    oVDRptObj.VDObjID = 0;
                    oVDRptObj.VoucherRecordType = EnumVoucherRecordType.BusinessUnit;
                    oVDRptObj.CAmount = 0;
                    oVDRptObj.CRate = 0;
                    oVDRptObj.Amount = 0;
                    oVDRptObj.IsDr = false;
                    oVDRptObj.DrAmount = 0;
                    oVDRptObj.CrAmount = 0;
                    oVDRptObj.BCDrAmount = 0;
                    oVDRptObj.BCCrAmount = 0;
                    oVDRptObjs.Add(oVDRptObj);

                    List<VDRptObj> oLedgerObjs = new List<VDRptObj>();
                    List<VDRptObj> oCostCenterObjs = new List<VDRptObj>();
                    oLedgerObjs = this.MapLedgers(oItem.BUID);
                    foreach (VDRptObj oTemp in oLedgerObjs)
                    {
                        oVDRptObjs.Add(oTemp);
                        oCostCenterObjs = new List<VDRptObj>();
                        oCostCenterObjs = this.MapCostCenters(oItem.BUID, oTemp.AHID, oTemp.IsDr);
                        if (oCostCenterObjs.Count > 0)
                        {
                            oVDRptObjs.AddRange(oCostCenterObjs);
                        }
                    }
                }
                nBUID = oItem.BUID;
            }
            return oVDRptObjs;
        }
        public List<CostCenterTransaction> MapCostCenterTransactions(List<CostCenterTransaction> oTempCostCenterTransactions, int nVoucherDetailID)
        {
            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            foreach (CostCenterTransaction oItem in oTempCostCenterTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    oCostCenterTransactions.Add(oItem);
                }
            }
            return oCostCenterTransactions;
        }
        public List<VoucherBillTransaction> MapVoucherBillTransaction(List<VoucherBillTransaction> oTempVoucherBillTransactions, int nVoucherDetailID, int nCCTID)
        {
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            foreach (VoucherBillTransaction oItem in oTempVoucherBillTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                {
                    oVoucherBillTransactions.Add(oItem);
                }
            }
            return oVoucherBillTransactions;
        }
        public List<VOReference> MapVOReferences(List<VOReference> oTempVOReferences, int nVoucherDetailID, int nCCTID)
        {
            List<VOReference> oVOReferences = new List<VOReference>();
            foreach (VOReference oItem in oTempVOReferences)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                {
                    oVOReferences.Add(oItem);
                }
            }
            return oVOReferences;
        }
        public List<VoucherCheque> MapVoucherCheques(List<VoucherCheque> oTempoVoucherCheques, int nVoucherDetailID, int nCCTID)
        {
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
            foreach (VoucherCheque oItem in oTempoVoucherCheques)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                {
                    oVoucherCheques.Add(oItem);
                }
            }
            return oVoucherCheques;
        }
        public List<VPTransaction> MapVPTransactions(List<VPTransaction> oTempVPTransactions, int nVoucherDetailID)
        {
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            foreach (VPTransaction oItem in oTempVPTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    oVPTransactions.Add(oItem);
                }
            }
            return oVPTransactions;
        }        
        private List<VDRptObj> MapLedgers(int nBUID)
        {
            VDRptObj oVDRptObj = new VDRptObj();
            List<VDRptObj> oLedgerObjs = new List<VDRptObj>();
            foreach (VoucherDetail oItem in _oVoucherDetaillst)
            {
                if (oItem.BUID == nBUID)
                {
                    if (!IsExists(oLedgerObjs, oItem.AccountHeadID, oItem.IsDebit))
                    {
                        VoucherDetail TempVD = new VoucherDetail();
                        TempVD = GetAmount(nBUID, oItem.AccountHeadID, oItem.IsDebit);
                        oVDRptObj = new VDRptObj();
                        oVDRptObj.VDObjID = 0;
                        oVDRptObj.VoucherRecordType = EnumVoucherRecordType.Ledger;
                        oVDRptObj.AHID = oItem.AccountHeadID;
                        oVDRptObj.CAmount = TempVD.AmountInCurrency;
                        oVDRptObj.CRate = TempVD.ConversionRate;
                        oVDRptObj.Amount = TempVD.Amount;
                        oVDRptObj.IsDr = oItem.IsDebit;
                        oVDRptObj.DetailText = oItem.AccountHeadName;
                        oVDRptObj.Code = oItem.AccountHeadCode;
                        oVDRptObj.DrAmount = TempVD.DebitAmount;
                        oVDRptObj.CrAmount = TempVD.CreditAmount;
                        oVDRptObj.BCDrAmount = TempVD.BCDebitAmount;
                        oVDRptObj.BCCrAmount = TempVD.BCCreditAmount;
                        oVDRptObj.Note = oItem.Narration;
                        oLedgerObjs.Add(oVDRptObj);
                    }
                }
            }
            return oLedgerObjs;
        }
        private bool IsExists(List<VDRptObj> oLedgewrObjs, int nAccountHeadID, bool bIsDr)
        {
            foreach (VDRptObj oItem in oLedgewrObjs)
            {
                if (oItem.AHID == nAccountHeadID && oItem.IsDr == bIsDr)
                {
                    return true;
                }
            }
            return false;
        }
        private VoucherDetail GetAmount(int nBUID, int nAccountHeadID, bool bIsDr)
        {
            VoucherDetail TempVD = new VoucherDetail();
            foreach (VoucherDetail oItem in _oVoucherDetaillst)
            {
                if (oItem.BUID == nBUID && oItem.AccountHeadID == nAccountHeadID && oItem.IsDebit == bIsDr)
                {
                    TempVD.AmountInCurrency = TempVD.AmountInCurrency + oItem.AmountInCurrency;
                    TempVD.ConversionRate = TempVD.ConversionRate;
                    TempVD.Amount = oItem.Amount + oItem.Amount;
                    TempVD.DebitAmount = TempVD.DebitAmount + oItem.DebitAmount;
                    TempVD.CreditAmount = TempVD.CreditAmount + oItem.CreditAmount;
                    TempVD.BCDebitAmount = TempVD.BCDebitAmount + oItem.BCDebitAmount;
                    TempVD.BCCreditAmount = TempVD.BCCreditAmount + oItem.BCCreditAmount;
                }
            }
            return TempVD;
        }
        private List<VDRptObj> MapCostCenters(int nBUID, int nAccountHeadID, bool bIsDebit)
        {
            VDRptObj oVDRptObj = new VDRptObj();
            List<VDRptObj> oCostCenterObjs = new List<VDRptObj>();
            foreach (VoucherDetail oItem in _oVoucherDetaillst)
            {

                if (oItem.CostCenterID > 0 && oItem.BUID == nBUID && oItem.AccountHeadID == nAccountHeadID && oItem.IsDebit == bIsDebit)
                {
                    oVDRptObj = new VDRptObj();
                    oVDRptObj.VDObjID = 0;
                    oVDRptObj.AHID = 0;
                    oVDRptObj.VoucherRecordType = EnumVoucherRecordType.CostCenter;
                    oVDRptObj.CCID = oItem.CostCenterID;
                    oVDRptObj.CAmount = oItem.AmountInCurrency;
                    oVDRptObj.CRate = 0;
                    oVDRptObj.Amount = 0;
                    oVDRptObj.IsDr = oItem.IsDebit;
                    oVDRptObj.DetailText = oItem.CCName;
                    oVDRptObj.Code = oItem.CCCode;
                    oVDRptObj.DrAmount = 0;
                    oVDRptObj.CrAmount = 0;
                    oVDRptObj.BCDrAmount = 0;
                    oVDRptObj.Note = oItem.Narration;
                    oVDRptObj.BCCrAmount = 0;
                    oCostCenterObjs.Add(oVDRptObj);
                }
            }
            return oCostCenterObjs;
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

        public System.Drawing.Image GetBULogo(BusinessUnit oBusinessUnit)
        {
            if (oBusinessUnit.BUImage != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oBusinessUnit.BUImage);
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

        #region print Voucher
        public ActionResult PrintVoucher(long id, int buid)
        {
            _oVoucher = new Voucher();
            _nBUID = buid;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<VDObj> oVDObjs = new List<VDObj>();
            _oVoucher = _oVoucher.Get(id, (int)Session[SessionInfo.currentUserID]);
            oVDObjs = this.MapVoucherExplanationObject(_oVoucher, true);
            oBusinessUnit = oBusinessUnit.GetWithImage(_oVoucher.BUID, (int)Session[SessionInfo.currentUserID]);
            oBusinessUnit.BULogo = GetBULogo(oBusinessUnit);
            _oVoucher.BusinessUnit = oBusinessUnit;
            _oVoucher.VDObjs = oVDObjs;            

            VoucherType oVoucherType = new VoucherType();
            oVoucherType = oVoucherType.Get(_oVoucher.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);
            _oVoucher.CurrentSession = (int)Session[SessionInfo.currentUserID];

            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.VoucherFormat, (int)Session[SessionInfo.currentUserID]);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.VoucherPreview, (int)Session[SessionInfo.currentUserID]);

            if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.SingleCurrencyVoucher)
            {
                bool bLogoPrint = false;
                oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.LogoPrintInVoucher, (int)Session[SessionInfo.currentUserID]);
                if (oClientOperationSetting.ClientOperationSettingID > 0)
                {
                    bLogoPrint = Convert.ToBoolean(Convert.ToInt16(oClientOperationSetting.Value));
                }

                rptVoucherSingleCurrency oReport = new rptVoucherSingleCurrency();
                byte[] abytes = oReport.PrepareReport(_oVoucher, _nBUID, bLogoPrint);
                return File(abytes, "application/pdf");
            }
            else
            {
                bool bLogoPrint = false;
                oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.LogoPrintInVoucher, (int)Session[SessionInfo.currentUserID]);
                if (oClientOperationSetting.ClientOperationSettingID > 0)
                {
                    bLogoPrint = Convert.ToBoolean(Convert.ToInt16(oClientOperationSetting.Value));
                }

                rptVoucherMultiCurrencyTemp oReport = new rptVoucherMultiCurrencyTemp();
                byte[] abytes = oReport.PrepareReport(_oVoucher, bLogoPrint, oSignatureSetups);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintVoucherHalfPage(long id, int buid)
        {
            _oVoucher = new Voucher();
            _nBUID = buid;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<VDObj> oVDObjs = new List<VDObj>();
            _oVoucher = _oVoucher.Get(id, (int)Session[SessionInfo.currentUserID]);
            oVDObjs = this.MapVoucherExplanationObject(_oVoucher, true);
            oBusinessUnit = oBusinessUnit.GetWithImage(_oVoucher.BUID, (int)Session[SessionInfo.currentUserID]);
            oBusinessUnit.BULogo = GetBULogo(oBusinessUnit);
            _oVoucher.BusinessUnit = oBusinessUnit;
            _oVoucher.VDObjs = oVDObjs;

            VoucherType oVoucherType = new VoucherType();
            oVoucherType = oVoucherType.Get(_oVoucher.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);
            _oVoucher.CurrentSession = (int)Session[SessionInfo.currentUserID];

            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.VoucherFormat, (int)Session[SessionInfo.currentUserID]);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.VoucherPreview, (int)Session[SessionInfo.currentUserID]);

            if (oVoucherType.IsProductRequired) //IsProductRequired use as a printing page size determine A4 or Half of A4
            {
                bool bLogoPrint = false;
                oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.LogoPrintInVoucher, (int)Session[SessionInfo.currentUserID]);
                if (oClientOperationSetting.ClientOperationSettingID > 0)
                {
                    bLogoPrint = Convert.ToBoolean(Convert.ToInt16(oClientOperationSetting.Value));
                }

                rptVoucherMultiCurrencyTemp oReport = new rptVoucherMultiCurrencyTemp();
                byte[] abytes = oReport.PrepareReport(_oVoucher, bLogoPrint, oSignatureSetups);
                return File(abytes, "application/pdf");
            }
            else
            {
                bool bLogoPrint = false;
                oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.LogoPrintInVoucher, (int)Session[SessionInfo.currentUserID]);
                if (oClientOperationSetting.ClientOperationSettingID > 0)
                {
                    bLogoPrint = Convert.ToBoolean(Convert.ToInt16(oClientOperationSetting.Value));
                }

                rptVoucherHalfPage oReport = new rptVoucherHalfPage();
                byte[] abytes = oReport.PrepareReport(_oVoucher, bLogoPrint, oSignatureSetups);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintMultiVoucher(string ids)
        {
            Voucher oVoucher = new Voucher();
            List<VDObj> oVDObjs = new List<VDObj>();            
            List<Voucher> oVouchers = new List<Voucher>();
            List<Voucher> oTempVouchers = new List<Voucher>();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            string sSQL = "SELECT * FROM View_Voucher AS HH WHERE HH.VoucherID IN (" + ids + ") ORDER BY HH.VoucherID ASC";
            oTempVouchers = Voucher.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (Voucher oItem in oTempVouchers)
            {
                oVoucher = new Voucher();
                oVDObjs = new List<VDObj>();                
                oBusinessUnit = new BusinessUnit();

                oVoucher = oVoucher.Get(oItem.VoucherID, (int)Session[SessionInfo.currentUserID]);
                oVDObjs = this.MapVoucherExplanationObject(oVoucher, true);
                oBusinessUnit = oBusinessUnit.GetWithImage(oVoucher.BUID, (int)Session[SessionInfo.currentUserID]);
                oBusinessUnit.BULogo = GetBULogo(oBusinessUnit);
                oVoucher.BusinessUnit = oBusinessUnit;
                oVoucher.VDObjs = oVDObjs;
                
                VoucherType oVoucherType = new VoucherType();
                oVoucherType = oVoucherType.Get(oVoucher.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);
                oVoucher.CurrentSession = (int)Session[SessionInfo.currentUserID];
                oVouchers.Add(oVoucher);
            }

            
            bool bLogoPrint = false;
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.LogoPrintInVoucher, (int)Session[SessionInfo.currentUserID]);
            if (oClientOperationSetting.ClientOperationSettingID > 0)
            {
                bLogoPrint = Convert.ToBoolean(Convert.ToInt16(oClientOperationSetting.Value));
            }

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.VoucherPreview, (int)Session[SessionInfo.currentUserID]);

            rptMultiVoucherMultiCurrency oReport = new rptMultiVoucherMultiCurrency();
            byte[] abytes = oReport.PrepareReport(oVouchers, bLogoPrint, oSignatureSetups);      
            return File(abytes, "application/pdf");
        }

        
        #region PrintVouchers
        public ActionResult PrintVouchers(string sIDs)
        {
            _oVoucher = new Voucher();
            string sSql = "SELECT * FROM View_Voucher WHERE OperationType=1 AND VoucherID IN (" + sIDs + ")";
            _oVoucher.VoucherList = Voucher.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get((int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oVoucher.Company = oCompany;
            rptVoucherList oReport = new rptVoucherList();
            string sMessage = "Voucher  List";
            byte[] abytes = oReport.PrepareReport(_oVoucher, sMessage);
            return File(abytes, "application/pdf");
        }


        #endregion


        #region PrintVouchersInXL
        public ActionResult PrintVouchersInXL(string sIDs)
        {
            _oVoucher = new Voucher();
            string sSql = "SELECT * FROM View_Voucher WHERE OperationType=1 AND VoucherID IN (" + sIDs + ")";
            _oVoucher.VoucherList = Voucher.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<VoucherXL>));

            //We load the data

            int nCount = 0; double nTotalAmount = 0;
            VoucherXL oVoucherXL = new VoucherXL();
            List<VoucherXL> oVoucherXLs = new List<VoucherXL>();
            foreach (Voucher oItem in _oVoucher.VoucherList)
            {
                nCount++;
                oVoucherXL = new VoucherXL();
                oVoucherXL.SLNo = nCount.ToString();
                oVoucherXL.VoucherNo = oItem.VoucherNo;
                oVoucherXL.VoucherName = oItem.VoucherName;
                oVoucherXL.Date = oItem.VoucherDateInString;
                oVoucherXL.AuthorizedBy = oItem.AuthorizedByName;
                oVoucherXL.Amount = oItem.VoucherAmount;
                oVoucherXLs.Add(oVoucherXL);
                nTotalAmount += nTotalAmount + oItem.VoucherAmount;
            }

            #region Total
            oVoucherXL = new VoucherXL();
            oVoucherXL.SLNo = "";
            oVoucherXL.VoucherNo = "";
            oVoucherXL.VoucherName = "";
            oVoucherXL.Date = "";
            oVoucherXL.AuthorizedBy = "Total :";
            oVoucherXL.Amount = nTotalAmount;
            oVoucherXLs.Add(oVoucherXL);
            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oVoucherXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Vouchers.xls");
        }
        #endregion

        #endregion

        #region Advance Search
        [HttpPost]
        public JsonResult AdvanceSearch(Voucher oVoucher)
        {
            List<Voucher> oVouchers = new List<Voucher>();
            try
            {
                string sSQL = GetSQL(oVoucher);
                oVouchers = Voucher.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oVoucher = new Voucher();
                oVouchers = new List<Voucher>();
                oVoucher.ErrorMessage = ex.Message;
                oVouchers.Add(oVoucher);
            }

            var jsonResult = Json(oVouchers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(Voucher oVoucher)
        {
            string sSearchingData = oVoucher.ReferenceNote;

            int nVoucherDateDate = Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dVrStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dVrEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            int nVoucherAmount = Convert.ToInt32(sSearchingData.Split('~')[3]);
            Double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[4]);
            Double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[5]);
            string sAccountHeadIDs = sSearchingData.Split('~')[6];
            string sOperation = sSearchingData.Split('~')[7];
            string sSubledgerName = sSearchingData.Split('~')[8];
            string sBatchNo = sSearchingData.Split('~')[9];


            string sReturn1 = "SELECT * FROM View_Voucher AS HH";
            string sReturn = "";
                        
            #region VoucherNo
            if (oVoucher.VoucherNo != null && oVoucher.VoucherNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.VoucherNo LIKE '%" + oVoucher.VoucherNo + "%'";
            }
            #endregion

            #region VoucherTypeID
            if (oVoucher.VoucherTypeID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.VoucherTypeID = " + oVoucher.VoucherTypeID.ToString();
            }
            #endregion

            #region AuthorizedBy
            if (oVoucher.AuthorizedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.AuthorizedBy = " + oVoucher.AuthorizedBy.ToString();
            }
            #endregion

            #region BUID
            if (oVoucher.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BUID = " + oVoucher.BUID.ToString();
            }
            #endregion

            #region CurrencyID
            if (oVoucher.CurrencyID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.CurrencyID = " + oVoucher.CurrencyID.ToString();
            }
            #endregion

            #region Date & Amount
            if (nVoucherDateDate != (int)EnumCompareOperator.None)
            {
                if (nVoucherDateDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (nVoucherDateDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (nVoucherDateDate == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (nVoucherDateDate == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (nVoucherDateDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (nVoucherDateDate == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dVrEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }

            if (nVoucherAmount > 0)
            {
                if (nVoucherAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VoucherAmount = " + nStartAmount;
                }
                if (nVoucherAmount == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VoucherAmount != " + nStartAmount;
                }
                if (nVoucherAmount == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VoucherAmount > " + nStartAmount;
                }
                if (nVoucherAmount == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VoucherAmount < " + nStartAmount;
                }
                if (nVoucherAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VoucherAmount >= " + nStartAmount + " AND VoucherAmount < " + nEndAmount;
                }
                if (nVoucherAmount == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VoucherAmount < " + nStartAmount + " OR VoucherAmount > " + nEndAmount;
                }
            }
            #endregion

            #region AccountHead
            if (sAccountHeadIDs != null && sAccountHeadIDs!="")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.VoucherID in (Select VoucherDetail.VoucherID from VoucherDetail where VoucherDetail.AccountHeadID in (" + sAccountHeadIDs + ")) ";
            }
            #endregion

            #region Narration
            if (oVoucher.Narration != null && oVoucher.Narration != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.Narration LIKE '%" + oVoucher.Narration + "%'";
            }
            #endregion
            
            #region OperationType
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " HH.OperationType=1 ";
            #endregion

            #region Voucher Batch
            if (sBatchNo != null && sBatchNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.VoucherBatchNO LIKE '%" + sBatchNo + "%'";
            }
            else
            {
                if (sOperation != "Approved")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.BatchID IN ( SELECT TT.VoucherBatchID FROM View_VoucherBatch AS TT WHERE TT.CreateBy =" + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";
                }
            }
            #endregion

            #region Subledger
            if (sSubledgerName != null && sSubledgerName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.VoucherID IN (SELECT MM.VoucherID FROM View_CostCenterTransaction AS MM WHERE MM.CostCenterName LIKE '%" + sSubledgerName + "%') ";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " order by HH.VoucherID ASC";
            return sReturn;
        }

        [HttpPost]
        public JsonResult GetsByBatch(VoucherBatch oVoucherBatch)
        {
            List<Voucher> oVouchers = new List<Voucher>();
            try
            {
                oVoucherBatch.ErrorMessage = oVoucherBatch.ErrorMessage == null ? "" : oVoucherBatch.ErrorMessage;
                if (oVoucherBatch.ErrorMessage != "")
                {
                    if (oVoucherBatch.ErrorMessage == "Approved")
                    {
                        oVouchers = Voucher.GetsByBatchForApprove(oVoucherBatch.VoucherBatchID, (int)Session[SessionInfo.currentUserID]);
                    }
                    else if (oVoucherBatch.ErrorMessage == "Entry")
                    {
                        oVouchers = Voucher.GetsByBatch(oVoucherBatch.VoucherBatchID, (int)Session[SessionInfo.currentUserID]);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                oVouchers = new List<Voucher>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVouchers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsUnApprovedVouchers(Voucher oVoucher)
        {
            List<Voucher> oVouchers = new List<Voucher>();
            try
            {
                DateTime dStartDate = DateTime.Today;
                DateTime dEndDate = DateTime.Today;
                if (oVoucher.Narration != null && oVoucher.Narration != "")
                {
                    dStartDate = Convert.ToDateTime(oVoucher.Narration.Split('~')[0]);
                    dEndDate = Convert.ToDateTime(oVoucher.Narration.Split('~')[1]);
                }
                string sSQL = "SELECT * FROM View_Voucher AS HH WHERE CONVERT(DATE,CONVERT(VARCHAR(12),HH.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106)) AND ISNULL(HH.AuthorizedBy,0)=0 ";
                if (oVoucher.BUID > 0)
                {
                    sSQL = sSQL + " AND HH.BUID =" + oVoucher.BUID.ToString();
                }
                if (oVoucher.VoucherTypeID > 0)
                {
                    sSQL = sSQL + " AND HH.VoucherTypeID= " + oVoucher.VoucherTypeID.ToString();
                }
                sSQL = sSQL + " ORDER BY HH.VoucherDate ASC";
                oVouchers = Voucher.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oVouchers = new List<Voucher>();
            }

            var jsonResult = Json(oVouchers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Gets ChartofAccounts, Cheque & Sales Reference
        [HttpPost]
        public JsonResult GetsByCodeOrName(ChartsOfAccount oChartsOfAccount)
        {
            VoucherType oVoucherType = new VoucherType();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            oChartsOfAccounts = ChartsOfAccount.GetsByCodeOrNameWithBUForVoucher(oChartsOfAccount, (int)Session[SessionInfo.currentUserID]);
            oVoucherType = oVoucherType.Get(oChartsOfAccount.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);

            foreach (ChartsOfAccount oItem in oChartsOfAccounts)
            {
                oItem.IsChequeApply = oItem.AccountOperationType == EnumAccountOperationType.BankAccount || oItem.AccountOperationType == EnumAccountOperationType.BankClearing;
                oItem.IsPaymentCheque = oVoucherType.IsPaymentCheque;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByCodeOrNameForExportEncashment(ChartsOfAccount oChartsOfAccount)
        {
            VoucherType oVoucherType = new VoucherType();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            oChartsOfAccounts = ChartsOfAccount.GetsByCodeOrNameWithBU(oChartsOfAccount, (int)Session[SessionInfo.currentUserID]);
         

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCheques(VoucherCheque oVoucherCheque)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";
            if (oVoucherCheque.ChequeType == EnumChequeType.Received)
            {
                oVoucherCheque.ChequeNo = oVoucherCheque.ChequeNo == null ? "" : oVoucherCheque.ChequeNo;
                List<ReceivedCheque> oReceivedCheques = new List<ReceivedCheque>();
                string sSQL = "";
                sSQL = "SELECT * FROM View_ReceivedCheque WHERE ChequeNo LIKE '%" + oVoucherCheque.ChequeNo + "%' AND SubLedgerID=" + oVoucherCheque.CCID + " AND ReceivedChequeID NOT IN (SELECT TT.ChequeID FROM VoucherCheque AS TT WHERE TT.ChequeType=1) OR ReceivedChequeID = " + oVoucherCheque.ChequeID;
                oReceivedCheques = ReceivedCheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sjson = serializer.Serialize((object)oReceivedCheques);
            }
            else
            {
                List<Cheque> oCheques = new List<Cheque>();
                //string sSQL = "SELECT * FROM View_Cheque WHERE ChequeID NOT IN (SELECT TT.ChequeID FROM VoucherCheque AS TT WHERE TT.ChequeType=2) OR ChequeID =" + oVoucherCheque.ChequeID;
                //string sSQL = "SELECT * FROM View_Cheque WHERE ChequeStatus = " + (int)EnumChequeStatus.Issued + " AND BankAccountID=(SELECT TT.ReferenceObjectID FROM ACCostCenter AS TT WHERE TT.ACCostCenterID=" + oVoucherCheque.CCID.ToString() + " AND TT.ReferenceType=" + ((int)EnumReferenceType.BankAccount).ToString() + ")";
                string sSQL = "SELECT * FROM View_Cheque WHERE ChequeStatus = " + (int)EnumChequeStatus.Issued;
                oCheques = Cheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sjson = serializer.Serialize((object)oCheques);
            }
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLastNarration(Voucher oVoucher)
        {
            oVoucher = oVoucher.GetLastNarration((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsOrderReferences(VOrder oVOrder)
        {
            string sSQL = "";
            List<VOrder> oVOrders = new List<VOrder>();
            if (oVOrder.SubledgerID > 0)
            {
                sSQL = "SELECT * FROM View_VOrder AS HH WHERE HH.BUID=" + oVOrder.BUID.ToString() + " AND HH.SubledgerID=" + oVOrder.SubledgerID.ToString() + " AND (ISNULL(HH.RefNo,'')+ISNULL(HH.OrderNo,'')+ISNULL(HH.SubledgerName,'')) LIKE '%" + oVOrder.OrderNo + "%' ORDER BY VOrderID ASC";
            }
            else
            {
                sSQL = "SELECT * FROM View_VOrder AS HH WHERE HH.BUID=" + oVOrder.BUID.ToString() + " AND (ISNULL(HH.RefNo,'')+ISNULL(HH.OrderNo,'')+ISNULL(HH.SubledgerName,'')) LIKE '%" + oVOrder.OrderNo + "%' ORDER BY VOrderID ASC";
            }
            oVOrders = VOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AutoVoucher Process
        public ActionResult ViewAutoVoucherProcess(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<IntegrationSetup> oIntegrationSetups = new List<IntegrationSetup>();            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(oIntegrationSetups);
        }

        public ActionResult ViewAutoVoucher(int id, double ts)
        {
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            _oVoucher = new Voucher();
            Company oCompany = new Company();
            _oVoucher.LstCurrency = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oVoucher.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oVoucher.RunningAccountingYear = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
            return PartialView(_oVoucher);
        }

        [HttpPost]
        public JsonResult GetsYetToEffectsData(IntegrationSetup oIntegrationSetup)
        {
            _oVouchers = new List<Voucher>();
            _oVoucher = new Voucher();
            _oIntegrationSetup = new IntegrationSetup();
            _oIntegrationSetupDetails = new List<IntegrationSetupDetail>();
            List<DataCollectionSetup> oDetailDataCollectionSetups = new List<DataCollectionSetup>();
            List<DataCollectionSetup> oDrCrDataCollectionSetups = new List<DataCollectionSetup>();

            try
            {
                _oIntegrationSetup = _oIntegrationSetup.Get(oIntegrationSetup.IntegrationSetupID, (int)Session[SessionInfo.currentUserID]);
                _oIntegrationSetup.DateType = oIntegrationSetup.DateType;
                _oIntegrationSetup.StartDate = oIntegrationSetup.StartDate;
                _oIntegrationSetup.EndDate = oIntegrationSetup.EndDate;                
                _oIntegrationSetupDetails = IntegrationSetupDetail.Gets(oIntegrationSetup.IntegrationSetupID, (int)Session[SessionInfo.currentUserID]);
                _oDebitCreditSetups = DebitCreditSetup.GetsByIntegrationSetup(oIntegrationSetup.IntegrationSetupID, (int)Session[SessionInfo.currentUserID]);
                oDetailDataCollectionSetups = DataCollectionSetup.GetsByIntegrationSetup(oIntegrationSetup.IntegrationSetupID, EnumDataReferenceType.IntegrationDetail, (int)Session[SessionInfo.currentUserID]); // Get for Details
                oDrCrDataCollectionSetups = DataCollectionSetup.GetsByIntegrationSetup(oIntegrationSetup.IntegrationSetupID, EnumDataReferenceType.DebitCreditSetup, (int)Session[SessionInfo.currentUserID]); // Get for DebitCredit
                _oIntegrationSetup.IntegrationSetupDetails = GetSetupDetails(_oIntegrationSetupDetails, _oDebitCreditSetups, oDetailDataCollectionSetups, oDrCrDataCollectionSetups);
                List<Voucher> oVouchers = new List<Voucher>();
                oVouchers = Voucher.GetsAutoVoucher(_oIntegrationSetup, null, false, (int)Session[SessionInfo.currentUserID]);
                _oVouchers = Voucher.MapVoucherExplanationObject(oVouchers);
            }
            catch (Exception ex)
            {
                _oVouchers = new List<Voucher>();
                _oVoucher = new Voucher();
                _oVoucher.ErrorMessage = ex.Message;
                _oVouchers.Add(_oVoucher);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVouchers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitAutoVoucher(List<Voucher> oVouchers)        
        {
            _oVouchers = new List<Voucher>();
            Voucher oTempVoucher = new Voucher();
            string sFeedBackMessage = "";
            try
            {
                foreach (Voucher oVoucher in oVouchers)
                {
                    #region Voucher
                    oTempVoucher = new Voucher();
                    oTempVoucher.VoucherID = oVoucher.VoucherID;
                    oTempVoucher.BUID = oVoucher.BUID;
                    oTempVoucher.BUCode = oVoucher.BUCode;
                    oTempVoucher.BUName = oVoucher.BUName;
                    oTempVoucher.VoucherTypeID = oVoucher.VoucherTypeID;
                    oTempVoucher.VoucherNo = oVoucher.VoucherNo;
                    oTempVoucher.Narration = oVoucher.Narration;
                    oTempVoucher.ReferenceNote = oVoucher.ReferenceNote;
                    oTempVoucher.VoucherDate = oVoucher.VoucherDate;
                    oTempVoucher.AuthorizedBy = 0;
                    oTempVoucher.BatchID = oVoucher.BatchID;
                    oTempVoucher.LastUpdatedDate = oVoucher.LastUpdatedDate;
                    oTempVoucher.OperationType = oVoucher.OperationType;
                    oTempVoucher.VoucherAmount = oVoucher.VoucherAmount;
                    oTempVoucher.NewVoucherNo = oVoucher.NewVoucherNo;
                    oTempVoucher.ErrorMessage = oVoucher.ErrorMessage;
                    oTempVoucher.AccountingSessionID = oVoucher.AccountingSessionID;
                    oTempVoucher.ProfitLossAppropriationAccountsInString = oVoucher.ProfitLossAppropriationAccountsInString;
                    oTempVoucher.VDObjs = new List<VDObj>();
                    oTempVoucher.VoucherDetailLst = new List<VoucherDetail>();
                    oTempVoucher.NumberMethodInInt = 0;
                    oTempVoucher.VoucherMapping = oVoucher.VoucherMapping;
                    oTempVoucher.TableName = oVoucher.TableName;
                    oTempVoucher.PKValue = oVoucher.PKValue;
                    oTempVoucher.CRate = oVoucher.VDObjs[0].CRate;
                    oTempVoucher.VoucherDetailLst = this.MapVoucherDetailObject(oTempVoucher, oVoucher.VDObjs);

                    #region Validate Data
                    sFeedBackMessage = "";
                    sFeedBackMessage = FeedBackForDataValidation(oTempVoucher.VoucherDetailLst);
                    if (sFeedBackMessage != "")
                    {
                        throw new Exception(sFeedBackMessage);
                    }
                    #endregion

                    #region Debit Credit Equal
                    sFeedBackMessage = "";
                    sFeedBackMessage = FeedBackForDebitCreditEqual(oTempVoucher.VoucherDetailLst);
                    if (sFeedBackMessage != "")
                    {
                        throw new Exception(sFeedBackMessage);
                    }
                    #endregion

                    _oVouchers.Add(oTempVoucher);
                    #endregion
                }
                _oVouchers = Voucher.CommitAutoVoucher(_oVouchers, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVoucher = new Voucher();
                _oVouchers = new List<Voucher>();
                _oVoucher.ErrorMessage = ex.Message;
                _oVouchers.Add(_oVoucher);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVouchers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<IntegrationSetupDetail> GetSetupDetails(List<IntegrationSetupDetail> oIntegrationSetupDetails, List<DebitCreditSetup> oDebitCreditSetups, List<DataCollectionSetup> oDetailDataCollectionSetups, List<DataCollectionSetup> oDrCrDataCollectionSetups)
        {

            foreach (IntegrationSetupDetail oItem in oIntegrationSetupDetails)
            {
                oItem.DebitCreditSetups = GetDebitCreditSetups(oItem.IntegrationSetupDetailID, oDebitCreditSetups, oDrCrDataCollectionSetups);
                oItem.DataCollectionSetups = GetDataCollectionSetups(oItem.IntegrationSetupDetailID, oDetailDataCollectionSetups); // Set Data Collection for single Integration setup Detail

            }
            return oIntegrationSetupDetails;
        }
        private List<DebitCreditSetup> GetDebitCreditSetups(int id, List<DebitCreditSetup> oDebitCreditSetups, List<DataCollectionSetup> oDrCrDataCollectionSetups)
        {
            _oDebitCreditSetups = new List<DebitCreditSetup>();

            foreach (DebitCreditSetup oItem in oDebitCreditSetups)
            {
                if (oItem.IntegrationSetupDetailID == id)
                {
                    oItem.DataCollectionSetups = GetDataCollectionSetups(oItem.DebitCreditSetupID, oDrCrDataCollectionSetups); // Set Data Collection for single Debit Credit setup
                    _oDebitCreditSetups.Add(oItem);
                }
            }
            return _oDebitCreditSetups;
        }
        private List<DataCollectionSetup> GetDataCollectionSetups(int id, List<DataCollectionSetup> oDataCollectionSetups)
        {
            _oDataCollectionSetups = new List<DataCollectionSetup>();
            foreach (DataCollectionSetup oItem in oDataCollectionSetups)
            {
                if (id == oItem.DataReferenceID)
                {
                    _oDataCollectionSetups.Add(oItem);
                }
            }
            return _oDataCollectionSetups;
        }
        #endregion
        
    }
}
