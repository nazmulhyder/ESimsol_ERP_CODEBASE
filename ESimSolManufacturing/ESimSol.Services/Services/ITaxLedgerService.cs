using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Services.Services
{
    public class ITaxLedgerService : MarshalByRefObject, IITaxLedgerService
    {
        #region Private functions and declaration
        private ITaxLedger MapObject(NullHandler oReader)
        {
            ITaxLedger oITaxLedger = new ITaxLedger();

            oITaxLedger.ITaxLedgerID = oReader.GetInt32("ITaxLedgerID");
            oITaxLedger.IsActive = oReader.GetInt32("IsActive");
            oITaxLedger.ITaxRateSchemeID = oReader.GetInt32("ITaxRateSchemeID");
            oITaxLedger.EmployeeID = oReader.GetInt32("EmployeeID");
            oITaxLedger.TaxBySalaryAmount = oReader.GetDouble("TaxBySalaryAmount");
            oITaxLedger.RebateAmount = oReader.GetDouble("RebateAmount");
            oITaxLedger.AdvancePaidAmount = oReader.GetDouble("AdvancePaidAmount");
            oITaxLedger.PaidByPreviousLedger = oReader.GetDouble("PaidByPreviousLedger");
            oITaxLedger.InstallmentAmount = oReader.GetDouble("InstallmentAmount");
            oITaxLedger.TaxableAmount = oReader.GetDouble("TaxableAmount");
            oITaxLedger.AssessmentYear = oReader.GetString("AssessmentYear");
            oITaxLedger.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oITaxLedger.TotalTax = oReader.GetDouble("TotalTax");
            oITaxLedger.DepartmentName = oReader.GetString("DepartmentName");
            oITaxLedger.DesignationName = oReader.GetString("DesignationName");
            return oITaxLedger;

        }

        private ITaxLedger CreateObject(NullHandler oReader)
        {
            ITaxLedger oITaxLedger = MapObject(oReader);
            return oITaxLedger;
        }

        private List<ITaxLedger> CreateObjects(IDataReader oReader)
        {
            List<ITaxLedger> oITaxLedgers = new List<ITaxLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxLedger oItem = CreateObject(oHandler);
                oITaxLedgers.Add(oItem);
            }
            return oITaxLedgers;
        }

        #endregion

        #region Interface implementation
        public ITaxLedgerService() { }

        public List<ITaxLedger> ITaxProcess(int nEmployeeID, int nITaxAssessmentYearID, bool IsAllEmployee,bool IsConsiderMaxRebate, double MinGross, Int64 nUserID)
        {
            ITaxLedger oITaxLedger = new ITaxLedger();
            List<ITaxLedger> oITaxLedgers = new List<ITaxLedger>();
            TransactionContext tc = null;
            try
            {
                string sEmpID;
                List<Employee> oEmployees = new List<Employee>();
                if (!IsAllEmployee) { sEmpID = nEmployeeID.ToString(); }
                else
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader;
                    reader = EmployeeDA.Gets(tc, "SELECT * FROM EmployeeSalaryStructure WHERE IsActive=1 AND GrossAmount>=" + MinGross + " AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE IsActive=1) ");
                    NullHandler oreader = new NullHandler(reader);
                    while (reader.Read())
                    {
                        Employee oItem = new Employee();
                        oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                        oEmployees.Add(oItem);
                    }
                    reader.Close();
                    tc.End();
                    sEmpID = string.Join(",", oEmployees.Select(x => x.EmployeeID));
                }

                string[] nEmpIDs;
                nEmpIDs = sEmpID.Split(',');

                int nCount = 0;
                foreach (string NEmpID in nEmpIDs)
                {
           
                    tc = TransactionContext.Begin();
                    int NID = Convert.ToInt32(NEmpID);
                    IDataReader reader = ITaxLedgerDA.ITaxProcess(NID, nITaxAssessmentYearID,IsConsiderMaxRebate, tc, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oITaxLedger = CreateObject(oReader);
                    }
                    reader.Close();
                    if (oITaxLedger.ITaxLedgerID > 0) { nCount++;  if (nCount <= 100) { oITaxLedgers.Add(oITaxLedger); }}
                    
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
            return oITaxLedgers;
        }

        public List<ITaxLedger> ITaxReprocess(int nEmployeeID, int nITaxAssessmentYearID, bool IsAllEmployee, bool IsConsiderMaxRebate, double MinGross, DateTime dtDate, Int64 nUserID)
        {
            ITaxLedger oITaxLedger = new ITaxLedger();
            ITaxLedger oITaxLedgerError = new ITaxLedger();
            List<ITaxLedger> oITaxLedgers = new List<ITaxLedger>();
            List<ITaxLedger> oITaxLedgersError = new List<ITaxLedger>();
            TransactionContext tc = null;
            try
            {
                string sEmpID;
                List<Employee> oEmployees = new List<Employee>();
                if (!IsAllEmployee) { sEmpID = nEmployeeID.ToString(); }
                else
                {
                    tc = TransactionContext.Begin();
                    IDataReader reader;
                    reader = EmployeeDA.Gets(tc, "SELECT * FROM EmployeeSalaryStructure WHERE IsActive=1 AND GrossAmount>=" + MinGross + " AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE IsActive=1) ");
                    NullHandler oreader = new NullHandler(reader);
                    while (reader.Read())
                    {
                        Employee oItem = new Employee();
                        oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                        oEmployees.Add(oItem);
                    }
                    reader.Close();
                    tc.End();
                    sEmpID = string.Join(",", oEmployees.Select(x => x.EmployeeID));
                }

                string[] nEmpIDs;
                nEmpIDs = sEmpID.Split(',');

                int nCount = 0;
                foreach (string NEmpID in nEmpIDs)
                {
                    try
                    {
                        tc = TransactionContext.Begin();
                        int NID = Convert.ToInt32(NEmpID);
                        IDataReader reader = ITaxLedgerDA.ITaxReProcess(NID, nITaxAssessmentYearID, IsConsiderMaxRebate, dtDate, tc, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oITaxLedger = CreateObject(oReader);
                        }
                        reader.Close();
                        if (oITaxLedger.ITaxLedgerID > 0) { nCount++; if (nCount <= 100) { oITaxLedgers.Add(oITaxLedger); } }

                        tc.End();
                    }
                    catch (Exception e)
                    {
                        if (tc != null)
                            tc.HandleError();
                        oITaxLedgerError = new ITaxLedger();
                        oITaxLedgerError.EmployeeID =Convert.ToInt32(NEmpID);
                        oITaxLedgerError.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                        oITaxLedgersError.Add(oITaxLedgerError);
                    }
                    
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
            return (oITaxLedgersError.Count > 0) ? oITaxLedgersError : oITaxLedgers;
        }
        public ITaxLedger Get(int nITaxLedgerID, Int64 nUserId)
        {
            ITaxLedger oITaxLedger = new ITaxLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxLedgerDA.Get(nITaxLedgerID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxLedger = CreateObject(oReader);
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

                oITaxLedger.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxLedger;
        }

        public ITaxLedger Get(string sSQL, Int64 nUserId)
        {
            ITaxLedger oITaxLedger = new ITaxLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxLedgerDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxLedger = CreateObject(oReader);
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

                oITaxLedger.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxLedger;
        }

        public List<ITaxLedger> Gets(Int64 nUserID)
        {
            List<ITaxLedger> oITaxLedger = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxLedgerDA.Gets(tc);
                oITaxLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxLedger", e);
                #endregion
            }
            return oITaxLedger;
        }

        public List<ITaxLedger> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxLedger> oITaxLedger = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxLedgerDA.Gets(sSQL, tc);
                oITaxLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxLedger", e);
                #endregion
            }
            return oITaxLedger;
        }

        public ITaxLedger ITaxLedger_Delete(string sITaxLedgerIDs, Int64 nUserId)
        {
            ITaxLedger oITaxLedger = new ITaxLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                ITaxLedgerDA.ITaxLedger_Delete(sITaxLedgerIDs, tc);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                oITaxLedger.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxLedger;
        }
        #endregion
    }
}
