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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUProGuideLineController : PdfViewController
    {
        #region Declaration
        DUProGuideLine _oDUProGuideLine = new DUProGuideLine();
        List<DUProGuideLine> _oDUProGuideLines = new List<DUProGuideLine>();
        List<DUProGuideLineDetail> _oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
        #endregion

        #region Action/JSon Result
        public ActionResult ViewDUProGuideLines(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUProGuideLine).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            #region Received Stores
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DUProGuideLine, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            //oReceivedStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            string sReceivedStoreIDs = string.Join(",", oReceivedStores.Select(x => x.WorkingUnitID).ToList());
            if (string.IsNullOrEmpty(sReceivedStoreIDs))
                sReceivedStoreIDs = "0";
            #endregion

            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            string sSQL = "SELECT * FROM View_DUProGuideLine WHERE ISNULL(ReceiveByID,0)=0 AND WorkingUnitID IN (" + sReceivedStoreIDs + ")";
            oDUProGuideLines = DUProGuideLine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ProductNatures = EnumObject.jGets(typeof(EnumProductNature)).Where(x => ((int)EnumProductNature.Yarn + "," + (int)EnumProductNature.DyesChemical).Contains(x.id.ToString())).ToList(); //2:POLY & 3:YARN
            ViewBag.ReceivedStores = oReceivedStores;
            ViewBag.BUID = buid;
            
            return View(oDUProGuideLines);
        }
        public ActionResult ViewDUProGuideLine(int nId, int buid, double ts)
        {
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DUProGuideLineDetail> grpDUProGuideLineDetails = new List<DUProGuideLineDetail>();

            if (nId > 0)
            {
                oDUProGuideLine = oDUProGuideLine.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUProGuideLine.DUProGuideLineDetails = DUProGuideLineDetail.Gets(oDUProGuideLine.DUProGuideLineID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDUProGuideLine.DyeingOrderID > 0) 
                {
                    string sSQL = "SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID=" + oDUProGuideLine.DyeingOrderID + " AND ISNULL(InOutType,0)!=102 )";
                    grpDUProGuideLineDetails = DUProGuideLineDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    grpDUProGuideLineDetails = grpDUProGuideLineDetails.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new DUProGuideLineDetail
                    {
                        ProductID = key.ProductID,
                        ProductName = key.ProductName,
                        Qty = grp.Sum(x => x.Qty)
                    }).ToList();
                }

                if (grpDUProGuideLineDetails.Any()) 
                {
                    foreach (var oItem in oDUProGuideLine.DUProGuideLineDetails)
                    {
                        oItem.Balance = oItem.Qty_Order - grpDUProGuideLineDetails.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty).FirstOrDefault();
                    }
                }
            }
            oDUProGuideLine.BUID = buid;
           
            #region Received Stores
            oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(oDUProGuideLine.BUID, EnumModuleName.DUProGuideLine, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            //oReceivedStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.ReceivedStores = oReceivedStores;

            ViewBag.BUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.ProductNatures = EnumObject.jGets(typeof(EnumProductNature)).Where(x => ((int)EnumProductNature.Yarn + "," + (int)EnumProductNature.DyesChemical).Contains(x.id.ToString())).ToList(); //2:POLY & 3:YARN
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.ContractorTypes = EnumObject.jGets(typeof(EnumContractorType)).Where(x => x.id == (int)EnumContractorType.Buyer || x.id == (int)EnumContractorType.Supplier || x.id == (int)EnumContractorType.Factory).ToList();
            oDUProGuideLine.BUID = buid;
            return View(oDUProGuideLine);
        }
        public ActionResult ViewDUProGuideLineReturn(int nId, int buid, double ts)
        {
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DUProGuideLineDetail> grpDUProGuideLineDetails = new List<DUProGuideLineDetail>();

            if (nId > 0)
            {
                oDUProGuideLine = oDUProGuideLine.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUProGuideLine.DUProGuideLineDetails = DUProGuideLineDetail.Gets(oDUProGuideLine.DUProGuideLineID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDUProGuideLine.DyeingOrderID > 0)
                {
                    string sSQL = "SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID=" + oDUProGuideLine.DyeingOrderID + ")";
                    grpDUProGuideLineDetails = DUProGuideLineDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    grpDUProGuideLineDetails = grpDUProGuideLineDetails.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new DUProGuideLineDetail
                    {
                        ProductID = key.ProductID,
                        ProductName = key.ProductName,
                        Qty = grp.Sum(x => x.Qty)
                    }).ToList();
                }

                if (grpDUProGuideLineDetails.Any())
                {
                    foreach (var oItem in oDUProGuideLine.DUProGuideLineDetails)
                    {
                        oItem.Balance = oItem.Qty_Order - grpDUProGuideLineDetails.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty).FirstOrDefault();
                    }
                }
            }
            oDUProGuideLine.BUID = buid;

            #region Received Stores
            oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(oDUProGuideLine.BUID, EnumModuleName.DUProGuideLine, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            //oReceivedStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.ReceivedStores = oReceivedStores;

            ViewBag.BUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.ProductNatures = EnumObject.jGets(typeof(EnumProductNature)).Where(x => ((int)EnumProductNature.Yarn + "," + (int)EnumProductNature.DyesChemical).Contains(x.id.ToString())).ToList(); //2:POLY & 3:YARN
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.ContractorTypes = EnumObject.jGets(typeof(EnumContractorType)).Where(x => x.id == (int)EnumContractorType.Buyer || x.id == (int)EnumContractorType.Supplier || x.id == (int)EnumContractorType.Factory).ToList();
            oDUProGuideLine.BUID = buid;
            return View(oDUProGuideLine);
        }
        public ActionResult AdvSearchDUProGuideLine()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Save(DUProGuideLine oDUProGuideLine)
        {
            oDUProGuideLine.RemoveNulls();
            _oDUProGuideLine = new DUProGuideLine();
            try
            {
                _oDUProGuideLine = oDUProGuideLine.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUProGuideLine = new DUProGuideLine();
                _oDUProGuideLine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUProGuideLine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult UpdateReturnQty(DUProGuideLine oDUProGuideLine)
        {
            oDUProGuideLine.RemoveNulls();
            _oDUProGuideLine = new DUProGuideLine();
            try
            {
                _oDUProGuideLine = oDUProGuideLine.Update_ReturnQty(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUProGuideLine = new DUProGuideLine();
                _oDUProGuideLine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUProGuideLine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Lot_Adjustment(LotParent oLotParent)
        {
            oLotParent.RemoveNulls();
            LotParent _oLotParent = new LotParent();
            try
            {
                _oLotParent = oLotParent.Lot_Adjustment(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLotParent = new LotParent();
                _oLotParent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotParent);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DUProGuideLine oDUProGuideLine)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDUProGuideLine.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ApproveDUProGuideLine(DUProGuideLine oDUProGuideLine)
        {
            _oDUProGuideLine = new DUProGuideLine();
            _oDUProGuideLine = oDUProGuideLine.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUProGuideLine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApproveDUProGuideLine(DUProGuideLine oDUProGuideLine)
        {
            _oDUProGuideLine = new DUProGuideLine();
            _oDUProGuideLine = oDUProGuideLine.UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUProGuideLine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RecivedDUProGuideLine(DUProGuideLine oDUProGuideLine)
        {
            _oDUProGuideLine = new DUProGuideLine();
            _oDUProGuideLine = oDUProGuideLine.Receive(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUProGuideLine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReturnDUProGuideLine(DUProGuideLine oDUProGuideLine)
        {
            _oDUProGuideLine = new DUProGuideLine();
            _oDUProGuideLine = oDUProGuideLine.Return(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUProGuideLine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search

        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            try
            {
                string sSQL = GetSQL(sTemp);
                oDUProGuideLines = DUProGuideLine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUProGuideLine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUProGuideLines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nDUProGuideLineDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtDUProGuideLineDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtDUProGuideLineEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            int nReceiveStore = Convert.ToInt32(sTemp.Split('~')[3]);
            bool nYTApprove = Convert.ToBoolean(sTemp.Split('~')[4]);
            bool nYTReceive = Convert.ToBoolean(sTemp.Split('~')[5]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[6]);
            int nOrderType = Convert.ToInt32(sTemp.Split('~')[7]);
            string sOrderNo = Convert.ToString(sTemp.Split('~')[8]);
            int nProductType = Convert.ToInt32(sTemp.Split('~')[9]);
            string sSLNo = Convert.ToString(sTemp.Split('~')[10]);
            string sLotNo = Convert.ToString(sTemp.Split('~')[11]);

            string sReturn1 = "SELECT * FROM View_DUProGuideLine";
            string sReturn = "";

            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion

            #region Receive Store
            if (nReceiveStore != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID = " + nReceiveStore;
            }
            else
            {
                #region Received Stores
                oReceivedStores = new List<WorkingUnit>();
                oReceivedStores = WorkingUnit.GetsPermittedStore(nBUID, EnumModuleName.DUProGuideLine, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
                //oReceivedStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
                #endregion
                string sReceivedStoreIDs = string.Join(",", oReceivedStores.Select(x => x.WorkingUnitID).ToList());
                if (string.IsNullOrEmpty(sReceivedStoreIDs))
                {
                    sReceivedStoreIDs = "0";
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID IN ( " + sReceivedStoreIDs + " )";
            }
            #endregion
            #region nYTApprove
            if (nYTApprove)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApproveByID,0)= 0 ";
            }
            #endregion
            #region nYTReceive
            if (nYTReceive)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ReceiveByID,0)= 0";
            }
            #endregion
            #region Pro. Guide Date Wise
            if (nDUProGuideLineDateCompare > 0)
            {
                if (nDUProGuideLineDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDUProGuideLineDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDUProGuideLineDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDUProGuideLineDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDUProGuideLineDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDUProGuideLineDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUProGuideLineEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Order Type
            if (nOrderType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderType = " + nOrderType;
            }
            #endregion
            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderNo LIKE '%" + sOrderNo + "%'";
            }
            #endregion
            #region SL No
            if (!string.IsNullOrEmpty(sSLNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SLNo LIKE '%" + sSLNo + "%'";
            }
            #endregion
            #region Lot No
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLineDetail WHERE LotNo LIKE '%" + sLotNo + "%')";
            }
            #endregion
            #region Product Type
            if (nProductType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductType = " + nProductType;
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion

        #region Product
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                //oProducts = Product.GetsByBU(oProduct.BUID, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.DUProGuideLine, (EnumProductUsages)oProduct.ProductUsagesInInt, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.DUProGuideLine, (EnumProductUsages)oProduct.ProductUsagesInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        #endregion

        #region GetsDyeingOrder
        [HttpPost]
        public JsonResult GetsDyeingOrder(DyeingOrder oDyeingOrder)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            try 
            {
                string sSQL = "SELECT * FROM View_DyeingOrder WHERE DyeingOrderID>0";
                if(string.IsNullOrEmpty(oDyeingOrder.OrderNo))
                    sSQL = "SELECT TOP 100 * FROM View_DyeingOrder WHERE DyeingOrderID>0";
                if (oDyeingOrder.DyeingOrderType>0)
                sSQL+=" AND DyeingOrderType=" + oDyeingOrder.DyeingOrderType;

                sSQL += " AND (ISNULL(OrderNo,'')+ ISNULL(RefNo,'')) LIKE '%" + oDyeingOrder.OrderNo + "%'";
                if (oDyeingOrder.ContractorID > 0)
                    sSQL += " AND ContractorID=" + oDyeingOrder.ContractorID;

                oDyeingOrders = DyeingOrder.Gets(sSQL + " AND [Status] NOT IN (" + (int)EnumDyeingOrderState.Cancelled + "," + (int)EnumDyeingOrderState.None + "," + (int)EnumDyeingOrderState.Initialized + "," + (int)EnumDyeingOrderState.WatingForApprove + ") ORDER BY OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch(Exception e)
            {
                DyeingOrder _oDUProGuideLine = new DyeingOrder();
                _oDUProGuideLine.ErrorMessage = e.Message;
                oDyeingOrders.Add(_oDUProGuideLine);
            }
     
            var jSonResult = Json(oDyeingOrders, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        public JsonResult GetsDyeingOrderDetails(DyeingOrder oDyeingOrder)
        {
            DyeingOrder oTempDyeingOrder = new DyeingOrder();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
            List<DUProGuideLineDetail> oDUPLDetails = new List<DUProGuideLineDetail>();
            try
            {
                oTempDyeingOrder = DyeingOrder.Get(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT * FROM DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID=" + oDyeingOrder.DyeingOrderID + ")";
                oDUPLDetails = DUProGuideLineDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDyeingOrderDetails = DyeingOrderDetail.Gets(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUProGuideLineDetails = oDyeingOrderDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.BuyerRef }, (key, grp) => new DUProGuideLineDetail
                {
                    ProductID = key.ProductID,
                    ProductName = key.ProductName,
                    LotNo = key.BuyerRef,
                    Qty = grp.Sum(x => x.Qty), // - grp.Sum(x => x.Qty_Pro),
                    Qty_Order = grp.Sum(x => x.Qty),
                    UnitPrice = grp.Max(x => x.UnitPrice),
                    MUnitID = grp.Max(x => x.MUnitID),
                    MUnit = grp.Max(x => x.MUnit),
                    CurrencyID = oTempDyeingOrder.CurrencyID
                }).ToList();

                if (oDUPLDetails != null && oDUPLDetails.Count > 0)
                {
                    foreach (var oItem in oDUPLDetails)
                    {
                        oDUProGuideLineDetails.Where(x => x.ProductID == oItem.ProductID).FirstOrDefault().Qty -= oItem.Qty;
                    }
                }
                foreach (DUProGuideLineDetail oItem in oDUProGuideLineDetails)
                {
                    if (oItem.Qty < 0) { oItem.Qty = 0; }
                }
            }
            catch (Exception e)
            {
                DyeingOrderDetail _oDyeingOrderDetail = new DyeingOrderDetail();
                _oDyeingOrderDetail.ErrorMessage = e.Message;
                oDyeingOrderDetails.Add(_oDyeingOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDUProGuideLineDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetsDyeingOrderDetails(DyeingOrder oDyeingOrder)
        //{
        //    DyeingOrder oTempDyeingOrder = new DyeingOrder();
        //    List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
        //    List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
        //    List<DUProGuideLineDetail> oDUPLDetails = new List<DUProGuideLineDetail>();
        //    try
        //    {
        //        oTempDyeingOrder = DyeingOrder.Get(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        string sSQL = "SELECT * FROM DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID=" + oDyeingOrder.DyeingOrderID + ")";
        //        oDUPLDetails = DUProGuideLineDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        oDyeingOrderDetails = DyeingOrderDetail.Gets(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oDUProGuideLineDetails = oDyeingOrderDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.BuyerRef }, (key, grp) => new DUProGuideLineDetail
        //            {
        //                ProductID=key.ProductID,
        //                ProductName = key.ProductName,
        //                LotNo = key.BuyerRef,
        //                Qty = grp.Sum(x => x.Qty), // - grp.Sum(x => x.Qty_Pro),
        //                Qty_Order = grp.Sum(x => x.Qty),
        //                Amount = grp.Sum(x => (x.Qty*x.UnitPrice)),
        //                MUnitID = grp.Max(x => x.MUnitID),
        //                MUnit = grp.Max(x => x.MUnit),
        //                CurrencyID = oTempDyeingOrder.CurrencyID
        //            }).ToList();

        //        if (oDUPLDetails != null && oDUPLDetails.Count > 0)
        //        {
        //            foreach (DUProGuideLineDetail oItem in oDUPLDetails)
        //            {
        //                oDUProGuideLineDetails.Where(x => x.ProductID == oItem.ProductID).FirstOrDefault().Qty -= oItem.Qty;
        //            }
        //        }
        //        foreach (DUProGuideLineDetail oItem in oDUProGuideLineDetails)
        //        {
        //            if (oItem.Qty < 0) { oItem.Qty = 0; }
        //            if (oItem.Amount > 0)
        //            {
        //                oItem.UnitPrice = oItem.Amount / oItem.Qty;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        DUProGuideLineDetail oDUProGuideLineDetail = new DUProGuideLineDetail();
        //        oDUProGuideLineDetail.ErrorMessage = e.Message;
        //        oDUProGuideLineDetails.Add(oDUProGuideLineDetail);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oDUProGuideLineDetails);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);

        //    //var jSonResult = Json(oDUProGuideLineDetails, JsonRequestBehavior.AllowGet);
        //    //jSonResult.MaxJsonLength = int.MaxValue;
        //    //return jSonResult;
        //}
        #endregion

        #region Gets Lot
        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            try
            {
                string sSQL = "SELECT * FROM View_Lot WHERE LotID<>0 and ParentType=" + (int)EnumTriggerParentsType.DUProGuideLineDetail + " and ContractorID=" + oLot.ContractorID;

                if (oLot.ProductID > 0)
                    sSQL = sSQL + " And ProductID = " + oLot.ProductID;
                if (oLot.BUID > 0)
                    sSQL = sSQL + " And BUID = " + oLot.BUID;
                if (oLot.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;
                if (!string.IsNullOrEmpty(oLot.LotNo))
                    sSQL = sSQL + " And LotNo LIKE '%" + oLot.LotNo + "%'";

                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLot_Others(LotParent oLotParent)
        {
            List<LotParent> _oLotParents = new List<LotParent>();
            try
            {
                //string ss = "SELECT * FROM LoTParent AS LP WHERE LP.LotID IN (SELECT LotID FROM DUProGuideLineDetail AS DUPD WHERE DUPD.DUProGuideLineID IN  ((SELECT DUP.DUProGuideLineID FROM DUProGuideLine AS DUP WHERE DUP.DyeingOrderID="+ oLot.ParentID +" AND DUP.WorkingUnitID ="+ oLot.WorkingUnitID +")))";

                //SELECT * FROM LoTParent AS LP WHERE LP.DyeingOrderID="++" AND LP.ProductID="++" AND LP.LotID IN (SELECT Lot.LotID FROM Lot WHERE Lot.WorkingUnitID="++") 
                string sSQL = "SELECT * FROM View_LoTParent AS LP WHERE ParentType=" + (int)EnumTriggerParentsType.DUProGuideLineDetail;// +" and ContractorID=" + oLot.ContractorID;

                if (oLotParent.ProductID > 0)
                    sSQL = sSQL + " AND LP.ProductID = " + oLotParent.ProductID;
                if (oLotParent.LotID > 0)
                    sSQL = sSQL + " AND LP.LotID = " + oLotParent.LotID;
                if (oLotParent.DyeingOrderID > 0)
                    sSQL = sSQL + " AND LP.DyeingOrderID_Out= " + oLotParent.DyeingOrderID;
                if (oLotParent.ContractorID > 0)
                    sSQL = sSQL + " AND LP.DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE ContractorID=" + oLotParent.ContractorID + ")";
                if (oLotParent.WorkingUnitID > 0)
                    sSQL = sSQL + " AND LP.LotID IN (SELECT Lot.LotID FROM Lot WHERE Lot.WorkingUnitID=" + oLotParent.WorkingUnitID + ")";
                if (!string.IsNullOrEmpty(oLotParent.LotNo))
                    sSQL = sSQL + " And LotNo LIKE '%" + oLotParent.LotNo + "%'";
                _oLotParents = LotParent.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLotParents = new List<LotParent>(); _oLotParents.Add(new LotParent() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotParents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLot_Distributed(LotParent oLotParent)
        {
            List<LotParent> _oLotParents = new List<LotParent>();
            try
            {
                //string ss = "SELECT * FROM LoTParent AS LP WHERE LP.LotID IN (SELECT LotID FROM DUProGuideLineDetail AS DUPD WHERE DUPD.DUProGuideLineID IN  ((SELECT DUP.DUProGuideLineID FROM DUProGuideLine AS DUP WHERE DUP.DyeingOrderID="+ oLot.ParentID +" AND DUP.WorkingUnitID ="+ oLot.WorkingUnitID +")))";

                //SELECT * FROM LoTParent AS LP WHERE LP.DyeingOrderID="++" AND LP.ProductID="++" AND LP.LotID IN (SELECT Lot.LotID FROM Lot WHERE Lot.WorkingUnitID="++") 
                string sSQL = "SELECT * FROM View_LoTParent AS LP WHERE LP.LotID<>0 and LP.ParentType=" + (int)EnumTriggerParentsType.DUProGuideLineDetail;

                if (oLotParent.ProductID > 0)
                    sSQL = sSQL + " And ProductID = " + oLotParent.ProductID;
                if (oLotParent.ParentID > 0)
                    sSQL = sSQL + " And DyeingOrderID = " + oLotParent.DyeingOrderID;
                //if (oLotParent.WorkingUnitID > 0)
                //    sSQL = sSQL + " LP.LotID IN (SELECT Lot.LotID FROM Lot WHERE Lot.WorkingUnitID=" + oLotParent.WorkingUnitID + ")";
                if (oLotParent.IsDistribute)
                    sSQL = sSQL + " And IsDistribute =1 ";
                _oLotParents = LotParent.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLotParent = new LotParent();
                oLotParent.ErrorMessage = ex.Message;
                _oLotParents = new List<LotParent>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotParents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult Gets_DUDetailProducts(DyeingOrderDetail oDyeingOrderDetail)
        {
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            oDyeingOrderDetails = DyeingOrderDetail.Gets(
                    "SELECT top(500) * FROM View_DyeingOrderDetail WHERE ProductID>0"
                    //+ (oDyeingOrderDetail.ProductID < 0 ? "" : " AND ProductID =" + oDyeingOrderDetail.ProductID)
                    + (oDyeingOrderDetail.ContractorID < 0 ? "" : " AND ContractorID =" + oDyeingOrderDetail.ContractorID)
                    
                    + (string.IsNullOrEmpty(oDyeingOrderDetail.OrderNo) ? "" : " AND OrderNo Like '%" + oDyeingOrderDetail.OrderNo + "%' ")
                    + (oDyeingOrderDetail.DyeingOrderID < 0 ? "" : " AND DyeingOrderID NOT IN ("+ oDyeingOrderDetail.DyeingOrderID + ")" )
                    + " AND DyeingOrderID IN (SELECT DO.DyeingOrderID FROM DyeingOrder AS DO WHERE ISNULL(DO.ApproveBy,0)<>0 AND DO.DyeingOrderType IN (Select OrderType from DUOrderSetup WHERE ISNULL(IsInHouse,0)=0))  ORDER BY OrderDate DESC",
                    ((User)Session[SessionInfo.CurrentUser]).UserID);

            oDyeingOrderDetails = oDyeingOrderDetails.GroupBy(x => new { x.DyeingOrderID, x.OrderNo, x.ProductID, x.ProductName }, (key, grp) => new DyeingOrderDetail
            {
                ProductID = key.ProductID,
                ProductName = key.ProductName,
                ProductCodeName = grp.Select(x => x.ProductCodeName).FirstOrDefault(),
                DyeingOrderID = key.DyeingOrderID,
                OrderNo = key.OrderNo,
                OrderDate = grp.Select(x => x.OrderDate).FirstOrDefault(),
                BuyerName = grp.Select(x => x.BuyerName).FirstOrDefault(),
                Qty = grp.Sum(x => x.Qty),
                UnitPrice = grp.Max(x => x.UnitPrice)
            }).ToList();

            var jsonResult = Json(oDyeingOrderDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Print
        public ActionResult DUProGuideLinePreview(int nDUProGuideLineID, int nBUID)
        {
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            List<LotParent> oLotParents = new List<LotParent>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();

            List<DURequisitionDetail> oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            #region Print Setup
            if (nDUProGuideLineID > 0)
            {
                _oDUProGuideLine = oDUProGuideLine.Get(nDUProGuideLineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                _oDUProGuideLine.DUProGuideLineDetails = DUProGuideLineDetail.Gets(nDUProGuideLineID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUProGuideLine.DyeingOrderID > 0)
                {
                    string sSQL = "SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) != " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID=" + _oDUProGuideLine.DyeingOrderID + ") ORDER BY ProductID";
                    _oDUProGuideLineDetails = DUProGuideLineDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    sSQL = "SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND InOutType = " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID=" + _oDUProGuideLine.DyeingOrderID + ") ORDER BY ProductID";
                    oDUProGuideLineDetails_Return = DUProGuideLineDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    oDyeingOrderDetails = DyeingOrderDetail.Gets(_oDUProGuideLine.DyeingOrderID, (int)Session[SessionInfo.currentUserID]);

                    oDURequisitionDetails_SRS = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID = " + _oDUProGuideLine.DyeingOrderID + "  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType= " + (int)EnumInOutType.Receive + " AND ISNULL(ReceiveByID,0)<>0 ) ", (int)Session[SessionInfo.currentUserID]);
                    oDURequisitionDetails_SRM = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID = " + _oDUProGuideLine.DyeingOrderID + "  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType= " + (int)EnumInOutType.Disburse + " AND ISNULL(ReceiveByID,0)<>0 )", (int)Session[SessionInfo.currentUserID]);

                    oLotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID = " + _oDUProGuideLine.DyeingOrderID + " OR DyeingOrderID_Out = " + _oDUProGuideLine.DyeingOrderID, (int)Session[SessionInfo.currentUserID]);
                    //oLotParents_Out = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID_Out = " + _oDUProGuideLine.DyeingOrderID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            rptDUProGuideLine oReport = new rptDUProGuideLine();
            oReport.LotParents = oLotParents;
            oReport.DURequisitionDetails_SRM = oDURequisitionDetails_SRM;
            oReport.DURequisitionDetails_SRS = oDURequisitionDetails_SRS;
            oReport.DUProGuideLineDetails_Return = oDUProGuideLineDetails_Return;
            byte[] abytes = oReport.PrepareReport(_oDUProGuideLine, _oDUProGuideLineDetails, oDyeingOrderDetails,oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult DUPGL_Report(int nDOID, int nBUID)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<LotParent> oLotParents = new List<LotParent>();
            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DUProductionStatusReport> oDUProductionStatusReports = new List<DUProductionStatusReport>();
                    
            oDyeingOrder = DyeingOrder.Get(nDOID, (int)Session[SessionInfo.currentUserID]);

            #region Print Setup
            if (oDyeingOrder.DyeingOrderID > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_DUProGuideLine WHERE DyeingOrderID=" + oDyeingOrder.DyeingOrderID + " ORDER BY ReceiveDate ASC";
                oDUProGuideLines = DUProGuideLine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID=" + oDyeingOrder.DyeingOrderID + ")";
                _oDUProGuideLineDetails = DUProGuideLineDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                oDUProductionStatusReports = DUProductionStatusReport.Gets(" where DyeingOrderID =" + oDyeingOrder.DyeingOrderID, EnumReportLayout.BankWise, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUProGuideLineDetails.Any())
                    oLotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE LotID IN ("+ string.Join(",",_oDUProGuideLineDetails.Select(x=>x.LotID)) +") ORDER BY LotNo", (int)Session[SessionInfo.currentUserID]);
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            rptDUProGuideLineReport oReport = new rptDUProGuideLineReport();
            byte[] abytes = oReport.PrepareReport(oDyeingOrder, oDUProGuideLines, _oDUProGuideLineDetails, oDUProductionStatusReports,oLotParents, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult DUPGL_ReportV2(int nDOID, int nBUID, int nDUProGuideLineID)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            _oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();

            oDyeingOrder = DyeingOrder.Get(nDOID, (int)Session[SessionInfo.currentUserID]);

            #region Print Setup
            if (oDyeingOrder.DyeingOrderID > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "";

                _oDUProGuideLine = oDUProGuideLine.Get(nDUProGuideLineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) != " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID=" + _oDUProGuideLine.DyeingOrderID + ") ORDER BY ProductID";
                _oDUProGuideLineDetails = DUProGuideLineDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                oLotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID = " + _oDUProGuideLine.DyeingOrderID + " OR DyeingOrderID_Out = " + _oDUProGuideLine.DyeingOrderID, (int)Session[SessionInfo.currentUserID]);
                oDyeingOrderDetails = DyeingOrderDetail.Gets(_oDUProGuideLine.DyeingOrderID, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND InOutType = " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID=" + _oDUProGuideLine.DyeingOrderID + ") ORDER BY ProductID";
                oDUProGuideLineDetails_Return = DUProGuideLineDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oDURequisitionDetails_SRS = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID = " + _oDUProGuideLine.DyeingOrderID + "  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType= " + (int)EnumInOutType.Receive + " AND ISNULL(ReceiveByID,0)<>0 ) ", (int)Session[SessionInfo.currentUserID]);
                oDURequisitionDetails_SRM = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID = " + _oDUProGuideLine.DyeingOrderID + "  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType= " + (int)EnumInOutType.Disburse + " AND ISNULL(ReceiveByID,0)<>0 )", (int)Session[SessionInfo.currentUserID]);


            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (nBUID > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            

            rptDUProGuideLineReportV2 oReport = new rptDUProGuideLineReportV2();
            byte[] abytes = oReport.PrepareReport(oDyeingOrder, _oDUProGuideLine, _oDUProGuideLineDetails, oLotParents, oDyeingOrderDetails, oDUProGuideLineDetails_Return, oDURequisitionDetails_SRS,oDURequisitionDetails_SRM,  oCompany);
            return File(abytes, "application/pdf");
        }

        public void Print_ReportXL_Details(string sTempString, int BUID)
        {
            List<DUProductionStatusReport> _oDUProductionStatusReports = new List<DUProductionStatusReport>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            //List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();

            if (string.IsNullOrEmpty(sTempString))
            {
                _oDUProductionStatusReports = new List<DUProductionStatusReport>();
            }
            else
            {
                List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
                string sSQL = GetSQL(sTempString);
                oDUProGuideLines = DUProGuideLine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                string sDyeingOrderIDs = string.Join(",", oDUProGuideLines.Select(x => x.DyeingOrderID));

                oDyeingOrderReports = DyeingOrderReport.Gets("SELECT * FROM View_DyeingOrderReport WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ") ORDER BY ContractorName", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDyeingOrderReports.Any())
                {
                    //string sDyeingOrderIDs = string.Join(",", oDyeingOrderReports.Select(x => x.DyeingOrderID));
                    oDUProGuideLineDetails_Receive = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) != " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID IN (" + sDyeingOrderIDs + ")) ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);
                    oDUProGuideLineDetails_Return = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) = " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID IN (" + sDyeingOrderIDs + ")) ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);

                    oDURequisitionDetails_SRS = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ")  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE ISNULL(ReceiveByID,0)<>0 and RequisitionType=" + (int)EnumInOutType.Receive + " ) ", (int)Session[SessionInfo.currentUserID]);
                    oDURequisitionDetails_SRM = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ")  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE ISNULL(ReceiveByID,0)<>0 and RequisitionType=" + (int)EnumInOutType.Disburse + " ) ", (int)Session[SessionInfo.currentUserID]);
                    oLotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ") OR DyeingOrderID_Out IN (" + sDyeingOrderIDs + ")", (int)Session[SessionInfo.currentUserID]);

                    oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets("SELECT * FROM View_DUDeliveryChallanDetail WHERE OrderID IN (" + sDyeingOrderIDs + ")", (int)Session[SessionInfo.currentUserID]);

                    if (oDUDeliveryChallanDetails.Any())
                        oDUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUDeliveryChallanDetailID IN (" + string.Join(",", oDUDeliveryChallanDetails.Select(x => x.DUDeliveryChallanDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
                }
            }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 16;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Yarn Receiving Report");
                sheet.Name = "Yarn Receiving Report";

                #region Sheet Columns

                //Order No	Order Date	Buyer Name	Style No	Yarn Type	Order Qty	Lot No	GRN Date	GRN Qty	SRM Qty	Transfer Qty	Received Due	Issued To S/W	Transfer Out	Return Qty	Balance	Date	Challan No	Lot No	Delivery Qty	Return Qty	

                sheet.Column(nStartCol++).Width = 8;//SL
                sheet.Column(nStartCol++).Width = 15;//Order No
                sheet.Column(nStartCol++).Width = 13;//Order Date
                sheet.Column(nStartCol++).Width = 30;//Buyer Name
                sheet.Column(nStartCol++).Width = 17;//Style No
                sheet.Column(nStartCol++).Width = 30;//Yarn Type
                sheet.Column(nStartCol++).Width = 12;//Order Qty

                sheet.Column(nStartCol++).Width = 15;//GRN No
                sheet.Column(nStartCol++).Width = 13;//GRN Date
                sheet.Column(nStartCol++).Width = 12;//GRN Qty
                sheet.Column(nStartCol++).Width = 20;//Lot

                sheet.Column(nStartCol++).Width = 15;//Challan No
                sheet.Column(nStartCol++).Width = 13;//Challan Date
                sheet.Column(nStartCol++).Width = 20;//Lot No
                sheet.Column(nStartCol++).Width = 12;//Delivery Qty 

                sheet.Column(nStartCol++).Width = 25;//Lot
                sheet.Column(nStartCol++).Width = 13;//Qty
                nEndCol = nStartCol;
                #endregion

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                nStartCol = 2;
                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Yarn Receiving Report";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                //if (GOrderDate > 0)
                //{
                //    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                //    cell.Value = "Order Date Between " + d1.ToString("dd MMM yyyy") + " to " + d2.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                //    cell.Style.WrapText = true;
                //    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.Size = 10;
                //    nRowIndex = nRowIndex + 1;
                //}

                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region HEADER
                nStartCol = 2;

                ExcelTool.FillCellMerge(ref sheet, "Order Info", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellMerge(ref sheet, "Received Status", nRowIndex, nRowIndex, nStartCol += 1, nStartCol += 3, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellMerge(ref sheet, "Delivery Status", nRowIndex, nRowIndex, nStartCol += 1, nStartCol += 3, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellMerge(ref sheet, "Balance", nRowIndex, nRowIndex, nStartCol += 1, nStartCol += 1, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                nRowIndex++;

                nStartCol = 2;

                var sHeaders = ("SL No,Order No,Order Date,Buyer Name,Style No,Yarn Type,Order Qty,GRN No,GRN Date,GRN Qty,Lot,Challan No,Challan Date,Lot No,Delivery Qty,Lot No,Qty").Split(',').ToList();

                foreach (var oHead in sHeaders)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = oHead; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                nRowIndex = nRowIndex + 1;
                #endregion


                #region DATA
                var data = oDyeingOrderReports.GroupBy(x => new { x.DyeingOrderID, x.ProductID }, (key, grp) => new
                {
                    DyeingOrderID = key.DyeingOrderID,
                    ProductID = key.ProductID,
                    Results = grp.ToList()
                });

                foreach (var oData in data)
                {
                    nSL = 0; int nMaxRow = 0; double nBalance = 0;
                    foreach (var oItem in oData.Results)
                    {
                        #region Gets Data Product Wise
                        List<DyeingOrderReport> oItem_oDyeingOrderReports = new List<DyeingOrderReport>();
                        List<LotParent> oItem_oLotParents_In = new List<LotParent>();
                        List<LotParent> oItem_oLotParents_Out = new List<LotParent>();
                        List<DURequisitionDetail> oItem_oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
                        List<DURequisitionDetail> oItem_oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
                        List<DUProGuideLineDetail> oItem_oDUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
                        List<DUProGuideLineDetail> oItem_oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
                        List<DUDeliveryChallanDetail> oItem_oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();

                        oItem_oDyeingOrderReports = oDyeingOrderReports.Where(p => p.DyeingOrderID == oItem.DyeingOrderID && p.ProductID == oItem.ProductID).ToList();
                        oItem_oLotParents_In = oLotParents.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oLotParents_Out = oLotParents.Where(x => x.DyeingOrderID_Out == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDURequisitionDetails_SRM = oDURequisitionDetails_SRM.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDURequisitionDetails_SRS = oDURequisitionDetails_SRS.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDUProGuideLineDetails_Return = oDUProGuideLineDetails_Return.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDUProGuideLineDetails_Receive = oDUProGuideLineDetails_Receive.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).OrderBy(z => z.LotID).ToList();
                        oItem_oDUDeliveryChallanDetails = oDUDeliveryChallanDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();

                        int nRowSpan = (oItem_oDyeingOrderReports == null) ? 0 : oItem_oDyeingOrderReports.Count();
                        //if (((oItem_oLotParents_In == null) ? 0 : oItem_oLotParents_In.Count()) > nRowSpan) nRowSpan = oItem_oLotParents_In.Count;
                        //if (((oItem_oLotParents_Out == null) ? 0 : oItem_oLotParents_Out.Count()) > nRowSpan) nRowSpan = oItem_oLotParents_Out.Count;
                        //if (((oItem_oDURequisitionDetails_SRM == null) ? 0 : oItem_oDURequisitionDetails_SRM.Count()) > nRowSpan) nRowSpan = oItem_oDURequisitionDetails_SRM.Count;
                        //if (((oItem_oDURequisitionDetails_SRS == null) ? 0 : oItem_oDURequisitionDetails_SRS.Count()) > nRowSpan) nRowSpan = oItem_oDURequisitionDetails_SRS.Count;
                        //if (((oItem_oDUProGuideLineDetails_Return == null) ? 0 : oItem_oDUProGuideLineDetails_Return.Count()) > nRowSpan) nRowSpan = oItem_oDUProGuideLineDetails_Return.Count;
                        if (((oItem_oDUProGuideLineDetails_Receive == null) ? 0 : oItem_oDUProGuideLineDetails_Receive.Count()) > nRowSpan) nRowSpan = oItem_oDUProGuideLineDetails_Receive.Count;
                        if (((oItem_oDUDeliveryChallanDetails == null) ? 0 : oItem_oDUDeliveryChallanDetails.Count()) > nRowSpan) nRowSpan = oItem_oDUDeliveryChallanDetails.Count;

                        nMaxRow = nRowSpan;
                        nRowSpan = nRowSpan - 1;
                        if (nRowSpan < 0) nRowSpan = 0;
                        #endregion

                        nSL++; nStartCol = 2;
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";

                        #region Order Info
                        ExcelTool.FillCellMerge(ref sheet, nSL.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.OrderDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.StyleNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.Qty.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                        #endregion

                        int nR = 0;
                        if (nMaxRow > 0)
                        {
                            int nTempRowIndex = nRowIndex; //0; 
                            int nLotID = -999; int nLotRowIndex = 0;
                            #region Receive & Issued info
                            foreach (var obj in oItem_oDUProGuideLineDetails_Receive)
                            {
                                nR++; nStartCol = 9;
                                nLotRowIndex = (oItem_oDUProGuideLineDetails_Receive.Where(x => x.LotID == obj.LotID).Count() - 1);
                                if (nLotRowIndex <= 0) nLotRowIndex = 0;

                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, obj.ReceiveDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, obj.Qty.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                                if (nLotID != obj.LotID)
                                    ExcelTool.FillCellMerge(ref sheet, obj.LotNo, nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                                else
                                    nStartCol = 13;

                                nBalance = obj.Qty;
                                if (nLotID != obj.LotID)
                                {
                                    nBalance = nBalance - oItem_oDURequisitionDetails_SRS.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                    if (oItem_oLotParents_Out != null)
                                    {
                                        nBalance = nBalance - oItem_oLotParents_Out.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                    }
                                    if (oItem_oDUProGuideLineDetails_Return != null)
                                    {
                                        nBalance = nBalance - oItem_oDUProGuideLineDetails_Return.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                    }
                                }
                                if (nLotID != obj.LotID)
                                {
                                    #region Balance
                                    ExcelTool.FillCellMerge(ref sheet, nBalance.ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    #endregion
                                }

                                nTempRowIndex++; nLotID = obj.LotID;
                            }
                            if (nR < nMaxRow)
                            {
                                while (nMaxRow != nR)
                                {
                                    nR++; nStartCol = 9;
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    nTempRowIndex++;
                                }
                            }

                            #endregion

                            #region Delivery Status
                            nR = 0; nTempRowIndex = 0;
                            foreach (var obj in oItem_oDUDeliveryChallanDetails)
                            {
                                nR++; nStartCol = 13;
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanNo, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanDate, false, ExcelHorizontalAlignment.Center, false, false);                                
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.Qty.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                                nTempRowIndex++;
                            }
                            if (nR < nMaxRow)
                            {
                                while (nMaxRow != nR)
                                {
                                    nR++; nStartCol = 13;
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    nTempRowIndex++;
                                }
                            }
                            #endregion

                            #region Balance
                            nStartCol = 17; nTempRowIndex = 0;
                            //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.ApproveLotNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                            ExcelTool.FillCellMerge(ref sheet, nBalance.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                            nTempRowIndex++;
                            #endregion
                        }
                        else
                        {
                            nStartCol = 9;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                        }

                        nRowIndex += nMaxRow;
                    }

                }
                #endregion


                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Yarn_Receiving_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }

        #endregion

        #region Get Company Logo
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
        public JsonResult Gets()
        {
            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            oDUProGuideLines = DUProGuideLine.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDUProGuideLines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
