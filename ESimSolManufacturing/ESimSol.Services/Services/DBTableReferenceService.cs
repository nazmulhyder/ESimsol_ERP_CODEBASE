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

    public class DBTableReferenceService : MarshalByRefObject, IDBTableReferenceService
    {
        #region Private functions and declaration
        private DBTableReference MapObject(NullHandler oReader)
        {
            DBTableReference oDBTableReference = new DBTableReference();
            oDBTableReference.DBTableReferenceID = oReader.GetInt32("DBTableReferenceID");
            oDBTableReference.MainTable = oReader.GetString("MainTable");
            oDBTableReference.ReferenceTable = oReader.GetString("ReferenceTable");
            oDBTableReference.ReferenceColumn = oReader.GetString("ReferenceColumn");
            return oDBTableReference;
        }

        private DBTableReference CreateObject(NullHandler oReader)
        {
            DBTableReference oDBTableReference = new DBTableReference();
            oDBTableReference = MapObject(oReader);
            return oDBTableReference;
        }

        private List<DBTableReference> CreateObjects(IDataReader oReader)
        {
            List<DBTableReference> oDBTableReference = new List<DBTableReference>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DBTableReference oItem = CreateObject(oHandler);
                oDBTableReference.Add(oItem);
            }
            return oDBTableReference;
        }

        #endregion

        #region DB Objects funcion maping
        private DBObject DOMapObject(NullHandler oReader)
        {
            DBObject oDBObject = new DBObject();
            oDBObject.ObjectName = oReader.GetString("name");
            return oDBObject;
        }

        private DBObject DOCreateObject(NullHandler oReader)
        {
            DBObject oDBObject = new DBObject();
            oDBObject = DOMapObject(oReader);
            return oDBObject;
        }

        private List<DBObject> DOCreateObjects(IDataReader oReader)
        {
            List<DBObject> oDBObjects = new List<DBObject>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DBObject oItem = DOCreateObject(oHandler);
                oDBObjects.Add(oItem);
            }
            return oDBObjects;
        }

        #endregion

        #region DB Objects Column funcion maping
        private DBObjectColumn ColumnMapObject(NullHandler oReader)
        {
            DBObjectColumn oDBObjectColumn = new DBObjectColumn();
            oDBObjectColumn.ColumnName = oReader.GetString("ColumnName");
            return oDBObjectColumn;
        }

        private DBObjectColumn ColumnCreateObject(NullHandler oReader)
        {
            DBObjectColumn oDBObjectColumn = new DBObjectColumn();
            oDBObjectColumn = ColumnMapObject(oReader);
            return oDBObjectColumn;
        }

        private List<DBObjectColumn> ColumnCreateObjects(IDataReader oReader)
        {
            List<DBObjectColumn> oDBObjectColumns = new List<DBObjectColumn>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DBObjectColumn oItem = ColumnCreateObject(oHandler);
                oDBObjectColumns.Add(oItem);
            }
            return oDBObjectColumns;
        }

        #endregion

        #region Interface implementation
        public DBTableReferenceService() { }

        public DBTableReference Save(DBTableReference oDBTableReference, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDBTableReference.DBTableReferenceID <= 0)
                {

                    //AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, "DBTableReference", EnumRoleOperationType.Add);
                    reader = DBTableReferenceDA.InsertUpdate(tc, oDBTableReference, EnumDBOperation.Insert, nCurrentUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, "DBTableReference", EnumRoleOperationType.Edit);
                    reader = DBTableReferenceDA.InsertUpdate(tc, oDBTableReference, EnumDBOperation.Update, nCurrentUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDBTableReference = new DBTableReference();
                    oDBTableReference = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDBTableReference = new DBTableReference();
                oDBTableReference.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDBTableReference;
        }
        public string Delete(int id, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DBTableReference oDBTableReference = new DBTableReference();
                oDBTableReference.DBTableReferenceID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, "DBTableReference", EnumRoleOperationType.Delete);
                DBTableReferenceDA.Delete(tc, oDBTableReference, EnumDBOperation.Delete, nCurrentUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete DBTableReference. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public DBTableReference Get(int id, int nCurrentUserID)
        {
            DBTableReference oAccountHead = new DBTableReference();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DBTableReferenceDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DBTableReference", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DBTableReference> Gets(int nCurrentUserID)
        {
            List<DBTableReference> oDBTableReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DBTableReferenceDA.Gets(tc);
                oDBTableReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DBTableReference", e);
                #endregion
            }

            return oDBTableReference;
        }

        public List<DBTableReference> Gets(string sSQL, int nCurrentUserID)
        {
            List<DBTableReference> oDBTableReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DBTableReferenceDA.Gets(tc, sSQL);
                oDBTableReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DBTableReference", e);
                #endregion
            }

            return oDBTableReference;
        }
        public List<DBObject> GetsDBObject(int nCurrentUserID)
        {
            List<DBObject> oDBObjects = new List<DBObject>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DBTableReferenceDA.GetsDBObject(tc);
                oDBObjects = DOCreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Object Name because of : ", e);
                #endregion
            }

            return oDBObjects;
        }

        public List<DBObjectColumn> GetsDBObjectColumn(string ObjectName, int nCurrentUserID)
        {
            List<DBObjectColumn> oDBObjectColumns = new List<DBObjectColumn>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DBTableReferenceDA.GetsDBObjectColumn(tc, ObjectName);
                oDBObjectColumns = ColumnCreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Object Name because of : ", e);
                #endregion
            }

            return oDBObjectColumns;
        }

        #endregion
    }   
    
    
}
