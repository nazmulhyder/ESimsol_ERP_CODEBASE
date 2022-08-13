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
   public class LotBaseTestService :MarshalByRefObject ,ILotBaseTestService
    {
        #region Private functions and declaration
        private static LotBaseTest MapObject(NullHandler oReader)
        {
            LotBaseTest oLotBaseTest = new LotBaseTest();
            oLotBaseTest.LotBaseTestID = oReader.GetInt32("LotBaseTestID");
            oLotBaseTest.BUID = oReader.GetInt32("BUID");
            oLotBaseTest.Name = oReader.GetString("Name");
            oLotBaseTest.Activity = oReader.GetBoolean("Activity");
            oLotBaseTest.IsRaw = oReader.GetBoolean("IsRaw");
            return oLotBaseTest;
        }

        public static LotBaseTest CreateObject(NullHandler oReader)
        {
            LotBaseTest oLotBaseTest = MapObject(oReader);
            return oLotBaseTest;
        }

        private List<LotBaseTest> CreateObjects(IDataReader oReader)
        {
            List<LotBaseTest> oLotBaseTests = new List<LotBaseTest>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LotBaseTest oItem = CreateObject(oHandler);
                oLotBaseTests.Add(oItem);
            }
            return oLotBaseTests;
        }

        #endregion
        
        #region Interface implementation
        public LotBaseTestService() { }

        public LotBaseTest IUD(LotBaseTest oLotBaseTest, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                {
                    reader = LotBaseTestDA.IUD(tc, oLotBaseTest, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLotBaseTest = new LotBaseTest();
                        oLotBaseTest = CreateObject(oReader);
                    }
                    reader.Close();

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = LotBaseTestDA.IUD(tc, oLotBaseTest, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oLotBaseTest.ErrorMessage = Global.DeleteMessage;
                }
                else
                {
                    throw new Exception("Invalid Operation In Service");
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLotBaseTest = new LotBaseTest();
                oLotBaseTest.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oLotBaseTest;
        }

        public LotBaseTest Get(int nLotBaseTestID, Int64 nUserId)
        {
            LotBaseTest oLotBaseTest = new LotBaseTest();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LotBaseTestDA.Get(tc, nLotBaseTestID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotBaseTest = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLotBaseTest = new LotBaseTest();
                oLotBaseTest.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oLotBaseTest;
        }

        public List<LotBaseTest> Gets(string sSQL, Int64 nUserID)
        {
            List<LotBaseTest> oLotBaseTests = new List<LotBaseTest>();
            LotBaseTest oLotBaseTest = new LotBaseTest();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LotBaseTestDA.Gets(tc, sSQL);
                oLotBaseTests = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLotBaseTest.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oLotBaseTests.Add(oLotBaseTest);
                #endregion
            }

            return oLotBaseTests;
        }


      

        #endregion
    }
}
