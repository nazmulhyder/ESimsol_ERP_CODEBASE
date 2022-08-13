using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class GUProductionTracingUnitDetailService : MarshalByRefObject, IGUProductionTracingUnitDetailService
    {
        #region Private functions and declaration
        private GUProductionTracingUnitDetail MapObject(NullHandler oReader)
        {
            GUProductionTracingUnitDetail oGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();
            oGUProductionTracingUnitDetail.GUProductionTracingUnitDetailID = oReader.GetInt32("GUProductionTracingUnitDetailID");
            oGUProductionTracingUnitDetail.GUProductionTracingUnitID = oReader.GetInt32("GUProductionTracingUnitID");
            oGUProductionTracingUnitDetail.GUProductionProcedureID = oReader.GetInt32("GUProductionProcedureID");
            oGUProductionTracingUnitDetail.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oGUProductionTracingUnitDetail.ExecutionQty = oReader.GetDouble("ExecutionQty");
            oGUProductionTracingUnitDetail.YetToExecutionQty = oReader.GetDouble("YetToExecutionQty");
            oGUProductionTracingUnitDetail.ExecutionStartDate = oReader.GetDateTime("ExecutionStartDate");
            oGUProductionTracingUnitDetail.StepName = oReader.GetString("StepName");
            oGUProductionTracingUnitDetail.Sequence = oReader.GetInt32("Sequence");
            return oGUProductionTracingUnitDetail;
        }

        private GUProductionTracingUnitDetail CreateObject(NullHandler oReader)
        {
            GUProductionTracingUnitDetail oGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();
            oGUProductionTracingUnitDetail = MapObject(oReader);
            return oGUProductionTracingUnitDetail;
        }

        private List<GUProductionTracingUnitDetail> CreateObjects(IDataReader oReader)
        {
            List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetail = new List<GUProductionTracingUnitDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUProductionTracingUnitDetail oItem = CreateObject(oHandler);
                oGUProductionTracingUnitDetail.Add(oItem);
            }
            return oGUProductionTracingUnitDetail;
        }

        #endregion

        #region Interface implementation
        public GUProductionTracingUnitDetailService() { }

        public List<GUProductionTracingUnitDetail> Gets(Int64 nUserID)
        {
            List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDetailDA.Gets(tc);
                oGUProductionTracingUnitDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Tracing Unit Details!!", e);
                #endregion
            }

            return oGUProductionTracingUnitDetails;
        }

        public List<GUProductionTracingUnitDetail> Gets(int PUID, Int64 nUserID)
        {
            List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDetailDA.Gets(PUID, tc);
                oGUProductionTracingUnitDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Tracing Unit Details!!", e);
                #endregion
            }

            return oGUProductionTracingUnitDetails;
        }
        public List<GUProductionTracingUnitDetail> Gets(string sql, Int64 nUserID)
        {
            List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDetailDA.Gets(sql, tc);
                oGUProductionTracingUnitDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Tracing Unit Details!!", e);
                #endregion
            }

            return oGUProductionTracingUnitDetails;
        }

        public List<GUProductionTracingUnitDetail> GetsByOrderRecap(int nOrderRecapID, Int64 nUserID)
        {
            List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDetailDA.GetsByOrderRecap(tc, nOrderRecapID);
                oGUProductionTracingUnitDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Tracing Unit Details!!", e);
                #endregion
            }

            return oGUProductionTracingUnitDetails;
        }

        public List<GUProductionTracingUnitDetail> Gets_byPOIDs(string sPOIDs, Int64 nUserID)
        {
            List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDetailDA.Gets_byPOIDs(tc, sPOIDs);
                oGUProductionTracingUnitDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Tracing Unit Details!!", e);
                #endregion
            }

            return oGUProductionTracingUnitDetails;
        }
        
        #endregion
    }
}
