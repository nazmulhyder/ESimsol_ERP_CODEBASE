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
    public class CurrencyConversionService : MarshalByRefObject, ICurrencyConversionService
    {
        #region Private functions and declaration
        private CurrencyConversion MapObject(NullHandler oReader)
        {
            CurrencyConversion oCurrencyConversion = new CurrencyConversion();
            oCurrencyConversion.CurrencyConversionID = oReader.GetInt32("CurrencyConversionID");
            oCurrencyConversion.CurrencyID = oReader.GetInt32("CurrencyID");
            oCurrencyConversion.ToCurrencyID = oReader.GetInt32("ToCurrencyID");
            oCurrencyConversion.BankID = oReader.GetInt32("BankID");
            oCurrencyConversion.RateSale = oReader.GetDouble("RateSale");
            oCurrencyConversion.RatePurchase = oReader.GetDouble("RatePurchase");
            oCurrencyConversion.Date = oReader.GetDateTime("Date");
            oCurrencyConversion.BankCode = oReader.GetString("BankCode");
            oCurrencyConversion.BankName = oReader.GetString("BankName");
            oCurrencyConversion.Currency = oReader.GetString("Currency");
            oCurrencyConversion.ToCurrency = oReader.GetString("ToCurrency");
            oCurrencyConversion.CurrencyName = oReader.GetString("CurrencyName");
            oCurrencyConversion.ToCurrencyName = oReader.GetString("ToCurrencyName");
            oCurrencyConversion.IsOpen = oReader.GetBoolean("IsOpen");
            oCurrencyConversion.Note = oReader.GetString("Note");
            oCurrencyConversion.Date = oReader.GetDateTime("Date");
            oCurrencyConversion.StartDate = oReader.GetDateTime("StartDate");
            oCurrencyConversion.EndDate = oReader.GetDateTime("EndDate");
            return oCurrencyConversion;
        }

        private CurrencyConversion CreateObject(NullHandler oReader)
        {
            CurrencyConversion oCurrencyConversion = new CurrencyConversion();
            oCurrencyConversion = MapObject(oReader);
            return oCurrencyConversion;
        }

        private List<CurrencyConversion> CreateObjects(IDataReader oReader)
        {
            List<CurrencyConversion> oCurrencyConversion = new List<CurrencyConversion>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CurrencyConversion oItem = CreateObject(oHandler);
                oCurrencyConversion.Add(oItem);
            }
            return oCurrencyConversion;
        }

        #endregion

        #region Interface implementation
        public CurrencyConversionService() { }

        public CurrencyConversion Save(CurrencyConversion oCurrencyConversion, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCurrencyConversion.CurrencyConversionID <= 0)
                {
                    reader = CurrencyConversionDA.InsertUpdate(tc, oCurrencyConversion, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = CurrencyConversionDA.InsertUpdate(tc, oCurrencyConversion, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCurrencyConversion = new CurrencyConversion();
                    oCurrencyConversion = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save CurrencyConversion. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oCurrencyConversion;
        }
        public string Delete(int nID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CurrencyConversion oCurrencyConversion = new CurrencyConversion();
                oCurrencyConversion.CurrencyConversionID = nID;
                CurrencyConversionDA.Delete(tc, oCurrencyConversion, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete CurrencyConversion. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public CurrencyConversion Get(int id, Int64 nUserId)
        {
            CurrencyConversion oCurrencyConversion = new CurrencyConversion();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CurrencyConversionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCurrencyConversion = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CurrencyConversion", e);
                #endregion
            }

            return oCurrencyConversion;
        }

        public List<CurrencyConversion> Gets(Int64 nUserId)
        {
            List<CurrencyConversion> oCurrencyConversion = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CurrencyConversionDA.Gets(tc);
                oCurrencyConversion = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CurrencyConversion", e);
                #endregion
            }
            return oCurrencyConversion;
        }

        public List<CurrencyConversion> GetsByFromCurrency(int nCurrencyID, Int64 nUserId)
        {
            List<CurrencyConversion> oCurrencyConversion = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CurrencyConversionDA.GetsByFromCurrency(tc, nCurrencyID);
                oCurrencyConversion = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CurrencyConversion", e);
                #endregion
            }

            return oCurrencyConversion;
        }

        public CurrencyConversion GetsLastConversionRate(int nFromCurrencyID, int nToCurrencyID, Int64 nUserId)
        {
            CurrencyConversion oCurrencyConversion = new CurrencyConversion();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CurrencyConversionDA.GetsLastConversionRate(tc, nFromCurrencyID, nToCurrencyID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCurrencyConversion = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CurrencyConversion", e);
                #endregion
            }

            return oCurrencyConversion;
        }       
        #endregion
    }
}
