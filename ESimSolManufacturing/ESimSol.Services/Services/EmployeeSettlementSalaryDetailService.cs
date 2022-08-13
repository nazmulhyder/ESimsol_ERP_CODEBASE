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
    public class EmployeeSettlementSalaryDetailService : MarshalByRefObject, IEmployeeSettlementSalaryDetailService
    {
        #region Private functions and declaration
        private EmployeeSettlementSalaryDetail MapObject(NullHandler oReader)
        {
            EmployeeSettlementSalaryDetail oEmployeeSettlementSalaryDetail = new EmployeeSettlementSalaryDetail();
            oEmployeeSettlementSalaryDetail.ESDSalarylID = oReader.GetInt32("ESDSalarylID");
            oEmployeeSettlementSalaryDetail.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSettlementSalaryDetail.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeSettlementSalaryDetail.Amount = oReader.GetDouble("Amount");
            oEmployeeSettlementSalaryDetail.CompAmount = oReader.GetDouble("CompAmount");
            oEmployeeSettlementSalaryDetail.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oEmployeeSettlementSalaryDetail.SalaryHeadType = oReader.GetInt16("SalaryHeadType");

            return oEmployeeSettlementSalaryDetail;

        }

        private EmployeeSettlementSalaryDetail CreateObject(NullHandler oReader)
        {
            EmployeeSettlementSalaryDetail oEmployeeSettlementSalaryDetail = MapObject(oReader);
            return oEmployeeSettlementSalaryDetail;
        }

        private List<EmployeeSettlementSalaryDetail> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSettlementSalaryDetail> oEmployeeSettlementSalaryDetail = new List<EmployeeSettlementSalaryDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSettlementSalaryDetail oItem = CreateObject(oHandler);
                oEmployeeSettlementSalaryDetail.Add(oItem);
            }
            return oEmployeeSettlementSalaryDetail;
        }



        #endregion

        #region Interface implementation
        public EmployeeSettlementSalaryDetailService() { }

        public EmployeeSettlementSalaryDetail Get(string sSQL, Int64 nUserId)
        {
            EmployeeSettlementSalaryDetail oEmployeeSettlementSalaryDetail = new EmployeeSettlementSalaryDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementSalaryDetailDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementSalaryDetail = CreateObject(oReader);
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

            return oEmployeeSettlementSalaryDetail;
        }

        public List<EmployeeSettlementSalaryDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSettlementSalaryDetail> oEmployeeSettlementSalaryDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementSalaryDetailDA.Gets(sSQL, tc);
                oEmployeeSettlementSalaryDetail = CreateObjects(reader);
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
            return oEmployeeSettlementSalaryDetail;
        }
        #endregion
    }
}


