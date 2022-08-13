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
    public class EmployeeSettlementService : MarshalByRefObject, IEmployeeSettlementService
    {
        #region Private functions and declaration
        private EmployeeSettlement MapObject(NullHandler oReader)
        {
            EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();

            oEmployeeSettlement.EmployeeSettlementID = oReader.GetInt32("EmployeeSettlementID");
            oEmployeeSettlement.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSettlement.Reason = oReader.GetString("Reason");
            oEmployeeSettlement.SubmissionDate = oReader.GetDateTime("SubmissionDate");
            oEmployeeSettlement.EffectDate = oReader.GetDateTime("EffectDate");
            oEmployeeSettlement.SettlementType = (EnumSettleMentType)oReader.GetInt16("SettlementType");
            oEmployeeSettlement.IsNoticePayDeduction = oReader.GetBoolean("IsNoticePayDeduction");
            oEmployeeSettlement.IsResigned = oReader.GetBoolean("IsResigned");
            oEmployeeSettlement.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeSettlement.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oEmployeeSettlement.EncryptID = Global.Encrypt(oEmployeeSettlement.EmployeeSettlementID.ToString());
            oEmployeeSettlement.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeSettlement.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeSettlement.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeSettlement.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeSettlement.DesignationName = oReader.GetString("DesignationName");
            oEmployeeSettlement.WorkingStatus = (EnumEmployeeWorkigStatus)oReader.GetInt32("WorkingStatus");
            oEmployeeSettlement.PaymentDate = oReader.GetDateTime("PaymentDate");
            oEmployeeSettlement.IsSalaryHold = oReader.GetBoolean("IsSalaryHold");
            return oEmployeeSettlement;

        }

        private EmployeeSettlement CreateObject(NullHandler oReader)
        {
            EmployeeSettlement oEmployeeSettlement = MapObject(oReader);
            return oEmployeeSettlement;
        }

        private List<EmployeeSettlement> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSettlement oItem = CreateObject(oHandler);
                oEmployeeSettlements.Add(oItem);
            }
            return oEmployeeSettlements;
        }

        #endregion

        #region Interface implementation
        public EmployeeSettlementService() { }

        public EmployeeSettlement IUD(EmployeeSettlement oEmployeeSettlement, int nDBOperation, Int64 nUsEmployeeSettlementID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSettlementDA.IUD(tc, oEmployeeSettlement, nUsEmployeeSettlementID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlement = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oEmployeeSettlement.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSettlement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSettlement.EmployeeSettlementID = 0;
                #endregion
            }
            return oEmployeeSettlement;
        }

        public List<EmployeeSettlement> GetHierarchy(string sEmployeeIDs, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<EmployeeSettlement> oEmployeeSettlements = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementDA.GetHierarchy(tc, sEmployeeIDs);
                oEmployeeSettlements = CreateObjects(reader);
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
            return oEmployeeSettlements;
        }
        public List<EmployeeSettlement> Approve_Multiple(EmployeeSettlement oEmployeeSettlement, int nDBOperation, Int64 nUsEmployeeSettlementID)
        {

            string[] sEmpSSIDs;
            sEmpSSIDs = oEmployeeSettlement.ErrorMessage.Split(',');

            string[] sEmpCodes;
            sEmpCodes = oEmployeeSettlement.Params.Split(',');
            string[] sempids;
            sempids = oEmployeeSettlement.EmpIDs.Split(',');

            EmployeeSettlement oESS = new EmployeeSettlement();
            List<EmployeeSettlement> oTempESSs = new List<EmployeeSettlement>();
            List<EmployeeSettlement> oESSs = new List<EmployeeSettlement>();
            List<EmployeeSettlement> oTempList = new List<EmployeeSettlement>();
            TransactionContext tc = null;
            int nESSID = 0;
            string Code = "";
            string empid = "";
            int nCounter = 0;
            foreach (string sESSID in sEmpSSIDs)
            {
                try
                {
                    tc = TransactionContext.Begin(true);

                    oTempESSs = new List<EmployeeSettlement>();
                    IDataReader reader = null;

                    int EmployeeSettID = Convert.ToInt32(sESSID);
                    oEmployeeSettlement.EmployeeSettlementID = EmployeeSettID;
                    nESSID = EmployeeSettID;
                    Code = sEmpCodes[nCounter];
                    empid = sempids[nCounter];
                    reader = EmployeeSettlementDA.IUD(tc, oEmployeeSettlement, nUsEmployeeSettlementID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oESS = CreateObject(oReader);
                    }

                    reader.Close();
                    tc.End();

                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();
                    EmployeeSettlement oItem = new EmployeeSettlement();

                    oItem.EmployeeSettlementID = nESSID;
                    oItem.EmployeeCode = Code;
                    oItem.EmployeeID = Convert.ToInt32(empid);
                    nCounter++;
                    oItem.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oItem);
                }
            }
            return oTempList;


        }

        public EmployeeSettlement Get(int nEmployeeSettlementID, Int64 nUsEmployeeSettlementID)
        {
            EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementDA.Get(nEmployeeSettlementID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlement = CreateObject(oReader);
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

                oEmployeeSettlement.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlement;
        }

        public EmployeeSettlement Get(string sSQL, Int64 nUsEmployeeSettlementID)
        {
            EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlement = CreateObject(oReader);
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

                oEmployeeSettlement.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlement;
        }

        public List<EmployeeSettlement> Gets(Int64 nUsEmployeeSettlementID)
        {
            List<EmployeeSettlement> oEmployeeSettlement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementDA.Gets(tc);
                oEmployeeSettlement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlement", e);
                #endregion
            }
            return oEmployeeSettlement;
        }

        public List<EmployeeSettlement> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSettlement> oEmployeeSettlement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementDA.Gets(sSQL, tc);
                oEmployeeSettlement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlement", e);
                #endregion
            }
            return oEmployeeSettlement;
        }

        //#region Approve
        //public EmployeeSettlement Approve(int nEmployeeSettlementID, Int64 nUsEmployeeSettlementID)
        //{
        //    EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin();

        //        IDataReader reader = EmployeeSettlementDA.Approve(nEmployeeSettlementID, nUsEmployeeSettlementID, tc);
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oEmployeeSettlement = CreateObject(oReader);
        //        }
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        ExceptionLog.Write(e);
        //        oEmployeeSettlement.ErrorMessage = e.Message;
        //        #endregion
        //    }

        //    return oEmployeeSettlement;
        //}


        //#endregion

        #region PaymentDone
        public EmployeeSettlement PaymentDone(int nEmployeeSettlementID, Int64 nUserID)
        {
            EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();
            TransactionContext tc = null;
            try
            {
                string sSalarySql = "SELECT ISNULL( COUNT(*),0) as TotalCount FROM View_EmployeeSettlementSalary WHERE '" + DateTime.Now.ToString("dd MMM yyyy") + "' BETWEEN StartDate AND EndDate";
                tc = TransactionContext.Begin(true);
                bool IsSalaryProcessed = EmployeeSalaryDA.GetSalary(sSalarySql, tc);
                tc.End();

                if (IsSalaryProcessed == false)
                {
                    throw new Exception("Salary is not processed . So payment done is not possible !");
                }

                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeSettlementDA.PaymentDone(nEmployeeSettlementID, nUserID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlement = CreateObject(oReader);
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
                oEmployeeSettlement.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlement;
        }


        #endregion

        #endregion


    }
}
