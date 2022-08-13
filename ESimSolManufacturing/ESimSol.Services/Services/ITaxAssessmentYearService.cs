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
    public class ITaxAssessmentYearService : MarshalByRefObject, IITaxAssessmentYearService
    {
        #region Private functions and declaration
        private ITaxAssessmentYear MapObject(NullHandler oReader)
        {
            ITaxAssessmentYear oITaxAssessmentYear = new ITaxAssessmentYear();

            oITaxAssessmentYear.ITaxAssessmentYearID = oReader.GetInt32("ITaxAssessmentYearID");
            oITaxAssessmentYear.Description = oReader.GetString("Description");
            oITaxAssessmentYear.StartDate = oReader.GetDateTime("StartDate");
            oITaxAssessmentYear.EndDate = oReader.GetDateTime("EndDate");
            oITaxAssessmentYear.IsActive = oReader.GetBoolean("IsActive");
            
            return oITaxAssessmentYear;

        }

        private ITaxAssessmentYear CreateObject(NullHandler oReader)
        {
            ITaxAssessmentYear oITaxAssessmentYear = MapObject(oReader);
            return oITaxAssessmentYear;
        }

        private List<ITaxAssessmentYear> CreateObjects(IDataReader oReader)
        {
            List<ITaxAssessmentYear> oITaxAssessmentYears = new List<ITaxAssessmentYear>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxAssessmentYear oItem = CreateObject(oHandler);
                oITaxAssessmentYears.Add(oItem);
            }
            return oITaxAssessmentYears;
        }

        #endregion

        #region Interface implementation
        public ITaxAssessmentYearService() { }

        public ITaxAssessmentYear IUD(ITaxAssessmentYear oITaxAssessmentYear, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxAssessmentYearDA.IUD(tc, oITaxAssessmentYear, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxAssessmentYear = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxAssessmentYear.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oITaxAssessmentYear.ITaxAssessmentYearID = 0;
                #endregion
            }
            return oITaxAssessmentYear;
        }


        public ITaxAssessmentYear Get(int nITaxAssessmentYearID, Int64 nUserId)
        {
            ITaxAssessmentYear oITaxAssessmentYear = new ITaxAssessmentYear();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxAssessmentYearDA.Get(nITaxAssessmentYearID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxAssessmentYear = CreateObject(oReader);
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
                
                oITaxAssessmentYear.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxAssessmentYear;
        }

        public ITaxAssessmentYear Get(string sSQL, Int64 nUserId)
        {
            ITaxAssessmentYear oITaxAssessmentYear = new ITaxAssessmentYear();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxAssessmentYearDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxAssessmentYear = CreateObject(oReader);
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
                
                oITaxAssessmentYear.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxAssessmentYear;
        }

        public List<ITaxAssessmentYear> Gets(Int64 nUserID)
        {
            List<ITaxAssessmentYear> oITaxAssessmentYear = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxAssessmentYearDA.Gets(tc);
                oITaxAssessmentYear = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxAssessmentYear", e);
                #endregion
            }
            return oITaxAssessmentYear;
        }

        public List<ITaxAssessmentYear> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxAssessmentYear> oITaxAssessmentYear = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxAssessmentYearDA.Gets(sSQL, tc);
                oITaxAssessmentYear = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxAssessmentYear", e);
                #endregion
            }
            return oITaxAssessmentYear;
        }

        #endregion


    }
}
