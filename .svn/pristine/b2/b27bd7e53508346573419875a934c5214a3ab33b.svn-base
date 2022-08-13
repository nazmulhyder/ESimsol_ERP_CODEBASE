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
    public class WUSubContractYarnChallanDetailService : MarshalByRefObject, IWUSubContractYarnChallanDetailService
    {
        #region Private functions and declaration
        private WUSubContractYarnChallanDetail MapObject(NullHandler oReader)
        {
            WUSubContractYarnChallanDetail oWUSubContractYarnChallanDetail = new WUSubContractYarnChallanDetail();
            oWUSubContractYarnChallanDetail.WUSubContractYarnChallanDetailID = oReader.GetInt32("WUSubContractYarnChallanDetailID");           
            oWUSubContractYarnChallanDetail.WUSubContractYarnChallanID = oReader.GetInt32("WUSubContractYarnChallanID");
            oWUSubContractYarnChallanDetail.WUSubContractID = oReader.GetInt32("WUSubContractID");
            oWUSubContractYarnChallanDetail.WUSubContractYarnConsumptionID = oReader.GetInt32("WUSubContractYarnConsumptionID");
            oWUSubContractYarnChallanDetail.IssueStoreID = oReader.GetInt32("IssueStoreID");
            oWUSubContractYarnChallanDetail.YarnID = oReader.GetInt32("YarnID");
            oWUSubContractYarnChallanDetail.LotID = oReader.GetInt32("LotID");
            oWUSubContractYarnChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oWUSubContractYarnChallanDetail.Qty = oReader.GetDouble("Qty");           
            oWUSubContractYarnChallanDetail.Remarks = oReader.GetString("Remarks");
            oWUSubContractYarnChallanDetail.BagQty = oReader.GetInt32("BagQty");
            oWUSubContractYarnChallanDetail.DBUserID = oReader.GetInt32("DBUserID");
            oWUSubContractYarnChallanDetail.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oWUSubContractYarnChallanDetail.StoreName = oReader.GetString("StoreName");
            oWUSubContractYarnChallanDetail.YarnCode = oReader.GetString("YarnCode");
            oWUSubContractYarnChallanDetail.YarnName = oReader.GetString("YarnName");
            oWUSubContractYarnChallanDetail.LotNo = oReader.GetString("LotNo");
            oWUSubContractYarnChallanDetail.LotBalance = oReader.GetString("LotBalance");
            oWUSubContractYarnChallanDetail.MUSymbol = oReader.GetString("MUSymbol");
            oWUSubContractYarnChallanDetail.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            return oWUSubContractYarnChallanDetail;
        }

        private WUSubContractYarnChallanDetail CreateObject(NullHandler oReader)
        {
            WUSubContractYarnChallanDetail oWUSubContractYarnChallanDetail = new WUSubContractYarnChallanDetail();
            oWUSubContractYarnChallanDetail = MapObject(oReader);
            return oWUSubContractYarnChallanDetail;
        }

        private List<WUSubContractYarnChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<WUSubContractYarnChallanDetail> oWUSubContractYarnChallanDetail = new List<WUSubContractYarnChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUSubContractYarnChallanDetail oItem = CreateObject(oHandler);
                oWUSubContractYarnChallanDetail.Add(oItem);
            }
            return oWUSubContractYarnChallanDetail;
        }

        #endregion

        #region Interface implementation
        public WUSubContractYarnChallanDetailService() { }

        public List<WUSubContractYarnChallanDetail> Gets(int id, Int64 nUserId)
        {
            List<WUSubContractYarnChallanDetail> oWUSubContractYarnChallanDetails = new List<WUSubContractYarnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractYarnChallanDetailDA.Gets(tc, id);
                oWUSubContractYarnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContractYarnChallanDetail", e);
                #endregion
            }
            return oWUSubContractYarnChallanDetails;
        }

        #endregion
    }
}