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
    public class ProductCategoryPropertyValueService : MarshalByRefObject, IProductCategoryPropertyValueService
    {
        #region Private functions and declaration
        private ProductCategoryPropertyValue MapObject(NullHandler oReader)
        {
            ProductCategoryPropertyValue oPCPI = new ProductCategoryPropertyValue();
            oPCPI.PCPVID = oReader.GetInt32("PCPVID");
            oPCPI.PCPID = oReader.GetInt32("PCPID");
            oPCPI.PropertyValueID = oReader.GetInt32("PropertyValueID");
            oPCPI.Note = oReader.GetString("Note");
            return oPCPI;
        }

        private ProductCategoryPropertyValue CreateObject(NullHandler oReader)
        {
            ProductCategoryPropertyValue oPCPI = new ProductCategoryPropertyValue();
            oPCPI = MapObject(oReader);
            return oPCPI;
        }

        private List<ProductCategoryPropertyValue> CreateObjects(IDataReader oReader)
        {
            List<ProductCategoryPropertyValue> oPCPI = new List<ProductCategoryPropertyValue>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductCategoryPropertyValue oItem = CreateObject(oHandler);
                oPCPI.Add(oItem);
            }
            return oPCPI;
        }

        #endregion

        #region Interface implementation
        public ProductCategoryPropertyValueService() { }

        public string Insert(ProductCategoryProperty oPCP, Int64 nUserId)
        {
            TransactionContext tc = null;
            string Message = "";
            try
            {
                tc = TransactionContext.Begin(true);
                ProductCategoryPropertyValueDA.DeleteByPCPID(tc, oPCP.PCPID);
                foreach (PropertyValue oItem in oPCP.PropertyValueList)
                {
                    ProductCategoryPropertyValue oPCPV = new ProductCategoryPropertyValue();
                    oPCPV.PCPID = oPCP.PCPID;
                    oPCPV.PropertyValueID = oItem.PropertyValueID;
                    ProductCategoryPropertyValueDA.Insert(tc, oPCPV);
                }                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductCategoryPropertyValueInformation. Because of " + e.Message, e);
                Message = e.Message;
                #endregion
            }
            return Message;
        }

        public ProductCategoryPropertyValue Get(int id, Int64 nUserId)
        {
            ProductCategoryPropertyValue oAccountHead = new ProductCategoryPropertyValue();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductCategoryPropertyValueDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductCategoryPropertyValueInformation", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProductCategoryPropertyValue> Gets(int nPCID, Int64 nUserId)
        {
            List<ProductCategoryPropertyValue> oPCPIs = new List<ProductCategoryPropertyValue>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductCategoryPropertyValueDA.Gets(tc, nPCID);
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
                //throw new ServiceException("Failed to Get ProductCategoryPropertyValueInformation", e);
                oPCPIs[0].ErrorMessage = e.Message;
                #endregion
            }

            return oPCPIs;
        }

        public List<ProductCategoryPropertyValue> Gets(string sSQL, Int64 nUserId)
        {
            List<ProductCategoryPropertyValue> oPCPI = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductCategoryPropertyValueDA.Gets(tc, sSQL);
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