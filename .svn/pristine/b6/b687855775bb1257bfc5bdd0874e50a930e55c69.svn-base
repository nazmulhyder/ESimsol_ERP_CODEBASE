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
    public class EmployeeSettlementClearanceService : MarshalByRefObject, IEmployeeSettlementClearanceService
    {
        #region Private functions and declaration
        private EmployeeSettlementClearance MapObject(NullHandler oReader)
        {
            EmployeeSettlementClearance oEmployeeSettlementClearance = new EmployeeSettlementClearance();
            oEmployeeSettlementClearance.ESCID = oReader.GetInt32("ESCID");
            oEmployeeSettlementClearance.EmployeeSettlementID = oReader.GetInt32("EmployeeSettlementID");
            oEmployeeSettlementClearance.ESCSetupID = oReader.GetInt32("ESCSetupID");
            oEmployeeSettlementClearance.CurrentStatus = (EnumESCrearance)oReader.GetInt16("CurrentStatus");
            oEmployeeSettlementClearance.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeSettlementClearance.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeSettlementClearance.EmployeeName = oReader.GetString("EmployeeName");
            return oEmployeeSettlementClearance;
        }

        private EmployeeSettlementClearance CreateObject(NullHandler oReader)
        {
            EmployeeSettlementClearance oEmployeeSettlementClearance = MapObject(oReader);
            return oEmployeeSettlementClearance;
        }

        private List<EmployeeSettlementClearance> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSettlementClearance> oEmployeeSettlementClearances = new List<EmployeeSettlementClearance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSettlementClearance oItem = CreateObject(oHandler);
                oEmployeeSettlementClearances.Add(oItem);
            }
            return oEmployeeSettlementClearances;
        }

        #endregion

        #region Interface implementation
        public EmployeeSettlementClearanceService() { }

        public List<EmployeeSettlementClearance> IUD(EmployeeSettlementClearance oEmployeeSettlementClearance, int nDBOperation, Int64 nUserID)
        {
            List<EmployeeSettlementClearance> oEmployeeSettlementClearances = new List<EmployeeSettlementClearance>();
            List<EmployeeSettlementClearanceHistory> oEmployeeSettlementClearanceHistorys = new List<EmployeeSettlementClearanceHistory>();
            //oEmployeeSettlementClearances=oEmployeeSettlementClearance.EmployeeSettlementClearances;
            //oEmployeeSettlementClearanceHistorys = oEmployeeSettlementClearance.EmployeeSettlementClearanceHistorys;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (EmployeeSettlementClearance oESC in oEmployeeSettlementClearance.EmployeeSettlementClearances)
                {
                    IDataReader reader;
                    reader = EmployeeSettlementClearanceDA.IUD(tc, oESC, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        EmployeeSettlementClearance oESClearance = new EmployeeSettlementClearance();
                        oESClearance = CreateObject(oReader);
                        oEmployeeSettlementClearances.Add(oESClearance);
                    }
                    reader.Close();
                }
                if (oEmployeeSettlementClearances.Count > 0)
                {
                    int n = 0;
                    foreach (EmployeeSettlementClearanceHistory oESCH in oEmployeeSettlementClearance.EmployeeSettlementClearanceHistorys)
                    {
                        oESCH.ESCID = oEmployeeSettlementClearances[n].ESCID;
                        n++;
                        IDataReader reader;
                        reader = EmployeeSettlementClearanceHistoryDA.IUD(tc, oESCH, nUserID, nDBOperation);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            EmployeeSettlementClearanceHistory oItem = new EmployeeSettlementClearanceHistory();
                            oItem.ESCHID = oReader.GetInt32("ESCHID");
                            oItem.ESCID = oReader.GetInt32("ESCID");
                            oItem.PreviousStatus = (EnumESCrearance)oReader.GetInt16("PreviousStatus");
                            oItem.CurrentStatus = (EnumESCrearance)oReader.GetInt16("CurrentStatus");
                            oItem.Note = oReader.GetString("Note");
                            oEmployeeSettlementClearanceHistorys.Add(oItem);
                        }
                        reader.Close();
                    }
                }
                tc.End();

                oEmployeeSettlementClearance.EmployeeSettlementClearances = new List<EmployeeSettlementClearance>();
                oEmployeeSettlementClearance.EmployeeSettlementClearanceHistorys = new List<EmployeeSettlementClearanceHistory>();
                oEmployeeSettlementClearance.EmployeeSettlementClearances.AddRange(oEmployeeSettlementClearances);
                oEmployeeSettlementClearance.EmployeeSettlementClearanceHistorys.AddRange(oEmployeeSettlementClearanceHistorys);
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSettlementClearance.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSettlementClearance.ESCID = 0;
                #endregion
            }
            return oEmployeeSettlementClearances;
        }

        public EmployeeSettlementClearance Get(int nESCID, Int64 nUserId)
        {
            EmployeeSettlementClearance oEmployeeSettlementClearance = new EmployeeSettlementClearance();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementClearanceDA.Get(nESCID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearance = CreateObject(oReader);
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

                oEmployeeSettlementClearance.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeSettlementClearance;
        }

        public EmployeeSettlementClearance Get(string sSQL, Int64 nUserId)
        {
            EmployeeSettlementClearance oEmployeeSettlementClearance = new EmployeeSettlementClearance();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeSettlementClearanceDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearance = CreateObject(oReader);
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

                oEmployeeSettlementClearance.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeSettlementClearance;
        }

        public List<EmployeeSettlementClearance> Gets(Int64 nUserID)
        {
            List<EmployeeSettlementClearance> oEmployeeSettlementClearance = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementClearanceDA.Gets(tc);
                oEmployeeSettlementClearance = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearance", e);
                #endregion
            }
            return oEmployeeSettlementClearance;
        }

        public List<EmployeeSettlementClearance> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSettlementClearance> oEmployeeSettlementClearance = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSettlementClearanceDA.Gets(sSQL, tc);
                oEmployeeSettlementClearance = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearance", e);
                #endregion
            }
            return oEmployeeSettlementClearance;
        }
        #endregion
    }
}
