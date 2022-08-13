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
    public class ExportPIPrintSetupService : MarshalByRefObject, IExportPIPrintSetupService
    {
        #region Private functions and declaration
        private ExportPIPrintSetup MapObject(NullHandler oReader)
        {
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            oExportPIPrintSetup.ExportPIPrintSetupID = oReader.GetInt32("ExportPIPrintSetupID");
            oExportPIPrintSetup.SetupNo = oReader.GetString("SetupNo");
            oExportPIPrintSetup.Date = oReader.GetDateTime("Date");
            oExportPIPrintSetup.Note = oReader.GetString("Note");
            oExportPIPrintSetup.Preface = oReader.GetString("Preface");
            oExportPIPrintSetup.TermsOfPayment = oReader.GetString("TermsOfPayment");
            oExportPIPrintSetup.PartShipment = oReader.GetString("PartShipment");
            oExportPIPrintSetup.ShipmentBy = oReader.GetString("ShipmentBy");
            oExportPIPrintSetup.PlaceOfShipment = oReader.GetString("PlaceOfShipment");
            oExportPIPrintSetup.PlaceOfDelivery = oReader.GetString("PlaceOfDelivery");
            oExportPIPrintSetup.Delivery = oReader.GetString("Delivery");
            oExportPIPrintSetup.RequiredPaper = oReader.GetString("RequiredPaper");
            oExportPIPrintSetup.OtherTerms = oReader.GetString("OtherTerms");
            oExportPIPrintSetup.AcceptanceBy = oReader.GetString("AcceptanceBy");
            oExportPIPrintSetup.For = oReader.GetString("For");
            oExportPIPrintSetup.Activity = oReader.GetBoolean("Activity");
            oExportPIPrintSetup.HeaderType = oReader.GetInt32("HeaderType");
            oExportPIPrintSetup.BUID = oReader.GetInt32("BUID");
            oExportPIPrintSetup.BaseCurrencyID = oReader.GetInt32("BaseCurrencyID");
            oExportPIPrintSetup.ValidityDays = oReader.GetInt32("ValidityDays");
            oExportPIPrintSetup.BINNo = oReader.GetString("BINNo");
            oExportPIPrintSetup.PrintNo = oReader.GetInt16("PrintNo");
            
            return oExportPIPrintSetup;
        }

        private ExportPIPrintSetup CreateObject(NullHandler oReader)
        {
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            oExportPIPrintSetup = MapObject(oReader);
            return oExportPIPrintSetup;
        }

        private List<ExportPIPrintSetup> CreateObjects(IDataReader oReader)
        {
            List<ExportPIPrintSetup> oExportPIPrintSetup = new List<ExportPIPrintSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPIPrintSetup oItem = CreateObject(oHandler);
                oExportPIPrintSetup.Add(oItem);
            }
            return oExportPIPrintSetup;
        }

        #endregion

        #region Interface implementation
        public ExportPIPrintSetupService() { }

        public ExportPIPrintSetup Save(ExportPIPrintSetup oExportPIPrintSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPIPrintSetup.ExportPIPrintSetupID <= 0)
                {
                    reader = ExportPIPrintSetupDA.InsertUpdate(tc, oExportPIPrintSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ExportPIPrintSetupDA.InsertUpdate(tc, oExportPIPrintSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIPrintSetup = new ExportPIPrintSetup();
                    oExportPIPrintSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPIPrintSetup.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return oExportPIPrintSetup;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
                oExportPIPrintSetup.ExportPIPrintSetupID = id;
                ExportPIPrintSetupDA.Delete(tc, oExportPIPrintSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ExportPIPrintSetup ActivatePIPrintSetup(ExportPIPrintSetup oExportPIPrintSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPIPrintSetupDA.ActivatePIPrintSetup(tc, oExportPIPrintSetup);
                IDataReader reader = ExportPIPrintSetupDA.Get(tc, oExportPIPrintSetup.ExportPIPrintSetupID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIPrintSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oExportPIPrintSetup.ErrorMessage = "Failed to Activate PIPrintSetup. Because of " + e.Message.Split('!');
                #endregion
            }
            return oExportPIPrintSetup;
        }

        public ExportPIPrintSetup Get(int id, Int64 nUserId)
        {
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIPrintSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIPrintSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportPIPrintSetup;
        }

        public ExportPIPrintSetup Get(bool bActivity,int BUID, Int64 nUserId)
        {
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIPrintSetupDA.Get(tc, bActivity, BUID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIPrintSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportPIPrintSetup;
        }

        public ExportPIPrintSetup Get(string sPINo, Int64 nUserId)
        {
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIPrintSetupDA.Get(tc, sPINo);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIPrintSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportPIPrintSetup;
        }
        
        public List<ExportPIPrintSetup> Gets(Int64 nUserId)
        {
            List<ExportPIPrintSetup> oExportPIPrintSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIPrintSetupDA.Gets(tc);
                oExportPIPrintSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportPIPrintSetup;
        }

        public List<ExportPIPrintSetup> BUWiseGets(int nBUID, Int64 nUserId)
        {
            List<ExportPIPrintSetup> oExportPIPrintSetup = new List<ExportPIPrintSetup>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIPrintSetupDA.BUWiseGets(tc, nBUID);
                oExportPIPrintSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportPIPrintSetup;
        }
        
        #endregion
    }
}
