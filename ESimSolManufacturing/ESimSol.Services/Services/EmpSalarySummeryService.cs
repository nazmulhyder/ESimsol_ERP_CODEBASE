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
    public class EmpSalarySummeryService : MarshalByRefObject, IEmpSalarySummeryService
    {
        #region Private functions and declaration
        private EmpSalarySummery MapObject(NullHandler oReader)
        {
            EmpSalarySummery oSalarySummery = new EmpSalarySummery();

            oSalarySummery.DepartmentID = oReader.GetInt32("DepartmentID");
            oSalarySummery.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oSalarySummery.DepartmentName = oReader.GetString("DepartmentName");
            oSalarySummery.NoOfEmp = oReader.GetInt32("NoOfEmp");
            oSalarySummery.ProductionAmount = oReader.GetDouble("ProductionAmount");
            oSalarySummery.ProductionBonus = oReader.GetDouble("ProductionBonus");
            oSalarySummery.AttBonus = oReader.GetDouble("AttBonus");
            oSalarySummery.OTAmount = oReader.GetDouble("OTAmount");
            oSalarySummery.LeaveAllw = oReader.GetDouble("LeaveAllw");
            oSalarySummery.ShortFall = oReader.GetDouble("ShortFall");
            oSalarySummery.TotalNoWorkDayAllowance = oReader.GetDouble("TotalNoWorkDayAllowance");
            oSalarySummery.AdvPayment = oReader.GetDouble("AdvPayment");
            oSalarySummery.RStamp = oReader.GetDouble("RStamp");
            return oSalarySummery;

        }

        private EmpSalarySummery CreateObject(NullHandler oReader)
        {
            EmpSalarySummery oSalarySummery = MapObject(oReader);
            return oSalarySummery;
        }

        private List<EmpSalarySummery> CreateObjects(IDataReader oReader)
        {
            List<EmpSalarySummery> oSalarySummerys = new List<EmpSalarySummery>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmpSalarySummery oItem = CreateObject(oHandler);
                oSalarySummerys.Add(oItem);
            }
            return oSalarySummerys;
        }

        #endregion

        #region Interface implementation
        public EmpSalarySummeryService() { }
        public List<EmpSalarySummery> Gets(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserId)
        {
            List<EmpSalarySummery> oSalarySummerys = new List<EmpSalarySummery>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmpSalarySummeryDA.Gets(sEmpIDs, StartDate, EndDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, tc);
                oSalarySummerys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalarySummery", e);
                #endregion
            }
            return oSalarySummerys;
        }

        public List<EmpSalarySummery> Gets_Compliance(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserId)
        {
            List<EmpSalarySummery> oSalarySummerys = new List<EmpSalarySummery>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmpSalarySummeryDA.Gets_Compliance(sEmpIDs, StartDate, EndDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, tc);
                oSalarySummerys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalarySummery", e);
                #endregion
            }
            return oSalarySummerys;
        }

        #endregion


    }
}
