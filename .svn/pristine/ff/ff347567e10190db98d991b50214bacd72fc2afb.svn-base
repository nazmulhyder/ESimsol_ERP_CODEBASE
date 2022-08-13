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
    public class WorkingUnitService : MarshalByRefObject, IWorkingUnitService
    {
        #region Private functions and declaration
        private WorkingUnit MapObject(NullHandler oReader)
        {
            WorkingUnit oWorkingUnit = new WorkingUnit();
            oWorkingUnit.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oWorkingUnit.WorkingUnitCode = oReader.GetString("WorkingUnitCode");
            oWorkingUnit.LocationID = oReader.GetInt32("LocationID");
            oWorkingUnit.OperationUnitID = oReader.GetInt32("OperationUnitID");
            oWorkingUnit.OperationUnitName = oReader.GetString("OperationUnitName");          
            oWorkingUnit.IsActive = oReader.GetBoolean("IsActive");            
            oWorkingUnit.LocationName = oReader.GetString("LocationName");
            oWorkingUnit.IsStore = oReader.GetBoolean("IsStore");
            oWorkingUnit.LOUNameCode = oReader.GetString("LOUNameCode");
            oWorkingUnit.BUID = oReader.GetInt32("BUID");
            oWorkingUnit.BUName = oReader.GetString("BUName");
            oWorkingUnit.UnitType = (EnumWoringUnitType)oReader.GetInt16("UnitType");
            oWorkingUnit.UnitTypeInt = oReader.GetInt16("UnitType");
            oWorkingUnit.LocationShortName = oReader.GetString("LocationShortName");
            return oWorkingUnit;
        }

        private WorkingUnit CreateObject(NullHandler oReader)
        {
            WorkingUnit oWorkingUnit = new WorkingUnit();
            oWorkingUnit = MapObject(oReader);
            return oWorkingUnit;
        }

        private List<WorkingUnit> CreateObjects(IDataReader oReader)
        {
            List<WorkingUnit> oWorkingUnit = new List<WorkingUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WorkingUnit oItem = CreateObject(oHandler);
                oWorkingUnit.Add(oItem);
            }
            return oWorkingUnit;
        }

        #endregion

        #region Interface implementation
        public WorkingUnitService() { }
        public List<WorkingUnit> GetsbyName(string sWorkingUnit, int nUserID)
        {
            List<WorkingUnit> oWorkingUnit = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = WorkingUnitDA.GetsbyName(tc, sWorkingUnit);
                NullHandler oReader = new NullHandler(reader);
                oWorkingUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }

            return oWorkingUnit;
        }
        public WorkingUnit Get(int id, int nUserId)
        {
            WorkingUnit oWorkingUnit = new WorkingUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = WorkingUnitDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWorkingUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }

            return oWorkingUnit;
        }
        public List<WorkingUnit> Gets(int nUserId)
        {
            List<WorkingUnit> oWorkingUnits = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WorkingUnitDA.Gets(tc);
                oWorkingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }

            return oWorkingUnits;
        }
        public List<WorkingUnit> Gets(int nLocationID, int nUserId)
        {
            List<WorkingUnit> oWorkingUnits = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WorkingUnitDA.Gets(tc, nLocationID);
                oWorkingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }

            return oWorkingUnits;
        }
        public List<WorkingUnit> Gets(string sSQL, int nUserId)
        {
            List<WorkingUnit> oWorkingUnits = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WorkingUnitDA.Gets(tc, sSQL);
                oWorkingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }

            return oWorkingUnits;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                WorkingUnit oWorkingUnit = new WorkingUnit();
                oWorkingUnit.WorkingUnitID = id;                
                WorkingUnitDA.Delete(tc, oWorkingUnit, EnumDBOperation.Delete, nUserId);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                WorkingUnit oWorkingUnit = new WorkingUnit();
                oWorkingUnit.ErrorMessage = e.Message.Split('!')[0];
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete WorkingUnit. Because of " + e.Message, e);
                #endregion
            }

        }
        public WorkingUnit UpdateForAcitivity(int nWorkingUnitID, int nUserId)
        {
            WorkingUnit oWorkingUnit = new WorkingUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = WorkingUnitDA.UpdateForAcitivity(tc, nWorkingUnitID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWorkingUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }

            return oWorkingUnit;
        }
        public WorkingUnit Save(WorkingUnit oWorkingUnit, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oWorkingUnit.WorkingUnitID <= 0)
                {
                    reader = WorkingUnitDA.InsertUpdate(tc, oWorkingUnit, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = WorkingUnitDA.InsertUpdate(tc, oWorkingUnit, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWorkingUnit = new WorkingUnit();
                    oWorkingUnit = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oWorkingUnit.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save WorkingUnit. Because of " + e.Message, e);
                #endregion
            }
            return oWorkingUnit;
        }
        public List<WorkingUnit> BUWiseGets(int nBUID, int nUserId)
        {
            List<WorkingUnit> oWorkingUnits = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WorkingUnitDA.BUWiseGets(tc, nBUID);
                oWorkingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }

            return oWorkingUnits;
        }
        public List<WorkingUnit> GetsPermittedStore(int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, int nUserID)
        {
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WorkingUnitDA.GetsPermittedStore(tc, nBUID, eModuleName, eStoreType, nUserID);
                oWorkingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }
            return oWorkingUnits;
        }
        public List<WorkingUnit> GetsPermittedStoreByStoreName(int nBUID, EnumModuleName eModuleName, EnumStoreType eStoreType, string sStoreName, int nUserID)
        {
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WorkingUnitDA.GetsPermittedStoreByStoreName(tc, nBUID, eModuleName, eStoreType, sStoreName, nUserID);
                oWorkingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkingUnit", e);
                #endregion
            }
            return oWorkingUnits;
        }
        public string WorkingUnit_AutoConfiguration(int nLocation_Assign, int nLocation_Source, int nUserID_Assign, int nUserID_Source, int nConType, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                WorkingUnitDA.WorkingUnit_AutoConfiguration(tc, nLocation_Assign, nLocation_Source, nUserID_Assign, nUserID_Source, nConType, nUserID);
                tc.End();
                return "Data save Successfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Update Approved", e);
                return e.Message;
                #endregion
            }
        }
        public List<WorkingUnit> Gets(string sDBObject, int nTRType, int nOEValue, int nInOutType, bool bDirection, int nPid, int nWUId, Int64 nUserID)
        {
            List<WorkingUnit> oWorkingUnits = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WorkingUnitDA.Gets(tc, sDBObject, nTRType, nOEValue, nInOutType, bDirection, nPid, nWUId, nUserID);

                oWorkingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oWorkingUnits;
        }
        #endregion

    }
        
}