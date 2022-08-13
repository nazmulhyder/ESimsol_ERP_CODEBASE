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
    public class PropertyValueService : MarshalByRefObject, IPropertyValueService
    {
        #region Private functions and declaration
        private PropertyValue MapObject(NullHandler oReader)
        {
            PropertyValue oPropertyValue = new PropertyValue();
            oPropertyValue.PropertyValueID = oReader.GetInt32("PropertyValueID");
            oPropertyValue.PropertyType = (EnumPropertyType) oReader.GetInt32("PropertyType");
            oPropertyValue.PropertyTypeInInt = oReader.GetInt32("PropertyType");
            oPropertyValue.Remarks = oReader.GetString("Remarks");
            oPropertyValue.ValueOfProperty = oReader.GetString("ValueOfProperty");            
            return oPropertyValue;
        }

        private PropertyValue CreateObject(NullHandler oReader)
        {
            PropertyValue oPropertyValue = new PropertyValue();
            oPropertyValue = MapObject(oReader);
            return oPropertyValue;
        }

        private List<PropertyValue> CreateObjects(IDataReader oReader)
        {
            List<PropertyValue> oPropertyValue = new List<PropertyValue>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PropertyValue oItem = CreateObject(oHandler);
                oPropertyValue.Add(oItem);
            }
            return oPropertyValue;
        }

        #endregion

        #region Interface implementation
        public PropertyValueService() { }

        public PropertyValue Save(PropertyValue oPropertyValue, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPropertyValue.PropertyValueID <= 0)
                {
                    reader = PropertyValueDA.InsertUpdate(tc, oPropertyValue, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = PropertyValueDA.InsertUpdate(tc, oPropertyValue, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPropertyValue = new PropertyValue();
                    oPropertyValue = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                    oPropertyValue.ErrorMessage = e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PropertyValue. Because of " + e.Message, e);
                #endregion
            }
            return oPropertyValue;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PropertyValue oPropertyValue = new PropertyValue();
                oPropertyValue.PropertyValueID = id;
                PropertyValueDA.Delete(tc, oPropertyValue, EnumDBOperation.Delete, nUserId);
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

        public PropertyValue Get(int id, Int64 nUserId)
        {
            PropertyValue oAccountHead = new PropertyValue();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PropertyValueDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PropertyValue", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PropertyValue> GetsByProperty(int nPropertyID, Int64 nUserId)
        {
            List<PropertyValue> oPropertyValue = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PropertyValueDA.GetsByProperty(tc, nPropertyID);
                oPropertyValue = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PropertyValue", e);
                #endregion
            }

            return oPropertyValue;
        }

        public List<PropertyValue> GetsByPropertyValue( string sTemp, int nPropertyID, Int64 nUserId)
        {
            List<PropertyValue> oPropertyValue = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PropertyValueDA.GetsByPropertyValue(tc, sTemp, nPropertyID);
                oPropertyValue = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PropertyValue", e);
                #endregion
            }

            return oPropertyValue;
        }

        

        public List<PropertyValue> Gets(Int64 nUserId)
        {
            List<PropertyValue> oPropertyValue = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PropertyValueDA.Gets(tc);
                oPropertyValue = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PropertyValue", e);
                #endregion
            }

            return oPropertyValue;
        }


        public List<PropertyValue> Gets(string sSQL, Int64 nUserId)
        {
            List<PropertyValue> oPropertyValues = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PropertyValueDA.Gets(tc, sSQL);
                oPropertyValues = CreateObjects(reader);
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

            return oPropertyValues;
        }
        #endregion
    }
}