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
    public class LotTrakingService : MarshalByRefObject, ILotTrakingService
    {
       
        #region Private functions and declaration
        private LotTraking MapObject(NullHandler oReader)
        {
            
            LotTraking oLotTraking = new LotTraking();
            oLotTraking.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            //oLotTraking.StartDate = oReader.GetDateTime("StartDate");
            oLotTraking.DateTime = oReader.GetDateTime("DateTime");
            oLotTraking.MUnitID = oReader.GetInt32("MUnitID");
            oLotTraking.TriggerNo = oReader.GetString("TriggerNo");
            oLotTraking.MUnit = oReader.GetString("MUnit");
            oLotTraking.OpeningQty = oReader.GetDouble("OpeningQty");
            oLotTraking.UnitPrice = oReader.GetDouble("UnitPrice");
            oLotTraking.Qty = oReader.GetDouble("Qty");
            oLotTraking.TriggerParentID = oReader.GetInt32("TriggerParentID");
            oLotTraking.TriggerParentType = oReader.GetInt32("TriggerParentType");
            oLotTraking.ClosingQty = oReader.GetDouble("ClosingQty");
            oLotTraking.WUName = oReader.GetString("WorkingUnitName");
            oLotTraking.LotNo = oReader.GetString("LotNo");
            oLotTraking.InOutType = oReader.GetInt16("InOutType");
            oLotTraking.ContractorName = oReader.GetString("ContractorName");
            oLotTraking.OrderNo = oReader.GetString("OrderNo");
            return oLotTraking;
        }
        private LotTraking CreateObject(NullHandler oReader)
        {
            LotTraking oLotTraking = new LotTraking();
            oLotTraking=MapObject(oReader);
            return oLotTraking;
        }
        private List<LotTraking> CreateObjects(IDataReader oReader)
        {
            List<LotTraking> oLotTrakings = new List<LotTraking>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LotTraking oItem = CreateObject(oHandler);
                oLotTrakings.Add(oItem);
            }
            return oLotTrakings;
        }
        #endregion


        #region Interface implementation
        public LotTrakingService() { }

      
        public List<LotTraking> Gets_Lot(int nBUID,string sLotIDs, Int64 nUserId)
        {
            List<LotTraking> oLotTrakings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LotTrakingDA.Gets_Lot(tc,nBUID, sLotIDs);
                oLotTrakings = CreateObjects(reader);
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

            return oLotTrakings;
        }
  
        #endregion
    }
}