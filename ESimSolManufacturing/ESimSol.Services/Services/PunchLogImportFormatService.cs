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
    public class PunchLogImportFormatService : MarshalByRefObject, IPunchLogImportFormatService
    {
        #region Private functions and declaration
        private PunchLogImportFormat MapObject(NullHandler oReader)
        {
            PunchLogImportFormat oPunchLogImportFormat = new PunchLogImportFormat();

            oPunchLogImportFormat.PLIFID = oReader.GetInt32("PLIFID");
            oPunchLogImportFormat.PunchFormat = (EnumPunchFormat)oReader.GetInt16("PunchFormat");

            return oPunchLogImportFormat;

        }

        private PunchLogImportFormat CreateObject(NullHandler oReader)
        {
            PunchLogImportFormat oPunchLogImportFormat = MapObject(oReader);
            return oPunchLogImportFormat;
        }

        private List<PunchLogImportFormat> CreateObjects(IDataReader oReader)
        {
            List<PunchLogImportFormat> oPunchLogImportFormats = new List<PunchLogImportFormat>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PunchLogImportFormat oItem = CreateObject(oHandler);
                oPunchLogImportFormats.Add(oItem);
            }
            return oPunchLogImportFormats;
        }

        #endregion

        #region Interface implementation
        public PunchLogImportFormatService() { }

        public PunchLogImportFormat IUD(PunchLogImportFormat oPunchLogImportFormat, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PunchLogImportFormatDA.IUD(tc, oPunchLogImportFormat, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPunchLogImportFormat = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPunchLogImportFormat.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oPunchLogImportFormat.PLIFID = 0;
                #endregion
            }
            return oPunchLogImportFormat;
        }

        public List<PunchLogImportFormat> Gets(Int64 nUserID)
        {
            List<PunchLogImportFormat> oPunchLogImportFormat = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PunchLogImportFormatDA.Gets(tc);
                oPunchLogImportFormat = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PunchLogImportFormat", e);
                #endregion
            }
            return oPunchLogImportFormat;
        }

        #endregion


    }
}
