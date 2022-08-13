using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
namespace ESimSol.Services.Services
{
    public class MeasurementUnitBUService :MarshalByRefObject,IMeasurementUnitBUService
    {
        #region Private functions and declaration

        private   MeasurementUnitBU MapObject(NullHandler oReader)
        {
              MeasurementUnitBU oMeasurementUnitBU = new   MeasurementUnitBU();
              oMeasurementUnitBU.BUID = oReader.GetInt32("BUID");
              oMeasurementUnitBU.MeasurementUnitConID =oReader.GetInt32("MeasurementUnitConID");
              oMeasurementUnitBU.BussinessUnitName = oReader.GetString("BUName");
              return oMeasurementUnitBU;
        }

        private MeasurementUnitBU CreateObject(NullHandler oReader)
        {
              MeasurementUnitBU oMeasurementUnitBU = new   MeasurementUnitBU();
                oMeasurementUnitBU= MapObject(oReader);
                return oMeasurementUnitBU;
        }

        private List<MeasurementUnitBU> CreateObjects(IDataReader oReader)
        {
            List<  MeasurementUnitBU> oMeasurementUnitBU = new List<  MeasurementUnitBU>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                  MeasurementUnitBU oItem = CreateObject(oHandler);
                oMeasurementUnitBU.Add(oItem);
            }
            return oMeasurementUnitBU;
        }

        #endregion

        #region Interface implementation
        public MeasurementUnitBUService() { }
        public List< MeasurementUnitBU> Gets(string sSQL, Int64 nUserID)
        {
            List< MeasurementUnitBU> oMeasurementUnitBUs = new List< MeasurementUnitBU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader =  MeasurementUnitBUDA.Gets(tc, sSQL);
                oMeasurementUnitBUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get  MeasurementUnitBU", e);
                #endregion
            }
            return oMeasurementUnitBUs;
        }

        #endregion
    }
}
