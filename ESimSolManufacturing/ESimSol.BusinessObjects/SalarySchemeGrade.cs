using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region SalarySchemeGrade

    public class SalarySchemeGrade : BusinessObject
    {
        public SalarySchemeGrade()
        {
            SSGradeID=0;
            Name="";
            ParentID=0;
            Note="";
            MinAmount=0;
            MaxAmount=0;
            IsLastLayer=false;
            IsActive = true;
            SalarySchemeID = 0;
            ErrorMessage = "";
            Params = "";
        }

        #region Properties

        public int SSGradeID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string Note { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public bool IsLastLayer { get; set; }
        public bool IsActive { get; set; }
        public int SalarySchemeID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }



        #endregion

        #region Derived Property
        public IEnumerable<SalarySchemeGrade> children { get; set; }
        public string ActivityInStr { get { if (this.IsActive) return "Active"; else return "Inactive"; } }
        public string NameWithAmount 
        { 
            get 
            { 
                return this.Name+"("+ Global.MillionFormat(this.MinAmount) +" - " + Global.MillionFormat(this.MaxAmount) +")";
            }
        }

        #endregion

        #region Functions

        public static SalarySchemeGrade Get(int nSSGradeID, long nUserID)
        {
            return SalarySchemeGrade.Service.Get(nSSGradeID, nUserID);
        }
        public static List<SalarySchemeGrade> Gets(string sSQL, long nUserID)
        {
            return SalarySchemeGrade.Service.Gets(sSQL, nUserID);
        }
        public SalarySchemeGrade IUD(int nDBOperation, long nUserID)
        {
            return SalarySchemeGrade.Service.IUD(this, nDBOperation, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ISalarySchemeGradeService Service
        {
            get { return (ISalarySchemeGradeService)Services.Factory.CreateService(typeof(ISalarySchemeGradeService)); }
        }

        #endregion
    }
    #endregion

    #region ISalarySchemeGrade interface

    public interface ISalarySchemeGradeService
    {

        SalarySchemeGrade Get(int nSSGradeID, Int64 nUserID);
        List<SalarySchemeGrade> Gets(string sSQL, Int64 nUserID);
        SalarySchemeGrade IUD(SalarySchemeGrade oSalarySchemeGrade, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
