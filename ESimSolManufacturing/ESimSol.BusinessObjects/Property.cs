using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;


namespace ESimSol.BusinessObjects
{
    
    #region Property
    
    public class Property : BusinessObject
    {
        public Property()
        {
          
            PropertyID = 0;
            PropertyName = "";
            Note = "";
            IsMandatory = false;
            ValueSelectStatus = "0 Item Select";
            SelectedPropertyValuesID = "";
        }

        #region Properties
        
        public int PropertyID { get; set; }
        
        public string PropertyName { get; set; }
        
        public string Note { get; set; }
        
        public bool IsMandatory { get; set; }  
        
        public string ProductCategoryName { get; set; }
        
        public int ProductCategoryID { get; set; }
        public List<Property> Properties {get; set;}
        #endregion

        #region Derived Property
        
        public string ErrorMessage { get; set; }
        
        public string ProductCategoryIDs { get; set; }
        public List<ProductCategory> ProductCategorys { get; set; }
        public List<PropertyValue> PropertyValues { get; set; }
        public string ValueSelectStatus { get; set; }
        public string SelectedPropertyValuesID { get; set; }
        #endregion




        #region Functions

        public static List<Property> Gets(long nUserID)
        {
            return Property.Service.Gets(nUserID);
        }

        public Property Get(int id, long nUserID)
        {
            return Property.Service.Get(id, nUserID);
        }

        public Property Save(long nUserID)
        {
            return Property.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return Property.Service.Delete(id, nUserID);
        }

        public static List<Property> Gets(string sSQL, long nUserID)
        {
            return Property.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPropertyService Service
        {
            get { return (IPropertyService)Services.Factory.CreateService(typeof(IPropertyService)); }
        }
        #endregion
       
    }
    #endregion

    #region Propertys
    public class Propertys : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(Property item)
        {
            base.AddItem(item);
        }
        public void Remove(Property item)
        {
            base.RemoveItem(item);
        }
        public Property this[int index]
        {
            get { return (Property)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IProperty interface
    
    public interface IPropertyService
    {
        
        Property Get(int id, Int64 nUserID);
        
        List<Property> Gets(Int64 nUserID);
        
        string Delete(int id, Int64 nUserID);
        
        Property Save(Property oProperty, Int64 nUserID);
        
        List<Property> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}