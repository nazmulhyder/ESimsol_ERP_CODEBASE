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

    public class TAPTemplateDetailService : MarshalByRefObject, ITAPTemplateDetailService
    {
        #region Private functions and declaration
        public static TAPTemplateDetail MapObject(NullHandler oReader)
        {
            TAPTemplateDetail oTAPTemplateDetail = new TAPTemplateDetail();
            oTAPTemplateDetail.TAPTemplateDetailID = oReader.GetInt32("TAPTemplateDetailID");
            oTAPTemplateDetail.TAPTemplateID = oReader.GetInt32("TAPTemplateID");
            oTAPTemplateDetail.OrderStepID = oReader.GetInt32("OrderStepID");
            oTAPTemplateDetail.OrderStepName = oReader.GetString("OrderStepName");
            oTAPTemplateDetail.Sequence = oReader.GetInt32("Sequence");
            oTAPTemplateDetail.BeforeShipment = oReader.GetInt32("BeforeShipment");
            oTAPTemplateDetail.Remarks = oReader.GetString("Remarks");
            oTAPTemplateDetail.OrderStepParentID = oReader.GetInt32("OrderStepParentID");
            oTAPTemplateDetail.OrderStepSequence = oReader.GetInt32("OrderStepSequence");
            oTAPTemplateDetail.CalculationType = (EnumCalculationType)oReader.GetInt32("CalculationType");
            oTAPTemplateDetail.TemplateName = oReader.GetString("TemplateName");
            return oTAPTemplateDetail;
        }

        public static TAPTemplateDetail CreateObject(NullHandler oReader)
        {
            TAPTemplateDetail oTAPTemplateDetail = new TAPTemplateDetail();
            oTAPTemplateDetail = MapObject(oReader);
            return oTAPTemplateDetail;
        }

        public static List<TAPTemplateDetail> CreateObjects(IDataReader oReader)
        {
            List<TAPTemplateDetail> oTAPTemplateDetail = new List<TAPTemplateDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TAPTemplateDetail oItem = CreateObject(oHandler);
                oTAPTemplateDetail.Add(oItem);
            }
            return oTAPTemplateDetail;
        }

        #endregion

        #region Interface implementation
        public TAPTemplateDetailService() { }

 
        public TAPTemplateDetail Get(int TAPTemplateDetailID, Int64 nUserId)
        {
            TAPTemplateDetail oAccountHead = new TAPTemplateDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader readerDetail = TAPTemplateDetailDA.Get(tc, TAPTemplateDetailID);
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
                throw new ServiceException("Failed to Get TAPTemplateDetail", e);
                #endregion
            }

            return oAccountHead;
        }




        public List<TAPTemplateDetail> Gets(int TAPTemplateID, Int64 nUserID)
        {
            List<TAPTemplateDetail> oTAPTemplateDetail = new List<TAPTemplateDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPTemplateDetailDA.Gets(TAPTemplateID, tc);
                oTAPTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPTemplateDetail", e);
                #endregion
            }

            return oTAPTemplateDetail;
        }

        public List<TAPTemplateDetail> GetsByTampleteType(int nTempleteType, Int64 nUserID)
        {
            List<TAPTemplateDetail> oTAPTemplateDetail = new List<TAPTemplateDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPTemplateDetailDA.GetsByTampleteType(nTempleteType, tc);
                oTAPTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPTemplateDetail", e);
                #endregion
            }

            return oTAPTemplateDetail;
        }


        public List<TAPTemplateDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<TAPTemplateDetail> oTAPTemplateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPTemplateDetailDA.Gets(tc, sSQL);
                oTAPTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPTemplateDetail", e);
                #endregion
            }

            return oTAPTemplateDetail;
        }

        public List<TAPTemplateDetail> Gets(Int64 nUserID)
        {
            List<TAPTemplateDetail> oTAPTemplateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPTemplateDetailDA.Gets(tc);
                oTAPTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPTemplateDetail", e);
                #endregion
            }

            return oTAPTemplateDetail;
        }

        #endregion
    }
    

}
