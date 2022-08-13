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
using ICS.Core.Utility;
using System.Web;


namespace ESimSolFinancial.Controllers
{
    public class ReceivedChequeController : Controller
    {
        #region Declaration
        ReceivedCheque _oReceivedCheque = new ReceivedCheque();
        List<ReceivedCheque> _oReceivedCheques = new List<ReceivedCheque>();
        string _sErrorMessage = "";
        string _sSQL = "";
        #endregion
        #region Functions
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_ReceivedCheque";
            string sChequeNo = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
             string sSQL = "";


            #region ChequeNo
            if (sChequeNo != null)
            {
                if (sChequeNo != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ChequeNo LIKE '%" + sChequeNo + "%' ";
                }
            }
            #endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
        }
        #endregion

        #region Used Code
        public ActionResult ViewReceivedCheques( string gfdb, int menuid)
        {
        
            if (gfdb == null)
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
                this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ReceivedCheque).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));

                _oReceivedCheques = new List<ReceivedCheque>();       
               //_oReceivedCheques = ReceivedCheque.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            return View(_oReceivedCheques);
        }
        //[HttpPost]
        public ActionResult ViewReceivedCheque(int id)
        {
            _oReceivedCheque = new ReceivedCheque();
            if (id > 0)
            {
                _oReceivedCheque = _oReceivedCheque.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oReceivedCheque.TransactionTypes = EnumObject.jGets(typeof(EnumTransactionType));
            return View(_oReceivedCheque);
        }
        public ActionResult ViewVoucherReceivedCheque(int id,int ccid)
        {
            _oReceivedCheque = new ReceivedCheque();
            if (id > 0)
            {
                _oReceivedCheque = _oReceivedCheque.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            BankAccount oBankAccount = new BankAccount();
            oBankAccount = oBankAccount.GetViaCostCenter(ccid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oReceivedCheque.SubLedgerID = ccid;
            _oReceivedCheque.ReceivedAccontID = oBankAccount.BankAccountID;
            _oReceivedCheque.ReceivedAccontNo = oBankAccount.AccountNo;
            _oReceivedCheque.TransactionTypes = EnumObject.jGets(typeof(EnumTransactionType));
            return PartialView(_oReceivedCheque);
        }

        #region Actions
       
        [HttpPost]
        public JsonResult Cancel(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            try
            {
                _oReceivedCheque = oReceivedCheque;
                #region Cheque History
                ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
                oReceivedChequeHistory.PreviousStatus = _oReceivedCheque.ChequeStatus;
                oReceivedChequeHistory.CurrentStatus = EnumReceivedChequeStatus.Cancel;
                oReceivedChequeHistory.ReceivedChequeID = _oReceivedCheque.ReceivedChequeID;
                oReceivedChequeHistory.Note = (_oReceivedCheque.ErrorMessage == null) ? "" : _oReceivedCheque.ErrorMessage;
                oReceivedChequeHistory.ChangeLog = oReceivedChequeHistory.CurrentStatusInString + " by " + ((User)Session[SessionInfo.CurrentUser]).UserName + ". \nPrevious Status: " + oReceivedChequeHistory.PreviousStatusInString + " Current Status: " + oReceivedChequeHistory.CurrentStatusInString;
                #endregion
                _oReceivedCheque = _oReceivedCheque.UpdateReceivedChequeStatus(oReceivedChequeHistory, (int)Session[SessionInfo.currentUserID]);
                if (_oReceivedCheque.ReceivedChequeID > 0)
                {
                    _oReceivedCheque.ErrorMessage = "Cheque Cancel Successfully";
                }
            }
            catch (Exception ex)
            {
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Return(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            try
            {
                _oReceivedCheque= oReceivedCheque;
                #region ReceivedChequeHistory
                ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
                oReceivedChequeHistory.PreviousStatus = _oReceivedCheque.ChequeStatus;
                oReceivedChequeHistory.CurrentStatus = EnumReceivedChequeStatus.Return;
                oReceivedChequeHistory.ReceivedChequeID = _oReceivedCheque.ReceivedChequeID;
                oReceivedChequeHistory.Note = (_oReceivedCheque.ErrorMessage == null) ? "" : _oReceivedCheque.ErrorMessage;
                oReceivedChequeHistory.ChangeLog = oReceivedChequeHistory.CurrentStatusInString + " by " + ((User)Session[SessionInfo.CurrentUser]).UserName + ". \nPrevious Status: " + oReceivedChequeHistory.PreviousStatusInString + " Current Status: " + oReceivedChequeHistory.CurrentStatusInString;
                #endregion
                _oReceivedCheque = _oReceivedCheque.UpdateReceivedChequeStatus(oReceivedChequeHistory, (int)Session[SessionInfo.currentUserID]);
                if (_oReceivedCheque.ReceivedChequeID > 0)
                {
                    _oReceivedCheque.ErrorMessage = "Cheque Return Successfully";
                }
            }
            catch (Exception ex)
            {
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Dishonor(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            try
            {
                _oReceivedCheque = oReceivedCheque;
                #region Cheque History
                ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
                oReceivedChequeHistory.PreviousStatus = _oReceivedCheque.ChequeStatus;
                oReceivedChequeHistory.CurrentStatus = EnumReceivedChequeStatus.Dishonor;
                oReceivedChequeHistory.ReceivedChequeID = _oReceivedCheque.ReceivedChequeID;
                oReceivedChequeHistory.Note = (_oReceivedCheque.ErrorMessage == null) ? "" : _oReceivedCheque.ErrorMessage;
                oReceivedChequeHistory.ChangeLog = oReceivedChequeHistory.CurrentStatusInString + " by " + ((User)Session[SessionInfo.CurrentUser]).UserName + ". \nPrevious Status: " + oReceivedChequeHistory.PreviousStatusInString + " Current Status: " + oReceivedChequeHistory.CurrentStatusInString;
                #endregion
                _oReceivedCheque = _oReceivedCheque.UpdateReceivedChequeStatus(oReceivedChequeHistory, (int)Session[SessionInfo.currentUserID]);
                if (_oReceivedCheque.ReceivedChequeID > 0)
                {
                    _oReceivedCheque.ErrorMessage = "Cheque Dishonor Successfully";
                }
            }
            catch (Exception ex)
            {
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Encash(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            try
            {
                _oReceivedCheque = oReceivedCheque;
                #region Cheque History
                ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
                oReceivedChequeHistory.PreviousStatus = _oReceivedCheque.ChequeStatus;
                oReceivedChequeHistory.CurrentStatus = EnumReceivedChequeStatus.Encash;
                oReceivedChequeHistory.ReceivedChequeID = _oReceivedCheque.ReceivedChequeID;
                oReceivedChequeHistory.Note = (_oReceivedCheque.ErrorMessage == null) ? "" : _oReceivedCheque.ErrorMessage;
                oReceivedChequeHistory.ChangeLog = oReceivedChequeHistory.CurrentStatusInString + " by " + ((User)Session[SessionInfo.CurrentUser]).UserName + ". \nPrevious Status: " + oReceivedChequeHistory.PreviousStatusInString + " Current Status: " + oReceivedChequeHistory.CurrentStatusInString;
                #endregion
                _oReceivedCheque = _oReceivedCheque.UpdateReceivedChequeStatus(oReceivedChequeHistory, (int)Session[SessionInfo.currentUserID]);
                if (_oReceivedCheque.ReceivedChequeID > 0)
                {
                    _oReceivedCheque.ErrorMessage = "Cheque Encash Successfully";
                }
            }
            catch (Exception ex)
            {
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Authorized(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            try
            {
                _oReceivedCheque = oReceivedCheque;
                #region Cheque History
                ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
                oReceivedChequeHistory.PreviousStatus = _oReceivedCheque.ChequeStatus;
                oReceivedChequeHistory.CurrentStatus = EnumReceivedChequeStatus.Authorized;
                oReceivedChequeHistory.ReceivedChequeID = _oReceivedCheque.ReceivedChequeID;
                oReceivedChequeHistory.Note = (_oReceivedCheque.ErrorMessage == null) ? "" : _oReceivedCheque.ErrorMessage;
                oReceivedChequeHistory.ChangeLog = oReceivedChequeHistory.CurrentStatusInString + " by " + ((User)Session[SessionInfo.CurrentUser]).UserName + ". \nPrevious Status: " + oReceivedChequeHistory.PreviousStatusInString + " Current Status: " + oReceivedChequeHistory.CurrentStatusInString;
                #endregion
                _oReceivedCheque = _oReceivedCheque.UpdateReceivedChequeStatus(oReceivedChequeHistory, (int)Session[SessionInfo.currentUserID]);
                if (_oReceivedCheque.ReceivedChequeID > 0)
                {
                    _oReceivedCheque.ErrorMessage = "Cheque Authorized Successfully";
                }
            }
            catch (Exception ex)
            {
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReceivedFromParty(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            try
            {
                _oReceivedCheque = oReceivedCheque;
                ReceivedChequeHistory oReceivedChequeHistory = new ReceivedChequeHistory();
                if (_oReceivedCheque.ChequeStatus == EnumReceivedChequeStatus.Initiate)
                {
                    #region Cheque History
                    oReceivedChequeHistory = new ReceivedChequeHistory();
                    oReceivedChequeHistory.PreviousStatus = _oReceivedCheque.ChequeStatus;
                    oReceivedChequeHistory.CurrentStatus = EnumReceivedChequeStatus.Authorized;
                    oReceivedChequeHistory.ReceivedChequeID = _oReceivedCheque.ReceivedChequeID;
                    oReceivedChequeHistory.Note = (_oReceivedCheque.ErrorMessage == null) ? "" : _oReceivedCheque.ErrorMessage;
                    oReceivedChequeHistory.ChangeLog = oReceivedChequeHistory.CurrentStatusInString + " by " + ((User)Session[SessionInfo.CurrentUser]).UserName + ". \nPrevious Status: " + oReceivedChequeHistory.PreviousStatusInString + " Current Status: " + oReceivedChequeHistory.CurrentStatusInString;
                    #endregion
                    _oReceivedCheque = _oReceivedCheque.UpdateReceivedChequeStatus(oReceivedChequeHistory, (int)Session[SessionInfo.currentUserID]);
                }
                #region Cheque History
                oReceivedChequeHistory = new ReceivedChequeHistory();
                oReceivedChequeHistory.PreviousStatus = _oReceivedCheque.ChequeStatus;
                oReceivedChequeHistory.CurrentStatus = EnumReceivedChequeStatus.ReceivedFromParty;
                oReceivedChequeHistory.ReceivedChequeID = _oReceivedCheque.ReceivedChequeID;
                oReceivedChequeHistory.Note = (_oReceivedCheque.ErrorMessage == null) ? "" : _oReceivedCheque.ErrorMessage;
                oReceivedChequeHistory.ChangeLog = oReceivedChequeHistory.CurrentStatusInString + " by " + ((User)Session[SessionInfo.CurrentUser]).UserName + ". \nPrevious Status: " + oReceivedChequeHistory.PreviousStatusInString + " Current Status: " + oReceivedChequeHistory.CurrentStatusInString;
                #endregion
                _oReceivedCheque = _oReceivedCheque.UpdateReceivedChequeStatus(oReceivedChequeHistory, (int)Session[SessionInfo.currentUserID]);
                if (_oReceivedCheque.ReceivedChequeID > 0)
                {
                    _oReceivedCheque.ErrorMessage = "Cheque Received Successfully";
                }
            }
            catch (Exception ex)
            {
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        #endregion

        [HttpPost]
        public JsonResult GetVouchers(ReceivedCheque oReceivedCheque)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";

            #region Create Voucher Batch
            string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE (TT.CreateBy =" + ((int)Session[SessionInfo.currentUserID]).ToString() + " AND  TT.BatchStatus=1) ORDER BY VoucherBatchID ASC";
            List<VoucherBatch> oVoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (oVoucherBatchs.Count <= 0)
            {
                VoucherBatch oVB = new VoucherBatch();
                oVB.BatchStatus = EnumVoucherBatchStatus.BatchOpen;
                oVB = oVB.Save((int)Session[SessionInfo.currentUserID]);
                oVoucherBatchs.Add(oVB);
            }
            #endregion

            IntegrationSetup oIntegrationSetup = new IntegrationSetup();
            oIntegrationSetup = oIntegrationSetup.GetByVoucherSetup(oReceivedCheque.Setup, (int)Session[SessionInfo.currentUserID]);
            if (oIntegrationSetup.IntegrationSetupID <= 0)
            {
                oReceivedCheque = new ReceivedCheque();
                oReceivedCheque.ErrorMessage = "Please Configure Integration Setup for Received Cheque";
                serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize(oReceivedCheque);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Voucher> oVouchers = new List<Voucher>();
                oVouchers = Voucher.GetsAutoVoucher(oIntegrationSetup, (object)oReceivedCheque, true, (int)Session[SessionInfo.currentUserID]);

                if (oVouchers[0].ErrorMessage == "")
                {
                    oReceivedCheque.Vouchers = new List<Voucher>();
                    oReceivedCheque.Vouchers = Voucher.MapVoucherExplanationObject(oVouchers);
                    sjson = serializer.Serialize(oReceivedCheque);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    oReceivedCheque = new ReceivedCheque();
                    oReceivedCheque.ErrorMessage = oVouchers[0].ErrorMessage;
                    serializer = new JavaScriptSerializer();
                    sjson = serializer.Serialize(oReceivedCheque);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult Gets(ReceivedCheque oReceivedCheque)
        {
            List<ReceivedCheque> oReceivedCheques = new List<ReceivedCheque>();
            oReceivedCheque.ErrorMessage = oReceivedCheque.ErrorMessage == null ? "" : oReceivedCheque.ErrorMessage;
            this.MakeSQL(oReceivedCheque.ErrorMessage);

            oReceivedCheques = ReceivedCheque.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oReceivedCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsForSuggestion(ReceivedCheque oReceivedCheque)
        {
            List<ReceivedCheque> oReceivedCheques = new List<ReceivedCheque>();

            oReceivedCheques = oReceivedCheque.GetsForSuggestion(((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oReceivedCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsACCostCenter(ACCostCenter oACCostCenter)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            string sSQL = "";
            sSQL = "SELECT * FROM View_ACCostCenter AS ACC WHERE ACC.NameCode LIKE '%" + oACCostCenter.Name + "%'";
            oACCostCenters = ACCostCenter.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            try
            {
                if (oReceivedCheque.ReceivedChequeID > 0)
                {
                    _oReceivedCheque = _oReceivedCheque.Get(oReceivedCheque.ReceivedChequeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oReceivedCheque = new ReceivedCheque();
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
            #endregion
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

        [HttpPost]
        public JsonResult Save(ReceivedCheque oReceivedCheque)
        {
            _oReceivedCheque = new ReceivedCheque();
            oReceivedCheque.AccountNo = oReceivedCheque.AccountNo == null ? "" : oReceivedCheque.AccountNo;
            oReceivedCheque.BankName = oReceivedCheque.BankName == null ? "" : oReceivedCheque.BankName;
            oReceivedCheque.BranchName = oReceivedCheque.BranchName == null ? "" : oReceivedCheque.BranchName;
            oReceivedCheque.ChequeNo = oReceivedCheque.ChequeNo == null ? "" : oReceivedCheque.ChequeNo;
            oReceivedCheque.Remarks = oReceivedCheque.Remarks == null ? "" : oReceivedCheque.Remarks;
            oReceivedCheque.MoneyReceiptNo = oReceivedCheque.MoneyReceiptNo == null ? "" : oReceivedCheque.MoneyReceiptNo;
            oReceivedCheque.BillNumber = oReceivedCheque.BillNumber == null ? "" : oReceivedCheque.BillNumber;
            oReceivedCheque.ProductDetails = oReceivedCheque.ProductDetails == null ? "" : oReceivedCheque.ProductDetails;
            try
            {
                _oReceivedCheque = oReceivedCheque;
                _oReceivedCheque = _oReceivedCheque.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oReceivedCheque = new ReceivedCheque();
                _oReceivedCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ReceivedCheque oReceivedCheque)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oReceivedCheque.Delete(oReceivedCheque.ReceivedChequeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public ActionResult PrintReceivedCheques(FormCollection DataCollection)
        {
            _oReceivedCheques = new List<ReceivedCheque>();
            _oReceivedCheques = new JavaScriptSerializer().Deserialize<List<ReceivedCheque>>(DataCollection["txtReceivedChequeCollectionList"]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            

            string Messge = "Received Cheque List";
            rptReceivedCheques oReport = new rptReceivedCheques();
            byte[] abytes = oReport.PrepareReport(_oReceivedCheques,oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        #endregion
        public ActionResult PrintReceivedChequesInXL()
        {
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ReceivedChequeXL>));

            //We load the data
            List<ReceivedCheque> oReceivedCheques = ReceivedCheque.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            int nCount = 0; double nTotal = 0;
            ReceivedChequeXL oReceivedChequeXL = new ReceivedChequeXL();
            List<ReceivedChequeXL> oReceivedChequeXLs = new List<ReceivedChequeXL>();
            foreach (ReceivedCheque oItem in oReceivedCheques)
            {
                nCount++;
                oReceivedChequeXL = new ReceivedChequeXL();
                oReceivedChequeXL.SLNo = nCount.ToString();
                oReceivedChequeXL.ChequeNo = oItem.ChequeNo;
                oReceivedChequeXL.ReceivedFrom = oItem.ContractorName;
                oReceivedChequeXL.IssueDate = oItem.IssueDateSt;
                oReceivedChequeXL.ChequeDate = oItem.ChequeDateSt;
                oReceivedChequeXL.Remarks = oItem.Remarks;
                oReceivedChequeXLs.Add(oReceivedChequeXL);
                nTotal = nTotal + nCount;
            }           

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oReceivedChequeXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Orders.xls");
        }
        
        [HttpPost]
        public JsonResult AdvSearch(ReceivedCheque oReceivedCheque)
        {
            List<ReceivedCheque> oReceivedCheques = new List<ReceivedCheque>();
            oReceivedCheque.ErrorMessage = (oReceivedCheque.ErrorMessage == null) ? "" : oReceivedCheque.ErrorMessage;
            string sSQL = this.GetSQL(oReceivedCheque.ErrorMessage);

            oReceivedCheques = ReceivedCheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oReceivedCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            //sMoneyReceiptNo + '~' + sBillNumber + '~' + sChequeNo + '~' + nReceiptDate + '~' + dReceiptStartDate + '~' + dReceiptEndDate + '~' + nChequeDate + '~' + dChequeStartDate + '~' + dChequeEndDate + '~' + _nContractorIDs;
            string sMoneyReceiptNo = sTemp.Split('~')[0];
            string sBillNumber = sTemp.Split('~')[1];
            string sChequeNo = sTemp.Split('~')[2];
            
            int nReceiptDate = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dReceiptStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dReceiptEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            int nChequeDate = Convert.ToInt32(sTemp.Split('~')[6]);
            DateTime dChequeStartDate = Convert.ToDateTime(sTemp.Split('~')[7]);
            DateTime dChequeEndDate = Convert.ToDateTime(sTemp.Split('~')[8]);
            string sContractorIDs = sTemp.Split('~')[9];

            string sReturn1 = "SELECT * FROM View_ReceivedCheque";
            string sReturn = "";

            #region MoneyReceiptNo
            if (sMoneyReceiptNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MoneyReceiptNo LIKE '%" + sMoneyReceiptNo + "%'";
            }
            #endregion

            #region BillNumber
            if (sBillNumber != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BillNumber LIKE '%" + sBillNumber + "%'";
            }
            #endregion

            #region ChequeNo
            if (sChequeNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChequeNo LIKE '%" + sChequeNo + "%'";
            }
            #endregion

            #region Customer
            if (sContractorIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (SELECT HH.ACCostCenterID FROM ACCostCenter AS HH WHERE HH.ReferenceType=1 AND ReferenceObjectID IN (" + sContractorIDs + "))";
            }

            #endregion

            #region MoneyReceiptDate
            if (nReceiptDate > 0)
            {
                if (nReceiptDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MoneyReceiptDate = '" + dReceiptStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nReceiptDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MoneyReceiptDate != '" + dReceiptStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nReceiptDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MoneyReceiptDate > '" + dReceiptStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nReceiptDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MoneyReceiptDate < '" + dReceiptStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nReceiptDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MoneyReceiptDate>= '" + dReceiptStartDate.ToString("dd MMM yyyy") + "' AND MoneyReceiptDate < '" + dReceiptEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nReceiptDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MoneyReceiptDate< '" + dReceiptStartDate.ToString("dd MMM yyyy") + "' OR MoneyReceiptDate > '" + dReceiptEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region ChequeDate
            if (nChequeDate > 0)
            {
                if (nChequeDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate = '" + dChequeStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nChequeDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate != '" + dChequeStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nChequeDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate > '" + dChequeStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nChequeDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate < '" + dChequeStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nChequeDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate>= '" + dChequeStartDate.ToString("dd MMM yyyy") + "' AND ChequeDate < '" + dChequeEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nChequeDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate< '" + dChequeStartDate.ToString("dd MMM yyyy") + "' OR ChequeDate > '" + dChequeEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }


        #region ReceivedCheque
        public PartialViewResult Attachment(int id, string ms, double ts)
        {
            ReceivedChequeAttachment oReceivedChequeAttachment = new ReceivedChequeAttachment();
            List<ReceivedChequeAttachment> oReceivedChequeAttachments = new List<ReceivedChequeAttachment>();
            oReceivedChequeAttachments = ReceivedChequeAttachment.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oReceivedChequeAttachment.ReceivedChequeID = id;
            oReceivedChequeAttachment.ReceivedChequeAttachments = oReceivedChequeAttachments;
            TempData["message"] = ms;
            return PartialView(oReceivedChequeAttachment);
        }

        [HttpPost]
        public ActionResult UploadAttchment(HttpPostedFileBase file, ReceivedChequeAttachment oReceivedChequeAttachment)
        {
            string sErrorMessage = "";
            byte[] data;
            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    double nMaxLength = 1024 * 1024;
                    if (data == null || data.Length <= 0)
                    {
                        sErrorMessage = "Please select an file!";
                    }
                    else if (data.Length > nMaxLength)
                    {
                        sErrorMessage = "You can select maximum 1MB file size!";
                    }
                    else if (oReceivedChequeAttachment.ReceivedChequeID <= 0)
                    {
                        sErrorMessage = "Your Selected Received Cheque Is Invalid!";
                    }
                    else
                    {
                        oReceivedChequeAttachment.AttatchFile = data;
                        oReceivedChequeAttachment.AttatchmentName = file.FileName;
                        oReceivedChequeAttachment.FileType = file.ContentType;
                        oReceivedChequeAttachment = oReceivedChequeAttachment.Save((int)Session[SessionInfo.currentUserID]);
                    }
                }
                else
                {
                    sErrorMessage = "Please select an file!";
                }


            }
            catch (Exception ex)
            {
                sErrorMessage = "";
                sErrorMessage = ex.Message;
            }
            return RedirectToAction("Attachment", new { id = oReceivedChequeAttachment.ReceivedChequeID, ms = sErrorMessage, ts = Convert.ToDouble(DateTime.Now.Millisecond) });
        }


        public ActionResult DownloadAttachment(int id, double ts)
        {
            ReceivedChequeAttachment oReceivedChequeAttachment = new ReceivedChequeAttachment();
            try
            {
                oReceivedChequeAttachment.ReceivedChequeAttachmentID = id;
                oReceivedChequeAttachment = ReceivedChequeAttachment.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oReceivedChequeAttachment.AttatchFile != null)
                {
                    var file = File(oReceivedChequeAttachment.AttatchFile, oReceivedChequeAttachment.FileType);
                    file.FileDownloadName = oReceivedChequeAttachment.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oReceivedChequeAttachment.AttatchmentName);
            }
        }

        [HttpPost]
        public JsonResult DeleteAttachment(ReceivedChequeAttachment oReceivedChequeAttachment)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oReceivedChequeAttachment.Delete(oReceivedChequeAttachment.ReceivedChequeAttachmentID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
