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
    public class FabricSalesContractService : MarshalByRefObject, IFabricSalesContractService
    {
    
        #region Private functions and declaration
        private static FabricSalesContract MapObject(NullHandler oReader)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            oFabricSalesContract.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFabricSalesContract.FabricSalesContractLogID = oReader.GetInt32("FabricSalesContractLogID");
            oFabricSalesContract.PaymentType = (EnumPIPaymentType)oReader.GetInt32("PaymentType");
            oFabricSalesContract.PaymentTypeInt = oReader.GetInt32("PaymentType");
            oFabricSalesContract.OrderType = oReader.GetInt32("OrderType");
            oFabricSalesContract.OrderTypeSt = oReader.GetString("OrderName");
            oFabricSalesContract.SCNo = oReader.GetString("SCNo");
            oFabricSalesContract.CurrentStatus = (EnumFabricPOStatus)oReader.GetInt32("CurrentStatus");
            oFabricSalesContract.CurrentStatusInt = oReader.GetInt32("CurrentStatus");
            oFabricSalesContract.SCDate = oReader.GetDateTime("SCDate");
          //  oFabricSalesContract.ValidityDate = oReader.GetDateTime("ValidityDate");
            oFabricSalesContract.ContractorID = oReader.GetInt32("ContractorID");
            oFabricSalesContract.BuyerID = oReader.GetInt32("BuyerID");
            oFabricSalesContract.MktAccountID = oReader.GetInt32("MktAccountID");
            oFabricSalesContract.CurrencyID = oReader.GetInt32("CurrencyID");
            oFabricSalesContract.Amount = oReader.GetDouble("Amount");
            oFabricSalesContract.Qty = oReader.GetDouble("Qty");
            oFabricSalesContract.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricSalesContract.LCTermID = oReader.GetInt32("LCTermID");
            oFabricSalesContract.Note = oReader.GetString("Note");
            oFabricSalesContract.EndUse = oReader.GetString("EndUse");
            oFabricSalesContract.QualityParameters = oReader.GetString("QualityParameters");
            oFabricSalesContract.QtyTolarance = oReader.GetString("QtyTolarance");
            oFabricSalesContract.PaymentInstruction = oReader.GetInt32("PaymentInstruction");
            oFabricSalesContract.ApproveBy = oReader.GetInt32("ApproveBy");
            oFabricSalesContract.ApprovedDate = oReader.GetDateTime("ApproveDate");
            
            oFabricSalesContract.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oFabricSalesContract.LightSourceID = oReader.GetInt32("LightSourceID");
            oFabricSalesContract.LightSourceIDTwo = oReader.GetInt32("LightSourceIDTwo");
          
          //  oFabricSalesContract.DeliveryToName = oReader.GetString("DeliveryToName");
            oFabricSalesContract.ContractorName = oReader.GetString("ContractorName");
            oFabricSalesContract.BuyerName = oReader.GetString("BuyerName");
            oFabricSalesContract.ApproveByName = oReader.GetString("ApproveByName");
            oFabricSalesContract.PreapeByName = oReader.GetString("PreapeByName");
            oFabricSalesContract.PreapeBy = oReader.GetInt32("DBUserID");
            oFabricSalesContract.LightSourceName = oReader.GetString("LightSourceName");
            oFabricSalesContract.LightSourceNameTwo = oReader.GetString("LightSourceNameTwo");
            oFabricSalesContract.LCTermsName = oReader.GetString("LCTermsName");
            oFabricSalesContract.Con_Address = oReader.GetString("ContractorAddress");
            oFabricSalesContract.BuyerAddress = oReader.GetString("BuyerAddress");
            //oFabricSalesContract.ContractorFax = oReader.GetString("ContractorFax");
            //oFabricSalesContract.ContractorEmail = oReader.GetString("ContractorEmail");
            oFabricSalesContract.MKTPName = oReader.GetString("MKTPName");
            oFabricSalesContract.MKTGroupName = oReader.GetString("MKTGroupName");
            oFabricSalesContract.MktGroupID = oReader.GetInt32("MktGroupID");
            oFabricSalesContract.Currency = oReader.GetString("Currency");

            oFabricSalesContract.BUID = oReader.GetInt32("BUID");
            oFabricSalesContract.ReviseNo = oReader.GetInt32("ReviseNo");
            oFabricSalesContract.SCNoFull = oReader.GetString("SCNoFull");
            oFabricSalesContract.PINo = oReader.GetString("PINo");
            oFabricSalesContract.GarmentWash = oReader.GetString("GarmentWash");
          
            oFabricSalesContract.AttCount = oReader.GetString("AttCount");
            //oFabricSalesContract.FabricReceiveDate = oReader.GetDateTime("FabricReceiveDate");
            //oFabricSalesContract.FabricReceiveBy = oReader.GetInt32("FabricReceiveBy");
            oFabricSalesContract.IsOpenPI = oReader.GetBoolean("IsOpenPI");
            oFabricSalesContract.IsPrint = oReader.GetBoolean("IsPrint");
            oFabricSalesContract.LCNo = oReader.GetString("LCNo");
            oFabricSalesContract.LCDate = oReader.GetDateTime("LCDate");
            oFabricSalesContract.CheckedBy = oReader.GetInt32("CheckedBy");
            oFabricSalesContract.CheckedDate = oReader.GetDateTime("CheckedDate");
            oFabricSalesContract.CheckedByName = oReader.GetString("CheckedByName");
            
            return oFabricSalesContract;

        }

        public static FabricSalesContract CreateObject(NullHandler oReader)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            oFabricSalesContract = MapObject(oReader);            
            return oFabricSalesContract;
        }

        private List<FabricSalesContract> CreateObjects(IDataReader oReader)
        {
            List<FabricSalesContract> oFabricSalesContracts = new List<FabricSalesContract>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSalesContract oItem = CreateObject(oHandler);
                oFabricSalesContracts.Add(oItem);
            }
            return oFabricSalesContracts;
        }

        private static FabricSalesContract MapObject_DistinctItem(NullHandler oReader)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();

            oFabricSalesContract.EndUse = oReader.GetString("DistinctItem");
            oFabricSalesContract.QualityParameters = oReader.GetString("DistinctItem");
            oFabricSalesContract.GarmentWash = oReader.GetString("DistinctItem");
            oFabricSalesContract.QtyTolarance = oReader.GetString("DistinctItem");
            oFabricSalesContract.FabricSalesContractID =10;
            return oFabricSalesContract;

        }

        public static FabricSalesContract CreateObject_DistinctItem(NullHandler oReader)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            oFabricSalesContract = MapObject_DistinctItem(oReader);
            return oFabricSalesContract;
        }

        private List<FabricSalesContract> CreateObjects_DistinctItem(IDataReader oReader)
        {
            List<FabricSalesContract> oFabricSalesContracts = new List<FabricSalesContract>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSalesContract oItem = CreateObject_DistinctItem(oHandler);
                oFabricSalesContracts.Add(oItem);
            }
            return oFabricSalesContracts;
        }

        #endregion

        #region Interface implementation
        public FabricSalesContractService() { }        
        public FabricSalesContract Get(int id, Int64 nUserId)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSalesContractDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oFabricSalesContract;
        }
        public FabricSalesContract GetByLog(int nLogid, Int64 nUserId)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSalesContractDA.GetByLog(tc, nLogid);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oFabricSalesContract;
        }
        
        public FabricSalesContract Get(string sPINo, int nTextTileUnit, Int64 nUserID)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSalesContractDA.Get(tc, sPINo, nTextTileUnit);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oFabricSalesContract;
        }
        public FabricSalesContract Save(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
             
                List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
                FabricSalesContractDetail  oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
                string sDetailIDs = "";
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricSalesContract.FabricSalesContractID <= 0)
                {
                    reader = FabricSalesContractDA.InsertUpdate(tc, oFabricSalesContract, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricSalesContractDA.InsertUpdate(tc, oFabricSalesContract, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = new FabricSalesContract();
                    oFabricSalesContract = CreateObject(oReader);
                }
                reader.Close();
                #region Export Pi Detail Part
                foreach (FabricSalesContractDetail oItem in oFabricSalesContractDetails)
                {
                    IDataReader readerdetail;
                    oItem.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                    if (!string.IsNullOrEmpty(oItem.Construction)) { oItem.Construction = oItem.Construction.Replace(" ", string.Empty); }
                    //oItem.Construction = oItem.Construction.Replace(" ", string.Empty);

                    if (oItem.FabricSalesContractDetailID <= 0)
                    {
                        readerdetail = FabricSalesContractDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = FabricSalesContractDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sDetailIDs = sDetailIDs + oReaderDetail.GetString("FabricSalesContractDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sDetailIDs.Length > 0)
                {
                    sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                }
                oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetail.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                FabricSalesContractDetailDA.Delete(tc, oFabricSalesContractDetail, EnumDBOperation.Delete, nUserID, sDetailIDs);
                #endregion


                //#region Get PI
                //reader = FabricSalesContractDA.Get(tc, oFabricSalesContract.FabricSalesContractID);
                //oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oFabricSalesContract = CreateObject(oReader);
                //}
                //reader.Close();
                //#endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSalesContract = new FabricSalesContract();
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContract;
        }
        public FabricSalesContract SaveLog(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
                FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
                string sDetailIDs = "";
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                reader = FabricSalesContractDA.InsertUpdateLog(tc, oFabricSalesContract, EnumDBOperation.Insert, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = new FabricSalesContract();
                    oFabricSalesContract = CreateObject(oReader);
                }
                reader.Close();
                #region Export Pi Detail Part
                foreach (FabricSalesContractDetail oItem in oFabricSalesContractDetails)
                {
                    IDataReader readerdetail;
                    oItem.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                    if (oItem.FabricSalesContractDetailID <= 0)
                    {
                        readerdetail = FabricSalesContractDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = FabricSalesContractDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sDetailIDs = sDetailIDs + oReaderDetail.GetString("FabricSalesContractDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sDetailIDs.Length > 0)
                {
                    sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                }
                oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetail.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                FabricSalesContractDetailDA.Delete(tc, oFabricSalesContractDetail, EnumDBOperation.Delete, nUserID, sDetailIDs);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSalesContract = new FabricSalesContract();
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContract;
        }
        public FabricSalesContract SaveRevise(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
                FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
                string sDetailIDs = "";
                tc = TransactionContext.Begin(true);
              
                #region Export Pi Detail Part
                foreach (FabricSalesContractDetail oItem in oFabricSalesContractDetails)
                {
                    IDataReader readerdetail;
                    readerdetail = FabricSalesContractDetailDA.Update_Revise(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    readerdetail.Close();
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSalesContract = new FabricSalesContract();
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContract;
        }

        public string UpdateBySQL(string sSQL, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricSalesContractDA.UpdateBySQL(tc, sSQL);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.SuccessMessage;
        }
        public FabricSalesContract Approved(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
                FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (FabricSalesContractDetail oItem in oFabricSalesContractDetails)
                {
                    FabricSalesContractDetailDA.Save_SLNo(tc, oItem);
                }
                reader = FabricSalesContractDA.InsertUpdate(tc, oFabricSalesContract, EnumDBOperation.Approval, nUserID);
                

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = new FabricSalesContract();
                    oFabricSalesContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return oFabricSalesContract;
        }
        public FabricSalesContract Save_FSCNote(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            FabricSalesContractNote oFabricSalesContractNote = new FabricSalesContractNote();
            oFabricSalesContractNotes = oFabricSalesContract.FabricSalesContractNotes;
           
            TransactionContext tc = null;
            try
            {

                #region Terms & Condition Part
                if (oFabricSalesContractNotes != null)
                {
                    string sExportPITandCClauseIDs = "";
                    foreach (FabricSalesContractNote oItem in oFabricSalesContractNotes)
                    {
                        //IDataReader readertnc;
                        oItem.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                        if (oItem.FabricSalesContractNoteID <= 0)
                        {
                          FabricSalesContractNoteDA.InsertUpdateTwo(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                             FabricSalesContractNoteDA.InsertUpdateTwo(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        //NullHandler oReaderTNC = new NullHandler(readertnc);
                        //if (readertnc.Read())
                        //{
                        //    sExportPITandCClauseIDs = sExportPITandCClauseIDs + oReaderTNC.GetString("FabricSalesContractNoteID") + ",";
                        //}
                        //readertnc.Close();
                    }
                    if (sExportPITandCClauseIDs.Length > 0)
                    {
                        sExportPITandCClauseIDs = sExportPITandCClauseIDs.Remove(sExportPITandCClauseIDs.Length - 1, 1);
                    }
                     oFabricSalesContractNote = new FabricSalesContractNote();
                    oFabricSalesContractNote.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                    FabricSalesContractNoteDA.Delete(tc, oFabricSalesContractNote, EnumDBOperation.Delete, nUserID, sExportPITandCClauseIDs);
                }
                #endregion

                #region Get PI
                IDataReader reader;
                reader = FabricSalesContractDA.Get(tc, oFabricSalesContract.FabricSalesContractID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return oFabricSalesContract;
        }
        public FabricSalesContract UpdateInfo(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricSalesContractDA.UpdateInfo(tc, oFabricSalesContract);
                 IDataReader reader;
                reader = FabricSalesContractDA.Get(tc,oFabricSalesContract.FabricSalesContractID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = new FabricSalesContract();
                    oFabricSalesContract = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSalesContract.ErrorMessage = e.Message;
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContract;
        }
       
        public string Delete(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricSalesContractDA.Delete(tc, oFabricSalesContract, EnumDBOperation.Delete, nUserID);                
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
        public List<FabricSalesContract> Gets(Int64 nUserId)
        {
            List<FabricSalesContract> oFabricSalesContracts = null;
            int nCount = 0;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                nCount = MarketingAccountDA.GetIsMKTUser(tc, nUserId);
                if (nCount > 0)
                {
                    reader = FabricSalesContractDA.GetsYetToApproveByMktGroup(tc, nUserId);
                }
                else
                {
                    reader = FabricSalesContractDA.GetsYetToApproveAll(tc, nUserId);
                }
                oFabricSalesContracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oFabricSalesContracts;
        }
        public List<FabricSalesContract> GetsByPI(int nPIID, Int64 nUserId)
        {
            List<FabricSalesContract> oFabricSalesContracts = null;
            int nCount = 0;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;

                reader = FabricSalesContractDA.GetsByPI(tc, nPIID);
                oFabricSalesContracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oFabricSalesContracts;
        }
        public List<FabricSalesContract> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricSalesContract> oFabricSalesContracts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDA.Gets(tc, sSQL);
                oFabricSalesContracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oFabricSalesContracts;
        }
        public List<FabricSalesContract> GetsReport(string sSQL, Int64 nUserId)
        {
            List<FabricSalesContract> oFabricSalesContracts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDA.GetsReport(tc, sSQL);
                oFabricSalesContracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Sales Contract", e);
                #endregion
            }

            return oFabricSalesContracts;
        }
        public FabricSalesContract GetLogID(int nLogID)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSalesContractDA.GetLogID(tc, nLogID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSalesContract", e);
                #endregion
            }

            return oFabricSalesContract;
        }
        public List<FabricSalesContract> GetsLog(int nPIID, Int64 nUserID)
        {
            List<FabricSalesContract> oFabricSalesContracts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSalesContractDA.GetsLog(tc, nPIID);
                oFabricSalesContracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }
            return oFabricSalesContracts;
        }
        public List<FabricSalesContract> Gets(int nContractorID, string sLCIDs, Int64 nUserID)
        {
            List<FabricSalesContract> oFabricSalesContracts = new List<FabricSalesContract>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDA.Gets(tc, nContractorID, sLCIDs);
                oFabricSalesContracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }

            return oFabricSalesContracts;
        }
  
 
    
        public FabricSalesContract Copy(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricSalesContractDA.Copy(tc, oFabricSalesContract, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = new FabricSalesContract();
                    oFabricSalesContract = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContract;
        }


        public List<FabricSalesContract> Gets_DistinctItem(string sSQL, Int64 nUserId)
        {
            List<FabricSalesContract> oFabricSalesContracts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractDA.Gets(tc, sSQL);
                oFabricSalesContracts = CreateObjects_DistinctItem(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oFabricSalesContracts;
        }

        public FabricSalesContract Cancel(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                //reader = FabricSalesContractDA.DOCancel(tc, oFabricSalesContract, EnumDBOperation.Cancel, nUserID);
                //if (oFabricSalesContract.FabricSalesContractID>0)
                //{
                    reader = FabricSalesContractDA.InsertUpdate(tc, oFabricSalesContract, EnumDBOperation.Cancel, nUserID);
                //}
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get FabricSalesContract", e);
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oFabricSalesContract;
        }

        public FabricSalesContract Check(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = FabricSalesContractDA.InsertUpdate(tc, oFabricSalesContract, EnumDBOperation.Check, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get FabricSalesContract", e);
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oFabricSalesContract;
        }

        public FabricSalesContract ReceiveFabric(int nFSCID, DateTime dtReceive, Int64 nUserID)
        {
            FabricSalesContract oFSC = new FabricSalesContract();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricSalesContractDA.ReceiveFabric(tc, nFSCID, dtReceive, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFSC = new FabricSalesContract();
                    oFSC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFSC.ErrorMessage = (e.Message.Contains("~"))? e.Message.Split('~')[0] : e.Message;
                #endregion
            }

            return oFSC;
        }
        public FabricSalesContract SaveSampleInvoice(FabricSalesContract oFabricSalesContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
                FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricSalesContractDA.InsertUpdateInvoice(tc, oFabricSalesContract, EnumDBOperation.Insert, nUserID);


                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = new FabricSalesContract();
                    oFabricSalesContract = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get FabricSalesContract", e);
                oFabricSalesContract.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSalesContract;
        }
        public FabricSalesContract UpdateReviseNo(int id, int ReviseNo, int nUserId)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                FabricSalesContractDA.UpdateReviseNo(tc, id, ReviseNo, nUserId);
                IDataReader reader = FabricSalesContractDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSalesContract", e);
                #endregion
            }

            return oFabricSalesContract;
        }
        #endregion
    }
}
