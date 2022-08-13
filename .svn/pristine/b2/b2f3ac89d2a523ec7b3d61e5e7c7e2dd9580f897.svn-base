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
    public class SupplierRateProcessService : MarshalByRefObject, ISupplierRateProcessService
    {
        #region Private functions and declaration
        public static SupplierRateProcess MapObject(NullHandler oReader)
        {
            SupplierRateProcess oSupplierRateProcess = new SupplierRateProcess();
            oSupplierRateProcess.ProductID = oReader.GetInt32("ProductID");
            oSupplierRateProcess.SupplierID = oReader.GetInt32("SupplierID");
            oSupplierRateProcess.ProductName = oReader.GetString("ProductName");
            oSupplierRateProcess.RequiredQty = oReader.GetDouble("RequiredQty");
            oSupplierRateProcess.PurchaseQty = oReader.GetDouble("PurchaseQty");
            oSupplierRateProcess.UnitName = oReader.GetString("UnitName");
            oSupplierRateProcess.UnitSymbol = oReader.GetString("UnitSymbol");
            oSupplierRateProcess.MUnitID = oReader.GetInt32("MUnitID");
            oSupplierRateProcess.SupplierID1 = oReader.GetInt32("SupplierID1");
            oSupplierRateProcess.SupplierName1 = oReader.GetString("SupplierName1");
            oSupplierRateProcess.Rate1 = oReader.GetString("Rate1");
            oSupplierRateProcess.SupplierID2 = oReader.GetInt32("SupplierID2");
            oSupplierRateProcess.SupplierName2 = oReader.GetString("SupplierName2");
            oSupplierRateProcess.Rate2 = oReader.GetString("Rate2");
            oSupplierRateProcess.SupplierID3 = oReader.GetInt32("SupplierID3");
            oSupplierRateProcess.SupplierName3 = oReader.GetString("SupplierName3");
            oSupplierRateProcess.Rate3 = oReader.GetString("Rate3");
            oSupplierRateProcess.SupplierID4 = oReader.GetInt32("SupplierID4");
            oSupplierRateProcess.SupplierName4 = oReader.GetString("SupplierName4");
            oSupplierRateProcess.Rate4 = oReader.GetString("Rate4");
            oSupplierRateProcess.SupplierID5 = oReader.GetInt32("SupplierID5");
            oSupplierRateProcess.SupplierName5 = oReader.GetString("SupplierName5");
            oSupplierRateProcess.Rate5 = oReader.GetString("Rate5");
            oSupplierRateProcess.SupplierID6 = oReader.GetInt32("SupplierID6");
            oSupplierRateProcess.SupplierName6 = oReader.GetString("SupplierName6");
            oSupplierRateProcess.Rate6 = oReader.GetString("Rate6");
            oSupplierRateProcess.SupplierID7 = oReader.GetInt32("SupplierID7");
            oSupplierRateProcess.SupplierName7 = oReader.GetString("SupplierName7");
            oSupplierRateProcess.Rate7 = oReader.GetString("Rate7");
            oSupplierRateProcess.SupplierID8 = oReader.GetInt32("SupplierID8");
            oSupplierRateProcess.SupplierName8 = oReader.GetString("SupplierName8");
            oSupplierRateProcess.Rate8 = oReader.GetString("Rate8");
            oSupplierRateProcess.SupplierID9 = oReader.GetInt32("SupplierID9");
            oSupplierRateProcess.SupplierName9 = oReader.GetString("SupplierName9");
            oSupplierRateProcess.Rate9 = oReader.GetString("Rate9");
            oSupplierRateProcess.SupplierID10 = oReader.GetInt32("SupplierID10");
            oSupplierRateProcess.SupplierName10 = oReader.GetString("SupplierName10");
            oSupplierRateProcess.Rate10 = oReader.GetString("Rate10");
            oSupplierRateProcess.SupplierID11 = oReader.GetInt32("SupplierID11");
            oSupplierRateProcess.SupplierName11 = oReader.GetString("SupplierName11");
            oSupplierRateProcess.Rate11 = oReader.GetString("Rate11");
            oSupplierRateProcess.SupplierID12 = oReader.GetInt32("SupplierID12");
            oSupplierRateProcess.SupplierName12 = oReader.GetString("SupplierName12");
            oSupplierRateProcess.Rate12 = oReader.GetString("Rate12");
            oSupplierRateProcess.SupplierID13 = oReader.GetInt32("SupplierID13");
            oSupplierRateProcess.SupplierName13 = oReader.GetString("SupplierName13");
            oSupplierRateProcess.Rate13 = oReader.GetString("Rate13");
            oSupplierRateProcess.SupplierID14 = oReader.GetInt32("SupplierID14");
            oSupplierRateProcess.SupplierName14 = oReader.GetString("SupplierName14");
            oSupplierRateProcess.Rate14 = oReader.GetString("Rate14");
            oSupplierRateProcess.SupplierID15 = oReader.GetInt32("SupplierID15");
            oSupplierRateProcess.SupplierName15 = oReader.GetString("SupplierName15");
            oSupplierRateProcess.Rate15 = oReader.GetString("Rate15");
            oSupplierRateProcess.UnitPrice = oReader.GetDouble("UnitPrice");
            oSupplierRateProcess.NOADetailID = oReader.GetInt32("NOADetailID");
            oSupplierRateProcess.Note = oReader.GetString("Note");
            oSupplierRateProcess.UnitSymbol = oReader.GetString("UnitSymbol");
            oSupplierRateProcess.DeliveryFromStockQty = oReader.GetDouble("DeliveryFromStockQty");
            
            oSupplierRateProcess.PQDetailID1 = oReader.GetInt32("PQDetailID1");
            oSupplierRateProcess.PQDetailID2 = oReader.GetInt32("PQDetailID2");
            oSupplierRateProcess.PQDetailID3 = oReader.GetInt32("PQDetailID3");
            oSupplierRateProcess.PQDetailID4 = oReader.GetInt32("PQDetailID4");
            oSupplierRateProcess.PQDetailID5 = oReader.GetInt32("PQDetailID5");
            oSupplierRateProcess.PQDetailID6 = oReader.GetInt32("PQDetailID6");
            oSupplierRateProcess.PQDetailID7 = oReader.GetInt32("PQDetailID7");
            oSupplierRateProcess.PQDetailID8 = oReader.GetInt32("PQDetailID8");
            oSupplierRateProcess.PQDetailID9 = oReader.GetInt32("PQDetailID9");
            oSupplierRateProcess.PQDetailID10 = oReader.GetInt32("PQDetailID10");
            oSupplierRateProcess.PQDetailID11 = oReader.GetInt32("PQDetailID11");
            oSupplierRateProcess.PQDetailID12 = oReader.GetInt32("PQDetailID12");
            oSupplierRateProcess.PQDetailID13 = oReader.GetInt32("PQDetailID13");
            oSupplierRateProcess.PQDetailID14 = oReader.GetInt32("PQDetailID14");
            oSupplierRateProcess.PQDetailID15 = oReader.GetInt32("PQDetailID15");
            oSupplierRateProcess.MaxSupplierCount = oReader.GetInt32("MaxSupplierCount");
            return oSupplierRateProcess;
        }

        public static SupplierRateProcess CreateObject(NullHandler oReader)
        {
            SupplierRateProcess oSupplierRateProcess = new SupplierRateProcess();
            oSupplierRateProcess = MapObject(oReader);
            return oSupplierRateProcess;
        }

        public static List<SupplierRateProcess> CreateObjects(IDataReader oReader)
        {
            List<SupplierRateProcess> oSupplierRateProcess = new List<SupplierRateProcess>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SupplierRateProcess oItem = CreateObject(oHandler);
                oSupplierRateProcess.Add(oItem);
            }
            return oSupplierRateProcess;
        }

        #endregion

        #region Interface implementation
        public SupplierRateProcessService() { }

        public List<SupplierRateProcess> Gets(int nNOAID, Int64 nUserID)
        {
            List<SupplierRateProcess> oSupplierRateProcesss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SupplierRateProcessDA.Gets(nNOAID, tc);
                oSupplierRateProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SupplierRateProcess", e);
                #endregion
            }
            return oSupplierRateProcesss;
        }
        public List<SupplierRateProcess> GetsByLog(int nNOAID, Int64 nUserID)
        {
            List<SupplierRateProcess> oSupplierRateProcesss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SupplierRateProcessDA.GetsByLog(nNOAID, tc);
                oSupplierRateProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SupplierRateProcess", e);
                #endregion
            }
            return oSupplierRateProcesss;
        }

        public List<SupplierRateProcess> GetsBy(int nNOAID, int nNOASignatoryID, Int64 nUserID)
        {
            List<SupplierRateProcess> oSupplierRateProcesss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SupplierRateProcessDA.GetsBy(nNOAID,  nNOASignatoryID,tc);
                oSupplierRateProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SupplierRateProcess", e);
                #endregion
            }
            return oSupplierRateProcesss;
        }
        #endregion
    }   
    
    

}
