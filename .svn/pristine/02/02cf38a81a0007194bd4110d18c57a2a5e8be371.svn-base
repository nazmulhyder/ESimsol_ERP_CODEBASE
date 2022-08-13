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

    public class ITransactionGRNService : MarshalByRefObject, IITransactionGRNService
    {
        #region Private functions and declaration


        private ITransactionGRN MapObject(NullHandler oReader)
        {
            ITransactionGRN oITransactionGRN = new ITransactionGRN();

            oITransactionGRN.StartDate = oReader.GetDateTime("StartDate");
            oITransactionGRN.GRNType = (EnumGRNType)oReader.GetInt16("GRNType");
            oITransactionGRN.GRNTypeint = oReader.GetInt16("GRNType");
            oITransactionGRN.ContractorName = oReader.GetString("ContractorName");
            oITransactionGRN.LocationName = oReader.GetString("LocationName");
            oITransactionGRN.OperationUnitName = oReader.GetString("OperationUnitName");
            oITransactionGRN.LCNo = oReader.GetString("LCNo");
            oITransactionGRN.GRNNo = oReader.GetString("GRNNo");
            oITransactionGRN.InvoiceNo = oReader.GetString("InvoiceNo");
            oITransactionGRN.ProductCode = oReader.GetString("ProductCode");
            oITransactionGRN.ProductName = oReader.GetString("ProductName");
            oITransactionGRN.Qty_Invoice = oReader.GetDouble("Qty_Invoice");
            oITransactionGRN.UnitName = oReader.GetString("UnitName");
            oITransactionGRN.LotNo = oReader.GetString("LotNo");
            //oITransactionGRN.PreviousStockInQty = oReader.GetDouble("PreviousStockInQty");
            //oITransactionGRN.LCValue = oReader.GetDouble("LCValue");
            oITransactionGRN.UnitPrice = oReader.GetDouble("UnitPrice");
            oITransactionGRN.Qty = oReader.GetDouble("Qty");
            oITransactionGRN.RefObjectID = oReader.GetInt32("RefObjectID");

            return oITransactionGRN;
        }

        private ITransactionGRN CreateObject(NullHandler oReader)
        {
            ITransactionGRN oITransactionGRN = new ITransactionGRN();
            oITransactionGRN = MapObject(oReader);
            return oITransactionGRN;
        }

        private List<ITransactionGRN> CreateObjects(IDataReader oReader)
        {
            List<ITransactionGRN> oITransactionGRN = new List<ITransactionGRN>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITransactionGRN oItem = CreateObject(oHandler);
                oITransactionGRN.Add(oItem);
            }
            return oITransactionGRN;
        }
        #endregion

        #region Interface implementation
        public ITransactionGRNService() { }

        public List<ITransactionGRN> Gets(DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType, int nImportProductID, Int64 nUserId)
        {
            List<ITransactionGRN> oITransactionGRNs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITransactionGRNDA.Gets(tc, dStartDate, dEndDate, nOrderType, nReportType, nImportProductID);
                oITransactionGRNs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITransactionGRN", e);
                #endregion
            }

            return oITransactionGRNs;
        }

        #endregion
    }


}
