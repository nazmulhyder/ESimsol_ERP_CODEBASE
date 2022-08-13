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
using System.Dynamic;

namespace ESimSolFinancial.Controllers
{
    public class ProductionManagementController : Controller
    {
        #region Declaration
        GUProductionOrder _oGUProductionOrder = new GUProductionOrder();
        List<GUProductionOrder> _oGUProductionOrders = new List<GUProductionOrder>();
        GUProductionProcedure _oGUProductionProcedure = new GUProductionProcedure();
        List<GUProductionProcedure> _oGUProductionProcedures = new List<GUProductionProcedure>();
        GUProductionTracingUnit _oGUProductionTracingUnit = new GUProductionTracingUnit();
        List<GUProductionTracingUnit> _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
        GUProductionTracingUnitDetail _oGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();
        List<GUProductionTracingUnitDetail> _oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
        PTUTransection _oPTUTransection = new PTUTransection();
        List<PTUTransection> _oPTUTransections = new List<PTUTransection>();
        ProductionTracking _oProductionTracking = new ProductionTracking();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        #endregion

        #region Functions
        private List<GUProductionProcedure> GetGUProductionProcedures(int nGUProductionOrderID)
        {
            List<GUProductionProcedure> oGUProductionProcedures = new List<GUProductionProcedure>();
            foreach (GUProductionProcedure oItem in _oGUProductionProcedures)
            {
                if (oItem.GUProductionOrderID == nGUProductionOrderID)
                {
                    oGUProductionProcedures.Add(oItem);
                }
            }
            return oGUProductionProcedures;
        }

        private List<GUProductionTracingUnit> GetGUProductionTracingUnits(int nGUProductionOrderID)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            foreach (GUProductionTracingUnit oItem in _oGUProductionTracingUnits)
            {
                if (oItem.GUProductionOrderID == nGUProductionOrderID)
                {
                    oItem.GUProductionTracingUnitDetails = this.GetGUProductionTracingUnitDetails(oItem.GUProductionTracingUnitID);
                    oGUProductionTracingUnits.Add(oItem);
                }
            }
            return oGUProductionTracingUnits;
        }

