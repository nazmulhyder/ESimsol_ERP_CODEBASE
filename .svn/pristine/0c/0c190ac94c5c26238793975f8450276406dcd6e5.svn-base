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
    public class EmployeeTrainingService : MarshalByRefObject, IEmployeeTrainingService
    {
        #region Private functions and declaration
        private EmployeeTraining MapObject(NullHandler oReader)
        {
            EmployeeTraining oET = new EmployeeTraining();
            oET.EmployeeTrainingID = oReader.GetInt32("EmployeeTrainingID");
            oET.EmployeeID = oReader.GetInt32("EmployeeID");
            oET.Sequence = oReader.GetInt32("Sequence");
            oET.CourseName = oReader.GetString("CourseName");
            oET.Specification = oReader.GetString("Specification");
            oET.CertificateRegNo = oReader.GetString("CertificateRegNo");
            oET.StartDate = oReader.GetDateTime("StartDate");
            oET.StartDateTrainingFormatType = (EnumCustomDateFormat)oReader.GetInt32("StartDateTrainingFormatType");
            oET.EndDate = oReader.GetDateTime("EndDate");
            oET.EndDateTrainingFormatType = (EnumCustomDateFormat)oReader.GetInt32("EndDateTrainingFormatType");
            oET.Duration = oReader.GetString("Duration");
            oET.TrainingDuration = oReader.GetString("TrainingDuration");
            oET.PassingDate = oReader.GetDateTime("PassingDate");
            oET.PassingDateTrainingFormatType = (EnumCustomDateFormat)oReader.GetInt32("PassingDateTrainingFormatType");
            oET.Result = oReader.GetString("Result");
            oET.Institution = oReader.GetString("Institution");
            oET.CertifyBodyVendor = oReader.GetString("CertifyBodyVendor");
            oET.Country = oReader.GetString("Country");
            return oET;
        }

        private EmployeeTraining CreateObject(NullHandler oReader)
        {
            EmployeeTraining oEmployeeTraining = new EmployeeTraining();
            oEmployeeTraining = MapObject(oReader);
            return oEmployeeTraining;
        }

        private List<EmployeeTraining> CreateObjects(IDataReader oReader)
        {
            List<EmployeeTraining> oEmployeeTraining = new List<EmployeeTraining>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeTraining oItem = CreateObject(oHandler);
                oEmployeeTraining.Add(oItem);
            }
            return oEmployeeTraining;
        }

        #endregion

        #region Interface implementation
        public EmployeeTrainingService() { }

        public EmployeeTraining IUD(EmployeeTraining oET, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeTrainingDA.IUD(tc, oET, nDBOperation, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oET = new EmployeeTraining();
                    oET = CreateObject(oReader);
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
                oET.ErrorMessage = e.Message;
                //throw new ServiceException("Failed to Save EmployeeTraining. Because of " + e.Message, e);
                #endregion
            }
            return oET;
        }


        public EmployeeTraining Get(int id, Int64 nUserId) //EmployeeTrainingID
        {
            EmployeeTraining oET = new EmployeeTraining();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeTrainingDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oET = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeTraining", e);
                #endregion
            }

            return oET;
        }

        public List<EmployeeTraining> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<EmployeeTraining> oEmployeeTraining = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeTrainingDA.Gets(tc, nEmployeeID);
                oEmployeeTraining = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeTraining", e);
                #endregion
            }

            return oEmployeeTraining;
        }

        #endregion
    }
}