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
    public class EmployeeNomineeService : MarshalByRefObject, IEmployeeNomineeService
    {
        #region Private functions and declaration
        private EmployeeNominee MapObject(NullHandler oReader)
        {
            EmployeeNominee oER = new EmployeeNominee();
            oER.ENID = oReader.GetInt32("ENID");
            oER.EmployeeID = oReader.GetInt32("EmployeeID");
            oER.Name = oReader.GetString("Name");
            oER.Address = oReader.GetString("Address");
            oER.ContactNo = oReader.GetString("ContactNo");
            oER.Email = oReader.GetString("Email");
            oER.Relation = oReader.GetString("Relation");
            oER.Percentage = oReader.GetDouble("Percentage");
       
            return oER;
        }
        private EmployeeNominee CreateObject(NullHandler oReader)
        {
            EmployeeNominee oEmployeeNominee = new EmployeeNominee();
            oEmployeeNominee = MapObject(oReader);
            return oEmployeeNominee;
        }
        private List<EmployeeNominee> CreateObjects(IDataReader oReader)
        {
            List<EmployeeNominee> oEmployeeNominee = new List<EmployeeNominee>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeNominee oItem = CreateObject(oHandler);
                oEmployeeNominee.Add(oItem);
            }
            return oEmployeeNominee;
        }
        #endregion

        #region Interface implementation
        public EmployeeNomineeService() { }

        public EmployeeNominee IUD(EmployeeNominee oER, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeNomineeDA.IUD(tc, oER, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oER = new EmployeeNominee();
                    oER = CreateObject(oReader);
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
                oER.ErrorMessage = e.Message;
                //throw new ServiceException("Failed to Save EmployeeNominee. Because of " + e.Message, e);
                #endregion
            }
            return oER;
        }

        public EmployeeNominee Get(int id, Int64 nUserId) //EmployeeNomineeID
        {
            EmployeeNominee oER = new EmployeeNominee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeNomineeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oER = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeNominee", e);
                #endregion
            }

            return oER;
        }

        public List<EmployeeNominee> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<EmployeeNominee> oEmployeeNominee = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeNomineeDA.Gets(tc, nEmployeeID);
                oEmployeeNominee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeNominee", e);
                #endregion
            }

            return oEmployeeNominee;
        }
        #endregion
    }
}