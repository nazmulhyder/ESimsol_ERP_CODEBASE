using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

using ICS.Core.Utility;
using ICS.Core.Framework;

//Please don not delete. it is our 2nd phase development
namespace ESimSol.Services.Services
{

    public class TableColumnService : MarshalByRefObject, ITableColumnService
    {
        #region Private functions and declaration
        
        private TableColumn MapObject(NullHandler oReader, bool isTable)
        {
            TableColumn oTableColumn = new TableColumn();            
            if (isTable)
            { 
                oTableColumn.Name = oReader.GetString("Name"); 
            }
            else
            {
                oTableColumn.Name = oReader.GetString("COLUMN_NAME");
            }
            return oTableColumn;
        }

        private TableColumn CreateObject(NullHandler oReader, bool isTable)
        {
            TableColumn oTableColumn = new TableColumn();
            oTableColumn = MapObject(oReader,isTable);
            return oTableColumn;
        }

        private List<TableColumn> CreateObjects(IDataReader oReader, bool isTable)
        {
            List<TableColumn> oTableColumn = new List<TableColumn>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TableColumn oItem = CreateObject(oHandler,isTable);
                oTableColumn.Add(oItem);
            }
            return oTableColumn;
        }

        #endregion

        #region Interface implementation
        public TableColumnService() { }

        public List<TableColumn> GetsTable(string sDbName, Int64 nUserId)
        {
            List<TableColumn> oTableColumn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TableColumnDA.GetsTable(tc,sDbName);
                oTableColumn = CreateObjects(reader,true);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TableColumn", e);
                #endregion
            }

            return oTableColumn;
        }

        public List<TableColumn> GetsColumn(string sDbName, string sTableName, Int64 nUserId)
        {
            List<TableColumn> oTableColumn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TableColumnDA.GetsColumn(tc, sDbName,sTableName);
                oTableColumn = CreateObjects(reader,false);
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

            return oTableColumn;
        }

        public List<TableColumn> GetsViews(string sDbName, Int64 nUserId)
        {
            List<TableColumn> oTableColumn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TableColumnDA.GetsViews(tc, sDbName);
                oTableColumn = CreateObjects(reader, true);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Views", e);
                #endregion
            }

            return oTableColumn;
        }

        #endregion
    }   
   
}
