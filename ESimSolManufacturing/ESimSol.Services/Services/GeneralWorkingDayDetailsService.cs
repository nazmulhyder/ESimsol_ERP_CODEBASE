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
    public class GeneralWorkingDayDetailService : MarshalByRefObject, IGeneralWorkingDayDetailService
    {
        #region Private functions and declaration
        private GeneralWorkingDayDetail MapObject(NullHandler oReader)
        {
            GeneralWorkingDayDetail oGeneralWorkingDayDetails = new GeneralWorkingDayDetail();
            oGeneralWorkingDayDetails.GWDDID = oReader.GetInt32("GWDDID");
            oGeneralWorkingDayDetails.GWDID = oReader.GetInt32("GWDID");
            oGeneralWorkingDayDetails.DRPID = oReader.GetInt32("DRPID");
            oGeneralWorkingDayDetails.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oGeneralWorkingDayDetails.LocationID = oReader.GetInt32("LocationID");
            oGeneralWorkingDayDetails.DepartmentID = oReader.GetInt32("DepartmentID");
            oGeneralWorkingDayDetails.BUName = oReader.GetString("BUName");
            oGeneralWorkingDayDetails.LocationName = oReader.GetString("LocationName");
            oGeneralWorkingDayDetails.DepartmentName = oReader.GetString("DepartmentName");
            return oGeneralWorkingDayDetails;
        }

        private GeneralWorkingDayDetail CreateObject(NullHandler oReader)
        {
            GeneralWorkingDayDetail oGeneralWorkingDayDetails = MapObject(oReader);
            return oGeneralWorkingDayDetails;
        }

        private List<GeneralWorkingDayDetail> CreateObjects(IDataReader oReader)
        {
            List<GeneralWorkingDayDetail> oGeneralWorkingDayDetails = new List<GeneralWorkingDayDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GeneralWorkingDayDetail oItem = CreateObject(oHandler);
                oGeneralWorkingDayDetails.Add(oItem);
            }
            return oGeneralWorkingDayDetails;
        }

        #endregion

        #region Interface implementation
        public GeneralWorkingDayDetailService() { }

        //public List<GeneralWorkingDayDetail> Save(GeneralWorkingDayDetail oGeneralWorkingDayDetail, int nDBOperation, Int64 nUserID)
        //{
        //    List<GeneralWorkingDayDetail> oGeneralWorkingDayDetails = new List<GeneralWorkingDayDetail>();
        //    TransactionContext tc = null;
        //    string[] DRPIDs=oGeneralWorkingDayDetail.DRPIDs.Split(',');
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        reader=GeneralWorkingDayDetailDA.Gets("DELETE FROM GeneralWorkingDayDetail WHERE GWDID=" + oGeneralWorkingDayDetail.GWDID, tc);
        //        reader.Close();
        //        tc.End();
        //        tc = TransactionContext.Begin(true);
        //        foreach (string sDRPID in DRPIDs)
        //        {
        //            GeneralWorkingDayDetail oGWDD = new GeneralWorkingDayDetail();
        //            int nDRPID = Convert.ToInt32(sDRPID);
        //            oGWDD.GWDDID = 0;
        //            oGWDD.GWDID = oGeneralWorkingDayDetail.GWDID;
        //            oGWDD.DRPID = nDRPID;

                    
        //            reader = GeneralWorkingDayDetailDA.InsertUpdate(tc, oGWDD, nDBOperation, nUserID);
        //            NullHandler oReader = new NullHandler(reader);
        //            if (reader.Read())
        //            {
        //                oGWDD = new GeneralWorkingDayDetail();
        //                oGWDD = CreateObject(oReader);
        //            }
        //            reader.Close();
        //            oGeneralWorkingDayDetails.Add(oGWDD);
        //        }
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        oGeneralWorkingDayDetail = new GeneralWorkingDayDetail();
        //        oGeneralWorkingDayDetail.ErrorMessage = e.Message.Split('!')[0];
        //        oGeneralWorkingDayDetails = new List<GeneralWorkingDayDetail>();
        //        oGeneralWorkingDayDetails.Add(oGeneralWorkingDayDetail);
        //        #endregion
        //    }
        //    return oGeneralWorkingDayDetails;
        //}

        public List<GeneralWorkingDayDetail> Gets(int id, Int64 nUserID)
        {
            List<GeneralWorkingDayDetail> oGeneralWorkingDayDetails = new List<GeneralWorkingDayDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GeneralWorkingDayDetailDA.Gets(tc, id);
                oGeneralWorkingDayDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GeneralWorkingDayDetails", e);
                #endregion
            }
            return oGeneralWorkingDayDetails;
        }
        #endregion
    }
}
