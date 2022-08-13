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
    public class AttendancePerformanceChartService : MarshalByRefObject, IAttendancePerformanceChartService
    {
        #region Private functions and declaration
        private AttendancePerformanceChart MapObject(NullHandler oReader)
        {
            AttendancePerformanceChart oAttendanceSummery = new AttendancePerformanceChart();

            oAttendanceSummery.Month = oReader.GetDateTime("MonthDate");
            oAttendanceSummery.PresentPercent = oReader.GetDouble("PresentPercent");
            return oAttendanceSummery;
        }

        private AttendancePerformanceChart CreateObject(NullHandler oReader)
        {
            AttendancePerformanceChart oAttendanceSummery = MapObject(oReader);
            return oAttendanceSummery;
        }

        private List<AttendancePerformanceChart> CreateObjects(IDataReader oReader)
        {
            List<AttendancePerformanceChart> oAttendanceSummery = new List<AttendancePerformanceChart>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendancePerformanceChart oItem = CreateObject(oHandler);
                oAttendanceSummery.Add(oItem);
            }
            return oAttendanceSummery;
        }

        #endregion

        #region Interface implementation
        public AttendancePerformanceChartService() { }

        public AttendancePerformanceChart Get( Int64 nUserID)
        {
            List<AttendancePerformanceChart> oAttendancePerformances = new List<AttendancePerformanceChart>();
            AttendancePerformanceChart oAP = new  AttendancePerformanceChart();
            oAP.List1 = new List<AttendancePerformanceChart> ();
            oAP.List2 = new List<AttendancePerformanceChart> ();
            oAP.List3 = new List<AttendancePerformanceChart> ();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                int n= Convert.ToInt32(DateTime.Now.ToString("yyyy"))-2;
                for (int i = 0; i < 3; i++)
                {
                    
                    IDataReader reader = AttendancePerformanceChartDA.Gets(n,tc);
                    NullHandler oReader = new NullHandler(reader);
                    oAttendancePerformances = CreateObjects(reader);
                    reader.Close();
                    n = n + 1;
                    if(i==0)oAP.List1.AddRange(oAttendancePerformances);
                    else if (i == 1) oAP.List2.AddRange(oAttendancePerformances);
                    else if (i == 2) oAP.List3.AddRange(oAttendancePerformances);
                    oAttendancePerformances = new List<AttendancePerformanceChart>();
                    
                }
                tc.End();


            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendancePerformance", e);
                #endregion
            }
            return oAP;
        }


        #endregion

    }
}
