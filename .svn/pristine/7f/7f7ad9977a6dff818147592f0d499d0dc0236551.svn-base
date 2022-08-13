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
    public class CandidateExperienceService : MarshalByRefObject, ICandidateExperienceService
    {
        #region Private functions and declaration
        private CandidateExperience MapObject(NullHandler oReader)
        {
            CandidateExperience oCandidateExperience = new CandidateExperience();
            oCandidateExperience.CExpID = oReader.GetInt32("CExpID");
            oCandidateExperience.CandidateID = oReader.GetInt32("CandidateID");
            oCandidateExperience.Organization = oReader.GetString("Organization");
            oCandidateExperience.OrganizationType = oReader.GetString("OrganizationType");
            oCandidateExperience.Address = oReader.GetString("Address");
            oCandidateExperience.Department = oReader.GetString("Department");
            oCandidateExperience.Designation = oReader.GetString("Designation");
            oCandidateExperience.StartDate = oReader.GetDateTime("StartDate");
            oCandidateExperience.EndDate = oReader.GetDateTime("EndDate");
            oCandidateExperience.AreaOfExperience = oReader.GetString("AreaOfExperience");
            oCandidateExperience.MajorResponsibility = oReader.GetString("MajorResponsibility");
            oCandidateExperience.LastUpdatedDate = oReader.GetDateTime("LastUpdatedDate");

            return oCandidateExperience;

        }

        private CandidateExperience CreateObject(NullHandler oReader)
        {
            CandidateExperience oCandidateExperience = MapObject(oReader);
            return oCandidateExperience;
        }

        private List<CandidateExperience> CreateObjects(IDataReader oReader)
        {
            List<CandidateExperience> oCandidateExperience = new List<CandidateExperience>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CandidateExperience oItem = CreateObject(oHandler);
                oCandidateExperience.Add(oItem);
            }
            return oCandidateExperience;
        }

        #endregion

        #region Interface implementation
        public CandidateExperienceService() { }

        public CandidateExperience IUD(CandidateExperience oCandidateExperience, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CandidateExperienceDA.IUD(tc, oCandidateExperience, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCandidateExperience = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidateExperience.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCandidateExperience.CExpID = 0;
                #endregion
            }
            return oCandidateExperience;
        }

        public CandidateExperience Get(int nCExpID, Int64 nUserId)
        {
            CandidateExperience oCandidateExperience = new CandidateExperience();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateExperienceDA.Get(nCExpID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateExperience = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateExperience", e);
                oCandidateExperience.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateExperience;
        }

        public CandidateExperience Get(string sSql, Int64 nUserId)
        {
            CandidateExperience oCandidateExperience = new CandidateExperience();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateExperienceDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateExperience = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateExperience", e);
                oCandidateExperience.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateExperience;
        }

        public List<CandidateExperience> Gets(int nCID,Int64 nUserID)
        {
            List<CandidateExperience> oCandidateExperience = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateExperienceDA.Gets(nCID,tc);
                oCandidateExperience = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateExperience", e);
                #endregion
            }
            return oCandidateExperience;
        }

        public List<CandidateExperience> Gets(string sSQL, Int64 nUserID)
        {
            List<CandidateExperience> oCandidateExperience = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateExperienceDA.Gets(sSQL, tc);
                oCandidateExperience = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateExperience", e);
                #endregion
            }
            return oCandidateExperience;
        }

        #endregion


    }
}
