using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using ESimSol.Reports;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace ESimSolFinancial.Controllers
{
    public class GUProductionOrderController : Controller
    {
        #region Declaration
        GUProductionOrder _oGUProductionOrder = new GUProductionOrder();
        GUProductionOrderDetail _oGUProductionOrderDetail = new GUProductionOrderDetail();
        List<GUProductionOrderDetail> _oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
        List<GUProductionOrder> _oGUProductionOrders = new List<GUProductionOrder>();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        Company _oCompany = new Company();
        TechnicalSheetThumbnail _oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
        TechnicalSheetImage _oTechnicalSheetImage = new TechnicalSheetImage();
        List<GUProductionProcedure> _oGUProductionProcedures = new List<GUProductionProcedure>();
        #endregion
        
        #region function
        #region Make Color Size ration from |Detial
        private List<ColorSizeRatio> MapColorSizeRationFromProductionOrderDetail(List<GUProductionOrderDetail> oGUProductionOrderDetails, List<TechnicalSheetSize> oSizes)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (GUProductionOrderDetail oItem in oGUProductionOrderDetails)
            {
                if (oItem.ColorID != nColorID)
                {
                    oColorSizeRatio = new ColorSizeRatio();
                    oColorSizeRatio.ColorID = oItem.ColorID;
                    oColorSizeRatio.ColorName = oItem.ColorName;
                    nCount = 0;
                    foreach (TechnicalSheetSize oSize in oSizes)
                    {
                        nCount++;
                        #region Set OrderQty
                        sPropertyName = "OrderQty" + nCount.ToString();
                        PropertyInfo prop = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oColorSizeRatio, GetQty(oSize.SizeCategoryID, oItem.ColorID, oGUProductionOrderDetails), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oGUProductionOrderDetails), null);
                        }
                        #endregion
                    }

                    #region ColorWiseTotal
                    sPropertyName = "ColorWiseTotal";
                    PropertyInfo propobjtotal = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjtotal && propobjtotal.CanWrite)
                    {
                        propobjtotal.SetValue(oColorSizeRatio, GetColorWiseTotalQty(oItem.ColorID, oGUProductionOrderDetails), null);
                    }
                    #endregion

                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }
        private double GetColorWiseTotalQty(int nColorID, List<GUProductionOrderDetail> oGUProductionOrderDetails)
        {
            double nTotalQty = 0;
            foreach (GUProductionOrderDetail oItem in oGUProductionOrderDetails)
            {
                if (oItem.ColorID == nColorID)
                {
                    nTotalQty = nTotalQty + oItem.Qty;
                }
            }
            return nTotalQty;
        }
        private double GetQty(int nSizeID, int nColorID, List<GUProductionOrderDetail> oGUProductionOrderDetails)
        {
            foreach (GUProductionOrderDetail oItem in oGUProductionOrderDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.Qty;
                }
            }
            return 0;
        }
        private int GetObjectID(int nSizeID, int nColorID, List<GUProductionOrderDetail> oGUProductionOrderDetails)
        {
            foreach (GUProductionOrderDetail oItem in oGUProductionOrderDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.GUProductionOrderDetailID;
                }
            }
            return 0;
        }
        #endregion
        #region Make POD from Color size Ratio
        private List<GUProductionOrderDetail> MapGUProductionOrderDetailFromColorSizeRation(List<ColorSizeRatio> oColorSizeRatios, List<TechnicalSheetSize> oSizes, GUProductionOrder oGUProductionOrder)
        {
            List<GUProductionOrderDetail> oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            GUProductionOrderDetail oGUProductionOrderDetail = new GUProductionOrderDetail();
            GUProductionOrderDetail oTempGUProductionOrderDetail = new GUProductionOrderDetail();
            int nCount = 0;
            foreach (ColorSizeRatio oItem in oColorSizeRatios)
            {
                nCount = 0;
                foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                {
                    nCount++;
                    oTempGUProductionOrderDetail = new GUProductionOrderDetail();
                    oTempGUProductionOrderDetail = GetObjIDAndQty(nCount, oItem);
                    if (oTempGUProductionOrderDetail.Qty > 0)
                    {
                        oGUProductionOrderDetail = new GUProductionOrderDetail();
                        oGUProductionOrderDetail.GUProductionOrderDetailID = oTempGUProductionOrderDetail.GUProductionOrderDetailID;
                        oGUProductionOrderDetail.GUProductionOrderID = oGUProductionOrder.GUProductionOrderID;
                        oGUProductionOrderDetail.ColorID = oItem.ColorID;
                        oGUProductionOrderDetail.SizeID = oTempTechnicalSheetSize.SizeCategoryID;
                        oGUProductionOrderDetail.UnitID = oGUProductionOrder.UnitID;
                        oGUProductionOrderDetail.Qty = oTempGUProductionOrderDetail.Qty;
                        oGUProductionOrderDetails.Add(oGUProductionOrderDetail);
                    }
                }
            }
            return oGUProductionOrderDetails;
        }
        private GUProductionOrderDetail GetObjIDAndQty(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            GUProductionOrderDetail oGUProductionOrderDetail = new GUProductionOrderDetail();
            switch (nCount)
            {
                case 1:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID1;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty1;
                    break;
                case 2:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID2;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty2;
                    break;
                case 3:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID3;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty3;
                    break;
                case 4:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID4;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty4;
                    break;
                case 5:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID5;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty5;
                    break;
                case 6:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID6;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty6;
                    break;
                case 7:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID7;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty7;
                    break;
                case 8:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID8;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty8;
                    break;
                case 9:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID9;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty9;
                    break;
                case 10:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID10;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty10;
                    break;
                case 11:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID11;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty11;
                    break;
                case 12:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID12;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty12;
                    break;
                case 13:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID13;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty13;
                    break;
                case 14:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID14;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty14;
                    break;
                case 15:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID15;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty15;
                    break;
                case 16:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID16;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty16;
                    break;
                case 17:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID17;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty17;
                    break;
                case 18:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID18;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty18;
                    break;
                case 19:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID19;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty19;
                    break;
                case 20:
                    oGUProductionOrderDetail.GUProductionOrderDetailID = oColorSizeRatio.OrderObjectID20;
                    oGUProductionOrderDetail.Qty = oColorSizeRatio.OrderQty20;
                    break;
            }
            return oGUProductionOrderDetail;
        }

        #endregion
        private List<GUProductionOrderDetail> GetShipmentScheduleDetails(List<ShipmentScheduleDetail> oShipmentScheduleDetails)
        {
            List<GUProductionOrderDetail> oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            GUProductionOrderDetail oGUProductionOrderDetail = new GUProductionOrderDetail();

            foreach (ShipmentScheduleDetail oItem in oShipmentScheduleDetails)
            {
                if (oItem.YetToPoductionOrderQty > 0)
                {
                    oGUProductionOrderDetail = new GUProductionOrderDetail();
                    oGUProductionOrderDetail.GUProductionOrderDetailID = 0;
                    oGUProductionOrderDetail.GUProductionOrderID = 0;
                    oGUProductionOrderDetail.ColorID = oItem.ColorID;
                    oGUProductionOrderDetail.SizeID = oItem.SizeID;
                    oGUProductionOrderDetail.UnitID = oItem.UnitID;
                    oGUProductionOrderDetail.Qty = oItem.YetToPoductionOrderQty;
                    oGUProductionOrderDetail.ColorName = oItem.ColorName;
                    oGUProductionOrderDetail.UnitName = oItem.UnitName;
                    oGUProductionOrderDetail.SizeName = oItem.SizeName;
                    oGUProductionOrderDetail.Symbol = "";
                    oGUProductionOrderDetails.Add(oGUProductionOrderDetail);
                }
            }


            return oGUProductionOrderDetails;
        }
        #endregion

        #region Views
        public ActionResult ViewODSOrderRecaps(int id, double ts)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap WHERE BuyerID="+id.ToString()+" AND OrderRecapID IN(SELECT ODS.OrderRecapID FROM View_OrderDistributionSheetDetail AS ODS WHERE ISNULL(ODS.YetToProduction,0)>0) ORDER BY OrderRecapID"; // Load thats Recaps which was include in ODS but not Produciton
            _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            @ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oOrderRecaps);
        }

        public ActionResult ViewGUProductionOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oGUProductionOrders = new List<GUProductionOrder>();
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GUProductionOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.StatusList = EnumObject.jGets(typeof(EnumGUProductionOrderStatus));
            string sSql = "SELECT * FROM View_User WHERE UserID IN (SELECT Distinct ApprovedBy FROM View_CostSheet)";
            ViewBag.Employees = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            return View(_oGUProductionOrders);
        }

        public ActionResult ViewGUProductionOrder(int id, double ts)
        {
            _oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            _oGUProductionOrder = new GUProductionOrder();
            if (id > 0)
            {
                _oGUProductionOrder = _oGUProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.YetToPoductionQty = _oGUProductionOrder.YetToPoductionQty + _oGUProductionOrder.TotalQty;
                _oGUProductionOrderDetails = GUProductionOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.ColorSizeRatios = MapColorSizeRationFromProductionOrderDetail(_oGUProductionOrderDetails, _oGUProductionOrder.TechnicalSheetSizes);
            }
           
            return View(_oGUProductionOrder);
        }

        public ActionResult ViewGUProductionOrderProgressReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oOrderRecaps = new List<OrderRecap>();
            //string sSQL ="SELECT * FROM View_OrderRecap WHERE OrderRecapID IN (848,558,858,177)";
            //_oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oOrderRecaps);
        }
        
        [HttpPost]
       

       public JsonResult GetODSOrderRecaps(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            try
            {
                string sSQL = "SELECT * FROM View_OrderRecap WHERE BuyerID=" + oOrderRecap.BuyerID.ToString() + " AND ShipmentDate > DATEADD(MONTH,-1,GETDATE())";
                if (!string.IsNullOrEmpty(oOrderRecap.OrderRecapNo))
                {
                    sSQL += " AND OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo+"%'";
                }
                sSQL += " ORDER BY OrderRecapID";

                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                OrderRecap oTempOrderRecap=new OrderRecap();
                oOrderRecap.ErrorMessage = "ex";
                _oOrderRecaps.Add(oTempOrderRecap);
            }

             var jsonResult = Json(_oOrderRecaps, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }




        [HttpGet]
        public JsonResult GetGUProductionOrderDetails(int id, int TSID, double ts)
        {
            _oGUProductionOrder = new GUProductionOrder();
            _oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            List<ShipmentScheduleDetail> oShipmentScheduleDetails = new List<ShipmentScheduleDetail>();
           try
            {
                if (id > 0)
                {
                    oShipmentScheduleDetails = ShipmentScheduleDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                    _oGUProductionOrderDetails = GetShipmentScheduleDetails(oShipmentScheduleDetails);
                    _oGUProductionOrder.TechnicalSheetSizes = TechnicalSheetSize.Gets(TSID, (int)Session[SessionInfo.currentUserID]);
                    _oGUProductionOrder.ColorSizeRatios = MapColorSizeRationFromProductionOrderDetail(_oGUProductionOrderDetails, _oGUProductionOrder.TechnicalSheetSizes);
                }
            }
            catch (Exception ex)
            {
                _oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Image
        public Image GetCoverImage(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetThumbnail.ThumbnailImage);
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
        public ActionResult GetCoverImageInBase64(int id)
        {

            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage == null)
            {
                oTechnicalSheetThumbnail.ThumbnailImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oTechnicalSheetThumbnail.ThumbnailImage) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save
        [HttpPost]
        public JsonResult Save(GUProductionOrder oGUProductionOrder)
        {
            _oGUProductionOrder = new GUProductionOrder();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                _oGUProductionOrder = oGUProductionOrder;
                _oGUProductionOrder.OrderStatus = (EnumGUProductionOrderStatus)_oGUProductionOrder.nOrderStatus;
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.GUProductionOrderDetails = MapGUProductionOrderDetailFromColorSizeRation(oGUProductionOrder.ColorSizeRatios, oTechnicalSheetSizes, oGUProductionOrder);
                _oGUProductionOrder = _oGUProductionOrder.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendToProduction(GUProductionOrder oGUProductionOrder)
        {
            _oGUProductionOrder = new GUProductionOrder();
            try
            {
                if (oGUProductionOrder.GUProductionOrderID<=0)
                {
                    _oGUProductionOrder.ErrorMessage = "Invalid Production Order!";
                }                
                else if (oGUProductionOrder.nOrderStatus != (int)EnumGUProductionOrderStatus.Approved)
                {
                    _oGUProductionOrder.ErrorMessage = "Your Selected Order is not Approved!";
                }
                else
                {
                    _oGUProductionOrder = _oGUProductionOrder.SendToProducton(oGUProductionOrder.GUProductionOrderID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      
        #endregion

        #region Update Tolerance With Winding Status
        public ActionResult ViewUpdateTolarence(int id, double ts)
        {
            _oGUProductionOrder = new GUProductionOrder();
            if (id > 0)
            {
                _oGUProductionOrder = _oGUProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return PartialView(_oGUProductionOrder);
        }
        [HttpPost]
        public JsonResult UpdateToleranceWithWindingStatus(GUProductionOrder oGUProductionOrder)
        {
            _oGUProductionOrder = new GUProductionOrder();
            try
            {
                _oGUProductionOrder = oGUProductionOrder;
                _oGUProductionOrder = _oGUProductionOrder.UpdateToleranceWithStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region HTTP Delete
        [HttpGet]
        public JsonResult Delete(int nPOID)
        {
            string smessage = "";
            try
            {
                GUProductionOrder oGUProductionOrder = new GUProductionOrder();
                smessage = oGUProductionOrder.Delete(nPOID, (int)Session[SessionInfo.currentUserID]);

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

        #region Waiting Search

        [HttpGet]
        public JsonResult WaitingSearch(int nStatusExtra)
        {
            _oGUProductionOrders = new List<GUProductionOrder>();
            string sSql = "SELECT * from View_GUProductionOrder Where OrderStatus =";
            try
            {
                if (nStatusExtra == 1)
                {
                    sSql += (int)EnumGUProductionOrderStatus.Initialized;
                }
                else
                {
                    sSql += (int)EnumGUProductionOrderStatus.Req_For_Approval;
                }
                #region User Set
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {
                    
                    sSql+= " AND BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
                }
                #endregion

                _oGUProductionOrders = GUProductionOrder.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Production  Order  Approve
        public ActionResult POApprove(int nPOID)
        {
            _oGUProductionOrder = new GUProductionOrder();
            if (nPOID > 0)
            {
                _oGUProductionOrder = _oGUProductionOrder.Get(nPOID, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.YetToPoductionQty = _oGUProductionOrder.YetToPoductionQty + _oGUProductionOrder.TotalQty;
                _oGUProductionOrderDetails = GUProductionOrderDetail.Gets(nPOID, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.ColorSizeRatios = MapColorSizeRationFromProductionOrderDetail(_oGUProductionOrderDetails, _oGUProductionOrder.TechnicalSheetSizes);
            }
            return View(_oGUProductionOrder);
        }
        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(GUProductionOrder oGUProductionOrder)
        {
            _oGUProductionOrder = new GUProductionOrder();
            try
            {
                if (oGUProductionOrder.ActionTypeExtra == "RequestForApproval")
                {

                    oGUProductionOrder.ActionType = EnumOPOActionType.RequestForApproval;

                }
                else if (oGUProductionOrder.ActionTypeExtra == "UndoRequest")
                {

                    oGUProductionOrder.ActionType = EnumOPOActionType.UndoRequest;

                }
                else if (oGUProductionOrder.ActionTypeExtra == "Approve")
                {
                    
                        oGUProductionOrder.ActionType = EnumOPOActionType.Approve;
                }
                else if (oGUProductionOrder.ActionTypeExtra == "NotApprove")
                {

                        oGUProductionOrder.ActionType = EnumOPOActionType.Cancel;

                }
                else if (oGUProductionOrder.ActionTypeExtra == "UndoApprove")
                {

                    oGUProductionOrder.ActionType = EnumOPOActionType.UndoApprove;
                }

                else if (oGUProductionOrder.ActionTypeExtra == "InActive")
                {

                    oGUProductionOrder.ActionType = EnumOPOActionType.InActive;
                }
                else if (oGUProductionOrder.ActionTypeExtra == "Close")
                {

                    oGUProductionOrder.ActionType = EnumOPOActionType.Cancel;
                }

                else if (oGUProductionOrder.ActionTypeExtra == "Active")
                {

                    oGUProductionOrder.ActionType = EnumOPOActionType.Active;
                }
                oGUProductionOrder = SetPOOrderStatus(oGUProductionOrder);
                _oGUProductionOrder = oGUProductionOrder.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private GUProductionOrder SetPOOrderStatus(GUProductionOrder oGUProductionOrder)//Set EnumOrderStatus Value
        {
            switch (oGUProductionOrder.nOrderStatus)
            {
                case 1:
                    {
                        oGUProductionOrder.OrderStatus = EnumGUProductionOrderStatus.Initialized;
                        break;
                    }
                case 2:
                    {
                        oGUProductionOrder.OrderStatus = EnumGUProductionOrderStatus.Req_For_Approval;
                        break;
                    }
                case 3:
                    {
                        oGUProductionOrder.OrderStatus = EnumGUProductionOrderStatus.Approved;
                        break;
                    }

                case 4:
                    {
                        oGUProductionOrder.OrderStatus = EnumGUProductionOrderStatus.InActive;
                        break;
                    }
                case 5:
                    {
                        oGUProductionOrder.OrderStatus = EnumGUProductionOrderStatus.Cancel;
                        break;
                    }

            }

            return oGUProductionOrder;
        }
        #endregion

        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch()
        {

            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            oGUProductionOrder.EmployeeList = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oGUProductionOrder);
        }
        
        [HttpGet]
        public JsonResult Gets(string sTemp, double ts)
        {
            List<GUProductionOrder> oGUProductionOrders = new List<GUProductionOrder>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oGUProductionOrders = GUProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGUProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dODSStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dODSEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            int nShipmentDateCom = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dShipmentStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dShipmentEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            string sStNo = sTemp.Split('~')[6];
            string sPONo = sTemp.Split('~')[7];
            int nBuyerID = Convert.ToInt32(sTemp.Split('~')[8]);
            int nFactoryID = Convert.ToInt32(sTemp.Split('~')[9]);
            int nMerchandiser = Convert.ToInt32(sTemp.Split('~')[10]);
            string sStatusIDs =  sTemp.Split('~')[11];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[12]);


            string sReturn1 = "SELECT * FROM View_GUProductionOrder";
            string sReturn = "";

            #region Style No

            if (sStNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleNo = '" + sStNo + "'";

            }
            #endregion

            #region ONS No

            if (sPONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  GUProductionOrderNo ='" + sPONo + "'";
            }
            #endregion

            #region Buyer Name

            if (nBuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID = " + nBuyerID;
            }
            #endregion

            #region Merchandiser Name

            if (nMerchandiser != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID = " + nMerchandiser;
            }
            #endregion

            #region Factory Name

            if (nFactoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionFactoryID = " + nFactoryID;
            }

            #endregion

            #region Order Status

            if (!string.IsNullOrEmpty(sStatusIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderStatus IN (" + sStatusIDs+ ")";
            }

            #endregion

            #region nBUID

            if (nBUID!= 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " (BUID = " + nBUID + " OR ISNULL(FBUID,0) = " + nBUID + " ) OR ISNULL(FBUID,0) = 0";
            }
            #endregion


            #region Date Wise

            #region Order Date
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate = '" + dODSStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate != '" + dODSStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate > '" + dODSStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate < '" + dODSStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate>= '" + dODSStartDate.ToString("dd MMM yyyy") + "' AND OrderDate < '" + dODSEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate< '" + dODSStartDate.ToString("dd MMM yyyy") + "' OR OrderDate > '" + dODSEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region Shipment Date
            if (nShipmentDateCom > 0)
            {
                if (nShipmentDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate< '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion
            #endregion

            #region Order Type
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " OrderType ="+ ((int)EnumOrderType.BulkOrder).ToString();
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

        #region Setup Priduction Procedure
        public ActionResult ViewGUProductionProcedure(int id, double ts)
        {
            _oGUProductionOrder = new GUProductionOrder();
            if (id > 0)
            {
                _oGUProductionOrder = _oGUProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionOrder.GUProductionProcedures = GUProductionProcedure.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oGUProductionOrder.ProductionSteps = ProductionStep.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oGUProductionOrder);
        }

        [HttpPost]
        public JsonResult SaveGUProductionProcedure(GUProductionOrder oGUProductionOrder)
        {
            _oGUProductionProcedures = new List<GUProductionProcedure>();
            try
            {
                _oGUProductionProcedures = GUProductionProcedure.Save(oGUProductionOrder, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                GUProductionProcedure oGUProductionProcedure = new GUProductionProcedure();
                _oGUProductionProcedures = new List<GUProductionProcedure>();
                oGUProductionProcedure.ErrorMessage = ex.Message;
                _oGUProductionProcedures.Add(oGUProductionProcedure);
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionProcedures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult PintProdcutionProgressReport(string sIDs) // sIDs  is Order Recap ID
        {

            _oGUProductionOrder = new GUProductionOrder();
            _oCompany = new Company();

            _oGUProductionOrder = _oGUProductionOrder.ProductionProgresReport(sIDs, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.Company = _oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.Company.CompanyLogo = GetCompanyLogo(_oGUProductionOrder.Company);
            if (_oGUProductionOrder.ErrorMessage == "")
            {
                rptProdcutionProgressReport oReport = new rptProdcutionProgressReport();
                byte[] abytes = oReport.PrepareReport(_oGUProductionOrder);
                return File(abytes, "application/pdf");
            }
            else
            {
                string sMessage = "There is no Data for Selected Date ";

                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }
        public ActionResult PODPrintList(string sID)
        {
            _oGUProductionOrder = new GUProductionOrder();
            _oCompany = new Company();
            string sSQL ="SELECT * FROM View_GUProductionOrder WHERE GUProductionOrderID IN ("+sID+")";
            _oGUProductionOrder.GUProductionOrders = GUProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.Company = _oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.Company.CompanyLogo = GetCompanyLogo(_oGUProductionOrder.Company);
            rptGUProductionOrderList oReport = new rptGUProductionOrderList();
            byte[] abytes = oReport.PrepareReport(_oGUProductionOrder);
            return File(abytes, "application/pdf");
        }
    
        public ActionResult PODPreview(int id)
        {
            _oGUProductionOrder = new GUProductionOrder();
            Company oCompany = new Company();
            _oGUProductionOrder = _oGUProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.GUProductionOrderDetails = GUProductionOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.TechnicalSheetImage = _oTechnicalSheetImage.GetFrontImage(_oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oGUProductionOrder.Company = oCompany;
            rptGUProductionOrder oReport = new rptGUProductionOrder();
            
            byte[] abytes = oReport.PrepareReport(_oGUProductionOrder);
            return File(abytes, "application/pdf");   
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

        #endregion Print

        #region Search Style OR Order  by Press Enter
        [HttpGet]
        public JsonResult SearchStyleAndOrder(string sTempData, bool bIsStyle, double ts)
        {
            _oGUProductionOrders = new List<GUProductionOrder>();
            string sSQL = "";
            if (bIsStyle == true)
            {
                sSQL = "SELECT * FROM View_GUProductionOrder WHERE StyleNo LIKE ('%" + sTempData + "%')";
            }
            else
            {
                sSQL = "SELECT * FROM View_GUProductionOrder WHERE GUProductionOrderNo LIKE ('%" + sTempData + "%')";
            }
            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {

                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            try
            {
                GUProductionOrder oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrders = GUProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
                _oGUProductionOrders.Add(_oGUProductionOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Sale Order
        [HttpPost]
        public JsonResult GetGUProductionOrders(GUProductionOrder oGUProductionOrder)
        {
            _oGUProductionOrders = new List<GUProductionOrder>();
            string sSQL = "";
            if (oGUProductionOrder.GUProductionOrderNo != "")
            {
                sSQL = "SELECT * FROM View_GUProductionOrder WHERE GUProductionOrderNo LIKE ('%" + oGUProductionOrder.GUProductionOrderNo + "%')";
            }
            else
            {
                sSQL = "SELECT * FROM View_GUProductionOrder";
            }

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            try
            {
                _oGUProductionOrders = GUProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
                _oGUProductionOrders.Add(_oGUProductionOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET GetPOD
        [HttpGet]
        public JsonResult GetPOD(int id, double ts)
        {
            _oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            try
            {
                _oGUProductionOrderDetails = GUProductionOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrderDetail = new GUProductionOrderDetail();
                _oGUProductionOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}
