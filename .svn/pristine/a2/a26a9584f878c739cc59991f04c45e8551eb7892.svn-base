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
    
    public class FabricDeliveryScheduleService : MarshalByRefObject, IFabricDeliveryScheduleService
    {
        #region Private functions and declaration
        private FabricDeliverySchedule MapObject(NullHandler oReader)
        {
            FabricDeliverySchedule oFabricDeliverySchedule = new FabricDeliverySchedule();
            oFabricDeliverySchedule.FabricDeliveryScheduleID = oReader.GetInt32("FabricDeliveryScheduleID");
            oFabricDeliverySchedule.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFabricDeliverySchedule.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oFabricDeliverySchedule.Name = oReader.GetString("Name");
            oFabricDeliverySchedule.DeliveryAddress = oReader.GetString("DeliveryAddress");
            oFabricDeliverySchedule.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFabricDeliverySchedule.DeliveryOrderNameID = oReader.GetInt32("DeliveryOrderNameID");
            oFabricDeliverySchedule.Qty = oReader.GetDouble("Qty");
            oFabricDeliverySchedule.Note = oReader.GetString("Note");
            oFabricDeliverySchedule.DeliveryToID = oReader.GetInt32("DeliveryToID");
            oFabricDeliverySchedule.DeliveryToName = oReader.GetString("DeliveryToName");
            oFabricDeliverySchedule.IsOwn = oReader.GetBoolean("IsOwn");
            oFabricDeliverySchedule.IsFoc = oReader.GetBoolean("IsFoc");
            oFabricDeliverySchedule.IsGrey = oReader.GetBoolean("IsGrey");
            return oFabricDeliverySchedule;
        }

        private FabricDeliverySchedule CreateObject(NullHandler oReader)
        {
            FabricDeliverySchedule oFabricDeliverySchedule = new FabricDeliverySchedule();
            oFabricDeliverySchedule=MapObject(oReader);
            return oFabricDeliverySchedule;
        }

        private List<FabricDeliverySchedule> CreateObjects(IDataReader oReader)
        {
            List<FabricDeliverySchedule> oFabricDeliverySchedule = new List<FabricDeliverySchedule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDeliverySchedule oItem = CreateObject(oHandler);
                oFabricDeliverySchedule.Add(oItem);
            }
            return oFabricDeliverySchedule;
        }
        
        #endregion

        #region Interface implementation
        public FabricDeliveryScheduleService() { }

        public FabricDeliverySchedule Save(FabricDeliverySchedule oFabricDeliverySchedule, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricDeliverySchedule.FabricDeliveryScheduleID <= 0)
                {
                    reader = FabricDeliveryScheduleDA.InsertUpdate(tc, oFabricDeliverySchedule, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = FabricDeliveryScheduleDA.InsertUpdate(tc, oFabricDeliverySchedule, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliverySchedule = new FabricDeliverySchedule();
                    oFabricDeliverySchedule = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricDeliverySchedule = new FabricDeliverySchedule();
                oFabricDeliverySchedule.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliverySchedule;
        }
        public string Delete(FabricDeliverySchedule oFabricDeliverySchedule, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricDeliveryScheduleDA.Delete(tc, oFabricDeliverySchedule, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricDeliverySchedule = new FabricDeliverySchedule();
                oFabricDeliverySchedule.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FabricDeliverySchedule Get(int id, Int64 nUserId)
        {
            FabricDeliverySchedule oFabricDeliverySchedule = new FabricDeliverySchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricDeliveryScheduleDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliverySchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricDeliverySchedule", e);
                #endregion
            }

            return oFabricDeliverySchedule;
        }
        public List<FabricDeliverySchedule> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricDeliverySchedule> oFabricDeliverySchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricDeliveryScheduleDA.Gets(tc, sSQL);
                oFabricDeliverySchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Master LCs", e);
                #endregion
            }

            return oFabricDeliverySchedules;
        }
        public List<FabricDeliverySchedule> Gets(Int64 nUserId)
        {
            List<FabricDeliverySchedule> oFabricDeliverySchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricDeliveryScheduleDA.Gets(tc);
                oFabricDeliverySchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Master LCs", e);
                #endregion
            }

            return oFabricDeliverySchedules;
        }

        public List<FabricDeliverySchedule> GetsFSCID(int nFSCID, Int64 nUserId)
        {
            List<FabricDeliverySchedule> oFabricDeliverySchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricDeliveryScheduleDA.GetsFSCID(tc, nFSCID);
                oFabricDeliverySchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Master LCs", e);
                #endregion
            }
            return oFabricDeliverySchedules;
        }
        public List<FabricDeliverySchedule> GetsFSCIDLog(int nFSCID, Int64 nUserId)
        {
            List<FabricDeliverySchedule> oFabricDeliverySchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricDeliveryScheduleDA.GetsFSCIDLog(tc, nFSCID);
                oFabricDeliverySchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Febric Delivery Schedule Log", e);
                #endregion
            }
            return oFabricDeliverySchedules;
        }

     
        #endregion
    }
}
