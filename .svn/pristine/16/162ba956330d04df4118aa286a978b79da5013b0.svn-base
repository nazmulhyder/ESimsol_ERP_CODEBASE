using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
namespace ESimSol.Services.Services
{
    public class DUDeliverySummaryRPTService : MarshalByRefObject, IDUDeliverySummaryRPTService
    {
        #region Private functions and declaration

        private DUDeliverySummaryRPT MapObject(NullHandler oReader)
        {
            DUDeliverySummaryRPT oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
            oDUDeliverySummaryRPT.StartDate = oReader.GetDateTime("StartDate");
            oDUDeliverySummaryRPT.EndDate = oReader.GetDateTime("EndDate");
            oDUDeliverySummaryRPT.OrderType = oReader.GetInt32("OrderType");
            oDUDeliverySummaryRPT.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oDUDeliverySummaryRPT.RefID = oReader.GetInt32("RefID");
            oDUDeliverySummaryRPT.RefName = oReader.GetString("RefName");

            oDUDeliverySummaryRPT.QtyIn = oReader.GetDouble("QtyIn");
            oDUDeliverySummaryRPT.QtyOut = oReader.GetDouble("QtyOut");
            oDUDeliverySummaryRPT.AmountIn = oReader.GetDouble("AmountIn");
            oDUDeliverySummaryRPT.AmountOut = oReader.GetDouble("AmountOut");
            oDUDeliverySummaryRPT.Remarks = oReader.GetString("Remarks");

            return oDUDeliverySummaryRPT;
        }

        private DUDeliverySummaryRPT CreateObject(NullHandler oReader)
        {
            DUDeliverySummaryRPT oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
            oDUDeliverySummaryRPT = MapObject(oReader);
            return oDUDeliverySummaryRPT;
        }

        private List<DUDeliverySummaryRPT> CreateObjects(IDataReader oReader)
        {
            List<DUDeliverySummaryRPT> oDUDeliverySummaryRPT = new List<DUDeliverySummaryRPT>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliverySummaryRPT oItem = CreateObject(oHandler);
                oDUDeliverySummaryRPT.Add(oItem);
            }
            return oDUDeliverySummaryRPT;
        }

        #endregion

        #region Interface implementation

        public List<DUDeliverySummaryRPT> GetsData(DUDeliverySummaryRPT oDUDeliverySummaryRPT, Int64 nUserID)
        {
            List<DUDeliverySummaryRPT> oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUDeliverySummaryRPTDA.GetsData(tc, oDUDeliverySummaryRPT);
                oDUDeliverySummaryRPTs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliverySummaryRPT", e);
                #endregion
            }
            return oDUDeliverySummaryRPTs;
        }

        #endregion
    }

}
