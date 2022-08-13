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
    public class FNBatchCardService : MarshalByRefObject, IFNBatchCardService
    {
        #region Private functions and declaration
        private FNBatchCard MapObject(NullHandler oReader)
        {
            FNBatchCard oFNBatchCard = new FNBatchCard();
            oFNBatchCard.FNBatchCardID = oReader.GetInt32("FNBatchCardID");
            oFNBatchCard.FNTreatmentProcessID = oReader.GetInt32("FNTreatmentProcessID");
            oFNBatchCard.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNBatchCard.PlannedDate = oReader.GetDateTime("PlannedDate");
            oFNBatchCard.FNBatchNo = oReader.GetString("FNBatchNo");
            oFNBatchCard.FNProcess = oReader.GetString("FNProcess");
            oFNBatchCard.FNTreatment = (EnumFNTreatment)oReader.GetInt16("FNTreatment");
            oFNBatchCard.Code = oReader.GetString("Code");
            oFNBatchCard.QCStatus = (EnumQCStatus)oReader.GetInt32("QCStatus");
            oFNBatchCard.Description = oReader.GetString("Description");
            oFNBatchCard.SequenceNo = oReader.GetInt32("SequenceNo");
            oFNBatchCard.SequenceNo_Pro = oReader.GetInt32("SequenceNo_Pro");
            oFNBatchCard.StartQty = oReader.GetDouble("StartQty");
            oFNBatchCard.EndQty = oReader.GetDouble("EndQty");
            oFNBatchCard.Qty_FNBatch = oReader.GetDouble("Qty_FNBatch");
            oFNBatchCard.Qty_Prod = oReader.GetDouble("Qty_Prod");
            oFNBatchCard.Qty_ReProd = oReader.GetDouble("Qty_ReProd");
            oFNBatchCard.Qty_FNPBatch = oReader.GetDouble("Qty_FNPBatch");
            
            oFNBatchCard.StartWidth = oReader.GetString("StartWidth");
            oFNBatchCard.EndWidth = oReader.GetString("EndWidth");
            oFNBatchCard.FNPBatchID = oReader.GetInt32("FNPBatchID");
            oFNBatchCard.IsProduction = oReader.GetBoolean("IsProduction");
            return oFNBatchCard;
        }
        private FNBatchCard CreateObject(NullHandler oReader)
        {
            FNBatchCard oFNBatchCard = new FNBatchCard();
            oFNBatchCard = MapObject(oReader);
            return oFNBatchCard;
        }
        private List<FNBatchCard> CreateObjects(IDataReader oReader)
        {
            List<FNBatchCard> oFNBatchCard = new List<FNBatchCard>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchCard oItem = CreateObject(oHandler);
                oFNBatchCard.Add(oItem);
            }
            return oFNBatchCard;
        }

        #endregion

        #region Interface implementation
        public FNBatchCardService() { }

        public FNBatchCard Save(FNBatchCard oFNBatchCard, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNBatchCard.FNBatchCardID <= 0)
                {
                    reader = FNBatchCardDA.InsertUpdate(tc, oFNBatchCard, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNBatchCardDA.InsertUpdate(tc, oFNBatchCard, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchCard = new FNBatchCard();
                    oFNBatchCard = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNBatchCard = new FNBatchCard();
                oFNBatchCard.ErrorMessage = e.Message.Split('~')[0];
                #endregion
               
            }
            return oFNBatchCard;
        }

        public List<FNBatchCard> SaveFNBatchCards(FNBatchCard oFNBCard, Int64 nUserID)
        {
            FNBatchCard oFNBatchCard = new FNBatchCard();
            List<FNBatchCard> oFNBCs = new List<FNBatchCard>();

            string sIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Delete Old FNBatch Card
                if (oFNBCard.FNBatchID > 0)
                {
                    #region get
                    IDataReader reader = null;
                    reader = FNBatchCardDA.Gets(tc, "SELECT * FROM View_FNBatchCard WHERE FNBatchID=" + oFNBCard.FNBatchID + " AND ISNULL(FNTreatment,0) != 1");
                    oFNBCs = CreateObjects(reader);
                    reader.Close();
                    #endregion
                    if (oFNBCs.Count > 0)
                    {
                        foreach (FNBatchCard oObj in oFNBCs)
                        {
                            sIDs += oObj.FNBatchCardID + ",";
                        }
                        oFNBatchCard = new FNBatchCard();
                        oFNBatchCard.FNBatchID = oFNBCs[0].FNBatchID;
                        oFNBatchCard.ErrorMessage = sIDs;
                        FNBatchCardDA.Delete(tc, oFNBatchCard, EnumDBOperation.Delete, nUserID);
                    }

                }
                #endregion

                #region save new FNBatch Cards
                oFNBCs = new List<FNBatchCard>();
                oFNBatchCard = new FNBatchCard();
                sIDs = "";
                if (oFNBCard.oFNBatchCards.Count > 0)
                {
                    IDataReader reader;
                    foreach (FNBatchCard oItem in oFNBCard.oFNBatchCards)
                    {
                        if (oItem.FNBatchCardID <= 0)
                        {
                            reader = FNBatchCardDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = FNBatchCardDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFNBatchCard = new FNBatchCard();
                            oFNBatchCard = CreateObject(oReader);
                            oFNBCs.Add(oFNBatchCard);
                            sIDs = sIDs + oFNBatchCard.FNBatchCardID + ",";
                        }
                        reader.Close();
                    }
                    if (sIDs.Length > 0)
                        sIDs = sIDs.Substring(0, sIDs.Length - 1);
                    oFNBatchCard = new FNBatchCard();
                    oFNBatchCard.FNBatchID = oFNBCard.oFNBatchCards[0].FNBatchID;
                    oFNBatchCard.ErrorMessage = sIDs;
                    FNBatchCardDA.Delete(tc, oFNBatchCard, EnumDBOperation.Delete, nUserID);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNBatchCard = new FNBatchCard();
                oFNBatchCard.ErrorMessage = e.Message.Split('~')[0];
                oFNBCs = new List<FNBatchCard>();
                oFNBCs.Add(oFNBatchCard);
                #endregion

            }
            return oFNBCs;
        }

        public string Delete(FNBatchCard oFNBatchCard, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNBatchCardDA.Delete(tc, oFNBatchCard, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public FNBatchCard Get(int id, Int64 nUserId)
        {
            FNBatchCard oFNBatchCard = new FNBatchCard();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNBatchCardDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchCard = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNBatchCard", e);
                #endregion
            }
            return oFNBatchCard;
        }
        public List<FNBatchCard> Gets(Int64 nUserID)
        {
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchCardDA.Gets(tc);
                oFNBatchCards = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNBatchCard", e);
                #endregion
            }
            return oFNBatchCards;
        }
        public List<FNBatchCard> Gets(string sSQL,Int64 nUserID)
        {
            List<FNBatchCard> oFNBatchCards = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchCardDA.Gets(tc,sSQL);
                oFNBatchCards = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNBatchCard", e);
                #endregion
            }
            return oFNBatchCards;
        }

       

        #endregion
    }   
}
