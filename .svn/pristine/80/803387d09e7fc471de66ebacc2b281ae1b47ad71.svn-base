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
    public class CandidateService : MarshalByRefObject, ICandidateService
    {
        #region Private functions and declaration
        private Candidate MapObject(NullHandler oReader)
        {
            Candidate oCandidate = new Candidate();
            oCandidate.CandidateID = oReader.GetInt32("CandidateID");
            oCandidate.Code = oReader.GetString("Code");
            oCandidate.Name = oReader.GetString("Name");
            oCandidate.FatherName = oReader.GetString("FatherName");
            oCandidate.MotherName = oReader.GetString("MotherName");
            oCandidate.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oCandidate.Gender = oReader.GetString("Gender");
            oCandidate.MaritalStatus = oReader.GetString("MaritalStatus");
            oCandidate.Nationalism = oReader.GetString("Nationalism");
            oCandidate.NationalID = oReader.GetString("NationalID");
            oCandidate.Religious = oReader.GetString("Religious");
            oCandidate.PresentAddress = oReader.GetString("PresentAddress");
            oCandidate.ParmanentAddress = oReader.GetString("ParmanentAddress");
            oCandidate.ContactNo = oReader.GetString("ContactNo");
            oCandidate.AlternateContactNo = oReader.GetString("AlternateContactNo");
            oCandidate.Email = oReader.GetString("Email");
            oCandidate.AlternateEmail = oReader.GetString("AlternateEmail");
            oCandidate.Objective = oReader.GetString("Objective");
            oCandidate.PresentSalary = oReader.GetDouble("PresentSalary");
            oCandidate.ExpectedSalary = oReader.GetDouble("ExpectedSalary");
            oCandidate.CareerSummary = oReader.GetString("CareerSummary");
            oCandidate.SpecialQualification = oReader.GetString("SpecialQualification");
            oCandidate.Photo = oReader.GetBytes("Photo");
            oCandidate.LastUpdatedDateTime = oReader.GetDateTime("LastUpdatedDateTime");

            return oCandidate;

        }

        private Candidate CreateObject(NullHandler oReader)
        {
            Candidate oCandidate = MapObject(oReader);
            return oCandidate;
        }

        private List<Candidate> CreateObjects(IDataReader oReader)
        {
            List<Candidate> oCandidate = new List<Candidate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Candidate oItem = CreateObject(oHandler);
                oCandidate.Add(oItem);
            }
            return oCandidate;
        }

        #endregion

        #region Interface implementation
        public CandidateService() { }

        public Candidate IUD(Candidate oCandidate, int nDBOperation, Int64 nUserID)
        {
            Candidate oTempC = new Candidate();
            oTempC.Photo = oCandidate.Photo;
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CandidateDA.IUD(tc, oCandidate, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCandidate = CreateObject(oReader);
                }
                reader.Close();
                oTempC.CandidateID = oCandidate.CandidateID;
                if (oTempC.Photo != null)
                {
                    CandidateDA.UpdatePhoto(tc, oTempC, nUserID);
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidate.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCandidate.CandidateID = 0;
                #endregion
            }
            return oCandidate;
        }


        public Candidate Get(int nCandidateID, Int64 nUserId)
        {
            Candidate oCandidate = new Candidate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateDA.Get(nCandidateID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidate = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get Candidate", e);
                oCandidate.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidate;
        }

        public Candidate Get(string sSql, Int64 nUserId)
        {
            Candidate oCandidate = new Candidate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidate = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get Candidate", e);
                oCandidate.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidate;
        }

        public List<Candidate> Gets(Int64 nUserID)
        {
            List<Candidate> oCandidate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateDA.Gets(tc);
                oCandidate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Candidate", e);
                #endregion
            }
            return oCandidate;
        }

        public List<Candidate> Gets(string sSQL, Int64 nUserID)
        {
            List<Candidate> oCandidate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateDA.Gets(sSQL, tc);
                oCandidate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Candidate", e);
                #endregion
            }
            return oCandidate;
        }
        public Candidate InsertNewCandidate(Candidate oCandidate, int nEnumDBOperation, Int64 nUserID)
        {
            Candidate oTempCP = new Candidate();
            oTempCP.Photo = oCandidate.Photo;
            CandidateUser oCandidateUser = new CandidateUser();
            oCandidateUser = oCandidate.CandidateUser;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader; NullHandler oReader;
                reader = CandidateDA.IUD(tc, oCandidate, nUserID, nEnumDBOperation);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidate = new Candidate();
                    oCandidate = CreateObject(oReader);
                }
                reader.Close();

                if (nEnumDBOperation == (int)EnumDBOperation.Insert)
                {
                    
                    #region User
                    IDataReader userreader;
                    oCandidateUser.CandidateID = oCandidate.CandidateID;
                    oCandidateUser.LogInID = oCandidateUser.UserName;
                    oCandidateUser.Password = Global.Encrypt(oCandidateUser.Password);
                    if (oCandidateUser.UserID <= 0)
                    {
                        userreader = CandidateUserDA.IUD(tc, oCandidateUser, nUserID, (int)EnumDBOperation.Insert);
                    }
                    else
                    {
                        userreader = CandidateUserDA.IUD(tc, oCandidateUser, nUserID, (int)EnumDBOperation.Update);

                    }
                    NullHandler oReaderDetail = new NullHandler(userreader);
                    userreader.Close();

                    #endregion User

                }

                oTempCP.CandidateID = oCandidate.CandidateID;
                if (oTempCP.Photo != null)
                {
                    CandidateDA.UpdatePhoto(tc, oTempCP, nUserID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                string sMessage = "";
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidate = new Candidate();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Candidate. Because of " + e.Message, e);
                sMessage = e.Message;
                if (sMessage.Contains("!"))
                {
                    sMessage = sMessage.Split('!')[0];
                }
                oCandidate.ErrorMessage = sMessage;
                #endregion
            }
            return oCandidate;
        }
        #endregion

        
    }
}
