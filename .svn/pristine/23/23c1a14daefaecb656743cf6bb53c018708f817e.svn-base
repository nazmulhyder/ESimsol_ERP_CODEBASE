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
    public class BrandService : MarshalByRefObject, IBrandService
    {
        #region Private functions and declaration
        private Brand MapObject(NullHandler oReader)
        {
            Brand oBrand = new Brand();
            oBrand.BrandID = oReader.GetInt32("BrandID");
            oBrand.BrandName = oReader.GetString("BrandName");
            oBrand.BrandCode = oReader.GetString("BrandCode");
            oBrand.WebAddress = oReader.GetString("WebAddress");
            oBrand.FaxNo = oReader.GetString("FaxNo");
            oBrand.EmailAddress = oReader.GetString("EmailAddress");
            oBrand.ShortName = oReader.GetString("ShortName");
            oBrand.Remarks = oReader.GetString("Remarks");
       
            return oBrand;
        }

        private Brand CreateObject(NullHandler oReader)
        {
            Brand oBrand = new Brand();
            oBrand = MapObject(oReader);
            return oBrand;
        }

        private List<Brand> CreateObjects(IDataReader oReader)
        {
            List<Brand> oBrand = new List<Brand>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Brand oItem = CreateObject(oHandler);
                oBrand.Add(oItem);
            }
            return oBrand;
        }

        #endregion

        #region Interface implementation
        public BrandService() { }

        public Brand Save(Brand oBrand, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBrand.BrandID <= 0)
                {
                    reader = BrandDA.InsertUpdate(tc, oBrand, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = BrandDA.InsertUpdate(tc, oBrand, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBrand = new Brand();
                    oBrand = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Brand. Because of " + e.Message, e);
                #endregion
            }
            return oBrand;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                Brand oBrand = new Brand();
                oBrand.BrandID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Brand, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Brand", id);
                BrandDA.Delete(tc, oBrand, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return "deleted";
        }

        public Brand Get(int id, Int64 nUserId)
        {
            Brand oAccountHead = new Brand();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BrandDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Brand", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<Brand> Gets(Int64 nUserId)
        {
            List<Brand> oBrand = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BrandDA.Gets(tc);
                oBrand = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Brand", e);
                #endregion
            }

            return oBrand;
        }
     
        
        public List<Brand> Gets(string sSQL, Int64 nUserId)
        {
            List<Brand> oBrand = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BrandDA.Gets(tc, sSQL);
                oBrand = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Brand", e);
                #endregion
            }

            return oBrand;
        }
        
        public List<Brand> GetsByName(string sName,  Int64 nUserId)
        {
            List<Brand> oBrands = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BrandDA.GetsByName(tc, sName );
                oBrands = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Brands", e);
                #endregion
            }

            return oBrands;
        }

       
        #endregion
    } 
}