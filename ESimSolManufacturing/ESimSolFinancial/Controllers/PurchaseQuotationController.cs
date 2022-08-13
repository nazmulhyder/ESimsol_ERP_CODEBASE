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
using System.Drawing.Printing;


namespace ESimSolFinancial.Controllers
{

    public class PurchaseQuotationController : Controller
    {
        #region Declaration
        PurchaseQuotation _oPurchaseQuotation = new PurchaseQuotation();
        List<PurchaseQuotation> _oPurchaseQuotations = new List<PurchaseQuotation>();
        List<PQTermsAndCondition> _oPQTermsAndConditions = new List<PQTermsAndCondition>();
        PurchaseQuotationDetail _oPurchaseQuotationDetail = new PurchaseQuotationDetail();
        List<PurchaseQuotationDetail> _oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();

        List<Product> _oProducts = new List<Product>();
        Product _oProduct = new Product();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult ViewPurchaseQuotations(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PurchaseQuotation).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseQuotations = new List<PurchaseQuotation>();
            string sSQL = "";
            if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL = "SELECT * FROM View_PurchaseQuotation  WHERE  QuotationStatus = " + (int)EnumQuotationStatus.Initialize + " AND   Isnull(BUID,0)= "+buid+" ORDER BY RateCollectDate";
            }
            else
            {
                sSQL = "SELECT * FROM View_PurchaseQuotation  WHERE  QuotationStatus = " + (int)EnumQuotationStatus.Initialize;    
            }
            _oPurchaseQuotations = PurchaseQuotation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Statuss = EnumObject.jGets(typeof(EnumQuotationStatus));
            _oPurchaseQuotations = _oPurchaseQuotations.OrderByDescending(x => x.RateCollectDate).ThenByDescending(x=>x.PurchaseQuotationID ).ToList();
            ViewBag.BUID = buid;
            ViewBag.ApproveByUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM Users WHERE UserID IN (SELECT ApprovedBy FROM PurchaseQuotation)", (int)Session[SessionInfo.currentUserID]);
            return View(_oPurchaseQuotations);
        }
        public ActionResult ViewPurchaseQuotation(int id, double ts)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            List<Currency> oCurrencys = new List<Currency>();
            Company oCompany = new Company();
            if (id > 0)
            {
                _oPurchaseQuotation = _oPurchaseQuotation.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseQuotation.PurchaseQuotationDetails = PurchaseQuotationDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseQuotation.PQTermsAndConditions = PQTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(_oPurchaseQuotation);
        }
        public ActionResult ViewPurchaseQuotationRevise(int id, double ts)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            List<Currency> oCurrencys = new List<Currency>();
            Company oCompany = new Company();
            if (id > 0)
            {
                _oPurchaseQuotation = _oPurchaseQuotation.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseQuotation.PurchaseQuotationDetails = PurchaseQuotationDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseQuotation.PQTermsAndConditions = PQTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(_oPurchaseQuotation);
        }

        public ActionResult ViewPurchaseQuotationReviseHistory(int id)
        {
            _oPurchaseQuotations = new List<PurchaseQuotation>();
            _oPurchaseQuotations = PurchaseQuotation.GetsByLog("SELECT * FROM View_PurchaseQuotationLog WHERE PurchaseQuotationID="+id, (int)Session[SessionInfo.currentUserID]);

            ViewBag.Statuss = EnumObject.jGets(typeof(EnumQuotationStatus));
            _oPurchaseQuotations = _oPurchaseQuotations.OrderByDescending(x => x.RateCollectDate).ThenByDescending(x => x.PurchaseQuotationID).ToList();

            return View(_oPurchaseQuotations);
        }
        public ActionResult ViewPurchaseQuotationApprove(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
           // this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'PurchaseQuotation'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oPurchaseQuotations = new List<PurchaseQuotation>();
            
            return View(_oPurchaseQuotations);
        }

        public ActionResult ViewPurchaseQuotationView(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser("'PurchaseQuotation'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oPurchaseQuotations = new List<PurchaseQuotation>();

            return View(_oPurchaseQuotations);
        }
     
        [HttpPost]
        public JsonResult Save(PurchaseQuotation oPurchaseQuotation)
        {
            try
            {
                _oPurchaseQuotation = oPurchaseQuotation.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RequestRevise(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            try
            {
                _oPurchaseQuotation = oPurchaseQuotation;

                _oPurchaseQuotation = _oPurchaseQuotation.RequestQuotationRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveRevise(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            oPurchaseQuotation.Note = oPurchaseQuotation.Note == null ? "" : oPurchaseQuotation.Note;
            try
            {
                _oPurchaseQuotation = oPurchaseQuotation;
                _oPurchaseQuotation = _oPurchaseQuotation.AcceptRevise(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Copy(PurchaseQuotation oPurchaseQuotation)
        {
            try
            {
                _oPurchaseQuotation = oPurchaseQuotation;
                _oPurchaseQuotation.ApprovedBy = 0;
                _oPurchaseQuotation.QuotationStatusInInt = 0;
                _oPurchaseQuotation.PurchaseQuotationDetails = PurchaseQuotationDetail.Gets(oPurchaseQuotation.PurchaseQuotationID, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseQuotation.PurchaseQuotationID = 0;
                foreach(PurchaseQuotationDetail oitem in _oPurchaseQuotation.PurchaseQuotationDetails)
                {
                    oitem.PurchaseQuotationDetailID = 0;
                    oitem.PurchaseQuotationID = 0;
                }
                _oPurchaseQuotation = _oPurchaseQuotation.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(PurchaseQuotation oPurchaseQuotation)
        {
            string sFeedBackMessage = "";
            try
            {
                
                sFeedBackMessage = oPurchaseQuotation.Delete(oPurchaseQuotation.PurchaseQuotationID, (int)Session[SessionInfo.currentUserID]);
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
        [HttpPost]
        public JsonResult GetPurchaseQuotation(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            try
            {
                _oPurchaseQuotation = _oPurchaseQuotation.Get(oPurchaseQuotation.PurchaseQuotationID, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseQuotation.PurchaseQuotationDetails = PurchaseQuotationDetail.Gets(oPurchaseQuotation.PurchaseQuotationID, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseQuotation.SCPersons = ContactPersonnel.GetsByContractor(_oPurchaseQuotation.SupplierID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //GetNonExpiredPQs
        [HttpPost]
        public JsonResult GetNonExpiredPQs(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotations = new List<PurchaseQuotation>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseQuotation WHERE ISNULL(ApprovedBy,0)!=0 AND ExpiredDate > '" + DateTime.Today + "'";
                if (oPurchaseQuotation.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oPurchaseQuotation.BUID;
                }
                if (oPurchaseQuotation.PurchaseQuotationNo != null && oPurchaseQuotation.PurchaseQuotationNo != "")
                {
                    sSQL += " AND PurchaseQuotationNo LIKE '%" + oPurchaseQuotation.PurchaseQuotationNo + "%'";
                }
                string sSQL1 = "SELECT * FROM View_PurchaseQuotation WHERE  PurchaseQuotationID IN (SELECT PurchaseQuotationID FROM PurchaseQuotationDetail WHERE ProductID IN(SELECT ProductID FROM NOADetail WHERE NOAID = "+oPurchaseQuotation.ErrorMessage+")) ORDER BY ExpiredDate DESC";
                
                if (oPurchaseQuotation.PurchaseQuotationNo != null && oPurchaseQuotation.PurchaseQuotationNo != "")
                {
                    _oPurchaseQuotations = PurchaseQuotation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oPurchaseQuotations = PurchaseQuotation.Gets(sSQL1, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
                _oPurchaseQuotations.Add(_oPurchaseQuotation);
            }

            _oPurchaseQuotations = _oPurchaseQuotations.OrderByDescending(x => x.RateCollectDate).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTermsAndCondition(int nPurchaseQuotationID, int nSupplierID)
        {
            List<PQTermsAndCondition> _oPQTermsAndConditions = new List<PQTermsAndCondition>();
            PQTermsAndCondition oPQTermsAndCondition = new PQTermsAndCondition();
            try
            {
                _oPQTermsAndConditions = PQTermsAndCondition.Gets("SELECT * FROM PQTermsAndCondition WHERE PurchaseQuotationID = (SELECT TOP 1 PurchaseQuotationID FROM PurchaseQuotation WHERE SupplierID=" + nSupplierID + " AND PurchaseQuotationID != "+nPurchaseQuotationID+" ORDER BY DBServerDateTime DESC)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPQTermsAndCondition = new PQTermsAndCondition();
                oPQTermsAndCondition.ErrorMessage = ex.Message;
                _oPQTermsAndConditions.Add(oPQTermsAndCondition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPQTermsAndConditions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.PurchaseQuotation, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.PurchaseQuotation, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);

            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;

        }


        [HttpPost]
        public JsonResult SendToMgt(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            try
            {
                _oPurchaseQuotation = _oPurchaseQuotation.SendToMgt(oPurchaseQuotation.PurchaseQuotationID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
         public JsonResult GetWaitForApproval(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseQuotationDetail WHERE QuotationStatus IN("+(int)EnumQuotationStatus.WaitForApproved+","+(int)EnumQuotationStatus.Initialize+")";
                if(oPurchaseQuotation.Parameter!=null )
                {
                    sSQL += " AND SupplierID IN (" + oPurchaseQuotation.Parameter + ")";
                }
                sSQL += " Order By SupplierID, ProductID";
                _oPurchaseQuotationDetails = PurchaseQuotationDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotationDetail = new PurchaseQuotationDetail();
                _oPurchaseQuotationDetail.ErrorMessage = ex.Message;
                _oPurchaseQuotationDetails.Add(_oPurchaseQuotationDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotationDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
         public JsonResult GetPRDetailsForQuotation(PurchaseRequisitionDetail oPurchaseRequisitionDetail)
         {
             List<PurchaseRequisitionDetail> oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
             List<PurchaseRequisitionDetail> _oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
             try
             {
                 string sSQL = "SELECT * FROM View_PurchaseRequisitionDetail WHERE PRDetailID  IN(" + oPurchaseRequisitionDetail.Remarks + ")";
                 _oPurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 if (_oPurchaseRequisitionDetails.Any())
                 {
                     var tempPRDsSpecExist = _oPurchaseRequisitionDetails.Where(x => x.IsSpecExist == true).ToList();
                     var tempPRDsSpecnotExist = _oPurchaseRequisitionDetails.Where(x => x.IsSpecExist == false).ToList();
                     if (tempPRDsSpecnotExist.Any())
                     {
                         _oPurchaseRequisitionDetails = tempPRDsSpecnotExist.GroupBy(x => new { x.ProductID }, (key, grp) =>
                         new PurchaseRequisitionDetail
                         {
                             MUnitID = grp.First().MUnitID,
                             ProductID = key.ProductID,
                             Qty = grp.Sum(p => p.Qty),
                             ProductCode = grp.First().ProductCode,
                             ProductName = grp.First().ProductName,
                             UnitName = grp.First().UnitName,
                             ProductSpec = grp.First().ProductSpec,
                             UnitSymbol = grp.First().UnitSymbol,
                             IsSpecExist = grp.First().IsSpecExist,
                             PRDetailID = grp.First().PRDetailID
                        }).ToList();
                         foreach (PurchaseRequisitionDetail oItem in tempPRDsSpecExist)
                         {
                             oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                             oPurchaseRequisitionDetail.PRDetailID = oItem.PRDetailID;
                             oPurchaseRequisitionDetail.ProductCode = oItem.ProductCode;
                             oPurchaseRequisitionDetail.ProductID = oItem.ProductID;
                             oPurchaseRequisitionDetail.ProductName = oItem.ProductName;
                             oPurchaseRequisitionDetail.ProductSpec = oItem.ProductSpec;
                             oPurchaseRequisitionDetail.Qty = oItem.Qty;
                             oPurchaseRequisitionDetail.MUnitID = oItem.MUnitID;
                             oPurchaseRequisitionDetail.UnitName = oItem.UnitName;
                             oPurchaseRequisitionDetail.IsSpecExist = oItem.IsSpecExist;
                             oPurchaseRequisitionDetail.PRDetailID = oItem.PRDetailID;
                             _oPurchaseRequisitionDetails.Add(oPurchaseRequisitionDetail);

                         }
                     }

                   
                     _oPurchaseRequisitionDetails.OrderBy(x => x.ProductID);
                 }

             }
             catch (Exception ex)
             {
                 oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                 oPurchaseRequisitionDetail.ErrorMessage = ex.Message;
                 _oPurchaseRequisitionDetails.Add(oPurchaseRequisitionDetail);
             }



             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oPurchaseRequisitionDetails);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
        public JsonResult Approve(PurchaseQuotation oPurchaseQuotation)
         {
             _oPurchaseQuotation = new PurchaseQuotation();
             try
             {
                 _oPurchaseQuotation = oPurchaseQuotation.Approve( (int)Session[SessionInfo.currentUserID]);
             }
             catch (Exception ex)
             {
                 _oPurchaseQuotation = new PurchaseQuotation();
                 _oPurchaseQuotation.ErrorMessage = ex.Message;
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oPurchaseQuotation);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        #region Advance Search
      

        #region HttpGet For Search
        [HttpPost]
         public JsonResult SearchForView(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            try
            {
                string sSQL = GetSQL(oPurchaseQuotation.Parameter);
                _oPurchaseQuotationDetails = PurchaseQuotationDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotationDetail = new PurchaseQuotationDetail();
                _oPurchaseQuotationDetail.ErrorMessage = ex.Message;
                _oPurchaseQuotationDetails.Add(_oPurchaseQuotationDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotationDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
           
            
            int nCboIssueDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sSupplierIDs = sTemp.Split('~')[3];
            string sProductIDs = sTemp.Split('~')[4];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[5]);

            string sReturn1 = "SELECT * FROM View_PurchaseQuotationDetail";
            string sReturn = "";

            #region Supplier Wise
            if (sSupplierIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SupplierID IN (" + sSupplierIDs + ")";
            }
            #endregion

            #region Product Wise
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID IN (" + sProductIDs + ")";
            }
            #endregion

            #region Date Wise
            
            if (nCboIssueDate > 0)
            {
                if (nCboIssueDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboIssueDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate != '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboIssueDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboIssueDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboIssueDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate>= '" + dIssueStartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dIssueEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCboIssueDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate< '" + dIssueStartDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dIssueEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
             #endregion

            #region Business unit Wise
            if (nBUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            sReturn = sReturn1 + sReturn + "AND ISNULL(ApprovedBy,0)!=0  ORDER BY PurchaseQuotationID, SupplierID, ProductID";
            return sReturn;
        }
        #endregion

        #endregion
        public ActionResult PurchaseQuotationPreview(int id)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            Contractor oContractor = new Contractor();
            Company oCompany = new Company();
            _oPurchaseQuotation = _oPurchaseQuotation.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseQuotation.PurchaseQuotationDetails = PurchaseQuotationDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseQuotation.PQTermsAndConditions = PQTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            

            if (_oPurchaseQuotation.PurchaseQuotationDetails.Any())
            {
                string sSQL = "SELECT * FROM View_PQSpec WHERE PQDetailID  IN(" + string.Join(",", _oPurchaseQuotation.PurchaseQuotationDetails.Select(x => x.PurchaseQuotationDetailID)) + ")";
                _oPurchaseQuotation.oPQSpec = PQSpec.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                _oPurchaseQuotation.Contractors = oContractor.Get(_oPurchaseQuotation.SupplierID, (int)Session[SessionInfo.currentUserID]);
                foreach (PurchaseQuotationDetail PQD in _oPurchaseQuotation.PurchaseQuotationDetails)
                {
                    PurchaseRequisition oPurchaseRequisition = new PurchaseRequisition();
                    oPurchaseRequisition = PurchaseRequisition.Gets("SELECT * FROM View_PurchaseRequisition WHERE PRID IN (SELECT PRID FROM PurchaseRequisitionDetail WHERE PRDetailID =" + PQD.PRDetailID + ")", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                    if (oPurchaseRequisition != null)
                    {
                        PQD.PRNo = oPurchaseRequisition.PRNo;
                    }
                }
            }


            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oPurchaseQuotation.Company = oCompany;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPurchaseQuotation.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPurchaseQuotation oReport = new rptPurchaseQuotation();
            byte[] abytes = oReport.PrepareReport(_oPurchaseQuotation, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PurchaseQuotationPreviewLog(int id)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            Contractor oContractor = new Contractor();
            Company oCompany = new Company();
            _oPurchaseQuotation = _oPurchaseQuotation.GetByLog(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseQuotation.PurchaseQuotationDetails = PurchaseQuotationDetail.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseQuotation.PurchaseQuotationDetails.ForEach(x =>
            {
                x.PurchaseQuotationDetailID = x.PurchaseQuotationDetailLogID;
            });
            
            _oPurchaseQuotation.PQTermsAndConditions = PQTermsAndCondition.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            if (_oPurchaseQuotation.PurchaseQuotationDetails.Any())
            {
                string sSQL = "SELECT * FROM View_PQSpecLog WHERE PQDetailLogID  IN(" + string.Join(",", _oPurchaseQuotation.PurchaseQuotationDetails.Select(x => x.PurchaseQuotationDetailLogID)) + ")";
                _oPurchaseQuotation.oPQSpec = PQSpec.GetsByLog(sSQL, (int)Session[SessionInfo.currentUserID]);

                _oPurchaseQuotation.oPQSpec.ForEach(x =>
                {
                    x.PQDetailID = x.PQDetailLogID;
                });

                _oPurchaseQuotation.Contractors = oContractor.Get(_oPurchaseQuotation.SupplierID, (int)Session[SessionInfo.currentUserID]);

            }


            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oPurchaseQuotation.Company = oCompany;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPurchaseQuotation.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPurchaseQuotation oReport = new rptPurchaseQuotation();
            byte[] abytes = oReport.PrepareReport(_oPurchaseQuotation, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintPurchaseQuotations(string sParam)
        {
            _oPurchaseQuotation = new PurchaseQuotation();
            string sSQL = "SELECT * FROM View_PurchaseQuotation WHERE PurchaseQuotationID IN (" + sParam + ")";
            _oPurchaseQuotation.PurchaseQuotations = PurchaseQuotation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oPurchaseQuotation.Company = oCompany;


            string Messge = "Purchase Quotation List";
            rptPurchaseQuotations oReport = new rptPurchaseQuotations();
            byte[] abytes = oReport.PrepareReport(_oPurchaseQuotation, Messge);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintPQDetails(string sParam)
        {
            _oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            string sSQL = "SELECT * FROM View_PurchaseQuotationDetail WHERE PurchaseQuotationDetailID IN (" + sParam + ") Order By PurchaseQuotationID, SupplierID,ProductID";
            _oPurchaseQuotationDetails = PurchaseQuotationDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string Messge = "Approve Purchase Item List";
            rptPQItems oReport = new rptPQItems();
            byte[] abytes = oReport.PrepareReport(_oPurchaseQuotationDetails, oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        #region PurchaseQuotationService PurchaseQuotationAttachment
        public ActionResult LoadAttachments(PurchaseQuotation oPurchaseQuotation)
        {
            PurchaseQuotationAttachment oPurchaseQuotationAttachment = new PurchaseQuotationAttachment();
            List<PurchaseQuotationAttachment> oPurchaseQuotationAttachments = new List<PurchaseQuotationAttachment>();
            oPurchaseQuotationAttachments = PurchaseQuotationAttachment.Gets(oPurchaseQuotation.PurchaseQuotationID, (int)Session[SessionInfo.currentUserID]);
           // oPurchaseQuotationAttachment.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
            //oPurchaseQuotationAttachment.PurchaseQuotationAttachments = oPurchaseQuotationAttachments;
            //TempData["message"] = ms;
            //return PartialView(oPurchaseQuotationAttachment);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseQuotationAttachments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadAttchment(HttpPostedFileBase file, PurchaseQuotationAttachment oPurchaseQuotationAttachment)
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
                    else if (oPurchaseQuotationAttachment.PurchaseQuotationID <= 0)
                    {
                        sErrorMessage = "Your Selected Purchase Order Is Invalid!";
                    }
                    else
                    {
                        oPurchaseQuotationAttachment.AttatchFile = data;
                        oPurchaseQuotationAttachment.AttatchmentName = file.FileName;
                        oPurchaseQuotationAttachment.FileType = file.ContentType;
                        oPurchaseQuotationAttachment = oPurchaseQuotationAttachment.Save((int)Session[SessionInfo.currentUserID]);
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
            return RedirectToAction("Attachment", new { id = oPurchaseQuotationAttachment.PurchaseQuotationID, ms = sErrorMessage, ts = Convert.ToDouble(DateTime.Now.Millisecond) });
        }

        public ActionResult DownloadAttachment(int id, double ts)
        {
            PurchaseQuotationAttachment oPurchaseQuotationAttachment = new PurchaseQuotationAttachment();
            try
            {
                oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID = id;
                oPurchaseQuotationAttachment = PurchaseQuotationAttachment.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oPurchaseQuotationAttachment.AttatchFile != null)
                {
                    var file = File(oPurchaseQuotationAttachment.AttatchFile, oPurchaseQuotationAttachment.FileType);
                    file.FileDownloadName = oPurchaseQuotationAttachment.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oPurchaseQuotationAttachment.AttatchmentName);
            }
        }
        [HttpPost]
        public JsonResult DeleteAttachment(PurchaseQuotationAttachment oPurchaseQuotationAttachment)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oPurchaseQuotationAttachment.Delete(oPurchaseQuotationAttachment.PurchaseQuotationAttachmentID, (int)Session[SessionInfo.currentUserID]);
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

        [HttpPost]
        public JsonResult SearchByQuotationNo(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotations = new List<PurchaseQuotation>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseQuotation WHERE PurchaseQuotationNo LIKE '%" + oPurchaseQuotation.PurchaseQuotationNo + "%'";
                if (oPurchaseQuotation.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oPurchaseQuotation.BUID;
                }
                sSQL += " ORDER BY PurchaseQuotationID";
                _oPurchaseQuotations = PurchaseQuotation.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
                _oPurchaseQuotations.Add(_oPurchaseQuotation);
            }

            //_oPurchaseQuotations = _oPurchaseQuotations.OrderByDescending(x => x.RateCollectDate).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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

        #region SpecificationHead
        [HttpPost]
        public JsonResult ProductSpecHeadByProduct(PQSpec oPQSpec)
        {
            ProductSpecHead _oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> _oSProductSpecHeads = new List<ProductSpecHead>();
            PQSpec _oPQSpec = new PQSpec();
            List<PQSpec> oPQSpecs = new List<PQSpec>();
            string sSQL = string.Empty;
            try
            {
                sSQL = "Select * from View_PQSpec Where PQDetailID =" + oPQSpec.PQDetailID;
                oPQSpecs = PQSpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from View_ProductSpecHead Where ProductID =" + oPQSpec.ProductID + "Order By Sequence";
                _oSProductSpecHeads = ProductSpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                if (_oSProductSpecHeads.Any())
                {
                    if (oPQSpecs.Any())
                    {
                        _oSProductSpecHeads.RemoveAll(x => oPQSpecs.Select(p => p.SpecHeadID).Contains(x.SpecHeadID));

                    }
                    if (_oSProductSpecHeads.Any())
                    {
                        foreach (var oitem in _oSProductSpecHeads)
                        {
                            _oPQSpec = new PQSpec();
                            _oPQSpec.SpecName = oitem.SpecName;
                            _oPQSpec.PQSpecID = 0;
                            _oPQSpec.SpecHeadID = oitem.SpecHeadID;
                            _oPQSpec.PQDescription = string.Empty;
                            _oPQSpec.PQDetailID = oPQSpec.PQDetailID;
                            oPQSpecs.Add(_oPQSpec);

                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                _oProductSpecHead = new ProductSpecHead();
                _oProductSpecHead.ErrorMessage = ex.Message;
                _oSProductSpecHeads.Add(_oProductSpecHead);
            }

            var jsonResult = Json(oPQSpecs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult IUDPQSpec(PQSpec oPQSpec)
        {
            try
            {
                oPQSpec = oPQSpec.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPQSpec = new PQSpec();
                oPQSpec.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPQSpec);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region AdvanceSearch
        [HttpPost]
        public JsonResult PurchaseQuotationSearch(PurchaseQuotation oPurchaseQuotation)
        {
            _oPurchaseQuotations = new List<PurchaseQuotation>();
            try
            {
                string sSQL = GetSQLAdvSrc(oPurchaseQuotation.Parameter);
                _oPurchaseQuotations = PurchaseQuotation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseQuotation = new PurchaseQuotation();
                _oPurchaseQuotation.ErrorMessage = ex.Message;
                _oPurchaseQuotations.Add(_oPurchaseQuotation);
            }
            var jSonResult = Json(_oPurchaseQuotations, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        private string GetSQLAdvSrc(string sTemp)
        {
            string sQuotationNo = sTemp.Split('~')[0];
            int nIssueDate = Convert.ToInt32(sTemp.Split('~')[1]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            DateTime dIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[3]);
            string sSupplierName = sTemp.Split('~')[4];
            int nStatus = Convert.ToInt32(sTemp.Split('~')[5]);

            string sProductIDs = sTemp.Split('~')[6];
            int nApproveBy = Convert.ToInt32(sTemp.Split('~')[7]);
            string sMPRNo = sTemp.Split('~')[8];
            

            string sReturn1 = "SELECT * FROM View_PurchaseQuotation";
            string sReturn = "";

            #region Quotation No
            if (sQuotationNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseQuotationNo ='" + sQuotationNo + "'";
            }
            #endregion

            #region Issue Date Wise

            if (nIssueDate > 0)
            {
                if (nIssueDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RateCollectDate = '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RateCollectDate != '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RateCollectDate > '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RateCollectDate < '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RateCollectDate>= '" + dIssueStartDate.ToString("dd MMM yyyy") + "' AND RateCollectDate < '" + dIssueEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RateCollectDate< '" + dIssueStartDate.ToString("dd MMM yyyy") + "' OR RateCollectDate > '" + dIssueEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            if (sSupplierName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SupplierName LIKE'%" + sSupplierName + "%'";
            }

            //Status
            if (nStatus >= 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " QuotationStatus = " + nStatus;
            }

            if (nApproveBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy = " + nApproveBy;
            }
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseQuotationID IN (SELECT PurchaseQuotationID FROM PurchaseQuotationDetail WHERE ProductID IN (" + sProductIDs + "))";
            }
            if (!string.IsNullOrEmpty(sMPRNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseQuotationID IN (SELECT PurchaseQuotationID FROM PurchaseQuotationDetail WHERE PRDetailID IN (SELECT PRDetailID FROM PurchaseRequisitionDetail WHERE PRID IN (SELECT PRID FROM PurchaseRequisition WHERE PRNo LIKE '%" + sMPRNo + "%')))";
            }

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
    }
    

}