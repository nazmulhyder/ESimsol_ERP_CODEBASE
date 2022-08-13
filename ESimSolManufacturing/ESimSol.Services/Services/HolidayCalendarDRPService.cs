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
    public class HolidayCalendarDRPService : MarshalByRefObject, IHolidayCalendarDRPService
    {
        private HolidayCalendarDRP MapObject(NullHandler oReader)
        {
            HolidayCalendarDRP oHolidayCalendarDRP = new HolidayCalendarDRP();
            oHolidayCalendarDRP.HolidayCalendarDRPID = oReader.GetInt32("HolidayCalendarDRPID");
            oHolidayCalendarDRP.HolidayCalendarDetailID = oReader.GetInt32("HolidayCalendarDetailID");
            oHolidayCalendarDRP.DRPID = oReader.GetInt32("DRPID");
            oHolidayCalendarDRP.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oHolidayCalendarDRP.LocationID = oReader.GetInt32("LocationID");
            oHolidayCalendarDRP.DepartmentID = oReader.GetInt32("DepartmentID");
            oHolidayCalendarDRP.BUName = oReader.GetString("BUName");
            oHolidayCalendarDRP.Location = oReader.GetString("Location");
            oHolidayCalendarDRP.Department = oReader.GetString("Department");
            return oHolidayCalendarDRP;
        }
        private HolidayCalendarDRP CreateObject(NullHandler oReader)
        {
            HolidayCalendarDRP oHolidayCalendarDRP = new HolidayCalendarDRP();
            oHolidayCalendarDRP = MapObject(oReader);
            return oHolidayCalendarDRP;
        }

        private List<HolidayCalendarDRP> CreateObjects(IDataReader oReader)
        {
            List<HolidayCalendarDRP> oHolidayCalendarDRP = new List<HolidayCalendarDRP>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HolidayCalendarDRP oItem = CreateObject(oHandler);
                oHolidayCalendarDRP.Add(oItem);
            }
            return oHolidayCalendarDRP;
        }
        public List<HolidayCalendarDRP> Gets(int id, Int64 nUserID)
        {
            List<HolidayCalendarDRP> oHolidayCalendarDRPs = new List<HolidayCalendarDRP>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = HolidayCalendarDRPDA.Gets(tc, id);
                oHolidayCalendarDRPs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                HolidayCalendarDRP oHolidayCalendarDRP = new HolidayCalendarDRP();
                oHolidayCalendarDRP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oHolidayCalendarDRPs;
        }

    }
 
}
