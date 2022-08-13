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
    public class PayrollProcessManagementService : MarshalByRefObject, IPayrollProcessManagementService
    {
        #region Private functions and declaration
        private PayrollProcessManagement MapObject(NullHandler oReader)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            oPayrollProcessManagement.PPMID = oReader.GetInt32("PPMID");
            oPayrollProcessManagement.EmployeeCount = oReader.GetInt32("EmployeeCount");
            oPayrollProcessManagement.DeptCount = oReader.GetInt32("DeptCount");
            oPayrollProcessManagement.SchemeCount = oReader.GetInt32("SchemeCount");
            oPayrollProcessManagement.CompanyID = oReader.GetInt32("CompanyID");
            oPayrollProcessManagement.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oPayrollProcessManagement.LocationID = oReader.GetInt32("LocationID");
            oPayrollProcessManagement.Status = (EnumProcessStatus)oReader.GetInt16("Status");
            oPayrollProcessManagement.PaymentCycle = (EnumPaymentCycle)oReader.GetInt16("PaymentCycle");
            oPayrollProcessManagement.ProcessDate = oReader.GetDateTime("ProcessDate");
            oPayrollProcessManagement.SalaryFrom = oReader.GetDateTime("SalaryFrom");
            oPayrollProcessManagement.SalaryTo = oReader.GetDateTime("SalaryTo");
            //oPayrollProcessManagement.AllowanceIDs = oReader.GetString("AllowanceIDs");
            oPayrollProcessManagement.MonthID = (EnumMonth)oReader.GetInt16("MonthID");
            oPayrollProcessManagement.MonthIDInt = (int)(EnumMonth)oReader.GetInt16("MonthID");
            oPayrollProcessManagement.BankAccountID = oReader.GetInt32("BankAccountID");
            //derive
            oPayrollProcessManagement.LocationName = oReader.GetString("LocationName");
            oPayrollProcessManagement.CompanyNmae = oReader.GetString("CompanyNmae");
            oPayrollProcessManagement.BUName = oReader.GetString("BUName");
            oPayrollProcessManagement.EncryptID = Global.Encrypt(oPayrollProcessManagement.PPMID.ToString());
            

            //Compliance Payroll Process
            oPayrollProcessManagement.MOCID = oReader.GetInt32("MOCID");
            return oPayrollProcessManagement;

        }

        private PayrollProcessManagement CreateObject(NullHandler oReader)
        {
            PayrollProcessManagement oPayrollProcessManagement = MapObject(oReader);
            return oPayrollProcessManagement;
        }

        private List<PayrollProcessManagement> CreateObjects(IDataReader oReader)
        {
            List<PayrollProcessManagement> oPayrollProcessManagement = new List<PayrollProcessManagement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PayrollProcessManagement oItem = CreateObject(oHandler);
                oPayrollProcessManagement.Add(oItem);
            }
            return oPayrollProcessManagement;
        }

        #endregion

        #region Interface implementation
        public PayrollProcessManagementService() { }

        public PayrollProcessManagement IUD(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                if(oPayrollProcessManagement.PPMID>0)
                {
                    VoucherDA.CheckVoucherReference(tc, "PayrollProcessManagement", "PPMID", oPayrollProcessManagement.PPMID);
                }
                IDataReader reader;
                reader = PayrollProcessManagementDA.IUD(tc, oPayrollProcessManagement, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public PayrollProcessManagement IUD_V1(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                 string sDIDs = string.Join(",", oPayrollProcessManagement.DepartmentRequirementPolicys.Select(x => x.DepartmentID));
                 string sSql = "SELECT * FROM PayrollProcessManagementObject WHERE PPMObject=1 AND ObjectID IN(" + sDIDs + ")"
                                    + "AND PPMID IN(SELECT PPMID FROM PayrollProcessManagement WHERE (('" + oPayrollProcessManagement.SalaryFrom.ToString("dd MMM yyyy") + "' BETWEEN SalaryFrom  AND SalaryTo)"
                                    + " OR ('" + oPayrollProcessManagement.SalaryTo.ToString("dd MMM yyyy") + "' BETWEEN SalaryFrom  AND SalaryTo)) AND CompanyID=" + oPayrollProcessManagement.CompanyID + " AND LocationID=" + oPayrollProcessManagement.LocationID + ")";
                tc = TransactionContext.Begin(true);
                bool IsPayrollProcessed = PayrollProcessManagementDA.CheckPayrollProcess(sSql, tc);
                tc.End();

                if (IsPayrollProcessed == true)
                {
                    throw new Exception("Payroll is processed for some department!");
                }

                List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
                List<SalaryScheme> oSalarySchemes = new List<SalaryScheme>();
                List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
                List<EmployeeGroup> oEmployeeGroups = new List<EmployeeGroup>();
                oDepartmentRequirementPolicys= oPayrollProcessManagement.DepartmentRequirementPolicys;
                oSalarySchemes = oPayrollProcessManagement.SalarySchemes;
                oSalaryHeads = oPayrollProcessManagement.SalaryHeads;
                oEmployeeGroups = oPayrollProcessManagement.EmployeeGroups;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PayrollProcessManagementDA.IUD(tc, oPayrollProcessManagement, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
                }
                reader.Close();

                if (oPayrollProcessManagement.PPMID>0)
                {
                   
                    foreach (DepartmentRequirementPolicy oitem in oDepartmentRequirementPolicys)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.Department;
                        oPPMO.ObjectID = oitem.DepartmentID;
                        Reader = PayrollProcessManagementObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (SalaryScheme oitem in oSalarySchemes)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.SalaryScheme;
                        oPPMO.ObjectID = oitem.SalarySchemeID;
                        Reader = PayrollProcessManagementObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (EmployeeGroup oitem in oEmployeeGroups)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.EmployeeGroup;
                        oPPMO.ObjectID = oitem.EmployeeTypeID;
                        Reader = PayrollProcessManagementObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (SalaryHead oitem in oSalaryHeads)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.SalaryHead;
                        oPPMO.ObjectID = oitem.SalaryHeadID;
                        Reader = PayrollProcessManagementObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                   
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public string ProcessPayroll(int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {
            string sMessage = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                PayrollProcessManagementDA.ProcessPayroll(tc, nSalarySchemeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get PayrollProcessManagement", e);
                sMessage = e.Message;
                #endregion
            }
            return sMessage;
        }

        //public string ProcessPayrollByEmployee(int nEmployeeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        //{
        //    string sMessage = "";
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin();
        //        PayrollProcessManagementDA.ProcessPayrollByEmployee(tc, nEmployeeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        ExceptionLog.Write(e);
        //        //throw new ServiceException("Failed to Get PayrollProcessManagement", e);
        //        sMessage = e.Message;
        //        #endregion
        //    }
        //    return sMessage;
        //}
        public int ProcessPayrollByEmployee(int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayrollByEmployee(tc, nIndex, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
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

        public PayrollProcessManagement Delete(int nPPMID, Int64 nUserID)
        {

            TransactionContext tc = null;
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "PayrollProcessManagement", "PPMID", oPayrollProcessManagement.PPMID);
                IDataReader reader;
                reader = PayrollProcessManagementDA.Delete(tc, nPPMID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
                }
                oPayrollProcessManagement.ErrorMessage = "";
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public PayrollProcessManagement Get(int nPPMID, Int64 nUserId)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PayrollProcessManagementDA.Get(nPPMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get PayrollProcessManagement", e);
                oPayrollProcessManagement.ErrorMessage = e.Message;
                #endregion
            }

            return oPayrollProcessManagement;
        }
        
        public int CheckPayrollProcess(string sSql, Int64 nUserID)
        {
            TransactionContext tc = null;
            tc = TransactionContext.Begin(true);
            int nIndex = PayrollProcessManagementDA.CheckPayrollProcessForIndex(sSql, tc);
            tc.End();
            return nIndex;
        }

        public PayrollProcessManagement PPM_Unfreeze(int nPPMID, Int64 nUserId)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PayrollProcessManagementDA.PPM_Unfreeze(nPPMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get PayrollProcessManagement", e);
                oPayrollProcessManagement.ErrorMessage = e.Message;
                #endregion
            }

            return oPayrollProcessManagement;
        }

        public List<PayrollProcessManagement> Gets(Int64 nUserID)
        {
            List<PayrollProcessManagement> oPayrollProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PayrollProcessManagementDA.Gets(tc);
                oPayrollProcessManagement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_PayrollProcessManagement", e);
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public List<PayrollProcessManagement> Gets(string sSQL, Int64 nUserID)
        {
            List<PayrollProcessManagement> oPayrollProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PayrollProcessManagementDA.Gets(sSQL, tc);
                oPayrollProcessManagement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_PayrollProcessManagement", e);
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public int ProcessPayroll_Mamiya(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_Mamiya(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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

        public int ProcessPayroll_Corporate(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_Corporate(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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
        public int ProcessPayroll_Corporate_Discontinue(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_Corporate_Discontinue(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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
        public PayrollProcessManagement CheckPPM(int nEmployeeID, int nMonthID, int nYear, Int64 nUserID)
        {
            PayrollProcessManagement oPayrollProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PayrollProcessManagementDA.CheckPPM(nEmployeeID, nMonthID, nYear, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
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
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPayrollProcessManagement;
        }
        public int ProcessPayroll_Production(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_Production(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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
        public EmployeeSalary SettlementSalaryProcess(int nEmployeeSettlementID, Int64 nUserId)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PayrollProcessManagementDA.SettlementSalaryProcess(nEmployeeSettlementID,nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalary.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
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
                //throw new ServiceException("Failed to Get EmployeeSalary", e);
                oEmployeeSalary.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeSalary;
        }

        public List<EmployeeSalary> SettlementSalaryProcess_Corporate_Multiple(EmployeeSettlement oEmployeeSettlement, Int64 nUserId)
        {

            string[] sEmpSSIDs;
            sEmpSSIDs = oEmployeeSettlement.ErrorMessage.Split(',');

            string[] sEmpCodes;
            sEmpCodes = oEmployeeSettlement.Params.Split(',');

            EmployeeSalary oESS = new EmployeeSalary();
            List<EmployeeSalary> oTempESSs = new List<EmployeeSalary>();
            List<EmployeeSalary> oESSs = new List<EmployeeSalary>();
            List<EmployeeSalary> oTempList = new List<EmployeeSalary>();
            TransactionContext tc = null;
            int nEmployeeSettlementID = 0;
            string Code = "";
            int nCounter = 0;
            foreach (string sESSID in sEmpSSIDs)
            {
                try
                {
                    tc = TransactionContext.Begin(true);

                    oTempESSs = new List<EmployeeSalary>();
                    IDataReader reader = null;

                    int EmployeeSettID = Convert.ToInt32(sESSID);
                    oEmployeeSettlement.EmployeeSettlementID = EmployeeSettID;
                    nEmployeeSettlementID = EmployeeSettID;
                    Code = sEmpCodes[nCounter];
                    reader = PayrollProcessManagementDA.SettlementSalaryProcess_Corporate(nEmployeeSettlementID, nUserId, tc);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oESS.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
                    }
                    reader.Close();
                    tc.End();

                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();
                    EmployeeSalary oItem = new EmployeeSalary();

                    oItem.EmployeeCode = Code;
                    nCounter++;
                    oItem.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oItem);
                }
            }
            return oTempList;


        }

        public EmployeeSalary SettlementSalaryProcess_Corporate(int nEmployeeSettlementID, Int64 nUserId)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PayrollProcessManagementDA.SettlementSalaryProcess_Corporate(nEmployeeSettlementID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalary.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
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
                //throw new ServiceException("Failed to Get EmployeeSalary", e);
                oEmployeeSalary.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeSalary;
        }




        #region Compliance Salary

        public PayrollProcessManagement IUDComp(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                if (oPayrollProcessManagement.PPMID > 0)
                {
                    VoucherDA.CheckVoucherReference(tc, "PayrollProcessManagement", "PPMID", oPayrollProcessManagement.PPMID);
                }
                IDataReader reader;
                reader = PayrollProcessManagementDA.IUDComp(tc, oPayrollProcessManagement, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public PayrollProcessManagement IUD_V1Comp(PayrollProcessManagement oPayrollProcessManagement, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                string sDIDs = string.Join(",", oPayrollProcessManagement.DepartmentRequirementPolicys.Select(x => x.DepartmentID));
                string sSql = "SELECT * FROM CompliancePayrollProcessManagementObject WHERE PPMObject=1 AND ObjectID IN(" + sDIDs + ")"
                                   + "AND PPMID IN(SELECT PPMID FROM CompliancePayrollProcessManagement WHERE (('" + oPayrollProcessManagement.SalaryFrom.ToString("dd MMM yyyy") + "' BETWEEN SalaryFrom  AND SalaryTo) AND MOCID="+oPayrollProcessManagement.MOCID
                                   + " AND ('" + oPayrollProcessManagement.SalaryTo.ToString("dd MMM yyyy") + "' BETWEEN SalaryFrom  AND SalaryTo)) AND CompanyID=" + oPayrollProcessManagement.CompanyID + " AND LocationID=" + oPayrollProcessManagement.LocationID + ")";
                tc = TransactionContext.Begin(true);
                bool IsPayrollProcessed = PayrollProcessManagementDA.CheckPayrollProcessComp(sSql, tc);
                tc.End();

                if (IsPayrollProcessed == true)
                {
                    throw new Exception("Payroll is processed for some department!");
                }

                List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
                List<SalaryScheme> oSalarySchemes = new List<SalaryScheme>();
                List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
                List<EmployeeGroup> oEmployeeGroups = new List<EmployeeGroup>();
                oDepartmentRequirementPolicys = oPayrollProcessManagement.DepartmentRequirementPolicys;
                oSalarySchemes = oPayrollProcessManagement.SalarySchemes;
                oSalaryHeads = oPayrollProcessManagement.SalaryHeads;
                oEmployeeGroups = oPayrollProcessManagement.EmployeeGroups;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PayrollProcessManagementDA.IUDComp(tc, oPayrollProcessManagement, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
                }
                reader.Close();

                if (oPayrollProcessManagement.PPMID > 0)
                {

                    foreach (DepartmentRequirementPolicy oitem in oDepartmentRequirementPolicys)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.Department;
                        oPPMO.ObjectID = oitem.DepartmentID;
                        Reader = PayrollProcessManagementObjectDA.IUDComp(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (SalaryScheme oitem in oSalarySchemes)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.SalaryScheme;
                        oPPMO.ObjectID = oitem.SalarySchemeID;
                        Reader = PayrollProcessManagementObjectDA.IUDComp(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (EmployeeGroup oitem in oEmployeeGroups)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.EmployeeGroup;
                        oPPMO.ObjectID = oitem.EmployeeTypeID;
                        Reader = PayrollProcessManagementObjectDA.IUDComp(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (SalaryHead oitem in oSalaryHeads)
                    {
                        IDataReader Reader;
                        PayrollProcessManagementObject oPPMO = new PayrollProcessManagementObject();
                        oPPMO.PPMOID = 0;
                        oPPMO.PPMID = oPayrollProcessManagement.PPMID;
                        oPPMO.PPMObject = EnumPPMObject.SalaryHead;
                        oPPMO.ObjectID = oitem.SalaryHeadID;
                        Reader = PayrollProcessManagementObjectDA.IUDComp(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }

                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public string ProcessPayrollComp(int nSalarySchemeID, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {
            string sMessage = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                PayrollProcessManagementDA.ProcessPayrollComp(tc, nSalarySchemeID, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get PayrollProcessManagement", e);
                sMessage = e.Message;
                #endregion
            }
            return sMessage;
        }

        public int ProcessPayrollByEmployeeComp(int nIndex, int nLocationID, DateTime dStartDate, DateTime dEndDate, string sAllowanceIDs, int nPayRollProcessID, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayrollByEmployeeComp(tc, nIndex, nLocationID, dStartDate, dEndDate, sAllowanceIDs, nPayRollProcessID, nUserID);
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

        public PayrollProcessManagement DeleteComp(int nPPMID, Int64 nUserID)
        {

            TransactionContext tc = null;
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "PayrollProcessManagement", "PPMID", oPayrollProcessManagement.PPMID);
                IDataReader reader;
                reader = PayrollProcessManagementDA.DeleteComp(tc, nPPMID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
                }
                oPayrollProcessManagement.ErrorMessage = "";
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public PayrollProcessManagement GetComp(int nPPMID, Int64 nUserId)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PayrollProcessManagementDA.GetComp(nPPMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get PayrollProcessManagement", e);
                oPayrollProcessManagement.ErrorMessage = e.Message;
                #endregion
            }

            return oPayrollProcessManagement;
        }

        public int CheckPayrollProcessComp(string sSql, Int64 nUserID)
        {
            TransactionContext tc = null;
            tc = TransactionContext.Begin(true);
            int nIndex = PayrollProcessManagementDA.CheckPayrollProcessForIndexComp(sSql, tc);
            tc.End();
            return nIndex;
        }

        public PayrollProcessManagement PPM_UnfreezeComp(int nPPMID, Int64 nUserId)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PayrollProcessManagementDA.PPM_UnfreezeComp(nPPMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get PayrollProcessManagement", e);
                oPayrollProcessManagement.ErrorMessage = e.Message;
                #endregion
            }

            return oPayrollProcessManagement;
        }

        public List<PayrollProcessManagement> GetsComp(Int64 nUserID)
        {
            List<PayrollProcessManagement> oPayrollProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PayrollProcessManagementDA.GetsComp(tc);
                oPayrollProcessManagement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_PayrollProcessManagement", e);
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public List<PayrollProcessManagement> GetsComp(string sSQL, Int64 nUserID)
        {
            List<PayrollProcessManagement> oPayrollProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PayrollProcessManagementDA.GetsComp(sSQL, tc);
                oPayrollProcessManagement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_PayrollProcessManagement", e);
                #endregion
            }
            return oPayrollProcessManagement;
        }

        public int ProcessPayroll_MamiyaComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_MamiyaComp(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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

        public int ProcessPayroll_CorporateComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_CorporateComp(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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
        public int ProcessPayroll_Corporate_DiscontinueComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_Corporate_DiscontinueComp(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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
        public PayrollProcessManagement CheckPPMComp(int nEmployeeID, int nMonthID, int nYear, Int64 nUserID)
        {
            PayrollProcessManagement oPayrollProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PayrollProcessManagementDA.CheckPPMComp(nEmployeeID, nMonthID, nYear, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagement = CreateObject(oReader);
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
                oPayrollProcessManagement.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPayrollProcessManagement;
        }
        public int ProcessPayroll_ProductionComp(int nIndex, int nPayRollProcessID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = PayrollProcessManagementDA.ProcessPayroll_ProductionComp(tc, nIndex, nPayRollProcessID, sEmployeeIDs, nUserID);
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
        public EmployeeSalary SettlementSalaryProcessComp(int nEmployeeSettlementID, Int64 nUserId)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PayrollProcessManagementDA.SettlementSalaryProcessComp(nEmployeeSettlementID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalary.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
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
                //throw new ServiceException("Failed to Get EmployeeSalary", e);
                oEmployeeSalary.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeSalary;
        }

        public List<EmployeeSalary> SettlementSalaryProcess_Corporate_MultipleComp(EmployeeSettlement oEmployeeSettlement, Int64 nUserId)
        {

            string[] sEmpSSIDs;
            sEmpSSIDs = oEmployeeSettlement.ErrorMessage.Split(',');

            string[] sEmpCodes;
            sEmpCodes = oEmployeeSettlement.Params.Split(',');

            EmployeeSalary oESS = new EmployeeSalary();
            List<EmployeeSalary> oTempESSs = new List<EmployeeSalary>();
            List<EmployeeSalary> oESSs = new List<EmployeeSalary>();
            List<EmployeeSalary> oTempList = new List<EmployeeSalary>();
            TransactionContext tc = null;
            int nEmployeeSettlementID = 0;
            string Code = "";
            int nCounter = 0;
            foreach (string sESSID in sEmpSSIDs)
            {
                try
                {
                    tc = TransactionContext.Begin(true);

                    oTempESSs = new List<EmployeeSalary>();
                    IDataReader reader = null;

                    int EmployeeSettID = Convert.ToInt32(sESSID);
                    oEmployeeSettlement.EmployeeSettlementID = EmployeeSettID;
                    nEmployeeSettlementID = EmployeeSettID;
                    Code = sEmpCodes[nCounter];
                    reader = PayrollProcessManagementDA.SettlementSalaryProcess_CorporateComp(nEmployeeSettlementID, nUserId, tc);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oESS.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
                    }
                    reader.Close();
                    tc.End();

                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();
                    EmployeeSalary oItem = new EmployeeSalary();

                    oItem.EmployeeCode = Code;
                    nCounter++;
                    oItem.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oItem);
                }
            }
            return oTempList;


        }

        public EmployeeSalary SettlementSalaryProcess_CorporateComp(int nEmployeeSettlementID, Int64 nUserId)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PayrollProcessManagementDA.SettlementSalaryProcess_CorporateComp(nEmployeeSettlementID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalary.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
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
                //throw new ServiceException("Failed to Get EmployeeSalary", e);
                oEmployeeSalary.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeSalary;
        }

        #endregion






        #endregion

    }
}
