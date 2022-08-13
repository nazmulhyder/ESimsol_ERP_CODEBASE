using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Services.Services
{
    public class ContactPersonnelService : MarshalByRefObject, IContactPersonnelService
    {
        #region Private functions and declaration
        private ContactPersonnel MapObject(NullHandler oReader)
        {
            ContactPersonnel oContractorPersonal = new ContactPersonnel();
            oContractorPersonal.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oContractorPersonal.ContractorID = oReader.GetInt32("ContractorID");
            oContractorPersonal.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oContractorPersonal.Name = oReader.GetString("Name");
            oContractorPersonal.Address = oReader.GetString("Address");
            oContractorPersonal.Phone = oReader.GetString("Phone");
            oContractorPersonal.Email = oReader.GetString("Email");
            oContractorPersonal.CPGroupID = oReader.GetInt32("CPGroupID");
            oContractorPersonal.Note = oReader.GetString("Note");
            oContractorPersonal.RefUpdated = oReader.GetBoolean("RefUpdated");
            //oContractorPersonal.CmsTotal = oReader.GetDouble("CmsTotal");
            //oContractorPersonal.CmsMatured = oReader.GetDouble("CmsMatured");
            //oContractorPersonal.CmsPaid = oReader.GetDouble("CmsPaid");
            oContractorPersonal.CommissionAmount = oReader.GetDouble("CommissionAmount");
            oContractorPersonal.CommissionApproveAmount = oReader.GetDouble("CommissionApproveAmount");
            oContractorPersonal.PayableAmount = oReader.GetDouble("PayableAmount");
            oContractorPersonal.PaidAmount = oReader.GetDouble("PaidAmount");
            oContractorPersonal.ContractorName = oReader.GetString("ContractorName");
            oContractorPersonal.Photo = oReader.GetBytes("Photo");
            oContractorPersonal.Signature = oReader.GetBytes("Signature");
            oContractorPersonal.Designation = oReader.GetString("Designation");
            oContractorPersonal.IsAuthenticate = oReader.GetBoolean("IsAuthenticate");
            return oContractorPersonal;
        }

        private ContactPersonnel CreateObject(NullHandler oReader)
        {
            ContactPersonnel oContractorPersonal = new ContactPersonnel();
            oContractorPersonal = MapObject(oReader);
            return oContractorPersonal;
        }

        private List<ContactPersonnel> CreateObjects(IDataReader oReader)
        {
            List<ContactPersonnel> oContractorPersonal = new List<ContactPersonnel>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ContactPersonnel oItem = CreateObject(oHandler);
                oContractorPersonal.Add(oItem);
            }
            return oContractorPersonal;
        }

        #endregion

        #region Interface implementation
        public ContactPersonnelService() { }

        public ContactPersonnel Save(ContactPersonnel oContractorPersonal, int nUserId)
        {
            ContactPersonnel oTempCP = new ContactPersonnel();
            oTempCP.Photo = oContractorPersonal.Photo;
            oTempCP.Signature = oContractorPersonal.Signature;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oContractorPersonal.ContactPersonnelID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "ContactPersonnel", EnumRoleOperationType.Add);
                    string BUIDs = string.Join(",", oContractorPersonal.BusinessUnits.Select(x => x.BusinessUnitID).ToList());
                    reader = ContactPersonnelDA.InsertUpdate(tc, oContractorPersonal, EnumDBOperation.Insert, nUserId, BUIDs);
                }
                else
                {
                    string BUIDs = string.Join(",", oContractorPersonal.BusinessUnits.Select(x => x.BusinessUnitID).ToList());
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "ContactPersonnel", EnumRoleOperationType.Edit);
                    reader = ContactPersonnelDA.InsertUpdate(tc, oContractorPersonal, EnumDBOperation.Update, nUserId, BUIDs);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oContractorPersonal = new ContactPersonnel();
                    oContractorPersonal = CreateObject(oReader);
                }
                reader.Close();

                oTempCP.ContactPersonnelID = oContractorPersonal.ContactPersonnelID;
                if (oTempCP.Photo != null)
                {
                    ContactPersonnelDA.UpdatePhoto(tc, oTempCP, nUserId);
                }
                if (oTempCP.Signature != null)
                {
                    ContactPersonnelDA.UpdateSignature(tc, oTempCP, nUserId);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oContractorPersonal.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ContractorPersonal. Because of " + e.Message, e);
                #endregion
            }
            return oContractorPersonal;
        }

        public ContactPersonnel MergeCP(ContactPersonnel oContractorPersonal, int nUserId)
        {
            ContactPersonnel oTempCP = new ContactPersonnel();
            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                
                ContactPersonnelDA.MergeCP(tc, oContractorPersonal, nUserId);
              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oContractorPersonal.ErrorMessage = e.Message;
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Merge ContractorPersonal. Because of " + e.Message, e);
                #endregion
            }
            return oContractorPersonal;
        }

        public ContactPersonnel IUDContractor(ContactPersonnel oContractorPersonal, EnumDBOperation eEnumDBOperation, long nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Contractors Part
                foreach (Contractor oItem in oContractorPersonal.Contractors)
                {
                    if (oItem.ContractorID > 0)
                    {
                        ContactPersonnelDA.InsertContractor(tc, oContractorPersonal.ContactPersonnelID, oItem, eEnumDBOperation, nUserId);
                    }
                }
                #endregion

                tc.End();
            }
            catch (Exception e){
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oContractorPersonal.ErrorMessage = e.Message;
                #endregion
            }
            return oContractorPersonal;
        }
        

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ContactPersonnel oContractorPersonal = new ContactPersonnel();
                oContractorPersonal.ContactPersonnelID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "ContactPersonnel", EnumRoleOperationType.Delete);
                ContactPersonnelDA.Delete(tc, oContractorPersonal, EnumDBOperation.Delete,nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ContractorPersonal. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public string DeleteImage(ContactPersonnel oContactPersonnel, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oContactPersonnel.IsPhoto)
                {
                    ContactPersonnelDA.UpdatePhoto(tc, oContactPersonnel, nUserId);
                }
                else
                {
                    ContactPersonnelDA.UpdateSignature(tc, oContactPersonnel, nUserId);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ContractorPersonal. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public ContactPersonnel Get(int id, int nUserId)
        {
            ContactPersonnel oAccountHead = new ContactPersonnel();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContactPersonnelDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oAccountHead;
        }

        public ContactPersonnel GetWithImage(int id, int nUserId)
        {
            ContactPersonnel oAccountHead = new ContactPersonnel();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContactPersonnelDA.GetWithImage(tc, id);
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
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ContactPersonnel> Gets(int nUserId)
        {
            List<ContactPersonnel> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContactPersonnelDA.Gets(tc);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }
        public List<ContactPersonnel> Gets(string sSQL, int nUserId)
        {
            List<ContactPersonnel> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContactPersonnelDA.Gets(tc, sSQL);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }
        public List<ContactPersonnel> GetsByContractor(int nContractorID, int nUserId)
        {
            List<ContactPersonnel> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContactPersonnelDA.GetsByContractor(tc, nContractorID);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }
        public List<ContactPersonnel> GetsOnlyCommission(string sContractorIDs, int nUserId)
        {
            List<ContactPersonnel> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContactPersonnelDA.GetsOnlyCommission(tc, sContractorIDs);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }
        public List<ContactPersonnel> Gets(int nContractorID, int nUserId)
        {
            List<ContactPersonnel> oContractorPersonal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContactPersonnelDA.Gets(tc, nContractorID);
                oContractorPersonal = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContractorPersonal", e);
                #endregion
            }

            return oContractorPersonal;
        }

        #endregion
    }
}
