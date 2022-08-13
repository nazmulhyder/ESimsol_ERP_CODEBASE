using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class GUProductionOrderService : MarshalByRefObject, IGUProductionOrderService
    {
        #region Private functions and declaration
        private GUProductionOrder MapObject(NullHandler oReader)
        {
            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            oGUProductionOrder.GUProductionOrderID = oReader.GetInt32("GUProductionOrderID");
            oGUProductionOrder.GUProductionOrderNo = oReader.GetString("GUProductionOrderNo");
            oGUProductionOrder.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oGUProductionOrder.BuyerID = oReader.GetInt32("BuyerID");
            oGUProductionOrder.ProductID = oReader.GetInt32("ProductID");
            oGUProductionOrder.FabricID = oReader.GetInt32("FabricID");
            oGUProductionOrder.FBUID = oReader.GetInt32("FBUID");
            oGUProductionOrder.GG = oReader.GetString("GG");
            oGUProductionOrder.Count = oReader.GetString("Count");
            oGUProductionOrder.ProductionUnitID = oReader.GetInt32("ProductionUnitID");
            oGUProductionOrder.UnitID = oReader.GetInt32("UnitID");
            oGUProductionOrder.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oGUProductionOrder.OrderStatus = (EnumGUProductionOrderStatus)oReader.GetInt32("OrderStatus");
            oGUProductionOrder.nOrderStatus = oReader.GetInt32("OrderStatus");
            oGUProductionOrder.OrderDate = oReader.GetDateTime("OrderDate");
            oGUProductionOrder.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oGUProductionOrder.Note = oReader.GetString("Note");
            oGUProductionOrder.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oGUProductionOrder.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oGUProductionOrder.ODSNo = oReader.GetString("ODSNo");
            oGUProductionOrder.ProductionFactoryName = oReader.GetString("ProductionFactoryName");
            oGUProductionOrder.ProductionFactoryPhone = oReader.GetString("ProductionFactoryPhone");
            oGUProductionOrder.BuyerName = oReader.GetString("BuyerName");
            oGUProductionOrder.BuyerPhone = oReader.GetString("BuyerPhone");
            oGUProductionOrder.GarmentsProductName = oReader.GetString("GarmentsProductName");
            oGUProductionOrder.GarmentsProductCode = oReader.GetString("GarmentsProductCode");
            oGUProductionOrder.FabricProductCode = oReader.GetString("FabricProductCode");
            oGUProductionOrder.FabricProductName = oReader.GetString("FabricProductName");
            oGUProductionOrder.FactoryContactPersonName = oReader.GetString("FactoryContactPersonName");
            oGUProductionOrder.FactoryContactPersonPhone = oReader.GetString("FactoryContactPersonPhone");
            oGUProductionOrder.MerchandiserName = oReader.GetString("MerchandiserName");
            oGUProductionOrder.MerchandiserContactNo = oReader.GetString("MerchandiserContactNo");
            oGUProductionOrder.ODSDetailQty = oReader.GetDouble("ODSDetailQty");
            oGUProductionOrder.StyleNo = oReader.GetString("StyleNo");
            oGUProductionOrder.TotalQty = oReader.GetInt32("TotalQty");
            oGUProductionOrder.OperationDate = oReader.GetDateTime("OrderDate");
            oGUProductionOrder.YetToPoductionQty = oReader.GetDouble("YetToPoductionQty");
            oGUProductionOrder.ApprovedBy = oReader.GetString("ApprovedBy");
            oGUProductionOrder.RecapQty = oReader.GetDouble("RecapQty");
            oGUProductionOrder.KnittingPattern = oReader.GetInt32("KnittingPattern");
            oGUProductionOrder.KnittingPatternName = oReader.GetString("KnittingPatternName");
            oGUProductionOrder.StyleDescription = oReader.GetString("StyleDescription");
            oGUProductionOrder.ToleranceInPercent = oReader.GetDouble("ToleranceInPercent");
            oGUProductionOrder.WindingStatus = (EnumWindingStatus)oReader.GetInt32("WindingStatus");
            oGUProductionOrder.WindingStatusInInt = oReader.GetInt32("WindingStatus");
            oGUProductionOrder.BUID = oReader.GetInt32("BUID");
            oGUProductionOrder.FactoryShipmentDate = oReader.GetDateTime("FactoryShipmentDate");
            oGUProductionOrder.ShipmentScheduleID = oReader.GetInt32("ShipmentScheduleID");
            oGUProductionOrder.InputDate = oReader.GetDateTime("InputDate");
            oGUProductionOrder.SSQty = oReader.GetDouble("SSQty");
            oGUProductionOrder.CutOffDate = oReader.GetDateTime("CutOffDate");
            oGUProductionOrder.CutOffType = (EnumCutOffType)oReader.GetInt32("CutOffType");
            oGUProductionOrder.ProductionUnitName = oReader.GetString("ProductionUnitName");
            return oGUProductionOrder;
        }

        private GUProductionOrder CreateObject(NullHandler oReader)
        {
            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            oGUProductionOrder = MapObject(oReader);
            return oGUProductionOrder;
        }

        private List<GUProductionOrder> CreateObjects(IDataReader oReader)
        {
            List<GUProductionOrder> oGUProductionOrder = new List<GUProductionOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUProductionOrder oItem = CreateObject(oHandler);
                oGUProductionOrder.Add(oItem);
            }
            return oGUProductionOrder;
        }

        #endregion

        #region Interface implementation
        public GUProductionOrderService() { }

        public GUProductionOrder Save(GUProductionOrder oGUProductionOrder, Int64 nUserID)
        {
            
            List<GUProductionOrderDetail> oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            GUProductionOrderDetail oGUProductionOrderDetail = new GUProductionOrderDetail();
            string sGUProductionOrderDetailIDs = "";
            
            TransactionContext tc = null;
            oGUProductionOrderDetails = oGUProductionOrder.GUProductionOrderDetails;
            if (oGUProductionOrderDetails.Count <= 0)
            {
                throw new Exception("Enter at least one Color!");
            }
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                #region Production Order
                if (oGUProductionOrder.GUProductionOrderID <= 0)
                {
                    reader = GUProductionOrderDA.InsertUpdate(tc, oGUProductionOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = GUProductionOrderDA.InsertUpdate(tc, oGUProductionOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrder = new GUProductionOrder();
                    oGUProductionOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Production Order Details
                 foreach (GUProductionOrderDetail oItem in oGUProductionOrderDetails)
                    {

                            oItem.GUProductionOrderID = oGUProductionOrder.GUProductionOrderID;
                            if (oItem.GUProductionOrderDetailID <= 0)
                            {
                                reader = GUProductionOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                reader = GUProductionOrderDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                sGUProductionOrderDetailIDs = sGUProductionOrderDetailIDs + oReader.GetString("GUProductionOrderDetailID") + ",";
                            }
                            reader.Close();
                      }
                    if (sGUProductionOrderDetailIDs.Length > 0)
                    {
                        sGUProductionOrderDetailIDs = sGUProductionOrderDetailIDs.Remove(sGUProductionOrderDetailIDs.Length - 1, 1);
                    }
                    oGUProductionOrderDetail = new GUProductionOrderDetail();
                    oGUProductionOrderDetail.GUProductionOrderID = oGUProductionOrder.GUProductionOrderID;
                    GUProductionOrderDetailDA.Delete(tc, oGUProductionOrderDetail, EnumDBOperation.Delete, nUserID, sGUProductionOrderDetailIDs);
                    
                #endregion

                #region Get PO
                reader = GUProductionOrderDA.Get(tc, oGUProductionOrder.GUProductionOrderID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion

               tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGUProductionOrder = new GUProductionOrder();
                oGUProductionOrder.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save GUProductionOrder. Because of " + e.Message, e);
                #endregion
            }
            return oGUProductionOrder;
        }

        public GUProductionOrder UpdateToleranceWithStatus(GUProductionOrder oGUProductionOrder, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                GUProductionOrderDA.UpdateToleranceWithStatus(tc, oGUProductionOrder);
                 reader = GUProductionOrderDA.Get(tc, oGUProductionOrder.GUProductionOrderID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrder = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GUProductionOrder", e);
                #endregion
            }

            return oGUProductionOrder;
        }

        public GUProductionOrder SendToProducton(int nGUProductionOrderID, Int64 nUserID)
        {
            TransactionContext tc = null;
            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                #region Production Tracing Uni
                reader = GUProductionOrderDA.SendToProducton(tc, nGUProductionOrderID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrder = new GUProductionOrder();
                    oGUProductionOrder = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGUProductionOrder = new GUProductionOrder();
                oGUProductionOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oGUProductionOrder;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GUProductionOrder oGUProductionOrder = new GUProductionOrder();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.GUProductionOrder, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "GUProductionOrder", id);
                oGUProductionOrder.GUProductionOrderID = id;
                GUProductionOrderDA.Delete(tc, oGUProductionOrder, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GUProductionOrder. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public GUProductionOrder GetbyGUProductionOrderNo(string GUProductionOrderno, Int64 nUserId)
        {
            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GUProductionOrderDA.GetbyGUProductionOrderNo(tc, GUProductionOrderno);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrder = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GUProductionOrder", e);
                #endregion
            }

            return oGUProductionOrder;
        }


        public GUProductionOrder Get(int id, Int64 nUserId)
        {
            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GUProductionOrderDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrder = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GUProductionOrder", e);
                #endregion
            }

            return oGUProductionOrder;
        }

        public GUProductionOrder ProductionProgresReport(string sRecapIDs, Int64 nUserId)
        {
            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            DataSet oDataSet = new DataSet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GUProductionOrderDA.ProductionProgresReport(tc, sRecapIDs);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[2]);
                oGUProductionOrder.ProductionSummeryValueTable = oDataSet.Tables[0];
                oGUProductionOrder.ProductionSummeryColumnTable = oDataSet.Tables[1];
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oGUProductionOrder.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }

            return oGUProductionOrder;
        }


        public GUProductionOrder ChangeStatus(GUProductionOrder oGUProductionOrder, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = GUProductionOrderDA.ChangeStatus(tc, oGUProductionOrder, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrder = new GUProductionOrder();
                    oGUProductionOrder = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oGUProductionOrder.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save OrderDistributionSheetDetail. Because of " + e.Message, e);
                #endregion
            }
            return oGUProductionOrder;
        }

        public List<GUProductionOrder> Gets(Int64 nUserID)
        {
            List<GUProductionOrder> oGUProductionOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDA.Gets(tc);
                oGUProductionOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrder", e);
                #endregion
            }

            return oGUProductionOrder;
        }

        public List<GUProductionOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<GUProductionOrder> oGUProductionOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDA.Gets(tc, sSQL);
                oGUProductionOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrder", e);
                #endregion
            }

            return oGUProductionOrder;
        }

        public List<GUProductionOrder> Gets_bySalorderID(int nOrderRecapID, Int64 nUserID)
        {
            List<GUProductionOrder> oGUProductionOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDA.Gets_bySalorderID(tc, nOrderRecapID);
                oGUProductionOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrder", e);
                #endregion
            }

            return oGUProductionOrder;
        }

        public List<GUProductionOrder> Gets_byPOIDs(string sPOIDs, Int64 nUserID)
        {
            List<GUProductionOrder> oGUProductionOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDA.Gets_byPOIDs(tc, sPOIDs);
                oGUProductionOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrder", e);
                #endregion
            }

            return oGUProductionOrder;
        }


        #endregion
    }
}
