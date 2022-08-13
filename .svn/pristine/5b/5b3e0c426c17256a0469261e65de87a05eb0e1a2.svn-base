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
    public class EmployeeReportingPersonService : MarshalByRefObject, IEmployeeReportingPersonService
    {
        #region Private functions and declaration
        private EmployeeReportingPerson MapObject(NullHandler oReader)
        {
            EmployeeReportingPerson oEmployeeReportingPerson = new EmployeeReportingPerson();
            oEmployeeReportingPerson.ERPID = oReader.GetInt32("ERPID");
            oEmployeeReportingPerson.RPID = oReader.GetInt32("RPID");
            oEmployeeReportingPerson.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeReportingPerson.StartDate = oReader.GetDateTime("StartDate");
            oEmployeeReportingPerson.IsActive = oReader.GetBoolean("IsActive");
            oEmployeeReportingPerson.EndDate = oReader.GetDateTime("EndDate");
            oEmployeeReportingPerson.ReportingPersonName = oReader.GetString("ReportingPersonName");
            return oEmployeeReportingPerson;
        }

        private EmployeeReportingPerson CreateObject(NullHandler oReader)
        {
            EmployeeReportingPerson oEmployeeReportingPerson = MapObject(oReader);
            return oEmployeeReportingPerson;
        }

        private List<EmployeeReportingPerson> CreateObjects(IDataReader oReader)
        {
            List<EmployeeReportingPerson> oEmployeeReportingPerson = new List<EmployeeReportingPerson>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeReportingPerson oItem = CreateObject(oHandler);
                oEmployeeReportingPerson.Add(oItem);
            }
            return oEmployeeReportingPerson;
        }

        #endregion

        #region Interface implementation
        public EmployeeReportingPersonService() { }

        public List<EmployeeReportingPerson> GetHierarchy(string sEmployeeIDs, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<EmployeeReportingPerson> oEmployeeReportingPersons = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeReportingPersonDA.GetHierarchy(tc, sEmployeeIDs);
                oEmployeeReportingPersons = CreateObjects(reader);
                reader.Close();

                reader.Close();
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Reporting Person", e);
                #endregion
            }
            return oEmployeeReportingPersons;
        }
        public EmployeeReportingPerson IUD(EmployeeReportingPerson oEmployeeReportingPerson, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = EmployeeReportingPersonDA.IUD(tc, oEmployeeReportingPerson, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployeeReportingPerson = new EmployeeReportingPerson();
                        oEmployeeReportingPerson = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = EmployeeReportingPersonDA.IUD(tc, oEmployeeReportingPerson, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oEmployeeReportingPerson.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oEmployeeReportingPerson = new EmployeeReportingPerson();
                oEmployeeReportingPerson.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oEmployeeReportingPerson;
        }


        public EmployeeReportingPerson Get(int nERPID, Int64 nUserId)
        {
            EmployeeReportingPerson oEmployeeReportingPerson = new EmployeeReportingPerson();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeReportingPersonDA.Get(tc, nERPID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeReportingPerson = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oEmployeeReportingPerson = new EmployeeReportingPerson();
                oEmployeeReportingPerson.ErrorMessage = ex.Message;
                #endregion
            }

            return oEmployeeReportingPerson;
        }

        public List<EmployeeReportingPerson> Gets(int nEmpID, Int64 nUserID)
        {
            List<EmployeeReportingPerson> oEmployeeReportingPersons = new List<EmployeeReportingPerson>();
            EmployeeReportingPerson oEmployeeReportingPerson = new EmployeeReportingPerson();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeReportingPersonDA.Gets(tc, nEmpID);
                oEmployeeReportingPersons = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oEmployeeReportingPerson.ErrorMessage = ex.Message;
                oEmployeeReportingPersons.Add(oEmployeeReportingPerson);
                #endregion
            }

            return oEmployeeReportingPersons;
        }

        public List<EmployeeReportingPerson> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeReportingPerson> oEmployeeReportingPersons = new List<EmployeeReportingPerson>();
            EmployeeReportingPerson oEmployeeReportingPerson = new EmployeeReportingPerson();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeReportingPersonDA.Gets(tc, sSQL);
                oEmployeeReportingPersons = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oEmployeeReportingPerson.ErrorMessage = ex.Message;
                oEmployeeReportingPersons.Add(oEmployeeReportingPerson);
                #endregion
            }

            return oEmployeeReportingPersons;
        }

        #endregion
    }
}
