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
    public class PTUTrackerService : MarshalByRefObject, IPTUTrackerService
    {
        #region Private functions and declaration
        private PTUTracker MapObject(NullHandler oReader)
        {
            PTUTracker oPTUTracker = new PTUTracker();
            oPTUTracker.PTUID = oReader.GetInt32("PTUID");
            oPTUTracker.OrderID = oReader.GetInt32("OrderID");
            oPTUTracker.OrderType = oReader.GetInt16("OrderType");
            oPTUTracker.ProductID = oReader.GetInt32("ProductID");
            oPTUTracker.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oPTUTracker.OrderNo = oReader.GetString("ColorName");
            oPTUTracker.ColorName = oReader.GetString("ColorName");
            oPTUTracker.PantonNo = oReader.GetString("PantonNo");
            oPTUTracker.ColorNo = oReader.GetString("ColorNo");

            oPTUTracker.BuyerID = oReader.GetInt32("BuyerID");
            oPTUTracker.ContractorID = oReader.GetInt32("ContractorID");
            oPTUTracker.State = oReader.GetInt16("State");
            oPTUTracker.Qty_PI = oReader.GetDouble("Qty_PI");
            oPTUTracker.OrderQty = oReader.GetDouble("OrderQty");
            oPTUTracker.ProductionGraceQty = oReader.GetDouble("ProductionGraceQty");
            oPTUTracker.ProductionPipeLineQty = oReader.GetDouble("ProductionPipeLineQty");
            oPTUTracker.ProductionFinishedQty = oReader.GetDouble("ProductionFinishedQty");

            oPTUTracker.Shade = oReader.GetInt32("Shade");
           
            oPTUTracker.ReturnQty = oReader.GetDouble("ReturnQty");
            oPTUTracker.ActualDeliveryQty = oReader.GetDouble("ActualDeliveryQty");
            oPTUTracker.ReadyStockInhand = oReader.GetDouble("ReadyStockInhand");
           
           
            oPTUTracker.OrderNo = oReader.GetString("OrderNo");
            oPTUTracker.ContractorName = oReader.GetString("ContractorName");
            oPTUTracker.BuyerName = oReader.GetString("BuyerName");
            oPTUTracker.ProductName = oReader.GetString("ProductName");
            oPTUTracker.ProductCode = oReader.GetString("ProductCode");
            oPTUTracker.YarnCount = oReader.GetString("YarnCount");
            oPTUTracker.LCNo = oReader.GetString("ExportLCNo");
            oPTUTracker.MKT = oReader.GetString("MKT");
            oPTUTracker.PINo = oReader.GetString("PINo");
            
            //oPTUTracker.PTUTrackerDistributionQTY = oReader.GetDouble("PTUTrackerDistributionQTY");
            //oPTUTracker.ShortClaimQty = oReader.GetDouble("ShortClaimQty");
        
            //oPTUTracker.ScheduledQty = oReader.GetDouble("ScheduledQty");
            //oPTUTracker.ProductionScheduleNo = oReader.GetString("ProductionScheduleNo");
            
            return oPTUTracker;
        }

        private PTUTracker CreateObject(NullHandler oReader)
        {
            PTUTracker oPTUTracker = new PTUTracker();
            oPTUTracker = MapObject(oReader);
            return oPTUTracker;
        }

        private List<PTUTracker> CreateObjects(IDataReader oReader)
        {
            List<PTUTracker> oPTUTracker = new List<PTUTracker>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PTUTracker oItem = CreateObject(oHandler);
                oPTUTracker.Add(oItem);
            }
            return oPTUTracker;
        }

        #endregion

        #region Interface implementation
        public PTUTrackerService() { }

        public PTUTracker Get(int nProductionTracingUnitID, Int64 nUserID)
        {
            PTUTracker oPTUTracker = new PTUTracker();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUTrackerDA.Get(tc, nProductionTracingUnitID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUTracker = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductionTracingUnit", e);
                #endregion
            }

            return oPTUTracker;
        }
    
        //public List<PTUTracker> GetsByOrder(int nOrderID, int eOrderType, Int64 nUserID)
        //{
        //    List<PTUTracker> oPTUTrackers = null;
        //    TransactionContext tc = null;

        //    try
        //    {
        //        tc = TransactionContext.Begin();

        //        IDataReader reader = null;
        //        reader = PTUTrackerDA.GetsByOrder(tc, nOrderID, eOrderType);
        //        oPTUTrackers = CreateObjects(reader);
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        ExceptionLog.Write(e);
        //        throw new ServiceException("Failed to Get view_PTUTrackers", e);
        //        #endregion
        //    }

        //    return oPTUTrackers;
        //}

        public List<PTUTracker> Gets(string sSQL, int nReportType, int nViewType, int nOrderType, Int64 nUserID)
        {
            List<PTUTracker> oPTUTrackers = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUTrackerDA.Gets(tc,  sSQL,   nReportType,  nViewType,  nOrderType);
                oPTUTrackers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUTrackers", e);
                #endregion
            }

            return oPTUTrackers;
        }

        public List<PTUTracker> GetsYetTOPro(int nReportType, int nViewType, Int64 nUserID)
        {
            List<PTUTracker> oPTUTrackers = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUTrackerDA.GetsYetTOPro(tc, nReportType, nViewType);
                oPTUTrackers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get view_PTUTrackers", e);
                #endregion
            }

            return oPTUTrackers;
        }

       
  
     
        #endregion


     
    }
}
