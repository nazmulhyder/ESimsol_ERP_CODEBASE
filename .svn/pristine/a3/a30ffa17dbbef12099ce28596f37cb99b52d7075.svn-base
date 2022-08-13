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
    public class CandidateEducationService : MarshalByRefObject, ICandidateEducationService
    {
        #region Private functions and declaration
        private CandidateEducation MapObject(NullHandler oReader)
        {
            CandidateEducation oCandidateEducation = new CandidateEducation();
            oCandidateEducation.CEID = oReader.GetInt32("CEID");
            oCandidateEducation.CandidateID = oReader.GetInt32("CandidateID");
            oCandidateEducation.Degree = oReader.GetString("Degree");
            oCandidateEducation.Major = oReader.GetString("Major");
            oCandidateEducation.Session = oReader.GetString("Session");
            oCandidateEducation.PassingYear = oReader.GetDateTime("PassingYear");
            oCandidateEducation.BoardUniversity = oReader.GetString("BoardUniversity");
            oCandidateEducation.Institution = oReader.GetString("Institution");
            oCandidateEducation.Result = oReader.GetString("Result");
            oCandidateEducation.Country = oReader.GetString("Country");
            oCandidateEducation.LastUpdatedDate = oReader.GetDateTime("LastUpdatedDate");

            return oCandidateEducation;

        }

        private CandidateEducation CreateObject(NullHandler oReader)
        {
            CandidateEducation oCandidateEducation = MapObject(oReader);
            return oCandidateEducation;
        }

        private List<CandidateEducation> CreateObjects(IDataReader oReader)
        {
            List<CandidateEducation> oCandidateEducation = new List<CandidateEducation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CandidateEducation oItem = CreateObject(oHandler);
                oCandidateEducation.Add(oItem);
            }
            return oCandidateEducation;
        }

        #endregion

        #region Interface implementation
        public CandidateEducationService() { }

        public CandidateEducation IUD(CandidateEducation oCandidateEducation, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CandidateEducationDA.IUD(tc, oCandidateEducation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCandidateEducation = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidateEducation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCandidateEducation.CEID = 0;
                #endregion
            }
            return oCandidateEducation;
        }

        public CandidateEducation Get(int nCEID, Int64 nUserId)
        {
            CandidateEducation oCandidateEducation = new CandidateEducation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateEducationDA.Get(nCEID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateEducation = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateEducation", e);
                oCandidateEducation.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateEducation;
        }

        public CandidateEducation Get(string sSql, Int64 nUserId)
        {
            CandidateEducation oCandidateEducation = new CandidateEducation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateEducationDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateEducation = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateEducation", e);
                oCandidateEducation.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateEducation;
        }

        public List<CandidateEducation> Gets(int nCID, Int64 nUserID)
        {
            List<CandidateEducation> oCandidateEducation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateEducationDA.Gets(nCID, tc);
                oCandidateEducation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateEducation", e);
                #endregion
            }
            return oCandidateEducation;
        }

        public List<CandidateEducation> Gets(string sSQL, Int64 nUserID)
        {
            List<CandidateEducation> oCandidateEducation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateEducationDA.Gets(sSQL, tc);
                oCandidateEducation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateEducation", e);
                #endregion
            }
            return oCandidateEducation;
        }

        #endregion


    }
}
