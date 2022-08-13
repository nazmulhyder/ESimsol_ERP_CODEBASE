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
    public class CompanyService : MarshalByRefObject, ICompanyService
    {
        #region Private functions and declaration
        private Company MapObject(NullHandler oReader)
        {
            Company oCompany = new Company();
            oCompany.CompanyID = oReader.GetInt32("CompanyID");
            oCompany.EncryptCompanyID = Global.Encrypt(oCompany.CompanyID.ToString());
            oCompany.Name = oReader.GetString("Name");
            oCompany.Address = oReader.GetString("Address");
            oCompany.FactoryAddress = oReader.GetString("FactoryAddress");
            oCompany.Phone = oReader.GetString("Phone");
            oCompany.Email = oReader.GetString("Email");
            oCompany.Note = oReader.GetString("Note");
            oCompany.WebAddress = oReader.GetString("WebAddress");
            oCompany.FaxNo = oReader.GetString("FaxNo");
            oCompany.OrganizationLogo = oReader.GetBytes("OrganizationLogo");
            oCompany.ImageSize = oReader.GetInt32("ImageSize");
            oCompany.BaseCurrencyID = oReader.GetInt32("BaseCurrencyID");
            //oCompany.ParentID = oReader.GetInt32("ParentID");
            oCompany.CurrencyNameSymbol = oReader.GetString("CurrencyNameSymbol");
            oCompany.OrganizationTitle = oReader.GetBytes("OrganizationTitle");
            oCompany.TitleImageSize = oReader.GetInt32("TitleImageSize");
            oCompany.BaseAddress = oReader.GetString("BaseAddress");
            oCompany.CurrencyName = oReader.GetString("CurrencyName");
            oCompany.CurrencySymbol = oReader.GetString("CurrencySymbol");

            oCompany.VatRegNo = oReader.GetString("VatRegNo");
            oCompany.PostalCode = oReader.GetString("PostalCode");
            oCompany.Country = oReader.GetString("Country");
            oCompany.NameInBangla = oReader.GetString("NameINBangla");
            oCompany.AddressInBangla = oReader.GetString("AddressInBangla");
            oCompany.AuthorizedSignature = oReader.GetBytes("AuthorizedSignature");

            return oCompany;
        }

        private Company CreateObject(NullHandler oReader)
        {
            Company oCompany = new Company();
            oCompany = MapObject(oReader);
            return oCompany;
        }

        private List<Company> CreateObjects(IDataReader oReader)
        {
            List<Company> oCompanys = new List<Company>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Company oItem = CreateObject(oHandler);
                oCompanys.Add(oItem);
            }
            return oCompanys;
        }

        #endregion

        #region Interface implementation
        public CompanyService() { }
        public Company Save(Company oCompany, Int64 nUserId)
        {
            TransactionContext tc = null;
            Company oTempCom = new Company();
            oTempCom.OrganizationLogo = oCompany.OrganizationLogo;
            //oTempCom.IsRemoved = oCompany.IsRemoved;
            oTempCom.OrganizationTitle = oCompany.OrganizationTitle;
            oTempCom.AuthorizedSignature = oCompany.AuthorizedSignature;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oCompany.OrganizationLogo = null;
                oCompany.OrganizationTitle = null;
                oCompany.AuthorizedSignature = null;
                if (oCompany.CompanyID <= 0)
                {
                    reader = CompanyDA.InsertUpdate(tc, oCompany, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = CompanyDA.InsertUpdate(tc, oCompany, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompany = new Company();
                    oCompany = CreateObject(oReader);
                }
                reader.Close();

                oTempCom.CompanyID = oCompany.CompanyID;
                if (oTempCom.OrganizationLogo != null)
                {
                    CompanyDA.UpdatePhoto(tc, oTempCom, nUserId);
                }
                if (oTempCom.OrganizationTitle != null)
                {
                    CompanyDA.UpdatePhotoCompanyTitle(tc, oTempCom, nUserId);
                }
                if (oTempCom.AuthorizedSignature != null)
                {
                    CompanyDA.UpdateAuthorizedSignature(tc, oTempCom, nUserId);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save OrganizationalInformation. Because of " + e.Message, e);
                #endregion
            }
            return oCompany;
        }
        public string Delete(int nID, Int64 nUserId)
        {
            TransactionContext tc = null;
            Company oCompany = new Company();
            try
            {
                tc = TransactionContext.Begin(true);
                oCompany.CompanyID = nID;
                CompanyDA.Delete(tc, oCompany, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete OrganizationalInformation. Because of " + e.Message, e);
                #endregion
            }
            return "Data Delete Successfully";
        }
        public Company Get(int id, Int64 nUserId)
        {
            Company oCompany = new Company();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompany = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Organizational Information", e);
                #endregion
            }
            return oCompany;
        }
        public List<Company> Gets(Int64 nUserId)
        {
            List<Company> oCompany = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDA.Gets(tc);
                oCompany = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Organizational Information", e);
                #endregion
            }
            return oCompany;
        }
        public Company GetCompanyLogo(int id, Int64 nUserId)
        {
            Company oCompany = new Company();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyDA.GetCompanyLogo(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompany = CreateObject(oReader);
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
               throw new ServiceException("Failed to Get Organizational Information", e);
                #endregion
            }

            return oCompany;
        }

        public List<Company> Gets(string sSQL, Int64 nUserId)
        {
            List<Company> oCompany = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyDA.Gets(tc, sSQL);
                oCompany = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                #endregion
            }

            return oCompany;
        }
        #endregion
    }
}
