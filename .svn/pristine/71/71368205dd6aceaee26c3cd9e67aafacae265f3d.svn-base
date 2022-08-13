using System;
using System.Data;
using System.Data.OleDb;
using ICS.Core.DataAccess; 


namespace ICS.Core.DataAccess.OleDb
{
    #region DataAccess: OleDb Helper Extension
    internal sealed class OleDbHelperExtension
    {
        #region Constructor
        private OleDbHelperExtension() { }
        #endregion

        #region Public function
        /// <summary>
        /// Fills a typed DataSet using the DataReader's current result. This method 
        /// allows paginated access to the database.
        /// </summary>
        /// <param name="dataReader">The DataReader used to fetch the values.</param>
        /// <param name="dataSet">The DataSet used to store the values.</param>
        /// <param name="tableName">The name of the DataSet table used to add the 
        /// DataReader records.</param>
        /// <param name="from">The quantity of records skipped before placing
        /// values on the DataReader on the DataSet.</param>
        /// <param name="count">The maximum quantity of records alloed to fill on the
        /// DataSet.</param>
        public static void Fill(IDataReader dataReader, DataSet dataSet, string tableName, int from, int count)
        {
            string fieldName;
            DataTable fillTable; DataRow fillRow;
            int recNumber = 0; int totalRecords = from + count;

            //If table name is null then set the string 
            if (tableName == null)
            {
                tableName = "unknownTable";
            }
            //If table does not exist in data set then add table
            if (dataSet.Tables[tableName] == null)
            {
                dataSet.Tables.Add(tableName);
            }
            // Get the DataTable reference
            if (tableName == null)
            {
                fillTable = dataSet.Tables[0];
            }
            else
            {
                fillTable = dataSet.Tables[tableName];
            }

            while (dataReader.Read())
            {
                if (recNumber++ >= from)
                {
                    fillRow = fillTable.NewRow();
                    for (int fieldIdx = 0; fieldIdx < dataReader.FieldCount; fieldIdx++)
                    {
                        fieldName = dataReader.GetName(fieldIdx);
                        Type fieldType = dataReader.GetValue(fieldIdx).GetType();
                        if (fillTable.Columns.IndexOf(fieldName) == -1)
                        {
                            fillTable.Columns.Add(fieldName, fieldType);
                        }
                        fillRow[fieldName] = GetValue(dataReader.GetValue(fieldIdx), fieldType);
                    }
                    fillTable.Rows.Add(fillRow);
                }
                if (count != 0 && totalRecords <= recNumber)
                {
                    break;
                }
            }
            dataSet.AcceptChanges();
        }

        public static void Fill(IDataReader dataReader, DataSet dataSet, string tableName)
        {
            Fill(dataReader, dataSet, tableName, 0, 0);
        }

        #region Parameter for Stored Procedure
        public static OleDbParameter CreateParam(ParameterDirection pDirection, OleDbType pType, object pValue)
        {
            OleDbParameter param = new OleDbParameter();
            param.Direction = pDirection;
            param.OleDbType = pType;
            if (pType == OleDbType.VarChar)
            {
                param.Size = 200;
            }
            param.Value = pValue;

            return param;
        }

        public static OleDbParameter CreateParam(string pName, OleDbType pType, ParameterDirection pDirection, object pValue)
        {
            OleDbParameter param = new OleDbParameter(pName, pType);
            param.Direction = pDirection;
            if (pType == OleDbType.VarChar)
            {
                param.Size = 200;
            }
            param.Value = pValue;

            return param;
        }

        public static OleDbParameter CreateParam(string pName, OleDbType pType, ParameterDirection pDirection)
        {
            OleDbParameter param = new OleDbParameter(pName, pType);

            param.Direction = pDirection;
            if (pType == OleDbType.VarChar)
            {
                param.Size = 200;
            }

            return param;
        }

        #endregion
        #endregion

        #region private function
        private static object GetValue(object fieldValue, Type fieldType)
        {
            object retValue = DBNull.Value;
            switch (fieldType.Name)
            {
                case "Int16":
                    if (fieldValue == DBNull.Value)
                    {
                        retValue = 0;
                    }
                    else
                    {
                        retValue = fieldType;
                    }
                    break;

            }
            return retValue;
        }
        #endregion
    }
    #endregion
}
