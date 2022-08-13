using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region PropertyValue
    
    public class PropertyValue : BusinessObject
    {
        public PropertyValue()
        {
            PropertyValueID = 0;
            PropertyType = EnumPropertyType.None;
            PropertyTypeInInt = 0;
            Remarks = "";
            ValueOfProperty = "";      
        }

        #region Properties
        
        public int PropertyValueID { get; set; }
        public int PropertyTypeInInt { get; set; } 
        public EnumPropertyType PropertyType { get; set; }
        
        public string Remarks { get; set; }
        
        public string ValueOfProperty { get; set; }
        
        #endregion

        #region Derived Property
        public List<PropertyValue> PropertyValuesForSelectedPropertry { get; set; }
        public string PropertyTypeInString 
        { 
            get
            {
                return EnumObject.jGet(this.PropertyType);
            }
        }
        
        public string ErrorMessage { get; set; }
     
        #endregion


        #region Functions

        public static List<PropertyValue> Gets(long nUserID)
        {
            return PropertyValue.Service.Gets(nUserID);
        }
        public static List<PropertyValue> GetsByProperty(int nPropertyID, long nUserID)
        {
            return PropertyValue.Service.GetsByProperty(nPropertyID, nUserID);
        }

        public static List<PropertyValue> GetsByPropertyValue(string sTemp, int nPropertyID, long nUserID)
        {
            return PropertyValue.Service.GetsByPropertyValue(sTemp, nPropertyID, nUserID);
        }

        public PropertyValue Get(int id, long nUserID)
        {
            return PropertyValue.Service.Get(id, nUserID);
        }

        public PropertyValue Save(long nUserID)
        {
            return PropertyValue.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return PropertyValue.Service.Delete(id, nUserID);
        }


        public static List<PropertyValue> Gets(string sSQL, long nUserID)
        {
            return PropertyValue.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IPropertyValueService Service
        {
            get { return (IPropertyValueService)Services.Factory.CreateService(typeof(IPropertyValueService)); }
        }
        #endregion
                
    }
    #endregion

    #region PropertyValues
    public class PropertyValues : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(PropertyValue item)
        {
            base.AddItem(item);
        }
        public void Remove(PropertyValue item)
        {
            base.RemoveItem(item);
        }
        public PropertyValue this[int index]
        {
            get { return (PropertyValue)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IPropertyValue interface
    
    public interface IPropertyValueService
    {
        
        PropertyValue Get(int id, Int64 nUserID);
        
        List<PropertyValue> Gets(Int64 nUserID);
        
        List<PropertyValue>GetsByProperty(int nPropertyID, Int64 nUserID);
        
        List<PropertyValue> GetsByPropertyValue(string sTemp, int nPropertyID, Int64 nUserID);
        
        string Delete(int id, Int64 nUserID);
        
        PropertyValue Save(PropertyValue oPropertyValue, Int64 nUserID);
        
        List<PropertyValue> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}