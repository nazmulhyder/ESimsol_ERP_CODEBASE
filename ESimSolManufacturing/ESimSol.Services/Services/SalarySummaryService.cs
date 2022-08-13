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
    public class SalarySummaryService : MarshalByRefObject, ISalarySummaryService
    {
        #region Private functions and declaration
        private SalarySummary MapObject(NullHandler oReader)
        {
            SalarySummary oSalarySummary = new SalarySummary();
            oSalarySummary.DepartmentID = oReader.GetInt32("DepartmentID");
            oSalarySummary.DepartmentName = oReader.GetString("DepartmentName");
            oSalarySummary.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oSalarySummary.SalaryHeadType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            oSalarySummary.Amount = oReader.GetDouble("Amount");
            return oSalarySummary;
        }

        private SalarySummary CreateObject(NullHandler oReader)
        {
            SalarySummary oSalarySummary = MapObject(oReader);
            return oSalarySummary;
        }

        private List<SalarySummary> CreateObjects(IDataReader oReader)
        {
            List<SalarySummary> oSalarySummary = new List<SalarySummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySummary oItem = CreateObject(oHandler);
                oSalarySummary.Add(oItem);
            }
            return oSalarySummary;
        }

        #endregion

        #region Interface implementation
        public SalarySummaryService() { }

        public List<SalarySummary> Gets(DateTime StartDate, DateTime EndDate, bool IsDateSearch, int nLocationID, Int64 nUserId)
        {
            SalarySummary oSalarySummary = new SalarySummary();
            List<SalarySummary> oSalarySummarys = new List<SalarySummary>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalarySummaryDA.Gets(StartDate, EndDate, IsDateSearch, nLocationID, tc);
                oSalarySummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oSalarySummarys = new List<SalarySummary>();
                oSalarySummary.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oSalarySummarys.Add(oSalarySummary);
                #endregion
            }

            return oSalarySummarys;
        }

        #endregion
    }
}
