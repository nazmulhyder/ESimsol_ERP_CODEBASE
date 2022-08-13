using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{


    [Serializable]
    public class ProductionScheduleDetailService : MarshalByRefObject, IProductionScheduleDetailService
    {
        #region Private functions and declaration
        private ProductionScheduleDetail MapObject(NullHandler oReader)
        {
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            oProductionScheduleDetail.ProductionScheduleDetailID = oReader.GetInt32("ProductionScheduleDetailID");
            oProductionScheduleDetail.ProductionScheduleID = oReader.GetInt32("ProductionScheduleID");
            oProductionScheduleDetail.ProductionTracingUnitID = oReader.GetInt32("ProductionTracingUnitID");
            oProductionScheduleDetail.DODID = oReader.GetInt32("DODID");
            oProductionScheduleDetail.ProductionQty = oReader.GetDouble("ProductionQty");
            oProductionScheduleDetail.PSBatchNo = oReader.GetString("PSBatchNo");
            oProductionScheduleDetail.Remarks = oReader.GetString("Remarks");
            oProductionScheduleDetail.DBUserID = oReader.GetInt32("DBUserID");
            oProductionScheduleDetail.LocationName = oReader.GetString("LocationName");
            oProductionScheduleDetail.MachineName = oReader.GetString("MachineName");
            oProductionScheduleDetail.StartTime = oReader.GetDateTime("StartTime");
            oProductionScheduleDetail.EndTime = oReader.GetDateTime("EndTime");
            oProductionScheduleDetail.OrderNo = oReader.GetString("OrderNo");
            oProductionScheduleDetail.ProductName = oReader.GetString("ProductName");
            oProductionScheduleDetail.FactoryName = oReader.GetString("FactoryName");
            oProductionScheduleDetail.BuyerName = oReader.GetString("BuyerName");
            oProductionScheduleDetail.ColorName = oReader.GetString("ColorName");
            oProductionScheduleDetail.WaitingForProductionQty = oReader.GetDouble("WaitingForProductionQty");
            oProductionScheduleDetail.TotalScheduledQuantity = oReader.GetDouble("TotalScheduledQuantity");
            oProductionScheduleDetail.YetToProductionQty = oReader.GetDouble("YetToProductionQty");
            oProductionScheduleDetail.RemainingScheduleQuantity = oReader.GetDouble("RemainingScheduleQuantity");
            if (oProductionScheduleDetail.RemainingScheduleQuantity < 0)
            {
                oProductionScheduleDetail.RemainingScheduleQuantity = 0;
            }
            oProductionScheduleDetail.ProductionScheduleNo = oReader.GetString("ProductionScheduleNo");
            oProductionScheduleDetail.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oProductionScheduleDetail.RSState = (EnumRSState)oReader.GetInt16("RSState");
            oProductionScheduleDetail.BuyerRef = oReader.GetString("BuyerRef");
            oProductionScheduleDetail.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oProductionScheduleDetail.UsesWeight = oReader.GetDouble("UsesWeight");
            oProductionScheduleDetail.RedyingForRSNo = oReader.GetString("RedyingForRSNo");
            oProductionScheduleDetail.CombineRSNo = oReader.GetString("CombineRSNo");
            oProductionScheduleDetail.CRSID = oReader.GetInt32("CRSID");
            oProductionScheduleDetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            oProductionScheduleDetail.DyeLoadTime = oReader.GetDateTime("DyeLoadTime");
            oProductionScheduleDetail.DyeUnLoadTime = oReader.GetDateTime("DyeUnLoadTime");
            oProductionScheduleDetail.RouteSheetQty = oReader.GetDouble("RouteSheetQty");
            oProductionScheduleDetail.ExpDeliveryDateByFactory = oReader.GetDateTime("ExpDeliveryDateByFactory");
            
            return oProductionScheduleDetail;
        }

        private ProductionScheduleDetail CreateObject(NullHandler oReader)
        {
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            oProductionScheduleDetail = MapObject(oReader);
            return oProductionScheduleDetail;
        }

        private List<ProductionScheduleDetail> CreateObjects(IDataReader oReader)
        {
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionScheduleDetail oItem = CreateObject(oHandler);
                oProductionScheduleDetails.Add(oItem);
            }
            return oProductionScheduleDetails;
        }
        #endregion

        #region Interface implementation
        public ProductionScheduleDetailService() { }



        public List<ProductionScheduleDetail> Gets(int id,Int64 nUserID)
        {
            List<ProductionScheduleDetail> oProductionScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionScheduleDetailDA.Gets(tc,id);
                oProductionScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get ProductionScheduleDetails", e);
                #endregion
            }

            return oProductionScheduleDetails;
        }

        public List<ProductionScheduleDetail> Gets(Int64 nUserID)
        {
            List<ProductionScheduleDetail> oProductionScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionScheduleDetailDA.Gets(tc);
                oProductionScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get ProductionScheduleDetails", e);
                #endregion
            }

            return oProductionScheduleDetails;
        }
        public List<ProductionScheduleDetail> Gets(string sPSIDs,Int64 nUserID)
        {
            List<ProductionScheduleDetail> oProductionScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionScheduleDetailDA.Gets(tc, sPSIDs);
                oProductionScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get ProductionScheduleDetails", e);
                #endregion
            }

            return oProductionScheduleDetails;
        }
        public List<ProductionScheduleDetail> GetsSqL(string sSQL, Int64 nUserID)
        {
            List<ProductionScheduleDetail> oProductionScheduleDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionScheduleDetailDA.GetsSqL(tc, sSQL);
                oProductionScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionScheduleDetails because of "+e.Message.ToString());
                #endregion
            }

            return oProductionScheduleDetails;
        }
        #endregion

    }
}

