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

namespace ESimSolFinancial.Controllers
{
    public class LCTransferController : Controller
    {

        #region Declartion
        LCTransfer _oLCTransfer = new LCTransfer();
        List<LCTransfer> _oLCTransfers = new List<LCTransfer>();
        LCTransferDetail _oLCTransferDetail = new LCTransferDetail();
        List<LCTransferDetail> _oLCTransferDetails = new List<LCTransferDetail>();
        MasterLC _oMasterLC = new MasterLC();
        

        #endregion

        #region function
        private bool HaveRateViewPermission(EnumRoleOperationType OperationType)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            if ((int)Session[SessionInfo.currentUserID] == -9)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < oAuthorizationRoleMappings.Count; i++)
                {
                    if (oAuthorizationRoleMappings[i].OperationType == OperationType && oAuthorizationRoleMappings[i].ModuleNameST == "LCTransfer")
                    {
                        return true;

                    }

                }
            }

            return false;
        }
        #endregion

        #region Add, Edit, Delete

        #region View LC Transfer
        public ActionResult ViewLCTransferMgt(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString() + "," + ((int)EnumModuleName.CommercialInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oLCTransfers = new List<LCTransfer>();
            return View(_oLCTransfers);
        }

        public ActionResult ViewLCWiseTransfers(int id)//master lc id
        {
            _oMasterLC = new MasterLC();
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString() + "," + ((int)EnumModuleName.CommercialInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oLCTransfers = new List<LCTransfer>();
            _oLCTransfers = LCTransfer.Gets(id,(int)Session[SessionInfo.currentUserID]);
            ViewBag.MasterLC = _oMasterLC.Get(id, (int)Session[SessionInfo.currentUserID]);
            return View(_oLCTransfers);
        }
        #endregion

        #region View LC Transfer
        public ActionResult ViewLCTransfer(int nMasterLCID,  int nLCTransferID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oLCTransfer = new LCTransfer();
            _oMasterLC = new MasterLC();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            if (nLCTransferID > 0)
            {
                _oLCTransfer = _oLCTransfer.Get(nLCTransferID, (int)Session[SessionInfo.currentUserID]);
                _oLCTransfer.LCTransferDetails = LCTransferDetail.Gets(nLCTransferID, (int)Session[SessionInfo.currentUserID]);
                
            }
            else
            {
                _oMasterLC = _oMasterLC.Get(nMasterLCID, (int)Session[SessionInfo.currentUserID]);
                _oLCTransfer.MasterLCID = _oMasterLC.MasterLCID;
                _oLCTransfer.MasterLCNo = _oMasterLC.MasterLCNo;
                _oLCTransfer.LCStatus = _oMasterLC.LCStatus;
                _oLCTransfer.BuyerID = _oMasterLC.Applicant;
                _oLCTransfer.BuyerName = _oMasterLC.ApplicantName;
                _oLCTransfer.CommissionFavorOf = _oMasterLC.Beneficiary;
                _oLCTransfer.CommissionAccountID = _oMasterLC.AdviceBankID;
                _oLCTransfer.LCValue = _oMasterLC.LCValue;
                _oLCTransfer.YetToTransferValue = _oMasterLC.YetToTransferValue;
                _oLCTransfer.Note = _oMasterLC.Remark;
            }            
            _oLCTransfer.Companies = Company.Gets((int)Session[SessionInfo.currentUserID]);
            //_oLCTransfer.BankAccounts = BankAccount.GetsByCategory(true, (int)Session[SessionInfo.currentUserID]); //true for RT Own Accounts 
            _oLCTransfer.BankBranches = BankBranch.Gets((int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.ClientOperationSetting = oClientOperationSetting.Get(1,(int)Session[SessionInfo.currentUserID]);

            return View(_oLCTransfer);
        }
        #endregion

        #region View LC Transfer
        public ActionResult ViewLCTransferLog(int id, double ts)//id is Log ID
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oLCTransfer = new LCTransfer();
            if (id > 0)
            {
                _oLCTransfer = _oLCTransfer.GetLog(id, (int)Session[SessionInfo.currentUserID]);
                _oLCTransfer.LCTransferDetails = LCTransferDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oLCTransfer.Companies = Company.Gets((int)Session[SessionInfo.currentUserID]);
            //_oLCTransfer.BankAccounts = BankAccount.GetsByCategory(true, (int)Session[SessionInfo.currentUserID]); //true for RT Own Accounts 
            _oLCTransfer.BankBranches = BankBranch.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oLCTransfer);
        }
        #endregion

        #region View LC Transfer Revise
        public ActionResult ViewLCTransferRevise(int nLCTransferID, double ts)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oLCTransfer = new LCTransfer();
            _oMasterLC = new MasterLC();
            if (nLCTransferID > 0)
            {
                _oLCTransfer = _oLCTransfer.Get(nLCTransferID, (int)Session[SessionInfo.currentUserID]);
                _oLCTransfer.LCTransferDetails = LCTransferDetail.Gets(nLCTransferID, (int)Session[SessionInfo.currentUserID]);

            }
            _oLCTransfer.Companies = Company.Gets((int)Session[SessionInfo.currentUserID]);
            //_oLCTransfer.BankAccounts = BankAccount.GetsByCategory(true, (int)Session[SessionInfo.currentUserID]); //true for RT Own Accounts 
            _oLCTransfer.BankBranches = BankBranch.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oLCTransfer);
        }
        public ActionResult LCTransferReviseHistory(int id)//LC Transfer ID
        {
            _oLCTransfers = new List<LCTransfer>();
            string sSQL = "SELECT * FROM View_LCTransferLog WHERE LCTransferID  = "+id;
            _oLCTransfers = LCTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oLCTransfers);
        }
        #endregion

        #region UpdateLCTransferNoDate

        public ActionResult UpdateLCTransferNoDate(int nLCTransferID, double ts)
        {
            _oLCTransfer = new LCTransfer();
            if (nLCTransferID > 0)
            {
                _oLCTransfer = _oLCTransfer.Get(nLCTransferID, (int)Session[SessionInfo.currentUserID]);
                _oLCTransfer.LCTransferDetails = LCTransferDetail.Gets(nLCTransferID, (int)Session[SessionInfo.currentUserID]);
            }

            _oLCTransfer.Companies = Company.Gets((int)Session[SessionInfo.currentUserID]);
            //_oLCTransfer.BankAccounts = BankAccount.GetsByCategory(true, (int)Session[SessionInfo.currentUserID]); //true for RT Own Accounts 

            return PartialView(_oLCTransfer);
        }
        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(LCTransfer oLCTransfer)
        {
            _oLCTransfer = new LCTransfer();
            _oLCTransfer = oLCTransfer;
            try
            {
                 if (oLCTransfer.ActionTypeExtra == "Approve")
                {

                    _oLCTransfer.LCTranferActionType = EnumLCTranferActionType.Approve;

                }
                else if (oLCTransfer.ActionTypeExtra == "Req_for_Revise")
                {

                    _oLCTransfer.LCTranferActionType = EnumLCTranferActionType.Req_for_Revise;
                }
                _oLCTransfer.Note = oLCTransfer.Note;
                _oLCTransfer = SetLCTransferStatus(_oLCTransfer);
                _oLCTransfer = _oLCTransfer.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTransfer);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private LCTransfer SetLCTransferStatus(LCTransfer oLCTransfer)//Set EnumOrderStatus Value
        {
            switch (oLCTransfer.LCTransferStatusInInt)
            {
                case 1:
                    {
                        oLCTransfer.LCTransferStatus = EnumLCTransferStatus.Initialize;
                        break;
                    }
                case 2:
                    {
                        oLCTransfer.LCTransferStatus = EnumLCTransferStatus.Approve;
                        break;
                    }
                case 3:
                    {
                        oLCTransfer.LCTransferStatus = EnumLCTransferStatus.Request_for_Revise;
                        break;
                    }

            }

            return oLCTransfer;
        }
        #endregion
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(LCTransfer oLCTransfer)
        {
            _oLCTransfer = new LCTransfer();
            try
            {
               
               _oLCTransfer = oLCTransfer.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTransfer);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Commit Revise
        [HttpPost]
        public JsonResult CommitRevise(LCTransfer oLCTransfer)
        {
            _oLCTransfer = new LCTransfer();
            try
            {
                _oLCTransfer = oLCTransfer.AcceptLCTransferRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTransfer);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP UpDateTransferNoDate
        [HttpPost]
        public JsonResult UpDateTransferNoDate(LCTransfer oLCTransfer)
        {
            _oLCTransfer = new LCTransfer();
            try
            {

                _oLCTransfer = oLCTransfer.UpdateTransferNoDate((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTransfer);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Post Delete
        [HttpPost]
        public JsonResult Delete(LCTransfer oLCTransfer)
        {
            string smessage = "";
            try
            {
                smessage = oLCTransfer.Delete(oLCTransfer.LCTransferID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Wait for Revise
        [HttpGet]
        public JsonResult WaitForRevise(double ts)
        {
            string sSQL = "SELECT * FROM View_LCTransfer  Where LCTransferStatus = " + (int)EnumLCTransferStatus.Request_for_Revise;
            try
            {
                _oLCTransfers = new List<LCTransfer>();
                _oLCTransfers = LCTransfer.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
                _oLCTransfers.Add(_oLCTransfer);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTransfers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region LC Transfer Detail Picker
        public ActionResult LCTranferDetailPicker(int id, double ts) // id is LC Transfer ID
        {
            _oLCTransferDetail = new LCTransferDetail();
            string sSQL = "SELECT * FROM View_LCTransferDetail WHERE LCTransferID = " + id + " AND YetToInvoiceQty >0";
            _oLCTransferDetail.LCTransferDetails = LCTransferDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oLCTransferDetail);
        }

     

        #endregion



        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch()
        {

            LCTransfer oLCTransfer = new LCTransfer();
            oLCTransfer.BussinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            return PartialView(oLCTransfer);
        }

        #region HttpGet For Search
        [HttpGet]
        public JsonResult Gets(string sTemp)
        {
            List<LCTransfer> oLCTransfers = new List<LCTransfer>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oLCTransfers = LCTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLCTransfers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            //Issue Date
            int nTrasferCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dTransferStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dTransferEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLCNo = sTemp.Split('~')[3];
            string sTransferNo = sTemp.Split('~')[4];
            string sBuyerIDs = sTemp.Split('~')[5];
            string sFactoryIDs = sTemp.Split('~')[6];
            int nBusinessSessionID = Convert.ToInt32(sTemp.Split('~')[7]);
           
            string sReturn1 = "SELECT * FROM View_LCTransfer";
            string sReturn = "";

            #region LC No

            if (sLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MasterLCID = (SELECT top 1 MasterLCID FROM MasterLC WHERE MasterLCNo ='"+sLCNo+"')";

            }
            #endregion

            #region File No
            if (sTransferNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo = '" + sTransferNo + "'";
            }
            #endregion

            #region Buyer wise

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Factory Wise

            if (sFactoryIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionFactoryID IN (" + sFactoryIDs + ")";
            }
            #endregion


            #region Session  Wise

            if (nBusinessSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCTransferID IN (SELECT LCTransferID FROM LCTransferDetail WHERE OrderRecapID IN (SELECT SaleOrderID FROM SaleOrder WHERE BusinessSessionID = "+nBusinessSessionID+"))";
            }
            #endregion

            #region Transfer Date Wise
            if (nTrasferCreateDateCom > 0)
            {
                if (nTrasferCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TransferIssueDate = '" + dTransferStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTrasferCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TransferIssueDate != '" + dTransferStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTrasferCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TransferIssueDate > '" + dTransferStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTrasferCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TransferIssueDate < '" + dTransferStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTrasferCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TransferIssueDate>= '" + dTransferStartDate.ToString("dd MMM yyyy") + "' AND TransferIssueDate < '" + dTransferEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nTrasferCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TransferIssueDate< '" + dTransferStartDate.ToString("dd MMM yyyy") + "' OR TransferIssueDate > '" + dTransferEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion



        #endregion

        #region Printing
        public ActionResult PrintLCTransferList(string sIDs)
        {
            _oLCTransfer = new LCTransfer();
            string sSQL = "SELECT * FROM View_LCTransfer WHERE LCTransferID IN (" + sIDs + ")";
            _oLCTransfer.LCTransfers = LCTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLCTransferList oReport = new rptLCTransferList();
            byte[] abytes = oReport.PrepareReport(_oLCTransfer, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintLCTransferPreview(int id)
        {
            _oLCTransfer = new LCTransfer();
            _oMasterLC = new MasterLC();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            _oLCTransfer = _oLCTransfer.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.LCTransferDetails = LCTransferDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.LCTransfer_PreviewFormat, (int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.IsPrintWithPadding, (int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.MasterLC = _oMasterLC.Get(_oLCTransfer.MasterLCID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(_oLCTransfer.CommissionFavorOf, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo =GetCompanyLogo(oCompany);

            rptLCTransfer oReport = new rptLCTransfer();
            byte[] abytes ;
            if (Convert.ToInt16(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_1)
            {
                 abytes = oReport.PrepareReportFormat1(_oLCTransfer, oCompany);
            }
            else if (Convert.ToInt16(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_2)
            {
                rptLCTransferPreviewFormat2 oReportFormat2 = new rptLCTransferPreviewFormat2();
                abytes = oReportFormat2.PrepareReport(_oLCTransfer, oCompany);
            }
            else 
            {
                abytes = oReport.PrepareReport(_oLCTransfer, oCompany);
            }
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintLCTransferForwarder(int id)
        {
            _oLCTransfer = new LCTransfer();
            _oLCTransfer = _oLCTransfer.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.LCTransferDetails = LCTransferDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            _oLCTransfer.ClientOperationSetting = oClientOperationSetting.Get(1, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(_oLCTransfer.CommissionFavorOf, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);

            rptLCTransferForwarder oReport = new rptLCTransferForwarder();
            byte[] abytes = oReport.PrepareReport(_oLCTransfer, oCompany);
            return File(abytes, "application/pdf");
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public ActionResult PrintLCTransferLogPreview(int id)//Log ID
        {
            _oLCTransfer = new LCTransfer();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            _oLCTransfer = _oLCTransfer.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.LCTransferDetails = LCTransferDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.LCTransfer_PreviewFormat, (int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsPrintWithPadding, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(_oLCTransfer.CommissionFavorOf, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);

            rptLCTransfer oReport = new rptLCTransfer();
            byte[] abytes;
            if (Convert.ToInt16(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_1)
            {
                abytes = oReport.PrepareReportFormat1(_oLCTransfer, oCompany);
            }
            else
            {
                abytes = oReport.PrepareReport(_oLCTransfer, oCompany);
            }
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Report
        public ActionResult LCTransferReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLCTransfer = new LCTransfer();
            _oLCTransfer.ReportLayouts = ReportLayout.Gets(EnumModuleName.LCTransfer, (int)Session[SessionInfo.currentUserID]);
            _oLCTransfer.BussinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            return View(_oLCTransfer);
        }
        [HttpGet]
        public JsonResult GetsBySession(int nBSessionID)
        {
            _oLCTransfers = new List<LCTransfer>();
            try
            {
                string sSQL = "SELECT * FROM View_LCTransfer WHERE LCTransferID IN (SELECT LCTransferID FROM LCTransferDetail WHERE OrderRecapID IN (SELECT SaleOrderID FROM SaleOrder WHERE BusinessSessionID = " + nBSessionID + "))";
                _oLCTransfers = LCTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCTransfer = new LCTransfer();
                _oLCTransfer.ErrorMessage = ex.Message;
                _oLCTransfers.Add(_oLCTransfer);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTransfers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        public ActionResult MISReports(string Param)
        {
            int  nBusinessSessionID = Convert.ToInt32(Param.Split('~')[0]);
            string sBSessionName = Param.Split('~')[1];
            int nReportType = Convert.ToInt32(Param.Split('~')[2]);
            string sReportNo = Param.Split('~')[3];
            _oLCTransfers = new List<LCTransfer>();
            List<LCTransfer> oTempLCTransfers = new List<LCTransfer>();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string sSQL = "SELECT * FROM View_LCTransfer WHERE LCTransferID IN (SELECT LCTransferID FROM LCTransferDetail WHERE OrderRecapID IN (SELECT SaleOrderID FROM SaleOrder WHERE BusinessSessionID = " + nBusinessSessionID + "))";
            oTempLCTransfers = LCTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            
            switch (nReportType)
            {

                case (int)EnumReportLayout.PartyWise:
                    {
                        sSQL += "Order By BuyerID , MasterLCID,ProductionFactoryID";
                        _oLCTransfers = LCTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        rptBuyerWiseLCTransfer oReport = new rptBuyerWiseLCTransfer();
                        byte[] abytes = oReport.PrepareReport(_oLCTransfers, oCompany, sBSessionName, sReportNo);
                        return File(abytes, "application/pdf");
                    };

                case (int)EnumReportLayout.Factorywise:
                    {
                        List<FactoryWiseLCTransferReport> oFactoryWiseLCTransferReports = new List<FactoryWiseLCTransferReport>();
                        string sLCTransferIDs = string.Join(",", oTempLCTransfers.Select(x=>x.LCTransferID));
                        oFactoryWiseLCTransferReports = FactoryWiseLCTransferReport.Gets(sLCTransferIDs, (int)Session[SessionInfo.currentUserID]);
                        if (oFactoryWiseLCTransferReports.Count> 0)
                        {
                            rptFactoryWiseLCTransfer oReport = new rptFactoryWiseLCTransfer();
                            byte[] abytes = oReport.PrepareReport(oFactoryWiseLCTransferReports, oCompany, sBSessionName, sReportNo);
                            return File(abytes, "application/pdf");
                        }
                        else
                        {
                            return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Data" });
                        }
                    };
               
                default:
                    return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Report" });
            }

        }
    }
}
