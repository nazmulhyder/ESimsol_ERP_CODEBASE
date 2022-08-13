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
    public class ELEncashComplianceService : MarshalByRefObject, IELEncashComplianceService
    {
        #region Private functions and declaration
        private ELEncashCompliance MapObject(NullHandler oReader)
        {
            ELEncashCompliance oELEncashCompliance = new ELEncashCompliance();
            oELEncashCompliance.ELEncashCompID = oReader.GetInt32("ELEncashCompID");
            oELEncashCompliance.DeclarationDate = oReader.GetDateTime("DeclarationDate");
            oELEncashCompliance.StartDate = oReader.GetDateTime("StartDate");
            oELEncashCompliance.EndDate = oReader.GetDateTime("EndDate");
            oELEncashCompliance.BUID = oReader.GetInt32("BUID");
            oELEncashCompliance.LocationID = oReader.GetInt32("LocationID");
            oELEncashCompliance.Note = oReader.GetString("Note");
            oELEncashCompliance.ConsiderELCount = oReader.GetInt32("ConsiderELCount");
            oELEncashCompliance.ApproveBy = oReader.GetInt32("ApproveBy");
            oELEncashCompliance.ApproveDate = oReader.GetDateTime("ApproveDate");
            oELEncashCompliance.BUName = oReader.GetString("BUName");
            oELEncashCompliance.LocationName = oReader.GetString("LocationName");
            oELEncashCompliance.ApproveByName = oReader.GetString("ApproveByName");

            return oELEncashCompliance;
        }
        private ELEncashCompliance CreateObject(NullHandler oReader)
        {
            ELEncashCompliance oELEncashCompliance = new ELEncashCompliance();
            oELEncashCompliance = MapObject(oReader);
            return oELEncashCompliance;
        }
        private List<ELEncashCompliance> CreateObjects(IDataReader oReader)
        {
            List<ELEncashCompliance> oELEncashCompliance = new List<ELEncashCompliance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ELEncashCompliance oItem = CreateObject(oHandler);
                oELEncashCompliance.Add(oItem);
            }
            return oELEncashCompliance;
        }
        #endregion

        #region Interface implementation


        public ELEncashCompliance Save(ELEncashCompliance oELEncashCompliance, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ELEncashComplianceDA.InsertUpdate(tc, oELEncashCompliance, EnumDBOperation.Insert, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELEncashCompliance = new ELEncashCompliance();
                    oELEncashCompliance = CreateObject(oReader);
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
                throw new ServiceException("Failed to save ELEncashCompliance " + e.Message, e);
                #endregion
            }
            return oELEncashCompliance;
        }

        public ELEncashCompliance Approve(ELEncashCompliance oELEncashCompliance, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = ELEncashComplianceDA.InsertUpdate(tc, oELEncashCompliance, EnumDBOperation.Approval, nUserID);
               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELEncashCompliance = new ELEncashCompliance();
                    oELEncashCompliance = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oELEncashCompliance = new ELEncashCompliance();
                oELEncashCompliance.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oELEncashCompliance;
        }
        public int ELEncashComplianceSave(int nIndex, int ELEncashComplianceID, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = ELEncashComplianceDA.ELEncashComplianceSave(tc, nIndex, ELEncashComplianceID, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                nNewIndex = 0;
                throw new ServiceException(e.Message);
                #endregion
            }
            return nNewIndex;
        }



        public ELEncashCompliance Delete(ELEncashCompliance oELEncashCompliance, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = ELEncashComplianceDA.InsertUpdate(tc, oELEncashCompliance, EnumDBOperation.Delete, nUserID);

                NullHandler oReader = new NullHandler(reader);
                
                reader.Close();
                oELEncashCompliance.ErrorMessage = "Deleted Successfully.";



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oELEncashCompliance = new ELEncashCompliance();
                oELEncashCompliance.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oELEncashCompliance;
        }


        public ELEncashCompliance Get(string sSQL, Int64 nUserId)
        {
            ELEncashCompliance oELEncashCompliance = new ELEncashCompliance();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ELSetupDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELEncashCompliance = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oELEncashCompliance;
        }


        public List<ELEncashCompliance> Gets(string sSQL, int nUserId)
        {
            List<ELEncashCompliance> oELEncashCompliance = new List<ELEncashCompliance>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ELEncashComplianceDA.Gets(tc, sSQL);
                oELEncashCompliance = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeAdvanceSalary", e);
                #endregion
            }

            return oELEncashCompliance;
        }


        public List<ELEncashCompliance> Gets(int nUserId)
        {
            List<ELEncashCompliance> oELEncashCompliance = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ELEncashComplianceDA.Gets(tc);
                oELEncashCompliance = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeAdvanceSalary ", e);
                #endregion
            }

            return oELEncashCompliance;
        }


        #endregion

       
    }
}

