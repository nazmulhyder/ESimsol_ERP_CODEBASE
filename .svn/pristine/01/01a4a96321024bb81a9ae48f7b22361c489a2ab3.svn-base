using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


//Please don not delete. it is our 2nd phase development
namespace ESimSol.BusinessObjects
{
    #region TableColumn
    
    public class TableColumn : BusinessObject
    {
        public TableColumn()
        {
            
            Name = "";
        }

        #region Properties       
        
        public string Name { get; set; }
        
        public string ErrorMessage { get; set; }

        #endregion

        #region Functions

        public static List<TableColumn> GetsTable(string sDbName, long nUserID)
        {
            return TableColumn.Service.GetsTable(sDbName, nUserID);
        }

        public static List<TableColumn> GetsViews(string sDbName, long nUserID)
        {
            return TableColumn.Service.GetsViews(sDbName, nUserID);
        }

        public static List<TableColumn> GetsColumn(string sDbName, string sTableName, long nUserID)
        {
            return TableColumn.Service.GetsColumn(sDbName, sTableName, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ITableColumnService Service
        {
            get { return (ITableColumnService)Services.Factory.CreateService(typeof(ITableColumnService)); }
        }
        #endregion
    }
    #endregion

    #region ITableColumn interface
    
    public interface ITableColumnService
    {
        List<TableColumn> GetsTable(string sDbName, long nUserID);
        
        List<TableColumn> GetsViews(string sDbName, long nUserID);
        
        List<TableColumn> GetsColumn(string sDbName, string sTableName, long nUserID);
    }
    #endregion

}
