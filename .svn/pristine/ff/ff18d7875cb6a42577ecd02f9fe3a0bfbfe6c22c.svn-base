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
namespace ESimSol.Services.DataAccess
{
    public class SampleInvoiceChargeService : MarshalByRefObject, ISampleInvoiceChargeService
    {
        #region Private functions and declaration
        private SampleInvoiceCharge MapObject(NullHandler oReader)
        {
            SampleInvoiceCharge oSampleInvoiceCharge = new SampleInvoiceCharge();
            oSampleInvoiceCharge.SampleInvoiceChargeID = oReader.GetInt32("SampleInvoiceChargeID");
            oSampleInvoiceCharge.SampleInvoiceID = oReader.GetInt32("SampleInvoiceID");
            oSampleInvoiceCharge.InoutType = oReader.GetInt32("InoutType");
            oSampleInvoiceCharge.Name = oReader.GetString("Name");
            oSampleInvoiceCharge.Amount = oReader.GetDouble("Amount");
            oSampleInvoiceCharge.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            //oSampleInvoiceCharge.LastUpdateDateTime = Convert.ToDateTime("LastUpdateDateTime");
            oSampleInvoiceCharge.LastUpdateByName = oReader.GetString("LastUpdateByName");
            return oSampleInvoiceCharge;
        }
        private SampleInvoiceCharge CreateObject(NullHandler oReader)
        {
            SampleInvoiceCharge oSampleInvoiceCharge = new SampleInvoiceCharge();
            oSampleInvoiceCharge = MapObject(oReader);
            return oSampleInvoiceCharge;
        }

        private List<SampleInvoiceCharge> CreateObjects(IDataReader oReader)
        {
            List<SampleInvoiceCharge> oSampleInvoiceCharge = new List<SampleInvoiceCharge>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleInvoiceCharge oItem = CreateObject(oHandler);
                oSampleInvoiceCharge.Add(oItem);
            }
            return oSampleInvoiceCharge;
        }
        #endregion
        #region Interface implementation
        public SampleInvoiceCharge Save(SampleInvoiceCharge oSampleInvoiceCharge, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSampleInvoiceCharge.SampleInvoiceChargeID <= 0)
                {

                    reader = SampleInvoiceChargeDA.InsertUpdate(tc, oSampleInvoiceCharge, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SampleInvoiceChargeDA.InsertUpdate(tc, oSampleInvoiceCharge, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceCharge = new SampleInvoiceCharge();
                    oSampleInvoiceCharge = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oSampleInvoiceCharge = new SampleInvoiceCharge();
                    oSampleInvoiceCharge.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oSampleInvoiceCharge;
        }

        public List<SampleInvoiceCharge> SaveList(List<SampleInvoiceCharge> oSampleInvoiceCharges, Int64 nUserID)
        {
            SampleInvoiceCharge oSampleInvoiceCharge = new SampleInvoiceCharge();
            List<SampleInvoiceCharge> oReturnSampleInvoiceCharges = new List<SampleInvoiceCharge>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (SampleInvoiceCharge oTempSampleInvoiceCharge in oSampleInvoiceCharges)
                {
                    #region SampleInvoiceCharge Part
                    IDataReader reader;
                    if (oTempSampleInvoiceCharge.SampleInvoiceChargeID <= 0)
                    {

                        reader = SampleInvoiceChargeDA.InsertUpdate(tc, oTempSampleInvoiceCharge, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {

                        reader = SampleInvoiceChargeDA.InsertUpdate(tc, oTempSampleInvoiceCharge, EnumDBOperation.Update, nUserID);

                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSampleInvoiceCharge = new SampleInvoiceCharge();
                        oSampleInvoiceCharge = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                    oReturnSampleInvoiceCharges.Add(oSampleInvoiceCharge);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oSampleInvoiceCharge = new SampleInvoiceCharge();
                    oSampleInvoiceCharge.ErrorMessage = e.Message;
                    oReturnSampleInvoiceCharges = new List<SampleInvoiceCharge>();
                    oReturnSampleInvoiceCharges.Add(oSampleInvoiceCharge);
                }
            }
            return oReturnSampleInvoiceCharges;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SampleInvoiceCharge oSampleInvoiceCharge = new SampleInvoiceCharge();
                oSampleInvoiceCharge.SampleInvoiceChargeID = id;
                DBTableReferenceDA.HasReference(tc, "SampleInvoiceCharge", id);
                SampleInvoiceChargeDA.Delete(tc, oSampleInvoiceCharge, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public SampleInvoiceCharge Get(int id, Int64 nUserId)
        {
            SampleInvoiceCharge oSampleInvoiceCharge = new SampleInvoiceCharge();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SampleInvoiceChargeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceCharge = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SampleInvoiceCharge", e);
                #endregion
            }
            return oSampleInvoiceCharge;
        }
        public List<SampleInvoiceCharge> Gets(int nSampleInvoiceID,Int64 nUserID)
        {
            List<SampleInvoiceCharge> oSampleInvoiceCharges = new List<SampleInvoiceCharge>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SampleInvoiceChargeDA.Gets(tc, nSampleInvoiceID);
                oSampleInvoiceCharges = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                SampleInvoiceCharge oSampleInvoiceCharge = new SampleInvoiceCharge();
                oSampleInvoiceCharge.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSampleInvoiceCharges;
        }
        public List<SampleInvoiceCharge> Gets(string sSQL, Int64 nUserID)
        {
            List<SampleInvoiceCharge> oSampleInvoiceCharges = new List<SampleInvoiceCharge>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SampleInvoiceChargeDA.Gets(tc, sSQL);
                oSampleInvoiceCharges = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoiceCharge", e);
                #endregion
            }
            return oSampleInvoiceCharges;
        }

        #endregion
    }
}
