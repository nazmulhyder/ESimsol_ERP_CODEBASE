using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.Services
{
    public class SalaryFieldSetupDetailService : MarshalByRefObject, ISalaryFieldSetupDetailService
    {
        #region Private functions and declaration
        private SalaryFieldSetupDetail MapObject(NullHandler oReader)
        {
            SalaryFieldSetupDetail oSalaryFieldSetupDetail = new SalaryFieldSetupDetail();
            oSalaryFieldSetupDetail.SalaryFieldSetupDetailID = oReader.GetInt32("SalaryFieldSetupDetailID");
            oSalaryFieldSetupDetail.SalaryFieldSetupID = oReader.GetInt32("SalaryFieldSetupID");
            oSalaryFieldSetupDetail.SalaryField = (EnumSalaryField)oReader.GetInt32("SalaryField");
            oSalaryFieldSetupDetail.Remarks = oReader.GetString("Remarks");
            oSalaryFieldSetupDetail.DBUserID = oReader.GetInt32("DBUserID");
            oSalaryFieldSetupDetail.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oSalaryFieldSetupDetail.UserName = oReader.GetString("UserName");
            oSalaryFieldSetupDetail.LogInID = oReader.GetString("LogInID");
            return oSalaryFieldSetupDetail;
        }

        private List<SalaryFieldSetupDetail> CreateObjects(IDataReader oReader)
        {
            List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalaryFieldSetupDetail oType = CreateObject(oHandler);
                oSalaryFieldSetupDetails.Add(oType);
            }
            return oSalaryFieldSetupDetails;
        }

        private SalaryFieldSetupDetail CreateObject(NullHandler oReader)
        {
            SalaryFieldSetupDetail oSalaryFieldSetupDetail = new SalaryFieldSetupDetail();
            oSalaryFieldSetupDetail = MapObject(oReader);
            return oSalaryFieldSetupDetail;
        }
        #endregion

        #region Interface implementation
        public SalaryFieldSetupDetailService() { }

        public List<SalaryFieldSetupDetail> Gets(int id, Int64 nUserID)
        {
            List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalaryFieldSetupDetailDA.Gets(tc, id);
                oSalaryFieldSetupDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                SalaryFieldSetupDetail oSalaryFieldSetupDetail = new SalaryFieldSetupDetail();
                oSalaryFieldSetupDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }

            return oSalaryFieldSetupDetails;
        }
        #endregion
    }
}
