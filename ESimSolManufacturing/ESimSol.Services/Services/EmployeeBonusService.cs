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
    public class EmployeeBonusService : MarshalByRefObject, IEmployeeBonusService
    {
        #region Private functions and declaration
        private EmployeeBonus MapObject(NullHandler oReader)
        {
            EmployeeBonus oEmployeeBonus = new EmployeeBonus();

            oEmployeeBonus.EBID = oReader.GetInt32("EBID");
            oEmployeeBonus.BlockID = oReader.GetInt32("BlockID");
            oEmployeeBonus.CompGrossAmount = oReader.GetDouble("CompGrossAmount");
            oEmployeeBonus.CompBasicAmount = oReader.GetDouble("CompBasicAmount");
            oEmployeeBonus.CompBonusAmount = oReader.GetDouble("CompBonusAmount");
            oEmployeeBonus.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeBonus.LocationID = oReader.GetInt32("LocationID");
            oEmployeeBonus.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeBonus.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeBonus.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oEmployeeBonus.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeBonus.Note = oReader.GetString("Note");
            oEmployeeBonus.BlockName = oReader.GetString("BlockName");
            oEmployeeBonus.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oEmployeeBonus.BonusAmount = oReader.GetDouble("BonusAmount");
            oEmployeeBonus.BasicAmount = oReader.GetDouble("BasicAmount");
            oEmployeeBonus.Year = oReader.GetInt32("Year");
            oEmployeeBonus.Month = (EnumMonth)oReader.GetInt16("Month");
            oEmployeeBonus.MonthInt = (int)(EnumMonth)oReader.GetInt16("Month");
            oEmployeeBonus.EmployeeCategory = (EnumEmployeeCategory)oReader.GetInt16("EmployeeCategory");
            oEmployeeBonus.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeBonus.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeBonus.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeBonus.DesignationName = oReader.GetString("DesignationName");
            oEmployeeBonus.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oEmployeeBonus.LocationName = oReader.GetString("LocationName");
            oEmployeeBonus.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oEmployeeBonus.ConfirmationDate = oReader.GetDateTime("ConfirmationDate");
            oEmployeeBonus.JoiningDate = oReader.GetDateTime("JoiningDate");
            oEmployeeBonus.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeBonus.BonusDeclarationDate = oReader.GetDateTime("BonusDeclarationDate");
            oEmployeeBonus.ApproveByName = oReader.GetString("ApproveByName");

            oEmployeeBonus.ESSID = oReader.GetInt32("ESSID");
            oEmployeeBonus.Stamp = oReader.GetDouble("Stamp");
            oEmployeeBonus.GrossAmount = oReader.GetDouble("GrossAmount");
            oEmployeeBonus.EBGrossAmount = oReader.GetDouble("EBGrossAmount");
            oEmployeeBonus.CompEBGrossAmount = oReader.GetDouble("CompEBGrossAmount");
            oEmployeeBonus.OthersAmount = oReader.GetDouble("OthersAmount");
            oEmployeeBonus.BankAccountNo = oReader.GetString("BankAccountNo");
            oEmployeeBonus.BankBranchName = oReader.GetString("BankBranchName");
            oEmployeeBonus.ETIN = oReader.GetString("ETIN");
            
            oEmployeeBonus.EmployeeBankAccountID = oReader.GetInt32("EmployeeBankAccountID");
            oEmployeeBonus.InPercent = oReader.GetInt32("InPercent");
            oEmployeeBonus.CompInPercent = oReader.GetInt32("CompInPercent");
            oEmployeeBonus.CalculateON = (EnumPayrollApplyOn)oReader.GetInt32("CalculateON");
            oEmployeeBonus.CompCalculateON = (EnumPayrollApplyOn)oReader.GetInt32("CompCalculateON");
            oEmployeeBonus.BonusCashAmount = oReader.GetDouble("BonusCashAmount");
            oEmployeeBonus.BonusBankAmount = oReader.GetDouble("BonusBankAmount");
            return oEmployeeBonus;

        }

        private EmployeeBonus CreateObject(NullHandler oReader)
        {
            EmployeeBonus oEmployeeBonus = MapObject(oReader);
            return oEmployeeBonus;
        }

        private List<EmployeeBonus> CreateObjects(IDataReader oReader)
        {
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeBonus oItem = CreateObject(oHandler);
                oEmployeeBonuss.Add(oItem);
            }
            return oEmployeeBonuss;
        }

        #endregion

        #region Interface implementation
        public EmployeeBonusService() { }

        public List<EmployeeBonus> Process(int SalaryHeadID, int Month, int Year, string Purpose, DateTime UptoDate, Int64 nUserId)
        {
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = EmployeeBonusDA.Process(tc,SalaryHeadID,Month, Year, Purpose, UptoDate,nUserId);
                oEmployeeBonuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeBonuss = new List<EmployeeBonus>();
                EmployeeBonus oEmployeeBonus = new EmployeeBonus();
                oEmployeeBonus.ErrorMessage = e.Message;
                oEmployeeBonuss.Add(oEmployeeBonus);
                #endregion
            }
            return oEmployeeBonuss;
        }

        public List<EmployeeBonus> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeBonus> oEmployeeBonus = new List<EmployeeBonus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeBonusDA.Gets(sSQL, tc);
                oEmployeeBonus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeBonus", e);
                #endregion
            }
            return oEmployeeBonus;
        }

        public string Delete(string IDs, Int64 nUserId)
        {
            EmployeeBonus oEmployeeBonus = new EmployeeBonus();
            TransactionContext tc = null;
            try
            {
                string sSql = "SELECT ISNULL( COUNT(*),0) as TotalCount FROM View_EmployeeBonus WHERE EBID IN(" + IDs + ") AND ApproveBy>0";
                tc = TransactionContext.Begin(true);
                bool IsExists= EmployeeBonusDA.IsExists(sSql, tc);
                tc.End();
                if (IsExists == true)
                {
                    throw new Exception("Some Bonuses are approved.so deletion is not possible !");
                }
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = EmployeeBonusDA.Delete(tc, IDs, nUserId);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeBonus.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeBonus.ErrorMessage;
        }

        public List<EmployeeBonus> Approve(string sParams, Int64 nUserID)
        {
            List<EmployeeBonus> oEmployeeBonus = new List<EmployeeBonus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeBonusDA.Approve(tc, sParams, nUserID);
                oEmployeeBonus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeBonus", e);
                #endregion
            }
            return oEmployeeBonus;
        }

        #endregion
    }
}
