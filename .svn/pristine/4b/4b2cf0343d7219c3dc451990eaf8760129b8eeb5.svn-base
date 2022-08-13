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
    public class ITaxRateSlabService : MarshalByRefObject, IITaxRateSlabService
    {
        #region Private functions and declaration
        private ITaxRateSlab MapObject(NullHandler oReader)
        {
            ITaxRateSlab oITaxRateSlab = new ITaxRateSlab();

            oITaxRateSlab.ITaxRateSlabID = oReader.GetInt32("ITaxRateSlabID");
            oITaxRateSlab.ITaxRateSchemeID = oReader.GetInt32("ITaxRateSchemeID");
            oITaxRateSlab.SequenceType = (EnumSequenceType)oReader.GetInt16("SequenceType");
            oITaxRateSlab.Amount = oReader.GetDouble("Amount");
            oITaxRateSlab.Percents = oReader.GetInt32("Percents");

            return oITaxRateSlab;

        }

        private ITaxRateSlab CreateObject(NullHandler oReader)
        {
            ITaxRateSlab oITaxRateSlab = MapObject(oReader);
            return oITaxRateSlab;
        }

        private List<ITaxRateSlab> CreateObjects(IDataReader oReader)
        {
            List<ITaxRateSlab> oITaxRateSlabs = new List<ITaxRateSlab>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxRateSlab oItem = CreateObject(oHandler);
                oITaxRateSlabs.Add(oItem);
            }
            return oITaxRateSlabs;
        }

        #endregion

        #region Interface implementation
        public ITaxRateSlabService() { }

        public ITaxRateSlab IUD(ITaxRateSlab oITaxRateSlab, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxRateSlabDA.IUD(tc, oITaxRateSlab, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxRateSlab = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRateSlab.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oITaxRateSlab.ITaxRateSlabID = 0;
                #endregion
            }
            return oITaxRateSlab;
        }


        public ITaxRateSlab Get(int nITaxRateSlabID, Int64 nUserId)
        {
            ITaxRateSlab oITaxRateSlab = new ITaxRateSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRateSlabDA.Get(nITaxRateSlabID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRateSlab = CreateObject(oReader);
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

                oITaxRateSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRateSlab;
        }

        public ITaxRateSlab Get(string sSQL, Int64 nUserId)
        {
            ITaxRateSlab oITaxRateSlab = new ITaxRateSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRateSlabDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRateSlab = CreateObject(oReader);
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

                oITaxRateSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRateSlab;
        }

        public List<ITaxRateSlab> Gets(Int64 nUserID)
        {
            List<ITaxRateSlab> oITaxRateSlab = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRateSlabDA.Gets(tc);
                oITaxRateSlab = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxRateSlab", e);
                #endregion
            }
            return oITaxRateSlab;
        }

        public List<ITaxRateSlab> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxRateSlab> oITaxRateSlab = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRateSlabDA.Gets(sSQL, tc);
                oITaxRateSlab = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxRateSlab", e);
                #endregion
            }
            return oITaxRateSlab;
        }

        #endregion


    }
}
