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

    public class TAPDetailService : MarshalByRefObject, ITAPDetailService
    {
        #region Private functions and declaration
        public static TAPDetail MapObject(NullHandler oReader)
        {
            TAPDetail oTAPDetail = new TAPDetail();
            oTAPDetail.TAPDetailID = oReader.GetInt32("TAPDetailID");
            oTAPDetail.TAPID = oReader.GetInt32("TAPID");
            oTAPDetail.OrderStepID = oReader.GetInt32("OrderStepID");
            oTAPDetail.OrderStepName = oReader.GetString("OrderStepName");
            oTAPDetail.Sequence = oReader.GetInt32("Sequence");
            oTAPDetail.ApprovalPlanDate = oReader.GetDateTime("ApprovalPlanDate");
            oTAPDetail.Remarks = oReader.GetString("Remarks");
            oTAPDetail.OrderStepParentID = oReader.GetInt32("OrderStepParentID");
            oTAPDetail.OrderStepSequence = oReader.GetInt32("OrderStepSequence");
            oTAPDetail.ExecutionIsDone = oReader.GetBoolean("ExecutionIsDone");
            oTAPDetail.ExecutionDoneDate = oReader.GetDateTime("ExecutionDoneDate");
            oTAPDetail.GroupName = oReader.GetString("GroupName");
            oTAPDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oTAPDetail.GroupID = oReader.GetInt32("GroupID");
            oTAPDetail.GroupSequence = oReader.GetInt32("GroupSequence");
            oTAPDetail.UpdatedData = oReader.GetString("UpdatedData");
            oTAPDetail.RequiredDataType = (EnumRequiredDataType) oReader.GetInt32("RequiredDataType");
            oTAPDetail.StepType = (EnumStepType)oReader.GetInt32("StepType");
            oTAPDetail.TnAStep = (EnumTnAStep)oReader.GetInt32("TnAStep");
            oTAPDetail.TnAStepInt = oReader.GetInt32("TnAStep");
            
            oTAPDetail.TSTypeInt = oReader.GetInt32("TSType");
            oTAPDetail.SubmissionDate = oReader.GetDateTime("SubmissionDate");
            oTAPDetail.ReqSubmissionDays = oReader.GetInt32("ReqSubmissionDays");
            oTAPDetail.ReqBuyerApprovalDays = oReader.GetInt32("ReqBuyerApprovalDays");
            oTAPDetail.SubStepName = oReader.GetString("SubStepName");
            return oTAPDetail;
        }

        public static TAPDetail CreateObject(NullHandler oReader)
        {
            TAPDetail oTAPDetail = new TAPDetail();
            oTAPDetail = MapObject(oReader);
            return oTAPDetail;
        }

        public static List<TAPDetail> CreateObjects(IDataReader oReader)
        {
            List<TAPDetail> oTAPDetail = new List<TAPDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TAPDetail oItem = CreateObject(oHandler);
                oTAPDetail.Add(oItem);
            }
            return oTAPDetail;
        }

        #endregion

        #region Interface implementation
        public TAPDetailService() { }

        public TAPDetail Save(TAPDetail oTAPDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<TAPDetail> _oTAPDetails = new List<TAPDetail>();
            oTAPDetail.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TAPDetailDA.InsertUpdate(tc, oTAPDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAPDetail = new TAPDetail();
                    oTAPDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTAPDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oTAPDetail;
        }

        public TAPDetail Get(int TAPDetailID, Int64 nUserId)
        {
            TAPDetail oAccountHead = new TAPDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader readerDetail = TAPDetailDA.Get(tc, TAPDetailID);
                NullHandler oReaderDetail = new NullHandler(readerDetail);
                if (readerDetail.Read())
                {
                    oAccountHead = CreateObject(oReaderDetail);
                }
                readerDetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<TAPDetail> Gets(int TAPID, Int64 nUserID)
        {
            List<TAPDetail> oTAPDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPDetailDA.Gets(TAPID, tc);
                oTAPDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPDetail", e);
                #endregion
            }

            return oTAPDetail;
        }


        public List<TAPDetail> FactoryTAPGets(int TAPID, Int64 nUserID)
        {
            List<TAPDetail> oTAPDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPDetailDA.FactoryTAPGets(TAPID, tc);
                oTAPDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPDetail", e);
                #endregion
            }

            return oTAPDetail;
        }
        public List<TAPDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<TAPDetail> oTAPDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPDetailDA.Gets(tc, sSQL);
                oTAPDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPDetail", e);
                #endregion
            }

            return oTAPDetail;
        }

        public List<TAPDetail> Gets(Int64 nUserID)
        {
            List<TAPDetail> oTAPDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPDetailDA.Gets(tc);
                oTAPDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPDetail", e);
                #endregion
            }

            return oTAPDetail;
        }

        #endregion
    }
    
   
}
