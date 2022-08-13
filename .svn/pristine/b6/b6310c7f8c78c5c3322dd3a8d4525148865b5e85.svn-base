using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region TextileSubUnit
    
    public class TextileSubUnit : BusinessObject
    {
        public TextileSubUnit()
        {
            TSUID = 0;
            TextileUnit = EnumTextileUnit.None;
            Name = "";
            Note = "";
            InactiveBy = 0;
            InactiveDate = DateTime.Now;
            InactiveByName = "";
            ErrorMessage = "";
            Params = "";
        }

        #region Properties

        public int TSUID { get; set; }
        public EnumTextileUnit TextileUnit { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int InactiveBy { get; set; }
        public DateTime InactiveDate { get; set; }
        public string InactiveByName { get; set; }
        public string ErrorMessage { get; set; }
        public List<TextileSubUnitMachine> TextileSubUnitMachines {get;set;}

        public string Params { get; set; }
        #endregion

        public string TextileUnitStr { get { return (this.TextileUnit==EnumTextileUnit.None) ? "" : this.TextileUnit.ToString(); } }
        public string InactiveDateInString
        {
            get { return  (this.InactiveDate == DateTime.MinValue) ? "-" : this.InactiveDate.ToString("dd MMM yyyy"); }
        }
        #endregion


        #region Functions
        public static List<TextileSubUnit> Gets(Int64 nUserID)
        {
            return TextileSubUnit.Service.Gets(nUserID);
        }
        public static List<TextileSubUnit> Gets(string sSQL, Int64 nUserID)
        {
            return TextileSubUnit.Service.Gets(sSQL, nUserID);
        }
        public TextileSubUnit Get(int nId, Int64 nUserID)
        {
            return TextileSubUnit.Service.Get(nId,nUserID);
        }

        public TextileSubUnit Save(int nDBOperation,Int64 nUserID)
        {
            return TextileSubUnit.Service.Save(this, nDBOperation,nUserID);
        }

        public string Delete(int nId, Int64 nUserID)
        {
            return TextileSubUnit.Service.Delete(nId,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITextileSubUnitService Service
        {
            get { return (ITextileSubUnitService)Services.Factory.CreateService(typeof(ITextileSubUnitService)); }
        }
        #endregion
    }


    #region ITextileSubUnit interface
    
    public interface ITextileSubUnitService
    {
        TextileSubUnit Get(int id, Int64 nUserID);              
        List<TextileSubUnit> Gets(Int64 nUserID);
        List<TextileSubUnit> Gets(string sSQL,Int64 nUserID);     
        string Delete(int id, Int64 nUserID);        
        TextileSubUnit Save(TextileSubUnit oTextileSubUnit,int nDBOperation, Int64 nUserID);
    }
    #endregion
}


