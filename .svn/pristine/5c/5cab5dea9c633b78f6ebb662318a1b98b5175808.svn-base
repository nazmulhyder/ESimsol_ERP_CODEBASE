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
    public class FNProductionBatchTransferService : MarshalByRefObject, IFNProductionBatchTransferService
    {
        #region Private functions and declaration
        private static  FNProductionBatchTransfer MapObject(NullHandler oReader)
        {
             FNProductionBatchTransfer  oFNProductionBatchTransfer = new  FNProductionBatchTransfer();
             oFNProductionBatchTransfer.FNPBTransferID = oReader.GetInt32("FNPBTransferID");
             oFNProductionBatchTransfer.FNPBTransferNo = oReader.GetString("FNPBTransferNo");
             oFNProductionBatchTransfer.ReceiveBy = oReader.GetInt32("ReceiveBy");
             oFNProductionBatchTransfer.ReceiveDate = oReader.GetDateTime("ReceiveDate");
             oFNProductionBatchTransfer.TransferNote = oReader.GetString("TransferNote");
             oFNProductionBatchTransfer.FNTreatment =(EnumFNTreatment) oReader.GetInt16("FNTreatment");
             oFNProductionBatchTransfer.TransferDate = oReader.GetDateTime("TransferDate");
             oFNProductionBatchTransfer.TransferBy = oReader.GetInt32("TransferBy");
             oFNProductionBatchTransfer.TransferByName = oReader.GetString("TransferByName");
             oFNProductionBatchTransfer.RecvByName = oReader.GetString("RecvByName");
   
            return  oFNProductionBatchTransfer;
        }

        public static  FNProductionBatchTransfer CreateObject(NullHandler oReader)
        {
             FNProductionBatchTransfer  oFNProductionBatchTransfer = MapObject(oReader);
            return  oFNProductionBatchTransfer;
        }

        private List< FNProductionBatchTransfer> CreateObjects(IDataReader oReader)
        {
            List< FNProductionBatchTransfer>  oFNProductionBatchTransfer = new List< FNProductionBatchTransfer>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                 FNProductionBatchTransfer oItem = CreateObject(oHandler);
                 oFNProductionBatchTransfer.Add(oItem);
            }
            return  oFNProductionBatchTransfer;
        }

        #endregion

        #region Interface implementation
        public FNProductionBatchTransferService() { }

        public  FNProductionBatchTransfer IUD( FNProductionBatchTransfer  oFNProductionBatchTransfer, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                {
                    reader =  FNProductionBatchTransferDA.IUD(tc,  oFNProductionBatchTransfer, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                         oFNProductionBatchTransfer = new  FNProductionBatchTransfer();
                         oFNProductionBatchTransfer = CreateObject(oReader);
                    }
                    reader.Close();

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader =  FNProductionBatchTransferDA.IUD(tc,  oFNProductionBatchTransfer, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                     oFNProductionBatchTransfer.ErrorMessage = Global.DeleteMessage;
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
                 oFNProductionBatchTransfer = new  FNProductionBatchTransfer();
                 oFNProductionBatchTransfer.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return  oFNProductionBatchTransfer;
        }

        public  FNProductionBatchTransfer Get(int nSULDID, Int64 nUserId)
        {
             FNProductionBatchTransfer  oFNProductionBatchTransfer = new  FNProductionBatchTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader =  FNProductionBatchTransferDA.Get(tc, nSULDID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                     oFNProductionBatchTransfer = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                 oFNProductionBatchTransfer = new  FNProductionBatchTransfer();
                 oFNProductionBatchTransfer.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return  oFNProductionBatchTransfer;
        }

        public List< FNProductionBatchTransfer> Gets(string sSQL, Int64 nUserID)
        {
            List< FNProductionBatchTransfer>  oFNProductionBatchTransfers = new List< FNProductionBatchTransfer>();
             FNProductionBatchTransfer  oFNProductionBatchTransfer = new  FNProductionBatchTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader =  FNProductionBatchTransferDA.Gets(tc, sSQL);
                 oFNProductionBatchTransfers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                 oFNProductionBatchTransfer.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                 oFNProductionBatchTransfers.Add( oFNProductionBatchTransfer);
                #endregion
            }

            return  oFNProductionBatchTransfers;
        }


     

        #endregion
    }
}
