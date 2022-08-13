using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class InspectionCertificateService : MarshalByRefObject, IInspectionCertificateService
    {
        #region Private functions and declaration
        private InspectionCertificate MapObject(NullHandler oReader)
        {
            InspectionCertificate oInspectionCertificate = new InspectionCertificate();
            oInspectionCertificate.InspectionCertificateID = oReader.GetInt32("InspectionCertificateID");
            oInspectionCertificate.RefNo = oReader.GetString("RefNo");
            oInspectionCertificate.ICDate = oReader.GetDateTime("ICDate");
            oInspectionCertificate.CommercialInvoiceID = oReader.GetInt32("CommercialInvoiceID");
            oInspectionCertificate.ShipperID = oReader.GetInt32("ShipperID");
            oInspectionCertificate.ManufacturerID = oReader.GetInt32("ManufacturerID");
            oInspectionCertificate.InvoiceNo = oReader.GetString("InvoiceNo");
            oInspectionCertificate.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oInspectionCertificate.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oInspectionCertificate.MasterLCNo = oReader.GetString("MasterLCNo");
            oInspectionCertificate.MasterLCDate = oReader.GetDateTime("MasterLCDate");
            oInspectionCertificate.BillOfLadingNo = oReader.GetString("BillOfLadingNo");
            oInspectionCertificate.BillOfLadingDate = oReader.GetDateTime("BillOfLadingDate");
            oInspectionCertificate.Vessel = oReader.GetString("Vessel");
            oInspectionCertificate.PortOfLoading = oReader.GetString("PortOfLoading");
            oInspectionCertificate.FinalDestination = oReader.GetString("FinalDestination");
            oInspectionCertificate.Remarks = oReader.GetString("Remarks");
            oInspectionCertificate.AuthorizeCompany = oReader.GetInt32("AuthorizeCompany");
            oInspectionCertificate.ShipperName = oReader.GetString("ShipperName");
            oInspectionCertificate.MenufacturerName = oReader.GetString("MenufacturerName");
            oInspectionCertificate.CompanyName = oReader.GetString("CompanyName");
            oInspectionCertificate.ShipmentMode = (EnumTransportType)oReader.GetInt32("ShipmentMode");
            return oInspectionCertificate;
        }

        private InspectionCertificate CreateObject(NullHandler oReader)
        {
            InspectionCertificate oInspectionCertificate = new InspectionCertificate();
            oInspectionCertificate = MapObject(oReader);
            return oInspectionCertificate;
        }

        private List<InspectionCertificate> CreateObjects(IDataReader oReader)
        {
            List<InspectionCertificate> oInspectionCertificate = new List<InspectionCertificate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InspectionCertificate oItem = CreateObject(oHandler);
                oInspectionCertificate.Add(oItem);
            }
            return oInspectionCertificate;
        }

        #endregion

        #region Interface implementation
        public InspectionCertificateService() { }

        public InspectionCertificate Save(InspectionCertificate oInspectionCertificate, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<InspectionCertificateDetail> oInspectionCertificateDetails = new List<InspectionCertificateDetail>();
                InspectionCertificateDetail oInspectionCertificateDetail = new InspectionCertificateDetail();

                oInspectionCertificateDetails = oInspectionCertificate.InspectionCertificateDetails;
                string sInspectionCertificateDetailIDs = "";

                #region InspectionCertificate part
                IDataReader reader;
                if (oInspectionCertificate.InspectionCertificateID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID,  EnumModuleName.InspectionCertificate, EnumRoleOperationType.Add);
                    reader = InspectionCertificateDA.InsertUpdate(tc, oInspectionCertificate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.InspectionCertificate, EnumRoleOperationType.Edit);
                    reader = InspectionCertificateDA.InsertUpdate(tc, oInspectionCertificate, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInspectionCertificate = new InspectionCertificate();
                    oInspectionCertificate = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region InspectionCertificate Detail Part
                if (oInspectionCertificateDetails != null)
                {
                    foreach (InspectionCertificateDetail oItem in oInspectionCertificateDetails)
                    {
                        IDataReader readerdetail;
                        oItem.InspectionCertificateID = oInspectionCertificate.InspectionCertificateID;
                        if (oItem.InspectionCertificateDetailID <= 0)
                        {
                            readerdetail = InspectionCertificateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = InspectionCertificateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sInspectionCertificateDetailIDs = sInspectionCertificateDetailIDs + oReaderDetail.GetString("InspectionCertificateDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sInspectionCertificateDetailIDs.Length > 0)
                    {
                        sInspectionCertificateDetailIDs = sInspectionCertificateDetailIDs.Remove(sInspectionCertificateDetailIDs.Length - 1, 1);
                    }
                    oInspectionCertificateDetail = new InspectionCertificateDetail();
                    oInspectionCertificateDetail.InspectionCertificateID = oInspectionCertificate.InspectionCertificateID;
                    InspectionCertificateDetailDA.Delete(tc, oInspectionCertificateDetail, EnumDBOperation.Delete, nUserID, sInspectionCertificateDetailIDs);

                }

                #endregion

                #region InspectionCertificate Get
                reader = InspectionCertificateDA.Get(tc, oInspectionCertificate.InspectionCertificateID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInspectionCertificate = CreateObject(oReader);
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
                oInspectionCertificate.ErrorMessage = Message;

                #endregion
            }
            return oInspectionCertificate;
        }

        public string Delete(int nInspectionCertificateID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                InspectionCertificate oInspectionCertificate = new InspectionCertificate();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.InspectionCertificate, EnumRoleOperationType.Delete);
                oInspectionCertificate.InspectionCertificateID = nInspectionCertificateID;
                InspectionCertificateDA.Delete(tc, oInspectionCertificate, EnumDBOperation.Delete, nUserId);
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


        public InspectionCertificate Get(int id, Int64 nUserId)
        {
            InspectionCertificate oInspectionCertificate = new InspectionCertificate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = InspectionCertificateDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInspectionCertificate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get InspectionCertificate", e);
                #endregion
            }

            return oInspectionCertificate;
        }

        public InspectionCertificate GetIC(int id, Int64 nUserId)
        {
            InspectionCertificate oInspectionCertificate = new InspectionCertificate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = InspectionCertificateDA.GetIC(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInspectionCertificate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get InspectionCertificate", e);
                #endregion
            }

            return oInspectionCertificate;
        }

        

        public List<InspectionCertificate> Gets(Int64 nUserID)
        {
            List<InspectionCertificate> oInspectionCertificate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InspectionCertificateDA.Gets(tc);
                oInspectionCertificate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InspectionCertificate", e);
                #endregion
            }

            return oInspectionCertificate;
        }
        public List<InspectionCertificate> Gets(int id, Int64 nUserID)
        {
            List<InspectionCertificate> oInspectionCertificate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InspectionCertificateDA.Gets(tc, id);
                oInspectionCertificate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InspectionCertificate", e);
                #endregion
            }

            return oInspectionCertificate;
        }

        public List<InspectionCertificate> Gets(string sSQL, Int64 nUserID)
        {
            List<InspectionCertificate> oInspectionCertificate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InspectionCertificateDA.Gets(tc, sSQL);
                oInspectionCertificate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InspectionCertificate", e);
                #endregion
            }

            return oInspectionCertificate;
        }
        #endregion
    }
    

}
