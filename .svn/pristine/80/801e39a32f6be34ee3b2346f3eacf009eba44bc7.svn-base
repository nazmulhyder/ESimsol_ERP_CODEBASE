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
    public class VehiclePartsService : MarshalByRefObject, IVehiclePartsService
    {
        #region Private functions and declaration
        private VehicleParts MapObject(NullHandler oReader)
        {
            VehicleParts oVehicleParts = new VehicleParts();
            oVehicleParts.VehiclePartsID = oReader.GetInt32("VehiclePartsID");
            oVehicleParts.PartsCode = oReader.GetString("PartsCode");
            oVehicleParts.PartsName = oReader.GetString("PartsName");
            oVehicleParts.PartsType = oReader.GetInt32("PartsType");
            oVehicleParts.Remarks = oReader.GetString("Remarks");
            return oVehicleParts;
        }

        private VehicleParts CreateObject(NullHandler oReader)
        {
            VehicleParts oVehicleParts = new VehicleParts();
            oVehicleParts = MapObject(oReader);
            return oVehicleParts;
        }

        private List<VehicleParts> CreateObjects(IDataReader oReader)
        {
            List<VehicleParts> oVehicleParts = new List<VehicleParts>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleParts oItem = CreateObject(oHandler);
                oVehicleParts.Add(oItem);
            }
            return oVehicleParts;
        }

        #endregion

        #region Interface implementation
        public VehiclePartsService() { }

        public VehicleParts Save(VehicleParts oVehicleParts, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleParts.VehiclePartsID <= 0)
                {
                    reader = VehiclePartsDA.InsertUpdate(tc, oVehicleParts, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehiclePartsDA.InsertUpdate(tc, oVehicleParts, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleParts = new VehicleParts();
                    oVehicleParts = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VehicleParts. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleParts;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleParts oVehicleParts = new VehicleParts();
                oVehicleParts.VehiclePartsID = id;
                VehiclePartsDA.Delete(tc, oVehicleParts, EnumDBOperation.Delete, nUserId);
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

        public VehicleParts Get(int id, Int64 nUserId)
        {
            VehicleParts oVehicleParts = new VehicleParts();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = VehiclePartsDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleParts = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleParts", e);
                #endregion
            }
            return oVehicleParts;
        }

        public List<VehicleParts> GetByPartsCode(string sPartsCode, Int64 nUserID)
        {
            List<VehicleParts> oVehiclePartss = new List<VehicleParts>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehiclePartsDA.GetsByPartsCode(tc, sPartsCode);
                oVehiclePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehiclePartss", e);
                #endregion
            }
            return oVehiclePartss;
        }

        public List<VehicleParts> Gets(Int64 nUserID)
        {
            List<VehicleParts> oVehiclePartss = new List<VehicleParts>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehiclePartsDA.Gets(tc);
                oVehiclePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleParts", e);
                #endregion
            }
            return oVehiclePartss;
        }
        public List<VehicleParts> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleParts> oVehiclePartss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehiclePartsDA.Gets(tc, sSQL);
                oVehiclePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleParts", e);
                #endregion
            }
            return oVehiclePartss;
        }


        public List<VehicleParts> GetsByPartsCode(string sPartsCode, Int64 nUserID)
        {
            List<VehicleParts> oVehiclePartss = new List<VehicleParts>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehiclePartsDA.GetsByPartsCode(tc, sPartsCode);
                oVehiclePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehiclePartss", e);
                #endregion
            }
            return oVehiclePartss;
        }
        public List<VehicleParts> GetsByPartsNameWithType(string sPartsName, int nPartsType, Int64 nUserID)
        {
            List<VehicleParts> oVehiclePartss = new List<VehicleParts>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehiclePartsDA.GetsByPartsNameWithType(tc, sPartsName, nPartsType);
                oVehiclePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehiclePartss", e);
                #endregion
            }
            return oVehiclePartss;
        }

        #endregion
    }
}