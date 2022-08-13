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
    public class PropertyService : MarshalByRefObject, IPropertyService
    {
        #region Private functions and declaration
        private Property MapObject(NullHandler oReader)
        {
            Property oProperty = new Property();
            oProperty.PropertyID = oReader.GetInt32("PropertyID");
            oProperty.PropertyName = oReader.GetString("PropertyName");
            oProperty.Note = oReader.GetString("Note");
            oProperty.IsMandatory = oReader.GetBoolean("IsMandatory");
            oProperty.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProperty.ProductCategoryName = oReader.GetString("ProductCategoryName");
            return oProperty;
        }

        private Property CreateObject(NullHandler oReader)
        {
            Property oProperty = new Property();
            oProperty = MapObject(oReader);
            return oProperty;
        }

        private List<Property> CreateObjects(IDataReader oReader)
        {
            List<Property> oProperty = new List<Property>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Property oItem = CreateObject(oHandler);
                oProperty.Add(oItem);
            }
            return oProperty;
        }

        #endregion

        #region Interface implementation
        public PropertyService() { }

        public Property Save(Property oProperty, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProperty.PropertyID <= 0)
                {
                    reader = PropertyDA.InsertUpdate(tc, oProperty, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = PropertyDA.InsertUpdate(tc, oProperty, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProperty = new Property();
                    oProperty = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                    oProperty.ErrorMessage = e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Property. Because of " + e.Message, e);
                #endregion
            }
            return oProperty;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Property oProperty = new Property();
                oProperty.PropertyID = id;
                PropertyDA.Delete(tc, oProperty, EnumDBOperation.Delete, nUserId);
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


        public List<Property> Gets(string sSQL, Int64 nUserId)
        {
            List<Property> oPropertys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PropertyDA.Gets(tc, sSQL);
                oPropertys = CreateObjects(reader);
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

            return oPropertys;
        }


        public Property Get(int id, Int64 nUserId)
        {
            Property oAccountHead = new Property();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PropertyDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Property", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<Property> Gets(Int64 nUserId)
        {
            List<Property> oProperty = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PropertyDA.Gets(tc);
                oProperty = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Property", e);
                #endregion
            }

            return oProperty;
        }
        #endregion
    }
}