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
    public class CandidateApplicationService : MarshalByRefObject, ICandidateApplicationService
    {
        #region Private functions and declaration
        private CandidateApplication MapObject(NullHandler oReader)
        {
            CandidateApplication oCandidateApplication = new CandidateApplication();
            oCandidateApplication.CAppID = oReader.GetInt32("CAppID");
            oCandidateApplication.CandidateID = oReader.GetInt64("CandidateID");
            oCandidateApplication.CircularID = oReader.GetInt32("CircularID");
            oCandidateApplication.ExpectedSalary = oReader.GetDouble("ExpectedSalary");
            oCandidateApplication.IsActive = oReader.GetBoolean("IsActive");
            oCandidateApplication.IsSelected = oReader.GetBoolean("IsSelected");
            oCandidateApplication.InterviewDate = oReader.GetDateTime("InterviewDate");
            oCandidateApplication.Note = oReader.GetString("Note");
            oCandidateApplication.CandidateName = oReader.GetString("CandidateName");
            oCandidateApplication.NoOfPosition = oReader.GetInt32("NoOfPosition");
            oCandidateApplication.CircularStartDate = oReader.GetDateTime("CircularStartDate");
            oCandidateApplication.CircularEndDate = oReader.GetDateTime("CircularStartDate");
            oCandidateApplication.DepartmentName = oReader.GetString("DepartmentName");
            oCandidateApplication.DesignationName = oReader.GetString("DesignationName");
            oCandidateApplication.ApplicationDate = oReader.GetDateTime("DBServerDate");
            return oCandidateApplication;

        }

        private CandidateApplication CreateObject(NullHandler oReader)
        {
            CandidateApplication oCandidateApplication = MapObject(oReader);
            return oCandidateApplication;
        }

        private List<CandidateApplication> CreateObjects(IDataReader oReader)
        {
            List<CandidateApplication> oCandidateApplication = new List<CandidateApplication>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CandidateApplication oItem = CreateObject(oHandler);
                oCandidateApplication.Add(oItem);
            }
            return oCandidateApplication;
        }

        #endregion

        #region Interface implementation
        public CandidateApplicationService() { }

        public CandidateApplication IUD(CandidateApplication oCandidateApplication, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CandidateApplicationDA.IUD(tc, oCandidateApplication, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCandidateApplication = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidateApplication.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCandidateApplication.CAppID = 0;
                #endregion
            }
            return oCandidateApplication;
        }

        public CandidateApplication Get(int nCAppID, Int64 nUserId)
        {
            CandidateApplication oCandidateApplication = new CandidateApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateApplicationDA.Get(nCAppID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateApplication = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateApplication", e);
                oCandidateApplication.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateApplication;
        }

        public CandidateApplication Get(string sSql, Int64 nUserId)
        {
            CandidateApplication oCandidateApplication = new CandidateApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateApplicationDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateApplication = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateApplication", e);
                oCandidateApplication.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateApplication;
        }

        public List<CandidateApplication> Gets(Int64 nUserID)
        {
            List<CandidateApplication> oCandidateApplication = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateApplicationDA.Gets(tc);
                oCandidateApplication = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateApplication", e);
                #endregion
            }
            return oCandidateApplication;
        }

        public List<CandidateApplication> Gets(string sSQL, Int64 nUserID)
        {
            List<CandidateApplication> oCandidateApplication = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateApplicationDA.Gets(sSQL, tc);
                oCandidateApplication = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateApplication", e);
                #endregion
            }
            return oCandidateApplication;
        }

        #endregion


    }
}
