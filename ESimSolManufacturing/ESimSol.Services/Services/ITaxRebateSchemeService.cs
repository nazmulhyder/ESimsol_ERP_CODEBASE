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
    public class ITaxRebateSchemeService : MarshalByRefObject, IITaxRebateSchemeService
    {
        #region Private functions and declaration
        private ITaxRebateScheme MapObject(NullHandler oReader)
        {
            ITaxRebateScheme oITaxRebateScheme = new ITaxRebateScheme();

            oITaxRebateScheme.ITaxRebateSchemeID = oReader.GetInt32("ITaxRebateSchemeID");
            oITaxRebateScheme.ITaxRateSchemeID = oReader.GetInt32("ITaxRateSchemeID");
            oITaxRebateScheme.ITaxRebateType = (EnumITaxRebateType)oReader.GetInt16("ITaxRebateType");
            oITaxRebateScheme.MaxRebateAmount = oReader.GetDouble("MaxRebateAmount");
            oITaxRebateScheme.PercentOfTaxIncome = oReader.GetDouble("PercentOfTaxIncome");
            oITaxRebateScheme.RebateInPercent = oReader.GetDouble("RebateInPercent");

            return oITaxRebateScheme;

        }

        private ITaxRebateScheme CreateObject(NullHandler oReader)
        {
            ITaxRebateScheme oITaxRebateScheme = MapObject(oReader);
            return oITaxRebateScheme;
        }

        private List<ITaxRebateScheme> CreateObjects(IDataReader oReader)
        {
            List<ITaxRebateScheme> oITaxRebateSchemes = new List<ITaxRebateScheme>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxRebateScheme oItem = CreateObject(oHandler);
                oITaxRebateSchemes.Add(oItem);
            }
            return oITaxRebateSchemes;
        }

        #endregion

        #region Interface implementation
        public ITaxRebateSchemeService() { }

        public ITaxRebateScheme IUD(ITaxRebateScheme oITaxRebateScheme, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxRebateSchemeDA.IUD(tc, oITaxRebateScheme, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxRebateScheme = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateScheme.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oITaxRebateScheme.ITaxRebateSchemeID = 0;
                #endregion
            }
            return oITaxRebateScheme;
        }


        public ITaxRebateScheme Get(int nITaxRebateSchemeID, Int64 nUserId)
        {
            ITaxRebateScheme oITaxRebateScheme = new ITaxRebateScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebateSchemeDA.Get(nITaxRebateSchemeID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateScheme = CreateObject(oReader);
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

                oITaxRebateScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebateScheme;
        }

        public ITaxRebateScheme Get(string sSQL, Int64 nUserId)
        {
            ITaxRebateScheme oITaxRebateScheme = new ITaxRebateScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebateSchemeDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateScheme = CreateObject(oReader);
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

                oITaxRebateScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebateScheme;
        }

        public List<ITaxRebateScheme> Gets(Int64 nUserID)
        {
            List<ITaxRebateScheme> oITaxRebateScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebateSchemeDA.Gets(tc);
                oITaxRebateScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxRebateScheme", e);
                #endregion
            }
            return oITaxRebateScheme;
        }

        public List<ITaxRebateScheme> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxRebateScheme> oITaxRebateScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebateSchemeDA.Gets(sSQL, tc);
                oITaxRebateScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxRebateScheme", e);
                #endregion
            }
            return oITaxRebateScheme;
        }

        #endregion
     

    }
}
