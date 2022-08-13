using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region DBConnection
    
    public class DBConnection : BusinessObject
    {
        public DBConnection()
        {
            DBConnectionID = 0;            
            ProjectName = "";
            Description = "";
            ServerName="";
            UserID="";
            Password="";
            DBName="";
            ErrorMessage = "";
        }

        #region Properties
        
        public int DBConnectionID { get; set; }       
        
        public string ProjectName { get; set; }
        
        public string Description { get; set; }
        
        public string ServerName { get; set; }
        
        public string UserID { get; set; }
        
        public string Password { get; set; }
        
        public string DBName { get; set; }
        
        public string ErrorMessage { get; set; }

        #region Derived Property
        public string ConnectionName
        {
            get { return "Server :" + ServerName + ", DB :" + DBName; }
        }
        #endregion


        #endregion




        #region Functions

        public static List<DBConnection> Gets(long nUserID)
        {
            return DBConnection.Service.Gets(nUserID);
        }

        public DBConnection Get(int nId, long nUserID)
        {
            return DBConnection.Service.Get(nId, nUserID);
        }

        public DBConnection Save(long nUserID)
        {
            return DBConnection.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return DBConnection.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDBConnectionService Service
        {
            get { return (IDBConnectionService)Services.Factory.CreateService(typeof(IDBConnectionService)); }
        }
        #endregion
    }
    #endregion

    #region DBConnections
    public class DBConnections : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(DBConnection item)
        {
            base.AddItem(item);
        }
        public void Remove(DBConnection item)
        {
            base.RemoveItem(item);
        }
        public DBConnection this[int index]
        {
            get { return (DBConnection)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IDBConnection interface
    
    public interface IDBConnectionService
    {
        
        DBConnection Get(int id, long nUserID);
        
        List<DBConnection> Gets(long nUserID);
        
        string Delete(int id, long nUserID);
        
        DBConnection Save(DBConnection oDBConnection, long nUserID);
    }
    #endregion
}
