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
    public class EmployeeAdvanceSalaryProcessService : MarshalByRefObject, IEmployeeAdvanceSalaryProcessService
    {
        #region Private functions and declaration
        private EmployeeAdvanceSalaryProcess MapObject(NullHandler oReader)
        {
            EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();

            oEmployeeAdvanceSalaryProcess.EASPID = oReader.GetInt32("EASPID");
            oEmployeeAdvanceSalaryProcess.BUID = oReader.GetInt32("BUID");
            oEmployeeAdvanceSalaryProcess.LocationID = oReader.GetInt32("LocationID");
            oEmployeeAdvanceSalaryProcess.Description = oReader.GetString("Description");
            oEmployeeAdvanceSalaryProcess.StartDate = oReader.GetDateTime("StartDate");
            oEmployeeAdvanceSalaryProcess.EndDate = oReader.GetDateTime("EndDate");
            oEmployeeAdvanceSalaryProcess.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeAdvanceSalaryProcess.NYear = oReader.GetInt32("NYear");
            oEmployeeAdvanceSalaryProcess.NMonth = oReader.GetInt32("NMonth");
            oEmployeeAdvanceSalaryProcess.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeAdvanceSalaryProcess.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeAdvanceSalaryProcess.BUName = oReader.GetString("BUName");
            oEmployeeAdvanceSalaryProcess.LocationName = oReader.GetString("LocationName");
            oEmployeeAdvanceSalaryProcess.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeAdvanceSalaryProcess.DeclarationDate = oReader.GetDateTime("DeclarationDate");

            return oEmployeeAdvanceSalaryProcess;
        }
        private EmployeeAdvanceSalaryProcess CreateObject(NullHandler oReader)
        {
            EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            oEmployeeAdvanceSalaryProcess = MapObject(oReader);
            return oEmployeeAdvanceSalaryProcess;
        }
        private List<EmployeeAdvanceSalaryProcess> CreateObjects(IDataReader oReader)
        {
            List<EmployeeAdvanceSalaryProcess> oEmployeeAdvanceSalaryProcess = new List<EmployeeAdvanceSalaryProcess>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeAdvanceSalaryProcess oItem = CreateObject(oHandler);
                oEmployeeAdvanceSalaryProcess.Add(oItem);
            }
            return oEmployeeAdvanceSalaryProcess;
        }
        #endregion

        #region Interface implementation


        public EmployeeAdvanceSalaryProcess Save(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, int nUserId)
        {
            int nIndex = 0;
            int nNewIndex = 1;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeAdvanceSalaryProcessDA.InsertUpdate(tc, oEmployeeAdvanceSalaryProcess, EnumDBOperation.Insert, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                    oEmployeeAdvanceSalaryProcess = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

          

                if (oEmployeeAdvanceSalaryProcess.EASPID > 0)
                {
                    while (nNewIndex != 0)
                    {
                        tc = TransactionContext.Begin(true);
                        nNewIndex = EmployeeAdvanceSalaryDA.EmployeeAdvanceSalarySave(tc, nIndex, oEmployeeAdvanceSalaryProcess.EASPID, nUserId);
                        nIndex = nNewIndex;
                        tc.End();
                    }
                }
              
                //IDataReader reader;
                //if (oEmployeeAdvanceSalaryProcess.EASPID <= 0)
                //{
                //    reader = EmployeeAdvanceSalaryProcessDA.InsertUpdate(tc, oEmployeeAdvanceSalaryProcess, EnumDBOperation.Insert, nUserId);
                //}
                //else
                //{
                //    reader = EmployeeAdvanceSalaryProcessDA.InsertUpdate(tc, oEmployeeAdvanceSalaryProcess, EnumDBOperation.Update, nUserId);
                //}
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                //    oEmployeeAdvanceSalaryProcess = CreateObject(oReader);
                //}
                //reader.Close();
                //tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to save EmployeeAdvanceSalaryProcess " + e.Message, e);
                #endregion
            }
            return oEmployeeAdvanceSalaryProcess;
        }


        public string Delete(int id, int nUserId)
        {
            string message = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                oEmployeeAdvanceSalaryProcess.EASPID = id;
                EmployeeAdvanceSalaryProcessDA.Delete(tc, oEmployeeAdvanceSalaryProcess, EnumDBOperation.Delete, nUserId);
                tc.End();
                message = Global.DeleteMessage;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                message = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return message;
        }


        public EmployeeAdvanceSalaryProcess Get(string sSQL, Int64 nUserId)
        {
            EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeAdvanceSalaryProcessDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeAdvanceSalaryProcess = CreateObject(oReader);
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

            return oEmployeeAdvanceSalaryProcess;
        }


        public List<EmployeeAdvanceSalaryProcess> Gets(string sSQL, int nUserId)
        {
            List<EmployeeAdvanceSalaryProcess> oEmployeeAdvanceSalaryProcess = new List<EmployeeAdvanceSalaryProcess>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeAdvanceSalaryProcessDA.Gets(tc, sSQL);
                oEmployeeAdvanceSalaryProcess = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeAdvanceSalaryProcess", e);
                #endregion
            }

            return oEmployeeAdvanceSalaryProcess;
        }


        public List<EmployeeAdvanceSalaryProcess> Gets(int nUserId)
        {
            List<EmployeeAdvanceSalaryProcess> oEmployeeAdvanceSalaryProcess = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeAdvanceSalaryProcessDA.Gets(tc);
                oEmployeeAdvanceSalaryProcess = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeAdvanceSalaryProcess ", e);
                #endregion
            }

            return oEmployeeAdvanceSalaryProcess;
        }


        public EmployeeAdvanceSalaryProcess Approve(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeAdvanceSalaryProcess.ApproveBy <= 0)
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = EmployeeAdvanceSalaryProcessDA.InsertUpdate(tc, oEmployeeAdvanceSalaryProcess, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = EmployeeAdvanceSalaryProcessDA.InsertUpdate(tc, oEmployeeAdvanceSalaryProcess, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                    oEmployeeAdvanceSalaryProcess = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                oEmployeeAdvanceSalaryProcess.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oEmployeeAdvanceSalaryProcess;
        }

        public EmployeeAdvanceSalaryProcess UndoApprove(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeAdvanceSalaryProcessDA.InsertUpdate(tc, oEmployeeAdvanceSalaryProcess, EnumDBOperation.UnApproval, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                    oEmployeeAdvanceSalaryProcess = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                oEmployeeAdvanceSalaryProcess.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oEmployeeAdvanceSalaryProcess;
        }

        public EmployeeAdvanceSalaryProcess Get(int id, long nUserId)
        {
            EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeAdvanceSalaryProcessDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeAdvanceSalaryProcess = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Employee Advance Salary Process", e);
                #endregion
            }

            return oEmployeeAdvanceSalaryProcess;
        }


        #endregion

       
    }
}

