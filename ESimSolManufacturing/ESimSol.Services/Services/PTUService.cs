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
    public class PTUService : MarshalByRefObject, IPTUService
    {
        #region Private functions and declaration
        private PTU MapObject(NullHandler oReader)
        {
            PTU oPTU = new PTU();
            oPTU.PTUID = oReader.GetInt32("PTUID");
            oPTU.OrderID = oReader.GetInt32("OrderID");
            oPTU.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oPTU.ProductID = oReader.GetInt32("ProductID");
            oPTU.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oPTU.ColorName = oReader.GetString("ColorName");
            oPTU.Shade = oReader.GetInt16("Shade");
            oPTU.PantonNo = oReader.GetString("PantonNo");
            oPTU.ColorNo = oReader.GetString("ColorNo");
            oPTU.LabdipNo = oReader.GetString("LabdipNo");
            oPTU.OrderQty = oReader.GetDouble("OrderQty");
            oPTU.ProductionGraceQty = oReader.GetDouble("ProductionGraceQty");
            oPTU.ProductionPipeLineQty = oReader.GetDouble("ProductionPipeLineQty");
            oPTU.ProductionFinishedQty = oReader.GetDouble("ProductionFinishedQty");

            oPTU.UnitPrice = oReader.GetDouble("UnitPrice");
            oPTU.BuyerID = oReader.GetInt32("BuyerID");
            oPTU.ContractorID = oReader.GetInt32("ContractorID");
            oPTU.State = oReader.GetInt16("State");
            oPTU.ReturnQty = oReader.GetDouble("ReturnQty");
            oPTU.ActualDeliveryQty = oReader.GetDouble("ActualDeliveryQty");
            oPTU.ReadyStockInhand = oReader.GetDouble("ReadyStockInhand");
            oPTU.ProductCode = oReader.GetString("ProductCode");
            oPTU.ProductName = oReader.GetString("ProductName");
            oPTU.OrderNo = oReader.GetString("OrderNo");
            oPTU.BuyerName = oReader.GetString("BuyerName");
            oPTU.ContractorName = oReader.GetString("ContractorName");
            //oPTU.ShadeFromOrderName = oReader.GetString("ShadeFromOrderName");
            //oPTU.PTUDistributionQTY = oReader.GetDouble("PTUDistributionQTY");
            //oPTU.ShortClaimQty = oReader.GetDouble("ShortClaimQty");
        
            //oPTU.ScheduledQty = oReader.GetDouble("ScheduledQty");
            //oPTU.ProductionScheduleNo = oReader.GetString("ProductionScheduleNo");
            
            return oPTU;
        }

        private PTU CreateObject(NullHandler oReader)
        {
            PTU oPTU = new PTU();
            oPTU = MapObject(oReader);
            return oPTU;
        }

        private List<PTU> CreateObjects(IDataReader oReader)
        {
            List<PTU> oPTU = new List<PTU>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PTU oItem = CreateObject(oHandler);
                oPTU.Add(oItem);
            }
            return oPTU;
        }

        #endregion

        #region Interface implementation
        public PTUService() { }

        public PTU Get(int nProductionTracingUnitID, Int64 nUserID)
        {
            PTU oPTU = new PTU();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUDA.Get(tc, nProductionTracingUnitID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTU = CreateObject(oReader);
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

            return oPTU;
        }
    
        public List<PTU> GetsByOrder(int nOrderID, int eOrderType, Int64 nUserID)
        {
            List<PTU> oPTUs = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUDA.GetsByOrder(tc, nOrderID, eOrderType);
                oPTUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get view_PTUs", e);
                #endregion
            }

            return oPTUs;
        }
   
        public List<PTU> Gets(string sSQL, Int64 nUserID)
        {
            List<PTU> oPTUs = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUDA.Gets(tc, sSQL);
                oPTUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUs", e);
                #endregion
            }

            return oPTUs;
        }

        public List<PTU> GetsRunningPTUByBuyer(int nContractorID, Int64 nUserID)
        {
            List<PTU> oPTUs = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUDA.GetsRunningPTUByBuyer(tc, nContractorID);
                oPTUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get view_PTUs", e);
                #endregion
            }

            return oPTUs;
        }

       
        public DataSet JobTracker(string sSQL, int bIsSearchWithDeyingOrderNotIssue, bool bIncludingAdjustment, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUDA.JobTracker(tc, sSQL, bIsSearchWithDeyingOrderNotIssue,  bIncludingAdjustment);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[2]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FROM SP Job Tracker", e);
                #endregion
            }
            return oDataSet;
        }
        public DataSet JobTracker_Mkt(string sSQL, int bIsSearchWithDeyingOrderNotIssue, bool bIncludingAdjustment, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUDA.JobTracker_Mkt(tc, sSQL, bIsSearchWithDeyingOrderNotIssue, bIncludingAdjustment);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[2]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FROM SP Job Tracker", e);
                #endregion
            }
            return oDataSet;
        }        
     
        #endregion


     
    }
}
