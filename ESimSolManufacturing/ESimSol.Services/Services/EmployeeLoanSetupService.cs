using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class EmployeeLoanSetupService : MarshalByRefObject, IEmployeeLoanSetupService
    {
        #region Private functions and declaration
        private EmployeeLoanSetup MapObject(NullHandler oReader)
        {
            EmployeeLoanSetup oEmployeeLoanSetup = new EmployeeLoanSetup();
            oEmployeeLoanSetup.ELSID = oReader.GetInt32("ELSID");
            oEmployeeLoanSetup.DeferredDay = oReader.GetInt32("DeferredDay");
            oEmployeeLoanSetup.ActivationAfter = (EnumRecruitmentEvent)oReader.GetInt16("ActivationAfter");
            oEmployeeLoanSetup.LimitInPercentOfPF = oReader.GetDouble("LimitInPercentOfPF");
            oEmployeeLoanSetup.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeLoanSetup.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeLoanSetup.InactiveBy = oReader.GetInt32("InactiveBy");
            oEmployeeLoanSetup.InactiveDate = oReader.GetDateTime("InactiveDate");
            oEmployeeLoanSetup.IsEmployeeContribution = oReader.GetBoolean("IsEmployeeContribution");
            oEmployeeLoanSetup.IsCompanyContribution = oReader.GetBoolean("IsCompanyContribution");
            oEmployeeLoanSetup.IsPFProfit = oReader.GetBoolean("IsPFProfit");
            oEmployeeLoanSetup.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeLoanSetup.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeLoanSetup.InactiveByName = oReader.GetString("InactiveByName");
            oEmployeeLoanSetup.SalaryHeadName = oReader.GetString("SalaryHeadName");
            return oEmployeeLoanSetup;
        }

        public static EmployeeLoanSetup CreateObject(NullHandler oReader)
        {
            EmployeeLoanSetup oEmployeeLoanSetup = new EmployeeLoanSetup();
            EmployeeLoanSetupService oService = new EmployeeLoanSetupService();
            oEmployeeLoanSetup = oService.MapObject(oReader);
            return oEmployeeLoanSetup;
        }
        private List<EmployeeLoanSetup> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanSetup> oEmployeeLoanSetups = new List<EmployeeLoanSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanSetup oItem = CreateObject(oHandler);
                oEmployeeLoanSetups.Add(oItem);
            }
            return oEmployeeLoanSetups;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanSetupService() { }

        public EmployeeLoanSetup IUD(EmployeeLoanSetup oEmployeeLoanSetup, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeLoanSetupDA.IUD(tc, oEmployeeLoanSetup, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanSetup = new EmployeeLoanSetup();
                    oEmployeeLoanSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oEmployeeLoanSetup = new EmployeeLoanSetup(); oEmployeeLoanSetup.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oEmployeeLoanSetup.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oEmployeeLoanSetup;
        }

        public EmployeeLoanSetup Get(int nELSID, long nUserID)
        {
            EmployeeLoanSetup oEmployeeLoanSetup = new EmployeeLoanSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanSetupDA.Get(tc, nELSID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oEmployeeLoanSetup.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanSetup;
        }

        public List<EmployeeLoanSetup> Gets(string sSQL, long nUserID)
        {
            List<EmployeeLoanSetup> oEmployeeLoanSetups = new List<EmployeeLoanSetup>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanSetupDA.Gets(tc, sSQL);
                oEmployeeLoanSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLoanSetup oEmployeeLoanSetup = new EmployeeLoanSetup();
                oEmployeeLoanSetup.ErrorMessage = e.Message;
                oEmployeeLoanSetups.Add(oEmployeeLoanSetup);
                #endregion
            }

            return oEmployeeLoanSetups;
        }


        #endregion
    }
}