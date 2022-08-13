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
   public class LotBuyerService :MarshalByRefObject,ILotBuyerService
    {
       #region Private functions and declaration
        private static LotBuyer MapObject(NullHandler oReader)
        {
            LotBuyer oLotBuyer = new LotBuyer();
            oLotBuyer.LotID = oReader.GetInt32("LotID");
            oLotBuyer.LotNo = oReader.GetString("LotNo");
            oLotBuyer.Balance = oReader.GetDouble("PINo");
            oLotBuyer.LocationName = oReader.GetString("LocationName");
            oLotBuyer.OperationUnitName = oReader.GetString("OperationUnitName");
            oLotBuyer.ProductCode = oReader.GetString("ProductCode");
            oLotBuyer.ProductName = oReader.GetString("ProductName");
            oLotBuyer.MUName = oReader.GetString("MUName");
            oLotBuyer.ContractorID = oReader.GetInt32("ContractorID");
            oLotBuyer.ContractorName = oReader.GetString("ContractorName");
            oLotBuyer.Balance = oReader.GetDouble("Balance");
            
            return oLotBuyer;
        }


        public static LotBuyer CreateObject(NullHandler oReader)
        {
            LotBuyer oLotBuyer = MapObject(oReader);
            return oLotBuyer;
        }

        private List<LotBuyer> CreateObjects(IDataReader oReader)
        {
            List<LotBuyer> oLotBuyers = new List<LotBuyer>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LotBuyer oItem = CreateObject(oHandler);
               oLotBuyers.Add(oItem);
            }
            return oLotBuyers;
        }

        #endregion

        #region Interface implementation
        public LotBuyerService() { }
        public List<LotBuyer> Gets(string sSQL,int reportType, Int64 nUserID)
        {
            List<LotBuyer> oLotBuyerS = new List<LotBuyer>();
            LotBuyer oLotBuyer = new LotBuyer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LotBuyerDA.Gets( sSQL, reportType,tc);
                oLotBuyerS = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLotBuyer.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oLotBuyerS.Add(oLotBuyer);
                #endregion
            }

            return oLotBuyerS;
        }

        #endregion
    }
}
