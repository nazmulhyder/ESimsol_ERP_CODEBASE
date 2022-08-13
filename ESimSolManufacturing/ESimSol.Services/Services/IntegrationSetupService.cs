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
    public class IntegrationSetupService : MarshalByRefObject, IIntegrationSetupService
    {
        #region Private functions and declaration
        private IntegrationSetup MapObject(NullHandler oReader)
        {
            IntegrationSetup oIntegrationSetup = new IntegrationSetup();
            oIntegrationSetup.IntegrationSetupID = oReader.GetInt32("IntegrationSetupID");
            oIntegrationSetup.SetupNo = oReader.GetString("SetupNo");
            oIntegrationSetup.VoucherSetup =  (EnumVoucherSetup)oReader.GetInt32("VoucherSetup");
            oIntegrationSetup.VoucherSetupInt = oReader.GetInt32("VoucherSetup");
            oIntegrationSetup.DataCollectionSQL = oReader.GetString("DataCollectionSQL");
            oIntegrationSetup.KeyColumn = oReader.GetString("KeyColumn");            
            oIntegrationSetup.Note = oReader.GetString("Note");
            oIntegrationSetup.Sequence = oReader.GetInt32("Sequence");
            oIntegrationSetup.SetupType = (EnumSetupType)oReader.GetInt32("SetupType");
            oIntegrationSetup.SetupTypeInInt = (int)oReader.GetInt32("SetupType");
            oIntegrationSetup.BUID = oReader.GetInt32("BUID");
            oIntegrationSetup.BUName = oReader.GetString("BUName");
            oIntegrationSetup.BUSName = oReader.GetString("BUSName");
            return oIntegrationSetup;
        }

        private IntegrationSetup CreateObject(NullHandler oReader)
        {
            IntegrationSetup oIntegrationSetup = new IntegrationSetup();
            oIntegrationSetup = MapObject(oReader);
            return oIntegrationSetup;
        }

        private List<IntegrationSetup> CreateObjects(IDataReader oReader)
        {
            List<IntegrationSetup> oIntegrationSetup = new List<IntegrationSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                IntegrationSetup oItem = CreateObject(oHandler);
                oIntegrationSetup.Add(oItem);
            }
            return oIntegrationSetup;
        }

        #endregion

        #region Interface implementation
        public IntegrationSetupService() { }
        public IntegrationSetup Save(IntegrationSetup oIntegrationSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<IntegrationSetupDetail> oIntegrationSetupDetails = new List<IntegrationSetupDetail>();
                IntegrationSetupDetail oIntegrationSetupDetail = new IntegrationSetupDetail();
                List<DebitCreditSetup> oDebitCreditSetups = new List<DebitCreditSetup>();                
                DebitCreditSetup oDebitCreditSetup = new DebitCreditSetup();
                List<DataCollectionSetup> oDataCollectionSetups = new List<DataCollectionSetup>();
                List<DataCollectionSetup> oDataCollectionSetupsFromIntegrationSetupDetails = new List<DataCollectionSetup>();
                DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();

                string sIntegrationSetupDetailIDs = "";                
                string sDebitCreditSetupIDs = "";
                string sDataCollectionSetupIDs = "";
                int nIntegrationSetupDetailID = 0;
                int nDebitCreditSetupID = 0;
                oIntegrationSetupDetails = oIntegrationSetup.IntegrationSetupDetails;

                #region IntegrationSetup
                IDataReader reader;
                if (oIntegrationSetup.IntegrationSetupID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.IntegrationSetup, EnumRoleOperationType.Add);
                    reader = IntegrationSetupDA.InsertUpdate(tc, oIntegrationSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.IntegrationSetup, EnumRoleOperationType.Edit);
                    reader = IntegrationSetupDA.InsertUpdate(tc, oIntegrationSetup, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oIntegrationSetup = new IntegrationSetup();
                    oIntegrationSetup = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region IntegrationSetupDetail Part
                if (oIntegrationSetupDetails != null)
                {
                    sIntegrationSetupDetailIDs = "";
                    foreach (IntegrationSetupDetail oItem in oIntegrationSetupDetails)
                    {
                        #region IntegrationSetupDetail
                        IDataReader readerdetail;
                        oDebitCreditSetups = new List<DebitCreditSetup>();
                        oDataCollectionSetupsFromIntegrationSetupDetails = new List<DataCollectionSetup>();
                        nIntegrationSetupDetailID = 0;                      
                        oItem.IntegrationSetupID = oIntegrationSetup.IntegrationSetupID;
                        oDebitCreditSetups = oItem.DebitCreditSetups;
                        oDataCollectionSetupsFromIntegrationSetupDetails = oItem.DataCollectionSetups;
                        if (oItem.IntegrationSetupDetailID <= 0)
                        {
                            readerdetail = IntegrationSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = IntegrationSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sIntegrationSetupDetailIDs = sIntegrationSetupDetailIDs + oReaderDetail.GetString("IntegrationSetupDetailID") + ",";
                            nIntegrationSetupDetailID = oReaderDetail.GetInt32("IntegrationSetupDetailID");
                        }
                        readerdetail.Close();
                        #endregion

                        #region DebitCreditSetup Part
                        if (oDebitCreditSetups != null)
                        {
                            sDebitCreditSetupIDs = "";
                            foreach (DebitCreditSetup oTempDebitCreditSetup in oDebitCreditSetups)
                            {
                                #region DebitCreditSetup
                                IDataReader readedebitcreditsetup;
                                oDataCollectionSetups = new List<DataCollectionSetup>();
                                nDebitCreditSetupID = 0;
                                oTempDebitCreditSetup.IntegrationSetupDetailID = nIntegrationSetupDetailID;
                                oDataCollectionSetups = oTempDebitCreditSetup.DataCollectionSetups;
                                if (oTempDebitCreditSetup.DebitCreditSetupID <= 0)
                                {
                                    readedebitcreditsetup = DebitCreditSetupDA.InsertUpdate(tc, oTempDebitCreditSetup, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readedebitcreditsetup = DebitCreditSetupDA.InsertUpdate(tc, oTempDebitCreditSetup, EnumDBOperation.Update, nUserID, "");
                                }
                                NullHandler oReaderDebitCreditSetup = new NullHandler(readedebitcreditsetup);
                                if (readedebitcreditsetup.Read())
                                {
                                    sDebitCreditSetupIDs = sDebitCreditSetupIDs + oReaderDebitCreditSetup.GetString("DebitCreditSetupID") + ",";
                                    nDebitCreditSetupID = oReaderDebitCreditSetup.GetInt32("DebitCreditSetupID");
                                }
                                readedebitcreditsetup.Close();
                                #endregion

                                #region DataCollectionSetups Part
                                if (oDataCollectionSetups != null)
                                {
                                    sDataCollectionSetupIDs = "";
                                    foreach (DataCollectionSetup oTempDataCollectionSetup in oDataCollectionSetups)
                                    {
                                        #region DataCollectionSetup
                                        IDataReader readerDataCollectionSetup;
                                        oTempDataCollectionSetup.DataReferenceType = EnumDataReferenceType.DebitCreditSetup;
                                        oTempDataCollectionSetup.DataReferenceTypeInInt = (int)EnumDataReferenceType.DebitCreditSetup;
                                        oTempDataCollectionSetup.DataReferenceID = nDebitCreditSetupID;
                                        if (oTempDataCollectionSetup.DataCollectionSetupID <= 0)
                                        {
                                            readerDataCollectionSetup = DataCollectionSetupDA.InsertUpdate(tc, oTempDataCollectionSetup, EnumDBOperation.Insert, nUserID, "");
                                        }
                                        else
                                        {
                                            readerDataCollectionSetup = DataCollectionSetupDA.InsertUpdate(tc, oTempDataCollectionSetup, EnumDBOperation.Update, nUserID, "");
                                        }
                                        NullHandler oReaderDataCollectionSetup = new NullHandler(readerDataCollectionSetup);
                                        if (readerDataCollectionSetup.Read())
                                        {
                                            sDataCollectionSetupIDs = sDataCollectionSetupIDs + oReaderDataCollectionSetup.GetString("DataCollectionSetupID") + ",";
                                        }
                                        readerDataCollectionSetup.Close();
                                        #endregion
                                    }
                                    if (sDataCollectionSetupIDs.Length > 0)
                                    {
                                        sDataCollectionSetupIDs = sDataCollectionSetupIDs.Remove(sDataCollectionSetupIDs.Length - 1, 1);
                                    }
                                    oDataCollectionSetup = new DataCollectionSetup();
                                    oDataCollectionSetup.DataReferenceType = EnumDataReferenceType.DebitCreditSetup;
                                    oDataCollectionSetup.DataReferenceTypeInInt = (int)EnumDataReferenceType.DebitCreditSetup;
                                    oDataCollectionSetup.DataReferenceID = nDebitCreditSetupID;
                                    DataCollectionSetupDA.Delete(tc, oDataCollectionSetup, EnumDBOperation.Delete, nUserID, sDataCollectionSetupIDs);
                                }
                                #endregion
                            }
                            if (sDebitCreditSetupIDs.Length > 0)
                            {
                                sDebitCreditSetupIDs = sDebitCreditSetupIDs.Remove(sDebitCreditSetupIDs.Length - 1, 1);
                            }
                            oDebitCreditSetup = new DebitCreditSetup();
                            oDebitCreditSetup.IntegrationSetupDetailID = nIntegrationSetupDetailID;
                            DebitCreditSetupDA.Delete(tc, oDebitCreditSetup, EnumDBOperation.Delete, nUserID, sDebitCreditSetupIDs);
                        }
                        #endregion

                        #region DataCollectionSetups Part
                        if (oDataCollectionSetupsFromIntegrationSetupDetails != null)
                        {
                            sDataCollectionSetupIDs = "";
                            foreach (DataCollectionSetup oTempDataCollection in oDataCollectionSetupsFromIntegrationSetupDetails)
                            {
                                #region DataCollectionSetup
                                IDataReader readerDataCollection;
                                oTempDataCollection.DataReferenceType = EnumDataReferenceType.IntegrationDetail;
                                oTempDataCollection.DataReferenceTypeInInt = (int)EnumDataReferenceType.IntegrationDetail;
                                oTempDataCollection.DataReferenceID = nIntegrationSetupDetailID;
                                if (oTempDataCollection.DataCollectionSetupID <= 0)
                                {
                                    readerDataCollection = DataCollectionSetupDA.InsertUpdate(tc, oTempDataCollection, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerDataCollection = DataCollectionSetupDA.InsertUpdate(tc, oTempDataCollection, EnumDBOperation.Update, nUserID, "");
                                }
                                NullHandler oReaderDataCollection = new NullHandler(readerDataCollection);
                                if (readerDataCollection.Read())
                                {
                                    sDataCollectionSetupIDs = sDataCollectionSetupIDs + oReaderDataCollection.GetString("DataCollectionSetupID") + ",";
                                }
                                readerDataCollection.Close();
                                #endregion
                            }
                            if (sDataCollectionSetupIDs.Length > 0)
                            {
                                sDataCollectionSetupIDs = sDataCollectionSetupIDs.Remove(sDataCollectionSetupIDs.Length - 1, 1);
                            }
                            oDataCollectionSetup = new DataCollectionSetup();
                            oDataCollectionSetup.DataReferenceType = EnumDataReferenceType.IntegrationDetail;
                            oDataCollectionSetup.DataReferenceTypeInInt = (int)EnumDataReferenceType.IntegrationDetail;
                            oDataCollectionSetup.DataReferenceID = nIntegrationSetupDetailID;
                            DataCollectionSetupDA.Delete(tc, oDataCollectionSetup, EnumDBOperation.Delete, nUserID, sDataCollectionSetupIDs);
                        }
                        #endregion

                    }
                    if (sIntegrationSetupDetailIDs.Length > 0)
                    {
                        sIntegrationSetupDetailIDs = sIntegrationSetupDetailIDs.Remove(sIntegrationSetupDetailIDs.Length - 1, 1);
                    }
                    oIntegrationSetupDetail = new IntegrationSetupDetail();
                    oIntegrationSetupDetail.IntegrationSetupID = oIntegrationSetup.IntegrationSetupID;
                    IntegrationSetupDetailDA.Delete(tc, oIntegrationSetupDetail, EnumDBOperation.Delete, nUserID, sIntegrationSetupDetailIDs);

                }
                #endregion

                #region IntegrationSetup Get
                reader = IntegrationSetupDA.Get(tc, oIntegrationSetup.IntegrationSetupID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oIntegrationSetup = CreateObject(oReader);
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

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oIntegrationSetup.ErrorMessage = Message;

                #endregion
            }
            return oIntegrationSetup;
        }

        public List<IntegrationSetup> UpdateSequence(IntegrationSetup oIntegrationSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<IntegrationSetup> oIntegrationSetups = new List<IntegrationSetup>();
            oIntegrationSetups = oIntegrationSetup.IntegrationSetups;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach(IntegrationSetup oItem in oIntegrationSetups)
                {
                    IntegrationSetupDA.UpdateSequence(tc, oItem);
                }
                IDataReader reader = null;
                reader = IntegrationSetupDA.Gets(tc);
                oIntegrationSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                oIntegrationSetups=new List<IntegrationSetup>();
                oIntegrationSetup=new IntegrationSetup();
                oIntegrationSetup.ErrorMessage = e.Message;
                oIntegrationSetups.Add(oIntegrationSetup);
                #endregion
            }
            return oIntegrationSetups;
        }

        public string Delete(int nIntegrationSetupID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IntegrationSetup oIntegrationSetup = new IntegrationSetup();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.IntegrationSetup, EnumRoleOperationType.Delete);
                oIntegrationSetup.IntegrationSetupID = nIntegrationSetupID;
                IntegrationSetupDA.Delete(tc, oIntegrationSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public IntegrationSetup Get(int id, Int64 nUserId)
        {
            IntegrationSetup oAccountHead = new IntegrationSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = IntegrationSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get IntegrationSetup", e);
                #endregion
            }

            return oAccountHead;
        }

        public IntegrationSetup GetByVoucherSetup(EnumVoucherSetup eEnumVoucherSetup, Int64 nUserID)
        {
            IntegrationSetup oAccountHead = new IntegrationSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = IntegrationSetupDA.GetByVoucherSetup(tc, eEnumVoucherSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get IntegrationSetup", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<IntegrationSetup> GetsBySetupType(EnumSetupType eEnumSetupType, Int64 nUserID)
        {
            List<IntegrationSetup> oIntegrationSetups = new List<IntegrationSetup>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = IntegrationSetupDA.GetsBySetupType(tc, eEnumSetupType);
                oIntegrationSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IntegrationSetup", e);
                #endregion
            }

            return oIntegrationSetups;
        }
        public List<IntegrationSetup> GetsByBU(int nBUID, Int64 nUserID)
        {
            List<IntegrationSetup> oIntegrationSetups = new List<IntegrationSetup>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = IntegrationSetupDA.GetsByBU(tc, nBUID);
                oIntegrationSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IntegrationSetup", e);
                #endregion
            }

            return oIntegrationSetups;
        }
        public List<IntegrationSetup> Gets(Int64 nUserID)
        {
            List<IntegrationSetup> oIntegrationSetups = new List<IntegrationSetup>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IntegrationSetupDA.Gets(tc);
                oIntegrationSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IntegrationSetup", e);
                #endregion
            }

            return oIntegrationSetups;
        }

        public List<IntegrationSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<IntegrationSetup> oIntegrationSetups = new List<IntegrationSetup>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IntegrationSetupDA.Gets(tc, sSQL);
                oIntegrationSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IntegrationSetup", e);
                #endregion
            }

            return oIntegrationSetups;
        }
        #endregion
    }
}
