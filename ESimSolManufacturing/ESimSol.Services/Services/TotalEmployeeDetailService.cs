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
    public class TotalEmployeeDetailService : MarshalByRefObject, ITotalEmployeeDetailService
    {
        #region Private functions and declaration
        private TotalEmployeeDetail MapObject(NullHandler oReader)
        {
            TotalEmployeeDetail oTotalEmployeeDetail = new TotalEmployeeDetail();
            oTotalEmployeeDetail.BUName = oReader.GetString("BUName");
            oTotalEmployeeDetail.LocationName = oReader.GetString("LocationName");
            oTotalEmployeeDetail.DepartmentName = oReader.GetString("DepartmentName");
            oTotalEmployeeDetail.TotalEmp = oReader.GetInt32("TotalEmp");
            oTotalEmployeeDetail.NewEmployee = oReader.GetInt32("NewEmployee");
            oTotalEmployeeDetail.LeftyEmployee = oReader.GetInt32("LeftyEmployee");
            oTotalEmployeeDetail.PFEmployee = oReader.GetInt32("PFEmployee");
            oTotalEmployeeDetail.YetToPFEmployee = oReader.GetInt32("YetToPFEmployee");
            oTotalEmployeeDetail.GratuityEmployee = oReader.GetInt32("GratuityEmployee");
            oTotalEmployeeDetail.Male = oReader.GetInt32("Male");
            oTotalEmployeeDetail.Female = oReader.GetInt32("Female");
            oTotalEmployeeDetail.Permanent = oReader.GetInt32("Permanent");
            oTotalEmployeeDetail.Probationary = oReader.GetInt32("Probationary");
            oTotalEmployeeDetail.Contractual = oReader.GetInt32("Contractual");
            oTotalEmployeeDetail.Seasonal = oReader.GetInt32("Seasonal");

            return oTotalEmployeeDetail;
        }

        private TotalEmployeeDetail CreateObject(NullHandler oReader)
        {
            TotalEmployeeDetail oTotalEmployeeDetail = MapObject(oReader);
            return oTotalEmployeeDetail;
        }

        private List<TotalEmployeeDetail> CreateObjects(IDataReader oReader)
        {
            List<TotalEmployeeDetail> oTotalEmployeeDetail = new List<TotalEmployeeDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TotalEmployeeDetail oItem = CreateObject(oHandler);
                oTotalEmployeeDetail.Add(oItem);
            }
            return oTotalEmployeeDetail;
        }

        #endregion

        #region Interface implementation
        public TotalEmployeeDetailService() { }

        public List<TotalEmployeeDetail> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID)
        {
            List<TotalEmployeeDetail> oTotalEmployeeDetails = new List<TotalEmployeeDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TotalEmployeeDetailDA.Gets(StartDate, EndDate, tc);
                oTotalEmployeeDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TotalEmployeeDetail", e);
                #endregion
            }
            return oTotalEmployeeDetails;
        }
        #endregion
    }
}
