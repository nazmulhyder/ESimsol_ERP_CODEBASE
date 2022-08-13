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
    public class VoucherCodeService : MarshalByRefObject, IVoucherCodeService
    {
        #region Private functions and declaration
        private VoucherCode MapObject(NullHandler oReader)
        {
            VoucherCode oVoucherCode = new VoucherCode();
            oVoucherCode.VoucherCodeID = oReader.GetInt32("VoucherCodeID");
            oVoucherCode.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oVoucherCode.VoucherCodeType= (EnumVoucherCodeType)oReader.GetInt32("VoucherCodeType");
            oVoucherCode.VoucherCodeTypeInInt = oReader.GetInt32("VoucherCodeType");
            oVoucherCode.Value= oReader.GetString("Value");
            oVoucherCode.Length= oReader.GetInt32("Length");
            oVoucherCode.Restart = (EnumRestartPeriod)oReader.GetInt32("Restart");
            oVoucherCode.RestartInInt = oReader.GetInt32("Restart");
            oVoucherCode.Sequence = oReader.GetInt32("Sequence");
            return oVoucherCode;
        }

        private VoucherCode CreateObject(NullHandler oReader)
        {
            VoucherCode oVoucherCode = new VoucherCode();
            oVoucherCode = MapObject(oReader);
            return oVoucherCode;
        }

        private List<VoucherCode> CreateObjects(IDataReader oReader)
        {
            List<VoucherCode> oVoucherCode = new List<VoucherCode>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherCode oItem = CreateObject(oHandler);
                oVoucherCode.Add(oItem);
            }
            return oVoucherCode;
        }

        #endregion

        #region Interface implementation
        public VoucherCodeService() { }

        public VoucherCode Save(VoucherCode oVoucherCode, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherCode.VoucherCodeID <= 0)
                {
                    reader = VoucherCodeDA.InsertUpdate(tc, oVoucherCode, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VoucherCodeDA.InsertUpdate(tc, oVoucherCode, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherCode = new VoucherCode();
                    oVoucherCode = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VoucherCode. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherCode;
        }
        public bool Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherCode oVoucherCode = new VoucherCode();
                oVoucherCode.VoucherCodeID = id;
                VoucherCodeDA.Delete(tc, oVoucherCode, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete VoucherCode. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public VoucherCode Get(int id, int nUserId)
        {
            VoucherCode oAccountHead = new VoucherCode();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherCodeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VoucherCode", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VoucherCode> Gets(int nUserID)
        {
            List<VoucherCode> oVoucherCode = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherCodeDA.Gets(tc);
                oVoucherCode = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherCode", e);
                #endregion
            }

            return oVoucherCode;
        }

        public List<VoucherCode> Gets(string sSQL, int nUserId)
        {
            List<VoucherCode> oVoucherCode = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherCodeDA.Gets(tc, sSQL);
                oVoucherCode = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Loan", e);
                #endregion
            }

            return oVoucherCode;
        }
        #endregion
    }
}
