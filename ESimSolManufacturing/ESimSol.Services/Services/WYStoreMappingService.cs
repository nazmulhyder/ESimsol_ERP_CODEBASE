using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class WYStoreMappingService : MarshalByRefObject, IWYStoreMappingService
    {
        #region Private functions and declaration
        private WYStoreMapping MapObject(NullHandler oReader)
        {
            WYStoreMapping oWYStoreMapping = new WYStoreMapping();
            oWYStoreMapping.WYStoreMappingID = oReader.GetInt32("WYStoreMappingID");
            oWYStoreMapping.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oWYStoreMapping.WYStoreType = (EnumStoreType)oReader.GetInt32("WYStoreType");
            oWYStoreMapping.WYarnType = (EnumWYarnType)oReader.GetInt32("WYarnType");
            oWYStoreMapping.WYStoreTypeInt = oReader.GetInt32("WYStoreType");
            oWYStoreMapping.WYarnTypeInt = oReader.GetInt32("WYarnType");
            oWYStoreMapping.StoreName = oReader.GetString("StoreName");
            oWYStoreMapping.Note = oReader.GetString("Note");
            return oWYStoreMapping;
        }

        private WYStoreMapping CreateObject(NullHandler oReader)
        {
            WYStoreMapping oWYStoreMapping = new WYStoreMapping();
            oWYStoreMapping = MapObject(oReader);
            return oWYStoreMapping;
        }

        private List<WYStoreMapping> CreateObjects(IDataReader oReader)
        {
            List<WYStoreMapping> oWYStoreMappings = new List<WYStoreMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WYStoreMapping oItem = CreateObject(oHandler);
                oWYStoreMappings.Add(oItem);
            }
            return oWYStoreMappings;
        }

        #endregion

        #region Interface implementation
        public WYStoreMappingService() { }


        public WYStoreMapping Save(WYStoreMapping oWYStoreMapping, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region WYStoreMapping
                IDataReader reader;
                if (oWYStoreMapping.WYStoreMappingID <= 0)
                {
                    reader = WYStoreMappingDA.InsertUpdate(tc, oWYStoreMapping, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = WYStoreMappingDA.InsertUpdate(tc, oWYStoreMapping, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYStoreMapping = new WYStoreMapping();
                    oWYStoreMapping = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oWYStoreMapping = new WYStoreMapping();
                oWYStoreMapping.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oWYStoreMapping;
        }
        public WYStoreMapping ToggleActivity(WYStoreMapping oWYStoreMapping, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = WYStoreMappingDA.ToggleActivity(tc, oWYStoreMapping);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYStoreMapping = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oWYStoreMapping = new WYStoreMapping();
                oWYStoreMapping.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oWYStoreMapping;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            
            string sMessage = "Delete sucessfully";
            try
            {
                tc = TransactionContext.Begin(true);
                WYStoreMapping oWYStoreMapping = new WYStoreMapping();
                oWYStoreMapping.WYStoreMappingID = id;
                WYStoreMappingDA.Delete(tc, oWYStoreMapping, EnumDBOperation.Delete, nUserId);
               
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = "Delete operation could not complete";
            }

            return sMessage;
        }

        public WYStoreMapping Get(int id, Int64 nUserId)
        {
            WYStoreMapping oWYStoreMapping = new WYStoreMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = WYStoreMappingDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWYStoreMapping = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oWYStoreMapping;
        }
        public List<WYStoreMapping> GetsActive(Int64 nUserId)
        {
            List<WYStoreMapping> oWYStoreMappings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WYStoreMappingDA.GetsActive(tc);
                oWYStoreMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RS Shift", e);
                #endregion
            }

            return oWYStoreMappings;
        }
        public List<WYStoreMapping> GetsByModule(int nBUID, string sModuleIDs, Int64 nUserId)
        {
            List<WYStoreMapping> oWYStoreMappings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WYStoreMappingDA.GetsByModule(tc, nBUID, sModuleIDs);
                oWYStoreMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oWYStoreMappings;
        }

        public List<WYStoreMapping> Gets(Int64 nUserId)
        {
            List<WYStoreMapping> oWYStoreMappings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WYStoreMappingDA.Gets(tc);
                oWYStoreMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oWYStoreMappings;
        }

        public List<WYStoreMapping> Gets(string sSQL, Int64 nUserId)
        {
            List<WYStoreMapping> oWYStoreMappings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WYStoreMappingDA.Gets(tc, sSQL);
                oWYStoreMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oWYStoreMappings;
        }


        #endregion
    }
}