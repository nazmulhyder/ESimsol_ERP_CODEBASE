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
    public class EmployeeSalaryDetailV2Service : MarshalByRefObject, IEmployeeSalaryDetailV2Service
    {
        #region Private functions and declaration
        private EmployeeSalaryDetailV2 MapObject(NullHandler oReader)
        {
            EmployeeSalaryDetailV2 oEmployeeSalaryDetailV2 = new EmployeeSalaryDetailV2();
            oEmployeeSalaryDetailV2.ESDID = oReader.GetInt32("ESDSalarylID");
            oEmployeeSalaryDetailV2.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSalaryDetailV2.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSalaryDetailV2.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeSalaryDetailV2.Amount = oReader.GetDouble("Amount");
            oEmployeeSalaryDetailV2.CompAmount = oReader.GetDouble("CompAmount");
            oEmployeeSalaryDetailV2.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oEmployeeSalaryDetailV2.SalaryHeadType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            oEmployeeSalaryDetailV2.SalaryHeadSequence = oReader.GetInt32("SalaryHeadSequence");
            oEmployeeSalaryDetailV2.SalaryHeadNameInBangla = oReader.GetString("SalaryHeadNameInBangla");
            return oEmployeeSalaryDetailV2;
        }

        private EmployeeSalaryDetailV2 CreateObject(NullHandler oReader)
        {
            EmployeeSalaryDetailV2 oEmployeeSalaryDetailV2 = MapObject(oReader);
            return oEmployeeSalaryDetailV2;
        }

        private List<EmployeeSalaryDetailV2> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailV2 = new List<EmployeeSalaryDetailV2>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalaryDetailV2 oItem = CreateObject(oHandler);
                oEmployeeSalaryDetailV2.Add(oItem);
            }
            return oEmployeeSalaryDetailV2;
        }
        #endregion

        public List<EmployeeSalaryDetailV2> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailV2 = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDetailV2DA.Gets(sSQL, tc);
                oEmployeeSalaryDetailV2 = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeSalaryDetail", e);
                #endregion
            }
            return oEmployeeSalaryDetailV2;
        }
    }
}
