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
    [Serializable]
    public class PrintService : MarshalByRefObject, IPrintService
    {
        #region Private functions and declaration
        private Print MapObject(NullHandler oReader)
        {
            Print oPrint = new Print();
            oPrint.PrintID = oReader.GetInt32("PrintID");
            oPrint.ReportCode = oReader.GetString("ReportCode");
            oPrint.ReportName = oReader.GetString("ReportName");
            oPrint.ObjectID = oReader.GetInt32("ObjectID");
            oPrint.NumberOfPrint = oReader.GetInt32("NumberOfPrint");
            oPrint.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            return oPrint;
        }

        private Print CreateObject(NullHandler oReader)
        {
            Print oPrint = new Print();
            oPrint = MapObject(oReader);
            return oPrint;
        }

        private List<Print> CreateObjects(IDataReader oReader)
        {
            List<Print> oPrints = new List<Print>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Print oItem = CreateObject(oHandler);
                oPrints.Add(oItem);
            }
            return oPrints;
        }
        #endregion

        #region Interface implementation
        public PrintService() { }

        public Print IU(Print oPrint, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PrintDA.IU(tc, oPrint, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPrint = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPrint = new Print();
                if (tc != null) { tc.HandleError(); }
                ExceptionLog.Write(e);
                oPrint.ErrorMessage = e.Message;
                #endregion
            }
            return oPrint;
        }
        #endregion
    }
}
