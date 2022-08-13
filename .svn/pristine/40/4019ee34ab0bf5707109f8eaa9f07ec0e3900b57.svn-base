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
    public class PerformanceIncentiveEvaluationService : MarshalByRefObject, IPerformanceIncentiveEvaluationService
    {
        #region Private functions and declaration
        private PerformanceIncentiveEvaluation MapObject(NullHandler oReader)
        {
            PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation = new PerformanceIncentiveEvaluation();

            oPerformanceIncentiveEvaluation.PIEvaluationID = oReader.GetInt32("PIEvaluationID");
            oPerformanceIncentiveEvaluation.PIMemberID = oReader.GetInt32("PIMemberID");
            oPerformanceIncentiveEvaluation.Point = oReader.GetDouble("Point");
            oPerformanceIncentiveEvaluation.Amount = oReader.GetDouble("Amount");
            oPerformanceIncentiveEvaluation.MonthID = (EnumMonth)oReader.GetInt16("MonthID");
            oPerformanceIncentiveEvaluation.Year = oReader.GetInt32("Year");
            oPerformanceIncentiveEvaluation.ApproveBy = oReader.GetInt32("ApproveBy");
            oPerformanceIncentiveEvaluation.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oPerformanceIncentiveEvaluation.EncryptPIMemberID = Global.Encrypt(oPerformanceIncentiveEvaluation.PIMemberID.ToString());
            oPerformanceIncentiveEvaluation.ApproveByName = oReader.GetString("ApproveByName");
            oPerformanceIncentiveEvaluation.EmployeeName = oReader.GetString("EmployeeName");
            oPerformanceIncentiveEvaluation.EmployeeCode = oReader.GetString("EmployeeCode");

            return oPerformanceIncentiveEvaluation;

        }

        private PerformanceIncentiveEvaluation CreateObject(NullHandler oReader)
        {
            PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation = MapObject(oReader);
            return oPerformanceIncentiveEvaluation;
        }

        private List<PerformanceIncentiveEvaluation> CreateObjects(IDataReader oReader)
        {
            List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations = new List<PerformanceIncentiveEvaluation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PerformanceIncentiveEvaluation oItem = CreateObject(oHandler);
                oPerformanceIncentiveEvaluations.Add(oItem);
            }
            return oPerformanceIncentiveEvaluations;
        }

        #endregion

        #region Interface implementation
        public PerformanceIncentiveEvaluationService() { }

        public PerformanceIncentiveEvaluation IUD(PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PerformanceIncentiveEvaluationDA.IUD(tc, oPerformanceIncentiveEvaluation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oPerformanceIncentiveEvaluation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oPerformanceIncentiveEvaluation.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPerformanceIncentiveEvaluation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oPerformanceIncentiveEvaluation.PIEvaluationID = 0;
                #endregion
            }
            return oPerformanceIncentiveEvaluation;
        }


        public PerformanceIncentiveEvaluation Get(int nPIID, Int64 nUserId)
        {
            PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation = new PerformanceIncentiveEvaluation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveEvaluationDA.Get(nPIID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveEvaluation = CreateObject(oReader);
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

                oPerformanceIncentiveEvaluation.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveEvaluation;
        }

        public PerformanceIncentiveEvaluation Get(string sSQL, Int64 nUserId)
        {
            PerformanceIncentiveEvaluation oPerformanceIncentiveEvaluation = new PerformanceIncentiveEvaluation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveEvaluationDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveEvaluation = CreateObject(oReader);
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

                oPerformanceIncentiveEvaluation.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveEvaluation;
        }

        public List<PerformanceIncentiveEvaluation> Gets(Int64 nUserID)
        {
            List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveEvaluationDA.Gets(tc);
                oPerformanceIncentiveEvaluation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentiveEvaluation", e);
                #endregion
            }
            return oPerformanceIncentiveEvaluation;
        }

        public List<PerformanceIncentiveEvaluation> Gets(string sSQL, Int64 nUserID)
        {
            List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveEvaluationDA.Gets(sSQL, tc);
                oPerformanceIncentiveEvaluation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentiveEvaluation", e);
                #endregion
            }
            return oPerformanceIncentiveEvaluation;
        }


        #region Approve
        public List<PerformanceIncentiveEvaluation> Approve(string sPIEIDs, Int64 nUserId)
        {
            List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations = new List<PerformanceIncentiveEvaluation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PerformanceIncentiveEvaluationDA.Approve(sPIEIDs, nUserId, tc);
                oPerformanceIncentiveEvaluations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                //oPerformanceIncentiveEvaluation.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveEvaluations;
        }


        #endregion

        #region UploadXL
        public List<PerformanceIncentiveEvaluation> UploadPIEXL(List<PerformanceIncentiveEvaluation> oPerformanceIncentiveEvaluations, Int64 nUserID)
        {
            PerformanceIncentiveEvaluation oTempPIE = new PerformanceIncentiveEvaluation();
            List<PerformanceIncentiveEvaluation> oPIEs = new List<PerformanceIncentiveEvaluation>();
            TransactionContext tc = null;
            try
            {
                foreach (PerformanceIncentiveEvaluation oItem in oPerformanceIncentiveEvaluations)
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    oTempPIE = new PerformanceIncentiveEvaluation();
                    reader = PerformanceIncentiveEvaluationDA.Upload_PIEvaluation(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempPIE = CreateObject(oReader);
                    }
                    if (oTempPIE.PIEvaluationID > 0) { oPIEs.Add(oTempPIE); }
                    reader.Close();
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new ServiceException(e.Message.Split('!')[0]);
                #endregion
            }
            return oPIEs;
        }
        #endregion UploadXl

        #endregion


    }
}
