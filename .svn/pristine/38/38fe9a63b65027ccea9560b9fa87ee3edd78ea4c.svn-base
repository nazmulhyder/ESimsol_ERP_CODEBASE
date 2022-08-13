using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{
        public class SampleTypeService : MarshalByRefObject, ISampleTypeService
    {
        #region Private functions and declaration
        private SampleType MapObject(NullHandler oReader)
        {
            SampleType oSampleType = new SampleType();
            oSampleType.SampleTypeID = oReader.GetInt32("SampleTypeID");
            oSampleType.Code = oReader.GetString("Code");
            oSampleType.SampleName = oReader.GetString("SampleName");
            oSampleType.Note = oReader.GetString("Note");
            return oSampleType;
        }

        private SampleType CreateObject(NullHandler oReader)
        {
            SampleType oSampleType = new SampleType();
            oSampleType = MapObject(oReader);
            return oSampleType;
        }

        private List<SampleType> CreateObjects(IDataReader oReader)
        {
            List<SampleType> oSampleType = new List<SampleType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleType oItem = CreateObject(oHandler);
                oSampleType.Add(oItem);
            }
            return oSampleType;
        }

        #endregion

        #region Interface implementation
        public SampleTypeService() { }

        public SampleType Save(SampleType oSampleType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSampleType.SampleTypeID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SampleType, EnumRoleOperationType.Add);
                    reader = SampleTypeDA.InsertUpdate(tc, oSampleType, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SampleType, EnumRoleOperationType.Edit);
                    reader = SampleTypeDA.InsertUpdate(tc, oSampleType, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleType = new SampleType();
                    oSampleType = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleType = new SampleType();
                oSampleType.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSampleType;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SampleType oSampleType = new SampleType();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SampleType, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "SampleType", id);
                oSampleType.SampleTypeID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SampleType, EnumRoleOperationType.Delete);
                SampleTypeDA.Delete(tc, oSampleType, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SampleType. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public SampleType Get(int id, Int64 nUserId)
        {
            SampleType oAccountHead = new SampleType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleTypeDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SampleType", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<SampleType> Gets(Int64 nUserID)
        {
            List<SampleType> oSampleType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleTypeDA.Gets(tc);
                oSampleType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleType", e);
                #endregion
            }

            return oSampleType;
        }

        public List<SampleType> Gets(string sSQL, Int64 nUserID)
        {
            List<SampleType> oSampleType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleTypeDA.Gets(tc, sSQL);
                oSampleType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleType", e);
                #endregion
            }

            return oSampleType;
        }

        #endregion
    }       
}
