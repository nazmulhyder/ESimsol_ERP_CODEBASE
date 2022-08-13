using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RouteSheetDOService : MarshalByRefObject, IRouteSheetDOService
    {
        #region Private functions and declaration
        private RouteSheetDO MapObject(NullHandler oReader)
        {
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            oRouteSheetDO.RouteSheetDOID = oReader.GetInt32("RouteSheetDOID");
            oRouteSheetDO.OrderNo = oReader.GetString("OrderNo");
            oRouteSheetDO.OrderDate = oReader.GetDateTime("OrderDate");
            oRouteSheetDO.Qty_RS = oReader.GetDouble("Qty_RS");
            oRouteSheetDO.Qty_QC = oReader.GetDouble("Qty_QC");
            oRouteSheetDO.Qty_Finish = oReader.GetDouble("Qty_Finish");
            oRouteSheetDO.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetDO.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oRouteSheetDO.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oRouteSheetDO.OrderNo = oReader.GetString("OrderNo");
            oRouteSheetDO.OrderDate = oReader.GetDateTime("OrderDate");
            oRouteSheetDO.PTUID = oReader.GetInt32("PTUID");
            oRouteSheetDO.ProductID = oReader.GetInt32("ProductID");
            oRouteSheetDO.ProductName = oReader.GetString("ProductName");
            oRouteSheetDO.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oRouteSheetDO.OrderTypeInt = oReader.GetInt32("OrderType");
            oRouteSheetDO.LabdipNo = oReader.GetString("LabdipNo");
            oRouteSheetDO.ColorName = oReader.GetString("ColorName");
            oRouteSheetDO.ColorNo = oReader.GetString("ColorNo");
            oRouteSheetDO.PantonNo = oReader.GetString("PantonNo");
            oRouteSheetDO.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oRouteSheetDO.OrderQty = oReader.GetDouble("OrderQty");
            oRouteSheetDO.Qty_Pro = oReader.GetDouble("Qty_Pro");
            oRouteSheetDO.SMUnitValue = oReader.GetDouble("SMUnitValue");
            oRouteSheetDO.NoCode = oReader.GetString("NoCode");
            oRouteSheetDO.Shade = (EnumShade)oReader.GetInt32("Shade");
            oRouteSheetDO.RSState = (EnumRSState)oReader.GetInt32("RSState");
           
            oRouteSheetDO.ClaimOrderNo = oReader.GetString("ClaimOrderNo");
            oRouteSheetDO.ContractorID = oReader.GetInt32("ContractorID");
            oRouteSheetDO.ApproveLotNo = oReader.GetString("ApproveLotNo");
            oRouteSheetDO.BuyerRef = oReader.GetString("BuyerRef");
            oRouteSheetDO.ContractorName = oReader.GetString("ContractorName");
            oRouteSheetDO.DeliveryToName = oReader.GetString("DeliveryToName");
            oRouteSheetDO.WastageQty = oReader.GetDouble("WastageQty");
            oRouteSheetDO.RecycleQty = oReader.GetDouble("RecycleQty");
            oRouteSheetDO.Note = oReader.GetString("Note");
            oRouteSheetDO.RoutesheetNo = oReader.GetString("RoutesheetNo");
            oRouteSheetDO.StyleNo = oReader.GetString("StyleNo");
            oRouteSheetDO.RefNo = oReader.GetString("RefNo");
            return oRouteSheetDO;

        }
        private RouteSheetDO CreateObject(NullHandler oReader)
        {
            RouteSheetDO oRouteSheetDO = MapObject(oReader);
            return oRouteSheetDO;
        }
        private List<RouteSheetDO> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetDO oItem = CreateObject(oHandler);
                oRouteSheetDOs.Add(oItem);
            }
            return oRouteSheetDOs;
        }

        private RouteSheetDO MapObject_DOD(NullHandler oReader)
        {
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            oRouteSheetDO.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oRouteSheetDO.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oRouteSheetDO.OrderNo = oReader.GetString("OrderNo");
            oRouteSheetDO.OrderDate = oReader.GetDateTime("OrderDate");
            oRouteSheetDO.PTUID = oReader.GetInt32("PTUID");
            oRouteSheetDO.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oRouteSheetDO.OrderTypeInt = oReader.GetInt32("OrderType");
            oRouteSheetDO.LabdipNo = oReader.GetString("LabdipNo");
            oRouteSheetDO.ColorName = oReader.GetString("ColorName");
            oRouteSheetDO.ColorNo = oReader.GetString("ColorNo");
            oRouteSheetDO.PantonNo = oReader.GetString("PantonNo");
            oRouteSheetDO.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oRouteSheetDO.OrderQty = oReader.GetDouble("OrderQty");
            oRouteSheetDO.WastageQty = oReader.GetDouble("WastageQty");
            oRouteSheetDO.RecycleQty = oReader.GetDouble("RecycleQty");
            oRouteSheetDO.Qty_Pro = oReader.GetDouble("Qty_Pro");
            oRouteSheetDO.Qty_PSD = oReader.GetDouble("Qty_PSD");
            oRouteSheetDO.ContractorName = oReader.GetString("ContractorName");
            oRouteSheetDO.DeliveryToName = oReader.GetString("DeliveryToName");
            oRouteSheetDO.Shade = (EnumShade)oReader.GetInt32("Shade");
            oRouteSheetDO.ProductName = oReader.GetString("ProductName");
            oRouteSheetDO.ProductID = oReader.GetInt32("ProductID");
            oRouteSheetDO.ContractorID = oReader.GetInt32("ContractorID");
            oRouteSheetDO.SMUnitValue = oReader.GetDouble("SMUnitValue");
            oRouteSheetDO.NoCode = oReader.GetString("NoCode");
            oRouteSheetDO.ApproveLotNo = oReader.GetString("ApproveLotNo");
            oRouteSheetDO.HankorCone = oReader.GetInt16("HankorCone");
            oRouteSheetDO.Note = oReader.GetString("Note");
            oRouteSheetDO.StyleNo = oReader.GetString("StyleNo");
            oRouteSheetDO.RefNo = oReader.GetString("RefNo");
            
            oRouteSheetDO.Qty_RS = oRouteSheetDO.OrderQty - oRouteSheetDO.Qty_Pro;
            return oRouteSheetDO;

        }
        private RouteSheetDO CreateObject_DOD(NullHandler oReader)
        {
            RouteSheetDO oRouteSheetDO = MapObject_DOD(oReader);
            return oRouteSheetDO;
        }
        private List<RouteSheetDO> CreateObjects_DOD(IDataReader oReader)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetDO oItem = CreateObject_DOD(oHandler);
                oRouteSheetDOs.Add(oItem);
            }
            return oRouteSheetDOs;
        }
        #endregion

        #region Interface implementation
        public RouteSheetDOService() { }

        public RouteSheetDO Save(RouteSheetDO oRouteSheetDO, Int64 nUserID)
        {
            oRouteSheetDO.Qty_RS = Math.Round(oRouteSheetDO.Qty_RS,2);
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRouteSheetDO.RouteSheetDOID <= 0)
                {
                    reader = RouteSheetDODA.InsertUpdate(tc, oRouteSheetDO, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = RouteSheetDODA.InsertUpdate(tc, oRouteSheetDO, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetDO = new RouteSheetDO();
                    oRouteSheetDO = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oRouteSheetDO = new RouteSheetDO();
                oRouteSheetDO.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRouteSheetDO;
        }
        public RouteSheetDO Get(int nDOID, Int64 nUserId)
        {
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetDODA.Get(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetDO = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get RouteSheetDO", e);
                oRouteSheetDO.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetDO;
        }

        public List<RouteSheetDO> GetsBy(int nRouteSheetID, Int64 nUserID)
        {
            List<RouteSheetDO> oRouteSheetDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDODA.GetsBy(tc, nRouteSheetID);
                oRouteSheetDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetDO", e);
                #endregion
            }
            return oRouteSheetDO;
        }
  

        public string Delete(RouteSheetDO oRouteSheetDO, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                RouteSheetDODA.Delete(tc, oRouteSheetDO, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public List<RouteSheetDO> Gets(string sSQL, Int64 nUserID)
        {
            List<RouteSheetDO> oRouteSheetDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDODA.Gets(sSQL, tc);
                oRouteSheetDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetDO", e);
                #endregion
            }
            return oRouteSheetDO;
        }
        public List<RouteSheetDO> GetsDOYetTORS(string sSQL, Int64 nUserID)
        {
            List<RouteSheetDO> oRouteSheetDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDODA.GetsDOYetTORS(sSQL, tc);
                oRouteSheetDO = CreateObjects_DOD(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetDO", e);
                #endregion
            }
            return oRouteSheetDO;
        }

        #endregion

    }
}
