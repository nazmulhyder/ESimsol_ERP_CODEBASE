using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class MeasurementUnitService : MarshalByRefObject, IMeasurementUnitService
    {
        #region Private functions and declaration
        private MeasurementUnit MapObject(NullHandler oReader)
        {
            MeasurementUnit oMeasurementUnit = new MeasurementUnit();
            oMeasurementUnit.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oMeasurementUnit.UnitName = oReader.GetString("UnitName");
            oMeasurementUnit.UnitType = (EnumUniteType)oReader.GetInt16("UnitType");
            oMeasurementUnit.Symbol = oReader.GetString("Symbol");
            oMeasurementUnit.Note = oReader.GetString("Note");
            oMeasurementUnit.IsRound = oReader.GetBoolean("IsRound");
            oMeasurementUnit.IsSmallUnit = oReader.GetBoolean("IsSmallUnit");
            return oMeasurementUnit;
        }

        private MeasurementUnit CreateObject(NullHandler oReader)
        {
            MeasurementUnit oMeasurementUnit = new MeasurementUnit();
            oMeasurementUnit = MapObject(oReader);
            return oMeasurementUnit;
        }

        private List<MeasurementUnit> CreateObjects(IDataReader oReader)
        {
            List<MeasurementUnit> oMeasurementUnit = new List<MeasurementUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MeasurementUnit oItem = CreateObject(oHandler);
                oMeasurementUnit.Add(oItem);
            }
            return oMeasurementUnit;
        }

        #endregion

        #region Interface implementation
        public MeasurementUnitService() { }

        public MeasurementUnit Save(MeasurementUnit oMeasurementUnit, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMeasurementUnit.MeasurementUnitID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "MeasurementUnit", EnumRoleOperationType.Add);
                    reader = MeasurementUnitDA.InsertUpdate(tc, oMeasurementUnit, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "MeasurementUnit", EnumRoleOperationType.Edit);
                    reader = MeasurementUnitDA.InsertUpdate(tc, oMeasurementUnit, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementUnit = new MeasurementUnit();
                    oMeasurementUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save MeasurementUnit. Because of " + e.Message, e);
                #endregion
            }
            return oMeasurementUnit;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MeasurementUnit oMeasurementUnit = new MeasurementUnit();
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "MeasurementUnit", EnumRoleOperationType.Delete);
               // DBTableReferenceDA.HasReference(tc, "MeasurementUnit", id);
                oMeasurementUnit.MeasurementUnitID = id;
                MeasurementUnitDA.Delete(tc, oMeasurementUnit, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete MeasurementUnit. Because of " + e.Message, e);
                #endregion
            }
            return "Data Delete Successfully";
        }
        public List<MeasurementUnit> GetsbyProductID(int productId, int nUserID)
        {
            List<MeasurementUnit> oMeasurementUnits = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementUnitDA.GetsbyProductID(tc, productId);
                NullHandler oReader = new NullHandler(reader);
                oMeasurementUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementUnit", e);
                #endregion
            }

            return oMeasurementUnits;
        }
        public MeasurementUnit Get(int id, int nUserId)
        {
            MeasurementUnit oAccountHead = new MeasurementUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementUnitDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MeasurementUnit", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<MeasurementUnit> Gets(int nUserId)
        {
            List<MeasurementUnit> oMeasurementUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementUnitDA.Gets(tc);
                oMeasurementUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementUnit", e);
                #endregion
            }

            return oMeasurementUnit;
        }

        public List<MeasurementUnit> Gets(int nUnitType, int nUserId)
        {
            List<MeasurementUnit> oMeasurementUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementUnitDA.Gets(tc, nUnitType);
                oMeasurementUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementUnit", e);
                #endregion
            }

            return oMeasurementUnit;
        }

        public List<MeasurementUnit> Gets(string sSQL, int nUserId)
        {
            List<MeasurementUnit> oMeasurementUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementUnitDA.Gets(tc, sSQL);
                oMeasurementUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementUnit", e);
                #endregion
            }

            return oMeasurementUnit;
        }
        #endregion
    }
}