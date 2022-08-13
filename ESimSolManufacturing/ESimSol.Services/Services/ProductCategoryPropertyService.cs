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
    public class ProductCategoryPropertyService : MarshalByRefObject, IProductCategoryPropertyService
    {
        #region Private functions and declaration
        private ProductCategoryProperty MapObject(NullHandler oReader)
        {
            ProductCategoryProperty oPCPI = new ProductCategoryProperty();
            oPCPI.PCPID = oReader.GetInt32("PCPID");
            oPCPI.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oPCPI.PropertyID = oReader.GetInt32("PropertyID");
            oPCPI.IsMandatory = oReader.GetBoolean("IsMandatory");
            oPCPI.Note = oReader.GetString("Note");
            oPCPI.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oPCPI.PropertyName = oReader.GetString("PropertyName");
            return oPCPI;
        }

        private ProductCategoryProperty CreateObject(NullHandler oReader)
        {
            ProductCategoryProperty oPCPI = new ProductCategoryProperty();
            oPCPI = MapObject(oReader);
            return oPCPI;
        }

        private List<ProductCategoryProperty> CreateObjects(IDataReader oReader)
        {
            List<ProductCategoryProperty> oPCPI = new List<ProductCategoryProperty>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductCategoryProperty oItem = CreateObject(oHandler);
                oPCPI.Add(oItem);
            }
            return oPCPI;
        }

        #endregion

        #region Interface implementation
        public ProductCategoryPropertyService() { }

        public ProductCategoryProperty IUD(ProductCategoryProperty oPCPI,int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            ProductCategoryProperty oNewPCPI = new ProductCategoryProperty();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductCategoryPropertyDA.IUD(tc, oPCPI, nDBOperation,nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNewPCPI = CreateObject(oReader);
                }
                reader.Close();
                if (nDBOperation == 1)
                {
                    foreach (PropertyValue oItem in oPCPI.PropertyValueList)
                    {
                        ProductCategoryPropertyValue oPCPV = new ProductCategoryPropertyValue();
                        oPCPV.PCPID = oNewPCPI.PCPID;
                        oPCPV.PropertyValueID = oItem.PropertyValueID;
                        ProductCategoryPropertyValueDA.Insert(tc, oPCPV);
                    }
                }
                else if (nDBOperation == 3)
                {
                    ProductCategoryPropertyValueDA.DeleteByPCPID(tc, oPCPI.PCPID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oNewPCPI.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductCategoryPropertyInformation. Because of " + e.Message, e);
                #endregion
            }
            return oNewPCPI;
        }

        public ProductCategoryProperty Get(int id, Int64 nUserId)
        {
            ProductCategoryProperty oAccountHead = new ProductCategoryProperty();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductCategoryPropertyDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductCategoryPropertyInformation", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProductCategoryProperty> Gets(int nPCID, Int64 nUserId)
        {
            List<ProductCategoryProperty> oPCPIs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductCategoryPropertyDA.Gets(tc, nPCID);
                oPCPIs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductCategoryPropertyInformation", e);
                #endregion
            }

            return oPCPIs;
        }

        public List<ProductCategoryProperty> Gets(string sSQL, Int64 nUserId)
        {
            List<ProductCategoryProperty> oPCPI = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductCategoryPropertyDA.Gets(tc, sSQL);
                oPCPI = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oPCPI;
        }

        #endregion
    }
}