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
    public class CandidateReferenceService : MarshalByRefObject, ICandidateReferenceService
    {
        #region Private functions and declaration
        private CandidateReference MapObject(NullHandler oReader)
        {
            CandidateReference oCandidateReference = new CandidateReference();
            oCandidateReference.CRefID = oReader.GetInt32("CRefID");
            oCandidateReference.CandidateID = oReader.GetInt32("CandidateID");
            oCandidateReference.Name = oReader.GetString("Name");
            oCandidateReference.Organization = oReader.GetString("Organization");
            oCandidateReference.Department = oReader.GetString("Department");
            oCandidateReference.Designation = oReader.GetString("Designation");
            oCandidateReference.ContactNo = oReader.GetString("ContactNo");
            oCandidateReference.Email = oReader.GetString("Email");
            oCandidateReference.Address = oReader.GetString("Address");
            oCandidateReference.Relation = oReader.GetString("Relation");
            oCandidateReference.LastUpdatedDate = oReader.GetDateTime("LastUpdatedDate");

            return oCandidateReference;

        }

        private CandidateReference CreateObject(NullHandler oReader)
        {
            CandidateReference oCandidateReference = MapObject(oReader);
            return oCandidateReference;
        }

        private List<CandidateReference> CreateObjects(IDataReader oReader)
        {
            List<CandidateReference> oCandidateReference = new List<CandidateReference>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CandidateReference oItem = CreateObject(oHandler);
                oCandidateReference.Add(oItem);
            }
            return oCandidateReference;
        }

        #endregion

        #region Interface implementation
        public CandidateReferenceService() { }

        public CandidateReference IUD(CandidateReference oCandidateReference, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CandidateReferenceDA.IUD(tc, oCandidateReference, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCandidateReference = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidateReference.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCandidateReference.CRefID = 0;
                #endregion
            }
            return oCandidateReference;
        }

        public CandidateReference Get(int nCRefID, Int64 nUserId)
        {
            CandidateReference oCandidateReference = new CandidateReference();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateReferenceDA.Get(nCRefID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateReference = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateReference", e);
                oCandidateReference.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateReference;
        }

        public CandidateReference Get(string sSql, Int64 nUserId)
        {
            CandidateReference oCandidateReference = new CandidateReference();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateReferenceDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateReference = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateReference", e);
                oCandidateReference.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateReference;
        }

        public List<CandidateReference> Gets(int nCID,Int64 nUserID)
        {
            List<CandidateReference> oCandidateReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateReferenceDA.Gets(nCID,tc);
                oCandidateReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateReference", e);
                #endregion
            }
            return oCandidateReference;
        }

        public List<CandidateReference> Gets(string sSQL, Int64 nUserID)
        {
            List<CandidateReference> oCandidateReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateReferenceDA.Gets(sSQL, tc);
                oCandidateReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateReference", e);
                #endregion
            }
            return oCandidateReference;
        }

        #endregion


    }
}
