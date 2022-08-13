using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region Country

    public class Country : BusinessObject
    {
        public Country()
        {
            CountryID = 0;
            Code = "";
            Name = "";
            ShortName = "";
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int CountryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Note { get; set; }

        #region Derived Property
        public string CountryWithCodeShortName
        {
            get
            {
                return '[' + this.Code + ']' + this.Name + '(' + this.ShortName + ')';
            }
        }
        public string ErrorMessage { get; set; }
      
        #endregion

        #endregion

        #region Functions
        public static List<Country> Gets(long nUserID)
        {
            return Country.Service.Gets(nUserID);
        }
        public static List<Country> Gets(int nBUID, long nUserID)
        {
            return Country.Service.Gets(nBUID, nUserID);
        }
        public static List<Country> Gets(string sSQL, long nUserID)
        {
            return Country.Service.Gets(sSQL, nUserID);
        }
        public Country Get(int id, long nUserID)
        {
            return Country.Service.Get(id, nUserID);
        }
        public Country GetByType(int nCountryType, long nUserID)
        {
            return Country.Service.GetByType(nCountryType, nUserID);
        }

        public Country Save(long nUserID)
        {
            return Country.Service.Save(this, nUserID);
        }
      
        public string Delete(long nUserID)
        {
            return Country.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICountryService Service
        {
            get { return (ICountryService)Services.Factory.CreateService(typeof(ICountryService)); }
        }
        #endregion
    }
    #endregion


    #region ICountry interface

    public interface ICountryService
    {
        Country Get(int id, Int64 nUserID);
        Country GetByType(int nCountryType, Int64 nUserID);
        List<Country> Gets(string sSQL, long nUserID);
        List<Country> Gets(Int64 nUserID);
        List<Country> Gets(int nBUID, Int64 nUserID);
        string Delete(Country oCountry, Int64 nUserID);
        Country Save(Country oCountry, Int64 nUserID);
    }
    #endregion
}
