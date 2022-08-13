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
    public class MgtDBObjService : MarshalByRefObject, IMgtDBObjService
    {
        #region Private functions and declaration

        private MgtDBObj MapObject(NullHandler oReader)
        {
            MgtDBObj oMgtDBObj = new MgtDBObj();
            oMgtDBObj.TempID = oReader.GetInt32("TempID");
            oMgtDBObj.BUID = oReader.GetInt32("BUID");
            oMgtDBObj.StartDate = oReader.GetDateTime("StartDate");
            oMgtDBObj.EndDate = oReader.GetDateTime("EndDate");
            oMgtDBObj.RefType = (EnumMgtDBRefType)oReader.GetInt32("RefType");
            oMgtDBObj.RefTypeInt = oReader.GetInt32("RefType");
            oMgtDBObj.RefValueType = (EnumMgtDBRefValueType)oReader.GetInt32("RefValueType");
            oMgtDBObj.RefValueTypeInt = oReader.GetInt32("RefValueType");
            oMgtDBObj.RefValueID = oReader.GetInt32("RefValueID");
            oMgtDBObj.RefCaption = oReader.GetString("RefCaption");
            oMgtDBObj.RefQty = oReader.GetDouble("RefQty");
            oMgtDBObj.RefAmount = oReader.GetDouble("RefAmount");
            oMgtDBObj.ExportPIAmount = oReader.GetDouble("ExportPIAmount");
            oMgtDBObj.LCRcvAmount = oReader.GetDouble("LCRcvAmount");
            oMgtDBObj.ExportRecevableAmount = oReader.GetDouble("ExportRecevableAmount");
            oMgtDBObj.ImportPayableAmount = oReader.GetDouble("ImportPayableAmount");
            oMgtDBObj.MUnitID = oReader.GetInt32("MUnitID");
            oMgtDBObj.MUnit = oReader.GetString("MUnit");
            oMgtDBObj.CurrencyID = oReader.GetInt32("CurrencyID");
            oMgtDBObj.Currency = oReader.GetString("Currency");
            return oMgtDBObj;
        }

        private MgtDBObj CreateObject(NullHandler oReader)
        {
            MgtDBObj oMgtDBObj = new MgtDBObj();
            oMgtDBObj = MapObject(oReader);
            return oMgtDBObj;
        }

        private List<MgtDBObj> CreateObjects(IDataReader oReader)
        {
            List<MgtDBObj> oMgtDBObj = new List<MgtDBObj>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MgtDBObj oItem = CreateObject(oHandler);
                oMgtDBObj.Add(oItem);
            }
            return oMgtDBObj;
        }

        #endregion

        #region Interface implementation
        public List<MgtDBObj> Gets(MgtDBObj oMgtDBObj, Int64 nUserID)
        {
            List<MgtDBObj> oMgtDBObjs = new List<MgtDBObj>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MgtDBObjDA.Gets(tc, oMgtDBObj);
                oMgtDBObjs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MgtDBObj", e);
                #endregion
            }
            return oMgtDBObjs;
        }

        #endregion
    }

}
