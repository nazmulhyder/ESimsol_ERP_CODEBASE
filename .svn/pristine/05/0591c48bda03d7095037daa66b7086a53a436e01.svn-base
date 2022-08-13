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
    public class WorkingHourService : MarshalByRefObject, IWorkingHourService
    {
        #region Private functions and declaration
        private WorkingHour MapObject(NullHandler oReader)
        {
            WorkingHour oWorkingHour = new WorkingHour();

            oWorkingHour.ShiftID = oReader.GetInt32("ShiftID");
            oWorkingHour.ShiftName = oReader.GetString("ShiftName");
            oWorkingHour.TotalEmployee = oReader.GetInt32("TotalEmployee");
            oWorkingHour.TotalPresent = oReader.GetDouble("TotalPresent");
            oWorkingHour.TotalAbsent = oReader.GetDouble("TotalAbsent");
            oWorkingHour.TotalLeave = oReader.GetDouble("TotalLeave");
            oWorkingHour.NormalPresent = oReader.GetDouble("NormalPresent");
            oWorkingHour.OTPresent = oReader.GetDouble("OTPresent");
            oWorkingHour.NormalWorkingHourInMinute = oReader.GetInt32("NormalWorkingHourInMinute");
            oWorkingHour.OTWorkingHourInMinute = oReader.GetInt32("OTWorkingHourInMinute");
            oWorkingHour.TotalWorkingHourInMinute = oReader.GetInt32("TotalWorkingHourInMinute");

            return oWorkingHour;

        }

        private WorkingHour CreateObject(NullHandler oReader)
        {
            WorkingHour oWorkingHour = MapObject(oReader);
            return oWorkingHour;
        }

        private List<WorkingHour> CreateObjects(IDataReader oReader)
        {
            List<WorkingHour> oWorkingHours = new List<WorkingHour>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WorkingHour oItem = CreateObject(oHandler);
                oWorkingHours.Add(oItem);
            }
            return oWorkingHours;
        }

        #endregion

        #region Interface implementation
        public WorkingHourService() { }

        public List<WorkingHour> GetsWorkingHour(string sParam, Int64 nUserId)
        {
            List<WorkingHour> oWorkingHours = new List<WorkingHour>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = WorkingHourDA.GetsWorkingHour(sParam,nUserId, tc);
                oWorkingHours = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                WorkingHour oWorkingHour = new WorkingHour();
                oWorkingHour.ErrorMessage = e.Message;
                #endregion
            }
            return oWorkingHours;
        }
        #endregion
    }
}
