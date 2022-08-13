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
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class KnittingOrderController : Controller
    {
        #region Declaration

        KnittingOrder _oKnittingOrder = new KnittingOrder();
        List<KnittingOrder> _oKnittingOrders = new List<KnittingOrder>();
        KnittingOrderDetail _oKnittingOrderDetail = new KnittingOrderDetail();
        List<KnittingOrderDetail> _oKnittingOrderDetails = new List<KnittingOrderDetail>();
        List<KnittingOrderRegister> _oKnittingOrderRegisters = new List<KnittingOrderRegister>();
        KnittingOrderRegister _oKnittingOrderRegister = new KnittingOrderRegister();
        #endregion

        #region Actions

        public ActionResult ViewKnittingOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.KnittingOrder).ToString() + "," + ((int)EnumModuleName.KnittingYarnChallan).ToString() + "," + ((int)EnumModuleName.KnittingFabricReceive).ToString() + "," + ((int)EnumModuleName.KnittingYarnReturn).ToString() + "," + ((int)EnumModuleName.KnittingComposition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_KnittingOrder AS HH WHERE  ISNULL(HH.BUID,0) = " + buid.ToString() + " AND ISNULL(HH.ApprovedBy,0) = 0 ORDER BY HH.KnittingOrderID ASC";
            _oKnittingOrders = new List<KnittingOrder>();
            _oKnittingOrders = KnittingOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oKnittingOrders);
        }

        public ActionResult ViewKnittingOrder(int id, int buid)
        {
            _oKnittingOrder = new KnittingOrder();
            if (id > 0)
            {
                _oKnittingOrder = _oKnittingOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oKnittingOrder.KnittingOrderDetails = KnittingOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oKnittingOrder.KnittingOrderTermsAndConditions = KnittingOrderTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oKnittingOrder.BUID = buid;

            //List<EnumObject> oTemp = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Hanger || x.id == (int)EnumProductNature.Poly || x.id == (int)EnumProductNature.Cone || x.id == (int)EnumProductNature.Sizer).ToList();
            //ViewBag.EnumType = oTemp;
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            //ViewBag.Units = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC", (int)Session[SessionInfo.currentUserID]);
            List<BusinessSession> oBusinessSessions = new List<BusinessSession>();
            oBusinessSessions.Add(new BusinessSession { BusinessSessionID=0, SessionName="Session" });
            oBusinessSessions.AddRange( BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]));
            ViewBag.BusinessSessions =oBusinessSessions;
            ViewBag.EnumKnittingOrderTypes = EnumObject.jGets(typeof(EnumKnittingOrderType));
            ViewBag.MUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            ViewBag.MWeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            return View(_oKnittingOrder);
        }

        [HttpPost]
        public JsonResult Save(KnittingOrder oKnittingOrder)
        {
            _oKnittingOrder = new KnittingOrder();
            try
            {
                _oKnittingOrder = oKnittingOrder.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnittingOrder = new KnittingOrder();
                _oKnittingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(KnittingOrder oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                KnittingOrder oKnittingOrder = new KnittingOrder();
                sFeedBackMessage = oKnittingOrder.Delete(oJB.KnittingOrderID, (int)Session[SessionInfo.currentUserID]);
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

        #region Functions
        #endregion

        #region Advance search 
        [HttpPost]
        public JsonResult GetsByOrderNo(KnittingOrder oKnittingOrder)
        {
            List<KnittingOrder> oKnittingOrders = new List<KnittingOrder>();
            try
            {
                string sSQL = "SELECT * FROM view_KnittingOrder WHERE OrderNo LIKE '%" + oKnittingOrder.OrderNo + "%' AND BUID = " + oKnittingOrder.BUID;
                oKnittingOrders = KnittingOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnittingOrders = new List<KnittingOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByStyleNo(KnittingOrder oKnittingOrder)
        {
            List<KnittingOrder> oKnittingOrders = new List<KnittingOrder>();
            try
            {
                string sSQL = "SELECT * FROM view_KnittingOrder WHERE KnittingOrderID IN (SELECT KnittingOrderID FROM View_KnittingOrderDetail WHERE StyleNo LIKE '%" + oKnittingOrder.Params + "%') AND BUID = " + oKnittingOrder.BUID;
                oKnittingOrders = KnittingOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnittingOrders = new List<KnittingOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<KnittingOrder> oKnittingOrders = new List<KnittingOrder>();
            try
            {
                string sSQL = GetSQL(Temp);
                oKnittingOrders = KnittingOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnittingOrders = new List<KnittingOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         private string GetSQL(string sTemp)
        {
            string sKnittingOrderNo = sTemp.Split('~')[0];
            int nOrderDate = Convert.ToInt32(sTemp.Split('~')[1]);
            DateTime dOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[2]);
            DateTime dOrderDateEnd = Convert.ToDateTime(sTemp.Split('~')[3]);
            int nStartDate = Convert.ToInt32(sTemp.Split('~')[4]);
            DateTime dStartDateStart = Convert.ToDateTime(sTemp.Split('~')[5]);
            DateTime dStartDateEnd = Convert.ToDateTime(sTemp.Split('~')[6]);
            int nCompleteDate = Convert.ToInt32(sTemp.Split('~')[7]);
            DateTime dCompleteDateStart = Convert.ToDateTime(sTemp.Split('~')[8]);
            DateTime dCompleteDateEnd = Convert.ToDateTime(sTemp.Split('~')[9]);
            int nFactoryID = Convert.ToInt32(sTemp.Split('~')[10]);
            int nBuyerID = Convert.ToInt32(sTemp.Split('~')[11]);
            int nBusinessSessionID = Convert.ToInt32(sTemp.Split('~')[12]);

            int nStyleID = Convert.ToInt32(sTemp.Split('~')[13]);
            int nFabricID = Convert.ToInt32(sTemp.Split('~')[14]);
            int nYarnID = Convert.ToInt32(sTemp.Split('~')[15]);

            string sMICDia = sTemp.Split('~')[16];
            string sFinishDia = sTemp.Split('~')[17];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[18]);

            string sReturn1 = "SELECT * FROM view_KnittingOrder";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sKnittingOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderNo LIKE '%" + sKnittingOrderNo + "%' ";
            }

            #region order date
            if (nOrderDate > 0)
            {
                if (nOrderDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate = '" + dOrderDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106))>= CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),OrderDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dOrderDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106))< 'CONVERT(Date,Convert(Varchar(12)," + dOrderDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),OrderDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dOrderDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region  Start Date

            if (nStartDate > 0)
            {
                if (nStartDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) = CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) >= CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),StartDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dStartDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region  Complete Date

            if (nCompleteDate > 0)
            {
                if (nCompleteDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) = CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) >= CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),ApproxCompleteDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            if (nFactoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FactoryID = " + nFactoryID;
            }

            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }

            if (nBuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE StyleID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE BuyerID = " + nBuyerID + "))";
            }

            if (nBusinessSessionID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nBusinessSessionID;
            }
            if (nStyleID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE StyleID = " + nStyleID + " )";
            }
            if (nFabricID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE FabricID = " + nFabricID + " )";
            }
            if (nYarnID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingYarn WHERE YarnID = " + nYarnID + " )";
            }
            if (!string.IsNullOrEmpty(sMICDia))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE MICDia LIKE '%" + sMICDia + "%' )";
            }
            if (!string.IsNullOrEmpty(sFinishDia))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE FinishDia LIKE '%" + sFinishDia + "%' )";
            }

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Get

        public JsonResult GetYarnCategory(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    //oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnittingOrder, EnumProductUsages.Yarn, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oProducts = Product.Gets("SELECT * FROM View_Product AS HH WHERE HH.Activity = 1 AND HH.ProductName LIKE '%" + oProduct.ProductName + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }   
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnittingOrder, EnumProductUsages.Yarn, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFabricCategory(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnittingOrder, EnumProductUsages.Fabric, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnittingOrder, EnumProductUsages.Fabric, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetsDetailsByID(KnittingOrderDetail oKnittingOrderDetail)//Id=ContractorID
        //{
        //    try
        //    {
        //        string Ssql = "SELECT*FROM View_KnittingOrderDetail WHERE KnittingOrderID=" + oKnittingOrderDetail.KnittingOrderID + " ";
        //        _oKnittingOrderDetails = new List<KnittingOrderDetail>();
        //        _oKnittingOrderDetail.KnittingOrderDetails = KnittingOrderDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oKnittingOrderDetail.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oKnittingOrderDetail);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Approve & Finish
        [HttpPost]
        public JsonResult Approve(KnittingOrder oKnittingOrder)
        {
            _oKnittingOrder = new KnittingOrder();
            try
            {
                _oKnittingOrder = oKnittingOrder.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnittingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UnApprove(KnittingOrder oKnittingOrder)
        {
            _oKnittingOrder = new KnittingOrder();
            try
            {
                _oKnittingOrder = oKnittingOrder.UnApprove((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnittingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FinishYarnChallan(KnittingOrder oKnittingOrder)
        {
            _oKnittingOrder = new KnittingOrder();
            try
            {
                _oKnittingOrder = oKnittingOrder.FinishYarnChallan((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnittingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FinishFabricReceive(KnittingOrder oKnittingOrder)
        {
            _oKnittingOrder = new KnittingOrder();
            try
            {
                _oKnittingOrder = oKnittingOrder.FinishFabricReceive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnittingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region print

        public ActionResult KnittingOrderPrintPreview(int id)
        {
            BusinessUnit oBU = new BusinessUnit();
            _oKnittingOrder = new KnittingOrder();            
            if (id > 0)
            {
                _oKnittingOrder = _oKnittingOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oKnittingOrder.KnittingOrderDetails = KnittingOrderDetail.Gets(_oKnittingOrder.KnittingOrderID, (int)Session[SessionInfo.currentUserID]);
                _oKnittingOrder.KnittingOrderTermsAndConditions = KnittingOrderTermsAndCondition.Gets(_oKnittingOrder.KnittingOrderID, (int)Session[SessionInfo.currentUserID]);
                _oKnittingOrder.KnittingYarnChallanDetails = KnittingYarnChallanDetail.Gets("SELECT * FROM View_KnittingYarnChallanDetail WHERE KnittingYarnChallanID IN (SELECT KnittingYarnChallanID FROM View_KnittingYarnChallan WHERE KnittingOrderID = " + _oKnittingOrder.KnittingOrderID + ")", (int)Session[SessionInfo.currentUserID]);
            }

            //List<ApprovalHead> oApprovalHead = new List<ApprovalHead>();
            //string ssql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.KnittingOrder + " AND BUID = " + _oKnittingOrder.BUID + "  Order By Sequence";
            //oApprovalHead = ApprovalHead.Gets(ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //List<ApprovalHistory> oApprovalHistorys = new List<ApprovalHistory>();
            //string sSql = "SELECT * FROM View_ApprovalHistory WHERE ModuleID = " + (int)EnumModuleName.KnittingOrder + " AND BUID = " + _oKnittingOrder.BUID + "  AND  ObjectRefID = " + id + " Order BY ApprovalSequence";
            //oApprovalHistorys = ApprovalHistory.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.KnittingOrder, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oKnittingOrder.BUID > 0)
            {
                oBU = oBU.Get(_oKnittingOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            List<KnittingComposition> oKnittingCompositions = new List<KnittingComposition>();
            oKnittingCompositions = KnittingComposition.Gets("SELECT * FROM View_KnittingComposition WHERE KnittingOrderDetailID IN(SELECT KnittingOrderDetailID FROM KnittingOrderDetail WHERE KnittingOrderID = " + _oKnittingOrder.KnittingOrderID + ")", (int)Session[SessionInfo.currentUserID]);
            byte[] abytes;
            rptKnittingOrder oReport = new rptKnittingOrder();
            abytes = oReport.PrepareReport(_oKnittingOrder, oKnittingCompositions, oCompany, oBU, oSignatureSetups);//oApprovalHead, oApprovalHistorys
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

        [HttpPost]
        public ActionResult SetKnittingOrderListData(KnittingOrder oKnittingOrder)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oKnittingOrder);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintKnittingOrders()
        {
            _oKnittingOrder = new KnittingOrder();
            try
            {
                _oKnittingOrder = (KnittingOrder)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_KnittingOrder WHERE KnittingOrderID IN (" + _oKnittingOrder.ErrorMessage + ") Order By KnittingOrderID";
                _oKnittingOrders = KnittingOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnittingOrder = new KnittingOrder();
                _oKnittingOrders = new List<KnittingOrder>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oKnittingOrder.Company = oCompany;

            rptKnittingOrders oReport = new rptKnittingOrders();
            byte[] abytes = oReport.PrepareReport(_oKnittingOrders, oCompany);
            return File(abytes, "application/pdf");
        }

        
        #endregion

        #region KnittingComposition

        [HttpPost]
        public JsonResult GetsDetailsByKnitOrderID(KnittingOrder oKnittingOrder)
        {
            try
            {
                string Ssql = "SELECT*FROM View_KnittingOrderDetail WHERE KnittingOrderID=" + oKnittingOrder.KnittingOrderID + " ";
                _oKnittingOrderDetails = new List<KnittingOrderDetail>();
                _oKnittingOrderDetails = KnittingOrderDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oKnittingOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsKnittingCompositionByDetail(KnittingOrderDetail oKnittingOrderDetail)
        {
            string sSQL = string.Empty;
            List<KnittingComposition> oKnittingCompositions = new List<KnittingComposition>();
            try
            {
               
                oKnittingOrderDetail = oKnittingOrderDetail.Get(oKnittingOrderDetail.KnittingOrderDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oKnittingOrderDetail.KnittingOrderID > 0)
                {
                    sSQL = "SELECT * FROM View_KnittingYarn WHERE KnittingOrderID=" + oKnittingOrderDetail.KnittingOrderID;
                    sSQL = "SELECT * FROM View_KnittingComposition WHERE KnittingOrderDetailID = " + oKnittingOrderDetail.KnittingOrderDetailID;
                    oKnittingCompositions = KnittingComposition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                
            }
            catch (Exception ex)
            {
                oKnittingCompositions = new List<KnittingComposition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingCompositions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveKnittingComposition(KnittingComposition oKnittingComposition)
        {

            try
            {   oKnittingComposition = oKnittingComposition.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
                if (oKnittingComposition.KnittingCompositions.Any())
                {
                    string sSQL = "SELECT * FROM View_KnittingYarn WHERE KnittingOrderID=" + oKnittingComposition.KnittingCompositions.FirstOrDefault().KnittingOrderID;
                    oKnittingOrderDetail = oKnittingOrderDetail.Get(oKnittingComposition.KnittingCompositions.FirstOrDefault().KnittingOrderDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //if (oUnsavedYarns.Any())
                    //{
                    //    foreach (var oitem in oUnsavedYarns)
                    //    {
                    //        oKnittingComposition.KnittingCompositions.Add(new KnittingComposition
                    //        {
                    //            KnittingCompositionID = 0,
                    //            KnittingOrderDetailID = oKnittingComposition.KnittingCompositions.FirstOrDefault().KnittingOrderDetailID,
                    //            FabricID = oKnittingComposition.KnittingCompositions.FirstOrDefault().FabricID,
                    //            YarnID = oitem.YarnID,
                    //            RatioInPercent = 0,
                    //            YarnCode = oitem.YarnCode,
                    //            YarnName = oitem.YarnName,
                    //            FabricCode = oKnittingOrderDetail.FabricCode,
                    //            FabricName = oKnittingOrderDetail.FabricName,

                    //        });
                    //    }
                    //}
                    oKnittingComposition.KnittingCompositions.OrderByDescending(x=>x.RatioInPercent);
                }
               

            }
            catch (Exception ex)
            {
                oKnittingComposition = new KnittingComposition();
                oKnittingComposition.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingComposition);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #endregion

        #region Knitting order register
        public ActionResult ViewKnittingOrderRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oKnittingOrder = new KnittingOrder();
            _oKnittingOrder.BUID = buid;

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.Order_Status_Wise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.Style_Wise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            return View(_oKnittingOrder);
        }

        public ActionResult SetSessionSearchCriteria(KnittingOrderRegister oKnittingOrderRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oKnittingOrderRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintKnittingOrderRegister(double ts)
        {
            List<KnittingOrderRegister> _oKnittingOrderRegisters = new List<KnittingOrderRegister>();
            KnittingOrderRegister oKnittingOrderRegister = new KnittingOrderRegister();
            string _sErrorMesage = "";
            try
            {
                _oKnittingOrderRegisters = new List<KnittingOrderRegister>();
                oKnittingOrderRegister = (KnittingOrderRegister)Session[SessionInfo.ParamObj];
                string SQL = "";
                string sSQL = this.GetSQLForRegister(oKnittingOrderRegister.ErrorMessage);
                if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.Order_Status_Wise)
                {
                    SQL = "SELECT KnittingOrderID FROM view_KnittingOrder ";
                    sSQL = SQL + sSQL;
                    _oKnittingOrderRegisters = KnittingOrderRegister.GetsForOrderStatusWise(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.DateWise)
                {
                    SQL = "SELECT * FROM View_KnittingOrderRegister ";
                    sSQL = SQL + sSQL + "ORDER BY OrderDate, KnittingOrderID, KnittingOrderDetailID, StyleID ASC";
                    _oKnittingOrderRegisters = KnittingOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    SQL = "SELECT * FROM View_KnittingOrderRegister ";
                    sSQL = SQL + sSQL + "ORDER BY FactoryID, KnittingOrderID, KnittingOrderDetailID, StyleID ASC";
                    _oKnittingOrderRegisters = KnittingOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.Style_Wise)
                {
                    SQL = "SELECT * FROM View_KnittingOrderRegister ";
                    sSQL = SQL + sSQL + "ORDER BY StyleID, KnittingOrderID, KnittingOrderDetailID ASC";
                    _oKnittingOrderRegisters = KnittingOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                
                if (_oKnittingOrderRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oKnittingOrderRegisters = new List<KnittingOrderRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oKnittingOrderRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oKnittingOrderRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptKnittingOrderRegisters oReport = new rptKnittingOrderRegisters();
                byte[] abytes = oReport.PrepareReport(_oKnittingOrderRegisters, oCompany, oKnittingOrderRegister.ReportLayout, "");
                return File(abytes, "application/pdf");
                //return null;
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        private string GetSQLForRegister(string sTemp)
        {
            string sKnittingOrderNo = sTemp.Split('~')[0];
            int nOrderDate = Convert.ToInt32(sTemp.Split('~')[1]);
            DateTime dOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[2]);
            DateTime dOrderDateEnd = Convert.ToDateTime(sTemp.Split('~')[3]);
            int nStartDate = Convert.ToInt32(sTemp.Split('~')[4]);
            DateTime dStartDateStart = Convert.ToDateTime(sTemp.Split('~')[5]);
            DateTime dStartDateEnd = Convert.ToDateTime(sTemp.Split('~')[6]);
            int nCompleteDate = Convert.ToInt32(sTemp.Split('~')[7]);
            DateTime dCompleteDateStart = Convert.ToDateTime(sTemp.Split('~')[8]);
            DateTime dCompleteDateEnd = Convert.ToDateTime(sTemp.Split('~')[9]);
            int nFactoryID = Convert.ToInt32(sTemp.Split('~')[10]);
            int nBuyerID = Convert.ToInt32(sTemp.Split('~')[11]);
            int nBusinessSessionID = Convert.ToInt32(sTemp.Split('~')[12]);

            int nStyleID = Convert.ToInt32(sTemp.Split('~')[13]);
            int nFabricID = Convert.ToInt32(sTemp.Split('~')[14]);
            int nYarnID = Convert.ToInt32(sTemp.Split('~')[15]);

            string sMICDia = sTemp.Split('~')[16];
            string sFinishDia = sTemp.Split('~')[17];
            string sGSM = sTemp.Split('~')[18];

            string sReturn1 = "";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sKnittingOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderNo LIKE '%" + sKnittingOrderNo + "%' ";
            }

            #region order date
            if (nOrderDate > 0)
            {
                if (nOrderDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate = '" + dOrderDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106))>= CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),OrderDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dOrderDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106))< 'CONVERT(Date,Convert(Varchar(12)," + dOrderDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),OrderDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dOrderDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region  Start Date

            if (nStartDate > 0)
            {
                if (nStartDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) = CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) >= CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),StartDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dStartDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region  Complete Date

            if (nCompleteDate > 0)
            {
                if (nCompleteDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) = CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) >= CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),ApproxCompleteDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            if (nFactoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FactoryID = " + nFactoryID;
            }

            if (nBuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE StyleID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE BuyerID = " + nBuyerID + "))";
            }

            if (nBusinessSessionID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nBusinessSessionID;
            }
            if (nStyleID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE StyleID = " + nStyleID + " )";
            }
            if (nFabricID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE FabricID = " + nFabricID + " )";
            }
            if (nYarnID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingYarn WHERE YarnID = " + nYarnID + " )";
            }
            if (!string.IsNullOrEmpty(sMICDia))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE MICDia LIKE '%" + sMICDia + "%' )";
            }
            if (!string.IsNullOrEmpty(sFinishDia))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM KnittingOrderDetail WHERE FinishDia LIKE '%" + sFinishDia + "%' )";
            }
            if (!string.IsNullOrEmpty(sGSM))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnittingOrderID IN (SELECT KnittingOrderID FROM View_KnittingOrderDetail WHERE GSM LIKE '%" + sGSM + "%' )";
            }
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        public void ExportToExcelKnittingOrder()
        {
            KnittingOrderRegister oKnittingOrderRegister = new KnittingOrderRegister();
            string _sErrorMesage = "";
            try
            {
                _oKnittingOrderRegisters = new List<KnittingOrderRegister>();
                oKnittingOrderRegister = (KnittingOrderRegister)Session[SessionInfo.ParamObj];
                string SQL = "";
                string sSQL = this.GetSQLForRegister(oKnittingOrderRegister.ErrorMessage);
                if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.Order_Status_Wise)
                {
                    SQL = "SELECT KnittingOrderID FROM view_KnittingOrder ";
                    sSQL = SQL + sSQL;
                    _oKnittingOrderRegisters = KnittingOrderRegister.GetsForOrderStatusWise(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.DateWise)
                {
                    SQL = "SELECT * FROM View_KnittingOrderRegister ";
                    sSQL = SQL + sSQL + "ORDER BY OrderDate, KnittingOrderID, KnittingOrderDetailID, StyleID ASC";
                    _oKnittingOrderRegisters = KnittingOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    SQL = "SELECT * FROM View_KnittingOrderRegister ";
                    sSQL = SQL + sSQL + "ORDER BY FactoryID, KnittingOrderID, KnittingOrderDetailID, StyleID ASC";
                    _oKnittingOrderRegisters = KnittingOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.Style_Wise)
                {
                    SQL = "SELECT * FROM View_KnittingOrderRegister ";
                    sSQL = SQL + sSQL + "ORDER BY StyleID, KnittingOrderID, KnittingOrderDetailID ASC";
                    _oKnittingOrderRegisters = KnittingOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

                if (_oKnittingOrderRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oKnittingOrderRegisters = new List<KnittingOrderRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
                if (oKnittingOrderRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oKnittingOrderRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                int count = 0, num = 0;
                double SubTotalStyleQty = 0, SubTotalFabricQty = 0, SubTotalFabricAmount = 0, SubTotalFabricRcvQty = 0, SubTotalFabricYetToRcvQty = 0, SubTotalYarnQty = 0, SubTotalYarnBalance = 0, SubTotalYarnConsumptionQty = 0, SubTotalYarnReturnQty = 0, SubTotalYarnProcessLossQty = 0;
                double GrandTotalStyleQty = 0, GrandTotalFabricQty = 0, GrandTotalFabricAmount = 0, GrandTotalFabricRcvQty = 0, GrandTotalFabricYetToRcvQty = 0, GrandTotalYarnQty = 0, GrandTotalYarnBalance = 0, GrandTotalYarnConsumptionQty = 0, GrandTotalYarnReturnQty = 0, GrandTotalYarnProcessLossQty = 0;

                string sKnittingOrderNo = "", sStyleNo = "";

                if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.Order_Status_Wise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Status Wise Knitting Order");
                        sheet.Name = "Order Status Wise Knitting Order";
                        sheet.Column(2).Width = 8; //SL
                        sheet.Column(3).Width = 13; //KO no/order date
                        sheet.Column(4).Width = 14; //business session/factory
                        sheet.Column(5).Width = 12; //start date/ approx date

                        sheet.Column(6).Width = 12; //style no
                        sheet.Column(7).Width = 14; //buyer
                        sheet.Column(8).Width = 12; //Style qty
                        sheet.Column(9).Width = 15; //fabric
                        sheet.Column(10).Width = 12; //color
                        sheet.Column(11).Width = 8; //M Unit
                        sheet.Column(12).Width = 12; //qty
                        sheet.Column(13).Width = 10; //unit price
                        sheet.Column(14).Width = 15; //amount
                        sheet.Column(15).Width = 12; //rcv qty
                        sheet.Column(16).Width = 12; //yet to qty

                        sheet.Column(17).Width = 15; //yarn name
                        sheet.Column(18).Width = 8; //MUnit
                        sheet.Column(19).Width = 12; //Qty
                        sheet.Column(20).Width = 12; //Consumption qty
                        sheet.Column(21).Width = 12; //return qty
                        sheet.Column(22).Width = 12; //process loss
                        sheet.Column(23).Width = 12; //Balance

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 23].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 23].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Order Status Wise Knitting Order"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 23].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "KO No/Order Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "B. Session/Factory"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Start Date/Approx Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Style Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Fabric"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = "Rcv Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = "Yet To Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 17]; cell.Value = "Yarn Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 19]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 20]; cell.Value = "Consumption Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 21]; cell.Value = "Loose Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 22]; cell.Value = "Process Loss Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 23]; cell.Value = "Balance"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        if (_oKnittingOrderRegisters != null && _oKnittingOrderRegisters.Count > 0)
                        {
                            int nKnittingOrderID = 0, nKnittingOrderDetailID = 0;
                            GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0; GrandTotalYarnQty = 0; GrandTotalYarnBalance = 0; GrandTotalYarnConsumptionQty = 0; GrandTotalYarnReturnQty = 0; GrandTotalYarnProcessLossQty = 0;
                            foreach (KnittingOrderRegister oItem in _oKnittingOrderRegisters)
                            {
                                #region body
                                if (oItem.KnittingOrderID != nKnittingOrderID)
                                {
                                    #region subtotal
                                    if (SubTotalStyleQty > 0)
                                    {
                                        cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 8]; cell.Value = SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 12]; cell.Value = SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 14]; cell.Value = SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 15]; cell.Value = SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 16]; cell.Value = SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                                        cell = sheet.Cells[rowIndex, 17, rowIndex, 18]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 19]; cell.Value = SubTotalYarnQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 20]; cell.Value = SubTotalYarnConsumptionQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 21]; cell.Value = SubTotalYarnReturnQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 22]; cell.Value = SubTotalYarnProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 23]; cell.Value = SubTotalYarnBalance.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        rowIndex += 1;
                                        SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0; SubTotalYarnQty = 0; SubTotalYarnBalance = 0; SubTotalYarnConsumptionQty = 0; SubTotalYarnReturnQty = 0; SubTotalYarnProcessLossQty = 0;
                                    }
                                    #endregion

                                    int rowCount = (_oKnittingOrderRegisters.Count(x => x.KnittingOrderID == oItem.KnittingOrderID)-1);
                                    rowCount = (rowCount == -1) ? 0 : rowCount;
                                    cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = ++num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.KnittingOrderNo + "\n" + oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.BusinessSessionName + "\n" + oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                }

                                if (oItem.KnittingOrderDetailID != nKnittingOrderDetailID)
                                {
                                    int rowCount = (_oKnittingOrderRegisters.Count(x => x.KnittingOrderID == oItem.KnittingOrderID && x.KnittingOrderDetailID == oItem.KnittingOrderDetailID)-1);
                                    rowCount = (rowCount == -1) ? 0 : rowCount;

                                    cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 7, rowIndex + rowCount, 7]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8, rowIndex + rowCount, 8]; cell.Merge = true; cell.Value = oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalStyleQty += oItem.FabricStyleQty;
                                    GrandTotalStyleQty += oItem.FabricStyleQty;

                                    cell = sheet.Cells[rowIndex, 9, rowIndex + rowCount, 9]; cell.Merge = true; cell.Value = oItem.FabricName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10, rowIndex + rowCount, 10]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11, rowIndex + rowCount, 11]; cell.Merge = true; cell.Value = oItem.FabricMUnitSymbol; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12, rowIndex + rowCount, 12]; cell.Merge = true; cell.Value = oItem.FabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricQty += oItem.FabricQty;
                                    GrandTotalFabricQty += oItem.FabricQty;

                                    cell = sheet.Cells[rowIndex, 13, rowIndex + rowCount, 13]; cell.Merge = true; cell.Value = oItem.FabricUnitPrice.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 14, rowIndex + rowCount, 14]; cell.Merge = true; cell.Value = oItem.FabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricAmount += oItem.FabricAmount;
                                    GrandTotalFabricAmount += oItem.FabricAmount;

                                    cell = sheet.Cells[rowIndex, 15, rowIndex + rowCount, 15]; cell.Merge = true; cell.Value = oItem.FabricRecvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricRcvQty += oItem.FabricRecvQty;
                                    GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                                    cell = sheet.Cells[rowIndex, 16, rowIndex + rowCount, 16]; cell.Merge = true; cell.Value = oItem.FabricYetRecvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricYetToRcvQty += oItem.FabricYetRecvQty;
                                    GrandTotalFabricYetToRcvQty += oItem.FabricYetRecvQty;
                                }

                                cell = sheet.Cells[rowIndex, 17]; cell.Merge = true; cell.Value = oItem.YarnName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 18]; cell.Merge = true; cell.Value = oItem.YarnMUnitSymbol; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 19]; cell.Merge = true; cell.Value = oItem.YarnChallanQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                SubTotalYarnQty += oItem.YarnChallanQty;
                                GrandTotalYarnQty += oItem.YarnChallanQty;


                                cell = sheet.Cells[rowIndex, 20]; cell.Merge = true; cell.Value = oItem.YarnConsumptionQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                SubTotalYarnConsumptionQty += oItem.YarnConsumptionQty;
                                GrandTotalYarnConsumptionQty += oItem.YarnConsumptionQty;

                                cell = sheet.Cells[rowIndex, 21]; cell.Merge = true; cell.Value = oItem.YarnReturnQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                SubTotalYarnReturnQty += oItem.YarnReturnQty;
                                GrandTotalYarnReturnQty += oItem.YarnReturnQty;

                                cell = sheet.Cells[rowIndex, 22]; cell.Merge = true; cell.Value = oItem.YarnProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                SubTotalYarnProcessLossQty += oItem.YarnProcessLossQty;
                                GrandTotalYarnProcessLossQty += oItem.YarnProcessLossQty;

                                cell = sheet.Cells[rowIndex, 23]; cell.Merge = true; cell.Value = oItem.YarnBalanceQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                SubTotalYarnBalance += oItem.YarnBalanceQty;
                                GrandTotalYarnBalance += oItem.YarnBalanceQty;

                                nKnittingOrderID = oItem.KnittingOrderID;
                                nKnittingOrderDetailID = oItem.KnittingOrderDetailID;
                                rowIndex += 1;
                                #endregion
                            }

                            #region subtotal
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 16]; cell.Value = SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 17, rowIndex, 18]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 19]; cell.Value = SubTotalYarnQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 20]; cell.Value = SubTotalYarnConsumptionQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 21]; cell.Value = SubTotalYarnReturnQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 22]; cell.Value = SubTotalYarnProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 23]; cell.Value = SubTotalYarnBalance.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;
                            #endregion

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 16]; cell.Value = GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 17, rowIndex, 18]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 19]; cell.Value = GrandTotalYarnQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 20]; cell.Value = GrandTotalYarnConsumptionQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 21]; cell.Value = GrandTotalYarnReturnQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 22]; cell.Value = GrandTotalYarnProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 23]; cell.Value = GrandTotalYarnBalance.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;
                            #endregion
                        }

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Order_Status_Wise_Knitting_Order.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.DateWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Knitting Order Register (Date Wise)");
                        sheet.Name = "Knitting Order Register (Date Wise)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 12; //KO no/order type
                        sheet.Column(4).Width = 12; //business session/factory
                        sheet.Column(5).Width = 12; //start date/ approx date

                        sheet.Column(6).Width = 10; //style no
                        sheet.Column(7).Width = 12; //buyer
                        sheet.Column(8).Width = 10; //Style qty
                        sheet.Column(9).Width = 15; //fabric
                        sheet.Column(10).Width = 12; //color
                        sheet.Column(11).Width = 8; //M Unit
                        sheet.Column(12).Width = 10; //qty
                        sheet.Column(13).Width = 10; //unit price
                        sheet.Column(14).Width = 15; //amount
                        sheet.Column(15).Width = 10; //rcv qty
                        sheet.Column(16).Width = 10; //yet to qty

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Knitting Order Register (Date Wise)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "KO No/Order Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "B. Session/Factory"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Start Date/Approx Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Style Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Fabric"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = "Rcv Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = "Yet To Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oKnittingOrderRegisters.Count > 0)
                        {
                            var data = _oKnittingOrderRegisters.GroupBy(x => new { x.OrderDateInString }, (key, grp) => new
                            {
                                OrderDate = key.OrderDateInString,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 16]; cell.Merge = true; cell.Value = "Order Date : @ " + oData.OrderDate; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    if (sKnittingOrderNo != "")
                                    {
                                        if (sKnittingOrderNo != oItem.KnittingOrderNo && count > 1)
                                        {
                                            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 8]; cell.Value = SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 12]; cell.Value = SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 14]; cell.Value = SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 15]; cell.Value = SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 16]; cell.Value = SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                                            rowIndex = rowIndex + 1;
                                            SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0; SubTotalYarnQty = 0; SubTotalYarnBalance = 0; SubTotalYarnConsumptionQty = 0; SubTotalYarnReturnQty = 0; SubTotalYarnProcessLossQty = 0;
                                        }
                                    }
                                    #endregion

                                    #region data
                                    if (sKnittingOrderNo != oItem.KnittingOrderNo)
                                    {
                                        num++;
                                        int rowCount = (oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo) - 1);
                                        rowCount = (rowCount == -1) ? 0 : rowCount;
                                        cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.KnittingOrderNo + "\n" + oItem.OrderTypeInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.BusinessSessionName + "\n" + oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    }

                                    if (sStyleNo != oItem.StyleNo)
                                    {
                                        int rowCount = (oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo && x.StyleNo == oItem.StyleNo)-1);
                                        rowCount = (rowCount == -1) ? 0 : rowCount;
                                        cell = sheet.Cells[rowIndex, 6,rowIndex + rowCount,6]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 7, rowIndex + rowCount, 7]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 8, rowIndex + rowCount, 8]; cell.Merge = true; cell.Value = oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        SubTotalStyleQty += oItem.FabricStyleQty;
                                        GrandTotalStyleQty += oItem.FabricStyleQty;
                                    }

                                    cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.FabricName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.FabricMUnitSymbol; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.FabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricQty += oItem.FabricQty;
                                    GrandTotalFabricQty += oItem.FabricQty;

                                    cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.FabricUnitPrice.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.FabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricAmount += oItem.FabricAmount;
                                    GrandTotalFabricAmount += oItem.FabricAmount;

                                    cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = oItem.FabricRecvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricRcvQty += oItem.FabricRecvQty;
                                    GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                                    cell = sheet.Cells[rowIndex, 16]; cell.Merge = true; cell.Value = (oItem.FabricQty - oItem.FabricRecvQty).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);
                                    GrandTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);

                                    #endregion
                                    rowIndex++;
                                    sKnittingOrderNo = oItem.KnittingOrderNo;
                                    sStyleNo = oItem.StyleNo;
                                }
                                #region subtotal
                                if (sKnittingOrderNo != "")
                                {
                                    //rowIndex = rowIndex + 1;

                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8]; cell.Value = SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Value = SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 14]; cell.Value = SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 15]; cell.Value = SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 16]; cell.Value = SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex = rowIndex + 1;
                                    SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0; SubTotalYarnQty = 0; SubTotalYarnBalance = 0; SubTotalYarnConsumptionQty = 0; SubTotalYarnReturnQty = 0; SubTotalYarnProcessLossQty = 0;
                                }
                                #endregion

                                #region total
                                #endregion

                                //cell = sheet.Cells[rowIndex, 2, rowIndex, 23]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 16]; cell.Value = GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;

                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Knitting_Order_Register_Date_Wise.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Knitting Order Register (Party Wise)");
                        sheet.Name = "Knitting Order Register (Party Wise)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 12; //KO no/order date
                        sheet.Column(4).Width = 12; //business session/order type
                        sheet.Column(5).Width = 12; //start date/ approx date

                        sheet.Column(6).Width = 10; //style no
                        sheet.Column(7).Width = 12; //buyer
                        sheet.Column(8).Width = 10; //Style qty
                        sheet.Column(9).Width = 15; //fabric
                        sheet.Column(10).Width = 12; //color
                        sheet.Column(11).Width = 8; //M Unit
                        sheet.Column(12).Width = 10; //qty
                        sheet.Column(13).Width = 10; //unit price
                        sheet.Column(14).Width = 15; //amount
                        sheet.Column(15).Width = 10; //rcv qty
                        sheet.Column(16).Width = 10; //yet to qty


                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Knitting Order Register (Party Wise)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "KO No/Order Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "B. Session/Order Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Start Date/Approx Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Style Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Fabric"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = "Rcv Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = "Yet To Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oKnittingOrderRegisters.Count > 0)
                        {
                            var data = _oKnittingOrderRegisters.GroupBy(x => new { x.FactoryName }, (key, grp) => new
                            {
                                FactoryName = key.FactoryName,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 16]; cell.Merge = true; cell.Value = "Party : @ " + oData.FactoryName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    if (sKnittingOrderNo != "")
                                    {
                                        if (sKnittingOrderNo != oItem.KnittingOrderNo && count > 1)
                                        {
                                            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 8]; cell.Value = SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 12]; cell.Value = SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 14]; cell.Value = SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 15]; cell.Value = SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 16]; cell.Value = SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                                            rowIndex = rowIndex + 1;
                                            SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0; SubTotalYarnQty = 0; SubTotalYarnBalance = 0; SubTotalYarnConsumptionQty = 0; SubTotalYarnReturnQty = 0; SubTotalYarnProcessLossQty = 0;
                                        }
                                    }
                                    #endregion

                                    #region data
                                    if (sKnittingOrderNo != oItem.KnittingOrderNo)
                                    {
                                        num++;
                                        int rowCount = (oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo) - 1);
                                        rowCount = (rowCount == -1) ? 0 : rowCount;
                                        cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.KnittingOrderNo + "\n" + oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.BusinessSessionName + "\n" + oItem.OrderTypeInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    }

                                    if (sStyleNo != oItem.StyleNo)
                                    {
                                        int rowCount = (oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo && x.StyleNo == oItem.StyleNo)-1);
                                        rowCount = (rowCount == -1) ? 0 : rowCount;
                                        cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 7, rowIndex + rowCount, 7]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 8, rowIndex + rowCount, 8]; cell.Merge = true; cell.Value = oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        SubTotalStyleQty += oItem.FabricStyleQty;
                                        GrandTotalStyleQty += oItem.FabricStyleQty;
                                    }

                                    cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.FabricName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.FabricMUnitSymbol; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.FabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricQty += oItem.FabricQty;
                                    GrandTotalFabricQty += oItem.FabricQty;

                                    cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.FabricUnitPrice.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.FabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricAmount += oItem.FabricAmount;
                                    GrandTotalFabricAmount += oItem.FabricAmount;

                                    cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = oItem.FabricRecvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricRcvQty += oItem.FabricRecvQty;
                                    GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                                    cell = sheet.Cells[rowIndex, 16]; cell.Merge = true; cell.Value = (oItem.FabricQty - oItem.FabricRecvQty).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);
                                    GrandTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);

                                    #endregion
                                    rowIndex++;
                                    sKnittingOrderNo = oItem.KnittingOrderNo;
                                    sStyleNo = oItem.StyleNo;
                                }
                                #region subtotal
                                if (sKnittingOrderNo != "")
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8]; cell.Value = SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Value = SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 14]; cell.Value = SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 15]; cell.Value = SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 16]; cell.Value = SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex = rowIndex + 1;
                                    SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0; SubTotalYarnQty = 0; SubTotalYarnBalance = 0; SubTotalYarnConsumptionQty = 0; SubTotalYarnReturnQty = 0; SubTotalYarnProcessLossQty = 0;
                                }
                                #endregion

                                #region total
                                #endregion

                                //cell = sheet.Cells[rowIndex, 2, rowIndex, 23]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 16]; cell.Value = GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;

                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Knitting_Order_Register_Party_Wise.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oKnittingOrderRegister.ReportLayout == EnumReportLayout.Style_Wise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Knitting Order Register (Style Wise)");
                        sheet.Name = "Knitting Order Register (Style Wise)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 12; //KO no/order date
                        sheet.Column(4).Width = 12; //business session/factory
                        sheet.Column(5).Width = 12; //start date/ approx date

                        sheet.Column(6).Width = 12; //buyer
                        sheet.Column(7).Width = 10; //Style qty
                        sheet.Column(8).Width = 15; //fabric
                        sheet.Column(9).Width = 12; //color
                        sheet.Column(10).Width = 8; //M Unit
                        sheet.Column(11).Width = 10; //qty
                        sheet.Column(12).Width = 10; //unit price
                        sheet.Column(13).Width = 15; //amount
                        sheet.Column(14).Width = 10; //rcv qty
                        sheet.Column(15).Width = 10; //yet to qty


                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Knitting Order Register (Style Wise)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "KO No/Order Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "B. Session/Order Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Start Date/Approx Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Style Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Fabric"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = "Rcv Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = "Yet To Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oKnittingOrderRegisters.Count > 0)
                        {
                            var data = _oKnittingOrderRegisters.GroupBy(x => new { x.StyleNo }, (key, grp) => new
                            {
                                StyleNo = key.StyleNo,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true; cell.Value = "Style No : @ " + oData.StyleNo; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    //if (sKnittingOrderNo != "")
                                    //{
                                    //    if (sKnittingOrderNo != oItem.KnittingOrderNo && count > 1)
                                    //    {
                                    //        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 7]; cell.Value = SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 8, rowIndex, 10]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 13]; cell.Value = SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 14]; cell.Value = SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 15]; cell.Value = SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                                    //        rowIndex = rowIndex + 1;
                                    //        SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0; 
                                    //    }
                                    //}
                                    #endregion

                                    #region data
                                    if (sKnittingOrderNo != oItem.KnittingOrderNo)
                                    {
                                        num++;
                                        int rowCount = (oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo) - 1);
                                        rowCount = (rowCount == -1) ? 0 : rowCount;
                                        cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.KnittingOrderNo + "\n" + oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.BusinessSessionName + "\n" + oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    }

                                    cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalStyleQty += oItem.FabricStyleQty;
                                    GrandTotalStyleQty += oItem.FabricStyleQty;

                                    cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.FabricName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.FabricMUnitSymbol; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.FabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricQty += oItem.FabricQty;
                                    GrandTotalFabricQty += oItem.FabricQty;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.FabricUnitPrice.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.FabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricAmount += oItem.FabricAmount;
                                    GrandTotalFabricAmount += oItem.FabricAmount;

                                    cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.FabricRecvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricRcvQty += oItem.FabricRecvQty;
                                    GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                                    cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = (oItem.FabricQty - oItem.FabricRecvQty).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);
                                    GrandTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);

                                    #endregion
                                    rowIndex++;
                                    sKnittingOrderNo = oItem.KnittingOrderNo;
                                    sStyleNo = oItem.StyleNo;
                                }
                                #region total
                                #endregion

                                //cell = sheet.Cells[rowIndex, 2, rowIndex, 23]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8, rowIndex, 10]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;

                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Knitting_Order_Register_Style_Wise.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
            }
        }

        #endregion

    }

}
