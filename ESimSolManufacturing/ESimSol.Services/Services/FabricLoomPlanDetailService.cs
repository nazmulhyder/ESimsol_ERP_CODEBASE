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
    public class FabricLoomPlanDetailService : MarshalByRefObject, IFabricLoomPlanDetailService
    {
        #region Private functions and declaration

        private FabricLoomPlanDetail MapObject(NullHandler oReader)
        {
            FabricLoomPlanDetail oFabricLoomPlanDetail = new FabricLoomPlanDetail();
            oFabricLoomPlanDetail.FLPDID = oReader.GetInt32("FLPDID");
            oFabricLoomPlanDetail.FLPID = oReader.GetInt32("FLPID");
            oFabricLoomPlanDetail.FBPBeamID = oReader.GetInt32("FBPBeamID");
            oFabricLoomPlanDetail.Construction = oReader.GetString("Construction");
            oFabricLoomPlanDetail.FEONo = oReader.GetString("FEONo");
            oFabricLoomPlanDetail.BatchNo = oReader.GetString("BatchNo");
            oFabricLoomPlanDetail.BuyerName = oReader.GetString("BuyerName");
            oFabricLoomPlanDetail.BeamID = oReader.GetInt32("BeamID");
            oFabricLoomPlanDetail.BeamNo = oReader.GetString("BeamNo");
            oFabricLoomPlanDetail.Qty = oReader.GetDouble("Qty");
            oFabricLoomPlanDetail.FEOID = oReader.GetInt32("FEOID");
            oFabricLoomPlanDetail.FEOSID = oReader.GetInt32("FEOSID");
            oFabricLoomPlanDetail.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");

            return oFabricLoomPlanDetail;
        }

        private FabricLoomPlanDetail CreateObject(NullHandler oReader)
        {
            FabricLoomPlanDetail oFabricLoomPlanDetail = new FabricLoomPlanDetail();
            oFabricLoomPlanDetail = MapObject(oReader);
            return oFabricLoomPlanDetail;
        }

        private List<FabricLoomPlanDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricLoomPlanDetail> oFabricLoomPlanDetail = new List<FabricLoomPlanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricLoomPlanDetail oItem = CreateObject(oHandler);
                oFabricLoomPlanDetail.Add(oItem);
            }
            return oFabricLoomPlanDetail;
        }

        #endregion

        #region Interface implementation


        public FabricLoomPlanDetail Get(int id, Int64 nUserId)
        {
            FabricLoomPlanDetail oFabricLoomPlanDetail = new FabricLoomPlanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricLoomPlanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricLoomPlanDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricLoomPlanDetail", e);
                #endregion
            }
            return oFabricLoomPlanDetail;
        }

        public List<FabricLoomPlanDetail> Gets(int nFabricLoomPlanID, Int64 nUserID)
        {
            List<FabricLoomPlanDetail> oFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricLoomPlanDetailDA.Gets(tc, nFabricLoomPlanID);
                oFabricLoomPlanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricLoomPlanDetail oFabricLoomPlanDetail = new FabricLoomPlanDetail();
                oFabricLoomPlanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricLoomPlanDetails;
        }

        public List<FabricLoomPlanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricLoomPlanDetail> oFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricLoomPlanDetailDA.Gets(tc, sSQL);
                oFabricLoomPlanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricLoomPlanDetail", e);
                #endregion
            }
            return oFabricLoomPlanDetails;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricLoomPlanDetail oFabricLoomPlanDetail = new FabricLoomPlanDetail();
                oFabricLoomPlanDetail.FLPDID = id;
                FabricLoomPlanDetailDA.DeleteByID(tc, oFabricLoomPlanDetail);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        #endregion
    }

}
