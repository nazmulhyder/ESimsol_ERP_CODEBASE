using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class EmployeeBatchDetailService :  MarshalByRefObject, IEmployeeBatchDetailService
    {
        private EmployeeBatchDetail MapObject(NullHandler oReader)
        {
            EmployeeBatchDetail oEmployeeBatchDetail = new EmployeeBatchDetail();
            oEmployeeBatchDetail.EmployeeBatchDetailID = oReader.GetInt32("EmployeeBatchDetailID");
            oEmployeeBatchDetail.EmployeeBatchID = oReader.GetInt32("EmployeeBatchID");
            oEmployeeBatchDetail.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeBatchDetail.Location = oReader.GetString("Location");
            oEmployeeBatchDetail.Department = oReader.GetString("Department");
            oEmployeeBatchDetail.Designation = oReader.GetString("Designation");
            oEmployeeBatchDetail.ShiftName = oReader.GetString("ShiftName");
            oEmployeeBatchDetail.AttendanceScheme = oReader.GetString("AttendanceScheme");
            oEmployeeBatchDetail.SalaryScheme = oReader.GetString("SalaryScheme");
            oEmployeeBatchDetail.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oEmployeeBatchDetail.GrossAmount = oReader.GetDouble("GrossAmount");
            oEmployeeBatchDetail.ComplianceGross = oReader.GetDouble("ComplianceGross");
            oEmployeeBatchDetail.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeBatchDetail.EmployeeCode = oReader.GetString("EmployeeCode");
            return oEmployeeBatchDetail;
        }
        private EmployeeBatchDetail CreateObject(NullHandler oReader)
        {
            EmployeeBatchDetail oEmployeeBatchDetail = new EmployeeBatchDetail();
            oEmployeeBatchDetail = MapObject(oReader);
            return oEmployeeBatchDetail;
        }

        private List<EmployeeBatchDetail> CreateObjects(IDataReader oReader)
        {
            List<EmployeeBatchDetail> oEmployeeBatchDetail = new List<EmployeeBatchDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeBatchDetail oItem = CreateObject(oHandler);
                oEmployeeBatchDetail.Add(oItem);
            }
            return oEmployeeBatchDetail;
        }
        public List<EmployeeBatchDetail> Gets(int id, Int64 nUserID)
        {
            List<EmployeeBatchDetail> oEmployeeBatchDetails = new List<EmployeeBatchDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeBatchDetailDA.Gets(tc, id);
                oEmployeeBatchDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                EmployeeBatchDetail oEmployeeBatchDetail = new EmployeeBatchDetail();
                oEmployeeBatchDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oEmployeeBatchDetails;
        }

        public List<EmployeeBatchDetail> ArchiveSalaryChnage(ArchiveSalaryStruc oArchiveSalaryStruc, Int64 nUserID)
        {
            List<EmployeeBatchDetail> oEmployeeBatchDetails = new List<EmployeeBatchDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeBatchDetailDA.ArchiveSalaryChnage(tc, oArchiveSalaryStruc, nUserID);
                oEmployeeBatchDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                EmployeeBatchDetail oEmployeeBatchDetail = new EmployeeBatchDetail();
                oEmployeeBatchDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oEmployeeBatchDetails;
        }

    }
 
}
