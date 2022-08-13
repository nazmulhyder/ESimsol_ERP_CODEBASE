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

    #region DBTableReference
    
    public class DBTableReference : BusinessObject
    {
        public DBTableReference()
        {
            DBTableReferenceID = 0;
            MainTable = "";
            ReferenceTable = "";
            ReferenceColumn = "";
            ErrorMessage = "";

        }

        #region Properties
        
        public int DBTableReferenceID { get; set; }
        
        public string MainTable { get; set; }
        
        public string ReferenceTable { get; set; }
        
        public string ReferenceColumn { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<DBTableReference> DBTableReferences { get; set; }
        public List<DBObject> MainTables { get; set; }
        public List<DBObject> ReferenceTables { get; set; }
        public List<DBObjectColumn> ReferenceColumns { get; set; }
             
        
        #endregion

        #region Functions

        public static List<DBTableReference> Gets(int nCurrentUserID)
        {
            return DBTableReference.Service.Gets(nCurrentUserID);
        }
        public static List<DBTableReference> Gets(string sSQL, int nCurrentUserID)
        {
            return DBTableReference.Service.Gets(sSQL, nCurrentUserID);
        }

        public DBTableReference Get(int id, int nCurrentUserID)
        {
            return DBTableReference.Service.Get(id, nCurrentUserID);
        }

        public DBTableReference Save(int nCurrentUserID)
        {
            return DBTableReference.Service.Save(this, nCurrentUserID);
        }

        public string Delete(int id, int nCurrentUserID)
        {
            return DBTableReference.Service.Delete(id, nCurrentUserID);
        }
        public static List<DBObject> GetsDBObject(int nCurrentUserID)
        {
            return DBTableReference.Service.GetsDBObject(nCurrentUserID);
        }

        public static List<DBObjectColumn> GetsDBObjectColumn(string ObjectName, int nCurrentUserID)
        {
            return DBTableReference.Service.GetsDBObjectColumn(ObjectName, nCurrentUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDBTableReferenceService Service
        {
            get { return (IDBTableReferenceService)Services.Factory.CreateService(typeof(IDBTableReferenceService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class DBTableReferenceList : List<DBTableReference>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IDBTableReference interface
    
    public interface IDBTableReferenceService
    {
        
        DBTableReference Get(int id, int nCurrentUserID);
        
        List<DBTableReference> Gets(int nCurrentUserID);
        
        List<DBTableReference> Gets(string sSQL, int nCurrentUserID);
        
        string Delete(int id, int nCurrentUserID);
        
        DBTableReference Save(DBTableReference oDBTableReference, int nCurrentUserID);
        List<DBObject> GetsDBObject(int nCurrentUserID);

        List<DBObjectColumn> GetsDBObjectColumn(string ObjectName, int nCurrentUserID);
        
    }
    #endregion

  
}
