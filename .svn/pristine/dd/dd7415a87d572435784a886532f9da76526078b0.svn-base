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
    public class GarmentsTypeService : MarshalByRefObject, IGarmentsTypeService
    {
        #region Private functions and declaration
        private GarmentsType MapObject(NullHandler oReader)
        {
            GarmentsType oGarmentsType = new GarmentsType();
            oGarmentsType.GarmentsTypeID = oReader.GetInt32("GarmentsTypeID");
            oGarmentsType.TypeName = oReader.GetString("TypeName");
            oGarmentsType.Note = oReader.GetString("Note");
            return oGarmentsType;
        }

        private GarmentsType CreateObject(NullHandler oReader)
        {
            GarmentsType oGarmentsType = new GarmentsType();
            oGarmentsType = MapObject(oReader);
            return oGarmentsType;
        }

        private List<GarmentsType> CreateObjects(IDataReader oReader)
        {
            List<GarmentsType> oGarmentsType = new List<GarmentsType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GarmentsType oItem = CreateObject(oHandler);
                oGarmentsType.Add(oItem);
            }
            return oGarmentsType;
        }

        #endregion

        #region Interface implementation
        public GarmentsTypeService() { }

        public GarmentsType Save(GarmentsType oGarmentsType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oGarmentsType.GarmentsTypeID <= 0)
                {
                    reader = GarmentsTypeDA.InsertUpdate(tc, oGarmentsType, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = GarmentsTypeDA.InsertUpdate(tc, oGarmentsType, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGarmentsType = new GarmentsType();
                    oGarmentsType = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oGarmentsType .ErrorMessage =   e.Message.Split('!')[0];
                #endregion
            }
            return oGarmentsType;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GarmentsType oGarmentsType = new GarmentsType();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.GarmentsType, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "GarmentsType", id);
                oGarmentsType.GarmentsTypeID = id;
                GarmentsTypeDA.Delete(tc, oGarmentsType, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public GarmentsType Get(int id, Int64 nUserId)
        {
            GarmentsType oAccountHead = new GarmentsType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GarmentsTypeDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get GarmentsType", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GarmentsType> Gets(Int64 nUserID)
        {
            List<GarmentsType> oGarmentsType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GarmentsTypeDA.Gets(tc);
                oGarmentsType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GarmentsType", e);
                #endregion
            }

            return oGarmentsType;
        }
        #endregion
    }
}
