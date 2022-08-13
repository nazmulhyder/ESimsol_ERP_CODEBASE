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
    public class EmployeeLeaveLedgerService : MarshalByRefObject, IEmployeeLeaveLedgerService
    {
        #region Private functions and declaration
        private EmployeeLeaveLedger MapObject(NullHandler oReader)
        {
            EmployeeLeaveLedger oELL = new EmployeeLeaveLedger();
            oELL.EmpLeaveLedgerID = oReader.GetInt32("EmpLeaveLedgerID");
            oELL.EmployeeID = oReader.GetInt32("EmployeeID");
            oELL.ACSID = oReader.GetInt32("ACSID");
            oELL.ASLID = oReader.GetInt32("ASLID");
            oELL.LeaveID = oReader.GetInt32("LeaveID");
            oELL.TotalDay = oReader.GetDouble("TotalDay");
            oELL.DeferredDay = oReader.GetInt32("DeferredDay");
            oELL.ActivationAfter = (EnumRecruitmentEvent)oReader.GetInt32("ActivationAfter");
            oELL.IsLeaveOnPresence = oReader.GetBoolean("IsLeaveOnPresence");
            oELL.IsComp = oReader.GetBoolean("IsComp");
            oELL.PresencePerLeave = oReader.GetInt32("PresencePerLeave");
            oELL.IsCarryForward = oReader.GetBoolean("IsCarryForward");
            oELL.MaxCarryDays = oReader.GetInt32("MaxCarryDays");
            oELL.Addition = oReader.GetInt32("Addition");
            oELL.Deduction = oReader.GetInt32("Deduction");
            oELL.LeaveName = oReader.GetString("LeaveName");
            oELL.FullLeave = oReader.GetInt32("FullLeave");
            oELL.HalfLeave = oReader.GetInt32("HalfLeave");
            oELL.ShortLeave = oReader.GetInt32("ShortLeave");
            oELL.IsLWP = oReader.GetBoolean("IsLWP");
            oELL.Session = oReader.GetString("Session");
            oELL.EnjoyLeave = oReader.GetInt32("EnjoyLeave");
            return oELL;
        }

        private EmployeeLeaveLedger CreateObject(NullHandler oReader)
        {
            EmployeeLeaveLedger oELL = MapObject(oReader);
            return oELL;
        }

        private List<EmployeeLeaveLedger> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLeaveLedger> oELL = new List<EmployeeLeaveLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLeaveLedger oItem = CreateObject(oHandler);
                oELL.Add(oItem);
            }
            return oELL;
        }

        #endregion

        #region Interface implementation
        public EmployeeLeaveLedgerService() { }

        public EmployeeLeaveLedger IUD(EmployeeLeaveLedger oELL, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = EmployeeLeaveLedgerDA.IUD(tc, oELL, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oELL = new EmployeeLeaveLedger();
                        oELL = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = EmployeeLeaveLedgerDA.IUD(tc, oELL, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oELL.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELL = new EmployeeLeaveLedger();
                oELL.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oELL;
        }
       

        public EmployeeLeaveLedger Get(int nELLID, Int64 nUserId)
        {
            EmployeeLeaveLedger oELL = new EmployeeLeaveLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLeaveLedgerDA.Get(tc, nELLID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELL = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELL = new EmployeeLeaveLedger();
                oELL.ErrorMessage = ex.Message;
                #endregion
            }

            return oELL;
        }

        public List<EmployeeLeaveLedger> Gets(int nEmpID, Int64 nUserID)
        {
            List<EmployeeLeaveLedger> oELLs = new List<EmployeeLeaveLedger>();
            EmployeeLeaveLedger oELL = new EmployeeLeaveLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveLedgerDA.Gets(tc, nEmpID);
                oELLs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELL.ErrorMessage = ex.Message;
                oELLs.Add(oELL);
                #endregion
            }

            return oELLs;
        }

        public List<EmployeeLeaveLedger> GetsActiveLeaveLedger(int nEmpID, Int64 nUserID)
        {
            List<EmployeeLeaveLedger> oELLs = new List<EmployeeLeaveLedger>();
            EmployeeLeaveLedger oELL = new EmployeeLeaveLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveLedgerDA.GetsActiveLeaveLedger(tc, nEmpID);
                oELLs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELL.ErrorMessage = ex.Message;
                oELLs.Add(oELL);
                #endregion
            }

            return oELLs;
        }

        public List<EmployeeLeaveLedger> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeLeaveLedger> oELLs = new List<EmployeeLeaveLedger>();
            EmployeeLeaveLedger oELL = new EmployeeLeaveLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveLedgerDA.Gets(tc, sSQL);
                oELLs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELL.ErrorMessage = ex.Message;
                oELLs.Add(oELL);
                #endregion
            }

            return oELLs;
        }


        public List<EmployeeLeaveLedger> TransferLeave(int nELLIDFrom, int nELLIDTo, int nDays, string sNote, Int64 nUserId)
        {
            List<EmployeeLeaveLedger> oELLs = new List<EmployeeLeaveLedger>();
            EmployeeLeaveLedger oELL = new EmployeeLeaveLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveLedgerDA.TransferLeave(tc, nELLIDFrom, nELLIDTo, nDays, sNote, nUserId);
                oELLs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELL.ErrorMessage = ex.Message;
                oELLs.Add(oELL);
                #endregion
            }

            return oELLs;
        }


        #endregion
    }
}
