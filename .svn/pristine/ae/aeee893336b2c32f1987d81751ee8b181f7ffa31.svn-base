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
    #region TextileSubUnitMachine

    public class TextileSubUnitMachine : BusinessObject
    {
        public TextileSubUnitMachine()
        {
            TSUMachineID = 0;
            TSUID = 0;
            FMID = 0;
            ActiveDate = DateTime.Now;
            Note = "";
            InactiveBy = 0;
            InactiveDate = DateTime.Now;
            InactiveByName = "";
            ErrorMessage = "";
        }

        #region Properties

        public int TSUMachineID { get; set; }
        public int TSUID { get; set; }
        public int FMID { get; set; }
        public DateTime ActiveDate { get; set; }
        public string Note { get; set; }
        public int InactiveBy { get; set; }
        public DateTime InactiveDate { get; set; }
        public string InactiveByName { get; set; }
        public string ErrorMessage { get; set; }

        public TextileSubUnit oTextileSubUnit{ get; set; }

        #endregion

        #region Derived Property
        public string InactiveDateInString
        {
            get { return this.InactiveDate.ToString("dd MMM yyyy"); }
        }
        #endregion


        #region Functions
        public static List<TextileSubUnitMachine> Gets(Int64 nUserID)
        {
            return TextileSubUnitMachine.Service.Gets(nUserID);
        }
        public static List<TextileSubUnitMachine> Gets(string sSQL, Int64 nUserID)
        {
            return TextileSubUnitMachine.Service.Gets(sSQL, nUserID);
        }
        public static TextileSubUnitMachine Get(int nId, Int64 nUserID)
        {
            return TextileSubUnitMachine.Service.Get(nId, nUserID);
        }

        public TextileSubUnitMachine Save(int nDBOperation, Int64 nUserID)
        {
            return TextileSubUnitMachine.Service.Save(this,nDBOperation, nUserID);
        }

        public string Delete(int nId, Int64 nUserID)
        {
            return TextileSubUnitMachine.Service.Delete(nId, nUserID);
        }

        public static List<TextileSubUnitMachine> AssignMachineToTextileUnit(List<TextileSubUnitMachine> oTSUMs, Int64 nUserID)
        {
            return TextileSubUnitMachine.Service.AssignMachineToTextileUnit(oTSUMs, nUserID);
        }
        

        #endregion

        #region ServiceFactory
        internal static ITextileSubUnitMachineService Service
        {
            get { return (ITextileSubUnitMachineService)Services.Factory.CreateService(typeof(ITextileSubUnitMachineService)); }
        }
        #endregion
    }
    #endregion

    #region ITextileSubUnitMachine interface

    public interface ITextileSubUnitMachineService
    {
        TextileSubUnitMachine Get(int id, Int64 nUserID);
        List<TextileSubUnitMachine> Gets(Int64 nUserID);
        List<TextileSubUnitMachine> Gets(string sSQL, Int64 nUserID);  
        string Delete(int id, Int64 nUserID);
        TextileSubUnitMachine Save(TextileSubUnitMachine oTextileSubUnitMachine, int nDBOperation, Int64 nUserID);

       List<TextileSubUnitMachine> AssignMachineToTextileUnit(List<TextileSubUnitMachine> oTSUMs, Int64 nUserID);

    }
    #endregion
}


