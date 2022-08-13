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
    public class TrainingDevelopmentService : MarshalByRefObject, ITrainingDevelopmentService
    {
        #region Private functions and declaration
        private TrainingDevelopment MapObject(NullHandler oReader)
        {
            TrainingDevelopment oTrainingDevelopment = new TrainingDevelopment();

            oTrainingDevelopment.TDID = oReader.GetInt32("TDID");
            oTrainingDevelopment.EmployeeID = oReader.GetInt32("EmployeeID");
            oTrainingDevelopment.CourseName = oReader.GetString("CourseName");
            oTrainingDevelopment.Specification = oReader.GetString("Specification");
            oTrainingDevelopment.Institute = oReader.GetString("Institute");
            oTrainingDevelopment.Vendor = oReader.GetString("Vendor");
            oTrainingDevelopment.Country = oReader.GetString("Country");
            oTrainingDevelopment.State = oReader.GetString("State");
            oTrainingDevelopment.Address = oReader.GetString("Address");
            oTrainingDevelopment.Duration = oReader.GetString("Duration");
            oTrainingDevelopment.StartDate = oReader.GetDateTime("StartDate");
            oTrainingDevelopment.EndDate = oReader.GetDateTime("EndDate");
            oTrainingDevelopment.EffectFromDate = oReader.GetDateTime("EffectFromDate");
            oTrainingDevelopment.EffectToDate = oReader.GetDateTime("EffectToDate");
            oTrainingDevelopment.Note = oReader.GetString("Note");
            oTrainingDevelopment.ApproveBy = oReader.GetInt32("ApproveBy");
            oTrainingDevelopment.ApprovalNote = oReader.GetString("ApprovalNote");
            oTrainingDevelopment.Result = oReader.GetString("Result");
            oTrainingDevelopment.PassingDate = oReader.GetDateTime("PassingDate");
            oTrainingDevelopment.ResultNote = oReader.GetString("ResultNote");
            oTrainingDevelopment.IsCompleted = oReader.GetBoolean("IsCompleted");
            oTrainingDevelopment.IsActive = oReader.GetBoolean("IsActive");
            oTrainingDevelopment.InactiveNote = oReader.GetString("InactiveNote");
            oTrainingDevelopment.InactiveDate = oReader.GetDateTime("InactiveDate");

            //derive
            oTrainingDevelopment.EmployeeName = oReader.GetString("EmployeeName");
            oTrainingDevelopment.EmployeeCode = oReader.GetString("EmployeeCode");
            oTrainingDevelopment.DepartmentName = oReader.GetString("DepartmentName");
            oTrainingDevelopment.DesignationName = oReader.GetString("DesignationName");
            oTrainingDevelopment.LocationName = oReader.GetString("LocationName");
            oTrainingDevelopment.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oTrainingDevelopment.WorkingStatus = (EnumEmployeeWorkigStatus)oReader.GetInt16("WorkingStatus");
            oTrainingDevelopment.ApproveByName = oReader.GetString("ApproveByName");

            
            
            return oTrainingDevelopment;

        }

        private TrainingDevelopment CreateObject(NullHandler oReader)
        {
            TrainingDevelopment oTrainingDevelopment = MapObject(oReader);
            return oTrainingDevelopment;
        }

        private List<TrainingDevelopment> CreateObjects(IDataReader oReader)
        {
            List<TrainingDevelopment> oTrainingDevelopments = new List<TrainingDevelopment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TrainingDevelopment oItem = CreateObject(oHandler);
                oTrainingDevelopments.Add(oItem);
            }
            return oTrainingDevelopments;
        }

        #endregion

        #region Interface implementation
        public TrainingDevelopmentService() { }

        public TrainingDevelopment IUD(TrainingDevelopment oTrainingDevelopment, int nDBOperation, Int64 nUserID)
        {

            
            TransactionContext tc = null;

            try
            {
               
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TrainingDevelopmentDA.IUD(tc, oTrainingDevelopment, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oTrainingDevelopment = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTrainingDevelopment.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oTrainingDevelopment.TDID = 0;
                #endregion
            }
            return oTrainingDevelopment;
        }


        public TrainingDevelopment Get(int nTDID, Int64 nUserId)
        {
            TrainingDevelopment oTrainingDevelopment = new TrainingDevelopment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TrainingDevelopmentDA.Get(nTDID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTrainingDevelopment = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get TrainingDevelopment", e);
                oTrainingDevelopment.ErrorMessage = e.Message;
                #endregion
            }

            return oTrainingDevelopment;
        }

        public TrainingDevelopment Get(string sSQL, Int64 nUserId)
        {
            TrainingDevelopment oTrainingDevelopment = new TrainingDevelopment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TrainingDevelopmentDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTrainingDevelopment = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get TrainingDevelopment", e);
                oTrainingDevelopment.ErrorMessage = e.Message;
                #endregion
            }

            return oTrainingDevelopment;
        }

        public List<TrainingDevelopment> Gets(Int64 nUserID)
        {
            List<TrainingDevelopment> oTrainingDevelopment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TrainingDevelopmentDA.Gets(tc);
                oTrainingDevelopment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TrainingDevelopment", e);
                #endregion
            }
            return oTrainingDevelopment;
        }

        public List<TrainingDevelopment> Gets(string sSQL, Int64 nUserID)
        {
            List<TrainingDevelopment> oTrainingDevelopment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TrainingDevelopmentDA.Gets(sSQL, tc);
                oTrainingDevelopment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TrainingDevelopment", e);
                #endregion
            }
            return oTrainingDevelopment;
        }

        #endregion
        #region Activity
        public TrainingDevelopment Activite(int EmpID, int nTDID, bool Active, Int64 nUserId)
        {
            TrainingDevelopment oTrainingDevelopment = new TrainingDevelopment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TrainingDevelopmentDA.Activity(EmpID, nTDID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTrainingDevelopment = CreateObject(oReader);
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
                oTrainingDevelopment.ErrorMessage = e.Message;
                #endregion
            }

            return oTrainingDevelopment;
        }


        #endregion

    }
}
