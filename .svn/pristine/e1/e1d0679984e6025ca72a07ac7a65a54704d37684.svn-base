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
    public class CertificateService : MarshalByRefObject, ICertificateService
    {
        #region Private functions and declaration
        private Certificate MapObject(NullHandler oReader)
        {
            Certificate oCertificate = new Certificate();
            oCertificate.CertificateID = oReader.GetInt32("CertificateID");
            oCertificate.CertificateNo = oReader.GetString("CertificateNo");
            oCertificate.Description = oReader.GetString("Description");
            oCertificate.RequiredFor = oReader.GetString("RequiredFor");
            oCertificate.CertificateType = (EnumCertificateType)oReader.GetInt16("CertificateType");
            oCertificate.IsActive = oReader.GetBoolean("IsActive");
           
            return oCertificate;

        }

        private Certificate CreateObject(NullHandler oReader)
        {
            Certificate oCertificate = MapObject(oReader);
            return oCertificate;
        }

        private List<Certificate> CreateObjects(IDataReader oReader)
        {
            List<Certificate> oCertificate = new List<Certificate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Certificate oItem = CreateObject(oHandler);
                oCertificate.Add(oItem);
            }
            return oCertificate;
        }

        #endregion

        #region Interface implementation
        public CertificateService() { }

        public Certificate IUD(Certificate oCertificate, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CertificateDA.IUD(tc, oCertificate, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCertificate = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCertificate.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCertificate.CertificateID = 0;
                #endregion
            }
            return oCertificate;
        }


        public Certificate Get(int nCertificateID, Int64 nUserId)
        {
            Certificate oCertificate = new Certificate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CertificateDA.Get(nCertificateID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCertificate = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get Certificate", e);
                oCertificate.ErrorMessage = e.Message;
                #endregion
            }

            return oCertificate;
        }

        public Certificate Get(string sSql, Int64 nUserId)
        {
            Certificate oCertificate = new Certificate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CertificateDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCertificate = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get Certificate", e);
                oCertificate.ErrorMessage = e.Message;
                #endregion
            }

            return oCertificate;
        }

        public List<Certificate> Gets(Int64 nUserID)
        {
            List<Certificate> oCertificate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CertificateDA.Gets(tc);
                oCertificate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Certificate", e);
                #endregion
            }
            return oCertificate;
        }

        public List<Certificate> Gets(string sSQL, Int64 nUserID)
        {
            List<Certificate> oCertificate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CertificateDA.Gets(sSQL, tc);
                oCertificate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Certificate", e);
                #endregion
            }
            return oCertificate;
        }


        #region Activity
        public Certificate Activity(int nCertificateID, bool Active, Int64 nUserId)
        {
            Certificate oCertificate = new Certificate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CertificateDA.Activity(nCertificateID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCertificate = CreateObject(oReader);
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
                oCertificate.ErrorMessage = e.Message;
                #endregion
            }

            return oCertificate;
        }


        #endregion

        #endregion

    }
}