        #region ProductionExecution Dynamic Object
        private List<dynamic> GetStepsByColorRatio(GUProductionTracingUnit oGUProductionTracingUnit)
        {
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            _oGUProductionTracingUnit.GUProductionProcedures = oGUProductionTracingUnit.GUProductionProcedures;//?
            _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            _oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            GUProductionTracingUnitDetail oPreviousGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();

            _oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionTracingUnitDetails = GUProductionTracingUnitDetail.Gets("SELECT * FROM View_GUProductionTracingUnitDetail AS PTUD WHERE PTUD.GUProductionTracingUnitID IN (SELECT GUProductionTracingUnitID FROM GUProductionTracingUnit WHERE GUProductionOrderID=" + oGUProductionTracingUnit.GUProductionOrderID + ")", (int)Session[SessionInfo.currentUserID]);
            double nTolerancePercent = oGUProductionTracingUnit.GUProductionOrder.ToleranceInPercent;

            //StepList
            List<dynamic> oDynamics = new List<dynamic>();

            foreach (GUProductionTracingUnit oColor in _oGUProductionTracingUnits)
            {
                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;

                expobj.Add("ColorName", oColor.ColorName);
                expobj.Add("OrderQty", oColor.OrderQty);
                double nPreviousStepExecutionQty = 0;
                foreach (GUProductionProcedure oStep in oGUProductionTracingUnit.GUProductionProcedures)
                {
                    _oGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();
                    if (_oGUProductionTracingUnitDetails.Count > 0)
                    {
                        _oGUProductionTracingUnitDetail = _oGUProductionTracingUnitDetails.Where(p => p.ProductionStepID == oStep.ProductionStepID && p.GUProductionTracingUnitID == oColor.GUProductionTracingUnitID).FirstOrDefault();
                        if (_oGUProductionTracingUnitDetail == null)
                            _oGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();
                    }
                    expobj.Add(oStep.StepName + "-TQ", (_oGUProductionTracingUnitDetail.GUProductionTracingUnitDetailID.ToString() + '~'
                              + _oGUProductionTracingUnitDetail.ExecutionQty.ToString()
                              + '~' + oStep.StepName + '~' + oColor.ColorName + '~' + "true"));
                    expobj.Add(oStep.StepName + "-EQ", 0);
                    if (oStep.Sequence == 1)
                    {
                        expobj.Add(oStep.StepName + "-YTQ", ((oColor.OrderQty * nTolerancePercent) / 100 + oColor.OrderQty) - _oGUProductionTracingUnitDetail.ExecutionQty);
                    }
                    else
                    {
                        int nPriviousStepID = oGUProductionTracingUnit.GUProductionProcedures.Where(x => x.Sequence == oStep.Sequence - 1).First().ProductionStepID;
                        if (_oGUProductionTracingUnitDetails.Count > 0)
                        {
                            oPreviousGUProductionTracingUnitDetail = _oGUProductionTracingUnitDetails.Where(p => p.ProductionStepID == nPriviousStepID && p.GUProductionTracingUnitID == oColor.GUProductionTracingUnitID).FirstOrDefault();
                            if (oPreviousGUProductionTracingUnitDetail == null)
                            {
                                oPreviousGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();
                            }
                            nPreviousStepExecutionQty = oPreviousGUProductionTracingUnitDetail.ExecutionQty;
                        }
                        expobj.Add(oStep.StepName + "-YTQ", nPreviousStepExecutionQty - _oGUProductionTracingUnitDetail.ExecutionQty);
                    }
                    expobj.Add(oStep.StepName + "-ID", oStep.ProductionStepID);
                    expobj.Add(oStep.StepName + "-TID", oColor.GUProductionTracingUnitID);
                    expobj.Add(oStep.StepName + "-OQ", oColor.OrderQty);
                    expobj.Add(oStep.StepName + "-MU", oColor.MeasurementUnitID);
                    nPreviousStepExecutionQty = 0;
                }
                oDynamics.Add(expobj);
            }
            return oDynamics;
        }
        #endregion

        private List<GUProductionTracingUnitDetail> GetGUProductionTracingUnitDetails(int nGUProductionTracingUnitID)
        {
            List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            foreach (GUProductionTracingUnitDetail oItem in _oGUProductionTracingUnitDetails)
            {
                if (oItem.GUProductionTracingUnitID == nGUProductionTracingUnitID)
                {
                    oItem.PTUTransections = this.GetPTUTransections(oItem.GUProductionTracingUnitDetailID);
                    oGUProductionTracingUnitDetails.Add(oItem);
                }
            }
            return oGUProductionTracingUnitDetails;
        }

        private List<PTUTransection> GetPTUTransections(int nGUProductionTracingUnitDetailID)
        {
            List<PTUTransection> oPTUTransections = new List<PTUTransection>();
            foreach (PTUTransection oItem in _oPTUTransections)
            {
                if (oItem.GUProductionTracingUnitDetailID == nGUProductionTracingUnitDetailID)
                {
                    oPTUTransections.Add(oItem);
                }
            }
            return oPTUTransections;
        }

