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
    public class ContractorService : MarshalByRefObject, IContractorService
    {
        #region Private functions and declaration
        private Contractor MapObject(NullHandler oReader)
        {
            Contractor oContractor = new Contractor();
            oContractor.ContractorID = oReader.GetInt32("ContractorID");
            oContractor.Name = oReader.GetString("Name");
            oContractor.Origin = oReader.GetString("Origin");
            oContractor.Zone = oReader.GetString("Zone");
            oContractor.Address = oReader.GetString("Address");
            oContractor.Address2 = oReader.GetString("Address2");
            oContractor.Address3 = oReader.GetString("Address3");
            oContractor.Phone = oReader.GetString("Phone");
            oContractor.Email = oReader.GetString("Email");
            oContractor.ShortName = oReader.GetString("ShortName");
            oContractor.ActualBalance = oReader.GetDouble("ActualBalance");
            oContractor.Fax = oReader.GetString("Fax");
            oContractor.Abbreviation = oReader.GetString("Abbreviation");
            oContractor.Note = oReader.GetString("Note");
            oContractor.Phone2 = oReader.GetString("Phone2");
            oContractor.TIN = oReader.GetString("TIN");
            oContractor.VAT = oReader.GetString("VAT");
            oContractor.GroupName = oReader.GetString("GroupName");
            oContractor.Activity = oReader.GetBoolean("Activity");
            oContractor.CountryID = oReader.GetInt32("CountryID");
            oContractor.CountryShortName = oReader.GetString("CountryShortName");
            oContractor.CountryCode = oReader.GetString("CountryCode");
            oContractor.CountryName = oReader.GetString("CountryName");
            oContractor.IsNeedCutOff = oReader.GetBoolean("IsNeedCutOff");
            oContractor.LastUpdateUserID = oReader.GetInt32("LastUpdateUserID");
            oContractor.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
            oContractor.UpdateByName = oReader.GetString("UpdateByName");
           
            return oContractor;
        }

        private Contractor CreateObject(NullHandler oReader)
        {
            Contractor oContractor = new Contractor();
            oContractor = MapObject(oReader);
            return oContractor;
        }

        private List<Contractor> CreateObjects(IDataReader oReader)
        {
            List<Contractor> oContractor = new List<Contractor>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Contractor oItem = CreateObject(oHandler);
                oContractor.Add(oItem);
            }
            return oContractor;
        }

        #endregion

        #region Interface implementation
        public ContractorService() { }

        public Contractor Save(Contractor oContractor, int nUserId)
        {
            TransactionContext tc = null;

            ContractorType oContractorType = new ContractorType();
            List<ContractorType> oContractorTypes = new List<ContractorType>();
            oContractorTypes = oContractor.ContractorTypes;
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = oContractor.BusinessUnits;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oContractor.ContractorID <= 0)
                {
                   // AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Contractor", EnumRoleOperationType.Add);
                    reader = ContractorDA.InsertUpdate(tc, oContractor, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                  //  AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Contractor", EnumRoleOperationType.Edit);
                    DBOperationArchiveDA.Insert(tc, EnumDBOperation.Update, EnumModuleName.Contractor, oContractor.ContractorID, "Contractor", "ContractorID", "", "Name", nUserId);
                    reader = ContractorDA.InsertUpdate(tc, oContractor, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oContractor = new Contractor();
                    oContractor = CreateObject(oReader);
                }
                reader.Close();
                #region noContractorTypes
                if (oContractorTypes != null)
                {
                    oContractorType.ContractorID = oContractor.ContractorID;
                    ContractorTypeDA.Delete(tc, oContractorType, EnumDBOperation.Delete, nUserId);
                   
                    foreach (ContractorType oItem in oContractorTypes)
                    {
                        IDataReader readertnc;
                        oItem.ContractorID = oContractor.ContractorID;
                        readertnc = ContractorTypeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        readertnc.Close();
                    }
                }
                #endregion

                #region 
                if (oBusinessUnits != null)
                {
                    BUWiseParty oBUWiseParty = new BUWiseParty();
                    oBUWiseParty.ContractorID = oContractor.ContractorID;
                    BUWisePartyDA.Delete(tc, oBUWiseParty, EnumDBOperation.Delete, nUserId);

                    foreach (BusinessUnit oItem in oBusinessUnits)
                    {
                        IDataReader readerBUParty;
                        oBUWiseParty = new BUWiseParty();
                        oBUWiseParty.ContractorID = oContractor.ContractorID;
                        oBUWiseParty.BUID = oItem.BusinessUnitID;
                        readerBUParty = BUWisePartyDA.InsertUpdate(tc, oBUWiseParty, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderTNC = new NullHandler(readerBUParty);
                        readerBUParty.Close();
                    }
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
              oContractor.ErrorMessage =  "Failed to Save Contractor. Because of " + e.Message;
                #endregion
            }
            return oContractor;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                Contractor oContractor = new Contractor();
                oContractor.ContractorID = id;
               // AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "Contractor", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Contractor", id);
                DBOperationArchiveDA.Insert(tc, EnumDBOperation.Delete, EnumModuleName.Contractor, oContractor.ContractorID, "Contractor", "ContractorID", "", "Name", nUserId);
                ContractorDA.Delete(tc, oContractor, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Delete sucessfully";
        }

        public Contractor Get(int id, int nUserId)
        {
            Contractor oAccountHead = new Contractor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContractorDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Contractor", e);
                #endregion
            }

            return oAccountHead;
        }

        public Contractor CommitActivity(int id, bool ActiveInActive, int nUserId)
        {
            Contractor oAccountHead = new Contractor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                ContractorDA.CommitActivity(tc, id, ActiveInActive, nUserId);
                IDataReader reader = ContractorDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Contractor", e);
                #endregion
            }

            return oAccountHead;
        }
        
       

        public List<Contractor> Gets(int nUserId)
        {
            List<Contractor> oContractor = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorDA.Gets(tc);
                oContractor = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Contractor", e);
                #endregion
            }

            return oContractor;
        }
        public List<Contractor> GetsByBU(int nBUID, int nUserID)
        {
            List<Contractor> oContractors =new List<Contractor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ContractorDA.GetsByBU(tc, nBUID);
                oContractors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Contractor", e);
                #endregion
            }

            return oContractors;
        }


        public List<Contractor> Gets(string sSQL, int nUserId)
        {
            List<Contractor> oContractor = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorDA.Gets(tc, sSQL);
                oContractor = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Contractor", e);
                #endregion
            }

            return oContractor;
        }
        

        public List<Contractor> Gets(int eContractorType,int nUserId)
        {
            List<Contractor> oContractors = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorDA.Gets(tc, eContractorType);
                oContractors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Contractors", e);
                #endregion
            }

            return oContractors;
        }

        public List<Contractor> GetsByName(string sName, int eContractorType, int nUserId)
        {
            List<Contractor> oContractors = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorDA.GetsByName(tc,sName, eContractorType);
                oContractors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Contractors", e);
                #endregion
            }

            return oContractors;
        }
     
        public List<Contractor> GetsByNamenType(string sName, string eContractorType, int nBUID, int nUserId)
        {
            List<Contractor> oContractors = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContractorDA.GetsByNamenType(tc, sName, eContractorType, nBUID);
                oContractors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Contractors", e);
                #endregion
            }

            return oContractors;
        }
        public List<Contractor> GetsForAccount(int nContractorType, int nReferenceType, int nUserId)
        {
            List<Contractor> oContractors = new List<Contractor>();            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ContractorDA.GetsForAccount(tc, nContractorType, nReferenceType);
                oContractors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Contractors", e);
                #endregion
            }

            return oContractors;
        }

        public Contractor UpdateNeedCutOff(Contractor oContractor, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContractorDA.UpdateNeedCutOff(tc, oContractor);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oContractor = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oContractor = new Contractor();
                oContractor.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oContractor;
        }

        public Contractor UpdateCountry(Contractor oContractor, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContractorDA.UpdateCountry(tc, oContractor);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oContractor = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oContractor = new Contractor();
                oContractor.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oContractor;
        }
        #endregion
    } 
}