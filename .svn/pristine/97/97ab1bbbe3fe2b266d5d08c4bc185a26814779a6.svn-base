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
    public class EmployeeSalaryDetailDisciplinaryActionService : MarshalByRefObject, IEmployeeSalaryDetailDisciplinaryActionService
    {
        #region Private functions and declaration
        private EmployeeSalaryDetailDisciplinaryAction MapObject(NullHandler oReader)
        {
            EmployeeSalaryDetailDisciplinaryAction oEmployeeSalaryDetailDisciplinaryAction = new EmployeeSalaryDetailDisciplinaryAction();
            oEmployeeSalaryDetailDisciplinaryAction.ESDDAID = oReader.GetInt32("ESDDAID");
            oEmployeeSalaryDetailDisciplinaryAction.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSalaryDetailDisciplinaryAction.DisciplinaryActionID = oReader.GetInt32("DisciplinaryActionID");
            oEmployeeSalaryDetailDisciplinaryAction.ActionName = oReader.GetString("ActionName");
            oEmployeeSalaryDetailDisciplinaryAction.Amount = oReader.GetDouble("Amount");
            oEmployeeSalaryDetailDisciplinaryAction.Note = oReader.GetString("Note");
            
            return oEmployeeSalaryDetailDisciplinaryAction;
        }

        private EmployeeSalaryDetailDisciplinaryAction CreateObject(NullHandler oReader)
        {
            EmployeeSalaryDetailDisciplinaryAction oEmployeeSalaryDetailDisciplinaryAction = MapObject(oReader);
            return oEmployeeSalaryDetailDisciplinaryAction;
        }

        private List<EmployeeSalaryDetailDisciplinaryAction> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalaryDetailDisciplinaryAction> oEmployeeSalaryDetailDisciplinaryAction = new List<EmployeeSalaryDetailDisciplinaryAction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalaryDetailDisciplinaryAction oItem = CreateObject(oHandler);
                oEmployeeSalaryDetailDisciplinaryAction.Add(oItem);
            }
            return oEmployeeSalaryDetailDisciplinaryAction;
        }

        #endregion

        #region Interface implementation
        public EmployeeSalaryDetailDisciplinaryActionService() { }

        public EmployeeSalaryDetailDisciplinaryAction IUD(EmployeeSalaryDetailDisciplinaryAction oEmployeeSalaryDetailDisciplinaryAction, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryDetailDisciplinaryActionDA.IUD(tc, oEmployeeSalaryDetailDisciplinaryAction, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeSalaryDetailDisciplinaryAction = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSalaryDetailDisciplinaryAction.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSalaryDetailDisciplinaryAction.ESDDAID = 0;
                #endregion
            }
            return oEmployeeSalaryDetailDisciplinaryAction;
        }


        public EmployeeSalaryDetailDisciplinaryAction Get(int nESDDAID, Int64 nUserId)
        {
            EmployeeSalaryDetailDisciplinaryAction oEmployeeSalaryDetailDisciplinaryAction = new EmployeeSalaryDetailDisciplinaryAction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryDetailDisciplinaryActionDA.Get(nESDDAID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryDetailDisciplinaryAction = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryDetailDisciplinaryAction", e);
                oEmployeeSalaryDetailDisciplinaryAction.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryDetailDisciplinaryAction;
        }

        public EmployeeSalaryDetailDisciplinaryAction Get(string sSql, Int64 nUserId)
        {
            EmployeeSalaryDetailDisciplinaryAction oEmployeeSalaryDetailDisciplinaryAction = new EmployeeSalaryDetailDisciplinaryAction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryDetailDisciplinaryActionDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryDetailDisciplinaryAction = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryDetailDisciplinaryAction", e);
                oEmployeeSalaryDetailDisciplinaryAction.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryDetailDisciplinaryAction;
        }

        public List<EmployeeSalaryDetailDisciplinaryAction> Gets(Int64 nUserID)
        {
            List<EmployeeSalaryDetailDisciplinaryAction> oEmployeeSalaryDetailDisciplinaryAction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDetailDisciplinaryActionDA.Gets(tc);
                oEmployeeSalaryDetailDisciplinaryAction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSalaryDetailDisciplinaryAction", e);
                #endregion
            }
            return oEmployeeSalaryDetailDisciplinaryAction;
        }

        public List<EmployeeSalaryDetailDisciplinaryAction> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalaryDetailDisciplinaryAction> oEmployeeSalaryDetailDisciplinaryAction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDetailDisciplinaryActionDA.Gets(sSQL, tc);
                oEmployeeSalaryDetailDisciplinaryAction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSalaryDetailDisciplinaryAction", e);
                #endregion
            }
            return oEmployeeSalaryDetailDisciplinaryAction;
        }


        #endregion

    }
}
