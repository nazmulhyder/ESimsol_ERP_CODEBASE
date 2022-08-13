using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class OTExceptComplianceService : MarshalByRefObject, IOTExceptComplianceService
    {
        #region Private functions and declaration
        private OTExceptCompliance MapObject(NullHandler oReader)
        {
            OTExceptCompliance oOTExceptCompliance = new OTExceptCompliance();
            oOTExceptCompliance.EmployeeID = oReader.GetInt32("EmployeeID");
            oOTExceptCompliance.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oOTExceptCompliance.LocationID = oReader.GetInt32("LocationID");
            oOTExceptCompliance.GrossAmount = oReader.GetDouble("GrossAmount");
            oOTExceptCompliance.CompGrossAmount = oReader.GetDouble("CompGrossAmount");
            oOTExceptCompliance.CompOTRatePerHour = oReader.GetDouble("CompOTRatePerHour");
            oOTExceptCompliance.BasicAmount = oReader.GetDouble("BasicAmount");
            oOTExceptCompliance.OTRatePerHour = oReader.GetDouble("OTRatePerHour");
            oOTExceptCompliance.OTInMinute = oReader.GetDouble("OTInMinute");
            oOTExceptCompliance.CompOTInMinute = oReader.GetDouble("CompOTInMinute");
            oOTExceptCompliance.AdditionalOTInMinute = oReader.GetDouble("AdditionalOTInMinute");
            oOTExceptCompliance.Name = oReader.GetString("Name");
            oOTExceptCompliance.BusinessUnitAddress = oReader.GetString("BusinessUnitAddress");
            oOTExceptCompliance.Code = oReader.GetString("Code");
            oOTExceptCompliance.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oOTExceptCompliance.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oOTExceptCompliance.LocationName = oReader.GetString("LocationName");
            oOTExceptCompliance.DepartmentName = oReader.GetString("DepartmentName");
            oOTExceptCompliance.DesignationName = oReader.GetString("DesignationName");
            return oOTExceptCompliance;
        }

        public static OTExceptCompliance CreateObject(NullHandler oReader)
        {
            OTExceptCompliance oOTExceptCompliance = new OTExceptCompliance();
            OTExceptComplianceService oService = new OTExceptComplianceService();
            oOTExceptCompliance = oService.MapObject(oReader);
            return oOTExceptCompliance;
        }
        private List<OTExceptCompliance> CreateObjects(IDataReader oReader)
        {
            List<OTExceptCompliance> oOTExceptCompliances = new List<OTExceptCompliance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OTExceptCompliance oItem = CreateObject(oHandler);
                oOTExceptCompliances.Add(oItem);
            }
            return oOTExceptCompliances;
        }

        #endregion

        #region Interface implementation
        public OTExceptComplianceService() { }

        public List<OTExceptCompliance> Gets(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sEmpIDs, string sBMMIDs, int nPayType, int nMonthID, int nYear, bool bNewJoin, bool bExceptComp, long nUserID)
        {
            List<OTExceptCompliance> oOTExceptCompliances = new List<OTExceptCompliance>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = OTExceptComplianceDA.Gets(tc, sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sEmpIDs, sBMMIDs, nPayType, nMonthID, nYear, bNewJoin, bExceptComp);
                oOTExceptCompliances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                OTExceptCompliance oOTExceptCompliance = new OTExceptCompliance();
                oOTExceptCompliance.ErrorMessage = e.Message;
                oOTExceptCompliances.Add(oOTExceptCompliance);
                #endregion
            }

            return oOTExceptCompliances;
        }
        #endregion
    }
}