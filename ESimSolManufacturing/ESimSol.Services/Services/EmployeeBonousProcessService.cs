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
    public class EmployeeBonousProcessService : MarshalByRefObject, IEmployeeBonousProcessService
    {
        #region Private functions and declaration
        private EmployeeBonusProcess MapObject(NullHandler oReader)
        {
            EmployeeBonusProcess oEmployeeBonousProcess = new EmployeeBonusProcess();
            oEmployeeBonousProcess.EBPID = oReader.GetInt32("EBPID");

            oEmployeeBonousProcess.BUID = oReader.GetInt32("BUID");
            oEmployeeBonousProcess.LocationID = oReader.GetInt32("LocationID");
            oEmployeeBonousProcess.SalaryheadID = oReader.GetInt32("SalaryheadID");
            oEmployeeBonousProcess.IsEmployeeWise = oReader.GetBoolean("IsEmployeeWise");
            oEmployeeBonousProcess.ProcessDate = oReader.GetDateTime("ProcessDate");
            oEmployeeBonousProcess.BonusDeclarationDate = oReader.GetDateTime("BonusDeclarationDate");
            oEmployeeBonousProcess.Note = oReader.GetString("Note");
            oEmployeeBonousProcess.Year = oReader.GetInt32("Year");
            oEmployeeBonousProcess.MonthID = (EnumMonth)oReader.GetInt32("MonthID");
            oEmployeeBonousProcess.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeBonousProcess.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeBonousProcess.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeBonousProcess.BUName = oReader.GetString("BUName");
            oEmployeeBonousProcess.LocationName = oReader.GetString("LocationName");

            return oEmployeeBonousProcess;

        }

        private EmployeeBonusProcess CreateObject(NullHandler oReader)
        {
            EmployeeBonusProcess oEmployeeBonousProcess = MapObject(oReader);
            return oEmployeeBonousProcess;
        }

        private List<EmployeeBonusProcess> CreateObjects(IDataReader oReader)
        {
            List<EmployeeBonusProcess> oEmployeeBonousProcess = new List<EmployeeBonusProcess>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeBonusProcess oItem = CreateObject(oHandler);
                oEmployeeBonousProcess.Add(oItem);
            }
            return oEmployeeBonousProcess;
        }


        #endregion

        #region Interface implementation
        public EmployeeBonousProcessService() { }
        public EmployeeBonusProcess IUD(EmployeeBonusProcess oEmployeeBonousProcess, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = EmployeeBonusProcessDA.IUD(tc, oEmployeeBonousProcess, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployeeBonousProcess = new EmployeeBonusProcess();
                        oEmployeeBonousProcess = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = EmployeeBonusProcessDA.IUD(tc, oEmployeeBonousProcess, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oEmployeeBonousProcess.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oEmployeeBonousProcess = new EmployeeBonusProcess();
                oEmployeeBonousProcess.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oEmployeeBonousProcess;
        }

        public EmployeeBonusProcess Approved(EmployeeBonusProcess oEmployeeBonusProcess, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeBonusProcessDA.IUD(tc, oEmployeeBonusProcess, nUserID, (int)EnumDBOperation.Approval);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBonusProcess = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oEmployeeBonusProcess = new EmployeeBonusProcess();
                oEmployeeBonusProcess.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oEmployeeBonusProcess;
        }

        public int ProcessEmpBonus(int nIndex, int nEBPID, string sEmployeeIDs, Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = EmployeeBonusProcessDA.ProcessBonus(tc, nIndex, nEBPID, sEmployeeIDs, nUserID);
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

        public EmployeeBonusProcess UndoApproved(EmployeeBonusProcess oEmployeeBonusProcess, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeBonusProcessDA.IUD(tc, oEmployeeBonusProcess, nUserID, (int)EnumDBOperation.UnApproval);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBonusProcess = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oEmployeeBonusProcess = new EmployeeBonusProcess();
                oEmployeeBonusProcess.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oEmployeeBonusProcess;
        }

        public EmployeeBonusProcess Get(string sSQL, Int64 nUserId)
        {
            EmployeeBonusProcess oEmployeeBonousProcess = new EmployeeBonusProcess();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeBonusProcessDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBonousProcess = CreateObject(oReader);
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
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeBonousProcess;
        }


        public List<EmployeeBonusProcess> Gets(Int64 nUserId)
        {
            List<EmployeeBonusProcess> oEmployeeBonusProcess = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeBonusProcessDA.Gets(tc);
                oEmployeeBonusProcess = CreateObjects(reader);
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

            return oEmployeeBonusProcess;
        }
        public EmployeeBonusProcess Get(int nPPMID, Int64 nUserId)
        {
            EmployeeBonusProcess oEmployeeBonusProcess = new EmployeeBonusProcess();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeBonusProcessDA.Get(nPPMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBonusProcess = CreateObject(oReader);
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
                oEmployeeBonusProcess.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeBonusProcess;
        }
        public List<EmployeeBonusProcess> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeBonusProcess> oEmployeeBonousProcess = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeBonusProcessDA.Gets(sSQL, tc);
                oEmployeeBonousProcess = CreateObjects(reader);
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
            return oEmployeeBonousProcess;
        }

        public EmployeeBonusProcess Process(EmployeeBonusProcess oEmployeeBonusProcess, Int64 nUserID)
        {
            string sEmpIDs = "";
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            TransactionContext tc = null;
            try
            {
                if (oEmployeeBonusProcess.IsEmployeeWise == false)
                {
                    string sDIDs = string.Join(",", oEmployeeBonusProcess.DepartmentRequirementPolicys.Select(x => x.DepartmentID));
                    string sSql = "SELECT * FROM EmployeeBonusProcessObject WHERE PPMObject=1 AND ObjectID IN(" + sDIDs + ")"
                                       + "AND EBPID IN(SELECT EBPID FROM EmployeeBonusProcess WHERE MonthID=" + oEmployeeBonusProcess.MonthIDInt+" AND Year="+oEmployeeBonusProcess.Year+ " AND LocationID=" + oEmployeeBonusProcess.LocationID + ")";
                    tc = TransactionContext.Begin(true);
                    bool IsBonusProcessed = EmployeeBonusProcessDA.CheckBonusProcess(sSql, tc);
                    tc.End();

                    if (IsBonusProcessed == true)
                    {
                        throw new Exception("Bonus is processed for some department!");
                    }
                }

                List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
                List<SalaryScheme> oSalarySchemes = new List<SalaryScheme>();
                List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
                List<EmployeeGroup> oEmployeeGroups = new List<EmployeeGroup>();
                oDepartmentRequirementPolicys = oEmployeeBonusProcess.DepartmentRequirementPolicys;
                oSalarySchemes = oEmployeeBonusProcess.SalarySchemes;
                oSalaryHeads = oEmployeeBonusProcess.SalaryHeads;
                oEmployeeGroups = oEmployeeBonusProcess.EmployeeGroups;
                sEmpIDs = oEmployeeBonusProcess.EmployeeIDs;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeBonusProcessDA.IUD(tc, oEmployeeBonusProcess, nUserID, 1);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBonusProcess = CreateObject(oReader);
                }
                reader.Close();

                if (oEmployeeBonusProcess.EBPID > 0 && oEmployeeBonusProcess.IsEmployeeWise == false)
                {

                    foreach (DepartmentRequirementPolicy oitem in oDepartmentRequirementPolicys)
                    {
                        IDataReader Reader;
                        EmployeeBonusProcessObject oPPMO = new EmployeeBonusProcessObject();
                        oPPMO.EBPObjectID = 0;
                        oPPMO.EBPID = oEmployeeBonusProcess.EBPID;
                        oPPMO.PPMObject = (int)EnumPPMObject.Department;
                        oPPMO.ObjectID = oitem.DepartmentID;
                        Reader = EmployeeBonusProcessObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (SalaryScheme oitem in oSalarySchemes)
                    {
                        IDataReader Reader;
                        EmployeeBonusProcessObject oPPMO = new EmployeeBonusProcessObject();
                        oPPMO.EBPObjectID = 0;
                        oPPMO.EBPID = oEmployeeBonusProcess.EBPID;
                        oPPMO.PPMObject = (int)EnumPPMObject.SalaryScheme;
                        oPPMO.ObjectID = oitem.SalarySchemeID;
                        Reader = EmployeeBonusProcessObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (EmployeeGroup oitem in oEmployeeGroups)
                    {
                        IDataReader Reader;
                        EmployeeBonusProcessObject oPPMO = new EmployeeBonusProcessObject();
                        oPPMO.EBPObjectID = 0;
                        oPPMO.EBPID = oEmployeeBonusProcess.EBPID;
                        oPPMO.PPMObject = (int)EnumPPMObject.EmployeeGroup;
                        oPPMO.ObjectID = oitem.EmployeeTypeID;
                        Reader = EmployeeBonusProcessObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }
                    foreach (SalaryHead oitem in oSalaryHeads)
                    {
                        IDataReader Reader;
                        EmployeeBonusProcessObject oPPMO = new EmployeeBonusProcessObject();
                        oPPMO.EBPObjectID = 0;
                        oPPMO.EBPID = oEmployeeBonusProcess.EBPID;
                        oPPMO.PPMObject = (int)EnumPPMObject.SalaryHead;
                        oPPMO.ObjectID = oitem.SalaryHeadID;
                        Reader = EmployeeBonusProcessObjectDA.IUD(tc, oPPMO, nUserID, 1);
                        Reader.Close();
                    }

                }

                //int nNewIndex = 0;
                //if (oEmployeeBonusProcess.EBPID > 0)
                //{
                //    //IDataReader Reader;
                //    oEmployeeBonusProcess.EmployeeIDs = sEmpIDs;
                //    nNewIndex = EmployeeBonusProcessDA.InsertBonus(tc, nIndex, oEmployeeBonusProcess, nUserID);
                //    //Reader = EmployeeBonusProcessDA.InsertBonus(tc, oEmployeeBonusProcess, nUserID);
                    
                //    //Reader.Close();
                //}

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeBonusProcess.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oEmployeeBonusProcess;
        }

        #endregion
    }
}


