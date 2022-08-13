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
    public class ITaxRateSchemeService : MarshalByRefObject, IITaxRateSchemeService
    {
        #region Private functions and declaration
        private ITaxRateScheme MapObject(NullHandler oReader)
        {
            ITaxRateScheme oITaxRateScheme = new ITaxRateScheme();

            oITaxRateScheme.ITaxRateSchemeID = oReader.GetInt32("ITaxRateSchemeID");
            oITaxRateScheme.ITaxAssessmentYearID = oReader.GetInt32("ITaxAssessmentYearID");
            oITaxRateScheme.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oITaxRateScheme.TaxPayerType = (EnumTaxPayerType)oReader.GetInt16("TaxPayerType");
            oITaxRateScheme.TaxArea = (EnumTaxArea)oReader.GetInt16("TaxArea");
            oITaxRateScheme.MinimumTax = oReader.GetDouble("MinimumTax");
            oITaxRateScheme.IsActive = oReader.GetBoolean("IsActive");

            return oITaxRateScheme;

        }

        private ITaxRateScheme CreateObject(NullHandler oReader)
        {
            ITaxRateScheme oITaxRateScheme = MapObject(oReader);
            return oITaxRateScheme;
        }

        private List<ITaxRateScheme> CreateObjects(IDataReader oReader)
        {
            List<ITaxRateScheme> oITaxRateSchemes = new List<ITaxRateScheme>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxRateScheme oItem = CreateObject(oHandler);
                oITaxRateSchemes.Add(oItem);
            }
            return oITaxRateSchemes;
        }

        #endregion

        #region Interface implementation
        public ITaxRateSchemeService() { }

        public ITaxRateScheme IUD(ITaxRateScheme oITaxRateScheme, int nDBOperation, Int64 nUserID)
        {

            //List<ITaxRateSlab> oITaxRateSlabs = new List<ITaxRateSlab>();
            //oITaxRateSlabs = oITaxRateScheme.ITaxRateSlabs;
            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxRateSchemeDA.IUD(tc, oITaxRateScheme, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxRateScheme = CreateObject(oReader);
                }
                reader.Close();
                //#region ITaxRateSlabPart
                //if (nDBOperation != 3)
                //{
                //    if (oITaxRateScheme.ITaxRateSchemeID > 0)
                //    {
                //        foreach (ITaxRateSlab oItem in oITaxRateSlabs)
                //        {
                //            IDataReader readerDetail;
                //            oItem.ITaxRateSchemeID = oITaxRateScheme.ITaxRateSchemeID;
                //            if (oItem.ITaxRateSlabID <= 0)
                //            {
                //                readerDetail = ITaxRateSlabDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                //            }
                //            else
                //            {
                //                readerDetail = ITaxRateSlabDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update);

                //            }
                //            NullHandler oReaderDetail = new NullHandler(readerDetail);
                //            readerDetail.Close();
                //        }
                //    }

                //}
                //#endregion ITaxRateSlabPart

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRateScheme.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oITaxRateScheme.ITaxRateSchemeID = 0;
                #endregion
            }
            return oITaxRateScheme;
        }


        public ITaxRateScheme Get(int nITaxRateSchemeID, Int64 nUserId)
        {
            ITaxRateScheme oITaxRateScheme = new ITaxRateScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRateSchemeDA.Get(nITaxRateSchemeID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRateScheme = CreateObject(oReader);
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

                oITaxRateScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRateScheme;
        }

        public ITaxRateScheme Get(string sSQL, Int64 nUserId)
        {
            ITaxRateScheme oITaxRateScheme = new ITaxRateScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRateSchemeDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRateScheme = CreateObject(oReader);
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

                oITaxRateScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRateScheme;
        }

        public List<ITaxRateScheme> Gets(Int64 nUserID)
        {
            List<ITaxRateScheme> oITaxRateScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRateSchemeDA.Gets(tc);
                oITaxRateScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxRateScheme", e);
                #endregion
            }
            return oITaxRateScheme;
        }

        public List<ITaxRateScheme> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxRateScheme> oITaxRateScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRateSchemeDA.Gets(sSQL, tc);
                oITaxRateScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxRateScheme", e);
                #endregion
            }
            return oITaxRateScheme;
        }

        #region Activity
        public ITaxRateScheme Activite(int nITaxRateSchemeID, bool Active, Int64 nUserId)
        {
            ITaxRateScheme oITaxRateScheme = new ITaxRateScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRateSchemeDA.Activity(nITaxRateSchemeID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRateScheme = CreateObject(oReader);
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
                oITaxRateScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRateScheme;
        }


        #endregion

        #endregion


    }
}