        private OrderRecap GetOrderRecap(int nOrderRecapID)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            foreach (OrderRecap oItem in _oOrderRecaps)
            {
                if (oItem.OrderRecapID == nOrderRecapID)
                {
                    return oItem;
                }
            }
            return oOrderRecap;
        }
        #endregion

        #region New Function
        #region Make Color Size ration from |Detial
        private List<ColorSizeRatio> MapColorSizeRationFromProductionTracingUnits(List<GUProductionTracingUnit> oGUProductionTracingUnits, List<TechnicalSheetSize> oSizes, int nListFor, int nSequence)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (GUProductionTracingUnit oItem in oGUProductionTracingUnits)
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
                            prop.SetValue(oColorSizeRatio, GetQty(oSize.SizeCategoryID, oItem.ColorID, oGUProductionTracingUnits, nListFor, nSequence), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oGUProductionTracingUnits), null);
                        }
                        #endregion
                    }
                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }
        //nListFor: 1:Privoius; 2:today 3:YetTo
        private double GetQty(int nSizeID, int nColorID, List<GUProductionTracingUnit> oGUProductionTracingUnits, int nListFor, int nSequence)
        {
            foreach (GUProductionTracingUnit oItem in oGUProductionTracingUnits)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    if (nListFor == 2)//Today list 
                    {
                        return 0;
                    }
                    if (nListFor == 3 && nSequence == 1)//Yet to list
                    {
                        return oItem.YetToExecutionQty;
                    } if (nListFor == 3 && nSequence != 1)//Yet to list
                    {
                        return oItem.PreviousStepExecutionQty - oItem.ExecutionQty;
                    }
                    
                    if (nListFor == 1 && nSequence==1)//Privioius list and Frist Sequence
                    {
                        return oItem.OrderQty;
                    }
                    if (nListFor == 1 && nSequence != 1)//Privioius list and Not Frist Sequence
                    {
                        return oItem.ExecutionQty;
                    }
                    
                }
            }
            return 0;
        }
        private int GetObjectID(int nSizeID, int nColorID, List<GUProductionTracingUnit> oGUProductionTracingUnits)
        {
            foreach (GUProductionTracingUnit oItem in oGUProductionTracingUnits)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.GUProductionTracingUnitID;
                }
            }
            return 0;
        }
        #endregion

        #region Make POD from Color size Ratio
        private List<PTUTransection> MapPTUTransectionFromColorSizeRation(List<ColorSizeRatio> oColorSizeRatios, List<TechnicalSheetSize> oSizes, GUProductionTracingUnit oGUProductionTracingUnit)
        {
            List<PTUTransection> oPTUTransections = new List<PTUTransection>();
            PTUTransection oPTUTransection = new PTUTransection();
            PTUTransection oTempPTUTransection = new PTUTransection();
            int nCount = 0;
            foreach (ColorSizeRatio oItem in oColorSizeRatios)
            {
                nCount = 0;
                foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                {
                    nCount++;
                    oTempPTUTransection = new PTUTransection();
                    oTempPTUTransection = GetObjIDAndQty(nCount, oItem);
                    if (oTempPTUTransection.Quantity > 0)
                    {
                        oPTUTransection = new PTUTransection();
                        oPTUTransection.PTUTransectionID = 0;
                        oPTUTransection.GUProductionTracingUnitID = oTempPTUTransection.GUProductionTracingUnitID;
                        oPTUTransection.GUProductionTracingUnitDetailID = 0;
                        oPTUTransection.ProductionStepID = oGUProductionTracingUnit.ProductionStepID;
                        oPTUTransection.PLineConfigureID = oGUProductionTracingUnit.PLineConfigureID;
                        oPTUTransection.MeasurementUnitID = oGUProductionTracingUnit.MeasurementUnitID;
                        oPTUTransection.Quantity = oTempPTUTransection.Quantity;
                        oPTUTransection.OperationDate = oGUProductionTracingUnit.OperationDate;
                        oPTUTransection.ActualWorkingHour = oGUProductionTracingUnit.ActualWorkingHour;
                        oPTUTransection.UseHelper = oGUProductionTracingUnit.UseHelper;
                        oPTUTransection.UseOperator = oGUProductionTracingUnit.UseOperator;
                        oPTUTransection.Note = oGUProductionTracingUnit.ProductionOperationNote;
                        oPTUTransection.IsUsesValueUpdate = oGUProductionTracingUnit.IsUsesValueUpdate;
                        oPTUTransection.StartDate = oGUProductionTracingUnit.OperationDate;
                        oPTUTransections.Add(oPTUTransection);
                    }
                }
            }
            return oPTUTransections;
        }
        private PTUTransection GetObjIDAndQty(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            PTUTransection oPTUTransection = new PTUTransection();
            switch (nCount)
            {
                case 1:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID1;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty1;
                    break;
                case 2:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID2;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty2;
                    break;
                case 3:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID3;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty3;
                    break;
                case 4:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID4;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty4;
                    break;
                case 5:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID5;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty5;
                    break;
                case 6:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID6;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty6;
                    break;
                case 7:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID7;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty7;
                    break;
                case 8:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID8;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty8;
                    break;
                case 9:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID9;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty9;
                    break;
                case 10:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID10;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty10;
                    break;
                case 11:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID11;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty11;
                    break;
                case 12:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID12;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty12;
                    break;
                case 13:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID13;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty13;
                    break;
                case 14:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID14;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty14;
                    break;
                case 15:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID15;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty15;
                    break;
                case 16:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID16;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty16;
                    break;
                case 17:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID17;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty17;
                    break;
                case 18:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID18;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty18;
                    break;
                case 19:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID19;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty19;
                    break;
                case 20:
                    oPTUTransection.GUProductionTracingUnitID = oColorSizeRatio.OrderObjectID20;
                    oPTUTransection.Quantity = oColorSizeRatio.OrderQty20;
                    break;
            }
            return oPTUTransection;
        }

        #endregion

        #region GetPTUHistoryColorSizeRatios
        private List<ColorSizeRatio> GetPTUHistoryColorSizeRatios(List<PTUTransection> oPTUTransections, List<TechnicalSheetSize> oSizes)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (PTUTransection oItem in oPTUTransections)
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
                        sPropertyName = "BarCode" + nCount.ToString();
                        PropertyInfo prop = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oColorSizeRatio, GetPTUQty(oSize.SizeCategoryID, oItem.ColorID, oPTUTransections), null);
                        }
                        #endregion

                        //#region Set ObjectID
                        //sPropertyName = "OrderObjectID" + nCount.ToString();
                        //PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        //if (null != propobj && propobj.CanWrite)
                        //{
                        //    propobj.SetValue(oColorSizeRatio, GetPTUObjectID(oSize.SizeCategoryID, oItem.ColorID, oPTUTransections), null);
                        //}
                        //#endregion
                    }
                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }

        private string GetPTUQty(int nSizeID, int nColorID, List<PTUTransection> oPTUTransections)
        {
            foreach (PTUTransection oItem in oPTUTransections)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.Quantity+ "~" + oItem.GUProductionTracingUnitDetailID.ToString();
                }
            }
            return "0~0";
        }
       
        #endregion
        #endregion
        #region Production Executions
        public ActionResult ViewProductionExecution(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GUProductionOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            return View(_oGUProductionTracingUnit);
        }

        public ActionResult ViewProductionExecutionTemp(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GUProductionOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            return View(_oGUProductionTracingUnit);
        }
        
        public ActionResult ViewGUProductionOrderPiker(int id, double ts)
        {
            string sSQL = "SELECT * FROM View_GUProductionOrder WHERE OrderStatus=4 AND BuyerID=" + id.ToString() + " ORDER BY GUProductionOrderID";
            _oGUProductionOrders = GUProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);            
            return PartialView(_oGUProductionOrders);
        }

        [HttpPost] //TempProductionExecution
        public JsonResult GetGUProductionOrders(GUProductionOrder oGUProductionOrder)
        {
            string sSQL = "SELECT * FROM View_GUProductionOrder WHERE OrderStatus="+(int)EnumGUProductionOrderStatus.InProduction;
            if (!string.IsNullOrEmpty(oGUProductionOrder.OrderRecapNo))
            {
                sSQL += " AND OrderRecapNo LIKE '%" + oGUProductionOrder .OrderRecapNo+ "%'";
            }
            sSQL += " ORDER BY GUProductionOrderID";
            _oGUProductionOrders = GUProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGUProductionOrdersForPicker(OrderRecap oOrderRecap)
        {
            _oGUProductionOrders = new List<GUProductionOrder>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            string sTechnicalSheetIDs = oOrderRecap.ErrorMessage.Split('~')[0];
            string sBuyerIDs = oOrderRecap.ErrorMessage.Split('~')[1];
            int nShipmentDateCom = Convert.ToInt32(oOrderRecap.ErrorMessage.Split('~')[2]);
            DateTime dShipmentStartDate = Convert.ToDateTime(oOrderRecap.ErrorMessage.Split('~')[3]);
            DateTime dShipmentEndDate = Convert.ToDateTime(oOrderRecap.ErrorMessage.Split('~')[4]);

            string sSql = "SELECT * FROM View_GUProductionOrder WHERE OrderStatus=" + (int)EnumGUProductionOrderStatus.InProduction;
            try
            {

                if (!string.IsNullOrEmpty(sTechnicalSheetIDs))
                {
                    sSql += " AND TechnicalSheetID IN (" + sTechnicalSheetIDs + ")";
                }
                if (!string.IsNullOrEmpty(sBuyerIDs))
                {
                    sSql += " AND BuyerID IN (" + sBuyerIDs + ")";
                }
                if (nShipmentDateCom != 0)
                {
                    if (nShipmentDateCom == 1)
                    {
                        sSql += " AND ShipmentDate = '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 2)
                    {
                        sSql += " AND ShipmentDate != '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 3)
                    {
                        sSql += " AND ShipmentDate > '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 4)
                    {
                        sSql += " AND ShipmentDate < '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 5)
                    {
                        sSql += " AND ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 6)
                    {
                        sSql += " AND ShipmentDate< '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                }
                if (!string.IsNullOrEmpty(oOrderRecap.OrderRecapNo))
                {
                    sSql += " AND OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo + "%'";
                }
                if (oOrderRecap.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSql += " AND BUID = " + oOrderRecap.BUID;
                }
                _oGUProductionOrders = GUProductionOrder.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
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


        [HttpPost]
        public JsonResult TempCommitProductionExecution(GUProductionTracingUnit oGUProductionTracingUnit)
        {
            List<dynamic> oDynamics = new List<dynamic>();
            _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            _oPTUTransections = new List<PTUTransection>();
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            _oGUProductionOrder = new GUProductionOrder();

            try
            {
                _oPTUTransections = oGUProductionTracingUnit.PTUTransections;
                if (_oPTUTransections.Count <= 0)
                {
                    _oGUProductionTracingUnit = new GUProductionTracingUnit();
                    _oGUProductionTracingUnit.ErrorMessage = "Please enter at least one item Qty!";
                }
                else
                {
                    _oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
                    try
                    {
                        _oGUProductionTracingUnits = GUProductionTracingUnit.CommitProductionExecution(_oPTUTransections, (int)Session[SessionInfo.currentUserID]);

                        _oGUProductionTracingUnit.GUProductionOrderID = oGUProductionTracingUnit.GUProductionOrderID;
                        _oGUProductionTracingUnit.GUProductionProcedures = GUProductionProcedure.Gets(oGUProductionTracingUnit.GUProductionOrderID, (int)Session[SessionInfo.currentUserID]);
                        oDynamics = GetStepsByColorRatio(_oGUProductionTracingUnit);
                    }
                    catch (Exception ex)
                    {
                        _oGUProductionTracingUnit = new GUProductionTracingUnit();
                        _oGUProductionTracingUnit.ErrorMessage = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _oGUProductionTracingUnit = new GUProductionTracingUnit();
                _oGUProductionTracingUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDynamics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsPTU(GUProductionTracingUnit oGUProductionTracingUnit)
        {
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            try
            {
                //nListFor: 1:Privoius; 2:today 3:YetTo
                _oGUProductionTracingUnit = oGUProductionTracingUnit;
                _oGUProductionTracingUnit.TechnicalSheetSizes = TechnicalSheetSize.Gets(oGUProductionTracingUnit.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                if (oGUProductionTracingUnit.CurrentStepSequence != 1)
                {
                    _oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.PriviousStepID, (int)Session[SessionInfo.currentUserID]);
                    _oGUProductionTracingUnit.PriviousColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 1, oGUProductionTracingUnit.CurrentStepSequence);
                }
                else
                {
                    _oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
                    _oGUProductionTracingUnit.PriviousColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 1, oGUProductionTracingUnit.CurrentStepSequence);
                }
                
                _oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
                _oGUProductionTracingUnit.TodayColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 2, oGUProductionTracingUnit.CurrentStepSequence);
                _oGUProductionTracingUnit.YetToColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 3, oGUProductionTracingUnit.CurrentStepSequence);

                if (_oGUProductionTracingUnits != null)
                {
                    if (_oGUProductionTracingUnits.Count > 0)
                    {
                        _oGUProductionTracingUnit.PTUTransections = PTUTransection.GetPTUTransactionHistory(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
                        _oGUProductionTracingUnit.PTUHistoryColorSizeRatios = GetPTUHistoryColorSizeRatios(_oGUProductionTracingUnit.PTUTransections, _oGUProductionTracingUnit.TechnicalSheetSizes);
                    }
                }
            }
            catch (Exception ex)
            {
                _oGUProductionTracingUnit = new GUProductionTracingUnit();
                _oGUProductionTracingUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionTracingUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsPTUHistory(PTUTransection oPTUTransection)
        {
            _oPTUTransections = new List<PTUTransection>();
            try
            {

                string sSQL = "SELECT * FROM View_PTUTransaction AS PTUT WHERE PTUT.GUProductionTracingUnitDetailID=" + oPTUTransection.GUProductionTracingUnitDetailID + " ORDER BY PTUTransectionID";
                _oPTUTransections = PTUTransection.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                             
            }
            catch (Exception ex)
            {
                _oPTUTransection = new PTUTransection();
                _oPTUTransection.ErrorMessage = ex.Message;
                _oPTUTransections.Add(_oPTUTransection);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUTransections);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Production Executions TEMP
        [HttpPost]
        public JsonResult GetsDynamicPTU(GUProductionTracingUnit oGUProductionTracingUnit)
        {
            List<dynamic> oDynamics = new List<dynamic>();
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            _oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            try
            {
                oDynamics = GetStepsByColorRatio(oGUProductionTracingUnit);
            }
            catch (Exception ex)
            {
                _oGUProductionTracingUnit = new GUProductionTracingUnit();
                _oGUProductionTracingUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDynamics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult GetProductionStep(GUProductionOrder oGUProductionOrder)
        {
            List<GUProductionProcedure> oGUProductionProcedures = new List<GUProductionProcedure>();
            
            try
            {
                oGUProductionOrder.GUProductionProcedures = GUProductionProcedure.Gets(oGUProductionOrder.GUProductionOrderID, (int)Session[SessionInfo.currentUserID]);
                oGUProductionOrder.GUProductionOrderDetails = GUProductionOrderDetail.Gets(oGUProductionOrder.GUProductionOrderID, (int)Session[SessionInfo.currentUserID]);
                oGUProductionOrder.TechnicalSheetSizes = TechnicalSheetSize.Gets(oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oGUProductionOrder.PLineConfigures = PLineConfigure.Gets(oGUProductionOrder.ProductionUnitID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUProductionOrder = new GUProductionOrder();
                _oGUProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGUProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitProductionExecution(GUProductionTracingUnit oGUProductionTracingUnit)
        {
            _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            _oPTUTransections = new List<PTUTransection>();
            _oPTUTransection = new PTUTransection();
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            _oGUProductionTracingUnit.TechnicalSheetSizes = TechnicalSheetSize.Gets(oGUProductionTracingUnit.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            try
            {                
               
                _oPTUTransections = MapPTUTransectionFromColorSizeRation(oGUProductionTracingUnit.TodayColorSizeRatios, _oGUProductionTracingUnit.TechnicalSheetSizes, oGUProductionTracingUnit); 
                if (_oPTUTransections.Count <= 0)
                {
                    _oGUProductionTracingUnit = new GUProductionTracingUnit();
                    _oGUProductionTracingUnit.ErrorMessage = "Please enter at least one item Qty!";

                }
                else
                {
                    _oGUProductionTracingUnits = GUProductionTracingUnit.CommitProductionExecution(_oPTUTransections, (int)Session[SessionInfo.currentUserID]);
                    if (_oGUProductionTracingUnits[0].ErrorMessage != "")
                    {
                        _oGUProductionTracingUnit = new GUProductionTracingUnit();
                        _oGUProductionTracingUnit.ErrorMessage = _oGUProductionTracingUnits[0].ErrorMessage;
                    }
                    else
                    {
                        //_oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
                        
                        //nListFor: 1:Privoius; 2:today 3:YetTo
                        if (oGUProductionTracingUnit.CurrentStepSequence != 1)
                        {
                            _oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.PriviousStepID, (int)Session[SessionInfo.currentUserID]);
                            _oGUProductionTracingUnit.PriviousColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 1, oGUProductionTracingUnit.CurrentStepSequence);
                        }
                        else
                        {
                            _oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
                            _oGUProductionTracingUnit.PriviousColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 1, oGUProductionTracingUnit.CurrentStepSequence);
                        }

                        _oGUProductionTracingUnits = GUProductionTracingUnit.GetsPTU(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
                        _oGUProductionTracingUnit.TodayColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 2, oGUProductionTracingUnit.CurrentStepSequence);
                        _oGUProductionTracingUnit.YetToColorSizeRatios = MapColorSizeRationFromProductionTracingUnits(_oGUProductionTracingUnits, _oGUProductionTracingUnit.TechnicalSheetSizes, 3, oGUProductionTracingUnit.CurrentStepSequence);

                        if (_oGUProductionTracingUnits != null)
                        {
                            if (_oGUProductionTracingUnits.Count > 0)
                            {
                                _oGUProductionTracingUnit.PTUTransections = PTUTransection.GetPTUTransactionHistory(oGUProductionTracingUnit.GUProductionOrderID, oGUProductionTracingUnit.ProductionStepID, (int)Session[SessionInfo.currentUserID]);
                                _oGUProductionTracingUnit.PTUHistoryColorSizeRatios = GetPTUHistoryColorSizeRatios(_oGUProductionTracingUnit.PTUTransections, _oGUProductionTracingUnit.TechnicalSheetSizes);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oGUProductionTracingUnit = new GUProductionTracingUnit();
                _oGUProductionTracingUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionTracingUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ExecutionDetalisPicker(PTUTransection oPTUTransection)
        {
            List<PTUTransection> _oPTUTransections = new List<PTUTransection>();
            try
            {
                if (oPTUTransection.GUProductionTracingUnitDetailID > 0)
                {
                    _oPTUTransections = PTUTransection.GetPTUViewDetails(oPTUTransection.GUProductionTracingUnitDetailID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oPTUTransection = new PTUTransection();
                _oPTUTransection.ErrorMessage = ex.Message;
                _oPTUTransections.Add(_oPTUTransection);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUTransections);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewDetalis(string sTemp, double ts)
        {
            List<PTUTransection> oPTUTransection = new List<PTUTransection>();
            _oPTUTransection = new PTUTransection();
            if (sTemp != "" || sTemp != null)
            {
                int nProductionStepID = Convert.ToInt32(sTemp.Split('~')[0]);
                int nGUProductionOrderID = Convert.ToInt32(sTemp.Split('~')[1]);
                int nExecutionFactoryID = Convert.ToInt32(sTemp.Split('~')[2]);
                DateTime dOperationDate = Convert.ToDateTime(sTemp.Split('~')[3]);
                _oPTUTransection.PTUTransections = PTUTransection.GetPTUViewDetails(nProductionStepID, dOperationDate, nExecutionFactoryID, nGUProductionOrderID, (int)Session[SessionInfo.currentUserID]);
            }
            return PartialView(_oPTUTransection);
        }        
        #endregion

        #region Production Report
        #region Productiion Report Action
        public ActionResult GUProductionOrderReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oGUProductionOrder = new GUProductionOrder();
           return View();
        }

        #endregion

        #region production Summery Report

        public ActionResult POPriview(int nPOID)
        {
            _oGUProductionOrder = new GUProductionOrder();
            _oGUProductionProcedures = new List<GUProductionProcedure>();
            _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oPTUTransections = new List<PTUTransection>();
           

            _oGUProductionOrder = _oGUProductionOrder.Get(nPOID, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.GUProductionProcedures = GUProductionProcedure.Gets_byPOIDs(nPOID.ToString(), (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oGUProductionOrder.PTUTransections = PTUTransection.Gets_byPOIDs(nPOID.ToString(), (int)Session[SessionInfo.currentUserID]);
           
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptGUProductionExecution oReport = new rptGUProductionExecution();
            byte[] abytes = oReport.PrepareReport(_oGUProductionOrder, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion

   
        #endregion

        #region Knitting Operation
        public ActionResult ViewKnitting()
        {
            
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            return View(_oGUProductionTracingUnit);
        }
     
        #endregion

        public ActionResult ViewFullProduction()
        {
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            return View(_oGUProductionTracingUnit);
        }
        
        #region  Production  Tracking
        public ActionResult ViewProductionTracking()
        {
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            return View(_oGUProductionTracingUnit);

        }
        [HttpPost]
        public JsonResult GetProductionTracing(ProductionTracking oProductionTracking)
        {
            _oProductionTracking = new ProductionTracking();

            string saleordrids = oProductionTracking.OrderRecapIDs;
            if (saleordrids != "" || saleordrids != null)
            {

                _oProductionTracking.ProductionTrackings = ProductionTracking.GetProductionTracking(saleordrids, (int)Session[SessionInfo.currentUserID]);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionTracking);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        # endregion


        #region Update PTU Transaction
        [HttpPost]
        public JsonResult UpdatePTUTransaction(PTUTransection oPTUTransection)
        {   
            try
            {
                oPTUTransection = oPTUTransection.UpdatePTUTransaction((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {

                oPTUTransection.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPTUTransection);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Prodcution Progress Report
              

        #region Get Producton Order

        [HttpGet]
        public JsonResult GetProdcutionTracingUnit(string sParam)
        {
            _oGUProductionTracingUnit = new GUProductionTracingUnit();
            List<GUProductionTracingUnit> _oGUProductionTracingUnits = new List<GUProductionTracingUnit>();


            string sProdcutionOrderNo = sParam.Split('~')[0];
            string sStyleNo = sParam.Split('~')[1];

            // DateTime dPCStartDate = Convert.ToDateTime(sParam.Split('~')[2]);
            // DateTime dPCEndDate = Convert.ToDateTime(sParam.Split('~')[3]);
            int sBuyerID = Convert.ToInt32(sParam.Split('~')[4]);
            int sFactoryID = Convert.ToInt32(sParam.Split('~')[5]);
            // string eCompareOperator = "Between";
            string sSQL1 = " SELECT * FROM View_OrderRecap ";
            string sSQL = "";
            if (sProdcutionOrderNo != null)
            {
                if (sProdcutionOrderNo != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + "GUProductionOrderNo= '" + sProdcutionOrderNo + "'";
                }

            }

            if (sStyleNo != null)
            {
                if (sStyleNo != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + "StyleNo='" + sStyleNo + "'";
                }

            }

            if (sBuyerID != 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + "BuyerID = '" + sBuyerID + "'";
            }
            if (sFactoryID != 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + "BuyerID = '" + sFactoryID + "'";
            }

            if (sStyleNo != null)
            {
                if (sStyleNo != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + "StyleNo='" + sStyleNo + "'";
                }

            }
            
            try
            {
                string sFinalSQL = sSQL1 + sSQL;
                _oGUProductionTracingUnits = GUProductionTracingUnit.Gets(sFinalSQL, (int)Session[SessionInfo.currentUserID]);

                _oGUProductionTracingUnit.GUProductionTracingUnits = _oGUProductionTracingUnits;
            }
            catch (Exception ex)
            {

                _oGUProductionTracingUnit = new GUProductionTracingUnit();
                _oGUProductionTracingUnit.ErrorMessage = ex.Message;                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUProductionTracingUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //public ActionResult ViewPrintProdcutionProgressReport(string OrderRecapIDs)
        //{
        //    ProductionProgressReport oProductionProgressReport = new ProductionProgressReport();
        //    oProductionProgressReport.ProductionProgressReportList = ESimSol.BusinessObjects.ProductionProgressReport.Gets(OrderRecapIDs, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);


        //    rptProdcutionProgressReport oReport = new rptProdcutionProgressReport();
        //    byte[] abytes = oReport.PrepareReport(oProductionProgressReport, oCompany);
        //    return File(abytes, "application/pdf");
        //}


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

     

        #endregion  Prodcution Progress Report

    }
}
