using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class CountryService : MarshalByRefObject, ICountryService
    {
        #region Private functions and declaration
        private Country MapObject(NullHandler oReader)
        {
            Country oCountry = new Country();
            oCountry.CountryID = oReader.GetInt32("CountryID");
            oCountry.Name = oReader.GetString("Name");
            oCountry.ShortName = oReader.GetString("ShortName");
            oCountry.Code = oReader.GetString("Code");
            oCountry.Note = oReader.GetString("Note");
         
            return oCountry;
        }

        private Country CreateObject(NullHandler oReader)
        {
            Country oCountry = new Country();
            oCountry = MapObject(oReader);
            return oCountry;
        }

        private List<Country> CreateObjects(IDataReader oReader)
        {
            List<Country> oCountrys = new List<Country>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Country oItem = CreateObject(oHandler);
                oCountrys.Add(oItem);
            }
            return oCountrys;
        }

        #endregion

        #region Interface implementation
        public CountryService() { }


        public Country Save(Country oCountry, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region Country
                IDataReader reader;
                if (oCountry.CountryID <= 0)
                {
                    reader = CountryDA.InsertUpdate(tc, oCountry, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = CountryDA.InsertUpdate(tc, oCountry, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCountry = new Country();
                    oCountry = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCountry = new Country();
                oCountry.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oCountry;
        }

        public String Delete(Country oCountry, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CountryDA.Delete(tc, oCountry, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public Country Get(int id, Int64 nUserId)
        {
            Country oCountry = new Country();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CountryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCountry = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oCountry;
        }

        public List<Country> Gets(string sSQL, Int64 nUserID)
        {
            List<Country> oCountry = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CountryDA.Gets(sSQL, tc);
                oCountry = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Country", e);
                #endregion
            }
            return oCountry;
        }
        public Country GetByType(int nRequisitionType, Int64 nUserId)
        {
            Country oCountry = new Country();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CountryDA.GetByType(tc, nRequisitionType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCountry = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oCountry;
        }


        public List<Country> Gets(Int64 nUserId)
        {
            List<Country> oCountrys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CountryDA.Gets(tc);
                oCountrys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oCountrys;
        }
        public List<Country> Gets(int nBUID, Int64 nUserId)
        {
            List<Country> oCountrys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CountryDA.Gets(tc, nBUID);
                oCountrys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oCountrys;
        }

        #endregion
    }
}