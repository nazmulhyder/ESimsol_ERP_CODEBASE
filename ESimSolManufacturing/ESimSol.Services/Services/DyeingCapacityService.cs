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
    public class DyeingCapacityService : MarshalByRefObject, IDyeingCapacityService
    {
        #region Private functions and declaration
        private DyeingCapacity MapObject(NullHandler oReader)
        {
            DyeingCapacity oDyeingCapacity = new DyeingCapacity();
            oDyeingCapacity.DyeingCapacityID = oReader.GetInt32("DyeingCapacityID");
            oDyeingCapacity.DyeingType = (EumDyeingType)oReader.GetInt32("DyeingType");
            oDyeingCapacity.DyeingTypeInt = oReader.GetInt32("DyeingType");
            oDyeingCapacity.ProductionHour = oReader.GetDouble("ProductionHour");
            oDyeingCapacity.ProductionCapacity = oReader.GetDouble("ProductionCapacity");
            oDyeingCapacity.MUnitId = oReader.GetInt32("MUnitId");
            oDyeingCapacity.CapacityPerHour = oReader.GetDouble("CapacityPerHour");
            oDyeingCapacity.Remarks = oReader.GetString("Remarks");
            oDyeingCapacity.BaseProductID = oReader.GetInt32("BaseProductID");//new field
            oDyeingCapacity.MUSymbol = oReader.GetString("MUSymbol");
           
            oDyeingCapacity.ProductName = oReader.GetString("ProductName");
            return oDyeingCapacity;
        }

        private DyeingCapacity CreateObject(NullHandler oReader)
        {
            DyeingCapacity oDyeingCapacity = new DyeingCapacity();
            oDyeingCapacity = MapObject(oReader);
            return oDyeingCapacity;
        }

        private List<DyeingCapacity> CreateObjects(IDataReader oReader)
        {
            List<DyeingCapacity> oDyeingCapacity = new List<DyeingCapacity>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DyeingCapacity oItem = CreateObject(oHandler);
                oDyeingCapacity.Add(oItem);
            }
            return oDyeingCapacity;
        }

        #endregion

        #region Interface implementation
        public DyeingCapacityService() { }

        public DyeingCapacity Save(DyeingCapacity oDyeingCapacity, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDyeingCapacity.DyeingCapacityID <= 0)
                {
                    reader = DyeingCapacityDA.InsertUpdate(tc, oDyeingCapacity, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DyeingCapacityDA.InsertUpdate(tc, oDyeingCapacity, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingCapacity = new DyeingCapacity();
                    oDyeingCapacity = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DyeingCapacity. Because of " + e.Message, e);
                #endregion
            }
            return oDyeingCapacity;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DyeingCapacity oDyeingCapacity = new DyeingCapacity();
                oDyeingCapacity.DyeingCapacityID = id;
                DyeingCapacityDA.Delete(tc, oDyeingCapacity, EnumDBOperation.Delete, nUserId);
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

        public DyeingCapacity Get(int id, Int64 nUserId)
        {
            DyeingCapacity oDyeingCapacity = new DyeingCapacity();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DyeingCapacityDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingCapacity = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DyeingCapacity", e);
                #endregion
            }
            return oDyeingCapacity;
        }
        public List<DyeingCapacity> Gets(Int64 nUserID)
        {
            List<DyeingCapacity> oDyeingCapacitys = new List<DyeingCapacity>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingCapacityDA.Gets(tc);
                oDyeingCapacitys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingCapacity", e);
                #endregion
            }
            return oDyeingCapacitys;
        }
        public List<DyeingCapacity> Gets(string sSQL,Int64 nUserID)
        {
            List<DyeingCapacity> oDyeingCapacitys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingCapacityDA.Gets(tc,sSQL);
                oDyeingCapacitys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingCapacity", e);
                #endregion
            }
            return oDyeingCapacitys;
        }
       
        #endregion
    }   
}