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

    public class UserWiseContractorConfigureDetailService : MarshalByRefObject, IUserWiseContractorConfigureDetailService
    {
        #region Private functions and declaration
        private UserWiseContractorConfigureDetail MapObject(NullHandler oReader)
        {
            UserWiseContractorConfigureDetail oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
            oUserWiseContractorConfigureDetail.UserWiseContractorConfigureDetailID = oReader.GetInt32("UserWiseContractorConfigureDetailID");
            oUserWiseContractorConfigureDetail.UserWiseContractorConfigureID = oReader.GetInt32("UserWiseContractorConfigureID");
            oUserWiseContractorConfigureDetail.StyleType = (EnumTSType)oReader.GetInt32("StyleType");
           
            return oUserWiseContractorConfigureDetail;
        }

        private UserWiseContractorConfigureDetail CreateObject(NullHandler oReader)
        {
            UserWiseContractorConfigureDetail oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
            oUserWiseContractorConfigureDetail = MapObject(oReader);
            return oUserWiseContractorConfigureDetail;
        }

        private List<UserWiseContractorConfigureDetail> CreateObjects(IDataReader oReader)
        {
            List<UserWiseContractorConfigureDetail> oUserWiseContractorConfigureDetail = new List<UserWiseContractorConfigureDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                UserWiseContractorConfigureDetail oItem = CreateObject(oHandler);
                oUserWiseContractorConfigureDetail.Add(oItem);
            }
            return oUserWiseContractorConfigureDetail;
        }

        #endregion

        #region Interface implementation
        public UserWiseContractorConfigureDetailService() { }

        public UserWiseContractorConfigureDetail Get(int UserWiseContractorConfigureDetailID, Int64 nUserId)
        {
            UserWiseContractorConfigureDetail oAccountHead = new UserWiseContractorConfigureDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader readerDetail = UserWiseContractorConfigureDetailDA.Get(tc, UserWiseContractorConfigureDetailID);
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
                throw new ServiceException("Failed to Get UserWiseContractorConfigureDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<UserWiseContractorConfigureDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<UserWiseContractorConfigureDetail> oUserWiseContractorConfigureDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseContractorConfigureDetailDA.Gets(LabDipOrderID, tc);
                oUserWiseContractorConfigureDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseContractorConfigureDetail", e);
                #endregion
            }

            return oUserWiseContractorConfigureDetail;
        }

        public List<UserWiseContractorConfigureDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<UserWiseContractorConfigureDetail> oUserWiseContractorConfigureDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseContractorConfigureDetailDA.Gets(tc, sSQL);
                oUserWiseContractorConfigureDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseContractorConfigureDetail", e);
                #endregion
            }

            return oUserWiseContractorConfigureDetail;
        }
        #endregion
    }
    
  
}
