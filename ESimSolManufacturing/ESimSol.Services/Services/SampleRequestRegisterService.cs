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
    class SampleRequestRegisterService: MarshalByRefObject,ISampleRequestRegisterService
    {
        private SampleRequestRegister MapObject(NullHandler oReader)
        {
            SampleRequestRegister oSampleRequestRegister = new SampleRequestRegister();
            oSampleRequestRegister.SampleRequestID = oReader.GetInt32("SampleRequestID");
            oSampleRequestRegister.RequestDate = oReader.GetDateTime("RequestDate");
            oSampleRequestRegister.RequestBy = oReader.GetInt32("RequestBy");
            oSampleRequestRegister.BUID = oReader.GetInt32("BUID");
            oSampleRequestRegister.Remarks = oReader.GetString("Remarks");
            oSampleRequestRegister.RequestNo = oReader.GetString("RequestNo");
            oSampleRequestRegister.RequestTo = oReader.GetInt32("RequestTo");
            oSampleRequestRegister.ContractorID = oReader.GetInt32("ContractorID");
            oSampleRequestRegister.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oSampleRequestRegister.ErrorMessage = oReader.GetString("ErrorMessage");
            oSampleRequestRegister.SampleRequestDetailID = oReader.GetInt32("SampleRequestRegisterID");
            oSampleRequestRegister.ProductID = oReader.GetInt32("ProductID");
            oSampleRequestRegister.ColorCategoryID = oReader.GetInt32("ColorCategoryID");
            oSampleRequestRegister.MUnitID = oReader.GetInt32("MUnitID");
            oSampleRequestRegister.SampleRequestDetailRemarks = oReader.GetString("SampleRequestDetailRemarks");
            oSampleRequestRegister.ColorName = oReader.GetString("ColorName");
            oSampleRequestRegister.ProductName = oReader.GetString("ProductName");
            oSampleRequestRegister.UnitName = oReader.GetString("UnitName");
            oSampleRequestRegister.ContractorName = oReader.GetString("ContractorName");
            oSampleRequestRegister.ContactPersonName = oReader.GetString("ContactPersonName");
            oSampleRequestRegister.Quantity = oReader.GetDouble("Quantity");
            return oSampleRequestRegister;
        }
        private SampleRequestRegister CreateObject(NullHandler oReader)
        {
            SampleRequestRegister oSampleRequestRegister = new SampleRequestRegister();
            oSampleRequestRegister = MapObject(oReader);
            return oSampleRequestRegister;
        }
        private List<SampleRequestRegister> CreateObjects(IDataReader oReader)
        {
            List<SampleRequestRegister> oSampleRequestRegisters = new List<SampleRequestRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleRequestRegister oItem = CreateObject(oHandler);
                oSampleRequestRegisters.Add(oItem);
            }
            return oSampleRequestRegisters;
        }
        #region Implement Interface
        public List<SampleRequestRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<SampleRequestRegister> oSampleRequestRegisters = new List<SampleRequestRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SampleRequestRegisterDA.Gets(tc, sSQL);
                oSampleRequestRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleRequestRegister", e);
                #endregion
            }
            return oSampleRequestRegisters;
        }
        #endregion
    }
}
