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
    public class CurrencyService : MarshalByRefObject, ICurrencyService
    {
        #region Private functions and declaration
        private Currency MapObject(NullHandler oReader)
        {
            Currency oCurrency = new Currency();
            oCurrency.CurrencyID = oReader.GetInt32("CurrencyID");
            oCurrency.CurrencyName= oReader.GetString("CurrencyName");
            oCurrency.IssueFigure = oReader.GetString("IssueFigure");
            oCurrency.Symbol= oReader.GetString("Symbol");
            oCurrency.SmallerUnit= oReader.GetString("SmallerUnit");
            oCurrency.SmUnitValue= oReader.GetDouble("SmUnitValue");
            oCurrency.Note= oReader.GetString("Note");            
            return oCurrency;
        }

        private Currency CreateObject(NullHandler oReader)
        {
            Currency oCurrency = new Currency();
            oCurrency = MapObject(oReader);
            return oCurrency;
        }

        private List<Currency> CreateObjects(IDataReader oReader)
        {
            List<Currency> oCurrency = new List<Currency>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Currency oItem = CreateObject(oHandler);
                oCurrency.Add(oItem);
            }
            return oCurrency;
        }

        #endregion

        #region Interface implementation
        public CurrencyService() { }

        public Currency Save(Currency oCurrency, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCurrency.CurrencyID <= 0)
                {
                    reader = CurrencyDA.InsertUpdate(tc, oCurrency, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = CurrencyDA.InsertUpdate(tc, oCurrency, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCurrency = new Currency();
                    oCurrency = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Currency. Because of " + e.Message, e);
                #endregion
            }
            return oCurrency;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Currency oCurrency = new Currency();
                oCurrency.CurrencyID = id;
                CurrencyDA.Delete(tc, oCurrency, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Currency. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public Currency Get(int id, Int64 nUserId)
        {
            Currency oAccountHead = new Currency();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CurrencyDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Currency", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<Currency> Gets(Int64 nUserId)
        {
            List<Currency> oCurrency = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CurrencyDA.Gets(tc);
                oCurrency = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Currency", e);
                #endregion
            }

            return oCurrency;
        }

        public List<Currency> Gets(String sql, Int64 nUserId)
        {
            List<Currency> oCurrency = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CurrencyDA.Gets(tc, sql);
                oCurrency = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Currency", e);
                #endregion
            }

            return oCurrency;
        }

        public List<Currency> GetsLeftSelectedCurrency(int nCurrencyID, Int64 nUserId)
        {
            List<Currency> oCurrencys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CurrencyDA.GetsLeftSelectedCurrency(tc, nCurrencyID);
                oCurrencys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Currency", e);
                #endregion
            }

            return oCurrencys;
        }
        #endregion
    } 
}
