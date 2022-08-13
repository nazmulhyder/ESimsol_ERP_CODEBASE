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
    public class AddressConfigService : MarshalByRefObject, IAddressConfigService
    {
        #region Private functions and declaration
        private AddressConfig MapObject(NullHandler oReader)
        {
            AddressConfig oAddressConfig = new AddressConfig();
            oAddressConfig.AddressConfigID = oReader.GetInt32("AddressConfigID");
            oAddressConfig.AddressType = (EnumConfig_AddressType)oReader.GetInt32("AddressType");
            oAddressConfig.NameInEnglish = oReader.GetString("NameInEnglish");
            oAddressConfig.NameInBangla = oReader.GetString("NameInBangla");
            oAddressConfig.ParentAddressID = oReader.GetInt32("ParentAddressID");
            oAddressConfig.Remarks = oReader.GetString("Remarks");
            oAddressConfig.Code = oReader.GetString("Code");
            return oAddressConfig;

        }

        private AddressConfig CreateObject(NullHandler oReader)
        {
            AddressConfig oAddressConfig = MapObject(oReader);
            return oAddressConfig;
        }

        private List<AddressConfig> CreateObjects(IDataReader oReader)
        {
            List<AddressConfig> oAddressConfig = new List<AddressConfig>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AddressConfig oItem = CreateObject(oHandler);
                oAddressConfig.Add(oItem);
            }
            return oAddressConfig;
        }


        #endregion

        #region Interface implementation
        public AddressConfigService() { }
        public AddressConfig IUD(AddressConfig oAddressConfig, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = AddressConfigDA.IUD(tc, oAddressConfig, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oAddressConfig = new AddressConfig();
                        oAddressConfig = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = AddressConfigDA.IUD(tc, oAddressConfig, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oAddressConfig.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oAddressConfig = new AddressConfig();
                oAddressConfig.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oAddressConfig;
        }


        public AddressConfig Get(string sSQL, Int64 nUserId)
        {
            AddressConfig oAddressConfig = new AddressConfig();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AddressConfigDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAddressConfig = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oAddressConfig;
        }

        public List<AddressConfig> Gets(string sSQL, Int64 nUserID)
        {
            List<AddressConfig> oAddressConfig = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AddressConfigDA.Gets(sSQL, tc);
                oAddressConfig = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAddressConfig;
        }
        #endregion
    }
}

