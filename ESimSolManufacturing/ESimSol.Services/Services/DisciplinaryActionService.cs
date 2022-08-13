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
    public class DisciplinaryActionService : MarshalByRefObject, IDisciplinaryActionService
    {
        #region Private functions and declaration
        private DisciplinaryAction MapObject(NullHandler oReader)
        {
            DisciplinaryAction oDisciplinaryAction = new DisciplinaryAction();
            oDisciplinaryAction.DisciplinaryActionID = oReader.GetInt32("DisciplinaryActionID");
            oDisciplinaryAction.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oDisciplinaryAction.ActionType = (EnumEnumAllowanceType)oReader.GetInt16("ActionType");
            oDisciplinaryAction.EmployeeID = oReader.GetInt32("EmployeeID");
            oDisciplinaryAction.PaymentCycle = (EnumPaymentCycle)oReader.GetInt16("PaymentCycle");
            oDisciplinaryAction.PaymentCycleInt = (int)(EnumPaymentCycle)oReader.GetInt16("PaymentCycle");
            oDisciplinaryAction.Description = oReader.GetString("Description");
            oDisciplinaryAction.Amount = oReader.GetInt32("Amount");
            oDisciplinaryAction.CompAmount = oReader.GetInt32("CompAmount");
            oDisciplinaryAction.ExecutedOn = oReader.GetDateTime("ExecutedOn");
            oDisciplinaryAction.ApproveBy = oReader.GetInt16("ApproveBy");
            oDisciplinaryAction.ApproveByName = oReader.GetString("ApproveByName");
            oDisciplinaryAction.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oDisciplinaryAction.ProcessID = oReader.GetInt32("ProcessID");
            oDisciplinaryAction.IsLock = oReader.GetBoolean("IsLock");
            oDisciplinaryAction.EncryptDAID = Global.Encrypt(oDisciplinaryAction.DisciplinaryActionID.ToString());

            //Derive Property
            oDisciplinaryAction.EmployeeCode = oReader.GetString("EmployeeCode");
            oDisciplinaryAction.EmployeeName = oReader.GetString("EmployeeName");
            oDisciplinaryAction.OfficialInfo = oReader.GetString("OfficialInfo");
            oDisciplinaryAction.LocationName = oReader.GetString("LocationName");
            oDisciplinaryAction.DesignationName = oReader.GetString("DesignationName");
            oDisciplinaryAction.DepartmentName = oReader.GetString("DepartmentName");
            oDisciplinaryAction.JoiningDate = oReader.GetDateTime("JoiningDate");
            oDisciplinaryAction.Remark = oReader.GetString("Remark");
            oDisciplinaryAction.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oDisciplinaryAction.SalaryHeadType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            oDisciplinaryAction.SalaryHeadTypeInt = (int)(EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            return oDisciplinaryAction;

           

        }

        private DisciplinaryAction CreateObject(NullHandler oReader)
        {
            DisciplinaryAction oDisciplinaryAction = MapObject(oReader);
            return oDisciplinaryAction;
        }

        private List<DisciplinaryAction> CreateObjects(IDataReader oReader)
        {
            List<DisciplinaryAction> oDisciplinaryAction = new List<DisciplinaryAction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DisciplinaryAction oItem = CreateObject(oHandler);
                oDisciplinaryAction.Add(oItem);
            }
            return oDisciplinaryAction;
        }

        #endregion

        #region Interface implementation
        public DisciplinaryActionService() { }

        public List<DisciplinaryAction> IUD(DisciplinaryAction oDisciplinaryAction, int nDBOperation, Int64 nUserID)
        {
            DisciplinaryAction oDA = new DisciplinaryAction();
            List<DisciplinaryAction> oDAs = new List<DisciplinaryAction>();
            TransactionContext tc = null;
            try
            {
                string[] IDs = oDisciplinaryAction.sIDs.Split(',');// it can be employeeID/ disciplinaryID
                tc = TransactionContext.Begin(true);
                foreach( string sID in IDs)
                {
                    int nID = Convert.ToInt32(sID);
                    if (nDBOperation == 1 || nDBOperation == 2)
                    {
                        oDisciplinaryAction.EmployeeID = nID;
                    }
                    else
                    {
                        oDisciplinaryAction.DisciplinaryActionID = nID;
                    }
                    
                    IDataReader reader;
                    reader = DisciplinaryActionDA.IUD(tc, oDisciplinaryAction, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDA = new DisciplinaryAction();
                        oDA = CreateObject(oReader);
                        oDAs.Add(oDA);
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
                throw new ServiceException(e.Message);
                #endregion
            }
            return oDAs;
        }


        public DisciplinaryAction Get(int id, Int64 nUserId)
        {
            DisciplinaryAction aDisciplinaryAction = new DisciplinaryAction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DisciplinaryActionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aDisciplinaryAction = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DisciplinaryAction", e);
                #endregion
            }

            return aDisciplinaryAction;
        }

        public List<DisciplinaryAction> Gets(string sSQL, Int64 nUserID)
        {
            List<DisciplinaryAction> oDisciplinaryAction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DisciplinaryActionDA.Gets(sSQL,tc);
                oDisciplinaryAction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DisciplinaryAction", e);
                #endregion
            }
            return oDisciplinaryAction;
        }

        public List<DisciplinaryAction> IUD_List(List<DisciplinaryAction> oDisciplinaryActions, Int64 nUserID)
        {
            TransactionContext tc = null;
            DisciplinaryAction oDisciplinaryAction = new DisciplinaryAction();
            List<DisciplinaryAction> oDAs = new List<DisciplinaryAction>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                int nDBOperation = 0;
                foreach (DisciplinaryAction ODA in oDisciplinaryActions)
                {
                    if (ODA.DisciplinaryActionID > 0)
                    {
                        nDBOperation = 2;
                    }
                    else
                    {
                        nDBOperation = 1;
                    }
                    ODA.PaymentCycle = EnumPaymentCycle.Monthly;
                    ODA.ActionType = EnumEnumAllowanceType.Deduction;
                    ODA.Description = "Advanced Payment";
                    reader = DisciplinaryActionDA.IUD(tc, ODA, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDisciplinaryAction = CreateObject(oReader);
                    }
                    oDAs.Add(oDisciplinaryAction);
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDAs.Add(oDisciplinaryAction);
                oDAs[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View  

                #endregion
            }
            return oDAs;
        }
        public List<DisciplinaryAction> GetsForDAProcess(string sParams, Int64 nUserID)
        {
            List<DisciplinaryAction> oDisciplinaryActions = new List<DisciplinaryAction>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DisciplinaryActionDA.GetsForDAProcess(sParams, tc);

                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    DisciplinaryAction oItem = new DisciplinaryAction();
                    oItem.DisciplinaryActionID = oreader.GetInt32("DAId");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.ProductionAmount = oreader.GetDouble("ProductionAmount");
                    oItem.Amount = oreader.GetDouble("DAAmount");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.ApproveBy = oreader.GetInt32("ApproveBy");
                    oDisciplinaryActions.Add(oItem);
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
                throw new ServiceException("Failed to Get DisciplinaryAction", e);
                #endregion
            }
            return oDisciplinaryActions;
        }

        #region UploadXL
        public List<DisciplinaryAction> UploadXL(List<DisciplinaryAction> oDAs, Int64 nUserID)
        {
            DisciplinaryAction oTempDA = new DisciplinaryAction();
            List<DisciplinaryAction> oTempDAs = new List<DisciplinaryAction>();
            List<DisciplinaryAction> oTempList = new List<DisciplinaryAction>();
            TransactionContext tc = null;
            //try
            //{
            //    int nCount = 0;
            //    foreach (DisciplinaryAction oItem in oDAs)
            //    {
            //        tc = TransactionContext.Begin(true);
            //        IDataReader reader;
            //        oTempDA = new DisciplinaryAction();
            //        reader = DisciplinaryActionDA.UploadXL(tc, oItem, nUserID);
            //        if (nCount < 100)
            //        {
            //            NullHandler oReader = new NullHandler(reader);
            //            if (reader.Read())
            //            {
            //                oTempDA = CreateObject(oReader);
            //            }
            //            if (oTempDA.DisciplinaryActionID > 0)
            //            {
            //                oTempDAs.Add(oTempDA);
            //                nCount++;
            //            }
            //        }
                    
            //        reader.Close();
            //        tc.End();
            //    }
            //}
            //catch (Exception e)
            //{
            //    #region Handle Exception
            //    if (tc != null)
            //        tc.HandleError();
            //    throw new ServiceException(e.Message.Split('!')[0]);
            //    #endregion
            //}
            //return oTempDAs;

            int nCount = 0;
            foreach (DisciplinaryAction oItem in oDAs)
            {
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    oTempDA = new DisciplinaryAction();
                    reader = DisciplinaryActionDA.UploadXL(tc, oItem, nUserID);
                    if (nCount < 100)
                    {
                        NullHandler oReader = new NullHandler(reader);
                        //if (reader.RecordsAffected <= 0)
                        //{
                        //    oTempList.Add(oItem);
                        //}
                        if (reader.Read())
                        {
                            oTempDA = CreateObject(oReader);
                        }
                        if (oTempDA.DisciplinaryActionID > 0)
                        {
                            oTempDAs.Add(oTempDA);
                            nCount++;
                        }
                    }

                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();

                    oItem.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oItem);
                }
            }

            return oTempList;
        }
        #endregion UploadXL

        #endregion
    }
}
