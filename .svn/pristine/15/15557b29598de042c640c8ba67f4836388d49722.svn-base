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
    public class CandidateTrainingService : MarshalByRefObject, ICandidateTrainingService
    {
        #region Private functions and declaration
        private CandidateTraining MapObject(NullHandler oReader)
        {
            CandidateTraining oCandidateTraining = new CandidateTraining();
            oCandidateTraining.CTID = oReader.GetInt32("CTID");
            oCandidateTraining.CandidateID = oReader.GetInt32("CandidateID");
            oCandidateTraining.CourseName = oReader.GetString("CourseName");
            oCandidateTraining.Specification = oReader.GetString("Specification");
            oCandidateTraining.StartDate = oReader.GetDateTime("StartDate");
            oCandidateTraining.EndDate = oReader.GetDateTime("EndDate");
            oCandidateTraining.Duration = oReader.GetInt32("Duration");
            oCandidateTraining.PassingDate = oReader.GetDateTime("PassingDate");
            oCandidateTraining.Result = oReader.GetString("Result");
            oCandidateTraining.Institution = oReader.GetString("Institution");
            oCandidateTraining.CertifyBodyVendor = oReader.GetString("CertifyBodyVendor");
            oCandidateTraining.Country = oReader.GetString("Country");
            oCandidateTraining.LastUpdatedDate = oReader.GetDateTime("LastUpdatedDate");

            return oCandidateTraining;

        }

        private CandidateTraining CreateObject(NullHandler oReader)
        {
            CandidateTraining oCandidateTraining = MapObject(oReader);
            return oCandidateTraining;
        }

        private List<CandidateTraining> CreateObjects(IDataReader oReader)
        {
            List<CandidateTraining> oCandidateTraining = new List<CandidateTraining>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CandidateTraining oItem = CreateObject(oHandler);
                oCandidateTraining.Add(oItem);
            }
            return oCandidateTraining;
        }

        #endregion

        #region Interface implementation
        public CandidateTrainingService() { }

        public CandidateTraining IUD(CandidateTraining oCandidateTraining, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CandidateTrainingDA.IUD(tc, oCandidateTraining, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCandidateTraining = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidateTraining.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCandidateTraining.CTID = 0;
                #endregion
            }
            return oCandidateTraining;
        }

        public CandidateTraining Get(int nCTID, Int64 nUserId)
        {
            CandidateTraining oCandidateTraining = new CandidateTraining();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateTrainingDA.Get(nCTID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateTraining = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateTraining", e);
                oCandidateTraining.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateTraining;
        }

        public CandidateTraining Get(string sSql, Int64 nUserId)
        {
            CandidateTraining oCandidateTraining = new CandidateTraining();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateTrainingDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateTraining = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateTraining", e);
                oCandidateTraining.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateTraining;
        }

        public List<CandidateTraining> Gets(int nCID,Int64 nUserID)
        {
            List<CandidateTraining> oCandidateTraining = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateTrainingDA.Gets(nCID,tc);
                oCandidateTraining = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateTraining", e);
                #endregion
            }
            return oCandidateTraining;
        }

        public List<CandidateTraining> Gets(string sSQL, Int64 nUserID)
        {
            List<CandidateTraining> oCandidateTraining = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateTrainingDA.Gets(sSQL, tc);
                oCandidateTraining = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateTraining", e);
                #endregion
            }
            return oCandidateTraining;
        }

        #endregion


    }
}
