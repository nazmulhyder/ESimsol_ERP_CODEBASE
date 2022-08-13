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
    public class EmployeeEducationService : MarshalByRefObject, IEmployeeEducationService
    {
        #region Private functions and declaration
        private EmployeeEducation MapObject(NullHandler oReader)
        {
            EmployeeEducation oEE = new EmployeeEducation();
            oEE.EmployeeEducationID = oReader.GetInt32("EmployeeEducationID");
            oEE.EmployeeID = oReader.GetInt32("EmployeeID");
            oEE.Sequence = oReader.GetInt32("Sequence");
            oEE.Degree = oReader.GetString("Degree");
            oEE.Major = oReader.GetString("Major");
            oEE.Session = oReader.GetString("Session");
            oEE.PassingYear = oReader.GetInt32("PassingYear");
            oEE.BoardUniversity = oReader.GetString("BoardUniversity");
            oEE.Institution = oReader.GetString("Institution");
            oEE.Result = oReader.GetString("Result");
            oEE.Country = oReader.GetString("Country");
            return oEE;
        }

        private EmployeeEducation CreateObject(NullHandler oReader)
        {
            EmployeeEducation oEmployeeEducation = new EmployeeEducation();
            oEmployeeEducation = MapObject(oReader);
            return oEmployeeEducation;
        }

        private List<EmployeeEducation> CreateObjects(IDataReader oReader)
        {
            List<EmployeeEducation> oEmployeeEducation = new List<EmployeeEducation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeEducation oItem = CreateObject(oHandler);
                oEmployeeEducation.Add(oItem);
            }
            return oEmployeeEducation;
        }

        #endregion

        #region Interface implementation
        public EmployeeEducationService() { }

        public EmployeeEducation IUD(EmployeeEducation oEE,int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeEducationDA.IUD(tc, oEE, nDBOperation, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEE = new EmployeeEducation();
                    oEE = CreateObject(oReader);
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
                oEE.ErrorMessage = e.Message;
                //throw new ServiceException("Failed to Save EmployeeEducation. Because of " + e.Message, e);
                #endregion
            }
            return oEE;
        }
        

        public EmployeeEducation Get(int id, Int64 nUserId) //EmployeeEducationID
        {
            EmployeeEducation oEE = new EmployeeEducation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeEducationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEE = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeEducation", e);
                #endregion
            }

            return oEE;
        }

        public List<EmployeeEducation> Gets(int nEmployeeID,Int64 nUserID)
        {
            List<EmployeeEducation> oEmployeeEducation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeEducationDA.Gets(tc, nEmployeeID);
                oEmployeeEducation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeEducation", e);
                #endregion
            }

            return oEmployeeEducation;
        }

        public List<EmployeeEducation> Gets(string sSql, Int64 nUserID)
        {
            List<EmployeeEducation> oEmployeeEducations = new List<EmployeeEducation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeEducationDA.Gets(tc, sSql);
                oEmployeeEducations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeEducation", e);
                #endregion
            }
            return oEmployeeEducations;
        }
        #endregion
    }   
}