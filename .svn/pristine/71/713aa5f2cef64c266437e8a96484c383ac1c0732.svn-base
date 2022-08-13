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
    public class VOrderRegisterService : MarshalByRefObject, IVOrderRegisterService
    {
        #region Private functions and declaration
        private VOrderRegister MapObject(NullHandler oReader)
        {
            VOrderRegister oVOrderRegister = new VOrderRegister();
            oVOrderRegister.VOReferenceID = oReader.GetInt32("VOReferenceID");
            oVOrderRegister.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oVOrderRegister.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVOrderRegister.VoucherID = oReader.GetInt32("VoucherID");
            oVOrderRegister.OrderID = oReader.GetInt32("OrderID");
            oVOrderRegister.TransactionDate = oReader.GetDateTime("TransactionDate");
            oVOrderRegister.Remarks = oReader.GetString("Remarks");
            oVOrderRegister.IsDebit = oReader.GetBoolean("IsDebit");
            oVOrderRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oVOrderRegister.ConversionRate = oReader.GetDouble("ConversionRate");
            oVOrderRegister.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
            oVOrderRegister.Amount = oReader.GetDouble("Amount");
            oVOrderRegister.CCTID = oReader.GetInt32("CCTID");
            oVOrderRegister.RefNo = oReader.GetString("RefNo");
            oVOrderRegister.OrderNo = oReader.GetString("OrderNo");
            oVOrderRegister.OrderDate = oReader.GetDateTime("OrderDate");
            oVOrderRegister.SubledgerID = oReader.GetInt32("SubledgerID");
            oVOrderRegister.SubledgerName = oReader.GetString("SubledgerName");
            oVOrderRegister.VoucherNo = oReader.GetString("VoucherNo");
            oVOrderRegister.CurrencyName = oReader.GetString("CurrencyName");
            oVOrderRegister.Symbol = oReader.GetString("Symbol");
            oVOrderRegister.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oVOrderRegister.AccountHeadName = oReader.GetString("AccountHeadName");
            oVOrderRegister.ComponentID = oReader.GetInt32("ComponentID");
            oVOrderRegister.LCID = oReader.GetInt32("LCID");
            oVOrderRegister.LCNo = oReader.GetString("LCNo");
            oVOrderRegister.VOrderRefType = (EnumVOrderRefType)oReader.GetInt32("VOrderRefType");
            return oVOrderRegister;
        }
        private VOrderRegister CreateObject(NullHandler oReader)
        {
            VOrderRegister oVOrderRegister = new VOrderRegister();
            oVOrderRegister = MapObject(oReader);
            return oVOrderRegister;
        }
        private List<VOrderRegister> CreateObjects(IDataReader oReader)
        {
            List<VOrderRegister> oVOrderRegister = new List<VOrderRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VOrderRegister oItem = CreateObject(oHandler);
                oVOrderRegister.Add(oItem);
            }
            return oVOrderRegister;
        }

        #endregion

        #region Interface implementation
        public VOrderRegisterService() { }
        public List<VOrderRegister> Gets(VOrderRegister oVOrderRegister, string sSQL, int nUserID)
        {
            List<VOrderRegister> oVOrderRegisters = new List<VOrderRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VOrderRegisterDA.Gets(tc, oVOrderRegister, sSQL);
                oVOrderRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOrderRegister", e);
                #endregion
            }

            return oVOrderRegisters;
        }     
        #endregion
    }   

    
}
