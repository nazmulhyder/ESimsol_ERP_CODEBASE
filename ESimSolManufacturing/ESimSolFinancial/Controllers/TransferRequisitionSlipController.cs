using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class TransferRequisitionSlipController : Controller
    {
        #region Declaration
        TransferRequisitionSlip _oTransferRequisitionSlip = new TransferRequisitionSlip();
        TransferRequisitionSlipDetail _oTransferRequisitionSlipDetail = new TransferRequisitionSlipDetail();
        List<TransferRequisitionSlipDetail> _oTransferRequisitionSlipDetails = new List<TransferRequisitionSlipDetail>();
        List<TransferRequisitionSlip> _oTransferRequisitionSlips = new List<TransferRequisitionSlip>();          
        #endregion

        #region Action
        public ActionResult View_TransferRequisitionSlips(int opt, int buid, int menuid)
        {
            //opt(Operation Type) = 0 means TransferRequisition Issue & sct = 1 means TransferRequisition Received
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TransferRequisitionSlip).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            #region Gets Data
            _oTransferRequisitionSlips = new List<TransferRequisitionSlip>();
            if (opt == 0)
            {
                string sSQL = "SELECT * FROM View_TransferRequisitionSlip AS HH WHERE HH.TransferStatus=" + ((int)EnumTransferStatus.Initialized).ToString();
                if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND HH.IssueBUID = " + buid;
                }
                sSQL += " ORDER BY TRSID ASC";
                _oTransferRequisitionSlips = TransferRequisitionSlip.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                string sSQL = "SELECT * FROM View_TransferRequisitionSlip AS HH WHERE HH.TransferStatus=" + ((int)EnumTransferStatus.Disburse).ToString();
                if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND HH.ReceivedBUID = " + buid;
                }
                sSQL += " ORDER BY TRSID ASC";
                _oTransferRequisitionSlips = TransferRequisitionSlip.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            #endregion

            #region Gets Business Unit & Requsition Type
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string sTempSQL = "SELECT * FROM View_BusinessUnit AS HH ORDER BY BusinessUnitID ASC";
            oBusinessUnits = BusinessUnit.Gets(sTempSQL, (int)Session[SessionInfo.currentUserID]);

            List<EnumObject> oRequisitionTypes = new List<EnumObject>();
            List<EnumObject> oTempRequisitionTypes = new List<EnumObject>();
            oTempRequisitionTypes = EnumObject.jGets(typeof(EnumRequisitionType));
            foreach (EnumObject oItem in oTempRequisitionTypes)
            {
                if ((EnumRequisitionType)oItem.id != EnumRequisitionType.None)
                {
                    oRequisitionTypes.Add(oItem);
                }
            }
            #endregion

            ViewBag.OPT = opt;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.RequisitionTypes = oRequisitionTypes;
            return View(_oTransferRequisitionSlips);
        }

        public ActionResult View_TransferRequisitionSlip(int id, int buid)
        {            
            _oTransferRequisitionSlip = new TransferRequisitionSlip();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            #region RequisitionType
            List<EnumObject> oRequisitionTypes = new List<EnumObject>();
            List<EnumObject> oTempRequisitionTypes = new List<EnumObject>();
            oTempRequisitionTypes  = EnumObject.jGets(typeof(EnumRequisitionType));
            foreach (EnumObject oItem in oTempRequisitionTypes)
            {
                if ((EnumRequisitionType)oItem.id == EnumRequisitionType.InternalTransfer)
                {
                    oRequisitionTypes.Add(oItem);
                }
                else if ((EnumRequisitionType)oItem.id == EnumRequisitionType.ExternalTransfer)
                {
                    oRequisitionTypes.Add(oItem);
                }
            }
            #endregion

            #region Received BusinessUnits
            List<BusinessUnit> oReceiveBusinessUnits = new List<BusinessUnit>();
            oReceiveBusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            #endregion

            if (id > 0)
            {
                _oTransferRequisitionSlip = _oTransferRequisitionSlip.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTransferRequisitionSlip.TransferRequisitionSlipDetails = TransferRequisitionSlipDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

                #region Issue Stores
                oIssueStores = new List<WorkingUnit>();
                oIssueStores = WorkingUnit.GetsPermittedStore(_oTransferRequisitionSlip.IssueBUID, EnumModuleName.TransferRequisitionSlip, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                #endregion

                #region Received Stores
                oReceivedStores = new List<WorkingUnit>();
                oReceivedStores = WorkingUnit.GetsPermittedStore(_oTransferRequisitionSlip.ReceivedBUID, EnumModuleName.TransferRequisitionSlip, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
                #endregion
            }
            else
            {
                BusinessUnit oIssueBusinessUnit = new BusinessUnit();
                oIssueBusinessUnit = oIssueBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

                _oTransferRequisitionSlip.IssueBUID = buid;
                _oTransferRequisitionSlip.IssueBUName = oIssueBusinessUnit.Name;

                #region Issue Stores
                oIssueStores = new List<WorkingUnit>();
                oIssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.TransferRequisitionSlip, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                #endregion

                #region Received Stores
                oReceivedStores = new List<WorkingUnit>();
                oReceivedStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.TransferRequisitionSlip, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
                #endregion
            }

            ViewBag.RequisitionTypes = oRequisitionTypes;
            ViewBag.ReceiveBusinessUnits = oReceiveBusinessUnits;
            ViewBag.IssueStores = oIssueStores;
            ViewBag.ReceivedStores = oReceivedStores;
            ViewBag.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oTransferRequisitionSlip);
        }
        #endregion

        #region Post Functions
        [HttpPost]
        public JsonResult GetsReceivedStore(BusinessUnit oBusinessUnit)
        {
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();            
            try
            {
                oReceivedStores = WorkingUnit.GetsPermittedStoreByStoreName(oBusinessUnit.BusinessUnitID, EnumModuleName.TransferRequisitionSlip, EnumStoreType.ReceiveStore,oBusinessUnit.Note, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oReceivedStores = new List<WorkingUnit>();
                WorkingUnit oWorkingUnit = new WorkingUnit();
                oWorkingUnit.ErrorMessage = ex.Message;
                oReceivedStores.Add(oWorkingUnit);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oReceivedStores);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            _oTransferRequisitionSlip = new TransferRequisitionSlip();
            try
            {
                _oTransferRequisitionSlip = oTransferRequisitionSlip.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTransferRequisitionSlip = new TransferRequisitionSlip();
                _oTransferRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oTransferRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            string sFeedBackMessage = "";
            try
            {                
                sFeedBackMessage = oTransferRequisitionSlip.Delete(oTransferRequisitionSlip.TRSID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            try
            {
                oTransferRequisitionSlip = oTransferRequisitionSlip.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //UndoApproved
        [HttpPost]
        public JsonResult UndoApproved(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            try
            {
                oTransferRequisitionSlip = oTransferRequisitionSlip.UndoApproved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Disburse(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            try
            {
                oTransferRequisitionSlip = oTransferRequisitionSlip.Disburse((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Received(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            try
            {
                oTransferRequisitionSlip = oTransferRequisitionSlip.Received((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchProducts(Lot oLot)
        {
            List<Product> oProducts = new List<Product>();
            Product oProduct = new Product();
            try
            {
                string sSQL = "SELECT * FROM View_Product";
                string sSQL1 = "";

                #region BUID
                if (oLot.BUID > 0)//if apply style configuration business unit
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID =" + oLot.BUID.ToString() + ")";
                }
                #endregion

                #region ProductName

                if (!string.IsNullOrEmpty(oLot.ProductName))
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductName LIKE '%" + oLot.ProductName + "%'";
                }
                #endregion

                #region Deafult
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " Activity=1";
                #endregion
                #region WorkingUnitID
                if (oLot.WorkingUnitID > 0) //Hare ProductID  Use as a StyleID
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID in (Select ProductID from Lot where Balance>0 and WorkingUnitID=" + oLot.WorkingUnitID + ")";
                }
                #endregion

                #region Style Wise Suggested Product
                if (oLot.ProductID > 0) //Hare ProductID  Use as a StyleID
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID IN (SELECT HH.ProductID FROM BillOfMaterial AS HH WHERE HH.TechnicalSheetID=" + oLot.ProductID.ToString() + ")";
                }
                #endregion

                sSQL = sSQL + sSQL1 + " Order By ProductName ASC";

                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oProducts.Count() <= 0) throw new Exception("No product found.");
            }
            catch (Exception ex)
            {
                oProducts = new List<Product>();
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult PickTransferRequisitionSlipDetails(BillOfMaterial oBillOfMaterial)
        {
            List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
            _oTransferRequisitionSlipDetail = new TransferRequisitionSlipDetail();
            try
            {
                oBillOfMaterials = BillOfMaterial.Gets(oBillOfMaterial.TechnicalSheetID,  (int)Session[SessionInfo.currentUserID]);
                foreach (BillOfMaterial oItem in oBillOfMaterials)
                {
                    _oTransferRequisitionSlipDetail = new TransferRequisitionSlipDetail();
                    _oTransferRequisitionSlipDetail.ProductID = oItem.ProductID;
                    _oTransferRequisitionSlipDetail.StyleID = oItem.TechnicalSheetID;
                    _oTransferRequisitionSlipDetail.ProductCode = oItem.ProductCode;
                    _oTransferRequisitionSlipDetail.ProductName = oItem.ProductName;
                    _oTransferRequisitionSlipDetail.ColorName = oItem.ColorName;
                    _oTransferRequisitionSlipDetail.SizeName = oItem.SizeName;
                    _oTransferRequisitionSlipDetail.StyleNo = oItem.StyleNo;
                    _oTransferRequisitionSlipDetails.Add(_oTransferRequisitionSlipDetail);
                }
            }
            catch (Exception ex)
            {
                _oTransferRequisitionSlipDetail = new TransferRequisitionSlipDetail();
                _oTransferRequisitionSlipDetail.ErrorMessage = ex.Message;
                _oTransferRequisitionSlipDetails.Add(_oTransferRequisitionSlipDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferRequisitionSlipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Searching
        private string GetSQL(string sTemp)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            int nTransferRequisitionSlipDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dTransferRequisitionSlipStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dTransferRequisitionSlipEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sTransferRequisitionSlipNo = sTemp.Split('~')[3];            
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[4]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[5]);
            int nRequsitionType = Convert.ToInt32(sTemp.Split('~')[6]);
            int nBusinessUnitID = Convert.ToInt32(sTemp.Split('~')[7]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[8]);
            int nOPT = Convert.ToInt32(sTemp.Split('~')[9]);
            

            string sReturn1 = "SELECT * FROM View_TransferRequisitionSlip";
            string sReturn = "";

            #region TransferRequisitionSlipNo
            if (sTransferRequisitionSlipNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TRSNO LIKE '%" + sTransferRequisitionSlipNo + "%'";
            }
            #endregion

            #region nRequsitionType
            if (nRequsitionType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionType = " + nRequsitionType.ToString();
            }
            #endregion

            #region BusinessUnit
            if (nBusinessUnitID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                if (nOPT == 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ReceivedBUID =" + nBusinessUnitID.ToString();

                    if (nBusinessUnitID != nBUID)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IssueBUID =" + nBUID.ToString();
                    }
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueBUID =" + nBusinessUnitID.ToString();

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ReceivedBUID =" + nBUID.ToString();
                }
            }
            #endregion

            #region BU
            if(nBusinessUnitID<=0 && nBUID>0)
            {
                if (nOPT == 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueBUID =" + nBUID.ToString();
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ReceivedBUID =" + nBUID.ToString();
                }
            }
            #endregion

            #region TransferRequisitionSlipDate
            if (nTransferRequisitionSlipDate > 0)
            {
                if (nTransferRequisitionSlipDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDateTime = '" + dTransferRequisitionSlipStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTransferRequisitionSlipDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDateTime != '" + dTransferRequisitionSlipStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTransferRequisitionSlipDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDateTime > '" + dTransferRequisitionSlipStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTransferRequisitionSlipDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDateTime < '" + dTransferRequisitionSlipStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nTransferRequisitionSlipDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDateTime>= '" + dTransferRequisitionSlipStartDate.ToString("dd MMM yyyy") + "' AND IssueDateTime < '" + dTransferRequisitionSlipEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nTransferRequisitionSlipDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDateTime< '" + dTransferRequisitionSlipStartDate.ToString("dd MMM yyyy") + "' OR IssueDateTime > '" + dTransferRequisitionSlipEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(AuthorisedByID,0) != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ISNULL(AuthorisedByID,0) = 0";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY TRSID ASC";
            return sReturn;
        }
        [HttpGet]
        public JsonResult TransferRequisitionSlipsAdvSearch(string Temp)
        {
            List<TransferRequisitionSlip> oTransferRequisitionSlips = new List<TransferRequisitionSlip>();
            try
            {
                string sSQL = GetSQL(Temp);
                oTransferRequisitionSlips = TransferRequisitionSlip.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTransferRequisitionSlips = new List<TransferRequisitionSlip>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferRequisitionSlips);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByTransferNo(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            List<TransferRequisitionSlip> oTransferRequisitionSlips = new List<TransferRequisitionSlip>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                if (oTransferRequisitionSlip.OPT == 0)
                {
                    string sSQL = "SELECT * FROM View_TransferRequisitionSlip AS HH WHERE  HH.TRSNO LIKE '%" + oTransferRequisitionSlip.TRSNO + "%' ";
                    if (oTransferRequisitionSlip.IssueBUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                    {
                        sSQL += " AND HH.IssueBUID = " + oTransferRequisitionSlip.IssueBUID;
                    }
                    sSQL += " ORDER BY TRSID ASC";
                    oTransferRequisitionSlips = TransferRequisitionSlip.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = "SELECT * FROM View_TransferRequisitionSlip AS HH WHERE HH.TRSNO LIKE '%" + oTransferRequisitionSlip.TRSNO + "%'";
                    if (oTransferRequisitionSlip.IssueBUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                    {
                        sSQL += " AND HH.ReceivedBUID = " + oTransferRequisitionSlip.IssueBUID;
                    }
                    sSQL += " ORDER BY TRSID ASC";
                    oTransferRequisitionSlips = TransferRequisitionSlip.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
               


            }
            catch (Exception ex)
            {
                oTransferRequisitionSlips = new List<TransferRequisitionSlip>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferRequisitionSlips);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Report
        [HttpPost]
        public ActionResult SetTransferRequisitionSlipListData(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oTransferRequisitionSlip);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferRequisitionSlipPrintPreview(int id)//, string copy
        {
            _oTransferRequisitionSlip = new TransferRequisitionSlip();                        
            BusinessUnit oBusinessUnit =  new BusinessUnit();
            _oTransferRequisitionSlip = _oTransferRequisitionSlip.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTransferRequisitionSlip.TransferRequisitionSlipDetails = TransferRequisitionSlipDetail.Gets(id,  (int)Session[SessionInfo.currentUserID]);

            _oTransferRequisitionSlip.BusinessUnit = oBusinessUnit.Get(_oTransferRequisitionSlip.IssueBUID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTransferRequisitionSlip.Company = oCompany;
            //List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            //oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.TransferRequisitionSlip + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.TransferRequisitionSlipPreview, (int)Session[SessionInfo.currentUserID]);

            rptTransferRequisitionSlip oReport = new rptTransferRequisitionSlip();
            byte[] abytes = oReport.PrepareReport(_oTransferRequisitionSlip, oSignatureSetups, "");// copy
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintTransferRequisitionSlips()
        {
            _oTransferRequisitionSlip = new TransferRequisitionSlip();
            try
            {
                _oTransferRequisitionSlip = (TransferRequisitionSlip)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_TransferRequisitionSlip WHERE TRSID IN (" + _oTransferRequisitionSlip.Param + ") Order By TRSID";
                _oTransferRequisitionSlip.TransferRequisitionSlips = TransferRequisitionSlip.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTransferRequisitionSlip = new TransferRequisitionSlip();
                _oTransferRequisitionSlips = new List<TransferRequisitionSlip>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTransferRequisitionSlip.Company = oCompany;

            string Messge = "Internal Challan List";
            rptTransferRequisitionSlips oReport = new rptTransferRequisitionSlips();
            byte[] abytes = oReport.PrepareReport(_oTransferRequisitionSlip, Messge);
            return File(abytes, "application/pdf");
        }

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

        #endregion

        #region UpdateVoucherEffect

        [HttpPost]
        public JsonResult UpdateVoucherEffect(TransferRequisitionSlip oTransferRequisitionSlip)
        {
            try
            {
                oTransferRequisitionSlip = oTransferRequisitionSlip.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}


