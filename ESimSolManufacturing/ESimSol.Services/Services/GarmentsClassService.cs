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
    public class GarmentsClassService : MarshalByRefObject, IGarmentsClassService
    {
        #region Private functions and declaration
        private GarmentsClass MapObject(NullHandler oReader)
        {
            GarmentsClass oGarmentsClass = new GarmentsClass();
            oGarmentsClass.GarmentsClassID = oReader.GetInt32("GarmentsClassID");
            oGarmentsClass.ClassName = oReader.GetString("ClassName");
            oGarmentsClass.ParentClassID = oReader.GetInt32("ParentClassID");
            oGarmentsClass.Note = oReader.GetString("Note");
            return oGarmentsClass;
        }

        private GarmentsClass CreateObject(NullHandler oReader)
        {
            GarmentsClass oGarmentsClass = new GarmentsClass();
            oGarmentsClass = MapObject(oReader);
            return oGarmentsClass;
        }

        private List<GarmentsClass> CreateObjects(IDataReader oReader)
        {
            List<GarmentsClass> oGarmentsClass = new List<GarmentsClass>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GarmentsClass oItem = CreateObject(oHandler);
                oGarmentsClass.Add(oItem);
            }
            return oGarmentsClass;
        }

        #endregion

        #region Interface implementation
        public GarmentsClassService() { }

        public GarmentsClass Save(GarmentsClass oGarmentsClass, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oGarmentsClass.GarmentsClassID <= 0)
                {
                    reader = GarmentsClassDA.InsertUpdate(tc, oGarmentsClass, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = GarmentsClassDA.InsertUpdate(tc, oGarmentsClass, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGarmentsClass = new GarmentsClass();
                    oGarmentsClass = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGarmentsClass.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save GarmentsClass. Because of " + e.Message, e);
                #endregion
            }
            return oGarmentsClass;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GarmentsClass oGarmentsClass = new GarmentsClass();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.GarmentsClass, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "GarmentsClass", id);
                oGarmentsClass.GarmentsClassID = id;
                GarmentsClassDA.Delete(tc, oGarmentsClass, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GarmentsClass. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public GarmentsClass Get(int id, Int64 nUserId)
        {
            GarmentsClass oAccountHead = new GarmentsClass();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GarmentsClassDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get GarmentsClass", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GarmentsClass> Gets(Int64 nUserID)
        {
            List<GarmentsClass> oGarmentsClass = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GarmentsClassDA.Gets(tc);
                oGarmentsClass = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GarmentsClass", e);
                #endregion
            }

            return oGarmentsClass;
        }

        public List<GarmentsClass> GetsGarmentsClass(Int64 nUserID)
        {
            List<GarmentsClass> oGarmentsClass = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GarmentsClassDA.GetsGarmentsClass(tc);
                oGarmentsClass = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GarmentsClass", e);
                #endregion
            }

            return oGarmentsClass;
        }

        public List<GarmentsClass> GetsGarmentsSubClass(Int64 nUserID)
        {
            List<GarmentsClass> oGarmentsClass = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GarmentsClassDA.GetsGarmentsSubClass(tc);
                oGarmentsClass = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GarmentsClass", e);
                #endregion
            }

            return oGarmentsClass;
        }

        public List<GarmentsClass> Gets(string sSQL, Int64 nUserID)
        {
            List<GarmentsClass> oGarmentsClass = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GarmentsClassDA.Gets(tc, sSQL);
                oGarmentsClass = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GarmentsClass", e);
                #endregion
            }

            return oGarmentsClass;
        }
        #endregion
    }
}
