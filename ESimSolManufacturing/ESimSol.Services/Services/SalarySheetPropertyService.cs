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
    public class SalarySheetPropertyService : MarshalByRefObject, ISalarySheetPropertyService
    {
        #region Private functions and declaration
        private SalarySheetProperty MapObject(NullHandler oReader)
        {
            SalarySheetProperty oSalarySheetProperty = new SalarySheetProperty();
            oSalarySheetProperty.SalarySheetPropertyID = oReader.GetInt32("SalarySheetPropertyID");
            oSalarySheetProperty.SalarySheetFormatProperty = (EnumSalarySheetFormatProperty)oReader.GetInt16("SalarySheetFormatProperty");
            oSalarySheetProperty.SalarySheetFormatPropertyInt = (int)(EnumSalarySheetFormatProperty)oReader.GetInt16("SalarySheetFormatProperty");
            oSalarySheetProperty.PropertyFor = oReader.GetInt16("PropertyFor");
            oSalarySheetProperty.IsActive = oReader.GetBoolean("IsActive");
            return oSalarySheetProperty;
        }

        public static SalarySheetProperty CreateObject(NullHandler oReader)
        {
            SalarySheetProperty oSalarySheetProperty = new SalarySheetProperty();
            SalarySheetPropertyService oService = new SalarySheetPropertyService();
            oSalarySheetProperty = oService.MapObject(oReader);
            return oSalarySheetProperty;
        }
        private List<SalarySheetProperty> CreateObjects(IDataReader oReader)
        {
            List<SalarySheetProperty> oSalarySheetPropertys = new List<SalarySheetProperty>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySheetProperty oItem = CreateObject(oHandler);
                oSalarySheetPropertys.Add(oItem);
            }
            return oSalarySheetPropertys;
        }

        #endregion

        #region Interface implementation
        public SalarySheetPropertyService() { }

        public SalarySheetProperty IUD(SalarySheetProperty oSalarySheetProperty, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalarySheetPropertyDA.IUD(tc, oSalarySheetProperty, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalarySheetProperty = new SalarySheetProperty();
                    oSalarySheetProperty = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oSalarySheetProperty = new SalarySheetProperty(); oSalarySheetProperty.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oSalarySheetProperty.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oSalarySheetProperty;
        }

        public SalarySheetProperty Get(int nSalarySheetPropertyID, long nUserID)
        {
            SalarySheetProperty oSalarySheetProperty = new SalarySheetProperty();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalarySheetPropertyDA.Get(tc, nSalarySheetPropertyID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalarySheetProperty = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oSalarySheetProperty.ErrorMessage = e.Message;
                #endregion
            }

            return oSalarySheetProperty;
        }

        public List<SalarySheetProperty> Gets(string sSQL, long nUserID)
        {
            List<SalarySheetProperty> oSalarySheetPropertys = new List<SalarySheetProperty>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySheetPropertyDA.Gets(tc, sSQL);
                oSalarySheetPropertys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                SalarySheetProperty oSalarySheetProperty = new SalarySheetProperty();
                oSalarySheetProperty.ErrorMessage = e.Message;
                oSalarySheetPropertys.Add(oSalarySheetProperty);
                #endregion
            }

            return oSalarySheetPropertys;
        }


        #endregion
    }
}