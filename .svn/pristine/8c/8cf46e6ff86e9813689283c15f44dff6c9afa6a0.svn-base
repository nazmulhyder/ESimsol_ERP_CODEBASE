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
    public class EmployeeSalaryStructureService : MarshalByRefObject, IEmployeeSalaryStructureService
    {
        #region Private functions and declaration
        private EmployeeSalaryStructure MapObject(NullHandler oReader)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            oEmployeeSalaryStructure.ESSID = oReader.GetInt32("ESSID");
            oEmployeeSalaryStructure.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSalaryStructure.SSGradeID = oReader.GetInt32("SSGradeID");
            oEmployeeSalaryStructure.SalarySchemeID = oReader.GetInt32("SalarySchemeID");
            oEmployeeSalaryStructure.Description = oReader.GetString("Description");
            oEmployeeSalaryStructure.GrossAmount = oReader.GetInt32("GrossAmount");
            oEmployeeSalaryStructure.IsIncludeFixedItem = oReader.GetBoolean("IsIncludeFixedItem");
            oEmployeeSalaryStructure.ActualGrossAmount = oReader.GetInt32("ActualGrossAmount");
            oEmployeeSalaryStructure.CurrencyID = oReader.GetInt32("CurrencyID");
            oEmployeeSalaryStructure.IsActive = oReader.GetBoolean("IsActive");
            oEmployeeSalaryStructure.IsCashFixed = oReader.GetBoolean("IsCashFixed");
            
            oEmployeeSalaryStructure.IsAllowBankAccount = oReader.GetBoolean("IsAllowBankAccount");
            oEmployeeSalaryStructure.IsFullBonus = oReader.GetBoolean("IsFullBonus");
            oEmployeeSalaryStructure.IsAllowOverTime = oReader.GetBoolean("IsAllowOverTime");
            oEmployeeSalaryStructure.IsAttendanceDependent = oReader.GetBoolean("IsAttendanceDependent");
            oEmployeeSalaryStructure.StartDay = oReader.GetInt32("StartDay");
            oEmployeeSalaryStructure.CashAmount = oReader.GetDouble("CashAmount");
            oEmployeeSalaryStructure.BonusCashAmount = oReader.GetDouble("BonusCashAmount");
            oEmployeeSalaryStructure.CompGrossAmount = oReader.GetDouble("CompGrossAmount");
            //derive

            oEmployeeSalaryStructure.SSGradeName = oReader.GetString("SSGradeName");
            oEmployeeSalaryStructure.SalarySchemeName = oReader.GetString("SalarySchemeName");
            oEmployeeSalaryStructure.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeSalaryStructure.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeSalaryStructure.FatherName = oReader.GetString("FatherName");
            oEmployeeSalaryStructure.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeSalaryStructure.DesignationName = oReader.GetString("DesignationName");
            oEmployeeSalaryStructure.EmployeeTypeName = oReader.GetString("EmployeeTypeName");            
            oEmployeeSalaryStructure.PaymentCycle = (EnumPaymentCycle)oReader.GetInt16("PaymentCycle");
            oEmployeeSalaryStructure.PaymentCycleInt = (int)(EnumPaymentCycle)oReader.GetInt16("PaymentCycle");
            oEmployeeSalaryStructure.OverTimeON = (EnumOverTimeON)oReader.GetInt16("OverTimeON");
            oEmployeeSalaryStructure.DividedBy = oReader.GetDouble("DividedBy");
            oEmployeeSalaryStructure.MultiplicationBy = oReader.GetDouble("MultiplicationBy");
            oEmployeeSalaryStructure.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oEmployeeSalaryStructure.LateCount = oReader.GetInt32("LateCount");
            oEmployeeSalaryStructure.EarlyLeavingCount = oReader.GetInt32("EarlyLeavingCount");
            oEmployeeSalaryStructure.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oEmployeeSalaryStructure.NatureOfEmployee = (EnumEmployeeNature)oReader.GetInt16("NatureOfEmployee");

            oEmployeeSalaryStructure.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oEmployeeSalaryStructure.DateOfJoin = oReader.GetDateTime("JoiningDate");
            oEmployeeSalaryStructure.Gender = oReader.GetString("Gender");
            oEmployeeSalaryStructure.Religion = oReader.GetString("Religion");
            oEmployeeSalaryStructure.LocationName = oReader.GetString("LocationName");

            return oEmployeeSalaryStructure;

        }

        private EmployeeSalaryStructure CreateObject(NullHandler oReader)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = MapObject(oReader);
            return oEmployeeSalaryStructure;
        }

        private List<EmployeeSalaryStructure> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalaryStructure oItem = CreateObject(oHandler);
                oEmployeeSalaryStructures.Add(oItem);
            }
            return oEmployeeSalaryStructures;
        }

        #endregion

        #region Interface implementation
        public EmployeeSalaryStructureService() { }

        public EmployeeSalaryStructure IUD(EmployeeSalaryStructure oEmployeeSalaryStructure, int nDBOperation, Int64 nUserID)
        {

            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            oEmployeeSalaryStructureDetails = oEmployeeSalaryStructure.EmployeeSalaryStructureDetails;
            TransactionContext tc = null;

            try
            {
                if (nDBOperation != 3)
                {
                    if (oEmployeeSalaryStructure.EmployeeSalaryStructureDetails.Count <= 0)
                    {
                        throw new Exception("Please Enter Salary Structure Deatail to Assign a Scheme ! ");
                    }
                    double TotalAmount = oEmployeeSalaryStructure.EmployeeSalaryStructureDetails.Where(x=>x.SalaryHeadType==EnumSalaryHeadType.Basic).Sum(x => x.Amount);
                    if (Math.Round(oEmployeeSalaryStructure.GrossAmount) != Math.Round(TotalAmount))
                    {
                        throw new Exception("Total Amount must be equal to Gross Salary!");
                    }
                }
                
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (nDBOperation == 1) // For Delete the previous Salary Structure
                {
                    EmployeeSalaryStructure oESS = new EmployeeSalaryStructure();
                    string sSQL = "Select * from EmployeeSalaryStructure Where IsActive=1 And EmployeeID=" + oEmployeeSalaryStructure.EmployeeID + "";
                    reader = EmployeeSalaryStructureDA.Get(sSQL,tc);
                    if (reader.Read())
                    {
                        oESS.ESSID = Convert.ToInt32(reader["ESSID"].ToString());
                    }
                    reader.Close();

                    if (oESS.ESSID > 0)
                    {
                        reader = EmployeeSalaryStructureDA.IUD(tc, oESS, nUserID, (int)EnumDBOperation.Delete);
                        reader.Close();
                    }
                    
                }

                reader = EmployeeSalaryStructureDA.IUD(tc, oEmployeeSalaryStructure, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeSalaryStructure = CreateObject(oReader);
                }
                reader.Close();

                #region EmployeeSalaryStructure Detail Part
                if (nDBOperation != 3)
                {                    
                    if (oEmployeeSalaryStructure.ESSID > 0)
                    {
                        string sESSDIDs = "";
                        foreach (EmployeeSalaryStructureDetail oItem in oEmployeeSalaryStructureDetails)
                        {

                            if (oItem.Amount != 0.00 || oItem.Amount != 0)
                            {
                                IDataReader readerDetail;
                                oItem.ESSID = oEmployeeSalaryStructure.ESSID;
                                if (oItem.ESSSID <= 0)
                                {
                                    readerDetail = EmployeeSalaryStructureDetailDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert, "");
                                }
                                else
                                {
                                    readerDetail = EmployeeSalaryStructureDetailDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update, "");

                                }
                                NullHandler oReaderDetail = new NullHandler(readerDetail);
                                if (readerDetail.Read())
                                {
                                    sESSDIDs = sESSDIDs + oReaderDetail.GetInt32("ESSSID").ToString() + ",";
                                }
                                readerDetail.Close();
                            }
                        }
                        if (sESSDIDs.Length > 0)
                        {
                            sESSDIDs = sESSDIDs.Remove(sESSDIDs.Length - 1, 1);
                        }
                        EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
                        oEmployeeSalaryStructureDetail.ESSID = oEmployeeSalaryStructure.ESSID;
                        IDataReader readerStructureDetail = EmployeeSalaryStructureDetailDA.IUD(tc, oEmployeeSalaryStructureDetail, nUserID, (int)EnumDBOperation.Delete, sESSDIDs);
                        readerStructureDetail.Close();
                    }
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSalaryStructure.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSalaryStructure.ESSID = 0;
                #endregion
            }
            return oEmployeeSalaryStructure;
        }


        public EmployeeSalaryStructure Get(int nESSID, Int64 nUserId)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryStructureDA.Get(nESSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryStructure = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryStructure", e);
                oEmployeeSalaryStructure.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryStructure;
        }

        public EmployeeSalaryStructure GetByEmp(int nEmployeeID, Int64 nUserId)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryStructureDA.GetByEmp(nEmployeeID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryStructure = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryStructure", e);
                oEmployeeSalaryStructure.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryStructure;
        }
                
        public EmployeeSalaryStructure Get(string sSQL, Int64 nUserId)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryStructureDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryStructure = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryStructure", e);
                oEmployeeSalaryStructure.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryStructure;
        }

        public EmployeeSalaryStructure EditBankCash(EmployeeSalaryStructure oEmployeeSalaryStructure, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryStructureDA.EditBankCash(tc, oEmployeeSalaryStructure, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                    oEmployeeSalaryStructure = CreateObject(oReader);
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
                oEmployeeSalaryStructure.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oEmployeeSalaryStructure;
        }
        public List<EmployeeSalaryStructure> Gets(Int64 nUserID)
        {
            List<EmployeeSalaryStructure> oEmployeeSalaryStructure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryStructureDA.Gets(tc);
                oEmployeeSalaryStructure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSalaryStructure", e);
                #endregion
            }
            return oEmployeeSalaryStructure;
        }

        public List<EmployeeSalaryStructure> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalaryStructure> oEmployeeSalaryStructure = new List<EmployeeSalaryStructure> ();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryStructureDA.Gets(sSQL, tc);
                oEmployeeSalaryStructure = CreateObjects(reader);
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
                #endregion
            }
            return oEmployeeSalaryStructure;
        }

        public List<EmployeeSalaryStructure> HistoryGets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalaryStructureDA.Gets(sSQL, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalaryStructure oItem = new EmployeeSalaryStructure();

                    oItem.ESSID = oreader.GetInt32("ESSID");
                    oItem.ESSHistoryID = oreader.GetInt32("ESSHistoryID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.SalarySchemeID = oreader.GetInt32("SalarySchemeID");
                    oItem.Description = oreader.GetString("Description");
                    oItem.GrossAmount = oreader.GetInt32("GrossAmount");
                    oItem.IsIncludeFixedItem = oreader.GetBoolean("IsIncludeFixedItem");
                    oItem.ActualGrossAmount = oreader.GetInt32("ActualGrossAmount");
                    oItem.CurrencyID = oreader.GetInt32("CurrencyID");
                    oItem.IsActive = oreader.GetBoolean("IsActive");
                    //oItem.IsAllowBankAccount = oreader.GetBoolean("IsAllowBankAccount");
                    //oItem.IsAllowOverTime = oreader.GetBoolean("IsAllowOverTime");
                    //oItem.IsAttendanceDependent = oreader.GetBoolean("IsAttendanceDependent");
                    //oItem.StartDay = oreader.GetInt32("StartDay");
                    //oItem.CashAmount = oreader.GetDouble("CashAmount");

                    oItem.SalarySchemeName = oreader.GetString("SalarySchemeName");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.EmployeeTypeName = oreader.GetString("EmployeeTypeName");
                    oItem.PaymentCycle = (EnumPaymentCycle)oreader.GetInt16("PaymentCycle");
                    oItem.PaymentCycleInt = (int)(EnumPaymentCycle)oreader.GetInt16("PaymentCycle");
                    oItem.OverTimeON = (EnumOverTimeON)oreader.GetInt16("OverTimeON");
                    oItem.DividedBy = oreader.GetDouble("DividedBy");
                    oItem.MultiplicationBy = oreader.GetDouble("MultiplicationBy");
                    oItem.IsProductionBase = oreader.GetBoolean("IsProductionBase");
                    oItem.LateCount = oreader.GetInt32("LateCount");
                    oItem.EarlyLeavingCount = oreader.GetInt32("EarlyLeavingCount");
                    oItem.CurrencySymbol = oreader.GetString("CurrencySymbol");
                    oItem.NatureOfEmployee = (EnumEmployeeNature)oreader.GetInt16("NatureOfEmployee");

                    oItem.DateOfBirth = oreader.GetDateTime("DateOfBirth");
                    oItem.DateOfJoin = oreader.GetDateTime("JoiningDate");
                    oItem.Gender = oreader.GetString("Gender");
                    oItem.Religion = oreader.GetString("Religion");
                    oItem.LocationName = oreader.GetString("LocationName");

                    oEmployeeSalaryStructures.Add(oItem);
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
                #endregion
            }
            return oEmployeeSalaryStructures;
        }

        public List<EmployeeSalaryStructure> CopyEmployeeSalaryStructure(int nCopyFromESSID, List<Employee> oEmployees, Int64 nUserId)
        {
            List<EmployeeSalaryStructure> oESSs = new List<EmployeeSalaryStructure>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                foreach (Employee oItem in oEmployees)
                {
                    IDataReader reader = EmployeeSalaryStructureDA.CopyEmployeeSalaryStructure(tc, oItem.EmployeeID, nCopyFromESSID, nUserId);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        EmployeeSalaryStructure oESS = new EmployeeSalaryStructure();
                        oESS = CreateObject(oReader);
                        oESSs.Add(oESS);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get EmployeeSalaryStructure", e);
                #endregion
            }
            return oESSs;
        }


        #endregion
        #region Activity
        public EmployeeSalaryStructure Activite(int EmpID, int nESSID, bool Active, Int64 nUserId)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryStructureDA.Activity(EmpID, nESSID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryStructure = CreateObject(oReader);
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
                oEmployeeSalaryStructure.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryStructure;
        }


        #endregion

        #region Multiple Increment
        public List<EmployeeSalaryStructure> MultipleIncrement(string sParams, Int64 nUserID)
        {
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryStructureDA.MultipleIncrement(sParams, nUserID, tc);
                oEmployeeSalaryStructures = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                oEmployeeSalaryStructure.ErrorMessage = e.Message;
                oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
                oEmployeeSalaryStructures.Add(oEmployeeSalaryStructure);
            }
            return oEmployeeSalaryStructures;
        }
        #endregion
    }
}
