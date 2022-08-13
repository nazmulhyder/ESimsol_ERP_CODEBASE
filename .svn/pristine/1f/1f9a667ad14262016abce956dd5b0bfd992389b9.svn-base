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
    public class VehicleTypeService : MarshalByRefObject, IVehicleTypeService
    {
        #region Private functions and declaration
        private VehicleType MapObject(NullHandler oReader)
        {
            VehicleType oVehicleType = new VehicleType();
            oVehicleType.VehicleTypeID = oReader.GetInt32("VehicleTypeID");
            oVehicleType.VehicleTypeCode = oReader.GetString("VehicleTypeCode");
            oVehicleType.VehicleTypeName = oReader.GetString("VehicleTypeName");
            oVehicleType.Remarks = oReader.GetString("Remarks");
            return oVehicleType;
        }
        private VehicleType CreateObject(NullHandler oReader)
        {
            VehicleType oVehicleType = new VehicleType();
            oVehicleType = MapObject(oReader);
            return oVehicleType;
        }
        private List<VehicleType> CreateObjects(IDataReader oReader)
        {
            List<VehicleType> oVehicleType = new List<VehicleType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleType oItem = CreateObject(oHandler);
                oVehicleType.Add(oItem);
            }
            return oVehicleType;
        }
        #endregion

        #region Interface implementation
        public VehicleTypeService() { }
        public VehicleType Save(VehicleType oVehicleType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleType.VehicleTypeID <= 0)
                {
                    reader = VehicleTypeDA.InsertUpdate(tc, oVehicleType, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleTypeDA.InsertUpdate(tc, oVehicleType, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleType = new VehicleType();
                    oVehicleType = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VehicleType. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleType;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleType oVehicleType = new VehicleType();
                oVehicleType.VehicleTypeID = id;
                VehicleTypeDA.Delete(tc, oVehicleType, EnumDBOperation.Delete, nUserId);
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
        public VehicleType Get(int id, Int64 nUserId)
        {
            VehicleType oVehicleType = new VehicleType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = VehicleTypeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleType = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleType", e);
                #endregion
            }
            return oVehicleType;
        }
        public List<VehicleType> Gets(Int64 nUserID)
        {
            List<VehicleType> oVehicleTypes = new List<VehicleType>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleTypeDA.Gets(tc);
                oVehicleTypes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleType", e);
                #endregion
            }
            return oVehicleTypes;
        }
        public List<VehicleType> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleType> oVehicleTypes = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleTypeDA.Gets(tc, sSQL);
                oVehicleTypes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleType", e);
                #endregion
            }
            return oVehicleTypes;
        }

        #endregion
    }
}