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
    public class ArchiveSalaryStrucService : MarshalByRefObject, IArchiveSalaryStrucService
    {
        #region Private functions and declaration
        private ArchiveSalaryStruc MapObject(NullHandler oReader)
        {
            ArchiveSalaryStruc oArchiveSalaryStruc = new ArchiveSalaryStruc();
            oArchiveSalaryStruc.ArchiveSalaryStrucID = oReader.GetInt32("ArchiveSalaryStrucID");
            oArchiveSalaryStruc.ArchiveDataID = oReader.GetInt32("ArchiveDataID");
            oArchiveSalaryStruc.SalaryMonthID = oReader.GetInt32("SalaryMonthID");
            oArchiveSalaryStruc.SalaryYearID = oReader.GetInt32("SalaryYearID");
            oArchiveSalaryStruc.EmployeeID = oReader.GetInt32("EmployeeID");
            oArchiveSalaryStruc.BUID = oReader.GetInt32("BUID");
            oArchiveSalaryStruc.LocationID = oReader.GetInt32("LocationID");
            oArchiveSalaryStruc.DepartmentID = oReader.GetInt32("DepartmentID");
            oArchiveSalaryStruc.DesignationID = oReader.GetInt32("DesignationID");
            oArchiveSalaryStruc.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oArchiveSalaryStruc.SSGradeID = oReader.GetInt32("SSGradeID");
            oArchiveSalaryStruc.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oArchiveSalaryStruc.SalarySchemeID = oReader.GetInt32("SalarySchemeID");
            oArchiveSalaryStruc.IsAllowBankAccount = oReader.GetBoolean("IsAllowBankAccount");
            oArchiveSalaryStruc.IsAllowOverTime = oReader.GetBoolean("IsAllowOverTime");
            oArchiveSalaryStruc.IsAttendanceDependent = oReader.GetBoolean("IsAttendanceDependent");
            oArchiveSalaryStruc.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oArchiveSalaryStruc.GrossAmount = oReader.GetDouble("GrossAmount");
            oArchiveSalaryStruc.CompGrossAmount = oReader.GetDouble("CompGrossAmount");
            oArchiveSalaryStruc.IsCashFixed = oReader.GetBoolean("IsCashFixed");
            oArchiveSalaryStruc.CashBankAmount = oReader.GetDouble("CashBankAmount");
            oArchiveSalaryStruc.Remarks = oReader.GetString("Remarks");
            oArchiveSalaryStruc.DBUserID = oReader.GetInt32("DBUserID");
            oArchiveSalaryStruc.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oArchiveSalaryStruc.EmpCode = oReader.GetString("EmpCode");
            oArchiveSalaryStruc.EmpName = oReader.GetString("EmpName");
            oArchiveSalaryStruc.LocName = oReader.GetString("LocName");
            oArchiveSalaryStruc.SchemeName = oReader.GetString("SchemeName");
            oArchiveSalaryStruc.DeptName = oReader.GetString("DeptName");
            oArchiveSalaryStruc.DesigName = oReader.GetString("DesigName");
            oArchiveSalaryStruc.BUName = oReader.GetString("BUName");
            oArchiveSalaryStruc.BUShortName = oReader.GetString("BUShortName");
            oArchiveSalaryStruc.EntryUserName = oReader.GetString("EntryUserName");
            oArchiveSalaryStruc.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            return oArchiveSalaryStruc;
        }

        private ArchiveSalaryStruc CreateObject(NullHandler oReader)
        {
            ArchiveSalaryStruc oArchiveSalaryStruc = new ArchiveSalaryStruc();
            oArchiveSalaryStruc = MapObject(oReader);
            return oArchiveSalaryStruc;
        }

        private List<ArchiveSalaryStruc> CreateObjects(IDataReader oReader)
        {
            List<ArchiveSalaryStruc> oArchiveSalaryStruc = new List<ArchiveSalaryStruc>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ArchiveSalaryStruc oItem = CreateObject(oHandler);
                oArchiveSalaryStruc.Add(oItem);
            }
            return oArchiveSalaryStruc;
        }

        #endregion

        #region Interface implementation
        public ArchiveSalaryStrucService() { }
        public ArchiveSalaryStruc Save(ArchiveSalaryStruc oArchiveSalaryStruc, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oArchiveSalaryStruc.ArchiveSalaryStrucID <= 0)
                {
                    reader = ArchiveSalaryStrucDA.InsertUpdate(tc, oArchiveSalaryStruc, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ArchiveSalaryStrucDA.InsertUpdate(tc, oArchiveSalaryStruc, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oArchiveSalaryStruc = new ArchiveSalaryStruc();
                    oArchiveSalaryStruc = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ArchiveSalaryStruc. Because of " + e.Message, e);
                #endregion
            }
            return oArchiveSalaryStruc;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ArchiveSalaryStruc oArchiveSalaryStruc = new ArchiveSalaryStruc();
                oArchiveSalaryStruc.ArchiveSalaryStrucID = id;
                ArchiveSalaryStrucDA.Delete(tc, oArchiveSalaryStruc, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public List<ArchiveSalaryStruc> ArchiveSalaryChnage(int nArchiveDataID, List<EmployeeBatchDetail> oEmployeeBatchDetails, Int64 nUserId)
        {
            ArchiveSalaryStruc oTempArchiveSalaryStruc = new ArchiveSalaryStruc();
            List<ArchiveSalaryStruc> oArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (EmployeeBatchDetail oEmployeeBatchDetail in oEmployeeBatchDetails)
                {
                    try
                    {
                        ArchiveSalaryStrucDA.ArchiveSalaryChnage(tc, nArchiveDataID, oEmployeeBatchDetail, nUserId);

                    }
                    catch (Exception e)
                    {
                        oTempArchiveSalaryStruc = new ArchiveSalaryStruc();
                        oTempArchiveSalaryStruc.EmpCode = oEmployeeBatchDetail.EmployeeCode;
                        oTempArchiveSalaryStruc.EmpName = oEmployeeBatchDetail.EmployeeName;
                        oTempArchiveSalaryStruc.ErrorMessage = e.Message.Split('~')[0];
                        oArchiveSalaryStrucs.Add(oTempArchiveSalaryStruc);
                    }
                }
                tc.End();
            }
            catch (Exception ex)
            {
                oTempArchiveSalaryStruc = new ArchiveSalaryStruc();
                oTempArchiveSalaryStruc.ErrorMessage = ex.Message.Split('~')[0];
                oArchiveSalaryStrucs.Add(oTempArchiveSalaryStruc);
            }
            return oArchiveSalaryStrucs;
        }


        public ArchiveSalaryStruc Get(int id, Int64 nUserId)
        {
            ArchiveSalaryStruc oArchiveSalaryStruc = new ArchiveSalaryStruc();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ArchiveSalaryStrucDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oArchiveSalaryStruc = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ArchiveSalaryStruc", e);
                #endregion
            }
            return oArchiveSalaryStruc;
        }
        public List<ArchiveSalaryStruc> Gets(Int64 nUserID)
        {
            List<ArchiveSalaryStruc> oArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ArchiveSalaryStrucDA.Gets(tc);
                oArchiveSalaryStrucs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ArchiveSalaryStruc", e);
                #endregion
            }
            return oArchiveSalaryStrucs;
        }
        public List<ArchiveSalaryStruc> Gets(string sSQL,Int64 nUserID)
        {
            List<ArchiveSalaryStruc> oArchiveSalaryStrucs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ArchiveSalaryStrucDA.Gets(tc,sSQL);
                oArchiveSalaryStrucs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ArchiveSalaryStruc", e);
                #endregion
            }
            return oArchiveSalaryStrucs;
        }
        #endregion
    }   
}