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
    public class AttendanceAccessPointService : MarshalByRefObject, IAttendanceAccessPointService
    {
        #region Private functions and declaration
        private static AttendanceAccessPoint MapObject(NullHandler oReader)
        {
            AttendanceAccessPoint oAttendanceAccessPoint = new AttendanceAccessPoint();
            oAttendanceAccessPoint.AAPID = oReader.GetInt32("AAPID");
            oAttendanceAccessPoint.Name = oReader.GetString("Name");
            oAttendanceAccessPoint.MachineSLNo = oReader.GetString("MachineSLNo");
            oAttendanceAccessPoint.Note = oReader.GetString("Note");
            oAttendanceAccessPoint.DataProvider =(EnumDataProvider) oReader.GetInt16("DataProvider");
            oAttendanceAccessPoint.DataProviderInInt = oReader.GetInt32("DataProvider");
            oAttendanceAccessPoint.DataSource = oReader.GetString("DataSource");
            oAttendanceAccessPoint.DBLoginID = oReader.GetString("DBLoginID");
            oAttendanceAccessPoint.DBPassword = oReader.GetString("DBPassword");
            oAttendanceAccessPoint.DBName = oReader.GetString("DBName");
            oAttendanceAccessPoint.IsThisPC = oReader.GetBoolean("IsThisPC");
            oAttendanceAccessPoint.IsActive = oReader.GetBoolean("IsActive");
            oAttendanceAccessPoint.Query = oReader.GetString("Query");
            return oAttendanceAccessPoint;

        }

        public static AttendanceAccessPoint CreateObject(NullHandler oReader)
        {
            AttendanceAccessPoint oAttendanceAccessPoint = new AttendanceAccessPoint();
            oAttendanceAccessPoint = MapObject(oReader);
            return oAttendanceAccessPoint;
        }

        private List<AttendanceAccessPoint> CreateObjects(IDataReader oReader)
        {
            List<AttendanceAccessPoint> oAttendanceAccessPoints = new List<AttendanceAccessPoint>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceAccessPoint oItem = CreateObject(oHandler);
                oAttendanceAccessPoints.Add(oItem);
            }
            return oAttendanceAccessPoints;
        }

        #endregion

        #region Interface implementation
        public AttendanceAccessPointService() { }

        public AttendanceAccessPoint IUD(AttendanceAccessPoint oAttendanceAccessPoint, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceAccessPointDA.IUD(tc, oAttendanceAccessPoint, nDBOperation, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceAccessPoint = new AttendanceAccessPoint();
                    oAttendanceAccessPoint = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oAttendanceAccessPoint = new AttendanceAccessPoint();
                    oAttendanceAccessPoint.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oAttendanceAccessPoint.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oAttendanceAccessPoint;
        }

        public AttendanceAccessPoint Get(int nAAPID, Int64 nUserId)
        {
            AttendanceAccessPoint oAttendanceAccessPoint = new AttendanceAccessPoint();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceAccessPointDA.Get(tc, nAAPID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceAccessPoint = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oAttendanceAccessPoint.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oAttendanceAccessPoint;
        }

        public List<AttendanceAccessPoint> Gets(string sSQL, Int64 nUserId)
        {
            List<AttendanceAccessPoint> oAttendanceAccessPoints = new List<AttendanceAccessPoint>();
            AttendanceAccessPoint oAttendanceAccessPoint = new AttendanceAccessPoint();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceAccessPointDA.Gets(tc, sSQL);
                oAttendanceAccessPoints = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oAttendanceAccessPoint.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oAttendanceAccessPoints.Add(oAttendanceAccessPoint);
                #endregion
            }

            return oAttendanceAccessPoints;
        }
        #endregion
    }
}
