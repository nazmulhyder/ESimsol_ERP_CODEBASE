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
    public class EmployeeLoanInstallmentService : MarshalByRefObject, IEmployeeLoanInstallmentService
    {
        #region Private functions and declaration
        private EmployeeLoanInstallment MapObject(NullHandler oReader)
        {
            EmployeeLoanInstallment oEmployeeLoanInstallment = new EmployeeLoanInstallment();
            oEmployeeLoanInstallment.ELInstallmentID = oReader.GetInt32("ELInstallmentID");
            oEmployeeLoanInstallment.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oEmployeeLoanInstallment.InstallmentAmount = oReader.GetDouble("InstallmentAmount");
            oEmployeeLoanInstallment.InterestPerInstallment = oReader.GetDouble("InterestPerInstallment");
            oEmployeeLoanInstallment.InstallmentDate = oReader.GetDateTime("InstallmentDate");
            oEmployeeLoanInstallment.ESDetailID = oReader.GetInt32("ESDetailID");
            return oEmployeeLoanInstallment;
        }

        public static EmployeeLoanInstallment CreateObject(NullHandler oReader)
        {
            EmployeeLoanInstallment oEmployeeLoanInstallment = new EmployeeLoanInstallment();
            EmployeeLoanInstallmentService oService = new EmployeeLoanInstallmentService();
            oEmployeeLoanInstallment = oService.MapObject(oReader);
            return oEmployeeLoanInstallment;
        }
        private List<EmployeeLoanInstallment> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanInstallment> oEmployeeLoanInstallments = new List<EmployeeLoanInstallment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanInstallment oItem = CreateObject(oHandler);
                oEmployeeLoanInstallments.Add(oItem);
            }
            return oEmployeeLoanInstallments;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanInstallmentService() { }

        public EmployeeLoanInstallment IUD(EmployeeLoanInstallment oEmployeeLoanInstallment, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeLoanInstallmentDA.IUD(tc, oEmployeeLoanInstallment, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanInstallment = new EmployeeLoanInstallment();
                    oEmployeeLoanInstallment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oEmployeeLoanInstallment = new EmployeeLoanInstallment(); oEmployeeLoanInstallment.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oEmployeeLoanInstallment.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oEmployeeLoanInstallment;
        }

        public EmployeeLoanInstallment Get(int nEmployeeLoanInstallmentID, long nUserID)
        {
            EmployeeLoanInstallment oEmployeeLoanInstallment = new EmployeeLoanInstallment();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanInstallmentDA.Get(tc, nEmployeeLoanInstallmentID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanInstallment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oEmployeeLoanInstallment.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanInstallment;
        }

        public List<EmployeeLoanInstallment> Gets(string sSQL, long nUserID)
        {
            List<EmployeeLoanInstallment> oEmployeeLoanInstallments = new List<EmployeeLoanInstallment>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanInstallmentDA.Gets(tc, sSQL);
                oEmployeeLoanInstallments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLoanInstallment oEmployeeLoanInstallment = new EmployeeLoanInstallment();
                oEmployeeLoanInstallment.ErrorMessage = e.Message;
                oEmployeeLoanInstallments.Add(oEmployeeLoanInstallment);
                #endregion
            }

            return oEmployeeLoanInstallments;
        }


        #endregion
    }
}