using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class EmployeeCodeDetailService : MarshalByRefObject, IEmployeeCodeDetailService
    {
        #region Private functions and declaration
        private EmployeeCodeDetail MapObject(NullHandler oReader)
        {
            EmployeeCodeDetail oEmployeeCodeDetail = new EmployeeCodeDetail();
            oEmployeeCodeDetail.ECDID = oReader.GetInt32("ECDID");
            oEmployeeCodeDetail.EmployeeCodeID = oReader.GetInt32("EmployeeCodeID");
            oEmployeeCodeDetail.ECDType = (EnumVoucherCodeType)oReader.GetInt32("ECDType");
            oEmployeeCodeDetail.ECDTypeInInt = oReader.GetInt32("ECDType");
            oEmployeeCodeDetail.Value = oReader.GetString("Value");
            oEmployeeCodeDetail.Length = oReader.GetInt32("Length");
            oEmployeeCodeDetail.Restart = (EnumRestartPeriod)oReader.GetInt32("Restart");
            oEmployeeCodeDetail.RestartInInt = oReader.GetInt32("Restart");
            oEmployeeCodeDetail.Sequence = oReader.GetInt32("Sequence");

            return oEmployeeCodeDetail;

        }

        private EmployeeCodeDetail CreateObject(NullHandler oReader)
        {
            EmployeeCodeDetail oEmployeeCodeDetail = MapObject(oReader);
            return oEmployeeCodeDetail;
        }

        private List<EmployeeCodeDetail> CreateObjects(IDataReader oReader)
        {
            List<EmployeeCodeDetail> oEmployeeCodeDetail = new List<EmployeeCodeDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeCodeDetail oItem = CreateObject(oHandler);
                oEmployeeCodeDetail.Add(oItem);
            }
            return oEmployeeCodeDetail;
        }

        #endregion

        #region Interface implementation
        public EmployeeCodeDetailService() { }

        public EmployeeCodeDetail IUD(EmployeeCodeDetail oEmployeeCodeDetail, int nDBOperation, Int64 nUserID)
        {

            List<EmployeeCodeDetail> oEmployeeCodeDetails = new List<EmployeeCodeDetail>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeCodeDetailDA.IUD(tc, oEmployeeCodeDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeCodeDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeCodeDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeCodeDetail.ECDID = 0;
                #endregion
            }
            return oEmployeeCodeDetail;
        }

        public EmployeeCodeDetail Get(int nEmployeeCodeDetailID, Int64 nUserId)
        {
            EmployeeCodeDetail oEmployeeCodeDetail = new EmployeeCodeDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCodeDetailDA.Get(nEmployeeCodeDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCodeDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeCodeDetail", e);
                oEmployeeCodeDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCodeDetail;
        }
        public List<EmployeeCodeDetail> Gets(Int64 nUserID)
        {
            List<EmployeeCodeDetail> oEmployeeCodeDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCodeDetailDA.Gets(tc);
                oEmployeeCodeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeCodeDetail", e);
                #endregion
            }
            return oEmployeeCodeDetail;
        }

        public List<EmployeeCodeDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeCodeDetail> oEmployeeCodeDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCodeDetailDA.Gets(sSQL, tc);
                oEmployeeCodeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeCodeDetail", e);
                #endregion
            }
            return oEmployeeCodeDetail;
        }


        #endregion

    }
}
