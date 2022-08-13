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
    public class ProductPropertyInformationService : MarshalByRefObject, IProductPropertyInformationService
    {
        #region Private functions and declaration
        public static ProductPropertyInformation MapObject(NullHandler oReader)
        {
            ProductPropertyInformation oProductPropertyInformation = new ProductPropertyInformation();
            oProductPropertyInformation.ProductPropertyInfoID = oReader.GetInt32("ProductPropertyInfoID");
            oProductPropertyInformation.ProductID = oReader.GetInt32("ProductID");
            oProductPropertyInformation.BUID = oReader.GetInt32("BUID");
            oProductPropertyInformation.PropertyValueID = oReader.GetInt32("PropertyValueID");
            oProductPropertyInformation.ProductName = oReader.GetString("ProductName");
            oProductPropertyInformation.PropertyType = (EnumPropertyType) oReader.GetInt32("PropertyType");
            oProductPropertyInformation.PropertyTypeInInt = oReader.GetInt32("PropertyType");
            oProductPropertyInformation.ValueOfProperty = oReader.GetString("ValueOfProperty");
            oProductPropertyInformation.BUName = oReader.GetString("BUName");
            oProductPropertyInformation.BUShortName = oReader.GetString("BUShortName");
            return oProductPropertyInformation;
        }

        public static ProductPropertyInformation CreateObject(NullHandler oReader)
        {
            ProductPropertyInformation oProductPropertyInformation = new ProductPropertyInformation();
            oProductPropertyInformation = MapObject(oReader);
            return oProductPropertyInformation;
        }

        public static List<ProductPropertyInformation> CreateObjects(IDataReader oReader)
        {
            List<ProductPropertyInformation> oProductPropertyInformation = new List<ProductPropertyInformation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductPropertyInformation oItem = CreateObject(oHandler);
                oProductPropertyInformation.Add(oItem);
            }
            return oProductPropertyInformation;
        }

        #endregion

        #region Interface implementation

        public ProductPropertyInformation Save(ProductPropertyInformation oProductPropertyInformation, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductPropertyInformation.ProductPropertyInfoID <= 0)
                {
                    reader = ProductPropertyInformationDA.InsertUpdate(tc, oProductPropertyInformation, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ProductPropertyInformationDA.InsertUpdate(tc, oProductPropertyInformation, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductPropertyInformation = new ProductPropertyInformation();
                    oProductPropertyInformation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductPropertyInformation.ErrorMessage = e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductPropertyInformation. Because of " + e.Message, e);
                #endregion
            }
            return oProductPropertyInformation;
        }


        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductPropertyInformation oProductPropertyInformation = new ProductPropertyInformation();
                oProductPropertyInformation.ProductPropertyInfoID = id;
                ProductPropertyInformationDA.Delete(tc, oProductPropertyInformation, EnumDBOperation.Delete, nUserId);
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
            return "Delete sucessfully";
        }


        public List<ProductPropertyInformation> Gets(string sSQL, int nUserId)
        {
            List<ProductPropertyInformation> oProductPropertyInformations = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductPropertyInformationDA.Gets(tc, sSQL);
                oProductPropertyInformations = CreateObjects(reader);
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
            return oProductPropertyInformations;
        }


        public ProductPropertyInformation Get(int id, int nUserId)
        {
            ProductPropertyInformation oProductPropertyInformation = new ProductPropertyInformation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ProductPropertyInformationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductPropertyInformation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductPropertyInformation", e);
                #endregion
            }
            return oProductPropertyInformation;
        }

        public List<ProductPropertyInformation> Gets(int nUserId)
        {
            List<ProductPropertyInformation> oProductPropertyInformation = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductPropertyInformationDA.Gets(tc);
                oProductPropertyInformation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductPropertyInformation", e);
                #endregion
            }

            return oProductPropertyInformation;
        }
        public List<ProductPropertyInformation> Gets(int nProductID, int BUID, int nUserID)
        {
            List<ProductPropertyInformation> oProductPropertyInformation = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductPropertyInformationDA.Gets(tc, nProductID, BUID);
                oProductPropertyInformation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductPropertyInformation", e);
                #endregion
            }

            return oProductPropertyInformation;
        }
        #endregion
    }
}
