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
    public class DBConnectionService : MarshalByRefObject, IDBConnectionService
    {
        #region Private functions and declaration
        private DBConnection MapObject(NullHandler oReader)
        {
            DBConnection oDBConnection = new DBConnection();
            oDBConnection.DBConnectionID = oReader.GetInt32("DBConnectionID");
            oDBConnection.ProjectName = oReader.GetString("ProjectName");
            oDBConnection.Description = oReader.GetString("Description");
            oDBConnection.ServerName = oReader.GetString("ServerName");
            oDBConnection.UserID = oReader.GetString("UserID");
            oDBConnection.Password = oReader.GetString("Password");
            oDBConnection.DBName = oReader.GetString("DBName");            
            return oDBConnection;           
        }

        private DBConnection CreateObject(NullHandler oReader)
        {
            DBConnection oDBConnection = new DBConnection();
            oDBConnection = MapObject(oReader);
            return oDBConnection;
        }

        private List<DBConnection> CreateObjects(IDataReader oReader)
        {
            List<DBConnection> oDBConnection = new List<DBConnection>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DBConnection oItem = CreateObject(oHandler);
                oDBConnection.Add(oItem);
            }
            return oDBConnection;
        }

        #endregion

        #region Interface implementation
        public DBConnectionService() { }

        public DBConnection Save(DBConnection oDBConnection, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDBConnection.DBConnectionID <= 0)
                {
                    reader = DBConnectionDA.InsertUpdate(tc, oDBConnection, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = DBConnectionDA.InsertUpdate(tc, oDBConnection, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDBConnection = new DBConnection();
                    oDBConnection = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DBConnection. Because of " + e.Message, e);
                #endregion
            }
            return oDBConnection;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DBConnection oDBConnection = new DBConnection();
                oDBConnection.DBConnectionID = id;
                DBConnectionDA.Delete(tc, oDBConnection, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete DBConnection. Because of " + e.Message, e);
                #endregion
            }
            return "Delete Successfully";
        }

        public DBConnection Get(int id, Int64 nUserId)
        {
            DBConnection oAccountHead = new DBConnection();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DBConnectionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DBConnection", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DBConnection> Gets(Int64 nUserId)
        {
            List<DBConnection> oDBConnection = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DBConnectionDA.Gets(tc);
                oDBConnection = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DBConnection", e);
                #endregion
            }

            return oDBConnection;
        }
        #endregion
    }
}
